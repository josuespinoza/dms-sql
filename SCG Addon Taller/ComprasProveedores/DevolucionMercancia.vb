Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports DMSOneFramework

Public Class DevolucionMercancia

#Region "Definiciones"

    Private WithEvents SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company

    Private mc_strSCGD_NoOT As String = "U_SCGD_NoOT"
    Public Shared EsDevolucionCompras As Boolean = False
#End Region

#Region "Constructor"
    <System.CLSCompliant(False)> _
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

    Private Enum TipoDocumentoMarketing
        OfertaCompra = 540000006
        OrdenCompra = 22
        EntradaMercancia = 20
        FacturaProveedor = 18
        NotaCredito = 19
        DevolucionMercancia = 21
    End Enum
#End Region

#Region "Propiedades"
    Public Property DocEntry() As String
        Get
            Return _strDocEntry
        End Get
        Set(ByVal value As String)
            _strDocEntry = value
        End Set
    End Property
    Public _strDocEntry As String

#End Region

#Region "Manejo de eventos"
    Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent
        Try
            Dim strKey As String = ""
            Dim xmlDocKey As New Xml.XmlDocument

            Select Case BusinessObjectInfo.EventType
                Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD
                    DocEntry = String.Empty
                    If BusinessObjectInfo.ActionSuccess Then
                        Select Case BusinessObjectInfo.FormTypeEx
                            'Oferta de ventas
                            Case "182"
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                If Not String.IsNullOrEmpty(strKey) Then
                                    DocEntry = strKey
                                End If
                        End Select
                    End If
            End Select
        Catch ex As Exception
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                 ByVal FormUID As String, _
                                                 ByRef BubbleEvent As Boolean)
        Try
            Dim oForm As SAPbouiCOM.Form

            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)

            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    If pval.ItemUID = "1" AndAlso oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        Dim str As String = oForm.DataSources.DBDataSources.Item("ORPD").GetValue("DocEntry", 0)
                        EsDevolucionCompras = True
                        CalculoCantidades.AccionSeleccionada = False
                        CalculoCantidades.AbreDocumentos = False
                    End If
                ElseIf pval.ActionSuccess Then
                    EsDevolucionCompras = False
                    Select Case pval.FormMode
                        Case pval.ItemUID = "1" And SAPbouiCOM.BoFormMode.fm_ADD_MODE
                            'Generar asiento anulación asiento entrada mercancia
                            If Not String.IsNullOrEmpty(DocEntry) Then
                                CalculoCantidades.ExisteOrdenCompra = ExisteOrdenCompra(DocEntry)
                                ProcesaCantidadesyCostosCotizacion(DocEntry)
                                ProcesaDevolucionMercancia(DocEntry)
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

    Public Function ExisteOrdenCompra(ByVal DocEntry As Integer) As Boolean
        Dim Query As String = "SELECT COUNT(*) AS ""Cuenta"" FROM ""RPD1"" T0 WITH (nolock) INNER JOIN ""PDN1"" T1 WITH (nolock) ON T0.""BaseEntry"" = T1.""DocEntry"" AND T0.""BaseLine"" = T1.""LineNum"" AND T0.""BaseType"" = T1.""ObjType"" WHERE T0.""DocEntry"" = '{0}' AND T0.""BaseType"" = '20' AND T1.""BaseType"" = '22'"
        Dim Cuenta As Integer = 0
        Try
            ExisteOrdenCompra = False
            Query = String.Format(Query, DocEntry)
            Cuenta = DMS_Connector.Helpers.EjecutarConsulta(Query)
            If Cuenta > 0 Then
                ExisteOrdenCompra = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

