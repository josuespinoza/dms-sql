Imports Proyecto_SCGToolBar.SCGToolBar
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic

Namespace SCG_User_Interface
    Public Class frmDetalleCita

#Region "Declaraciones"

#Region "Variables"

        Private m_intTipoInsercion As enumModoInsercion

        Private m_intIDCita As Integer
        Private m_intIDAgenda As Integer
        Private m_intIDRazon As Integer
        Private m_strObservaciones As String
        Private m_dtFechayHora As Date
        Private m_intNoCotizacion As Integer
        Private m_strNoCita As String
        Private m_strNoSerie As String
        Private m_strNoConsecutivo As String
        Private m_strCardCode As String
        Private m_intIDvehiculo As Integer
        Private m_strNoVehiculo As String

        Private m_intIntervaloCitas As Integer
        Private m_dtHoraVisual As Date
        Private m_blnEjecutarEvento As Boolean = False

        Private m_intIDListaPrecios As Integer

        '----Se agrego 09/12/2009------------------
        'Private m_intCodigoTecnico As Integer
        Private m_intCodigoTecnico As Nullable(Of Integer)

#End Region
#Region "Constantes"
        Private Const strUsaListaCliente As String = "UsaListaPreciosCliente"
        Private Const strListaPrecios As String = "ListaPrecios"
#End Region

#Region "Objetos"

        Private m_drdRazonesCita As SqlClient.SqlDataReader
        Private m_adpRazonesCita As New RazonesCitaDataAdapter

        Private m_drdAgendas As SqlClient.SqlDataReader
        Private m_adpAgendas As New AgendaDataAdapter

        Private m_drwCita As DMSOneFramework.CitasDataset.SCGTA_TB_CitasRow
        Private m_dstCita As New DMSOneFramework.CitasDataset
        Private m_adpCitas As New DMSOneFramework.SCGDataAccess.CitasDataAdapter

        Private m_dstItems As New QUT1Dataset
        Private m_adpItems As New QUT1DataAdapter

        Private m_objCotizacion As New CotizacionCLS(G_objCompany)
        Private m_objUtilitarios As New Utilitarios(strConexionADO)

        Private WithEvents m_objBuscador As New Buscador.SubBuscador
        Private WithEvents m_objfrmClientes As New frmCtrlInformacionClientes
        Private WithEvents m_objfrmCalendarioAgenda As New frmCalendarioAgenda(True)
        Private WithEvents m_objFrmVehiculos As frmCtrlInformacionVehiculos

        Public Event eDatosGuardados(ByVal strNumeroCita As String)

#End Region

#Region "Enums"

        Private Enum enumModoInsercion
            scgNuevo = 1
            scgModificar = 2
            scgSoloLectura = 3
        End Enum

#End Region

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()
            m_intTipoInsercion = enumModoInsercion.scgNuevo
            HabilitarControles()
            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

        Public Sub New(ByVal p_intTipoInsercion As Integer, Optional ByVal p_drwCita As CitasDataset.SCGTA_TB_CitasRow = Nothing)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()
            m_intTipoInsercion = p_intTipoInsercion
            If p_drwCita IsNot Nothing Then
                If p_drwCita.FechayHora < m_objUtilitarios.CargarFechaHoraServidor Then
                    m_intTipoInsercion = enumModoInsercion.scgSoloLectura
                End If
                CrearFilaAModificar(p_drwCita)

            End If
            HabilitarControles()
            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

#End Region

