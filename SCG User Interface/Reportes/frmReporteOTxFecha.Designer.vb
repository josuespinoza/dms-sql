Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmReporteOTxFecha
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteOTxFecha))
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
            Me.rptReporte = New ComponenteCristalReport.SubReportView
            Me.chkSolosinFacturar = New System.Windows.Forms.CheckBox
            Me.txtIdOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtDescripcionOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picTipoOT = New System.Windows.Forms.PictureBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.txtIDMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtDescripcionMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picMarca = New System.Windows.Forms.PictureBox
            Me.Label5 = New System.Windows.Forms.Label
            Me.Label6 = New System.Windows.Forms.Label
            Me.gbxRangoFechas.SuspendLayout()
            CType(Me.picTipoOT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picMarca, System.ComponentModel.ISupportInitialize).BeginInit()
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
            'chkSolosinFacturar
            '
            resources.ApplyResources(Me.chkSolosinFacturar, "chkSolosinFacturar")
            Me.chkSolosinFacturar.Name = "chkSolosinFacturar"
            Me.chkSolosinFacturar.UseVisualStyleBackColor = True
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
            'picTipoOT
            '
            Me.picTipoOT.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picTipoOT, "picTipoOT")
            Me.picTipoOT.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picTipoOT.Name = "picTipoOT"
            Me.picTipoOT.TabStop = False
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'Label2
            '
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'txtIDMarca
            '
            Me.txtIDMarca.AceptaNegativos = False
            Me.txtIDMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtIDMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtIDMarca, "txtIDMarca")
            Me.txtIDMarca.MaxDecimales = 0
            Me.txtIDMarca.MaxEnteros = 0
            Me.txtIDMarca.Millares = False
            Me.txtIDMarca.Name = "txtIDMarca"
            Me.txtIDMarca.ReadOnly = True
            Me.txtIDMarca.Size_AdjustableHeight = 20
            Me.txtIDMarca.TeclasDeshacer = True
            Me.txtIDMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtDescripcionMarca
            '
            Me.txtDescripcionMarca.AceptaNegativos = False
            Me.txtDescripcionMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtDescripcionMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtDescripcionMarca, "txtDescripcionMarca")
            Me.txtDescripcionMarca.MaxDecimales = 0
            Me.txtDescripcionMarca.MaxEnteros = 0
            Me.txtDescripcionMarca.Millares = False
            Me.txtDescripcionMarca.Name = "txtDescripcionMarca"
            Me.txtDescripcionMarca.ReadOnly = True
            Me.txtDescripcionMarca.Size_AdjustableHeight = 20
            Me.txtDescripcionMarca.TeclasDeshacer = True
            Me.txtDescripcionMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picMarca
            '
            Me.picMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.picMarca, "picMarca")
            Me.picMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.picMarca.Name = "picMarca"
            Me.picMarca.TabStop = False
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'frmReporteOTxFecha
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btncerrar
            Me.Controls.Add(Me.txtIDMarca)
            Me.Controls.Add(Me.txtDescripcionMarca)
            Me.Controls.Add(Me.picMarca)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.Label6)
            Me.Controls.Add(Me.txtIdOrden)
            Me.Controls.Add(Me.txtDescripcionOrden)
            Me.Controls.Add(Me.picTipoOT)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.chkSolosinFacturar)
            Me.Controls.Add(Me.rptReporte)
            Me.Controls.Add(Me.gbxRangoFechas)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnCargar)
            Me.MaximizeBox = False
            Me.Name = "frmReporteOTxFecha"
            Me.gbxRangoFechas.ResumeLayout(False)
            CType(Me.picTipoOT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picMarca, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents rptReporte As ComponenteCristalReport.SubReportView
        Friend WithEvents chkSolosinFacturar As System.Windows.Forms.CheckBox
        Friend WithEvents txtIdOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtDescripcionOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picTipoOT As System.Windows.Forms.PictureBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents txtIDMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtDescripcionMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picMarca As System.Windows.Forms.PictureBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
    End Class
End Namespace