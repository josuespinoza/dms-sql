Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI.Extensions
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany
Imports DMS_Addon.ControlesSBO

Imports SCG.DMSOne.Framework
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Collections.Generic


Partial Public Class KardexInventarioVehiculo


#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company
    Private dtDisponibilidad As SAPbouiCOM.DataTable
    Private m_oJournalEntries As SAPbobsCOM.JournalEntries
    Private m_oJournalEntriesLines As SAPbobsCOM.JournalEntries_Lines

    Public n As New Globalization.NumberFormatInfo

    Private Debito As Double
    Private Credito As Double
    Private SysDebito As Double
    Private SysCredito As Double

    Private strCodigoUnidad As String = String.Empty

#End Region

#Region "Metodos"


    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                  ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                  ByRef BubbleEvent As Boolean)

        Dim oMatrix As SAPbouiCOM.Matrix = Nothing
        Dim oForm As SAPbouiCOM.Form
        oForm = _applicationSbo.Forms.Item(FormUID)

        Dim strSeleccionTodas As String = ""
        Dim strUnidad As String = ""

        If Not oForm Is Nothing Then
            If pVal.BeforeAction Then

                Select Case pVal.ItemUID
                    Case "btnCons"

                        CargarDatosVehiculos()

                    Case "mtxVehi2"

                        Dim intNumeroLinea As Integer
                        Dim tipo As String
                        'requiero el numero de linea para hacer el cambio en caso de que cambie el vehiculo

                        If pVal.ColUID = "col_DocEn" Then
                            intNumeroLinea = pVal.Row
                            tipo = dataTableVehiculo.GetValue("TipoDocumento", pVal.Row - 1)
                            Dim DocEntry As Integer = dataTableVehiculo.GetValue("docentry", pVal.Row - 1)

                        End If
                     
                    Case "btnUni"




                End Select
            ElseIf pVal.ActionSuccess Then


            End If
        End If


    End Sub

    Public Sub CargarDatosVehiculos()

        Dim strConsulta As String = String.Empty
        Dim dtVehiculos As DataTable

        Dim strTipoDocumento As String = String.Empty
        Dim strFechaContabilizacion As String = String.Empty
        Dim strDocEntry As String = String.Empty
        Dim strUnidad As String = String.Empty
        Dim strAsiento As String = String.Empty
        Dim strTotalEntradaLocal As String
        Dim strTotalEntradaSistema As String
        Dim strTotalSalidaLocal As String
        Dim strTotalSalidaSistema As String
        Dim strTipoInventario As String
        Dim strNombreInventario As String
        Dim strTrasladado As String
        Dim strIdVehiculo As String
        Dim dblMontoEntrada As String
        Dim blnValorNegativoEntrada As Boolean = False

        Dim strFechaD As String
        Dim strFechaH As String
        Dim fechaDesde As Date
        Dim fechaHasta As Date
        Dim strCodigoUnidad As String

        Dim strFechaContabilizacionEntrada As String
        Dim strFechaContabilizacionSalida As String
        Dim blnUsaFechas As Boolean = False
        Dim ConsultaFechas As String

        Dim strFechaDFormateada As String
        Dim strFechaHFormateada As String

        strFechaD = FormularioSBO.DataSources.UserDataSources.Item("FechaDesde").Value.ToString.Trim
        strFechaH = FormularioSBO.DataSources.UserDataSources.Item("FechaHasta").Value.ToString.Trim
        strCodigoUnidad = FormularioSBO.DataSources.UserDataSources.Item("unidad").Value.ToString.Trim

        Debito = 0
        Credito = 0
        SysDebito = 0
        SysCredito = 0

        If Not String.IsNullOrEmpty(strFechaD) AndAlso Not String.IsNullOrEmpty(strFechaH) Then

            strFechaDFormateada = Utilitarios.RetornaFechaFormatoDB(strFechaD, ApplicationSBO.Company.ServerName)
            strFechaHFormateada = Utilitarios.RetornaFechaFormatoDB(strFechaH, ApplicationSBO.Company.ServerName)

            strFechaContabilizacionEntrada = " and U_Fec_Cont between '" & strFechaDFormateada & "' and '" & strFechaHFormateada & "' "
            strFechaContabilizacionSalida = " and U_Fech_Con between '" & strFechaDFormateada & "' and '" & strFechaHFormateada & "' "

            blnUsaFechas = True

        End If


        Dim Consulta As String = "Select  'ENT' as TipoDocumento, U_SCGD_Trasl, U_Fec_Cont As FechaContabilizacion,  docentry, U_Unidad, U_As_Entr as Asiento, '' as Total_EntradaLocal, '' as Total_EntradaSistema, '' As Total_SalidaLocal, '' as Total_SalidaSistema, U_Tipo as Tipo, " & _
                        "(Select Name from [@SCGD_TIPOVEHICULO] with (nolock) WHERE code = U_Tipo) as Inventario, U_ID_Vehiculo as ID_Vehiculo, U_GASTRA as MontoEntrada " & _
                        "From [@SCGD_GOODRECEIVE] with (nolock) where U_Unidad = '" & strCodigoUnidad & "' {0} and " & _
                        "(U_EsTrasl = 'N' or U_EsTrasl is null) and (U_As_Entr is not null or U_As_Entr <> '') " & _
                        "Union All " & _
                        "Select  'SAL' as TipoDocumento, '', U_Fech_Con As FechaContabilizacion, docentry, U_Unidad, U_As_Sali as Asiento, '' as Total_EntradaLocal, '' as Total_EntradaSistema, '' As Total_SalidaLocal, '' as Total_SalidaSistema, (Select U_Tipo_Ven from [@SCGD_VEHICULO] where  code = U_ID_Veh) as Tipo, " & _
                        "(Select Name from [@SCGD_TIPOVEHICULO] with (nolock) WHERE code = (Select U_Tipo_Ven from [@SCGD_VEHICULO] with (nolock) where  code = U_ID_Veh)) as Inventario, U_ID_Veh as ID_Vehiculo, 0 as MontoEntrada " & _
                        "From [@SCGD_GOODISSUE] GI with (nolock) where U_Unidad = '" & strCodigoUnidad & "' {1} and (GI.U_As_Sali is not null or GI.U_As_Sali <> '') " & _
                        "Union All " & _
                        "Select  'TRL' as TipoDocumento, 'Y', U_SCGD_Fec As FechaContabilizacion, [@SCGD_TR_COSTOS].DocEntry, U_SCGD_Cod, [@SCGD_GOODRECEIVE].U_As_Entr as Asiento, '' as Total_EntradaLocal, '' as Total_EntradaSistema, '' As Total_SalidaLocal, '' as Total_SalidaSistema, U_SCGD_Inv as Tipo, " & _
                        "(Select Name from [@SCGD_TIPOVEHICULO] with (nolock) WHERE code = U_SCGD_Inv) as Inventario, '', 0 as MontoEntrada " & _
                        "FROM [@SCGD_TR_COSTOS] with (nolock) INNER JOIN " & _
                        "[@SCGD_TR_COSTOLINEAS] with (nolock) ON [@SCGD_TR_COSTOS].DocEntry = [@SCGD_TR_COSTOLINEAS].DocEntry INNER JOIN " & _
                        "[@SCGD_GOODRECEIVE] with (nolock) ON [@SCGD_TR_COSTOLINEAS].U_SCGD_EN = [@SCGD_GOODRECEIVE].DocEntry " & _
                        "where U_SCGD_Cod = '" & strCodigoUnidad & "' and (U_As_Entr is not null or U_As_Entr <> '') " & _
                        "Order by Asiento, U_Fec_Cont"


        ConsultaFechas = String.Format(Consulta, strFechaContabilizacionEntrada, strFechaContabilizacionSalida)

        MatrizVehiculo.Matrix.Clear()

        dtVehiculos = FormularioSBO.DataSources.DataTables.Item("VH")
        dtVehiculos.Rows.Clear()

        If dataTableVehiculo.Rows.Count > 0 Then
            dataTableVehiculo.Rows.Clear()
        End If

        dtVehiculos.ExecuteQuery(ConsultaFechas)

        FormularioSBO.Freeze(True)


        If Not dtVehiculos.IsEmpty Then

            For i As Integer = 0 To dtVehiculos.Rows.Count - 1

                strTotalEntradaLocal = String.Empty
                strTotalEntradaSistema = String.Empty
                strTotalSalidaLocal = String.Empty
                strTotalSalidaSistema = String.Empty

                strTipoDocumento = dtVehiculos.GetValue("TipoDocumento", i)
                strFechaContabilizacion = dtVehiculos.GetValue("FechaContabilizacion", i)
                strDocEntry = dtVehiculos.GetValue("docentry", i)
                strUnidad = dtVehiculos.GetValue("U_Unidad", i)
                strAsiento = dtVehiculos.GetValue("Asiento", i)
                strNombreInventario = dtVehiculos.GetValue("Inventario", i)
                strTrasladado = dtVehiculos.GetValue("U_SCGD_Trasl", i)
                strIdVehiculo = dtVehiculos.GetValue("ID_Vehiculo", i)
                dblMontoEntrada = dtVehiculos.GetValue("MontoEntrada", i)

                If dblMontoEntrada < 0 Then

                    blnValorNegativoEntrada = True

                End If

                CargarAsiento(strAsiento, blnValorNegativoEntrada)

                If strTipoDocumento = "ENT" Then
                    strTotalEntradaLocal = Convert.ToString(Debito, n)
                    strTotalEntradaSistema = Convert.ToString(SysCredito, n)
                ElseIf strTipoDocumento = "SAL" Then
                    strTotalSalidaLocal = Convert.ToString(Debito, n)
                    strTotalSalidaSistema = Convert.ToString(SysCredito, n)
                ElseIf strTipoDocumento = "TRL" Then
                    'strTotalEntradaLocal = Convert.ToString(Debito, n)
                    'strTotalEntradaSistema = Convert.ToString(SysCredito, n)
                End If

                strTipoInventario = dtVehiculos.GetValue("Tipo", i)


                dataTableVehiculo.Rows.Add()


                If Not String.IsNullOrEmpty(strTipoDocumento) Then

                    Select Case strTipoDocumento
                        Case "TRL"
                            dataTableVehiculo.SetValue("DescTraslado", i, " - Traslado de Costos - ")
                        Case "ENT"
                            dataTableVehiculo.SetValue("DescTraslado", i, " - Entrada Vehículo- ")
                        Case "SAL"
                            dataTableVehiculo.SetValue("DescTraslado", i, " - Salida Vehículo - ")

                    End Select

                    dataTableVehiculo.SetValue("TipoDocumento", i, dtVehiculos.GetValue("TipoDocumento", i))
                End If

                If Not String.IsNullOrEmpty(strFechaContabilizacion) Then
                    dataTableVehiculo.SetValue("FechaContabilizacion", i, dtVehiculos.GetValue("FechaContabilizacion", i))
                End If

                If Not String.IsNullOrEmpty(strDocEntry) Then
                    dataTableVehiculo.SetValue("docentry", i, dtVehiculos.GetValue("docentry", i))
                End If

                If Not String.IsNullOrEmpty(strUnidad) Then
                    dataTableVehiculo.SetValue("Unidad", i, dtVehiculos.GetValue("U_Unidad", i))
                End If

                If Not String.IsNullOrEmpty(strAsiento) Then
                    dataTableVehiculo.SetValue("Asiento", i, dtVehiculos.GetValue("Asiento", i))
                End If

                If Not String.IsNullOrEmpty(strTipoInventario) Then
                    dataTableVehiculo.SetValue("Tipo", i, strTipoInventario)
                End If

                If Not String.IsNullOrEmpty(strTotalEntradaLocal) Then
                    dataTableVehiculo.SetValue("Total_EntradaLocal", i, strTotalEntradaLocal)
                End If

                If Not String.IsNullOrEmpty(strTotalEntradaSistema) Then
                    dataTableVehiculo.SetValue("Total_EntradaSistema", i, strTotalEntradaSistema)
                End If
                If Not String.IsNullOrEmpty(strTotalSalidaLocal) Then
                    dataTableVehiculo.SetValue("Total_SalidaLocal", i, strTotalSalidaLocal)
                End If
                If Not String.IsNullOrEmpty(strTotalSalidaSistema) Then
                    dataTableVehiculo.SetValue("Total_SalidaSistema", i, strTotalSalidaSistema)
                End If

                If Not String.IsNullOrEmpty(strTrasladado) Then

                    dataTableVehiculo.SetValue("Trasladado", i, strTrasladado)

                    If strTrasladado = "Y" Then

                        If Not String.IsNullOrEmpty(strTotalEntradaLocal) Then
                            dataTableVehiculo.SetValue("ValorAcumulado", i, strTotalEntradaLocal)
                        End If

                    Else

                        If Not String.IsNullOrEmpty(strTotalEntradaLocal) Then
                            dataTableVehiculo.SetValue("ValorAcumulado", i, strTotalEntradaLocal)
                        End If


                    End If

                End If

                If Not String.IsNullOrEmpty(strNombreInventario) Then
                    dataTableVehiculo.SetValue("NombreInventario", i, strNombreInventario)
                End If

                If Not String.IsNullOrEmpty(strIdVehiculo) Then
                    dataTableVehiculo.SetValue("IdVehiculo", i, strIdVehiculo)
                End If

                blnValorNegativoEntrada = False

            Next


            LlenarValoresAcumulados(dataTableVehiculo)

        End If

        MatrizVehiculo.Matrix.LoadFromDataSource()

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub LlenarValoresAcumulados(ByRef p_dtValores As SAPbouiCOM.DataTable)

        Dim strEntradaLocal As String
        Dim dblValorAcumulado As Double
        Dim strTrasladado As String
        Dim strTipoDocumento As String
        Dim dblEntrada As Double
        Dim dblAnterior As Double


        For j As Integer = 0 To p_dtValores.Rows.Count - 1

            dblValorAcumulado = 0
            dblEntrada = 0
            strEntradaLocal = p_dtValores.GetValue("Total_EntradaLocal", j)
            strTrasladado = p_dtValores.GetValue("Trasladado", j)
            strTipoDocumento = p_dtValores.GetValue("TipoDocumento", j)

            dblValorAcumulado = Convert.ToDouble(strEntradaLocal, n)
            dblEntrada = Convert.ToDouble(strEntradaLocal, n)

            If strTipoDocumento = "ENT" Or strTipoDocumento = "TRL" Then
                If j <> 0 Then
                    dblAnterior = p_dtValores.GetValue("ValorAcumulado", j - 1)

                    If dblEntrada <> dblAnterior Then
                        dblValorAcumulado = dblValorAcumulado + dblAnterior
                        Dim strAcumulado As String = Convert.ToString(dblValorAcumulado, n).ToString
                        p_dtValores.SetValue("ValorAcumulado", j, dblValorAcumulado)
                    End If
                End If

            End If
        Next

    End Sub

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                             ByRef pval As SAPbouiCOM.ItemEvent, _
                                           ByRef BubbleEvent As Boolean)

        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_ID As String
        sCFL_ID = oCFLEvento.ChooseFromListUID

        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = FormularioSBO.ChooseFromLists.Item(sCFL_ID)

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        If oCFLEvento.BeforeAction = True Then
            Dim intBracket As Integer = 0
            Dim strDisponibilidad As String = String.Empty

            oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 0
            oCondition.Alias = "U_Cod_Unid"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCondition.BracketCloseNum = 0

            oCFL.SetConditions(oConditions)

        ElseIf oCFLEvento.ActionSuccess Then

            oDataTable = oCFLEvento.SelectedObjects

            Dim Unidad As String = oDataTable.GetValue("U_Cod_Unid", 0)

            strCodigoUnidad = Unidad.Trim()

            FormularioSBO.DataSources.UserDataSources.Item("unidad").Value = strCodigoUnidad

        End If

    End Sub

    Private Function CargarAsiento(ByVal p_NumAsiento As Integer, p_blnValorNegativo As Boolean) As LineasSumaAsientos

        Dim oListaCostos As New List(Of LineasSumaAsientos)()

        Try
            m_oJournalEntries = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            If m_oJournalEntries.GetByKey(p_NumAsiento) Then

                m_oJournalEntriesLines = m_oJournalEntries.Lines

                For i As Integer = 0 To m_oJournalEntriesLines.Count - 1

                    m_oJournalEntriesLines.SetCurrentLine(i)

                    Dim cuenta As String = m_oJournalEntriesLines.AccountCode

                    Dim dblDebito As Double = m_oJournalEntriesLines.Debit
                    Dim dblCredito As Double = m_oJournalEntriesLines.Credit
                    Dim dblDebitoSys As Double = m_oJournalEntriesLines.DebitSys
                    Dim dblCreditoSys As Double = m_oJournalEntriesLines.CreditSys

                    'los asientos con valores positivos cambian a negativo al 
                    'tratarse de una nota de credito

                    If p_blnValorNegativo Then
                        dblDebito = dblDebito * -1
                        dblCredito = dblCredito * -1
                        dblDebitoSys = dblDebitoSys * -1
                        dblCreditoSys = dblCreditoSys * -1
                    End If

                    oListaCostos.Add(New LineasSumaAsientos() With {.DebitoCosto = dblDebito,
                                                                    .CreditoCosto = dblCredito,
                                                                    .SysDebitoCosto = dblDebitoSys,
                                                                    .SysCreditoCosto = dblCreditoSys})
                Next

                Dim dblMontoTemp As Double

                Debito = 0
                Credito = 0
                SysCredito = 0
                SysDebito = 0

                For Each C1 As LineasSumaAsientos In oListaCostos

                    dblMontoTemp = 0

                    Debito = Debito + C1.DebitoCosto
                    Credito = Credito + C1.CreditoCosto
                    SysDebito = SysDebito + C1.SysDebitoCosto
                    SysCredito = SysCredito + C1.SysCreditoCosto

                Next

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Throw ex

        End Try
        Return Nothing
    End Function

