Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmReporteHistorialResumido
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteHistorialResumido))
            Me.btncerrar = New System.Windows.Forms.Button
            Me.btnBuscar = New System.Windows.Forms.Button
            Me.rptReporte = New ComponenteCristalReport.SubReportView
            Me.txtMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtEstilo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            Me.Label4 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label5 = New System.Windows.Forms.Label
            Me.txtModelo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label6 = New System.Windows.Forms.Label
            Me.txtUnidad = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.rbtResumido = New System.Windows.Forms.RadioButton
            Me.rbtDetallado = New System.Windows.Forms.RadioButton
            Me.Label7 = New System.Windows.Forms.Label
            Me.Label8 = New System.Windows.Forms.Label
            Me.Label9 = New System.Windows.Forms.Label
            Me.Label10 = New System.Windows.Forms.Label
            Me.Label11 = New System.Windows.Forms.Label
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btncerrar
            '
            Me.btncerrar.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.Name = "btncerrar"
            Me.btncerrar.Tag = "0"
            Me.btncerrar.UseVisualStyleBackColor = False
            '
            'btnBuscar
            '
            Me.btnBuscar.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btnBuscar, "btnBuscar")
            Me.btnBuscar.ForeColor = System.Drawing.Color.Black
            Me.btnBuscar.Name = "btnBuscar"
            Me.btnBuscar.Tag = "0"
            Me.btnBuscar.UseVisualStyleBackColor = False
            '
            'rptReporte
            '
            Me.rptReporte.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.rptReporte, "rptReporte")
            Me.rptReporte.Name = "rptReporte"
            Me.rptReporte.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_NCopias = 0
            Me.rptReporte.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'txtMarca
            '
            Me.txtMarca.AceptaNegativos = False
            Me.txtMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtMarca, "txtMarca")
            Me.txtMarca.MaxDecimales = 0
            Me.txtMarca.MaxEnteros = 0
            Me.txtMarca.Millares = False
            Me.txtMarca.Name = "txtMarca"
            Me.txtMarca.ReadOnly = True
            Me.txtMarca.Size_AdjustableHeight = 20
            Me.txtMarca.TeclasDeshacer = True
            Me.txtMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtEstilo
            '
            Me.txtEstilo.AceptaNegativos = False
            Me.txtEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEstilo.EstiloSBO = True
            resources.ApplyResources(Me.txtEstilo, "txtEstilo")
            Me.txtEstilo.MaxDecimales = 0
            Me.txtEstilo.MaxEnteros = 0
            Me.txtEstilo.Millares = False
            Me.txtEstilo.Name = "txtEstilo"
            Me.txtEstilo.ReadOnly = True
            Me.txtEstilo.Size_AdjustableHeight = 20
            Me.txtEstilo.TeclasDeshacer = True
            Me.txtEstilo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picRepuesto
            '
            Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picRepuesto, "picRepuesto")
            Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picRepuesto.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'Label5
            '
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'txtModelo
            '
            Me.txtModelo.AceptaNegativos = False
            Me.txtModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtModelo.EstiloSBO = True
            resources.ApplyResources(Me.txtModelo, "txtModelo")
            Me.txtModelo.MaxDecimales = 0
            Me.txtModelo.MaxEnteros = 0
            Me.txtModelo.Millares = False
            Me.txtModelo.Name = "txtModelo"
            Me.txtModelo.ReadOnly = True
            Me.txtModelo.Size_AdjustableHeight = 20
            Me.txtModelo.TeclasDeshacer = True
            Me.txtModelo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.ReadOnly = True
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'txtUnidad
            '
            Me.txtUnidad.AceptaNegativos = False
            Me.txtUnidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtUnidad.EstiloSBO = True
            resources.ApplyResources(Me.txtUnidad, "txtUnidad")
            Me.txtUnidad.MaxDecimales = 0
            Me.txtUnidad.MaxEnteros = 0
            Me.txtUnidad.Millares = False
            Me.txtUnidad.Name = "txtUnidad"
            Me.txtUnidad.ReadOnly = True
            Me.txtUnidad.Size_AdjustableHeight = 20
            Me.txtUnidad.TeclasDeshacer = True
            Me.txtUnidad.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'rbtResumido
            '
            resources.ApplyResources(Me.rbtResumido, "rbtResumido")
            Me.rbtResumido.Checked = True
            Me.rbtResumido.Name = "rbtResumido"
            Me.rbtResumido.TabStop = True
            Me.rbtResumido.UseVisualStyleBackColor = True
            '
            'rbtDetallado
            '
            resources.ApplyResources(Me.rbtDetallado, "rbtDetallado")
            Me.rbtDetallado.Name = "rbtDetallado"
            Me.rbtDetallado.UseVisualStyleBackColor = True
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'Label11
            '
            Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.Name = "Label11"
            '
            'frmReporteHistorialResumido
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.Label11)
            Me.Controls.Add(Me.Label10)
            Me.Controls.Add(Me.Label9)
            Me.Controls.Add(Me.Label8)
            Me.Controls.Add(Me.Label7)
            Me.Controls.Add(Me.rbtDetallado)
            Me.Controls.Add(Me.rbtResumido)
            Me.Controls.Add(Me.txtUnidad)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.txtPlaca)
            Me.Controls.Add(Me.txtModelo)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.txtMarca)
            Me.Controls.Add(Me.txtEstilo)
            Me.Controls.Add(Me.picRepuesto)
            Me.Controls.Add(Me.rptReporte)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnBuscar)
            Me.MaximizeBox = False
            Me.Name = "frmReporteHistorialResumido"
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Protected WithEvents btncerrar As System.Windows.Forms.Button
        Protected WithEvents btnBuscar As System.Windows.Forms.Button
        Friend WithEvents rptReporte As ComponenteCristalReport.SubReportView
        Friend WithEvents txtMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtEstilo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtModelo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents txtUnidad As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents rbtResumido As System.Windows.Forms.RadioButton
        Friend WithEvents rbtDetallado As System.Windows.Forms.RadioButton
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents Label11 As System.Windows.Forms.Label
    End Class
End Namespace
