Imports DMSOneFramework.SCGBusinessLogic

Namespace SCGDataAccess

    Public Class CostosDataAdapter

#Region "Declaraciones"

        Private m_DAConexion As DMSOneFramework.SCGDataAccess.DAConexion
        Private m_cnnCostos As SqlClient.SqlConnection

        Private mc_strArroba As String = "@"

        ''Sps
        Private Const mc_strSCGTA_SP_SELGetDirectCostMO As String = "SCGTA_SP_SELGetDirectCostMO"
        Private Const mc_strSCGTA_SP_SELGetDirectCostSumin As String = "SCGTA_SP_SELGetDirectCostSumin"
        Private Const mc_strSCGTA_SP_SELGetDirectCostRepues As String = "SCGTA_SP_SELGetDirectCostRepues"
        Private Const mc_strSCGTA_SP_SELMovimientosCostos As String = "SCGTA_SP_SELMovimientosCostos"
        Private Const mc_strSCGTA_SP_SELCentrosCostoCostos As String = "SCGTA_SP_SELCentrosCostoCostos"
        Private Const mc_strSCGTA_SP_SELConceptosCostos As String = "SCGTA_SP_SELConceptosCostos"
        Private Const mc_strSCGTA_SP_SELCuentasConfig As String = "SCGTA_SP_SELCuentasConfig"
        Private Const mc_strSCGTA_SP_SELMovTipoCalculo As String = "SCGTA_SP_SELMovTipoCalculo"
        Private Const mc_strSCGTA_SP_INSConfCuentasDetalle As String = "SCGTA_SP_INSConfCuentasDetalle"
        Private Const mc_strSCGTA_SP_UPDConfCuentasDetalle As String = "SCGTA_SP_UPDConfCuentasDetalle"
        Private Const mc_strSCGTA_SP_DELConfCuentasDetalle As String = "SCGTA_SP_DELConfCuentasDetalle"
        Private Const mc_strSCGTA_SP_UPDCostoTipoCalculo As String = "SCGTA_SP_UPDCostoTipoCalculo"

        Private Const mc_strSCGTA_SP_SELOrdenCompraRep As String = "SCGTA_SP_SELOrdenCompraRep"
        Private Const mc_strSCGTA_SP_SELOrdenIsAsegurada As String = "SCGTA_SP_SELOrdenIsAsegurada"
        Private Const mc_strSCGTA_SP_SELOrdenesByExp As String = "SCGTA_SP_SELOrdenesByExp"
        Private Const mc_strSCGTA_SP_SELFaseXOrdenByNoOrdenNoFase As String = "SCGTA_SP_SELFaseXOrdenByNoOrdenNoFase"
        Private Const mc_strSCGTA_SP_UPDFaseXOrdenIsCostoCal As String = "SCGTA_SP_UPDFaseXOrdenIsCostoCal"

        Private Const mc_strSCGTA_SP_SELDebitosRealizadosByCuenta As String = "SCGTA_SP_SELDebitosRealizadosByCuenta"
        Private Const mc_strSCGTA_SP_UPDDebitosRealizadosByCuenta As String = "SCGTA_SP_UPDDebitosRealizadosByCuenta"

        Private Const mc_strSCGTA_SP_SelColaboradoresCostear As String = "SCGTA_SP_SelColaboradoresCostear"

        ''Fields
        Private Const mc_strNoExpediente As String = "NoExpediente"
        Private Const mc_strNoOrden As String = "NoOrden"
        Private Const mc_strID As String = "ID"
        Private Const mc_strIDAsiento As String = "IDAsiento"
        Private Const mc_strIDCentroCosto As String = "IDCentroCosto"
        Private Const mc_strIDConcepto As String = "IDConcepto"
        Private Const mc_strNoCuentaDebe As String = "NoCuentaDebe"
        Private Const mc_strNoCuentaHaber As String = "NoCuentaHaber"
        Private Const mc_strTasa As String = "Tasa"
        Private Const mc_strTasa2 As String = "Tasa2"
        Private Const mc_strTasa3 As String = "Tasa3"
        Private Const mc_strTasa4 As String = "Tasa4"
        Private Const mc_strPorPanel As String = "PorPanel"
        Private Const mc_strIndirecto As String = "IndIndirecto"

        Private Const mc_strIsCostoPanelCalculado As String = "IsCostoPanelCalculado"
        Private Const mc_strNoFase As String = "NoFase"
        Private Const mc_strNoPaneles As String = "NoPaneles"
        Private Const mc_strCostoMatIndirec As String = "CostoMatIndirec"
        Private Const mc_strCostoMOIndirec As String = "CostoMOIndirec"
        Private Const mc_strCostoGastosIndirec As String = "CostoGastosIndirec"

        Private Const mc_strU_Costo As String = "U_SCGD_Costo"

        ''PreValores
        Private Const mc_intNumMatIndirectos As Integer = 5
        Private Const mc_intNumMODirecta As Integer = 8
        Private Const mc_intNumMOIndirecta As Integer = 9
        Private Const mc_intNumGastosIndirectos As Integer = 10
        Private Const mc_intNumAlmacenamiento As Integer = 11
        Private Const mc_intNumOrdenesTerminadas As Integer = 12

        Private Enum me_TipoOrdenesInExp
            SoloPersonales = 0
            UnaAsegurada = 1
            VariasAseguradas = 2
        End Enum


#End Region

#Region "Constructor"

        Public Sub New()
            m_DAConexion = New DMSOneFramework.SCGDataAccess.DAConexion
        End Sub

#End Region

#Region "Costos Directos"

#Region "Procedimientos"

        Public Function GetCostoMODirecto(ByVal p_strNoOrden As String) As Decimal
            Dim cmdCostoMO As SqlClient.SqlCommand
            Dim decResult As Decimal

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdCostoMO = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELGetDirectCostMO, m_cnnCostos)

            With cmdCostoMO
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden
            End With

            decResult = CType(cmdCostoMO.ExecuteScalar, Decimal)

            Return decResult

        End Function

        Public Function GetCostoSuminDirecto(ByVal p_strNoOrden As String) As Decimal
            Dim cmdCostoSum As SqlClient.SqlCommand
            Dim decResult As Decimal

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdCostoSum = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELGetDirectCostSumin, m_cnnCostos)

            With cmdCostoSum
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden
            End With

            decResult = CType(cmdCostoSum.ExecuteScalar, Decimal)

            Return decResult

        End Function

        Public Function GetCostoRepuesDirecto(ByVal p_strNoOrden As String) As Decimal
            Dim cmdCostoSum As SqlClient.SqlCommand
            Dim decResult As Decimal

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdCostoSum = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELGetDirectCostRepues, m_cnnCostos)

            With cmdCostoSum
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden
            End With

            decResult = CType(cmdCostoSum.ExecuteScalar, Decimal)

            Return decResult

        End Function

        Private Sub GeneraAsientoMO(ByRef p_objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento, _
                                    ByRef p_drwColaborador As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow)

            'Dim dstDetallesAsientos As New DMSOneFramework.CuentasConfDetaDataset
            'Dim intTipoMov As Integer
            'Dim drwCuentaConfCollec() As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim drwCuentaConf As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim objAsientoLinea As DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta
            'Dim arrLineas As ArrayList

            'intTipoMov = SearchCuentasConf(mc_intNumMODirecta, dstDetallesAsientos)

            'drwCuentaConfCollec = dstDetallesAsientos.SCGTA_SP_SELCuentasConfig.Select("IDCentroCosto=" & p_drwColaborador.NoFase)

            'arrLineas = New ArrayList

            'For Each drwCuentaConf In drwCuentaConfCollec

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = 0
            '        .decDebit = p_drwColaborador.Costo
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoOrden = p_drwColaborador.NoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = p_drwColaborador.Costo
            '        .decDebit = 0
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoOrden = p_drwColaborador.NoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            'Next

            'p_objAsientoContable.arrLineas = arrLineas

        End Sub

#End Region

#Region "Comandos"

#End Region

#End Region

#Region "Costos Indirectos"

#Region "Procedimientos"

        Private Sub GeneraAsientoMatIndirectos(ByRef p_objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento, _
                                    ByRef p_drwColaborador As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow)

            'Dim dstDetallesAsientos As New DMSOneFramework.CuentasConfDetaDataset
            'Dim intTipoMov As Integer
            'Dim drwCuentaConfCollec() As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim drwCuentaConf As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim objAsientoLinea As DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta
            'Dim arrLineas As ArrayList

            'intTipoMov = SearchCuentasConf(mc_intNumMatIndirectos, dstDetallesAsientos)

            'drwCuentaConfCollec = dstDetallesAsientos.SCGTA_SP_SELCuentasConfig.Select("IDCentroCosto=" & p_drwColaborador.NoFase & " or IndIndirecto=1")

            'arrLineas = p_objAsientoContable.arrLineas

            'For Each drwCuentaConf In drwCuentaConfCollec

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = 0
            '        .decDebit = p_drwColaborador.TiempoHoras * drwCuentaConf.Tasa
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoOrden = p_drwColaborador.NoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = p_drwColaborador.TiempoHoras * drwCuentaConf.Tasa
            '        .decDebit = 0
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoOrden = p_drwColaborador.NoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            'Next

            'p_objAsientoContable.arrLineas = arrLineas

        End Sub

        Private Sub GeneraAsientoMOYGastosIndirectos(ByRef p_objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento, _
                                    ByRef p_drwColaborador As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow, _
                                    ByVal p_intTipoMovimiento As Integer, ByVal p_blnCompraRep As Boolean, ByVal p_blnAsegurado As Boolean)

            'Dim dstDetallesAsientos As New DMSOneFramework.CuentasConfDetaDataset
            'Dim intTipoMov As Integer
            'Dim drwCuentaConfCollec() As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim drwCuentaConf As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim objAsientoLinea As DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta
            'Dim arrLineas As ArrayList
            'Dim decTasaUtilizada As Decimal

            'intTipoMov = SearchCuentasConf(p_intTipoMovimiento, dstDetallesAsientos)

            'drwCuentaConfCollec = dstDetallesAsientos.SCGTA_SP_SELCuentasConfig.Select("IDCentroCosto=" & p_drwColaborador.NoFase & " or IndIndirecto=1")

            'arrLineas = p_objAsientoContable.arrLineas

            'For Each drwCuentaConf In drwCuentaConfCollec

            '    If p_blnAsegurado And p_blnCompraRep Then
            '        decTasaUtilizada = drwCuentaConf.Tasa
            '    ElseIf p_blnAsegurado And Not p_blnCompraRep Then
            '        decTasaUtilizada = drwCuentaConf.Tasa2
            '    ElseIf Not p_blnAsegurado And p_blnCompraRep Then
            '        decTasaUtilizada = drwCuentaConf.Tasa3
            '    Else
            '        decTasaUtilizada = drwCuentaConf.Tasa4
            '    End If

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = 0
            '        .decDebit = p_drwColaborador.TiempoHoras * decTasaUtilizada
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoOrden = p_drwColaborador.NoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = p_drwColaborador.TiempoHoras * decTasaUtilizada
            '        .decDebit = 0
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoOrden = p_drwColaborador.NoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            'Next

            'p_objAsientoContable.arrLineas = arrLineas

        End Sub

        Private Sub GeneraAsientoMatIndiPorPanel(ByRef p_objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento, _
                            ByVal p_decNoPaneles As Decimal, ByVal p_intNoFase As Integer, ByRef p_decTotalDebito As Decimal)

            'Dim dstDetallesAsientos As New DMSOneFramework.CuentasConfDetaDataset
            'Dim intTipoMov As Integer
            'Dim drwCuentaConfCollec() As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim drwCuentaConf As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim objAsientoLinea As DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta
            'Dim arrLineas As ArrayList

            'intTipoMov = SearchCuentasConf(mc_intNumMatIndirectos, dstDetallesAsientos)

            'drwCuentaConfCollec = dstDetallesAsientos.SCGTA_SP_SELCuentasConfig.Select("IDCentroCosto=" & p_intNoFase & " or IndIndirecto=1")

            'arrLineas = p_objAsientoContable.arrLineas

            'For Each drwCuentaConf In drwCuentaConfCollec

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = 0
            '        .decDebit = p_decNoPaneles * drwCuentaConf.Tasa
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoOrden = p_objAsientoContable.strNoOrden
            '        p_decTotalDebito += p_decNoPaneles * drwCuentaConf.Tasa
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = p_decNoPaneles * drwCuentaConf.Tasa
            '        .decDebit = 0
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoOrden = p_objAsientoContable.strNoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            'Next

            'p_objAsientoContable.arrLineas = arrLineas

        End Sub

        Private Sub GeneraAsientoMOYGastosIndiPorPanel(ByRef p_objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento, _
                                    ByVal p_decPaneles As Decimal, ByVal p_intNoFase As Integer, _
                                    ByVal p_intTipoMovimiento As Integer, ByVal p_blnCompraRep As Boolean, ByVal p_blnAsegurado As Boolean, _
                                    ByRef p_decTotalDebito As Decimal)

            'Dim dstDetallesAsientos As New DMSOneFramework.CuentasConfDetaDataset
            'Dim intTipoMov As Integer
            'Dim drwCuentaConfCollec() As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim drwCuentaConf As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim objAsientoLinea As DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta
            'Dim arrLineas As ArrayList
            'Dim decTasaUtilizada As Decimal

            'intTipoMov = SearchCuentasConf(p_intTipoMovimiento, dstDetallesAsientos)

            'drwCuentaConfCollec = dstDetallesAsientos.SCGTA_SP_SELCuentasConfig.Select("IDCentroCosto=" & p_intNoFase & " or IndIndirecto=1")

            'arrLineas = p_objAsientoContable.arrLineas

            'For Each drwCuentaConf In drwCuentaConfCollec

            '    If p_blnAsegurado And p_blnCompraRep Then
            '        decTasaUtilizada = drwCuentaConf.Tasa
            '    ElseIf p_blnAsegurado And Not p_blnCompraRep Then
            '        decTasaUtilizada = drwCuentaConf.Tasa2
            '    ElseIf Not p_blnAsegurado And p_blnCompraRep Then
            '        decTasaUtilizada = drwCuentaConf.Tasa3
            '    Else
            '        decTasaUtilizada = drwCuentaConf.Tasa4
            '    End If

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = 0
            '        .decDebit = p_decPaneles * decTasaUtilizada
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoOrden = p_objAsientoContable.strNoOrden
            '        p_decTotalDebito += p_decPaneles * decTasaUtilizada
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea
            '        .intCentroCosto = drwCuentaConf.IDCentroCosto
            '        .decCredit = p_decPaneles * decTasaUtilizada
            '        .decDebit = 0
            '        .strNoContraCuenta = drwCuentaConf.NoCuentaDebe
            '        .strNoCuenta = drwCuentaConf.NoCuentaHaber
            '        .strNoOrden = p_objAsientoContable.strNoOrden
            '    End With

            '    arrLineas.Add(objAsientoLinea)

            'Next

            'p_objAsientoContable.arrLineas = arrLineas

        End Sub

