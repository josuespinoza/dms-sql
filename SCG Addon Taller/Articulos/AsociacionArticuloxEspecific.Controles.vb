Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Public Delegate Function CargaFormularioAsociaxEspDelegate(ByVal form As IFormularioSBO) As Form

Partial Public Class AsociacionArticuloxEspecific : Implements IFormularioSBO, IUsaMenu


    Private _cargaFormulario As CargaFormularioAsociaxEspDelegate

    Private _formType As String

    Private _nombreXml As String

    Private _titulo As String

    Private _menuPadre As String

    Private _nombreMenu As String

    Private _idMenu As String

    Private _posicion As Integer

    Private _formularioSbo As IForm

    Private _inicializado As Boolean

    Private _applicationSbo As Application

    Private _companySbo As ICompany

    Private _strConexion As String


    Public EditTextArticulo As EditTextSBO
    Public EditTextDescArticulo As EditTextSBO

    Public CheckBoxSelecAllE As CheckBoxSBO
    Public CheckBoxSelecAllM As CheckBoxSBO

    Public ButtonBuscar As ButtonSBO
    Public ButtonCrear As ButtonSBO
    Public ButtonCancelar As ButtonSBO

    Public FolderEstilo As FolderSBO
    Public FolderModelo As FolderSBO

    Public matrixtest As MatrixSBO

    Public MatrixEstilo As MatrixSBOEstiloAsocArtXEsp
    Public MatrixModelo As MatrixSBOModeloAsocArtXEsp

    Public DataTableEstilo As DataTable
    Public DataTableModelo As DataTable
    Public DataTableEspecific As DataTable
    Public DataTableConsulta As DataTable

    Public Especificacion As String

    Public Enum TiposArticulo

        Repuesto = 1
        Servicio = 2
        Suministro = 3
        ServicioExterno = 4

    End Enum

    Private tipoArticulo As String

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        DMS_Connector.Helpers.SetCulture(Threading.Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture )

    End Sub
    

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If Not FormularioSBO Is Nothing Then

            FormularioSBO.Freeze(True)

            For Each Item As SAPbouiCOM.Item In FormularioSBO.Items

                Item.AffectsFormMode = False

            Next

            CargarFormulario()

            FormularioSBO.Freeze(False)

        End If

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        If Not FormularioSBO Is Nothing Then

            FormularioSBO.Freeze(True)

            Dim userDataSource As UserDataSources = FormularioSBO.DataSources.UserDataSources
            userDataSource.Add("articulo", BoDataType.dt_LONG_TEXT, 150)
            userDataSource.Add("descArt", BoDataType.dt_LONG_TEXT, 225)
            userDataSource.Add("selAllE", BoDataType.dt_SHORT_TEXT, 10)
            userDataSource.Add("selAllM", BoDataType.dt_SHORT_TEXT, 10)

            DataTableEstilo = FormularioSBO.DataSources.DataTables.Add("SeleccionE")
            DataTableEstilo.Columns.Add("selec", BoFieldsType.ft_AlphaNumeric, 100)
            DataTableEstilo.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            DataTableEstilo.Columns.Add("duraE", BoFieldsType.ft_Price, 100)
            DataTableEstilo.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100)

            DataTableModelo = FormularioSBO.DataSources.DataTables.Add("SeleccionM")
            DataTableModelo.Columns.Add("selec", BoFieldsType.ft_AlphaNumeric, 100)
            DataTableModelo.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            DataTableModelo.Columns.Add("desc", BoFieldsType.ft_AlphaNumeric, 100)
            DataTableModelo.Columns.Add("duraE", BoFieldsType.ft_Price, 100)

            DataTableEspecific = FormularioSBO.DataSources.DataTables.Add("Especific")
            DataTableConsulta = FormularioSBO.DataSources.DataTables.Add("Consulta")

            EditTextArticulo = New EditTextSBO("txtArticul", True, "", "articulo", FormularioSBO)
            EditTextDescArticulo = New EditTextSBO("txtDescArt", True, "", "descArt", FormularioSBO)

            CheckBoxSelecAllE = New CheckBoxSBO("chkSelAllE", True, "", "selAllE", FormularioSBO)
            CheckBoxSelecAllM = New CheckBoxSBO("chkSelAllM", True, "", "selAllM", FormularioSBO)

            FolderEstilo = New FolderSBO("fldEstilo", FormularioSBO)
            FolderModelo = New FolderSBO("fldModelo", FormularioSBO)

            ButtonBuscar = New ButtonSBO("btnBuscar", FormularioSBO)
            ButtonCrear = New ButtonSBO("1", FormularioSBO)
            ButtonCancelar = New ButtonSBO("2", FormularioSBO)

            MatrixEstilo = New MatrixSBOEstiloAsocArtXEsp("mtxEstilo", FormularioSBO, "SeleccionE")
            MatrixModelo = New MatrixSBOModeloAsocArtXEsp("mtxModelo", FormularioSBO, "SeleccionM")

            EditTextArticulo.AsignaBinding()
            EditTextDescArticulo.AsignaBinding()

            CheckBoxSelecAllE.AsignaBinding()
            CheckBoxSelecAllM.AsignaBinding()

            MatrixEstilo.CreaColumnas()
            MatrixEstilo.LigaColumnas()
            MatrixModelo.CreaColumnas()
            MatrixModelo.LigaColumnas()

            FormularioSBO.Freeze(False)

        End If

    End Sub

    Private Sub CargarFormulario()

        ManejoTabs()

    End Sub

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            If pVal.ItemUID = ButtonCrear.UniqueId Then

                ButtonSBOCrearEventoItemPressed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = FolderEstilo.UniqueId Then

                FormularioSBO.Freeze(True)
                FormularioSBO.PaneLevel = 1
                FormularioSBO.Freeze(False)

            ElseIf pVal.ItemUID = FolderModelo.UniqueId Then

                FormularioSBO.Freeze(True)
                FormularioSBO.PaneLevel = 2
                FormularioSBO.Freeze(False)

            ElseIf pVal.ItemUID = CheckBoxSelecAllE.UniqueId Then

                CheckSBOSelectAllEItemPressed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = CheckBoxSelecAllM.UniqueId Then

                CheckSBOSelectAllMItemPressed(FormUID, pVal, BubbleEvent)

            ElseIf pVal.ItemUID = MatrixEstilo.UniqueId Then

                FormularioSBO.Items.Item("fldModelo").Enabled = False

            ElseIf pVal.ItemUID = MatrixModelo.UniqueId Then

                FormularioSBO.Items.Item("fldEstilo").Enabled = False

            End If

        ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

            If pVal.ItemUID = ButtonBuscar.UniqueId Then

                EditTextArticulo.AsignaValorUserDataSource("")
                EditTextDescArticulo.AsignaValorUserDataSource("")

                CFLArticulos(FormUID, pVal)
                CargarMatrixSBO(FormUID, pVal, BubbleEvent)


            End If


        End If




    End Sub


    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                            ByVal strQuery As String, _
                                                            ByRef strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Try
            oItem = oForm.Items.Item(strIDItem)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            Configuracion.CrearCadenaDeconexion(CompanySBO.Server, CompanySBO.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then

                    cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Sub

#Region "Propiedades"

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
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
            _formularioSbo = value
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

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _nombreMenu
        End Get
        Set(ByVal value As String)
            _nombreMenu = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Property StrConexion As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property CargaFormulario As CargaFormularioAsociaxEspDelegate
        Get
            Return _cargaFormulario
        End Get
        Set(ByVal value As CargaFormularioAsociaxEspDelegate)
            _cargaFormulario = value
        End Set
    End Property

#End Region

    

End Class
