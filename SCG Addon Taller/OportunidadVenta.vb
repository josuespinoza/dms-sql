'Agregado 27/09/2010: Clase para manejar funciones en oportunidad de ventas
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework

Public Class OportunidadVenta

    Private Const mc_strbtGeneraCV As String = "SCGD_btGCV"
    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company
    Private oDataTableSucursal As SAPbouiCOM.DataTable


    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, ByVal m_oCompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Application
        SBO_Company = m_oCompany

    End Sub

    Private _Bandera As Boolean

    Public Property Bandera As Boolean
        Get
            Return _Bandera
        End Get
        Set(ByVal value As Boolean)
            _Bandera = value
        End Set
    End Property


    Public Sub ManejoEventoLoad(ByRef pVal As SAPbouiCOM.ItemEvent)
        Dim oForm As SAPbouiCOM.Form
        Try
            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            oDataTableSucursal = oForm.DataSources.DataTables.Add("SucursalUsuario")

            AgregaBoton(oForm)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Sub ManejarEstados(ByRef oForm As SAPbouiCOM.Form)

        Dim strEstado As String
        Dim oItem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oComboEtapa As SAPbouiCOM.ComboBox
        Dim strEtapaCV As String

        Try

            strEstado = oForm.Items.Item("139").Specific.string

            oItem = oForm.Items.Item("56")
            oMatrix = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)
            strEtapaCV = Utilitarios.EjecutarConsulta("Select U_SCGD_EtapaCV from [@SCGD_ADMIN] where Code = 'DMS'", SBO_Company.CompanyDB, SBO_Company.Server)

            Dim i As Integer = oMatrix.RowCount

            oComboEtapa = DirectCast(oMatrix.Columns.Item("4").Cells.Item(i).Specific, SAPbouiCOM.ComboBox)

            If strEstado = "O" AndAlso oComboEtapa.Selected.Value = strEtapaCV Then
                oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            Else
                oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Function AgregaBoton(ByVal oForm As SAPbouiCOM.Form) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oitemCancel As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button

        Try
            oitemCancel = oForm.Items.Item("2")
            oitem = oForm.Items.Add(mc_strbtGeneraCV, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = oitemCancel.Left + 200
            oitem.Top = oitemCancel.Top
            oitem.Width = 73
            oitem.Height = oitemCancel.Height

            oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
            oitem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 9, BoModeVisualBehavior.mvb_True)

            oButton = oitem.Specific
            oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
            oButton.Caption = My.Resources.Resource.BotonCV

            Return oitem

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
        Return Nothing
    End Function

    Public Sub ManejadorEventoItemPressed(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        Dim strNumeroCotizacion As String

        Try
            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If pVal.ItemUID = mc_strbtGeneraCV AndAlso pVal.BeforeAction = True Then
                'comprobar si es cliente
                Dim strId As String

                strId = oForm.DataSources.DBDataSources.Item("OOPR").GetValue("CardCode", 0)
                Dim strEsCliente As String = Utilitarios.EjecutarConsulta("SELECT CARDTYPE FROM OCRD WHERE CardCode = '" & strId.Trim() & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                Select Case strEsCliente
                    Case "C"
                        Bandera = True
                    Case Else
                        Bandera = False
                End Select
            End If


            If pVal.ItemUID = mc_strbtGeneraCV AndAlso pVal.BeforeAction = False Then

                Dim intOportunidadVenta As Integer
                Dim strOportunidadCV As String = String.Empty

                'Obtengo el numero de Oportunidad de Ventas
                intOportunidadVenta = oForm.DataSources.DBDataSources.Item("OOPR").GetValue("OpprId", 0)

                'Select para traer el Contrato de Ventas que le pertenece a la Oportunidad de Ventas y que no este Cancelado
                If Not String.IsNullOrEmpty(intOportunidadVenta) Then
                    strOportunidadCV = Utilitarios.EjecutarConsulta(String.Format("SELECT DocNum FROM dbo.[@SCGD_CVENTA] WHERE U_cod_OV = '{0}' and U_Estado <> 0 ", intOportunidadVenta), SBO_Company.CompanyDB, SBO_Company.Server)
                Else
                    strOportunidadCV = ""
                End If

                'Si no existe la Oportunidad en el Contrato de Ventas then
                If strOportunidadCV = "" Then

                    If Bandera = True Then

                        Dim oMatrix As SAPbouiCOM.Matrix
                        Dim oItem As SAPbouiCOM.Item
                        Dim oComboTipoDoc As SAPbouiCOM.ComboBox
                        Dim oComboEtapa As SAPbouiCOM.ComboBox
                        Dim oEditTextDocumento As SAPbouiCOM.EditText
                        Dim blCotizacion As Boolean = False
                        Dim strEtapaCV As String = ""

                        oItem = oForm.Items.Item("56")

                        oMatrix = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)

                        strEtapaCV = Utilitarios.EjecutarConsulta("Select U_SCGD_EtapaCV from [@SCGD_ADMIN] where Code = 'DMS'", SBO_Company.CompanyDB, SBO_Company.Server)

                        For i As Integer = 0 To oMatrix.RowCount - 1

                            oComboTipoDoc = DirectCast(oMatrix.Columns.Item("14").Cells.Item(i + 1).Specific, SAPbouiCOM.ComboBox)
                            oComboEtapa = DirectCast(oMatrix.Columns.Item("4").Cells.Item(i + 1).Specific, SAPbouiCOM.ComboBox)

                            If oComboTipoDoc.Selected.Value = "23" AndAlso oComboEtapa.Selected.Value = strEtapaCV Then

                                oEditTextDocumento = DirectCast(oMatrix.Columns.Item("15").Cells.Item(i + 1).Specific, SAPbouiCOM.EditText)

                                If Not String.IsNullOrEmpty(oEditTextDocumento.Value) Then

                                    'strNumeroCotizacion = oEditTextDocumento.Value
                                    strNumeroCotizacion = Utilitarios.EjecutarConsulta("Select DocEntry from [OQUT] where DocNum = '" & oEditTextDocumento.Value & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                                    'Crear CV con datos de oportunidad de ventas con cotizacion
                                    Call CreaCVCotizacion(oForm, strNumeroCotizacion)

                                    blCotizacion = True
                                    Exit For

                                End If

                            End If

                        Next i

                        If blCotizacion = False Then

                            'Crear CV con datos de oportunidad de ventas sin cotizacion
                            Call CreaCVOpotunidad(oForm)
                            'if bandera
                        End If

                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorNoEsCliente, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If

                Else
                    'Mando el error donde indica que en la Oportunidad ya se encuentra ligado el Contrato de Ventas
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaOportunidadconContratoVent, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If

            End If

            If pVal.ActionSuccess AndAlso (pVal.ItemUID = "59" OrElse pVal.ItemUID = "60" OrElse pVal.ItemUID = "61") Then
                Dim optionItem As Item = oForm.Items.Item(pVal.ItemUID)
                Dim optionS As OptionBtn = optionItem.Specific

                If optionS.Selected AndAlso pVal.ItemUID = "61" Then
                    oForm.Items.Item(mc_strbtGeneraCV).Enabled = True
                Else
                    oForm.Items.Item(mc_strbtGeneraCV).Enabled = False
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub ManejoEventosCombo(ByRef oForm As SAPbouiCOM.Form, _
                                      ByVal pval As SAPbouiCOM.ItemEvent, _
                                      ByRef BubbleEvent As Boolean)

        Try

            If pval.ActionSuccess Then

                If pval.ItemUID = "56" Then
                    Dim strEstado As String
                    Dim oItem As SAPbouiCOM.Item
                    Dim oMatrix As SAPbouiCOM.Matrix
                    Dim oComboEtapa As SAPbouiCOM.ComboBox
                    Dim strEtapaCV As String
                    Dim strNumOportunidad As String
                    Dim strExisteOportunidad As String

                    strNumOportunidad = oForm.Items.Item("74").Specific.string
                    strExisteOportunidad = Utilitarios.EjecutarConsulta(String.Format("Select OpprId from OOPR where OpprId = '{0}'", strNumOportunidad), SBO_Company.CompanyDB, SBO_Company.Server)

                    If Not String.IsNullOrEmpty(strExisteOportunidad) Then

                        strEstado = oForm.Items.Item("139").Specific.string

                        oItem = oForm.Items.Item("56")
                        oMatrix = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)
                        strEtapaCV = Utilitarios.EjecutarConsulta("Select U_SCGD_EtapaCV from [@SCGD_ADMIN] where Code = 'DMS'", SBO_Company.CompanyDB, SBO_Company.Server)

                        Dim i As Integer = oMatrix.RowCount

                        oComboEtapa = DirectCast(oMatrix.Columns.Item("4").Cells.Item(i).Specific, SAPbouiCOM.ComboBox)

                        If strEstado = "O" AndAlso oComboEtapa.Selected.Value = strEtapaCV Then
                            oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                        Else
                            oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                        End If

                    End If

                End If

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub ManejoEventosMenu(ByRef oForm As SAPbouiCOM.Form, _
                                      ByVal pval As SAPbouiCOM.MenuEvent, _
                                      ByRef BubbleEvent As Boolean)

        Try
            Select Case pval.MenuUID
                Case "1293"
                    Dim strEstado As String
                    Dim oItem As SAPbouiCOM.Item
                    Dim oMatrix As SAPbouiCOM.Matrix
                    Dim oComboEtapa As SAPbouiCOM.ComboBox
                    Dim strEtapaCV As String

                    strEstado = oForm.Items.Item("139").Specific.string

                    oItem = oForm.Items.Item("56")
                    oMatrix = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)
                    strEtapaCV = Utilitarios.EjecutarConsulta("Select U_SCGD_EtapaCV from [@SCGD_ADMIN] where Code = 'DMS'", SBO_Company.CompanyDB, SBO_Company.Server)

                    Dim i As Integer = oMatrix.RowCount

                    oComboEtapa = DirectCast(oMatrix.Columns.Item("4").Cells.Item(i - 1).Specific, SAPbouiCOM.ComboBox)

                    If strEstado = "O" AndAlso oComboEtapa.Selected.Value = strEtapaCV Then
                        oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

                    Else
                        oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    End If
                Case "1282"
                    oForm.Items.Item("SCGD_btGCV").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            End Select



        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub CreaCVOpotunidad(ByVal oForm As SAPbouiCOM.Form)

        Dim strNumCV As String
        Dim strNumOportunidad As String
        Dim strNombreOportunidad As String
        Dim strCodigoSocio As String
        Dim strNombreSocio As String
        Dim strCodigoEmpleado As String
        Dim strNombreEmpleado As String
        Dim strNivelContrato As String
        Dim strFechaContrato As String
        Dim strMoneda As String

        'Erick Sanabria. Agregar campos de Sucursal al Contrato de Ventas  
        Dim strCodigoSucursal As String = ""
        Dim strNombreSucursal As String = ""
        Dim strUserCode As String = ""
        Dim strConsulta As String = ""


        Dim oCombo As SAPbouiCOM.ComboBox

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildAcc As SAPbobsCOM.GeneralData
        Dim oChildrenAcc As SAPbobsCOM.GeneralDataCollection
        Dim oChildRes As SAPbobsCOM.GeneralData
        Dim oChildrenRes As SAPbobsCOM.GeneralDataCollection
        Dim oChildSum As SAPbobsCOM.GeneralData
        Dim oChildrenSum As SAPbobsCOM.GeneralDataCollection
        Dim oChildNuevo As SAPbobsCOM.GeneralData
        Dim oChildrenNuevo As SAPbobsCOM.GeneralDataCollection
        Dim oChildUsado As SAPbobsCOM.GeneralData
        Dim oChildrenUsado As SAPbobsCOM.GeneralDataCollection
        Dim oChildTram As SAPbobsCOM.GeneralData
        Dim oChildrenTram As SAPbobsCOM.GeneralDataCollection

        Dim strCodigoVendedor As String

        Dim oRecordset As SAPbobsCOM.Recordset
        Dim oSBObob As SAPbobsCOM.SBObob

        Try

            strNumCV = Utilitarios.EjecutarConsulta("Select AutoKey from [ONNM] where ObjectCode = 'SCGD_CVT'", SBO_Company.CompanyDB, SBO_Company.Server)

            strNumOportunidad = oForm.Items.Item("74").Specific.string
            strNombreOportunidad = oForm.Items.Item("137").Specific.string
            strCodigoSocio = oForm.Items.Item("9").Specific.string
            strNombreSocio = oForm.Items.Item("11").Specific.string
            oCombo = DirectCast(oForm.Items.Item("15").Specific, ComboBox)
            strCodigoEmpleado = oCombo.Selected.Value
            strNombreEmpleado = oCombo.Selected.Description

            'Erick Sanabria. Agregar campos de Sucursal al Contrato de Ventas cuando es creado desde la oportunidad de Ventas 
            'Modificación e consulta para codigo de sucursal y nombre 12/08/2013 

            strConsulta = "Select USR.USER_CODE As CodigoUsuario,USR.Branch As CodigoSucursal,SUC.Name As NombreSucursal " &
                          "From OOPR OPR " &
                          "Inner Join OHEM OHEM On OPR.SlpCode=OHEM.salesPrson " &
                          "Inner Join OUSR USR On OHEM.userId=USR.USERID " &
                          "Inner Join [@SCGD_SUC_VENTA] SUC On USR.Branch = SUC.Code " &
                          "Where OPR.OpprId=" & strNumOportunidad & ""

            oDataTableSucursal.Clear()
            oDataTableSucursal.ExecuteQuery(strConsulta)

            strUserCode = oDataTableSucursal.GetValue("CodigoUsuario", 0)
            strCodigoSucursal = oDataTableSucursal.GetValue("CodigoSucursal", 0)
            strNombreSucursal = oDataTableSucursal.GetValue("NombreSucursal", 0)

            If ((strUserCode <> "") AndAlso (strCodigoSucursal <> "")) Then

                strNivelContrato = Utilitarios.EjecutarConsulta("SELECT MIN(U_Prio +1) FROM [@SCGD_ADMIN9]", SBO_Company.CompanyDB, SBO_Company.Server)

                strFechaContrato = DateAndTime.Now.ToString("yyyyMMdd")

                'strMoneda = Utilitarios.EjecutarConsulta("Select Currency from [OCRD] where CardCode = '" & strCodigoSocio & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                'If strMoneda = "##" Then
                oSBObob = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
                oRecordset = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                oRecordset = oSBObob.GetLocalCurrency()
                oRecordset.MoveFirst()
                If oRecordset.EoF Then
                    Throw New Exception(My.Resources.Resource.MonedaLocalNoConfig)
                Else
                    strMoneda = oRecordset.Fields.Item(0).Value
                End If
                'End If

                oCompanyService = SBO_Company.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")

                oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
                oGeneralData.SetProperty("DocNum", strNumCV)
                oGeneralData.SetProperty("U_Cod_OV", strNumOportunidad)
                oGeneralData.SetProperty("U_Name_OV", strNombreOportunidad)
                oGeneralData.SetProperty("U_CardCode", strCodigoSocio)
                oGeneralData.SetProperty("U_CardName", strNombreSocio)
                oGeneralData.SetProperty("U_CCl_Veh", strCodigoSocio)
                oGeneralData.SetProperty("U_NCl_Veh", strNombreSocio)
                If strCodigoEmpleado <> "-1" Then
                    oGeneralData.SetProperty("U_SlpCode", strCodigoEmpleado)
                    oGeneralData.SetProperty("U_SlpName", strNombreEmpleado)
                    oGeneralData.SetProperty("U_OwrCode", strCodigoEmpleado)
                    oGeneralData.SetProperty("U_OwrName", strNombreEmpleado)
                End If

                'Erick Sanabria. Agregar campos de Sucursal al Contrato de Ventas cuando es creado desde la oportunidad de Ventas
                oGeneralData.SetProperty("U_CSucu", strCodigoSucursal)
                oGeneralData.SetProperty("U_Sucu", strNombreSucursal)

                oGeneralData.SetProperty("U_Estado", strNivelContrato)
                oGeneralData.SetProperty("U_DocDate", DateAndTime.Now)
                oGeneralData.SetProperty("U_Moneda", strMoneda)
                oGeneralData.SetProperty("U_SCGD_TipoCambio", 1)
                
                oChildrenAcc = oGeneralData.Child("SCGD_ACCXCONT")
                oChildAcc = oChildrenAcc.Add()
                oChildAcc.SetProperty("U_SCGD_AccPrecio", "0")

                oChildrenRes = oGeneralData.Child("SCGD_LINEASRES")
                oChildRes = oChildrenRes.Add()
                oChildRes.SetProperty("U_Descuent", "0")

                oChildrenSum = oGeneralData.Child("SCGD_LINEASSUM")
                oChildSum = oChildrenSum.Add()
                oChildSum.SetProperty("U_Descuent", "0")

                oChildrenNuevo = oGeneralData.Child("SCGD_VEHIXCONT")
                oChildNuevo = oChildrenNuevo.Add()
                oChildNuevo.SetProperty("U_Pre_Vta", "0")

                oChildrenUsado = oGeneralData.Child("SCGD_USADOXCONT")
                oChildUsado = oChildrenUsado.Add()
                oChildUsado.SetProperty("U_Val_Rec", "0")

                oChildrenTram = oGeneralData.Child("SCGD_TRAMXCONT")
                oChildTram = oChildrenTram.Add()
                oChildTram.SetProperty("U_Pre_Uni", "0")

                oGeneralService.Add(oGeneralData)

                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCVCreado, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

                If strCodigoEmpleado <> "-1" Then
                    strCodigoVendedor = Utilitarios.EjecutarConsulta("SELECT USER_CODE FROM [OUSR]   WHERE userID = " & _
                                                    "( Select userID from OHEM where salesPrson = " & strCodigoEmpleado & " )", SBO_Company.CompanyDB, SBO_Company.Server)

                    Call EnviarMensaje(strCodigoVendedor, strNumCV, strNombreSocio)
                End If

            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.SucursalDesdeOportunidad, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If


        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Sub CreaCVCotizacion(ByVal oForm As SAPbouiCOM.Form, ByVal strNumCotizacion As String)

        Dim strNumCV As String
        Dim strNumOportunidad As String
        Dim strNombreOportunidad As String
        Dim strNombreCotizacion As String
        Dim strCodigoSocio As String
        Dim strNombreSocio As String
        Dim strCodigoEmpleado As String
        Dim strNombreEmpleado As String
        Dim strNivelContrato As String
        Dim strFechaContrato As String
        Dim strMoneda As String

        'Erick Sanabria. Agregar campos de Sucursal al Contrato de Ventas  
        Dim strCodigoSucursal As String = ""
        Dim strNombreSucursal As String = ""
        Dim strUserCode As String = ""
        Dim strConsulta As String = ""

        Dim oRecordset As SAPbobsCOM.Recordset
        Dim oSBObob As SAPbobsCOM.SBObob

        Dim oCotizacion As Documents
        Dim intNumCotizacion As Integer

        Dim strArticulo As String
        Dim strNombreArticulo As String
        Dim dblPrecioArticulo As Double
        Dim strTipoArticulo As String
        Dim intCantArticulo As Integer

        Dim dblPrecioTotal As Double = 0
        Dim dblSumaAcc As Double = 0

        Dim blAcc As Boolean = False

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildAcc As SAPbobsCOM.GeneralData
        Dim oChildrenAcc As SAPbobsCOM.GeneralDataCollection
        Dim oChildRes As SAPbobsCOM.GeneralData
        Dim oChildrenRes As SAPbobsCOM.GeneralDataCollection
        Dim oChildSum As SAPbobsCOM.GeneralData
        Dim oChildrenSum As SAPbobsCOM.GeneralDataCollection
        Dim oChildNuevo As SAPbobsCOM.GeneralData
        Dim oChildrenNuevo As SAPbobsCOM.GeneralDataCollection
        Dim oChildUsado As SAPbobsCOM.GeneralData
        Dim oChildrenUsado As SAPbobsCOM.GeneralDataCollection
        Dim oChildTram As SAPbobsCOM.GeneralData
        Dim oChildrenTram As SAPbobsCOM.GeneralDataCollection

        Dim strCodigoVendedor As String

        Dim dblTipoCambio As Double

        Dim strExisteCV As String

        Dim strCot As String

        Dim strDireccionCliente As String
        Dim strImpuestoCliente As String

        Dim strRate As String
        Dim dblRate As Double = 0
        Dim dblPrecioImp As Double
        Dim dblImpuestoTotal As Double
        Dim dblTotalConImp As Double

        Dim strCostoAcc As String
        Dim decCostoAcc As Decimal
        Dim strGestStockAlm As String
        Dim strBodegaAcc As String

        Dim strCostoTram As String
        Dim decCostoTram As Decimal
        Dim decPrecioTotalTram As Decimal = 0
        Dim decTotalTramites As Decimal = 0
        Dim blnTram As Boolean = False

        Dim n As NumberFormatInfo

        Try

            n = DIHelper.GetNumberFormatInfo(SBO_Company)

            strNumCV = Utilitarios.EjecutarConsulta("Select AutoKey from [ONNM] where ObjectCode = 'SCGD_CVT'", SBO_Company.CompanyDB, SBO_Company.Server)
            strNumOportunidad = oForm.Items.Item("74").Specific.string
            strExisteCV = Utilitarios.EjecutarConsulta("Select DocEntry from [@SCGD_CVENTA] where U_Cod_OV = '" & strNumOportunidad & "'", SBO_Company.CompanyDB, SBO_Company.Server)

            'Erick Sanabria. Agregar campos de Sucursal al Contrato de Ventas cuando es creado desde la oportunidad de Ventas con Oferta de Ventas
            'Modificación e consulta para codigo de sucursal y nombre 16/10/2013

            strConsulta = "Select USR.USER_CODE As CodigoUsuario,USR.Branch As CodigoSucursal,SUC.Name As NombreSucursal " &
                          "From OOPR OPR " &
                          "Inner Join OHEM OHEM On OPR.SlpCode=OHEM.salesPrson " &
                          "Inner Join OUSR USR On OHEM.userId=USR.USERID " &
                          "Inner Join [@SCGD_SUC_VENTA] SUC On USR.Branch = SUC.Code " &
                          "Where OPR.OpprId=" & strNumOportunidad & ""

            oDataTableSucursal.Clear()
            oDataTableSucursal.ExecuteQuery(strConsulta)

            strUserCode = oDataTableSucursal.GetValue("CodigoUsuario", 0)
            strCodigoSucursal = oDataTableSucursal.GetValue("CodigoSucursal", 0)
            strNombreSucursal = oDataTableSucursal.GetValue("NombreSucursal", 0)

            If ((strUserCode <> "") AndAlso (strCodigoSucursal <> "")) Then

                If Not String.IsNullOrEmpty(strExisteCV) AndAlso SBO_Application.MessageBox(Text:=My.Resources.Resource.PreguntaCVCreado, DefaultBtn:=2, Btn1Caption:=My.Resources.Resource.Si, Btn2Caption:="No") = 2 Then Return

                strNombreOportunidad = oForm.Items.Item("137").Specific.string
                strCot = Utilitarios.EjecutarConsulta("Select DocNum from [OQUT] where DocEntry = '" & strNumCotizacion & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                strNombreCotizacion = My.Resources.Resource.NumeroCotizacion & strCot

                strCodigoSocio = Utilitarios.EjecutarConsulta("Select CardCode from OQUT  where DocEntry = '" & strNumCotizacion & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                strNombreSocio = Utilitarios.EjecutarConsulta("Select CardName from OQUT  where DocEntry = '" & strNumCotizacion & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                strCodigoEmpleado = Utilitarios.EjecutarConsulta("Select SlpCode from OQUT  where DocEntry = '" & strNumCotizacion & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                strNombreEmpleado = Utilitarios.EjecutarConsulta("Select SlpName from OSLP  where SlpCode = '" & strCodigoEmpleado & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                strDireccionCliente = Utilitarios.EjecutarConsulta("Select ShipToDef from [OCRD] where CardCode = '" & strCodigoSocio & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                strImpuestoCliente = Utilitarios.EjecutarConsulta("Select TaxCode from [CRD1] where CardCode = '" & strCodigoSocio & "' and Address = '" & strDireccionCliente & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                strNivelContrato = Utilitarios.EjecutarConsulta("SELECT MIN(U_Prio +1) FROM [@SCGD_ADMIN9]", SBO_Company.CompanyDB, SBO_Company.Server)

                strFechaContrato = DateAndTime.Now.ToString("yyyyMMdd")

                intNumCotizacion = CInt(strNumCotizacion)

                oCotizacion = SBO_Company.GetBusinessObject(BoObjectTypes.oQuotations)
                oCotizacion.GetByKey(intNumCotizacion)

                dblTipoCambio = oCotizacion.DocRate

                If dblTipoCambio = 0 OrElse dblTipoCambio = 1 Then
                    oSBObob = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
                    oRecordset = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                    oRecordset = oSBObob.GetLocalCurrency()
                    oRecordset.MoveFirst()
                    If oRecordset.EoF Then
                        Throw New Exception(My.Resources.Resource.MonedaLocalNoConfig)
                    Else
                        strMoneda = oRecordset.Fields.Item(0).Value
                    End If
                Else
                    strMoneda = oCotizacion.DocCurrency
                End If

                oCompanyService = SBO_Company.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_CVT")

                oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)

                For i As Integer = 0 To oCotizacion.Lines.Count - 1

                    oCotizacion.Lines.SetCurrentLine(i)

                    strArticulo = oCotizacion.Lines.ItemCode

                    strNombreArticulo = oCotizacion.Lines.ItemDescription

                    intCantArticulo = oCotizacion.Lines.Quantity

                    If dblTipoCambio = 0 Or dblTipoCambio = 1 Then
                        dblPrecioArticulo = oCotizacion.Lines.Price
                    Else
                        dblPrecioArticulo = oCotizacion.Lines.Price / dblTipoCambio
                    End If

                    strTipoArticulo = Utilitarios.EjecutarConsulta("Select U_SCGD_TipoArticulo from OITM where ItemCode = '" & strArticulo & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                    If strTipoArticulo = "8" Then

                        If dblTipoCambio = 0 Or dblTipoCambio = 1 Then
                            dblPrecioTotal += CDbl(oCotizacion.Lines.LineTotal)
                        Else
                            dblPrecioTotal += CDbl(oCotizacion.Lines.RowTotalFC)
                        End If

                    ElseIf strTipoArticulo = "7" Then

                        'Agregado 17/05/2012 - Diego Herrera: Carga de costo de accesorios segun tipo de gestión de stock

                        strGestStockAlm = Utilitarios.EjecutarConsulta("Select ByWh from OITM where ItemCode = '" & strArticulo & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                        If Not strGestStockAlm = "Y" Then

                            strCostoAcc = Utilitarios.EjecutarConsultaPrecios("Select AvgPrice from OITM where ItemCode = '" & strArticulo & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                            If Not String.IsNullOrEmpty(strCostoAcc) Then
                                decCostoAcc = Decimal.Parse(strCostoAcc)
                            End If

                        ElseIf strGestStockAlm = "Y" Then

                            strBodegaAcc = Utilitarios.EjecutarConsulta("Select U_SCGD_BodAcc from [@SCGD_ADMIN] where Code = 'DMS'", SBO_Company.CompanyDB, SBO_Company.Server)
                            strCostoAcc = Utilitarios.EjecutarConsultaPrecios("Select AvgPrice from OITW where ItemCode = '" & strArticulo & "' And WhsCode = '" & strBodegaAcc & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                            If Not String.IsNullOrEmpty(strCostoAcc) Then
                                decCostoAcc = Decimal.Parse(strCostoAcc)
                            End If

                        End If

                        If Not (dblTipoCambio = 0 Or dblTipoCambio = 1) Then

                            decCostoAcc = decCostoAcc / dblTipoCambio

                        End If

                        oChildrenAcc = oGeneralData.Child("SCGD_ACCXCONT")
                        oChildAcc = oChildrenAcc.Add()
                        oChildAcc.SetProperty("U_Acc", strArticulo)
                        oChildAcc.SetProperty("U_N_Acc", strNombreArticulo)
                        oChildAcc.SetProperty("U_SCGD_AccPrecio", dblPrecioArticulo)
                        oChildAcc.SetProperty("U_Cant_Acc", intCantArticulo)
                        oChildAcc.SetProperty("U_Imp_Acc", strImpuestoCliente)
                        oChildAcc.SetProperty("U_AccPr_I", dblPrecioArticulo)
                        oChildAcc.SetProperty("U_Cost_Acc", decCostoAcc.ToString(n))

                        dblSumaAcc += dblPrecioArticulo

                        If Not String.IsNullOrEmpty(strImpuestoCliente) Then
                            dblRate = Utilitarios.RetornaImpuestoVenta(strImpuestoCliente, DateTime.Now) / 100
                        Else
                            dblRate = 0
                        End If

                        dblPrecioImp = dblPrecioArticulo * dblRate
                        dblImpuestoTotal += dblPrecioImp

                        dblTotalConImp = dblSumaAcc + dblImpuestoTotal

                        blAcc = True

                    ElseIf strTipoArticulo = "9" Then

                        strGestStockAlm = Utilitarios.EjecutarConsulta("Select ByWh from OITM where ItemCode = '" & strArticulo & "'", SBO_Company.CompanyDB, SBO_Company.Server)

                        If Not strGestStockAlm = "Y" Then

                            strCostoTram = Utilitarios.EjecutarConsultaPrecios("Select AvgPrice from OITM where ItemCode = '" & strArticulo & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                            If Not String.IsNullOrEmpty(strCostoTram) Then
                                decCostoTram = Decimal.Parse(strCostoTram)
                            End If

                        End If

                        If Not (dblTipoCambio = 0 Or dblTipoCambio = 1) Then

                            decCostoTram = decCostoTram / dblTipoCambio

                        End If

                        decPrecioTotalTram = dblPrecioArticulo * intCantArticulo

                        oChildrenTram = oGeneralData.Child("SCGD_TRAMXCONT")
                        oChildTram = oChildrenTram.Add()
                        oChildTram.SetProperty("U_Cod_Tram", strArticulo)
                        oChildTram.SetProperty("U_Des_Tram", strNombreArticulo)
                        oChildTram.SetProperty("U_Pre_Uni", dblPrecioArticulo)
                        oChildTram.SetProperty("U_Cant", intCantArticulo)
                        oChildTram.SetProperty("U_Pre_Tot", decPrecioTotalTram.ToString(n))
                        oChildTram.SetProperty("U_Costo", decCostoTram.ToString(n))

                        decTotalTramites += decPrecioTotalTram

                        blnTram = True

                    End If

                Next i

                oGeneralData.SetProperty("DocNum", strNumCV)
                oGeneralData.SetProperty("U_Cod_OV", strNumOportunidad)
                oGeneralData.SetProperty("U_Name_OV", strNombreOportunidad)
                oGeneralData.SetProperty("U_SCGD_CodCotiz", strNumCotizacion)
                oGeneralData.SetProperty("U_SCGD_NameCotiz", strNombreCotizacion)
                oGeneralData.SetProperty("U_CardCode", strCodigoSocio)
                oGeneralData.SetProperty("U_CardName", strNombreSocio)
                oGeneralData.SetProperty("U_CCl_Veh", strCodigoSocio)
                oGeneralData.SetProperty("U_NCl_Veh", strNombreSocio)

                'Erick Sanabria. Agregar campos de Sucursal al Contrato de Ventas cuando es creado desde la oportunidad de Ventas con Cotización
                oGeneralData.SetProperty("U_CSucu", strCodigoSucursal)
                oGeneralData.SetProperty("U_Sucu", strNombreSucursal)

                If strCodigoEmpleado <> "-1" Then
                    oGeneralData.SetProperty("U_SlpCode", strCodigoEmpleado)
                    oGeneralData.SetProperty("U_SlpName", strNombreEmpleado)
                    oGeneralData.SetProperty("U_OwrCode", strCodigoEmpleado)
                    oGeneralData.SetProperty("U_OwrName", strNombreEmpleado)
                End If
                oGeneralData.SetProperty("U_Estado", strNivelContrato)
                oGeneralData.SetProperty("U_DocDate", DateAndTime.Now)
                oGeneralData.SetProperty("U_Moneda", strMoneda)
                If dblTipoCambio = 0 Then
                    dblTipoCambio = 1
                End If
                oGeneralData.SetProperty("U_SCGD_TipoCambio", dblTipoCambio)
                oGeneralData.SetProperty("U_Mon_Cot", dblPrecioTotal)
                oGeneralData.SetProperty("U_Ext_Adi", dblTotalConImp)
                oGeneralData.SetProperty("U_Tot_Tram", decTotalTramites.ToString(n))

                If blAcc = False Then

                    oChildrenAcc = oGeneralData.Child("SCGD_ACCXCONT")
                    oChildAcc = oChildrenAcc.Add()
                    oChildAcc.SetProperty("U_SCGD_AccPrecio", "0")

                End If

                oChildrenRes = oGeneralData.Child("SCGD_LINEASRES")
                oChildRes = oChildrenRes.Add()
                oChildRes.SetProperty("U_Descuent", "0")

                oChildrenSum = oGeneralData.Child("SCGD_LINEASSUM")
                oChildSum = oChildrenSum.Add()
                oChildSum.SetProperty("U_Descuent", "0")

                oChildrenNuevo = oGeneralData.Child("SCGD_VEHIXCONT")
                oChildNuevo = oChildrenNuevo.Add()
                oChildNuevo.SetProperty("U_Pre_Vta", "0")

                oChildrenUsado = oGeneralData.Child("SCGD_USADOXCONT")
                oChildUsado = oChildrenUsado.Add()
                oChildUsado.SetProperty("U_Val_Rec", "0")

                If blnTram = False Then

                    oChildrenTram = oGeneralData.Child("SCGD_TRAMXCONT")
                    oChildTram = oChildrenTram.Add()
                    oChildTram.SetProperty("U_Pre_Uni", "0")

                End If

                oGeneralService.Add(oGeneralData)

                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCVCreado, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

                If strCodigoEmpleado <> "-1" Then

                    strCodigoVendedor = Utilitarios.EjecutarConsulta("SELECT USER_CODE FROM [OUSR]   WHERE userID = " & _
                                                    "( Select userID from OHEM where salesPrson = " & strCodigoEmpleado & " )", SBO_Company.CompanyDB, SBO_Company.Server)

                    Call EnviarMensaje(strCodigoVendedor, strNumCV, strNombreSocio)

                End If

            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.SucursalDesdeOportunidad, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Sub EnviarMensaje(ByVal strVendedor As String, ByVal strCodigoCV As String, ByVal strCliente As String)

        Dim oMsg As SAPbobsCOM.Messages
        Dim strMensaje As String
        Dim intResultado As Integer
        Dim strError As String = String.Empty

        Try

            If Not String.IsNullOrEmpty(strVendedor) Then

                strMensaje = My.Resources.Resource.ElContratoVenta & strCodigoCV & My.Resources.Resource.Contratode & strCliente & My.Resources.Resource.EstadoCVOportunidad

                oMsg = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages)
                oMsg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High
                oMsg.MessageText = strMensaje
                oMsg.Subject = My.Resources.Resource.ContratoCreado
                oMsg.Recipients.Add()

                oMsg.Recipients.UserCode = strVendedor
                oMsg.Recipients.NameTo = strVendedor
                oMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES
                intResultado = oMsg.Add
                If (intResultado <> 0) Then
                    SBO_Company.GetLastError(intResultado, strError)
                    Throw New ExceptionsSBO(intResultado, strError)
                End If

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub



End Class
