Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany


Partial Public Class CrearDocumentosGastosCostos : Implements IFormularioSBO

#Region "Declaraciones"

    Public Shared oForm As SAPbouiCOM.Form
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany

    Private Shared _formType As String
    Private Shared _formularioSBO As SAPbouiCOM.IForm
    Private Shared _inicializado As Boolean

    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

    'formulario
    Dim m_oFormularioCrearDocumentos As CrearDocumentosGastosCostos

    Private Shared txtProveedor As EditTextSBO
    Private Shared txtProveedorNam As EditTextSBO
    Private Shared txtNoOrden As EditTextSBO
    Private Shared txtNoUnid As EditTextSBO
    Private Shared txtTipoOrden As EditTextSBO
    Private Shared txtObs As EditTextSBO
    Private Shared txtDocE As EditTextSBO

    Private Shared chxFactura As CheckBoxSBO
    Private Shared chxAsiento As CheckBoxSBO

    Dim btnCancel As ButtonSBO
    Dim btnCrear As ButtonSBO

    Private Shared oGestorFormularios As GestorFormularios
    Private Shared UDS_IncluyeGastos As UserDataSources
    Private UDS_SeleccionaRepuestos As UserDataSources

    Dim n As NumberFormatInfo

    Private Shared dtGastos As SAPbouiCOM.DataTable
    Private Shared dtConfig As SAPbouiCOM.DataTable
    Private Shared dtLocal As SAPbouiCOM.DataTable


    Private Const strDTGastos As String = "tGastos"
    Private m_strUDFGeneraAsiento As String = "U_GenASGastos"
    Private m_strUDFGeneraFactura As String = "U_GenFAGastos"
    Dim MatrizDocGastos As MatrizDocumentoGastos

    Private oFormularioIncluirGastos As IncluirGastosCostosOT

    Private Enum TipoDocumento
        Factura = 1
        Orden = 2
        Asiento = 3
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)

        _companySbo = companySbo
        _applicationSbo = application

        n = DIHelper.GetNumberFormatInfo(_companySbo)
        oFormularioIncluirGastos = New IncluirGastosCostosOT(application, companySbo, CatchingEvents.strMenuIncluirGastosOT)

    End Sub
#End Region

#Region "Propieades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property