#End Region


    Public Sub ProcesaDevolucionMercancia(ByRef p_strDocEntry As String)
        Try
            '**********DataContract****************
            Dim oConfiguracionSucursalList As ConfiguracionSucursal_List = New ConfiguracionSucursal_List
            Dim oLineaDevolucionMercanciaList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oServicioExternoList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oDimensionesContablesList As DimensionesContables_List = New DimensionesContables_List
            Dim oDatosGeneralesList As DatoGenerico_List = New DatoGenerico_List
            Dim oAsientoServicioExternoList As Asiento_List = New Asiento_List
            '********Listas genericas*************
            Dim oSucursalList As List(Of String) = New Generic.List(Of String)
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim oCodigoMarcaList As List(Of String) = New Generic.List(Of String)
            Dim oTipoOTList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List
            '*************Clases**************************
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            '**********Declaración Variables*****************
            Dim blnProcesaDevolucionMercancia As Boolean = False
            Dim strMonedaLocal As String = String.Empty
            Dim blnDimensionesYaCargadas As Boolean = False
            Dim blnAsientoServicioExternoExitoso As Boolean = False
            Dim blnMensajeServicioExternoExitoso As Boolean = False
            Dim clsDocumentoProcesoCompra As DocumentoProcesoCompra = New DocumentoProcesoCompra(SBO_Company, SBO_Application)
            blnProcesaDevolucionMercancia = CargaDevolucionMercancia(CInt(p_strDocEntry), oLineaDevolucionMercanciaList, oSucursalList, oNoOrdenList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList)
            '********Valida si existen lineas en la factura de clientes que sean de tipo(Servicio Externo) que esten ligadas a una OT y que necesite procesar para saber si genera asiento*************
            If blnProcesaDevolucionMercancia Then
                '********Carga configuración sucursal*************
                If oSucursalList.Count > 0 Then
                    CargaConfiguracionSucursal(oSucursalList, oConfiguracionSucursalList)
                End If
                '********Si a nivel de compañia se usan dimensiones, valida si lo hace a nivel de Tipo OT*************
                For Each rowConfiguracionSucursal As ConfiguracionSucursal In oConfiguracionSucursalList
                    If rowConfiguracionSucursal.UsaAsientoServicioExterno Then
                        If rowConfiguracionSucursal.UsaDimensiones Then
                            If oTipoOTList.Count > 0 Then
                                ValidaUsaDimensionesTipoOT(oTipoOTList)
                            End If
                            If Not blnDimensionesYaCargadas Then
                                ClsLineasDocumentosDimension.CargaCentrosCostoDimensionesOT(oSucursalList, oCodigoMarcaList, oDimensionesContablesList)
                                blnDimensionesYaCargadas = True
                            End If
                        End If
                        CargaListasTipoArticulo(oLineaDevolucionMercanciaList, oServicioExternoList, oTipoOTList, rowConfiguracionSucursal, oDimensionesContablesList)
                    End If
                Next

                clsDocumentoProcesoCompra.ManejarTracking(oNoOrdenList, oLineaDevolucionMercanciaList, oDatosGeneralesList, TipoDocumentoMarketing.DevolucionMercancia)

                If DMS_Connector.Configuracion.ParamGenAddon.U_GenAsSE = "Y" Then ProcesaAsientoServicioExterno(oServicioExternoList, oAsientoServicioExternoList)
                If oAsientoServicioExternoList.Count() > 0 Then
                    '****************Maneja transacción**************
                    ResetTransaction()
                    '************Verifica si genera asiento para servicio externo****************
                    If oAsientoServicioExternoList.Count > 0 Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesaAsiento, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning)
                        If CrearAsiento(oDatosGeneralesList, oAsientoServicioExternoList, TipoArticulo.ServicioExterno) > 0 Then
                            blnAsientoServicioExternoExitoso = True
                            blnMensajeServicioExternoExitoso = True
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Sub
                        End If
                    Else
                        blnAsientoServicioExternoExitoso = True
                        blnMensajeServicioExternoExitoso = False
                    End If

                    If blnAsientoServicioExternoExitoso Then
                        '*****************Realiza commit ala transaccion**************
                        CommitTransaction()
                        '*****************Mensaje asiento generado correctamente*****************
                        If blnMensajeServicioExternoExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                    End If
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            RollbackTransaction()
        End Try
    End Sub

    Public Function CrearAsiento(ByRef p_oDatosGeneralesList As DatoGenerico_List, _
                                ByRef p_oAsientoList As Asiento_List, _
                                ByVal p_intTipoArticulo As Integer) As Integer
        Try
            '************Objetos*********************
            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            '************Variables*******************
            Dim intAsientoGenerado As Integer = 0
            Dim strAsientoGenerado As String = String.Empty
            Dim intDocEntry As Integer = 0
            Dim intDocNum As Integer = 0
            Dim dateFechaContabilizacion As Date = Nothing
            Dim strMonedaLocal As String = String.Empty
            Dim intError As Integer = 0
            Dim strMensajeError As String = String.Empty
            Dim strReference2 As String

            For Each rowGeneral As DatoGenerico In p_oDatosGeneralesList
                With rowGeneral
                    intDocEntry = .DocEntry
                    intDocNum = .DocNum
                    dateFechaContabilizacion = .FechaContabilizacion
                    strMonedaLocal = .MonedaLocal
                    strReference2 = .BaseEntry.ToString()
                End With
                Exit For
            Next

            If p_oAsientoList.Count > 0 Then
                oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                If Not dateFechaContabilizacion = Nothing Then
                    oJournalEntry.ReferenceDate = dateFechaContabilizacion
                End If               
                oJournalEntry.Reference2 = strReference2
                oJournalEntry.Memo = My.Resources.Resource.RegistroDevolucionSE + intDocNum.ToString()

                For Each rowAsiento As Asiento In p_oAsientoList
                    '*********************
                    'Cuenta Credito
                    '*********************
                    oJournalEntry.Lines.AccountCode = rowAsiento.CuentaCredito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        oJournalEntry.Lines.Credit = rowAsiento.Costo
                    Else
                        oJournalEntry.Lines.FCCredit = rowAsiento.Costo
                        oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference2 = strReference2
                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(rowAsiento.IDSucursal) Then oJournalEntry.Lines.BPLID = rowAsiento.IDSucursal
                    End If
                    If rowAsiento.UsaDimensiones Then
                        oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    oJournalEntry.Lines.Add()

                    '*****************
                    'Cuenta Debito
                    '*****************
                    oJournalEntry.Lines.AccountCode = rowAsiento.CuentaDebito

                    If rowAsiento.Moneda = strMonedaLocal Or rowAsiento.Moneda = Nothing Then
                        oJournalEntry.Lines.Debit = rowAsiento.Costo
                    Else
                        oJournalEntry.Lines.FCDebit = rowAsiento.Costo
                        oJournalEntry.Lines.FCCurrency = rowAsiento.Moneda
                    End If

                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference1 = rowAsiento.NoOrden
                    oJournalEntry.Lines.Reference2 = strReference2
                    If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If Not String.IsNullOrEmpty(rowAsiento.IDSucursal) Then oJournalEntry.Lines.BPLID = rowAsiento.IDSucursal
                    End If
                    If rowAsiento.UsaDimensiones Then
                        oJournalEntry.Lines.CostingCode = rowAsiento.CostingCode
                        oJournalEntry.Lines.CostingCode2 = rowAsiento.CostingCode2
                        oJournalEntry.Lines.CostingCode3 = rowAsiento.CostingCode3
                        oJournalEntry.Lines.CostingCode4 = rowAsiento.CostingCode4
                        oJournalEntry.Lines.CostingCode5 = rowAsiento.CostingCode5
                    End If

                    oJournalEntry.Lines.Add()
                Next

                If oJournalEntry.Add <> 0 Then
                    intAsientoGenerado = 0
                    SBO_Company.GetLastError(intError, strMensajeError)
                    Throw New ExceptionsSBO(intError, strMensajeError)
                Else
                    SBO_Company.GetNewObjectCode(strAsientoGenerado)
                    If Not String.IsNullOrEmpty(strAsientoGenerado) Then
                        intAsientoGenerado = CInt(strAsientoGenerado)
                    Else
                        intAsientoGenerado = 0
                    End If
                End If
            End If
            Return intAsientoGenerado
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function

    Public Sub ProcesaAsientoServicioExterno(ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                             ByRef p_oLineaAsientoList As Asiento_List)
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
                    'If Not String.IsNullOrEmpty(rowServicioExterno.ItemCode) And Not String.IsNullOrEmpty(rowServicioExterno.BodegaOrigen) Then
                    '    strCuentaDebito = ObtenerCuentaArticulo(rowServicioExterno.ItemCode, rowServicioExterno.BodegaOrigen, "TransferAc")
                    '    strCuentaCredito = ObtenerCuentaArticulo(rowServicioExterno.ItemCode, rowServicioExterno.BodegaOrigen, "ExpensesAc")
                    'End If
                    strCuentaDebito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.TransferAc, rowServicioExterno.Sucursal, rowServicioExterno.BodegaOrigen)
                    strCuentaCredito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, rowServicioExterno.Sucursal, rowServicioExterno.BodegaOrigen)
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

    Public Function ObtenerCuentaArticulo(ByVal p_strItemCode As String, _
                                          ByVal p_strAlmacen As String, _
                                          ByVal p_strValor As String) As String
        '********Valor campos********
        'Cuenta costo de ventas= SaleCostAc --- Mano de Obra
        '********Valor campos********
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

    Public Sub CargaListasTipoArticulo(ByRef p_oLineaDevolucionMercanciaList As DocumentoMarketing_List, _
                                      ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                      ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                      ByRef p_rowConfiguracionSucursal As ConfiguracionSucursal, _
                                      ByRef p_oDimensionesContablesList As DimensionesContables_List)
        Try
            '**************Declaracion de data contract**********
            Dim oServicioExterno As DocumentoMarketing
            Dim oServicio As DocumentoMarketing
            Dim oOtroGasto As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            '************Variables********************************

            For Each rowLineaFactura As DocumentoMarketing In p_oLineaDevolucionMercanciaList
                '********************Valida si la sucursal es la misma de la cual se esta recorriendo************
                If rowLineaFactura.Sucursal = p_rowConfiguracionSucursal.SucursalID Then
                    '************Según tipo de articulo valida que lista cargar********************************
                    Select Case rowLineaFactura.TipoArticulo
                        Case TipoArticulo.ServicioExterno
                            If p_rowConfiguracionSucursal.UsaAsientoServicioExterno Then
                                oServicioExterno = New DocumentoMarketing()
                                With oServicioExterno
                                    .ItemCode = rowLineaFactura.ItemCode
                                    .BodegaOrigen = rowLineaFactura.BodegaOrigen
                                    .TipoArticulo = rowLineaFactura.TipoArticulo
                                    .NoOrden = rowLineaFactura.NoOrden
                                    .TipoOT = rowLineaFactura.TipoOT
                                    .CodigoProyecto = rowLineaFactura.CodigoProyecto
                                    .Costo = rowLineaFactura.Costo
                                    .Sucursal = rowLineaFactura.Sucursal
                                    .CodigoMarca = rowLineaFactura.CodigoMarca
                                    '*********************Valida que usa dimensiones y asigna centro de costo*********
                                    If p_rowConfiguracionSucursal.UsaDimensiones Then
                                        AsignaCentrosCostoDimensiones(rowLineaFactura, oServicioExterno, p_oTipoOTList, p_oDimensionesContablesList)
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

    Public Sub CargaConfiguracionSucursal(ByRef p_oSucursalList As Generic.List(Of String), _
                                         ByRef p_oConfiguracionSucursalList As ConfiguracionSucursal_List)
        Try
            '********Declaración de data contract*************
            Dim oConfiguracionSucursal As ConfiguracionSucursal
            '********Declaración de variables*****************
            Dim oDataTableConfiguracionSucursal As System.Data.DataTable = Nothing
            Dim oDataRowConfiguracionSucursal As System.Data.DataRow
            Dim strIDSucursales As String = String.Empty
            Dim blnUsaCosteoManoObra As Boolean = False
            Dim blnUsaAientoOtroGasto As Boolean = False
            Dim blnUsaAsientoServicioExterno As Boolean = False
            Dim intContSucursalList As Integer = 0
            Dim intContTemporal As Integer = 0
            '******************************************************************************
            '******************** Carga Configuración de tabla ConfiguracionSucursal*******
            '******************************************************************************
            intContSucursalList = p_oSucursalList.Count()
            For Each rowSucursal As String In p_oSucursalList
                intContTemporal += 1
                If Not strIDSucursales.Contains(rowSucursal) Then
                    If intContTemporal = intContSucursalList Then
                        strIDSucursales = strIDSucursales & String.Format("'{0}'", rowSucursal)
                    Else
                        strIDSucursales = strIDSucursales & String.Format("'{0}', ", rowSucursal)
                    End If
                End If
            Next
            If (strIDSucursales.Length > 0) Then
                strIDSucursales = strIDSucursales.Substring(0, strIDSucursales.Length - 0)
                oDataTableConfiguracionSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_GenAsSE, U_UsaDimC,U_Sucurs,U_CosteoMO_C, U_TiempoEst_C, U_TiempoReal_C, U_Moneda_C, U_CuentaSys_C, U_GenASGastos,U_MonDocGastos,U_CtaDebGast From [@SCGD_CONF_SUCURSAL] with (nolock), dbo.[@SCGD_ADMIN] with (nolock)  Where U_Sucurs in ({0})",
                                                           strIDSucursales),
                                                           SBO_Company.CompanyDB,
                                                           SBO_Company.Server)
            End If
            '******************************************************************************
            '******************** Recorre configuraciones y agrega a objeto list*******
            '******************************************************************************
            For Each oDataRowConfiguracionSucursal In oDataTableConfiguracionSucursal.Rows
                blnUsaCosteoManoObra = False
                blnUsaAsientoServicioExterno = False
                blnUsaAientoOtroGasto = False
                oConfiguracionSucursal = New ConfiguracionSucursal()
                With oConfiguracionSucursal
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Sucurs")) Then
                        .SucursalID = oDataRowConfiguracionSucursal.Item("U_Sucurs").ToString.Trim()
                    End If
                    '****************************************************
                    '*********Valida si costea mano de obra**************
                    '****************************************************
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CosteoMO_C")) Then
                        If oDataRowConfiguracionSucursal.Item("U_CosteoMO_C") = "Y" Then
                            .UsaCosteoManoObra = True
                            blnUsaCosteoManoObra = True
                        Else
                            .UsaCosteoManoObra = False
                        End If
                    Else
                        .UsaCosteoManoObra = False
                    End If

                    If blnUsaCosteoManoObra Then
                        '*******************Valida Moneda Costo Mano Obra****************
                        If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Moneda_C")) Then
                            .MonedaManoObra = oDataRowConfiguracionSucursal.Item("U_Moneda_C").ToString.Trim()
                        Else
                            .UsaCosteoManoObra = False
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaMonedaMO, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If

                        '*************Valida Cuenta Costo Mano Obra****************
                        If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CuentaSys_C")) Then
                            .CuentaCreditoManoObra = oDataRowConfiguracionSucursal.Item("U_CuentaSys_C").ToString.Trim()
                        Else
                            .UsaCosteoManoObra = False
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaCuentaCreditoMO, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    End If
                    '**********************************************************
                    '**************Valida si genera asiento gastos*************
                    '**********************************************************
                    If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_GenASGastos")) Then
                        If oDataRowConfiguracionSucursal.Item("U_GenASGastos") = "Y" Then
                            .UsaAsientosGastos = True
                            blnUsaAientoOtroGasto = True
                        Else
                            .UsaAsientosGastos = False
                        End If
                    Else
                        .UsaAsientosGastos = False
                    End If
                    If blnUsaAientoOtroGasto Then
                        'Valida Moneda Otros Costos
                        If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_MonDocGastos")) Then
                            .MonedaOtrosGastos = oDataRowConfiguracionSucursal.Item("U_MonDocGastos").ToString.Trim()
                        Else
                            .UsaAsientosGastos = False
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfOtrosGastos, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                        'Valida Cuenta Credito Otros Gastos
                        If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_CtaDebGast")) Then
                            .CuentaCreditoOtrosGastos = oDataRowConfiguracionSucursal.Item("U_CtaDebGast").ToString.Trim()
                        Else
                            .UsaAsientosGastos = False
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfOtrosGastos, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
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

    Public Function CargaDevolucionMercancia(ByVal p_intDocEntry As Integer, _
                                             ByRef p_oLineaDevolucionMercanciaList As DocumentoMarketing_List, _
                                             ByRef p_oSucursalList As Generic.List(Of String), _
                                             ByRef p_oNoOrdenList As Generic.List(Of String), _
                                             ByRef p_oCodigoMarcaList As Generic.List(Of String), _
                                             ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                             ByRef p_oDatosGeneralesList As DatoGenerico_List) As Boolean
        Dim oDevolucionMercancia As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaDevolucionMercancia As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            Dim oDatosGenerales As DatoGenerico
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strSucursal As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim strCodigoMarca As String = String.Empty
            Dim blnProcesoAsientoDevolucionMercancia As Boolean = False
            Dim strMonedaLocal As String = String.Empty

            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oDevolucionMercancia = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns),  _
                                                     SAPbobsCOM.Documents)
                '************Carga Objeto Factura de clientes********************************
                If oDevolucionMercancia.GetByKey(p_intDocEntry) Then
                    oDatosGenerales = New DatoGenerico
                    With oDatosGenerales
                        .DocEntry = oDevolucionMercancia.DocEntry
                        .DocNum = oDevolucionMercancia.DocNum
                        .FechaContabilizacion = oDevolucionMercancia.DocDate
                        oDevolucionMercancia.Lines.SetCurrentLine(0)
                        .BaseEntry = oDevolucionMercancia.Lines.BaseEntry
                    End With
                    p_oDatosGeneralesList.Add(oDatosGenerales)
                    '********Recorre lineas de la factura***********************
                    For rowDevolucionMercancia As Integer = 0 To oDevolucionMercancia.Lines.Count - 1
                        oDevolucionMercancia.Lines.SetCurrentLine(rowDevolucionMercancia)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strSucursal = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oDevolucionMercancia.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            oLineaDevolucionMercancia = New DocumentoMarketing()
                            With oLineaDevolucionMercancia
                                .ItemCode = oDevolucionMercancia.Lines.ItemCode
                                .BodegaOrigen = oDevolucionMercancia.Lines.WarehouseCode
                                .TipoArticulo = intTipoArticulo
                                .Costo = oDevolucionMercancia.Lines.LineTotal
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    .TipoOT = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                ElseIf Not String.IsNullOrEmpty(oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                    .TipoOT = oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value) Then
                                    .CodigoProyecto = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()) Then
                                    .Sucursal = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                ElseIf Not String.IsNullOrEmpty(oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                    .Sucursal = oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value.ToString()) Then
                                    .CodigoMarca = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                ElseIf Not String.IsNullOrEmpty(oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                    .CodigoMarca = oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    .ID = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()
                                End If
                            End With
                            p_oLineaDevolucionMercanciaList.Add(oLineaDevolucionMercancia)

                            '***************Agrega Sucursal al List*************************
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                strSucursal = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()
                                If Not p_oSucursalList.Contains(strSucursal) Then
                                    p_oSucursalList.Add(strSucursal)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                strSucursal = oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()
                                If Not p_oSucursalList.Contains(strSucursal) Then
                                    p_oSucursalList.Add(strSucursal)
                                End If
                            End If
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            '**************Agrega Codigo Marca al List******************
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value) Then
                                strCodigoMarca = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                    p_oCodigoMarcaList.Add(strCodigoMarca)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                strCodigoMarca = oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                    p_oCodigoMarcaList.Add(strCodigoMarca)
                                End If
                            End If
                            '**************Agrega TipoOT al List******************
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                oTipoOT = New ConfiguracionOrdenTrabajo
                                With oTipoOT
                                    .TipoOT = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                End With
                                If Not p_oTipoOTList.Contains(oTipoOT) Then
                                    p_oTipoOTList.Add(oTipoOT)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                oTipoOT = New ConfiguracionOrdenTrabajo
                                With oTipoOT
                                    .TipoOT = oDevolucionMercancia.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                End With
                                If Not p_oTipoOTList.Contains(oTipoOT) Then
                                    p_oTipoOTList.Add(oTipoOT)
                                End If
                            End If
                            blnProcesoAsientoDevolucionMercancia = True
                        End If
                    Next
                End If
            End If
            Return blnProcesoAsientoDevolucionMercancia
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If Not oDevolucionMercancia Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDevolucionMercancia)
                oDevolucionMercancia = Nothing
            End If
        End Try
    End Function

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

    Public Sub AsignaCentrosCostoDimensiones(ByRef p_rowLineaDevolucion As DocumentoMarketing, _
                                            ByRef p_oListaTipoArticulo As DocumentoMarketing, _
                                            ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                            ByRef p_oDimensionesContablesList As DimensionesContables_List)
        Try
            For Each rowTipoOT As ConfiguracionOrdenTrabajo In p_oTipoOTList
                If p_rowLineaDevolucion.TipoOT = rowTipoOT.TipoOT Then
                    If rowTipoOT.UsaDimensiones And rowTipoOT.UsaDimensionAsientoEntradaMercancia Then
                        For Each rowDimensionesContables As DimensionesContables In p_oDimensionesContablesList
                            If p_rowLineaDevolucion.Sucursal = rowDimensionesContables.Sucursal And p_rowLineaDevolucion.CodigoMarca = rowDimensionesContables.CodigoMarca Then
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

    Public Sub ProcesaCantidadesyCostosCotizacion(ByRef p_strDocEntry As String)
        Try
            Dim oLineaDevolucionMercanciaList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim strDocEntryCotizacion As String = String.Empty
            Dim CancelStatus As SAPbobsCOM.CancelStatusEnum

            If CargaDocumentoDevolucionMercancia(Convert.ToInt32(p_strDocEntry), oLineaDevolucionMercanciaList, oNoOrdenList, CancelStatus) Then
                If oLineaDevolucionMercanciaList.Count > 0 Then
                    For Each rowNoOrden As String In oNoOrdenList
                        If Not String.IsNullOrEmpty(rowNoOrden) Then
                            strDocEntryCotizacion = CargaDocEntryCotizacion(rowNoOrden)
                            If Not String.IsNullOrEmpty(strDocEntryCotizacion) Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaCotizacion, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                ActualizaCantidadesyCostosCotizacion(strDocEntryCotizacion, oLineaDevolucionMercanciaList, CancelStatus)
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function CargaDocumentoDevolucionMercancia(ByVal p_intDocEntry As Integer, _
                                                   ByRef p_oLineaFacturaProveedorList As DocumentoMarketing_List, _
                                                   ByRef p_oNoOrdenList As Generic.List(Of String), ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum) As Boolean
        Dim oDevolucionMercancia As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaDevolucionMercancia As DocumentoMarketing
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim blnProcesaDevolucionMercancia As Boolean = False

            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oDevolucionMercancia = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns),  _
                                                SAPbobsCOM.Documents)
                '************Carga Objeto Devolucion Mercancia********************************
                If oDevolucionMercancia.GetByKey(p_intDocEntry) Then
                    CancelStatus = oDevolucionMercancia.CancelStatus
                    '********Recorre lineas de la Devolucion de mercancia***********************
                    For rowDevolucion As Integer = 0 To oDevolucionMercancia.Lines.Count - 1
                        oDevolucionMercancia.Lines.SetCurrentLine(rowDevolucion)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oDevolucionMercancia.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            oLineaDevolucionMercancia = New DocumentoMarketing()
                            With oLineaDevolucionMercancia
                                .ItemCode = oDevolucionMercancia.Lines.ItemCode
                                .TipoArticulo = intTipoArticulo
                                .Cantidad = oDevolucionMercancia.Lines.Quantity
                                .VisOrder = oDevolucionMercancia.Lines.VisualOrder
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                .Costo = oDevolucionMercancia.Lines.LineTotal
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    .ID = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                    .Sucursal = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                End If
                                If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    .TipoOT = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                End If
                            End With
                            p_oLineaFacturaProveedorList.Add(oLineaDevolucionMercancia)
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oDevolucionMercancia.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            blnProcesaDevolucionMercancia = True
                        End If
                    Next
                End If
            End If
            Return blnProcesaDevolucionMercancia
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If Not oDevolucionMercancia Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDevolucionMercancia)
                oDevolucionMercancia = Nothing
            End If
        End Try
    End Function

    Public Function CargaDocEntryCotizacion(ByRef p_strNoOrden As String) As String
        Try
            Dim strQuery As String = String.Empty
            Dim strDocEntryCotizacion As String = String.Empty
            If Not String.IsNullOrEmpty(p_strNoOrden) Then
                strQuery = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strDocEntryCotizacionxNoOrden"), p_strNoOrden.Trim())
                strDocEntryCotizacion = Utilitarios.EjecutarConsulta(strQuery)
            End If
            Return strDocEntryCotizacion
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return String.Empty
        End Try
    End Function

    Public Sub ActualizaCantidadesyCostosCotizacion(ByRef p_strDocEntry As String, ByRef p_oLineaDevolucionMercanciaList As DocumentoMarketing_List, ByRef CancelStatus As SAPbobsCOM.CancelStatusEnum)
        Dim oCotizacion As Documents
        Dim oDocumento As DMS_Connector.Business_Logic.DataContract.SAPDocumento.oDocumento
        Dim strIDSucur As String
        Dim strTipoOT As String
        Dim strID As String
        Dim intPosicion As Integer
        Dim CantidadOfertaVentas As Double = 0
        Dim CantidadRecibida As Double = 0
        Dim CantidadPendiente As Double = 0
        Dim CantidadSolicitada As Double = 0
        Dim CantidadAbiertaDocumentoCompra As Double = 0
        Dim TipoMovimiento As CalculoCantidades.TipoMovimiento
        Dim CostoOfertaVentas As Double = 0
        Dim CostoDocumentoCompra As Double = 0
        Try
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim blnActualizaCotizacion As Boolean = False
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                intDocEntry = Convert.ToInt32(p_strDocEntry)
                oCotizacion = Nothing
                oDocumento = DMS_Connector.Helpers.CargaCotizacionConVisOrder(intDocEntry, oCotizacion)
                If Not IsNothing(oCotizacion) Then
                    For Each rowDevolucion As DocumentoMarketing In p_oLineaDevolucionMercanciaList
                        If Not String.IsNullOrEmpty(oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim()) Then
                            If oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowDevolucion.NoOrden.Trim Then
                                strID = rowDevolucion.ID.Trim()
                                intPosicion = DMS_Connector.Helpers.GetLinePosition(oDocumento.Lineas, strID)
                                If intPosicion <> -1 Then
                                    oCotizacion.Lines.SetCurrentLine(intPosicion)
                                    CantidadAbiertaDocumentoCompra = rowDevolucion.Cantidad
                                    CantidadOfertaVentas = oCotizacion.Lines.Quantity
                                    CantidadSolicitada = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value
                                    CantidadPendiente = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value
                                    CantidadRecibida = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value

                                    If CancelStatus = CancelStatusEnum.csCancellation Then
                                        TipoMovimiento = CalculoCantidades.TipoMovimiento.Cancelacion
                                    Else
                                        TipoMovimiento = CalculoCantidades.TipoMovimiento.Creacion
                                    End If

                                    Select Case rowDevolucion.TipoArticulo
                                        Case TipoArticulo.ServicioExterno
                                            CalculoCantidades.RecalcularCantidades(SAPbobsCOM.BoObjectTypes.oPurchaseReturns, TipoMovimiento, True, CantidadOfertaVentas, CantidadAbiertaDocumentoCompra, CantidadSolicitada, CantidadPendiente, CantidadRecibida)
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CantidadRecibida
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CantidadSolicitada
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CantidadPendiente

                                            If oCotizacion.Lines.Quantity = rowDevolucion.Cantidad Then
                                                'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "N"
                                            Else
                                                'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value -= rowDevolucion.Costo
                                                If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = oCotizacion.Lines.Quantity Then
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "N"
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = String.Empty
                                                End If
                                            End If
                                        Case TipoArticulo.Repuesto
                                            CalculoCantidades.RecalcularCantidades(SAPbobsCOM.BoObjectTypes.oPurchaseReturns, TipoMovimiento, True, CantidadOfertaVentas, CantidadAbiertaDocumentoCompra, CantidadSolicitada, CantidadPendiente, CantidadRecibida)
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CantidadRecibida
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CantidadSolicitada
                                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CantidadPendiente

                                            If oCotizacion.Lines.Quantity = rowDevolucion.Cantidad Then
                                                'oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = 0
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "N"
                                                oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = String.Empty
                                            Else
                                                If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = oCotizacion.Lines.Quantity Then
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "N"
                                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Resultado").Value = String.Empty
                                                End If
                                            End If
                                    End Select
                                    CostoOfertaVentas = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                    CostoDocumentoCompra = rowDevolucion.Costo

                                    CalculoCantidades.RecalcularCostos(BoObjectTypes.oPurchaseReturns, TipoMovimiento, True, oCotizacion.Lines.Quantity, CostoOfertaVentas, CantidadAbiertaDocumentoCompra, CostoDocumentoCompra)

                                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = CostoOfertaVentas

                                    blnActualizaCotizacion = True
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            '****************Manejo Transaccion SAP ********************
            If blnActualizaCotizacion Then
                ResetTransaction()
                StartTransaction()
                If oCotizacion.Update() <> 0 Then
                    SBO_Application.StatusBar.SetText(String.Format("{0}", SBO_Company.GetLastErrorDescription), SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    SCG.ServicioPostVenta.Utilitarios.ManejadorErrores(New Exception(String.Format("{0}: {1}", SBO_Company.GetLastErrorDescription, p_strDocEntry)), SBO_Application)
                    RollbackTransaction()
                Else
                    CommitTransaction()
                End If
            End If
        Catch ex As Exception
            RollbackTransaction()
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            If Not oCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
        End Try
    End Sub

    Public Sub StartTransaction()
        Try
            If Not SBO_Company.InTransaction Then
                SBO_Company.StartTransaction()
            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
End Class
