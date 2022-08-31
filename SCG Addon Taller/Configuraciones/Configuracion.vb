Public NotInheritable Class Configuracion


    'Ale
    Public Shared Function CrearCadenaDeconexion(ByVal strNombreServidor As String, _
                                                 ByVal BasedeDatosSCG As String, _
                                                 ByRef p_strCadenaDeConexion As String) As Boolean

        'Dim strConectionString As String

        Try
            'Verifica si la conexión utiliza autenticación de windows
            'Si utiliza Windows Autentication crea el string sin el Usuario y Password
            'Si No envia el Usuario y Password de Conexión

            p_strCadenaDeConexion = "Data Source=" & strNombreServidor.ToLower() & ";" & _
                                    "Initial Catalog=" & BasedeDatosSCG.ToLower() & ";" & _
                                    "Connect Timeout=120;" & _
                                    "User ID=" & CatchingEvents.DBUser & ";" & _
                                    "pwd=" & CatchingEvents.DBPassword & _
                                    ";Pooling=False"

            'If oCompany.WinAuthentication Then
            '    strConectionString &= ";Trusted_Connection=Yes"
            'Else
            '    
            'End If
            Return True

        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

    End Function

End Class
