Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmVentasPorEmpleado
        Inherits frmRangoFechas

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVentasPorEmpleado))
            Me.rbtDetallado = New System.Windows.Forms.RadioButton()
            Me.rbtResumido = New System.Windows.Forms.RadioButton()
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
            Me.GroupBox1.Controls.Add(Me.rbtDetallado)
            Me.GroupBox1.Controls.Add(Me.rbtResumido)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Controls.SetChildIndex(Me.Label2, 0)
            Me.GroupBox1.Controls.SetChildIndex(Me.Label3, 0)
            Me.GroupBox1.Controls.SetChildIndex(Me.lblLine2, 0)
            Me.GroupBox1.Controls.SetChildIndex(Me.lblLine1, 0)
            Me.GroupBox1.Controls.SetChildIndex(Me.dtpDesde, 0)
            Me.GroupBox1.Controls.SetChildIndex(Me.rbtResumido, 0)
            Me.GroupBox1.Controls.SetChildIndex(Me.rbtDetallado, 0)
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
            'rbtDetallado
            '
            resources.ApplyResources(Me.rbtDetallado, "rbtDetallado")
            Me.rbtDetallado.Name = "rbtDetallado"
            Me.rbtDetallado.UseVisualStyleBackColor = True
            '
            'rbtResumido
            '
            resources.ApplyResources(Me.rbtResumido, "rbtResumido")
            Me.rbtResumido.Checked = True
            Me.rbtResumido.Name = "rbtResumido"
            Me.rbtResumido.TabStop = True
            Me.rbtResumido.UseVisualStyleBackColor = True
            '
            'frmVentasPorEmpleado
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Name = "frmVentasPorEmpleado"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private Sub frmRpt2CumplimientoEntrega_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Text = My.Resources.ResourceUI.BarraTituloVentasXempleado
            'Me.lblNoReporte.Text = "DMS-03"
            Me.cbDetallado.Visible = False
            Me.cbResumido.Visible = False

            Me.cbDetallado.Visible = False
            Me.cbResumido.Visible = False
            Me.txtEmpleado.Visible = False
            Me.txtIdEmpleado.Visible = False
            Me.picRepuesto.Visible = False
            Me.lblMecanico.Visible = False
            Me.Size = New Size(324, 194)

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
                Dim strResumido As String

                strFechaDesde = Utilitarios.RetornaFechaFormatoRegional(dtpDesde.Value.Date)
                strFechaHasta = Utilitarios.RetornaFechaFormatoRegional(dtpHasta.Value.Date)

                If rbtResumido.Checked Then
                    strResumido = "1"
                Else
                    strResumido = "0"
                End If
                strParametros = String.Format("{0},{1},{2}", strFechaHasta, strFechaDesde, strResumido)


                '********************************************************************************************************

                With rptTiempo

                    .P_BarraTitulo = My.Resources.ResourceUI.rptbarratituloVentasXEmpleado
                    .P_WorkFolder = PATH_REPORTES
                    .P_Filename = My.Resources.ResourceUI.rptNombreVentasXMecanico
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

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New(p_blnEstado)

            InitializeComponent()

        End Sub
        Friend WithEvents rbtDetallado As System.Windows.Forms.RadioButton
        Friend WithEvents rbtResumido As System.Windows.Forms.RadioButton

    End Class

End Namespace