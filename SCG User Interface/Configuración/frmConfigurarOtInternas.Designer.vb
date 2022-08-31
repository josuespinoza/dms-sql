Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfigurarOtInternas
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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfigurarOtInternas))
            Me.dtgOtInternas = New System.Windows.Forms.DataGridView
            Me.piCuentasContables = New System.Windows.Forms.PictureBox
            Me.picBuscadorTiposOrdenes = New System.Windows.Forms.PictureBox
            Me.txtNumeroCuenta = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtTipoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtNombreCuenta = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.ScgToolBar1 = New Proyecto_SCGToolBar.SCGToolBar
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblTipoOrden = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblCuentaContable = New System.Windows.Forms.Label
            Me.m_adpTransacciones = New DMSOneFramework.SCGTA_VW_Tran_CompDatasetTableAdapters.SCGTA_VW_Tran_CompTableAdapter
            Me.m_dtsTransacciones = New DMSOneFramework.SCGTA_VW_Tran_CompDataset
            Me.cboTransaccion = New SCGComboBox.SCGComboBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblTransaccion = New System.Windows.Forms.Label
            Me.dstOTIntertasDataSet = New DMSONEDKFramework.Conf_Ot_IternaDataSet
            CType(Me.dtgOtInternas, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.piCuentasContables, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picBuscadorTiposOrdenes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dtsTransacciones, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dstOTIntertasDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtgOtInternas
            '
            Me.dtgOtInternas.AccessibleDescription = Nothing
            Me.dtgOtInternas.AccessibleName = Nothing
            resources.ApplyResources(Me.dtgOtInternas, "dtgOtInternas")
            Me.dtgOtInternas.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgOtInternas.BackgroundImage = Nothing
            Me.dtgOtInternas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgOtInternas.Font = Nothing
            Me.dtgOtInternas.GridColor = System.Drawing.Color.Silver
            Me.dtgOtInternas.Name = "dtgOtInternas"
            '
            'piCuentasContables
            '
            Me.piCuentasContables.AccessibleDescription = Nothing
            Me.piCuentasContables.AccessibleName = Nothing
            resources.ApplyResources(Me.piCuentasContables, "piCuentasContables")
            Me.piCuentasContables.BackgroundImage = Nothing
            Me.piCuentasContables.Font = Nothing
            Me.piCuentasContables.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.piCuentasContables.ImageLocation = Nothing
            Me.piCuentasContables.Name = "piCuentasContables"
            Me.piCuentasContables.TabStop = False
            '
            'picBuscadorTiposOrdenes
            '
            Me.picBuscadorTiposOrdenes.AccessibleDescription = Nothing
            Me.picBuscadorTiposOrdenes.AccessibleName = Nothing
            resources.ApplyResources(Me.picBuscadorTiposOrdenes, "picBuscadorTiposOrdenes")
            Me.picBuscadorTiposOrdenes.BackgroundImage = Nothing
            Me.picBuscadorTiposOrdenes.Font = Nothing
            Me.picBuscadorTiposOrdenes.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picBuscadorTiposOrdenes.ImageLocation = Nothing
            Me.picBuscadorTiposOrdenes.Name = "picBuscadorTiposOrdenes"
            Me.picBuscadorTiposOrdenes.TabStop = False
            '
            'txtNumeroCuenta
            '
            Me.txtNumeroCuenta.AccessibleDescription = Nothing
            Me.txtNumeroCuenta.AccessibleName = Nothing
            Me.txtNumeroCuenta.AceptaNegativos = False
            resources.ApplyResources(Me.txtNumeroCuenta, "txtNumeroCuenta")
            Me.txtNumeroCuenta.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNumeroCuenta.BackgroundImage = Nothing
            Me.txtNumeroCuenta.EstiloSBO = True
            Me.txtNumeroCuenta.MaxDecimales = 0
            Me.txtNumeroCuenta.MaxEnteros = 0
            Me.txtNumeroCuenta.Millares = False
            Me.txtNumeroCuenta.Name = "txtNumeroCuenta"
            Me.txtNumeroCuenta.ReadOnly = True
            Me.txtNumeroCuenta.Size_AdjustableHeight = 20
            Me.txtNumeroCuenta.TeclasDeshacer = True
            Me.txtNumeroCuenta.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtTipoOrden
            '
            Me.txtTipoOrden.AccessibleDescription = Nothing
            Me.txtTipoOrden.AccessibleName = Nothing
            Me.txtTipoOrden.AceptaNegativos = False
            resources.ApplyResources(Me.txtTipoOrden, "txtTipoOrden")
            Me.txtTipoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTipoOrden.BackgroundImage = Nothing
            Me.txtTipoOrden.EstiloSBO = True
            Me.txtTipoOrden.MaxDecimales = 0
            Me.txtTipoOrden.MaxEnteros = 0
            Me.txtTipoOrden.Millares = False
            Me.txtTipoOrden.Name = "txtTipoOrden"
            Me.txtTipoOrden.ReadOnly = True
            Me.txtTipoOrden.Size_AdjustableHeight = 20
            Me.txtTipoOrden.TeclasDeshacer = True
            Me.txtTipoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNombreCuenta
            '
            Me.txtNombreCuenta.AccessibleDescription = Nothing
            Me.txtNombreCuenta.AccessibleName = Nothing
            Me.txtNombreCuenta.AceptaNegativos = False
            resources.ApplyResources(Me.txtNombreCuenta, "txtNombreCuenta")
            Me.txtNombreCuenta.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNombreCuenta.BackgroundImage = Nothing
            Me.txtNombreCuenta.EstiloSBO = True
            Me.txtNombreCuenta.MaxDecimales = 0
            Me.txtNombreCuenta.MaxEnteros = 0
            Me.txtNombreCuenta.Millares = False
            Me.txtNombreCuenta.Name = "txtNombreCuenta"
            Me.txtNombreCuenta.ReadOnly = True
            Me.txtNombreCuenta.Size_AdjustableHeight = 20
            Me.txtNombreCuenta.TeclasDeshacer = True
            Me.txtNombreCuenta.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'ScgToolBar1
            '
            Me.ScgToolBar1.AccessibleDescription = Nothing
            Me.ScgToolBar1.AccessibleName = Nothing
            resources.ApplyResources(Me.ScgToolBar1, "ScgToolBar1")
            Me.ScgToolBar1.BackgroundImage = Nothing
            Me.ScgToolBar1.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgToolBar1.Font = Nothing
            Me.ScgToolBar1.Name = "ScgToolBar1"
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
            'lblTipoOrden
            '
            Me.lblTipoOrden.AccessibleDescription = Nothing
            Me.lblTipoOrden.AccessibleName = Nothing
            resources.ApplyResources(Me.lblTipoOrden, "lblTipoOrden")
            Me.lblTipoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipoOrden.Name = "lblTipoOrden"
            '
            'Label1
            '
            Me.Label1.AccessibleDescription = Nothing
            Me.Label1.AccessibleName = Nothing
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.Label1.Font = Nothing
            Me.Label1.Name = "Label1"
            '
            'lblCuentaContable
            '
            Me.lblCuentaContable.AccessibleDescription = Nothing
            Me.lblCuentaContable.AccessibleName = Nothing
            resources.ApplyResources(Me.lblCuentaContable, "lblCuentaContable")
            Me.lblCuentaContable.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCuentaContable.Name = "lblCuentaContable"
            '
            'm_adpTransacciones
            '
            Me.m_adpTransacciones.ClearBeforeFill = True
            '
            'm_dtsTransacciones
            '
            Me.m_dtsTransacciones.DataSetName = "SCGTA_VW_Tran_CompDataset"
            Me.m_dtsTransacciones.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'cboTransaccion
            '
            Me.cboTransaccion.AccessibleDescription = Nothing
            Me.cboTransaccion.AccessibleName = Nothing
            resources.ApplyResources(Me.cboTransaccion, "cboTransaccion")
            Me.cboTransaccion.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTransaccion.BackgroundImage = Nothing
            Me.cboTransaccion.DataSource = Me.m_dtsTransacciones
            Me.cboTransaccion.DisplayMember = "SCGTA_VW_Tran_Comp.Name"
            Me.cboTransaccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTransaccion.EstiloSBO = True
            Me.cboTransaccion.Name = "cboTransaccion"
            Me.cboTransaccion.ValueMember = "SCGTA_VW_Tran_Comp.Code"
            '
            'Label2
            '
            Me.Label2.AccessibleDescription = Nothing
            Me.Label2.AccessibleName = Nothing
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.Label2.Font = Nothing
            Me.Label2.Name = "Label2"
            '
            'lblTransaccion
            '
            Me.lblTransaccion.AccessibleDescription = Nothing
            Me.lblTransaccion.AccessibleName = Nothing
            resources.ApplyResources(Me.lblTransaccion, "lblTransaccion")
            Me.lblTransaccion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTransaccion.Name = "lblTransaccion"
            '
            'dstOTIntertasDataSet
            '
            Me.dstOTIntertasDataSet.DataSetName = "Conf_Ot_IternaDataSet"
            Me.dstOTIntertasDataSet.Namespace = "http://www.tempuri.org/Conf_Ot_Iterna"
            '
            'frmConfigurarOtInternas
            '
            Me.AccessibleDescription = Nothing
            Me.AccessibleName = Nothing
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.BackgroundImage = Nothing
            Me.Controls.Add(Me.cboTransaccion)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.lblTransaccion)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblCuentaContable)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblTipoOrden)
            Me.Controls.Add(Me.txtNombreCuenta)
            Me.Controls.Add(Me.txtTipoOrden)
            Me.Controls.Add(Me.txtNumeroCuenta)
            Me.Controls.Add(Me.picBuscadorTiposOrdenes)
            Me.Controls.Add(Me.piCuentasContables)
            Me.Controls.Add(Me.dtgOtInternas)
            Me.Controls.Add(Me.ScgToolBar1)
            Me.Font = Nothing
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmConfigurarOtInternas"
            Me.Tag = "Configuración,1"
            CType(Me.dtgOtInternas, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.piCuentasContables, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picBuscadorTiposOrdenes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dtsTransacciones, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dstOTIntertasDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents dtgOtInternas As System.Windows.Forms.DataGridView
        Friend WithEvents piCuentasContables As System.Windows.Forms.PictureBox
        Friend WithEvents picBuscadorTiposOrdenes As System.Windows.Forms.PictureBox
        Friend WithEvents txtNumeroCuenta As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtTipoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNombreCuenta As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents ScgToolBar1 As Proyecto_SCGToolBar.SCGToolBar
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblTipoOrden As System.Windows.Forms.Label
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblCuentaContable As System.Windows.Forms.Label
        Friend WithEvents m_adpTransacciones As DMSOneFramework.SCGTA_VW_Tran_CompDatasetTableAdapters.SCGTA_VW_Tran_CompTableAdapter
        Friend WithEvents m_dtsTransacciones As DMSOneFramework.SCGTA_VW_Tran_CompDataset
        Friend WithEvents cboTransaccion As SCGComboBox.SCGComboBox
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblTransaccion As System.Windows.Forms.Label
        Private WithEvents dstOTIntertasDataSet As DMSONEDKFramework.Conf_Ot_IternaDataSet
    End Class
End Namespace