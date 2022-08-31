Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfOTsEspeciales
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
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfOTsEspeciales))
            Me.tlbOTsEspeciales = New Proyecto_SCGToolBar.SCGToolBar
            Me.errConfOrdenesEspeciales = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.grpOTEspecial = New System.Windows.Forms.GroupBox
            Me.UsaListaPreciosCheckBox = New System.Windows.Forms.CheckBox
            Me.btnEliminar = New System.Windows.Forms.Button
            Me.btnAgregar = New System.Windows.Forms.Button
            Me.dtgUsuarios = New System.Windows.Forms.DataGridView
            Me.IDConfOTEspecialDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.UsuarioDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.m_dtsUsuarioOTEspecial = New DMSOneFramework.UsuariosOTEspecialDataset
            Me.cboTiposOrdenes = New SCGComboBox.SCGComboBox
            Me.txtCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picCliente = New System.Windows.Forms.PictureBox
            Me.txtAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picAsesor = New System.Windows.Forms.PictureBox
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblCliente = New System.Windows.Forms.Label
            Me.lblLine2 = New System.Windows.Forms.Label
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblAsesor = New System.Windows.Forms.Label
            Me.lblTiposOrdenes = New System.Windows.Forms.Label
            Me.dtgTiposConfigurados = New System.Windows.Forms.DataGridView
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDAsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardCodeClienteDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescTipoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardNameClienteDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.NombreAsesorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.m_dtsConfOrdenesEspeciales = New DMSOneFramework.ConfOrdenesEspeciales
            CType(Me.errConfOrdenesEspeciales, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpOTEspecial.SuspendLayout()
            CType(Me.dtgUsuarios, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dtsUsuarioOTEspecial, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picAsesor, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgTiposConfigurados, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dtsConfOrdenesEspeciales, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'tlbOTsEspeciales
            '
            Me.tlbOTsEspeciales.AccessibleDescription = Nothing
            Me.tlbOTsEspeciales.AccessibleName = Nothing
            resources.ApplyResources(Me.tlbOTsEspeciales, "tlbOTsEspeciales")
            Me.tlbOTsEspeciales.BackgroundImage = Nothing
            Me.tlbOTsEspeciales.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbOTsEspeciales.Font = Nothing
            Me.tlbOTsEspeciales.Name = "tlbOTsEspeciales"
            '
            'errConfOrdenesEspeciales
            '
            Me.errConfOrdenesEspeciales.ContainerControl = Me
            resources.ApplyResources(Me.errConfOrdenesEspeciales, "errConfOrdenesEspeciales")
            '
            'grpOTEspecial
            '
            Me.grpOTEspecial.AccessibleDescription = Nothing
            Me.grpOTEspecial.AccessibleName = Nothing
            resources.ApplyResources(Me.grpOTEspecial, "grpOTEspecial")
            Me.grpOTEspecial.BackgroundImage = Nothing
            Me.grpOTEspecial.Controls.Add(Me.UsaListaPreciosCheckBox)
            Me.grpOTEspecial.Controls.Add(Me.btnEliminar)
            Me.grpOTEspecial.Controls.Add(Me.btnAgregar)
            Me.grpOTEspecial.Controls.Add(Me.dtgUsuarios)
            Me.grpOTEspecial.Controls.Add(Me.cboTiposOrdenes)
            Me.grpOTEspecial.Controls.Add(Me.txtCliente)
            Me.grpOTEspecial.Controls.Add(Me.picCliente)
            Me.grpOTEspecial.Controls.Add(Me.txtAsesor)
            Me.grpOTEspecial.Controls.Add(Me.picAsesor)
            Me.grpOTEspecial.Controls.Add(Me.Label1)
            Me.grpOTEspecial.Controls.Add(Me.lblCliente)
            Me.grpOTEspecial.Controls.Add(Me.lblLine2)
            Me.grpOTEspecial.Controls.Add(Me.lblLine1)
            Me.grpOTEspecial.Controls.Add(Me.lblAsesor)
            Me.grpOTEspecial.Controls.Add(Me.lblTiposOrdenes)
            Me.errConfOrdenesEspeciales.SetError(Me.grpOTEspecial, resources.GetString("grpOTEspecial.Error"))
            Me.grpOTEspecial.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.grpOTEspecial, CType(resources.GetObject("grpOTEspecial.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.grpOTEspecial, CType(resources.GetObject("grpOTEspecial.IconPadding"), Integer))
            Me.grpOTEspecial.Name = "grpOTEspecial"
            Me.grpOTEspecial.TabStop = False
            '
            'UsaListaPreciosCheckBox
            '
            Me.UsaListaPreciosCheckBox.AccessibleDescription = Nothing
            Me.UsaListaPreciosCheckBox.AccessibleName = Nothing
            resources.ApplyResources(Me.UsaListaPreciosCheckBox, "UsaListaPreciosCheckBox")
            Me.UsaListaPreciosCheckBox.BackgroundImage = Nothing
            Me.errConfOrdenesEspeciales.SetError(Me.UsaListaPreciosCheckBox, resources.GetString("UsaListaPreciosCheckBox.Error"))
            Me.UsaListaPreciosCheckBox.Font = Nothing
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.UsaListaPreciosCheckBox, CType(resources.GetObject("UsaListaPreciosCheckBox.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.UsaListaPreciosCheckBox, CType(resources.GetObject("UsaListaPreciosCheckBox.IconPadding"), Integer))
            Me.UsaListaPreciosCheckBox.Name = "UsaListaPreciosCheckBox"
            Me.UsaListaPreciosCheckBox.UseVisualStyleBackColor = True
            '
            'btnEliminar
            '
            Me.btnEliminar.AccessibleDescription = Nothing
            Me.btnEliminar.AccessibleName = Nothing
            resources.ApplyResources(Me.btnEliminar, "btnEliminar")
            Me.errConfOrdenesEspeciales.SetError(Me.btnEliminar, resources.GetString("btnEliminar.Error"))
            Me.btnEliminar.ForeColor = System.Drawing.Color.Black
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.btnEliminar, CType(resources.GetObject("btnEliminar.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.btnEliminar, CType(resources.GetObject("btnEliminar.IconPadding"), Integer))
            Me.btnEliminar.Name = "btnEliminar"
            '
            'btnAgregar
            '
            Me.btnAgregar.AccessibleDescription = Nothing
            Me.btnAgregar.AccessibleName = Nothing
            resources.ApplyResources(Me.btnAgregar, "btnAgregar")
            Me.errConfOrdenesEspeciales.SetError(Me.btnAgregar, resources.GetString("btnAgregar.Error"))
            Me.btnAgregar.ForeColor = System.Drawing.Color.Black
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.btnAgregar, CType(resources.GetObject("btnAgregar.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.btnAgregar, CType(resources.GetObject("btnAgregar.IconPadding"), Integer))
            Me.btnAgregar.Name = "btnAgregar"
            '
            'dtgUsuarios
            '
            Me.dtgUsuarios.AccessibleDescription = Nothing
            Me.dtgUsuarios.AccessibleName = Nothing
            Me.dtgUsuarios.AllowUserToAddRows = False
            Me.dtgUsuarios.AllowUserToDeleteRows = False
            resources.ApplyResources(Me.dtgUsuarios, "dtgUsuarios")
            Me.dtgUsuarios.AutoGenerateColumns = False
            Me.dtgUsuarios.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgUsuarios.BackgroundImage = Nothing
            Me.dtgUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgUsuarios.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDConfOTEspecialDataGridViewTextBoxColumn, Me.IDDataGridViewTextBoxColumn1, Me.Check, Me.UsuarioDataGridViewTextBoxColumn})
            Me.dtgUsuarios.DataMember = "SCGTA_TB_ConfUsuariosConfOTEspecial"
            Me.dtgUsuarios.DataSource = Me.m_dtsUsuarioOTEspecial
            Me.errConfOrdenesEspeciales.SetError(Me.dtgUsuarios, resources.GetString("dtgUsuarios.Error"))
            Me.dtgUsuarios.Font = Nothing
            Me.dtgUsuarios.GridColor = System.Drawing.Color.Silver
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.dtgUsuarios, CType(resources.GetObject("dtgUsuarios.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.dtgUsuarios, CType(resources.GetObject("dtgUsuarios.IconPadding"), Integer))
            Me.dtgUsuarios.Name = "dtgUsuarios"
            '
            'IDConfOTEspecialDataGridViewTextBoxColumn
            '
            Me.IDConfOTEspecialDataGridViewTextBoxColumn.DataPropertyName = "IDConfOTEspecial"
            resources.ApplyResources(Me.IDConfOTEspecialDataGridViewTextBoxColumn, "IDConfOTEspecialDataGridViewTextBoxColumn")
            Me.IDConfOTEspecialDataGridViewTextBoxColumn.Name = "IDConfOTEspecialDataGridViewTextBoxColumn"
            '
            'IDDataGridViewTextBoxColumn1
            '
            Me.IDDataGridViewTextBoxColumn1.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn1, "IDDataGridViewTextBoxColumn1")
            Me.IDDataGridViewTextBoxColumn1.Name = "IDDataGridViewTextBoxColumn1"
            Me.IDDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'Check
            '
            Me.Check.DataPropertyName = "Check"
            Me.Check.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.Check, "Check")
            Me.Check.Name = "Check"
            '
            'UsuarioDataGridViewTextBoxColumn
            '
            Me.UsuarioDataGridViewTextBoxColumn.DataPropertyName = "Usuario"
            resources.ApplyResources(Me.UsuarioDataGridViewTextBoxColumn, "UsuarioDataGridViewTextBoxColumn")
            Me.UsuarioDataGridViewTextBoxColumn.Name = "UsuarioDataGridViewTextBoxColumn"
            Me.UsuarioDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            '
            'm_dtsUsuarioOTEspecial
            '
            Me.m_dtsUsuarioOTEspecial.DataSetName = "UsuariosOTEspecialDataset"
            Me.m_dtsUsuarioOTEspecial.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'cboTiposOrdenes
            '
            Me.cboTiposOrdenes.AccessibleDescription = Nothing
            Me.cboTiposOrdenes.AccessibleName = Nothing
            resources.ApplyResources(Me.cboTiposOrdenes, "cboTiposOrdenes")
            Me.cboTiposOrdenes.BackColor = System.Drawing.Color.White
            Me.cboTiposOrdenes.BackgroundImage = Nothing
            Me.cboTiposOrdenes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.errConfOrdenesEspeciales.SetError(Me.cboTiposOrdenes, resources.GetString("cboTiposOrdenes.Error"))
            Me.cboTiposOrdenes.EstiloSBO = True
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.cboTiposOrdenes, CType(resources.GetObject("cboTiposOrdenes.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.cboTiposOrdenes, CType(resources.GetObject("cboTiposOrdenes.IconPadding"), Integer))
            Me.cboTiposOrdenes.Items.AddRange(New Object() {Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation})
            Me.cboTiposOrdenes.Name = "cboTiposOrdenes"
            '
            'txtCliente
            '
            Me.txtCliente.AccessibleDescription = Nothing
            Me.txtCliente.AccessibleName = Nothing
            Me.txtCliente.AceptaNegativos = False
            resources.ApplyResources(Me.txtCliente, "txtCliente")
            Me.txtCliente.BackColor = System.Drawing.Color.White
            Me.txtCliente.BackgroundImage = Nothing
            Me.errConfOrdenesEspeciales.SetError(Me.txtCliente, resources.GetString("txtCliente.Error"))
            Me.txtCliente.EstiloSBO = True
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.txtCliente, CType(resources.GetObject("txtCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.txtCliente, CType(resources.GetObject("txtCliente.IconPadding"), Integer))
            Me.txtCliente.MaxDecimales = 0
            Me.txtCliente.MaxEnteros = 0
            Me.txtCliente.Millares = False
            Me.txtCliente.Name = "txtCliente"
            Me.txtCliente.Size_AdjustableHeight = 20
            Me.txtCliente.TeclasDeshacer = True
            Me.txtCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picCliente
            '
            Me.picCliente.AccessibleDescription = Nothing
            Me.picCliente.AccessibleName = Nothing
            resources.ApplyResources(Me.picCliente, "picCliente")
            Me.picCliente.BackgroundImage = Nothing
            Me.errConfOrdenesEspeciales.SetError(Me.picCliente, resources.GetString("picCliente.Error"))
            Me.picCliente.Font = Nothing
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.picCliente, CType(resources.GetObject("picCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.picCliente, CType(resources.GetObject("picCliente.IconPadding"), Integer))
            Me.picCliente.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picCliente.ImageLocation = Nothing
            Me.picCliente.Name = "picCliente"
            Me.picCliente.TabStop = False
            '
            'txtAsesor
            '
            Me.txtAsesor.AccessibleDescription = Nothing
            Me.txtAsesor.AccessibleName = Nothing
            Me.txtAsesor.AceptaNegativos = False
            resources.ApplyResources(Me.txtAsesor, "txtAsesor")
            Me.txtAsesor.BackColor = System.Drawing.Color.White
            Me.txtAsesor.BackgroundImage = Nothing
            Me.errConfOrdenesEspeciales.SetError(Me.txtAsesor, resources.GetString("txtAsesor.Error"))
            Me.txtAsesor.EstiloSBO = True
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.txtAsesor, CType(resources.GetObject("txtAsesor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.txtAsesor, CType(resources.GetObject("txtAsesor.IconPadding"), Integer))
            Me.txtAsesor.MaxDecimales = 0
            Me.txtAsesor.MaxEnteros = 0
            Me.txtAsesor.Millares = False
            Me.txtAsesor.Name = "txtAsesor"
            Me.txtAsesor.Size_AdjustableHeight = 20
            Me.txtAsesor.TeclasDeshacer = True
            Me.txtAsesor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picAsesor
            '
            Me.picAsesor.AccessibleDescription = Nothing
            Me.picAsesor.AccessibleName = Nothing
            resources.ApplyResources(Me.picAsesor, "picAsesor")
            Me.picAsesor.BackgroundImage = Nothing
            Me.errConfOrdenesEspeciales.SetError(Me.picAsesor, resources.GetString("picAsesor.Error"))
            Me.picAsesor.Font = Nothing
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.picAsesor, CType(resources.GetObject("picAsesor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.picAsesor, CType(resources.GetObject("picAsesor.IconPadding"), Integer))
            Me.picAsesor.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picAsesor.ImageLocation = Nothing
            Me.picAsesor.Name = "picAsesor"
            Me.picAsesor.TabStop = False
            '
            'Label1
            '
            Me.Label1.AccessibleDescription = Nothing
            Me.Label1.AccessibleName = Nothing
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetError(Me.Label1, resources.GetString("Label1.Error"))
            Me.Label1.Font = Nothing
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.Label1, CType(resources.GetObject("Label1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.Label1, CType(resources.GetObject("Label1.IconPadding"), Integer))
            Me.Label1.Name = "Label1"
            '
            'lblCliente
            '
            Me.lblCliente.AccessibleDescription = Nothing
            Me.lblCliente.AccessibleName = Nothing
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.errConfOrdenesEspeciales.SetError(Me.lblCliente, resources.GetString("lblCliente.Error"))
            Me.lblCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.lblCliente, CType(resources.GetObject("lblCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.lblCliente, CType(resources.GetObject("lblCliente.IconPadding"), Integer))
            Me.lblCliente.Name = "lblCliente"
            '
            'lblLine2
            '
            Me.lblLine2.AccessibleDescription = Nothing
            Me.lblLine2.AccessibleName = Nothing
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetError(Me.lblLine2, resources.GetString("lblLine2.Error"))
            Me.lblLine2.Font = Nothing
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.lblLine2, CType(resources.GetObject("lblLine2.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.lblLine2, CType(resources.GetObject("lblLine2.IconPadding"), Integer))
            Me.lblLine2.Name = "lblLine2"
            '
            'lblLine1
            '
            Me.lblLine1.AccessibleDescription = Nothing
            Me.lblLine1.AccessibleName = Nothing
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetError(Me.lblLine1, resources.GetString("lblLine1.Error"))
            Me.lblLine1.Font = Nothing
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.lblLine1, CType(resources.GetObject("lblLine1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.lblLine1, CType(resources.GetObject("lblLine1.IconPadding"), Integer))
            Me.lblLine1.Name = "lblLine1"
            '
            'lblAsesor
            '
            Me.lblAsesor.AccessibleDescription = Nothing
            Me.lblAsesor.AccessibleName = Nothing
            resources.ApplyResources(Me.lblAsesor, "lblAsesor")
            Me.errConfOrdenesEspeciales.SetError(Me.lblAsesor, resources.GetString("lblAsesor.Error"))
            Me.lblAsesor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.lblAsesor, CType(resources.GetObject("lblAsesor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.lblAsesor, CType(resources.GetObject("lblAsesor.IconPadding"), Integer))
            Me.lblAsesor.Name = "lblAsesor"
            '
            'lblTiposOrdenes
            '
            Me.lblTiposOrdenes.AccessibleDescription = Nothing
            Me.lblTiposOrdenes.AccessibleName = Nothing
            resources.ApplyResources(Me.lblTiposOrdenes, "lblTiposOrdenes")
            Me.errConfOrdenesEspeciales.SetError(Me.lblTiposOrdenes, resources.GetString("lblTiposOrdenes.Error"))
            Me.lblTiposOrdenes.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.lblTiposOrdenes, CType(resources.GetObject("lblTiposOrdenes.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.lblTiposOrdenes, CType(resources.GetObject("lblTiposOrdenes.IconPadding"), Integer))
            Me.lblTiposOrdenes.Name = "lblTiposOrdenes"
            '
            'dtgTiposConfigurados
            '
            Me.dtgTiposConfigurados.AccessibleDescription = Nothing
            Me.dtgTiposConfigurados.AccessibleName = Nothing
            Me.dtgTiposConfigurados.AllowUserToAddRows = False
            Me.dtgTiposConfigurados.AllowUserToDeleteRows = False
            resources.ApplyResources(Me.dtgTiposConfigurados, "dtgTiposConfigurados")
            Me.dtgTiposConfigurados.AutoGenerateColumns = False
            Me.dtgTiposConfigurados.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgTiposConfigurados.BackgroundImage = Nothing
            Me.dtgTiposConfigurados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgTiposConfigurados.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.IDTipoOrdenDataGridViewTextBoxColumn, Me.IDAsesorDataGridViewTextBoxColumn, Me.CardCodeClienteDataGridViewTextBoxColumn, Me.DescTipoOrdenDataGridViewTextBoxColumn, Me.CardNameClienteDataGridViewTextBoxColumn, Me.NombreAsesorDataGridViewTextBoxColumn})
            Me.dtgTiposConfigurados.DataMember = "SCGTA_TB_ConfOrdenesEspeciales"
            Me.dtgTiposConfigurados.DataSource = Me.m_dtsConfOrdenesEspeciales
            Me.errConfOrdenesEspeciales.SetError(Me.dtgTiposConfigurados, resources.GetString("dtgTiposConfigurados.Error"))
            Me.dtgTiposConfigurados.Font = Nothing
            Me.dtgTiposConfigurados.GridColor = System.Drawing.Color.Silver
            Me.errConfOrdenesEspeciales.SetIconAlignment(Me.dtgTiposConfigurados, CType(resources.GetObject("dtgTiposConfigurados.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.errConfOrdenesEspeciales.SetIconPadding(Me.dtgTiposConfigurados, CType(resources.GetObject("dtgTiposConfigurados.IconPadding"), Integer))
            Me.dtgTiposConfigurados.Name = "dtgTiposConfigurados"
            Me.dtgTiposConfigurados.ReadOnly = True
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDTipoOrdenDataGridViewTextBoxColumn
            '
            Me.IDTipoOrdenDataGridViewTextBoxColumn.DataPropertyName = "IDTipoOrden"
            resources.ApplyResources(Me.IDTipoOrdenDataGridViewTextBoxColumn, "IDTipoOrdenDataGridViewTextBoxColumn")
            Me.IDTipoOrdenDataGridViewTextBoxColumn.Name = "IDTipoOrdenDataGridViewTextBoxColumn"
            Me.IDTipoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDAsesorDataGridViewTextBoxColumn
            '
            Me.IDAsesorDataGridViewTextBoxColumn.DataPropertyName = "IDAsesor"
            resources.ApplyResources(Me.IDAsesorDataGridViewTextBoxColumn, "IDAsesorDataGridViewTextBoxColumn")
            Me.IDAsesorDataGridViewTextBoxColumn.Name = "IDAsesorDataGridViewTextBoxColumn"
            Me.IDAsesorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardCodeClienteDataGridViewTextBoxColumn
            '
            Me.CardCodeClienteDataGridViewTextBoxColumn.DataPropertyName = "CardCodeCliente"
            resources.ApplyResources(Me.CardCodeClienteDataGridViewTextBoxColumn, "CardCodeClienteDataGridViewTextBoxColumn")
            Me.CardCodeClienteDataGridViewTextBoxColumn.Name = "CardCodeClienteDataGridViewTextBoxColumn"
            Me.CardCodeClienteDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DescTipoOrdenDataGridViewTextBoxColumn
            '
            Me.DescTipoOrdenDataGridViewTextBoxColumn.DataPropertyName = "DescTipoOrden"
            resources.ApplyResources(Me.DescTipoOrdenDataGridViewTextBoxColumn, "DescTipoOrdenDataGridViewTextBoxColumn")
            Me.DescTipoOrdenDataGridViewTextBoxColumn.Name = "DescTipoOrdenDataGridViewTextBoxColumn"
            Me.DescTipoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardNameClienteDataGridViewTextBoxColumn
            '
            Me.CardNameClienteDataGridViewTextBoxColumn.DataPropertyName = "CardNameCliente"
            resources.ApplyResources(Me.CardNameClienteDataGridViewTextBoxColumn, "CardNameClienteDataGridViewTextBoxColumn")
            Me.CardNameClienteDataGridViewTextBoxColumn.Name = "CardNameClienteDataGridViewTextBoxColumn"
            Me.CardNameClienteDataGridViewTextBoxColumn.ReadOnly = True
            '
            'NombreAsesorDataGridViewTextBoxColumn
            '
            Me.NombreAsesorDataGridViewTextBoxColumn.DataPropertyName = "NombreAsesor"
            resources.ApplyResources(Me.NombreAsesorDataGridViewTextBoxColumn, "NombreAsesorDataGridViewTextBoxColumn")
            Me.NombreAsesorDataGridViewTextBoxColumn.Name = "NombreAsesorDataGridViewTextBoxColumn"
            Me.NombreAsesorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'm_dtsConfOrdenesEspeciales
            '
            Me.m_dtsConfOrdenesEspeciales.DataSetName = "ConfOrdenesEspeciales"
            Me.m_dtsConfOrdenesEspeciales.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmConfOTsEspeciales
            '
            Me.AccessibleDescription = Nothing
            Me.AccessibleName = Nothing
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.BackgroundImage = Nothing
            Me.Controls.Add(Me.grpOTEspecial)
            Me.Controls.Add(Me.dtgTiposConfigurados)
            Me.Controls.Add(Me.tlbOTsEspeciales)
            Me.Font = Nothing
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmConfOTsEspeciales"
            CType(Me.errConfOrdenesEspeciales, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpOTEspecial.ResumeLayout(False)
            Me.grpOTEspecial.PerformLayout()
            CType(Me.dtgUsuarios, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dtsUsuarioOTEspecial, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picAsesor, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgTiposConfigurados, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dtsConfOrdenesEspeciales, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents tlbOTsEspeciales As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtgTiposConfigurados As System.Windows.Forms.DataGridView
        Friend WithEvents m_dtsConfOrdenesEspeciales As DMSOneFramework.ConfOrdenesEspeciales
        Friend WithEvents errConfOrdenesEspeciales As System.Windows.Forms.ErrorProvider
        Friend WithEvents grpOTEspecial As System.Windows.Forms.GroupBox
        Friend WithEvents dtgUsuarios As System.Windows.Forms.DataGridView
        Friend WithEvents cboTiposOrdenes As SCGComboBox.SCGComboBox
        Friend WithEvents txtCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picCliente As System.Windows.Forms.PictureBox
        Friend WithEvents txtAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picAsesor As System.Windows.Forms.PictureBox
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblAsesor As System.Windows.Forms.Label
        Friend WithEvents lblTiposOrdenes As System.Windows.Forms.Label
        Friend WithEvents m_dtsUsuarioOTEspecial As DMSOneFramework.UsuariosOTEspecialDataset
        Friend WithEvents btnEliminar As System.Windows.Forms.Button
        Friend WithEvents btnAgregar As System.Windows.Forms.Button
        Friend WithEvents IDConfOTEspecialDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents UsuarioDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDAsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeClienteDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescTipoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameClienteDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NombreAsesorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents UsaListaPreciosCheckBox As System.Windows.Forms.CheckBox
    End Class
End Namespace