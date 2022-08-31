Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports System.Timers
Imports ICompany = SAPbobsCOM.ICompany
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Linq
Imports SAPbobsCOM


Partial Public Class CitasReservacion : Implements IFormularioSBO, IUsaMenu


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
    Private _strConexion As String
    Private _strDireccionReportes As String
    Private _strUsuarioBD As String
    Private _strContraseñaBD As String
    Dim dtConsulta As SAPbouiCOM.DataTable

    Public m_oCompany As SAPbobsCOM.Company

    'Dim oDataTable As SAPbouiCOM.DataTable

    Private m_strCita As String = "@SCGD_CITA"
    Private m_strCboAgenda As String = "cboAgenda"
    Private m_strCboSucursal As String = "cboSucur"

    Private m_strListaPreciosCli As String
    Private m_strCardCode As String
    Private m_strCodUnid As String
    Private m_strIDVehi As String
    Private m_strHoraCita As String
    Private m_strFechaCita As String
    Private m_strTipoAgenda As String
    'Fila seleccionada
    Private _intFila As Integer = -1

    'EditText 
    'Encabezado
    Public EditTextMarca As EditTextSBO
    Public EditTextEstilo As EditTextSBO
    Public EditTextModelo As EditTextSBO
    Public EditTextCombustible As EditTextSBO
    Public EditTextMotor As EditTextSBO
    Public EditTextAno As EditTextSBO
    Public EditTextTiempo As EditTextSBO
    Public EditTextServicios As EditTextSBO

    Public EditTextUnidad As EditTextSBO
    'Public EditTextPlaca As EditTextSBO
    Public EditTextNumCita As EditTextSBO
    Public EditTextCotizacion As EditTextSBO
    Public EditTextFecha As EditTextSBO
    Public EditTextHora As EditTextSBO
    Public EditTextObservaciones As EditTextSBO
    Public EditTextCardCode As EditTextSBO
    Public EditTextCardName As EditTextSBO

    Public EditTextCardCodeCliOT As EditTextSBO
    Public EditTextCardNameCliOT As EditTextSBO
    Public EditTextNomAsesor As EditTextSBO
    'Public EditTextCodTecnico As EditTextSBO
    'Public EditTextNomTecnico As EditTextSBO
    Public EditTextNumSerie As EditTextSBO
    Public EditTextCreado As EditTextSBO
    Public EditTextDocEntry As EditTextSBO
    Public EditTextCampaña As EditTextSBO
    Public EditTextCampañaNombre As EditTextSBO
    Public EditTextTotalLineas As EditTextSBO
    Public EditTextTotalImpuesto As EditTextSBO
    Public EditTextTotalDocumento As EditTextSBO
    Public EditTextTipoCambio As EditTextSBO
    Public EditTextFechaDoc As EditTextSBO
    Public EditTextFechaCitaFin As EditTextSBO
    Public EditTextHoraCitaFin As EditTextSBO
    Public EditTextIdVehiculo As EditTextSBO

    Public EditTextFechaServFin As EditTextSBO
    Public EditTextHoraServFin As EditTextSBO

    Public EditTextNumPlaca As EditTextSBO
    Public EditTextFhaServicio As EditTextSBO
    Public EditTextHoraServicio As EditTextSBO

    Public EditCboEstado As ComboBoxSBO
    Public EditCboSucursal As ComboBoxSBO
    Public EditCboAgenda As ComboBoxSBO
    Public EditCboRazon As ComboBoxSBO
    Public EditCboMoneda As ComboBoxSBO
    Public EditCboTecnico As ComboBoxSBO
    Public EditCboAsesor As ComboBoxSBO


    Public EditCbxArticulos As CheckBoxSBO
    Public EditCbxTiempo As CheckBoxSBO
    Dim loRow() As DataRow

    Private md_Datos As SAPbouiCOM.DataTable
    Private md_Cita As SAPbouiCOM.DataTable
    Private md_Agenda As SAPbouiCOM.DataTable

    Private md_Configuracion As SAPbouiCOM.DataTable
    Private md_Campana As SAPbouiCOM.DataTable
    Private md_Suspension As SAPbouiCOM.DataTable
    Private md_Paquetes As SAPbouiCOM.DataTable
    Private md_Local As SAPbouiCOM.DataTable
    Private md_Local2 As SAPbouiCOM.DataTable

    Private md_ArtPadre As SAPbouiCOM.DataTable
    
    Private m_strCodCitasCancel As String

    Private m_strImpRepuesto As String
    Private m_strImpServicio As String
    Private m_strImpSuministro As String
    Private m_strImpServExt As String

    Private m_strUsaGruposTrabajo As String = "N"
    Private m_strNombreBDTaller As String
    Private m_strTiempoServEmpleado As String = String.Empty

    Private WithEvents _frmAgendaCitas As frmCalendario
    Private WithEvents _frmAgendaCitasColor As frmCalendarioColor
    Private WithEvents _frmAcupacionAgendas As frmListaCitas

    Private m_TypeVehiculo As String = String.Empty
    Private m_TypeCountVehiculo As String

    Private WithEvents m_oVehiculo As VehiculosCls
    Dim n As NumberFormatInfo

    Private m_blnFlagTimer As Boolean = False
    Shared m_oTimer As Timer

    Dim versionsap As Integer
    Dim m_blnVersion9 As Boolean = True

    Enum TipoDeAgenda
        Mecanico = 0
        Agenda = 1
        Grupos = 2
    End Enum

    Public g_strUsaConfEstiMode = String.Empty
    Public g_strFiltroEstiMod = String.Empty


#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As SAPbobsCOM.Company, ByVal p_menuCitas As String, ByVal p_strUISCGD_Citas As String)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        NombreXml = Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLCitasReservacion
        MenuPadre = p_menuCitas
        Nombre = "Citas"
        IdMenu = p_strUISCGD_Citas
        Titulo = My.Resources.Resource.TituloCitas
        Posicion = 2
        FormType = p_strUISCGD_Citas
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = CatchingEvents.DBUser
        StrContraseñaBD = CatchingEvents.DBPassword
        n = DIHelper.GetNumberFormatInfo(_companySbo)

    End Sub

#End Region

#Region "Propieadades"

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

    Public Property StrConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property StrDireccionReportes() As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD() As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD() As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
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

    Public Property intFila() As Integer
        Get
            Return _intFila
        End Get
        Set(ByVal value As Integer)
            _intFila = value
        End Set
    End Property

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As SAPbobsCOM.ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property
#End Region

