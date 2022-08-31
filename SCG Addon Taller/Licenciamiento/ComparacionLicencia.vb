Public Class ComparacionLicencia
    Public Property Type As String
    Public Property Description As String
    Public Property CurrentQuantity As String
    Public Property NewQuantity As String
    Public Property Remarks As String

    Sub New(ByVal Type As String, ByVal Description As String, ByVal CurrentQuantity As String, ByVal NewQuantity As String, ByVal Remarks As String)
        Try
            Me.Type = Type
            Me.Description = Description
            Me.CurrentQuantity = CurrentQuantity
            Me.NewQuantity = NewQuantity
            Me.Remarks = Remarks
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Class
