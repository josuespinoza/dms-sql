Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmDetalleCita
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
            Me.components = New System.ComponentModel.Container()
            Dim dstItems As DMSOneFramework.QUT1Dataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDetalleCita))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.dtpHoraCita = New System.Windows.Forms.DateTimePicker()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.lblHora = New System.Windows.Forms.Label()
            Me.dtpFechaCita = New System.Windows.Forms.DateTimePicker()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.lblFecha = New System.Windows.Forms.Label()
            Me.stlbCita = New Proyecto_SCGToolBar.SCGToolBar()
            Me.grpPaquetes = New System.Windows.Forms.GroupBox()
            Me.btnAgregarAct = New System.Windows.Forms.Button()
            Me.btnEliminarAct = New System.Windows.Forms.Button()
            Me.dtgDetalles = New System.Windows.Forms.DataGridView()
            Me.Check = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.Codigo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Servicio = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Moneda = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Observaciones = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.LineNum = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.U_TipoArticulo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.bcItems = New System.Windows.Forms.BindingSource(Me.components)
            Me.grpVehiculo = New System.Windows.Forms.GroupBox()
            Me.txtCombustible = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtMotor = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label15 = New System.Windows.Forms.Label()
            Me.lblCombustible = New System.Windows.Forms.Label()
            Me.Label17 = New System.Windows.Forms.Label()
            Me.lblMotor = New System.Windows.Forms.Label()
            Me.txtEstilo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtModelo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picConfVehiculo = New System.Windows.Forms.PictureBox()
            Me.picConfCliente = New System.Windows.Forms.PictureBox()
            Me.txtAnoVehiculo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtCodCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.lblEstilo = New System.Windows.Forms.Label()
            Me.txtNoUnidad = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.picClientes = New System.Windows.Forms.PictureBox()
            Me.picVehiculos = New System.Windows.Forms.PictureBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.txtNombreCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine8 = New System.Windows.Forms.Label()
            Me.lblLine3 = New System.Windows.Forms.Label()
            Me.lblLine2 = New System.Windows.Forms.Label()
            Me.lblLine1 = New System.Windows.Forms.Label()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.lblAnoVehiculo = New System.Windows.Forms.Label()
            Me.lblNoPlaca = New System.Windows.Forms.Label()
            Me.lblNumeroVehículo = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.lblAgenda = New System.Windows.Forms.Label()
            Me.grpCita = New System.Windows.Forms.GroupBox()
            Me.txtNoCita = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtTecnico = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picTecnico = New System.Windows.Forms.PictureBox()
            Me.lblTecnico = New System.Windows.Forms.Label()
            Me.txtCreador = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.txtAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.picAsesor = New System.Windows.Forms.PictureBox()
            Me.lblLineaNoCita = New System.Windows.Forms.Label()
            Me.lblNoCitaTitulo = New System.Windows.Forms.Label()
            Me.cboAgenda = New SCGComboBox.SCGComboBox()
            Me.txtObservaciones = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.cboRazonesCita = New SCGComboBox.SCGComboBox()
            Me.btnAgendaCitas = New System.Windows.Forms.Button()
            Me.lblCreador = New System.Windows.Forms.Label()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.lblAsesor = New System.Windows.Forms.Label()
            Me.lblObservacion = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.lblRazon = New System.Windows.Forms.Label()
            Me.EPCitas = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.VisualizarUDFCita = New ControlUDF.VisualizarUDF()
            dstItems = New DMSOneFramework.QUT1Dataset()
            CType(dstItems, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpPaquetes.SuspendLayout()
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.bcItems, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpVehiculo.SuspendLayout()
            CType(Me.picConfVehiculo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picConfCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picClientes, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picVehiculos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCita.SuspendLayout()
            CType(Me.picTecnico, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.picAsesor, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EPCitas, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dstItems
            '
            dstItems.DataSetName = "QUT1Dataset"
            dstItems.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'dtpHoraCita
            '
            resources.ApplyResources(Me.dtpHoraCita, "dtpHoraCita")
            Me.dtpHoraCita.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHoraCita.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHoraCita.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHoraCita.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHoraCita.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.EPCitas.SetIconAlignment(Me.dtpHoraCita, CType(resources.GetObject("dtpHoraCita.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.dtpHoraCita.Name = "dtpHoraCita"
            Me.dtpHoraCita.ShowUpDown = True
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label8, CType(resources.GetObject("Label8.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'lblHora
            '
            resources.ApplyResources(Me.lblHora, "lblHora")
            Me.lblHora.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblHora, CType(resources.GetObject("lblHora.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblHora.Name = "lblHora"
            '
            'dtpFechaCita
            '
            resources.ApplyResources(Me.dtpFechaCita, "dtpFechaCita")
            Me.dtpFechaCita.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCita.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaCita.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaCita.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCita.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCita.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.dtpFechaCita.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.EPCitas.SetIconAlignment(Me.dtpFechaCita, CType(resources.GetObject("dtpFechaCita.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.dtpFechaCita.Name = "dtpFechaCita"
            Me.dtpFechaCita.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label3, CType(resources.GetObject("Label3.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'lblFecha
            '
            resources.ApplyResources(Me.lblFecha, "lblFecha")
            Me.lblFecha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblFecha, CType(resources.GetObject("lblFecha.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblFecha.Name = "lblFecha"
            '
            'stlbCita
            '
            resources.ApplyResources(Me.stlbCita, "stlbCita")
            Me.stlbCita.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.stlbCita.Name = "stlbCita"
            '
            'grpPaquetes
            '
            Me.grpPaquetes.BackColor = System.Drawing.SystemColors.Control
            Me.grpPaquetes.Controls.Add(Me.btnAgregarAct)
            Me.grpPaquetes.Controls.Add(Me.btnEliminarAct)
            Me.grpPaquetes.Controls.Add(Me.dtgDetalles)
            resources.ApplyResources(Me.grpPaquetes, "grpPaquetes")
            Me.grpPaquetes.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.grpPaquetes, CType(resources.GetObject("grpPaquetes.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.grpPaquetes.Name = "grpPaquetes"
            Me.grpPaquetes.TabStop = False
            '
            'btnAgregarAct
            '
            resources.ApplyResources(Me.btnAgregarAct, "btnAgregarAct")
            Me.btnAgregarAct.ForeColor = System.Drawing.Color.Maroon
            Me.EPCitas.SetIconAlignment(Me.btnAgregarAct, CType(resources.GetObject("btnAgregarAct.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.btnAgregarAct.Name = "btnAgregarAct"
            '
            'btnEliminarAct
            '
            resources.ApplyResources(Me.btnEliminarAct, "btnEliminarAct")
            Me.btnEliminarAct.ForeColor = System.Drawing.Color.Maroon
            Me.EPCitas.SetIconAlignment(Me.btnEliminarAct, CType(resources.GetObject("btnEliminarAct.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.btnEliminarAct.Name = "btnEliminarAct"
            '
            'dtgDetalles
            '
            Me.dtgDetalles.AllowUserToAddRows = False
            Me.dtgDetalles.AllowUserToDeleteRows = False
            Me.dtgDetalles.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgDetalles.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            Me.dtgDetalles.AutoGenerateColumns = False
            Me.dtgDetalles.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgDetalles.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
            Me.dtgDetalles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Check, Me.Codigo, Me.Servicio, Me.Cantidad, Me.Moneda, Me.Precio, Me.Observaciones, Me.LineNum, Me.U_TipoArticulo})
            Me.dtgDetalles.DataSource = Me.bcItems
            Me.EPCitas.SetIconAlignment(Me.dtgDetalles, CType(resources.GetObject("dtgDetalles.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.dtgDetalles, "dtgDetalles")
            Me.dtgDetalles.MultiSelect = False
            Me.dtgDetalles.Name = "dtgDetalles"
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
            DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgDetalles.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
            DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(49, Byte), Integer))
            DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgDetalles.RowsDefaultCellStyle = DataGridViewCellStyle5
            Me.dtgDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            '
            'Check
            '
            Me.Check.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.Check.Name = "Check"
            Me.Check.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.Check.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            resources.ApplyResources(Me.Check, "Check")
            '
            'Codigo
            '
            Me.Codigo.DataPropertyName = "itemCode"
            resources.ApplyResources(Me.Codigo, "Codigo")
            Me.Codigo.Name = "Codigo"
            Me.Codigo.ReadOnly = True
            '
            'Servicio
            '
            Me.Servicio.DataPropertyName = "itemName"
            resources.ApplyResources(Me.Servicio, "Servicio")
            Me.Servicio.Name = "Servicio"
            Me.Servicio.ReadOnly = True
            '
            'Cantidad
            '
            Me.Cantidad.DataPropertyName = "Quantity"
            resources.ApplyResources(Me.Cantidad, "Cantidad")
            Me.Cantidad.Name = "Cantidad"
            '
            'Moneda
            '
            Me.Moneda.DataPropertyName = "Moneda"
            resources.ApplyResources(Me.Moneda, "Moneda")
            Me.Moneda.Name = "Moneda"
            Me.Moneda.ReadOnly = True
            '
            'Precio
            '
            Me.Precio.DataPropertyName = "Precio"
            DataGridViewCellStyle3.Format = "N2"
            DataGridViewCellStyle3.NullValue = Nothing
            Me.Precio.DefaultCellStyle = DataGridViewCellStyle3
            resources.ApplyResources(Me.Precio, "Precio")
            Me.Precio.Name = "Precio"
            Me.Precio.ReadOnly = True
            '
            'Observaciones
            '
            Me.Observaciones.DataPropertyName = "FreeTxt"
            resources.ApplyResources(Me.Observaciones, "Observaciones")
            Me.Observaciones.Name = "Observaciones"
            '
            'LineNum
            '
            Me.LineNum.DataPropertyName = "LineNum"
            resources.ApplyResources(Me.LineNum, "LineNum")
            Me.LineNum.Name = "LineNum"
            Me.LineNum.ReadOnly = True
            '
            'U_TipoArticulo
            '
            Me.U_TipoArticulo.DataPropertyName = "U_TipoArticulo"
            resources.ApplyResources(Me.U_TipoArticulo, "U_TipoArticulo")
            Me.U_TipoArticulo.Name = "U_TipoArticulo"
            Me.U_TipoArticulo.ReadOnly = True
            '
            'bcItems
            '
            Me.bcItems.DataSource = dstItems
            Me.bcItems.Position = 0
            '
            'grpVehiculo
            '
            Me.grpVehiculo.Controls.Add(Me.txtCombustible)
            Me.grpVehiculo.Controls.Add(Me.txtMotor)
            Me.grpVehiculo.Controls.Add(Me.Label15)
            Me.grpVehiculo.Controls.Add(Me.lblCombustible)
            Me.grpVehiculo.Controls.Add(Me.Label17)
            Me.grpVehiculo.Controls.Add(Me.lblMotor)
            Me.grpVehiculo.Controls.Add(Me.txtEstilo)
            Me.grpVehiculo.Controls.Add(Me.txtModelo)
            Me.grpVehiculo.Controls.Add(Me.txtMarca)
            Me.grpVehiculo.Controls.Add(Me.picConfVehiculo)
            Me.grpVehiculo.Controls.Add(Me.picConfCliente)
            Me.grpVehiculo.Controls.Add(Me.txtAnoVehiculo)
            Me.grpVehiculo.Controls.Add(Me.txtCodCliente)
            Me.grpVehiculo.Controls.Add(Me.Label13)
            Me.grpVehiculo.Controls.Add(Me.lblEstilo)
            Me.grpVehiculo.Controls.Add(Me.txtNoUnidad)
            Me.grpVehiculo.Controls.Add(Me.Label6)
            Me.grpVehiculo.Controls.Add(Me.picClientes)
            Me.grpVehiculo.Controls.Add(Me.picVehiculos)
            Me.grpVehiculo.Controls.Add(Me.Label2)
            Me.grpVehiculo.Controls.Add(Me.Label1)
            Me.grpVehiculo.Controls.Add(Me.txtNombreCliente)
            Me.grpVehiculo.Controls.Add(Me.txtPlaca)
            Me.grpVehiculo.Controls.Add(Me.lblLine8)
            Me.grpVehiculo.Controls.Add(Me.lblLine3)
            Me.grpVehiculo.Controls.Add(Me.lblLine2)
            Me.grpVehiculo.Controls.Add(Me.lblLine1)
            Me.grpVehiculo.Controls.Add(Me.Label10)
            Me.grpVehiculo.Controls.Add(Me.Label11)
            Me.grpVehiculo.Controls.Add(Me.Label12)
            Me.grpVehiculo.Controls.Add(Me.lblAnoVehiculo)
            Me.grpVehiculo.Controls.Add(Me.lblNoPlaca)
            Me.grpVehiculo.Controls.Add(Me.lblNumeroVehículo)
            resources.ApplyResources(Me.grpVehiculo, "grpVehiculo")
            Me.grpVehiculo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.grpVehiculo, CType(resources.GetObject("grpVehiculo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.grpVehiculo.Name = "grpVehiculo"
            Me.grpVehiculo.TabStop = False
            '
            'txtCombustible
            '
            Me.txtCombustible.AceptaNegativos = False
            Me.txtCombustible.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCombustible.EstiloSBO = True
            resources.ApplyResources(Me.txtCombustible, "txtCombustible")
            Me.EPCitas.SetIconAlignment(Me.txtCombustible, CType(resources.GetObject("txtCombustible.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtCombustible.MaxDecimales = 0
            Me.txtCombustible.MaxEnteros = 0
            Me.txtCombustible.Millares = False
            Me.txtCombustible.Name = "txtCombustible"
            Me.txtCombustible.ReadOnly = True
            Me.txtCombustible.Size_AdjustableHeight = 20
            Me.txtCombustible.TeclasDeshacer = True
            Me.txtCombustible.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtMotor
            '
            Me.txtMotor.AceptaNegativos = False
            Me.txtMotor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtMotor.EstiloSBO = True
            resources.ApplyResources(Me.txtMotor, "txtMotor")
            Me.EPCitas.SetIconAlignment(Me.txtMotor, CType(resources.GetObject("txtMotor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtMotor.MaxDecimales = 0
            Me.txtMotor.MaxEnteros = 0
            Me.txtMotor.Millares = False
            Me.txtMotor.Name = "txtMotor"
            Me.txtMotor.ReadOnly = True
            Me.txtMotor.Size_AdjustableHeight = 20
            Me.txtMotor.TeclasDeshacer = True
            Me.txtMotor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label15
            '
            Me.Label15.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label15, CType(resources.GetObject("Label15.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label15, "Label15")
            Me.Label15.Name = "Label15"
            '
            'lblCombustible
            '
            resources.ApplyResources(Me.lblCombustible, "lblCombustible")
            Me.lblCombustible.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblCombustible, CType(resources.GetObject("lblCombustible.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblCombustible.Name = "lblCombustible"
            '
            'Label17
            '
            Me.Label17.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label17, CType(resources.GetObject("Label17.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label17, "Label17")
            Me.Label17.Name = "Label17"
            '
            'lblMotor
            '
            resources.ApplyResources(Me.lblMotor, "lblMotor")
            Me.lblMotor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblMotor, CType(resources.GetObject("lblMotor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblMotor.Name = "lblMotor"
            '
            'txtEstilo
            '
            Me.txtEstilo.AceptaNegativos = False
            Me.txtEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEstilo.EstiloSBO = True
            resources.ApplyResources(Me.txtEstilo, "txtEstilo")
            Me.EPCitas.SetIconAlignment(Me.txtEstilo, CType(resources.GetObject("txtEstilo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtEstilo.MaxDecimales = 0
            Me.txtEstilo.MaxEnteros = 0
            Me.txtEstilo.Millares = False
            Me.txtEstilo.Name = "txtEstilo"
            Me.txtEstilo.ReadOnly = True
            Me.txtEstilo.Size_AdjustableHeight = 20
            Me.txtEstilo.TeclasDeshacer = True
            Me.txtEstilo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtModelo
            '
            Me.txtModelo.AceptaNegativos = False
            Me.txtModelo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtModelo.EstiloSBO = True
            resources.ApplyResources(Me.txtModelo, "txtModelo")
            Me.EPCitas.SetIconAlignment(Me.txtModelo, CType(resources.GetObject("txtModelo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtModelo.MaxDecimales = 0
            Me.txtModelo.MaxEnteros = 0
            Me.txtModelo.Millares = False
            Me.txtModelo.Name = "txtModelo"
            Me.txtModelo.ReadOnly = True
            Me.txtModelo.Size_AdjustableHeight = 20
            Me.txtModelo.TeclasDeshacer = True
            Me.txtModelo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtMarca
            '
            Me.txtMarca.AceptaNegativos = False
            Me.txtMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtMarca, "txtMarca")
            Me.EPCitas.SetIconAlignment(Me.txtMarca, CType(resources.GetObject("txtMarca.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtMarca.MaxDecimales = 0
            Me.txtMarca.MaxEnteros = 0
            Me.txtMarca.Millares = False
            Me.txtMarca.Name = "txtMarca"
            Me.txtMarca.ReadOnly = True
            Me.txtMarca.Size_AdjustableHeight = 20
            Me.txtMarca.TeclasDeshacer = True
            Me.txtMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picConfVehiculo
            '
            Me.EPCitas.SetIconAlignment(Me.picConfVehiculo, CType(resources.GetObject("picConfVehiculo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.picConfVehiculo, "picConfVehiculo")
            Me.picConfVehiculo.Name = "picConfVehiculo"
            Me.picConfVehiculo.TabStop = False
            '
            'picConfCliente
            '
            Me.EPCitas.SetIconAlignment(Me.picConfCliente, CType(resources.GetObject("picConfCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.picConfCliente, "picConfCliente")
            Me.picConfCliente.Name = "picConfCliente"
            Me.picConfCliente.TabStop = False
            '
            'txtAnoVehiculo
            '
            Me.txtAnoVehiculo.AceptaNegativos = False
            Me.txtAnoVehiculo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAnoVehiculo.EstiloSBO = True
            resources.ApplyResources(Me.txtAnoVehiculo, "txtAnoVehiculo")
            Me.EPCitas.SetIconAlignment(Me.txtAnoVehiculo, CType(resources.GetObject("txtAnoVehiculo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtAnoVehiculo.MaxDecimales = 0
            Me.txtAnoVehiculo.MaxEnteros = 0
            Me.txtAnoVehiculo.Millares = False
            Me.txtAnoVehiculo.Name = "txtAnoVehiculo"
            Me.txtAnoVehiculo.ReadOnly = True
            Me.txtAnoVehiculo.Size_AdjustableHeight = 20
            Me.txtAnoVehiculo.TeclasDeshacer = True
            Me.txtAnoVehiculo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtCodCliente
            '
            Me.txtCodCliente.AceptaNegativos = False
            Me.txtCodCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCodCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtCodCliente, "txtCodCliente")
            Me.EPCitas.SetIconAlignment(Me.txtCodCliente, CType(resources.GetObject("txtCodCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtCodCliente.MaxDecimales = 0
            Me.txtCodCliente.MaxEnteros = 0
            Me.txtCodCliente.Millares = False
            Me.txtCodCliente.Name = "txtCodCliente"
            Me.txtCodCliente.ReadOnly = True
            Me.txtCodCliente.Size_AdjustableHeight = 20
            Me.txtCodCliente.TeclasDeshacer = True
            Me.txtCodCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label13
            '
            Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label13, CType(resources.GetObject("Label13.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label13, "Label13")
            Me.Label13.Name = "Label13"
            '
            'lblEstilo
            '
            resources.ApplyResources(Me.lblEstilo, "lblEstilo")
            Me.lblEstilo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblEstilo, CType(resources.GetObject("lblEstilo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblEstilo.Name = "lblEstilo"
            '
            'txtNoUnidad
            '
            Me.txtNoUnidad.AceptaNegativos = False
            Me.txtNoUnidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoUnidad.EstiloSBO = True
            resources.ApplyResources(Me.txtNoUnidad, "txtNoUnidad")
            Me.EPCitas.SetIconAlignment(Me.txtNoUnidad, CType(resources.GetObject("txtNoUnidad.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtNoUnidad.MaxDecimales = 0
            Me.txtNoUnidad.MaxEnteros = 0
            Me.txtNoUnidad.Millares = False
            Me.txtNoUnidad.Name = "txtNoUnidad"
            Me.txtNoUnidad.ReadOnly = True
            Me.txtNoUnidad.Size_AdjustableHeight = 20
            Me.txtNoUnidad.TeclasDeshacer = True
            Me.txtNoUnidad.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label6
            '
            Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label6, CType(resources.GetObject("Label6.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'picClientes
            '
            Me.EPCitas.SetIconAlignment(Me.picClientes, CType(resources.GetObject("picClientes.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.picClientes.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picClientes, "picClientes")
            Me.picClientes.Name = "picClientes"
            Me.picClientes.TabStop = False
            '
            'picVehiculos
            '
            Me.EPCitas.SetIconAlignment(Me.picVehiculos, CType(resources.GetObject("picVehiculos.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.picVehiculos.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picVehiculos, "picVehiculos")
            Me.picVehiculos.Name = "picVehiculos"
            Me.picVehiculos.TabStop = False
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label2, CType(resources.GetObject("Label2.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'txtNombreCliente
            '
            Me.txtNombreCliente.AceptaNegativos = False
            Me.txtNombreCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNombreCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtNombreCliente, "txtNombreCliente")
            Me.EPCitas.SetIconAlignment(Me.txtNombreCliente, CType(resources.GetObject("txtNombreCliente.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtNombreCliente.MaxDecimales = 0
            Me.txtNombreCliente.MaxEnteros = 0
            Me.txtNombreCliente.Millares = False
            Me.txtNombreCliente.Name = "txtNombreCliente"
            Me.txtNombreCliente.ReadOnly = True
            Me.txtNombreCliente.Size_AdjustableHeight = 20
            Me.txtNombreCliente.TeclasDeshacer = True
            Me.txtNombreCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.EPCitas.SetIconAlignment(Me.txtPlaca, CType(resources.GetObject("txtPlaca.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.ReadOnly = True
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine8
            '
            Me.lblLine8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblLine8, CType(resources.GetObject("lblLine8.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.lblLine8, "lblLine8")
            Me.lblLine8.Name = "lblLine8"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblLine3, CType(resources.GetObject("lblLine3.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblLine2, CType(resources.GetObject("lblLine2.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblLine1, CType(resources.GetObject("lblLine1.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'Label10
            '
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label10, CType(resources.GetObject("Label10.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.Label10.Name = "Label10"
            '
            'Label11
            '
            resources.ApplyResources(Me.Label11, "Label11")
            Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label11, CType(resources.GetObject("Label11.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.Label11.Name = "Label11"
            '
            'Label12
            '
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label12, CType(resources.GetObject("Label12.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.Label12.Name = "Label12"
            '
            'lblAnoVehiculo
            '
            resources.ApplyResources(Me.lblAnoVehiculo, "lblAnoVehiculo")
            Me.lblAnoVehiculo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblAnoVehiculo, CType(resources.GetObject("lblAnoVehiculo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblAnoVehiculo.Name = "lblAnoVehiculo"
            '
            'lblNoPlaca
            '
            resources.ApplyResources(Me.lblNoPlaca, "lblNoPlaca")
            Me.lblNoPlaca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblNoPlaca, CType(resources.GetObject("lblNoPlaca.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblNoPlaca.Name = "lblNoPlaca"
            '
            'lblNumeroVehículo
            '
            resources.ApplyResources(Me.lblNumeroVehículo, "lblNumeroVehículo")
            Me.lblNumeroVehículo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblNumeroVehículo, CType(resources.GetObject("lblNumeroVehículo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblNumeroVehículo.Name = "lblNumeroVehículo"
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label4, CType(resources.GetObject("Label4.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'lblAgenda
            '
            resources.ApplyResources(Me.lblAgenda, "lblAgenda")
            Me.lblAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblAgenda, CType(resources.GetObject("lblAgenda.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblAgenda.Name = "lblAgenda"
            '
            'grpCita
            '
            Me.grpCita.Controls.Add(Me.txtNoCita)
            Me.grpCita.Controls.Add(Me.txtTecnico)
            Me.grpCita.Controls.Add(Me.picTecnico)
            Me.grpCita.Controls.Add(Me.lblTecnico)
            Me.grpCita.Controls.Add(Me.txtCreador)
            Me.grpCita.Controls.Add(Me.Label9)
            Me.grpCita.Controls.Add(Me.txtAsesor)
            Me.grpCita.Controls.Add(Me.picAsesor)
            Me.grpCita.Controls.Add(Me.lblLineaNoCita)
            Me.grpCita.Controls.Add(Me.lblNoCitaTitulo)
            Me.grpCita.Controls.Add(Me.cboAgenda)
            Me.grpCita.Controls.Add(Me.txtObservaciones)
            Me.grpCita.Controls.Add(Me.Label7)
            Me.grpCita.Controls.Add(Me.cboRazonesCita)
            Me.grpCita.Controls.Add(Me.btnAgendaCitas)
            Me.grpCita.Controls.Add(Me.dtpFechaCita)
            Me.grpCita.Controls.Add(Me.dtpHoraCita)
            Me.grpCita.Controls.Add(Me.Label8)
            Me.grpCita.Controls.Add(Me.lblHora)
            Me.grpCita.Controls.Add(Me.lblCreador)
            Me.grpCita.Controls.Add(Me.Label14)
            Me.grpCita.Controls.Add(Me.lblAsesor)
            Me.grpCita.Controls.Add(Me.lblObservacion)
            Me.grpCita.Controls.Add(Me.Label5)
            Me.grpCita.Controls.Add(Me.lblRazon)
            Me.grpCita.Controls.Add(Me.Label3)
            Me.grpCita.Controls.Add(Me.Label4)
            Me.grpCita.Controls.Add(Me.lblAgenda)
            Me.grpCita.Controls.Add(Me.lblFecha)
            resources.ApplyResources(Me.grpCita, "grpCita")
            Me.grpCita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.grpCita, CType(resources.GetObject("grpCita.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.grpCita.Name = "grpCita"
            Me.grpCita.TabStop = False
            '
            'txtNoCita
            '
            Me.txtNoCita.AceptaNegativos = False
            Me.txtNoCita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoCita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoCita, "txtNoCita")
            Me.EPCitas.SetIconAlignment(Me.txtNoCita, CType(resources.GetObject("txtNoCita.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtNoCita.MaxDecimales = 0
            Me.txtNoCita.MaxEnteros = 0
            Me.txtNoCita.Millares = False
            Me.txtNoCita.Name = "txtNoCita"
            Me.txtNoCita.ReadOnly = True
            Me.txtNoCita.Size_AdjustableHeight = 20
            Me.txtNoCita.TeclasDeshacer = True
            Me.txtNoCita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtTecnico
            '
            Me.txtTecnico.AceptaNegativos = False
            Me.txtTecnico.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTecnico.EstiloSBO = True
            resources.ApplyResources(Me.txtTecnico, "txtTecnico")
            Me.EPCitas.SetIconAlignment(Me.txtTecnico, CType(resources.GetObject("txtTecnico.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtTecnico.MaxDecimales = 0
            Me.txtTecnico.MaxEnteros = 0
            Me.txtTecnico.Millares = False
            Me.txtTecnico.Name = "txtTecnico"
            Me.txtTecnico.ReadOnly = True
            Me.txtTecnico.Size_AdjustableHeight = 20
            Me.txtTecnico.TeclasDeshacer = True
            Me.txtTecnico.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picTecnico
            '
            Me.EPCitas.SetIconAlignment(Me.picTecnico, CType(resources.GetObject("picTecnico.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.picTecnico.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picTecnico, "picTecnico")
            Me.picTecnico.Name = "picTecnico"
            Me.picTecnico.TabStop = False
            '
            'lblTecnico
            '
            resources.ApplyResources(Me.lblTecnico, "lblTecnico")
            Me.lblTecnico.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblTecnico, CType(resources.GetObject("lblTecnico.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblTecnico.Name = "lblTecnico"
            '
            'txtCreador
            '
            Me.txtCreador.AceptaNegativos = False
            Me.txtCreador.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCreador.EstiloSBO = True
            resources.ApplyResources(Me.txtCreador, "txtCreador")
            Me.EPCitas.SetIconAlignment(Me.txtCreador, CType(resources.GetObject("txtCreador.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtCreador.MaxDecimales = 0
            Me.txtCreador.MaxEnteros = 0
            Me.txtCreador.Millares = False
            Me.txtCreador.Name = "txtCreador"
            Me.txtCreador.ReadOnly = True
            Me.txtCreador.Size_AdjustableHeight = 20
            Me.txtCreador.TeclasDeshacer = True
            Me.txtCreador.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label9, CType(resources.GetObject("Label9.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'txtAsesor
            '
            Me.txtAsesor.AceptaNegativos = False
            Me.txtAsesor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAsesor.EstiloSBO = True
            resources.ApplyResources(Me.txtAsesor, "txtAsesor")
            Me.EPCitas.SetIconAlignment(Me.txtAsesor, CType(resources.GetObject("txtAsesor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtAsesor.MaxDecimales = 0
            Me.txtAsesor.MaxEnteros = 0
            Me.txtAsesor.Millares = False
            Me.txtAsesor.Name = "txtAsesor"
            Me.txtAsesor.ReadOnly = True
            Me.txtAsesor.Size_AdjustableHeight = 20
            Me.txtAsesor.TeclasDeshacer = True
            Me.txtAsesor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'picAsesor
            '
            Me.EPCitas.SetIconAlignment(Me.picAsesor, CType(resources.GetObject("picAsesor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.picAsesor.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picAsesor, "picAsesor")
            Me.picAsesor.Name = "picAsesor"
            Me.picAsesor.TabStop = False
            '
            'lblLineaNoCita
            '
            Me.lblLineaNoCita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblLineaNoCita, CType(resources.GetObject("lblLineaNoCita.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.lblLineaNoCita, "lblLineaNoCita")
            Me.lblLineaNoCita.Name = "lblLineaNoCita"
            '
            'lblNoCitaTitulo
            '
            resources.ApplyResources(Me.lblNoCitaTitulo, "lblNoCitaTitulo")
            Me.lblNoCitaTitulo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblNoCitaTitulo, CType(resources.GetObject("lblNoCitaTitulo.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblNoCitaTitulo.Name = "lblNoCitaTitulo"
            '
            'cboAgenda
            '
            Me.cboAgenda.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboAgenda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAgenda.EstiloSBO = True
            resources.ApplyResources(Me.cboAgenda, "cboAgenda")
            Me.EPCitas.SetIconAlignment(Me.cboAgenda, CType(resources.GetObject("cboAgenda.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.cboAgenda.Name = "cboAgenda"
            '
            'txtObservaciones
            '
            Me.txtObservaciones.AceptaNegativos = False
            Me.txtObservaciones.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtObservaciones.EstiloSBO = True
            resources.ApplyResources(Me.txtObservaciones, "txtObservaciones")
            Me.EPCitas.SetIconAlignment(Me.txtObservaciones, CType(resources.GetObject("txtObservaciones.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.txtObservaciones.MaxDecimales = 0
            Me.txtObservaciones.MaxEnteros = 0
            Me.txtObservaciones.Millares = False
            Me.txtObservaciones.Name = "txtObservaciones"
            Me.txtObservaciones.Size_AdjustableHeight = 55
            Me.txtObservaciones.TeclasDeshacer = True
            Me.txtObservaciones.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'cboRazonesCita
            '
            Me.cboRazonesCita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboRazonesCita.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboRazonesCita.EstiloSBO = True
            resources.ApplyResources(Me.cboRazonesCita, "cboRazonesCita")
            Me.EPCitas.SetIconAlignment(Me.cboRazonesCita, CType(resources.GetObject("cboRazonesCita.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.cboRazonesCita.Name = "cboRazonesCita"
            '
            'btnAgendaCitas
            '
            Me.btnAgendaCitas.BackgroundImage = Global.SCG_User_Interface.My.Resources.Resources.calendario2
            resources.ApplyResources(Me.btnAgendaCitas, "btnAgendaCitas")
            Me.btnAgendaCitas.ForeColor = System.Drawing.Color.Black
            Me.EPCitas.SetIconAlignment(Me.btnAgendaCitas, CType(resources.GetObject("btnAgendaCitas.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.btnAgendaCitas.Image = Global.SCG_User_Interface.My.Resources.Resources.calendario2
            Me.btnAgendaCitas.Name = "btnAgendaCitas"
            '
            'lblCreador
            '
            resources.ApplyResources(Me.lblCreador, "lblCreador")
            Me.lblCreador.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblCreador, CType(resources.GetObject("lblCreador.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblCreador.Name = "lblCreador"
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label14, CType(resources.GetObject("Label14.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.Name = "Label14"
            '
            'lblAsesor
            '
            resources.ApplyResources(Me.lblAsesor, "lblAsesor")
            Me.lblAsesor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblAsesor, CType(resources.GetObject("lblAsesor.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblAsesor.Name = "lblAsesor"
            '
            'lblObservacion
            '
            resources.ApplyResources(Me.lblObservacion, "lblObservacion")
            Me.lblObservacion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblObservacion, CType(resources.GetObject("lblObservacion.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblObservacion.Name = "lblObservacion"
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.Label5, CType(resources.GetObject("Label5.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'lblRazon
            '
            resources.ApplyResources(Me.lblRazon, "lblRazon")
            Me.lblRazon.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.EPCitas.SetIconAlignment(Me.lblRazon, CType(resources.GetObject("lblRazon.IconAlignment"), System.Windows.Forms.ErrorIconAlignment))
            Me.lblRazon.Name = "lblRazon"
            '
            'EPCitas
            '
            Me.EPCitas.ContainerControl = Me
            '
            'VisualizarUDFCita
            '
            resources.ApplyResources(Me.VisualizarUDFCita, "VisualizarUDFCita")
            Me.VisualizarUDFCita.CampoLlave = Nothing
            Me.VisualizarUDFCita.CodigoFormularioSBO = 0
            Me.VisualizarUDFCita.CodigoUsuario = 0
            Me.VisualizarUDFCita.Conexion = Nothing
            Me.VisualizarUDFCita.Form = Nothing
            Me.VisualizarUDFCita.Name = "VisualizarUDFCita"
            Me.VisualizarUDFCita.NombreBaseDatosSBO = Nothing
            Me.VisualizarUDFCita.Tabla = Nothing
            Me.VisualizarUDFCita.VisualizarUDFSBO = False
            Me.VisualizarUDFCita.Where = Nothing
            '
            'frmDetalleCita
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.VisualizarUDFCita)
            Me.Controls.Add(Me.grpCita)
            Me.Controls.Add(Me.grpVehiculo)
            Me.Controls.Add(Me.grpPaquetes)
            Me.Controls.Add(Me.stlbCita)
            Me.MaximizeBox = False
            Me.Name = "frmDetalleCita"
            CType(dstItems, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpPaquetes.ResumeLayout(False)
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.bcItems, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpVehiculo.ResumeLayout(False)
            Me.grpVehiculo.PerformLayout()
            CType(Me.picConfVehiculo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picConfCliente, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picClientes, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picVehiculos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpCita.ResumeLayout(False)
            Me.grpCita.PerformLayout()
            CType(Me.picTecnico, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.picAsesor, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EPCitas, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        'Friend WithEvents SubBuscador1 As Buscador.SubBuscador
        Friend WithEvents dtpHoraCita As System.Windows.Forms.DateTimePicker
        Public WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents lblHora As System.Windows.Forms.Label
        Friend WithEvents dtpFechaCita As System.Windows.Forms.DateTimePicker
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents lblFecha As System.Windows.Forms.Label
        Friend WithEvents stlbCita As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents grpPaquetes As System.Windows.Forms.GroupBox
        Friend WithEvents dtgDetalles As System.Windows.Forms.DataGridView
        Friend WithEvents grpVehiculo As System.Windows.Forms.GroupBox
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents picClientes As System.Windows.Forms.PictureBox
        Friend WithEvents txtNombreCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtAnoVehiculo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCodCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine8 As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents lblAnoVehiculo As System.Windows.Forms.Label
        Friend WithEvents lblNoPlaca As System.Windows.Forms.Label
        Friend WithEvents picVehiculos As System.Windows.Forms.PictureBox
        Public WithEvents Label2 As System.Windows.Forms.Label
        Public WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents lblAgenda As System.Windows.Forms.Label
        Friend WithEvents grpCita As System.Windows.Forms.GroupBox
        Public WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents lblEstilo As System.Windows.Forms.Label
        Friend WithEvents txtNoUnidad As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents lblNumeroVehículo As System.Windows.Forms.Label
        Friend WithEvents picConfVehiculo As System.Windows.Forms.PictureBox
        Friend WithEvents picConfCliente As System.Windows.Forms.PictureBox
        Friend WithEvents txtEstilo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtModelo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents btnAgendaCitas As System.Windows.Forms.Button
        Friend WithEvents cboRazonesCita As SCGComboBox.SCGComboBox
        Public WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents lblRazon As System.Windows.Forms.Label
        Friend WithEvents txtObservaciones As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents lblObservacion As System.Windows.Forms.Label
        Friend WithEvents btnAgregarAct As System.Windows.Forms.Button
        Friend WithEvents btnEliminarAct As System.Windows.Forms.Button
        Friend WithEvents bcItems As System.Windows.Forms.BindingSource
        Friend WithEvents cboAgenda As SCGComboBox.SCGComboBox
        Public WithEvents lblLineaNoCita As System.Windows.Forms.Label
        Friend WithEvents lblNoCitaTitulo As System.Windows.Forms.Label
        Friend WithEvents EPCitas As System.Windows.Forms.ErrorProvider
        Friend WithEvents txtNoCita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtCreador As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents lblCreador As System.Windows.Forms.Label
        Friend WithEvents txtAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picAsesor As System.Windows.Forms.PictureBox
        Public WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents lblAsesor As System.Windows.Forms.Label
        Friend WithEvents txtCombustible As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtMotor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label15 As System.Windows.Forms.Label
        Friend WithEvents lblCombustible As System.Windows.Forms.Label
        Public WithEvents Label17 As System.Windows.Forms.Label
        Friend WithEvents lblMotor As System.Windows.Forms.Label
        Friend WithEvents txtTecnico As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents picTecnico As System.Windows.Forms.PictureBox
        Friend WithEvents lblTecnico As System.Windows.Forms.Label
        Friend WithEvents VisualizarUDFCita As ControlUDF.VisualizarUDF
        Friend WithEvents Check As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents Codigo As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Servicio As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Moneda As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Observaciones As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents LineNum As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents U_TipoArticulo As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace