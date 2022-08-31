Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports System.Globalization
Imports SAPbobsCOM
Imports SCG.SBOFramework

Public Class ComprasCls

#Region "Declaraciones"


    Private m_SBOApplication As SAPbouiCOM.Application

    Private m_cnnSCGTaller As SqlClient.SqlConnection
    'Private m_dstRepuestosxOrden As RepuestosxOrdenDataset
    'Private m_adpRepuestosxOrden As RepuestosxOrdenDataAdapter

    Private m_dstRepuestosxEstado As EstadoxRepuestosDataset
    Private m_adpRepuestosxEstado As RepuestosxEstadoDataAdapter
    Private m_dstRepuestosxEstadoxOrden As New EstadoxRepuestosDataset
    Private m_dstRepuestosProveeduria As RepuestosProveduriaDataset
    Private m_adpRepuestosProveeduria As RepuestosProveeduriaDataAdapter
    Private m_dstRepuestosxOrden As RepuestosxOrdenDataset
    Private m_adpRepuestosxOrden As RepuestosxOrdenDataAdapter
    Private m_strNoOrden As String
    Private m_strNoSerie As Integer
    Private m_strCadenaConexion As String

    Private Const mc_strNoOrdendeTrabajo As String = "U_SCGD_Numero_OT"
    ' Private Const mc_strProcesada As String = "U_Procesad"
    Private Const mc_strTipoSuministro As String = "U_SCGD_TipoSum"
    Private Const mc_strTipoArticulo As String = "U_SCGD_TipoArticulo"
    Private Const mc_strIdSucursal As String = "U_SCGD_idSucursal"
    Private Const mc_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"
    Private Const mc_strNoOt As String = "U_SCGD_NoOT"
    Private Const mc_strID As String = "U_SCGD_ID"

    Private Const mc_strSPSelRepuestosProveeduria As String = "SCGTA_SP_SelTotalTrackingxRepuesto"
    Private Const mc_strNoRepuesto As String = "NoRepuesto"
    Private Const mc_strNoOrden As String = "NoOrden"
    Private Const mc_strArroba As String = "@"
    Private Const mc_strGuion As String = "-"
    Private m_intIdSucursal As Integer

    'variable para verificar si hace Ordenes de compra parciales
    Private strBO_TipoParcial As String = String.Empty

    Public n As NumberFormatInfo

    'varible para validar si el documento base es de una Entrada de Mercancia
    Private Const intDocumentoBaseObjType As Integer = 20



    Friend Const mc_strEntradaDeInventario As String = "SELECT DocEntry " & _
                                                        " FROM [OIGN]" & _
                                                        " Where U_SCGD_Procesad='2'"

    Private Const mc_strTipoDeRequisiscion As String = "U_TipoReq"

    Private Const mc_strDocEntry As String = "DocEntry"

    Private blnUsaOTInternaConfiguracion As Boolean = False

    Private Enum EstadoRepuestos
        Pendiente = 1
        Solicitado
        Recibido
        PendientexDevoluciones

    End Enum

    Public Enum TrabajaConSucursal
        No = 0
        Si = 1
    End Enum

    Private m_oCompany As Company
    Private m_udtTieneSucursal As TrabajaConSucursal
    Private mc_intOrdenDeCompra As Integer = 142
    Private oform As SAPbouiCOM.Form
    
#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal TieneSucursal As TrabajaConSucursal, _
                   ByVal SBO_Application As SAPbouiCOM.Application)

        m_oCompany = ocompany
        m_udtTieneSucursal = TieneSucursal
        m_SBOApplication = SBO_Application
        
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "Propiedades"


    <System.CLSCompliant(False)> _
    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return m_oCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            m_oCompany = value
        End Set
    End Property
    

#End Region

#Region "Metodos"

