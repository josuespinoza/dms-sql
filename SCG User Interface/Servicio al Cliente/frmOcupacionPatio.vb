Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports system.Collections.Generic
Imports System.Data.SqlClient

Namespace SCG_User_Interface

    Public Class frmOcupacionPatio
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
        Friend WithEvents btnSiguienteDay As System.Windows.Forms.Button
        Friend WithEvents TTButtons As System.Windows.Forms.ToolTip
        Friend WithEvents TTCita As System.Windows.Forms.ToolTip
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOcupacionPatio))
            Me.dtgOcupacion = New System.Windows.Forms.DataGrid
            Me.dtpFecha = New System.Windows.Forms.DateTimePicker
            Me.lblFecha = New System.Windows.Forms.Label
            Me.Label14 = New System.Windows.Forms.Label
            Me.TTButtons = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnSiguienteDay = New System.Windows.Forms.Button
            Me.btnAnteriorDay = New System.Windows.Forms.Button
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
            Me.dtgOcupacion.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None
            Me.dtgOcupacion.HeaderBackColor = System.Drawing.Color.White
            Me.dtgOcupacion.HeaderFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.dtgOcupacion.HeaderForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(106, Byte), Integer))
            Me.dtgOcupacion.Name = "dtgOcupacion"
            Me.dtgOcupacion.ParentRowsBackColor = System.Drawing.Color.Silver
            Me.dtgOcupacion.PreferredRowHeight = 25
            Me.dtgOcupacion.RowHeadersVisible = False
            Me.dtgOcupacion.Tag = Global.SCG_User_Interface.My.Resources.ResourceUI.Quotation
            '
            'dtpFecha
            '
            resources.ApplyResources(Me.dtpFecha, "dtpFecha")
            Me.dtpFecha.Name = "dtpFecha"
            Me.dtpFecha.Value = New Date(2006, 5, 17, 0, 0, 0, 0)
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
            Me.btnSiguienteDay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.btnSiguienteDay.Name = "btnSiguienteDay"
            Me.btnSiguienteDay.Tag = "1"
            Me.TTCita.SetToolTip(Me.btnSiguienteDay, resources.GetString("btnSiguienteDay.ToolTip"))
            Me.TTButtons.SetToolTip(Me.btnSiguienteDay, resources.GetString("btnSiguienteDay.ToolTip1"))
            '
            'btnAnteriorDay
            '
            resources.ApplyResources(Me.btnAnteriorDay, "btnAnteriorDay")
            Me.btnAnteriorDay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.btnAnteriorDay.Name = "btnAnteriorDay"
            Me.btnAnteriorDay.Tag = "-1"
            Me.TTCita.SetToolTip(Me.btnAnteriorDay, resources.GetString("btnAnteriorDay.ToolTip"))
            Me.TTButtons.SetToolTip(Me.btnAnteriorDay, resources.GetString("btnAnteriorDay.ToolTip1"))
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
            'frmOcupacionPatio
            '
            resources.ApplyResources(Me, "$this")
            Me.BackColor = System.Drawing.SystemColors.Control
            Me.CancelButton = Me.btnCerrar
            Me.Controls.Add(Me.dtpFecha)
            Me.Controls.Add(Me.Label14)
            Me.Controls.Add(Me.btnSiguienteDay)
            Me.Controls.Add(Me.btnAnteriorDay)
            Me.Controls.Add(Me.Panel10)
            Me.Controls.Add(Me.lblFecha)
            Me.Controls.Add(Me.btnCerrar)
            Me.Controls.Add(Me.dtgOcupacion)
            Me.Controls.Add(Me.btnActualizar)
            Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
            Me.MaximizeBox = False
            Me.Name = "frmOcupacionPatio"
            Me.Tag = "Servicio al Cliente,1"
            CType(Me.dtgOcupacion, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)
            MyBase.New()

            InitializeComponent()


        End Sub

#End Region

