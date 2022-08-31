Namespace SCGCommon

    Public Class ExceptionsSBO
        Inherits System.ApplicationException

        Private strMensage As String
        Private intCodigoError As Integer

        Public Property Codigo() As Integer
            Get
                Return intCodigoError
            End Get
            Set(ByVal Value As Integer)
                intCodigoError = Value
            End Set
        End Property

'        Public Sub New(ByVal coderror As Integer)
'            MyBase.New()
'            intCodigoError = coderror
'        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(coderror As Integer, ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
            intCodigoError = coderror
        End Sub

         Public Sub New(coderror As Integer, ByVal message As String)
            Me.New(coderror, message, Nothing)
         End Sub

    End Class

End Namespace
