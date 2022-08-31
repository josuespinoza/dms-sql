Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmReporteOTxEstado
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteOTxEstado))
            Me.chkRangoFechas = New System.Windows.Forms.CheckBox
            Me.gbxRangoFechas = New System.Windows.Forms.GroupBox
            Me.Panel2 = New System.Windows.Forms.Panel
            Me.dtpHasta = New System.Windows.Forms.DateTimePicker
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.dtpDesde = New System.Windows.Forms.DateTimePicker
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtIdOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtDescripcionOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblTipoOrden = New System.Windows.Forms.Label
            Me.chkTipoOrden = New System.Windows.Forms.CheckBox
            Me.btncerrar = New System.Windows.Forms.Button
            Me.btnBuscar = New System.Windows.Forms.Button
            Me.rbtEstandar = New System.Windows.Forms.RadioButton
            Me.rbtDetallado = New System.Windows.Forms.RadioButton
            Me.chkEstado = New System.Windows.Forms.CheckBox
            Me.cboEstadoOT = New SCGComboBox.SCGComboBox
            Me.Label5 = New System.Windows.Forms.Label
            Me.lblEstado = New System.Windows.Forms.Label
            Me.gbxRangoFechas.SuspendLayout()
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'chkRangoFechas
            '
            resources.ApplyResources(Me.chkRangoFechas, "chkRangoFechas")
            Me.chkRangoFechas.Name = "chkRangoFechas"
            Me.chkRangoFechas.UseVisualStyleBackColor = True
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
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.Name = "Label4"
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
            Me.txtIdOrden.ReadOnly = True
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
            'picRepuesto
            '
            Me.picRepuesto.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picRepuesto, "picRepuesto")
            Me.picRepuesto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblTipoOrden
            '
            resources.ApplyResources(Me.lblTipoOrden, "lblTipoOrden")
            Me.lblTipoOrden.Name = "lblTipoOrden"
            '
            'chkTipoOrden
            '
            resources.ApplyResources(Me.chkTipoOrden, "chkTipoOrden")
            Me.chkTipoOrden.Name = "chkTipoOrden"
            Me.chkTipoOrden.UseVisualStyleBackColor = True
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
            'rbtEstandar
            '
            resources.ApplyResources(Me.rbtEstandar, "rbtEstandar")
            Me.rbtEstandar.Checked = True
            Me.rbtEstandar.Name = "rbtEstandar"
            Me.rbtEstandar.TabStop = True
            Me.rbtEstandar.UseVisualStyleBackColor = True
            '
            'rbtDetallado
            '
            resources.ApplyResources(Me.rbtDetallado, "rbtDetallado")
            Me.rbtDetallado.Name = "rbtDetallado"
            Me.rbtDetallado.UseVisualStyleBackColor = True
            '
            'chkEstado
            '
            resources.ApplyResources(Me.chkEstado, "chkEstado")
            Me.chkEstado.Name = "chkEstado"
            Me.chkEstado.UseVisualStyleBackColor = True
            '
            'cboEstadoOT
            '
            Me.cboEstadoOT.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboEstadoOT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            resources.ApplyResources(Me.cboEstadoOT, "cboEstadoOT")
            Me.cboEstadoOT.EstiloSBO = True
            Me.cboEstadoOT.Items.AddRange(New Object() {resources.GetString("cboEstadoOT.Items"), resources.GetString("cboEstadoOT.Items1"), resources.GetString("cboEstadoOT.Items2"), resources.GetString("cboEstadoOT.Items3"), resources.GetString("cboEstadoOT.Items4"), resources.GetString("cboEstadoOT.Items5")})
            Me.cboEstadoOT.Name = "cboEstadoOT"
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'lblEstado
            '
            resources.ApplyResources(Me.lblEstado, "lblEstado")
            Me.lblEstado.Name = "lblEstado"
            '
            'frmReporteOTxEstado
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.cboEstadoOT)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.lblEstado)
            Me.Controls.Add(Me.chkEstado)
            Me.Controls.Add(Me.rbtDetallado)
            Me.Controls.Add(Me.rbtEstandar)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnBuscar)
            Me.Controls.Add(Me.chkTipoOrden)
            Me.Controls.Add(Me.txtIdOrden)
            Me.Controls.Add(Me.txtDescripcionOrden)
            Me.Controls.Add(Me.picRepuesto)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblTipoOrden)
            Me.Controls.Add(Me.chkRangoFechas)
            Me.Controls.Add(Me.gbxRangoFechas)
            Me.MaximizeBox = False
            Me.Name = "frmReporteOTxEstado"
            Me.gbxRangoFechas.ResumeLayout(False)
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents chkRangoFechas As System.Windows.Forms.CheckBox
        Friend WithEvents gbxRangoFechas As System.Windows.Forms.GroupBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtIdOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtDescripcionOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblTipoOrden As System.Windows.Forms.Label
        Friend WithEvents chkTipoOrden As System.Windows.Forms.CheckBox
        Protected WithEvents btncerrar As System.Windows.Forms.Button
        Protected WithEvents btnBuscar As System.Windows.Forms.Button
        Friend WithEvents rbtEstandar As System.Windows.Forms.RadioButton
        Friend WithEvents rbtDetallado As System.Windows.Forms.RadioButton
        Friend WithEvents chkEstado As System.Windows.Forms.CheckBox
        Friend WithEvents cboEstadoOT As SCGComboBox.SCGComboBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents lblEstado As System.Windows.Forms.Label
    End Class
End Namespace