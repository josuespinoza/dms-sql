Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfCatalogoRepxMarca
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfCatalogoRepxMarca))
            Me.grpMarca = New System.Windows.Forms.GroupBox
            Me.btnAgregarAct = New System.Windows.Forms.Button
            Me.DataGridView1 = New System.Windows.Forms.DataGridView
            Me.IDCatalogoRepxMarcaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardCodeProveedorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.CardNameProveedorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ProveedorXMarca = New DMSOneFramework.ProveedorXMarcaDataset
            Me.Label3 = New System.Windows.Forms.Label
            Me.cboMarcas = New SCGComboBox.SCGComboBox
            Me.lblMarca = New System.Windows.Forms.Label
            Me.grpSeguridad = New System.Windows.Forms.GroupBox
            Me.Label6 = New System.Windows.Forms.Label
            Me.Label5 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.txtPasswordServidor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtUsuarioServidor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtServidor = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblServidor = New System.Windows.Forms.Label
            Me.lblUsuario = New System.Windows.Forms.Label
            Me.lblContraseña = New System.Windows.Forms.Label
            Me.grpCompañia = New System.Windows.Forms.GroupBox
            Me.Label9 = New System.Windows.Forms.Label
            Me.Label8 = New System.Windows.Forms.Label
            Me.Label7 = New System.Windows.Forms.Label
            Me.txtPasswordSBO = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtUsuarioSBO = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label1 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.picCompañia = New System.Windows.Forms.PictureBox
            Me.cboCompañia = New SCGComboBox.SCGComboBox
            Me.lblCompania = New System.Windows.Forms.Label
            Me.grpDetallesCatalogo = New System.Windows.Forms.GroupBox
            Me.Label15 = New System.Windows.Forms.Label
            Me.Label14 = New System.Windows.Forms.Label
            Me.picAlmacen = New System.Windows.Forms.PictureBox
            Me.txtAlmacen = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.picListaPrecios = New System.Windows.Forms.PictureBox
            Me.txtListaPrecios = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblAlmacen = New System.Windows.Forms.Label
            Me.lblListaPrecios = New System.Windows.Forms.Label
            Me.btnCancelar = New System.Windows.Forms.Button
            Me.epConfRepXMarca = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.grpMarca.SuspendLayout()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ProveedorXMarca, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpSeguridad.SuspendLayout()
            Me.grpCompañia.SuspendLayout()
            CType(Me.picCompañia, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpDetallesCatalogo.SuspendLayout()
            CType(Me.picAlmacen, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picListaPrecios, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.epConfRepXMarca, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grpMarca
            '
            Me.grpMarca.AccessibleDescription = Nothing
            Me.grpMarca.AccessibleName = Nothing
            resources.ApplyResources(Me.grpMarca, "grpMarca")
            Me.grpMarca.BackgroundImage = Nothing
            Me.grpMarca.Controls.Add(Me.btnAgregarAct)
            Me.grpMarca.Controls.Add(Me.DataGridView1)
            Me.grpMarca.Controls.Add(Me.Label3)
            Me.grpMarca.Controls.Add(Me.cboMarcas)
            Me.grpMarca.Controls.Add(Me.lblMarca)
            Me.epConfRepXMarca.SetError(Me.grpMarca, resources.GetString("grpMarca.Error"))
            Me.grpMarca.ForeColor = System.Drawing.Color.Black
            Me.epConfRepXMarca.SetIconAlignment(Me.grpMarca, CType(resources.GetObject("grpMarca.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.grpMarca, CType(resources.GetObject("grpMarca.IconPadding"), Integer))
            Me.grpMarca.Name = "grpMarca"
            Me.grpMarca.TabStop = False
            '
            'btnAgregarAct
            '
            Me.btnAgregarAct.AccessibleDescription = Nothing
            Me.btnAgregarAct.AccessibleName = Nothing
            resources.ApplyResources(Me.btnAgregarAct, "btnAgregarAct")
            Me.epConfRepXMarca.SetError(Me.btnAgregarAct, resources.GetString("btnAgregarAct.Error"))
            Me.btnAgregarAct.ForeColor = System.Drawing.Color.Maroon
            Me.epConfRepXMarca.SetIconAlignment(Me.btnAgregarAct, CType(resources.GetObject("btnAgregarAct.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.btnAgregarAct, CType(resources.GetObject("btnAgregarAct.IconPadding"), Integer))
            Me.btnAgregarAct.Name = "btnAgregarAct"
            '
            'DataGridView1
            '
            Me.DataGridView1.AccessibleDescription = Nothing
            Me.DataGridView1.AccessibleName = Nothing
            Me.DataGridView1.AllowUserToAddRows = False
            resources.ApplyResources(Me.DataGridView1, "DataGridView1")
            Me.DataGridView1.AutoGenerateColumns = False
            Me.DataGridView1.BackgroundColor = System.Drawing.SystemColors.Control
            Me.DataGridView1.BackgroundImage = Nothing
            Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDCatalogoRepxMarcaDataGridViewTextBoxColumn, Me.CardCodeProveedorDataGridViewTextBoxColumn, Me.CardNameProveedorDataGridViewTextBoxColumn, Me.IDDataGridViewTextBoxColumn})
            Me.DataGridView1.DataMember = "SCGTB_TA_ProveedorXMarca"
            Me.DataGridView1.DataSource = Me.ProveedorXMarca
            Me.epConfRepXMarca.SetError(Me.DataGridView1, resources.GetString("DataGridView1.Error"))
            Me.DataGridView1.Font = Nothing
            Me.epConfRepXMarca.SetIconAlignment(Me.DataGridView1, CType(resources.GetObject("DataGridView1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.DataGridView1, CType(resources.GetObject("DataGridView1.IconPadding"), Integer))
            Me.DataGridView1.Name = "DataGridView1"
            Me.DataGridView1.ReadOnly = True
            '
            'IDCatalogoRepxMarcaDataGridViewTextBoxColumn
            '
            Me.IDCatalogoRepxMarcaDataGridViewTextBoxColumn.DataPropertyName = "IDCatalogoRepxMarca"
            resources.ApplyResources(Me.IDCatalogoRepxMarcaDataGridViewTextBoxColumn, "IDCatalogoRepxMarcaDataGridViewTextBoxColumn")
            Me.IDCatalogoRepxMarcaDataGridViewTextBoxColumn.Name = "IDCatalogoRepxMarcaDataGridViewTextBoxColumn"
            Me.IDCatalogoRepxMarcaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardCodeProveedorDataGridViewTextBoxColumn
            '
            Me.CardCodeProveedorDataGridViewTextBoxColumn.DataPropertyName = "CardCodeProveedor"
            resources.ApplyResources(Me.CardCodeProveedorDataGridViewTextBoxColumn, "CardCodeProveedorDataGridViewTextBoxColumn")
            Me.CardCodeProveedorDataGridViewTextBoxColumn.Name = "CardCodeProveedorDataGridViewTextBoxColumn"
            Me.CardCodeProveedorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CardNameProveedorDataGridViewTextBoxColumn
            '
            Me.CardNameProveedorDataGridViewTextBoxColumn.DataPropertyName = "CardNameProveedor"
            resources.ApplyResources(Me.CardNameProveedorDataGridViewTextBoxColumn, "CardNameProveedorDataGridViewTextBoxColumn")
            Me.CardNameProveedorDataGridViewTextBoxColumn.Name = "CardNameProveedorDataGridViewTextBoxColumn"
            Me.CardNameProveedorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ProveedorXMarca
            '
            Me.ProveedorXMarca.DataSetName = "ProveedorXMarcaDataset"
            Me.ProveedorXMarca.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'Label3
            '
            Me.Label3.AccessibleDescription = Nothing
            Me.Label3.AccessibleName = Nothing
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label3, resources.GetString("Label3.Error"))
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label3, CType(resources.GetObject("Label3.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label3, CType(resources.GetObject("Label3.IconPadding"), Integer))
            Me.Label3.Name = "Label3"
            '
            'cboMarcas
            '
            Me.cboMarcas.AccessibleDescription = Nothing
            Me.cboMarcas.AccessibleName = Nothing
            resources.ApplyResources(Me.cboMarcas, "cboMarcas")
            Me.cboMarcas.BackColor = System.Drawing.Color.White
            Me.cboMarcas.BackgroundImage = Nothing
            Me.cboMarcas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.epConfRepXMarca.SetError(Me.cboMarcas, resources.GetString("cboMarcas.Error"))
            Me.cboMarcas.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.cboMarcas, CType(resources.GetObject("cboMarcas.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.cboMarcas, CType(resources.GetObject("cboMarcas.IconPadding"), Integer))
            Me.cboMarcas.Name = "cboMarcas"
            '
            'lblMarca
            '
            Me.lblMarca.AccessibleDescription = Nothing
            Me.lblMarca.AccessibleName = Nothing
            resources.ApplyResources(Me.lblMarca, "lblMarca")
            Me.epConfRepXMarca.SetError(Me.lblMarca, resources.GetString("lblMarca.Error"))
            Me.lblMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblMarca, CType(resources.GetObject("lblMarca.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblMarca, CType(resources.GetObject("lblMarca.IconPadding"), Integer))
            Me.lblMarca.Name = "lblMarca"
            '
            'grpSeguridad
            '
            Me.grpSeguridad.AccessibleDescription = Nothing
            Me.grpSeguridad.AccessibleName = Nothing
            resources.ApplyResources(Me.grpSeguridad, "grpSeguridad")
            Me.grpSeguridad.BackgroundImage = Nothing
            Me.grpSeguridad.Controls.Add(Me.Label6)
            Me.grpSeguridad.Controls.Add(Me.Label5)
            Me.grpSeguridad.Controls.Add(Me.Label4)
            Me.grpSeguridad.Controls.Add(Me.txtPasswordServidor)
            Me.grpSeguridad.Controls.Add(Me.txtUsuarioServidor)
            Me.grpSeguridad.Controls.Add(Me.txtServidor)
            Me.grpSeguridad.Controls.Add(Me.lblServidor)
            Me.grpSeguridad.Controls.Add(Me.lblUsuario)
            Me.grpSeguridad.Controls.Add(Me.lblContraseña)
            Me.epConfRepXMarca.SetError(Me.grpSeguridad, resources.GetString("grpSeguridad.Error"))
            Me.grpSeguridad.ForeColor = System.Drawing.Color.Black
            Me.epConfRepXMarca.SetIconAlignment(Me.grpSeguridad, CType(resources.GetObject("grpSeguridad.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.grpSeguridad, CType(resources.GetObject("grpSeguridad.IconPadding"), Integer))
            Me.grpSeguridad.Name = "grpSeguridad"
            Me.grpSeguridad.TabStop = False
            '
            'Label6
            '
            Me.Label6.AccessibleDescription = Nothing
            Me.Label6.AccessibleName = Nothing
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label6, resources.GetString("Label6.Error"))
            Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label6, CType(resources.GetObject("Label6.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label6, CType(resources.GetObject("Label6.IconPadding"), Integer))
            Me.Label6.Name = "Label6"
            '
            'Label5
            '
            Me.Label5.AccessibleDescription = Nothing
            Me.Label5.AccessibleName = Nothing
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label5, resources.GetString("Label5.Error"))
            Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label5, CType(resources.GetObject("Label5.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label5, CType(resources.GetObject("Label5.IconPadding"), Integer))
            Me.Label5.Name = "Label5"
            '
            'Label4
            '
            Me.Label4.AccessibleDescription = Nothing
            Me.Label4.AccessibleName = Nothing
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label4, resources.GetString("Label4.Error"))
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label4, CType(resources.GetObject("Label4.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label4, CType(resources.GetObject("Label4.IconPadding"), Integer))
            Me.Label4.Name = "Label4"
            '
            'txtPasswordServidor
            '
            Me.txtPasswordServidor.AccessibleDescription = Nothing
            Me.txtPasswordServidor.AccessibleName = Nothing
            Me.txtPasswordServidor.AceptaNegativos = False
            resources.ApplyResources(Me.txtPasswordServidor, "txtPasswordServidor")
            Me.txtPasswordServidor.BackColor = System.Drawing.Color.White
            Me.txtPasswordServidor.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtPasswordServidor, resources.GetString("txtPasswordServidor.Error"))
            Me.txtPasswordServidor.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtPasswordServidor, CType(resources.GetObject("txtPasswordServidor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtPasswordServidor, CType(resources.GetObject("txtPasswordServidor.IconPadding"), Integer))
            Me.txtPasswordServidor.MaxDecimales = 0
            Me.txtPasswordServidor.MaxEnteros = 0
            Me.txtPasswordServidor.Millares = False
            Me.txtPasswordServidor.Name = "txtPasswordServidor"
            Me.txtPasswordServidor.Size_AdjustableHeight = 20
            Me.txtPasswordServidor.TeclasDeshacer = True
            Me.txtPasswordServidor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtUsuarioServidor
            '
            Me.txtUsuarioServidor.AccessibleDescription = Nothing
            Me.txtUsuarioServidor.AccessibleName = Nothing
            Me.txtUsuarioServidor.AceptaNegativos = False
            resources.ApplyResources(Me.txtUsuarioServidor, "txtUsuarioServidor")
            Me.txtUsuarioServidor.BackColor = System.Drawing.Color.White
            Me.txtUsuarioServidor.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtUsuarioServidor, resources.GetString("txtUsuarioServidor.Error"))
            Me.txtUsuarioServidor.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtUsuarioServidor, CType(resources.GetObject("txtUsuarioServidor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtUsuarioServidor, CType(resources.GetObject("txtUsuarioServidor.IconPadding"), Integer))
            Me.txtUsuarioServidor.MaxDecimales = 0
            Me.txtUsuarioServidor.MaxEnteros = 0
            Me.txtUsuarioServidor.Millares = False
            Me.txtUsuarioServidor.Name = "txtUsuarioServidor"
            Me.txtUsuarioServidor.Size_AdjustableHeight = 20
            Me.txtUsuarioServidor.TeclasDeshacer = True
            Me.txtUsuarioServidor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtServidor
            '
            Me.txtServidor.AccessibleDescription = Nothing
            Me.txtServidor.AccessibleName = Nothing
            Me.txtServidor.AceptaNegativos = False
            resources.ApplyResources(Me.txtServidor, "txtServidor")
            Me.txtServidor.BackColor = System.Drawing.Color.White
            Me.txtServidor.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtServidor, resources.GetString("txtServidor.Error"))
            Me.txtServidor.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtServidor, CType(resources.GetObject("txtServidor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtServidor, CType(resources.GetObject("txtServidor.IconPadding"), Integer))
            Me.txtServidor.MaxDecimales = 0
            Me.txtServidor.MaxEnteros = 0
            Me.txtServidor.Millares = False
            Me.txtServidor.Name = "txtServidor"
            Me.txtServidor.Size_AdjustableHeight = 20
            Me.txtServidor.TeclasDeshacer = True
            Me.txtServidor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblServidor
            '
            Me.lblServidor.AccessibleDescription = Nothing
            Me.lblServidor.AccessibleName = Nothing
            resources.ApplyResources(Me.lblServidor, "lblServidor")
            Me.epConfRepXMarca.SetError(Me.lblServidor, resources.GetString("lblServidor.Error"))
            Me.lblServidor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblServidor, CType(resources.GetObject("lblServidor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblServidor, CType(resources.GetObject("lblServidor.IconPadding"), Integer))
            Me.lblServidor.Name = "lblServidor"
            '
            'lblUsuario
            '
            Me.lblUsuario.AccessibleDescription = Nothing
            Me.lblUsuario.AccessibleName = Nothing
            resources.ApplyResources(Me.lblUsuario, "lblUsuario")
            Me.epConfRepXMarca.SetError(Me.lblUsuario, resources.GetString("lblUsuario.Error"))
            Me.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblUsuario, CType(resources.GetObject("lblUsuario.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblUsuario, CType(resources.GetObject("lblUsuario.IconPadding"), Integer))
            Me.lblUsuario.Name = "lblUsuario"
            '
            'lblContraseña
            '
            Me.lblContraseña.AccessibleDescription = Nothing
            Me.lblContraseña.AccessibleName = Nothing
            resources.ApplyResources(Me.lblContraseña, "lblContraseña")
            Me.epConfRepXMarca.SetError(Me.lblContraseña, resources.GetString("lblContraseña.Error"))
            Me.lblContraseña.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblContraseña, CType(resources.GetObject("lblContraseña.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblContraseña, CType(resources.GetObject("lblContraseña.IconPadding"), Integer))
            Me.lblContraseña.Name = "lblContraseña"
            '
            'grpCompañia
            '
            Me.grpCompañia.AccessibleDescription = Nothing
            Me.grpCompañia.AccessibleName = Nothing
            resources.ApplyResources(Me.grpCompañia, "grpCompañia")
            Me.grpCompañia.BackgroundImage = Nothing
            Me.grpCompañia.Controls.Add(Me.Label9)
            Me.grpCompañia.Controls.Add(Me.Label8)
            Me.grpCompañia.Controls.Add(Me.Label7)
            Me.grpCompañia.Controls.Add(Me.txtPasswordSBO)
            Me.grpCompañia.Controls.Add(Me.txtUsuarioSBO)
            Me.grpCompañia.Controls.Add(Me.Label1)
            Me.grpCompañia.Controls.Add(Me.Label2)
            Me.grpCompañia.Controls.Add(Me.picCompañia)
            Me.grpCompañia.Controls.Add(Me.cboCompañia)
            Me.grpCompañia.Controls.Add(Me.lblCompania)
            Me.epConfRepXMarca.SetError(Me.grpCompañia, resources.GetString("grpCompañia.Error"))
            Me.grpCompañia.ForeColor = System.Drawing.Color.Black
            Me.epConfRepXMarca.SetIconAlignment(Me.grpCompañia, CType(resources.GetObject("grpCompañia.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.grpCompañia, CType(resources.GetObject("grpCompañia.IconPadding"), Integer))
            Me.grpCompañia.Name = "grpCompañia"
            Me.grpCompañia.TabStop = False
            '
            'Label9
            '
            Me.Label9.AccessibleDescription = Nothing
            Me.Label9.AccessibleName = Nothing
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label9, resources.GetString("Label9.Error"))
            Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label9, CType(resources.GetObject("Label9.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label9, CType(resources.GetObject("Label9.IconPadding"), Integer))
            Me.Label9.Name = "Label9"
            '
            'Label8
            '
            Me.Label8.AccessibleDescription = Nothing
            Me.Label8.AccessibleName = Nothing
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label8, resources.GetString("Label8.Error"))
            Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label8, CType(resources.GetObject("Label8.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label8, CType(resources.GetObject("Label8.IconPadding"), Integer))
            Me.Label8.Name = "Label8"
            '
            'Label7
            '
            Me.Label7.AccessibleDescription = Nothing
            Me.Label7.AccessibleName = Nothing
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label7, resources.GetString("Label7.Error"))
            Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label7, CType(resources.GetObject("Label7.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label7, CType(resources.GetObject("Label7.IconPadding"), Integer))
            Me.Label7.Name = "Label7"
            '
            'txtPasswordSBO
            '
            Me.txtPasswordSBO.AccessibleDescription = Nothing
            Me.txtPasswordSBO.AccessibleName = Nothing
            Me.txtPasswordSBO.AceptaNegativos = False
            resources.ApplyResources(Me.txtPasswordSBO, "txtPasswordSBO")
            Me.txtPasswordSBO.BackColor = System.Drawing.Color.White
            Me.txtPasswordSBO.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtPasswordSBO, resources.GetString("txtPasswordSBO.Error"))
            Me.txtPasswordSBO.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtPasswordSBO, CType(resources.GetObject("txtPasswordSBO.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtPasswordSBO, CType(resources.GetObject("txtPasswordSBO.IconPadding"), Integer))
            Me.txtPasswordSBO.MaxDecimales = 0
            Me.txtPasswordSBO.MaxEnteros = 0
            Me.txtPasswordSBO.Millares = False
            Me.txtPasswordSBO.Name = "txtPasswordSBO"
            Me.txtPasswordSBO.Size_AdjustableHeight = 20
            Me.txtPasswordSBO.TeclasDeshacer = True
            Me.txtPasswordSBO.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtUsuarioSBO
            '
            Me.txtUsuarioSBO.AccessibleDescription = Nothing
            Me.txtUsuarioSBO.AccessibleName = Nothing
            Me.txtUsuarioSBO.AceptaNegativos = False
            resources.ApplyResources(Me.txtUsuarioSBO, "txtUsuarioSBO")
            Me.txtUsuarioSBO.BackColor = System.Drawing.Color.White
            Me.txtUsuarioSBO.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtUsuarioSBO, resources.GetString("txtUsuarioSBO.Error"))
            Me.txtUsuarioSBO.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtUsuarioSBO, CType(resources.GetObject("txtUsuarioSBO.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtUsuarioSBO, CType(resources.GetObject("txtUsuarioSBO.IconPadding"), Integer))
            Me.txtUsuarioSBO.MaxDecimales = 0
            Me.txtUsuarioSBO.MaxEnteros = 0
            Me.txtUsuarioSBO.Millares = False
            Me.txtUsuarioSBO.Name = "txtUsuarioSBO"
            Me.txtUsuarioSBO.Size_AdjustableHeight = 20
            Me.txtUsuarioSBO.TeclasDeshacer = True
            Me.txtUsuarioSBO.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label1
            '
            Me.Label1.AccessibleDescription = Nothing
            Me.Label1.AccessibleName = Nothing
            resources.ApplyResources(Me.Label1, "Label1")
            Me.epConfRepXMarca.SetError(Me.Label1, resources.GetString("Label1.Error"))
            Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label1, CType(resources.GetObject("Label1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label1, CType(resources.GetObject("Label1.IconPadding"), Integer))
            Me.Label1.Name = "Label1"
            '
            'Label2
            '
            Me.Label2.AccessibleDescription = Nothing
            Me.Label2.AccessibleName = Nothing
            resources.ApplyResources(Me.Label2, "Label2")
            Me.epConfRepXMarca.SetError(Me.Label2, resources.GetString("Label2.Error"))
            Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label2, CType(resources.GetObject("Label2.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label2, CType(resources.GetObject("Label2.IconPadding"), Integer))
            Me.Label2.Name = "Label2"
            '
            'picCompañia
            '
            Me.picCompañia.AccessibleDescription = Nothing
            Me.picCompañia.AccessibleName = Nothing
            resources.ApplyResources(Me.picCompañia, "picCompañia")
            Me.picCompañia.BackgroundImage = Global.SCG_User_Interface.My.Resources.Resources.S_B_NOAC
            Me.epConfRepXMarca.SetError(Me.picCompañia, resources.GetString("picCompañia.Error"))
            Me.picCompañia.Font = Nothing
            Me.epConfRepXMarca.SetIconAlignment(Me.picCompañia, CType(resources.GetObject("picCompañia.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.picCompañia, CType(resources.GetObject("picCompañia.IconPadding"), Integer))
            Me.picCompañia.ImageLocation = Nothing
            Me.picCompañia.Name = "picCompañia"
            Me.picCompañia.TabStop = False
            '
            'cboCompañia
            '
            Me.cboCompañia.AccessibleDescription = Nothing
            Me.cboCompañia.AccessibleName = Nothing
            resources.ApplyResources(Me.cboCompañia, "cboCompañia")
            Me.cboCompañia.BackColor = System.Drawing.Color.White
            Me.cboCompañia.BackgroundImage = Nothing
            Me.cboCompañia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.epConfRepXMarca.SetError(Me.cboCompañia, resources.GetString("cboCompañia.Error"))
            Me.cboCompañia.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.cboCompañia, CType(resources.GetObject("cboCompañia.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.cboCompañia, CType(resources.GetObject("cboCompañia.IconPadding"), Integer))
            Me.cboCompañia.Name = "cboCompañia"
            '
            'lblCompania
            '
            Me.lblCompania.AccessibleDescription = Nothing
            Me.lblCompania.AccessibleName = Nothing
            resources.ApplyResources(Me.lblCompania, "lblCompania")
            Me.epConfRepXMarca.SetError(Me.lblCompania, resources.GetString("lblCompania.Error"))
            Me.lblCompania.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblCompania, CType(resources.GetObject("lblCompania.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblCompania, CType(resources.GetObject("lblCompania.IconPadding"), Integer))
            Me.lblCompania.Name = "lblCompania"
            '
            'grpDetallesCatalogo
            '
            Me.grpDetallesCatalogo.AccessibleDescription = Nothing
            Me.grpDetallesCatalogo.AccessibleName = Nothing
            resources.ApplyResources(Me.grpDetallesCatalogo, "grpDetallesCatalogo")
            Me.grpDetallesCatalogo.BackgroundImage = Nothing
            Me.grpDetallesCatalogo.Controls.Add(Me.Label15)
            Me.grpDetallesCatalogo.Controls.Add(Me.Label14)
            Me.grpDetallesCatalogo.Controls.Add(Me.picAlmacen)
            Me.grpDetallesCatalogo.Controls.Add(Me.txtAlmacen)
            Me.grpDetallesCatalogo.Controls.Add(Me.picListaPrecios)
            Me.grpDetallesCatalogo.Controls.Add(Me.txtListaPrecios)
            Me.grpDetallesCatalogo.Controls.Add(Me.lblAlmacen)
            Me.grpDetallesCatalogo.Controls.Add(Me.lblListaPrecios)
            Me.epConfRepXMarca.SetError(Me.grpDetallesCatalogo, resources.GetString("grpDetallesCatalogo.Error"))
            Me.grpDetallesCatalogo.ForeColor = System.Drawing.Color.Black
            Me.epConfRepXMarca.SetIconAlignment(Me.grpDetallesCatalogo, CType(resources.GetObject("grpDetallesCatalogo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.grpDetallesCatalogo, CType(resources.GetObject("grpDetallesCatalogo.IconPadding"), Integer))
            Me.grpDetallesCatalogo.Name = "grpDetallesCatalogo"
            Me.grpDetallesCatalogo.TabStop = False
            '
            'Label15
            '
            Me.Label15.AccessibleDescription = Nothing
            Me.Label15.AccessibleName = Nothing
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label15, resources.GetString("Label15.Error"))
            Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label15, CType(resources.GetObject("Label15.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label15, CType(resources.GetObject("Label15.IconPadding"), Integer))
            Me.Label15.Name = "Label15"
            '
            'Label14
            '
            Me.Label14.AccessibleDescription = Nothing
            Me.Label14.AccessibleName = Nothing
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.epConfRepXMarca.SetError(Me.Label14, resources.GetString("Label14.Error"))
            Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.Label14, CType(resources.GetObject("Label14.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.Label14, CType(resources.GetObject("Label14.IconPadding"), Integer))
            Me.Label14.Name = "Label14"
            '
            'picAlmacen
            '
            Me.picAlmacen.AccessibleDescription = Nothing
            Me.picAlmacen.AccessibleName = Nothing
            resources.ApplyResources(Me.picAlmacen, "picAlmacen")
            Me.picAlmacen.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.picAlmacen, resources.GetString("picAlmacen.Error"))
            Me.picAlmacen.Font = Nothing
            Me.epConfRepXMarca.SetIconAlignment(Me.picAlmacen, CType(resources.GetObject("picAlmacen.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.picAlmacen, CType(resources.GetObject("picAlmacen.IconPadding"), Integer))
            Me.picAlmacen.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picAlmacen.ImageLocation = Nothing
            Me.picAlmacen.Name = "picAlmacen"
            Me.picAlmacen.TabStop = False
            '
            'txtAlmacen
            '
            Me.txtAlmacen.AccessibleDescription = Nothing
            Me.txtAlmacen.AccessibleName = Nothing
            Me.txtAlmacen.AceptaNegativos = False
            resources.ApplyResources(Me.txtAlmacen, "txtAlmacen")
            Me.txtAlmacen.BackColor = System.Drawing.Color.White
            Me.txtAlmacen.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtAlmacen, resources.GetString("txtAlmacen.Error"))
            Me.txtAlmacen.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtAlmacen, CType(resources.GetObject("txtAlmacen.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtAlmacen, CType(resources.GetObject("txtAlmacen.IconPadding"), Integer))
            Me.txtAlmacen.MaxDecimales = 0
            Me.txtAlmacen.MaxEnteros = 0
            Me.txtAlmacen.Millares = False
            Me.txtAlmacen.Name = "txtAlmacen"
            Me.txtAlmacen.Size_AdjustableHeight = 20
            Me.txtAlmacen.TeclasDeshacer = True
            Me.txtAlmacen.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picListaPrecios
            '
            Me.picListaPrecios.AccessibleDescription = Nothing
            Me.picListaPrecios.AccessibleName = Nothing
            resources.ApplyResources(Me.picListaPrecios, "picListaPrecios")
            Me.picListaPrecios.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.picListaPrecios, resources.GetString("picListaPrecios.Error"))
            Me.picListaPrecios.Font = Nothing
            Me.epConfRepXMarca.SetIconAlignment(Me.picListaPrecios, CType(resources.GetObject("picListaPrecios.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.picListaPrecios, CType(resources.GetObject("picListaPrecios.IconPadding"), Integer))
            Me.picListaPrecios.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.picListaPrecios.ImageLocation = Nothing
            Me.picListaPrecios.Name = "picListaPrecios"
            Me.picListaPrecios.TabStop = False
            '
            'txtListaPrecios
            '
            Me.txtListaPrecios.AccessibleDescription = Nothing
            Me.txtListaPrecios.AccessibleName = Nothing
            Me.txtListaPrecios.AceptaNegativos = False
            resources.ApplyResources(Me.txtListaPrecios, "txtListaPrecios")
            Me.txtListaPrecios.BackColor = System.Drawing.Color.White
            Me.txtListaPrecios.BackgroundImage = Nothing
            Me.epConfRepXMarca.SetError(Me.txtListaPrecios, resources.GetString("txtListaPrecios.Error"))
            Me.txtListaPrecios.EstiloSBO = True
            Me.epConfRepXMarca.SetIconAlignment(Me.txtListaPrecios, CType(resources.GetObject("txtListaPrecios.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.txtListaPrecios, CType(resources.GetObject("txtListaPrecios.IconPadding"), Integer))
            Me.txtListaPrecios.MaxDecimales = 0
            Me.txtListaPrecios.MaxEnteros = 0
            Me.txtListaPrecios.Millares = False
            Me.txtListaPrecios.Name = "txtListaPrecios"
            Me.txtListaPrecios.Size_AdjustableHeight = 20
            Me.txtListaPrecios.TeclasDeshacer = True
            Me.txtListaPrecios.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblAlmacen
            '
            Me.lblAlmacen.AccessibleDescription = Nothing
            Me.lblAlmacen.AccessibleName = Nothing
            resources.ApplyResources(Me.lblAlmacen, "lblAlmacen")
            Me.epConfRepXMarca.SetError(Me.lblAlmacen, resources.GetString("lblAlmacen.Error"))
            Me.lblAlmacen.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblAlmacen, CType(resources.GetObject("lblAlmacen.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblAlmacen, CType(resources.GetObject("lblAlmacen.IconPadding"), Integer))
            Me.lblAlmacen.Name = "lblAlmacen"
            '
            'lblListaPrecios
            '
            Me.lblListaPrecios.AccessibleDescription = Nothing
            Me.lblListaPrecios.AccessibleName = Nothing
            resources.ApplyResources(Me.lblListaPrecios, "lblListaPrecios")
            Me.epConfRepXMarca.SetError(Me.lblListaPrecios, resources.GetString("lblListaPrecios.Error"))
            Me.lblListaPrecios.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.epConfRepXMarca.SetIconAlignment(Me.lblListaPrecios, CType(resources.GetObject("lblListaPrecios.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.lblListaPrecios, CType(resources.GetObject("lblListaPrecios.IconPadding"), Integer))
            Me.lblListaPrecios.Name = "lblListaPrecios"
            '
            'btnCancelar
            '
            Me.btnCancelar.AccessibleDescription = Nothing
            Me.btnCancelar.AccessibleName = Nothing
            resources.ApplyResources(Me.btnCancelar, "btnCancelar")
            Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.epConfRepXMarca.SetError(Me.btnCancelar, resources.GetString("btnCancelar.Error"))
            Me.btnCancelar.ForeColor = System.Drawing.Color.Black
            Me.epConfRepXMarca.SetIconAlignment(Me.btnCancelar, CType(resources.GetObject("btnCancelar.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.btnCancelar, CType(resources.GetObject("btnCancelar.IconPadding"), Integer))
            Me.btnCancelar.Name = "btnCancelar"
            '
            'epConfRepXMarca
            '
            Me.epConfRepXMarca.BlinkRate = 1000
            Me.epConfRepXMarca.ContainerControl = Me
            resources.ApplyResources(Me.epConfRepXMarca, "epConfRepXMarca")
            '
            'btnAceptar
            '
            Me.btnAceptar.AccessibleDescription = Nothing
            Me.btnAceptar.AccessibleName = Nothing
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.epConfRepXMarca.SetError(Me.btnAceptar, resources.GetString("btnAceptar.Error"))
            Me.btnAceptar.ForeColor = System.Drawing.Color.Black
            Me.epConfRepXMarca.SetIconAlignment(Me.btnAceptar, CType(resources.GetObject("btnAceptar.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.epConfRepXMarca.SetIconPadding(Me.btnAceptar, CType(resources.GetObject("btnAceptar.IconPadding"), Integer))
            Me.btnAceptar.Name = "btnAceptar"
            '
            'frmConfCatalogoRepxMarca
            '
            Me.AccessibleDescription = Nothing
            Me.AccessibleName = Nothing
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.BackgroundImage = Nothing
            Me.CancelButton = Me.btnCancelar
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.grpDetallesCatalogo)
            Me.Controls.Add(Me.btnCancelar)
            Me.Controls.Add(Me.grpCompañia)
            Me.Controls.Add(Me.grpSeguridad)
            Me.Controls.Add(Me.grpMarca)
            Me.Font = Nothing
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmConfCatalogoRepxMarca"
            Me.grpMarca.ResumeLayout(False)
            Me.grpMarca.PerformLayout()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ProveedorXMarca, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpSeguridad.ResumeLayout(False)
            Me.grpSeguridad.PerformLayout()
            Me.grpCompañia.ResumeLayout(False)
            Me.grpCompañia.PerformLayout()
            CType(Me.picCompañia, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpDetallesCatalogo.ResumeLayout(False)
            Me.grpDetallesCatalogo.PerformLayout()
            CType(Me.picAlmacen, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picListaPrecios, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.epConfRepXMarca, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents grpMarca As System.Windows.Forms.GroupBox
        Friend WithEvents grpSeguridad As System.Windows.Forms.GroupBox
        Friend WithEvents grpCompañia As System.Windows.Forms.GroupBox
        Friend WithEvents btnCancelar As System.Windows.Forms.Button
        Friend WithEvents lblMarca As System.Windows.Forms.Label
        Friend WithEvents cboMarcas As SCGComboBox.SCGComboBox
        Friend WithEvents lblServidor As System.Windows.Forms.Label
        Friend WithEvents lblUsuario As System.Windows.Forms.Label
        Friend WithEvents lblContraseña As System.Windows.Forms.Label
        Friend WithEvents lblCompania As System.Windows.Forms.Label
        Friend WithEvents grpDetallesCatalogo As System.Windows.Forms.GroupBox
        Friend WithEvents lblAlmacen As System.Windows.Forms.Label
        Friend WithEvents lblListaPrecios As System.Windows.Forms.Label
        Friend WithEvents cboCompañia As SCGComboBox.SCGComboBox
        Friend WithEvents picListaPrecios As System.Windows.Forms.PictureBox
        Friend WithEvents txtListaPrecios As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtPasswordServidor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtUsuarioServidor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtServidor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picAlmacen As System.Windows.Forms.PictureBox
        Friend WithEvents txtAlmacen As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picCompañia As System.Windows.Forms.PictureBox
        Friend WithEvents txtPasswordSBO As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtUsuarioSBO As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents epConfRepXMarca As System.Windows.Forms.ErrorProvider
        Public WithEvents Label3 As System.Windows.Forms.Label
        Public WithEvents Label6 As System.Windows.Forms.Label
        Public WithEvents Label5 As System.Windows.Forms.Label
        Public WithEvents Label4 As System.Windows.Forms.Label
        Public WithEvents Label7 As System.Windows.Forms.Label
        Public WithEvents Label15 As System.Windows.Forms.Label
        Public WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents ProveedorXMarca As DMSOneFramework.ProveedorXMarcaDataset
        Friend WithEvents btnAgregarAct As System.Windows.Forms.Button
        Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
        Friend WithEvents IDCatalogoRepxMarcaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardCodeProveedorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CardNameProveedorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Public WithEvents Label9 As System.Windows.Forms.Label
        Public WithEvents Label8 As System.Windows.Forms.Label
    End Class

End Namespace