#End Region

#End Region

#Region "Costos Cierre Orden"

        Public Sub CostosPorCierre(ByVal p_strNoOrden As String, ByVal p_strNoCotizacion As String, ByVal p_intCosteoServicios As Integer, ByVal p_intCodTipodOrden As Integer)
            Dim dstColaboradoresCostear As New ColaboradoresCostearDataSet
            Dim drwColaborC As ColaboradoresCostearDataSet.ColaboradoresCostearRow
            Dim adpColaborC As New SqlClient.SqlDataAdapter
            Dim cmdCC As SqlClient.SqlCommand

            Dim blnRealizaCosto As Boolean = False
            Dim blnTipoCalculoEstandar As Boolean = False
            Dim strCentroBeneficio As String = String.Empty

            Dim decTotal As Decimal

            Dim clsCostosSBO As CostosSBO
            Dim objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento = Nothing
            Dim objCuentaContable As SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            Try

                m_cnnCostos = m_DAConexion.ObtieneConexion

                cmdCC = New SqlClient.SqlCommand(mc_strSCGTA_SP_SelColaboradoresCostear, m_cnnCostos)

                With cmdCC
                    .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                    .CommandType = CommandType.StoredProcedure
                End With

                adpColaborC.SelectCommand = cmdCC

                adpColaborC.Fill(dstColaboradoresCostear.ColaboradoresCostear)

                If p_intCosteoServicios <> 0 Then

                    blnRealizaCosto = True

                    If p_intCosteoServicios = 1 Then

                        blnTipoCalculoEstandar = True

                    Else

                        blnTipoCalculoEstandar = False

                    End If

                Else

                    blnRealizaCosto = False

                End If

                Dim objTransf As TransferenciaItems = New TransferenciaItems(G_objCompany)
                strCentroBeneficio = objTransf.RetornaCentroBeneficioByTipoOrden(p_intCodTipodOrden)

                If dstColaboradoresCostear.ColaboradoresCostear.Rows.Count <> 0 And blnRealizaCosto Then
                    objAsientoContable.strNoOrden = p_strNoOrden
                    objAsientoContable.arrLineas = New ArrayList

                    For Each drwColaborC In dstColaboradoresCostear.ColaboradoresCostear

                        If blnTipoCalculoEstandar Then
                            decTotal = drwColaborC.Duracion * drwColaborC.SalXHora / 60
                        Else
                            decTotal = drwColaborC.Tiempo * drwColaborC.SalXHora / 60
                        End If

                        objCuentaContable = New SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

                        With objCuentaContable
                            .strNoOrden = p_strNoOrden
                            .strNoCuenta = drwColaborC.CuentaDebe
                            .strNoContraCuenta = drwColaborC.CuentaHaber
                            .decDebit = decTotal
                            .decCredit = 0
                            .strRef2 = drwColaborC.ItemCode
                            If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficio = objTransf.RetornaCentroBeneficioByItem(drwColaborC.ItemCode)
                            If Not String.IsNullOrEmpty(strCentroBeneficio) Then .strCostingCode = strCentroBeneficio
                        End With

                        objAsientoContable.arrLineas.Add(objCuentaContable)

                        objCuentaContable = New SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

                        With objCuentaContable
                            .strNoOrden = p_strNoOrden
                            .strNoCuenta = drwColaborC.CuentaHaber
                            .strNoContraCuenta = drwColaborC.CuentaDebe
                            .decDebit = 0
                            .decCredit = decTotal
                            .strRef2 = drwColaborC.ItemCode
                            If String.IsNullOrEmpty(strCentroBeneficio) Then strCentroBeneficio = objTransf.RetornaCentroBeneficioByItem(drwColaborC.ItemCode)
                            If Not String.IsNullOrEmpty(strCentroBeneficio) Then .strCostingCode = strCentroBeneficio
                        End With

                        objAsientoContable.arrLineas.Add(objCuentaContable)

                    Next

                End If

                m_cnnCostos.Close()

                clsCostosSBO = New CostosSBO
                If objAsientoContable.arrLineas IsNot Nothing Then
                    If clsCostosSBO.CrearAsientoContable(objAsientoContable) = 0 Then

                        ActualizarCostoCotizacion(p_strNoOrden, p_strNoCotizacion, dstColaboradoresCostear, blnTipoCalculoEstandar)

                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Private Sub ActualizarCostoCotizacion(ByVal p_strNoOrden As String, ByVal p_strNoCotizacion As String, ByVal p_dstColaboradoresCostear As ColaboradoresCostearDataSet, _
                            ByVal p_blnCalculaTiempoEstandar As Boolean)
            Dim drwCC As ColaboradoresCostearDataSet.ColaboradoresCostearRow
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim intResult As Integer
            Dim strMensajeError As String

            oCotizacion = BLSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If oCotizacion.GetByKey(p_strNoCotizacion) Then

                m_cnnCostos = m_DAConexion.ObtieneConexion

                For Each drwCC In p_dstColaboradoresCostear.ColaboradoresCostear
                    With oCotizacion

                        .Lines.SetCurrentLine(drwCC.LineNum)

                        If Not .Lines.UserFields.Fields.Item(mc_strU_Costo).Value Is DBNull.Value Then
                            If p_blnCalculaTiempoEstandar Then
                                .Lines.UserFields.Fields.Item(mc_strU_Costo).Value += (drwCC.Duracion * (drwCC.SalXHora / 60))
                            Else
                                .Lines.UserFields.Fields.Item(mc_strU_Costo).Value += (drwCC.Tiempo * (drwCC.SalXHora / 60))
                            End If
                        Else
                            If p_blnCalculaTiempoEstandar Then
                                .Lines.UserFields.Fields.Item(mc_strU_Costo).Value = (drwCC.Duracion * (drwCC.SalXHora / 60))
                            Else
                                .Lines.UserFields.Fields.Item(mc_strU_Costo).Value = (drwCC.Tiempo * (drwCC.SalXHora / 60))
                            End If
                        End If

                    End With
                Next

                intResult = oCotizacion.Update()

                If intResult <> 0 Then

                    strMensajeError = intResult.ToString & " " & BLSBO.oCompany.GetLastErrorDescription

                End If

            End If

        End Sub

