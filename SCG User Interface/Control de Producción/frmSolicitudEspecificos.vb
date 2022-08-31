Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmSolicitudEspecificos

#Region "Declaraciones"

#Region "Variables"

        Private m_intIDSolicitud As Integer
        Private m_intIDItem As Integer


        'Variables para cargar buscador
        Private m_strServidor As String
        Private m_strBasedatos As String
        Private m_strUsuario As String
        Private m_strPassword As String
        Private m_strListaPrecios As String
        Private m_strAlmacen As String

        Private m_strCampos As String
        Private m_strTabla As String
        Private m_strWhere As String

        Private m_blnAgregarACompañiaActual As Boolean
        Private blnArticuloNuevo As Boolean = False
        Private cb As DataGridViewCheckBoxCell

        Private blPrimeravez As Boolean = True
        Private strCurrencyDocumento As String = String.Empty
        Private strMonedaLocal As String = String.Empty
        Private strMonedaSistema As String = String.Empty
        Private strTipoCambioCotizacion As String = String.Empty
        Private decTipoCambioCotizacion As Decimal = 0
        Private strTipoCambioMS As String = String.Empty
        Private decTipoCambioMS As Decimal = 0
        Private strFechaCotizacion As String = String.Empty
        
        Private strMonedaAnterior As String
        Private strPrecioTotal As String = String.Empty

        Private strListaPreciosManejar As String = String.Empty
        Public g_strUsaAsocxEspecif As String
        Public g_strEspecifVehi As String
        Dim DtConf As System.Data.DataTable
        Dim g_strUsaFilSer As String
        Dim g_strUsaFilRep As String


#End Region

#Region "Constantes"
        Private Const strUsaListaCliente As String = "UsaListaPreciosCliente"
        Private Const strListaPrecios As String = "ListaPrecios"
#End Region
#Region "Acceso a datos"

        Private m_dtsSolicitudEspecificos As New SolicitudEspecificosDataset
        Private m_adpSolicitudEspecificos As New SolicitudEspecificosDataAdapter

        Private m_drwSolicitudEspecificos As SolicitudEspecificosDataset.SCGTA_SP_SelSolicitudEspecificoRow

        Private m_dtsItemsSolicitados As New ItemSolicitudEspecificoDataset
        Private m_adpItemsSolicitados As New ItemSolicitudEspecificoDataAdapter

        Private m_objDATemp As New DMSOneFramework.SCGDataAccess.DAConexion

        Private m_cnConeccion As SqlClient.SqlConnection

        Private m_dtsSoloLectura As Boolean = False

#End Region

#Region "Objetos Generales"

        Private WithEvents m_objBuscadorItems As Buscador.SubBuscador

#End Region

#Region "Eventos"

        Public Event eSolicitudCreada(ByVal p_intNoOslicitud As Integer)

#End Region

#End Region

#Region "Constructor"

        Public Sub New(ByVal p_intIdSolicitud As Integer)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_intIDSolicitud = p_intIdSolicitud

        End Sub

#End Region

