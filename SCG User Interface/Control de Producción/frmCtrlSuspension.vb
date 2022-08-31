Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Class frmCtrlSuspension
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
        Friend WithEvents btnAceptar As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Public WithEvents lblLine3 As System.Windows.Forms.Label
        Friend WithEvents lblObservacion As System.Windows.Forms.Label
        Friend WithEvents txtObservación As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Public WithEvents lblFase As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents cboRazones As SCGComboBox.SCGComboBox
        Public WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
        Friend WithEvents dtpHora As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLineaFin As System.Windows.Forms.Label
        Friend WithEvents chkFecha As System.Windows.Forms.CheckBox
        Friend WithEvents Panel7 As System.Windows.Forms.Panel
        Friend WithEvents lblDescFase As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCtrlSuspension))
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.lblLine3 = New System.Windows.Forms.Label
            Me.lblObservacion = New System.Windows.Forms.Label
            Me.txtObservación = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.lblLine1 = New System.Windows.Forms.Label
            Me.lblFase = New System.Windows.Forms.Label
            Me.lblDescFase = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.cboRazones = New SCGComboBox.SCGComboBox
            Me.Label2 = New System.Windows.Forms.Label
            Me.dtpFecha = New System.Windows.Forms.DateTimePicker
            Me.dtpHora = New System.Windows.Forms.DateTimePicker
            Me.lblLineaFin = New System.Windows.Forms.Label
            Me.chkFecha = New System.Windows.Forms.CheckBox
            Me.Panel7 = New System.Windows.Forms.Panel
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
            Me.btnAceptar.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.ForeColor = System.Drawing.Color.Black
            Me.btnAceptar.Name = "btnAceptar"
            Me.btnAceptar.UseVisualStyleBackColor = False
            '
            'lblLine3
            '
            Me.lblLine3.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.lblLine3, "lblLine3")
            Me.lblLine3.Name = "lblLine3"
            '
            'lblObservacion
            '
            resources.ApplyResources(Me.lblObservacion, "lblObservacion")
            Me.lblObservacion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblObservacion.Name = "lblObservacion"
            '
            'txtObservación
            '
            Me.txtObservación.AceptaNegativos = False
            Me.txtObservación.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.txtObservación.EstiloSBO = True
            resources.ApplyResources(Me.txtObservación, "txtObservación")
            Me.txtObservación.MaxDecimales = 0
            Me.txtObservación.MaxEnteros = 0
            Me.txtObservación.Millares = False
            Me.txtObservación.Name = "txtObservación"
            Me.txtObservación.Size_AdjustableHeight = 20
            Me.txtObservación.TeclasDeshacer = True
            Me.txtObservación.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.AllSimbols
            '
            'lblLine1
            '
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.Name = "lblLine1"
            '
            'lblFase
            '
            resources.ApplyResources(Me.lblFase, "lblFase")
            Me.lblFase.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFase.Name = "lblFase"
            '
            'lblDescFase
            '
            resources.ApplyResources(Me.lblDescFase, "lblDescFase")
            Me.lblDescFase.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblDescFase.Name = "lblDescFase"
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label1.Name = "Label1"
            '
            'cboRazones
            '
            Me.cboRazones.BackColor = System.Drawing.Color.White
            Me.cboRazones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboRazones.EstiloSBO = True
            resources.ApplyResources(Me.cboRazones, "cboRazones")
            Me.cboRazones.Name = "cboRazones"
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.White
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'dtpFecha
            '
            Me.dtpFecha.CustomFormat = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            resources.ApplyResources(Me.dtpFecha, "dtpFecha")
            Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFecha.Name = "dtpFecha"
            '
            'dtpHora
            '
            resources.ApplyResources(Me.dtpHora, "dtpHora")
            Me.dtpHora.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHora.Name = "dtpHora"
            Me.dtpHora.ShowUpDown = True
            '
            'lblLineaFin
            '
            Me.lblLineaFin.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.lblLineaFin, "lblLineaFin")
            Me.lblLineaFin.Name = "lblLineaFin"
            '
            'chkFecha
            '
            resources.ApplyResources(Me.chkFecha, "chkFecha")
            Me.chkFecha.Name = "chkFecha"
            Me.chkFecha.UseVisualStyleBackColor = True
            '
            'Panel7
            '
            resources.ApplyResources(Me.Panel7, "Panel7")
            Me.Panel7.Name = "Panel7"
            '
            'frmCtrlSuspension
            '
            Me.AcceptButton = Me.btnAceptar
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.ControlBox = False
            Me.Controls.Add(Me.Panel7)
            Me.Controls.Add(Me.dtpFecha)
            Me.Controls.Add(Me.dtpHora)
            Me.Controls.Add(Me.lblLineaFin)
            Me.Controls.Add(Me.chkFecha)
            Me.Controls.Add(Me.cboRazones)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.lblLine1)
            Me.Controls.Add(Me.lblDescFase)
            Me.Controls.Add(Me.lblLine3)
            Me.Controls.Add(Me.lblObservacion)
            Me.Controls.Add(Me.txtObservación)
            Me.Controls.Add(Me.lblFase)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.btnAceptar)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmCtrlSuspension"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Declaraciones"

        Private objUtilitarios As New SCGDataAccess.Utilitarios(strConectionString)
        Private m_strNoOrden As String

        Private m_adpSuspensionesxOrden As New SuspensionesxOrdenDataAdapter
        Private m_dstSuspensinesxorden As New SuspensionesxOrdenDataset
        Private m_drdRazonesSuspencion As SqlClient.SqlDataReader
        Private m_adpRazonesSuspencion As New RazonesSuspensionDataAdapter

        Private m_intNofase As Integer
        Private m_strDescFase As String
        Private m_indicador As Integer

        Private m_dtFechaInicio As Date

        Public Event RetornaCodigo(ByVal intCodigo As Integer, ByVal dtFecha As Date)

        Private WithEvents objFrmConfSuspension As frmConfRazonesSuspension


        Private m_blnOk As Boolean
