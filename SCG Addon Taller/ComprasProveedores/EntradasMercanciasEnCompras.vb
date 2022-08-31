
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports DMSOneFramework
Imports System.Timers

Public Class EntradasMercanciasEnCompras

#Region "Definiciones"

    Private WithEvents SBO_Application As Application
    Private SBO_Company As SAPbobsCOM.Company

    Private _FormEMC As Form

    Private mc_strSCGD_NoOT As String = "U_SCGD_NoOT"
    Private blnReprocesaEntrada As Boolean = False
    Private Shared oTimer As System.Timers.Timer
    Private _blnDocCerrar As Boolean = False


#End Region

#Region "Constructor"
    <CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)
        SBO_Application = SBOAplication
        SBO_Company = ocompany
    End Sub

#End Region

#Region "Enumeradores"
    Private Enum TipoArticulo
        Repuesto = 1
        Servicio = 2
        Suministro = 3
        ServicioExterno = 4
        Paquete = 5
        Otros = 6
        Accesorio = 7
        Vehiculo = 8
        Tramite = 9
        ArticuloCita = 10
        OtrosCostosGastos = 11
        OtrosIngresos = 12
    End Enum

    Private Enum Account
        ExpensesAc = 0
        TransferAc = 1
    End Enum

    Private Enum TipoDocumentoMarketing
        OfertaCompra = 540000006
        OrdenCompra = 22
        EntradaMercancia = 20
        FacturaProveedor = 18
        NotaCredito = 19
        DevolucionMercancia = 21
    End Enum

    Private Enum ArticuloAprobado
        scgSi = 1
        scgNo = 2
        scgFalta = 3
        scgCambioOT = 4
    End Enum
#End Region

#Region "Propiedades"
    Public Property FormEmc As Form
        Get
            Return _FormEMC
        End Get
        Set(ByVal value As Form)
            _FormEMC = value
        End Set
    End Property

    Public Property blnDocCerrar As Boolean
        Get
            Return _blnDocCerrar
        End Get
        Set(ByVal value As Boolean)
            _blnDocCerrar = value
        End Set
    End Property
#End Region

#Region "Manejo de eventos"

    ''' <summary>
    ''' Maneja el eventos ItemPressed del formulario de Entradas de Mercancia de Compra
    ''' </summary>
    ''' <param name="pval">Objeto evento</param>
    ''' <param name="FormUID">La propiedad FORMUID del evento</param>
    ''' <param name="BubbleEvent">BubbleEvent</param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                        ByVal FormUID As String, _
                                        ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Try

            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)

            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            If pval.ItemUID = "1" Then
                                'El formulario debe asignarse, ya que se utiliza en el ActionSuccess
                                'de lo contrario genera una excepción
                                FormEmc = oForm
                                If ValidaLineasSinAprobar() AndAlso ExistenLineasSinAprobar() Then
                                    BubbleEvent = False
                                End If
                            End If
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            If pval.ItemUID = "1" Then
                                FormEmc = oForm
                                blnReprocesaEntrada = True
                            End If
                    End Select
                ElseIf pval.ActionSuccess Then
                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            'Implementar aquí operaciones en modo crear
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            'Implementar aquí operaciones en modo actualizar
                        Case SAPbouiCOM.BoFormMode.fm_OK_MODE
                            If pval.ItemUID = "1" Then
                                If blnReprocesaEntrada Then
                                    blnDocCerrar = False
                                    ReprocesarEntrada()
                                End If
                                blnReprocesaEntrada = False
                            End If
                    End Select
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            If pval.ActionSuccess Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                End If
            End If
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressCierre(ByRef pval As SAPbouiCOM.ItemEvent, _
                                        ByVal FormUID As String, _
                                        ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Try
            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)
            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        Case SAPbouiCOM.BoFormMode.fm_OK_MODE
                            ReprocesoEntradaMercancia()
                    End Select
                ElseIf pval.ActionSuccess Then
                    Select Case pval.FormMode
                        Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            'Implementar aquí operaciones en modo crear
                        Case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            'Implementar aquí operaciones en modo actualizar
                        Case SAPbouiCOM.BoFormMode.fm_OK_MODE
                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            If pval.ActionSuccess Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                End If
            End If
        End Try
    End Sub
    
#End Region
    Private Sub SBO_Application_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, _
                                      ByRef BubbleEvent As Boolean) Handles SBO_Application.MenuEvent
        Dim oForm As SAPbouiCOM.Form
        Try
            If pVal.BeforeAction Then
                Select Case pVal.MenuUID
                    Case IDMenus.IDMenus.strMenuCerrar
                        oForm = SBO_Application.Forms.ActiveForm
                        Select Case SBO_Application.Forms.ActiveForm.TypeEx
                            Case "143"
                                FormEmc = oForm
                                blnDocCerrar = True
                        End Select
                End Select
            ElseIf Not pVal.BeforeAction Then
               
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ReprocesoEntradaMercancia()
        Dim blnEstado As Boolean = False
        Try
            For index As Integer = 1 To 10
                blnEstado = ManejaEntradaMercancia()
                If blnEstado Then
                    Exit For
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


