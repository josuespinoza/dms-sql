Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess.DAConexion

Imports DMSOneFramework.BLSBO


Namespace SCG_User_Interface
    Public Class frmConfiguracionApp

#Region "Constructor"

        Sub New(ByVal cargafoma As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

        End Sub

#End Region

#Region "Declaraciones"

        'Constantes de valores en la tabla de configuración de parametros

        Private Const mc_strEncargadoAccesorios As String = "EncargadoAccesorios"       
        Private Const mc_strBodegaProceso As String = "BodegaProceso"
        Private Const mc_strBodegaRepuestos As String = "BodegaRepuestos"
        Private Const mc_strBodegaServiciosExternos As String = "BodegaServiciosExternos"
        Private Const mc_strBodegaSuministros As String = "BodegaSuministros"
        Private Const mc_strEncargadoBodega As String = "EncargadoBodega"
        Private Const mc_strEncargadoSuministros As String = "EncargadoSuministros"
        Private Const mc_strEncargadoProduccion As String = "EncargadoProduccion"
        Private Const mc_strEncargadoRepuestos As String = "EncargadoRepuestos"
        Private Const mc_strEncargadoCompras As String = "EncargadoCompras"
        Private Const mc_strIDSerieDocumentosCompra As String = "IDSerieDocumentosCompra"
        Private Const mc_strIDSerieDocumentosTraslado As String = "IDSerieDocumentosTraslado"
        Private Const mc_strIDSerieDocumentosVentas As String = "IDSerieDocumentosVentas"
        Private Const mc_strIDSerieDocumentosCotizaciones As String = "IDSerieDocumentosCotizaciones"
        Private Const mc_strIDSerieOfertaCompra As String = "IDSerieOfertaCompra"
        Private Const mc_strTiempoMensMensajeria As String = "TiempoMensajeria"
        Private Const mc_strListaPrecios As String = "ListaPrecios"

        Private Const mc_strUsaRepuestos As String = "UsaRepuestos"
        Private Const mc_strUsaServicios As String = "UsaServicios"
        Private Const mc_strUsaServiciosExternos As String = "UsaServiciosExternos"
        Private Const mc_strUsaSuministros As String = "UsaSuministros"

        Private objUtilitarios As New DMSOneFramework.SCGDataAccess.Utilitarios(strConectionString)

        '********************************************************************************************
        'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar
        'Autor: José Soto
        Private Const mc_strUsaValTiempoEs As String = "UsaVTiempoEstandar"

        Private Const mc_strUsaValTiempoReal As String = "UsaTiempoReal"

        Private Const mc_strUsaFiltCliente As String = "UsaFiltCliente"


        '********************************************************************************************

        Private Const mc_strUsaDraft As String = "CreaDraftTransferenciasStock"
        Private Const mc_strUtilizaAsignacionAutomaticaEncargadoOper As String = "RealizarAsignacionAutomaticaColaborador"
        'citas a clientes inactivos 
        Private Const mc_strCitasAClientesInactivos As String = "CitasAClientesInactivos"
        'Mensajeria por centro de costo
        Private Const mc_strUsaMensajeriaXCentroCosto As String = "UsaMensajeriaXCentroCosto"

        'Finaliza OT Cantidad Solicitada
        Private Const mc_strFinalizaOTCantSolicitada As String = "FinalizaOTCantSolicitada"


        Private Const mc_strImpRepuestos As String = "ImpuestoRepuestos"
        Private Const mc_strImpServicios As String = "ImpuestoServicios"
        Private Const mc_strImpServiciosExternos As String = "ImpuestoServiciosExternos"
        Private Const mc_strImpSuministros As String = "ImpuestoSuministros"

        Private Const mc_UnidadTiempo As String = "UnidadTiempo"


        Private Const mc_strCatalogosExternos As String = "CatalogosExternos"
        Private Const mc_strSEInventariables As String = "SEInventariables"
        Private Const mc_strDireccionB2B As String = "DireccionB2B"

        Private Const mc_strGeneraOTsEspeciales As String = "GeneraOTsEspeciales"

        Private Const mc_strCopiasRepRecepcion As String = "CopiasRepRecepcion"



        Private Const mc_strCosteoServicios As String = "CosteoServicios"

        Private Const mc_strTipoCosto As String = "TipoCosto"
        Private Const mc_strTipoCompra As String = "TipoCompra"
        Private Const mc_strTipoMoneda As String = "TipoMoneda"

        Private Const mc_strUsaListaCliente As String = "UsaListaPreciosCliente"

        Private Const mc_strPermiteCambioPrecio As String = "PermiteCambioPrecio"
        Private Const mc_strValidaEstadoOTPadre As String = "ValidaEstadoOTPadre"

        Private Const mc_strAsignacionUnicaMO As String = "AsignacionUnicaMO"

        Private Const mc_strUsaSolicitudOTEspecial As String = "UsaSolicitudOTEspecial"

        'Constantes para checkbox
        Private Const mc_strCheckeado As String = "1"
        Private Const mc_strdesCheckeado As String = "0"

        'Variables
        Private m_drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow
        Private m_BuConfiguracion As Buscador.SubBuscador

        Private m_dstConfRepuestosXMarca As ConfCatalogoRepXMarcaDataset
        Private m_adpConfRepuestosXMArca As ConfCatalogoRepXMarcaDataAdapter

        Private WithEvents m_objfrmConfRepuestosXMarca As frmConfCatalogoRepxMarca

        'Variable para buscador de cuentas contables
        Private WithEvents m_objBuscador As New Buscador.SubBuscador

#End Region

#Region "Eventos"

        Private Sub frmConfiguracionApp_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                m_BuConfiguracion = New Buscador.SubBuscador

                AddHandler m_BuConfiguracion.AppAceptar, _
                AddressOf m_BuSeries_AppAceptar

                ''Para ocultar el TAB d Bodegas
                tabConfiguracion.TabPages.Remove(tpBodega)

                Call CargaFormulario(g_dstConfiguracion.SCGTA_TB_Configuracion, m_drwConfiguracion)

                lblName.Text = G_strNombreSucursal

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            Finally
            End Try

        End Sub

        Private Sub m_BuSeries_AppAceptar(ByVal Campo_Llave As String, _
                                          ByVal Arreglo_Campos As System.Collections.ArrayList, _
                                          ByVal sender As Object)

            If Not m_BuConfiguracion.OUT_DataTable Is Nothing _
                AndAlso m_BuConfiguracion.OUT_DataTable.Rows.Count > 0 Then

                Select Case sender.name
                    Case picArticuloCotizacion.Name
                        txtArtCotizacion.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("ItemCode")

                    Case picOrdenesdeCompra.Name

                        ntxtOrdendeCompra.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("SeriesName")
                        ntxtOrdendeCompra.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Series")

                    Case picOfertasdeCompra.Name

                        ntxtOfertadeCompra.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("SeriesName")
                        ntxtOfertadeCompra.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Series")

                    Case picOrdVentas.Name

                        ntxtOrdenVentas.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("SeriesName")
                        ntxtOrdenVentas.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Series")

                    Case picCotizaciones.Name

                        txtCotizaciones.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("SeriesName")
                        txtCotizaciones.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Series")

                    Case picTraslados.Name

                        ntxtTraslados.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("SeriesName")
                        ntxtTraslados.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Series")

                    Case picRepuestos.Name

                        ntxtRepuestos.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsName")
                        ntxtRepuestos.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsCode")

                    Case picSuministros.Name

                        ntxtSuministros.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsName")
                        ntxtSuministros.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsCode")

                    Case PicSE.Name

                        ntxtSE.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsName")
                        ntxtSE.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsCode")

                    Case picProceso.Name

                        ntxtProcesos.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsName")
                        ntxtProcesos.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("WhsCode")

                    Case picEncargadoBodega.Name

                        If Trim(ntxtEncargadoBodega.Text) = String.Empty Then
                            ntxtEncargadoBodega.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            ntxtEncargadoBodega.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            ntxtEncargadoBodega.Text = ntxtEncargadoBodega.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            ntxtEncargadoBodega.Tag = ntxtEncargadoBodega.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If


                    Case picEncargadoSuministros.Name

                        If Trim(txtEncargadoSuministros.Text) = String.Empty Then
                            txtEncargadoSuministros.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            txtEncargadoSuministros.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoSuministros.Text = txtEncargadoSuministros.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            txtEncargadoSuministros.Tag = txtEncargadoSuministros.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If

                        'Agregado 01/11/2010: Conecta el textBox de encargado de accesorios 
                    Case picEncargadoAccesorios.Name

                        If Trim(txtEncargadoAccesorios.Text) = String.Empty Then
                            txtEncargadoAccesorios.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            txtEncargadoAccesorios.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoAccesorios.Text = txtEncargadoAccesorios.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            txtEncargadoAccesorios.Tag = txtEncargadoAccesorios.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If

                    Case picencargadoproduccion.Name

                        If Trim(ntxtEncargadoProduccion.Text) = String.Empty Then
                            ntxtEncargadoProduccion.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("Nombre_Completo")
                            ntxtEncargadoProduccion.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Usuario")
                        Else
                            ntxtEncargadoProduccion.Text = ntxtEncargadoProduccion.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("Nombre_Completo")
                            ntxtEncargadoProduccion.Tag = ntxtEncargadoProduccion.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("Usuario")
                        End If


                    Case picEncargadoRepuestos.Name
                        If Trim(txtEncargadoRepuestos.Text) = String.Empty Then
                            txtEncargadoRepuestos.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("Nombre_Completo")
                            txtEncargadoRepuestos.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Usuario")
                        Else
                            txtEncargadoRepuestos.Text = txtEncargadoRepuestos.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("Nombre_Completo")
                            txtEncargadoRepuestos.Tag = txtEncargadoRepuestos.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("Usuario")
                        End If


                    Case picImpRefacciones.Name

                        txtImpRefacciones.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("Name")
                        txtImpRefacciones.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Code")

                    Case picImpSuministros.Name

                        txtImpSuministros.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("Name")
                        txtImpSuministros.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Code")

                    Case picImpServicios.Name

                        txtImpServicios.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("Name")
                        txtImpServicios.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Code")

                    Case picImpServiciosExternos.Name

                        txtImpServiciosExternos.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("Name")
                        txtImpServiciosExternos.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("Code")

                    Case picListaPrecios.Name

                        txtListaPrecios.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("ListName")
                        txtListaPrecios.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("ListNum")

                    Case picUnidadesTiempo.Name
                        txtUnidadTiempo.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("CodigoUnidadTiempo")
                        txtUnidadTiempo.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("DescripcionUnidadTiempo")

                    Case picEncargadoOrdenCompra.Name
                        If Trim(txtEncargadoOrdenCompra.Text) = String.Empty Then
                            txtEncargadoOrdenCompra.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            txtEncargadoOrdenCompra.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        Else
                            txtEncargadoOrdenCompra.Text = txtEncargadoOrdenCompra.Text & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("U_Name")
                            txtEncargadoOrdenCompra.Tag = txtEncargadoOrdenCompra.Tag & ", " & m_BuConfiguracion.OUT_DataTable.Rows(0)("User_Code")
                        End If

                    Case picTipoMoneda.Name

                        txtTipoMoneda.Text = m_BuConfiguracion.OUT_DataTable.Rows(0)("CurrName")
                        txtTipoMoneda.Tag = m_BuConfiguracion.OUT_DataTable.Rows(0)("CurrCode")

                End Select

            End If

        End Sub

        Private Sub CargarBuscadordeSeries(ByVal buSeries As Buscador.SubBuscador, _
                                           ByVal Sender As Object)

            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buSeries

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorSeries
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.serie
                    .Criterios = "Series,SeriesName"
                    '.Criterios_Ocultos = 0
                    .Where = ""
                    '.Criterios_OcultosEx = "1,3"
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_Series"
                    .Activar_Buscador(Sender)

                End With

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub CargarBuscadordeBodegas(ByVal buBodegas As Buscador.SubBuscador, _
                                            ByVal Sender As Object)

            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buBodegas

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorAlmacenes
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Almacen
                    .Criterios = "WhsCode,WhsName"
                    .Criterios_OcultosEx = ""
                    .Where = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_Bodegas"
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub CargarBuscadordeUsuariosSBO(ByVal buUsuariosSBO As Buscador.SubBuscador, _
                                             ByVal Sender As Object)

            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buUsuariosSBO

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarratituloBuscadorusuariosSBO
                    .Titulos = My.Resources.ResourceUI.Usuario & "," & My.Resources.ResourceUI.Nombre & "," & My.Resources.ResourceUI.Sucursal
                    .Criterios = "User_Code,U_Name, Sucursal"
                    .Criterios_OcultosEx = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_OUSR"
                    'If G_strIDSucursal <> "" Then
                    '    .Where = "branch=" & G_strIDSucursal
                    'Else
                    .Where = ""
                    'End If
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub CargarBuscadordeUsuariosDMSOne(ByVal buUsuariosDMSOne As Buscador.SubBuscador, _
                                                    ByVal Sender As Object)

            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buUsuariosDMSOne

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorUsuariosDMS
                    .Titulos = My.Resources.ResourceUI.usuario & "," & My.Resources.ResourceUI.Nombre
                    .Criterios = "Usuario,Nombre_Completo"
                    .Criterios_OcultosEx = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_Usuarios"
                    .Where = "IdParametros=" & G_strIDConfig
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub CargarBuscadordeImpuestos(ByVal buBodegas As Buscador.SubBuscador, _
                                            ByVal Sender As Object)

            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buBodegas

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorImpuestos

                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Almacen & "," & My.Resources.ResourceUI.Tarifa

                    .Criterios = "Code,Name,Rate"
                    .Criterios_OcultosEx = ""
                    .Where = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_OSTC"
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub picSeries_Click(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) Handles picOrdenesdeCompra.Click

            Call CargarBuscadordeSeries(m_BuConfiguracion, sender)

        End Sub

        Private Sub picOrdVentas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOrdVentas.Click

            Call CargarBuscadordeSeries(m_BuConfiguracion, sender)

        End Sub


        Private Sub picTraslados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picTraslados.Click

            Call CargarBuscadordeSeries(m_BuConfiguracion, sender)

        End Sub

        Private Sub picProceso_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picProceso.Click
            Call CargarBuscadordeBodegas(m_BuConfiguracion, sender)
        End Sub

        Private Sub picRepuestos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picRepuestos.Click
            Call CargarBuscadordeBodegas(m_BuConfiguracion, sender)
        End Sub

        Private Sub picSuministros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picSuministros.Click
            Call CargarBuscadordeBodegas(m_BuConfiguracion, sender)
        End Sub

        Private Sub PicSE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicSE.Click
            Call CargarBuscadordeBodegas(m_BuConfiguracion, sender)
        End Sub

        Private Sub picencargadoproduccion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picencargadoproduccion.Click, picEncargadoRepuestos.Click
            CargarBuscadordeUsuariosDMSOne(m_BuConfiguracion, sender)
        End Sub

        Private Sub picEncargadoBodega_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoBodega.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub

        Private Sub picEncargadoSuministros_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picEncargadoSuministros.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub

        Private Sub btnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
            Call Me.Close()
        End Sub

        Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

            If GuardaValoresDeConfiguracionEnDataSet(g_dstConfiguracion.SCGTA_TB_Configuracion, m_drwConfiguracion) Then
                Call g_adpConfiguracion.Update(g_dstConfiguracion)
                g_blnUsaRepuestos = chkUsaRepuestos.Checked
                g_blnUsaSuministros = chkUsaSuministros.Checked
                g_blnUsaServicios = chkUsaServicios.Checked
                g_blnUsaServiciosExternos = chkUsaServiciosExternos.Checked
                g_strCuentaContableAcre = txtNumeroCuenta.Text.Trim()
                g_blnModificaPrecio = chkCambiaPrecio.Checked
                g_blnValidaEstadoOTPadre = chkCrearOThijas.Checked
                '********************************************************************************************
                'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar
                'Autor: José Soto

                g_blnUsaValTiempoEs = chkUsaValTiempoEs.Checked
                g_blnUsaValFiltClient = chkUsaFiltroClientes.Checked

                '********************************************************************************************

                g_strImpRepuestos = txtImpRefacciones.Tag
                g_strImpServicios = txtImpServicios.Tag
                g_strImpSuministros = txtImpSuministros.Tag
                g_strImpServiciosExternos = txtImpServiciosExternos.Tag
                g_strEncagadoCompras = txtEncargadoOrdenCompra.Tag
                g_blnCatalogosExternos = chkCatalogosExternos.Checked
                g_blnGeneraOTsEspeciales = chkGeneraOTsEspeciales.Checked
                g_blnServiciosExternosInventariables = chkSEInventariables.Checked
                g_blnCosteaActividades = chkCosteoServicios.Checked
                g_blnUsaOtrosGastos = chkOtrosGastos.Checked
                g_strEncargadoAcc = txtEncargadoAccesorios.Tag
                'Mensajeria por centro de costo
                g_blnUsaMensajeriaXCentroCosto = chkUsaMensajeriaXCentroCosto.Checked

                'WebConfig
                g_blnVerOTCodigos = chkOTRepuestos.Checked
                g_blnVerOTTotales = chkOTTotales.Checked



                If Trim(txtUnidadTiempo.Tag) = "" Or String.IsNullOrEmpty(CStr(txtUnidadTiempo.Tag)) Or Trim(txtUnidadTiempo.Text) = "" Or String.IsNullOrEmpty(CStr(txtUnidadTiempo.Text)) Then
                    g_intUnidadTiempo = -1
                    txtUnidadTiempo.Tag = ""
                    txtUnidadTiempo.Text = ""
                Else
                    g_intUnidadTiempo = txtUnidadTiempo.Tag
                End If

            End If

            'Validación cuenta acreedora

            If chkCosteoServicios.Checked = True Then

                If ValidarCuentaAcreedora() = True Then

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.ValidarCuentaAcree)
                    Exit Sub
                End If

            End If

            'Validación Oferta de compra

            If rb_OfertaCompra.Checked = True Then
                If ValidarOfertaCompra() = True Then

                    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.ValidarOfertaCompra)

                    Exit Sub
                End If
            End If




            Call Me.Close()
        End Sub

        Private Function ValidarCuentaAcreedora() As Boolean

            If txtNumeroCuenta.Text.Trim() = String.Empty Then

                Return True

            Else
                Return False

            End If

        End Function

        Private Function ValidarOfertaCompra() As Boolean

            Dim objOfertaDeCompra As SAPbobsCOM.Documents

            Try
                objOfertaDeCompra = DirectCast(oCompany.GetBusinessObject(540000006),  _
                                                                SAPbobsCOM.Documents)
            Catch ex As Exception
                Return True
            End Try

            Return False

        End Function

        Private Sub picImpServicios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picImpServicios.Click

            CargarBuscadordeImpuestos(m_BuConfiguracion, sender)

        End Sub

        Private Sub picImpRefacciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picImpRefacciones.Click

            CargarBuscadordeImpuestos(m_BuConfiguracion, sender)

        End Sub

        Private Sub picImpSuministros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picImpSuministros.Click

            CargarBuscadordeImpuestos(m_BuConfiguracion, sender)

        End Sub

        Private Sub picImpServiciosExternos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picImpServiciosExternos.Click

            CargarBuscadordeImpuestos(m_BuConfiguracion, sender)

        End Sub

        Private Sub picListaPrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picListaPrecios.Click
            Try

                CargarBusquedaListaPrecios(m_BuConfiguracion, sender)

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try
        End Sub

        Private Sub btnAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregar.Click

            If m_objfrmConfRepuestosXMarca IsNot Nothing Then
                m_objfrmConfRepuestosXMarca.Dispose()
                m_objfrmConfRepuestosXMarca = Nothing
            End If

            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean

            Try

                For Each Forma_Nueva In Me.MdiParent.MdiChildren
                    If Forma_Nueva.Name = "frmConfCatalogoRepxMarca" Then
                        blnExisteForm = True
                    End If
                Next

                If Not blnExisteForm Then

                    m_objfrmConfRepuestosXMarca = New frmConfCatalogoRepxMarca(m_dstConfRepuestosXMarca)
                    m_objfrmConfRepuestosXMarca.MdiParent = Me.MdiParent
                    m_objfrmConfRepuestosXMarca.Show()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminar.Click

            Dim intIDConfiguracion As Integer
            Dim drwConfRepuestosXMarca As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow

            Try

                If dtgMarcasConfiguradas.CurrentRow IsNot Nothing Then

                    intIDConfiguracion = CInt(dtgMarcasConfiguradas.CurrentRow.Cells(1).Value)
                    drwConfRepuestosXMarca = m_dstConfRepuestosXMarca.SCGTA_TB_ConfCatalogoRepxMarca.FindByID(intIDConfiguracion)
                    drwConfRepuestosXMarca.Delete()
                    m_adpConfRepuestosXMArca = New ConfCatalogoRepXMarcaDataAdapter
                    m_adpConfRepuestosXMArca.Update(m_dstConfRepuestosXMarca)

                    Call CargarDatosConfRepuestosXMarca()

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub dtgMarcasConfiguradas_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgMarcasConfiguradas.DoubleClick

            Dim intIDConfiguracion As Integer
            Dim drwConfRepuestosXMarca As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow
            Dim Forma_Nueva As Form
            Dim blnExisteForm As Boolean

            Try

                If m_objfrmConfRepuestosXMarca IsNot Nothing Then
                    m_objfrmConfRepuestosXMarca.Dispose()
                    m_objfrmConfRepuestosXMarca = Nothing
                End If

                If dtgMarcasConfiguradas.CurrentRow IsNot Nothing Then

                    intIDConfiguracion = CInt(dtgMarcasConfiguradas.CurrentRow.Cells(1).Value)
                    drwConfRepuestosXMarca = m_dstConfRepuestosXMarca.SCGTA_TB_ConfCatalogoRepxMarca.FindByID(intIDConfiguracion)

                    For Each Forma_Nueva In Me.MdiParent.MdiChildren
                        If Forma_Nueva.Name = "frmConfCatalogoRepxMarca" Then
                            blnExisteForm = True
                        End If
                    Next

                    If Not blnExisteForm Then

                        m_objfrmConfRepuestosXMarca = New frmConfCatalogoRepxMarca(drwConfRepuestosXMarca, m_dstConfRepuestosXMarca)
                        m_objfrmConfRepuestosXMarca.MdiParent = Me.MdiParent
                        m_objfrmConfRepuestosXMarca.Show()

                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Private Sub m_objfrmConfRepuestosXMarca_FinalizoProcesamiento() Handles m_objfrmConfRepuestosXMarca.FinalizoProcesamiento
            Try
                Call CargarDatosConfRepuestosXMarca()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub chkCatalogosExternos_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCatalogosExternos.CheckedChanged

            dtgMarcasConfiguradas.Enabled = chkCatalogosExternos.Checked
            btnAgregar.Enabled = chkCatalogosExternos.Checked
            btnEliminar.Enabled = chkCatalogosExternos.Checked
            picDireccionB2B.Enabled = chkCatalogosExternos.Checked

            If Not chkCatalogosExternos.Checked Then

                m_dstConfRepuestosXMarca = Nothing
                dtgMarcasConfiguradas.DataSource = Nothing
                txtDireccionB2b.Text = ""
            Else

                Call CargarDatosConfRepuestosXMarca()
                txtDireccionB2b.Text = txtDireccionB2b.Tag

            End If

        End Sub

        Private Sub picDireccionB2B_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picDireccionB2B.Click

            fbdDireccionB2B.Description = My.Resources.ResourceUI.DireccionXML
            fbdDireccionB2B.ShowDialog()
            txtDireccionB2b.Tag = fbdDireccionB2B.SelectedPath()
            txtDireccionB2b.Text = fbdDireccionB2B.SelectedPath()

        End Sub

