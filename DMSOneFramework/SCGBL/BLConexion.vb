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
        Private strCompa�ia As String
        Private strUsuario As String
        Private strUsuarioAplicacion As String
        Private strContrase�a As String
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

        Public Property Contrase�a() As String
            Get
                Return strContrase�a
            End Get
            Set(ByVal Value As String)
                strContrase�a = Value
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

        Public Property Compa�ia() As String
            Get
                Return strCompa�ia
            End Get
            Set(ByVal Value As String)
                strCompa�ia = Value
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
        'Public Function ObtieneListaCompa�ias() As ArrayList

        '    'Dim objCompa�ia As clsCompany
        '    Dim alListaCompa�ias As ArrayList
        '    'Dim intCantCompa�ias As Integer

        '    Try
        '        alListaCompa�ias = objDAConexion.DevuelveCompa�ias
        '        If Not IsNothing(alListaCompa�ias) Then
        '            'intCantCompa�ias = alListaCompa�ias.Count - 1
        '            Return alListaCompa�ias
        '        Else
        '            Return Nothing
        '        End If


        '    Catch ex As ExceptionsSBO
        '        Throw ex

        '    Catch ex As Exception
        '        Throw ex

        '    End Try

        'End Function

        Public Function InicializaCompa�ia() As Boolean
            'Este metodo envia los valores del Server y WinAuthentication para
            'inicializar la conexion con la compa�ia respectiva

            Dim objCompa�ia As clsCompany

            Try
                objDAConexion = New DAConexion

                'Se le asignan las propiedades de Servidor y Tipo de Conexion
                objCompa�ia = New clsCompany
                objCompa�ia.Server = Me.Servidor
                objCompa�ia.WinAuthentication = Me.WinAuthentication

                If objDAConexion.InicializarCompa�ia(objCompa�ia) Then
                    InicializaCompa�ia = True
                Else
                    InicializaCompa�ia = False
                End If

                'Catch ex As Exception 'ExceptionsSBO
                '    Throw ex
                '    InicializaCompa�ia = False

            Catch ex As Exception
                Throw ex
                InicializaCompa�ia = False

            Finally
                'Se debe tener un metodo en el DA para que se pueda destruir el objeto
                'y liberar la memoria
            End Try

        End Function

        'esta funci�n inicializa la conexi�n con las compa��a
        Public Function ConectarCompa�ia() As Boolean

            Dim objCompa�ia As SCGCommon.clsCompany

            If objDAConexion Is Nothing Then
                objDAConexion = New DAConexion
            End If

            Try

                objCompa�ia = New SCGCommon.clsCompany
                objCompa�ia.Company = Me.Compa�ia
                objCompa�ia.Server = Me.Servidor
                objCompa�ia.DataBase = Me.BaseDatosSBO
                objCompa�ia.BaseDatosSCG = Me.BaseDatosSCG
                objCompa�ia.UserName = Me.Usuario
                objCompa�ia.Password = Me.Contrase�a
                objCompa�ia.DBUser = Me.DBUser
                objCompa�ia.DBPassword = Me.DBPassword
                objCompa�ia.UsuarioSistema = Me.UsuarioAplicacion
                objCompa�ia.WinAuthentication = Me.WinAuthentication
                objCompa�ia.LicenseServer = Me.LicenseServer
                objCompa�ia.TipoServidor = Me.TipoServidor

                blnConectar = objDAConexion.ConectarCompa�ia(objCompa�ia)

                If blnConectar Then
                    'Define el string de Conexion para ADO
                    objDAConexion.CrearStrConexionADO(objCompa�ia)
                    strConexionSBO = objDAConexion.CadenaConexionSBO(objCompa�ia)
                    'objDAConexion.AbrirConexion() 'Revisar esto

                    USUARIO_SISTEMA = objCompa�ia.UsuarioSistema
                    g_strServidorLicencia = objCompa�ia.LicenseServer

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
