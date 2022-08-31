<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalendarioColor
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalendarioColor))
        Me.lblNombreAgenda = New System.Windows.Forms.Label()
        Me.lblAgenda = New System.Windows.Forms.Label()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.dtgOcupacion = New System.Windows.Forms.DataGridView()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.btnAnteriorDay = New System.Windows.Forms.Button()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.btnSiguienteWeek = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSiguienteDay = New System.Windows.Forms.Button()
        Me.btnAnteriorWeek = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TTCita = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblNombreAgenda
        '
        Me.lblNombreAgenda.AutoSize = True
        Me.lblNombreAgenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lblNombreAgenda.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblNombreAgenda.Location = New System.Drawing.Point(75, 9)
        Me.lblNombreAgenda.Name = "lblNombreAgenda"
        Me.lblNombreAgenda.Size = New System.Drawing.Size(51, 15)
        Me.lblNombreAgenda.TabIndex = 9131
        Me.lblNombreAgenda.Text = "Label1"
        '
        'lblAgenda
        '
        Me.lblAgenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.lblAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblAgenda.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblAgenda.Location = New System.Drawing.Point(-206, 9)
        Me.lblAgenda.Name = "lblAgenda"
        Me.lblAgenda.Size = New System.Drawing.Size(58, 13)
        Me.lblAgenda.TabIndex = 9130
        Me.lblAgenda.Text = "Agenda"
        '
        'btnAceptar
        '
        Me.btnAceptar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAceptar.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAceptar.Location = New System.Drawing.Point(562, 680)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(75, 19)
        Me.btnAceptar.TabIndex = 9134
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        Me.btnAceptar.Visible = False
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.btnCerrar.ForeColor = System.Drawing.Color.Black
        Me.btnCerrar.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCerrar.Location = New System.Drawing.Point(75, 680)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(70, 20)
        Me.btnCerrar.TabIndex = 9136
        Me.btnCerrar.Text = "Cerrar"
        '
        'btnActualizar
        '
        Me.btnActualizar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.btnActualizar.ForeColor = System.Drawing.Color.Black
        Me.btnActualizar.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnActualizar.Location = New System.Drawing.Point(-1, 680)
        Me.btnActualizar.Name = "btnActualizar"
        Me.btnActualizar.Size = New System.Drawing.Size(70, 20)
        Me.btnActualizar.TabIndex = 9135
        Me.btnActualizar.Text = "Actualizar"
        '
        'dtgOcupacion
        '
        Me.dtgOcupacion.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtgOcupacion.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dtgOcupacion.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dtgOcupacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dtgOcupacion.DefaultCellStyle = DataGridViewCellStyle5
        Me.dtgOcupacion.Location = New System.Drawing.Point(7, 41)
        Me.dtgOcupacion.Name = "dtgOcupacion"
        Me.dtgOcupacion.ReadOnly = True
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dtgOcupacion.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dtgOcupacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dtgOcupacion.Size = New System.Drawing.Size(686, 633)
        Me.dtgOcupacion.TabIndex = 9138
        '
        'lblFecha
        '
        Me.lblFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.lblFecha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.lblFecha.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblFecha.Location = New System.Drawing.Point(389, 10)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(43, 13)
        Me.lblFecha.TabIndex = 9144
        Me.lblFecha.Text = "Fecha"
        '
        'btnAnteriorDay
        '
        Me.btnAnteriorDay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAnteriorDay.BackgroundImage = CType(resources.GetObject("btnAnteriorDay.BackgroundImage"), System.Drawing.Image)
        Me.btnAnteriorDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAnteriorDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.btnAnteriorDay.ForeColor = System.Drawing.Color.Black
        Me.btnAnteriorDay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAnteriorDay.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAnteriorDay.Location = New System.Drawing.Point(602, 3)
        Me.btnAnteriorDay.Name = "btnAnteriorDay"
        Me.btnAnteriorDay.Size = New System.Drawing.Size(30, 21)
        Me.btnAnteriorDay.TabIndex = 9141
        Me.btnAnteriorDay.Tag = "-1"
        Me.btnAnteriorDay.Text = "<"
        '
        'dtpFecha
        '
        Me.dtpFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(438, 5)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(87, 20)
        Me.dtpFecha.TabIndex = 9139
        Me.dtpFecha.Value = New Date(2007, 3, 8, 0, 0, 0, 0)
        '
        'btnSiguienteWeek
        '
        Me.btnSiguienteWeek.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSiguienteWeek.BackgroundImage = CType(resources.GetObject("btnSiguienteWeek.BackgroundImage"), System.Drawing.Image)
        Me.btnSiguienteWeek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSiguienteWeek.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.btnSiguienteWeek.ForeColor = System.Drawing.Color.Black
        Me.btnSiguienteWeek.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSiguienteWeek.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSiguienteWeek.Location = New System.Drawing.Point(663, 3)
        Me.btnSiguienteWeek.Name = "btnSiguienteWeek"
        Me.btnSiguienteWeek.Size = New System.Drawing.Size(30, 21)
        Me.btnSiguienteWeek.TabIndex = 9143
        Me.btnSiguienteWeek.Tag = "7"
        Me.btnSiguienteWeek.Text = ">>"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(4, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 9145
        Me.Label2.Text = "Agenda"
        '
        'btnSiguienteDay
        '
        Me.btnSiguienteDay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSiguienteDay.BackgroundImage = CType(resources.GetObject("btnSiguienteDay.BackgroundImage"), System.Drawing.Image)
        Me.btnSiguienteDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSiguienteDay.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.btnSiguienteDay.ForeColor = System.Drawing.Color.Black
        Me.btnSiguienteDay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSiguienteDay.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSiguienteDay.Location = New System.Drawing.Point(633, 3)
        Me.btnSiguienteDay.Name = "btnSiguienteDay"
        Me.btnSiguienteDay.Size = New System.Drawing.Size(30, 21)
        Me.btnSiguienteDay.TabIndex = 9142
        Me.btnSiguienteDay.Tag = "1"
        Me.btnSiguienteDay.Text = ">"
        '
        'btnAnteriorWeek
        '
        Me.btnAnteriorWeek.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAnteriorWeek.BackgroundImage = CType(resources.GetObject("btnAnteriorWeek.BackgroundImage"), System.Drawing.Image)
        Me.btnAnteriorWeek.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnAnteriorWeek.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!)
        Me.btnAnteriorWeek.ForeColor = System.Drawing.Color.Black
        Me.btnAnteriorWeek.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAnteriorWeek.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAnteriorWeek.Location = New System.Drawing.Point(572, 3)
        Me.btnAnteriorWeek.Name = "btnAnteriorWeek"
        Me.btnAnteriorWeek.Size = New System.Drawing.Size(30, 21)
        Me.btnAnteriorWeek.TabIndex = 9140
        Me.btnAnteriorWeek.Tag = "-7"
        Me.btnAnteriorWeek.Text = "<<"
        '
        'TTCita
        '
        Me.TTCita.AutomaticDelay = 0
        Me.TTCita.AutoPopDelay = 5000
        Me.TTCita.InitialDelay = 0
        Me.TTCita.ReshowDelay = 0
        '
        'frmCalendarioColor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(705, 702)
        Me.Controls.Add(Me.lblFecha)
        Me.Controls.Add(Me.btnAnteriorDay)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.btnSiguienteWeek)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnSiguienteDay)
        Me.Controls.Add(Me.btnAnteriorWeek)
        Me.Controls.Add(Me.dtgOcupacion)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.btnActualizar)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.lblNombreAgenda)
        Me.Controls.Add(Me.lblAgenda)
        Me.Name = "frmCalendarioColor"
        Me.Text = "Agenda"
        CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNombreAgenda As System.Windows.Forms.Label
    Friend WithEvents lblAgenda As System.Windows.Forms.Label
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents dtgOcupacion As System.Windows.Forms.DataGridView
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents btnAnteriorDay As System.Windows.Forms.Button
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnSiguienteWeek As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSiguienteDay As System.Windows.Forms.Button
    Friend WithEvents btnAnteriorWeek As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents TTCita As System.Windows.Forms.ToolTip
End Class