#End Region

#Region "Costos Cierre Mensual"

        Public Sub CostosPorCierreMensual()

        End Sub

#End Region

#Region "Costos Inicio Fase"

        Public Sub CostosPorInicioFase(ByVal p_strNoOrden As String, ByVal p_intNoFase As Integer)
            Dim clsCostosSBO As CostosSBO

            Dim drdFaseXOrden As SqlClient.SqlDataReader
            Dim cmdFasesXOrden As SqlClient.SqlCommand

            Dim blnCalcMatIndirectos As Boolean = False
            Dim blnCalcMOIndirecta As Boolean = False
            Dim blnCalcGastosIndirectos As Boolean = False
            Dim drdMovimientos As SqlClient.SqlDataReader

            Dim objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento
            Dim objCuentaContable As SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta =  Nothing
            Dim blnCompraRep As Boolean
            Dim blnAsegurada As Boolean
            Dim intResultSBO As Integer
            Dim decTotalMatInderec As Decimal = 0
            Dim decTotalMOInderec As Decimal = 0
            Dim decTotalGastosInderec As Decimal = 0

            Dim decNoPaneles As Decimal

            cmdFasesXOrden = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELFaseXOrdenByNoOrdenNoFase)

            cmdFasesXOrden.Connection = m_DAConexion.ObtieneConexion
            cmdFasesXOrden.CommandType = CommandType.StoredProcedure

            With cmdFasesXOrden.Parameters
                .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int).Value = p_intNoFase
            End With

            drdFaseXOrden = cmdFasesXOrden.ExecuteReader

            If drdFaseXOrden.Read Then

                If drdFaseXOrden.Item(mc_strIsCostoPanelCalculado) = 0 Then

                    decNoPaneles = CDec(drdFaseXOrden.Item(mc_strNoPaneles))

                    cmdFasesXOrden.Connection.Close()

                    drdMovimientos = GetListMovimientos()

                    While drdMovimientos.Read

                        If drdMovimientos.Item("ID") = mc_intNumMatIndirectos Then

                            If drdMovimientos.Item(mc_strPorPanel) = 1 Then
                                blnCalcMatIndirectos = True
                            End If

                        ElseIf drdMovimientos.Item("ID") = mc_intNumMOIndirecta Then

                            If drdMovimientos.Item(mc_strPorPanel) = 1 Then
                                blnCalcMOIndirecta = True
                            End If

                        ElseIf drdMovimientos.Item("ID") = mc_intNumGastosIndirectos Then

                            If drdMovimientos.Item(mc_strPorPanel) = 1 Then
                                blnCalcGastosIndirectos = True
                            End If

                        End If

                    End While

                    drdMovimientos.Close()

                    objAsientoContable.strNoOrden = p_strNoOrden

                    blnCompraRep = GetOrdenCompraRep(objAsientoContable.strNoOrden)

                    blnAsegurada = GetOrdenIsAsegurada(objAsientoContable.strNoOrden)

                    objAsientoContable.arrLineas = New ArrayList

                    If blnCalcMatIndirectos Then

                        GeneraAsientoMatIndiPorPanel(objAsientoContable, decNoPaneles, p_intNoFase, decTotalMatInderec)

                    End If

                    If blnCalcMOIndirecta Then

                        GeneraAsientoMOYGastosIndiPorPanel(objAsientoContable, decNoPaneles, p_intNoFase, mc_intNumMOIndirecta, blnCompraRep, blnAsegurada, decTotalMOInderec)

                    End If

                    If blnCalcGastosIndirectos Then

                        GeneraAsientoMOYGastosIndiPorPanel(objAsientoContable, decNoPaneles, p_intNoFase, mc_intNumGastosIndirectos, blnCompraRep, blnAsegurada, decTotalGastosInderec)

                    End If

                    clsCostosSBO = New CostosSBO

                    intResultSBO = clsCostosSBO.CrearAsientoContable(objAsientoContable)

                    If intResultSBO = 0 Then

                        cmdFasesXOrden.Dispose()

                        cmdFasesXOrden = New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDFaseXOrdenIsCostoCal)

                        cmdFasesXOrden.Connection = m_DAConexion.ObtieneConexion
                        cmdFasesXOrden.CommandType = CommandType.StoredProcedure

                        With cmdFasesXOrden.Parameters
                            .Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                            .Add(mc_strArroba & mc_strNoFase, SqlDbType.Int).Value = p_intNoFase
                            .Add(mc_strArroba & mc_strCostoMatIndirec, SqlDbType.Decimal).Value = decTotalMatInderec
                            .Add(mc_strArroba & mc_strCostoMOIndirec, SqlDbType.Decimal).Value = decTotalMOInderec
                            .Add(mc_strArroba & mc_strCostoGastosIndirec, SqlDbType.Decimal).Value = decTotalGastosInderec
                        End With

                        cmdFasesXOrden.ExecuteNonQuery()

                        cmdFasesXOrden.Connection.Close()

                    End If


                End If

            End If

        End Sub