#End Region

#Region "Metodos Generales"

        Private Function CargaFormulario(ByVal dtbConfiguracionApp As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                             ByVal drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow) As Boolean

            Try

                Dim strValor As String = ""

                'Carga de controles Checks

                '********************************************************************************************
                'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar
                'Autor: José Soto
                Call ManipulaControlesChecks(chkUsaValTiempoEs, dtbConfiguracionApp, mc_strUsaValTiempoEs)
                '********************************************************************************************

                Call ManipulaControlesChecks(chkUsaFiltroClientes, dtbConfiguracionApp, mc_strUsaFiltCliente)
                Call ManipulaControlesChecks(chkUsaRepuestos, dtbConfiguracionApp, mc_strUsaRepuestos)
                Call ManipulaControlesChecks(chkUsaServicios, dtbConfiguracionApp, mc_strUsaServicios)
                Call ManipulaControlesChecks(chkUsaServiciosExternos, dtbConfiguracionApp, mc_strUsaServiciosExternos)
                Call ManipulaControlesChecks(chkUsaSuministros, dtbConfiguracionApp, mc_strUsaSuministros)
                Call ManipulaControlesChecks(chkOtrosGastos, dtbConfiguracionApp, "UsaOtrosGastos")
                Call ManipulaControlesChecks(chkUsaDraftTransferencia, dtbConfiguracionApp, mc_strUsaDraft)
                Call ManipulaControlesChecks(chkUsaAsignacionAutomaticaEncargadoOper, dtbConfiguracionApp, mc_strUtilizaAsignacionAutomaticaEncargadoOper)
                'citas a clientes inactivos 
                Call ManipulaControlesChecks(chkCitasCliInv, dtbConfiguracionApp, mc_strCitasAClientesInactivos)

                'Mensajeria por centro de costo
                Call ManipulaControlesChecks(chkUsaMensajeriaXCentroCosto, dtbConfiguracionApp, mc_strUsaMensajeriaXCentroCosto)

                'Finaliza OT Cantidad Solicitada
                Call ManipulaControlesChecks(chkFinalizaOTCantSolicitada, dtbConfiguracionApp, mc_strFinalizaOTCantSolicitada)


                'Carga de controles de textbox

                'Citas
                Call ManipulaControlesText(txtArtCotizacion, dtbConfiguracionApp, "ArticuloCita", False)

                'Cuenta Contable Acreedora
                Call ManipulaControlesText(txtNumeroCuenta, dtbConfiguracionApp, "CuentaContableAcre", False)


                'Series
                Call ManipulaControlesText(ntxtOfertadeCompra, dtbConfiguracionApp, mc_strIDSerieOfertaCompra, True)
                Call ManipulaControlesText(ntxtOrdendeCompra, dtbConfiguracionApp, mc_strIDSerieDocumentosCompra, True)
                Call ManipulaControlesText(ntxtOrdenVentas, dtbConfiguracionApp, mc_strIDSerieDocumentosVentas, True)
                Call ManipulaControlesText(ntxtTraslados, dtbConfiguracionApp, mc_strIDSerieDocumentosTraslado, True)
                Call ManipulaControlesText(txtCotizaciones, dtbConfiguracionApp, mc_strIDSerieDocumentosCotizaciones, True)

                'Bodegas
                'Call ManipulaControlesText(ntxtRepuestos, dtbConfiguracionApp, mc_strBodegaRepuestos, True)
                'Call ManipulaControlesText(ntxtSuministros, dtbConfiguracionApp, mc_strBodegaSuministros, True)
                'Call ManipulaControlesText(ntxtSE, dtbConfiguracionApp, mc_strBodegaServiciosExternos, True)
                'Call ManipulaControlesText(ntxtProcesos, dtbConfiguracionApp, mc_strBodegaProceso, True)

                'Mensajeria
                Call ManipulaControlesText(ntxtEncargadoBodega, dtbConfiguracionApp, mc_strEncargadoBodega, True)
                Call ManipulaControlesText(txtEncargadoSuministros, dtbConfiguracionApp, mc_strEncargadoSuministros, True)
                Call ManipulaControlesText(ntxtEncargadoProduccion, dtbConfiguracionApp, mc_strEncargadoProduccion, True)
                Call ManipulaControlesText(txtEncargadoRepuestos, dtbConfiguracionApp, mc_strEncargadoRepuestos, True)
                Call ManipulaControlesText(ntxtIntervaloMen, dtbConfiguracionApp, mc_strTiempoMensMensajeria, False)
                Call ManipulaControlesText(txtEncargadoOrdenCompra, dtbConfiguracionApp, mc_strEncargadoCompras, True)
                Call ManipulaControlesText(txtEncargadoAccesorios, dtbConfiguracionApp, mc_strEncargadoAccesorios, True)


                'Impuestos
                Call ManipulaControlesText(txtImpRefacciones, dtbConfiguracionApp, mc_strImpRepuestos, True)
                Call ManipulaControlesText(txtImpSuministros, dtbConfiguracionApp, mc_strImpSuministros, True)
                Call ManipulaControlesText(txtImpServicios, dtbConfiguracionApp, mc_strImpServicios, True)
                Call ManipulaControlesText(txtImpServiciosExternos, dtbConfiguracionApp, mc_strImpServiciosExternos, True)

                Call ManipulaControlesText(txtUnidadTiempo, dtbConfiguracionApp, mc_UnidadTiempo, True)

                'ListaPrecios
                Call ManipulaControlesText(txtListaPrecios, dtbConfiguracionApp, mc_strListaPrecios, True)

                'Configuración Repuestos Por Marca
                Call ManipulaControlesChecks(chkCatalogosExternos, dtbConfiguracionApp, mc_strCatalogosExternos)

                If chkCatalogosExternos.Checked Then

                    Call ManipulaControlesText(txtDireccionB2b, dtbConfiguracionApp, mc_strDireccionB2B, False)
                    Call CargarDatosConfRepuestosXMarca()

                End If

                'Generación de OT's especiales
                Call ManipulaControlesChecks(chkGeneraOTsEspeciales, dtbConfiguracionApp, mc_strGeneraOTsEspeciales)

                'Servicios Externos inventariables
                Call ManipulaControlesChecks(chkSEInventariables, dtbConfiguracionApp, mc_strSEInventariables)

                'Copias Reporte Recepción
                Call ManipulaControlesText(txtCopiasRepRecepcion, dtbConfiguracionApp, mc_strCopiasRepRecepcion, False)

                'Configuracion Para Web***'
                Call ManipulaControlesChecks(chkOTRepuestos, dtbConfiguracionApp, "MuestraCodigoWeb")
                Call ManipulaControlesChecks(chkOTTotales, dtbConfiguracionApp, "MuestraTotalesWeb")

                Call ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dtbConfiguracionApp, mc_strCosteoServicios, strValor)

                If Not IsNumeric(strValor) Then
                    strValor = "0"
                End If
                g_intCosteoServicios = CInt(strValor)

                If strValor = "0" Then
                    chkCosteoServicios.Checked = False
                Else
                    chkCosteoServicios.Checked = True
                    If strValor = "1" Then
                        rbtEstandar.Checked = True
                    Else
                        rbtTiempoReal.Checked = True
                    End If
                End If


                Call ManipulaControlesText(txtTipoMoneda, dtbConfiguracionApp, mc_strTipoMoneda, True)


                'Cargar parametros de costo simple o detallado

                Call ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dtbConfiguracionApp, mc_strTipoCosto, strValor)

                If Not IsNumeric(strValor) Then
                    strValor = "0"
                End If
                ' g_intCosteoServicios = CInt(strValor)

                If strValor = "0" Then
                    'chkCosteoServicios.Checked = False
                    gbxTipoCostos.Enabled = False

                Else
                    'chkCosteoServicios.Checked = True
                    gbxTipoCostos.Enabled = True

                    If strValor = "1" Then
                        rbtSimple.Checked = True
                    Else
                        rbtDetallado.Checked = True
                    End If
                End If



                'Cargar parametros de compra por Orden o Cotización
                Call ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dtbConfiguracionApp, mc_strTipoCompra, strValor)
                If strValor = "2" Then
                    rb_OfertaCompra.Checked = True
                Else
                    rb_OrdenCompra.Checked = True
                End If


                'Usa Lista de Precios Cliente
                Call ManipulaControlesChecks(chckUsaListaCliente, dtbConfiguracionApp, mc_strUsaListaCliente)

                'Permite Modificar Precio
                Call ManipulaControlesChecks(chkCambiaPrecio, dtbConfiguracionApp, mc_strPermiteCambioPrecio)

                'Permite Crear OT's Hijas validando el estado de la OT Padre
                Call ManipulaControlesChecks(chkCrearOThijas, dtbConfiguracionApp, mc_strValidaEstadoOTPadre)


                'Asignación única de mecánico a MO
                Call ManipulaControlesChecks(chkAsignacionUnicaMO, dtbConfiguracionApp, mc_strAsignacionUnicaMO)

                Call ManipulaControlesChecks(chkSolOTEsp, dtbConfiguracionApp, mc_strUsaSolicitudOTEspecial)


                'Cargar cuenta acreedora
                txtNombreCuenta.Text = objUtilitarios.ObtenerNombreCuenta(txtNumeroCuenta.Text)

                Return True

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
                Return False
            Finally
            End Try
        End Function

        Private Sub ManipulaControlesChecks(ByRef chkControl As System.Windows.Forms.CheckBox, _
                                            ByVal dtbConfiguracionApp As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                            ByVal strPropiedad As String)
            Dim strValor As String = ""

            Try

                If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dtbConfiguracionApp, _
                                                                                  strPropiedad, _
                                                                                  strValor) Then
                    chkControl.CheckState = CInt(strValor)

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try
        End Sub

        Private Sub ManipulaControlesText(ByRef ntxtControl As NEWTEXTBOX.NEWTEXTBOX_CTRL, _
                                              ByVal dtbConfiguracionApp As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                              ByVal strPropiedad As String, _
                                              ByVal blnBuscador As Boolean)

            Dim strValor As String = ""
            Dim strEtiqueta As String = ""

            Try
                If blnBuscador Then

                    If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dtbConfiguracionApp, _
                                                                                      strPropiedad, _
                                                                                      strValor) Then
                        ntxtControl.Tag = strValor

                    End If

                    If ConfiguracionDataAdapter.DevuelveEtiquetaDeParametosConfiguracion(dtbConfiguracionApp, _
                                                                                         strPropiedad, _
                                                                                         strEtiqueta) Then
                        ntxtControl.Text = strEtiqueta

                    End If

                Else

                    If ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dtbConfiguracionApp, _
                                                                                      strPropiedad, _
                                                                                      strValor) Then
                        ntxtControl.Text = strValor

                    End If

                End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'MsgBox(ex.Message)
            End Try

        End Sub

        Private Function GuardaValoresDeConfiguracionEnDataSet(ByRef dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                               ByVal drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow) As Boolean
            Try

                'Llamadas al metodo actualiza valores de checks en la forma de configuraciòn
                'Generales
                '********************************************************************************************
                'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar
                'Autor: José Soto
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaValTiempoEs, mc_strUsaValTiempoEs)

                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaFiltroClientes, mc_strUsaFiltCliente)
                '********************************************************************************************

                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaRepuestos, mc_strUsaRepuestos)
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaServicios, mc_strUsaServicios)
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaServiciosExternos, mc_strUsaServiciosExternos)
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaSuministros, mc_strUsaSuministros)
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkOtrosGastos, "UsaOtrosGastos")
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaDraftTransferencia, mc_strUsaDraft)
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaAsignacionAutomaticaEncargadoOper, mc_strUtilizaAsignacionAutomaticaEncargadoOper)
                'citas a clientes inactivos 
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkCitasCliInv, mc_strCitasAClientesInactivos)
                'Mensajeria por centro de costo
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkUsaMensajeriaXCentroCosto, mc_strUsaMensajeriaXCentroCosto)

                'Citas
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtArtCotizacion, "ArticuloCita", False)

                'Cuenta Contable Acreedora
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtNumeroCuenta, "CuentaContableAcre", False)



                'FinalizaOTCantSolicitada
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkFinalizaOTCantSolicitada, mc_strFinalizaOTCantSolicitada)

                'Web***
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkOTRepuestos, "MuestraCodigoWeb")
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkOTTotales, "MuestraTotalesWeb")

                'Llamadas al metodo actualiza valores de los ntextbox en la forma de configuraciòn
                'Series
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtOfertadeCompra, mc_strIDSerieOfertaCompra, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtOrdendeCompra, mc_strIDSerieDocumentosCompra, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtOrdenVentas, mc_strIDSerieDocumentosVentas, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtTraslados, mc_strIDSerieDocumentosTraslado, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtCotizaciones, mc_strIDSerieDocumentosCotizaciones, True)

                'Bodegas
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtProcesos, mc_strBodegaProceso, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtRepuestos, mc_strBodegaRepuestos, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtSE, mc_strBodegaServiciosExternos, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtSuministros, mc_strBodegaSuministros, True)

                'Agregado 01/10/2010: encargado accesorios
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtEncargadoAccesorios, mc_strEncargadoAccesorios, True)

                'Mensajeria 
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtEncargadoBodega, mc_strEncargadoBodega, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtEncargadoSuministros, mc_strEncargadoSuministros, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtEncargadoProduccion, mc_strEncargadoProduccion, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtEncargadoRepuestos, mc_strEncargadoRepuestos, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, ntxtIntervaloMen, mc_strTiempoMensMensajeria, False)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtEncargadoOrdenCompra, mc_strEncargadoCompras, True)


                'Impuestos
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtImpRefacciones, mc_strImpRepuestos, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtImpSuministros, mc_strImpSuministros, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtImpServicios, mc_strImpServicios, True)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtImpServiciosExternos, mc_strImpServiciosExternos, True)


                'ListaPrecios
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtListaPrecios, mc_strListaPrecios, True)


                '************
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtUnidadTiempo, mc_UnidadTiempo, True)
                '************

                'Servicios Externos Inventariables
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkSEInventariables, mc_strSEInventariables)

                'Repuestos Por Marca
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkCatalogosExternos, mc_strCatalogosExternos)
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtDireccionB2b, mc_strDireccionB2B, False)
                If m_dstConfRepuestosXMarca IsNot Nothing Then
                    m_adpConfRepuestosXMArca.Update(m_dstConfRepuestosXMarca)
                End If

                'Generación de OT's especiales
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkGeneraOTsEspeciales, mc_strGeneraOTsEspeciales)

                'Copias Reporte Recepcion
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtCopiasRepRecepcion, mc_strCopiasRepRecepcion, False)

                Dim strValorConfiguracion As String
                Dim strEtiqueta As String

                If chkCosteoServicios.Checked = False Then
                    strValorConfiguracion = 0
                    strEtiqueta = "No Configurado"
                Else
                    If rbtEstandar.Checked = True Then
                        strValorConfiguracion = 1
                        strEtiqueta = "Estandar"
                    Else
                        strValorConfiguracion = 2
                        strEtiqueta = "Real"
                    End If
                End If

                Call ActualizaValorenFilaPersonalizado(dtbConfiguracion, drwConfiguracion, strValorConfiguracion, strEtiqueta, mc_strCosteoServicios)
                g_intCosteoServicios = CInt(strValorConfiguracion)

                'Tipo de Moneda para Costeo de Mano de Obra
                Call ActualizaValoresenFilaTextbox(dtbConfiguracion, m_drwConfiguracion, txtTipoMoneda, mc_strTipoMoneda, True)

                'Setear parametros de costo simple o detallado
                strValorConfiguracion = String.Empty
                strEtiqueta = String.Empty

                If chkCosteoServicios.Checked = False Then
                    strValorConfiguracion = 0
                    strEtiqueta = "No Configurado"
                Else
                    If rbtSimple.Checked = True Then
                        strValorConfiguracion = 1
                        strEtiqueta = "Simple"
                    Else
                        strValorConfiguracion = 2
                        strEtiqueta = "Detallado"
                    End If
                End If

                Call ActualizaValorenFilaPersonalizado(dtbConfiguracion, drwConfiguracion, strValorConfiguracion, strEtiqueta, mc_strTipoCosto)
                ' g_intCosteoServicios = CInt(strValorConfiguracion)

                strValorConfiguracion = String.Empty
                strEtiqueta = String.Empty

                If rb_OrdenCompra.Checked = True Then
                    strValorConfiguracion = 1
                    strEtiqueta = "OrdenCompra"
                Else
                    strValorConfiguracion = 2
                    strEtiqueta = "OfertaCompra"
                End If


                    Call ActualizaValorenFilaPersonalizado(dtbConfiguracion, drwConfiguracion, strValorConfiguracion, strEtiqueta, mc_strTipoCompra)
                    ' g_intCosteoServicios = CInt(strValorConfiguracion)

                'Usa Lista Precios Cliente
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chckUsaListaCliente, mc_strUsaListaCliente)

                'Permite cambiar Precio
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkCambiaPrecio, mc_strPermiteCambioPrecio)

                'Valida el Estado de la OT Padre para la creacion de la Hija
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkCrearOThijas, mc_strValidaEstadoOTPadre)

                'Asignación única de mecánico a MO
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkAsignacionUnicaMO, mc_strAsignacionUnicaMO)

                'Usa Solicitud OT Especiales
                Call ActualizaValorenFilaCheckbox(dtbConfiguracion, m_drwConfiguracion, chkSolOTEsp, mc_strUsaSolicitudOTEspecial)

                    Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw ex
                Return False
            End Try
        End Function

        Private Function ActualizaValorenFilaPersonalizado(ByVal dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, ByVal drwRow As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow, ByVal strValor As String, ByVal strEtiqueta As String, ByVal strPropiedad As String) As Boolean
            Dim txtTextBoxPersonal As New NEWTEXTBOX.NEWTEXTBOX_CTRL
            If strEtiqueta = "" Then
                strEtiqueta = " "
            End If
            txtTextBoxPersonal.Text = strEtiqueta
            txtTextBoxPersonal.Tag = strValor
            ActualizaValoresenFilaTextbox(dtbConfiguracion, drwRow, txtTextBoxPersonal, strPropiedad, True)

        End Function

        Private Function ActualizaValorenFilaCheckbox(ByRef dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                      ByRef drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow, _
                                                      ByVal chkControl As CheckBox, _
                                                      ByVal strPropiedad As String) As Boolean
            Try
                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)

                If Not drwConfiguracion Is Nothing Then

                    drwConfiguracion.Valor = CStr(chkControl.CheckState)

                Else

                    drwConfiguracion = dtbConfiguracion.NewSCGTA_TB_ConfiguracionRow
                    drwConfiguracion.Propiedad = strPropiedad
                    drwConfiguracion.Valor = CStr(chkControl.CheckState)
                    dtbConfiguracion.AddSCGTA_TB_ConfiguracionRow(drwConfiguracion)

                End If

                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Return False
            End Try
        End Function

        Private Function ActualizaValoresenFilaTextbox(ByRef dtbConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionDataTable, _
                                                        ByRef drwConfiguracion As ConfiguracionDataSet.SCGTA_TB_ConfiguracionRow, _
                                                        ByRef ntxtControl As NEWTEXTBOX.NEWTEXTBOX_CTRL, _
                                                        ByVal strPropiedad As String, _
                                                        ByVal blnBuscador As Boolean) As Boolean
            Try
                drwConfiguracion = dtbConfiguracion.FindByPropiedad(strPropiedad)

                If Not drwConfiguracion Is Nothing Then

                    If blnBuscador Then
                        If ntxtControl.Text <> "" Then
                            drwConfiguracion.Valor = ntxtControl.Tag
                            drwConfiguracion.Etiqueta = ntxtControl.Text
                        Else
                            drwConfiguracion.Valor = ""
                            drwConfiguracion.Etiqueta = ""
                        End If
                    Else
                        drwConfiguracion.Valor = ntxtControl.Text
                    End If
                Else
                    drwConfiguracion = dtbConfiguracion.NewSCGTA_TB_ConfiguracionRow

                    If blnBuscador Then
                        drwConfiguracion.Propiedad = strPropiedad
                        If ntxtControl.Text <> "" Then
                            drwConfiguracion.Valor = ntxtControl.Tag
                            drwConfiguracion.Etiqueta = ntxtControl.Text
                        Else
                            drwConfiguracion.Valor = ""
                            drwConfiguracion.Etiqueta = ""
                        End If
                    Else
                        drwConfiguracion.Propiedad = strPropiedad
                        drwConfiguracion.Valor = ntxtControl.Text
                    End If
                    dtbConfiguracion.AddSCGTA_TB_ConfiguracionRow(drwConfiguracion)
                End If

                Return True
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Return False
            End Try
        End Function

        Private Sub CargarBusquedaListaPrecios(ByRef buListas As Buscador.SubBuscador, ByRef Sender As Object)
            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buListas

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorListasPrecios
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Listaprecios
                    .Criterios = "ListNum,ListName"
                    .Criterios_OcultosEx = ""
                    .Where = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_OPLN"
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub CargarBusquedaTiposdeMoneda(ByVal buMonedas As Buscador.SubBuscador,
                                                ByVal sender As Object)
            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buMonedas

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorTiposMonedas
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.TipoMoneda
                    .Criterios = "Currcode,CurrName"
                    .Criterios_OcultosEx = ""
                    .Where = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_OCRN"
                    .Activar_Buscador(sender)
                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub CargarDatosConfRepuestosXMarca()

            m_dstConfRepuestosXMarca = Nothing
            m_dstConfRepuestosXMarca = New ConfCatalogoRepXMarcaDataset
            m_adpConfRepuestosXMArca = Nothing
            m_adpConfRepuestosXMArca = New ConfCatalogoRepXMarcaDataAdapter

            dtgMarcasConfiguradas.DataSource = Nothing
            m_adpConfRepuestosXMArca.Fill(m_dstConfRepuestosXMarca)
            dtgMarcasConfiguradas.DataSource = m_dstConfRepuestosXMarca
            dtgMarcasConfiguradas.DataMember = "SCGTA_TB_ConfCatalogoRepxMarca"

        End Sub

