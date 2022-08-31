Partial Class OtrosGastosDataSet
End Class

Namespace OtrosGastosDataSetTableAdapters
    
    Partial Class SCGTA_VW_OtrosGastosResumidoTableAdapter

        Public Property CadenaConexion() As String
            Get
                Return Me.Connection.ConnectionString
            End Get
            Set(ByVal value As String)
                Me.Connection.ConnectionString = value
            End Set
        End Property

    End Class

    Partial Public Class SCGTA_VW_OtrosGastosTableAdapter


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
