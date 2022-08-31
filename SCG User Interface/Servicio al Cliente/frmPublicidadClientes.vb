Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
'Imports Microsoft.Office
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGCommon
Namespace SCG_User_Interface
    Public Class FrmPublicidadClientes
        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP


#Region "Declaraciones"

        Private m_strNombreArchivoAdjunto As String



        Private WithEvents m_buClientes As New Buscador.SubBuscador
        Private WithEvents m_buPublicidad As New Buscador.SubBuscador

        Private m_ListadeDestinatarios As String

        Public m_dstCliente As New DataSet

        Private m_dstPublicidad As New DMSOneFramework.PublicidadEnvioDataset
        Private m_adpPublicidad As New PublicidadEnviosAdapter
        Friend WithEvents dtpHoraEnvio As System.Windows.Forms.DateTimePicker

        Private m_intEnviado As Integer
#End Region






#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal carga As Boolean)
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
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpFechaCita As System.Windows.Forms.DateTimePicker
        Public WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents lblFecha As System.Windows.Forms.Label
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents lblCliente As System.Windows.Forms.Label
        Public WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents picCliente As System.Windows.Forms.PictureBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents txtAsunto As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents lblMaterial As System.Windows.Forms.Label
        Friend WithEvents txtDetalle As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents txtMaterial As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents btnEliminarRep As System.Windows.Forms.Button
        Friend WithEvents btnAgregarRep As System.Windows.Forms.Button
        Friend WithEvents dtgClientes As System.Windows.Forms.DataGrid
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents ScgTbClientes As Proyecto_SCGToolBar.SCGToolBar
        Friend WithEvents Button1 As System.Windows.Forms.Button
        Public WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Public WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents lblPublicidad As System.Windows.Forms.Label
        Friend WithEvents gbEncabezado As System.Windows.Forms.GroupBox
        Friend WithEvents txtPublicidad As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents EPPublicidad As System.Windows.Forms.ErrorProvider
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmPublicidadClientes))
            Me.dtpFechaCita = New System.Windows.Forms.DateTimePicker
            Me.Label3 = New System.Windows.Forms.Label
            Me.lblFecha = New System.Windows.Forms.Label
            Me.txtAsunto = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label2 = New System.Windows.Forms.Label
            Me.lblCliente = New System.Windows.Forms.Label
            Me.txtMaterial = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label1 = New System.Windows.Forms.Label
            Me.lblMaterial = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.dtgClientes = New System.Windows.Forms.DataGrid
            Me.txtDetalle = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label5 = New System.Windows.Forms.Label
            Me.ScgTbClientes = New Proyecto_SCGToolBar.SCGToolBar
            Me.gbEncabezado = New System.Windows.Forms.GroupBox
            Me.dtpHoraEnvio = New System.Windows.Forms.DateTimePicker
            Me.txtPublicidad = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label8 = New System.Windows.Forms.Label
            Me.lblPublicidad = New System.Windows.Forms.Label
            Me.Label6 = New System.Windows.Forms.Label
            Me.Label7 = New System.Windows.Forms.Label
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.picCliente = New System.Windows.Forms.PictureBox
            Me.EPPublicidad = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.Button1 = New System.Windows.Forms.Button
            Me.btnEliminarRep = New System.Windows.Forms.Button
            Me.btnAgregarRep = New System.Windows.Forms.Button
            CType(Me.dtgClientes, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.gbEncabezado.SuspendLayout()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.EPPublicidad, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtpFechaCita
            '
            resources.ApplyResources(Me.dtpFechaCita, "dtpFechaCita")
            Me.dtpFechaCita.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCita.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaCita.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpFechaCita.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCita.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpFechaCita.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFechaCita.Name = "dtpFechaCita"
            Me.dtpFechaCita.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'lblFecha
            '
            resources.ApplyResources(Me.lblFecha, "lblFecha")
            Me.lblFecha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFecha.Name = "lblFecha"
            '
            'txtAsunto
            '
            Me.txtAsunto.AceptaNegativos = False
            Me.txtAsunto.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtAsunto.EstiloSBO = True
            resources.ApplyResources(Me.txtAsunto, "txtAsunto")
            Me.txtAsunto.MaxDecimales = 0
            Me.txtAsunto.MaxEnteros = 0
            Me.txtAsunto.Millares = False
            Me.txtAsunto.Name = "txtAsunto"
            Me.txtAsunto.Size_AdjustableHeight = 20
            Me.txtAsunto.TeclasDeshacer = True
            Me.txtAsunto.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'lblCliente
            '
            resources.ApplyResources(Me.lblCliente, "lblCliente")
            Me.lblCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblCliente.Name = "lblCliente"
            '
            'txtMaterial
            '
            Me.txtMaterial.AceptaNegativos = False
            Me.txtMaterial.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtMaterial.EstiloSBO = True
            resources.ApplyResources(Me.txtMaterial, "txtMaterial")
            Me.txtMaterial.MaxDecimales = 0
            Me.txtMaterial.MaxEnteros = 0
            Me.txtMaterial.Millares = False
            Me.txtMaterial.Name = "txtMaterial"
            Me.txtMaterial.ReadOnly = True
            Me.txtMaterial.Size_AdjustableHeight = 20
            Me.txtMaterial.TeclasDeshacer = True
            Me.txtMaterial.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label1
            '
            Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'lblMaterial
            '
            resources.ApplyResources(Me.lblMaterial, "lblMaterial")
            Me.lblMaterial.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblMaterial.Name = "lblMaterial"
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.Name = "Label4"
            '
            'dtgClientes
            '
            Me.dtgClientes.BackColor = System.Drawing.SystemColors.Control
            Me.dtgClientes.BackgroundColor = System.Drawing.Color.White
            Me.dtgClientes.CaptionVisible = False
            Me.dtgClientes.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgClientes.HeaderForeColor = System.Drawing.SystemColors.ControlText
            resources.ApplyResources(Me.dtgClientes, "dtgClientes")
            Me.dtgClientes.Name = "dtgClientes"
            '
            'txtDetalle
            '
            Me.txtDetalle.AceptaNegativos = False
            Me.txtDetalle.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtDetalle.EstiloSBO = True
            resources.ApplyResources(Me.txtDetalle, "txtDetalle")
            Me.txtDetalle.MaxDecimales = 0
            Me.txtDetalle.MaxEnteros = 0
            Me.txtDetalle.Millares = False
            Me.txtDetalle.Name = "txtDetalle"
            Me.txtDetalle.Size_AdjustableHeight = 106
            Me.txtDetalle.TeclasDeshacer = True
            Me.txtDetalle.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label5
            '
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label5.Name = "Label5"
            '
            'ScgTbClientes
            '
            resources.ApplyResources(Me.ScgTbClientes, "ScgTbClientes")
            Me.ScgTbClientes.EstadoActual = Proyecto_SCGToolBar.SCGToolBar.enumEstadoToolBar.Modificando
            Me.ScgTbClientes.Name = "ScgTbClientes"
            '
            'gbEncabezado
            '
            resources.ApplyResources(Me.gbEncabezado, "gbEncabezado")
            Me.gbEncabezado.Controls.Add(Me.dtpHoraEnvio)
            Me.gbEncabezado.Controls.Add(Me.Label1)
            Me.gbEncabezado.Controls.Add(Me.txtPublicidad)
            Me.gbEncabezado.Controls.Add(Me.Label8)
            Me.gbEncabezado.Controls.Add(Me.lblPublicidad)
            Me.gbEncabezado.Controls.Add(Me.Label6)
            Me.gbEncabezado.Controls.Add(Me.Label7)
            Me.gbEncabezado.Controls.Add(Me.txtMaterial)
            Me.gbEncabezado.Controls.Add(Me.Panel1)
            Me.gbEncabezado.Controls.Add(Me.txtAsunto)
            Me.gbEncabezado.Controls.Add(Me.picCliente)
            Me.gbEncabezado.Controls.Add(Me.lblMaterial)
            Me.gbEncabezado.Controls.Add(Me.dtpFechaCita)
            Me.gbEncabezado.Controls.Add(Me.Label2)
            Me.gbEncabezado.Controls.Add(Me.Label3)
            Me.gbEncabezado.Controls.Add(Me.lblFecha)
            Me.gbEncabezado.Controls.Add(Me.lblCliente)
            Me.gbEncabezado.Name = "gbEncabezado"
            Me.gbEncabezado.TabStop = False
            '
            'dtpHoraEnvio
            '
            resources.ApplyResources(Me.dtpHoraEnvio, "dtpHoraEnvio")
            Me.dtpHoraEnvio.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHoraEnvio.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHoraEnvio.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHoraEnvio.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHoraEnvio.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHoraEnvio.Format = System.Windows.Forms.DateTimePickerFormat.Time
            Me.dtpHoraEnvio.Name = "dtpHoraEnvio"
            Me.dtpHoraEnvio.ShowUpDown = True
            Me.dtpHoraEnvio.Value = New Date(2005, 11, 28, 0, 0, 0, 0)
            '
            'txtPublicidad
            '
            Me.txtPublicidad.AceptaNegativos = False
            Me.txtPublicidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtPublicidad.EstiloSBO = True
            resources.ApplyResources(Me.txtPublicidad, "txtPublicidad")
            Me.txtPublicidad.MaxDecimales = 0
            Me.txtPublicidad.MaxEnteros = 0
            Me.txtPublicidad.Millares = False
            Me.txtPublicidad.Name = "txtPublicidad"
            Me.txtPublicidad.Size_AdjustableHeight = 20
            Me.txtPublicidad.TeclasDeshacer = True
            Me.txtPublicidad.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'lblPublicidad
            '
            resources.ApplyResources(Me.lblPublicidad, "lblPublicidad")
            Me.lblPublicidad.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblPublicidad.Name = "lblPublicidad"
            '
            'Label6
            '
            Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'Label7
            '
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label7.Name = "Label7"
            '
            'Panel1
            '
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Panel1.Name = "Panel1"
            '
            'picCliente
            '
            Me.picCliente.Image = Global.SCG_User_Interface.My.Resources.Resources.dialogBox
            resources.ApplyResources(Me.picCliente, "picCliente")
            Me.picCliente.Name = "picCliente"
            Me.picCliente.TabStop = False
            '
            'EPPublicidad
            '
            Me.EPPublicidad.ContainerControl = Me
            '
            'Button1
            '
            resources.ApplyResources(Me.Button1, "Button1")
            Me.Button1.ForeColor = System.Drawing.Color.Black
            Me.Button1.Name = "Button1"
            '
            'btnEliminarRep
            '
            resources.ApplyResources(Me.btnEliminarRep, "btnEliminarRep")
            Me.btnEliminarRep.ForeColor = System.Drawing.Color.Black
            Me.btnEliminarRep.Name = "btnEliminarRep"
            '
            'btnAgregarRep
            '
            resources.ApplyResources(Me.btnAgregarRep, "btnAgregarRep")
            Me.btnAgregarRep.ForeColor = System.Drawing.Color.Black
            Me.btnAgregarRep.Name = "btnAgregarRep"
            '
            'FrmPublicidadClientes
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.Controls.Add(Me.Button1)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.btnEliminarRep)
            Me.Controls.Add(Me.btnAgregarRep)
            Me.Controls.Add(Me.txtDetalle)
            Me.Controls.Add(Me.dtgClientes)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.gbEncabezado)
            Me.Controls.Add(Me.ScgTbClientes)
            Me.MaximizeBox = False
            Me.Name = "FrmPublicidadClientes"
            CType(Me.dtgClientes, System.ComponentModel.ISupportInitialize).EndInit()
            Me.gbEncabezado.ResumeLayout(False)
            Me.gbEncabezado.PerformLayout()
            CType(Me.picCliente, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.EPPublicidad, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        Private Sub picCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picCliente.Click

            If SeleccionaArchivo(m_strNombreArchivoAdjunto) Then
                txtMaterial.Text = m_strNombreArchivoAdjunto
            End If
        End Sub

        Private Function SeleccionaArchivo(ByRef NombreDeArchivo As String) As Boolean

            Dim openFileDialog1 As New OpenFileDialog

            Try

                openFileDialog1.InitialDirectory = "c:\"
                openFileDialog1.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
                openFileDialog1.FilterIndex = 2
                openFileDialog1.RestoreDirectory = True

                If openFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    NombreDeArchivo = openFileDialog1.FileName
                End If

                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
            End Try
        End Function

        Private Sub btnAgregarRep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarRep.Click
            Try

                'Se debe consultar a BD y preguntar si se filtran clientes


                Dim objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)


                If objUtilitarios.TraerValorFiltros Then

                    Dim frmFiltroClientes As New frmFiltroClientes(Me)


                    frmFiltroClientes.Show()

                    Exit Sub

                End If

                '**********************************************************************************************************************************

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                With m_buClientes
                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Email
                    .Criterios = "CardCode, CardName,e_mail"
                    .Criterios_Ocultos = 1
                    .Tabla = "SCGTA_VW_Clientes"
                    .MultiSeleccion = True
                    .Where = ""
                    .Activar_Buscador(sender)
                End With


            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub m_buClientes_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buClientes.AppAceptar
            Try

                If m_dstCliente.Tables.Count = 0 Then

                    m_dstCliente.Tables.Add(m_buClientes.OUT_DataTable)
                    m_dstCliente.Tables(0).DefaultView.AllowNew = False
                    'm_dstCliente.Tables(0).DefaultView.AllowEdit = False
                    'm_dstCliente.Tables(0).Columns("CardCode").Unique = True
                Else

                    m_dstCliente.Merge(m_buClientes.OUT_DataTable)
                End If


                dtgClientes.DataSource = m_dstCliente.Tables(0)
                Call estiloGrid("Table")
                'CType(dtgClientes.DataSource, DataTable).DataSet.Merge(m_buClientes.OUT_DataTable)
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub estiloGrid(ByVal NombredeTabla As String)

            'Dim mensaje As String
            'Esta funciön pone las propiedades del datagrid por código con el objetivo de que cumpla los estándares.
            'Declaraciones(generales)
            Dim tsReprocesos As New DataGridTableStyle

            Call dtgClientes.TableStyles.Clear()

            Dim tcCardCode As New DataGridTextBoxColumn
            Dim tcCardName As New DataGridTextBoxColumn
            Dim tcE_mail As New DataGridTextBoxColumn
            Dim tcSelectCheck As New DataGridBoolColumn

            Try

                tsReprocesos.MappingName = NombredeTabla '"Table"


                With tcCardCode
                    .Width = 70
                    .HeaderText = My.Resources.ResourceUI.CodCliente
                    .MappingName = "CardCode"
                    .ReadOnly = True
                End With

                With tcCardName
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.Cliente
                    .MappingName = "CardName"
                    .ReadOnly = True
                End With

                With tcE_mail
                    .Width = 150
                    .HeaderText = My.Resources.ResourceUI.Email
                    .MappingName = "e_mail"
                    .NullText = "----"
                    .ReadOnly = True
                End With

                With tcSelectCheck
                    .Width = 30 '60
                    '.HeaderText = "Check"
                    .MappingName = "SelectCheck"
                    .ReadOnly = False
                    .AllowNull = False
                End With

                'Agrega las columnas al tableStyle
                ' tsReprocesos.GridColumnStyles.Add(tcNoReprocesoxOrden)

                With tsReprocesos

                    .GridColumnStyles.Add(tcSelectCheck)
                    .GridColumnStyles.Add(tcCardCode)
                    .GridColumnStyles.Add(tcCardName)
                    .GridColumnStyles.Add(tcE_mail)

                End With

                'tsReprocesos.GridColumnStyles.Add(tcSelectCheck)

                'Establece propiedades del datagrid (colores estándares).
                tsReprocesos.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
                tsReprocesos.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
                tsReprocesos.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
                tsReprocesos.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
                tsReprocesos.RowHeadersVisible = False
                'Hace que el datagrid adopte las propiedades del TableStyle.

                dtgClientes.TableStyles.Add(tsReprocesos)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub FrmPublicidadClientes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            With ScgTbClientes

                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Exportar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Imprimir).Visible = False

                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Eliminar).Visible = False
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Cancelar).Visible = False

            End With
            'Carga fecha y hora actuales
            dtpHoraEnvio.Value = New Date(1900, 1, 1, Now.Hour, Now.Minute, Now.Second)

            dtpFechaCita.Value = Today

            Call estiloGrid("table")

            Call HabilitoControles(False)

        End Sub

        Private Sub HabilitoControles(ByVal Habilitado As Boolean)
            Try
                gbEncabezado.Enabled = Habilitado
                dtgClientes.Enabled = Habilitado
                btnAgregarRep.Enabled = Habilitado
                btnEliminarRep.Enabled = Habilitado
                txtDetalle.Enabled = Habilitado
                txtAsunto.Enabled = Habilitado
                dtpFechaCita.Enabled = Habilitado
                dtpHoraEnvio.Enabled = Habilitado
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub ScgTbClientes_Click_Imprimir(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Imprimir

        End Sub

        Private Sub ScgTbClientes_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Nuevo

            With ScgTbClientes

                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True
                .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Buscar).Enabled = True

            End With

            dtpFechaCita.Value = System.DateTime.Now
            dtpHoraEnvio.Value = System.DateTime.Now.ToLocalTime

            Call LimpiarControles()
            Call HabilitoControles(True)
            Call LimpiaErroesDeErrorProvider()
        End Sub

        Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
            Try

                Me.Cursor = Cursors.WaitCursor

                If Not FaltanCamposRequeridos() Then

                    If m_dstCliente.Tables.Count > 0 _
                                   AndAlso m_dstCliente.Tables(0).Rows.Count > 0 Then

                        If Mensajeria.CreaListadeDestinatarios(m_dstCliente.Tables(0), m_ListadeDestinatarios) Then

                            If Not Mensajeria.EnviaCorreo(m_ListadeDestinatarios, txtAsunto.Text, _
                                                        txtDetalle.Text, txtMaterial.Text, _
                                                        g_strServidordeCorreo, g_strDirEnviaCorreo, _
                                                        g_strUsuarioSMTP, g_strPasswordSMTP, "", m_dstCliente.Tables(0), g_strPuerto, g_chkUsaSSL) Then

                                Call MsgBox(My.Resources.ResourceUI.MensajeLosCorreosNoPuedenSerEnviadosX & ":" & vbCrLf & _
                                                                   " -" & My.Resources.ResourceUI.MensajeServidorNoConfigurado & vbCrLf & _
                                                                   " -" & My.Resources.ResourceUI.MensajeCuentaCorreoNoConfigurada & vbCrLf & _
                                                                   " -" & My.Resources.ResourceUI.MensajeUsuarioOContrasenaInvalidos, MsgBoxStyle.Information)
                                m_intEnviado = 1

                                Call Guardar(m_intEnviado)

                            Else


                                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeCorreoEnviadoSatisfactoriamente)
                            End If

                        End If

                    Else
                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarClienteEnviarCorreo)
                    End If

                End If
                Me.Cursor = Cursors.Arrow
            Catch ex As Exception
                If ex.Message = "1" Then
                    MsgBox(My.Resources.ResourceUI.MensajeEnvioCitasNoEjecutado, MsgBoxStyle.Information, "SCG Taller")
                Else
                    Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                    'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                End If
            End Try

        End Sub



        Private Sub ScgTbClientes_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Cerrar
            Me.Close()
        End Sub

        Private Sub btnEliminarRep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminarRep.Click

            Try
                Dim indice As Integer

                If m_dstCliente.Tables.Count > 0 Then 'Para que no se caiga cuando no se ha agregado antes ningún cliente

                    If m_dstCliente.Tables(0).Rows.Count > 0 Then

                        For indice = m_dstCliente.Tables(0).Rows.Count - 1 To 0 Step -1

                            If m_dstCliente.Tables(0).Rows(indice).RowState <> DataRowState.Deleted Then

                                If CBool(m_dstCliente.Tables(0).Rows(indice)("SelectCheck")) = True Then


                                    m_dstCliente.Tables(0).Rows(indice).Delete()

                                End If

                            End If

                        Next indice


                    End If

                End If ' m_dstCliente.Tables.Count > 0

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try



        End Sub

        Private Sub ScgTbClientes_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Guardar

            Call Guardar(0)

        End Sub

        Private Function Guardar(ByVal Enviado As Integer) As Boolean
            Try

                Dim intidPublicidad As Integer

                If Not FaltanCamposRequeridos() Then

                    If AgregaEncabezadoPublicidad(m_dstPublicidad.SCGTA_TB_EnvioPublicidad, _
                                                  txtPublicidad.Text, _
                                                  intidPublicidad, _
                                                  dtpFechaCita.Value, _
                                                  dtpHoraEnvio.Value, _
                                                  txtAsunto.Text, _
                                                  txtMaterial.Text, _
                                                  txtDetalle.Text, _
                                                  Enviado) Then


                        Call m_dstPublicidad.SCGTA_TB_DetalleEnvioPublicidad.Clear()
                        Call AgregaDetallePublicidad(m_dstCliente.Tables(0), _
                                                     intidPublicidad, m_dstPublicidad.SCGTA_TB_DetalleEnvioPublicidad)
                        Call m_adpPublicidad.Update(m_dstPublicidad)
                        ScgTbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                        Call HabilitoControles(False)

                        ScgTbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

                        'Else

                        '    Call objSCGMSGBox.msgInformationCustom("No agrego ningún contacto para enviar la publicidad")
                        'End If
                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Function

        Private Function CantidadDerowsAgregadas(ByVal dtbCliente As DataTable) As Integer
            Dim drwCliente As DataRow
            Dim intRowsValidas As Integer
            Try

                For Each drwCliente In dtbCliente.Rows

                    If drwCliente.RowState <> DataRowState.Deleted Then
                        intRowsValidas += 1
                    End If
                Next drwCliente

                Return intRowsValidas

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Function



        Private Function AgregaEncabezadoPublicidad(ByRef dtbPublicidad As PublicidadEnvioDataset.SCGTA_TB_EnvioPublicidadDataTable, _
                                                    ByVal EtiquetaPublicidad As String, _
                                                    ByRef idPublicidad As Integer, _
                                                    ByVal dtFechaEnvio As Date, _
                                                    ByVal dtHoraEnvio As Date, _
                                                    ByVal strAsunto As String, _
                                                    ByVal strMaterial As String, _
                                                    ByVal strDetalle As String, _
                                                    ByVal intEnviado As Integer)

            Dim drwPublicidad As PublicidadEnvioDataset.SCGTA_TB_EnvioPublicidadRow

            Try
                drwPublicidad = dtbPublicidad.NewSCGTA_TB_EnvioPublicidadRow

                drwPublicidad.Asunto = strAsunto
                drwPublicidad.Detalle = strDetalle
                drwPublicidad.Material = strMaterial
                drwPublicidad.FechaEnvio = dtFechaEnvio.Date

                drwPublicidad.HoraEnvio = New Date(1900, 1, 1, _
                                                    dtHoraEnvio.Hour, _
                                                    dtHoraEnvio.Minute, _
                                                    dtHoraEnvio.Second)



                drwPublicidad.EtiquetaPublicidad = EtiquetaPublicidad
                drwPublicidad.Enviado = intEnviado

                Call dtbPublicidad.AddSCGTA_TB_EnvioPublicidadRow(drwPublicidad)

                idPublicidad = drwPublicidad.IdEnvioPublicidad

                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            Finally

            End Try
        End Function

        Private Function AgregaDetallePublicidad(ByVal dtbCliente As DataTable, _
                                                 ByVal intIdPublicidad As Integer, _
                                                 ByRef dtbDetallePublicidad As PublicidadEnvioDataset.SCGTA_TB_DetalleEnvioPublicidadDataTable) As Boolean
            Dim drwDestinatarios As DataRow
            Dim drwPublicidadDetalle As PublicidadEnvioDataset.SCGTA_TB_DetalleEnvioPublicidadRow


            Try
                For Each drwDestinatarios In dtbCliente.Rows

                    If drwDestinatarios.RowState <> DataRowState.Deleted Then

                        drwPublicidadDetalle = dtbDetallePublicidad.NewSCGTA_TB_DetalleEnvioPublicidadRow

                        drwPublicidadDetalle.idEnvioPublicidad = intIdPublicidad
                        drwPublicidadDetalle.CardCode = drwDestinatarios("CardCode")

                        Call dtbDetallePublicidad.AddSCGTA_TB_DetalleEnvioPublicidadRow(drwPublicidadDetalle)

                    End If

                Next drwDestinatarios
                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            End Try
        End Function

        Private Sub ScgTbClientes_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ScgTbClientes.Click_Buscar
            Try
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                With m_buPublicidad

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorPublicidad
                    .Titulos = My.Resources.ResourceUI.Publicidad & "," & My.Resources.ResourceUI.FechaEnvio & "," & My.Resources.ResourceUI.HoraEnvio & "," & My.Resources.ResourceUI.IDEnvioPublicidad & "," & My.Resources.ResourceUI.Enviado '"Publicidad,Fecha Envio,Hora Envio,IdEnvioPublicidad,enviado"
                    .Criterios = "EtiquetaPublicidad,FechaEnvio,HoraEnvio,IdEnvioPublicidad,enviado"
                    .Criterios_Ocultos = 2
                    .Tabla = "SCGTA_TB_EnvioPublicidad"
                    .MultiSeleccion = False
                    .Where = ""
                    .Activar_Buscador(sender)

                End With


                With ScgTbClientes

                    .Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Nuevo).Enabled = True
                    '.Buttons(.enumButton.Guardar).Enabled = True

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub CargaDatosDePublicidadenForm(ByVal drwEncabezadoPublicidad As PublicidadEnvioDataset.SCGTA_TB_EnvioPublicidadRow, _
                                                      ByVal dtbDetallePublicidad As PublicidadEnvioDataset.SCGTA_TB_DetalleEnvioPublicidadDataTable)
            Try

                txtPublicidad.Text = drwEncabezadoPublicidad.EtiquetaPublicidad
                txtAsunto.Text = drwEncabezadoPublicidad.Asunto
                txtDetalle.Text = drwEncabezadoPublicidad.Detalle
                txtMaterial.Text = drwEncabezadoPublicidad.Material
                dtpFechaCita.Value = drwEncabezadoPublicidad.FechaEnvio
                m_intEnviado = drwEncabezadoPublicidad.Enviado

                dtpHoraEnvio.Value = drwEncabezadoPublicidad.HoraEnvio



                dtgClientes.DataSource = dtbDetallePublicidad

                Call estiloGrid(m_dstPublicidad.SCGTA_TB_DetalleEnvioPublicidad.TableName)

                Call HabilitoControles(False)

                ScgTbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = False

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
            End Try
        End Sub

        Private Sub m_buPublicidad_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_buPublicidad.AppAceptar
            Try

                Call m_dstPublicidad.Clear()

                Call m_adpPublicidad.Fill(m_dstPublicidad, Arreglo_Campos(3), -1)

                ScgTbClientes.Buttons(Proyecto_SCGToolBar.SCGToolBar.enumButton.Guardar).Enabled = True

                Call CargaDatosDePublicidadenForm(CType(m_dstPublicidad.SCGTA_TB_EnvioPublicidad.Rows(0), PublicidadEnvioDataset.SCGTA_TB_EnvioPublicidadRow), _
                                                   m_dstPublicidad.SCGTA_TB_DetalleEnvioPublicidad)


                Call LimpiaErroesDeErrorProvider()

                Call ActivaBotonDeEnvio(dtpFechaCita.Value.Date, System.DateTime.Today, m_intEnviado)

                dtpHoraEnvio.Enabled = True
                dtpFechaCita.Enabled = True


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private sub LimpiarControles()
            Try

                If Not m_dstCliente Is Nothing Then
                    Call m_dstCliente.Clear()
                End If

                If Not m_dstPublicidad Is Nothing Then
                    Call m_dstPublicidad.Clear()
                End If

                Call txtAsunto.Clear()
                Call txtDetalle.Clear()
                Call txtMaterial.Clear()
                Call txtPublicidad.Clear()


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End sub

        Private Function FaltanCamposRequeridos() As Boolean

            Dim blnFaltanCamposRequerido As Boolean = False

            If txtAsunto.Text = "" Then
                Call EPPublicidad.SetError(txtAsunto, My.Resources.ResourceUI.MensajeDebeIngresarAsunto)
                blnFaltanCamposRequerido = True
            Else
                Call EPPublicidad.SetError(txtAsunto, "")
            End If

            If txtDetalle.Text = "" Then
                Call EPPublicidad.SetError(txtDetalle, My.Resources.ResourceUI.MensajeDebeIngresarDetalle)
                blnFaltanCamposRequerido = True
            Else
                Call EPPublicidad.SetError(txtDetalle, "")
            End If

            If txtPublicidad.Text = "" Then
                Call EPPublicidad.SetError(txtPublicidad, My.Resources.ResourceUI.MensajeDebeIngresarNombreCampana)
                blnFaltanCamposRequerido = True
            Else
                Call EPPublicidad.SetError(txtPublicidad, "")
            End If


            If txtMaterial.Text = "" Then
                Call EPPublicidad.SetError(txtMaterial, My.Resources.ResourceUI.MensajeDebeAdjuntarMaterialPublicidad)
                blnFaltanCamposRequerido = True
            Else
                Call EPPublicidad.SetError(txtMaterial, "")
            End If

            If dtpFechaCita.Value <= Date.Today Then
                Call EPPublicidad.SetError(dtpFechaCita, My.Resources.ResourceUI.MensajeFechaEnvioInvalida)
                blnFaltanCamposRequerido = True
            Else
                Call EPPublicidad.SetError(dtpFechaCita, "")
            End If

            'OrElse m_dstCliente.Tables("Table").Rows.Count = 0

            If m_dstCliente.Tables.Count > 0 _
                       AndAlso CantidadDerowsAgregadas(m_dstCliente.Tables(0)) = 0 Then

                Call EPPublicidad.SetError(dtgClientes, My.Resources.ResourceUI.MensajeDebeSeleccionarClienteEnvioPublicidad)
                blnFaltanCamposRequerido = True
            Else
                Call EPPublicidad.SetError(dtgClientes, "")
            End If

            If blnFaltanCamposRequerido Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub LimpiaErroesDeErrorProvider()
            Try
                Call EPPublicidad.SetError(txtDetalle, "")
                Call EPPublicidad.SetError(txtAsunto, "")
                Call EPPublicidad.SetError(txtPublicidad, "")
                Call EPPublicidad.SetError(txtMaterial, "")
                Call EPPublicidad.SetError(dtpFechaCita, "")
                Call EPPublicidad.SetError(dtgClientes, "")
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub ActivaBotonDeEnvio(ByVal datFechaEnvio As Date, _
                                       ByVal datFechaActual As Date, _
                                       ByVal intEnviado As Integer)
            Try



                If (datFechaActual > datFechaEnvio Or datFechaActual < datFechaEnvio) _
                    Or (datFechaActual = Date.Today And intEnviado = 1) Then

                    Button1.Enabled = False
                Else
                    Button1.Enabled = True

                End If



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpFechaCita_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFechaCita.ValueChanged
            Call ActivaBotonDeEnvio(dtpFechaCita.Value.Date, System.DateTime.Today, 0)
        End Sub

        
      
    End Class
End Namespace

