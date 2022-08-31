Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess.DAConexion
'Imports SCG_ComponenteImagenes.SCG_Imagenes



Namespace SCG_User_Interface

    Public Class frmCtrlDocumentosVisita
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
        Friend WithEvents tabVisita As System.Windows.Forms.TabControl
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents txtNoVisita As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents ScG_Ingreso As SCG_ComponenteImagenes.SCG_Imagenes
        Friend WithEvents ScG_Vistas As SCG_ComponenteImagenes.SCG_Imagenes
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents tabIngreso As System.Windows.Forms.TabPage
        Friend WithEvents tabVistas As System.Windows.Forms.TabPage
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCtrlDocumentosVisita))
            Me.tabVisita = New System.Windows.Forms.TabControl
            Me.tabIngreso = New System.Windows.Forms.TabPage
            Me.ScG_Ingreso = New SCG_ComponenteImagenes.SCG_Imagenes
            Me.tabVistas = New System.Windows.Forms.TabPage
            Me.ScG_Vistas = New SCG_ComponenteImagenes.SCG_Imagenes
            Me.Label1 = New System.Windows.Forms.Label
            Me.txtNoVisita = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label2 = New System.Windows.Forms.Label
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.tabVisita.SuspendLayout()
            Me.tabIngreso.SuspendLayout()
            Me.tabVistas.SuspendLayout()
            Me.SuspendLayout()
            '
            'tabVisita
            '
            Me.tabVisita.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tabVisita.Controls.Add(Me.tabIngreso)
            Me.tabVisita.Controls.Add(Me.tabVistas)
            Me.tabVisita.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tabVisita.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.tabVisita.ItemSize = New System.Drawing.Size(45, 20)
            Me.tabVisita.Location = New System.Drawing.Point(6, 34)
            Me.tabVisita.Name = "tabVisita"
            Me.tabVisita.SelectedIndex = 0
            Me.tabVisita.Size = New System.Drawing.Size(496, 368)
            Me.tabVisita.TabIndex = 949
            '
            'tabIngreso
            '
            Me.tabIngreso.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.tabIngreso.Controls.Add(Me.ScG_Ingreso)
            Me.tabIngreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tabIngreso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.tabIngreso.Location = New System.Drawing.Point(4, 24)
            Me.tabIngreso.Name = "tabIngreso"
            Me.tabIngreso.Size = New System.Drawing.Size(488, 340)
            Me.tabIngreso.TabIndex = 1
            Me.tabIngreso.Text = "Ingreso"
            '
            'ScG_Ingreso
            '
            Me.ScG_Ingreso.BackColor = System.Drawing.SystemColors.Control
            Me.ScG_Ingreso.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ScG_Ingreso.Location = New System.Drawing.Point(0, 0)
            Me.ScG_Ingreso.Name = "ScG_Ingreso"
            Me.ScG_Ingreso.P_EstiloInterfaz = SCG_ComponenteImagenes.SCG_Imagenes.TipoInterfaz.Ingreso_Imagenes
            Me.ScG_Ingreso.P_IDGrupo = 0
            Me.ScG_Ingreso.P_Imagen_Compresion = 30
            Me.ScG_Ingreso.P_Imagen_Tamanio_Peq = New System.Drawing.Size(130, 100)
            Me.ScG_Ingreso.P_SQL_Cnn = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.ScG_Ingreso.P_TablaImagenes = "SCGTA_TB_IMAGENES"
            Me.ScG_Ingreso.Size = New System.Drawing.Size(488, 340)
            Me.ScG_Ingreso.TabIndex = 949
            '
            'tabVistas
            '
            Me.tabVistas.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.tabVistas.Controls.Add(Me.ScG_Vistas)
            Me.tabVistas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.tabVistas.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.tabVistas.Location = New System.Drawing.Point(4, 24)
            Me.tabVistas.Name = "tabVistas"
            Me.tabVistas.Size = New System.Drawing.Size(488, 340)
            Me.tabVistas.TabIndex = 6
            Me.tabVistas.Text = "Vistas"
            Me.tabVistas.Visible = False
            '
            'ScG_Vistas
            '
            Me.ScG_Vistas.BackColor = System.Drawing.SystemColors.Control
            Me.ScG_Vistas.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ScG_Vistas.Location = New System.Drawing.Point(0, 0)
            Me.ScG_Vistas.Name = "ScG_Vistas"
            Me.ScG_Vistas.P_EstiloInterfaz = SCG_ComponenteImagenes.SCG_Imagenes.TipoInterfaz.Galeria
            Me.ScG_Vistas.P_IDGrupo = 0
            Me.ScG_Vistas.P_Imagen_Compresion = 30
            Me.ScG_Vistas.P_Imagen_Tamanio_Peq = New System.Drawing.Size(130, 100)
            Me.ScG_Vistas.P_SQL_Cnn = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.ScG_Vistas.P_TablaImagenes = "SCGTA_TB_IMAGENES"
            Me.ScG_Vistas.Size = New System.Drawing.Size(488, 340)
            Me.ScG_Vistas.TabIndex = 950
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label1.Location = New System.Drawing.Point(7, 14)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(62, 13)
            Me.Label1.TabIndex = 950
            Me.Label1.Text = "No. Visita"
            '
            'txtNoVisita
            '
            Me.txtNoVisita.AceptaNegativos = False
            Me.txtNoVisita.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtNoVisita.EstiloSBO = True
            Me.txtNoVisita.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.txtNoVisita.Location = New System.Drawing.Point(103, 10)
            Me.txtNoVisita.MaxDecimales = 0
            Me.txtNoVisita.MaxEnteros = 0
            Me.txtNoVisita.Millares = False
            Me.txtNoVisita.Name = "txtNoVisita"
            Me.txtNoVisita.ReadOnly = True
            Me.txtNoVisita.RightToLeft = System.Windows.Forms.RightToLeft.Yes
            Me.txtNoVisita.Size = New System.Drawing.Size(90, 20)
            Me.txtNoVisita.Size_AdjustableHeight = 20
            Me.txtNoVisita.TabIndex = 951
            Me.txtNoVisita.TeclasDeshacer = True
            Me.txtNoVisita.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.Label2.Location = New System.Drawing.Point(7, 28)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(97, 1)
            Me.Label2.TabIndex = 952
            '
            'btnCerrar
            '
            Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.Location = New System.Drawing.Point(6, 410)
            Me.btnCerrar.Name = "btnCerrar"
            Me.btnCerrar.Size = New System.Drawing.Size(80, 22)
            Me.btnCerrar.TabIndex = 953
            Me.btnCerrar.Text = "Cerrar"
            '
            'frmCtrlDocumentosVisita
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.ClientSize = New System.Drawing.Size(508, 464)
            Me.Controls.Add(Me.txtNoVisita)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.tabVisita)
            Me.Controls.Add(Me.Label1)
            Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "frmCtrlDocumentosVisita"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "<SCG> Documentos Visita"
            Me.tabVisita.ResumeLayout(False)
            Me.tabIngreso.ResumeLayout(False)
            Me.tabVistas.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal strNoVisita As String)

            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            p_strNoVisita = strNoVisita

            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Declaraciones"

        Private p_strNoVisita As String

