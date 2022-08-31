Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon


Namespace SCG_User_Interface
    Public Class frmTrackingRepuestos
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Declaraciones"

        Private m_dstRepuestosProveeduria As New RepuestosProveduriaDataset
        Private m_adpRepuestosProveeduria As New RepuestosProveeduriaDataAdapter

        Private Const mc_strPkRepuestoxOrdenesdeCompraPro As String = "PkRepuestoxOrdenesdeCompraPro"
        Private Const mc_strNoRepuesto As String = "NoRepuesto"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strFechaSolicitud As String = "FechaSolicitud"
        Private Const mc_strFechaCompromiso As String = "FechaCompromiso"
        Private Const mc_strFechaEntrega As String = "FechaEntrega"
        Private Const mc_strCardCode As String = "CardCode"
        Private Const mc_strCantSolicitados As String = "CantSolicitados"
        Private Const mc_strCantSuministrados As String = "CantSuministrados"
        Private Const mc_strNoAdicional As String = "NoAdicional"
        Private Const mc_strNoOrdendeCompra As String = "NoOrdendeCompra"
        Private Const mc_strNoFactura As String = "NoFactura"
        Private Const mc_strCostoRepuesto As String = "CostoRepuesto"
        Private Const mc_strPrecioCompraReal As String = "PrecioCompraReal"
        Private Const mc_strPrecioCompraDesc As String = "MontoDesc"
        Private Const mc_strDescuento As String = "Descuento"
        Private Const mc_strDescripcionProveedor As String = "CardName"
        Private Const mc_strObservaciones As String = "Observaciones"
        Private Const mc_strHoraCompromiso As String = "HoraCompromiso"
        Private Const mc_strHoraSolicitud As String = "HoraSolicitud"
        Private Const mc_strHoraEntrega As String = "HoraEntrega"

        Private Const mc_intDetalle As Integer = 11

        Private Const mc_strNulo As String = "- - -"

        Private m_strNoOrden As String
        'Private m_strNoRepuesto As String
        Private m_intIdRepuestoxOrden As Integer
        Private m_strNombreRepuesto As String
        Private m_strSeccion As String

        Private m_strMarca As String
        Private m_strModelo As String
        Private m_intAnio As Integer
        Private m_strNoChasis As String

        Private m_intPkRepuestoxOrdenesdeCompraPro As Integer

        Private tcFechaSolicitud As New DataGridTextBoxColumn
