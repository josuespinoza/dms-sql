Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmRpt2CumplimientoEntrega
        Inherits frmRangoFechas
        Friend WithEvents cbEstadoOT As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRpt2CumplimientoEntrega))
            Me.Label1 = New System.Windows.Forms.Label()
            Me.cbEstadoOT = New System.Windows.Forms.ComboBox()
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'dtpDesde
            '
            resources.ApplyResources(Me.dtpDesde, "dtpDesde")
            Me.dtpDesde.Value = New Date(2007, 12, 20, 0, 0, 0, 0)
            '
            'GroupBox1
            '
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            '
            'btnBuscar
            '
            Me.btnBuscar.BackgroundImage = Global.SCG_User_Interface.My.Resources.Resources.Boton_SCG
            resources.ApplyResources(Me.btnBuscar, "btnBuscar")
            '
            'btncerrar
            '
            Me.btncerrar.BackgroundImage = Global.SCG_User_Interface.My.Resources.Resources.Boton_SCG
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'cbEstadoOT
            '
            Me.cbEstadoOT.FormattingEnabled = True
            Me.cbEstadoOT.Items.AddRange(New Object() {resources.GetString("cbEstadoOT.Items"), resources.GetString("cbEstadoOT.Items1"), resources.GetString("cbEstadoOT.Items2"), resources.GetString("cbEstadoOT.Items3")})
            resources.ApplyResources(Me.cbEstadoOT, "cbEstadoOT")
            Me.cbEstadoOT.Name = "cbEstadoOT"
            '
            'frmRpt2CumplimientoEntrega
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.cbEstadoOT)
            Me.Controls.Add(Me.Label1)
            Me.Name = "frmRpt2CumplimientoEntrega"
            Me.Controls.SetChildIndex(Me.GroupBox1, 0)
            Me.Controls.SetChildIndex(Me.btnBuscar, 0)
            Me.Controls.SetChildIndex(Me.btncerrar, 0)
            Me.Controls.SetChildIndex(Me.Label1, 0)
            Me.Controls.SetChildIndex(Me.cbEstadoOT, 0)
            cbEstadoOT.SelectedIndex = 0
            Me.GroupBox1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub frmRpt2CumplimientoEntrega_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Text = My.Resources.ResourceUI.rptBarraTituloCumplimientoEntrega
            'Me.lblNoReporte.Text = "DMS-02"
        End Sub

        Public Overrides Sub CargaReporte()
            Dim strParametros As String = ""
            Dim rptTiempo As New ComponenteCristalReport.SubReportView

            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

            Try
                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                

                '********************************************************************************************************

                'strParametros = strParametros & dtpDesde.Value.ToString & ","

                'strParametros = strParametros & dtpHasta.Value.ToString

                'Manejo de la fecha obteniendo el formato de la maquina
                Dim strFechaDesde As String
                Dim strFechaHasta As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                strParametros = strParametros & strFechaDesde & "," & strFechaHasta & "," & obtieneIdEstado()

                '********************************************************************************************************


                With rptTiempo

                    .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloCumplimientoEntrega
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreCumplimientoEntrega
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

        Private Function obtieneIdEstado() As Integer
            Return Utilitarios.EjecutarConsulta(String.Format(" SELECT Code FROM [@SCGD_ESTADOS_OT] WHERE Name = '{0}'",
                                                                cbEstadoOT.SelectedItem.ToString.Trim), strConexionSBO)
        End Function


        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New(p_blnEstado)

            InitializeComponent()

        End Sub

        Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click

        End Sub
    End Class

End Namespace
