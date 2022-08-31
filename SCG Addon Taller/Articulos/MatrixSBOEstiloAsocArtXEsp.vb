Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBOEstiloAsocArtXEsp
    Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub


    Dim _columnaSeleccion As ColumnaMatrixSBOCheckBox(Of String)

    Public Property columnaSeleccion As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSeleccion
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSeleccion = value
        End Set
    End Property

    Dim _columnaCodeEstilo As ColumnaMatrixSBOEditText(Of Integer)
    Public Property columnaCodeEstilo As ColumnaMatrixSBOEditText(Of Integer)
        Get
            Return _columnaCodeEstilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Integer))
            _columnaCodeEstilo = value
        End Set
    End Property

    Dim _columnaDescEstilo As ColumnaMatrixSBOEditText(Of String)
    Public Property columnaDescEstilo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDescEstilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDescEstilo = value
        End Set
    End Property

    Dim _columnaDuracion As ColumnaMatrixSBOEditText(Of Decimal)
    Public Property columnaDuracion As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaDuracion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaDuracion = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        _columnaSeleccion = New ColumnaMatrixSBOCheckBox(Of String)("Col_SelE", True, "selec", Me)
        _columnaCodeEstilo = New ColumnaMatrixSBOEditText(Of Integer)("Col_CodE", True, "cod", Me)
        _columnaDuracion = New ColumnaMatrixSBOEditText(Of Decimal)("Col_DurE", True, "duraE", Me)
        _columnaDescEstilo = New ColumnaMatrixSBOEditText(Of String)("Col_DesE", True, "desc", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccion.AsignaBindingDataTable()
        _columnaCodeEstilo.AsignaBindingDataTable()
        _columnaDuracion.AsignaBindingDataTable()
        _columnaDescEstilo.AsignaBindingDataTable()

    End Sub
End Class