#Region "Nuevos metodos"
    Public Function ManejaEntradaMercancia() As Boolean
        Dim oListaCotizacion As List(Of SAPbobsCOM.Documents)
        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim oGeneralDataList As List(Of SAPbobsCOM.GeneralData)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim strDocEntry As String = String.Empty
        Dim intError As Integer = 0
        Dim strMensajeError As String = String.Empty
        Dim blnProcesar As Boolean = False
        Dim blnCorrecto As Boolean = False

        Try
            strDocEntry = FormEmc.DataSources.DBDataSources.Item("OPDN").GetValue("DocEntry", 0)
            InicializarTimer()

            If Not String.IsNullOrEmpty(strDocEntry) Then
                oListaCotizacion = New List(Of SAPbobsCOM.Documents)
                oGeneralDataList = New List(Of SAPbobsCOM.GeneralData)
                oCompanyService = SBO_Company.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                If ProcesaCantidadesyCostosCotizacion(strDocEntry, oListaCotizacion) Then blnProcesar = True Else blnProcesar = False
                If ProcesaEntradaMercancia(strDocEntry, oJournalEntry, oGeneralDataList) Then blnProcesar = True Else blnProcesar = False

                If blnProcesar Then
                    ResetTransaction()
                    StartTransaction()
                    '****************Actualiza Cotización - Cantidades y Costos ********************
                    If Not oListaCotizacion Is Nothing Then
                        For Each rowCotizacion As SAPbobsCOM.Documents In oListaCotizacion
                            If rowCotizacion.Update() <> 0 Then
                                SBO_Company.GetLastError(intError, strMensajeError)
                                Throw New ExceptionsSBO(intError, strMensajeError)
                            End If
                        Next
                    End If
                    '****************Asiento Servicio Externo********************
                    If Not oJournalEntry Is Nothing Then
                        If oJournalEntry.Add <> 0 Then
                            SBO_Company.GetLastError(intError, strMensajeError)
                            Throw New ExceptionsSBO(intError, strMensajeError)
                        End If
                    End If

                    '****************Tracking OT********************
                    If Not oGeneralDataList Is Nothing Then
                        For Each rowoGeneralData As SAPbobsCOM.GeneralData In oGeneralDataList
                            oGeneralService.Update(rowoGeneralData)
                        Next
                    End If
                    '****************Actualizar entrada mercancia********************
                    If Not ActualizarEntradaMercancias(strDocEntry) Then
                        RollbackTransaction()
                    End If

                    CommitTransaction()
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                    Return True
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorTransaccion, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            RollbackTransaction()
            Return False
        Finally
            DetenerTimer()
            'If Not oCotizacion Is Nothing Then
            '    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
            '    oCotizacion = Nothing
            'End If
        End Try
    End Function

    Public Function ValidaLineasSinAprobar() As Boolean
        Dim strBloqueaEntradasSinAprobar As String = String.Empty
        Dim blnResultado As Boolean = False

        Try
            'Si el campo U_BloqEntradaSA de los parámetros generales del add-on está en "Y", 
            'se valida que todas las líneas de repuestos y servicios externos estén aprobadas en la oferta de ventas
            'previo a crear la entrada de mercancías, en caso de no ser así se bloquea la creación del documento.
            strBloqueaEntradasSinAprobar = DMS_Connector.Configuracion.ParamGenAddon.U_BloqEntradaSA
            If Not String.IsNullOrEmpty(strBloqueaEntradasSinAprobar) AndAlso strBloqueaEntradasSinAprobar.Equals("Y") Then
                blnResultado = True
            End If

            Return blnResultado

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Function ExistenLineasSinAprobar() As Boolean
        Dim strNoOT As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strIDActividad As String = String.Empty
        Dim strIDsParameter As String = String.Empty
        Dim oMatrix As Matrix
        Dim oEditText As SAPbouiCOM.EditText
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strMensajeError As String = String.Empty
        Dim strItemCode As String = String.Empty
        Dim strItemDescription As String = String.Empty
        Dim strQuery As String = "SELECT T1.""ItemCode"", T1.""Dscription"" FROM ""OQUT"" T0 INNER JOIN ""QUT1"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""DocEntry"" IN (SELECT Max(S1.""DocEntry"") FROM ""OQUT"" S1 WHERE S1.""U_SCGD_Numero_OT"" = '{0}') AND T1.""U_SCGD_ID"" IN({1}) AND T1.""U_SCGD_Aprobado"" IN ('2','3')"
        Dim blnResultado As Boolean = False

        Try
            oRecordset = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oMatrix = FormEmc.Items.Item("38").Specific
            strNoOT = FormEmc.DataSources.DBDataSources.Item("OPDN").GetValue("U_SCGD_Numero_OT", 0).Trim

            If Not String.IsNullOrEmpty(strNoOT) Then
                'Recorre todas las líneas de la entrada y guarda los IDs de los artículos de tipo repuesto y servicio externo en una variable,
                'para posteriormente ser utilizada en un query
                For i As Integer = 0 To oMatrix.RowCount - 1
                    oEditText = oMatrix.Columns.Item("U_SCGD_ID").Cells.Item(i + 1).Specific
                    strIDActividad = oEditText.Value.Trim()
                    oEditText = oMatrix.Columns.Item("U_SCGD_TipArt").Cells.Item(i + 1).Specific
                    strTipoArticulo = oEditText.Value.Trim()
                    If Not String.IsNullOrEmpty(strIDActividad) AndAlso Not String.IsNullOrEmpty(strTipoArticulo) Then
                        If strTipoArticulo = TipoArticulo.Repuesto Or strTipoArticulo = TipoArticulo.ServicioExterno Then
                            If String.IsNullOrEmpty(strIDsParameter) Then
                                strIDsParameter = String.Format("'{0}'", strIDActividad)
                            Else
                                strIDsParameter = String.Format("{0},'{1}'", strIDsParameter, strIDActividad)
                            End If
                        End If
                    End If
                Next

                'Ejecuta el query con los IDs y obtiene los artículos que no están aprobados
                'imprimiendo un mensaje en pantalla con la información de la línea
                If Not String.IsNullOrEmpty(strIDsParameter) Then
                    strQuery = String.Format(strQuery, strNoOT, strIDsParameter)
                    oRecordset.DoQuery(strQuery)
                    While Not oRecordset.EoF
                        'Por cada artículo que no esté aprobado, se muestra un mensaje de error
                        blnResultado = True
                        strItemCode = oRecordset.Fields.Item(0).Value.ToString()
                        strItemDescription = oRecordset.Fields.Item(1).Value.ToString()
                        strMensajeError = My.Resources.Resource.ErrorSinAprobar
                        SBO_Application.StatusBar.SetText(String.Format(strMensajeError, strItemCode, strItemDescription), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        oRecordset.MoveNext()
                    End While
                End If
            End If

            Return blnResultado

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Metodo encargado de volver a procesar la entrada para actualizar cantidades, costos y generar asiento de servicios externos
    ''' en caso de que por algún motivo, se haya presentado un error ocasionando que no se actualicen los datos.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ReprocesarEntrada()
        Dim strProcesaAsientoSE As String = String.Empty
        Dim strDocEntry As String = String.Empty
        Try
            strDocEntry = FormEmc.DataSources.DBDataSources.Item("OPDN").GetValue("DocEntry", 0)
            strProcesaAsientoSE = FormEmc.DataSources.DBDataSources.Item("OPDN").GetValue("U_SCGD_ProASEE", 0)

            If Not String.IsNullOrEmpty(strProcesaAsientoSE) AndAlso strProcesaAsientoSE.ToUpper().Equals("Y") Then
                ReprocesoEntradaMercancia()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub InicializarTimer()
        Try
            'Inicializa un timer que se ejecuta cada 30 segundos
            'y llama al método LimpiarColaMensajes
            oTimer = New System.Timers.Timer(30000)
            RemoveHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            AddHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            oTimer.AutoReset = True
            oTimer.Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub DetenerTimer()
        Try
            oTimer.Stop()
            oTimer.Dispose()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LimpiarColaMensajes()
        Try
            'En las operaciones muy largas, la cola de mensajes se llena ocasionando que el add-on se desconecte y genere errores como
            'RPC Server call o similares. Para solucionarlo se debe ejecutar este método cada cierto tiempo (30 o 60 segundos) para limpiar
            'la cola de mensajes
            DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub



    Public Function ProcesaEntradaMercancia(ByVal p_strDocEntry As String, ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries, ByRef p_oGeneralDataList As List(Of SAPbobsCOM.GeneralData)) As Boolean
        Try
            '**********DataContract****************
            Dim oLineaEntradaMercanciaList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oTipoOTList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List
            Dim oDatosGeneralesList As DatoGenerico_List = New DatoGenerico_List
            Dim oConfiguracionGeneralList As ConfiguracionGeneral_List = New ConfiguracionGeneral_List
            '********Listas genericas*************
            Dim oSucursalList As List(Of String) = New Generic.List(Of String)
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim oCodigoMarcaList As List(Of String) = New Generic.List(Of String)
            Dim oDocEntryCotizacionList As List(Of String) = New Generic.List(Of String)
            Dim oBaseEntryList As List(Of Integer) = New Generic.List(Of Integer)
            Dim oBodegaCentroCostoList As BodegaCentroCosto_List = New BodegaCentroCosto_List()
            '**********Declaración Variables*****************
            Dim blnProcesaEntradaMercancia As Boolean = False
            '*************Clases**************************
            Dim clsDocumentoProcesoCompra As DocumentoProcesoCompra = New DocumentoProcesoCompra(SBO_Company, SBO_Application)
            Dim CancelStatus As SAPbobsCOM.CancelStatusEnum
            Dim blnCancela As Boolean = False
            '********Carga información lineas de entrada mercancia*************
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                CargaConfiguracionGeneral(oConfiguracionGeneralList)
                blnProcesaEntradaMercancia = CargaEntradaMercancia(CInt(p_strDocEntry), oLineaEntradaMercanciaList, oSucursalList, oNoOrdenList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList, oBaseEntryList, CancelStatus)
            End If
            '********Valida si existen lineas en la entrada de mercancia que sean de tipo(Servicio Externo) que esten ligadas a una OT y que necesite procesar para saber si genera asiento*************
            If blnProcesaEntradaMercancia Then
                '**********************************************
                '*********** Actualiza Valores Cotizacion******
                '**********************************************
                'SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                'CargarDocEntryCotizacion(oNoOrdenList, oDocEntryCotizacionList)
                'ActualizaValoresCotizacion(oNoOrdenList, oLineaEntradaMercanciaList, oConfiguracionGeneralList, oDocEntryCotizacionList)
                '**********************************************
                '*********** Recorre Documentos Marketing******
                '**********************************************
                'Se deshabilita la funcionalidad de cierre de backorders,
                'el cliente debe cerrar los documentos manualmente
                'clsDocumentoProcesoCompra.ManejarBackOrder(oLineaEntradaMercanciaList, oConfiguracionGeneralList, oBaseEntryList)
                '**********************************************
                '*********** Genera Asiento Servicio Externo******
                '**********************************************
                If Not ManejarAsientoServicioExterno(oLineaEntradaMercanciaList, oConfiguracionGeneralList, oSucursalList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList, p_oJournalEntry, CancelStatus) Then Return False
                '**********************************************
                '*********** Maneja Tracking******
                '**********************************************
                If oConfiguracionGeneralList.Item(0).UsaOTInterna Then
                    If CancelStatus = CancelStatusEnum.csCancellation Or blnDocCerrar Then blnCancela = True
                    If Not clsDocumentoProcesoCompra.ManejarTrackingOT(oNoOrdenList, oLineaEntradaMercanciaList, oDatosGeneralesList, TipoDocumentoMarketing.EntradaMercancia, p_oGeneralDataList, blnCancela) Then Return False
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function


    Public Function ManejarAsientoServicioExterno(ByRef p_oLineaEntradaMercanciaList As DocumentoMarketing_List, _
                                             ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List, _
                                             ByRef p_oSucursalList As List(Of String), _
                                             ByRef p_oCodigoMarcaList As List(Of String), _
                                             ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                             ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                             ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries, _
                                             ByRef p_CancelStatus As SAPbobsCOM.CancelStatusEnum) As Boolean
        Try
            '**************Data Contract****************
            Dim oConfiguracionSucursalList As ConfiguracionSucursal_List = New ConfiguracionSucursal_List
            Dim oServicioExternoList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oDimensionesContablesList As DimensionesContables_List = New DimensionesContables_List
            Dim oAsientoServicioExternoList As Asiento_List = New Asiento_List
            Dim oBodegaCentroCostoList As BodegaCentroCosto_List = New BodegaCentroCosto_List
            '**************Variables ********************
            Dim blnDimensionesYaCargadas As Boolean = False
            Dim blnAsientoServicioExternoExitoso As Boolean = False
            '*************Clases**************************
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)


            If p_oConfiguracionGeneralList.Item(0).UsaAsientoServicioExterno Then
                '********Carga configuración sucursal*************
                If p_oSucursalList.Count > 0 Then
                    CargaConfiguracionSucursal(p_oSucursalList, oConfiguracionSucursalList, oBodegaCentroCostoList)
                End If
                '********Si a nivel de compañia se usan dimensiones, valida si lo hace a nivel de Tipo OT*************
                For Each rowConfiguracionSucursal As ConfiguracionSucursal In oConfiguracionSucursalList
                    If rowConfiguracionSucursal.UsaAsientoServicioExterno Then
                        If rowConfiguracionSucursal.UsaDimensiones Then
                            If p_oTipoOTList.Count > 0 Then
                                ValidaUsaDimensionesTipoOT(p_oTipoOTList)
                            End If
                            If Not blnDimensionesYaCargadas Then
                                ClsLineasDocumentosDimension.CargaCentrosCostoDimensionesOT(p_oSucursalList, p_oCodigoMarcaList, oDimensionesContablesList)
                                blnDimensionesYaCargadas = True
                            End If
                        End If
                        CargaListasTipoArticulo(p_oLineaEntradaMercanciaList, oServicioExternoList, p_oTipoOTList, rowConfiguracionSucursal, oDimensionesContablesList, oBodegaCentroCostoList)
                    End If
                Next
                ProcesaAsientoServicioExterno(oServicioExternoList, oAsientoServicioExternoList, p_CancelStatus)

                '************Verifica si genera asiento para servicio externo****************
                If oAsientoServicioExternoList.Count > 0 Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesandoAsientoServExt, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning)
                    If Not CrearAsiento(p_oDatosGeneralesList, oAsientoServicioExternoList, TipoArticulo.ServicioExterno, p_oJournalEntry) Then Return False
                    '****************Maneja transacción**************
                    'ResetTransaction()
                    'StartTransaction()
                    'If CrearAsiento(p_oDatosGeneralesList, oAsientoServicioExternoList, TipoArticulo.ServicioExterno) > 0 Then
                    '    ActualizarEntradaMercancias(p_oDatosGeneralesList)
                    '    '*****************Realiza commit ala transaccion**************
                    '    CommitTransaction()
                    '    '*****************Mensaje asiento generado correctamente*****************
                    '    SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                    'Else
                    '    RollbackTransaction()
                    '    SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                    '    Exit Function
                    'End If
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Actualiza la entrada de mercancías posterior al procesamiento de DMS
    ''' </summary>
    ''' <param name="p_oDatosGeneralesList"></param>
    ''' <remarks></remarks>
    Public Function ActualizarEntradaMercancias(ByRef p_strDocEntry As String) As Boolean
        Dim oEntrada As SAPbobsCOM.Documents
        Try
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                oEntrada = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)
                If oEntrada.GetByKey(CInt(p_strDocEntry)) Then
                    oEntrada.UserFields.Fields.Item("U_SCGD_ProASEE").Value = "N"
                    oEntrada.Update()
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub ManejaCantidadesyCosto(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                      ByRef p_rowEntrada As DocumentoMarketing, _
                                      ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List)
        Try
            Select Case p_rowEntrada.TipoArticulo
                Case TipoArticulo.ServicioExterno
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value += p_rowEntrada.Costo
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += p_rowEntrada.Cantidad
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value -= p_rowEntrada.Cantidad
                    If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value < 0 Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                    End If
                Case Else
                    If p_oConfiguracionGeneralList.Item(0).UsaBackOrder Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += p_rowEntrada.Cantidad
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value -= p_rowEntrada.Cantidad
                        If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value < 0 Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                        End If
                    Else
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value += p_rowEntrada.Cantidad
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value += (p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value) - p_rowEntrada.Cantidad
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                    End If
            End Select
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ActualizaValoresCotizacion(ByRef p_oNoOrdenList As Generic.List(Of String), _
                                          ByRef p_oLineaEntradaMercanciaList As DocumentoMarketing_List, _
                                          ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List, _
                                          ByRef p_oDocEntryCotizacionList As Generic.List(Of String))
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            '*************Objetos SAP *******************
            Dim oListaCotizacion As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim strCampo As String = String.Empty
            Dim blnUsaIdRepXOrd As Boolean = False
            Dim blnProcesaLinea As Boolean = False
            Dim blnActualizaCotizacion As Boolean = False
            Dim intResultado As Integer = 1

            For Each rowDocEntry As String In p_oDocEntryCotizacionList
                If Not String.IsNullOrEmpty(rowDocEntry) Then
                    oCotizacion = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations),  _
                                                           SAPbobsCOM.Documents)
                    intDocEntry = Convert.ToInt32(rowDocEntry)
                    If oCotizacion.GetByKey(intDocEntry) Then
                        For Each rowEntrada As DocumentoMarketing In p_oLineaEntradaMercanciaList
                            blnActualizaCotizacion = False
                            If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                                If oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowEntrada.NoOrden.Trim Then
                                    strCampo = String.Empty
                                    blnUsaIdRepXOrd = False
                                    If Not String.IsNullOrEmpty(rowEntrada.ID) Then
                                        strCampo = "U_SCGD_ID"
                                        blnUsaIdRepXOrd = False
                                    ElseIf Not String.IsNullOrEmpty(rowEntrada.IdRepxOrd) Then
                                        strCampo = "U_SCGD_IdRepxOrd"
                                        blnUsaIdRepXOrd = True
                                    End If
                                    For contador As Integer = 0 To oCotizacion.Lines.Count - 1
                                        oCotizacion.Lines.SetCurrentLine(contador)
                                        blnProcesaLinea = False
                                        If blnUsaIdRepXOrd Then
                                            If oCotizacion.Lines.UserFields.Fields.Item(strCampo).Value = rowEntrada.IdRepxOrd Then
                                                blnProcesaLinea = True
                                            End If
                                        Else
                                            If oCotizacion.Lines.UserFields.Fields.Item(strCampo).Value.ToString.Trim() = rowEntrada.ID.Trim Then
                                                blnProcesaLinea = True
                                            End If
                                        End If
                                        If blnProcesaLinea Then
                                            ManejaCantidadesyCosto(oCotizacion, rowEntrada, p_oConfiguracionGeneralList)
                                            blnActualizaCotizacion = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        Next
                        oListaCotizacion.Add(oCotizacion)
                    End If
                End If
            Next
            '****************Manejo Transaccion SAP ********************
            ResetTransaction()
            StartTransaction()
            For Each rowCotizacion As SAPbobsCOM.Documents In oListaCotizacion
                intResultado = rowCotizacion.Update()
                If intResultado <> 0 Then
                    SBO_Application.StatusBar.SetText(String.Format("{0}: {1}", intResultado, SBO_Company.GetLastErrorDescription), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    SCG.ServicioPostVenta.Utilitarios.ManejadorErrores(New Exception(String.Format("{2} - {0}: {1}", intResultado, SBO_Company.GetLastErrorDescription, rowCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value)), SBO_Application)
                    RollbackTransaction()
                    Exit Sub
                End If
            Next
            CommitTransaction()
        Catch ex As Exception
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If Not oCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
        End Try
    End Sub

    Public Function CargaEntradaMercancia(ByVal p_intDocEntry As Integer, _
                                          ByRef p_oLineaEntradaMercanciaList As DocumentoMarketing_List, _
                                          ByRef p_oSucursalList As Generic.List(Of String), _
                                          ByRef p_oNoOrdenList As Generic.List(Of String), _
                                          ByRef p_oCodigoMarcaList As Generic.List(Of String), _
                                          ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                          ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                          ByRef p_oBaseEntryList As Generic.List(Of Integer), _
                                          ByRef p_CancelStatus As SAPbobsCOM.CancelStatusEnum) As Boolean
        Dim oEntradaMercancia As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaEntradaMercancia As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            Dim oDatosGenerales As DatoGenerico
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strCentroCosto As String = String.Empty
            Dim strSucursal As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim strCodigoMarca As String = String.Empty
            Dim blnProcesaEntradaMercancia As Boolean = False
            Dim strMonedaLocal As String = String.Empty

            '****Consulta moneda local*********
            strMonedaLocal = ConsultaMonedaLocal()
            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oEntradaMercancia = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                     SAPbobsCOM.Documents)
                '************Carga Objeto Entrada Mercancia********************************
                If oEntradaMercancia.GetByKey(p_intDocEntry) Then
                    p_CancelStatus = oEntradaMercancia.CancelStatus
                    oDatosGenerales = New DatoGenerico
                    With oDatosGenerales
                        .DocEntry = oEntradaMercancia.DocEntry
                        .DocNum = oEntradaMercancia.DocNum
                        .FechaContabilizacion = oEntradaMercancia.DocDate
                        .FechaCreacion = oEntradaMercancia.CreationDate
                        .CardCode = oEntradaMercancia.CardCode
                        .CardName = oEntradaMercancia.CardName
                        .MonedaLocal = strMonedaLocal
                        .Observaciones = oEntradaMercancia.Comments
                        If Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            .NoOrden = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()
                        End If
                    End With
                    p_oDatosGeneralesList.Add(oDatosGenerales)
                    '********Recorre lineas de la Entrada Mercancia***********************
                    For rowEntrada As Integer = 0 To oEntradaMercancia.Lines.Count - 1
                        oEntradaMercancia.Lines.SetCurrentLine(rowEntrada)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strSucursal = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oEntradaMercancia.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            oLineaEntradaMercancia = New DocumentoMarketing()
                            With oLineaEntradaMercancia
                                .ItemCode = oEntradaMercancia.Lines.ItemCode
                                .ItemDescripcion = oEntradaMercancia.Lines.ItemDescription
                                .BodegaOrigen = oEntradaMercancia.Lines.WarehouseCode
                                .TipoArticulo = intTipoArticulo
                                .Cantidad = oEntradaMercancia.Lines.Quantity
                                .BaseDocType = oEntradaMercancia.Lines.BaseType
                                .BaseDocEntry = oEntradaMercancia.Lines.BaseEntry
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    .TipoOT = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                ElseIf Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                    .TipoOT = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                End If
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value) Then
                                    .CodigoProyecto = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value
                                End If
                                .Costo = oEntradaMercancia.Lines.LineTotal
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()) Then
                                    .Sucursal = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                ElseIf Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                    .Sucursal = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                                End If
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value.ToString()) Then
                                    .CodigoMarca = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                ElseIf Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                    .CodigoMarca = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                End If
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value) Then
                                    .IdRepxOrd = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    .ID = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString()) Then
                                    .CentroCosto = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()
                                Else
                                    .CentroCosto = DevuelveValorArticulo(oEntradaMercancia.Lines.ItemCode, "U_SCGD_CodCtroCosto")
                                End If
                            End With
                            p_oLineaEntradaMercanciaList.Add(oLineaEntradaMercancia)
                            '***************Agrega Sucursal al List*************************
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                strSucursal = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()
                                If Not p_oSucursalList.Contains(strSucursal) Then
                                    p_oSucursalList.Add(strSucursal)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                strSucursal = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()
                                If Not p_oSucursalList.Contains(strSucursal) Then
                                    p_oSucursalList.Add(strSucursal)
                                End If
                            End If
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            '**************Agrega Codigo Marca al List******************
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value) Then
                                strCodigoMarca = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                    p_oCodigoMarcaList.Add(strCodigoMarca)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                strCodigoMarca = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                    p_oCodigoMarcaList.Add(strCodigoMarca)
                                End If
                            End If
                            '**************Agrega TipoOT al List******************
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                oTipoOT = New ConfiguracionOrdenTrabajo
                                With oTipoOT
                                    .TipoOT = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                End With
                                If Not p_oTipoOTList.Contains(oTipoOT) Then
                                    p_oTipoOTList.Add(oTipoOT)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                oTipoOT = New ConfiguracionOrdenTrabajo
                                With oTipoOT
                                    .TipoOT = oEntradaMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                End With
                                If Not p_oTipoOTList.Contains(oTipoOT) Then
                                    p_oTipoOTList.Add(oTipoOT)
                                End If
                            End If
                            '**************Agrega Base Entry al List******************
                            If Not p_oBaseEntryList.Contains(oEntradaMercancia.Lines.BaseEntry) Then
                                p_oBaseEntryList.Add(oEntradaMercancia.Lines.BaseEntry)
                            End If
                            blnProcesaEntradaMercancia = True
                        End If
                    Next
                End If
            End If
            Return blnProcesaEntradaMercancia
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If Not oEntradaMercancia Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oEntradaMercancia)
                oEntradaMercancia = Nothing
            End If
        End Try
    End Function

    Public Sub AsignaCentrosCostoDimensiones(ByRef p_rowLineaEntrada As DocumentoMarketing, _
                                            ByRef p_oListaTipoArticulo As DocumentoMarketing, _
                                            ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                            ByRef p_oDimensionesContablesList As DimensionesContables_List)
        Try
            For Each rowTipoOT As ConfiguracionOrdenTrabajo In p_oTipoOTList
                If p_rowLineaEntrada.TipoOT = rowTipoOT.TipoOT Then
                    If rowTipoOT.UsaDimensionAsientoEntradaMercancia Then
                        For Each rowDimensionesContables As DimensionesContables In p_oDimensionesContablesList
                            If p_rowLineaEntrada.Sucursal = rowDimensionesContables.Sucursal And p_rowLineaEntrada.CodigoMarca = rowDimensionesContables.CodigoMarca Then
                                With p_oListaTipoArticulo
                                    .CostingCode = rowDimensionesContables.CostingCode
                                    .CostingCode2 = rowDimensionesContables.CostingCode2
                                    .CostingCode3 = rowDimensionesContables.CostingCode3
                                    .CostingCode4 = rowDimensionesContables.CostingCode4
                                    .CostingCode5 = rowDimensionesContables.CostingCode5
                                    .UsaDimensiones = True
                                End With
                                Exit For
                            End If
                        Next
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargaConfiguracionGeneral(ByRef p_oConfiguracionGeneralList As ConfiguracionGeneral_List)
        Try
            '********Declaración de data contract*************
            Dim oConfiguracionGeneral As ConfiguracionGeneral
            '********Declaración de variables*****************
            Dim oDataTableConfiguracionGeneral As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionGeneral As System.Data.DataRow
            '******************************************************************************
            '******************** Carga Configuración de tabla ConfiguracionSucursal*******
            '******************************************************************************
            oDataTableConfiguracionGeneral = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_GenAsSE, U_BO_Parc From dbo.[@SCGD_ADMIN] with (nolock)"),
                                                       SBO_Company.CompanyDB,
                                                       SBO_Company.Server)
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            For Each oDataRowConfiguracionGeneral In oDataTableConfiguracionGeneral.Rows
                oConfiguracionGeneral = New ConfiguracionGeneral()
                With oConfiguracionGeneral
                    '*********************************************************************
                    '**************Valida si genera asientos servicio externo*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionGeneral.Item("U_GenAsSE")) Then
                        If oDataRowConfiguracionGeneral.Item("U_GenAsSE").ToString.Trim() = "Y" Then
                            .UsaAsientoServicioExterno = True
                        Else
                            .UsaAsientoServicioExterno = False
                        End If
                    Else
                        .UsaAsientoServicioExterno = False
                    End If
                    '*********************************************************************
                    '**************Valida si usa back order*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionGeneral.Item("U_BO_Parc")) Then
                        If oDataRowConfiguracionGeneral.Item("U_BO_Parc").ToString.Trim() = "Y" Then
                            .UsaBackOrder = True
                        Else
                            .UsaBackOrder = False
                        End If
                    Else
                        .UsaBackOrder = False
                    End If
                    '*********************************************************************
                    '**************Valida si usa OT SAP*************
                    '*********************************************************************
                    .UsaOTInterna = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
                End With
                p_oConfiguracionGeneralList.Add(oConfiguracionGeneral)
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargaListasTipoArticulo(ByRef p_oLineaEntradaMercanciaList As DocumentoMarketing_List, _
                                       ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                       ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                       ByRef p_rowConfiguracionSucursal As ConfiguracionSucursal, _
                                       ByRef p_oDimensionesContablesList As DimensionesContables_List, _
                                       ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List)
        Try
            '**************Declaracion de data contract**********
            Dim oServicioExterno As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            '************Variables********************************

            For Each rowLineaEntrada As DocumentoMarketing In p_oLineaEntradaMercanciaList
                '********************Valida si la sucursal es la misma de la cual se esta recorriendo************
                If rowLineaEntrada.Sucursal = p_rowConfiguracionSucursal.SucursalID Then
                    '************Según tipo de articulo valida que lista cargar********************************
                    Select Case rowLineaEntrada.TipoArticulo
                        Case TipoArticulo.ServicioExterno
                            If p_rowConfiguracionSucursal.UsaAsientoServicioExterno Then
                                oServicioExterno = New DocumentoMarketing()
                                With oServicioExterno
                                    .ItemCode = rowLineaEntrada.ItemCode
                                    .BodegaOrigen = rowLineaEntrada.BodegaOrigen
                                    .TipoArticulo = rowLineaEntrada.TipoArticulo
                                    .NoOrden = rowLineaEntrada.NoOrden
                                    .TipoOT = rowLineaEntrada.TipoOT
                                    .CodigoProyecto = rowLineaEntrada.CodigoProyecto
                                    .Costo = rowLineaEntrada.Costo
                                    .Sucursal = rowLineaEntrada.Sucursal
                                    .CodigoMarca = rowLineaEntrada.CodigoMarca
                                    '*********************Asignación almacen segun centro de costo*********
                                    If Not String.IsNullOrEmpty(rowLineaEntrada.CentroCosto) Then
                                        .CentroCosto = rowLineaEntrada.CentroCosto
                                        AsignaBodegaCentroCosto(p_oBodegaCentroCostoList, rowLineaEntrada, oServicioExterno)
                                    End If
                                    '*********************Valida que usa dimensiones y asigna centro de costo dimensiones*********
                                    If p_rowConfiguracionSucursal.UsaDimensiones Then
                                        AsignaCentrosCostoDimensiones(rowLineaEntrada, oServicioExterno, p_oTipoOTList, p_oDimensionesContablesList)
                                    End If
                                End With
                                p_oServicioExternoList.Add(oServicioExterno)
                            End If
                    End Select
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub AsignaBodegaCentroCosto(ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List, _
                                       ByRef p_rowLineaEntrada As DocumentoMarketing, _
                                       ByRef p_oServicioExterno As DocumentoMarketing)
        Try
            For Each row As BodegaCentroCosto In p_oBodegaCentroCostoList
                If row.CentroCosto = p_rowLineaEntrada.CentroCosto AndAlso row.Sucursal = p_rowLineaEntrada.Sucursal Then
                    p_oServicioExterno.Almacen = row.BodegaServicioExterno
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function CrearAsiento(ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                ByRef p_oAsientoList As Asiento_List, _
                                ByVal p_intTipoArticulo As Integer, _
                                ByRef p_oJournalEntry As SAPbobsCOM.JournalEntries) As Boolean
        Try
            '************Objetos*********************
            'Dim oJournalEntry As SAPbobsCOM.JournalEntries
            '************Variables*******************
            Dim intAsientoGenerado As Integer = 0
            Dim strAsientoGenerado As String = String.Empty
            Dim intDocEntry As Integer = 0
            Dim intDocNum As Integer = 0
            Dim dateFechaContabilizacion As Date = Nothing
            Dim strMonedaLocal As String = String.Empty
            Dim intError As Integer = 0
            Dim strMensajeError As String = String.Empty
            Dim strNoOrden As String = String.Empty

            For Each rowGeneral As DatoGenerico In p_oDatosGeneralesList
                With rowGeneral
                    intDocEntry = .DocEntry
                    intDocNum = .DocNum
                    dateFechaContabilizacion = .FechaContabilizacion
                    strMonedaLocal = .MonedaLocal
                    strNoOrden = .NoOrden
                End With
                Exit For
            Next

            If p_oAsientoList.Count > 0 Then
                p_oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                If Not dateFechaContabilizacion = Nothing Then
                    p_oJournalEntry.ReferenceDate = dateFechaContabilizacion
                End If
                If Not String.IsNullOrEmpty(strNoOrden) Then
                    p_oJournalEntry.Reference = strNoOrden
                    p_oJournalEntry.UserFields.Fields.Item("U_SCGD_DocNum").Value = intDocNum.ToString()
                End If

                Select Case p_intTipoArticulo
                    Case TipoArticulo.Servicio
                        p_oJournalEntry.Memo = String.Empty
                    Case TipoArticulo.ServicioExterno
                        p_oJournalEntry.Memo = My.Resources.Resource.AsientoEntradaMercanciaCompra + intDocNum.ToString()
                    Case TipoArticulo.OtrosCostosGastos
                        p_oJournalEntry.Memo = String.Empty
                End Select


                For Each rowAsiento As Asiento In p_oAsientoList
                    '*********************
                    'Cuenta Credito
                    '*********************
                    p_oJournalEntry.Lines.AccountCode = rowAsiento.CuentaCredito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        p_oJournalEntry.Lines.Credit = rowAsiento.Costo
                    Else
                        p_oJournalEntry.Lines.FCCredit = rowAsiento.Costo
                        p_oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    p_oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = rowAsiento.IDSucursal
                    End If
                    If rowAsiento.UsaDimensiones Then
                        p_oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        p_oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        p_oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        p_oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        p_oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    p_oJournalEntry.Lines.Add()

                    '*****************
                    'Cuenta Debito
                    '*****************
                    p_oJournalEntry.Lines.AccountCode = rowAsiento.CuentaDebito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        p_oJournalEntry.Lines.Debit = rowAsiento.Costo
                    Else
                        p_oJournalEntry.Lines.FCDebit = rowAsiento.Costo
                        p_oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    p_oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    p_oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    p_oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(rowAsiento.IDSucursal) Then p_oJournalEntry.Lines.BPLID = rowAsiento.IDSucursal
                    End If
                    If rowAsiento.UsaDimensiones Then
                        p_oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        p_oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        p_oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        p_oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        p_oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    p_oJournalEntry.Lines.Add()
                Next
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Sub StartTransaction()
        Try
            If Not SBO_Company.InTransaction Then
                SBO_Company.StartTransaction()
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ResetTransaction()
        Try
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CommitTransaction()
        Try
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub RollbackTransaction()
        Try
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ObtenerCuentaAlmacen(ByRef p_strAlmacen As String, _
                                         ByRef p_intCuenta As Integer) As String
        Dim oAlmacen As SAPbobsCOM.Warehouses
        Try
            oAlmacen = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses)
            If oAlmacen.GetByKey(p_strAlmacen) Then
                Select Case p_intCuenta
                    Case Account.ExpensesAc
                        Return oAlmacen.ExpenseAccount
                    Case Account.TransferAc
                        Return oAlmacen.TransfersAcc
                End Select
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oAlmacen)
        End Try
    End Function

    Public Sub ProcesaAsientoServicioExterno(ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                             ByRef p_oLineaAsientoList As Asiento_List, _
                                             ByRef p_CancelStatus As SAPbobsCOM.CancelStatusEnum)
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim strCuentaCredito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            '*************Recorre lineas ServicioList*****************
            For Each rowServicioExterno As DocumentoMarketing In p_oServicioExternoList
                strCuentaDebito = String.Empty
                strCuentaCredito = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicioExterno.NoOrden
                    .Costo = rowServicioExterno.Costo
                    .Moneda = Nothing
                    '******Cuenta debito y cuenta credito************
                    If p_CancelStatus = CancelStatusEnum.csCancellation Or blnDocCerrar Then
                        strCuentaCredito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                        strCuentaDebito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.TransferAc, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                    Else
                        strCuentaDebito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                        strCuentaCredito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.TransferAc, rowServicioExterno.Sucursal, rowServicioExterno.Almacen)
                    End If
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(strCuentaCredito) Then
                        .CuentaCredito = strCuentaCredito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaCreditoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(rowServicioExterno.Sucursal) Then oLineaAsientoTemporal.IDSucursal = rowServicioExterno.Sucursal
                    If rowServicioExterno.UsaDimensiones Then
                        .UsaDimensiones = True
                        .CostingCode = rowServicioExterno.CostingCode
                        .CostingCode2 = rowServicioExterno.CostingCode2
                        .CostingCode3 = rowServicioExterno.CostingCode3
                        .CostingCode4 = rowServicioExterno.CostingCode4
                        .CostingCode5 = rowServicioExterno.CostingCode5
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            'Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.CuentaCredito = rowAsiento1.CuentaCredito And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        rowAsiento2.Aplicado = True
                        If dblCosto > 0 Then
                            blnAgregar = True
                        End If
                    End If
                Next
                If blnAgregar Then
                    oLineaAsiento = New Asiento
                    With oLineaAsiento
                        .NoOrden = rowAsiento1.NoOrden
                        .CuentaDebito = rowAsiento1.CuentaDebito
                        .CuentaCredito = rowAsiento1.CuentaCredito
                        .Costo = dblCosto
                        .Moneda = rowAsiento1.Moneda
                        .IDSucursal = rowAsiento1.IDSucursal
                        If rowAsiento1.UsaDimensiones Then
                            .UsaDimensiones = True
                            .CostingCode = rowAsiento1.CostingCode
                            .CostingCode2 = rowAsiento1.CostingCode2
                            .CostingCode3 = rowAsiento1.CostingCode3
                            .CostingCode4 = rowAsiento1.CostingCode4
                            .CostingCode5 = rowAsiento1.CostingCode5
                        End If
                    End With
                    p_oLineaAsientoList.Add(oLineaAsiento)
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ProcesaAsientoServicio(ByRef p_oServicioList As DocumentoMarketing_List, _
                                      ByRef p_oLineaAsientoList As Asiento_List)
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            '*************Recorre lineas ServicioList*****************
            For Each rowServicio As DocumentoMarketing In p_oServicioList
                strCuentaDebito = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowServicio.NoOrden
                    .CuentaCredito = rowServicio.CuentaCreditoManoObra
                    .Costo = rowServicio.Costo
                    .Moneda = rowServicio.MonedaManoObra
                    If Not String.IsNullOrEmpty(rowServicio.ItemCode) And Not String.IsNullOrEmpty(rowServicio.BodegaOrigen) Then
                        strCuentaDebito = ObtenerCuentaArticulo(rowServicio.ItemCode, rowServicio.BodegaOrigen, "SaleCostAc")
                    End If
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If rowServicio.UsaDimensiones Then
                        .UsaDimensiones = True
                        .CostingCode = rowServicio.CostingCode
                        .CostingCode2 = rowServicio.CostingCode2
                        .CostingCode3 = rowServicio.CostingCode3
                        .CostingCode4 = rowServicio.CostingCode4
                        .CostingCode5 = rowServicio.CostingCode5
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            'Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        rowAsiento2.Aplicado = True
                        If dblCosto > 0 Then
                            blnAgregar = True
                        End If
                    End If
                Next
                If blnAgregar Then
                    oLineaAsiento = New Asiento
                    With oLineaAsiento
                        .NoOrden = rowAsiento1.NoOrden
                        .CuentaDebito = rowAsiento1.CuentaDebito
                        .CuentaCredito = rowAsiento1.CuentaCredito
                        .Costo = dblCosto
                        .Moneda = rowAsiento1.Moneda
                        If rowAsiento1.UsaDimensiones Then
                            .UsaDimensiones = True
                            .CostingCode = rowAsiento1.CostingCode
                            .CostingCode2 = rowAsiento1.CostingCode2
                            .CostingCode3 = rowAsiento1.CostingCode3
                            .CostingCode4 = rowAsiento1.CostingCode4
                            .CostingCode5 = rowAsiento1.CostingCode5
                        End If
                    End With
                    p_oLineaAsientoList.Add(oLineaAsiento)
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ProcesaAsientoOtrosCostosGastos(ByRef p_oOtrosGastosList As DocumentoMarketing_List, _
                                               ByRef p_oLineaAsientoList As Asiento_List)
        Try
            '***********Data Contracts*********
            Dim oLineaAsiento As Asiento
            Dim oLineaAsientoTemporal As Asiento
            Dim oLineaAsientoTemporalList As Asiento_List = New Asiento_List
            '*****Variable***********
            Dim strCuentaDebito As String = String.Empty
            Dim dblCosto As Double = 0
            Dim blnAgregar As Boolean = False
            '*************Recorre lineas ServicioList*****************
            For Each rowOtroGasto As DocumentoMarketing In p_oOtrosGastosList
                strCuentaDebito = String.Empty
                oLineaAsientoTemporal = New Asiento
                With oLineaAsientoTemporal
                    .NoOrden = rowOtroGasto.NoOrden
                    .CuentaCredito = rowOtroGasto.CuentaCreditoOtrosGastos
                    .Costo = rowOtroGasto.Costo
                    .Moneda = rowOtroGasto.MonedaOtrosGastos
                    If Not String.IsNullOrEmpty(rowOtroGasto.ItemCode) And Not String.IsNullOrEmpty(rowOtroGasto.BodegaOrigen) Then
                        strCuentaDebito = ObtenerCuentaArticulo(rowOtroGasto.ItemCode, rowOtroGasto.BodegaOrigen, "SaleCostAc")
                    End If
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If rowOtroGasto.UsaDimensiones Then
                        .UsaDimensiones = True
                        .CostingCode = rowOtroGasto.CostingCode
                        .CostingCode2 = rowOtroGasto.CostingCode2
                        .CostingCode3 = rowOtroGasto.CostingCode3
                        .CostingCode4 = rowOtroGasto.CostingCode4
                        .CostingCode5 = rowOtroGasto.CostingCode5
                    End If
                End With
                oLineaAsientoTemporalList.Add(oLineaAsientoTemporal)
            Next
            'Recorre lineas de objeto temporal para agrupar el definitivo
            For Each rowAsiento1 As Asiento In oLineaAsientoTemporalList
                dblCosto = 0
                blnAgregar = False
                For Each rowAsiento2 As Asiento In oLineaAsientoTemporalList
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.Aplicado = False Then
                        dblCosto += rowAsiento2.Costo
                        rowAsiento2.Aplicado = True
                        If dblCosto > 0 Then
                            blnAgregar = True
                        End If
                    End If
                Next
                If blnAgregar Then
                    oLineaAsiento = New Asiento
                    With oLineaAsiento
                        .NoOrden = rowAsiento1.NoOrden
                        .CuentaDebito = rowAsiento1.CuentaDebito
                        .CuentaCredito = rowAsiento1.CuentaCredito
                        .Costo = dblCosto
                        .Moneda = rowAsiento1.Moneda
                        If rowAsiento1.UsaDimensiones Then
                            .UsaDimensiones = True
                            .CostingCode = rowAsiento1.CostingCode
                            .CostingCode2 = rowAsiento1.CostingCode2
                            .CostingCode3 = rowAsiento1.CostingCode3
                            .CostingCode4 = rowAsiento1.CostingCode4
                            .CostingCode5 = rowAsiento1.CostingCode5
                        End If
                    End With
                    p_oLineaAsientoList.Add(oLineaAsiento)
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ObtenerCuentaArticulo(ByVal p_strItemCode As String, _
                                          ByVal p_strAlmacen As String, _
                                          ByVal p_strValor As String) As String
        Dim oItemArticulo As SAPbobsCOM.IItems
        Try
            '**********Variables****************
            Dim cuentaContable As String = String.Empty

            oItemArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(p_strItemCode)
            '*********Obtiene cuenta según configuración contable del articulo
            Select Case oItemArticulo.GLMethod
                Case SAPbobsCOM.BoGLMethods.glm_WH
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0} FROM OWHS with(nolock) Where WhsCode = '{1}'",
                                                        p_strValor, p_strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)

                Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0}  From OITB with(nolock) Where ItmsGrpCod = '{1}'",
                                                        p_strValor, oItemArticulo.ItemsGroupCode.ToString.Trim()),
                                                        SBO_Company.CompanyDB,
                                                        SBO_Company.Server)
                Case SAPbobsCOM.BoGLMethods.glm_ItemLevel
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0} From OITW with(nolock) Where ItemCode= '{1}' AND WhsCode = '{2}'",
                                                        p_strValor, p_strItemCode, p_strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)
                Case Else
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select {0} FROM OWHS with(nolock) Where WhsCode = '{1}'",
                                                        p_strValor, p_strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)
            End Select
            Return cuentaContable
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            If Not oItemArticulo Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oItemArticulo)
                oItemArticulo = Nothing
            End If
        End Try
    End Function

    Public Sub CargaConfiguracionSucursal(ByRef p_oSucursalList As Generic.List(Of String), _
                                          ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List, _
                                          ByRef p_oBodegaCentroCostoList As BodegaCentroCosto_List)
        Try
            '********Declaración de data contract*************
            Dim oConfiguracionSucursal As ConfiguracionSucursal
            '********Declaración de variables*****************
            Dim oDataTableConfiguracionSucursal As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionSucursal As System.Data.DataRow
            Dim strIDSucursales As String = String.Empty
            Dim blnUsaAsientoServicioExterno As Boolean = False
            Dim intContSucursalList As Integer = 0
            Dim intContTemporal As Integer = 0
            '******************************************************************************
            '******************** Carga Configuración de tabla ConfiguracionSucursal*******
            '******************************************************************************
            intContSucursalList = p_oSucursalList.Count()
            For Each rowSucursal As String In p_oSucursalList
                intContTemporal += 1
                If intContTemporal = intContSucursalList Then
                    strIDSucursales = strIDSucursales & String.Format("'{0}'", rowSucursal)
                Else
                    strIDSucursales = strIDSucursales & String.Format("'{0}', ", rowSucursal)
                End If
            Next
            If (strIDSucursales.Length > 0) Then
                strIDSucursales = strIDSucursales.Substring(0, strIDSucursales.Length - 0)
                oDataTableConfiguracionSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_GenAsSE, U_UsaDimC,U_Sucurs From [@SCGD_CONF_SUCURSAL] with (nolock), dbo.[@SCGD_ADMIN] with (nolock)  Where U_Sucurs in ({0})",
                                                           strIDSucursales),
                                                           SBO_Company.CompanyDB,
                                                           SBO_Company.Server)
                Utilitarios.ObtenerAlmacenXCentroCosto(p_oSucursalList, SBO_Company, p_oBodegaCentroCostoList)
            End If
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            For Each oDataRowConfiguracionSucursal In oDataTableConfiguracionSucursal.Rows
                blnUsaAsientoServicioExterno = False
                oConfiguracionSucursal = New ConfiguracionSucursal()
                With oConfiguracionSucursal
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Sucurs")) Then
                        .SucursalID = oDataRowConfiguracionSucursal.Item("U_Sucurs").ToString.Trim()
                    End If
                    '*********************************************************************
                    '**************Valida si genera asientos servicio externo*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_GenAsSE")) Then
                        If oDataRowConfiguracionSucursal.Item("U_GenAsSE").ToString.Trim() = "Y" Then
                            .UsaAsientoServicioExterno = True
                        Else
                            .UsaAsientoServicioExterno = False
                        End If
                    Else
                        .UsaAsientoServicioExterno = False
                    End If
                    '*********************************************************************
                    '**************Valida si dimensiones*************
                    '*********************************************************************
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_UsaDimC")) Then
                        If oDataRowConfiguracionSucursal.Item("U_UsaDimC").ToString.Trim() = "Y" Then
                            .UsaDimensiones = True
                        Else
                            .UsaDimensiones = False
                        End If
                    Else
                        .UsaDimensiones = False
                    End If
                End With
                p_oConfiguracionSucursalList.Add(oConfiguracionSucursal)
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Private Function DevuelveValorArticulo(ByVal strItemcode As String, _
                                           ByVal strUDfName As String) As String
        Try
            Dim oItemArticulo As SAPbobsCOM.IItems
            Dim valorUDF As String = String.Empty

            oItemArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(strItemcode)
            If oItemArticulo IsNot Nothing Then
                valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value
                If Not String.IsNullOrEmpty(valorUDF) Then
                    Return valorUDF
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Private Sub ValidaUsaDimensionesTipoOT(ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List)
        Try
            '**************Declaración DataContracts****************
            Dim oConfiguracionOrdenTrabajoList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List()
            '**************Declaración de variables******************************
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            ClsLineasDocumentosDimension.ObtieneConfiguracionDimensionesOT(oConfiguracionOrdenTrabajoList)
            For Each rowTipoOT As ConfiguracionOrdenTrabajo In p_oTipoOTList
                For Each rowConfiguracion As ConfiguracionOrdenTrabajo In oConfiguracionOrdenTrabajoList
                    If rowTipoOT.TipoOT = rowConfiguracion.TipoOT Then
                        rowTipoOT.UsaDimensiones = rowConfiguracion.UsaDimensiones
                        rowTipoOT.UsaDimensionAsientoEntradaMercancia = rowConfiguracion.UsaDimensionAsientoEntradaMercancia
                        rowTipoOT.UsaDimensionAsientoFacturaProveedor = rowConfiguracion.UsaDimensionAsientoFacturaProveedor
                        Exit For
                    End If
                Next
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function ConsultaMonedaLocal() As String
        Try
            '*****Variables*******
            Dim strMonedaLocal As String = String.Empty

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM with(nolock)", SBO_Company.CompanyDB, SBO_Company.Server)

            Return strMonedaLocal
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Sub CargarDocEntryCotizacion(ByVal p_oListaNoOrden As Generic.List(Of String), ByRef p_oListaCotizacion As Generic.List(Of String))
        Try
            Dim strNoOrden As String = String.Empty
            Dim strQuery As String = String.Empty
            Dim dtCotizacion As System.Data.DataTable
            Dim intDocEntry As Integer = 0

            For Each rowOT As String In p_oListaNoOrden
                If Not strNoOrden.Contains(rowOT) Then
                    strNoOrden = strNoOrden & String.Format("'{0}', ", rowOT)
                End If
            Next
            If (strNoOrden.Length > 0) Then
                strNoOrden = strNoOrden.Substring(0, strNoOrden.Length - 2)
                strQuery = String.Format("select Q.DocEntry from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT in ({0})", strNoOrden)
                dtCotizacion = Utilitarios.EjecutarConsultaDataTable(strQuery, SBO_Company.CompanyDB, SBO_Company.Server)
            End If
            For Each rowCotizacion As DataRow In dtCotizacion.Rows
                If Not String.IsNullOrEmpty(rowCotizacion.Item("DocEntry")) Then
                    If Not p_oListaCotizacion.Contains(rowCotizacion.Item("DocEntry")) Then
                        p_oListaCotizacion.Add(rowCotizacion.Item("DocEntry"))
                    End If
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
#End Region