#Region "Metodos / Funciones"

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        FormularioSBO.Freeze(True)

        addTextComentarios()

        'Conexion a los componentes que NO se encuentran en la matriz - Los EditText
        Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources

        'agrega columnas al ds
        userDS.Add("marca", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("estilo", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("modelo", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("combust", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("motor", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("ano", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("tiempo", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("serv", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("tiemp", BoDataType.dt_LONG_TEXT, 100)


        'instancia los edittext
        EditTextMarca = New EditTextSBO("txtMarca", True, "", "marca", FormularioSBO)
        EditTextEstilo = New EditTextSBO("txtEstilo", True, "", "estilo", FormularioSBO)
        EditTextModelo = New EditTextSBO("txtModelo", True, "", "modelo", FormularioSBO)
        EditTextCombustible = New EditTextSBO("txtCombust", True, "", "combust", FormularioSBO)
        EditTextMotor = New EditTextSBO("txtMotor", True, "", "motor", FormularioSBO)
        EditTextAno = New EditTextSBO("txtAno", True, "", "ano", FormularioSBO)
        EditTextTiempo = New EditTextSBO("txtTE", True, "", "tiempo", FormularioSBO)
        EditTextServicios = New EditTextSBO("txtCS", True, "", "serv", FormularioSBO)
        EditCbxTiempo = New CheckBoxSBO("chkTiempo", True, "", "tiemp", FormularioSBO)

        EditTextUnidad = New EditTextSBO("txtCodUnid", True, m_strCita, "U_Cod_Unid", FormularioSBO)
        EditTextNumCita = New EditTextSBO("txtNoCita", True, m_strCita, "U_NumCita", FormularioSBO)
        EditTextCotizacion = New EditTextSBO("txt_NumCot", True, m_strCita, "U_Num_Cot", FormularioSBO)
        EditTextFecha = New EditTextSBO("txtFhaCita", True, m_strCita, "U_FechaCita", FormularioSBO)
        EditTextHora = New EditTextSBO("txtHora", True, m_strCita, "U_HoraCita", FormularioSBO)
        EditTextObservaciones = New EditTextSBO("txtObserv", True, m_strCita, "U_Observ", FormularioSBO)
        EditTextCardCode = New EditTextSBO("txtCliente", True, m_strCita, "U_CardCode", FormularioSBO)
        EditTextCardName = New EditTextSBO("txtNombre", True, m_strCita, "U_CardName", FormularioSBO)
        EditTextCardCodeCliOT = New EditTextSBO("txt_cliOT", True, m_strCita, "U_CCliOT", FormularioSBO)
        EditTextCardNameCliOT = New EditTextSBO("txt_NoClOT", True, m_strCita, "U_NCliOT", FormularioSBO)
        EditTextNomAsesor = New EditTextSBO("txtNomAses", True, m_strCita, "U_Name_Asesor", FormularioSBO)
        EditTextNumSerie = New EditTextSBO("txtSerie", True, m_strCita, "U_Num_Serie", FormularioSBO)
        EditTextCreado = New EditTextSBO("txtCreador", True, m_strCita, "U_CreadoPor", FormularioSBO)
        EditTextDocEntry = New EditTextSBO("txtDocEntr", True, m_strCita, "DocEntry", FormularioSBO)
        EditTextCampaña = New EditTextSBO("txtCpnNo", True, m_strCita, "U_CpnNo", FormularioSBO)
        EditTextCampañaNombre = New EditTextSBO("txtCpnName", True, m_strCita, "U_CpnName", FormularioSBO)
        EditTextFechaDoc = New EditTextSBO("txtFhaDoc", True, m_strCita, "CreateDate", FormularioSBO)
        EditTextNumPlaca = New EditTextSBO("txtPlaca", True, m_strCita, "U_Num_Placa", FormularioSBO)
        EditTextFhaServicio = New EditTextSBO("txtFhaServ", True, m_strCita, "U_FhaServ", FormularioSBO)
        EditTextHoraServicio = New EditTextSBO("txtHoraSer", True, m_strCita, "U_HoraServ", FormularioSBO)

        EditTextFechaCitaFin = New EditTextSBO("txtFhaFin", True, m_strCita, "U_FhaCita_Fin", FormularioSBO)
        EditTextHoraCitaFin = New EditTextSBO("txtHoraFin", True, m_strCita, "U_HoraCita_Fin", FormularioSBO)
        EditTextFechaServFin = New EditTextSBO("txtFSFin", True, m_strCita, "U_FhaServ_Fin", FormularioSBO)
        EditTextHoraServFin = New EditTextSBO("txtHSFin", True, m_strCita, "U_HoraServ_Fin", FormularioSBO)
        EditTextIdVehiculo = New EditTextSBO("txtIdVehi", True, m_strCita, "U_CodVehi", FormularioSBO)

        EditTextTipoCambio = New EditTextSBO("txtTipoC", True, m_strCita, "U_TipoC", FormularioSBO)
        EditTextTotalLineas = New EditTextSBO("txtTotLin", True, m_strCita, "U_Total_Lin", FormularioSBO)
        EditTextTotalImpuesto = New EditTextSBO("txtImp", True, m_strCita, "U_Total_Imp", FormularioSBO)
        EditTextTotalDocumento = New EditTextSBO("txtTotDoc", True, m_strCita, "U_Total_Doc", FormularioSBO)

        EditCboEstado = New ComboBoxSBO("cboEstado", FormularioSBO, True, m_strCita, "U_Estado")
        EditCboSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, m_strCita, "U_Cod_Sucursal")
        EditCboAgenda = New ComboBoxSBO("cboAgenda", FormularioSBO, True, m_strCita, "U_Cod_Agenda")
        EditCboRazon = New ComboBoxSBO("cboRazon", FormularioSBO, True, m_strCita, "U_Cod_Razon")
        EditCboMoneda = New ComboBoxSBO("cboMoneda", FormularioSBO, True, m_strCita, "U_Moneda")
        EditCboTecnico = New ComboBoxSBO("cboTecnico", FormularioSBO, True, m_strCita, "U_Cod_Tecnico")
        EditCboAsesor = New ComboBoxSBO("cboAsesor", FormularioSBO, True, m_strCita, "U_Cod_Asesor")

        EditCbxArticulos = New CheckBoxSBO("cbx_Artic", True, m_strCita, "U_UsaArt", FormularioSBO)

        'enlaza los edittext y las columnas
        EditTextMarca.AsignaBinding()
        EditTextEstilo.AsignaBinding()
        EditTextModelo.AsignaBinding()
        EditTextCombustible.AsignaBinding()
        ' EditTextPlaca.AsignaBinding()
        EditTextMotor.AsignaBinding()
        EditTextAno.AsignaBinding()
        EditTextTiempo.AsignaBinding()
        EditTextServicios.AsignaBinding()
        EditCbxTiempo.AsignaBinding()

        EditTextUnidad.AsignaBinding()
        EditTextNumCita.AsignaBinding()
        EditTextCotizacion.AsignaBinding()
        EditTextFecha.AsignaBinding()
        EditTextHora.AsignaBinding()
        EditTextObservaciones.AsignaBinding()
        EditTextCardCode.AsignaBinding()
        EditTextCardName.AsignaBinding()
        EditTextCardCodeCliOT.AsignaBinding()
        EditTextCardNameCliOT.AsignaBinding()
        EditTextNomAsesor.AsignaBinding()
        EditTextNumSerie.AsignaBinding()
        EditTextCreado.AsignaBinding()
        EditTextDocEntry.AsignaBinding()
        EditTextCampaña.AsignaBinding()
        EditTextCampañaNombre.AsignaBinding()
        EditTextTipoCambio.AsignaBinding()
        EditTextTotalLineas.AsignaBinding()
        EditTextTotalImpuesto.AsignaBinding()
        EditTextTotalDocumento.AsignaBinding()
        EditTextFechaDoc.AsignaBinding()
        EditTextFechaCitaFin.AsignaBinding()
        EditTextHoraCitaFin.AsignaBinding()
        EditTextNumPlaca.AsignaBinding()
        EditTextFhaServicio.AsignaBinding()
        EditTextHoraServicio.AsignaBinding()
        EditTextFechaServFin.AsignaBinding()
        EditTextHoraServFin.AsignaBinding()
        EditTextIdVehiculo.AsignaBinding()


        EditCboAgenda.AsignaBinding()
        EditCboSucursal.AsignaBinding()
        EditCboRazon.AsignaBinding()
        EditCboMoneda.AsignaBinding()
        EditCboTecnico.AsignaBinding()
        EditCboAsesor.AsignaBinding()

        EditCbxArticulos.AsignaBinding()
        FormularioSBO.PaneLevel = 1
        FormularioSBO.Freeze(False)



    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try
            FormularioSBO.Freeze(True)

            EditTextFechaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))
            EditTextCreado.AsignaValorDataSource(DMS_Connector.Company.ApplicationSBO.Company.UserName)
            CargarFormulario()
            m_oVehiculo = New VehiculosCls(m_oCompany, ApplicationSBO)

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub addTextComentarios()
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oEdit As SAPbouiCOM.EditText
            Dim intTop As Integer
            oitem = FormularioSBO.Items.Item("91")
            intTop = oitem.Top

            oitem = FormularioSBO.Items.Add("txtObserv", SAPbouiCOM.BoFormItemTypes.it_EXTEDIT)
            oitem.Top = intTop + 20
            oitem.Left = 110
            oitem.Width = 589
            oitem.Height = 28
            oitem.FromPane = 1
            oitem.ToPane = 1

            oEdit = oitem.Specific
            oEdit.ScrollBars = SAPbouiCOM.BoScrollBars.sb_Vertical
            oEdit.DataBind.SetBound(True, m_strCita, "U_Observ")

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Public Sub CargarFormulario()

        Try
            _formularioSbo.Freeze(True)

            md_Datos = FormularioSBO.DataSources.DataTables.Add("dtDatos")
            md_Cita = FormularioSBO.DataSources.DataTables.Add("DatosCita")
            md_Agenda = FormularioSBO.DataSources.DataTables.Add("DatosAgenda")
            md_Campana = FormularioSBO.DataSources.DataTables.Add("DatosCampana")
            md_Configuracion = FormularioSBO.DataSources.DataTables.Add("DatosConfig")
            md_Suspension = FormularioSBO.DataSources.DataTables.Add("DatosSuspension")
            md_Paquetes = FormularioSBO.DataSources.DataTables.Add("datosPaquete")
            md_Local = FormularioSBO.DataSources.DataTables.Add("dtLocal")
            md_Local2 = FormularioSBO.DataSources.DataTables.Add("dtLocal2")
            md_ArtPadre = FormularioSBO.DataSources.DataTables.Add("dtPadre")

            dtListaServicios = FormularioSBO.DataSources.DataTables.Add("listServicios")

            dtListaServicios.Columns.Add("codigo", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("descripcion", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("cantidad", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("moneda", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("precio", BoFieldsType.ft_Price, 100)
            dtListaServicios.Columns.Add("tipo", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("duracion", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("linea", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("impuesto", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("total", BoFieldsType.ft_Price, 100)
            dtListaServicios.Columns.Add("hijo", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("padre", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("paquete", BoFieldsType.ft_AlphaNumeric, 100)
            dtListaServicios.Columns.Add("barras", BoFieldsType.ft_AlphaNumeric, 100)

            MatrizServicios = New MatrizServicios("mtxArtic", FormularioSBO, "listServicios")

            MatrizServicios.CreaColumnas()
            MatrizServicios.LigaColumnas()

            If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                AddChooseFromList(FormularioSBO, "5", "CFL_Imp")
            Else
                AddChooseFromList(FormularioSBO, "128", "CFL_Imp")
            End If

            AddChooseFromList(FormularioSBO, "37", "CFL_Cur")
            AddChooseFromList(FormularioSBO, "4", "CFL_Itm")
            AddChooseFromList(FormularioSBO, "4", "CFL_Itms")

            AsignaCFLColumn("mtxArtic", "Col_Imp", "CFL_Imp", "Code")
            AsignaCFLColumn("mtxArtic", "Col_Mon", "CFL_Cur", "CurrCode")
            AsignaCFLColumn("mtxArtic", "Col_Code", "CFL_Itm", "ItemCode")
            AsignaCFLColumn("mtxArtic", "Col_Barra", "CFL_Itms", "CodeBars")

            _formularioSbo.Items.Item(EditCboAsesor.UniqueId).Enabled = False
            _formularioSbo.Items.Item(EditTextFhaServicio.UniqueId).Enabled = False
            _formularioSbo.Items.Item(EditTextHoraServicio.UniqueId).Enabled = False


            CargarCombos()
            CargarMonedaLocal()
            AgregaLineaVacia()
            CargaConfiguracion()

            versionsap = m_oCompany.Version
            If versionsap < 900000 Then
                m_blnVersion9 = False
            End If

            _formularioSbo.Freeze(False)
            SeleccionarSucursalUsuario(_formularioSbo)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Sub ManejadorEventoLoad(ByVal FormUID As String, _
                        ByRef pVal As SAPbouiCOM.ItemEvent, _
                        ByRef BubbleEvent As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim l_intTopActual As Integer
        Dim l_intLeftActual As Integer

        Try
            oform = ApplicationSBO.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            l_intTopActual = 0
            l_intLeftActual = 0

            If pVal.BeforeAction Then

                Call AgregaButtonPic(oform, "btLkUnid", l_intLeftActual + 90, l_intTopActual + 119, 1, 1, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\Flecha.BMP", "")

            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function AgregaButtonPic(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal ButtonType As SAPbouiCOM.BoButtonTypes, _
                                   ByVal PathImagen As String, _
                                   ByVal UDO As String) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oButton = oitem.Specific
            oButton.Type = ButtonType
            oitem.Width = 20
            oitem.Height = 20
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oButton.Image = PathImagen

            If UDO <> "" Then
                oButton.ChooseFromListUID = UDO
            End If

            Return oitem
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

    Public Sub CargarCombos()
        Try
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM [@SCGD_RAZONCITA] with(nolock) ORDER BY Name ASC", EditCboRazon.UniqueId)
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, U_Descripcion FROM [@SCGD_CITA_ESTADOS] with(nolock) ORDER BY Code ASC", EditCboEstado.UniqueId)
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT CurrCode, CurrName from OCRN with(nolock)", EditCboMoneda.UniqueId)

            Call CargarValoresValidosCombos("SELECT Code, Name FROM [@SCGD_SUCURSALES] with(nolock) ORDER BY name", EditCboSucursal.UniqueId, True)
            Call CargarValoresValidosCombos("SELECT DocEntry, U_Agenda FROM [@SCGD_AGENDA] with (nolock) where U_EstadoLogico = 'Y'", EditCboAgenda.UniqueId, True)
            Call CargarValoresValidosCombos("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_TipoEmp = 'T' ", EditCboTecnico.UniqueId, True)
            Call CargarValoresValidosCombos("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_TipoEmp = 'A' ", EditCboAsesor.UniqueId, True)

            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM ""@SCGD_MOTIVOCANC"" WITH(nolock)", "cboMCanc")
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM ""@SCGD_MOVILIDAD"" WITH(nolock)", "cboMovi")
            Call CargarValidValuesEnCombos(FormularioSBO, "SELECT Code, Name FROM ""@SCGD_FCONTACTO"" WITH(nolock)", "cboCntc")

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Busca el usuario en el ComboBox sucursal y si la sucursal existe, la selecciona
    ''' </summary>
    ''' <param name="oComboBox">Objeto ComboBox de SAP</param>
    ''' <remarks></remarks>
    Private Sub SeleccionarSucursalUsuario(ByRef oFormulario As SAPbouiCOM.Form)
        Dim strSucursal As String = String.Empty
        Dim oComboBox As SAPbouiCOM.ComboBox
        Try
            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count > 0 Then
                strSucursal = ObtenerSucursalUsuario()
                If Not String.IsNullOrEmpty(strSucursal) Then
                    For Each oValidValue As SAPbouiCOM.ValidValue In oComboBox.ValidValues
                        If oValidValue.Value = strSucursal Then
                            oComboBox.Select(strSucursal, SAPbouiCOM.BoSearchKey.psk_ByValue)
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve la sucursal del usuario conectado a SAP
    ''' </summary>
    ''' <returns>Código de la sucursal del usuario conectado</returns>
    ''' <remarks></remarks>
    Private Function ObtenerSucursalUsuario() As String
        Dim oUser As SAPbobsCOM.Users
        Dim strSucursal As String = String.Empty
        Dim strInternalKey As String = String.Empty
        Try
            strInternalKey = DMS_Connector.Company.CompanySBO.UserSignature
            oUser = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUsers)
            oUser.GetByKey(strInternalKey)
            strSucursal = oUser.Branch
            Return strSucursal
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strSucursal
        End Try
    End Function

    Public Sub CargaConfiguracion()
        Try
            g_strUsaConfEstiMode = DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV.Trim()
            g_strFiltroEstiMod = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef oTmpForm As SAPbouiCOM.Form)
        Try
            Dim l_strCodSucursal As String
            Dim l_strCotizacion As String
            Dim l_strCodVehiculo As String
            Dim l_strDocEntry As String
            Dim l_strCardCode As String
            Dim strConsultClie As String

            oTmpForm.Freeze(True)

            l_strDocEntry = EditTextDocEntry.ObtieneValorDataSource()
            l_strCodSucursal = EditCboSucursal.ObtieneValorDataSource()
            l_strCardCode = EditTextCardCode.ObtieneValorDataSource()
            l_strCodVehiculo = EditTextUnidad.ObtieneValorDataSource()
            l_strCotizacion = EditTextCotizacion.ObtieneValorDataSource()

            Call LimpiarCampos()
            dtListaServicios.Rows.Clear()

            If Not String.IsNullOrEmpty(l_strCodVehiculo) Then
                ObtenerDatosVehiculo(l_strCodVehiculo)
            End If

            If Not String.IsNullOrEmpty(l_strCotizacion) Then
                ObtenerLineasCotizacion(l_strCotizacion)
                MarcarItemsTipoPaquete()
            End If

            ManejaEstadoTextTipoCambio()
            AgregaLineaVacia()
            ObtenerInformacionDeTecnico()
            CalculaTiempoDeServicio()
            CalculaTotales()
            ActualizaValoresCombos()

            m_strCardCode = EditTextCardCode.ObtieneValorDataSource()
            m_strCodUnid = EditTextUnidad.ObtieneValorDataSource()
            m_strHoraCita = EditTextHora.ObtieneValorDataSource()
            m_strFechaCita = EditTextFecha.ObtieneValorDataSource()

            With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                If .U_UseLisPreCli.Trim().Equals("Y") Then
                    strConsultClie = "SELECT ListNum FROM OCRD WHERE CardCode = '{0}' "
                    m_strListaPreciosCli = Utilitarios.EjecutarConsulta(String.Format(strConsultClie, EditTextCardCode.ObtieneValorDataSource()),m_oCompany.CompanyDB,m_oCompany.Server)
                Else
                    m_strListaPreciosCli = .U_CodLisPre.Trim()
                End If
            End With

            m_strCodCitasCancel = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(EditCboSucursal.ObtieneValorDataSource())).U_CodCitaCancel.Trim

            m_strNombreBDTaller = ObtenerBaseDatosTaller(l_strCodSucursal)

            If EditCboEstado.ObtieneValorDataSource() = m_strCodCitasCancel Then
                FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
            Else
                FormularioSBO.Mode = BoFormMode.fm_OK_MODE
            End If

            ManejaEstadoMotivoCancelacion()

            oTmpForm.Freeze(False)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Public Sub ActualizaValoresCombos()
        Try

            Dim l_strSucur As String
            Dim l_strAgenda As String
            Dim l_strSQL As String
            Dim m_bIngresaAg As Boolean = False
            l_strSQL = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_Equipo = '{0}' and HE.U_SCGD_TipoEmp = 'T' "
            Dim l_strSQLAg As String = "SELECT DocNum, U_Agenda, U_CodTecnico, U_NameTecnico FROM [@SCGD_AGENDA] with (nolock) where U_Cod_Sucursal = '{0}' AND U_EstadoLogico = 'Y'"

            l_strSucur = EditCboSucursal.ObtieneValorDataSource()
            l_strAgenda = EditCboAgenda.ObtieneValorDataSource()
            m_strNumeroGrupo = ObtenerNumeroDeEquipo_PorAgenda(l_strAgenda)

            m_strUsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(EditCboSucursal.ObtieneValorDataSource())).U_GrpTrabajo.Trim

            If Not String.IsNullOrEmpty(l_strAgenda) Then

                CargarValoresValidosCombos(String.Format(l_strSQLAg, l_strSucur), EditCboAgenda.UniqueId)
                CargarValoresValidosCombos(String.Format(l_strSQL, m_strNumeroGrupo), EditCboTecnico.UniqueId, True)
                m_bIngresaAg = True

            End If

            If m_strUsaGruposTrabajo.Equals("Y") Then
                m_strNumeroGrupo = ObtenerNumeroDeEquipo_PorAgenda(l_strAgenda)
                If Not m_bIngresaAg Then
                    Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_T_Fase is not null and branch = '{0}' and U_SCGD_TipoEmp = 'T' ", l_strSucur), EditCboTecnico.UniqueId, True)
                    If (DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES) Then
                        Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where   (branch = '{0}' or  BPLId = '{0}') and U_SCGD_TipoEmp = 'A' order by HE.lastName", l_strSucur), EditCboAsesor.UniqueId, True)
                    Else
                        Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where   branch = '{0}' and U_SCGD_TipoEmp = 'A' order by HE.lastName", l_strSucur), EditCboAsesor.UniqueId, True)
                    End If
                End If
            ElseIf m_strUsaGruposTrabajo.Equals("N") Then
                If (DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES) Then
                    Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_T_Fase is not null and  (branch = '{0}' or  BPLId = '{0}') and U_SCGD_TipoEmp = 'T' ", l_strSucur), EditCboTecnico.UniqueId, True)
                    Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where   (branch = '{0}' or  BPLId = '{0}') and U_SCGD_TipoEmp = 'A' order by HE.lastName", l_strSucur), EditCboAsesor.UniqueId, True)
                Else
                    Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_T_Fase is not null and branch = '{0}' and U_SCGD_TipoEmp = 'T' ", l_strSucur), EditCboTecnico.UniqueId, True)
                    Call CargarValoresValidosCombos(String.Format("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where   branch = '{0}' and U_SCGD_TipoEmp = 'A' order by HE.lastName", l_strSucur), EditCboAsesor.UniqueId, True)
                End If
            ElseIf m_strUsaGruposTrabajo.Equals("-1") Then

                Call CargarValoresValidosCombos("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_T_Fase is not null  and U_SCGD_TipoEmp = 'T' ", EditCboTecnico.UniqueId, True)
                Call CargarValoresValidosCombos("Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock)  where  U_SCGD_TipoEmp = 'A' order by HE.lastName", EditCboAsesor.UniqueId, True)

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Protected Friend Sub HabilitarCombos(ByRef oForm As SAPbouiCOM.Form, _
                                        ByVal strIDItem As String, ByVal blnEstado As Boolean)
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Try
            If oForm IsNot Nothing Then
                oItem = oForm.Items.Item(strIDItem)
                oItem.Enabled = blnEstado
                cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Sub

    Private Sub HandlerTimer(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs)

        If m_blnFlagTimer Then

            Try
                _applicationSbo.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)

            Catch ex As Exception
                DMS_Connector.Helpers.ManejoErrores(ex)
            End Try

        End If
    End Sub

    Public Sub IniciaTimer()

        m_blnFlagTimer = True

        m_oTimer = New System.Timers.Timer()
        AddHandler m_oTimer.Elapsed, New ElapsedEventHandler(AddressOf HandlerTimer)
        m_oTimer.Interval = 30000
        m_oTimer.Enabled = True

    End Sub

    Private Sub FinalizaTimer()
        m_blnFlagTimer = False

        m_oTimer.Enabled = False
        m_oTimer.Stop()
        m_oTimer.Dispose()

    End Sub

    Public Sub ManejadorEventoItemPress(ByVal formUID As String,
                                        ByRef pVal As SAPbouiCOM.ItemEvent,
                                        ByRef bubbleEvent As Boolean,
                                        ByRef oDetVehiculos As VehiculosCls,
                                        ByRef p_oFormularioAdicionalesCitasArt As BuscadorArticulosCitas)

        Dim l_strUsaArticulo As String
        Dim l_strCodVehiculo As String
        Dim l_strNumCotizacion As String
        Dim l_strCodAgenda As String
        Dim descripcionAgenda As String
        Dim l_strCodSucur As String
        Dim sboCombo As ComboBox
        Dim l_strCita As String
        Dim l_intOcupacionAsesor As Integer
        Dim l_intOCupacionTecnico As Integer
        Dim l_intIntevaloAgn As Integer
        Dim oForm As SAPbouiCOM.Form

        Try
            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID

                    Case "btnAgenda"
                        Dim fecha As Date
                        fecha = DateTime.Now
                        Utilitarios.RetornaFechaFormatoRegional(fecha.ToString("yyyy-MM-dd"))

                        Dim ptr As IntPtr = GetForegroundWindow()
                        Dim wrapper As New WindowWrapper(ptr)

                        sboCombo = DirectCast(FormularioSBO.Items.Item(m_strCboAgenda).Specific, ComboBox)
                        descripcionAgenda = sboCombo.Selected.Description
                        l_strCodAgenda = sboCombo.Selected.Value

                        sboCombo = DirectCast(FormularioSBO.Items.Item(m_strCboSucursal).Specific, ComboBox)
                        l_strCodSucur = sboCombo.Selected.Value

                        l_strCodSucur = EditCboSucursal.ObtieneValorDataSource()
                        l_intIntevaloAgn = DevuelveValorItemAgenda("U_IntervaloCitas", EditCboAgenda.ObtieneValorDataSource)
                        l_intOcupacionAsesor = ObtenerCantidadEspaciosAgenda(l_intIntevaloAgn)
                        l_intOCupacionTecnico = ObtenerCantidadEspaciosAgenda(EditTextTiempo.ObtieneValorUserDataSource)

                        m_strTipoAgenda = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(EditCboSucursal.ObtieneValorDataSource())).U_AgendaColor.ToString()

                        If m_strUsaGruposTrabajo.Equals("Y") Then

                            _frmAcupacionAgendas = New frmListaCitas(Date.Today, l_strCodSucur, l_strCodAgenda, m_strNumeroGrupo, True, TipoDeAgenda.Grupos, m_blnVersion9, l_intOcupacionAsesor, l_intOCupacionTecnico, m_strTipoAgenda, CompanySBO, ApplicationSBO)
                            _frmAcupacionAgendas.ShowInTaskbar = False

                            If m_blnVersion9 Then
                                IniciaTimer()
                                _frmAcupacionAgendas.ShowDialog(wrapper)
                                FinalizaTimer()
                            Else
                                _frmAcupacionAgendas.ShowDialog(wrapper)
                            End If

                        Else
                            If m_strTipoAgenda <> "Y" Then
                                _frmAgendaCitas = New frmCalendario(True, Date.Parse(fecha), descripcionAgenda, l_strCodAgenda, l_strCodSucur, m_strCodCitasCancel, m_blnVersion9, True, m_oCompany, ApplicationSBO)
                                _frmAgendaCitas.ShowInTaskbar = False
                            Else
                                _frmAgendaCitasColor = New frmCalendarioColor(True, Date.Parse(fecha), descripcionAgenda, l_strCodAgenda, l_strCodSucur, m_strCodCitasCancel, m_blnVersion9, True, m_oCompany, ApplicationSBO)
                                _frmAgendaCitasColor.ShowInTaskbar = False
                            End If



                            If m_blnVersion9 Then
                                IniciaTimer()
                                If m_strTipoAgenda <> "Y" Then
                                    _frmAgendaCitas.ShowDialog(wrapper)
                                Else
                                    _frmAgendaCitasColor.ShowDialog(wrapper)
                                End If
                                FinalizaTimer()
                            Else
                                If m_strTipoAgenda <> "Y" Then
                                    _frmAgendaCitas.ShowDialog(wrapper)
                                Else
                                    _frmAgendaCitasColor.ShowDialog(wrapper)
                                End If
                            End If

                        End If
                    Case "btnLess"
                        DesasignarValorMatriz(formUID, pVal, bubbleEvent)
                        CalculaTotales()
                        CalculaTiempoDeServicio()
                        CalculaFechaFinCita()


                        If FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
                        End If
                    Case "btnAdd"

                        '' AgregaLineaVacia()
                        AbreVentanaBuscadorCita(p_oFormularioAdicionalesCitasArt)
                    Case EditCbxArticulos.UniqueId
                        l_strUsaArticulo = EditCbxArticulos.ObtieneValorDataSource()
                        If l_strUsaArticulo = "Y" Then
                            FormularioSBO.Items.Item("btnAdd").Enabled = False
                            FormularioSBO.Items.Item("btnLess").Enabled = False
                        ElseIf l_strUsaArticulo = "N" Then
                            FormularioSBO.Items.Item("btnAdd").Enabled = True
                            FormularioSBO.Items.Item("btnLess").Enabled = True
                        End If

                    Case "mtxArtic"
                        intFila = pVal.Row

                    Case "1"
                        If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                            l_strCodVehiculo = EditTextUnidad.ObtieneValorDataSource()
                            l_strNumCotizacion = EditTextCotizacion.ObtieneValorDataSource()

                            If EditCboEstado.ObtieneValorDataSource() = m_strCodCitasCancel Then
                                FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
                            Else
                                FormularioSBO.Mode = BoFormMode.fm_OK_MODE
                            End If

                        ElseIf pVal.FormMode = BoFormMode.fm_ADD_MODE Then
                            'If bubbleEvent Then
                            '    If m_strUsaGruposTrabajo.Equals("Y") Then
                            '        l_strCita = CreaCotizacion(bubbleEvent)
                            '        If Not String.IsNullOrEmpty(l_strCita) Then
                            '            ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaCreada & " * " & l_strCita, 1, "OK")


                            '        End If
                            '    Else
                            '        l_strCita = CreaCotizacion(bubbleEvent)

                            '        If Not String.IsNullOrEmpty(l_strCita) Then
                            '            ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaCreada & " - " & l_strCita, 1, "OK")



                            '            LimpiarCampos()

                            '        End If
                            '    End If
                            'End If



                            dtListaServicios.Rows.Clear()

                            FormularioSBO.Items.Item("btnAdd").Enabled = True
                            FormularioSBO.Items.Item("btnLess").Enabled = True
                            FormularioSBO.Items.Item(EditCbxArticulos.UniqueId).Enabled = True
                            EditCbxArticulos.AsignaValorDataSource("N")

                            CargarMonedaLocal()
                            ManejaEstadoTextTipoCambio()
                            AgregaLineaVacia()
                            EditTextFechaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

                            If bubbleEvent Then
                                FormularioSBO.Mode = BoFormMode.fm_ADD_MODE
                            End If
                            FormularioSBO.Items.Item("tabCtrl").Click()
                            SeleccionarSucursalUsuario(FormularioSBO)
                        End If
                    Case "btLkUnid"
                        m_strCardCode = EditTextCardCode.ObtieneValorDataSource()
                        m_strCodUnid = EditTextUnidad.ObtieneValorDataSource()
                        m_strIDVehi = Utilitarios.EjecutarConsulta("SELECT Code From [@SCGD_VEHICULO] with (nolock) WHERE U_Cod_Unid = '" & m_strCodUnid & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                        m_oVehiculo = oDetVehiculos

                        If Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then

                            VehiculosCls.blnDesdeCita = True
                            VehiculosCls.blnDesdeCotizacion = False

                            Call m_oVehiculo.DibujarFormularioDetalleInformacionVehiculo(m_strCardCode, _
                                                                                        m_strIDVehi, _
                                                                                        True, _
                                                                                        m_TypeVehiculo, _
                                                                                        m_TypeCountVehiculo, False, False, VehiculosCls.ModoFormulario.scgTaller)
                        End If
                    Case EditCbxTiempo.UniqueId
                        ManejaFomatoTiempo()
                    Case "tabCtrl"
                        ManejaEstadoTextTipoCambio()
                End Select
            ElseIf pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    'manejo de eliminacion de lineas de la matriz
                    Case "btnLess"
                        If intFila = -1 Then
                            bubbleEvent = False
                        End If
                    Case "1"

                        oForm = ApplicationSBO.Forms.Item(formUID)
                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                            Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                            If usaInterFazFord Then
                                Dim socioNegTip = Utilitarios.ValidaIFTipoSN(m_oCompany, EditTextCardCode.ObtieneValorDataSource())

                                If Not socioNegTip Then
                                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoSN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    bubbleEvent = False
                                    Exit Sub
                                End If
                            End If
                        End If

                        If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If ValidarPeriodoContable(bubbleEvent) Then
                                If ValidarDatos(formUID, pVal, bubbleEvent) Then
                                    If m_strUsaGruposTrabajo.Equals("Y") Then
                                        If Not ValidarDatosServicio(formUID, pVal, bubbleEvent) Then
                                            If bubbleEvent Then
                                                l_strCita = CreaCotizacion(bubbleEvent)
                                                If Not String.IsNullOrEmpty(l_strCita) Then
                                                    ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaCreada & " * " & l_strCita, 1, "OK")


                                                End If
                                            End If
                                        End If
                                    Else
                                        l_strCita = CreaCotizacion(bubbleEvent)

                                        If Not String.IsNullOrEmpty(l_strCita) Then
                                            ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaCreada & " - " & l_strCita, 1, "OK")



                                            LimpiarCampos()

                                        End If
                                    End If

                                End If
                            End If

                        ElseIf pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                            If ValidarPeriodoContable(bubbleEvent) Then
                                If ValidarDatos(formUID, pVal, bubbleEvent) Then
                                    ActualizaCotizacion(bubbleEvent)
                                End If
                            End If

                        ElseIf pVal.FormMode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                            FormularioSBO.Items.Item(EditCbxArticulos.UniqueId).Enabled = False
                        End If

                    Case "btnAgenda"
                        Dim l_strSucursal As String
                        Dim l_strAgenda As String

                        l_strSucursal = EditCboSucursal.ObtieneValorDataSource
                        l_strAgenda = EditCboAgenda.ObtieneValorDataSource


                        If m_strUsaGruposTrabajo.Equals("Y") Then
                            If String.IsNullOrEmpty(l_strSucursal) Then
                                bubbleEvent = False
                                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinSucursal, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            Else
                                m_strFechaCita = EditTextFecha.ObtieneValorDataSource()
                                m_strHoraCita = EditTextHora.ObtieneValorDataSource()
                            End If
                        Else
                            If String.IsNullOrEmpty(l_strSucursal) Then
                                bubbleEvent = False
                                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinSucursal, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            ElseIf String.IsNullOrEmpty(l_strAgenda) Then
                                bubbleEvent = False
                                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAgenda, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                            Else
                                m_strFechaCita = EditTextFecha.ObtieneValorDataSource()
                                m_strHoraCita = EditTextHora.ObtieneValorDataSource()
                            End If
                        End If
                    Case "tabCtrl"
                        ManejaEstadoTextTipoCambio()
                    Case "btnOcupa"
                        ConstructorDisponibilidadEmpleados.CrearInstanciaFormulario()
                End Select
            End If
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            Throw ex
        End Try
    End Sub

    Public Sub ManejaFomatoTiempo()
        Try

            Dim l_strValorCheck As String
            Dim l_numDuracion As Decimal = 0
            Dim l_numResult As Decimal = 0

            Dim oItem As SAPbouiCOM.Item
            Dim oLabel As SAPbouiCOM.StaticText

            oItem = _formularioSbo.Items.Item("89")
            oLabel = CType(oItem.Specific, SAPbouiCOM.StaticText)

            l_strValorCheck = EditCbxTiempo.ObtieneValorUserDataSource()

            If String.IsNullOrEmpty(EditTextTiempo.ObtieneValorUserDataSource()) Then
                If l_strValorCheck.Equals("Y") Then
                    oLabel.Caption = My.Resources.Resource.Horas
                Else
                    oLabel.Caption = My.Resources.Resource.Minutos
                End If
                l_numResult = 0
            Else
                l_numDuracion = EditTextTiempo.ObtieneValorUserDataSource()

                If l_strValorCheck.Equals("Y") Then
                    l_numResult = l_numDuracion / 60
                    oLabel.Caption = My.Resources.Resource.Horas
                Else
                    l_numResult = l_numDuracion * 60
                    oLabel.Caption = My.Resources.Resource.Minutos
                End If
            End If

            EditTextTiempo.AsignaValorUserDataSource(Decimal.Round(l_numResult, 2))

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ObtenerInformacionDeTecnico()
        Try
            Dim l_strCodTecnico As String
            Dim l_strSQL As String
            l_strSQL = " Select empID, U_SCGD_TiempServ from OHEM with (nolock) where empID = '{0}'"
            l_strCodTecnico = EditCboTecnico.ObtieneValorDataSource()

            md_Datos = FormularioSBO.DataSources.DataTables.Item("dtDatos")
            md_Datos.Clear()
            md_Datos.ExecuteQuery(String.Format(l_strSQL, l_strCodTecnico))

            If Not String.IsNullOrEmpty(md_Datos.GetValue("empID", 0)) Then
                m_strTiempoServEmpleado = md_Datos.GetValue("U_SCGD_TiempServ", 0)
            Else
                m_strTiempoServEmpleado = String.Empty
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Public Sub ObtenerInformacionDeAgenda()

        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim l_strSucursal As String
        Dim l_strAgenda As String
        Dim l_strSQLAgenda As String
        Dim l_strCodRazon As String

        l_strSQLAgenda = "SELECT DocNum, U_Agenda, U_CodTecnico, U_NameTecnico, U_CodAsesor, U_NameAsesor , U_RazonCita FROM [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}' AND U_EstadoLogico = 'Y'"

        Try
            oItem = FormularioSBO.Items.Item(EditCboAgenda.UniqueId)
            cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
            l_strAgenda = cboCombo.Selected.Value

            If m_strUsaGruposTrabajo.Equals("N") OrElse String.IsNullOrEmpty(m_strUsaGruposTrabajo) Then

                l_strSQLAgenda = String.Format(l_strSQLAgenda, l_strAgenda)
                md_Agenda = FormularioSBO.DataSources.DataTables.Item("DatosAgenda")
                md_Agenda.Clear()
                md_Agenda.ExecuteQuery(l_strSQLAgenda)

                If md_Agenda.Rows.Count <> 0 Then
                    EditCboAsesor.AsignaValorDataSource(md_Agenda.GetValue("U_CodAsesor", 0))
                    EditTextNomAsesor.AsignaValorDataSource(md_Agenda.GetValue("U_NameAsesor", 0))

                    l_strCodRazon = md_Agenda.GetValue("U_RazonCita", 0)
                    If Not String.IsNullOrEmpty(l_strCodRazon) Then

                        If String.IsNullOrEmpty(EditTextCotizacion.ObtieneValorDataSource) Then
                            EditCboRazon.AsignaValorDataSource(l_strCodRazon)
                        End If

                    End If
                End If

            ElseIf m_strUsaGruposTrabajo.Equals("Y") Then

                Dim l_strSQL As String
                l_strSQL = " Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where U_SCGD_TipoEmp = 'T' and U_SCGD_Equipo = '{0}' "

                If cboCombo.Active Then

                    l_strSucursal = EditCboSucursal.ObtieneValorDataSource()
                    l_strAgenda = EditCboAgenda.ObtieneValorDataSource()
                    m_strNumeroGrupo = ObtenerNumeroDeEquipo_PorAgenda(l_strAgenda)


                    If Not String.IsNullOrEmpty(l_strAgenda) Then
                        l_strSQL = String.Format(l_strSQL, m_strNumeroGrupo)

                        Call HabilitarCombos(FormularioSBO, EditCboTecnico.UniqueId, True)
                        CargarValoresValidosCombos(l_strSQL, EditCboTecnico.UniqueId, True)

                    End If

                End If

                If Not String.IsNullOrEmpty(l_strAgenda) Then

                    l_strSQLAgenda = String.Format(l_strSQLAgenda, l_strAgenda)
                    md_Agenda = FormularioSBO.DataSources.DataTables.Item("DatosAgenda")
                    md_Agenda.Clear()
                    md_Agenda.ExecuteQuery(l_strSQLAgenda)

                    If md_Agenda.Rows.Count <> 0 Then
                        EditCboAsesor.AsignaValorDataSource(md_Agenda.GetValue("U_CodAsesor", 0))
                        EditTextNomAsesor.AsignaValorDataSource(md_Agenda.GetValue("U_NameAsesor", 0))

                        l_strCodRazon = md_Agenda.GetValue("U_RazonCita", 0)
                        If Not String.IsNullOrEmpty(l_strCodRazon) Then

                            If String.IsNullOrEmpty(EditTextCotizacion.ObtieneValorDataSource) Then
                                EditCboRazon.AsignaValorDataSource(l_strCodRazon)
                            End If

                        End If
                    End If
                Else
                    If String.IsNullOrEmpty(l_strAgenda) Then
                        EditCboAsesor.AsignaValorDataSource(String.Empty)
                        EditTextNomAsesor.AsignaValorDataSource(String.Empty)
                        EditCboTecnico.AsignaValorDataSource(String.Empty)
                    End If
                End If
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
    Public Sub ManejadorEventoCombos(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean)
        Try
            Dim cboCombo As SAPbouiCOM.ComboBox
            Dim oItem As SAPbouiCOM.Item
            Dim l_strSucursal As String
            Dim l_strSQLAgendas As String
            Dim l_strSQLConfig As String


            l_strSQLAgendas = "SELECT DocNum, U_Agenda, U_CodTecnico, U_NameTecnico FROM [@SCGD_AGENDA] with (nolock) where U_Cod_Sucursal = '{0}' AND U_EstadoLogico = 'Y'"
            l_strSQLConfig = " SELECT U_CodCitaCancel ,U_CodCitaNueva ,U_Imp_Serv, U_Imp_Repuestos, U_Imp_Suminis, U_Imp_ServExt FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = '{0}'"

            If pVal.ActionSuccess Then

                Select Case pVal.ItemUID
                    Case EditCboSucursal.UniqueId
                        oItem = FormularioSBO.Items.Item(EditCboSucursal.UniqueId)
                        cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

                        Call LimpiarDatosSucursal()
                        Call ActualizaValoresCombos()

                        l_strSucursal = cboCombo.Selected.Value
                        m_strUsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(EditCboSucursal.ObtieneValorDataSource())).U_GrpTrabajo.Trim
                        m_strNombreBDTaller = ObtenerBaseDatosTaller(l_strSucursal)

                        Call HabilitarCombos(FormularioSBO, EditCboAgenda.UniqueId, True)
                        Call CargarValoresValidosCombos(String.Format(l_strSQLAgendas, l_strSucursal), EditCboAgenda.UniqueId, True)

                        'If cboCombo.Active Then



                        'End If

                        l_strSQLConfig = String.Format(l_strSQLConfig, l_strSucursal)

                        md_Configuracion = FormularioSBO.DataSources.DataTables.Item("DatosConfig")
                        md_Configuracion.Clear()
                        md_Configuracion.ExecuteQuery(l_strSQLConfig)

                        If md_Configuracion.Rows.Count <> 0 Then
                            If EditCboEstado.ObtieneValorDataSource() = "" AndAlso
                                FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then
                                EditCboEstado.AsignaValorDataSource(md_Configuracion.GetValue("U_CodCitaNueva", 0))
                                m_strCodCitasCancel = md_Configuracion.GetValue("U_CodCitaCancel", 0)
                            End If

                            m_strImpServicio = md_Configuracion.GetValue("U_Imp_Serv", 0)
                            m_strImpRepuesto = md_Configuracion.GetValue("U_Imp_Repuestos", 0)
                            m_strImpSuministro = md_Configuracion.GetValue("U_Imp_Suminis", 0)
                            m_strImpServExt = md_Configuracion.GetValue("U_Imp_ServExt", 0)

                        End If
                        _formularioSbo.Freeze(True)
                        If m_strUsaGruposTrabajo.Equals("Y") Then
                            _formularioSbo.Items.Item(EditCboAsesor.UniqueId).Enabled = False
                            _formularioSbo.Items.Item(EditTextFhaServicio.UniqueId).Enabled = True
                            _formularioSbo.Items.Item(EditTextHoraServicio.UniqueId).Enabled = True
                        ElseIf m_strUsaGruposTrabajo.Equals("N") Then
                            _formularioSbo.Items.Item(EditCboAsesor.UniqueId).Enabled = True
                            _formularioSbo.Items.Item(EditTextFhaServicio.UniqueId).Enabled = False
                            _formularioSbo.Items.Item(EditTextHoraServicio.UniqueId).Enabled = False
                        End If
                        _formularioSbo.Freeze(False)
                    Case EditCboAgenda.UniqueId

                        LimpiarDatosAgenda()
                        ObtenerInformacionDeAgenda()

                    Case EditCboMoneda.UniqueId
                        m_strMonedaDestino = EditCboMoneda.ObtieneValorDataSource()

                        If ManejaTipoCambio(bubbleEvent) Then
                            ManejaEstadoTextTipoCambio()
                            ManejoCambioDeMoneda()
                        End If
                    Case EditCboTecnico.UniqueId

                        ObtenerInformacionDeTecnico()
                        CalculaFechaFinCita()
                        CalculaTiempoDeServicio()
                    Case "cboEstado"
                        ManejaEstadoMotivoCancelacion()
                End Select

            ElseIf pVal.BeforeAction Then


                Select Case pVal.ItemUID
                    Case EditCboMoneda.UniqueId
                        m_strMonedaOrigen = EditCboMoneda.ObtieneValorDataSource()
                    Case EditCboAgenda.UniqueId
                        l_strSucursal = EditCboSucursal.ObtieneValorDataSource

                        If String.IsNullOrEmpty(l_strSucursal) Then
                            ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorCitaSinSucursal)
                            bubbleEvent = False
                        End If

                End Select

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Sub

    Public Sub ManejadorEventosChooseFromList(ByVal formUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim l_strCodCliente As String = ""
        Dim l_strCardName As String
        Dim l_strCardCode As String

        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

            If oCFLEvento.BeforeAction = False Then

                Dim oDataTable As SAPbouiCOM.DataTable
                oDataTable = oCFLEvento.SelectedObjects

                If Not oCFLEvento.SelectedObjects Is Nothing Then

                    If Not oDataTable Is Nothing And
                        FormularioSBO.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                        If pVal.ItemUID = EditTextCardCode.UniqueId Then
                            AsignaValoresEditTextUICliente(formUID, pVal, oDataTable)
                        ElseIf pVal.ItemUID = EditTextCardCodeCliOT.UniqueId Then
                            AsignaValoresEditTextUIClienteNumeroOT(formUID, pVal, oDataTable)
                        ElseIf pVal.ItemUID = EditTextUnidad.UniqueId Then
                            AsignaValoresEditTextUnidad(formUID, pVal, oDataTable)

                        ElseIf pVal.ItemUID = EditTextNumPlaca.UniqueId Then
                            AsignaValoresEditTextUnidad(formUID, pVal, oDataTable)

                        ElseIf pVal.ItemUID = EditTextCardName.UniqueId Then
                            AsignaValoresEditTextUICliente(formUID, pVal, oDataTable)
                        ElseIf pVal.ItemUID = EditTextCardNameCliOT.UniqueId Then
                            AsignaValoresEditTextUIClienteNumeroOT(formUID, pVal, oDataTable)

                        ElseIf pVal.ItemUID = "btnAdd" Then


                        ElseIf pVal.ColUID = MatrizServicios.ColumnaCol_Imp.UniqueId Then
                            AsignaValoresColImpuestoVeh(formUID, pVal, oDataTable)
                            CalculaTotales()

                        ElseIf pVal.ColUID = MatrizServicios.ColumnaCol_Cur.UniqueId Then
                            AsignaValoresColMoneda(formUID, pVal, oDataTable)

                        ElseIf pVal.ColUID = MatrizServicios.ColumnaCol_Code.UniqueId Then
                            AsignaValoresMatriz(formUID, pVal, oDataTable)
                            CalculaFechaFinCita()
                            CalculaTotales()
                            CalculaTiempoDeServicio(True)
                        ElseIf pVal.ColUID = MatrizServicios.ColumnaCol_Barra.UniqueId Then
                            AsignaValoresMatriz(formUID, pVal, oDataTable)
                            CalculaFechaFinCita()
                            CalculaTotales()
                            CalculaTiempoDeServicio(True)
                        End If

                    End If

                End If


            ElseIf oCFLEvento.BeforeAction = True Then

                l_strCardName = EditTextCardName.ObtieneValorDataSource()
                l_strCardCode = EditTextCardCode.ObtieneValorDataSource()

                Select Case pVal.ItemUID

                    Case EditTextUnidad.UniqueId, EditTextNumPlaca.UniqueId

                        l_strCodCliente = EditTextCardCode.ObtieneValorDataSource()

                        If Not String.IsNullOrEmpty(l_strCodCliente) Then
                            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                            oCondition = oConditions.Add()
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "U_CardCode"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = l_strCodCliente
                            oCondition.BracketCloseNum = 1
                        Else
                            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                            oCondition = oConditions.Add()

                            If pVal.ItemUID = EditTextNumPlaca.UniqueId Then
                                oCondition.BracketOpenNum = 1
                                oCondition.Alias = "U_Num_Plac"
                                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                                oCondition.BracketCloseNum = 1

                            ElseIf pVal.ItemUID = EditTextUnidad.UniqueId Then
                                oCondition.BracketOpenNum = 1
                                oCondition.Alias = "U_Cod_Unid"
                                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                                oCondition.BracketCloseNum = 1
                            End If

                        End If
                        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 2
                        oCondition.Alias = "U_Activo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "Y"
                        oCondition.BracketCloseNum = 2

                        oCFL.SetConditions(oConditions)

                    Case EditTextNumPlaca.UniqueId


                        'ElseIf pVal.ItemUID = EditTextCodTecnico.UniqueId Then
                        '    oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        '    oCondition = oConditions.Add()
                        '    oCondition.BracketOpenNum = 1
                        '    oCondition.Alias = "U_SCGD_T_Fase"
                        '    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                        '    oCondition.BracketCloseNum = 1

                        '    oCFL.SetConditions(oConditions)

                    Case EditTextCardName.UniqueId
                        EditTextCardName.AsignaValorDataSource("")

                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add()
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "CardName"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_CONTAIN
                        oCondition.CondVal = l_strCardName
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                    Case EditTextCardCode.UniqueId

                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "CardType"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "C"
                        oCondition.BracketCloseNum = 1

                        oCondition.Relationship = BoConditionRelationship.cr_AND

                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "frozenFor"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = "Y"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)


                    Case Is = "mtxArtic"

                        Select Case pVal.ColUID

                            Case "Col_Code", "Col_Barra"
                                If String.IsNullOrEmpty(l_strCardCode) Then
                                    _applicationSbo.SetStatusBarMessage(My.Resources.Resource.DebeSeleccionarSN, BoMessageTime.bmt_Short, True)
                                    BubbleEvent = False
                                End If

                            Case "Col_Imp"
                                oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                                oCondition = oConditions.Add()
                                If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                                    oCondition.BracketOpenNum = 1
                                    oCondition.Alias = "Category"
                                    oCondition.CondVal = "O"
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                                    oCondition.BracketCloseNum = 1

                                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                                    oCondition = oConditions.Add
                                    oCondition.BracketOpenNum = 2
                                    oCondition.Alias = "Locked"
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                                    oCondition.CondVal = "N"
                                    oCondition.BracketCloseNum = 2
                                Else
                                    oCondition.BracketOpenNum = 1
                                    oCondition.Alias = "ValidForAR"
                                    oCondition.CondVal = "Y"
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                                    oCondition.BracketCloseNum = 1

                                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                                    oCondition = oConditions.Add
                                    oCondition.BracketOpenNum = 2
                                    oCondition.Alias = "Lock"
                                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                                    oCondition.CondVal = "N"
                                    oCondition.BracketCloseNum = 2
                                End If
                                oCFL.SetConditions(oConditions)
                        End Select

                End Select

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Sub

    Sub ApplicationSboOnItemEvent(ByVal FormUID As String,
                                  ByRef pVal As SAPbouiCOM.ItemEvent,
                                  ByRef BubbleEvent As Boolean,
                                   ByRef oDetVehiculos As VehiculosCls,
                                   ByRef p_oFormularioAdicionalesCitasArt As BuscadorArticulosCitas)
        Try
            If Not pVal.FormTypeEx = FormType Then Return

            If pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
                ManejadorEventoCombos(FormUID, pVal, BubbleEvent)
            ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then
                ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)
            ElseIf pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
                ManejadorEventoItemPress(FormUID, pVal, BubbleEvent, oDetVehiculos, p_oFormularioAdicionalesCitasArt)
            End If

        Catch ex As Exception

            BubbleEvent = False
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try

            If pval.MenuUID = "1281" OrElse pval.MenuUID = "1282" Then
                Select Case pval.MenuUID
                    Case "1282"                 'BOTON NUEVO
                        LimpiarCampos()

                        CargarMonedaLocal(True)
                        EditTextFechaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

                        dtListaServicios.Rows.Clear()
                        m_strNumeroGrupo = "-1"
                        AgregaLineaVacia()

                        EditTextCreado.AsignaValorDataSource(DMS_Connector.Company.ApplicationSBO.Company.UserName)
                        SeleccionarSucursalUsuario(_formularioSbo)

                    Case "1281"                 'BOTON BUSCAR
                        LimpiarCampos()
                        dtListaServicios.Rows.Clear()
                        _formularioSbo.Items.Item(EditTextNumSerie.UniqueId).Enabled = True
                        _formularioSbo.Items.Item(EditTextNumCita.UniqueId).Enabled = True
                End Select
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Public Sub ManejadorEventoValidate(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case MatrizServicios.ColumnaCol_Pri.UniqueId
                        EventoValidateColumnaPrecio(pVal)
                End Select
            ElseIf pVal.ActionSuccess Then

                If pVal.ItemUID = MatrizServicios.UniqueId Then
                    Select Case pVal.ColUID
                        Case MatrizServicios.ColumnaCol_Imp.UniqueId, MatrizServicios.ColumnaCol_Quan.UniqueId

                            CalculaTotales()
                            CalculaTiempoDeServicio()
                            CalculaFechaFinCita()

                        Case MatrizServicios.ColumnaCol_Pri.UniqueId
                            EventoValidateColumnaPrecio(pVal)

                    End Select

                ElseIf pVal.ItemUID = EditTextHora.UniqueId OrElse
                    pVal.ItemUID = EditTextFecha.UniqueId OrElse
                    pVal.ItemUID = EditTextFhaServicio.UniqueId OrElse
                    pVal.ItemUID = EditTextHoraServicio.UniqueId Then

                    CalculaFechaFinCita()

                End If

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ControlesCitaCancelada(ByVal p_blnEstado As Boolean)
        Try
            If p_blnEstado Then
                FormularioSBO.Items.Item("1").Enabled = False
            Else
                FormularioSBO.Items.Item("1").Enabled = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ObtenerDatosVehiculo(ByVal p_strCodVehiculo As String, Optional ByVal p_blnBuscaCliente As Boolean = False)
        Try
            Dim l_strSQL As String
            Dim l_dtDatosVeh As System.Data.DataTable

            l_strSQL = " SELECT VEHI.U_Cod_Unid, VEHI.U_Num_Plac,VEHI.U_Des_Marc,VEHI.U_Des_Esti, VEHI.U_Des_Mode, VEHI.U_Ano_Vehi, VEHI.U_MarcaMot, VEHI.U_Combusti, VEHI.U_CardCode, VEHI.U_CardName," & _
                        " MM.Name AS Marc_Mot, COM.Name as Combust" & _
                        " FROM [@SCGD_VEHICULO] VEHI with (nolock)" & _
                        " LEFT OUTER JOIN [@SCGD_MARCA_MOTOR] MM with (nolock) on VEHI.U_MarcaMot = MM.Code " & _
                        " LEFT OUTER JOIN [@SCGD_COMBUSTIBLE] COM with (nolock) on VEHI.U_Combusti = COM.Code" & _
                        " WHERE VEHI.U_Cod_Unid = '{0}'"

            l_strSQL = String.Format(l_strSQL, p_strCodVehiculo)

            l_dtDatosVeh = Utilitarios.EjecutarConsultaDataTable(l_strSQL, m_oCompany.CompanyDB, m_oCompany.Server)
            If l_dtDatosVeh.Rows.Count <> 0 Then

                With l_dtDatosVeh.Rows(0)
                    EditTextUnidad.AsignaValorDataSource(IIf(IsDBNull(.Item("U_Cod_Unid")), "", .Item("U_Cod_Unid")))

                    EditTextMarca.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Des_Marc")), "", .Item("U_Des_Marc")))
                    EditTextMarca.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Des_Marc")), "", .Item("U_Des_Marc")))
                    EditTextAno.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Ano_Vehi")), "", .Item("U_Ano_Vehi")))
                    EditTextEstilo.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Des_Esti")), "", .Item("U_Des_Esti")))
                    EditTextModelo.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Des_Mode")), "", .Item("U_Des_Mode")))
                    EditTextMotor.AsignaValorUserDataSource(IIf(IsDBNull(.Item("Marc_Mot")), "", .Item("Marc_Mot")))
                    EditTextCombustible.AsignaValorUserDataSource(IIf(IsDBNull(.Item("Combust")), "", .Item("Combust")))

                    If p_blnBuscaCliente Then
                        EditTextCardCode.AsignaValorDataSource(IIf(IsDBNull(.Item("U_CardCode")), "", .Item("U_CardCode")))
                        EditTextCardName.AsignaValorDataSource(IIf(IsDBNull(.Item("U_CardName")), "", .Item("U_CardName")))
                    End If

                End With
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Sub

    Private Sub ObtenerLineasCotizacion(ByVal p_strNumCot As String)
        Try
            Dim l_strMonCot As String
            Dim l_strMonLin As String
            Dim l_decTotalLine As Decimal
            Dim l_decTipoC As Decimal
            Dim m_Cotizacion As SAPbobsCOM.Documents
            Dim m_CotizacionLineas As SAPbobsCOM.Document_Lines
            Dim l_strDuracion As String

            l_decTipoC = Decimal.Parse(EditTextTipoCambio.ObtieneValorDataSource(), n)

            m_Cotizacion = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            m_Cotizacion.GetByKey(p_strNumCot)

            m_CotizacionLineas = m_Cotizacion.Lines

            For i As Integer = 0 To m_CotizacionLineas.Count - 1
                m_CotizacionLineas.SetCurrentLine(i)

                l_strMonLin = m_CotizacionLineas.Currency
                l_strMonCot = m_Cotizacion.DocCurrency

                dtListaServicios.Rows.Add()
                dtListaServicios.SetValue("codigo", i, m_CotizacionLineas.ItemCode)
                dtListaServicios.SetValue("descripcion", i, m_CotizacionLineas.ItemDescription)
                dtListaServicios.SetValue("moneda", i, m_CotizacionLineas.Currency)
                dtListaServicios.SetValue("linea", i, m_CotizacionLineas.LineNum)
                dtListaServicios.SetValue("precio", i, m_CotizacionLineas.Price)
                dtListaServicios.SetValue("cantidad", i, m_CotizacionLineas.Quantity.ToString(n))
                dtListaServicios.SetValue("barras", i, m_CotizacionLineas.BarCode.ToString(n))

                If String.IsNullOrEmpty(m_CotizacionLineas.UserFields.Fields.Item("U_SCGD_TipArt").Value) Then
                    dtListaServicios.SetValue("tipo", i, String.Empty)
                Else
                    dtListaServicios.SetValue("tipo", i, m_CotizacionLineas.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                End If

                If String.IsNullOrEmpty(m_CotizacionLineas.UserFields.Fields.Item("U_SCGD_DurSt").Value) OrElse m_CotizacionLineas.UserFields.Fields.Item("U_SCGD_TipArt").Value.Equals("5") Then
                    dtListaServicios.SetValue("duracion", i, 0)
                Else
                    dtListaServicios.SetValue("duracion", i, m_CotizacionLineas.UserFields.Fields.Item("U_SCGD_DurSt").Value)
                    l_strDuracion = m_CotizacionLineas.UserFields.Fields.Item("U_SCGD_DurSt").Value
                End If

                If l_strMonCot.Equals(m_strMonedaLocal) Then
                    dtListaServicios.SetValue("total", i, m_CotizacionLineas.LineTotal)
                Else
                    l_decTotalLine = m_CotizacionLineas.LineTotal / l_decTipoC
                    dtListaServicios.SetValue("total", i, l_decTotalLine.ToString(n))
                End If

                dtListaServicios.SetValue("impuesto", i, m_CotizacionLineas.TaxCode)

            Next

            MatrizServicios.Matrix.LoadFromDataSource()

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Sub

    Public Sub MarcarItemsTipoPaquete()
        Try
            Dim l_strSQL As String
            Dim l_strCodArt As String
            Dim l_strCodArtPadre As String = String.Empty
            Dim l_strTipoArtPadre As String = String.Empty

            l_strSQL = " Select  TT.Code, TT.Quantity, OI.U_SCGD_Duracion, U_SCGD_TipoArticulo " +
                    " from OITT IT with (nolock)" +
                    " INNER JOIN ITT1 TT with (nolock) ON IT.Code =  TT.Father " +
                    " INNER JOIN OITM OI with (nolock) ON  OI.ItemCode = TT.Code " +
                    " where IT.Code = '{0}' "

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()

            MatrizServicios.Matrix.FlushToDataSource()
            For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                If Not String.IsNullOrEmpty(dtListaServicios.GetValue("tipo", i)) Then

                    l_strCodArt = dtListaServicios.GetValue("codigo", i)


                    If dtListaServicios.GetValue("tipo", i) = "5" Then

                        dtListaServicios.SetValue("hijo", i, "N")

                        l_strCodArtPadre = dtListaServicios.GetValue("padre", i)
                        l_strTipoArtPadre = ObtenerTipoDelPadre(l_strCodArt)

                        dtListaServicios.SetValue("paquete", i, l_strTipoArtPadre)

                        If String.IsNullOrEmpty(dtListaServicios.GetValue("padre", i)) Then
                            dtListaServicios.SetValue("padre", i, dtListaServicios.GetValue("codigo", i))
                        End If


                        If l_strTipoArtPadre.Equals("S") Then
                            dtListaServicios.SetValue("paquete", i, l_strTipoArtPadre)
                            md_Local.ExecuteQuery(String.Format(l_strSQL, l_strCodArt))

                            For j As Integer = 0 To md_Local.Rows.Count - 1
                                If Not String.IsNullOrEmpty(md_Local.GetValue("Code", j)) Then
                                    If md_Local.GetValue("U_SCGD_TipoArticulo", j) <> "5" Then

                                        For k As Integer = 0 To md_Local.Rows.Count - 1
                                            If Not String.IsNullOrEmpty(md_Local.GetValue("Code", k)) Then

                                                For m As Integer = 0 To dtListaServicios.Rows.Count - 1

                                                    If md_Local.GetValue("Code", k).Equals(dtListaServicios.GetValue("codigo", m)) Then
                                                        dtListaServicios.SetValue("hijo", m, "Y")
                                                        dtListaServicios.SetValue("paquete", m, l_strTipoArtPadre)

                                                        If String.IsNullOrEmpty(l_strCodArtPadre) Then
                                                            dtListaServicios.SetValue("padre", m, l_strCodArt)
                                                        Else
                                                            dtListaServicios.SetValue("padre", m, l_strCodArtPadre & "##" & l_strCodArt)
                                                        End If
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        Next

                                    ElseIf md_Local.GetValue("U_SCGD_TipoArticulo", j) = "5" Then

                                        md_Local2 = _formularioSbo.DataSources.DataTables.Item("dtLocal2")
                                        md_Local2.Clear()

                                        md_Local2.ExecuteQuery(String.Format(l_strSQL, md_Local.GetValue("Code", j)))

                                        For n As Integer = 0 To md_Local2.Rows.Count - 1

                                            For k As Integer = 0 To dtListaServicios.Rows.Count - 1

                                                If md_Local2.GetValue("Code", n).Equals(dtListaServicios.GetValue("codigo", k)) Then

                                                    dtListaServicios.SetValue("hijo", k, "Y")
                                                    dtListaServicios.SetValue("paquete", k, l_strTipoArtPadre)

                                                    If String.IsNullOrEmpty(l_strCodArtPadre) Then
                                                        dtListaServicios.SetValue("padre", k, l_strCodArt & "##" & md_Local.GetValue("Code", j))
                                                    Else
                                                        dtListaServicios.SetValue("padre", k, l_strCodArt & "##" & md_Local.GetValue("Code", j))
                                                    End If
                                                    Exit For
                                                End If
                                            Next
                                        Next
                                    End If
                                End If
                            Next


                        Else


                            md_Local.ExecuteQuery(String.Format(l_strSQL, l_strCodArt))

                            For j As Integer = 0 To md_Local.Rows.Count - 1
                                If Not String.IsNullOrEmpty(md_Local.GetValue("Code", j)) Then

                                    For m As Integer = 0 To dtListaServicios.Rows.Count - 1

                                        If md_Local.GetValue("Code", j).Equals(dtListaServicios.GetValue("codigo", m)) Then
                                            dtListaServicios.SetValue("hijo", m, "Y")
                                            dtListaServicios.SetValue("paquete", m, l_strTipoArtPadre)

                                            If String.IsNullOrEmpty(l_strCodArtPadre) Then
                                                dtListaServicios.SetValue("padre", m, l_strCodArt)
                                            Else
                                                dtListaServicios.SetValue("padre", m, l_strCodArtPadre & "##" & l_strCodArt)
                                            End If
                                            Exit For
                                        End If
                                    Next
                                End If
                            Next

                        End If
                    End If
                End If
            Next

            MatrizServicios.Matrix.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Private Sub AsignaValoresEditTextUnidad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim l_strCombus As String
            Dim l_strMotor As String
            Dim l_strCardCode As String
            Dim blMultiples As Boolean = False
            Dim l_strNombreCamp As String
            Dim l_strCodCampa As String

            Dim strConsultaComb As String = "select Name from [@SCGD_COMBUSTIBLE] with (nolock) where Code = '{0}'"
            Dim strConsultaMot As String = "select Name from [@SCGD_MARCA_MOTOR] with (nolock) where Code = '{0}'"
            Dim strConsultClie As String = "SELECT ListNum FROM OCRD with (nolock) WHERE CardCode = '{0}' "

            Dim strComb As String = ""
            Dim strMot As String = ""
            Call LimpiarCampos()

            l_strCombus = oDataTable.Columns.Item("U_Combusti").Cells.Item(0).Value()
            l_strMotor = oDataTable.Columns.Item("U_MarcaMot").Cells.Item(0).Value()
            l_strCardCode = oDataTable.Columns.Item("U_CardCode").Cells.Item(0).Value()

            strComb = Utilitarios.EjecutarConsulta(String.Format(strConsultaComb, l_strCombus), m_oCompany.CompanyDB, m_oCompany.Server)
            strMot = Utilitarios.EjecutarConsulta(String.Format(strConsultaMot, l_strMotor), m_oCompany.CompanyDB, m_oCompany.Server)
            m_strListaPreciosCli = Utilitarios.EjecutarConsulta(String.Format(strConsultClie, l_strCardCode), m_oCompany.CompanyDB, m_oCompany.Server)
            ' Call ObtenerMonedaSocioNegocios(l_strCardCode)

            EditTextCardCodeCliOT.AsignaValorDataSource(oDataTable.Columns.Item("U_CardCode").Cells.Item(0).Value())
            EditTextCardNameCliOT.AsignaValorDataSource(oDataTable.Columns.Item("U_CardName").Cells.Item(0).Value())
            EditTextUnidad.AsignaValorDataSource(oDataTable.Columns.Item("U_Cod_Unid").Cells.Item(0).Value())
            EditTextNumPlaca.AsignaValorDataSource(oDataTable.Columns.Item("U_Num_Plac").Cells.Item(0).Value())

            EditTextMarca.AsignaValorUserDataSource(oDataTable.Columns.Item("U_Des_Marc").Cells.Item(0).Value())
            EditTextEstilo.AsignaValorUserDataSource(oDataTable.Columns.Item("U_Des_Esti").Cells.Item(0).Value())
            EditTextModelo.AsignaValorUserDataSource(oDataTable.Columns.Item("U_Des_Mode").Cells.Item(0).Value())
            EditTextCombustible.AsignaValorUserDataSource(strComb)
            EditTextMotor.AsignaValorUserDataSource(strMot)
            EditTextAno.AsignaValorUserDataSource(oDataTable.Columns.Item("U_Ano_Vehi").Cells.Item(0).Value())
            FormularioSBO.Freeze(False)
            EditTextIdVehiculo.AsignaValorDataSource(oDataTable.Columns.Item("DocEntry").Cells.Item(0).Value())
            '  EditTextPlaca.AsignaValorUserDataSource(oDataTable.Columns.Item("U_Num_Plac").Cells.Item(0).Value())

            If DMS_Connector.Configuracion.ParamGenAddon.U_CnpDMS.Trim().Equals("Y") Then

                EditTextObservaciones.AsignaValorDataSource(Utilitarios.VerificaCampanaPorUnidad(EditTextUnidad.ObtieneValorDataSource(),
                                                                                                                 String.Empty,
                                                                                                                 ApplicationSBO, blMultiples, l_strCodCampa, l_strNombreCamp))

            End If

            If blMultiples = True Then
                EditTextCampaña.AsignaValorDataSource("Multiples")
                EditTextCampañaNombre.AsignaValorDataSource("Multiples Campañas")
            ElseIf blMultiples = False Then
                EditTextCampaña.AsignaValorDataSource(l_strCodCampa)
                EditTextCampañaNombre.AsignaValorDataSource(l_strNombreCamp)
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try

    End Sub

    Public Sub AsignaValoresEditTextUICliente(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try

            EditTextCardCode.AsignaValorDataSource(oDataTable.GetValue("CardCode", 0))
            EditTextCardName.AsignaValorDataSource(oDataTable.GetValue("CardName", 0))

            m_strListaPreciosCli = oDataTable.GetValue("ListNum", 0)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AsignaValoresEditTextUIClienteNumeroOT(ByVal FormUID As String, ByVal pVal As ItemEvent, ByVal oDataTable As DataTable)
        Try
            EditTextCardCodeCliOT.AsignaValorDataSource(oDataTable.GetValue("CardCode", 0))
            EditTextCardNameCliOT.AsignaValorDataSource(oDataTable.GetValue("CardName", 0))

            m_strListaPreciosCli = oDataTable.GetValue("ListNum", 0)
        Catch ex As Exception

        End Try
    End Sub


    Public Sub AsignaValoresMatrizSinArtic(ByRef BubbleEvent As Boolean)

        Try
            Dim l_strSQLSucursal As String
            Dim l_nFila As Integer


            l_strSQLSucursal = " SELECT SU.U_ArtCita, IT.ItemName, IT.U_SCGD_TipoArticulo FROM [@SCGD_CONF_SUCURSAL] SU with (nolock) INNER JOIN OITM IT ON IT.ItemCode = SU.U_ArtCita " & _
                                   " WHERE U_Sucurs = '{0}'"
            l_strSQLSucursal = String.Format(l_strSQLSucursal, EditCboSucursal.ObtieneValorDataSource())

            md_Configuracion.Clear()
            md_Configuracion.ExecuteQuery(l_strSQLSucursal)

            dtListaServicios.Rows.Clear()

            If md_Configuracion.Rows.Count > 1 _
                OrElse (md_Configuracion.GetValue("U_ArtCita", 0) <> "0") _
                OrElse Not String.IsNullOrEmpty(md_Configuracion.GetValue("U_ArtCita", 0)) Then

                If dtListaServicios.Rows.Count = 0 OrElse String.IsNullOrEmpty(dtListaServicios.GetValue("codigo", 0)) Then
                    l_nFila = 0
                Else
                    l_nFila = dtListaServicios.Rows.Count
                End If

                dtListaServicios.Rows.Add()

                dtListaServicios.SetValue("codigo", l_nFila, md_Configuracion.GetValue("U_ArtCita", 0))
                dtListaServicios.SetValue("descripcion", l_nFila, md_Configuracion.GetValue("ItemName", 0))
                dtListaServicios.SetValue("cantidad", l_nFila, 1)
                dtListaServicios.SetValue("tipo", l_nFila, md_Configuracion.GetValue("U_SCGD_TipoArticulo", 0))
                dtListaServicios.SetValue("moneda", l_nFila, String.Empty)
                dtListaServicios.SetValue("precio", l_nFila, 0)
                dtListaServicios.SetValue("duracion", l_nFila, 0)
                dtListaServicios.SetValue("impuesto", l_nFila, String.Empty)
                dtListaServicios.SetValue("total", l_nFila, 0)
                dtListaServicios.SetValue("hijo", l_nFila, "N")

                MatrizServicios.Matrix.LoadFromDataSource()

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            BubbleEvent = False
        End Try
    End Sub

    Private Function ValidarSiFormularioAbierto(ByVal strFormUID As String, _
                                           ByVal blnselectIfOpen As Boolean) As Boolean

        Dim intI As Integer = 0
        Dim blnFound As Boolean = False
        Dim frmForma As SAPbouiCOM.Form

        While (Not blnFound AndAlso intI < ApplicationSBO.Forms.Count)

            frmForma = ApplicationSBO.Forms.Item(intI)

            If frmForma.UniqueID = strFormUID Then
                blnFound = True
                If (blnselectIfOpen) Then
                    If Not (frmForma.Selected) Then
                        ApplicationSBO.Forms.Item(strFormUID).Select()
                    End If
                End If
            Else
                intI += 1
            End If
        End While

        Return blnFound

    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal ObjectType As String, ByVal UniqueID As String)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaCFLColumn(ByVal p_strMatriz As String, ByVal p_strColumn As String, ByVal p_strCFL As String, ByVal p_Alias As String)
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oMatrix As SAPbouiCOM.Matrix

            oitem = FormularioSBO.Items.Item(p_strMatriz)
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

            oMatrix.Columns.Item(p_strColumn).ChooseFromListUID = p_strCFL
            oMatrix.Columns.Item(p_strColumn).ChooseFromListAlias = p_Alias
            '-----------------------------------------------
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Private Sub CargarValoresValidosCombos(ByVal p_strSQL As String, ByRef p_strIDItem As String, Optional ByVal p_blnUsarVacio As Boolean = False)
        Try

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim intRecIndex As Integer
            _formularioSbo.Freeze(True)

            oItem = FormularioSBO.Items.Item(p_strIDItem)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            md_Local.Clear()
            md_Local.ExecuteQuery(p_strSQL)

            If oCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To oCombo.ValidValues.Count - 1
                    oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            If p_blnUsarVacio Then
                oCombo.ValidValues.Add("", "")
            End If

            For i As Integer = 0 To md_Local.Rows.Count - 1
                oCombo.ValidValues.Add(md_Local.GetValue(0, i), md_Local.GetValue(1, i))
            Next

            _formularioSbo.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
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
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try

    End Sub

    Public Sub AsignaValoresColImpuestoVeh(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            FormularioSBO.Freeze(True)

            MatrizServicios.Matrix.FlushToDataSource()
            dtListaServicios.SetValue("impuesto", pVal.Row - 1, oDataTable.GetValue("Code", 0))

            MatrizServicios.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            FormularioSBO.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AsignaValoresColMoneda(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            FormularioSBO.Freeze(True)

            MatrizServicios.Matrix.FlushToDataSource()
            dtListaServicios.SetValue("moneda", pVal.Row - 1, oDataTable.GetValue("CurrCode", 0))
            MatrizServicios.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            FormularioSBO.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

#Region "Agenda"

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetForegroundWindow() As IntPtr
    End Function

    Dim otmpForm As SAPbouiCOM.Form

    Private Sub _frmAgenda_eFechaYHoraSeleccionada(ByVal p_dtFechaYHora As Date,
                                                   ByVal p_strNombreAgenda As String,
                                                   ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaCitas.eFechaYHoraSeleccionada

        Dim l_strfechaCita As String
        Dim l_strHoraCita As String
        Dim l_strMinutosCita As String


        l_strfechaCita = p_dtFechaYHora.ToString("yyyyMMdd")
        EditTextFecha.AsignaValorDataSource(l_strfechaCita)

        l_strHoraCita = p_dtFechaYHora.ToString("HH")
        l_strMinutosCita = p_dtFechaYHora.ToString("mm")
        l_strHoraCita = l_strHoraCita & l_strMinutosCita

        EditTextHora.AsignaValorDataSource(l_strHoraCita)
        CalculaFechaFinCita()

        If m_strFechaCita <> EditTextFecha.ObtieneValorDataSource() OrElse
            m_strHoraCita <> EditTextHora.ObtieneValorDataSource() Then

            If FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        End If

        _frmAgendaCitas.Close()
        _frmAgendaCitas = Nothing

    End Sub

    Private Sub _frmOcupacionAgenda_CitaNueva_Equipos(ByVal p_fhaAsesor As Date,
                                                         ByVal p_fhaTecnico As Date,
                                                         ByVal p_strCodAsesor As String,
                                                         ByVal p_strCodTecnico As String,
                                                         ByVal p_strCodSucur As String,
                                                         ByVal p_strCodAgenda As String) Handles _frmAcupacionAgendas.eCargaCitaNueva_PorEquipos 'eCargaCitaNueva_PorAgenda
        ' ) Handles _frmAcupacionAgendas.eFechaYHoraAsesorTecnicoSinCita 'eCargaCitaNueva_PorAgenda
        Try

            Dim l_strFhaAsesor As String
            Dim l_strFhaTecnico As String
            Dim l_strHraASesor As String
            Dim l_strHraTecnico As String

            If p_fhaAsesor = Date.MinValue Then
                l_strFhaAsesor = String.Empty
                l_strHraASesor = String.Empty
            Else
                l_strFhaAsesor = p_fhaAsesor.ToString("yyyyMMdd")
                l_strHraASesor = p_fhaAsesor.ToString("HH") & p_fhaAsesor.ToString("mm")
            End If

            If p_fhaTecnico = Date.MinValue Then
                l_strFhaTecnico = String.Empty
                l_strHraTecnico = String.Empty
            Else
                l_strFhaTecnico = p_fhaTecnico.ToString("yyyyMMdd")
                l_strHraTecnico = p_fhaTecnico.ToString("HH") & p_fhaTecnico.ToString("mm")
            End If

            p_strCodAsesor = IIf(p_strCodAsesor.Equals("-1"), String.Empty, p_strCodAsesor)
            p_strCodTecnico = IIf(p_strCodTecnico.Equals("-1"), String.Empty, p_strCodTecnico)

            EditTextFecha.AsignaValorDataSource(l_strFhaAsesor)
            EditTextHora.AsignaValorDataSource(l_strHraASesor)
            EditTextFhaServicio.AsignaValorDataSource(l_strFhaTecnico)
            EditTextHoraServicio.AsignaValorDataSource(l_strHraTecnico)

            EditCboAsesor.AsignaValorDataSource(p_strCodAsesor)
            EditCboTecnico.AsignaValorDataSource(p_strCodTecnico)
            EditCboAgenda.AsignaValorDataSource(p_strCodAgenda)


            _frmAcupacionAgendas.Close()
            _frmAcupacionAgendas = Nothing

            If _formularioSbo.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSbo.Mode = BoFormMode.fm_UPDATE_MODE
            End If


            ActualizaValoresCombos()
            ObtenerInformacionDeTecnico()
            CalculaFechaFinCita()

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub _frmAgendaColor_eFechaYHoraSeleccionadaColor(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles _frmAgendaCitasColor.eFechaYHoraSeleccionadaColor

        Dim l_strfechaCita As String
        Dim l_strHoraCita As String
        Dim l_strMinutosCita As String


        l_strfechaCita = p_dtFechaYHora.ToString("yyyyMMdd")
        EditTextFecha.AsignaValorDataSource(l_strfechaCita)

        l_strHoraCita = p_dtFechaYHora.ToString("HH")
        l_strMinutosCita = p_dtFechaYHora.ToString("mm")
        l_strHoraCita = l_strHoraCita & l_strMinutosCita

        EditTextHora.AsignaValorDataSource(l_strHoraCita)
        CalculaFechaFinCita()

        If m_strFechaCita <> EditTextFecha.ObtieneValorDataSource() OrElse
            m_strHoraCita <> EditTextHora.ObtieneValorDataSource() Then

            If FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        End If

        _frmAgendaCitasColor.Close()
        _frmAgendaCitasColor = Nothing

    End Sub


#End Region

End Class

