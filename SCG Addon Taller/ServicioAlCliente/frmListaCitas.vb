Imports Deklarit
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Text
Imports System.Threading
Imports SAPbouiCOM
Imports Sunisoft.IrisSkin
Imports SCG_User_Interface
Imports System.Timers
Imports SAPbobsCOM
Imports SCG.SBOFramework
Imports System.Linq


Public Class frmListaCitas

#Region "Declaraciones"

    Private m_dtFecha As Date = Nothing
    Private m_blnEjecutarEvento As Boolean = False
    Public m_oCompany As SAPbobsCOM.Company
    Public m_oApplication As SAPbouiCOM.Application
    Public m_blnInterno As Boolean
    Public m_strFormUID As String
    ' Public m_blnVersio9 As Boolean
    Public m_blnVersion9 As Boolean = False
    Public m_intEspacioAse As Integer
    Public m_intEspacioTec As Integer
    Public m_intServRapido As Integer = 0

    Private m_strCodSucursal As String
    Private m_strCodAgenda As String = String.Empty
    Private m_strNumGrupo As String
    Private m_blnEstadoForma As Boolean
    Private m_tipoAgendaCargar As Integer

    Private m_strNombreAgenda As String = ""

    Private Const mc_strIDAgenda As String = "IdAgenda"
    Private Const mc_strID As String = "ID"
    Private Const mc_strName As String = "Name"
    Private Const mc_strCita As String = "Citas"
    Private Const mc_strAgenda As String = "Agenda"
    Private Const mc_strEquipo As String = "Grupo"
    Private Const mc_strRol As String = "Rol"
    Private Const mc_strInterv As String = "Intervalo"
    Private Const mc_strServRap As String = "ServRap"

    Private m_fhaHoraInicio As Date
    Private m_fhaHoraFin As Date
    Private m_numIntervalos As Integer
    Private m_blnUsaIntevEstandar As Boolean

    Dim dtAgenda As New System.Data.DataTable
    Dim dtNombres As New System.Data.DataTable
    Private dtCitasProbl As New System.Data.DataTable

    Private _skinEngine As SkinEngine

    Private listColumsGrid As New List(Of String)

    Private m_OldCell As DataGridCell

    Private m_strTextoInfo As String = ""
    Private m_strTipoAgendaColor As String = ""

    Private tableCitasProbl As New Hashtable

    Public pivot As String
    Public actual As String
    Public blnCambio As Boolean = False

    Private oGestionColor As GestionColor
    Private oModoCeldasAgenda As ModoCeldasAgenda
    Private oVersionModuloCita As VersionModuloCita
    Private strUsaColorAgenda As String = String.Empty
    Private CitaAbierta As Boolean = False

    Public Enum GestionColor
        RazonCita = 1
        EstadoCita = 2
    End Enum

    Private Enum ModoCeldasAgenda
        Estandar = 1
        TamanoEncabezados = 2
        Ajustar = 3
    End Enum

    Public Enum VersionModuloCita
        Estandar = 1
        V2 = 2
    End Enum

    Private Enum EstadoServicio
        NoIniciado = 1
        Iniciado = 2
        Suspendido = 3
        Finalizado = 4
    End Enum

    Private Enum EstadoCotizacion
        NoIniciada = 1
        EnProceso = 2
        Suspendida = 3
    End Enum


    Structure infoColumn
        Public _position As Integer
        Public _Hora As String
        Public _Minutos As String
        Public _horaFull As String
    End Structure

    Structure infoRows
        Public _position As Integer
        Public _Code As String
        Public _Name As String
        Public _DocEntryAgenda As String
        Public _Type As String
        Public _Equipo As String
        Public _Rol As String
        Public _Intervalo As String
        Public _ServRap As String
    End Structure

    Structure infoCelda
        Public _ColNum As Integer
        Public _FilNum As Integer
        Public _Texto As String
        Public _Rol As String
        Public _Equipo As String
    End Structure

    Dim m_InfoColum As infoColumn
    Dim m_InfoRows As infoRows

    Dim m_oCeldaAsesor As infoCelda
    Dim m_oCeldaTecnico As infoCelda

    Dim m_oCeldaAsesorAnt As infoCelda
    Dim m_oCeldaTecnicoAnt As infoCelda

    Public m_listColums As List(Of infoColumn)
    Public m_listRows As List(Of infoRows)

    Public m_strNombreBDTaller As String
    Public m_strUsarTallerSAP As String

    Enum TipoDeAgenda
        Agenda = 1
        Equipos = 2
    End Enum

    Enum TipoDocumento
        Cita = 1
        OrdenTrabajo = 2
        Bloqueo = 3
        Suspension = 4
        Reprogramada = 5
        ServicioNoIniciado = 6
        CitaAsesor = 7
    End Enum

    Public Event eCargaCitaExiste(ByVal p_strSerie As String, ByVal p_strNumCita As String, ByVal p_strCodAgenda As String)

    'Public Event eFechaYHoraSeleccionadaSinCita(ByVal p_strAno As String, ByVal p_strMes As String, ByVal p_strDia As String, ByVal p_strHoraFull As String, ByVal p_strHora As String,
    '                                            ByVal p_strMinutos As String, ByVal p_CodAsesor As String, ByVal p_strTecnico As String, ByVal p_StrSucursal As String, ByVal p_strAgenda As String)

    Public Event eCargaCitaNueva_PorAgenda(ByVal p_dtFechaYHora As Date, ByVal p_CodAsesor As String, ByVal p_strTecnico As String, ByVal p_StrSucursal As String, ByVal p_strAgenda As String)

    Public Event eCargaCitaNueva_PorEquipos(ByVal p_fhaAsesor As Date, ByVal p_fhaTecnico As Date, ByVal p_strCodAseror As String, ByVal p_strCodTecnico As String, ByVal p_strCodSucursal As String, ByVal p_strCodAgenda As String)


    '  Public Event eCitaSeleccionadaSelect(ByVal p_strSerie As String, ByVal p_strNumCita As String, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer)
    'Public Event eCitaNuevaSelect(ByVal p_dtFechaYHora As Date, ByVal p_strAno As String, ByVal p_strMes As String, ByVal p_strDia As String, ByVal p_strHoraFull As String, ByVal p_strHora As String,
    '                                            ByVal p_strMinutos As String, ByVal p_CodAsesor As String, ByVal p_strTecnico As String, ByVal p_StrSucursal As String, ByVal p_strAgenda As String)

    ' Public Event eCargaCitaNueva_PorEquipos(ByVal p_dtFechaYHora As Date, ByVal p_CodAsesor As String, ByVal p_strTecnico As String, ByVal p_StrSucursal As String, ByVal p_strAgenda As String)


#End Region

#Region "Constructor"

    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal p_datFecha As Date, ByVal p_codSucursal As String, ByVal p_codAgenda As String, ByVal p_numGrupo As String, ByVal p_blnInterno As Boolean, ByVal p_tipoAgendaCargar As Integer, ByVal p_blnVersion9 As Boolean, ByVal p_intEspaciosAsesor As Integer, ByVal p_intEspaciosTecnico As Integer, ByVal p_strTipoAgendaColor As String, ByVal p_oCompany As SAPbobsCOM.Company, ByVal p_oApplication As SAPbouiCOM.Application, ByVal p_strformUID As String, ByVal p_CitaAbierta As Boolean)
        MyBase.New()

        Try
            m_dtFecha = p_datFecha
            m_strCodSucursal = p_codSucursal
            m_strCodAgenda = p_codAgenda
            m_strNumGrupo = p_numGrupo
            m_oCompany = p_oCompany
            m_blnInterno = p_blnInterno
            m_tipoAgendaCargar = p_tipoAgendaCargar
            m_oApplication = p_oApplication
            m_strFormUID = p_strformUID
            m_blnVersion9 = p_blnVersion9
            m_intEspacioAse = p_intEspaciosAsesor
            m_intEspacioTec = p_intEspaciosTecnico
            m_strTipoAgendaColor = p_strTipoAgendaColor
            CitaAbierta = p_CitaAbierta
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
            InitializeComponent()

            'Obtiene la configuración de color de la sucursal
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_ManageColorBy) Then
                oGestionColor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_ManageColorBy
            Else
                oGestionColor = GestionColor.RazonCita
            End If

            'Obtiene la configuración del tamaño de las celdas de la agenda
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_SchSizeMode) Then
                oModoCeldasAgenda = DMS_Connector.Configuracion.ParamGenAddon.U_SchSizeMode
            Else
                oModoCeldasAgenda = ModoCeldasAgenda.Estandar
            End If

            'Obtiene la configuración del tamaño de las celdas de la agenda
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Then
                oVersionModuloCita = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
            Else
                oVersionModuloCita = VersionModuloCita.Estandar
            End If

            strUsaColorAgenda = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_AgendaColor

            If String.IsNullOrEmpty(strUsaColorAgenda) Then
                strUsaColorAgenda = "N"
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub New(ByVal p_datFecha As Date, ByVal p_codSucursal As String, ByVal p_codAgenda As String, ByVal p_numGrupo As String, ByVal p_blnInterno As Boolean, ByVal p_tipoAgendaCargar As Integer, ByVal p_blnVersion9 As Boolean, ByVal p_intEspaciosAsesor As Integer, ByVal p_intEspaciosTecnico As Integer, ByVal p_strTipoAgendaColor As String, ByVal p_oCompany As SAPbobsCOM.Company, ByVal p_oApplication As SAPbouiCOM.Application, Optional ByVal p_strformUID As String = "")
        MyBase.New()

        Try
            m_dtFecha = p_datFecha
            m_strCodSucursal = p_codSucursal
            m_strCodAgenda = p_codAgenda
            m_strNumGrupo = p_numGrupo
            m_oCompany = p_oCompany
            m_blnInterno = p_blnInterno
            m_tipoAgendaCargar = p_tipoAgendaCargar
            m_oApplication = p_oApplication
            m_strFormUID = p_strformUID
            m_blnVersion9 = p_blnVersion9
            m_intEspacioAse = p_intEspaciosAsesor
            m_intEspacioTec = p_intEspaciosTecnico
            m_strTipoAgendaColor = p_strTipoAgendaColor
            DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)
            InitializeComponent()

            'Obtiene la configuración de color de la sucursal
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_ManageColorBy) Then
                oGestionColor = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_ManageColorBy
            Else
                oGestionColor = GestionColor.RazonCita
            End If

            'Obtiene la configuración del tamaño de las celdas de la agenda
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_SchSizeMode) Then
                oModoCeldasAgenda = DMS_Connector.Configuracion.ParamGenAddon.U_SchSizeMode
            Else
                oModoCeldasAgenda = ModoCeldasAgenda.Estandar
            End If

            'Obtiene la configuración de la versión del módulo de citas a utilizar
            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType) Then
                oVersionModuloCita = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
            Else
                oVersionModuloCita = VersionModuloCita.Estandar
            End If

            strUsaColorAgenda = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = m_strCodSucursal).U_AgendaColor

            If String.IsNullOrEmpty(strUsaColorAgenda) Then
                strUsaColorAgenda = "N"
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

#Region "Metodos"

    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub LoadConsultaOcupacion()
        Try
            If oVersionModuloCita = VersionModuloCita.Estandar Then
                dtCitasProbl.Clear()
                tableCitasProbl.Clear()
                dtAgenda.Dispose()
                dtNombres.Dispose()

                dtAgenda = New System.Data.DataTable
                dtNombres = New System.Data.DataTable

                removerColumnasDataTables(dtAgenda)
                removerColumnasDataTables(dtCitasProbl)
                removerColumnasDataTables(dtNombres)

                DefinirColumnas()
                LoadEstiloGrid()
                CrearTablaAgenda()
                LoadEstiloGridCitasProblem()
                CreateDataTableCitasProblemas()
                CargarAgenda()
                LlenarReservacion()
                LlenarBloqueodeMecanicos()
                dgv_AgendaCitas.DataSource = dtAgenda
                LlenarOcupacion()
                LoadCitasProblemas()
                ActualizaTextoFecha()

                ManejorDataGridView()
                dtgvCitasReasignar.DataSource = dtCitasProbl

                CrearTablaAgendaNombres()
                CargarAgendaNombres()

                LimpiarDatosSeleccionAsesorTecnico()
                AjustarCeldas()
            Else
                CargarAgenda(False)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo encargado de ajustar el tamaño de las celdas (Alto y ancho) de acuerdo a las configuraciones generales
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AjustarCeldas()
        Dim oAutoSizeMode As DataGridViewAutoSizeColumnMode
        Dim strFuente As String = String.Empty
        Try
            If Not oModoCeldasAgenda = ModoCeldasAgenda.Estandar Then
                Select Case oModoCeldasAgenda
                    Case ModoCeldasAgenda.Estandar
                        'No aplica
                    Case ModoCeldasAgenda.TamanoEncabezados
                        oAutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                    Case ModoCeldasAgenda.Ajustar
                        oAutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End Select

                dgv_AgendaCitas.ColumnHeadersDefaultCellStyle.Font = New Font(dgv_AgendaCitas.DefaultFont.Name, 6.75, FontStyle.Regular)
                dgv_AgendaCitas.DefaultCellStyle.Font = New Font(dgv_AgendaCitas.DefaultFont.Name, 6.75, FontStyle.Regular)

                'Recorre todas las columnas y les asigna el AutoSizeMode
                For Each oColumn As DataGridViewColumn In dgv_AgendaCitas.Columns
                    If Not String.IsNullOrEmpty(oColumn.HeaderText) AndAlso oColumn.HeaderText.Contains(":") Then
                        oColumn.AutoSizeMode = oAutoSizeMode
                    Else
                        oColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                        If oColumn.Index = 2 Or oColumn.Index = 3 Then
                            oColumn.Width = 70
                        Else
                            oColumn.Width = 50
                        End If
                    End If
                Next
            End If

            'dgv_AgendaCitas.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText

        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    Public Sub LimpiarDatosSeleccionAsesorTecnico()
        m_oCeldaAsesor._FilNum = 0
        m_oCeldaAsesor._ColNum = 0
        m_oCeldaAsesor._Equipo = String.Empty
        m_oCeldaAsesor._Texto = String.Empty
        m_oCeldaAsesor._Rol = String.Empty

        m_oCeldaTecnico._FilNum = 0
        m_oCeldaTecnico._ColNum = 0
        m_oCeldaTecnico._Equipo = String.Empty
        m_oCeldaTecnico._Texto = String.Empty
        m_oCeldaTecnico._Rol = String.Empty


    End Sub

    Public Sub CargarAgenda()

        Dim dtConsulta As System.Data.DataTable
        Dim dtSucursal As System.Data.DataTable
        Dim l_strSQLSucursal As String
        Dim strPosicion As String = String.Empty

        Dim oDataRow As DataRow

        m_listRows = New List(Of infoRows)()

        Dim strConsulta As String = String.Empty

        If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
            strConsulta = String.Format("SELECT DocEntry, DocNum, U_Agenda, (SELECT TOP 1 S1.Name FROM OHEM S0  WITH (nolock) INNER JOIN OHPS S1 ON S0.position = S1.posID WHERE S0.empID = U_CodAsesor) AS 'DscPosicion' from  [@SCGD_AGENDA] with (nolock) where U_Cod_Sucursal = '{0}' and U_EstadoLogico = 'Y'", m_strCodSucursal)

        ElseIf m_tipoAgendaCargar = TipoDeAgenda.Equipos Then


            strConsulta = "Select HE.U_SCGD_Equipo, HE.U_SCGD_TipoEmp, HE.empID, HE.lastName + ' ' + HE.firstName as name, HE.U_SCGD_TiempServ , AG.U_Agenda, AG.DocEntry, AG.U_IntervaloCitas, T0.name AS 'DscPosicion'  " +
                            " from OHEM HE with (nolock) " +
                            " left outer JOIN [@SCGD_AGENDA] AG with (nolock) on AG.U_CodAsesor = HE.empID  " +
                            " LEFT JOIN OHPS T0 ON HE.position = T0.posID " +
                            " where(U_SCGD_Equipo Is Not null)" +
                            " AND HE.U_SCGD_TipoEmp is not null " +
                            " AND HE.U_SCGD_Equipo in  " +
                            "	(Select HE.U_SCGD_Equipo from OHEM HE with (nolock) 	" +
                            "			left outer join [@SCGD_AGENDA] AG on AG.U_CodAsesor = HE.empID	" +
                            "			where U_SCGD_TipoEmp = 'A'" +
                            "				and AG.U_EstadoLogico = 'Y' " +
                            "				and U_SCGD_Equipo is not null " +
                            "				and  HE.U_SCGD_TipoEmp is not null )  "

            If DMS_Connector.Company.AdminInfo.EnableBranches = BoYesNoEnum.tNO Then
                strConsulta = String.Format("{0} {1}", strConsulta, " AND HE.branch = '{0}' ")
            Else
                strConsulta = String.Format("{0} {1}", strConsulta, " AND HE.BPLId = '{0}' ")
            End If

            If Not m_strNumGrupo.Equals("-1") AndAlso Not String.IsNullOrEmpty(m_strNumGrupo) Then
                strConsulta = strConsulta + " AND U_SCGD_Equipo = '" + m_strNumGrupo + "' "
            End If

            strConsulta = strConsulta + " order by U_SCGD_Equipo ASC, U_SCGD_TipoEmp ASC, T0.name ASC "
            strConsulta = String.Format(strConsulta, m_strCodSucursal)

        End If

        dtConsulta = Utilitarios.EjecutarConsultaDataTable(strConsulta)
        Dim intCont As Integer = 0
        Try

            For Each row As DataRow In dtConsulta.Rows
                oDataRow = dtAgenda.NewRow()

                With oDataRow

                    If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                        .Item(mc_strID) = row.Item("DocEntry")
                        .Item(mc_strName) = row.Item("U_Agenda")
                        .Item("Posicion") = row.Item("DscPosicion")
                    ElseIf m_tipoAgendaCargar = TipoDeAgenda.Equipos Then
                        If row.Item("U_SCGD_TipoEmp") = "A" Then
                            .Item(mc_strID) = row.Item("DocEntry")
                            .Item(mc_strName) = row.Item("U_Agenda")
                            .Item(mc_strRol) = "A"
                            .Item(mc_strInterv) = row.Item("U_IntervaloCitas")
                            .Item("Posicion") = row.Item("DscPosicion")
                        ElseIf row.Item("U_SCGD_TipoEmp") = "T" Then
                            .Item(mc_strID) = row.Item("empID")
                            .Item(mc_strName) = row.Item("name")
                            .Item(mc_strRol) = "T"
                            .Item(mc_strInterv) = 15
                            .Item(mc_strServRap) = row.Item("U_SCGD_TiempServ")
                            strPosicion = row.Item("DscPosicion")
                            If Not String.IsNullOrEmpty(strPosicion) AndAlso Not strPosicion = "0" Then
                                .Item("Posicion") = strPosicion
                            End If
                        End If
                        .Item(mc_strEquipo) = row.Item("U_SCGD_Equipo")

                    End If

                    .Item(mc_strIDAgenda) = row.Item("DocEntry")

                    For Each column As String In listColumsGrid

                        If m_tipoAgendaCargar = TipoDeAgenda.Equipos Then
                            If row.Item("U_SCGD_TipoEmp") = "A" Then
                                .Item(column) = "."
                            Else
                                .Item(column) = String.Empty
                            End If
                        Else
                            .Item(column) = String.Empty
                        End If
                    Next

                End With

                If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                    m_InfoRows._position = intCont
                    m_InfoRows._Code = row.Item("DocEntry")
                    m_InfoRows._Name = row.Item("U_Agenda")
                    m_InfoRows._DocEntryAgenda = row.Item("DocEntry")

                ElseIf m_tipoAgendaCargar = TipoDeAgenda.Equipos Then

                    m_InfoRows._position = intCont
                    m_InfoRows._Equipo = row.Item("U_SCGD_Equipo")

                    If row.Item("U_SCGD_TipoEmp") = "A" Then
                        m_InfoRows._DocEntryAgenda = row.Item("DocEntry")
                        m_InfoRows._Code = row.Item("empID")
                        m_InfoRows._Name = row.Item("U_Agenda")
                        m_InfoRows._Rol = "A"
                        m_InfoRows._Intervalo = row.Item("U_IntervaloCitas")
                        m_InfoRows._ServRap = "N"
                    ElseIf row.Item("U_SCGD_TipoEmp") = "T" Then
                        m_InfoRows._DocEntryAgenda = String.Empty
                        m_InfoRows._Code = row.Item("empID")
                        m_InfoRows._Name = row.Item("name")
                        m_InfoRows._Rol = "T"
                        m_InfoRows._Intervalo = 15
                        m_InfoRows._ServRap = IIf(IsDBNull(row.Item("U_SCGD_TiempServ")), "", row.Item("U_SCGD_TiempServ"))

                    End If
                End If

                intCont += 1
                m_listRows.Add(m_InfoRows)
                dtAgenda.Rows.Add(oDataRow)
            Next

            l_strSQLSucursal = String.Format("SELECT U_HoraInicio, U_HoraFin FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = {0}", m_strCodSucursal)
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

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Public Sub CargarAgendaNombres()

        Dim dtConsulta As System.Data.DataTable
        Dim oDataRow As DataRow
        Dim strConsulta As String = String.Empty

        If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
            strConsulta = String.Format("SELECT DocEntry, DocNum, U_Agenda from  [@SCGD_AGENDA] where U_Cod_Sucursal = '{0}' and U_EstadoLogico = 'Y'", m_strCodSucursal)
        ElseIf m_tipoAgendaCargar = TipoDeAgenda.Equipos Then

            strConsulta = "Select HE.U_SCGD_Equipo, HE.U_SCGD_TipoEmp, HE.empID, HE.lastName + ' ' + HE.firstName as name, HE.U_SCGD_TiempServ , AG.U_Agenda, AG.DocEntry, AG.U_IntervaloCitas  " +
                                      " from OHEM HE with (nolock) " +
                                      " left outer JOIN [@SCGD_AGENDA] AG with (nolock) on AG.U_CodAsesor = HE.empID  " +
                                      " where(U_SCGD_Equipo Is Not null)" +
                                      " AND HE.U_SCGD_TipoEmp is not null " +
                                      " AND HE.U_SCGD_Equipo in  " +
                                      "	(Select HE.U_SCGD_Equipo from OHEM HE with (nolock) 	" +
                                      "			left outer join [@SCGD_AGENDA] AG on AG.U_CodAsesor = HE.empID	" +
                                      "			where U_SCGD_TipoEmp = 'A'" +
                                      "				and AG.U_EstadoLogico = 'Y' " +
                                      "				and U_SCGD_Equipo is not null " +
                                      "				and  HE.U_SCGD_TipoEmp is not null )  " +
                                      " AND HE.branch = '{0}' "

            If Not m_strNumGrupo.Equals("-1") AndAlso Not String.IsNullOrEmpty(m_strNumGrupo) Then
                strConsulta = strConsulta + " AND U_SCGD_Equipo = '" + m_strNumGrupo + "' "
            End If

            strConsulta = strConsulta + " order by U_SCGD_Equipo ASC, U_SCGD_TipoEmp ASC "
            strConsulta = String.Format(strConsulta, m_strCodSucursal)

        End If

        dtConsulta = Utilitarios.EjecutarConsultaDataTable(strConsulta)

        Try
            For Each row As DataRow In dtConsulta.Rows
                oDataRow = dtNombres.NewRow()

                With oDataRow

                    If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                        .Item(mc_strName) = row.Item("U_Agenda")

                    ElseIf m_tipoAgendaCargar = TipoDeAgenda.Equipos Then

                        If row.Item("U_SCGD_TipoEmp") = "A" Then
                            .Item(mc_strName) = row.Item("U_Agenda")

                        ElseIf row.Item("U_SCGD_TipoEmp") = "T" Then
                            .Item(mc_strName) = row.Item("name")
                        End If
                    End If

                End With

                dtNombres.Rows.Add(oDataRow)
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Public Sub LlenarOcupacion()
        Try
            Select Case m_tipoAgendaCargar
                Case TipoDeAgenda.Equipos
                    LlenarOcupacionSuspensionHorario()
                    LlenarOcupacionReprogramadas()
                    LlenarOcupacionNoIniciadas()
                    LlenarOcupacionPorGruposAlmuerzo(dtAgenda)
                    LlenarOcupacionPorGruposAsesor(dtAgenda)
                    LlenarOcupacionPorGruposOrdenes(dtAgenda)
                    LlenarOcupacionPostPorGrupos_Tecnico()
                    LlenarOcupacionPorGruposTecnico(dtAgenda)
                Case TipoDeAgenda.Agenda
                    LlenarOcupacionPostPorAgenda()
                    LlenarOcupacionPorAgenda(dtAgenda)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function EsDiaLaboral(ByVal CodigoSucursal As String, ByRef Fecha As DateTime) As Boolean
        Dim Resultado As Boolean = True
        Dim AperturaLunesViernes As DateTime?
        Dim CierreLunesViernes As DateTime?
        Dim AperturaSabados As DateTime?
        Dim CierreSabados As DateTime?
        Dim AperturaDomingos As DateTime?
        Dim CierreDomingos As DateTime?
        Dim FechaMinima As DateTime?

        Try
            FechaMinima = New DateTime(1899, 12, 30)
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)) IsNot Nothing Then
                AperturaLunesViernes = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraInicio
                CierreLunesViernes = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFin

                AperturaSabados = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraIS
                CierreSabados = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFS

                AperturaDomingos = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraID
                CierreDomingos = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFD

                If (AperturaLunesViernes = FechaMinima Or CierreLunesViernes = FechaMinima) AndAlso Not EsFinSemana(Fecha) Then
                    Resultado = False
                End If

                If (AperturaSabados = FechaMinima Or CierreSabados = FechaMinima) AndAlso Fecha.DayOfWeek = DayOfWeek.Saturday Then
                    Resultado = False
                End If

                If (AperturaDomingos = FechaMinima Or CierreDomingos = FechaMinima) AndAlso Fecha.DayOfWeek = DayOfWeek.Sunday Then
                    Resultado = False
                End If
            End If

            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Resultado
        End Try
    End Function

    Private Function EsFinSemana(ByRef Fecha As DateTime)
        Dim Resultado As Boolean = False
        Try
            Select Case Fecha.DayOfWeek
                Case DayOfWeek.Monday
                    Resultado = False
                Case DayOfWeek.Tuesday
                    Resultado = False
                Case DayOfWeek.Wednesday
                    Resultado = False
                Case DayOfWeek.Thursday
                    Resultado = False
                Case DayOfWeek.Friday
                    Resultado = False
                Case DayOfWeek.Saturday
                    Resultado = True
                Case DayOfWeek.Sunday
                    Resultado = True
            End Select
            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Resultado
        End Try
    End Function

    ''' <summary>
    ''' Método encargado del procesamiento de la agenda, verifica que sea día laboral, consulta los documentos y grafica la ocupación de la agenda
    ''' </summary>
    ''' <param name="TipoAgenda">Tipo de agenda, si es sencilla o por equipos</param>
    ''' <param name="HorarioSucursal">Diccionario con el horario de la sucursal por cada día de la semana</param>
    ''' <param name="ListaIntervalos">Lista de intervalos de tiempo que representan las columnas donde se grafican los documentos</param>
    ''' <remarks></remarks>
    Private Sub LlenarAgenda(ByVal TipoAgenda As Integer, ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef ListaIntervalos As List(Of DateTime))
        Dim HoraAperturaSucursal As DateTime
        Dim HoraCierreSucursal As DateTime
        Dim HoraInicioAlmuerzo As DateTime
        Dim HoraFinAlmuerzo As DateTime
        Dim DuracionAlmuerzo As Integer = 0
        Dim Documentos As System.Data.DataTable
        Dim FechaSeleccionada As DateTime
        Dim FilaAgenda As DataRow
        Dim NumeroAtributos As Integer = 0
        Dim ConfiguracionColores As ConfiguracionColores

        Try
            FechaSeleccionada = dtpFecha.Value
            'Si el horario de la sucursal está correctamente configurado se procede a completar los datos de la agenda
            If EsDiaLaboral(FechaSeleccionada, HorarioSucursal) Then
                ConfiguracionColores = New ConfiguracionColores()
                NumeroAtributos = CalcularNoAtributos()
                GraficarHorasAlmuerzo(HorarioSucursal, FechaSeleccionada.DayOfWeek, dtAgenda, NumeroAtributos, ConfiguracionColores)
                Documentos = New System.Data.DataTable
                Documentos = ConsultarDocumentos(FechaSeleccionada, m_strCodSucursal, TipoAgenda)
                If Documentos.Rows.Count > 0 Then
                    tableCitasProbl.Clear()
                    'Recorre cada una de las líneas de la agenda,
                    'en caso de que el documento coincida con el asesor y técnico, se grafica la ocupación
                    For i As Integer = 0 To dtAgenda.Rows.Count - 1
                        FilaAgenda = dtAgenda.Rows.Item(i)
                        GraficarCeldas(i, Documentos, FilaAgenda, HorarioSucursal, FechaSeleccionada, ListaIntervalos, NumeroAtributos, ConfiguracionColores)
                    Next
                End If
            End If
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de graficar las celdas de la agenda con los distintos documentos
    ''' </summary>
    ''' <param name="NumeroFila">Número de fila de la agenda</param>
    ''' <param name="Documentos">Tabla con todos los documentos que se deben graficar</param>
    ''' <param name="FilaAgenda">Objeto Row con la información de la fila de la agenda</param>
    ''' <param name="HorarioSucursal">Diccionario con el horario de la sucursal</param>
    ''' <param name="FechaSeleccionada">Fecha seleccionada</param>
    ''' <param name="ListaIntervalos">Listado de los distintos intervalos o columnas con la horas de la agenda</param>
    ''' <param name="NumeroAtributos">Cantidad de atributos previos a las columnas de tipo hora donde se grafica la agenda</param>
    ''' <param name="ConfiguracionColores">Objeto con la configuración de los colores</param>
    ''' <remarks></remarks>
    Private Sub GraficarCeldas(ByVal NumeroFila As Integer, ByRef Documentos As System.Data.DataTable, ByRef FilaAgenda As DataRow, ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef FechaSeleccionada As DateTime, ByRef ListaIntervalos As List(Of DateTime), ByVal NumeroAtributos As Integer, ByRef ConfiguracionColores As ConfiguracionColores)
        Dim FilaDocumento As DataRow
        Dim empID As Integer
        Dim EmpleadoAgenda As Integer
        Dim IDAgendaDocumento As Integer
        Dim IDAgenda As Integer
        Dim TipoEmpleado As String = String.Empty
        Dim TipoDocumento As TipoDocumento
        Dim NumeroCita As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim RazonCita As String = String.Empty
        Dim NumeroOT As String = String.Empty
        Dim EstadoServicio As String = String.Empty
        Dim FechaInicio As DateTime
        Dim FechaFin As DateTime
        Dim Cantidad As Double
        Dim DuracionEstandar As Double
        Dim TiempoOtorgado As Double
        Dim TiempoServicio As Double
        Dim ColorCelda As Color
        Dim TipoColor As DMS_Addon.ConfiguracionColores.TipoColor
        Dim TextoCelda As String = String.Empty
        Dim CeldasPorGraficar As List(Of Integer)
        Dim ValorActual As String = String.Empty
        Dim NombreColorActual As String = String.Empty
        Dim ConflictoDetectado As Boolean
        Dim Placa As String = String.Empty
        Dim EsDiaPosterior As Boolean
        Dim NombreEmpleado As String = String.Empty
        Dim FilaAgendaID As String = String.Empty
        Dim FilaAgendaIdAgenda As String = String.Empty
        Dim MensajeError As String = String.Empty
        Try
            FilaAgendaID = If(IsDBNull(FilaAgenda.Item("ID")), String.Empty, FilaAgenda.Item("ID").ToString())
            FilaAgendaIdAgenda = If(IsDBNull(FilaAgenda.Item("IdAgenda")), String.Empty, FilaAgenda.Item("IdAgenda").ToString())
            For i As Integer = 0 To Documentos.Rows.Count - 1
                'El bloque try se utiliza para evitar que toda la agenda falle si existe algún valor inválido
                Try
                    FilaDocumento = Documentos.Rows.Item(i)
                    'Obtiene los datos del documento, en caso de que no sean válidos o no estén en el formato correcto, 
                    'simplemente se omiten y se continua con el siguiente documento
                    If ObtenerDatosFila(FilaDocumento, empID, IDAgendaDocumento, TipoEmpleado, TipoDocumento, NumeroCita, EstadoCita, RazonCita, NumeroOT, EstadoServicio, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio, Placa) Then
                        If Integer.TryParse(FilaAgendaID, EmpleadoAgenda) AndAlso EmpleadoAgenda = empID AndAlso Integer.TryParse(FilaAgendaIdAgenda, IDAgenda) AndAlso IDAgenda = IDAgendaDocumento Then
                            CeldasPorGraficar = New List(Of Integer)
                            TipoColor = ObtenerTipoColor(TipoDocumento)
                            FechaFin = CalcularFechaFinalizacion(TipoDocumento, HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio, EsDiaPosterior)
                            ColorCelda = ConfiguracionColores.ObtenerColor(TipoColor, NumeroCita, RazonCita, EstadoCita, NumeroOT, m_strCodSucursal, EsDiaPosterior)
                            TextoCelda = ObtenerDescripcionCelda(TipoDocumento, NumeroCita, EstadoCita, NumeroOT, Placa)
                            ConflictoDetectado = False
                            For j As Integer = 0 To ListaIntervalos.Count - 1
                                If ListaIntervalos.Item(j).TimeOfDay >= FechaInicio.TimeOfDay AndAlso ListaIntervalos.Item(j).TimeOfDay < FechaFin.TimeOfDay Then
                                    ValorActual = If(IsDBNull(FilaAgenda.Item(j + NumeroAtributos)), String.Empty, FilaAgenda.Item(j + NumeroAtributos))
                                    NombreColorActual = dgv_AgendaCitas.Rows(NumeroFila).Cells(j + NumeroAtributos).Style.BackColor.Name
                                    If Not EsConflicto(ValorActual, NombreColorActual) Or (EsMismoDocumento(ValorActual, Placa, NumeroCita, NumeroOT) And NombreColorActual = ColorCelda.Name) Then
                                        If ValorActual <> "n/a" Then
                                            CeldasPorGraficar.Add(j + NumeroAtributos)
                                        End If
                                    Else
                                        ConflictoDetectado = True
                                        Exit For
                                    End If
                                End If
                            Next

                            If Not ConflictoDetectado Then
                                For Each NumeroCelda As Integer In CeldasPorGraficar
                                    dgv_AgendaCitas.Rows(NumeroFila).Cells(NumeroCelda).Style.BackColor = ColorCelda
                                    FilaAgenda.Item(NumeroCelda) = TextoCelda
                                Next
                            Else
                                If EsDistintoDocumento(ValorActual, Placa, NumeroCita, NumeroOT) Then
                                    'Agregar conflicto a la tabla de conflictos
                                    If IsDBNull(FilaAgenda.Item("Name")) Then
                                        NombreEmpleado = String.Empty
                                    Else
                                        NombreEmpleado = FilaAgenda.Item("Name")
                                    End If
                                    AgregarConflicto(Placa, NumeroCita, NumeroOT, NombreEmpleado, FechaInicio, FechaFin)
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    If Not MensajeError = ex.Message Then
                        MensajeError = ex.Message
                        ManejoErroresAgenda(ex)
                    End If
                End Try
            Next
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    Private Function EsMismoDocumento(ByVal ValorCelda As String, ByVal Placa As String, ByVal NumeroCita As String, ByVal NumeroOT As String) As Boolean
        Dim Resultado As Boolean = False
        Try
            If Not String.IsNullOrEmpty(ValorCelda) Then
                If Not String.IsNullOrEmpty(NumeroCita) AndAlso ValorCelda.Contains(NumeroCita) Then
                    Resultado = True
                End If
                If Not String.IsNullOrEmpty(NumeroOT) AndAlso ValorCelda.Contains(NumeroOT) Then
                    Resultado = True
                End If
            End If
            Return Resultado
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Resultado
        End Try
    End Function

    Private Sub AgregarConflicto(ByVal Placa As String, ByVal NumeroCita As String, ByVal NumeroOT As String, ByVal NombreEmpleado As String, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime)
        Dim Valor As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(NombreEmpleado) Then
                Valor += String.Format("{0} {1}{2} ", My.Resources.Resource.Empleado, NombreEmpleado, Environment.NewLine)
            End If

            If Not String.IsNullOrEmpty(NumeroCita) Or Not String.IsNullOrEmpty(NumeroOT) Then
                Valor += vbTab
                If Not String.IsNullOrEmpty(NumeroCita) Then
                    Valor += String.Format(" | {0}: {1} ", My.Resources.Resource.Cita, NumeroCita)
                End If

                If Not String.IsNullOrEmpty(NumeroOT) Then
                    Valor += String.Format(" | {0}: {1} ", My.Resources.Resource.OrdenTrabajo, NumeroOT)
                End If

                Valor += Environment.NewLine
            End If

            Valor += String.Format(" | {0} {1} ", My.Resources.Resource.HoraInicio, FechaInicio.ToString())
            Valor += Environment.NewLine

            Valor += String.Format(" | {0} {1} ", My.Resources.Resource.HoraFin, FechaFin.ToString())
            Valor += Environment.NewLine

            tableCitasProbl.Add(tableCitasProbl.Count, Valor)
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    Private Function EsDistintoDocumento(ByVal ValorCelda As String, ByVal Placa As String, ByVal NumeroCita As String, ByVal NumeroOT As String) As Boolean
        Dim Resultado As Boolean = True
        Try
            If Not String.IsNullOrEmpty(ValorCelda) Then
                If Not String.IsNullOrEmpty(NumeroCita) AndAlso ValorCelda.Contains(NumeroCita) Then
                    Resultado = False
                End If
                If Not String.IsNullOrEmpty(NumeroOT) AndAlso ValorCelda.Contains(NumeroOT) Then
                    Resultado = False
                End If
            End If
            Return Resultado
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Resultado
        End Try
    End Function

    Private Sub CompletarColorColumnasAsesor(ByVal NumeroFila As Integer, ByRef FilaAgenda As DataRow, ByVal CeldaInicial As Integer, ByVal CeldaFinal As Integer)
        Dim ValorCelda As String = String.Empty
        Try
            For i As Integer = CeldaInicial To CeldaFinal
                ValorCelda = FilaAgenda.Item(i)
                If ValorCelda = "." Then
                    dgv_AgendaCitas.Rows(NumeroFila).Cells(i).Style.BackColor = Color.LightGray
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function EsConflicto(ByVal ValorCelda As String, ByVal NombreColorCelda As String) As Boolean
        Try
            Select Case ValorCelda
                Case "n/a"
                    Return False
                Case "."
                    Return False
                Case Else
                    If NombreColorCelda <> "0" Then
                        Return True
                    End If
            End Select
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Function

    Private Function ObtenerDescripcionCelda(ByRef TipoDocumento As TipoDocumento, ByRef NumeroCita As String, ByRef EstadoCita As String, ByRef NumeroOT As String, ByRef Placa As String) As String
        Dim Descripcion As String = String.Empty
        Try
            Select Case TipoDocumento
                Case frmListaCitas.TipoDocumento.Cita
                    If Not String.IsNullOrEmpty(Placa) Then
                        Descripcion = String.Format("{0}/{1}", Placa, NumeroCita)
                    Else
                        Descripcion = NumeroCita
                    End If
                Case frmListaCitas.TipoDocumento.CitaAsesor
                    If Not String.IsNullOrEmpty(Placa) Then
                        Descripcion = String.Format("{0}/{1}", Placa, NumeroCita)
                    Else
                        Descripcion = NumeroCita
                    End If
                Case frmListaCitas.TipoDocumento.OrdenTrabajo
                    If Not String.IsNullOrEmpty(Placa) Then
                        Descripcion = String.Format("{0}/{1}", Placa, NumeroOT)
                    Else
                        Descripcion = NumeroOT
                    End If
                Case frmListaCitas.TipoDocumento.Bloqueo
                    Descripcion = "n/a"
                Case frmListaCitas.TipoDocumento.Suspension
                    If Not String.IsNullOrEmpty(Placa) Then
                        Descripcion = String.Format("{0}/{1}", Placa, NumeroOT)
                    Else
                        Descripcion = NumeroOT
                    End If
                Case frmListaCitas.TipoDocumento.Reprogramada
                    If Not String.IsNullOrEmpty(Placa) Then
                        Descripcion = String.Format("{0}/{1}", Placa, NumeroOT)
                    Else
                        Descripcion = NumeroOT
                    End If
                Case frmListaCitas.TipoDocumento.ServicioNoIniciado
                    If Not String.IsNullOrEmpty(Placa) Then
                        Descripcion = String.Format("{0}/{1}", Placa, NumeroOT)
                    Else
                        Descripcion = NumeroOT
                    End If
            End Select
            Return Descripcion
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Descripcion
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el tipo de color de acuerdo al tipo de documento
    ''' </summary>
    ''' <param name="TipoDocumento">Tipo de documento</param>
    ''' <returns>Tipo de color</returns>
    ''' <remarks></remarks>
    Private Function ObtenerTipoColor(ByRef TipoDocumento As TipoDocumento) As DMS_Addon.ConfiguracionColores.TipoColor
        Try
            Select Case TipoDocumento
                Case frmListaCitas.TipoDocumento.Cita
                    Return DMS_Addon.ConfiguracionColores.TipoColor.Cita
                Case frmListaCitas.TipoDocumento.CitaAsesor
                    Return DMS_Addon.ConfiguracionColores.TipoColor.Cita
                Case frmListaCitas.TipoDocumento.OrdenTrabajo
                    Return DMS_Addon.ConfiguracionColores.TipoColor.OrdenTrabajo
                Case frmListaCitas.TipoDocumento.Bloqueo
                    Return DMS_Addon.ConfiguracionColores.TipoColor.Bloqueo
                Case frmListaCitas.TipoDocumento.Suspension
                    Return DMS_Addon.ConfiguracionColores.TipoColor.SuspensionHorario
                Case frmListaCitas.TipoDocumento.Reprogramada
                    Return DMS_Addon.ConfiguracionColores.TipoColor.Reprogramada
                Case frmListaCitas.TipoDocumento.ServicioNoIniciado
                    Return DMS_Addon.ConfiguracionColores.TipoColor.ServicioNoIniciado
            End Select
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene los distintos valores de la fila y los convierte a un formato adecuado para su manipulación en la agenda
    ''' cualquier tarea de conversión, limpieza de datos y similares se realiza en este método
    ''' </summary>
    ''' <param name="FilaDocumento">Fila del Documento</param>
    ''' <param name="empID">Código del empleado</param>
    ''' <param name="IDAgendaDocumento">ID de la agenda</param>
    ''' <param name="TipoEmpleado">Tipo de empleado (Técnico, Asesor, Supervisor, ...)</param>
    ''' <param name="TipoDocumento">Tipo de documento que indica como se debe procesar la fila</param>
    ''' <param name="NumeroCita">Número de cita (Serie-Consecutivo)</param>
    ''' <param name="EstadoCita">Código del estado de la cita</param>
    ''' <param name="RazonCita">Código de la razón de la cita</param>
    ''' <param name="NumeroOT">Número de orden de trabajo</param>
    ''' <param name="EstadoServicio">Estado del servicio</param>
    ''' <param name="FechaInicio">Fecha de inicio (Fecha y hora)</param>
    ''' <param name="FechaFin">Fecha de fin (Fecha y hora)</param>
    ''' <param name="Cantidad">Cantidad de servicios en la línea. En caso de ser cero, no se multiplica la duración estándar, tiempo otorgado ni el tiempo de servicio</param>
    ''' <param name="DuracionEstandar">Duración de la actividad</param>
    ''' <param name="TiempoOtorgado">Tiempo adicional otorgado a la actividad</param>
    ''' <param name="TiempoServicio">Tiempo de servicio</param>
    ''' <param name="Placa">Número de placa</param>
    ''' <returns>True = Datos válidos y obtenidos correctamente. False = Se produjeron errores, datos inválidos, faltantes o en el formato incorrecto</returns>
    ''' <remarks></remarks>
    Private Function ObtenerDatosFila(ByRef FilaDocumento As System.Data.DataRow, ByRef empID As Integer, ByRef IDAgendaDocumento As Integer, ByRef TipoEmpleado As String, ByRef TipoDocumento As TipoDocumento, ByRef NumeroCita As String, ByRef EstadoCita As String, ByRef RazonCita As String, ByRef NumeroOT As String, ByRef EstadoServicio As String, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef Cantidad As Double, ByRef DuracionEstandar As Double, ByRef TiempoOtorgado As Double, ByRef TiempoServicio As Double, ByRef Placa As String) As Boolean
        Dim TextoFechaInicio As String = String.Empty
        Dim TextoHoraInicio As String = String.Empty
        Dim TextoFechaFin As String = String.Empty
        Dim TextoHoraFin As String = String.Empty
        Dim FechaTemporal As DateTime
        Dim TextoFecha As String = String.Empty
        Dim RedondeoInicio As Integer = 0
        Dim RedondeoFin As Integer = 0
        Try
            empID = Convert.ToInt32(FilaDocumento.Item("empID"))
            IDAgendaDocumento = Convert.ToInt32(FilaDocumento.Item("IdAgenda"))
            TipoEmpleado = If(IsDBNull(FilaDocumento.Item("TipoEmpleado")), String.Empty, FilaDocumento.Item("TipoEmpleado").ToString)
            TipoDocumento = If(IsDBNull(FilaDocumento.Item("TipoDocumento")), String.Empty, FilaDocumento.Item("TipoDocumento").ToString)
            NumeroCita = If(IsDBNull(FilaDocumento.Item("NumeroCita")), String.Empty, FilaDocumento.Item("NumeroCita").ToString)
            EstadoCita = If(IsDBNull(FilaDocumento.Item("EstadoCita")), String.Empty, FilaDocumento.Item("EstadoCita").ToString)
            RazonCita = If(IsDBNull(FilaDocumento.Item("RazonCita")), String.Empty, FilaDocumento.Item("RazonCita").ToString)
            NumeroOT = If(IsDBNull(FilaDocumento.Item("NumeroOT")), String.Empty, FilaDocumento.Item("NumeroOT").ToString)
            EstadoServicio = If(IsDBNull(FilaDocumento.Item("EstadoServicio")), String.Empty, FilaDocumento.Item("EstadoServicio").ToString)
            Cantidad = Convert.ToDouble(FilaDocumento.Item("Cantidad"))
            DuracionEstandar = Convert.ToDouble(FilaDocumento.Item("DuracionEstandar"))
            TiempoOtorgado = Convert.ToDouble(FilaDocumento.Item("TiempoOtorgado"))
            TiempoServicio = Convert.ToDouble(FilaDocumento.Item("TiempoServicio"))
            TextoFechaInicio = If(IsDBNull(FilaDocumento.Item("FechaInicio")), String.Empty, FilaDocumento.Item("FechaInicio").ToString)
            TextoHoraInicio = If(IsDBNull(FilaDocumento.Item("HoraInicio")), String.Empty, FilaDocumento.Item("HoraInicio").ToString)
            TextoFechaFin = If(IsDBNull(FilaDocumento.Item("FechaFin")), String.Empty, FilaDocumento.Item("FechaFin").ToString)
            TextoHoraFin = If(IsDBNull(FilaDocumento.Item("HoraFin")), String.Empty, FilaDocumento.Item("HoraFin").ToString)
            TextoHoraInicio = AjustarFormatoHoras(TextoHoraInicio)
            RedondeoInicio = RedondearMinutos(TextoHoraInicio)
            TextoHoraFin = AjustarFormatoHoras(TextoHoraFin)
            RedondeoFin = RedondearMinutos(TextoHoraFin)
            Placa = If(IsDBNull(FilaDocumento.Item("Placa")), String.Empty, FilaDocumento.Item("Placa").ToString())

            If Not String.IsNullOrEmpty(TextoFechaInicio) AndAlso Not String.IsNullOrEmpty(TextoHoraInicio) Then
                TextoFecha = String.Format("{0} {1}", DateTime.Parse(FilaDocumento.Item("FechaInicio")).ToString("yyyyMMdd"), TextoHoraInicio)
                FechaInicio = DateTime.ParseExact(TextoFecha, "yyyyMMdd HHmm", Nothing)
                If RedondeoInicio > 0 Then
                    FechaInicio = FechaInicio.AddMinutes(RedondeoInicio - FechaInicio.Minute)
                End If
            End If

            If Not String.IsNullOrEmpty(TextoFechaFin) AndAlso Not String.IsNullOrEmpty(TextoHoraFin) AndAlso Not TextoHoraFin = 0 Then
                TextoFecha = String.Format("{0} {1}", DateTime.Parse(FilaDocumento.Item("FechaFin")).ToString("yyyyMMdd"), TextoHoraFin)
                FechaFin = DateTime.ParseExact(TextoFecha, "yyyyMMdd HHmm", Nothing)
                If RedondeoFin > 0 Then
                    FechaFin = FechaFin.AddMinutes(RedondeoFin - FechaFin.Minute)
                End If
            End If

            Return True
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return False
        End Try
    End Function

    'Respaldo 15 03 2019 previo a los cambios en los métodos de redondeo
    'Private Function ObtenerDatosFila(ByRef FilaDocumento As System.Data.DataRow, ByRef empID As Integer, ByRef IDAgendaDocumento As Integer, ByRef TipoEmpleado As String, ByRef TipoDocumento As TipoDocumento, ByRef NumeroCita As String, ByRef EstadoCita As String, ByRef RazonCita As String, ByRef NumeroOT As String, ByRef EstadoServicio As String, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef Cantidad As Double, ByRef DuracionEstandar As Double, ByRef TiempoOtorgado As Double, ByRef TiempoServicio As Double, ByRef Placa As String) As Boolean
    '    Dim TextoFechaInicio As String = String.Empty
    '    Dim TextoHoraInicio As String = String.Empty
    '    Dim TextoFechaFin As String = String.Empty
    '    Dim TextoHoraFin As String = String.Empty
    '    Dim FechaTemporal As DateTime
    '    Dim TextoFecha As String = String.Empty
    '    Dim RedondeoInicio As Integer = 0
    '    Dim RedondeoFin As Integer = 0
    '    Try
    '        empID = Convert.ToInt32(FilaDocumento.Item("empID"))
    '        IDAgendaDocumento = Convert.ToInt32(FilaDocumento.Item("IdAgenda"))
    '        TipoEmpleado = FilaDocumento.Item("TipoEmpleado").ToString
    '        TipoDocumento = FilaDocumento.Item("TipoDocumento").ToString
    '        NumeroCita = FilaDocumento.Item("NumeroCita").ToString
    '        EstadoCita = FilaDocumento.Item("EstadoCita").ToString
    '        RazonCita = FilaDocumento.Item("RazonCita").ToString
    '        NumeroOT = FilaDocumento.Item("NumeroOT").ToString
    '        EstadoServicio = FilaDocumento.Item("EstadoServicio").ToString
    '        Cantidad = Convert.ToDouble(FilaDocumento.Item("Cantidad"))
    '        DuracionEstandar = Convert.ToDouble(FilaDocumento.Item("DuracionEstandar"))
    '        TiempoOtorgado = Convert.ToDouble(FilaDocumento.Item("TiempoOtorgado"))
    '        TiempoServicio = Convert.ToDouble(FilaDocumento.Item("TiempoServicio"))
    '        TextoFechaInicio = FilaDocumento.Item("FechaInicio").ToString
    '        TextoHoraInicio = FilaDocumento.Item("HoraInicio").ToString
    '        TextoFechaFin = FilaDocumento.Item("FechaFin").ToString
    '        TextoHoraFin = FilaDocumento.Item("HoraFin").ToString
    '        TextoHoraInicio = AjustarFormatoHoras(TextoHoraInicio)
    '        TextoHoraInicio = RegresaHora(TextoHoraInicio) 'Redondea la hora
    '        TextoHoraFin = AjustarFormatoHoras(TextoHoraFin)
    '        TextoHoraFin = RegresaHora(TextoHoraFin) 'Redondea la hora
    '        Placa = FilaDocumento.Item("Placa").ToString()

    '        If Not String.IsNullOrEmpty(TextoFechaInicio) AndAlso Not String.IsNullOrEmpty(TextoHoraInicio) Then
    '            TextoFecha = String.Format("{0} {1}", DateTime.Parse(FilaDocumento.Item("FechaInicio")).ToString("yyyyMMdd"), TextoHoraInicio)
    '            FechaInicio = DateTime.ParseExact(TextoFecha, "yyyyMMdd HHmm", Nothing)
    '        End If

    '        If Not String.IsNullOrEmpty(TextoFechaFin) AndAlso Not String.IsNullOrEmpty(TextoHoraFin) AndAlso Not TextoHoraFin = 0 Then
    '            TextoFecha = String.Format("{0} {1}", DateTime.Parse(FilaDocumento.Item("FechaFin")).ToString("yyyyMMdd"), TextoHoraFin)
    '            FechaFin = DateTime.ParseExact(TextoFecha, "yyyyMMdd HHmm", Nothing)
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        ManejoErroresAgenda(ex)
    '        Return False
    '    End Try
    'End Function

    Private Function RedondearMinutos(ByVal Hora As String) As Integer
        Dim MinutosTexto As String = String.Empty
        Dim Minutos As Integer = 0

        Try
            If Not String.IsNullOrEmpty(Hora) Then
                Select Case Hora.Length
                    Case 1
                        'La hora no puede ser cero
                        If Not Hora.Equals("0") Then
                            'La hora no tiene detalle de los minutos, por lo tanto se toma como minutos 00
                            MinutosTexto = "00"
                        End If
                    Case 2
                        'La hora no tiene detalle de los minutos, por lo tanto se toma como minutos 00
                        MinutosTexto = "00"
                    Case 3
                        'El primer número representa la hora, los siguientes dos los minutos
                        MinutosTexto = Hora.Substring(1)
                    Case 4
                        'Los primeros dos números representan la hora, los siguientes dos los minutos
                        MinutosTexto = Hora.Substring(2)
                End Select

                If Not String.IsNullOrEmpty(MinutosTexto) Then
                    'Se realiza un redondeo a múltiplos de 15, esto debido a que la agenda se administra
                    'en intervalos de 15 minutos solamente. Ejemplo: 0, 15, 30, 45.
                    Minutos = Convert.ToInt32(MinutosTexto)

                    Select Case Minutos
                        Case 1 To 14
                            Minutos = 15
                        Case 16 To 29
                            Minutos = 30
                        Case 31 To 44
                            Minutos = 45
                        Case 46 To 59
                            Minutos = 60
                    End Select
                End If
            End If
            Return Minutos
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Function

    Private Function AjustarFormatoHoras(ByVal Hora As String) As String
        Dim HoraFormateada As String = String.Empty
        Try
            If String.IsNullOrEmpty(Hora) Then
                HoraFormateada = String.Empty
            Else
                If Hora.Length = 4 Then
                    HoraFormateada = Hora
                End If
                If Hora.Length = 3 Then
                    HoraFormateada = String.Format("{0}{1}", "0", Hora)
                End If
                If Hora.Length < 3 Then
                    HoraFormateada = String.Empty
                End If
            End If

            If Hora.Contains(":") Then
                Hora = Hora.Replace(".", String.Empty)
            End If

            Return HoraFormateada
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return HoraFormateada
        End Try
    End Function

    ''' <summary>
    ''' Calcula la fecha de finalización de los documentos
    ''' en caso de ser necesario realiza ajustes a la hora final o corrige datos faltantes cuando sea posible
    ''' </summary>
    ''' <param name="TipoDocumento">Tipo de documento</param>
    ''' <param name="HorarioSucursal">Diccionario con la información del horario de la sucursal por cada día</param>
    ''' <param name="FechaSeleccionada">Fecha Seleccionada</param>
    ''' <param name="FechaInicio">Fecha de inicio</param>
    ''' <param name="FechaFin">Fecha de Fin</param>
    ''' <param name="Cantidad">Cantidad, en caso de ser cero, se toma la duración estándar, tiempo otorgado, tiempo servicio como valores totalizados</param>
    ''' <param name="DuracionEstandar">Duración estándar del servicio</param>
    ''' <param name="TiempoOtorgado">Tiempo adicional otorgado al servicio</param>
    ''' <param name="TiempoServicio">Tiempo de servicio</param>
    ''' <param name="EsDiaPosterior">True = El documento inicia en una fecha anterior. False = El documento inicia y finaliza en la fecha seleccionada.</param>
    ''' <returns>True = Fecha calculada correctamente. False = Ocurrió un error.</returns>
    ''' <remarks></remarks>
    Private Function CalcularFechaFinalizacion(ByRef TipoDocumento As TipoDocumento, ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef FechaSeleccionada As DateTime, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef Cantidad As Double, ByRef DuracionEstandar As Double, ByRef TiempoOtorgado As Double, ByRef TiempoServicio As Double, ByRef EsDiaPosterior As Boolean) As DateTime
        Dim HoraCierreDelDia As DateTime
        Try
            If FechaFin = DateTime.MinValue Then
                FechaFin = New DateTime(FechaInicio.Year, FechaInicio.Month, FechaInicio.Day, FechaInicio.Hour, FechaInicio.Minute, 0)
                FechaFin = FechaFin.AddMinutes(15)
            End If

            EsDiaPosterior = False

            Select Case TipoDocumento
                Case frmListaCitas.TipoDocumento.Cita
                    CalcularFechaFinalizacionCita(HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio, EsDiaPosterior)
                Case frmListaCitas.TipoDocumento.CitaAsesor
                    CalcularFechasCitaAsesor(HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio, EsDiaPosterior)
                Case frmListaCitas.TipoDocumento.Bloqueo
                    'Se mantiene la misma fecha de finalización sin cambios
                Case frmListaCitas.TipoDocumento.OrdenTrabajo
                    CalcularFechaFinalizacionServicio(HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio)
                Case frmListaCitas.TipoDocumento.Suspension
                    CalcularFechaFinalizacionServicio(HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio)
                Case frmListaCitas.TipoDocumento.Reprogramada
                    CalcularFechaFinalizacionServicio(HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio)
                Case frmListaCitas.TipoDocumento.ServicioNoIniciado
                    CalcularFechaFinalizacionServicio(HorarioSucursal, FechaSeleccionada, FechaInicio, FechaFin, Cantidad, DuracionEstandar, TiempoOtorgado, TiempoServicio)
            End Select
            HoraCierreDelDia = New DateTime(FechaSeleccionada.Year, FechaSeleccionada.Month, FechaSeleccionada.Day, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraCierre.Hour, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraCierre.Minute, 0)
            'Si la fecha de finalización supera el día, se utiliza la fecha de cierre de sucursal como fecha de finalización
            If FechaFin > HoraCierreDelDia Then
                FechaFin = New DateTime(FechaSeleccionada.Year, FechaSeleccionada.Month, FechaSeleccionada.Day, HoraCierreDelDia.Hour, HoraCierreDelDia.Minute, 0)
            End If
            Return FechaFin
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return FechaFin
        End Try
    End Function

    Private Function CalcularFechasCitaAsesor(ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef FechaSeleccionada As DateTime, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef Cantidad As Double, ByRef DuracionEstandar As Double, ByRef TiempoOtorgado As Double, ByRef TiempoServicio As Double, ByRef EsDiaPosterior As Boolean) As DateTime
        Dim HorarioApertura As DateTime
        Try
            HorarioApertura = New DateTime(FechaSeleccionada.Year, FechaSeleccionada.Month, FechaSeleccionada.Day, HorarioSucursal(FechaSeleccionada.DayOfWeek).HoraApertura.Hour, HorarioSucursal(FechaSeleccionada.DayOfWeek).HoraApertura.Minute, 0)

            If FechaInicio.DayOfYear <> HorarioApertura.DayOfYear Then
                EsDiaPosterior = True
            End If

            If FechaInicio < HorarioApertura Then
                FechaInicio = New DateTime(HorarioApertura.Year, HorarioApertura.Month, HorarioApertura.Day, HorarioApertura.Hour, HorarioApertura.Minute, 0)
            End If
            If FechaFin <= FechaInicio Then
                'La fecha fin no puede ser menor o igual a la fecha de inicio
                FechaFin = FechaInicio.AddMinutes(15)
            End If
            Return FechaFin
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return FechaFin
        End Try
    End Function

    Private Function CalcularFechaFinalizacionCita(ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef FechaSeleccionada As DateTime, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef Cantidad As Double, ByRef DuracionEstandar As Double, ByRef TiempoOtorgado As Double, ByRef TiempoServicio As Double, ByRef EsDiaPosterior As Boolean) As DateTime
        'La duración mínima de cualquier actividad es de 15 minutos
        Dim DuracionTotal As Integer = 15
        Dim HorarioApertura As DateTime
        Try
            HorarioApertura = New DateTime(FechaSeleccionada.Year, FechaSeleccionada.Month, FechaSeleccionada.Day, HorarioSucursal(FechaSeleccionada.DayOfWeek).HoraApertura.Hour, HorarioSucursal(FechaSeleccionada.DayOfWeek).HoraApertura.Minute, 0)

            If FechaInicio.DayOfYear <> HorarioApertura.DayOfYear Then
                EsDiaPosterior = True
            End If

            If FechaInicio < HorarioApertura Then
                FechaInicio = New DateTime(HorarioApertura.Year, HorarioApertura.Month, HorarioApertura.Day, HorarioApertura.Hour, HorarioApertura.Minute, 0)
            End If

            If Not EsDiaPosterior Then
                If DuracionEstandar > 0 Or TiempoServicio > 0 Then
                    If TiempoServicio > 0 Then
                        DuracionTotal = TiempoServicio + TiempoOtorgado
                    Else
                        DuracionTotal = DuracionEstandar + TiempoOtorgado
                    End If
                End If
                FechaFin = FechaInicio.AddMinutes(DuracionTotal)
            End If

            If HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).MinutosAlmuerzo > 0 Then
                'Si la hora abarca parte de la hora de almuerzo, se debe sumar la duración del almuerzo
                If IntersecaAlmuerzo(FechaInicio, FechaFin, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraInicioAlmuerzo, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraFinAlmuerzo, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).MinutosAlmuerzo) Then
                    FechaFin = FechaFin.AddMinutes(HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).MinutosAlmuerzo)
                End If
            End If

            Return FechaFin
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return FechaFin
        End Try
    End Function

    Private Function CalcularFechaFinalizacionServicio(ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef FechaSeleccionada As DateTime, ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef Cantidad As Double, ByRef DuracionEstandar As Double, ByRef TiempoOtorgado As Double, ByRef TiempoServicio As Double) As DateTime
        'La duración mínima de cualquier actividad es de 15 minutos
        Dim DuracionTotal As Integer = 15

        Try
            'Orden de prioridades para calcular la duración de una actividad
            '1-Servicio rápido
            '2-Duración estándar

            'Determinamos si se utiliza el servicio rápido o el tiempo estándar
            If TiempoServicio > 0 Then
                'Prioridad 1 Servicio rápido
                DuracionTotal = TiempoServicio + TiempoOtorgado
            Else
                'Prioridad 2 Duración estándar
                DuracionTotal = DuracionEstandar + TiempoOtorgado
            End If

            'La duración mínima para cualquier actividad es de 15 minutos
            If DuracionTotal < 15 Then
                DuracionTotal = 15
            End If

            If UsaVariosDias(FechaInicio, DuracionTotal) Then
                FechaFin = HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraCierre
            Else
                FechaFin = FechaInicio.AddMinutes(DuracionTotal)
                If HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).MinutosAlmuerzo > 0 Then
                    'Si la hora abarca parte de la hora de almuerzo, se debe sumar la duración del almuerzo
                    If IntersecaAlmuerzo(FechaInicio, FechaFin, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraInicioAlmuerzo, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraFinAlmuerzo, HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).MinutosAlmuerzo) Then
                        FechaFin = FechaFin.AddMinutes(HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).MinutosAlmuerzo)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Function IntersecaAlmuerzo(ByRef FechaInicio As DateTime, ByRef FechaFin As DateTime, ByRef HoraInicioAlmuerzo As DateTime, ByRef HoraFinAlmuerzo As DateTime, ByVal MinutosAlmuerzo As Integer) As Boolean
        Dim Resultado As Boolean = False
        Try
            If FechaInicio.TimeOfDay <= HoraInicioAlmuerzo.TimeOfDay AndAlso FechaFin.TimeOfDay > HoraInicioAlmuerzo.TimeOfDay Then
                Resultado = True
            End If
            Return Resultado
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Resultado
        End Try
    End Function

    Private Function EsDiaLaboral(ByRef FechaSeleccionada As DateTime, ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario)) As Boolean
        Dim Resultado As Boolean = False
        Try
            If HorarioSucursal.ContainsKey(FechaSeleccionada.DayOfWeek) Then
                If HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HorarioConfigurado = True Then
                    Resultado = True
                End If
            End If
            Return Resultado
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Resultado
        End Try
    End Function

    ''' <summary>
    ''' Consulta el horario de la sucursal y marca las casillas de horario de almuerzo como bloqueadas para evitar que se grafique encima
    ''' </summary>
    ''' <param name="HorarioSucursal">Diccionario con la información del horario de la sucursal por cada día de la semana</param>
    ''' <param name="Dia">Día de la semana</param>
    ''' <param name="Agenda">Tabla con la información de la agenda</param>
    ''' <param name="NumeroAtributos">Cantidad de columnas de tipo atributo previo a las celdas de tipo hora</param>
    ''' <param name="ConfiguracionColores">Objeto con la configuración de los colores por cada tipo de documento</param>
    ''' <remarks></remarks>
    Private Sub GraficarHorasAlmuerzo(ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef Dia As DayOfWeek, ByRef Agenda As System.Data.DataTable, ByVal NumeroAtributos As Integer, ByRef ConfiguracionColores As ConfiguracionColores)
        Dim FilaAgenda As DataRow
        Dim HoraInicioAlmuerzo As DateTime
        Dim HoraFinAlmuerzo As DateTime
        Dim HoraCelda As DateTime
        Dim CeldasAlmuerzo As List(Of Integer)
        Dim TextoEncabezado As String = String.Empty
        Dim TextoHora As String = String.Empty
        Dim TextoMinutos As String = String.Empty
        Dim ColorAlmuerzo As Color
        Dim TipoEmpleado As String = String.Empty
        Try
            If HorarioSucursal.Item(Dia).MinutosAlmuerzo > 0 Then
                ColorAlmuerzo = ConfiguracionColores.ObtenerColor(DMS_Addon.ConfiguracionColores.TipoColor.Almuerzo, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, False)
                HoraInicioAlmuerzo = HorarioSucursal.Item(Dia).HoraInicioAlmuerzo
                HoraFinAlmuerzo = HorarioSucursal.Item(Dia).HoraFinAlmuerzo
                CeldasAlmuerzo = New List(Of Integer)
                'Recorre todas las líneas de la agenda y bloquea las celdas de almuerzo
                For i As Integer = 0 To Agenda.Rows.Count - 1
                    FilaAgenda = Agenda.Rows.Item(i)
                    TipoEmpleado = If(IsDBNull(FilaAgenda.Item("Rol")), String.Empty, FilaAgenda.Item("Rol").ToString())
                    If TipoEmpleado = "T" Then
                        If CeldasAlmuerzo.Count = 0 Then
                            For j As Integer = NumeroAtributos To Agenda.Columns.Count - 1
                                TextoEncabezado = Agenda.Columns.Item(j).Caption
                                TextoHora = Split(TextoEncabezado, ":")(0)
                                TextoMinutos = Split(TextoEncabezado, ":")(1)
                                HoraCelda = New DateTime(HoraInicioAlmuerzo.Year, HoraInicioAlmuerzo.Month, HoraInicioAlmuerzo.Day, Integer.Parse(TextoHora), Integer.Parse(TextoMinutos), 0)

                                If HoraCelda.TimeOfDay >= HoraInicioAlmuerzo.TimeOfDay AndAlso HoraCelda.TimeOfDay < HoraFinAlmuerzo.TimeOfDay Then
                                    dgv_AgendaCitas.Rows(i).Cells(j).Style.BackColor = ColorAlmuerzo
                                    FilaAgenda.Item(j) = "n/a"
                                    CeldasAlmuerzo.Add(j)
                                End If
                                If HoraCelda.TimeOfDay > HoraFinAlmuerzo.TimeOfDay Then
                                    Exit For
                                End If
                            Next
                        Else
                            For h As Integer = 0 To CeldasAlmuerzo.Count - 1
                                dgv_AgendaCitas.Rows(i).Cells(CeldasAlmuerzo.Item(h)).Style.BackColor = ColorAlmuerzo
                                FilaAgenda.Item(CeldasAlmuerzo.Item(h)) = "n/a"
                            Next
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Cuenta la cantidad de columnas de tipo atributo que no corresponden a las horas
    ''' </summary>
    ''' <returns>Número entero con la cantidad de columnas de atributo antes de las columnas con las horas</returns>
    ''' <remarks></remarks>
    Private Function CalcularNoAtributos() As Integer
        Dim DescripcionColumna As String = String.Empty
        Try
            For i As Integer = 0 To dtAgenda.Columns.Count - 1
                DescripcionColumna = dtAgenda.Columns.Item(i).Caption
                If Not String.IsNullOrEmpty(DescripcionColumna) AndAlso DescripcionColumna.Contains(":") Then
                    Return i
                End If
            Next
            Return 0
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el horario de la sucursal a partir de las configuraciones de la sucursal
    ''' </summary>
    ''' <param name="CodigoSucursal">Código de la sucursal en formato texto</param>
    ''' <param name="HorarioSucursal">Diccionario donde se va a guardar el horario de la sucursal para su posterior uso</param>
    ''' <returns>True = Horario configurado correctamente. False = El horario no está configurado correctamente</returns>
    ''' <remarks></remarks>
    Private Function ObtenerHorarioSucursal(ByVal CodigoSucursal As String, ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario)) As Boolean
        Dim Apertura As DateTime
        Dim Cierre As DateTime
        Dim InicioAlmuerzo As DateTime
        Dim FinAlmuerzo As DateTime
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)) IsNot Nothing Then
                'Horario de semana laboral (Lunes a Viernes)
                Apertura = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraInicio
                Cierre = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFin
                InicioAlmuerzo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HorAlI
                FinAlmuerzo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraAlF
                HorarioSucursal.Add(DayOfWeek.Monday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))
                HorarioSucursal.Add(DayOfWeek.Tuesday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))
                HorarioSucursal.Add(DayOfWeek.Wednesday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))
                HorarioSucursal.Add(DayOfWeek.Thursday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))
                HorarioSucursal.Add(DayOfWeek.Friday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))

                'Horario Sábados
                Apertura = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraIS
                Cierre = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFS
                InicioAlmuerzo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HorAlI
                FinAlmuerzo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraAlF
                HorarioSucursal.Add(DayOfWeek.Saturday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))

                'Horario Domingos
                Apertura = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraID
                Cierre = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFD
                InicioAlmuerzo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HorAlI
                FinAlmuerzo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraAlF
                HorarioSucursal.Add(DayOfWeek.Sunday, New Horario(Apertura, Cierre, InicioAlmuerzo, FinAlmuerzo))
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return False
        End Try
    End Function

    Private Function ObtenerIntervalos(ByRef HorarioSucursal As Dictionary(Of DayOfWeek, Horario), ByRef FechaSeleccionada As DateTime) As List(Of DateTime)
        Dim ListaIntervalos As List(Of DateTime)
        Dim Intervalo As Integer
        Dim HoraCelda As DateTime
        Dim HoraApertura As DateTime
        Dim HoraCierre As DateTime
        Try
            Intervalo = ObtenerIntervalo()
            ListaIntervalos = New List(Of DateTime)
            HoraApertura = HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraApertura
            HoraCierre = HorarioSucursal.Item(FechaSeleccionada.DayOfWeek).HoraCierre
            HoraCelda = New DateTime(FechaSeleccionada.Year, FechaSeleccionada.Month, FechaSeleccionada.Day, HoraApertura.Hour, HoraApertura.Minute, 0)
            While HoraCelda.TimeOfDay < HoraCierre.TimeOfDay
                ListaIntervalos.Add(New DateTime(FechaSeleccionada.Year, FechaSeleccionada.Month, FechaSeleccionada.Day, HoraCelda.Hour, HoraCelda.Minute, 0))
                HoraCelda = HoraCelda.AddMinutes(Intervalo)
            End While

            Return ListaIntervalos
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Nothing
        End Try
    End Function

    Private Function ObtenerIntervalo()
        Try
            'Implementar configuraciones de intervalos aquí (Ejemplo 15, 30 o dependencias de acuerdo a la configuración de agenda)
            Return 15
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return 15
        End Try
    End Function

    ''' <summary>
    ''' Consulta todos los documentos (Citas, ordenes de trabajo, bloqueos, almuerzo,...) que se deben graficar y los guarda en un solo DataTable para facilitar su manipulación
    ''' </summary>
    ''' <param name="FechaSeleccionada">Fecha seleccionada en el formulario de la agenda</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="TipoAgenda">Tipo de agenda, si es sencilla o en equipos</param>
    ''' <returns>DataTable con todos los documentos que coinciden con la fecha y sucursal indicada</returns>
    ''' <remarks></remarks>
    Private Function ConsultarDocumentos(ByRef FechaSeleccionada As DateTime, ByVal CodigoSucursal As String, ByVal TipoAgenda As Integer) As System.Data.DataTable
        Dim Query As String = String.Empty
        Dim DataTable As System.Data.DataTable
        Try
            'Completa el query con los datos que se desean consultar
            Query = GenerarQueryDocumentos(FechaSeleccionada, CodigoSucursal, TipoAgenda)
            DataTable = DMS_Connector.Helpers.EjecutarConsultaDataTable(Query)
            Return DataTable
        Catch ex As Exception
            ManejoErroresAgenda(ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Genera un query consolidado que abarca todos los tipos de documentos que se deben graficar
    ''' </summary>
    ''' <param name="FechaSeleccionada">Fecha seleccionada</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="TipoAgenda">Tipo de agenda, sencilla o en equipos</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GenerarQueryDocumentos(ByVal FechaSeleccionada As DateTime, ByVal CodigoSucursal As String, ByVal TipoAgenda As Integer) As String
        Dim Query As String = String.Empty
        Dim QueryBloqueoAgenda = String.Empty
        Dim QuerySuspensionPorHorario As String = String.Empty
        Dim QueryServiciosReprogramados As String = String.Empty
        Dim QueryServiciosNoIniciados As String = String.Empty
        Dim QueryCitasAsesores As String = String.Empty
        Dim QueryOrdenesTrabajo As String = String.Empty
        Dim QueryCitasTecnicos As String = String.Empty
        Dim QueryBloqueoMecanicos As String = String.Empty
        Dim FechaUniversal As String = String.Empty
        Dim CodigoEstadoCitaCancelado As String = String.Empty

        Try
            FechaUniversal = FechaSeleccionada.ToString("yyyyMMdd")
            CodigoEstadoCitaCancelado = DMS_Connector.Helpers.EjecutarConsulta(String.Format("SELECT U_CodCitaCancel FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{0}'", CodigoSucursal))
            'Suspende la agenda (Solamente la de los asesores)
            QueryBloqueoAgenda = "SELECT T0.U_Cod_Agenda AS 'IdAgenda', T0.U_Cod_Agenda AS 'empID', 'A' AS 'TipoEmpleado', '3' AS 'TipoDocumento', '' AS 'NumeroCita', '' AS 'EstadoCita', '' AS 'RazonCita', '' AS 'NumeroOT', '' AS 'EstadoServicio', T0.U_Fha_Desde AS 'FechaInicio', CASE WHEN LEN(ISNULL(T0.U_Hora_Desde,'')) < 3 THEN '' ELSE T0.U_Hora_Desde END AS 'HoraInicio', T0.U_Fha_Hasta AS 'FechaFin',CASE WHEN LEN(ISNULL(T0.U_Hora_Hasta,'')) < 3 THEN '' ELSE T0.U_Hora_Hasta END AS 'HoraFin', 0 AS 'Cantidad', 0 AS 'DuracionEstandar', 0 AS 'TiempoOtorgado', 0 AS 'TiempoServicio', '' AS 'Placa' " +
                                           " FROM [@SCGD_AGENDA_SUSP] T0 WITH (nolock) " +
                                           " WHERE U_Fha_Desde = '{0}' AND U_Cod_Sucur = '{1}' AND U_Estado = 'Y' "
            QueryBloqueoAgenda = String.Format(QueryBloqueoAgenda, FechaUniversal, CodigoSucursal)

            'Suspende las líneas del mecánico (Solamente para los técnicos)
            QueryBloqueoMecanicos = " SELECT '0' AS 'IdAgenda', BM.U_IdMec AS 'empID', 'T' AS 'TipoEmpleado', '3' AS 'TipoDocumento', '' AS 'NumeroCita', '' AS 'EstadoCita', '' AS 'RazonCita', '' AS 'NumeroOT', '' AS 'EstadoServicio', LBM.U_FechCon AS 'FechaInicio', CASE WHEN LEN(ISNULL(BM.U_HorI,'')) < 3 THEN '' ELSE BM.U_HorI END AS 'HoraInicio', LBM.U_FechCon AS 'FechaFin', CASE WHEN LEN(ISNULL(BM.U_HoraF,'')) < 3 THEN '' ELSE BM.U_HoraF END AS 'HoraFin', 0 AS 'Cantidad', 0 AS 'DuracionEstandar', 0 AS 'TiempoOtorgado', 0 AS 'TiempoServicio', '' AS 'Placa' FROM [@SCGD_BLOCMEC] as BM with(nolock) " +
                                             " inner join  [dbo].[@SCGD_LINEAS_BLOME] as LBM on BM.DocEntry = LBM.DocEntry " +
                                             " where  LBM.U_FechCon = '{0}' and BM.U_IdSucu = '{1}'"
            QueryBloqueoMecanicos = String.Format(QueryBloqueoMecanicos, FechaUniversal, CodigoSucursal)

            'Citas - Ocupación de los asesores sin importar si tienen o no orden de trabajo
            QueryCitasAsesores = "SELECT T0.U_Cod_Agenda AS 'IdAgenda', T0.U_Cod_Agenda AS 'empID', 'A' AS 'TipoEmpleado', '7' AS 'TipoDocumento', CONCAT(T0.U_Num_Serie,'-', T0.U_NumCita) AS 'NumeroCita', T0.U_Estado AS 'EstadoCita', T0.U_Cod_Razon AS 'RazonCita', ISNULL(T1.U_SCGD_Numero_OT, '') AS 'NumeroOT', '' AS 'EstadoServicio', T0.U_FechaCita AS 'FechaInicio', CASE WHEN LEN(ISNULL(T0.U_HoraCita,'')) < 3 THEN '' ELSE T0.U_HoraCita END AS 'HoraInicio', T0.U_FhaCita_Fin AS 'FechaFin', CASE WHEN LEN(ISNULL(T0.U_HoraCita_Fin,'')) < 3 THEN '' ELSE T0.U_HoraCita_Fin END AS 'HoraFin', 0 AS 'Cantidad', 0 AS 'DuracionEstandar', 0 AS 'TiempoOtorgado', 0 AS 'TiempoServicio', T0.U_Num_Placa AS 'Placa' FROM [@SCGD_CITA] T0 INNER JOIN OQUT T1 ON T0.U_Num_Cot = T1.DocEntry WHERE T0.U_Cod_Sucursal = '{2}' AND ((T0.U_FechaCita <> '{0}' AND T0.U_FhaCita_Fin = '{0}') OR T0.U_FechaCita = '{0}') AND ( T0.U_Estado <> '{1}' OR T0.U_Estado IS NULL)"
            QueryCitasAsesores = String.Format(QueryCitasAsesores, FechaUniversal, CodigoEstadoCitaCancelado, CodigoSucursal)

            'Citas - Todas las citas de los técnicos excepto las que ya tienen orden de trabajo con un servicio asignado
            'es decir sin que existan líneas en Control Colaborador
            QueryCitasTecnicos = "SELECT 0 AS 'IdAgenda', CI.U_Cod_Tecnico AS 'empID', 'T' AS 'TipoEmpleado', '1' AS 'TipoDocumento', CONCAT(CI.U_Num_Serie,'-', CI.U_NumCita) AS 'NumeroCita', CI.U_Estado AS 'EstadoCita', CI.U_Cod_Razon AS 'RazonCita', ISNULL(QU.U_SCGD_Numero_OT,'') AS 'NumeroOT', '' AS 'EstadoServicio', CI.U_FhaServ AS 'FechaInicio', CASE WHEN LEN(ISNULL(CI.U_HoraServ,'')) < 3 THEN '' ELSE CI.U_HoraServ END AS 'HoraInicio', CI.U_FhaServ_Fin AS 'FechaFin', CASE WHEN LEN(ISNULL(CI.U_HoraServ_Fin, '')) < 3 THEN '' ELSE CI.U_HoraServ_Fin END AS 'HoraFin', 0 AS 'Cantidad', CASE WHEN Q1.U_SCGD_TipArt = '2' THEN SUM(Q1.Quantity * ISNULL(Q1.U_SCGD_DurSt, 0)) ELSE 0 END AS 'DuracionEstandar', CASE WHEN Q1.U_SCGD_TipArt = '2' THEN SUM(Q1.Quantity * ISNULL(Q1.U_SCGD_TiOtor, 0)) ELSE 0 END AS 'TiempoOtorgado', ISNULL(T4.U_SCGD_TiempServ, 0) AS 'TiempoServicio', CI.U_Num_Placa AS 'Placa' " +
            "FROM [@SCGD_CITA] CI with(nolock) " +
            "INNER JOIN OQUT QU with(nolock) ON QU.DocEntry = CI.U_Num_Cot " +
            "INNER JOIN QUT1 Q1 with (nolock) ON QU.DocEntry = Q1.DocEntry " +
            "INNER JOIN OHEM T4 with (nolock) ON T4.empID = CI.U_Cod_Tecnico " +
            "LEFT JOIN [@SCGD_CTRLCOL] T5 ON QU.U_SCGD_Numero_OT = T5.Code AND T5.LineId = 1 " +
            "WHERE CI.U_Cod_Sucursal = '{2}' AND (CI.U_FhaServ = '{0}' OR (CI.U_FhaServ < '{0}' AND CI.U_FhaServ_Fin >= '{0}')) AND CI.U_Estado <> '{1}' AND T5.Code IS NULL " +
            "GROUP BY CI.U_Cod_Tecnico, CI.U_Num_Serie, CI.U_NumCita, CI.U_Estado, CI.U_Cod_Razon, CI.U_FhaServ, CI.U_HoraServ, CI.U_FhaServ_Fin, CI.U_HoraServ_Fin, CI.U_Num_Placa, Q1.U_SCGD_TipArt, T4.U_SCGD_TiempServ, QU.U_SCGD_Numero_OT "
            QueryCitasTecnicos = String.Format(QueryCitasTecnicos, FechaUniversal, CodigoEstadoCitaCancelado, CodigoSucursal)

            'Query ordenes de trabajo (Servicios no iniciados, reprogramados, en proceso)
            QueryOrdenesTrabajo = "SELECT '0' AS 'IdAgenda', T0.empID AS 'empID', T0.U_SCGD_TipoEmp AS 'TipoEmpleado', '2' AS 'TipoDocumento', CASE WHEN T2.U_SCGD_NoCita IS NULL OR T2.U_SCGD_NoSerieCita IS NULL THEN '' ELSE CONCAT(T2.U_SCGD_NoSerieCita, '-', T2.U_SCGD_NoCita) END AS 'NumeroCita', ISNULL(T4.U_Estado, '') AS 'EstadoCita', ISNULL(T4.U_Cod_Razon, '') AS 'RazonCita', T1.Code AS 'NumeroOT', T1.U_Estad AS 'EstadoServicio', CASE WHEN T1.U_Estad = '2' THEN T1.U_DFIni ELSE T1.U_FechPro END AS 'FechaInicio', CASE WHEN T1.U_Estad = '2' THEN T1.U_HFIni ELSE CASE WHEN LEN(ISNULL(T1.U_HoraIni,'')) < 3 THEN '' ELSE REPLACE(T1.U_HoraIni, ':','') END END  AS 'HoraInicio', '' AS 'FechaFin', '' AS 'HoraFin', 0 AS 'Cantidad', SUM(T3.Quantity * ISNULL(T3.U_SCGD_DurSt, 0)) AS 'DuracionEstandar', SUM(T3.Quantity * ISNULL(T3.U_SCGD_TiOtor, 0)) AS 'TiempoOtorgado', ISNULL(T0.U_SCGD_TiempServ, 0) AS 'TiempoServicio', T2.U_SCGD_Num_Placa AS 'Placa' " +
                                  "FROM OHEM T0 " +
                                  "INNER JOIN [@SCGD_CTRLCOL] T1 ON T0.empID = T1.U_Colab " +
                                  "INNER JOIN OQUT T2 ON T1.Code = T2.U_SCGD_Numero_OT " +
                                  "INNER JOIN QUT1 T3 ON T2.DocEntry = T3.DocEntry AND T3.U_SCGD_ID = T1.U_IdAct AND T3.U_SCGD_TipArt = '2' " +
                                  "LEFT JOIN [@SCGD_CITA] T4 ON T4.U_Num_Cot = T2.DocEntry " +
                                  "WHERE T1.U_Estad NOT IN('3', '4') " +
                                  "AND ((T1.U_Estad = '1' AND U_FechPro = '{0}') OR (T1.U_Estad = '2' AND T1.U_DFIni = '{0}')) AND T2.U_SCGD_Estado_CotID IN('1', '2', '3') "
            If DMS_Connector.Company.AdminInfo.EnableBranches = BoYesNoEnum.tNO Then
                QueryOrdenesTrabajo += String.Format(" AND T0.branch = '{0}' ", CodigoSucursal)
            Else
                QueryOrdenesTrabajo += String.Format(" AND T0.BPLId = '{0}' ", CodigoSucursal)
            End If

            QueryOrdenesTrabajo += " GROUP BY T0.empID, T0.U_SCGD_TipoEmp, T1.Code, T1.U_Estad, T1.U_FechPro, T1.U_HoraIni, T0.U_SCGD_TiempServ, T2.U_SCGD_Num_Placa, T1.U_DFIni, T1.U_HFIni, T2.U_SCGD_NoSerieCita, T2.U_SCGD_NoCita, T4.U_Estado, T4.U_Cod_Razon "
            QueryOrdenesTrabajo = String.Format(QueryOrdenesTrabajo, FechaUniversal)

            'Ordenes de trabajo con servicios suspendidos por horario
            QuerySuspensionPorHorario = "SELECT '0' AS 'IdAgenda', T4.empID AS 'empID', T4.U_SCGD_TipoEmp AS 'TipoEmpleado', '4' AS 'TipoDocumento', '' AS 'NumeroCita', '' AS 'EstadoCita', '' AS 'RazonCita', T3.U_SCGD_Numero_OT AS 'NumeroOT', T0.U_Estad AS 'EstadoServicio', T0.U_DFIni AS 'FechaInicio', T0.U_HFIni AS 'HoraInicio', '' AS 'FechaFin', '' AS 'HoraFin', 0 AS 'Cantidad', CASE WHEN T2.U_SCGD_TipArt = '2' THEN SUM(T2.Quantity * ISNULL(T2.U_SCGD_DurSt, 0)) ELSE 0 END AS 'DuracionEstandar', CASE WHEN T2.U_SCGD_TipArt = '2' THEN SUM(T2.Quantity * ISNULL(T2.U_SCGD_TiOtor, 0)) ELSE 0 END 'TiempoOtorgado', ISNULL(T4.U_SCGD_TiempServ, 0) AS 'TiempoServicio', T3.U_SCGD_Num_Placa AS 'Placa' FROM [@SCGD_CTRLCOL] T0 WITH (NOLOCK) INNER JOIN QUT1 T2 WITH (NOLOCK) ON T0.U_IdAct = T2.U_SCGD_ID INNER JOIN OQUT T3 WITH (NOLOCK) ON T2.DocEntry = T3.DocEntry LEFT JOIN OHEM T4 WITH(nolock) ON T0.U_Colab = T4.empID WHERE T0.U_DFIni = '{0}' AND T3.U_SCGD_Estado_CotID IN ('{1}', '{2}') AND T0.U_Estad = '{3}' AND T0.U_SuspensionHorario = 'Y' AND T0.LineId IN (SELECT Max(S1.LineId) FROM [@SCGD_CTRLCOL] S1 with(nolock) WHERE S1.U_IdAct = T0.U_IdAct AND S1.U_Colab = T0.U_Colab) AND T3.DocStatus = 'O' AND T2.U_SCGD_NombEmpleado IS NOT NULL AND T0.U_IdAct NOT IN (SELECT Distinct (S2.U_IdAct) FROM [@SCGD_CTRLCOL] S2 WHERE S2.U_Estad IN ('1','2','4') AND S2.U_Colab = T0.U_Colab) AND T3.U_SCGD_idSucursal = '{4}' GROUP BY T4.empID, T4.U_SCGD_TipoEmp, T3.U_SCGD_Numero_OT, T0.U_Estad, T0.U_DFIni, T0.U_HFIni, T2.U_SCGD_TipArt, T4.U_SCGD_TiempServ, T3.U_SCGD_Num_Placa"
            QuerySuspensionPorHorario = String.Format(QuerySuspensionPorHorario, FechaUniversal, Integer.Parse(EstadoServicio.Iniciado), Integer.Parse(EstadoServicio.Suspendido), Integer.Parse(EstadoServicio.Suspendido), CodigoSucursal)

            If TipoAgenda = TipoDeAgenda.Agenda Then
                Query = String.Format("{0} UNION ALL {1} ", QueryBloqueoAgenda, QueryCitasAsesores)
            Else
                Query = String.Format("{0} UNION ALL {1} UNION ALL {2} UNION ALL {3} UNION ALL {4} UNION ALL {5} ", QueryBloqueoAgenda, QueryBloqueoMecanicos, QuerySuspensionPorHorario, QueryCitasAsesores, QueryCitasTecnicos, QueryOrdenesTrabajo)
            End If

            Return Query
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Sub LlenarOcupacionPorAgenda(ByRef p_dtAgenda As System.Data.DataTable)
        Try

            Dim strIdAgenda As String
            Dim strIntervalo As String
            Dim strUsaTiempoServ As String
            Dim intDuracion As Integer
            Dim intIntervaloCita As Integer

            Dim dtCitas As System.Data.DataTable


            Dim strMinutos As String
            Dim strHora As String
            Dim intMinutos As Integer
            Dim strHoraCita As String
            Dim intCantMinutos As Integer
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim strCita As String
            Dim strDuracionServ As String
            Dim strEstadoCancelado As String
            Dim strfecha As String
            Dim intCont As Integer = 0
            Dim intPosition As Integer = 0
            Dim strPlaca As String = String.Empty
            Dim strDescripcionCelda As String = String.Empty
            Dim CodigoAgendaCita As String = String.Empty

            m_dtFecha = dtpFecha.Value
            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)
            strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] with (nolock) " &
                                                                            " where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            dtCitas = Utilitarios.EjecutarConsultaDataTable(String.Format("Select CI.DocEntry, CI.U_HoraCita, CI.U_NumCita, CI.U_Num_Serie, CI.U_Cod_Unid, CI.U_Num_Placa, CI.U_CardCode, CI.U_CardName , ISNULL(CO.U_Color,'DarkSeaGreen') U_Color, CASE WHEN T0.U_SCGD_Numero_OT IS NOT NULL THEN ISNULL(CE.U_ColorOT,'DarkSeaGreen') ELSE ISNULL(CE.U_Color,'DarkSeaGreen') END ColorEstado, CI.U_Cod_Agenda From [@SCGD_CITA] CI with (nolock) FULL OUTER JOIN [@SCGD_COLORESAGENDA] CO ON CO.U_RazonCita = CI.U_Cod_Razon LEFT JOIN [@SCGD_CITA_ESTADOS] CE ON CI.U_Estado = CE.Code LEFT JOIN OQUT T0 ON T0.DocEntry = CI.U_Num_Cot where CI.U_FechaCita = '{0}' and ( CI.U_Estado <> '{1}' or CI.U_Estado is null)", strfecha, strEstadoCancelado), m_oCompany.CompanyDB, m_oCompany.Server)

            If dtCitas.Rows.Count > 0 Then
                For Each row As DataRow In dtAgenda.Rows

                    intPosition = dtAgenda.Rows.IndexOf(row)

                    strIdAgenda = row.Item(mc_strIDAgenda)


                    strIntervalo = ObtenerIntervaloAgenda(strIdAgenda)

                    If String.IsNullOrEmpty(strIntervalo) Then
                        intIntervaloCita = 15
                    Else
                        intIntervaloCita = Integer.Parse(strIntervalo)
                    End If

                    strUsaTiempoServ = Utilitarios.EjecutarConsulta(String.Format("Select U_TmpServ from [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}'", strIdAgenda), m_oCompany.CompanyDB, m_oCompany.Server)

                    For Each rowCitas As DataRow In dtCitas.Rows
                        CodigoAgendaCita = rowCitas.Item("U_Cod_Agenda")
                        If strIdAgenda = CodigoAgendaCita Then
                            If Not IsDBNull(rowCitas.Item("U_HoraCita")) And
                            Not IsDBNull(rowCitas.Item("U_Num_Serie")) And
                            Not IsDBNull(rowCitas.Item("U_NumCita")) Then

                                strHoraCita = rowCitas.Item("U_HoraCita")
                                strSerieCita = rowCitas.Item("U_Num_Serie")
                                strNumCita = rowCitas.Item("U_NumCita")
                                If Not IsDBNull(rowCitas.Item("U_Num_Placa")) Then
                                    strPlaca = rowCitas.Item("U_Num_Placa")
                                Else
                                    strPlaca = String.Empty
                                End If
                                intCantMinutos = (strHoraCita.Length - 1) - 1
                                strMinutos = strHoraCita.Substring(intCantMinutos, 2)
                                strHora = strHoraCita.Substring(0, intCantMinutos)
                                intMinutos = Convert.ToInt32(strMinutos)

                                strCita = String.Format("{0}-{1}", strSerieCita, strNumCita)

                                If Not String.IsNullOrEmpty(strPlaca) Then
                                    strDescripcionCelda = String.Format("{0}/{1}", strPlaca, strCita)
                                Else
                                    strDescripcionCelda = strCita
                                End If


                                strDuracionServ = ObtenerDuracionCita(strSerieCita, strNumCita, m_strCodSucursal, "")

                                If strUsaTiempoServ.Equals("Y") Then
                                    intDuracion = Convert.ToInt32(strDuracionServ)
                                Else
                                    intDuracion = intIntervaloCita
                                End If

                                If intDuracion = 0 OrElse intDuracion < intIntervaloCita Then
                                    intDuracion = intIntervaloCita
                                End If

                                For Each element As String In listColumsGrid

                                    Dim result As String() = element.Split(New Char() {":"c})
                                    Dim horAgenda As String = result(0)
                                    Dim minAgenda As String = result(1)
                                    Dim strIntervaloAct As String = ""

                                    Dim strDocEntry As String = ""

                                    If strHora.Equals(horAgenda) And strMinutos.Equals(minAgenda) Then

                                        For intI As Integer = intDuracion To 1 Step -15
                                            If intCont <= listColumsGrid.Count - 1 Then

                                                Dim valorColumna As String() = listColumsGrid.ToArray()
                                                strIntervaloAct = row.Item(valorColumna(intCont))

                                                If Not String.IsNullOrEmpty(strIntervaloAct) Then

                                                    Dim strTest As String() = strIntervaloAct.Split(New Char() {"-"c})
                                                    If strTest(0).Trim() <> "n/a" Then

                                                        strDocEntry = Utilitarios.EjecutarConsulta(String.Format("select DocEntry from [@SCGD_CITA] where U_NumCita = '{0}' and U_Num_Serie = '{1}'", strTest(1).Trim(), strTest(0).Trim()), m_oCompany.CompanyDB, m_oCompany.Server)
                                                        Dim strDocEntryAct As String = rowCitas.Item("DocEntry")

                                                        If Not tableCitasProbl.ContainsKey(strDocEntry) And Not tableCitasProbl.ContainsKey(strDocEntryAct) Then
                                                            tableCitasProbl.Add(strDocEntryAct, strCita)
                                                        End If
                                                        Exit For
                                                    End If

                                                End If
                                                If row.Item(valorColumna(intCont)) <> "n/a" Then
                                                    row.Item(valorColumna(intCont)) = strDescripcionCelda
                                                    If strUsaColorAgenda = "Y" Then
                                                        If oGestionColor = GestionColor.EstadoCita Then
                                                            dgv_AgendaCitas.Rows(intPosition).Cells(valorColumna(intCont)).Style.BackColor = Color.FromName(rowCitas.Item("ColorEstado"))
                                                        Else
                                                            dgv_AgendaCitas.Rows(intPosition).Cells(valorColumna(intCont)).Style.BackColor = Color.FromName(rowCitas.Item("U_Color"))
                                                        End If
                                                    Else
                                                        dgv_AgendaCitas.Rows(intPosition).Cells(valorColumna(intCont)).Style.BackColor = Color.DarkSeaGreen
                                                    End If

                                                    intCont = intCont + 1
                                                Else
                                                    intI = intI + 15
                                                    intCont += 1
                                                End If
                                            End If
                                        Next
                                    End If
                                    intCont += 1
                                Next
                            End If
                            intCont = 0
                        End If
                    Next
                Next
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try

    End Sub

    Public Sub LlenarOcupacionPorGruposAsesor(ByRef p_dtAgendas As System.Data.DataTable)
        Try

            Dim strIdAgenda As String
            Dim strIntervalo As String
            Dim strUsaTiempoServ As String
            Dim intDuracion As Integer
            Dim intIntervaloCita As Integer

            Dim dtCitas As System.Data.DataTable

            Dim strMinutos As String
            Dim strHora As String
            Dim intMinutos As Integer
            Dim strHoraCita As String
            Dim intCantMinutos As Integer
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim strCita As String
            Dim strEstadoCancelado As String
            Dim strfecha As String
            Dim intCont As Integer = 0
            Dim strname As String = String.Empty
            Dim ColorDefualt As Color = Color.DarkSeaGreen
            Dim strPlaca As String = String.Empty
            Dim strDescripcionCelda As String = String.Empty
            Dim CodigoAgendaCita As String = String.Empty

            m_dtFecha = dtpFecha.Value
            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)
            strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)
            m_strTipoAgendaColor = Utilitarios.EjecutarConsulta(String.Format("select ISNULL(U_AgendaColor,'N') as AgendaColor  from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            If m_strTipoAgendaColor.ToUpper = "Y" Then
                dtCitas = Utilitarios.EjecutarConsultaDataTable(String.Format("Select CI.DocEntry, CI.U_HoraCita, CI.U_NumCita, CI.U_Num_Serie, CI.U_Cod_Unid, CI.U_Num_Placa, CI.U_CardCode, CI.U_CardName , ISNULL(CO.U_Color,'DarkSeaGreen') U_Color, CASE WHEN T0.U_SCGD_Numero_OT IS NOT NULL THEN ISNULL(CE.U_ColorOT,'DarkSeaGreen') ELSE ISNULL(CE.U_Color,'DarkSeaGreen') END ColorEstado, CI.U_Cod_Agenda from [@SCGD_CITA] CI LEFT JOIN [@SCGD_COLORESAGENDA] CO ON CI.U_Cod_Razon = CO.U_RazonCita  LEFT JOIN [@SCGD_CITA_ESTADOS] CE ON CI.U_Estado = CE.Code LEFT JOIN OQUT T0 ON T0.DocEntry = CI.U_Num_Cot where CI.U_FechaCita = '{0}' and ( CI.U_Estado <> '{1}' or CI.U_Estado is null)", strfecha, strEstadoCancelado))
            Else
                dtCitas = Utilitarios.EjecutarConsultaDataTable(String.Format("Select DocEntry, U_HoraCita, U_NumCita, U_Num_Serie, U_Cod_Unid, U_Num_Placa, U_CardCode, U_CardName, U_Cod_Agenda  from [@SCGD_CITA] where U_FechaCita = '{0}' and ( U_Estado <> '{1}' or U_Estado is null)", strfecha, strEstadoCancelado))
            End If

            For Each row As DataRow In p_dtAgendas.Rows
                If Not IsDBNull(row.Item(mc_strIDAgenda)) AndAlso CInt(row.Item(mc_strIDAgenda)) <> 0 Then

                    Dim intPosition As Integer = p_dtAgendas.Rows.IndexOf(row)
                    strname = p_dtAgendas.Columns(intPosition).ColumnName


                    For Each Column As System.Data.DataColumn In p_dtAgendas.Columns

                        Dim strNameColumn As String = Column.ColumnName

                        If p_dtAgendas.Rows(intPosition)(strNameColumn).ToString() = "n/a" Then
                            dgv_AgendaCitas.Rows(intPosition).Cells(strNameColumn).Style.BackColor = Color.DarkGray
                        End If
                    Next


                    dgv_AgendaCitas.Rows(intPosition).DefaultCellStyle.BackColor = Color.Gainsboro
                    strIdAgenda = row.Item(mc_strIDAgenda)



                    'strUsaTiempoServ = Utilitarios.EjecutarConsulta(String.Format("Select U_TmpServ from [@SCGD_AGENDA] where DocEntry = '{0}'", strIdAgenda), m_oCompany.CompanyDB, m_oCompany.Server)



                    For Each rowCitas As DataRow In dtCitas.Rows
                        CodigoAgendaCita = rowCitas.Item("U_Cod_Agenda")
                        If strIdAgenda = CodigoAgendaCita Then
                            If Not IsDBNull(rowCitas.Item("U_HoraCita")) And
                           Not IsDBNull(rowCitas.Item("U_Num_Serie")) And
                           Not IsDBNull(rowCitas.Item("U_NumCita")) Then

                                strHoraCita = rowCitas.Item("U_HoraCita")
                                strSerieCita = rowCitas.Item("U_Num_Serie")
                                strNumCita = rowCitas.Item("U_NumCita")

                                If Not IsDBNull(rowCitas.Item("U_Num_Placa")) Then
                                    strPlaca = rowCitas.Item("U_Num_Placa")
                                Else
                                    strPlaca = String.Empty
                                End If

                                strIntervalo = ObtenerIntervaloAgenda(strIdAgenda)
                                If String.IsNullOrEmpty(strIntervalo) Then
                                    intIntervaloCita = 15
                                Else
                                    intIntervaloCita = Convert.ToInt32(strIntervalo)
                                End If

                                intCantMinutos = (strHoraCita.Length - 1) - 1
                                strMinutos = strHoraCita.Substring(intCantMinutos, 2)
                                strHora = strHoraCita.Substring(0, intCantMinutos)
                                intMinutos = Convert.ToInt32(strMinutos)

                                strCita = String.Format("{0}-{1}", strSerieCita, strNumCita)

                                If Not String.IsNullOrEmpty(strPlaca) Then
                                    strDescripcionCelda = String.Format("{0}/{1}", strPlaca, strCita)
                                Else
                                    strDescripcionCelda = strCita
                                End If

                                intDuracion = intIntervaloCita

                                For Each element As String In listColumsGrid

                                    Dim result As String() = element.Split(New Char() {":"c})
                                    Dim horAgenda As String = result(0)
                                    Dim minAgenda As String = result(1)
                                    Dim strIntervaloAct As String = ""

                                    Dim strDocEntry As String = ""

                                    If strHora.Equals(horAgenda) And strMinutos.Equals(minAgenda) Then

                                        For intI As Integer = intDuracion To 1 Step -15
                                            If intCont <= listColumsGrid.Count - 1 Then


                                                Dim valorColumna As String() = listColumsGrid.ToArray()
                                                strIntervaloAct = row.Item(valorColumna(intCont))

                                                If Not String.IsNullOrEmpty(strIntervaloAct) AndAlso Not strIntervaloAct.Equals(".") Then

                                                    Dim strTest As String() = strIntervaloAct.Split(New Char() {"-"c})
                                                    If strTest(0).Trim() <> "n/a" Then

                                                        strDocEntry = Utilitarios.EjecutarConsulta(String.Format("select DocEntry from [@SCGD_CITA] where U_NumCita = '{0}' and U_Num_Serie = '{1}'", strTest(1).Trim(), strTest(0).Trim()), m_oCompany.CompanyDB, m_oCompany.Server)
                                                        Dim strDocEntryAct As String = rowCitas.Item("DocEntry")

                                                        If Not tableCitasProbl.ContainsKey(strDocEntry) And Not tableCitasProbl.ContainsKey(strDocEntryAct) Then
                                                            tableCitasProbl.Add(strDocEntryAct, strCita)
                                                        End If
                                                        Exit For
                                                    End If

                                                End If
                                                If row.Item(valorColumna(intCont)) <> "n/a" Then
                                                    row.Item(valorColumna(intCont)) = strDescripcionCelda
                                                    Dim strvalorColum As String = valorColumna(intCont)
                                                    If m_strTipoAgendaColor.ToUpper = "Y" Then
                                                        If oGestionColor = GestionColor.EstadoCita Then
                                                            dgv_AgendaCitas.Rows(intPosition).Cells(strvalorColum).Style.BackColor = Color.FromName(rowCitas.Item("ColorEstado").ToString)
                                                        Else
                                                            dgv_AgendaCitas.Rows(intPosition).Cells(strvalorColum).Style.BackColor = Color.FromName(rowCitas.Item("U_Color").ToString)
                                                        End If
                                                    Else
                                                        dgv_AgendaCitas.Rows(intPosition).Cells(strvalorColum).Style.BackColor = Color.DarkSeaGreen
                                                    End If

                                                    intCont = intCont + 1
                                                Else
                                                    intI = intI + 15
                                                    intCont = intCont + 1
                                                End If
                                            End If
                                        Next
                                    End If
                                    intCont = intCont + 1

                                Next
                            End If
                            intCont = 0
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    ''' <summary>
    ''' Bloquea en la agenda, los espacios correspondientes a suspensión por horario.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LlenarOcupacionSuspensionHorario()

        Dim strIdEmp As String
        Dim intDuracionTotal As Integer = 15
        Dim intIntervUnitario As Integer = 15
        Dim dtOrdenesSuspendidas As System.Data.DataTable
        Dim strBMensaje As StringBuilder = New StringBuilder()
        Dim strMinutos As String
        Dim strHora As String
        Dim strHoraCita As String
        Dim strSerieCita As String
        Dim strCodTecnico As String
        Dim strNumCita As String
        Dim strValor As String
        Dim strfecha As String
        Dim intCont As Integer = 0
        Dim strHoraOrden As String
        Dim strNoOrden As String
        Dim MiColor As Color
        Dim strIntervaloAct As String = ""
        Dim strSQL As String
        Dim strSQL_Int As String
        Dim strBDTaller As String
        Dim strQuery As String = String.Empty
        Dim strCodSuspensionHorario As String = "8"
        Dim strIniciada As String = "2"
        Dim strSuspendida As String = "3"
        Dim strTextoBloqueo As String = "n/a"
        Dim dtmHoraInicioAgenda As Date
        Dim dtmHoraFinalAgenda As Date

        Try
            'Hora de inicio y fin del calendario
            dtmHoraInicioAgenda = FormatDateTime(m_fhaHoraInicio, DateFormat.ShortTime)
            dtmHoraFinalAgenda = FormatDateTime(m_fhaHoraFin, DateFormat.ShortTime)

            'Verifica que se utilice taller adentro
            If m_strUsarTallerSAP.Equals("Y") Then

                m_dtFecha = dtpFecha.Value
                strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

                'Obtiene un datatable con la información de todas las ordenes suspendidas por horario en la fecha indicada
                strQuery = "SELECT DISTINCT T3.U_SCGD_Numero_OT, T0.U_Estad AS 'U_Estad', T0.U_Colab, T0.U_FIni, T0.U_DFIni, T0.U_HFIni, T2.U_SCGD_DurSt, T2.U_SCGD_TiOtor, T4.U_SCGD_TiempServ, T2.Quantity FROM [@SCGD_CTRLCOL] T0 WITH (NOLOCK) INNER JOIN QUT1 T2 WITH (NOLOCK) ON T0.U_IdAct = T2.U_SCGD_ID INNER JOIN OQUT T3 WITH (NOLOCK) ON T2.DocEntry = T3.DocEntry LEFT JOIN OHEM T4 WITH(nolock) ON T0.U_Colab = T4.empID WHERE T0.U_DFIni = '{0}' AND T3.U_SCGD_Estado_CotID IN ('{1}', '{2}') AND T0.U_Estad = '{3}' AND T0.U_SuspensionHorario = 'Y' AND T0.LineId IN (SELECT Max(S1.LineId) FROM [@SCGD_CTRLCOL] S1 with(nolock) WHERE S1.U_IdAct = T0.U_IdAct AND S1.U_Colab = T0.U_Colab) AND T3.DocStatus = 'O' AND T2.U_SCGD_NombEmpleado IS NOT NULL AND T0.U_IdAct NOT IN (SELECT Distinct (S2.U_IdAct) FROM [@SCGD_CTRLCOL] S2 WHERE S2.U_Estad IN ('1','2','4') AND S2.U_Colab = T0.U_Colab) AND T3.U_SCGD_idSucursal = '{4}'"
                strQuery = String.Format(strQuery, strfecha, strIniciada, strSuspendida, strSuspendida, m_strCodSucursal)
                dtOrdenesSuspendidas = Utilitarios.EjecutarConsultaDataTable(strQuery)

                'Si hay ordenes suspendidas por horario se procede a bloquear el tiempo en la agenda
                If dtOrdenesSuspendidas.Rows.Count > 0 Then

                    'Recorre todas las líneas de la agenda
                    For Each row As DataRow In dtAgenda.Rows

                        Dim strIDAgenda As String = String.Empty

                        'El ID de la agenda debe ser 0 para identificarlo como mecanico
                        If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                            strIDAgenda = row.Item(mc_strIDAgenda)
                        End If

                        'El ID del mecánico no puede estar en blanco y el ID de la agenda debe ser "0" para identificar que es un mecánico
                        'y no el líder el grupo
                        If Not IsDBNull(row.Item(mc_strID)) And strIDAgenda = "0" Then

                            'Posicion actual del recorrido
                            Dim intposition As Integer = dtAgenda.Rows.IndexOf(row)
                            strIdEmp = row.Item(mc_strID)

                            'Obtiene el ID del mecanico de la linea actual
                            Dim strMecanicoAgenda As String = String.Empty
                            strMecanicoAgenda = dtAgenda.Rows(intposition)("ID").ToString()

                            'Recorre todas las lineas del datatable con las ordenes suspendidas por horario y las asigna a la agenda como tiempo bloqueado
                            For Each rowBloquear As DataRow In dtOrdenesSuspendidas.Rows
                                Dim intPosicionBloqueo As Integer = dtOrdenesSuspendidas.Rows.IndexOf(rowBloquear)
                                Dim strMecanicoDataTable As String = String.Empty
                                Dim strNumOT As String = String.Empty
                                strNumOT = dtOrdenesSuspendidas.Rows(intPosicionBloqueo)("U_SCGD_Numero_OT").ToString()
                                strMecanicoDataTable = dtOrdenesSuspendidas.Rows(intPosicionBloqueo)("U_Colab").ToString()

                                If strMecanicoAgenda = strMecanicoDataTable Then

                                    'El campo fecha no puede estar en blanco, de lo contrario no se sabe a que día se debe asignar el bloqueo
                                    If Not IsDBNull(rowBloquear.Item("U_DFIni")) Then
                                        'Hora inicio y fin en formato texto
                                        Dim strHoraInicio As String = Utilitarios.FormatoHora(rowBloquear.Item("U_HFIni"))
                                        'Dim intDuracionEstandar As Integer = 0

                                        ''Obtiene la duración estándar de la actividad desde la oferta de ventas
                                        'If Not Integer.TryParse(rowBloquear.Item("U_SCGD_DurSt"), intDuracionEstandar) Then
                                        '    intDuracionEstandar = 15
                                        'End If

                                        intDuracionTotal = CalcularDuracionActividad(rowBloquear.Item("U_SCGD_DurSt").ToString(), rowBloquear.Item("U_SCGD_TiOtor").ToString(), rowBloquear.Item("U_SCGD_TiempServ").ToString(), rowBloquear.Item("Quantity").ToString())

                                        'Redondea los minutos a múltiplos de 15 para que coincidan con las columnas de la agenda
                                        strHoraInicio = RegresaHora(strHoraInicio.Replace(":", ""))

                                        If strHoraInicio.Length = 3 Then
                                            strHoraInicio = strHoraInicio.Insert(1, ":")
                                        ElseIf strHoraInicio.Length = 4 Then
                                            strHoraInicio = strHoraInicio.Insert(2, ":")
                                        End If

                                        'Hora inicio y fin en formato Date
                                        Dim dtmHoraInicio As Date = FormatDateTime(strHoraInicio, DateFormat.ShortTime)
                                        Dim dtmHoraFinal As Date = dtmHoraInicio.AddMinutes(intDuracionTotal)
                                        Dim dtmFechaBloqueo As Date = DateTime.Parse(rowBloquear.Item("U_DFIni"))

                                        If UsaVariosDias(dtmHoraInicio, intDuracionTotal) Then
                                            dtmHoraFinal = dtmHoraFinalAgenda
                                        Else
                                            'Verificamos que la suspension no coincida con el horario de almuerzo, en caso de ser así
                                            'se debe sumar el tiempo de almuerzo a la hora final de la actividad
                                            Dim dtmInicioAlmuerzo As Date
                                            Dim dtmFinAlmuerzo As Date
                                            Dim intDuracionAlmuerzo As Integer = 0

                                            ObtenerHorarioAlmuerzo(dtmInicioAlmuerzo, dtmFinAlmuerzo, intDuracionAlmuerzo)

                                            If dtmHoraInicio <= dtmInicioAlmuerzo And dtmHoraFinal >= dtmInicioAlmuerzo Then
                                                dtmHoraFinal = dtmHoraFinal.AddMinutes(intDuracionAlmuerzo)
                                                'Redondeamos nuevamente la hora final, en caso de que la hora de almuerzo no sea exacta y haya desajustado la hora
                                                Dim strHoraFinalRedondeada As String = RegresaHora(dtmHoraFinal.Hour.ToString() + dtmHoraFinal.Minute.ToString()).Insert(2, ":")
                                                dtmHoraFinal = FormatDateTime(strHoraFinalRedondeada, DateFormat.ShortTime)
                                            End If

                                            'Si la actividad suspendida se excede de la hora final de la agenda, se asigna como hora final la de la agenda
                                            If dtmHoraFinalAgenda < dtmHoraFinal Then
                                                dtmHoraFinal = dtmHoraFinalAgenda
                                            End If
                                        End If

                                        Dim intContador As Integer = 0

                                        'Recorre todas las columnas de la agenda
                                        For Each columna As String In listColumsGrid

                                            If columna.Equals(strHoraInicio.TrimStart("0″")) Then
                                                'Asigna el valor de la hora de inicio a una variable que va a ir aumentando su valor hasta llegar a la hora final
                                                Dim dtmHoraActividad As Date = dtmHoraInicio

                                                'Recorre todas las horas y les asigna el valor bloqueado hasta llegar a la hora final de la actividad
                                                While dtmHoraActividad < dtmHoraFinal
                                                    'Obtiene un arreglo de las columnas de la agenda en formato texto
                                                    Dim arregloColumnas As String() = listColumsGrid.ToArray()

                                                    If intContador < arregloColumnas.Length Then

                                                        'Verificamos que la celda no tenga asignada ninguna actividad ni tampoco este bloqueada
                                                        Dim strValorCelda = row.Item(arregloColumnas(intContador))

                                                        If String.IsNullOrEmpty(strValorCelda) Then
                                                            dgv_AgendaCitas.Rows(intposition).Cells(arregloColumnas(intContador)).Style.BackColor = Color.DarkGray
                                                            row.Item(arregloColumnas(intContador)) = strNumOT
                                                        Else
                                                            Exit For
                                                        End If

                                                        intContador += 1

                                                    End If

                                                    'Se agregan 15 minutos, para rellenar la siguiente posición de la agenda
                                                    dtmHoraActividad = dtmHoraActividad.AddMinutes(15)

                                                End While
                                            Else
                                                'En caso de que no se llegue a la columna, se va aumentando el contador
                                                intContador += 1
                                            End If

                                        Next

                                    End If

                                End If


                            Next
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub


    ''' <summary>
    ''' Bloquea en la agenda, los espacios correspondientes a órdenes de trabajo reprogramadas.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LlenarOcupacionReprogramadas()

        Dim strIdEmp As String
        Dim intDuracionTotal As Integer = 15
        Dim intIntervUnitario As Integer = 15
        Dim dtOrdenesReprogramadas As System.Data.DataTable
        Dim strBMensaje As StringBuilder = New StringBuilder()
        Dim strMinutos As String
        Dim strHora As String
        Dim strHoraCita As String
        Dim strSerieCita As String
        Dim strCodTecnico As String
        Dim strNumCita As String
        Dim strValor As String
        Dim strfecha As String
        Dim intCont As Integer = 0
        Dim strHoraOrden As String
        Dim strNoOrden As String
        Dim MiColor As Color
        Dim strIntervaloAct As String = ""
        Dim strSQL As String
        Dim strSQL_Int As String
        Dim strBDTaller As String
        Dim strQuery As String = String.Empty
        Dim strCodSuspensionHorario As String = "8"
        Dim strNoIniciada As String = "1"
        Dim strIniciada As String = "2"
        Dim strSuspendida As String = "3"
        Dim strTextoBloqueo As String = "n/a"
        Dim dtmHoraInicioAgenda As Date
        Dim dtmHoraFinalAgenda As Date

        Try
            'Hora de inicio y fin del calendario
            dtmHoraInicioAgenda = FormatDateTime(m_fhaHoraInicio, DateFormat.ShortTime)
            dtmHoraFinalAgenda = FormatDateTime(m_fhaHoraFin, DateFormat.ShortTime)

            'Verifica que se utilice taller adentro
            If m_strUsarTallerSAP.Equals("Y") Then

                m_dtFecha = dtpFecha.Value
                strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

                'Obtiene un datatable con la información de todas las ordenes reprogramadas por horario en la fecha indicada
                strQuery = " SELECT Distinct oq.U_SCGD_Numero_OT, T0.U_Estad, T0.U_Colab, T0.U_FechPro, T0.U_DFIni, T0.U_HoraIni, qu.U_SCGD_DurSt, qu.U_SCGD_TiOtor, oh.U_SCGD_TiempServ, qu.Quantity FROM [@SCGD_CTRLCOL] T0 INNER JOIN QUT1 as qu on T0.U_IdAct  = qu.U_SCGD_ID INNER JOIN OQUT as oq on qu.DocEntry = oq.DocEntry INNER JOIN [@SCGD_OT] as tb on T0.Code = tb.Code INNER JOIN OITM as oi with(nolock) on qu.ItemCode = oi.ItemCode INNER JOIN OHEM as oh with(nolock) on qu.OwnerCode = oh.empID INNER JOIN [@SCGD_ESTADOS_ACTOT] eo ON eo.Code = T0.U_Estad WHERE qu.U_SCGD_NombEmpleado IS NOT NULL AND ((T0.U_Estad = '{0}' AND T0.U_RePro IS NOT NULL) OR (T0.U_Estad = '{1}' AND T0.U_ReAsig IS NOT NULL)) AND T0.U_FechPro = '{2}'  AND T0.U_HoraIni IS NOT NULL AND oq.U_SCGD_idSucursal = '{3}' "
                strQuery = String.Format(strQuery, strSuspendida, strNoIniciada, strfecha, m_strCodSucursal)
                dtOrdenesReprogramadas = Utilitarios.EjecutarConsultaDataTable(strQuery)

                'Si hay ordenes reprogramadas se procede a dibujar en pantalla y asignar el tiempo
                If dtOrdenesReprogramadas.Rows.Count > 0 Then

                    'Recorre todas las líneas de la agenda
                    For Each row As DataRow In dtAgenda.Rows

                        Dim strIDAgenda As String = String.Empty

                        'El ID de la agenda debe ser 0 para identificarlo como mecanico
                        If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                            strIDAgenda = row.Item(mc_strIDAgenda)
                        End If

                        'El ID del mecánico no puede estar en blanco y el ID de la agenda debe ser "0" para identificar que es un mecánico
                        'y no el líder el grupo
                        If Not IsDBNull(row.Item(mc_strID)) And strIDAgenda = "0" Then

                            'Posicion actual del recorrido
                            Dim intposition As Integer = dtAgenda.Rows.IndexOf(row)
                            strIdEmp = row.Item(mc_strID)

                            'Obtiene el ID del mecanico de la linea actual
                            Dim strMecanicoAgenda As String = String.Empty
                            strMecanicoAgenda = dtAgenda.Rows(intposition)("ID").ToString()

                            'Recorre todas las lineas del datatable con las ordenes reprogramadas y las asigna en la agenda
                            For Each rowBloquear As DataRow In dtOrdenesReprogramadas.Rows
                                Dim intPosicionBloqueo As Integer = dtOrdenesReprogramadas.Rows.IndexOf(rowBloquear)
                                Dim strMecanicoDataTable As String = String.Empty
                                Dim strNumOT As String = String.Empty
                                strNumOT = dtOrdenesReprogramadas.Rows(intPosicionBloqueo)("U_SCGD_Numero_OT").ToString()
                                strMecanicoDataTable = dtOrdenesReprogramadas.Rows(intPosicionBloqueo)("U_Colab").ToString()

                                If strMecanicoAgenda = strMecanicoDataTable Then

                                    'El campo fecha no puede estar en blanco, de lo contrario no se sabe a que día se debe asignar el bloqueo
                                    If Not IsDBNull(rowBloquear.Item("U_FechPro")) Then
                                        'Hora inicio y fin en formato texto
                                        Dim strHoraInicio As String = Utilitarios.FormatoHora(rowBloquear.Item("U_HoraIni"))
                                        'Dim intDuracionEstandar As Integer = 0

                                        ''Obtiene la duración estándar de la actividad desde la oferta de ventas
                                        'If Not Integer.TryParse(rowBloquear.Item("U_SCGD_DurSt"), intDuracionEstandar) Then
                                        '    intDuracionEstandar = 15
                                        'End If

                                        intDuracionTotal = CalcularDuracionActividad(rowBloquear.Item("U_SCGD_DurSt").ToString(), rowBloquear.Item("U_SCGD_TiOtor").ToString(), rowBloquear.Item("U_SCGD_TiempServ").ToString(), rowBloquear.Item("Quantity").ToString())

                                        'Redondea los minutos a múltiplos de 15 para que coincidan con las columnas de la agenda
                                        strHoraInicio = RegresaHora(strHoraInicio.Replace(":", ""))

                                        If strHoraInicio.Length = 3 Then
                                            strHoraInicio = strHoraInicio.Insert(1, ":")
                                        ElseIf strHoraInicio.Length = 4 Then
                                            strHoraInicio = strHoraInicio.Insert(2, ":")
                                        End If

                                        'Hora inicio y fin en formato Date
                                        Dim dtmHoraInicio As Date = FormatDateTime(strHoraInicio, DateFormat.ShortTime)
                                        Dim dtmHoraFinal As Date = dtmHoraInicio.AddMinutes(intDuracionTotal)
                                        Dim dtmFechaBloqueo As Date = DateTime.Parse(rowBloquear.Item("U_FechPro"))

                                        'Verificamos que la suspension no coincida con el horario de almuerzo, en caso de ser así
                                        'se debe sumar el tiempo de almuerzo a la hora final de la actividad
                                        Dim dtmInicioAlmuerzo As Date
                                        Dim dtmFinAlmuerzo As Date
                                        Dim intDuracionAlmuerzo As Integer = 0

                                        If UsaVariosDias(dtmHoraInicio, intDuracionTotal) Then
                                            dtmHoraFinal = dtmHoraFinalAgenda
                                        Else
                                            ObtenerHorarioAlmuerzo(dtmInicioAlmuerzo, dtmFinAlmuerzo, intDuracionAlmuerzo)

                                            If dtmHoraInicio <= dtmInicioAlmuerzo And dtmHoraFinal >= dtmInicioAlmuerzo Then
                                                dtmHoraFinal = dtmHoraFinal.AddMinutes(intDuracionAlmuerzo)
                                                'Redondeamos nuevamente la hora final, en caso de que la hora de almuerzo no sea exacta y haya desajustado la hora
                                                Dim strHoraFinalRedondeada As String = RegresaHora(dtmHoraFinal.Hour.ToString() + dtmHoraFinal.Minute.ToString()).Insert(2, ":")
                                                dtmHoraFinal = FormatDateTime(strHoraFinalRedondeada, DateFormat.ShortTime)
                                            End If

                                            'Si la actividad suspendida se excede de la hora final de la agenda, se asigna como hora final la de la agenda
                                            If dtmHoraFinalAgenda < dtmHoraFinal Then
                                                dtmHoraFinal = dtmHoraFinalAgenda
                                            End If
                                        End If

                                        Dim intContador As Integer = 0

                                        'Recorre todas las columnas de la agenda
                                        For Each columna As String In listColumsGrid

                                            If columna.Equals(strHoraInicio.TrimStart("0″")) Then
                                                'Asigna el valor de la hora de inicio a una variable que va a ir aumentando su valor hasta llegar a la hora final
                                                Dim dtmHoraActividad As Date = dtmHoraInicio

                                                'Recorre todas las horas y les asigna el valor bloqueado hasta llegar a la hora final de la actividad
                                                While dtmHoraActividad < dtmHoraFinal
                                                    'Obtiene un arreglo de las columnas de la agenda en formato texto
                                                    Dim arregloColumnas As String() = listColumsGrid.ToArray()

                                                    If intContador < arregloColumnas.Length Then

                                                        'Verificamos que la celda no tenga asignada ninguna actividad ni tampoco este bloqueada
                                                        Dim strValorCelda = row.Item(arregloColumnas(intContador))

                                                        If String.IsNullOrEmpty(strValorCelda) Then
                                                            dgv_AgendaCitas.Rows(intposition).Cells(arregloColumnas(intContador)).Style.BackColor = Color.DarkSeaGreen
                                                            row.Item(arregloColumnas(intContador)) = strNumOT
                                                        Else
                                                            Exit For
                                                        End If

                                                        intContador += 1

                                                    End If

                                                    'Se agregan 15 minutos, para rellenar la siguiente posición de la agenda
                                                    dtmHoraActividad = dtmHoraActividad.AddMinutes(15)

                                                End While
                                            Else
                                                'En caso de que no se llegue a la columna, se va aumentando el contador
                                                intContador += 1
                                            End If

                                        Next

                                    End If

                                End If

                            Next
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    ''' <summary>
    ''' Completa en la agenda, los espacios correspondientes a órdenes de trabajo asignadas y no iniciadas.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LlenarOcupacionNoIniciadas()

        Dim strIdEmp As String
        Dim intDuracion As Integer = 15
        Dim intIntervUnitario As Integer = 15
        Dim dtOrdenesNoIniciadas As System.Data.DataTable
        Dim strBMensaje As StringBuilder = New StringBuilder()
        Dim strMinutos As String
        Dim strHora As String
        Dim strHoraCita As String
        Dim strSerieCita As String
        Dim strCodTecnico As String
        Dim strNumCita As String
        Dim strValor As String
        Dim strfecha As String
        Dim intCont As Integer = 0
        Dim strHoraOrden As String
        Dim strNoOrden As String
        Dim MiColor As Color
        Dim strIntervaloAct As String = ""
        Dim strSQL As String
        Dim strSQL_Int As String
        Dim strBDTaller As String
        Dim strQuery As String = String.Empty
        Dim strCodSuspensionHorario As String = "8"
        Dim strNoIniciada As String = "1"
        Dim strIniciada As String = "2"
        Dim strSuspendida As String = "3"
        Dim strTextoBloqueo As String = "n/a"
        Dim dtmHoraInicioAgenda As Date
        Dim dtmHoraFinalAgenda As Date

        Try
            'Hora de inicio y fin del calendario
            dtmHoraInicioAgenda = FormatDateTime(m_fhaHoraInicio, DateFormat.ShortTime)
            dtmHoraFinalAgenda = FormatDateTime(m_fhaHoraFin, DateFormat.ShortTime)

            'Verifica que se utilice taller adentro
            If m_strUsarTallerSAP.Equals("Y") Then

                m_dtFecha = dtpFecha.Value
                strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

                'Obtiene un datatable con la información de todas las ordenes no iniciadas en la fecha indicada
                strQuery = " SELECT Distinct oq.DocEntry, oq.U_SCGD_Numero_OT, T0.U_Estad, T0.U_Colab, T0.U_FechPro, T0.U_DFIni, T0.U_HoraIni, qu.U_SCGD_DurSt, qu.U_SCGD_TiOtor, oh.U_SCGD_TiempServ, qu.Quantity FROM [@SCGD_CTRLCOL] T0 WITH (nolock) INNER JOIN QUT1 as qu WITH (nolock) on T0.U_IdAct  = qu.U_SCGD_ID INNER JOIN OQUT as oq on qu.DocEntry = oq.DocEntry INNER JOIN [@SCGD_OT] as tb on T0.Code = tb.Code INNER JOIN OITM as oi with(nolock) on qu.ItemCode = oi.ItemCode INNER JOIN OHEM as oh with(nolock) on qu.OwnerCode = oh.empID INNER JOIN [@SCGD_ESTADOS_ACTOT] eo WITH (nolock) ON eo.Code = T0.U_Estad WHERE qu.U_SCGD_NombEmpleado IS NOT NULL AND (T0.U_Estad = '{0}' AND T0.U_RePro IS NULL AND T0.U_ReAsig IS NULL) AND T0.U_FechPro = '{1}'  AND T0.U_HoraIni IS NOT NULL AND oq.U_SCGD_idSucursal = '{2}' AND qu.U_SCGD_ID IS NOT NULL AND (qu.OrigItem IS NULL OR oq.U_SCGD_NoCita IS NULL) "
                strQuery = String.Format(strQuery, strNoIniciada, strfecha, m_strCodSucursal)
                dtOrdenesNoIniciadas = Utilitarios.EjecutarConsultaDataTable(strQuery)

                'Si hay ordenes no iniciadas se procede a dibujar en pantalla y asignar el tiempo
                If dtOrdenesNoIniciadas.Rows.Count > 0 Then

                    'Recorre todas las líneas de la agenda
                    For Each row As DataRow In dtAgenda.Rows

                        Dim strIDAgenda As String = String.Empty

                        'El ID de la agenda debe ser 0 para identificarlo como mecanico
                        If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                            strIDAgenda = row.Item(mc_strIDAgenda)
                        End If

                        'El ID del mecánico no puede estar en blanco y el ID de la agenda debe ser "0" para identificar que es un mecánico
                        'y no el líder el grupo
                        If Not IsDBNull(row.Item(mc_strID)) And strIDAgenda = "0" Then

                            'Posicion actual del recorrido
                            Dim intposition As Integer = dtAgenda.Rows.IndexOf(row)
                            strIdEmp = row.Item(mc_strID)

                            'Obtiene el ID del mecanico de la linea actual
                            Dim strMecanicoAgenda As String = String.Empty
                            strMecanicoAgenda = dtAgenda.Rows(intposition)("ID").ToString()

                            'Recorre todas las lineas del datatable con las ordenes no iniciadas y las asigna en la agenda
                            For Each rowBloquear As DataRow In dtOrdenesNoIniciadas.Rows
                                Dim intPosicionBloqueo As Integer = dtOrdenesNoIniciadas.Rows.IndexOf(rowBloquear)
                                Dim strMecanicoDataTable As String = String.Empty
                                Dim strNumOT As String = String.Empty
                                strNumOT = dtOrdenesNoIniciadas.Rows(intPosicionBloqueo)("U_SCGD_Numero_OT").ToString()
                                strMecanicoDataTable = dtOrdenesNoIniciadas.Rows(intPosicionBloqueo)("U_Colab").ToString()

                                If strMecanicoAgenda = strMecanicoDataTable Then

                                    'El campo fecha no puede estar en blanco, de lo contrario no se sabe a que día se debe asignar la actividad
                                    If Not IsDBNull(rowBloquear.Item("U_FechPro")) Then
                                        'Hora inicio y fin en formato texto
                                        Dim strHoraInicio As String = Utilitarios.FormatoHora(rowBloquear.Item("U_HoraIni"))
                                        'Dim intDuracionEstandar As Integer = 15
                                        'Dim intTiempoOtorgado As Integer = 0

                                        ''Obtiene la duración estándar y el tiempo otorgado adicional de la actividad desde la oferta de ventas
                                        'Integer.TryParse(rowBloquear.Item("U_SCGD_DurSt"), intDuracionEstandar)
                                        'Integer.TryParse(rowBloquear.Item("U_SCGD_TiOtor"), intTiempoOtorgado)

                                        intDuracion = CalcularDuracionActividad(rowBloquear.Item("U_SCGD_DurSt").ToString(), rowBloquear.Item("U_SCGD_TiOtor").ToString(), rowBloquear.Item("U_SCGD_TiempServ").ToString(), rowBloquear.Item("Quantity").ToString())

                                        'intDuracion = intDuracionEstandar + intTiempoOtorgado

                                        'Redondea los minutos a múltiplos de 15 para que coincidan con las columnas de la agenda
                                        strHoraInicio = RegresaHora(strHoraInicio.Replace(":", ""))

                                        If strHoraInicio.Length = 3 Then
                                            strHoraInicio = strHoraInicio.Insert(1, ":")
                                        ElseIf strHoraInicio.Length = 4 Then
                                            strHoraInicio = strHoraInicio.Insert(2, ":")
                                        End If

                                        'Hora inicio y fin en formato Date
                                        Dim dtmHoraInicio As Date = FormatDateTime(strHoraInicio, DateFormat.ShortTime)
                                        Dim dtmHoraFinal As Date = dtmHoraInicio.AddMinutes(intDuracion)
                                        Dim dtmFechaBloqueo As Date = DateTime.Parse(rowBloquear.Item("U_FechPro"))

                                        If UsaVariosDias(dtmHoraInicio, intDuracion) Then
                                            dtmHoraFinal = dtmHoraFinalAgenda
                                        Else
                                            'Verificamos que la suspension no coincida con el horario de almuerzo, en caso de ser así
                                            'se debe sumar el tiempo de almuerzo a la hora final de la actividad
                                            Dim dtmInicioAlmuerzo As Date
                                            Dim dtmFinAlmuerzo As Date
                                            Dim intDuracionAlmuerzo As Integer = 0

                                            ObtenerHorarioAlmuerzo(dtmInicioAlmuerzo, dtmFinAlmuerzo, intDuracionAlmuerzo)

                                            If dtmHoraInicio <= dtmInicioAlmuerzo And dtmHoraFinal >= dtmInicioAlmuerzo Then
                                                dtmHoraFinal = dtmHoraFinal.AddMinutes(intDuracionAlmuerzo)
                                                'Redondeamos nuevamente la hora final, en caso de que la hora de almuerzo no sea exacta y haya desajustado la hora
                                                Dim strHoraFinalRedondeada As String = RegresaHora(dtmHoraFinal.Hour.ToString() + dtmHoraFinal.Minute.ToString()).Insert(2, ":")
                                                dtmHoraFinal = FormatDateTime(strHoraFinalRedondeada, DateFormat.ShortTime)
                                            End If

                                            'Si la actividad suspendida se excede de la hora final de la agenda, se asigna como hora final la de la agenda
                                            If dtmHoraFinalAgenda < dtmHoraFinal Then
                                                dtmHoraFinal = dtmHoraFinalAgenda
                                            End If
                                        End If

                                        Dim intContador As Integer = 0

                                        'Recorre todas las columnas de la agenda
                                        For Each columna As String In listColumsGrid

                                            If columna.Equals(strHoraInicio.TrimStart("0″")) Then
                                                'Asigna el valor de la hora de inicio a una variable que va a ir aumentando su valor hasta llegar a la hora final
                                                Dim dtmHoraActividad As Date = dtmHoraInicio

                                                'Recorre todas las horas y les asigna el valor bloqueado hasta llegar a la hora final de la actividad
                                                While dtmHoraActividad < dtmHoraFinal
                                                    'Obtiene un arreglo de las columnas de la agenda en formato texto
                                                    Dim arregloColumnas As String() = listColumsGrid.ToArray()

                                                    If intContador < arregloColumnas.Length Then

                                                        'Verificamos que la celda no tenga asignada ninguna actividad ni tampoco este bloqueada
                                                        Dim strValorCelda = row.Item(arregloColumnas(intContador))

                                                        If String.IsNullOrEmpty(strValorCelda) Then
                                                            dgv_AgendaCitas.Rows(intposition).Cells(arregloColumnas(intContador)).Style.BackColor = Color.DarkSeaGreen
                                                            row.Item(arregloColumnas(intContador)) = strNumOT
                                                        Else
                                                            Exit For
                                                        End If

                                                        intContador += 1

                                                    End If

                                                    'Se agregan 15 minutos, para rellenar la siguiente posición de la agenda
                                                    dtmHoraActividad = dtmHoraActividad.AddMinutes(15)

                                                End While
                                            Else
                                                'En caso de que no se llegue a la columna, se va aumentando el contador
                                                intContador += 1
                                            End If

                                        Next

                                    End If

                                End If

                            Next
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    Public Function UsaVariosDias(ByRef dtInicioActividad As Date, ByVal intDuracionActividad As Integer) As Boolean
        Dim blnResultado As Boolean = False
        Dim dtFechaInicioCompleta As Date
        Dim dtFechaFinCompleta As Date
        Try
            dtFechaInicioCompleta = New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, dtInicioActividad.Hour, dtInicioActividad.Minute, 0)
            dtFechaFinCompleta = dtFechaInicioCompleta.AddMinutes(intDuracionActividad)

            'Si la actividad se extiende por más de un día se le asigna como fecha de fin de actividad la hora de cierre de la sucursal
            If Not dtFechaInicioCompleta.DayOfYear = dtFechaFinCompleta.DayOfYear Or Not dtFechaInicioCompleta.Year = dtFechaFinCompleta.Year Then
                blnResultado = True
            End If
            Return blnResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Devuelve la hora de almuerzo desde la tabla de configuraciones generales del addon [@SCGD_CONF_SUCURSAL]
    ''' </summary>
    ''' <param name="dtmInicioAlmuerzo">Hora de inicio del almuerzo</param>
    ''' <param name="dtmFinAlmuerzo">Hora de fin del almuerzo</param>
    ''' <remarks></remarks>
    Public Sub ObtenerHorarioAlmuerzo(ByRef dtmInicioAlmuerzo As Date, ByRef dtmFinAlmuerzo As Date, ByRef duracionAlmuerzo As Integer)

        Dim strQuery As String = "SELECT U_HorAlI, U_HoraAlF FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs ='{0}'"
        Dim dtConfiguracion As System.Data.DataTable
        Dim strHoraInicio, strHoraFin As String

        Try
            dtConfiguracion = Utilitarios.EjecutarConsultaDataTable(String.Format(strQuery, m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            'Si se obtuvieron datos se procede a asignar las respectivas horas de inicio y fin de almuerzo
            If dtConfiguracion.Rows.Count > 0 Then
                If Not IsDBNull(dtConfiguracion.Rows(0).Item("U_HorAlI")) AndAlso
                    Not IsDBNull(dtConfiguracion.Rows(0).Item("U_HoraAlF")) Then

                    strHoraInicio = dtConfiguracion.Rows(0).Item("U_HorAlI")
                    strHoraFin = dtConfiguracion.Rows(0).Item("U_HoraAlF")

                    strHoraInicio = strHoraInicio.Insert(2, ":")
                    strHoraFin = strHoraFin.Insert(2, ":")

                    dtmInicioAlmuerzo = FormatDateTime(strHoraInicio, DateFormat.ShortTime)
                    dtmFinAlmuerzo = FormatDateTime(strHoraFin, DateFormat.ShortTime)
                    duracionAlmuerzo = DateDiff(DateInterval.Minute, dtmInicioAlmuerzo, dtmFinAlmuerzo)

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    Public Sub LlenarOcupacionPorGruposOrdenes(ByRef p_dtAgendas As System.Data.DataTable)

        Try

            Dim strIdEmp As String
            Dim intDuracion As Integer
            Dim intTiempoOtorgado As Integer = 0
            Dim intIntervUnitario As Integer = 15

            Dim dtOrdenes As System.Data.DataTable
            Dim strBMensaje As StringBuilder = New StringBuilder()

            Dim strMinutos As String
            Dim strHora As String
            Dim strHoraCita As String
            Dim strSerieCita As String
            Dim strCodTecnico As String
            Dim strDuracionEstandar As String = String.Empty 'Tiempo estándar de la actividad
            Dim strTiempoOtorgado As String = String.Empty 'Representa el tiempo que se le agrega o resta a la actividad adicional al tiempo estándar
            Dim strTiempoServicioRapido As String = String.Empty 'Representa el tiempo de servicio rápido que realiza un empleado
            Dim strCantidad As String = String.Empty
            Dim strNumCita As String
            Dim strValor As String
            Dim strfecha As String
            Dim intCont As Integer = 0
            Dim strHoraOrden As String
            Dim strNoOrden As String
            Dim MiColor As Color
            Dim strIntervaloAct As String = ""
            Dim strSQL As String
            Dim strSQL_Int As String
            Dim strBDTaller As String
            Dim strIDAgenda As String = String.Empty
            Dim strDescripcionCelda As String = String.Empty
            Dim strPlaca As String = String.Empty

            strBDTaller = m_strNombreBDTaller

            strSQL = " Select  Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT, replace(cc.horainicio,':', '') as horainicio,CI.U_FhaServ, CI.U_HoraServ , isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig) U_Cod_Tecnico , ISNULL(AC.U_Color,'DarkSeaGreen') as U_Color " +
            " from OQUT QU with (nolock) " +
            " LEFT OUTER join QUT1 Q1 with (nolock) on QU.DocEntry = Q1.DocEntry " +
            " left outer join [@SCGD_CITA] CI with (nolock) on Ci.U_Num_Cot = QU.DocEntry " +
            " left outer join {2}.dbo.SCGTA_TB_ControlColaborador  CC with (nolock) on  cc.IDActividad  = Q1.U_SCGD_IdRepxOrd and U_SCGD_TipArt = 2  " +
            " left outer join {2}.dbo.SCGTA_TB_Orden ORD with (nolock) on cc.NoOrden = ORD.NoOrden " +
            " where " +
            " (QU.U_SCGD_Estado_Cot in('{3}','{4}')  " +
            " and (cc.FechaProgramacion = '{0}' or CI.U_FhaServ = '{0}') " +
            " and Q1.U_SCGD_EmpAsig = '{1}') " +
            " OR " +
            " (QU.U_SCGD_Estado_Cot in('{5}') " +
            " and (cc.FechaProgramacion = '{0}' and cc.FechaProgramacion <> '1900-01-01' ) " +
            " and Q1.U_SCGD_EmpAsig = '{1}') " +
            " GROUP BY qU.DocEntry,Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT,cc.horainicio,CI.U_FhaServ, CI.U_HoraServ, isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig) order by isnull (horainicio ,U_HoraServ ) asc "

            strSQL_Int = " Select  Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT, QU.U_SCGD_Num_Placa, cc.U_HFIni as horainicio,CI.U_FhaServ, CI.U_HoraServ, ISNULL(cc.U_Colab,isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig)) U_Cod_Tecnico  , ISNULL(AC.U_Color,'DarkSeaGreen') as U_Color, ISNULL(CE.U_ColorOT,'DarkSeaGreen') as ColorEstado, Q1.U_SCGD_DurSt, Q1.U_SCGD_TiOtor, T0.U_SCGD_TiempServ, Q1.Quantity " +
            " from OQUT QU with (nolock) " +
            " LEFT OUTER join QUT1 Q1 with (nolock) on QU.DocEntry = Q1.DocEntry  " +
            " left outer join [@SCGD_CITA] CI with (nolock) on Ci.U_Num_Cot = QU.DocEntry  " +
            " left outer join [@SCGD_CTRLCOL] CC with (nolock) on  cc.U_IdAct  = Q1.U_SCGD_ID and CC.U_Estad = '2' " +
            " left outer join [@SCGD_COLORESAGENDA] AC with (nolock)  on AC.U_RazonCita = Ci.U_Cod_Razon " +
            " left outer join OHEM T0 with (nolock) on cc.U_Colab = T0.empID " +
            " LEFT OUTER JOIN [@SCGD_CITA_ESTADOS] CE ON CI.U_Estado = CE.Code" +
            " where" +
            " (QU.U_SCGD_Estado_CotID in('{3}','{4}') " +
            " and (cc.U_DFIni = '{0}' or CI.U_FhaServ = '{0}') ) " +
            " OR " +
            " (QU.U_SCGD_Estado_CotID in('{5}') " +
            " and (cc.U_FechPro = '{0}' and cc.U_DFIni <> '1900-01-01' and CC.U_FechPro is not null) ) " +
            " GROUP BY qU.DocEntry,Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT, QU.U_SCGD_Num_Placa, cc.U_HFIni,CI.U_FhaServ, CI.U_HoraServ, ISNULL(cc.U_Colab,isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig)) , AC.U_Color, CE.U_Color, CE.U_ColorOT, Q1.U_SCGD_DurSt, Q1.U_SCGD_TiOtor, T0.U_SCGD_TiempServ, Q1.Quantity order by isnull (cc.U_HFIni ,U_HoraServ ) asc "


            m_dtFecha = dtpFecha.Value
            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

            If m_strUsarTallerSAP.Equals("Y") Then
                strSQL = strSQL_Int
                dtOrdenes = Utilitarios.EjecutarConsultaDataTable(String.Format(strSQL, strfecha, String.Empty, strBDTaller, "1", "2", "3"))
            Else
                dtOrdenes = Utilitarios.EjecutarConsultaDataTable(String.Format(strSQL, strfecha, strIdEmp, strBDTaller, "1", "2", "3"))
            End If

            If dtOrdenes.Rows.Count > 0 Then
                For Each row As DataRow In p_dtAgendas.Rows

                    'El ID de la agenda debe ser 0 para identificarlo como mecanico
                    If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                        strIDAgenda = row.Item(mc_strIDAgenda)
                    End If

                    If Not IsDBNull(row.Item(mc_strID)) And strIDAgenda = "0" Then

                        Dim intposition As Integer = p_dtAgendas.Rows.IndexOf(row)
                        strIdEmp = row.Item(mc_strID)

                        For Each rowCitas As DataRow In dtOrdenes.Rows
                            strCodTecnico = IIf(IsDBNull(rowCitas.Item("U_Cod_Tecnico")), "", rowCitas.Item("U_Cod_Tecnico"))
                            'Verifica que el empleado de la línea de la agenda sea el mismo que la línea del dataTable
                            If strIdEmp = strCodTecnico Then
                                strBMensaje.Length = 0
                                intCont = 0

                                If (Not IsDBNull(rowCitas.Item("U_SCGD_NoSerieCita")) And
                                    Not IsDBNull(rowCitas.Item("U_SCGD_NoCita"))) Or
                                    (Not IsDBNull(rowCitas.Item("U_SCGD_Numero_OT"))) Then

                                    strHoraCita = IIf(IsDBNull(rowCitas.Item("U_HoraServ")), "", rowCitas.Item("U_HoraServ"))
                                    strSerieCita = IIf(IsDBNull(rowCitas.Item("U_SCGD_NoSerieCita")), "", rowCitas.Item("U_SCGD_NoSerieCita"))
                                    strNumCita = IIf(IsDBNull(rowCitas.Item("U_SCGD_NoCita")), "", rowCitas.Item("U_SCGD_NoCita"))

                                    strNoOrden = IIf(IsDBNull(rowCitas.Item("U_SCGD_Numero_OT")), "", rowCitas.Item("U_SCGD_Numero_OT"))
                                    strHoraOrden = IIf(IsDBNull(rowCitas.Item("horainicio")), "", rowCitas.Item("horainicio"))

                                    If strHoraOrden.Length = 3 Then strHoraOrden = String.Format("0{0}", strHoraOrden)
                                    strHoraOrden = RegresaHora(strHoraOrden)
                                    If Not String.IsNullOrEmpty(strHoraOrden) Then

                                        If strHoraOrden.Substring(0, 1).Equals("0") Then
                                            strHora = strHoraOrden.Substring(1, 1)
                                            strMinutos = strHoraOrden.Substring(2, 2)
                                        Else
                                            strHora = strHoraOrden.Substring(0, 2)
                                            strMinutos = strHoraOrden.Substring(2, 2)
                                        End If

                                        If m_strUsarTallerSAP.Equals("Y") Then
                                            strDuracionEstandar = IIf(IsDBNull(rowCitas.Item("U_SCGD_DurSt")), "", rowCitas.Item("U_SCGD_DurSt"))
                                            strTiempoOtorgado = IIf(IsDBNull(rowCitas.Item("U_SCGD_TiOtor")), "", rowCitas.Item("U_SCGD_TiOtor"))
                                            strTiempoServicioRapido = IIf(IsDBNull(rowCitas.Item("U_SCGD_TiempServ")), "", rowCitas.Item("U_SCGD_TiempServ"))
                                            strCantidad = IIf(IsDBNull(rowCitas.Item("Quantity")), "", rowCitas.Item("Quantity"))

                                            intDuracion = CalcularDuracionActividad(strDuracionEstandar, strTiempoOtorgado, strTiempoServicioRapido, strCantidad)
                                        Else
                                            intDuracion = ObtenerDuracionOrden(strNoOrden, strIdEmp, m_strCodSucursal)
                                        End If

                                    Else
                                        If strHoraCita.Length = 3 Then
                                            strHora = strHoraCita.Substring(0, 1)
                                            strMinutos = strHoraCita.Substring(1, 2)
                                        ElseIf strHoraCita.Length = 4 Then
                                            strHora = strHoraCita.Substring(0, 2)
                                            strMinutos = strHoraCita.Substring(2, 2)
                                        End If


                                        intDuracion = ObtenerDuracionCita(strSerieCita, strNumCita, m_strCodSucursal, strCodTecnico)
                                    End If

                                    strValor = strNoOrden
                                    strDescripcionCelda = strNoOrden

                                    If m_strUsarTallerSAP = "Y" Then
                                        If Not IsDBNull(rowCitas.Item("U_SCGD_Num_Placa")) Then
                                            strPlaca = rowCitas.Item("U_SCGD_Num_Placa")
                                            If Not String.IsNullOrEmpty(strPlaca) Then
                                                strDescripcionCelda = String.Format("{0}/{1}", strPlaca, strNoOrden)
                                            End If
                                        End If
                                    End If

                                    If intDuracion = 0 Or intDuracion < intIntervUnitario Then
                                        intDuracion = intIntervUnitario
                                    End If

                                    strIntervaloAct = String.Empty

                                    For Each element As String In listColumsGrid

                                        Dim result As String() = element.Split(New Char() {":"c})
                                        Dim horAgenda As String = result(0)
                                        Dim minAgenda As String = result(1)
                                        Dim valorColumna As String()

                                        If strHora.Equals(horAgenda) And strMinutos.Equals(minAgenda) Then

                                            For intI As Integer = intDuracion To 1 Step -15
                                                If intCont <= listColumsGrid.Count - 1 Then

                                                    valorColumna = listColumsGrid.ToArray()
                                                    If row.Item(valorColumna(intCont)) <> "n/a" Then
                                                        If dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor.Name.Equals("0") Then
                                                            row.Item(valorColumna(intCont)) = strDescripcionCelda
                                                            If m_strTipoAgendaColor.ToUpper().ToString() = "Y" Then
                                                                If oGestionColor = GestionColor.EstadoCita Then
                                                                    dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor = MiColor.FromName(rowCitas.Item("ColorEstado").ToString)
                                                                Else
                                                                    dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor = MiColor.FromName(rowCitas.Item("U_Color").ToString)
                                                                End If
                                                            Else
                                                                dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor = Color.DarkSeaGreen
                                                            End If
                                                        Else
                                                            Dim strDocEntryAct As String = rowCitas.Item("DocEntry")
                                                            If Not tableCitasProbl.ContainsKey(strDocEntryAct) Then
                                                                tableCitasProbl.Add(strDocEntryAct, String.Format("{2}  {0}-{1}", strSerieCita, strNumCita, strValor))
                                                            End If
                                                            Exit For
                                                        End If
                                                    Else
                                                        intI = intI + 15
                                                    End If
                                                End If
                                                intCont += 1
                                            Next
                                        End If
                                        intCont += 1
                                    Next
                                End If
                            End If
                        Next
                        intCont = 0
                    End If
                Next
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    'Respaldo del código 14 diciembre 2017
    'Public Sub LlenarOcupacionPorGruposOrdenes(ByRef p_dtAgendas As System.Data.DataTable)
    '    Try

    '        Dim strIdEmp As String
    '        Dim intDuracion As Integer
    '        Dim intIntervUnitario As Integer = 15

    '        Dim dtOrdenes As System.Data.DataTable
    '        Dim strBMensaje As StringBuilder = New StringBuilder()

    '        Dim strMinutos As String
    '        Dim strHora As String
    '        Dim strHoraCita As String
    '        Dim strSerieCita As String
    '        Dim strCodTecnico As String
    '        Dim strNumCita As String
    '        Dim strValor As String
    '        Dim strfecha As String
    '        Dim intCont As Integer = 0
    '        Dim strHoraOrden As String
    '        Dim strNoOrden As String
    '        Dim MiColor As Color
    '        Dim strIntervaloAct As String = ""
    '        Dim strSQL As String
    '        Dim strSQL_Int As String
    '        Dim strBDTaller As String

    '        strBDTaller = m_strNombreBDTaller

    '        strSQL = " Select  Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT, replace(cc.horainicio,':', '') as horainicio,CI.U_FhaServ, CI.U_HoraServ , isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig) U_Cod_Tecnico , ISNULL(AC.U_Color,'DarkSeaGreen') as U_Color " +
    '        " from OQUT QU with (nolock) " +
    '        " LEFT OUTER join QUT1 Q1 with (nolock) on QU.DocEntry = Q1.DocEntry " +
    '        " left outer join [@SCGD_CITA] CI with (nolock) on Ci.U_Num_Cot = QU.DocEntry " +
    '        " left outer join {2}.dbo.SCGTA_TB_ControlColaborador  CC with (nolock) on  cc.IDActividad  = Q1.U_SCGD_IdRepxOrd and U_SCGD_TipArt = 2  " +
    '        " left outer join {2}.dbo.SCGTA_TB_Orden ORD with (nolock) on cc.NoOrden = ORD.NoOrden " +
    '        " where " +
    '        " (QU.U_SCGD_Estado_Cot in('{3}','{4}')  " +
    '        " and (cc.FechaProgramacion = '{0}' or CI.U_FhaServ = '{0}') " +
    '        " and Q1.U_SCGD_EmpAsig = '{1}') " +
    '        " OR " +
    '        " (QU.U_SCGD_Estado_Cot in('{5}') " +
    '        " and (cc.FechaProgramacion = '{0}' and cc.FechaProgramacion <> '1900-01-01' ) " +
    '        " and Q1.U_SCGD_EmpAsig = '{1}') " +
    '        " GROUP BY qU.DocEntry,Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT,cc.horainicio,CI.U_FhaServ, CI.U_HoraServ, isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig) order by isnull (horainicio ,U_HoraServ ) asc "

    '        strSQL_Int = " Select  Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT, cc.U_HFIni as horainicio,CI.U_FhaServ, CI.U_HoraServ, isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig) U_Cod_Tecnico  , ISNULL(AC.U_Color,'DarkSeaGreen') as U_Color " +
    '        " from OQUT QU with (nolock) " +
    '        " LEFT OUTER join QUT1 Q1 with (nolock) on QU.DocEntry = Q1.DocEntry  " +
    '        " left outer join [@SCGD_CITA] CI with (nolock) on Ci.U_Num_Cot = QU.DocEntry  " +
    '        " left outer join [@SCGD_CTRLCOL] CC with (nolock) on  cc.U_IdAct  = Q1.U_SCGD_ID and CC.U_Estad =2 " +
    '        " left outer join [@SCGD_COLORESAGENDA] AC with (nolock)  on AC.U_RazonCita = Ci.U_Cod_Razon " +
    '        " where" +
    '        " (QU.U_SCGD_Estado_CotID in('{3}','{4}') " +
    '        " and (cc.U_DFIni = '{0}' or CI.U_FhaServ = '{0}') " +
    '        " and Q1.U_SCGD_EmpAsig = '{1}') " +
    '        " OR " +
    '        " (QU.U_SCGD_Estado_CotID in('{5}') " +
    '        " and (cc.U_FechPro = '{0}' and cc.U_DFIni <> '1900-01-01' and CC.U_FechPro is not null) " +
    '        " and Q1.U_SCGD_EmpAsig = '{1}')   " +
    '        " GROUP BY qU.DocEntry,Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT,cc.U_HFIni,CI.U_FhaServ, CI.U_HoraServ, isnull(CI.U_Cod_Tecnico,Q1.U_SCGD_EmpAsig) , AC.U_Color order by isnull (cc.U_HFIni ,U_HoraServ ) asc"


    '        m_dtFecha = dtpFecha.Value
    '        strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

    '        For Each row As DataRow In p_dtAgendas.Rows

    '            If Not IsDBNull(row.Item(mc_strID)) Then

    '                Dim intposition As Integer = p_dtAgendas.Rows.IndexOf(row)
    '                strIdEmp = row.Item(mc_strID)

    '                If m_strUsarTallerSAP.Equals("Y") Then
    '                    strSQL = strSQL_Int
    '                End If

    '                dtOrdenes = Utilitarios.EjecutarConsultaDataTable(String.Format(strSQL, strfecha, strIdEmp, strBDTaller, "1", "2", "3"))

    '                For Each rowCitas As DataRow In dtOrdenes.Rows

    '                    strBMensaje.Length = 0
    '                    intCont = 0

    '                    If (Not IsDBNull(rowCitas.Item("U_SCGD_NoSerieCita")) And
    '                        Not IsDBNull(rowCitas.Item("U_SCGD_NoCita"))) Or
    '                        (Not IsDBNull(rowCitas.Item("U_SCGD_Numero_OT"))) Then

    '                        strHoraCita = IIf(IsDBNull(rowCitas.Item("U_HoraServ")), "", rowCitas.Item("U_HoraServ"))
    '                        strSerieCita = IIf(IsDBNull(rowCitas.Item("U_SCGD_NoSerieCita")), "", rowCitas.Item("U_SCGD_NoSerieCita"))
    '                        strNumCita = IIf(IsDBNull(rowCitas.Item("U_SCGD_NoCita")), "", rowCitas.Item("U_SCGD_NoCita"))

    '                        strNoOrden = IIf(IsDBNull(rowCitas.Item("U_SCGD_Numero_OT")), "", rowCitas.Item("U_SCGD_Numero_OT"))
    '                        strHoraOrden = IIf(IsDBNull(rowCitas.Item("horainicio")), "", rowCitas.Item("horainicio"))
    '                        strCodTecnico = IIf(IsDBNull(rowCitas.Item("U_Cod_Tecnico")), "", rowCitas.Item("U_Cod_Tecnico"))
    '                        If strHoraOrden.Length = 3 Then strHoraOrden = String.Format("0{0}", strHoraOrden)
    '                        strHoraOrden = RegresaHora(strHoraOrden)
    '                        If Not String.IsNullOrEmpty(strHoraOrden) Then

    '                            If strHoraOrden.Substring(0, 1).Equals("0") Then
    '                                strHora = strHoraOrden.Substring(1, 1)
    '                                strMinutos = strHoraOrden.Substring(2, 2)
    '                            Else
    '                                strHora = strHoraOrden.Substring(0, 2)
    '                                strMinutos = strHoraOrden.Substring(2, 2)
    '                            End If

    '                            intDuracion = ObtenerDuracionOrden(strNoOrden, strIdEmp, m_strCodSucursal)

    '                        Else
    '                            If strHoraCita.Length = 3 Then
    '                                strHora = strHoraCita.Substring(0, 1)
    '                                strMinutos = strHoraCita.Substring(1, 2)
    '                            ElseIf strHoraCita.Length = 4 Then
    '                                strHora = strHoraCita.Substring(0, 2)
    '                                strMinutos = strHoraCita.Substring(2, 2)
    '                            End If


    '                            intDuracion = ObtenerDuracionCita(strSerieCita, strNumCita, m_strCodSucursal, strCodTecnico)
    '                        End If

    '                        strValor = strNoOrden

    '                        If intDuracion = 0 Or intDuracion < intIntervUnitario Then
    '                            intDuracion = intIntervUnitario
    '                        End If

    '                        strIntervaloAct = String.Empty

    '                        For Each element As String In listColumsGrid

    '                            Dim result As String() = element.Split(New Char() {":"c})
    '                            Dim horAgenda As String = result(0)
    '                            Dim minAgenda As String = result(1)
    '                            Dim valorColumna As String()

    '                            If strHora.Equals(horAgenda) And strMinutos.Equals(minAgenda) Then

    '                                For intI As Integer = intDuracion To 1 Step -15
    '                                    If intCont <= listColumsGrid.Count - 1 Then

    '                                        valorColumna = listColumsGrid.ToArray()
    '                                        If row.Item(valorColumna(intCont)) <> "n/a" Then
    '                                            If dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor.Name.Equals("0") Then
    '                                                row.Item(valorColumna(intCont)) = strValor
    '                                                If m_strTipoAgendaColor.ToUpper().ToString() = "Y" Then
    '                                                    dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor = MiColor.FromName(rowCitas.Item("U_Color").ToString)
    '                                                Else
    '                                                    dgv_AgendaCitas.Rows(intposition).Cells(valorColumna(intCont)).Style.BackColor = Color.DarkSeaGreen
    '                                                End If
    '                                            Else
    '                                                Dim strDocEntryAct As String = rowCitas.Item("DocEntry")
    '                                                If Not tableCitasProbl.ContainsKey(strDocEntryAct) Then
    '                                                    tableCitasProbl.Add(strDocEntryAct, String.Format("{2}  {0}-{1}", strSerieCita, strNumCita, strValor))
    '                                                End If
    '                                                Exit For
    '                                            End If
    '                                        Else
    '                                            intI = intI + 15
    '                                        End If
    '                                    End If
    '                                    intCont += 1
    '                                Next
    '                            End If
    '                            intCont += 1
    '                        Next
    '                    End If
    '                Next
    '                intCont = 0
    '            End If
    '        Next

    '    Catch ex As Exception
    '        Utilitarios.ManejadorErrores(ex, m_oApplication)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Devuelve la hora con los minutos redondeados a múltiplos de 15
    ''' </summary>
    ''' <param name="p_strHora">Hora en formato texto sin carácteres especiales. Ejemplo: 15, 100, 315, 1600. </param>
    ''' <returns>Hora redondeada a múltiplos de 15 en formato texto sin carácteres especiales.</returns>
    ''' <remarks></remarks>
    Private Function RegresaHora(ByVal p_strHora As String) As String
        Dim m_strHoraE As String = String.Empty
        Dim m_strHoraReal As String
        Dim m_strMinutos As String = String.Empty
        Dim m_ValorMin As Integer

        Try

            Select Case p_strHora.Length
                Case 1
                    'La hora no puede ser cero
                    If Not p_strHora.Equals("0") Then
                        'La hora no tiene detalle de los minutos, por lo tanto se toma como minutos 00
                        m_strHoraE = p_strHora
                        m_strMinutos = "00"
                    End If
                Case 2
                    'La hora no tiene detalle de los minutos, por lo tanto se toma como minutos 00
                    m_strHoraE = p_strHora
                    m_strMinutos = "00"
                Case 3
                    'El primer número representa la hora, los siguientes dos los minutos
                    m_strHoraE = p_strHora.Substring(0, 1)
                    m_strMinutos = p_strHora.Substring(1)
                Case 4
                    'Los primeros dos números representan la hora, los siguientes dos los minutos
                    m_strHoraE = p_strHora.Substring(0, 2)
                    m_strMinutos = p_strHora.Substring(2)
                Case Else
                    'Si no concuerda con ninguno de los formatos, la hora debe estar incorrecta
                    'se devuelve el valor para que se genere una excepción en el método superior y muestre el error al usuario
                    Return p_strHora
            End Select

            If Not String.IsNullOrEmpty(m_strMinutos) Then
                'Se realiza un redondeo a múltiplos de 15, esto debido a que la agenda se administra
                'en intervalos de 15 minutos solamente. Ejemplo: 0, 15, 30, 45.
                m_ValorMin = Convert.ToInt32(m_strMinutos)

                Select Case m_ValorMin
                    Case 0
                        m_strMinutos = "00"
                    Case 1 To 14
                        m_strMinutos = "15"
                    Case 16 To 29
                        m_strMinutos = "30"
                    Case 31 To 44
                        m_strMinutos = "45"
                    Case 45 To 59                        'Si es mayor o igual a 45, se redondea a 45
                        m_strMinutos = "45"
                End Select

            End If

            m_strHoraReal = String.Format("{0}{1}", m_strHoraE, m_strMinutos)

            Return m_strHoraReal

        Catch ex As Exception
            Throw
        End Try

    End Function

    Public Sub LlenarOcupacionPorGruposTecnico(ByRef p_dtAgendas As System.Data.DataTable)
        Try

            Dim strIdEmp As String
            Dim intDuracion As Integer
            Dim intIntervUnitario As Integer = 15

            Dim dtCitas As System.Data.DataTable
            Dim strBMensaje As StringBuilder = New StringBuilder()

            Dim strMinutos As String
            Dim strHora As String
            Dim intMinutos As Integer
            Dim strHoraCita As String
            Dim intCantMinutos As Integer
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim strCita As String
            Dim strCodTecnico As String
            Dim strEstadoCancelado As String
            Dim strfecha As String
            Dim intCont As Integer = 0
            Dim MiColor As Color

            Dim strIntervaloAct As String = ""
            Dim strSQL As String
            Dim strIDAgenda As String = String.Empty
            Dim strDescripcionCelda As String = String.Empty
            Dim strPlaca As String = String.Empty
            Dim CodigoTecnicoCita As String = String.Empty

            m_dtFecha = dtpFecha.Value
            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

            strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            strSQL = " Select CI.DocEntry, CI.U_HoraCita, CI.U_NumCita, CI.U_Num_Serie, CI.U_Cod_Unid, CI.U_Num_Placa, CI.U_CardCode, CI.U_CardName , CI.U_FhaServ ,CI.U_HoraServ, CI.U_Cod_Tecnico , ISNULL(CO.U_Color,'DarkSeaGreen') as Color, ISNULL(CE.U_Color,'DarkSeaGreen') as ColorEstado " +
                        " from [@SCGD_CITA] CI " +
                        " LEFT JOIN [@SCGD_COLORESAGENDA] CO WITH (NOLOCK) ON CI.U_Cod_Razon = CO.U_RazonCita " +
                        " LEFT JOIN [@SCGD_CITA_ESTADOS] CE WITH (NOLOCK) ON CI.U_Estado = CE.Code" +
                        " INNER JOIN OQUT QU WITH (NOLOCK) ON QU.DocEntry = CI.U_Num_Cot" +
                        " where  CI.U_FhaServ = '{0}' and ( CI.U_Estado <> '{1}' or CI.U_Estado is null) " +
                        " AND (QU.U_SCGD_Numero_OT is null or QU.U_SCGD_Numero_OT = '')" +
                        " order by CI.U_HoraCita ASC"

            dtCitas = Utilitarios.EjecutarConsultaDataTable(String.Format(strSQL, strfecha, strEstadoCancelado))

            If dtCitas.Rows.Count > 0 Then
                For Each row As DataRow In p_dtAgendas.Rows

                    If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                        strIDAgenda = row.Item(mc_strIDAgenda)
                    Else
                        strIDAgenda = String.Empty
                    End If

                    'If Not IsDBNull(row.Item(mc_strID)) AndAlso (Not IsDBNull(row.Item(mc_strIDAgenda)) AndAlso CInt(row.Item(mc_strIDAgenda))) <> 0 Then
                    If Not IsDBNull(row.Item(mc_strID)) And strIDAgenda = "0" Then

                        Dim intposition As Integer = p_dtAgendas.Rows.IndexOf(row)
                        strIdEmp = row.Item(mc_strID)



                        For Each Column As System.Data.DataColumn In p_dtAgendas.Columns

                            Dim strNameColumn As String = Column.ColumnName

                            If p_dtAgendas.Rows(intposition)(strNameColumn).ToString() = "n/a" Then
                                dgv_AgendaCitas.Rows(intposition).Cells(strNameColumn).Style.BackColor = Color.DarkGray
                            End If
                        Next

                        For Each rowCitas As DataRow In dtCitas.Rows
                            CodigoTecnicoCita = rowCitas.Item("U_Cod_Tecnico")

                            If strIdEmp = CodigoTecnicoCita Then
                                strBMensaje.Length = 0
                                intCont = 0

                                If Not IsDBNull(rowCitas.Item("U_HoraServ")) And
                                    Not IsDBNull(rowCitas.Item("U_Num_Serie")) And
                                    Not IsDBNull(rowCitas.Item("U_NumCita")) Then

                                    strHoraCita = rowCitas.Item("U_HoraServ")
                                    strSerieCita = rowCitas.Item("U_Num_Serie")
                                    strNumCita = rowCitas.Item("U_NumCita")
                                    strCodTecnico = rowCitas.Item("U_Cod_Tecnico")
                                    If Not IsDBNull(rowCitas.Item("U_Num_Placa")) Then
                                        strPlaca = rowCitas.Item("U_Num_Placa")
                                    Else
                                        strPlaca = String.Empty
                                    End If

                                    intCantMinutos = (strHoraCita.Length - 1) - 1
                                    strMinutos = strHoraCita.Substring(intCantMinutos, 2)
                                    strHora = strHoraCita.Substring(0, intCantMinutos)
                                    intMinutos = Convert.ToInt32(strMinutos)

                                    strCita = String.Format("{0}-{1}", strSerieCita, strNumCita)

                                    If Not String.IsNullOrEmpty(strPlaca) Then
                                        strDescripcionCelda = String.Format("{0}/{1}", strPlaca, strCita)
                                    Else
                                        strDescripcionCelda = strCita
                                    End If

                                    intDuracion = ObtenerDuracionCita(strSerieCita, strNumCita, m_strCodSucursal, strCodTecnico)

                                    If intDuracion = 0 Or intDuracion < intIntervUnitario Then
                                        intDuracion = intIntervUnitario
                                    End If

                                    strIntervaloAct = String.Empty

                                    For Each element As String In listColumsGrid

                                        Dim result As String() = element.Split(New Char() {":"c})
                                        Dim horAgenda As String = result(0)
                                        Dim minAgenda As String = result(1)
                                        Dim valorColumna As String()
                                        Dim strDocEntry As String = ""

                                        If strHora.Equals(horAgenda) And strMinutos.Equals(minAgenda) Then

                                            For intI As Integer = intDuracion To 1 Step -15
                                                If intCont <= listColumsGrid.Count - 1 Then

                                                    valorColumna = listColumsGrid.ToArray()
                                                    strIntervaloAct = row.Item(valorColumna(intCont))

                                                    If Not String.IsNullOrEmpty(strIntervaloAct) AndAlso Not strIntervaloAct.Equals(".") Then

                                                        Dim strTest As String() = strIntervaloAct.Split(New Char() {"-"c})
                                                        If strTest(0).Trim() <> "n/a" Then

                                                            strDocEntry = Utilitarios.EjecutarConsulta(String.Format("select DocEntry from [@SCGD_CITA] where U_NumCita = '{0}' and U_Num_Serie = '{1}'", strTest(1).Trim(), strTest(0).Trim()), m_oCompany.CompanyDB, m_oCompany.Server)
                                                            Dim strDocEntryAct As String = rowCitas.Item("DocEntry")

                                                            If Not tableCitasProbl.ContainsKey(strDocEntry) And Not tableCitasProbl.ContainsKey(strDocEntryAct) Then
                                                                tableCitasProbl.Add(strDocEntryAct, strCita)
                                                            End If
                                                            Exit For
                                                        End If

                                                    End If
                                                    If row.Item(valorColumna(intCont)) <> "n/a" Then
                                                        row.Item(valorColumna(intCont)) = strDescripcionCelda
                                                        Dim strvalorCelda As String = valorColumna(intCont)
                                                        If m_strTipoAgendaColor.ToUpper = "Y" Then

                                                            If oGestionColor = GestionColor.EstadoCita Then
                                                                dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = MiColor.FromName(rowCitas.Item("ColorEstado").ToString)
                                                            Else
                                                                dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = MiColor.FromName(rowCitas.Item("Color").ToString)
                                                            End If
                                                        Else
                                                            dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.DarkSeaGreen
                                                        End If
                                                        intCont += 1

                                                    Else
                                                        intCont += 1
                                                        intI = intI + 15
                                                    End If
                                                End If
                                                'intCont += 1
                                            Next
                                        End If
                                        intCont += 1
                                    Next
                                End If
                            End If
                        Next
                        intCont = 0
                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub LlenarOcupacionPorGruposAlmuerzo(ByRef p_dtAgendas As System.Data.DataTable)
        Try

            Dim intDuracion As Integer
            Dim dtConfig As System.Data.DataTable

            Dim strMinutos As String
            Dim strHora As String
            Dim strHoraInicio As String
            Dim strHoraFin As String
            Dim strEstadoCancelado As String
            Dim strfecha As String
            Dim intCont As Integer = 0

            Dim strSQL As String

            m_dtFecha = dtpFecha.Value
            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

            strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            strSQL = "Select U_HorAlI, U_HoraAlF from [@SCGD_CONF_SUCURSAL] where U_Sucurs ='{0}'"
            dtConfig = Utilitarios.EjecutarConsultaDataTable(String.Format(strSQL, m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            If dtConfig.Rows.Count <> 0 Then
                If Not IsDBNull(dtConfig.Rows(0).Item("U_HorAlI")) AndAlso
                    Not IsDBNull(dtConfig.Rows(0).Item("U_HoraAlF")) Then

                    strHoraInicio = dtConfig.Rows(0).Item("U_HorAlI")
                    strHoraFin = dtConfig.Rows(0).Item("U_HoraAlF")

                    Dim dtInicio As Date = DateTime.ParseExact("19000101" & Utilitarios.FormatoHora2(strHoraInicio), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
                    Dim dtFin As Date = DateTime.ParseExact("19000101" & Utilitarios.FormatoHora2(strHoraFin), "yyyyMMddHHmm", CultureInfo.CurrentCulture)

                    intDuracion = DateDiff(DateInterval.Minute, dtInicio, dtFin)
                    Dim valorColumna As String()
                    valorColumna = listColumsGrid.ToArray()

                    If strHoraInicio.Length = 3 Then
                        strHora = strHoraInicio.Substring(0, 1)
                        strMinutos = strHoraInicio.Substring(1, 2)
                    ElseIf strHoraInicio.Length = 4 Then
                        strHora = strHoraInicio.Substring(0, 2)
                        strMinutos = strHoraInicio.Substring(2, 2)
                    End If

                    For Each row As DataRow In p_dtAgendas.Rows

                        Dim intPosition As Integer = p_dtAgendas.Rows.IndexOf(row)

                        If row.Item(mc_strRol).Equals("T") Then
                            intCont = 0
                            For Each element As String In listColumsGrid

                                Dim result As String() = element.Split(New Char() {":"c})
                                Dim horAgenda As String = result(0)
                                Dim minAgenda As String = result(1)

                                If strHora.Equals(horAgenda) And strMinutos.Equals(minAgenda) Then
                                    For intI As Integer = intDuracion To 1 Step -15

                                        Dim strvalorColum As String = valorColumna(intCont)
                                        Dim int As Integer = dgv_AgendaCitas.Rows.Count
                                        dgv_AgendaCitas.Rows(intPosition).Cells(strvalorColum).Style.BackColor = Color.DarkGray
                                        row.Item(valorColumna(intCont)) = "n/a"
                                        intCont += 1
                                    Next
                                    Exit For
                                End If
                                intCont += 1
                            Next
                        End If
                    Next
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    Public Function ConsultaEsCita(ByVal p_strValor As String) As Boolean
        Try
            Dim l_blnResutl As Boolean = False
            Dim strInicio As String

            If p_strValor <> "n/a" Then

                strInicio = p_strValor.Substring(0, 1)

                If IsNumeric(strInicio) Then
                    l_blnResutl = False
                Else
                    l_blnResutl = True
                End If
            End If



            Return l_blnResutl
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function


    Public Sub LlenarOcupacionPostPorAgenda()
        Try

            Dim dtCitas As System.Data.DataTable
            Dim dtConfig As System.Data.DataTable

            Dim strfecha As String
            Dim strIdAgenda As String
            Dim strIntervalo As String
            Dim strEstadoCancelado As String
            Dim strMinutosCita As String
            Dim strHoraCita As String
            Dim strHoraCitaFin As String
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim strCita As String
            Dim intDuracion As Integer
            Dim strBMensaje As StringBuilder = New StringBuilder()

            Dim l_strInicioTaller As String
            Dim l_strFinTaller As String

            Dim intPosMin As Integer
            Dim strHoraFin As String
            Dim strMinFin As String


            m_dtFecha = dtpFecha.Value

            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

            Dim l_strSQLConfig As String
            Dim l_strSQLCita As String

            Dim l_HoraInicio As Date = "1900-01-01 08:00"
            Dim l_HoraFin As Date = "1900-01-01 18:00"
            Dim l_HoraCita As Date
            Dim CodigoAgendaCita As String = String.Empty

            l_strSQLConfig = "Select U_CodCitaCancel ,U_HoraInicio, U_HoraFin  from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'"

            l_strSQLCita = " Select DocEntry, U_HoraCita, U_HoraCita_Fin, U_NumCita, U_Num_Serie, U_Cod_Unid, U_CardCode, U_CardName, U_Cod_Agenda " +
                            " from [@SCGD_CITA] " +
                            " where  U_FechaCita <> '{0}' and U_FhaCita_Fin = '{0}' and ( U_Estado <> '{1}' or U_Estado is null)"

            dtConfig = Utilitarios.EjecutarConsultaDataTable(String.Format(l_strSQLConfig, m_strCodSucursal))

            If dtConfig.Rows.Count <> 0 Then
                strEstadoCancelado = dtConfig.Rows(0).Item("U_CodCitaCancel")
                l_strInicioTaller = dtConfig.Rows(0).Item("U_HoraInicio")
                l_strFinTaller = dtConfig.Rows(0).Item("U_HoraFin")
            End If

            Dim intMinInicio As Integer
            Dim intHoraInicio As Integer
            Dim intMinFin As Integer
            Dim intHoraFin As Integer

            If (l_strInicioTaller = Nothing) Then
                Return
            End If

            If l_strInicioTaller.Length = 3 Then
                intHoraInicio = l_strInicioTaller.Substring(0, 1)
                intMinInicio = l_strInicioTaller.Substring(1, 2)
            ElseIf l_strInicioTaller.Length = 4 Then
                intHoraInicio = l_strInicioTaller.Substring(0, 1)
                intMinInicio = l_strInicioTaller.Substring(1)
            End If

            If l_strFinTaller.Length = 3 Then
                intHoraFin = l_strFinTaller.Substring(0, 1)
                intMinFin = l_strFinTaller.Substring(1, 2)
            ElseIf l_strFinTaller.Length = 4 Then
                intHoraFin = l_strFinTaller.Substring(0, 2)
                intMinFin = l_strFinTaller.Substring(2)
            End If

            l_HoraInicio = m_dtFecha.AddHours(intHoraInicio)
            l_HoraInicio = l_HoraInicio.AddMinutes(intMinInicio)

            l_HoraFin = m_dtFecha.AddHours(intHoraFin)
            l_HoraFin = l_HoraFin.AddMinutes(intMinFin)

            dtCitas = Utilitarios.EjecutarConsultaDataTable(String.Format(l_strSQLCita, strfecha, strEstadoCancelado))

            If dtCitas.Rows.Count > 0 Then
                For Each row As DataRow In dtAgenda.Rows
                    If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                        strIdAgenda = row.Item(mc_strIDAgenda)

                        strIntervalo = ObtenerIntervaloAgenda(strIdAgenda)

                        For Each rowCitas As DataRow In dtCitas.Rows
                            CodigoAgendaCita = rowCitas.Item("U_Cod_Agenda")
                            If strIdAgenda = CodigoAgendaCita Then
                                strBMensaje.Length = 0

                                If Not IsDBNull(rowCitas.Item("U_HoraCita_Fin")) And
                                    Not IsDBNull(rowCitas.Item("U_Num_Serie")) And
                                    Not IsDBNull(rowCitas.Item("U_NumCita")) Then
                                    '   l_FhaCitaFin.ToString("HH") & l_FhaCitaFin.ToString("mm")

                                    strHoraCitaFin = rowCitas.Item("U_HoraCita_Fin")
                                    strSerieCita = rowCitas.Item("U_Num_Serie")
                                    strNumCita = rowCitas.Item("U_NumCita")

                                    Dim strDura As String = ObtenerDuracionCita(strSerieCita, strNumCita, m_strCodSucursal, "")

                                    intPosMin = (strHoraCitaFin.Length - 2)
                                    strHoraFin = strHoraCitaFin.Substring(0, intPosMin)
                                    strMinFin = strHoraCitaFin.Substring(intPosMin, 2)

                                    strMinutosCita = l_HoraInicio.ToString("mm") ' strHoraCitaFin.Substring(intCantMinutos, 2)
                                    strHoraCita = l_HoraInicio.ToString("HH") 'strHoraCitaFin.Substring(0, intCantMinutos)
                                    strHoraCita = strHoraCita.TrimStart("0")

                                    strCita = strBMensaje.Append(strSerieCita).Append("-").Append(strNumCita).ToString()

                                    l_HoraCita = m_dtFecha
                                    l_HoraCita = l_HoraCita.AddHours(strHoraFin)
                                    l_HoraCita = l_HoraCita.AddMinutes(strMinFin)

                                    intDuracion = DateDiff(DateInterval.Minute, l_HoraInicio, l_HoraCita)

                                    If intDuracion = 0 Then
                                        intDuracion = 15
                                    End If

                                    Dim intCont As Integer = 0

                                    For Each element As String In listColumsGrid

                                        Dim result As String() = element.Split(New Char() {":"c})
                                        Dim horAgenda As String = result(0)
                                        Dim minAgenda As String = result(1)
                                        Dim strIntervaloAct As String = ""

                                        Dim strDocEntry As String = ""

                                        If strHoraCita.Equals(horAgenda) And strMinutosCita.Equals(minAgenda) Then

                                            For intI As Integer = intDuracion To 1 Step -15
                                                Dim valorColumna As String() = listColumsGrid.ToArray()
                                                strIntervaloAct = row.Item(valorColumna(intCont))

                                                If row.Item(valorColumna(intCont)) <> "n/a" Then
                                                    row.Item(valorColumna(intCont)) = strBMensaje
                                                    intCont = intCont + 1
                                                Else
                                                    intI = intI + 15
                                                    intCont = intCont + 1
                                                End If

                                            Next
                                        End If
                                        intCont = intCont + 1
                                    Next
                                End If
                            End If
                        Next

                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub LlenarOcupacionPostPorGrupos_Tecnico()
        Try

            Dim dtServicios As System.Data.DataTable
            Dim dtConfig As System.Data.DataTable

            Dim strfecha As String
            Dim strEstadoCancelado As String
            Dim strMinutosCita As String
            Dim strHoraCita As String
            Dim strHoraServFin As String
            Dim strFechaServFin As String
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim strCita As String
            Dim strNoOT As String
            Dim intDuracion As Integer

            Dim l_strInicioTaller As String
            Dim l_strFinTaller As String

            Dim intPosMin As Integer
            Dim strHoraFin As String
            Dim strMinFin As String
            Dim strIdEmp As String
            Dim dtFhaServ As Date
            Dim strIntervaloAct As String
            m_dtFecha = dtpFecha.Value

            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

            Dim l_strSQLConfig As String
            Dim l_strSQLServ As String

            Dim l_HoraInicio As Date = "1900-01-01 08:00"
            Dim l_HoraFin As Date = "1900-01-01 18:00"
            Dim l_HoraCita As Date
            Dim strPlaca As String = String.Empty
            Dim strDescripcionCelda As String = String.Empty
            Dim MecanicoAgenda As String = String.Empty
            Dim MecanicoServicio As String = String.Empty

            l_strSQLConfig = "Select U_CodCitaCancel ,U_HoraInicio, U_HoraFin  from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}'"


            l_strSQLServ = " Select CI.DocEntry, CI.U_HoraServ, CI.U_HoraServ_Fin, CI.U_NumCita, CI.U_Num_Serie, CI.U_Cod_Unid, CI.U_Num_Placa, CI.U_CardCode, CI.U_CardName , CI.U_FhaServ, CI.U_FhaServ_Fin, QU.U_SCGD_Numero_OT, ISNULL(T0.U_Color, 'Aqua') AS U_Color, CASE WHEN QU.U_SCGD_Numero_OT IS NOT NULL THEN ISNULL(T1.U_ColorOT, 'Aqua') ELSE ISNULL(T1.U_Color, 'Aqua') END AS 'ColorEstado', CI.U_Cod_Tecnico " +
                                " from [@SCGD_CITA] CI with(nolock) " +
                                " INNER JOIN OQUT QU with(nolock) ON QU.DocEntry = CI.U_Num_Cot " +
                                "LEFT JOIN [@SCGD_COLORESAGENDA] T0 ON T0.U_RazonCita = CI.U_Cod_Razon " +
                                "LEFT JOIN [@SCGD_CITA_ESTADOS] T1 ON T1.Code = CI.U_Estado " +
                                " where CI.U_FhaServ < '{0}' AND CI.U_FhaServ_Fin >= '{0}'  AND ( CI.U_Estado <>  '{1}' or CI.U_Estado is null ) AND (( QU.U_SCGD_Numero_OT is null or QU.U_SCGD_Numero_OT = '') or (QU.U_SCGD_Numero_OT is not null or QU.U_SCGD_Numero_OT <> '' and QU.U_SCGD_Estado_CotID = 1)) "

            dtConfig = Utilitarios.EjecutarConsultaDataTable(String.Format(l_strSQLConfig, m_strCodSucursal))

            If dtConfig.Rows.Count <> 0 Then
                strEstadoCancelado = dtConfig.Rows(0).Item("U_CodCitaCancel")
                l_strInicioTaller = dtConfig.Rows(0).Item("U_HoraInicio")
                l_strFinTaller = dtConfig.Rows(0).Item("U_HoraFin")
            End If


            Dim intMinInicio As Integer
            Dim intHoraInicio As Integer
            Dim intMinFin As Integer
            Dim intHoraFin As Integer

            If l_strInicioTaller.Length = 3 Then
                intHoraInicio = l_strInicioTaller.Substring(0, 1)
                intMinInicio = l_strInicioTaller.Substring(1, 2)
            ElseIf l_strInicioTaller.Length = 4 Then
                intHoraInicio = l_strInicioTaller.Substring(0, 1)
                intMinInicio = l_strInicioTaller.Substring(1)
            End If

            If l_strFinTaller.Length = 3 Then
                intHoraFin = l_strFinTaller.Substring(0, 1)
                intMinFin = l_strFinTaller.Substring(1, 2)
            ElseIf l_strFinTaller.Length = 4 Then
                intHoraFin = l_strFinTaller.Substring(0, 2)
                intMinFin = l_strFinTaller.Substring(2)
            End If

            l_HoraInicio = m_dtFecha.AddHours(intHoraInicio)
            l_HoraInicio = l_HoraInicio.AddMinutes(intMinInicio)

            l_HoraFin = m_dtFecha.AddHours(intHoraFin)
            l_HoraFin = l_HoraFin.AddMinutes(intMinFin)

            dtServicios = Utilitarios.EjecutarConsultaDataTable(String.Format(l_strSQLServ, strfecha, strEstadoCancelado))
            If dtServicios.Rows.Count > 0 Then
                For Each row As DataRow In dtAgenda.Rows

                    If Not IsDBNull(row.Item(mc_strID)) Then
                        Dim intposition As Integer = dtAgenda.Rows.IndexOf(row)
                        MecanicoAgenda = row.Item(mc_strID)

                        For Each rowServicio As DataRow In dtServicios.Rows

                            MecanicoServicio = rowServicio.Item("U_Cod_Tecnico")

                            If MecanicoAgenda = MecanicoServicio Then

                                If Not IsDBNull(rowServicio.Item("U_HoraServ_Fin")) And
                                Not IsDBNull(rowServicio.Item("U_Num_Serie")) And
                                Not IsDBNull(rowServicio.Item("U_NumCita")) Then

                                    strHoraServFin = rowServicio.Item("U_HoraServ_Fin")
                                    strFechaServFin = rowServicio.Item("U_FhaServ_Fin")
                                    strSerieCita = rowServicio.Item("U_Num_Serie")
                                    strNumCita = rowServicio.Item("U_NumCita")
                                    strNoOT = rowServicio.Item("U_SCGD_Numero_OT")

                                    If Not IsDBNull(rowServicio.Item("U_Num_Placa")) Then
                                        strPlaca = rowServicio.Item("U_Num_Placa")
                                    Else
                                        strPlaca = String.Empty
                                    End If

                                    intPosMin = (strHoraServFin.Length - 2)
                                    strHoraFin = strHoraServFin.Substring(0, intPosMin)
                                    strMinFin = strHoraServFin.Substring(intPosMin, 2)

                                    strMinutosCita = l_HoraInicio.ToString("mm")
                                    strHoraCita = l_HoraInicio.ToString("HH")
                                    strHoraCita = strHoraCita.TrimStart("0")

                                    strCita = String.Format("{0}-{1}", strSerieCita, strNumCita)

                                    If Not String.IsNullOrEmpty(strPlaca) Then
                                        strDescripcionCelda = String.Format("{0}/{1}", strPlaca, strCita)
                                    Else
                                        strDescripcionCelda = strCita
                                    End If

                                    l_HoraCita = m_dtFecha
                                    l_HoraCita = l_HoraCita.AddHours(strHoraFin)
                                    l_HoraCita = l_HoraCita.AddMinutes(strMinFin)


                                    dtFhaServ = FormatDateTime(strFechaServFin, DateFormat.GeneralDate)

                                    intDuracion = DateDiff(DateInterval.Minute, l_HoraInicio, l_HoraCita)

                                    If intDuracion = 0 Then
                                        intDuracion = 15
                                    End If

                                    Dim intCont As Integer = 0

                                    If dtFhaServ > m_dtFecha Then

                                        If EsDiaLaboral(m_strCodSucursal, m_dtFecha) Then
                                            Dim valorColumna As String() = listColumsGrid.ToArray()
                                            strIntervaloAct = row.Item(valorColumna(intCont))
                                            For i As Integer = 0 To listColumsGrid.Count - 1
                                                If intCont <> listColumsGrid.Count Then
                                                    If row.Item(valorColumna(i)) <> "n/a" AndAlso String.IsNullOrEmpty(strIntervaloAct) AndAlso Not strIntervaloAct.Equals(".") Then
                                                        row.Item(valorColumna(i)) = strDescripcionCelda
                                                        Dim strvalorCelda As String = valorColumna(intCont)
                                                        If String.IsNullOrEmpty(strNoOT) Then
                                                            If strUsaColorAgenda = "Y" Then
                                                                If oGestionColor = GestionColor.EstadoCita Then
                                                                    dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.FromName(rowServicio.Item("ColorEstado"))
                                                                Else
                                                                    dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.FromName(rowServicio.Item("U_Color"))
                                                                End If
                                                            Else
                                                                dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.Aqua
                                                            End If
                                                        Else
                                                            dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.CornflowerBlue
                                                            row.Item(valorColumna(i)) = strNoOT
                                                        End If

                                                        intCont = intCont + 1
                                                    Else
                                                        intCont = intCont + 1
                                                    End If
                                                End If
                                            Next
                                        End If
                                    Else
                                        For Each element As String In listColumsGrid

                                            Dim result As String() = element.Split(New Char() {":"c})
                                            Dim horAgenda As String = result(0)
                                            Dim minAgenda As String = result(1)

                                            If strHoraCita.Equals(horAgenda) And strMinutosCita.Equals(minAgenda) Then

                                                For intI As Integer = intDuracion To 1 Step -15
                                                    If intCont <> listColumsGrid.Count Then
                                                        Dim valorColumna As String() = listColumsGrid.ToArray()
                                                        strIntervaloAct = row.Item(valorColumna(intCont))
                                                        If intCont < valorColumna.Length Then

                                                            If row.Item(valorColumna(intCont)) <> "n/a" AndAlso String.IsNullOrEmpty(strIntervaloAct) AndAlso Not strIntervaloAct.Equals(".") Then
                                                                row.Item(valorColumna(intCont)) = strDescripcionCelda
                                                                Dim strvalorCelda As String = valorColumna(intCont)
                                                                If String.IsNullOrEmpty(strNoOT) Then
                                                                    If strUsaColorAgenda = "Y" Then
                                                                        If oGestionColor = GestionColor.EstadoCita Then
                                                                            dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.FromName(rowServicio.Item("ColorEstado"))
                                                                        Else
                                                                            dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.FromName(rowServicio.Item("U_Color"))
                                                                        End If
                                                                    Else
                                                                        dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.Aqua
                                                                    End If
                                                                Else
                                                                    dgv_AgendaCitas.Rows(intposition).Cells(strvalorCelda).Style.BackColor = Color.CornflowerBlue
                                                                    If String.IsNullOrEmpty(strPlaca) Then
                                                                        row.Item(valorColumna(intCont)) = strNoOT
                                                                    Else
                                                                        row.Item(valorColumna(intCont)) = String.Format("{0}/{1}", strPlaca, strNoOT)
                                                                    End If
                                                                End If
                                                                intCont = intCont + 1
                                                            Else
                                                                intI = intI + 15
                                                                intCont = intCont + 1
                                                            End If

                                                        End If

                                                    End If
                                                Next
                                            End If
                                            intCont = intCont + 1
                                        Next
                                    End If

                                End If
                            End If
                        Next
                    End If

                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ActualizaTextoFecha()
        Try
            lblFechaAct.Text = dtpFecha.Value
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try

    End Sub

    Public Function ObtenerIntervaloAgenda(ByVal p_idAgenda As String) As Integer
        Try
            Dim l_intIntervalo As Integer = 0
            Dim l_strSQLInt As String = "SELECT U_IntervaloCitas from [@SCGD_AGENDA] where DocEntry = '{0}'"
            Dim l_strntervalo As String = Utilitarios.EjecutarConsulta(String.Format(l_strSQLInt, p_idAgenda), m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(l_strntervalo) Then
                l_intIntervalo = Integer.Parse(l_strntervalo)
                If l_intIntervalo <= 0 Then
                    l_intIntervalo = 15
                Else
                    l_intIntervalo = Integer.Parse(l_strntervalo)
                End If
            End If

            Return l_intIntervalo

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Function

    Public Function ObtenerDuracionCita(ByVal p_strSerie As String, ByVal p_strNumCita As String, ByVal p_strSucur As String, ByVal p_strIDEmpleado As String) As Integer
        Try
            Dim intDuracion As Integer = 0
            Dim intDuracionEstadar As Integer = 0
            Dim intDuracionServicioRapido As Integer = 0
            Dim intDuracionTiempoOtorgado As Integer = 0
            Dim strDuracionCita As String
            Dim strTiempoOtorgado As String
            Dim strTiempoEmpl As String
            Dim strEmpAsiTiempoOtor As String = " and Q.U_SCGD_EmpAsig = '{0}' "


            Dim strSQL As String = "select SUM(I.U_SCGD_Duracion * q1.Quantity) " +
                                    " from [@SCGD_CITA] as C with (nolock) " +
                                    " inner join OQUT as Q with (nolock) on C.U_Num_Cot = Q.DocEntry" +
                                    " inner join QUT1 as Q1 with (nolock) on Q.DocEntry = Q1.DocEntry " +
                                    " inner join OITM as I with (nolock) on Q1.ItemCode = I.ItemCode " +
                                    " and Q1.U_SCGD_Aprobado in (1,4) " +
                                    " and I.U_SCGD_TipoArticulo = 2 " +
                                    " where Q.U_SCGD_NoSerieCita = '{0}' and Q.U_SCGD_NoCita = '{1}' and C.U_Cod_Sucursal = '{2}'"

            Dim strSQLEmpleado As String = "Select U_SCGD_TiempServ from OHEM with (nolock) where empID = '{0}'"

            Dim strSQLTOtor As String = "  Select U_SCGD_TiOtor as TiempoOtorgado from QUT1 as Q with(nolock) " &
                                        " inner join OQUT as OQ with(nolock) on q.DocEntry = oq.DocEntry  " &
                                        " where OQ.U_SCGD_NoSerieCita = '{0}' and OQ.U_SCGD_NoCita = '{1}' and OQ.U_SCGD_idSucursal  = '{2}' and Q.U_SCGD_TiOtor != 0  "

            strSQL = String.Format(strSQL, p_strSerie, p_strNumCita, p_strSucur)
            strSQLTOtor = String.Format(strSQLTOtor, p_strSerie, p_strNumCita, p_strSucur)


            If Not String.IsNullOrEmpty(p_strIDEmpleado) Then
                strSQLEmpleado = String.Format(strSQLEmpleado, p_strIDEmpleado)
                strTiempoEmpl = Utilitarios.EjecutarConsulta(strSQLEmpleado, m_oCompany.CompanyDB, m_oCompany.Server)
                strSQL = strSQL & String.Format(" and Q1.U_SCGD_EmpAsig = '{0}' ", p_strIDEmpleado)

                strSQLTOtor = strSQLTOtor & String.Format(strEmpAsiTiempoOtor, p_strIDEmpleado)
                strTiempoOtorgado = Utilitarios.EjecutarConsulta(strSQLTOtor, m_oCompany.CompanyDB, m_oCompany.Server)
                strDuracionCita = Utilitarios.EjecutarConsulta(strSQL, m_oCompany.CompanyDB, m_oCompany.Server)
            Else
                strTiempoEmpl = String.Empty
                strTiempoOtorgado = Utilitarios.EjecutarConsulta(strSQLTOtor, m_oCompany.CompanyDB, m_oCompany.Server)
                strDuracionCita = Utilitarios.EjecutarConsulta(strSQL, m_oCompany.CompanyDB, m_oCompany.Server)
            End If


            If Not String.IsNullOrEmpty(strTiempoOtorgado) Or (strTiempoOtorgado = "0" And strTiempoOtorgado = "") Then
                intDuracionTiempoOtorgado = Integer.Parse(strTiempoOtorgado)
            End If

            If Not String.IsNullOrEmpty(strTiempoEmpl) Then
                intDuracionServicioRapido = Integer.Parse(strTiempoEmpl)
                If intDuracionTiempoOtorgado <> 0 Then
                    intDuracion = intDuracionServicioRapido + intDuracionTiempoOtorgado
                Else
                    intDuracion = intDuracionServicioRapido
                End If

            ElseIf Not String.IsNullOrEmpty(strDuracionCita) Then

                intDuracionEstadar = Decimal.Parse(strDuracionCita)
                If intDuracionTiempoOtorgado <> 0 Then
                    intDuracion = intDuracionEstadar + intDuracionTiempoOtorgado
                Else
                    intDuracion = intDuracionEstadar
                End If

            End If

            Return intDuracion

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Function

    Private Function ObtenerCantidadEspaciosAgenda(ByVal p_strDuracion As String) As Integer
        Try
            Dim l_intResult As Integer
            Dim l_intTmp As Decimal
            Dim l_intDuracion As Integer

            If String.IsNullOrEmpty(p_strDuracion) Then
                Return 1
            Else
                l_intDuracion = CInt(p_strDuracion)
                'l_intTmp = l_intDuracion / 15

                If (l_intDuracion Mod 15) <> 0 Then
                    l_intResult = (Math.Truncate(l_intDuracion / 15)) + 1
                Else
                    l_intResult = (Math.Truncate(l_intDuracion / 15))
                End If
            End If

            Return l_intResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene la duración de una actividad
    ''' </summary>
    ''' <param name="p_strDuracion">Duración estándar en formato texto</param>
    ''' <param name="p_strTiempoOtorgado">Tiempo otorgado adicional en formato texto</param>
    ''' <param name="p_strServicioRapido">Duración servicio rápido en formato texto</param>
    ''' <returns>Duración total de la actividad</returns>
    ''' <remarks></remarks>
    Public Function CalcularDuracionActividad(ByVal p_strDuracionEstandar As String, ByVal p_strTiempoOtorgado As String, ByVal p_strServicioRapido As String, Optional ByVal p_strCantidad As String = "1") As Integer

        Dim intDuracionEstandar As Integer = 0
        Dim intTiempoOtorgado As Integer = 0
        Dim intServicioRapido As Integer = 0
        Dim decCantidad As Decimal = 1

        'La duración mínima de cualquier actividad es de 15 minutos
        Dim intDuracionTotal As Integer = 15

        Try

            Decimal.TryParse(p_strCantidad, decCantidad)
            Integer.TryParse(p_strDuracionEstandar, intDuracionEstandar)
            Integer.TryParse(p_strTiempoOtorgado, intTiempoOtorgado)
            Integer.TryParse(p_strServicioRapido, intServicioRapido)

            intDuracionTotal = CalcularDuracionActividad(intDuracionEstandar, intTiempoOtorgado, intServicioRapido, decCantidad)

            Return intDuracionTotal

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try

    End Function

    ''' <summary>
    ''' Obtiene la duración de una actividad
    ''' </summary>
    ''' <param name="p_intDuracion">Duración estándar en formato entero</param>
    ''' <param name="p_intTiempoOtorgado">Tiempo otorgado adicional en formato entero</param>
    ''' <param name="p_intServicioRapido">Duración servicio rápido en formato entero</param>
    ''' <returns>Duración total de la actividad</returns>
    ''' <remarks></remarks>
    Public Function CalcularDuracionActividad(ByVal p_intDuracionEstandar As Integer, ByVal p_intTiempoOtorgado As Integer, ByVal p_intServicioRapido As Integer, Optional ByVal p_decCantidad As Decimal = 1) As Integer

        'La duración mínima de cualquier actividad es de 15 minutos
        Dim intDuracionTotal As Integer = 15

        Try
            'Orden de prioridades para calcular la duración de una actividad
            '1-Servicio rápido
            '2-Duración estándar

            'Determinamos si se utiliza el servicio rápido o el tiempo estándar
            If p_intServicioRapido > 0 Then
                'Prioridad 1 Servicio rápido
                intDuracionTotal = p_intServicioRapido + p_intTiempoOtorgado
            Else
                'Prioridad 2 Duración estándar
                intDuracionTotal = p_intDuracionEstandar + p_intTiempoOtorgado
            End If

            'El tiempo total se calcula multiplicando por la cantidad de servicios
            'es decir si una actividad tarda 60 minutos, pero a nivel de línea en el campo cantidad se digitaron 2 unidades, el tiempo total será de 120 minutos.
            intDuracionTotal = intDuracionTotal * p_decCantidad

            'La duración mínima para cualquier actividad es de 15 minutos
            If intDuracionTotal < 15 Then
                intDuracionTotal = 15
            End If

            Return intDuracionTotal

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try

    End Function

    Public Function ObtenerDuracionOrden(ByVal p_strNoOrden As String, ByVal p_strCodTecnico As String, ByVal p_strCodSucursal As String) As Integer
        Try

            Dim intDuracion As Integer = 0
            Dim intDuracionEstadar As Integer = 0
            Dim intDuracionServicioRapido As Integer = 0
            Dim intDuracionTiempoOtorgado As Integer = 0
            Dim strDuracion As String
            Dim strTiempoEmpl As String
            Dim strTiempoOtorgado As String
            Dim strEmpAsiTiempoOtor As String = " and Q.U_SCGD_EmpAsig = '{0}' "


            Dim strSQL As String = " Select  SUM(I.U_SCGD_Duracion * Q1.Quantity) " +
                                    " from OQUT QU " +
                                    " INNER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry " +
                                    " inner join OITM as I with (nolock) on Q1.ItemCode = I.ItemCode  " +
                                    " WHERE" +
                                    " Q1.U_SCGD_Aprobado in (1,4)   " +
                                    " AND I.U_SCGD_TipoArticulo = 2 " +
                                    " AND q1.U_SCGD_NoOT = '{0}' " +
                                    " AND Q1.U_SCGD_EmpAsig = '{1}' "

            Dim strSQLEmpleado As String = "Select U_SCGD_TiempServ from OHEM with (nolock) where empID = '{0}'"

            Dim strSQLTOtor As String = "  Select U_SCGD_TiOtor as TiempoOtorgado from QUT1 as Q with(nolock) " &
                                    " inner join OQUT as OQ with(nolock) on q.DocEntry = oq.DocEntry  " &
                                    " where Q.U_SCGD_NoOT = '{0}' and OQ.U_SCGD_idSucursal  = '{1}' and Q.U_SCGD_TiOtor != 0  "


            strSQL = String.Format(strSQL, p_strNoOrden, p_strCodTecnico)
            strSQLTOtor = String.Format(strSQLTOtor, p_strNoOrden, p_strCodSucursal)

            If Not String.IsNullOrEmpty(p_strCodTecnico) Then
                strSQLEmpleado = String.Format(strSQLEmpleado, p_strCodTecnico)
                strTiempoEmpl = Utilitarios.EjecutarConsulta(strSQLEmpleado, m_oCompany.CompanyDB, m_oCompany.Server)
                strSQLTOtor = strSQLTOtor & String.Format(strEmpAsiTiempoOtor, p_strCodTecnico)
                strTiempoOtorgado = Utilitarios.EjecutarConsulta(strSQLTOtor, m_oCompany.CompanyDB, m_oCompany.Server)

                If Not String.IsNullOrEmpty(strTiempoOtorgado) Or (strTiempoOtorgado = "0" And strTiempoOtorgado = "") Then
                    intDuracionTiempoOtorgado = Integer.Parse(strTiempoOtorgado)
                End If

            Else
                strTiempoEmpl = String.Empty
                strTiempoOtorgado = Utilitarios.EjecutarConsulta(strSQLTOtor, m_oCompany.CompanyDB, m_oCompany.Server)

                If Not String.IsNullOrEmpty(strTiempoOtorgado) Or (strTiempoOtorgado = "0" And strTiempoOtorgado = "") Then
                    intDuracionTiempoOtorgado = Integer.Parse(strTiempoOtorgado)
                End If

            End If

            strDuracion = Utilitarios.EjecutarConsulta(strSQL, m_oCompany.CompanyDB, m_oCompany.Server)



            If Not String.IsNullOrEmpty(strTiempoEmpl) Then
                intDuracionServicioRapido = Integer.Parse(strTiempoEmpl)

                If intDuracionTiempoOtorgado <> 0 Then
                    intDuracion = intDuracionServicioRapido + intDuracionTiempoOtorgado
                Else
                    intDuracion = intDuracionServicioRapido
                End If
            ElseIf Not String.IsNullOrEmpty(strDuracion) Then
                intDuracionEstadar = Decimal.Parse(strDuracion)
                If intDuracionTiempoOtorgado <> 0 Then
                    intDuracion = intDuracionEstadar + intDuracionTiempoOtorgado
                Else
                    intDuracion = intDuracionEstadar
                End If
            End If


            Return intDuracion

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Function

    Public Function obtenerCodigoCotizacion(ByVal p_strSerie As String, ByVal p_strNumCita As String, ByVal p_strSucursal As String) As String
        Try

            Dim l_strCodigo As String
            Dim l_strSQL As String = "Select U_Num_Cot from [@SCGD_CITA] Where U_Num_Serie = '{0}' and U_NumCita = '{1}' and U_Cod_Sucursal = '{2}'"

            l_strSQL = String.Format(l_strSQL, p_strSerie, p_strNumCita, p_strSucursal)

            l_strCodigo = Utilitarios.EjecutarConsulta(l_strSQL, m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(l_strCodigo) Then
                Return l_strCodigo
            Else
                Return String.Empty
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try

    End Function

    Private Function ObtenerNombreDBTaller(ByVal p_strSucursal As String) As String
        Try

            Dim l_strResult As String = String.Empty
            Dim l_strSQL As String = "SELECT U_BDSucursal FROM  [@SCGD_SUCURSALES] with (nolock) where Code = '{0}'"

            l_strResult = Utilitarios.EjecutarConsulta(String.Format(l_strSQL, m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            Return l_strResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Function

    Private Function ObtenerConfiguracionTallerInterno() As String
        Try
            If Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO) Then
                Return "Y"
            Else
                Return "N"
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Function

    Public Sub LlenarReservacion()
        Try

            Dim dtSuspension As System.Data.DataTable
            Dim strfecha As String
            Dim strIdAgenda As String
            Dim strIntervalo As String
            Dim strConsulta As String
            Dim strUsaDurE As String
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim intDuracion As Integer
            Dim strBMensaje As StringBuilder = New StringBuilder()

            Dim l_strSQLSuspension As String

            Dim l_horaReservaInicio As Date
            Dim l_horaReservaFin As Date
            Dim l_horaCont As Date
            Dim l_FhaReservaInicio As Date
            Dim l_FhaReservaFin As Date
            Dim l_numDocEntry As Integer
            Dim l_strReserv As String
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String
            Dim l_strHoraAgenda As String
            Dim l_horaFinAgenda As Date
            Dim l_horaInicioAgenda As Date


            m_dtFecha = dtpFecha.Value

            strfecha = Utilitarios.RetornaFechaFormatoDB(m_dtFecha, m_oCompany.Server)

            l_horaInicioAgenda = FormatDateTime(m_fhaHoraInicio, DateFormat.ShortTime)
            l_horaFinAgenda = FormatDateTime(m_fhaHoraFin, DateFormat.ShortTime)

            'strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            For Each row As DataRow In dtAgenda.Rows

                If Not IsDBNull(row.Item(mc_strIDAgenda)) Then
                    strIdAgenda = row.Item(mc_strIDAgenda)

                    l_strSQLSuspension = "SELECT DocEntry, U_Cod_Sucur,U_Cod_Agenda,U_Fha_Desde,U_Hora_Desde,U_Fha_Hasta,U_Hora_Hasta,U_Estado,U_Observ  " & _
                                           " FROM [@SCGD_AGENDA_SUSP] " & _
                                           " WHERE U_Fha_Desde = '{0}' AND U_Cod_Sucur = '{1}' AND U_Cod_Agenda = '{2}' AND U_Estado = 'Y' "

                    l_strSQLSuspension = String.Format(l_strSQLSuspension, strfecha, m_strCodSucursal, strIdAgenda)

                    dtSuspension = Utilitarios.EjecutarConsultaDataTable(l_strSQLSuspension)

                    'If strUsaDurE.Equals("N") Or String.IsNullOrEmpty(strUsaDurE) Then

                    '  strIntervalo = Utilitarios.EjecutarConsulta(String.Format("Select U_IntervaloCitas from [@SCGD_AGENDA] where DocEntry = '{0}'", strIdAgenda), m_oCompany.CompanyDB, m_oCompany.Server)

                    'strIntervalo = ObtenerIntervalorAgenda(strIdAgenda)
                    'intDuracion = Convert.ToInt32(strIntervalo)

                    'If String.IsNullOrEmpty(strIntervalo) Then
                    '    intDuracion = 15
                    'Else
                    '    intDuracion = Convert.ToInt32(strIntervalo)
                    'End If

                    For Each rowSusp As DataRow In dtSuspension.Rows

                        strBMensaje.Length = 0

                        If Not IsDBNull(rowSusp.Item("U_Fha_Desde")) And Not IsDBNull(rowSusp.Item("U_Fha_Hasta")) Then
                            l_numDocEntry = rowSusp.Item("DocEntry")

                            l_strHoraDesde = Utilitarios.FormatoHora(rowSusp.Item("U_Hora_Desde"))
                            l_strHoraHasta = Utilitarios.FormatoHora(rowSusp.Item("U_Hora_Hasta"))

                            l_horaReservaInicio = FormatDateTime(l_strHoraDesde, DateFormat.ShortTime)
                            l_horaReservaFin = FormatDateTime(l_strHoraHasta, DateFormat.ShortTime)

                            l_FhaReservaInicio = DateTime.Parse(rowSusp.Item("U_Fha_Desde"))
                            l_FhaReservaFin = DateTime.Parse(rowSusp.Item("U_Fha_Hasta"))

                            l_strReserv = "n/a"


                            Dim intCont As Integer = 0

                            If l_horaFinAgenda < l_horaReservaFin Then
                                l_horaReservaFin = l_horaFinAgenda
                            End If


                            For Each element As String In listColumsGrid
                                l_strHoraAgenda = element

                                If l_strHoraAgenda.Equals(l_strHoraDesde.TrimStart("0″")) Then
                                    l_horaCont = l_horaReservaInicio

                                    While l_horaCont < l_horaReservaFin
                                        Dim valorColumna As String() = listColumsGrid.ToArray()

                                        row.Item(valorColumna(intCont)) = l_strReserv
                                        intCont = intCont + 1
                                        l_horaCont = l_horaCont.AddMinutes(15)

                                    End While
                                Else
                                    intCont = intCont + 1
                                End If


                            Next
                            ' End If
                        End If
                    Next
                End If


            Next

        Catch ex As Exception

        End Try
    End Sub

    Public Sub LlenarBloqueodeMecanicos()
        Try

            Dim dtMecBloq As System.Data.DataTable
            Dim strfecha As String
            Dim strIdMecAgenda As String
            Dim strIdMecBloc As String
            Dim strIntervalo As String
            Dim strConsulta As String
            Dim strUsaDurE As String
            Dim strSerieCita As String
            Dim strNumCita As String
            Dim intDuracion As Integer
            Dim strBMensaje As StringBuilder = New StringBuilder()

            Dim l_strSQLSuspension As String

            Dim l_horaInicio As Date
            Dim l_horaFin As Date
            Dim l_horaCont As Date
            Dim l_FhaInicio As Date
            Dim l_FhaFin As Date
            Dim l_numDocEntry As Integer
            Dim l_strBloq As String
            Dim l_strHoraDesde As String
            Dim l_strHoraHasta As String
            Dim l_strHoraAgenda As String
            Dim l_horaFinAgenda As Date
            Dim l_horaInicioAgenda As Date
            Dim strname As String = String.Empty



            m_dtFecha = dtpFecha.Value

            strfecha = Utilitarios.RetornaFechaFormatoDB(Convert.ToDateTime(m_dtFecha), m_oCompany.Server)

            l_horaInicioAgenda = FormatDateTime(m_fhaHoraInicio, DateFormat.ShortTime)
            l_horaFinAgenda = FormatDateTime(m_fhaHoraFin, DateFormat.ShortTime)

            l_strSQLSuspension = " Select  BM.DocEntry,BM.U_idMec,BM.U_FechI,LBM.U_FechCon,BM.U_HorI,BM.U_FechF,BM.U_HoraF,BM.U_Observ from [dbo].[@SCGD_BLOCMEC] as BM with(nolock) " & _
                                             " inner join  [dbo].[@SCGD_LINEAS_BLOME] as LBM on BM.DocEntry = LBM.DocEntry " & _
                                             " where  LBM.U_FechCon = '{0}' and BM.U_IdSucu = '{1}'"

            l_strSQLSuspension = String.Format(l_strSQLSuspension, strfecha, m_strCodSucursal)

            dtMecBloq = Utilitarios.EjecutarConsultaDataTable(l_strSQLSuspension)

            'strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            For Each row As DataRow In dtAgenda.Rows

                If Not IsDBNull(row.Item(mc_strIDAgenda)) Then

                    If row.Item(mc_strIDAgenda) = "0" Then
                        Dim intPosition As Integer = dtAgenda.Rows.IndexOf(row)
                        strname = dtAgenda.Columns(intPosition).ColumnName

                        strIdMecAgenda = dtAgenda.Rows(intPosition)("ID").ToString()

                        For Each rowBloc As DataRow In dtMecBloq.Rows

                            Dim intPositionBloc As Integer = dtMecBloq.Rows.IndexOf(rowBloc)
                            ''strname = dtMecBloq.Columns(intPosition).ColumnName
                            strBMensaje.Length = 0

                            strIdMecBloc = dtMecBloq.Rows(intPositionBloc)("U_idMec").ToString()

                            If strIdMecBloc = strIdMecAgenda Then

                                If Not IsDBNull(rowBloc.Item("U_FechF")) And Not IsDBNull(rowBloc.Item("U_FechCon")) Then
                                    l_numDocEntry = rowBloc.Item("DocEntry")

                                    l_strHoraDesde = Utilitarios.FormatoHora(rowBloc.Item("U_HorI"))
                                    l_strHoraHasta = Utilitarios.FormatoHora(rowBloc.Item("U_HoraF"))

                                    l_horaInicio = FormatDateTime(l_strHoraDesde, DateFormat.ShortTime)
                                    l_horaFin = FormatDateTime(l_strHoraHasta, DateFormat.ShortTime)

                                    l_FhaInicio = DateTime.Parse(rowBloc.Item("U_FechCon"))
                                    l_FhaFin = DateTime.Parse(rowBloc.Item("U_FechF"))

                                    l_strBloq = "n/a"


                                    Dim intCont As Integer = 0

                                    If l_horaFinAgenda < l_horaFin Then
                                        l_horaFin = l_horaFinAgenda
                                    End If


                                    For Each element As String In listColumsGrid
                                        l_strHoraAgenda = element

                                        If l_strHoraAgenda.Equals(l_strHoraDesde.TrimStart("0″")) Then
                                            l_horaCont = l_horaInicio

                                            While l_horaCont <= l_horaFin
                                                Dim valorColumna As String() = listColumsGrid.ToArray()

                                                'Valida que no se salga del indice
                                                If intCont < valorColumna.Length Then
                                                    row.Item(valorColumna(intCont)) = l_strBloq
                                                    intCont = intCont + 1
                                                End If

                                                l_horaCont = l_horaCont.AddMinutes(15)

                                            End While
                                        Else
                                            intCont = intCont + 1
                                        End If


                                    Next

                                End If
                            End If
                        Next

                    End If

                End If

            Next

        Catch ex As Exception

        End Try
    End Sub

    'Public Sub LlenarBloqueodeMecanicos()
    '    Try

    '        Dim dtMecBloq As System.Data.DataTable
    '        Dim strfecha As String
    '        Dim strIdMecAgenda As String
    '        Dim strIdMecBloc As String
    '        Dim strIntervalo As String
    '        Dim strConsulta As String
    '        Dim strUsaDurE As String
    '        Dim strSerieCita As String
    '        Dim strNumCita As String
    '        Dim intDuracion As Integer
    '        Dim strBMensaje As StringBuilder = New StringBuilder()

    '        Dim l_strSQLSuspension As String

    '        Dim l_horaInicio As Date
    '        Dim l_horaFin As Date
    '        Dim l_horaCont As Date
    '        Dim l_FhaInicio As Date
    '        Dim l_FhaFin As Date
    '        Dim l_numDocEntry As Integer
    '        Dim l_strBloq As String
    '        Dim l_strHoraDesde As String
    '        Dim l_strHoraHasta As String
    '        Dim l_strHoraAgenda As String
    '        Dim l_horaFinAgenda As Date
    '        Dim l_horaInicioAgenda As Date
    '        Dim strname As String = String.Empty



    '        m_dtFecha = dtpFecha.Value

    '        strfecha = Utilitarios.RetornaFechaFormatoDB(Convert.ToDateTime(m_dtFecha), m_oCompany.Server)

    '        l_horaInicioAgenda = FormatDateTime(m_fhaHoraInicio, DateFormat.ShortTime)
    '        l_horaFinAgenda = FormatDateTime(m_fhaHoraFin, DateFormat.ShortTime)

    '        l_strSQLSuspension = " Select  BM.DocEntry,BM.U_idMec,BM.U_FechI,LBM.U_FechCon,BM.U_HorI,BM.U_FechF,BM.U_HoraF,BM.U_Observ from [dbo].[@SCGD_BLOCMEC] as BM with(nolock) " & _
    '                                         " inner join  [dbo].[@SCGD_LINEAS_BLOME] as LBM on BM.DocEntry = LBM.DocEntry " & _
    '                                         " where  LBM.U_FechCon = '{0}' and BM.U_IdSucu = '{1}'"

    '        l_strSQLSuspension = String.Format(l_strSQLSuspension, strfecha, m_strCodSucursal)

    '        dtMecBloq = Utilitarios.EjecutarConsultaDataTable(l_strSQLSuspension)

    '        'strEstadoCancelado = Utilitarios.EjecutarConsulta(String.Format("select U_CodCitaCancel from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", m_strCodSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

    '        For Each row As DataRow In dtAgenda.Rows

    '            If IsDBNull(row.Item(mc_strIDAgenda)) Then

    '                Dim intPosition As Integer = dtAgenda.Rows.IndexOf(row)
    '                strname = dtAgenda.Columns(intPosition).ColumnName

    '                strIdMecAgenda = dtAgenda.Rows(intPosition)("ID").ToString()

    '                For Each rowBloc As DataRow In dtMecBloq.Rows

    '                    Dim intPositionBloc As Integer = dtMecBloq.Rows.IndexOf(rowBloc)
    '                    ''strname = dtMecBloq.Columns(intPosition).ColumnName
    '                    strBMensaje.Length = 0

    '                    strIdMecBloc = dtMecBloq.Rows(intPositionBloc)("U_idMec").ToString()

    '                    If strIdMecBloc = strIdMecAgenda Then

    '                        If Not IsDBNull(rowBloc.Item("U_FechF")) And Not IsDBNull(rowBloc.Item("U_FechCon")) Then
    '                            l_numDocEntry = rowBloc.Item("DocEntry")

    '                            l_strHoraDesde = Utilitarios.FormatoHora(rowBloc.Item("U_HorI"))
    '                            l_strHoraHasta = Utilitarios.FormatoHora(rowBloc.Item("U_HoraF"))

    '                            l_horaInicio = FormatDateTime(l_strHoraDesde, DateFormat.ShortTime)
    '                            l_horaFin = FormatDateTime(l_strHoraHasta, DateFormat.ShortTime)

    '                            l_FhaInicio = DateTime.Parse(rowBloc.Item("U_FechCon"))
    '                            l_FhaFin = DateTime.Parse(rowBloc.Item("U_FechF"))

    '                            l_strBloq = "n/a"


    '                            Dim intCont As Integer = 0

    '                            If l_horaFinAgenda < l_horaFin Then
    '                                l_horaFin = l_horaFinAgenda
    '                            End If


    '                            For Each element As String In listColumsGrid
    '                                l_strHoraAgenda = element

    '                                If l_strHoraAgenda.Equals(l_strHoraDesde.TrimStart("0″")) Then
    '                                    l_horaCont = l_horaInicio

    '                                    While l_horaCont <= l_horaFin
    '                                        Dim valorColumna As String() = listColumsGrid.ToArray()

    '                                        row.Item(valorColumna(intCont)) = l_strBloq
    '                                        intCont = intCont + 1
    '                                        l_horaCont = l_horaCont.AddMinutes(15)

    '                                    End While
    '                                Else
    '                                    intCont = intCont + 1
    '                                End If


    '                            Next

    '                        End If
    '                    End If
    '                Next
    '            End If


    '        Next

    '    Catch ex As Exception

    '    End Try
    'End Sub

    Dim m_intCantTotalColumnas As Integer

    Public Sub CrearTablaAgenda()
        Try

            dtAgenda.Columns.Add(mc_strIDAgenda, GetType(String))
            dtAgenda.Columns.Add(mc_strID, GetType(String))
            dtAgenda.Columns.Add("Posicion", GetType(String))
            dtAgenda.Columns.Add(mc_strName, GetType(String))
            dtAgenda.Columns.Add(mc_strEquipo, GetType(String))
            dtAgenda.Columns.Add(mc_strRol, GetType(String))
            dtAgenda.Columns.Add(mc_strInterv, GetType(String))
            dtAgenda.Columns.Add(mc_strServRap, GetType(String))


            For Each column As String In listColumsGrid

                dtAgenda.Columns.Add(column, GetType(String))

            Next

            dtAgenda.DefaultView.AllowNew = False
            m_intCantTotalColumnas = dtAgenda.Columns.Count
        Catch ex As Exception

        End Try

    End Sub

    Public Sub CrearTablaAgendaNombres()
        Try
            dtNombres.Columns.Add(mc_strName, GetType(String))
            dtNombres.DefaultView.AllowNew = False
        Catch ex As Exception

        End Try

    End Sub

    Private Sub CargarCombos()

        Try
            Dim l_strSQL As String
            Dim ldDatos As System.Data.DataTable

            l_strSQL = String.Format("SELECT Code, Name  from [DBO].[@SCGD_SUCURSALES]")

            ldDatos = Utilitarios.EjecutarConsultaDataTable(l_strSQL, m_oCompany.CompanyDB, m_oCompany.Server)

            cboAgenda.DataSource = ldDatos
            cboAgenda.ValueMember = "Code"
            cboAgenda.DisplayMember = "Name"

            cboAgenda.SelectedValue = m_strCodSucursal
        Catch ex As Exception

        End Try

    End Sub

    Private Sub LoadEstiloGrid()
        Const intWithDateCol As Integer = 60 '75
        Dim tsEstiloGrid As DataGridTableStyle

        tsEstiloGrid = New DataGridTableStyle

        Dim scIdAgenda As DataGridLabelColumn
        Dim scID As DataGridLabelColumn
        Dim scName As DataGridHoraColumn

        scIdAgenda = New DataGridLabelColumn
        With scIdAgenda
            .HeaderText = ""
            .MappingName = mc_strIDAgenda
            .Width = 0
        End With

        scID = New DataGridLabelColumn
        With scID
            .HeaderText = ""

            .MappingName = mc_strID
            .Width = 0
        End With

        scName = New DataGridHoraColumn
        With scName

            .HeaderText = ""
            .MappingName = mc_strName
            .Width = 0
        End With

        tsEstiloGrid.GridColumnStyles.Add(scIdAgenda)
        tsEstiloGrid.GridColumnStyles.Add(scID)
        tsEstiloGrid.GridColumnStyles.Add(scName)

        For Each column As String In listColumsGrid

            Dim columnaGrid As DataGridCitaAgendaColumn

            columnaGrid = New DataGridCitaAgendaColumn
            columnaGrid.HeaderText = column
            columnaGrid.MappingName = column
            columnaGrid.Width = intWithDateCol
            tsEstiloGrid.GridColumnStyles.Add(columnaGrid)
        Next

        tsEstiloGrid.PreferredRowHeight = 35
        tsEstiloGrid.SelectionBackColor = System.Drawing.Color.FromArgb(CType(20, Byte), CType(100, Byte), CType(44, Byte))
        tsEstiloGrid.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        tsEstiloGrid.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        'tsEstiloGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
        tsEstiloGrid.GridLineStyle = DataGridLineStyle.None
        tsEstiloGrid.RowHeadersVisible = False
        tsEstiloGrid.AllowSorting = False

        '' dtgOcupacion.TableStyles.Add(tsEstiloGrid)

    End Sub

    Private Sub LoadEstiloGrid2()
        Const intWithDateCol As Integer = 60 '75
        Dim tsEstiloGrid As DataGridTableStyle

        tsEstiloGrid = New DataGridTableStyle

        Dim scIdAgenda As DataGridLabelColumn
        Dim scID As DataGridLabelColumn
        Dim scName As DataGridHoraColumn

        scName = New DataGridHoraColumn
        With scName
            If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                .HeaderText = My.Resources.Resource.Agenda
            End If
            .MappingName = mc_strName
            .Width = 110
        End With

        tsEstiloGrid.GridColumnStyles.Add(scName)

        tsEstiloGrid.PreferredRowHeight = 35
        tsEstiloGrid.SelectionBackColor = System.Drawing.Color.FromArgb(CType(20, Byte), CType(100, Byte), CType(44, Byte))
        tsEstiloGrid.SelectionForeColor = System.Drawing.Color.FromArgb(CType(0, Byte), CType(53, Byte), CType(106, Byte))
        tsEstiloGrid.HeaderBackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
        'tsEstiloGrid.AlternatingBackColor = System.Drawing.Color.FromArgb(CType(244, Byte), CType(244, Byte), CType(240, Byte))
        tsEstiloGrid.GridLineStyle = DataGridLineStyle.None
        tsEstiloGrid.RowHeadersVisible = False
        tsEstiloGrid.AllowSorting = False

        ''dtgNombres.TableStyles.Add(tsEstiloGrid)

    End Sub

    Private Sub LoadEstiloGridCitasProblem()

        Dim columnaAgenda As New DataGridViewTextBoxColumn
        Dim columnaCita As New DataGridViewTextBoxColumn

        With columnaAgenda

            If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                .HeaderText = My.Resources.Resource.Agenda

            End If

            .DataPropertyName = mc_strName
            .Width = 200
            .ReadOnly = True
            .SortMode = False
            '.HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))

        End With

        With columnaCita
            .HeaderText = My.Resources.Resource.Citas
            .DataPropertyName = mc_strCita
            .ReadOnly = True
            .SortMode = False
            .FillWeight = 500
            '.HeaderCell.Style.BackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))

        End With

        dtgvCitasReasignar.Columns.Add(columnaAgenda)
        dtgvCitasReasignar.Columns.Add(columnaCita)
        With dtgvCitasReasignar.RowTemplate
            .Height = 40
        End With

        dtgvCitasReasignar.Anchor = AnchorStyles.Top & AnchorStyles.Left
        dtgvCitasReasignar.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill

    End Sub

    Private Sub DefinirColumnas()

        Dim strConsulta As String
        Dim dtHorario As System.Data.DataTable
        Dim strHoraInicio As String
        Dim strMinInicio As String
        Dim strHoraFinal As String
        Dim strMinFinal As String
        Dim strAResult As String()
        Dim intHoraInicio As Integer
        Dim intHoraFinal As Integer
        Dim intCont As Integer = 0
        Dim intMinInicio As Integer
        Dim intMinFinal As Integer
        Dim intContHora As Integer = 0
        Dim intContHoraFin As Integer = 0
        Dim strMin As String

        listColumsGrid = New List(Of String)()
        m_listColums = New List(Of infoColumn)()


        strConsulta = String.Format("SELECT U_HoraInicio, U_HoraFin FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = {0}", m_strCodSucursal)
        dtHorario = Utilitarios.EjecutarConsultaDataTable(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

        If dtHorario.Rows.Count <> 0 Then
            strAResult = FormatoHora(dtHorario.Rows(0)("U_HoraInicio")).Split(New Char() {":"c})
            strHoraInicio = strAResult(0)
            strMinInicio = strAResult(1)

            strAResult = FormatoHora(dtHorario.Rows(0)("U_HoraFin")).Split(New Char() {":"c})
            strHoraFinal = strAResult(0)
            strMinFinal = strAResult(1)

        End If

        intHoraInicio = Convert.ToInt32(strHoraInicio)
        intHoraFinal = Convert.ToInt32(strHoraFinal)

        intMinInicio = Convert.ToInt32(strMinInicio)
        intMinFinal = Convert.ToInt32(strMinFinal)

        If intContHora <> intMinInicio Then
            intContHora = intMinInicio
        End If

        For i As Integer = intHoraInicio To intHoraFinal
            If i = intHoraFinal Then
                intContHoraFin = intMinFinal
            Else
                intContHoraFin = 60
            End If
            While intContHora < intContHoraFin
                strMin = IIf(intContHora.ToString.Length = 1, "0" + intContHora.ToString, intContHora.ToString)
                listColumsGrid.Add(Convert.ToString(i) + ":" + strMin)

                intContHora += 15

                m_InfoColum._position = intCont
                m_InfoColum._Hora = Convert.ToString(i)
                m_InfoColum._Minutos = strMin
                m_InfoColum._horaFull = Convert.ToString(i) + ":" + strMin
                m_listColums.Add(m_InfoColum)

                intCont += 1
            End While
            intContHora = 0
        Next
        Dim strin As String
    End Sub

    Private Sub DefinirColumnasGrid()

        Dim intHoraInicio As Integer
        Dim intHoraFinal As Integer
        Dim strConsulta As String
        Dim dtHorario As System.Data.DataTable
        Dim strAResult As String()
        Dim strHoraInicio As String
        Dim strHoraFinal As String
        Dim strMinInicio As String
        Dim strMinFinal As String
        Dim intMinInicio As Integer
        Dim intMinFinal As Integer
        Dim intContHora As Integer = 0
        Dim intContHoraFin As Integer = 0
        Dim strMin As String

        listColumsGrid = New List(Of String)()

        strConsulta = String.Format("SELECT U_HoraInicio, U_HoraFin FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = {0}", m_strCodSucursal)
        dtHorario = Utilitarios.EjecutarConsultaDataTable(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)

        If dtHorario.Rows.Count <> 0 Then
            strAResult = FormatoHora(dtHorario.Rows(0)("U_HoraInicio")).Split(New Char() {":"c})
            strHoraInicio = strAResult(0)
            strMinInicio = strAResult(1)

            strAResult = FormatoHora(dtHorario.Rows(0)("U_HoraFin")).Split(New Char() {":"c})
            strHoraFinal = strAResult(0)
            strMinFinal = strAResult(1)

        End If

        intHoraInicio = Convert.ToInt32(strHoraInicio)
        intMinInicio = Convert.ToInt32(strMinInicio)

        intHoraFinal = Convert.ToInt32(strHoraFinal)
        intMinFinal = Convert.ToInt32(strMinFinal)

        If intContHora <> intMinInicio Then
            intContHora = intMinInicio
        End If

        For i As Integer = intHoraInicio To intHoraFinal

            If i = intHoraFinal Then
                intContHoraFin = intMinFinal
            Else
                intContHoraFin = 60
            End If
            While intContHora < intContHoraFin

                strMin = IIf(intContHora.ToString.Length = 1, "0" + intContHora.ToString, intContHora.ToString)
                listColumsGrid.Add(Convert.ToString(i) + ":" + strMin)
                intContHora += 15

            End While
            intContHora = 0
        Next

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

    'Private Sub ShowToolTipInfo(ByVal p_objPoint As Point)
    '    Dim objHTI As DataGrid.HitTestInfo
    '    Dim CurrentCell As DataGridCell


    '    objHTI = dtgOcupacion.HitTest(p_objPoint)

    '    If objHTI.Type = DataGrid.HitTestType.Cell Then
    '        If objHTI.Column > 2 And objHTI.Row > -1 And objHTI.Row < dtAgenda.Rows.Count Then

    '            CurrentCell.ColumnNumber = objHTI.Column
    '            CurrentCell.RowNumber = objHTI.Row

    '            If CurrentCell.ColumnNumber <> m_OldCell.ColumnNumber Or _
    '                CurrentCell.RowNumber <> m_OldCell.RowNumber Then

    '                If Not IsDBNull(dtgOcupacion.Item(objHTI.Row, objHTI.Column)) Then
    '                    m_strTextoInfo = GetToolTipInfo(CStr(dtgOcupacion.Item(objHTI.Row, objHTI.Column)))
    '                End If
    '            End If
    '            ToolTip1.SetToolTip(dtgOcupacion, m_strTextoInfo)
    '        Else
    '            ToolTip1.SetToolTip(dtgOcupacion, "")
    '        End If

    '        m_OldCell.ColumnNumber = objHTI.Column
    '        m_OldCell.RowNumber = objHTI.Row

    '    Else
    '        ToolTip1.SetToolTip(dtgOcupacion, "")
    '    End If

    'End Sub

    Private Sub ShowToolTipInfoDataGridView(ByVal p_objPoint As Point)
        Dim objHTI As DataGridView.HitTestInfo
        Dim CurrentCell As DataGridCell
        Dim X, Y As Integer

        Try
            'dgv_AgendaCitas.ShowCellToolTips = True
            dgv_AgendaCitas.ShowCellToolTips = False
            objHTI = dgv_AgendaCitas.HitTest(p_objPoint.X, p_objPoint.Y)

            If objHTI.Type = DataGrid.HitTestType.Cell Then
                If objHTI.ColumnIndex > 7 And objHTI.RowIndex > -1 And objHTI.RowIndex < dtAgenda.Rows.Count Then

                    CurrentCell.ColumnNumber = objHTI.ColumnIndex
                    CurrentCell.RowNumber = objHTI.RowIndex

                    If CurrentCell.ColumnNumber <> m_OldCell.ColumnNumber Or _
                        CurrentCell.RowNumber <> m_OldCell.RowNumber Then

                        If Not IsDBNull(dgv_AgendaCitas.Item(objHTI.ColumnIndex, objHTI.RowIndex).Value) Then
                            m_strTextoInfo = GetToolTipInfo(CStr(dgv_AgendaCitas.Item(objHTI.ColumnIndex, objHTI.RowIndex).Value))
                            dgv_AgendaCitas.Item(objHTI.ColumnIndex, objHTI.RowIndex).ToolTipText = m_strTextoInfo
                            X = p_objPoint.X
                            Y = p_objPoint.Y
                            X += AjustePosicionToolTip(X, Me.Size.Width, m_strTextoInfo)
                            ToolTip1.Show(m_strTextoInfo, Me, New Point(X, Y), 120000)
                        End If
                    End If
                Else
                    ToolTip1.Hide(Me)
                End If

                m_OldCell.ColumnNumber = objHTI.ColumnIndex
                m_OldCell.RowNumber = objHTI.RowIndex
            End If

            If objHTI.Type = DataGridViewHitTestType.None Then
                ToolTip1.Hide(Me)
                m_OldCell.ColumnNumber = 0
                m_OldCell.RowNumber = 0
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Calcula la distancia entre el borde de la ventana y el tamaño del ToolTip para evitar que quede oculto
    ''' </summary>
    ''' <param name="X">Posición X desde la cual se genera el evento</param>
    ''' <param name="WindowWidth">Tamaño de la ventana</param>
    ''' <param name="strComentarios">Comentarios o texto del ToolTip</param>
    ''' <returns>Número entero que representa el ajuste en la posición X</returns>
    ''' <remarks></remarks>
    Private Function AjustePosicionToolTip(ByVal X As Integer, ByVal WindowWidth As Integer, ByVal strComentarios As String) As Integer
        Dim intResultado As Integer = 0
        Dim intLargo As Integer = 0
        Dim intEspacioDisponible As Integer = 0
        Try
            Dim strLineas() As String = Split(strComentarios, vbCrLf)

            If Not String.IsNullOrEmpty(strComentarios) Then
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

    Private Function GetToolTipInfo(ByVal p_strCitaNum As String) As String

        Dim strConsulta As String
        Dim resultCita As String()
        Dim resultOT As String()
        Dim dtConsulta As System.Data.DataTable
        Dim stbMensaje As New StringBuilder()
        Dim strMovilidad As String = String.Empty
        Dim strFormaContacto As String = String.Empty

        Try
            p_strCitaNum = ObtenerNumeroCita(p_strCitaNum)
            If Not String.IsNullOrEmpty(p_strCitaNum) And Not p_strCitaNum.Equals("n/a") And Not p_strCitaNum.Equals(".") And Not p_strCitaNum.Equals("***") Then
                If ConsultaEsCita(p_strCitaNum.Trim()) Then
                    resultCita = p_strCitaNum.Split(New Char() {"-"c})
                    strConsulta = String.Format("SELECT T0.U_CardCode, T0.U_CardName, T0.U_Cod_Unid, T1.U_Num_Plac, T0.U_Observ, T0.U_CRetiro, T0.U_CEntrega, T0.U_Movilidad, T3.Name AS 'DscMovilidad', T0.U_CMovilidad, T0.U_Contacto, T4.Name AS 'DscContacto', T0.U_CContacto FROM ""@SCGD_CITA"" AS T0 INNER JOIN ""@SCGD_VEHICULO"" AS T1 ON T0.U_Cod_Unid = T1.U_Cod_Unid LEFT JOIN ""@SCGD_MOVILIDAD"" T3 ON T0.U_Movilidad = T3.Code LEFT JOIN ""@SCGD_FCONTACTO"" T4 ON T0.U_Contacto = T4.Code WHERE U_Num_Serie = '{0}' and U_NumCita = '{1}'", resultCita(0), resultCita(1).Trim())
                    dtConsulta = Utilitarios.EjecutarConsultaDataTable(strConsulta)

                    stbMensaje.Append(My.Resources.Resource.NumCita).Append(p_strCitaNum).Append(vbCrLf)

                    For Each rowConsulta As DataRow In dtConsulta.Rows

                        stbMensaje.Append(My.Resources.Resource.cliente).Append(rowConsulta.Item("U_CardName")).Append(" ").Append(My.Resources.Resource.codigo).Append(rowConsulta.Item("U_CardCode")).Append(vbCrLf)
                        stbMensaje.Append(My.Resources.Resource.vehiculo1).Append(rowConsulta.Item("U_Num_Plac")).Append(" ").Append(My.Resources.Resource.codigo).Append(rowConsulta.Item("U_Cod_Unid")).Append(vbCrLf)
                        If (rowConsulta.Item("U_Observ").ToString() <> "") Then
                            stbMensaje.Append(My.Resources.Resource.TXTObservaciones).Append(rowConsulta.Item("U_Observ")).Append(vbCrLf)
                        End If
                        If (rowConsulta.Item("U_CRetiro").ToString() <> "") Then
                            stbMensaje.Append(My.Resources.Resource.RetiroVehiculo).Append(rowConsulta.Item("U_CRetiro")).Append(vbCrLf)
                        End If
                        If (rowConsulta.Item("U_CEntrega").ToString() <> "") Then
                            stbMensaje.Append(My.Resources.Resource.EntregaVehiculo).Append(rowConsulta.Item("U_CEntrega")).Append(vbCrLf)
                        End If
                        If (rowConsulta.Item("DscMovilidad").ToString() <> "") Then
                            strMovilidad = String.Format("{0}{1}{2}{3}", My.Resources.Resource.Movilidad, rowConsulta.Item("DscMovilidad"), ". ", rowConsulta.Item("U_CMovilidad"))
                            stbMensaje.Append(strMovilidad).Append(vbCrLf)
                        End If
                        If (rowConsulta.Item("DscContacto").ToString() <> "") Then
                            strFormaContacto = String.Format("{0}{1}{2}{3}", My.Resources.Resource.FormaContacto, rowConsulta.Item("DscContacto"), ". ", rowConsulta.Item("U_CContacto"))
                            stbMensaje.Append(strFormaContacto).Append(vbCrLf)
                        End If

                    Next
                Else
                    resultOT = p_strCitaNum.Split(New Char() {"-"c})
                    strConsulta = String.Format("Select QU.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Cod_Unidad,QU.U_SCGD_Num_Placa,QU.CardCode, QU.CardName " +
                                                " from OQUT QU " +
                                                " where U_SCGD_Numero_OT = '{0}'", p_strCitaNum)

                    dtConsulta = Utilitarios.EjecutarConsultaDataTable(strConsulta)

                    For Each rowConsulta As DataRow In dtConsulta.Rows
                        stbMensaje.Append(My.Resources.Resource.NumCita).Append(rowConsulta.Item("U_SCGD_NoSerieCita")).Append("-").Append(rowConsulta.Item("U_SCGD_NoCita")).Append(vbCrLf)
                        stbMensaje.Append(My.Resources.Resource.cliente).Append(rowConsulta.Item("CardName")).Append(" ").Append(My.Resources.Resource.codigo).Append(rowConsulta.Item("CardCode")).Append(vbCrLf)
                        stbMensaje.Append(My.Resources.Resource.vehiculo1).Append(rowConsulta.Item("U_SCGD_Num_Placa")).Append(" ").Append(My.Resources.Resource.codigo).Append(rowConsulta.Item("U_SCGD_Cod_Unidad"))
                    Next

                End If
            End If

            Return stbMensaje.ToString()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

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

    Private Sub CreateDataTableCitasProblemas()

        dtCitasProbl.Columns.Add(mc_strName, GetType(String))
        dtCitasProbl.Columns.Add(mc_strCita, GetType(String))

    End Sub

    Private Sub LoadCitasProblemas()

        Dim enumerator As IDictionaryEnumerator = tableCitasProbl.GetEnumerator()
        Dim dataRow As DataRow = Nothing
        Dim strCita As String = String.Empty
        Dim strAgenda As String = String.Empty
        Dim strDocEntryCitas As String = String.Empty

        While enumerator.MoveNext()

            strDocEntryCitas = Convert.ToString(enumerator.Key)
            strCita = Convert.ToString(enumerator.Value)

            If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                strAgenda = Utilitarios.EjecutarConsulta(String.Format("Select A.U_Agenda from [@SCGD_CITA] as C inner join [@SCGD_AGENDA] as A on C.U_Cod_Agenda = A.DocEntry where C.DocEntry = '{0}'", strDocEntryCitas), m_oCompany.CompanyDB, m_oCompany.Server)
            End If


            dataRow = dtCitasProbl.NewRow()
            dataRow(mc_strName) = strAgenda
            dataRow(mc_strCita) = strCita

            dtCitasProbl.Rows.Add(dataRow)

        End While

    End Sub

    Private Sub removerColumnasDataTables(ByRef dataTable As System.Data.DataTable)

        Dim intCantColums As Integer = dataTable.Columns.Count - 1

        For i As Integer = 0 To intCantColums

            dataTable.Columns.RemoveAt(0)

        Next

    End Sub

#End Region

#Region "Eventos"

    Dim intRowNum As Integer
    Dim strEquipo As String = String.Empty
    Dim strRol As String = String.Empty
    Dim strCode As String = String.Empty
    Dim intColNum As Integer

    Dim blnSeleccionAs As Boolean = False
    Dim blnSeleccionTec As Boolean = False

    Dim intFAnt As Integer
    Dim intCAnt As Integer
    Dim intFNue As Integer
    Dim intCNue As Integer

    Dim intFAntTec As Integer
    Dim intCAntTec As Integer
    Dim intFNueTec As Integer
    Dim intCNueTec As Integer

    Dim fhaAsesorSel As Date
    Dim fhaTecnicoSel As Date

    Dim m_strCodTecnico As String = "-1"
    Dim m_strCodAsesor As String = "-1"

    Dim m_strSerieCita As String
    Dim m_strNumCita As String

    Private listColOcupadasAse As New List(Of String)
    Private listColOcupadasTec As New List(Of String)
    Private listOcupadas As New List(Of String)


    'Private Sub MarcarOcupacion()
    '    Dim strCurrenCell As String
    '    Dim intCantColServicio As Integer
    '    Dim intCantColServicioAse As Integer
    '    Dim blnEspacio As Boolean = True
    '    Dim intIntervalo As Integer

    '    If m_intServRapido = 0 Then
    '        intCantColServicio = m_intEspacioTec
    '    Else
    '        intCantColServicio = m_intServRapido
    '    End If

    '    '  intCantColServicio = m_intEspacioTec
    '    intCantColServicioAse = m_intEspacioAse

    '    If dtgOcupacion.CurrentRowIndex <> -1 Then
    '        Dim oCell As DataGridCell
    '        oCell = dtgOcupacion.CurrentCell

    '        strCurrenCell = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
    '        intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '        intColNum = dtgOcupacion.CurrentCell.ColumnNumber

    '        ' strEquipo = m_listRows(intRowNum)._Equipo
    '        strRol = m_listRows(intRowNum)._Rol
    '        intIntervalo = CInt(IIf(String.IsNullOrEmpty(m_listRows(intRowNum)._Intervalo), 15, m_listRows(intRowNum)._Intervalo))
    '        '  intCantColServicioAse = ObtenerCantidadEspaciosAgenda(intIntervalo)

    '        ' strCode = m_listRows(intRowNum)._Code

    '        Select Case strCurrenCell
    '            Case "."
    '                If blnSeleccionAs = False Then
    '                    intFAnt = 0
    '                    intCAnt = 0

    '                    intFNue = oCell.RowNumber
    '                    intCNue = oCell.ColumnNumber
    '                    blnSeleccionAs = True

    '                Else
    '                    intFAnt = intFNue
    '                    intCAnt = intCNue
    '                    intFNue = oCell.RowNumber
    '                    intCNue = oCell.ColumnNumber
    '                End If

    '                For Each l_col As Integer In listColOcupadasAse
    '                    dtgOcupacion.Item(intFAnt, l_col) = "."
    '                Next
    '                listColOcupadasAse.Clear()

    '                For i As Integer = 0 To intCantColServicioAse - 1
    '                    If intCNue + i < m_intCantTotalColumnas - 3 Then

    '                        If dtgOcupacion.Item(intFNue, intCNue + i).Equals(".") OrElse
    '                       dtgOcupacion.Item(intFNue, intCNue + i).Equals("***") Then
    '                            listColOcupadasAse.Add(intCNue + i)
    '                        Else
    '                            blnEspacio = False
    '                            Exit For
    '                        End If
    '                    Else
    '                        blnEspacio = False
    '                        Exit For
    '                    End If

    '                Next
    '                For Each l_col As Integer In listColOcupadasAse
    '                    If blnEspacio Then
    '                        dtgOcupacion.Item(intFNue, l_col) = "***"
    '                    Else

    '                        dtgOcupacion.Item(intFNue, l_col) = "."
    '                    End If
    '                Next
    '                If Not blnEspacio Then
    '                    MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioReqRecep)
    '                End If


    '                fhaAsesorSel = ObtenerFechaHoraOnClick()
    '                m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                m_strCodAsesor = m_listRows(intRowNum)._Code

    '            Case "***"

    '                If strRol.Equals("A") Then
    '                    intFAnt = intFNue
    '                    intCAnt = intCNue

    '                    intFNue = oCell.RowNumber
    '                    intCNue = oCell.ColumnNumber


    '                    For Each l_col As Integer In listColOcupadasAse
    '                        dtgOcupacion.Item(intFAnt, l_col) = "."
    '                    Next
    '                    listColOcupadasAse.Clear()

    '                    If m_oCeldaAsesor._ColNum <> 0 OrElse m_oCeldaAsesor._FilNum <> 0 Then
    '                        For i As Integer = 0 To intCantColServicioAse - 1
    '                            If intCNue + i < m_intCantTotalColumnas - 3 Then
    '                                If dtgOcupacion.Item(intFNue, intCNue + i).Equals(".") OrElse
    '                               dtgOcupacion.Item(intFNue, intCNue + i).Equals("***") Then
    '                                    listColOcupadasAse.Add(intCNue + i)
    '                                Else
    '                                    blnEspacio = False
    '                                    Exit For
    '                                End If
    '                            Else
    '                                blnEspacio = False
    '                                Exit For
    '                            End If

    '                        Next
    '                        For Each l_col As Integer In listColOcupadasAse
    '                            If blnEspacio Then
    '                                dtgOcupacion.Item(intFNue, l_col) = "***"
    '                            Else

    '                                dtgOcupacion.Item(intFNue, l_col) = "."
    '                            End If
    '                        Next
    '                        If Not blnEspacio Then
    '                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioReqRecep)
    '                            pnlMensaje.BackColor = Color.RosyBrown
    '                        End If

    '                        fhaAsesorSel = ObtenerFechaHoraOnClick()
    '                        m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                        m_strCodAsesor = m_listRows(intRowNum)._Code

    '                    Else
    '                        fhaAsesorSel = Nothing
    '                        m_strCodAgenda = String.Empty
    '                        m_strCodAsesor = String.Empty
    '                    End If

    '                ElseIf strRol.Equals("T") Then
    '                    intFAntTec = intFNueTec
    '                    intCAntTec = intCNueTec

    '                    intFNueTec = oCell.RowNumber
    '                    intCNueTec = oCell.ColumnNumber

    '                    For Each l_col As Integer In listColOcupadasTec
    '                        dtgOcupacion.Item(intFAntTec, l_col) = ""
    '                    Next

    '                    listColOcupadasTec.Clear()

    '                    Dim intCont As Integer = 0
    '                    Dim intEspaciosLlenos As Integer = 0

    '                    While intEspaciosLlenos <= intCantColServicio - 1
    '                        If intCNueTec + intCont < m_intCantTotalColumnas - 3 Then

    '                            If dtgOcupacion.Item(intFNueTec, intCNueTec + intCont).Equals(String.Empty) OrElse
    '                                dtgOcupacion.Item(intFNueTec, intCNueTec + intCont).Equals("***") Then

    '                                listColOcupadasTec.Add(intCNueTec + intCont)
    '                                intEspaciosLlenos += 1
    '                                intCont += 1
    '                            ElseIf dtgOcupacion.Item(intFNueTec, intCNueTec + intCont).Equals("n/a") Then
    '                                intCont += 1
    '                            Else
    '                                blnEspacio = False
    '                                Exit While
    '                            End If
    '                        Else
    '                            Exit While
    '                        End If
    '                    End While


    '                    If m_oCeldaTecnico._FilNum <> 0 OrElse m_oCeldaTecnico._ColNum <> 0 Then


    '                        For Each l_col As Integer In listColOcupadasTec
    '                            If blnEspacio Then
    '                                dtgOcupacion.Item(intFNueTec, l_col) = "***"
    '                            Else
    '                                dtgOcupacion.Item(intFNueTec, l_col) = ""
    '                            End If
    '                        Next
    '                        If Not blnEspacio Then
    '                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioRequerido)
    '                            pnlMensaje.BackColor = Color.RosyBrown
    '                        End If
    '                    Else
    '                        fhaTecnicoSel = Nothing
    '                        m_strCodTecnico = String.Empty
    '                    End If

    '                End If

    '            Case String.Empty
    '                If blnSeleccionTec = False Then
    '                    intFAntTec = 0
    '                    intCAntTec = 0
    '                    intFNueTec = oCell.RowNumber
    '                    intCNueTec = oCell.ColumnNumber
    '                    blnSeleccionTec = True
    '                Else
    '                    intFAntTec = intFNueTec
    '                    intCAntTec = intCNueTec

    '                    dtgOcupacion.Item(intFAntTec, intCAntTec) = ""
    '                    intFNueTec = oCell.RowNumber
    '                    intCNueTec = oCell.ColumnNumber
    '                End If

    '                For Each l_col As Integer In listColOcupadasTec
    '                    dtgOcupacion.Item(intFAntTec, l_col) = ""
    '                Next
    '                listColOcupadasTec.Clear()

    '                Dim intCont As Integer = 0
    '                Dim intEspaciosLlenos As Integer = 0

    '                While intEspaciosLlenos <= intCantColServicio - 1

    '                    If intCNueTec + intCont < m_intCantTotalColumnas - 3 Then

    '                        If (dtgOcupacion.Item(intFNueTec, intCNueTec + intCont).Equals(String.Empty) OrElse
    '                            dtgOcupacion.Item(intFNueTec, intCNueTec + intCont).Equals("***")) Then

    '                            listColOcupadasTec.Add(intCNueTec + intCont)

    '                            If intCNueTec + intCont >= m_intCantTotalColumnas - 1 Then
    '                                Exit While
    '                            End If
    '                            intEspaciosLlenos += 1
    '                            intCont += 1

    '                        ElseIf dtgOcupacion.Item(intFNueTec, intCNueTec + intCont).Equals("n/a") Then
    '                            intCont += 1
    '                        Else
    '                            blnEspacio = False
    '                            Exit While
    '                        End If
    '                    Else
    '                        Exit While
    '                    End If

    '                End While

    '                For Each l_col As Integer In listColOcupadasTec
    '                    If blnEspacio Then
    '                        dtgOcupacion.Item(intFNueTec, l_col) = "***"
    '                    Else
    '                        MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioRequerido)


    '                        dtgOcupacion.Item(intFNueTec, l_col) = ""
    '                    End If
    '                Next

    '                fhaTecnicoSel = ObtenerFechaHoraOnClick()
    '                m_strCodTecnico = m_listRows(intRowNum)._Code

    '        End Select

    '    End If
    ' End Sub

    Private Sub MarcarOcupacionDataGridView()
        Dim strCurrenCell As String
        Dim intCantColServicio As Integer
        Dim intCantColServicioAse As Integer
        Dim blnEspacio As Boolean = True
        Dim intIntervalo As Integer
        Dim BIngresoLis As Boolean = False


        If m_intServRapido = 0 Then
            intCantColServicio = m_intEspacioTec
        Else
            intCantColServicio = m_intServRapido
        End If

        '  intCantColServicio = m_intEspacioTec
        intCantColServicioAse = m_intEspacioAse

        If dgv_AgendaCitas.CurrentCell.RowIndex <> -1 Then
            Dim oCell As DataGridViewCell
            oCell = dgv_AgendaCitas.CurrentCell

            strCurrenCell = dgv_AgendaCitas.Item(oCell.ColumnIndex, oCell.RowIndex).Value.ToString()
            intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
            intColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex

            ' strEquipo = m_listRows(intRowNum)._Equipo
            strRol = m_listRows(intRowNum)._Rol
            intIntervalo = CInt(IIf(String.IsNullOrEmpty(m_listRows(intRowNum)._Intervalo), 15, m_listRows(intRowNum)._Intervalo))
            '  intCantColServicioAse = ObtenerCantidadEspaciosAgenda(intIntervalo)

            ' strCode = m_listRows(intRowNum)._Code

            Select Case strCurrenCell
                Case "."
                    If blnSeleccionAs = False Then
                        intFAnt = 0
                        intCAnt = 0

                        intFNue = oCell.RowIndex
                        intCNue = oCell.ColumnIndex
                        blnSeleccionAs = True

                    Else
                        intFAnt = intFNue
                        intCAnt = intCNue
                        intFNue = oCell.RowIndex
                        intCNue = oCell.ColumnIndex
                    End If

                    For Each l_col As Integer In listColOcupadasAse
                        dgv_AgendaCitas.Item(l_col, intFAnt).Value = "."
                        dgv_AgendaCitas.Item(l_col, intFAnt).Style.BackColor = Color.Gainsboro
                        BIngresoLis = True
                    Next
                    If BIngresoLis Then
                        intFAnt = oCell.RowIndex
                        intCAnt = oCell.ColumnIndex
                    End If
                    listColOcupadasAse.Clear()

                    For i As Integer = 0 To intCantColServicioAse - 1
                        If intCNue + i < m_intCantTotalColumnas Then
                            ''Dim strValor As String = dgv_AgendaCitas.Item(intCNue + i, intFNue)

                            If dgv_AgendaCitas.Item(intCNue + i, intFNue).Value.Equals(".") OrElse
                           dgv_AgendaCitas.Item(intCNue + i, intFNue).Value.Equals("***") Then
                                listColOcupadasAse.Add(intCNue + i)
                            Else
                                blnEspacio = False
                                Exit For
                            End If
                        Else
                            blnEspacio = False
                            Exit For
                        End If

                    Next
                    For Each l_col As Integer In listColOcupadasAse
                        If blnEspacio Then
                            dgv_AgendaCitas.Item(l_col, intFNue).Value = "***"
                            dgv_AgendaCitas.Item(l_col, intFNue).Style.BackColor = Color.IndianRed
                            dgv_AgendaCitas.Item(l_col, intFNue).Style.SelectionBackColor = Color.IndianRed
                        Else

                            dgv_AgendaCitas.Item(l_col, intFAnt).Value = "."
                            dgv_AgendaCitas.Item(l_col, intFAnt).Style.BackColor = Color.Gainsboro
                            dgv_AgendaCitas.Item(l_col, intFAnt).Style.SelectionBackColor = Color.Gainsboro
                        End If
                    Next
                    If Not blnEspacio Then
                        MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioReqRecep)
                    End If


                    fhaAsesorSel = ObtenerFechaHoraOnClickDataGridView()
                    m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
                    m_strCodAsesor = m_listRows(intRowNum)._Code

                Case "***"

                    If strRol.Equals("A") Then
                        intFAnt = intFNue
                        intCAnt = intCNue

                        intFNue = oCell.RowIndex
                        intCNue = oCell.ColumnIndex


                        For Each l_col As Integer In listColOcupadasAse
                            dgv_AgendaCitas.Item(l_col, intFAnt).Value = "."
                            dgv_AgendaCitas.Item(l_col, intFAnt).Style.BackColor = Color.Gainsboro
                            dgv_AgendaCitas.Item(l_col, intFAnt).Style.SelectionBackColor = Color.Gainsboro
                        Next
                        listColOcupadasAse.Clear()

                        If m_oCeldaAsesor._ColNum <> 0 OrElse m_oCeldaAsesor._FilNum <> 0 Then
                            For i As Integer = 0 To intCantColServicioAse - 1
                                If intCNue + i < m_intCantTotalColumnas Then
                                    If dgv_AgendaCitas.Item(intCNue + i, intFNue).Value.Equals(".") OrElse
                                   dgv_AgendaCitas.Item(intCNue + i, intFNue).Value.Equals("***") Then
                                        listColOcupadasAse.Add(intCNue + i)
                                    Else
                                        blnEspacio = False
                                        Exit For
                                    End If
                                Else
                                    blnEspacio = False
                                    Exit For
                                End If

                            Next
                            For Each l_col As Integer In listColOcupadasAse
                                If blnEspacio Then
                                    dgv_AgendaCitas.Item(l_col, intFNue).Value = "***"
                                    dgv_AgendaCitas.Item(l_col, intFNue).Style.BackColor = Color.IndianRed
                                    dgv_AgendaCitas.Item(l_col, intFNue).Style.SelectionBackColor = Color.IndianRed
                                Else

                                    dgv_AgendaCitas.Item(l_col, intFNue).Value = "."
                                    dgv_AgendaCitas.Item(l_col, intFNue).Style.BackColor = Color.Gainsboro
                                    dgv_AgendaCitas.Item(l_col, intFNue).Style.SelectionBackColor = Color.Gainsboro
                                End If
                            Next
                            If Not blnEspacio Then
                                dgv_AgendaCitas.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent
                                MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioReqRecep)
                                pnlMensaje.BackColor = Color.RosyBrown
                            End If

                            fhaAsesorSel = ObtenerFechaHoraOnClickDataGridView()
                            m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
                            m_strCodAsesor = m_listRows(intRowNum)._Code

                        Else
                            fhaAsesorSel = Nothing
                            m_strCodAgenda = String.Empty
                            m_strCodAsesor = String.Empty
                        End If

                    ElseIf strRol.Equals("T") Then
                        intFAntTec = intFNueTec
                        intCAntTec = intCNueTec

                        intFNueTec = oCell.RowIndex
                        intCNueTec = oCell.ColumnIndex

                        For Each l_col As Integer In listColOcupadasTec
                            dgv_AgendaCitas.Item(l_col, intFAntTec).Value = ""
                            dgv_AgendaCitas.Item(l_col, intFAntTec).Style.BackColor = Color.White
                            dgv_AgendaCitas.Item(l_col, intFAntTec).Style.SelectionBackColor = Color.White
                        Next

                        listColOcupadasTec.Clear()

                        Dim intCont As Integer = 0
                        Dim intEspaciosLlenos As Integer = 0

                        While intEspaciosLlenos <= intCantColServicio - 1
                            If intCNueTec + intCont < m_intCantTotalColumnas Then

                                If dgv_AgendaCitas.Item(intCNueTec + intCont, intFNueTec).Value.Equals(String.Empty) OrElse
                                    dgv_AgendaCitas.Item(intCNueTec + intCont, intFNueTec).Value.Equals("***") Then

                                    listColOcupadasTec.Add(intCNueTec + intCont)
                                    intEspaciosLlenos += 1
                                    intCont += 1
                                ElseIf dgv_AgendaCitas.Item(intCNueTec + intCont, intFNueTec).Value.Equals("n/a") Then
                                    intCont += 1
                                Else
                                    blnEspacio = False
                                    Exit While
                                End If
                            Else
                                Exit While
                            End If
                        End While


                        If m_oCeldaTecnico._FilNum <> 0 OrElse m_oCeldaTecnico._ColNum <> 0 Then


                            For Each l_col As Integer In listColOcupadasTec
                                If blnEspacio Then
                                    dgv_AgendaCitas.Item(l_col, intFNueTec).Value = "***"
                                    dgv_AgendaCitas.Item(l_col, intFNueTec).Style.BackColor = Color.IndianRed
                                    dgv_AgendaCitas.Item(l_col, intFNueTec).Style.SelectionBackColor = Color.IndianRed
                                Else
                                    dgv_AgendaCitas.Item(l_col, intFNueTec).Value = ""
                                    dgv_AgendaCitas.Item(l_col, intFNueTec).Style.BackColor = Color.White
                                    dgv_AgendaCitas.Item(l_col, intFNueTec).Style.SelectionBackColor = Color.White
                                End If
                            Next
                            If Not blnEspacio Then
                                dgv_AgendaCitas.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent
                                MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioRequerido)
                                pnlMensaje.BackColor = Color.RosyBrown
                            End If
                        Else
                            fhaTecnicoSel = Nothing
                            m_strCodTecnico = String.Empty
                        End If

                    End If

                Case String.Empty
                    If blnSeleccionTec = False Then
                        intFAntTec = 0
                        intCAntTec = 0
                        intFNueTec = oCell.RowIndex
                        intCNueTec = oCell.ColumnIndex
                        blnSeleccionTec = True
                    Else
                        intFAntTec = intFNueTec
                        intCAntTec = intCNueTec

                        dgv_AgendaCitas.Item(intCAntTec, intFAntTec).Value = ""
                        intFNueTec = oCell.RowIndex
                        intCNueTec = oCell.ColumnIndex
                    End If

                    For Each l_col As Integer In listColOcupadasTec
                        dgv_AgendaCitas.Item(l_col, intFAntTec).Value = ""
                        dgv_AgendaCitas.Item(l_col, intFAntTec).Style.BackColor = Color.White
                    Next
                    listColOcupadasTec.Clear()

                    Dim intCont As Integer = 0
                    Dim intEspaciosLlenos As Integer = 0

                    While intEspaciosLlenos <= intCantColServicio - 1

                        If intCNueTec + intCont < m_intCantTotalColumnas Then

                            If (dgv_AgendaCitas.Item(intCNueTec + intCont, intFNueTec).Value.Equals(String.Empty) OrElse
                                dgv_AgendaCitas.Item(intCNueTec + intCont, intFNueTec).Value.Equals("***")) Then

                                listColOcupadasTec.Add(intCNueTec + intCont)

                                If intCNueTec + intCont >= m_intCantTotalColumnas - 1 Then
                                    Exit While
                                End If
                                intEspaciosLlenos += 1
                                intCont += 1

                            ElseIf dgv_AgendaCitas.Item(intCNueTec + intCont, intFNueTec).Value.Equals("n/a") Then
                                intCont += 1
                            Else
                                blnEspacio = False
                                Exit While
                            End If
                        Else
                            Exit While
                        End If

                    End While

                    For Each l_col As Integer In listColOcupadasTec
                        If blnEspacio Then
                            dgv_AgendaCitas.Item(l_col, intFNueTec).Value = "***"
                            dgv_AgendaCitas.Item(l_col, intFNueTec).Style.BackColor = Color.IndianRed
                            dgv_AgendaCitas.Item(l_col, intFNueTec).Style.SelectionBackColor = Color.IndianRed
                        Else
                            dgv_AgendaCitas.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent
                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasEspacioRequerido)


                            dgv_AgendaCitas.Item(l_col, intFNueTec).Value = ""
                            dgv_AgendaCitas.Item(l_col, intFNueTec).Style.BackColor = Color.White
                        End If
                    Next

                    fhaTecnicoSel = ObtenerFechaHoraOnClickDataGridView()
                    m_strCodTecnico = m_listRows(intRowNum)._Code

            End Select

        End If
    End Sub
    Private Sub timerMensajeTick()
        pnlMensaje.Visible = False
        lblMensaje.Text = String.Empty
        lblMensaje.Enabled = False

        timerMensaje.Stop()
        timerMensaje.Dispose()

    End Sub

    Private Sub MostrarMensaje(ByVal p_strMensaje As String)


        Try
            pnlMensaje.Visible = True
            lblMensaje.Text = p_strMensaje
            pnlMensaje.BackColor = Color.LightCoral
            lblMensaje.Enabled = True

            timerMensaje.Interval = 3000
            AddHandler timerMensaje.Tick, AddressOf timerMensajeTick
            timerMensaje.Start()
        Catch ex As Exception
            Throw
        End Try
    End Sub

    'Private Sub ActualizaInfoCelda()

    '    If dtgOcupacion.CurrentRowIndex <> -1 Then

    '        intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '        If m_listRows(intRowNum)._Rol = "A" Then

    '            m_oCeldaAsesorAnt._FilNum = m_oCeldaAsesor._FilNum
    '            m_oCeldaAsesorAnt._ColNum = m_oCeldaAsesor._ColNum
    '            m_oCeldaAsesorAnt._Equipo = m_oCeldaAsesor._Equipo
    '            m_oCeldaAsesorAnt._Texto = m_oCeldaAsesor._Texto
    '            m_oCeldaAsesorAnt._Rol = m_oCeldaAsesor._Rol

    '            If m_oCeldaAsesor._FilNum = dtgOcupacion.CurrentCell.RowNumber AndAlso
    '                m_oCeldaAsesor._ColNum = dtgOcupacion.CurrentCell.ColumnNumber Then
    '                m_oCeldaAsesor._FilNum = 0
    '                m_oCeldaAsesor._ColNum = 0

    '                m_oCeldaAsesor._Equipo = String.Empty
    '                m_oCeldaAsesor._Texto = String.Empty
    '                m_oCeldaAsesor._Rol = String.Empty

    '            Else
    '                ''m_oCeldaAsesor._FilNum = dgv_AgendaCitas.CurrentCell.RowIndex
    '                '' m_oCeldaAsesor._ColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex
    '                m_oCeldaAsesor._FilNum = dtgOcupacion.CurrentCell.RowNumber
    '                m_oCeldaAsesor._ColNum = dtgOcupacion.CurrentCell.ColumnNumber

    '                m_oCeldaAsesor._Equipo = m_listRows(intRowNum)._Equipo
    '                m_oCeldaAsesor._Texto = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
    '                '' m_oCeldaAsesor._Texto = dgv_AgendaCitas.Rows(dgv_AgendaCitas.CurrentCell.RowIndex).Cells(dgv_AgendaCitas.CurrentCell.ColumnIndex).Value

    '                m_oCeldaAsesor._Rol = m_listRows(intRowNum)._Rol
    '                m_intEspacioAse = ObtenerCantidadEspaciosAgenda(m_listRows(intRowNum)._Intervalo)

    '            End If

    '        ElseIf m_listRows(intRowNum)._Rol = "T" Then

    '            m_oCeldaTecnicoAnt._FilNum = m_oCeldaTecnico._FilNum
    '            m_oCeldaTecnicoAnt._ColNum = m_oCeldaTecnico._ColNum
    '            m_oCeldaTecnicoAnt._Equipo = m_oCeldaTecnico._Equipo
    '            m_oCeldaTecnicoAnt._Texto = m_oCeldaTecnico._Texto
    '            m_oCeldaTecnicoAnt._Rol = m_oCeldaTecnico._Rol

    '            If m_oCeldaTecnico._FilNum = dtgOcupacion.CurrentCell.RowNumber AndAlso
    '                m_oCeldaTecnico._ColNum = dtgOcupacion.CurrentCell.ColumnNumber Then

    '                m_oCeldaTecnico._FilNum = 0
    '                m_oCeldaTecnico._ColNum = 0


    '                m_oCeldaTecnico._Equipo = String.Empty
    '                m_oCeldaTecnico._Texto = String.Empty
    '                m_oCeldaTecnico._Rol = String.Empty

    '            Else
    '                m_oCeldaTecnico._FilNum = dtgOcupacion.CurrentCell.RowNumber
    '                m_oCeldaTecnico._ColNum = dtgOcupacion.CurrentCell.ColumnNumber


    '                m_oCeldaTecnico._Equipo = m_listRows(intRowNum)._Equipo
    '                m_oCeldaTecnico._Texto = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
    '                m_oCeldaTecnico._Rol = m_listRows(intRowNum)._Rol

    '                If Not String.IsNullOrEmpty(m_listRows(intRowNum)._ServRap) Then
    '                    m_intServRapido = ObtenerCantidadEspaciosAgenda(m_listRows(intRowNum)._ServRap)
    '                Else
    '                    m_intServRapido = 0
    '                End If

    '                'If (m_listRows(intRowNum)._Intervalo > 15) Then
    '                '    m_intEspacioTec = ObtenerCantidadEspaciosAgenda(m_listRows(intRowNum)._Intervalo)
    '                'End If

    '            End If

    '        End If
    '    End If

    'End Sub

    ''' <summary>
    ''' Actualiza informacion celda DatagridView
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizaInfoCeldaDataGridView()

        If dgv_AgendaCitas.CurrentRow.Index <> -1 Then

            intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
            If m_listRows(intRowNum)._Rol = "A" Then

                m_oCeldaAsesorAnt._FilNum = m_oCeldaAsesor._FilNum
                m_oCeldaAsesorAnt._ColNum = m_oCeldaAsesor._ColNum
                m_oCeldaAsesorAnt._Equipo = m_oCeldaAsesor._Equipo
                m_oCeldaAsesorAnt._Texto = m_oCeldaAsesor._Texto
                m_oCeldaAsesorAnt._Rol = m_oCeldaAsesor._Rol

                If m_oCeldaAsesor._FilNum = dgv_AgendaCitas.CurrentCell.RowIndex AndAlso
                    m_oCeldaAsesor._ColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex Then
                    m_oCeldaAsesor._FilNum = 0
                    m_oCeldaAsesor._ColNum = 0

                    m_oCeldaAsesor._Equipo = String.Empty
                    m_oCeldaAsesor._Texto = String.Empty
                    m_oCeldaAsesor._Rol = String.Empty

                Else
                    m_oCeldaAsesor._FilNum = dgv_AgendaCitas.CurrentCell.RowIndex
                    m_oCeldaAsesor._ColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex


                    m_oCeldaAsesor._Equipo = m_listRows(intRowNum)._Equipo

                    m_oCeldaAsesor._Texto = dgv_AgendaCitas.Rows(dgv_AgendaCitas.CurrentCell.RowIndex).Cells(dgv_AgendaCitas.CurrentCell.ColumnIndex).Value

                    m_oCeldaAsesor._Rol = m_listRows(intRowNum)._Rol
                    m_intEspacioAse = ObtenerCantidadEspaciosAgenda(m_listRows(intRowNum)._Intervalo)

                End If

            ElseIf m_listRows(intRowNum)._Rol = "T" Then

                m_oCeldaTecnicoAnt._FilNum = m_oCeldaTecnico._FilNum
                m_oCeldaTecnicoAnt._ColNum = m_oCeldaTecnico._ColNum
                m_oCeldaTecnicoAnt._Equipo = m_oCeldaTecnico._Equipo
                m_oCeldaTecnicoAnt._Texto = m_oCeldaTecnico._Texto
                m_oCeldaTecnicoAnt._Rol = m_oCeldaTecnico._Rol

                If m_oCeldaTecnico._FilNum = dgv_AgendaCitas.CurrentCell.RowIndex AndAlso
                    m_oCeldaTecnico._ColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex Then

                    m_oCeldaTecnico._FilNum = 0
                    m_oCeldaTecnico._ColNum = 0


                    m_oCeldaTecnico._Equipo = String.Empty
                    m_oCeldaTecnico._Texto = String.Empty
                    m_oCeldaTecnico._Rol = String.Empty

                Else
                    m_oCeldaTecnico._FilNum = dgv_AgendaCitas.CurrentCell.RowIndex
                    m_oCeldaTecnico._ColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex


                    m_oCeldaTecnico._Equipo = m_listRows(intRowNum)._Equipo
                    m_oCeldaTecnico._Texto = dgv_AgendaCitas.Rows(dgv_AgendaCitas.CurrentCell.RowIndex).Cells(dgv_AgendaCitas.CurrentCell.ColumnIndex).Value
                    m_oCeldaTecnico._Rol = m_listRows(intRowNum)._Rol

                    If Not String.IsNullOrEmpty(m_listRows(intRowNum)._ServRap) Then
                        m_intServRapido = ObtenerCantidadEspaciosAgenda(m_listRows(intRowNum)._ServRap)
                    Else
                        m_intServRapido = 0
                    End If

                    'If (m_listRows(intRowNum)._Intervalo > 15) Then
                    '    m_intEspacioTec = ObtenerCantidadEspaciosAgenda(m_listRows(intRowNum)._Intervalo)
                    'End If

                End If

            End If
        End If

    End Sub

    'Private Sub dtgOcupacion_OneClick(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim strCurrenCell As String


    '        If m_tipoAgendaCargar = TipoDeAgenda.Equipos Then

    '            ''ActualizaInfoCelda()
    '            If ValidarSeleccion() Then
    '                Exit Sub
    '            End If
    '            MarcarOcupacion()


    '        ElseIf m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
    '            Dim oCell As DataGridCell
    '            oCell = dtgOcupacion.CurrentCell

    '            strCurrenCell = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
    '            intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '            intColNum = dtgOcupacion.CurrentCell.ColumnNumber


    '            Select Case strCurrenCell
    '                Case String.Empty

    '                    If blnSeleccionTec = False Then
    '                        intFAntTec = 0
    '                        intCAntTec = 0
    '                        intFNueTec = oCell.RowNumber
    '                        intCNueTec = oCell.ColumnNumber
    '                        dtgOcupacion.Item(intFNueTec, intCNueTec) = "***"
    '                        blnSeleccionTec = True
    '                    Else
    '                        intFAntTec = intFNueTec
    '                        intCAntTec = intCNueTec

    '                        dtgOcupacion.Item(intFAntTec, intCAntTec) = ""
    '                        intFNueTec = oCell.RowNumber
    '                        intCNueTec = oCell.ColumnNumber
    '                        dtgOcupacion.Item(intFNueTec, intCNueTec) = "***"
    '                    End If

    '                    fhaAsesorSel = ObtenerFechaHoraOnClick()
    '                    m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                    m_strCodAsesor = m_listRows(intRowNum)._Code
    '                Case "***"
    '                    intFAnt = intFNue
    '                    intCAnt = intCNue

    '                    intFNue = oCell.RowNumber
    '                    intCNue = oCell.ColumnNumber

    '                    dtgOcupacion.Item(intFNue, intCNue) = String.Empty

    '                    fhaAsesorSel = Nothing
    '                    m_strCodAgenda = String.Empty
    '                    m_strCodAsesor = String.Empty

    '            End Select
    '        End If

    '    Catch ex As Exception

    '    End Try
    'End Sub


    Private Sub dgv_AgendaCitas_Click(sender As System.Object, e As System.EventArgs) Handles dgv_AgendaCitas.Click
        Try
            Dim strCurrenCell As String

            If dgv_AgendaCitas.CurrentCell IsNot Nothing Then
                If dgv_AgendaCitas.CurrentCell.ColumnIndex <> 2 Then

                    If m_tipoAgendaCargar = TipoDeAgenda.Equipos Then

                        ActualizaInfoCeldaDataGridView()
                        If ValidarSeleccionDataGridView() Then
                            Exit Sub
                        End If
                        MarcarOcupacionDataGridView()


                    ElseIf m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
                        Dim oCell As DataGridViewCell
                        oCell = dgv_AgendaCitas.CurrentCell

                        strCurrenCell = dgv_AgendaCitas.Item(dgv_AgendaCitas.CurrentCell.ColumnIndex, dgv_AgendaCitas.CurrentCell.RowIndex).Value.ToString()
                        intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                        intColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex


                        Select Case strCurrenCell
                            Case String.Empty

                                If blnSeleccionTec = False Then
                                    intFAntTec = 0
                                    intCAntTec = 0
                                    intFNueTec = oCell.RowIndex
                                    intCNueTec = oCell.ColumnIndex
                                    dgv_AgendaCitas.Item(intCNueTec, intFNueTec).Value = "***"
                                    blnSeleccionTec = True
                                Else
                                    intFAntTec = intFNueTec
                                    intCAntTec = intCNueTec

                                    dgv_AgendaCitas.Item(intCAntTec, intFAntTec).Value = ""
                                    intFNueTec = oCell.RowIndex
                                    intCNueTec = oCell.ColumnIndex
                                    dgv_AgendaCitas.Item(intCNueTec, intFNueTec).Value = "***"
                                End If

                                fhaAsesorSel = ObtenerFechaHoraOnClickDataGridView()
                                m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
                                m_strCodAsesor = m_listRows(intRowNum)._Code
                            Case "***"
                                intFAnt = intFNue
                                intCAnt = intCNue

                                intFNue = oCell.RowIndex
                                intCNue = oCell.ColumnIndex

                                dgv_AgendaCitas.Item(intCNue, intFNue).Value = String.Empty

                                fhaAsesorSel = Nothing
                                m_strCodAgenda = String.Empty
                                m_strCodAsesor = String.Empty

                        End Select
                    End If

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    ' Private Function ValidarSeleccion() As Boolean
    '    Try
    '        Dim l_blnResutl As Boolean = False

    '        If dtgOcupacion.CurrentRowIndex <> -1 Then

    '            intRowNum = dtgOcupacion.CurrentCell.RowNumber

    '            If m_listRows(intRowNum)._Rol = "A" Then

    '                If Not String.IsNullOrEmpty(m_oCeldaTecnico._Equipo) Then
    '                    If m_oCeldaTecnico._Equipo <> m_oCeldaAsesor._Equipo AndAlso
    '                        Not String.IsNullOrEmpty(m_oCeldaAsesor._Equipo) Then
    '                        MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasTecnicoAsesor)

    '                        m_oCeldaAsesor._FilNum = m_oCeldaAsesorAnt._FilNum
    '                        m_oCeldaAsesor._ColNum = m_oCeldaAsesorAnt._ColNum
    '                        m_oCeldaAsesor._Equipo = m_oCeldaAsesorAnt._Equipo
    '                        m_oCeldaAsesor._Texto = m_oCeldaAsesorAnt._Texto
    '                        m_oCeldaAsesor._Rol = m_oCeldaAsesorAnt._Rol

    '                        Return True
    '                    ElseIf m_oCeldaTecnico._ColNum <> 0 Then

    '                        If m_oCeldaAsesor._ColNum + m_intEspacioAse - 1 >= m_oCeldaTecnico._ColNum Then
    '                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasHoraRecepcion)

    '                            m_oCeldaAsesor._FilNum = m_oCeldaAsesorAnt._FilNum
    '                            m_oCeldaAsesor._ColNum = m_oCeldaAsesorAnt._ColNum
    '                            m_oCeldaAsesor._Equipo = m_oCeldaAsesorAnt._Equipo
    '                            m_oCeldaAsesor._Texto = m_oCeldaAsesorAnt._Texto
    '                            m_oCeldaAsesor._Rol = m_oCeldaAsesorAnt._Rol

    '                            Return True
    '                        End If

    '                    End If
    '                End If


    '            ElseIf m_listRows(intRowNum)._Rol = "T" Then
    '                If m_oCeldaTecnico._ColNum <> 0 AndAlso
    '                    m_oCeldaTecnico._FilNum <> 0 Then


    '                    If m_oCeldaTecnico._Equipo <> m_oCeldaAsesor._Equipo AndAlso
    '                       Not String.IsNullOrEmpty(m_oCeldaAsesor._Equipo) Then
    '                        MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasTecnicoAsesor)

    '                        m_oCeldaTecnico._FilNum = m_oCeldaTecnicoAnt._FilNum
    '                        m_oCeldaTecnico._ColNum = m_oCeldaTecnicoAnt._ColNum
    '                        m_oCeldaTecnico._Equipo = m_oCeldaTecnicoAnt._Equipo
    '                        m_oCeldaTecnico._Texto = m_oCeldaTecnicoAnt._Texto
    '                        m_oCeldaTecnico._Rol = m_oCeldaTecnicoAnt._Rol

    '                        Return True
    '                    ElseIf m_oCeldaAsesor._ColNum <> 0 Then
    '                        If m_oCeldaTecnico._ColNum <= m_oCeldaAsesor._ColNum + m_intEspacioAse - 1 Then
    '                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasHoraServicio)

    '                            m_oCeldaTecnico._FilNum = m_oCeldaTecnicoAnt._FilNum
    '                            m_oCeldaTecnico._ColNum = m_oCeldaTecnicoAnt._ColNum
    '                            m_oCeldaTecnico._Equipo = m_oCeldaTecnicoAnt._Equipo
    '                            m_oCeldaTecnico._Texto = m_oCeldaTecnicoAnt._Texto
    '                            m_oCeldaTecnico._Rol = m_oCeldaTecnicoAnt._Rol
    '                            Return True
    '                        End If
    '                    End If
    '                End If

    '            End If
    '        End If
    '        Return l_blnResutl

    '    Catch ex As Exception

    '    End Try

    'End Function


    ''' <summary>
    ''' Valida Seleccion en DataGridView
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidarSeleccionDataGridView() As Boolean
        Try
            Dim l_blnResutl As Boolean = False

            If dgv_AgendaCitas.CurrentRow.Index <> -1 Then

                intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex

                If m_listRows(intRowNum)._Rol = "A" Then

                    If Not String.IsNullOrEmpty(m_oCeldaTecnico._Equipo) Then
                        If m_oCeldaTecnico._Equipo <> m_oCeldaAsesor._Equipo AndAlso
                            Not String.IsNullOrEmpty(m_oCeldaAsesor._Equipo) Then
                            dgv_AgendaCitas.Item(m_oCeldaTecnicoAnt._ColNum, m_oCeldaTecnicoAnt._FilNum).Style.SelectionBackColor = Color.Transparent
                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasTecnicoAsesor)

                            m_oCeldaAsesor._FilNum = m_oCeldaAsesorAnt._FilNum
                            m_oCeldaAsesor._ColNum = m_oCeldaAsesorAnt._ColNum
                            m_oCeldaAsesor._Equipo = m_oCeldaAsesorAnt._Equipo
                            m_oCeldaAsesor._Texto = m_oCeldaAsesorAnt._Texto
                            m_oCeldaAsesor._Rol = m_oCeldaAsesorAnt._Rol

                            Return True
                        ElseIf m_oCeldaTecnico._ColNum <> 0 Then

                            If m_oCeldaAsesor._ColNum + m_intEspacioAse - 1 >= m_oCeldaTecnico._ColNum Then
                                dgv_AgendaCitas.Item(m_oCeldaTecnicoAnt._ColNum, m_oCeldaTecnicoAnt._FilNum).Style.SelectionBackColor = Color.Transparent
                                MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasHoraRecepcion)

                                m_oCeldaAsesor._FilNum = m_oCeldaAsesorAnt._FilNum
                                m_oCeldaAsesor._ColNum = m_oCeldaAsesorAnt._ColNum
                                m_oCeldaAsesor._Equipo = m_oCeldaAsesorAnt._Equipo
                                m_oCeldaAsesor._Texto = m_oCeldaAsesorAnt._Texto
                                m_oCeldaAsesor._Rol = m_oCeldaAsesorAnt._Rol

                                Return True
                            End If

                        End If
                    End If


                ElseIf m_listRows(intRowNum)._Rol = "T" Then
                    If m_oCeldaTecnico._ColNum <> 0 AndAlso
                        m_oCeldaTecnico._FilNum <> 0 Then


                        If m_oCeldaTecnico._Equipo <> m_oCeldaAsesor._Equipo AndAlso
                           Not String.IsNullOrEmpty(m_oCeldaAsesor._Equipo) Then
                            dgv_AgendaCitas.Item(m_oCeldaTecnicoAnt._ColNum, m_oCeldaTecnicoAnt._FilNum).Style.SelectionBackColor = Color.Transparent
                            MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasTecnicoAsesor)

                            m_oCeldaTecnico._FilNum = m_oCeldaTecnicoAnt._FilNum
                            m_oCeldaTecnico._ColNum = m_oCeldaTecnicoAnt._ColNum
                            m_oCeldaTecnico._Equipo = m_oCeldaTecnicoAnt._Equipo
                            m_oCeldaTecnico._Texto = m_oCeldaTecnicoAnt._Texto
                            m_oCeldaTecnico._Rol = m_oCeldaTecnicoAnt._Rol

                            Return True
                        ElseIf m_oCeldaAsesor._ColNum <> 0 Then
                            If m_oCeldaTecnico._ColNum <= m_oCeldaAsesor._ColNum + m_intEspacioAse - 1 Then
                                dgv_AgendaCitas.Item(m_oCeldaTecnicoAnt._ColNum, m_oCeldaTecnicoAnt._FilNum).Style.SelectionBackColor = Color.Transparent
                                MostrarMensaje(My.Resources.Resource.MensajeErrorListaCitasHoraServicio)

                                m_oCeldaTecnico._FilNum = m_oCeldaTecnicoAnt._FilNum
                                m_oCeldaTecnico._ColNum = m_oCeldaTecnicoAnt._ColNum
                                m_oCeldaTecnico._Equipo = m_oCeldaTecnicoAnt._Equipo
                                m_oCeldaTecnico._Texto = m_oCeldaTecnicoAnt._Texto
                                m_oCeldaTecnico._Rol = m_oCeldaTecnicoAnt._Rol
                                Return True
                            End If
                        End If
                    End If

                End If
            End If
            Return l_blnResutl

        Catch ex As Exception

        End Try

    End Function

    'Private Sub dtgOcupacion_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Dim strCitas As String
    '    Dim strCantCitas As String()
    '    Dim intCantCitas As Integer
    '    ' Dim intCodigoAgenda As Integer
    '    Dim strNombreAgenda As String

    '    ' Dim strSerie As String = String.Empty
    '    ' Dim strNumCita As String = String.Empty
    '    Dim splitCita As String()

    '    Dim strAno As String
    '    Dim strMes As String
    '    Dim strDia As String
    '    Dim strHora As String
    '    Dim strHoraFull As String
    '    Dim strMinutos As String
    '    Dim codAsesor As String
    '    Dim codTecnico As String
    '    Dim strSucursar As String
    '    Dim strAgenda As String
    '    Dim intColNum As Integer
    '    Dim intRowNum As Integer
    '    Dim dtFechaYHora As Date
    '    Dim dtFhaHoraAsesor As Date
    '    Dim dtFhaHoraTecnico As Date

    '    Try

    '        Select Case m_tipoAgendaCargar
    '            Case TipoDeAgenda.Agenda
    '                If dtgOcupacion.CurrentRowIndex <> -1 Then

    '                    strCitas = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
    '                    strCantCitas = strCitas.Split(",")
    '                    intCantCitas = strCantCitas.Length

    '                    intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '                    intColNum = dtgOcupacion.CurrentCell.ColumnNumber - 3


    '                    If String.IsNullOrEmpty(strCitas) OrElse
    '                        strCitas.Equals("***") OrElse
    '                        strCitas.Equals(".") Then   'Cuando no hay cita para esa hora

    '                        'strAno = m_dtFecha.Year()
    '                        'strMes = m_dtFecha.Month()
    '                        'strDia = m_dtFecha.Day()

    '                        'strHoraFull = m_listColums(intColNum)._horaFull
    '                        'strHora = m_listColums(intColNum)._Hora
    '                        'strMinutos = m_listColums(intColNum)._Minutos
    '                        'codTecnico = m_listRows(intRowNum)._Code
    '                        'strAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                        'strSucursar = m_strCodSucursal
    '                        intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '                        m_strCodAsesor = m_listRows(intRowNum)._Code

    '                        RaiseEvent eCargaCitaNueva_PorAgenda(fhaAsesorSel, m_strCodAsesor, m_strCodTecnico, m_strCodSucursal, m_strCodAgenda) ' 2

    '                    Else

    '                        splitCita = strCitas.Split("-")
    '                        m_strSerieCita = splitCita(0)
    '                        m_strNumCita = splitCita(1)

    '                        intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '                        m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                        m_strCodAsesor = m_listRows(intRowNum)._Code


    '                        RaiseEvent eCargaCitaExiste(m_strSerieCita, m_strNumCita, m_strCodAgenda) '1

    '                    End If
    '                End If

    '            Case TipoDeAgenda.Equipos
    '                If dtgOcupacion.CurrentRowIndex <> -1 Then

    '                    strCitas = dtgOcupacion.Item(dtgOcupacion.CurrentCell)
    '                    'strCantCitas = strCitas.Split(",")
    '                    'intCantCitas = strCantCitas.Length

    '                    intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '                    intColNum = dtgOcupacion.CurrentCell.ColumnNumber - 3


    '                    If String.IsNullOrEmpty(strCitas) OrElse
    '                    strCitas.Equals(".") OrElse
    '                    strCitas.Equals("***") Then 'Cuando no hay cita para esa hora

    '                        dtFechaYHora = ObtenerFechaHoraOnClick()
    '                        intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '                        ' m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                        ' m_strCodAsesor = m_listRows(intRowNum)._Code

    '                        ' RaiseEvent eCargaCitaNueva_PorEquipos(fhaAsesorSel, fhaTecnicoSel, m_strCodAsesor, m_strCodTecnico, m_strCodSucursal, m_strCodAgenda)
    '                    Else
    '                        If ConsultaEsCita(strCitas) Then

    '                            splitCita = strCitas.Split("-")
    '                            m_strSerieCita = splitCita(0)
    '                            m_strNumCita = splitCita(1)

    '                            intRowNum = dtgOcupacion.CurrentCell.RowNumber
    '                            m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
    '                            m_strCodAsesor = m_listRows(intRowNum)._Code

    '                            RaiseEvent eCargaCitaExiste(m_strSerieCita, m_strNumCita, m_strCodAgenda)

    '                        End If


    '                    End If
    '                End If

    '        End Select



    '    Catch ex As System.Exception
    '        MessageBox.Show(ex.Message)
    '    End Try


    'End Sub

    Private Sub dgv_AgendaCitas_DoubleClick(sender As System.Object, e As System.EventArgs) Handles dgv_AgendaCitas.DoubleClick
        Dim strCitas As String = String.Empty
        Dim strCantCitas As String()
        Dim intCantCitas As Integer
        Dim splitCita As String()
        Dim intColNum As Integer
        Dim intRowNum As Integer
        Dim dtFechaYHora As Date

        Try
            If dgv_AgendaCitas.CurrentCell IsNot Nothing Then
                Select Case m_tipoAgendaCargar

                    Case TipoDeAgenda.Agenda
                        If dgv_AgendaCitas.CurrentCell.RowIndex <> -1 Then

                            strCitas = dgv_AgendaCitas.Item(dgv_AgendaCitas.CurrentCell.ColumnIndex, dgv_AgendaCitas.CurrentCell.RowIndex).Value.ToString
                            strCitas = ObtenerNumeroCita(strCitas)
                            strCantCitas = strCitas.Split(",")
                            intCantCitas = strCantCitas.Length

                            'intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                            'intColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex - 3

                            If String.IsNullOrEmpty(strCitas) OrElse
                                strCitas.Equals("***") OrElse
                                strCitas.Equals(".") Then   'Cuando no hay cita para esa hora

                                intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                                m_strCodAsesor = m_listRows(intRowNum)._Code
                                Me.WindowState = FormWindowState.Minimized
                                If oVersionModuloCita = VersionModuloCita.Estandar Then
                                    RaiseEvent eCargaCitaNueva_PorAgenda(fhaAsesorSel, m_strCodAsesor, m_strCodTecnico, m_strCodSucursal, m_strCodAgenda) ' 2
                                Else
                                    ConstructorCitas.CrearInstanciaFormulario(m_strCodSucursal, m_strCodAgenda, fhaAsesorSel)
                                End If
                            Else

                                splitCita = strCitas.Split("-")
                                m_strSerieCita = splitCita(0)
                                m_strNumCita = splitCita(1)

                                intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                                m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
                                m_strCodAsesor = m_listRows(intRowNum)._Code
                                Me.WindowState = FormWindowState.Minimized
                                If oVersionModuloCita = VersionModuloCita.Estandar Then
                                    RaiseEvent eCargaCitaExiste(m_strSerieCita.Trim(), m_strNumCita.Trim(), m_strCodAgenda)
                                Else
                                    ConstructorCitas.CrearInstanciaFormularioExistente(m_strSerieCita, m_strNumCita)
                                End If
                            End If
                        End If

                    Case TipoDeAgenda.Equipos
                        If dgv_AgendaCitas.CurrentCell.RowIndex <> -1 AndAlso dgv_AgendaCitas.CurrentCell.ColumnIndex >= 7 Then

                            strCitas = dgv_AgendaCitas.Item(dgv_AgendaCitas.CurrentCell.ColumnIndex, dgv_AgendaCitas.CurrentCell.RowIndex).Value.ToString()
                            strCitas = ObtenerNumeroCita(strCitas)


                            'intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                            'intColNum = dgv_AgendaCitas.CurrentCell.ColumnIndex - 3

                            If String.IsNullOrEmpty(strCitas) OrElse
                            strCitas.Equals(".") OrElse
                            strCitas.Equals("***") Then 'Cuando no hay cita para esa hora

                                dtFechaYHora = ObtenerFechaHoraOnClickDataGridView()
                                intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                            Else
                                If ConsultaEsCita(strCitas) Then

                                    splitCita = strCitas.Split("-")
                                    m_strSerieCita = splitCita(0)
                                    m_strNumCita = splitCita(1)

                                    intRowNum = dgv_AgendaCitas.CurrentCell.RowIndex
                                    m_strCodAgenda = m_listRows(intRowNum)._DocEntryAgenda
                                    m_strCodAsesor = m_listRows(intRowNum)._Code

                                    Me.WindowState = FormWindowState.Minimized
                                    If oVersionModuloCita = VersionModuloCita.Estandar Then
                                        RaiseEvent eCargaCitaExiste(m_strSerieCita.Trim(), m_strNumCita.Trim(), m_strCodAgenda)
                                    Else
                                        ConstructorCitas.CrearInstanciaFormularioExistente(m_strSerieCita, m_strNumCita)
                                    End If
                                End If
                            End If
                        End If

                End Select
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Function ObtenerNumeroCita(ByVal strValor As String) As String
        Dim strArreglo As String()
        Try
            Select Case strValor
                Case String.Empty
                    Return String.Empty
                Case Nothing
                    Return String.Empty
                Case "."
                    Return String.Empty
                Case "***"
                    Return String.Empty
                Case "n/a"
                    Return String.Empty
                Case Else
                    If strValor.Contains("-") Then
                        If strValor.Contains("/") Then
                            strValor = strValor.Split("/")(1).Trim
                        End If
                    Else
                        Return String.Empty
                    End If
            End Select

            Return strValor
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strValor
        End Try
    End Function

    Private Function ObtenerValorCeldaDataTable(ByVal intColumnaGrid As Integer, ByVal intFilaGrid As Integer) As String
        Dim strValor As String = String.Empty
        Try
            If intColumnaGrid > -1 AndAlso intFilaGrid > -1 Then
                If dtAgenda IsNot Nothing AndAlso dtAgenda.Rows.Count > 0 Then
                    strValor = dtAgenda.Rows.Item(intFilaGrid).Item(intColumnaGrid).ToString()
                End If
            End If

            Return strValor
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strValor
        End Try
    End Function

    ''' <summary>
    ''' Obtiene hora
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ObtenerFechaHoraOnClickDataGridView() As Date
        Dim l_fhaResult As Date
        Dim intDias As Integer
        Dim dtFecha As Date
        Dim m_listColums2 As List(Of infoColumn)

        Dim strHoraFull As String
        Dim strHora As String
        Dim strMinutos As String
        Dim intColNumber As Integer

        If dgv_AgendaCitas.CurrentCell.RowIndex <> -1 Then

            Dim int As Integer = dgv_AgendaCitas.Columns.Count
            intColNumber = dgv_AgendaCitas.CurrentCell.ColumnIndex - 8

            dtFecha = dtpFecha.Value
            m_listColums2 = m_listColums
            strHoraFull = m_listColums(intColNumber)._horaFull
            strHora = m_listColums(intColNumber)._Hora
            strMinutos = m_listColums(intColNumber)._Minutos

            dtFecha = dtFecha.AddHours(strHora)
            dtFecha = dtFecha.AddMinutes(strMinutos)


            l_fhaResult = dtFecha

            Return l_fhaResult
        End If

    End Function

    'Private Sub ObtenerDatosCita(ByRef p_strSerie As String, ByRef p_strNumCita As String)

    '    Dim datFecha As Date
    '    Dim datHora As Date
    '    Dim intDias As Integer


    '    Try
    '        If dtgOcupacion.CurrentRowIndex <> -1 Then

    '            intDias = dtgOcupacion.CurrentCell.ColumnNumber
    '            datFecha = dtpFecha.Value.AddDays((intDias - 2))
    '            datHora = CDate("01-01-1900" & " " & dtgOcupacion.Item(dtgOcupacion.CurrentRowIndex, 1))

    '            datFecha = New Date(datFecha.Year, datFecha.Month, datFecha.Day, datHora.Hour, datHora.Minute, 0)
    '        End If

    '    Catch ex As System.Exception
    '        MessageBox.Show(ex.Message)
    '    End Try

    'End Sub

    Private Sub btnSiguienteDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteDay.Click

        m_dtFecha = m_dtFecha.AddDays(1)
        dtpFecha.Value = m_dtFecha

        dgv_AgendaCitas.DataSource = Nothing
        dgv_AgendaCitas.Rows.Clear()





        LoadConsultaOcupacion()

    End Sub

    Private Sub btnAnteriorDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorDay.Click

        m_dtFecha = m_dtFecha.AddDays(-1)
        dtpFecha.Value = m_dtFecha
        dgv_AgendaCitas.DataSource = Nothing
        dgv_AgendaCitas.Rows.Clear()
        LoadConsultaOcupacion()

    End Sub

    Private Sub btnAnteriorWeek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnteriorWeek.Click

        m_dtFecha = m_dtFecha.AddDays(-7)
        dtpFecha.Value = m_dtFecha
        dgv_AgendaCitas.DataSource = Nothing
        dgv_AgendaCitas.Rows.Clear()
        LoadConsultaOcupacion()

    End Sub

    Private Sub btnSiguienteWeek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguienteWeek.Click

        m_dtFecha = m_dtFecha.AddDays(7)
        dtpFecha.Value = m_dtFecha
        dgv_AgendaCitas.DataSource = Nothing
        dgv_AgendaCitas.Rows.Clear()
        LoadConsultaOcupacion()

    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        Try

            m_dtFecha = dtpFecha.Value
            m_strCodSucursal = cboAgenda.SelectedValue
            dgv_AgendaCitas.DataSource = Nothing
            dgv_AgendaCitas.Rows.Clear()
            LoadConsultaOcupacion()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub HandleTimerTick()
        Try
            Timer1.Stop()
            Timer1.Dispose()
            Me.Close()
            m_oApplication.StatusBar.SetText(My.Resources.Resource.MensajeListaCitasAgendaCerradaTiempo, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Sub frmListaCitas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If oVersionModuloCita = VersionModuloCita.Estandar Then
                Dim l_intWidth As Integer
                Dim l_intHeigth As Integer

                If IsNothing(m_dtFecha) Then
                    dtpFecha.Value = Utilitarios.EjecutarConsulta("SELECT GETDATE()", m_oCompany.CompanyDB, m_oCompany.Server)
                Else
                    dtpFecha.Value = m_dtFecha
                End If

                Call CargarCombos()

                m_strNombreBDTaller = ObtenerNombreDBTaller(m_strCodSucursal)
                m_strUsarTallerSAP = ObtenerConfiguracionTallerInterno()


                DefinirColumnasGrid()
                CrearTablaAgenda()

                '********************************
                If m_blnVersion9 = False Then
                    Timer1.Interval = 60000
                    AddHandler Timer1.Tick, AddressOf HandleTimerTick
                    Timer1.Start()
                End If
                '********************************

                DefinirColumnas()
                LoadEstiloGrid()

                LoadEstiloGridCitasProblem()
                CreateDataTableCitasProblemas()
                CargarAgenda()
                LlenarReservacion()
                LlenarBloqueodeMecanicos()
                dgv_AgendaCitas.DataSource = dtAgenda
                LlenarOcupacion()
                LoadCitasProblemas()

                ActualizaTextoFecha()
                ''dtgOcupacion.DataSource = dtAgenda

                ManejorDataGridView()


                dtgvCitasReasignar.DataSource = dtCitasProbl
                '' dtgOcupacion.AllowSorting = False

                CrearTablaAgendaNombres()
                CargarAgendaNombres()
                '' dtgNombres.DataSource = dtNombres
                LoadEstiloGrid2()

                If m_blnInterno Then
                    l_intWidth = 1100
                    l_intHeigth = 600

                    lblCitasReasignar.Anchor = AnchorStyles.Top & AnchorStyles.Left

                    Me.Size = New Size(l_intWidth, l_intHeigth)
                    Me.MaximizeBox = True

                    cargarSkin()

                End If

                '  dtgOcupacion.Columns(1).Locked = True
                dgv_AgendaCitas.Focus()
                AjustarCeldas()
            Else
                CargarAgenda(True)
            End If
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de crear, graficar y refrescar la agenda
    ''' </summary>
    ''' <param name="Inicializar">Variable que indica si se está inicializando la agenda o actualizando la agenda</param>
    ''' <remarks></remarks>
    Private Sub CargarAgenda(ByVal Inicializar As Boolean)
        Dim HorarioSucursal As Dictionary(Of DayOfWeek, Horario)
        Dim ListaIntervalos As List(Of DateTime)
        Dim NumeroAtributos As Integer
        Try
            LimpiarFormulario()
            If Inicializar Then
                dtpFecha.Value = DateTime.Today
                If m_blnVersion9 = False Then
                    Timer1.Interval = 60000
                    AddHandler Timer1.Tick, AddressOf HandleTimerTick
                    Timer1.Start()
                End If
                CargarComboBoxSucursal(m_strCodSucursal)
            End If

            lblFechaAct.Text = dtpFecha.Value
            'Obtiene el horario de la sucursal, en caso de estar configurado correctamente,
            'procese a cargar los distintos documentos de la fecha indicada e inicializar los distintos objetos (Grids, Tablas, Columnas)
            HorarioSucursal = New Dictionary(Of DayOfWeek, Horario)
            If ObtenerHorarioSucursal(m_strCodSucursal, HorarioSucursal) Then
                ListaIntervalos = ObtenerIntervalos(HorarioSucursal, dtpFecha.Value)
                If ListaIntervalos.Count > 0 Then
                    InicializarColumnas(ListaIntervalos)
                    dgv_AgendaCitas.DataSource = dtAgenda
                    CargarEmpleados(dtAgenda, m_tipoAgendaCargar, m_strCodSucursal, m_strNumGrupo)
                    NumeroAtributos = CalcularNoAtributos()
                    CargarEstilos(dgv_AgendaCitas, dtAgenda, m_tipoAgendaCargar, ListaIntervalos, NumeroAtributos)
                    LlenarAgenda(m_tipoAgendaCargar, HorarioSucursal, ListaIntervalos)
                    CargarConflictos(dtgvCitasReasignar, dtCitasProbl, m_tipoAgendaCargar)
                    AjustarCeldas()
                End If
            End If
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    Private Sub ManejoErroresAgenda(ByRef Excepcion As System.Exception)
        Dim DescripcionError As String = String.Empty
        Try
            DMS_Connector.Helpers.ManejoErroresWinForms(Excepcion)
            MostrarMensaje(Excepcion.Message)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CargarConflictos(ByRef GridConflictos As DataGridView, ByRef Conflictos As System.Data.DataTable, ByVal TipoAgenda As Integer)
        Dim enumerator As IDictionaryEnumerator
        Dim FilaNueva As DataRow
        Dim Llave As String = String.Empty
        Dim Valor As String = String.Empty
        Dim ColumnAgenda As New DataGridViewTextBoxColumn
        Dim ColumnCita As New DataGridViewTextBoxColumn

        Try
            ColumnAgenda.DataPropertyName = "Name"
            ColumnAgenda.HeaderText = "No."
            ColumnAgenda.Width = 200
            ColumnAgenda.ReadOnly = True
            ColumnAgenda.SortMode = False
            ColumnCita.DataPropertyName = "Citas"
            ColumnCita.HeaderText = My.Resources.Resource.Detalle
            ColumnCita.ReadOnly = True
            ColumnCita.SortMode = False
            ColumnCita.FillWeight = 500

            GridConflictos.Columns.Add(ColumnAgenda)
            GridConflictos.Columns.Add(ColumnCita)
            GridConflictos.RowTemplate.Height = 40

            enumerator = tableCitasProbl.GetEnumerator()
            While enumerator.MoveNext()
                Llave = enumerator.Key
                Valor = enumerator.Value
                FilaNueva = Conflictos.NewRow()
                FilaNueva("Name") = Llave
                FilaNueva("Citas") = Valor
                Conflictos.Rows.Add(FilaNueva)
            End While

            GridConflictos.Anchor = AnchorStyles.Top & AnchorStyles.Left
            GridConflictos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill

            GridConflictos.DataSource = Conflictos
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpiar los distintos datatables, datagrids y variables previo a la carga de los datos en la agenda
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LimpiarFormulario()
        Try
            dtAgenda = New System.Data.DataTable
            dtCitasProbl = New System.Data.DataTable
            tableCitasProbl.Clear()
            LimpiarDatosSeleccionAsesorTecnico()
            While dtgvCitasReasignar.Columns.Count > 0
                dtgvCitasReasignar.Columns.RemoveAt(0)
            End While
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los distintos empleados (Asesores y Técnicos) que se grafican como las líneas de la agenda
    ''' </summary>
    ''' <param name="Agenda">Tabla con la información de la agenda</param>
    ''' <param name="TipoAgenda">Tipo de agenda, si sencilla o por equipos</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="NumeroGrupo">Número de equipo</param>
    ''' <remarks></remarks>
    Private Sub CargarEmpleados(ByRef Agenda As System.Data.DataTable, ByVal TipoAgenda As Integer, ByVal CodigoSucursal As String, ByVal NumeroGrupo As String)
        Dim Empleados As System.Data.DataTable
        Dim Query As String = String.Empty
        Dim FilaNueva As DataRow
        Dim Posicion As String = String.Empty

        Try
            m_listRows = New List(Of infoRows)()
            Select Case TipoAgenda
                Case TipoDeAgenda.Agenda
                    Query = "SELECT DocEntry, DocNum, U_Agenda, (SELECT TOP 1 S1.Name FROM OHEM S0  WITH (nolock) INNER JOIN OHPS S1 ON S0.position = S1.posID WHERE S0.empID = U_CodAsesor) AS 'DscPosicion' from  [@SCGD_AGENDA] with (nolock) where U_Cod_Sucursal = '{0}' and U_EstadoLogico = 'Y'"
                    Query = String.Format(Query, CodigoSucursal)
                Case TipoDeAgenda.Equipos
                    Query = "Select HE.U_SCGD_Equipo, HE.U_SCGD_TipoEmp, HE.empID, HE.lastName + ' ' + HE.firstName as name, HE.U_SCGD_TiempServ , AG.U_Agenda, AG.DocEntry, AG.U_IntervaloCitas, T0.name AS 'DscPosicion'  " +
                                        " from OHEM HE with (nolock) " +
                                        " left outer JOIN [@SCGD_AGENDA] AG with (nolock) on AG.U_CodAsesor = HE.empID  " +
                                        " LEFT JOIN OHPS T0 ON HE.position = T0.posID " +
                                        " where(U_SCGD_Equipo Is Not null)" +
                                        " AND HE.U_SCGD_TipoEmp is not null " +
                                        " AND HE.U_SCGD_Equipo in  " +
                                        "	(Select HE.U_SCGD_Equipo from OHEM HE with (nolock) 	" +
                                        "			left outer join [@SCGD_AGENDA] AG on AG.U_CodAsesor = HE.empID	" +
                                        "			where U_SCGD_TipoEmp = 'A'" +
                                        "				and AG.U_EstadoLogico = 'Y' " +
                                        "				and U_SCGD_Equipo is not null " +
                                        "				and  HE.U_SCGD_TipoEmp is not null )  "
                    If DMS_Connector.Company.AdminInfo.EnableBranches = BoYesNoEnum.tNO Then
                        Query += " AND HE.branch = '{0}' "
                    Else
                        Query += " AND HE.BPLId = '{0}' "
                    End If

                    If Not m_strNumGrupo.Equals("-1") AndAlso Not String.IsNullOrEmpty(m_strNumGrupo) Then
                        Query += " AND U_SCGD_Equipo = '" + m_strNumGrupo + "' "
                    End If
                    Query = Query + " Order By U_SCGD_Equipo ASC, U_SCGD_TipoEmp ASC, T0.name ASC "
                    Query = String.Format(Query, CodigoSucursal)
            End Select
            Empleados = DMS_Connector.Helpers.EjecutarConsultaDataTable(Query)

            For Each Empleado As DataRow In Empleados.Rows
                FilaNueva = Agenda.NewRow()

                If TipoAgenda = TipoDeAgenda.Agenda Then
                    FilaNueva.Item("IdAgenda") = Empleado.Item("DocEntry")
                    FilaNueva("ID") = Empleado.Item("DocEntry")
                    FilaNueva("Name") = Empleado.Item("U_Agenda")
                    FilaNueva("Posicion") = Empleado.Item("DscPosicion")
                    FilaNueva.Item("Rol") = "A"
                    m_InfoRows._position = Agenda.Rows.Count
                    m_InfoRows._Code = Empleado.Item("DocEntry")
                    m_InfoRows._Name = Empleado.Item("U_Agenda")
                    m_InfoRows._DocEntryAgenda = Empleado.Item("DocEntry")
                Else
                    If Empleado.Item("U_SCGD_TipoEmp") = "A" Then
                        FilaNueva.Item("IdAgenda") = Empleado.Item("DocEntry")
                        FilaNueva.Item("ID") = Empleado.Item("DocEntry")
                        FilaNueva.Item("Name") = Empleado.Item("U_Agenda")
                        FilaNueva.Item("Rol") = "A"
                        FilaNueva.Item("Intervalo") = Empleado.Item("U_IntervaloCitas")
                        FilaNueva.Item("Posicion") = Empleado.Item("DscPosicion")
                        m_InfoRows._DocEntryAgenda = Empleado.Item("DocEntry")
                        m_InfoRows._Code = Empleado.Item("empID")
                        m_InfoRows._Name = Empleado.Item("U_Agenda")
                        m_InfoRows._Rol = "A"
                        m_InfoRows._Intervalo = Empleado.Item("U_IntervaloCitas")
                        m_InfoRows._ServRap = "N"
                    ElseIf Empleado.Item("U_SCGD_TipoEmp") = "T" Then
                        FilaNueva.Item("IdAgenda") = "0"
                        FilaNueva.Item("ID") = Empleado.Item("empID")
                        FilaNueva.Item("Name") = Empleado.Item("name")
                        FilaNueva.Item("Rol") = "T"
                        FilaNueva.Item("Intervalo") = 15
                        FilaNueva.Item("ServRap") = Empleado.Item("U_SCGD_TiempServ")
                        Posicion = Empleado.Item("DscPosicion")
                        If Not String.IsNullOrEmpty(Posicion) AndAlso Not Posicion = "0" Then
                            FilaNueva.Item("Posicion") = Posicion
                        End If
                        m_InfoRows._DocEntryAgenda = String.Empty
                        m_InfoRows._Code = Empleado.Item("empID")
                        m_InfoRows._Name = Empleado.Item("name")
                        m_InfoRows._Rol = "T"
                        m_InfoRows._Intervalo = 15
                        m_InfoRows._ServRap = IIf(IsDBNull(Empleado.Item("U_SCGD_TiempServ")), "", Empleado.Item("U_SCGD_TiempServ"))
                    End If
                    FilaNueva.Item("Grupo") = Empleado.Item("U_SCGD_Equipo")
                    m_InfoRows._position = Agenda.Rows.Count
                    m_InfoRows._Equipo = Empleado.Item("U_SCGD_Equipo")
                End If
                Agenda.Rows.Add(FilaNueva)
                m_listRows.Add(m_InfoRows)
            Next
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los estilos gráficos del datagridview de la agenda
    ''' </summary>
    ''' <param name="Grid">DataGridView de la agenda</param>
    ''' <param name="Agenda">Tabla con la información de la agenda</param>
    ''' <param name="TipoAgenda">Tipo de agenda, si es sencilla o por equipos</param>
    ''' <param name="ListaIntervalos">Listado de los intervalos de tiempo o columnas con las horas válidas</param>
    ''' <param name="NumeroAtributos">Número de atributos previo a las celdas donde se grafican las horas</param>
    ''' <remarks></remarks>
    Private Sub CargarEstilos(ByRef Grid As DataGridView, ByRef Agenda As System.Data.DataTable, ByVal TipoAgenda As Integer, ByRef ListaIntervalos As List(Of DateTime), ByVal NumeroAtributos As Integer)
        Dim TipoEmpleado As String = String.Empty
        Dim FilaGrid As DataGridViewRow
        Dim ColumnaGrid As DataGridViewColumn
        Try
            If Grid.Rows.Count > 0 Then
                Grid.Columns(0).Visible = False
                Grid.Columns(1).Visible = False
                Grid.Columns(4).Visible = False
                Grid.Columns(5).Visible = False
                Grid.Columns(6).Visible = False
                Grid.Columns(7).Visible = False
                Grid.Columns(2).Visible = True
                Grid.Columns(2).HeaderText = My.Resources.Resource.EncabezadoPosicion
                Grid.Columns(3).Frozen = True
                Grid.Columns(3).HeaderText = ""

                For i As Integer = 0 To dtAgenda.Rows.Count - 1
                    FilaGrid = Grid.Rows(i)
                    FilaGrid.Height = 30
                    Grid.Rows(i).Cells(2).Style.BackColor = Color.Wheat
                    Grid.Rows(i).Cells(3).Style.BackColor = Color.Wheat

                    For j As Integer = 0 To ListaIntervalos.Count - 1
                        If TipoAgenda = TipoDeAgenda.Equipos Then
                            TipoEmpleado = dtAgenda.Rows.Item(i).Item("Rol").ToString()
                            If TipoEmpleado = "A" Then
                                dtAgenda.Rows.Item(i).Item(j + NumeroAtributos) = "."
                                dgv_AgendaCitas.Rows(i).Cells(j + NumeroAtributos).Style.BackColor = Color.LightGray
                            Else
                                dtAgenda.Rows.Item(i).Item(j + NumeroAtributos) = String.Empty
                            End If
                        Else
                            dtAgenda.Rows.Item(i).Item(j + NumeroAtributos) = String.Empty
                        End If
                        ColumnaGrid = dgv_AgendaCitas.Columns(j + NumeroAtributos)
                        ColumnaGrid.Width = 75
                        ColumnaGrid.SortMode = DataGridViewColumnSortMode.NotSortable
                    Next
                Next
            End If

            lblCitasReasignar.Anchor = AnchorStyles.Top & AnchorStyles.Left

            Me.Size = New Size(1100, 600)
            Me.MaximizeBox = True

            'cargarSkin()
            Grid.Focus()
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa las columnas del DataTable que contiene la información de los empleados que se grafican en la agenda
    ''' y los intervalos de tiempo sobre los cuales se grafican los documentos
    ''' </summary>
    ''' <param name="ListaIntervalos"></param>
    ''' <remarks></remarks>
    Private Sub InicializarColumnas(ByRef ListaIntervalos As List(Of DateTime))
        Dim Encabezado As String = String.Empty
        Try
            'Inicializa las columnas del datatable agenda
            dtAgenda.Columns.Add("IdAgenda", GetType(String))
            dtAgenda.Columns.Add("ID", GetType(String))
            dtAgenda.Columns.Add("Posicion", GetType(String))
            dtAgenda.Columns.Add("Name", GetType(String))
            dtAgenda.Columns.Add("Grupo", GetType(String))
            dtAgenda.Columns.Add("Rol", GetType(String))
            dtAgenda.Columns.Add("Intervalo", GetType(String))
            dtAgenda.Columns.Add("ServRap", GetType(String))
            m_listColums = New List(Of infoColumn)()
            For Each Intervalo As DateTime In ListaIntervalos
                Encabezado = Intervalo.ToString("HH:mm")
                dtAgenda.Columns.Add(Encabezado, GetType(String))
                m_InfoColum._position = m_listColums.Count
                m_InfoColum._Hora = Intervalo.ToString("HH")
                m_InfoColum._Minutos = Intervalo.ToString("mm")
                m_InfoColum._horaFull = Intervalo.ToString("HH:mm")
                m_listColums.Add(m_InfoColum)
            Next

            dtAgenda.DefaultView.AllowNew = False
            m_intCantTotalColumnas = dtAgenda.Columns.Count
            'Inicializa las columnas del Datatable problemas
            dtCitasProbl.Columns.Add("Name", GetType(String))
            dtCitasProbl.Columns.Add("Citas", GetType(String))
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el ComboBox con las distintas sucursales
    ''' </summary>
    ''' <param name="CodigoSucursal">Código de la sucursal en formato texto</param>
    ''' <remarks></remarks>
    Private Sub CargarComboBoxSucursal(ByVal CodigoSucursal As String)
        Dim Query As String = "SELECT T0.Code, T0.Name FROM [@SCGD_SUCURSALES] T0"
        Dim DataTable As System.Data.DataTable
        Try
            DataTable = DMS_Connector.Helpers.EjecutarConsultaDataTable(Query)
            cboAgenda.DataSource = DataTable
            cboAgenda.ValueMember = "Code"
            cboAgenda.DisplayMember = "Name"
            cboAgenda.SelectedValue = CodigoSucursal
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub



    ''' <summary>
    ''' Manaejo de Columnas DataGridView
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManejorDataGridView()

        Try
            If dgv_AgendaCitas.Rows.Count > 0 Then

                dgv_AgendaCitas.Columns(0).Visible = False
                dgv_AgendaCitas.Columns(1).Visible = False
                dgv_AgendaCitas.Columns(4).Visible = False
                dgv_AgendaCitas.Columns(5).Visible = False
                dgv_AgendaCitas.Columns(6).Visible = False
                dgv_AgendaCitas.Columns(7).Visible = False
                dgv_AgendaCitas.Columns(2).Visible = True
                dgv_AgendaCitas.Columns(2).HeaderText = My.Resources.Resource.EncabezadoPosicion
                dgv_AgendaCitas.Columns(3).Frozen = True
                dgv_AgendaCitas.Columns(3).HeaderText = ""


                For count As Integer = 0 To dgv_AgendaCitas.Rows.Count - 1

                    Dim row As DataGridViewRow = dgv_AgendaCitas.Rows(count)
                    row.Height = 30

                    dgv_AgendaCitas.Rows(count).Cells(2).Style.BackColor = Color.Wheat
                    dgv_AgendaCitas.Rows(count).Cells(3).Style.BackColor = Color.Wheat
                Next

                For Count As Integer = 7 To dgv_AgendaCitas.Columns.Count - 1
                    Dim Column As DataGridViewColumn = dgv_AgendaCitas.Columns(Count)
                    Column.Width = 75
                Next

                For Each dgv As DataGridViewColumn In dgv_AgendaCitas.Columns
                    dgv.SortMode = DataGridViewColumnSortMode.NotSortable
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    'Private Sub dtgOcupacion_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
    '    Try

    '        ShowToolTipInfo(New Point(e.X, e.Y))

    '    Catch ex As Exception

    '    End Try

    'End Sub

    Private Sub dgv_AgendaCitas_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles dgv_AgendaCitas.MouseMove
        Try
            ShowToolTipInfoDataGridView(New Point(e.X, e.Y))
        Catch ex As Exception

        End Try


    End Sub

    Private Sub dtgvCitasReasignar_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dtgvCitasReasignar.CellPainting
        e.Paint(e.ClipBounds, DataGridViewPaintParts.All)
        If e.ColumnIndex = 0 And e.RowIndex >= 0 Then
            dtgvCitasReasignar.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = System.Drawing.Color.FromArgb(CType(222, Byte), CType(223, Byte), CType(206, Byte))
            e.Handled = True
        End If
    End Sub

    Private Sub dtgvCitasReasignar_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtgvCitasReasignar.SelectionChanged
        dtgvCitasReasignar.ClearSelection()
    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If m_tipoAgendaCargar = TipoDeAgenda.Agenda Then
            If oVersionModuloCita = VersionModuloCita.Estandar Then
                RaiseEvent eCargaCitaNueva_PorAgenda(fhaAsesorSel, m_strCodAsesor, m_strCodTecnico, m_strCodSucursal, m_strCodAgenda)
            Else
                If Not String.IsNullOrEmpty(m_strCodSucursal) AndAlso Not String.IsNullOrEmpty(m_strCodAgenda) Then
                    Me.WindowState = FormWindowState.Minimized
                    ConstructorCitas.CrearInstanciaFormulario(m_strCodSucursal, m_strCodAgenda, fhaAsesorSel)
                End If
            End If
        ElseIf m_tipoAgendaCargar = TipoDeAgenda.Equipos Then
            If oVersionModuloCita = VersionModuloCita.Estandar Then
                RaiseEvent eCargaCitaNueva_PorEquipos(fhaAsesorSel, fhaTecnicoSel, m_strCodAsesor, m_strCodTecnico, m_strCodSucursal, m_strCodAgenda)
            Else
                If CitaAbierta Then
                    RaiseEvent eCargaCitaNueva_PorEquipos(fhaAsesorSel, fhaTecnicoSel, m_strCodAsesor, m_strCodTecnico, m_strCodSucursal, m_strCodAgenda)
                Else
                    If Not String.IsNullOrEmpty(m_strCodSucursal) AndAlso Not String.IsNullOrEmpty(m_strCodAgenda) Then
                        Me.WindowState = FormWindowState.Minimized
                        ConstructorCitas.CrearInstanciaFormulario(m_strCodSucursal, m_strCodAgenda, m_strCodAsesor, fhaAsesorSel, m_strCodTecnico, fhaTecnicoSel)
                    End If
                End If
            End If
        End If
    End Sub



    Private Sub dgv_AgendaCitas_CellFormatting(sender As System.Object, e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgv_AgendaCitas.CellFormatting
        ''e.CellStyle.SelectionBackColor = Color.IndianRed
    End Sub

    Private Sub frmListaCitas_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick

    End Sub

    Private Sub dtpFecha_CloseUp(sender As System.Object, e As System.EventArgs) Handles dtpFecha.CloseUp
        Try
            m_dtFecha = dtpFecha.Value
            m_strCodSucursal = cboAgenda.SelectedValue
            dgv_AgendaCitas.DataSource = Nothing
            dgv_AgendaCitas.Rows.Clear()
            LoadConsultaOcupacion()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub cboAgenda_DropDownClosed(sender As System.Object, e As System.EventArgs) Handles cboAgenda.DropDownClosed
        Try
            m_dtFecha = dtpFecha.Value
            m_strCodSucursal = cboAgenda.SelectedValue
            dgv_AgendaCitas.DataSource = Nothing
            dgv_AgendaCitas.Rows.Clear()
            LoadConsultaOcupacion()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub dgv_AgendaCitas_CellMouseClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv_AgendaCitas.CellMouseClick
        Dim ValorCelda As String = String.Empty
        Try
            If e.Button = Windows.Forms.MouseButtons.Right Then
                If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
                    ValorCelda = dgv_AgendaCitas.Item(e.ColumnIndex, e.RowIndex).Value
                    Dim Hilo As New Thread(
                        Sub()
                            If Not String.IsNullOrEmpty(ValorCelda) Then
                                My.Computer.Clipboard.SetText(ValorCelda)
                            Else
                                My.Computer.Clipboard.SetText(" ")
                            End If
                        End Sub
                    )
                    Hilo.SetApartmentState(ApartmentState.STA)
                    Hilo.Start()
                End If
            End If
        Catch ex As Exception
            ManejoErroresAgenda(ex)
        End Try
    End Sub
End Class