#End Region

#Region "Costos Day to Day"

        Public Sub CostosPorDtD(ByRef p_dtbColaborador As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorDataTable)
            Dim clsCostosSBO As CostosSBO
            Dim blnCalcMatIndirectos As Boolean = False
            Dim blnCalcMOIndirecta As Boolean = False
            Dim blnCalcGastosIndirectos As Boolean = False
            Dim drdMovimientos As SqlClient.SqlDataReader
            Dim drwColaborador As DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow
            Dim objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento = Nothing
            Dim blnCompraRep As Boolean
            Dim blnAsegurada As Boolean

            drdMovimientos = GetListMovimientos()

            While drdMovimientos.Read

                If drdMovimientos.Item("ID") = mc_intNumMatIndirectos Then

                    If drdMovimientos.Item(mc_strPorPanel) = 1 Then
                        blnCalcMatIndirectos = True
                    End If

                ElseIf drdMovimientos.Item("ID") = mc_intNumMOIndirecta Then

                    If drdMovimientos.Item(mc_strPorPanel) = 1 Then
                        blnCalcMOIndirecta = True
                    End If

                ElseIf drdMovimientos.Item("ID") = mc_intNumGastosIndirectos Then

                    If drdMovimientos.Item(mc_strPorPanel) = 1 Then
                        blnCalcGastosIndirectos = True
                    End If

                End If

            End While

            drdMovimientos.Close()

            If p_dtbColaborador.Count <> 0 Then

                objAsientoContable.strNoOrden = CType(p_dtbColaborador.Rows(0), DMSOneFramework.ColaboradorDataset.SCGTA_TB_ControlColaboradorRow).NoOrden

                blnCompraRep = GetOrdenCompraRep(objAsientoContable.strNoOrden)

                blnAsegurada = GetOrdenIsAsegurada(objAsientoContable.strNoOrden)

                For Each drwColaborador In p_dtbColaborador

                    GeneraAsientoMO(objAsientoContable, drwColaborador)

                    If Not blnCalcMatIndirectos Then

                        GeneraAsientoMatIndirectos(objAsientoContable, drwColaborador)

                    End If

                    If Not blnCalcMOIndirecta Then

                        GeneraAsientoMOYGastosIndirectos(objAsientoContable, drwColaborador, mc_intNumMOIndirecta, blnCompraRep, blnAsegurada)

                    End If

                    If Not blnCalcGastosIndirectos Then

                        GeneraAsientoMOYGastosIndirectos(objAsientoContable, drwColaborador, mc_intNumGastosIndirectos, blnCompraRep, blnAsegurada)

                    End If

                    clsCostosSBO = New CostosSBO

                    clsCostosSBO.CrearAsientoContable(objAsientoContable)

                Next

            End If

        End Sub

#End Region