#Region "Metodos"

        Private Sub CargarDatos()

            m_adpSolicitudEspecificos.Fill(m_dtsSolicitudEspecificos, m_intIDSolicitud)
            GlobalesUI.LlenarEstadoSolicitudEspecificosResources(m_dtsSolicitudEspecificos)
            m_drwSolicitudEspecificos = m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.FindByID(m_intIDSolicitud)


            If Not m_drwSolicitudEspecificos.IsCardNameNull() Then
                txtCliente.Text = m_drwSolicitudEspecificos.CardName
            End If

            'txtEstado.Text = m_drwSolicitudEspecificos.DescEstado
            txtEstado.Text = m_drwSolicitudEspecificos.DescEstadoResources
            If Not m_drwSolicitudEspecificos.IsDescEstiloNull() Then
                txtEstilo.Text = m_drwSolicitudEspecificos.DescEstilo
            End If
            If Not m_drwSolicitudEspecificos.IsDescMarcaNull Then
                txtMarca.Text = m_drwSolicitudEspecificos.DescMarca
            End If
            If Not m_drwSolicitudEspecificos.IsAsesorNull Then
                txtAsesor.Text = m_drwSolicitudEspecificos.Asesor
            End If
            txtNoOrden.Text = m_drwSolicitudEspecificos.NoOrden
            txtNoSolicitud.Text = m_drwSolicitudEspecificos.ID
            txtNoVisita.Text = m_drwSolicitudEspecificos.NoVisita
            If Not m_drwSolicitudEspecificos.IsObservacionNull Then
                txtObservacionesOrden.Text = m_drwSolicitudEspecificos.Observacion
            End If
            If Not m_drwSolicitudEspecificos.IsPlacaNull Then
                txtPlaca.Text = m_drwSolicitudEspecificos.Placa
            End If
            If Not m_drwSolicitudEspecificos.IsRespondidoPorNull Then
                txtResponde.Text = m_drwSolicitudEspecificos.RespondidoPor
            End If
            txtSolicita.Text = m_drwSolicitudEspecificos.SolicitadoPor
            txtTipoOrden.Text = m_drwSolicitudEspecificos.TipoDesc
            If Not m_drwSolicitudEspecificos.IsNoVehiculoNull Then
                txtNoUnidad.Text = m_drwSolicitudEspecificos.NoVehiculo
            End If
            If Not m_drwSolicitudEspecificos.IsFechaRespuestaNull Then
                txtFechaRespuesta.Text = m_drwSolicitudEspecificos.FechaRespuesta.ToShortDateString
            End If
            txtFechaSolicitud.Text = m_drwSolicitudEspecificos.FechaSolicitud.ToShortDateString

            If Not m_drwSolicitudEspecificos.IsAnoVehiculoNull Then
                txtAño.Text = m_drwSolicitudEspecificos.AnoVehiculo
            End If

            If Not m_drwSolicitudEspecificos.IsVINNull Then
                txtVIN.Text = m_drwSolicitudEspecificos.VIN
            End If

            lblDocCur.Text = m_drwSolicitudEspecificos.DocCur


            If IsDBNull(m_drwSolicitudEspecificos("PrecioTotal")) Then
                txtTotalRepuestos.Text = "0.00"
            Else
                txtTotalRepuestos.Text = m_drwSolicitudEspecificos.PrecioTotal.ToString("n2")
            End If
            m_adpItemsSolicitados.Fill(m_dtsItemsSolicitados, m_intIDSolicitud)


            dtgDetalles.DataSource = Nothing

            dtgDetalles.DataSource = m_dtsItemsSolicitados
            dtgDetalles.DataMember = m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.TableName

            If m_drwSolicitudEspecificos.Estado <> 0 Then
                dtgDetalles.Columns.Item("PrecioAcordado").ReadOnly = True
                dtgDetalles.Columns.Item("SinExistencia").ReadOnly = True
                m_dtsSoloLectura = True
                btnAceptar.Enabled = False
                btnCancelarSolicitud.Enabled = False
            End If

        End Sub

        Private Sub CargarDatosCompañia()

            Dim m_dtsConfRepuestosXMarca As New ConfCatalogoRepXMarcaDataset
            Dim m_adpConfRepuestosXMarca As New ConfCatalogoRepXMarcaDataAdapter
            Dim drwConfRepuestosXMarca As ConfCatalogoRepXMarcaDataset.SCGTA_TB_ConfCatalogoRepxMarcaRow
            Dim m_dtArticulos As System.Data.DataTable
            Dim m_strConsultaArticulos As String = "  Select U_ItemCode from [@SCGD_ARTXESP] where U_TipoArt = '{0}' "
            Dim m_strFiltroMod As String = " and U_CodMod = '{0}' "
            Dim m_strFiltroArt As String = " and U_CodEsti = '{0}' "
            Dim m_bTieneArt As Boolean = False

            Const c_strListaPrecios As String = "ListaPrecios"

            DtConf = Utilitarios.EjecutarConsultaDataTable("Select U_UsaAXEV,U_EspVehic,U_UsaFilSer,U_UsaFilRep from [@SCGD_ADMIN]", strConexionSBO)
            g_strUsaAsocxEspecif = DtConf.Rows(0)("U_UsaAXEV").ToString().Trim()           ''Utilitarios.EjecutarConsulta("Select U_UsaAXEV from [@SCGD_ADMIN]", strConexionSBO)
            g_strEspecifVehi = DtConf.Rows(0)("U_EspVehic").ToString().Trim() ''Utilitarios.EjecutarConsulta("Select U_EspVehic from [@SCGD_ADMIN]", strConexionSBO)
            g_strUsaFilRep = DtConf.Rows(0)("U_UsaFilRep").ToString().Trim()
            g_strUsaFilSer = DtConf.Rows(0)("U_UsaFilSer").ToString().Trim()



            m_adpConfRepuestosXMarca.Fill(m_dtsConfRepuestosXMarca, , m_drwSolicitudEspecificos.CodMarca)

            If g_strUsaAsocxEspecif.Equals("Y") Then

              
                If g_strUsaFilRep.Equals("Y") Then

                    If g_strEspecifVehi.Equals("M") Then
                        m_strFiltroMod = String.Format(m_strFiltroMod, m_drwSolicitudEspecificos.CodModelo)
                        m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 1)
                        m_strConsultaArticulos = m_strConsultaArticulos & m_strFiltroMod
                        m_dtArticulos = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, strConexionSBO)

                        If m_dtArticulos.Rows.Count > 0 Then
                            m_bTieneArt = True
                        End If
                    Else
                        m_strFiltroMod = String.Format(m_strFiltroMod, m_drwSolicitudEspecificos.CodEstilo)
                        m_strConsultaArticulos = String.Format(m_strConsultaArticulos, 1)
                        m_strConsultaArticulos = m_strConsultaArticulos & m_strFiltroMod
                        m_dtArticulos = Utilitarios.EjecutarConsultaDataTable(m_strConsultaArticulos, strConexionSBO)

                        If m_dtArticulos.Rows.Count > 0 Then
                            m_bTieneArt = True
                        End If
                    End If



                End If
            End If



            If m_dtsConfRepuestosXMarca.SCGTA_TB_ConfCatalogoRepxMarca.Rows.Count > 0 Then

                drwConfRepuestosXMarca = m_dtsConfRepuestosXMarca.SCGTA_TB_ConfCatalogoRepxMarca.Rows(0)

                m_strServidor = drwConfRepuestosXMarca.Servidor
                m_strBasedatos = drwConfRepuestosXMarca.BDCompañia
                m_strUsuario = drwConfRepuestosXMarca.UsuarioServidor
                m_strPassword = drwConfRepuestosXMarca.PasswordServidor

                m_strListaPrecios = drwConfRepuestosXMarca.CodListaPrecio
                m_strAlmacen = drwConfRepuestosXMarca.CodAlmacen


                If g_strUsaAsocxEspecif.Equals("Y") Then

                    If m_bTieneArt Then


                        If g_strUsaFilRep.Equals("Y") Then
                            m_strCampos = " TOP 50 SCGTA_VW_OITM.itemcode,itemname(SCGTA_VW_OITW.OnHand  - SCGTA_VW_OITW.IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"

                            m_strTabla = "SCGTA_VW_OITM " & _
                                        "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                        "INNER JOIN SCGTA_VW_ARTXESP AS AXESP ON SCGTA_VW_OITM.ItemCode = AXESP.U_ItemCode " & _
                                        "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                        "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                        "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default " & _
                                        "LEFT OUTER JOIN SCGTA_VW_Estilos as Esti on AXESP.U_CodEsti = Esti.Code " & _
                                        "LEFT OUTER JOIN SCGTA_VW_Modelos as Mode on AXESP.U_CodMod = Mode.Code "

                            If g_strEspecifVehi.Equals("E") Then

                                m_strWhere = " AXESP.U_TipoArt= '1' " & _
                                               " and PriceList='" & m_strListaPrecios & "' and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'" & _
                                               " and AXESP.U_CodEsti ='" & (m_drwSolicitudEspecificos.CodEstilo) & "'"

                            ElseIf g_strEspecifVehi.Equals("M") Then

                                m_strWhere = " AXESP.U_TipoArt= '1' " & _
                                               " and PriceList='" & m_strListaPrecios & "' and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'" & _
                                               " and AXESP.U_CodMod ='" & (m_drwSolicitudEspecificos.CodModelo) & "'"

                            Else

                                m_strWhere = " AXESP.U_TipoArt= '1' " & " and PriceList=" & CStr(m_strListaPrecios) & " and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null "

                            End If
                        Else
                            m_strTabla = "OITM Inner join OITW on OITM.itemCode = OITW.itemCode " & _
                                                       "INNER JOIN ITM1 AS ListP ON OITM.ItemCode = ListP.ItemCode"
                            m_strCampos = "top 50 OITM.itemcode,OITM.itemname,(OITW.OnHand - OITW.IsCommited),ListP.Currency,ListP.Price, OITM.CodeBars"
                            m_strWhere = "ListP.PriceList = " & m_strListaPrecios & " and OITW.WhsCode = '" & m_strAlmacen & "'"

                            m_cnConeccion = m_objDATemp.ObtieneConexion(m_strServidor, m_strBasedatos, m_strUsuario, m_strPassword)

                            m_blnAgregarACompañiaActual = False


                        End If
                    Else
                        m_strTabla = "OITM Inner join OITW on OITM.itemCode = OITW.itemCode " & _
                                                                       "INNER JOIN ITM1 AS ListP ON OITM.ItemCode = ListP.ItemCode"
                        m_strCampos = "top 50 OITM.itemcode,OITM.itemname,(OITW.OnHand - OITW.IsCommited),ListP.Currency,ListP.Price, OITM.CodeBars"
                        m_strWhere = "ListP.PriceList = " & m_strListaPrecios & " and OITW.WhsCode = '" & m_strAlmacen & "'"

                        m_cnConeccion = m_objDATemp.ObtieneConexion(m_strServidor, m_strBasedatos, m_strUsuario, m_strPassword)

                        m_blnAgregarACompañiaActual = False
                    End If




                Else
                    m_strTabla = "OITM Inner join OITW on OITM.itemCode = OITW.itemCode " & _
                                                "INNER JOIN ITM1 AS ListP ON OITM.ItemCode = ListP.ItemCode"
                    m_strCampos = "top 50 OITM.itemcode,OITM.itemname,(OITW.OnHand - OITW.IsCommited),ListP.Currency,ListP.Price, OITM.CodeBars"
                    m_strWhere = "ListP.PriceList = " & m_strListaPrecios & " and OITW.WhsCode = '" & m_strAlmacen & "'"

                    m_cnConeccion = m_objDATemp.ObtieneConexion(m_strServidor, m_strBasedatos, m_strUsuario, m_strPassword)

                    m_blnAgregarACompañiaActual = False

                End If



            Else

                If g_strUsaAsocxEspecif.Equals("Y") Then

                    If m_bTieneArt Then



                        If g_strUsaFilRep.Equals("Y") Then
                            ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, c_strListaPrecios, m_strListaPrecios)
                            m_cnConeccion = m_objDATemp.ObtieneConexion()
                            m_strCampos = " TOP 50 SCGTA_VW_OITM.itemcode,itemname,(SCGTA_VW_OITW.OnHand  - SCGTA_VW_OITW.IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"

                            m_strTabla = "SCGTA_VW_OITM " & _
                                         "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                         "INNER JOIN SCGTA_VW_ARTXESP AS AXESP ON SCGTA_VW_OITM.ItemCode = AXESP.U_ItemCode " & _
                                         "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                         "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                         "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default " & _
                                         "LEFT OUTER JOIN SCGTA_VW_Estilos as Esti on AXESP.U_CodEsti = Esti.Code " & _
                                         "LEFT OUTER JOIN SCGTA_VW_Modelos as Mode on AXESP.U_CodMod = Mode.Code "

                            If g_strEspecifVehi.Equals("E") Then

                                m_strWhere = " U_SCGD_TipoArticulo= '1' " & _
                                              " and PriceList=" & CStr(m_strListaPrecios) & " and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'" & _
                                              " and AXESP.U_CodEsti ='" & (m_drwSolicitudEspecificos.CodEstilo) & "'"

                            ElseIf g_strEspecifVehi.Equals("M") Then

                                m_strWhere = " U_SCGD_TipoArticulo= '1' " & _
                                               " and PriceList='" & m_strListaPrecios & "'  and PrchseItem = 'Y' and SellItem = 'Y' and InvntItem = 'Y'" & _
                                               " and AXESP.U_CodMod ='" & (m_drwSolicitudEspecificos.CodModelo) & "'"

                            Else

                                m_strWhere = " U_SCGD_TipoArticulo= '1' " & " and PriceList='" & m_strListaPrecios & "' and SellItem = 'Y' and InvntItem = 'N' and U_SCGD_T_Fase is not null "

                            End If
                        Else
                            ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, c_strListaPrecios, m_strListaPrecios)
                            m_cnConeccion = m_objDATemp.ObtieneConexion()

                            m_strTabla = "SCGTA_VW_OITM " & _
                                        "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                        "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                        "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                        "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default"
                            m_strCampos = "top 50 SCGTA_VW_OITM.itemcode,SCGTA_VW_OITM.itemname,(SCGTA_VW_OITW.OnHand  - SCGTA_VW_OITW.IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"
                            m_strWhere = "ListP.PriceList = '" & m_strListaPrecios & "' and SCGTA_VW_OITM.U_SCGD_TipoArticulo = 1 And SCGTA_VW_OITM.U_SCGD_Generico = 1"

                            m_blnAgregarACompañiaActual = True
                        End If
                    Else
                        ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, c_strListaPrecios, m_strListaPrecios)
                        m_cnConeccion = m_objDATemp.ObtieneConexion()

                        m_strTabla = "SCGTA_VW_OITM " & _
                                    "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                    "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                    "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                    "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default"
                        m_strCampos = "top 50 SCGTA_VW_OITM.itemcode,SCGTA_VW_OITM.itemname,(SCGTA_VW_OITW.OnHand  - SCGTA_VW_OITW.IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"
                        m_strWhere = "ListP.PriceList = '" & m_strListaPrecios & "' and SCGTA_VW_OITM.U_SCGD_TipoArticulo = 1 And SCGTA_VW_OITM.U_SCGD_Generico = 1"

                        m_blnAgregarACompañiaActual = True
                    End If



                Else
                    ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, c_strListaPrecios, m_strListaPrecios)
                    m_cnConeccion = m_objDATemp.ObtieneConexion()

                    m_strTabla = "SCGTA_VW_OITM " & _
                                "Inner join SCGTA_VW_OITW on SCGTA_VW_OITM.itemCode = SCGTA_VW_OITW.itemCode " & _
                                "INNER JOIN SCGTA_VW_ITM1 AS ListP ON SCGTA_VW_OITM.ItemCode = ListP.ItemCode " & _
                                "INNER JOIN SCGTA_TB_ConfBodegasXCentroCosto AS BXCC ON SCGTA_VW_OITM.U_SCGD_CodCtroCosto = BXCC.IDCentroCosto " & _
                                "AND SCGTA_VW_OITW.WhsCode  = BXCC.Repuestos COLLATE database_default"
                    m_strCampos = "top 50 SCGTA_VW_OITM.itemcode,SCGTA_VW_OITM.itemname,(SCGTA_VW_OITW.OnHand  - SCGTA_VW_OITW.IsCommited),ListP.Currency,ListP.Price,SCGTA_VW_OITM.CodeBars"
                    m_strWhere = "ListP.PriceList = '" & m_strListaPrecios & "' and SCGTA_VW_OITM.U_SCGD_TipoArticulo = 1 And SCGTA_VW_OITM.U_SCGD_Generico = 1"

                    m_blnAgregarACompañiaActual = True
                End If

            End If



        End Sub

        Private Function ResponderSolicitud() As Boolean

            Dim dstRepuestosxOrden As New RepuestosxOrdenDataset
            Dim dtsRepuestosxOrdenAnterior As New RepuestosxOrdenDataset
            Dim cnConeccion As SqlClient.SqlConnection = Nothing
            Dim tnTransaccion As SqlClient.SqlTransaction = Nothing
            Dim blnResultado As Boolean = True
            Dim adpRep As New RepuestosxOrdenDataAdapter
            Dim drwItemSolicitud As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow
            Dim drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim intFilasModificadas As Integer = 0

            Dim lsListaPrecios As IList(Of String) = New List(Of String)

            '****************************Manejo Multimoneda***************************
            'Dim PreciosinConvert As Decimal = 0
            'Dim decCodArticulo As String = ""
            'Dim strCurrencyArticulo As String = ""
            'Dim PrecioConvertido As Decimal = 0

            'Dim decTipoCambioCotizacion As Decimal = 0
            'Dim strTipoCambioCotizacion As String = 0

            'Dim strMonedaCotizacion As String = ""
            'Dim strFechaCotizacion As String = ""

            'Dim strMonedaSistema As String = ""
            'Dim strMonedaLocal As String = ""

            'Dim decTipoCambioMS As String = 0
            'Dim strTipoCambioMS As String = ""
            '**************************************************************************

            Try
                adpRep.Fill(dtsRepuestosxOrdenAnterior, m_drwSolicitudEspecificos.NoOrden)
                MetodosCompartidosSBOCls.IniciaTransaccion()
                If m_blnAgregarACompañiaActual Then
                    MetodosCompartidosSBOCls.IniciarCotizacion(m_drwSolicitudEspecificos.NoCotizacion)


                    ''********************** Manejo de moneda para la cotizacion, moneda local,sistema y Extranjera *************************
                    'strMonedaLocal = Utilitarios.ObtenerMonedaLocal()
                    'strMonedaSistema = Utilitarios.ObtenerMonedaSistema()

                    'strTipoCambioCotizacion = Utilitarios.EjecutarConsulta(
                    '                                         String.Format("select DocRate from SCGTA_VW_OQUT where DocEntry = '{0}'",
                    '                                                         m_drwSolicitudEspecificos.NoCotizacion.ToString.Trim()),
                    '                                         strConexionADO)

                    'If Not String.IsNullOrEmpty(strTipoCambioCotizacion) Then decTipoCambioCotizacion = Decimal.Parse(strTipoCambioCotizacion)


                    'strMonedaCotizacion = Utilitarios.EjecutarConsulta(
                    '                                        String.Format("select DocCur from SCGTA_VW_OQUT where DocEntry = '{0}'",
                    '                                                        m_drwSolicitudEspecificos.NoCotizacion.ToString.Trim()),
                    '                                        strConexionADO)

                    'strFechaCotizacion = Utilitarios.EjecutarConsulta(
                    '                                        String.Format("select DocDate from SCGTA_VW_OQUT where DocEntry = '{0}'",
                    '                                                        m_drwSolicitudEspecificos.NoCotizacion.ToString.Trim()),
                    '                                        strConexionADO)
                    'strFechaCotizacion = Utilitarios.RetornaFechaFormatoRegional(strFechaCotizacion.ToString.Trim())


                    'strTipoCambioMS = Utilitarios.EjecutarConsulta(
                    '                                        String.Format("select Rate from SCGTA_VW_ORTT where RateDate = '{0}' and Currency = '{1}'",
                    '                                                       strFechaCotizacion.ToString.Trim(), strMonedaSistema.ToString.Trim()),
                    '                                       strConexionADO)
                    'If Not String.IsNullOrEmpty(strTipoCambioMS) Then decTipoCambioMS = Decimal.Parse(strTipoCambioMS)


                    For Each drwItemSolicitud In m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.Rows

                        ''*************************************************************************************
                        ''Currency del repuesto
                        'strCurrencyArticulo = drwItemSolicitud.Item("Currency").ToString.Trim()

                        ''Trae el precio del servicio SIN CONVERSION
                        'If Not String.IsNullOrEmpty(drwItemSolicitud.Item("PrecioAcordado").ToString().Trim()) Then PreciosinConvert = Decimal.Parse(drwItemSolicitud.Item("PrecioAcordado").ToString().Trim())
                        'If Not String.IsNullOrEmpty(drwItemSolicitud.Item("CodEspecifico").ToString().Trim()) Then decCodArticulo = drwItemSolicitud.Item("CodEspecifico").ToString().Trim()


                        ''Llamada al proceso que me hace la conversion de la moneda
                        'PrecioConvertido = Utilitarios.ManejoMultimonedaPrecios(PreciosinConvert,
                        '                                            strMonedaCotizacion,
                        '                                            decTipoCambioCotizacion,
                        '                                            decTipoCambioMS,
                        '                                            decCodArticulo,
                        '                                            strFechaCotizacion,
                        '                                            strMonedaLocal,
                        '                                            strMonedaSistema,
                        '                                            strCurrencyArticulo)

                        'If PrecioConvertido = -2 Then
                        '    MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        '    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + strMonedaSistema + My.Resources.ResourceUI.ParaLaFecha + strFechaCotizacion)
                        '    Exit Function
                        'End If
                        'If PrecioConvertido = -4 Then
                        '    MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        '    objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + strCurrencyArticulo + My.Resources.ResourceUI.ParaLaFecha + strFechaCotizacion)
                        '    Exit Function
                        'End If

                        'drwItemSolicitud.Item("PrecioAcordado") = PrecioConvertido
                        '************************************************************************

                        If drwItemSolicitud.LineNum > -1 Then
                            For Each drwRepuestos In dtsRepuestosxOrdenAnterior.SCGTA_TB_RepuestosxOrden.Rows
                                If drwRepuestos.RowState <> DataRowState.Deleted Then
                                    If drwRepuestos.LineNum = drwItemSolicitud.LineNum Then
                                        MetodosCompartidosSBOCls.EliminarItemCotizacion(drwRepuestos.LineNum, "El ítem se cambió por " & drwItemSolicitud.CodEspecifico)
                                        drwRepuestos.Delete()
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Next
                    MetodosCompartidosSBOCls.ActualizarCotizacion()
                Else
                    MetodosCompartidosSBOCls.IniciarCotizacion(m_drwSolicitudEspecificos.NoCotizacion)
                    For Each drwItemSolicitud In m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.Rows
                        If drwItemSolicitud.LineNum > -1 Then
                            For Each drwRepuestos In dtsRepuestosxOrdenAnterior.SCGTA_TB_RepuestosxOrden.Rows
                                If drwRepuestos.RowState <> DataRowState.Deleted Then
                                    If drwRepuestos.LineNum = drwItemSolicitud.LineNum Then
                                        intFilasModificadas += 1
                                        drwRepuestos.ItemCodeEspecifico = drwItemSolicitud.CodEspecifico
                                        drwRepuestos.ItemNameEspecifico = drwItemSolicitud.NomEspecifico
                                        drwRepuestos.PrecioAcordado = drwItemSolicitud.PrecioAcordado
                                        drwRepuestos.Cantidad = drwItemSolicitud.Cantidad
                                        MetodosCompartidosSBOCls.AgregarEspecificoAItemCotizacion(drwRepuestos.LineNum, drwRepuestos.ItemCodeEspecifico, drwRepuestos.ItemNameEspecifico, drwRepuestos.PrecioAcordado, drwItemSolicitud.Cantidad, drwItemSolicitud.Currency)
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Next
                    MetodosCompartidosSBOCls.ActualizarCotizacion()
                End If
                If intFilasModificadas = 0 Then
                    blnResultado = GuardaRepuestos(dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, _
                                                       m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico)
                End If
                If blnResultado Then

                    m_drwSolicitudEspecificos.RespondidoPor = G_strUsuarioAplicacion
                    m_drwSolicitudEspecificos.Estado = 1
                    m_drwSolicitudEspecificos.PrecioTotal = Convert.ToDecimal(txtTotalRepuestos.Text.ToString.Trim())
                    
                    m_adpSolicitudEspecificos.Update(m_dtsSolicitudEspecificos, cnConeccion, tnTransaccion, False)
                    Call ActualizaRepuestosxOrdenenBD(dtsRepuestosxOrdenAnterior, cnConeccion, tnTransaccion, m_drwSolicitudEspecificos.NoCotizacion)
                    Call ActualizaRepuestosxOrdenenBD(dstRepuestosxOrden, cnConeccion, tnTransaccion, m_drwSolicitudEspecificos.NoCotizacion)
                    m_adpItemsSolicitados.Update(m_dtsItemsSolicitados, cnConeccion, tnTransaccion)
                    tnTransaccion.Commit()
                    cnConeccion.Close()

                End If

                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Commit)
                Return blnResultado
            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                If cnConeccion.State = ConnectionState.Open Then
                    tnTransaccion.Rollback()
                    cnConeccion.Close()
                End If
                Throw ex

            Finally
                Dim Cont As Integer = 0
                For Each drwItemSolicitud In m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.Rows
                    If Cont >= lsListaPrecios.Count Then Exit For
                    drwItemSolicitud.Item("PrecioAcordado") = lsListaPrecios(Cont)
                    Cont = Cont + 1
                Next
                'Agregado 05072010
                If cnConeccion IsNot Nothing Then
                    Call cnConeccion.Close()
                End If

            End Try
        End Function

        Private Function ValiarItemsEspecíficos() As Boolean
            Dim drwItemSolicitud As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow
            Dim blnTieneEspecificos As Boolean = True
            For Each drwItemSolicitud In m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.Rows
                If drwItemSolicitud.IsCodEspecificoNull OrElse drwItemSolicitud.CodEspecifico = "" Then
                    blnTieneEspecificos = False
                    Exit For
                End If
            Next
            Return blnTieneEspecificos
        End Function

        Private Function GuardaRepuestos(ByRef dtbRepuestosxOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable, _
                                           ByVal dtbitemSAP As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoDataTable) As Boolean

            Try
                Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
                Dim drwItemSap As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow

                Dim strItemCode As String
                Dim strItemName As String
                Dim strItemCodeEspecifico As String
                Dim strItemNameEspecifico As String

                Dim intLineNum As Integer
                Dim intIDMecanico As Integer
                Dim strNombreMecanico As String
                Dim blnRepuestosAgregados As Boolean = True
                Dim blnActulizaCotizacion As Boolean = True

                dtbRepuestosxOrden.Rows.Clear()

                'Ciclo para validar si existe tipo de cambio para los items
                For Each drwItemSap In dtbitemSAP.Rows
                    If Not drwItemSap.IsCurrencyNull Then
                        If ValidarMonedaItems(drwItemSap.Currency.ToString.Trim()) = False Then
                            Return False
                        End If
                    Else
                        drwItemSap.Currency = ""
                    End If

                Next

                MetodosCompartidosSBOCls.IniciarCotizacion(m_drwSolicitudEspecificos.NoCotizacion)
                For Each drwItemSap In dtbitemSAP.Rows

                    If drwItemSap.Nuevo = False Then
                        If Not drwItemSap.IsCodEspecificoNull Then
                            If m_blnAgregarACompañiaActual Then
                                strItemCode = drwItemSap.CodEspecifico
                                strItemName = drwItemSap.NomEspecifico
                                strItemCodeEspecifico = ""
                                strItemNameEspecifico = ""

                            Else
                                strItemCode = drwItemSap.ItemCodeGenerico
                                strItemName = drwItemSap.NomEspecifico
                                strItemCodeEspecifico = drwItemSap.CodEspecifico
                                strItemNameEspecifico = drwItemSap.NomEspecifico
                            End If
                            If Not drwItemSap.IsIDEmpleadoNull Then
                                intIDMecanico = drwItemSap.IDEmpleado
                            Else
                                intIDMecanico = -1
                            End If
                            If Not drwItemSap.IsNombreEmpleadoNull Then
                                strNombreMecanico = drwItemSap.NombreEmpleado
                            Else
                                strNombreMecanico = -1
                            End If
                            drwRepuesto = dtbRepuestosxOrden.NewSCGTA_TB_RepuestosxOrdenRow
                            If drwItemSap.IsPrecioAcordadoNull() Then
                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_drwSolicitudEspecificos.NoCotizacion, strItemCode, drwItemSap.Cantidad, drwItemSap.FreeTxt, g_strImpRepuestos, , strItemCodeEspecifico, strItemNameEspecifico, intIDMecanico, strNombreMecanico)
                            ElseIf drwItemSap.PrecioAcordado = 0 Then

                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_drwSolicitudEspecificos.NoCotizacion, strItemCode, drwItemSap.Cantidad, drwItemSap.FreeTxt, g_strImpRepuestos, , strItemCodeEspecifico, strItemNameEspecifico, intIDMecanico, strNombreMecanico)

                            Else


                                intLineNum = MetodosCompartidosSBOCls.AgregarItemCotizacion(m_drwSolicitudEspecificos.NoCotizacion, strItemCode, drwItemSap.Cantidad, drwItemSap.FreeTxt, g_strImpRepuestos, drwItemSap.PrecioAcordado, drwItemSap.Currency, strItemCodeEspecifico, strItemNameEspecifico, intIDMecanico, strNombreMecanico)


                            End If

                            drwItemSap.LineNum = intLineNum

                            With drwRepuesto

                                .NoOrden = m_drwSolicitudEspecificos.NoOrden
                                .NoRepuesto = strItemCode
                                .Cantidad = drwItemSap.Cantidad
                                .Adicional = 1
                                .TipoArticulo = 1 'Repuesto
                                .LineNum = intLineNum
                                .RespondidoPor = G_strUsuarioAplicacion
                                If Not m_blnAgregarACompañiaActual Then
                                    .ItemCodeEspecifico = strItemCodeEspecifico
                                    .ItemNameEspecifico = strItemNameEspecifico
                                End If

                            End With

                            Call dtbRepuestosxOrden.AddSCGTA_TB_RepuestosxOrdenRow(drwRepuesto)

                        Else
                            blnRepuestosAgregados = False
                            MessageBox.Show(My.Resources.ResourceUI.MensajeDebeAgregarEspecificosdeLista)
                            Exit For

                        End If
                    End If

                Next drwItemSap


                MetodosCompartidosSBOCls.ActualizarCotizacion()
                Return blnRepuestosAgregados

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
                Return False
            End Try
        End Function

        Private Function ActualizaRepuestosxOrdenenBD(ByVal dstRepuestos As RepuestosxOrdenDataset, _
                                                      ByRef cnConeccion As SqlClient.SqlConnection, _
                                                      ByRef tnTransaccion As SqlClient.SqlTransaction, _
                                                      ByVal p_intNoCotizacion As Integer) As Boolean
            Try

                Dim adpActRep As New DMSOneFramework.SCGDataAccess.RepuestosxOrdenDataAdapter
                'Dim drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

                If dstRepuestos.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then

                    adpActRep.Update(dstRepuestos.SCGTA_TB_RepuestosxOrden, cnConeccion, tnTransaccion, False, False, True)

                End If


            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                Throw ex
            End Try

        End Function

        Private Function ValidarMonedaItems(ByVal strCurrency As String) As Boolean

            Dim objBLSBO As New BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal = 0
            Dim strMonedaLocal As String = String.Empty

            strMonedaLocal = objBLSBO.RetornarMonedaLocal

            'Valida la Moneda del Item
            If strCurrency <> strMonedaLocal And strCurrency <> "" Then
                decTipoCambio = objBLSBO.RetornarTipoCambioMoneda(strCurrency, Today, strConectionString, True)
            End If

            If decTipoCambio = -1 Then
                MsgBox(My.Resources.ResourceUI.MensajeErrorTipoCambioME + " " + strCurrency + " " + My.Resources.ResourceUI.ParaLaFecha + " " + Today)
                Return False
            End If

            Return True

        End Function

