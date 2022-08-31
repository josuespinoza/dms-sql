Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmRepOrdenesPintura

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
            Try
                CargaReporte()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Try
                Me.Close()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgInformationCustom(ex.Message)
            End Try
        End Sub

        Public Overridable Sub CargaReporte()

            'Dim strFechadesde As String
            'Dim strFechaHasta As String

            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim strParametros As String = ""

            If optOrdenTrabajo.Checked = True Then
                If Trim(txtOrdendeTrabajo.Text) <> "" Then
                    strParametros = Trim(txtOrdendeTrabajo.Text) & "," & Today.Date & "," & Today.Date

                    If chkSoloFacturadas.Checked = True Then
                        strParametros = strParametros & ",1"
                    Else
                        strParametros = strParametros & ",2"
                    End If

                Else
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
                    Exit Sub
                End If
            Else


                strParametros = "-1,"
                strParametros = strParametros & dtpDesde.Value.ToString & "," 'CDate(Format(dtpDesde.Value, "yyyyMMdd") & " 00:00:00.000" & ",")
                strParametros = strParametros & DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString



                'strFechadesde = dtpDesde.Value.ToString 'Format(dtpDesde.Value, "yyyyMMdd").ToString   '& " 00:00:00.000"
                'strFechaHasta = dtpHasta.Value.ToString 'Format(dtpHasta.Value, "yyyyMMdd").ToString   '& " 23:59:59.999"

                ' & Format(dtpDesde.Value, "dd/MM/yyyy") & " 00:00:00.000" & "," & Format(dtpHasta.Value, "dd/MM/yyyy") & "  23:59:59.999"
                'strParametros = dtpDesde.Value.ToString & "," & dtpDesde.Value.ToString & "," & dtpHasta.Value.ToString


                If chkSoloFacturadas.Checked = True Then
                    strParametros = strParametros & ",1"
                Else
                    strParametros = strParametros & ",2"
                End If

            End If


            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                'strParametros = Trim(txtOrdendeTrabajo.Text)
                With rptTiempo
                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloOrdenesPintura
                    .P_WorkFolder = PATH_REPORTES

                    If chkSoloFacturadas.Checked = True Then
                        .P_Filename = My.Resources.ResourceUI.rptNombreOrdenesPinturaFacturadas
                    Else
                        .P_Filename = My.Resources.ResourceUI.rptNombreordenesPintura
                    End If


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

#Region "Constructor"

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub
#End Region

        Private Sub optOrdenTrabajo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optOrdenTrabajo.CheckedChanged
            If optOrdenTrabajo.Checked = True Then
                gbxRangoFechas.Enabled = False
                txtOrdendeTrabajo.Enabled = True
            End If
        End Sub

        Private Sub optRangoFechas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optRangoFechas.CheckedChanged
            If optRangoFechas.Checked = True Then
                txtOrdendeTrabajo.Enabled = False
                gbxRangoFechas.Enabled = True
            End If
        End Sub

        Private Sub frmRepOrdenesPintura_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            optOrdenTrabajo.Checked = True
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
            'MsgBox(System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToString)
        End Sub

        Private Sub chkSoloFacturadas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSoloFacturadas.CheckedChanged

        End Sub
    End Class
End Namespace
