Imports DMSOneFramework.SCGCommon

Namespace SCG_User_Interface
    Public Module GlobalesUI

        Public objSCGMSGBox As New Proyecto_SCGMSGBox.SCGMSGBox(gc_strAplicacion)

#Region "Declaraciones"
        'Public clsExceptionHandler As New SCGExceptionHandler.clsExceptionHandler
        'Public continuar As Integer
        'Public G_blnConexion As Boolean
        'Public G_strUser As String
        'Public G_strCompaniaSCG As String
        'Public Const gc_strAplicacion As String = "SCG Sistema Taller"
        Public g_strCOMPANIA As String
        'Public User As String
        'Public Password As String
        'Public UserSCGInternal As String
        'Public PasswordSCGInternal As String
        'Public Server As String
        'Public ServerLicense As String
        'Public strDATABASE As String
        'Public strDATABASESCG As String
        'Public PATH_REPORTES As String
        'Public G_intNoRazon As Integer
        'Public g_blnMixitActivado As Boolean = False

#End Region

#Region "Declaraciones"

        Public continuar As Integer
        Public G_blnConexion As Boolean
        Public G_strUser As String
        Public G_strCompaniaSCG As String

        'Public G_strIDBodegaRep As String
        'Public G_strIDBodegaSum As String
        'Public G_strIDBodegaSer As String
        Public G_strIDSucursal As String
        Public G_strIDConfig As String
        Public G_strNombreSucursal As String
        Public G_strUsuarioAplicacion As String
        Public Const gc_strAplicacion As String = "SCG DMS One"
        Public COMPANIA As String
        Public User As String
        Public Password As String
        Public UserSCGInternal As String
        Public PasswordSCGInternal As String
        Public Server As String
        Public ServerLicense As String
        Public strDATABASE As String
        Public strDATABASESCG As String
        Public PATH_REPORTES As String
        Public G_intNoRazon As Integer
        Public g_blnMixitActivado As Boolean = False
        Public g_AgregaAdicionales As Boolean

        Public g_TipoSkin As Integer

        'Variables de configuración del Servidor de Correo
        Public g_strServidordeCorreo As String
        Public g_strDirEnviaCorreo As String
        Public g_strUsuarioSMTP As String
        Public g_strPasswordSMTP As String
        Public g_strPuerto As String
        Public g_chkUsaSSL As Boolean

        'Variable de cnfiguracion Modifica Precio
        Public g_blnModificaPrecio As Boolean
        Public g_blnValidaEstadoOTPadre As Boolean

        Public g_blnUsaRepuestos As Boolean
        Public g_blnUsaServicios As Boolean
        Public g_blnUsaServiciosExternos As Boolean
        Public g_blnUsaSuministros As Boolean
        Public g_blnUsaOtrosGastos As Boolean
        Public g_strCuentaContableAcre As String

        '********************************************************************************************
        'Agregado 29/02/2012: Agregar configuración validación de tiempo estándar
        'Autor: José Soto
        Public g_blnUsaValTiempoEs As Boolean

        Public g_blnUsaValFiltClient As Boolean
        '********************************************************************************************

        '
        'Mensajeria por centro de costo
        Public g_blnUsaMensajeriaXCentroCosto As Boolean

        Public g_blnGeneraOTsEspeciales As Boolean
        Public g_blnCosteaActividades As Boolean


        Public g_strImpRepuestos As String
        Public g_strImpServicios As String
        Public g_strImpServiciosExternos As String
        Public g_strImpSuministros As String
        Public g_strEncagadoCompras As String

        'Agregado 01/11/2010: Guarda el encargado de accesorios
        Public g_strEncargadoAcc As String

        Public g_strDireccionB2B As String
        Public g_blnCatalogosExternos As Boolean

        Public g_dstConfiguracion As DMSOneFramework.ConfiguracionDataSet
        Public g_adpConfiguracion As ConfiguracionDataAdapter
      
        'Public clsExceptionHandler As New SCGExceptionHandler.clsExceptionHandler
        Public g_intUnidadTiempo As Integer

        Public g_blnServiciosExternosInventariables As Boolean
        Public g_intCosteoServicios As Integer

        Public g_strTablaArchivosDigitales As String = "SCGTA_Archivos"

        Public g_blnCampoVisible As Boolean = False

        'Variables de Configuracion para el DMS Web
        Public g_blnVerOTCodigos As Boolean
        Public g_blnVerOTTotales As Boolean


