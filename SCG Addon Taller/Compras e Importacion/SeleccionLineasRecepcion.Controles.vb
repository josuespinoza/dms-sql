Imports SAPbouiCOM
Imports SCG.SBOFramework.UI


Partial Public Class SeleccionLineasRecepcion : Implements IFormularioSBO

#Region "Declaracion Variables"
    Private MatrizSeleccion As MatrizSeleccionLineasRecepcion

    Public Shared _formularioSBO As Form
    Public Shared _companySBO As SAPbobsCOM.Company

    Public Shared _applicationSBO As Application
    Public Property strCodProveedor As String
    Private _nombreXml As String
    Private _titulo As String
    Private _inicializado As Boolean
    Public _formType As String
    Private oMatrix As Matrix

    Public txtRecepcion As EditTextSBO
    Public txtPedido As EditTextSBO
    Public txtUnidad As EditTextSBO

    Public btnAceptar As ButtonSBO
    Public btnCancelar As ButtonSBO
    Public btnActualiza As ButtonSBO

    Private dtVehiculos As DataTable
    Private dtSeleccionados As DataTable
    
    Public m_strCodDispoDevueltos As String
#End Region

#Region "Constructor"

    Public Sub New(ByVal SBOAplication As Application,
                ByVal ocompany As SAPbobsCOM.Company, Optional p_strCodProveedor As String = "")

        _companySBO = ocompany
        _applicationSBO = SBOAplication
        strCodProveedor = p_strCodProveedor
        NombreXml = Environment.CurrentDirectory + My.Resources.Resource.XMLFormularioSeleccionLineasRecepcion
        FormType = "SCGD_SLR"

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

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try
            CargarFormulario()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSBO)
        End Try
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try

            dtVehiculos = _formularioSBO.DataSources.DataTables.Add("dtVehiculos")

            dtVehiculos.Columns.Add("pedi", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("rece", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("unid", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("marc", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("esti", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("mode", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("vin", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("moto", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("tipo", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("cMar", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("cEst", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("cMod", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("ano", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("line", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("arti", BoFieldsType.ft_AlphaNumeric, 100)
            dtVehiculos.Columns.Add("col", BoFieldsType.ft_AlphaNumeric, 100)
           MatrizSeleccion = New MatrizSeleccionLineasRecepcion("mtxVeh", _formularioSBO, "dtVehiculos")
            MatrizSeleccion.CreaColumnas()
            MatrizSeleccion.LigaColumnas()

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
            dtSeleccionados.Columns.Add("code", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("cMar", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("cEst", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("cMod", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("ano", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("line", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("arti", BoFieldsType.ft_AlphaNumeric, 100)
            dtSeleccionados.Columns.Add("col", BoFieldsType.ft_AlphaNumeric, 100)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub CargarFormulario()
        Try
            CargarMatriz(strCodProveedor)
        Catch ex As Exception

        End Try
    End Sub

    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If Not pVal.FormTypeEx = "SCGD_SLR" Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                If pVal.ActionSuccess Then
                    Select Case pVal.ItemUID
                        Case "btnUpdate"
                            CargarMatriz("")
                        Case "btnAcept"
                            ButtonAceptarItemPressed(FormUID, pVal, BubbleEvent)
                    End Select
                Else
                    Select Case pVal.ItemUID
                        Case "btnAcept"
                            ButtonAceptarItemPressed(FormUID, pVal, BubbleEvent)
                    End Select
                End If

            ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then

                '   ManejadorEventosChooseFromList(FormUID, pVal, BubbleEvent)

            ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then

                '   ManejadorEventoCombos(FormUID, pVal, BubbleEvent)

            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try


    End Sub


End Class
