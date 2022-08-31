Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizTipoInventario : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

    Private _columnaCodigoTI As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCodigoTI() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCodigoTI
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCodigoTI = value
        End Set
    End Property

    Private _columnaNombreTI As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaNombreTI() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaNombreTI
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaNombreTI = value
        End Set
    End Property

    Private _columnaSeleccionarTI As ColumnaMatrixSBOCheckBox(Of String)

    Public Property ColumnaSeleccionarTI() As ColumnaMatrixSBOCheckBox(Of String)
        Get
            Return _columnaSeleccionarTI
        End Get
        Set(ByVal value As ColumnaMatrixSBOCheckBox(Of String))
            _columnaSeleccionarTI = value
        End Set
    End Property

    Public Overrides Sub CreaColumnas()

        _columnaSeleccionarTI = New ColumnaMatrixSBOCheckBox(Of String)("Col_SelTI", True, "seleccionar", Me)
        _columnaCodigoTI = New ColumnaMatrixSBOEditText(Of String)("Col_CodTI", True, "codigo", Me)
        _columnaNombreTI = New ColumnaMatrixSBOEditText(Of String)("Col_TI", True, "ti", Me)

    End Sub

    Public Overrides Sub LigaColumnas()

        _columnaSeleccionarTI.AsignaBindingDataTable()
        _columnaCodigoTI.AsignaBindingDataTable()
        _columnaNombreTI.AsignaBindingDataTable()

    End Sub

End Class
