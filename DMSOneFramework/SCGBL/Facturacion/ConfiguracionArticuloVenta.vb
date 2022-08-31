Imports System.Data.SqlClient

Partial Class ConfiguracionArticuloVenta
    Partial Class ConfiguracionesArticuloVentaDataTable

        Private Sub ConfiguracionesArticuloVentaDataTable_ConfiguracionesArticuloVentaRowChanging(ByVal sender As System.Object, ByVal e As ConfiguracionesArticuloVentaRowChangeEvent) Handles Me.ConfiguracionesArticuloVentaRowChanging

        End Sub

    End Class

End Class

Namespace ConfiguracionArticuloVentaTableAdapters

    Partial Public Class ConfiguracionesArticuloVentaTableAdapter
        Public WriteOnly Property CadenaConexion() As String
            Set(ByVal value As String)
                Me.Connection.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property ConexionSQL() As SqlConnection
            Set(ByVal value As SqlConnection)
                Me.Connection = value
            End Set
        End Property
    End Class
End Namespace