#Region "General"

    Private Function CreaInstanciasDeObjetosDeTaller(ByVal strIdSucursal As String, _
                                                     ByVal udtTieneSucursal As TrabajaConSucursal, _
                                                     ByVal strNombreBDTaller As String, _
                                                     ByVal strNombreCompania As String) As Boolean
        Try

            Dim strCadenaConexionBDTaller As String = ""

            If Utilitarios.DevuelveCadenaConexionBDTaller(m_SBOApplication,
                                                          strIdSucursal, _
                                                   strCadenaConexionBDTaller) Then

                'm_dstRepuestosxOrden = New RepuestosxOrdenDataset
                'm_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)
                m_dstRepuestosProveeduria = New RepuestosProveduriaDataset
                m_adpRepuestosProveeduria = New RepuestosProveeduriaDataAdapter(strCadenaConexionBDTaller)
                m_adpRepuestosxEstado = New RepuestosxEstadoDataAdapter(strCadenaConexionBDTaller)
                m_dstRepuestosxEstado = New EstadoxRepuestosDataset
                m_strCadenaConexion = strCadenaConexionBDTaller
                Return True
            End If
            Return False

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Function

  Public Function CargaOrdenCompra(ByVal NoOrdenCompra As Integer) As Boolean
        Try
            Dim oOrdenDeCompra As SAPbobsCOM.Documents
            Dim intIndice As Integer
            Dim strResultado As String = ""
            Dim blnTieneDestino As Boolean = False
            Dim blnNoTieneDestino As Boolean = False

            oOrdenDeCompra = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders),  _
                                                                SAPbobsCOM.Documents)
            If oOrdenDeCompra.GetByKey(NoOrdenCompra) Then

                For intIndice = 0 To oOrdenDeCompra.Lines.Count - 1

                    oOrdenDeCompra.Lines.SetCurrentLine(intIndice)
                    strResultado = Utilitarios.EjecutarConsulta("Select (case Isnumeric(TrgetEntry) when 0 then 0 else 1 end ) TieneDestino from POR1 inner join OPOR " & _
                                                                "on OPOR.DocEntry = POR1.DocEntry where DocNum = " & oOrdenDeCompra.DocNum & " and LineNum = " & oOrdenDeCompra.Lines.LineNum, m_oCompany.CompanyDB, m_oCompany.Server)
                    If IsNumeric(strResultado) Then
                        blnTieneDestino = CBool(strResultado)
                    Else
                        blnTieneDestino = False
                    End If
                    strResultado = Utilitarios.EjecutarConsulta("Select case LineStatus when 'C' then Isnull(targetType,0) else 0 end from POR1 inner join OPOR " & _
                                            "on OPOR.DocEntry = POR1.DocEntry where DocNum = " & oOrdenDeCompra.DocNum & " and LineNum = " & oOrdenDeCompra.Lines.LineNum, m_oCompany.CompanyDB, m_oCompany.Server)
                    If strResultado = "-1" Then
                        blnNoTieneDestino = True
                    Else
                        blnNoTieneDestino = False
                    End If

                    ' MessageBox.Show(oOrdenDeCompra.DocumentStatus)
                    If (oOrdenDeCompra.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close Or blnNoTieneDestino) AndAlso Not blnTieneDestino Then
                        If CStr(oOrdenDeCompra.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value) <> "" Then

                            If m_udtTieneSucursal = TrabajaConSucursal.Si Then

                                If Not blnUsaOTInternaConfiguracion Then
                                    Call CreaInstanciasDeObjetosDeTaller(CStr(oOrdenDeCompra.UserFields.Fields.Item(mc_strIdSucursal).Value), _
                                                                     TrabajaConSucursal.Si, _
                                                                     m_oCompany.CompanyDB, _
                                                                     m_oCompany.CompanyName)

                                End If

                            End If
                            If Not blnUsaOTInternaConfiguracion Then
                                'Call ActualizaEstadoRepuesto(oOrdenDeCompra.Lines.ItemCode, _
                                '                         CStr(oOrdenDeCompra.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value), _
                                '                         EstadoRepuestos.Solicitado, _
                                '                         EstadoRepuestos.Pendiente, _
                                '                         CInt(oOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value))

                            End If

                        End If
                    End If

                Next intIndice

                Return True
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Function

    Public Function CargaOfertaCompra(ByVal NoOrdenCompra As Integer) As Boolean
        Try
            Dim oOrdenDeCompra As SAPbobsCOM.Documents
            Dim intIndice As Integer
            Dim strResultado As String = ""
            Dim blnTieneDestino As Boolean = False
            Dim blnNoTieneDestino As Boolean = False

            oOrdenDeCompra = CType(m_oCompany.GetBusinessObject(540000006),  _
                                                                SAPbobsCOM.Documents)
            If oOrdenDeCompra.GetByKey(NoOrdenCompra) Then

                For intIndice = 0 To oOrdenDeCompra.Lines.Count - 1

                    oOrdenDeCompra.Lines.SetCurrentLine(intIndice)
                    strResultado = Utilitarios.EjecutarConsulta("Select (case Isnumeric(TrgetEntry) when 0 then 0 else 1 end ) TieneDestino from PQT1 inner join OPQT " & _
                                                                "on OPQT.DocEntry = PQT1.DocEntry where DocNum = " & oOrdenDeCompra.DocNum & " and LineNum = " & oOrdenDeCompra.Lines.LineNum, m_oCompany.CompanyDB, m_oCompany.Server)
                    If IsNumeric(strResultado) Then
                        blnTieneDestino = CBool(strResultado)
                    Else
                        blnTieneDestino = False
                    End If
                    strResultado = Utilitarios.EjecutarConsulta("Select case LineStatus when 'C' then Isnull(targetType,0) else 0 end from PQT1 inner join OPQT " & _
                                            "on OPQT.DocEntry = PQT1.DocEntry where DocNum = " & oOrdenDeCompra.DocNum & " and LineNum = " & oOrdenDeCompra.Lines.LineNum, m_oCompany.CompanyDB, m_oCompany.Server)
                    If strResultado = "-1" Then
                        blnNoTieneDestino = True
                    Else
                        blnNoTieneDestino = False
                    End If

                    ' MessageBox.Show(oOrdenDeCompra.DocumentStatus)
                    If (oOrdenDeCompra.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close Or blnNoTieneDestino) AndAlso Not blnTieneDestino Then


                        If CStr(oOrdenDeCompra.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value) <> "" Then

                            If m_udtTieneSucursal = TrabajaConSucursal.Si Then

                                If Not blnUsaOTInternaConfiguracion Then
                                    Call CreaInstanciasDeObjetosDeTaller(CStr(oOrdenDeCompra.UserFields.Fields.Item(mc_strIdSucursal).Value), _
                                                                     TrabajaConSucursal.Si, _
                                                                     m_oCompany.CompanyDB, _
                                                                     m_oCompany.CompanyName)

                                End If

                            End If

                            If Not blnUsaOTInternaConfiguracion Then
                                'Call ActualizaEstadoRepuesto(oOrdenDeCompra.Lines.ItemCode, _
                                '                         CStr(oOrdenDeCompra.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value), _
                                '                         EstadoRepuestos.Solicitado, _
                                '                         EstadoRepuestos.Pendiente, _
                                '                         CInt(oOrdenDeCompra.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value))

                            End If

                        End If

                    End If

                Next intIndice

                Return True
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
        End Try
    End Function

    <System.CLSCompliant(False)> _
    Public Shared Function DevuelveEtiquetaDeSerie(ByVal intSeries As Integer, _
                                                    ByVal oCompany As SAPbobsCOM.Company, _
                                                    ByRef strEtiquetadeSeries As String) As Boolean

        Dim strConsultaEtiquetadeSerie As String = "Select SeriesName" & _
                                                       " From NNM1" & _
                                                       " Where Series =" & CStr(intSeries)


        Try

            strEtiquetadeSeries = Utilitarios.EjecutarConsulta(strConsultaEtiquetadeSerie, oCompany.CompanyDB, oCompany.Server)

            Return True

        Catch ex As Exception
            Throw ex
            Return False

        End Try
    End Function

    Public Sub EnviarMensajeOrdenCompra(ByVal p_strNoOrden As String, ByVal p_strNoFactura As String)
        Dim strVisita As String
        Dim m_blnConf_TallerEnSAP As Boolean

        Try
            m_blnConf_TallerEnSAP = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
              
            Dim clsMensajeria As New MensajeriaCls(m_SBOApplication, m_oCompany)
            Dim strSucursal As String = Utilitarios.EjecutarConsulta(String.Format(" Select U_SCGD_idSucursal From OQUT WHERE U_SCGD_Numero_OT = '{0}' ", p_strNoOrden), m_oCompany.CompanyDB, m_oCompany.Server)
            'Envia mensaje al encargado de Taller para avisar de una creación o actualización de la cotización
            If ((p_strNoOrden <> "")) Then

                strVisita = p_strNoOrden.Split("-")(0)
                If Not m_blnConf_TallerEnSAP Then
                    clsMensajeria.CreaMensajeSBO_DMS(My.Resources.Resource.MensajeRepuestoRecibido, p_strNoOrden, 0, MensajeriaCls.RecibeMensaje.EncargadoTaller, 0, strVisita, strSucursal)
                Else
                    clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeRepuestoRecibido, 0, p_strNoOrden, MensajeriaCls.RecibeMensaje.EncargadoTaller, False, oform, "dtConsulta", strSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion).ToString(), False, True)
                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region "Funciones Factura de Acreedores"

    <System.CLSCompliant(False)> _
    Public Function RecorreLineasOrdenDeCompra(ByVal oOrdenCompra As SAPbobsCOM.Documents, _
                                                ByVal oFacturaCompra As SAPbobsCOM.Documents, Optional p_blnConf_TallerEnSAP As Boolean = False) As Boolean

        Dim intIndice As Integer

        Try

            Dim a As Integer = oOrdenCompra.Lines.Count


            For intIndice = 0 To oOrdenCompra.Lines.Count - 1

                oOrdenCompra.Lines.SetCurrentLine(intIndice)

                Call RevisaSiExisteLineaFactura(oFacturaCompra, _
                                                oOrdenCompra.Lines.ItemCode, _
                                                CStr(oOrdenCompra.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value), _
                                                 oOrdenCompra.Lines.LineNum, p_blnConf_TallerEnSAP)
            Next intIndice

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            Return False
        End Try
    End Function

    Private Function RevisaSiExisteLineaFactura(ByVal oFacturaCompra As SAPbobsCOM.Documents, _
                                                ByVal ItemcodeOC As String, _
                                                ByVal NoOrden As String, _
                                                ByVal NoLinea As Integer, Optional p_blnConf_TallerEnSAP As Boolean = False) As Boolean

        Dim intIndice As Integer
        Dim blnActualizaEstado As Boolean = True

        Try

            Dim a As Integer = oFacturaCompra.Lines.Count

            For intIndice = 0 To oFacturaCompra.Lines.Count - 1

                Call oFacturaCompra.Lines.SetCurrentLine(intIndice)

                If oFacturaCompra.Lines.ItemCode = ItemcodeOC _
                AndAlso oFacturaCompra.Lines.ItemCode = NoLinea.ToString Then


                    blnActualizaEstado = False

                End If

            Next intIndice

            If blnActualizaEstado Then

                If Not p_blnConf_TallerEnSAP Then
                    'Call ActualizaEstadoRepuesto(ItemcodeOC, NoOrden, _
                    '                        EstadoRepuestos.Solicitado, _
                    '                        EstadoRepuestos.Pendiente, _
                    '                        NoLinea)
                End If


            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        Finally
        End Try
    End Function

    Private Function ActualizaEstadoRepuesto(ByVal itemcode As String, _
                                             ByVal NoOrden As String, _
                                             ByVal EstadoaBuscar As EstadoRepuestos, _
                                             ByVal EstadoNuevo As EstadoRepuestos, _
                                             ByVal NoLinea As Integer) As Boolean

        Dim drwRepuestosxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow
        Dim drwNewRepuestosxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow

        Try
            Call m_dstRepuestosxEstadoxOrden.Clear()

            If m_adpRepuestosxEstado.Fill(m_dstRepuestosxEstadoxOrden, _
                                          NoOrden, _
                                          itemcode, _
                                          EstadoaBuscar, _
                                          NoLinea) = 1 Then

                drwRepuestosxEstado = DirectCast(m_dstRepuestosxEstadoxOrden.SCGTA_TB_RepuestosxEstado.Rows(0),  _
                                                EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow)

                If Not drwRepuestosxEstado Is Nothing Then

                    drwNewRepuestosxEstado = m_dstRepuestosxEstadoxOrden.SCGTA_TB_RepuestosxEstado.NewSCGTA_TB_RepuestosxEstadoRow
                    drwNewRepuestosxEstado.IdRepuestosxOrden = drwRepuestosxEstado.IdRepuestosxOrden
                    drwNewRepuestosxEstado.CodEstadoRep = EstadoNuevo
                    drwNewRepuestosxEstado.Cantidad = drwRepuestosxEstado.Cantidad

                    Call m_dstRepuestosxEstadoxOrden.SCGTA_TB_RepuestosxEstado.AddSCGTA_TB_RepuestosxEstadoRow(drwNewRepuestosxEstado)

                    Call drwRepuestosxEstado.Delete()

                    Call m_adpRepuestosxEstado.Update(m_dstRepuestosxEstadoxOrden)

                End If

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(ex.Message)
        Finally

        End Try

    End Function

    Private Function InteraccionConSCGTaller(ByVal oLineaDocumentoMarketing As SAPbobsCOM.Document_Lines, _
                                             ByRef oOrdenDeCompra As SAPbobsCOM.Documents, _
                                             ByVal FechaFactura As Date, _
                                             ByVal NoFactura As Integer, _
                                             ByVal SerieFactura As Integer, _
                                             ByRef NoOrdenTrabajo As String, _
                                             ByVal dtbEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoDataTable, Optional p_blnUsaTallerInterno As Boolean = False) As Boolean

        'Dim strNoOrdenDeTrabajo As String
        'Dim strNoOrdenDeCompra As String
        Dim strNoRepuesto As String
        'Dim LineasOrdenDeCompra As SAPbobsCOM.Document_Lines
        'Dim oOrdenDeCompra As SAPbobsCOM.Documents
        Dim drwRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
        Dim drwRepuestosxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow
        Dim intCantidadDeRegistrosenTracking As Integer
        Dim decCantidadPendiente As Decimal
        Dim intIdRepuestoxOrden As Integer
        Dim decCantidadEntregada As Decimal
        'Dim intUltimaFilaRepuestosProveeduria As Integer

        Try

            strBO_TipoParcial = Utilitarios.EjecutarConsulta("select U_BO_Parc from dbo.[@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)

            If Not oLineaDocumentoMarketing Is Nothing Then

                If Not CObj(oLineaDocumentoMarketing.BaseEntry) Is System.Convert.DBNull _
                  AndAlso oLineaDocumentoMarketing.BaseEntry > 0 Then

                    If Not oOrdenDeCompra Is Nothing Then

                        'MsgBox("La orden de compra para la factura: " & NoFactura & " ha sido cargada")

                        strNoRepuesto = oLineaDocumentoMarketing.ItemCode
                        intIdRepuestoxOrden = oLineaDocumentoMarketing.UserFields.Fields.Item(mc_strIdRepxOrd).Value

                        Call m_dstRepuestosProveeduria.Clear()
                        Call m_dstRepuestosxEstado.Clear()




                        If m_adpRepuestosxEstado.Fill(m_dstRepuestosxEstado, _
                                                        "", _
                                                        strNoRepuesto, _
                                                        EstadoRepuestos.Solicitado, _
                                                        intIdRepuestoxOrden) = 1 Then

                            'MsgBox("Se cargo el repuesto " & intNoRepuesto & " de la orden" & NoOrdenTrabajo)

                            drwRepuestosxEstado = CType(m_dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado(0),  _
                                                        EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow)
                            '********************************************
                            'Revisarse
                            '*********************************************
                            If ModificaRepuestoxEstado(oLineaDocumentoMarketing, _
                                                        decCantidadPendiente, _
                                                        drwRepuestosxEstado, _
                                                        dtbEstadoxRepuestoxOrden, _
                                                        decCantidadEntregada) Then

                                'MsgBox("El repuesto " & intNoRepuesto & " ha sido actualizado en el dataset")

                                'Call m_adpRepuestosxOrden.Update(m_dstRepuestosxOrden, True)

                                Call m_adpRepuestosxEstado.Update(m_dstRepuestosxEstadoxOrden)
                                'MsgBox("El repuesto " & intNoRepuesto & " ha sido actualizado en la Base de Datos")

                                Call AsignarCostoALinea(oLineaDocumentoMarketing.UserFields.Fields.Item(mc_strIdRepxOrd).Value, (oLineaDocumentoMarketing.LineTotal), NoOrdenTrabajo)
                                If m_adpRepuestosProveeduria.Fill(m_dstRepuestosProveeduria, NoOrdenTrabajo, strNoRepuesto, intIdRepuestoxOrden) > 0 _
                                Then
                                    'MsgBox("Se cargo el ultimo tracking del repuesto " & intNoRepuesto & " de la orden" & NoOrdenTrabajo)

                                    'intUltimaFilaRepuestosProveeduria = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Rows.Count - 1

                                    drwRepuestosProveeduria = CType(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0),  _
                                                                    RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow)

                                    Call DevuelveCantidadDeTracks(strNoRepuesto, NoOrdenTrabajo, intCantidadDeRegistrosenTracking)

                                    If intCantidadDeRegistrosenTracking >= 1 Then

                                        If (Not drwRepuestosProveeduria.IsFechaCompromisoNull _
                                            AndAlso FechaFactura = drwRepuestosProveeduria.FechaCompromiso) _
                                            Or drwRepuestosProveeduria.IsFechaCompromisoNull Then

                                            If ModificaLineaDeTracking(oLineaDocumentoMarketing, drwRepuestosProveeduria, FechaFactura, _
                                                                       oOrdenDeCompra.DocNum, oOrdenDeCompra.Series, SerieFactura, NoFactura, decCantidadEntregada, NoOrdenTrabajo) Then
                                                'MsgBox("Se actualizo la ultima linea de tracking del repuesto: " & intNoRepuesto)

                                                Call m_adpRepuestosProveeduria.Update(m_dstRepuestosProveeduria)

                                                'MsgBox("Se actualizo la ultima linea de tracking del repuesto: " & intNoRepuesto & " en la Base de Datos")

                                                Return True
                                            Else

                                                'MsgBox("No se actualiza la ultima linea de tracking del repuesto: " & intNoRepuesto & " en la Base de Datos")
                                                Return False
                                            End If

                                        ElseIf Not drwRepuestosProveeduria.IsFechaCompromisoNull _
                                                AndAlso (FechaFactura > drwRepuestosProveeduria.FechaCompromiso _
                                                         Or FechaFactura < drwRepuestosProveeduria.FechaCompromiso) Then   ' drwRepuestosProveeduria.FechaCompromiso()

                                            If CreaNuevaLineaDeTracking(oLineaDocumentoMarketing, m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                                                                     drwRepuestosProveeduria, FechaFactura, oOrdenDeCompra.DocNum, NoFactura, decCantidadEntregada) Then

                                                Call m_adpRepuestosProveeduria.Update(m_dstRepuestosProveeduria)
                                                Return True

                                            Else

                                                Return False

                                            End If

                                        End If 'drwRepuestosProveeduria.FechaCompromiso

                                    End If 'CantidadDeRegistrosenTracking

                                Else
                                    'MsgBox("No se pudo cargar el ultimo tracking del repuesto " & intNoRepuesto & " de la orden" & NoOrdenTrabajo)
                                    Return False
                                End If 'adpRepuestosProveeduria

                            Else

                                'MsgBox("El repuesto " & intNoRepuesto & " no ha sido actualizado en el dataset")
                                Return False
                            End If 'ModificaRepuestoxOrden

                        Else

                            'MsgBox("No se cargo el repuesto " & intNoRepuesto & " de la orden: " & NoOrdenTrabajo)
                            Return False
                        End If 'm_adpRepuestosxEstado


                    Else 'Borrar else
                        'MsgBox("La orden de compra para la factura: " & NoFactura & " no ha sido cargada")
                        Return False
                    End If 'oOrdenDeCompra

                End If 'If Not CObj(p_oLineasFacturaCompra.BaseEntry) Is System.Convert.DBNull AndAlso Not CObj(strNoOrdenDeTrabajo) Is System.Convert.DBNull Then

            Else 'p_oLineasFacturaCompra
                Return False

            End If 'p_oLineasFacturaCompra

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'Call MsgBox(ex.Message)
        Finally
        End Try
    End Function

    Private Function InteraccionConTallerInterno(ByVal oLineaDocumentoMarketing As SAPbobsCOM.Document_Lines, _
                                             ByRef oOrdenDeCompra As SAPbobsCOM.Documents, _
                                             ByVal FechaFactura As Date, _
                                             ByVal NoFactura As Integer, _
                                             ByVal SerieFactura As Integer, _
                                             ByRef NoOrdenTrabajo As String) As Boolean


        Dim strNoRepuesto As String
        Dim decCantidadPendiente As Decimal
        Dim strID As String
        Dim decCantidadEntregada As Decimal
        'Dim intUltimaFilaRepuestosProveeduria As Integer

        Try

            strBO_TipoParcial = Utilitarios.EjecutarConsulta("select U_BO_Parc from dbo.[@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)

            If Not oLineaDocumentoMarketing Is Nothing Then

                If Not CObj(oLineaDocumentoMarketing.BaseEntry) Is System.Convert.DBNull _
                  AndAlso oLineaDocumentoMarketing.BaseEntry > 0 Then

                    If Not oOrdenDeCompra Is Nothing Then

                        strNoRepuesto = oLineaDocumentoMarketing.ItemCode
                        strID = oLineaDocumentoMarketing.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim

                        Call AsignarCostoALinea(0, (oLineaDocumentoMarketing.LineTotal), NoOrdenTrabajo, strID, True)
                        If Not String.IsNullOrEmpty(strID) Then
                            Utilitarios.ActualizarLineaTrackinginterno(oLineaDocumentoMarketing, oOrdenDeCompra, FechaFactura, NoFactura, SerieFactura, NoOrdenTrabajo, m_oCompany, False, False)
                        End If
                        Return (True)
                    Else
                        Return False
                    End If

                End If

            Else
                Return False

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'Call MsgBox(ex.Message)
        Finally
        End Try
    End Function


    Private Function AsignarCostoALinea(ByVal p_intIDItem As Integer, ByVal p_decMonto As Double, ByVal p_strNumeroOT As String, Optional p_LineaID As String = "", _
                                        Optional p_blnUsaTallerInterno As Boolean = False) As Boolean

        Dim objCotizacion As SAPbobsCOM.Documents
        Dim objCotizacionLineas As SAPbobsCOM.Document_Lines
        Dim strNumeroLinea As String
        Dim strCotizacion As String
        Dim strNombreBDTAller As String = ""
        Dim strSucursal As String = Utilitarios.EjecutarConsulta(String.Format(" Select U_SCGD_idSucursal From OQUT WHERE U_SCGD_Numero_OT = '{0}' ", p_strNumeroOT), m_oCompany.CompanyDB, m_oCompany.Server)
        Call Utilitarios.DevuelveNombreBDTaller(m_SBOApplication, strSucursal, strNombreBDTAller)

        If Not p_blnUsaTallerInterno Then
            strCotizacion = Utilitarios.EjecutarConsulta("Select NoCotizacion from dbo.SCGTA_TB_RepuestosxOrden inner join dbo.SCGTA_TB_Orden on SCGTA_TB_Orden.NoOrden = SCGTA_TB_RepuestosxOrden.NoOrden where ID = " & p_intIDItem, strNombreBDTAller, m_oCompany.Server)
            strCotizacion = strCotizacion.Trim

            If Not String.IsNullOrEmpty(strCotizacion) Then

                strNumeroLinea = Utilitarios.EjecutarConsulta("Select ID from dbo.SCGTA_TB_RepuestosxOrden inner join dbo.SCGTA_TB_Orden on SCGTA_TB_Orden.NoOrden = SCGTA_TB_RepuestosxOrden.NoOrden where ID = " & p_intIDItem, strNombreBDTAller, m_oCompany.Server)
                If Not String.IsNullOrEmpty(strNumeroLinea) Then
                    objCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                    objCotizacion.GetByKey(strCotizacion)

                    objCotizacionLineas = objCotizacion.Lines

                    For i As Integer = 0 To objCotizacionLineas.Count - 1
                        objCotizacion.Lines.SetCurrentLine(i)

                        If objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = strNumeroLinea Then
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = p_decMonto
                        End If
                    Next
                    objCotizacion.Update()
                End If
            End If
            If Not objCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objCotizacion)
                objCotizacion = Nothing
            End If

        Else

            strCotizacion = Utilitarios.EjecutarConsulta("Select DocEntry From dbo.[OQUT] where U_SCGD_Numero_OT = '" & p_strNumeroOT & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            strCotizacion = strCotizacion.Trim

            If Not String.IsNullOrEmpty(strCotizacion) Then

                objCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                objCotizacion.GetByKey(strCotizacion)

                objCotizacionLineas = objCotizacion.Lines

                For i As Integer = 0 To objCotizacionLineas.Count - 1
                    objCotizacion.Lines.SetCurrentLine(i)


                    If objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = p_LineaID Then
                        objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = p_decMonto
                    End If

                Next

                objCotizacion.Update()

            End If
            If Not objCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objCotizacion)
                objCotizacion = Nothing
            End If

        End If



    End Function

    Private Overloads Function ModificaRepuestoxEstado(ByVal p_oLineasFacturaCompra As SAPbobsCOM.Document_Lines, _
                                                       ByRef CantidadPendiente As Decimal, _
                                                       ByRef drwOldRepuestoxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow, _
                                                       ByRef dtbEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoDataTable, _
                                                       ByRef decCantidadItems As Decimal, Optional ByVal blnTipoParcial As Boolean = False) As Boolean

        Dim drwEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow = Nothing

        Try

            'cambiar condicion 


            If p_oLineasFacturaCompra.Quantity > drwOldRepuestoxEstado.Cantidad Then

                decCantidadItems = drwOldRepuestoxEstado.Cantidad
            Else

                decCantidadItems = p_oLineasFacturaCompra.Quantity

            End If


            If decCantidadItems <= drwOldRepuestoxEstado.Cantidad _
                AndAlso decCantidadItems > 0 Then

                Call ManipulaEstadosDeRepuesto(drwOldRepuestoxEstado, drwEstadoxRepuestoxOrden, _
                                               dtbEstadoxRepuestoxOrden, decCantidadItems, _
                                               EstadoRepuestos.Recibido)

                CantidadPendiente = drwOldRepuestoxEstado.Cantidad - decCantidadItems

                '***************************************
                'se agrega condicion para entradas parciales
                If CantidadPendiente > 0 Then

                    If strBO_TipoParcial = "Y" Then

                        Call ManipulaEstadosDeRepuesto(drwOldRepuestoxEstado, drwEstadoxRepuestoxOrden, _
                                                  dtbEstadoxRepuestoxOrden, CantidadPendiente, _
                                                   EstadoRepuestos.Solicitado)

                    Else

                        Call ManipulaEstadosDeRepuesto(drwOldRepuestoxEstado, drwEstadoxRepuestoxOrden, _
                                                      dtbEstadoxRepuestoxOrden, CantidadPendiente, _
                                                       EstadoRepuestos.Pendiente)

                        drwEstadoxRepuestoxOrden = dtbEstadoxRepuestoxOrden.FindByIdRepuestosxOrdenCodEstadoRep(drwOldRepuestoxEstado.IdRepuestosxOrden, _
                                                                                      EstadoRepuestos.Solicitado)

                        If Not drwEstadoxRepuestoxOrden Is Nothing Then '2

                            Call drwEstadoxRepuestoxOrden.Delete()

                        End If

                    End If

                Else

                    drwEstadoxRepuestoxOrden = dtbEstadoxRepuestoxOrden.FindByIdRepuestosxOrdenCodEstadoRep(drwOldRepuestoxEstado.IdRepuestosxOrden, _
                                                                                    EstadoRepuestos.Solicitado)
                    If Not drwEstadoxRepuestoxOrden Is Nothing Then

                        Call drwEstadoxRepuestoxOrden.Delete()

                    End If

                End If

            End If

            Return True

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(ex.Message)
            Return False
        Finally

        End Try

    End Function

    Private Function ManipulaEstadosDeRepuesto(ByVal drwOldRepuestoxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow, _
                                               ByVal drwEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow, _
                                               ByRef dtbEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoDataTable, _
                                               ByVal Cantidad As Decimal, _
                                               ByVal oEstadoRepuesto As EstadoRepuestos) As Boolean

        Try



            drwEstadoxRepuestoxOrden = dtbEstadoxRepuestoxOrden.FindByIdRepuestosxOrdenCodEstadoRep(drwOldRepuestoxEstado.IdRepuestosxOrden, _
                                                                                                    oEstadoRepuesto)
            If Not drwEstadoxRepuestoxOrden Is Nothing Then

                If strBO_TipoParcial = "Y" Then

                    Select Case oEstadoRepuesto

                        Case EstadoRepuestos.Pendiente

                            drwEstadoxRepuestoxOrden.Cantidad += Cantidad

                        Case EstadoRepuestos.Recibido
                            drwEstadoxRepuestoxOrden.Cantidad += Cantidad

                        Case EstadoRepuestos.Solicitado

                            drwEstadoxRepuestoxOrden.Cantidad = Cantidad

                    End Select

                Else

                    drwEstadoxRepuestoxOrden.Cantidad += Cantidad

                End If


            Else 'drwEstadoxRepuestoxOrden

                drwEstadoxRepuestoxOrden = dtbEstadoxRepuestoxOrden.NewSCGTA_TB_RepuestosxEstadoRow

                drwEstadoxRepuestoxOrden.IdRepuestosxOrden = drwOldRepuestoxEstado.IdRepuestosxOrden

                drwEstadoxRepuestoxOrden.CodEstadoRep = oEstadoRepuesto

                drwEstadoxRepuestoxOrden.Cantidad = Cantidad

                Call dtbEstadoxRepuestoxOrden.AddSCGTA_TB_RepuestosxEstadoRow(drwEstadoxRepuestoxOrden)

            End If 'drwEstadoxRepuestoxOrden

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) ' MsgBox(ex.Message)
            Return False
        End Try
    End Function

    Private Function ActaulizaRepuestoxEstado(ByVal CodEstado As Integer, _
                                              ByVal idRepuestoxOrden As Integer, _
                                              ByVal dtbEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoDataTable) As Boolean
        Try

            Dim drwEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow

            drwEstadoxRepuestoxOrden = dtbEstadoxRepuestoxOrden.FindByIdRepuestosxOrdenCodEstadoRep(idRepuestoxOrden, CodEstado)



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        End Try

    End Function


    Private Function ModificaLineaDeTracking(ByVal p_oLineasFacturaCompra As SAPbobsCOM.Document_Lines, _
                                             ByRef drwOldRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow, _
                                             ByVal FechaFactura As Date, _
                                             ByVal DocNum As Integer, _
                                             ByVal Series As Integer, _
                                             ByVal SeriesFactura As Integer, _
                                             ByVal NoFactura As String, _
                                             ByVal decCantidad As Decimal, _
                                             ByVal p_strNoOrden As String) As Boolean

        Dim strEtiquetadeSerie As String = ""

        Try

            With drwOldRepuestosProveduria

                .FechaEntrega = New Date(FechaFactura.Year, FechaFactura.Month, FechaFactura.Day, _
                                         System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second)

                Call DevuelveEtiquetaDeSerie(SeriesFactura, m_oCompany, strEtiquetadeSerie)

                .NoFactura = strEtiquetadeSerie & mc_strGuion & NoFactura

                Call DevuelveEtiquetaDeSerie(Series, m_oCompany, strEtiquetadeSerie)

                .NoOrdendeCompra = strEtiquetadeSerie & mc_strGuion & DocNum

                .CantSuministrados = decCantidad

                .CostoRepuesto = CDec(decCantidad * p_oLineasFacturaCompra.Price)

                .PrecioCompraReal = CDec(.CostoRepuesto - ((.CostoRepuesto) * (p_oLineasFacturaCompra.DiscountPercent / 100)))

                .Descuento = CDec(p_oLineasFacturaCompra.DiscountPercent)

                .MontoDesc = .CostoRepuesto - .PrecioCompraReal

                .Observaciones = CreaObservacionDeTracking(p_oLineasFacturaCompra, _
                                                           drwOldRepuestosProveduria, _
                                                           .FechaEntrega, drwOldRepuestosProveduria.NoFactura, _
                                                           False, _
                                                           decCantidad)


                EnviarMensajeOrdenCompra(p_strNoOrden, .NoFactura)

            End With

            Return True

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) ' MsgBox(ex.Message)
            Return False
        Finally

        End Try
    End Function

    Private Function CreaNuevaLineaDeTracking(ByVal p_oLineasFacturaCompra As SAPbobsCOM.Document_Lines, _
                                              ByRef dtbRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaDataTable, _
                                              ByVal drwOldRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow, _
                                              ByVal FechaFactura As Date, _
                                              ByVal DocNum As Integer, _
                                              ByVal NoFactura As Integer, _
                                              ByVal decCantidad As Decimal) As Boolean

        Dim drwNewRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
        Dim dtcRepuestosProveduria As Data.DataColumn
        'Dim strObservacion As String

        Try

            drwNewRepuestosProveduria = dtbRepuestosProveeduria.NewSCGTA_TB_RepuestosxOrden_ProveduriaRow

            For Each dtcRepuestosProveduria In dtbRepuestosProveeduria.Columns

                drwNewRepuestosProveduria(dtcRepuestosProveduria.ColumnName) = drwOldRepuestosProveduria(dtcRepuestosProveduria.ColumnName)

            Next dtcRepuestosProveduria


            With drwNewRepuestosProveduria

                .FechaEntrega = New Date(FechaFactura.Year, FechaFactura.Month, FechaFactura.Day, _
                                         System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second)
                .NoFactura = drwOldRepuestosProveduria.NoFactura  'NoFactura

                .NoOrdendeCompra = drwOldRepuestosProveduria.NoOrdendeCompra

                .CantSuministrados = decCantidad

                .CostoRepuesto = CDec(decCantidad * p_oLineasFacturaCompra.Price)

                .PrecioCompraReal = CDec(.CostoRepuesto - ((.CostoRepuesto) * (p_oLineasFacturaCompra.DiscountPercent / 100)))

                .Descuento = CDec(p_oLineasFacturaCompra.DiscountPercent)

                .MontoDesc = .CostoRepuesto - .PrecioCompraReal

                .Observaciones = CreaObservacionDeTracking(p_oLineasFacturaCompra, _
                                                           drwNewRepuestosProveduria, _
                                                           .FechaEntrega, _
                                                           drwNewRepuestosProveduria.NoFactura, _
                                                           False, _
                                                           decCantidad)

            End With

            Call dtbRepuestosProveeduria.AddSCGTA_TB_RepuestosxOrden_ProveduriaRow(drwNewRepuestosProveduria)

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) ' MsgBox(ex.Message)
            Return False
        Finally
        End Try
    End Function

    Private Function CreaObservacionDeTracking(ByVal p_oLineasDetalleDoc As SAPbobsCOM.Document_Lines, _
                                               ByVal drwNewRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow, _
                                               ByVal FechaDoc As Date, _
                                               ByVal NumeroDeDoc As String, _
                                               ByVal blnPendientexDevoluciones As Boolean, _
                                               ByVal decCantidad As Decimal) As String

        Dim strObservacion As String

        Try

            With drwNewRepuestosProveduria

                If blnPendientexDevoluciones Then

                    'strObservacion = "Se devolvieron " & CStr(intCantidad) & " " & p_oLineasDetalleDoc.ItemDescription & _
                    '                 ".La fecha y hora de devolución fue " & FechaDoc & "."

                    'strObservacion = CStr(intCantidad) & " " & p_oLineasDetalleDoc.ItemDescription & _
                    '                 " were returned. The date and hour of restitution was " & FechaDoc & "."

                    strObservacion = My.Resources.Resource.ObservacionTrackingParte1 & CStr(decCantidad) & " " & p_oLineasDetalleDoc.ItemDescription & _
                        My.Resources.Resource.ObservacionTrackingParte2 & FechaDoc & "."

                Else

                    'strObservacion = "Se recibieron " & .CantSuministrados & " " & p_oLineasDetalleDoc.ItemDescription

                    'strObservacion = .CantSuministrados & " " & p_oLineasDetalleDoc.ItemDescription & " were received"

                    strObservacion = My.Resources.Resource.ObservacionTrackingParte3 & _
                        .CantSuministrados & " " & p_oLineasDetalleDoc.ItemDescription & _
                        My.Resources.Resource.ObservacionTrackingParte4

                    If .CantSolicitados > .CantSuministrados Then

                        'strObservacion &= " quedan pendientes de recibir " & CStr(.CantSolicitados - .CantSuministrados)

                        'strObservacion &= " open quantity " & CStr(.CantSolicitados - .CantSuministrados)

                        strObservacion &= My.Resources.Resource.ObservacionTrackingParte5 & CStr(.CantSolicitados - .CantSuministrados)

                    ElseIf .CantSolicitados = .CantSuministrados Then

                        'strObservacion &= " no quedan pendientes"

                        'strObservacion &= " without pending"

                        strObservacion &= My.Resources.Resource.ObservacionTrackingParte6

                    End If

                    If Not .IsFechaCompromisoNull Then


                        'strObservacion &= ".La fecha y hora de entrega fue " & FechaDoc & " y la fecha y hora de compromiso era " & .FechaCompromiso & "."

                        'strObservacion &= ".The date and hour of delivery was " & FechaDoc & " and the date and hour of commitment was " & .FechaCompromiso & "."

                        strObservacion &= My.Resources.Resource.ObservacionTrackingParte7 & FechaDoc & My.Resources.Resource.ObservacionTrackingParte8 & .FechaCompromiso & "."

                    Else

                        'strObservacion &= ".La fecha y hora de entrega fue " & FechaDoc & " y la fecha y hora de compromiso nunca se estableció."

                        'strObservacion &= ".The date and hour of delivery was " & FechaDoc & " and the date and hour of commitment it was not used."

                        strObservacion &= My.Resources.Resource.ObservacionTrackingParte7 & FechaDoc & My.Resources.Resource.ObservacionTrackingParte9

                    End If

                End If

            End With

            Return strObservacion

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            Throw ex
            Return ""

        End Try

    End Function

    Private Function AgregaSalidaDeInventarioOrden(ByVal FacturaDeCompra As SAPbobsCOM.Documents, _
                                                   ByVal NoOrdendeTrabajo As String) As Boolean

        Dim intIndice As Integer

        Try
            Dim objSalidaInvetario As SAPbobsCOM.Documents

            'Almacena el número del error en el caso que exista
            Dim intError As Integer

            'Almacena el mensaje de error que envia SBO en el
            'el caso que exista
            Dim strError As String = ""

            intIndice = 0

            'Obtiene la instancia del objeto de SBO para crear salidas de inventario
            objSalidaInvetario = DirectCast(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit),  _
                                            SAPbobsCOM.Documents)

            'Se indica que el numero que se genera en las salidas de inventario
            'se genera automaticamente y no de forma manual
            objSalidaInvetario.HandWritten = SAPbobsCOM.BoYesNoEnum.tNO

            'Se instancian los UDF del encabezdo de las salidas de inventario en SBO
            With objSalidaInvetario.UserFields.Fields

                .Item(mc_strNoOrdendeTrabajo).Value = NoOrdendeTrabajo
                ' .Item(mc_strTipoDeRequisiscion).Value = CStr(1)

            End With

            objSalidaInvetario.DocDate = System.DateTime.Today

            Call FacturaDeCompra.Lines.SetCurrentLine(0)

            objSalidaInvetario.Lines.ItemCode = FacturaDeCompra.Lines.ItemCode

            objSalidaInvetario.Lines.Quantity = FacturaDeCompra.Lines.Quantity

            ' '' ''Se usa en 3R
            ' '' ''Call ActualizaCostosxOrden(m_strCadenaConexion, _
            ' '' ''                       0, _
            ' '' ''                       CDec(FacturaDeCompra.Lines.LineTotal()), _
            ' '' ''                       0, _
            ' '' ''                       NoOrdendeTrabajo, _
            ' '' ''                       True)

            For intIndice = 1 To FacturaDeCompra.Lines.Count - 1

                'se agregan las lineas de las salidas de inventario
                objSalidaInvetario.Lines.Add()

                Call FacturaDeCompra.Lines.SetCurrentLine(intIndice)

                'Llamada para obtener el itemcode del articulo de SAP a partir de id del producto que se lee del archivo de mixit
                objSalidaInvetario.Lines.ItemCode = FacturaDeCompra.Lines.ItemCode

                objSalidaInvetario.Lines.Quantity = FacturaDeCompra.Lines.Quantity


                'MsgBox(CDec(FacturaDeCompra.Lines.LineTotal()))


                'MsgBox(CDec(FacturaDeCompra.Lines.PriceAfterVAT()))

                ' '' ''Se usa en 3R

                ' '' ''Call ActualizaCostosxOrden(m_strCadenaConexion, _
                ' '' ''                      0, _
                ' '' ''                      CDec(FacturaDeCompra.Lines.LineTotal()), _
                ' '' ''                      0, _
                ' '' ''                      NoOrdendeTrabajo, _
                ' '' ''                      True)


            Next intIndice

            intError = objSalidaInvetario.Add()

            'Call m_oCompany.GetLastError(intError, strError)


            If intError <> 0 Then

                Call m_oCompany.GetLastError(intError, strError)
                m_SBOApplication.StatusBar.SetText(My.Resources.Resource.ErrorCode + intError + ": " + strError, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(strError)

                'If Not System.IO.File.Exists(m_strPathLogProcesoMixit) Then

                '    Call CreaArchivo(m_strPathLogProcesoMixit)

                'End If

                'Call m_oCompany.GetLastError(lErrCode, strError)

                'Dim strMensaje As String = "Fecha: " & CStr(System.DateTime.Now) & vbCrLf & _
                '                          "La salida de invetario para la Orden: " & NoOrden & " de la fecha: " & CStr(Fecha) & "no se ha creado por el siguiente motivo:" & vbCrLf & _
                '                           strError

                'Call escribirArchivo(m_strPathLogProcesoMixit, strMensaje)

                Return False
            Else

                Return True
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) ' MsgBox(ex.Message)
            Return False
        Finally

        End Try

    End Function

    Public Function DevuelveCantidadDeTracks(ByVal NoRepuesto As String, _
                                                         ByVal NoOrdenDetrabajo As String, _
                                                         ByRef CantidadDeRegistrosEnTracking As Integer) As Boolean
        Dim cmdSel As SqlClient.SqlCommand
        'Dim param As SqlClient.SqlParameter
        Dim m_cnnSCGTaller As New SqlClient.SqlConnection

        Try
            m_cnnSCGTaller = New SqlClient.SqlConnection(m_strCadenaConexion)

            cmdSel = New SqlClient.SqlCommand(mc_strSPSelRepuestosProveeduria)
            cmdSel.CommandType = CommandType.StoredProcedure

            cmdSel.Connection = m_cnnSCGTaller

            If Not m_cnnSCGTaller Is Nothing _
                AndAlso m_cnnSCGTaller.State = ConnectionState.Closed Then

                Call m_cnnSCGTaller.Open()

            End If

            With cmdSel.Parameters

                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
                .Add(mc_strArroba & mc_strNoRepuesto, SqlDbType.VarChar, 20)

            End With

            cmdSel.Parameters(mc_strArroba & mc_strNoOrden).Value = NoOrdenDetrabajo

            cmdSel.Parameters(mc_strArroba & mc_strNoRepuesto).Value = NoRepuesto


            CantidadDeRegistrosEnTracking = CInt(cmdSel.ExecuteScalar)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'Call MsgBox(ex.Message)

        Finally
            If Not m_cnnSCGTaller Is Nothing Then
                Call m_cnnSCGTaller.Close()
            End If

        End Try

    End Function


