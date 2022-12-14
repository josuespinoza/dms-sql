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
        Private strCompaņia As String
        Private strUsuario As String
        Private strUsuarioAplicacion As String
        Private strContraseņa As String
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

        Public Property Contraseņa() As String
            Get
                Return strContraseņa
            End Get
            Set(ByVal Value As String)
                strContraseņa = Value
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

        Public Property Compaņia() As String
            Get
                Return strCompaņia
            End Get
            Set(ByVal Value As String)
                strCompaņia = Value
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
        'Public Function ObtieneListaCompaņias() As ArrayList

        '    'Dim objCompaņia As clsCompany
        '    Dim alListaCompaņias As ArrayList
        '    'Dim intCantCompaņias As Integer

        '    Try
        '        alListaCompaņias = objDAConexion.DevuelveCompaņias
        '        If Not IsNothing(alListaCompaņias) Then
        '            'intCantCompaņias = alListaCompaņias.Count - 1
        '            Return alListaCompaņias
        '        Else
        '            Return Nothing
        '        End If


        '    Catch ex As ExceptionsSBO
        '        Throw ex

        '    Catch ex As Exception
        '        Throw ex

        '    End Try

        'End Function

        Public Function InicializaCompaņia() As Boolean
            'Este metodo envia los valores del Server y WinAuthentication para
            'inicializar la conexion con la compaņia respectiva

            Dim objCompaņia As clsCompany

            Try
                objDAConexion = New DAConexion

                'Se le asignan las propiedades de Servidor y Tipo de Conexion
                objCompaņia = New clsCompany
                objCompaņia.Server = Me.Servidor
                objCompaņia.WinAuthentication = Me.WinAuthentication

                If objDAConexion.InicializarCompaņia(objCompaņia) Then
                    InicializaCompaņia = True
                Else
                    InicializaCompaņia = False
                End If

                'Catch ex As Exception 'ExceptionsSBO
                '    Throw ex
                '    InicializaCompaņia = False

            Catch ex As Exception
                Throw ex
                InicializaCompaņia = False

            Finally
                'Se debe tener un metodo en el DA para que se pueda destruir el objeto
                'y liberar la memoria
            End Try

        End Function

        'esta función inicializa la conexión con las compaņía
        Public Function ConectarCompaņia() As Boolean

            Dim objCompaņia As SCGCommon.clsCompany

            If objDAConexion Is Nothing Then
                objDAConexion = New DAConexion
            End If

            Try

                objCompaņia = New SCGCommon.clsCompany
                objCompaņia.Company = Me.Compaņia
                objCompaņia.Server = Me.Servidor
                objCompaņia.DataBase = Me.BaseDatosSBO
                objCompaņia.BaseDatosSCG = Me.BaseDatosSCG
                objCompaņia.UserName = Me.Usuario
                objCompaņia.Password = Me.Contraseņa
                objCompaņia.DBUser = Me.DBUser
                objCompaņia.DBPassword = Me.DBPassword
                objCompaņia.UsuarioSistema = Me.UsuarioAplicacion
                objCompaņia.WinAuthentication = Me.WinAuthentication
                objCompaņia.LicenseServer = Me.LicenseServer
                objCompaņia.TipoServidor = Me.TipoServidor

                blnConectar = objDAConexion.ConectarCompaņia(objCompaņia)

                If blnConectar Then
                    'Define el string de Conexion para ADO
                    objDAConexion.CrearStrConexionADO(objCompaņia)
                    strConexionSBO = objDAConexion.CadenaConexionSBO(objCompaņia)
                    'objDAConexion.AbrirConexion() 'Revisar esto

                    USUARIO_SISTEMA = objCompaņia.UsuarioSistema
                    g_strServidorLicencia = objCompaņia.LicenseServer

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
