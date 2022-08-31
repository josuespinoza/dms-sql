Option Explicit On
Option Strict On

Imports SCG.UX.Windows.CitasAutomaticas

Namespace ServicioAlCliente.ProgramacionCitas
    Public Class FiltroDMS
        Implements IFiltro
        Private _condicion As String
        Private _activo As Boolean
        Private _codigoCategoriaFiltro As Integer
        Private _descripcion As String
        Private _filtro As String
        Private _confPorAgenda As Dictionary(Of Integer, IConfiguracionFiltroAgenda)

        Public Sub New(ByVal filtro As String, ByVal descripcion As String, ByVal condicion As String, ByVal codigoCategoriaFiltro As Integer, ByVal activo As Boolean)
            _filtro = filtro
            _descripcion = descripcion
            _condicion = condicion
            _codigoCategoriaFiltro = codigoCategoriaFiltro
            _activo = activo
            _confPorAgenda = New Dictionary(Of Integer, IConfiguracionFiltroAgenda)()
        End Sub

        Public Property Filtro() As String Implements IFiltro.Filtro
            Get
                Return _filtro
            End Get
            Set(ByVal value As String)
                _filtro = value
            End Set
        End Property

        Public Property Descripcion() As String Implements IFiltro.Descripcion
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property CodigoCategoriaFiltro() As Integer Implements IFiltro.CodigoCategoriaFiltro
            Get
                Return _codigoCategoriaFiltro
            End Get
            Set(ByVal value As Integer)
                _codigoCategoriaFiltro = value
            End Set
        End Property

        Public Property Condicion() As String Implements IFiltro.Condicion
            Get
                Return _condicion
            End Get
            Set(ByVal value As String)
                _condicion = value
            End Set
        End Property

        Public Property Activo() As Boolean Implements IFiltro.Activo
            Get
                Return _activo
            End Get
            Set(ByVal value As Boolean)
                _activo = value
            End Set
        End Property

        Public ReadOnly Property ConfiguracionesPorAgenda() As IDictionary(Of Integer, IConfiguracionFiltroAgenda) Implements IFiltro.ConfiguracionesPorAgenda
            Get
                Return _confPorAgenda
            End Get
        End Property

    End Class
End Namespace

