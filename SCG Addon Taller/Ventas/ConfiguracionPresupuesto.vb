Option Strict On
Option Explicit On

Namespace Ventas
    Public Structure ConfiguracionPresupuesto
        Private _code As String
        Private _name As String
        Private _query As String

        Public Sub New(ByVal code As String, ByVal name As String, ByVal query As String)
            _code = code
            _name = name
            _query = query
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

        Public Property Query() As String
            Get
                Return _query
            End Get
            Set (ByVal value As String)
                _query = value
            End Set
        End Property
    End Structure
End Namespace