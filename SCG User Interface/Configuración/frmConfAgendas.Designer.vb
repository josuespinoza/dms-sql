Namespace SCG_User_Interface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmConfAgendas
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfAgendas))
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
            Me.txtAgenda = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblAgenda = New System.Windows.Forms.Label
            Me.tlbAgendas = New Proyecto_SCGToolBar.SCGToolBar
            Me.dtgAgendas = New System.Windows.Forms.DataGridView
            Me.EstadoLogicoDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AgendaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.IntervaloCitas = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Abreviatura = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionAsesor = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.Tecnico = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionRazonCita = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DescripcionArticulo = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn
            Me.AgendaDataTableBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblIntervaloCitas = New System.Windows.Forms.Label
            Me.txtAbreviatura = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblAbreviatura = New System.Windows.Forms.Label
            Me.nudIntervalo = New System.Windows.Forms.NumericUpDown
            Me.PictureBoxRazon = New System.Windows.Forms.PictureBox
            Me.txtRazonCita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.LabelRazonAgenda = New System.Windows.Forms.Label
            Me.PictureBoxArticuloCita = New System.Windows.Forms.PictureBox
            Me.txtArticuloAgenda = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.LabelArticuloCita = New System.Windows.Forms.Label
            Me.PictureBoxTecnico = New System.Windows.Forms.PictureBox
            Me.txtTecnicoAgenda = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.LabelTecnicoAgenda = New System.Windows.Forms.Label
            Me.PictureBoxAsesor = New System.Windows.Forms.PictureBox
            Me.txtAsesorAgenda = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.LabelAsesorAgenda = New System.Windows.Forms.Label
            CType(Me.dtgAgendas, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AgendaDataTableBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudIntervalo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBoxRazon, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBoxArticuloCita, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBoxTecnico, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PictureBoxAsesor, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'txtAgenda
            '
            Me.txtAgenda.AccessibleDescription = Nothing
            Me.txtAgenda.AccessibleName = Nothing
            Me.txtAgenda.AceptaNegativos = False
            resources.ApplyResources(Me.txtAgenda, "txtAgenda")
            Me.txtAgenda.BackColor = System.Drawing.Color.White
            Me.txtAgenda.BackgroundImage = Nothing
            Me.txtAgenda.EstiloSBO = True
            Me.txtAgenda.MaxDecimales = 0
            Me.txtAgenda.MaxEnteros = 0
            Me.txtAgenda.Millares = False
            Me.txtAgenda.Name = "txtAgenda"
            Me.txtAgenda.Size_AdjustableHeight = 20
            Me.txtAgenda.TeclasDeshacer = True
            Me.txtAgenda.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
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
            'lblAgenda
            '
            Me.lblAgenda.AccessibleDescription = Nothing
            Me.lblAgenda.AccessibleName = Nothing
            resources.ApplyResources(Me.lblAgenda, "lblAgenda")
            Me.lblAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAgenda.Name = "lblAgenda"
            '
            'tlbAgendas
            '
            Me.tlbAgendas.AccessibleDescription = Nothing
            Me.tlbAgendas.AccessibleName = Nothing
            resources.ApplyResources(Me.tlbAgendas, "tlbAgendas")
            Me.tlbAgendas.BackgroundImage = Nothing
            Me.tlbAgendas.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.tlbAgendas.Font = Nothing
            Me.tlbAgendas.Name = "tlbAgendas"
            '
            'dtgAgendas
            '
            Me.dtgAgendas.AccessibleDescription = Nothing
            Me.dtgAgendas.AccessibleName = Nothing
            Me.dtgAgendas.AllowUserToAddRows = False
            Me.dtgAgendas.AllowUserToDeleteRows = False
            DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(44, Byte), Integer))
            DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgAgendas.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle5
            resources.ApplyResources(Me.dtgAgendas, "dtgAgendas")
            Me.dtgAgendas.AutoGenerateColumns = False
            Me.dtgAgendas.BackgroundColor = System.Drawing.Color.White
            Me.dtgAgendas.BackgroundImage = Nothing
            Me.dtgAgendas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
            DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgAgendas.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
            Me.dtgAgendas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgAgendas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.EstadoLogicoDataGridViewCheckBoxColumn, Me.IDDataGridViewTextBoxColumn, Me.AgendaDataGridViewTextBoxColumn, Me.IntervaloCitas, Me.Abreviatura, Me.DescripcionAsesor, Me.Tecnico, Me.DescripcionRazonCita, Me.DescripcionArticulo, Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4})
            Me.dtgAgendas.DataSource = Me.AgendaDataTableBindingSource
            Me.dtgAgendas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2
            Me.dtgAgendas.EnableHeadersVisualStyles = False
            Me.dtgAgendas.Font = Nothing
            Me.dtgAgendas.GridColor = System.Drawing.Color.Silver
            Me.dtgAgendas.MultiSelect = False
            Me.dtgAgendas.Name = "dtgAgendas"
            DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dtgAgendas.RowHeadersDefaultCellStyle = DataGridViewCellStyle7
            DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
            DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(44, Byte), Integer))
            DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgAgendas.RowsDefaultCellStyle = DataGridViewCellStyle8
            Me.dtgAgendas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            '
            'EstadoLogicoDataGridViewCheckBoxColumn
            '
            Me.EstadoLogicoDataGridViewCheckBoxColumn.DataPropertyName = "EstadoLogico"
            Me.EstadoLogicoDataGridViewCheckBoxColumn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.EstadoLogicoDataGridViewCheckBoxColumn.Frozen = True
            resources.ApplyResources(Me.EstadoLogicoDataGridViewCheckBoxColumn, "EstadoLogicoDataGridViewCheckBoxColumn")
            Me.EstadoLogicoDataGridViewCheckBoxColumn.Name = "EstadoLogicoDataGridViewCheckBoxColumn"
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'AgendaDataGridViewTextBoxColumn
            '
            Me.AgendaDataGridViewTextBoxColumn.DataPropertyName = "Agenda"
            resources.ApplyResources(Me.AgendaDataGridViewTextBoxColumn, "AgendaDataGridViewTextBoxColumn")
            Me.AgendaDataGridViewTextBoxColumn.Name = "AgendaDataGridViewTextBoxColumn"
            Me.AgendaDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IntervaloCitas
            '
            Me.IntervaloCitas.DataPropertyName = "IntervaloCitas"
            resources.ApplyResources(Me.IntervaloCitas, "IntervaloCitas")
            Me.IntervaloCitas.Name = "IntervaloCitas"
            Me.IntervaloCitas.ReadOnly = True
            '
            'Abreviatura
            '
            Me.Abreviatura.DataPropertyName = "Abreviatura"
            resources.ApplyResources(Me.Abreviatura, "Abreviatura")
            Me.Abreviatura.Name = "Abreviatura"
            Me.Abreviatura.ReadOnly = True
            '
            'DescripcionAsesor
            '
            Me.DescripcionAsesor.DataPropertyName = "CodAsesor"
            resources.ApplyResources(Me.DescripcionAsesor, "DescripcionAsesor")
            Me.DescripcionAsesor.Name = "DescripcionAsesor"
            Me.DescripcionAsesor.ReadOnly = True
            '
            'Tecnico
            '
            Me.Tecnico.DataPropertyName = "CodTecnico"
            resources.ApplyResources(Me.Tecnico, "Tecnico")
            Me.Tecnico.Name = "Tecnico"
            Me.Tecnico.ReadOnly = True
            '
            'DescripcionRazonCita
            '
            Me.DescripcionRazonCita.DataPropertyName = "RazonCita"
            resources.ApplyResources(Me.DescripcionRazonCita, "DescripcionRazonCita")
            Me.DescripcionRazonCita.Name = "DescripcionRazonCita"
            Me.DescripcionRazonCita.ReadOnly = True
            '
            'DescripcionArticulo
            '
            Me.DescripcionArticulo.DataPropertyName = "ArticuloCita"
            resources.ApplyResources(Me.DescripcionArticulo, "DescripcionArticulo")
            Me.DescripcionArticulo.Name = "DescripcionArticulo"
            Me.DescripcionArticulo.ReadOnly = True
            '
            'DataGridViewTextBoxColumn1
            '
            Me.DataGridViewTextBoxColumn1.DataPropertyName = "DescripcionAsesor"
            resources.ApplyResources(Me.DataGridViewTextBoxColumn1, "DataGridViewTextBoxColumn1")
            Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
            Me.DataGridViewTextBoxColumn1.ReadOnly = True
            '
            'DataGridViewTextBoxColumn2
            '
            Me.DataGridViewTextBoxColumn2.DataPropertyName = "DescripcionTecnico"
            resources.ApplyResources(Me.DataGridViewTextBoxColumn2, "DataGridViewTextBoxColumn2")
            Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
            Me.DataGridViewTextBoxColumn2.ReadOnly = True
            '
            'DataGridViewTextBoxColumn3
            '
            Me.DataGridViewTextBoxColumn3.DataPropertyName = "DescripcionRazonCita"
            resources.ApplyResources(Me.DataGridViewTextBoxColumn3, "DataGridViewTextBoxColumn3")
            Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
            Me.DataGridViewTextBoxColumn3.ReadOnly = True
            '
            'DataGridViewTextBoxColumn4
            '
            Me.DataGridViewTextBoxColumn4.DataPropertyName = "DescripcionArticulo"
            resources.ApplyResources(Me.DataGridViewTextBoxColumn4, "DataGridViewTextBoxColumn4")
            Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
            Me.DataGridViewTextBoxColumn4.ReadOnly = True
            '
            'AgendaDataTableBindingSource
            '
            Me.AgendaDataTableBindingSource.DataSource = GetType(DMSOneFramework.AgendaDataset.SCGTA_TB_AgendasDataTable)
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
            'lblIntervaloCitas
            '
            Me.lblIntervaloCitas.AccessibleDescription = Nothing
            Me.lblIntervaloCitas.AccessibleName = Nothing
            resources.ApplyResources(Me.lblIntervaloCitas, "lblIntervaloCitas")
            Me.lblIntervaloCitas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblIntervaloCitas.Name = "lblIntervaloCitas"
            '
            'txtAbreviatura
            '
            Me.txtAbreviatura.AccessibleDescription = Nothing
            Me.txtAbreviatura.AccessibleName = Nothing
            Me.txtAbreviatura.AceptaNegativos = False
            resources.ApplyResources(Me.txtAbreviatura, "txtAbreviatura")
            Me.txtAbreviatura.BackColor = System.Drawing.Color.White
            Me.txtAbreviatura.BackgroundImage = Nothing
            Me.txtAbreviatura.EstiloSBO = True
            Me.txtAbreviatura.MaxDecimales = 0
            Me.txtAbreviatura.MaxEnteros = 0
            Me.txtAbreviatura.Millares = True
            Me.txtAbreviatura.Name = "txtAbreviatura"
            Me.txtAbreviatura.Size_AdjustableHeight = 20
            Me.txtAbreviatura.TeclasDeshacer = True
            Me.txtAbreviatura.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
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
            'lblAbreviatura
            '
            Me.lblAbreviatura.AccessibleDescription = Nothing
            Me.lblAbreviatura.AccessibleName = Nothing
            resources.ApplyResources(Me.lblAbreviatura, "lblAbreviatura")
            Me.lblAbreviatura.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAbreviatura.Name = "lblAbreviatura"
            '
            'nudIntervalo
            '
            Me.nudIntervalo.AccessibleDescription = Nothing
            Me.nudIntervalo.AccessibleName = Nothing
            resources.ApplyResources(Me.nudIntervalo, "nudIntervalo")
            Me.nudIntervalo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.nudIntervalo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.nudIntervalo.Name = "nudIntervalo"
            '
            'PictureBoxRazon
            '
            Me.PictureBoxRazon.AccessibleDescription = Nothing
            Me.PictureBoxRazon.AccessibleName = Nothing
            resources.ApplyResources(Me.PictureBoxRazon, "PictureBoxRazon")
            Me.PictureBoxRazon.BackgroundImage = Nothing
            Me.PictureBoxRazon.Font = Nothing
            Me.PictureBoxRazon.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.PictureBoxRazon.ImageLocation = Nothing
            Me.PictureBoxRazon.Name = "PictureBoxRazon"
            Me.PictureBoxRazon.TabStop = False
            '
            'txtRazonCita
            '
            Me.txtRazonCita.AccessibleDescription = Nothing
            Me.txtRazonCita.AccessibleName = Nothing
            Me.txtRazonCita.AceptaNegativos = False
            resources.ApplyResources(Me.txtRazonCita, "txtRazonCita")
            Me.txtRazonCita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtRazonCita.BackgroundImage = Nothing
            Me.txtRazonCita.EstiloSBO = True
            Me.txtRazonCita.ForeColor = System.Drawing.Color.Black
            Me.txtRazonCita.MaxDecimales = 0
            Me.txtRazonCita.MaxEnteros = 0
            Me.txtRazonCita.Millares = False
            Me.txtRazonCita.Name = "txtRazonCita"
            Me.txtRazonCita.Size_AdjustableHeight = 20
            Me.txtRazonCita.TeclasDeshacer = True
            Me.txtRazonCita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'LabelRazonAgenda
            '
            Me.LabelRazonAgenda.AccessibleDescription = Nothing
            Me.LabelRazonAgenda.AccessibleName = Nothing
            resources.ApplyResources(Me.LabelRazonAgenda, "LabelRazonAgenda")
            Me.LabelRazonAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.LabelRazonAgenda.Name = "LabelRazonAgenda"
            '
            'PictureBoxArticuloCita
            '
            Me.PictureBoxArticuloCita.AccessibleDescription = Nothing
            Me.PictureBoxArticuloCita.AccessibleName = Nothing
            resources.ApplyResources(Me.PictureBoxArticuloCita, "PictureBoxArticuloCita")
            Me.PictureBoxArticuloCita.BackgroundImage = Nothing
            Me.PictureBoxArticuloCita.Font = Nothing
            Me.PictureBoxArticuloCita.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.PictureBoxArticuloCita.ImageLocation = Nothing
            Me.PictureBoxArticuloCita.Name = "PictureBoxArticuloCita"
            Me.PictureBoxArticuloCita.TabStop = False
            '
            'txtArticuloAgenda
            '
            Me.txtArticuloAgenda.AccessibleDescription = Nothing
            Me.txtArticuloAgenda.AccessibleName = Nothing
            Me.txtArticuloAgenda.AceptaNegativos = False
            resources.ApplyResources(Me.txtArticuloAgenda, "txtArticuloAgenda")
            Me.txtArticuloAgenda.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtArticuloAgenda.BackgroundImage = Nothing
            Me.txtArticuloAgenda.EstiloSBO = True
            Me.txtArticuloAgenda.ForeColor = System.Drawing.Color.Black
            Me.txtArticuloAgenda.MaxDecimales = 0
            Me.txtArticuloAgenda.MaxEnteros = 0
            Me.txtArticuloAgenda.Millares = False
            Me.txtArticuloAgenda.Name = "txtArticuloAgenda"
            Me.txtArticuloAgenda.Size_AdjustableHeight = 20
            Me.txtArticuloAgenda.TeclasDeshacer = True
            Me.txtArticuloAgenda.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'LabelArticuloCita
            '
            Me.LabelArticuloCita.AccessibleDescription = Nothing
            Me.LabelArticuloCita.AccessibleName = Nothing
            resources.ApplyResources(Me.LabelArticuloCita, "LabelArticuloCita")
            Me.LabelArticuloCita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.LabelArticuloCita.Name = "LabelArticuloCita"
            '
            'PictureBoxTecnico
            '
            Me.PictureBoxTecnico.AccessibleDescription = Nothing
            Me.PictureBoxTecnico.AccessibleName = Nothing
            resources.ApplyResources(Me.PictureBoxTecnico, "PictureBoxTecnico")
            Me.PictureBoxTecnico.BackgroundImage = Nothing
            Me.PictureBoxTecnico.Font = Nothing
            Me.PictureBoxTecnico.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.PictureBoxTecnico.ImageLocation = Nothing
            Me.PictureBoxTecnico.Name = "PictureBoxTecnico"
            Me.PictureBoxTecnico.TabStop = False
            '
            'txtTecnicoAgenda
            '
            Me.txtTecnicoAgenda.AccessibleDescription = Nothing
            Me.txtTecnicoAgenda.AccessibleName = Nothing
            Me.txtTecnicoAgenda.AceptaNegativos = False
            resources.ApplyResources(Me.txtTecnicoAgenda, "txtTecnicoAgenda")
            Me.txtTecnicoAgenda.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTecnicoAgenda.BackgroundImage = Nothing
            Me.txtTecnicoAgenda.EstiloSBO = True
            Me.txtTecnicoAgenda.ForeColor = System.Drawing.Color.Black
            Me.txtTecnicoAgenda.MaxDecimales = 0
            Me.txtTecnicoAgenda.MaxEnteros = 0
            Me.txtTecnicoAgenda.Millares = False
            Me.txtTecnicoAgenda.Name = "txtTecnicoAgenda"
            Me.txtTecnicoAgenda.Size_AdjustableHeight = 20
            Me.txtTecnicoAgenda.TeclasDeshacer = True
            Me.txtTecnicoAgenda.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'LabelTecnicoAgenda
            '
            Me.LabelTecnicoAgenda.AccessibleDescription = Nothing
            Me.LabelTecnicoAgenda.AccessibleName = Nothing
            resources.ApplyResources(Me.LabelTecnicoAgenda, "LabelTecnicoAgenda")
            Me.LabelTecnicoAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.LabelTecnicoAgenda.Name = "LabelTecnicoAgenda"
            '
            'PictureBoxAsesor
            '
            Me.PictureBoxAsesor.AccessibleDescription = Nothing
            Me.PictureBoxAsesor.AccessibleName = Nothing
            resources.ApplyResources(Me.PictureBoxAsesor, "PictureBoxAsesor")
            Me.PictureBoxAsesor.BackgroundImage = Nothing
            Me.PictureBoxAsesor.Font = Nothing
            Me.PictureBoxAsesor.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.PictureBoxAsesor.ImageLocation = Nothing
            Me.PictureBoxAsesor.Name = "PictureBoxAsesor"
            Me.PictureBoxAsesor.TabStop = False
            '
            'txtAsesorAgenda
            '
            Me.txtAsesorAgenda.AccessibleDescription = Nothing
            Me.txtAsesorAgenda.AccessibleName = Nothing
            Me.txtAsesorAgenda.AceptaNegativos = False
            resources.ApplyResources(Me.txtAsesorAgenda, "txtAsesorAgenda")
            Me.txtAsesorAgenda.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAsesorAgenda.BackgroundImage = Nothing
            Me.txtAsesorAgenda.EstiloSBO = True
            Me.txtAsesorAgenda.ForeColor = System.Drawing.Color.Black
            Me.txtAsesorAgenda.MaxDecimales = 0
            Me.txtAsesorAgenda.MaxEnteros = 0
            Me.txtAsesorAgenda.Millares = False
            Me.txtAsesorAgenda.Name = "txtAsesorAgenda"
            Me.txtAsesorAgenda.Size_AdjustableHeight = 20
            Me.txtAsesorAgenda.TeclasDeshacer = True
            Me.txtAsesorAgenda.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'LabelAsesorAgenda
            '
            Me.LabelAsesorAgenda.AccessibleDescription = Nothing
            Me.LabelAsesorAgenda.AccessibleName = Nothing
            resources.ApplyResources(Me.LabelAsesorAgenda, "LabelAsesorAgenda")
            Me.LabelAsesorAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.LabelAsesorAgenda.Name = "LabelAsesorAgenda"
            '
            'frmConfAgendas
            '
            Me.AccessibleDescription = Nothing
            Me.AccessibleName = Nothing
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.BackgroundImage = Nothing
            Me.Controls.Add(Me.lblAbreviatura)
            Me.Controls.Add(Me.PictureBoxAsesor)
            Me.Controls.Add(Me.txtAsesorAgenda)
            Me.Controls.Add(Me.LabelAsesorAgenda)
            Me.Controls.Add(Me.PictureBoxTecnico)
            Me.Controls.Add(Me.txtTecnicoAgenda)
            Me.Controls.Add(Me.LabelTecnicoAgenda)
            Me.Controls.Add(Me.PictureBoxArticuloCita)
            Me.Controls.Add(Me.txtArticuloAgenda)
            Me.Controls.Add(Me.LabelArticuloCita)
            Me.Controls.Add(Me.PictureBoxRazon)
            Me.Controls.Add(Me.txtRazonCita)
            Me.Controls.Add(Me.LabelRazonAgenda)
            Me.Controls.Add(Me.nudIntervalo)
            Me.Controls.Add(Me.txtAbreviatura)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblIntervaloCitas)
            Me.Controls.Add(Me.dtgAgendas)
            Me.Controls.Add(Me.txtAgenda)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblAgenda)
            Me.Controls.Add(Me.tlbAgendas)
            Me.Font = Nothing
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmConfAgendas"
            Me.Tag = "Configuración,1"
            CType(Me.dtgAgendas, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AgendaDataTableBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudIntervalo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBoxRazon, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBoxArticuloCita, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBoxTecnico, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PictureBoxAsesor, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents txtAgenda As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblAgenda As System.Windows.Forms.Label
        Friend WithEvents tlbAgendas As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtgAgendas As System.Windows.Forms.DataGridView
        Friend WithEvents AgendaDataTableBindingSource As System.Windows.Forms.BindingSource
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents lblIntervaloCitas As System.Windows.Forms.Label
        Friend WithEvents txtAbreviatura As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblAbreviatura As System.Windows.Forms.Label
        Friend WithEvents nudIntervalo As System.Windows.Forms.NumericUpDown
        Friend WithEvents PictureBoxRazon As System.Windows.Forms.PictureBox
        Friend WithEvents txtRazonCita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents LabelRazonAgenda As System.Windows.Forms.Label
        Friend WithEvents PictureBoxArticuloCita As System.Windows.Forms.PictureBox
        Friend WithEvents txtArticuloAgenda As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents LabelArticuloCita As System.Windows.Forms.Label
        Friend WithEvents PictureBoxTecnico As System.Windows.Forms.PictureBox
        Friend WithEvents txtTecnicoAgenda As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents LabelTecnicoAgenda As System.Windows.Forms.Label
        Friend WithEvents PictureBoxAsesor As System.Windows.Forms.PictureBox
        Friend WithEvents txtAsesorAgenda As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents LabelAsesorAgenda As System.Windows.Forms.Label
        Friend WithEvents EstadoLogicoDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AgendaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IntervaloCitas As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Abreviatura As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionAsesor As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Tecnico As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionRazonCita As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescripcionArticulo As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace