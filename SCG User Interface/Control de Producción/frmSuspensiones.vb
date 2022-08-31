Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmSuspensiones
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
        Friend WithEvents lblNoOrden As System.Windows.Forms.Label
        Friend WithEvents scgtbSuspensiones As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents dtgSuspensionesxOrden As System.Windows.Forms.DataGrid
        Friend WithEvents lblDescFase As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSuspensiones))
            Me.dtgSuspensionesxOrden = New System.Windows.Forms.DataGrid
            Me.lblNoOrden = New System.Windows.Forms.Label
            Me.scgtbSuspensiones = New Proyecto_SCGToolBar.SCGToolBar
            Me.lblDescFase = New System.Windows.Forms.Label
            CType(Me.dtgSuspensionesxOrden, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtgSuspensionesxOrden
            '
            Me.dtgSuspensionesxOrden.BackgroundColor = System.Drawing.Color.White
            Me.dtgSuspensionesxOrden.CaptionVisible = False
            Me.dtgSuspensionesxOrden.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgSuspensionesxOrden.HeaderForeColor = System.Drawing.SystemColors.ControlText
            resources.ApplyResources(Me.dtgSuspensionesxOrden, "dtgSuspensionesxOrden")
            Me.dtgSuspensionesxOrden.Name = "dtgSuspensionesxOrden"
            Me.dtgSuspensionesxOrden.ReadOnly = True
            '
            'lblNoOrden
            '
            resources.ApplyResources(Me.lblNoOrden, "lblNoOrden")
            Me.lblNoOrden.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblNoOrden.Name = "lblNoOrden"
            '
            'scgtbSuspensiones
            '
            resources.ApplyResources(Me.scgtbSuspensiones, "scgtbSuspensiones")
            Me.scgtbSuspensiones.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.scgtbSuspensiones.Name = "scgtbSuspensiones"
            '
            'lblDescFase
            '
            resources.ApplyResources(Me.lblDescFase, "lblDescFase")
            Me.lblDescFase.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblDescFase.Name = "lblDescFase"
            '
            'frmSuspensiones
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.lblDescFase)
            Me.Controls.Add(Me.dtgSuspensionesxOrden)
            Me.Controls.Add(Me.lblNoOrden)
            Me.Controls.Add(Me.scgtbSuspensiones)
            Me.MaximizeBox = False
            Me.Name = "frmSuspensiones"
            CType(Me.dtgSuspensionesxOrden, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Declaraciones"

        Private m_strNoOrden As String
        Private objUtilitarios As New SCGDataAccess.Utilitarios(strConectionString)
        Private m_dstSuspensionesxOrden As New SuspensionesxOrdenDataset
        Private m_adpSuspensionesxOrden As New SuspensionesxOrdenDataAdapter

        Private Const mc_strNoSuspensionxOrden As String = "NoSuspensionxOrden"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strNoReproceso As String = "NoReproceso"
        Private Const mc_strNoReprocesoxOrden As String = "NoReprocesoxOrden"
        Private Const mc_strFecha As String = "Fecha"
        Private Const mc_strRazon As String = "Razon"
        Private Const mc_strNoColaborador As String = "NoColaborador"
        Private Const mc_strTiempoManoObra As String = "TiempoManoObra"
        Private Const mc_strCosto As String = "Costo"
        Private Const mc_strFechaFin As String = "FechaFin"
        Private Const mc_strNombreFase As String = "FasedeProduccion"
        Private Const mc_strNombreColaborador As String = "Nombre"
        Private Const mc_strNoFase As String = "NoFase"
        Private Const mc_strFaseDesc As String = "FaseDesc"
        Private Const mc_strIndividual As String = "Individual"

        Private m_intNoSuspension As Integer

        Private m_intNoFase As Integer

        Private m_strDescripcionFase As String

        Private m_blnOk As Boolean

        Private m_blnEstadoASuspender As Boolean

        Public Event NuevaSuspension(ByVal ok As Boolean, ByVal NoSuspension As Integer, ByVal sender As Object)

        Private WithEvents frmChild As New frmCtrlSuspension
#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal Noorden As String, _
                       ByVal NoFase As Integer, _
                       ByVal DescripcionFase As String, _
                       ByVal p_blnEstadoASuspender As Boolean)

            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            m_strNoOrden = Noorden
            m_intNoFase = NoFase
            m_strDescripcionFase = DescripcionFase
            m_blnEstadoASuspender = p_blnEstadoASuspender
        End Sub

#End Region

