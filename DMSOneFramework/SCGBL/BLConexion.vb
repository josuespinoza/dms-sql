Option Strict On
Option Explicit On 
Imports System.Data
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess

Namespace SCGBusinessLogic
    Public Class BLConexion
        'Variables para las Propiedades
        Private strServidor As String
        Private strBaseDatosSCG As String
        Private strBaseDatosSBO As String
        Private strCompañia As String
        Private strUsuario As String
        Private strUsuarioAplicacion As String
        Private strContraseña As String
        Private strDBUser As String
        Private strDBPassword As String
        Private strLicenseSever As String
        Private BlnWinAuthentication As Boolean
        Private serverType As String

        'Objeto de Tipo Data Access Conexion 
        Private objDAConexion As DAConexion

        'Variable para Evaluar el resultado de la conexion
        Dim blnConectar As Boolean

#Region "Propiedades"
        Public Property TipoServidor() As String
            Get
                Return serverType
            End Get
            Set(ByVal value As String)
                serverType = value
            End Set
        End Property

        'Propiedades del Objeto BLConexion
        Public Property Servidor() As String
            Get
                Return strServidor
            End Get
            Set(ByVal Value As String)
                If (Value.Trim <> "") Then
                    strServidor = Value
                Else
                    Throw New Exception 'ExceptionsSBO(-114)
                End If
            End Set
        End Property

        Public Property BaseDatosSCG() As String
            Get
                Return strBaseDatosSCG
            End Get
            Set(ByVal Value As String)
                strBaseDatosSCG = Value
            End Set
        End Property

        Public Property BaseDatosSBO() As String
            Get
                Return strBaseDatosSBO
            End Get
            Set(ByVal Value As String)
                strBaseDatosSBO = Value
            End Set
        End Property

        Public Property UsuarioAplicacion() As String
            Get
                Return strUsuarioAplicacion
            End Get
            Set(ByVal Value As String)
                strUsuarioAplicacion = Value
            End Set
        End Property

        Public Property Usuario() As String
            Get
                Return strUsuario
            End Get
            Set(ByVal Value As String)
                strUsuario = Value
            End Set
        End Property

        Public Property Contraseña() As String
            Get
                Return strContraseña
            End Get
            Set(ByVal Value As String)
                strContraseña = Value
            End Set
        End Property

        Public Property DBUser() As String
            Get
                Return strDBUser
            End Get
            Set(ByVal Value As String)
                strDBUser = Value
            End Set
        End Property

        Public Property DBPassword() As String
            Get
                Return strDBPassword
            End Get
            Set(ByVal Value As String)
                strDBPassword = Value
            End Set
        End Property

        Public Property Compañia() As String
            Get
                Return strCompañia
            End Get
            Set(ByVal Value As String)
                strCompañia = Value
            End Set
        End Property

        Public Property LicenseServer() As String
            Get
                Return strLicenseSever
            End Get
            Set(ByVal Value As String)
                strLicenseSever = Value
            End Set
        End Property

        Public Property WinAuthentication() As Boolean
            Get
                Return BlnWinAuthentication
            End Get
            Set(ByVal Value As Boolean)
                BlnWinAuthentication = Value
            End Set
        End Property

#End Region

        'Metodos del Objeto Conexion
        'Public Function ObtieneListaCompañias() As ArrayList

        '    'Dim objCompañia As clsCompany
        '    Dim alListaCompañias As ArrayList
        '    'Dim intCantCompañias As Integer

        '    Try
        '        alListaCompañias = objDAConexion.DevuelveCompañias
        '        If Not IsNothing(alListaCompañias) Then
        '            'intCantCompañias = alListaCompañias.Count - 1
        '            Return alListaCompañias
        '        Else
        '            Return Nothing
        '        End If


        '    Catch ex As ExceptionsSBO
        '        Throw ex

        '    Catch ex As Exception
        '        Throw ex

        '    End Try

        'End Function

        Public Function InicializaCompañia() As Boolean
            'Este metodo envia los valores del Server y WinAuthentication para
            'inicializar la conexion con la compañia respectiva

            Dim objCompañia As clsCompany

            Try
                objDAConexion = New DAConexion

                'Se le asignan las propiedades de Servidor y Tipo de Conexion
                objCompañia = New clsCompany
                objCompañia.Server = Me.Servidor
                objCompañia.WinAuthentication = Me.WinAuthentication

                If objDAConexion.InicializarCompañia(objCompañia) Then
                    InicializaCompañia = True
                Else
                    InicializaCompañia = False
                End If

                'Catch ex As Exception 'ExceptionsSBO
                '    Throw ex
                '    InicializaCompañia = False

            Catch ex As Exception
                Throw ex
                InicializaCompañia = False

            Finally
                'Se debe tener un metodo en el DA para que se pueda destruir el objeto
                'y liberar la memoria
            End Try

        End Function

        'esta función inicializa la conexión con las compañía
        Public Function ConectarCompañia() As Boolean

            Dim objCompañia As SCGCommon.clsCompany

            If objDAConexion Is Nothing Then
                objDAConexion = New DAConexion
            End If

            Try

                objCompañia = New SCGCommon.clsCompany
                objCompañia.Company = Me.Compañia
                objCompañia.Server = Me.Servidor
                objCompañia.DataBase = Me.BaseDatosSBO
                objCompañia.BaseDatosSCG = Me.BaseDatosSCG
                objCompañia.UserName = Me.Usuario
                objCompañia.Password = Me.Contraseña
                objCompañia.DBUser = Me.DBUser
                objCompañia.DBPassword = Me.DBPassword
                objCompañia.UsuarioSistema = Me.UsuarioAplicacion
                objCompañia.WinAuthentication = Me.WinAuthentication
                objCompañia.LicenseServer = Me.LicenseServer
                objCompañia.TipoServidor = Me.TipoServidor

                blnConectar = objDAConexion.ConectarCompañia(objCompañia)

                If blnConectar Then
                    'Define el string de Conexion para ADO
                    objDAConexion.CrearStrConexionADO(objCompañia)
                    strConexionSBO = objDAConexion.CadenaConexionSBO(objCompañia)
                    'objDAConexion.AbrirConexion() 'Revisar esto

                    USUARIO_SISTEMA = objCompañia.UsuarioSistema
                    g_strServidorLicencia = objCompañia.LicenseServer

                    Return True
                Else
                    Return False
                End If

            Catch ex As ExceptionsSBO
                Throw ex
                Return False

            Catch ex As Exception
                Throw ex
                Return False

            End Try

        End Function

        Public Function ExtraerPathReportes(ByVal strDataBaseSCG As String) As String
            Dim oDAConexion As New DAConexion
            Try
                ExtraerPathReportes = oDAConexion.PathReportes(strDataBaseSCG)
            Catch ex As Exception
                Throw ex
            End Try

        End Function


    End Class
End Namespace
