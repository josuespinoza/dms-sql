Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmReporteCostosXOrdenInt

        Inherits SCG.UX.Windows.SAP.frmPlantillaSAP

#Region "Incializar pantalla"


        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub


        Public Sub New()

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()


        End Sub

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReporteCostosXOrdenInt))
            Me.optResumido = New System.Windows.Forms.RadioButton()
            Me.optDetallado = New System.Windows.Forms.RadioButton()
            Me.rptReporte = New ComponenteCristalReport.SubReportView()
            Me.gbxRangoFechas = New System.Windows.Forms.GroupBox()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
            Me.lblLine1 = New System.Windows.Forms.Label()
            Me.lblLine2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.btncerrar = New System.Windows.Forms.Button()
            Me.btnCargar = New System.Windows.Forms.Button()
            Me.gbxTipo_Rep = New System.Windows.Forms.GroupBox()
            Me.gbx_TipoCosto = New System.Windows.Forms.GroupBox()
            Me.rbt_Real = New System.Windows.Forms.RadioButton()
            Me.rbt_Estandar = New System.Windows.Forms.RadioButton()
            Me.gbxRangoFechas.SuspendLayout()
            Me.gbxTipo_Rep.SuspendLayout()
            Me.gbx_TipoCosto.SuspendLayout()
            Me.SuspendLayout()
            '
            'optResumido
            '
            resources.ApplyResources(Me.optResumido, "optResumido")
            Me.optResumido.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.optResumido.Name = "optResumido"
            Me.optResumido.UseVisualStyleBackColor = True
            '
            'optDetallado
            '
            resources.ApplyResources(Me.optDetallado, "optDetallado")
            Me.optDetallado.Checked = True
            Me.optDetallado.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.optDetallado.Name = "optDetallado"
            Me.optDetallado.TabStop = True
            Me.optDetallado.UseVisualStyleBackColor = True
            '
            'rptReporte
            '
            resources.ApplyResources(Me.rptReporte, "rptReporte")
            Me.rptReporte.BackColor = System.Drawing.Color.White
            Me.rptReporte.Name = "rptReporte"
            Me.rptReporte.P_Authentication = False
            Me.rptReporte.P_BarraTitulo = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_CompanyName = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_DataBase = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_Filename = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_NCopias = 0
            Me.rptReporte.P_Owner = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_ParArray = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_Password = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_Server = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_User = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rptReporte.P_WorkFolder = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'gbxRangoFechas
            '
            resources.ApplyResources(Me.gbxRangoFechas, "gbxRangoFechas")
            Me.gbxRangoFechas.Controls.Add(Me.Panel2)
            Me.gbxRangoFechas.Controls.Add(Me.dtpHasta)
            Me.gbxRangoFechas.Controls.Add(Me.Panel1)
            Me.gbxRangoFechas.Controls.Add(Me.dtpDesde)
            Me.gbxRangoFechas.Controls.Add(Me.lblLine1)
            Me.gbxRangoFechas.Controls.Add(Me.lblLine2)
            Me.gbxRangoFechas.Controls.Add(Me.Label3)
            Me.gbxRangoFechas.Controls.Add(Me.Label4)
            Me.gbxRangoFechas.Name = "gbxRangoFechas"
            Me.gbxRangoFechas.TabStop = False
            '
            'Panel2
            '
            resources.ApplyResources(Me.Panel2, "Panel2")
            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.Panel2.Name = "Panel2"
            '
            'dtpHasta
            '
            resources.ApplyResources(Me.dtpHasta, "dtpHasta")
            Me.dtpHasta.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpHasta.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpHasta.Name = "dtpHasta"
            Me.dtpHasta.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
            '
            'Panel1
            '
            resources.ApplyResources(Me.Panel1, "Panel1")
            Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(222, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(206, Byte), Integer))
            Me.Panel1.Name = "Panel1"
            '
            'dtpDesde
            '
            resources.ApplyResources(Me.dtpDesde, "dtpDesde")
            Me.dtpDesde.CalendarForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarMonthBackground = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleBackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.dtpDesde.CalendarTitleForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpDesde.Name = "dtpDesde"
            Me.dtpDesde.Value = New Date(2006, 7, 12, 0, 0, 0, 0)
            '
            'lblLine1
            '
            resources.ApplyResources(Me.lblLine1, "lblLine1")
            Me.lblLine1.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.lblLine1.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblLine1.Name = "lblLine1"
            '
            'lblLine2
            '
            resources.ApplyResources(Me.lblLine2, "lblLine2")
            Me.lblLine2.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.lblLine2.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.lblLine2.Name = "lblLine2"
            '
            'Label3
            '
            resources.ApplyResources(Me.Label3, "Label3")
            Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label3.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label3.Name = "Label3"
            '
            'Label4
            '
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Label4.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.Label4.Name = "Label4"
            '
            'btncerrar
            '
            resources.ApplyResources(Me.btncerrar, "btncerrar")
            Me.btncerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btncerrar.ForeColor = System.Drawing.Color.Black
            Me.btncerrar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btncerrar.Name = "btncerrar"
            '
            'btnCargar
            '
            resources.ApplyResources(Me.btnCargar, "btnCargar")
            Me.btnCargar.ForeColor = System.Drawing.Color.Black
            Me.btnCargar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.btnCargar.Name = "btnCargar"
            '
            'gbxTipo_Rep
            '
            resources.ApplyResources(Me.gbxTipo_Rep, "gbxTipo_Rep")
            Me.gbxTipo_Rep.Controls.Add(Me.optResumido)
            Me.gbxTipo_Rep.Controls.Add(Me.optDetallado)
            Me.gbxTipo_Rep.Name = "gbxTipo_Rep"
            Me.gbxTipo_Rep.TabStop = False
            '
            'gbx_TipoCosto
            '
            resources.ApplyResources(Me.gbx_TipoCosto, "gbx_TipoCosto")
            Me.gbx_TipoCosto.Controls.Add(Me.rbt_Real)
            Me.gbx_TipoCosto.Controls.Add(Me.rbt_Estandar)
            Me.gbx_TipoCosto.Name = "gbx_TipoCosto"
            Me.gbx_TipoCosto.TabStop = False
            '
            'rbt_Real
            '
            resources.ApplyResources(Me.rbt_Real, "rbt_Real")
            Me.rbt_Real.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rbt_Real.Name = "rbt_Real"
            Me.rbt_Real.UseVisualStyleBackColor = True
            '
            'rbt_Estandar
            '
            resources.ApplyResources(Me.rbt_Estandar, "rbt_Estandar")
            Me.rbt_Estandar.Checked = True
            Me.rbt_Estandar.ImageKey = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.rbt_Estandar.Name = "rbt_Estandar"
            Me.rbt_Estandar.TabStop = True
            Me.rbt_Estandar.UseVisualStyleBackColor = True
            '
            'frmReporteCostosXOrdenInt
            '
            resources.ApplyResources(Me, "$this")
            Me.Controls.Add(Me.gbx_TipoCosto)
            Me.Controls.Add(Me.gbxTipo_Rep)
            Me.Controls.Add(Me.rptReporte)
            Me.Controls.Add(Me.gbxRangoFechas)
            Me.Controls.Add(Me.btncerrar)
            Me.Controls.Add(Me.btnCargar)
            Me.Name = "frmReporteCostosXOrdenInt"
            Me.gbxRangoFechas.ResumeLayout(False)
            Me.gbxTipo_Rep.ResumeLayout(False)
            Me.gbxTipo_Rep.PerformLayout()
            Me.gbx_TipoCosto.ResumeLayout(False)
            Me.gbx_TipoCosto.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents optResumido As System.Windows.Forms.RadioButton
        Friend WithEvents optDetallado As System.Windows.Forms.RadioButton
        Friend WithEvents rptReporte As ComponenteCristalReport.SubReportView
        Friend WithEvents gbxRangoFechas As System.Windows.Forms.GroupBox
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents dtpHasta As System.Windows.Forms.DateTimePicker
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents dtpDesde As System.Windows.Forms.DateTimePicker
        Public WithEvents lblLine1 As System.Windows.Forms.Label
        Public WithEvents lblLine2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents btncerrar As System.Windows.Forms.Button
        Friend WithEvents btnCargar As System.Windows.Forms.Button
        Friend WithEvents gbxTipo_Rep As System.Windows.Forms.GroupBox
        Friend WithEvents gbx_TipoCosto As System.Windows.Forms.GroupBox
        Friend WithEvents rbt_Real As System.Windows.Forms.RadioButton
        Friend WithEvents rbt_Estandar As System.Windows.Forms.RadioButton
