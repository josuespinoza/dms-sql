Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

'Clase para control y liga a fuente de datos de columnas de matriz de pagos a reversar en pantalla de préstamo

Public Class MatrixSBOPagosReversar : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaNumero As ColumnaMatrixSBOEditText(Of Integer)

    Public Property ColumnaNumero() As ColumnaMatrixSBOEditText(Of Integer)
        Get
            Return _columnaNumero
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Integer))
            _columnaNumero = value
        End Set
    End Property

    Private _columnaFecha As ColumnaMatrixSBOEditText(Of Date)

    Public Property ColumnaFecha() As ColumnaMatrixSBOEditText(Of Date)
        Get
            Return _columnaFecha
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Date))
            _columnaFecha = value
        End Set
    End Property

    Private _columnaCuota As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCuota() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCuota
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCuota = value
        End Set
    End Property

    Private _columnaCapital As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCapital() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCapital
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCapital = value
        End Set
    End Property

    Private _columnaInteres As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaInteres() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaInteres
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaInteres = value
        End Set
    End Property

    Private _columnaIntMora As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaIntMora() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaIntMora
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaIntMora = value
        End Set
    End Property

    Private _columnaCapPend As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCapPend() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCapPend
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCapPend = value
        End Set
    End Property

    Private _columnaIntPend As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaIntPend() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaIntPend
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaIntPend = value
        End Set
    End Property

    Private _columnaDiasInt As ColumnaMatrixSBOEditText(Of Integer)

    Public Property ColumnaDiasInt() As ColumnaMatrixSBOEditText(Of Integer)
        Get
            Return _columnaDiasInt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Integer))
            _columnaDiasInt = value
        End Set
    End Property

    Private _columnaMoraPend As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaMoraPend() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaMoraPend
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaMoraPend = value
        End Set
    End Property

    Private _columnaDiasMora As ColumnaMatrixSBOEditText(Of Integer)

    Public Property ColumnaDiasMora() As ColumnaMatrixSBOEditText(Of Integer)
        Get
            Return _columnaDiasMora
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Integer))
            _columnaDiasMora = value
        End Set
    End Property

    'Liga columnas a DataTable

    Public Overrides Sub LigaColumnas()
        _columnaNumero.AsignaBindingDataTable()
        _columnaFecha.AsignaBindingDataTable()
        _columnaCuota.AsignaBindingDataTable()
        _columnaCapital.AsignaBindingDataTable()
        _columnaInteres.AsignaBindingDataTable()
        _columnaIntMora.AsignaBindingDataTable()
        _columnaCapPend.AsignaBindingDataTable()
        _columnaIntPend.AsignaBindingDataTable()
        _columnaDiasInt.AsignaBindingDataTable()
        _columnaMoraPend.AsignaBindingDataTable()
        _columnaDiasMora.AsignaBindingDataTable()
    End Sub

    'Crea liga de columnas con campos especificos del DataTable

    Public Overrides Sub CreaColumnas()
        _columnaNumero = New ColumnaMatrixSBOEditText(Of Integer)("col_Num", True, "numero", Me)
        _columnaFecha = New ColumnaMatrixSBOEditText(Of Date)("col_Fecha", True, "fecha", Me)
        _columnaCuota = New ColumnaMatrixSBOEditText(Of Decimal)("col_Cuota", True, "cuota", Me)
        _columnaCapital = New ColumnaMatrixSBOEditText(Of Decimal)("col_Cap", True, "capital", Me)
        _columnaInteres = New ColumnaMatrixSBOEditText(Of Decimal)("col_Int", True, "interes", Me)
        _columnaIntMora = New ColumnaMatrixSBOEditText(Of Decimal)("col_Mora", True, "intMora", Me)
        _columnaCapPend = New ColumnaMatrixSBOEditText(Of Decimal)("col_Cap_Pe", True, "capPend", Me)
        _columnaIntPend = New ColumnaMatrixSBOEditText(Of Decimal)("col_Int_Pe", True, "intPend", Me)
        _columnaDiasInt = New ColumnaMatrixSBOEditText(Of Integer)("col_Dia_In", True, "diasInt", Me)
        _columnaMoraPend = New ColumnaMatrixSBOEditText(Of Decimal)("col_Mor_Pe", True, "moraPend", Me)
        _columnaDiasMora = New ColumnaMatrixSBOEditText(Of Integer)("col_Dia_Mo", True, "diasMora", Me)
    End Sub

End Class
