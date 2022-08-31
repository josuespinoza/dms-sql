Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmConfHorario
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
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents txttotalhoras As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents dtpHInicio As System.Windows.Forms.DateTimePicker
        Friend WithEvents txtRangoRampas As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents lblRangoRampas As System.Windows.Forms.Label
        Friend WithEvents txtTiempoMuerto As NEWTEXTBOX.NEWTEXTBOX_CTRL
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents lblTiemposMuertos As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents dtpHFin As System.Windows.Forms.DateTimePicker
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfHorario))
            Me.btnAceptar = New System.Windows.Forms.Button
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.GroupBox1 = New System.Windows.Forms.GroupBox
            Me.Label9 = New System.Windows.Forms.Label
            Me.txtTiempoMuerto = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label8 = New System.Windows.Forms.Label
            Me.lblTiemposMuertos = New System.Windows.Forms.Label
            Me.txtRangoRampas = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.Label7 = New System.Windows.Forms.Label
            Me.lblRangoRampas = New System.Windows.Forms.Label
            Me.txttotalhoras = New NEWTEXTBOX.NEWTEXTBOX_CTRL
            Me.dtpHFin = New System.Windows.Forms.DateTimePicker
            Me.dtpHInicio = New System.Windows.Forms.DateTimePicker
            Me.Label5 = New System.Windows.Forms.Label
            Me.Label6 = New System.Windows.Forms.Label
            Me.Label3 = New System.Windows.Forms.Label
            Me.Label2 = New System.Windows.Forms.Label
            Me.Label4 = New System.Windows.Forms.Label
            Me.Label1 = New System.Windows.Forms.Label
            Me.GroupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnAceptar
            '
            resources.ApplyResources(Me.btnAceptar, "btnAceptar")
            Me.btnAceptar.ForeColor = System.Drawing.Color.Black
            Me.btnAceptar.Name = "btnAceptar"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.ForeColor = System.Drawing.Color.Black
            Me.btnCerrar.Name = "btnCerrar"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.Label9)
            Me.GroupBox1.Controls.Add(Me.txtTiempoMuerto)
            Me.GroupBox1.Controls.Add(Me.Label8)
            Me.GroupBox1.Controls.Add(Me.lblTiemposMuertos)
            Me.GroupBox1.Controls.Add(Me.txtRangoRampas)
            Me.GroupBox1.Controls.Add(Me.Label7)
            Me.GroupBox1.Controls.Add(Me.lblRangoRampas)
            Me.GroupBox1.Controls.Add(Me.txttotalhoras)
            Me.GroupBox1.Controls.Add(Me.dtpHFin)
            Me.GroupBox1.Controls.Add(Me.dtpHInicio)
            Me.GroupBox1.Controls.Add(Me.Label5)
            Me.GroupBox1.Controls.Add(Me.Label6)
            Me.GroupBox1.Controls.Add(Me.Label3)
            Me.GroupBox1.Controls.Add(Me.Label2)
            Me.GroupBox1.Controls.Add(Me.Label4)
            Me.GroupBox1.Controls.Add(Me.Label1)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'Label9
            '
            Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label9, "Label9")
            Me.Label9.Name = "Label9"
            '
            'txtTiempoMuerto
            '
            Me.txtTiempoMuerto.AceptaNegativos = False
            Me.txtTiempoMuerto.BackColor = System.Drawing.Color.White
            Me.txtTiempoMuerto.Cursor = System.Windows.Forms.Cursors.Default
            Me.txtTiempoMuerto.EstiloSBO = True
            resources.ApplyResources(Me.txtTiempoMuerto, "txtTiempoMuerto")
            Me.txtTiempoMuerto.MaxDecimales = 2
            Me.txtTiempoMuerto.MaxEnteros = 0
            Me.txtTiempoMuerto.Millares = False
            Me.txtTiempoMuerto.Name = "txtTiempoMuerto"
            Me.txtTiempoMuerto.Size_AdjustableHeight = 20
            Me.txtTiempoMuerto.TeclasDeshacer = True
            Me.txtTiempoMuerto.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal
            '
            'Label8
            '
            Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label8, "Label8")
            Me.Label8.Name = "Label8"
            '
            'lblTiemposMuertos
            '
            resources.ApplyResources(Me.lblTiemposMuertos, "lblTiemposMuertos")
            Me.lblTiemposMuertos.Name = "lblTiemposMuertos"
            '
            'txtRangoRampas
            '
            Me.txtRangoRampas.AceptaNegativos = False
            Me.txtRangoRampas.BackColor = System.Drawing.Color.White
            Me.txtRangoRampas.Cursor = System.Windows.Forms.Cursors.Default
            Me.txtRangoRampas.EstiloSBO = True
            resources.ApplyResources(Me.txtRangoRampas, "txtRangoRampas")
            Me.txtRangoRampas.MaxDecimales = 2
            Me.txtRangoRampas.MaxEnteros = 0
            Me.txtRangoRampas.Millares = False
            Me.txtRangoRampas.Name = "txtRangoRampas"
            Me.txtRangoRampas.Size_AdjustableHeight = 20
            Me.txtRangoRampas.TeclasDeshacer = True
            Me.txtRangoRampas.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal
            '
            'Label7
            '
            Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label7, "Label7")
            Me.Label7.Name = "Label7"
            '
            'lblRangoRampas
            '
            resources.ApplyResources(Me.lblRangoRampas, "lblRangoRampas")
            Me.lblRangoRampas.Name = "lblRangoRampas"
            '
            'txttotalhoras
            '
            Me.txttotalhoras.AceptaNegativos = False
            Me.txttotalhoras.BackColor = System.Drawing.Color.White
            Me.txttotalhoras.Cursor = System.Windows.Forms.Cursors.Default
            Me.txttotalhoras.EstiloSBO = True
            resources.ApplyResources(Me.txttotalhoras, "txttotalhoras")
            Me.txttotalhoras.MaxDecimales = 2
            Me.txttotalhoras.MaxEnteros = 0
            Me.txttotalhoras.Millares = False
            Me.txttotalhoras.Name = "txttotalhoras"
            Me.txttotalhoras.ReadOnly = True
            Me.txttotalhoras.Size_AdjustableHeight = 20
            Me.txttotalhoras.TeclasDeshacer = True
            Me.txttotalhoras.Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal
            '
            'dtpHFin
            '
            Me.dtpHFin.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHFin.CalendarMonthBackground = System.Drawing.Color.White
            resources.ApplyResources(Me.dtpHFin, "dtpHFin")
            Me.dtpHFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHFin.Name = "dtpHFin"
            Me.dtpHFin.ShowUpDown = True
            Me.dtpHFin.Value = New Date(2005, 12, 6, 13, 51, 0, 0)
            '
            'dtpHInicio
            '
            Me.dtpHInicio.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHInicio.CalendarMonthBackground = System.Drawing.Color.White
            Me.dtpHInicio.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            resources.ApplyResources(Me.dtpHInicio, "dtpHInicio")
            Me.dtpHInicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.dtpHInicio.Name = "dtpHInicio"
            Me.dtpHInicio.ShowUpDown = True
            Me.dtpHInicio.Value = New Date(2005, 12, 6, 13, 51, 0, 0)
            '
            'Label5
            '
            Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label5, "Label5")
            Me.Label5.Name = "Label5"
            '
            'Label6
            '
            resources.ApplyResources(Me.Label6, "Label6")
            Me.Label6.Name = "Label6"
            '
            'Label3
            '
            Me.Label3.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.Name = "Label3"
            '
            'Label2
            '
            Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(231, Byte), Integer))
            resources.ApplyResources(Me.Label2, "Label2")
            Me.Label2.Name = "Label2"
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'Label1
            '
            resources.ApplyResources(Me.Label1, "Label1")
            Me.Label1.Name = "Label1"
            '
            'frmConfHorario
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.Controls.Add(Me.GroupBox1)
            Me.Controls.Add(Me.btnAceptar)
            Me.Controls.Add(Me.btnCerrar)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmConfHorario"
            Me.GroupBox1.ResumeLayout(False)
            Me.GroupBox1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            InitializeComponent()

        End Sub


