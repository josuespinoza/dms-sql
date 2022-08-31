Imports System.Data.SqlClient
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGBusinessLogic

Namespace SCGDataAccess
    Public Class DAConexion

        'Objeto Rxecordset del SDK
        'Dim oRxecordset As SAPbobsCOM.Rxecordset
        'Dim intCodError As Integer

        Dim intcodigo As Integer
        Dim strMensaje As String

        Public Shared strConectionString As String

        'Constantes
        Private Const CONNECTIONSTRING_DEFAULT As String = "Server=(local); User ID=sa; Database=SCGDMSOne"

        Public Shared ReadOnly Property ConnectionString() As String
            Get
                ConnectionString = strConectionString
            End Get
        End Property

        '****************************** METODOS ********************************

        Public Function InicializarCompañia(ByVal objCompañia As clsCompany) As Boolean

            Try
                G_objCompany = New SAPbobsCOM.Company

                G_objCompany.Server = objCompañia.Server
                G_objCompany.language = SAPbobsCOM.BoSuppLangs.ln_English
                G_objCompany.UseTrusted = objCompañia.WinAuthentication

                InicializarCompañia = True

            Catch ex As Exception
                Throw ex
                InicializarCompañia = False
            End Try

        End Function

        Public Function ConectarCompañia(ByVal objCompañia As clsCompany) As Boolean

            If G_objCompany Is Nothing Then
                InicializarCompañia(objCompañia)
            End If

            Try
                G_objCompany.Server = objCompañia.Server
                G_objCompany.UserName = objCompañia.UserName
                G_objCompany.Password = objCompañia.Password
                G_objCompany.CompanyDB = objCompañia.DataBase
                G_objCompany.DbUserName = objCompañia.DBUser
                G_objCompany.DbPassword = objCompañia.DBPassword
                G_objCompany.LicenseServer = objCompañia.LicenseServer

                G_objCompany.DbServerType = objCompañia.TipoServidor

                'Dim a As String = System.Configuration.ConfigurationSettings.AppSettings.Item("TipoServidor")

                intcodigo = G_objCompany.Connect()

                If intcodigo = 0 Then

                    With New BLSBO.GlobalFunctionsSBO
                        .Set_Compania(G_objCompany)
                        .Set_DB_SCG(objCompañia.BaseDatosSCG)
                    End With
                    USUARIO_SISTEMA = objCompañia.UserName
                    Return True
                Else
                    G_objCompany.GetLastError(intcodigo, strMensaje)
                    CreaLinea("Código: " & CStr(intcodigo) & " Descripción: " & strMensaje)
                    Throw New ExceptionsSBO(intcodigo, strMensaje)
                End If

            Catch ex As ExceptionsSBO
                Throw ex
            Catch ex As Exception
                CreaLinea("Código: .NET Error Descripción: " & ex.Message)
                Throw ex
            End Try

        End Function

        'Metodo que carga un ArrayList(al) que contiene el listado de compañias
        'validas para ese servidor con el tipo de conexion especifica
        'Public Function DevuelveCompañias() As ArrayList

        '    Dim alListaCompañias As ArrayList
        '    Dim objCompañia As clsCompany

        '    Try
        '        alListaCompañias = New ArrayList

        '        oRxecordset = G_objCompany.GetCompanyList
        '        G_objCompany.GetLastError(intcodigo, strMensaje)


        '        If intcodigo <> 0 Then
        '            Throw New Exception 'ExceptionsSBO(intcodigo)
        '        End If

        '        Do Until oRxecordset.EoF = True

        '            objCompañia = New clsCompany
        '            objCompañia.DataBase = (oRxecordset.Fields.Item(0).Value)
        '            objCompañia.Company = (oRxecordset.Fields.Item(1).Value)

        '            alListaCompañias.Add(objCompañia)

        '            oRxecordset.MoveNext()
        '        Loop

        '        DevuelveCompañias = alListaCompañias

        '    Catch ex As ExceptionsSBO
        '        Throw ex

        '    Catch ex As Exception
        '        Throw ex

        '    Finally
        '        alListaCompañias = Nothing
        '    End Try
        'End Function

        Public Sub CrearStrConexionADO(ByVal objCompañia As clsCompany)

            Dim DatabaseServer As String = objCompañia.Server


            Try
                'Verifica si la conexión utiliza autenticación de windows
                'Si utiliza Windows Autentication crea el string sin el Usuario y Password
                'Si No envia el Usuario y Password de Conexión

                strConectionString = "Data Source=" & objCompañia.Server.ToLower() & ";" & _
                                     "Initial Catalog=" & objCompañia.BaseDatosSCG.ToLower() & ";" & _
                                     "Connect Timeout=120"

                If objCompañia.WinAuthentication Then
                    strConectionString &= ";Trusted_Connection=Yes"
                Else
                    strConectionString &= ";User ID=" & objCompañia.UserName & _
                                          ";pwd=" & objCompañia.Password & _
                                           ";Pooling=False"

                End If

                strConexionADO = strConectionString

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Function ObtieneConexion(ByVal p_strServidor As String, _
                                        ByVal p_strBaseDatos As String, _
                                        ByVal p_strUsuario As String, _
                                        ByVal p_strPassword As String) As SqlConnection
            Dim sqlconexion As SqlConnection
            Dim strStringDeConeccion As String
            Try

                strStringDeConeccion = "Data Source=" & p_strServidor.ToLower() & ";" & _
                                       "Initial Catalog=" & p_strBaseDatos.ToLower() & ";" & _
                                       "Connect Timeout=120;" & _
                                       "User ID=" & p_strUsuario & ";" & _
                                       "pwd=" & p_strPassword & _
                                       ";Pooling=False"

                'strStringDeConeccion &= ";Trusted_Connection=No"

                sqlconexion = New SqlConnection(strStringDeConeccion)
                sqlconexion.Open()

                Return sqlconexion

            Catch ex As Exception
                Throw ex
            Finally
                'Agregado 02072010
            End Try
        End Function

        Public Function ObtieneConexion() As SqlConnection
            Dim sqlconexion As SqlConnection

            Try
                If Not String.IsNullOrEmpty(DAConexion.ConnectionString) Then
                    sqlconexion = New SqlConnection(DAConexion.ConnectionString)
                    sqlconexion.Open()

                    Return sqlconexion
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        Public Function PathReportes(ByVal strDBSCG As String) As String
            Dim cmdCommand As New SqlCommand("SELECT DireccionReportes FROM SCGConfiguracion.dbo.SCGCONF_PARAMETROS WHERE SCGDataBase = '" & strDBSCG & "'", ObtieneConexion)
            Dim strDir As String = ""

            Using sqlReader As SqlDataReader = cmdCommand.ExecuteReader
                'sqlReader = cmdCommand.ExecuteReader
                While sqlReader.Read
                    strDir = CStr(sqlReader("DIRECCIONREPORTES")).Trim
                End While
            End Using

            'Dim sqlReader As SqlDataReader

            'sqlReader.Close()
            cmdCommand.Dispose()
            PathReportes = strDir
        End Function

        Public Sub CreaLinea(ByVal strMensgError As String)
            Dim objStream As IO.StreamWriter
            Dim strNombreArchivoFULL As String
            Dim strFecha As String
            Dim strHora As String

            strNombreArchivoFULL = Windows.Forms.Application.StartupPath & "\" & G_ArchivoErrores

            strFecha = Today.ToString("yyyy/MM/dd")
            strHora = Now.ToString("hh:mm:ss")

            objStream = New IO.StreamWriter(strNombreArchivoFULL, True)

            objStream.WriteLine(strFecha & " " & strHora & " " & strMensgError)

            objStream.Close()
        End Sub

        Public Function ExtraerPathReportes(ByVal strDataBaseSCG As String) As String
            Dim oDAConexion As New DAConexion
            Try
                ExtraerPathReportes = oDAConexion.PathReportes(strDataBaseSCG)
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function CadenaConexionSBO(ByVal objCompañia As clsCompany) As String
            Dim strConectionString As String
            strConectionString = "Data Source=" & objCompañia.Server.ToLower() & ";" & _
                                 "Initial Catalog=" & objCompañia.DataBase.ToLower() & ";" & _
                                 "Connect Timeout=120"

            If objCompañia.WinAuthentication Then
                strConectionString &= ";Trusted_Connection=Yes"
            Else
                strConectionString &= ";User ID=" & objCompañia.UserName & _
                                      ";pwd=" & objCompañia.Password & _
                                       ";Pooling=False"

            End If

            Return strConectionString
        End Function

    End Class
End Namespace

