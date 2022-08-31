Public Class SalidaMercanciaEncabezado

    Private strDocCurrency As String
    private strNumeroOT As String 
    Private strCodUnidad As String
    Private intNumVehiculo As Integer
    Private strProcesado As String
    Private strProyecto As String
    Private strReference2 As String
    
    Public Property DocCurrency As String
        Get
            Return strDocCurrency
        End Get
        Set(value As String)
            strDocCurrency = value
        End Set
    End Property

    Public Property CodUnidad As String
        Get
            Return strCodUnidad
        End Get
        Set(value As String)
            strCodUnidad = value
        End Set
    End Property

    Public Property NumVehiculo As Integer
        Get
            Return intNumVehiculo
        End Get
        Set(value As Integer)
            intNumVehiculo = value
        End Set
    End Property

    Public Property Procesado As String
        Get
            Return strProcesado
        End Get
        Set(value As String)
            strProcesado = value
        End Set
    End Property

    Public Property Proyecto As String
        Get
            Return strProyecto
        End Get
        Set(value As String)
            strProyecto = value
        End Set
    End Property

    Public Property Reference2 As String
        Get
            Return strReference2
        End Get
        Set(value As String)
            strReference2 = value
        End Set
    End Property

    Public Property NumeroOT As String
        Get
            Return strNumeroOT
        End Get
        Set(value As String)
            strNumeroOT = value
        End Set
    End Property
End Class
