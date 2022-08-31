Option Strict On
Option Explicit On

Namespace Ventas

    Public Class ConfiguracionArticuloVenta
        Private _code As String
        Private _name As String
        Private _articuloVenta As String

        Public Sub New(ByVal code As String, ByVal name As String, ByVal articuloVenta As String)
            _code = code
            _name = name
            _articuloVenta = articuloVenta
        End Sub

        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property ArticuloVenta() As String
            Get
                Return _articuloVenta
            End Get
            Set(ByVal value As String)
                _articuloVenta = value
            End Set
        End Property
    End Class

End Namespace