#Region "Costo de Almacenamiento"

        'Public Function CostoAlmacenamiento(ByVal p_intTotalDias As Decimal, ByRef p_drwOrden As DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenRow, ByVal p_dtFechaFinMes As Date) As Decimal
        '    Dim decPorcRepartir As Decimal
        '    Dim decCostoFinal As Decimal
        '    Dim decCostoAnterior As Decimal

        '    Dim clsCostosSBO As CostosSBO

        '    Dim objAsientoContable As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento
        '    Dim objAsientoLinea As SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

        '    Dim dstCuentasConfig As New DMSOneFramework.CuentasConfDetaDataset
        '    Dim drwCuentasConfigArray() As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
        '    Dim drwCuentasConfig As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow

        '    SearchCuentasConf(mc_intNumAlmacenamiento, dstCuentasConfig)

        '    drwCuentasConfigArray = dstCuentasConfig.SCGTA_SP_SELCuentasConfig.Select("IDCentroCosto=9")

        '    objAsientoContable.strNoOrden = p_drwOrden.NoOrden
        '    objAsientoContable.arrLineas = New ArrayList

        '    For Each drwCuentasConfig In drwCuentasConfigArray

        '        objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

        '        decPorcRepartir = DefinirPorcOrdenActual(p_drwOrden, p_dtFechaFinMes)

        '        decCostoFinal = Math.Round((p_intTotalDias * drwCuentasConfig.Tasa) * (decPorcRepartir / 100), 4)

        '        If p_drwOrden.IsCostoAlmacenamientoNull Then
        '            decCostoAnterior = 0
        '        Else
        '            decCostoAnterior = p_drwOrden.CostoAlmacenamiento
        '        End If

        '        With objAsientoLinea

        '            .intCentroCosto = drwCuentasConfig.IDCentroCosto
        '            .decCredit = 0
        '            .decDebit = decCostoFinal - decCostoAnterior
        '            .strNoContraCuenta = drwCuentasConfig.NoCuentaHaber
        '            .strNoCuenta = drwCuentasConfig.NoCuentaDebe
        '            .strNoOrden = p_drwOrden.NoOrden

        '        End With

        '        objAsientoContable.arrLineas.Add(objAsientoLinea)

        '        objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

        '        With objAsientoLinea

        '            .intCentroCosto = drwCuentasConfig.IDCentroCosto
        '            .decCredit = decCostoFinal - decCostoAnterior
        '            .decDebit = 0
        '            .strNoContraCuenta = drwCuentasConfig.NoCuentaDebe
        '            .strNoCuenta = drwCuentasConfig.NoCuentaHaber
        '            .strNoOrden = p_drwOrden.NoOrden

        '        End With

        '        objAsientoContable.arrLineas.Add(objAsientoLinea)

        '    Next

        '    clsCostosSBO = New CostosSBO

        '    clsCostosSBO.CrearAsientoContable(objAsientoContable)

        '    Return decCostoFinal

        'End Function

        'Private Function DefinirPorcOrdenActual(ByRef p_drwOrden As DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenRow, _
        '                                        ByVal p_dtFechaFinMes As Date) As Decimal

        '    Dim cmdOrdenes As SqlClient.SqlCommand
        '    Dim adpOrdenes As SqlClient.SqlDataAdapter
        '    Dim dtbOrdenes As DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenDataTable
        '    Dim drwOrden As DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenRow

        '    Dim decTotalDuracionAseg As Decimal
        '    Dim decTotalDuracionPers As Decimal

        '    Dim decResultPorc As Decimal
        '    Dim enumIndTipoExpedientes As me_TipoOrdenesInExp
        '    Dim dtFechaFin As Date

        '    cmdOrdenes = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrdenesByExp)

        '    adpOrdenes = New SqlClient.SqlDataAdapter
        '    dtbOrdenes = New DMSOneFramework.OrdenTrabajoDataset.SCGTA_TB_OrdenDataTable

        '    With cmdOrdenes

        '        .CommandType = CommandType.StoredProcedure
        '        .Connection = m_DAConexion.ObtieneConexion
        '        .Parameters.Add(mc_strArroba & mc_strNoExpediente, SqlDbType.Int).Value = p_drwOrden.NoVisita

        '    End With

        '    adpOrdenes.SelectCommand = cmdOrdenes

        '    adpOrdenes.Fill(dtbOrdenes)

        '    cmdOrdenes.Connection.Close()

        '    If dtbOrdenes.Rows.Count = 1 Then

        '        decResultPorc = 100

        '    Else

        '        enumIndTipoExpedientes = me_TipoOrdenesInExp.SoloPersonales

        '        For Each drwOrden In dtbOrdenes.Rows

        '            If drwOrden.IsFecha_cierreNull Then
        '                dtFechaFin = p_dtFechaFinMes
        '            Else
        '                dtFechaFin = drwOrden.Fecha_cierre
        '            End If

        '            If drwOrden.IndAsegurada = 1 Then

        '                decTotalDuracionAseg += CInt(dtFechaFin.Subtract(drwOrden.Fecha_apertura).TotalDays)

        '                If enumIndTipoExpedientes = me_TipoOrdenesInExp.UnaAsegurada Then
        '                    enumIndTipoExpedientes = me_TipoOrdenesInExp.VariasAseguradas
        '                Else
        '                    enumIndTipoExpedientes = me_TipoOrdenesInExp.UnaAsegurada
        '                End If

        '            Else

        '                decTotalDuracionPers += CInt(dtFechaFin.Subtract(drwOrden.Fecha_apertura).TotalDays)

        '            End If

        '        Next

        '        If enumIndTipoExpedientes = me_TipoOrdenesInExp.SoloPersonales Then

        '            If p_drwOrden.IsFecha_cierreNull Then
        '                dtFechaFin = p_dtFechaFinMes
        '            Else
        '                dtFechaFin = p_drwOrden.Fecha_cierre
        '            End If

        '            decResultPorc = (CInt(dtFechaFin.Subtract(p_drwOrden.Fecha_apertura).TotalDays) * 100) / decTotalDuracionPers

        '        ElseIf enumIndTipoExpedientes = me_TipoOrdenesInExp.UnaAsegurada Then

        '            If p_drwOrden.IndAsegurada = 1 Then
        '                decResultPorc = 100
        '            Else
        '                decResultPorc = 0
        '            End If

        '        Else

        '            If p_drwOrden.IsFecha_cierreNull Then
        '                dtFechaFin = p_dtFechaFinMes
        '            Else
        '                dtFechaFin = p_drwOrden.Fecha_cierre
        '            End If

        '            decResultPorc = (CInt(dtFechaFin.Subtract(p_drwOrden.Fecha_apertura).TotalDays) * 100) / decTotalDuracionAseg

        '        End If

        '    End If

        '    Return Math.Round(decResultPorc, 4)


        'End Function

#End Region

#Region "Costo Ordenes Terminados"

        Public Sub CrearAsientoOrdenesTerminadas(ByVal p_strNoOrden As String)
            'Dim dstCuentasConf As DMSOneFramework.CuentasConfDetaDataset
            'Dim drwCuentas As DMSOneFramework.CuentasConfDetaDataset.SCGTA_SP_SELCuentasConfigRow
            'Dim cmdCostoAcumulado As SqlClient.SqlCommand

            'Dim decMonto As Decimal
            'Dim decMontoTotal As Decimal
            'Dim objAsiento As SCGDataAccess.CostosSBO.G_Type_EsquemaAsiento
            'Dim objAsientoLinea As SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta
            'Dim intResultOrdenTerminada As Integer

            'Dim objParametroNoOrden As SqlClient.SqlParameter
            'Dim objParametroNoCuenta As SqlClient.SqlParameter

            'Dim clsCostosSBO As CostosSBO

            'dstCuentasConf = New DMSOneFramework.CuentasConfDetaDataset

            'SearchCuentasConf(mc_intNumOrdenesTerminadas, dstCuentasConf)

            'cmdCostoAcumulado = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELDebitosRealizadosByCuenta)

            'With cmdCostoAcumulado

            '    .Connection = m_DAConexion.ObtieneConexion
            '    .CommandType = CommandType.StoredProcedure
            '    objParametroNoOrden = .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)
            '    objParametroNoCuenta = .Parameters.Add(mc_strArroba & mc_strNoCuentaDebe, SqlDbType.VarChar, 50)

            'End With

            'objAsiento.strNoOrden = p_strNoOrden
            'objAsiento.arrLineas = New ArrayList

            'For Each drwCuentas In dstCuentasConf.SCGTA_SP_SELCuentasConfig

            '    objParametroNoOrden.Value = p_strNoOrden
            '    objParametroNoCuenta.Value = drwCuentas.NoCuentaHaber

            '    decMonto = CDec(cmdCostoAcumulado.ExecuteScalar)

            '    decMontoTotal += decMonto

            '    objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            '    With objAsientoLinea

            '        .intCentroCosto = drwCuentas.IDCentroCosto
            '        .decCredit = decMonto
            '        .decDebit = 0
            '        .strNoContraCuenta = drwCuentas.NoCuentaDebe
            '        .strNoCuenta = drwCuentas.NoCuentaHaber
            '        .strNoOrden = p_strNoOrden

            '    End With

            '    objAsiento.arrLineas.Add(objAsientoLinea)

            'Next

            'objAsientoLinea = New DMSOneFramework.SCGDataAccess.CostosSBO.G_Type_EsquemaCuenta

            'With objAsientoLinea

            '    .intCentroCosto = 0
            '    .decCredit = 0
            '    .decDebit = decMontoTotal
            '    .strNoContraCuenta = ""
            '    .strNoCuenta = drwCuentas.NoCuentaDebe
            '    .strNoOrden = p_strNoOrden

            'End With

            'objAsiento.arrLineas.Add(objAsientoLinea)

            'clsCostosSBO = New CostosSBO

            'intResultOrdenTerminada = clsCostosSBO.CrearAsientoContable(objAsiento)

            'If intResultOrdenTerminada = 0 Then

            '    ActualizarAsientosUtilizados(p_strNoOrden, cmdCostoAcumulado)

            'End If

            'If cmdCostoAcumulado.Connection.State <> ConnectionState.Closed Then
            '    cmdCostoAcumulado.Connection.Close()
            'End If

        End Sub

        Private Sub ActualizarAsientosUtilizados(ByVal p_strNoOrden As String, ByRef p_cmdCostoAcumulado As SqlClient.SqlCommand)

            With p_cmdCostoAcumulado

                .Parameters.Clear()

                .Parameters.Add(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50).Value = p_strNoOrden
                .CommandText = mc_strSCGTA_SP_UPDDebitosRealizadosByCuenta
                .ExecuteNonQuery()

            End With

        End Sub

