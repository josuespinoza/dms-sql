
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizServicios : Inherits MatrixSBO


#Region "New"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Propiedades de columnas"

    Private _columnaCol_Code As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Des As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Quan As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Cur As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Pri As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Tot As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Imp As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Lin As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Tip As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Dur As ColumnaMatrixSBOEditText(Of Decimal)
    Private _columnaCol_Hij As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Pad As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Paq As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_Barr As ColumnaMatrixSBOEditText(Of String)

    'columna codigo 
    Public Property ColumnaCol_Code() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Code
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Code = value
        End Set
    End Property

    'columna codigo de Barras
    Public Property ColumnaCol_Barra() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Barr
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Barr = value
        End Set
    End Property

    'columna Descripcion 
    Public Property ColumnaCol_Des() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Des
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Des = value
        End Set
    End Property

    'columna Quantity 
    Public Property ColumnaCol_Quan() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Quan
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Quan = value
        End Set
    End Property

    'columna Currency
    Public Property ColumnaCol_Cur() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cur
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cur = value
        End Set
    End Property

    'columna Precio 
    Public Property ColumnaCol_Pri() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Pri
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Pri = value
        End Set
    End Property

    'columna Total 
    Public Property ColumnaCol_Tot() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Tot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Tot = value
        End Set
    End Property

    'Columna Impuesto    
    Public Property ColumnaCol_Imp() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Imp
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Imp = value
        End Set
    End Property

    'columna Numero de linea
  Public Property ColumnaCol_Lin() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Lin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Lin = value
        End Set
    End Property

    'columna Tipo Articulo
    Public Property ColumnaCol_Tip() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Tip
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Tip = value
        End Set
    End Property

    'columna Duracion
    Public Property ColumnaCol_Dur() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Dur
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Dur = value
        End Set
    End Property

    'columna Hijo de Paquete
    Public Property ColumnaCol_Hij() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Hij
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Hij = value
        End Set
    End Property

    'columna Codigo Padre
    Public Property ColumnaCol_Pad() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Pad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Pad = value
        End Set
    End Property

    'columna Tipo Paquete
    Public Property ColumnaCol_Paq() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Paq
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Paq = value
        End Set
    End Property
#End Region

#Region "Metodos"

    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        _columnaCol_Code = New ColumnaMatrixSBOEditText(Of String)("Col_Code", True, "codigo", Me)
        _columnaCol_Des = New ColumnaMatrixSBOEditText(Of String)("Col_Item", True, "descripcion", Me)
        _columnaCol_Quan = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Cant", True, "cantidad", Me)
        _columnaCol_Cur = New ColumnaMatrixSBOEditText(Of String)("Col_Mon", True, "moneda", Me)
        _columnaCol_Pri = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Prec", True, "precio", Me)
        _columnaCol_Lin = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Linea", True, "linea", Me)
        _columnaCol_Tip = New ColumnaMatrixSBOEditText(Of String)("Col_Tipo", True, "tipo", Me)
        _columnaCol_Dur = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Dura", True, "duracion", Me)
        _columnaCol_Imp = New ColumnaMatrixSBOEditText(Of String)("Col_Imp", True, "impuesto", Me)
        _columnaCol_Tot = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Tot", True, "total", Me)
        _columnaCol_Hij = New ColumnaMatrixSBOEditText(Of String)("Col_Hijo", True, "hijo", Me)
        _columnaCol_Pad = New ColumnaMatrixSBOEditText(Of String)("Col_Padre", True, "padre", Me)
        _columnaCol_Paq = New ColumnaMatrixSBOEditText(Of String)("Col_Paque", True, "paquete", Me)
        _columnaCol_Barr = New ColumnaMatrixSBOEditText(Of String)("Col_Barra", True, "barras", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        _columnaCol_Code.AsignaBindingDataTable()
        _columnaCol_Des.AsignaBindingDataTable()
        _columnaCol_Quan.AsignaBindingDataTable()
        _columnaCol_Cur.AsignaBindingDataTable()
        _columnaCol_Pri.AsignaBindingDataTable()
        _columnaCol_Lin.AsignaBindingDataTable()
        _columnaCol_Tip.AsignaBindingDataTable()
        _columnaCol_Imp.AsignaBindingDataTable()
        _columnaCol_Tot.AsignaBindingDataTable()
        _columnaCol_Dur.AsignaBindingDataTable()
        _columnaCol_Hij.AsignaBindingDataTable()
        _columnaCol_Pad.AsignaBindingDataTable()
        _columnaCol_Paq.AsignaBindingDataTable()
        _columnaCol_Barr.AsignaBindingDataTable()
    End Sub

#End Region

End Class
