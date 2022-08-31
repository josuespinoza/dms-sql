
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizRptOrdenesXEstado : Inherits MatrixSBO


    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    'columna codigo 
    Private _columnaCol_Sel As ColumnaMatrixSBOCheckBox(Of String)

    Public Property ColumnaCol_Sel() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaCol_Sel
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaCol_Sel = value
        End Set
    End Property

    'columna codigo 
    Private _columnaCol_Cod As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Cod() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cod = value
        End Set
    End Property

    'columna codigo 
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Des() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()
        _columnaCol_Sel = New ColumnaMatrixSBOCheckBox(Of String)("Col_sel", True, "sel", Me)
        _columnaCol_Cod = New ColumnaMatrixSBOEditText(Of String)("Col_cod", True, "cod", Me)
        _columnaCol_Des = New ColumnaMatrixSBOEditText(Of String)("Col_des", True, "des", Me)
    End Sub

    Public Overrides Sub LigaColumnas()
        _columnaCol_Sel.AsignaBindingDataTable()
        _columnaCol_Cod.AsignaBindingDataTable()
        _columnaCol_Des.AsignaBindingDataTable()
    End Sub
End Class
