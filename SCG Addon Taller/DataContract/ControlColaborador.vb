Imports System

<Serializable()> _
Public Class ControlColaborador
    Public Property Code() As String
        Get
            Return strCode
        End Get
        Set(ByVal value As String)
            strCode = value
        End Set
    End Property
    Private strCode As String

    Public Property LineId() As Integer
        Get
            Return intLineId
        End Get
        Set(ByVal value As Integer)
            intLineId = value
        End Set
    End Property
    Private intLineId As Integer

    Public Property Colaborador() As String
        Get
            Return strColaborador
        End Get
        Set(ByVal value As String)
            strColaborador = value
        End Set
    End Property
    Private strColaborador As String

    Public Property TiempoMinutos() As Double
        Get
            Return dblTiempoMinutos
        End Get
        Set(ByVal value As Double)
            dblTiempoMinutos = value
        End Set
    End Property
    Private dblTiempoMinutos As Double

    Public Property Reproceso() As String
        Get
            Return strReproceso
        End Get
        Set(ByVal value As String)
            strReproceso = value
        End Set
    End Property
    Private strReproceso As String

    Public Property FaseProduccion() As String
        Get
            Return strFaseProduccion
        End Get
        Set(ByVal value As String)
            strFaseProduccion = value
        End Set
    End Property
    Private strFaseProduccion As String

    Public Property Estado() As String
        Get
            Return strEstado
        End Get
        Set(ByVal value As String)
            strEstado = value
        End Set
    End Property
    Private strEstado As String

    Public Property IdActividad() As String
        Get
            Return strIdActividad
        End Get
        Set(ByVal value As String)
            strIdActividad = value
        End Set
    End Property
    Private strIdActividad As String

    Public Property CostoReal() As Double
        Get
            Return dblCostoReal
        End Get
        Set(ByVal value As Double)
            dblCostoReal = value
        End Set
    End Property
    Private dblCostoReal As Double

    Public Property CostoEstandar() As Double
        Get
            Return dblCostoEstandar
        End Get
        Set(ByVal value As Double)
            dblCostoEstandar = value
        End Set
    End Property
    Private dblCostoEstandar As Double

    Public Property ReAsigna() As String
        Get
            Return strReAsigna
        End Get
        Set(ByVal value As String)
            strReAsigna = value
        End Set
    End Property
    Private strReAsigna As String

    Public Property FechaInicio() As String
        Get
            Return strFechaInicio
        End Get
        Set(ByVal value As String)
            strFechaInicio = value
        End Set
    End Property
    Private strFechaInicio As String

    Public Property FechaFin() As String
        Get
            Return strFechaFin
        End Get
        Set(ByVal value As String)
            strFechaFin = value
        End Set
    End Property
    Private strFechaFin As String

    Public Property FechaProceso() As DateTime
        Get
            Return dateFechaProceso
        End Get
        Set(ByVal value As DateTime)
            dateFechaProceso = value
        End Set
    End Property
    Private dateFechaProceso As DateTime

    Public Property CodigoFaseProduccion() As String
        Get
            Return strCodigoFaseProduccion
        End Get
        Set(ByVal value As String)
            strCodigoFaseProduccion = value
        End Set
    End Property
    Private strCodigoFaseProduccion As String
End Class
