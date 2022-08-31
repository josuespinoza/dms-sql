Partial Class RepuestosxOrdenDataset
    Partial Class SCGTA_TB_RepuestosxOrdenDataTable

        Private Sub SCGTA_TB_RepuestosxOrdenDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.ResultadoActividadColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
