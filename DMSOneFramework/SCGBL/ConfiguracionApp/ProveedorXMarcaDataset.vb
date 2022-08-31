Namespace ProveedorXMarcaDatasetTableAdapters

    Partial Class SCGTB_TA_ProveedorXMarcaTableAdapter

        Public Sub SetTransaction(ByRef tnTransaccion As SqlClient.SqlTransaction)
            Dim cmdComand As SqlClient.SqlCommand
            For Each cmdComand In Me._commandCollection
                cmdComand.Transaction = tnTransaccion
            Next
            Me.Adapter.UpdateCommand.Transaction = tnTransaccion
            Me.Adapter.InsertCommand.Transaction = tnTransaccion
            Me.Adapter.DeleteCommand.Transaction = tnTransaccion
        End Sub
    End Class


End Namespace

