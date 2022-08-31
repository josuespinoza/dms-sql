Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBORecosteo : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaUnidad As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaUnidad() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaUnidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaUnidad = value
        End Set
    End Property

    Private _columnaMarca As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaMarca() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaMarca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaMarca = value
        End Set
    End Property

    Private _columnaEstilo As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaEstilo() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaEstilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaEstilo = value
        End Set
    End Property


    Private _columnaVIN As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaVIN() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaVIN
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaVIN = value
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

    Private _columnaDocumento As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaDocumento() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDocumento
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDocumento = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        _columnaSeleccionar = New ColumnaMatrixSBOCheckBox(Of String)("col_Sel", True, "seleccion", Me)
        _columnaUnidad = New ColumnaMatrixSBOEditText(Of String)("col_Unid", True, "unidad", Me)
        _columnaMarca = New ColumnaMatrixSBOEditText(Of String)("col_Mar", True, "marca", Me)
        _columnaEstilo = New ColumnaMatrixSBOEditText(Of String)("col_Est", True, "estilo", Me)
        _columnaVIN = New ColumnaMatrixSBOEditText(Of String)("col_Cont", True, "vin", Me)
        _columnaDocumento = New ColumnaMatrixSBOEditText(Of String)("col_DocEn", True, "doc", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccionar.AsignaBindingDataTable()
        _columnaUnidad.AsignaBindingDataTable()
        _columnaMarca.AsignaBindingDataTable()
        _columnaEstilo.AsignaBindingDataTable()
        _columnaVIN.AsignaBindingDataTable()
        _columnaDocumento.AsignaBindingDataTable()

    End Sub

End Class