#Region "Eventos"

        Private Sub frmSuspensiones_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'cargarGrid()

            Call Refrescar()

            lblNoOrden.Text &= " " & CStr(m_strNoOrden)
            lblDescFase.Text &= " " & CStr(m_strDescripcionFase)

            With scgtbSuspensiones
                If Not m_blnEstadoASuspender Then
                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = False
                End If
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False
            End With
        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim form As frmCtrlSuspension = nothing
            If IsNothing(form) Then
                form = New frmCtrlSuspension
                Me.AddOwnedForm(form)
            End If
            form.Show()
        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Me.Close()
        End Sub

        Private Sub scgtbSuspensiones_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles scgtbSuspensiones.Click_Nuevo

            'Dim frmChild As New frmCtrlSuspension(m_strNoOrden, m_intNofase, m_strDescripcionFase)

            frmChild.NoFase = m_intNoFase
            frmChild.NoOrden = m_strNoOrden
            frmChild.DescFase = m_strDescripcionFase

            frmChild.Owner = Me

            Call frmChild.ShowDialog()

            RaiseEvent NuevaSuspension(frmChild.Ok, m_intNoSuspension, Me)

            scgtbSuspensiones.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
            'scgtbSuspensiones.Buttons(scgtbSuspensiones.enumButton.Imprimir).Enabled = True
            Call Refrescar()

        End Sub

        Private Sub scgtbSuspensiones_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles scgtbSuspensiones.Click_Cerrar
            Me.Close()
        End Sub

        Private Sub frmChild_RetornaCodigo(ByVal intCodigo As Integer, ByVal dtFecha As Date) Handles frmChild.RetornaCodigo
            m_intNoSuspension = intCodigo
        End Sub

#End Region

#Region "Metodos"

        Public Sub Refrescar()

            Call m_dstSuspensionesxOrden.Clear()

            Call m_adpSuspensionesxOrden.Fill(m_dstSuspensionesxOrden, _
                                              m_strNoOrden, _
                                              m_intNoFase, _
                                              G_strCompaniaSCG, _
                                              gc_strAplicacion)


            dtgSuspensionesxOrden.DataSource = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden
            Call estiloGridSuspensiones(dtgSuspensionesxOrden)
        End Sub


        Private Sub estiloGridSuspensiones(ByRef dtgSuspensiones As DataGrid)

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.
            'Declaraciones generales
            Dim tsReprocesos As New DataGridTableStyle

            Call dtgSuspensiones.TableStyles.Clear()

            Dim tcNoSuspension As New DataGridLabelColumn
            Dim tcNoOrden As New DataGridLabelColumn
            Dim tcNoFase As New DataGridLabelColumn
            Dim tcFaseDesc As New DataGridLabelColumn
            Dim tcRazon As New DataGridLabelColumn
            Dim tcFecha As New DataGridLabelColumn
            Dim tcIndividual As New DataGridBoolColumn

            Try

                tsReprocesos.MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.TableName

                With tcNoSuspension
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoSuspension
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strNoSuspensionxOrden).ColumnName
                End With

                With tcNoOrden
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoOrden
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strNoOrden).ColumnName
                End With

                With tcNoFase
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.NoFase
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strNoFase).ColumnName
                End With

                With tcFaseDesc
                    .Width = 0
                    .HeaderText = My.Resources.ResourceUI.Fase
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strFaseDesc).ColumnName
                End With

                With tcRazon
                    .Width = 220
                    .HeaderText = My.Resources.ResourceUI.RazonSuspension
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strRazon).ColumnName
                End With

                With tcFecha
                    .Width = 200
                    .HeaderText = My.Resources.ResourceUI.FechaSuspension
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strFecha).ColumnName
                End With

                With tcIndividual
                    .Width = 135
                    .HeaderText = My.Resources.ResourceUI.SuspensionIndividual
                    .MappingName = m_dstSuspensionesxOrden.SCGTA_TB_SuspensionesxOrden.Columns(mc_strIndividual).ColumnName
                    .AllowNull = False
                End With

                'Agrega las columnas al tableStyle
                tsReprocesos.GridColumnStyles.Add(tcNoSuspension)
                tsReprocesos.GridColumnStyles.Add(tcNoOrden)
                tsReprocesos.GridColumnStyles.Add(tcNoFase)
                tsReprocesos.GridColumnStyles.Add(tcFaseDesc)
                tsReprocesos.GridColumnStyles.Add(tcRazon)
                tsReprocesos.GridColumnStyles.Add(tcFecha)
                tsReprocesos.GridColumnStyles.Add(tcIndividual)


                'Establece propiedades del datagrid (colores estándares).
                tsReprocesos.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsReprocesos.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsReprocesos.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsReprocesos.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))

                'Hace que el datagrid adopte las propiedades del TableStyle.

                dtgSuspensiones.TableStyles.Add(tsReprocesos)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
            End Try

        End Sub

#End Region

#Region "Propiedades"

        Public Property ok() As Boolean
            Get
                Return m_blnOk
            End Get
            Set(ByVal Value As Boolean)
                m_blnOk = Value
            End Set
        End Property

#End Region

    End Class
End Namespace