#End Region

#Region "Procedimientos"

     

        Friend Function CargarAsociacionExpImg(ByVal intNoVisita As Integer, ByVal intTipo As Integer)
            Dim objExp_ImgDA As New DMSOneFramework.SCGDataAccess.Expediente_ImagenesDataAdapter
            Dim dtsExp_Img As New DMSOneFramework.Visita_ImagenesDataset

            objExp_ImgDA.Fill(dtsExp_Img, intNoVisita, intTipo)

            With dtsExp_Img
                If dtsExp_Img.SCGTA_TB_Exped_Img.Rows.Count <> 0 Then
                    Return dtsExp_Img.SCGTA_TB_Exped_Img(0).ID
                End If
            End With

            Return 0
        End Function

        Public Sub Carga_Combo_Meses_Culure(ByRef Combo As SCGComboBox.SCGComboBox)
            'Dim objCulture As Globalization.CultureInfo
            Dim strMonthName As String

            Combo.Items.Clear()

            For Each strMonthName In Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                If strMonthName.Trim <> "" Then
                    Combo.Items.Add(CType(strMonthName, String))
                End If
            Next

            Combo.SelectedIndex = Today.Month - 1
        End Sub

        Public Sub Carga_Combo_Anios_Culure(ByRef Combo As SCGComboBox.SCGComboBox)
            'Dim objCulture As Globalization.CultureInfo
            Dim intYear As Integer
            Dim MinYear As Integer = Today.Year - 50
            Dim MaxYear As Integer = Today.Year + 50
            Dim intIndexSelec As Integer = 0
            Dim intIndexCont As Integer = 0

            Combo.Items.Clear()

            For intYear = MaxYear To MinYear Step -1
                Combo.Items.Add(CStr(intYear))
                If intYear = Today.Year Then
                    intIndexSelec = intIndexCont
                End If
                intIndexCont += 1
            Next

            Combo.SelectedIndex = intIndexSelec
        End Sub

        Public Sub Busca_Item_Combo(ByRef Combo As ComboBox, ByVal Cod_Item As String)

            Dim intItemCont As Integer
            Dim strTempItem As String
            Dim strCod_Item_Comp As String
            Dim blnExiste As Boolean
            Try


                With Combo

                    If .Items.Count <> 0 Then
                        blnExiste = False
                        For intItemCont = 0 To .Items.Count - 1
                            strTempItem = .Items(intItemCont)
                            strCod_Item_Comp = Busca_Codigo_Texto(strTempItem)
                            If Cod_Item = strCod_Item_Comp Then
                                blnExiste = True
                                Exit For
                            End If
                        Next
                        If blnExiste Then
                            .SelectedIndex = intItemCont
                        End If
                    End If

                End With
            Catch ex As Exception
                Call ManejoErrores(ex, COMPANIA, GlobalesUI.g_TipoSkin)
                'SCGExceptionHandler.clsExceptionHandler.handException(ex, Application.StartupPath, gc_strAplicacion)
            End Try

        End Sub

        Public Function Busca_Codigo_Texto(ByVal strTempItem As String, Optional ByVal blnGetCodigo As Boolean = True) As String

            Dim strCod_Item_Comp As String
            Dim strTemp As String
            Dim intCharCont As Integer
            Dim strTextoNoCodigo As String = ""

            strTemp = ""
            strCod_Item_Comp = ""

            If strTempItem <> "" Then

                For intCharCont = strTempItem.Length - 1 To 0 Step -1
                    If Char.IsWhiteSpace(strTempItem.Chars(intCharCont)) Then
                        Exit For
                    End If
                    strTemp = strTemp & strTempItem.Chars(intCharCont)
                Next

                If strTempItem.Length > 0 Then
                    strTextoNoCodigo = strTempItem.Substring(0, strTempItem.Length - (strTempItem.Length - intCharCont)).Trim
                End If
                For intCharCont = strTemp.Length - 1 To 0 Step -1
                    strCod_Item_Comp = strCod_Item_Comp & strTemp.Chars(intCharCont)
                Next

                If blnGetCodigo Then
                    Return strCod_Item_Comp
                Else
                    Return strTextoNoCodigo
                End If
            Else
                Return ""
            End If

        End Function

        Public Sub G_LimpiarCollect_LateBinding(ByRef p_Controls As Object)
            Dim ctrlActual As Control

            For Each ctrlActual In p_Controls
                If ctrlActual.Controls.Count <> 0 Then
                    G_LimpiarCollect_LateBinding(ctrlActual.Controls)
                Else
                    If TypeOf (ctrlActual) Is NEWTEXTBOX.NEWTEXTBOX_CTRL Then

                        If CType(ctrlActual, NEWTEXTBOX.NEWTEXTBOX_CTRL).Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.Numeric _
                            Or CType(ctrlActual, NEWTEXTBOX.NEWTEXTBOX_CTRL).Tipo_TextBox = NEWTEXTBOX.NEWTEXTBOX_CTRL.Tipo_Text.NumDecimal Then
                            CType(ctrlActual, NEWTEXTBOX.NEWTEXTBOX_CTRL).Text = "0"
                        Else
                            CType(ctrlActual, NEWTEXTBOX.NEWTEXTBOX_CTRL).Text = ""
                        End If

                    End If
                End If
            Next
        End Sub

        Public Sub G_CancelarEditColumnDataGrid(ByRef p_frm As Form, ByRef p_dtgEdited As Windows.Forms.DataGrid)

            Dim gridCurrencyManager As CurrencyManager 

            If p_dtgEdited IsNot Nothing Then
                If p_dtgEdited.DataSource IsNot Nothing Then

                    gridCurrencyManager = CType(p_frm.BindingContext(p_dtgEdited.DataSource, _
                    p_dtgEdited.DataMember), CurrencyManager)

                    With p_dtgEdited

                        If .TableStyles(0).GridColumnStyles(.CurrentCell.ColumnNumber).GetType Is GetType(DataGridCheckColumn) Then

                            gridCurrencyManager.CancelCurrentEdit()

                        End If

                    End With
                End If
            End If

        End Sub

        Public Function CargarEstadoOTResources(ByVal strEstado As String) As String
            Select Case (strEstado.ToLower)
                Case My.Resources.ResourceUI.NoIniciada
                    Return My.Resources.ResourceUI.NoIniciada
                Case My.Resources.ResourceUI.Enproceso
                    Return My.Resources.ResourceUI.Enproceso
                Case My.Resources.ResourceUI.Finalizada
                    Return My.Resources.ResourceUI.Finalizada
                Case My.Resources.ResourceUI.Cancelada
                    Return My.Resources.ResourceUI.Cancelada
                Case My.Resources.ResourceUI.Suspendida
                    Return My.Resources.ResourceUI.Suspendida
                Case Else
                    Return String.Empty
            End Select

        End Function


        Public Sub LlenarEstadoOrdenTrabajoResources(ByVal dstOrdenTrabajo As DMSOneFramework.OrdenTrabajoDataset)
            Dim intFila As Integer

            For intFila = 0 To dstOrdenTrabajo.SCGTA_TB_Orden.Rows.Count - 1

                Select Case dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("Estado")
                    Case 1
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.NoIniciada
                    Case 2
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Enproceso
                    Case 3
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Suspendida
                    Case 4
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Finalizada
                    Case 5
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Cancelada
                    Case 6
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Cerrada
                    Case 7
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Facturada
                    Case 8
                        dstOrdenTrabajo.SCGTA_TB_Orden.Rows(intFila).Item("DescipcionEstado") = My.Resources.ResourceUI.Entregada
                End Select

            Next

        End Sub

        Public Sub LlenarEstadoVisitaResources(ByVal dstVisita As DMSOneFramework.VisitaDataset)
            Dim intFila As Integer

            For intFila = 0 To dstVisita.SCGTA_TB_Visita.Rows.Count - 1

                Select Case (dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("Estado")).ToLower
                    Case "en proceso"
                        dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("DescripcionEstadoVisita") = My.Resources.ResourceUI.Enproceso
                    Case "suspendida"
                        dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("DescripcionEstadoVisita") = My.Resources.ResourceUI.Suspendida
                    Case "suspendido"
                        dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("DescripcionEstadoVisita") = My.Resources.ResourceUI.Suspendido
                    Case "finalizada", "finalizado"
                        dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("DescripcionEstadoVisita") = My.Resources.ResourceUI.Finalizada
                    Case "entregado", "entregada"
                        dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("DescripcionEstadoVisita") = My.Resources.ResourceUI.Entregado
                    Case Else
                        dstVisita.SCGTA_TB_Visita.Rows(intFila).Item("DescripcionEstadoVisita") = ""
                End Select

            Next

        End Sub


        Public Sub LlenarEstadoSolicitudEspecificosResources(ByVal dstSolicitud As DMSOneFramework.SolicitudEspecificosDataset)
            Dim intFila As Integer

            For intFila = 0 To dstSolicitud.SCGTA_SP_SelSolicitudEspecifico.Rows.Count - 1
                Select Case (dstSolicitud.SCGTA_SP_SelSolicitudEspecifico.Rows(intFila).Item("DescEstado")).ToLower
                    Case "respondida"
                        dstSolicitud.SCGTA_SP_SelSolicitudEspecifico.Rows(intFila).Item("DescEstadoResources") = My.Resources.ResourceUI.Respondida
                    Case "sin respuesta"
                        dstSolicitud.SCGTA_SP_SelSolicitudEspecifico.Rows(intFila).Item("DescEstadoResources") = My.Resources.ResourceUI.SinResponder
                    Case "cancelada"
                        dstSolicitud.SCGTA_SP_SelSolicitudEspecifico.Rows(intFila).Item("DescEstadoResources") = My.Resources.ResourceUI.Cancelada
                    Case Else
                        dstSolicitud.SCGTA_SP_SelSolicitudEspecifico.Rows(intFila).Item("DescEstadoResources") = ""
                End Select
            Next

        End Sub

        Public Sub LlenarRepuestosXOrdenResources(ByVal dstRepXOrden As DMSOneFramework.RepuestosxOrdenDataset)

            Dim dstEstadoRepuestosDataSet As New DMSOneFramework.EstadoRepuestosResourcesDataset
            Dim adpEstadoRepuestosDataAdapter As New DMSOneFramework.SCGDataAccess.EstadoRepuestosResourcesDataAdapter
            Dim drwFila() As DataRow
            Dim intFila As Integer

            adpEstadoRepuestosDataAdapter.Fill(dstEstadoRepuestosDataSet)

            For intFila = 0 To dstRepXOrden.SCGTA_TB_RepuestosxOrden.Rows.Count - 1  'dstRepXOrden.SCGTA_TB_RepuestosxOrden.Rows.Count - 1
                If System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName = "es" Then
                    drwFila = dstEstadoRepuestosDataSet.SCGTA_TB_EstadoRepuestoResources.Select("codigo = " & dstRepXOrden.SCGTA_TB_RepuestosxOrden.Rows(intFila).Item("CodEstadoRep") & " and Cultura = ''")
                    dstRepXOrden.SCGTA_TB_RepuestosxOrden.Rows(intFila).Item("DescEstadoResources") = drwFila(0)("Descripcion")
                Else
                    drwFila = dstEstadoRepuestosDataSet.SCGTA_TB_EstadoRepuestoResources.Select("codigo = " & dstRepXOrden.SCGTA_TB_RepuestosxOrden.Rows(intFila).Item("CodEstadoRep") & " and Cultura = '" & System.Threading.Thread.CurrentThread.CurrentCulture.Name & "'")
                    dstRepXOrden.SCGTA_TB_RepuestosxOrden.Rows(intFila).Item("DescEstadoResources") = drwFila(0)("Descripcion")
                End If

            Next

        End Sub

        Public Sub CargarEstadosActividadesResurces(ByVal dstActXOrden As DMSOneFramework.ActividadesXFaseDataset)
            Dim intFila As Integer

            For intFila = 0 To dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows.Count - 1
                Select Case (dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows(intFila).Item("Estado")).ToLower
                    Case "no iniciado", "no iniciada"
                        dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.NoIniciada

                    Case "proceso", "en proceso"
                        dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Enproceso

                    Case "suspendido", "suspendida"
                        dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Suspendida

                    Case "finalizado", "finalizada"
                        dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Finalizada

                    Case "iniciado", "iniciada"
                        dstActXOrden.SCGTA_TB_ActividadesxOrden.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Iniciada



                End Select
            Next
            dstActXOrden.AcceptChanges()
        End Sub

        Public Sub CargarEstadosActividadesResurces(ByVal dstActXOrden As DMSOneFramework.ColaboradorDataset)
            Dim intFila As Integer

            For intFila = 0 To dstActXOrden.SCGTA_TB_ControlColaborador.Rows.Count - 1
                Debug.Print((dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("Estado")).ToLower())
                Select Case (dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("Estado")).ToLower
                    Case "no iniciado", "no iniciada"
                        dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.NoIniciada

                    Case "proceso", "en proceso"
                        dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Enproceso

                    Case "suspendido", "suspendida"
                        dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Suspendida

                    Case "finalizado", "finalizada"
                        dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Finalizada

                    Case "iniciado", "iniciada"
                        dstActXOrden.SCGTA_TB_ControlColaborador.Rows(intFila).Item("DescripcionActividadResources") = My.Resources.ResourceUI.Iniciada

                End Select
            Next
        End Sub


        Public Sub CargarUnidadesTiempoConfigurada(ByRef strDescripcionUnidadTiempo As String, ByRef dblValorUnidadTiempo As Double)

            If g_intUnidadTiempo <> -1 Then

                Dim adpUnidadTiempoDataAdapter As New DMSONEDKFramework.UnidadTiempoDataAdapter
                Dim dstUnidadTiempoDataSet As New DMSONEDKFramework.UnidadTiempoDataSet
                Dim drwFila() As DataRow
                adpUnidadTiempoDataAdapter.Fill(dstUnidadTiempoDataSet)
                drwFila = dstUnidadTiempoDataSet.SCGTA_TB_UnidadTiempo.Select("CodigoUnidadTiempo = " & g_intUnidadTiempo)
                strDescripcionUnidadTiempo = drwFila(0)("DescripcionUnidadTiempo")
                dblValorUnidadTiempo = drwFila(0)("TiempoMinutosUnidadTiempo")

            End If

        End Sub


        Public Sub MostrarCampos()
        
            Dim strCampoVisible As String = System.Configuration.ConfigurationManager.AppSettings("CamposVisibles")

            If strCampoVisible <> String.Empty Then
                g_blnCampoVisible = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings("CamposVisibles"))
            End If

        End Sub




#End Region

    End Module
End Namespace
