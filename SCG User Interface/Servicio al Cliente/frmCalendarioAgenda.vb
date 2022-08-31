Imports Deklarit
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports Sunisoft.IrisSkin
Imports SCG.SkinManager

Namespace SCG_User_Interface

    Public Class frmCalendarioAgenda
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
        Friend WithEvents btnActualizar As System.Windows.Forms.Button
        Friend WithEvents btnCerrar As System.Windows.Forms.Button
        Friend WithEvents Panel10 As System.Windows.Forms.Panel
        Friend WithEvents dtpFecha As System.Windows.Forms.DateTimePicker
        Friend WithEvents lblFecha As System.Windows.Forms.Label
        Public WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents dtgOcupacion As System.Windows.Forms.DataGrid
        Friend WithEvents btnAnteriorDay As System.Windows.Forms.Button
        Friend WithEvents btnSiguienteWeek As System.Windows.Forms.Button
        Friend WithEvents btnAnteriorWeek As System.Windows.Forms.Button
        Friend WithEvents btnSiguienteDay As System.Windows.Forms.Button
        Friend WithEvents TTButtons As System.Windows.Forms.ToolTip
        Friend WithEvents cboAgenda As SCGComboBox.SCGComboBox
        Public WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents lblAgenda As System.Windows.Forms.Label
        Friend WithEvents TTCita As System.Windows.Forms.ToolTip
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCalendarioAgenda))
            Me.dtgOcupacion = New System.Windows.Forms.DataGrid
            Me.dtpFecha = New System.Windows.Forms.DateTimePicker
            Me.lblFecha = New System.Windows.Forms.Label
            Me.Label14 = New System.Windows.Forms.Label
            Me.TTButtons = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnSiguienteDay = New System.Windows.Forms.Button
            Me.btnAnteriorWeek = New System.Windows.Forms.Button
            Me.btnSiguienteWeek = New System.Windows.Forms.Button
            Me.btnAnteriorDay = New System.Windows.Forms.Button
            Me.cboAgenda = New SCGComboBox.SCGComboBox
            Me.Label4 = New System.Windows.Forms.Label
            Me.lblAgenda = New System.Windows.Forms.Label
            Me.Panel10 = New System.Windows.Forms.Panel
            Me.btnActualizar = New System.Windows.Forms.Button
            Me.btnCerrar = New System.Windows.Forms.Button
            Me.TTCita = New System.Windows.Forms.ToolTip(Me.components)
            CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'dtgOcupacion
            '
            resources.ApplyResources(Me.dtgOcupacion, "dtgOcupacion")
            Me.dtgOcupacion.BackgroundColor = System.Drawing.Color.White
            Me.dtgOcupacion.CaptionVisible = False
            Me.dtgOcupacion.DataMember = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            Me.dtgOcupacion.GridLineColor = System.Drawing.Color.Silver
            Me.dtgOcupacion.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
            Me.dtgOcupacion.HeaderBackColor = System.Drawing.Color.White
            Me.dtgOcupacion.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgOcupacion.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgOcupacion.Name = "dtgOcupacion"
            Me.dtgOcupacion.PreferredRowHeight = 25
            Me.dtgOcupacion.RowHeadersVisible = False
            Me.dtgOcupacion.Tag = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'dtpFecha
            '
            resources.ApplyResources(Me.dtpFecha, "dtpFecha")
            Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
            Me.dtpFecha.Name = "dtpFecha"
            Me.dtpFecha.Value = New Date(2007, 3, 8, 0, 0, 0, 0)
            '
            'lblFecha
            '
            resources.ApplyResources(Me.lblFecha, "lblFecha")
            Me.lblFecha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblFecha.Name = "lblFecha"
            '
            'Label14
            '
            Me.Label14.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label14, "Label14")
            Me.Label14.Name = "Label14"
            '
            'btnSiguienteDay
            '
            resources.ApplyResources(Me.btnSiguienteDay, "btnSiguienteDay")
            Me.btnSiguienteDay.ForeColor = System.Drawing.Color.Black
            Me.btnSiguienteDay.Name = "btnSiguienteDay"
            Me.btnSiguienteDay.Tag = "1"
            Me.TTCita.SetToolTip(Me.btnSiguienteDay, resources.GetString("btnSiguienteDay.ToolTip"))
            Me.TTButtons.SetToolTip(Me.btnSiguienteDay, resources.GetString("btnSiguienteDay.ToolTip1"))
            '
            'btnAnteriorWeek
            '
            resources.ApplyResources(Me.btnAnteriorWeek, "btnAnteriorWeek")
            Me.btnAnteriorWeek.ForeColor = System.Drawing.Color.Black
            Me.btnAnteriorWeek.Name = "btnAnteriorWeek"
            Me.btnAnteriorWeek.Tag = "-7"
            Me.TTCita.SetToolTip(Me.btnAnteriorWeek, resources.GetString("btnAnteriorWeek.ToolTip"))
            Me.TTButtons.SetToolTip(Me.btnAnteriorWeek, resources.GetString("btnAnteriorWeek.ToolTip1"))
            '
            'btnSiguienteWeek
            '
            resources.ApplyResources(Me.btnSiguienteWeek, "btnSiguienteWeek")
            Me.btnSiguienteWeek.ForeColor = System.Drawing.Color.Black
            Me.btnSiguienteWeek.Name = "btnSiguienteWeek"
            Me.btnSiguienteWeek.Tag = "7"
            Me.TTCita.SetToolTip(Me.btnSiguienteWeek, resources.GetString("btnSiguienteWeek.ToolTip"))
            Me.TTButtons.SetToolTip(Me.btnSiguienteWeek, resources.GetString("btnSiguienteWeek.ToolTip1"))
            '
            'btnAnteriorDay
            '
            resources.ApplyResources(Me.btnAnteriorDay, "btnAnteriorDay")
            Me.btnAnteriorDay.ForeColor = System.Drawing.Color.Black
            Me.btnAnteriorDay.Name = "btnAnteriorDay"
            Me.btnAnteriorDay.Tag = "-1"
            Me.TTCita.SetToolTip(Me.btnAnteriorDay, resources.GetString("btnAnteriorDay.ToolTip"))
            Me.TTButtons.SetToolTip(Me.btnAnteriorDay, resources.GetString("btnAnteriorDay.ToolTip1"))
            '
            'cboAgenda
            '
            Me.cboAgenda.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            Me.cboAgenda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAgenda.EstiloSBO = True
            resources.ApplyResources(Me.cboAgenda, "cboAgenda")
            Me.cboAgenda.Name = "cboAgenda"
            '
            'Label4
            '
            Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(243, Byte), Integer))
            resources.ApplyResources(Me.Label4, "Label4")
            Me.Label4.Name = "Label4"
            '
            'lblAgenda
            '
            resources.ApplyResources(Me.lblAgenda, "lblAgenda")
            Me.lblAgenda.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.lblAgenda.Name = "lblAgenda"
            '
            'Panel10
            '
            resources.ApplyResources(Me.Panel10, "Panel10")
            Me.Panel10.Name = "Panel10"
            '
            'btnActualizar
            '
            resources.ApplyResources(Me.btnActualizar, "btnActualizar")
            Me.btnActualizar.ForeColor = System.Drawing.Color.Black
            Me.btnActualizar.Name = "btnActualizar"
            '
            'btnCerrar
            '
            resources.ApplyResources(Me.btnCerrar, "btnCerrar")
            Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCerrar.ForeColor = System.Drawing.Color.Black
            Me.btnCerrar.Name = "btnCerrar"
            '
            'frmCalendarioAgenda
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.Controls.Add(Me.Label14)
            Me.Controls.Add(Me.cboAgenda)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.dtgOcupacion)
            Me.Controls.Add(Me.btnActualizar)
            Me.Controls.Add(Me.lblFecha)
            Me.Controls.Add(Me.btnAnteriorDay)
            Me.Controls.Add(Me.Panel10)
            Me.Controls.Add(Me.dtpFecha)
            Me.Controls.Add(Me.btnSiguienteWeek)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.lblAgenda)
            Me.Controls.Add(Me.btnSiguienteDay)
            Me.Controls.Add(Me.btnAnteriorWeek)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.Name = "frmCalendarioAgenda"
            Me.Tag = "Servicio al Cliente,1"
            CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            InitializeComponent()
            m_blnEstadoForma = p_blnEstado

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean, _
                       ByVal p_datFecha As Date, _
                       ByVal p_strNombreAgenda As String)

            MyBase.New()

            InitializeComponent()
            m_dtFecha = p_datFecha
            m_blnEstadoForma = p_blnEstado
            m_strNombreAgenda = p_strNombreAgenda

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean, _
                       ByVal p_datFecha As Date, _
                       ByVal p_strNombreAgenda As String, ByVal p_blnInterno As Boolean)

            MyBase.New()

            InitializeComponent()
            m_dtFecha = p_datFecha
            m_blnEstadoForma = p_blnEstado
            m_strNombreAgenda = p_strNombreAgenda
            m_blnInterno = p_blnInterno

        End Sub

