Public Class LineaRestablecerCantidades
    Public DocEntry As Integer
    Public LineNum As Integer
    Public ItemCode As String
    Public ItemName As String
    Public CantidadEntrada As Double
    Public TipoArticulo As Integer
    Public TipoTransaccion As Integer
    Public DataTableLine As Integer 'Línea de la tabla en memoria, no confundir con la línea del documento

    Sub New(ByVal p_DocEntry As Integer, ByVal p_LineNum As Integer, ByVal p_ItemCode As String, ByVal p_ItemName As String,
            ByVal p_CantidadEntrada As Double, ByVal p_TipoArticulo As Integer, ByVal p_TipoTransaccion As Integer, ByVal p_DataTableLine As Integer)
        DocEntry = p_DocEntry
        LineNum = p_LineNum
        ItemCode = p_ItemCode
        ItemName = p_ItemName
        CantidadEntrada = p_CantidadEntrada
        TipoArticulo = p_TipoArticulo
        TipoTransaccion = p_TipoTransaccion
        DataTableLine = p_DataTableLine
    End Sub
End Class
