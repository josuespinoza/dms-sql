Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports SCG.Requisiciones.UI
Imports DMSOneFramework.SCGBL.Requisiciones

Public Class NotaCreditoClientes

#Region "Definiciones"


    Private WithEvents SBO_Application As SAPbouiCOM.Application

    Private SBO_Company As SAPbobsCOM.Company

    Public n As NumberFormatInfo

    Private strNoAsiento As String = ""

    Private _CreaAsiento As Boolean
    Private _FormNotCredito As SAPbouiCOM.Form
    Private DocEntryFacturaEnAsiento As String = String.Empty

    Private m_oOrdenVenta As SAPbobsCOM.Documents
    Private m_oOrdenVentaLines As SAPbobsCOM.Document_Lines

    Private ListaTargetEntry As Generic.List(Of Integer) = New Generic.List(Of Integer)
    Private blnUsaDimensiones As Boolean = False

    Private ListaNotaCreditoOrdenTrabajo As List(Of LineasNotaCreditoOT)
    Private ListaOrdenesTrabajo As Generic.List(Of String)
    Private _DocEntry As String
    Private strGenerarRequisicionDevolucion As String = String.Empty


#End Region

#Region "Constructor"
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)

        SBO_Application = SBOAplication
        SBO_Company = ocompany
        n = DIHelper.GetNumberFormatInfo(SBO_Company)

    End Sub

#End Region

