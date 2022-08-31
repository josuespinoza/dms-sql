Public Class Response
    Public Class Responseerror
        Public Property RootObject As RootObject
    End Class

    Public Class Responseok
        Public Property RootObject As String
        Public Property value As String
    End Class

    Public Class Message
        Public Property lang As String
        Public Property value As String
    End Class

    Public Class RootObject
        Public Property code As String
        Public Property message As Message
    End Class
End Class
