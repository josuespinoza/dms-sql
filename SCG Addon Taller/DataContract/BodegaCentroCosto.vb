Imports System

<Serializable()> _
Public Class BodegaCentroCosto
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property CentroCosto() As String
        Get
            Return strCentroCosto
        End Get
        Set(ByVal value As String)
            strCentroCosto = value
        End Set
    End Property
    Private strCentroCosto As String

    Public Property BodegaRepuesto() As String
        Get
            Return strBodegaRepuesto
        End Get
        Set(ByVal value As String)
            strBodegaRepuesto = value
        End Set
    End Property
    Private strBodegaRepuesto As String

    Public Property BodegaServicio() As String
        Get
            Return strBodegaServicio
        End Get
        Set(ByVal value As String)
            strBodegaServicio = value
        End Set
    End Property
    Private strBodegaServicio As String

    Public Property BodegaSuministro() As String
        Get
            Return strBodegaSuministro
        End Get
        Set(ByVal value As String)
            strBodegaSuministro = value
        End Set
    End Property
    Private strBodegaSuministro As String

    Public Property BodegaServicioExterno() As String
        Get
            Return strBodegaServicioExterno
        End Get
        Set(ByVal value As String)
            strBodegaServicioExterno = value
        End Set
    End Property
    Private strBodegaServicioExterno As String

    Public Property BodegaProceso() As String
        Get
            Return strBodegaProceso
        End Get
        Set(ByVal value As String)
            strBodegaProceso = value
        End Set
    End Property
    Private strBodegaProceso As String

    Public Property BodegaReservas() As String
        Get
            Return strBodegaReservas
        End Get
        Set(ByVal value As String)
            strBodegaReservas = value
        End Set
    End Property
    Private strBodegaReservas As String

    Public Property UsaUbicaciones() As Boolean
        Get
            Return blnUsaUbicaciones
        End Get
        Set(ByVal value As Boolean)
            blnUsaUbicaciones = value
        End Set
    End Property
    Private blnUsaUbicaciones As Boolean

    Public Property UbicacionDBP() As String
        Get
            Return strUbicacionDBP
        End Get
        Set(ByVal value As String)
            strUbicacionDBP = value
        End Set
    End Property
    Private strUbicacionDBP As String

    Public Property Sucursal() As String
        Get
            Return strSucursal
        End Get
        Set(ByVal value As String)
            strSucursal = value
        End Set
    End Property
    Private strSucursal As String
End Class
