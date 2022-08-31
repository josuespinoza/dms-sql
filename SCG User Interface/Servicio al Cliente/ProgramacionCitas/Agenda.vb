Option Explicit On
Option Strict On

Imports SCG.UX.Windows.CitasAutomaticas

Namespace ServicioAlCliente.ProgramacionCitas
    Public Class Agenda
        Implements IAgenda

        Private _idAgenda As Integer
        Private _agenda As String
        Private _intervalo As Integer
        Private _abreviatura As String
        Private _codigoAsesor As Integer
        Private _codigoTecnico As Integer
        Private _razonCita As Integer
        Private _articuloCita As String

        Public Sub New(ByVal idAgenda As Integer, ByVal agenda As String, ByVal abreviatura As String, ByVal intervalo As Integer)
            _idAgenda = idAgenda
            _agenda = agenda
            _abreviatura = abreviatura
            _intervalo = intervalo
        End Sub

        Public Property IdAgenda() As Integer Implements IAgenda.IdAgenda
            Get
                Return _idAgenda
            End Get
            Set (ByVal value As Integer)
                _idAgenda = value
            End Set
        End Property

        Public Property Agenda() As String Implements IAgenda.Agenda
            Get
                Return _agenda
            End Get
            Set (ByVal value As String)
                _agenda = value
            End Set
        End Property

        Public Property Intervalo() As Integer Implements IAgenda.Intervalo
            Get
                Return _intervalo
            End Get
            Set (ByVal value As Integer)
                _intervalo = value
            End Set
        End Property

        Public Property Abreviatura() As String Implements IAgenda.Abreviatura
            Get
                Return _abreviatura
            End Get
            Set (ByVal value As String)
                _abreviatura = value
            End Set
        End Property

        Public Property CodigoAsesor() As Integer
            Get
                Return _codigoAsesor
            End Get
            Set (ByVal value As Integer)
                _codigoAsesor = value
            End Set
        End Property

        Public Property CodigoTecnico() As Integer
            Get
                Return _codigoTecnico
            End Get
            Set (ByVal value As Integer)
                _codigoTecnico = value
            End Set
        End Property

        Public Property RazonCita() As Integer
            Get
                Return _razonCita
            End Get
            Set (ByVal value As Integer)
                _razonCita = value
            End Set
        End Property

        Public Property ArticuloCita() As String
            Get
                Return _articuloCita
            End Get
            Set (ByVal value As String)
                _articuloCita = value
            End Set
        End Property
    End Class
End Namespace