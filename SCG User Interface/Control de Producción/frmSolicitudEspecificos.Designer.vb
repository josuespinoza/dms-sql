
Namespace SCG_User_Interface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmSolicitudEspecificos
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
            Dim ItemSolicitudEspecificoDataset1 As DMSOneFramework.ItemSolicitudEspecificoDataset
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSolicitudEspecificos))
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.grpInformacionGeneral = New System.Windows.Forms.GroupBox()
            Me.lblDocCur = New System.Windows.Forms.Label()
            Me.txtTotalRepuestos = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblTotalRepuestos = New System.Windows.Forms.Label()
            Me.txtCliente = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblCliente = New System.Windows.Forms.Label()
            Me.txtAsesor = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.lblAsesor = New System.Windows.Forms.Label()
            Me.txtAño = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.lblAño = New System.Windows.Forms.Label()
            Me.txtVIN = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.lblVIN = New System.Windows.Forms.Label()
            Me.txtNoSolicitud = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.lblNoSolicitud = New System.Windows.Forms.Label()
            Me.txtFechaRespuesta = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtFechaSolicitud = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtObservacionesOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.txtNoUnidad = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.lblNoUnidad = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.lblFechaRespuesta = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.lblFechaSolicitud = New System.Windows.Forms.Label()
            Me.txtSolicita = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.lblSolicitadoPor = New System.Windows.Forms.Label()
            Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine1 = New System.Windows.Forms.Label()
            Me.lblNoVisita = New System.Windows.Forms.Label()
            Me.txtEstado = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtPlaca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtEstilo = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtMarca = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtTipoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtNoOrden = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.txtResponde = New NEWTEXTBOX.NEWTEXTBOX_CTRL()
            Me.lblLine4 = New System.Windows.Forms.Label()
            Me.lblLine5 = New System.Windows.Forms.Label()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.lblLine7 = New System.Windows.Forms.Label()
            Me.lblLine8 = New System.Windows.Forms.Label()
            Me.lblLine2 = New System.Windows.Forms.Label()
            Me.lblLine3 = New System.Windows.Forms.Label()
            Me.lblPlaca = New System.Windows.Forms.Label()
            Me.lblMarca = New System.Windows.Forms.Label()
            Me.lblModelo = New System.Windows.Forms.Label()
            Me.lblNoOrden = New System.Windows.Forms.Label()
            Me.lblTipoOrdenO = New System.Windows.Forms.Label()
            Me.lblEstadoSolicitud = New System.Windows.Forms.Label()
            Me.lblResponde = New System.Windows.Forms.Label()
            Me.grpItems = New System.Windows.Forms.GroupBox()
            Me.dtgDetalles = New System.Windows.Forms.DataGridView()
            Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IDSolicitudDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ItemCodeGenericoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ItemNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CantidadDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ObservacionesDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.CodEspecifico = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NomEspecifico = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Currency = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.PrecioAcordado = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.AgregarEspecifico = New System.Windows.Forms.DataGridViewImageColumn()
            Me.SinExistencia = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.Nuevo = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.IngresoPE = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.TransaccionNula = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.FreeText = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.btnAceptar = New System.Windows.Forms.Button()
            Me.btnCerrar = New System.Windows.Forms.Button()
            Me.btnImprimir = New System.Windows.Forms.Button()
            Me.btnCancelarSolicitud = New System.Windows.Forms.Button()
            ItemSolicitudEspecificoDataset1 = New DMSOneFramework.ItemSolicitudEspecificoDataset()
            CType(ItemSolicitudEspecificoDataset1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpInformacionGeneral.SuspendLayout()
            Me.grpItems.SuspendLayout()
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ItemSolicitudEspecificoDataset1
            '
            ItemSolicitudEspecificoDataset1.DataSetName = "ItemSolicitudEspecificoDataset"
            ItemSolicitudEspecificoDataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'grpInformacionGeneral
            '
            Me.grpInformacionGeneral.Controls.Add(Me.lblDocCur)
            Me.grpInformacionGeneral.Controls.Add(Me.txtTotalRepuestos)
            Me.grpInformacionGeneral.Controls.Add(Me.lblTotalRepuestos)
            Me.grpInformacionGeneral.Controls.Add(Me.txtCliente)
            Me.grpInformacionGeneral.Controls.Add(Me.lblCliente)
            Me.grpInformacionGeneral.Controls.Add(Me.txtAsesor)
            Me.grpInformacionGeneral.Controls.Add(Me.Label10)
            Me.grpInformacionGeneral.Controls.Add(Me.lblAsesor)
            Me.grpInformacionGeneral.Controls.Add(Me.txtAño)
            Me.grpInformacionGeneral.Controls.Add(Me.Label9)
            Me.grpInformacionGeneral.Controls.Add(Me.lblAño)
            Me.grpInformacionGeneral.Controls.Add(Me.txtVIN)
            Me.grpInformacionGeneral.Controls.Add(Me.Label8)
            Me.grpInformacionGeneral.Controls.Add(Me.lblVIN)
            Me.grpInformacionGeneral.Controls.Add(Me.txtNoSolicitud)
            Me.grpInformacionGeneral.Controls.Add(Me.Label7)
            Me.grpInformacionGeneral.Controls.Add(Me.lblNoSolicitud)
            Me.grpInformacionGeneral.Controls.Add(Me.txtFechaRespuesta)
            Me.grpInformacionGeneral.Controls.Add(Me.txtFechaSolicitud)
            Me.grpInformacionGeneral.Controls.Add(Me.txtObservacionesOrden)
            Me.grpInformacionGeneral.Controls.Add(Me.Label6)
            Me.grpInformacionGeneral.Controls.Add(Me.txtNoUnidad)
            Me.grpInformacionGeneral.Controls.Add(Me.Label3)
            Me.grpInformacionGeneral.Controls.Add(Me.lblNoUnidad)
            Me.grpInformacionGeneral.Controls.Add(Me.Label2)
            Me.grpInformacionGeneral.Controls.Add(Me.lblFechaRespuesta)
            Me.grpInformacionGeneral.Controls.Add(Me.Label1)
            Me.grpInformacionGeneral.Controls.Add(Me.lblFechaSolicitud)
            Me.grpInformacionGeneral.Controls.Add(Me.txtSolicita)
            Me.grpInformacionGeneral.Controls.Add(Me.Label5)
            Me.grpInformacionGeneral.Controls.Add(Me.lblSolicitadoPor)
            Me.grpInformacionGeneral.Controls.Add(Me.txtNoVisita)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine1)
            Me.grpInformacionGeneral.Controls.Add(Me.lblNoVisita)
            Me.grpInformacionGeneral.Controls.Add(Me.txtEstado)
            Me.grpInformacionGeneral.Controls.Add(Me.txtPlaca)
            Me.grpInformacionGeneral.Controls.Add(Me.txtEstilo)
            Me.grpInformacionGeneral.Controls.Add(Me.txtMarca)
            Me.grpInformacionGeneral.Controls.Add(Me.txtTipoOrden)
            Me.grpInformacionGeneral.Controls.Add(Me.txtNoOrden)
            Me.grpInformacionGeneral.Controls.Add(Me.txtResponde)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine4)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine5)
            Me.grpInformacionGeneral.Controls.Add(Me.Label12)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine7)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine8)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine2)
            Me.grpInformacionGeneral.Controls.Add(Me.lblLine3)
            Me.grpInformacionGeneral.Controls.Add(Me.lblPlaca)
            Me.grpInformacionGeneral.Controls.Add(Me.lblMarca)
            Me.grpInformacionGeneral.Controls.Add(Me.lblModelo)
            Me.grpInformacionGeneral.Controls.Add(Me.lblNoOrden)
            Me.grpInformacionGeneral.Controls.Add(Me.lblTipoOrdenO)
            Me.grpInformacionGeneral.Controls.Add(Me.lblEstadoSolicitud)
            Me.grpInformacionGeneral.Controls.Add(Me.lblResponde)
            resources.ApplyResources(Me.grpInformacionGeneral, "grpInformacionGeneral")
            Me.grpInformacionGeneral.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpInformacionGeneral.Name = "grpInformacionGeneral"
            Me.grpInformacionGeneral.TabStop = False
            '
            'lblDocCur
            '
            resources.ApplyResources(Me.lblDocCur, "lblDocCur")
            Me.lblDocCur.ForeColor = System.Drawing.SystemColors.GrayText
            Me.lblDocCur.Name = "lblDocCur"
            '
            'txtTotalRepuestos
            '
            Me.txtTotalRepuestos.AceptaNegativos = False
            Me.txtTotalRepuestos.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTotalRepuestos.EstiloSBO = True
            resources.ApplyResources(Me.txtTotalRepuestos, "txtTotalRepuestos")
            Me.txtTotalRepuestos.ForeColor = System.Drawing.Color.Black
            Me.txtTotalRepuestos.MaxDecimales = 0
            Me.txtTotalRepuestos.MaxEnteros = 0
            Me.txtTotalRepuestos.Millares = False
            Me.txtTotalRepuestos.Name = "txtTotalRepuestos"
            Me.txtTotalRepuestos.ReadOnly = True
            Me.txtTotalRepuestos.Size_AdjustableHeight = 20
            Me.txtTotalRepuestos.TeclasDeshacer = True
            Me.txtTotalRepuestos.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblTotalRepuestos
            '
            resources.ApplyResources(Me.lblTotalRepuestos, "lblTotalRepuestos")
            Me.lblTotalRepuestos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTotalRepuestos.Name = "lblTotalRepuestos"
            '
            'txtCliente
            '
            Me.txtCliente.AceptaNegativos = False
            Me.txtCliente.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtCliente.EstiloSBO = True
            resources.ApplyResources(Me.txtCliente, "txtCliente")
            Me.txtCliente.ForeColor = System.Drawing.Color.Black
            Me.txtCliente.MaxDecimales = 0
            Me.txtCliente.MaxEnteros = 0
            Me.txtCliente.Millares = False
            Me.txtCliente.Name = "txtCliente"
            Me.txtCliente.ReadOnly = True
            Me.txtCliente.Size_AdjustableHeight = 20
            Me.txtCliente.TeclasDeshacer = True
            Me.txtCliente.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.lblCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCliente.Name = "lblCliente"
            '
            'txtAsesor
            '
            Me.txtAsesor.AceptaNegativos = False
            Me.txtAsesor.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAsesor.EstiloSBO = True
            resources.ApplyResources(Me.txtAsesor, "txtAsesor")
            Me.txtAsesor.ForeColor = System.Drawing.Color.Black
            Me.txtAsesor.MaxDecimales = 0
            Me.txtAsesor.MaxEnteros = 0
            Me.txtAsesor.Millares = False
            Me.txtAsesor.Name = "txtAsesor"
            Me.txtAsesor.ReadOnly = True
            Me.txtAsesor.Size_AdjustableHeight = 20
            Me.txtAsesor.TeclasDeshacer = True
            Me.txtAsesor.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label10
            '
            Me.Label10.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label10, "Label10")
            Me.Label10.Name = "Label10"
            '
            'lblAsesor
            '
            resources.ApplyResources(Me.lblAsesor, "lblAsesor")
            Me.lblAsesor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAsesor.Name = "lblAsesor"
            '
            'txtAño
            '
            Me.txtAño.AceptaNegativos = False
            Me.txtAño.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAño.EstiloSBO = True
            resources.ApplyResources(Me.txtAño, "txtAño")
            Me.txtAño.ForeColor = System.Drawing.Color.Black
            Me.txtAño.MaxDecimales = 0
            Me.txtAño.MaxEnteros = 0
            Me.txtAño.Millares = False
            Me.txtAño.Name = "txtAño"
            Me.txtAño.ReadOnly = True
            Me.txtAño.Size_AdjustableHeight = 20
            Me.txtAño.TeclasDeshacer = True
            Me.txtAño.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'lblAño
            '
            resources.ApplyResources(Me.lblAño, "lblAño")
            Me.lblAño.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAño.Name = "lblAño"
            '
            'txtVIN
            '
            Me.txtVIN.AceptaNegativos = False
            Me.txtVIN.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtVIN.EstiloSBO = True
            resources.ApplyResources(Me.txtVIN, "txtVIN")
            Me.txtVIN.ForeColor = System.Drawing.Color.Black
            Me.txtVIN.MaxDecimales = 0
            Me.txtVIN.MaxEnteros = 0
            Me.txtVIN.Millares = False
            Me.txtVIN.Name = "txtVIN"
            Me.txtVIN.ReadOnly = True
            Me.txtVIN.Size_AdjustableHeight = 20
            Me.txtVIN.TeclasDeshacer = True
            Me.txtVIN.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'lblVIN
            '
            resources.ApplyResources(Me.lblVIN, "lblVIN")
            Me.lblVIN.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblVIN.Name = "lblVIN"
            '
            'txtNoSolicitud
            '
            Me.txtNoSolicitud.AceptaNegativos = False
            Me.txtNoSolicitud.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoSolicitud.EstiloSBO = True
            resources.ApplyResources(Me.txtNoSolicitud, "txtNoSolicitud")
            Me.txtNoSolicitud.ForeColor = System.Drawing.Color.Black
            Me.txtNoSolicitud.MaxDecimales = 0
            Me.txtNoSolicitud.MaxEnteros = 0
            Me.txtNoSolicitud.Millares = False
            Me.txtNoSolicitud.Name = "txtNoSolicitud"
            Me.txtNoSolicitud.ReadOnly = True
            Me.txtNoSolicitud.Size_AdjustableHeight = 20
            Me.txtNoSolicitud.TeclasDeshacer = True
            Me.txtNoSolicitud.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'lblNoSolicitud
            '
            resources.ApplyResources(Me.lblNoSolicitud, "lblNoSolicitud")
            Me.lblNoSolicitud.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoSolicitud.Name = "lblNoSolicitud"
            '
            'txtFechaRespuesta
            '
            Me.txtFechaRespuesta.AceptaNegativos = False
            Me.txtFechaRespuesta.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFechaRespuesta.EstiloSBO = True
            resources.ApplyResources(Me.txtFechaRespuesta, "txtFechaRespuesta")
            Me.txtFechaRespuesta.ForeColor = System.Drawing.Color.Black
            Me.txtFechaRespuesta.MaxDecimales = 0
            Me.txtFechaRespuesta.MaxEnteros = 0
            Me.txtFechaRespuesta.Millares = False
            Me.txtFechaRespuesta.Name = "txtFechaRespuesta"
            Me.txtFechaRespuesta.ReadOnly = True
            Me.txtFechaRespuesta.Size_AdjustableHeight = 20
            Me.txtFechaRespuesta.TeclasDeshacer = True
            Me.txtFechaRespuesta.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtFechaSolicitud
            '
            Me.txtFechaSolicitud.AceptaNegativos = False
            Me.txtFechaSolicitud.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtFechaSolicitud.EstiloSBO = True
            resources.ApplyResources(Me.txtFechaSolicitud, "txtFechaSolicitud")
            Me.txtFechaSolicitud.ForeColor = System.Drawing.Color.Black
            Me.txtFechaSolicitud.MaxDecimales = 0
            Me.txtFechaSolicitud.MaxEnteros = 0
            Me.txtFechaSolicitud.Millares = False
            Me.txtFechaSolicitud.Name = "txtFechaSolicitud"
            Me.txtFechaSolicitud.ReadOnly = True
            Me.txtFechaSolicitud.Size_AdjustableHeight = 20
            Me.txtFechaSolicitud.TeclasDeshacer = True
            Me.txtFechaSolicitud.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtObservacionesOrden
            '
            Me.txtObservacionesOrden.AceptaNegativos = False
            Me.txtObservacionesOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtObservacionesOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtObservacionesOrden, "txtObservacionesOrden")
            Me.txtObservacionesOrden.ForeColor = System.Drawing.Color.Black
            Me.txtObservacionesOrden.MaxDecimales = 0
            Me.txtObservacionesOrden.MaxEnteros = 0
            Me.txtObservacionesOrden.Millares = False
            Me.txtObservacionesOrden.Name = "txtObservacionesOrden"
            Me.txtObservacionesOrden.ReadOnly = True
            Me.txtObservacionesOrden.Size_AdjustableHeight = 53
            Me.txtObservacionesOrden.TeclasDeshacer = True
            Me.txtObservacionesOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label6.Name = "Label6"
            '
            'txtNoUnidad
            '
            Me.txtNoUnidad.AceptaNegativos = False
            Me.txtNoUnidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoUnidad.EstiloSBO = True
            resources.ApplyResources(Me.txtNoUnidad, "txtNoUnidad")
            Me.txtNoUnidad.ForeColor = System.Drawing.Color.Black
            Me.txtNoUnidad.MaxDecimales = 0
            Me.txtNoUnidad.MaxEnteros = 0
            Me.txtNoUnidad.Millares = False
            Me.txtNoUnidad.Name = "txtNoUnidad"
            Me.txtNoUnidad.ReadOnly = True
            Me.txtNoUnidad.Size_AdjustableHeight = 20
            Me.txtNoUnidad.TeclasDeshacer = True
            Me.txtNoUnidad.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'lblNoUnidad
            '
            resources.ApplyResources(Me.lblNoUnidad, "lblNoUnidad")
            Me.lblNoUnidad.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoUnidad.Name = "lblNoUnidad"
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'lblFechaRespuesta
            '
            resources.ApplyResources(Me.lblFechaRespuesta, "lblFechaRespuesta")
            Me.lblFechaRespuesta.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFechaRespuesta.Name = "lblFechaRespuesta"
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblFechaSolicitud
            '
            resources.ApplyResources(Me.lblFechaSolicitud, "lblFechaSolicitud")
            Me.lblFechaSolicitud.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFechaSolicitud.Name = "lblFechaSolicitud"
            '
            'txtSolicita
            '
            Me.txtSolicita.AceptaNegativos = False
            Me.txtSolicita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtSolicita.EstiloSBO = True
            resources.ApplyResources(Me.txtSolicita, "txtSolicita")
            Me.txtSolicita.ForeColor = System.Drawing.Color.Black
            Me.txtSolicita.MaxDecimales = 0
            Me.txtSolicita.MaxEnteros = 0
            Me.txtSolicita.Millares = False
            Me.txtSolicita.Name = "txtSolicita"
            Me.txtSolicita.ReadOnly = True
            Me.txtSolicita.Size_AdjustableHeight = 20
            Me.txtSolicita.TeclasDeshacer = True
            Me.txtSolicita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'lblSolicitadoPor
            '
            resources.ApplyResources(Me.lblSolicitadoPor, "lblSolicitadoPor")
            Me.lblSolicitadoPor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblSolicitadoPor.Name = "lblSolicitadoPor"
            '
            'txtNoVisita
            '
            Me.txtNoVisita.AceptaNegativos = False
            Me.txtNoVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVisita.EstiloSBO = True
            resources.ApplyResources(Me.txtNoVisita, "txtNoVisita")
            Me.txtNoVisita.ForeColor = System.Drawing.Color.Black
            Me.txtNoVisita.MaxDecimales = 0
            Me.txtNoVisita.MaxEnteros = 0
            Me.txtNoVisita.Millares = False
            Me.txtNoVisita.Name = "txtNoVisita"
            Me.txtNoVisita.ReadOnly = True
            Me.txtNoVisita.Size_AdjustableHeight = 20
            Me.txtNoVisita.TeclasDeshacer = True
            Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblNoVisita
            '
            resources.ApplyResources(Me.lblNoVisita, "lblNoVisita")
            Me.lblNoVisita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoVisita.Name = "lblNoVisita"
            '
            'txtEstado
            '
            Me.txtEstado.AceptaNegativos = False
            Me.txtEstado.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEstado.EstiloSBO = True
            resources.ApplyResources(Me.txtEstado, "txtEstado")
            Me.txtEstado.ForeColor = System.Drawing.Color.Black
            Me.txtEstado.MaxDecimales = 0
            Me.txtEstado.MaxEnteros = 0
            Me.txtEstado.Millares = False
            Me.txtEstado.Name = "txtEstado"
            Me.txtEstado.ReadOnly = True
            Me.txtEstado.Size_AdjustableHeight = 20
            Me.txtEstado.TeclasDeshacer = True
            Me.txtEstado.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtPlaca
            '
            Me.txtPlaca.AceptaNegativos = False
            Me.txtPlaca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPlaca.EstiloSBO = True
            resources.ApplyResources(Me.txtPlaca, "txtPlaca")
            Me.txtPlaca.ForeColor = System.Drawing.Color.Black
            Me.txtPlaca.MaxDecimales = 0
            Me.txtPlaca.MaxEnteros = 0
            Me.txtPlaca.Millares = False
            Me.txtPlaca.Name = "txtPlaca"
            Me.txtPlaca.ReadOnly = True
            Me.txtPlaca.Size_AdjustableHeight = 20
            Me.txtPlaca.TeclasDeshacer = True
            Me.txtPlaca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtEstilo
            '
            Me.txtEstilo.AceptaNegativos = False
            Me.txtEstilo.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtEstilo.EstiloSBO = True
            resources.ApplyResources(Me.txtEstilo, "txtEstilo")
            Me.txtEstilo.ForeColor = System.Drawing.Color.Black
            Me.txtEstilo.MaxDecimales = 0
            Me.txtEstilo.MaxEnteros = 0
            Me.txtEstilo.Millares = False
            Me.txtEstilo.Name = "txtEstilo"
            Me.txtEstilo.ReadOnly = True
            Me.txtEstilo.Size_AdjustableHeight = 20
            Me.txtEstilo.TeclasDeshacer = True
            Me.txtEstilo.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtMarca
            '
            Me.txtMarca.AceptaNegativos = False
            Me.txtMarca.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtMarca.EstiloSBO = True
            resources.ApplyResources(Me.txtMarca, "txtMarca")
            Me.txtMarca.ForeColor = System.Drawing.Color.Black
            Me.txtMarca.MaxDecimales = 0
            Me.txtMarca.MaxEnteros = 0
            Me.txtMarca.Millares = False
            Me.txtMarca.Name = "txtMarca"
            Me.txtMarca.ReadOnly = True
            Me.txtMarca.Size_AdjustableHeight = 20
            Me.txtMarca.TeclasDeshacer = True
            Me.txtMarca.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtTipoOrden
            '
            Me.txtTipoOrden.AceptaNegativos = False
            Me.txtTipoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtTipoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtTipoOrden, "txtTipoOrden")
            Me.txtTipoOrden.ForeColor = System.Drawing.Color.Black
            Me.txtTipoOrden.MaxDecimales = 0
            Me.txtTipoOrden.MaxEnteros = 0
            Me.txtTipoOrden.Millares = False
            Me.txtTipoOrden.Name = "txtTipoOrden"
            Me.txtTipoOrden.ReadOnly = True
            Me.txtTipoOrden.Size_AdjustableHeight = 20
            Me.txtTipoOrden.TeclasDeshacer = True
            Me.txtTipoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtNoOrden
            '
            Me.txtNoOrden.AceptaNegativos = False
            Me.txtNoOrden.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoOrden.EstiloSBO = True
            resources.ApplyResources(Me.txtNoOrden, "txtNoOrden")
            Me.txtNoOrden.ForeColor = System.Drawing.Color.Black
            Me.txtNoOrden.MaxDecimales = 0
            Me.txtNoOrden.MaxEnteros = 0
            Me.txtNoOrden.Millares = False
            Me.txtNoOrden.Name = "txtNoOrden"
            Me.txtNoOrden.ReadOnly = True
            Me.txtNoOrden.Size_AdjustableHeight = 20
            Me.txtNoOrden.TeclasDeshacer = True
            Me.txtNoOrden.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'txtResponde
            '
            Me.txtResponde.AceptaNegativos = False
            Me.txtResponde.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtResponde.EstiloSBO = True
            resources.ApplyResources(Me.txtResponde, "txtResponde")
            Me.txtResponde.ForeColor = System.Drawing.Color.Black
            Me.txtResponde.MaxDecimales = 0
            Me.txtResponde.MaxEnteros = 0
            Me.txtResponde.Millares = False
            Me.txtResponde.Name = "txtResponde"
            Me.txtResponde.ReadOnly = True
            Me.txtResponde.Size_AdjustableHeight = 20
            Me.txtResponde.TeclasDeshacer = True
            Me.txtResponde.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine4
            '
            Me.lblLine4.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine4, "lblLine4")
            Me.lblLine4.Name = "lblLine4"
            '
            'lblLine5
            '
            Me.lblLine5.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine5, "lblLine5")
            Me.lblLine5.Name = "lblLine5"
            '
            'Label12
            '
            Me.Label12.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label12, "Label12")
            Me.Label12.Name = "Label12"
            '
            'lblLine7
            '
            Me.lblLine7.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine7, "lblLine7")
            Me.lblLine7.Name = "lblLine7"
            '
            'lblLine8
            '
            Me.lblLine8.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLine8, "lblLine8")
            Me.lblLine8.Name = "lblLine8"
            '
            'lblLine2
            '
            Me.lblLine2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.Name = "lblLine2"
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblPlaca
            '
            resources.ApplyResources(Me.lblPlaca, "lblPlaca")
            Me.lblPlaca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblPlaca.Name = "lblPlaca"
            '
            'lblMarca
            '
            resources.ApplyResources(Me.lblMarca, "lblMarca")
            Me.lblMarca.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarca.Name = "lblMarca"
            '
            'lblModelo
            '
            resources.ApplyResources(Me.lblModelo, "lblModelo")
            Me.lblModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblModelo.Name = "lblModelo"
            '
            'lblNoOrden
            '
            resources.ApplyResources(Me.lblNoOrden, "lblNoOrden")
            Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoOrden.Name = "lblNoOrden"
            '
            'lblTipoOrdenO
            '
            resources.ApplyResources(Me.lblTipoOrdenO, "lblTipoOrdenO")
            Me.lblTipoOrdenO.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblTipoOrdenO.Name = "lblTipoOrdenO"
            '
            'lblEstadoSolicitud
            '
            resources.ApplyResources(Me.lblEstadoSolicitud, "lblEstadoSolicitud")
            Me.lblEstadoSolicitud.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblEstadoSolicitud.Name = "lblEstadoSolicitud"
            '
            'lblResponde
            '
            resources.ApplyResources(Me.lblResponde, "lblResponde")
            Me.lblResponde.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblResponde.Name = "lblResponde"
            '
            'grpItems
            '
            resources.ApplyResources(Me.grpItems, "grpItems")
            Me.grpItems.Controls.Add(Me.dtgDetalles)
            Me.grpItems.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.grpItems.Name = "grpItems"
            Me.grpItems.TabStop = False
            '
            'dtgDetalles
            '
            Me.dtgDetalles.AllowUserToAddRows = False
            Me.dtgDetalles.AllowUserToDeleteRows = False
            Me.dtgDetalles.AllowUserToResizeRows = False
            DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            Me.dtgDetalles.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
            resources.ApplyResources(Me.dtgDetalles, "dtgDetalles")
            Me.dtgDetalles.AutoGenerateColumns = False
            Me.dtgDetalles.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtgDetalles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.IDSolicitudDataGridViewTextBoxColumn, Me.ItemCodeGenericoDataGridViewTextBoxColumn, Me.ItemNameDataGridViewTextBoxColumn, Me.CantidadDataGridViewTextBoxColumn, Me.ObservacionesDataGridViewTextBoxColumn, Me.CodEspecifico, Me.NomEspecifico, Me.Currency, Me.PrecioAcordado, Me.AgregarEspecifico, Me.SinExistencia, Me.Nuevo, Me.IngresoPE, Me.TransaccionNula, Me.FreeText})
            Me.dtgDetalles.DataMember = "SCGTA_SP_SelItemSolicitudEspecifico"
            Me.dtgDetalles.DataSource = ItemSolicitudEspecificoDataset1
            Me.dtgDetalles.MultiSelect = False
            Me.dtgDetalles.Name = "dtgDetalles"
            DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
            DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgDetalles.RowsDefaultCellStyle = DataGridViewCellStyle6
            Me.dtgDetalles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
            '
            'IDDataGridViewTextBoxColumn
            '
            Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
            DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.IDDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle2
            resources.ApplyResources(Me.IDDataGridViewTextBoxColumn, "IDDataGridViewTextBoxColumn")
            Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
            Me.IDDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IDSolicitudDataGridViewTextBoxColumn
            '
            Me.IDSolicitudDataGridViewTextBoxColumn.DataPropertyName = "IDSolicitud"
            resources.ApplyResources(Me.IDSolicitudDataGridViewTextBoxColumn, "IDSolicitudDataGridViewTextBoxColumn")
            Me.IDSolicitudDataGridViewTextBoxColumn.Name = "IDSolicitudDataGridViewTextBoxColumn"
            Me.IDSolicitudDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemCodeGenericoDataGridViewTextBoxColumn
            '
            Me.ItemCodeGenericoDataGridViewTextBoxColumn.DataPropertyName = "ItemCodeGenerico"
            resources.ApplyResources(Me.ItemCodeGenericoDataGridViewTextBoxColumn, "ItemCodeGenericoDataGridViewTextBoxColumn")
            Me.ItemCodeGenericoDataGridViewTextBoxColumn.Name = "ItemCodeGenericoDataGridViewTextBoxColumn"
            Me.ItemCodeGenericoDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ItemNameDataGridViewTextBoxColumn
            '
            Me.ItemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName"
            resources.ApplyResources(Me.ItemNameDataGridViewTextBoxColumn, "ItemNameDataGridViewTextBoxColumn")
            Me.ItemNameDataGridViewTextBoxColumn.Name = "ItemNameDataGridViewTextBoxColumn"
            Me.ItemNameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CantidadDataGridViewTextBoxColumn
            '
            Me.CantidadDataGridViewTextBoxColumn.DataPropertyName = "Cantidad"
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            DataGridViewCellStyle3.Format = "N2"
            DataGridViewCellStyle3.NullValue = "0"
            Me.CantidadDataGridViewTextBoxColumn.DefaultCellStyle = DataGridViewCellStyle3
            resources.ApplyResources(Me.CantidadDataGridViewTextBoxColumn, "CantidadDataGridViewTextBoxColumn")
            Me.CantidadDataGridViewTextBoxColumn.Name = "CantidadDataGridViewTextBoxColumn"
            Me.CantidadDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ObservacionesDataGridViewTextBoxColumn
            '
            Me.ObservacionesDataGridViewTextBoxColumn.DataPropertyName = "Observaciones"
            resources.ApplyResources(Me.ObservacionesDataGridViewTextBoxColumn, "ObservacionesDataGridViewTextBoxColumn")
            Me.ObservacionesDataGridViewTextBoxColumn.Name = "ObservacionesDataGridViewTextBoxColumn"
            Me.ObservacionesDataGridViewTextBoxColumn.ReadOnly = True
            '
            'CodEspecifico
            '
            Me.CodEspecifico.DataPropertyName = "CodEspecifico"
            resources.ApplyResources(Me.CodEspecifico, "CodEspecifico")
            Me.CodEspecifico.Name = "CodEspecifico"
            Me.CodEspecifico.ReadOnly = True
            '
            'NomEspecifico
            '
            Me.NomEspecifico.DataPropertyName = "NomEspecifico"
            resources.ApplyResources(Me.NomEspecifico, "NomEspecifico")
            Me.NomEspecifico.Name = "NomEspecifico"
            Me.NomEspecifico.ReadOnly = True
            '
            'Currency
            '
            Me.Currency.DataPropertyName = "Currency"
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            Me.Currency.DefaultCellStyle = DataGridViewCellStyle4
            resources.ApplyResources(Me.Currency, "Currency")
            Me.Currency.Name = "Currency"
            Me.Currency.ReadOnly = True
            '
            'PrecioAcordado
            '
            Me.PrecioAcordado.DataPropertyName = "PrecioAcordado"
            DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            DataGridViewCellStyle5.Format = "N2"
            DataGridViewCellStyle5.NullValue = "0"
            Me.PrecioAcordado.DefaultCellStyle = DataGridViewCellStyle5
            resources.ApplyResources(Me.PrecioAcordado, "PrecioAcordado")
            Me.PrecioAcordado.Name = "PrecioAcordado"
            '
            'AgregarEspecifico
            '
            Me.AgregarEspecifico.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.AgregarEspecifico.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            Me.AgregarEspecifico.Name = "AgregarEspecifico"
            Me.AgregarEspecifico.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            resources.ApplyResources(Me.AgregarEspecifico, "AgregarEspecifico")
            '
            'SinExistencia
            '
            Me.SinExistencia.DataPropertyName = "SinExistencia"
            resources.ApplyResources(Me.SinExistencia, "SinExistencia")
            Me.SinExistencia.Name = "SinExistencia"
            '
            'Nuevo
            '
            Me.Nuevo.DataPropertyName = "Nuevo"
            resources.ApplyResources(Me.Nuevo, "Nuevo")
            Me.Nuevo.Name = "Nuevo"
            '
            'IngresoPE
            '
            Me.IngresoPE.DataPropertyName = "IngresoPE"
            resources.ApplyResources(Me.IngresoPE, "IngresoPE")
            Me.IngresoPE.Name = "IngresoPE"
            '
            'TransaccionNula
            '
            Me.TransaccionNula.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
            Me.TransaccionNula.DataPropertyName = "TransaccionNula"
            resources.ApplyResources(Me.TransaccionNula, "TransaccionNula")
            Me.TransaccionNula.Name = "TransaccionNula"
            '
            'FreeText
            '
            Me.FreeText.DataPropertyName = "FreeTxt"
            resources.ApplyResources(Me.FreeText, "FreeText")
            Me.FreeText.Name = "FreeText"
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnAceptar.Name = "btnAceptar"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.Name = "btnCerrar"
            '
            'btnImprimir
            '
            resources.ApplyResources(Me.btnImprimir, "btnImprimir")
            Me.btnImprimir.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnImprimir.Name = "btnImprimir"
            '
            'btnCancelarSolicitud
            '
            resources.ApplyResources(Me.btnCancelarSolicitud, "btnCancelarSolicitud")
            Me.btnCancelarSolicitud.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancelarSolicitud.Name = "btnCancelarSolicitud"
            '
            'frmSolicitudEspecificos
            '
            resources.ApplyResources(Me, "$this")
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.btnCancelarSolicitud)
            Me.Controls.Add(Me.grpInformacionGeneral)
            Me.Controls.Add(Me.grpItems)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.btnImprimir)
            Me.Name = "frmSolicitudEspecificos"
            CType(ItemSolicitudEspecificoDataset1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpInformacionGeneral.ResumeLayout(False)
            Me.grpInformacionGeneral.PerformLayout()
            Me.grpItems.ResumeLayout(False)
            CType(Me.dtgDetalles, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents grpInformacionGeneral As System.Windows.Forms.GroupBox
        Friend WithEvents txtSolicita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents lblSolicitadoPor As System.Windows.Forms.Label
        Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Friend WithEvents lblNoVisita As System.Windows.Forms.Label
        Friend WithEvents txtEstado As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtPlaca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtEstilo As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtMarca As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtTipoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtResponde As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblLine4 As System.Windows.Forms.Label
        Friend WithEvents lblLine5 As System.Windows.Forms.Label
        Public WithEvents Label12 As System.Windows.Forms.Label
        Public WithEvents lblLine7 As System.Windows.Forms.Label
        Public WithEvents lblLine8 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents lblPlaca As System.Windows.Forms.Label
        Friend WithEvents lblMarca As System.Windows.Forms.Label
        Public WithEvents lblModelo As System.Windows.Forms.Label
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Public WithEvents lblResponde As System.Windows.Forms.Label
        Public WithEvents lblTipoOrdenO As System.Windows.Forms.Label
        Public WithEvents lblEstadoSolicitud As System.Windows.Forms.Label
        Public WithEvents Label2 As System.Windows.Forms.Label
        Public WithEvents lblFechaRespuesta As System.Windows.Forms.Label
        Public WithEvents Label1 As System.Windows.Forms.Label
        Public WithEvents lblFechaSolicitud As System.Windows.Forms.Label
        Friend WithEvents txtNoUnidad As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label3 As System.Windows.Forms.Label
        Public WithEvents lblNoUnidad As System.Windows.Forms.Label
        Friend WithEvents txtObservacionesOrden As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents grpItems As System.Windows.Forms.GroupBox
        Friend WithEvents dtgDetalles As System.Windows.Forms.DataGridView
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents txtFechaRespuesta As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtFechaSolicitud As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtNoSolicitud As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents lblNoSolicitud As System.Windows.Forms.Label
        Friend WithEvents txtAño As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents lblAño As System.Windows.Forms.Label
        Friend WithEvents txtVIN As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label8 As System.Windows.Forms.Label
        Public WithEvents lblVIN As System.Windows.Forms.Label
        Friend WithEvents txtAsesor As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents Label10 As System.Windows.Forms.Label
        Public WithEvents lblAsesor As System.Windows.Forms.Label
        Friend WithEvents btnImprimir As System.Windows.Forms.Button
        Friend WithEvents btnCancelarSolicitud As System.Windows.Forms.Button
        Friend WithEvents txtCliente As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Friend WithEvents txtTotalRepuestos As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblTotalRepuestos As System.Windows.Forms.Label
        Friend WithEvents lblDocCur As System.Windows.Forms.Label
        Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IDSolicitudDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemCodeGenericoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ItemNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CantidadDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ObservacionesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CodEspecifico As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NomEspecifico As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Currency As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents PrecioAcordado As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents AgregarEspecifico As System.Windows.Forms.DataGridViewImageColumn
        Friend WithEvents SinExistencia As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents Nuevo As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IngresoPE As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents TransaccionNula As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents FreeText As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class

End Namespace