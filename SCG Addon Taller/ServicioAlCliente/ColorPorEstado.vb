Public Class ColorPorEstado
    Private ConOrdenTrabajo As String
    Private SinOrdenTrabajo As String

    Public ReadOnly Property ColorConOrdenTrabajo() As String
        Get
            Return ConOrdenTrabajo
        End Get
    End Property

    Public ReadOnly Property ColorSinOrdenTrabajo() As String
        Get
            Return SinOrdenTrabajo
        End Get
    End Property

    Sub New(ByVal pColorSinOrdenTrabajo As String, ByVal pColorConOrdenTrabajo As String)
        Try
            ConOrdenTrabajo = pColorConOrdenTrabajo
            SinOrdenTrabajo = pColorSinOrdenTrabajo
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Class
