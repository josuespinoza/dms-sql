Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class BusquedaOrdenesTrabajo : Implements IFormularioSBO, IUsaMenu

#Region "Declaracioens"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSBO As SAPbouiCOM.IForm
    Private _inicializado As Boolean

    Private UDS_dtBusquedas As UserDataSources
    Private txtNoOT As EditTextSBO
    Private txtNoUnidad As EditTextSBO
    Private txtPlaca As EditTextSBO
    Private txtNoVisita As EditTextSBO
    Private txtNoCita As EditTextSBO
    Private txtNoCitaAb As EditTextSBO
    Private txtNoCono As EditTextSBO
    Private chkEstado As CheckBoxSBO
    Private chkMarca As CheckBoxSBO
    Private chkAsesor As CheckBoxSBO
    Private chkSucursal As CheckBoxSBO
    Private cboEstado As ComboBoxSBO
    Private cboMarca As ComboBoxSBO
    Private cboAsesor As ComboBoxSBO
    Private cboSucursal As ComboBoxSBO
    Private chkRecepcion As CheckBoxSBO
    Private chkCompromiso As CheckBoxSBO
    Private chkCerrado As CheckBoxSBO
    Private txtRecepcion1 As EditTextSBO
    Private txtRecepcion2 As EditTextSBO
    Private txtCompromiso1 As EditTextSBO
    Private txtCompromiso2 As EditTextSBO
    Private txtCerrado1 As EditTextSBO
    Private txtCerrado2 As EditTextSBO

    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strMenuBusqeudaOt As String)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLBusquedaOT
        MenuPadre = "SCGD_GOV"
        Nombre = My.Resources.Resource.TituloBusquedaOT
        IdMenu = p_strMenuBusqeudaOt
        Posicion = 199
        FormType = p_strMenuBusqeudaOt
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
            Return _companySBO
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
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
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

#End Region

