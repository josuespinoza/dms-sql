

Partial Public Class OrdenTrabajoDataset
    Partial Class SCGTA_TB_OrdenDataTable

        Private Sub SCGTA_TB_OrdenDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.DescUbicacionColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
