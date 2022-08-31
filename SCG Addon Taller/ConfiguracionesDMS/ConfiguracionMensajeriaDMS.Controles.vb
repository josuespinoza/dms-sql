Imports System.Globalization
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class ConfiguracionMensajeriaDMS : Implements IFormularioSBO, IUsaPermisos

#Region "Declaraciones"
    'maneja informacion de la aplicacion
    Dim m_oApplication As Application
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
    Private Shared dtConfigUsrRol As DataTable
    Private Shared dtConfigLineas As DataTable
    Private Shared dtBusqueda As DataTable

    'Constantes
    Private Const g_strdtSucursales As String = "dtSucursales"
    Private Const g_strdtMSJS As String = "dtMSJS"
    Private Const g_strdtUsuariosBD As String = "dtUsuariosBD"
    Private Const g_strdtConfRol As String = "dtConfigUsrRol"
    Private Const g_strdtBusqueda As String = "dtBusqueda"
    Private Const mc_strFormUnidadesPorNivel As String = "SCGD_VSEP"
    Private Const strMatrizMSJ As String = "mtx_MSJ"
    
    Private Shared oComboSucursal As SCG.SBOFramework.UI.ComboBoxSBO
    
    Private Shared oComboRoles As SCG.SBOFramework.UI.ComboBoxSBO

    'matriz
    Private MatrizConfMSJ As MatrizMensajeriaDMS
    Dim udsConfiguracionMSJ As UserDataSources
    Private g_oMtxMSJ As Matrix
    
    Private oGestorFormularios As GestorFormularios
    Private oFormListaEmpSel As ListaEmpleadosSeleccion

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strUISCGD_FormConfMSJ As String)

        m_oCompany = companySbo
        m_oApplication = application
        NombreXml = Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormularioConfMSJ
        MenuPadre = "SCGD_CFG"
        Nombre = My.Resources.Resource.MenuConfMensajeria
        IdMenu = p_strUISCGD_FormConfMSJ
        Titulo = My.Resources.Resource.MenuConfMensajeria
        Posicion = 77
        FormType = p_strUISCGD_FormConfMSJ

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
            CargaFormulario()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return m_oApplication
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

#Region "...metodos..."


#End Region

    Public Sub CargaFormulario()

        'items de sap
        Dim oItem As SAPbouiCOM.Item
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox

        Try
            FormularioSBO.Freeze(True)
            FormularioSBO.EnableMenu("1282", False)

            oItem = FormularioSBO.Items.Item("mtx_MSJ")

            oMatriz = CType(oItem.Specific, SAPbouiCOM.Matrix)

            LinkComponentes()

            CreaDataTablesSBO()

            CargaSucursales()
            CargaRoles()

            oItem = FormularioSBO.Items.Item("cboSucu")
            oCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            oCombo.Active = True

            ManipulaComponentes(True, True, False, False, False, False)

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    'asocia los edit text con la tabla en base de datos
    'asocia tambien el combo
    Private Sub LinkComponentes()

        udsConfiguracionMSJ = FormularioSBO.DataSources.UserDataSources
        udsConfiguracionMSJ.Add("Sucu", BoDataType.dt_LONG_TEXT, 100)
        udsConfiguracionMSJ.Add("IdRol", BoDataType.dt_LONG_TEXT, 100)

        oComboSucursal = New SCG.SBOFramework.UI.ComboBoxSBO("cboSucu", FormularioSBO, True, "", "Sucu")
        oComboRoles = New SCG.SBOFramework.UI.ComboBoxSBO("cboRID", FormularioSBO, True, "", "IdRol")

        oComboRoles.AsignaBinding()
        oComboSucursal.AsignaBinding()

    End Sub

    Public Sub ManipulaComponentes(ByVal ComboSucu As Boolean, ByVal btn_1 As Boolean,
                                   ByVal ComboRolID As Boolean, ByVal btn_msjAdd As Boolean,
                                   ByVal btn_msjEli As Boolean, ByVal mtx_MSJ As Boolean)

        FormularioSBO.Items.Item("cboSucu").Enabled = ComboSucu
        'FormularioSBO.Items.Item("btnBus").Enabled = btn_1
        FormularioSBO.Items.Item("cboRID").Enabled = ComboRolID
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

        'dtConfigUsrRol = FormularioSBO.DataSources.DataTables.Add(g_strdtConfRol)
        'dtConfigUsrRol.Columns.Add("Sucu", BoFieldsType.ft_AlphaNumeric, 100)
        'dtConfigUsrRol.Columns.Add("NAprob", BoFieldsType.ft_AlphaNumeric, 100)

        dtConfigUsrRol = FormularioSBO.DataSources.DataTables.Add(g_strdtConfRol)
        dtConfigUsrRol.Columns.Add("UsrID", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigUsrRol.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigUsrRol.Columns.Add("EmpId", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigUsrRol.Columns.Add("DocEntry", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigUsrRol.Columns.Add("LineId", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigUsrRol.Columns.Add("RolId", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfigUsrRol.Columns.Add("UserName", BoFieldsType.ft_AlphaNumeric, 100)


        MatrizConfMSJ = New MatrizMensajeriaDMS("mtx_MSJ", FormularioSBO, g_strdtConfRol)
        MatrizConfMSJ.CreaColumnas()
        MatrizConfMSJ.LigaColumnas()

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
            Call Utilitarios.ManejadorErrores(ex, m_oApplication)
            Throw ex
        End Try

    End Sub

End Class
