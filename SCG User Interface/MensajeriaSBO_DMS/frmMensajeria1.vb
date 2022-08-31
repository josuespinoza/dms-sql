Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports SCG_User_Interface.SCG_User_Interface
Imports SCG.SkinManager

Namespace SCG_User_Interface


    Public Class frmMensajeria1
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

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
        Friend WithEvents lblMensajes As System.Windows.Forms.Label
        Friend WithEvents btnBorrarMensaje As System.Windows.Forms.Button
        Friend WithEvents dtgComunicacionView As System.Windows.Forms.DataGridView
        Friend WithEvents CheckDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents NoMensajeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DetalleDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoOrdenDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoCotizacionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaCompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoraCompromisoDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FechaAperturaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HoraAperturaDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TipoMensajeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents NoSolicitudDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents FormSkinEngine As Sunisoft.IrisSkin.SkinEngine
        Friend WithEvents Button1 As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim DestinaXMensajeSBODMSDataSetGrid As DMSOneFramework.DestinaXMensajeSBODMSDataSet
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMensajeria1))
            Me.lblMensajes = New System.Windows.Forms.Label()
            Me.btnBorrarMensaje = New System.Windows.Forms.Button()
            Me.btnCerrar = New System.Windows.Forms.Button()
            Me.dtgComunicacionView = New System.Windows.Forms.DataGridView()
            Me.CheckDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.NoMensajeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DetalleDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoOrdenDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoCotizacionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.FechaCompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.HoraCompromisoDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.FechaAperturaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.HoraAperturaDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.TipoMensajeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.NoSolicitudDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.FormSkinEngine = New Sunisoft.IrisSkin.SkinEngine(CType(Me, System.ComponentModel.Component))
            Me.Button1 = New System.Windows.Forms.Button()
            DestinaXMensajeSBODMSDataSetGrid = New DMSOneFramework.DestinaXMensajeSBODMSDataSet()
            CType(DestinaXMensajeSBODMSDataSetGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dtgComunicacionView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'DestinaXMensajeSBODMSDataSetGrid
            '
            DestinaXMensajeSBODMSDataSetGrid.DataSetName = "DestinaXMensajeSBODMSDataSet"
            DestinaXMensajeSBODMSDataSetGrid.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'lblMensajes
            '
            resources.ApplyResources(Me.lblMensajes, "lblMensajes")
            Me.lblMensajes.ForeColor = System.Drawing.Color.Black
            Me.lblMensajes.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.lblMensajes.Name = "lblMensajes"
            '
            'btnBorrarMensaje
            '
            resources.ApplyResources(Me.btnBorrarMensaje, "btnBorrarMensaje")
            Me.btnBorrarMensaje.ForeColor = System.Drawing.Color.Black
            Me.btnBorrarMensaje.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.btnBorrarMensaje.Name = "btnBorrarMensaje"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.ForeColor = System.Drawing.Color.Black
            Me.btnCerrar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.btnCerrar.Name = "btnCerrar"
            '
            'dtgComunicacionView
            '
            resources.ApplyResources(Me.dtgComunicacionView, "dtgComunicacionView")
            Me.dtgComunicacionView.AllowUserToAddRows = False
            Me.dtgComunicacionView.AllowUserToDeleteRows = False
            Me.dtgComunicacionView.AutoGenerateColumns = False
            Me.dtgComunicacionView.BackgroundColor = System.Drawing.Color.White
            Me.dtgComunicacionView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dtgComunicacionView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CheckDataGridViewCheckBoxColumn, Me.NoMensajeDataGridViewTextBoxColumn, Me.DetalleDataGridViewTextBoxColumn, Me.NoOrdenDataGridViewTextBoxColumn, Me.NoCotizacionDataGridViewTextBoxColumn, Me.FechaCompromisoDataGridViewTextBoxColumn, Me.HoraCompromisoDataGridViewTextBoxColumn, Me.FechaAperturaDataGridViewTextBoxColumn, Me.HoraAperturaDataGridViewTextBoxColumn, Me.TipoMensajeDataGridViewTextBoxColumn, Me.NoSolicitudDataGridViewTextBoxColumn})
            Me.dtgComunicacionView.DataMember = "SCGTA_TB_MensajesSBO_DMS"
            Me.dtgComunicacionView.DataSource = DestinaXMensajeSBODMSDataSetGrid
            Me.dtgComunicacionView.GridColor = System.Drawing.Color.Silver
            Me.dtgComunicacionView.Name = "dtgComunicacionView"
            '
            'CheckDataGridViewCheckBoxColumn
            '
            Me.CheckDataGridViewCheckBoxColumn.DataPropertyName = "Check"
            Me.CheckDataGridViewCheckBoxColumn.HeaderText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            resources.ApplyResources(Me.CheckDataGridViewCheckBoxColumn, "CheckDataGridViewCheckBoxColumn")
            Me.CheckDataGridViewCheckBoxColumn.Name = "CheckDataGridViewCheckBoxColumn"
            Me.CheckDataGridViewCheckBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'NoMensajeDataGridViewTextBoxColumn
            '
            Me.NoMensajeDataGridViewTextBoxColumn.DataPropertyName = "NoMensaje"
            resources.ApplyResources(Me.NoMensajeDataGridViewTextBoxColumn, "NoMensajeDataGridViewTextBoxColumn")
            Me.NoMensajeDataGridViewTextBoxColumn.Name = "NoMensajeDataGridViewTextBoxColumn"
            Me.NoMensajeDataGridViewTextBoxColumn.ReadOnly = True
            Me.NoMensajeDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'DetalleDataGridViewTextBoxColumn
            '
            Me.DetalleDataGridViewTextBoxColumn.DataPropertyName = "Detalle"
            resources.ApplyResources(Me.DetalleDataGridViewTextBoxColumn, "DetalleDataGridViewTextBoxColumn")
            Me.DetalleDataGridViewTextBoxColumn.Name = "DetalleDataGridViewTextBoxColumn"
            Me.DetalleDataGridViewTextBoxColumn.ReadOnly = True
            Me.DetalleDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'NoOrdenDataGridViewTextBoxColumn
            '
            Me.NoOrdenDataGridViewTextBoxColumn.DataPropertyName = "NoOrden"
            resources.ApplyResources(Me.NoOrdenDataGridViewTextBoxColumn, "NoOrdenDataGridViewTextBoxColumn")
            Me.NoOrdenDataGridViewTextBoxColumn.Name = "NoOrdenDataGridViewTextBoxColumn"
            Me.NoOrdenDataGridViewTextBoxColumn.ReadOnly = True
            Me.NoOrdenDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'NoCotizacionDataGridViewTextBoxColumn
            '
            Me.NoCotizacionDataGridViewTextBoxColumn.DataPropertyName = "NoCotizacion"
            resources.ApplyResources(Me.NoCotizacionDataGridViewTextBoxColumn, "NoCotizacionDataGridViewTextBoxColumn")
            Me.NoCotizacionDataGridViewTextBoxColumn.Name = "NoCotizacionDataGridViewTextBoxColumn"
            Me.NoCotizacionDataGridViewTextBoxColumn.ReadOnly = True
            Me.NoCotizacionDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'FechaCompromisoDataGridViewTextBoxColumn
            '
            Me.FechaCompromisoDataGridViewTextBoxColumn.DataPropertyName = "FechaCompromiso"
            resources.ApplyResources(Me.FechaCompromisoDataGridViewTextBoxColumn, "FechaCompromisoDataGridViewTextBoxColumn")
            Me.FechaCompromisoDataGridViewTextBoxColumn.Name = "FechaCompromisoDataGridViewTextBoxColumn"
            Me.FechaCompromisoDataGridViewTextBoxColumn.ReadOnly = True
            Me.FechaCompromisoDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'HoraCompromisoDataGridViewTextBoxColumn
            '
            Me.HoraCompromisoDataGridViewTextBoxColumn.DataPropertyName = "HoraCompromiso"
            resources.ApplyResources(Me.HoraCompromisoDataGridViewTextBoxColumn, "HoraCompromisoDataGridViewTextBoxColumn")
            Me.HoraCompromisoDataGridViewTextBoxColumn.Name = "HoraCompromisoDataGridViewTextBoxColumn"
            Me.HoraCompromisoDataGridViewTextBoxColumn.ReadOnly = True
            Me.HoraCompromisoDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'FechaAperturaDataGridViewTextBoxColumn
            '
            Me.FechaAperturaDataGridViewTextBoxColumn.DataPropertyName = "FechaApertura"
            resources.ApplyResources(Me.FechaAperturaDataGridViewTextBoxColumn, "FechaAperturaDataGridViewTextBoxColumn")
            Me.FechaAperturaDataGridViewTextBoxColumn.Name = "FechaAperturaDataGridViewTextBoxColumn"
            Me.FechaAperturaDataGridViewTextBoxColumn.ReadOnly = True
            Me.FechaAperturaDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'HoraAperturaDataGridViewTextBoxColumn
            '
            Me.HoraAperturaDataGridViewTextBoxColumn.DataPropertyName = "HoraApertura"
            resources.ApplyResources(Me.HoraAperturaDataGridViewTextBoxColumn, "HoraAperturaDataGridViewTextBoxColumn")
            Me.HoraAperturaDataGridViewTextBoxColumn.Name = "HoraAperturaDataGridViewTextBoxColumn"
            Me.HoraAperturaDataGridViewTextBoxColumn.ReadOnly = True
            Me.HoraAperturaDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'TipoMensajeDataGridViewTextBoxColumn
            '
            Me.TipoMensajeDataGridViewTextBoxColumn.DataPropertyName = "TipoMensaje"
            resources.ApplyResources(Me.TipoMensajeDataGridViewTextBoxColumn, "TipoMensajeDataGridViewTextBoxColumn")
            Me.TipoMensajeDataGridViewTextBoxColumn.Name = "TipoMensajeDataGridViewTextBoxColumn"
            Me.TipoMensajeDataGridViewTextBoxColumn.ReadOnly = True
            Me.TipoMensajeDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'NoSolicitudDataGridViewTextBoxColumn
            '
            Me.NoSolicitudDataGridViewTextBoxColumn.DataPropertyName = "NoSolicitud"
            resources.ApplyResources(Me.NoSolicitudDataGridViewTextBoxColumn, "NoSolicitudDataGridViewTextBoxColumn")
            Me.NoSolicitudDataGridViewTextBoxColumn.Name = "NoSolicitudDataGridViewTextBoxColumn"
            Me.NoSolicitudDataGridViewTextBoxColumn.ReadOnly = True
            Me.NoSolicitudDataGridViewTextBoxColumn.ToolTipText = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            '
            'FormSkinEngine
            '
            Me.FormSkinEngine.__DrawButtonFocusRectangle = True
            Me.FormSkinEngine.DisabledButtonTextColor = System.Drawing.Color.Gray
            Me.FormSkinEngine.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText
            Me.FormSkinEngine.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText
            Me.FormSkinEngine.SerialNumber = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.FormSkinEngine.SkinFile = Nothing
            '
            'Button1
            '
            resources.ApplyResources(Me.Button1, "Button1")
            Me.Button1.ForeColor = System.Drawing.Color.Black
            Me.Button1.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.String1
            Me.Button1.Name = "Button1"
            '
            'frmMensajeria1
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.Controls.Add(Me.Button1)
            Me.Controls.Add(Me.dtgComunicacionView)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.btnBorrarMensaje)
            Me.Controls.Add(Me.lblMensajes)
            Me.ForeColor = System.Drawing.SystemColors.ControlText
            Me.MaximizeBox = False
            Me.Name = "frmMensajeria1"
            CType(DestinaXMensajeSBODMSDataSetGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dtgComunicacionView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            m_intActivadoXTimer = 0 'Indica que el frm fue creado desde el menú

        End Sub

        Public Sub New(ByVal tipo As Integer, ByVal p_strIdioma As String)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(p_strIdioma)
            'Add any initialization after the InitializeComponent() call
            m_intActivadoXTimer = tipo ' = 1 Indica que el frm fue creado en el Start

            Dim path As String = Application.StartupPath
            Dim skinsPath As String = path & "\Skins.xml"

            Dim skinManager As Skin = New Skin(FormSkinEngine)

            Dim nombreMenu As String = ""

            If GlobalesUI.g_TipoSkin = 0 Then

                nombreMenu = "SAP 2007"

            End If

            If GlobalesUI.g_TipoSkin = 1 Then

                nombreMenu = "SAP 8.8"

            End If

            If System.IO.File.Exists(skinsPath) Then

                skinManager.CargarConfiguracionXml(skinsPath)

                skinManager.CargarSkin(skinsPath, nombreMenu)

            End If

        End Sub

#End Region

#Region "Declaraciones"

        'Declaracion de objeto dataAdapter y Dataset.
        Private m_adpMensajeria As SCGDataAccess.MensajeriaSBOTallerDataAdapter
        Public m_dstMensajeria As New DestinaXMensajeSBODMSDataSet

        'Columnas del grid
        Private Const mc_intNoMensaje As String = "NoMensaje"
        Private Const mc_strDetalle As String = "Detalle"
        Private Const mc_strHoraApertura As String = "HoraApertura"
        Private Const mc_strHoraCompromiso As String = "HoraCompromiso"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoCotizacion As String = "NoCotizacion"
        Private Const mc_strCheck As String = "Check"
        Private Const mc_strFechaApertura As String = "FechaApertura"
        Private Const mc_strFechaCompromiso As String = "FechaCompromiso"
        Private Const mc_strNoSolicitud As String = "NoSolicitud"
        Private Const mc_strTipoMensaje As String = "TipoMensaje"

        'Declaracion de un row del dataset, el cual sirve para insertar como para modificar y eliminar.
        Private drwMensaje As DestinaXMensajeSBODMSDataSet.SCGTA_TB_MensajesSBO_DMSRow

        Private m_intActivadoXTimer As Integer 'Se usa para saber si debe ejecutarse el evento closing

        Private WithEvents objfrmOpenOrden As frmOrden
        Private WithEvents objfrmOpenSolicitud As frmSolicitudEspecificos

        Public Enum enumTipoMensaje
            scgOrdenTrabajo = 1
            scgSolicitudEspecificos = 2
        End Enum

#End Region

#Region "Eventos"

        Private Sub frmMensajeria1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            If m_intActivadoXTimer = 1 Then 'El frm fue activado por el timer

                btnBorrarMensaje.Focus()

                m_dstMensajeria.Clear()

                dtgComunicacionView.DataSource = Nothing

                e.Cancel = True
                Me.Hide()

            End If
        End Sub

        Private Sub frmMensajeria1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
            If Asc(e.KeyChar) = Keys.Escape Then
                Me.Close()
            End If
        End Sub

        Private Sub frmMensajeria1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(m_strIdioma)
                'If dtgComunicacionView.Columns.Count = 0 Then

                '    EstiloGridMensajeria()

                'End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub frmMensajeria1_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
            Try
                'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(m_strIdioma)

                If Me.Visible Then

                    'If dtgComunicacionView.Columns.Count = 0 Then

                    '    EstiloGridMensajeria()

                    'End If

                    CargarGridRegistros() 'Carga los mensajes del usuario

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgComunicacionView_CellContentDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgComunicacionView.CellDoubleClick

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Dim adpOrdenTrabajo As New OrdenTrabajoDataAdapter
            Dim dstOrdenTrabajo As New OrdenTrabajoDataset
            Dim strNoOrden As String
            Dim intNoSolicitud As Integer

            Try
                blnExisteForm = False

                If m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.Rows.Count <> 0 Then

                    If dtgComunicacionView.CurrentRow IsNot Nothing Then

                        Select Case CInt(dtgComunicacionView.CurrentRow.Cells.Item("TipoMensajeDataGridViewTextBoxColumn").Value)
                            Case enumTipoMensaje.scgOrdenTrabajo

                                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                                    If Forma_Nueva.Name = "frmOrden" Then
                                        blnExisteForm = True
                                    End If
                                Next

                                If Not blnExisteForm Then
                                    strNoOrden = CStr(dtgComunicacionView.CurrentRow.Cells.Item(3).Value)
                                    adpOrdenTrabajo.SelOrden(dstOrdenTrabajo, strNoOrden)

                                    'Si el dataset cargó los datos
                                    If dstOrdenTrabajo.SCGTA_TB_Orden.Rows.Count > 0 Then

                                        objfrmOpenOrden = New frmOrden(dstOrdenTrabajo, strNoOrden)

                                        If Not Me.MdiParent Is Nothing Then
                                            objfrmOpenOrden.MdiParent = Me.MdiParent
                                        End If

                                        objfrmOpenOrden.Show()

                                    Else
                                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorCargarInfoOrden)
                                    End If

                                End If

                            Case enumTipoMensaje.scgSolicitudEspecificos

                                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                                    If Forma_Nueva.Name = "frmSolicitudEspecificos" Then
                                        blnExisteForm = True
                                    End If
                                Next

                                If Not blnExisteForm Then
                                    intNoSolicitud = CInt(dtgComunicacionView.CurrentRow.Cells.Item("NoSolicitudDataGridViewTextBoxColumn").Value)


                                    objfrmOpenSolicitud = New frmSolicitudEspecificos(intNoSolicitud)

                                    If Not Me.MdiParent Is Nothing Then
                                        objfrmOpenSolicitud.MdiParent = Me.MdiParent
                                    End If

                                    objfrmOpenSolicitud.Show()

                                Else
                                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorCargarInfoSolicitud)
                                End If


                        End Select


                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnBorrarMensaje_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBorrarMensaje.Click
            Try
                m_adpMensajeria = New SCGDataAccess.MensajeriaSBOTallerDataAdapter
                Dim drw As DestinaXMensajeSBODMSDataSet.SCGTA_TB_MensajesSBO_DMSRow
                Dim blnMarcados As Boolean = False
                'validacion de que el datagrid no este vacio
                If dtgComunicacionView.CurrentRow IsNot Nothing Then
                    If m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.Rows.Count > 0 Then
                        For Each drw In m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.Rows
                            If Not drw.Check Then
                                drw.RejectChanges()
                            Else
                                blnMarcados = True
                            End If
                        Next
                        'marcar el mensaje como leido
                        If blnMarcados Then
                            m_adpMensajeria.MarcarComoLeido(m_dstMensajeria, G_strUser, G_strCompaniaSCG, gc_strAplicacion, CInt(G_strIDSucursal))
                            'Call Estilo
                            Call CargarGridRegistros()
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeProcesoSatisfactorio)
                        Else
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeMarcarMensaje)
                        End If
                    End If

                End If

                ''actualiza el grid para que muestre solo los mensajes no leidos
                'm_dstRegistro.Dispose()
                'm_dstRegistro = Nothing
                'm_dstRegistro = New RegistroDataset
                'txtobservacion.Clear() 'Limpia el campo Observacion
                'CargarGridRegistros()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Try

                Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnReenviar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            ''Dim mensaje As String
            'Dim form As frmCtrlRegistroComunic

            'Try
            '    'validacion de que el datagrid no este vacio
            '    If dtgcomunicacion.CurrentRowIndex <> -1 Then

            '        If IsNothing(form) Then 'Le indica al Registro de Comunicacion que debe estar en modo Mensajeria (reenviar mensajes)
            '            form = New frmCtrlRegistroComunic(m_dstRegistro, m_dstRegistro.SCGTA_TB_Registro.Rows(dtgcomunicacion.CurrentRowIndex), 2)
            '            Me.AddOwnedForm(form)
            '        End If

            '        form.ShowDialog()

            '        'Carga de nuevo el dtgComunicacion. Esto para que no se muestren los mensajes reenviados a un destinatario que no sea el usuario.
            '        m_dstRegistro.Dispose()
            '        m_dstRegistro = Nothing
            '        m_dstRegistro = New RegistroDataset
            '        CargarGridRegistros()

            '    End If

            'Catch ex As Exception
            '    'mensaje = ex.Message
            '    clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            'End Try
        End Sub

