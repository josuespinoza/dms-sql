'Matriz de vehiculos en la pantalla 
'Balance de Contratos de ventas

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizVehiculos
    : Inherits MatrixSBO

#Region "New"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub

#End Region

#Region "Propiedades de Columnas"

    'columna unidad
    Private _columnaCol_Unid As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Unid() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Unid
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Unid = value
        End Set
    End Property

    'columna marca
    Private _columnaCol_Marca As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Marca() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Marca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Marca = value
        End Set
    End Property

    'columna modelo
    Private _columnaCol_Mod As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Mod() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Mod
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Mod = value
        End Set
    End Property

    'columna estilo
    Private _columnaCol_Est As ColumnaMatrixSBOEditText(Of String)

    Public Property ColumnaCol_Est() As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Est
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Est = value
        End Set
    End Property

    'columna valor 
    Private _columnaCol_ValVeh As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_ValVeh() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_ValVeh
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_ValVeh = value
        End Set
    End Property

    'columna costo
    Private _columnaCol_CosVeh As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_CosVeh() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_CosVeh
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_CosVeh = value
        End Set
    End Property

    'columna utilidad 
    Private _columnaCol_UtilVeh As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_UtilVeh() As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_UtilVeh
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_UtilVeh = value
        End Set
    End Property
    
    'columna %utilidad
    Private _columnaCol_PUtiV As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaColPUtiV As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_PUtiV
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_PUtiV = value
        End Set
    End Property


    'columna %utilidad
    Private _columnaCol_Bono As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaColBono As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Bono
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Bono = value
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

    Private _columnaCol_Desc As ColumnaMatrixSBOEditText(Of Decimal)

    Public Property ColumnaCol_Desc As ColumnaMatrixSBOEditText(Of Decimal)
        Get
            Return _columnaCol_Desc
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of Decimal))
            _columnaCol_Desc = value
        End Set
    End Property




#End Region

#Region "Metodos"

    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        _columnaCol_Unid = New ColumnaMatrixSBOEditText(Of String)("Col_Unid", True, "unidad", Me)
        _columnaCol_Marca = New ColumnaMatrixSBOEditText(Of String)("Col_Marca", True, "marca", Me)
        _columnaCol_Mod = New ColumnaMatrixSBOEditText(Of String)("Col_Mod", True, "modelo", Me)
        _columnaCol_Est = New ColumnaMatrixSBOEditText(Of String)("Col_Est", True, "estilo", Me)
        _columnaCol_ValVeh = New ColumnaMatrixSBOEditText(Of Decimal)("Col_ValVeh", True, "valor", Me)
        _columnaCol_CosVeh = New ColumnaMatrixSBOEditText(Of Decimal)("Col_CosVeh", True, "costo", Me)
        _columnaCol_UtilVeh = New ColumnaMatrixSBOEditText(Of Decimal)("Col_UtiVeh", True, "utilidad", Me)
        _columnaCol_PUtiV = New ColumnaMatrixSBOEditText(Of Decimal)("Col_PUtiV", True, "putilidad", Me)
        _columnaCol_Bono = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Bono", True, "bono", Me)
        _columnaCol_PreLis = New ColumnaMatrixSBOEditText(Of Decimal)("Col_PreLis", True, "prelis", Me)
        _columnaCol_Desc = New ColumnaMatrixSBOEditText(Of Decimal)("Col_Desc", True, "desc", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        _columnaCol_Unid.AsignaBindingDataTable()
        _columnaCol_Marca.AsignaBindingDataTable()
        _columnaCol_Mod.AsignaBindingDataTable()
        _columnaCol_Est.AsignaBindingDataTable()
        _columnaCol_ValVeh.AsignaBindingDataTable()
        _columnaCol_CosVeh.AsignaBindingDataTable()
        _columnaCol_UtilVeh.AsignaBindingDataTable()
        _columnaCol_PUtiV.AsignaBindingDataTable()
        _columnaCol_Bono.AsignaBindingDataTable()
        _columnaCol_PreLis.AsignaBindingDataTable()
        _columnaCol_Desc.AsignaBindingDataTable()
    End Sub

#End Region

End Class