#Region "Métodos"

        Private Sub HabilitarControles()

            stlbCita.Buttons(enumButton.Buscar).Visible = False
            stlbCita.Buttons(enumButton.Exportar).Visible = False
            stlbCita.Buttons(enumButton.Imprimir).Visible = False
            stlbCita.Buttons(enumButton.Nuevo).Visible = False

            Select Case m_intTipoInsercion

                Case enumModoInsercion.scgModificar

                    stlbCita.Buttons(enumButton.Cancelar).Visible = False
                    picClientes.Enabled = False
                    picVehiculos.Enabled = False
                    cboAgenda.Enabled = False
                    dtgDetalles.ReadOnly = False
                    btnEliminarAct.Enabled = False

                Case enumModoInsercion.scgNuevo

                    stlbCita.Buttons(enumButton.Eliminar).Visible = False
                    dtgDetalles.ReadOnly = False
                    txtCreador.Text = G_strUsuarioAplicacion

                Case enumModoInsercion.scgSoloLectura

                    stlbCita.Buttons(enumButton.Eliminar).Visible = False
                    stlbCita.Buttons(enumButton.Guardar).Visible = False
                    stlbCita.Buttons(enumButton.Cancelar).Visible = False
                    picClientes.Enabled = False
                    picVehiculos.Enabled = False
                    cboAgenda.Enabled = False
                    cboRazonesCita.Enabled = False
                    txtObservaciones.ReadOnly = True
                    btnAgendaCitas.Enabled = False
                    dtgDetalles.ReadOnly = True
                    btnAgregarAct.Enabled = False
                    btnEliminarAct.Enabled = False
                    dtpFechaCita.Enabled = False
                    dtpHoraCita.Enabled = False
                    picAsesor.Enabled = False

            End Select


        End Sub

        Private Sub CargarDatosCatalogos()

            Try
                Const c_strListaPrecios As String = "ListaPrecios"
                Dim strValorRetorno As String = ""

                If m_intTipoInsercion = enumModoInsercion.scgSoloLectura Then
                    Call m_adpRazonesCita.Fill(m_drdRazonesCita, 0)
                Else
                    Call m_adpRazonesCita.Fill(m_drdRazonesCita)
                End If
                Call Utilitarios.CargarComboSourceByReader(cboRazonesCita, m_drdRazonesCita)
                Call m_adpAgendas.Fill(m_drdAgendas)
                m_blnEjecutarEvento = False
                Call Utilitarios.CargarComboSourceByReader(cboAgenda, m_drdAgendas)
                m_blnEjecutarEvento = True
                Call m_adpAgendas.Fill(m_drdAgendas)
                Do While m_drdAgendas.Read()
                    m_intIntervaloCitas = m_drdAgendas.GetInt32(3)
                    Exit Do
                Loop
                m_drdAgendas.Close()
                If m_intIntervaloCitas = 0 Then
                    m_intIntervaloCitas = 15
                End If
                bcItems.DataSource = m_dstItems.QUT1
                dtpFechaCita.Value = m_objUtilitarios.CargarFechaHoraServidor
                m_dtHoraVisual = dtpFechaCita.Value.AddMinutes(-1)
                dtpHoraCita.Value = dtpFechaCita.Value

                ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, c_strListaPrecios, strValorRetorno)

                If Not String.IsNullOrEmpty(strValorRetorno) Then
                    m_intIDListaPrecios = CInt(strValorRetorno)
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 01072010
                If m_drdAgendas IsNot Nothing Then
                    If Not m_drdAgendas.IsClosed Then
                        Call m_drdAgendas.Close()
                    End If
                End If

                'Agregado 02072010
                If m_drdRazonesCita IsNot Nothing Then
                    If Not m_drdRazonesCita.IsClosed Then
                        Call m_drdRazonesCita.Close()
                    End If
                End If
            End Try

        End Sub

        Private Sub PasarDatosADataRow()

            If m_drwCita Is Nothing Then

                m_drwCita = m_dstCita.SCGTA_TB_Citas.NewSCGTA_TB_CitasRow

            Else

                m_dstCita.AcceptChanges()

            End If

            m_drwCita.CardCode = txtCodCliente.Text
            m_drwCita.CodEstilo = txtEstilo.Tag
            m_drwCita.CodMarca = txtMarca.Tag
            m_drwCita.CodModelo = txtModelo.Tag
            m_drwCita.DescEstilo = txtEstilo.Text
            m_drwCita.DescMarca = txtMarca.Text
            m_drwCita.DescModelo = txtModelo.Text

            'If txtTecnico.Text = String.Empty Then
            '    m_intCodigoTecnico = Nothing
            'Else
            '    m_drwCita.CodTecnico = m_intCodigoTecnico

            'End If


            If txtTecnico.Text <> "" Then
                m_drwCita.CodTecnico = txtTecnico.Tag
                m_drwCita.DescripcionTecnico = txtTecnico.Text
            End If


            If txtAnoVehiculo.Text <> "" Then

                m_drwCita.AnoVehiculo = txtAnoVehiculo.Text

            End If

            m_drwCita.CardName = txtNombreCliente.Text
            m_drwCita.IDVehiculo = m_intIDvehiculo
            m_drwCita.NoVehiculo = txtNoUnidad.Text
            m_drwCita.Placa = txtPlaca.Text
            m_drwCita.Observaciones = txtObservaciones.Text
            m_drwCita.VIN = txtPlaca.Tag

            m_drwCita.IDAgenda = cboAgenda.SelectedValue
            m_drwCita.IDRazon = cboRazonesCita.SelectedValue
            m_drwCita.Razon = cboRazonesCita.SelectedItem.Descripcion
            m_drwCita.FechayHora = New Date(dtpFechaCita.Value.Year, dtpFechaCita.Value.Month, dtpFechaCita.Value.Day, dtpHoraCita.Value.Hour, dtpHoraCita.Value.Minute, 0)
            m_drwCita.FechayHoraEnHorario = New Date(1900, 1, 1, dtpHoraCita.Value.Hour, dtpHoraCita.Value.Minute, 0)

            m_drwCita.CreadaPor = G_strUsuarioAplicacion
            If txtAsesor.Text <> "" Then
                m_drwCita.empId = txtAsesor.Tag
                m_drwCita.empName = txtAsesor.Text
            End If

            If m_drwCita.NoCotizacion = -1 And m_dstCita.SCGTA_TB_Citas.Rows.Count = 0 Then

                m_dstCita.SCGTA_TB_Citas.AddSCGTA_TB_CitasRow(m_drwCita)

            End If

        End Sub

        Private Sub LimpiarDatos()

            EPCitas.Clear()
            m_intTipoInsercion = enumModoInsercion.scgNuevo
            txtAnoVehiculo.Clear()
            txtCodCliente.Clear()
            txtEstilo.Clear()
            txtMarca.Clear()
            txtModelo.Clear()
            txtNombreCliente.Clear()
            txtNoUnidad.Clear()
            txtObservaciones.Clear()
            txtPlaca.Clear()
            cboAgenda.SelectedIndex = -1
            cboRazonesCita.SelectedIndex = -1
            dtpFechaCita.Value = m_objUtilitarios.CargarFechaHoraServidor
            dtpHoraCita.Value = dtpFechaCita.Value
            m_dstCita.SCGTA_TB_Citas.Rows.Clear()
            m_dstItems.QUT1.Rows.Clear()
            bcItems.DataSource = m_dstItems.QUT1
            txtNoCita.Text = ""
            txtCombustible.Text = ""
            txtMotor.Text = ""
            txtTecnico.Text = ""
            Call HabilitarControles()

        End Sub

        Public Sub EstablecerValoresCita(ByVal p_datFechaYHora As Date, _
                                         ByVal p_strNombreAgenda As String, _
                                         ByVal p_intCodigoAgenda As String)

            dtpFechaCita.Value = p_datFechaYHora
            dtpHoraCita.Value = p_datFechaYHora
            cboAgenda.Text = p_strNombreAgenda

        End Sub

        Private Function ValidaTipoCliente(ByVal p_CardCode) As Boolean
            Dim strResultadoConsulta As String
            Dim srtUsaIntFord As String

            srtUsaIntFord = Utilitarios.EjecutarConsulta("select ISNULL(U_Usa_IFord, 'N') from [@SCGD_ADMIN]  with (nolock) ", strConexionSBO)
            If Not String.IsNullOrEmpty(srtUsaIntFord) AndAlso Not srtUsaIntFord = "N" Then
                Dim strVista As String
                If m_objUtilitarios.CitasClientesInactivos Then
                    strVista = "SCGTA_VW_ClientesCitas"
                Else
                    strVista = "SCGTA_VW_Clientes"
                End If
                strResultadoConsulta = Utilitarios.EjecutarConsulta(String.Format("select ISNULL(U_SCGD_CusType, '' ) from {0}  with (nolock) where CardCode = '{1}'",
                                                                                            strVista, p_CardCode),
                                                                            strConexionADO)
                If String.IsNullOrEmpty(strResultadoConsulta) Then
                    Return False
                Else
                    Return True
                End If

            Else
                Return True
            End If

        End Function

        Private Function ValidarDatosSAP() As Boolean

            'Valida que el tipo de cambio y el periodo fiscal sean validos antes de realizar calculo de costos

            Dim blnValido As Boolean = True
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaSistema As String
            Dim strMonedaLocal As String

            Try
                objBLSBO = New BLSBO.GlobalFunctionsSBO
                strMonedaSistema = objBLSBO.RetornarMonedaSistema
                strMonedaLocal = objBLSBO.RetornarMonedaLocal

                If strMonedaLocal <> strMonedaSistema Then
                    decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedaSistema, Today, strConectionString, True)
                Else
                    decTipoCambio = 1
                End If
                If decTipoCambio = -1 Then

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.SBOActualizarTipoCambio)
                    blnValido = False

                End If
                Return blnValido

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try
        End Function

        Private Function ValidarDatos() As Boolean

            Dim blnDatosCorrectos As Boolean = True

            EPCitas.Clear()

            'If Not ValidarDatosSAP() Then
            '    blnDatosCorrectos = False
            'End If

            If txtCodCliente.Text = "" Then
                blnDatosCorrectos = False
                EPCitas.SetError(Label12, My.Resources.ResourceUI.MensajeDebeSeleccionarCliente)
            End If
            If m_intIDvehiculo = 0 Then
                blnDatosCorrectos = False
                EPCitas.SetError(lblNumeroVehículo, My.Resources.ResourceUI.MensajeDebeSeleccionarVehiculo)
            End If
            If cboAgenda.SelectedIndex = -1 Then

                blnDatosCorrectos = False
                EPCitas.SetError(lblAgenda, My.Resources.ResourceUI.MensajeDebeSeleccionarAgenda)
            End If
            If cboRazonesCita.SelectedIndex = -1 Then

                blnDatosCorrectos = False
                EPCitas.SetError(lblRazon, My.Resources.ResourceUI.MensajeDebeSeleccionarRazon)
            End If
            If dtgDetalles.RowCount = 0 Then

                blnDatosCorrectos = False
                EPCitas.SetError(dtgDetalles, My.Resources.ResourceUI.MensajeDebeSeleccionarAlMenos1ItemCita)
            End If
            Return blnDatosCorrectos
        End Function

        Private Sub MostrarDatosPantalla()

            'Dim intContador As Integer
            'Dim objItemCombo As Object
            'Dim blnEncontrada As Boolean

            m_intIDCita = m_drwCita.IDCita
            m_intNoCotizacion = m_drwCita.NoCotizacion
            m_intIDvehiculo = m_drwCita.IDVehiculo

            txtCodCliente.Text = m_drwCita.CardCode
            txtEstilo.Text = m_drwCita.DescEstilo
            txtMarca.Text = m_drwCita.DescMarca
            txtModelo.Text = m_drwCita.DescModelo
            txtAnoVehiculo.Text = m_drwCita.AnoVehiculo
            txtNombreCliente.Text = m_drwCita.CardName
            txtNoUnidad.Text = m_drwCita.NoVehiculo
            txtPlaca.Text = m_drwCita.Placa
            txtPlaca.Tag = m_drwCita.VIN
            txtObservaciones.Text = m_drwCita.Observaciones
            txtNoCita.Text = m_drwCita.NoCita

            If m_drwCita.IsCodTecnicoNull Or m_drwCita.IsDescripcionTecnicoNull Then
                txtTecnico.Text = ""
            Else
                txtTecnico.Text = m_drwCita.DescripcionTecnico
            End If

            If Not m_drwCita.IsCilindradaNull And m_drwCita.Cilindrada <> -1 Then
                txtMotor.Text = m_drwCita.Cilindrada
            End If

            If Not m_drwCita.IsCombustibleNull Then
                txtCombustible.Text = m_drwCita.Combustible
            End If
            cboAgenda.Text = m_drwCita.Agenda
            cboRazonesCita.Text = m_drwCita.Razon

            dtpFechaCita.Value = m_drwCita.FechayHora
            m_dtHoraVisual = m_drwCita.FechayHora
            dtpHoraCita.Value = m_dtHoraVisual
            If Not m_drwCita.IsempIdNull And m_drwCita.empId <> -1 Then
                txtAsesor.Tag = m_drwCita.empId
                txtAsesor.Text = m_drwCita.empName
            End If

            txtCreador.Text = m_drwCita.CreadaPor

            If m_drwCita.NoCotizacion <> -1 Then
                'Se le manda una N para que ejecute el SP normal
                m_adpItems.Fill(m_dstItems, m_drwCita.NoCotizacion, "N")
                bcItems.DataSource = m_dstItems.QUT1
            End If

        End Sub

        Private Sub CrearFilaAModificar(ByVal p_drwCita As CitasDataset.SCGTA_TB_CitasRow)

            'Agregar columana al dataset para hacer las actualizaciones
            If m_drwCita IsNot Nothing Then
                m_drwCita = Nothing
            End If
            m_drwCita = m_dstCita.SCGTA_TB_Citas.NewSCGTA_TB_CitasRow
            m_dstCita.SCGTA_TB_Citas.Rows.Clear()


            m_drwCita.IDCita = p_drwCita.IDCita
            m_drwCita.NoCotizacion = p_drwCita.NoCotizacion
            m_drwCita.IDVehiculo = p_drwCita.IDVehiculo

            m_drwCita.CardCode = p_drwCita.CardCode
            m_drwCita.DescEstilo = p_drwCita.DescEstilo
            m_drwCita.DescMarca = p_drwCita.DescMarca
            m_drwCita.DescModelo = p_drwCita.DescModelo
            m_drwCita.AnoVehiculo = p_drwCita.AnoVehiculo
            m_drwCita.CardName = p_drwCita.CardName
            m_drwCita.NoVehiculo = p_drwCita.NoVehiculo
            m_drwCita.Placa = p_drwCita.Placa
            m_drwCita.VIN = p_drwCita.VIN
            m_drwCita.Observaciones = p_drwCita.Observaciones
            m_drwCita.NoCita = p_drwCita.NoCita
            m_drwCita.IDAgenda = p_drwCita.IDAgenda
            m_drwCita.IDRazon = p_drwCita.IDRazon

            m_drwCita.Razon = p_drwCita.Razon
            m_drwCita.Agenda = p_drwCita.Agenda

            If Not p_drwCita.IsCilindradaNull Then
                m_drwCita.Cilindrada = p_drwCita.Cilindrada
            End If
            If Not p_drwCita.IsCombustibleNull Then
                m_drwCita.Combustible = p_drwCita.Combustible
            End If

            If Not p_drwCita.IsCodTecnicoNull Then
                m_drwCita.CodTecnico = p_drwCita.CodTecnico
                If Not p_drwCita.IsDescripcionTecnicoNull Then
                    m_drwCita.DescripcionTecnico = p_drwCita.DescripcionTecnico
                End If
            End If



            m_drwCita.FechayHora = p_drwCita.FechayHora
            m_drwCita.CreadaPor = p_drwCita.CreadaPor
            If Not p_drwCita.IsempIdNull Then
                m_drwCita.empId = p_drwCita.empId
                m_drwCita.empName = p_drwCita.empName
            End If

            m_drwCita.FechayHoraEnHorario = p_drwCita.FechayHoraEnHorario

            m_dstCita.SCGTA_TB_Citas.AddSCGTA_TB_CitasRow(m_drwCita)

        End Sub

        Public Sub Visualizacion_UDF()

            VisualizarUDFCita.Tabla = "SCGTA_TB_Cita"

            VisualizarUDFCita.Conexion = SCGDataAccess.DAConexion.ConnectionString

            VisualizarUDFCita.CampoLlave = "IDCita = " & m_intIDCita

            VisualizarUDFCita.Form = Me

            VisualizarUDFCita.VisualizarUDF()

            VisualizarUDFCita.Where = "IDCita = '" & m_intIDCita & "'"

            ' VisualizarUDFOrden.CargarDatosUDF("NoOrden = '" & txtNoOrden.Text & "'")
            VisualizarUDFCita.CargarComboCategorias()

        End Sub

