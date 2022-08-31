Partial Class ActividadesXFaseDataset
    Partial Class SCGTA_TB_ActividadesxOrdenDataTable

        Private Sub SCGTA_TB_ActividadesxOrdenDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.DescripcionActividadResourcesColumn.ColumnName) Then
                'Agregar código de usuario aquí
            End If

        End Sub

    End Class

End Class
