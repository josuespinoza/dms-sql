
Partial Class Principal_Default2
    Inherits System.Web.UI.Page





    Public Function DevolverColor(ByVal porcentaje As Integer) As String

        If (porcentaje < 75) Then
            Return "'barraverde.png'"
        ElseIf porcentaje >= 100 Then
            Return "'barraroja.png'"
        Else
            Return "'barraamarilla.png'"
        End If
    End Function


End Class