#End Region

#Region "Eventos"

        Private Sub frmSolicitudEspecificos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try

                Call CargarDatos()
                Call CargarDatosCompañia()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 02072010
                If m_cnConeccion IsNot Nothing Then
                    Call m_cnConeccion.Close()
                End If

            End Try

        End Sub

        Private Sub btnCerrar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCerrar.Click

            Try

                Me.Close()

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub dtgDetalles_CellBorderStyleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgDetalles.CellBorderStyleChanged

        End Sub

        Private Sub dtgDetalles_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgDetalles.CellClick

            Try

                If e.ColumnIndex = 10 And dtgDetalles.CurrentRow IsNot Nothing And Not m_dtsSoloLectura Then

                    m_intIDItem = CInt(dtgDetalles.CurrentRow.Cells(0).Value)
                    m_objBuscadorItems = Nothing
                    m_objBuscadorItems = New Buscador.SubBuscador()

                    If m_cnConeccion.State = ConnectionState.Closed Then
                        m_cnConeccion.Open()
                    End If
                    m_objBuscadorItems.SQL_Cnn = m_cnConeccion
                    m_objBuscadorItems.Barra_Titulo = My.Resources.ResourceUI.busBarraTituloItemsEspecificos

                    m_objBuscadorItems.Titulos = My.Resources.ResourceUI.Codigo & "," & My.Resources.ResourceUI.Nombre & _
                    "," & My.Resources.ResourceUI.Stock & "," & My.Resources.ResourceUI.TipoMoneda & "," & My.Resources.ResourceUI.Precio & "," & My.Resources.ResourceUI.CodigoBarras


                    '"Código, Nombre,Stock,Precio"
                    m_objBuscadorItems.Criterios = m_strCampos
                    m_objBuscadorItems.Tabla = m_strTabla
                    m_objBuscadorItems.Where = m_strWhere
                    'm_objBuscadorItems.Top = 500
                    m_objBuscadorItems.ConsultarDBPorFiltrado = True
                    m_objBuscadorItems.Activar_Buscador(sender)
                    'ElseIf e.ColumnIndex = 10 And dtgDetalles.CurrentRow IsNot Nothing And Not m_dtsSoloLectura Then

                    '    Dim drwItems As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow

                    '    m_intIDItem = CInt(dtgDetalles.CurrentRow.Cells(0).Value)

                    '    drwItems = m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.FindByID(m_intIDItem)
                    '    If drwItems IsNot Nothing Then

                    '        If drwItems.SinExistencia = True Then
                    '            If String.IsNullOrEmpty(drwItems.Observaciones) Then

                    '                drwItems.Observaciones = "No hay existencias disponibles"

                    '            Else

                    '                drwItems.Observaciones = drwItems.Observaciones & " (No hay existencias disponibles)"

                    '            End If
                    '        End If

                    '    End If

                Else

                    m_intIDItem = 0

                End If

                'Para la columna "Nuevo" en los itemsEspecificos
                If e.ColumnIndex = 12 And dtgDetalles.CurrentRow IsNot Nothing And Not m_dtsSoloLectura Then

                    cb = TryCast(Me.dtgDetalles.CurrentCell, DataGridViewCheckBoxCell)

                    If cb IsNot Nothing Then
                        Dim bln As Boolean = CBool(cb.Value)
                        If bln = True Then
                            dtgDetalles.CurrentRow.Cells(11).Value = 0

                            dtgDetalles.CurrentRow.Cells(6).ReadOnly = True
                            dtgDetalles.CurrentRow.Cells(7).ReadOnly = True

                            blnArticuloNuevo = False
                        Else
                            dtgDetalles.CurrentRow.Cells(11).Value = 1
                            dtgDetalles.CurrentRow.Cells(6).ReadOnly = False
                            dtgDetalles.CurrentRow.Cells(7).ReadOnly = False

                            blnArticuloNuevo = True
                        End If

                        Dim drwItems As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow

                        m_intIDItem = CInt(dtgDetalles.CurrentRow.Cells(0).Value)

                        drwItems = m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.FindByID(m_intIDItem)
                        If drwItems IsNot Nothing Then

                            drwItems.FreeTxt = My.Resources.ResourceUI.MensajeArticuloNuevo

                        End If

                    End If

                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub m_objBuscadorItems_AppAceptar(ByVal Campo_Llave As String, ByVal Arreglo_Campos As System.Collections.ArrayList, ByVal sender As Object) Handles m_objBuscadorItems.AppAceptar

            Try
                Dim dcPrecioAcordado As Decimal = 0
                Dim dcPrecioTotal As Decimal = 0
                Dim dcPrecioAnterior As Decimal = 0
                Dim dcCantidad As Decimal = 0
               
                Dim drwItems As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow
                drwItems = m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.FindByID(m_intIDItem)

                If blPrimeravez = True Then

                    'Hago un unico select para traerme todo lo relacionado al documento, monedas, fechas, etc
                    strMonedaLocal = Utilitarios.ObtenerMonedaLocal()
                    strMonedaSistema = Utilitarios.ObtenerMonedaSistema()

                    strTipoCambioCotizacion = Utilitarios.EjecutarConsulta(
                                                             String.Format("select DocRate from SCGTA_VW_OQUT with(nolock) where DocEntry = '{0}'",
                                                                             m_drwSolicitudEspecificos.NoCotizacion.ToString.Trim()),
                                                             strConexionADO)
                    If Not String.IsNullOrEmpty(strTipoCambioCotizacion) Then decTipoCambioCotizacion = Decimal.Parse(strTipoCambioCotizacion)

                    strFechaCotizacion = Utilitarios.EjecutarConsulta(
                                                            String.Format("select DocDate from SCGTA_VW_OQUT with(nolock) where DocEntry = '{0}'",
                                                                            m_drwSolicitudEspecificos.NoCotizacion.ToString.Trim()),
                                                            strConexionADO)
                    strFechaCotizacion = Utilitarios.RetornaFechaFormatoRegional(strFechaCotizacion.ToString.Trim())


                    strTipoCambioMS = Utilitarios.EjecutarConsulta(
                                                            String.Format("select Rate from SCGTA_VW_ORTT with(nolock) where RateDate = '{0}' and Currency = '{1}'",
                                                                           strFechaCotizacion.ToString.Trim(), strMonedaSistema.ToString.Trim()),
                                                           strConexionADO)
                    If Not String.IsNullOrEmpty(strTipoCambioMS) Then decTipoCambioMS = Decimal.Parse(strTipoCambioMS)


                    strCurrencyDocumento = m_drwSolicitudEspecificos.DocCur

                    blPrimeravez = False
                End If

                If drwItems IsNot Nothing Then

                    drwItems.CodEspecifico = Arreglo_Campos(0)
                    drwItems.NomEspecifico = Arreglo_Campos(1)

                    'Me indica la moneda que tiene el item antes de cambiarlo por otro item -- esto para sumar o restar el total
                    strMonedaAnterior = dtgDetalles.Item("Currency", dtgDetalles.CurrentRow.Index).Value.ToString()
                    If String.IsNullOrEmpty(strMonedaAnterior) Then
                        strMonedaAnterior = strCurrencyDocumento
                    End If

                    'Asigno la nueva moneda del item seleccionado
                    drwItems.Currency = Arreglo_Campos(3)

                    'Asigno los precios. Nuevo y Viejo
                    dcPrecioAnterior = IIf(String.IsNullOrEmpty(drwItems.PrecioAcordado), 0, Convert.ToDecimal(drwItems.PrecioAcordado))
                    drwItems.PrecioAcordado = IIf(Not String.IsNullOrEmpty(Arreglo_Campos(4)), Arreglo_Campos(4), 0)

                    'Proceso para calcular el precio nuevo y asi sumarlo al total
                    dcPrecioAcordado = Utilitarios.ManejoMultimonedaPrecios(drwItems.PrecioAcordado,
                                                         strCurrencyDocumento,
                                                         decTipoCambioCotizacion,
                                                         decTipoCambioMS,
                                                         Nothing,
                                                         strFechaCotizacion,
                                                         strMonedaLocal,
                                                         strMonedaSistema,
                                                         drwItems.Currency)
                    If dcPrecioAcordado = -2 Then
                        MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + strMonedaSistema + My.Resources.ResourceUI.ParaLaFecha + strFechaCotizacion)
                        Me.Close()
                        Exit Sub
                    End If
                    If dcPrecioAcordado = -4 Then
                        MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + drwItems.Currency + My.Resources.ResourceUI.ParaLaFecha + strFechaCotizacion)
                        Me.Close()
                        Exit Sub
                    End If

                    'Proceso para calcular el precio anterior y asi restarlo al total
                    dcPrecioAnterior = Utilitarios.ManejoMultimonedaPrecios(dcPrecioAnterior,
                                                         strCurrencyDocumento,
                                                         decTipoCambioCotizacion,
                                                         decTipoCambioMS,
                                                         Nothing,
                                                         strFechaCotizacion,
                                                         strMonedaLocal,
                                                         strMonedaSistema,
                                                         strMonedaAnterior)

                    If dcPrecioAnterior = -2 Then
                        MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + strMonedaSistema + My.Resources.ResourceUI.ParaLaFecha + strFechaCotizacion)
                        Me.Close()
                        Exit Sub
                    End If
                    If dcPrecioAnterior = -4 Then
                        MetodosCompartidosSBOCls.FinalizaTransaccion(MetodosCompartidosSBOCls.EstadoDeTransaccion.Rollback)
                        objSCGMSGBox.msgExclamationCustom(My.Resources.ResourceUI.MensajeErrorTipoCambioME + drwItems.Currency + My.Resources.ResourceUI.ParaLaFecha + strFechaCotizacion)
                        Me.Close()
                        Exit Sub
                    End If

                    'Asigno un 0 al Total en el inicio del proceso o bien me traigo la suma que ya estaba contemplada en el texto
                    If String.IsNullOrEmpty(txtTotalRepuestos.Text) Then
                        dcPrecioTotal = 0
                    Else
                        dcPrecioTotal = Convert.ToDecimal(txtTotalRepuestos.Text)
                    End If

                    'Multiplica el precio x la cantidad
                    dcCantidad = IIf(String.IsNullOrEmpty(drwItems.Cantidad), 1, Convert.ToDecimal(drwItems.Cantidad))

                    dcPrecioAcordado = dcPrecioAcordado * dcCantidad
                    dcPrecioAnterior = dcPrecioAnterior * dcCantidad

                    'Hago las sumas y restas pertinentes
                    dcPrecioTotal = dcPrecioTotal + dcPrecioAcordado - dcPrecioAnterior

                    'Imprimo el resultado en el txt
                    txtTotalRepuestos.Text = dcPrecioTotal.ToString("n2")

                
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub


        Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click

            Dim adpMensajeria As New MensajeriaSBOTallerDataAdapter
            Dim intID As Integer
            Try
                intID = m_drwSolicitudEspecificos.ID
                m_adpSolicitudEspecificos.Fill(m_dtsSolicitudEspecificos, intID)
                m_drwSolicitudEspecificos = m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.FindByID(intID)
                If m_drwSolicitudEspecificos.Estado = 0 Then
                    If ValiarItemsEspecíficos() Then

                        'valida si el articulo esta marcado como Nuevo
                        'If Not blnArticuloNuevo Then

                        If ResponderSolicitud() Then
                            adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(My.Resources.ResourceUI.MensajeCotizacionActualizada, _
                                    My.Resources.ResourceUI.Actualizada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_drwSolicitudEspecificos.NoOrden)
                            RaiseEvent eSolicitudCreada(m_intIDSolicitud)
                            objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeExitoSolicitud)
                            Me.Close()
                        End If

                    Else

                        Dim resultado = MessageBox.Show(My.Resources.ResourceUI.MensajeUpdateItemEspecifico, My.Resources.ResourceUI.PreguntaDeseaGuardarCambiosFormulario, MessageBoxButtons.YesNoCancel)

                        If resultado = DialogResult.Yes Then
                            Dim tnTransaccion As SqlClient.SqlTransaction = Nothing
                            Dim cnConeccion As SqlClient.SqlConnection = Nothing

                            m_adpItemsSolicitados.UpdateItemEspecifico(m_dtsItemsSolicitados, cnConeccion, tnTransaccion)
                            Me.Close()
                        ElseIf resultado = DialogResult.No Then
                            Me.Close()
                        ElseIf resultado = DialogResult.Cancel Then

                        End If



                        ' MessageBox.Show(My.Resources.ResourceUI.MensajeTodosItemsDebenTenerEspecifico)
                    End If
                Else
                    MessageBox.Show(My.Resources.ResourceUI.MensajeAlguienHaProcesadoSolicitud)
                    Me.Close()
                End If
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImprimir.Click
            Dim strParametros As String = ""
            Dim objBLConexion As DMSOneFramework.SCGDataAccess.DAConexion
            Dim rptorden As New ComponenteCristalReport.SubReportView

            Try

                objBLConexion = New DMSOneFramework.SCGDataAccess.DAConexion

                PATH_REPORTES = objBLConexion.ExtraerPathReportes(strDATABASESCG)
                If txtNoOrden.Text <> "" Then

                    strParametros = txtNoSolicitud.Text.Trim

                    'strParametros = strParametros & txtNoVisita.Text.Trim

                    With rptorden
                        .P_BarraTitulo = My.Resources.ResourceUI.busBarraTituloBuscadorEspecificos
                        .P_WorkFolder = PATH_REPORTES
                        .P_Filename = My.Resources.ResourceUI.rptNombreReporteEspecificos
                        .P_Server = Server
                        .P_DataBase = strDATABASESCG
                        .P_User = UserSCGInternal
                        .P_Password = Password
                        .P_ParArray = strParametros
                    End With

                    rptorden.VerReporte()
                Else
                    objSCGMSGBox.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarOT)
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try
        End Sub

        Private Sub btnCancelarSolicitud_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelarSolicitud.Click
            Dim a_drwSolicitudes As SolicitudEspecificosDataset.SCGTA_SP_SelSolicitudEspecificoRow()
            Dim drwSolicitudes As SolicitudEspecificosDataset.SCGTA_SP_SelSolicitudEspecificoRow
            Dim cnConection As SqlClient.SqlConnection = Nothing
            Dim tnTransation As SqlClient.SqlTransaction = Nothing

            Try

                If MessageBox.Show(My.Resources.ResourceUI.MensajeCancelarSolicitud, G_strCompaniaSCG, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    a_drwSolicitudes = m_dtsSolicitudEspecificos.SCGTA_SP_SelSolicitudEspecifico.Select("Estado = 0")
                    For Each drwSolicitudes In a_drwSolicitudes
                        If drwSolicitudes.Estado = 0 Then
                            drwSolicitudes.Estado = 2
                            drwSolicitudes.RespondidoPor = USUARIO_SISTEMA
                        End If
                    Next
                    m_adpSolicitudEspecificos.Update(m_dtsSolicitudEspecificos, cnConection, tnTransation, False)
                    m_adpItemsSolicitados.Update(m_dtsItemsSolicitados, cnConection, tnTransation, True)
                    Me.Close()
                End If

            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 05072010
                Call cnConection.Close()

            End Try

        End Sub

        Private Sub dtgDetalles_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dtgDetalles.CellValueChanged
            If e.ColumnIndex = 11 And dtgDetalles.CurrentRow IsNot Nothing And Not m_dtsSoloLectura Then
                Dim drwItems As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow

                m_intIDItem = CInt(dtgDetalles.CurrentRow.Cells(0).Value)

                drwItems = m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.FindByID(m_intIDItem)
                If drwItems IsNot Nothing Then

                    If drwItems.SinExistencia = True Then
                        If String.IsNullOrEmpty(drwItems.FreeTxt) Then

                            drwItems.FreeTxt = "No hay existencias disponibles"

                        Else

                            drwItems.FreeTxt = drwItems.FreeTxt & " (No hay existencias disponibles)"

                        End If

                    End If

                End If
            End If

            'If e.ColumnIndex = 11 And dtgDetalles.CurrentRow IsNot Nothing And Not m_dtsSoloLectura Then

            '    Dim cb As DataGridViewCheckBoxCell = TryCast(Me.dtgDetalles.CurrentCell, DataGridViewCheckBoxCell)

            '    If cb IsNot Nothing Then
            '        ' Confirmammos los cambios efectuados en la celda actual
            '        '
            '        dtgDetalles.CommitEdit(DataGridViewDataErrorContexts.Commit)
            '    End If


            'End If

        End Sub

#End Region



        Private Sub dtgDetalles_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtgDetalles.CurrentCellDirtyStateChanged
            ' Referenciamos el control DataGridViewCheckBoxCell actual
            '

            Dim cb As DataGridViewCheckBoxCell = TryCast(Me.dtgDetalles.CurrentCell, DataGridViewCheckBoxCell)

            If cb IsNot Nothing Then
                ' Confirmammos los cambios efectuados en la celda actual
                '
                dtgDetalles.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If


        End Sub

        Private Function ResponderSolicitudItemNuevo() As Boolean

            Dim dstRepuestosxOrden As New RepuestosxOrdenDataset
            Dim dtsRepuestosxOrdenAnterior As New RepuestosxOrdenDataset
            Dim cnConeccion As SqlClient.SqlConnection = Nothing
            Dim tnTransaccion As SqlClient.SqlTransaction = Nothing
            Dim blnResultado As Boolean = True
            Dim adpRep As New RepuestosxOrdenDataAdapter
            Dim drwItemSolicitud As ItemSolicitudEspecificoDataset.SCGTA_SP_SelItemSolicitudEspecificoRow
            Dim drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
            Dim intFilasModificadas As Integer = 0

            Try

                adpRep.Fill(dtsRepuestosxOrdenAnterior, m_drwSolicitudEspecificos.NoOrden)
                If m_blnAgregarACompañiaActual Then
                    For Each drwItemSolicitud In m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.Rows
                        If drwItemSolicitud.LineNum > -1 Then
                            For Each drwRepuestos In dtsRepuestosxOrdenAnterior.SCGTA_TB_RepuestosxOrden.Rows
                                If drwRepuestos.RowState <> DataRowState.Deleted Then
                                    If drwRepuestos.LineNum = drwItemSolicitud.LineNum Then
                                        'MetodosCompartidosSBOCls.EliminarItemCotizacion(drwRepuestos.LineNum, "El ítem se cambió por " & drwItemSolicitud.CodEspecifico)
                                        'drwRepuestos.Delete()
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Next
                Else
                    For Each drwItemSolicitud In m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico.Rows
                        If drwItemSolicitud.LineNum > -1 Then
                            For Each drwRepuestos In dtsRepuestosxOrdenAnterior.SCGTA_TB_RepuestosxOrden.Rows
                                If drwRepuestos.RowState <> DataRowState.Deleted Then
                                    If drwRepuestos.LineNum = drwItemSolicitud.LineNum Then
                                        intFilasModificadas += 1
                                        drwRepuestos.ItemCodeEspecifico = drwItemSolicitud.CodEspecifico
                                        drwRepuestos.ItemNameEspecifico = drwItemSolicitud.NomEspecifico
                                        drwRepuestos.PrecioAcordado = drwItemSolicitud.PrecioAcordado
                                        drwRepuestos.Cantidad = drwItemSolicitud.Cantidad
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Next

                End If
                'If intFilasModificadas = 0 Then
                '    blnResultado = GuardaRepuestos(dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, _
                '                                       m_dtsItemsSolicitados.SCGTA_SP_SelItemSolicitudEspecifico)
                'End If
                If blnResultado Then

                    m_drwSolicitudEspecificos.RespondidoPor = G_strUsuarioAplicacion
                    m_drwSolicitudEspecificos.Estado = 1

                    m_adpSolicitudEspecificos.Update(m_dtsSolicitudEspecificos, cnConeccion, tnTransaccion, False)

                    m_adpItemsSolicitados.Update(m_dtsItemsSolicitados, cnConeccion, tnTransaccion)

                    cnConeccion.Close()

                End If

                Return blnResultado
            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                If cnConeccion.State = ConnectionState.Open Then
                    tnTransaccion.Rollback()
                    cnConeccion.Close()
                End If
                Throw ex

            Finally
                'Agregado 05072010
                Call cnConeccion.Close()

            End Try
        End Function

    End Class


End Namespace