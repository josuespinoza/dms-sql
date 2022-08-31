Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports SCG.DMSOne.Framework
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Threading
Imports SAPbouiCOM
Imports DMS_Connector.Business_Logic.DataContract.SAPDocumento
Imports SAPbobsCOM


Partial Public Class Cotizacion_ProcesaOT
#Region "Declaraciones"
#Region "Enumeradores"
    '*********************************
    'Cambios Procesamiento OT
    '*********************************
    Private Enum ArticuloAprobado
        scgSi = 1
        scgNo = 2
        scgFalta = 3
        scgCambioOT = 4
    End Enum
    Private Enum TipoProcesamiento
        Crear = 1
        Actualizar = 2
        OTEspecial = 3
    End Enum

    Private Enum TipoMovimiento
        Requisicion = 1
        Comprar = 2
        Trasladar = 3
        Rechazar = 4
    End Enum

    Private Enum enumTrasladadoOTHija

        scgOTHijaSI = 1
        scgOTHijaNO = 2

    End Enum

    Private Enum GeneraOT
        SI = 1
        NO = 2
    End Enum

    Private Enum TipoArticulo
        Repuesto = 1
        Servicio = 2
        Suministro = 3
        ServicioExterno = 4
        Paquete = 5
        Otros = 6
        Accesorio = 7
        Vehiculo = 8
        Tramite = 9
        ArticuloCita = 10
        OtrosCostos = 11
        OtrosIngresos = 12
    End Enum

    Private Enum Trasladado
        NoProcesado = 0
        NO = 1
        SI = 2
        PendienteTraslado = 3
        PendienteBodega = 4
    End Enum

    Private Enum CodigoEstadoRequisicion
        Pendiente = 1
        Trasladado = 2
    End Enum

    Private Enum TipoBodega
        BodegaOrigen = 1
        BodegaDestino = 2
    End Enum

    Private Enum ImprimeOR
        SI = 1
        NO = 2
    End Enum

    Private Enum TipoRequisicion
        Traslado = 1
        Devolucion = 2
        Reserva = 3
        DevolucionReserva = 4
    End Enum

    Private Enum CampoID
        ID = 1
        IdRepxOrd = 2
    End Enum

    Private Enum TipoDocumentoMarketing
        OfertaCompra = 540000006
        OrdenCompra = 22
        EntradaMercancia = 20
        FacturaProveedor = 18
    End Enum

    Private Enum ProcesamientoLinea
        Requisicion = 1
        RequisicionDevolucion = 2
        NingunaAccion = 3
        TrasladoBodega = 4
        AgregarControlColaborador = 5
        EliminarControlColaborador = 6
        AnularRequisicion = 7
        AnularTrasladoBodega = 8
        ProcesaServicioExterno = 9
        AnulaServicioExterno = 10
        AnularRequisicionDevolucion = 11
    End Enum
#End Region
#Region "Objetos SAP"
    Private WithEvents SBO_Application As SAPbouiCOM.Application
    Private m_oCotizacionAnterior As SAPbobsCOM.Documents
    Private m_oCotizacion As SAPbobsCOM.Documents
    Private m_objCotizacionPadre As SAPbobsCOM.Documents
    Private m_oCompany As SAPbobsCOM.Company
    Private m_oForm As SAPbouiCOM.Form

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private oCotizacionlocal As SAPbobsCOM.Documents

#End Region
#Region "DataContract"
    'Private oCotizacionInicial As Cotizacion
    Dim oRecepcionList As Recepcion_List
    Private oCotizacionInicial As oDocumento
#End Region
#Region "Variables"

    'Cambio Proceso Cotizacion
    Dim blnProcesaCotizacion As Boolean = False
    Private m_strCentroCosto As String
    Public g_strCreaHjaCanPend As String = String.Empty

    Public Shared NoOT As String
    Public Shared IdSucursal As String
#End Region
#Region "Constantes"
    Public n As NumberFormatInfo
    Private Const mc_strIDBotonEjecucion As String = "1"
    Private Const mc_strCboTipoPago As String = "cboTipPago"
    Private Const mc_strCboDptoSrv As String = "cboDptoSrv"

    Private Const mc_strBtnSolOtEsp As String = "btnSotE"
    Private Const g_strFormSolicitaOTEsp As String = "SCGD_SOTE"

    Private Const g_strAsignacionMultiple As String = "SCGD_ASM"
    Private Const mc_strBtnAsigMult As String = "btnAsM"
    Private Const mc_strFase As String = "U_SCGD_T_Fase"

    Private oFormAsignacionMultiple As AsignacionMultiple
    Private Shared oTimer As System.Timers.Timer
#End Region
#Region "Formularios"
    Private oGestorFormularios As GestorFormularios
    Private oFormSolOTEspecial As SolicitaOTEspecial
#End Region
#End Region

#Region "Propiedades"
    'Cambios OT
    Public Property DocEntryActual() As String
        Get
            Return _strDocEntryActual
        End Get
        Set(ByVal value As String)
            _strDocEntryActual = value
        End Set
    End Property
    Public _strDocEntryActual As String

    Private _dtMecAsignados As DataTable
    Public Property dtMecAsignados() As DataTable
        Get
            Return _dtMecAsignados
        End Get
        Set(ByVal value As DataTable)
            _dtMecAsignados = value
        End Set
    End Property
#End Region