#End Region

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal NoOrden As String, _
                       ByVal IdREpuestoxOrden As Integer, _
                       ByVal NombreRepuesto As String, _
                       ByVal Marca As String, _
                       ByVal Modelo As String, _
                       ByVal Anio As Integer, _
                       ByVal NoChasis As String, _
                       ByVal TipoArticulo As Integer) 'Modificado 12/06/06. Alejandra. Recibe como nuevo parametro la seccion del repuesto


            MyBase.New()
            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            m_strNoOrden = NoOrden
            m_intIdRepuestoxOrden = IdREpuestoxOrden
            m_strNombreRepuesto = NombreRepuesto
            m_strMarca = Marca
            m_strModelo = Modelo
            m_intAnio = Anio
            m_strNoChasis = NoChasis
            If TipoArticulo = 4 Then
                lblRepuestos.Text = lblRepuestos.Text.Replace("Refacción", "Servicio Externo")
                Me.Text = Me.Text.Replace("Refacción", "Servicio Externo")

            End If
        End Sub


        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents dtgRepuestos As System.Windows.Forms.DataGrid
        Friend WithEvents lblRepuestos As System.Windows.Forms.Label
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents grbDatosAuto As System.Windows.Forms.GroupBox
        Public WithEvents lblAnio As System.Windows.Forms.Label
        Public WithEvents lblNoChasis As System.Windows.Forms.Label
        Public WithEvents lblMarcayModelo As System.Windows.Forms.Label
        Friend WithEvents txtDetalle As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTrackingRepuestos))
            Me.dtgRepuestos = New System.Windows.Forms.DataGrid
            Me.lblRepuestos = New System.Windows.Forms.Label
            Me.lblNoOrden = New System.Windows.Forms.Label
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.grbDatosAuto = New System.Windows.Forms.GroupBox
            Me.lblAnio = New System.Windows.Forms.Label
            Me.lblNoChasis = New System.Windows.Forms.Label
            Me.lblMarcayModelo = New System.Windows.Forms.Label
            Me.txtDetalle = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grbDatosAuto.SuspendLayout()
            Me.SuspendLayout()
            '
            'dtgRepuestos
            '
            Me.dtgRepuestos.BackgroundColor = System.Drawing.SystemColors.Window
            Me.dtgRepuestos.CaptionVisible = False
            Me.dtgRepuestos.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgRepuestos.HeaderForeColor = System.Drawing.SystemColors.ControlText
            resources.ApplyResources(Me.dtgRepuestos, "dtgRepuestos")
            Me.dtgRepuestos.Name = "dtgRepuestos"
            '
            'lblRepuestos
            '
            resources.ApplyResources(Me.lblRepuestos, "lblRepuestos")
            Me.lblRepuestos.Name = "lblRepuestos"
            '
            'lblNoOrden
            '
            resources.ApplyResources(Me.lblNoOrden, "lblNoOrden")
            Me.lblNoOrden.Name = "lblNoOrden"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.Name = "btnCerrar"
            '
            'grbDatosAuto
            '
            Me.grbDatosAuto.Controls.Add(Me.lblAnio)
            Me.grbDatosAuto.Controls.Add(Me.lblNoChasis)
            Me.grbDatosAuto.Controls.Add(Me.lblMarcayModelo)
            resources.ApplyResources(Me.grbDatosAuto, "grbDatosAuto")
            Me.grbDatosAuto.Name = "grbDatosAuto"
            Me.grbDatosAuto.TabStop = False
            '
            'lblAnio
            '
            resources.ApplyResources(Me.lblAnio, "lblAnio")
            Me.lblAnio.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAnio.Name = "lblAnio"
            '
            'lblNoChasis
            '
            resources.ApplyResources(Me.lblNoChasis, "lblNoChasis")
            Me.lblNoChasis.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoChasis.Name = "lblNoChasis"
            '
            'lblMarcayModelo
            '
            resources.ApplyResources(Me.lblMarcayModelo, "lblMarcayModelo")
            Me.lblMarcayModelo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMarcayModelo.Name = "lblMarcayModelo"
            '
            'txtDetalle
            '
            Me.txtDetalle.AceptaNegativos = False
            Me.txtDetalle.BackColor = System.Drawing.Color.White
            Me.txtDetalle.EstiloSBO = True
            resources.ApplyResources(Me.txtDetalle, "txtDetalle")
            Me.txtDetalle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.txtDetalle.MaxDecimales = 0
            Me.txtDetalle.MaxEnteros = 0
            Me.txtDetalle.Millares = False
            Me.txtDetalle.Name = "txtDetalle"
            Me.txtDetalle.ReadOnly = True
            Me.txtDetalle.Size_AdjustableHeight = 34
            Me.txtDetalle.TeclasDeshacer = True
            Me.txtDetalle.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'frmTrackingRepuestos
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.Controls.Add(Me.grbDatosAuto)
            Me.Controls.Add(Me.txtDetalle)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.lblNoOrden)
            Me.Controls.Add(Me.lblRepuestos)
            Me.Controls.Add(Me.dtgRepuestos)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmTrackingRepuestos"
            CType(Me.dtgRepuestos, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grbDatosAuto.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmTrackingRepuestos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                'Modificado 12/06/06. Alejandra. Agrega la seccion a la descripcion del repuesto
                lblRepuestos.Text &= " " & m_strNombreRepuesto '& " " & "/" & " " & m_strSeccion
                lblNoOrden.Text &= " " & m_strNoOrden

                m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.DefaultView.AllowEdit = False
                m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.DefaultView.AllowNew = False
                m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.DefaultView.AllowDelete = False

                Call m_adpRepuestosProveeduria.Fill(m_dstRepuestosProveeduria, m_intIdRepuestoxOrden)

                dtgRepuestos.DataSource = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria

                lblMarcayModelo.Text &= m_strMarca & "/" & m_strModelo
                lblNoChasis.Text &= m_strNoChasis
                lblAnio.Text &= CStr(m_intAnio)

                'AddHandler tcFechaSolicitud.TextBox.Validating, _
                'AddressOf caca


                'AddHandler m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.SCGTA_TB_RepuestosxOrden_ProveduriaRowChanging, _
                'AddressOf CambiaFila

                Call EstiloGridRepuestos()
                If dtgRepuestos.VisibleRowCount >= 1 _
                                         AndAlso Not dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle) Is System.Convert.DBNull Then

                    txtDetalle.Text = CStr(dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle))

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'Call MsgBox(ex.Message)

            Finally

            End Try
        End Sub

        'Private Sub CambiaFila(ByVal Sender As Object, ByVal e As DMSOneFramework.RepuestosProveduriaDataset.SCGTA_TB_RepuestosxOrden_ProveduriaRowChangeEvent)
        '    Try
        '        If e.Row.PkRepuestoxOrdenesdeCompraPro = 2 Then

        '            e.Row.EndEdit()

        '        End If
        '    Catch ex As Exception
        '        MsgBox(ex.Message)
        '    End Try
        'End Sub

        'Private Sub dtgRepuestos_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRepuestos.CurrentCellChanged

        '    If dtgRepuestos.VisibleRowCount >= 1 _
        '        AndAlso Not dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle) Is System.Convert.DBNull Then

        '        txtDetalle.Text = CStr(dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle))

        '    End If

        'End Sub

