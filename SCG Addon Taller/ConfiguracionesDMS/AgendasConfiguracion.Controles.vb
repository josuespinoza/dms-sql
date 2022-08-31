Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class AgendasConfiguracion : Implements IFormularioSBO, IUsaMenu

#Region "Declaraciones"
    Private _formType As String
    Private _nombreXml As String
    Private _titulo As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSbo As IApplication
    Private _companySbo As ICompany
    Private _idMenu As String
    Private _menuPadre As String
    Private _posicion As Integer
    Private _nombre As String

    Dim oDataTable As DataTable

    Private m_strCboSucursal As String = "cboSucur"

    Private m_numSucursarActual As Integer
    Private m_strAbrevAnt As String

    Private TablaAgenda As String = "@SCGD_AGENDA"
    Private m_lEstadoNuevo As Boolean = True
    Private m_lModificoCampos As Boolean = False

    Public Structure EmpleadoUDT

        Public CodEmpleado As String
        Public DetEmpleado As String

    End Structure

    Public mtxAgenda As MatrixSBO

    Public EditTextAgenda As EditTextSBO
    Public EditTextAgendaNomb As EditTextSBO
    Public EditTextCodAsesor As EditTextSBO
    Public EditTextNomAsesor As EditTextSBO
    Public EditTextCodTecnico As EditTextSBO
    Public EditTextNomTecnico As EditTextSBO
    Public EditTextArticulo As EditTextSBO
    Public EditTextArticuloNomb As EditTextSBO

    Public EditTextIntervalo As EditTextSBO
    Public EditTextAbreviatura As EditTextSBO

    Public EditTextCitasLunes As EditTextSBO
    Public EditTextCitasMartes As EditTextSBO
    Public EditTextCitasMiercoles As EditTextSBO
    Public EditTextCitasJueves As EditTextSBO
    Public EditTextCitasViernes As EditTextSBO
    Public EditTextCitasSabado As EditTextSBO
    Public EditTextCitasDomingo As EditTextSBO

    Public EditCboSucursal As ComboBoxSBO
    Public EditCboRazon As ComboBoxSBO

    Public EditCbxDisponible As CheckBoxSBO
    Public EditCbxUsaWeb As CheckBoxSBO
    Public EditCbxUsaTmpServ As CheckBoxSBO

    Public EditBtnAceptar As ButtonSBO
    Public EditBtnCancelar As ButtonSBO

    #End Region
#Region "Propiedades"
    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo() As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property IdMenu() As String Implements IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre() As String Implements IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Posicion() As Integer Implements IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Property Nombre() As String Implements IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

