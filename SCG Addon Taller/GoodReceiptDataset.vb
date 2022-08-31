Namespace GoodReceiptDatasetTableAdapters
    Partial Class _SCG_GOODRECEIVETableAdapter

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

    Partial Class _SCG_GRLINESTableAdapter

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
Partial Class GoodReceiptDataset
    Partial Class __SCG_GOODRECEIVEDataTable

        Private Sub __SCG_GOODRECEIVEDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.U_FLETE_SColumn.ColumnName) Then
                'Agregar código de usuario aquí
            End If

        End Sub

    End Class

End Class