#End Region

#Region "Metodods"

        Private Sub EstiloGridRepuestos()

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.

            'Declaraciones generales
            Dim tsRepuestosProv As New DataGridTableStyle

            Call dtgRepuestos.TableStyles.Clear()

            Dim tcPkRepuestoxOrdenesdeCompraPro As New DataGridTextBoxColumn
            Dim tcNoRepuesto As New DataGridTextBoxColumn
            Dim tcNoOrden As New DataGridTextBoxColumn

            Dim tcFechaCompromiso As New DataGridTextBoxColumn
            Dim tcFechaEntrega As New DataGridTextBoxColumn
            Dim tcCardCode As New DataGridTextBoxColumn
            Dim tcCantSolicitados As New DataGridTextBoxColumn
            Dim tcCantSuministrados As New DataGridTextBoxColumn
            Dim tcNoAdicional As New DataGridTextBoxColumn
            Dim tcNoOrdendeCompra As New DataGridTextBoxColumn
            Dim tcNoFactura As New DataGridTextBoxColumn
            Dim tcCostoRepuesto As New DataGridTextBoxColumn
            Dim tcPrecioCompraReal As New DataGridTextBoxColumn
            Dim tcPrecioCompraDesc As New DataGridTextBoxColumn
            Dim tcDescuento As New DataGridTextBoxColumn
            Dim tcDescripcionProveedor As New DataGridTextBoxColumn
            Dim tcObservaciones As New DataGridTextBoxColumn
            Dim tcHoraSolicitud As New DataGridTextBoxColumn
            Dim tcHoraCompromiso As New DataGridTextBoxColumn
            Dim tcHoraEntrega As New DataGridTextBoxColumn

            Try

                tsRepuestosProv.MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.TableName

                With tcPkRepuestoxOrdenesdeCompraPro
                    .Width = 0
                    .HeaderText = ""
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strPkRepuestoxOrdenesdeCompraPro).ColumnName
                End With

                With tcNoRepuesto
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoRepuesto
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoRepuesto).ColumnName
                End With


                With tcCantSuministrados
                    .Width = 105
                    .HeaderText = My.Resources.ResourceUI.CantidadEntregada
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strCantSuministrados).ColumnName
                End With


                With tcNoOrden
                    .Width = 48
                    .HeaderText = My.Resources.ResourceUI.NoOrden
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoOrden).ColumnName
                    .Format = "###"
                End With

                With tcFechaSolicitud
                    .Width = 90
                    .HeaderText = My.Resources.ResourceUI.FechaSolicitud
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strFechaSolicitud).ColumnName
                    .Format = "dd/MM/yyyy"
                    .NullText = mc_strNulo
                End With

                With tcHoraSolicitud
                    .Width = 90
                    .HeaderText = my.Resources.ResourceUI.HoraSolicitud
                    .MappingName = mc_strHoraSolicitud
                    .Format = "hh:mm tt"
                    .NullText = mc_strNulo
                End With

                With tcFechaEntrega

                    .Width = 90
                    .HeaderText = My.Resources.ResourceUI.FechaEntrega
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strFechaEntrega).ColumnName
                    .NullText = mc_strNulo
                    .Format = "dd/MM/yyyy"
                End With

                With tcHoraEntrega
                    .Width = 90
                    .HeaderText = My.Resources.ResourceUI.HoraEntrega
                    .MappingName = mc_strHoraEntrega
                    .Format = "hh:mm tt"
                    .NullText = mc_strNulo
                End With

                With tcFechaCompromiso
                    .Width = 105
                    .HeaderText = My.Resources.ResourceUI.FechaCompromiso
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strFechaCompromiso).ColumnName
                    .NullText = mc_strNulo
                    .Format = "dd/MM/yyyy"
                End With

                With tcHoraCompromiso
                    .Width = 105
                    .HeaderText = My.Resources.ResourceUI.HoraCompromiso
                    .MappingName = mc_strHoraCompromiso
                    .Format = "hh:mm tt"
                    .NullText = mc_strNulo
                End With

                With tcNoAdicional
                    .Width = 75
                    .HeaderText = My.Resources.ResourceUI.NoAdicional
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoAdicional).ColumnName
                End With


                With tcNoOrdendeCompra
                    .Width = 117
                    .HeaderText = My.Resources.ResourceUI.NoOrdenCompra
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoOrdendeCompra).ColumnName
                    .NullText = mc_strNulo
                End With

                With tcNoFactura
                    .Width = 77
                    .HeaderText = My.Resources.ResourceUI.NoDocumento
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strNoFactura).ColumnName
                    .NullText = mc_strNulo
                End With

                With tcCostoRepuesto
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.CostoRepuesto
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strCostoRepuesto).ColumnName
                End With

                With tcPrecioCompraReal
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.PrecioCompra
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strPrecioCompraReal).ColumnName
                End With

                With tcPrecioCompraDesc
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.CompraDescuento
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strPrecioCompraDesc).ColumnName
                End With

                With tcDescuento
                    .Width = 100
                    .HeaderText = My.Resources.ResourceUI.Descuento
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strDescuento).ColumnName
                End With

                With tcDescripcionProveedor

                    .Width = 175
                    .HeaderText = My.Resources.ResourceUI.Proveedor
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strDescripcionProveedor).ColumnName
                    .NullText = mc_strNulo

                End With


                With tcObservaciones

                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Observaciones
                    .MappingName = m_dstRepuestosProveeduria.SCGTA_TB_RepuestosxOrden_Proveduria.Columns(mc_strObservaciones).ColumnName

                End With


                'Agrega las columnas al tableStyle

                With tsRepuestosProv.GridColumnStyles

                    '.Add(tcPkRepuestoxOrdenesdeCompraPro)
                    '.Add(tcNoRepuesto)
                    '.Add(tcNoOrden)
                    .Add(tcDescripcionProveedor)
                    .Add(tcFechaSolicitud)
                    .Add(tcHoraSolicitud)
                    .Add(tcFechaCompromiso)
                    .Add(tcHoraCompromiso)
                    .Add(tcFechaEntrega)
                    .Add(tcHoraEntrega)
                    '.Add(tcCardCode)
                    .Add(tcCantSolicitados)
                    '.Add(tcNoAdicional)
                    .Add(tcNoOrdendeCompra)
                    .Add(tcNoFactura)
                    .Add(tcCantSuministrados)
                    .Add(tcObservaciones)
                    '.Add(tcCostoRepuesto)
                    '.Add(tcPrecioCompraReal)
                    '.Add(tcPrecioCompraDesc)
                    '.Add(tcDescuento)

                End With

                'Establece propiedades del datagrid (colores estándares).
                tsRepuestosProv.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsRepuestosProv.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsRepuestosProv.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsRepuestosProv.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

                'Hace que el datagrid adopte las propiedades del TableStyle.

                dtgRepuestos.TableStyles.Add(tsRepuestosProv)


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
            End Try

        End Sub


#End Region






        Private Sub dtgRepuestos_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRepuestos.GotFocus
            'If dtgRepuestos.VisibleRowCount >= 1 _
            '              AndAlso Not dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle) Is System.Convert.DBNull Then

            '    txtDetalle.Text = CStr(dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle))

            'End If
        End Sub

        Private Sub dtgRepuestos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRepuestos.Click
            If dtgRepuestos.VisibleRowCount >= 1 _
                                      AndAlso Not dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle) Is System.Convert.DBNull Then

                txtDetalle.Text = CStr(dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle))

            End If
        End Sub

        Private Sub dtgRepuestos_CurrentCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgRepuestos.CurrentCellChanged
            If dtgRepuestos.VisibleRowCount >= 1 _
                                                 AndAlso Not dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle) Is System.Convert.DBNull Then

                txtDetalle.Text = CStr(dtgRepuestos.Item(dtgRepuestos.CurrentRowIndex, mc_intDetalle))

            End If
        End Sub
    End Class
End Namespace