#End Region

#Region "General"

#Region "Procedimientos"

        Public Function GetListMovimientos() As SqlClient.SqlDataReader
            Dim cmdMovimientos As SqlClient.SqlCommand
            Dim drdMovimientos As SqlClient.SqlDataReader

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdMovimientos = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELMovimientosCostos, m_cnnCostos)

            With cmdMovimientos
                .CommandType = CommandType.StoredProcedure
            End With

            drdMovimientos = cmdMovimientos.ExecuteReader(CommandBehavior.CloseConnection)

            Return drdMovimientos

        End Function

        Public Function GetListCentrosCosto() As SqlClient.SqlDataReader
            Dim cmdCentrosCosto As SqlClient.SqlCommand
            Dim drdCentrosCosto As SqlClient.SqlDataReader

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdCentrosCosto = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCentrosCostoCostos, m_cnnCostos)

            With cmdCentrosCosto
                .CommandType = CommandType.StoredProcedure
            End With

            drdCentrosCosto = cmdCentrosCosto.ExecuteReader(CommandBehavior.CloseConnection)

            Return drdCentrosCosto

        End Function

        Public Function GetListConceptos() As SqlClient.SqlDataReader
            Dim cmdConceptos As SqlClient.SqlCommand
            Dim drdConceptos As SqlClient.SqlDataReader

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdConceptos = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELConceptosCostos, m_cnnCostos)

            With cmdConceptos
                .CommandType = CommandType.StoredProcedure
            End With

            drdConceptos = cmdConceptos.ExecuteReader(CommandBehavior.CloseConnection)

            Return drdConceptos

        End Function

        Public Function SearchCuentasConf(ByVal p_intIDAsiento As Integer, ByRef p_dstCuentasConf As DMSOneFramework.CuentasConfDetaDataset) As Integer
            Dim adpCuentasConf As SqlClient.SqlDataAdapter
            Dim cmdCuentasConf As SqlClient.SqlCommand =  Nothing
            Dim intResult As Integer

            Try

                cmdCuentasConf = CrearCommandoSearchCuentasConf(p_intIDAsiento)

                adpCuentasConf = New SqlClient.SqlDataAdapter

                adpCuentasConf.SelectCommand = cmdCuentasConf

                adpCuentasConf.Fill(p_dstCuentasConf.SCGTA_SP_SELCuentasConfig)

                CrearCommandoMovTipoCalculo(cmdCuentasConf)

                intResult = CInt(cmdCuentasConf.ExecuteScalar)

                Return intResult

            Catch ex As Exception
                Throw ex
            Finally
                If Not cmdCuentasConf Is Nothing Then
                    If Not cmdCuentasConf.Connection Is Nothing Then
                        If cmdCuentasConf.Connection.State <> ConnectionState.Closed Then
                            cmdCuentasConf.Connection.Close()
                        End If
                    End If
                End If
            End Try
        End Function

        Public Sub InsertCuentasConf(ByRef p_dstCuentasConf As DMSOneFramework.CuentasConfDetaDataset)
            Dim adpCuentasConf As SqlClient.SqlDataAdapter
            Dim cmdCuentasConf As SqlClient.SqlCommand =  Nothing

            Try

                cmdCuentasConf = CrearCommandoInsModCuentasConf(mc_strSCGTA_SP_INSConfCuentasDetalle)

                adpCuentasConf = New SqlClient.SqlDataAdapter

                With cmdCuentasConf.Parameters(mc_strArroba & mc_strID)
                    .Direction = ParameterDirection.Output
                End With

                adpCuentasConf.InsertCommand = cmdCuentasConf

                adpCuentasConf.Update(p_dstCuentasConf.SCGTA_SP_SELCuentasConfig)

            Catch ex As Exception
                Throw ex
            Finally
                If Not cmdCuentasConf Is Nothing Then
                    If Not cmdCuentasConf.Connection Is Nothing Then
                        If cmdCuentasConf.Connection.State <> ConnectionState.Closed Then
                            cmdCuentasConf.Connection.Close()
                        End If
                    End If
                End If
            End Try
        End Sub

        Public Sub ModifyCuentasConf(ByRef p_dstCuentasConf As DMSOneFramework.CuentasConfDetaDataset)
            Dim adpCuentasConf As SqlClient.SqlDataAdapter
            Dim cmdCuentasConf As SqlClient.SqlCommand =  Nothing

            Try

                cmdCuentasConf = CrearCommandoInsModCuentasConf(mc_strSCGTA_SP_UPDConfCuentasDetalle)

                adpCuentasConf = New SqlClient.SqlDataAdapter

                adpCuentasConf.UpdateCommand = cmdCuentasConf

                adpCuentasConf.Update(p_dstCuentasConf.SCGTA_SP_SELCuentasConfig)

            Catch ex As Exception
                Throw ex
            Finally
                If Not cmdCuentasConf Is Nothing Then
                    If Not cmdCuentasConf.Connection Is Nothing Then
                        If cmdCuentasConf.Connection.State <> ConnectionState.Closed Then
                            cmdCuentasConf.Connection.Close()
                        End If
                    End If
                End If
            End Try
        End Sub

        Public Sub DeleteCuentasConf(ByRef p_dstCuentasConf As DMSOneFramework.CuentasConfDetaDataset)
            Dim adpCuentasConf As SqlClient.SqlDataAdapter
            Dim cmdCuentasConf As SqlClient.SqlCommand =  Nothing

            Try

                cmdCuentasConf = CrearCommandoDeleteCuentasConf(mc_strSCGTA_SP_DELConfCuentasDetalle)

                adpCuentasConf = New SqlClient.SqlDataAdapter

                adpCuentasConf.DeleteCommand = cmdCuentasConf

                adpCuentasConf.Update(p_dstCuentasConf.SCGTA_SP_SELCuentasConfig)

            Catch ex As Exception
                Throw ex
            Finally
                If Not cmdCuentasConf Is Nothing Then
                    If Not cmdCuentasConf.Connection Is Nothing Then
                        If cmdCuentasConf.Connection.State <> ConnectionState.Closed Then
                            cmdCuentasConf.Connection.Close()
                        End If
                    End If
                End If
            End Try
        End Sub

        Public Sub ActualizarTipoCalculo(ByVal p_intIDAsiento As Integer, ByVal p_intValor As Integer)
            Dim cmdCuentasConf As SqlClient.SqlCommand =  Nothing

            Try

                cmdCuentasConf = CrearCommandoActualizarTipoCalculo(p_intIDAsiento, p_intValor)

                cmdCuentasConf.ExecuteNonQuery()

            Catch ex As Exception
                Throw ex
            Finally
                If Not cmdCuentasConf Is Nothing Then
                    If Not cmdCuentasConf.Connection Is Nothing Then
                        If cmdCuentasConf.Connection.State <> ConnectionState.Closed Then
                            cmdCuentasConf.Connection.Close()
                        End If
                    End If
                End If
            End Try
        End Sub

        Public Function GetOrdenCompraRep(ByVal p_strNoOrden As String) As Boolean
            Dim cmdOrden As SqlClient.SqlCommand
            Dim blnComraRep As Boolean

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdOrden = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrdenCompraRep, m_cnnCostos)

            cmdOrden.CommandType = CommandType.StoredProcedure
            cmdOrden.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

            blnComraRep = CType(cmdOrden.ExecuteScalar, Boolean)

            m_cnnCostos.Close()

            Return blnComraRep

        End Function

        Public Function GetOrdenIsAsegurada(ByVal p_strNoOrden As String) As Boolean
            Dim cmdOrden As SqlClient.SqlCommand
            Dim blnComraRep As Boolean

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdOrden = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELOrdenIsAsegurada, m_cnnCostos)

            cmdOrden.CommandType = CommandType.StoredProcedure
            cmdOrden.Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoOrden, SqlDbType.VarChar, 50)).Value = p_strNoOrden

            blnComraRep = CType(cmdOrden.ExecuteScalar, Boolean)

            m_cnnCostos.Close()

            Return blnComraRep

        End Function

