Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizLineasDimensionesOT : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaMarca As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaMarca() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMarca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMarca = value
        End Set
    End Property

    Private _columnaDescripcion As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDescripcion() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDescripcion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDescripcion = value
        End Set
    End Property

    Private _columnaDim1 As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDim1() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDim1
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDim1 = value
        End Set
    End Property


    Private _columnaDim2 As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDim2() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDim2
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDim2 = value
        End Set
    End Property

    Private _columnaDim3 As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDim3() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDim3
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDim3 = value
        End Set
    End Property

    Private _columnaDim4 As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDim4() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDim4
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDim4 = value
        End Set
    End Property

    Private _columnaDim5 As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDim5() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDim5
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDim5 = value
        End Set
    End Property


    Public Overrides Sub CreaColumnas()

        _columnaDim1 = New ColumnaMatrixSBOEditText(Of String)("colDim1", True, "U_Dim1", Me)
        _columnaDim2 = New ColumnaMatrixSBOEditText(Of String)("colDim2", True, "U_Dim2", Me)
        _columnaDim3 = New ColumnaMatrixSBOEditText(Of String)("colDim3", True, "U_Dim3", Me)
        _columnaDim4 = New ColumnaMatrixSBOEditText(Of String)("colDim4", True, "U_Dim4", Me)
        _columnaDim5 = New ColumnaMatrixSBOEditText(Of String)("colDim5", True, "U_Dim5", Me)
        _columnaMarca = New ColumnaMatrixSBOEditText(Of String)("colMarc", True, "U_CodMar", Me)
        _columnaDescripcion = New ColumnaMatrixSBOEditText(Of String)("colDesTipo", True, "U_DesMar", Me)

    End Sub

    Public Overrides Sub LigaColumnas()


        _columnaMarca.AsignaBindingDataTable()
        _columnaDescripcion.AsignaBindingDataTable()
        _columnaDim1.AsignaBindingDataTable()
        _columnaDim2.AsignaBindingDataTable()
        _columnaDim3.AsignaBindingDataTable()
        _columnaDim4.AsignaBindingDataTable()
        _columnaDim5.AsignaBindingDataTable()


    End Sub
End Class
