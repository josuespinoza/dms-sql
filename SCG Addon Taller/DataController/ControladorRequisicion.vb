Imports SAPbouiCOM
Imports SCG.Requisiciones.UI
Imports System.Collections.Generic
Imports System.Linq
Imports SAPbobsCOM

Public Class ControladorRequisicion

#Region "Variables Globales"
    Private _company As SAPbobsCOM.Company
    Private _application As Application
#End Region

#Region "Constantes"
    Public Const TablaEncabezado As String = "SCGD_REQUISICIONES"
    Public Const TablaLineas As String = "SCGD_LINEAS_REQ"
    Public Const TablaMovimientos As String = "SCGD_MOVS_REQ"
    Public Const UserDataTable As String = "SCGD_USER"
    Public Const UDORequisiciones As String = "SCGD_REQ"
#End Region

#Region "Propiedades"
    Public ReadOnly Property oCompany() As SAPbobsCOM.Company
        Get
            Return _company
        End Get
    End Property

    Public ReadOnly Property oApplication() As Application
        Get
            Return _application
        End Get
    End Property
#End Region
#Region "Variables"

#End Region

#Region "Enumeradores"
    Private Enum TipoTransferencia
        Ninguno = 0
        Transferencia = 1
        TransferenciaDevolusion = 2
    End Enum
#End Region

#Region "Constructor"
    Public Sub New(ByVal company As SAPbobsCOM.Company, ByVal application As Application)
        _company = company
        _application = application
    End Sub
#End Region

