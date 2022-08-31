<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCalendario
    Inherits System.Windows.Forms.Form


    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalendario))
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.dtgOcupacion = New System.Windows.Forms.DataGrid()
        Me.btnActualizar = New System.Windows.Forms.Button()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.btnAnteriorDay = New System.Windows.Forms.Button()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.btnSiguienteWeek = New System.Windows.Forms.Button()
        Me.lblAgenda = New System.Windows.Forms.Label()
        Me.btnSiguienteDay = New System.Windows.Forms.Button()
        Me.btnAnteriorWeek = New System.Windows.Forms.Button()
        Me.lblNombreAgenda = New System.Windows.Forms.Label()
        Me.TTCita = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCerrar
        '
        resources.ApplyResources(Me.btnCerrar, "btnCerrar")
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.ForeColor = System.Drawing.Color.Black
        Me.btnCerrar.Name = "btnCerrar"
        '
        'dtgOcupacion
        '
        resources.ApplyResources(Me.dtgOcupacion, "dtgOcupacion")
        Me.dtgOcupacion.BackgroundColor = System.Drawing.Color.White
        Me.dtgOcupacion.CaptionBackColor = System.Drawing.SystemColors.MenuHighlight
        Me.dtgOcupacion.CaptionVisible = False
        Me.dtgOcupacion.DataMember = Global.DMS_Addon.My.Resources.Resource.YaExisteUsuarioXSucursal
        Me.dtgOcupacion.GridLineColor = System.Drawing.Color.Silver
        Me.dtgOcupacion.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
        Me.dtgOcupacion.HeaderBackColor = System.Drawing.Color.White
        Me.dtgOcupacion.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtgOcupacion.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
        Me.dtgOcupacion.Name = "dtgOcupacion"
        Me.dtgOcupacion.PreferredRowHeight = 25
        Me.dtgOcupacion.RowHeadersVisible = False
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
        Me.dtpFecha.Value = New Date(2007, 3, 8, 0, 0, 0, 0)
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
        'lblNombreAgenda
        '
        resources.ApplyResources(Me.lblNombreAgenda, "lblNombreAgenda")
        Me.lblNombreAgenda.Name = "lblNombreAgenda"
        '
        'btnAceptar
        '
        resources.ApplyResources(Me.btnAceptar, "btnAceptar")
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'frmCalendario
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.lblNombreAgenda)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.dtgOcupacion)
        Me.Controls.Add(Me.btnActualizar)
        Me.Controls.Add(Me.lblFecha)
        Me.Controls.Add(Me.btnAnteriorDay)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.btnSiguienteWeek)
        Me.Controls.Add(Me.lblAgenda)
        Me.Controls.Add(Me.btnSiguienteDay)
        Me.Controls.Add(Me.btnAnteriorWeek)
        Me.Name = "frmCalendario"
        CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCerrar As System.Windows.Forms.Button
    Friend WithEvents dtgOcupacion As System.Windows.Forms.DataGrid
    Friend WithEvents btnActualizar As System.Windows.Forms.Button
    Friend WithEvents lblFecha As System.Windows.Forms.Label
    Friend WithEvents btnAnteriorDay As System.Windows.Forms.Button
    Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnSiguienteWeek As System.Windows.Forms.Button
    Friend WithEvents lblAgenda As System.Windows.Forms.Label
    Friend WithEvents btnSiguienteDay As System.Windows.Forms.Button
    Friend WithEvents btnAnteriorWeek As System.Windows.Forms.Button
    Friend WithEvents lblNombreAgenda As System.Windows.Forms.Label
    Friend WithEvents TTCita As System.Windows.Forms.ToolTip
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