#End Region

#Region "Comandos"

        Private Sub CrearCommandoMovTipoCalculo(ByRef p_cmdMovTipoCal As SqlClient.SqlCommand)

            With p_cmdMovTipoCal
                .CommandText = mc_strSCGTA_SP_SELMovTipoCalculo
            End With

        End Sub

        Private Function CrearCommandoActualizarTipoCalculo(ByVal p_intIDAsiento As Integer, ByVal p_intValor As Integer) As SqlClient.SqlCommand
            Dim cmdTipoCalculo As SqlClient.SqlCommand

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdTipoCalculo = New SqlClient.SqlCommand(mc_strSCGTA_SP_UPDCostoTipoCalculo, m_cnnCostos)

            With cmdTipoCalculo
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strIDAsiento, SqlDbType.Int, 4)).Value = p_intIDAsiento
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strPorPanel, SqlDbType.Int, 4)).Value = p_intValor
            End With

            Return cmdTipoCalculo

        End Function

        Private Function CrearCommandoSearchCuentasConf(ByVal p_intIDAsiento As Integer) As SqlClient.SqlCommand
            Dim cmdSearchCuentaConf As SqlClient.SqlCommand

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdSearchCuentaConf = New SqlClient.SqlCommand(mc_strSCGTA_SP_SELCuentasConfig, m_cnnCostos)

            With cmdSearchCuentaConf
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strIDAsiento, SqlDbType.Int, 4)).Value = p_intIDAsiento
            End With

            Return cmdSearchCuentaConf

        End Function

        Private Function CrearCommandoInsModCuentasConf(ByVal p_strStoreProc As String) As SqlClient.SqlCommand
            Dim cmdInsModCuentaConf As SqlClient.SqlCommand

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdInsModCuentaConf = New SqlClient.SqlCommand(p_strStoreProc, m_cnnCostos)

            With cmdInsModCuentaConf
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strIDAsiento, SqlDbType.Int, 4, mc_strIDAsiento))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strIDCentroCosto, SqlDbType.Int, 4, mc_strIDCentroCosto))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strIDConcepto, SqlDbType.Int, 4, mc_strIDConcepto))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoCuentaDebe, SqlDbType.VarChar, 50, mc_strNoCuentaDebe))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strNoCuentaHaber, SqlDbType.VarChar, 50, mc_strNoCuentaHaber))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strTasa, SqlDbType.Decimal, 9, mc_strTasa))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strTasa2, SqlDbType.Decimal, 9, mc_strTasa2))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strTasa3, SqlDbType.Decimal, 9, mc_strTasa3))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strTasa4, SqlDbType.Decimal, 9, mc_strTasa4))
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strIndirecto, SqlDbType.Bit, 1, mc_strIndirecto))
            End With

            Return cmdInsModCuentaConf

        End Function

        Private Function CrearCommandoDeleteCuentasConf(ByVal p_strStoreProc As String) As SqlClient.SqlCommand
            Dim cmdDeleteCuentaConf As SqlClient.SqlCommand

            m_cnnCostos = m_DAConexion.ObtieneConexion

            cmdDeleteCuentaConf = New SqlClient.SqlCommand(p_strStoreProc, m_cnnCostos)

            With cmdDeleteCuentaConf
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlClient.SqlParameter(mc_strArroba & mc_strID, SqlDbType.Int, 4, mc_strID))
            End With

            Return cmdDeleteCuentaConf

        End Function

#End Region

#End Region

    End Class

End Namespace
