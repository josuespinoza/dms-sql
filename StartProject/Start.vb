Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGCommon
Imports SCG_User_Interface.SCG_User_Interface
Imports SCG_User_Interface.SCG_User_Interface.GlobalesUI
Imports Proyecto_SCGMSGBox

Module Start
    WithEvents objSCGSegurityMain As New SCG.Seguridad.SCGUI_SEG.frmMain(gc_strAplicacion)
    Private WithEvents m_tmrEjecutaMixit As New Timers.Timer
    WithEvents objfrmMensajeria As frmMensajeria1 'Mensajeria
    Private WithEvents m_tmrMensajeria As New Windows.Forms.Timer
    'ff
    Private Const mc_strpathFuenteMixit As String = "'pathFuenteMixit'"
    Private Const mc_strpathDestinoMixit As String = "'pathDestinoMixit'"
    Private Const mc_strIdCentroCostoPintura As String = "'CentrodeCostosPintura'"
    Private Const mc_strTiempoEnMinutos As String = "'TiempoEnMinutos'"
    Private Const mc_strPathConfig As String = "'PathConfig'"
    Private Const gc_strBackSlash As String = "/"
    Private Const mc_strTiempoMensajeria As String = "TiempoMensajeria" 'Mensajeria

    Public m_strpathFuenteMixit As String
    Public m_strpathDestinoMixit As String
    Public m_intIdCentroCostoPintura As Integer
    Public m_intTiempoEnMinutos As Integer

    Private m_blnConectar As Boolean
    Private m_intTiempoMensajeria As Integer 'Mensajeria
    Private m_blnHayConexion As Boolean 'Mensajeria

    Sub Main(ByVal args() As String)

        Try
            Dim objParametros As New SCG.Seguridad.Parametros

            objSCGSegurityMain.CargarArgumentos(args, objParametros)

            objSCGSegurityMain.ObjParametros = objParametros

            Application.Run(objSCGSegurityMain)

        Catch ex As Exception
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
        End Try

    End Sub

    Private Sub objSCGSecurityMain_CambioDeSkin() Handles objSCGSegurityMain.CambioDeSkin

        Dim btnRepuesta As DialogResult

        btnRepuesta = MessageBoxEx.Show(My.Resources.ResourceStart.PreguntaSkin, My.Resources.ResourceStart.SalidaSistema, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)

        If btnRepuesta = DialogResult.Yes Then
            Application.Restart()
        End If

    End Sub

    Private Sub objSCGSegurityMain_CambioDeIdioma(ByVal sender As Object, ByVal IdiomaNuevo As String) Handles objSCGSegurityMain.CambioDeIdioma

        Dim btnRepuesta As DialogResult
        btnRepuesta = MessageBoxEx.Show(My.Resources.ResourceStart.PreguntaSalir, My.Resources.ResourceStart.SalidaSistema, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If btnRepuesta = DialogResult.Yes Then
            Application.Restart()
        End If

    End Sub

    Private Sub objSCGSegurityMain_Salir() Handles objSCGSegurityMain.Salir
        Dim btnRepuesta As DialogResult
        btnRepuesta = MessageBoxEx.Show(My.Resources.ResourceStart.PreguntaSalir, My.Resources.ResourceStart.SalidaSistema, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1)
        If btnRepuesta = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub
    
    Private Sub objSCGSegurityMain_DevuelveDatosComp(ByVal p_objCompany As SCG.Seguridad.SCGBusinessLogic_SEG.BLConexion) Handles objSCGSegurityMain.DevuelveDatosComp

        'Dim strSplit As String

        Try
            Dim objCompany As New clsCompany
            Dim strParaObtenerValor As String = ""
            
            objBLConexion = New SCGBusinessLogic.BLConexion

            

            With p_objCompany

                'objUtil.CompanyL = .CompaniaSBO
                'objUtil.ServerL = .Servidor
                'objUtil.DBSBOL = .BaseDatosSBO
                'objUtil.UserDBL = .DBUser
                'objUtil.PassDBL = .DBPassword

                objBLConexion.BaseDatosSBO = .BaseDatosSBO
                objBLConexion.BaseDatosSCG = .BaseDatosSCG
                objBLConexion.Compañia = .CompaniaSBO
                objBLConexion.Usuario = .UsuarioDB
                objBLConexion.DBUser = .DBUser
                objBLConexion.DBPassword = .DBPassword
                objBLConexion.Contraseña = .ContrasenaUsuarioDB
                objBLConexion.Servidor = .Servidor
                objBLConexion.LicenseServer = .ServidorLicencias
                objBLConexion.UsuarioAplicacion = .UsuarioAplicacion
                objBLConexion.WinAuthentication = .WinAuthentication
                objBLConexion.TipoServidor = System.Configuration.ConfigurationManager.AppSettings.Item("TipoServidor")

                'objeto de utilitarios para definir los valores de la conexion 
                'para obtener los indicadores
                Dim objMC As New SCGBusinessLogic.MetodosCompartidosSBOCls(.CompaniaSBO, .Servidor, .BaseDatosSBO, .DBUser, .DBPassword)

                m_blnConectar = objBLConexion.ConectarCompañia

                m_blnHayConexion = m_blnConectar 'objBLConexion.ConectarCompañia 'Mensajeria

                If m_blnConectar Then
                    G_blnConexion = True
                    G_strUser = .UsuarioAplicacion
                    User = .UsuarioAplicacion
                    Password = .ContrasenaUsuarioDB
                    UserSCGInternal = .UsuarioDB
                    PasswordSCGInternal = .ContrasenaUsuarioDB
                    Server = .Servidor
                    ServerLicense = .ServidorLicencias
                    strDATABASE = .BaseDatosSBO
                    strDATABASESCG = .BaseDatosSCG
                    COMPANIA = .CompaniaSBO
                    PATH_REPORTES = .DireccionReportes
                    G_strCompaniaSCG = .BaseDatosSCG
                    G_strIDSucursal = .IDSucursal
                    G_strIDConfig = .IDConfig
                    G_strNombreSucursal = .NombreSucursal
                    G_strUsuarioAplicacion = .UsuarioAplicacion
                    g_TipoSkin = .TipoSkin

                    ''Sólo para servillantas
                    IniciarVariablesConfBodegas(p_objCompany)

                    g_adpConfiguracion = New ConfiguracionDataAdapter
                    g_dstConfiguracion = New ConfiguracionDataSet

                    Call g_adpConfiguracion.Fill(g_dstConfiguracion)

                    'Agregado 01/11/2010: Devuelve dato de configuracion de encargado de accesorios
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "EncargadoAccesorios", strParaObtenerValor)
                    g_strEncargadoAcc = strParaObtenerValor

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UsaSuministros", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnUsaSuministros = CBool(strParaObtenerValor)
                    Else
                        g_blnUsaSuministros = True
                    End If
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UsaServiciosExternos", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnUsaServiciosExternos = CBool(strParaObtenerValor)
                    Else
                        g_blnUsaServiciosExternos = True
                    End If
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UsaServicios", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnUsaServicios = CBool(strParaObtenerValor)
                    Else
                        g_blnUsaServicios = True
                    End If

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UsaOtrosGastos", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnUsaOtrosGastos = CBool(strParaObtenerValor)
                    Else
                        g_blnUsaOtrosGastos = True
                    End If


                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UsaRepuestos", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnUsaRepuestos = CBool(strParaObtenerValor)
                    Else
                        g_blnUsaRepuestos = True
                    End If
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "SEInventariables", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnServiciosExternosInventariables = CBool(strParaObtenerValor)
                    Else
                        g_blnServiciosExternosInventariables = False
                    End If
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "GeneraOTsEspeciales", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnGeneraOTsEspeciales = CBool(strParaObtenerValor)
                    Else
                        g_blnGeneraOTsEspeciales = False
                    End If
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "CosteoServicios", strParaObtenerValor)
                    If String.IsNullOrEmpty(strParaObtenerValor) Or strParaObtenerValor = "0" Then
                        g_blnCosteaActividades = False
                    Else
                        g_blnCosteaActividades = True
                    End If

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "ImpuestoRepuestos", strParaObtenerValor)
                    g_strImpRepuestos = strParaObtenerValor

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "ImpuestoServicios", strParaObtenerValor)
                    g_strImpServicios = strParaObtenerValor

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "ImpuestoSuministros", strParaObtenerValor)
                    g_strImpSuministros = strParaObtenerValor

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "ImpuestoServiciosExternos", strParaObtenerValor)
                    g_strImpServiciosExternos = strParaObtenerValor

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "DireccionB2B", strParaObtenerValor)
                    g_strDireccionB2B = strParaObtenerValor

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UnidadTiempo", strParaObtenerValor)
                    If strParaObtenerValor Is System.DBNull.Value Or Trim(strParaObtenerValor) = "" Then
                        strParaObtenerValor = -1
                        g_intUnidadTiempo = CInt(strParaObtenerValor)
                    Else
                        g_intUnidadTiempo = CInt(strParaObtenerValor)
                    End If

                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "CosteoServicios", strParaObtenerValor)
                    If Not IsNumeric(strParaObtenerValor) Then
                        strParaObtenerValor = "0"
                    End If
                    g_intCosteoServicios = CInt(strParaObtenerValor)


                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "CatalogosExternos", strParaObtenerValor)
                    If strParaObtenerValor = "1" Or strParaObtenerValor = "0" Then
                        g_blnCatalogosExternos = CBool(strParaObtenerValor)
                    Else
                        g_blnCatalogosExternos = False
                    End If

                    If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, mc_strTiempoMensajeria, strParaObtenerValor) Then
                        m_intTiempoMensajeria = CInt(strParaObtenerValor)

                    Else
                        m_intTiempoMensajeria = 15
                    End If

                    ''If g_blnMixitActivado Then

                    ''B ESte código no va ser implementado para el proyecto de servillantas
                    ''If CargaParametros() Then

                    ''    'Inicialización del Timer
                    g_blnMixitActivado = True
                    m_tmrEjecutaMixit.Enabled = True
                    m_intTiempoEnMinutos = 1
                    m_tmrEjecutaMixit.Interval = m_intTiempoEnMinutos * 60 * 1000

                    ''End If

                    'Se comento para evitar que salgan estos mensajes
                    ' ''Call Mensajeria.DevuelveParametrosdeConexionServidor(g_strServidordeCorreo, _
                    ' ''                                                     g_strDirEnviaCorreo, _
                    ' ''                                                     g_strUsuarioSMTP, _
                    ' ''                                                     g_strPasswordSMTP, _
                    ' ''                                                     g_strPuerto, _
                    ' ''                                                     g_chkUsaSSL)

                    ' ''If g_strServidordeCorreo <> "" _
                    ' ''    And g_strDirEnviaCorreo <> "" Then

                    ' ''    'If Mensajeria.CargaCitasparaenviarCorreos(g_strServidordeCorreo, g_strDirEnviaCorreo, _
                    ' ''    '                                                       g_strUsuarioSMTP, g_strPasswordSMTP) Then

                    ' ''    If Not Mensajeria.EnviaPublicidadMasivaProgramada(g_strServidordeCorreo, g_strDirEnviaCorreo, _
                    ' ''                                                  g_strUsuarioSMTP, g_strPasswordSMTP) Then

                    ' ''        Call MsgBox(My.Resources.ResourceStart.MensajeCorreosRecordatorioNiPuedenEnviar & vbCrLf & _
                    ' ''                                                                      " -" & My.Resources.ResourceStart.MensajeServidorNoConfigurado & vbCrLf & _
                    ' ''                                                                      " -" & My.Resources.ResourceStart.MensajeCuentaNoFonfigurado & vbCrLf & _
                    ' ''                                                                      " -" & My.Resources.ResourceStart.MensajeUsuarioOContraseñaNoValida, MsgBoxStyle.Information)
                    ' ''    End If

                    ' ''Else


                    ' ''    Call MsgBox(My.Resources.ResourceStart.MensajeCorreosRecordatorioNiPuedenEnviar & vbCrLf & _
                    ' ''                                                                  " -" & My.Resources.ResourceStart.MensajeServidorNoConfigurado & vbCrLf & _
                    ' ''                                                                  " -" & My.Resources.ResourceStart.MensajeCuentaNoFonfigurado & vbCrLf & _
                    ' ''                                                                  " -" & My.Resources.ResourceStart.MensajeUsuarioOContraseñaNoValida, MsgBoxStyle.Information)
                    ' ''End If
                    'Se comento para evitar que salgan estos mensajes




                    'Else
                    '    Call MsgBox("Los correos de recordatorio de citas y de envío de publicidad no pueden ser enviados por uno de los siguientes motivos:" & vbCrLf & _
                    '                                           " -El servidor de correo no ha sido configurado o es un servidor no válido." & vbCrLf & _
                    '                                           " -La cuenta de correo electrónico no se encuentra configurada o no es válida para el servidor configurado." & vbCrLf & _
                    '                                           " -El usuario o la contraseña del correo electrónico no es válida.", MsgBoxStyle.Information)
                    'End If


                    'Dim frmCitas As New frmCita(System.DateTime.Today.Day, _
                    '                            System.DateTime.Today.Month, _
                    '                            System.DateTime.Today.Year)

                    'frmCitas.MdiParent = objSCGSegurityMain

                    'frmCitas.StartPosition = FormStartPosition.Manual

                    'Call frmCitas.SetDesktopLocation(400, 20)

                    'Call frmCitas.Show()

                    ''frmCitas.ShowInTaskbar = False

                    ''End If

                    ''''''''''''''''
                    'Mensajeria
                    objfrmMensajeria = New frmMensajeria1(1, objSCGSegurityMain.Idioma)
                    'objfrmMensajeria.CheckForIllegalCrossThreadCalls = False
                    objfrmMensajeria.MdiParent = objSCGSegurityMain

                    'MensajeriaSBO_DMS()
                    'En la tabla, el tiempo para mensajeria se establece en segundos
                    m_tmrMensajeria.Interval = m_intTiempoMensajeria * 1000

                    m_tmrMensajeria.Enabled = True

                    ''''''''''''''

                Else
                    Application.Exit()
                End If
            End With

            '***********************
            '***********************
            ConfiguracionDeklarit()

            'para mostrar alguno campos
            Call MostrarCampos()

        Catch ex As Exception
            MessageBoxEx.Show(My.Resources.ResourceStart.MensajeNosePudoEstablecerConexion, My.Resources.ResourceStart.MensajeErrorenlaconexion, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            G_blnConexion = False
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Application.Exit()
        End Try
    End Sub
    ''' <summary>
    ''' Configura la conexion para la Dll de deklarit
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ConfiguracionDeklarit()

        Try


            Dim objDA As New SCGDataAccess.DAConexion
            Dim objConfiguracion As New DMSONEDKFramework.DefaultConfigurationProvider
            objConfiguracion.SetConnectionString(DAConexion.ConnectionString)
            DMSONEDKFramework.Configuration.ConfigurationProvider = objConfiguracion



        Catch ex As Exception
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")

        End Try


    End Sub

    Private Function CargaParametros() As Boolean

        Try

            Dim dstConfiguracion As New configuration
            Dim drwParametros As configuration.addRow
            Dim pathParametros As String

            pathParametros = Configuracion.DevuelveValordeParametro(mc_strPathConfig, COMPANIA, strDATABASESCG, objBLConexion)


            If System.IO.Directory.Exists(pathParametros) OrElse System.IO.File.Exists(pathParametros) Then

                'If Not pathParametros.EndsWith(gc_strBackSlash) Then

                '    pathParametros &= gc_strBackSlash
                'End If

                Call dstConfiguracion.ReadXml(pathParametros)

                drwParametros = dstConfiguracion.Tables(1).Select("Key=" & mc_strpathFuenteMixit).GetValue(0)
                m_strpathFuenteMixit = drwParametros.value

                drwParametros = dstConfiguracion.Tables(1).Select("Key=" & mc_strIdCentroCostoPintura).GetValue(0)
                m_intIdCentroCostoPintura = drwParametros.value


                drwParametros = dstConfiguracion.Tables(1).Select("Key=" & mc_strpathDestinoMixit).GetValue(0)
                m_strpathDestinoMixit = drwParametros.value

                drwParametros = dstConfiguracion.Tables(1).Select("Key=" & mc_strTiempoEnMinutos).GetValue(0)
                m_intTiempoEnMinutos = drwParametros.value

                Return True

            Else
                MessageBoxEx.Show(My.Resources.ResourceStart.MensajeProblemasConfiguracion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                Return False
            End If
        Catch ex As Exception
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Return False
        Finally
        End Try

    End Function

    Private Sub m_tmrEjecutaMixit_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles m_tmrEjecutaMixit.Elapsed

        ' If g_blnMixitActivado Then

        'Se comento para evitar que salgan estos mensajes
        ' ''If g_strServidordeCorreo <> "" _
        ' ''              And g_strDirEnviaCorreo <> "" Then

        ' ''    'If Mensajeria.CargaCitasparaenviarCorreos(g_strServidordeCorreo, g_strDirEnviaCorreo, _
        ' ''    '                                                       g_strUsuarioSMTP, g_strPasswordSMTP) Then

        ' ''    If Not Mensajeria.EnviaPublicidadMasivaProgramada(g_strServidordeCorreo, g_strDirEnviaCorreo, _
        ' ''                                                  g_strUsuarioSMTP, g_strPasswordSMTP) Then

        ' ''        Call MsgBox(My.Resources.ResourceStart.MensajeCorreosRecordatorioNiPuedenEnviar & vbCrLf & _
        ' ''                                                                      " -" & My.Resources.ResourceStart.MensajeServidorNoConfigurado & vbCrLf & _
        ' ''                                                                      " -" & My.Resources.ResourceStart.MensajeCuentaNoFonfigurado & vbCrLf & _
        ' ''                                                                      " -" & My.Resources.ResourceStart.MensajeUsuarioOContraseñaNoValida, MsgBoxStyle.Information)
        ' ''    End If
        ' ''    'Else
        ' ''    '    Call MsgBox("Los correos de recordatorio de citas y de envío de publicidad no pueden ser enviados por uno de los siguientes motivos:" & vbCrLf & _
        ' ''    '                                                                  " -El servidor de correo no ha sido configurado o es un servidor no válido." & vbCrLf & _
        ' ''    '                                                                  " -La cuenta de correo electrónico no se encuentra configurada o no es válida para el servidor configurado." & vbCrLf & _
        ' ''    '                                                                  " -El usuario o la contraseña del correo electrónico no es válida.", MsgBoxStyle.Information)
        ' ''    'End If
        ' ''Else
        ' ''    Call MsgBox(My.Resources.ResourceStart.MensajeCorreosRecordatorioNiPuedenEnviar & vbCrLf & _
        ' ''                                                                  " -" & My.Resources.ResourceStart.MensajeServidorNoConfigurado & vbCrLf & _
        ' ''                                                                  " -" & My.Resources.ResourceStart.MensajeCuentaNoFonfigurado & vbCrLf & _
        ' ''                                                                  " -" & My.Resources.ResourceStart.MensajeUsuarioOContraseñaNoValida, MsgBoxStyle.Information)
        ' ''End If
        'Se comento para evitar que salgan estos mensajes




        'Mixit no se usa en servillantas

        'If m_blnConectar Then

        '    Dim clsProcesaMixit As New ProcesaMixitcls(m_strpathFuenteMixit, _
        '                                               m_strpathDestinoMixit, _
        '                                               m_intIdCentroCostoPintura)

        '    Call clsProcesaMixit.ProcesaMixitenDataset()

        'End If

        'End If

    End Sub

    Private Sub IniciarVariablesConfBodegas(ByRef p_objCompany As SCG.Seguridad.SCGBusinessLogic_SEG.BLConexion)
        Dim SCGDA As New DMSOneFramework.SCGDataAccess.DAConexion
        Dim SCGDAUtils As DMSOneFramework.SCGDataAccess.Utilitarios
        Dim strConexLocal As String


        strConexLocal = DAConexion.ConnectionString

        SCGDAUtils = New DMSOneFramework.SCGDataAccess.Utilitarios(strConexLocal)

        ''MXXM

        'G_strIDBodegaRep = SCGDAUtils.GF_CargarIDBodegaRep()

        'G_strIDBodegaSum = SCGDAUtils.GF_CargarIDBodegaSum()

        'G_strIDBodegaSer = SCGDAUtils.GF_CargarIDBodegaSer()

        SCGDAUtils.CerrarConexionPendiente()

    End Sub

    Private Sub m_tmrMensajeria_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_tmrMensajeria.Tick

        Try

            If m_blnHayConexion Then
                m_blnHayConexion = False
                MensajeriaSBO_DMS()
            End If
        Catch ex As Exception
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
        End Try

    End Sub

    Private Sub MensajeriaSBO_DMS()
        Try
            Dim adpMensajeria As New MensajeriaSBOTallerDataAdapter

            'El frm se muestra solo si el usuario tiene mensajes no leidos
            If adpMensajeria.HayMensajesNuevos(G_strUser, G_strCompaniaSCG, gc_strAplicacion, G_strIDSucursal) Then


                If Not VerificarFrmMensajeriaAbierto() Then
                    If Not objfrmMensajeria.Visible Then

                        m_tmrMensajeria.Stop()
                        Call objfrmMensajeria.Show()
                        Call objfrmMensajeria.BringToFront()
                        m_tmrMensajeria.Start()

                    End If
                End If



            End If

            m_blnHayConexion = True


        Catch ex As Exception
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            Throw ex
        End Try
    End Sub

    Private Function VerificarFrmMensajeriaAbierto() As Boolean
        Dim Forma_Nueva As Form
        Dim blnExisteForm As Boolean = False

        If Not objSCGSegurityMain.MdiChildren Is Nothing Then
            For Each Forma_Nueva In objSCGSegurityMain.MdiChildren
                If Forma_Nueva.Name = "frmMensajeria1" And Forma_Nueva.Visible Then
                    blnExisteForm = True
                End If
            Next

        End If

        Return blnExisteForm

    End Function

End Module

