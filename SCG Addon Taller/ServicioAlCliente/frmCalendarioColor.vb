Imports Deklarit
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Collections.Generic
Imports SAPbouiCOM
Imports Sunisoft.IrisSkin
Imports SCG_User_Interface
Imports System.Text
Imports System.Threading
Imports System.Linq


Public Class frmCalendarioColor
    Private m_strNombreAgenda As String
    Private m_strCodAgenda As String
    Private m_dtFecha As Date = Nothing
    Private m_strCodSucursal As String
    Private m_strCodCitaCancel As String
    Private m_blnEjecutarEvento As Boolean = False
    Public m_oCompany As SAPbobsCOM.Company
    Public m_oApplication As SAPbouiCOM.Application
    Public m_blnInterno As Boolean
    Private m_strFormUID As String
    Private m_blnVersion9 As Boolean

    Private Const mc_strID As String = "ID"
    Private Const mc_strHora As String = "Hora"
    Private Const mc_strDia As String = "Dia"
    Private Const mc_strDia1 As String = "Dia1"
    Private Const mc_strDia2 As String = "Dia2"
    Private Const mc_strDia3 As String = "Dia3"
    Private Const mc_strDia4 As String = "Dia4"
    Private Const mc_strDia5 As String = "Dia5"
    Private Const mc_strDia6 As String = "Dia6"
    Private Const mc_strDia7 As String = "Dia7"

    Private Const mc_strUI_Citas As String = "SCGD_CIT"
    Private Const mc_strUI_SuspenderAgenda As String = "SCGD_SDA"

    Private oCeldaActual As DataGridCell
    Private oCeldaPrevia As DataGridCell

    Dim dtAgenda As New System.Data.DataTable
    Dim dtSucursal As System.Data.DataTable
    Dim dtSuspension As System.Data.DataTable

    Dim m_fhaHoraInicio As Date
    Dim m_fhaHoraFin As Date
    Dim m_numIntervalos As Integer

    Dim ListReservas As New List(Of Reservacion)
    Dim ListaColores As New List(Of ColoresAgenda)

    Private listColumsGrid As New List(Of String)
    Private oGestionColor As GestionColor

    Dim m_blnUsaIntevEstandar As Boolean = False

    Private _skinEngine As SkinEngine
    Structure Reservacion
        Public fhaDesde As Date
        Public fhaHasta As Date
        Public intEstado As Integer
    End Structure

    Structure ColoresAgenda
        Public Fila As Integer
        Public Columna As Integer
        Public Color As Color
    End Structure

    Private Enum GestionColor
        RazonCita = 1
        EstadoCita = 2
    End Enum

    Public Event eFechaYHoraSeleccionadaColor(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer)
    Public Event eListaSuspecionesAgendaColor(ByVal p_objListaSuspe As List(Of Reservacion), ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer)


    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Private Sub HandleTimerTick()
        Try
            Timer1.Stop()
            Timer1.Dispose()
            Me.Close()
            m_oApplication.StatusBar.SetText("Ventana agenda cerrada por tiempo de espera...", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Public Sub ObtieneCitasDias(ByRef p_dtAgenda As System.Data.DataTable, ByVal p_strDia As String, ByRef p_intCitas As Integer)
        Try
            Dim strUDF As String

            Select Case UCase(p_strDia)
                Case "1"
                    strUDF = "U_CantCLunes"
                Case "2"
                    strUDF = "U_CantCMartes"
                Case "3"
                    strUDF = "U_CantCMiercoles"
                Case "4"
                    strUDF = "U_CantCJueves"
                Case "5"
                    strUDF = "U_CantCViernes"
                Case "6"
                    strUDF = "U_CantCSabado"
                Case "0"
                    strUDF = "U_CantCDomingo"
            End Select

            With p_dtAgenda.Rows(0)
                If IsDBNull(.Item(strUDF)) OrElse String.IsNullOrEmpty(.Item(strUDF)) OrElse .Item(strUDF) < 0 Then
                    p_intCitas = 0
                Else
                    p_intCitas = .Item(strUDF)
                End If
            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub New(ByVal p_blnEstado As Boolean, ByVal p_datFecha As Date, ByVal p_strNombreAgenda As String, ByVal p_strCodAgenda As String, ByVal p_strCodSucursal As String, ByVal p_strCodCancel As String, ByVal p_blnVersion9 As Boolean, ByVal p_blnInterno As Boolean, ByVal p_oCompany As SAPbobsCOM.Company, ByVal p_oApplication As SAPbouiCOM.Application, Optional ByVal p_strformUID As String = "")

        MyBase.New()
        Try
            m_dtFecha = p_datFecha
            m_strNombreAgenda = p_strNombreAgenda
            m_strCodAgenda = p_strCodAgenda
            m_strCodSucursal = p_strCodSucursal
            m_strCodCitaCancel = p_strCodCancel
            m_oCompany = p_oCompany
            m_blnInterno = p_blnInterno
            m_oApplication = p_oApplication
            m_strFormUID = p_strformUID
            m_blnVersion9 = p_blnVersion9
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)

            InitializeComponent()
            'Obtiene la configuración de color de la sucursal
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_ManageColorBy) Then
                oGestionColor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_ManageColorBy
            Else
                oGestionColor = GestionColor.RazonCita
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try


    End Sub

    Private Sub VerificaCitasDisponibles(ByRef p_dtAgenda As System.Data.DataTable, _
                                     ByVal p_intCitas As Integer, _
                                     ByVal p_strDiaNum As String, _
                                     ByVal p_hraInicio As Date, _
                                     ByVal p_hraFin As Date, _
                                     ByVal p_strDiaCol As String)
        Try
            Dim l_intCitasXDia As Integer
            Dim l_HoraCont As Date
            Dim l_intFila As Integer = 1
            Dim nuevacelda As ColoresAgenda

            Call ObtieneCitasDias(p_dtAgenda, p_strDiaNum, l_intCitasXDia)

            If p_intCitas >= l_intCitasXDia Then
                l_HoraCont = p_hraInicio

                For i As Integer = 0 To dtAgenda.Rows.Count - 1
                    If IsDBNull(dtAgenda.Rows(i)(p_strDiaCol)) OrElse String.IsNullOrEmpty(dtAgenda.Rows(i)(p_strDiaCol)) Then
                        dtAgenda.Rows(i)(p_strDiaCol) = "n/a"
                        nuevacelda.Fila = i
                        nuevacelda.Columna = Replace(p_strDiaCol.ToLower, "dia", "")
                        nuevacelda.Color = System.Drawing.Color.FromArgb(192, 192, 192)
                        ListaColores.Add(nuevacelda)
                    End If
                Next

            End If



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Function FormatoHora(ByVal p_Hora As String) As String
        Try
            Select Case p_Hora.Length
                Case 3
                    p_Hora = "0" & p_Hora
            End Select
            p_Hora = p_Hora.Insert(2, ":")
            Return p_Hora
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Sub CrearTablaAgenda()
        Try
            Dim lCont As Integer = 1

            dtAgenda.Columns.Add(mc_strHora)

            For i As Integer = 0 To 6
                dtAgenda.Columns.Add(mc_strDia & lCont, GetType(String))
                lCont += 1
            Next

            dtAgenda.DefaultView.AllowNew = False

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub LoadEstiloGrid()
        Const intWithDateCol As Integer = 100
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

        ' tsEstiloGrid.MappingName = m_dstOcupacion.SCGTA_TB_Ocupacion.TableName

        scID = New DataGridLabelColumn
        With scID
            .HeaderText = ""
            .MappingName = mc_strID
            .Width = 0
        End With

        scHora = New DataGridHoraColumn
        With scHora
            .HeaderText = My.Resources.Resource.HeaderAgendaHora
            .MappingName = mc_strHora
            .Width = 55
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

        For Each column As String In listColumsGrid

            Dim columnaGrid As DataGridCitaAgendaColumn

            columnaGrid = New DataGridCitaAgendaColumn
            With columnaGrid
                .HeaderText = column
                .MappingName = column
                .Width = intWithDateCol
            End With

            tsEstiloGrid.GridColumnStyles.Add(columnaGrid)

        Next

        tsEstiloGrid.PreferredRowHeight = 10
        tsEstiloGrid.SelectionBackColor = System.Drawing.Color.FromArgb(CType(253, Byte), CType(208, Byte), CType(44, Byte))
        tsEstiloGrid.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        tsEstiloGrid.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        tsEstiloGrid.GridLineStyle = DataGridLineStyle.None
        tsEstiloGrid.RowHeadersVisible = False
        tsEstiloGrid.AllowSorting = False

        dtgOcupacion.MultiSelect = False
        dtgOcupacion.DataSource = Nothing
        dtgOcupacion.Rows.Clear()
        dtgOcupacion.Columns.Clear()


    End Sub

    Private Function GetTitleCol(ByVal p_intCol As Integer) As String

        Return WeekdayName(Weekday(dtpFecha.Value.AddDays(p_intCol)), True, FirstDayOfWeek.Sunday) & " - " & MonthName(Month(dtpFecha.Value.AddDays(p_intCol)), True) & " - " & CStr(dtpFecha.Value.AddDays(p_intCol).Day)

    End Function

    Public Sub CargarAgenda()
        Dim l_intCont As Integer = 0
        Dim l_strSQLAgenda As String
        Dim l_strSQLSucursal As String


        Dim l_strIntervalo As String
        Dim l_fhaHoraCita As Date
        Dim l_oRow As DataRow

        l_strSQLSucursal = String.Format("SELECT U_HoraInicio, U_HoraFin, U_UsaDurEC FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = {0}", m_strCodSucursal)
        dtSucursal = Utilitarios.EjecutarConsultaDataTable(l_strSQLSucursal, m_oCompany.CompanyDB, m_oCompany.Server)

        If dtSucursal.Rows.Count <> 0 Then

            If String.IsNullOrEmpty(dtSucursal.Rows(0)("U_HoraInicio")) Or
                String.IsNullOrEmpty(dtSucursal.Rows(0)("U_HoraFin")) Then
                m_fhaHoraInicio = "1900-01-01 08:00"
                m_fhaHoraFin = "1900-01-01 18:00"
            Else
                m_fhaHoraInicio = DateTime.Parse("1900-01-01" & " " & FormatoHora(dtSucursal.Rows(0)("U_HoraInicio")))
                m_fhaHoraFin = DateTime.Parse("1900-01-01" & " " & FormatoHora(dtSucursal.Rows(0)("U_HoraFin")))
            End If

            If dtSucursal.Rows(0)("U_UsaDurEC") = "Y" Then
                m_blnUsaIntevEstandar = True
            Else
                m_blnUsaIntevEstandar = False
            End If
        End If

        If m_blnUsaIntevEstandar Then
            l_strIntervalo = "15"
            m_numIntervalos = Integer.Parse(l_strIntervalo)
        Else
            l_strSQLAgenda = String.Format("SELECT U_IntervaloCitas FROM [dbo].[@SCGD_AGENDA] WHERE DocNum = '{0}'  AND U_Cod_Sucursal = '{1}'", m_strCodAgenda, m_strCodSucursal)
            l_strIntervalo = Utilitarios.EjecutarConsulta(l_strSQLAgenda, m_oCompany.CompanyDB, m_oCompany.Server)
            m_numIntervalos = Integer.Parse(l_strIntervalo)
        End If


        Try
            l_intCont = 1
            l_fhaHoraCita = m_fhaHoraInicio

            While l_fhaHoraCita <= m_fhaHoraFin
                l_oRow = dtAgenda.NewRow()
                With l_oRow
                    .Item(mc_strHora) = String.Format(String.Format("{0:HH:mm}", l_fhaHoraCita))
                    .Item(mc_strDia1) = String.Empty
                    .Item(mc_strDia2) = String.Empty
                    .Item(mc_strDia3) = String.Empty
                    .Item(mc_strDia4) = String.Empty
                    .Item(mc_strDia5) = String.Empty
                    .Item(mc_strDia6) = String.Empty
                    .Item(mc_strDia7) = String.Empty
                End With
                dtAgenda.Rows.Add(l_oRow)
                l_fhaHoraCita = l_fhaHoraCita.AddMinutes(l_strIntervalo)
            End While

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Sub LlenarReservasion()
        Try
            Dim l_strSQLAgenda As String
            Dim l_strSQLConfig As String
            Dim l_strSQLSuspension As String
            Dim l_strIntervalo As String

            Dim l_fhaInicio As Date
            Dim l_fhaFinal As Date
            Dim l_fhaTemp As Date

            Dim l_dtSuspension As System.Data.DataTable
            Dim l_dtAgenda As System.Data.DataTable
            Dim l_dtConfing As System.Data.DataTable
            Dim l_oDataRow As DataRow()

            Dim l_numDias As Integer = 1
            Dim l_numFila As Integer = 0
            Dim l_numCitas As Integer = 0

            Dim l_HoraCont As Date
            Dim l_HoraInicio As Date
            Dim l_HoraFin As Date

            Dim l_blnUsaIntevEstandar As Boolean = False

            l_HoraInicio = m_fhaHoraInicio
            l_HoraFin = m_fhaHoraFin

            l_strSQLAgenda = " SELECT " & _
                            " U_Agenda, U_EstadoLogico, U_IntervaloCitas, U_Abreviatura, U_CodAsesor, U_CodTecnico, U_RazonCita, U_ArticuloCita, U_VisibleWeb, U_CantCLunes, " & _
                            " U_CantCMartes, U_CantCMiercoles, U_CantCJueves, U_CantCViernes, U_CantCSabado, U_CantCDomingo, U_Num_Art, U_Num_Razon, U_Cod_Sucursal, U_NameAsesor, U_NameTecnico " & _
                            " FROM dbo.[@SCGD_AGENDA] " & _
                            " WHERE DocEntry = '{0}' AND U_Cod_Sucursal = '{1}'"

            l_strSQLConfig = " SELECT U_Sucurs, U_HoraFin, U_UsaDurEC " & _
                            "  FROM [@SCGD_CONF_SUCURSAL] " & _
                            "  WHERE U_Sucurs = '{0}'"


            l_strSQLAgenda = String.Format(l_strSQLAgenda, m_strCodAgenda, m_strCodSucursal)
            l_strSQLConfig = String.Format(l_strSQLConfig, m_strCodSucursal)

            l_dtConfing = Utilitarios.EjecutarConsultaDataTable(l_strSQLConfig, m_oCompany.CompanyDB, m_oCompany.Server)
            l_dtAgenda = Utilitarios.EjecutarConsultaDataTable(l_strSQLAgenda, m_oCompany.CompanyDB, m_oCompany.Server)

            If l_dtConfing.Rows.Count > 0 Then
                If l_dtConfing.Rows(0)("U_UsaDurEC") = "Y" Then
                    l_strIntervalo = "15"
                    l_blnUsaIntevEstandar = True
                Else
                    If l_dtAgenda.Rows.Count <> 0 Then
                        With l_dtAgenda.Rows(0)
                            l_strIntervalo = .Item("U_IntervaloCitas")
                            l_blnUsaIntevEstandar = False
                        End With
                    Else
                        l_strIntervalo = "15"
                    End If

                End If
            End If

            l_HoraInicio = FormatDateTime(l_HoraInicio, DateFormat.ShortTime)
            l_HoraCont = FormatDateTime(l_HoraInicio, DateFormat.ShortTime)
            l_HoraFin = FormatDateTime(l_HoraFin, DateFormat.ShortTime)

            l_fhaInicio = FormatDateTime(m_dtFecha, DateFormat.ShortDate)
            l_fhaTemp = FormatDateTime(m_dtFecha, DateFormat.ShortDate)
            l_fhaFinal = FormatDateTime(m_dtFecha.AddDays(7), DateFormat.ShortDate)

            l_strSQLSuspension = "SELECT U_Cod_Sucur,U_Cod_Agenda,U_Fha_Desde,U_Hora_Desde,U_Fha_Hasta,U_Hora_Hasta,U_Estado,U_Observ  " & _
                                " FROM [@SCGD_AGENDA_SUSP] " & _
                                " WHERE U_Fha_Desde BETWEEN '{0}' AND '{1}' AND U_Cod_Sucur = '{2}' AND U_Cod_Agenda = '{3}' AND U_Estado = 'Y' "

            l_strSQLSuspension = String.Format(l_strSQLSuspension, Utilitarios.RetornaFechaFormatoDB(l_fhaInicio, m_oCompany.Server), Utilitarios.RetornaFechaFormatoDB(l_fhaFinal, m_oCompany.Server), m_strCodSucursal, m_strCodAgenda)
            l_dtSuspension = Utilitarios.EjecutarConsultaDataTable(l_strSQLSuspension, m_oCompany.CompanyDB, m_oCompany.Server)
            l_numDias = 1

            Dim l_horaReservaInicio As Date
            Dim l_horaReservaFin As Date
            Dim l_FhaReservaInicio As Date
            Dim l_FhaReservaFin As Date


            While l_fhaTemp < l_fhaFinal

                While l_HoraCont <= l_HoraFin
                    l_oDataRow = l_dtSuspension.Select("U_Fha_Desde = '" & l_fhaTemp & "' AND U_Hora_Desde = '" & String.Format("{0:HHmm}", l_HoraCont) & "'")

                    If l_oDataRow.Length <> 0 Then


                        l_horaReservaInicio = FormatDateTime(Utilitarios.FormatoHora(l_oDataRow(0)("U_Hora_Desde")), DateFormat.ShortTime)
                        l_horaReservaFin = FormatDateTime(Utilitarios.FormatoHora(l_oDataRow(0)("U_Hora_Hasta")), DateFormat.ShortTime)

                        l_FhaReservaInicio = DateTime.Parse(l_oDataRow(0)("U_Fha_Desde"))
                        l_FhaReservaFin = DateTime.Parse(l_oDataRow(0)("U_Fha_Hasta"))

                        'If l_fhaTemp < l_fhaFinal Then

                        '    While l_HoraCont < l_fhaFinal
                        '        dtAgenda.Rows(l_numFila)(mc_strDia & l_numDias) = "n/a"
                        '        l_numFila += 1
                        '        l_HoraCont = l_HoraCont.AddMinutes(l_strIntervalo)
                        '    End While

                        'ElseIf l_fhaTemp = l_fhaFinal Then
                        If l_HoraFin < l_horaReservaFin Then
                            l_horaReservaFin = l_HoraFin
                        End If

                        While l_HoraCont < l_horaReservaFin
                            dtAgenda.Rows(l_numFila)(mc_strDia & l_numDias) = "n/a"
                            l_numFila += 1
                            l_HoraCont = l_HoraCont.AddMinutes(l_strIntervalo)
                        End While
                    Else
                        l_HoraCont = l_HoraCont.AddMinutes(l_strIntervalo)
                        l_numFila += 1
                    End If
                End While

                ' l_strDiaNum = l_fhaTemp.DayOfWeek
                ' VerificaCitasDisponibles(l_dtAgenda, l_numCitas, l_strDiaNum, l_HoraInicio, l_HoraFin, mc_strDia & l_numDias)

                l_numFila = 0
                l_numDias += 1
                l_numCitas = 0
                l_HoraCont = l_HoraInicio
                l_fhaTemp = l_fhaTemp.AddDays(1)
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    

    Public Sub LlenarOcupacion()
        Try
            Dim l_strSQLCitas As String
            Dim l_strSQLAgenda As String

            'Dim l_strIntervalo As String

            Dim l_fhaInicio As Date
            Dim l_fhaFinal As Date
            Dim l_fhaTemp As Date

            Dim l_dtCitas As System.Data.DataTable
            Dim l_dtAgenda As System.Data.DataTable
            Dim l_oDataRow As DataRow()
            Dim nuevacelda As ColoresAgenda
            Dim l_numDias As Integer = 1
            Dim l_numFila As Integer = 0
            Dim l_numFilaLoop As Integer = 0
            Dim l_numCitas As Integer = 0
            Dim l_numTiempoServ As Integer = 0
            Dim l_numDuraServ As Integer = 0

            Dim l_HoraCont As Date
            Dim l_HoraInicio As Date
            Dim l_HoraFin As Date
            Dim l_HoraContTemp As Date

            Dim l_strDiaNum As String
            Dim l_strSerieNumCita As String
            Dim l_strCelda As String
            Dim l_strUsaTiempoServicios As String = "N"

            Dim intDurLocal As Integer = 0
            Dim intIntevAgenda As Integer = 0
            Dim strIntervAgenda As String



            l_HoraInicio = m_fhaHoraInicio
            l_HoraFin = m_fhaHoraFin

            l_strSQLAgenda = " SELECT " & _
                            " U_Agenda, U_EstadoLogico, U_IntervaloCitas, U_Abreviatura, U_CodAsesor, U_CodTecnico, U_RazonCita, U_ArticuloCita, U_VisibleWeb, U_CantCLunes, " & _
                            " U_CantCMartes, U_CantCMiercoles, U_CantCJueves, U_CantCViernes, U_CantCSabado, U_CantCDomingo, U_Num_Art, U_Num_Razon, U_Cod_Sucursal, U_NameAsesor, U_NameTecnico, U_TmpServ " & _
                            " FROM dbo.[@SCGD_AGENDA] " & _
                            " WHERE DocEntry = '{0}' AND U_Cod_Sucursal = '{1}'"


            l_strSQLAgenda = String.Format(l_strSQLAgenda, m_strCodAgenda, m_strCodSucursal)
            l_dtAgenda = Utilitarios.EjecutarConsultaDataTable(l_strSQLAgenda, m_oCompany.CompanyDB, m_oCompany.Server)

            


            If Not String.IsNullOrEmpty(l_dtAgenda.Rows(0)("U_Agenda")) Then

                If Not IsDBNull(l_dtAgenda.Rows(0)("U_TmpServ")) Then
                    l_strUsaTiempoServicios = l_dtAgenda.Rows(0)("U_TmpServ")
                Else
                    l_strUsaTiempoServicios = "N"
                End If

                If Not IsDBNull(l_dtAgenda.Rows(0)("U_IntervaloCitas")) Then
                    strIntervAgenda = l_dtAgenda.Rows(0)("U_IntervaloCitas")
                Else
                    strIntervAgenda = "15"
                End If

            End If

            intIntevAgenda = Integer.Parse(strIntervAgenda)

            If intIntevAgenda <= 15 Then
                intIntevAgenda = 15
            End If
            'l_strIntervalo = "15"

            l_HoraInicio = FormatDateTime(l_HoraInicio, DateFormat.ShortTime)
            l_HoraCont = FormatDateTime(l_HoraInicio, DateFormat.ShortTime)
            l_HoraFin = FormatDateTime(l_HoraFin, DateFormat.ShortTime)

            l_fhaInicio = FormatDateTime(m_dtFecha, DateFormat.ShortDate)
            l_fhaTemp = FormatDateTime(m_dtFecha, DateFormat.ShortDate)
            l_fhaFinal = FormatDateTime(m_dtFecha.AddDays(7), DateFormat.ShortDate)

            l_strSQLCitas = " SELECT CI.DocEntry, CI.U_Num_Serie,CI.U_NumCita,CI.U_FechaCita, CI.U_HoraCita,CI.U_Cod_Agenda,CI.U_Cod_Sucursal, CI.U_Num_Cot,ISNULL( SUM (IT.U_SCGD_Duracion), 0) as U_SCGD_Duracion , ISNULL(CO.U_Color,'DarkSeaGreen') as Color, CASE WHEN QU.U_SCGD_Numero_OT IS NOT NULL THEN ISNULL(CE.U_ColorOT,'DarkSeaGreen') ELSE ISNULL(CE.U_Color,'DarkSeaGreen') END AS ColorEstado  " & _
                            " FROM [@SCGD_CITA] CI" & _
                            " LEFT JOIN [@SCGD_COLORESAGENDA] CO WITH (NOLOCK) ON CI.U_Cod_Razon = CO.U_RazonCita " & _
                            " LEFT OUTER JOIN  OQUT QU ON	QU.DocEntry = CI.U_Num_Cot	AND QU.U_SCGD_NoSerieCita is not null AND QU.U_SCGD_NoCita is not null" & _
                            " LEFT OUTER JOIN QUT1 Q1 ON Q1.DocEntry = QU.DocEntry	AND Q1.U_SCGD_Aprobado in (1, 4)" & _
                            " LEFT OUTER JOIN [@SCGD_CITA_ESTADOS] CE ON CI.U_Estado = CE.Code " & _
                            " INNER JOIN OITM IT ON IT.ItemCode = Q1.ItemCode" & _
                            " WHERE U_FechaCita BETWEEN '{0}' AND '{1}' AND CI.U_Cod_Sucursal	= '{2}' AND CI.U_Cod_Agenda	= '{3}' AND CI.U_Estado <> '{4}'" & _
                            " GROUP BY CI.DocEntry, CI.U_NumCita, CI.U_Num_Serie,CI.U_FechaCita, CI.U_HoraCita,CI.U_Cod_Agenda,CI.U_Cod_Sucursal, CI.U_Num_Cot, CO.U_Color, CE.U_Color, CE.U_ColorOT, QU.U_SCGD_Numero_OT  "


            l_strSQLCitas = String.Format(l_strSQLCitas, Utilitarios.RetornaFechaFormatoDB(l_fhaInicio, m_oCompany.Server), _
                                                         Utilitarios.RetornaFechaFormatoDB(l_fhaFinal, m_oCompany.Server), m_strCodSucursal, _
                                                        m_strCodAgenda, m_strCodCitaCancel)

            l_dtCitas = Utilitarios.EjecutarConsultaDataTable(l_strSQLCitas, m_oCompany.CompanyDB, m_oCompany.Server)
            l_numDias = 1

            While l_fhaTemp < l_fhaFinal
                While l_HoraCont <= l_HoraFin
                    l_oDataRow = l_dtCitas.Select("U_HoraCita = '" & String.Format("{0:HHmm}", l_HoraCont) & "' AND U_FechaCita = '" & l_fhaTemp & "'")

                    If l_oDataRow.Length <> 0 Then
                        l_strSerieNumCita = l_oDataRow(0)("U_Num_Serie") & "-" & l_oDataRow(0)("U_NumCita")

                        l_HoraContTemp = FormatDateTime(l_HoraCont, DateFormat.ShortTime)

                        If l_strUsaTiempoServicios = "Y" Then
                            intDurLocal = l_oDataRow(0)("U_SCGD_Duracion")

                            If intDurLocal > 0 Then
                                l_numDuraServ = Convert.ToInt32(intDurLocal)
                            Else
                                l_numDuraServ = Convert.ToInt32(intIntevAgenda)
                            End If
                        Else
                            l_numDuraServ = Convert.ToInt32(intIntevAgenda)
                        End If

                        l_numFilaLoop = l_numFila
                        While l_numTiempoServ < l_numDuraServ

                            If l_HoraContTemp < l_HoraFin Then
                                l_strCelda = dtAgenda.Rows(l_numFilaLoop)(mc_strDia & l_numDias)
                                If l_strCelda <> "n/a" Then
                                    dtAgenda.Rows(l_numFilaLoop)(mc_strDia & l_numDias) = l_strSerieNumCita
                                    nuevacelda.Fila = l_numFilaLoop
                                    nuevacelda.Columna = l_numDias
                                    Dim checking As Color
                                    If oGestionColor = GestionColor.EstadoCita Then
                                        checking = Color.FromName(l_oDataRow(0)("ColorEstado").ToString().ToUpper())
                                    Else
                                        checking = Color.FromName(l_oDataRow(0)("Color").ToString().ToUpper())
                                    End If
                                    nuevacelda.Color = System.Drawing.Color.FromArgb(checking.R, checking.G, checking.B)
                                    ListaColores.Add(nuevacelda)

                                    l_numTiempoServ += intIntevAgenda
                                    'l_numTiempoServ += Convert.ToInt32(l_strIntervalo)
                                    l_numFilaLoop += 1
                                    l_HoraContTemp = l_HoraContTemp.AddMinutes(strIntervAgenda)
                                    'l_HoraContTemp = l_HoraContTemp.AddMinutes(l_strIntervalo)
                                Else
                                    l_numFilaLoop += 1
                                    l_HoraContTemp = l_HoraContTemp.AddMinutes(strIntervAgenda)
                                    'l_HoraContTemp = l_HoraContTemp.AddMinutes(l_strIntervalo)
                                End If

                            Else
                                l_HoraCont = l_HoraFin
                                l_numTiempoServ = l_numDuraServ
                            End If

                        End While

                        'l_HoraCont = l_HoraCont.AddMinutes(l_strIntervalo)
                        l_HoraCont = l_HoraCont.AddMinutes(strIntervAgenda)
                        l_numTiempoServ = 0
                        l_numFila += 1
                        l_numCitas += 1
                    Else
                        l_HoraCont = l_HoraCont.AddMinutes(strIntervAgenda)
                        l_numFila += 1
                    End If


                End While

                l_strDiaNum = l_fhaTemp.DayOfWeek
                VerificaCitasDisponibles(l_dtAgenda, l_numCitas, l_strDiaNum, l_HoraInicio, l_HoraFin, mc_strDia & l_numDias)

                l_numFila = 0
                l_numDias += 1
                l_numCitas = 0
                l_HoraCont = l_HoraInicio
                l_fhaTemp = l_fhaTemp.AddDays(1)
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub LlenarOcupacionPost()
        Try
            Dim l_strSQLCitas As String
            Dim l_strSQLAgenda As String
            Dim l_strIntervalo As String

            Dim l_fhaInicio As Date
            Dim l_fhaFinal As Date
            Dim l_fhaTemp As Date

            Dim l_dtCitas As System.Data.DataTable
            Dim l_dtAgenda As System.Data.DataTable
            Dim l_oDataRow As DataRow()

            Dim l_numDias As Integer = 1
            Dim l_numFilaLoop As Integer = 0

            Dim l_strSerieNumCita As String
            Dim l_strCelda As String
            Dim strHoraCitaFin As String
            Dim intPosMin As Integer
            Dim strHoraFin As String
            Dim strMinFin As String
            Dim intDuracion As Integer

            Dim l_FhaFinalCita As Date
            Dim l_fhaInicioCita As Date

            Dim intMinInicio As Integer
            Dim intHoraInicio As Integer
            Dim l_strInicioTaller As String
            Dim l_strFinTaller As String
            Dim l_strUsaTiempoServ As String

            m_dtFecha = dtpFecha.Value


            l_strSQLAgenda = " SELECT " & _
                            " U_Agenda, U_EstadoLogico, U_IntervaloCitas, U_Abreviatura, U_CodAsesor, U_CodTecnico, U_RazonCita, U_ArticuloCita, U_VisibleWeb, U_CantCLunes, " & _
                            " U_CantCMartes, U_CantCMiercoles, U_CantCJueves, U_CantCViernes, U_CantCSabado, U_CantCDomingo, U_Num_Art, U_Num_Razon, U_Cod_Sucursal, U_NameAsesor, U_NameTecnico, ISNULL(U_TmpServ, 'N') AS U_TmpServ " & _
                            " FROM dbo.[@SCGD_AGENDA] " & _
                            " WHERE DocEntry = '{0}' AND U_Cod_Sucursal = '{1}'"

            l_strSQLAgenda = String.Format(l_strSQLAgenda, m_strCodAgenda, m_strCodSucursal)

            l_dtAgenda = Utilitarios.EjecutarConsultaDataTable(l_strSQLAgenda, m_oCompany.CompanyDB, m_oCompany.Server)

            If l_dtAgenda.Rows.Count <> 0 Then
                l_strUsaTiempoServ = l_dtAgenda.Rows(0)("U_TmpServ")
                l_strIntervalo = l_dtAgenda.Rows(0)("U_IntervaloCitas")
                If String.IsNullOrEmpty(l_strIntervalo) OrElse l_strIntervalo < 15 Then
                    l_strIntervalo = "15"

                End If
            Else
                l_strIntervalo = "15"
            End If

            If l_strUsaTiempoServ.Equals("Y") Then

                l_strInicioTaller = Utilitarios.EjecutarConsulta(String.Format("Select U_HoraInicio from  [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)
                l_strFinTaller = Utilitarios.EjecutarConsulta(String.Format("Select U_HoraFin from  [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)


                If l_strInicioTaller.Length = 3 Then
                    intHoraInicio = l_strInicioTaller.Substring(0, 1)
                    intMinInicio = l_strInicioTaller.Substring(1, 2)
                ElseIf l_strInicioTaller.Length = 4 Then
                    intHoraInicio = l_strInicioTaller.Substring(0, 1)
                    intMinInicio = l_strInicioTaller.Substring(1)
                End If


                '  l_HoraInicio = FormatDateTime(l_HoraInicio, DateFormat.ShortTime)
                'l_HoraFin = FormatDateTime(l_HoraFin, DateFormat.ShortTime)

                l_fhaInicio = FormatDateTime(m_dtFecha, DateFormat.ShortDate)
                l_fhaTemp = FormatDateTime(m_dtFecha, DateFormat.ShortDate)
                l_fhaFinal = FormatDateTime(m_dtFecha.AddDays(7), DateFormat.ShortDate)

                l_strSQLCitas = " SELECT CI.DocEntry, CI.U_Num_Serie,CI.U_NumCita,CI.U_FechaCita,CI.U_HoraCita,U_FhaCita_Fin,U_HoraCita_Fin,CI.U_Cod_Agenda,CI.U_Cod_Sucursal, CI.U_Num_Cot" +
                    " FROM [@SCGD_CITA] CI " +
                    " WHERE(U_FechaCita <> U_FhaCita_Fin)" +
                    " AND U_FhaCita_Fin BETWEEN '{0}' AND '{1}' " +
                    " AND CI.U_Cod_Sucursal	= '{2}' " +
                    " AND CI.U_Cod_Agenda	= '{3}' " +
                    " AND CI.U_Estado <> '{4}' " +
                    " AND CI.U_NumCita is not null "

                l_strSQLCitas = String.Format(l_strSQLCitas, Utilitarios.RetornaFechaFormatoDB(l_fhaInicio, m_oCompany.Server), _
                                                             Utilitarios.RetornaFechaFormatoDB(l_fhaFinal, m_oCompany.Server), m_strCodSucursal, _
                                                            m_strCodAgenda, m_strCodCitaCancel)

                l_dtCitas = Utilitarios.EjecutarConsultaDataTable(l_strSQLCitas, m_oCompany.CompanyDB, m_oCompany.Server)
                l_numDias = 1

                While l_fhaTemp < l_fhaFinal
                    l_oDataRow = l_dtCitas.Select(" U_FhaCita_Fin = '" & l_fhaTemp & "'")
                    If l_oDataRow.Length <> 0 Then

                        If Not String.IsNullOrEmpty(l_oDataRow(0)("U_Num_Serie")) AndAlso
                            Not String.IsNullOrEmpty(l_oDataRow(0)("U_NumCita")) Then

                            l_strSerieNumCita = l_oDataRow(0)("U_Num_Serie") & "-" & l_oDataRow(0)("U_NumCita")

                            strHoraCitaFin = l_oDataRow(0)("U_HoraCita_Fin")

                            intPosMin = (strHoraCitaFin.Length - 2)
                            strHoraFin = strHoraCitaFin.Substring(0, intPosMin)
                            strMinFin = strHoraCitaFin.Substring(intPosMin, 2)

                            l_fhaInicioCita = l_fhaTemp
                            l_fhaInicioCita = l_fhaInicioCita.AddHours(intHoraInicio)
                            l_fhaInicioCita = l_fhaInicioCita.AddMinutes(intMinInicio)

                            l_FhaFinalCita = l_fhaTemp
                            l_FhaFinalCita = l_FhaFinalCita.AddHours(strHoraFin)
                            l_FhaFinalCita = l_FhaFinalCita.AddMinutes(strMinFin)


                            intDuracion = DateDiff(DateInterval.Minute, l_fhaInicioCita, l_FhaFinalCita)

                            If intDuracion <> 0 Then
                                l_numFilaLoop = 0

                                While l_fhaInicioCita < l_FhaFinalCita
                                    l_strCelda = dtAgenda.Rows(l_numFilaLoop)(mc_strDia & l_numDias)

                                    If l_strCelda <> "n/a" Then
                                        dtAgenda.Rows(l_numFilaLoop)(mc_strDia & l_numDias) = l_strSerieNumCita
                                        l_numFilaLoop += 1
                                        l_fhaInicioCita = l_fhaInicioCita.AddMinutes(15)
                                    Else
                                        l_numFilaLoop += 1
                                    End If

                                End While

                                l_numFilaLoop = 0

                            Else
                                dtAgenda.Rows(l_numFilaLoop)(mc_strDia & l_numDias) = l_strSerieNumCita
                                l_fhaTemp.AddDays(1)
                                l_numFilaLoop = 0
                            End If

                        End If
                    End If

                    l_numDias += 1
                    l_fhaTemp = l_fhaTemp.AddDays(1)
                End While
            End If




        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ManejorDataGridView()
        Dim pintada As ColoresAgenda
        Try
            If dtgOcupacion.Rows.Count > 0 Then

                'dtgOcupacion.Columns(0).Visible = False

                dtgOcupacion.Columns(0).Frozen = True
                dtgOcupacion.RowHeadersVisible = False


                For Each row As DataGridViewRow In dtgOcupacion.Rows
                    row.Cells(0).Style.BackColor = Color.White
                Next

                Dim Column As DataGridViewColumn = dtgOcupacion.Columns(0)
                Column.Width = 47

                For count As Integer = 0 To dtgOcupacion.Rows.Count - 1
                    Dim row As DataGridViewRow = dtgOcupacion.Rows(count)
                    row.Height = 24
                    dtgOcupacion.Rows(count).Cells(0).Style.BackColor = System.Drawing.Color.FromArgb(222, 223, 206)
                    dtgOcupacion.Rows(count).Cells(0).ReadOnly = True
                Next

                dtgOcupacion.AllowUserToResizeRows = False
                dtgOcupacion.AllowUserToResizeColumns = False

                For Count As Integer = 1 To dtgOcupacion.Columns.Count - 1
                    Dim Columns As DataGridViewColumn = dtgOcupacion.Columns(Count)
                    Columns.HeaderText = GetTitleCol(Count - 1)
                Next

                For Each pintada In ListaColores
                    dtgOcupacion.Rows(pintada.Fila).Cells(pintada.Columna).Style.BackColor = pintada.Color
                Next

            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub frmCalendarioColor_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim l_intWidth As Integer
        Dim l_intHeigth As Integer

        Try

            If IsNothing(m_dtFecha) Then
                dtpFecha.Value = Utilitarios.EjecutarConsulta("SELECT GETDATE()", m_oCompany.CompanyDB, m_oCompany.Server)
            Else
                dtpFecha.Value = m_dtFecha
            End If

            lblNombreAgenda.Text = m_strNombreAgenda
            ListaColores.Clear()
            LoadEstiloGrid()
            CrearTablaAgenda()
            CargarAgenda()
            LlenarReservasion()
            LlenarOcupacion()
            LlenarOcupacionPost()

            '***************************************
            If m_blnVersion9 = False Then
                Timer1.Interval = 60000
                AddHandler Timer1.Tick, AddressOf HandleTimerTick
                Timer1.Start()

            End If

            '***************************************

            '****************

            dtgOcupacion.DataSource = dtAgenda
            ManejorDataGridView()

            If m_blnInterno Then
                l_intWidth = 800
                l_intHeigth = 600

                Me.CenterToScreen()

                Me.Size = New Size(l_intWidth, l_intHeigth)
                Me.MaximizeBox = False
                cargarSkin()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub cargarSkin()

        Dim ruta As String = System.Windows.Forms.Application.StartupPath
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

    Private Sub LoadConsultaOcupacion()

        ListaColores.Clear()
        dtAgenda.Clear()
        LoadEstiloGrid()
        CargarAgenda()
        LlenarReservasion()
        LlenarOcupacion()
        LlenarOcupacionPost()

        dtgOcupacion.DataSource = dtAgenda
        ManejorDataGridView()
    End Sub

    Private Sub btnAnteriorWeek_Click(sender As System.Object, e As System.EventArgs) Handles btnAnteriorWeek.Click
        m_dtFecha = m_dtFecha.AddDays(-1)
        dtpFecha.Value = m_dtFecha

        LoadConsultaOcupacion()
    End Sub

    Private Sub btnAnteriorDay_Click(sender As System.Object, e As System.EventArgs) Handles btnAnteriorDay.Click
        m_dtFecha = m_dtFecha.AddDays(-1)
        dtpFecha.Value = m_dtFecha

        LoadConsultaOcupacion()
    End Sub

    Private Sub btnSiguienteDay_Click(sender As System.Object, e As System.EventArgs) Handles btnSiguienteDay.Click
        m_dtFecha = m_dtFecha.AddDays(1)
        dtpFecha.Value = m_dtFecha

        LoadConsultaOcupacion()
    End Sub

    Private Sub btnSiguienteWeek_Click(sender As System.Object, e As System.EventArgs) Handles btnSiguienteWeek.Click
        m_dtFecha = m_dtFecha.AddDays(7)
        dtpFecha.Value = m_dtFecha
        LoadConsultaOcupacion()
    End Sub

    Private Sub dtgOcupacion_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgOcupacion.CellContentClick
        Me.dtgOcupacion.SelectionMode = DataGridViewSelectionMode.CellSelect
        Me.dtgOcupacion.DefaultCellStyle.SelectionBackColor = Color.DarkSeaGreen
        Me.dtgOcupacion.ReadOnly = True
    End Sub

    Private Sub LlenaListaReserva()
        Dim l_fhaDesde As Date
        Dim l_fhaHasta As Date

        Dim datFecha As Date
        Dim datHora As Date
        Dim intDias As Integer

        Dim newReserva As New Reservacion

        Try
            If dtgOcupacion.CurrentCell.RowIndex <> -1 Then

                intDias = dtgOcupacion.CurrentCell.ColumnIndex
                datFecha = dtpFecha.Value.AddDays((intDias - 2))
                datHora = CDate("01-01-1900" & " " & dtgOcupacion.Item(dtgOcupacion.CurrentRow.Index, 1).ToString())

                l_fhaDesde = New Date(datFecha.Year, datFecha.Month, datFecha.Day, datHora.Hour, datHora.Minute, 0)
                l_fhaHasta = l_fhaDesde.AddMinutes(m_numIntervalos)

                newReserva.fhaDesde = l_fhaDesde
                newReserva.fhaHasta = l_fhaHasta

                If Not ListReservas.Exists(Function(x) x.fhaDesde = newReserva.fhaDesde And _
                                            x.fhaHasta = newReserva.fhaHasta) Then
                    ListReservas.Add(newReserva)
                Else
                    ListReservas.Remove(newReserva)
                End If

            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub dtgOcupacion_CellClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgOcupacion.CellClick
        Try
            Dim strCurrenCell As String
            If (Not String.IsNullOrEmpty(m_strFormUID)) Then

                If m_strFormUID = mc_strUI_SuspenderAgenda Then
                    If dtgOcupacion.CurrentCell.RowIndex <> -1 Then

                        strCurrenCell = dtgOcupacion.Item(dtgOcupacion.CurrentCell.ColumnIndex, dtgOcupacion.CurrentCell.RowIndex).ToString()
                        If String.IsNullOrEmpty(strCurrenCell) Then
                            dtgOcupacion.Item(dtgOcupacion.CurrentCell.ColumnIndex, dtgOcupacion.CurrentCell.RowIndex).Value = "***"
                        ElseIf strCurrenCell = "***" Then
                            dtgOcupacion.Item(dtgOcupacion.CurrentCell.ColumnIndex, dtgOcupacion.CurrentCell.RowIndex).Value = String.Empty
                        End If
                        LlenaListaReserva()
                    End If
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAceptar_Click(sender As System.Object, e As System.EventArgs) Handles btnAceptar.Click

        Dim intCodigoAgenda As Integer
        Dim strNombreAgenda As String

        RaiseEvent eListaSuspecionesAgendaColor(ListReservas, strNombreAgenda, intCodigoAgenda)
    End Sub

    Private Sub btnAceptar_DoubleClick(sender As Object, e As System.EventArgs) Handles btnAceptar.DoubleClick

    End Sub

    Private Sub ObtenerFechaHoraCita(ByRef p_datFechaYHora As Date)

        Dim datFecha As Date
        Dim datHora As Date
        Dim intDias As Integer


        Try
            If dtgOcupacion.CurrentCell.RowIndex <> -1 Then

                intDias = dtgOcupacion.CurrentCell.ColumnIndex
                datFecha = dtpFecha.Value.AddDays((intDias - 1))
                datHora = CDate("01-01-1900" & " " & dtgOcupacion.Item(0, dtgOcupacion.CurrentCell.RowIndex).Value)

                datFecha = New Date(datFecha.Year, datFecha.Month, datFecha.Day, datHora.Hour, datHora.Minute, 0)

                p_datFechaYHora = datFecha

            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub


    Private Sub dtgOcupacion_DoubleClick(sender As System.Object, e As System.EventArgs) Handles dtgOcupacion.DoubleClick

        Dim strCitas As String
        Dim strCantCitas As String()
        Dim intCantCitas As Integer
        Dim datFechaYHora As Date
        Dim intCodigoAgenda As Integer
        Dim strNombreAgenda As String


        Try

            If dtgOcupacion.CurrentCell.RowIndex <> -1 Then

                strCitas = dtgOcupacion.Item(dtgOcupacion.CurrentCell.ColumnIndex, dtgOcupacion.CurrentCell.RowIndex).Value
                strCantCitas = strCitas.Split(",")
                intCantCitas = strCantCitas.Length

                If strCitas <> "n/a" Then  'Cuando no hay cita para esa hora

                    ObtenerFechaHoraCita(datFechaYHora)
                    strNombreAgenda = m_strNombreAgenda

                    RaiseEvent eFechaYHoraSeleccionadaColor(datFechaYHora, strNombreAgenda, intCodigoAgenda)


                ElseIf intCantCitas = 1 Then 'Cuando solo hay una cita para esa hora
                    If Not m_blnInterno Then

                        ' CargarCitasModoModificar(strCitas)

                    End If

                End If '1 Cita
            End If 'Validacion grid

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub frmCalendarioColor_DoubleClick(sender As System.Object, e As System.EventArgs) Handles MyBase.DoubleClick

    End Sub

    Private Sub dtgOcupacion_SelectionChanged(sender As System.Object, e As System.EventArgs) Handles dtgOcupacion.SelectionChanged
        For Each cell As DataGridViewCell In DirectCast(sender, DataGridView).SelectedCells
            cell.Style = New DataGridViewCellStyle() With { _
             .BackColor = Color.White, _
             .Font = New Font("Tahoma", 8.0F), _
             .ForeColor = SystemColors.WindowText, _
             .SelectionBackColor = Color.DarkSeaGreen, _
             .SelectionForeColor = SystemColors.HighlightText _
            }
        Next
    End Sub

    Private Sub btnActualizar_Click(sender As System.Object, e As System.EventArgs) Handles btnActualizar.Click
        Try

            m_dtFecha = dtpFecha.Value
            m_strCodAgenda = m_strCodAgenda

            LoadConsultaOcupacion()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As System.Object, e As System.EventArgs) Handles btnCerrar.Click

    End Sub

    Private Sub dtgOcupacion_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles dtgOcupacion.MouseMove
        Try
            ControlarEventoToolTip(sender, e)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado del manejo del ToolTip del DataGrid
    ''' </summary>
    ''' <param name="sender">Objeto que contiene la información del evento</param>
    ''' <param name="e">Objeto que contiene la información de los parámetros del evento</param>
    ''' <remarks></remarks>
    Private Sub ControlarEventoToolTip(ByRef sender As System.Object, ByRef e As System.Windows.Forms.MouseEventArgs)
        Dim X As Integer = 0
        Dim Y As Integer = 0
        Dim oHitTestInfo As DataGridView.HitTestInfo
        Dim strTextoToolTip As String = String.Empty
        Dim strNumeroCita As String = String.Empty

        Try
            X = e.X
            Y = e.Y
            oHitTestInfo = dtgOcupacion.HitTest(X, Y)
            oCeldaActual.ColumnNumber = oHitTestInfo.ColumnIndex
            oCeldaActual.RowNumber = oHitTestInfo.RowIndex

            'Valida que se haya posicionado el mouse sobre una celda válida
            If EsCeldaValida(oHitTestInfo, dtgOcupacion) AndAlso EsCeldaDistinta() Then
                strNumeroCita = dtgOcupacion.Item(oHitTestInfo.ColumnIndex, oHitTestInfo.RowIndex).Value
                If EsCita(strNumeroCita) Then
                    strTextoToolTip = ObtenerDatosToolTip(strNumeroCita)
                    MostrarToolTip(X, Y, strTextoToolTip)
                Else
                    'Oculta el ToolTip al cambiar de celda o cuando los datos no son válidos
                    TTCita.Hide(Me)
                End If
            End If

            'Guarda la posición de la celda para en el próximo evento comparar si se cambió de celda o no
            oCeldaPrevia.ColumnNumber = oHitTestInfo.ColumnIndex
            oCeldaPrevia.RowNumber = oHitTestInfo.RowIndex
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Muestra un ToolTip en la posición y con el texto indicado
    ''' </summary>
    ''' <param name="X">Coordenada X en formato entero</param>
    ''' <param name="Y">Coordenada Y en formato entero</param>
    ''' <param name="strTextoToolTip">Texto del ToolTipo</param>
    ''' <remarks></remarks>
    Private Sub MostrarToolTip(ByVal X As Integer, ByVal Y As Integer, ByVal strTextoToolTip As String)
        Dim intDuracionMilisegundos As Integer = 120000
        Try
            X += AjustePosicionToolTip(X, Y, Me.Size.Width, strTextoToolTip)
            TTCita.Show(strTextoToolTip, Me, New Point(X, Y), intDuracionMilisegundos)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ajusta la posición del ToolTip para evitar que quede fuera de la pantalla
    ''' </summary>
    ''' <param name="X">Posición actual X</param>
    ''' <param name="Y">Posición Actual Y</param>
    ''' <param name="WindowWidth">Tamaño de la ventana en formato entero</param>
    ''' <param name="strTextoToolTip">Texto del ToolTip que se va a mostrar</param>
    ''' <returns>Nueva posición en formato entero</returns>
    ''' <remarks></remarks>
    Private Function AjustePosicionToolTip(ByVal X As Integer, ByVal Y As Integer, ByVal WindowWidth As Integer, ByVal strTextoToolTip As String)
        Dim intResultado As Integer = 0
        Dim intLargo As Integer = 0
        Dim intEspacioDisponible As Integer = 0

        Try
            Dim strLineas() As String = Split(strTextoToolTip, vbCrLf)

            If Not String.IsNullOrEmpty(strTextoToolTip) Then
                For Each strLinea As String In strLineas
                    If strLinea.Length > intLargo Then
                        intLargo = strLinea.Length
                    End If
                Next

                intEspacioDisponible = WindowWidth - X
                intResultado = intLargo * 5.85

                If intEspacioDisponible > intResultado Then
                    intResultado = 0
                Else
                    intResultado = intResultado * -1
                End If
            End If

            Return intResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return intResultado
        End Try
    End Function

    ''' <summary>
    ''' Consulta la información de la cita y la devuelve en formato texto
    ''' </summary>
    ''' <param name="strNumeroCita">Número de cita incluido la serie y el código es un solo texto</param>
    ''' <returns>Texto con la información de la cita</returns>
    ''' <remarks></remarks>
    Private Function ObtenerDatosToolTip(ByVal strNumeroCita As String) As String
        Dim strQuery As String = "SELECT T0.U_CardCode, T0.U_CardName, T0.U_Cod_Unid, T1.U_Num_Plac, T0.U_Observ, T0.U_CRetiro, T0.U_CEntrega, T0.U_Movilidad, T3.Name AS 'DscMovilidad', T0.U_CMovilidad, T0.U_Contacto, T4.Name AS 'DscContacto', T0.U_CContacto FROM ""@SCGD_CITA"" AS T0 INNER JOIN ""@SCGD_VEHICULO"" AS T1 ON T0.U_Cod_Unid = T1.U_Cod_Unid LEFT JOIN ""@SCGD_MOVILIDAD"" T3 ON T0.U_Movilidad = T3.Code LEFT JOIN ""@SCGD_FCONTACTO"" T4 ON T0.U_Contacto = T4.Code WHERE U_Num_Serie = '{0}' and U_NumCita = '{1}'"
        Dim strTextoToolTip As String = String.Empty
        Dim strCardCode As String = String.Empty
        Dim strCardName As String = String.Empty
        Dim strCodigoUnidad As String = String.Empty
        Dim strPlaca As String = String.Empty
        Dim strObservaciones As String = String.Empty
        Dim strComentariosRetiro As String = String.Empty
        Dim strComentariosEntrega As String = String.Empty
        Dim strCodigoMovilidad As String = String.Empty
        Dim strDescripcionMovilidad As String = String.Empty
        Dim strComentariosMovilidad As String = String.Empty
        Dim strCodigoContacto As String = String.Empty
        Dim strDescripcionContacto As String = String.Empty
        Dim strComentariosContacto As String = String.Empty
        Dim oRecordSet As SAPbobsCOM.Recordset

        Try
            Dim strSerieNumeroCita() As String = strNumeroCita.Split(New Char() {"-"c})
            strQuery = String.Format(strQuery, strSerieNumeroCita(0).Trim(), strSerieNumeroCita(1).Trim())

            oRecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery(strQuery)

            If oRecordSet.RecordCount > 0 Then
                'Cita
                If Not String.IsNullOrEmpty(strNumeroCita) Then
                    strTextoToolTip += String.Format("{0}{1}", My.Resources.Resource.NumCita, strNumeroCita)
                    strTextoToolTip += vbCrLf
                End If

                'Cliente
                strCardCode = oRecordSet.Fields().Item(0).Value.ToString()
                strCardName = oRecordSet.Fields().Item(1).Value.ToString()

                If Not String.IsNullOrEmpty(strCardCode) AndAlso Not String.IsNullOrEmpty(strCardName) Then
                    strTextoToolTip += String.Format("{0}{1}{2}{3}{4}", My.Resources.Resource.cliente, strCardName, " ", My.Resources.Resource.codigo, strCardCode)
                    strTextoToolTip += vbCrLf
                End If

                'Vehículo
                strCodigoUnidad = oRecordSet.Fields().Item(2).Value.ToString()
                strPlaca = oRecordSet.Fields().Item(3).Value.ToString()

                If Not String.IsNullOrEmpty(strCodigoUnidad) Or Not String.IsNullOrEmpty(strPlaca) Then
                    strTextoToolTip += String.Format("{0}{1}{2}{3}{4}", My.Resources.Resource.vehiculo1, strPlaca, " ", My.Resources.Resource.codigo, strCodigoUnidad)
                    strTextoToolTip += vbCrLf
                End If

                'Observaciones
                strObservaciones = oRecordSet.Fields().Item(4).Value.ToString()
                If Not String.IsNullOrEmpty(strObservaciones) Then
                    strTextoToolTip += String.Format("{0}{1}", My.Resources.Resource.TXTObservaciones, strObservaciones)
                    strTextoToolTip += vbCrLf
                End If

                'Retiro
                strComentariosRetiro = oRecordSet.Fields().Item(5).Value.ToString()
                If Not String.IsNullOrEmpty(strComentariosRetiro) Then
                    strTextoToolTip += String.Format("{0}{1}", My.Resources.Resource.RetiroVehiculo, strComentariosRetiro)
                    strTextoToolTip += vbCrLf
                End If

                'Entrega
                strComentariosEntrega = oRecordSet.Fields().Item(6).Value.ToString()
                If Not String.IsNullOrEmpty(strComentariosEntrega) Then
                    strTextoToolTip += String.Format("{0}{1}", My.Resources.Resource.EntregaVehiculo, strComentariosEntrega)
                    strTextoToolTip += vbCrLf
                End If

                'Movilidad
                strCodigoMovilidad = oRecordSet.Fields().Item(7).Value.ToString()
                strDescripcionMovilidad = oRecordSet.Fields().Item(8).Value.ToString()
                strComentariosMovilidad = oRecordSet.Fields().Item(9).Value.ToString()
                If Not String.IsNullOrEmpty(strDescripcionMovilidad) Or Not String.IsNullOrEmpty(strComentariosMovilidad) Then
                    strTextoToolTip += String.Format("{0}{1}{2}{3}", My.Resources.Resource.Movilidad, strDescripcionMovilidad, ". ", strComentariosMovilidad)
                    strTextoToolTip += vbCrLf
                End If

                'Contacto
                strCodigoContacto = oRecordSet.Fields().Item(10).Value.ToString()
                strDescripcionContacto = oRecordSet.Fields().Item(11).Value.ToString()
                strComentariosContacto = oRecordSet.Fields().Item(12).Value.ToString()
                If Not String.IsNullOrEmpty(strDescripcionContacto) Or Not String.IsNullOrEmpty(strComentariosContacto) Then
                    strTextoToolTip += String.Format("{0}{1}{2}{3}", My.Resources.Resource.FormaContacto, strDescripcionContacto, ". ", strComentariosContacto)
                    strTextoToolTip += vbCrLf
                End If

                'Agregar nuevos datos al texto aquí

                Return strTextoToolTip
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try
    End Function

    ''' <summary>
    ''' Verifica si la celda corresponde a una cita
    ''' </summary>
    ''' <param name="strNumeroCita">Número de cita en formato texto</param>
    ''' <returns>True = Es cita. False = No es cita</returns>
    ''' <remarks>Verifica si es una cita de acuerdo al texto de la celda</remarks>
    Private Function EsCita(ByVal strNumeroCita As String) As Boolean
        Dim strSerieCita As String = String.Empty
        Try
            If String.IsNullOrEmpty(strNumeroCita) Then
                Return False
            End If

            If strNumeroCita = "n/a" Or strNumeroCita = "." Or strNumeroCita = "***" Then
                Return False
            End If

            strSerieCita = strNumeroCita.Substring(0, 1)

            If IsNumeric(strSerieCita) Then
                Return False
            End If

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Verifica si la posición del mouse es una celda distinta a la seleccionada
    ''' </summary>
    ''' <returns>True = Es una celda distinta. False = Es la misma celda.</returns>
    ''' <remarks>Verifica si es una celda distinta para mejorar el rendimiento y evitar que se cargue el ToolTip cada vez que se mueve el mouse en la misma celda</remarks>
    Private Function EsCeldaDistinta() As Boolean
        Try
            If oCeldaActual.ColumnNumber <> oCeldaPrevia.ColumnNumber Or oCeldaActual.RowNumber <> oCeldaPrevia.RowNumber Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Verifica que la celda tenga datos válidos, no este en blanco y no sea una celda bloqueada
    ''' </summary>
    ''' <param name="oHitTestInfo">Objeto HitTestInfo con la información de la posición donde está el mouse</param>
    ''' <param name="oDataGrid">Objeto DataGridView con los datos</param>
    ''' <returns>True = Es una celda válida. False = No es una celda válida.</returns>
    ''' <remarks></remarks>
    Private Function EsCeldaValida(ByRef oHitTestInfo As DataGridView.HitTestInfo, ByRef oDataGrid As DataGridView) As Boolean
        Dim blnResultado As Boolean = False
        Try
            If oHitTestInfo.Type = DataGrid.HitTestType.Cell Then
                If oHitTestInfo.ColumnIndex > 0 AndAlso oHitTestInfo.RowIndex > -1 AndAlso oHitTestInfo.RowIndex < oDataGrid.Rows.Count Then
                    If Not IsDBNull(oDataGrid.Item(oHitTestInfo.ColumnIndex, oHitTestInfo.RowIndex).Value) Then
                        blnResultado = True
                    End If
                End If
            End If
            Return blnResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return blnResultado
        End Try
    End Function

End Class