#End Region

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try
            CargaFormulario()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

    End Sub

    Private Sub CargaFormulario()


        Dim strUsuario As String
        Dim strSucursalUsuario As String
        Dim strUsaAsientos As String
        Dim strUsaFactura As String
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            oForm = ApplicationSBO.Forms.Item("SCGD_GenDoc")
            _formularioSBO = CType(oForm, SAPbouiCOM.IForm)

            oForm.Freeze(True)

            'asocia controles de interfaz

            '  AgregaCFLProv()
            AgregaCFLImp(oForm)

            AsociaControlesInterfaz()


            dtGastos = FormularioSBO.DataSources.DataTables.Add(strDTGastos)
            dtGastos.Columns.Add("sel", BoFieldsType.ft_AlphaNumeric, 10)
            dtGastos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("can", BoFieldsType.ft_Quantity, 100)
            dtGastos.Columns.Add("mon", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("cos", BoFieldsType.ft_Price, 100)
            dtGastos.Columns.Add("pre", BoFieldsType.ft_Price, 100)
            dtGastos.Columns.Add("imp", BoFieldsType.ft_AlphaNumeric, 100)
            dtGastos.Columns.Add("lnum", BoFieldsType.ft_AlphaNumeric, 100)


            MatrizDocGastos = New MatrizDocumentoGastos("mtxGas", FormularioSBO, strDTGastos)
            MatrizDocGastos.CreaColumnas()
            MatrizDocGastos.LigaColumnas()

            chxFactura.AsignaValorUserDataSource("Y")

            dtConfig = FormularioSBO.DataSources.DataTables.Add("DatosConfig")
            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtLocal")

            'Para el CFL de la impuesto en la columna --------
            oitem = oForm.Items.Item("mtxGas")
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item("Col_imp").ChooseFromListUID = "CFL_Imp"
            oMatrix.Columns.Item("Col_imp").ChooseFromListAlias = "Code"
            '-----------------------------------------------

            strUsuario = ApplicationSBO.Company.UserName
            strSucursalUsuario = Utilitarios.EjecutarConsulta(
                String.Format("Select Branch from OUSR where USER_CODE = '{0}'", strUsuario),
                                                              CompanySBO.CompanyDB,
                                                              CompanySBO.Server)


            strUsaAsientos = DevuelveValorItem(strSucursalUsuario, m_strUDFGeneraAsiento)
            strUsaFactura = DevuelveValorItem(strSucursalUsuario, m_strUDFGeneraFactura)




            If strUsaAsientos = "Y" And strUsaFactura = "Y" Then
                chxFactura.AsignaValorUserDataSource("Y")
                chxAsiento.AsignaValorUserDataSource("N")

                oForm.Items.Item(txtProveedor.UniqueId).Enabled = True
                oForm.Items.Item(chxAsiento.UniqueId).Enabled = True
                oForm.Items.Item(chxFactura.UniqueId).Enabled = True

            ElseIf strUsaAsientos = "Y" And strUsaFactura = "N" Then
                chxFactura.AsignaValorUserDataSource("N")
                chxAsiento.AsignaValorUserDataSource("Y")

                oForm.Items.Item(txtProveedor.UniqueId).Enabled = False
                oForm.Items.Item(chxAsiento.UniqueId).Enabled = True
                oForm.Items.Item(chxFactura.UniqueId).Enabled = False

            ElseIf strUsaAsientos = "N" And strUsaFactura = "Y" Then
                chxFactura.AsignaValorUserDataSource("Y")
                chxAsiento.AsignaValorUserDataSource("N")

                oForm.Items.Item(txtProveedor.UniqueId).Enabled = True
                oForm.Items.Item(chxAsiento.UniqueId).Enabled = False
                oForm.Items.Item(chxFactura.UniqueId).Enabled = True

            ElseIf strUsaAsientos = "N" And strUsaFactura = "N" Then
                chxFactura.AsignaValorUserDataSource("N")
                chxAsiento.AsignaValorUserDataSource("N")

                oForm.Items.Item(txtProveedor.UniqueId).Enabled = False
                oForm.Items.Item(chxAsiento.UniqueId).Enabled = False
                oForm.Items.Item(chxFactura.UniqueId).Enabled = False

                oForm.Items.Item("btnCrear").Enabled = False
            End If

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub AsociaControlesInterfaz()
        Try
            UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources
            UDS_SeleccionaRepuestos.Add("asi", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("fac", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("NoOT", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("CodUnid", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("TipoOT", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("Observ", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("Prov", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("ProvN", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("DocEnt", BoDataType.dt_LONG_TEXT, 100)

            txtNoOrden = New EditTextSBO("txtNoOT", True, "", "NoOt", FormularioSBO)
            txtNoUnid = New EditTextSBO("txtCodUnid", True, "", "CodUnid", FormularioSBO)
            txtProveedor = New EditTextSBO("txPro", True, "", "Prov", FormularioSBO)
            txtProveedorNam = New EditTextSBO("txProN", True, "", "ProvN", FormularioSBO)
            txtTipoOrden = New EditTextSBO("txtTipoOT", True, "", "TipoOT", FormularioSBO)
            txtObs = New EditTextSBO("txtObs", True, "", "Observ", FormularioSBO)
            txtDocE = New EditTextSBO("txtDocE", True, "", "DocEnt", FormularioSBO)

            chxAsiento = New CheckBoxSBO("chx1", True, "", "asi", FormularioSBO)
            chxFactura = New CheckBoxSBO("chx2", True, "", "fac", FormularioSBO)

            chxFactura.AsignaBinding()
            chxAsiento.AsignaBinding()

            txtNoOrden.AsignaBinding()
            txtNoUnid.AsignaBinding()
            txtProveedor.AsignaBinding()
            txtProveedorNam.AsignaBinding()
            txtTipoOrden.AsignaBinding()
            txtObs.AsignaBinding()
            txtDocE.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub AgregaCFLProv()

        ' Dim oCP As SAPbouiCOM.FormCreationParams
        Dim oItem As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText

        oItem = oForm.Items.Item("txPro")
        oEdit = oItem.Specific

        'oCP = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
        'oCP.UniqueID = "CFL_Prov"
        'oCP.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
        oForm.DataSources.UserDataSources.Add("IdEmpleado", SAPbouiCOM.BoDataType.dt_SHORT_TEXT)

        oEdit.DataBind.SetBound(True, "", "IdEmpleado")
        oEdit.ChooseFromListUID = "CFL_Prov"
        oEdit.ChooseFromListAlias = "CardCode"

    End Sub

    Private Sub AgregaCFLImp(ByVal oform As Form)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "128"
            oCFLCreationParams.UniqueID = "CFL_Imp"
            oCFL = oCFLs.Add(oCFLCreationParams)

            '' Adding Conditions to CFL1
            'oCons = oCFL.GetConditions()
            'oCon = oCons.Add()
            'oCon.Alias = "U_SCGD_T_Fase"
            'oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            'oCFL.SetConditions(oCons)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    'Private Sub AgregaCFLImp()

    '    'Dim oCFLs As SAPbouiCOM.ChooseFromListCollection = oForm.ChooseFromLists
    '    'Dim oCFL As SAPbouiCOM.ChooseFromList
    '    'Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams

    '    'oCFLs = oForm.ChooseFromLists

    '    'oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)
    '    'oCFLCreationParams.MultiSelection = False
    '    'oCFLCreationParams.ObjectType = "37"
    '    'oCFLCreationParams.UniqueID = "CFL_Imp"
    '    'oCFL = oCFLs.Add(oCFLCreationParams)


    '    Dim oMatriz As SAPbouiCOM.Matrix
    '    Dim oColumns As SAPbouiCOM.Columns
    '    Dim oColumn As SAPbouiCOM.Column
    '    oForm.DataSources.UserDataSources.Add("Prueba", SAPbouiCOM.BoDataType.dt_SHORT_TEXT)


    '    oMatriz = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)
    '    oColumns = oMatriz.Columns
    '    oColumn = oColumns.Item("Col_imp")

    '    oColumn.DataBind.SetBound(True, "", "Prueba")
    '    oColumn.ChooseFromListUID = "CFL_Imp"
    '    oColumn.ChooseFromListAlias = "Col_imp"

    'End Sub

    
#Region "EVentos"
    Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If Not pVal.FormTypeEx = "SCGD_GenDoc" Then Return

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
            Case BoEventTypes.et_CHOOSE_FROM_LIST
                ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
        End Select

    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oMatrix As Matrix

        Try

            If pVal.BeforeAction = True Then
                Select Case pVal.ItemUID

                    Case chxFactura.UniqueId
                        ControlesCrearFactura()

                    Case chxAsiento.UniqueId
                        ControlesCrearAsiento()

                    Case "btnCrear"
                        If ValidarDatos(pVal, BubbleEvent) Then

                        Else
                            Exit Sub
                        End If
                    Case "btnCancel"
                        If FormularioSBO.Mode <> BoFormMode.fm_VIEW_MODE Then
                            If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCreaDocGastoPerderaDatos, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                                BubbleEvent = False
                                oForm.Close()
                            End If
                        Else
                            FormularioSBO.Close()
                        End If

                End Select
            ElseIf pVal.ActionSuccess = True Then
                Select Case pVal.ItemUID
                    Case "mtxGas"
                        oForm.Freeze(True)
                        If pVal.ColUID = "Col_sel" Then

                            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
                            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)
                            oMatrix.FlushToDataSource()
                        End If
                        oForm.Freeze(False)
                    Case "btnCrear"
                        If chxAsiento.ObtieneValorUserDataSource = "Y" AndAlso
                            (chxFactura.ObtieneValorUserDataSource = "N" OrElse String.IsNullOrEmpty(chxFactura.ObtieneValorUserDataSource)) Then

                            CrearAsientoGastos()

                        ElseIf (chxAsiento.ObtieneValorUserDataSource = "N" Or String.IsNullOrEmpty(chxAsiento.ObtieneValorUserDataSource)) AndAlso
                                chxFactura.ObtieneValorUserDataSource = "Y" Then

                            CrearFactura()
                        End If
                End Select

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub ManejadorEventosChooseFromList(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.IItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim oConditions As SAPbouiCOM.Conditions
            Dim oCondition As SAPbouiCOM.Condition

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim strCFL_Id As String

            Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)

            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            strCFL_Id = oCFLEvent.ChooseFromListUID
            oCFL = _formularioSBO.ChooseFromLists.Item(strCFL_Id)

            If oCFLEvent.BeforeAction = False Then
                Dim oDataTable As SAPbouiCOM.DataTable
                oDataTable = oCFLEvent.SelectedObjects

                If Not oCFLEvent.SelectedObjects Is Nothing Then
                    If Not oDataTable Is Nothing And
                        _formularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then

                        Select Case pVal.ItemUID
                            Case txtProveedor.UniqueId
                                AsignaValoresTxtProvedor(FormUID, pVal, oDataTable)
                            Case "mtxGas"
                                AsignaValoresColImpuesto(FormUID, pVal, oDataTable)

                        End Select

                    End If
                End If
            ElseIf oCFLEvent.BeforeAction = True Then
                Select Case pVal.ItemUID
                    Case txtProveedor.UniqueId
                        oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add

                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "CardType"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "S"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)
                    Case "mtxGas"
                        oForm.Items.Item(txtObs.UniqueId).Click()
                End Select

            End If




        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaValoresTxtProvedor(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            txtProveedor.AsignaValorUserDataSource(oDataTable.GetValue("CardCode", 0))
            txtProveedorNam.AsignaValorUserDataSource(oDataTable.GetValue("CardName", 0))

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresColImpuesto(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix

        Try

            dtGastos = oForm.DataSources.DataTables.Item(strDTGastos)
            oMatrix = DirectCast(oForm.Items.Item("mtxGas").Specific, Matrix)

            dtGastos.SetValue("imp", pVal.Row, oDataTable.GetValue("Code", 0))
            oMatrix.LoadFromDataSource()

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub




#End Region


End Class