#End Region


        Private Sub CargarBuscadorArticulos(ByVal sender As Object)
            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With m_BuConfiguracion

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busArticuloCotizacion
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                    .Criterios = "ItemCode,ItemName"
                    .Criterios_OcultosEx = ""
                    .Where = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_VW_OITM"
                    .Activar_Buscador(sender)
                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try
        End Sub


        Private Sub CargarBusquedaUnidadesTiempo(ByRef buListas As Buscador.SubBuscador, ByRef Sender As Object)
            Dim DATemp As DMSOneFramework.SCGDataAccess.DAConexion

            Try

                DATemp = New DMSOneFramework.SCGDataAccess.DAConexion

                With buListas

                    .SQL_Cnn = DATemp.ObtieneConexion
                    .Barra_Titulo = My.Resources.ResourceUI.busBarraTituloUnidadesTiempo
                    .Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                    .Criterios = "CodigoUnidadTiempo,DescripcionUnidadTiempo"
                    .Criterios_OcultosEx = ""
                    .Where = ""
                    .MultiSeleccion = False
                    .Tabla = "SCGTA_TB_UnidadTiempo"
                    .Activar_Buscador(Sender)
                End With

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Sub

        Private Sub picUnidadesTiempo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picUnidadesTiempo.Click
            CargarBusquedaUnidadesTiempo(m_BuConfiguracion, sender)
        End Sub


        Private Sub chkCosteoServicios_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCosteoServicios.CheckedChanged
            If chkCosteoServicios.Checked = True Then
                gbxTipoCosteoServicios.Enabled = True
                gbxTipoCostos.Enabled = True
                If rbtTiempoReal.Checked = False Then
                    rbtEstandar.Checked = True
                End If

                If rbtDetallado.Checked = False Then
                    rbtSimple.Checked = True
                End If

            Else
                gbxTipoCosteoServicios.Enabled = False
                gbxTipoCostos.Enabled = False
            End If

      

        End Sub


        Private Sub picCotizaciones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picCotizaciones.Click
            Call CargarBuscadordeSeries(m_BuConfiguracion, sender)
        End Sub

        Private Sub picOrdenesVenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Call CargarBuscadordeSeries(m_BuConfiguracion, sender)
        End Sub

        Private Sub picSolicitudEspecificos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        End Sub

        Private Sub picOrdenCompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoOrdenCompra.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub

        Private Sub picArticuloCotizacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picArticuloCotizacion.Click
            CargarBuscadorArticulos(sender)
        End Sub

        Private Sub gbVentas_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbVentas.Enter

        End Sub

        'Agregado 01/11/2010: Busca usuarios de SBO para encargado de accesorios
        Private Sub picEncargadoAccesorios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picEncargadoAccesorios.Click
            Call CargarBuscadordeUsuariosSBO(m_BuConfiguracion, sender)
        End Sub

        Private Sub CargarBuscadorCuentasContables(ByVal sender As Object)
            Try

                Dim DATemp As New DMSOneFramework.SCGDataAccess.DAConexion
                m_objBuscador = New Buscador.SubBuscador
                m_objBuscador.SQL_Cnn = DATemp.ObtieneConexion
                m_objBuscador.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloBuscadorCuentasContables
                m_objBuscador.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Descripcion
                m_objBuscador.Criterios = "ACCTCODE, ACCTNAME"
                m_objBuscador.Where = "postable = 'Y' and fixed = 'N'"
                m_objBuscador.Tabla = "SCGTA_VW_OACT"
                m_objBuscador.Activar_Buscador(sender)

            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")
            End Try

        End Sub

        Private Sub m_objBuscador_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscador.AppAceptar
            Try
                Select Case sender.name
                    'Case picBuscadorTiposOrdenes.Name
                    '    txtTipoOrden.Tag = Arreglo_Campos(0)
                    '    txtTipoOrden.Text = Arreglo_Campos(1)
                    Case piCuentasContables.Name
                        txtNumeroCuenta.Text = Arreglo_Campos(0)
                        txtNombreCuenta.Text = Arreglo_Campos(1)
                End Select
            Catch ex As Exception
                ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, "SCG DMS ONE")
            End Try
        End Sub

        Private Sub piCuentasContables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles piCuentasContables.Click
            CargarBuscadorCuentasContables(sender)
        End Sub

        Private Sub rbtSimple_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtSimple.CheckedChanged
            If rbtSimple.Checked = True Then

                rbtDetallado.Checked = False

            End If
        End Sub

        Private Sub rbtDetallado_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtDetallado.CheckedChanged
            If rbtDetallado.Checked = True Then

                rbtSimple.Checked = False

            End If
        End Sub


        Private Sub picOfertasdeCompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picOfertasdeCompra.Click
            Call CargarBuscadordeSeries(m_BuConfiguracion, sender)
        End Sub

        Private Sub picTipoMoneda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles picTipoMoneda.Click

            CargarBusquedaTiposdeMoneda(m_BuConfiguracion, sender)

        End Sub

    End Class

End Namespace



