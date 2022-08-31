Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteCitasXFechaYAgenda

        Private m_drdAgendas As SqlClient.SqlDataReader
        Private m_adpAgendas As New AgendaDataAdapter
        Private m_objUtilitarios As New Utilitarios(strConexionADO)

#Region "Constructores"



        Public Sub New()
            MyBase.New()

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub
#End Region

        Private Sub CargarDatosCatalogos()
            Try
                Call m_adpAgendas.Fill(m_drdAgendas)
                Call Utilitarios.CargarComboSourceByReader(cboAgenda, m_drdAgendas)
                Call m_adpAgendas.Fill(m_drdAgendas)
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            Finally
                'Agregado 01072010
                If m_drdAgendas IsNot Nothing Then
                    If Not m_drdAgendas.IsClosed Then
                        Call m_drdAgendas.Close()
                    End If
                End If
            End Try


        End Sub

        Private Sub frmReporteCitasXFechaYAgenda_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Call CargarDatosCatalogos()
            dtpDesde.Value = m_objUtilitarios.CargarFechaHoraServidor()
            dtpHasta.Value = dtpDesde.Value
        End Sub

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

            Dim strParametros As String
            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                If chkAgenda.Checked Then
                    strParametros = cboAgenda.Text & ","
                Else
                    'strParametros = "Todas,"
                    strParametros = My.Resources.ResourceUI.ParametroAgendaTODAS

                End If

                '********************************************************************************************************

                'strParametros = strParametros & New Date(dtpDesde.Value.Year, dtpDesde.Value.Month, dtpDesde.Value.Day, 0, 0, 0).ToString & ","

                'strParametros = strParametros & New Date(dtpHasta.Value.Year, dtpHasta.Value.Month, dtpHasta.Value.Day, 23, 59, 59).ToString & ","

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & strFechaDesde & "," & strFechaHasta & ","

                '********************************************************************************************************

                If chkAgenda.Checked Then
                    If Not String.IsNullOrEmpty(cboAgenda.SelectedValue) Then
                        strParametros = strParametros & cboAgenda.SelectedValue
                    Else
                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarAgenda)
                        Exit Sub
                    End If
                Else
                    strParametros = strParametros & "-1"
                End If


                With rptTiempo

                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloCitasxFechayAgenda
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreCitasxFechayAgenda
                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros
                End With

                rptTiempo.VerReporte()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgInformationCustom(ex.Message)
            End Try

        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()
        End Sub

        Private Sub chkAgenda_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAgenda.CheckedChanged
            cboAgenda.Enabled = chkAgenda.Checked
        End Sub
    End Class

End Namespace