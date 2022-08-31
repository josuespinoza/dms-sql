Public Class Horario
    Private Apertura As DateTime
    Private Cierre As DateTime
    Private InicioAlmuerzo As DateTime
    Private FinAlmuerzo As DateTime
    Private DuracionAlmuerzo As Integer = 0
    Private Configurado As Boolean = False

    Public ReadOnly Property MinutosAlmuerzo() As Integer
        Get
            Return DuracionAlmuerzo
        End Get
    End Property

    Public ReadOnly Property HoraInicioAlmuerzo() As DateTime
        Get
            Return InicioAlmuerzo
        End Get
    End Property

    Public ReadOnly Property HoraFinAlmuerzo() As DateTime
        Get
            Return FinAlmuerzo
        End Get
    End Property

    Public ReadOnly Property HoraApertura() As DateTime
        Get
            Return Apertura
        End Get
    End Property

    Public ReadOnly Property HoraCierre() As DateTime
        Get
            Return Cierre
        End Get
    End Property

    Public ReadOnly Property HorarioConfigurado() As Boolean
        Get
            Return Configurado
        End Get
    End Property


    Sub New(ByVal HorarioApertura As DateTime, ByVal HorarioCierre As DateTime, ByVal HorarioInicioAlmuerzo As DateTime, ByVal HorarioFinAlmuerzo As DateTime)
        'Se utiliza la fecha mínima, ya que esta es diferente entre los objetos COM y .NET ocasionando
        'que no se puedan verificar de la manera tradicional con DateTime.MinValue
        Dim FechaMinima As DateTime
        Dim EspacioTiempo As TimeSpan
        Try
            FechaMinima = New DateTime(1899, 12, 30)
            Apertura = HorarioApertura
            Cierre = HorarioCierre
            InicioAlmuerzo = HorarioInicioAlmuerzo
            FinAlmuerzo = HorarioFinAlmuerzo
            If Not InicioAlmuerzo = FechaMinima AndAlso Not FinAlmuerzo = FechaMinima Then
                EspacioTiempo = FinAlmuerzo - InicioAlmuerzo
                DuracionAlmuerzo = EspacioTiempo.TotalMinutes
            End If

            If Not Apertura = FechaMinima AndAlso Not Cierre = FechaMinima Then
                Configurado = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Class
