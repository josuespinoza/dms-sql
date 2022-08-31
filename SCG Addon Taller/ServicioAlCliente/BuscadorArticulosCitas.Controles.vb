Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany
Imports System.Globalization
Imports System.Threading


Partial Public Class BuscadorArticulosCitas : Implements IFormularioSBO, IUsaMenu

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
    Private _strDireccionReportes As String
    Private _strUsuarioBD As String
    Private _strContraseñaBD As String
    Public m_oCompany As SAPbobsCOM.Company


 
    Private g_objMatrizAdicionales As MatrizBusquedaArticulosCitas


    Private g_dtAdicionales As SAPbouiCOM.DataTable
    Public g_dtAdicionalesSeleccionados As SAPbouiCOM.DataTable
    Private g_ObjMatrizArticulos As MatrizBusquedaArticulosCitas
    Public g_strmtxAdicionales As String = "mtxArt"
    Public g_strdtAdicionales As String = "dtAdicionales"
    Public g_strdtAdicionalesSeleccionados As String = "dtSeleccionados"
    Public g_strUsaConfEstiMode As String
    Public g_strFiltroEstiMod As String
    Public g_CodVehi As Integer
    Public g_strCodUsa As String
    Public g_FiltroAUsar As String
    Public dtArtTab As System.Data.DataTable

    Public strCodLisPrecio As String
    Public strCodCliente As String
    Public idSucursal As Integer
    Public g_strConsultaArticulos As String
    Public g_strConsultaTablaArtEsp As String
    Public g_strConsultaServExternos As String
    Public g_strConsultaExistenciaArticulos As String
    Public m_strConsultaArticulos As String =
                   " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " +
                   " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " +
                   " 1 as cant, it.Price as prec, it.Currency as mone, oi.U_SCGD_T_Fase as nofa, oi.U_SCGD_Duracion as dura,oi.CodeBars" +
                   " from OITM as oi with (nolock) " +
                   " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                   " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                   " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}' "
    Public m_strConsultaTablaArtEsp As String =
                    "  select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " +
                    " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " +
                    " 1 as cant, it.Price as prec, it.Currency as mone,  oi.U_SCGD_T_Fase as nofa, Art.U_Duracion as dura,oi.CodeBars" +
                    " from OITM as oi with (nolock) " +
                    " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                    " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode   " +
                    " inner join [@SCGD_ARTXESP] as Art with(nolock) on oi.ItemCode = art.U_ItemCode  " +
                    " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}'  {2}  "

    Public m_strConsultaServExternos As String = " select top(100) '' as sele, oi.ItemCode as code, oi.ItemName as 'desc', cfnb.U_Rep as bode, " +
                   " (select OnHand from OITW with (nolock) where oitw.WhsCode = cfnb.U_Rep and oitw.ItemCode = oi.ItemCode) as csto, " +
                   " 1 as cant, it.Price as prec, it.Currency as mone,  oi.U_SCGD_T_Fase as nofa, oi.U_SCGD_Duracion as dura,oi.CodeBars " +
                   " from OITM as oi with (nolock) " +
                   " inner join [@SCGD_CONF_BODXCC] as cfnb with (nolock) on oi.U_SCGD_CodCtroCosto = cfnb.U_CC " +
                   " inner join ITM1 as it with (nolock) on oi.ItemCode = it.ItemCode  " +
                   " where it.PriceList = '{0}' and cfnb.DocEntry = '{1}' and oi.U_SCGD_TipoArticulo in(3,4,5)  "

    Public m_strConsultaExistenciaArticulos As String = "  Select Count(U_ItemCode) as U_ItemCode from [@SCGD_ARTXESP] as art where U_TipoArt in (1,2) "
    

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As SAPbobsCOM.Company)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)

    End Sub

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

    Public Property StrDireccionReportes As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
        End Set
    End Property

#End Region

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

        g_dtAdicionales = FormularioSBO.DataSources.DataTables.Add(g_strdtAdicionales)
        g_dtAdicionales.Columns.Add("Sel", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Cod", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Desc", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Bod", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Stock", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Cant", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Prec", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Mon", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("NoF", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("Dur", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionales.Columns.Add("CodBar", BoFieldsType.ft_AlphaNumeric, 100)
        g_objMatrizAdicionales = New MatrizBusquedaArticulosCitas(g_strmtxAdicionales, FormularioSBO, g_strdtAdicionales)
        g_objMatrizAdicionales.CreaColumnas()
        g_objMatrizAdicionales.LigaColumnas()

        g_dtAdicionalesSeleccionados = FormularioSBO.DataSources.DataTables.Add(g_strdtAdicionalesSeleccionados)
        g_dtAdicionalesSeleccionados.Columns.Add("Cod", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("Desc", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("Bod", BoFieldsType.ft_AlphaNumeric, 100)
        ''g_dtAdicionalesSeleccionados.Columns.Add("Stock", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("Cant", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("Prec", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("Mon", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("NoF", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("Dur", BoFieldsType.ft_AlphaNumeric, 100)
        g_dtAdicionalesSeleccionados.Columns.Add("CodBar", BoFieldsType.ft_AlphaNumeric, 100)

        g_strConsultaArticulos = m_strConsultaArticulos
        g_strConsultaTablaArtEsp = m_strConsultaTablaArtEsp
        g_strConsultaServExternos = m_strConsultaServExternos
        g_strConsultaExistenciaArticulos = m_strConsultaExistenciaArticulos


    End Sub

    Private Sub CargarFormulario()
        Try
            FormularioSBO.Freeze(True)

          

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


End Class
