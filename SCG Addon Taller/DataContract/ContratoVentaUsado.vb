Imports System

<Serializable()> _
Public Class ContratoVentaUsado
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

    Public Property CodigoSucursal() As String
        Get
            Return strCodigoSucursal
        End Get
        Set(ByVal value As String)
            strCodigoSucursal = value
        End Set
    End Property
    Private strCodigoSucursal As String

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