#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        Public Sub New(ByVal NoOrden As String, _
                       ByVal NoFase As Integer, _
                       ByVal DescripcionFase As String, _
                       ByVal p_dtFechaInicio As Date, _
                       Optional ByVal indicador As Integer = 0)

            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()
            m_strNoOrden = NoOrden
            m_intNofase = NoFase
            m_strDescFase = DescripcionFase
            m_indicador = indicador
            m_dtFechaInicio = p_dtFechaInicio
            'Add any initialization after the InitializeComponent() call

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmCtrlSuspension_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                lblDescFase.Text = m_strDescFase
                m_adpRazonesSuspencion.Fill(m_drdRazonesSuspencion)
                Utilitarios.CargarComboSourceByReader(cboRazones, m_drdRazonesSuspencion)
                m_drdRazonesSuspencion.Close()
                dtpFecha.MinDate = m_dtFechaInicio
                dtpHora.MinDate = m_dtFechaInicio
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            Finally
                'Agregado 02072010
                If m_drdRazonesSuspencion IsNot Nothing Then
                    If Not m_drdRazonesSuspencion.IsClosed Then
                        Call m_drdRazonesSuspencion.Close()
                    End If
                End If
            End Try

        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            m_blnOk = False
            Me.Close()
        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Dim intIDSuspension As Integer
            Dim dtFecha As Date

            Try
                If cboRazones.Text <> vbNullString Then
                    If m_indicador <> 1 Then

                        If AgregarSuspensionesxOrden(m_dstSuspensinesxorden.SCGTA_TB_SuspensionesxOrden, _
                                                     m_strNoOrden, _
                                                     m_intNofase, _
                                                     m_strDescFase, _
                                                     txtObservación.Text, _
                                                     System.DateTime.Now, _
                                                     IIf(m_indicador = 0, False, True), _
                                                     cboRazones.SelectedValue) Then

                            intIDSuspension = m_adpSuspensionesxOrden.Update(m_dstSuspensinesxorden)

                            m_blnOk = True

                            'Me.Close()
                        End If
                    End If
                    If chkFecha.Checked Then
                        dtFecha = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, dtpHora.Value.Hour, dtpHora.Value.Minute, 0)
                    Else
                        dtFecha = Nothing
                    End If
                    RaiseEvent RetornaCodigo(intIDSuspension, dtFecha)

                    Me.Close()

                Else
                    Call objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeFaltanCamposRequeridosVerificar)
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
            End Try



        End Sub

        Private Sub picRazonSusp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            'Se agregó el pic el 17/05/06. Alejandra
            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Dim strCodigoRazonSeleccionada As String = ""

            Try


                For Each Forma_Nueva In Me.Owner.MdiParent.MdiChildren
                    If Forma_Nueva.Name = "frmConfRazonesSuspension" Then
                        blnExisteForm = True
                        Exit For
                    End If
                Next


                If Not blnExisteForm Then
                    objFrmConfSuspension = New frmConfRazonesSuspension


                    objFrmConfSuspension.ShowInTaskbar = False

                    'Else
                    'Forma_Nueva.Activate()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub chkFecha_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFecha.CheckedChanged

            dtpFecha.Enabled = chkFecha.Checked
            dtpHora.Enabled = chkFecha.Checked

        End Sub

#End Region

#Region "Metodos"

        Private Function AgregarSuspensionesxOrden(ByRef dtbSuspensionesxOrden As  _
                                                 SuspensionesxOrdenDataset.SCGTA_TB_SuspensionesxOrdenDataTable, _
                                                ByVal p_strNoOrden As String, _
                                                ByVal p_strNoFase As Integer, _
                                                ByVal p_strFaseDesc As String, _
                                                ByVal p_strRazon As String, _
                                                ByVal p_dtFecha As DateTime, _
                                                ByVal p_blnIndividual As Boolean, _
                                                ByVal p_intCodRazon As Integer) As Boolean

            Dim drwSuspensionesxOrden As SuspensionesxOrdenDataset.SCGTA_TB_SuspensionesxOrdenRow

            Try

                drwSuspensionesxOrden = dtbSuspensionesxOrden.NewSCGTA_TB_SuspensionesxOrdenRow

                With drwSuspensionesxOrden

                    .NoOrden = p_strNoOrden
                    .NoFase = p_strNoFase
                    .FaseDesc = p_strFaseDesc
                    .Razon = p_strRazon
                    .Fecha = p_dtFecha
                    .Individual = p_blnIndividual
                    .CodRazon = p_intCodRazon

                End With

                Call dtbSuspensionesxOrden.AddSCGTA_TB_SuspensionesxOrdenRow(drwSuspensionesxOrden)

                Return True

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
                Return False
            Finally

            End Try
        End Function

#End Region

#Region "Propiedades"

        Public Property Ok() As Boolean
            Get
                Return m_blnOk
            End Get
            Set(ByVal Value As Boolean)
                m_blnOk = Value
            End Set
        End Property

        Public Property NoOrden() As String
            Get
                Return m_strNoOrden
            End Get
            Set(ByVal Value As String)
                m_strNoOrden = Value
            End Set
        End Property

        Public Property NoFase() As Integer
            Get
                Return m_intNofase
            End Get
            Set(ByVal Value As Integer)
                m_intNofase = Value
            End Set
        End Property

        Public Property DescFase() As String
            Get
                Return m_strDescFase
            End Get
            Set(ByVal Value As String)
                m_strDescFase = Value
            End Set
        End Property

#End Region

    End Class
End Namespace
