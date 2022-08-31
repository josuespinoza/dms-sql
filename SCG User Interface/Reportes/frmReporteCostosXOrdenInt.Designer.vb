<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReporteCostosXOrdenInt
    Inherits System.Windows.Forms.Form

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteCostosXOrdenInt))
        Me.optResumido = New System.Windows.Forms.RadioButton()
        Me.optDetallado = New System.Windows.Forms.RadioButton()
        Me.rptReporte = New ComponenteCristalReport.SubReportView()
        Me.gbxRangoFechas = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblLine1 = New System.Windows.Forms.Label()
        Me.lblLine2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btncerrar = New System.Windows.Forms.Button()
        Me.btnCargar = New System.Windows.Forms.Button()
        Me.gbxRangoFechas.SuspendLayout()
        Me.SuspendLayout()
        '
        'optResumido
        '
        Me.optResumido.AutoSize = True
        Me.optResumido.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optResumido.Location = New System.Drawing.Point(180, 84)
        Me.optResumido.Name = "optResumido"
        Me.optResumido.Size = New System.Drawing.Size(72, 17)
        Me.optResumido.TabIndex = 9153
        Me.optResumido.Text = "Resumido"
        Me.optResumido.UseVisualStyleBackColor = True
        '
        'optDetallado
        '
        Me.optDetallado.AutoSize = True
        Me.optDetallado.Checked = True
        Me.optDetallado.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.optDetallado.Location = New System.Drawing.Point(36, 84)
        Me.optDetallado.Name = "optDetallado"
        Me.optDetallado.Size = New System.Drawing.Size(70, 17)
        Me.optDetallado.TabIndex = 9152
        Me.optDetallado.TabStop = True
        Me.optDetallado.Text = "Detallado"
        Me.optDetallado.UseVisualStyleBackColor = True
        '
        'rptReporte
        '
        Me.rptReporte.BackColor = System.Drawing.Color.White
        Me.rptReporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.rptReporte.Location = New System.Drawing.Point(252, 108)
        Me.rptReporte.Name = "rptReporte"
        Me.rptReporte.P_Authentication = False
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
        Me.rptReporte.Size = New System.Drawing.Size(24, 24)
        Me.rptReporte.TabIndex = 9151
        Me.rptReporte.Visible = False
        '
        'gbxRangoFechas
        '
        Me.gbxRangoFechas.Controls.Add(Me.Panel2)
        Me.gbxRangoFechas.Controls.Add(Me.dtpHasta)
        Me.gbxRangoFechas.Controls.Add(Me.Panel1)
        Me.gbxRangoFechas.Controls.Add(Me.dtpDesde)
        Me.gbxRangoFechas.Controls.Add(Me.lblLine1)
        Me.gbxRangoFechas.Controls.Add(Me.lblLine2)
        Me.gbxRangoFechas.Controls.Add(Me.Label3)
        Me.gbxRangoFechas.Controls.Add(Me.Label4)
        Me.gbxRangoFechas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.gbxRangoFechas.Location = New System.Drawing.Point(12, 12)
        Me.gbxRangoFechas.Name = "gbxRangoFechas"
        Me.gbxRangoFechas.Size = New System.Drawing.Size(264, 62)
        Me.gbxRangoFechas.TabIndex = 9150
        Me.gbxRangoFechas.TabStop = False
        Me.gbxRangoFechas.Text = "Fechas"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
        Me.Panel2.BackgroundImage = CType(resources.GetObject("Panel2.BackgroundImage"), System.Drawing.Image)
        Me.Panel2.Enabled = False
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Panel2.Location = New System.Drawing.Point(235, 37)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(18, 16)
        Me.Panel2.TabIndex = 504
        '
        'dtpHasta
        '
        Me.dtpHasta.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtpHasta.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.dtpHasta.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.dtpHasta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtpHasta.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtpHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(133, 35)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(122, 20)
        Me.dtpHasta.TabIndex = 500
        Me.dtpHasta.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
        Me.Panel1.BackgroundImage = CType(resources.GetObject("Panel1.BackgroundImage"), System.Drawing.Image)
        Me.Panel1.Enabled = False
        Me.Panel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Panel1.Location = New System.Drawing.Point(235, 16)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(18, 16)
        Me.Panel1.TabIndex = 503
        '
        'dtpDesde
        '
        Me.dtpDesde.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtpDesde.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.dtpDesde.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.dtpDesde.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtpDesde.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtpDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(133, 14)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(122, 20)
        Me.dtpDesde.TabIndex = 498
        Me.dtpDesde.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
        '
        'lblLine1
        '
        Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.lblLine1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblLine1.Location = New System.Drawing.Point(7, 33)
        Me.lblLine1.Name = "lblLine1"
        Me.lblLine1.Size = New System.Drawing.Size(125, 1)
        Me.lblLine1.TabIndex = 501
        '
        'lblLine2
        '
        Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.lblLine2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblLine2.Location = New System.Drawing.Point(7, 53)
        Me.lblLine2.Name = "lblLine2"
        Me.lblLine2.Size = New System.Drawing.Size(125, 1)
        Me.lblLine2.TabIndex = 502
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label3.Location = New System.Drawing.Point(4, 41)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 499
        Me.Label3.Text = "Hasta:"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(4, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 497
        Me.Label4.Text = "Desde:"
        '
        'btncerrar
        '
        Me.btncerrar.BackgroundImage = CType(resources.GetObject("btncerrar.BackgroundImage"), System.Drawing.Image)
        Me.btncerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btncerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.btncerrar.ForeColor = System.Drawing.Color.Black
        Me.btncerrar.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btncerrar.Location = New System.Drawing.Point(94, 116)
        Me.btncerrar.Name = "btncerrar"
        Me.btncerrar.Size = New System.Drawing.Size(72, 20)
        Me.btncerrar.TabIndex = 9149
        Me.btncerrar.Text = "Cerrar"
        '
        'btnCargar
        '
        Me.btnCargar.BackgroundImage = CType(resources.GetObject("btnCargar.BackgroundImage"), System.Drawing.Image)
        Me.btnCargar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.btnCargar.ForeColor = System.Drawing.Color.Black
        Me.btnCargar.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCargar.Location = New System.Drawing.Point(16, 116)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(72, 20)
        Me.btnCargar.TabIndex = 9148
        Me.btnCargar.Text = "Cargar"
        '
        'frmReporteCostosXOrdenInt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(300, 171)
        Me.Controls.Add(Me.optResumido)
        Me.Controls.Add(Me.optDetallado)
        Me.Controls.Add(Me.rptReporte)
        Me.Controls.Add(Me.gbxRangoFechas)
        Me.Controls.Add(Me.btncerrar)
        Me.Controls.Add(Me.btnCargar)
        Me.Name = "frmReporteCostosXOrdenInt"
        Me.Text = "frmReporteCostosXOrdenInt"
        Me.gbxRangoFechas.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents optResumido As System.Windows.Forms.RadioButton
    Friend WithEvents optDetallado As System.Windows.Forms.RadioButton
    Friend WithEvents rptReporte As ComponenteCristalReport.SubReportView
    Friend WithEvents gbxRangoFechas As System.Windows.Forms.GroupBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
    Public WithEvents lblLine1 As System.Windows.Forms.Label
    Public WithEvents lblLine2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btncerrar As System.Windows.Forms.Button
    Friend WithEvents btnCargar As System.Windows.Forms.Button
End Class
