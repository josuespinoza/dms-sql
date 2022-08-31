Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBOFacturas : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaFacturaVieja As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaFacturaVieja() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaFacturaVieja
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaFacturaVieja = value
        End Set
    End Property

    Private _columnaNotaCreditoReversa As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaNotaCreditoReversa() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaNotaCreditoReversa
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaNotaCreditoReversa = value
        End Set
    End Property

    Private _columnaFacturaNueva As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaFacturaNueva() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaFacturaNueva
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaFacturaNueva = value
        End Set
    End Property


    Private _columnaNoContrato As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaNoContrato() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaNoContrato
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaNoContrato = value
        End Set
    End Property

    Private _columnaSeleccionar As ColumnaMatrixSBOCheckBox(Of String)

    Public Property ColumnaSeleccionar() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSeleccionar
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSeleccionar = value
        End Set
    End Property


    Private _columnaFechaContabilizacion As ColumnaMatrixSBOEditText(Of Date)

    Public Property ColumnaFechaContabilizacion() As ColumnaMatrixSBOEditText(Of Date)
        Get
            Return _columnaFechaContabilizacion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Date))
            _columnaFechaContabilizacion = value
        End Set
    End Property


    Public Overrides Sub CreaColumnas()

        _columnaSeleccionar = New ColumnaMatrixSBOCheckBox(Of String)("col_Refac", True, "col_Refac", Me)
        '_columnaSeleccionar.Columna.ValOff = "0"
        '_columnaSeleccionar.Columna.ValOn = "1"

        _columnaNoContrato = New ColumnaMatrixSBOEditText(Of String)("col_CV", True, "NoContrato", Me)
        _columnaFechaContabilizacion = New ColumnaMatrixSBOEditText(Of Date)("col_FCont", True, "FechaContabilizacion", Me)

        _columnaFacturaVieja = New ColumnaMatrixSBOEditText(Of String)("col_Fact", True, "vieja", Me)
        _columnaNotaCreditoReversa = New ColumnaMatrixSBOEditText(Of String)("col_NCR", True, "reversa", Me)
        _columnaFacturaNueva = New ColumnaMatrixSBOEditText(Of String)("col_NF", True, "nueva", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccionar.AsignaBindingDataTable()
        _columnaNoContrato.AsignaBindingDataTable()
        _columnaFechaContabilizacion.AsignaBindingDataTable()

        _columnaFacturaVieja.AsignaBindingDataTable()
        _columnaNotaCreditoReversa.AsignaBindingDataTable()
        _columnaFacturaNueva.AsignaBindingDataTable()
    End Sub

End Class
