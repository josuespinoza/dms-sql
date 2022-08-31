Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmDetalleOrdenesEspeciales
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalleOrdenesEspeciales))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.cboTipoOrden = New SCGComboBox.SCGComboBox()
            Me.lblLine3 = New System.Windows.Forms.Label()
            Me.lblTipoOrden = New System.Windows.Forms.Label()
            Me.btnCrear = New System.Windows.Forms.Button()
            Me.btnCerrar = New System.Windows.Forms.Button()
            Me.dtgTiposConfigurados = New System.Windows.Forms.DataGridView()
            Me.m_dtsOQUT1 = New DMSOneFramework.QUT1Dataset()
            Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.ItemCodeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ItemNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.QuantityDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Moneda = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PrecioDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.FreeTxtDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.UTipoArticuloDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.LineNumDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.TrasladadoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            CType(Me.dtgTiposConfigurados, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.m_dtsOQUT1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cboTipoOrden
            '
            Me.cboTipoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboTipoOrden.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTipoOrden.EstiloSBO = True
            resources.ApplyResources(Me.cboTipoOrden, "cboTipoOrden")
            Me.cboTipoOrden.Name = "cboTipoOrden"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblLine3.Name = "lblLine3"
            '
            'lblTipoOrden
            '
            resources.ApplyResources(Me.lblTipoOrden, "lblTipoOrden")
            Me.lblTipoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipoOrden.Name = "lblTipoOrden"
            '
            'btnCrear
            '
            resources.ApplyResources(Me.btnCrear, "btnCrear")
            Me.btnCrear.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCrear.Name = "btnCrear"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.Name = "btnCerrar"
            '
            'dtgTiposConfigurados
            '
            Me.dtgTiposConfigurados.AllowUserToAddRows = False
            Me.dtgTiposConfigurados.AllowUserToDeleteRows = False
            Me.dtgTiposConfigurados.AutoGenerateColumns = False
            Me.dtgTiposConfigurados.BackgroundColor = System.Drawing.SystemColors.Control
            Me.dtgTiposConfigurados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgTiposConfigurados.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Check, Me.ItemCodeDataGridViewTextBoxColumn, Me.ItemNameDataGridViewTextBoxColumn, Me.QuantityDataGridViewTextBoxColumn, Me.Moneda, Me.PrecioDataGridViewTextBoxColumn, Me.FreeTxtDataGridViewTextBoxColumn, Me.UTipoArticuloDataGridViewTextBoxColumn, Me.LineNumDataGridViewTextBoxColumn, Me.TrasladadoDataGridViewTextBoxColumn})
            Me.dtgTiposConfigurados.DataMember = "QUT1"
            Me.dtgTiposConfigurados.DataSource = Me.m_dtsOQUT1
            resources.ApplyResources(Me.dtgTiposConfigurados, "dtgTiposConfigurados")
            Me.dtgTiposConfigurados.Name = "dtgTiposConfigurados"
            '
            'm_dtsOQUT1
            '
            Me.m_dtsOQUT1.DataSetName = "QUT1Dataset"
            Me.m_dtsOQUT1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'Check
            '
            Me.Check.DataPropertyName = "Check"
            Me.Check.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.Check.Name = "Check"
            resources.ApplyResources(Me.Check, "Check")
            '
            'ItemCodeDataGridViewTextBoxColumn
            '
            Me.ItemCodeDataGridViewTextBoxColumn.DataPropertyName = "itemCode"
            resources.ApplyResources(Me.ItemCodeDataGridViewTextBoxColumn, "ItemCodeDataGridViewTextBoxColumn")
            Me.ItemCodeDataGridViewTextBoxColumn.Name = "ItemCodeDataGridViewTextBoxColumn"
            Me.ItemCodeDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemNameDataGridViewTextBoxColumn
            '
            Me.ItemNameDataGridViewTextBoxColumn.DataPropertyName = "itemName"
            resources.ApplyResources(Me.ItemNameDataGridViewTextBoxColumn, "ItemNameDataGridViewTextBoxColumn")
            Me.ItemNameDataGridViewTextBoxColumn.Name = "ItemNameDataGridViewTextBoxColumn"
            Me.ItemNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'QuantityDataGridViewTextBoxColumn
            '
            Me.QuantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity"
            resources.ApplyResources(Me.QuantityDataGridViewTextBoxColumn, "QuantityDataGridViewTextBoxColumn")
            Me.QuantityDataGridViewTextBoxColumn.Name = "QuantityDataGridViewTextBoxColumn"
            Me.QuantityDataGridViewTextBoxColumn.ReadOnly = True
            '
            'Moneda
            '
            Me.Moneda.DataPropertyName = "Moneda"
            resources.ApplyResources(Me.Moneda, "Moneda")
            Me.Moneda.Name = "Moneda"
            Me.Moneda.ReadOnly = True
            '
            'PrecioDataGridViewTextBoxColumn
            '
            Me.PrecioDataGridViewTextBoxColumn.DataPropertyName = "Precio"
            DataGridViewCellStyle1.Format = "N2"
            DataGridViewCellStyle1.NullValue = Nothing
            Me.PrecioDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle1
            resources.ApplyResources(Me.PrecioDataGridViewTextBoxColumn, "PrecioDataGridViewTextBoxColumn")
            Me.PrecioDataGridViewTextBoxColumn.Name = "PrecioDataGridViewTextBoxColumn"
            Me.PrecioDataGridViewTextBoxColumn.ReadOnly = True
            '
            'FreeTxtDataGridViewTextBoxColumn
            '
            Me.FreeTxtDataGridViewTextBoxColumn.DataPropertyName = "FreeTxt"
            resources.ApplyResources(Me.FreeTxtDataGridViewTextBoxColumn, "FreeTxtDataGridViewTextBoxColumn")
            Me.FreeTxtDataGridViewTextBoxColumn.Name = "FreeTxtDataGridViewTextBoxColumn"
            Me.FreeTxtDataGridViewTextBoxColumn.ReadOnly = True
            '
            'UTipoArticuloDataGridViewTextBoxColumn
            '
            Me.UTipoArticuloDataGridViewTextBoxColumn.DataPropertyName = "U_TipoArticulo"
            resources.ApplyResources(Me.UTipoArticuloDataGridViewTextBoxColumn, "UTipoArticuloDataGridViewTextBoxColumn")
            Me.UTipoArticuloDataGridViewTextBoxColumn.Name = "UTipoArticuloDataGridViewTextBoxColumn"
            '
            'LineNumDataGridViewTextBoxColumn
            '
            Me.LineNumDataGridViewTextBoxColumn.DataPropertyName = "LineNum"
            resources.ApplyResources(Me.LineNumDataGridViewTextBoxColumn, "LineNumDataGridViewTextBoxColumn")
            Me.LineNumDataGridViewTextBoxColumn.Name = "LineNumDataGridViewTextBoxColumn"
            '
            'TrasladadoDataGridViewTextBoxColumn
            '
            Me.TrasladadoDataGridViewTextBoxColumn.DataPropertyName = "Trasladado"
            resources.ApplyResources(Me.TrasladadoDataGridViewTextBoxColumn, "TrasladadoDataGridViewTextBoxColumn")
            Me.TrasladadoDataGridViewTextBoxColumn.Name = "TrasladadoDataGridViewTextBoxColumn"
            '
            'frmDetalleOrdenesEspeciales
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.btnCrear)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.dtgTiposConfigurados)
            Me.Controls.Add(Me.cboTipoOrden)
            Me.Controls.Add(Me.lblLine3)
            Me.Controls.Add(Me.lblTipoOrden)
            Me.MaximizeBox = False
            Me.Name = "frmDetalleOrdenesEspeciales"
            CType(Me.dtgTiposConfigurados, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.m_dtsOQUT1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cboTipoOrden As SCGComboBox.SCGComboBox
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents lblTipoOrden As System.Windows.Forms.Label
        Friend WithEvents dtgTiposConfigurados As System.Windows.Forms.DataGridView
        Friend WithEvents btnCrear As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents m_dtsOQUT1 As DMSOneFramework.QUT1Dataset
        Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents ItemCodeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents QuantityDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Moneda As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PrecioDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FreeTxtDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents UTipoArticuloDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNumDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TrasladadoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace