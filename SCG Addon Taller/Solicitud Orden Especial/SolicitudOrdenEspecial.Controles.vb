Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI.Extensions
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMS_Addon.ControlesSBO

Partial Public Class SolicitudOrdenEspecial : Implements IFormularioSBO, IUsaPermisos

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

    Private EditTextDocEntry As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCodCliente As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextNomCliente As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCodAsesor As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCotizacion As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextTipoOrden As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextOTReferencia As SCG.SBOFramework.UI.EditTextSBO

    Private EditTextCodigoUnidad As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextIDVehiculo As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextAnno As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextVIN As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextPlaca As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextKilometraje As SCG.SBOFramework.UI.EditTextSBO

    Private EditTextDesMarca As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextDesModelo As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextDesEstilo As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCodMarca As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCodModelo As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCodEstilo As SCG.SBOFramework.UI.EditTextSBO

    Private EditTextNoVisita As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextOTPadre As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextEstadoOT As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaApertura As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaCompromiso As SCG.SBOFramework.UI.EditTextSBO

    Private EditTextCotizacionCreada As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextCotizacionReferencia As SCG.SBOFramework.UI.EditTextSBO

    Private EditTextNombreAsesor As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextNombreTipoOT As SCG.SBOFramework.UI.EditTextSBO

    Private EditTextEstadoDocumento As SCG.SBOFramework.UI.EditTextSBO

    Private CheckBoxSel As SCG.SBOFramework.UI.CheckBoxSBO
    Private checkBoxImpresion As SCG.SBOFramework.UI.CheckBoxSBO


    Private ButtonCrearOTEspecial As SCG.SBOFramework.UI.ButtonSBO

    Private MatrixLineasCotizacion As MatrixSBOLineasCot

    Private dataTableEncabezado As DataTable
    Private dataTableMatriz As DataTable
    Private g_dtEstadosOT As SAPbouiCOM.DataTable
    
    Public Const g_strdtEstadosOT As String = "tEstadosOT"

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
    
    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        Dim oItem As SAPbouiCOM.Item

        FormularioSBO.Freeze(True)

        FormularioSBO.PaneLevel = 1

        Call AgregaButtonPic(FormularioSBO, "btnVeh", 98, 197, 0, 0, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\Flecha.BMP", "")

        If FormularioSBO IsNot Nothing Then

            For Each oItem In FormularioSBO.Items

                If oItem.UniqueID = "chkAll" Then

                    oItem.AffectsFormMode = False

                End If

            Next

        End If

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)
        Dim dtLocal As DataTable
        Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSources.Add("seltod", BoDataType.dt_LONG_TEXT, 100)
        CheckBoxSel = New SCG.SBOFramework.UI.CheckBoxSBO("chkAll", True, "", "seltod", FormularioSBO)
        CheckBoxSel.AsignaBinding()


        EditTextDocEntry = New SCG.SBOFramework.UI.EditTextSBO("txtDocEn", True, "@SCGD_SOT_ESP", "DocEntry", FormularioSBO)
        EditTextCodCliente = New SCG.SBOFramework.UI.EditTextSBO("txtCodC", True, "@SCGD_SOT_ESP", "U_Cod_Clie", FormularioSBO)
        EditTextNomCliente = New SCG.SBOFramework.UI.EditTextSBO("txtNCli", True, "@SCGD_SOT_ESP", "U_Nom_Clie", FormularioSBO)
        EditTextCodAsesor = New SCG.SBOFramework.UI.EditTextSBO("txtCodA", True, "@SCGD_SOT_ESP", "U_Cod_Ases", FormularioSBO)
        EditTextCotizacion = New SCG.SBOFramework.UI.EditTextSBO("txtCotiz", True, "@SCGD_SOT_ESP", "U_Num_Coti", FormularioSBO)
        EditTextTipoOrden = New SCG.SBOFramework.UI.EditTextSBO("txtTipOr", True, "@SCGD_SOT_ESP", "U_TipoOrd", FormularioSBO)
        EditTextOTReferencia = New SCG.SBOFramework.UI.EditTextSBO("txtOTRef", True, "@SCGD_SOT_ESP", "U_OTRefer", FormularioSBO)

        EditTextCodigoUnidad = New SCG.SBOFramework.UI.EditTextSBO("txtCodUni", True, "@SCGD_SOT_ESP", "U_Cod_Uni", FormularioSBO)
        EditTextIDVehiculo = New SCG.SBOFramework.UI.EditTextSBO("txtIDVeh", True, "@SCGD_SOT_ESP", "U_Id_Vehi", FormularioSBO)
        EditTextAnno = New SCG.SBOFramework.UI.EditTextSBO("txtAnno", True, "@SCGD_SOT_ESP", "U_Anno", FormularioSBO)
        EditTextVIN = New SCG.SBOFramework.UI.EditTextSBO("txtVIN", True, "@SCGD_SOT_ESP", "U_VIN", FormularioSBO)
        EditTextPlaca = New SCG.SBOFramework.UI.EditTextSBO("txtPlac", True, "@SCGD_SOT_ESP", "U_Placa", FormularioSBO)
        EditTextKilometraje = New SCG.SBOFramework.UI.EditTextSBO("txtklm", True, "@SCGD_SOT_ESP", "U_klm", FormularioSBO)

        EditTextDesMarca = New SCG.SBOFramework.UI.EditTextSBO("txtDesM", True, "@SCGD_SOT_ESP", "U_Des_Mar", FormularioSBO)
        EditTextDesModelo = New SCG.SBOFramework.UI.EditTextSBO("txtDesMa", True, "@SCGD_SOT_ESP", "U_Des_Mod", FormularioSBO)
        EditTextDesEstilo = New SCG.SBOFramework.UI.EditTextSBO("txtDesEs", True, "@SCGD_SOT_ESP", "U_Des_Est", FormularioSBO)
        EditTextCodMarca = New SCG.SBOFramework.UI.EditTextSBO("txtCodM", True, "@SCGD_SOT_ESP", "U_Cod_Mar", FormularioSBO)
        EditTextCodModelo = New SCG.SBOFramework.UI.EditTextSBO("txtCodMa", True, "@SCGD_SOT_ESP", "U_Cod_Mod", FormularioSBO)
        EditTextCodEstilo = New SCG.SBOFramework.UI.EditTextSBO("txtCodEs", True, "@SCGD_SOT_ESP", "U_Cod_Est", FormularioSBO)

        EditTextNoVisita = New SCG.SBOFramework.UI.EditTextSBO("txtNoVis", True, "@SCGD_SOT_ESP", "U_No_Vis", FormularioSBO)
        EditTextOTPadre = New SCG.SBOFramework.UI.EditTextSBO("txtOTPad", True, "@SCGD_SOT_ESP", "U_OTPadre", FormularioSBO)
        EditTextEstadoOT = New SCG.SBOFramework.UI.EditTextSBO("txtEstOT", True, "@SCGD_SOT_ESP", "U_Estad_OT", FormularioSBO)
        EditTextFechaApertura = New SCG.SBOFramework.UI.EditTextSBO("txtFecAp", True, "@SCGD_SOT_ESP", "U_Fec_Ape", FormularioSBO)
        EditTextFechaCompromiso = New SCG.SBOFramework.UI.EditTextSBO("txtFecCom", True, "@SCGD_SOT_ESP", "U_Fec_Com", FormularioSBO)

        EditTextCotizacionCreada = New SCG.SBOFramework.UI.EditTextSBO("txtCoCread", True, "@SCGD_SOT_ESP", "U_CotCread", FormularioSBO)
        EditTextCotizacionReferencia = New SCG.SBOFramework.UI.EditTextSBO("txtCoCread", True, "@SCGD_SOT_ESP", "U_CotRef", FormularioSBO)
        EditTextNombreTipoOT = New SCG.SBOFramework.UI.EditTextSBO("txtNomTOT", True, "@SCGD_SOT_ESP", "U_NomTipOT", FormularioSBO)
        EditTextNombreAsesor = New SCG.SBOFramework.UI.EditTextSBO("txtNomAse", True, "@SCGD_SOT_ESP", "U_NomAse", FormularioSBO)
        'EditTextEstadoDocumento = New SCG.SBOFramework.UI.EditTextSBO("txtNomAse", True, "@SCGD_SOT_ESP", "U_NomAse", FormularioSBO)
        checkBoxImpresion = New SCG.SBOFramework.UI.CheckBoxSBO("chkImp", True, "@SCGD_SOT_ESP", "U_ImpRecp", FormularioSBO)
        
        EditTextDocEntry.AsignaBinding()
        EditTextCodAsesor.AsignaBinding()
        EditTextCotizacion.AsignaBinding()
        EditTextCodCliente.AsignaBinding()
        EditTextNomCliente.AsignaBinding()
        EditTextTipoOrden.AsignaBinding()
        EditTextOTReferencia.AsignaBinding()
        EditTextCodigoUnidad.AsignaBinding()
        EditTextIDVehiculo.AsignaBinding()
        EditTextAnno.AsignaBinding()
        EditTextVIN.AsignaBinding()
        EditTextPlaca.AsignaBinding()
        EditTextKilometraje.AsignaBinding()

        EditTextDesMarca.AsignaBinding()
        EditTextDesModelo.AsignaBinding()
        EditTextDesEstilo.AsignaBinding()
        EditTextCodMarca.AsignaBinding()
        EditTextCodModelo.AsignaBinding()
        EditTextCodEstilo.AsignaBinding()

        EditTextCotizacionReferencia.AsignaBinding()

        EditTextNoVisita.AsignaBinding()
        EditTextOTPadre.AsignaBinding()
        EditTextEstadoOT.AsignaBinding()
        EditTextFechaApertura.AsignaBinding()
        EditTextFechaCompromiso.AsignaBinding()

        EditTextNombreAsesor.AsignaBinding()
        EditTextNombreTipoOT.AsignaBinding()

        checkBoxImpresion.AsignaBinding()

        ButtonCrearOTEspecial = New SCG.SBOFramework.UI.ButtonSBO("1", FormularioSBO)
        ButtonCrearOTEspecial.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

        Dim DocentryItem As SAPbouiCOM.Item = FormularioSBO.Items.Item("txtDocEn")
        DocentryItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 12, BoModeVisualBehavior.mvb_True)

        FormularioSBO.SupportedModes = 5
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        FormularioSBO.DataBrowser.BrowseBy = "txtDocEn"
        FormularioSBO.Mode = BoFormMode.fm_FIND_MODE

        FormularioSBO.Items.Item("btnAddQ").Enabled = False

        dtLocal = FormularioSBO.DataSources.DataTables.Add("local")
        g_dtEstadosOT = FormularioSBO.DataSources.DataTables.Add(g_strdtEstadosOT)
        g_dtEstadosOT.ExecuteQuery(" select Code, Name from [@SCGD_ESTADOS_OT] with(nolock) order by Code ")
        FormularioSBO.Freeze(False)

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
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

End Class