#End Region

#Region "Declaraciones"

        Dim m_dstOcupacion As New CalendarioAgendaDataset
        Private m_OldCell As DataGridCell
        Private m_strTextoInfo As String = ""

        Private m_blnEstadoForma As Boolean

        Private objUtilitarios As New Utilitarios(strConectionString)

        Private m_drdAgendas As SqlClient.SqlDataReader
        Private m_adpAgendas As New AgendaDataAdapter
        Private m_strNombreAgenda As String
        Private m_dtFecha As Date = Nothing
        Private m_intIntervaloCitas As Integer
        Private m_blnEjecutarEvento As Boolean = False
        'Declaraciones usadas para ver el detalle de las citas
        Private WithEvents objfrmCita As frmDetalleCita
        Public Event eFechaYHoraSeleccionada(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer)

        Private m_datHoraInicio As Date
        Private m_datHoraFin As Date

        Private m_blnInterno As Boolean = False

        Private _skinEngine As SkinEngine

#End Region

#Region "Constantes"

        Private Const mc_strTableName As String = ""

        Private Const mc_strID As String = "ID"
        Private Const mc_strHora As String = "Hora"
        Private Const mc_strDia1 As String = "Dia1"
        Private Const mc_strDia2 As String = "Dia2"
        Private Const mc_strDia3 As String = "Dia3"
        Private Const mc_strDia4 As String = "Dia4"
        Private Const mc_strDia5 As String = "Dia5"
        Private Const mc_strDia6 As String = "Dia6"
        Private Const mc_strDia7 As String = "Dia7"

