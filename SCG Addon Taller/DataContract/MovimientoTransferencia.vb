Imports System

<Serializable()> _
Public Class MovimientoTransferencia
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property ItemCode() As String
        Get
            Return strItemCode
        End Get
        Set(ByVal value As String)
            strItemCode = value
        End Set
    End Property
    Private strItemCode As String

    Public Property Description() As String
        Get
            Return strDescription
        End Get
        Set(ByVal value As String)
            strDescription = value
        End Set
    End Property
    Private strDescription As String


    Public Property CantidadDisponible() As Double
        Get
            Return dblCantidadDisponible
        End Get
        Set(ByVal value As Double)
            dblCantidadDisponible = value
        End Set
    End Property
    Private dblCantidadDisponible As Double

    Public Property TipoArticulo() As Integer
        Get
            Return intTipoArticulo
        End Get
        Set(ByVal value As Integer)
            intTipoArticulo = value
        End Set
    End Property
    Private intTipoArticulo As Integer

    Public Property CantidadTransferir() As Double
        Get
            Return dblCantidadTransferir
        End Get
        Set(ByVal value As Double)
            dblCantidadTransferir = value
        End Set
    End Property
    Private dblCantidadTransferir As Double

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property Entregado() As String
        Get
            Return strEntregado
        End Get
        Set(ByVal value As String)
            strEntregado = value
        End Set
    End Property
    Private strEntregado As String

    Public Property ID() As String
        Get
            Return strID
        End Get
        Set(ByVal value As String)
            strID = value
        End Set
    End Property
    Private strID As String

    Public Property Procesar() As Boolean
        Get
            Return blnProcesar
        End Get
        Set(ByVal value As Boolean)
            blnProcesar = value
        End Set
    End Property
    Private blnProcesar As Boolean

    Public Property BodegaOrigen() As String
        Get
            Return strBodegaOrigen
        End Get
        Set(ByVal value As String)
            strBodegaOrigen = value
        End Set
    End Property
    Private strBodegaOrigen As String

    Public Property BodegaDestino() As String
        Get
            Return strBodegaDestino
        End Get
        Set(ByVal value As String)
            strBodegaDestino = value
        End Set
    End Property
    Private strBodegaDestino As String

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

    Public Property CodigoEstadoLinea() As Integer
        Get
            Return intCodigoEstadoLinea
        End Get
        Set(ByVal value As Integer)
            intCodigoEstadoLinea = value
        End Set
    End Property
    Private intCodigoEstadoLinea As Integer

    Public Property EstadoLinea() As String
        Get
            Return strEstadoLinea
        End Get
        Set(ByVal value As String)
            strEstadoLinea = value
        End Set
    End Property
    Private strEstadoLinea As String

    Public Property UbicacionOrigen() As String
        Get
            Return strUbicacionOrigen
        End Get
        Set(ByVal value As String)
            strUbicacionOrigen = value
        End Set
    End Property
    Private strUbicacionOrigen As String

    Public Property UbicacionDestino() As String
        Get
            Return strUbicacionDestino
        End Get
        Set(ByVal value As String)
            strUbicacionDestino = value
        End Set
    End Property
    Private strUbicacionDestino As String

    Public Property NombreCliente() As String
        Get
            Return strNombreCliente
        End Get
        Set(ByVal value As String)
            strNombreCliente = value
        End Set
    End Property
    Private strNombreCliente As String

    Public Property TipoDocumento() As String
        Get
            Return strTipoDocumento
        End Get
        Set(ByVal value As String)
            strTipoDocumento = value
        End Set
    End Property
    Private strTipoDocumento As String

    Public Property Usuario() As String
        Get
            Return strUsuario
        End Get
        Set(ByVal value As String)
            strUsuario = value
        End Set
    End Property
    Private strUsuario As String

    Public Property Comentario() As String
        Get
            Return strComentario
        End Get
        Set(ByVal value As String)
            strComentario = value
        End Set
    End Property
    Private strComentario As String

    Public Property IdRepxOrd() As String
        Get
            Return strIdRepxOrd
        End Get
        Set(ByVal value As String)
            strIdRepxOrd = value
        End Set
    End Property
    Private strIdRepxOrd As String

    Public Property BodegaUbicacion() As String
        Get
            Return strBodegaUbicacion
        End Get
        Set(ByVal value As String)
            strBodegaUbicacion = value
        End Set
    End Property
    Private strBodegaUbicacion As String

    Public Property FechaDocumento() As DateTime
        Get
            Return dateFechaDocumento
        End Get
        Set(ByVal value As DateTime)
            dateFechaDocumento = value
        End Set
    End Property
    Private dateFechaDocumento As DateTime

    Public Property DocEntryTransferencia() As Integer
        Get
            Return intDocEntryTransferencia
        End Get
        Set(ByVal value As Integer)
            intDocEntryTransferencia = value
        End Set
    End Property
    Private intDocEntryTransferencia As Integer

    Public Property DocNumTransferencia() As Integer
        Get
            Return intDocNumTransferencia
        End Get
        Set(ByVal value As Integer)
            intDocNumTransferencia = value
        End Set
    End Property
    Private intDocNumTransferencia As Integer

    Public Property CardCode() As String
        Get
            Return strCardCode
        End Get
        Set(ByVal value As String)
            strCardCode = value
        End Set
    End Property
    Private strCardCode As String

    Public Property Series() As Integer
        Get
            Return intSeries
        End Get
        Set(ByVal value As Integer)
            intSeries = value
        End Set
    End Property
    Private intSeries As Integer

    Public Property TipoTransferencia() As Integer
        Get
            Return intTipoTransferencia
        End Get
        Set(ByVal value As Integer)
            intTipoTransferencia = value
        End Set
    End Property
    Private intTipoTransferencia As Integer
End Class

