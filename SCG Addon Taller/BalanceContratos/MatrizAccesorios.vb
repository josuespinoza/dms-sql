'Matriz de accesorios en la pantalla 
'Balance de Contratos de ventas

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizAccesorios
    : Inherits MatrixSBO

#Region "New"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Propiedades de columnas"

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

    'columna Descripcion 
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Des() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    'columna valor 
    Private _columnaCol_ValAcc As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_ValAcc() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_ValAcc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_ValAcc = value
        End Set
    End Property

    'columna costo
    Private _columnaCol_CosAcc As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_CosAcc() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CosAcc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CosAcc = value
        End Set
    End Property

    'columna utilidad 
    Private _columnaCol_UtilAcc As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_UtilAcc() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_UtilAcc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_UtilAcc = value
        End Set
    End Property

    'columna %utilidad
    Private _columnaCol_PUtiA As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaColPUtiA As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_PUtiA
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_PUtiA = value
        End Set
    End Property

    Private _columnaCol_Desc As ColumnaMatrixSBOEditText(Of Decimal)
    Public Property ColumnaColDesc As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _ColumnaCol_Desc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _ColumnaCol_Desc = value
        End Set
    End Property


    Private _columnaCol_PreLis As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_PreLis As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_PreLis
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_PreLis = value
        End Set
    End Property

#End Region

#Region "Metodos"

    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        _columnaCol_Cod = New ColumnaMatrixSBOEditText(Of String)("Col_Cod", True, "codigo", Me)
        _columnaCol_Des = New ColumnaMatrixSBOEditText(Of String)("Col_Des", True, "descripcion", Me)
        _columnaCol_ValAcc = New ColumnaMatrixSBOEditText(Of Decimal)("Col_ValAcc", True, "valor", Me)
        _columnaCol_CosAcc = New ColumnaMatrixSBOEditText(Of Decimal)("Col_CosAcc", True, "costo", Me)
        _columnaCol_UtilAcc = New ColumnaMatrixSBOEditText(Of Decimal)("Col_UtiAcc", True, "utilidad", Me)
        _columnaCol_PUtiA = New ColumnaMatrixSBOEditText(Of Decimal)("Col_PUtiA", True, "putilidad", Me)
        _columnaCol_PreLis = New ColumnaMatrixSBOEditText(Of Decimal)("Col_PreLis", True, "prelis", Me)
        _columnaCol_Desc = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Desc", True, "desc", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        _columnaCol_Cod.AsignaBindingDataTable()
        _columnaCol_Des.AsignaBindingDataTable()
        _columnaCol_ValAcc.AsignaBindingDataTable()
        _columnaCol_CosAcc.AsignaBindingDataTable()
        _columnaCol_UtilAcc.AsignaBindingDataTable()
        _columnaCol_PUtiA.AsignaBindingDataTable()
        _columnaCol_PreLis.AsignaBindingDataTable()
        _columnaCol_Desc.AsignaBindingDataTable()
    End Sub

#End Region

End Class
