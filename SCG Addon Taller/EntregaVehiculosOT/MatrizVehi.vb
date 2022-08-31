'Matriz de vehiculos en la pantalla 
'Balance de Contratos de ventas

Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Public Class MatrizVehi
    : Inherits MatrixSBO

#Region "Declaraciones"
    Private _columnaCol_Cotizacion As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_ot As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_unidad As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_placa As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_vin As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_marca As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_estilo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_modelo As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_ano As ColumnaMatrixSBOEditText(Of String)
    Private _columnaCol_numv As ColumnaMatrixSBOEditText(Of String)

#End Region

#Region "Propiedades"

    Public Property ColumnaCol_Ot As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ot
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ot = value
        End Set
    End Property

    Public Property ColumnaCol_Unidad As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_unidad
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_unidad = value
        End Set
    End Property

    Public Property ColumnaCol_Placa As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_placa
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_placa = value
        End Set
    End Property

    Public Property ColumnaCol_Marca As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_marca
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_marca = value
        End Set
    End Property

    Public Property ColumnaCol_Estilo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_estilo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_estilo = value
        End Set
    End Property

    Public Property ColumnaCol_Modelo As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_modelo
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_modelo = value
        End Set
    End Property

    Public Property ColumnaCol_Ano As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_ano
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_ano = value
        End Set
    End Property

    Public Property ColumnaCol_Numv As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_numv
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_numv = value
        End Set
    End Property

    Public Property ColumnaCol_vin As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_vin
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_vin = value
        End Set
    End Property

    Public Property ColumnaCol_Cotizacion As ColumnaMatrixSBOEditText(Of String)
        Get
            Return _columnaCol_Cotizacion
        End Get
        Set(ByVal value As ColumnaMatrixSBOEditText(Of String))
            _columnaCol_Cotizacion = value
        End Set
    End Property

#End Region

#Region "New"

    Public Sub New(ByVal UniqueId As String, ByVal formularioSBO As IForm, ByVal tablaLigada As String)
        MyBase.New(UniqueId, formularioSBO)
        Me.TablaLigada = tablaLigada
    End Sub


#End Region

#Region "Metodos"

    'Crear columnas en la matriz, para ligarlas al datatable
    Public Overrides Sub CreaColumnas()
        ColumnaCol_Cotizacion = New ColumnaMatrixSBOEditText(Of String)("Col_NoCot", True, "cotizacion", Me)
        ColumnaCol_Ot = New ColumnaMatrixSBOEditText(Of String)("Col_NoOT", True, "ot", Me)
        ColumnaCol_Unidad = New ColumnaMatrixSBOEditText(Of String)("Col_Unid", True, "unidad", Me)
        ColumnaCol_Placa = New ColumnaMatrixSBOEditText(Of String)("Col_Placa", True, "placa", Me)
        ColumnaCol_vin = New ColumnaMatrixSBOEditText(Of String)("Col_VIN", True, "vin", Me)
        ColumnaCol_Marca = New ColumnaMatrixSBOEditText(Of String)("Col_Marca", True, "marca", Me)
        ColumnaCol_Estilo = New ColumnaMatrixSBOEditText(Of String)("Col_Estilo", True, "estilo", Me)
        ColumnaCol_Modelo = New ColumnaMatrixSBOEditText(Of String)("Col_Modelo", True, "modelo", Me)
        ColumnaCol_Ano = New ColumnaMatrixSBOEditText(Of String)("Col_Ano", True, "ano", Me)
        ColumnaCol_Numv = New ColumnaMatrixSBOEditText(Of String)("Col_NumV", True, "numv", Me)
    End Sub

    'ligar las columnas del dataTable con la matriz
    Public Overrides Sub LigaColumnas()
        ColumnaCol_Cotizacion.AsignaBindingDataTable()
        ColumnaCol_Ot.AsignaBindingDataTable()
        ColumnaCol_Unidad.AsignaBindingDataTable()
        ColumnaCol_Placa.AsignaBindingDataTable()
        ColumnaCol_vin.AsignaBindingDataTable()
        ColumnaCol_Marca.AsignaBindingDataTable()
        ColumnaCol_Estilo.AsignaBindingDataTable()
        ColumnaCol_Modelo.AsignaBindingDataTable()
        ColumnaCol_Ano.AsignaBindingDataTable()
        ColumnaCol_Numv.AsignaBindingDataTable()
    End Sub

#End Region

End Class
