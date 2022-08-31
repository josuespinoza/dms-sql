Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrixSBOModeloAsocArtXEsp
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

    Dim _columnaCodeModelo As ColumnaMatrixSBOEditText(Of Integer)
    Public Property columnaCodeModelo As ColumnaMatrixSBOEditText(Of Integer)
        Get
            Return _columnaCodeModelo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Integer))
            _columnaCodeModelo = value
        End Set
    End Property

    Dim _columnaDescModelo As ColumnaMatrixSBOEditText(Of String)
    Public Property columnaDescModelo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaDescModelo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaDescModelo = value
        End Set
    End Property

    Dim _columnaDuracion As ColumnaMatrixSBOEditText(Of Single)
    Public Property columnaDuracion As ColumnaMatrixSBOEditText(Of Single)
        Get
            Return _columnaDuracion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Single))
            _columnaDuracion = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        _columnaSeleccion = New ColumnaMatrixSBOCheckBox(Of String)("Col_SelM", True, "selec", Me)
        _columnaCodeModelo = New ColumnaMatrixSBOEditText(Of Integer)("Col_CodM", True, "cod", Me)
        _columnaDescModelo = New ColumnaMatrixSBOEditText(Of String)("Col_DesM", True, "desc", Me)
        _columnaDuracion = New ColumnaMatrixSBOEditText(Of Single)("Col_DurM", True, "duraE", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccion.AsignaBindingDataTable()
        _columnaCodeModelo.AsignaBindingDataTable()
        _columnaDescModelo.AsignaBindingDataTable()
        _columnaDuracion.AsignaBindingDataTable()

    End Sub

End Class
