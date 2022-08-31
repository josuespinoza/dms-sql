Namespace SCGCommon
    Public Class clsCompany
        'Variables para las Propiedades
        Private strServidor As String
        Private strBaseDatos As String
        Private strCompañia As String
        Private strUsuario As String
        Private strContraseña As String
        Private strUsuarioSistema As String
        Private strBaseDatosSCG As String
        Private strLicenseServer As String
        Private strDBUser As String
        Private strDBPassword As String
        Private BlnWinAuthentication As Boolean
        Private serverType As String


        Public Property TipoServidor() As String
            Get
                Return serverType
            End Get
            Set(ByVal value As String)
                serverType = value
            End Set
        End Property

        Public Property Server() As String
            Get
                Return strServidor
            End Get
            Set(ByVal Value As String)
                If (Value.Trim <> "") Then

                    strServidor = Value
                End If
            End Set
        End Property

        Public Property BaseDatosSCG() As String
            Get
                Return strBaseDatosSCG
            End Get
            Set(ByVal Value As String)
                If (Value.Trim <> "") Then

                    strBaseDatosSCG = Value
                End If
            End Set
        End Property

        Public Property DataBase() As String
            Get
                Return strBaseDatos
            End Get
            Set(ByVal Value As String)
                strBaseDatos = Value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return strUsuario
            End Get
            Set(ByVal Value As String)
                strUsuario = Value
            End Set
        End Property

        Public Property UsuarioSistema() As String
            Get
                Return strUsuarioSistema
            End Get
            Set(ByVal Value As String)
                strUsuarioSistema = Value
            End Set
        End Property

        Public Property Password() As String
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

        Public Property Company() As String
            Get
                Return strCompañia
            End Get
            Set(ByVal Value As String)
                strCompañia = Value
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

        Public Property LicenseServer() As String
            Get
                Return strLicenseServer
            End Get
            Set(ByVal Value As String)
                strLicenseServer = Value
            End Set
        End Property

        Public Function ReturnCompany() As clsCompany
            Dim TempCompany As New clsCompany

            TempCompany.Server = Me.Server
            TempCompany.DataBase = Me.DataBase
            TempCompany.UserName = Me.UserName
            TempCompany.Company = Me.Company
            TempCompany.DBUser = Me.DBUser
            TempCompany.DBPassword = Me.DBPassword
            TempCompany.WinAuthentication = Me.WinAuthentication
            TempCompany.TipoServidor = Me.TipoServidor

            Return TempCompany
        End Function
    End Class
End Namespace