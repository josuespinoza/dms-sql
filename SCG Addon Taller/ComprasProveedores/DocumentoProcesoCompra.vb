Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports DMSOneFramework
Public Class DocumentoProcesoCompra
#Region "Declaraciones"
    Private m_SBOApplication As SAPbouiCOM.Application
    Private Const mc_strNoOrdendeTrabajo As String = "U_SCGD_Numero_OT"
    Private Const mc_strTipoSuministro As String = "U_SCGD_TipoSum"
    Private Const mc_strTipoArticulo As String = "U_SCGD_TipoArticulo"
    Private Const mc_strIdSucursal As String = "U_SCGD_idSucursal"
    Private Const mc_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"
    Private Const mc_strNoOt As String = "U_SCGD_NoOT"
    Private Const mc_strID As String = "U_SCGD_ID"

    Private oForm As SAPbouiCOM.Form

    'variable para verificar si hace Ordenes de compra parciales
    Private strTipoParcial As String = String.Empty

    Public n As NumberFormatInfo

    Private Const mc_strDocEntry As String = "DocEntry"

    Private blnUsaOTInternaConfiguracion As Boolean = False

    Private strNoOrden As String = String.Empty
    Private strItemCode As String = String.Empty
    Private dblCosto As Double = 0
    Private dblCantidad As Double = 0
    Private dblCantidadRecibida As Double = 0
    Private dblCantidadSolicitada As Double = 0
    Private dblCantidadPendiente As Double = 0
    Private dblCantidadPendienteTraslado As Double = 0
    Private dblCantidadPendienteBodega As Double = 0
    Private dblCantidadPendienteDevolucion As Double = 0
    Private strIdRepuestosxOrden As String = String.Empty
    Private strID As String = String.Empty
    Private intTipoDocumentoMarketingBase As Integer = 0
    Private intDocEntryDocMarketingBase As Integer = 0
    Private strTipoArticulo As String = String.Empty
    Private dblResultadoCosto As Double = 0

    Private strIdItemMarketing As String = String.Empty
    Private strIdItemCotizacion As String = String.Empty
    Private dblCantidadTemp As Double = 0
    Private dblCantidadRecibidaTemp As Double = 0
    Private dblCantidadSolicitadaTemp As Double = 0
    Private dblCantidadPendienteTemp As Double = 0
    Private dblCantidadPendienteTrasladoTemp As Double = 0
    Private dblCantidadPendienteBodegaTemp As Double = 0
    Private dblCantidadPendienteDevolucionTemp As Double = 0

    Private Si As String = "Y"
    Private No As String = "N"

    Private Enum TipoDocumentoMarketing
        FacturaProveedor = 0
        NotaCredito = 1
        EntradaMercancia = 2
        DevolucionMercancia = 3
    End Enum

    Private Enum TipoDocumentoMarketingBase
        OfertaCompra = 540000006
        OrdenCompra = 22
        EntradaMercancia = 20
        FacturaProveedor = 18
        NotaCredito = 19
        DevolucionMercancia = 21
    End Enum

    Dim name As String = _
        System.Enum.GetName(GetType(DateInterval), 0)


    Private Enum UsaCosteoSExFP
        No = 0
        Si = 1
    End Enum

    Public Enum TrabajaConSucursal
        No = 0
        Si = 1
    End Enum

    Private m_oCompany As SAPbobsCOM.Company
    Private m_udtTieneSucursal As TrabajaConSucursal
    Private mc_intOrdenDeCompra As Integer = 142
#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal oCompany As SAPbobsCOM.Company, _
                   ByVal SBO_Application As Application)

        m_oCompany = oCompany
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

#Region "Eventos"

