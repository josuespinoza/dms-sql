Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports System.Web.Mail
Imports System.Text.RegularExpressions
Imports DMSOneFramework.SCGCommon
Imports SAPbobsCOM
Imports System.Net


Public NotInheritable Class Mensajeria

    ' Public Shared Function CargaCitasparaenviarCorreos(ByVal strServidorDeCorreo As String, _
    'ByVal strDirEnvia As String, _
    '    ByVal strUsuarioSMTP As String, _
    '    ByVal strPasswordSMTP As String) As Boolean

    '    Dim dstCitas As CitasDataset
    '    Dim adpCitas As CitasDataAdapter

    '    Try

    '        dstCitas = New CitasDataset
    '        adpCitas = New CitasDataAdapter


    '        Dim drwCitas As CitasDataset.SCGTA_TB_CitasRow
    '        Dim datFechaEnvioCorreo As DateTime


    '        If adpCitas.Fill(dstCitas) > 0 Then

    '            For Each drwCitas In dstCitas.SCGTA_TB_Citas.Rows

    '                If Not drwCitas.IsRecordatorioNull AndAlso drwCitas.Recordatorio Then

    '                    datFechaEnvioCorreo = drwCitas.fecha.AddDays(-1 * drwCitas.CantidadDeDias)

    '                    If datFechaEnvioCorreo = System.DateTime.Now.Today Then

    '                        If drwCitas.IsDetalleNull Then
    '                            drwCitas.Detalle = ""
    '                        End If

    '                        If drwCitas.IsRazonNull Then
    '                            drwCitas.Razon = ""
    '                        End If

    '                        If EnviaCorreo(drwCitas.e_mail, drwCitas.Razon, drwCitas.Detalle, "", _
    '                                       strServidorDeCorreo, strDirEnvia, strUsuarioSMTP, strPasswordSMTP) Then

    '                            drwCitas.EstadoDeCorreo = True
    '                        Else
    '                            Return False

    '                        End If

    '                    End If

    '                End If

    '            Next drwCitas

    '            Call adpCitas.Update(dstCitas)

    '            Return True
    '        End If
    '        Return True
    '    Catch ex As Exception

    '        'MsgBox(ex.Message)
    '        Return False
    '    Finally
    '        Call dstCitas.Dispose()
    '        adpCitas = Nothing
    '    End Try

    'End Function


    Public Shared Function EnviaPublicidadMasivaProgramada(ByVal strServidorDeCorreo As String, _
                                                           ByVal strDirEnviaCorreo As String, _
                                                           ByVal strUsuarioSMTP As String, _
                                                           ByVal strPasswordSMTP As String) As Boolean

        Dim dstEnvioPublicidad As PublicidadEnvioDataset
        Dim adpEnvioPublicidad As PublicidadEnviosAdapter
        Dim drwEnvioPublicidad As PublicidadEnvioDataset.SCGTA_TB_EnvioPublicidadRow
        Dim drwDetallePublicidad As PublicidadEnvioDataset.SCGTA_TB_DetalleEnvioPublicidadRow
        Dim strListadeDestinatarios As String

        Try
            dstEnvioPublicidad = New PublicidadEnvioDataset
            adpEnvioPublicidad = New PublicidadEnviosAdapter

            Call adpEnvioPublicidad.Fill(dstEnvioPublicidad, -1, 0)

            For Each drwEnvioPublicidad In dstEnvioPublicidad.SCGTA_TB_EnvioPublicidad.Rows

                'If drwEnvioPublicidad.FechaEnvio = System.DateTime.Now.Today Then


                If CreaListadeDestinatarios(dstEnvioPublicidad.SCGTA_TB_DetalleEnvioPublicidad.Select("idEnvioPublicidad=" & drwEnvioPublicidad.IdEnvioPublicidad), _
                                            strListadeDestinatarios) Then