#Region "Declaraciones"

        Private m_dstOcupacion As New OcupacionDataset
        Private m_blnDobleClick As Boolean = False
        Private m_datFechaSeleccionada As Date
        Private m_adtRampas As New RampasDataAdapter
        Dim m_OldCell As DataGridCell
        Dim m_strTextoInfo As String = ""
        Private objUtilitarios As New Utilitarios(strConectionString)
        'Declaraciones usadas para ver el detalle de las citas

        'Declaracion de objeto dataAdapter y Dataset.
        Private m_adpCita As SCGDataAccess.CitasDataAdapter
        Private m_dstCita As CitasDataset
        Private m_datHoraInicio As Date
        Private m_datHoraFin As Date
        Private m_intRango As Integer = 30
        Private m_datFecha As Date
        Private m_dtbOcupacion As OcupacionDataset.SCGTA_TB_OcupacionDataTable
        Private m_intWidth As Integer = 0
        Private m_intHeight As Integer = 0
        Private m_strRampa As String
        Public Event e_SeleccionarOcupacion()


#End Region

#Region "Propiedades"

        Public ReadOnly Property EsDobleClick() As Boolean

            Get

                Return m_blnDobleClick

            End Get

        End Property

        Public ReadOnly Property FechaSeleccionada() As Date

            Get

                Return m_datFechaSeleccionada

            End Get

        End Property

        Public ReadOnly Property Rampa() As String

            Get

                Return m_strRampa

            End Get

        End Property

#End Region

#Region "Constantes"

        Private Const mc_strTableName As String = ""

        Private mc_strID As String = My.Resources.ResourceUI.ID
        Private mc_strHora As String = My.Resources.ResourceUI.HoraCalendario
        Private mc_strHoraColumna As String = "Hora"

        Private mc_strDescripcion As String = "Descripcion"
        Private mc_strDescripción As String = My.Resources.ResourceUI.Descripcion
        Private mc_strNoOrden As String = "NoOrden" 'My.Resources.ResourceUI.NoOrden

        Private mc_strFieldNoOrden As String = "NoOrden"

#End Region

