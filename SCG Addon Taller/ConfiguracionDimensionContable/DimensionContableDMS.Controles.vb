Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMS_Addon.ControlesSBO

Partial Public Class DimensionContableDMS : Implements IFormularioSBO, IUsaPermisos

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

    Private ButtonCrear As SCG.SBOFramework.UI.ButtonSBO

    Private EditTextDocEntry As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextInventario As SCG.SBOFramework.UI.ComboBoxSBO

    Private MatrixLineasDimension As MatrizLineasDimensiones
    Private MatrixLineasConfiguracion As MatrizLineasConfiguracionDocumentos

    Private dtConfiguraciones As SAPbouiCOM.DataTable
    Private Const strDtConfiguraciones As String = "Configuraciones"


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

        FormularioSBO.Freeze(True)

     'FormularioSBO.Items.Item("TipInv").Update()


        'FormularioSBO.PaneLevel = 1

        'Call AgregaButtonPic(FormularioSBO, "btnVeh", 98, 197, 0, 0, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\Flecha.BMP", "")

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        Dim sboItem As SAPbouiCOM.Item

        Dim ocombo As SAPbouiCOM.ComboBox
        Dim oMatrix As SAPbouiCOM.Matrix

        ocombo = DirectCast(FormularioSBO.Items.Item("TipInv").Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(ocombo.ValidValues, "Select ""Code"",""Name"" From ""@SCGD_TIPOVEHICULO"" Order by ""Name"" ")

        ButtonCrear = New SCG.SBOFramework.UI.ButtonSBO("1", FormularioSBO)
        ButtonCrear.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

        EditTextDocEntry = New SCG.SBOFramework.UI.EditTextSBO("txtDocEnt", True, "@SCGD_DIMEN", "DocEntry", FormularioSBO)
        EditTextInventario = New SCG.SBOFramework.UI.ComboBoxSBO("TipInv", FormularioSBO, True, "@SCGD_DIMEN", "U_Tip_Inv")

        MatrixLineasDimension = New MatrizLineasDimensiones("mtxDim", FormularioSBO, "@SCGD_LINEAS_DIMEN")
        MatrixLineasDimension.CreaColumnas()

        EnlazaColumnasMatrixaDatasource(MatrixLineasDimension)

        EditTextDocEntry.AsignaBinding()
        EditTextInventario.AsignaBinding()
        ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim DocentryItem As SAPbouiCOM.Item = FormularioSBO.Items.Item("TipInv")
        DocentryItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 12, BoModeVisualBehavior.mvb_True)

        ' FormularioSBO.SupportedModes = 12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        FormularioSBO.DataBrowser.BrowseBy = "TipInv"

        FormularioSBO.Mode = BoFormMode.fm_FIND_MODE

        ListaActualConfiguracion.Clear()
        ListaModificadaConfiguracion.Clear()

        Call CreaDataTableConfiguracion()

        'Call EnlazarCamposConfiguraciones(FormularioSBO)
        CargarConfiguracionDocumentos()

        FormularioSBO.PaneLevel = 1
        FormularioSBO.Freeze(False)

    End Sub

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As MatrizLineasDimensiones) As Boolean

        Dim oColumna As ColumnaMatrixSBO(Of String)

        Try

            oColumna = oMatrix.ColumnaMarca
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_CodMar")

            oColumna = oMatrix.ColumnaDescripcion
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_DesMar")

            oColumna = oMatrix.ColumnaDim1
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_Dim1")

            oColumna = oMatrix.ColumnaDim2
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_Dim2")

            oColumna = oMatrix.ColumnaDim3
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_Dim3")

            oColumna = oMatrix.ColumnaDim4
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_Dim4")

            oColumna = oMatrix.ColumnaDim5
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_LINEAS_DIMEN", "U_Dim5")

            Return True

        Catch ex As Exception
            ' Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function


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
            Return Nothing
        End Try

    End Function

    Private Sub CreaDataTableConfiguracion()

        'datatable para el manejo de Configuraciones de documentos en CV
        dtConfiguraciones = FormularioSBO.DataSources.DataTables.Add(strDtConfiguraciones)
        dtConfiguraciones.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfiguraciones.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric, 100)
        dtConfiguraciones.Columns.Add("U_Valor", BoFieldsType.ft_AlphaNumeric, 100)
       
        MatrixLineasConfiguracion = New MatrizLineasConfiguracionDocumentos("mtxConf", FormularioSBO, strDtConfiguraciones)
        MatrixLineasConfiguracion.CreaColumnas()
        MatrixLineasConfiguracion.LigaColumnas()
    End Sub

End Class