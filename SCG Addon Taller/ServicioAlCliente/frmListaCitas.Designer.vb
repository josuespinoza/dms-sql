<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListaCitas
    Inherits System.Windows.Forms.Form


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmListaCitas))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.btnAnteriorDay = New System.Windows.Forms.Button()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.btnSiguienteWeek = New System.Windows.Forms.Button()
        Me.lblAgenda = New System.Windows.Forms.Label()
        Me.btnSiguienteDay = New System.Windows.Forms.Button()
        Me.btnAnteriorWeek = New System.Windows.Forms.Button()
        Me.cboAgenda = New System.Windows.Forms.ComboBox()
        Me.dtgvCitasReasignar = New System.Windows.Forms.DataGridView()
        Me.lblCitasReasignar = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblFechaAct = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnlMensaje = New System.Windows.Forms.Panel()
        Me.lblMensaje = New System.Windows.Forms.Label()
        Me.dgv_AgendaCitas = New System.Windows.Forms.DataGridView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.timerMensaje = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dtgvCitasReasignar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMensaje.SuspendLayout()
        CType(Me.dgv_AgendaCitas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCerrar
        '
        resources.ApplyResources(Me.btnCerrar, "btnCerrar")
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.ForeColor = System.Drawing.Color.Black
        Me.btnCerrar.Name = "btnCerrar"
        '
        'btnActualizar
        '
        resources.ApplyResources(Me.btnActualizar, "btnActualizar")
        Me.btnActualizar.ForeColor = System.Drawing.Color.Black
        Me.btnActualizar.Name = "btnActualizar"
        '
        'lblFecha
        '
        resources.ApplyResources(Me.lblFecha, "lblFecha")
        Me.lblFecha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblFecha.Name = "lblFecha"
        '
        'btnAnteriorDay
        '
        resources.ApplyResources(Me.btnAnteriorDay, "btnAnteriorDay")
        Me.btnAnteriorDay.ForeColor = System.Drawing.Color.Black
        Me.btnAnteriorDay.Name = "btnAnteriorDay"
        Me.btnAnteriorDay.Tag = "-1"
        '
        'dtpFecha
        '
        resources.ApplyResources(Me.dtpFecha, "dtpFecha")
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Value = New Date(2013, 5, 21, 0, 0, 0, 0)
        '
        'btnSiguienteWeek
        '
        resources.ApplyResources(Me.btnSiguienteWeek, "btnSiguienteWeek")
        Me.btnSiguienteWeek.ForeColor = System.Drawing.Color.Black
        Me.btnSiguienteWeek.Name = "btnSiguienteWeek"
        Me.btnSiguienteWeek.Tag = "7"
        '
        'lblAgenda
        '
        resources.ApplyResources(Me.lblAgenda, "lblAgenda")
        Me.lblAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblAgenda.Name = "lblAgenda"
        '
        'btnSiguienteDay
        '
        resources.ApplyResources(Me.btnSiguienteDay, "btnSiguienteDay")
        Me.btnSiguienteDay.ForeColor = System.Drawing.Color.Black
        Me.btnSiguienteDay.Name = "btnSiguienteDay"
        Me.btnSiguienteDay.Tag = "1"
        '
        'btnAnteriorWeek
        '
        resources.ApplyResources(Me.btnAnteriorWeek, "btnAnteriorWeek")
        Me.btnAnteriorWeek.ForeColor = System.Drawing.Color.Black
        Me.btnAnteriorWeek.Name = "btnAnteriorWeek"
        Me.btnAnteriorWeek.Tag = "-7"
        '
        'cboAgenda
        '
        Me.cboAgenda.FormattingEnabled = True
        resources.ApplyResources(Me.cboAgenda, "cboAgenda")
        Me.cboAgenda.Name = "cboAgenda"
        '
        'dtgvCitasReasignar
        '
        Me.dtgvCitasReasignar.AllowUserToAddRows = False
        Me.dtgvCitasReasignar.AllowUserToDeleteRows = False
        resources.ApplyResources(Me.dtgvCitasReasignar, "dtgvCitasReasignar")
        Me.dtgvCitasReasignar.BackgroundColor = System.Drawing.SystemColors.ScrollBar
        Me.dtgvCitasReasignar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dtgvCitasReasignar.Name = "dtgvCitasReasignar"
        Me.dtgvCitasReasignar.RowHeadersVisible = False
        Me.dtgvCitasReasignar.Tag = "Cita"
        '
        'lblCitasReasignar
        '
        resources.ApplyResources(Me.lblCitasReasignar, "lblCitasReasignar")
        Me.lblCitasReasignar.Name = "lblCitasReasignar"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.BackColor = System.Drawing.SystemColors.Control
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button1.ForeColor = System.Drawing.Color.Black
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'lblFechaAct
        '
        resources.ApplyResources(Me.lblFechaAct, "lblFechaAct")
        Me.lblFechaAct.Name = "lblFechaAct"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'pnlMensaje
        '
        resources.ApplyResources(Me.pnlMensaje, "pnlMensaje")
        Me.pnlMensaje.Controls.Add(Me.lblMensaje)
        Me.pnlMensaje.Name = "pnlMensaje"
        '
        'lblMensaje
        '
        resources.ApplyResources(Me.lblMensaje, "lblMensaje")
        Me.lblMensaje.Name = "lblMensaje"
        '
        'dgv_AgendaCitas
        '
        resources.ApplyResources(Me.dgv_AgendaCitas, "dgv_AgendaCitas")
        Me.dgv_AgendaCitas.BackgroundColor = System.Drawing.SystemColors.ScrollBar
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgv_AgendaCitas.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgv_AgendaCitas.EnableHeadersVisualStyles = False
        Me.dgv_AgendaCitas.Name = "dgv_AgendaCitas"
        Me.dgv_AgendaCitas.ReadOnly = True
        Me.dgv_AgendaCitas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgv_AgendaCitas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        '
        'timerMensaje
        '
        Me.timerMensaje.Interval = 3000
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 0
        Me.ToolTip1.AutoPopDelay = 30000
        Me.ToolTip1.InitialDelay = 0
        Me.ToolTip1.ReshowDelay = 0
        '
        'frmListaCitas
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.dgv_AgendaCitas)
        Me.Controls.Add(Me.pnlMensaje)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblFechaAct)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblCitasReasignar)
        Me.Controls.Add(Me.dtgvCitasReasignar)
        Me.Controls.Add(Me.cboAgenda)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.btnActualizar)
        Me.Controls.Add(Me.lblFecha)
        Me.Controls.Add(Me.btnAnteriorDay)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.btnSiguienteWeek)
        Me.Controls.Add(Me.lblAgenda)
        Me.Controls.Add(Me.btnSiguienteDay)
        Me.Controls.Add(Me.btnAnteriorWeek)
        Me.Name = "frmListaCitas"
        CType(Me.dtgvCitasReasignar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMensaje.ResumeLayout(False)
        Me.pnlMensaje.PerformLayout()
        CType(Me.dgv_AgendaCitas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents btnAnteriorDay As System.Windows.Forms.Button
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnSiguienteWeek As System.Windows.Forms.Button
    Friend WithEvents lblAgenda As System.Windows.Forms.Label
    Friend WithEvents btnSiguienteDay As System.Windows.Forms.Button
    Friend WithEvents btnAnteriorWeek As System.Windows.Forms.Button
    Friend WithEvents cboAgenda As System.Windows.Forms.ComboBox
    Friend WithEvents dtgvCitasReasignar As System.Windows.Forms.DataGridView
    Friend WithEvents lblCitasReasignar As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblFechaAct As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents pnlMensaje As System.Windows.Forms.Panel
    Friend WithEvents lblMensaje As System.Windows.Forms.Label
    Friend WithEvents timerMensaje As System.Windows.Forms.Timer
    Friend WithEvents dgv_AgendaCitas As System.Windows.Forms.DataGridView
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