#End Region

#Region "Procedimientos"

        Private Sub LoadConsultaOcupacion()

            m_dstOcupacion.Dispose()

            m_dstOcupacion = New CalendarioAgendaDataset

            Call CargarHorarioProduccion()
            Call Load24Horas(m_dstOcupacion)

            With m_dstOcupacion.SCGTA_TB_Ocupacion.DefaultView
                .AllowDelete = False
                .AllowEdit = False
                .AllowNew = False
            End With

            dtgOcupacion.DataSource = m_dstOcupacion.SCGTA_TB_Ocupacion

        End Sub

        Private Sub LoadEstiloGrid()
            Const intWithDateCol As Integer = 60
            Dim tsEstiloGrid As DataGridTableStyle

            Dim scID As DataGridLabelColumn
            Dim scHora As DataGridHoraColumn
            Dim scDia1 As DataGridCitaAgendaColumn
            Dim scDia2 As DataGridCitaAgendaColumn
            Dim scDia3 As DataGridCitaAgendaColumn
            Dim scDia4 As DataGridCitaAgendaColumn
            Dim scDia5 As DataGridCitaAgendaColumn
            Dim scDia6 As DataGridCitaAgendaColumn
            Dim scDia7 As DataGridCitaAgendaColumn

            tsEstiloGrid = New DataGridTableStyle

            tsEstiloGrid.MappingName = m_dstOcupacion.SCGTA_TB_Ocupacion.TableName

            scID = New DataGridLabelColumn
            With scID
                .HeaderText = ""
                .MappingName = mc_strID
                .Width = 0
            End With

            scHora = New DataGridHoraColumn
            With scHora
                .HeaderText = My.Resources.ResourceUI.Hora
                .MappingName = mc_strHora
                .Width = 63
            End With

            scDia1 = New DataGridCitaAgendaColumn
            With scDia1
                .HeaderText = GetTitleCol(0)
                .MappingName = mc_strDia1
                .Width = intWithDateCol
            End With

            scDia2 = New DataGridCitaAgendaColumn
            With scDia2
                .HeaderText = GetTitleCol(1)
                .MappingName = mc_strDia2
                .Width = intWithDateCol
            End With

            scDia3 = New DataGridCitaAgendaColumn
            With scDia3
                .HeaderText = GetTitleCol(2)
                .MappingName = mc_strDia3
                .Width = intWithDateCol
            End With

            scDia4 = New DataGridCitaAgendaColumn
            With scDia4
                .HeaderText = GetTitleCol(3)
                .MappingName = mc_strDia4
                .Width = intWithDateCol
            End With

            scDia5 = New DataGridCitaAgendaColumn
            With scDia5
                .HeaderText = GetTitleCol(4)
                .MappingName = mc_strDia5
                .Width = intWithDateCol
            End With

            scDia6 = New DataGridCitaAgendaColumn
            With scDia6
                .HeaderText = GetTitleCol(5)
                .MappingName = mc_strDia6
                .Width = intWithDateCol
            End With

            scDia7 = New DataGridCitaAgendaColumn
            With scDia7
                .HeaderText = GetTitleCol(6)
                .MappingName = mc_strDia7
                .Width = intWithDateCol
            End With

            tsEstiloGrid.GridColumnStyles.Add(scID)
            tsEstiloGrid.GridColumnStyles.Add(scHora)
            tsEstiloGrid.GridColumnStyles.Add(scDia1)
            tsEstiloGrid.GridColumnStyles.Add(scDia2)
            tsEstiloGrid.GridColumnStyles.Add(scDia3)
            tsEstiloGrid.GridColumnStyles.Add(scDia4)
            tsEstiloGrid.GridColumnStyles.Add(scDia5)
            tsEstiloGrid.GridColumnStyles.Add(scDia6)
            tsEstiloGrid.GridColumnStyles.Add(scDia7)

            tsEstiloGrid.PreferredRowHeight = 10
            tsEstiloGrid.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsEstiloGrid.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsEstiloGrid.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            'tsEstiloGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsEstiloGrid.GridLineStyle = DataGridLineStyle.None
            tsEstiloGrid.RowHeadersVisible = False
            tsEstiloGrid.AllowSorting = False

            dtgOcupacion.TableStyles.Add(tsEstiloGrid)

        End Sub

        Private Function GetTitleCol(ByVal p_intCol As Integer) As String

            Return WeekdayName(Weekday(dtpFecha.Value.AddDays(p_intCol)), True, FirstDayOfWeek.Sunday) & " - " & MonthName(Month(dtpFecha.Value.AddDays(p_intCol)), True) & " - " & CStr(dtpFecha.Value.AddDays(p_intCol).Day)

        End Function

        Private Sub Load24Horas(ByRef p_dstOcupacion As CalendarioAgendaDataset)
            'Dim intMinHours As Integer = 7
            'Dim intMaxHours As Integer = 19
            'Dim intMinutos As Integer = 0
            Dim dtFechaActual As Date
            Dim dtFechaFinal As Date

            Dim intContHours As Integer = 1
            Dim drwFilaCita As CalendarioAgendaDataset.SCGTA_TB_OcupacionRow
            'Dim dtHora As Date
            dtFechaActual = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, m_datHoraInicio.Hour, m_datHoraInicio.Minute, 0)
            dtFechaFinal = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, m_datHoraFin.Hour, m_datHoraFin.Minute, 0)
            'For intContHours = intMinHours To intMaxHours
            '    intMinutos = 0
            '    Do While intMinutos < 60
            '        ''Hora cero minutos
            '        drwFilaCita = p_dstOcupacion.SCGTA_TB_Ocupacion.NewSCGTA_TB_OcupacionRow

            '        dtHora = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, intContHours, intMinutos, 0)

            '        LoadRowTime(intContHours, dtHora, drwFilaCita)

            '        p_dstOcupacion.SCGTA_TB_Ocupacion.Rows.Add(drwFilaCita)
            '        intMinutos += m_intIntervaloCitas
            '    Loop
            ' ''Hora 30 minutos
            'drwFilaCita = p_dstOcupacion.SCGTA_TB_Ocupacion.NewSCGTA_TB_OcupacionRow

            'dtHora = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, intContHours, 30, 0)

            'LoadRowTime(intContHours, dtHora, drwFilaCita)

            'p_dstOcupacion.SCGTA_TB_Ocupacion.Rows.Add(drwFilaCita)

            'Next

            Do While dtFechaActual <= dtFechaFinal

                drwFilaCita = p_dstOcupacion.SCGTA_TB_Ocupacion.NewSCGTA_TB_OcupacionRow
                LoadRowTime(intContHours, dtFechaActual, drwFilaCita)
                p_dstOcupacion.SCGTA_TB_Ocupacion.Rows.Add(drwFilaCita)
                dtFechaActual = dtFechaActual.AddMinutes(m_intIntervaloCitas)
                intContHours += 1
            Loop

        End Sub

        Private Sub LoadRowTime(ByVal p_intContHours As Integer, ByVal p_dtFecha As Date, ByRef p_drwFilaCita As CalendarioAgendaDataset.SCGTA_TB_OcupacionRow)

            With p_drwFilaCita
                .ID = p_intContHours
                .HORA = p_dtFecha.ToShortTimeString
                .DIA1 = ObtenerInfoCita(p_dtFecha, 0)
                .DIA2 = ObtenerInfoCita(p_dtFecha, 1)
                .DIA3 = ObtenerInfoCita(p_dtFecha, 2)
                .DIA4 = ObtenerInfoCita(p_dtFecha, 3)
                .DIA5 = ObtenerInfoCita(p_dtFecha, 4)
                .DIA6 = ObtenerInfoCita(p_dtFecha, 5)
                .DIA7 = ObtenerInfoCita(p_dtFecha, 6)
            End With

        End Sub

        Private Function ObtenerInfoCita(ByVal p_dtFecha As Date, ByVal p_intDia As Integer) As String
            Dim adpOcupacion As New SCGDataAccess.CalendarioAgendaDataAdapter
            Dim strCodigos As String

            strCodigos = adpOcupacion.GetCodsCitas(p_dtFecha.AddDays(p_intDia), CInt(cboAgenda.SelectedValue))

            Return strCodigos
        End Function

        Private Sub CambiarTamanioColums()
            Const intGridSize As Integer = 504
            Const intDaySize As Integer = 60
            Const intHoraSize As Integer = 63

            Dim intResult As Integer
            Dim intContDay As Integer

            intResult = Math.Round((intHoraSize * dtgOcupacion.Size.Width) / intGridSize)
            dtgOcupacion.TableStyles(0).GridColumnStyles("Hora").Width = intResult

            For intContDay = 2 To 8
                intResult = Math.Round((intDaySize * dtgOcupacion.Size.Width) / intGridSize)
                dtgOcupacion.TableStyles(0).GridColumnStyles("Dia" & CStr(intContDay - 1)).Width = intResult
            Next

        End Sub

        Private Sub ActualizarEstilo()
            If dtgOcupacion.TableStyles.Count > 0 Then
                With dtgOcupacion.TableStyles(0).GridColumnStyles
                    .Item(mc_strDia1).HeaderText = GetTitleCol(0)
                    .Item(mc_strDia2).HeaderText = GetTitleCol(1)
                    .Item(mc_strDia3).HeaderText = GetTitleCol(2)
                    .Item(mc_strDia4).HeaderText = GetTitleCol(3)
                    .Item(mc_strDia5).HeaderText = GetTitleCol(4)
                    .Item(mc_strDia6).HeaderText = GetTitleCol(5)
                    .Item(mc_strDia7).HeaderText = GetTitleCol(6)
                End With
            End If

        End Sub

        Private Sub ShowToolTipInfo(ByVal p_objPoint As Point)
            Dim objHTI As DataGrid.HitTestInfo
            Dim CurrentCell As DataGridCell

            objHTI = dtgOcupacion.HitTest(p_objPoint)

            If objHTI.Type = DataGrid.HitTestType.Cell Then
                If objHTI.Column > 1 Then

                    CurrentCell.ColumnNumber = objHTI.Column
                    CurrentCell.RowNumber = objHTI.Row

                    If CurrentCell.ColumnNumber <> m_OldCell.ColumnNumber Or _
                        CurrentCell.RowNumber <> m_OldCell.RowNumber Then

                        m_strTextoInfo = LoadClienteInto(CStr(dtgOcupacion.Item(objHTI.Row, objHTI.Column)))

                    End If

                    TTCita.SetToolTip(dtgOcupacion, m_strTextoInfo)

                Else

                    TTCita.SetToolTip(dtgOcupacion, "")

                End If

                m_OldCell.ColumnNumber = objHTI.Column
                m_OldCell.RowNumber = objHTI.Row

            Else

                TTCita.SetToolTip(dtgOcupacion, "")

            End If

        End Sub

        Private Function LoadClienteInto(ByVal p_strCodigos As String) As String
            Dim adpClientFromCitas As SCGDataAccess.CalendarioAgendaDataAdapter
            Dim strCodigosArray() As String
            Dim strCodigoCita As String = ""
            Dim strCardCode As String = ""
            Dim strCardName As String = ""
            Dim dtHoraIni As Date
            Dim dtHoraFin As Date

            Dim strTextoResult As String = ""

            If p_strCodigos <> "" Then

                adpClientFromCitas = New SCGDataAccess.CalendarioAgendaDataAdapter

                strCodigosArray = p_strCodigos.Split(",")

                For Each strCodigoCita In strCodigosArray

                    adpClientFromCitas.GetInfoClientesFromCitas(strCodigoCita, strCardCode, strCardName, dtHoraIni, dtHoraFin)

                    strTextoResult &= Chr(13) & My.Resources.ResourceUI.Cliente & strCardCode & "  -  " & strCardName & " " & My.Resources.ResourceUI.MensajeDeLas & " " & dtHoraIni.ToShortTimeString & " " & My.Resources.ResourceUI.MensajeALas & " " & dtHoraFin.ToShortTimeString

                Next

                strTextoResult &= Chr(13)

            End If

            Return strTextoResult

        End Function

        Private Sub CargarCitasModoModificar(ByVal p_strNoCita As String)

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            'Dim datFechaYHora As Date
            Dim dstCitas As New CitasDataset
            Dim adpCitas As New CitasDataAdapter
            Dim drwCita As CitasDataset.SCGTA_TB_CitasRow

            Try
                If objfrmCita IsNot Nothing Then
                    objfrmCita.Dispose()
                    objfrmCita = Nothing
                End If
                For Each Forma_Nueva In Me.MdiParent.MdiChildren

                    If Forma_Nueva.Name = "frmDetalleCita" Then
                        blnExisteForm = True
                    End If

                Next

                If Not blnExisteForm Then
                    adpCitas.Fill(dstCitas, Nothing, Nothing, p_strNoCita)
                    If dstCitas.SCGTA_TB_Citas.Rows.Count > 0 Then

                        drwCita = dstCitas.SCGTA_TB_Citas.Rows(0)
                        If drwCita.FechayHora >= objUtilitarios.CargarFechaHoraServidor() Then
                            objfrmCita = New frmDetalleCita(2, drwCita)
                        Else
                            objfrmCita = New frmDetalleCita(3, drwCita)
                        End If

                        With objfrmCita

                            .MdiParent = Me.MdiParent
                            .Show()

                        End With
                    End If

                End If

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw ex

            End Try

        End Sub

        Private Sub CargarCitasModoNuevo()

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean
            Dim datFechaYHora As Date
            Dim intCodigoAgenda As Integer
            Dim strNombreAgeda As String

            Try
                If objfrmCita IsNot Nothing Then

                    objfrmCita.Dispose()
                    objfrmCita = Nothing

                End If
                For Each Forma_Nueva In Me.MdiParent.MdiChildren

                    If Forma_Nueva.Name = "frmDetalleCita" Then
                        blnExisteForm = True
                    End If

                Next

                If Not blnExisteForm Then

                    objfrmCita = New frmDetalleCita(1)
                    ObtenerFechaHoraCita(datFechaYHora)
                    intCodigoAgenda = cboAgenda.SelectedValue
                    strNombreAgeda = cboAgenda.SelectedItem.Descripcion

                    With objfrmCita

                        .MdiParent = Me.MdiParent
                        .Show()
                        .EstablecerValoresCita(datFechaYHora, strNombreAgeda, intCodigoAgenda)

                    End With
                End If

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try
        End Sub

        Private Sub ObtenerFechaHoraCita(ByRef p_datFechaYHora As Date)

            Dim datFecha As Date
            Dim datHora As Date
            Dim intDias As Integer


            Try
                If dtgOcupacion.CurrentRowIndex <> -1 Then

                    intDias = dtgOcupacion.CurrentCell.ColumnNumber
                    datFecha = dtpFecha.Value.AddDays((intDias - 2))
                    datHora = CDate("01-01-1900" & " " & dtgOcupacion.Item(dtgOcupacion.CurrentRowIndex, 1))

                    datFecha = New Date(datFecha.Year, datFecha.Month, datFecha.Day, datHora.Hour, datHora.Minute, 0)

                    p_datFechaYHora = datFecha

                End If

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub CargarHorarioProduccion()

            Dim drdDatosHorario As SqlClient.SqlDataReader = Nothing
            'Dim strHoraInicio As String
            'Dim strHoraFin As String
            'Dim strRango As String

            Try

                drdDatosHorario = objUtilitarios.CargaValoresHorarios()

                If drdDatosHorario.Read Then

                    With drdDatosHorario
                        m_datHoraInicio = CDate(drdDatosHorario.Item("FechaIni"))
                        m_datHoraFin = CDate(drdDatosHorario.Item("FechaFin"))

                    End With

                    drdDatosHorario.Close()

                Else

                    drdDatosHorario.Close()

                    m_datHoraInicio = New Date(1900, 1, 1, 7, 0, 0)
                    m_datHoraFin = New Date(1900, 1, 1, 19, 0, 0)

                End If

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 02072010
                If drdDatosHorario IsNot Nothing Then
                    If Not drdDatosHorario.IsClosed Then
                        Call drdDatosHorario.Close()
                    End If
                End If
            End Try

        End Sub

#End Region

#Region "Eventos"

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
            Me.Close()
        End Sub

        Private Sub frmOcupacionPatio_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim intWidth As Integer
            Dim intHeight As Integer

            Try

                If m_dtFecha = Nothing Then
                    dtpFecha.Value = objUtilitarios.CargarFechaHoraServidor
                Else
                    dtpFecha.Value = m_dtFecha
                End If
                Call m_adpAgendas.Fill(m_drdAgendas)
                m_blnEjecutarEvento = False
                Call Utilitarios.CargarComboSourceByReader(cboAgenda, m_drdAgendas)
                m_blnEjecutarEvento = True
                m_intIntervaloCitas = 0
                Call m_adpAgendas.Fill(m_drdAgendas)
                If m_drdAgendas.Read() Then
                    m_intIntervaloCitas = m_drdAgendas.GetInt32(3)
                End If
                If m_intIntervaloCitas = 0 Then
                    m_intIntervaloCitas = 15
                End If
                If m_strNombreAgenda <> "" Then
                    cboAgenda.Text = m_strNombreAgenda
                End If

                LoadEstiloGrid()

                LoadConsultaOcupacion()

                If Not m_blnInterno Then
                    intWidth = (Me.MdiParent.ClientSize.Width * 95) / 100
                    intHeight = (Me.MdiParent.ClientSize.Height * 95) / 100

                    Me.Size = New Size(intWidth, intHeight)
                    Me.Top = 0
                    Me.Left = 0
                    Me.MaximizeBox = False
                Else                    
                    cargarSkin()
                    Me.Size = New Size(800, 600)
                    Me.Top = 3
                    'Me.Left = 0
                    'Me.MaximizeBox = True
                    'Me.MinimizeBox = False
                End If


                
            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                'Agregado
                If m_drdAgendas IsNot Nothing Then
                    If Not m_drdAgendas.IsClosed Then
                        Call m_drdAgendas.Close()
                    End If
                End If
            End Try
        End Sub

        Private Sub dtgOcupacion_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgOcupacion.Resize
            Try
                If dtgOcupacion.TableStyles.Count <> 0 Then
                    CambiarTamanioColums()
                End If
            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
            Try

                ActualizarEstilo()

                LoadConsultaOcupacion()

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnNavegar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorWeek.Click, btnAnteriorDay.Click, btnSiguienteWeek.Click, btnSiguienteDay.Click

            dtpFecha.Value = dtpFecha.Value.AddDays(CDbl(CType(sender, Button).Tag))

            ActualizarEstilo()

            LoadConsultaOcupacion()

        End Sub

        Private Sub dtpFecha_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFecha.CloseUp
            Try

                ActualizarEstilo()

                LoadConsultaOcupacion()

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpFecha_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpFecha.KeyUp
            Try

                If e.KeyCode = Keys.Enter Then

                    ActualizarEstilo()

                    LoadConsultaOcupacion()

                End If

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnActualizarOcupacion_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            'Permite fijar la ocupación máxima de vehículos que puede tener el patio
            Try

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgOcupacion_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgOcupacion.DoubleClick

            Dim strCitas As String
            Dim strCantCitas As String()
            Dim intCantCitas As Integer
            Dim datFechaYHora As Date
            Dim intCodigoAgenda As Integer
            Dim strNombreAgenda As String

            Try

                If dtgOcupacion.CurrentRowIndex <> -1 Then

                    strCitas = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
                    strCantCitas = strCitas.Split(",")
                    intCantCitas = strCantCitas.Length

                    If strCitas = "" Then  'Cuando no hay cita para esa hora

                        If Not m_blnEstadoForma Then

                            CargarCitasModoNuevo()

                        Else

                            ObtenerFechaHoraCita(datFechaYHora)
                            intCodigoAgenda = cboAgenda.SelectedValue
                            strNombreAgenda = cboAgenda.SelectedItem.Descripcion

                            RaiseEvent eFechaYHoraSeleccionada(datFechaYHora, strNombreAgenda, intCodigoAgenda)

                        End If

                    ElseIf intCantCitas = 1 Then 'Cuando solo hay una cita para esa hora
                        If Not m_blnInterno Then

                            CargarCitasModoModificar(strCitas)

                        End If

                    End If '1 Cita
                End If 'Validacion grid

            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try


        End Sub

        Private Sub objfrmCita_eDatosGuardados(ByVal strNumeroCita As String) Handles objfrmCita.eDatosGuardados

            Call ActualizarEstilo()
            Call LoadConsultaOcupacion()

            objfrmCita.Hide()

        End Sub

        Private Sub dtgOcupacion_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dtgOcupacion.MouseMove
            Try

                ShowToolTipInfo(New Point(e.X, e.Y))

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

