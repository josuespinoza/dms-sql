Partial Class ItemsSAPDataset
    Partial Class SCGTA_TB_ItemsSAPDataTable

        Private Sub SCGTA_TB_ItemsSAPDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ItemNameColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