#End Region

#Region "Eventos"

        Private Sub stlbCita_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles stlbCita.Click_Cancelar

            Try

                Call LimpiarDatos()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub stlbCita_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles stlbCita.Click_Cerrar

            Try

                Me.Close()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub frmDetalleCita_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                Call CargarDatosCatalogos()
                If m_intTipoInsercion <> enumModoInsercion.scgNuevo Then
                    Call MostrarDatosPantalla()
                End If
                Call HabilitarControles()

                Call Visualizacion_UDF()

                If cboAgenda.SelectedValue = Nothing Then
                    btnAgendaCitas.Enabled = False
                Else
                    btnAgendaCitas.Enabled = True

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnAgregarAct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarAct.Click

            Try
                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion

                'Poner aqui llamada a configuracion para ver si utiliza lista de precios
                Dim adpConfig As New ConfiguracionDataAdapter
                Dim dstConfig As New ConfiguracionDataSet
                Dim blnUsaListaClientes As Boolean = False
                Dim strCardCodeCliente As String = ""
                adpConfig.Fill(dstConfig)
                Dim strCodListaPrecios As String = ""

                Dim intListaPrecios As Integer
                Dim cmdConsult As SqlClient.SqlCommand


                'Valida si se utiliza la lista de precios del cliente, esto se configura el las parametrizaciones de DMS
                If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracionValorBooleano(dstConfig.SCGTA_TB_Configuracion, strUsaListaCliente, blnUsaListaClientes) Then

                    'strCodListaPrecios = Utilitarios.EjecutarConsulta(
                    '                    String.Format("select oc.ListNum from dbo.SCGTA_VW_OCRD as oc inner join dbo.SCGTA_VW_OQUT as oq on oc.cardcode = oq.cardcode  where oq.DocNum = '{0}'",
                    '                     m_intNoCotizacion), strConexionADO)
                    strCardCodeCliente = txtCodCliente.Text
                    If Not String.IsNullOrEmpty(strCardCodeCliente) Then

                        cmdConsult = CreateSelectCommandListaPreciosCliente(strCardCodeCliente)

                        cmdConsult.Connection = DATemp.ObtieneConexion

                        intListaPrecios = cmdConsult.ExecuteScalar

                        strCodListaPrecios = Convert.ToString(intListaPrecios)
                    End If



                Else

                    Dim strValorRetorno As String = ""
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, strListaPrecios, strValorRetorno)

                    If Not String.IsNullOrEmpty(strValorRetorno) Then
                        strCodListaPrecios = strValorRetorno
                    End If

                End If

                If Not String.IsNullOrEmpty(strCodListaPrecios) Then
                    m_intIDListaPrecios = CInt(strCodListaPrecios)
                End If





                'Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorItems

                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & _
                                        "," & My.Resources.ResourceUI.TipoArticulo & "," & My.Resources.ResourceUI.TipoMoneda & _
                                        "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras


                m_objBuscador.Criterios = "top 50 SCGTA_VW_OITM.itemCode, itemName, U_SCGD_TipoArticulo,ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"
                m_objBuscador.Tabla = "SCGTA_VW_OITM " & _
                                      "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode"
                If g_blnServiciosExternosInventariables Then
                    m_objBuscador.Where = "((U_SCGD_TipoArticulo = 1 and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y') and U_SCGD_Generico is not null " & _
                                        "or (U_SCGD_TipoArticulo = 2 and SellItem = 'Y' and InvntItem = 'N') and U_SCGD_T_Fase is not null " & _
                                        "or (U_SCGD_TipoArticulo = 3 and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y') " & _
                                        "or (U_SCGD_TipoArticulo = 4 and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y') and U_SCGD_Generico is not null " & _
                                        "or (U_SCGD_TipoArticulo = 5)) and ListP.PriceList = " & CStr(m_intIDListaPrecios)
                Else
                    m_objBuscador.Where = "((U_SCGD_TipoArticulo = 1 and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y') and U_SCGD_Generico is not null " & _
                                        "or (U_SCGD_TipoArticulo = 2 and SellItem = 'Y' and InvntItem = 'N') and U_SCGD_T_Fase is not null " & _
                                        "or (U_SCGD_TipoArticulo = 3 and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y') " & _
                                        "or (U_SCGD_TipoArticulo = 4 and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'N') and U_SCGD_Generico is not null " & _
                                        "or (U_SCGD_TipoArticulo = 5)) and ListP.PriceList = " & CStr(m_intIDListaPrecios)
                End If
                m_objBuscador.Criterios_OcultosEx = "3"
                m_objBuscador.ConsultarDBPorFiltrado = True
                'm_objBuscador.Top = 200
                m_objBuscador.MultiSeleccion = True
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picClientes.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Identificacion '"Código, Nombre, Identificación"
                m_objBuscador.Criterios = "top 200 CardCode, CardName, LicTradNum"
                'valida ci da citas o no a clientes inactivos 
                If m_objUtilitarios.CitasClientesInactivos Then
                    m_objBuscador.Tabla = "SCGTA_VW_ClientesCitas"
                Else
                    m_objBuscador.Tabla = "SCGTA_VW_Clientes"
                End If
                m_objBuscador.ConsultarDBPorFiltrado = True
                m_objBuscador.Where = ""
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub picVehiculos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picVehiculos.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorVehiculos '"SCG - Buscador Vehículos"

                'se muestran los campos CodigoRuta, Ruta y Ubicacion
                If g_blnCampoVisible Then

                    m_objBuscador.Titulos = My.Resources.ResourceUI.IDVehiculo & "," & My.Resources.ResourceUI.NoVehiculo & _
               "," & My.Resources.ResourceUI.Placa & "," & My.Resources.ResourceUI.CodCliente & _
               "," & My.Resources.ResourceUI.Cliente & _
               "," & "Codigo Ruta" & _
               "," & "Ruta" & _
               "," & "Ubicación" & _
               "," & My.Resources.ResourceUI.CodMarca & "," & My.Resources.ResourceUI.Marca & _
               "," & My.Resources.ResourceUI.CodEstilo & "," & My.Resources.ResourceUI.Estilo & _
               "," & My.Resources.ResourceUI.CodModelo & "," & My.Resources.ResourceUI.Modelo & _
               "," & My.Resources.ResourceUI.Año & "," & My.Resources.ResourceUI.VIN & _
               "," & My.Resources.ResourceUI.Combustible & "," & My.Resources.ResourceUI.Motor


                    m_objBuscador.Criterios = "top 200 IDVehiculo, NoVehiculo, Placa, CardCode, Cliente , Ruta, Ubicacion, DescUbicacion, CodMarca, " & _
                                              "DescMarca, CodEstilo, DescEstilo, " & _
                                              "CodModelo, DescModelo, AnoVehiculo, VIN, DescCombustible, Num_Motor "

                    m_objBuscador.Tabla = "SCGTA_VW_Vehiculos"

                Else

                    m_objBuscador.Titulos = My.Resources.ResourceUI.IDVehiculo & "," & My.Resources.ResourceUI.NoVehiculo & _
               "," & My.Resources.ResourceUI.Placa & "," & My.Resources.ResourceUI.CodCliente & _
               "," & My.Resources.ResourceUI.Cliente & _
               "," & My.Resources.ResourceUI.CodMarca & "," & My.Resources.ResourceUI.Marca & _
               "," & My.Resources.ResourceUI.CodEstilo & "," & My.Resources.ResourceUI.Estilo & _
               "," & My.Resources.ResourceUI.CodModelo & "," & My.Resources.ResourceUI.Modelo & _
               "," & My.Resources.ResourceUI.Año & "," & My.Resources.ResourceUI.VIN & _
               "," & My.Resources.ResourceUI.Combustible & "," & My.Resources.ResourceUI.Motor


                    m_objBuscador.Criterios = "top 200 IDVehiculo, NoVehiculo, Placa, CardCode, Cliente , CodMarca, " & _
                                              "DescMarca, CodEstilo, DescEstilo, " & _
                                              "CodModelo, DescModelo, AnoVehiculo, VIN, DescCombustible, Num_Motor "

                    m_objBuscador.Tabla = "SCGTA_VW_Vehiculos"

                End If


                If Not Trim(txtCodCliente.Text) = String.Empty Then
                    m_objBuscador.Where = "CardCode = '" & txtCodCliente.Text & "'"
                ElseIf Trim(txtCodCliente.Text) = String.Empty Then
                    'valida ci da citas o no a clientes inactivos
                    If m_objUtilitarios.CitasClientesInactivos Then
                        m_objBuscador.Where = " CardCode IN ( SELECT CardCode FROM SCGTA_VW_ClientesCitas ) "
                    Else
                        m_objBuscador.Where = " CardCode IN ( SELECT CardCode FROM SCGTA_VW_Clientes ) "

                    End If
                End If



                m_objBuscador.ConsultarDBPorFiltrado = True
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar

            Try

                Select Case sender.name

                    Case "picVehiculos"


                        If g_blnCampoVisible Then

                            m_intIDvehiculo = Arreglo_Campos(0)
                            txtNoUnidad.Text = Arreglo_Campos(1)
                            m_strNoVehiculo = Arreglo_Campos(1)
                            txtPlaca.Text = Arreglo_Campos(2)
                            txtPlaca.Tag = Arreglo_Campos(12)
                            txtMarca.Tag = Arreglo_Campos(8)
                            txtMarca.Text = Arreglo_Campos(9)
                            txtEstilo.Tag = Arreglo_Campos(10)
                            txtEstilo.Text = Arreglo_Campos(11)
                            txtModelo.Tag = Arreglo_Campos(12)
                            txtModelo.Text = Arreglo_Campos(13)
                            txtAnoVehiculo.Text = Arreglo_Campos(14)
                            txtCombustible.Text = Arreglo_Campos(16)
                            txtMotor.Text = Arreglo_Campos(17)
                        Else

                            m_intIDvehiculo = Arreglo_Campos(0)
                            txtNoUnidad.Text = Arreglo_Campos(1)
                            m_strNoVehiculo = Arreglo_Campos(1)
                            txtPlaca.Text = Arreglo_Campos(2)
                            txtPlaca.Tag = Arreglo_Campos(12)
                            txtMarca.Tag = Arreglo_Campos(5)
                            txtMarca.Text = Arreglo_Campos(6)
                            txtEstilo.Tag = Arreglo_Campos(7)
                            txtEstilo.Text = Arreglo_Campos(8)
                            txtModelo.Tag = Arreglo_Campos(9)
                            txtModelo.Text = Arreglo_Campos(10)
                            txtAnoVehiculo.Text = Arreglo_Campos(11)
                            txtCombustible.Text = Arreglo_Campos(13)
                            txtMotor.Text = Arreglo_Campos(14)

                        End If



                        If Trim(txtCodCliente.Text) = String.Empty Then
                            txtCodCliente.Text = Arreglo_Campos(3)
                            txtNombreCliente.Text = Arreglo_Campos(4)
                        End If

                        'If Not m_drwCita.IsCilindradaNull And m_drwCita.Cilindrada <> -1 Then
                        '    txtMotor.Text = m_drwCita.Cilindrada
                        'End If

                        'If Not m_drwCita.IsCombustibleNull Then
                        '    txtCombustible.Text = m_drwCita.Combustible
                        'End If

                    Case "picClientes"

                        txtCodCliente.Text = Arreglo_Campos(0)
                        txtNombreCliente.Text = Arreglo_Campos(1)

                    Case "btnAgregarAct"
                        Dim intCantidad As Integer

                        Dim drwItems As QUT1Dataset.QUT1Row


                        For intCantidad = 0 To m_objBuscador.OUT_DataTable.Rows.Count - 1

                            drwItems = m_dstItems.QUT1.NewQUT1Row
                            drwItems.itemCode = m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("itemCode")
                            drwItems.itemName = m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("itemName")
                            drwItems.U_TipoArticulo = m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("U_SCGD_TipoArticulo")
                            drwItems.Moneda = IIf(m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("Currency") Is DBNull.Value, "", m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("Currency"))
                            drwItems.Precio = IIf(m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("Price") Is DBNull.Value, 0, m_objBuscador.OUT_DataTable.Rows.Item(intCantidad).Item("Price"))
                            drwItems.CPen = 0
                            drwItems.CSol = 0
                            drwItems.CRec = 0
                            drwItems.CPDe = 0
                            drwItems.CPTr = 0
                            drwItems.CPBo = 0
                            drwItems.Compra = "N"
                            drwItems.Entregado = "N"
                            m_dstItems.QUT1.AddQUT1Row(drwItems)

                        Next

                        m_dstItems.QUT1.AcceptChanges()
                        bcItems.DataSource = m_dstItems.QUT1

                    Case "picAsesor"
                        txtAsesor.Tag = Arreglo_Campos(0)
                        txtAsesor.Text = Arreglo_Campos(1) + " " + Arreglo_Campos(2)

                        '---------------
                    Case "picTecnico"

                        m_intCodigoTecnico = CType(Arreglo_Campos(0), Integer)
                        txtTecnico.Text = Arreglo_Campos(1)

                End Select

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnEliminarAct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminarAct.Click

            Dim intCantidadLineasGrid As Integer
            Dim intCantidadEliminada As Integer

            Try

                For intCantidadLineasGrid = 0 To dtgDetalles.RowCount - 1
                    If dtgDetalles.Rows.Item(intCantidadLineasGrid - intCantidadEliminada).Cells("Check").EditedFormattedValue = True Then

                        m_dstItems.QUT1.Rows(intCantidadLineasGrid).Delete()
                        intCantidadEliminada += 1

                    End If
                Next

                m_dstItems.QUT1.AcceptChanges()
                bcItems.DataSource = m_dstItems.QUT1

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtgDetalles_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles dtgDetalles.DataError

            Try

                dtgDetalles.CancelEdit()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub stlbCita_Click_Eliminar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles stlbCita.Click_Eliminar

            Try
                m_objCotizacion.IniciarProceso()
                m_objCotizacion.CancelarCotizacion(m_drwCita.NoCotizacion)
                m_dstCita.AcceptChanges()
                m_drwCita.Delete()
                m_objCotizacion.FinalizarProceso()
                m_adpCitas.Update(m_dstCita)
                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeCitaEliminadaCorrectamente)
                RaiseEvent eDatosGuardados("")
                Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub stlbCita_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles stlbCita.Click_Guardar

            Dim cnConection As New SqlClient.SqlConnection
            Dim tnTransacion As SqlClient.SqlTransaction = Nothing
            Dim strSerieCotizaciones As String = ""

            Try

                If ValidarDatos() Then

                    'Validar Que el tipo de Moneda del item tenga ya su Tipo de Cambio en SAP
                    If ValidarMonedaItems(m_dstItems) = True Then
                        If ValidaTipoCliente(txtCodCliente.Text) Then

                            Call PasarDatosADataRow()

                            m_objCotizacion = New CotizacionCLS(G_objCompany)

                            ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "IDSerieDocumentosCotizaciones", strSerieCotizaciones)
                            m_objCotizacion.ImpuestoRepuestos = g_strImpRepuestos
                            m_objCotizacion.ImpuestoServicios = g_strImpServicios
                            m_objCotizacion.ImpuestoSuministros = g_strImpSuministros
                            m_objCotizacion.ImpuestoServiciosExternos = g_strImpServiciosExternos
                            m_objCotizacion.SerieDocumento = strSerieCotizaciones

                            m_objCotizacion.IniciarProceso()

                            m_adpCitas.Update(m_dstCita, cnConection, tnTransacion, True)

                            If m_drwCita.IDCita <> -1 Then
                                m_intNoCotizacion = m_objCotizacion.ManejarCotizacion(m_drwCita, m_dstItems, "")

                                If m_intNoCotizacion <> 0 Then
                                    m_drwCita.NoCotizacion = m_intNoCotizacion
                                    m_dstCita.Tables(0).Rows(0).Item("NoCotizacion") = m_intNoCotizacion
                                    m_adpCitas.Update(m_dstCita, cnConection, tnTransacion)

                                    m_objCotizacion.FinalizarProceso()

                                    'obtiene el numero de cita
                                    txtNoCita.Text = m_drwCita.NoCita

                                    If m_intTipoInsercion = enumModoInsercion.scgNuevo Then

                                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeSehaCreadoCita & " " & m_drwCita.NoCita)
                                        m_intTipoInsercion = enumModoInsercion.scgModificar
                                        Call HabilitarControles()

                                    Else

                                        objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeCitaModificadaCorrectamente)

                                        Me.Close()
                                    End If

                                    If m_drwCita.NoCotizacion <> -1 Then

                                        m_dstItems.QUT1.Rows.Clear()
                                        'Se le manda una N para que ejecute el SP normal
                                        m_adpItems.Fill(m_dstItems, m_drwCita.NoCotizacion, "N")
                                        bcItems.DataSource = m_dstItems.QUT1

                                    End If

                                Else
                                    Throw New Exception(My.Resources.ResourceUI.MensajeProblemasCrearCotizac)
                                End If

                                If cnConection IsNot Nothing Then
                                    If cnConection.State = ConnectionState.Open Then
                                        If tnTransacion IsNot Nothing Then
                                            tnTransacion.Commit()

                                        End If
                                        cnConection.Close()
                                    End If
                                    RaiseEvent eDatosGuardados(txtNoCita.Text)
                                    Me.Close()
                                End If

                                'Else de la primera Validacion, "ValidarDatos()"
                            Else
                                objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeFechaCitaasignadaoFechaNoValida)
                                If m_intTipoInsercion = enumModoInsercion.scgNuevo Then
                                    m_dstCita.SCGTA_TB_Citas.Rows.Clear()
                                End If
                                If cnConection IsNot Nothing Then
                                    If cnConection.State = ConnectionState.Open Then
                                        If tnTransacion IsNot Nothing Then
                                            tnTransacion.Rollback()
                                        End If
                                        cnConection.Close()
                                    End If
                                End If

                                m_objCotizacion.RetrocederProceso()
                            End If
                        Else
                            EPCitas.SetError(Label12, My.Resources.ResourceUI.MSJValidaTipoSN)
                        End If
                    End If
                End If

            Catch ex As Exception
                m_dstCita.SCGTA_TB_Citas.Rows.Clear()
                If cnConection IsNot Nothing Then
                    If cnConection.State = ConnectionState.Open Then
                        If tnTransacion IsNot Nothing Then
                            tnTransacion.Rollback()
                        End If
                        cnConection.Close()
                    End If
                End If
                m_objCotizacion.RetrocederProceso()
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally

                'Agregado 06072010
                Call cnConection.Close()

                tnTransacion = Nothing
                cnConection = Nothing

            End Try

        End Sub

        Private Sub picConfCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picConfCliente.Click

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean

            Try

                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                    If Forma_Nueva.Name = "frmCtrlInformacionClientes" Then
                        blnExisteForm = True
                    End If
                Next

                If Not blnExisteForm Then

                    If m_objfrmClientes IsNot Nothing Then

                        m_objfrmClientes.Dispose()
                        m_objfrmClientes = Nothing

                    End If

                    If txtCodCliente.Text.Trim() <> "" Then 'Modificar cliente
                        m_objfrmClientes = New frmCtrlInformacionClientes(2, txtCodCliente.Text.Trim())
                    Else 'Cliente Nuevo
                        m_objfrmClientes = New frmCtrlInformacionClientes(1)
                    End If

                    m_objfrmClientes.MdiParent = Me.MdiParent
                    m_objfrmClientes.Show()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub objfrmClientes_RetornarDatos(ByVal p_strCardCode As String, ByVal p_strCardName As String) Handles m_objfrmClientes.RetornarDatos

            Try

                If p_strCardCode <> "" Then

                    txtCodCliente.Text = p_strCardCode
                    txtNombreCliente.Text = p_strCardName
                    m_objfrmClientes.Close()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnAgendaCitas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgendaCitas.Click

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean

            Try
                If Not cboAgenda.SelectedValue = Nothing Then

                    btnAgendaCitas.Enabled = True

                    If m_objfrmCalendarioAgenda IsNot Nothing Then
                        m_objfrmCalendarioAgenda.Dispose()
                        m_objfrmCalendarioAgenda = Nothing
                    End If

                    For Each Forma_Nueva In Me.MdiParent.MdiChildren
                        If Forma_Nueva.Name = "frmCalendarioAgenda" Then
                            blnExisteForm = True
                        End If
                    Next

                    If Not blnExisteForm Then
                        m_objfrmCalendarioAgenda = New frmCalendarioAgenda(True, dtpFechaCita.Value, cboAgenda.SelectedItem.Descripcion)
                        m_objfrmCalendarioAgenda.MdiParent = Me.MdiParent
                        m_objfrmCalendarioAgenda.Show()
                    End If

                Else
                    btnAgendaCitas.Enabled = False

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtpHoraCita_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpHoraCita.ValueChanged

            'Dim intMinutos As Integer
            Dim intDiferencia As Integer
            Dim intIntervaloActual As Integer

            Try

                If m_dtHoraVisual.Hour = dtpHoraCita.Value.Hour AndAlso m_dtHoraVisual.Minute <> dtpHoraCita.Value.Minute Then
                    If m_dtHoraVisual < dtpHoraCita.Value Then
                        If dtpHoraCita.Value.Minute <> 59 Then
                            intIntervaloActual = m_intIntervaloCitas
                            Do While intIntervaloActual <= 60
                                If m_dtHoraVisual.Minute < intIntervaloActual Then
                                    intDiferencia = intIntervaloActual - m_dtHoraVisual.Minute
                                    m_dtHoraVisual = New Date(m_dtHoraVisual.Year, m_dtHoraVisual.Month, m_dtHoraVisual.Day, m_dtHoraVisual.Hour, m_dtHoraVisual.AddMinutes(intDiferencia).Minute, 0)
                                    Exit Do
                                Else
                                    intIntervaloActual += m_intIntervaloCitas
                                End If
                            Loop
                        Else
                            intDiferencia = 59 - (60 - m_intIntervaloCitas)
                            m_dtHoraVisual = New Date(m_dtHoraVisual.Year, m_dtHoraVisual.Month, m_dtHoraVisual.Day, m_dtHoraVisual.Hour, dtpHoraCita.Value.AddMinutes(0 - intDiferencia).Minute, 0)
                        End If
                    Else
                        intIntervaloActual = 60
                        Do While intIntervaloActual >= 0
                            If dtpHoraCita.Value.Minute > intIntervaloActual Then
                                intDiferencia = dtpHoraCita.Value.Minute - intIntervaloActual
                                m_dtHoraVisual = dtpHoraCita.Value.AddMinutes((0 - intDiferencia))
                                Exit Do
                            Else
                                intIntervaloActual -= m_intIntervaloCitas
                            End If
                        Loop
                    End If
                    dtpHoraCita.Value = m_dtHoraVisual
                End If
                m_dtHoraVisual = dtpHoraCita.Value

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub cboAgenda_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboAgenda.SelectedValueChanged

            Dim intIDAgenda As Integer

            Try

                If m_blnEjecutarEvento Then
                    Call m_adpAgendas.Fill(m_drdAgendas)
                    intIDAgenda = CInt(cboAgenda.SelectedValue)
                    m_intIntervaloCitas = 0
                    Do While m_drdAgendas.Read()
                        If m_drdAgendas.GetInt32(0) = intIDAgenda Then
                            m_intIntervaloCitas = m_drdAgendas.GetInt32(3)
                            Exit Do
                        End If
                    Loop
                    m_drdAgendas.Close()
                    If m_intIntervaloCitas = 0 Then
                        m_intIntervaloCitas = 15
                    End If
                    Call dtpHoraCita_ValueChanged(sender, e)
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 01072010
                If m_drdAgendas IsNot Nothing Then
                    If Not m_drdAgendas.IsClosed Then
                        Call m_drdAgendas.Close()
                    End If
                End If

            End Try

        End Sub

        Private Sub m_objfrmCalendarioAgenda_eFechaYHoraSeleccionada(ByVal p_dtFechaYHora As Date, ByVal p_strNombreAgenda As String, ByVal p_intCodigoAgenda As Integer) Handles m_objfrmCalendarioAgenda.eFechaYHoraSeleccionada

            Try

                If p_strNombreAgenda <> "" Then
                    cboAgenda.Text = p_strNombreAgenda
                End If
                dtpFechaCita.Value = p_dtFechaYHora
                dtpHoraCita.Value = p_dtFechaYHora
                m_objfrmCalendarioAgenda.Hide()
                m_objfrmCalendarioAgenda.Dispose()
                m_objfrmCalendarioAgenda = Nothing

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picConfVehiculo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picConfVehiculo.Click

            Try

                If txtCodCliente.Text <> "" Then
                    If m_intIDvehiculo <> 0 Then
                        m_objFrmVehiculos = New frmCtrlInformacionVehiculos(frmCtrlInformacionVehiculos.enumModoInsercion.scgModificarPreseleccionado, m_intIDvehiculo)
                    Else
                        m_objFrmVehiculos = New frmCtrlInformacionVehiculos(txtCodCliente.Text, txtNombreCliente.Text)
                    End If
                    m_objFrmVehiculos.MdiParent = Me.MdiParent
                    m_objFrmVehiculos.Show()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picAsesor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picAsesor.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorAsesores
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Apellido '"Código, Nombre, Apellidos"
                m_objBuscador.Criterios = "empID, FirstName, lastName"
                m_objBuscador.Tabla = "SCGTA_VW_OHEM"
                m_objBuscador.Where = "userId is not null"
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub m_objFrmVehiculos_RetornaValores(ByRef p_drwVehiculo As DMSOneFramework.VehiculosDataset.SCGTA_VW_VehiculosRow) Handles m_objFrmVehiculos.RetornaValores

            Try


                m_intIDvehiculo = CType(p_drwVehiculo.IDVehiculo, Integer)
                If Not p_drwVehiculo.IsNoVehiculoNull Then
                    txtNoUnidad.Text = p_drwVehiculo.NoVehiculo
                    m_strNoVehiculo = p_drwVehiculo.NoVehiculo
                Else
                    txtNoUnidad.Text = ""
                    m_strNoVehiculo = ""
                End If
                If Not p_drwVehiculo.IsPlacaNull Then
                    txtPlaca.Text = p_drwVehiculo.Placa
                Else
                    txtPlaca.Text = ""
                End If
                If Not p_drwVehiculo.IsVINNull Then
                    txtPlaca.Tag = p_drwVehiculo.VIN
                Else
                    txtPlaca.Tag = ""
                End If
                txtMarca.Tag = p_drwVehiculo.CodMarca
                If Not p_drwVehiculo.IsDescMarcaNull Then
                    txtMarca.Text = p_drwVehiculo.DescMarca
                End If
                txtEstilo.Tag = p_drwVehiculo.CodEstilo
                If Not p_drwVehiculo.IsDescEstiloNull Then
                    txtEstilo.Text = p_drwVehiculo.DescEstilo
                Else
                    txtEstilo.Text = ""
                End If
                If Not p_drwVehiculo.IsCodModeloNull Then
                    txtModelo.Tag = p_drwVehiculo.CodModelo
                Else
                    txtModelo.Tag = ""
                End If
                If Not p_drwVehiculo.IsDescModeloNull Then
                    txtModelo.Text = p_drwVehiculo.DescModelo
                Else
                    txtModelo.Text = ""
                End If
                If Not p_drwVehiculo.IsAnoVehiculoNull Then
                    txtAnoVehiculo.Text = p_drwVehiculo.AnoVehiculo
                Else
                    txtAnoVehiculo.Text = ""
                End If
                If Not p_drwVehiculo.IsDescCombustibleNull Then
                    txtCombustible.Text = p_drwVehiculo.DescCombustible
                Else
                    txtCombustible.Text = ""
                End If
                txtCodCliente.Text = p_drwVehiculo.CardCode
                If Not p_drwVehiculo.IsClienteNull Then
                    txtNombreCliente.Text = p_drwVehiculo.Cliente
                Else
                    txtNombreCliente.Text = ""
                End If

                m_objFrmVehiculos.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picTecnico_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picTecnico.Click
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.TituloEmpleados
                m_objBuscador.Titulos = My.Resources.ResourceUI.Cod & "," & My.Resources.ResourceUI.Apellido & "," & My.Resources.ResourceUI.Nombre  '"Codigo, Nombre, Apellido"
                m_objBuscador.Criterios = "empID,firstName, lastName"
                m_objBuscador.Tabla = "SCGTA_VW_OHEM"
                m_objBuscador.Where = ""
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        'Public Sub guardarDatoscomponentesUDF()

        '    VisualizarUDFCita.Tabla = "SCGTA_TB_Cita"

        '    VisualizarUDFCita.Conexion = SCGDataAccess.DAConexion.ConnectionString

        '    VisualizarUDFCita.CampoLlave = "IDCita = " & m_intIDCita

        '    VisualizarUDFCita.Form = Me

        '    VisualizarUDFCita.VisualizarUDF()

        '    VisualizarUDFCita.Where = "NoCita = '" & m_drwCita.NoCita & "'"

        '    VisualizarUDFCita.UpdateDatosUDF(Me)

        'End Sub

        Private Function ValidarMonedaItems(ByVal p_dstItems As QUT1Dataset) As Boolean

            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal = 0
            Dim strMonedaLocal As String = String.Empty
            Dim strMonedas As String = String.Empty
            Dim drw As QUT1Dataset.QUT1Row
            objBLSBO = New BLSBO.GlobalFunctionsSBO

            strMonedaLocal = objBLSBO.RetornarMonedaLocal

            'Ciclo para Validar las Monedas de los Items
            For Each drw In p_dstItems.QUT1.Rows
                strMonedas = drw.Item("Moneda").ToString().Trim()

                If strMonedas <> strMonedaLocal And strMonedas <> "" Then
                    decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strMonedas, Today, strConectionString, True)
                End If

                If decTipoCambio = -1 Then
                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + strMonedas + My.Resources.ResourceUI.ParaLaFecha + Today)
                    Return False
                End If

            Next

            Return True

        End Function

        Private Function CreateSelectCommandListaPreciosCliente(ByVal p_strCodCliente As String) As SqlClient.SqlCommand

            Try
                Dim cmdSelListaPreciosCliente As New SqlClient.SqlCommand("Select ListNum from SCGTA_VW_Clientes where CardCode=" & "'" & p_strCodCliente & "'")

                cmdSelListaPreciosCliente.CommandType = CommandType.Text

                Return cmdSelListaPreciosCliente

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Function
#End Region




    End Class

End Namespace