#Region "Nuevos metodos actualiza costos y cantidades cotizacion"
    Public Function ProcesaCantidadesyCostosCotizacion(ByRef p_strDocEntry As String, ByRef p_oListCotizacion As List(Of SAPbobsCOM.Documents)) As Boolean
        Dim oLineaEntradaMercanciaList As DocumentoMarketing_List = New DocumentoMarketing_List
        Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
        Dim strDocEntryCotizacion As String = String.Empty
        Dim CancelStatus As SAPbobsCOM.CancelStatusEnum
        Dim oCotizacion As SAPbobsCOM.Documents
        Try
            If CargaDocumentoEntradaMercancia(Convert.ToInt32(p_strDocEntry), oLineaEntradaMercanciaList, oNoOrdenList, CancelStatus) Then
                If oLineaEntradaMercanciaList.Count > 0 Then
                    For Each rowNoOrden As String In oNoOrdenList
                        If Not String.IsNullOrEmpty(rowNoOrden) Then
                            strDocEntryCotizacion = CargaDocEntryCotizacion(rowNoOrden)
                            If Not String.IsNullOrEmpty(strDocEntryCotizacion) Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                If ActualizaCantidadesyCostosCotizacion(strDocEntryCotizacion, oLineaEntradaMercanciaList, CancelStatus, oCotizacion) Then
                                    If Not IsNothing(oCotizacion) Then
                                        p_oListCotizacion.Add(oCotizacion)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            Return True
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function CargaDocEntryCotizacion(ByRef p_strNoOrden As String) As String
        Try
            Dim strQuery As String = String.Empty
            Dim strDocEntryCotizacion As String = String.Empty
            If Not String.IsNullOrEmpty(p_strNoOrden) Then
                strQuery = String.Format("select Q.DocEntry from OQUT Q with (nolock) where Q.U_SCGD_Numero_OT = '{0}'", p_strNoOrden.Trim())
                strDocEntryCotizacion = Utilitarios.EjecutarConsulta(strQuery, SBO_Company.CompanyDB, SBO_Company.Server)
            End If
            Return strDocEntryCotizacion
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return String.Empty
        End Try
    End Function

    Public Function ActualizaCantidadesyCostosCotizacion(ByRef p_strDocEntry As String, _
                                                    ByRef p_oLineaEntradaMercanciaList As DocumentoMarketing_List, ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum, ByRef p_oCotizacion As SAPbobsCOM.Documents) As Boolean

        Try
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim blnActualizaCotizacion As Boolean = False
            Dim CantidadRecibida As Double = 0
            Dim CantidadPendiente As Double = 0
            Dim CantidadSolicitada As Double = 0
            Dim CantidadAbiertaDocumentoCompra As Double = 0
            Dim TipoMovimiento As CalculoCantidades.TipoMovimiento
            Dim CostoOfertaVentas As Double = 0
            Dim CostoDocumentoCompra As Double = 0
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                p_oCotizacion = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                intDocEntry = Convert.ToInt32(p_strDocEntry)
                If p_oCotizacion.GetByKey(intDocEntry) Then
                    For Each rowEntrada As DocumentoMarketing In p_oLineaEntradaMercanciaList
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowEntrada.NoOrden.Trim Then
                                For contador As Integer = 0 To p_oCotizacion.Lines.Count - 1
                                    p_oCotizacion.Lines.SetCurrentLine(contador)
                                    If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim() = rowEntrada.ID.Trim Then
                                        'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = rowEntrada.Costo

                                        If CancelStatus = CancelStatusEnum.csCancellation Then
                                            TipoMovimiento = CalculoCantidades.TipoMovimiento.Cancelacion
                                        Else
                                            TipoMovimiento = CalculoCantidades.TipoMovimiento.Creacion
                                        End If
                                        If blnDocCerrar Then TipoMovimiento = CalculoCantidades.TipoMovimiento.Cierre

                                        CantidadAbiertaDocumentoCompra = rowEntrada.Cantidad
                                        CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                                        CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                        CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value

                                        CalculoCantidades.RecalcularCantidades(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes, TipoMovimiento, True, p_oCotizacion.Lines.Quantity, CantidadAbiertaDocumentoCompra, CantidadSolicitada, CantidadPendiente, CantidadRecibida)

                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CantidadSolicitada
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CantidadPendiente
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CantidadRecibida


                                        CostoOfertaVentas = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                        CostoDocumentoCompra = rowEntrada.Costo

                                        CalculoCantidades.RecalcularCostos(BoObjectTypes.oPurchaseDeliveryNotes, TipoMovimiento, True, p_oCotizacion.Lines.Quantity, CostoOfertaVentas, CantidadAbiertaDocumentoCompra, CostoDocumentoCompra)

                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = CostoOfertaVentas

                                        blnActualizaCotizacion = True
                                        Exit For
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
            End If
            '****************Manejo Transaccion SAP ********************
            'If blnActualizaCotizacion Then
            '    ResetTransaction()
            '    StartTransaction()
            '    If oCotizacion.Update() <> 0 Then
            '        SBO_Application.StatusBar.SetText(String.Format("{0}", SBO_Company.GetLastErrorDescription), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '        SCG.ServicioPostVenta.Utilitarios.ManejadorErrores(New Exception(String.Format("{0}: {1} COTIZACION DMS ONE // OT {2}", SBO_Company.GetLastErrorDescription, p_strDocEntry, IIf(IsNothing(oCotizacion), "", oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim()))), SBO_Application)
            '        RollbackTransaction()
            '    Else
            '        CommitTransaction()
            '    End If
            'End If
            Return True
        Catch ex As Exception
            'blnActualizaCotizacion = False
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function CargaDocumentoEntradaMercancia(ByVal p_intDocEntry As Integer, ByRef p_oLineaEntradaMercanciaList As DocumentoMarketing_List, _
                                                   ByRef p_oNoOrdenList As Generic.List(Of String), ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum) As Boolean
        Dim oEntradaMercancia As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaEntradaMercancia As DocumentoMarketing
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim blnProcesaEntradaMercancia As Boolean = False

            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oEntradaMercancia = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes),  _
                                                     SAPbobsCOM.Documents)
                '************Carga Objeto Entrada Mercancia********************************
                If oEntradaMercancia.GetByKey(p_intDocEntry) Then
                    CancelStatus = oEntradaMercancia.CancelStatus
                    '********Recorre lineas de la Entrada Mercancia***********************
                    For rowEntrada As Integer = 0 To oEntradaMercancia.Lines.Count - 1
                        oEntradaMercancia.Lines.SetCurrentLine(rowEntrada)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oEntradaMercancia.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            oLineaEntradaMercancia = New DocumentoMarketing()
                            With oLineaEntradaMercancia
                                .ItemCode = oEntradaMercancia.Lines.ItemCode
                                .TipoArticulo = intTipoArticulo
                                .Cantidad = oEntradaMercancia.Lines.Quantity
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                .Costo = oEntradaMercancia.Lines.LineTotal
                                If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    .ID = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                            End With
                            p_oLineaEntradaMercanciaList.Add(oLineaEntradaMercancia)
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oEntradaMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            blnProcesaEntradaMercancia = True
                        End If
                    Next
                End If
            End If
            Return blnProcesaEntradaMercancia
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If Not oEntradaMercancia Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oEntradaMercancia)
                oEntradaMercancia = Nothing
            End If
        End Try
    End Function
#End Region

End Class
