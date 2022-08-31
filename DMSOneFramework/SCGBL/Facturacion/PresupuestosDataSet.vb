Imports System.Data.SqlClient

Namespace PresupuestosDataSetTableAdapters

    Partial Public Class ConfiguracionesPresupuestosTableAdapter
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

Partial Class PresupuestosDataSet
End Class

Namespace PresupuestosDataSetTableAdapters
    
    Partial Public Class MarcasPresupuestoTableAdapter
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
