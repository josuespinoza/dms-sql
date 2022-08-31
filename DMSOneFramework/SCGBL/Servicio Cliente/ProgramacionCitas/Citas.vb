

Partial Public Class Citas
End Class

Namespace CitasTableAdapters
    
    Partial Class SCGTA_VW_Vehiculos2TableAdapter
        Public Property CadenaConexion() As String
            Get
                Return Me.Connection.ConnectionString
            End Get
            Set(ByVal value As String)
                Me.Connection.ConnectionString = value
            End Set
        End Property

    End Class

    Partial Public Class SCGTA_TB_CitaTableAdapter
        Public Property Transaccion() As Global.System.Data.SqlClient.SqlTransaction
            Get
                Return Me._transaction
            End Get
            Set(ByVal value As Global.System.Data.SqlClient.SqlTransaction)
                Me.Transaction = value
            End Set
        End Property
    End Class
End Namespace

Namespace CitasTableAdapters

    Partial Public Class SBO_SCG_AGENDACITATableAdapter
        Public Property CadenaConexion() As String
            Get
                Return Me.Connection.ConnectionString
            End Get
            Set(ByVal value As String)
                Me.Connection.ConnectionString = value
            End Set
        End Property
    End Class
End Namespace