#End Region

#Region "Métodos"



        Private Sub frmReporteOTxFecha_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            dtpDesde.Value = Today.Date
            dtpHasta.Value = Today.Date
        End Sub

        ''' <summary>
        ''' Genera un reporte entre las fechas indicadas por el usuario
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub btnCargar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCargar.Click
            Dim strParametros As String = ""
            Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion
            Dim strTipoCosto As String
            Dim dtFechaInicio As Date
            Dim dtFechaFinal As Date

            Try

                If rbt_Real.Checked = True Then

                    strTipoCosto = "1"

                Else
                    strTipoCosto = "2"

                End If

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                dtFechaInicio = Date.Parse(dtpDesde.Value.ToString)
                dtFechaFinal = Date.Parse(DateAdd(DateInterval.Second, -1, DateAdd(DateInterval.Day, 1, dtpHasta.Value)).ToString)

                strParametros = DMSOneFramework.SCGDataAccess.Utilitarios.RetornaFechaFormatoDB(dtFechaInicio, Server, UserSCGInternal, PasswordSCGInternal) & ","
                strParametros = strParametros & DMSOneFramework.SCGDataAccess.Utilitarios.RetornaFechaFormatoDB(dtFechaFinal, Server, UserSCGInternal, PasswordSCGInternal) & ","

                strParametros = strParametros & strTipoCosto.Trim

                With rptReporte
                    .P_WorkFolder = PATH_REPORTES

                    If optDetallado.Checked = True Then
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloCostoPorOTInt_Detallado
                        .P_Filename = My.Resources.ResourceUI.rptNombreCostoPorOTInt_Detallado
                    Else
                        .P_BarraTitulo = My.Resources.ResourceUI.rptTituloCostoPorOTInt_Resumido
                        .P_Filename = My.Resources.ResourceUI.rptNombreCostoPorOTInt_Resumido
                    End If

                    .P_Server = Server
                    .P_DataBase = strDATABASESCG
                    .P_CompanyName = COMPANIA
                    .P_User = UserSCGInternal
                    .P_Password = Password
                    .P_ParArray = strParametros


                End With

                rptReporte.VerReporte()
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btncerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btncerrar.Click
            Me.Close()

        End Sub
      

#End Region


    End Class

End Namespace

