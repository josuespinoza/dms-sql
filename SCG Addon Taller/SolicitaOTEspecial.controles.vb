Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework
Imports SAPbobsCOM
Imports SAPbouiCOM

'*******************************************
'*Maneja los controles del formulario Solicita ot especial
'*******************************************
Partial Public Class SolicitaOTEspecial : Implements IFormularioSBO

#Region "... Declaraciones ..."
    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSBO As IApplication
    Public num_OT As String
    'controles de interfaz
    Private g_oEditNoOT As EditText
    Private g_oEditNoCot As EditText
    Private g_oMtxOtLines As Matrix
    Private Shared cboTOT As ComboBoxSBO

    Private sboItem As Item
    Private sboCombo As ComboBox

    'userDataSource
    Private UDS_SeleccionaRepuestos As UserDataSources

    'matriz Solicita OT especial
    Private MatrizSolicitaOtEsp As MatrizSolicitaOTEspecial

    'tabla para repuestos
    Private dtLineas As DataTable

    'constantes
    Private Const strDataTableLineas As String = "tTodosLineas"
    Public Const mc_strTipoOtEspeciales As String = "cboTipOtE"
    Public Const mc_strMatizCotLines As String = "mtxOTLines"

#End Region

#Region "Constructor"
    

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

        Dim dtLocal As DataTable

        Try
            FormularioSBO.Freeze(True)
            'asocia controles de interfaz
            AsociaControlesInterfaz()
            'matriz para todos los repuestos
            dtLineas = FormularioSBO.DataSources.DataTables.Add(strDataTableLineas)
            dtLineas.Columns.Add("col_Sel", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Code", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Name", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Quant", BoFieldsType.ft_Quantity, 100)
            dtLineas.Columns.Add("col_Curr", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Price", BoFieldsType.ft_Price, 100)
            dtLineas.Columns.Add("col_Obs", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_DEnt", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_LNum", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_PrcDes", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_IdRXOr", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Costo", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_IndImp", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Compra", BoFieldsType.ft_AlphaNumeric, 10)
            dtLineas.Columns.Add("col_CPend", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_CSol", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_CRec", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_PenDev", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_PenTra", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_PenBod", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_IDLine", BoFieldsType.ft_AlphaNumeric)
            dtLineas.Columns.Add("col_TipAr", BoFieldsType.ft_AlphaNumeric)
            
            'crea matriz
            MatrizSolicitaOtEsp = New MatrizSolicitaOTEspecial(mc_strMatizCotLines, FormularioSBO, strDataTableLineas)
            MatrizSolicitaOtEsp.CreaColumnas()
            MatrizSolicitaOtEsp.LigaColumnas()

            MatrizSolicitaOtEsp.Matrix.Columns.Item("col_Sel").Editable = True

            dtLocal = FormularioSBO.DataSources.DataTables.Add("local")

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Asocia controles con la interfaz
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AsociaControlesInterfaz()
        Try
            UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources
            UDS_SeleccionaRepuestos.Add("TOT", BoDataType.dt_LONG_TEXT, 100)

            cboTOT = New ComboBoxSBO(mc_strTipoOtEspeciales, FormularioSBO, True, "", "TOT")
            cboTOT.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

#End Region

End Class
