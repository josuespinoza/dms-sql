Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Namespace GastosContratoVentas

    Partial Public Class GastosAdicionales : Implements IFormularioSBO

        Private Shared _formType As String
        Private _nombreXml As String
        Private _titulo As String
        Private _formularioSbo As IForm
        Private _inicializado As Boolean
        Private _applicationSbo As Application
        Private _companySbo As ICompany
        Private Shared _formContrato As Form

        Public Shared MatrixGastosPantalla As MatrixSBOGastos
        Public ButtonOk As ButtonSBO

        Private Shared dataTablePantGastos As DataTable
        Private Shared dataTableLineasSum As DataTable

        Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
            _companySbo = companySbo
            _applicationSbo = application
        End Sub

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

        Public Property FormType() As String Implements IFormularioSBO.FormType
            Get
                Return _formType
            End Get
            Set(ByVal value As String)
                _formType = value
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

        Public Property FormContrato() As SAPbouiCOM.Form
            Get
                Return _formContrato
            End Get
            Set(ByVal value As SAPbouiCOM.Form)
                _formContrato = value
            End Set
        End Property

        Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oColumn As SAPbouiCOM.Column

            If FormularioSBO IsNot Nothing Then

                oMatrix = DirectCast(FormularioSBO.Items.Item("mtx_Gastos").Specific, SAPbouiCOM.Matrix)

                For Each oColumn In oMatrix.Columns

                    oColumn.AffectsFormMode = False

                Next

            End If

        End Sub

        Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

            'DataTable de la Tabla @SCGD_CONFLINEASSUM si DataTable de Contrato de Ventas está vacío
            dataTableLineasSum = FormularioSBO.DataSources.DataTables.Add("LineasSum")

            'DataTable ligado a Pantalla de Gastos de Unidad
            dataTablePantGastos = FormularioSBO.DataSources.DataTables.Add("PantallaGastos")
            dataTablePantGastos.Columns.Add("codigo", BoFieldsType.ft_AlphaNumeric, 100)
            dataTablePantGastos.Columns.Add("descrip", BoFieldsType.ft_AlphaNumeric, 100)
            dataTablePantGastos.Columns.Add("monto", BoFieldsType.ft_Float, 100)

            MatrixGastosPantalla = New MatrixSBOGastos("mtx_Gastos", FormularioSBO, "PantallaGastos")
            MatrixGastosPantalla.CreaColumnas()
            MatrixGastosPantalla.LigaColumnas()

            ButtonOk = New ButtonSBO("1", FormularioSBO)

        End Sub

        Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

            If Not pVal.FormTypeEx = FormType Then Return

            If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                If pVal.ItemUID = "1" Then

                    ButtonSBOOk(FormUID, pVal)

                End If

            End If

        End Sub

    End Class

End Namespace