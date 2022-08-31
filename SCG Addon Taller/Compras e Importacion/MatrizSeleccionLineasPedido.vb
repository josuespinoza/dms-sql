Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizSeleccionLineasPedido : Inherits MatrixSBO


    Private _columnaPed As ColumnaMatrixSBOEditText(Of String)
    Private _columnaArt As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCAr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaAno As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCan As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPen As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCPr As ColumnaMatrixSBOEditText(Of String)
    Private _columnaPro As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCodCol As ColumnaMatrixSBOEditText(Of String)
    Private _columnaline As ColumnaMatrixSBOEditText(Of String)
    Private _columnaMon As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCurr As ColumnaMatrixSBOEditText(Of String)

    'Private _columnaAsi As ColumnaMatrixSBOEditText(Of String)
    'Private _columnaSta As ColumnaMatrixSBOEditText(Of String)
    'Private _columnaCod As ColumnaMatrixSBOEditText(Of String)


    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Public Property ColumnaPed As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPed
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPed = value
        End Set
    End Property

    Public Property ColumnaCodArt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCAr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCAr = value
        End Set
    End Property

    Public Property ColumnaArt As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaArt
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaArt = value
        End Set
    End Property

 

    Public Property ColumnaAno As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaAno
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaAno = value
        End Set
    End Property

    Public Property ColumnaCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol = value
        End Set
    End Property

    Public Property ColumnaCan As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCan
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCan = value
        End Set
    End Property

    Public Property ColumnaPen As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPen
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPen = value
        End Set
    End Property

    Public Property ColumnaCodPro As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCPr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCPr = value
        End Set
    End Property

    Public Property ColumnaPro As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaPro
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaPro = value
        End Set
    End Property

    Public Property ColumnaCodCol As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCodCol
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCodCol = value
        End Set
    End Property

    Public Property ColumnaLine As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaline
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaline = value
        End Set
    End Property

    Public Property ColumnaMon As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaMon
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaMon = value
        End Set
    End Property

    Public Property ColumnaCurr As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCurr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCurr = value
        End Set
    End Property


    Public Overrides Sub CreaColumnas()

        Try
            ColumnaPed = New ColumnaMatrixSBOEditText(Of String)("col_Ped", True, "pedi", Me)
            ColumnaArt = New ColumnaMatrixSBOEditText(Of String)("col_Art", True, "arti", Me)
            ColumnaCodArt = New ColumnaMatrixSBOEditText(Of String)("col_CAr", True, "cart", Me)
            ColumnaAno = New ColumnaMatrixSBOEditText(Of String)("col_Ano", True, "ano", Me)
            ColumnaCol = New ColumnaMatrixSBOEditText(Of String)("col_Col", True, "colo", Me)
            ColumnaCan = New ColumnaMatrixSBOEditText(Of String)("col_Can", True, "cant", Me)
            ColumnaPen = New ColumnaMatrixSBOEditText(Of String)("col_Pen", True, "pend", Me)
            ColumnaCodPro = New ColumnaMatrixSBOEditText(Of String)("col_CPr", True, "cpro", Me)
            ColumnaPro = New ColumnaMatrixSBOEditText(Of String)("col_Pro", True, "prov", Me)
            ColumnaMon = New ColumnaMatrixSBOEditText(Of Decimal)("col_Mon", True, "mont", Me)
            ColumnaCodCol = New ColumnaMatrixSBOEditText(Of String)("col_CCol", True, "codCol", Me)
            ColumnaLine = New ColumnaMatrixSBOEditText(Of String)("col_Line", True, "line", Me)
            ColumnaCurr = New ColumnaMatrixSBOEditText(Of String)("col_Curr", True, "curr", Me)

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Overrides Sub LigaColumnas()

        ColumnaPed.AsignaBindingDataTable()
        ColumnaCodArt.AsignaBindingDataTable()
        ColumnaArt.AsignaBindingDataTable()
        ColumnaAno.AsignaBindingDataTable()
        ColumnaCol.AsignaBindingDataTable()
        ColumnaCan.AsignaBindingDataTable()
        ColumnaPen.AsignaBindingDataTable()
        ColumnaCodPro.AsignaBindingDataTable()
        ColumnaPro.AsignaBindingDataTable()
        ColumnaMon.AsignaBindingDataTable()
        ColumnaCodCol.AsignaBindingDataTable()
        ColumnaLine.AsignaBindingDataTable()
        ColumnaCurr.AsignaBindingDataTable()

    End Sub


End Class
