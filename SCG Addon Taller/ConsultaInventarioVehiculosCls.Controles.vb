Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework
Imports SAPbobsCOM
Imports SAPbouiCOM

Partial Public Class ConsultaInventarioVehiculosCls : Implements IFormularioSBO

#Region "... Declaraciones ..."
    'propiedades
    Private m_strDireccionConfiguracion As String
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As SAPbouiCOM.IForm
    Private _inicializado As Boolean
    Private _applicationSBO As SAPbouiCOM.IApplication

    Public num_OT As String
    Private m_objMensajeria As New Proyecto_SCGMSGBox.SCGMSGBox

    'controles de interfaz
    Private g_oEditNoOT As SAPbouiCOM.EditText
    Private g_oEditNoCot As SAPbouiCOM.EditText
    Private g_oMtxOtLines As SAPbouiCOM.Matrix
    Private m_dbLineas As SAPbouiCOM.DBDataSource
    Private Shared cboTOT As ComboBoxSBO

    Private sboItem As SAPbouiCOM.Item
    Private sboCombo As SAPbouiCOM.ComboBox

    'userDataSource
    Private UDS_SeleccionaRepuestos As UserDataSources

    'matriz Solicita OT especial
    Private mtxConsultaInventario As MatrizConsultaInventario
    Private mtxConsultaPedidos As MatrizConsultaPedidos

    'tabla para repuestos
    Private dtLineas As Data.DataTable

    'constantes
    Private Const strDataTablePed As String = "tTodosLineas"
    Private Const strDataTableInv As String = "tTodosInventario"
    'Public Const mc_strTipoOtEspeciales As String = "cboTipOtE"
    Public Const mc_strMatrixInventario As String = "mtx_0"
    Public Const mc_strMatrixPedidos As String = "mtxPedidos"
    Public Const mc_strDtLocal As String = "dtLocal"

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal application As System.Windows.Forms.Application, ByVal companySbo As SAPbobsCOM.ICompany)
        _companySbo = companySbo
        _applicationSBO = application

        n = DIHelper.GetNumberFormatInfo(_companySbo)
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

    'Propiedad Formulario
    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
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

    Public Property StrConexion As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property
#End Region


#Region "Métodos"

    ''' <summary>
    ''' Carga el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaFormulario()
        Dim etAno As EditText
        Dim etCantInv As EditText
        Dim dtLocal As DataTable
        Dim userDS As UserDataSources

        Try
            'FormularioSBO.Freeze(True)
            dtLocal = FormularioSBO.DataSources.DataTables.Add(mc_strDtLocal)

            'matriz para todos los repuestos
            m_dtPedidos = FormularioSBO.DataSources.DataTables.Add(mc_strDTPedidos)
            m_dtPedidos.Columns.Add("col_code", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_desc", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_year", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_color", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_qtyP", BoFieldsType.ft_Quantity, 100)
            m_dtPedidos.Columns.Add("col_qtyR", BoFieldsType.ft_Quantity, 100)
            m_dtPedidos.Columns.Add("col_trac", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_tran", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_feca", BoFieldsType.ft_AlphaNumeric, 100)
            m_dtPedidos.Columns.Add("col_comb", BoFieldsType.ft_AlphaNumeric, 100)

            m_dbInventario = FormularioSBO.DataSources.DataTables.Add(mc_strSCG_VEHICULO)
            m_dbInventario.Columns.Add("Col_Code", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Unid", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Vin", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Mot", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Marca", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Estilo", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Mode", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("CardName", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Dias", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_MarcM", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Trans", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Tracc", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Combu", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Tech", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Ubic", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Tipo", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Dispo", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Col", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_ColTa", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Carro", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Cab", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Cate", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Ano", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Esta", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_FecAr", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_FecRe", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_FecVe", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Vend", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Mon", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Pre", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Val", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("Col_Bon", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("col_Plac", BoFieldsType.ft_AlphaNumeric, 100)
            m_dbInventario.Columns.Add("col_Res", BoFieldsType.ft_AlphaNumeric, 100)


            userDS = FormularioSBO.DataSources.UserDataSources
            userDS.Add("Ano", BoDataType.dt_SHORT_NUMBER)
            userDS.Add("InvCantD", BoDataType.dt_SHORT_NUMBER)
            etAno = DirectCast(FormularioSBO.Items.Item(mc_strAno).Specific, SAPbouiCOM.EditText)
            etAno.DataBind.SetBound(True, "", "Ano")
            etCantInv = DirectCast(FormularioSBO.Items.Item(mc_strCantidadDias).Specific, SAPbouiCOM.EditText)
            etCantInv.DataBind.SetBound(True, "", "InvCantD")


            'crea matriz pedidos
            mtxConsultaPedidos = New MatrizConsultaPedidos(mc_strMatrixPedidos, FormularioSBO, mc_strDTPedidos)
            mtxConsultaPedidos.CreaColumnas()
            mtxConsultaPedidos.LigaColumnas()

            'crea matriz inventario
            mtxConsultaInventario = New MatrizConsultaInventario(mc_strMatrixInventario, FormularioSBO, mc_strSCG_VEHICULO)
            mtxConsultaInventario.CreaColumnas()
            mtxConsultaInventario.LigaColumnas()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub
    
#End Region


End Class
