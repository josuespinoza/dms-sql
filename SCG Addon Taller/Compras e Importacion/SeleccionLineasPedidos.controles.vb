Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports System.Globalization

Partial Public Class SeleccionLineasPedidos : Implements IFormularioSBO

#Region "Declaracion Variables"


    Public Shared _formularioSBO As Form
    Public Shared _companySBO As SAPbobsCOM.Company
    Public Shared _applicationSBO As Application

    Private _nombreXml As String
    Private _titulo As String
    Private _inicializado As Boolean
    Public _formType As String
    Public _codProv As String
    Private _monedaPedido As String

    Private oMatrix As Matrix

    Public txtRecepcion As EditTextSBO
    Public txtPedido As EditTextSBO
    Public txtUnidad As EditTextSBO
    Public btnAceptar As ButtonSBO
    Public btnCancelar As ButtonSBO
    Public btnActualiza As ButtonSBO
    Private dtPedidos As DataTable
    Private dtSeleccionados As DataTable
    Private matrixPedidos As MatrizSeleccionLineasPedido
    Private m_oEntradaDeVehiculos As EntradaDeVehiculos


    Public m_strCodDispoDevueltos As String
    Dim n As NumberFormatInfo

#End Region

#Region "Constructor"

    Public Sub New(ByVal SBOAplication As Application,
                ByVal ocompany As SAPbobsCOM.Company)

        _companySBO = ocompany
        _applicationSBO = SBOAplication
        n = DIHelper.GetNumberFormatInfo(_companySBO)
    End Sub

#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _companySBO
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

    Public Property CodProveedor As String
        Get
            Return _codProv
        End Get
        Set(ByVal value As String)
            _codProv = value
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
            CargarFormulario()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try
            Dim userDS As UserDataSources = _formularioSBO.DataSources.UserDataSources

            dtPedidos = _formularioSBO.DataSources.DataTables.Add("dtPedidos")

            dtPedidos.Columns.Add("pedi", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("cart", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("arti", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("ano", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("colo", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("cant", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("pend", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("cpro", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("prov", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("mont", BoFieldsType.ft_Price, 100)
            dtPedidos.Columns.Add("codCol", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("line", BoFieldsType.ft_AlphaNumeric, 100)
            dtPedidos.Columns.Add("curr", BoFieldsType.ft_AlphaNumeric, 100)

            matrixPedidos = New MatrizSeleccionLineasPedido("mtxPed", _formularioSBO, "dtPedidos")
            matrixPedidos.CreaColumnas()
            matrixPedidos.LigaColumnas()

            dtSeleccionados = _formularioSBO.DataSources.DataTables.Add("dtSeleccion")

            dtSeleccionados.Columns.Add("pedi", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("arti", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("ano", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("colo", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("cant", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("pend", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("prov", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("mont", BoFieldsType.ft_Price, 100)
            dtSeleccionados.Columns.Add("cpro", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("cart", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("codCol", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("line", BoFieldsType.ft_AlphaNumeric, 100)


            CargarMatriz()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Public Sub CargarFormulario()
        Try

            'AddChooseFromList(_formularioSBO, "2", "CFL_Pro")
            'AsignaChooseFromList("txtCodPro", "CFL_Pro", "CardCode")

        Catch ex As Exception

        End Try
    End Sub

    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = "SCGD_SLP" Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                If pVal.ActionSuccess Then
                    Select Case pVal.ItemUID
                        Case "btnAcept"
                            m_oEntradaDeVehiculos = New EntradaDeVehiculos(_applicationSBO, _companySBO, CatchingEvents.mc_strEntradaDeVehiculos)

                            If SeleccionarLineasPedidos() = True Then
                                m_oEntradaDeVehiculos.AgregarLineasSeleccionadas(dtSeleccionados)
                                'm_oCosteoDeEntradas.
                                FormularioSBO.Close()
                            Else
                                dtSeleccionados.Rows.Clear()
                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ValidacionMonedaRecepcion, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            End If


                            
                        Case "btnUpdate"
                            CargarMatriz()
                        Case "btnCancel"
                            FormularioSBO.Close()
                    End Select
                End If

                ' ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

                ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

                '   ManejadorEventoCombos(FormUID, pVal, BubbleEvent)

            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try


    End Sub



    Private Sub ManejadorEventoItemPress(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "btnAcept"

                    Case "btnUpdate"
                        CargarMatriz()

                End Select
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)
        Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim strCFL_Id As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim oDataTable As SAPbouiCOM.DataTable

        oCFLEvent = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        strCFL_Id = oCFLEvent.ChooseFromListUID
        oCFL = _formularioSBO.ChooseFromLists.Item(strCFL_Id)

        If oCFLEvent.ActionSuccess Then

            oDataTable = oCFLEvent.SelectedObjects

            If Not oCFLEvent.SelectedObjects Is Nothing Then

                If Not oDataTable Is Nothing And
                    _formularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then
                    Select Case pval.ItemUID
                        Case "txtCodPro"
                            AsignaValoresProveedor(oDataTable)
                        Case "txtCodArt"

                    End Select

                End If
            End If

        ElseIf oCFLEvent.BeforeAction Then

            Select Case pval.ItemUID
                Case "txtCodArt"
                    oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_SCGD_TipoArticulo"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = "8"
                    oCondition.BracketCloseNum = 1
                    oCFL.SetConditions(oConditions)

                Case "txtCodPro"

                    oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                    oCondition = oConditions.Add()

                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "CardType"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_EQUAL
                    oCondition.CondVal = "S"
                    oCondition.BracketCloseNum = 1

                    oCondition.Relationship = BoConditionRelationship.cr_AND

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "frozenFor"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                    oCondition.CondVal = "Y"

                    oCondition.BracketCloseNum = 1

                    oCFL.SetConditions(oConditions)


            End Select

        End If
    End Sub

    Public Sub AsignaValoresProveedor(ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            FormularioSBO.Freeze(True)

            Dim oItem As SAPbouiCOM.Item
            Dim otext As SAPbouiCOM.EditText

            Dim test1 As String = oDataTable.GetValue("CardCode", 0)
            Dim test2 As String = oDataTable.GetValue("CardName", 0)

            oItem = _formularioSBO.Items.Item("txtCodPro")
            otext = DirectCast(oItem.Specific, SAPbouiCOM.EditText)

            otext.Value = test1
            'DirectCast(_formularioSBO.Items.Item("txtNamPro").Specific, SAPbouiCOM.EditText).Value.Trim()

            '_formularioSBO.Items.Item("txtCodPro").Specific.value = test1
            '_formularioSBO.Items.Item("txtNamPro").Specific.value = test2


            otext.Value = test1


            ' DirectCast(_formularioSBO.Items.Item("txtPedido").Specific, SAPbouiCOM.EditText).Value.Trim()



            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

End Class
