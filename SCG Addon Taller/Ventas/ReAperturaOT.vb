Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Globalization
Imports SCG.SBOFramework
Imports System.Xml
Imports System.IO
Imports System.Collections.Generic
Imports SCG.Cifrado
Imports System.Reflection
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports System.Data.DataTable
Imports System.Linq
Imports DMS_Connector.Business_Logic.DataContract.SAPDocumento

Module ReAperturaOT
    Private WithEvents oApplication As SAPbouiCOM.Application
    Private oCompany As SAPbobsCOM.Company
    Private oFormulario As SAPbouiCOM.Form
    Private n As NumberFormatInfo
    Private oForm As SAPbouiCOM.Form
    Private oTimer As System.Timers.Timer
    Private formID As String = "SCGD_REAOT"

    Private m_strOT As String = "@SCGD_OT"

    Public EditTextNoOT As EditTextSBO
    Public EditTextCliente As EditTextSBO
    Public EditTextClienteOT As EditTextSBO
    Public EditTextPlaca As EditTextSBO
    Public EditTextNoUnidad As EditTextSBO
    Public EditTextVIN As EditTextSBO

    Public EditTextDEst As EditTextSBO
    Public EditTextNCli As EditTextSBO
    Public EditTextNCliOT As EditTextSBO
    Public EditTextMar As EditTextSBO
    Public EditTextEst As EditTextSBO
    Public EditTextMode As EditTextSBO

    Public EditTextDECot As EditTextSBO
    Public EditTextSucu As EditTextSBO
    Public EditTextTipoOT As EditTextSBO
    Public EditTextAse As EditTextSBO

    Public EditTextNCot As EditTextSBO
    Public EditTextNOT As EditTextSBO
    Public EditTextEstOT As EditTextSBO

    Private strIDEstado As String = String.Empty
    '*****Objetos SAP *****
    Private oCotizacion As SAPbobsCOM.Documents
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Try
            oApplication = DMS_Connector.Company.ApplicationSBO
            oCompany = DMS_Connector.Company.CompanySBO
            'oForm = oApplication.Forms.Item("SCGD_IJD")
            n = DIHelper.GetNumberFormatInfo(oCompany)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#Region "Eventos"
    Public Sub AbrirFormulario()
        Dim oFormCreationParams As FormCreationParams
        Dim Path As String = String.Empty
        'Dim oForm As SAPbouiCOM.Form
        Dim oMatrix As Matrix

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.BorderStyle = BoFormBorderStyle.fbs_Sizable
            oFormCreationParams.FormType = "SCGD_REAOT"

            Path = My.Resources.Resource.XMLReAperturaOT
            oFormCreationParams.XmlData = CargarDesdeXML(Path)

            oForm = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)

            InicializarControles()

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub SBO_Application_ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Handles oApplication.ItemEvent
        Dim strTipoCarga As String
        Try
            If pVal.FormTypeEx = formID Then
                If pVal.EventType <> BoEventTypes.et_FORM_UNLOAD Then
                    If pVal.Before_Action Then
                        Select Case pVal.EventType
                            Case BoEventTypes.et_ITEM_PRESSED

                            Case BoEventTypes.et_CHOOSE_FROM_LIST
                                ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
                        End Select
                    Else
                        Select Case pVal.EventType
                            Case BoEventTypes.et_ITEM_PRESSED
                                Select Case pVal.ItemUID
                                    Case "btnOK"

                                    Case "btnReA"
                                        ManejaReAperturaOT()
                                        HabilitaCampos(False)
                                    Case "btnRef"
                                        HabilitaCampos(True)
                                    Case "linkOT"
                                End Select
                            Case BoEventTypes.et_CHOOSE_FROM_LIST
                                ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
                            Case BoEventTypes.et_LOST_FOCUS
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region
#Region "Metodos"
    Public Sub ManejaReAperturaOT()
        Try
            InicializarTimer()
            oForm.Freeze(True)
            Select Case strIDEstado
                Case "4"
                    ProcesaReAperturaOTFinalizada()
                Case "6"
                    ProcesaReAperturaOTCerrada()
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            oForm.Freeze(False)
            DetenerTimer()
        End Try
    End Sub

    Public Sub ProcesaReAperturaOTFinalizada()
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceOT As SAPbobsCOM.GeneralService
        Dim oGeneralDataOT As SAPbobsCOM.GeneralData
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Dim strDocEntry As String = String.Empty
        Dim strNoOrden As String = String.Empty
        Dim strError As String
        Dim intError As Integer
        Try
            oApplication.StatusBar.SetText(My.Resources.Resource.InicioReAperturaOT, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            oitem = oForm.Items.Item("txtDECot")
            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            strDocEntry = oEditText.String.ToString()
            oitem = oForm.Items.Item("txtNoOT")
            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            strNoOrden = oEditText.String.ToString()

            If Not String.IsNullOrEmpty(strNoOrden) Then
                oCotizacion = CType(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                    SAPbobsCOM.Documents)
                If oCotizacion.GetByKey(CInt(strDocEntry)) Then
                    ActualizarCotizacionReAperturaFinalizada(oCotizacion)
                    ActualizarOT(oCompanyService, oGeneralServiceOT, oGeneralDataOT, strNoOrden, oCompany.GetNewObjectKey)

                    If Not oCompany.InTransaction() Then
                        oCompany.StartTransaction()
                        'Actualiza Cotización
                        If oCotizacion.Update() = 0 Then
                            If Not oGeneralServiceOT Is Nothing Then
                                oGeneralServiceOT.Update(oGeneralDataOT)
                                If oCompany.InTransaction() Then
                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                    AsignaValoresResultado(strDocEntry, strNoOrden)
                                    oApplication.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                End If
                            Else
                                oCompany.GetLastError(intError, strError)
                                If oCompany.InTransaction() Then
                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                End If
                                oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarOT + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            End If
                        Else
                            oCompany.GetLastError(intError, strError)
                            If oCompany.InTransaction() Then
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                            oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarOT + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            If oCompany.InTransaction() Then
                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ProcesaReAperturaOTCerrada()
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Dim oCotizacionActual As SAPbobsCOM.Documents
        Dim oCotizacionNueva As SAPbobsCOM.Documents
        Dim oOrdenDeVenta As SAPbobsCOM.Documents
        Dim oDocumentoCotizacion As oDocumento
        Dim strDocEntry As String = String.Empty
        Dim strDocEntryOrdenVenta As String = String.Empty
        Try
            oApplication.StatusBar.SetText(My.Resources.Resource.InicioReAperturaOT, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            oitem = oForm.Items.Item("txtDECot")
            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            strDocEntry = oEditText.String.ToString()
            If Not String.IsNullOrEmpty(strDocEntry) Then
                oOrdenDeVenta = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                oDocumentoCotizacion = New oDocumento()
                oDocumentoCotizacion = CargarCotizacionActual(CInt(strDocEntry))
                If Not oDocumentoCotizacion Is Nothing Then
                    If AsignarValoresNuevaCotizacion(oCotizacionNueva, oDocumentoCotizacion) Then
                        strDocEntryOrdenVenta = ConsultaDocEntryOrdenVenta(oDocumentoCotizacion.NoOrden)
                        LimpiarDocumento(oCotizacion)
                        If Not String.IsNullOrEmpty(strDocEntryOrdenVenta) Then
                            If oOrdenDeVenta.GetByKey(CInt(strDocEntryOrdenVenta)) Then
                                LimpiarDocumento(oOrdenDeVenta)
                            End If
                        End If
                        GuardarDatosDB(oCotizacion, oCotizacionNueva, oOrdenDeVenta, oDocumentoCotizacion, oDocumentoCotizacion.NoOrden)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ActualizarCotizacionReAperturaFinalizada(ByRef p_oDocument As SAPbobsCOM.Documents)
        Try
            With p_oDocument
                .UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenEnproceso
                .UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "2"
            End With
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ActualizarOT(ByRef p_oCompanyService As SAPbobsCOM.CompanyService, ByRef p_oGeneralServiceOT As GeneralService, ByRef p_oGeneralDataOT As GeneralData, ByRef strNoOT As String, ByVal p_strDocEntry As String)
        Dim oGeneralParams As GeneralDataParams
        Try
            p_oCompanyService = oCompany.GetCompanyService()
            p_oGeneralServiceOT = p_oCompanyService.GetGeneralService("SCGD_OT")
            If Not String.IsNullOrEmpty(strNoOT) Then
                oGeneralParams = p_oGeneralServiceOT.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", strNoOT)
                p_oGeneralDataOT = p_oGeneralServiceOT.GetByParams(oGeneralParams)
                p_oGeneralDataOT.SetProperty("U_EstO", "2")
                p_oGeneralDataOT.SetProperty("U_DEstO", My.Resources.Resource.EstadoOrdenEnproceso)
                p_oGeneralDataOT.SetProperty("U_DocEntry", p_strDocEntry)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oGeneralParams)
        End Try
    End Sub
    Private Function GuardarDatosDB(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oCotizacionNueva As SAPbobsCOM.Documents, ByRef p_oPedido As SAPbobsCOM.Documents, ByRef p_oDocumento As oDocumento, ByRef strNoOT As String) As Boolean
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceOT As SAPbobsCOM.GeneralService
        Dim oGeneralDataOT As SAPbobsCOM.GeneralData
        Dim strError As String
        Dim intError As Integer
        Dim strDocEntry As String = String.Empty
        Try
            'Inicio de Transaction
            If Not oCompany.InTransaction() Then
                oCompany.StartTransaction()
                'Actualiza Cotización
                If p_oCotizacion.Update() = 0 Then
                    'Crea Nueva Cotización
                    If p_oCotizacionNueva.Add() = 0 Then
                        oCompany.GetNewObjectCode(strDocEntry)
                        If p_oCotizacionNueva.GetByKey(strDocEntry) Then
                            AsignarValoresLineasPaquete(p_oCotizacionNueva, p_oDocumento)
                            If p_oCotizacionNueva.Update() = 0 Then
                                'Actualiza Orden de Venta
                                ActualizarOT(oCompanyService, oGeneralServiceOT, oGeneralDataOT, strNoOT, oCompany.GetNewObjectKey)
                                If Not p_oPedido Is Nothing Then
                                    If p_oPedido.Update() = 0 Then
                                        If p_oPedido.Cancel() = 0 Then
                                            'Actualiza la OT
                                            If Not oGeneralServiceOT Is Nothing Then
                                                oGeneralServiceOT.Update(oGeneralDataOT)
                                                If oCompany.InTransaction() Then
                                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                                    AsignaValoresResultado(strDocEntry, strNoOT)
                                                    oApplication.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                                Else
                                                    oCompany.GetLastError(intError, strError)
                                                    If oCompany.InTransaction() Then
                                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                                    End If
                                                    oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarOT + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                                    Return False
                                                End If
                                            Else
                                                oCompany.GetLastError(intError, strError)
                                                If oCompany.InTransaction() Then
                                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                                End If
                                                oApplication.StatusBar.SetText(My.Resources.Resource.ErrorCancelarOrdenVenta + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                                Return False
                                            End If
                                        Else
                                            oCompany.GetLastError(intError, strError)
                                            If oCompany.InTransaction() Then
                                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                            End If
                                            oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarOrdenVenta + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                            Return False
                                        End If
                                    Else
                                        oCompany.GetLastError(intError, strError)
                                        If oCompany.InTransaction() Then
                                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                        End If
                                        Return False
                                    End If
                                Else
                                    oCompany.GetLastError(intError, strError)
                                    If oCompany.InTransaction() Then
                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                    End If
                                    oApplication.StatusBar.SetText(My.Resources.Resource.ErrorReAperturaCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                    Return False
                                End If
                            Else
                                oCompany.GetLastError(intError, strError)
                                If oCompany.InTransaction() Then
                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                End If
                                oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                Return False
                            End If
                        Else
                            oCompany.GetLastError(intError, strError)
                            If oCompany.InTransaction() Then
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                            oApplication.StatusBar.SetText(My.Resources.Resource.ErrorReAperturaCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            Return False
                        End If
                    Else
                        oCompany.GetLastError(intError, strError)
                        If oCompany.InTransaction() Then
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                        oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                Else
                    oCompany.GetLastError(intError, strError)
                    If oCompany.InTransaction() Then
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If
        Catch ex As Exception
            If oCompany.InTransaction() Then
                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Function CargarCotizacionActual(ByVal p_intDocEntry As Integer) As oDocumento
        '*****Objetos SAP *****
        'Dim oCotizacion As SAPbobsCOM.Documents
        Try
            '*****DataContract *****
            Dim oDocumento As oDocumento
            Dim oLineasDocumento As List(Of oLineasDocumento)
            If p_intDocEntry > 0 Then
                oCotizacion = CType(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                          SAPbobsCOM.Documents)
                If oCotizacion.GetByKey(p_intDocEntry) Then
                    oDocumento = New oDocumento()
                    oDocumento.DocEntry = oCotizacion.DocEntry
                    oDocumento.CardCode = oCotizacion.CardCode
                    oDocumento.CardName = oCotizacion.CardName
                    oDocumento.DocCurrency = oCotizacion.DocCurrency
                    oDocumento.Serie = oCotizacion.Series
                    oDocumento.Comments = oCotizacion.Comments
                    oDocumento.SlpCode = oCotizacion.SalesPersonCode
                    oDocumento.DiscountPercent = oCotizacion.DiscountPercent
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                        oDocumento.NoOrden = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                        oDocumento.Sucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString()) Then
                        oDocumento.GeneraOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value) Then
                        oDocumento.EstadoCotizacionID = "4"
                    End If
                    If oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value <> Nothing Then
                        oDocumento.FechaCreacionOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value
                    End If
                    If oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value <> Nothing Then
                        oDocumento.HoraCreacionOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value) Then
                        oDocumento.GeneraRecepcion = oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value) Then
                        oDocumento.OTPadre = oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value) Then
                        oDocumento.NoOTReferencia = oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value) Then
                        oDocumento.NumeroVIN = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                        oDocumento.CodigoUnidad = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.DocumentsOwner.ToString()) Then
                        oDocumento.CodigoAsesor = oCotizacion.DocumentsOwner
                    Else
                        oDocumento.CodigoAsesor = 0
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
                        oDocumento.TipoOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                    Else
                        oDocumento.TipoOT = 0
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value) Then
                        oDocumento.CodigoProyecto = oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value) Then
                        oDocumento.NoVisita = oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value) Then
                        oDocumento.NoSerieCita = oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value) Then
                        oDocumento.NoCita = oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value) Then
                        oDocumento.Cono = oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()) Then
                        oDocumento.Year = oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()) Then
                        oDocumento.DescripcionMarca = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()) Then
                        oDocumento.DescripcionModelo = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()) Then
                        oDocumento.DescripcionEstilo = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()) Then
                        oDocumento.CodigoMarca = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()) Then
                        oDocumento.CodigoEstilo = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()) Then
                        oDocumento.CodigoModelo = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString.Trim()) Then
                        oDocumento.Kilometraje = oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString.Trim()) Then
                        oDocumento.Placa = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString.Trim()) Then
                        oDocumento.NombreClienteOT = oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString().Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString.Trim()) Then
                        oDocumento.CodigoClienteOT = oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString().Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value.ToString.Trim()) Then
                        oDocumento.FechaRecepcion = oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value.ToString.Trim()) Then
                        oDocumento.HoraRecepcion = oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value.ToString()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value.ToString.Trim()) Then
                        oDocumento.NivelGasolina = oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value) Then
                        oDocumento.Observaciones = oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value.ToString.Trim()
                    End If
                    If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value) Then
                        oDocumento.EstadoCotizacion = My.Resources.Resource.EstadoOTFinalizada
                    End If
                    oLineasDocumento = New List(Of oLineasDocumento)()
                    For rowCotizacion As Integer = 0 To oCotizacion.Lines.Count - 1
                        oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                        With oLineasDocumento
                            .Add(New oLineasDocumento())
                            With .Item(rowCotizacion)
                                .DocEntry = oCotizacion.Lines.DocEntry
                                .ItemCode = oCotizacion.Lines.ItemCode
                                .Description = oCotizacion.Lines.ItemDescription
                                .Quantity = oCotizacion.Lines.Quantity
                                .TreeType = oCotizacion.Lines.TreeType
                                .Price = oCotizacion.Lines.Price
                                .TaxCode = oCotizacion.Lines.TaxCode
                                .VatGroup = oCotizacion.Lines.VatGroup
                                .FreeText = oCotizacion.Lines.FreeText
                                .Currency = oCotizacion.Lines.Currency

                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    .ID = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                .Aprobado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                .Trasladado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value) Then
                                    .OTHija = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Then
                                    .DuracionEstandar = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                Else
                                    .DuracionEstandar = 0
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()) Then
                                    .NombreEmpleado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                    .EstadoActividad = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value.ToString.Trim()) Then
                                    .CantidadRecibida = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString.Trim()) Then
                                    .CantidadSolicitada = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString.Trim()) Then
                                    .CantidadPendiente = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value.ToString.Trim()) Then
                                    .CantidadPendienteBodega = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value.ToString.Trim()) Then
                                    .CantidadPendienteTraslado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value.ToString.Trim()) Then
                                    .CantidadPendienteDevolucion = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()) Then
                                    .Costo = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()) Then
                                    .NoOrden = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value.ToString.Trim()) Then
                                    .Entregado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()) Then
                                    .TipoArticulo = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value.ToString.Trim()) Then
                                    .Comprar = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim()) Then
                                    .Sucursal = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()) Then
                                    .CentroCosto = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()) Then
                                    .TipoOT = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value.ToString.Trim()) Then
                                    .ProcesarInteger = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                    .EstadoActividad = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                'If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()) Then
                                '    .NombreEmpleado = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()
                                'End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()) Then
                                    .CostoEstandar = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value.ToString.Trim()) Then
                                    .PaquetePadre = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value.ToString.Trim()
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
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Nothing
        End Try
    End Function

    Private Function AsignarValoresNuevaCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oDocumento As oDocumento) As Boolean
        Try
            p_oCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            'Encabezado de la Cotizacion
            With p_oDocumento
                p_oCotizacion.CardCode = .CardCode
                p_oCotizacion.CardName = .CardName
                p_oCotizacion.DocCurrency = .DocCurrency
                p_oCotizacion.Series = .Serie
                p_oCotizacion.Comments = .Comments
                p_oCotizacion.SalesPersonCode = .SlpCode
                p_oCotizacion.DiscountPercent = .DiscountPercent
                If Not String.IsNullOrEmpty(.NoOrden) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .NoOrden
                End If
                If Not String.IsNullOrEmpty(.Sucursal) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = .Sucursal
                End If
                If Not String.IsNullOrEmpty(.GeneraOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = .GeneraOT
                End If
                If Not String.IsNullOrEmpty(.EstadoCotizacionID) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "2"
                End If
                If .FechaCreacionOT <> Nothing Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = .FechaCreacionOT
                End If
                If .HoraCreacionOT <> Nothing Then
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
                If Not String.IsNullOrEmpty(.NumeroVIN) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = .NumeroVIN
                End If
                If Not String.IsNullOrEmpty(.CodigoUnidad) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = .CodigoUnidad
                End If
                If Not String.IsNullOrEmpty(.CodigoAsesor) Then
                    p_oCotizacion.DocumentsOwner = .CodigoAsesor
                End If
                If Not String.IsNullOrEmpty(.TipoOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = .TipoOT
                End If
                If Not String.IsNullOrEmpty(.CodigoProyecto) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value = .CodigoProyecto
                End If
                If Not String.IsNullOrEmpty(.NoVisita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = .NoVisita
                End If
                If Not String.IsNullOrEmpty(.NoSerieCita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = .NoSerieCita
                End If
                If Not String.IsNullOrEmpty(.NoCita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = .NoCita
                End If
                If Not String.IsNullOrEmpty(.Cono) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value = .Cono
                End If
                If Not String.IsNullOrEmpty(.Year) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = .Year
                End If
                If Not String.IsNullOrEmpty(.DescripcionMarca) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = .DescripcionMarca
                End If
                If Not String.IsNullOrEmpty(.DescripcionModelo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = .DescripcionModelo
                End If
                If Not String.IsNullOrEmpty(.DescripcionEstilo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = .DescripcionEstilo
                End If
                If Not String.IsNullOrEmpty(.CodigoMarca) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = .CodigoMarca
                End If
                If Not String.IsNullOrEmpty(.CodigoEstilo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = .CodigoEstilo
                End If
                If Not String.IsNullOrEmpty(.CodigoModelo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = .CodigoModelo
                End If
                If Not String.IsNullOrEmpty(.Kilometraje) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = .Kilometraje
                End If
                If Not String.IsNullOrEmpty(.Placa) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = .Placa
                End If
                If Not String.IsNullOrEmpty(.NombreClienteOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value = .NombreClienteOT
                End If
                If Not String.IsNullOrEmpty(.CodigoClienteOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value = .CodigoClienteOT
                End If
                If Not String.IsNullOrEmpty(.FechaRecepcion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value = .FechaRecepcion
                End If
                If Not String.IsNullOrEmpty(.HoraRecepcion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value = .HoraRecepcion
                End If
                If Not String.IsNullOrEmpty(.NivelGasolina) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value = .NivelGasolina
                End If
                If Not String.IsNullOrEmpty(.Observaciones) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value = .Observaciones
                End If
                If Not String.IsNullOrEmpty(.EstadoCotizacion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenEnproceso
                End If
            End With

            For Each row As oLineasDocumento In p_oDocumento.Lineas
                With row
                    If row.TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree Or row.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                        p_oCotizacion.Lines.ItemCode = .ItemCode
                        p_oCotizacion.Lines.ItemDescription = .Description
                        p_oCotizacion.Lines.Quantity = .Quantity
                        p_oCotizacion.Lines.UnitPrice = .Price
                        p_oCotizacion.Lines.TaxCode = .TaxCode
                        p_oCotizacion.Lines.VatGroup = .VatGroup
                        p_oCotizacion.Lines.FreeText = .FreeText
                        p_oCotizacion.Lines.Currency = .Currency
                        p_oCotizacion.Lines.DiscountPercent = .LineDscPrcnt
                        If Not String.IsNullOrEmpty(.ID) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = .ID
                        End If
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = .Aprobado
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = .Trasladado
                        If Not String.IsNullOrEmpty(.OTHija) Then
                            If .OTHija <> 0 Then
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = .OTHija
                            Else
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 2
                            End If
                        End If
                        If Not String.IsNullOrEmpty(.DuracionEstandar) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = .DuracionEstandar
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
                        If Not String.IsNullOrEmpty(.CantidadRecibida) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = .CantidadRecibida
                        End If
                        If Not String.IsNullOrEmpty(.CantidadSolicitada) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = .CantidadSolicitada
                        End If
                        If Not String.IsNullOrEmpty(.CantidadPendiente) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = .CantidadPendiente
                        End If
                        If Not String.IsNullOrEmpty(.CantidadPendienteBodega) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = .CantidadPendienteBodega
                        End If
                        If Not String.IsNullOrEmpty(.CantidadPendienteTraslado) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = .CantidadPendienteTraslado
                        End If
                        If Not String.IsNullOrEmpty(.CantidadPendienteDevolucion) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = .CantidadPendienteDevolucion
                        End If
                        If Not String.IsNullOrEmpty(.Costo) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .Costo
                        End If
                        If Not String.IsNullOrEmpty(.NoOrden) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = .NoOrden
                        End If
                        If Not String.IsNullOrEmpty(.Entregado) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value = .Entregado
                        End If
                        If Not .TipoArticulo Is Nothing Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = CStr(.TipoArticulo)
                        End If
                        If Not String.IsNullOrEmpty(.Comprar) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value = .Comprar
                        End If
                        If Not String.IsNullOrEmpty(.Sucursal) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = .Sucursal
                        End If
                        If Not String.IsNullOrEmpty(.CentroCosto) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = .CentroCosto
                        End If
                        If Not String.IsNullOrEmpty(.TipoOT) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = .TipoOT
                        End If
                        If Not String.IsNullOrEmpty(.Procesar) Then
                            If .Procesar <> 0 Then
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = .ProcesarInteger
                            Else
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = 1
                            End If
                        End If
                        If Not String.IsNullOrEmpty(.EstadoActividad) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                        End If
                        If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                        End If
                        If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                        End If
                        If Not String.IsNullOrEmpty(.CostoEstandar) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .CostoEstandar
                        End If
                        p_oCotizacion.Lines.Add()
                    End If
                End With
            Next
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Private Function AsignarValoresLineasPaquete(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oDocumento As oDocumento) As Boolean
        Try
            For rowCotizacion As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(rowCotizacion)
                If p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                    For Each row As oLineasDocumento In p_oDocumento.Lineas
                        With row
                            If p_oCotizacion.Lines.ItemCode = .ItemCode And row.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_oCotizacion.Lines.Quantity = .Quantity
                                'p_oCotizacion.Lines.UnitPrice = .Price
                                'p_oCotizacion.Lines.TaxCode = .TaxCode
                                'p_oCotizacion.Lines.VatGroup = .VatGroup
                                'p_oCotizacion.Lines.FreeText = .FreeText
                                'p_oCotizacion.Lines.Currency = .Currency
                                'p_oCotizacion.Lines.DiscountPercent = .LineDscPrcnt
                                If Not String.IsNullOrEmpty(.ID) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = .ID
                                End If
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = .Aprobado
                                p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = .Trasladado
                                If Not String.IsNullOrEmpty(.OTHija) Then
                                    If .OTHija <> 0 Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = .OTHija
                                    Else
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 2
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(.DuracionEstandar) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = .DuracionEstandar
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
                                If Not String.IsNullOrEmpty(.CantidadRecibida) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = .CantidadRecibida
                                End If
                                If Not String.IsNullOrEmpty(.CantidadSolicitada) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = .CantidadSolicitada
                                End If
                                If Not String.IsNullOrEmpty(.CantidadPendiente) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = .CantidadPendiente
                                End If
                                If Not String.IsNullOrEmpty(.CantidadPendienteBodega) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = .CantidadPendienteBodega
                                End If
                                If Not String.IsNullOrEmpty(.CantidadPendienteTraslado) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = .CantidadPendienteTraslado
                                End If
                                If Not String.IsNullOrEmpty(.CantidadPendienteDevolucion) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = .CantidadPendienteDevolucion
                                End If
                                If Not String.IsNullOrEmpty(.Costo) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .Costo
                                End If
                                If Not String.IsNullOrEmpty(.NoOrden) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = .NoOrden
                                End If
                                If Not String.IsNullOrEmpty(.Entregado) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value = .Entregado
                                End If
                                If Not String.IsNullOrEmpty(.TipoArticulo) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = CStr(.TipoArticulo)
                                End If
                                If Not String.IsNullOrEmpty(.Comprar) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value = .Comprar
                                End If
                                If Not String.IsNullOrEmpty(.Sucursal) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = .Sucursal
                                End If
                                If Not String.IsNullOrEmpty(.CentroCosto) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = .CentroCosto
                                End If
                                If Not String.IsNullOrEmpty(.TipoOT) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = .TipoOT
                                End If
                                If Not String.IsNullOrEmpty(.Procesar) Then
                                    If .Procesar <> 0 Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = .ProcesarInteger
                                    Else
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = 1
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(.EstadoActividad) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                                End If
                                If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                                End If
                                If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                                End If
                                If Not String.IsNullOrEmpty(.CostoEstandar) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .CostoEstandar
                                End If
                                If Not String.IsNullOrEmpty(.PaquetePadre) Then
                                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_PaqPadre").Value = .PaquetePadre
                                End If
                                p_oDocumento.Lineas.Remove(row)
                                Exit For
                            End If
                        End With
                    Next
                End If
            Next
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Private Sub LimpiarDocumento(ByRef p_oDocument As SAPbobsCOM.Documents)
        Try
            With p_oDocument
                .UserFields.Fields.Item("U_SCGD_NoOtRef").Value = .UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                .UserFields.Fields.Item("U_SCGD_Numero_OT").Value = ""
                .UserFields.Fields.Item("U_SCGD_OT_Padre").Value = ""
                .UserFields.Fields.Item("U_SCGD_No_Visita").Value = ""
                .UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = ""
                .UserFields.Fields.Item("U_SCGD_NoCita").Value = ""
                .UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = ""
                .UserFields.Fields.Item("U_SCGD_Num_VIN").Value = ""
                .UserFields.Fields.Item("U_SCGD_Num_Placa").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = ""
                .UserFields.Fields.Item("U_SCGD_Fech_Recep").Value = ""
                .UserFields.Fields.Item("U_SCGD_Des_Marc").Value = ""
                .UserFields.Fields.Item("U_SCGD_Des_Mode").Value = ""
                .UserFields.Fields.Item("U_SCGD_Des_Esti").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = ""
                .UserFields.Fields.Item("U_SCGD_CCliOT").Value = ""
                .UserFields.Fields.Item("U_SCGD_NCliOT").Value = ""
                .UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = ""
                For index As Integer = 0 To .Lines.Count - 1
                    .Lines.SetCurrentLine(index)
                    .Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = ""
                Next
            End With
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub InicializarControles()
        Try
            oForm.Freeze(True)

            'AddChooseFromList(oForm, "SCGD_OT", "CFL_OT")

            EditTextNoOT = New EditTextSBO("txtNoOT", True, m_strOT, "Code", oForm)
            EditTextNoOT.AsignaBinding()
            EditTextCliente = New EditTextSBO("txtCodCli", True, m_strOT, "U_CodCli", oForm)
            EditTextCliente.AsignaBinding()
            EditTextClienteOT = New EditTextSBO("txtCodCOT", True, m_strOT, "U_CodCOT", oForm)
            EditTextClienteOT.AsignaBinding()
            EditTextPlaca = New EditTextSBO("txtPla", True, m_strOT, "U_Plac", oForm)
            EditTextPlaca.AsignaBinding()
            EditTextNoUnidad = New EditTextSBO("txtNoUni", True, m_strOT, "U_NoUni", oForm)
            EditTextNoUnidad.AsignaBinding()
            EditTextVIN = New EditTextSBO("txtVIN", True, m_strOT, "U_VIN", oForm)
            EditTextVIN.AsignaBinding()

            EditTextDEst = New EditTextSBO("txtDEst", True, m_strOT, "U_DEstO", oForm)
            EditTextDEst.AsignaBinding()
            EditTextNCli = New EditTextSBO("txtNCli", True, m_strOT, "U_NCli", oForm)
            EditTextNCli.AsignaBinding()
            EditTextNCliOT = New EditTextSBO("txtNCliOT", True, m_strOT, "U_NCliOT", oForm)
            EditTextNCliOT.AsignaBinding()
            EditTextMar = New EditTextSBO("txtMar", True, m_strOT, "U_Marc", oForm)
            EditTextMar.AsignaBinding()
            EditTextEst = New EditTextSBO("txtEst", True, m_strOT, "U_Esti", oForm)
            EditTextEst.AsignaBinding()
            EditTextMode = New EditTextSBO("txtMode", True, m_strOT, "U_Mode", oForm)
            EditTextMode.AsignaBinding()

            EditTextDECot = New EditTextSBO("txtDECot", True, m_strOT, "U_DocEntry", oForm)
            EditTextDECot.AsignaBinding()
            EditTextSucu = New EditTextSBO("txtSucu", True, m_strOT, "U_Sucu", oForm)
            EditTextSucu.AsignaBinding()
            EditTextTipoOT = New EditTextSBO("txtTipoOT", True, m_strOT, "U_TipOT", oForm)
            EditTextTipoOT.AsignaBinding()
            EditTextAse = New EditTextSBO("txtAse", True, m_strOT, "U_Ase", oForm)
            EditTextAse.AsignaBinding()

            EditTextNCot = New EditTextSBO("txtNCot", True, m_strOT, "U_Bahia", oForm)
            EditTextNCot.AsignaBinding()
            EditTextNOT = New EditTextSBO("txtNOT", True, m_strOT, "U_ObservDiag", oForm)
            EditTextNOT.AsignaBinding()
            EditTextEstOT = New EditTextSBO("txtEstOT", True, m_strOT, "U_ObservCierre", oForm)
            EditTextEstOT.AsignaBinding()

            AsignaCFLCampo("txtNoOT", "CFL_OT1", "Code")
            AsignaCFLCampo("txtCodCli", "CFL_OT2", "U_CodCli")
            AsignaCFLCampo("txtCodCOT", "CFL_OT3", "U_CodCOT")
            AsignaCFLCampo("txtPla", "CFL_OT4", "U_Plac")
            AsignaCFLCampo("txtNoUni", "CFL_OT5", "U_NoUni")
            AsignaCFLCampo("txtVIN", "CFL_OT6", "U_VIN")

            HabilitaCampos(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            oForm.Freeze(False)
        End Try
    End Sub

    Public Sub ManejadorEventoLinkPress(ByRef p_oFormOT As SCG.ServicioPostVenta.OrdenTrabajo)
        Dim oGestorFormularios As GestorFormularios
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Dim strNoOrden As String = String.Empty
        Try
            oitem = oForm.Items.Item("txtNoOT")
            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            strNoOrden = oEditText.String.ToString()

            If Not String.IsNullOrEmpty(strNoOrden) Then
                oGestorFormularios = New GestorFormularios(oApplication)
                If Not oGestorFormularios.FormularioAbierto(p_oFormOT, activarSiEstaAbierto:=True) Then
                    p_oFormOT.FormularioSBO = oGestorFormularios.CargaFormulario(p_oFormOT)
                    p_oFormOT.CargarOT(strNoOrden)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejadorEventosChooseFromList(ByVal formUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

            If Not oCFLEvento.BeforeAction Then
                oDataTable = oCFLEvento.SelectedObjects
                If Not oCFLEvento.SelectedObjects Is Nothing Then
                    If Not oDataTable Is Nothing Then
                        AsignaValoresCFLACampos(formUID, pVal, oDataTable)
                    End If
                End If
            ElseIf oCFLEvento.BeforeAction Then
                Select Case pVal.ItemUID
                    Case EditTextNoOT.UniqueId

                        oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case EditTextCliente.UniqueId

                        oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case EditTextClienteOT.UniqueId

                        oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case EditTextPlaca.UniqueId

                        oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case EditTextNoUnidad.UniqueId

                        oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case EditTextVIN.UniqueId

                        oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "4"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_EstO"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "6"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para cargar las formas desde el archivo XML
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        Dim oXMLDoc As XmlDocument
        Dim strPath As String

        strPath = Windows.Forms.Application.StartupPath & strFileName
        oXMLDoc = New XmlDocument()

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml
    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams

        Try
            oCFLs = oform.ChooseFromLists

            oCFLCreationParams = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID
            oCFL = oCFLs.Add(oCFLCreationParams)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaCFLCampo(ByVal p_strCampo As String, ByVal p_strCFL As String, ByVal p_Alias As String)
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText

        Try
            oitem = oForm.Items.Item(p_strCampo)
            oEditText = DirectCast(oitem.Specific, SAPbouiCOM.EditText)
            oEditText.ChooseFromListUID = p_strCFL
            oEditText.ChooseFromListAlias = p_Alias
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub HabilitaCampos(ByRef p_blnEnable As Boolean)
        Dim oitem As SAPbouiCOM.Item
        Try
            oForm.Freeze(True)
            If p_blnEnable Then
                oitem = oForm.Items.Item("btnReA")
                oitem.Enabled = True
                'oitem = oForm.Items.Item("txtNoOT")
                'oitem.Enabled = True
                'oitem = oForm.Items.Item("txtCodCli")
                'oitem.Enabled = True
                'oitem = oForm.Items.Item("txtCodCOT")
                'oitem.Enabled = True
                'oitem = oForm.Items.Item("txtPla")
                'oitem.Enabled = True
                'oitem = oForm.Items.Item("txtNoUni")
                'oitem.Enabled = True
                'oitem = oForm.Items.Item("txtVIN")
                'oitem.Enabled = True
            Else
                oitem = oForm.Items.Item("btnReA")
                oitem.Enabled = False
                'oitem = oForm.Items.Item("txtNoOT")
                'oitem.Enabled = False
                'oitem = oForm.Items.Item("txtCodCli")
                'oitem.Enabled = False
                'oitem = oForm.Items.Item("txtCodCOT")
                'oitem.Enabled = False
                'oitem = oForm.Items.Item("txtPla")
                'oitem.Enabled = False
                'oitem = oForm.Items.Item("txtNoUni")
                'oitem.Enabled = False
                'oitem = oForm.Items.Item("txtVIN")
                'oitem.Enabled = False
            End If
            oForm.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub LimpiarCamposResultado()
        Try
            oForm.Freeze(True)
            EditTextNCot.AsignaValorDataSource(String.Empty)
            EditTextNOT.AsignaValorDataSource(String.Empty)
            EditTextEstOT.AsignaValorDataSource(String.Empty)
            oForm.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaValoresResultado(ByRef p_strNCot As String, ByRef p_strNOT As String)
        Try
            oForm.Freeze(True)
            EditTextNCot.AsignaValorDataSource(p_strNCot)
            EditTextNOT.AsignaValorDataSource(p_strNOT)
            EditTextEstOT.AsignaValorDataSource(My.Resources.Resource.EstadoOrdenEnproceso)
            oForm.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaValoresCFLACampos(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Dim strValor As String = String.Empty
        Dim strCondicion As String = String.Empty
        Try
            oForm.Freeze(True)
            strIDEstado = oDataTable.GetValue("U_EstO", 0).ToString.Trim()
            EditTextNoOT.AsignaValorDataSource(oDataTable.GetValue("Code", 0))
            EditTextCliente.AsignaValorDataSource(oDataTable.GetValue("U_CodCli", 0))
            EditTextClienteOT.AsignaValorDataSource(oDataTable.GetValue("U_CodCOT", 0))
            EditTextPlaca.AsignaValorDataSource(oDataTable.GetValue("U_Plac", 0))
            EditTextNoUnidad.AsignaValorDataSource(oDataTable.GetValue("U_NoUni", 0))
            EditTextVIN.AsignaValorDataSource(oDataTable.GetValue("U_VIN", 0))

            EditTextDEst.AsignaValorDataSource(oDataTable.GetValue("U_DEstO", 0))
            EditTextNCli.AsignaValorDataSource(oDataTable.GetValue("U_NCli", 0))
            EditTextNCliOT.AsignaValorDataSource(oDataTable.GetValue("U_NCliOT", 0))
            EditTextMar.AsignaValorDataSource(oDataTable.GetValue("U_Marc", 0))
            EditTextEst.AsignaValorDataSource(oDataTable.GetValue("U_Esti", 0))
            EditTextMode.AsignaValorDataSource(oDataTable.GetValue("U_Mode", 0))

            EditTextDECot.AsignaValorDataSource(oDataTable.GetValue("U_DocEntry", 0))
            strCondicion = oDataTable.GetValue("U_Sucu", 0)
            strValor = ConsultaNombreSucursal(strCondicion)
            If Not String.IsNullOrEmpty(strValor) Then EditTextSucu.AsignaValorDataSource(strValor)
            strCondicion = oDataTable.GetValue("U_TipOT", 0)
            strValor = ConsultaNombreTipoOT(strCondicion)
            If Not String.IsNullOrEmpty(strValor) Then EditTextTipoOT.AsignaValorDataSource(strValor)
            strCondicion = oDataTable.GetValue("U_Ase", 0)
            strValor = ConsultaNombreAsesor(strCondicion)
            If Not String.IsNullOrEmpty(strValor) Then EditTextAse.AsignaValorDataSource(strValor)
            oForm.Freeze(False)
            HabilitaCampos(True)
            LimpiarCamposResultado()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function ConsultaNombreSucursal(ByRef p_strCondicion As String) As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsInformacion As DBDataSource
        Dim index As Integer
        Dim strResultado As String = String.Empty
        Try
            oForm.DataSources.DBDataSources.Add("@SCGD_SUCURSALES")
            dsInformacion = oForm.DataSources.DBDataSources.Item("@SCGD_SUCURSALES")

            oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "Code"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strCondicion
            oCondition.BracketCloseNum = 1

            dsInformacion.Query(oConditions)

            For index = 0 To dsInformacion.Size - 1
                If Not String.IsNullOrEmpty(dsInformacion.GetValue("Name", index)) Then
                    strResultado = dsInformacion.GetValue("Name", index).ToString()
                End If
            Next
            Return strResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Function ConsultaNombreTipoOT(ByRef p_strCondicion As String) As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsInformacion As DBDataSource
        Dim index As Integer
        Dim strResultado As String = String.Empty
        Try
            oForm.DataSources.DBDataSources.Add("@SCGD_TIPO_ORDEN")
            dsInformacion = oForm.DataSources.DBDataSources.Item("@SCGD_TIPO_ORDEN")

            oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "Code"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strCondicion
            oCondition.BracketCloseNum = 1

            dsInformacion.Query(oConditions)

            For index = 0 To dsInformacion.Size - 1
                If Not String.IsNullOrEmpty(dsInformacion.GetValue("Name", index)) Then
                    strResultado = dsInformacion.GetValue("Name", index).ToString()
                End If
            Next
            Return strResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Function ConsultaNombreAsesor(ByRef p_strCondicion As String) As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsInformacion As DBDataSource
        Dim index As Integer
        Dim strResultado As String = String.Empty
        Try
            oForm.DataSources.DBDataSources.Add("OHEM")
            dsInformacion = oForm.DataSources.DBDataSources.Item("OHEM")

            oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "empID"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strCondicion
            oCondition.BracketCloseNum = 1

            dsInformacion.Query(oConditions)

            For index = 0 To dsInformacion.Size - 1
                If Not String.IsNullOrEmpty(dsInformacion.GetValue("firstName", index)) Then
                    strResultado = dsInformacion.GetValue("firstName", index).ToString() + " " + dsInformacion.GetValue("lastName", index).ToString()
                End If
            Next
            Return strResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Function ConsultaDocEntryOrdenVenta(ByRef p_strCondicion As String) As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsInformacion As DBDataSource
        Dim index As Integer
        Dim strResultado As String = String.Empty
        Try
            oForm.DataSources.DBDataSources.Add("ORDR")
            dsInformacion = oForm.DataSources.DBDataSources.Item("ORDR")

            oConditions = oApplication.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "U_SCGD_Numero_OT"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strCondicion
            oCondition.BracketCloseNum = 1

            dsInformacion.Query(oConditions)

            For index = 0 To dsInformacion.Size - 1
                If Not String.IsNullOrEmpty(dsInformacion.GetValue("DocEntry", index)) Then
                    strResultado = dsInformacion.GetValue("DocEntry", index).ToString()
                End If
            Next
            Return strResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

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
#End Region
End Module
