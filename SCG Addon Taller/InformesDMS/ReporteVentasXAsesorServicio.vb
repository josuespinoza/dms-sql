Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports DMSOneFramework

Public Class ReporteVentasXAsesorServicio : Implements IUsaPermisos, IFormularioSBO

#Region "Declaraciones"
    Private _FormType As String
    Private _NombreXml As String
    Private _Titulo As String
    Private _FormularioSBO As IForm
    Private _Inicializado As Boolean
    Private _ApplicationSBO As IApplication
    Private _CompanySBO As SAPbobsCOM.ICompany
    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String

    'Conection
    Private oComboAsesor As SAPbouiCOM.ComboBox
    Private strConsulta As String = "SELECT OHEM.empID , OSLP.SlpName FROM OSLP INNER JOIN OHEM ON OHEM.salesPrson = OSLP.SlpCode WHERE OSLP.Active = 'Y'"
    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon


#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _ApplicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _CompanySBO
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(value As Boolean)
            _Inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXml
        End Get
        Set(value As String)
            _NombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(value As String)
            _Titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(value As Integer)
            _Posicion = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="p_Application"></param>
    ''' <param name="p_CompanySbo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal p_Application As Application, ByVal p_CompanySbo As SAPbobsCOM.ICompany, ByVal mc_strUISCGD_FormSCGD_RVAS As String)
        _CompanySBO = p_CompanySbo
        _ApplicationSBO = p_Application
        NombreXml = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLReporteVentasXAsesorServicio
        MenuPadre = "SCGD_IND"
        Nombre = My.Resources.Resource.txtReporteVentasXAsesorServicio
        IdMenu = mc_strUISCGD_FormSCGD_RVAS
        Titulo = My.Resources.Resource.txtReporteVentasXAsesorServicio
        Posicion = 42
        FormType = mc_strUISCGD_FormSCGD_RVAS
    End Sub

#End Region

#Region "Eventos"
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        oComboAsesor = DirectCast(FormularioSBO.Items.Item("cboAsesor").Specific, SAPbouiCOM.ComboBox)
        Utilitarios.CargarValidValuesEnCombos(oComboAsesor.ValidValues, strConsulta, True)
    End Sub

    Sub ManejadorEventoComboSelected(pVal As SAPbouiCOM.ItemEvent, BubbleEvent As Boolean)
        Try
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Sub ManejadorEventoItemPressed(p_Val As SAPbouiCOM.ItemEvent, BubbleEvent As Boolean)
        Dim strParametros As String
        Try
            If p_Val.ItemUID = "btnprint" Then
                If CargarParametros(strParametros) Then
                    ImprimirReporte(My.Resources.Resource.rptVentasXAsesorServicio, My.Resources.Resource.rptVentasXAsesorServicioTITULO, strParametros)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

#Region "Metodos"
    ''' <summary>
    ''' Cargar los Parámetros para el reporte
    ''' </summary>
    ''' <param name="strParametros"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarParametros(ByRef strParametros As String) As Boolean
        Dim strAsesor As String
        Dim dtFi As Date
        Dim dtFf As Date

        Try
            strAsesor = " AND OINV.OwnerCode = '{0}'"

            If Not String.IsNullOrEmpty(FormularioSBO.DataSources.UserDataSources.Item("udFi").Value) And Not String.IsNullOrEmpty(FormularioSBO.DataSources.UserDataSources.Item("udFf").Value) Then

                dtFi = FormularioSBO.DataSources.UserDataSources.Item("udFi").Value.Trim()
                dtFf = FormularioSBO.DataSources.UserDataSources.Item("udFf").Value.Trim()

                If dtFi <= dtFf Then

                    If oComboAsesor.Selected IsNot Nothing Then
                        strParametros = String.Format(strAsesor, oComboAsesor.Selected.Value.Trim())
                    End If
                    strParametros = dtFf.ToString() & "," & dtFi.ToString() & "," & strParametros
                Else
                    ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorFechaInicioMenoraFechaFin, BoMessageTime.bmt_Short, False)
                    Return False
                End If

                Return True
            Else
                ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorIngresarFechaFinFechaInicio, BoMessageTime.bmt_Short, False)
                Return False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    'Imprimir reportes
    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                               ByVal strBarraTitulo As String, _
                               ByVal strParametros As String)
        Try

            Dim strPathExe As String = String.Empty

            objConfiguracionGeneral = Nothing

            Configuracion.CrearCadenaDeconexion(CompanySBO.Server, CompanySBO.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString

            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & strDireccionReporte
            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strParametros = strParametros.Replace(" ", "°")

            strBarraTitulo = strBarraTitulo.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & CompanySBO.Server & "," & CompanySBO.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

End Class
