Namespace SCGDataAccess
    Public Module GlobalesDA

        Public G_objCompany As SAPbobsCOM.Company
        Public strConexionADO As String
        ''' <summary>
        ''' Cadena de conexion a la base de datos de SBO
        ''' </summary>
        ''' <remarks></remarks>
        Public strConexionSBO As String
        Public USUARIO_SISTEMA As String
        Public COMPANY As String
        Friend Const G_ArchivoErrores As String = "ErroresConexion.txt"
        Public objBLConexion As SCGBusinessLogic.BLConexion
        Public g_strServidorLicencia As String


#Region "Procedimientos"

        Friend Sub CloseConnection(ByRef p_Command As SqlClient.SqlCommand)
            If Not IsNothing(p_Command) Then
                If Not IsNothing(p_Command.Connection) Then
                    p_Command.Connection.Close()
                End If
                p_Command.Dispose()
            End If
        End Sub

#End Region

    End Module

End Namespace
