Imports System

<Serializable()> _
Public Class NivelAprobacion
    Public Property Codigo() As String
        Get
            Return strCodigo
        End Get
        Set(ByVal value As String)
            strCodigo = value
        End Set
    End Property
    Private strCodigo As String

    Public Property Name() As String
        Get
            Return strName
        End Get
        Set(ByVal value As String)
            strName = value
        End Set
    End Property
    Private strName As String

    Public Property Prioridad() As Integer
        Get
            Return intPrioridad
        End Get
        Set(ByVal value As Integer)
            intPrioridad = value
        End Set
    End Property
    Private intPrioridad As Integer

    Public Property PEmp() As String
        Get
            Return strPEmp
        End Get
        Set(ByVal value As String)
            strPEmp = value
        End Set
    End Property
    Private strPEmp As String

    Public Property Estado() As String
        Get
            Return strEstado
        End Get
        Set(ByVal value As String)
            strEstado = value
        End Set
    End Property
    Private strEstado As String

    Public Property UsaMenu() As Boolean
        Get
            Return blnUsaMenu
        End Get
        Set(ByVal value As Boolean)
            blnUsaMenu = value
        End Set
    End Property
    Private blnUsaMenu As Boolean


    Public Property ValidaTipoInventario() As Boolean
        Get
            Return blnValidaTipoInventario
        End Get
        Set(ByVal value As Boolean)
            blnValidaTipoInventario = value
        End Set
    End Property
    Private blnValidaTipoInventario As Boolean

    Public Property EsUsado() As Boolean
        Get
            Return blnEsUsado
        End Get
        Set(ByVal value As Boolean)
            blnEsUsado = value
        End Set
    End Property
    Private blnEsUsado As Boolean

    Public Property NivelUsado() As String
        Get
            Return strNivelUsado
        End Get
        Set(ByVal value As String)
            strNivelUsado = value
        End Set
    End Property
    Private strNivelUsado As String

    Public Property NivelFinanciamiento() As String
        Get
            Return strNivelFinanciamiento
        End Get
        Set(ByVal value As String)
            strNivelFinanciamiento = value
        End Set
    End Property
    Private strNivelFinanciamiento As String
End Class