#End Region

#Region "Declaraciones"

#Region "Constantes"

        Private Const mc_strHoraInicio As String = "HoraInicioProduccion"
        Private Const mc_strHoraFin As String = "HoraFinProduccion"
        Private Const mc_strRangoRampas As String = "RangoRampas"
        Private Const mc_strTotalHorasProduccion As String = "TotalHorasProduccion"

#End Region

#Region "Variables"

        Private m_strHoraInicio As String
        Private m_strHoraFin As String
        Private m_intRangoRampas As Integer
        Private m_dblTotalHoras As Double

#End Region

#Region "Objetos"

        Private m_adtConfiguracion As New ConfiguracionDataAdapter
        Private m_dtbConfiguracion As New ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable

#End Region

#End Region

#Region "Procedimientos"

        Private Sub CargarDatosHorario()
            Dim objDA As DMSOneFramework.SCGDataAccess.Utilitarios
            Dim drdDatosHorario As SqlClient.SqlDataReader = Nothing
            Dim strRango As String = ""

            Try

                objDA = New DMSOneFramework.SCGDataAccess.Utilitarios(DMSOneFramework.SCGDataAccess.DAConexion.ConnectionString)
                m_adtConfiguracion.Fill(m_dtbConfiguracion)
                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(m_dtbConfiguracion, mc_strRangoRampas, strRango)
                m_intRangoRampas = IIf(IsNumeric(strRango), strRango, 30)
                drdDatosHorario = objDA.CargaValoresHorarios

                If drdDatosHorario.Read Then

                    With drdDatosHorario
                        dtpHInicio.Value = CDate(drdDatosHorario.Item("FechaIni"))
                        dtpHFin.Value = CDate(drdDatosHorario.Item("FechaFin"))
                        txttotalhoras.Text = CDbl(drdDatosHorario.Item("TotalHoras"))
                        txtTiempoMuerto.Text = CDbl(drdDatosHorario.Item("TiempoDescanso"))
                    End With

                    drdDatosHorario.Close()
                    txtRangoRampas.Text = m_intRangoRampas


                Else

                    drdDatosHorario.Close()
                    'dtpHInicio.Value = New Date(1900, 1, 1, 8, 0, 0)
                    'dtpHFin.Value = New Date(1900, 1, 1, 18, 0, 0)
                    dtpHInicio.Value = New Date(1900, 1, 1, 8, 0, 0)
                    dtpHFin.Value = New Date(1900, 1, 1, 18, 0, 0)
                    txttotalhoras.Text = 0
                    txtRangoRampas.Text = m_intRangoRampas
                    CalculaRango()
                    ModificarDatosHorario()

                    objDA.CargaValoresHorarios(dtpHInicio.Value, dtpHFin.Value, 0, 0)


                End If
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw
            Finally
                'Agregado 02072010
                If drdDatosHorario IsNot Nothing Then
                    If Not drdDatosHorario.IsClosed Then
                        Call drdDatosHorario.Close()
                    End If
                End If
            End Try
        End Sub

        Private Sub ModificarDatosHorario()

            Dim objDA As DMSOneFramework.SCGDataAccess.Utilitarios
            Dim blnAgregarFilaConfiguracion As Boolean = False

            objDA = New DMSOneFramework.SCGDataAccess.Utilitarios(DMSOneFramework.SCGDataAccess.DAConexion.ConnectionString)
            Dim drdConfiguracionHora As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow

            m_strHoraFin = dtpHFin.Value.ToString
            m_strHoraInicio = dtpHInicio.Value.ToString
            m_dblTotalHoras = CDbl(txttotalhoras.Text)
            m_intRangoRampas = IIf(txtRangoRampas.Text <> 0, txtRangoRampas.Text, 30)

            drdConfiguracionHora = m_dtbConfiguracion.FindByPropiedad(mc_strRangoRampas)
            If drdConfiguracionHora Is Nothing Then
                drdConfiguracionHora = m_dtbConfiguracion.NewSCGTA_TB_ConfiguracionRow
                drdConfiguracionHora.Propiedad = mc_strRangoRampas
                blnAgregarFilaConfiguracion = True
            End If
            drdConfiguracionHora.Valor = m_intRangoRampas
            If blnAgregarFilaConfiguracion Then
                m_dtbConfiguracion.AddSCGTA_TB_ConfiguracionRow(drdConfiguracionHora)
            End If

            m_adtConfiguracion.Update(m_dtbConfiguracion)
            objDA.ModificarValoresHorarios(dtpHInicio.Value, dtpHFin.Value, CDbl(txttotalhoras.Text), CDbl(txtTiempoMuerto.Text))


        End Sub

        Private Sub CalculaRango()
            Dim dblTotalHoras As Double

            dblTotalHoras = DateDiff(DateInterval.Minute, dtpHInicio.Value, dtpHFin.Value)

            txttotalhoras.Text = dblTotalHoras / 60
        End Sub

#End Region

#Region "Eventos"

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Me.Close()
        End Sub

        Private Sub frmConfHorario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                CargarDatosHorario()
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally

            End Try
        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
            Try

                If dtpHInicio.Value < dtpHFin.Value Then
                    ModificarDatosHorario()
                    Me.Close()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeHoraInicioMenorHoraFin)
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpHInicio_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpHInicio.ValueChanged
            Try

                If dtpHInicio.Value > dtpHFin.Value Then
                    txttotalhoras.Text = 0
                Else
                    CalculaRango()
                End If



            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpHFin_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpHFin.ValueChanged
            Try

                If dtpHInicio.Value > dtpHFin.Value Then
                    txttotalhoras.Text = 0
                Else
                    CalculaRango()
                End If

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

#End Region

    End Class

End Namespace