#End Region
#Region "Metodos"
#Region "Generales"
    Public Sub ProcesaDocumentoMarketing(ByVal p_strDocEntry As String, ByVal p_intTipoDocumentoMarketing As Integer)
        Try
            Dim strTipoDocumentoMarketing As String = String.Empty
            Dim oLineasDocumentoMarketing As New List(Of ListaLineasDocumentoMarketing)()
            Dim oLineasDocumentoMarketingBaseDiferencias As New List(Of ListaLineasDocumentoMarketing)()
            Dim oListaNoOrden As Generic.List(Of String) = New Generic.List(Of String)
            Dim intDocEntry As Integer = 0
            Dim oDocumentoMarketing As SAPbobsCOM.Documents
            Dim strUsaCostoSExFP As String = String.Empty
            Dim intUsaCostoSExFP As Integer = 0
            Dim dblLineTotal As Double = 0
            Dim strBOTipoParcial As String = String.Empty
            Dim blnUsaBackOrder As Boolean = True
            strNoOrden = String.Empty
            strItemCode = String.Empty
            intTipoDocumentoMarketingBase = 0
            intDocEntryDocMarketingBase = 0
            dblCosto = 0
            dblCantidad = 0
            strIdRepuestosxOrden = String.Empty
            strID = String.Empty
            Select Case p_intTipoDocumentoMarketing
                Case TipoDocumentoMarketing.FacturaProveedor
                    strTipoDocumentoMarketing = "OPCH"
                    oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketing.NotaCredito
                    strTipoDocumentoMarketing = "ORPC"
                Case TipoDocumentoMarketing.EntradaMercancia
                    strTipoDocumentoMarketing = "OPDN"
                Case TipoDocumentoMarketing.DevolucionMercancia
            End Select
            'se obtiene la configuracion para la obtencion de costo
            strUsaCostoSExFP = Utilitarios.EjecutarConsulta("SELECT U_CostSExFP FROM [@SCGD_ADMIN] with (nolock)",
                                               m_oCompany.CompanyDB,
                                               m_oCompany.Server)
            If strUsaCostoSExFP = Si Then
                intUsaCostoSExFP = 1
            Else
                intUsaCostoSExFP = 0
            End If
            strBOTipoParcial = Utilitarios.EjecutarConsulta("select U_BO_Parc from dbo.[@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)
            If strBOTipoParcial = Si Then
                blnUsaBackOrder = True
            Else
                blnUsaBackOrder = False
            End If
            oLineasDocumentoMarketing.Clear()
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                intDocEntry = p_strDocEntry
            End If
            If oDocumentoMarketing.GetByKey(intDocEntry) Then
                For cont As Integer = 0 To oDocumentoMarketing.Lines.Count - 1
                    oDocumentoMarketing.Lines.SetCurrentLine(cont)
                    intTipoDocumentoMarketingBase = 0
                    intDocEntryDocMarketingBase = 0
                    If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                        strNoOrden = oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()
                        strItemCode = oDocumentoMarketing.Lines.ItemCode.ToString.Trim()
                        dblCantidad = oDocumentoMarketing.Lines.Quantity
                        dblLineTotal = oDocumentoMarketing.Lines.LineTotal
                        intTipoDocumentoMarketingBase = oDocumentoMarketing.Lines.BaseType
                        intDocEntryDocMarketingBase = oDocumentoMarketing.Lines.BaseEntry
                        If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value) Then
                            dblCosto = oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                        Else
                            dblCosto = 0
                        End If
                        If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then
                            strIdRepuestosxOrden = oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim()
                        Else
                            strIdRepuestosxOrden = String.Empty
                        End If
                        If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                            strID = oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim()
                        Else
                            strID = String.Empty
                        End If
                        If Not String.IsNullOrEmpty(strItemCode) And Not String.IsNullOrEmpty(strNoOrden) Then
                            oLineasDocumentoMarketing.Add(New ListaLineasDocumentoMarketing() _
                                                                                       With {.NoOrden = strNoOrden,
                                                                                             .ItemCode = strItemCode,
                                                                                             .CantidadDocMarketing = dblCantidad,
                                                                                             .TipoDocumentoMarketing = p_intTipoDocumentoMarketing,
                                                                                             .TipoDocumentoMarketingBase = intTipoDocumentoMarketingBase,
                                                                                             .DocEntryDocMarketingBase = intDocEntryDocMarketingBase,
                                                                                             .LineTotalDocMarketing = dblLineTotal,
                                                                                             .ID = strID,
                                                                                             .IDRepuestosxOrden = strIdRepuestosxOrden})
                            If Not oListaNoOrden.Contains(strNoOrden) Then
                                oListaNoOrden.Add(strNoOrden)
                            End If
                        End If
                    End If
                Next
                ProcesaDiferenciasDocumentoMarketingBase(oLineasDocumentoMarketing, oLineasDocumentoMarketingBaseDiferencias)
                If Not blnUsaBackOrder Then
                    m_SBOApplication.StatusBar.SetText(My.Resources.Resource.ProcesaBackOrder, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    ProcesaBackOrder(oLineasDocumentoMarketing)
                End If
                If oLineasDocumentoMarketing.Count > 0 Then
                    m_SBOApplication.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    ActualizaCotizacion(oListaNoOrden, oLineasDocumentoMarketing, blnUsaBackOrder, p_intTipoDocumentoMarketing, intUsaCostoSExFP, oLineasDocumentoMarketingBaseDiferencias)
                End If
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
    Public Sub ActualizaCotizacion(ByRef p_oListaNoOrden As Generic.List(Of String), _
                                   ByVal p_oLineasDocumentoMarketing As List(Of ListaLineasDocumentoMarketing), _
                                   ByVal p_blnUsaBackOrder As Boolean, _
                                   ByVal p_intTipoDocumentoMarketing As Integer, _
                                   ByVal p_intUsaCostoSExFP As Integer, _
                                   ByVal p_oLineasDocumentoMarketingBaseDiferenciasBackOrder As List(Of ListaLineasDocumentoMarketing))
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            Dim oListaDocEntryCotizacion As Generic.List(Of String) = New Generic.List(Of String)
            Dim intDocEntry As Integer = 0
            Dim oLineasCotizacion As New List(Of ListaLineasDocumentoMarketing)()
            Dim oLineasCotizacionResultado As New List(Of ListaLineasDocumentoMarketing)()
            Dim blnActualizaCotizacion As Boolean = False
            Dim intResultado As Integer = 1
            strNoOrden = String.Empty
            strItemCode = String.Empty
            dblCosto = 0
            dblCantidad = 0
            dblCantidadRecibida = 0
            dblCantidadSolicitada = 0
            dblCantidadPendiente = 0
            dblCantidadPendienteTraslado = 0
            dblCantidadPendienteBodega = 0
            dblCantidadPendienteDevolucion = 0
            strIdRepuestosxOrden = String.Empty
            strID = String.Empty
            strTipoArticulo = String.Empty
            oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                                             SAPbobsCOM.Documents)
            CargarDocEntryCotizacion(p_oListaNoOrden, oListaDocEntryCotizacion)
            For Each rowDocEntry As String In oListaDocEntryCotizacion
                If Not String.IsNullOrEmpty(rowDocEntry) Then
                    blnActualizaCotizacion = False
                    intDocEntry = Convert.ToInt32(rowDocEntry)
                    If oCotizacion.GetByKey(intDocEntry) Then
                        For contador As Integer = 0 To oCotizacion.Lines.Count - 1
                            oCotizacion.Lines.SetCurrentLine(contador)
                            If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) And Not String.IsNullOrEmpty(oCotizacion.Lines.ItemCode) Then
                                oLineasCotizacionResultado.Clear()
                                strNoOrden = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()
                                strItemCode = oCotizacion.Lines.ItemCode.ToString.Trim()
                                dblCantidad = oCotizacion.Lines.Quantity
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value) Then
                                    dblCosto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                Else
                                    dblCosto = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value) Then
                                    dblCantidadRecibida = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                                Else
                                    dblCantidadRecibida = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value) Then
                                    dblCantidadSolicitada = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                                Else
                                    dblCantidadSolicitada = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value) Then
                                    dblCantidadPendiente = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                Else
                                    dblCantidadPendiente = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value) Then
                                    dblCantidadPendienteBodega = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value
                                Else
                                    dblCantidadPendienteBodega = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value) Then
                                    dblCantidadPendienteTraslado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value
                                Else
                                    dblCantidadPendienteTraslado = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value) Then
                                    dblCantidadPendienteDevolucion = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value
                                Else
                                    dblCantidadPendienteDevolucion = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then
                                    strIdRepuestosxOrden = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString.Trim()
                                Else
                                    strIdRepuestosxOrden = String.Empty
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    strID = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim()
                                Else
                                    strID = String.Empty
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value) Then
                                    strTipoArticulo = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()
                                Else
                                    strTipoArticulo = String.Empty
                                End If

                                If Not String.IsNullOrEmpty(strItemCode) And Not String.IsNullOrEmpty(strNoOrden) Then
                                    oLineasCotizacion.Clear()
                                    oLineasCotizacion.Add(New ListaLineasDocumentoMarketing() _
                                                                                               With {.NoOrden = strNoOrden,
                                                                                                     .ItemCode = strItemCode,
                                                                                                     .TipoDocumentoMarketing = p_intTipoDocumentoMarketing,
                                                                                                     .CantidadCotizacion = dblCantidad,
                                                                                                     .CantidadRecibida = dblCantidadRecibida,
                                                                                                     .CantidadPendiente = dblCantidadPendiente,
                                                                                                     .CantidadSolicitada = dblCantidadSolicitada,
                                                                                                     .CantidadPendienteTraslado = dblCantidadPendienteTraslado,
                                                                                                     .CantidadPendienteDevolucion = dblCantidadPendienteDevolucion,
                                                                                                     .CantidadPendienteBodega = dblCantidadPendienteBodega,
                                                                                                     .CostoCotizacion = dblCosto,
                                                                                                     .TipoArticulo = strTipoArticulo,
                                                                                                     .ID = strID,
                                                                                                     .IDRepuestosxOrden = strIdRepuestosxOrden})
                                    If oLineasCotizacion.Count > 0 And p_oLineasDocumentoMarketing.Count > 0 Then
                                        ManejoCantidadesyCostoCotizacion(p_oLineasDocumentoMarketing, oLineasCotizacion, oLineasCotizacionResultado, p_oLineasDocumentoMarketingBaseDiferenciasBackOrder, p_blnUsaBackOrder)
                                        If oLineasCotizacionResultado.Count > 0 Then
                                            For Each rowResultado As ListaLineasDocumentoMarketing In oLineasCotizacionResultado
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = rowResultado.CantidadRecibida
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = rowResultado.CantidadSolicitada
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = rowResultado.CantidadPendiente
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = rowResultado.CantidadPendienteBodega
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = rowResultado.CantidadPendienteTraslado
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = rowResultado.CantidadPendienteDevolucion
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CBOD").Value = rowResultado.CantidadBackOrderDiferencia
                                                ' Actualiza Costo
                                                If p_intTipoDocumentoMarketing = TipoDocumentoMarketing.FacturaProveedor Then
                                                    If String.IsNullOrEmpty(strTipoArticulo) Or strTipoArticulo = "4" Then
                                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0
                                                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value += rowResultado.CostoCotizacion
                                                        'If p_blnUsaBackOrder Then
                                                        '    If rowResultado.CantidadRecibida = oCotizacion.Lines.Quantity And rowResultado.CantidadSolicitada = 0 And rowResultado.CantidadPendiente = 0 Then
                                                        '        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = rowResultado.CostoCotizacion
                                                        '    End If
                                                        'Else
                                                        '    If rowResultado.CantidadRecibida = oCotizacion.Lines.Quantity And rowResultado.CantidadSolicitada = 0 And rowResultado.CantidadPendiente = 0 Then
                                                        '        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = rowResultado.CostoCotizacion
                                                        '        ' ElseIf oCotizacion.Lines.Quantity= rowResultado.CantidadRecibida
                                                        '    End If
                                                        'End If
                                                        'If rowResultado.CantidadRecibida = (rowResultado.CantidadCotizacion - rowResultado.CantidadBackOrderDiferencia) Then
                                                        '    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = rowResultado.CostoCotizacion
                                                        'Else
                                                        '    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value += rowResultado.CostoCotizacion
                                                        'End If
                                                    End If

                                                End If
                                                blnActualizaCotizacion = True
                                                Exit For
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        If blnActualizaCotizacion Then
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                            End If
                            If Not m_oCompany.InTransaction Then
                                intResultado = 1
                                m_oCompany.StartTransaction()
                                intResultado = oCotizacion.Update()
                            End If
                            If intResultado <> 0 Then
                                If m_oCompany.InTransaction Then
                                    m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                                End If
                            Else
                                If m_oCompany.InTransaction Then
                                    m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If Not oCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
        End Try
    End Sub

    Public Sub ManejoCantidadesyCostoCotizacion(ByVal p_oLineasDocumentoMarketing As List(Of ListaLineasDocumentoMarketing), _
                                          ByVal p_oLineasCotizacion As List(Of ListaLineasDocumentoMarketing), _
                                          ByRef p_oLineasCotizacionResultado As List(Of ListaLineasDocumentoMarketing), _
                                          ByVal p_oLineasDocumentoMarketingBaseDiferencias As List(Of ListaLineasDocumentoMarketing), _
                                          ByVal p_blnUsaBackOrder As Boolean)
        Try
            p_oLineasCotizacionResultado.Clear()
            For Each rowDiferencias As ListaLineasDocumentoMarketing In p_oLineasDocumentoMarketingBaseDiferencias
                For Each rowDocMarketing As ListaLineasDocumentoMarketing In p_oLineasDocumentoMarketing
                    strIdItemMarketing = String.Empty
                    strIdItemCotizacion = String.Empty
                    If Not String.IsNullOrEmpty(rowDocMarketing.ID) Then
                        strIdItemMarketing = rowDocMarketing.ID
                    ElseIf Not String.IsNullOrEmpty(rowDocMarketing.IDRepuestosxOrden) Then
                        strIdItemMarketing = rowDocMarketing.IDRepuestosxOrden
                    End If
                    If rowDiferencias.IdItem = strIdItemMarketing Then
                        rowDocMarketing.CantidadBackOrderDiferencia = rowDiferencias.CantidadBackOrderDiferencia
                        dblResultadoCosto = (rowDocMarketing.LineTotalDocMarketing + rowDiferencias.LineTotalDocBaseMarketing) - rowDiferencias.LineTotalDocBaseMarketing
                        rowDocMarketing.ResultadoCosto = dblResultadoCosto
                        Exit For
                    End If
                Next
            Next
            For Each rowCotizacion As ListaLineasDocumentoMarketing In p_oLineasCotizacion
                For Each rowDocMarketing As ListaLineasDocumentoMarketing In p_oLineasDocumentoMarketing
                    strIdItemMarketing = String.Empty
                    strIdItemCotizacion = String.Empty
                    If Not String.IsNullOrEmpty(rowDocMarketing.ID) Then
                        strIdItemMarketing = rowDocMarketing.ID
                        strIdItemCotizacion = rowCotizacion.ID
                    ElseIf Not String.IsNullOrEmpty(rowDocMarketing.IDRepuestosxOrden) Then
                        strIdItemMarketing = rowDocMarketing.IDRepuestosxOrden
                        strIdItemCotizacion = rowCotizacion.IDRepuestosxOrden
                    End If
                    If strIdItemCotizacion = strIdItemMarketing And rowCotizacion.ItemCode = rowDocMarketing.ItemCode Then
                        Select Case rowDocMarketing.TipoDocumentoMarketing
                            Case TipoDocumentoMarketing.FacturaProveedor
                                'Si la cantidad recibida es igual a la cantidad cotizacion
                                If rowCotizacion.CantidadCotizacion = rowCotizacion.CantidadRecibida Then
                                    rowCotizacion.CostoCotizacion = rowDocMarketing.LineTotalDocMarketing
                                    rowCotizacion.CantidadSolicitada = 0
                                ElseIf rowCotizacion.CantidadCotizacion = rowCotizacion.CantidadRecibida + rowDocMarketing.CantidadDocMarketing Then
                                    'If rowCotizacion.CantidadRecibida < rowDocMarketing.CantidadDocMarketing Then
                                    '    rowCotizacion.CostoCotizacion += rowDocMarketing.LineTotalDocMarketing
                                    'End If
                                    rowCotizacion.CostoCotizacion = rowDocMarketing.ResultadoCosto
                                    rowCotizacion.CantidadRecibida += rowCotizacion.CantidadSolicitada
                                    rowCotizacion.CantidadSolicitada = 0
                                ElseIf rowCotizacion.CantidadCotizacion < rowCotizacion.CantidadRecibida + rowDocMarketing.CantidadDocMarketing Then
                                    rowCotizacion.CantidadRecibida += rowCotizacion.CantidadSolicitada
                                    'rowCotizacion.CostoCotizacion += rowDocMarketing.LineTotalDocMarketing
                                    rowCotizacion.CostoCotizacion = rowDocMarketing.ResultadoCosto
                                    rowCotizacion.CantidadBackOrderDiferencia += ((rowCotizacion.CantidadRecibida + rowDocMarketing.CantidadDocMarketing) - rowCotizacion.CantidadCotizacion)
                                    rowCotizacion.CantidadSolicitada = 0
                                    ' Cantidad Documento Marketing es igual a la solicitada
                                ElseIf rowCotizacion.CantidadSolicitada = rowDocMarketing.CantidadDocMarketing Then
                                    rowCotizacion.CantidadRecibida += rowDocMarketing.CantidadDocMarketing
                                    rowCotizacion.CantidadSolicitada = 0
                                    'rowCotizacion.CostoCotizacion += rowDocMarketing.LineTotalDocMarketing
                                    rowCotizacion.CostoCotizacion = rowDocMarketing.ResultadoCosto
                                    ' Cantidad Documento Marketing es menor a la solicitada
                                ElseIf rowCotizacion.CantidadSolicitada > rowDocMarketing.CantidadDocMarketing Then
                                    If p_blnUsaBackOrder Then
                                        rowCotizacion.CantidadRecibida += rowDocMarketing.CantidadDocMarketing
                                        rowCotizacion.CantidadSolicitada = rowCotizacion.CantidadSolicitada - rowDocMarketing.CantidadDocMarketing
                                        rowCotizacion.CantidadBackOrderDiferencia += rowDocMarketing.CantidadBackOrderDiferencia
                                        'rowCotizacion.CostoCotizacion += rowDocMarketing.LineTotalDocMarketing
                                        rowCotizacion.CostoCotizacion = rowDocMarketing.ResultadoCosto
                                    Else
                                        rowCotizacion.CantidadRecibida += rowDocMarketing.CantidadDocMarketing
                                        rowCotizacion.CantidadSolicitada = rowCotizacion.CantidadSolicitada - rowDocMarketing.CantidadDocMarketing
                                        If ((rowDocMarketing.CantidadBackOrderDiferencia * -1) + rowCotizacion.CantidadRecibida) = rowCotizacion.CantidadCotizacion Then
                                            rowCotizacion.CantidadSolicitada = 0
                                        End If
                                        If rowDocMarketing.CantidadBackOrderDiferencia < 0 Then
                                            rowCotizacion.CantidadPendiente = (rowDocMarketing.CantidadBackOrderDiferencia * -1)
                                        End If
                                        'rowCotizacion.CostoCotizacion += rowDocMarketing.LineTotalDocMarketing
                                        rowCotizacion.CostoCotizacion = rowDocMarketing.ResultadoCosto
                                        rowCotizacion.CantidadBackOrderDiferencia += rowDocMarketing.CantidadBackOrderDiferencia
                                    End If
                                    ' Cantidad Documento Marketing es mayor a la solicitada
                                ElseIf rowCotizacion.CantidadSolicitada < rowDocMarketing.CantidadDocMarketing Then
                                    rowCotizacion.CantidadRecibida += rowCotizacion.CantidadSolicitada
                                    rowCotizacion.CantidadSolicitada = 0
                                    rowCotizacion.CantidadBackOrderDiferencia += rowDocMarketing.CantidadBackOrderDiferencia
                                    rowCotizacion.CostoCotizacion = rowDocMarketing.ResultadoCosto
                                End If
                            Case TipoDocumentoMarketing.NotaCredito

                            Case TipoDocumentoMarketing.EntradaMercancia

                            Case TipoDocumentoMarketing.DevolucionMercancia

                        End Select
                        p_oLineasCotizacionResultado.Add(New ListaLineasDocumentoMarketing() _
                                                                                               With {.NoOrden = rowCotizacion.NoOrden,
                                                                                                     .ItemCode = rowCotizacion.ItemCode,
                                                                                                     .TipoDocumentoMarketing = rowDocMarketing.TipoDocumentoMarketing,
                                                                                                     .CantidadCotizacion = rowCotizacion.CantidadCotizacion,
                                                                                                     .CantidadBackOrderDiferencia = rowCotizacion.CantidadBackOrderDiferencia,
                                                                                                     .CantidadRecibida = rowCotizacion.CantidadRecibida,
                                                                                                     .CantidadPendiente = rowCotizacion.CantidadPendiente,
                                                                                                     .CantidadSolicitada = rowCotizacion.CantidadSolicitada,
                                                                                                     .CantidadPendienteTraslado = rowCotizacion.CantidadPendienteTraslado,
                                                                                                     .CantidadPendienteDevolucion = rowCotizacion.CantidadPendienteDevolucion,
                                                                                                     .CantidadPendienteBodega = rowCotizacion.CantidadPendienteBodega,
                                                                                                     .CostoCotizacion = rowCotizacion.CostoCotizacion,
                                                                                                     .LineTotalDocMarketing = rowDocMarketing.LineTotalDocMarketing,
                                                                                                     .ID = rowCotizacion.ID,
                                                                                                     .IDRepuestosxOrden = rowCotizacion.IDRepuestosxOrden})

                        Exit For
                    End If
                Next
                Exit For
            Next
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
    Public Sub ProcesaDiferenciasDocumentoMarketingBase(ByVal p_oLineasDocumentoMarketing As List(Of ListaLineasDocumentoMarketing), _
                            ByRef p_oLineasDocumentoMarketingBaseDiferencias As List(Of ListaLineasDocumentoMarketing))
        Dim oDocumentoMarketingBase As SAPbobsCOM.Documents
        Try
            Dim oLineasDocumentoMarketingBase As New List(Of ListaLineasDocumentoMarketing)()
            Dim oListaDocEntryDocBase As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim intTipoDocumentoMarketingBase As Integer = 0
            Dim strItemCode As String = String.Empty
            Dim strIdItemListaDocMarketingBase As String = String.Empty
            Dim strIdItemDocMarketingBase As String = String.Empty
            Dim strNombreColumna As String = String.Empty
            Dim intResultado As Integer = 1
            Dim dblCantidadBackOrderDiferencia As Double = 0
            Dim blnCalculaDiferenciaLineTotal As Boolean = False
            Dim dblLineTotalDocBase As Double = 0
            For Each rowDocumentoMarketing As ListaLineasDocumentoMarketing In p_oLineasDocumentoMarketing
                oLineasDocumentoMarketingBase.Add(New ListaLineasDocumentoMarketing() _
                                                                                       With {.NoOrden = rowDocumentoMarketing.NoOrden,
                                                                                             .ItemCode = rowDocumentoMarketing.ItemCode,
                                                                                             .CantidadDocMarketing = rowDocumentoMarketing.CantidadDocMarketing,
                                                                                             .TipoDocumentoMarketingBase = rowDocumentoMarketing.TipoDocumentoMarketingBase,
                                                                                             .DocEntryDocMarketingBase = rowDocumentoMarketing.DocEntryDocMarketingBase,
                                                                                             .LineTotalDocMarketing = rowDocumentoMarketing.LineTotalDocMarketing,
                                                                                             .ID = rowDocumentoMarketing.ID,
                                                                                             .IDRepuestosxOrden = rowDocumentoMarketing.IDRepuestosxOrden})
                If Not oListaDocEntryDocBase.Contains(rowDocumentoMarketing.DocEntryDocMarketingBase) Then
                    oListaDocEntryDocBase.Add(rowDocumentoMarketing.DocEntryDocMarketingBase)
                End If
                If rowDocumentoMarketing.TipoDocumentoMarketingBase > 0 Then
                    intTipoDocumentoMarketingBase = rowDocumentoMarketing.TipoDocumentoMarketingBase
                End If
            Next
            Select Case intTipoDocumentoMarketingBase
                Case TipoDocumentoMarketingBase.OfertaCompra
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.OrdenCompra
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.EntradaMercancia
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                                                  SAPbobsCOM.Documents)
                    blnCalculaDiferenciaLineTotal = True
                Case TipoDocumentoMarketingBase.FacturaProveedor
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.NotaCredito
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes),  _
                                                               SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.DevolucionMercancia
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns),  _
                                                                                  SAPbobsCOM.Documents)
            End Select
            For Each docEntry As Integer In oListaDocEntryDocBase
                If oDocumentoMarketingBase.GetByKey(docEntry) Then
                    For row As Integer = 0 To oDocumentoMarketingBase.Lines.Count - 1
                        oDocumentoMarketingBase.Lines.SetCurrentLine(row)
                        For Each rowLinesDocMarketingBase As ListaLineasDocumentoMarketing In oLineasDocumentoMarketingBase
                            strIdItemDocMarketingBase = String.Empty
                            strIdItemListaDocMarketingBase = String.Empty
                            If Not String.IsNullOrEmpty(rowLinesDocMarketingBase.ID) Then
                                strIdItemListaDocMarketingBase = rowLinesDocMarketingBase.ID
                                strNombreColumna = "U_SCGD_ID"
                            ElseIf Not String.IsNullOrEmpty(rowLinesDocMarketingBase.IDRepuestosxOrden) Then
                                strIdItemListaDocMarketingBase = rowLinesDocMarketingBase.IDRepuestosxOrden
                                strNombreColumna = "U_SCGD_IdRepxOrd"
                            End If
                            If Not String.IsNullOrEmpty(oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value) Then
                                strIdItemDocMarketingBase = oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value.ToString.Trim()
                            End If
                            If strIdItemDocMarketingBase = strIdItemListaDocMarketingBase And oDocumentoMarketingBase.Lines.ItemCode = rowLinesDocMarketingBase.ItemCode Then
                                If oDocumentoMarketingBase.Lines.Quantity <> rowLinesDocMarketingBase.CantidadDocMarketing Then
                                    dblCantidadBackOrderDiferencia = rowLinesDocMarketingBase.CantidadDocMarketing - oDocumentoMarketingBase.Lines.Quantity
                                Else
                                    dblCantidadBackOrderDiferencia = 0
                                End If
                                If blnCalculaDiferenciaLineTotal Then
                                    dblLineTotalDocBase = oDocumentoMarketingBase.Lines.LineTotal
                                Else
                                    dblLineTotalDocBase = 0
                                End If
                                p_oLineasDocumentoMarketingBaseDiferencias.Add(New ListaLineasDocumentoMarketing() _
                                                                                          With {.NoOrden = rowLinesDocMarketingBase.NoOrden,
                                                                                                .ItemCode = rowLinesDocMarketingBase.ItemCode,
                                                                                                .IdItem = strIdItemListaDocMarketingBase,
                                                                                                .CantidadBackOrderDiferencia = dblCantidadBackOrderDiferencia,
                                                                                                .LineTotalDocMarketing = rowLinesDocMarketingBase.LineTotalDocMarketing,
                                                                                                .LineTotalDocBaseMarketing = dblLineTotalDocBase})

                                Exit For
                            End If
                        Next
                    Next
                End If
            Next
        Catch ex As Exception
        Finally
            If Not oDocumentoMarketingBase Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketingBase)
                oDocumentoMarketingBase = Nothing
            End If
        End Try
    End Sub
    Public Sub ProcesaBackOrder(ByVal p_oLineasDocumentoMarketing As List(Of ListaLineasDocumentoMarketing))
        Dim oDocumentoMarketingBase As SAPbobsCOM.Documents
        Try
            Dim oLineasDocumentoMarketingBase As New List(Of ListaLineasDocumentoMarketing)()
            Dim oListaDocEntryDocBase As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim intTipoDocumentoMarketingBase As Integer = 0
            Dim strItemCode As String = String.Empty
            Dim strIdItemListaDocMarketingBase As String = String.Empty
            Dim strIdItemDocMarketingBase As String = String.Empty
            Dim strNombreColumna As String = String.Empty
            Dim intResultado As Integer = 1
            Dim blnActualizaDocumentoMarketingBase As Boolean = False
            For Each rowDocumentoMarketing As ListaLineasDocumentoMarketing In p_oLineasDocumentoMarketing
                oLineasDocumentoMarketingBase.Add(New ListaLineasDocumentoMarketing() _
                                                                                       With {.NoOrden = rowDocumentoMarketing.NoOrden,
                                                                                             .ItemCode = rowDocumentoMarketing.ItemCode,
                                                                                             .CantidadDocMarketing = rowDocumentoMarketing.CantidadDocMarketing,
                                                                                             .TipoDocumentoMarketingBase = rowDocumentoMarketing.TipoDocumentoMarketingBase,
                                                                                             .DocEntryDocMarketingBase = rowDocumentoMarketing.DocEntryDocMarketingBase,
                                                                                             .ID = rowDocumentoMarketing.ID,
                                                                                             .IDRepuestosxOrden = rowDocumentoMarketing.IDRepuestosxOrden})
                If Not oListaDocEntryDocBase.Contains(rowDocumentoMarketing.DocEntryDocMarketingBase) Then
                    oListaDocEntryDocBase.Add(rowDocumentoMarketing.DocEntryDocMarketingBase)
                End If
                If rowDocumentoMarketing.TipoDocumentoMarketingBase > 0 Then
                    intTipoDocumentoMarketingBase = rowDocumentoMarketing.TipoDocumentoMarketingBase
                End If
            Next
            Select Case intTipoDocumentoMarketingBase
                Case TipoDocumentoMarketingBase.OfertaCompra
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.OrdenCompra
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.EntradaMercancia
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                                                  SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.FacturaProveedor
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                                   SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.NotaCredito
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes),  _
                                                               SAPbobsCOM.Documents)
                Case TipoDocumentoMarketingBase.DevolucionMercancia
                    oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns),  _
                                                                                  SAPbobsCOM.Documents)
            End Select
            For Each docEntry As Integer In oListaDocEntryDocBase
                If oDocumentoMarketingBase.GetByKey(docEntry) Then
                    For row As Integer = 0 To oDocumentoMarketingBase.Lines.Count - 1
                        oDocumentoMarketingBase.Lines.SetCurrentLine(row)
                        For Each rowLinesDocMarketingBase As ListaLineasDocumentoMarketing In oLineasDocumentoMarketingBase
                            strIdItemDocMarketingBase = String.Empty
                            strIdItemListaDocMarketingBase = String.Empty
                            If Not String.IsNullOrEmpty(rowLinesDocMarketingBase.ID) Then
                                strIdItemListaDocMarketingBase = rowLinesDocMarketingBase.ID
                                strNombreColumna = "U_SCGD_ID"
                            ElseIf Not String.IsNullOrEmpty(rowLinesDocMarketingBase.IDRepuestosxOrden) Then
                                strIdItemListaDocMarketingBase = rowLinesDocMarketingBase.IDRepuestosxOrden
                                strNombreColumna = "U_SCGD_IdRepxOrd"
                            End If
                            If Not String.IsNullOrEmpty(oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value) Then
                                strIdItemDocMarketingBase = oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value.ToString.Trim()
                            End If
                            If strIdItemDocMarketingBase = strIdItemListaDocMarketingBase And oDocumentoMarketingBase.Lines.ItemCode = rowLinesDocMarketingBase.ItemCode Then
                                If oDocumentoMarketingBase.Lines.Quantity > rowLinesDocMarketingBase.CantidadDocMarketing Then
                                    oDocumentoMarketingBase.Lines.LineStatus = BoStatus.bost_Close
                                    blnActualizaDocumentoMarketingBase = True
                                End If
                                Exit For
                            End If
                        Next
                    Next
                    If blnActualizaDocumentoMarketingBase Then
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                        End If
                        If Not m_oCompany.InTransaction Then
                            intResultado = 1
                            m_oCompany.StartTransaction()
                            intResultado = oDocumentoMarketingBase.Update()
                        End If
                        If intResultado <> 0 Then
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                            End If
                        Else
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                            End If
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If Not oDocumentoMarketingBase Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketingBase)
                oDocumentoMarketingBase = Nothing
            End If
        End Try
    End Sub
    Public Sub CargarDocEntryCotizacion(ByVal p_oListaNoOrden As Generic.List(Of String), ByRef p_oListaCotizacion As Generic.List(Of String))
        Try
            Dim strNoOrden As String = String.Empty
            Dim strQuery As String = String.Empty
            Dim dtCotizacion As System.Data.DataTable
            Dim intDocEntry As Integer = 0

            For Each rowOT As String In p_oListaNoOrden
                If Not strNoOrden.Contains(rowOT) Then
                    strNoOrden = strNoOrden & String.Format("'{0}', ", rowOT)
                End If
            Next
            If (strNoOrden.Length > 0) Then
                strNoOrden = strNoOrden.Substring(0, strNoOrden.Length - 2)
                strQuery = String.Format("select Q.DocEntry from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT in ({0})", strNoOrden)
                dtCotizacion = Utilitarios.EjecutarConsultaDataTable(strQuery, m_oCompany.CompanyDB, m_oCompany.Server)
            End If
            For Each rowCotizacion As DataRow In dtCotizacion.Rows
                If Not String.IsNullOrEmpty(rowCotizacion.Item("DocEntry")) Then
                    If Not p_oListaCotizacion.Contains(rowCotizacion.Item("DocEntry")) Then
                        p_oListaCotizacion.Add(rowCotizacion.Item("DocEntry"))
                    End If
                End If
            Next
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
#End Region
#Region "FacturaProveedores"

