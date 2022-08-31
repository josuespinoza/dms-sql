Option Explicit On
Option Strict On

Imports SCG_User_Interface.ServicioAlCliente.ProgramacionCitas
Imports SCG.UX.Windows.CitasAutomaticas

Public Class Vehiculo
    Implements IElementoCita

    Private _fechaUltimoServicio As Nullable(Of DateTime)
    Private _fechaProximoServicio As Nullable(Of DateTime)
    Private _frecuenciaDias As Nullable(Of Integer)
    Private _enAgenda As Boolean
    Private _generarCita As Boolean
    Private _idVehiculo As String
    Private _codMarca As String
    Private _codModelo As String
    Private _codEstilo As String
    Private _descMarca As String
    Private _descModelo As String
    Private _descEstilo As String
    Private _descripcion As String
    Private _cardCode As String
    Private _cardName As String
    Private _codUnidad As String
    Private _numPlaca As String
    Private _vin As String
    Private _filtro As FiltroDMS

    Private _modificadoPorUsuario As Boolean

#Region "Columnas"

    Public Const ColumnaCode As String = "Code"
    Public Const ColumnaFechaProximoServicio As String = "U_FchPrSv"
    Public Const ColumnaFechaUltimoServicio As String = "U_FchUSv"
    Public Const ColumnaFrecuenciaServicio As String = "U_FrecSvc"
    Public Const ColumnaDescMarca As String = "U_Des_Marc"
    Public Const ColumnaDescModelo As String = "U_Des_Mode"
    Public Const ColumnaDescEstilo As String = "U_Des_Esti"
    Public Const ColumnaCodMarca As String = "U_Cod_Marc"
    Public Const ColumnaCodModelo As String = "U_Cod_Mode"
    Public Const ColumnaCodEstilo As String = "U_Cod_Esti"
    Public Const ColumnaCardCode As String = "U_CardCode"
    Public Const ColumnaCardName As String = "U_CardName"
    Public Const ColumnaVIN As String = "U_Num_VIN"
    Public Const ColumnaPlaca As String = "U_Num_Plac"
    Public Const ColumnaNoUnidad As String = "U_Cod_Unid"

#End Region

    Public Sub New(ByVal idVehiculo As String, ByVal descripcion As String, ByVal fechaProximoServicio As Nullable(Of Date), ByVal fechaUltimoServicio As Nullable(Of Date), ByVal frecuenciaDias As Nullable(Of Integer))
        _fechaProximoServicio = fechaProximoServicio
        _fechaUltimoServicio = fechaUltimoServicio
        _frecuenciaDias = frecuenciaDias
        _idVehiculo = idVehiculo
        _descripcion = descripcion
    End Sub

    Public Property TodoElDia() As Boolean Implements IElementoCita.TodoElDia
        Get
            Return True
        End Get
        Set (ByVal value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property FechaUltimoServicio() As Nullable(Of DateTime) Implements IElementoCita.FechaUltimoServicio
        Get
            Return _fechaUltimoServicio
        End Get
        Set(ByVal value As Nullable(Of Date))
            _fechaUltimoServicio = value
        End Set
    End Property

    Public Property FechaProximoServicio() As Nullable(Of DateTime) Implements IElementoCita.FechaProximoServicio
        Get
            Return _fechaProximoServicio
        End Get
        Set(ByVal value As Nullable(Of Date))
            _fechaProximoServicio = value
        End Set
    End Property

    Public Property FrecuenciaDias() As Nullable(Of Integer) Implements IElementoCita.FrecuenciaDias
        Get
            Return _frecuenciaDias
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _frecuenciaDias = value
        End Set
    End Property

    Public Property CodigoObjeto() As Object Implements IElementoCita.CodigoObjeto
        Get
            Return _idVehiculo
        End Get
        Set(ByVal value As Object)
            _idVehiculo = value.ToString()
        End Set
    End Property

    Public Property Descripcion() As String Implements IElementoCita.Descripcion
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

    Public Property ModificadoPorUsuario() As Boolean Implements IElementoCita.ModificadoPorUsuario
        Get
            Return _modificadoPorUsuario
        End Get
        Set(ByVal value As Boolean)
            _modificadoPorUsuario = value
        End Set
    End Property

    Public Property EnAgenda() As Boolean Implements IElementoCita.EnAgenda
        Get
            Return _enAgenda
        End Get
        Set(ByVal value As Boolean)
            _enAgenda = value
        End Set
    End Property

    Public Property GenerarCita() As Boolean Implements IElementoCita.GenerarCita
        Get
            Return _generarCita
        End Get
        Set(ByVal value As Boolean)
            _generarCita = value
        End Set
    End Property

    Public Property Filtro() As IFiltro Implements IElementoCita.Filtro
        Get
            Return _filtro
        End Get
        Set (ByVal value As IFiltro)
            _filtro = DirectCast(value, FiltroDMS)
        End Set
    End Property

    Public Property DescMarca() As String
        Get
            Return _descMarca
        End Get
        Set(ByVal value As String)
            _descMarca = value
        End Set
    End Property

    Public Property DescModelo() As String
        Get
            Return _descModelo
        End Get
        Set(ByVal value As String)
            _descModelo = value
        End Set
    End Property

    Public Property DescEstilo() As String
        Get
            Return _descEstilo
        End Get
        Set(ByVal value As String)
            _descEstilo = value
        End Set
    End Property

    Public Property CardCode() As String
        Get
            Return _cardCode
        End Get
        Set(ByVal value As String)
            _cardCode = value
        End Set
    End Property

    Public ReadOnly Property IdVehiculo() As String
        Get
            Return _idVehiculo
        End Get
    End Property

    Public Property CodUnidad() As String
        Get
            Return _codUnidad
        End Get
        Set(ByVal value As String)
            _codUnidad = value
        End Set
    End Property

    Public Property NumPlaca() As String
        Get
            Return _numPlaca
        End Get
        Set(ByVal value As String)
            _numPlaca = value
        End Set
    End Property

    Public Property CodMarca() As String
        Get
            Return _codMarca
        End Get
        Set(ByVal value As String)
            _codMarca = value
        End Set
    End Property

    Public Property CodModelo() As String
        Get
            Return _codModelo
        End Get
        Set(ByVal value As String)
            _codModelo = value
        End Set
    End Property

    Public Property CodEstilo() As String
        Get
            Return _codEstilo
        End Get
        Set(ByVal value As String)
            _codEstilo = value
        End Set
    End Property

    Public Property Vin() As String
        Get
            Return _vin
        End Get
        Set(ByVal value As String)
            _vin = value
        End Set
    End Property

    Public Property CardName() As String
        Get
            Return _cardName
        End Get
        Set(ByVal value As String)
            _cardName = value
        End Set
    End Property
End Class
