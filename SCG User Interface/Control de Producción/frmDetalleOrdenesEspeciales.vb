
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface

    Public Class frmDetalleOrdenesEspeciales

#Region "Declaraciones"

        Private m_drdTiposOTEspeciales As SqlClient.SqlDataReader
        Private m_adpConfOrdenesEspeciales As New ConfOTsEspecialesDataAdapter

        Private m_objCotizacion As New CotizacionCLS(G_objCompany)
        Private m_objUtilitarios As New Utilitarios(strConexionADO)

        Private m_adpItemsCotizacion As New QUT1DataAdapter

        'Adapter para la tabla de lineas de SolicitudOTEspecial
        Private m_adpLineasSolicitudOTEspecial As New LineasSolicitudOTEspecialDataAdapter

        Private m_intNoCotizacion As Integer
        Private m_intNoCotizacionPadre As Integer

        Private m_drwOrdenPadre As OrdenTrabajoDataset.SCGTA_TB_OrdenRow

        Private m_objMensajeria As New Proyecto_SCGMSGBox.SCGMSGBox

        Private m_adpMensajeria As New MensajeriaSBOTallerDataAdapter

        Private m_intCodTipoOrden As Integer
        Private m_intCodAsesor As Integer
        Private m_strCardCodeCliente As String
        Private m_strCardCodeClienteOriginal As String
        Private m_strCardNameClienteOriginal As String
        Private m_strValidaRepPendientes As String

        Public Event eOrdenGenerada(ByVal p_intNoCotizacion As Integer)

#End Region

#Region "Constructor"


        Public Sub New(ByVal p_intNoCotizacion As Integer, _
                       ByVal p_drwOrdenPadre As OrdenTrabajoDataset.SCGTA_TB_OrdenRow, _
                       ByVal p_strCardCodeClienteOriginal As String, _
                       ByVal p_strCardNameClienteOriginal As String, _
                       ByVal p_strValidaRepPendientes As String)

            ' Llamada necesaria para el Diseñador de Windows Forms.
            InitializeComponent()

            ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
            m_intNoCotizacionPadre = p_intNoCotizacion
            m_drwOrdenPadre = p_drwOrdenPadre
            m_strCardCodeClienteOriginal = p_strCardCodeClienteOriginal
            m_strCardNameClienteOriginal = p_strCardNameClienteOriginal
            m_strValidaRepPendientes = p_strValidaRepPendientes

        End Sub

#End Region

