Option Strict On
Option Explicit On

Imports DMSOneFramework.CitasTableAdapters
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.UX.Windows.CitasAutomaticas
Imports DMSOneFramework
Imports DMSOneFramework.ProgramacionCitasDataSetTableAdapters
Imports DMSOneFramework.SCGCommon
Imports SCG_User_Interface.SCG_User_Interface
Imports SCG_User_Interface.ServicioAlCliente.ProgramacionCitas
Imports System.Data.SqlClient

Public Class frmProgramacionCitasAutom
    Private dstCitas As Citas = New Citas()

    Public Sub New(ByVal p_blnEstado As Boolean)
        MyBase.New()
        InitializeComponent()
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmProgramacionCitasAutom_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Carga()
    End Sub

    Private Sub Carga()

        txtSalida.Clear()
        Dim administradFiltros As AdministradorFiltrosVehiculos = New AdministradorFiltrosVehiculos(strConexionSBO)

        Dim dstProgramacionCitas As ProgramacionCitasDataSet = New ProgramacionCitasDataSet()

        'Carga las categorias
        Dim categoriasFiltros As List(Of CategoriaFiltro)
        Dim adpCategorias As SCGTA_TB_CategoriasFiltrosTableAdapter = New SCGTA_TB_CategoriasFiltrosTableAdapter()
        adpCategorias.Connection.ConnectionString = strConexionADO
        adpCategorias.Fill(dstProgramacionCitas.SCGTA_TB_CategoriasFiltros)
        categoriasFiltros = New List(Of CategoriaFiltro)(dstProgramacionCitas.SCGTA_TB_CategoriasFiltros.Rows.Count)
        For Each categoriasFiltrosRow As ProgramacionCitasDataSet.SCGTA_TB_CategoriasFiltrosRow In dstProgramacionCitas.SCGTA_TB_CategoriasFiltros
            categoriasFiltros.Add(New CategoriaFiltro(categoriasFiltrosRow.IdCategoriaFiltro, categoriasFiltrosRow.CategoriaFiltro))
        Next

        'Carga los filtros
        Dim filtros As List(Of IFiltro)
        Dim adpFiltros As SCGTA_TB_ConfiguracionFiltrosTableAdapter = New SCGTA_TB_ConfiguracionFiltrosTableAdapter()
        adpFiltros.Connection.ConnectionString = strConexionADO
        adpFiltros.Fill(dstProgramacionCitas.SCGTA_TB_ConfiguracionFiltros)
        filtros = New List(Of IFiltro)(dstProgramacionCitas.SCGTA_TB_ConfiguracionFiltros.Rows.Count)

        'Carga Agendas
        Dim agendas As List(Of IAgenda)
        Dim adpAgendas As SCGTA_TB_AgendasTableAdapter = New SCGTA_TB_AgendasTableAdapter()
        adpAgendas.Connection.ConnectionString = strConexionADO
        adpAgendas.Fill(dstProgramacionCitas.SCGTA_TB_Agendas, Nothing, True)
        agendas = New List(Of IAgenda)(dstProgramacionCitas.SCGTA_TB_Agendas.Rows.Count)
        For Each agendaRow As ProgramacionCitasDataSet.SCGTA_TB_AgendasRow In dstProgramacionCitas.SCGTA_TB_Agendas
            Dim agenda As Agenda = New Agenda(agendaRow.ID, agendaRow.Agenda, agendaRow.Abreviatura, agendaRow.IntervaloCitas)
            If agendaRow.IsCodAsesorNull Then agenda.CodigoAsesor = -1 Else agenda.CodigoAsesor = agendaRow.CodAsesor
