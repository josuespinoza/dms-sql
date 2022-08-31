

Partial Public Class AgendaDataset
    Partial Class SCGTA_TB_AgendasDataTable

        Private Sub SCGTA_TB_AgendasDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.DescripcionTecnicoColumn.ColumnName) Then
                'Add user code here
            End If

        End Sub

    End Class

End Class