#End Region

#Region "Funciones Nota de Crédito"

    'Public Function RecorreLineasNotaCredito(ByVal oNotaDeCredito As SAPbobsCOM.Documents, _
    '                                          ByVal NoOrden As String) As Boolean
    '    Dim intIndice As Integer

    '    Try
    '        For intIndice = 0 To oNotaDeCredito.Lines.Count - 1

    '            Call oNotaDeCredito.Lines.SetCurrentLine(intIndice)

    '            Call ActualizaCostosxOrden(m_strCadenaConexion, _
    '                                       0, _
    '                                       CDec(oNotaDeCredito.Lines.LineTotal), _
    '                                       0, _
    '                                       NoOrden, _
    '                                       False)

    '        Next intIndice
    '    Catch ex As Exception
    '        m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) ' MsgBox(ex.Message)
    '    End Try
    'End Function

    'Private Function RecorreLineasdeFactura(ByVal oNotaDeCredito As SAPbobsCOM.Documents, _
    '                                        ByVal oFacturaCompra As SAPbobsCOM.Documents) As Boolean

    '    Dim intIndice As Integer

    '    Try

    '        Dim a As Integer = oFacturaCompra.Lines.Count

    '        For intIndice = 0 To oFacturaCompra.Lines.Count - 1

    '            oFacturaCompra.Lines.SetCurrentLine(intIndice)

    '            Call RevisaSiExisteLineaFactura(oFacturaCompra, _
    '                                            oFacturaCompra.Lines.ItemCode, _
    '                                            CStr(oFacturaCompra.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value))

    '        Next intIndice

    '        Return True
    '    Catch ex As Exception

    '        Return False
    '    End Try
    'End Function

    'Private Function RevisaSiExisteLineaNotadeCredito(ByVal oNotaDeCredito As SAPbobsCOM.Documents, _
    '                                                  ByVal ItemcodeOC As String, _
    '                                                  ByVal NoOrden As String) As Boolean

    '    Dim intIndice As Integer
    '    Dim blnActualizaEstado As Boolean = True

    '    Try


    '        Dim a As Integer = oNotaDeCredito.Lines.Count

    '        For intIndice = 0 To oNotaDeCredito.Lines.Count - 1

    '            oNotaDeCredito.Lines.SetCurrentLine(intIndice)

    '            If oNotaDeCredito.Lines.ItemCode = ItemcodeOC Then


    '                blnActualizaEstado = False

    '            End If

    '        Next intIndice

    '        If blnActualizaEstado Then

    '            Call ActualizaEstadoRepuesto(ItemcodeOC, NoOrden, EstadoRepuestos.Recibido, )

    '        End If

    '    Catch ex As Exception

    '    Finally
    '    End Try
    'End Function



    Private Function InteraccionConSCGTallerNotasdeCredito(ByVal p_oLineasDocumentoMarketing As SAPbobsCOM.Document_Lines, _
                                                           ByRef oDocumentoMarketingBase As SAPbobsCOM.Documents, _
                                                           ByVal FechaNotaCredito As Date, _
                                                           ByVal NoNotadeCredito As Integer, _
                                                           ByVal NoOrdenTrabajo As String, _
                                                           ByVal SeriesNotaCredito As Integer, _
                                                           ByVal dtbEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoDataTable) As Boolean

        'Dim strNoOrdenDeTrabajo As String
        'Dim strNoOrdenDeCompra As String
        Dim strNoRepuesto As String
        'Dim LineasOrdenDeCompra As SAPbobsCOM.Document_Lines
        'Dim oOrdenDeCompra As SAPbobsCOM.Documents
        Dim drwRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
        Dim drwRepuestosxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow
        Dim intCantidadDeRegistrosenTracking As Integer
        Dim intCantidadPendiente As Integer
        Dim intCantidadevuelta As Integer
        Dim intIdRepuestoxOrden As Integer
        'Dim intUltimaFilaRepuestosProveeduria As Integer

        Try

            If Not p_oLineasDocumentoMarketing Is Nothing Then


                strNoRepuesto = p_oLineasDocumentoMarketing.ItemCode
                intIdRepuestoxOrden = p_oLineasDocumentoMarketing.UserFields.Fields.Item(mc_strIdRepxOrd).Value


                Call m_dstRepuestosProveeduria.Clear()
                Call m_dstRepuestosxEstado.Clear()


                If m_adpRepuestosxEstado.Fill(m_dstRepuestosxEstado, _
                                             NoOrdenTrabajo, _
                                             strNoRepuesto, _
                                             EstadoRepuestos.Recibido, _
                                             intIdRepuestoxOrden) = 1 Then

                    drwRepuestosxEstado = CType(m_dstRepuestosxEstado.SCGTA_TB_RepuestosxEstado(0),  _
                                              EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow)

                    If ModificaRepuestoxOrden(p_oLineasDocumentoMarketing, _
                                              intCantidadPendiente, _
                                              drwRepuestosxEstado, _
                                              dtbEstadoxRepuestoxOrden, _
                                              intCantidadevuelta) Then

                        'Call m_adpRepuestosxEstado.Update(m_dstRepuestosxEstadoxOrden, True)
                        Call m_adpRepuestosxEstado.Update(m_dstRepuestosxEstadoxOrden)

                        If m_adpRepuestosProveeduria.Fill(m_dstRepuestosProveeduria, NoOrdenTrabajo, strNoRepuesto, intIdRepuestoxOrden) > 0 _
                        Then

                            drwRepuestosProveeduria = CType(m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria(0),  _
                                                            RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow)

                            Call DevuelveCantidadDeTracks(strNoRepuesto, NoOrdenTrabajo, intCantidadDeRegistrosenTracking)

                            'If Not drwRepuestosProveeduria.IsFechaCompromisoNull _
                            '            AndAlso (FechaNotaCredito > drwRepuestosProveeduria.FechaCompromiso _
                            '                     Or FechaNotaCredito < drwRepuestosProveeduria.FechaCompromiso) Then   ' drwRepuestosProveeduria.FechaCompromiso()


                            If CreaNuevaLineaDeTrackingxNotaCredito(p_oLineasDocumentoMarketing, m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria, _
                                                               drwRepuestosProveeduria, FechaNotaCredito, oDocumentoMarketingBase.DocNum, SeriesNotaCredito, NoNotadeCredito, intCantidadevuelta) Then

                                Call m_adpRepuestosProveeduria.Update(m_dstRepuestosProveeduria)
                                Return True

                            Else

                                Return False








                                'End If 'drwRepuestosProveeduria.FechaCompromiso

                            End If 'CantidadDeRegistrosenTracking

                        Else
                            'MsgBox("No se pudo cargar el ultimo tracking del repuesto " & intNoRepuesto & " de la orden" & NoOrdenTrabajo)
                            Return False
                        End If 'adpRepuestosProveeduria

                    Else

                        'MsgBox("El repuesto " & intNoRepuesto & " no ha sido actualizado en el dataset")
                        If Not IsNumeric(strNoRepuesto) Then
                            Return True
                        Else
                            Return False
                        End If

                    End If 'ModificaRepuestoxOrden

                Else

                    'MsgBox("No se cargo el repuesto " & intNoRepuesto & " de la orden: " & NoOrdenTrabajo)
                    Return False
                End If 'm_adpRepuestosxOrden

                'Else 'Borrar else
                '    'MsgBox("La orden de compra para la factura: " & NoFactura & " no ha sido cargada")
                '    Return False
                'End If 'oOrdenDeCompra

            End If



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'Call MsgBox(ex.Message)
        Finally
        End Try
    End Function


    Private Overloads Function ModificaRepuestoxOrden(ByVal p_oLineasFacturaCompra As SAPbobsCOM.Document_Lines, _
                                                      ByRef CantidadPendiente As Integer, _
                                                      ByRef drwOldRepuestoxEstado As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow, _
                                                      ByRef dtbEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoDataTable, _
                                                      ByRef intCantidaddeItemsDevuelta As Integer) As Boolean
        Try

            Dim drwEstadoxRepuestoxOrden As EstadoxRepuestosDataset.SCGTA_TB_RepuestosxEstadoRow

            If p_oLineasFacturaCompra.Quantity > 0 Then


                drwEstadoxRepuestoxOrden = dtbEstadoxRepuestoxOrden.FindByIdRepuestosxOrdenCodEstadoRep(drwOldRepuestoxEstado.IdRepuestosxOrden, _
                                                                                                                          EstadoRepuestos.Recibido)


                If Not drwEstadoxRepuestoxOrden Is Nothing Then '2


                    If p_oLineasFacturaCompra.Quantity > drwEstadoxRepuestoxOrden.Cantidad Then

                        intCantidaddeItemsDevuelta = drwEstadoxRepuestoxOrden.Cantidad

                    Else

                        intCantidaddeItemsDevuelta = p_oLineasFacturaCompra.Quantity

                    End If


                    drwEstadoxRepuestoxOrden.Cantidad -= intCantidaddeItemsDevuelta

                    If drwEstadoxRepuestoxOrden.Cantidad = 0 Then

                        Call drwEstadoxRepuestoxOrden.Delete()

                    End If

                End If 'drwEstadoxRepuestoxOrden2


                Call ManipulaEstadosDeRepuesto(drwOldRepuestoxEstado, drwEstadoxRepuestoxOrden, _
                                                 dtbEstadoxRepuestoxOrden, intCantidaddeItemsDevuelta, _
                                                 EstadoRepuestos.Pendiente)



            End If
            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(ex.Message)
            Return False
        Finally

        End Try

    End Function

    Private Function CreaNuevaLineaDeTrackingxNotaCredito(ByVal p_oLineasFacturaCompra As SAPbobsCOM.Document_Lines, _
                                              ByRef dtbRepuestosProveeduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaDataTable, _
                                              ByVal drwOldRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow, _
                                              ByVal FechaFactura As Date, _
                                              ByVal DocNum As Integer, _
                                              ByVal SeriesNotaCredito As Integer, _
                                              ByVal NoFactura As String, _
                                              ByVal intCantidad As Integer) As Boolean

        Dim drwNewRepuestosProveduria As RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRow
        Dim dtcRepuestosProveduria As Data.DataColumn
        Dim strEtiquetadeSerie As String = ""
        'Dim strObservacion As String

        Try

            drwNewRepuestosProveduria = dtbRepuestosProveeduria.NewSCGTA_TB_RepuestosxOrden_ProveduriaRow

            For Each dtcRepuestosProveduria In dtbRepuestosProveeduria.Columns

                drwNewRepuestosProveduria(dtcRepuestosProveduria.ColumnName) = drwOldRepuestosProveduria(dtcRepuestosProveduria.ColumnName)

            Next dtcRepuestosProveduria

            With drwNewRepuestosProveduria

                .FechaEntrega = New Date(FechaFactura.Year, FechaFactura.Month, FechaFactura.Day, _
                                                System.DateTime.Now.Hour, System.DateTime.Now.Minute, System.DateTime.Now.Second)

                Call DevuelveEtiquetaDeSerie(SeriesNotaCredito, m_oCompany, strEtiquetadeSerie)

                .NoFactura = strEtiquetadeSerie & mc_strGuion & NoFactura

                .NoOrdendeCompra = drwOldRepuestosProveduria.NoOrdendeCompra  'DocNum
                .CantSuministrados = CInt(intCantidad)

                .CostoRepuesto = CDec(p_oLineasFacturaCompra.Price / intCantidad)

                '.PrecioCompraReal = CDec(.CostoRepuesto - ((.CostoRepuesto) * (p_oLineasFacturaCompra.DiscountPercent / 100)))

                .Descuento = CDec(p_oLineasFacturaCompra.DiscountPercent)

                '.MontoDesc = .CostoRepuesto - .PrecioCompraReal

                .Observaciones = CreaObservacionDeTracking(p_oLineasFacturaCompra, _
                                                           drwNewRepuestosProveduria, _
                                                           .FechaEntrega, NoFactura, True, intCantidad)

            End With

            Call dtbRepuestosProveeduria.AddSCGTA_TB_RepuestosxOrden_ProveduriaRow(drwNewRepuestosProveduria)

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(ex.Message)
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "Funciones Salidas de Inventario"

    <System.CLSCompliant(False)> _
    Public Overloads Function RecorreDocumentosMarketingSinProcesar(ByVal objCompany As SAPbobsCOM.Company,
                                                                    ByVal TipoDocumentoMarketing As SAPbobsCOM.BoObjectTypes, _
                                                                    ByVal TipoDocumentoMArketingBase As SAPbobsCOM.BoObjectTypes, _
                                                                    ByVal strDocnum As String) As Boolean
        Dim oDocumentoMarketing As SAPbobsCOM.Documents
        Dim intDocEntry As Integer = 0
        Dim docEntry As String
        Dim blnEntradaOT As Boolean

        Try

            If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                blnUsaOTInternaConfiguracion = True '
            Else
                blnUsaOTInternaConfiguracion = False
                'Return False
            End If

            If TipoDocumentoMarketing = BoObjectTypes.oPurchaseInvoices Then
                docEntry = Utilitarios.EjecutarConsulta(String.Format(" Select DocEntry From OPCH with (nolock) WHERE DocNum = {0} ", strDocnum), m_oCompany.CompanyDB, m_oCompany.Server)
            ElseIf TipoDocumentoMarketing = BoObjectTypes.oPurchaseDeliveryNotes Then
                docEntry = Utilitarios.EjecutarConsulta(String.Format(" Select DocEntry From OPDN with (nolock) WHERE DocNum = {0} ", strDocnum), m_oCompany.CompanyDB, m_oCompany.Server)
            End If

            oDocumentoMarketing = DirectCast(m_oCompany.GetBusinessObject(TipoDocumentoMarketing),  _
                                             SAPbobsCOM.Documents)
            If oDocumentoMarketing.GetByKey(docEntry) Then
                blnEntradaOT = False
                For index As Integer = 0 To oDocumentoMarketing.Lines.Count - 1
                    oDocumentoMarketing.Lines.SetCurrentLine(index)
                    If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim) Then
                        blnEntradaOT = True
                        Exit For
                    End If
                Next
                If Not oDocumentoMarketing Is Nothing Then
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketing)
                    oDocumentoMarketing = Nothing
                End If
                If Not blnEntradaOT Then
                    Exit Function
                End If
            End If


            'If Not blnUsaOTInternaConfiguracion Then

            intDocEntry = docEntry

            If intDocEntry > 0 Then

                Call CargaDocumentosMarketing(intDocEntry, _
                                              TipoDocumentoMarketing, _
                                              TipoDocumentoMArketingBase)
            End If

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(ex.Message)
            Return False
        End Try

    End Function

    <System.CLSCompliant(False)> _
    Public Overloads Function RecorreDocumentosMarketingSinProcesar(ByVal objCompany As SAPbobsCOM.Company,
                                                                    ByVal TipoDocumentoMarketing As SAPbobsCOM.BoObjectTypes, _
                                                                    ByVal TipoDocumentoMArketingBase1 As SAPbobsCOM.BoObjectTypes, _
                                                                    ByVal TipoDocumentoMArketingBase2 As SAPbobsCOM.BoObjectTypes, _
                                                                    ByVal strDocnum As String) As Boolean

        Dim intDocEntry As Integer = 0
        Dim oDocumentoMarketing As SAPbobsCOM.Documents
        Dim docEntry As String
        Dim strSucursal As String
        Try

            If TipoDocumentoMarketing = BoObjectTypes.oPurchaseReturns Then
                docEntry = Utilitarios.EjecutarConsulta(String.Format(" Select DocEntry From ORPD with (nolock) WHERE DocNum = {0} ", strDocnum), m_oCompany.CompanyDB, m_oCompany.Server)
            ElseIf TipoDocumentoMarketing = BoObjectTypes.oPurchaseCreditNotes Then
                docEntry = Utilitarios.EjecutarConsulta(String.Format(" Select DocEntry From ORPC with (nolock) WHERE DocNum = {0} ", strDocnum), m_oCompany.CompanyDB, m_oCompany.Server)
            End If

            If Not String.IsNullOrEmpty(docEntry) Then

                oDocumentoMarketing = DirectCast(m_oCompany.GetBusinessObject(TipoDocumentoMarketing),  _
                                                            SAPbobsCOM.Documents)
                oDocumentoMarketing.GetByKey(docEntry)
                strSucursal = oDocumentoMarketing.UserFields.Fields.Item("U_SCGD_idSucursal").Value

                If Not oDocumentoMarketing Is Nothing Then
                    'Destruyo el Objeto - Error HRESULT  
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketing)
                    oDocumentoMarketing = Nothing
                End If

                'Cargar todas las facturas que no han sido procesadas, todas las facturas se guardan con el campo
                'procesada en no, para poder obtener el numero de código
                intDocEntry = docEntry
                If intDocEntry > 0 Then

                    Call CargaDocumentosMarketing(intDocEntry, _
                                                   TipoDocumentoMarketing, _
                                                   TipoDocumentoMArketingBase1, _
                                                   TipoDocumentoMArketingBase2, _
                                                   strSucursal)

                End If

                Return True
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            Return False
        End Try

    End Function

    Private Overloads Function CargaDocumentosMarketing(ByVal p_DocEntry As Integer, _
                                                        ByVal TipoDocumentoMarketing As SAPbobsCOM.BoObjectTypes, _
                                                        ByVal TipoDocumentoMarketingBase As SAPbobsCOM.BoObjectTypes) As Boolean

        Dim oDocumentoMarketing As SAPbobsCOM.Documents
        Dim oDocumentoMarketingBase As SAPbobsCOM.Documents
        Dim intIndiceDeLineas As Integer
        Dim strNoOrdendeTrabjo As String
        Dim ProcesoFactura As Boolean = True
        Dim oListaDocumentoBase As Generic.List(Of Integer) = New Generic.List(Of Integer)
        Dim drwRepuestosXOrdenRow As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
        Dim intIdRepuestosXOrden As Integer
        Dim intDocEntryLinea As Integer
        Dim intVisOrderLinea As Integer
        Dim strIdRepuestoXOrden As String
        Dim strConsulta As String = String.Empty
        Dim drID() As System.Data.DataRow
        Dim listDocEntry As System.Data.DataTable
        Dim strNombreColumna As String = String.Empty
        Dim strTabla As String = String.Empty


        Select Case TipoDocumentoMarketing

            Case SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes
                strConsulta = " Select distinct BaseEntry, U_SCGD_NoOT from PDN1 with (nolock) where DocEntry = '{0}' "
            Case SAPbobsCOM.BoObjectTypes.oPurchaseInvoices
                strConsulta = " Select distinct BaseEntry, U_SCGD_NoOT from PCH1 with (nolock) where DocEntry = '{0}' "

        End Select

        Try

            oDocumentoMarketing = DirectCast(m_oCompany.GetBusinessObject(TipoDocumentoMarketing),  _
                                             SAPbobsCOM.Documents)

            oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(TipoDocumentoMarketingBase),  _
                                            SAPbobsCOM.Documents)
            If oDocumentoMarketing.GetByKey(p_DocEntry) Then

                If TipoDocumentoMarketing = BoObjectTypes.oPurchaseInvoices Then
                    strTabla = "PCH1"
                ElseIf TipoDocumentoMarketing = BoObjectTypes.oPurchaseDeliveryNotes Then
                    strTabla = "PDN1"
                End If
                strConsulta = String.Format(strConsulta, oDocumentoMarketing.DocEntry, strTabla)
                listDocEntry = Utilitarios.EjecutarConsultaDataTable(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
                '***
                If Not blnUsaOTInternaConfiguracion Then

                    m_dstRepuestosxOrden = New RepuestosxOrdenDataset
                    m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter

                    ManejaCantidadesCotizacion(oDocumentoMarketing)
                    strNombreColumna = mc_strIdRepxOrd
                Else
                    ManejaCantidadesCotizacion(oDocumentoMarketing)
                    strNombreColumna = mc_strID
                End If

                For intIndiceDeLineas = 0 To oDocumentoMarketing.Lines.Count - 1
                    Call oDocumentoMarketing.Lines.SetCurrentLine(intIndiceDeLineas)

                    If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim) Then
                        drID = listDocEntry.Select(String.Format(" U_SCGD_NoOT = '{0}' ", oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim))
                        If drID.Length > 0 Then
                            If Not IsDBNull(drID(0).Item("BaseEntry")) Then
                                If oDocumentoMarketingBase.GetByKey(drID(0).Item("BaseEntry")) Then
                                    For index As Integer = 0 To oDocumentoMarketingBase.Lines.Count - 1
                                        oDocumentoMarketingBase.Lines.SetCurrentLine(index)
                                        If oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value.ToString.Trim =
                                           oDocumentoMarketing.Lines.UserFields.Fields.Item(strNombreColumna).Value.ToString.Trim Then
                                            strNoOrdendeTrabjo = CStr(oDocumentoMarketingBase.Lines.UserFields.Fields.Item(mc_strNoOt).Value)
                                            If m_udtTieneSucursal = TrabajaConSucursal.Si Then

                                                If Not blnUsaOTInternaConfiguracion Then
                                                    CreaInstanciasDeObjetosDeTaller(oDocumentoMarketingBase.UserFields.Fields.Item(mc_strIdSucursal).Value.ToString.Trim, _
                                                                           TrabajaConSucursal.Si, _
                                                                           m_oCompany.CompanyDB, _
                                                                           m_oCompany.CompanyName)
                                                End If


                                            End If

                                            If Not blnUsaOTInternaConfiguracion Then
                                                Call m_dstRepuestosxEstadoxOrden.Clear()
                                                Call m_adpRepuestosxEstado.Fill(m_dstRepuestosxEstadoxOrden, strNoOrdendeTrabjo)
                                                Call m_dstRepuestosxOrden.Clear()
                                                Call m_adpRepuestosxOrden.Fill(m_dstRepuestosxOrden, strNoOrdendeTrabjo)
                                            End If


                                            If oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim = strNoOrdendeTrabjo Then

                                                If Not blnUsaOTInternaConfiguracion Then
                                                    If Not InteraccionConSCGTaller(oDocumentoMarketing.Lines, _
                                                                 oDocumentoMarketingBase, _
                                                                    oDocumentoMarketing.DocDate, _
                                                                    oDocumentoMarketing.DocNum, _
                                                                    oDocumentoMarketing.Series, _
                                                                    strNoOrdendeTrabjo, _
                                                                    m_dstRepuestosxEstadoxOrden.SCGTA_TB_RepuestosxEstado) Then

                                                        ProcesoFactura = False
                                                        intIndiceDeLineas = oDocumentoMarketing.Lines.Count - 1


                                                    End If
                                                Else

                                                    If Not InteraccionConTallerInterno(oDocumentoMarketing.Lines, _
                                                                 oDocumentoMarketingBase, _
                                                                    oDocumentoMarketing.DocDate, _
                                                                    oDocumentoMarketing.DocNum, _
                                                                    oDocumentoMarketing.Series, _
                                                                    strNoOrdendeTrabjo) Then

                                                        ProcesoFactura = False
                                                        intIndiceDeLineas = oDocumentoMarketing.Lines.Count - 1

                                                        ' MsgBox("El proceso de interaccion de la linea " & intIndiceDeLineas & " de la factura " & p_DocEntry & " no se ejecuto correctamente")
                                                    End If
                                                End If

                                                Dim TipoDocumentoBase As String

                                                Try
                                                    TipoDocumentoBase = Utilitarios.EjecutarConsulta("select distinct BaseType from [PCH1]where DocEntry = '" & p_DocEntry & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                Catch ex As Exception

                                                End Try

                                                If TipoDocumentoBase.Trim() = "540000006" Then
                                                    Call CargaOfertaCompra(oDocumentoMarketingBase.DocEntry)
                                                Else
                                                    Call CargaOrdenCompra(oDocumentoMarketingBase.DocEntry)

                                                End If

                                                If Not blnUsaOTInternaConfiguracion Then

                                                    If m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows.Count() > 0 Then

                                                        intDocEntryLinea = CInt(oDocumentoMarketing.DocEntry)
                                                        intVisOrderLinea = CInt(oDocumentoMarketing.Lines.VisualOrder)

                                                        ' DocType # 18 es para factura de proveedores
                                                        If oDocumentoMarketing.DocObjectCode = 18 Then
                                                            strIdRepuestoXOrden = Utilitarios.EjecutarConsulta("select U_SCGD_IdRepxOrd from [PCH1]where DocEntry = '" & intDocEntryLinea & "' and VisOrder= '" & intVisOrderLinea & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                            ' DocType # 20 es para entrada mercancia
                                                        ElseIf oDocumentoMarketing.DocObjectCode = 20 Then
                                                            strIdRepuestoXOrden = Utilitarios.EjecutarConsulta("select U_SCGD_IdRepxOrd from [PDN1]where DocEntry = '" & intDocEntryLinea & "' and VisOrder= '" & intVisOrderLinea & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                                        End If

                                                        If strIdRepuestoXOrden <> String.Empty Or strIdRepuestoXOrden <> "" Then

                                                            For Each drwRepuestosXOrdenRow In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                                                                intIdRepuestosXOrden = CInt(strIdRepuestoXOrden)

                                                                If oDocumentoMarketing.Lines.ItemCode = drwRepuestosXOrdenRow.NoRepuesto And intIdRepuestosXOrden = drwRepuestosXOrdenRow.ID Then
                                                                    drwRepuestosXOrdenRow.Costo = CDec(oDocumentoMarketing.Lines.Price)
                                                                End If

                                                            Next
                                                        End If

                                                    End If
                                                    'Actualiza Costo SE en la tabla SCGTA_TB_RepuestosXOrden
                                                    m_adpRepuestosxOrden.UpdateCostoRepuestosXOrden(m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden)

                                                End If

                                            End If
                                            Exit For
                                        End If

                                    Next

                                End If

                            End If
                        End If
                    End If

                Next
            Else
                Return False
            End If
            If ProcesoFactura Then

                If Not strBO_TipoParcial = "Y" Then

                    If oDocumentoMarketingBase.DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then Call oDocumentoMarketingBase.Close()

                End If

                ''''LLamar a metodo que reccorre las lineas de la Orden de Compra

                Call RecorreLineasOrdenDeCompra(oDocumentoMarketingBase, oDocumentoMarketing, blnUsaOTInternaConfiguracion)

            End If
            If Not oDocumentoMarketing Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketing)
                oDocumentoMarketing = Nothing
            End If
            If Not oDocumentoMarketingBase Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketingBase)
                oDocumentoMarketingBase = Nothing
            End If
            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) 'MsgBox(ex.Message)
            Return False
        Finally

            oListaDocumentoBase.Clear()
        End Try

    End Function

    Private Sub ManejaCantidadesCotizacion(ByVal oDocumentoMarketing As Documents, Optional p_blnEsNotaCredito As Boolean = False, Optional p_docEntryNotaCredito As Integer = 0)

        Dim oCotizacion As SAPbobsCOM.Documents
        Dim strConsulta As String = " Select DocEntry from OQUT with (nolock) where U_SCGD_Numero_OT in ({0}) "
        Dim strNumeroCotizacion As String
        Dim decCantidadDM As Decimal = 0
        Dim decCantidadSolicitado As Decimal = 0
        Dim decCantidadPendiente As Decimal = 0
        Dim strCantidadSolicitado As String
        Dim strNombreColumna As String = String.Empty
        Dim strNombreColumna2 As String = String.Empty
        Dim listOTs As Generic.List(Of String) = New Generic.List(Of String)
        Dim listDocEntry As Data.DataTable
        Dim m_blnConfOTSAP As Boolean
        Dim drOT As DataRow

        Try
            
            m_blnConfOTSAP = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)

            If Not m_blnConfOTSAP Then
                strNombreColumna = mc_strIdRepxOrd
                strNombreColumna2 = mc_strID
            Else
                strNombreColumna = mc_strID
                strNombreColumna2 = mc_strIdRepxOrd
            End If

            oCotizacion = m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations)
            strNumeroCotizacion = ""

            If p_blnEsNotaCredito Then

                oDocumentoMarketing = DirectCast(m_oCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes),  _
                                         Documents)

                oDocumentoMarketing.GetByKey(p_docEntryNotaCredito)

            End If


            For indLns As Integer = 0 To oDocumentoMarketing.Lines.Count - 1
                
                oDocumentoMarketing.Lines.SetCurrentLine(indLns)
                
                If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim) Then
                    If Not listOTs.Contains(oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim) Then
                        listOTs.Add(oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim)
                        strNumeroCotizacion += String.Format("'{0}',", oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim)
                    End If
                End If
            Next

            'si el documento no presenta OT's 
            If listOTs.Count = 0 Then
                Exit Sub
            End If

            strConsulta = String.Format(strConsulta, strNumeroCotizacion.TrimEnd(","))
            listDocEntry = Utilitarios.EjecutarConsultaDataTable(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

            For index As Integer = 0 To listDocEntry.Rows.Count - 1
                drOT = listDocEntry.Rows(index)
                If Not IsDBNull(drOT.Item("DocEntry")) Then
                    If oCotizacion.GetByKey(drOT.Item("DocEntry")) Then
                        For indLns As Integer = 0 To oDocumentoMarketing.Lines.Count - 1
                            oDocumentoMarketing.Lines.SetCurrentLine(indLns)
                            If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim) Then
                                If oDocumentoMarketing.Lines.UserFields.Fields.Item(mc_strNoOt).Value.ToString.Trim = oCotizacion.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value.ToString.Trim Then

                                    For indCot As Integer = 0 To oCotizacion.Lines.Count - 1
                                        oCotizacion.Lines.SetCurrentLine(indCot)

                                        If oCotizacion.Lines.UserFields.Fields.Item(strNombreColumna).Value =
                                            oDocumentoMarketing.Lines.UserFields.Fields.Item(strNombreColumna).Value Then

                                            decCantidadDM = oDocumentoMarketing.Lines.Quantity
                                            strCantidadSolicitado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value

                                            If Not String.IsNullOrEmpty(strCantidadSolicitado) Then decCantidadSolicitado = Decimal.Parse(strCantidadSolicitado)

                                            If m_blnConfOTSAP Then
                                                If decCantidadSolicitado = 0 Then
                                                    strCantidadSolicitado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                                    If Not String.IsNullOrEmpty(strCantidadSolicitado) Then decCantidadSolicitado = Decimal.Parse(strCantidadSolicitado)
                                                End If

                                            End If

                                            If Not p_blnEsNotaCredito Then
                                                If decCantidadDM > decCantidadSolicitado Then
                                                    decCantidadDM = decCantidadSolicitado
                                                End If

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += decCantidadDM

                                                decCantidadPendiente = decCantidadSolicitado - decCantidadDM
                                                'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = Double.Parse(decCantidadPendiente)

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = Double.Parse(decCantidadPendiente)

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = oDocumentoMarketing.Lines.LineTotal

                                                Exit For
                                            Else
                                                Dim decCantidadRecibida As Decimal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                                                Dim decCantidadNotaCredito As Decimal = oDocumentoMarketing.Lines.Quantity

                                                If decCantidadNotaCredito >= decCantidadRecibida Then
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value += oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                End If

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0
                                            End If
                                        ElseIf Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item(strNombreColumna2).Value) AndAlso oCotizacion.Lines.UserFields.Fields.Item(strNombreColumna2).Value =
                                    oDocumentoMarketing.Lines.UserFields.Fields.Item(strNombreColumna2).Value Then

                                            decCantidadDM = oDocumentoMarketing.Lines.Quantity
                                            strCantidadSolicitado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value

                                            If Not String.IsNullOrEmpty(strCantidadSolicitado) Then decCantidadSolicitado = Decimal.Parse(strCantidadSolicitado)

                                            If m_blnConfOTSAP Then
                                                If decCantidadSolicitado = 0 Then
                                                    strCantidadSolicitado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                                    If Not String.IsNullOrEmpty(strCantidadSolicitado) Then decCantidadSolicitado = Decimal.Parse(strCantidadSolicitado)
                                                End If

                                            End If

                                            If Not p_blnEsNotaCredito Then
                                                If decCantidadDM > decCantidadSolicitado Then
                                                    decCantidadDM = decCantidadSolicitado
                                                End If

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += decCantidadDM

                                                decCantidadPendiente = decCantidadSolicitado - decCantidadDM
                                                'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = Double.Parse(decCantidadPendiente)

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = Double.Parse(decCantidadPendiente)

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = oDocumentoMarketing.Lines.LineTotal

                                                Exit For
                                            Else
                                                Dim decCantidadRecibida As Decimal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                                                Dim decCantidadNotaCredito As Decimal = oDocumentoMarketing.Lines.Quantity

                                                If decCantidadNotaCredito >= decCantidadRecibida Then
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value += oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                                End If

                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0
                                            End If

                                        End If

                                    Next
                                End If
                            End If
                        Next
                        oCotizacion.Update()
                    End If
                End If
            Next
            If Not oCotizacion Is Nothing Then
                'Destruyo el Objeto - Error HRESULT  
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
        End Try

    End Sub

    Private Overloads Function CargaDocumentosMarketing(ByVal p_DocEntry As Integer, _
                                              ByVal TipoDocumentoMarketing As SAPbobsCOM.BoObjectTypes, _
                                              ByVal TipoDocumentoMarketingBase1 As SAPbobsCOM.BoObjectTypes, _
                                              ByVal TipoDocumentoMarketingBase2 As SAPbobsCOM.BoObjectTypes, _
                                              ByVal intIdSucursal As String) As Boolean
        'se cambia el tipo del parametro intIdSucursal de Integer a String

        Dim oDocumentoMarketing As SAPbobsCOM.Documents
        Dim oDocumentoMarketingBase1 As SAPbobsCOM.Documents
        Dim oDocumentoMarketingBase2 As SAPbobsCOM.Documents
        Dim intIndiceDeLineas As Integer = 0
        Dim ProcesoFactura As Boolean = True
        Dim NoOrdenDeTrabajo As String
        Dim intTieneDocumentoBase1 As Integer = 0
        Dim intTieneDocumentoBase2 As Integer = 0
        Dim oListaDocumentoBase As Generic.List(Of Integer) = New Generic.List(Of Integer)
        Dim bNotaCredito As Boolean

        Try

            oDocumentoMarketing = DirectCast(m_oCompany.GetBusinessObject(TipoDocumentoMarketing), SAPbobsCOM.Documents)

            oDocumentoMarketingBase1 = DirectCast(m_oCompany.GetBusinessObject(TipoDocumentoMarketingBase1), SAPbobsCOM.Documents)

            oDocumentoMarketingBase2 = DirectCast(m_oCompany.GetBusinessObject(TipoDocumentoMarketingBase2), SAPbobsCOM.Documents)

            If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                blnUsaOTInternaConfiguracion = True

                If TipoDocumentoMarketing = SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes Then
                    'Se comenta por el problema al generar las notas de credito que cambia mal las cantidades en la cotización
                    'ManejaCantidadesCotizacion(oDocumentoMarketing, True, p_DocEntry)

                    oDocumentoMarketing.GetByKey(p_DocEntry)
                    bNotaCredito = True
                    If Not InteraccionInterno(oDocumentoMarketing.Lines, _
                                                                     oDocumentoMarketingBase2, _
                                                                     oDocumentoMarketing.DocDate, _
                                                                     oDocumentoMarketing.DocNum, _
                                                                     oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value, _
                                                                     oDocumentoMarketing.Series, bNotaCredito) Then

                        ProcesoFactura = False
                        intIndiceDeLineas = oDocumentoMarketing.Lines.Count - 1
                    End If
                    Exit Function
                Else
                    oDocumentoMarketing.GetByKey(p_DocEntry)
                    Dim strot As String = oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                    bNotaCredito = False
                    If Not InteraccionInterno(oDocumentoMarketing.Lines, _
                                                                  oDocumentoMarketingBase2, _
                                                                  oDocumentoMarketing.DocDate, _
                                                                  oDocumentoMarketing.DocNum, _
                                                                  oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value, _
                                                                  oDocumentoMarketing.Series, bNotaCredito) Then

                        ProcesoFactura = False
                        intIndiceDeLineas = oDocumentoMarketing.Lines.Count - 1
                    End If
                    Exit Function
                End If

            Else
                blnUsaOTInternaConfiguracion = False
            End If


            If oDocumentoMarketing.GetByKey(p_DocEntry) Then

                'For j As Integer = 0 To oListaDocumentoBase.Count - 1

                Call oDocumentoMarketing.Lines.SetCurrentLine(intIndiceDeLineas)

                intTieneDocumentoBase1 = oDocumentoMarketing.Lines.BaseEntry

                If intTieneDocumentoBase1 > 0 Then

                    oDocumentoMarketingBase1.GetByKey(oDocumentoMarketing.Lines.BaseEntry)

                    'lleno la lista con los baseEntry en caso de que provenga de multiples entradas o Pedidos
                    For i As Integer = 0 To oDocumentoMarketingBase1.Lines.Count - 1
                        oDocumentoMarketingBase1.Lines.SetCurrentLine(i)
                        If Not oListaDocumentoBase.Contains(oDocumentoMarketingBase1.Lines.BaseEntry) Then
                            oListaDocumentoBase.Add(oDocumentoMarketingBase1.Lines.BaseEntry)
                        End If
                    Next


                    For j As Integer = 0 To oListaDocumentoBase.Count - 1

                        intTieneDocumentoBase2 = oListaDocumentoBase.Item(j)

                        If intTieneDocumentoBase2 > 0 Then

                            oDocumentoMarketingBase2.GetByKey(intTieneDocumentoBase2)

                            NoOrdenDeTrabajo = CStr(oDocumentoMarketingBase2.UserFields.Fields.Item(mc_strNoOrdendeTrabajo).Value)

                            If NoOrdenDeTrabajo <> "" Then

                                If m_udtTieneSucursal = TrabajaConSucursal.Si Then

                                    If Not blnUsaOTInternaConfiguracion Then

                                        Call CreaInstanciasDeObjetosDeTaller(intIdSucursal, _
                                                                         TrabajaConSucursal.Si, _
                                                                         m_oCompany.CompanyDB, _
                                                                         m_oCompany.CompanyName)
                                    End If


                                End If

                                If Not blnUsaOTInternaConfiguracion Then
                                    Call m_dstRepuestosxEstadoxOrden.Clear()
                                    Call m_adpRepuestosxEstado.Fill(m_dstRepuestosxEstadoxOrden, NoOrdenDeTrabajo)
                                End If

                            Else

                                Return False

                            End If 'NoOrdenDeTrabajo

                        Else

                            Return False

                        End If 'oDocumentoMarketingBase2

                        Dim c As Integer = oDocumentoMarketing.Lines.Count

                        For intIndiceDeLineas = 0 To oDocumentoMarketing.Lines.Count - 1

                            Call oDocumentoMarketing.Lines.SetCurrentLine(intIndiceDeLineas)

                            Dim strNoOt As String = oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value

                            If strNoOt = NoOrdenDeTrabajo Then

                                If Not blnUsaOTInternaConfiguracion Then

                                    If Not InteraccionConSCGTallerNotasdeCredito(oDocumentoMarketing.Lines, _
                                                                     oDocumentoMarketingBase2, _
                                                                     oDocumentoMarketing.DocDate, _
                                                                     oDocumentoMarketing.DocNum, _
                                                                     strNoOt, _
                                                                     oDocumentoMarketing.Series, _
                                                                     m_dstRepuestosxEstadoxOrden.SCGTA_TB_RepuestosxEstado) Then

                                        ProcesoFactura = False
                                        intIndiceDeLineas = oDocumentoMarketing.Lines.Count - 1

                                    End If 'InteraccionConSCGTaller


                                End If

                            End If



                        Next
                    Next
                Else

                    'oDocumentoMarketing.UserFields.Fields.Item(mc_strProcesada).Value = "1"
                    'Call oDocumentoMarketing.Update()
                    Return False

                End If 'oDocumentoMarketingBase1
                'Next

            Else
                Return False

            End If 'oDocumentoMarketing

            If ProcesoFactura Then

                'oDocumentoMarketing.UserFields.Fields.Item(mc_strProcesada).Value = "1"

                'Call RecorreLineasNotaCredito(oDocumentoMarketing, _
                '                              NoOrdenDeTrabajo)

                'Call oDocumentoMarketing.Update()

            End If 'ProcesoFactura

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error) ' MsgBox(ex.Message)
            Return False
        Finally

            oListaDocumentoBase.Clear()

        End Try

    End Function

    Private Function InteraccionInterno(ByVal p_oLineasDocumentoMarketing As SAPbobsCOM.Document_Lines, _
                                                           ByRef oDocumentoMarketingBase As SAPbobsCOM.Documents, _
                                                           ByVal FechaNotaCredito As Date, _
                                                           ByVal NoNotadeCredito As Integer, _
                                                           ByVal NoOrdenTrabajo As String, _
                                                           ByVal SeriesNotaCredito As Integer, _
                                                           ByVal p_bNotaCredito As Boolean) As Boolean


        Try
            Utilitarios.ActualizarLineaTrackinginterno(p_oLineasDocumentoMarketing, oDocumentoMarketingBase, FechaNotaCredito, oDocumentoMarketingBase.DocNum, SeriesNotaCredito, NoOrdenTrabajo, m_oCompany, True, p_bNotaCredito)
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


#End Region

#End Region

#Region "Eventos"

    <System.CLSCompliant(False)> _
    Public Sub ManejaEventoLoad(ByVal FormUID As String, _
                                 ByRef pVal As SAPbouiCOM.ItemEvent, _
                                 ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = mc_intOrdenDeCompra Then
                oform = m_SBOApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If pVal.Before_Action Then
                    oform.DataSources.DataTables.Add("dtConsulta")
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoUnload(ByVal FormUID As String, _
                                 ByRef pVal As SAPbouiCOM.ItemEvent, _
                                 ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = mc_intOrdenDeCompra Then

                oform = Nothing

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            'm_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Public Sub ObtieneNumeroDocumentoACancelar()

        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox

        oitem = m_SBOApplication.Forms.ActiveForm.Items.Item("8")
        oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
        m_strNoOrden = oEditText.String


        oCombo = m_SBOApplication.Forms.ActiveForm.Items.Item("88").Specific
        m_strNoSerie = oCombo.Selected.Value
    End Sub

    Public Sub ManejarDocumentoACancelar()

        Dim strDocEntry As Integer

        If DevuelveDocEntry(m_strNoOrden, m_strNoSerie, strDocEntry) Then


            Dim TipoDocumentoBase As String

            Try
                TipoDocumentoBase = Utilitarios.EjecutarConsulta("select distinct BaseType from [PCH1]where DocEntry = '" & strDocEntry & "'", m_oCompany.CompanyDB, m_oCompany.Server)
            Catch ex As Exception

            End Try


            If TipoDocumentoBase.Trim() = "540000006" Then

                Call CargaOfertaCompra(CInt(strDocEntry))

            Else

                Call CargaOrdenCompra(CInt(strDocEntry))
            End If

            'Call CargaOrdenCompra(CInt(strDocEntry))

        End If

    End Sub


    Private Function DevuelveDocEntry(ByVal NoDocumento As Integer, _
                                      ByVal Series As Integer, _
                                      ByRef Docentry As Integer) As Boolean

        Dim strDocEntry As String
        Dim strConsultaDocEntry As String = "Select Docentry " & _
                                  " From OPOR " & _
                                  " Where Docnum=" & CStr(NoDocumento) & _
                                  " and Series=" & CStr(Series)

        Try

            strDocEntry = Utilitarios.EjecutarConsulta(strConsultaDocEntry, m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strDocEntry) AndAlso IsNumeric(strDocEntry) Then
                Docentry = CInt(strDocEntry)
                Return True
            Else
                Return False
            End If



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBOApplication)
            Return False
        End Try

    End Function

#End Region

End Class
