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

        Public Function InicializarCompa�ia(ByVal objCompa�ia As clsCompany) As Boolean

            Try
                G_objCompany = New SAPbobsCOM.Company

                G_objCompany.Server = objCompa�ia.Server
                G_objCompany.language = SAPbobsCOM.BoSuppLangs.ln_English
                G_objCompany.UseTrusted = objCompa�ia.WinAuthentication

                InicializarCompa�ia = True

            Catch ex As Exception
                Throw ex
                InicializarCompa�ia = False
            End Try

        End Function

        Public Function ConectarCompa�ia(ByVal objCompa�ia As clsCompany) As Boolean

            If G_objCompany Is Nothing Then
                InicializarCompa�ia(objCompa�ia)
            End If

            Try
                G_objCompany.Server = objCompa�ia.Server
                G_objCompany.UserName = objCompa�ia.UserName
                G_objCompany.Password = objCompa�ia.Password
                G_objCompany.CompanyDB = objCompa�ia.DataBase
                G_objCompany.DbUserName = objCompa�ia.DBUser
                G_objCompany.DbPassword = objCompa�ia.DBPassword
                G_objCompany.LicenseServer = objCompa�ia.LicenseServer

                G_objCompany.DbServerType = objCompa�ia.TipoServidor

                'Dim a As String = System.Configuration.ConfigurationSettings.AppSettings.Item("TipoServidor")

                intcodigo = G_objCompany.Connect()

                If intcodigo = 0 Then

                    With New BLSBO.GlobalFunctionsSBO
                        .Set_Compania(G_objCompany)
                        .Set_DB_SCG(objCompa�ia.BaseDatosSCG)
                    End With
                    USUARIO_SISTEMA = objCompa�ia.UserName
                    Return True
                Else
                    G_objCompany.GetLastError(intcodigo, strMensaje)
                    CreaLinea("C�digo: " & CStr(intcodigo) & " Descripci�n: " & strMensaje)
                    Throw New ExceptionsSBO(intcodigo, strMensaje)
                End If

            Catch ex As ExceptionsSBO
                Throw ex
            Catch ex As Exception
                CreaLinea("C�digo: .NET Error Descripci�n: " & ex.Message)
                Throw ex
            End Try

        End Function

        'Metodo que carga un ArrayList(al) que contiene el listado de compa�ias
        'validas para ese servidor con el tipo de conexion especifica
        'Public Function DevuelveCompa�ias() As ArrayList

        '    Dim alListaCompa�ias As ArrayList
        '    Dim objCompa�ia As clsCompany

        '    Try
        '        alListaCompa�ias = New ArrayList

        '        oRxecordset = G_objCompany.GetCompanyList
        '        G_objCompany.GetLastError(intcodigo, strMensaje)


        '        If intcodigo <> 0 Then
        '            Throw New Exception 'ExceptionsSBO(intcodigo)
        '        End If

        '        Do Until oRxecordset.EoF = True

        '            objCompa�ia = New clsCompany
        '            objCompa�ia.DataBase = (oRxecordset.Fields.Item(0).Value)
        '            objCompa�ia.Company = (oRxecordset.Fields.Item(1).Value)

        '            alListaCompa�ias.Add(objCompa�ia)

        '            oRxecordset.MoveNext()
        '        Loop

        '        DevuelveCompa�ias = alListaCompa�ias

        '    Catch ex As ExceptionsSBO
        '        Throw ex

        '    Catch ex As Exception
        '        Throw ex

        '    Finally
        '        alListaCompa�ias = Nothing
        '    End Try
        'End Function

        Public Sub CrearStrConexionADO(ByVal objCompa�ia As clsCompany)

            Dim DatabaseServer As String = objCompa�ia.Server


            Try
                'Verifica si la conexi�n utiliza autenticaci�n de windows
                'Si utiliza Windows Autentication crea el string sin el Usuario y Password
                'Si No envia el Usuario y Password de Conexi�n

                strConectionString = "Data Source=" & objCompa�ia.Server.ToLower() & ";" & _
                                     "Initial Catalog=" & objCompa�ia.BaseDatosSCG.ToLower() & ";" & _
                                     "Connect Timeout=120"

                If objCompa�ia.WinAuthentication Then
                    strConectionString &= ";Trusted_Connection=Yes"
                Else
                    strConectionString &= ";User ID=" & objCompa�ia.UserName & _
                                          ";pwd=" & objCompa�ia.Password & _
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

        Public Function CadenaConexionSBO(ByVal objCompa�ia As clsCompany) As String
            Dim strConectionString As String
            strConectionString = "Data Source=" & objCompa�ia.Server.ToLower() & ";" & _
                                 "Initial Catalog=" & objCompa�ia.DataBase.ToLower() & ";" & _
                                 "Connect Timeout=120"

            If objCompa�ia.WinAuthentication Then
                strConectionString &= ";Trusted_Connection=Yes"
            Else
                strConectionString &= ";User ID=" & objCompa�ia.UserName & _
                                      ";pwd=" & objCompa�ia.Password & _
                                       ";Pooling=False"

            End If

            Return strConectionString
        End Function

    End Class
End Namespace