#End Region

#Region "Procedimientos"

        Private Sub LimpiarMensajes()
            Try
                m_adpMensajeria = New SCGDataAccess.MensajeriaSBOTallerDataAdapter
                Dim drw As DestinaXMensajeSBODMSDataSet.SCGTA_TB_MensajesSBO_DMSRow
                Dim blnMarcados As Boolean = False
                'validacion de que el datagrid no este vacio
                If dtgComunicacionView.CurrentRow IsNot Nothing Then
                    If m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.Rows.Count > 0 Then
                        For Each drw In m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.Rows
                            drw.Check = True
                            'If Not drw.Check Then
                            '    drw.RejectChanges()
                            'Else
                            '    blnMarcados = True
                            'End If
                        Next
                        'marcar el mensaje como leido
                        'If blnMarcados Then
                        m_adpMensajeria.MarcarComoLeido(m_dstMensajeria, G_strUser, G_strCompaniaSCG, gc_strAplicacion, CInt(G_strIDSucursal))
                        'Call Estilo
                        Call CargarGridRegistros()
                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeProcesoSatisfactorio)
                        'Else
                        '    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeMarcarMensaje)
                        'End If
                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        'Private Sub EstiloGridMensajeria()
        '    Dim tcCheck As DataGridViewCheckBoxColumn
        '    Dim tcNoMensaje As DataGridViewTextBoxColumn
        '    Dim tcDetalle As New DataGridViewTextBoxColumn
        '    Dim tcNoOrden As New DataGridViewTextBoxColumn
        '    Dim tcNoCotizacion As New DataGridViewTextBoxColumn
        '    Dim tcFechaApertura As New DataGridViewTextBoxColumn
        '    Dim tcFechaCompromiso As New DataGridViewTextBoxColumn
        '    Dim tcHoraApertura As New DataGridViewTextBoxColumn
        '    Dim tcHoraCompromiso As New DataGridViewTextBoxColumn
        '    Dim tcTipoMensaje As New DataGridViewTextBoxColumn
        '    Dim tcNoSolicitud As New DataGridViewTextBoxColumn

        '    dtgComunicacionView.DefaultCellStyle = GetEstiloCellNormal()
        '    dtgComunicacionView.ColumnHeadersDefaultCellStyle = GetEstiloCellHeader()

        '    tcCheck = New DataGridViewCheckBoxColumn
        '    With tcCheck
        '        .Name = mc_strCheck
        '        .ReadOnly = False
        '        .DataPropertyName = mc_strCheck
        '        .HeaderText = ""
        '        .Width = 30
        '        .ThreeState = False
        '        .Frozen = True
        '        .SortMode = DataGridViewColumnSortMode.NotSortable
        '    End With

        '    tcNoMensaje = New DataGridViewTextBoxColumn
        '    With tcNoMensaje
        '        .Name = mc_intNoMensaje
        '        .ReadOnly = True
        '        .DataPropertyName = mc_intNoMensaje
        '        .HeaderText = ""
        '        .Visible = False
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '    End With

        '    tcDetalle = New DataGridViewTextBoxColumn
        '    With tcDetalle
        '        .Name = mc_strDetalle
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strDetalle
        '        .HeaderText = my.Resources.ResourceUI.Detalle
        '        .Width = 160
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '    End With

        '    tcNoSolicitud = New DataGridViewTextBoxColumn
        '    With tcNoSolicitud
        '        .Name = mc_strNoSolicitud
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strNoSolicitud
        '        .HeaderText = My.Resources.ResourceUI.NoSolicitud
        '        .Width = 160
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '        .Visible = False
        '    End With

        '    tcTipoMensaje = New DataGridViewTextBoxColumn
        '    With tcTipoMensaje
        '        .Name = mc_strTipoMensaje
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strTipoMensaje
        '        .HeaderText = My.Resources.ResourceUI.TipoMensaje
        '        .Width = 160
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '        .Visible = False
        '    End With

        '    tcNoOrden = New DataGridViewTextBoxColumn
        '    With tcNoOrden
        '        .Name = mc_strNoOrden
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strNoOrden
        '        .HeaderText = My.Resources.ResourceUI.NoOrden
        '        .Width = 70
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '    End With

        '    tcNoCotizacion = New DataGridViewTextBoxColumn
        '    With tcNoCotizacion
        '        .Name = mc_strNoCotizacion
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strNoCotizacion
        '        .HeaderText = My.Resources.ResourceUI.NoCotizacion
        '        .Width = 80
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '    End With

        '    tcHoraApertura = New DataGridViewTextBoxColumn
        '    With tcHoraApertura
        '        .Name = mc_strHoraApertura
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strHoraApertura
        '        .HeaderText = My.Resources.ResourceUI.HoraRecepcion
        '        .Width = 86
        '        .DefaultCellStyle.Format = "MM:HH"
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '    End With

        '    tcHoraCompromiso = New DataGridViewTextBoxColumn
        '    With tcHoraCompromiso
        '        .Name = mc_strHoraCompromiso
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strHoraCompromiso
        '        .HeaderText = My.Resources.ResourceUI.HoraCompromiso
        '        .Width = 96
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '    End With

        '    tcFechaApertura = New DataGridViewTextBoxColumn
        '    With tcFechaApertura
        '        .Name = mc_strFechaApertura
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strFechaApertura
        '        .HeaderText = My.Resources.ResourceUI.FechaRecepcion
        '        .Width = 94
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '        .DefaultCellStyle.Format = "dd/MM/yyyy"
        '    End With

        '    tcFechaCompromiso = New DataGridViewTextBoxColumn
        '    With tcFechaCompromiso
        '        .Name = mc_strFechaCompromiso
        '        .ReadOnly = True
        '        .DataPropertyName = mc_strFechaCompromiso
        '        .HeaderText = My.Resources.ResourceUI.FechaCompromiso
        '        .Width = 94
        '        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        '        .DefaultCellStyle.Format = "dd/MM/yyyy"
        '    End With

        '    With dtgComunicacionView

        '        .Columns.Add(tcCheck)
        '        .Columns.Add(tcNoMensaje)
        '        .Columns.Add(tcDetalle)
        '        .Columns.Add(tcNoOrden)
        '        .Columns.Add(tcNoCotizacion)
        '        .Columns.Add(tcNoSolicitud)
        '        .Columns.Add(tcFechaApertura)
        '        .Columns.Add(tcHoraApertura)
        '        .Columns.Add(tcFechaCompromiso)
        '        .Columns.Add(tcHoraCompromiso)
        '        .Columns.Add(tcTipoMensaje)

        '        .AutoGenerateColumns = False
        '        .AllowUserToAddRows = False
        '        .AllowUserToDeleteRows = False
        '        .AllowUserToOrderColumns = False
        '        .RowHeadersVisible = False
        '        .MultiSelect = False
        '        .SelectionMode = DataGridViewSelectionMode.FullRowSelect

        '        .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(244, 244, 240)

        '    End With

        'End Sub

        Private Function GetEstiloCellHeader() As DataGridViewCellStyle
            Dim objEstiloCell As DataGridViewCellStyle

            objEstiloCell = New DataGridViewCellStyle

            With objEstiloCell
                .Font = New Font("Arial Unicode MS", 9, FontStyle.Regular)
                .Alignment = DataGridViewContentAlignment.MiddleLeft
                .BackColor = Color.FromArgb(222, 223, 206)
                .ForeColor = Color.FromArgb(77, 77, 77)
                .SelectionBackColor = Color.FromArgb(222, 223, 206)
            End With

            Return objEstiloCell

        End Function

        Private Function GetEstiloCellNormal() As DataGridViewCellStyle
            Dim objEstiloCell As DataGridViewCellStyle

            objEstiloCell = New DataGridViewCellStyle

            With objEstiloCell
                .Font = New Font("Arial Unicode MS", 8, FontStyle.Regular)
                .Alignment = DataGridViewContentAlignment.MiddleLeft
                .BackColor = Color.FromArgb(253, 253, 253)
                .ForeColor = Color.FromArgb(77, 77, 77)
                .SelectionBackColor = Color.Beige
                .SelectionForeColor = Color.FromArgb(0, 53, 106)
            End With

            Return objEstiloCell

        End Function

        Private Sub mostrarObservacion()

            'Dim NoRegistro As Integer

            'Try
            '    'Limpia el campo observacion correspondiente al expediente seleccionado anteriormente
            '    txtobservacion.Clear()

            '    'Validacion de que el datagrid no esté vacio
            '    If dtgcomunicacion.CurrentRowIndex <> -1 Then

            '        'Se pone la inserción en modo de modificación.
            '        'intTipoInsercion = 2

            '        'Se valida que almenos exista un valor en el datagrid (o sino se cae al seleccionar)
            '        If m_dstRegistro.SCGTA_TB_Registro.Rows.Count <> 0 Then

            '            'Se asignan los codigos correspondientes a las variables según la selección en el datagrid
            '            NoRegistro = dtgcomunicacion.Item(dtgcomunicacion.CurrentRowIndex, 0)
            '            drwRegistro = m_dstRegistro.SCGTA_TB_Registro.FindByNoRegistro(NoRegistro)

            '            'Se asigna el texto correspondiente al campo Observacion
            '            If Not IsDBNull(drwRegistro.Observacion) Then
            '                txtobservacion.Text = drwRegistro.Observacion
            '            End If

            '        End If

            '    End If

            'Catch ex As Exception
            '    clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            'End Try


        End Sub

        Public Sub CargarGridRegistros()

            'Funcion que sera llamada desde el Start
            Try
                m_adpMensajeria = New SCGDataAccess.MensajeriaSBOTallerDataAdapter

                'Carga el dataset con los mensajes no leidos del usuario de la aplicacion
                m_dstMensajeria = Nothing
                m_dstMensajeria = New DestinaXMensajeSBODMSDataSet
                m_adpMensajeria.SeleccionarMensajes(m_dstMensajeria, G_strUser, G_strCompaniaSCG, gc_strAplicacion, CInt(G_strIDSucursal))

                With m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.DefaultView
                    .AllowDelete = False
                    .AllowEdit = True
                    .AllowNew = False
                End With

                'Refresca las vista que tiene el filtro establecido
                dtgComunicacionView.DataSource = Nothing
                dtgComunicacionView.DataSource = m_dstMensajeria.SCGTA_TB_MensajesSBO_DMS.DefaultView

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            End Try
        End Sub

#End Region

        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
            Call LimpiarMensajes()
        End Sub
    End Class
End Namespace