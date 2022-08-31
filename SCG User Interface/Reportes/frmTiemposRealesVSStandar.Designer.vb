Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmTiemposRealesVSStandar
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTiemposRealesVSStandar))
            Me.gbxRangoFechas = New System.Windows.Forms.GroupBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.dtpHasta = New System.Windows.Forms.DateTimePicker
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.dtpDesde = New System.Windows.Forms.DateTimePicker
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.btncerrar = New System.Windows.Forms.Button
            Me.btnCargar = New System.Windows.Forms.Button
            Me.txtIdEmpleado = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtEmpleado = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picEmpleado = New System.Windows.Forms.PictureBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblMecanico = New System.Windows.Forms.Label
            Me.chkMecanico = New System.Windows.Forms.CheckBox
            Me.chkRangoFechas = New System.Windows.Forms.CheckBox
            Me.txtIdOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtDescripcionOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picTipoOT = New System.Windows.Forms.PictureBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblTipoOT = New System.Windows.Forms.Label
            Me.chkTipoOrden = New System.Windows.Forms.CheckBox
            Me.gbxRangoFechas.SuspendLayout()
            CType(Me.picEmpleado, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picTipoOT, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
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
            resources.ApplyResources(Me.gbxRangoFechas, "gbxRangoFechas")
            Me.gbxRangoFechas.Name = "gbxRangoFechas"
            Me.gbxRangoFechas.TabStop = False
            '
            'Panel2
            '
            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.Name = "Panel2"
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
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.Name = "Panel1"
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
            Me.lblLine1.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label3.Name = "Label3"
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.Name = "Label4"
            '
            'btncerrar
            '
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.Name = "btncerrar"
            '
            'btnCargar
            '
            resources.ApplyResources(Me.btnCargar, "btnCargar")
            Me.btnCargar.ForeColor = System.Drawing.Color.Black
            Me.btnCargar.Name = "btnCargar"
            '
            'txtIdEmpleado
            '
            Me.txtIdEmpleado.AceptaNegativos = False
            Me.txtIdEmpleado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.txtIdEmpleado, "txtIdEmpleado")
            Me.txtIdEmpleado.EstiloSBO = True
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
            Me.txtEmpleado.AceptaNegativos = False
            Me.txtEmpleado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.txtEmpleado, "txtEmpleado")
            Me.txtEmpleado.EstiloSBO = True
            Me.txtEmpleado.MaxDecimales = 0
            Me.txtEmpleado.MaxEnteros = 0
            Me.txtEmpleado.Millares = False
            Me.txtEmpleado.Name = "txtEmpleado"
            Me.txtEmpleado.ReadOnly = True
            Me.txtEmpleado.Size_AdjustableHeight = 20
            Me.txtEmpleado.TeclasDeshacer = True
            Me.txtEmpleado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picEmpleado
            '
            Me.picEmpleado.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picEmpleado, "picEmpleado")
            Me.picEmpleado.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picEmpleado.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picEmpleado.Name = "picEmpleado"
            Me.picEmpleado.TabStop = False
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblMecanico
            '
            resources.ApplyResources(Me.lblMecanico, "lblMecanico")
            Me.lblMecanico.Name = "lblMecanico"
            '
            'chkMecanico
            '
            resources.ApplyResources(Me.chkMecanico, "chkMecanico")
            Me.chkMecanico.Name = "chkMecanico"
            Me.chkMecanico.UseVisualStyleBackColor = True
            '
            'chkRangoFechas
            '
            resources.ApplyResources(Me.chkRangoFechas, "chkRangoFechas")
            Me.chkRangoFechas.Name = "chkRangoFechas"
            Me.chkRangoFechas.UseVisualStyleBackColor = True
            '
            'txtIdOrden
            '
            Me.txtIdOrden.AceptaNegativos = False
            Me.txtIdOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.txtIdOrden, "txtIdOrden")
            Me.txtIdOrden.EstiloSBO = True
            Me.txtIdOrden.MaxDecimales = 0
            Me.txtIdOrden.MaxEnteros = 0
            Me.txtIdOrden.Millares = False
            Me.txtIdOrden.Name = "txtIdOrden"
            Me.txtIdOrden.Size_AdjustableHeight = 20
            Me.txtIdOrden.TeclasDeshacer = True
            Me.txtIdOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtDescripcionOrden
            '
            Me.txtDescripcionOrden.AceptaNegativos = False
            Me.txtDescripcionOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.txtDescripcionOrden, "txtDescripcionOrden")
            Me.txtDescripcionOrden.EstiloSBO = True
            Me.txtDescripcionOrden.MaxDecimales = 0
            Me.txtDescripcionOrden.MaxEnteros = 0
            Me.txtDescripcionOrden.Millares = False
            Me.txtDescripcionOrden.Name = "txtDescripcionOrden"
            Me.txtDescripcionOrden.ReadOnly = True
            Me.txtDescripcionOrden.Size_AdjustableHeight = 20
            Me.txtDescripcionOrden.TeclasDeshacer = True
            Me.txtDescripcionOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picTipoOT
            '
            Me.picTipoOT.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picTipoOT, "picTipoOT")
            Me.picTipoOT.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picTipoOT.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picTipoOT.Name = "picTipoOT"
            Me.picTipoOT.TabStop = False
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'lblTipoOT
            '
            resources.ApplyResources(Me.lblTipoOT, "lblTipoOT")
            Me.lblTipoOT.Name = "lblTipoOT"
            '
            'chkTipoOrden
            '
            resources.ApplyResources(Me.chkTipoOrden, "chkTipoOrden")
            Me.chkTipoOrden.Name = "chkTipoOrden"
            Me.chkTipoOrden.UseVisualStyleBackColor = True
            '
            'frmTiemposRealesVSStandar
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.lblTipoOT)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.chkMecanico)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnCargar)
            Me.Controls.Add(Me.gbxRangoFechas)
            Me.Controls.Add(Me.lblMecanico)
            Me.Controls.Add(Me.chkTipoOrden)
            Me.Controls.Add(Me.picTipoOT)
            Me.Controls.Add(Me.txtIdOrden)
            Me.Controls.Add(Me.txtDescripcionOrden)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.chkRangoFechas)
            Me.Controls.Add(Me.picEmpleado)
            Me.Controls.Add(Me.txtIdEmpleado)
            Me.Controls.Add(Me.txtEmpleado)
            Me.MaximizeBox = False
            Me.Name = "frmTiemposRealesVSStandar"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
            Me.gbxRangoFechas.ResumeLayout(False)
            CType(Me.picEmpleado, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picTipoOT, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
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
        Friend WithEvents txtIdEmpleado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtEmpleado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picEmpleado As System.Windows.Forms.PictureBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblMecanico As System.Windows.Forms.Label
        Friend WithEvents chkMecanico As System.Windows.Forms.CheckBox
        Friend WithEvents chkRangoFechas As System.Windows.Forms.CheckBox
        Friend WithEvents txtIdOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtDescripcionOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picTipoOT As System.Windows.Forms.PictureBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblTipoOT As System.Windows.Forms.Label
        Friend WithEvents chkTipoOrden As System.Windows.Forms.CheckBox
    End Class
End Namespace