#End Region

End Class

Public Class LineasSumaAsientos

    Public Property DebitoCosto() As Double
        Get
            Return dblDebitoCosto
        End Get
        Set(ByVal value As Double)
            dblDebitoCosto = value
        End Set
    End Property
    Private dblDebitoCosto As Decimal

    Public Property CreditoCosto() As Double
        Get
            Return dblCreditoCosto
        End Get
        Set(ByVal value As Double)
            dblCreditoCosto = value
        End Set
    End Property
    Private dblCreditoCosto As Decimal

    Public Property SysDebitoCosto() As Double
        Get
            Return dblSysDebitoCosto
        End Get
        Set(ByVal value As Double)
            dblSysDebitoCosto = value
        End Set
    End Property
    Private dblSysDebitoCosto As Double

    Public Property SysCreditoCosto() As Double
        Get
            Return dblSysCreditoCosto
        End Get
        Set(ByVal value As Double)
            dblSysCreditoCosto = value
        End Set
    End Property
    Private dblSysCreditoCosto As Double

End Class

Public Class ListaDocumentos

    Public strDocumento As String
    Public Property Documento As String
        Get
            Return strDocumento
        End Get
        Set(value As String)
            strDocumento = value
        End Set
    End Property

    Public intAsientoDocumento As Integer
    Public Property AsientoDocumento As Integer
        Get
            Return intAsientoDocumento
        End Get
        Set(value As Integer)
            intAsientoDocumento = value
        End Set
    End Property


End Class