#Region "Propiedades"

    Public Property DocEntry As String
        Get
            Return _DocEntry
        End Get
        Set(value As String)
            _DocEntry = value
        End Set
    End Property


    Public Property CreaAsiento As Boolean
        Get
            Return _CreaAsiento
        End Get
        Set(ByVal value As Boolean)
            _CreaAsiento = value
        End Set
    End Property

    Public Property FormNotCredito As Form
        Get
            Return _FormNotCredito
        End Get
        Set(ByVal value As Form)
            _FormNotCredito = value
        End Set
    End Property


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
                            'Nota de crédito clientes
                            Case "179"
                                xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
                                Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
                                If Not String.IsNullOrEmpty(strKey) Then
                                    DocEntry = strKey
                                End If
                        End Select
                    End If
            End Select
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub



    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                ByVal FormUID As String, _
                                                ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim ExisteDataSource As Boolean


        Try

            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)


            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    'Evento para el boton crear factura 
                    If pval.ItemUID = "1" AndAlso oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        'se habilita para crear el asiento 
                        CreaAsiento = True
                        FormNotCredito = oForm
                        strGenerarRequisicionDevolucion = oForm.DataSources.DBDataSources.Item("ORIN").GetValue("U_SCGD_GenReqDev", 0).ToString()
                    End If
                Else
                    'Evento para el boton crear factura 
                    If pval.ItemUID = "1" AndAlso oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        'Después de crear la nota de crédito se crea una requisición de devolución en caso de existir movimientos de inventario
                        'ya sea de repuestos o suministros
                        If Not String.IsNullOrEmpty(DocEntry) Then
                            If ObtenerCampoConfiguracionGeneral("U_GenReqDev").ToUpper().Equals("Y") Or strGenerarRequisicionDevolucion.ToUpper().Equals("Y") Then
                                GenerarDevoluciones(DocEntry)
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        Finally
            If pval.ActionSuccess Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
            End If
        End Try

    End Sub

    ''' <summary>
    ''' Obtiene un campo desde la configuración general de DMS
    ''' </summary>
    ''' <param name="strNombreCampo">Nombre del UDF en formato texto. Ejemplo: U_GenReqDev</param>
    ''' <returns>Valor del campo en formato texto.</returns>
    ''' <remarks></remarks>
    Private Function ObtenerCampoConfiguracionGeneral(ByVal strNombreCampo As String) As String
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim strCode As String = "DMS" 'Solamente existe una configuración general
        Dim strValor As String = String.Empty

        Try

            oCompanyService = SBO_Company.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_ADMIN")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", strCode)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            strValor = oGeneralData.GetProperty("U_GenReqDev").ToString()

            Return strValor

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Function

    Public Function FinalizaTransaccion(Optional ByVal p_DocEntry As String = "") As Boolean

        'inicio de transacciones 
        Dim target As Integer = 0
        Try

            If Not String.IsNullOrEmpty(FormNotCredito.DataSources.DBDataSources.Item("ORIN").GetValue("DocDate", 0)) AndAlso
                            Not String.IsNullOrEmpty(FormNotCredito.DataSources.DBDataSources.Item("ORIN").GetValue("CardCode", 0)) AndAlso
                            FormNotCredito.DataSources.DBDataSources.Item("RIN1").Size > 0 Then
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If

                For i As Integer = 0 To FormNotCredito.DataSources.DBDataSources.Item("RIN1").Size - 1


                    If Not String.IsNullOrEmpty(FormNotCredito.DataSources.DBDataSources.Item("RIN1").GetValue("BaseEntry", i)) Then
                        target = FormNotCredito.DataSources.DBDataSources.Item("RIN1").GetValue("BaseEntry", i)
                    End If

                    If target <> 0 Then
                        If Not ListaTargetEntry.Contains(target) Then
                            ListaTargetEntry.Add(target)
                        End If
                    End If

                Next

                If ListaTargetEntry.Count <> 0 Then
                    DocEntryFacturaEnAsiento = ListaTargetEntry.Item(0)
                Else
                    DocEntryFacturaEnAsiento = 0
                End If

                If DocEntryFacturaEnAsiento <> 0 Then
                    'inicia transaccion
                    SBO_Company.StartTransaction()
                    'crea asiento 
                    strNoAsiento = CrearAsientoReversado(SBO_Company, FormNotCredito, DocEntryFacturaEnAsiento)
                End If

                ListaTargetEntry.Clear()
            Else
                strNoAsiento = String.Empty
            End If


            If Not String.IsNullOrEmpty(strNoAsiento) Then
                'commit en la transaccion 
                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
                strNoAsiento = String.Empty
            Else
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If
            End If


            'Call ListaOrdenesTrabajoEnNotaCredito(FormNotCredito)



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                strNoAsiento = String.Empty
            End If
        Finally
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                strNoAsiento = String.Empty
            End If
        End Try
    End Function

    Public Sub ListaOrdenesTrabajoEnNotaCredito(p_form As SAPbouiCOM.Form)



        ListaNotaCreditoOrdenTrabajo = New List(Of LineasNotaCreditoOT)

        ListaOrdenesTrabajo = New Generic.List(Of String)

        For i As Integer = 0 To p_form.DataSources.DBDataSources.Item("RIN1").Size - 1


            ListaNotaCreditoOrdenTrabajo.Add(New LineasNotaCreditoOT() With {._idRepxOrden = p_form.DataSources.DBDataSources.Item("RIN1").GetValue("U_SCGD_IdRepxOrd", i).Trim, _
                                                                             ._itemCode = p_form.DataSources.DBDataSources.Item("RIN1").GetValue("ItemCode", i).Trim, _
                                                                             ._numeroOT = p_form.DataSources.DBDataSources.Item("RIN1").GetValue("U_SCGD_NoOT", i).Trim, _
                                                                             ._whrCode = p_form.DataSources.DBDataSources.Item("RIN1").GetValue("WhsCode", i).Trim, _
                                                                             ._cantidad = p_form.DataSources.DBDataSources.Item("RIN1").GetValue("Quantity", i).Trim})

            If Not ListaOrdenesTrabajo.Contains(p_form.DataSources.DBDataSources.Item("RIN1").GetValue("U_SCGD_NoOT", i).Trim) Then
                ListaOrdenesTrabajo.Add(p_form.DataSources.DBDataSources.Item("RIN1").GetValue("U_SCGD_NoOT", i).Trim)
            End If

        Next

        'Call BuscarOrdenVentaAsociadas(ListaOrdenesTrabajo, ListaNotaCreditoOrdenTrabajo)


    End Sub

    Public Function BuscarOrdenVentaAsociadas(ByRef p_listaOTs As Generic.List(Of String), p_listaNotaCreditoOT As List(Of LineasNotaCreditoOT))

        Dim oInventario As SAPbobsCOM.Items
        Dim oInventarioLineas As SAPbobsCOM.IItemWarehouseInfo

        Dim UpdateQueryOV As String = "UPDATE [dbo].[ORDR] SET [DocStatus] = 'O'  WHERE [DocEntry] =  '{0}'"
        Dim UpdateQueryLinesOV As String = "UPDATE [dbo].[RDR1] SET [LineStatus] = 'O' ,[InvntSttus] = 'O' WHERE [DocEntry] = {0} and [ItemCode] = '{1}' and [U_SCGD_IdRepxOrd] = '{2}' "

        Try
            For rowL As Integer = 0 To p_listaOTs.Count - 1

                Dim strNumeroOT As String = p_listaOTs.Item(rowL)

                Dim strQuery2 = String.Format("SELECT DocEntry FROM ORDR WHERE U_SCGD_Numero_OT = '{0}'", strNumeroOT)

                m_oOrdenVenta = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)

                Dim DocEntryOrdenVenta As String = Utilitarios.EjecutarConsulta(strQuery2, SBO_Company.CompanyDB, SBO_Company.Server)

                Utilitarios.EjecutarConsulta(String.Format(UpdateQueryOV, DocEntryOrdenVenta), SBO_Company.CompanyDB, SBO_Company.Server)

                'If m_oOrdenVenta.GetByKey(DocEntryOrdenVenta) Then

                'm_oOrdenVentaLines = m_oOrdenVenta.Lines

                'If m_oOrdenVenta.DocumentStatus = BoStatus.bost_Close Then

                'm_oOrdenVenta.Reopen()

                'm_oOrdenVenta.Update()

                ' For j As Integer = 0 To m_oOrdenVentaLines.Count - 1

                'm_oOrdenVentaLines.SetCurrentLine(j)

                'oInventario = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems), SAPbobsCOM.Items)



                For rowNC As Integer = 0 To p_listaNotaCreditoOT.Count - 1

                    'If m_oOrdenVentaLines.ItemCode = p_listaNotaCreditoOT.Item(rowNC).ItemCode And m_oOrdenVentaLines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = p_listaNotaCreditoOT.Item(rowNC).IdRepXOrden Then

                    'm_oOrdenVentaLines.LineStatus = BoStatus.bost_Open
                    Utilitarios.EjecutarConsulta(String.Format(UpdateQueryLinesOV, DocEntryOrdenVenta, p_listaNotaCreditoOT.Item(rowNC).ItemCode, p_listaNotaCreditoOT.Item(rowNC).IdRepXOrden), SBO_Company.CompanyDB, SBO_Company.Server)

                    'If oInventario.GetByKey(p_listaNotaCreditoOT.Item(rowNC).ItemCode) Then

                    '    oInventarioLineas = oInventario.WhsInfo

                    '    For j As Integer = 0 To m_oOrdenVentaLines.Count - 1

                    '        oInventario.WhsInfo.SetCurrentLine(j)

                    '        If oInventarioLineas.WarehouseCode = p_listaNotaCreditoOT.Item(rowNC).WhrCode Then
                    '            oInventarioLineas.Commit()

                    '        End If


                    '    Next





                    'End If
                    'Exit For

                    'End If

                Next
                'Next
                'End If

                'End If

                'm_oOrdenVenta.Update()

            Next

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex

        End Try

    End Function

    <System.CLSCompliant(False)> _
    Public Function CrearAsientoReversado(ByRef ocompany As SAPbobsCOM.Company,
                                                         ByVal oForm As SAPbouiCOM.Form, Optional ByVal p_DocEntry As Integer = 0) As Integer

        Dim oJournalEntryAConsultar As SAPbobsCOM.JournalEntries
        Dim oJournalEntryAConsultar_Lines As SAPbobsCOM.JournalEntries_Lines
        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO

        Dim intError As Integer
        Dim strMensajeError As String = ""

        'monedas
        Dim strMonedaLocal As String = ""
        Dim strMonedaEntrada As String = ""

        'manejo de precios
        Dim strPrecioML As String = ""
        Dim dcPrecioML As Decimal = 0
        Dim strPrecioME As String = ""
        Dim dcPrecioME As Decimal = 0
        Dim dcPrecioAcumuladoML As Decimal = 0
        Dim dcPrecioAcumuladoME As Decimal = 0
        Dim strFechaEntrada As String = ""
        Dim strTipoCambioEntrada As String = ""

        Dim dcValorRetorno As Decimal = 0
        Dim strMemo As String = ""
        Dim strNumeroOT As String = String.Empty
        Dim strBaseRef As String = String.Empty
        Dim strAsientoAReversar As String = String.Empty

        Dim datatable As System.Data.DataTable
        Dim row As System.Data.DataRow

        Dim strUsaDimension As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] ", SBO_Company.CompanyDB, SBO_Company.Server)

        If strUsaDimension = "Y" Then
            blnUsaDimensiones = True
        End If

        datatable = Utilitarios.EjecutarConsultaDataTable(String.Format("SELECT TransId FROM dbo.[OJDT] WHERE U_SCGD_FacC = '{0}'",
                                             p_DocEntry),
                                          SBO_Company.CompanyDB,
                                          SBO_Company.Server)



        'strAsientoAReversar = Utilitarios.EjecutarConsulta(
        '                        String.Format("SELECT TransId FROM dbo.[OJDT] WHERE U_SCGD_FacC = '{0}'",
        '                                     p_DocEntry),
        '                                  SBO_Company.CompanyDB,
        '                                  SBO_Company.Server)

        strNoAsiento = 0

        For Each row In datatable.Rows

            strAsientoAReversar = row.Item("TransId")

            If Not String.IsNullOrEmpty(strAsientoAReversar) Then

                oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

                oJournalEntry.Reference = oForm.DataSources.DBDataSources.Item("ORIN").GetValue("U_SCGD_Numero_OT", 0).Trim()
                strFechaEntrada = oForm.DataSources.DBDataSources.Item("ORIN").GetValue("DocDate", 0).Trim()

                strMemo = oForm.DataSources.DBDataSources.Item("ORIN").GetValue("DocNum", 0).Trim()

                strMemo = My.Resources.Resource.AsientoNotaCreditoClientes +
                oForm.DataSources.DBDataSources.Item("ORIN").GetValue("DocNum", 0).Trim()

                strMonedaEntrada = oForm.DataSources.DBDataSources.Item("ORIN").GetValue("DocCur", 0).Trim()
                strTipoCambioEntrada = oForm.DataSources.DBDataSources.Item("ORIN").GetValue("DocRate", 0).Trim()

                strMonedaLocal = RetornarMonedaLocal()

                oJournalEntry.Memo = strMemo
                Dim Contador As Integer = 0


                oJournalEntryAConsultar = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

                If oJournalEntryAConsultar.GetByKey(strAsientoAReversar) Then

                    oJournalEntryAConsultar_Lines = oJournalEntryAConsultar.Lines

                    For i As Integer = 0 To oJournalEntryAConsultar_Lines.Count - 1

                        oJournalEntryAConsultar_Lines.SetCurrentLine(i)

                        With oJournalEntryAConsultar_Lines

                            Dim strVer As String = .Reference1


                            oJournalEntry.Lines.Reference1 = .Reference1

                            oJournalEntry.Lines.AccountCode = .AccountCode
                            If .Credit = 0 Then
                                oJournalEntry.Lines.Credit = .Debit
                            ElseIf .Debit = 0 Then

                                oJournalEntry.Lines.Debit = .Credit
                            End If

                            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                            If blnUsaDimensiones Then
                                oJournalEntry.Lines.CostingCode = .CostingCode
                                oJournalEntry.Lines.CostingCode2 = .CostingCode2
                                oJournalEntry.Lines.CostingCode3 = .CostingCode3
                                oJournalEntry.Lines.CostingCode4 = .CostingCode4
                                oJournalEntry.Lines.CostingCode5 = .CostingCode5
                            End If


                            oJournalEntry.Lines.Add()

                        End With

                    Next
                End If

                'SERVICIO EXTERNO ************************************************************** 

                'oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaHaber", 0).Trim()
                'dcValorRetorno = 0

                'oJournalEntry.Lines.Debit = Decimal.Parse(dcPrecioAcumuladoML)

                'oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                'oJournalEntry.Lines.Add()

                ''COSTOS ************************************************************************ 
                'oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaDebe", 0).Trim()

                'oJournalEntry.Lines.Credit = Decimal.Parse(dcPrecioAcumuladoML)
                'oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                'oJournalEntry.Lines.Add()
                'COSTOS *************************************************************************


                'GENERA ASIENTOS ****************************************************************
                If oJournalEntry.Add <> 0 Then
                    strNoAsiento = "0"
                    ocompany.GetLastError(intError, strMensajeError)
                    Throw New ExceptionsSBO(intError, strMensajeError)
                Else
                    dcPrecioML = 0
                    dcPrecioAcumuladoML = 0

                    ocompany.GetNewObjectCode(strNoAsiento)
                End If
            End If
        Next



        Return CInt(strNoAsiento)

    End Function


    ''' <summary>
    ''' Retorna moneda local
    ''' </summary>
    ''' <returns>Retorna moneda local</returns>
    ''' <remarks></remarks>
    Public Function RetornarMonedaLocal() As String
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim sToday As String
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strResult As String

        Try

            oSBObob = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
            oRecordset = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            oRecordset = oSBObob.GetLocalCurrency()
            strResult = oRecordset.Fields.Item(0).Value

            Return strResult

        Catch ex As Exception
            Return -1
        End Try

    End Function


    Private oDataTableConfiguracionesSucursal As System.Data.DataTable
    Private oDataRowConfiguracionSucursal As System.Data.DataRow


    ''' <summary>
    ''' Crea una requisición de devolución si se realiza una factura para un documento ligado a una OT
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerarDevoluciones(ByVal p_strDocEntryNotaCredito As String)
        Dim boolUsaTallerInterno = False
        Dim strIDSucursal As String = String.Empty
        Dim strSerieTransferencias As String = String.Empty
        Dim boolUsaRequisiciones As Boolean = False
        Dim strNumeroOT As String = String.Empty

        'Objetos
        Dim oNotaCredito As Documents

        Try
            oNotaCredito = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes), SAPbobsCOM.Documents)
            oNotaCredito.GetByKey(p_strDocEntryNotaCredito)

            strNumeroOT = oNotaCredito.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim()
            strIDSucursal = oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()
            'strNumeroOT = FormNotCredito.DataSources.DBDataSources.Item("ORIN").GetValue("U_SCGD_Numero_OT", 0).Trim()
            'strIDSucursal = FormNotCredito.DataSources.DBDataSources.Item("ORIN").GetValue("U_SCGD_idSucursal", 0).Trim()

            'Verifica que la nota de crédito este ligada a una orden de trabajo, de lo contrario no es necesario realizar devoluciones
            If Not String.IsNullOrEmpty(strNumeroOT) Then

                boolUsaTallerInterno = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
                oDataTableConfiguracionesSucursal = Utilitarios.ObtenerConsultaConfiguracionPorSucursal(strIDSucursal, SBO_Company)
                If oDataTableConfiguracionesSucursal.Rows.Count <> 0 Then
                    oDataRowConfiguracionSucursal = oDataTableConfiguracionesSucursal.Rows(0)
                Else
                    oDataRowConfiguracionSucursal = Nothing
                End If

                If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                    'Obtiene la serie de numeración para transferencias
                    If Not String.IsNullOrEmpty(oDataRowConfiguracionSucursal.Item("U_SerInv")) Then
                        strSerieTransferencias = oDataRowConfiguracionSucursal.Item("U_SerInv")
                    End If

                End If

                If Not IsDBNull(oDataRowConfiguracionSucursal.Item("U_Requis")) Then
                    If oDataRowConfiguracionSucursal.Item("U_Requis").ToString.ToUpper() = "Y" Then
                        boolUsaRequisiciones = True
                    End If
                End If

                If boolUsaTallerInterno Then

                    If boolUsaRequisiciones Then
                        If oNotaCredito.DocType = BoDocumentTypes.dDocument_Items Then
                            'Generamos requisiciones por devolucion
                            GenerarRequisicionDevolucion(strNumeroOT, strSerieTransferencias, p_strDocEntryNotaCredito)
                        End If
                    End If

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Enum enumTipoArticulo
        Repuesto = 1
        Suministro = 3
    End Enum

    Private Enum enumTipoRequisicion
        Transferencia = 1
        Devolucion = 2
        Reserva = 3
    End Enum

    ''' <summary>
    ''' Genera requisiciones de devolución
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GenerarRequisicionDevolucion(ByVal p_strNumeroOT As String, ByVal p_strSerieTransferencias As String, ByVal p_strDocEntryNotaCredito As String)

        Dim m_strSerie As String = String.Empty
        Dim m_boolGenerarRollback As Boolean = False
        Dim m_boolProcesarLinea As Boolean = False
        Dim m_boolCrearDocumento As Boolean = False
        Dim strCentroCosto As String = String.Empty
        Dim m_strNoOrden As String = String.Empty
        Dim m_intDocEntry As Integer = -1
        Dim m_intDocEntryRequisicion As Integer = -1
        Dim strCodTipoArticulo As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim intError As Integer = 0
        Dim strErrorMsj As String = String.Empty
        Dim strIDSucursal As String = String.Empty
        Dim strComentarios As String = String.Empty
        Dim strNombreAsesor As String = String.Empty

        'Bodegas
        Dim strBodegaStock As String = String.Empty
        Dim strTipoBodega As String = String.Empty

        'Objetos
        Dim oItem As SAPbobsCOM.IItems
        Dim oNotaCredito As Documents

        'Enumeraciones
        Dim TipoArticulo As enumTipoArticulo

        Try

            oNotaCredito = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes), SAPbobsCOM.Documents)
            m_intDocEntry = CInt(p_strDocEntryNotaCredito)

            'Procesa el documento
            If oNotaCredito.GetByKey(m_intDocEntry) And m_intDocEntry > 0 Then

                'Para cada tipo de artículo se debe generar una requisición de devolución distinta
                'Por ejemplo: Repuestos, debe llevar su propia requisición y en otra separada los suministros
                For Each eTipoArticulo As enumTipoArticulo In [Enum].GetValues(GetType(enumTipoArticulo))

                    'Objeto requisición
                    Dim oRequisicion As SAPbobsCOM.GeneralData
                    Dim oChildrenLineasReq As SAPbobsCOM.GeneralDataCollection
                    Dim oReqLinea As SAPbobsCOM.GeneralData
                    Dim oCompanyService As SAPbobsCOM.CompanyService
                    Dim oGeneralService As SAPbobsCOM.GeneralService
                    Dim oEmployeesInfo As SAPbobsCOM.EmployeesInfo

                    m_boolCrearDocumento = False

                    oCompanyService = SBO_Company.GetCompanyService()
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ")
                    oRequisicion = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData)
                    oChildrenLineasReq = oRequisicion.Child("SCGD_LINEAS_REQ")

                    strIDSucursal = oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()
                    'Consulta las bodegas a utilizar para la devolución
                    If Not String.IsNullOrEmpty(strIDSucursal) Then

                        oItem = SBO_Company.GetBusinessObject(BoObjectTypes.oItems)

                        'Obtiene la información del asesor desde el maestro de empleados
                        oEmployeesInfo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oEmployeesInfo)

                        If Not oEmployeesInfo Is Nothing Then
                            oEmployeesInfo.GetByKey(oNotaCredito.DocumentsOwner)
                            strNombreAsesor = String.Format("{0} {1}", oEmployeesInfo.FirstName, oEmployeesInfo.LastName)
                        End If

                        strComentarios = String.Format("{0} {1} {2} {3}", My.Resources.Resource.OT_Referencia, p_strNumeroOT, My.Resources.Resource.Asesor, strNombreAsesor)

                        'Encabezado de la requisición
                        oRequisicion.SetProperty("U_SCGD_NoOrden", p_strNumeroOT)
                        oRequisicion.SetProperty("U_SCGD_CodCliente", oNotaCredito.CardCode)
                        oRequisicion.SetProperty("U_SCGD_NombCliente", oNotaCredito.CardName)
                        oRequisicion.SetProperty("U_SCGD_TipoReq", My.Resources.Resource.Devolucion)
                        oRequisicion.SetProperty("U_SCGD_CodTipoReq", CInt(enumTipoRequisicion.Devolucion))
                        oRequisicion.SetProperty("U_SCGD_TipoDoc", "Transf. Inv")
                        oRequisicion.SetProperty("U_SCGD_Usuario", SBO_Company.UserName)
                        oRequisicion.SetProperty("U_SCGD_Comm", strComentarios)
                        oRequisicion.SetProperty("U_SCGD_TipArt", CInt(eTipoArticulo).ToString())
                        oRequisicion.SetProperty("U_SCGD_CodEst", EstadosLineas.Pendiente)
                        oRequisicion.SetProperty("U_SCGD_Est", My.Resources.Resource.Pendiente)
                        oRequisicion.SetProperty("U_ActualizaDoc", "N")

                        'Metadata del encabezado
                        Dim m_objData As EncabezadoTrasladoDMSData = New EncabezadoTrasladoDMSData()
                        m_objData.TipoTransferencia = enumTipoRequisicion.Devolucion
                        m_objData.Serie = p_strSerieTransferencias
                        m_objData.NumCotizacionOrigen = oNotaCredito.DocEntry

                        oRequisicion.SetProperty("U_SCGD_Data", m_objData.Serialize())
                        oRequisicion.SetProperty("U_SCGD_IDSuc", oNotaCredito.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim())

                        'Información del vehículo
                        oRequisicion.SetProperty("U_SCGD_Placa", oNotaCredito.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim())
                        oRequisicion.SetProperty("U_SCGD_Marca", oNotaCredito.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString().Trim())
                        oRequisicion.SetProperty("U_SCGD_Estilo", oNotaCredito.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString().Trim())
                        oRequisicion.SetProperty("U_SCGD_VIN", oNotaCredito.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString().Trim())

                        'Recorre las líneas de la nota de crédito y genera las devoluciones para los artículos inventariables
                        'como repuestos o suministros
                        For i As Integer = 0 To oNotaCredito.Lines.Count - 1
                            oNotaCredito.Lines.SetCurrentLine(i)
                            oItem.GetByKey(oNotaCredito.Lines.ItemCode)

                            'Verifica si la nota de crédito produce movimiento de inventario
                            'en caso de no producir no se deben generar devoluciones para esta línea.
                            Dim boolSinContabilizacionStock = False

                            If oNotaCredito.Lines.WithoutInventoryMovement = BoYesNoEnum.tYES Then
                                boolSinContabilizacionStock = True
                            End If

                            m_boolProcesarLinea = False

                            strCentroCosto = oItem.UserFields.Fields.Item("U_SCGD_CodCtroCosto").Value.ToString().Trim()
                            strCodTipoArticulo = oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value.ToString().Trim()

                            'Si el tipo de artículo de la linea es del mismo tipo que se esta generando la requisición se agrega la línea
                            'de lo contrario se omite la línea y se procesa en la requisición que le corresponde ya sea suministros o repuestos
                            If strCodTipoArticulo = CInt(eTipoArticulo) Then
                                'Tipo de artículo
                                If strCodTipoArticulo = CInt(enumTipoArticulo.Repuesto).ToString() And boolSinContabilizacionStock = False Then
                                    strTipoBodega = TransferenciaItems.mc_strBodegaRepuestos
                                    strTipoArticulo = My.Resources.Resource.Repuesto
                                    m_boolProcesarLinea = True
                                ElseIf strCodTipoArticulo = CInt(enumTipoArticulo.Suministro).ToString() And boolSinContabilizacionStock = False Then
                                    strTipoBodega = TransferenciaItems.mc_strBodegaSuministros
                                    strTipoArticulo = My.Resources.Resource.Suministro
                                    m_boolProcesarLinea = True
                                End If
                            Else
                                m_boolProcesarLinea = False
                            End If

                            strBodegaStock = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, strTipoBodega, strIDSucursal, SBO_Application)

                            If strBodegaStock = oNotaCredito.Lines.WarehouseCode Then
                                'Si la bodega destino es la misma que la bodega origen se omite la línea de la requisición
                                m_boolProcesarLinea = False
                            End If

                            'Agrega la linea a la requisicion
                            If m_boolProcesarLinea = True Then

                                oReqLinea = oChildrenLineasReq.Add()

                                'Completa la información de las columnas de la tabla hija "@SCGD_LINEAS_REQ" con los datos de la requisición
                                oReqLinea.SetProperty("U_SCGD_CodArticulo", oNotaCredito.Lines.ItemCode)
                                oReqLinea.SetProperty("U_SCGD_DescArticulo", oItem.ItemName)
                                oReqLinea.SetProperty("U_SCGD_ID", oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_ID").Value)
                                oReqLinea.SetProperty("U_SCGD_CodBodOrigen", oNotaCredito.Lines.WarehouseCode)
                                oReqLinea.SetProperty("U_SCGD_CodBodDest", strBodegaStock)
                                oReqLinea.SetProperty("U_SCGD_CantRec", 0)
                                oReqLinea.SetProperty("U_SCGD_CodEst", EstadosLineas.Pendiente)
                                oReqLinea.SetProperty("U_SCGD_CCosto", strCentroCosto)
                                oReqLinea.SetProperty("U_SCGD_LNumOr", oNotaCredito.Lines.LineNum)
                                oReqLinea.SetProperty("U_SCGD_COrig", oNotaCredito.Lines.Quantity)
                                oReqLinea.SetProperty("U_SCGD_CantSol", oNotaCredito.Lines.Quantity)
                                oReqLinea.SetProperty("U_SCGD_Lidsuc", oNotaCredito.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value)
                                oReqLinea.SetProperty("U_SCGD_CodTipoArt", strCodTipoArticulo)
                                oReqLinea.SetProperty("U_SCGD_TipoArticulo", strTipoArticulo)
                                oReqLinea.SetProperty("U_SCGD_Estado", My.Resources.Resource.Pendiente)
                                oReqLinea.SetProperty("U_SCGD_DocOr", oNotaCredito.DocEntry)

                                If (SBO_Company.Version >= 900000) Then
                                    Dim strUbicacion As String = String.Empty
                                    strUbicacion = CargaUbicacion(oItem.ItemCode, strBodegaStock, oItem.ItemsGroupCode)
                                    oReqLinea.SetProperty("U_DeUbic", String.Empty)
                                    oReqLinea.SetProperty("U_AUbic", strUbicacion)
                                End If

                                Dim cantidadDisponible As Double = 0
                                cantidadDisponible = ObtenerCantidadDisponible(oItem.ItemCode, oNotaCredito.Lines.WarehouseCode)
                                oReqLinea.SetProperty("U_SCGD_CantDispo", cantidadDisponible)

                                'Si se agrega al menos 1 línea, se puede crear el documento
                                m_boolCrearDocumento = True
                            End If

                        Next

                    End If

                    'Crea el documento
                    If m_boolCrearDocumento Then
                        'Inicia la transacción
                        If Not SBO_Company.InTransaction Then
                            SBO_Company.StartTransaction()
                        End If

                        'Genera la requisición de devolución
                        oGeneralService.Add(oRequisicion)

                        SBO_Company.GetLastError(intError, strErrorMsj)

                        If intError <> 0 Then
                            m_boolGenerarRollback = True
                        End If

                        'Finaliza la transacción
                        If SBO_Company.InTransaction Then
                            If m_boolGenerarRollback Then
                                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                            Else
                                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
                            End If
                        End If
                    End If

                Next

            Else
                'Mensaje de error número de documento inválido o incorrecto
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorNumeroDocumento, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            'En caso de errores finaliza la transacción y genera un rollback.
            If SBO_Company.InTransaction Then
                If m_boolGenerarRollback Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                End If
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la cantidad disponible en la bodega
    ''' </summary>
    ''' <param name="p_strItemCode">Código del item</param>
    ''' <param name="p_strBodegaUbicacion">Código de la bodega</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerCantidadDisponible(ByVal p_strItemCode As String, ByVal p_strBodega As String) As Double
        Dim oArticulo As SAPbobsCOM.IItems
        Dim disponibleAlmacen As Double = 0

        Try
            oArticulo = SBO_Company.GetBusinessObject(BoObjectTypes.oItems)
            oArticulo.GetByKey(p_strItemCode)

            For i As Integer = 0 To oArticulo.WhsInfo.Count - 1
                oArticulo.WhsInfo.SetCurrentLine(i)
                If oArticulo.WhsInfo.WarehouseCode = p_strBodega Then
                    disponibleAlmacen = oArticulo.WhsInfo.InStock + oArticulo.WhsInfo.Ordered - oArticulo.WhsInfo.Committed
                    Exit For
                End If
            Next

            Return disponibleAlmacen

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    'public float CantidadDisponible()
    '  {
    '      Items items = (Items) CompanySBO.GetBusinessObject(BoObjectTypes.oItems);
    '      if (items.GetByKey(ItemCode))
    '      {
    '          for (int i = 0; i < items.WhsInfo.Count && items.WhsInfo.WarehouseCode != WhsCode; i++)
    '              items.WhsInfo.SetCurrentLine(i);
    '          return (float) (items.WhsInfo.InStock + items.WhsInfo.Ordered - items.WhsInfo.Committed);
    '      }
    '      throw new InvalidOperationException(string.Format("Item {0} does not exist",ItemCode));
    '  }

    ''' <summary>
    ''' Carga la ubicación por defecto para el artículo y bodega indicados de acuerdo al orden de prioridad
    ''' </summary>
    ''' <param name="p_strItemCode">Código de artículo</param>
    ''' <param name="p_strBodegaUbicacion">Bodega desde la cual se va a mover el artículo</param>
    ''' <param name="p_intItemGroupCode">Código del Grupo de artículos</param>
    ''' <returns>Código de la ubicación en formato texto</returns>
    ''' <remarks></remarks>
    Private Function CargaUbicacion(ByVal p_strItemCode As String, ByVal p_strBodegaUbicacion As String, ByVal p_intItemGroupCode As Integer) As String
        Dim strUbicacion As String = String.Empty

        Try
            '************Explicacion **************
            ' La jerarquia en SAP para ubicaciones es la siguiente 
            'Default Bin Location of Item > Default Bin Location of Item Group > Default Bin Location of Warehouse
            '***** Objetos SAP *****

            'Primer nivel ubicación por artículo
            strUbicacion = CargaUbicacionDefectoArticulo(p_strItemCode, p_strBodegaUbicacion)
            If String.IsNullOrEmpty(strUbicacion) Then

                'Segundo nivel ubicación por grupo de artículos
                strUbicacion = CargaUbicacionDefectoGrupoArticulo(p_intItemGroupCode, p_strBodegaUbicacion)
                If String.IsNullOrEmpty(strUbicacion) Then

                    'TercerNivel ubicación predeterminada del almacén
                    strUbicacion = CargaUbicacionDefectoAlmacen(p_strBodegaUbicacion)
                End If

            End If

            Return strUbicacion

        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Function


    ''' <summary>
    ''' Consulta la descripción de la ubicación
    ''' </summary>
    ''' <param name="p_intBinCode">Código de la ubicación</param>
    ''' <returns>Descripción de la ubicación en formato texto</returns>
    ''' <remarks></remarks>
    Private Function CargaDescripcionUbicacion(ByVal p_intBinCode As Integer) As String
        Dim strBinCode As String = String.Empty
        Dim oBinLocation As SAPbobsCOM.BinLocation

        Try
            If p_intBinCode > 0 Then
                strBinCode = Utilitarios.EjecutarConsulta(String.Format("SELECT ""BinCode"" FROM ""OBIN"" WHERE ""AbsEntry"" = {0}", p_intBinCode))
            End If

            Return strBinCode

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return String.Empty
        End Try

    End Function

    ''' <summary>
    ''' Consulta la ubicación por defecto para el artículo
    ''' </summary>
    ''' <param name="p_strItemCode">Código del artículo</param>
    ''' <param name="p_strBodegaUbicacion">Bodega desde la cual se va a mover el artículo</param>
    ''' <returns>Código de la ubicación en formato texto</returns>
    ''' <remarks></remarks>
    Private Function CargaUbicacionDefectoArticulo(ByVal p_strItemCode As String, ByVal p_strBodegaUbicacion As String) As String
        Dim oArticulo As SAPbobsCOM.IItems
        Dim strUbicacion As String = String.Empty

        Try
            oArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            If oArticulo.GetByKey(p_strItemCode) Then

                For i As Integer = 0 To oArticulo.WhsInfo.Count - 1
                    oArticulo.WhsInfo.SetCurrentLine(i)

                    If oArticulo.WhsInfo.WarehouseCode = p_strBodegaUbicacion Then

                        If oArticulo.WhsInfo.DefaultBin > 0 Then
                            strUbicacion = oArticulo.WhsInfo.DefaultBin.ToString().Trim()
                        End If

                    End If
                Next

            End If

            Return strUbicacion

        Catch ex As Exception
            Utilitarios.DestruirObjeto(oArticulo)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oArticulo)
        End Try

    End Function

    ''' <summary>
    ''' Carga la ubicación por defecto de acuerdo al grupo de artículos
    ''' </summary>
    ''' <param name="p_intItemGroupCode">Código del grupo de artículos en formato entero</param>
    ''' <param name="p_strBodegaUbicacion">Código de la bodega desde la cual se realiza el movimiento en formato texto</param>
    ''' <returns>Código de la ubicación en formato texto</returns>
    ''' <remarks></remarks>
    Private Function CargaUbicacionDefectoGrupoArticulo(ByVal p_intItemGroupCode As Integer, ByVal p_strBodegaUbicacion As String) As String
        Dim oIItemGroups As IItemGroups
        Dim strUbicacion As String = String.Empty

        Try

            oIItemGroups = SBO_Company.GetBusinessObject(BoObjectTypes.oItemGroups)

            If oIItemGroups.GetByKey(p_intItemGroupCode) Then

                For i As Integer = 0 To oIItemGroups.WarehouseInfo.Count - 1
                    oIItemGroups.WarehouseInfo.SetCurrentLine(i)

                    If oIItemGroups.WarehouseInfo.WarehouseCode = p_strBodegaUbicacion Then
                        If oIItemGroups.WarehouseInfo.DefaultBin > 0 Then
                            strUbicacion = oIItemGroups.WarehouseInfo.DefaultBin.ToString().Trim()
                        End If
                    End If
                Next

            End If

            Return strUbicacion

        Catch ex As Exception
            Utilitarios.DestruirObjeto(oIItemGroups)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oIItemGroups)
        End Try

    End Function


    ''' <summary>
    ''' Carga la ubicación por defecto del almacén
    ''' </summary>
    ''' <param name="p_strBodegaUbicacion">Código del almacén desde el cual se realiza el movimiento en formato texto</param>
    ''' <returns>Código de la ubicación en formato texto</returns>
    ''' <remarks></remarks>
    Private Function CargaUbicacionDefectoAlmacen(ByVal p_strBodegaUbicacion As String) As String
        Dim oIWarehouses As IWarehouses
        Dim strUbicacion As String = String.Empty

        Try

            oIWarehouses = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses)

            If oIWarehouses.GetByKey(p_strBodegaUbicacion) Then

                If oIWarehouses.EnableBinLocations = SAPbobsCOM.BoYesNoEnum.tYES Then
                    If oIWarehouses.DefaultBin > 0 Then
                        strUbicacion = oIWarehouses.DefaultBin.ToString().Trim()
                    End If
                End If

            End If

            Return strUbicacion

        Catch ex As Exception
            Utilitarios.DestruirObjeto(oIWarehouses)
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oIWarehouses)
        End Try

    End Function




#End Region

    Public Class LineasNotaCreditoOT

        Public _itemCode As String
        Public Property ItemCode As String
            Get
                Return _itemCode

            End Get
            Set(value As String)
                value = _itemCode
            End Set
        End Property

        Public _idRepxOrden As Integer
        Public Property IdRepXOrden As Integer
            Get
                Return _idRepxOrden
            End Get
            Set(value As Integer)
                value = _idRepxOrden
            End Set
        End Property

        Public _numeroOT As String
        Public Property NumeroOT As String
            Get
                Return _numeroOT
            End Get
            Set(value As String)

                value = _numeroOT
            End Set
        End Property

        Public _whrCode As String
        Public Property WhrCode As String
            Get
                Return _whrCode
            End Get
            Set(value As String)

                value = _whrCode
            End Set
        End Property

        Public _cantidad As Decimal
        Public Property Cantidad As Decimal
            Get
                Return _cantidad
            End Get
            Set(value As Decimal)

                value = _cantidad
            End Set
        End Property

    End Class

End Class
