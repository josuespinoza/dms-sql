Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGBusinessLogic

Namespace SCGDataAccess
    Public NotInheritable Class Configuracion

        Private Const mc_strComillaSimple As String = "'"

        Public Shared Function DevuelveValordeParametro(ByVal NombredeParametro As String, _
                                                        ByVal Compania As String, _
                                                        ByVal Database As String, _
                                                        ByVal objCompany As BLConexion) As String

            Dim cmdConsultar As SqlClient.SqlCommand
            Dim strQuery As String
            Dim strValorDeParametro As String
            Dim strCadenaConexion As String
            Dim m_cnnSCGTaller As SqlClient.SqlConnection = Nothing

            Try

                strCadenaConexion = CrearStrConexionADO(objCompany)

                m_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexion)

                Call m_cnnSCGTaller.Open()

                'Compania = Compania & "(" & objCompany.BaseDatosSBO & ")"

                strQuery = "SELECT 	[Valor]" & _
                           " FROM [SCGCONF_TA_ParametrosGenerales]" & _
                           " WHERE [NombreParametro]= " & NombredeParametro & _
                           "        and Compania=" & "'" & Compania & "'" & _
                           "        and SCGDatabase= " & "'" & Database & "'"

                cmdConsultar = New SqlClient.SqlCommand
                cmdConsultar.Connection = m_cnnSCGTaller

                With cmdConsultar

                    .CommandType = CommandType.Text
                    .CommandText = strQuery
                    strValorDeParametro = cmdConsultar.ExecuteScalar()
                End With

                Return strValorDeParametro

            Catch ex As Exception
                MsgBox(ex.Message & "    " & "DevuelveValordeParametro")
                Return ""
            Finally

                m_cnnSCGTaller.Close()

            End Try

        End Function


        Public Shared Function CrearStrConexionADO(ByVal objCompa�ia As BLConexion) As String

            Dim DatabaseName As String = "SCGConfiguracion"
            Dim strConectionString As String
            'Dim DatabaseServer As String = objCompa�ia.Server
            Try
                'Verifica si la conexi�n utiliza autenticaci�n de windows
                'Si utiliza Windows Autentication crea el string sin el Usuario y Password
                'Si No envia el Usuario y Password de Conexi�n

                strConectionString = "Data Source=" & objCompa�ia.Servidor & _
                                     ";Initial Catalog =" & DatabaseName & ";" & _
                                     "Connect Timeout=60;" & _
                                     "connection reset=false;" & _
                                     "connection lifetime=5;" & _
                                     "enlist=true;" & _
                                     "min pool size=1;" & _
                                     "max pool size=100;" & _
                                     "Pooling=true;" & _
                                     "User ID=" & objCompa�ia.Usuario & ";" & _
                                     "Pwd=" & objCompa�ia.Contrase�a

                If objCompa�ia.WinAuthentication Then
                    strConectionString &= ";Trusted_Connection=Yes"
                Else
                    strConectionString &= ";Trusted_Connection=No"
                End If

                Return strConectionString

            Catch ex As Exception
                Throw ex
            End Try

        End Function



    End Class
End Namespace