#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, ByVal ocompany As SAPbobsCOM.Company)
        Try
            SBO_Application = p_SBO_Application
            m_oCompany = ocompany
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub
#End Region
#Region "Manejo de Eventos"


    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed_TallerSAP(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        '*****Objetos SAP *****
        Dim oitem As SAPbouiCOM.Item
        Dim sbutton As SAPbouiCOM.Button
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oform As SAPbouiCOM.Form
        '*****Variables *****
        Dim strDocNum As String
        Dim strIDSucursal As String = String.Empty
        Dim strGeneraOT As String = String.Empty
        Dim NumeroOT As String = String.Empty

        Try
            If Not CatchingEvents.m_blnUsaOrdenesDeTrabajo Then
                Exit Sub
            End If
            '*****Valida si Usa OT en SAP *****
            If DMS_Connector.Configuracion.ParamGenAddon.U_OT_SAP = "Y" Then
                m_oFormGenCotizacion = SBO_Application.Forms.Item(pVal.FormUID)

                If pVal.ItemUID = mc_strIDBotonEjecucion AndAlso pVal.BeforeAction Then

                    oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                    DocEntryActual = 0

                    If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        '*****Realiza la carga de la cotizacion inicial *****
                        ValidarCargaCotizacionInicial(oform)
                        strIDSucursal = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd
                        If String.IsNullOrEmpty(strIDSucursal) Then
                            AsignaSucursal(oform)
                        End If
                        NumeroOT = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()
                        oitem = oform.Items.Item("SCGD_cbGOT")
                        oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
                        strGeneraOT = oCombo.Selected.Value


                        If ValidarKilometraje(strIDSucursal, strGeneraOT, NumeroOT, oform.Mode) Then
                            If Not ValidarKM_HorasServico(oform) Then
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If
                        'If Not String.IsNullOrEmpty(strIDSucursal) AndAlso strGeneraOT.Trim() = "1" AndAlso oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        '    If Not ValidarKM_HorasServico(oform) Then
                        '        BubbleEvent = False
                        '        Exit Sub
                        '    End If
                        'End If
                        '*****Valida si usa interfaz Ford *****
                        If Utilitarios.UsaInterfazFord(m_oCompany) Then
                            If Not InterfazFord_Validaciones(oform) Then
                                BubbleEvent = False
                                Exit Sub
                            End If
                        End If
                        blnProcesaCotizacion = True
                    Else
                        blnProcesaCotizacion = False
                    End If
                Else
                    Dim boolExisteForm As Boolean = False
                    If pVal.ItemUID = mc_strIDBotonEjecucion AndAlso pVal.ActionSuccess Then
                        If blnProcesaCotizacion Then
                            If Not String.IsNullOrEmpty(DocEntryActual) Then
                                ProcesaCotizacion(CInt(DocEntryActual), oCotizacionInicial)
                            End If
                        End If
                    Else
                        Select Case pVal.ItemUID
                            Case mc_strBtnSolOtEsp
                                If pVal.Before_Action Then
                                    oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                    If oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrFormQuotationUpdateMode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        BubbleEvent = False
                                    ElseIf oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                                        boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strFormSolicitaOTEsp, True, SBO_Application)
                                        If Not boolExisteForm Then
                                            ValidaSolicitaOTEspecial(pVal, BubbleEvent)
                                        End If
                                    End If
                                Else
                                    Dim numOT As String
                                    Dim DocEntry As String
                                    Dim DocStatus As String
                                    oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                    numOT = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim()
                                    DocEntry = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim()
                                    DocStatus = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim()

                                    If DocStatus = "O" Then

                                        Dim blnUsaTallerOTSAP As Boolean = False
                                        If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                                            blnUsaTallerOTSAP = True
                                        End If
                                        oFormSolOTEspecial.CargaCOT_OT(pVal, numOT, DocEntry)
                                        oFormSolOTEspecial.LoadMatrixLines(blnUsaTallerOTSAP, g_strCreaHjaCanPend)
                                    End If
                                End If

                            Case mc_strBtnAsigMult

                                oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                                Dim DocEntry As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim
                                Dim DocStatus As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim
                                Dim idSuc As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).Trim
                                Dim numOT As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Numero_OT", 0).Trim
                                Dim queryServ As String = String.Empty
                                Dim resultServ As String = String.Empty
                                Dim itemCode As SAPbouiCOM.EditText
                                Dim strCode = String.Empty

                                Dim mtxCot As SAPbouiCOM.Matrix = DirectCast(oform.Items.Item("38").Specific, SAPbouiCOM.Matrix)


                                If pVal.BeforeAction Then
                                    If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, "SCGD_btnAsM") Then
                                        BubbleEvent = False
                                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                    Else
                                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                            boolExisteForm = Utilitarios.ValidarSiFormularioAbierto(g_strAsignacionMultiple, True, SBO_Application)

                                            If Not boolExisteForm Then

                                                'queryServ = "select count(q.DocEntry) from QUT1 q with (nolock) left join OITM i with (nolock) on q.ItemCode = i.ItemCode where q.DocEntry = '{0}' and i.U_SCGD_TipoArticulo =2"
                                                queryServ = DMS_Connector.Queries.GetStrSpecificQuery("strGetContCotLin")
                                                queryServ = String.Format(queryServ, DocEntry)
                                                resultServ = Utilitarios.EjecutarConsulta(queryServ)

                                                If CInt(resultServ) > 0 Then
                                                    CargarFormularioAsignacionMultiple(pVal, BubbleEvent, DocStatus, numOT, idSuc)
                                                Else
                                                    If mtxCot.RowCount - 1 > 0 Then
                                                        queryServ = String.Empty
                                                        queryServ = DMS_Connector.Queries.GetStrSpecificQuery("strGetConItm")
                                                        '"select COUNT(ItemCode) from OITM q with (nolock) where q.U_SCGD_TipoArticulo = 2 and q.itemCode in ({0})"

                                                        For y As Integer = 1 To mtxCot.RowCount - 1
                                                            itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                                                            If String.IsNullOrEmpty(strCode) Then
                                                                strCode = String.Format("'{0}'", itemCode.Value.Trim())
                                                            Else
                                                                strCode = String.Format("{0}, '{1}'", strCode, itemCode.Value.Trim())
                                                            End If
                                                        Next

                                                        queryServ = String.Format(queryServ, strCode)
                                                        resultServ = Utilitarios.EjecutarConsulta(queryServ)

                                                        If CInt(resultServ) > 0 Then
                                                            CargarFormularioAsignacionMultiple(pVal, BubbleEvent, DocStatus, numOT, idSuc)
                                                        Else
                                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                        End If
                                                    Else
                                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                ElseIf (pVal.ActionSuccess) AndAlso BubbleEvent Then

                                    If DocStatus = "O" Then
                                        'queryServ = "select count(q.docentry) from QUT1 q with (nolock) left join OITM i with (nolock) on q.ItemCode = i.ItemCode where q.docentry = '{0}' and i.U_SCGD_TipoArticulo =2"
                                        queryServ = DMS_Connector.Queries.GetStrSpecificQuery("strGetContCotLin")
                                        queryServ = String.Format(queryServ, DocEntry)
                                        resultServ = Utilitarios.EjecutarConsulta(queryServ)

                                        If CInt(resultServ) > 0 Then
                                            oFormAsignacionMultiple.CargaCOT_OT(pVal, numOT, DocEntry, idSuc)
                                            oFormAsignacionMultiple.LoadMatrixLines(pVal.FormTypeEx, idSuc, numOT)
                                        Else
                                            If mtxCot.RowCount - 1 > 0 Then
                                                queryServ = String.Empty
                                                queryServ = DMS_Connector.Queries.GetStrSpecificQuery("strGetConItm")
                                                '"select COUNT(ItemCode) from OITM q with (nolock) where q.U_SCGD_TipoArticulo = 2 and q.itemCode in ({0})"

                                                For y As Integer = 1 To mtxCot.RowCount - 1
                                                    itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                                                    If String.IsNullOrEmpty(strCode) Then
                                                        strCode = String.Format("'{0}'", itemCode.Value.Trim())
                                                    Else
                                                        strCode = String.Format("{0}, '{1}'", strCode, itemCode.Value.Trim())
                                                    End If
                                                Next

                                                queryServ = String.Format(queryServ, strCode)
                                                resultServ = Utilitarios.EjecutarConsulta(queryServ)

                                                If CInt(resultServ) > 0 Then
                                                    oFormAsignacionMultiple.CargaCOT_OT(pVal, "", "", idSuc)
                                                    oFormAsignacionMultiple.LoadMatrixLines(pVal.FormTypeEx, idSuc, numOT)
                                                Else
                                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                                End If
                                            Else
                                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrNoWorksToAssign, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                            End If
                                        End If
                                    End If
                                End If
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Private Function AsignaSucursal(ByVal Form As SAPbouiCOM.Form)
        Dim oForm As SAPbouiCOM.Form
        Dim oUsers As SAPbobsCOM.Users
        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox

        Try
            oForm = Form
            oUsers = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oUsers)
            oItem = oForm.Items.Item("SCGD_cbSuc")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            If oCombo.Selected Is Nothing Then
                For i As Integer = 0 To oCombo.ValidValues.Count
                    If oCombo.ValidValues.Item(i).Value = oUsers.Branch Then
                        oCombo.Select(oCombo.ValidValues.Item(i).Value, BoSearchKey.psk_ByValue)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
        
    End Function

    Private Function ValidarKilometraje(ByVal Sucursal As String, ByVal GenerarOrdenTrabajo As String, ByVal NumeroOT As String, ByVal ModoFormulario As SAPbouiCOM.BoFormMode) As Boolean
        Try
            ValidarKilometraje = False
            GenerarOrdenTrabajo.Trim()
            Select Case ModoFormulario
                Case BoFormMode.fm_ADD_MODE
                    If Not String.IsNullOrEmpty(Sucursal) AndAlso GenerarOrdenTrabajo = "1" Then
                        ValidarKilometraje = True
                    End If
                Case BoFormMode.fm_UPDATE_MODE
                    If Not String.IsNullOrEmpty(Sucursal) AndAlso GenerarOrdenTrabajo = "1" AndAlso String.IsNullOrEmpty(NumeroOT) Then
                        ValidarKilometraje = True
                    End If
                Case Else
                    'No aplica validaciones
                    ValidarKilometraje = False
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent
        Try
            Dim strKey As String = ""
            Dim xmlDocKey As New Xml.XmlDocument

            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD
                    DocEntryActual = String.Empty
                    If BusinessObjectInfo.ActionSuccess Then
                        Select Case BusinessObjectInfo.FormTypeEx
                            'Oferta de ventas
                            Case "149"
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                If Not String.IsNullOrEmpty(strKey) Then
                                    DocEntryActual = strKey
                                End If
                        End Select
                    End If
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                    DocEntryActual = String.Empty
                    Select Case BusinessObjectInfo.FormTypeEx
                        'Oferta de ventas
                        Case "149"
                            xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                            Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                            If Not String.IsNullOrEmpty(strKey) Then
                                DocEntryActual = strKey
                            End If
                    End Select
            End Select
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Manejo de Metodos"
    Public Function CargarCotizacionInicial(ByVal p_intDocEntry As Integer) As oDocumento
        '*****Objetos SAP ******
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            '*****DataContract *****
            Dim oDocumento As oDocumento
            Dim oLineasDocumento As List(Of oLineasDocumento)
            If p_intDocEntry > 0 Then
                oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                If oCotizacion.GetByKey(p_intDocEntry) Then
                    oDocumento = New oDocumento()
                    oLineasDocumento = New List(Of oLineasDocumento)()
                    For rowCotizacion As Integer = 0 To oCotizacion.Lines.Count - 1
                        oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                        With oLineasDocumento
                            .Add(New oLineasDocumento())
                            With .Item(rowCotizacion)
                                .DocEntry = oCotizacion.Lines.DocEntry
                                .LineNum = oCotizacion.Lines.LineNum
                                .ItemCode = oCotizacion.Lines.ItemCode
                                .OriginalQuantity = oCotizacion.Lines.Quantity
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                                    .IdRepxOrd = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    .ID = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                .AprobadoOriginal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                .TrasladadoOriginal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value.ToString()) Then
                                    .OTHija = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                    .TipoArticulo = CInt(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                                End If
                            End With
                        End With
                    Next
                    oDocumento.Lineas = oLineasDocumento
                    Return oDocumento
                End If
                Return Nothing
            End If
            Return Nothing
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
        End Try
    End Function

    Public Sub ActualizaCotizacionPadre(ByRef p_oCotizacionActual As oDocumento, _
                                        ByRef p_oCotizacionPadre As SAPbobsCOM.Documents, _
                                        ByRef p_blnActualizaCotizacionPadre As Boolean, _
                                        ByRef p_blnCotizacionPadreCancelar As Boolean, _
                                        ByRef p_oCotizacionDocumentoMarketingList As DocumentoMarketing_List)
        Try
            '************Data Contract **********
            Dim rowCotizacionDocumentoMarketing As DocumentoMarketing
            '************Variables **************
            Dim strDocEntryPadre As String = String.Empty
            Dim strQuery As String = String.Empty
            Dim intContadorCotizacionActual As Integer = 0
            p_oCotizacionDocumentoMarketingList = New DocumentoMarketing_List()

            If Not String.IsNullOrEmpty(p_oCotizacionActual.NoOTReferencia) Then
                intContadorCotizacionActual = p_oCotizacionActual.Lineas.Count
                strQuery = DMS_Connector.Queries.GetStrSpecificQuery("strDocEntryOfertaPadre")
                strDocEntryPadre = Utilitarios.EjecutarConsulta(String.Format(strQuery, p_oCotizacionActual.NoOTReferencia))
                If Not String.IsNullOrEmpty(strDocEntryPadre) Then
                    p_oCotizacionPadre = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                              SAPbobsCOM.Documents)
                    If p_oCotizacionPadre.GetByKey(strDocEntryPadre) Then
                        For rowCotizacion As Integer = 0 To p_oCotizacionPadre.Lines.Count - 1
                            p_oCotizacionPadre.Lines.SetCurrentLine(rowCotizacion)
                            For Each rowCotizacionActual As oLineasDocumento In p_oCotizacionActual.Lineas
                                With rowCotizacionActual
                                    If p_oCotizacionActual.NoOTReferencia = p_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value Then
                                        If .ID = p_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value Or .IdRepxOrd = p_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value Then
                                            If .ItemCode = p_oCotizacionPadre.Lines.ItemCode Then
                                                p_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                p_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = Trasladado.NoProcesado
                                                p_oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = enumTrasladadoOTHija.scgOTHijaSI
                                                p_blnActualizaCotizacionPadre = True
                                                rowCotizacionDocumentoMarketing = New DocumentoMarketing()
                                                If Not String.IsNullOrEmpty(.ID) Then
                                                    rowCotizacionDocumentoMarketing.ID = .ID
                                                End If
                                                If .IdRepxOrd > 0 Then
                                                    rowCotizacionDocumentoMarketing.IdRepxOrd = .IdRepxOrd
                                                End If
                                                If Not String.IsNullOrEmpty(p_oCotizacionActual.NoOTReferencia) Then
                                                    rowCotizacionDocumentoMarketing.NoOrdenPadre = p_oCotizacionActual.NoOTReferencia
                                                End If
                                                If Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) Then
                                                    rowCotizacionDocumentoMarketing.NoOrdenHija = p_oCotizacionActual.NoOrden
                                                End If
                                                If Not String.IsNullOrEmpty(.ItemCode) Then
                                                    rowCotizacionDocumentoMarketing.ItemCode = .ItemCode
                                                End If
                                                '***********************************
                                                'Agregar linea a documento marketing
                                                '***********************************
                                                p_oCotizacionDocumentoMarketingList.Add(rowCotizacionDocumentoMarketing)
                                                '***********************************
                                                'Elimina linea para recorrer el for
                                                '***********************************
                                                p_oCotizacionActual.Lineas.Remove(rowCotizacionActual)
                                                Exit For
                                            End If
                                        End If
                                    End If
                                End With
                            Next
                        Next
                        If p_oCotizacionPadre.Lines.Count = intContadorCotizacionActual Then
                            p_oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenCancelada
                            p_oCotizacionPadre.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "5"
                            p_blnCotizacionPadreCancelar = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub InstanciaDocumentoMarketing(ByRef p_oDocumentoMarketing As SAPbobsCOM.Documents, _
                                           ByRef p_intTipoDocumentoMarketing As Integer)
        Try
            Select Case p_intTipoDocumentoMarketing
                Case TipoDocumentoMarketing.OfertaCompra
                    p_oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations), SAPbobsCOM.Documents)
                Case TipoDocumentoMarketing.OrdenCompra
                    p_oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders), SAPbobsCOM.Documents)
                Case TipoDocumentoMarketing.EntradaMercancia
                    p_oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes), SAPbobsCOM.Documents)
                Case TipoDocumentoMarketing.FacturaProveedor
                    p_oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
            End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ActualizaObjeto(ByRef p_oDocumentoMarketing As SAPbobsCOM.Documents, _
                                    ByRef p_intDocEntry As Integer, _
                                    ByRef p_odocumentoMarketingList As DocumentoMarketing_List) As Boolean
        Try
            If p_intDocEntry > 0 Then
                If p_oDocumentoMarketing.GetByKey(p_intDocEntry) Then
                    For cont As Integer = 0 To p_oDocumentoMarketing.Lines.Count - 1
                        p_oDocumentoMarketing.Lines.SetCurrentLine(cont)
                        If p_oDocumentoMarketing.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open Then
                            For Each rowMarketing As DocumentoMarketing In p_odocumentoMarketingList
                                If Not String.IsNullOrEmpty(p_oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    If p_oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = rowMarketing.ID Then
                                        p_oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = rowMarketing.NoOrdenHija
                                    End If
                                ElseIf p_oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value > 0 Then
                                    If p_oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = rowMarketing.IdRepxOrd Then
                                        p_oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = rowMarketing.NoOrdenHija
                                    End If
                                End If
                            Next
                        End If
                    Next
                    If p_oDocumentoMarketing.Update() <> 0 Then
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function ActualizaDocumentosMarketing(ByRef p_oDocumentoMarketingList As DocumentoMarketing_List, _
                                                  ByRef p_oDocEntryOfertaCompra As Generic.List(Of Integer), _
                                                  ByRef p_oDocEntryOrdenCompra As Generic.List(Of Integer), _
                                                  ByRef p_oDocEntryEntradaMercancia As Generic.List(Of Integer), _
                                                  ByRef p_oDocEntryFacturaProveedor As Generic.List(Of Integer)) As Boolean
        Dim oDocumentoMarketing As SAPbobsCOM.Documents
        Try
            If p_oDocumentoMarketingList.Count > 0 Then

                '***********************************
                'Recorre oferta de compra
                '***********************************
                If p_oDocEntryOfertaCompra.Count > 0 Then
                    For Each row As Integer In p_oDocEntryOfertaCompra
                        InstanciaDocumentoMarketing(oDocumentoMarketing, TipoDocumentoMarketing.OfertaCompra)
                        If Not ActualizaObjeto(oDocumentoMarketing, row, p_oDocumentoMarketingList) Then
                            Return False
                        End If
                    Next
                End If

                '***********************************
                'Recorre orden compra
                '***********************************
                If p_oDocEntryOrdenCompra.Count > 0 Then
                    For Each row As Integer In p_oDocEntryOrdenCompra
                        InstanciaDocumentoMarketing(oDocumentoMarketing, TipoDocumentoMarketing.OrdenCompra)
                        If Not ActualizaObjeto(oDocumentoMarketing, row, p_oDocumentoMarketingList) Then
                            Return False
                        End If
                    Next
                End If

                '***********************************
                'Recorre entrada mercancia
                '***********************************
                If p_oDocEntryEntradaMercancia.Count > 0 Then
                    For Each row As Integer In p_oDocEntryEntradaMercancia
                        InstanciaDocumentoMarketing(oDocumentoMarketing, TipoDocumentoMarketing.EntradaMercancia)
                        If Not ActualizaObjeto(oDocumentoMarketing, row, p_oDocumentoMarketingList) Then
                            Return False
                        End If
                    Next
                End If

                '***********************************
                'Recorre factura proveedor
                '***********************************
                If p_oDocEntryFacturaProveedor.Count > 0 Then
                    For Each row As Integer In p_oDocEntryFacturaProveedor
                        InstanciaDocumentoMarketing(oDocumentoMarketing, TipoDocumentoMarketing.FacturaProveedor)
                        If Not ActualizaObjeto(oDocumentoMarketing, row, p_oDocumentoMarketingList) Then
                            Return False
                        End If
                    Next
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oDocumentoMarketing)
        End Try
    End Function

    Public Sub ManejaDocumentosMarketing(ByRef p_oDocumentoMarketingList As DocumentoMarketing_List,
                                         ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, _
                                         ByRef p_oDocEntryOfertaCompra As Generic.List(Of Integer), _
                                         ByRef p_oDocEntryOrdenCompra As Generic.List(Of Integer), _
                                         ByRef p_oDocEntryEntradaMercancia As Generic.List(Of Integer), _
                                         ByRef p_oDocEntryFacturaProveedor As Generic.List(Of Integer))
        Try
            If Not CatchingEvents.m_blnUsaOrdenesDeTrabajo Then
                Exit Sub
            End If
            Dim intTipoDocumentoMarketingInicial As Integer = 0
            Dim strTablaDocumentoInicial As String = String.Empty
            Dim intDocEntry As Integer = 0
            Dim dtDocEntriesDocumentoMarketing As System.Data.DataTable
            Dim drwDocEntries As System.Data.DataRow
            Dim oDocEntryOfertaCompraProcesar As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim oDocEntryOrdenCompraProcesar As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim oDocEntryEntradaMercanciaProcesar As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim oDocEntryFacturaProveedorProcesar As Generic.List(Of Integer) = New Generic.List(Of Integer)

            If p_oDocumentoMarketingList.Count > 0 Then
                If p_oConfiguracionSucursalList.Item(0).UsaOfertaCompra Then
                    strTablaDocumentoInicial = "PQT1"
                    intTipoDocumentoMarketingInicial = TipoDocumentoMarketing.OfertaCompra
                Else
                    strTablaDocumentoInicial = "POR1"
                    intTipoDocumentoMarketingInicial = TipoDocumentoMarketing.OrdenCompra
                End If
                dtDocEntriesDocumentoMarketing = Utilitarios.EjecutarConsultaDataTable(String.Format("strDocEntryMarketing", strTablaDocumentoInicial, p_oDocumentoMarketingList.Item(0).NoOrdenPadre))
                If dtDocEntriesDocumentoMarketing.Rows.Count > 0 Then
                    For Each drwDocEntries In dtDocEntriesDocumentoMarketing.Rows
                        If Not String.IsNullOrEmpty(drwDocEntries.Item("DocEntry")) Then
                            intDocEntry = CInt(drwDocEntries.Item("DocEntry"))
                            Select Case intTipoDocumentoMarketingInicial
                                Case TipoDocumentoMarketing.OfertaCompra
                                    If Not oDocEntryOfertaCompraProcesar.Contains(intDocEntry) Then
                                        oDocEntryOfertaCompraProcesar.Add(intDocEntry)
                                    End If
                                Case TipoDocumentoMarketing.OrdenCompra
                                    If Not oDocEntryOrdenCompraProcesar.Contains(intDocEntry) Then
                                        oDocEntryOrdenCompraProcesar.Add(intDocEntry)
                                    End If
                            End Select
                        End If
                    Next
                End If
                '***********************************
                'Recorre oferta de compra
                '***********************************
                For Each row As Integer In oDocEntryOfertaCompraProcesar
                    RecorreDocumentosMarketing(p_oDocumentoMarketingList, row, TipoDocumentoMarketing.OfertaCompra, p_oDocEntryOfertaCompra, p_oDocEntryOrdenCompra, p_oDocEntryEntradaMercancia, p_oDocEntryFacturaProveedor, _
                                               oDocEntryOfertaCompraProcesar, oDocEntryOrdenCompraProcesar, oDocEntryEntradaMercanciaProcesar, oDocEntryFacturaProveedorProcesar)
                Next
                '***********************************
                'Recorre orden compra
                '***********************************
                For Each row As Integer In oDocEntryOrdenCompraProcesar
                    RecorreDocumentosMarketing(p_oDocumentoMarketingList, row, TipoDocumentoMarketing.OrdenCompra, p_oDocEntryOfertaCompra, p_oDocEntryOrdenCompra, p_oDocEntryEntradaMercancia, p_oDocEntryFacturaProveedor, _
                                               oDocEntryOfertaCompraProcesar, oDocEntryOrdenCompraProcesar, oDocEntryEntradaMercanciaProcesar, oDocEntryFacturaProveedorProcesar)
                Next
                '***********************************
                'Recorre entrada mercancia
                '***********************************
                For Each row As Integer In oDocEntryEntradaMercanciaProcesar
                    RecorreDocumentosMarketing(p_oDocumentoMarketingList, row, TipoDocumentoMarketing.EntradaMercancia, p_oDocEntryOfertaCompra, p_oDocEntryOrdenCompra, p_oDocEntryEntradaMercancia, p_oDocEntryFacturaProveedor, _
                                               oDocEntryOfertaCompraProcesar, oDocEntryOrdenCompraProcesar, oDocEntryEntradaMercanciaProcesar, oDocEntryFacturaProveedorProcesar)
                Next
                '***********************************
                'Recorre factura proveedor
                '***********************************
                For Each row As Integer In oDocEntryFacturaProveedorProcesar
                    RecorreDocumentosMarketing(p_oDocumentoMarketingList, row, TipoDocumentoMarketing.FacturaProveedor, p_oDocEntryOfertaCompra, p_oDocEntryOrdenCompra, p_oDocEntryEntradaMercancia, p_oDocEntryFacturaProveedor, _
                                               oDocEntryOfertaCompraProcesar, oDocEntryOrdenCompraProcesar, oDocEntryEntradaMercanciaProcesar, oDocEntryFacturaProveedorProcesar)
                Next
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub RecorreDocumentosMarketing(ByRef p_oDocumentoMarketingList As DocumentoMarketing_List, _
                                          ByRef p_intDocEntry As Integer, _
                                          ByRef p_intTipoDocumentoMarketing As Integer, _
                                          ByRef p_oDocEntryOfertaCompra As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryOrdenCompra As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryEntradaMercancia As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryFacturaProveedor As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryOfertaCompraProcesar As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryOrdenCompraProcesar As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryEntradaMercanciaProcesar As Generic.List(Of Integer), _
                                          ByRef p_oDocEntryFacturaProveedorProcesar As Generic.List(Of Integer))
        Dim oDocumentoMarketing As SAPbobsCOM.Documents
        Try
            Dim strDocumentoTarget As String = String.Empty
            Dim strNombreTabla As String = String.Empty
            Dim intDocTypeTarget As Integer = 0
            Dim intDocEntryTarget As Integer = 0
            Dim strEncargadoAccesorios As String = ""
            Dim arrayDocTarget() As String
            Dim intIndicearreglo As Integer
            Dim strQuery As String = String.Empty
            Dim strIdentidicadorItem As String = String.Empty
            Dim intCampoID As Integer = 0
            Dim blnProcesa As Boolean = False
            Dim intindiceUsuarios As Integer
            Dim dtDocEntriesDocumentoMarketingDestino As System.Data.DataTable
            Dim drwDocEntries As System.Data.DataRow
            Select Case p_intTipoDocumentoMarketing
                Case TipoDocumentoMarketing.OfertaCompra
                    oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations), SAPbobsCOM.Documents)
                    strNombreTabla = "PQT1"
                Case TipoDocumentoMarketing.OrdenCompra
                    oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders), SAPbobsCOM.Documents)
                    strNombreTabla = "POR1"
                Case TipoDocumentoMarketing.EntradaMercancia
                    oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes), SAPbobsCOM.Documents)
                    strNombreTabla = "PDN1"
                Case TipoDocumentoMarketing.FacturaProveedor
                    oDocumentoMarketing = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices), SAPbobsCOM.Documents)
                    strNombreTabla = "PCH1"
            End Select
            If p_intDocEntry > 0 Then
                If oDocumentoMarketing.GetByKey(p_intDocEntry) Then
                    For cont As Integer = 0 To oDocumentoMarketing.Lines.Count - 1
                        oDocumentoMarketing.Lines.SetCurrentLine(cont)
                        intCampoID = 0
                        If Not String.IsNullOrEmpty(oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                            'strQuery = "SELECT Cast(TargetType as varchar) +','+  Cast (TrgetEntry as varchar)as Destino  FROM {0} with (nolock) where DocEntry = {1} and U_SCGD_ID = '{2}'"
                            strQuery = "strDocEntryMarketingDestinoID"
                            intCampoID = CampoID.ID
                        ElseIf oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value > 0 Then
                            'strQuery = "SELECT Cast(TargetType as varchar) +','+  Cast (TrgetEntry as varchar)as Destino  FROM {0} with (nolock) where DocEntry = {1} and U_SCGD_IdRepxOrd = {2}"
                            strQuery = "strDocEntryMarketingDestinoIdRepXOrd"
                            intCampoID = CampoID.IdRepxOrd
                        End If
                        For Each rowDocumentoMarketing As DocumentoMarketing In p_oDocumentoMarketingList
                            blnProcesa = False
                            If intCampoID = CampoID.ID Then
                                If oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = rowDocumentoMarketing.ID Then
                                    blnProcesa = True
                                End If
                            ElseIf intCampoID = CampoID.IdRepxOrd Then
                                If oDocumentoMarketing.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = rowDocumentoMarketing.IdRepxOrd Then
                                    blnProcesa = True
                                End If
                            End If
                            If blnProcesa Then
                                Select Case p_intTipoDocumentoMarketing
                                    '***********************************
                                    'Case Oferta compra
                                    '***********************************
                                    Case TipoDocumentoMarketing.OfertaCompra
                                        intDocTypeTarget = 0
                                        intDocEntryTarget = 0
                                        If intCampoID = CampoID.ID Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.ID))
                                        ElseIf intCampoID = CampoID.IdRepxOrd Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.IdRepxOrd))
                                        End If
                                        For Each drwDocEntries In dtDocEntriesDocumentoMarketingDestino.Rows
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TargetType")) Then
                                                intDocTypeTarget = CInt(drwDocEntries.Item("TargetType"))
                                            End If
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TrgetEntry")) Then
                                                intDocEntryTarget = CInt(drwDocEntries.Item("TrgetEntry"))
                                            End If
                                            Exit For
                                        Next
                                        Select Case intDocTypeTarget
                                            Case TipoDocumentoMarketing.OfertaCompra
                                                rowDocumentoMarketing.DocEntryOfertaCompra = intDocEntryTarget
                                                If Not p_oDocEntryOfertaCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOfertaCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.OrdenCompra
                                                rowDocumentoMarketing.DocEntryOrdenCompra = intDocEntryTarget
                                                If Not p_oDocEntryOrdenCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOrdenCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.EntradaMercancia
                                                rowDocumentoMarketing.DocEntryEntradaMercancia = intDocEntryTarget
                                                If Not p_oDocEntryEntradaMercanciaProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryEntradaMercanciaProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.FacturaProveedor
                                                rowDocumentoMarketing.DocEntryFacturaProveedor = intDocEntryTarget
                                                If Not p_oDocEntryFacturaProveedorProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryFacturaProveedorProcesar.Add(intDocEntryTarget)
                                                End If
                                        End Select
                                        rowDocumentoMarketing.DocEntryOfertaCompra = p_intDocEntry
                                        If oDocumentoMarketing.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open Then
                                            If Not p_oDocEntryOfertaCompra.Contains(p_intDocEntry) Then
                                                p_oDocEntryOfertaCompra.Add(p_intDocEntry)
                                            End If
                                        End If
                                        '***********************************
                                        'Case orden compra
                                        '***********************************
                                    Case TipoDocumentoMarketing.OrdenCompra
                                        intDocTypeTarget = 0
                                        intDocEntryTarget = 0
                                        If intCampoID = CampoID.ID Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.ID))
                                        ElseIf intCampoID = CampoID.IdRepxOrd Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.IdRepxOrd))
                                        End If
                                        For Each drwDocEntries In dtDocEntriesDocumentoMarketingDestino.Rows
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TargetType")) Then
                                                intDocTypeTarget = CInt(drwDocEntries.Item("TargetType"))
                                            End If
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TrgetEntry")) Then
                                                intDocEntryTarget = CInt(drwDocEntries.Item("TrgetEntry"))
                                            End If
                                            Exit For
                                        Next
                                        Select Case intDocTypeTarget
                                            Case TipoDocumentoMarketing.OfertaCompra
                                                rowDocumentoMarketing.DocEntryOfertaCompra = intDocEntryTarget
                                                If Not p_oDocEntryOfertaCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOfertaCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.OrdenCompra
                                                rowDocumentoMarketing.DocEntryOrdenCompra = intDocEntryTarget
                                                If Not p_oDocEntryOrdenCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOrdenCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.EntradaMercancia
                                                rowDocumentoMarketing.DocEntryEntradaMercancia = intDocEntryTarget
                                                If Not p_oDocEntryEntradaMercanciaProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryEntradaMercanciaProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.FacturaProveedor
                                                rowDocumentoMarketing.DocEntryFacturaProveedor = intDocEntryTarget
                                                If Not p_oDocEntryFacturaProveedorProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryFacturaProveedorProcesar.Add(intDocEntryTarget)
                                                End If
                                        End Select
                                        rowDocumentoMarketing.DocEntryOrdenCompra = p_intDocEntry
                                        If oDocumentoMarketing.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open Then
                                            If Not p_oDocEntryOrdenCompra.Contains(p_intDocEntry) Then
                                                p_oDocEntryOrdenCompra.Add(p_intDocEntry)
                                            End If
                                        End If
                                        '***********************************
                                        'Case entrada mercancia
                                        '***********************************
                                    Case TipoDocumentoMarketing.EntradaMercancia
                                        intDocTypeTarget = 0
                                        intDocEntryTarget = 0
                                        If intCampoID = CampoID.ID Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.ID))
                                        ElseIf intCampoID = CampoID.IdRepxOrd Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.IdRepxOrd))
                                        End If
                                        For Each drwDocEntries In dtDocEntriesDocumentoMarketingDestino.Rows
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TargetType")) Then
                                                intDocTypeTarget = CInt(drwDocEntries.Item("TargetType"))
                                            End If
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TrgetEntry")) Then
                                                intDocEntryTarget = CInt(drwDocEntries.Item("TrgetEntry"))
                                            End If
                                            Exit For
                                        Next
                                        Select Case intDocTypeTarget
                                            Case TipoDocumentoMarketing.OfertaCompra
                                                rowDocumentoMarketing.DocEntryOfertaCompra = intDocEntryTarget
                                                If Not p_oDocEntryOfertaCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOfertaCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.OrdenCompra
                                                rowDocumentoMarketing.DocEntryOrdenCompra = intDocEntryTarget
                                                If Not p_oDocEntryOrdenCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOrdenCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.EntradaMercancia
                                                rowDocumentoMarketing.DocEntryEntradaMercancia = intDocEntryTarget
                                                If Not p_oDocEntryEntradaMercanciaProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryEntradaMercanciaProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.FacturaProveedor
                                                rowDocumentoMarketing.DocEntryFacturaProveedor = intDocEntryTarget
                                                If Not p_oDocEntryFacturaProveedorProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryFacturaProveedorProcesar.Add(intDocEntryTarget)
                                                End If
                                        End Select
                                        rowDocumentoMarketing.DocEntryEntradaMercancia = p_intDocEntry
                                        If oDocumentoMarketing.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open Then
                                            If Not p_oDocEntryEntradaMercancia.Contains(p_intDocEntry) Then
                                                p_oDocEntryEntradaMercancia.Add(p_intDocEntry)
                                            End If
                                        End If
                                        '***********************************
                                        'Case factura proveedor
                                        '***********************************
                                    Case TipoDocumentoMarketing.FacturaProveedor
                                        intDocTypeTarget = 0
                                        intDocEntryTarget = 0
                                        If intCampoID = CampoID.ID Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.ID))
                                        ElseIf intCampoID = CampoID.IdRepxOrd Then
                                            dtDocEntriesDocumentoMarketingDestino = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, strNombreTabla, p_intDocEntry, rowDocumentoMarketing.IdRepxOrd))
                                        End If
                                        For Each drwDocEntries In dtDocEntriesDocumentoMarketingDestino.Rows
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TargetType")) Then
                                                intDocTypeTarget = CInt(drwDocEntries.Item("TargetType"))
                                            End If
                                            If Not String.IsNullOrEmpty(drwDocEntries.Item("TrgetEntry")) Then
                                                intDocEntryTarget = CInt(drwDocEntries.Item("TrgetEntry"))
                                            End If
                                            Exit For
                                        Next
                                        Select Case intDocTypeTarget
                                            Case TipoDocumentoMarketing.OfertaCompra
                                                rowDocumentoMarketing.DocEntryOfertaCompra = intDocEntryTarget
                                                If Not p_oDocEntryOfertaCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOfertaCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.OrdenCompra
                                                rowDocumentoMarketing.DocEntryOrdenCompra = intDocEntryTarget
                                                If Not p_oDocEntryOrdenCompraProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryOrdenCompraProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.EntradaMercancia
                                                rowDocumentoMarketing.DocEntryEntradaMercancia = intDocEntryTarget
                                                If Not p_oDocEntryEntradaMercanciaProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryEntradaMercanciaProcesar.Add(intDocEntryTarget)
                                                End If
                                            Case TipoDocumentoMarketing.FacturaProveedor
                                                rowDocumentoMarketing.DocEntryFacturaProveedor = intDocEntryTarget
                                                If Not p_oDocEntryFacturaProveedorProcesar.Contains(intDocEntryTarget) Then
                                                    p_oDocEntryFacturaProveedorProcesar.Add(intDocEntryTarget)
                                                End If
                                        End Select
                                        rowDocumentoMarketing.DocEntryFacturaProveedor = p_intDocEntry
                                        If oDocumentoMarketing.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Open Then
                                            If Not p_oDocEntryFacturaProveedor.Contains(p_intDocEntry) Then
                                                p_oDocEntryFacturaProveedor.Add(p_intDocEntry)
                                            End If
                                        End If
                                End Select
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If oDocumentoMarketing IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoMarketing)
                oDocumentoMarketing = Nothing
            End If
        End Try
    End Sub

    Public Sub CargarCotizacionPadre(ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, _
                                     ByRef p_oCotizacionPadreList As Cotizacion_List)
        Dim oCotizacionPadre As SAPbobsCOM.Documents
        Try
            Dim rowCotizacionPadre As Cotizacion
            Dim strDocEntryPadre As String = String.Empty

            If Not String.IsNullOrEmpty(p_oCotizacionEncabezadoList.Item(0).NoOTReferencia) Then
                strDocEntryPadre = Utilitarios.EjecutarConsulta(String.Format("Select DocEntry from OQUT with(nolock) WHERE U_SCGD_Numero_OT= '{0}'", p_oCotizacionEncabezadoList.Item(0).NoOTReferencia), _
                                                           m_oCompany.CompanyDB, m_oCompany.Server)
                If Not String.IsNullOrEmpty(strDocEntryPadre) Then
                    oCotizacionPadre = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                              SAPbobsCOM.Documents)
                    If oCotizacionPadre.GetByKey(strDocEntryPadre) Then
                        ' Carga lineas de la cotización
                        For rowCotizacion As Integer = 0 To oCotizacionPadre.Lines.Count - 1
                            oCotizacionPadre.Lines.SetCurrentLine(rowCotizacion)
                            rowCotizacionPadre = New Cotizacion()
                            With rowCotizacionPadre
                                If Not String.IsNullOrEmpty(p_oCotizacionEncabezadoList.Item(0).Sucursal) Then
                                    .Sucursal = p_oCotizacionEncabezadoList.Item(0).Sucursal
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacionEncabezadoList.Item(0).NoOrden) Then
                                    .NoOrden = p_oCotizacionEncabezadoList.Item(0).NoOrden
                                End If
                                .DocEntry = oCotizacionPadre.Lines.DocEntry
                                .LineNum = oCotizacionPadre.Lines.LineNum
                                .ItemCode = oCotizacionPadre.Lines.ItemCode
                                .Quantity = oCotizacionPadre.Lines.Quantity
                                .TreeType = oCotizacionPadre.Lines.TreeType
                                .VisOrder = oCotizacionPadre.Lines.VisualOrder
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                                    .IdRepxOrd = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    .ID = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                .Aprobado = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                .Trasladado = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value) Then
                                    .OTHija = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Then
                                    .DuracionEstandar = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                Else
                                    .DuracionEstandar = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()) Then
                                    .NombreEmpleado = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                    .EstadoActividad = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                    .TipoArticulo = CInt(oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                                End If

                                .Costo = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                .CantidadRecibida = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                                .CantidadSolicitada = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                                .CantidadPendiente = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                .CantidadPendienteBodega = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value
                                .CantidadPendienteTraslado = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value
                                .CantidadPendienteDevolucion = oCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value
                            End With
                            p_oCotizacionPadreList.Add(rowCotizacionPadre)
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If Not oCotizacionPadre Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacionPadre)
                oCotizacionPadre = Nothing
            End If
        End Try
    End Sub

    Public Sub CargarCotizacionActual(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                      ByRef p_oCotizacionActual As oDocumento, _
                                      ByRef p_oPaqueteList As Paquete_List, _
                                      ByRef p_blnImprimeReporteRecepcion As Boolean)
        '*****Objetos SAP *****
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
        Dim strNumeroSerieCita As String = String.Empty
        Dim strNumeroCita As String = String.Empty

        Try
            '***********Data Contract ************************
            Dim oPaquete As Paquete
            '**********************************
            'Carga Encabezado de la Cotizacion
            '**********************************
            With p_oCotizacionActual
                .DocEntry = p_oCotizacion.DocEntry
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                    .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .IDSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString()) Then
                    .GeneraOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value) Then
                    .EstadoCotizacionID = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value) Then
                    .GeneraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value) Then
                    .OTPadre = p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value) Then
                    .NoOTReferencia = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value) Then
                    .NumeroVIN = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value) Then
                    .NumeroVehiculo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                    .CodigoUnidad = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.DocumentsOwner.ToString()) Then
                    .CodigoAsesor = p_oCotizacion.DocumentsOwner
                Else
                    .CodigoAsesor = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
                    .TipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                Else
                    .TipoOT = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value) Then
                    .CodigoProyecto = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
                End If
                .CotizacionCancelled = p_oCotizacion.Cancelled
                .CotizacionDocumentStatus = p_oCotizacion.DocumentStatus
                .CardCode = p_oCotizacion.CardCode
                .CardName = p_oCotizacion.CardName
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value) Then
                    .NoVisita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value) Then
                    strNumeroSerieCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString.Trim()
                    strNumeroCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value.ToString.Trim()
                    .NoSerieCita = strNumeroSerieCita
                    .NoCita = strNumeroCita
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value) Then
                    .Cono = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()) Then
                    .Year = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()) Then
                    .DescripcionMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()) Then
                    .DescripcionModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()) Then
                    .DescripcionEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()) Then
                    .CodigoMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()) Then
                    .CodigoEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()) Then
                    .CodigoModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString.Trim()) Then
                    .Kilometraje = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString.Trim()) Then
                    .Placa = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString.Trim()) Then
                    .NombreClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString().Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString.Trim()) Then
                    .CodigoClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString().Trim()
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value Is Nothing Then
                    .FechaRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value Is Nothing Then
                    .HoraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Comp").Value Is Nothing Then
                    .FechaCompromiso = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Comp").Value
                End If
                If Not p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Comp").Value Is Nothing Then
                    .HoraCompromiso = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Comp").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value.ToString.Trim()) Then
                    .NivelGasolina = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Comments) Then
                    .Observaciones = p_oCotizacion.Comments
                End If
                'If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value) Then
                '    .Observaciones = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value.ToString.Trim()
                'End If
                .FechaCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value
                .HoraCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value
                .HorasServicio = Convert.ToDouble(p_oCotizacion.UserFields.Fields.Item("U_SCGD_HoSr").Value)
            End With
            If p_oCotizacionActual.GeneraRecepcion = ImprimeOR.SI AndAlso p_oCotizacionActual.GeneraOT = 1 Then
                p_blnImprimeReporteRecepcion = True
            End If
            For rowCotizacion As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                '********************************
                'Carga Paquete Data Contract
                '********************************
                If p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree _
                    Or p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree _
                    Or p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iProductionTree Then
                    oPaquete = New Paquete()
                    With oPaquete
                        .ItemCodePadre = p_oCotizacion.Lines.ItemCode
                        .TreeTypePadre = p_oCotizacion.Lines.TreeType
                        .AprobadoPadre = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                        .IDItem = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                        .LineNumCotizacionPadre = p_oCotizacion.Lines.LineNum
                    End With
                    p_oPaqueteList.Add(oPaquete)
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
            Utilitarios.DestruirObjeto(oBusinessPartner)
        End Try
    End Sub

    'Public Sub CargarCotizacionActualEncabezado(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
    '                                            ByVal p_intDocEntry As Integer, _
    '                                            ByRef p_oCotizacionActual As Cotizacion, _
    '                                            ByRef p_oPaqueteList As Paquete_List)
    '    Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
    '    Try
    '        Dim oPaquete As Paquete
    '        Dim blnCardType As Boolean = False
    '        Dim dateActual As Date
    '        If p_intDocEntry > 0 Then
    '            oBusinessPartner = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners),  _
    '                                      SAPbobsCOM.BusinessPartners)
    '            If oBusinessPartner.GetByKey(p_oCotizacion.CardCode) Then
    '                blnCardType = True
    '            End If
    '            '**********************************
    '            'Carga Encabezado de la Cotizacion
    '            '**********************************
    '            With p_oCotizacionActual
    '                .DocEntry = p_oCotizacion.DocEntry
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
    '                    .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
    '                    .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString()) Then
    '                    .GeneraOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value) Then
    '                    .EstadoCotizacionID = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value
    '                End If
    '                If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value <> Nothing Then
    '                    .FechaCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value
    '                End If
    '                If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value <> Nothing Then
    '                    .HoraCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value) Then
    '                    .GeneraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value) Then
    '                    .OTPadre = p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value) Then
    '                    .NoOTReferencia = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value) Then
    '                    .NumeroVIN = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
    '                    .CodigoUnidad = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.DocumentsOwner.ToString()) Then
    '                    .CodigoAsesor = p_oCotizacion.DocumentsOwner
    '                Else
    '                    .CodigoAsesor = 0
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
    '                    .TipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
    '                Else
    '                    .TipoOT = 0
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value) Then
    '                    .CodigoProyecto = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
    '                End If
    '                .CotizacionCancelled = p_oCotizacion.Cancelled
    '                .CotizacionDocumentStatus = p_oCotizacion.DocumentStatus
    '                .CardCode = p_oCotizacion.CardCode
    '                .CardName = p_oCotizacion.CardName
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value) Then
    '                    .NoVisita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
    '                End If
    '                If blnCardType Then
    '                    .CardType = oBusinessPartner.CardType
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value) Then
    '                    .NoSerieCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value) Then
    '                    .Cono = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()) Then
    '                    .Year = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()) Then
    '                    .DescripcionMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()) Then
    '                    .DescripcionModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()) Then
    '                    .DescripcionEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()) Then
    '                    .CodigoMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()) Then
    '                    .CodigoEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()) Then
    '                    .CodigoModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString.Trim()) Then
    '                    .Kilometraje = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString.Trim()) Then
    '                    .Placa = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString.Trim()) Then
    '                    .NombreClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString().Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString.Trim()) Then
    '                    .CodigoClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString().Trim()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value.ToString.Trim()) Then
    '                    .FechaRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value.ToString.Trim()) Then
    '                    .HoraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value.ToString()
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value.ToString.Trim()) Then
    '                    .NivelGasolina = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value
    '                End If
    '                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value) Then
    '                    .Observaciones = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value.ToString.Trim()
    '                End If
    '                dateActual = Utilitarios.RetornaFechaActual(m_oCompany.CompanyDB, m_oCompany.Server)
    '                If .FechaCreacionOT <> Nothing AndAlso .HoraCreacionOT <> Nothing Then
    '                    .FechaCreacionOT = Convert.ToDateTime(dateActual).ToShortDateString()
    '                    .HoraCreacionOT = Convert.ToDateTime(dateActual).ToShortTimeString()
    '                End If
    '            End With
    '            For rowCotizacion As Integer = 0 To p_oCotizacion.Lines.Count - 1
    '                p_oCotizacion.Lines.SetCurrentLine(rowCotizacion)
    '                '********************************
    '                'Carga Paquete Data Contract
    '                '********************************
    '                If p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree _
    '                    Or p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree _
    '                    Or p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iProductionTree Then
    '                    oPaquete = New Paquete()
    '                    With oPaquete
    '                        .ItemCodePadre = p_oCotizacion.Lines.ItemCode
    '                        .TreeTypePadre = p_oCotizacion.Lines.TreeType
    '                        .AprobadoPadre = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
    '                        .LineNumCotizacionPadre = p_oCotizacion.Lines.LineNum
    '                    End With
    '                    p_oPaqueteList.Add(oPaquete)
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '    Finally
    '        If Not oBusinessPartner Is Nothing Then
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(oBusinessPartner)
    '            oBusinessPartner = Nothing
    '        End If
    '    End Try
    'End Sub

    Public Sub ProcesaCotizacion(ByRef p_intDocEntry As Integer, Optional ByRef p_oCotizacionInicial As oDocumento = Nothing)
        '*************************Objetos SAP ***********************
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oCotizacionPadre As SAPbobsCOM.Documents
        Dim oOfertaCompra As SAPbobsCOM.Documents
        Dim oOrdenCompra As SAPbobsCOM.Documents
        Dim oEntradaMercancia As SAPbobsCOM.Documents
        Dim oFacturaProveedor As SAPbobsCOM.Documents

        Try
            '*************************Data Contract *********************
            'Dim oCotizacionActual As Cotizacion = New Cotizacion()
            Dim oCotizacionActual As oDocumento = New oDocumento()
            Dim oConfiguracionSucursalList As ConfiguracionSucursal_List = New ConfiguracionSucursal_List()
            Dim oBodegaCentroCostoList As BodegaCentroCosto_List = New BodegaCentroCosto_List()
            Dim oRequisicionDataList As RequisicionData_List = New RequisicionData_List()
            Dim oControlColaboradorList As ControlColaborador_List = New ControlColaborador_List()
            Dim oPaqueteList As Paquete_List = New Paquete_List()
            Dim oRecepcionList As Recepcion_List
            Dim oDocumentoMarketingList As DocumentoMarketing_List
            Dim clsMensajeria As New MensajeriaCls(SBO_Application, m_oCompany)
            '*************************Data Controller *******************
            Dim oControladorRequisicion As ControladorRequisicion = New ControladorRequisicion(m_oCompany, SBO_Application)
            '*************************Listas Genericas ******************
            Dim oDocEntryOfertaCompra As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim oDocEntryOrdenCompra As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim oDocEntryEntradaMercancia As Generic.List(Of Integer) = New Generic.List(Of Integer)
            Dim oDocEntryFacturaProveedor As Generic.List(Of Integer) = New Generic.List(Of Integer)
            '*************************General Services ******************
            Dim oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData)
            '*************Objetos SAP *******************
            Dim oListaCotizacion As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '*************************Variables ************************* 
            Dim strSucursalID As String = String.Empty
            Dim blnResultadoProcesar As Boolean = False
            Dim strCode As String = String.Empty
            Dim blnActualizaCotizacionPadre As Boolean = False
            Dim blnCotizacionPadreCancelar As Boolean = False
            Dim blnResultadoTransaccion As Boolean = False
            Dim intTipoProcesamiento As Integer = 0
            Dim blnImprimeReporteRecepcion As Boolean = False
            Dim blnSolicitudOTEspecial As Boolean = False
            Dim blnDraft As Boolean
            Dim strAsesor As String
            Dim EsReservacion As Boolean = False
            Dim EsTransferenciaAutomatica As Boolean = False

            If p_intDocEntry > 0 Then
                InicializarTimer()
                oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                '************************************
                'Carga Cotizacion
                '************************************
                If oCotizacion.GetByKey(p_intDocEntry) Then
                    '************************************
                    'Actualiza la cita 
                    '************************************
                    ActualizarCita(oCotizacion)
                    '************************************
                    'Valida Informacion Cotizacion
                    '************************************
                    EsReservacion = EsReserva(oCotizacion)
                    If ValidaInformacionCotizacion(oCotizacion) Or EsReservacion Then
                        '************************************
                        'Carga Cotización Actual
                        '************************************
                        CargarCotizacionActual(oCotizacion, oCotizacionActual, oPaqueteList, blnImprimeReporteRecepcion)
                        '************************************
                        'Carga Configuración Sucursal
                        '************************************
                        If CargaConfiguracionSucursal(oCotizacionActual, oConfiguracionSucursalList, oBodegaCentroCostoList) Then
                            '************************************
                            'Se define el tipo de procesamiento
                            '************************************
                            intTipoProcesamiento = TipoProcesamientoCotizacion(oCotizacionActual, EsReservacion)
                            Select Case intTipoProcesamiento
                                Case TipoProcesamiento.Crear
                                    blnResultadoProcesar = ProcesaCotizacionCrear(oCotizacion, oCotizacionActual, oConfiguracionSucursalList, oBodegaCentroCostoList, oRequisicionDataList, oControlColaboradorList, oPaqueteList)
                                Case TipoProcesamiento.Actualizar
                                    blnResultadoProcesar = ProcesaCotizacionActualizar(oCotizacion, oCotizacionActual, oConfiguracionSucursalList, oBodegaCentroCostoList, oRequisicionDataList, oControlColaboradorList, oPaqueteList, EsReservacion)
                                Case TipoProcesamiento.OTEspecial
                                    blnSolicitudOTEspecial = True
                                    'blnResultadoProcesar = ProcesaCotizacionEspecial(oCotizacion, oCotizacionPadre, oCotizacionActual, oConfiguracionSucursalList, oBodegaCentroCostoList, oRequisicionDataList, _
                                    '                                                 oControlColaboradorList, oPaqueteList, blnActualizaCotizacionPadre, blnCotizacionPadreCancelar, oDocEntryOfertaCompra, oDocEntryOrdenCompra, _
                                    '                                                 oDocEntryEntradaMercancia, oDocEntryFacturaProveedor, oDocumentoMarketingList)
                            End Select
                            If blnResultadoProcesar Then
                                '************************************
                                'Maneja requisición
                                '************************************
                                If oRequisicionDataList.Count > 0 Then
                                    ManejaRequisicion(oRequisicionDataList, oListaRequisicionGeneralData)
                                End If
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.GuardandoResultados, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                Utilitarios.ResetTransaction(m_oCompany, SBO_Application)
                                '**********************************
                                'Inicia Transaccion
                                '**********************************
                                Utilitarios.StartTransaction(m_oCompany, SBO_Application)
                                blnResultadoTransaccion = False

                                If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) And Not blnSolicitudOTEspecial Then
                                    BotonAsignacionMultiple(m_oFormGenCotizacion, True, oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString(), oCotizacion)
                                End If

                                If SCG.Requisiciones.TransferenciasDirectas.PermiteTransferenciasDirectas(oCotizacion) Then
                                    EsTransferenciaAutomatica = True
                                End If

                                Select Case intTipoProcesamiento
                                    Case TipoProcesamiento.Crear
                                        '************************************
                                        'Crea Orden de Trabajo
                                        '************************************
                                        If Not CreaOrdenTrabajo(oCotizacionActual, oControlColaboradorList) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Crea transferencias de la bodega reserva hacia la bodega proceso
                                        '************************************
                                        If Not CrearTransferenciasReserva(oCotizacion, oCotizacionActual, oBodegaCentroCostoList, oConfiguracionSucursalList) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Crea requisicion
                                        '************************************
                                        If Not CrearRequisicion(oListaRequisicionGeneralData, EsTransferenciaAutomatica) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Actualiza cotizacion
                                        '************************************
                                        If Not ActualizarCotizacion(oCotizacion, EsTransferenciaAutomatica) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Maneja maestro vehiculo
                                        '************************************
                                        If Not ActualizaMaestroVehiculo(oCotizacionActual) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Bandera resultado transaccion
                                        '************************************
                                        blnResultadoTransaccion = True
                                    Case TipoProcesamiento.Actualizar
                                        '************************************
                                        'Crea control colaborador
                                        '************************************
                                        If Not CrearControlColaborador(oControlColaboradorList, oCotizacionActual) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Crea requisicion
                                        '************************************
                                        If Not CrearRequisicion(oListaRequisicionGeneralData, EsTransferenciaAutomatica) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Actualiza cotizacion
                                        '************************************
                                        If Not ActualizarCotizacion(oCotizacion, EsTransferenciaAutomatica) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Maneja maestro vehiculo
                                        '************************************
                                        If Not ActualizaMaestroVehiculo(oCotizacionActual) Then ManejaTransaccion(False) : Exit Sub
                                        '************************************
                                        'Bandera resultado transaccion
                                        '************************************
                                        blnResultadoTransaccion = True
                                    Case TipoProcesamiento.OTEspecial
                                        ''************************************
                                        ''Crea Orden de Trabajo
                                        ''************************************
                                        'SBO_Application.StatusBar.SetText("Crea Orden", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                        'If Not CreaOrdenTrabajo(oCotizacionActual, oControlColaboradorList) Then ManejaTransaccion(False) : Exit Sub
                                        ''************************************
                                        ''Crea requisicion
                                        ''************************************
                                        'If Not CrearRequisicion(oListaRequisicionGeneralData) Then ManejaTransaccion(False) : Exit Sub
                                        ''************************************
                                        ''Actualiza cotizacion
                                        ''************************************
                                        'SBO_Application.StatusBar.SetText("Actualiza Cotizacion", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                        'If Not ActualizarCotizacion(oCotizacion) Then ManejaTransaccion(False) : Exit Sub
                                        ''************************************
                                        ''Actualiza cotizacion padre
                                        ''************************************
                                        'If blnActualizaCotizacionPadre Then
                                        '    If Not ActualizarCotizacion(oCotizacionPadre) Then ManejaTransaccion(False) : Exit Sub
                                        '    If blnCotizacionPadreCancelar Then
                                        '        If Not CancelarCotizacion(oCotizacionPadre) Then ManejaTransaccion(False) : Exit Sub
                                        '    End If
                                        '    If Not ActualizaDocumentosMarketing(oDocumentoMarketingList, oDocEntryOfertaCompra, oDocEntryOrdenCompra, oDocEntryEntradaMercancia, oDocEntryFacturaProveedor) Then ManejaTransaccion(False) : Exit Sub
                                        'End If
                                        ''************************************
                                        ''Bandera resultado transaccion
                                        ''************************************
                                        'blnResultadoTransaccion = True
                                End Select


                                '************************************
                                'Maneja Transacción
                                '************************************
                                ManejaTransaccion(blnResultadoTransaccion)

                                If blnResultadoTransaccion Then
                                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(x) x.U_Sucurs = oCotizacionActual.IDSucursal) Then
                                        blnDraft = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = oCotizacionActual.IDSucursal).U_Requis = "Y"
                                    End If
                                    strAsesor = Utilitarios.EjecutarConsulta(String.Format(DMS_Connector.Queries.GetStrQueryFormat("strQueryNombreEmpleadoXOT"), oCotizacionActual.NoOrden))
                                    'DMS_Connector.Configuracion.
                                    Select Case intTipoProcesamiento
                                        Case TipoProcesamiento.Crear
                                            clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeCotizacionCreada, oCotizacionActual.DocEntry, oCotizacionActual.NoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, blnDraft, m_oForm, "dtConsulta", oCotizacionActual.IDSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion), True, True)

                                        Case TipoProcesamiento.Actualizar
                                            clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeCotizacionActualizada, oCotizacionActual.DocEntry, oCotizacionActual.NoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, blnDraft, m_oForm, "dtConsulta", oCotizacionActual.IDSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion), True, True)
                                    End Select

                                    If Not IsNothing(oListaRequisicionGeneralData) Then
                                        If oListaRequisicionGeneralData.Count > 0 Then
                                            For Each generalData As GeneralData In oListaRequisicionGeneralData
                                                If generalData.GetProperty("U_SCGD_CodTipoReq").ToString() = "1" Then
                                                    'Transferencia
                                                    If generalData.Child("SCGD_LINEAS_REQ").Item(0).GetProperty("U_SCGD_CodTipoArt").ToString().Trim() = "1" Then
                                                        'Repuestos
                                                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, generalData.GetProperty("DocNum").ToString(), oCotizacionActual.NoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, blnDraft, m_oForm, "dtConsulta", oCotizacionActual.IDSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), False, True, strAsesor)
                                                    Else
                                                        'Suministro
                                                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, generalData.GetProperty("DocNum").ToString(), oCotizacionActual.NoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionSuministros, blnDraft, m_oForm, "dtConsulta", oCotizacionActual.IDSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSuministros), False, True, strAsesor)
                                                    End If
                                                Else
                                                    'Devolución
                                                    If generalData.Child("SCGD_LINEAS_REQ").Item(0).GetProperty("U_SCGD_CodTipoArt").ToString().Trim() = "1" Then
                                                        'Repuestos
                                                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, generalData.GetProperty("DocNum").ToString(), oCotizacionActual.NoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgDevolucionRepuestos, blnDraft, m_oForm, "dtConsulta", oCotizacionActual.IDSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), False, True, strAsesor)
                                                    Else
                                                        'Suministro
                                                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, generalData.GetProperty("DocNum").ToString(), oCotizacionActual.NoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgDevolucionSuministros, blnDraft, m_oForm, "dtConsulta", oCotizacionActual.IDSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSuministros), False, True, strAsesor)
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                End If

                            End If
                        End If
                    End If
                    If blnImprimeReporteRecepcion Then
                        If p_intDocEntry > 0 Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ImprimirOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            ImprimirReporteRecepcion(p_intDocEntry.ToString())
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.RollbackTransaction(m_oCompany, SBO_Application)
            Utilitarios.DestruirObjeto(oCotizacion)
            DMS_Connector.Helpers.ManejoErrores(ex)
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
            Utilitarios.DestruirObjeto(oCotizacionPadre)
            DetenerTimer()
        End Try
    End Sub

    Public Function EsReserva(ByRef Cotizacion As SAPbobsCOM.Documents) As Boolean
        Dim Resultado As Boolean = False
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim UsaRequisicionReserva As String = String.Empty
        Dim EstadoDisparaReserva As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim Query As String = "SELECT TOP 1 T0.""U_Estado"" FROM ""@SCGD_CITA"" T0 WHERE T0.""U_Num_Serie"" = '{0}' AND T0.""U_NumCita"" = '{1}' "
        Dim NumeroOT As String = String.Empty
        Try
            NumeroOT = Cotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value

            If String.IsNullOrEmpty(NumeroOT) Then
                Sucursal = Cotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                SerieCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value
                NumeroCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value
                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                    UsaRequisicionReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_UsePrepicking.Trim
                    EstadoDisparaReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_PrepickingSS.Trim
                End If

                If Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                    Query = String.Format(Query, SerieCita, NumeroCita)
                    EstadoCita = DMS_Connector.Helpers.EjecutarConsulta(Query)
                End If

                If UsaRequisicionReserva = "Y" Then
                    If Not String.IsNullOrEmpty(EstadoCita) AndAlso EstadoCita = EstadoDisparaReserva Then
                        Resultado = True
                    End If
                End If
            End If
            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function


    Public Function CrearTransferenciasReserva(ByRef Cotizacion As SAPbobsCOM.Documents, ByRef CotizacionCache As oDocumento, ByRef p_oBodegaCentroCosto As BodegaCentroCosto_List, ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List) As Boolean
        Dim Resultado As Boolean = True
        Dim Recordset As SAPbobsCOM.Recordset
        Dim Query As String = "SELECT T0.""DocEntry"" AS 'NumeroRequisicion', T1.""U_SCGD_DocEntry"" AS 'NumeroTransferencia' FROM ""@SCGD_REQUISICIONES"" T0 INNER JOIN ""@SCGD_MOVS_REQ"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' "
        Dim ListaRequisiciones As List(Of String)
        Dim DiccTransferencias As Dictionary(Of String, String)
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Try
            SerieCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value
            NumeroCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value
            If Not String.IsNullOrEmpty(CotizacionCache.NoOrden) AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                ListaRequisiciones = New List(Of String)
                DiccTransferencias = New Dictionary(Of String, String)
                Query = String.Format(Query, SerieCita, NumeroCita)
                Recordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset)
                Recordset.DoQuery(Query)

                While Not Recordset.EoF
                    If Not ListaRequisiciones.Contains(Recordset.Fields.Item("NumeroRequisicion").Value.ToString()) Then
                        ListaRequisiciones.Add(Recordset.Fields.Item("NumeroRequisicion").Value.ToString())
                    End If

                    If Not DiccTransferencias.ContainsKey(Recordset.Fields.Item("NumeroTransferencia").Value.ToString()) Then
                        DiccTransferencias.Add(Recordset.Fields.Item("NumeroTransferencia").Value.ToString(), Recordset.Fields.Item("NumeroRequisicion").Value.ToString())
                    End If

                    Recordset.MoveNext()
                End While

                If Not ActualizarReferenciasRequisicion(ListaRequisiciones, CotizacionCache.NoOrden) Then
                    Return False
                End If
                If Not TransferirReservasBodegaProceso(DiccTransferencias, p_oBodegaCentroCosto, p_oConfiguracionSucursal, CotizacionCache.NoOrden) Then
                    Return False
                End If
            End If
            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function ActualizarReferenciasRequisicion(ByVal ListaRequisiciones As List(Of String), ByVal NumeroOT As String) As Boolean
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim sCmp As SAPbobsCOM.CompanyService
        Try
            sCmp = DMS_Connector.Company.CompanySBO.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("SCGD_REQ")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)

            For Each Key As String In ListaRequisiciones
                oGeneralParams.SetProperty("DocEntry", Key)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                oGeneralData.SetProperty("U_SCGD_NoOrden", NumeroOT)
                oGeneralService.Update(oGeneralData)
            Next

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function TransferirReservasBodegaProceso(ByVal DiccTransferencias As Dictionary(Of String, String), ByRef p_oBodegaCentroCosto As BodegaCentroCosto_List, ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, ByVal NumeroOT As String) As Boolean
        Dim TransferenciaReserva As SAPbobsCOM.StockTransfer
        Dim TransferenciaProceso As SAPbobsCOM.StockTransfer
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim sCmp As SAPbobsCOM.CompanyService
        Dim AuditoriaTransferencias As SAPbobsCOM.GeneralDataCollection
        Dim ListID As List(Of String) = New List(Of String)
        Dim LineaAuditoria As SAPbobsCOM.GeneralData
        Dim NewObjectKey As String = String.Empty
        Dim CodigoError As Integer = 0
        Dim MensajeError As String = String.Empty
        Try
            sCmp = DMS_Connector.Company.CompanySBO.GetCompanyService
            oGeneralService = sCmp.GetGeneralService("SCGD_OT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", NumeroOT)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            AuditoriaTransferencias = oGeneralData.Child("SCGD_OTTA")
            TransferenciaReserva = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oStockTransfer)

            For Each Kvp As KeyValuePair(Of String, String) In DiccTransferencias
                If TransferenciaReserva.GetByKey(Kvp.Key) Then
                    TransferenciaProceso = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oStockTransfer)
                    'Completa el número de OT faltante para las transferencias por reserva
                    TransferenciaReserva.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = NumeroOT

                    'Completa los encabezados  
                    TransferenciaProceso.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = NumeroOT
                    TransferenciaProceso.CardCode = TransferenciaReserva.CardCode
                    TransferenciaProceso.Comments = TransferenciaReserva.Comments

                    For i As Integer = 0 To TransferenciaReserva.Lines.Count - 1
                        TransferenciaReserva.Lines.SetCurrentLine(i)
                        If Not (ListID.Contains(TransferenciaReserva.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())) Then
                            TransferenciaProceso.Lines.ItemCode = TransferenciaReserva.Lines.ItemCode
                            TransferenciaProceso.Lines.ItemDescription = TransferenciaReserva.Lines.ItemDescription
                            TransferenciaProceso.Lines.Quantity = TransferenciaReserva.Lines.Quantity
                            TransferenciaProceso.Lines.FromWarehouseCode = TransferenciaReserva.Lines.WarehouseCode
                            TransferenciaProceso.Lines.WarehouseCode = ObtenerBodegaProceso(TransferenciaProceso.Lines.ItemCode, p_oBodegaCentroCosto, p_oConfiguracionSucursal)
                            TransferenciaProceso.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = TransferenciaReserva.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                            TransferenciaProceso.Lines.Add()
                            ListID.Add(TransferenciaReserva.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString())
                        End If
                    Next
                    CodigoError = TransferenciaProceso.Add()
                    If CodigoError <> 0 Then
                        MensajeError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                        DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(MensajeError, BoMessageTime.bmt_Short, True)
                        Return False
                    End If

                    NewObjectKey = DMS_Connector.Company.CompanySBO.GetNewObjectKey()

                    For i As Integer = 0 To TransferenciaProceso.Lines.Count - 1
                        TransferenciaProceso.Lines.SetCurrentLine(i)
                        LineaAuditoria = AuditoriaTransferencias.Add()
                        LineaAuditoria.SetProperty("U_SCGD_ID", TransferenciaProceso.Lines.UserFields.Fields.Item("U_SCGD_ID").Value)
                        LineaAuditoria.SetProperty("U_BaseEntry", Kvp.Key)
                        LineaAuditoria.SetProperty("U_ReqEntry", Kvp.Value)
                        LineaAuditoria.SetProperty("U_ItemCode", TransferenciaProceso.Lines.ItemCode)
                        LineaAuditoria.SetProperty("U_Description", TransferenciaProceso.Lines.ItemDescription)
                        LineaAuditoria.SetProperty("U_Quantity", TransferenciaProceso.Lines.Quantity)
                        LineaAuditoria.SetProperty("U_TransEntry", NewObjectKey)
                        LineaAuditoria.SetProperty("U_FromWarehouse", TransferenciaProceso.Lines.FromWarehouseCode)
                        LineaAuditoria.SetProperty("U_Warehouse", TransferenciaProceso.Lines.WarehouseCode)
                        LineaAuditoria.SetProperty("U_Date", DateTime.Now)
                        LineaAuditoria.SetProperty("U_Hour", DateTime.Now)
                        LineaAuditoria.SetProperty("U_User", DMS_Connector.Company.CompanySBO.UserName)
                    Next
                    TransferenciaReserva.Update()
                End If
            Next

            'Actualiza la tabla con la información de las transferencias en la orden de trabajo
            'con fines de auditoría y reportes
            oGeneralService.Update(oGeneralData)

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Private Function ObtenerBodegaProceso(ByVal ItemCode As String, ByRef p_oBodegaCentroCosto As BodegaCentroCosto_List, ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List) As String
        Dim Articulo As SAPbobsCOM.Items
        Dim CentroCosto As String = String.Empty
        Dim BodegaProceso As String = String.Empty
        Try
            Articulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            Articulo.GetByKey(ItemCode)
            If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                CentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
            Else
                CentroCosto = Articulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
            End If
            For Each row As BodegaCentroCosto In p_oBodegaCentroCosto
                If row.CentroCosto = CentroCosto Then
                    BodegaProceso = row.BodegaProceso
                    'Si se obtiene la bodega proceso se termina el ciclo for
                    Exit For
                End If
            Next
            Return BodegaProceso
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return BodegaProceso
        End Try
    End Function

    Public Sub ManejaTransaccion(ByRef p_blnResultadoTransaccion As Boolean)
        Try
            If p_blnResultadoTransaccion Then
                Utilitarios.CommitTransaction(m_oCompany, SBO_Application)
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
            Else
                Utilitarios.RollbackTransaction(m_oCompany, SBO_Application)
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorProcesandoOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If
        Catch ex As Exception
            Utilitarios.RollbackTransaction(m_oCompany, SBO_Application)
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub ImprimirReporteRecepcion(ByRef p_strDocEntry As String)
        Dim strDireccionReporte As String
        Dim strParametros As String
        Try
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                strDireccionReporte = DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim()
                strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptOrdenRecepcionInterna & ".rpt"
                strParametros = p_strDocEntry
                Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.rptOrdenRecepcionInterna, strParametros, CatchingEvents.DBUser, CatchingEvents.DBPassword, m_oCompany.CompanyDB, m_oCompany.Server)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Public Function ActualizaMaestroVehiculo(ByRef p_oCotizacionActual As oDocumento) As Boolean
        Try
            '************Objetos SAP **********
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim strKilometraje As String = String.Empty
            Dim intKilometraje As Integer
            If p_oCotizacionActual.NumeroVehiculo > 0 Then
                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", Convert.ToString(p_oCotizacionActual.NumeroVehiculo))
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                oGeneralData.SetProperty("U_HorSer", Convert.ToInt32(p_oCotizacionActual.HorasServicio))
                intKilometraje = Integer.Parse(oGeneralData.GetProperty("U_Km_Unid"))
                If intKilometraje < p_oCotizacionActual.Kilometraje Then
                    oGeneralData.SetProperty("U_Km_Unid", p_oCotizacionActual.Kilometraje)
                End If
                oGeneralService.Update(oGeneralData)
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function ProcesaCotizacionEspecial(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                              ByRef p_oCotizacionPadre As SAPbobsCOM.Documents, _
                                              ByRef p_oCotizacionActual As oDocumento, _
                                              ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, _
                                              ByRef p_oBodegaCentroCosto As BodegaCentroCosto_List, _
                                              ByRef p_oRequisicionDataLineas As RequisicionData_List, _
                                              ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                              ByRef p_oPaqueteList As Paquete_List, _
                                              ByRef p_blnActualizaCotizacionPadre As Boolean, _
                                              ByRef p_blnCotizacionPadreCancelar As Boolean, _
                                              ByRef p_oDocEntryOfertaCompra As Generic.List(Of Integer), _
                                              ByRef p_oDocEntryOrdenCompra As Generic.List(Of Integer), _
                                              ByRef p_oDocEntryEntradaMercancia As Generic.List(Of Integer), _
                                              ByRef p_oDocEntryFacturaProveedor As Generic.List(Of Integer), _
                                              ByRef p_oDocumentoMarketingList As DocumentoMarketing_List) As Boolean
        '**************Objetos SAP ****************
        Dim oArticulo As SAPbobsCOM.IItems
        Try
            '****************Data Contract ***********
            Dim rowCotizacion As oLineasDocumento
            Dim oCotizacionActualList As Cotizacion_List = New Cotizacion_List()
            Dim oPaqueteListResultado As Paquete_List
            Dim rowCotizacionPadreList As Cotizacion_List = New Cotizacion_List()
            '****************Variables ***************
            Dim strNumeroVisita As String = String.Empty
            Dim strNoOrdenSiguiente As String = String.Empty
            Dim blnMensajeCCOT As Boolean = False
            oArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            '********************************
            'Se asigna el numero de Orden
            '*******************************
            AsignaNumeroOTSiguiente(p_oCotizacionActual)
            If Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) And Not String.IsNullOrEmpty(p_oCotizacionActual.NoOTReferencia) Then
                '********************************
                'Carga cotizacion padre
                '*******************************
                'CargarCotizacionPadre(oCotizacionEncabezadoList, rowCotizacionPadreList)
                '********************************
                'Se asignan valores manuales
                '*******************************
                AsignaValoresManualesActualizar(p_oCotizacionActual)
                '********************************
                'Valida Paquetes
                '*******************************
                ManejarPaquete(p_oPaqueteList, oPaqueteListResultado)
                '********************************
                'Mensaje de Procesando Lineas
                '*******************************
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoLineas, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                For rowContador As Integer = 0 To p_oCotizacion.Lines.Count - 1
                    p_oCotizacion.Lines.SetCurrentLine(rowContador)
                    rowCotizacion = New oLineasDocumento()
                    '*******************************************
                    'Asigna Lineas a Cotizacion Data Contract
                    '*******************************************
                    If AsignaValorACotizacionDataContract(p_oCotizacion, rowCotizacion) Then
                        '*****************************************
                        'Valida si la linea pertenece a un paquete
                        '*****************************************
                        ValidaPaquete(rowCotizacion, oPaqueteListResultado, p_oCotizacion, False, String.Empty, String.Empty)
                        '***********************************
                        'Valida si la linea se debe procesar
                        '***********************************
                        If ValidaProcesoActualizar(p_oCotizacion, rowCotizacion, p_oConfiguracionSucursal) Then
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & ":  " & rowCotizacion.VisOrder + 1 & My.Resources.Resource.Separador & rowCotizacion.ItemCode & "     " & rowCotizacion.Description, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            '********************************
                            'Carga Articulo
                            '*******************************
                            If oArticulo.GetByKey(rowCotizacion.ItemCode) Then
                                '********************************
                                'Valida Informacion Articulo
                                '*******************************
                                If ValidaArticulo(rowCotizacion, oArticulo, p_oBodegaCentroCosto, p_oConfiguracionSucursal, blnMensajeCCOT, False, String.Empty, String.Empty) Then
                                    If rowCotizacion.Procesar Then
                                        '*************************************************
                                        'Valida que accion tomar para el procesamiento
                                        '*************************************************
                                        If TipoProcesamientoActualizar(rowCotizacion) <> ProcesamientoLinea.NingunaAccion Then
                                            If rowCotizacion.EsAdicional Then
                                                If rowCotizacion.Aprobado = ArticuloAprobado.scgSi Then
                                                    '********************************
                                                    'Asigna ID
                                                    'Carga datos lineas cotizacion
                                                    '*******************************
                                                    DatosLineasCotizacion(rowCotizacion, p_oCotizacionActual, False)
                                                    '********************************
                                                    'Valida disponibilidad articulo
                                                    '********************************
                                                    ValidaDisponibilidadArticulo(rowCotizacion, oArticulo, p_oConfiguracionSucursal)
                                                    '********************************
                                                    'Maneja Cantidades,Estados y Documentos
                                                    '********************************
                                                    ManejaLineasCrear(rowCotizacion, p_oCotizacionActual, p_oRequisicionDataLineas, p_oControlColaboradorList, False)
                                                End If
                                            Else
                                                '********************************
                                                'Asigna ID
                                                'Carga datos lineas cotizacion
                                                '*******************************
                                                DatosLineasCotizacion(rowCotizacion, p_oCotizacionActual, False)
                                                '********************************
                                                'Maneja Cantidades,Estados y Documentos
                                                '********************************
                                                ManejaLineasActualizar(rowCotizacion, oArticulo, p_oCotizacionActual, p_oRequisicionDataLineas, p_oControlColaboradorList, p_oConfiguracionSucursal, False)
                                            End If
                                        Else
                                            AsignarCantidadesNegativas(rowCotizacion)
                                        End If
                                    Else
                                        AsignarCantidadesNegativas(rowCotizacion)
                                    End If
                                Else
                                    AsignarCantidadesNegativas(rowCotizacion)
                                    If blnMensajeCCOT Then
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.RevisarConfCentroCostoXOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Else
                                        SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & rowCotizacion.ItemCode & " " & rowCotizacion.Description & My.Resources.Resource.MalConfigurado)
                                    End If
                                End If
                            End If
                            '********************************
                            'Asigna Lineas a Cotizacion data Contract
                            '*******************************
                            ReplicaValorACotizacion(p_oCotizacion, rowCotizacion)
                        End If
                        p_oCotizacionActual.Lineas.Add(rowCotizacion)
                    End If
                Next
                '*********************************
                'Carga Ubicaciones
                '*********************************
                If p_oConfiguracionSucursal.Item(0).UsaUbicaciones = True Then
                    CargaUbicaciones(p_oRequisicionDataLineas)
                End If
                '*********************************
                'Actualiza Encabezado Cotizacion
                '*********************************
                AsignaValorEncabezadoCotizacion(p_oCotizacion, p_oCotizacionActual)
                '*************************************
                'Actualiza valores en cotizacion padre
                '*************************************
                ActualizaCotizacionPadre(p_oCotizacionActual, p_oCotizacionPadre, p_blnActualizaCotizacionPadre, p_blnCotizacionPadreCancelar, p_oDocumentoMarketingList)
                '********************************************
                'Actualiza documentos marketing relacionados
                '********************************************
                ManejaDocumentosMarketing(p_oDocumentoMarketingList, p_oConfiguracionSucursal, p_oDocEntryOfertaCompra, p_oDocEntryOrdenCompra, p_oDocEntryEntradaMercancia, p_oDocEntryFacturaProveedor)
                Return True
            End If
            Return False
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If oArticulo IsNot Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oArticulo)
                oArticulo = Nothing
            End If
        End Try
    End Function

    Public Sub AsignarCantidadesNegativas(ByRef p_rowCotizacion As oLineasDocumento)
        Try
            '**********************************************************************************************************
            'Esto se hace para validar si la linea fue modificada al momento de actualizar los valores en la cotizacion
            ' El valor -1 representa que los valores no fueron actualizados
            '**********************************************************************************************************
            With p_rowCotizacion
                .CantidadRecibida = -1
                .CantidadPendiente = -1
                .CantidadSolicitada = -1
                .CantidadPendienteBodega = -1
                .CantidadPendienteTraslado = -1
                .CantidadPendienteDevolucion = -1
            End With
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ProcesaCotizacionActualizar(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                                ByRef p_oCotizacionActual As oDocumento, _
                                                ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, _
                                                ByRef p_oBodegaCentroCosto As BodegaCentroCosto_List, _
                                                ByRef p_oRequisicionDataLineas As RequisicionData_List, _
                                                ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                                ByRef p_oPaqueteList As Paquete_List, ByVal EsReservacion As Boolean) As Boolean
        '***************Objetos SAP *************
        Dim oArticulo As SAPbobsCOM.IItems = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        Try
            '**************Data Contract ******************
            Dim rowCotizacion As oLineasDocumento
            Dim oPaqueteListResultado As Paquete_List = New Paquete_List()
            '**************Variables **********************
            Dim strNumeroVisita As String = String.Empty
            Dim strNoOrdenSiguiente As String = String.Empty
            Dim blnMensajeCCOT As Boolean = False
            Dim NumeroSerieCita As String = String.Empty
            Dim ConsecutivoCita As String = String.Empty
            '********************************
            'Se asignan valores manuales
            '*******************************
            AsignaValoresManualesActualizar(p_oCotizacionActual)
            '********************************
            'Valida Paquetes
            '*******************************
            ManejarPaquete(p_oPaqueteList, oPaqueteListResultado)
            '********************************
            'Mensaje de Procesando Lineas
            '*******************************
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoLineas, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            NumeroSerieCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value
            ConsecutivoCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value
            For rowContador As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(rowContador)
                rowCotizacion = New oLineasDocumento()
                '*******************************************
                'Asigna Lineas a Cotizacion Data Contract
                '*******************************************
                AsignaValorACotizacionDataContract(p_oCotizacion, rowCotizacion, True)
                '*****************************************
                'Valida si la linea pertenece a un paquete
                '*****************************************
                If (oPaqueteListResultado.Count > 0) Then
                    ValidaPaquete(rowCotizacion, oPaqueteListResultado, p_oCotizacion, EsReservacion, NumeroSerieCita, ConsecutivoCita)
                End If
                '***********************************
                'Valida si la linea se debe procesar
                '***********************************
                If ValidaProcesoActualizar(p_oCotizacion, rowCotizacion, p_oConfiguracionSucursal) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & ":  " & rowCotizacion.VisOrder + 1 & My.Resources.Resource.Separador & rowCotizacion.ItemCode & "     " & rowCotizacion.Description, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    '********************************
                    'Carga Articulo
                    '*******************************
                    If oArticulo.GetByKey(rowCotizacion.ItemCode) Then
                        '********************************
                        'Valida Informacion Articulo
                        '*******************************
                        If ValidaArticulo(rowCotizacion, oArticulo, p_oBodegaCentroCosto, p_oConfiguracionSucursal, blnMensajeCCOT, EsReservacion, NumeroSerieCita, ConsecutivoCita) Then
                            If rowCotizacion.Procesar Then
                                '*************************************************
                                'Valida que accion tomar para el procesamiento
                                '*************************************************
                                If TipoProcesamientoActualizar(rowCotizacion) <> ProcesamientoLinea.NingunaAccion Then
                                    If rowCotizacion.EsAdicional Then
                                        If rowCotizacion.Aprobado = ArticuloAprobado.scgSi Then
                                            '********************************
                                            'Asigna ID
                                            'Carga datos lineas cotizacion
                                            '*******************************
                                            DatosLineasCotizacion(rowCotizacion, p_oCotizacionActual, EsReservacion)
                                            '********************************
                                            'Valida disponibilidad articulo
                                            '********************************
                                            ValidaDisponibilidadArticulo(rowCotizacion, oArticulo, p_oConfiguracionSucursal)
                                            '********************************
                                            'Maneja Cantidades,Estados y Documentos
                                            '********************************
                                            ManejaLineasCrear(rowCotizacion, p_oCotizacionActual, p_oRequisicionDataLineas, p_oControlColaboradorList, EsReservacion)
                                        End If
                                    Else
                                        '********************************
                                        'Asigna ID
                                        'Carga datos lineas cotizacion
                                        '*******************************
                                        DatosLineasCotizacion(rowCotizacion, p_oCotizacionActual, EsReservacion)
                                        '********************************
                                        'Maneja Cantidades,Estados y Documentos
                                        '********************************
                                        ManejaLineasActualizar(rowCotizacion, oArticulo, p_oCotizacionActual, p_oRequisicionDataLineas, p_oControlColaboradorList, p_oConfiguracionSucursal, EsReservacion)
                                    End If
                                Else
                                    AsignarCantidadesNegativas(rowCotizacion)
                                End If
                            Else
                                AsignarCantidadesNegativas(rowCotizacion)
                            End If
                        Else
                            AsignarCantidadesNegativas(rowCotizacion)
                            If blnMensajeCCOT Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.RevisarConfCentroCostoXOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            Else
                                SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & rowCotizacion.ItemCode & " " & rowCotizacion.Description & My.Resources.Resource.MalConfigurado)
                            End If
                        End If
                        '********************************
                        'Asigna Lineas a Cotizacion data Contract
                        '*******************************
                        ReplicaValorACotizacion(p_oCotizacion, rowCotizacion)
                    End If
                Else
                    '***** Asigna ID en Aprobado= Falta de Aprobación *****
                    AsignaIDFaltaAprobacion(p_oCotizacion, rowCotizacion, p_oCotizacionActual)
                End If
            Next
            '*********************************
            'Carga Ubicaciones
            '*********************************
            If p_oConfiguracionSucursal.Item(0).UsaUbicaciones = True Then
                If p_oRequisicionDataLineas.Count > 0 Then
                    CargaUbicaciones(p_oRequisicionDataLineas)
                End If
            End If
            '*********************************
            'Actualiza Encabezado Cotizacion
            '*********************************
            AsignaValorEncabezadoCotizacion(p_oCotizacion, p_oCotizacionActual)
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If oArticulo IsNot Nothing Then
                Utilitarios.DestruirObjeto(oArticulo)
            End If
        End Try
    End Function

    Public Sub ActualizarCita(ByRef p_oCotizacion As SAPbobsCOM.Documents)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim strPoseeCampana As String = String.Empty
        Dim strGarantiaVigente As String = String.Empty
        Dim strIngresoPorGrua As String = String.Empty
        Dim strDocEntryCotizacion As String = String.Empty
        Dim strDocEntryCita As String = String.Empty
        Dim strQuery As String = "SELECT TOP 1 T0.""DocEntry"" FROM ""@SCGD_CITA"" T0 WITH(nolock)  WHERE T0.""U_Num_Cot"" = '{0}' ORDER BY T0.""DocEntry"" DESC"

        Try
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CIT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)

            strDocEntryCotizacion = p_oCotizacion.DocEntry
            strQuery = String.Format(strQuery, strDocEntryCotizacion)
            strDocEntryCita = DMS_Connector.Helpers.EjecutarConsulta(strQuery)

            If Not String.IsNullOrEmpty(strDocEntryCita) AndAlso Not String.IsNullOrEmpty(strDocEntryCotizacion) Then
                oGeneralParams.SetProperty("DocEntry", strDocEntryCita)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                strPoseeCampana = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Campana").Value
                If Not String.IsNullOrEmpty(strPoseeCampana) Then
                    oGeneralData.SetProperty("U_Campana", strPoseeCampana)
                End If

                strGarantiaVigente = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Garantia").Value
                If Not String.IsNullOrEmpty(strGarantiaVigente) Then
                    oGeneralData.SetProperty("U_Garantia", strGarantiaVigente)
                End If

                strIngresoPorGrua = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Towing").Value
                If Not String.IsNullOrEmpty(strIngresoPorGrua) Then
                    oGeneralData.SetProperty("U_Towing", strIngresoPorGrua)
                End If

                oGeneralService.Update(oGeneralData)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaIDFaltaAprobacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                       ByRef p_rowCotizacion As oLineasDocumento, _
                                       ByRef p_oCotizacionActual As oDocumento)
        Try
            Dim oArticulo As SAPbobsCOM.IItems
            oArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            oArticulo.GetByKey(p_rowCotizacion.ItemCode)
            If p_rowCotizacion.Aprobado = ArticuloAprobado.scgFalta Then
                If String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                    p_rowCotizacion.NoOrden = p_oCotizacionActual.NoOrden
                End If
                If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                    p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_rowCotizacion.NoOrden)
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = p_rowCotizacion.ID
                    If (Not String.IsNullOrEmpty(p_rowCotizacion.Sucursal)) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = p_rowCotizacion.Sucursal
                    End If
                    If (Not String.IsNullOrEmpty(p_rowCotizacion.CentroCosto)) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = p_rowCotizacion.CentroCosto
                    End If
                    If Not String.IsNullOrEmpty(oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = Conversion.Str(CInt(oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value))
                    End If
                    If p_rowCotizacion.CantidadRecibida = 0 AndAlso p_rowCotizacion.CantidadPendiente = 0 Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = p_rowCotizacion.Quantity
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function TipoProcesamientoActualizar(ByRef p_rowCotizacion As oLineasDocumento) As Integer
        Try
            With p_rowCotizacion
                '********************************
                'Se valida según tipo de articulo
                '*******************************
                Select Case CInt(p_rowCotizacion.TipoArticulo)
                    Case TipoArticulo.Repuesto
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                                Select Case .Trasladado
                                    Case Trasladado.SI
                                        If p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        Else
                                            .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                            Return ProcesamientoLinea.NingunaAccion
                                        End If

                                    Case Trasladado.NO
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.TrasladoBodega
                                        Return ProcesamientoLinea.TrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.Requisicion
                                        Return ProcesamientoLinea.Requisicion
                                End Select
                            Case ArticuloAprobado.scgNo
                                Select Case .Trasladado
                                    Case Trasladado.SI
                                        .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                        Return ProcesamientoLinea.RequisicionDevolucion
                                    Case Trasladado.NO
                                        If .Comprar = "Y" And .CantidadRecibida = .Quantity Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        End If
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularRequisicion
                                        Return ProcesamientoLinea.AnularRequisicion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularTrasladoBodega
                                        Return ProcesamientoLinea.AnularTrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                End Select
                            Case ArticuloAprobado.scgFalta
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                            Case ArticuloAprobado.scgCambioOT
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                        End Select
                    Case TipoArticulo.Servicio
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                            Case ArticuloAprobado.scgNo
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                        End Select
                    Case TipoArticulo.ServicioExterno
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                                If String.IsNullOrEmpty(.ID) Or .Comprar = "N" Then
                                    .ProcesamientoLinea = ProcesamientoLinea.ProcesaServicioExterno
                                    Return ProcesamientoLinea.ProcesaServicioExterno
                                End If
                            Case ArticuloAprobado.scgNo
                                .ProcesamientoLinea = ProcesamientoLinea.AnulaServicioExterno
                                Return ProcesamientoLinea.AnulaServicioExterno
                        End Select
                    Case TipoArticulo.Suministro
                        Select Case .Aprobado
                            Case ArticuloAprobado.scgSi
                                Select Case .Trasladado
                                    Case Trasladado.SI

                                        If p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        Else
                                            .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                            Return ProcesamientoLinea.NingunaAccion
                                        End If
                                    Case Trasladado.NO
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.TrasladoBodega
                                        Return ProcesamientoLinea.TrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.Requisicion
                                        Return ProcesamientoLinea.Requisicion
                                End Select
                            Case ArticuloAprobado.scgNo
                                Select Case .Trasladado
                                    Case Trasladado.SI
                                        .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                        Return ProcesamientoLinea.RequisicionDevolucion
                                    Case Trasladado.NO
                                        If .Comprar = "Y" And .CantidadRecibida = .Quantity Then
                                            .ProcesamientoLinea = ProcesamientoLinea.RequisicionDevolucion
                                            Return ProcesamientoLinea.RequisicionDevolucion
                                        End If
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                    Case Trasladado.PendienteBodega
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularRequisicion
                                        Return ProcesamientoLinea.AnularRequisicion
                                    Case Trasladado.PendienteTraslado
                                        .ProcesamientoLinea = ProcesamientoLinea.AnularTrasladoBodega
                                        Return ProcesamientoLinea.AnularTrasladoBodega
                                    Case Trasladado.NoProcesado
                                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                        Return ProcesamientoLinea.NingunaAccion
                                End Select
                            Case ArticuloAprobado.scgFalta
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                            Case ArticuloAprobado.scgCambioOT
                                .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                                Return ProcesamientoLinea.NingunaAccion
                        End Select
                    Case Else
                        .ProcesamientoLinea = ProcesamientoLinea.NingunaAccion
                        Return ProcesamientoLinea.NingunaAccion
                End Select
            End With
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Function ValidaSiExisteOT(ByRef p_strNoOT As String) As Boolean
        Try
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", p_strNoOT)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ExisteOT & " " & p_strNoOT, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function ValidaVisitaAsociada(ByRef p_strNoVisita As String, ByRef p_strNoUnidad As String) As Boolean
        Dim oForm As SAPbouiCOM.Form
        Dim creationPackage As SAPbouiCOM.FormCreationParams
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsOfertaVisita As DBDataSource
        Try
            If Not String.IsNullOrEmpty(p_strNoVisita) And Not String.IsNullOrEmpty(p_strNoUnidad) Then
                creationPackage = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
                'creationPackage.UniqueID = ""
                creationPackage.FormType = "Visita"
                creationPackage.ObjectType = ""

                oForm = SBO_Application.Forms.AddEx(creationPackage)
                oForm.DataSources.DBDataSources.Add("OQUT")
                dsOfertaVisita = oForm.DataSources.DBDataSources.Item("OQUT")

                oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 2
                oCondition.Alias = "U_SCGD_No_Visita"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = p_strNoVisita
                oCondition.BracketCloseNum = 1

                oCondition.Relationship = BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_Cod_Unidad"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCondition.CondVal = p_strNoUnidad
                oCondition.BracketCloseNum = 2

                dsOfertaVisita.Query(oConditions)

                If dsOfertaVisita.Size > 0 Then Return False
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        Finally
            oForm.Close()
        End Try
    End Function

    Public Function ProcesaCotizacionCrear(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                           ByRef p_oCotizacionActual As oDocumento, _
                                           ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, _
                                           ByRef p_oBodegaCentroCosto As BodegaCentroCosto_List, _
                                           ByRef p_oRequisicionDataLineas As RequisicionData_List, _
                                           ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                           ByRef p_oPaqueteList As Paquete_List) As Boolean
        '********************Objetos SAP **********************************
        Dim oArticulo As SAPbobsCOM.IItems
        Try
            '******************Data Contract *********************
            Dim oRequisicionData As RequisicionData
            Dim oPaqueteListResultado As Paquete_List = New Paquete_List()
            'Dim rowCotizacion As LineasCotizacion
            Dim rowCotizacion As oLineasDocumento
            '******************Variables *************************
            Dim strNumeroVisita As String = String.Empty
            Dim strNoOrdenSiguiente As String = String.Empty
            Dim blnMensajeCCOT As Boolean = False

            oArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            '********************************
            'Se asigna el numero de visita
            '*******************************
            If String.IsNullOrEmpty(p_oCotizacionActual.NoVisita) Then
                strNumeroVisita = AsignaNumeracionVisita(p_oCotizacionActual, p_oCotizacion)
                If Not String.IsNullOrEmpty(strNumeroVisita) Then
                    p_oCotizacionActual.NoVisita = strNumeroVisita
                Else
                    Return False
                End If
                '********************************
                'Se asigna el numero de Orden
                '*******************************
                p_oCotizacionActual.NoOrden = String.Format("{0}-01", strNumeroVisita)
            Else
                '***Valida si existe un mismo número de visita asociado a otra unidad
                If Not ValidaVisitaAsociada(p_oCotizacionActual.NoVisita, p_oCotizacionActual.CodigoUnidad) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaVisita, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Return False
                End If
                '********************************
                'Se asigna el numero de Orden
                '*******************************
                AsignaNumeroOTSiguiente(p_oCotizacionActual)
            End If

            'Correción para problema que no se le asignaba el número de Orden, número de ID y número de paquete padre correctamente a las líneas ya que faltaba el número de orden.
            If Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) Then
                If ValidaSiExisteOT(p_oCotizacionActual.NoOrden) Then Return False
                p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = p_oCotizacionActual.NoOrden
            End If



            '********************************
            'Se asignan valores manuales
            '*******************************
            AsignaValoresManualesCrear(p_oCotizacionActual)
            '********************************
            'Valida Paquetes
            '*******************************
            ManejarPaquete(p_oPaqueteList, oPaqueteListResultado)
            '********************************
            'Mensaje de Procesando Lineas
            '*******************************
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoLineas, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            '*********Declaro List para la lineas de la cotización *************
            For rowContador As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(rowContador)
                rowCotizacion = New oLineasDocumento()
                '*******************************************
                'Asigna Lineas a Cotizacion Data Contract
                '*******************************************
                If AsignaValorACotizacionDataContract(p_oCotizacion, rowCotizacion) Then
                    '********************************
                    'Valida si la linea pertenece a un paquete
                    '*******************************
                    ValidaPaquete(rowCotizacion, oPaqueteListResultado, p_oCotizacion, False, String.Empty, String.Empty)
                    If rowCotizacion.Aprobado = ArticuloAprobado.scgSi Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & ":  " & rowCotizacion.VisOrder + 1 & My.Resources.Resource.Separador & rowCotizacion.ItemCode & "     " & rowCotizacion.Description, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        If Not EsReserva(p_oCotizacion, rowCotizacion) Then
                            '********************************
                            'Carga Articulo
                            '*******************************
                            If oArticulo.GetByKey(rowCotizacion.ItemCode) Then
                                AsignaTipoArticulo(rowCotizacion, oArticulo)
                                '********************************
                                'Valida Informacion Articulo
                                '*******************************
                                If ValidaArticulo(rowCotizacion, oArticulo, p_oBodegaCentroCosto, p_oConfiguracionSucursal, blnMensajeCCOT, False, String.Empty, String.Empty) Then
                                    If rowCotizacion.Procesar Then
                                        '********************************
                                        'Asigna ID
                                        'Carga datos lineas cotizacion
                                        '*******************************
                                        DatosLineasCotizacion(rowCotizacion, p_oCotizacionActual, False)
                                        '********************************
                                        'Valida disponibilidad articulo
                                        '********************************
                                        ValidaDisponibilidadArticulo(rowCotizacion, oArticulo, p_oConfiguracionSucursal)
                                        '********************************
                                        'Maneja Cantidades,Estados y Documentos
                                        '********************************
                                        ManejaLineasCrear(rowCotizacion, p_oCotizacionActual, p_oRequisicionDataLineas, p_oControlColaboradorList, False)
                                    End If
                                Else
                                    If blnMensajeCCOT Then
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.RevisarConfCentroCostoXOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Else
                                        SBO_Application.MessageBox(m_strCentroCosto + My.Resources.Resource.El_Item & rowCotizacion.ItemCode & " " & rowCotizacion.Description & My.Resources.Resource.MalConfigurado)
                                    End If
                                End If
                            End If
                        Else
                            CompletarDatosReserva(p_oCotizacion, rowCotizacion, p_oCotizacionActual)
                        End If
                    ElseIf rowCotizacion.Aprobado = ArticuloAprobado.scgFalta Then
                        '***** Asigna ID en Aprobado= Falta de Aprobación *****
                        AsignaIDFaltaAprobacion(p_oCotizacion, rowCotizacion, p_oCotizacionActual)
                    End If
                    '********************************
                    'Asigna Lineas a Cotizacion data Contract
                    '*******************************
                    ReplicaValorACotizacion(p_oCotizacion, rowCotizacion)
                End If
            Next
            '*********************************
            'Carga Ubicaciones
            '*********************************
            If p_oConfiguracionSucursal.Item(0).UsaUbicaciones = True Then
                CargaUbicaciones(p_oRequisicionDataLineas)
            End If
            '*********************************
            'Actualiza Encabezado Cotizacion
            '*********************************
            AsignaValorEncabezadoCotizacion(p_oCotizacion, p_oCotizacionActual)
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oArticulo)
        End Try
    End Function

    Private Sub CompletarDatosReserva(ByRef Cotizacion As SAPbobsCOM.Documents, ByRef LineaCotizacion As oLineasDocumento, ByRef CotizacionCache As oDocumento)
        Try
            If Not String.IsNullOrEmpty(LineaCotizacion.NoOrden) Then
                LineaCotizacion.NoOrden = CotizacionCache.NoOrden
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AsignaTipoArticulo(ByRef p_rowCotizacion As oLineasDocumento, ByRef p_oArticulo As SAPbobsCOM.IItems)
        Try
            If Not String.IsNullOrEmpty(p_oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value) Then
                p_rowCotizacion.TipoArticulo = CInt(p_oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Private Function EsReserva(ByRef Cotizacion As SAPbobsCOM.Documents, ByRef LineaCotizacion As oLineasDocumento) As Boolean
        Dim Resultado = False
        Dim ID As String = String.Empty
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Try
            ID = LineaCotizacion.ID
            SerieCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value
            NumeroCita = Cotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value

            If Not String.IsNullOrEmpty(ID) AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                Resultado = True
            End If

            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Function ValidaProcesoActualizar(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_rowCotizacion As oLineasDocumento, ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List) As Boolean
        Dim valor As String = String.Empty
        Try
            For Each rowCotizacionInicial As oLineasDocumento In oCotizacionInicial.Lineas
                With rowCotizacionInicial
                    If p_rowCotizacion.ItemCode = .ItemCode And p_rowCotizacion.ID = .ID And p_rowCotizacion.LineNum = .LineNum Then
                        If (p_rowCotizacion.Aprobado <> .AprobadoOriginal) Or p_rowCotizacion.Quantity <> .OriginalQuantity _
                            Or p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado _
                            Or (p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi And p_rowCotizacion.Trasladado = Trasladado.NoProcesado And p_rowCotizacion.TipoArticulo <> "2") _
                            Or (p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo And p_rowCotizacion.Trasladado = Trasladado.SI) _
                            Or (p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo And p_rowCotizacion.Comprar = "Y") _
                            Or Not p_rowCotizacion.EmpleadoAsignado Is Nothing And p_rowCotizacion.TipoArticulo = "2" _
                            Or String.IsNullOrEmpty(p_rowCotizacion.ID) Then

                            p_rowCotizacion.OriginalQuantity = .OriginalQuantity
                            '******************************
                            ' Valida si pertenece a OT Hija
                            '******************************
                            If p_rowCotizacion.OTHija = enumTrasladadoOTHija.scgOTHijaNO Then
                                '*********************
                                ' Valida cantidades
                                '*********************
                                Select Case p_rowCotizacion.Quantity
                                    Case Is < .OriginalQuantity
                                        If p_oConfiguracionSucursalList.Item(0).UsuarioDisminuye Then
                                            p_rowCotizacion.OriginalQuantity = .OriginalQuantity
                                            If p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto Or p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro Then
                                                p_rowCotizacion.RequisicionDevolucion = True
                                            End If
                                        Else
                                            p_oCotizacion.Lines.Quantity = .OriginalQuantity
                                            SBO_Application.MessageBox(My.Resources.Resource.CantidadNoDisminuye + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                            Return False
                                        End If
                                    Case Is > .OriginalQuantity
                                        If p_rowCotizacion.TipoArticulo <> TipoArticulo.Servicio Then
                                            SBO_Application.MessageBox(My.Resources.Resource.LacantidadDelItem + "   " + p_rowCotizacion.ItemCode + ")    " + p_rowCotizacion.Description + My.Resources.Resource.CantidadNoAumenta + vbCrLf + My.Resources.Resource.AgregueLineaParaCantidad)
                                            p_oCotizacion.Lines.Quantity = .OriginalQuantity
                                            Return False
                                        End If
                                End Select
                                '*********************
                                ' Valida Aprobado
                                '*********************
                                Select Case p_rowCotizacion.Aprobado
                                    Case Is = .AprobadoOriginal
                                        Select Case p_rowCotizacion.TipoArticulo
                                            Case TipoArticulo.Repuesto
                                                Select Case p_rowCotizacion.Aprobado
                                                    Case ArticuloAprobado.scgSi
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf (p_rowCotizacion.Trasladado = Trasladado.NoProcesado Or p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado) Then
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI AndAlso p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                                            SBO_Application.MessageBox(My.Resources.Resource.DevolverItemNoAprob + ":     " + p_rowCotizacion.ItemCode + ")      " + p_rowCotizacion.Description)
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        Else
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        End If
                                                    Case ArticuloAprobado.scgNo
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Comprar = "Y" And p_rowCotizacion.Quantity = p_rowCotizacion.CantidadRecibida Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        End If
                                                End Select
                                            Case TipoArticulo.Servicio
                                                If String.IsNullOrEmpty(rowCotizacionInicial.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    If (Not String.IsNullOrEmpty(rowCotizacionInicial.EstadoActividad) Or IsNothing(rowCotizacionInicial.EstadoActividad)) Then
                                                        Return True
                                                    End If
                                                    Return False
                                                End If
                                            Case TipoArticulo.Suministro
                                                Select Case p_rowCotizacion.Aprobado
                                                    Case ArticuloAprobado.scgSi
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf (p_rowCotizacion.Trasladado = Trasladado.NoProcesado Or p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado) Then
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI AndAlso p_rowCotizacion.Quantity < p_rowCotizacion.OriginalQuantity AndAlso p_rowCotizacion.Quantity > 0 Then
                                                            SBO_Application.MessageBox(My.Resources.Resource.DevolverItemNoAprob + ":     " + p_rowCotizacion.ItemCode + "      " + p_rowCotizacion.Description)
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        Else
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        End If
                                                    Case ArticuloAprobado.scgNo
                                                        If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return False
                                                        ElseIf p_rowCotizacion.Comprar = "Y" And p_rowCotizacion.Quantity = p_rowCotizacion.CantidadRecibida Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        ElseIf p_rowCotizacion.Trasladado = Trasladado.SI Then
                                                            p_rowCotizacion.RequisicionDevolucion = True
                                                            oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                            Return True
                                                        End If
                                                End Select
                                            Case TipoArticulo.ServicioExterno
                                                If p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi And p_rowCotizacion.Trasladado = Trasladado.NoProcesado Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                End If
                                            Case TipoArticulo.OtrosCostos
                                                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                            Case TipoArticulo.OtrosIngresos
                                                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                            Case TipoArticulo.Otros
                                                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                Else
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                        End Select
                                    Case Is <> .AprobadoOriginal
                                        If p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo Then
                                            If p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto Or p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro Then
                                                If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                    SBO_Application.MessageBox(My.Resources.Resource.PendienteProcesarRequisicion + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion > 0 Then
                                                    SBO_Application.MessageBox(My.Resources.Resource.PendienteProcesarRequisicion + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                                SBO_Application.MessageBox(My.Resources.Resource.DevolverItemNoAprob + ":     " + p_rowCotizacion.ItemCode + ")      " + p_rowCotizacion.Description)
                                                p_rowCotizacion.RequisicionDevolucion = True
                                                oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                Return True
                                            ElseIf p_rowCotizacion.TipoArticulo = TipoArticulo.ServicioExterno Then
                                                'Ajuste DITEC
                                                If (p_rowCotizacion.CantidadSolicitada.GetValueOrDefault > 0 Or p_rowCotizacion.CantidadRecibida.GetValueOrDefault > 0) Then
                                                    SBO_Application.MessageBox(My.Resources.Resource.PendienteProcesoCompra + "   " + p_rowCotizacion.ItemCode + " - " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = Trasladado.NO
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                End If
                                            End If
                                        ElseIf p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi Then
                                            If p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto Or p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro Then
                                                If p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteBodega > 0 Then
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteBodega And p_rowCotizacion.CantidadPendienteDevolucion = p_rowCotizacion.Quantity Then
                                                    SBO_Application.MessageBox(My.Resources.Resource.PendienteProcesarRequisicion + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return False
                                                ElseIf p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado Then
                                                    oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                                    Return True
                                                End If
                                            End If
                                        End If
                                End Select
                                Return True
                            ElseIf p_rowCotizacion.OTHija = enumTrasladadoOTHija.scgOTHijaSI Then
                                oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                                SBO_Application.MessageBox(My.Resources.Resource.PerteneceOTHija + "   " + p_rowCotizacion.ItemCode + ")   " + p_rowCotizacion.Description)
                                Return False
                            End If
                        End If
                        oCotizacionInicial.Lineas.Remove(rowCotizacionInicial)
                        Return False
                    End If
                End With
            Next
            If p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi And String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                p_rowCotizacion.EsAdicional = True
                Return True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Private Sub AsignaValoresManualesCrear(ByRef p_oCotizacionActual As oDocumento)
        Try
            '********************************
            'Se asignan valores manuales
            '*******************************
            p_oCotizacionActual.EstadoCotizacionID = "1"
            p_oCotizacionActual.EstadoCotizacion = "No iniciada"
            p_oCotizacionActual.GeneraRecepcion = "2"
            p_oCotizacionActual.FechaCreacionOT = Utilitarios.RetornaFechaActual(m_oCompany.CompanyDB, m_oCompany.Server)
            p_oCotizacionActual.HoraCreacionOT = m_oCompany.GetCompanyTime()
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub AsignaValoresManualesActualizar(ByRef p_oCotizacionActual As oDocumento)
        Try
            '********************************
            'Se asignan valores manuales
            '*******************************
            p_oCotizacionActual.GeneraRecepcion = "2"
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub AsignaNumeroOTSiguiente(ByRef p_oCotizacionActual As oDocumento)
        Try
            '*************Variables *************
            Dim strNoOrdenSiguiente As String = String.Empty
            Dim strNoVisita As String = String.Empty
            Dim strNoOTRef As String = String.Empty
            Dim query As String = String.Empty
            If Not String.IsNullOrEmpty(p_oCotizacionActual.NoVisita) Then
                query = DMS_Connector.Queries.GetStrSpecificQuery("strNumeroOTSiguiente")
                strNoOrdenSiguiente = Utilitarios.EjecutarConsulta(String.Format(query, p_oCotizacionActual.NoVisita))
                If Not String.IsNullOrEmpty(strNoOrdenSiguiente) Then
                    If Integer.Parse(strNoOrdenSiguiente) < 10 Then
                        strNoOrdenSiguiente = String.Format("0{0}", strNoOrdenSiguiente)
                    End If
                    p_oCotizacionActual.NoOrden = String.Format("{0}-{1}", p_oCotizacionActual.NoVisita, strNoOrdenSiguiente)
                End If
            ElseIf Not String.IsNullOrEmpty(p_oCotizacionActual.NoOTReferencia) Then
                strNoOTRef = p_oCotizacionActual.NoOTReferencia.Trim()
                strNoVisita = strNoOTRef.Substring(0, strNoOTRef.Length() - 3)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Function AsignaValorACotizacionDataContract(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                                        ByRef p_rowCotizacion As oLineasDocumento, Optional ByVal p_AsignaTipoArticulo As Boolean = False) As Boolean
        Try
            Dim strTipoArticulo As String = String.Empty
            With p_rowCotizacion
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                    .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                    .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                End If
                .DocEntry = p_oCotizacion.Lines.DocEntry
                .LineNum = p_oCotizacion.Lines.LineNum
                .ItemCode = p_oCotizacion.Lines.ItemCode
                .Quantity = p_oCotizacion.Lines.Quantity
                .TreeType = p_oCotizacion.Lines.TreeType
                .VisOrder = p_oCotizacion.Lines.VisualOrder
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.ItemDescription) Then
                    .Description = p_oCotizacion.Lines.ItemDescription
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                    .IdRepxOrd = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                    .ID = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                End If
                .Aprobado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                .Trasladado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value) Then
                    .Comprar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value) Then
                    .OTHija = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Then
                    .DuracionEstandar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                Else
                    .DuracionEstandar = 0
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                    .EmpleadoAsignado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()) Then
                    .NombreEmpleado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                    .EstadoActividad = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                End If
                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                    .TipoArticulo = CInt(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                Else
                    If p_AsignaTipoArticulo Then
                        strTipoArticulo = CargaTipoArticulo(.ItemCode)
                        If Not String.IsNullOrEmpty(strTipoArticulo) Then
                            .TipoArticulo = CInt(strTipoArticulo)
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = strTipoArticulo
                        End If
                    End If
                End If
                If .TipoArticulo = 2 Then
                    If String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1"
                    End If
                End If
                .CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value
                .CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                .CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                .CantidadPendienteBodega = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value
                .CantidadPendienteTraslado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value
                .CantidadPendienteDevolucion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value
            End With
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Private Function CargaTipoArticulo(ByRef p_strItemCode As String) As String
        Dim oArticulo As SAPbobsCOM.IItems
        Try
            oArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oArticulo.GetByKey(p_strItemCode)
            If Not String.IsNullOrEmpty(oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value) Then
                Return oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value.ToString
            End If
            Return String.Empty
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function
    Private Sub ReplicaValorACotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                        ByRef p_rowCotizacion As oLineasDocumento)
        Try
            '************************************
            'Asigna Valores Lineas Cotizacion
            '************************************
            With p_rowCotizacion
                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = .Aprobado
                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = .Trasladado
                p_oCotizacion.Lines.Quantity = .Quantity
                If Not String.IsNullOrEmpty(.NoOrden) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = .NoOrden
                End If
                If Not String.IsNullOrEmpty(.TipoArticulo.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = Convert.ToString(.TipoArticulo)
                End If
                If Not String.IsNullOrEmpty(.Sucursal) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = .Sucursal
                End If
                If Not String.IsNullOrEmpty(.Comprar) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = .Comprar
                End If
                If Not String.IsNullOrEmpty(.FaseProduccion) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = .FaseProduccion
                End If
                If Not String.IsNullOrEmpty(.CentroCosto) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = .CentroCosto
                End If
                If Not String.IsNullOrEmpty(.IdRepxOrd.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = .IdRepxOrd
                End If
                If Not String.IsNullOrEmpty(.ID) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = .ID
                End If
                If Not String.IsNullOrEmpty(.OTHija.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = .OTHija
                End If
                If Not String.IsNullOrEmpty(.DuracionEstandar.ToString()) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = .DuracionEstandar
                Else
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = 0
                End If
                If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                End If
                If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                End If
                If Not String.IsNullOrEmpty(.EstadoActividad) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                ElseIf String.IsNullOrEmpty(.EstadoActividad) And (.Aprobado = 1) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1"
                End If
                If Not String.IsNullOrEmpty(.PaquetePadre) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = .PaquetePadre
                End If
                If Not String.IsNullOrEmpty(.Resultado) Then
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = .Resultado
                End If

                If .CantidadRecibida <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = .CantidadRecibida
                If .CantidadSolicitada <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = .CantidadSolicitada
                If .CantidadPendiente <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = .CantidadPendiente
                If .CantidadPendienteBodega <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = .CantidadPendienteBodega
                If .CantidadPendienteTraslado <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = .CantidadPendienteTraslado
                If .CantidadPendienteDevolucion <> -1 Then p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = .CantidadPendienteDevolucion

                If String.IsNullOrEmpty(.NoOrden) Then
                    'Todas las líneas procesadas previo a la orden de trabajo llevan el indicador de reserva (Prepicking)
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Prepicking").Value = "Y"
                End If

            End With
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub AsignaValorEncabezadoCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                                ByRef p_oCotizacionActual As oDocumento)
        Try
            '************************************
            'Asigna Valores Encabezado Cotizacion
            '************************************
            With p_oCotizacionActual
                If Not String.IsNullOrEmpty(.NoOrden) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .NoOrden
                End If
                If Not String.IsNullOrEmpty(.EstadoCotizacion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = .EstadoCotizacion
                End If
                If Not String.IsNullOrEmpty(.EstadoCotizacionID) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = .EstadoCotizacionID
                End If
                If .FechaCreacionOT IsNot Nothing Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = .FechaCreacionOT
                End If
                If .HoraCreacionOT IsNot Nothing Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = .HoraCreacionOT
                End If
                If Not String.IsNullOrEmpty(.GeneraRecepcion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = .GeneraRecepcion
                End If
                If Not String.IsNullOrEmpty(.OTPadre) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value = .OTPadre
                End If
                If Not String.IsNullOrEmpty(.NoOTReferencia) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value = .NoOTReferencia
                End If
                If Not String.IsNullOrEmpty(.NoVisita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = .NoVisita
                End If
            End With
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Function CrearControlColaborador(ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                             ByRef p_oCotizacionActual As oDocumento) As Boolean
        Try
            '*********************Variables ******************
            Dim oControladorOrdeTrabajo As ControladorOrdenTrabajo
            If p_oControlColaboradorList.Count > 0 Then
                oControladorOrdeTrabajo = New ControladorOrdenTrabajo(m_oCompany, SBO_Application)
                Return oControladorOrdeTrabajo.CrearControlColaborador(p_oControlColaboradorList, p_oCotizacionActual)
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    'Private Function CrearOrdenTrabajo(ByRef oCotizacionEncabezadoList As CotizacionEncabezado_List, _
    '                                   ByRef p_strDocEntryOT As String) As Boolean
    '    Try
    '        Dim UDOOrden As UDOOrden
    '        Dim UDOEncabezado As EncabezadoUDOOrden
    '        Dim strCode As String = String.Empty

    '        UDOOrden = New UDOOrden(m_oCompany)
    '        UDOEncabezado = New EncabezadoUDOOrden()
    '        strCode = ObtenerCodeSiguienteOT()
    '        UDOEncabezado.Code = strCode
    '        With UDOEncabezado
    '            .U_DocEntry = oCotizacionEncabezadoList.Item(0).DocEntry
    '            .U_NoOT = oCotizacionEncabezadoList.Item(0).NoOrden
    '            .U_NoUni = oCotizacionEncabezadoList.Item(0).CodigoUnidad
    '            .U_NoCon = oCotizacionEncabezadoList.Item(0).Cono
    '            .U_Ano = oCotizacionEncabezadoList.Item(0).Year
    '            .U_Plac = oCotizacionEncabezadoList.Item(0).Placa
    '            .U_Marc = oCotizacionEncabezadoList.Item(0).DescripcionMarca
    '            .U_Esti = oCotizacionEncabezadoList.Item(0).DescripcionEstilo
    '            .U_Mode = oCotizacionEncabezadoList.Item(0).DescripcionModelo
    '            .U_CMar = oCotizacionEncabezadoList.Item(0).CodigoMarca
    '            .U_CEst = oCotizacionEncabezadoList.Item(0).CodigoEstilo
    '            .U_CMod = oCotizacionEncabezadoList.Item(0).CodigoModelo
    '            .U_NoVis = oCotizacionEncabezadoList.Item(0).NoVisita
    '            .U_VIN = oCotizacionEncabezadoList.Item(0).NumeroVIN
    '            .U_km = oCotizacionEncabezadoList.Item(0).Kilometraje.ToString()
    '            .U_TipOT = oCotizacionEncabezadoList.Item(0).TipoOT
    '            .U_Sucu = oCotizacionEncabezadoList.Item(0).Sucursal
    '            .U_CodCli = oCotizacionEncabezadoList.Item(0).CardCode
    '            .U_NCli = oCotizacionEncabezadoList.Item(0).CardName
    '            .U_CodCOT = oCotizacionEncabezadoList.Item(0).CodigoClienteOT
    '            .U_NCliOT = oCotizacionEncabezadoList.Item(0).NombreClienteOT
    '            .U_FCom = Nothing
    '            .U_HCom = Nothing
    '            .U_FApe = oCotizacionEncabezadoList.Item(0).FechaRecepcion
    '            .U_HApe = oCotizacionEncabezadoList.Item(0).HoraRecepcion
    '            .U_FFin = Nothing
    '            .U_HFin = Nothing
    '            .U_FCerr = Nothing
    '            .U_FFact = Nothing
    '            .U_FEntr = Nothing
    '            .U_OTRef = oCotizacionEncabezadoList.Item(0).NoOTReferencia
    '            .U_NGas = oCotizacionEncabezadoList.Item(0).NivelGasolina.ToString()
    '            .U_HMot = String.Empty
    '            .U_EstO = "1"
    '            .U_DEstO = My.Resources.Resource.EstadoOrdenNoIniciada
    '            .U_Ase = ""
    '            .U_EncO = ""
    '            .U_Obse = oCotizacionEncabezadoList.Item(0).Observaciones
    '        End With
    '        UDOOrden.Encabezado = UDOEncabezado
    '        UDOOrden.Company = m_oCompany
    '        UDOOrden.Insert()
    '        p_strDocEntryOT = UDOOrden.Encabezado.Code
    '        Return True
    '    Catch ex As Exception
    '        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '        Return False
    '    End Try
    'End Function

    'Private Function ObtenerCodeSiguienteOT() As String
    '    Try
    '        Dim strValor As String
    '        strValor = Utilitarios.EjecutarConsulta(" select MAX(DocEntry + 1) from [@SCGD_OT] with(nolock) ",
    '                                                    m_oCompany.CompanyDB, m_oCompany.Server)
    '        If String.IsNullOrEmpty(strValor) Then
    '            Return String.Empty
    '        Else
    '            Return strValor
    '        End If
    '    Catch ex As Exception
    '        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '    End Try
    'End Function

    Public Sub ReplicaDatosCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                      ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, _
                                      ByRef p_oCotizacionList As Cotizacion_List)
        Try
            p_oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                      SAPbobsCOM.Documents)
            If Not String.IsNullOrEmpty(p_oCotizacionEncabezadoList.Item(0).DocEntry) Then
                If p_oCotizacion.GetByKey(p_oCotizacionEncabezadoList.Item(0).DocEntry) Then
                    '************************************
                    'Asigna Valores Encabezado Cotizacion
                    '************************************
                    With p_oCotizacionEncabezadoList.Item(0)
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .NoOrden
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = .EstadoCotizacion
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = .EstadoCotizacionID
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = .FechaCreacionOT
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = .HoraCreacionOT
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = .GeneraRecepcion
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value = .OTPadre
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value = .NoOTReferencia
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = .NoVisita
                        If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value <> Nothing AndAlso p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value <> Nothing Then
                            p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = .FechaCreacionOT
                            p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = .HoraCreacionOT
                        End If
                    End With
                    '************************************
                    'Asigna Valores Lineas Cotizacion
                    '************************************
                    For contador As Integer = 0 To p_oCotizacion.Lines.Count - 1
                        p_oCotizacion.Lines.SetCurrentLine(contador)
                        For Each rowCotizacion As Cotizacion In p_oCotizacionList
                            If p_oCotizacion.Lines.LineNum = rowCotizacion.LineNum And p_oCotizacion.Lines.ItemCode = rowCotizacion.ItemCode Then
                                With rowCotizacion
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = .Aprobado
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = .Trasladado
                                    p_oCotizacion.Lines.Quantity = .Quantity
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = .NoOrden
                                    If Not String.IsNullOrEmpty(.TipoArticulo) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = .TipoArticulo
                                    End If
                                    If Not String.IsNullOrEmpty(.Sucursal) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = .Sucursal
                                    End If
                                    If Not String.IsNullOrEmpty(.Comprar) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = .Comprar
                                    End If
                                    If Not String.IsNullOrEmpty(.FaseProduccion) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = .FaseProduccion
                                    End If
                                    If Not String.IsNullOrEmpty(.CentroCosto) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = .CentroCosto
                                    End If
                                    If Not String.IsNullOrEmpty(.IdRepxOrd.ToString()) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = .IdRepxOrd
                                    End If
                                    If Not String.IsNullOrEmpty(.ID) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = .ID
                                    End If
                                    If Not String.IsNullOrEmpty(.OTHija.ToString()) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = .OTHija
                                    End If
                                    If Not String.IsNullOrEmpty(.DuracionEstandar.ToString()) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = .DuracionEstandar
                                    Else
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = 0
                                    End If
                                    If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                                    End If
                                    If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                                    End If
                                    If Not String.IsNullOrEmpty(.EstadoActividad) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                                    End If
                                    If Not String.IsNullOrEmpty(.PaquetePadre) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = .PaquetePadre
                                    End If
                                    If Not String.IsNullOrEmpty(.Resultado) Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = .Resultado
                                    End If
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = .CantidadRecibida
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = .CantidadSolicitada
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = .CantidadPendiente
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = .CantidadPendienteBodega
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = .CantidadPendienteTraslado
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = .CantidadPendienteDevolucion
                                End With
                                p_oCotizacionList.Remove(rowCotizacion)
                                Exit For
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ManejaLineasActualizar(ByRef p_rowCotizacion As oLineasDocumento, _
                                      ByRef p_oArticulo As SAPbobsCOM.IItems, _
                                      ByRef p_oCotizacionActual As oDocumento, _
                                      ByRef p_oRequisicionDataList As RequisicionData_List, _
                                      ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                      ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, ByVal EsReservacion As Boolean)
        Try
            '*****************Variables **********
            Dim blnLineaModificada As Boolean = False
            With p_rowCotizacion
                '********************************
                'Se valida según tipo de articulo
                '*******************************
                Select Case CInt(.TipoArticulo)
                    Case TipoArticulo.Repuesto
                        Select Case .ProcesamientoLinea
                            Case ProcesamientoLinea.Requisicion
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    If Not .RequisicionDevolucion Then
                                        .Trasladado = Trasladado.PendienteBodega
                                        .Comprar = "N"
                                        .CantidadRecibida = 0
                                        .CantidadPendiente = 0
                                        .CantidadSolicitada = 0
                                        .CantidadPendienteBodega = .Quantity
                                        .CantidadPendienteTraslado = 0
                                        .CantidadPendienteDevolucion = 0
                                        '********************************
                                        'Carga Requisicion Data Contract
                                        '********************************
                                        AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado, EsReservacion)
                                        blnLineaModificada = True
                                    End If
                                ElseIf .TipoMovimiento = TipoMovimiento.Comprar Then
                                    .Trasladado = Trasladado.NO
                                    .Comprar = "Y"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = .Quantity
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                ElseIf .TipoMovimiento = TipoMovimiento.Rechazar Then
                                    .Aprobado = ArticuloAprobado.scgNo
                                    .Trasladado = Trasladado.NoProcesado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                ElseIf .TipoMovimiento = TipoMovimiento.Trasladar Then
                                    .Trasladado = Trasladado.PendienteTraslado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = .Quantity
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.RequisicionDevolucion
                                If .RequisicionDevolucion Then

                                    If .OriginalQuantity = .Quantity Then
                                        .CantidadPendienteDevolucion = .Quantity
                                    Else
                                        .CantidadPendienteDevolucion = .OriginalQuantity - .Quantity
                                    End If
                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = .CantidadRecibida - .CantidadPendienteDevolucion
                                    '.CantidadPendiente = .CantidadPendiente
                                    '.CantidadSolicitada = .CantidadSolicitada
                                    '.CantidadPendienteBodega = .CantidadPendienteBodega
                                    '.CantidadPendienteTraslado = .CantidadPendienteTraslado
                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Devolucion, EsReservacion)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.TrasladoBodega
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = p_rowCotizacion.Quantity
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado, EsReservacion)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.AnularRequisicion
                            Case ProcesamientoLinea.AnularTrasladoBodega
                                .Trasladado = Trasladado.NoProcesado
                                .Comprar = "N"
                                .CantidadRecibida = 0
                                .CantidadPendiente = 0
                                .CantidadSolicitada = 0
                                .CantidadPendienteBodega = 0
                                .CantidadPendienteTraslado = 0
                                .CantidadPendienteDevolucion = 0
                                blnLineaModificada = True
                            Case ProcesamientoLinea.AnularRequisicionDevolucion
                        End Select
                        '************************************************************************************
                        'Valida si la linea fue modificada sino para dejar los valores originales de la linea
                        '************************************************************************************
                        If blnLineaModificada = False Then
                            .CantidadRecibida = -1
                            .CantidadPendiente = -1
                            .CantidadSolicitada = -1
                            .CantidadPendienteBodega = -1
                            .CantidadPendienteTraslado = -1
                            .CantidadPendienteDevolucion = -1
                        End If
                    Case TipoArticulo.Servicio
                        Select Case .ProcesamientoLinea
                            Case ProcesamientoLinea.AgregarControlColaborador
                                '********************************
                                'Carga Control Colaborador Data Contract
                                '********************************
                                If Not String.IsNullOrEmpty(p_rowCotizacion.EmpleadoAsignado) Then
                                    AgregarControlColaboradorDataContract(p_rowCotizacion, p_oControlColaboradorList)
                                End If

                        End Select
                    Case TipoArticulo.ServicioExterno
                        Select Case .ProcesamientoLinea
                            Case ProcesamientoLinea.ProcesaServicioExterno
                                .Trasladado = Trasladado.NO
                                .Comprar = "Y"
                                .CantidadRecibida = 0
                                .CantidadPendiente = .Quantity
                                .CantidadSolicitada = 0
                                .CantidadPendienteBodega = 0
                                .CantidadPendienteTraslado = 0
                                .CantidadPendienteDevolucion = 0
                                blnLineaModificada = True
                            Case ProcesamientoLinea.AnulaServicioExterno
                                If .Quantity = .CantidadPendiente Then
                                    .Trasladado = Trasladado.NoProcesado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                End If
                        End Select
                        '************************************************************************************
                        'Valida si la linea fue modificada sino para dejar los valores originales de la linea
                        '************************************************************************************
                        If blnLineaModificada = False Then
                            .CantidadRecibida = -1
                            .CantidadPendiente = -1
                            .CantidadSolicitada = -1
                            .CantidadPendienteBodega = -1
                            .CantidadPendienteTraslado = -1
                            .CantidadPendienteDevolucion = -1
                        End If
                    Case TipoArticulo.Suministro
                        Select Case .ProcesamientoLinea
                            Case ProcesamientoLinea.Requisicion
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    If Not .RequisicionDevolucion Then
                                        .Trasladado = Trasladado.PendienteBodega
                                        .Comprar = "N"
                                        .CantidadRecibida = 0
                                        .CantidadPendiente = 0
                                        .CantidadSolicitada = 0
                                        .CantidadPendienteBodega = p_rowCotizacion.Quantity
                                        .CantidadPendienteTraslado = 0
                                        .CantidadPendienteDevolucion = 0
                                        '********************************
                                        'Carga Requisicion Data Contract
                                        '********************************
                                        AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado, EsReservacion)
                                        blnLineaModificada = True
                                    End If
                                ElseIf .TipoMovimiento = TipoMovimiento.Trasladar Then
                                    .Trasladado = Trasladado.PendienteTraslado
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = 0
                                    .CantidadPendienteTraslado = .Quantity
                                    .CantidadPendienteDevolucion = 0
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.RequisicionDevolucion
                                If .RequisicionDevolucion Then

                                    If .OriginalQuantity = .Quantity Then
                                        .CantidadPendienteDevolucion = .Quantity
                                    Else
                                        .CantidadPendienteDevolucion = .OriginalQuantity - .Quantity
                                    End If

                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = .CantidadRecibida - .CantidadPendienteDevolucion
                                    '.CantidadPendiente = .CantidadPendiente
                                    '.CantidadSolicitada = .CantidadSolicitada
                                    '.CantidadPendienteBodega = .CantidadPendienteBodega
                                    '.CantidadPendienteTraslado = .CantidadPendienteTraslado

                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Devolucion, EsReservacion)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.TrasladoBodega
                                '********************************
                                'Valida disponibilidad articulo
                                '********************************
                                ValidaDisponibilidadArticulo(p_rowCotizacion, p_oArticulo, p_oConfiguracionSucursalList)
                                '********************************
                                If .TipoMovimiento = TipoMovimiento.Requisicion Then
                                    .Trasladado = Trasladado.PendienteBodega
                                    .Comprar = "N"
                                    .CantidadRecibida = 0
                                    .CantidadPendiente = 0
                                    .CantidadSolicitada = 0
                                    .CantidadPendienteBodega = p_rowCotizacion.Quantity
                                    .CantidadPendienteTraslado = 0
                                    .CantidadPendienteDevolucion = 0
                                    '********************************
                                    'Carga Requisicion Data Contract
                                    '********************************
                                    AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado, False)
                                    blnLineaModificada = True
                                End If
                            Case ProcesamientoLinea.AnularRequisicion
                            Case ProcesamientoLinea.AnularTrasladoBodega
                                .Trasladado = Trasladado.NoProcesado
                                .Comprar = "N"
                                .CantidadRecibida = 0
                                .CantidadPendiente = 0
                                .CantidadSolicitada = 0
                                .CantidadPendienteBodega = 0
                                .CantidadPendienteTraslado = 0
                                .CantidadPendienteDevolucion = 0
                                blnLineaModificada = True
                            Case ProcesamientoLinea.AnularRequisicionDevolucion
                        End Select
                        '************************************************************************************
                        'Valida si la linea fue modificada sino para dejar los valores originales de la linea
                        '************************************************************************************
                        If blnLineaModificada = False Then
                            .CantidadRecibida = -1
                            .CantidadPendiente = -1
                            .CantidadSolicitada = -1
                            .CantidadPendienteBodega = -1
                            .CantidadPendienteTraslado = -1
                            .CantidadPendienteDevolucion = -1
                        End If
                End Select
            End With
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ManejaLineasCrear(ByRef p_rowCotizacion As oLineasDocumento, _
                                 ByRef p_oCotizacionActual As oDocumento, _
                                 ByRef p_oRequisicionDataList As RequisicionData_List, _
                                 ByRef p_oControlColaboradorList As ControlColaborador_List, ByVal EsReservacion As Boolean)
        Try
            '********************************
            'Se valida según tipo de articulo
            '*******************************
            Select Case CInt(p_rowCotizacion.TipoArticulo)
                Case TipoArticulo.Repuesto
                    Select Case p_rowCotizacion.TipoMovimiento
                        Case TipoMovimiento.Requisicion
                            p_rowCotizacion.Trasladado = Trasladado.PendienteBodega
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteTraslado = 0
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                            '********************************
                            'Carga Requisicion Data Contract
                            '********************************
                            AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado, EsReservacion)
                        Case TipoMovimiento.Comprar
                            p_rowCotizacion.Trasladado = Trasladado.NO
                            p_rowCotizacion.Comprar = "Y"
                            p_rowCotizacion.Resultado = "Para Comprar"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = 0
                            p_rowCotizacion.CantidadPendienteTraslado = 0
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                        Case TipoMovimiento.Trasladar
                            p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = 0
                            p_rowCotizacion.CantidadPendienteTraslado = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                        Case TipoMovimiento.Rechazar
                            p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo
                            p_rowCotizacion.Trasladado = Trasladado.NoProcesado
                    End Select
                Case TipoArticulo.Servicio
                    '***************************************
                    'Carga Control Colaborador Data Contract
                    '***************************************
                    If Not String.IsNullOrEmpty(p_rowCotizacion.EmpleadoAsignado) Then
                        AgregarControlColaboradorDataContract(p_rowCotizacion, p_oControlColaboradorList)
                    End If
                Case TipoArticulo.ServicioExterno
                    p_rowCotizacion.Trasladado = Trasladado.NO
                    p_rowCotizacion.Comprar = "Y"
                    p_rowCotizacion.CantidadRecibida = 0
                    p_rowCotizacion.CantidadPendiente = p_rowCotizacion.Quantity
                    p_rowCotizacion.CantidadSolicitada = 0
                    p_rowCotizacion.CantidadPendienteBodega = 0
                    p_rowCotizacion.CantidadPendienteTraslado = 0
                    p_rowCotizacion.CantidadPendienteDevolucion = 0
                Case TipoArticulo.Suministro
                    Select Case p_rowCotizacion.TipoMovimiento
                        Case TipoMovimiento.Requisicion
                            p_rowCotizacion.Trasladado = Trasladado.PendienteBodega
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteTraslado = 0
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                            '********************************
                            'Carga Requisicion Data Contract
                            '********************************
                            AgregarRequisicionDataContract(p_rowCotizacion, p_oCotizacionActual, p_oRequisicionDataList, TipoRequisicion.Traslado, EsReservacion)
                        Case Else
                            p_rowCotizacion.Trasladado = Trasladado.PendienteTraslado
                            p_rowCotizacion.Comprar = "N"
                            p_rowCotizacion.CantidadRecibida = 0
                            p_rowCotizacion.CantidadPendiente = 0
                            p_rowCotizacion.CantidadSolicitada = 0
                            p_rowCotizacion.CantidadPendienteBodega = 0
                            p_rowCotizacion.CantidadPendienteTraslado = p_rowCotizacion.Quantity
                            p_rowCotizacion.CantidadPendienteDevolucion = 0
                    End Select
            End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ManejarPaquete(ByRef p_oPaqueteList As Paquete_List, _
                              ByRef p_oPaqueteListResultado As Paquete_List)
        Try
            CargarPaquete(p_oPaqueteList, p_oPaqueteListResultado)
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargarPaquete(ByRef p_oPaqueteList As Paquete_List, _
                             ByRef p_oPaqueteListResultado As Paquete_List)
        '*************Objeto SAP ***************************
        Dim oDocumentoPaquete As SAPbobsCOM.ProductTrees
        Try
            '************Data Contract ****************************
            Dim oPaqueteResultado As Paquete
            oDocumentoPaquete = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees),  _
                                          SAPbobsCOM.ProductTrees)
            For Each rowPaquetePadre As Paquete In p_oPaqueteList
                If Not String.IsNullOrEmpty(rowPaquetePadre.ItemCodePadre) Then
                    If oDocumentoPaquete.GetByKey(rowPaquetePadre.ItemCodePadre) Then
                        For cont As Integer = 0 To oDocumentoPaquete.Items.Count - 1
                            oDocumentoPaquete.Items.SetCurrentLine(cont)
                            oPaqueteResultado = New Paquete()
                            With oPaqueteResultado
                                .AprobadoPadre = rowPaquetePadre.AprobadoPadre
                                .ItemCode = oDocumentoPaquete.Items.ItemCode
                                .ItemCodePadre = rowPaquetePadre.ItemCodePadre
                                .LineNumCotizacionPadre = rowPaquetePadre.LineNumCotizacionPadre
                                .TreeTypePadre = rowPaquetePadre.TreeTypePadre
                                .IDItem = rowPaquetePadre.IDItem
                                Select Case rowPaquetePadre.TreeTypePadre
                                    Case SAPbobsCOM.BoItemTreeTypes.iSalesTree
                                        .TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient
                                    Case SAPbobsCOM.BoItemTreeTypes.iTemplateTree
                                        .TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree
                                    Case Else
                                        .TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree
                                End Select
                            End With
                            p_oPaqueteListResultado.Add(oPaqueteResultado)
                        Next
                    End If
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oDocumentoPaquete)
        End Try
    End Sub

    Public Function ValidaPaquete(ByRef p_rowCotizacion As oLineasDocumento, _
                                  ByRef p_oPaqueteListResultado As Paquete_List, _
                                  ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal EsReservacion As Boolean, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String)

        Dim IDActividad As String = String.Empty

        Try
            For Each rowPaquete As Paquete In p_oPaqueteListResultado
                If rowPaquete.ItemCode = p_rowCotizacion.ItemCode And rowPaquete.TreeType = p_rowCotizacion.TreeType Then

                    IDActividad = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value

                    If String.IsNullOrEmpty(IDActividad) Then
                        If EsReservacion Then
                            IDActividad = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, rowPaquete.LineNumCotizacionPadre, NumeroSerieCita, ConsecutivoCita)
                        Else
                            IDActividad = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, rowPaquete.LineNumCotizacionPadre, p_rowCotizacion.NoOrden)
                        End If
                    End If

                    Select Case rowPaquete.AprobadoPadre
                        Case ArticuloAprobado.scgSi
                            If rowPaquete.TreeTypePadre = SAPbobsCOM.BoItemTreeTypes.iSalesTree And rowPaquete.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_rowCotizacion.Aprobado = ArticuloAprobado.scgSi
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi
                                If String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = IDActividad
                                End If
                            End If
                            If String.IsNullOrEmpty(p_rowCotizacion.PaquetePadre) Then
                                p_rowCotizacion.PaquetePadre = IDActividad
                            End If
                        Case ArticuloAprobado.scgNo
                            If rowPaquete.TreeTypePadre = SAPbobsCOM.BoItemTreeTypes.iSalesTree And rowPaquete.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_rowCotizacion.Aprobado = ArticuloAprobado.scgNo
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgNo
                                If String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = IDActividad
                                End If
                            End If
                        Case ArticuloAprobado.scgFalta
                    End Select
                    p_oPaqueteListResultado.Remove(rowPaquete)
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function


    Public Sub CargaUbicacionDefectoAlmacen(ByRef p_rowRequisicion As RequisicionData, _
                                            ByRef p_oCompany As SAPbobsCOM.Company)
        '***** Objetos SAP 
        Dim oIWarehouses As IWarehouses
        Try
            oIWarehouses = p_oCompany.GetBusinessObject(BoObjectTypes.oWarehouses)
            If oIWarehouses.GetByKey(p_rowRequisicion.BodegaUbicacion) Then
                If oIWarehouses.EnableBinLocations = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If oIWarehouses.DefaultBin > 0 Then
                        If p_rowRequisicion.RequisicionDevolucion Then
                            p_rowRequisicion.UbicacionDestino = oIWarehouses.DefaultBin.ToString().Trim()
                            p_rowRequisicion.DescripcionUbicacionDestino = CargaBinCode(oIWarehouses.DefaultBin)
                        Else
                            p_rowRequisicion.UbicacionOrigen = oIWarehouses.DefaultBin.ToString().Trim()
                            p_rowRequisicion.DescripcionUbicacionOrigen = CargaBinCode(oIWarehouses.DefaultBin)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.DestruirObjeto(oIWarehouses)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oIWarehouses)
        End Try
    End Sub

    Public Function CargaUbicacionDefectoGrupoArticulo(ByRef p_intGroupCode As Integer,
                                                       ByRef p_rowRequisicion As RequisicionData, _
                                                       ByRef p_oCompany As SAPbobsCOM.Company) As Boolean
        '***** Objetos SAP 
        Dim oIItemGroup As IItemGroups
        Dim oBodega As SAPbobsCOM.Warehouses
        Try
            oIItemGroup = p_oCompany.GetBusinessObject(BoObjectTypes.oItemGroups)
            oBodega = p_oCompany.GetBusinessObject(BoObjectTypes.oWarehouses)

            If oBodega.GetByKey(p_rowRequisicion.BodegaUbicacion) AndAlso oBodega.EnableBinLocations = BoYesNoEnum.tYES Then
                If oIItemGroup.GetByKey(p_intGroupCode) Then
                    If oIItemGroup.WarehouseInfo.Count > 0 Then
                        For cont As Integer = 0 To oIItemGroup.WarehouseInfo.Count - 1
                            oIItemGroup.WarehouseInfo.SetCurrentLine(cont)
                            If oIItemGroup.WarehouseInfo.WarehouseCode = p_rowRequisicion.BodegaUbicacion Then
                                If oIItemGroup.WarehouseInfo.DefaultBin > 0 Then
                                    If p_rowRequisicion.RequisicionDevolucion Then
                                        p_rowRequisicion.UbicacionDestino = oIItemGroup.WarehouseInfo.DefaultBin.ToString().Trim()
                                        p_rowRequisicion.DescripcionUbicacionDestino = CargaBinCode(oIItemGroup.WarehouseInfo.DefaultBin)
                                    Else
                                        p_rowRequisicion.UbicacionOrigen = oIItemGroup.WarehouseInfo.DefaultBin.ToString().Trim()
                                        p_rowRequisicion.DescripcionUbicacionOrigen = CargaBinCode(oIItemGroup.WarehouseInfo.DefaultBin)
                                    End If
                                    Return False
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Utilitarios.DestruirObjeto(oIItemGroup)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oIItemGroup)
        End Try
    End Function

    Public Function CargaBinCode(ByRef p_intAbsEntry As Integer) As String
        '****** Variable ***********
        Dim strBinCode As String = String.Empty
        Try
            If p_intAbsEntry > 0 Then
                strBinCode = Utilitarios.EjecutarConsulta(String.Format("SELECT ""BinCode"" FROM ""OBIN"" WHERE ""AbsEntry"" = {0}", p_intAbsEntry))
            End If
            Return strBinCode
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    Public Sub CargaUbicacionesDefecto(ByRef p_oRequisicionDataLineas As RequisicionData_List,
                                       ByRef p_oCompany As SAPbobsCOM.Company)
        '************Explicacion **************
        ' La jerarquia en SAP para ubicaciones es la siguiente 
        'Default Bin Location of Item > Default Bin Location of Item Group > Default Bin Location of Warehouse
        '***** Objetos SAP *****
        Dim oArticulo As SAPbobsCOM.IItems
        Try
            '***** Variables *****
            Dim blnSiguienteJerarquia As Boolean = False

            oArticulo = p_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            For Each rowRequisicion As RequisicionData In p_oRequisicionDataLineas
                blnSiguienteJerarquia = False
                If oArticulo.GetByKey(rowRequisicion.ItemCode) Then
                    If oArticulo.WhsInfo.Count > 0 Then
                        For cont As Integer = 0 To oArticulo.WhsInfo.Count - 1
                            oArticulo.WhsInfo.SetCurrentLine(cont)
                            If oArticulo.WhsInfo.WarehouseCode = rowRequisicion.BodegaUbicacion Then
                                If oArticulo.WhsInfo.DefaultBin > 0 Then
                                    If rowRequisicion.RequisicionDevolucion Then
                                        rowRequisicion.UbicacionDestino = oArticulo.WhsInfo.DefaultBin.ToString().Trim()
                                        rowRequisicion.DescripcionUbicacionDestino = CargaBinCode(oArticulo.WhsInfo.DefaultBin)
                                    Else
                                        rowRequisicion.UbicacionOrigen = oArticulo.WhsInfo.DefaultBin.ToString().Trim()
                                        rowRequisicion.DescripcionUbicacionOrigen = CargaBinCode(oArticulo.WhsInfo.DefaultBin)
                                    End If
                                    blnSiguienteJerarquia = False
                                Else
                                    blnSiguienteJerarquia = True
                                End If
                                Exit For
                            End If
                        Next
                    Else
                        blnSiguienteJerarquia = True
                    End If
                    If blnSiguienteJerarquia Then
                        If oArticulo.ItemsGroupCode > 0 Then
                            blnSiguienteJerarquia = CargaUbicacionDefectoGrupoArticulo(CInt(oArticulo.ItemsGroupCode), rowRequisicion, p_oCompany)
                            If blnSiguienteJerarquia Then
                                CargaUbicacionDefectoAlmacen(rowRequisicion, p_oCompany)
                            End If
                        Else
                            CargaUbicacionDefectoAlmacen(rowRequisicion, p_oCompany)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Utilitarios.DestruirObjeto(oArticulo)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oArticulo)
        End Try
    End Sub

    Public Sub CargaUbicaciones(ByRef p_oRequisicionDataLineas As RequisicionData_List)
        Try
            For Each rowRequisicion As RequisicionData In p_oRequisicionDataLineas
                If rowRequisicion.RequisicionDevolucion Then
                    rowRequisicion.BodegaUbicacion = rowRequisicion.BodegaDestino
                Else
                    rowRequisicion.BodegaUbicacion = rowRequisicion.BodegaOrigen
                End If
            Next
            CargaUbicacionesDefecto(p_oRequisicionDataLineas, m_oCompany)
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AgregarControlColaboradorDataContract(ByRef p_rowCotizacion As oLineasDocumento, _
                                                     ByRef p_oControlColaboradorList As ControlColaborador_List)
        Try
            '***************Variables ******************
            Dim oControlColaborador As ControlColaborador = New ControlColaborador()
            With oControlColaborador
                If Not String.IsNullOrEmpty(p_rowCotizacion.ID) Then .IdActividad = p_rowCotizacion.ID
                If Not String.IsNullOrEmpty(p_rowCotizacion.EmpleadoAsignado) Then .Colaborador = p_rowCotizacion.EmpleadoAsignado
                If Not String.IsNullOrEmpty(p_rowCotizacion.EstadoActividad) Then .Estado = p_rowCotizacion.EstadoActividad
                .CostoEstandar = 0
                .CostoReal = 0
            End With
            p_oControlColaboradorList.Add(oControlColaborador)
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AgregarRequisicionDataContract(ByRef p_rowCotizacion As oLineasDocumento, _
                                              ByRef p_rowCotizacionActual As oDocumento, _
                                              ByRef p_oRequisicionDataLineas As RequisicionData_List,
                                              ByRef p_intTipoRequisicion As Integer, ByVal EsReservacion As Boolean)
        Try
            '********************Variables ****************************
            Dim oRequisicionData As RequisicionData = New RequisicionData()
            With oRequisicionData
                .TipoArticulo = p_rowCotizacion.TipoArticulo
                '*****************************
                'Datos encabezado
                '*****************************
                .NoOrden = p_rowCotizacionActual.NoOrden
                .CodigoCliente = p_rowCotizacionActual.CardCode
                .NombreCliente = p_rowCotizacionActual.CardName
                If EsReservacion AndAlso p_intTipoRequisicion = TipoRequisicion.Traslado Then
                    p_intTipoRequisicion = TipoRequisicion.Reserva
                End If

                If EsReservacion AndAlso p_intTipoRequisicion = TipoRequisicion.Devolucion Then
                    p_intTipoRequisicion = TipoRequisicion.DevolucionReserva
                End If

                .CodigoTipoRequisicion = p_intTipoRequisicion
                .TipoDocumento = My.Resources.Resource.DocGeneraReq
                .Usuario = m_oCompany.UserName
                .Comentario = My.Resources.Resource.OT_Referencia & p_rowCotizacionActual.NoOrden & " " & My.Resources.Resource.Asesor & p_rowCotizacionActual.CodigoAsesor
                .Data = String.Empty
                .SucursalID = p_rowCotizacionActual.Sucursal
                .CodigoEstadoRequisicion = CodigoEstadoRequisicion.Pendiente
                .EstadoRequisicion = My.Resources.Resource.Pendiente
                .SerieCita = p_rowCotizacionActual.NoSerieCita
                .NumeroCita = p_rowCotizacionActual.NoCita
                '*****************************
                'Datos lineas
                '*****************************
                .ItemCode = p_rowCotizacion.ItemCode
                .Description = p_rowCotizacion.Description
                .TipoArticulo = p_rowCotizacion.TipoArticulo
                .CentroCosto = p_rowCotizacion.CentroCosto
                .CodigoEstadoLinea = CodigoEstadoRequisicion.Pendiente
                .EstadoLinea = My.Resources.Resource.Pendiente
                .LineNumOrigen = p_rowCotizacion.LineNum
                .DocumentoOrigen = p_rowCotizacionActual.DocEntry
                .LineaSucursalID = p_rowCotizacionActual.Sucursal
                .ID = p_rowCotizacion.ID
                Select Case p_intTipoRequisicion
                    Case TipoRequisicion.Traslado
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                .BodegaOrigen = p_rowCotizacion.BodegaRepuesto
                                .BodegaDestino = p_rowCotizacion.BodegaProceso
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                .BodegaOrigen = p_rowCotizacion.BodegaSuministro
                                .BodegaDestino = p_rowCotizacion.BodegaProceso
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        .TipoRequisicion = My.Resources.Resource.RequisicionTraslado
                        .RequisicionDevolucion = False
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadRecibida = 0
                        If m_oCompany.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                    Case TipoRequisicion.Devolucion
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                If EsReservacion Then
                                    .BodegaOrigen = p_rowCotizacion.BodegaReservas
                                Else
                                    .BodegaOrigen = p_rowCotizacion.BodegaProceso
                                End If
                                .BodegaDestino = p_rowCotizacion.BodegaRepuesto
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                '.BodegaOrigen = p_rowCotizacion.BodegaProceso
                                If EsReservacion Then
                                    .BodegaOrigen = p_rowCotizacion.BodegaReservas
                                Else
                                    .BodegaOrigen = p_rowCotizacion.BodegaProceso
                                End If
                                .BodegaDestino = p_rowCotizacion.BodegaSuministro
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        .TipoRequisicion = My.Resources.Resource.Devolucion
                        .RequisicionDevolucion = True
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadRecibida = 0
                        If m_oCompany.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                    Case TipoRequisicion.DevolucionReserva
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                If EsReservacion Then
                                    .BodegaOrigen = p_rowCotizacion.BodegaReservas
                                Else
                                    .BodegaOrigen = p_rowCotizacion.BodegaProceso
                                End If
                                .BodegaDestino = p_rowCotizacion.BodegaRepuesto
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                '.BodegaOrigen = p_rowCotizacion.BodegaProceso
                                If EsReservacion Then
                                    .BodegaOrigen = p_rowCotizacion.BodegaReservas
                                Else
                                    .BodegaOrigen = p_rowCotizacion.BodegaProceso
                                End If
                                .BodegaDestino = p_rowCotizacion.BodegaSuministro
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        .TipoRequisicion = My.Resources.Resource.DevolucionReserva
                        .RequisicionDevolucion = True
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteDevolucion
                        .CantidadRecibida = 0
                        If m_oCompany.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                    Case TipoRequisicion.Reserva
                        Select Case p_rowCotizacion.TipoArticulo
                            Case TipoArticulo.Repuesto
                                .BodegaOrigen = p_rowCotizacion.BodegaRepuesto
                                .BodegaDestino = p_rowCotizacion.BodegaReservas
                                .DescripcionTipoArticulo = My.Resources.Resource.Repuesto
                            Case TipoArticulo.Suministro
                                .BodegaOrigen = p_rowCotizacion.BodegaSuministro
                                .BodegaDestino = p_rowCotizacion.BodegaReservas
                                .DescripcionTipoArticulo = My.Resources.Resource.Suministro
                        End Select
                        .TipoRequisicion = My.Resources.Resource.RequisicionReserva
                        .RequisicionDevolucion = False
                        .CantidadOriginal = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadSolicitada = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadPendiente = p_rowCotizacion.CantidadPendienteBodega
                        .CantidadRecibida = 0
                        If m_oCompany.Version >= 900000 Then
                            .UbicacionDestino = p_rowCotizacion.UbicacionDestino
                            .UbicacionOrigen = p_rowCotizacion.UbicacionOrigen
                        End If
                End Select
            End With
            p_oRequisicionDataLineas.Add(oRequisicionData)
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ActualizarCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal EsTransferenciaAutomatica As Boolean) As Boolean
        Dim MensajeError = String.Empty
        Dim CodigoError As Integer = 0
        Try
            If EsTransferenciaAutomatica Then
                SCG.Requisiciones.TransferenciasDirectas.AjustarPendientesRequisicion(p_oCotizacion, False, CodigoError, MensajeError)
                If CodigoError <> 0 Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(MensajeError, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            If p_oCotizacion.Update() <> 0 Then
                MensajeError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                If Not String.IsNullOrEmpty(MensajeError) Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(MensajeError, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                End If
                Return False
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function CancelarCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents) As Boolean
        Try
            If p_oCotizacion.Cancel() <> 0 Then
                Return False
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function CrearRequisicion(ByRef p_oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData), ByVal EsTransferenciaAutomatica As Boolean) As Boolean
        Try
            Dim oControladorRequisicion As ControladorRequisicion = New ControladorRequisicion(m_oCompany, SBO_Application)
            If Not p_oListaRequisicionGeneralData Is Nothing Then
                If p_oListaRequisicionGeneralData.Count > 0 Then
                    Return oControladorRequisicion.CrearRequisicionGeneralData(p_oListaRequisicionGeneralData, EsTransferenciaAutomatica)
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Carga el formulario de Asignacion Multiple
    ''' </summary>
    Public Sub CargarFormularioAsignacionMultiple(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal p_DocStatus As String, ByVal p_NoOT As String, ByVal p_IdSucursal As String)
        Dim strPath As String

        Try

            If p_DocStatus <> "C" Then
                'Variable Global
                NoOT = p_NoOT
                IdSucursal = p_IdSucursal
                oGestorFormularios = New GestorFormularios(SBO_Application)
                oFormAsignacionMultiple = New AsignacionMultiple(m_oCompany, SBO_Application)
                oFormAsignacionMultiple.FormType = g_strAsignacionMultiple
                oFormAsignacionMultiple.Titulo = My.Resources.Resource.TituloAsigancionMultiple
                strPath = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormAsignacionMultiple
                oFormAsignacionMultiple.NombreXml = strPath
                oFormAsignacionMultiple.IDSucursal = p_IdSucursal
                oFormAsignacionMultiple.FormularioSBO = oGestorFormularios.CargaFormulario(oFormAsignacionMultiple)
                'oFormAsignacionMultiple.CargaMecanicosAsignados(pVal.FormUID)
                oFormAsignacionMultiple.CargaMecanicosAsignados(pVal.FormUID, p_IdSucursal, p_NoOT)
            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ERR_SalesOferClosed, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                BubbleEvent = False
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub BotonAsignacionMultiple(ByVal oFormCot As SAPbouiCOM.Form, ByVal UsaTallerSap As Boolean, ByVal p_srtNumOT As String, ByVal p_Cotizacion As SAPbobsCOM.Documents)

        Dim query As String = String.Empty
        Dim queryNF As String
        Dim strIdSucursales As String
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim OT As SAPbobsCOM.GeneralData
        Dim m_dtConsutla As DataTable
        Dim m_childs As SAPbobsCOM.GeneralDataCollection = Nothing
        Dim m_childdata As SAPbobsCOM.GeneralData = Nothing
        Dim dtQuery As DataTable
        Dim filters As String = String.Empty
        Dim rowAdded As String
        Dim strHora, strMinutos As String
        Dim intDuracion As Integer
        Dim intSalario As Integer
        Dim strConsultaCosto As String = String.Empty
        Dim QuerySalario As String = String.Empty
        Dim DecSalario As Integer

        If oFormCot.DataSources.DataTables.Item("MecanicosAsignados").Rows.Count > 0 Then
            Dim numeroCot = p_Cotizacion.DocEntry
            m_dtConsutla = oFormCot.DataSources.DataTables.Item("dtConsulta")

            strConsultaCosto = " Select U_SCGD_sALXHORA From OHEM  Where empID = "
            dtMecAsignados = oFormCot.DataSources.DataTables.Item("MecanicosAsignados")
            strIdSucursales = p_Cotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

            If Utilitarios.ValidaExisteDataTable(oFormCot, "LocalDt") Then
                dtQuery = oFormCot.DataSources.DataTables.Item("LocalDt")
            Else
                dtQuery = oFormCot.DataSources.DataTables.Add("LocalDt")
            End If

            If Not String.IsNullOrEmpty(strIdSucursales) Then
                queryNF = DMS_Connector.Queries.GetStrQueryFormat("strQueryAsignacionesOTInterna")
                Dim queryComp As String = String.Format(queryNF, p_srtNumOT, strIdSucursales)
                dtQuery.ExecuteQuery(queryComp)

                For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                    For line As Integer = 0 To p_Cotizacion.Lines.Count - 1
                        p_Cotizacion.Lines.SetCurrentLine(line)

                        If dtMecAsignados.GetValue("col_CodAct", i) = p_Cotizacion.Lines.UserFields.Fields.Item("ItemCode").Value AndAlso dtMecAsignados.GetValue("col_LineNum", i) = p_Cotizacion.Lines.LineNum.ToString().Trim() Then
                            intDuracion = CType(p_Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value.ToString().Trim(), Integer)
                            QuerySalario = strConsultaCosto + dtMecAsignados.GetValue(1, i).ToString
                            m_dtConsutla.ExecuteQuery(QuerySalario)
                            intSalario = m_dtConsutla.GetValue(0, 0)
                            DecSalario = ((intSalario / 60) * intDuracion)
                            dtMecAsignados.SetValue("col_IdRepXOrd", i, p_Cotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim())
                            dtMecAsignados.SetValue("col_PrecioSt", i, DecSalario)
                            Exit For
                        End If
                        dtMecAsignados.SetValue("col_NoOrden", i, p_srtNumOT)
                    Next
                Next

                Try
                    oCompanyService = m_oCompany.GetCompanyService()
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParams.SetProperty("Code", p_srtNumOT)
                    OT = oGeneralService.GetByParams(oGeneralParams)
                    m_childs = OT.Child("SCGD_CTRLCOL")

                    For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                        rowAdded = dtMecAsignados.GetValue("col_Added", i).ToString().Trim()
                        If rowAdded = "N" Then
                            For y As Integer = 0 To dtQuery.Rows.Count - 1
                                Dim strLn = dtMecAsignados.GetValue("col_LineNum", i).ToString().Trim()
                                Dim lineNum = 0
                                Integer.TryParse(strLn, lineNum)
                                If dtMecAsignados.GetValue("col_CodAct", i).ToString().Trim() = dtQuery.GetValue("ItemCode", y).ToString().Trim() AndAlso _
                                    (strLn).ToString() = dtQuery.GetValue("LineNum", y).ToString().Trim() Then
                                    dtMecAsignados.SetValue("col_IdRepXOrd", i, dtQuery.GetValue("IDRepXOrd", y).ToString().Trim())
                                    dtMecAsignados.SetValue("col_NoFase", i, dtQuery.GetValue("NoFase", y).ToString().Trim())
                                    dtMecAsignados.SetValue("col_Estado", i, dtQuery.GetValue("Estado", y).ToString().Trim())

                                    Exit For
                                End If
                            Next
                            m_childdata = m_childs.Add()

                            'If String.IsNullOrEmpty(dtMecAsignados.GetValue("col_Estado", i)) Then
                            '    m_childdata.SetProperty("U_Estad", "1")
                            'Else
                            '    m_childdata.SetProperty("U_Estad", dtMecAsignados.GetValue("col_Estado", i))
                            'End If
                            m_childdata.SetProperty("U_Estad", "1")
                            m_childdata.SetProperty("U_IdAct", dtMecAsignados.GetValue("col_IdRepXOrd", i))
                            m_childdata.SetProperty("U_NoFas", dtMecAsignados.GetValue("col_DesNoFase", i))
                            m_childdata.SetProperty("U_Colab", dtMecAsignados.GetValue("col_CodEmp", i))
                            m_childdata.SetProperty("U_TMin", 0)
                            m_childdata.SetProperty("U_CosRe", 0)
                            m_childdata.SetProperty("U_CosEst", dtMecAsignados.GetValue("col_PrecioSt", i))
                            m_childdata.SetProperty("U_CodFas", dtMecAsignados.GetValue("col_NoFase", i))
                            strHora = DateTime.Now.Hour.ToString()
                            If strHora.Length = 1 Then strHora = String.Format("0{0}", strHora)
                            strMinutos = DateTime.Now.Minute.ToString()
                            If (strMinutos.Length = 1) Then strMinutos = String.Format("0{0}", strMinutos)
                            strHora = strHora + ":" + strMinutos
                            m_childdata.SetProperty("U_FechPro", DateTime.Now)
                            m_childdata.SetProperty("U_HoraIni", strHora)

                        End If
                    Next

                    For i As Integer = 0 To dtMecAsignados.Rows.Count - 1
                        DecSalario = CType(dtMecAsignados.GetValue("col_PrecioSt", i).ToString().Trim(), Decimal)
                        ActulizaLineasCot(dtMecAsignados.GetValue("col_IdRepXOrd", i).ToString().Trim(), dtMecAsignados.GetValue("col_CodEmp", i).ToString().Trim(), dtMecAsignados.GetValue("col_NomEmp", i).ToString().Trim(), DecSalario, UsaTallerSap, p_Cotizacion)
                    Next

                    If Not m_oCompany.InTransaction Then
                        m_oCompany.StartTransaction()
                    End If

                    If p_Cotizacion.Update() = 0 Then
                        oGeneralService.Update(OT)
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        End If
                        dtMecAsignados.Rows.Clear()
                    Else
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                    End If
                Catch ex As Exception
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                End Try
            End If
        End If
    End Sub

    Public Function ActulizaLineasCot(ByVal strIdAct As String, ByVal strIdMecanico As String, ByVal strNombreMecanico As String, ByVal intCostoSTD As Integer, ByVal usaTallerSAP As Boolean, ByRef m_objCotizacion As SAPbobsCOM.Documents) As Boolean


        Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
        Dim m_strValorId As String
        Dim idRep As String

        oLineasCotizacion = m_objCotizacion.Lines
        If usaTallerSAP Then
            idRep = "U_SCGD_ID"
        Else
            idRep = "U_SCGD_IdRepxOrd"
        End If
        For i As Integer = 0 To oLineasCotizacion.Count - 1

            oLineasCotizacion.SetCurrentLine(i)
            m_strValorId = oLineasCotizacion.UserFields.Fields.Item(idRep).Value.ToString.Trim()

            If (strIdAct = m_strValorId) Then
                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = strIdMecanico
                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = strNombreMecanico 'cboColabora.Especifico.ValidValues.Item(strIdMecanico).Description.Trim()
                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Costo").Value = intCostoSTD
                Exit For
            End If
        Next

        Return True

    End Function

    Public Sub ManejaRequisicion(ByRef p_oRequisicionDataList As RequisicionData_List, _
                                 ByRef p_oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData))
        Try
            '******Data Contract *************
            Dim oControladorRequisicion As ControladorRequisicion
            Dim oRequisicionData As RequisicionData
            Dim oRequisicionDataList As RequisicionData_List
            Dim blnProcesar As Boolean = False

            If p_oRequisicionDataList.Count > 0 Then
                p_oListaRequisicionGeneralData = New List(Of SAPbobsCOM.GeneralData)
                oControladorRequisicion = New ControladorRequisicion(m_oCompany, SBO_Application)
                For Each rowRequisicion1 As RequisicionData In p_oRequisicionDataList
                    oRequisicionDataList = New RequisicionData_List()
                    For Each rowRequisicion2 As RequisicionData In p_oRequisicionDataList
                        If Not rowRequisicion2.Aplicado Then
                            If rowRequisicion1.TipoArticulo = rowRequisicion2.TipoArticulo And rowRequisicion1.TipoRequisicion = rowRequisicion2.TipoRequisicion Then
                                oRequisicionData = New RequisicionData()
                                rowRequisicion2.Aplicado = True
                                oRequisicionData = rowRequisicion2
                                oRequisicionDataList.Add(oRequisicionData)
                                blnProcesar = True
                            End If
                        End If
                    Next
                    If blnProcesar Then
                        oControladorRequisicion.CrearRequisicion(oRequisicionDataList, p_oListaRequisicionGeneralData, oRequisicionDataList.Item(0).SerieCita, oRequisicionDataList.Item(0).NumeroCita)
                        blnProcesar = False
                    End If
                Next
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Public Function CreaOrdenTrabajo(ByRef p_oCotizacionActual As oDocumento, _
                                     ByRef oControlColaboradorList As ControlColaborador_List) As Boolean
        Try
            '*************Controller ***************
            Dim oControladorOrdeTrabajo As ControladorOrdenTrabajo = New ControladorOrdenTrabajo(m_oCompany, SBO_Application)
            Return oControladorOrdeTrabajo.CrearOrdenTrabajo(p_oCotizacionActual, oControlColaboradorList)
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function ValidaArticulo(ByRef p_rowCotizacion As oLineasDocumento, _
                                   ByRef p_oArticulo As SAPbobsCOM.IItems, _
                                   ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List, _
                                   ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List, _
                                   ByRef p_blnMensajeCCOT As Boolean, ByVal EsReservacion As Boolean, ByVal SerieCita As String, ByVal NumeroCita As String) As Boolean
        Try
            '*******Variables ******
            Dim intTipoArticulo As Integer = 0
            Dim strCentroCosto As String = String.Empty
            Dim strFaseProduccion As String = String.Empty

            If Not String.IsNullOrEmpty(p_oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value) Then
                intTipoArticulo = CInt(p_oArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value)
            End If
            '********************************
            'Se valida según tipo de articulo
            '*******************************
            Select Case intTipoArticulo
                Case TipoArticulo.Repuesto
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                            strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                            p_blnMensajeCCOT = True
                        Else
                            strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                        End If
                        If Not String.IsNullOrEmpty(strCentroCosto) Then
                            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                If row.CentroCosto = strCentroCosto Then
                                    p_rowCotizacion.BodegaRepuesto = row.BodegaRepuesto
                                    p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                    p_rowCotizacion.CentroCosto = row.CentroCosto
                                    p_rowCotizacion.BodegaOrigen = row.BodegaRepuesto
                                    p_rowCotizacion.BodegaDestino = row.BodegaProceso
                                    p_rowCotizacion.BodegaReservas = row.BodegaReservas
                                    p_rowCotizacion.TipoArticulo = TipoArticulo.Repuesto
                                    p_rowCotizacion.Procesar = True
                                    Return True
                                End If
                            Next
                        End If
                    End If
                    Return False
                Case TipoArticulo.Servicio
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        strFaseProduccion = p_oArticulo.UserFields.Fields.Item(mc_strFase).Value.ToString.Trim()
                        If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                            strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                            p_blnMensajeCCOT = True
                        Else
                            strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                        End If
                        If Not String.IsNullOrEmpty(strCentroCosto) And Not String.IsNullOrEmpty(strFaseProduccion) Then
                            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                If row.CentroCosto = strCentroCosto Then
                                    p_rowCotizacion.BodegaServicio = row.BodegaServicio
                                    p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                    p_rowCotizacion.CentroCosto = row.CentroCosto
                                    If p_rowCotizacion.DuracionEstandar = 0 Then
                                        p_rowCotizacion.DuracionEstandar = p_oArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value
                                    End If
                                    p_rowCotizacion.TipoArticulo = TipoArticulo.Servicio
                                    p_rowCotizacion.Procesar = True
                                    Return True
                                End If
                            Next
                        End If
                    End If
                    Return False
                Case TipoArticulo.ServicioExterno
                    If p_oConfiguracionSucursal.Item(0).UsaServiciosExternosInventariables = True Then
                        If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                            If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                                strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                                p_blnMensajeCCOT = True
                            Else
                                strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                            End If
                            If Not String.IsNullOrEmpty(strCentroCosto) Then
                                For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                    If row.CentroCosto = strCentroCosto Then
                                        p_rowCotizacion.BodegaServicioExterno = row.BodegaServicioExterno
                                        p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                        p_rowCotizacion.CentroCosto = row.CentroCosto
                                        p_rowCotizacion.TipoArticulo = TipoArticulo.ServicioExterno
                                        p_rowCotizacion.Procesar = True
                                        Return True
                                    End If
                                Next
                            End If
                        End If
                        Return False
                    Else
                        If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                            If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                                strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                                p_blnMensajeCCOT = True
                            Else
                                strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                            End If
                            If Not String.IsNullOrEmpty(strCentroCosto) Then
                                For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                    If row.CentroCosto = strCentroCosto Then
                                        p_rowCotizacion.BodegaServicioExterno = row.BodegaServicioExterno
                                        p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                        p_rowCotizacion.CentroCosto = row.CentroCosto
                                        p_rowCotizacion.TipoArticulo = TipoArticulo.ServicioExterno
                                        p_rowCotizacion.Procesar = True
                                        Return True
                                    End If
                                Next
                            End If
                        End If
                        Return False
                    End If
                Case TipoArticulo.Suministro
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT) Then
                            strCentroCosto = p_oConfiguracionSucursal.Item(0).CentroCostoTipoOT
                            p_blnMensajeCCOT = True
                        Else
                            strCentroCosto = p_oArticulo.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString.Trim
                        End If
                        If Not String.IsNullOrEmpty(strCentroCosto) Then
                            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                                If row.CentroCosto = strCentroCosto Then
                                    p_rowCotizacion.BodegaSuministro = row.BodegaSuministro
                                    p_rowCotizacion.BodegaProceso = row.BodegaProceso
                                    p_rowCotizacion.CentroCosto = row.CentroCosto
                                    p_rowCotizacion.BodegaOrigen = row.BodegaSuministro
                                    p_rowCotizacion.BodegaDestino = row.BodegaProceso
                                    p_rowCotizacion.BodegaReservas = row.BodegaReservas
                                    p_rowCotizacion.TipoArticulo = TipoArticulo.Suministro
                                    p_rowCotizacion.Procesar = True
                                    Return True
                                End If
                            Next
                        End If
                    End If
                    Return False
                Case TipoArticulo.Paquete
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_rowCotizacion.NoOrden)
                        Else
                            If EsReservacion AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                                p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, SerieCita, NumeroCita)
                            End If
                        End If
                        p_rowCotizacion.Procesar = False
                        Return True
                    End If
                    Return False
                Case TipoArticulo.OtrosCostos
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                        And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                        And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_rowCotizacion.NoOrden)
                        Else
                            If EsReservacion AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                                p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, SerieCita, NumeroCita)
                            End If
                        End If
                        p_rowCotizacion.TipoArticulo = TipoArticulo.OtrosCostos
                        p_rowCotizacion.Procesar = False
                        Return True
                    End If
                    Return False
                Case TipoArticulo.OtrosIngresos
                    If p_oArticulo.InventoryItem = SAPbobsCOM.BoYesNoEnum.tNO _
                       And p_oArticulo.PurchaseItem = SAPbobsCOM.BoYesNoEnum.tYES _
                       And p_oArticulo.SalesItem = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_rowCotizacion.NoOrden)
                        Else
                            If EsReservacion AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                                p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, SerieCita, NumeroCita)
                            End If
                        End If
                        p_rowCotizacion.TipoArticulo = TipoArticulo.OtrosIngresos
                        p_rowCotizacion.Procesar = False
                        Return True
                    End If
                    Return False
                Case TipoArticulo.Otros
                    If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                        p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_rowCotizacion.NoOrden)
                    Else
                        If EsReservacion AndAlso Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, SerieCita, NumeroCita)
                        End If
                    End If
                    p_rowCotizacion.TipoArticulo = TipoArticulo.Otros
                    p_rowCotizacion.Procesar = False
                    Return True
                Case TipoArticulo.ArticuloCita
                    p_rowCotizacion.NoOrden = String.Empty
                    p_rowCotizacion.TipoArticulo = TipoArticulo.ArticuloCita
                    p_rowCotizacion.Procesar = False
                    Return True
                Case Else
                    Return False
            End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Sub DatosLineasCotizacion(ByRef p_rowCotizacion As oLineasDocumento, _
                                     ByRef p_oCotizacionActual As oDocumento, ByVal EsReservacion As Boolean)
        Try
            If String.IsNullOrEmpty(p_rowCotizacion.NoOrden) Then
                p_rowCotizacion.NoOrden = p_oCotizacionActual.NoOrden
            End If
            '********************************
            'Se valida según tipo de articulo
            '*******************************
            If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                Select Case CInt(p_rowCotizacion.TipoArticulo)
                    Case TipoArticulo.Repuesto
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoOrden)
                        End If
                        If EsReservacion Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoSerieCita, p_oCotizacionActual.NoCita)
                        End If
                    Case TipoArticulo.Servicio
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoOrden)
                        End If
                        If EsReservacion Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoSerieCita, p_oCotizacionActual.NoCita)
                        End If
                        If String.IsNullOrEmpty(p_rowCotizacion.EstadoActividad) Then
                            p_rowCotizacion.EstadoActividad = "1"
                        End If
                    Case TipoArticulo.ServicioExterno
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoOrden)
                        End If
                        If EsReservacion Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoSerieCita, p_oCotizacionActual.NoCita)
                        End If
                        p_rowCotizacion.Comprar = "Y"
                    Case TipoArticulo.Suministro
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoOrden)
                        End If
                        If EsReservacion Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoSerieCita, p_oCotizacionActual.NoCita)
                        End If
                    Case TipoArticulo.Paquete
                        If String.IsNullOrEmpty(p_rowCotizacion.ID) AndAlso Not String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoOrden)
                        End If
                        If EsReservacion Then
                            p_rowCotizacion.ID = String.Format("{0}-{1}-{2}-{3}", p_rowCotizacion.Sucursal, p_rowCotizacion.LineNum, p_oCotizacionActual.NoSerieCita, p_oCotizacionActual.NoCita)
                        End If
                End Select
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ValidaDisponibilidadArticulo(ByRef p_rowCotizacion As oLineasDocumento, _
                                            ByRef p_oArticulo As SAPbobsCOM.IItems, _
                                            ByRef p_oConfiguracionSucursal As ConfiguracionSucursal_List)
        Try
            '********Variables *****************
            Dim dblStockDisponible As Double = 0
            Dim intTipoMovimiento As Integer = 0
            '********************************
            'Se valida stock disponible
            '*******************************
            Select Case CInt(p_rowCotizacion.TipoArticulo)
                Case TipoArticulo.Repuesto
                    dblStockDisponible = ArticuloEnStock(p_oArticulo, p_rowCotizacion.BodegaRepuesto)
                    p_rowCotizacion.CantidadStock = dblStockDisponible
                    If dblStockDisponible < p_rowCotizacion.Quantity Then
                        intTipoMovimiento = SBO_Application.MessageBox(My.Resources.Resource.El_Item & p_rowCotizacion.ItemCode & " " & p_rowCotizacion.Description & My.Resources.Resource.SinInventario, 1, My.Resources.Resource.Comprar, My.Resources.Resource.Rechazar, My.Resources.Resource.Trasladar)
                        If intTipoMovimiento > 0 Then
                            Select Case intTipoMovimiento
                                Case 1
                                    p_rowCotizacion.TipoMovimiento = TipoMovimiento.Comprar
                                Case 2
                                    p_rowCotizacion.TipoMovimiento = TipoMovimiento.Rechazar
                                Case 3
                                    p_rowCotizacion.TipoMovimiento = TipoMovimiento.Trasladar
                            End Select
                        End If
                    Else
                        If p_oConfiguracionSucursal.Item(0).UsaRequisiciones = True Then
                            p_rowCotizacion.TipoMovimiento = TipoMovimiento.Requisicion
                        End If
                    End If
                Case TipoArticulo.Suministro
                    dblStockDisponible = ArticuloEnStock(p_oArticulo, p_rowCotizacion.BodegaSuministro)
                    p_rowCotizacion.CantidadStock = dblStockDisponible
                    If dblStockDisponible < p_rowCotizacion.Quantity Then
                        p_rowCotizacion.TipoMovimiento = TipoMovimiento.Trasladar
                    Else
                        If p_oConfiguracionSucursal.Item(0).UsaRequisiciones = True Then
                            p_rowCotizacion.TipoMovimiento = TipoMovimiento.Requisicion
                        End If
                    End If
            End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ArticuloEnStock(ByRef p_oArticulo As SAPbobsCOM.IItems, ByRef p_strBodegaOrigen As String) As Double
        Try
            Dim oItemWhsInfo As SAPbobsCOM.IItemWarehouseInfo
            Dim dblStock As Double = 0
            Dim contador As Integer

            oItemWhsInfo = p_oArticulo.WhsInfo

            For contador = 0 To oItemWhsInfo.Count - 1
                With oItemWhsInfo
                    .SetCurrentLine(contador)
                    If .WarehouseCode = p_strBodegaOrigen Then
                        dblStock = .InStock - .Committed
                        Exit For
                    End If
                End With
            Next
            Return dblStock
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Function CargaConfiguracionSucursal(ByVal p_oCotizacionActual As oDocumento, _
                                               ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List,
                                               ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List) As Boolean
        Try
            '*********Data Contract ************
            Dim oConfiguracionSucursal As ConfiguracionSucursal = New ConfiguracionSucursal()
            '*********Variables ************
            Dim strCentroCostoPorTipoOT As String = String.Empty
            '*********Objetos System ************
            Dim oDataTableConfiguracionSucursal As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionSucursal As System.Data.DataRow
            'Obtiene la configuración por sucursal OT
            oDataTableConfiguracionSucursal = Utilitarios.ObtenerConsultaConfiguracionPorSucursal(p_oCotizacionActual.IDSucursal, m_oCompany)
            For Each oDataRowConfiguracionSucursal In oDataTableConfiguracionSucursal.Rows
                With oConfiguracionSucursal
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_UsaOfeVenta")) Then
                        If oDataRowConfiguracionSucursal.Item("U_UsaOfeVenta") = "Y" Then
                            .UsaOfertaCompra = True
                            .UsaOrdenCompra = False
                        Else
                            .UsaOfertaCompra = False
                            .UsaOrdenCompra = True
                        End If
                    ElseIf Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_UsaOrdVenta")) Then
                        If oDataRowConfiguracionSucursal.Item("U_UsaOrdVenta") = "Y" Then
                            .UsaOfertaCompra = False
                            .UsaOrdenCompra = True
                        Else
                            .UsaOfertaCompra = True
                            .UsaOrdenCompra = False
                        End If
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Requis")) Then
                        If oDataRowConfiguracionSucursal.Item("U_Requis") = "Y" Then
                            .UsaRequisiciones = True
                        Else
                            .UsaRequisiciones = False
                        End If
                    Else
                        .UsaRequisiciones = False
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_AsigAutCol")) Then
                        'Verifico el valor para RealizarAsignacionAutomaticaColaborador
                        If oDataRowConfiguracionSucursal.Item("U_AsigAutCol") = "Y" Then
                            .AsignacionAutomaticaColaborador = True
                        Else
                            .AsignacionAutomaticaColaborador = False
                        End If
                    Else
                        .AsignacionAutomaticaColaborador = False
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SEInvent")) Then
                        'Verifico el valor para Servicios Externos Inventariables
                        If oDataRowConfiguracionSucursal.Item("U_SEInvent") = "Y" Then
                            .UsaServiciosExternosInventariables = True
                        Else
                            .UsaServiciosExternosInventariables = False
                        End If
                    Else
                        .UsaServiciosExternosInventariables = False
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                        'Verifico el valor para SerieNumeracionTransferencia
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                            .SerieNumeracionTrasnferencia = oDataRowConfiguracionSucursal.Item("U_SerInv")
                        End If
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CopiasOT")) Then
                        'Verifico el valor para Numero de copias
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_CopiasOT")) Then
                            .CantidadCopiasOT = oDataRowConfiguracionSucursal.Item("U_CopiasOT")
                        End If

                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodRep")) Then
                        'Verifico el valor para Bodega Repuesto
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodRep")) Then
                            .BodegaRepuesto = oDataRowConfiguracionSucursal.Item("U_NoBodRep")
                        End If

                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodPro")) Then
                        'Verifico el valor para Bodega Proceso
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodPro")) Then
                            .BodegaProceso = oDataRowConfiguracionSucursal.Item("U_NoBodPro")
                        End If

                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodSE")) Then
                        'Verifico el valor para Bodega Servicios Externos
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodSE")) Then
                            .BodegaServicioExterno = oDataRowConfiguracionSucursal.Item("U_NoBodSE")
                        End If
                    End If

                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_NoBodSum")) Then
                        'Verifico el valor para Bodega Suministros
                        If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_NoBodSum")) Then
                            .BodegaSuministro = oDataRowConfiguracionSucursal.Item("U_NoBodSum")
                        End If
                    End If
                End With
                '*************************************
                'Carga Centro de Costo por OT
                '*************************************
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucursal) confSucursal.U_Sucurs = p_oCotizacionActual.IDSucursal) Then
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(confSucursal) confSucursal.U_Sucurs = p_oCotizacionActual.IDSucursal).Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code = p_oCotizacionActual.TipoOT) Then
                        strCentroCostoPorTipoOT = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(confSucursal) confSucursal.U_Sucurs = p_oCotizacionActual.IDSucursal).Configuracion_Tipo_Orden.First(Function(tipoOT) tipoOT.U_Code = p_oCotizacionActual.TipoOT).U_CodCtCos.Trim()
                    End If
                End If
                If Not String.IsNullOrEmpty(strCentroCostoPorTipoOT) Then
                    oConfiguracionSucursal.CentroCostoTipoOT = strCentroCostoPorTipoOT.Trim()
                End If
                '*************************************
                'Usa Ubicaciones
                '*************************************
                If m_oCompany.Version > 900000 Then
                    If DMS_Connector.Configuracion.ParamGenAddon.U_UsaUbicD = "Y" Then
                        oConfiguracionSucursal.UsaUbicaciones = True
                    End If
                End If
                '*************************************************************
                'Valida si el usuario puede disminuir la cantidad de los items
                '*************************************************************

                If DMS_Connector.Configuracion.ParamGenAddon.U_ReduceCant.ToUpper.Equals("Y") Then
                    oConfiguracionSucursal.UsuarioDisminuye = ValidaUsuarioDisminuye()
                Else
                    oConfiguracionSucursal.UsuarioDisminuye = True
                End If

                'oConfiguracionSucursal.UsuarioDisminuye = ValidaUsuarioDisminuye()

                p_oConfiguracionSucursalList.Add(oConfiguracionSucursal)
                '****************************
                'Carga Bodega Centro Costo
                '****************************
                Utilitarios.ObtenerAlmacenXCentroCosto(p_oCotizacionActual.IDSucursal, p_oBodegaCentroCostoList)
                Return True
            Next
            Return False
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Sub ObtenerCodeUDOOrdenTrabajo(ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, _
                                          ByRef p_strCode As String)
        Try
            If Not String.IsNullOrEmpty(p_oCotizacionEncabezadoList.Item(0).NoOrden) Then
                p_strCode = Utilitarios.EjecutarConsulta(String.Format("select code from [@SCGD_OT] with (nolock) where U_NoOT='{0}'", _
                                                                                   p_oCotizacionEncabezadoList.Item(0).NoOrden), m_oCompany.CompanyDB, m_oCompany.Server)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ValidaUsuarioDisminuye() As Boolean
        Try
            Return DMS_Connector.Helpers.PermisosMenu("SCGD_RED")
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function AsignaNumeracionVisita(ByRef p_oCotizacionActual As oDocumento, _
                                           ByRef p_oCotizacion As SAPbobsCOM.Documents) As String
        Dim blnMensajeError As Boolean = False
        Try
            '**************Variables *****************
            Dim intNumVisita As Integer = 0
            Dim strNumVisita As String = String.Empty
            Utilitarios.ResetTransaction(m_oCompany, SBO_Application)
            Utilitarios.StartTransaction(m_oCompany, SBO_Application)
            intNumVisita = Utilitarios.ObtieneNumeracionPorSucursalObjeto(p_oCotizacionActual.Sucursal, "SCGD_OT", m_oCompany)
            If Not String.IsNullOrEmpty(intNumVisita.ToString()) Then
                If intNumVisita > 0 Then
                    strNumVisita = intNumVisita.ToString().Trim()
                    '***Valida si existe un mismo número de visita asociado a otra unidad
                    If Not ValidaVisitaAsociada(strNumVisita, p_oCotizacionActual.CodigoUnidad) Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaVisita, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Utilitarios.RollbackTransaction(m_oCompany, SBO_Application)
                        Return String.Empty
                    Else
                        p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = strNumVisita
                        p_oCotizacion.Update()
                        Utilitarios.CommitTransaction(m_oCompany, SBO_Application)
                    End If
                Else
                    Utilitarios.RollbackTransaction(m_oCompany, SBO_Application)
                    Return String.Empty
                End If
            End If
            Return strNumVisita
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Utilitarios.RollbackTransaction(m_oCompany, SBO_Application)
            Return String.Empty
        End Try
    End Function

    Public Function TipoProcesamientoCotizacion(ByRef p_oCotizacionActual As oDocumento, ByVal EsReservacion As Boolean) As Integer
        Try
            Dim intIDEstadoCotizacion As Integer = 0
            With p_oCotizacionActual
                If .GeneraOT = GeneraOT.SI And String.IsNullOrEmpty(.NoOrden) Then
                    Return TipoProcesamiento.Crear
                ElseIf Not String.IsNullOrEmpty(.NoOrden) Then
                    If Not String.IsNullOrEmpty(.EstadoCotizacionID) Then
                        intIDEstadoCotizacion = CInt(.EstadoCotizacionID)
                        If intIDEstadoCotizacion <= 3 Then
                            Return TipoProcesamiento.Actualizar
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.CambiosNoAplicadosOT, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                        End If
                    End If
                End If
            End With

            If EsReservacion AndAlso String.IsNullOrEmpty(p_oCotizacionActual.NoOrden) AndAlso p_oCotizacionActual.GeneraOT = GeneraOT.NO Then
                Return TipoProcesamiento.Actualizar
            End If

            Return 0
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return 0
        End Try
    End Function

    Public Function ValidarCargaCotizacionInicial(ByRef p_oForm As SAPbouiCOM.Form) As Boolean
        Try
            Dim intDocEntry As Integer = 0
            'oCotizacionInicial = New Cotizacion() 
            oCotizacionInicial = New oDocumento
            If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).ToString()) Then
                intDocEntry = p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0)
                oCotizacionInicial = CargarCotizacionInicial(intDocEntry)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function



    Public Function ValidaInformacionCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents) As Boolean
        '*******************Objetos SAP **********************
        Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
        Try
            '********Variables ***************
            Dim strUsaLead As String
            Dim intGeneraOT As Integer = 0
            Dim strIDSucursal As String = String.Empty
            With p_oCotizacion
                If .Cancelled = SAPbobsCOM.BoYesNoEnum.tNO And .DocumentStatus <> SAPbobsCOM.BoStatus.bost_Close Then

                    If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString()) Then
                        intGeneraOT = CInt(.UserFields.Fields.Item("U_SCGD_Genera_OT").Value)

                        Select Case intGeneraOT
                            Case GeneraOT.SI
                                '********Valida Sucursal **************
                                If String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeSucursalTaller, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Return False
                                End If
                                '********Valida tipo OT **************
                                If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
                                    If .UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = 0 Then
                                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinTipo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                        Return False
                                    End If
                                Else
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinTipo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Return False
                                End If
                                '********Valida Asesor **************
                                If .DocumentsOwner <= 0 Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinAsesor, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Return False
                                End If
                                '********Valida Codigo Unidad **************
                                If String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.CotizacionSinVehiculo, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    Return False
                                End If
                                '********Valida Typo Socio negocios ***************************
                                oBusinessPartner = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners),  _
                                         SAPbobsCOM.BusinessPartners)
                                If oBusinessPartner.GetByKey(.CardCode) Then
                                    If oBusinessPartner.CardType = SAPbobsCOM.BoCardTypes.cLid Then
                                        If DMS_Connector.Configuracion.ParamGenAddon.U_UsaLed = "N" Then
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTipoCliente, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            Return False
                                        End If
                                    End If
                                End If
                                Return True
                            Case GeneraOT.NO
                                Return False
                        End Select
                    End If
                End If
            End With
            Return False
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            Utilitarios.DestruirObjeto(oBusinessPartner)
        End Try
    End Function

    Private Sub ImprimirRecepcion()
        Try
            Dim objReporte As New ComponenteCristalReport.SubReportView
            Dim strDireccionReporte As String = String.Empty
            Dim intCopias As Integer = 0
            Dim strCopias As String = String.Empty

            Utilitarios.DevuelveDireccionReportes(SBO_Application, strDireccionReporte)
            strCopias = Utilitarios.EjecutarConsulta("select U_OT_SAP from [@SCGD_ADMIN] with(nolock) ", m_oCompany.CompanyDB, m_oCompany.Server)
            If Not String.IsNullOrEmpty(strCopias) Then
                intCopias = CInt(strCopias)
            End If

            If Not String.IsNullOrEmpty(strDireccionReporte) Then
                If intCopias > 0 Then
                    objReporte.P_BarraTitulo = My.Resources.Resource.OrdenRecepcion
                    objReporte.P_CompanyName = m_oCompany.CompanyName
                    objReporte.P_DataBase = m_oCompany.CompanyDB
                    objReporte.P_Filename = My.Resources.Resource.rptOrdenRecepcion & ".rpt"
                    ' objReporte.P_ParArray = m_strNoOrden
                    objReporte.P_ParArray = ""
                    objReporte.P_Password = m_oCompany.DbPassword
                    objReporte.P_Server = m_oCompany.Server
                    objReporte.P_User = m_oCompany.DbUserName
                    objReporte.P_WorkFolder = strDireccionReporte

                    For i As Integer = 1 To intCopias
                        objReporte.PrintReporte(False)
                    Next
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub InicializarTimer()
        Try
            'Inicializa un timer que se ejecuta cada 30 segundos
            'y llama al método LimpiarColaMensajes
            oTimer = New System.Timers.Timer(30000)
            RemoveHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            AddHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            oTimer.AutoReset = True
            oTimer.Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub DetenerTimer()
        Try
            oTimer.Stop()
            oTimer.Dispose()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LimpiarColaMensajes()
        Try
            'En las operaciones muy largas, la cola de mensajes se llena ocasionando que el add-on se desconecte y genere errores como
            'RPC Server call o similares. Para solucionarlo se debe ejecutar este método cada cierto tiempo (30 o 60 segundos) para limpiar
            'la cola de mensajes
            DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#Region "Solicitud OT Especial"
    ''' <summary>
    ''' Solicitud de OT Especial
    ''' </summary>
    ''' 
    Private Sub ValidaSolicitaOTEspecial(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim strPath As String
        Dim oForm As SAPbouiCOM.Form

        Try
            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            Dim strDocEntry As String
            Dim docStatus As String
            Dim strIDSucursal As String

            strDocEntry = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("DocEntry", 0).Trim
            docStatus = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("DocStatus", 0).Trim
            strIDSucursal = oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).ToString().Trim

            If docStatus = "O" Then
                oGestorFormularios = New GestorFormularios(SBO_Application)
                oFormSolOTEspecial = New SolicitaOTEspecial(m_oCompany, SBO_Application)
                If Not String.IsNullOrEmpty(strIDSucursal) Then
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal)) Then
                        With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal))
                            'If .U_HjaCanPen = "N" Then
                            If ValidaEstadoPendienteLineasCotizacion(strDocEntry, .U_HjaCanPen) Then
                                oFormSolOTEspecial.FormType = g_strFormSolicitaOTEsp
                                oFormSolOTEspecial.Titulo = My.Resources.Resource.TituloSolicitaOTEspecial
                                DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
                                strPath = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormSolicitaOTEsp
                                oFormSolOTEspecial.NombreXml = strPath
                                oFormSolOTEspecial.FormularioSBO = oGestorFormularios.CargaFormulario(oFormSolOTEspecial)
                            Else
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeNoCreaOTEspecialesPendienteTraslado, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                BubbleEvent = False
                            End If

                        End With
                    End If
                End If
            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ERR_SalesOferClosed, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
#End Region


#Region "Validaciones"
    Public Function ValidarKM_HorasServico(ByRef p_oForm As SAPbouiCOM.Form) As Boolean
        Try
            '***** Declaración de variables *****
            Dim strIDSucursal As String = String.Empty
            Dim intCodeVehiculo As Integer = 0
            Dim dblKMCotizacion As Double = 0
            Dim dblKMMaestroVehiculo As Double = 0
            Dim dblHSCotizacion As Double = 0
            Dim dblHSMaestroVehiculo As Double = 0
            n = DIHelper.GetNumberFormatInfo(m_oCompany)
            If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).ToString()) Then
                If p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Genera_OT", 0) = GeneraOT.SI Then
                    strIDSucursal = p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).ToString().Trim()
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal)) Then
                        With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal))
                            '****** Valida el Kilometraje ******
                            If .U_ValKm = "Y" Then
                                If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Num_Vehiculo", 0)) Then
                                    intCodeVehiculo = p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Num_Vehiculo", 0)
                                    If intCodeVehiculo > 0 Then
                                        dblKMMaestroVehiculo = DMS_Connector.Helpers.EjecutarConsultaDouble(String.Format("SELECT ""U_Km_Unid"" FROM ""@SCGD_VEHICULO"" WHERE ""Code"" = '{0}'", intCodeVehiculo))
                                        dblKMCotizacion = Convert.ToDouble(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Kilometraje", 0), n)
                                        If dblKMCotizacion < dblKMMaestroVehiculo Then
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorValidacionKilometraje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            Return False
                                        End If
                                    End If
                                End If
                            End If
                            '****** Valida las horas servicio *********
                            If .U_ValHS = "Y" Then
                                If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Num_Vehiculo", 0)) Then
                                    intCodeVehiculo = p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Num_Vehiculo", 0)
                                    If intCodeVehiculo > 0 Then
                                        dblHSMaestroVehiculo = DMS_Connector.Helpers.EjecutarConsultaDouble(String.Format("SELECT ""U_HorSer"" FROM ""@SCGD_VEHICULO"" WHERE ""Code"" = '{0}'", intCodeVehiculo))
                                        dblHSCotizacion = p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_HoSr", 0)
                                        If dblHSCotizacion < dblHSMaestroVehiculo Then
                                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorValidacionHorasServicio, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            Return False
                                        End If
                                    End If
                                End If
                            End If
                        End With
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Function InterfazFord_Validaciones(ByRef p_oForm As SAPbouiCOM.Form) As Boolean
        Try
            '*****Objetos SAP *****
            Dim oComboTipoPago As SAPbouiCOM.ComboBox
            Dim oComboDptoServ As SAPbouiCOM.ComboBox
            '*****Valida si esta definido el Tipo del Socio de Negocios *****
            If Not Utilitarios.ValidaIFTipoSN(m_oCompany, p_oForm.DataSources.DBDataSources.Item("OQUT").GetValue("CardCode", 0)) Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoSN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return False
            End If
            '*****Valida Tipo de pago y departaento de servicios *****
            oComboTipoPago = p_oForm.Items.Item(mc_strCboTipoPago).Specific
            oComboDptoServ = p_oForm.Items.Item(mc_strCboDptoSrv).Specific
            If String.IsNullOrEmpty(oComboDptoServ.Value) Or String.IsNullOrEmpty(oComboTipoPago.Value) Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoPagoDptoServ, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Return False
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Function ValidaEstadoPendienteLineasCotizacion(ByRef p_strDocEntry As String, ByVal p_strCreaHjaCanPend As String) As Boolean
        '*****Objetos SAP *****
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            If Not String.IsNullOrEmpty(p_strCreaHjaCanPend) AndAlso p_strCreaHjaCanPend.Equals("N") Then
                oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                If oCotizacion.GetByKey(p_strDocEntry) Then
                    For rowCotizacion As Integer = 0 To oCotizacion.Lines.Count - 1
                        oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                        If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = ArticuloAprobado.scgSi And _
                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = TipoArticulo.Repuesto And _
                             oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value <> Trasladado.SI And _
                             oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "N" Then
                            Return False
                        End If
                    Next
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Utilitarios.DestruirObjeto(oCotizacion)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
        End Try
    End Function
#End Region
#End Region
End Class
