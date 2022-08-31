Partial Class LineasSalidaDataset
End Class

Namespace LineasSalidaDatasetTableAdapters
    
    Partial Public Class _SCG_GOODRECEIVETableAdapter
    End Class
End Namespace

Namespace LineasSalidaDatasetTableAdapters
    
    Partial Public Class SCGTA_TB_SalidasVehiculosTableAdapter
        Public WriteOnly Property CadenaConexion() As String
            Set(ByVal value As String)
                Me.Connection.ConnectionString = value
            End Set
        End Property
    End Class
End Namespace