#Region "Procedimientos"

        Private Sub EstiloGrid()

            Const intWithDateCol As Integer = 60
            Dim tsEstiloGrid As DataGridTableStyle
            Dim a_scRampas As New List(Of DataGridCitaColumn)
            Dim scRampa As New DataGridCitaColumn
            Dim drdRampas As SqlDataReader
            'Dim objColumna As System.Data.DataColumn
            Dim intCantidadColumnas = 0


            Dim scID As DataGridLabelColumn
            Dim scHora As DataGridHoraColumn
            Dim scHoraConFecha As DataGridColumnDate

            m_datFecha = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, 23, 59, 59)
            drdRampas = CargarDatosRampas()
            tsEstiloGrid = New DataGridTableStyle

            tsEstiloGrid.MappingName = m_dtbOcupacion.TableName

            scID = New DataGridLabelColumn
            With scID
                .HeaderText = ""
                .MappingName = mc_strID
                .Width = 0
            End With

            scHora = New DataGridHoraColumn
            With scHora
                .HeaderText = mc_strHora
                .MappingName = mc_strHoraColumna
                .Width = 60
            End With

            scHoraConFecha = New DataGridColumnDate
            With scHoraConFecha
                .HeaderText = ""
                .MappingName = "HoraConFecha"
                .Width = 0
            End With

            Do While drdRampas.Read
                scRampa = New DataGridCitaColumn
                m_dtbOcupacion.Columns.Add(drdRampas.Item(mc_strDescripcion))
                With scRampa
                    .HeaderText = drdRampas.Item(mc_strDescripcion)
                    .MappingName = drdRampas.Item(mc_strDescripcion)
                    .Width = intWithDateCol * 2
                    .NullText = ""
                End With
                intCantidadColumnas = intCantidadColumnas + 1
                a_scRampas.Add(scRampa)
            Loop
            If Not drdRampas.IsClosed Then
                drdRampas.Close()
            End If
            tsEstiloGrid.GridColumnStyles.Add(scID)
            tsEstiloGrid.GridColumnStyles.Add(scHora)
            tsEstiloGrid.GridColumnStyles.Add(scHoraConFecha)

            If intCantidadColumnas < 8 Then
                m_intWidth = 70
            Else
                m_intWidth = 0
            End If
            For Each scRampa In a_scRampas
                tsEstiloGrid.GridColumnStyles.Add(scRampa)
                If intCantidadColumnas < 8 Then
                    m_intWidth += scRampa.Width + 10
                End If
            Next

            tsEstiloGrid.PreferredRowHeight = 10
            tsEstiloGrid.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
            tsEstiloGrid.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
            tsEstiloGrid.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            'tsEstiloGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
            tsEstiloGrid.GridLineStyle = DataGridLineStyle.None
            tsEstiloGrid.RowHeadersVisible = False
            tsEstiloGrid.AllowSorting = False
            dtgOcupacion.TableStyles.Clear()
            dtgOcupacion.TableStyles.Add(tsEstiloGrid)
            If Not drdRampas.IsClosed Then
                drdRampas.Close()
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

                        If m_dtbOcupacion.Rows.Count - 1 >= objHTI.Row Then
                            If dtgOcupacion.Item(objHTI.Row, objHTI.Column) IsNot DBNull.Value Then

                                m_strTextoInfo = CStr(dtgOcupacion.Item(objHTI.Row, objHTI.Column))

                            Else

                                m_strTextoInfo = ""

                            End If
                        End If


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

        Private Sub CargarHorarioProduccion()

            Dim objDA As DMSOneFramework.SCGDataAccess.Utilitarios
            Dim drdDatosHorario As SqlClient.SqlDataReader = Nothing
            'Dim strHoraInicio As String
            'Dim strHoraFin As String
            Dim strRango As String = ""

            Try

                objDA = New DMSOneFramework.SCGDataAccess.Utilitarios(DMSOneFramework.SCGDataAccess.DAConexion.ConnectionString)
                drdDatosHorario = objDA.CargaValoresHorarios()
                objDA.CargaValorRango(strRango)

                m_intRango = IIf(IsNumeric(strRango), CInt(strRango), 30)

                If drdDatosHorario.Read Then

                    With drdDatosHorario
                        m_datHoraInicio = CDate(drdDatosHorario.Item("FechaIni"))
                        m_datHoraFin = CDate(drdDatosHorario.Item("FechaFin"))

                    End With

                    drdDatosHorario.Close()

                Else

                    drdDatosHorario.Close()

                    m_datHoraInicio = New Date(1900, 1, 1, 0, 0, 0)
                    m_datHoraFin = New Date(1900, 1, 1, 0, 0, 0)

                End If

            Catch ex As Exception
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

        Private Sub CargarHorasPorRango()



            Dim datFechaInicioCiclo As Date
            Dim datFechaFinCiclo As Date
            Dim intContador As Integer
            Dim drwRampa As OcupacionDataset.SCGTA_TB_OcupacionRow
            Call CargarHorarioProduccion()
            m_datHoraInicio = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, m_datHoraInicio.Hour, m_datHoraInicio.Minute, m_datHoraInicio.Second)
            m_datHoraFin = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, m_datHoraFin.Hour, m_datHoraFin.Minute, m_datHoraFin.Second)

            datFechaInicioCiclo = m_datHoraInicio
            datFechaFinCiclo = m_datHoraFin

            intContador = 1
            m_dtbOcupacion.Rows.Clear()
            Do While datFechaInicioCiclo <= datFechaFinCiclo

                drwRampa = m_dtbOcupacion.NewSCGTA_TB_OcupacionRow
                drwRampa.ID = intContador
                drwRampa.Hora = datFechaInicioCiclo.ToShortTimeString
                drwRampa.HoraConFecha = datFechaInicioCiclo
                intContador += 1
                datFechaInicioCiclo = datFechaInicioCiclo.AddMinutes(m_intRango)
                m_dtbOcupacion.AddSCGTA_TB_OcupacionRow(drwRampa)
            Loop

        End Sub

        Private Sub CargarDatosOcupacion()

            If m_dtbOcupacion IsNot Nothing Then
                m_dtbOcupacion.Dispose()
                m_dtbOcupacion = Nothing
            End If
            m_dtbOcupacion = New OcupacionDataset.SCGTA_TB_OcupacionDataTable
            m_datFecha = dtpFecha.Value
            Call CargarHorasPorRango()

            EstiloGrid()
            CargarOrdenesAsignadas()
            dtgOcupacion.DataSource = m_dtbOcupacion
            If m_intWidth = 0 Then
                m_intWidth = (Me.MdiParent.ClientSize.Width * 95) / 100
            End If
            m_intHeight = (Me.MdiParent.ClientSize.Height * 95) / 100

            Me.Size = New Size(m_intWidth, m_intHeight)
            Me.Top = 0
            Me.Left = 0

        End Sub

        Private Function CargarDatosRampas() As SqlDataReader
            Dim drdRampas As SqlDataReader
            drdRampas = m_adtRampas.Fill(m_datFecha)
            Return drdRampas
        End Function

        Private Sub CargarOrdenesAsignadas()
            Dim drdOcupacion As SqlDataReader = Nothing
            Dim drwOcupacion As OcupacionDataset.SCGTA_TB_OcupacionRow
            Dim strOrdenes As String
            Try

                drdOcupacion = m_adtRampas.GetRampasOcupadas(m_datHoraInicio, m_datHoraFin, m_intRango)
                Do While drdOcupacion.Read
                    drwOcupacion = m_dtbOcupacion.FindByID(drdOcupacion.Item(mc_strID))
                    If drwOcupacion.Item(drdOcupacion.Item(mc_strDescripcion)) Is DBNull.Value Then
                        drwOcupacion.Item(drdOcupacion.Item(mc_strDescripcion)) = drdOcupacion.Item(mc_strFieldNoOrden)
                    Else
                        strOrdenes = drwOcupacion.Item(drdOcupacion.Item(mc_strDescripcion)).ToString
                        If Not strOrdenes.Contains(drdOcupacion.Item(mc_strNoOrden)) Then
                            drwOcupacion.Item(drdOcupacion.Item(mc_strDescripcion)) = strOrdenes + vbCrLf + drdOcupacion.Item(mc_strNoOrden)
                        End If
                    End If
                Loop
                If Not drdOcupacion.IsClosed Then
                    drdOcupacion.Close()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'objSCGMSGBox.msgExclamationCustom(ex.Message)
            Finally
                'Agregado 01072010
                If drdOcupacion IsNot Nothing Then
                    If Not drdOcupacion.IsClosed Then
                        Call drdOcupacion.Close()
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


            Try

                dtpFecha.Value = Now.Date
                CargarDatosOcupacion()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click

            Try

                CargarDatosOcupacion()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnNavegar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorDay.Click, btnSiguienteDay.Click

            dtpFecha.Value = dtpFecha.Value.AddDays(CDbl(CType(sender, Button).Tag))

            Call CargarDatosOcupacion()

        End Sub

        Private Sub dtpFecha_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFecha.CloseUp
            Try

                Call CargarDatosOcupacion()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtpFecha_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dtpFecha.KeyUp
            Try

                If e.KeyCode = Keys.Enter Then

                    Call CargarDatosOcupacion()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub dtgOcupacion_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgOcupacion.DoubleClick
            Dim strHora As String
            Dim intFila As Integer
            'Dim intMinutos As Integer
            'Dim intHora As Integer
            'Dim strAMoPM As String
            Dim cllCelda As DataGridCell
            cllCelda = dtgOcupacion.CurrentCell()

            intFila = cllCelda.RowNumber
            m_datFechaSeleccionada = dtgOcupacion.Item(intFila, 2)
            'intHora = CInt(strHora.Substring(0, 2))
            'intMinutos = CInt(strHora.Substring(3, 2))
            'strAMoPM = strHora.Substring(6, 4)
            'If strAMoPM = "p.m." Then
            '    If intHora < 12 Then
            '        intHora = intHora + 12
            '    End If
            'End If
            strHora = mc_strHora
            m_strRampa = dtgOcupacion.TableStyles(0).GridColumnStyles(cllCelda.ColumnNumber).HeaderText
            m_blnDobleClick = True
            'm_datFechaSeleccionada = New Date(dtpFecha.Value.Year, dtpFecha.Value.Month, dtpFecha.Value.Day, intHora, intMinutos, 0)

            RaiseEvent e_SeleccionarOcupacion()

            'Me.Close()
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

    End Class

End Namespace

