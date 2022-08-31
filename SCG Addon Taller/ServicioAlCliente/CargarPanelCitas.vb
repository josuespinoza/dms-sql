Imports System.Runtime.InteropServices
Imports System.Threading

Partial Public Class CargarPanelCitas

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetForegroundWindow() As IntPtr

    End Function

    Private oThread As Thread


    Public Sub ButtonCargarItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim blnAbrirFormulario As Boolean = False

        Try
            If pVal.ActionSuccess AndAlso pVal.BeforeAction = False Then

                Dim intTipoAgenda As Integer
                Dim strCodSucursal As String
                Dim sucursalCargar As String
                Dim m_strTipoAgenda As String

                strCodSucursal = EditComboSucursal.ObtieneValorUserDataSource


                If String.IsNullOrEmpty(strCodSucursal) Then
                    sucursalCargar = Utilitarios.EjecutarConsulta("Select Top(1) Code from [@SCGD_SUCURSALES]", CompanySBO.CompanyDB, CompanySBO.Server)
                Else
                    sucursalCargar = strCodSucursal
                End If

                If m_strUsaGruposTrabajo.Equals("Y") Then
                    intTipoAgenda = TipoDeAgenda.Equipos
                Else
                    intTipoAgenda = TipoDeAgenda.Agenda
                End If

                Dim ptr As IntPtr = GetForegroundWindow()
                Dim wrapper As New WindowWrapper(ptr)

                If _frmPanelCitaDotNet Is Nothing Then
                    blnAbrirFormulario = True
                Else
                    If _frmPanelCitaDotNet.Visible = False Then
                        blnAbrirFormulario = True
                    End If
                End If

                If _frmPanelCitaDotNet IsNot Nothing AndAlso _frmPanelCitaDotNet.WindowState = FormWindowState.Minimized Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MsjAgendaAbiertaMinimizada, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                End If

                If blnAbrirFormulario = True Then
                    g_strSucursal = sucursalCargar
                    g_intTipoAgenda = intTipoAgenda
                    oThread = New Thread(AddressOf InvocarAgenda)
                    oThread.IsBackground = True
                    oThread.Start()
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private g_strSucursal As String
    Private g_intTipoAgenda As Integer
    Private intContador As Integer

    Private Sub InvocarAgenda()
        Dim oAgenda As frmListaCitas
        Dim ptr As IntPtr = GetForegroundWindow()
        Dim wrapper As New WindowWrapper(ptr)
        Try
            _frmPanelCitaDotNet = New frmListaCitas(Date.Today, g_strSucursal, String.Empty, String.Empty, True, g_intTipoAgenda, m_blnVersion9, 1, 1, Nothing, CompanySBO, ApplicationSBO)
            _frmPanelCitaDotNet.ShowInTaskbar = True
            '_frmPanelCitaDotNet.TopLevel = True
            '_frmPanelCitaDotNet.TopMost = True
            _frmPanelCitaDotNet.Show(wrapper)
            Application.Run(_frmPanelCitaDotNet)
            _frmPanelCitaDotNet = Nothing
            'TerminarSubProceso()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub TerminarSubProceso()
        Try
            oThread.Abort()
        Catch ex As Exception

        End Try
    End Sub

    'Dim oDelegado As InvocarFormulario = AddressOf AbrirAgenda
    'oDelegado.Invoke(wrapper)

    'Private Delegate Sub InvocarFormulario(ByRef wrapper As WindowWrapper)

    'Private Sub AbrirAgenda(ByRef wrapper As WindowWrapper)
    '    Try
    '        _frmPanelCitaDotNet.Show()
    '        Application.Run()
    '    Catch ex As Exception
    '        DMS_Connector.Helpers.ManejoErrores(ex)
    '    End Try
    'End Sub

    Private Sub _frmAgenda_eFechaYHoraSeleccionada(ByVal p_strSerie As String,
                                                   ByVal p_strNumCita As String,
                                                   ByVal p_intCodigoAgenda As String) Handles _frmPanelCitaDotNet.eCargaCitaExiste

        Dim l_strSQL As String
        Dim l_strDocEntry As String
        Dim l_blnCerrarPanel As Boolean = False

        m_oGestorFormularios = New GestorFormularios(ApplicationSBO)

        If Not String.IsNullOrEmpty(p_strSerie) And Not String.IsNullOrEmpty(p_strNumCita) Then

            l_strSQL = "Select DocEntry from  [dbo].[@SCGD_CITA] where U_Num_Serie = '{0}' AND U_NumCita = '{1}'"
            l_strSQL = String.Format(l_strSQL, p_strSerie, p_strNumCita)
            l_strDocEntry = Utilitarios.EjecutarConsulta(l_strSQL, _companySbo.CompanyDB, _companySbo.Server)


            If Not String.IsNullOrEmpty(l_strDocEntry) Then

                otmpForm = ApplicationSBO.Forms.ActiveForm

                If Not (m_oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True)) Then
                    m_oGestorFormularios.CargaFormulario(m_oFormularioCitas)
                    m_oFormularioCitas.CargarCitaDesdePanel_Existe(l_strDocEntry)
                    l_blnCerrarPanel = True
                End If

                otmpForm = Nothing

            End If

        End If

        'If l_blnCerrarPanel Then
        '    _frmPanelCitaDotNet.Close()
        '    _frmPanelCitaDotNet = Nothing
        'End If

    End Sub

    Private Sub _frmAgenda_NuevaCita(ByVal p_fhaNuevaCita As Date,
                                        ByVal p_CodAsesor As String,
                                        ByVal p_strTecnico As String,
                                        ByVal p_strSucursal As String,
                                        ByVal p_strAgenda As String) Handles _frmPanelCitaDotNet.eCargaCitaNueva_PorAgenda

        Dim l_blnCerrarPanel As Boolean = False

        m_oGestorFormularios = New GestorFormularios(ApplicationSBO)

        otmpForm = ApplicationSBO.Forms.ActiveForm

        If Not (m_oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True)) Then
            m_oGestorFormularios.CargaFormulario(m_oFormularioCitas)
            m_oFormularioCitas.CargarCitaDesdePanel_Nueva(p_strSucursal, p_strAgenda, p_fhaNuevaCita)
            l_blnCerrarPanel = True
        End If

        otmpForm = Nothing

        If l_blnCerrarPanel Then
            _frmPanelCitaDotNet.Close()
            _frmPanelCitaDotNet = Nothing
        End If

    End Sub

    Private Sub _frmOcupacionAgenda_AsesorTecnicoSinCita(ByVal p_fhaAsesor As Date,
                                                        ByVal p_fhaTecnico As Date,
                                                        ByVal p_strCodAsesor As String,
                                                        ByVal p_strCodTecnico As String,
                                                        ByVal p_strCodSucur As String,
                                                        ByVal p_strCodAgenda As String
                                                     ) Handles _frmPanelCitaDotNet.eCargaCitaNueva_PorEquipos

        Dim l_blnCerrarPanel As Boolean = False

        m_oGestorFormularios = New GestorFormularios(ApplicationSBO)
        otmpForm = ApplicationSBO.Forms.ActiveForm


        If Not (m_oGestorFormularios.FormularioAbierto(m_oFormularioCitas, activarSiEstaAbierto:=True)) Then
            m_oGestorFormularios.CargaFormulario(m_oFormularioCitas)

            l_blnCerrarPanel = True
            If l_blnCerrarPanel Then
                _frmPanelCitaDotNet.Close()
                _frmPanelCitaDotNet = Nothing
            End If

            m_oFormularioCitas.CargarDesdePanelAsesorTecnico(p_fhaAsesor, p_fhaTecnico, p_strCodAsesor, p_strCodTecnico, p_strCodSucur, p_strCodAgenda)

        End If
        otmpForm = Nothing

    End Sub

End Class
