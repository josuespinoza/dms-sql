Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmRangoFechas
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRangoFechas))
            Me.txtIdEmpleado = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtEmpleado = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.dtpHasta = New System.Windows.Forms.DateTimePicker
            Me.dtpDesde = New System.Windows.Forms.DateTimePicker
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.btnBuscar = New System.Windows.Forms.Button
            Me.btncerrar = New System.Windows.Forms.Button
            Me.cbDetallado = New System.Windows.Forms.CheckBox
            Me.cbResumido = New System.Windows.Forms.CheckBox
            Me.lblMecanico = New System.Windows.Forms.Label
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtIdEmpleado
            '
            Me.txtIdEmpleado.AccessibleDescription = Nothing
            Me.txtIdEmpleado.AccessibleName = Nothing
            Me.txtIdEmpleado.AceptaNegativos = False
            resources.ApplyResources(Me.txtIdEmpleado, "txtIdEmpleado")
            Me.txtIdEmpleado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtIdEmpleado.BackgroundImage = Nothing
            Me.txtIdEmpleado.EstiloSBO = True
            Me.txtIdEmpleado.Font = Nothing
            Me.txtIdEmpleado.MaxDecimales = 0
            Me.txtIdEmpleado.MaxEnteros = 0
            Me.txtIdEmpleado.Millares = False
            Me.txtIdEmpleado.Name = "txtIdEmpleado"
            Me.txtIdEmpleado.Size_AdjustableHeight = 20
            Me.txtIdEmpleado.TeclasDeshacer = True
            Me.txtIdEmpleado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtEmpleado
            '
            Me.txtEmpleado.AccessibleDescription = Nothing
            Me.txtEmpleado.AccessibleName = Nothing
            Me.txtEmpleado.AceptaNegativos = False
            resources.ApplyResources(Me.txtEmpleado, "txtEmpleado")
            Me.txtEmpleado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEmpleado.BackgroundImage = Nothing
            Me.txtEmpleado.EstiloSBO = True
            Me.txtEmpleado.Font = Nothing
            Me.txtEmpleado.MaxDecimales = 0
            Me.txtEmpleado.MaxEnteros = 0
            Me.txtEmpleado.Millares = False
            Me.txtEmpleado.Name = "txtEmpleado"
            Me.txtEmpleado.ReadOnly = True
            Me.txtEmpleado.Size_AdjustableHeight = 20
            Me.txtEmpleado.TeclasDeshacer = True
            Me.txtEmpleado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picRepuesto
            '
            Me.picRepuesto.AccessibleDescription = Nothing
            Me.picRepuesto.AccessibleName = Nothing
            resources.ApplyResources(Me.picRepuesto, "picRepuesto")
            Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.picRepuesto.BackgroundImage = Nothing
            Me.picRepuesto.Font = Nothing
            Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picRepuesto.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picRepuesto.ImageLocation = Nothing
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'GroupBox1
            '
            Me.GroupBox1.AccessibleDescription = Nothing
            Me.GroupBox1.AccessibleName = Nothing
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
            Me.GroupBox1.BackgroundImage = Nothing
            Me.GroupBox1.Controls.Add(Me.Panel2)
            Me.GroupBox1.Controls.Add(Me.Panel1)
            Me.GroupBox1.Controls.Add(Me.dtpHasta)
            Me.GroupBox1.Controls.Add(Me.dtpDesde)
            Me.GroupBox1.Controls.Add(Me.lblLine1)
            Me.GroupBox1.Controls.Add(Me.lblLine2)
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'Panel2
            '
            Me.Panel2.AccessibleDescription = Nothing
            Me.Panel2.AccessibleName = Nothing
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.Panel2.Name = "Panel2"
            '
            'Panel1
            '
            Me.Panel1.AccessibleDescription = Nothing
            Me.Panel1.AccessibleName = Nothing
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.Panel1.Name = "Panel1"
            '
            'dtpHasta
            '
            Me.dtpHasta.AccessibleDescription = Nothing
            Me.dtpHasta.AccessibleName = Nothing
            resources.ApplyResources(Me.dtpHasta, "dtpHasta")
            Me.dtpHasta.BackgroundImage = Nothing
            Me.dtpHasta.CalendarFont = Nothing
            Me.dtpHasta.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CustomFormat = Nothing
            Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpHasta.Name = "dtpHasta"
            Me.dtpHasta.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
            '
            'dtpDesde
            '
            Me.dtpDesde.AccessibleDescription = Nothing
            Me.dtpDesde.AccessibleName = Nothing
            resources.ApplyResources(Me.dtpDesde, "dtpDesde")
            Me.dtpDesde.BackgroundImage = Nothing
            Me.dtpDesde.CalendarFont = Nothing
            Me.dtpDesde.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CustomFormat = Nothing
            Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpDesde.Name = "dtpDesde"
            Me.dtpDesde.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
            '
            'lblLine1
            '
            Me.lblLine1.AccessibleDescription = Nothing
            Me.lblLine1.AccessibleName = Nothing
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.lblLine1.Font = Nothing
            Me.lblLine1.Name = "lblLine1"
            '
            'lblLine2
            '
            Me.lblLine2.AccessibleDescription = Nothing
            Me.lblLine2.AccessibleName = Nothing
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.lblLine2.Font = Nothing
            Me.lblLine2.Name = "lblLine2"
            '
            'Label3
            '
            Me.Label3.AccessibleDescription = Nothing
            Me.Label3.AccessibleName = Nothing
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label3.Name = "Label3"
            '
            'Label2
            '
            Me.Label2.AccessibleDescription = Nothing
            Me.Label2.AccessibleName = Nothing
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label2.Name = "Label2"
            '
            'btnBuscar
            '
            Me.btnBuscar.AccessibleDescription = Nothing
            Me.btnBuscar.AccessibleName = Nothing
            resources.ApplyResources(Me.btnBuscar, "btnBuscar")
            Me.btnBuscar.ForeColor = System.Drawing.Color.Black
            Me.btnBuscar.Name = "btnBuscar"
            '
            'btncerrar
            '
            Me.btncerrar.AccessibleDescription = Nothing
            Me.btncerrar.AccessibleName = Nothing
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.Name = "btncerrar"
            '
            'cbDetallado
            '
            Me.cbDetallado.AccessibleDescription = Nothing
            Me.cbDetallado.AccessibleName = Nothing
            resources.ApplyResources(Me.cbDetallado, "cbDetallado")
            Me.cbDetallado.BackgroundImage = Nothing
            Me.cbDetallado.Font = Nothing
            Me.cbDetallado.Name = "cbDetallado"
            Me.cbDetallado.UseVisualStyleBackColor = True
            '
            'cbResumido
            '
            Me.cbResumido.AccessibleDescription = Nothing
            Me.cbResumido.AccessibleName = Nothing
            resources.ApplyResources(Me.cbResumido, "cbResumido")
            Me.cbResumido.BackgroundImage = Nothing
            Me.cbResumido.Font = Nothing
            Me.cbResumido.Name = "cbResumido"
            Me.cbResumido.UseVisualStyleBackColor = True
            '
            'lblMecanico
            '
            Me.lblMecanico.AccessibleDescription = Nothing
            Me.lblMecanico.AccessibleName = Nothing
            resources.ApplyResources(Me.lblMecanico, "lblMecanico")
            Me.lblMecanico.Name = "lblMecanico"
            '
            'frmRangoFechas
            '
            Me.AccessibleDescription = Nothing
            Me.AccessibleName = Nothing
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.BackgroundImage = Nothing
            Me.Controls.Add(Me.picRepuesto)
            Me.Controls.Add(Me.lblMecanico)
            Me.Controls.Add(Me.cbResumido)
            Me.Controls.Add(Me.cbDetallado)
            Me.Controls.Add(Me.txtIdEmpleado)
            Me.Controls.Add(Me.txtEmpleado)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnBuscar)
            Me.Controls.Add(Me.GroupBox1)
            Me.Font = Nothing
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmRangoFechas"
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
        Friend WithEvents txtIdEmpleado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtEmpleado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Public WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
        Public WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Public WithEvents Label2 As System.Windows.Forms.Label
        Public WithEvents Label3 As System.Windows.Forms.Label
        Public WithEvents btnBuscar As System.Windows.Forms.Button
        Public WithEvents btncerrar As System.Windows.Forms.Button
        Friend WithEvents cbDetallado As System.Windows.Forms.CheckBox
        Friend WithEvents cbResumido As System.Windows.Forms.CheckBox
        Friend WithEvents lblMecanico As System.Windows.Forms.Label
    End Class
End Namespace

