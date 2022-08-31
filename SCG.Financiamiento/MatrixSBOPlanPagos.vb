Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

'Clase que controla y liga a fuente de datos las columnas de la matriz de pagos de los planes de pagos

Public Class MatrixSBOPlanPagos
    :
    Inherits MatrixSBO


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

    Private _columnaSaldoInicial As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaSaldoInicial() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaSaldoInicial
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaSaldoInicial = value
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

    Private _columnaSaldoFinal As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaFinal() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaSaldoFinal
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaSaldoFinal = value
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

    Private _columnaPagado As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaPagado() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPagado
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPagado = value
        End Set
    End Property

    Private _columnaNotaCred As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaNotaCred() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaNotaCred
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaNotaCred = value
        End Set
    End Property

    Private _columnaDocInt As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDocInt() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDocInt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDocInt = value
        End Set
    End Property

    Private _columnaDocFac As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDocFac() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDocFac
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDocFac = value
        End Set
    End Property

    Private _columnaBorrador As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaBorrador() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaBorrador
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaBorrador = value
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

    Private _columnaMoraPend As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaMoraPend() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaMoraPend
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaMoraPend = value
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
        _columnaSaldoInicial.AsignaBindingDataTable()
        _columnaCuota.AsignaBindingDataTable()
        _columnaCapital.AsignaBindingDataTable()
        _columnaInteres.AsignaBindingDataTable()
        _columnaSaldoFinal.AsignaBindingDataTable()
        _columnaIntMora.AsignaBindingDataTable()
        _columnaPagado.AsignaBindingDataTable()
        _columnaNotaCred.AsignaBindingDataTable()
        _columnaDocInt.AsignaBindingDataTable()
        _columnaDocFac.AsignaBindingDataTable()
        _columnaBorrador.AsignaBindingDataTable()
        _columnaCapPend.AsignaBindingDataTable()
        _columnaIntPend.AsignaBindingDataTable()
        _columnaMoraPend.AsignaBindingDataTable()
        _columnaDiasInt.AsignaBindingDataTable()
        _columnaDiasMora.AsignaBindingDataTable()
    End Sub

    'Crea liga de columnas a campos especificos del DataTable

    Public Overrides Sub CreaColumnas()
        _columnaNumero = New ColumnaMatrixSBOEditText(Of Integer)("col_Numero", True, "numero", Me)
        _columnaFecha = New ColumnaMatrixSBOEditText(Of Date)("col_Fecha", True, "fecha", Me)
        _columnaSaldoInicial = New ColumnaMatrixSBOEditText(Of Decimal)("col_Saldo", True, "saldoInicial", Me)
        _columnaCuota = New ColumnaMatrixSBOEditText(Of Decimal)("col_Cuota", True, "cuota", Me)
        _columnaCapital = New ColumnaMatrixSBOEditText(Of Decimal)("col_Capita", True, "capital", Me)
        _columnaInteres = New ColumnaMatrixSBOEditText(Of Decimal)("col_Intere", True, "interes", Me)
        _columnaSaldoFinal = New ColumnaMatrixSBOEditText(Of Decimal)("col_Final", True, "saldoFinal", Me)
        _columnaIntMora = New ColumnaMatrixSBOEditText(Of Decimal)("col_IntMor", True, "intMora", Me)
        _columnaPagado = New ColumnaMatrixSBOEditText(Of String)("col_Pagado", True, "pagado", Me)
        _columnaNotaCred = New ColumnaMatrixSBOEditText(Of String)("col_CredCa", True, "notaCred", Me)
        _columnaDocInt = New ColumnaMatrixSBOEditText(Of String)("col_DocInt", True, "docInt", Me)
        _columnaDocFac = New ColumnaMatrixSBOEditText(Of String)("col_DocFac", True, "docFac", Me)
        _columnaBorrador = New ColumnaMatrixSBOEditText(Of String)("col_Bor", True, "borrador", Me)
        _columnaCapPend = New ColumnaMatrixSBOEditText(Of Decimal)("col_CapPen", True, "capPend", Me)
        _columnaIntPend = New ColumnaMatrixSBOEditText(Of Decimal)("col_IntPen", True, "intPend", Me)
        _columnaMoraPend = New ColumnaMatrixSBOEditText(Of Decimal)("col_MorPen", True, "moraPend", Me)
        _columnaDiasInt = New ColumnaMatrixSBOEditText(Of Integer)("col_DiaInt", True, "diasInt", Me)
        _columnaDiasMora = New ColumnaMatrixSBOEditText(Of Integer)("col_DiaMor", True, "diasMora", Me)
    End Sub
End Class
