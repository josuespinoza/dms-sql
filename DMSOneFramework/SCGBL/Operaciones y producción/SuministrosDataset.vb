
Partial Public Class SuministrosDataset
End Class

Namespace SuministrosDatasetTableAdapters
    Partial Public Class CantidadSuministrosQueryAdapater
        protected _conexion As String

        Public Property Conexion() As String
            Get
                Return _conexion
            End Get
            Set (ByVal value As String)
                _conexion = value
            End Set
        End Property
    End Class
End Namespace
