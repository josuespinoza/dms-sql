Imports SCG.DMSOne.Framework
Imports Proyecto_SCGToolBar.SCGToolBar
Imports DMSOneFramework
Imports DMSOneFramework.SCGCommon
Imports SCG.UX.Windows

Namespace SCG_User_Interface
    Public Class frmCtrlInformacionVehiculos

#Region "Constructor"

        Public Sub New(ByVal p_blnEstado As Boolean)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()            
            m_intTipoInsercion = enumModoInsercion.scgNuevo
            HabilitarControles()

            'Me.Width = 619

            'VisualizarUDFVehiculo.BackColor = 
            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

        Public Sub New(ByVal p_intTipoInsercion As enumModoInsercion, Optional ByVal p_intIDVehiculo As Integer = -1)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()
            m_intTipoInsercion = p_intTipoInsercion
            If p_intIDVehiculo <> -1 Then

                CrearFilaAModificar(p_intIDVehiculo)

            End If
            HabilitarControles()
            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

        Public Sub New(ByVal p_strCardCode As String, ByVal p_strCardName As String)

            MyBase.New()
            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()
            txtCardCode.Text = p_strCardCode
            txtCardName.Text = p_strCardName
            m_intTipoInsercion = enumModoInsercion.scgNuevo
            HabilitarControles()
            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        End Sub

#End Region

#Region "Declaraciones"

#Region "Enums"

        Public Enum enumModoInsercion
            scgNuevo = 1
            scgModificar = 2
            scgSoloLectura = 3
            scgModificarPreseleccionado = 4
        End Enum

#End Region

#Region "Eventos"

        Public Event RetornaValores(ByRef p_drwVehiculo As VehiculosDataset.SCGTA_VW_VehiculosRow)

#End Region

#Region "Objetos"

        Private m_objUtilitarios As New Utilitarios(strConexionADO)
        Private m_objMensajes As New Proyecto_SCGMSGBox.SCGMSGBox
        Private WithEvents m_objBuscadorVehiculos As New Buscador.SubBuscador

#End Region

#Region "Variables"

        Private m_intTipoInsercion As enumModoInsercion
        Private m_blnIniciando As Boolean

#End Region

#Region "Acceso a Datos"

        Private m_adpVehiculos As New VehiculosDataAdapter
        Private m_dtsVehiculos As New VehiculosDataset

        'Objetos para los catálogos
        Private m_adpMarcas As New MarcaDataAdapter
        Private m_adpEstilos As New EstiloDataAdapter
        Private m_adpModelos As New ModelosDataAdapter

        Private m_drwVehiculo As VehiculosDataset.SCGTA_VW_VehiculosRow

        Private m_drdCatalogo As SqlClient.SqlDataReader

#End Region

#End Region

#Region "Eventos"

        Private Sub tlbVehiculos_Click_Buscar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVehiculos.Click_Buscar
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscadorVehiculos = New Buscador.SubBuscador
                m_objBuscadorVehiculos.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscadorVehiculos.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorVehiculos


                m_objBuscadorVehiculos.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.CardCode & "," & My.Resources.ResourceUI.Cliente & _
                                                "," & My.Resources.ResourceUI.Placa & "," & My.Resources.ResourceUI.NoUnidad & _
                                                "," & My.Resources.ResourceUI.VIN


                m_objBuscadorVehiculos.Criterios = "top 50 IDVehiculo, CardCode, Cliente, Placa, NoVehiculo, VIN"
                m_objBuscadorVehiculos.Tabla = "SCGTA_VW_Vehiculos"
                '  m_objBuscadorVehiculos.Criterios_OcultosEx = "1"
                m_objBuscadorVehiculos.Where = ""
                m_objBuscadorVehiculos.ConsultarDBPorFiltrado = True
                m_objBuscadorVehiculos.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub chkFechaVenta_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFechaVenta.CheckedChanged

            Try

                dtpFechaVenta.Enabled = chkFechaVenta.Checked

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbVehiculos_Click_Cancelar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVehiculos.Click_Cancelar

            Try

                Call LimpiarControles()
                m_intTipoInsercion = enumModoInsercion.scgNuevo
                Call HabilitarControles()
                m_drwVehiculo = Nothing

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbVehiculos_Click_Guardar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVehiculos.Click_Guardar

            Dim codeVehiculo As String = ""
            Try
                If ValidarDatos() Then
                    Call PasarDatosADataRow()
                    If m_intTipoInsercion <> enumModoInsercion.scgNuevo Then
                        m_adpVehiculos.Update(m_dtsVehiculos)
                    Else
                        InsertarVehiculo(codeVehiculo)
                        m_drwVehiculo.IDVehiculo = codeVehiculo
                        m_drwVehiculo.AcceptChanges()
                    End If
                    RaiseEvent RetornaValores(m_drwVehiculo)
                    'Call tlbVehiculos_Click_Cancelar(sender, e)
                    m_drwVehiculo = Nothing
                    btnArchivos.Enabled = False

