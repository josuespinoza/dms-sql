Partial Class EstadoxRepuestosDataset
End Class

namespace EstadoxRepuestosDatasetTableAdapters
    Partial Public Class RepuestosXEstadoQueriesAdapter
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