#Region "Métodos"

        Private Sub CrearOTEspecial()

            Dim dtsConfOrdenesEspeciales As New ConfOrdenesEspeciales
            Dim drwConfOrdenesEspeciales As ConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspecialesRow
            Dim strSerieDocumento As String = ""
            Dim UsaPreciosClientes As Boolean = False
            Dim UsaSolicitudOTEspecial As String = String.Empty
            Dim blnSolicitudOT As Boolean = False
            Dim strNombreTipoOT As String = String.Empty


            ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "IDSerieDocumentosCotizaciones", strSerieDocumento)
            ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(g_dstConfiguracion.SCGTA_TB_Configuracion, "UsaSolicitudOTEspecial", UsaSolicitudOTEspecial)

            If UsaSolicitudOTEspecial = "1" Then
                blnSolicitudOT = True
            End If

            m_objCotizacion.ImpuestoRepuestos = g_strImpRepuestos
            m_objCotizacion.ImpuestoServicios = g_strImpServicios
            m_objCotizacion.ImpuestoSuministros = g_strImpSuministros
            m_objCotizacion.ImpuestoServiciosExternos = g_strImpServiciosExternos
            If strSerieDocumento <> "" Then
                m_objCotizacion.SerieDocumento = strSerieDocumento
            End If

            If DeterminarLineasAIncluir() Then

                m_adpConfOrdenesEspeciales.Fill(dtsConfOrdenesEspeciales, CInt(cboTipoOrden.SelectedValue))
                If dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.Rows.Count = 1 Then
                    drwConfOrdenesEspeciales = dtsConfOrdenesEspeciales.SCGTA_TB_ConfOrdenesEspeciales.Rows(0)
                    If drwConfOrdenesEspeciales.IDAsesor <> -1 Then
                        m_intCodAsesor = drwConfOrdenesEspeciales.IDAsesor
                    Else
                        m_intCodAsesor = CInt(m_drwOrdenPadre.Asesor)
                    End If
                    m_intCodTipoOrden = cboTipoOrden.SelectedValue
                    If drwConfOrdenesEspeciales.IsCardCodeClienteNull Then
                        m_strCardCodeCliente = m_strCardCodeClienteOriginal
                    Else
                        If drwConfOrdenesEspeciales.CardCodeCliente <> "" Then
                            m_strCardCodeCliente = drwConfOrdenesEspeciales.CardCodeCliente
                            m_strCardNameClienteOriginal = ""

                            If drwConfOrdenesEspeciales.UsaListaPrecios Then
                                UsaPreciosClientes = True
                            End If

                        Else
                            m_strCardCodeCliente = m_strCardCodeClienteOriginal
                        End If
                    End If


                    If Not blnSolicitudOT Then

                        m_objCotizacion.IniciarProceso()
                        m_objCotizacion.UsaListaPreciosCliente = UsaPreciosClientes
                        m_intNoCotizacion = m_objCotizacion.ManejarCotizacion(m_intCodTipoOrden, m_strCardCodeCliente, m_intCodAsesor, m_strCardCodeClienteOriginal, m_strCardNameClienteOriginal, m_drwOrdenPadre, m_dtsOQUT1, strSerieDocumento)
                        m_objCotizacion.FinalizarProceso()

                        'actualiza los IdRepuestosxOrden en la nueva cotizacion Especial
                        m_objCotizacion.ActualizarIdLineasHijasPaquetes(m_intNoCotizacion)

                        Call EnviarMensajes()
                        m_objMensajeria.msgInformationCustom(My.Resources.ResourceUI.MensajeProcesoSatisfactorio)
                        RaiseEvent eOrdenGenerada(m_intNoCotizacion)
                    Else

                        strNombreTipoOT = Utilitarios.EjecutarConsulta("SELECT Descripcion FROM dbo.[SCGTA_TB_TipoOrden] WHERE (EstadoLogico = 1) AND (CodTipoOrden = " & m_intCodTipoOrden & ")", strConectionString)

                        m_objCotizacion.UsaListaPreciosCliente = UsaPreciosClientes

                        m_intNoCotizacion = m_objCotizacion.ManejarSolicitudOTEspecial(m_intCodTipoOrden, m_strCardCodeCliente, m_intCodAsesor, m_strCardCodeClienteOriginal, m_strCardNameClienteOriginal, m_drwOrdenPadre, m_dtsOQUT1, strSerieDocumento, m_drwOrdenPadre.NombreAsesor, strNombreTipoOT)


                            If m_intNoCotizacion <> 0 Then
                                Call EnviarMensajes(True)
                                m_objMensajeria.msgInformationCustom(My.Resources.ResourceUI.MensajeSolicitudSatisfactoria)
                                RaiseEvent eOrdenGenerada(m_intNoCotizacion)
                            End If


                        End If

                Else

                        m_objMensajeria.msgInformationCustom(My.Resources.ResourceUI.MensajeProblemaConfigTipoOT)

                End If
            Else

                m_objMensajeria.msgInformationCustom(My.Resources.ResourceUI.MensajeDebeSeleccionarAlgunaLinea)

            End If

        End Sub

       


        Private Function DeterminarLineasAIncluir() As Boolean

            Dim drwItemsCotizacion As QUT1Dataset.QUT1Row
            Dim blnResultado As Boolean
            Dim intCantidadInicial As Integer
            Dim intCantidadEliminadas As Integer

            intCantidadInicial = m_dtsOQUT1.QUT1.Rows.Count
            intCantidadEliminadas = 0

            For Each drwItemsCotizacion In m_dtsOQUT1.QUT1.Rows

                If Not drwItemsCotizacion.Check Then

                    drwItemsCotizacion.Delete()
                    intCantidadEliminadas += 1
                Else

                    drwItemsCotizacion.LineNum = -1

                End If

            Next

            If intCantidadInicial <> intCantidadEliminadas Then

                m_dtsOQUT1.AcceptChanges()
                blnResultado = True
            Else

                m_dtsOQUT1.RejectChanges()
                blnResultado = False

            End If

            Return blnResultado

        End Function

        Private Sub EnviarMensajes(Optional ByVal blnSolicitud As Boolean = False)

            Dim drdUsuariosParaEnviarMensajes As SqlClient.SqlDataReader = Nothing
            Dim adpUsuarios As New UsuariosOTEspecialDataAdapter
            Dim intCodConfiguracion As Integer
            Dim strTipoOrden As String

            Try

                If cboTipoOrden.Text.Length > 30 Then
                    strTipoOrden = cboTipoOrden.Text.Substring(0, 30)
                Else
                    strTipoOrden = cboTipoOrden.Text
                End If


                If Not blnSolicitud Then

                    Dim mensaj As String = String.Format(My.Resources.ResourceUI.MensajeOrdenTipo, strTipoOrden)

                    'Mensaje para el asesor
                    m_adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(mensaj & " " & My.Resources.ResourceUI.MensajeSolicitada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_intNoCotizacion)

                    'Mensajes para los otros usuarios
                    m_adpConfOrdenesEspeciales.Fill(m_drdTiposOTEspeciales, cboTipoOrden.SelectedValue)
                    Do While m_drdTiposOTEspeciales.Read

                        intCodConfiguracion = m_drdTiposOTEspeciales.GetInt32(2)

                    Loop
                    adpUsuarios.Fill(drdUsuariosParaEnviarMensajes, intCodConfiguracion)

                    Do While drdUsuariosParaEnviarMensajes.Read
                        m_adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(mensaj & " " & My.Resources.ResourceUI.MensajeSolicitada, drdUsuariosParaEnviarMensajes.GetString(2), m_intNoCotizacion)
                    Loop

                Else
                    Dim MensajeSolicitud As String = String.Empty

                    MensajeSolicitud = String.Format(My.Resources.ResourceUI.MensajeOrdenTipo, strTipoOrden)

                    Dim mensaj As String = My.Resources.ResourceUI.MensajeSolicitudOTEspecial & " " & MensajeSolicitud
                    'Mensaje para el asesor
                    m_adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(mensaj & " " & My.Resources.ResourceUI.MensajeSolicitada, MensajeriaSBOTallerDataAdapter.RecibeMensaje.Asesor, m_intNoCotizacion, True)

                    'Mensajes para los otros usuarios
                    m_adpConfOrdenesEspeciales.Fill(m_drdTiposOTEspeciales, cboTipoOrden.SelectedValue)
                    Do While m_drdTiposOTEspeciales.Read

                        intCodConfiguracion = m_drdTiposOTEspeciales.GetInt32(2)

                    Loop
                    adpUsuarios.Fill(drdUsuariosParaEnviarMensajes, intCodConfiguracion)

                    Do While drdUsuariosParaEnviarMensajes.Read
                        m_adpMensajeria.CreaMensajeDMS_SBO_Cotizacion(mensaj & " " & My.Resources.ResourceUI.MensajeSolicitada, drdUsuariosParaEnviarMensajes.GetString(2), m_intNoCotizacion)
                    Loop


                End If
              
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)

                Throw
            Finally
                'Agregado 01072010
                If m_drdTiposOTEspeciales IsNot Nothing Then
                    If Not m_drdTiposOTEspeciales.IsClosed Then
                        Call m_drdTiposOTEspeciales.Close()
                    End If
                End If

                'Agregado 02072010
                If drdUsuariosParaEnviarMensajes IsNot Nothing Then
                    If Not drdUsuariosParaEnviarMensajes.IsClosed Then
                        Call drdUsuariosParaEnviarMensajes.Close()
                    End If
                End If
            End Try

        End Sub