'                    VisualizarUDFVehiculo.UpdateDatosUDF_SBO(Me)
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeProcesoSatisfactorio)

                    'Agregado 09072010
                    Call LimpiarControles()

                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub InsertarVehiculo(Optional ByRef p_IdVehiculo As String = "")


            Dim udoVeh As UDOVehiculos = New UDOVehiculos(G_objCompany, AddressOf AutoKey)
            udoVeh.Encabezado = New EncabezadoUDOVehiculos()

            If udoVeh.Encabezado Is Nothing Then
                m_drwVehiculo = m_dtsVehiculos.SCGTA_VW_Vehiculos.NewSCGTA_VW_VehiculosRow
            End If

            If chkFechaVenta.Checked Then

                udoVeh.Encabezado.FechaVenta = dtpFechaVenta.Value

            End If

            If chkFechaUltimoServicio.Checked Then
                udoVeh.Encabezado.FechaUltimoServicio = dtpFechaUltimoServicio.Value
            End If

            If chkFechaPxServicio.Checked Then
                udoVeh.Encabezado.FechaProxServicio = dtpFechaPxServicio.Value
            End If

            If chkFechaReserva.Checked Then
                udoVeh.Encabezado.FechaReserva = dtpFechaReserva.Value
            End If

            If chkFechaVencimientoReserva.Checked Then
                udoVeh.Encabezado.FechaVencidoReserva = dtpFechaVencimientoReserva.Value
            End If

            If Not String.IsNullOrEmpty(txtNoPedidoFab.Text) Then
                udoVeh.Encabezado.NumeroPedidoFabrica = txtNoPedidoFab.Text
            End If

            If txtAño.Text <> "" Then
                udoVeh.Encabezado.Ano = txtAño.Text
            End If

            udoVeh.Encabezado.CodigoCliente = txtCardCode.Text
            udoVeh.Encabezado.NombreCliente = txtCardName.Text

            If txtCilindrada.Text <> "" Then
                udoVeh.Encabezado.Cilindrada = txtCilindrada.Text
            End If

            If txtGarantiaAños.Text <> "" Then
                udoVeh.Encabezado.GarantiaTiempo = txtGarantiaAños.Text
            End If

            If txtGarantiaKM.Text <> "" Then
                udoVeh.Encabezado.GarantiaKm = txtGarantiaKM.Text
            End If

            If txtNoCilindros.Text <> "" Then
                udoVeh.Encabezado.Cilindros = txtNoCilindros.Text
            End If

            If txtNoEjes.Text <> "" Then
                udoVeh.Encabezado.Ejes = txtNoEjes.Text
            End If

            If txtNoMotor.Text <> "" Then
                udoVeh.Encabezado.NumeroMotor = txtNoMotor.Text
            End If

            If txtNoPasajeros.Text <> "" Then
                udoVeh.Encabezado.CantidadPasajeros = txtNoPasajeros.Text
            End If

            If txtNoPuertas.Text <> "" Then
                udoVeh.Encabezado.Puertas = txtNoPuertas.Text
            End If

            If txtNoUnidad.Text <> "" Then
                udoVeh.Encabezado.NoUnidad = txtNoUnidad.Text
            End If

            If txtObservaciones.Text <> "" Then
                udoVeh.Encabezado.Accesorios = txtObservaciones.Text
            End If

            If txtPeso.Text <> "" Then
                udoVeh.Encabezado.Peso = txtPeso.Text
            End If

            If txtPlaca.Text <> "" Then
                udoVeh.Encabezado.Placa = txtPlaca.Text
            End If

            If txtPotenciaKW.Text <> "" Then
                udoVeh.Encabezado.Potencia = txtPotenciaKW.Text
            End If

            If txtVIN.Text <> "" Then
                udoVeh.Encabezado.Vin = txtVIN.Text
            End If

            If cboCabina.SelectedIndex > -1 Then
                udoVeh.Encabezado.Cabina = cboCabina.SelectedValue
            End If

            If cboCarroceria.SelectedIndex > -1 Then
                udoVeh.Encabezado.Carroceria = cboCarroceria.SelectedValue
            End If

            If cboCategoria.SelectedIndex > -1 Then
                udoVeh.Encabezado.Categoria = cboCategoria.SelectedValue
            End If

            If cboColor.SelectedIndex > -1 Then
                udoVeh.Encabezado.CodigoColor = cboColor.SelectedValue
            End If

            If cboColorTapiceria.SelectedIndex > -1 Then
                udoVeh.Encabezado.ColorTapiceria = cboColorTapiceria.SelectedValue
            End If

            If cboCombustible.SelectedIndex > -1 Then
                udoVeh.Encabezado.Combustible = cboCombustible.SelectedValue
            End If

            If cboEstado.SelectedIndex > -1 Then
                udoVeh.Encabezado.Estado = cboEstado.SelectedValue
            End If

            udoVeh.Encabezado.CodigoMarca = cboMarca.SelectedValue
            udoVeh.Encabezado.CodigoEstilo = cboEstilo.SelectedValue

            udoVeh.Encabezado.Marca = cboMarca.Text
            udoVeh.Encabezado.Estilo = cboEstilo.Text

            If cboMarcaMotor.SelectedIndex > -1 Then
                udoVeh.Encabezado.MarcaMotor = cboMarcaMotor.SelectedValue
            End If

            If cboModelo.SelectedIndex > -1 Then
                udoVeh.Encabezado.CodigoModelo = cboModelo.SelectedValue
                udoVeh.Encabezado.Modelo = cboModelo.Text
            End If

            If cboTecho.SelectedIndex > -1 Then
                udoVeh.Encabezado.TipoTecho = cboTecho.SelectedValue
            End If

            If cboTipo.SelectedIndex > -1 Then
                udoVeh.Encabezado.Tipo = cboTipo.SelectedValue
            End If

            If cboTraccion.SelectedIndex > -1 Then
                udoVeh.Encabezado.TipoTraccion = cboTraccion.SelectedValue
            End If

            If cboTransmision.SelectedIndex > -1 Then
                udoVeh.Encabezado.Transmision = cboTransmision.SelectedValue
            End If

            If cboUbicacion.SelectedIndex > -1 Then
                udoVeh.Encabezado.CodigoUbicacion = cboUbicacion.SelectedValue
            End If

            'Utilitarios.EjecutarConsulta("Update [@SCGD_VEHICULO] set Code = DocEntry Where Code = '" & m_drwVehiculo.IDVehiculo & "'")
            ', m_oCompany.CompanyDB, m_oCompany.Server)

            udoVeh.Insert()


            'Dim strIDVehiculo As String = Utilitarios.EjecutarConsulta("Select Docentry from [@SCGD_VEHICULO] where U_Cod_Unid = '" & m_drwVehiculo.NoVehiculo & "'",cn m_oCompany.CompanyDB, m_oCompany.Server)

            Dim docentryVehiculo As String = udoVeh.Encabezado.Code

            p_IdVehiculo = udoVeh.Encabezado.Code
            'Dim strIDVehiculo As String = Utilitarios.EjecutarConsulta("Select Docentry from [@SCGD_VEHICULO] where U_Cod_Unid = '" & m_drwVehiculo.NoVehiculo & "'", G_objCompany.CompanyDB, G_objCompany.Server)


        End Sub

        Private Function AutoKey(ByVal udoId As String) As Integer
            Return Utilitarios.ObtieneAutoKey(udoId, strConexionADO)
        End Function

        Private Sub tlbVehiculos_Click_Cerrar(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVehiculos.Click_Cerrar
            Try

                Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub frmCtrlInformacionVehiculos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try

                m_blnIniciando = True
                Call CargarCatalogos()

'                Call VisualizarUDF()

                m_blnIniciando = False
                If m_drwVehiculo IsNot Nothing Then
                    Call MostrarDatosPantalla()

                End If

                tlbVehiculos.Buttons(3).Style = ToolBarButtonStyle.DropDownButton
                tlbVehiculos.Buttons(3).DropDownMenu = mnuImprimir



            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub cboMarca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMarca.SelectedIndexChanged
            Try

                If Not m_blnIniciando Then
                    If cboMarca.SelectedIndex <> -1 Then
                        m_blnIniciando = True
                        cboEstilo.DataSource = Nothing
                        If Not m_drdCatalogo.IsClosed Then
                            m_drdCatalogo.Close()
                        End If
                        m_adpEstilos.CargaEstilosdeVehiculo(m_drdCatalogo, cboMarca.SelectedValue)
                        Utilitarios.CargarComboSourceByReader(cboEstilo, m_drdCatalogo)
                        m_blnIniciando = False
                        cboEstilo.SelectedIndex = -1

                    Else
                        cboEstilo.DataSource = Nothing
                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            Finally
                'Agregado 01072010
                If m_drdCatalogo IsNot Nothing Then
                    If Not m_drdCatalogo.IsClosed Then
                        Call m_drdCatalogo.Close()
                    End If
                End If
            End Try
        End Sub

        Private Sub cboEstilo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboEstilo.SelectedIndexChanged

            Try

                If Not m_blnIniciando Then
                    If cboEstilo.SelectedIndex <> -1 Then
                        cboModelo.DataSource = Nothing
                        If Not m_drdCatalogo.IsClosed Then
                            m_drdCatalogo.Close()
                        End If
                        m_adpModelos.CargaModelosdeVehiculo(m_drdCatalogo, cboEstilo.SelectedValue)
                        Utilitarios.CargarComboSourceByReader(cboModelo, m_drdCatalogo)
                        cboModelo.SelectedIndex = -1
                    Else
                        cboModelo.DataSource = Nothing
                    End If
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 02072010
                If m_drdCatalogo IsNot Nothing Then
                    If Not m_drdCatalogo.IsClosed Then
                        Call m_drdCatalogo.Close()
                    End If
                End If
            End Try

        End Sub

        Private Sub m_objBuscadorVehiculos_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscadorVehiculos.AppAceptar

            Try
                Select Case sender.name
                    Case tlbVehiculos.Name
                        m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Clear()
                        m_adpVehiculos.Fill(m_dtsVehiculos, Arreglo_Campos(0))
                        If m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Count > 0 Then
                            Call LimpiarControles()
                            m_drwVehiculo = Nothing
                            m_drwVehiculo = m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows(0)
                        End If
                        m_intTipoInsercion = enumModoInsercion.scgModificar

                        Call MostrarDatosPantalla()
                        'Call VisualizarUDF()
'                        VisualizarUDFVehiculo.Where = "U_SCGD_Cod_Unid = '" & Arreglo_Campos(4) & "'"

'                        VisualizarUDFVehiculo.CargarDatosUDF_SBO("U_SCGD_Cod_Unid = '" & Arreglo_Campos(4) & "'")
                        Call HabilitarControles()

                    Case picCliente.Name
                        txtCardCode.Text = Arreglo_Campos(0)
                        txtCardName.Text = Arreglo_Campos(1)
                End Select
                btnArchivos.Enabled = Not m_drwVehiculo Is Nothing
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub picCliente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picCliente.Click

            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscadorVehiculos = New Buscador.SubBuscador
                m_objBuscadorVehiculos.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscadorVehiculos.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorClientes
                m_objBuscadorVehiculos.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre
                m_objBuscadorVehiculos.Criterios = "top 50 CardCode, CardName"
                m_objBuscadorVehiculos.Tabla = "SCGTA_VW_Clientes"
                m_objBuscadorVehiculos.Where = ""
                m_objBuscadorVehiculos.ConsultarDBPorFiltrado = True
                m_objBuscadorVehiculos.Activar_Buscador(sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbVehiculos_Click_Nuevo(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVehiculos.Click_Nuevo

            Try

                m_drwVehiculo = Nothing
                Call tlbVehiculos_Click_Cancelar(sender, e)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub tlbVehiculos_Click_Imprimir(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbVehiculos.Click_Imprimir
            MostrarReporteFichaVehiculo()
        End Sub

        Private Sub mnuFichaVehiculo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFichaVehiculo.Click
            MostrarReporteFichaVehiculo()
        End Sub

        Private Sub btnArchivos_Click(ByVal sender As [Object], ByVal e As System.EventArgs) Handles btnArchivos.Click
            Dim archivoDigital As FrmArchivoDigital = New FrmArchivoDigital(My.Resources.ResourceUI.TituloArchivosDigitales, "SCGTA_VW_Vehiculos", m_drwVehiculo.IDVehiculo, g_strTablaArchivosDigitales, SCGDataAccess.DAConexion.strConectionString, 10, GlobalesUI.g_TipoSkin)
            archivoDigital.StartPosition = FormStartPosition.CenterParent
            archivoDigital.ShowDialog()

        End Sub

        Private Sub chkFechaPxServicio_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFechaPxServicio.CheckedChanged
            dtpFechaPxServicio.Enabled = chkFechaPxServicio.Checked
        End Sub

        Private Sub chkFechaUltimoServicio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFechaUltimoServicio.CheckedChanged
            dtpFechaUltimoServicio.Enabled = chkFechaUltimoServicio.Checked
        End Sub

        Private Sub chkFechaReserva_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFechaReserva.CheckedChanged
            dtpFechaReserva.Enabled = chkFechaReserva.Checked
        End Sub

        Private Sub chkFechaVencimientoReserva_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFechaVencimientoReserva.CheckedChanged
            dtpFechaVencimientoReserva.Enabled = chkFechaVencimientoReserva.Checked
        End Sub


        Private Sub mnuHistorialResumido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHistorialResumido.Click
            CargaReporteHistorialResumido()
        End Sub

#End Region

#Region "Métodos"

        Private Sub CrearFilaAModificar(ByVal p_intIDVehiculo As Integer)

            m_adpVehiculos.Fill(m_dtsVehiculos, p_intIDVehiculo)
            If m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Count > 0 Then
                m_drwVehiculo = m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows(0)
            End If

        End Sub

        Private Sub MostrarDatosPantalla()

            If Not m_drwVehiculo.IsFechaProximoServicioNull Then
                dtpFechaPxServicio.Value = m_drwVehiculo.FechaProximoServicio
                chkFechaPxServicio.Checked = True
            End If

            If Not m_drwVehiculo.IsFechaUltimoServicioNull Then
                dtpFechaUltimoServicio.Value = m_drwVehiculo.FechaUltimoServicio
                chkFechaUltimoServicio.Checked = True
            End If

            If Not m_drwVehiculo.IsFechaReservaNull Then
                dtpFechaReserva.Value = m_drwVehiculo.FechaReserva
                chkFechaReserva.Checked = True
            End If

            If Not m_drwVehiculo.IsFechaVencimientoReservaNull Then
                dtpFechaVencimientoReserva.Value = m_drwVehiculo.FechaVencimientoReserva
                chkFechaVencimientoReserva.Checked = True
            End If

            If Not m_drwVehiculo.IsNoPedidoFabricaNull Then
                txtNoPedidoFab.Text = m_drwVehiculo.NoPedidoFabrica
            End If

            If Not m_drwVehiculo.IsFechaVentaNull Then
                dtpFechaVenta.Value = m_drwVehiculo.FechaVenta
                chkFechaVenta.Checked = True
            Else
                chkFechaVenta.Checked = False
            End If

            If Not m_drwVehiculo.IsAnoVehiculoNull Then
                txtAño.Text = m_drwVehiculo.AnoVehiculo
            End If
            If Not m_drwVehiculo.IsCardCodeNull Then
                txtCardCode.Text = m_drwVehiculo.CardCode
            End If

            If Not m_drwVehiculo.IsClienteNull Then
                txtCardName.Text = m_drwVehiculo.Cliente
            End If
            If Not m_drwVehiculo.IsCilindradaNull Then
                txtCilindrada.Text = m_drwVehiculo.Cilindrada
            End If
            If Not m_drwVehiculo.IsGarantiaTMNull Then
                txtGarantiaAños.Text = m_drwVehiculo.GarantiaTM
            End If
            If Not m_drwVehiculo.IsGarantiaKMNull Then
                txtGarantiaKM.Text = m_drwVehiculo.GarantiaKM
            End If
            If Not m_drwVehiculo.IsNumeroCilindrosNull Then
                txtNoCilindros.Text = m_drwVehiculo.NumeroCilindros
            End If
            If Not m_drwVehiculo.IsCantEjesNull Then
                txtNoEjes.Text = m_drwVehiculo.CantEjes
            End If
            If Not m_drwVehiculo.IsNum_MotorNull Then
                txtNoMotor.Text = m_drwVehiculo.Num_Motor
            End If
            If Not m_drwVehiculo.IsCant_PasajerosNull Then
                txtNoPasajeros.Text = m_drwVehiculo.Cant_Pasajeros
            End If
            If Not m_drwVehiculo.IsCantidadPuertasNull Then
                txtNoPuertas.Text = m_drwVehiculo.CantidadPuertas
            End If
            If Not m_drwVehiculo.IsNoVehiculoNull Then
                txtNoUnidad.Text = m_drwVehiculo.NoVehiculo
            End If
            If Not m_drwVehiculo.IsAccesoriosNull Then
                txtObservaciones.Text = m_drwVehiculo.Accesorios
            End If
            If Not m_drwVehiculo.IsPesoNull Then
                txtPeso.Text = m_drwVehiculo.Peso
            End If
            If Not m_drwVehiculo.IsPlacaNull Then
                txtPlaca.Text = m_drwVehiculo.Placa
            End If
            If Not m_drwVehiculo.IsPotenciaNull Then
                txtPotenciaKW.Text = m_drwVehiculo.Potencia
            End If
            If Not m_drwVehiculo.IsVINNull Then
                txtVIN.Text = m_drwVehiculo.VIN
            End If

            If Not m_drwVehiculo.IsDescCabinaNull Then
                cboCabina.Text = m_drwVehiculo.DescCabina
            End If
            If Not m_drwVehiculo.IsDescCarroceriaNull Then
                cboCarroceria.Text = m_drwVehiculo.DescCarroceria
            End If
            If Not m_drwVehiculo.IsDescCategoriaNull Then
                cboCategoria.Text = m_drwVehiculo.DescCategoria
            End If
            If Not m_drwVehiculo.IsColorNull Then
                cboColor.Text = m_drwVehiculo.Color
            End If
            If Not m_drwVehiculo.IsColorTapiceriaNull Then
                cboColorTapiceria.Text = m_drwVehiculo.ColorTapiceria
            End If
            If Not m_drwVehiculo.IsDescCombustibleNull Then
                cboCombustible.Text = m_drwVehiculo.DescCombustible
            End If
            If Not m_drwVehiculo.IsEstatusNull Then
                cboEstado.Text = m_drwVehiculo.Estatus
            End If
            If Not m_drwVehiculo.IsDescMarcaNull Then
                cboMarca.Text = m_drwVehiculo.DescMarca
            End If
            If Not m_drwVehiculo.IsDescEstiloNull Then
                cboEstilo.Text = m_drwVehiculo.DescEstilo
            End If
            If Not m_drwVehiculo.IsDescMarca_MotorNull Then
                cboMarcaMotor.Text = m_drwVehiculo.DescMarca_Motor
            End If
            If Not m_drwVehiculo.IsDescModeloNull Then
                cboModelo.Text = m_drwVehiculo.DescModelo
            End If
            If Not m_drwVehiculo.IsTipoTechoNull Then
                cboTecho.Text = m_drwVehiculo.DescTecho
            End If
            If Not m_drwVehiculo.IsTipoNull Then
                cboTipo.Text = m_drwVehiculo.DescTipo
            End If
            If Not m_drwVehiculo.IsDescTraccionNull Then
                cboTraccion.Text = m_drwVehiculo.DescTraccion
            End If
            If Not m_drwVehiculo.IsDescTransmisionNull Then
                cboTransmision.Text = m_drwVehiculo.DescTransmision
            End If
            If Not m_drwVehiculo.IsDescUbicacionNull Then
                cboUbicacion.Text = m_drwVehiculo.DescUbicacion
            End If


        End Sub

        Private Sub PasarDatosADataRow()

            If m_intTipoInsercion = enumModoInsercion.scgNuevo Then
                m_drwVehiculo = Nothing
            End If

            If m_drwVehiculo Is Nothing Then
                m_drwVehiculo = m_dtsVehiculos.SCGTA_VW_Vehiculos.NewSCGTA_VW_VehiculosRow
            End If

            If chkFechaVenta.Checked Then

                m_drwVehiculo.FechaVenta = dtpFechaVenta.Value
            Else

                m_drwVehiculo.SetFechaVentaNull()

            End If

            If chkFechaUltimoServicio.Checked Then
                m_drwVehiculo.FechaUltimoServicio = dtpFechaUltimoServicio.Value
            Else
                m_drwVehiculo.SetFechaUltimoServicioNull()
            End If

            If chkFechaPxServicio.Checked Then
                m_drwVehiculo.FechaProximoServicio = dtpFechaPxServicio.Value
            Else
                m_drwVehiculo.SetFechaProximoServicioNull()
            End If

            If chkFechaReserva.Checked Then
                m_drwVehiculo.FechaReserva = dtpFechaReserva.Value
            Else
                m_drwVehiculo.SetFechaReservaNull()
            End If

            If chkFechaVencimientoReserva.Checked Then
                m_drwVehiculo.FechaVencimientoReserva = dtpFechaVencimientoReserva.Value
            Else
                m_drwVehiculo.SetFechaVencimientoReservaNull()
            End If

            If String.IsNullOrEmpty(txtNoPedidoFab.Text) Then
                m_drwVehiculo.SetNoPedidoFabricaNull()
            Else
                m_drwVehiculo.NoPedidoFabrica = txtNoPedidoFab.Text
            End If

            If txtAño.Text <> "" Then
                m_drwVehiculo.AnoVehiculo = txtAño.Text
            Else
                m_drwVehiculo.SetAnoVehiculoNull()
            End If
            m_drwVehiculo.CardCode = txtCardCode.Text
            m_drwVehiculo.Cliente = txtCardName.Text
            If txtCilindrada.Text <> "" Then
                m_drwVehiculo.Cilindrada = txtCilindrada.Text
            Else
                m_drwVehiculo.SetCilindradaNull()
            End If
            If txtGarantiaAños.Text <> "" Then
                m_drwVehiculo.GarantiaTM = txtGarantiaAños.Text
            Else
                m_drwVehiculo.SetGarantiaTMNull()
            End If
            If txtGarantiaKM.Text <> "" Then
                m_drwVehiculo.GarantiaKM = txtGarantiaKM.Text
            Else
                m_drwVehiculo.SetGarantiaKMNull()
            End If
            If txtNoCilindros.Text <> "" Then
                m_drwVehiculo.NumeroCilindros = txtNoCilindros.Text
            Else
                m_drwVehiculo.SetNumeroCilindrosNull()
            End If
            If txtNoEjes.Text <> "" Then
                m_drwVehiculo.CantEjes = txtNoEjes.Text
            Else
                m_drwVehiculo.SetCantEjesNull()
            End If
            If txtNoMotor.Text <> "" Then
                m_drwVehiculo.Num_Motor = txtNoMotor.Text
            Else
                m_drwVehiculo.SetNum_MotorNull()
            End If
            If txtNoPasajeros.Text <> "" Then
                m_drwVehiculo.Cant_Pasajeros = txtNoPasajeros.Text
            Else
                m_drwVehiculo.SetCant_PasajerosNull()
            End If
            If txtNoPuertas.Text <> "" Then
                m_drwVehiculo.CantidadPuertas = txtNoPuertas.Text
            Else
                m_drwVehiculo.SetCantidadPuertasNull()
            End If
            If txtNoUnidad.Text <> "" Then
                m_drwVehiculo.NoVehiculo = txtNoUnidad.Text
            Else
                m_drwVehiculo.SetNoVehiculoNull()
            End If
            If txtObservaciones.Text <> "" Then
                m_drwVehiculo.Accesorios = txtObservaciones.Text
            Else
                m_drwVehiculo.SetAccesoriosNull()
            End If

            If txtPeso.Text <> "" Then
                m_drwVehiculo.Peso = txtPeso.Text
            Else
                m_drwVehiculo.SetPesoNull()
            End If
            If txtPlaca.Text <> "" Then
                m_drwVehiculo.Placa = txtPlaca.Text
            Else
                m_drwVehiculo.SetPlacaNull()
            End If

            If txtPotenciaKW.Text <> "" Then
                m_drwVehiculo.Potencia = txtPotenciaKW.Text
            Else
                m_drwVehiculo.SetPotenciaNull()
            End If
            If txtVIN.Text <> "" Then
                m_drwVehiculo.VIN = txtVIN.Text
            Else
                m_drwVehiculo.SetVINNull()
            End If

            If cboCabina.SelectedIndex > -1 Then
                m_drwVehiculo.CodTipoCabina = cboCabina.SelectedValue
            Else
                m_drwVehiculo.SetCodTipoCabinaNull()
            End If
            If cboCarroceria.SelectedIndex > -1 Then
                m_drwVehiculo.CodCarroceria = cboCarroceria.SelectedValue
            Else
                m_drwVehiculo.SetCodCarroceriaNull()
            End If
            If cboCategoria.SelectedIndex > -1 Then
                m_drwVehiculo.CodCategoria = cboCategoria.SelectedValue
            Else
                m_drwVehiculo.SetCodCategoriaNull()
            End If
            If cboColor.SelectedIndex > -1 Then
                m_drwVehiculo.CodigoColor = cboColor.SelectedValue
            Else
                m_drwVehiculo.SetCodigoColorNull()
            End If
            If cboColorTapiceria.SelectedIndex > -1 Then
                m_drwVehiculo.CodColorTap = cboColorTapiceria.SelectedValue
            Else
                m_drwVehiculo.SetCodColorTapNull()
            End If
            If cboCombustible.SelectedIndex > -1 Then
                m_drwVehiculo.CodCombustible = cboCombustible.SelectedValue
            Else
                m_drwVehiculo.SetCodCombustibleNull()
            End If
            If cboEstado.SelectedIndex > -1 Then
                m_drwVehiculo.CodEstatus = cboEstado.SelectedValue
            Else
                m_drwVehiculo.SetCodEstatusNull()
            End If

            m_drwVehiculo.CodMarca = cboMarca.SelectedValue
            m_drwVehiculo.CodEstilo = cboEstilo.SelectedValue

            m_drwVehiculo.DescMarca = cboMarca.Text
            m_drwVehiculo.DescEstilo = cboEstilo.Text

            If cboMarcaMotor.SelectedIndex > -1 Then
                m_drwVehiculo.CodMarcaMotor = cboMarcaMotor.SelectedValue
            Else
                m_drwVehiculo.SetCodMarcaMotorNull()
            End If
            If cboModelo.SelectedIndex > -1 Then
                m_drwVehiculo.CodModelo = cboModelo.SelectedValue
                m_drwVehiculo.DescModelo = cboModelo.Text
            Else
                m_drwVehiculo.SetCodModeloNull()
            End If
            If cboTecho.SelectedIndex > -1 Then
                m_drwVehiculo.TipoTecho = cboTecho.SelectedValue
            Else
                m_drwVehiculo.SetTipoTechoNull()
            End If
            If cboTipo.SelectedIndex > -1 Then
                m_drwVehiculo.Tipo = cboTipo.SelectedValue
            Else
                m_drwVehiculo.SetTipoNull()
            End If
            If cboTraccion.SelectedIndex > -1 Then
                m_drwVehiculo.TipoTraccion = cboTraccion.SelectedValue
            Else
                m_drwVehiculo.SetTipoTraccionNull()
            End If
            If cboTransmision.SelectedIndex > -1 Then
                m_drwVehiculo.CodTransmision = cboTransmision.SelectedValue
            Else
                m_drwVehiculo.SetCodTransmisionNull()
            End If
            If cboUbicacion.SelectedIndex > -1 Then
                m_drwVehiculo.CodigoUbicacion = cboUbicacion.SelectedValue
            Else
                m_drwVehiculo.SetCodigoUbicacionNull()
            End If

            If m_intTipoInsercion = enumModoInsercion.scgNuevo Then

                m_dtsVehiculos.SCGTA_VW_Vehiculos.AddSCGTA_VW_VehiculosRow(m_drwVehiculo)

            End If

        End Sub

        Private Sub HabilitarControles()


            tlbVehiculos.Buttons(enumButton.Exportar).Visible = False
            tlbVehiculos.Buttons(enumButton.Imprimir).Visible = True
            tlbVehiculos.Buttons(enumButton.Eliminar).Visible = False
            tlbVehiculos.Buttons(enumButton.Buscar).Enabled = True
            tlbVehiculos.Buttons(enumButton.Imprimir).Enabled = True


            Select Case m_intTipoInsercion

                Case enumModoInsercion.scgModificar
                    tlbVehiculos.Buttons(enumButton.Nuevo).Enabled = False

                Case enumModoInsercion.scgNuevo
                    tlbVehiculos.Buttons(enumButton.Nuevo).Enabled = True

                Case enumModoInsercion.scgModificarPreseleccionado
                    tlbVehiculos.Buttons(enumButton.Cancelar).Enabled = False
                    tlbVehiculos.Buttons(enumButton.Nuevo).Enabled = False

            End Select


        End Sub

        Private Sub CargarCatalogos()
            Try
                'Cabinas
                m_drdCatalogo = m_adpVehiculos.FillReaderCabinas()
                Utilitarios.CargarComboSourceByReader(cboCabina, m_drdCatalogo)
                cboCabina.SelectedIndex = -1

                'Carroceria
                m_drdCatalogo = m_adpVehiculos.FillReaderCarroceria()
                Utilitarios.CargarComboSourceByReader(cboCarroceria, m_drdCatalogo)
                cboCarroceria.SelectedIndex = -1

                'Categorias
                m_drdCatalogo = m_adpVehiculos.FillReaderCategorias()
                Utilitarios.CargarComboSourceByReader(cboCategoria, m_drdCatalogo)
                cboCategoria.SelectedIndex = -1

                'Color
                m_drdCatalogo = m_adpVehiculos.FillReaderColor()
                Utilitarios.CargarComboSourceByReader(cboColor, m_drdCatalogo)
                cboColor.SelectedIndex = -1

                'Color Tapicería
                m_drdCatalogo = m_adpVehiculos.FillReaderColor()
                Utilitarios.CargarComboSourceByReader(cboColorTapiceria, m_drdCatalogo)
                cboColorTapiceria.SelectedIndex = -1

                'Combustible
                m_drdCatalogo = m_adpVehiculos.FillReaderCombustible()
                Utilitarios.CargarComboSourceByReader(cboCombustible, m_drdCatalogo)
                cboCombustible.SelectedIndex = -1

                'Estado Vehiculo
                m_drdCatalogo = m_adpVehiculos.FillReaderEstadoVehiculo()
                Utilitarios.CargarComboSourceByReader(cboEstado, m_drdCatalogo)
                cboEstado.SelectedIndex = -1

                'Marca_Motor
                m_drdCatalogo = m_adpVehiculos.FillReaderMarca_Motor()
                Utilitarios.CargarComboSourceByReader(cboMarcaMotor, m_drdCatalogo)
                cboMarcaMotor.SelectedIndex = -1

                'Techo
                m_drdCatalogo = m_adpVehiculos.FillReaderTecho()
                Utilitarios.CargarComboSourceByReader(cboTecho, m_drdCatalogo)
                cboTecho.SelectedIndex = -1

                'Tipo
                m_drdCatalogo = m_adpVehiculos.FillReaderTipoVehiculo()
                Utilitarios.CargarComboSourceByReader(cboTipo, m_drdCatalogo)
                cboTipo.SelectedIndex = -1

                'Traccion
                m_drdCatalogo = m_adpVehiculos.FillReaderTraccion()
                Utilitarios.CargarComboSourceByReader(cboTraccion, m_drdCatalogo)
                cboTraccion.SelectedIndex = -1

                'Transmision
                m_drdCatalogo = m_adpVehiculos.FillReaderTransmision()
                Utilitarios.CargarComboSourceByReader(cboTransmision, m_drdCatalogo)
                cboTransmision.SelectedIndex = -1

                'Ubicaciones
                m_drdCatalogo = m_adpVehiculos.FillReaderUbicaciones()
                Utilitarios.CargarComboSourceByReader(cboUbicacion, m_drdCatalogo)
                cboUbicacion.SelectedIndex = -1

                'Marcas
                m_adpMarcas.CargaMarcasdeVehiculo(m_drdCatalogo)
                Utilitarios.CargarComboSourceByReader(cboMarca, m_drdCatalogo)
                cboMarca.SelectedIndex = -1
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            Finally
                'Agregado 01072010
                If m_drdCatalogo IsNot Nothing Then
                    If Not m_drdCatalogo.IsClosed Then
                        Call m_drdCatalogo.Close()
                    End If
                End If
            End Try

        End Sub

        Private Function ValidarDatos() As Boolean

            Dim blnDatosCorrectos As Boolean
            blnDatosCorrectos = True

            errVehiculos.Clear()

            If txtCardCode.Text = "" Then

                errVehiculos.SetError(txtCardCode, My.Resources.ResourceUI.MensajeDebeSeleccionarCliente)
                errVehiculos.SetIconAlignment(txtCardCode, ErrorIconAlignment.MiddleLeft)
                blnDatosCorrectos = False

            End If

            If cboMarca.SelectedIndex = -1 Then

                errVehiculos.SetError(cboMarca, My.Resources.ResourceUI.MensajeDebeSelecMarca)
                errVehiculos.SetIconAlignment(cboMarca, ErrorIconAlignment.MiddleLeft)
                blnDatosCorrectos = False

            End If

            If cboEstilo.SelectedIndex = -1 Then

                errVehiculos.SetError(cboEstilo, My.Resources.ResourceUI.MensajeDebeSeleccionarEstilo)
                errVehiculos.SetIconAlignment(cboEstilo, ErrorIconAlignment.MiddleLeft)
                blnDatosCorrectos = False

            End If

            If blnDatosCorrectos Then

                If ValidarSiPlacaExiste() Then
                    blnDatosCorrectos = False
                Else
                    If ValidarSiNoVehiculoExiste() Then

                        blnDatosCorrectos = False

                    End If

                End If

            End If
            Return blnDatosCorrectos
        End Function

        Private Sub LimpiarControles()

            txtAño.Text = ""
            txtCardCode.Text = ""
            txtCardName.Text = ""
            txtCilindrada.Text = ""
            txtGarantiaAños.Text = ""
            txtGarantiaKM.Text = ""
            txtNoCilindros.Text = ""
            txtNoEjes.Text = ""
            txtNoMotor.Text = ""
            txtNoPasajeros.Text = ""
            txtNoPuertas.Text = ""
            txtNoUnidad.Text = ""
            txtObservaciones.Text = ""
            txtPeso.Text = ""
            txtPlaca.Text = ""
            txtPotenciaKW.Text = ""
            txtVIN.Text = ""
            cboCabina.SelectedIndex = -1
            cboCarroceria.SelectedIndex = -1
            cboCategoria.SelectedIndex = -1
            cboColor.SelectedIndex = -1
            cboColorTapiceria.SelectedIndex = -1
            cboCombustible.SelectedIndex = -1
            cboEstado.SelectedIndex = -1
            cboMarca.SelectedIndex = -1
            cboEstilo.SelectedIndex = -1
            cboMarcaMotor.SelectedIndex = -1
            cboModelo.SelectedIndex = -1
            cboTecho.SelectedIndex = -1
            cboTipo.SelectedIndex = -1
            cboTraccion.SelectedIndex = -1
            cboTransmision.SelectedIndex = -1
            cboUbicacion.SelectedIndex = -1
            errVehiculos.Clear()

        End Sub

        Private Function ValidarSiPlacaExiste() As Boolean

            Dim dtsVehiculoPorPlaca As New VehiculosDataset
            Dim blnPlacaExiste As Boolean = False
            Dim blnHacerPregunta As Boolean = False
            If txtPlaca.Text <> "" Then

                m_adpVehiculos.Fill(dtsVehiculoPorPlaca, , txtPlaca.Text)
                If dtsVehiculoPorPlaca.SCGTA_VW_Vehiculos.Rows.Count > 0 Then
                    If m_drwVehiculo Is Nothing Then
                        blnHacerPregunta = True
                    ElseIf dtsVehiculoPorPlaca.SCGTA_VW_Vehiculos.Rows(0).Item("IDVehiculo") <> m_drwVehiculo.IDVehiculo Then
                        blnHacerPregunta = True
                    End If
                    If blnHacerPregunta Then
                        If m_objMensajes.msgPregunta("La placa ingresada ya existe. ¿Desea cargar los datos del vehículo que tiene esta placa?") = MsgBoxResult.Yes Then
                            m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Clear()
                            m_adpVehiculos.Fill(m_dtsVehiculos, , txtPlaca.Text)

                            If m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Count > 0 Then
                                Call LimpiarControles()
                                m_drwVehiculo = Nothing
                                m_drwVehiculo = m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows(0)
                                m_intTipoInsercion = enumModoInsercion.scgModificar

                                Call MostrarDatosPantalla()
                                Call HabilitarControles()
                            End If
                        End If
                        blnPlacaExiste = True
                    End If
                End If
            End If
            Return blnPlacaExiste
        End Function

        Private Function ValidarSiNoVehiculoExiste() As Boolean

            Dim dtsVehiculoPorPlaca As New VehiculosDataset
            Dim blnPlacaExiste As Boolean = False
            Dim blnHacerPregunta As Boolean = False

            If txtNoUnidad.Text <> "" Then

                m_adpVehiculos.Fill(dtsVehiculoPorPlaca, , , txtNoUnidad.Text)
                If dtsVehiculoPorPlaca.SCGTA_VW_Vehiculos.Rows.Count > 0 Then
                    If m_drwVehiculo Is Nothing Then
                        blnHacerPregunta = True
                    ElseIf dtsVehiculoPorPlaca.SCGTA_VW_Vehiculos.Rows(0).Item("NoVehiculo") <> m_drwVehiculo.NoVehiculo Then
                        blnHacerPregunta = True
                    End If
                    If blnHacerPregunta Then
                        If m_objMensajes.msgPregunta("El Número de unidad ingresado ya existe. ¿Desea cargar los datos del vehículo que tiene ese número?") = MsgBoxResult.Yes Then
                            m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Clear()
                            m_adpVehiculos.Fill(m_dtsVehiculos, , , txtNoUnidad.Text)

                            If m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows.Count > 0 Then
                                Call LimpiarControles()
                                m_drwVehiculo = Nothing
                                m_drwVehiculo = m_dtsVehiculos.SCGTA_VW_Vehiculos.Rows(0)
                                m_intTipoInsercion = enumModoInsercion.scgModificar

                                Call MostrarDatosPantalla()
                                Call HabilitarControles()
                            End If

                        End If
                        blnPlacaExiste = True
                    End If
                End If
            End If
            Return blnPlacaExiste
        End Function

        Private Sub MostrarReporteFichaVehiculo()

            Try
                If Not m_drwVehiculo Is Nothing AndAlso m_drwVehiculo(0) <> "" Then


                    Dim strParametros As String
                    Dim strNombreSociedad As String


                    'INICIO Buscar el nombre de la compañía sin los parentesis
                    '***************************************************
                    Dim intindiceParentesis As Integer
                    intindiceParentesis = (COMPANIA.LastIndexOf("("))
                    If Not intindiceParentesis > 0 Then
                        intindiceParentesis = COMPANIA.Length - 1
                    End If
                    strNombreSociedad = COMPANIA.Substring(0, intindiceParentesis)
                    '**********************************************************
                    ' FIN de Buscar el nombre de la compañía sin los parentesis

                    strParametros = strNombreSociedad & ","
                    strParametros = strParametros & m_drwVehiculo(0).ToString

                    With rptVehiculo
                        .P_ParArray = strParametros
                        .P_BarraTitulo = My.Resources.ResourceUI.rptbarratituloFichaVehiculo
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreFichaVehiculo
                        .P_Server = Server
                        .P_DataBase = strDATABASE
                        .P_CompanyName = COMPANIA
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptVehiculo.VerReporte()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Public Overridable Sub CargaReporteHistorialResumido()

            If Not m_drwVehiculo Is Nothing AndAlso m_drwVehiculo(0) <> "" Then

                Dim rptTiempo As New ComponenteCristalReport.SubReportView

                Dim strParametros As String = ""

                Dim objBLConexion As New DMSOneFramework.SCGDataAccess.DAConexion

                Try
                    PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)

                    strParametros = m_drwVehiculo(0).ToString

                    With rptTiempo
                        .P_BarraTitulo = My.Resources.ResourceUI.rptBarraTituloHistorialResumido
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreHistorialResumido
                        .P_Server = Server
                        .P_DataBase = strDATABASESCG
                        .P_CompanyName = COMPANIA
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptTiempo.VerReporte()

                Catch ex As Exception
                    objSCGMSGBox.msgInformationCustom(ex.Message)
                End Try
            End If

        End Sub

        Public Sub VisualizarUDF()

            Try

                VisualizarUDFVehiculo.Tabla = "@SCGD_VEHICULO"

                VisualizarUDFVehiculo.NombreBaseDatosSBO = G_objCompany.CompanyDB

                VisualizarUDFVehiculo.VisualizarUDFSBO = True

                VisualizarUDFVehiculo.Form = Me

                VisualizarUDFVehiculo.Conexion = SCGDataAccess.DAConexion.ConnectionString

                'VisualizarUDFVehiculo.CampoLlave = "Code = " & CInt(m_drwVehiculo.IDVehiculo)

                VisualizarUDFVehiculo.Where = "U_SCGD_Cod_Unid = '0'"

                VisualizarUDFVehiculo.CodigoFormularioSBO = 0

                VisualizarUDFVehiculo.CodigoUsuario = 1

                'VisualizarUDFVehiculo.CargarDatosUDF_SBO("U_CardCode = " & CInt(m_drwVehiculo.IDVehiculo))

                VisualizarUDFVehiculo.CargarComboCategorias_SBO()

                'VisualizarUDFVehiculo.CargarDatosUDF_SBO("U_CardCode = 'CL0007'")

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex

            End Try

        End Sub



#End Region
    End Class

End Namespace