Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfMensajeria
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP
        'Inherits System.Windows.Forms.Form

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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfMensajeria))
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblEncargadoAcc = New System.Windows.Forms.Label()
            Me.lblLine1 = New System.Windows.Forms.Label()
            Me.lblCentroCosto = New System.Windows.Forms.Label()
            Me.txtEncargadoAcc = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picEncargadoAcc = New System.Windows.Forms.PictureBox()
            Me.dtgConfMensajeria = New System.Windows.Forms.DataGridView()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.lblEncargadoRep = New System.Windows.Forms.Label()
            Me.txtEncargadoRep = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picEncargadoRep = New System.Windows.Forms.PictureBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.lblEncargadoSum = New System.Windows.Forms.Label()
            Me.txtEncargadoSum = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picEncargadoSum = New System.Windows.Forms.PictureBox()
            Me.cboCentroCosto = New SCGComboBox.SCGComboBox()
            Me.txtIdConfMensajeria = New System.Windows.Forms.TextBox()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.lblEncargadoSer = New System.Windows.Forms.Label()
            Me.txtEncargadoSer = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picEncargadoSer = New System.Windows.Forms.PictureBox()
            CType(Me.picEncargadoAcc, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgConfMensajeria, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoRep, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoSum, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picEncargadoSer, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ScgToolBar1
            '
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Name = "ScgToolBar1"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblEncargadoAcc
            '
            resources.ApplyResources(Me.lblEncargadoAcc, "lblEncargadoAcc")
            Me.lblEncargadoAcc.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEncargadoAcc.Name = "lblEncargadoAcc"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblCentroCosto
            '
            resources.ApplyResources(Me.lblCentroCosto, "lblCentroCosto")
            Me.lblCentroCosto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCentroCosto.Name = "lblCentroCosto"
            '
            'txtEncargadoAcc
            '
            Me.txtEncargadoAcc.AceptaNegativos = False
            Me.txtEncargadoAcc.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEncargadoAcc.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoAcc, "txtEncargadoAcc")
            Me.txtEncargadoAcc.MaxDecimales = 0
            Me.txtEncargadoAcc.MaxEnteros = 0
            Me.txtEncargadoAcc.Millares = False
            Me.txtEncargadoAcc.Name = "txtEncargadoAcc"
            Me.txtEncargadoAcc.Size_AdjustableHeight = 20
            Me.txtEncargadoAcc.TeclasDeshacer = True
            Me.txtEncargadoAcc.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picEncargadoAcc
            '
            Me.picEncargadoAcc.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoAcc, "picEncargadoAcc")
            Me.picEncargadoAcc.Name = "picEncargadoAcc"
            Me.picEncargadoAcc.TabStop = False
            '
            'dtgConfMensajeria
            '
            Me.dtgConfMensajeria.AllowUserToAddRows = False
            Me.dtgConfMensajeria.AllowUserToDeleteRows = False
            Me.dtgConfMensajeria.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgConfMensajeria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgConfMensajeria.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgConfMensajeria, "dtgConfMensajeria")
            Me.dtgConfMensajeria.Name = "dtgConfMensajeria"
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'lblEncargadoRep
            '
            resources.ApplyResources(Me.lblEncargadoRep, "lblEncargadoRep")
            Me.lblEncargadoRep.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEncargadoRep.Name = "lblEncargadoRep"
            '
            'txtEncargadoRep
            '
            Me.txtEncargadoRep.AceptaNegativos = False
            Me.txtEncargadoRep.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEncargadoRep.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoRep, "txtEncargadoRep")
            Me.txtEncargadoRep.MaxDecimales = 0
            Me.txtEncargadoRep.MaxEnteros = 0
            Me.txtEncargadoRep.Millares = False
            Me.txtEncargadoRep.Name = "txtEncargadoRep"
            Me.txtEncargadoRep.Size_AdjustableHeight = 20
            Me.txtEncargadoRep.TeclasDeshacer = True
            Me.txtEncargadoRep.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picEncargadoRep
            '
            Me.picEncargadoRep.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoRep, "picEncargadoRep")
            Me.picEncargadoRep.Name = "picEncargadoRep"
            Me.picEncargadoRep.TabStop = False
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'lblEncargadoSum
            '
            resources.ApplyResources(Me.lblEncargadoSum, "lblEncargadoSum")
            Me.lblEncargadoSum.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEncargadoSum.Name = "lblEncargadoSum"
            '
            'txtEncargadoSum
            '
            Me.txtEncargadoSum.AceptaNegativos = False
            Me.txtEncargadoSum.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEncargadoSum.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoSum, "txtEncargadoSum")
            Me.txtEncargadoSum.MaxDecimales = 0
            Me.txtEncargadoSum.MaxEnteros = 0
            Me.txtEncargadoSum.Millares = False
            Me.txtEncargadoSum.Name = "txtEncargadoSum"
            Me.txtEncargadoSum.Size_AdjustableHeight = 20
            Me.txtEncargadoSum.TeclasDeshacer = True
            Me.txtEncargadoSum.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picEncargadoSum
            '
            Me.picEncargadoSum.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoSum, "picEncargadoSum")
            Me.picEncargadoSum.Name = "picEncargadoSum"
            Me.picEncargadoSum.TabStop = False
            '
            'cboCentroCosto
            '
            Me.cboCentroCosto.BackColor = System.Drawing.Color.White
            Me.cboCentroCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboCentroCosto.EstiloSBO = True
            Me.cboCentroCosto.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation})
            resources.ApplyResources(Me.cboCentroCosto, "cboCentroCosto")
            Me.cboCentroCosto.Name = "cboCentroCosto"
            '
            'txtIdConfMensajeria
            '
            resources.ApplyResources(Me.txtIdConfMensajeria, "txtIdConfMensajeria")
            Me.txtIdConfMensajeria.Name = "txtIdConfMensajeria"
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'lblEncargadoSer
            '
            resources.ApplyResources(Me.lblEncargadoSer, "lblEncargadoSer")
            Me.lblEncargadoSer.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEncargadoSer.Name = "lblEncargadoSer"
            '
            'txtEncargadoSer
            '
            Me.txtEncargadoSer.AceptaNegativos = False
            Me.txtEncargadoSer.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEncargadoSer.EstiloSBO = True
            resources.ApplyResources(Me.txtEncargadoSer, "txtEncargadoSer")
            Me.txtEncargadoSer.MaxDecimales = 0
            Me.txtEncargadoSer.MaxEnteros = 0
            Me.txtEncargadoSer.Millares = False
            Me.txtEncargadoSer.Name = "txtEncargadoSer"
            Me.txtEncargadoSer.Size_AdjustableHeight = 20
            Me.txtEncargadoSer.TeclasDeshacer = True
            Me.txtEncargadoSer.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picEncargadoSer
            '
            Me.picEncargadoSer.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picEncargadoSer, "picEncargadoSer")
            Me.picEncargadoSer.Name = "picEncargadoSer"
            Me.picEncargadoSer.TabStop = False
            '
            'frmConfMensajeria
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.picEncargadoSer)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.lblEncargadoSer)
            Me.Controls.Add(Me.txtEncargadoSer)
            Me.Controls.Add(Me.txtIdConfMensajeria)
            Me.Controls.Add(Me.cboCentroCosto)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.lblEncargadoSum)
            Me.Controls.Add(Me.txtEncargadoSum)
            Me.Controls.Add(Me.picEncargadoSum)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblEncargadoRep)
            Me.Controls.Add(Me.txtEncargadoRep)
            Me.Controls.Add(Me.picEncargadoRep)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblEncargadoAcc)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblCentroCosto)
            Me.Controls.Add(Me.txtEncargadoAcc)
            Me.Controls.Add(Me.picEncargadoAcc)
            Me.Controls.Add(Me.dtgConfMensajeria)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.Name = "frmConfMensajeria"
            CType(Me.picEncargadoAcc, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgConfMensajeria, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoRep, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoSum, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picEncargadoSer, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblEncargadoAcc As System.Windows.Forms.Label
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblCentroCosto As System.Windows.Forms.Label
        Friend WithEvents txtEncargadoAcc As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picEncargadoAcc As System.Windows.Forms.PictureBox
        Friend WithEvents dtgConfMensajeria As System.Windows.Forms.DataGridView
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblEncargadoRep As System.Windows.Forms.Label
        Friend WithEvents txtEncargadoRep As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picEncargadoRep As System.Windows.Forms.PictureBox
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents lblEncargadoSum As System.Windows.Forms.Label
        Friend WithEvents txtEncargadoSum As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picEncargadoSum As System.Windows.Forms.PictureBox
        Friend WithEvents cboCentroCosto As SCGComboBox.SCGComboBox
        Friend WithEvents txtIdConfMensajeria As System.Windows.Forms.TextBox
        Public WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents lblEncargadoSer As System.Windows.Forms.Label
        Friend WithEvents txtEncargadoSer As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picEncargadoSer As System.Windows.Forms.PictureBox
    End Class
End Namespace
