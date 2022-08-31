Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmFactVtaNocturna
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFactVtaNocturna))
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.lblMensajes = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.txtFecha = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.txtNoFactura = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label4 = New System.Windows.Forms.Label
            Me.Label5 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.picFacturas = New System.Windows.Forms.PictureBox
            Me.SubBFacturas = New Buscador.SubBuscador
            Me.SubBEmpleados = New Buscador.SubBuscador
            Me.dtgActividades = New System.Windows.Forms.DataGridView
            Me.LineNumDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ItemCodeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ItemNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.EmpIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.ColaboradorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Buscar = New System.Windows.Forms.DataGridViewButtonColumn
            Me.dstFacturasEspeciales = New DMSOneFramework.FacturasEspecialesDataset
            Me.GroupBox1.SuspendLayout()
            CType(Me.picFacturas, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dstFacturasEspeciales, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.ForeColor = System.Drawing.Color.Black
            Me.btnCerrar.Name = "btnCerrar"
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.ForeColor = System.Drawing.Color.Black
            Me.btnAceptar.Name = "btnAceptar"
            '
            'lblMensajes
            '
            resources.ApplyResources(Me.lblMensajes, "lblMensajes")
            Me.lblMensajes.ForeColor = System.Drawing.Color.Black
            Me.lblMensajes.Name = "lblMensajes"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.txtFecha)
            Me.GroupBox1.Controls.Add(Me.txtCliente)
            Me.GroupBox1.Controls.Add(Me.txtNoFactura)
            Me.GroupBox1.Controls.Add(Me.Label4)
            Me.GroupBox1.Controls.Add(Me.Label5)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Controls.Add(Me.picFacturas)
            Me.GroupBox1.Controls.Add(Me.Label1)
            Me.GroupBox1.Controls.Add(Me.lblMensajes)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'txtFecha
            '
            Me.txtFecha.AceptaNegativos = False
            Me.txtFecha.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFecha.EstiloSBO = True
            resources.ApplyResources(Me.txtFecha, "txtFecha")
            Me.txtFecha.MaxDecimales = 0
            Me.txtFecha.MaxEnteros = 0
            Me.txtFecha.Millares = False
            Me.txtFecha.Name = "txtFecha"
            Me.txtFecha.ReadOnly = True
            Me.txtFecha.Size_AdjustableHeight = 20
            Me.txtFecha.TeclasDeshacer = True
            Me.txtFecha.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCliente
            '
            Me.txtCliente.AceptaNegativos = False
            Me.txtCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtCliente, "txtCliente")
            Me.txtCliente.MaxDecimales = 0
            Me.txtCliente.MaxEnteros = 0
            Me.txtCliente.Millares = False
            Me.txtCliente.Name = "txtCliente"
            Me.txtCliente.ReadOnly = True
            Me.txtCliente.Size_AdjustableHeight = 20
            Me.txtCliente.TeclasDeshacer = True
            Me.txtCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoFactura
            '
            Me.txtNoFactura.AceptaNegativos = False
            Me.txtNoFactura.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoFactura.EstiloSBO = True
            resources.ApplyResources(Me.txtNoFactura, "txtNoFactura")
            Me.txtNoFactura.MaxDecimales = 0
            Me.txtNoFactura.MaxEnteros = 0
            Me.txtNoFactura.Millares = False
            Me.txtNoFactura.Name = "txtNoFactura"
            Me.txtNoFactura.ReadOnly = True
            Me.txtNoFactura.Size_AdjustableHeight = 20
            Me.txtNoFactura.Tag = "0"
            Me.txtNoFactura.TeclasDeshacer = True
            Me.txtNoFactura.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'Label5
            '
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.ForeColor = System.Drawing.Color.Black
            Me.Label5.Name = "Label5"
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.Black
            Me.Label3.Name = "Label3"
            '
            'picFacturas
            '
            Me.picFacturas.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picFacturas, "picFacturas")
            Me.picFacturas.Name = "picFacturas"
            Me.picFacturas.TabStop = False
            '
            'SubBFacturas
            '
            Me.SubBFacturas.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.SubBFacturas.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBFacturas.ConsultarDBPorFiltrado = False
            Me.SubBFacturas.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBFacturas.Criterios_Ocultos = 0
            Me.SubBFacturas.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBFacturas.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBFacturas, "SubBFacturas")
            Me.SubBFacturas.MultiSeleccion = False
            Me.SubBFacturas.Name = "SubBFacturas"
            Me.SubBFacturas.SQL_Cnn = Nothing
            Me.SubBFacturas.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBFacturas.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBFacturas.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'SubBEmpleados
            '
            Me.SubBEmpleados.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.SubBEmpleados.Barra_Titulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBEmpleados.ConsultarDBPorFiltrado = False
            Me.SubBEmpleados.Criterios = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBEmpleados.Criterios_Ocultos = 0
            Me.SubBEmpleados.Criterios_OcultosEx = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBEmpleados.IN_DataTable = Nothing
            resources.ApplyResources(Me.SubBEmpleados, "SubBEmpleados")
            Me.SubBEmpleados.MultiSeleccion = False
            Me.SubBEmpleados.Name = "SubBEmpleados"
            Me.SubBEmpleados.SQL_Cnn = Nothing
            Me.SubBEmpleados.Tabla = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBEmpleados.Titulos = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.SubBEmpleados.Where = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'dtgActividades
            '
            Me.dtgActividades.AllowUserToAddRows = False
            Me.dtgActividades.AllowUserToDeleteRows = False
            Me.dtgActividades.AutoGenerateColumns = False
            Me.dtgActividades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader
            Me.dtgActividades.BackgroundColor = System.Drawing.Color.White
            Me.dtgActividades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgActividades.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.LineNumDataGridViewTextBoxColumn, Me.ItemCodeDataGridViewTextBoxColumn, Me.ItemNameDataGridViewTextBoxColumn, Me.EmpIDDataGridViewTextBoxColumn, Me.ColaboradorDataGridViewTextBoxColumn, Me.Buscar})
            Me.dtgActividades.DataMember = "FacturasEspecialesDataTable"
            Me.dtgActividades.DataSource = Me.dstFacturasEspeciales
            Me.dtgActividades.GridColor = System.Drawing.Color.Silver
            resources.ApplyResources(Me.dtgActividades, "dtgActividades")
            Me.dtgActividades.Name = "dtgActividades"
            Me.dtgActividades.RowHeadersVisible = False
            Me.dtgActividades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            '
            'LineNumDataGridViewTextBoxColumn
            '
            Me.LineNumDataGridViewTextBoxColumn.DataPropertyName = "LineNum"
            Me.LineNumDataGridViewTextBoxColumn.FillWeight = 22.0!
            Me.LineNumDataGridViewTextBoxColumn.Frozen = True
            resources.ApplyResources(Me.LineNumDataGridViewTextBoxColumn, "LineNumDataGridViewTextBoxColumn")
            Me.LineNumDataGridViewTextBoxColumn.Name = "LineNumDataGridViewTextBoxColumn"
            Me.LineNumDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemCodeDataGridViewTextBoxColumn
            '
            Me.ItemCodeDataGridViewTextBoxColumn.DataPropertyName = "ItemCode"
            resources.ApplyResources(Me.ItemCodeDataGridViewTextBoxColumn, "ItemCodeDataGridViewTextBoxColumn")
            Me.ItemCodeDataGridViewTextBoxColumn.Name = "ItemCodeDataGridViewTextBoxColumn"
            Me.ItemCodeDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemNameDataGridViewTextBoxColumn
            '
            Me.ItemNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.ItemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName"
            Me.ItemNameDataGridViewTextBoxColumn.FillWeight = 2.0!
            resources.ApplyResources(Me.ItemNameDataGridViewTextBoxColumn, "ItemNameDataGridViewTextBoxColumn")
            Me.ItemNameDataGridViewTextBoxColumn.Name = "ItemNameDataGridViewTextBoxColumn"
            Me.ItemNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'EmpIDDataGridViewTextBoxColumn
            '
            Me.EmpIDDataGridViewTextBoxColumn.DataPropertyName = "EmpID"
            Me.EmpIDDataGridViewTextBoxColumn.FillWeight = 225.0!
            resources.ApplyResources(Me.EmpIDDataGridViewTextBoxColumn, "EmpIDDataGridViewTextBoxColumn")
            Me.EmpIDDataGridViewTextBoxColumn.Name = "EmpIDDataGridViewTextBoxColumn"
            Me.EmpIDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ColaboradorDataGridViewTextBoxColumn
            '
            Me.ColaboradorDataGridViewTextBoxColumn.DataPropertyName = "Colaborador"
            Me.ColaboradorDataGridViewTextBoxColumn.FillWeight = 225.0!
            resources.ApplyResources(Me.ColaboradorDataGridViewTextBoxColumn, "ColaboradorDataGridViewTextBoxColumn")
            Me.ColaboradorDataGridViewTextBoxColumn.Name = "ColaboradorDataGridViewTextBoxColumn"
            Me.ColaboradorDataGridViewTextBoxColumn.ReadOnly = True
            '
            'Buscar
            '
            Me.Buscar.FillWeight = 75.0!
            Me.Buscar.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.Buscar, "Buscar")
            Me.Buscar.Name = "Buscar"
            Me.Buscar.Text = "Buscar"
            Me.Buscar.UseColumnTextForButtonValue = True
            '
            'dstFacturasEspeciales
            '
            Me.dstFacturasEspeciales.DataSetName = "dstFacturasEspeciales"
            Me.dstFacturasEspeciales.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'frmFactVtaNocturna
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.SubBEmpleados)
            Me.Controls.Add(Me.SubBFacturas)
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.dtgActividades)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmFactVtaNocturna"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            CType(Me.picFacturas, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgActividades, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dstFacturasEspeciales, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents dtgActividades As System.Windows.Forms.DataGridView
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents lblMensajes As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents txtNoFactura As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picFacturas As System.Windows.Forms.PictureBox
        Friend WithEvents SubBFacturas As Buscador.SubBuscador
        Friend WithEvents txtFecha As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents txtCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents dstFacturasEspeciales As DMSOneFramework.FacturasEspecialesDataset
        Friend WithEvents SubBEmpleados As Buscador.SubBuscador
        Friend WithEvents LineNumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemCodeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents EmpIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColaboradorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Buscar As System.Windows.Forms.DataGridViewButtonColumn
    End Class

End Namespace
