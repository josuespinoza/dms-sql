
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizTramites : Inherits MatrixSBO

#Region "New"
    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub
#End Region


#Region "Propiedades de columnas"

    'columna codigo 
    Private _columnaCol_CodTra As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_CodTra() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_CodTra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_CodTra = value
        End Set
    End Property

    'columna Descripcion 
    Private _columnaCol_DesTra As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_DesTra() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_DesTra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_DesTra = value
        End Set
    End Property

    'columna valor 
    Private _columnaCol_ValTra As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_ValTra() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_ValTra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_ValTra = value
        End Set
    End Property

    'columna costo
    Private _columnaCol_CosTra As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_CosTra() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CosTra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CosTra = value
        End Set
    End Property

    'columna utilidad 
    Private _columnaCol_UtilTra As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_UtilTra() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_UtilTra
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_UtilTra = value
        End Set
    End Property

    'columna %utilidad
    Private _columnaCol_PUtiT As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaColPUtiT As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_PUtiT
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_PUtiT = value
        End Set
    End Property

#End Region

#Region "Metodos"
    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        _columnaCol_CodTra = New ColumnaMatrixSBOEditText(Of String)("Col_Cod", True, "codigo", Me)
        _columnaCol_DesTra = New ColumnaMatrixSBOEditText(Of String)("Col_Des", True, "descripcion", Me)
        _columnaCol_ValTra = New ColumnaMatrixSBOEditText(Of Decimal)("Col_PreTra", True, "valor", Me)
        _columnaCol_CosTra = New ColumnaMatrixSBOEditText(Of Decimal)("Col_CosTra", True, "costo", Me)
        _columnaCol_UtilTra = New ColumnaMatrixSBOEditText(Of Decimal)("Col_UtiTra", True, "utilidad", Me)
        _columnaCol_PUtiT = New ColumnaMatrixSBOEditText(Of Decimal)("Col_PUtiT", True, "putilidad", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        _columnaCol_CodTra.AsignaBindingDataTable()
        _columnaCol_DesTra.AsignaBindingDataTable()
        _columnaCol_ValTra.AsignaBindingDataTable()
        _columnaCol_CosTra.AsignaBindingDataTable()
        _columnaCol_UtilTra.AsignaBindingDataTable()
        _columnaCol_PUtiT.AsignaBindingDataTable()
    End Sub
#End Region

End Class
