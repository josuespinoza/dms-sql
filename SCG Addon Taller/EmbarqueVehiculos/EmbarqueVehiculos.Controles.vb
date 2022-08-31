Imports System.Globalization
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class EmbarqueVehiculos
    : Implements IFormularioSBO, IUsaPermisos

#Region "Declaraciones"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany

    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _formType As String
    Private _inicializado As Boolean
    Private _formularioSBO As SAPbouiCOM.IForm
    
    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Nombre As String
    Private _Posicion As String

    'userDataSource
    Private UDS_SeleccionaRepuestos As UserDataSources

    Private dtArticulos As DataTable
    Private dtUnidades As DataTable
    Private dtDocumentos As DataTable
    Private dtUnidadesDoc As DataTable

    Private Const strdtArticulos As String = "dtArticulos"
    Private Const strdtUnidades As String = "dtUnidades"
    Private Const strdtDocumentos As String = "dtDocumentos"
    Private Const strdtUnidadesDoc As String = "dtUnidadesDoc"

    Private MatrizArticulos As MatrizEmbArticulos
    Private MatrizUnidades As MatrizEmbUnidades
    Private MatrizDocumentos As MatrizEmbDocumentos
    Private MatrizUnidadesDocumentos As MatrizEmbUnidadesDoc

    Private Const strmtxArticulos As String = "mtxArt"
    Private Const strmtxUnidades As String = "mtxUni"
    Private Const strmtxDocumentos As String = "mtxDoc"
    Private Const strmtxUnidadesDoc As String = "mtxUnD"

    Private txtNumEmb As UI.EditTextSBO
    Private txtFecEmb As UI.EditTextSBO
    Private txtFecArr As UI.EditTextSBO
    Private txtUbi As UI.EditTextSBO
    Private txtEst As UI.EditTextSBO
    Private txtPro As UI.EditTextSBO
    Private txtTipT As UI.EditTextSBO
    Private txtNomb As UI.EditTextSBO
    Private txtFecCont As UI.EditTextSBO
    Private txtMone As UI.EditTextSBO
    Private txtTipC As UI.EditTextSBO
    Private txtMonCos As UI.EditTextSBO
    Private txtToUni As UI.EditTextSBO
    Private txtMoTo As UI.EditTextSBO

    Private cboTipCos As UI.ComboBoxSBO

    Private btnGenUni As UI.ButtonSBO

    Private n As NumberFormatInfo

#End Region