#Region "Metodos"

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If Not FormularioSBO Is Nothing Then
            CargarFormulario()
        End If

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        If Not FormularioSBO Is Nothing Then
            FormularioSBO.Freeze(True)

            UDS_dtBusquedas = FormularioSBO.DataSources.UserDataSources
            UDS_dtBusquedas.Add("noot", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("nounidad", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("placa", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("novisita", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("nocita", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("nocitaab", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("nocono", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chkestado", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chkmarca", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chkasesor", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chksucur", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("estado", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("marca", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("asesor", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("sucursal", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chkrece", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chkcomp", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("chkcerr", BoDataType.dt_LONG_TEXT, 200)
            UDS_dtBusquedas.Add("recepci1", BoDataType.dt_DATE, 200)
            UDS_dtBusquedas.Add("recepci2", BoDataType.dt_DATE, 200)
            UDS_dtBusquedas.Add("compro1", BoDataType.dt_DATE, 200)
            UDS_dtBusquedas.Add("compro2", BoDataType.dt_DATE, 200)
            UDS_dtBusquedas.Add("cerrado1", BoDataType.dt_DATE, 200)
            UDS_dtBusquedas.Add("cerrado2", BoDataType.dt_DATE, 200)

            txtNoOT = New EditTextSBO("txtNoOT", True, "", "noot", FormularioSBO)
            txtNoUnidad = New EditTextSBO("txtNoUni", True, "", "nounidad", FormularioSBO)
            txtPlaca = New EditTextSBO("txtPlac", True, "", "placa", FormularioSBO)
            txtNoVisita = New EditTextSBO("txtNoVisi", True, "", "novisita", FormularioSBO)
            txtNoCita = New EditTextSBO("txtNoCita", True, "", "nocita", FormularioSBO)
            txtNoCitaAb = New EditTextSBO("txtNoCitAb", True, "", "nocitaab", FormularioSBO)
            txtNoCono = New EditTextSBO("txtNoCon", True, "", "nocono", FormularioSBO)
            chkEstado = New CheckBoxSBO("chkEst", True, "", "chkestado", FormularioSBO)
            chkMarca = New CheckBoxSBO("chkMar", True, "", "chkmarca", FormularioSBO)
            chkAsesor = New CheckBoxSBO("chkAse", True, "", "chkasesor", FormularioSBO)
            cboEstado = New ComboBoxSBO("cboEst", FormularioSBO, True, "", "estado")
            cboMarca = New ComboBoxSBO("cboMar", FormularioSBO, True, "", "marca")
            cboAsesor = New ComboBoxSBO("cboAse", FormularioSBO, True, "", "asesor")
            cboSucursal = New ComboBoxSBO("cboSucur", FormularioSBO, True, "", "sucursal")
            chkRecepcion = New CheckBoxSBO("chkAbie", True, "", "chkrece", FormularioSBO)
            chkCompromiso = New CheckBoxSBO("chkProc", True, "", "chkcomp", FormularioSBO)
            chkCerrado = New CheckBoxSBO("chkCer", True, "", "chkcerr", FormularioSBO)
            chkSucursal = New CheckBoxSBO("chkSucur", True, "", "chksucur", FormularioSBO)
            txtRecepcion1 = New EditTextSBO("txtRece1", True, "", "recepci1", FormularioSBO)
            txtRecepcion2 = New EditTextSBO("txtRece2", True, "", "recepci2", FormularioSBO)
            txtCompromiso1 = New EditTextSBO("txtComp1", True, "", "compro1", FormularioSBO)
            txtCompromiso2 = New EditTextSBO("txtComp2", True, "", "compro2", FormularioSBO)
            txtCerrado1 = New EditTextSBO("txtCerr1", True, "", "cerrado1", FormularioSBO)
            txtCerrado2 = New EditTextSBO("txtCerr2", True, "", "cerrado2", FormularioSBO)

            txtNoOT.AsignaBinding()
            txtNoUnidad.AsignaBinding()
            txtPlaca.AsignaBinding()
            txtNoVisita.AsignaBinding()
            txtNoCita.AsignaBinding()
            txtNoCitaAb.AsignaBinding()
            txtNoCono.AsignaBinding()
            chkEstado.AsignaBinding()
            chkMarca.AsignaBinding()
            chkAsesor.AsignaBinding()
            chkSucursal.AsignaBinding()
            cboEstado.AsignaBinding()
            cboMarca.AsignaBinding()
            cboAsesor.AsignaBinding()
            cboAsesor.AsignaBinding()
            cboSucursal.AsignaBinding()
            chkRecepcion.AsignaBinding()
            chkCompromiso.AsignaBinding()
            chkCerrado.AsignaBinding()
            txtRecepcion1.AsignaBinding()
            txtRecepcion2.AsignaBinding()
            txtCompromiso1.AsignaBinding()
            txtCompromiso2.AsignaBinding()
            txtCerrado1.AsignaBinding()
            txtCerrado2.AsignaBinding()

            FormularioSBO.Freeze(False)
        End If
    End Sub

    ''' <summary>
    ''' Manejo de eventos para el fomulario de busqueda de OT's
    ''' </summary>
    ''' <param name="FormUID">UID del formulario</param>
    ''' <param name="pVal">Objeto de tipo evento</param>
    ''' <param name="BubbleEvent">Evento burbuja de la aplicacion</param>
    ''' <remarks></remarks>
    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByVal BubbleEvent As Boolean)
        If pVal.FormTypeEx <> FormType Then Exit Sub
        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
        End Select
    End Sub
    
    ''' <summary>
    ''' carga los combos
    ''' </summary>
    ''' <param name="oForm">Formulario</param>
    ''' <param name="strQuery">Query a ejecutar para cargar el combo</param>
    ''' <param name="strIDItem">Combo que desea cargar</param>
    ''' <remarks></remarks>
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
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Sub

    ''' <summary>
    ''' maneja el estado Habilitado o desHabilitado para los componentes
    ''' </summary>
    ''' <param name="strComponente">Nombre del componente</param>
    ''' <param name="Valor">True o False si se desea o no habilitado</param>
    ''' <remarks></remarks>
    Private Sub ManejoComponente(ByVal strComponente As String, ByVal Valor As Boolean)
        FormularioSBO.Items.Item(strComponente).Enabled = Valor
    End Sub

#End Region

End Class
