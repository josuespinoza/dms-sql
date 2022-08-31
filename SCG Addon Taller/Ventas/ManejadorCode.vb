Imports System.Data.SqlClient

Namespace Ventas
    Public Class ManejadorCode
        Private _conexion As String

        Public Sub New(ByVal conexion As String)
            _conexion = conexion
        End Sub

        Public Function ObtieneCode() As String
            Dim dataReader As SqlDataReader
            Dim conexionSql As SqlConnection
            Dim command As SqlCommand
            Dim code As String = String.Empty

            conexionSql = New SqlConnection(_conexion)
            Try
                conexionSql.Open()
                command = New SqlCommand("SELECT MAX (CONVERT(INT, Code)) FROM [@SCGD_PRESUPUESTOS]", conexionSql)
                command.CommandType = CommandType.Text
                dataReader = command.ExecuteReader()

                While dataReader.Read
                    If Not dataReader.IsDBNull(0) Then code = dataReader.Item(0)
                End While
                If (dataReader IsNot Nothing) Then dataReader.Close()
            Catch
                Throw
            Finally
                conexionSql.Close()
            End Try

            If String.IsNullOrEmpty(code) Then
                code = "1"
            Else
                code = CStr(CInt(code) + 1)
            End If

            Return code
        End Function
    End Class
End Namespace