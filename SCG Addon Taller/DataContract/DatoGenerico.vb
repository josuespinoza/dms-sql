Imports System

<Serializable()> _
Public Class DatoGenerico
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property DocNum() As Integer
        Get
            Return intDocNum
        End Get
        Set(ByVal value As Integer)
            intDocNum = value
        End Set
    End Property
    Private intDocNum As Integer

    Public Property FechaContabilizacion() As Date
        Get
            Return dateFechaContabilizacion
        End Get
        Set(ByVal value As Date)
            dateFechaContabilizacion = value
        End Set
    End Property
    Private dateFechaContabilizacion As Date

    Public Property FechaCreacion() As Date
        Get
            Return dateFechaCreacion
        End Get
        Set(ByVal value As Date)
            dateFechaCreacion = value
        End Set
    End Property
    Private dateFechaCreacion As Date

    Public Property MonedaLocal() As String
        Get
            Return strMonedaLocal
        End Get
        Set(ByVal value As String)
            strMonedaLocal = value
        End Set
    End Property
    Private strMonedaLocal As String

    Public Property BaseEntry() As Integer
        Get
            Return intBaseEntry
        End Get
        Set(ByVal value As Integer)
            intBaseEntry = value
        End Set
    End Property
    Private intBaseEntry As Integer

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property CardCode() As String
        Get
            Return strCardCode
        End Get
        Set(ByVal value As String)
            strCardCode = value
        End Set
    End Property
    Private strCardCode As String

    Public Property CardName() As String
        Get
            Return strCardName
        End Get
        Set(ByVal value As String)
            strCardName = value
        End Set
    End Property
    Private strCardName As String

    Public Property Observaciones() As String
        Get
            Return strObservaciones
        End Get
        Set(ByVal value As String)
            strObservaciones = value
        End Set
    End Property
    Private strObservaciones As String
End Class
