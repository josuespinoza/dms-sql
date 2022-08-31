Imports System

<Serializable()> _
Public Class ContratoVentaVehiculo
    Public Property DocEntry() As Integer
        Get
            Return intDocEntry
        End Get
        Set(ByVal value As Integer)
            intDocEntry = value
        End Set
    End Property
    Private intDocEntry As Integer

    Public Property CodigoUnidad() As String
        Get
            Return strCodigoUnidad
        End Get
        Set(ByVal value As String)
            strCodigoUnidad = value
        End Set
    End Property
    Private strCodigoUnidad As String

    Public Property Estado() As Integer
        Get
            Return intEstado
        End Get
        Set(ByVal value As Integer)
            intEstado = value
        End Set
    End Property
    Private intEstado As Integer
End Class