#End Region

        Private Sub cboAgenda_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAgenda.SelectedValueChanged
            Try
                If m_blnEjecutarEvento Then
                    m_intIntervaloCitas = 0
                    Call m_adpAgendas.Fill(m_drdAgendas)
                    Do While m_drdAgendas.Read()
                        If cboAgenda.SelectedValue = m_drdAgendas.GetInt32(0) Then
                            m_intIntervaloCitas = m_drdAgendas.GetInt32(3)
                            Exit Do
                        End If
                    Loop
                    If m_intIntervaloCitas = 0 Then
                        m_intIntervaloCitas = 15
                    End If
                    Call btnActualizar_Click(sender, e)
                End If
            Catch ex As System.Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex

            Finally
                'Agregado 01072010
                If m_drdAgendas IsNot Nothing Then
                    If Not m_drdAgendas.IsClosed Then
                        Call m_drdAgendas.Close()
                    End If
                End If
            End Try
        End Sub

        Private Sub cargarSkin()

            Dim ruta As String = Application.StartupPath
            Dim rutaSkin As String = ruta & "\Skins.xml"            

            If System.IO.File.Exists(rutaSkin) Then
                _skinEngine = New Sunisoft.IrisSkin.SkinEngine(CType(Me, System.ComponentModel.Component))
                _skinEngine.__DrawButtonFocusRectangle = True
                _skinEngine.DisabledButtonTextColor = System.Drawing.Color.Gray
                _skinEngine.DisabledMenuFontColor = System.Drawing.SystemColors.GrayText
                _skinEngine.InactiveCaptionColor = System.Drawing.SystemColors.InactiveCaptionText
                _skinEngine.SerialNumber = "U8XxhWQ7f0vz2ZCQ0R/Zoar2JJsDYOIzWNjdiqqfm9x4rZSajGGoJQ=="
                _skinEngine.SkinFile = Nothing

                Dim oSkinManager As New SCG.SkinManager.Skin(_skinEngine)

                oSkinManager.CargarConfiguracionXml(rutaSkin)
                oSkinManager.CargarSkin(rutaSkin, "SAP 8.8")
                '_skinEngine.Active = True
                '_skinEngine.SkinAllForm = True                
            End If

        End Sub

        Private Sub frmCalendarioAgenda_MaximizedBoundsChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MaximizedBoundsChanged

        End Sub
    End Class

End Namespace

