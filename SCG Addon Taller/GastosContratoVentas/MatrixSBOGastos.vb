Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBOGastos : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaCodItem As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCodItem() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCodItem
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCodItem = value
        End Set
    End Property

    Private _columnaDesItem As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDesItem() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDesItem
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDesItem = value
        End Set
    End Property

    Private _columnaMonto As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaMonto() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaMonto
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaMonto = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()
        _columnaCodItem = New ColumnaMatrixSBOEditText(Of String)("col_Cod", True, "codigo", Me)
        _columnaDesItem = New ColumnaMatrixSBOEditText(Of String)("col_Nom", True, "descrip", Me)
        _columnaMonto = New ColumnaMatrixSBOEditText(Of Decimal)("col_Mon", True, "monto", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        _columnaCodItem.AsignaBindingDataTable()
        _columnaDesItem.AsignaBindingDataTable()
        _columnaMonto.AsignaBindingDataTable()
    End Sub

End Class
