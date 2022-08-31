Partial Class VehiculosAddonDataset
    Partial Class SCG_VEHICULODataTable

        Private Sub SCG_VEHICULODataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.CodeColumn.ColumnName) Then
                'Agregar código de usuario aquí
            End If

        End Sub

    End Class

End Class

Namespace VehiculosAddonDatasetTableAdapters

    Partial Class SCG_VEHICULOTableAdapter

        Public Sub SetTransaction(ByRef tnTransaccion As SqlClient.SqlTransaction)
            Dim cmdComand As SqlClient.SqlCommand
            For Each cmdComand In Me._commandCollection
                cmdComand.Transaction = tnTransaccion
            Next
            Me.Adapter.UpdateCommand.Transaction = tnTransaccion
'            Me.Adapter.InsertCommand.Transaction = tnTransaccion
            Me.Adapter.DeleteCommand.Transaction = tnTransaccion
        End Sub

    End Class

End Namespace

Namespace VehiculosAddonDatasetTableAdapters

    Partial Public Class SCG_VEHICULOTableAdapter
    End Class
End Namespace