#Region "Metodos"
    Public Sub CrearRequisicion(ByRef p_oRequisicionData As RequisicionData_List, _
                                ByRef p_oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData), ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String)
        Try
            '************Objetos SAP **********
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oLineaRequisicion As SAPbobsCOM.GeneralData
            Dim oLineasRequisicion As SAPbobsCOM.GeneralDataCollection
            Dim oLineaMovimiento As SAPbobsCOM.GeneralData
            Dim oLineasMovimiento As SAPbobsCOM.GeneralDataCollection

            oCompanyService = oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService(UDORequisiciones)
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
            With p_oRequisicionData.Item(0)
                If Not String.IsNullOrEmpty(.NoOrden) Then oGeneralData.SetProperty("U_SCGD_NoOrden", .NoOrden)
                If Not String.IsNullOrEmpty(.CodigoCliente) Then oGeneralData.SetProperty("U_SCGD_CodCliente", .CodigoCliente)
                If Not String.IsNullOrEmpty(.NombreCliente) Then oGeneralData.SetProperty("U_SCGD_NombCliente", .NombreCliente)
                If Not String.IsNullOrEmpty(.CodigoTipoRequisicion.ToString()) Then oGeneralData.SetProperty("U_SCGD_CodTipoReq", .CodigoTipoRequisicion)
                If Not String.IsNullOrEmpty(.TipoRequisicion) Then oGeneralData.SetProperty("U_SCGD_TipoReq", .TipoRequisicion)
                If Not String.IsNullOrEmpty(.TipoDocumento) Then oGeneralData.SetProperty("U_SCGD_TipoDoc", .TipoDocumento)
                If Not String.IsNullOrEmpty(.Usuario) Then oGeneralData.SetProperty("U_SCGD_Usuario", .Usuario)
                If Not String.IsNullOrEmpty(.Comentario) Then oGeneralData.SetProperty("U_SCGD_Comm", .Comentario)
                If Not String.IsNullOrEmpty(.Data) Then oGeneralData.SetProperty("U_SCGD_Data", .Data)
                If Not String.IsNullOrEmpty(.SucursalID) Then oGeneralData.SetProperty("U_SCGD_IDSuc", .SucursalID)
                If Not String.IsNullOrEmpty(.CodigoEstadoRequisicion.ToString()) Then oGeneralData.SetProperty("U_SCGD_CodEst", .CodigoEstadoRequisicion)
                If Not String.IsNullOrEmpty(.EstadoRequisicion) Then oGeneralData.SetProperty("U_SCGD_Est", .EstadoRequisicion)
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSuc) confSuc.U_Sucurs.Trim = .SucursalID) Then
                    With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(confSuc) confSuc.U_Sucurs.Trim = .SucursalID)
                        If Not String.IsNullOrEmpty(.U_SerInv) Then oGeneralData.SetProperty("U_Serie", Convert.ToInt32(.U_SerInv))
                    End With
                End If
                If Not String.IsNullOrEmpty(NumeroSerieCita) Then oGeneralData.SetProperty("U_SerieCita", NumeroSerieCita)
                If Not String.IsNullOrEmpty(ConsecutivoCita) Then oGeneralData.SetProperty("U_NumeroCita", ConsecutivoCita)
            End With
            oLineasRequisicion = oGeneralData.Child(TablaLineas)
            For Each row As RequisicionData In p_oRequisicionData
                oLineaRequisicion = oLineasRequisicion.Add()
                If Not String.IsNullOrEmpty(row.ItemCode) Then oLineaRequisicion.SetProperty("U_SCGD_CodArticulo", row.ItemCode)
                If Not String.IsNullOrEmpty(row.Description) Then oLineaRequisicion.SetProperty("U_SCGD_DescArticulo", row.Description)
                If Not String.IsNullOrEmpty(row.BodegaOrigen) Then oLineaRequisicion.SetProperty("U_SCGD_CodBodOrigen", row.BodegaOrigen)
                If Not String.IsNullOrEmpty(row.BodegaDestino) Then oLineaRequisicion.SetProperty("U_SCGD_CodBodDest", row.BodegaDestino)
                If Not String.IsNullOrEmpty(row.TipoArticulo.ToString()) Then oLineaRequisicion.SetProperty("U_SCGD_CodTipoArt", row.TipoArticulo)
                If Not String.IsNullOrEmpty(row.DescripcionTipoArticulo) Then oLineaRequisicion.SetProperty("U_SCGD_TipoArticulo", row.DescripcionTipoArticulo)
                If IsNumeric(row.CantidadSolicitada) Then oLineaRequisicion.SetProperty("U_SCGD_CantSol", row.CantidadSolicitada)
                If IsNumeric(row.CantidadRecibida) Then oLineaRequisicion.SetProperty("U_SCGD_CantRec", row.CantidadRecibida)
                If Not String.IsNullOrEmpty(row.CentroCosto) Then oLineaRequisicion.SetProperty("U_SCGD_CCosto", row.CentroCosto)
                If Not String.IsNullOrEmpty(row.CodigoEstadoLinea.ToString()) Then oLineaRequisicion.SetProperty("U_SCGD_CodEst", row.CodigoEstadoLinea)
                If Not String.IsNullOrEmpty(row.EstadoLinea) Then oLineaRequisicion.SetProperty("U_SCGD_Estado", row.EstadoLinea)
                If Not String.IsNullOrEmpty(row.LineNumOrigen.ToString()) Then oLineaRequisicion.SetProperty("U_SCGD_LNumOr", row.LineNumOrigen)
                If Not String.IsNullOrEmpty(row.DocumentoOrigen.ToString()) Then oLineaRequisicion.SetProperty("U_SCGD_DocOr", row.DocumentoOrigen)
                If IsNumeric(row.CantidadOriginal) Then oLineaRequisicion.SetProperty("U_SCGD_COrig", row.CantidadOriginal)
                If Not String.IsNullOrEmpty(row.LineaSucursalID) Then oLineaRequisicion.SetProperty("U_SCGD_Lidsuc", row.LineaSucursalID)
                If Not String.IsNullOrEmpty(row.ID) Then oLineaRequisicion.SetProperty("U_SCGD_ID", row.ID)
                If oCompany.Version >= 900000 Then
                    If Not String.IsNullOrEmpty(row.UbicacionDestino) Then oLineaRequisicion.SetProperty("U_AUbic", row.UbicacionDestino)
                    If Not String.IsNullOrEmpty(row.UbicacionOrigen) Then oLineaRequisicion.SetProperty("U_DeUbic", row.UbicacionOrigen)
                    If Not String.IsNullOrEmpty(row.DescripcionUbicacionDestino) Then oLineaRequisicion.SetProperty("U_DesAUbic", row.DescripcionUbicacionDestino)
                    If Not String.IsNullOrEmpty(row.DescripcionUbicacionOrigen) Then oLineaRequisicion.SetProperty("U_DesDeUbic", row.DescripcionUbicacionOrigen)
                End If
            Next
            oLineasMovimiento = oGeneralData.Child(TablaMovimientos)
            oLineaMovimiento = oLineasMovimiento.Add()
            oLineaMovimiento.SetProperty("U_SCGD_CodArticulo", "-1")

            'oGeneralService.Add(oGeneralData)
            p_oListaRequisicionGeneralData.Add(oGeneralData)
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function CrearRequisicionGeneralData(ByRef p_oListaRequisicionGeneralData As List(Of SAPbobsCOM.GeneralData), ByVal EsTransferenciaAutomatica As Boolean) As Boolean
        '************Objetos SAP **********
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim generalDataParams As SAPbobsCOM.GeneralDataParams
        Dim lstTempGeneralData As List(Of SAPbobsCOM.GeneralData)
        Dim ErrorCode As Integer = 0
        Dim ErrorMessage As String = String.Empty
        Try
            oCompanyService = oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService(UDORequisiciones)
            lstTempGeneralData = New List(Of GeneralData)()
            For Each oGeneralData As SAPbobsCOM.GeneralData In p_oListaRequisicionGeneralData
                generalDataParams = oGeneralService.Add(oGeneralData)
                If Not IsNothing(generalDataParams) Then
                    If EsTransferenciaAutomatica Then
                        SCG.Requisiciones.TransferenciasDirectas.CrearTransferencia(oGeneralService.GetByParams(generalDataParams), ErrorCode, ErrorMessage)
                        If ErrorCode <> 0 Then
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(ErrorMessage, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            Return False
                        End If
                    End If
                    lstTempGeneralData.Add(oGeneralService.GetByParams(generalDataParams))
                End If
            Next

            p_oListaRequisicionGeneralData = lstTempGeneralData

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function


    Public Function CrearTransferenciaStock(ByRef p_oMovimientoTransferenciaList As MovimientoTransferencia_List) As Integer
        Dim oDocumentoStock As SAPbobsCOM.StockTransfer
        Try
            Dim intResultadoAdd As Integer = 0
            Dim strError As String = String.Empty
            Dim intDocEntry As Integer = 0
            If p_oMovimientoTransferenciaList.Count > 0 Then
                oDocumentoStock = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
                '**************************************
                'Carga datos encabezado documento
                '**************************************
                With p_oMovimientoTransferenciaList.Item(0)
                    oDocumentoStock.CardCode = .CardCode
                    oDocumentoStock.FromWarehouse = .BodegaOrigen
                    If .Series > 0 Then
                        oDocumentoStock.Series = .Series
                    End If
                    If Not String.IsNullOrEmpty(.NoOrden) Then
                        oDocumentoStock.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .NoOrden
                    End If
                    If .TipoTransferencia > 0 Then
                        oDocumentoStock.UserFields.Fields.Item("U_SCGD_TipoTransf").Value = .TipoTransferencia
                    Else
                        oDocumentoStock.UserFields.Fields.Item("U_SCGD_TipoTransf").Value = TipoTransferencia.Ninguno
                    End If
                    If .TipoTransferencia = TipoTransferencia.Transferencia Then
                        oDocumentoStock.Comments &= My.Resources.Resource.OT_Referencia & .NoOrden
                    ElseIf .TipoTransferencia = TipoTransferencia.TransferenciaDevolusion Then
                        oDocumentoStock.Comments &= " * * " & My.Resources.Resource.Devolucion & " * * "
                    End If
                End With
                '**************************************
                'Carga datos lineas documento
                '**************************************
                For Each row As MovimientoTransferencia In p_oMovimientoTransferenciaList
                    With oDocumentoStock
                        .Lines.Add()
                        .Lines.ItemCode = row.ItemCode
                        .Lines.Quantity = row.CantidadTransferir
                        .Lines.WarehouseCode = row.BodegaDestino
                        If oCompany.Version > 900000 Then
                            If row.UsaUbicaciones Then
                                If Not String.IsNullOrEmpty(row.UbicacionOrigen) Then
                                    If row.TipoTransferencia = TipoTransferencia.Transferencia Then
                                        oDocumentoStock.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse
                                        oDocumentoStock.Lines.BinAllocations.BinAbsEntry = CInt(row.UbicacionOrigen)
                                        oDocumentoStock.Lines.BinAllocations.Quantity = row.CantidadTransferir
                                        oDocumentoStock.Lines.BinAllocations.Add()
                                    ElseIf row.TipoTransferencia = TipoTransferencia.TransferenciaDevolusion Then
                                        oDocumentoStock.Lines.BinAllocations.BinActionType = SAPbobsCOM.BinActionTypeEnum.batFromWarehouse
                                        oDocumentoStock.Lines.BinAllocations.BinAbsEntry = CInt(row.UbicacionDestino)
                                        oDocumentoStock.Lines.BinAllocations.Quantity = row.CantidadTransferir
                                        oDocumentoStock.Lines.BinAllocations.Add()
                                    End If
                                End If
                            End If
                        End If
                    End With
                Next
                intResultadoAdd = oDocumentoStock.Add()
                If intResultadoAdd <> 0 Then
                    strError = oCompany.GetLastErrorDescription()
                    oApplication.StatusBar.SetText(strError, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return 0
                Else
                    intDocEntry = CInt(oCompany.GetNewObjectKey)
                    Return intDocEntry
                End If
            End If
            Return 0
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return 0
        Finally
            If Not oDocumentoStock Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oDocumentoStock)
                oDocumentoStock = Nothing
            End If
        End Try
    End Function

#End Region
End Class