Public Class DataGridCheckColumn
    Inherits DataGridBoolColumn

#Region "Declaraciones"

    Public Event CambioValueSingle(ByVal p_intRow As Integer)

#End Region

#Region "Procedimientos"

    Protected Overrides Sub SetColumnValueAtRow(ByVal lm As System.Windows.Forms.CurrencyManager, ByVal row As Integer, ByVal value As Object)
        MyBase.SetColumnValueAtRow(lm, row, value)
        RaiseEvent CambioValueSingle(row)
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer, ByVal bounds As System.Drawing.Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)

        If Me.GetColumnValueAtRow(source, rowNum) Then
            Me.SetColumnValueAtRow(source, rowNum, False)
        Else
            Me.SetColumnValueAtRow(source, rowNum, True)
        End If

        'MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)

    End Sub

#End Region

End Class
