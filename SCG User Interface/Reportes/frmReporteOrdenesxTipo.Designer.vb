Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmReporteOrdenesxTipo

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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteOrdenesxTipo))
            Me.txtDescripcionOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picRepuesto = New System.Windows.Forms.PictureBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
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
            Me.txtIdOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.optMonedaSistema = New System.Windows.Forms.RadioButton
            Me.optMonedaLocal = New System.Windows.Forms.RadioButton
            Me.GroupBox2 = New System.Windows.Forms.GroupBox
            Me.chkTipoOt = New System.Windows.Forms.CheckBox
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.GroupBox1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'txtDescripcionOrden
            '
            Me.txtDescripcionOrden.AceptaNegativos = False
            Me.txtDescripcionOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtDescripcionOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtDescripcionOrden, "txtDescripcionOrden")
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
            Me.picRepuesto.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picRepuesto.Name = "picRepuesto"
            Me.picRepuesto.TabStop = False
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
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
            'txtIdOrden
            '
            Me.txtIdOrden.AceptaNegativos = False
            Me.txtIdOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtIdOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtIdOrden, "txtIdOrden")
            Me.txtIdOrden.MaxDecimales = 0
            Me.txtIdOrden.MaxEnteros = 0
            Me.txtIdOrden.Millares = False
            Me.txtIdOrden.Name = "txtIdOrden"
            Me.txtIdOrden.ReadOnly = True
            Me.txtIdOrden.Size_AdjustableHeight = 20
            Me.txtIdOrden.TeclasDeshacer = True
            Me.txtIdOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'optMonedaSistema
            '
            resources.ApplyResources(Me.optMonedaSistema, "optMonedaSistema")
            Me.optMonedaSistema.Name = "optMonedaSistema"
            Me.optMonedaSistema.TabStop = True
            Me.optMonedaSistema.UseVisualStyleBackColor = True
            '
            'optMonedaLocal
            '
            resources.ApplyResources(Me.optMonedaLocal, "optMonedaLocal")
            Me.optMonedaLocal.Name = "optMonedaLocal"
            Me.optMonedaLocal.TabStop = True
            Me.optMonedaLocal.UseVisualStyleBackColor = True
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.optMonedaLocal)
            Me.GroupBox2.Controls.Add(Me.optMonedaSistema)
            resources.ApplyResources(Me.GroupBox2, "GroupBox2")
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.TabStop = False
            '
            'chkTipoOt
            '
            resources.ApplyResources(Me.chkTipoOt, "chkTipoOt")
            Me.chkTipoOt.Name = "chkTipoOt"
            Me.chkTipoOt.UseVisualStyleBackColor = True
            '
            'frmReporteOrdenesxTipo
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.picRepuesto)
            Me.Controls.Add(Me.chkTipoOt)
            Me.Controls.Add(Me.GroupBox2)
            Me.Controls.Add(Me.txtIdOrden)
            Me.Controls.Add(Me.btnBuscar)
            Me.Controls.Add(Me.txtDescripcionOrden)
            Me.Controls.Add(Me.btncerrar)
            Me.MaximizeBox = False
            Me.Name = "frmReporteOrdenesxTipo"
            CType(Me.picRepuesto, System.ComponentModel.ISupportInitialize).EndInit()
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox2.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtDescripcionOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picRepuesto As System.Windows.Forms.PictureBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
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
        Friend WithEvents txtIdOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents optMonedaSistema As System.Windows.Forms.RadioButton
        Friend WithEvents optMonedaLocal As System.Windows.Forms.RadioButton
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents chkTipoOt As System.Windows.Forms.CheckBox
    End Class
End Namespace