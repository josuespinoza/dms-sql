Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizBusquedaArticulosCitas : Inherits MatrixSBO

    Public Sub New(ByVal uniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(uniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub
#Region "Declaraciones"

    Private _ColSeleccionar As ColumnaMatrixSBOEditText(Of String)
    Private _ColCodigo As ColumnaMatrixSBOEditText(Of String)
    Private _ColDescripcion As ColumnaMatrixSBOEditText(Of String)
    Private _ColBodega As ColumnaMatrixSBOEditText(Of String)
    Private _ColCantSto As ColumnaMatrixSBOEditText(Of String)
    Private _ColCant As ColumnaMatrixSBOEditText(Of String)
    Private _ColPrecio As ColumnaMatrixSBOEditText(Of String)
    Private _ColMoneda As ColumnaMatrixSBOEditText(Of String)
    Private _ColDuracion As ColumnaMatrixSBOEditText(Of String)
    Private _ColNoFas As ColumnaMatrixSBOEditText(Of String)
    Private _ColCodeBar As ColumnaMatrixSBOEditText(Of String)
    

#End Region


#Region "Propiedades"

    Public Property ColSeleccionar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColSeleccionar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColSeleccionar = value
        End Set
    End Property

    Public Property ColCodigo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCodigo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCodigo = value
        End Set
    End Property

    Public Property ColDescripcion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColDescripcion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColDescripcion = value
        End Set
    End Property

    Public Property ColBodega As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColBodega
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColBodega = value
        End Set
    End Property

    Public Property ColCantSto As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCantSto
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCantSto = value
        End Set
    End Property

    Public Property ColCantidad As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCant
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCant = value
        End Set
    End Property

    Public Property ColPrecio As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColPrecio
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColPrecio = value
        End Set
    End Property

    Public Property ColMoneda As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColMoneda
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColMoneda = value
        End Set
    End Property

    Public Property ColDuracion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColDuracion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColDuracion = value
        End Set
    End Property

    Public Property ColNoFase As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColNoFas
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColNoFas = value
        End Set
    End Property

    Public Property ColCodeBar As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _ColCodeBar
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _ColCodeBar = value
        End Set
    End Property

#End Region


    Public Overrides Sub CreaColumnas()

        _ColSeleccionar = New ColumnaMatrixSBOEditText(Of String)("Col_Sel", True, "Sel", Me)
        _ColCodigo = New ColumnaMatrixSBOEditText(Of String)("Col_Cod", True, "Cod", Me)
        _ColDescripcion = New ColumnaMatrixSBOEditText(Of String)("Col_Desc", True, "Desc", Me)
        _ColBodega = New ColumnaMatrixSBOEditText(Of String)("Col_Bod", True, "Bod", Me)
        _ColCantSto = New ColumnaMatrixSBOEditText(Of String)("Col_Stock", True, "Stock", Me)
        _ColCant = New ColumnaMatrixSBOEditText(Of String)("Col_Cant", True, "Cant", Me)
        _ColPrecio = New ColumnaMatrixSBOEditText(Of String)("Col_Prec", True, "Prec", Me)
        _ColMoneda = New ColumnaMatrixSBOEditText(Of String)("Col_Mon", True, "Mon", Me)
        _ColNoFas = New ColumnaMatrixSBOEditText(Of String)("Col_NoF", True, "NoF", Me)
        _ColDuracion = New ColumnaMatrixSBOEditText(Of String)("Col_Dur", True, "Dur", Me)
        _ColCodeBar = New ColumnaMatrixSBOEditText(Of String)("Col_CodBar", True, "CodBar", Me)

    End Sub

    Public Overrides Sub LigaColumnas()
        ColSeleccionar.AsignaBindingDataTable()
        ColCodigo.AsignaBindingDataTable()
        ColDescripcion.AsignaBindingDataTable()
        ColBodega.AsignaBindingDataTable()
        ColCantSto.AsignaBindingDataTable()
        ColCantidad.AsignaBindingDataTable()
        ColPrecio.AsignaBindingDataTable()
        ColMoneda.AsignaBindingDataTable()
        ColNoFase.AsignaBindingDataTable()
        ColDuracion.AsignaBindingDataTable()
        ColCodeBar.AsignaBindingDataTable()
    End Sub
End Class
