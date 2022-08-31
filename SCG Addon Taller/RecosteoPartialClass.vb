Partial Public Class RecosteoDataSet

End Class

Namespace RecosteoDataSetTableAdapters

    Partial Public Class FormulariosTableAdapter

        Public Sub SetTimeOut(ByVal p_time As Integer)


            For Each command As SqlClient.SqlCommand In Me.CommandCollection

                command.CommandTimeout = p_time
            Next

        End Sub

    End Class

    Partial Public Class FacturaClientesTableAdapter

        Public Sub SetTimeOut(ByVal p_time As Integer)


            For Each command As SqlClient.SqlCommand In Me.CommandCollection

                command.CommandTimeout = p_time
            Next

        End Sub

    End Class

    Partial Public Class AsientosTableAdapter

        Public Sub SetTimeOut(ByVal p_time As Integer)


            For Each command As SqlClient.SqlCommand In Me.CommandCollection

                command.CommandTimeout = p_time
            Next

        End Sub

    End Class

    Partial Public Class SaldosInicialesTableAdapter

        Public Sub SetTimeOut(ByVal p_time As Integer)


            For Each command As SqlClient.SqlCommand In Me.CommandCollection

                command.CommandTimeout = p_time
            Next

        End Sub

    End Class

    Partial Public Class AsientoSalidaInventarioTableAdapter

        Public Sub SetTimeOut(ByVal p_time As Integer)


            For Each command As SqlClient.SqlCommand In Me.CommandCollection

                command.CommandTimeout = p_time
            Next

        End Sub

    End Class

    Partial Public Class NotaCreditoProveedorDataAdapter

        Public Sub SetTimeOut(ByVal p_time As Integer)


            For Each command As SqlClient.SqlCommand In Me.CommandCollection

                command.CommandTimeout = p_time
            Next

        End Sub

    End Class


End Namespace