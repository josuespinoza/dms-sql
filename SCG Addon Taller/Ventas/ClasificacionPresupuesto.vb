Option Strict On
Option Explicit On

Namespace Ventas

    Public Structure ClasificacionPresupuesto
        Private _code As String
        Private _name As String

        Public Sub New(ByVal code As String, ByVal name As String)
            _code = code
            _name = name
        End Sub

        Public Property Code() As String
            Get
                Return _code
            End Get
            Set (ByVal value As String)
                _code = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set (ByVal value As String)
                _name = value
            End Set
        End Property
    End Structure

End Namespace