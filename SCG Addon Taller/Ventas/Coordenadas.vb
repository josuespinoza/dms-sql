Public Class Coordenadas
    Public Left As Integer
    Public Top As Integer

    Public Sub New(ByVal intLeft As Integer, ByVal intTop As Integer)
        Try
            Left = intLeft
            Top = intTop
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Class