#End Region
#Region "Metodos - Funciones"

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        Try
            If FormularioSBO IsNot Nothing Then

            End If
            Dim m_strAgendaTabla As String = "@SCGD_AGENDA"

            EditTextAgenda = New EditTextSBO("txtDocNum", True, m_strAgendaTabla, "DocNum", FormularioSBO)
            EditTextAgendaNomb = New EditTextSBO("txtAgenda", True, m_strAgendaTabla, "U_Agenda", FormularioSBO)
            EditTextCodAsesor = New EditTextSBO("txtCodAses", True, m_strAgendaTabla, "U_CodAsesor", FormularioSBO)
            EditTextNomAsesor = New EditTextSBO("txtAsesor", True, m_strAgendaTabla, "U_NameAsesor", FormularioSBO)
            EditTextCodTecnico = New EditTextSBO("txtCodEnc", True, m_strAgendaTabla, "U_CodTecnico", FormularioSBO)
            EditTextNomTecnico = New EditTextSBO("txtEncarg", True, m_strAgendaTabla, "U_NameTecnico", FormularioSBO)
            EditTextArticulo = New EditTextSBO("txtCodArt", True, m_strAgendaTabla, "U_Num_Art", FormularioSBO)

            EditTextArticuloNomb = New EditTextSBO("txtArt", True, m_strAgendaTabla, "U_ArticuloCita", FormularioSBO)
            EditTextIntervalo = New EditTextSBO("txtInterv", True, m_strAgendaTabla, "U_IntervaloCitas", FormularioSBO)
            EditTextAbreviatura = New EditTextSBO("txtAbrev", True, m_strAgendaTabla, "U_Abreviatura", FormularioSBO)

            EditTextCitasLunes = New EditTextSBO("txtLunes", True, m_strAgendaTabla, "U_CantCLunes", FormularioSBO)
            EditTextCitasMartes = New EditTextSBO("txtMartes", True, m_strAgendaTabla, "U_CantCMartes", FormularioSBO)
            EditTextCitasMiercoles = New EditTextSBO("txtMierc", True, m_strAgendaTabla, "U_CantCMiercoles", FormularioSBO)
            EditTextCitasJueves = New EditTextSBO("txtJueves", True, m_strAgendaTabla, "U_CantCJueves", FormularioSBO)
            EditTextCitasViernes = New EditTextSBO("txtViernes", True, m_strAgendaTabla, "U_CantCViernes", FormularioSBO)
            EditTextCitasSabado = New EditTextSBO("txtSabado", True, m_strAgendaTabla, "U_CantCSabado", FormularioSBO)
            EditTextCitasDomingo = New EditTextSBO("txtDoming", True, m_strAgendaTabla, "U_CantCDomingo", FormularioSBO)

            EditCboSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, m_strAgendaTabla, "U_Cod_Sucursal")
            EditCboRazon = New ComboBoxSBO("cboRazon", FormularioSBO, True, m_strAgendaTabla, "U_RazonCita")

            EditCbxDisponible = New CheckBoxSBO("cbxEstado", True, m_strAgendaTabla, "U_EstadoLogico", FormularioSBO)
            EditCbxUsaWeb = New CheckBoxSBO("cbxUsaWeb", True, m_strAgendaTabla, "U_VisibleWeb", FormularioSBO)
            EditCbxUsaTmpServ = New CheckBoxSBO("cboUsaTiem", True, m_strAgendaTabla, "U_TmpServ", FormularioSBO)

            EditTextAgenda.AsignaBinding()
            EditTextAgendaNomb.AsignaBinding()
            EditTextCodAsesor.AsignaBinding()
            EditTextNomAsesor.AsignaBinding()
            EditTextCodTecnico.AsignaBinding()
            EditTextNomTecnico.AsignaBinding()
            EditTextArticulo.AsignaBinding()
            EditTextArticuloNomb.AsignaBinding()
            EditTextIntervalo.AsignaBinding()
            EditTextAbreviatura.AsignaBinding()
            EditTextCitasLunes.AsignaBinding()
            EditTextCitasMartes.AsignaBinding()
            EditTextCitasMiercoles.AsignaBinding()
            EditTextCitasJueves.AsignaBinding()
            EditTextCitasViernes.AsignaBinding()
            EditTextCitasSabado.AsignaBinding()
            EditTextCitasDomingo.AsignaBinding()
            EditCboSucursal.AsignaBinding()
            EditCboRazon.AsignaBinding()

            EditCbxDisponible.AsignaBinding()
            EditCbxUsaWeb.AsignaBinding()
            EditCbxUsaTmpServ.AsignaBinding()

            EditBtnAceptar = New ButtonSBO("1", FormularioSBO)
            EditBtnCancelar = New ButtonSBO("2", FormularioSBO)




        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try
            CargarFormulario()
            CargarCombos()
            
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

  Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = FormType Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                ManejadorEventoItemPress(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

                ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

                ManejadorEventoCombos(FormUID, pVal, BubbleEvent)

            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try


    End Sub
    
    Public Sub ManejadorEventosChooseFromList(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
        sCFL_ID = oCFLEvento.ChooseFromListUID
        oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

        If pVal.BeforeAction Then

            If pVal.ItemUID = EditTextCodAsesor.UniqueId Then
                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add()
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "userId"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)

            ElseIf (pVal.ItemUID = EditTextCodTecnico.UniqueId) Then
                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_T_Fase"
                oCondition.Operation = BoConditionOperation.co_NOT_NULL

                oCondition.BracketCloseNum = 1

                oCFL.SetConditions(oConditions)

            End If

        ElseIf pVal.ActionSuccess Then

            oDataTable = oCFLEvento.SelectedObjects
            If Not oDataTable Is Nothing Then
                If pVal.ItemUID = EditTextCodAsesor.UniqueId Then
                    AsignaValoresEditTextUIAsesor(FormUID, pVal, BubbleEvent)
                ElseIf pVal.ItemUID = EditTextCodTecnico.UniqueId Then
                    AsignaValoresEditTextUITecnico(FormUID, pVal, BubbleEvent)
                ElseIf pVal.ItemUID = EditTextArticulo.UniqueId Then
                    AsignaValoresEditTextUIArticulo(FormUID, pVal, BubbleEvent)
                End If

            End If

        End If
    End Sub
    
    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID

                    Case EditBtnAceptar.UniqueId
                        If pVal.FormMode = BoFormMode.fm_ADD_MODE OrElse pVal.FormMode = BoFormMode.fm_UPDATE_MODE Then
                            ValidarCampos(formUID, pVal, bubbleEvent)
                        End If

                End Select

            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                  
                End Select
              
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub ManejadorEventoCombos(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean)
        Try
            Dim cboCombo As SAPbouiCOM.ComboBox
            Dim oItem As SAPbouiCOM.Item
            Dim l_strSucursal As String
            
            If pVal.ActionSuccess Then
                If pVal.ItemUID = m_strCboSucursal Then

                    oItem = FormularioSBO.Items.Item(m_strCboSucursal)
                    cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                    l_strSucursal = CStr(cboCombo.Selected.Value)
                    m_numSucursarActual = l_strSucursal
                    Call obtenerAgendasPorSucursar(l_strSucursal)
                    
                End If
            ElseIf pVal.BeforeAction Then
                If pVal.ItemUID = m_strCboSucursal Then

                    If m_lModificoCampos Then
                        If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosAgenda, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                            bubbleEvent = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByVal p_oForm As Form, ByRef BubbleEvent As Boolean)
        Try
            m_strAbrevAnt = EditTextAbreviatura.ObtieneValorDataSource()
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            If pval.MenuUID = "1281" Then
                FormularioSBO.Items.Item(EditTextAgenda.UniqueId).Enabled = True
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub CargarFormulario()

        dtListaAgendas = FormularioSBO.DataSources.DataTables.Add("listAgendas")

        dtListaAgendas.Columns.Add("DocNum", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAgendas.Columns.Add("U_Agenda", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAgendas.Columns.Add("U_EstadoLogico", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAgendas.Columns.Add("U_IntervaloCitas", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAgendas.Columns.Add("U_Abreviatura", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAgendas.Columns.Add("U_CodAsesor", BoFieldsType.ft_AlphaNumeric, 100)
        dtListaAgendas.Columns.Add("U_VisibleWeb", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizAgendas = New AgendasConfiguracionMatriz("mtxAgenda", FormularioSBO, "listAgendas")

        MatrizAgendas.CreaColumnas()
        MatrizAgendas.LigaColumnas()

    End Sub

    Public Sub obtenerAgendasPorSucursar(ByVal p_strCodSucursal As String)
        Try
            Dim l_strSQL As String

            l_strSQL = "SELECT ""DocNum"", ""U_Agenda"", ""U_EstadoLogico"", ""U_IntervaloCitas"", ""U_Abreviatura"", ""U_NameTecnico"", ""U_VisibleWeb"" FROM ""@SCGD_AGENDA"" WHERE ""U_Cod_Sucursal"" = '{0}' ORDER BY ""DocNum"" ASC"
            l_strSQL = String.Format(l_strSQL, p_strCodSucursal)

            FormularioSBO.Freeze(True)

            dtListaAgendas.ExecuteQuery(l_strSQL)
            If dtListaAgendas.Rows.Count > 0 Then
                MatrizAgendas.Matrix.LoadFromDataSource()
            Else
                dtListaAgendas.Clear()
                MatrizAgendas.Matrix.LoadFromDataSource()
            End If

            If EditCboSucursal.ObtieneValorDataSource() = "" Then
                EditCboSucursal.AsignaValorDataSource(m_numSucursarActual)
            End If

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub CargarCombos()

        Dim sboItem As SAPbouiCOM.Item
        Dim sboCombo As SAPbouiCOM.ComboBox

        sboItem = FormularioSBO.Items.Item("cboSucur")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "SELECT ""Code"",""Name"" FROM ""@SCGD_SUCURSALES""  Order by ""Name""")

        sboItem = FormularioSBO.Items.Item("cboRazon")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "Select ""Code"", ""Name"" From ""@SCGD_RAZONCITA"" Order by ""Name""")


    End Sub

    Public Sub AsignaValoresEditTextUIAsesor(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            EditTextCodAsesor.AsignaValorDataSource(oDataTable.GetValue("empID", 0))
            EditTextNomAsesor.AsignaValorDataSource(oDataTable.GetValue("firstName", 0) & " " & oDataTable.GetValue("lastName", 0))
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresEditTextUITecnico(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Try
            EditTextCodTecnico.AsignaValorDataSource(oDataTable.GetValue("empID", 0))
            EditTextNomTecnico.AsignaValorDataSource(oDataTable.GetValue("firstName", 0) & " " & oDataTable.GetValue("lastName", 0))
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresEditTextUIArticulo(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Try
            EditTextArticulo.AsignaValorDataSource(oDataTable.GetValue("ItemCode", 0))
            EditTextArticuloNomb.AsignaValorDataSource(oDataTable.GetValue("ItemName", 0))

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

 #End Region

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strUISCGD_FormAgendasConfiguracion As String)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        NombreXml = Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormularioAgendasConfig
        MenuPadre = "SCGD_CDE"
        Nombre = "Agendas"
        IdMenu = p_strUISCGD_FormAgendasConfiguracion
        Titulo = "Agendas"
        Posicion = 2
        FormType = p_strUISCGD_FormAgendasConfiguracion
    End Sub

End Class