#End Region

#Region "Eventos"

        Private Sub frmDetalleOrdenesEspeciales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Try
                m_adpItemsCotizacion.Fill(m_dtsOQUT1, m_intNoCotizacionPadre, m_strValidaRepPendientes)
                m_adpConfOrdenesEspeciales.Fill(m_drdTiposOTEspeciales)

                Utilitarios.CargarComboSourceByReader(cboTipoOrden, m_drdTiposOTEspeciales)

                If Not m_drdTiposOTEspeciales.IsClosed Then

                    m_drdTiposOTEspeciales.Close()

                End If

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            Finally
                'Agregado 01072010
                If m_drdTiposOTEspeciales IsNot Nothing Then
                    If Not m_drdTiposOTEspeciales.IsClosed Then
                        Call m_drdTiposOTEspeciales.Close()
                    End If
                End If
            End Try

        End Sub

        Private Function ValidarDatosSAP() As Boolean

            'Valida que el tipo de cambio y el periodo fiscal sean validos antes de realizar calculo de costos

            Dim blnValido As Boolean = True
            Dim objBLSBO As BLSBO.GlobalFunctionsSBO
            Dim decTipoCambio As Decimal
            Dim strMonedaLocal As String
            Dim strMonedaSistema As String


            Try

                objBLSBO = New BLSBO.GlobalFunctionsSBO

                strMonedaSistema = objBLSBO.RetornarMonedaSistema
                strMonedaLocal = objBLSBO.RetornarMonedaLocal
                If strMonedaSistema <> strMonedaLocal Then
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

        Private Sub btnCrear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCrear.Click

            Try
                If ValidarDatosSAP() Then
                    Call CrearOTEspecial()
                End If
            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

        Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click

            Try

                Me.Close()

            Catch ex As Exception

                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)

            End Try

        End Sub

#End Region

    End Class

End Namespace