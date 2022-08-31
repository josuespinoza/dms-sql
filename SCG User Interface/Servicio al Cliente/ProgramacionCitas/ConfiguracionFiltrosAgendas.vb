Option Strict On
Option Explicit On

Imports SCG.UX.Windows.CitasAutomaticas

Namespace ServicioAlCliente.ProgramacionCitas

    Public Class ConfiguracionFiltrosAgendas
        Implements IConfiguracionFiltroAgenda

        Private _idAgenda As Integer
        Private _agenda As String
        Private _iniciaActivo As Boolean
        Private _color As Color

        Public Sub New(ByVal idAgenda As Integer, ByVal agenda As String, ByVal iniciaActivo As Boolean, color As Color)
            _idAgenda = idAgenda
            _agenda = agenda
            _iniciaActivo = iniciaActivo
            _color = color
        End Sub

        Public Property IdAgenda() As Integer Implements IConfiguracionFiltroAgenda.IdAgenda
            Get
                Return _idAgenda
            End Get
            Set (ByVal value As Integer)
                _idAgenda = value
            End Set
        End Property

        Public Property Agenda() As String Implements IConfiguracionFiltroAgenda.Agenda
            Get
                Return _agenda
            End Get
            Set (ByVal value As String)
                _agenda = value
            End Set
        End Property

        Public Property IniciaActivo() As Boolean Implements IConfiguracionFiltroAgenda.IniciaActivo
            Get
                Return _iniciaActivo
            End Get
            Set (ByVal value As Boolean)
                _iniciaActivo = value
            End Set
        End Property

        Public Property Color() As Color Implements IConfiguracionFiltroAgenda.Color
            Get
                Return _color
            End Get
            Set (ByVal value As Color)
                _color = value
            End Set
        End Property


        Public Shadows Function Equals(ByVal other As IConfiguracionFiltroAgenda) As Boolean Implements IEquatable _
                                                                                         (Of IConfiguracionFiltroAgenda) _
                                                                                         .Equals
            Return other.IdAgenda = _idAgenda
        End Function
    End Class

End Namespace