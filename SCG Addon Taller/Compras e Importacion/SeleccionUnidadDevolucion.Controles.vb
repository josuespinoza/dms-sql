Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI

Partial Public Class SeleccionUnidadDevolucion : Implements IFormularioSBO



#Region "Declaracion Variables"
    Private MatrizSeleccion As MatrizSeleccionUnidad

    Public Shared _formularioSBO As Form
    Public Shared _companySBO As SAPbobsCOM.Company
    Public Shared _applicationSBO As Application

    Private _nombreXml As String
    Private _titulo As String
    Private _inicializado As Boolean
    Private oMatrix As Matrix
    Public txtRecepcion As EditTextSBO
    Public txtPedido As EditTextSBO
    Public txtUnidad As EditTextSBO

    Public btnAceptar As ButtonSBO
    Public btnCancelar As ButtonSBO
    Public btnActualiza As ButtonSBO
    Public _formType As String

    Public dtSeleccionados As DataTable
    Private Shared dtVehiculos As DataTable

    Public m_strCodDispoDevueltos As String
#End Region


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

#Region "Constructor"

    Public Sub New(ByVal SBOAplication As Application,
                    ByVal ocompany As SAPbobsCOM.Company)

        _companySBO = ocompany
        _applicationSBO = SBOAplication
        n = DIHelper.GetNumberFormatInfo(_companySBO)
    End Sub

#End Region

#Region "Metodos / Funciones"


    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try
            CargarFormulario()
        Catch ex As Exception

        End Try
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try

            LigarControles()


        Catch ex As Exception

        End Try
    End Sub

    Public Sub CargarFormulario()

        Try


            dtSeleccionados = _formularioSBO.DataSources.DataTables.Add("dtSeleccionados")

            dtSeleccionados.Columns.Add("pedi", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("rece", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("unid", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("marc", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("esti", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("mode", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("moto", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("tipo", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("mont", BoFieldsType.ft_Price, 100)
            dtSeleccionados.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("rate", BoFieldsType.ft_Float, 100)
            dtSeleccionados.Columns.Add("asie", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("stat", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("line", BoFieldsType.ft_Integer, 10)
            dtSeleccionados.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)

            m_oDevolucionDeVehiculos = New DevolucionDeVehiculos(_applicationSBO, _companySBO, CatchingEvents.mc_strDevolucionDeVehiculos)



            CargarMatriz()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub



    Private Sub LigarControles()
        Try

            Dim userDS As UserDataSources = _formularioSBO.DataSources.UserDataSources

            dtVehiculos = _formularioSBO.DataSources.DataTables.Add("dtVehiculos")

            dtVehiculos.Columns.Add("rece", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("pedi", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("unid", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("marc", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("esti", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("mode", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("moto", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("tipo", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("mont", BoFieldsType.ft_Price, 100)
            dtVehiculos.Columns.Add("mone", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("rate", BoFieldsType.ft_Price, 100)
            dtVehiculos.Columns.Add("asie", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("stat", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)

            MatrizSeleccion = New MatrizSeleccionUnidad("mtxVeh", _formularioSBO, "dtVehiculos")
            MatrizSeleccion.CreaColumnas()
            MatrizSeleccion.LigaColumnas()

            'userDS.Add("pedido", BoDataType.dt_LONG_TEXT, 100)
            'userDS.Add("recepcion", BoDataType.dt_LONG_TEXT, 100)
            'userDS.Add("unidad", BoDataType.dt_LONG_TEXT, 100)

            'txtPedido = New EditTextSBO("txtPedi", True, "", "pedido", _formularioSBO)
            'txtRecepcion = New EditTextSBO("txtRece", True, "", "recepcion", _formularioSBO)
            'txtUnidad = New EditTextSBO("txtUnid", True, "", "unidad", _formularioSBO)

            btnAceptar = New ButtonSBO("btnAcep", _formularioSBO)
            btnCancelar = New ButtonSBO("btnCanc", _formularioSBO)
            btnActualiza = New ButtonSBO("btnActu", _formularioSBO)

            'txtPedido.AsignaBinding()
            'txtRecepcion.AsignaBinding()
            'txtUnidad.AsignaBinding()



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try



    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

#End Region


End Class
