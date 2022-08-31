Imports System.Globalization
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Class ConfiguracionNivAprobacion
    : Implements IFormularioSBO, IUsaPermisos

    'maneja informacion de la aplicacion
    Private m_SBO_Application As Application
    'maneja informacion de la compania 
    Private m_oCompany As ICompany
    Private _formType As String
    Private _formularioSBO As IForm
    Private _inicializado As Boolean
    

    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer
    Public n As NumberFormatInfo
    
    'dt
    Private Shared dtSucursales As DataTable
    Private Shared dtSucursalesEnMSJS As DataTable
    Private Shared dtUsuariosBD As DataTable
    Private Shared dtConfigNAprob As DataTable
    Private Shared dtConfigLineas As DataTable
    Private Shared dtBusqueda As DataTable

    'objeto matriz
    Private Const g_strdtSucursales As String = "dtSucursales"
    Private Const g_strdtMSJS As String = "dtMSJS"
    Private Const g_strdtUsuariosBD As String = "dtUsuariosBD"
    Private Const g_strdtConfigNAprob As String = "dtConfigNAprob"
    Private Const g_strdtConfigLineas As String = "dtConfigL"
    Private Const g_strdtBusqueda As String = "dtBusqueda"

    Private Shared oComboSucursal As UI.ComboBoxSBO
    Private Shared oComboNiveles As UI.ComboBoxSBO

    'matriz
    Private MatrizConfigNAprob As MatrizConfigNAprob

    Dim udsConfiguracionNAprob As UserDataSources
    

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strMenuConfigNivAprob As String)
        m_oCompany = companySbo
        m_SBO_Application = application
        n = DIHelper.GetNumberFormatInfo(m_oCompany)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLConfigNivAprob
        MenuPadre = "SCGD_CFG"
        Nombre = My.Resources.Resource.TituloConfigNivAprob
        IdMenu = p_strMenuConfigNivAprob
        Posicion = 74
        FormType = p_strMenuConfigNivAprob
        m_oUnidadesXNivel = New UsuariosPorNAprob(m_oCompany, m_SBO_Application)

    End Sub

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

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSBO = value
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

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

    End Sub

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try
            CargaFormularioConfigNivAprob()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return m_SBO_Application
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return m_oCompany
        End Get
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

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

#End Region

    Public Sub CargaFormularioConfigNivAprob()

        'items de sap
        Dim oItem As SAPbouiCOM.Item
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox

        Try
            FormularioSBO.Freeze(True)
            FormularioSBO.EnableMenu("1282", False)

            oItem = FormularioSBO.Items.Item("mtx_MSJ")

            oMatriz = CType(oItem.Specific, SAPbouiCOM.Matrix)
            oMatriz.Columns.Item("Col_CSucu").Visible = False
            oMatriz.Columns.Item("Col_LineId").Visible = False

            LinkComponentes()

            CreaDataTablesSBO()

            CargaSucursales()

            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT U_Codigo, U_Name FROM [@SCGD_ADMIN9]", "cboNAp")
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT U_CSucu, U_Sucu FROM [@SCGD_MSJS]", "cboSucu")

            oItem = FormularioSBO.Items.Item("cboSucu")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            oCombo.Active = True

            ManipulaComponentes(True, True, False, False, False, False)

            m_oUnidadesXNivel = New UsuariosPorNAprob(m_oCompany, m_SBO_Application)
            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'asocia los edit text con la tabla en base de datos
    'asocia tambien el combo
    Private Sub LinkComponentes()

        udsConfiguracionNAprob = FormularioSBO.DataSources.UserDataSources
        udsConfiguracionNAprob.Add("Sucu", BoDataType.dt_LONG_TEXT, 100)
        udsConfiguracionNAprob.Add("NAprob", BoDataType.dt_LONG_TEXT, 100)

        oComboNiveles = New UI.ComboBoxSBO("cboNAp", FormularioSBO, True, "", "Sucu")
        oComboSucursal = New UI.ComboBoxSBO("cboSucu", FormularioSBO, True, "", "NAprob")

        oComboNiveles.AsignaBinding()
        oComboSucursal.AsignaBinding()

    End Sub

    Public Sub ManipulaComponentes(ByVal ComboSucu As Boolean, ByVal btn_1 As Boolean,
                                   ByVal ComboNApro As Boolean, ByVal btn_msjAdd As Boolean,
                                   ByVal btn_msjEli As Boolean, ByVal mtx_MSJ As Boolean)

        FormularioSBO.Items.Item("cboSucu").Enabled = ComboSucu
        'FormularioSBO.Items.Item("btnBus").Enabled = btn_1
        FormularioSBO.Items.Item("cboNAp").Enabled = ComboNApro
        FormularioSBO.Items.Item("btn_MSJAdd").Enabled = btn_msjAdd
        FormularioSBO.Items.Item("btn_MSJEli").Enabled = btn_msjEli
        FormularioSBO.Items.Item("mtx_MSJ").Enabled = mtx_MSJ

    End Sub

    'crea datatables para manejod e sucursales y Niveles de aprobacion
    Private Sub CreaDataTablesSBO()

        dtSucursales = FormularioSBO.DataSources.DataTables.Add(g_strdtSucursales)
        dtSucursales.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
        dtSucursales.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric, 100)

        dtSucursalesEnMSJS = FormularioSBO.DataSources.DataTables.Add(g_strdtMSJS)
        dtSucursalesEnMSJS.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
        dtSucursalesEnMSJS.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric, 100)

        dtConfigNAprob = FormularioSBO.DataSources.DataTables.Add(g_strdtConfigNAprob)
        dtConfigNAprob.Columns.Add("Sucu", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigNAprob.Columns.Add("NAprob", BoFieldsType.ft_AlphaNumeric, 100)

        dtConfigLineas = FormularioSBO.DataSources.DataTables.Add(g_strdtConfigLineas)
        dtConfigLineas.Columns.Add("usua", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("cnap", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("rmsj", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("mcv", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("csucu", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("lineid", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigLineas.Columns.Add("acv", BoFieldsType.ft_AlphaNumeric, 100)


        MatrizConfigNAprob = New MatrizConfigNAprob("mtx_MSJ", FormularioSBO, g_strdtConfigLineas)
        MatrizConfigNAprob.CreaColumnas()
        MatrizConfigNAprob.LigaColumnas()

        dtUsuariosBD = FormularioSBO.DataSources.DataTables.Add(g_strdtUsuariosBD)

        dtBusqueda = FormularioSBO.DataSources.DataTables.Add(g_strdtBusqueda)

    End Sub

    'carga los combos
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

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
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
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try

    End Sub

End Class
