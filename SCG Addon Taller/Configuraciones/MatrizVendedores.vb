Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizVendedores : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaCodigo As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCodigo() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCodigo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCodigo = value
        End Set
    End Property

    Private _columnaNombre As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaNombre() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaNombre
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaNombre = value
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
    
    Public Overrides Sub CreaColumnas()

        _columnaSeleccionar = New ColumnaMatrixSBOCheckBox(Of String)("Col_SelV", True, "seleccionar", Me)
        _columnaCodigo = New ColumnaMatrixSBOEditText(Of String)("Col_CodV", True, "codigo", Me)
        _columnaNombre = New ColumnaMatrixSBOEditText(Of String)("Col_NomV", True, "vendedor", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccionar.AsignaBindingDataTable()
        _columnaCodigo.AsignaBindingDataTable()
        _columnaNombre.AsignaBindingDataTable()

    End Sub

End Class