#End Region

#Region "Métodos"

        Private Sub CargarImagenes()
            Try
                Dim objDA As New DMSOneFramework.SCGDataAccess.DAConexion
                Dim intNumeroAsociacion As Integer

                intNumeroAsociacion = CargarAsociacionExpImg(CInt(p_strNoVisita), 2)

                ScG_Ingreso.P_IDGrupo = intNumeroAsociacion
                ScG_Ingreso.P_SQL_Cnn = DAConexion.ConnectionString
                ScG_Ingreso.P_Imagen_Compresion = SCGDataAccess.Configuracion.DevuelveValordeParametro("'ImageCompresion'", COMPANIA, strDATABASESCG, DMSOneFramework.SCGDataAccess.objBLConexion)
                ScG_Ingreso.ActivarComponente()

                ScG_Vistas.P_IDGrupo = intNumeroAsociacion
                ScG_Vistas.P_SQL_Cnn = DAConexion.ConnectionString
                ScG_Vistas.P_Imagen_Compresion = SCGDataAccess.Configuracion.DevuelveValordeParametro("'ImageCompresion'", COMPANIA, strDATABASESCG, DMSOneFramework.SCGDataAccess.objBLConexion)
                ScG_Vistas.ActivarComponente()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

#Region "Eventos"

        Private Sub frmCtrlDocumentosVisita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Me.MdiParent.Cursor = Cursors.WaitCursor

                Me.txtNoVisita.Text = p_strNoVisita

                CargarImagenes()

                Me.MdiParent.Cursor = Cursors.Arrow

            Catch ex As Exception

                Me.MdiParent.Cursor = Cursors.Arrow

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Me.Close()
        End Sub

        Private Sub tabVisita_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabVisita.SelectedIndexChanged
            Try
                If tabVisita.SelectedIndex = 1 Then
                    ScG_Vistas.ActivarComponente()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

    End Class

End Namespace