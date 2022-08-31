Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmTrabajoActividad
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form reemplaza a Dispose para limpiar la lista de componentes.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Requerido por el Diseñador de Windows Forms
        Private components As System.ComponentModel.IContainer

        'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
        'Se puede modificar usando el Diseñador de Windows Forms.  
        'No lo modifique con el editor de código.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrabajoActividad))
            Me.grpAsignarHoras = New System.Windows.Forms.GroupBox
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.dtpHoraInicio = New System.Windows.Forms.DateTimePicker
            Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker
            Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker
            Me.dtpHoraFin = New System.Windows.Forms.DateTimePicker
            Me.lblLineaInicio = New System.Windows.Forms.Label
            Me.lblLineaFin = New System.Windows.Forms.Label
            Me.chkHoraFin = New System.Windows.Forms.CheckBox
            Me.chkHoraInicio = New System.Windows.Forms.CheckBox
            Me.btnCancelar = New System.Windows.Forms.Button
            Me.btnAsignar = New System.Windows.Forms.Button
            Me.lblActividad = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.lblActividadTitulo = New System.Windows.Forms.Label
            Me.lblColaborador = New System.Windows.Forms.Label
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.lblColaboradorTitulo = New System.Windows.Forms.Label
            Me.rbtRangoHoras = New System.Windows.Forms.RadioButton
            Me.rbtTiempo = New System.Windows.Forms.RadioButton
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.txtTiempo = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.grpAsignarHoras.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'grpAsignarHoras
            '
            Me.grpAsignarHoras.Controls.Add(Me.Panel1)
            Me.grpAsignarHoras.Controls.Add(Me.dtpHoraInicio)
            Me.grpAsignarHoras.Controls.Add(Me.dtpFechaFin)
            Me.grpAsignarHoras.Controls.Add(Me.dtpFechaInicio)
            Me.grpAsignarHoras.Controls.Add(Me.dtpHoraFin)
            Me.grpAsignarHoras.Controls.Add(Me.lblLineaInicio)
            Me.grpAsignarHoras.Controls.Add(Me.lblLineaFin)
            Me.grpAsignarHoras.Controls.Add(Me.chkHoraFin)
            Me.grpAsignarHoras.Controls.Add(Me.chkHoraInicio)
            resources.ApplyResources(Me.grpAsignarHoras, "grpAsignarHoras")
            Me.grpAsignarHoras.Name = "grpAsignarHoras"
            Me.grpAsignarHoras.TabStop = False
            '
            'Panel1
            '
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
            '
            'dtpHoraInicio
            '
            resources.ApplyResources(Me.dtpHoraInicio, "dtpHoraInicio")
            Me.dtpHoraInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHoraInicio.Name = "dtpHoraInicio"
            Me.dtpHoraInicio.ShowUpDown = True
            '
            'dtpFechaFin
            '
            Me.dtpFechaFin.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.dtpFechaFin, "dtpFechaFin")
            Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaFin.Name = "dtpFechaFin"
            '
            'dtpFechaInicio
            '
            Me.dtpFechaInicio.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.dtpFechaInicio, "dtpFechaInicio")
            Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaInicio.Name = "dtpFechaInicio"
            '
            'dtpHoraFin
            '
            resources.ApplyResources(Me.dtpHoraFin, "dtpHoraFin")
            Me.dtpHoraFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHoraFin.Name = "dtpHoraFin"
            Me.dtpHoraFin.ShowUpDown = True
            '
            'lblLineaInicio
            '
            Me.lblLineaInicio.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLineaInicio, "lblLineaInicio")
            Me.lblLineaInicio.Name = "lblLineaInicio"
            '
            'lblLineaFin
            '
            Me.lblLineaFin.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLineaFin, "lblLineaFin")
            Me.lblLineaFin.Name = "lblLineaFin"
            '
            'chkHoraFin
            '
            resources.ApplyResources(Me.chkHoraFin, "chkHoraFin")
            Me.chkHoraFin.Name = "chkHoraFin"
            Me.chkHoraFin.UseVisualStyleBackColor = True
            '
            'chkHoraInicio
            '
            resources.ApplyResources(Me.chkHoraInicio, "chkHoraInicio")
            Me.chkHoraInicio.Name = "chkHoraInicio"
            Me.chkHoraInicio.UseVisualStyleBackColor = True
            '
            'btnCancelar
            '
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancelar.Name = "btnCancelar"
            '
            'btnAsignar
            '
            Me.btnAsignar.BackColor = System.Drawing.Color.FromArgb(CType(CType(223, Byte), Integer), CType(CType(222, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btnAsignar, "btnAsignar")
            Me.btnAsignar.ForeColor = System.Drawing.Color.Black
            Me.btnAsignar.Name = "btnAsignar"
            Me.btnAsignar.UseVisualStyleBackColor = False
            '
            'lblActividad
            '
            resources.ApplyResources(Me.lblActividad, "lblActividad")
            Me.lblActividad.Name = "lblActividad"
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'lblActividadTitulo
            '
            resources.ApplyResources(Me.lblActividadTitulo, "lblActividadTitulo")
            Me.lblActividadTitulo.Name = "lblActividadTitulo"
            '
            'lblColaborador
            '
            resources.ApplyResources(Me.lblColaborador, "lblColaborador")
            Me.lblColaborador.Name = "lblColaborador"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblColaboradorTitulo
            '
            resources.ApplyResources(Me.lblColaboradorTitulo, "lblColaboradorTitulo")
            Me.lblColaboradorTitulo.Name = "lblColaboradorTitulo"
            '
            'rbtRangoHoras
            '
            resources.ApplyResources(Me.rbtRangoHoras, "rbtRangoHoras")
            Me.rbtRangoHoras.Name = "rbtRangoHoras"
            Me.rbtRangoHoras.TabStop = True
            Me.rbtRangoHoras.UseVisualStyleBackColor = True
            '
            'rbtTiempo
            '
            resources.ApplyResources(Me.rbtTiempo, "rbtTiempo")
            Me.rbtTiempo.Name = "rbtTiempo"
            Me.rbtTiempo.TabStop = True
            Me.rbtTiempo.UseVisualStyleBackColor = True
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.Label2)
            Me.GroupBox2.Controls.Add(Me.txtTiempo)
            Me.GroupBox2.Controls.Add(Me.rbtTiempo)
            resources.ApplyResources(Me.GroupBox2, "GroupBox2")
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.TabStop = False
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'txtTiempo
            '
            Me.txtTiempo.AceptaNegativos = False
            Me.txtTiempo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTiempo.EstiloSBO = True
            resources.ApplyResources(Me.txtTiempo, "txtTiempo")
            Me.txtTiempo.ForeColor = System.Drawing.Color.Black
            Me.txtTiempo.MaxDecimales = 0
            Me.txtTiempo.MaxEnteros = 0
            Me.txtTiempo.Millares = False
            Me.txtTiempo.Name = "txtTiempo"
            Me.txtTiempo.Size_AdjustableHeight = 20
            Me.txtTiempo.TeclasDeshacer = True
            Me.txtTiempo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
            '
            'frmTrabajoActividad
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.Panel2)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.rbtRangoHoras)
            Me.Controls.Add(Me.lblActividad)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.lblActividadTitulo)
            Me.Controls.Add(Me.lblColaborador)
            Me.Controls.Add(Me.lblLine3)
            Me.Controls.Add(Me.lblColaboradorTitulo)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.btnAsignar)
            Me.Controls.Add(Me.grpAsignarHoras)
            Me.MaximizeBox = False
            Me.Name = "frmTrabajoActividad"
            Me.grpAsignarHoras.ResumeLayout(False)
            Me.grpAsignarHoras.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents grpAsignarHoras As System.Windows.Forms.GroupBox
        Public WithEvents lblLineaInicio As System.Windows.Forms.Label
        Friend WithEvents dtpHoraFin As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLineaFin As System.Windows.Forms.Label
        Friend WithEvents chkHoraFin As System.Windows.Forms.CheckBox
        Friend WithEvents chkHoraInicio As System.Windows.Forms.CheckBox
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents btnAsignar As System.Windows.Forms.Button
        Friend WithEvents dtpFechaFin As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpFechaInicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpHoraInicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents lblActividad As System.Windows.Forms.Label
        Public WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents lblActividadTitulo As System.Windows.Forms.Label
        Friend WithEvents lblColaborador As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents lblColaboradorTitulo As System.Windows.Forms.Label
        Friend WithEvents rbtRangoHoras As System.Windows.Forms.RadioButton
        Friend WithEvents rbtTiempo As System.Windows.Forms.RadioButton
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtTiempo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
    End Class

End Namespace