'                    If Mensajeria.EnviaCorreo(strListadeDestinatarios, _
'                                            drwEnvioPublicidad.Asunto, _
'                                            drwEnvioPublicidad.Detalle, _
'                                            drwEnvioPublicidad.Material, _
'                                            strServidorDeCorreo, _
'                                            strDirEnviaCorreo, _
'                                            strUsuarioSMTP, _
'                                            strPasswordSMTP) Then
'
'                        drwEnvioPublicidad.Enviado = 1
'                    Else
'                        Return False
'                    End If

                End If
                ' End If

            Next drwEnvioPublicidad

            Call adpEnvioPublicidad.Update(dstEnvioPublicidad)
            Return True
        Catch ex As Exception
            'MsgBox(ex.Message)
            Return False
        End Try
    End Function


    'Public Shared Function EnviaCorreo(ByVal DireccionDeCorreo As String, _
    '                                   ByVal Asunto As String, _
    '                                   ByVal Detalle As String, _
    '                                   ByVal Atachment As String, _
    '                                   ByRef strServidorDeCorreo As String, _
    '                                   ByRef strDirEnvia As String, _
    '                                   ByRef strUsuarioSMTP As String, _
    '                                   ByRef strPasswordSMTP As String) As Boolean
    '    Try

    '        Dim email As New System.Web.Mail.MailMessage



    '        Dim Fields As System.Collections.IDictionary
    '        Dim ConfigNamespace As String = strServidorDeCorreo & "/" '"http://schemas.microsoft.com/cdo/configuration/"

    '        Fields = email.Fields

    '        With Fields
    '            .Add(ConfigNamespace & "sendusername", strUsuarioSMTP) '
    '            .Add(ConfigNamespace & "sendpassword", strPasswordSMTP) '
    '            .Add(ConfigNamespace & "smtpauthenticate", 1)
    '        End With
    '        email.From = "SenderAddress"

    '        If strDirEnvia <> "" _
    '            And strServidorDeCorreo <> "" Then
    '            email.Subject = Asunto
    '            email.To = DireccionDeCorreo
    '            email.From = strDirEnvia
    '            email.Body = Detalle
    '            email.BodyFormat = Web.Mail.MailFormat.Text


    '            If Atachment <> "" _
    '                AndAlso System.IO.File.Exists(Atachment) Then

    '                Dim myAttachment As MailAttachment = New MailAttachment(Atachment)
    '                email.Attachments.Add(myAttachment)

    '            End If

    '            System.Web.Mail.SmtpMail.SmtpServer = strServidorDeCorreo


    '            System.Web.Mail.SmtpMail.Send(email)

    '            Return True
    '        Else
    '            Throw New Exception("1")
    '            Return False
    '        End If

    '    Catch ex As Exception

    '        MsgBox(ex.Message)
    '        Return False

    '    End Try
    'End Function

   

    Public Shared Function EnviaCorreo(ByVal DireccionDeCorreo As String, _
    ByVal Asunto As String, _
    ByVal Detalle As String, _
    ByVal Atachment As String, _
    ByRef strServidorDeCorreo As String, _
    ByRef strDirEnvia As String, _
    ByRef strUsuarioSMTP As String, _
    ByRef strPasswordSMTP As String, _
    ByRef strMsjError As String, ByVal dtbClientes As DataTable, _
    ByRef strPuerto As String, _
    ByRef boolUsaSSL As Boolean) As Boolean

        Try
            Dim email As New System.Net.Mail.MailMessage

            Dim drwDestinatarios As DataRow
            ''obtiene la autenticacion de seguridad del app.config
            Dim useSsl As Boolean = boolUsaSSL
            ''obtiene el puerto a conectarse
            Dim smtpPort As String = strPuerto

            '  Dim Fields As System.Collections.IDictionary
            Dim ConfigNamespace As String = strServidorDeCorreo & "/" '"http://schemas.microsoft.com/cdo/configuration/"

            ' Fields = email.Fields

            'With Fields
            '    .Add(ConfigNamespace & "sendusername", strUsuarioSMTP) '
            '    .Add(ConfigNamespace & "sendpassword", strPasswordSMTP) '
            '    .Add(ConfigNamespace & "smtpauthenticate", 1)
            'End With
            'email.From = "SenderAddress"
            'email.
            If strDirEnvia <> "" _
            And strServidorDeCorreo <> "" Then
                '    email.Subject = Asunto
                '    email.To = DireccionDeCorreo
                '    email.From = strDirEnvia
                '    email.Body = Detalle
                '    email.BodyFormat = Web.Mail.MailFormat.Text
                email.Subject = Asunto

                'Agregar destinatarios del correo
                For Each drwDestinatarios In dtbClientes.Rows


                    If drwDestinatarios.RowState <> DataRowState.Deleted Then

                        If Not drwDestinatarios("e_mail") Is System.Convert.DBNull Then

                            If EmailValido(drwDestinatarios("e_mail")) Then

                                'ListaDeDestinatarios &= drwDestinatarios("e_mail") & "; "
                                email.To.Add(drwDestinatarios("e_mail"))

                            End If


                        End If

                    End If

                Next drwDestinatarios


                ' email.To.Add(DireccionDeCorreo)

                email.From = New System.Net.Mail.MailAddress(strDirEnvia)
                email.Body = Detalle
                email.BodyEncoding = System.Text.Encoding.Default


                If Atachment <> "" _
                AndAlso System.IO.File.Exists(Atachment) Then

                    Dim myAttachment As System.Net.Mail.Attachment = New System.Net.Mail.Attachment(Atachment)
                    email.Attachments.Add(myAttachment)

                End If

                Dim smtpclient As New System.Net.Mail.SmtpClient(strServidorDeCorreo, smtpPort)

                ''pregunta si necesita o no SSL
                smtpclient.EnableSsl = useSsl

                smtpclient.Credentials = New System.Net.NetworkCredential(strUsuarioSMTP, strPasswordSMTP)

                smtpclient.Send(email)

                'System.Web.Mail.SmtpMail.SmtpServer = strServidorDeCorreo


                'System.Web.Mail.SmtpMail.Send(email)

                Return True
            Else
                'Throw New Exception("1")
                Return False
            End If

        Catch ex As Exception

            'MsgBox(ex.Message)
            MsgBox(ex.Message)
            MsgBox(ex.ToString)

            strMsjError = ex.Message

            Return False

        End Try

    End Function




    Public Shared Function CreaListadeDestinatarios(ByVal dtbClientes As DataTable, _
                                                    ByRef ListaDeDestinatarios As String) As Boolean

        Dim drwDestinatarios As DataRow

        Try

            For Each drwDestinatarios In dtbClientes.Rows


                If drwDestinatarios.RowState <> DataRowState.Deleted Then

                    If Not drwDestinatarios("e_mail") Is System.Convert.DBNull Then


                        ListaDeDestinatarios &= drwDestinatarios("e_mail") & ";"

                    End If

                End If

            Next drwDestinatarios

            If ListaDeDestinatarios.EndsWith(";") Then

                ListaDeDestinatarios = ListaDeDestinatarios.TrimEnd(";")

            End If



            Return True
        Catch ex As Exception

            MsgBox(ex.Message)
            Return False

        End Try

    End Function

    Public Shared Function CreaListadeDestinatarios(ByVal dtbClientes As System.Data.DataRow(), _
                                                    ByRef ListaDeDestinatarios As String) As Boolean

        Dim drwDestinatarios As DataRow
        Dim intIndice As Integer

        Try

            For intIndice = 0 To dtbClientes.Length - 1

                If CType(dtbClientes.GetValue(intIndice), System.Data.DataRow).RowState <> DataRowState.Deleted Then

                    If Not CType(dtbClientes.GetValue(intIndice), System.Data.DataRow)("e_mail") Is System.Convert.DBNull Then

                        ListaDeDestinatarios &= CType(dtbClientes.GetValue(intIndice), System.Data.DataRow)("e_mail") & ";"

                    End If

                End If

            Next intIndice

            Call ListaDeDestinatarios.TrimEnd(";")

            Return True
        Catch ex As Exception

            MsgBox(ex.Message)
            Return False

        End Try

    End Function

    Public Shared Function EmailValido(ByVal dirCorreo As String) As Boolean

        Try
            Dim EmailRegex As Regex = New Regex("(?<user>[^@]+)@(?<host>.+)")
            Dim M As Match = EmailRegex.Match(dirCorreo)

            If M.Success Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Function

    Public Shared Function DevuelveParametrosdeConexionServidor(ByRef strServidorDeCorreo As String, _
                                                                ByRef strDirEnvia As String, _
                                                                ByRef strUsuarioSMTP As String, _
                                                                ByRef strPasswordSMTP As String, _
                                                                ByRef strPuerto As String, _
                                                                ByRef chkUsaSSL As Boolean) As Boolean
        Try
            Dim dstConfigServidordeCorreo As New ConfigServidorCorreoDataset
            Dim adpConfigServidordeCorreo As New PublicidadEnviosAdapter
            Dim drwConfigServidordeCorreo As ConfigServidorCorreoDataset.SCGTA_TB_ConfiguracionDeCorreoRow

            Call adpConfigServidordeCorreo.Fill(dstConfigServidordeCorreo)

            If dstConfigServidordeCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows.Count = 1 Then

                drwConfigServidordeCorreo = dstConfigServidordeCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows(0)

                strServidorDeCorreo = drwConfigServidordeCorreo.ServidorDeCorreo
                strDirEnvia = drwConfigServidordeCorreo.DireccionCorreoEnvia
                strUsuarioSMTP = drwConfigServidordeCorreo.UsuarioSMTP
                strPasswordSMTP = drwConfigServidordeCorreo.PasswordSMTP


                strPuerto = drwConfigServidordeCorreo.Puerto


                'En el caso de que el valor drwConfigServidordeCorreo.UsaSSL es nulo, se captura el tipo de excepción
                'para que no se caiga el sistema
                Try
                    If IsDBNull(drwConfigServidordeCorreo.UsaSSL) Then
                        chkUsaSSL = True
                    Else
                        chkUsaSSL = drwConfigServidordeCorreo.UsaSSL
                    End If

                Catch ex As StrongTypingException
                    chkUsaSSL = True
                End Try

            End If

            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function DevuelveParametrosdeConexionServidor(ByRef strServidorDeCorreo As String, _
                                                               ByRef strDirEnvia As String, _
                                                               ByRef strUsuarioSMTP As String, _
                                                               ByRef strPasswordSMTP As String, _
                                                               ByRef strPuerto As String, _
                                                               ByRef strUsaSSL As Char) As Boolean
        Try
            Dim dstConfigServidordeCorreo As New ConfigServidorCorreoDataset
            Dim adpConfigServidordeCorreo As New PublicidadEnviosAdapter
            Dim drwConfigServidordeCorreo As ConfigServidorCorreoDataset.SCGTA_TB_ConfiguracionDeCorreoRow

            Call adpConfigServidordeCorreo.Fill(dstConfigServidordeCorreo)

            If dstConfigServidordeCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows.Count = 1 Then

                drwConfigServidordeCorreo = dstConfigServidordeCorreo.SCGTA_TB_ConfiguracionDeCorreo.Rows(0)

                strServidorDeCorreo = drwConfigServidordeCorreo.ServidorDeCorreo
                strDirEnvia = drwConfigServidordeCorreo.DireccionCorreoEnvia
                strUsuarioSMTP = drwConfigServidordeCorreo.UsuarioSMTP
                strPasswordSMTP = drwConfigServidordeCorreo.PasswordSMTP
                'strPuerto = drwConfigServidordeCorreo.Puerto
                'strUsaSSL = drwConfigServidordeCorreo.UsaSSL


            End If

            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function




End Class
