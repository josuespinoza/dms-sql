Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmReporteResumenFacturacionXMecanico
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteResumenFacturacionXMecanico))
            Me.txtDiasTrabajados = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label5 = New System.Windows.Forms.Label
            Me.btncerrar = New System.Windows.Forms.Button
            Me.btnBuscar = New System.Windows.Forms.Button
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.dtpHasta = New System.Windows.Forms.DateTimePicker
            Me.dtpDesde = New System.Windows.Forms.DateTimePicker
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.rptReporte = New ComponenteCristalReport.SubReportView
            Me.Label1 = New System.Windows.Forms.Label
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtDiasTrabajados
            '
            Me.txtDiasTrabajados.AceptaNegativos = False
            Me.txtDiasTrabajados.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtDiasTrabajados.EstiloSBO = True
            resources.ApplyResources(Me.txtDiasTrabajados, "txtDiasTrabajados")
            Me.txtDiasTrabajados.MaxDecimales = 0
            Me.txtDiasTrabajados.MaxEnteros = 0
            Me.txtDiasTrabajados.Millares = False
            Me.txtDiasTrabajados.Name = "txtDiasTrabajados"
            Me.txtDiasTrabajados.Size_AdjustableHeight = 20
            Me.txtDiasTrabajados.TeclasDeshacer = True
            Me.txtDiasTrabajados.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Label5
            '
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'btncerrar
            '
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.Name = "btncerrar"
            '
            'btnBuscar
            '
            resources.ApplyResources(Me.btnBuscar, "btnBuscar")
            Me.btnBuscar.ForeColor = System.Drawing.Color.Black
            Me.btnBuscar.Name = "btnBuscar"
            '
            'GroupBox1
            '
            Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
            Me.GroupBox1.Controls.Add(Me.Panel2)
            Me.GroupBox1.Controls.Add(Me.Panel1)
            Me.GroupBox1.Controls.Add(Me.dtpHasta)
            Me.GroupBox1.Controls.Add(Me.dtpDesde)
            Me.GroupBox1.Controls.Add(Me.lblLine1)
            Me.GroupBox1.Controls.Add(Me.lblLine2)
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Controls.Add(Me.Label2)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'dtpHasta
            '
            Me.dtpHasta.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpHasta, "dtpHasta")
            Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpHasta.Name = "dtpHasta"
            Me.dtpHasta.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
            '
            'dtpDesde
            '
            Me.dtpDesde.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpDesde, "dtpDesde")
            Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpDesde.Name = "dtpDesde"
            Me.dtpDesde.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label3.Name = "Label3"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label2.Name = "Label2"
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
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'frmReporteResumenFacturacionXMecanico
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.txtDiasTrabajados)
            Me.Controls.Add(Me.rptReporte)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnBuscar)
            Me.MaximizeBox = False
            Me.Name = "frmReporteResumenFacturacionXMecanico"
            Me.GroupBox1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtDiasTrabajados As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents btncerrar As System.Windows.Forms.Button
        Friend WithEvents btnBuscar As System.Windows.Forms.Button
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents rptReporte As ComponenteCristalReport.SubReportView
        Public WithEvents Label1 As System.Windows.Forms.Label
    End Class
End Namespace
