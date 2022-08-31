Imports System.IO
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Configuration.Install
Imports Microsoft.SqlServer.Management.Smo
Imports Microsoft.SqlServer.Management.Common

Public Class Basics

    Private ServerConn As ServerConnection
    Private Servidor As Server

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Public Sub RestaurarDB(ByVal p_strNombreBaseDatos As String)
        Dim restaurar As New Restore
        Dim CollateCaseSensitive As Boolean

        Dim dt As New DataTable
        Dim NombreMDFOriginal As String
        Dim NombreLOGOriginal As String
        Dim strNombreDB As String
        Dim drwInfo As DataRow
        Dim drwInfo2 As DataRow

        Try

            strNombreDB = Path.GetFileNameWithoutExtension(p_strNombreBaseDatos)

            restaurar.Database = strNombreDB
            restaurar.Devices.AddDevice(m_strFolderOrigen & mc_RutaArchivoRespaldo & _
                        "\" & p_strNombreBaseDatos, DeviceType.File)

            ' Initialize the server settings
            Servidor.Settings.Initialize(True)

            ' Initialize the server information settings
            Servidor.Information.Initialize(True)

            ' Initialize the server
            Servidor.Initialize(True)


            CollateCaseSensitive = Servidor.Information.Properties("IsCaseSensitive").Value
            If CollateCaseSensitive = False Then

                dt = restaurar.ReadFileList(Servidor)
                drwInfo = dt.Rows(0)
                drwInfo2 = dt.Rows(1)

                NombreMDFOriginal = drwInfo(0)
                NombreLOGOriginal = drwInfo2(0)

                restaurar.RelocateFiles.Add(New RelocateFile(NombreMDFOriginal, m_pathBDServidor & "\" & strNombreDB & ".MDF"))
                restaurar.RelocateFiles.Add(New RelocateFile(NombreLOGOriginal, m_pathBDServidor & "\" & strNombreDB & "_Log.LDF"))

                restaurar.ReplaceDatabase = True

                Servidor.KillAllProcesses(strNombreDB)
                restaurar.Wait()
                restaurar.SqlRestore(Servidor)

            End If
        Catch ex As SmoException
            Throw ex
        End Try

    End Sub

    Public Sub CrearConexionServidor()
        Try


            If ServerConn Is Nothing Then
                ServerConn = New ServerConnection
            End If

            ServerConn.ServerInstance = m_NombreServidor

            ServerConn.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql

            If m_AutenticacionWindows Then

                ServerConn.LoginSecure = True

            Else

                ServerConn.LoginSecure = False
                ServerConn.Login = m_UsuarioServidor
                ServerConn.Password = m_ContraseñaServidor

            End If


            ServerConn.Connect()
            Servidor = New Server(ServerConn)

            m_pathBDServidor = Servidor.Information.Properties("MasterDBPath").Value.ToString
        Catch ex As SmoException
            Throw ex
        End Try
    End Sub

    Public Sub CerrarConexionServidor()
        Try


            If Not ServerConn Is Nothing Then
                ServerConn.Disconnect()
                Servidor = Nothing
                ServerConn = Nothing
            End If

        Catch ex As SmoException
            Throw ex
        End Try
    End Sub

    Public Sub CrearLoginSQL(ByVal strNombreUsuario As String, ByVal strContrasena As String)

        Dim bolExisteLogin As Boolean
        Dim log As Login
        Try

            For Each log In Servidor.Logins

                If log.Name = strNombreUsuario Then

                    'Ya existe el Login en el servidor
                    bolExisteLogin = True
                    Exit For

                End If
            Next
            If bolExisteLogin = False Then

                log = New Login(Servidor, strNombreUsuario)
                log.LoginType = LoginType.SqlLogin
                log.PasswordPolicyEnforced = False
                log.Create(strContrasena)


            End If

        Catch ex As SmoException
            Throw ex
        End Try

    End Sub

    Public Sub AsignarUsuarioBaseDatos(ByVal p_strBaseDatos As String, ByVal strNombreUsuario As String)

        Dim db As Database
        Dim usr As User
        Dim sch As Schema
        Dim strBaseDatos As String

        'Asignarlo a una base de datos
        Try

            strBaseDatos = Path.GetFileNameWithoutExtension(p_strBaseDatos)

            db = CType(Servidor.Databases(strBaseDatos), Database)

            If Not db Is Nothing Then

                If Not db.Schemas Is Nothing Then

                    For Each sch In db.Schemas
                        If sch.Name = strNombreUsuario Then

                            sch.Drop()

                            Exit For

                        End If
                    Next

                End If

                If Not db.Users Is Nothing Then

                    For Each usr In db.Users
                        If usr.Name = strNombreUsuario Then

                            usr.Drop()

                            Exit For
                        End If
                    Next

                End If

                usr = New User(db, strNombreUsuario)

                db.IsDbOwner = True

                usr.DefaultSchema = "dbo"

                usr.Login = strNombreUsuario
                usr.UserType = UserType.SqlLogin
                usr.Create()
                usr.AddToRole("db_owner")

            End If
        Catch ex As SmoException
            Throw ex
        End Try
    End Sub

    Public Overrides Sub Install(ByVal stateSaver As _
      System.Collections.IDictionary)

        Try
            MyBase.Install(stateSaver)

            m_strPFF = Me.Context.Parameters.Item("strProgramFilesFolder")
            m_strFolderDestino = Me.Context.Parameters.Item("strDestinoFolder")
            m_strFolderOrigen = Me.Context.Parameters.Item("strOrigenFolder")
            m_NombreServidor = Me.Context.Parameters.Item("strServer")
            m_UsuarioServidor = Me.Context.Parameters.Item("strUserDB")
            m_ContraseñaServidor = Me.Context.Parameters.Item("strPassDB")

            If m_UsuarioServidor <> "" And m_ContraseñaServidor <> "" Then
                m_AutenticacionWindows = False
            End If

            CrearConexionServidor()

            CrearLoginSQL(mc_strSQLAddonUser, mc_strSQLAddonPass)

            If File.Exists(m_strFolderOrigen & "\" & mc_RutaArchivoRespaldo & "\" & mc_NombreBaseDatosSCGConfg) Then
                RestaurarDB(mc_NombreBaseDatosSCGConfg)
                AsignarUsuarioBaseDatos(mc_NombreBaseDatosSCGConfg, mc_strSQLAddonUser)
            End If

            If File.Exists(m_strFolderOrigen & "\" & mc_RutaArchivoRespaldo & "\" & mc_NombreBaseDatosSCGConfg) Then
                RestaurarDB(mc_NombreBaseDatosSBO)
                AsignarUsuarioBaseDatos(mc_NombreBaseDatosSBO, mc_strSQLAddonUser)
            End If

            If File.Exists(m_strFolderOrigen & "\" & mc_RutaArchivoRespaldo & "\" & mc_NombreBaseDatosSCGConfg) Then
                RestaurarDB(mc_NombreBaseDatosSCGDMS)
                AsignarUsuarioBaseDatos(mc_NombreBaseDatosSCGDMS, mc_strSQLAddonUser)
            End If

            AsignarUsuarioBaseDatos("SBO-COMMON.bak", mc_strSQLAddonUser)

            CerrarConexionServidor()

        Catch ex As Exception
            MsgBox(ex.Message)
            Throw ex
        End Try

    End Sub

End Class