'            agenda.CodigoTecnico = agendaRow.CodTecnico
            agenda.ArticuloCita = agendaRow.ArticuloCita
            agenda.RazonCita = agendaRow.RazonCita
            agendas.Add(agenda)
        Next

        'Carga configuracion AgendasFiltros
        Dim adpConfFiltrosAgendas As SCGTA_TB_ConfFiltrosAgendasTableAdapter = New SCGTA_TB_ConfFiltrosAgendasTableAdapter()

        adpConfFiltrosAgendas.Connection.ConnectionString = strConexionADO
        adpConfFiltrosAgendas.Fill(dstProgramacionCitas.SCGTA_TB_ConfFiltrosAgendas)
        'Actualizar lista confAgendaFiltros a IFiltro
        For Each filtrosRow As ProgramacionCitasDataSet.SCGTA_TB_ConfiguracionFiltrosRow In dstProgramacionCitas.SCGTA_TB_ConfiguracionFiltros
            Dim filtroDms As FiltroDMS = New FiltroDMS(filtrosRow.Filtro, filtrosRow.Descripción, filtrosRow.Condicion, filtrosRow.idCategoriaFiltro, True)

            For Each confFiltroAgendaRow As ProgramacionCitasDataSet.SCGTA_TB_ConfFiltrosAgendasRow In filtrosRow.GetSCGTA_TB_ConfFiltrosAgendasRows
                'si mostrar está en True entonces agrego la configuración de Agenda
                If confFiltroAgendaRow.Mostrar Then filtroDms.ConfiguracionesPorAgenda.Add(confFiltroAgendaRow.IdAgenda, New ConfiguracionFiltrosAgendas(confFiltroAgendaRow.IdAgenda, confFiltroAgendaRow.SCGTA_TB_AgendasRow.Agenda, confFiltroAgendaRow.Activo, Color.FromName(confFiltroAgendaRow.Color)))
            Next

            filtros.Add(filtroDms)
        Next

        Dim ac As AdministradorPropuestasCitas
        ac = New AdministradorPropuestasCitas(administradFiltros, filtros, categoriasFiltros, agendas)
        AgendaPropuestaCitas1.AdministradorPropuestasCitas = ac
        AgendaPropuestaCitas1.AgendaActual = agendas(0)
        AgendaPropuestaCitas1.CargaProgramacion()

    End Sub

    Private Sub AgendaPropuestaCitas1_EditarElementoCita(ByVal elementoCita As SCG.UX.Windows.CitasAutomaticas.IElementoCita, ByVal formularioPadre As System.Windows.Forms.Form, ByVal flowLayout As FlowLayoutPanel, ByVal control As Control) Handles AgendaPropuestaCitas1.EditarElementoCita
        Dim frm As frmEditarElementoCita = New frmEditarElementoCita()
        frm.Vehiculo = DirectCast(elementoCita, Vehiculo)
        frm.CargaPropiedades()
        If frm.ShowDialog(formularioPadre) = Windows.Forms.DialogResult.OK Then
            elementoCita.ModificadoPorUsuario = True
        Else
            elementoCita.ModificadoPorUsuario = False
        End If

    End Sub

    Private Function GeneraCita(ByVal veh As Vehiculo, ByVal agend As Agenda) As Citas.SCGTA_TB_CitaRow

        Dim observaciones As String = String.Empty

        'No se para qué es esto
        Dim fechaHoraEnHorario As Date

        transaccion = Nothing
        adpCitas.Connection.ConnectionString = strConexionADO

        fechaHoraEnHorario = New Date(1900, 1, 1, veh.FechaProximoServicio.Value.Hour, veh.FechaProximoServicio.Value.Minute, 0)

        Dim _
    row As Citas.SCGTA_TB_CitaRow = _
        dstCitas.SCGTA_TB_Cita.AddSCGTA_TB_CitaRow(String.Empty, -999, veh.FechaProximoServicio.Value, agend.IdAgenda, _
                                                     agend.RazonCita, _
                                                     observaciones, True, veh.CardCode, _
                                                     veh.IdVehiculo, veh.IdVehiculo, agend.CodigoAsesor, _
                                                      GlobalesUI.G_strUsuarioAplicacion, _
                                                     Nothing, fechaHoraEnHorario, String.Empty, String.Empty)
        adpCitas.Connection.Open()
        transaccion = adpCitas.Connection.BeginTransaction()
        adpCitas.Transaccion = transaccion
        adpCitas.Update(dstCitas.SCGTA_TB_Cita)

        If (row.NoCita = "-1") Then
            AgregaMensaje(String.Format("Error: {0} - {1}", veh.Descripcion, My.Resources.ResourceUI.ErrorGenCita))
            row = Nothing
        End If

        Return row
    End Function

    Private transaccion As SqlTransaction = Nothing
    Private adpCitas As SCGTA_TB_CitaTableAdapter = New SCGTA_TB_CitaTableAdapter()

    Private Sub GeneraCotizacion(ByVal veh As Vehiculo, ByVal agend As Agenda, ByVal row As Citas.SCGTA_TB_CitaRow)

        Dim sboCotizacion As Documents

        Dim dstConf As New ConfiguracionDataSet
        Dim adapterConfiguracion As ConfiguracionDataAdapter
        Dim serieCotizacion As String = String.Empty
        Dim razon As String = String.Empty

        sboCotizacion = DirectCast(G_objCompany.GetBusinessObject(BoObjectTypes.oQuotations), Documents)

        adapterConfiguracion = New ConfiguracionDataAdapter(strConexionADO)
        adapterConfiguracion.Fill(dstConf.SCGTA_TB_Configuracion)

        ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, "IDSerieDocumentosCotizaciones", serieCotizacion)

        sboCotizacion.CardCode = veh.CardCode
        sboCotizacion.DocDate = Date.Now
        sboCotizacion.DocDueDate = Date.Now
        If agend.CodigoAsesor <> -1 Then sboCotizacion.DocumentsOwner = agend.CodigoAsesor

        If Not String.IsNullOrEmpty(serieCotizacion) Then sboCotizacion.Series = CInt(serieCotizacion)

        'copiar datos vehiculo
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = veh.IdVehiculo
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = veh.CodUnidad
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = veh.NumPlaca
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = veh.CodMarca
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = veh.DescMarca
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = veh.CodEstilo
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = veh.DescEstilo
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = veh.Vin
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = veh.CodModelo
        sboCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = veh.DescModelo

        sboCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = "No Iniciada"
        sboCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = row.NoConsecutivo
        sboCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = row.NoSerie

        sboCotizacion.Comments = "Cita automática"
        sboCotizacion.Lines.ItemCode = agend.ArticuloCita
        sboCotizacion.Lines.Add()

        Dim codigoError As Integer
        Dim descError As String

        G_objCompany.StartTransaction()
        codigoError = sboCotizacion.Add()

        If codigoError <> 0 Then
            descError = G_objCompany.GetLastErrorDescription()
            AgregaMensaje(String.Format("Error: {0} - {1} {2}", veh.Descripcion, descError, codigoError.ToString))
            If (G_objCompany.InTransaction) Then G_objCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            If transaccion IsNot Nothing Then
                transaccion.Rollback()
                If adpCitas.Connection.State <> ConnectionState.Closed Then adpCitas.Connection.Close()
            End If
            Return
        End If

        If (G_objCompany.InTransaction) Then G_objCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
        transaccion.Commit()

        Dim key As String = String.Empty
        G_objCompany.GetNewObjectCode(key)
        sboCotizacion.GetByKey(CInt(key))

        row.NoCotizacion = sboCotizacion.DocEntry
        adpCitas.Update(dstCitas.SCGTA_TB_Cita)
        If adpCitas.Connection.State <> ConnectionState.Closed Then adpCitas.Connection.Close()

        'actualizar vehículo

        Dim adpVehiculo As SCGTA_VW_Vehiculos2TableAdapter = New SCGTA_VW_Vehiculos2TableAdapter()
        adpVehiculo.CadenaConexion = strConexionADO
        adpVehiculo.ActualizaFechasServicios(veh.FechaProximoServicio, veh.FechaUltimoServicio, veh.IdVehiculo)

    End Sub

    Private Sub btnGenerarCitas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerarCitas.Click

        Try

            txtSalida.Text = String.Empty
            AgregaMensaje(My.Resources.ResourceUI.GenerandoCitas)

            If (AgendaPropuestaCitas1.ElementosCitas IsNot Nothing) Then
                For Each elementoCita As IElementoCita In AgendaPropuestaCitas1.ElementosCitas
                    If elementoCita.EnAgenda AndAlso elementoCita.FechaProximoServicio > DateTime.Now AndAlso elementoCita.GenerarCita AndAlso elementoCita.EnAgenda Then
                        Dim citaRow As Citas.SCGTA_TB_CitaRow = GeneraCita(CType(elementoCita, Vehiculo), CType(AgendaPropuestaCitas1.AgendaActual, Agenda))
                        If citaRow IsNot Nothing Then
                            GeneraCotizacion(CType(elementoCita, Vehiculo), CType(AgendaPropuestaCitas1.AgendaActual, Agenda), citaRow)
                        End If
                    ElseIf elementoCita.EnAgenda Then

                        If elementoCita.GenerarCita = False Then
                            AgregaMensaje(String.Format(My.Resources.ResourceUI.VehYaCita, elementoCita.Descripcion))
                        Else
                            AgregaMensaje(String.Format(My.Resources.ResourceUI.VehHoraPost, elementoCita.Descripcion))
                        End If
                    End If
                Next
            End If

            AgendaPropuestaCitas1.Cargar()

        Catch ex As Exception
            If (G_objCompany.InTransaction) Then G_objCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            If transaccion IsNot Nothing Then
                If adpCitas.Connection.State <> ConnectionState.Closed Then adpCitas.Connection.Close()
            End If
            Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
            'SCGExceptionHandler.clsExceptionHandler.handException(ex, System.Windows.Forms.Application.StartupPath, gc_strAplicacion)
        End Try

    End Sub

    Private Sub AgregaMensaje(ByVal str As String)

        If String.IsNullOrEmpty(txtSalida.Text) Then txtSalida.Text = str Else txtSalida.Text = txtSalida.Text + System.Environment.NewLine + str
        txtSalida.Select(txtSalida.Text.Length - 1, 0)
        txtSalida.ScrollToCaret()

    End Sub

End Class