#Region "Propiedades"

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
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
    
    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(ByVal value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(ByVal value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(ByVal value As Integer)
            _Posicion = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="companySbo"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strMenuEmbarqueVehiculos As String)
        _companySbo = companySbo
        _applicationSbo = application
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLEmbarqueVehiculos
        MenuPadre = "SCGD_MNO"
        Nombre = My.Resources.Resource.TituloEmbarqueVehiculos
        IdMenu = p_strMenuEmbarqueVehiculos
        Posicion = 74
        FormType = p_strMenuEmbarqueVehiculos
    End Sub

#End Region

#Region "Metodos"

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try
            CargaFormulario()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub CargaFormulario()
        Dim dtLocal As DataTable

        Try
            FormularioSBO.Freeze(True)

            AsociaControlesInterfaz()

            'FormularioSBO.Items.Item("cboGrp").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'FormularioSBO.Items.Item("cboPro").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

            dtArticulos = FormularioSBO.DataSources.DataTables.Add(strdtArticulos)
            dtArticulos.Columns.Add("cod", BoFieldsType.ft_AlphaNumeric, 100)
            dtArticulos.Columns.Add("des", BoFieldsType.ft_AlphaNumeric, 100)
            dtArticulos.Columns.Add("col", BoFieldsType.ft_AlphaNumeric, 100)
            dtArticulos.Columns.Add("can", BoFieldsType.ft_Quantity, 100)

            MatrizArticulos = New MatrizEmbArticulos(strmtxArticulos, FormularioSBO, strdtArticulos)
            MatrizArticulos.CreaColumnas()
            MatrizArticulos.LigaColumnas()

            dtUnidades = FormularioSBO.DataSources.DataTables.Add(strdtUnidades)
            dtUnidades.Columns.Add("uni", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("mar", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("est", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("mod", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("ubi", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("esta", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("dis", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidades.Columns.Add("tip", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizUnidades = New MatrizEmbUnidades(strmtxUnidades, FormularioSBO, strdtUnidades)
            MatrizUnidades.CreaColumnas()
            MatrizUnidades.LigaColumnas()

            dtUnidadesDoc = FormularioSBO.DataSources.DataTables.Add(strdtUnidadesDoc)
            dtUnidadesDoc.Columns.Add("uni", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidadesDoc.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidadesDoc.Columns.Add("cos", BoFieldsType.ft_Price, 100)
            dtUnidadesDoc.Columns.Add("mar", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidadesDoc.Columns.Add("est", BoFieldsType.ft_AlphaNumeric, 100)
            dtUnidadesDoc.Columns.Add("mod", BoFieldsType.ft_AlphaNumeric, 100)

            'crea matriz
            MatrizUnidadesDocumentos = New MatrizEmbUnidadesDoc(strmtxUnidadesDoc, FormularioSBO, strdtUnidadesDoc)
            MatrizUnidadesDocumentos.CreaColumnas()
            MatrizUnidadesDocumentos.LigaColumnas()

            dtDocumentos = FormularioSBO.DataSources.DataTables.Add(strdtDocumentos)
            dtDocumentos.Columns.Add("nod", BoFieldsType.ft_AlphaNumeric, 100)
            dtDocumentos.Columns.Add("doc", BoFieldsType.ft_AlphaNumeric, 100)
            dtDocumentos.Columns.Add("fdoc", BoFieldsType.ft_Date, 100)

            'crea matriz
            MatrizDocumentos = New MatrizEmbDocumentos(strmtxDocumentos, FormularioSBO, strdtDocumentos)
            MatrizDocumentos.CreaColumnas()
            MatrizDocumentos.LigaColumnas()

            dtLocal = FormularioSBO.DataSources.DataTables.Add("local")

            dtLocal.ExecuteQuery("select code, name from [@SCGD_COLOR]")

            AgregaChooseFromListItems(FormularioSBO, dtLocal)

            dtArticulos.Rows.Add()
            dtArticulos.SetValue("cod", 0, "")
            MatrizArticulos.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub AsociaControlesInterfaz()
        Try
            UDS_SeleccionaRepuestos = FormularioSBO.DataSources.UserDataSources
            UDS_SeleccionaRepuestos.Add("num", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("fec", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("farr", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("ubi", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("est", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("pro", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("tipt", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("nomb", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("feccont", BoDataType.dt_DATE, 100)
            UDS_SeleccionaRepuestos.Add("mone", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("tipc", BoDataType.dt_RATE, 100)
            UDS_SeleccionaRepuestos.Add("tipcos", BoDataType.dt_LONG_TEXT, 100)
            UDS_SeleccionaRepuestos.Add("monpro", BoDataType.dt_PRICE, 100)
            UDS_SeleccionaRepuestos.Add("touni", BoDataType.dt_QUANTITY, 100)
            UDS_SeleccionaRepuestos.Add("moto", BoDataType.dt_PRICE, 100)

            txtNumEmb = New UI.EditTextSBO("txtNumEmb", True, "", "num", FormularioSBO)
            txtNumEmb.AsignaBinding()

            txtFecEmb = New UI.EditTextSBO("txtFecEmb", True, "", "fec", FormularioSBO)
            txtFecEmb.AsignaBinding()

            txtFecArr = New UI.EditTextSBO("txtFecArr", True, "", "farr", FormularioSBO)
            txtFecArr.AsignaBinding()

            txtUbi = New UI.EditTextSBO("txtUbi", True, "", "ubi", FormularioSBO)
            txtUbi.AsignaBinding()

            txtEst = New UI.EditTextSBO("txtEst", True, "", "est", FormularioSBO)
            txtEst.AsignaBinding()

            txtPro = New UI.EditTextSBO("txtPro", True, "", "pro", FormularioSBO)
            txtPro.AsignaBinding()

            txtTipT = New UI.EditTextSBO("txtTipT", True, "", "tipt", FormularioSBO)
            txtTipT.AsignaBinding()

            txtNomb = New UI.EditTextSBO("txtNomb", True, "", "nomb", FormularioSBO)
            txtNomb.AsignaBinding()

            txtFecCont = New UI.EditTextSBO("txtFecCont", True, "", "feccont", FormularioSBO)
            txtFecCont.AsignaBinding()

            txtMone = New UI.EditTextSBO("txtMone", True, "", "mone", FormularioSBO)
            txtMone.AsignaBinding()

            txtTipC = New UI.EditTextSBO("txtTipC", True, "", "tipc", FormularioSBO)
            txtTipC.AsignaBinding()

            cboTipCos = New UI.ComboBoxSBO("cboTipCos", FormularioSBO, True, "", "tipc")
            cboTipCos.AsignaBinding()

            txtMonCos = New UI.EditTextSBO("txtMonPro", True, "", "monpro", FormularioSBO)
            txtMonCos.AsignaBinding()

            txtToUni = New UI.EditTextSBO("txtToUni", True, "", "touni", FormularioSBO)
            txtToUni.AsignaBinding()

            txtMoTo = New UI.EditTextSBO("txtMoTo", True, "", "moto", FormularioSBO)
            txtMoTo.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub AddChooseFromListItems(ByVal oform As Form)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "4"
            oCFLCreationParams.UniqueID = "CFL_Item"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL1
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_SCGD_TipoArticulo"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCon.CondVal = "8"

            oCFL.SetConditions(oCons)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub AgregaChooseFromListItems(ByVal oform As Form, ByVal dataTable As DataTable)
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oCombo As SAPbouiCOM.ComboBox

        Try
            If Not oform Is Nothing Then

                Call AddChooseFromListItems(oform)

                oitem = oform.Items.Item("mtxArt")
                oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

                oMatrix.Columns.Item("ColCod").ChooseFromListUID = "CFL_Item"
                oMatrix.Columns.Item("ColCod").ChooseFromListAlias = "ItemCode"

                'oMatrix.Columns.Item("ColCod"= "CFL_Item"

                'RS = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'RS.DoQuery("select ItemCode,ItemName from oitm")

                For i As Integer = 0 To dataTable.Rows.Count - 1
                    oMatrix.Columns.Item("ColCol").ValidValues.Add(dataTable.GetValue("code", i),
                                                                   dataTable.GetValue("name", i))
                Next

                'For i = 1 To RS.RecordCount
                '    If RS.EoF = False Then
                '        oCombo.ValidValues.Add(RS.Fields.Item("ItemCode").Value, RS.Fields.Item("ItemName").Value)
                '        RS.MoveNext()
                '    End If
                'Next

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


#End Region

End Class