#End Region

#Region "Orden de compra"
    Public Sub ManejaOrdenCompra(ByRef p_strDocEntry As String)
        Try
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                If ValidarOTInterna() Then
                    ProcesaOrdenCompra(p_strDocEntry)
                End If
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ProcesaOrdenCompra(ByVal p_strDocEntry As String)
        Try
            '**********DataContract****************
            Dim oLineaOrdenCompraList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oDatosGeneralesList As DatoGenerico_List = New DatoGenerico_List
            '********Listas genericas*************
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            '**********Declaración Variables*****************
            Dim blnProcesaOrdenCompra As Boolean = False
            '********Carga información lineas de entrada mercancia*************
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                blnProcesaOrdenCompra = CargaOrdenCompra(CInt(p_strDocEntry), oLineaOrdenCompraList, oNoOrdenList, oDatosGeneralesList)
            End If
            If blnProcesaOrdenCompra Then
                '**********************************************
                '*********** Recorre Documentos Marketing******
                '**********************************************
                'clsDocumentoProcesoCompra.ManejarBackOrder(oLineaEntradaMercanciaList, oConfiguracionGeneralList, oBaseEntryList)
                '**********************************************
                '*********** Maneja Tracking******
                '**********************************************
                ManejarTracking(oNoOrdenList, oLineaOrdenCompraList, oDatosGeneralesList, TipoDocumentoMarketingBase.OrdenCompra)
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        End Try
    End Sub

    Public Function CargaOrdenCompra(ByVal p_intDocEntry As Integer, _
                                     ByRef p_oLineaOrdenCompraList As DocumentoMarketing_List, _
                                     ByRef p_oNoOrdenList As Generic.List(Of String), _
                                     ByRef p_oDatosGeneralesList As DatoGenerico_List) As Boolean
        Dim oOrdenCompra As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaOrdenCompra As DocumentoMarketing
            Dim oDatosGenerales As DatoGenerico
            '************Variables********************************
            Dim strNoOrden As String = String.Empty
            Dim blnProcesaOrdenCompra As Boolean = False
            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oOrdenCompra = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders),  _
                                                     SAPbobsCOM.Documents)
                '************Carga Objeto Orden Compra********************************
                If oOrdenCompra.GetByKey(p_intDocEntry) Then
                    oDatosGenerales = New DatoGenerico
                    With oDatosGenerales
                        .DocEntry = oOrdenCompra.DocEntry
                        .DocNum = oOrdenCompra.DocNum
                        .FechaContabilizacion = oOrdenCompra.DocDate
                        .FechaCreacion = oOrdenCompra.CreationDate
                        .CardCode = oOrdenCompra.CardCode
                        .CardName = oOrdenCompra.CardName
                        .Observaciones = oOrdenCompra.Comments
                        If Not String.IsNullOrEmpty(oOrdenCompra.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            .NoOrden = oOrdenCompra.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()
                        End If
                    End With
                    p_oDatosGeneralesList.Add(oDatosGenerales)
                    '********Recorre lineas de la Orden Compra***********************
                    For rowOrdenCompra As Integer = 0 To oOrdenCompra.Lines.Count - 1
                        oOrdenCompra.Lines.SetCurrentLine(rowOrdenCompra)
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            oLineaOrdenCompra = New DocumentoMarketing()
                            With oLineaOrdenCompra
                                .ItemCode = oOrdenCompra.Lines.ItemCode
                                .ItemDescripcion = oOrdenCompra.Lines.ItemDescription
                                .Cantidad = oOrdenCompra.Lines.Quantity
                                .BaseDocType = oOrdenCompra.Lines.BaseType
                                .BaseDocEntry = oOrdenCompra.Lines.BaseEntry
                                If Not String.IsNullOrEmpty(oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                If Not String.IsNullOrEmpty(oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then
                                    .IdRepxOrd = oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not String.IsNullOrEmpty(oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    .ID = oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                            End With
                            p_oLineaOrdenCompraList.Add(oLineaOrdenCompra)
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oOrdenCompra.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            blnProcesaOrdenCompra = True
                        End If
                    Next
                End If
            End If
            Return blnProcesaOrdenCompra
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oOrdenCompra)
        End Try
    End Function

    Public Function ValidarOTInterna() As Boolean
        Try
            Return Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function
#End Region

#Region "Entradas Mercancia"
    Public Sub ManejarBackOrder(ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List, _
                                ByRef p_oBaseEntryList As Generic.List(Of Integer))
        Try
            '**********Variables *************
            Dim intBaseType As Integer = 0

            ' Valida si se utiliza back order a nivel general
            If Not p_oConfiguracionGeneralList.Item(0).UsaBackOrder Then
                '*********Se elige cual es el documento de marketing base********
                intBaseType = p_oLineaFacturaProveedorList.Item(0).BaseDocType
                If intBaseType > 0 Then
                    m_SBOApplication.StatusBar.SetText(My.Resources.Resource.ProcesaBackOrder, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    ProcesaBackOrderDocMarketing(p_oLineaFacturaProveedorList, intBaseType, p_oBaseEntryList)
                End If
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

#End Region

#Region "Metodos Generales Nuevos"
    Public Sub ProcesaBackOrderDocMarketing(ByRef p_oLineasDocumentoMarketingList As DocumentoMarketing_List, _
                                            ByRef p_intTipoDocumentoMarketingBase As Integer, _
                                            ByRef p_oBaseEntryList As List(Of Integer))
        Dim oDocumentoMarketingBase As SAPbobsCOM.Documents
        Try
            '*************Objetos SAP *******************
            Dim oListaDocumentoMarketing As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '**************Variables **************************
            Dim strIdItemDocMarketing As String = String.Empty
            Dim strIdItemDocMarketingBase As String = String.Empty
            Dim strNombreColumna As String = String.Empty
            Dim blnActualizaDocumentoMarketingBase As Boolean = False
            Dim intResultado As Integer = 1

            For Each DocEntry As Integer In p_oBaseEntryList
                Select Case p_intTipoDocumentoMarketingBase
                    Case TipoDocumentoMarketingBase.OfertaCompra
                        oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations),  _
                                                                                       SAPbobsCOM.Documents)
                    Case TipoDocumentoMarketingBase.OrdenCompra
                        oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders),  _
                                                                                       SAPbobsCOM.Documents)
                    Case TipoDocumentoMarketingBase.EntradaMercancia
                        oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                                                      SAPbobsCOM.Documents)
                    Case TipoDocumentoMarketingBase.FacturaProveedor
                        oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices),  _
                                                                                       SAPbobsCOM.Documents)
                    Case TipoDocumentoMarketingBase.NotaCredito
                        oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes),  _
                                                                   SAPbobsCOM.Documents)
                    Case TipoDocumentoMarketingBase.DevolucionMercancia
                        oDocumentoMarketingBase = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns),  _
                                                                                      SAPbobsCOM.Documents)
                End Select
                If oDocumentoMarketingBase.GetByKey(DocEntry) Then
                    blnActualizaDocumentoMarketingBase = False
                    For row As Integer = 0 To oDocumentoMarketingBase.Lines.Count - 1
                        oDocumentoMarketingBase.Lines.SetCurrentLine(row)
                        For Each rowLinesDocMarketingBase As DocumentoMarketing In p_oLineasDocumentoMarketingList
                            strIdItemDocMarketingBase = String.Empty
                            strIdItemDocMarketing = String.Empty
                            If Not String.IsNullOrEmpty(rowLinesDocMarketingBase.ID) Then
                                strIdItemDocMarketing = rowLinesDocMarketingBase.ID
                                strNombreColumna = "U_SCGD_ID"
                            ElseIf Not String.IsNullOrEmpty(rowLinesDocMarketingBase.IdRepxOrd) Then
                                strIdItemDocMarketing = rowLinesDocMarketingBase.IdRepxOrd.ToString.Trim()
                                strNombreColumna = "U_SCGD_IdRepxOrd"
                            End If
                            If Not String.IsNullOrEmpty(oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value) Then
                                strIdItemDocMarketingBase = oDocumentoMarketingBase.Lines.UserFields.Fields.Item(strNombreColumna).Value.ToString.Trim()
                            End If
                            If strIdItemDocMarketingBase = strIdItemDocMarketing And oDocumentoMarketingBase.Lines.ItemCode = rowLinesDocMarketingBase.ItemCode Then
                                If oDocumentoMarketingBase.Lines.Quantity > rowLinesDocMarketingBase.Cantidad Then
                                    oDocumentoMarketingBase.Lines.LineStatus = BoStatus.bost_Close
                                    blnActualizaDocumentoMarketingBase = True
                                End If
                                Exit For
                            End If
                        Next
                    Next
                    If blnActualizaDocumentoMarketingBase Then
                        oListaDocumentoMarketing.Add(oDocumentoMarketingBase)
                    End If
                End If
            Next
            '****************Manejo Transaccion SAP ********************
            If oListaDocumentoMarketing.Count > 0 Then
                ResetTransaction()
                StartTransaction()
                For Each rowDocumentoMarketingBase As SAPbobsCOM.Documents In oListaDocumentoMarketing
                    intResultado = rowDocumentoMarketingBase.Update()
                    If intResultado <> 0 Then
                        RollbackTransaction()
                        Exit Sub
                    End If
                Next
                CommitTransaction()
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        Finally
            Utilitarios.DestruirObjeto(oDocumentoMarketingBase)
        End Try
    End Sub

    Public Sub StartTransaction()
        Try
            If Not m_oCompany.InTransaction Then
                m_oCompany.StartTransaction()
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ResetTransaction()
        Try
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CommitTransaction()
        Try
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub RollbackTransaction()
        Try
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ManejarTracking(ByRef p_oNoOrdenList As List(Of String), _
                               ByRef p_oLineasDocMarketingList As DocumentoMarketing_List, _
                               ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                               ByRef p_intTipoDocMarketing As Integer)
        Try
            If p_oNoOrdenList.Count > 0 And p_oLineasDocMarketingList.Count > 0 Then
                AgregarLineaTracking(p_oNoOrdenList, p_oLineasDocMarketingList, p_oDatosGeneralesList, p_intTipoDocMarketing)
            End If
        Catch ex As Exception
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AgregarLineaTracking(ByRef p_oNoOrdenList As List(Of String), _
                                    ByRef p_oLineasDocMarketingList As DocumentoMarketing_List, _
                                    ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                    ByRef p_intTipoDocMarketing As Integer)
        Dim intErrorCode As Integer = 0
        Dim strErrorMessage As String = String.Empty
        Try
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oChildOT As SAPbobsCOM.GeneralData
            Dim oChildrenOT As SAPbobsCOM.GeneralDataCollection
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oGeneralDataList As List(Of SAPbobsCOM.GeneralData) = New List(Of SAPbobsCOM.GeneralData)

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            For Each rowNoOrden As String In p_oNoOrdenList
                oGeneralParams.SetProperty("Code", rowNoOrden)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oChildrenOT = oGeneralData.Child("SCGD_TRACKXOT")
                For Each rowLinea As DocumentoMarketing In p_oLineasDocMarketingList
                    If Not rowLinea.TrackingAplicado Then
                        If rowLinea.NoOrden = rowNoOrden Then
                            oChildOT = oChildrenOT.Add()
                            If Not String.IsNullOrEmpty(rowLinea.NoOrden) Then oChildOT.SetProperty("U_NoOrden", rowLinea.NoOrden)
                            If Not String.IsNullOrEmpty(rowLinea.ItemCode) Then oChildOT.SetProperty("U_ItemCode", rowLinea.ItemCode)
                            If Not String.IsNullOrEmpty(rowLinea.ItemDescripcion) Then oChildOT.SetProperty("U_Descripcion", rowLinea.ItemDescripcion)
                            If Not String.IsNullOrEmpty(rowLinea.ID) Then
                                oChildOT.SetProperty("U_ID", rowLinea.ID)
                            ElseIf Not String.IsNullOrEmpty(rowLinea.IdRepxOrd) Then
                                oChildOT.SetProperty("U_ID", rowLinea.IdRepxOrd)
                            End If
                            '*********Fecha Documento *****************
                            If p_oDatosGeneralesList.Item(0).FechaCreacion <> Nothing Then
                                oChildOT.SetProperty("U_FechaDoc", p_oDatosGeneralesList.Item(0).FechaCreacion)
                            Else
                                oChildOT.SetProperty("U_FechaDoc", Date.Now)
                            End If
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).CardCode) Then oChildOT.SetProperty("U_CardCode", p_oDatosGeneralesList.Item(0).CardCode)
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).CardName) Then oChildOT.SetProperty("U_CardName", p_oDatosGeneralesList.Item(0).CardName)
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).DocEntry) Then oChildOT.SetProperty("U_DocEntry", p_oDatosGeneralesList.Item(0).DocEntry)
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).DocNum) Then oChildOT.SetProperty("U_DocNum", p_oDatosGeneralesList.Item(0).DocNum)
                            Select Case p_intTipoDocMarketing
                                Case TipoDocumentoMarketingBase.OfertaCompra
                                    oChildOT.SetProperty("U_CanSol", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_CanRec", 0)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.OfertaCompra)
                                Case TipoDocumentoMarketingBase.OrdenCompra
                                    oChildOT.SetProperty("U_CanSol", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_CanRec", 0)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.OrdenCompra)
                                Case TipoDocumentoMarketingBase.EntradaMercancia
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.EntradaMercancia)
                                Case TipoDocumentoMarketingBase.FacturaProveedor
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.FacturaProveedor)
                                Case TipoDocumentoMarketingBase.NotaCredito
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad * -1)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.NotaCredito)
                                Case TipoDocumentoMarketingBase.DevolucionMercancia
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad * -1)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.DevolucionMercancia)
                            End Select
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).Observaciones) Then oChildOT.SetProperty("U_Observ", p_oDatosGeneralesList.Item(0).Observaciones)
                            rowLinea.TrackingAplicado = True
                        End If
                    End If
                Next
                oGeneralDataList.Add(oGeneralData)
            Next
            ResetTransaction()
            StartTransaction()
            For Each rowoGeneralData As SAPbobsCOM.GeneralData In oGeneralDataList
                oGeneralService.Update(rowoGeneralData)
            Next
            CommitTransaction()
        Catch ex As Exception
            m_oCompany.GetLastError(intErrorCode, strErrorMessage)
            m_SBOApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            m_SBOApplication.StatusBar.SetText(strErrorMessage, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        End Try
    End Sub

    Public Function ManejarTrackingOT(ByRef p_oNoOrdenList As List(Of String), _
                               ByRef p_oLineasDocMarketingList As DocumentoMarketing_List, _
                               ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                               ByRef p_intTipoDocMarketing As Integer, ByRef p_oGeneralDataList As List(Of SAPbobsCOM.GeneralData), _
                               ByRef p_blnCancela As Boolean) As Boolean
        Try
            If p_oNoOrdenList.Count > 0 And p_oLineasDocMarketingList.Count > 0 Then
                If Not AgregarLineaTrackingOT(p_oNoOrdenList, p_oLineasDocMarketingList, p_oDatosGeneralesList, p_intTipoDocMarketing, p_oGeneralDataList, p_blnCancela) Then Return False
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function AgregarLineaTrackingOT(ByRef p_oNoOrdenList As List(Of String), _
                                    ByRef p_oLineasDocMarketingList As DocumentoMarketing_List, _
                                    ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                    ByRef p_intTipoDocMarketing As Integer, ByRef p_oGeneralDataList As List(Of SAPbobsCOM.GeneralData), _
                                    ByRef p_blnCancela As Boolean) As Boolean
        Dim intErrorCode As Integer = 0
        Dim strErrorMessage As String = String.Empty
        Try
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oChildOT As SAPbobsCOM.GeneralData
            Dim oChildrenOT As SAPbobsCOM.GeneralDataCollection
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            'Dim oGeneralDataList As List(Of SAPbobsCOM.GeneralData) = New List(Of SAPbobsCOM.GeneralData)

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            For Each rowNoOrden As String In p_oNoOrdenList
                oGeneralParams.SetProperty("Code", rowNoOrden)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oChildrenOT = oGeneralData.Child("SCGD_TRACKXOT")
                For Each rowLinea As DocumentoMarketing In p_oLineasDocMarketingList
                    If Not rowLinea.TrackingAplicado Then
                        If rowLinea.NoOrden = rowNoOrden Then
                            oChildOT = oChildrenOT.Add()
                            If Not String.IsNullOrEmpty(rowLinea.NoOrden) Then oChildOT.SetProperty("U_NoOrden", rowLinea.NoOrden)
                            If Not String.IsNullOrEmpty(rowLinea.ItemCode) Then oChildOT.SetProperty("U_ItemCode", rowLinea.ItemCode)
                            If Not String.IsNullOrEmpty(rowLinea.ItemDescripcion) Then oChildOT.SetProperty("U_Descripcion", rowLinea.ItemDescripcion)
                            If Not String.IsNullOrEmpty(rowLinea.ID) Then
                                oChildOT.SetProperty("U_ID", rowLinea.ID)
                            ElseIf Not String.IsNullOrEmpty(rowLinea.IdRepxOrd) Then
                                oChildOT.SetProperty("U_ID", rowLinea.IdRepxOrd)
                            End If
                            '*********Fecha Documento *****************
                            If p_oDatosGeneralesList.Item(0).FechaCreacion <> Nothing Then
                                oChildOT.SetProperty("U_FechaDoc", p_oDatosGeneralesList.Item(0).FechaCreacion)
                            Else
                                oChildOT.SetProperty("U_FechaDoc", Date.Now)
                            End If
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).CardCode) Then oChildOT.SetProperty("U_CardCode", p_oDatosGeneralesList.Item(0).CardCode)
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).CardName) Then oChildOT.SetProperty("U_CardName", p_oDatosGeneralesList.Item(0).CardName)
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).DocEntry) Then oChildOT.SetProperty("U_DocEntry", p_oDatosGeneralesList.Item(0).DocEntry)
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).DocNum) Then oChildOT.SetProperty("U_DocNum", p_oDatosGeneralesList.Item(0).DocNum)
                            Select Case p_intTipoDocMarketing
                                Case TipoDocumentoMarketingBase.OfertaCompra
                                    oChildOT.SetProperty("U_CanSol", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_CanRec", 0)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.OfertaCompra)
                                Case TipoDocumentoMarketingBase.OrdenCompra
                                    oChildOT.SetProperty("U_CanSol", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_CanRec", 0)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.OrdenCompra)
                                Case TipoDocumentoMarketingBase.EntradaMercancia
                                    If p_blnCancela Then
                                        oChildOT.SetProperty("U_CanSol", 0)
                                        oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad * -1)
                                        oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.EntradaMercancia)
                                    Else
                                        oChildOT.SetProperty("U_CanSol", 0)
                                        oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad)
                                        oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.EntradaMercancia)
                                    End If
                                Case TipoDocumentoMarketingBase.FacturaProveedor
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.FacturaProveedor)
                                Case TipoDocumentoMarketingBase.NotaCredito
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad * -1)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.NotaCredito)
                                Case TipoDocumentoMarketingBase.DevolucionMercancia
                                    oChildOT.SetProperty("U_CanSol", 0)
                                    oChildOT.SetProperty("U_CanRec", rowLinea.Cantidad * -1)
                                    oChildOT.SetProperty("U_TipoDoc", TipoDocumentoMarketingBase.DevolucionMercancia)
                            End Select
                            If Not String.IsNullOrEmpty(p_oDatosGeneralesList.Item(0).Observaciones) Then oChildOT.SetProperty("U_Observ", p_oDatosGeneralesList.Item(0).Observaciones)
                            rowLinea.TrackingAplicado = True
                        End If
                    End If
                Next
                p_oGeneralDataList.Add(oGeneralData)
            Next
            'ResetTransaction()
            'StartTransaction()
            'For Each rowoGeneralData As SAPbobsCOM.GeneralData In oGeneralDataList
            '    oGeneralService.Update(rowoGeneralData)
            'Next
            'CommitTransaction()
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function
#End Region
#End Region
End Class
' Clase para la definición de la lista
Public Class ListaLineasDocumentoMarketing
    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property TipoDocumentoMarketing() As Integer
        Get
            Return intTipoDocumentoMarketing
        End Get
        Set(ByVal value As Integer)
            intTipoDocumentoMarketing = value
        End Set
    End Property
    Private intTipoDocumentoMarketing As Integer

    Public Property TipoDocumentoMarketingBase() As Integer
        Get
            Return intTipoDocumentoMarketingBase
        End Get
        Set(ByVal value As Integer)
            intTipoDocumentoMarketingBase = value
        End Set
    End Property
    Private intTipoDocumentoMarketingBase As Integer

    Public Property DocEntryDocMarketingBase() As Integer
        Get
            Return intDocEntryDocMarketingBase
        End Get
        Set(ByVal value As Integer)
            intDocEntryDocMarketingBase = value
        End Set
    End Property
    Private intDocEntryDocMarketingBase As Integer


    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(ByVal value As String)
            strID = value
        End Set
    End Property
    Private strID As String

    Public Property IDRepuestosxOrden() As String
        Get
            Return strIDRepuestosxOrden
        End Get
        Set(ByVal value As String)
            strIDRepuestosxOrden = value
        End Set
    End Property
    Private strIDRepuestosxOrden As String

    Public Property TipoArticulo() As String
        Get
            Return strTipoArticulo
        End Get
        Set(ByVal value As String)
            strTipoArticulo = value
        End Set
    End Property
    Private strTipoArticulo As String

    Public Property IdItem() As String
        Get
            Return strIdItem
        End Get
        Set(ByVal value As String)
            strIdItem = value
        End Set
    End Property
    Private strIdItem As String

    Public Property CostoCotizacion() As Double
        Get
            Return dblCostoCotizacion
        End Get
        Set(ByVal value As Double)
            dblCostoCotizacion = value
        End Set
    End Property
    Private dblCostoCotizacion As Double

    Public Property ResultadoCosto() As Double
        Get
            Return dblResultadoCosto
        End Get
        Set(ByVal value As Double)
            dblResultadoCosto = value
        End Set
    End Property
    Private dblResultadoCosto As Double

    Public Property LineTotalDocBaseMarketing() As Double
        Get
            Return dblLineTotalDocBaseMarketing
        End Get
        Set(ByVal value As Double)
            dblLineTotalDocBaseMarketing = value
        End Set
    End Property
    Private dblLineTotalDocBaseMarketing As Double

    Public Property LineTotalDocMarketing() As Double
        Get
            Return dblLineTotalDocMarketing
        End Get
        Set(ByVal value As Double)
            dblLineTotalDocMarketing = value
        End Set
    End Property
    Private dblLineTotalDocMarketing As Double

    Public Property CantidadCotizacion() As Double
        Get
            Return dblCantidadCotizacion
        End Get
        Set(ByVal value As Double)
            dblCantidadCotizacion = value
        End Set
    End Property
    Private dblCantidadCotizacion As Double

    Public Property CantidadDocMarketing() As Double
        Get
            Return dblCantidadDocMarketing
        End Get
        Set(ByVal value As Double)
            dblCantidadDocMarketing = value
        End Set
    End Property
    Private dblCantidadDocMarketing As Double

    Public Property CantidadDocMarketingBase() As Double
        Get
            Return dblCantidadDocMarketingBase
        End Get
        Set(ByVal value As Double)
            dblCantidadDocMarketingBase = value
        End Set
    End Property
    Private dblCantidadDocMarketingBase As Double

    Public Property CantidadBackOrderDiferencia() As Double
        Get
            Return dblCantidadBackOrderDiferencia
        End Get
        Set(ByVal value As Double)
            dblCantidadBackOrderDiferencia = value
        End Set
    End Property
    Private dblCantidadBackOrderDiferencia As Double

    Public Property CantidadRecibida() As Double
        Get
            Return dblCantidadRecibida
        End Get
        Set(ByVal value As Double)
            dblCantidadRecibida = value
        End Set
    End Property
    Private dblCantidadRecibida As Double

    Public Property CantidadSolicitada() As Double
        Get
            Return dblCantidadSolicitada
        End Get
        Set(ByVal value As Double)
            dblCantidadSolicitada = value
        End Set
    End Property
    Private dblCantidadSolicitada As Double

    Public Property CantidadPendiente() As Double
        Get
            Return dblCantidadPendiente
        End Get
        Set(ByVal value As Double)
            dblCantidadPendiente = value
        End Set
    End Property
    Private dblCantidadPendiente As Double

    Public Property CantidadPendienteBodega() As Double
        Get
            Return dblCantidadPendienteBodega
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteBodega = value
        End Set
    End Property
    Private dblCantidadPendienteBodega As Double

    Public Property CantidadPendienteTraslado() As Double
        Get
            Return dblCantidadPendienteTraslado
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteTraslado = value
        End Set
    End Property
    Private dblCantidadPendienteTraslado As Double

    Public Property CantidadPendienteDevolucion() As Double
        Get
            Return dblCantidadPendienteDevolucion
        End Get
        Set(ByVal value As Double)
            dblCantidadPendienteDevolucion = value
        End Set
    End Property
    Private dblCantidadPendienteDevolucion As Double

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

End Class
