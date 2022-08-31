Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports System.Collections.Generic

Public Class GoodReceiveCls

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private m_blnLineasAgregadas As Boolean = False
    Private m_blnPrimeraCarga As Boolean = False
    Private m_blnRecargarLineas As Boolean
    Private m_strUnidad As String
    Private m_strVIN As String
    Private m_decTotalLocal As Decimal
    Private m_decTotalSistema As Decimal
    Private m_decTipoCambio As Decimal
    Private m_objBLSBO As New BLSBO.GlobalFunctionsSBO
    Private m_strFecha As String
    Private m_strFechaFinDia As String
    Private m_dtFecha As Date
    Private m_intDocEntry As Integer
    Private m_strMonedaSistema As String
    Private m_strMonedaLocal As String



    Public mc_strSCG_GOODRECEIVE As String = "@SCGD_GOODRECEIVE"
    Private Const mc_strMTZDetalles As String = "mtx_0"

    Private Const mc_strMontoCero As String = "0"

    Private Const mc_strUIDCargar As String = "btnCargar"

    Private Const mc_strFolder1 As String = "Costos"
    Private Const mc_strFolder2 As String = "Detalle"

    Private m_dbContratos As SAPbouiCOM.DBDataSource

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private m_intFilaMatrix As Integer = 1

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private WithEvents SBO_Application As SAPbouiCOM.Application

    Private blnUtilizaCosteoAccesorios As String = String.Empty

    Private strTipoDocumentoServicio As String = "S"

    Private strTipoDocumentoArticulo As String = "I"


    'Consulta para sacar facturas de clientes
    'Private Const mc_strConsultaPrimeraParteFC As String = "Declare @SysCurrncy as nvarchar(10) " & _
    '                                                       "Select @SysCurrncy = SysCurrncy from OADM " & _
    '                                                       "Declare @MainCurrncy as nvarchar(10) " & _
    '                                                       "Select @MainCurrncy = MainCurncy from OADM " & _
    '                                                       "Select OJDT.TransID, U_Cod_Tran + ' ' + Isnull(INV1.Dscription, '') Memo, " & _
    '                                                       "Case OINV.DocRate when 0 then Sysrate else Isnull(OINV.DocRate,Sysrate) end Rate, " & _
    '                                                       "(((LineTotal ) / (Case OINV.DocRate when 0 then Sysrate else Isnull(OINV.DocRate,Sysrate) end)) * (SysRate)*-1) Local, " & _
    '                                                       "(((LineTotal ) / (Case OINV.DocRate when 0 then Sysrate else Isnull(OINV.DocRate,Sysrate) end))*-1) Systema, " & _
    '                                                       "null as FP, OINV.DocEntry as FC, OINV.DocCur as 'Moneda Registro', AcctCode, U_Cod_Tran from INV1 " & _
    '                                                       "inner join OINV on INV1.DocEntry = OINV.DocEntry and TrgetEntry is null " & _
    '                                                       "inner join OJDT on OINV.TransID = OJDT.TransId where OINV.U_Cod_Unidad = '"

    Private Const mc_strConsultaPrimeraParteFC As String = "Declare @SysCurrncy as nvarchar(10) " & _
                                                       "Select @SysCurrncy = SysCurrncy from OADM " & _
                                                       "Declare @MainCurrncy as nvarchar(10) " & _
                                                       "Select @MainCurrncy = MainCurncy from OADM " & _
                                                       "Select OJDT.TransID, U_SCGD_Cod_Tran + ' ' + Isnull(INV1.Dscription, '') Memo, " & _
                                                        "Case OINV.DocRate when 0 then Sysrate else Isnull(OINV.DocRate,Sysrate) end Rate, " & _
                                                        "(INV1.LineTotal*-1) " & _
                                                        "Local, (INV1.TotalSumSy*-1) Systema, " & _
                                                       "null as FP, OINV.DocEntry as FC, OINV.DocCur as 'Moneda Registro', AcctCode, U_SCGD_Cod_Tran from INV1 " & _
                                                       "inner join OINV on INV1.DocEntry = OINV.DocEntry and TrgetEntry is null " & _
                                                       "inner join OJDT on OINV.TransID = OJDT.TransId where OINV.U_SCGD_Cod_Unidad = '"

    Private Const mc_strConsultaSegundaParteFC As String = "' and OINV.DocType = 'S' and OJDT.TransID not in " & _
                                                           "(Select U_NoAsient from dbo.[@SCGD_GRLINES] L inner join dbo.[@SCGD_GOODRECEIVE] GR " & _
                                                           "on GR.DocEntry = L.DocEntry and GR.U_unidad = '"
    Private Const mc_strConsultaTerceraParteFC As String = "' and U_NoAsient is not null)  and OINV.DocDate <= cast ('"
    Private Const mc_strConsultaCuartaParteFC As String = "' as datetime)"

    'Consulta para sacar salidas de mercancia
    'Private Const mc_strConsultaPrimeraParteSM As String = "Declare @SysCurrncy as nvarchar(10) Select @SysCurrncy = SysCurrncy from OADM  Declare @MainCurrncy as nvarchar(10) " & _
    '                                                       "Select @MainCurrncy = MainCurncy from OADM Select OJDT.TransID, OIGE.U_Numero_OT Memo, SysRate Rate, " & _
    '                                                       "SUM(((StockPrice*Quantity)/ (Case OIGE.DocRate when 0 then SysRate else Isnull(OIGE.DocRate,SysRate) end) " & _
    '                                                       "* SysRate)) Local, SUM((StockPrice*Quantity) / (Case OIGE.DocRate when 0 then SysRate else Isnull(OIGE.DocRate,SysRate) end)) Systema, " & _
    '                                                       "null as FP, null as FC, OIGE.DocCur as 'Moneda Registro', IGE1.AcctCode AcctCode, null U_Cod_Tran from  OIGE " & _
    '                                                       "inner join IGE1 on IGE1.DocEntry = OIGE.DocEntry inner join OJDT on OIGE.TransID = OJDT.TransId " & _
    '                                                       "where U_Procesad = 1 and (OIGE.U_Cod_Unidad = '"
    Private Const mc_strConsultaPrimeraParteSM As String = "Declare @SysCurrncy as nvarchar(10) Select @SysCurrncy = SysCurrncy from OADM  Declare @MainCurrncy as nvarchar(10) " & _
                                                   "Select @MainCurrncy = MainCurncy from OADM Select OJDT.TransID, OIGE.U_SCGD_Numero_OT Memo, SysRate Rate, " & _
                                                   "sum(IGE1.stockprice*quantity) Local, " & _
                                                   "sum((IGE1.stockprice*quantity)/sysrate) Systema, " & _
                                                   "null as FP, null as FC, OIGE.DocCur as 'Moneda Registro', IGE1.AcctCode AcctCode, null U_SCGD_Cod_Tran from  OIGE " & _
                                                   "inner join IGE1 on IGE1.DocEntry = OIGE.DocEntry inner join OJDT on OIGE.TransID = OJDT.TransId " & _
                                                   "where U_SCGD_Procesad = 1 and (OIGE.U_SCGD_Cod_Unidad = '"
    Private Const mc_strConsultaSegundaParteSM As String = "' and U_SCGD_Num_Vehiculo is not null) and OJDT.REfDate <= cast ('"
    Private Const mc_strConsultaTerceraParteSM As String = "' as datetime) and OJDT.TransID not in (Select U_NoAsient from dbo.[@SCGD_GRLINES] L " & _
                                                           "inner join dbo.[@SCGD_GOODRECEIVE] GR on GR.DocEntry = L.DocEntry and GR.U_unidad = '"
    Private Const mc_strConsultaCuartaParteSM As String = "' and U_NoAsient is not null) group By OJDT.TransID,OIGE.DocEntry,OIGE.DocCur,LocTotal,OIGE.VatSum," & _
                                                           "OIGE.DocNum, DocTotal,OIGE.U_SCGD_Numero_OT,SysRate, IGE1.AcctCode"

    'Consultas para facturas de proveedores
    Private Const mc_strConsultaPrimeraParteFP As String = "Declare @SysCurrncy as nvarchar(10) Select @SysCurrncy = SysCurrncy from OADM " & _
                                                           "Declare @MainCurrncy as nvarchar(10) Select @MainCurrncy = MainCurncy from OADM " & _
                                                           "Select OJDT.TransID, U_SCGD_Cod_Tran + ' ' + Isnull(PCH1.Dscription, '') Memo, " & _
                                                           "Sysrate  Rate, LineTotal Local, ((LineTotal ) / Sysrate ) Systema, " & _
                                                           "OPCH.DocEntry as FP, null as FC, OPCH.DocCur as 'Moneda Registro', AcctCode, U_SCGD_Cod_Tran " & _
                                                           "from PCH1 inner join OPCH on PCH1.DocEntry = OPCH.DocEntry and TrgetEntry is null " & _
                                                           "inner join OJDT on OPCH.TransID = OJDT.TransId where U_SCGD_Cod_Tran in ('"
    Private Const mc_strConsultaPrimeraParteFP2 As String = "') and U_SCGD_Cod_Unid = '"
    Private Const mc_strConsultaSegundaParteFP As String = "' and OPCH.DocType = 'S' and OJDT.TransID not in (Select U_NoAsient from dbo.[@SCGD_GRLINES] L inner join dbo.[@SCGD_GOODRECEIVE] GR " & _
                                                            "on GR.DocEntry = L.DocEntry and GR.U_unidad = '"
    Private Const mc_strConsultaTerceraParteFP As String = "' and U_NoAsient is not null) and OPCH.DocDate <= cast ('"
    Private Const mc_strConsultaCuartaParteFP As String = "' as datetime)"

    'Consultas para notas de crédito
    Private Const mc_strConsultaPrimeraParteNC As String = "Declare @SysCurrncy as nvarchar(10) " & _
                                                            "Select @SysCurrncy = SysCurrncy from OADM " & _
                                                            "Declare @MainCurrncy as nvarchar(10)  " & _
                                                            "Select @MainCurrncy = MainCurncy from OADM " & _
                                                            "Select OJDT.TransID, U_SCGD_Cod_Tran + ' ' + Isnull(RIN1.Dscription, '') Memo, " & _
                                                            "Case ORIN.DocRate when 0 then Sysrate else Isnull(ORIN.DocRate,Sysrate) end Rate, " & _
                                                            "RIN1.LineTotal " & _
                                                            "Local, RIN1.TotalSumSy Systema, " & _
                                                            "null as FP, null as FC, ORIN.DocCur as 'Moneda Registro', AcctCode, null U_SCGD_Cod_Tran " & _
                                                            "from RIN1 inner join ORIN on RIN1.DocEntry = ORIN.DocEntry and TrgetEntry is null " & _
                                                            "inner join OJDT on ORIN.TransID = OJDT.TransId where ORIN.U_SCGD_Cod_Unidad = '"
    Private Const mc_strConsultaSegundaParteNC As String = "' and ORIN.DocType = 'S' and OJDT.TransID not in (Select U_NoAsient from dbo.[@SCGD_GRLINES] L inner join dbo.[@SCGD_GOODRECEIVE] GR on GR.DocEntry = L.DocEntry and GR.U_unidad = '"
    Private Const mc_strConsultaTerceraParteNC As String = "' and U_NoAsient is not null) and ORIN.DocDate <= cast ('"
    Private Const mc_strConsultaCuartaParteNC As String = "' as datetime)"


    'Consultas para tomar en cuenta las líneas de los asientos
    Private Const mc_strConsultaPrimeraParteLAC As String = "Declare @SysCurrncy as nvarchar(10) " & _
                                                            "Select @SysCurrncy = SysCurrncy from OADM " & _
                                                            "Declare @MainCurrncy as nvarchar(10) " & _
                                                            "Select @MainCurrncy = MainCurncy from OADM " & _
                                                            "Select OJDT.TransID, U_SCGD_Cod_Tran Memo, " & _
                                                            "Case OJDT.TransRate when 0 then ORTT.Rate else Isnull(OJDT.TransRate,ORTT.Rate) end Rate, " & _
                                                            "Case debit when 0 then " & _
                                                            "((Case SYSDeb when 0 then FCdebit else SYSDeb end) / " & _
                                                            "(Select rate from ORTT where RateDate = '"
    Private Const mc_strConsultaSegundaParteLAC As String = "' and Currency = @SysCurrncy) ) else debit end Local, " & _
                                                            "Case SYSDeb when 0 then FCdebit else SYSDeb end  Systema, " & _
                                                            "null as FP, null as FC, Case (debit + FCdebit ) when Cast(0 as decimal) then @SysCurrncy else ISNULL(JDT1.FCCurrency,'"
    Private Const mc_strConsultaSegundaParteLAC2 As String = "') end as 'Moneda Registro', Account AcctCode , U_SCGD_Cod_Tran " & _
                                                            "from JDT1 inner join OJDT on JDT1.TransID = OJDT.TransId " & _
                                                            "Left outer join ORTT on OJDT.RefDate = RateDate and (ORTT.Currency = JDT1.FCCurrency or ((JDT1.FCCurrency is null or JDT1.FCCurrency = @MainCurrncy) and ORTT.Currency = @SysCurrncy)) " & _
                                                            "where U_SCGD_Cod_Tran in ('"
    Private Const mc_strConsultaTerceraParteLAC As String = "') and U_SCGD_Cod_Unidad = '"
    Private Const mc_strConsultaCuartaParteLAC As String = "' and (SYSDeb <> 0 or FCdebit <> 0 or  debit <> 0) and OJDT.TransID not in (Select U_NoAsient from dbo.[@SCGD_GRLINES]  L inner join dbo.[@SCGD_GOODRECEIVE] GR on GR.DocEntry = L.DocEntry and GR.U_unidad = '"
    Private Const mc_strConsultaCuartaParteLAC2 As String = "' and U_NoAsient is not null) and OJDT.RefDate <= cast ('"

    Private Const mc_strConsultaQuintaParteLAC1 As String = "' as datetime) union Select OJDT.TransID, U_SCGD_Cod_Tran Memo, " & _
                                                            "Case OJDT.TransRate when 0 then ORTT.Rate else Isnull(OJDT.TransRate,ORTT.Rate) end Rate, " & _
                                                            "(Case credit when 0 then ((Case SYSCred when 0 then FCcredit else SYSCred end) / " & _
                                                            "(Select rate from ORTT where RateDate = '"
    Private Const mc_strConsultaQuintaParteLAC As String = "' and Currency = @SysCurrncy) ) else credit end) * -1 Local, " & _
                                                            "(Case SYSCred when 0 then FCcredit else SYSCred end) * -1  Systema, " & _
                                                            "null as FP, null as FC, Case (credit + FCcredit ) when 0  then @SysCurrncy else ISNULL(JDT1.FCCurrency,@MainCurrncy) end  as 'Moneda Registro', Account AcctCode , U_SCGD_Cod_Tran from JDT1 " & _
                                                            "inner join OJDT on JDT1.TransID = OJDT.TransId Left outer join ORTT on OJDT.RefDate = RateDate " & _
                                                            "and (ORTT.Currency = JDT1.FCCurrency or ((JDT1.FCCurrency is null or JDT1.FCCurrency = @MainCurrncy) and ORTT.Currency = @SysCurrncy)) " & _
                                                            "where U_SCGD_Cod_Tran in ('"
    Private Const mc_strConsultaSextaParteLAC As String = "') and U_SCGD_Cod_Unidad = '"
    Private Const mc_strConsultaSetimaParteLAC As String = "' and (SYSCred <> 0 or FCcredit <> 0 or  credit <> 0) and OJDT.TransID not in (Select U_NoAsient from dbo.[@SCGD_GRLINES]  L inner join dbo.[@SCGD_GOODRECEIVE] GR on GR.DocEntry = L.DocEntry and GR.U_unidad = '"
    Private Const mc_strConsultaOctavaParteLAC As String = "' and U_NoAsient is not null) and OJDT.RefDate <= cast ('"
    Private Const mc_strConsultaNovenaParteLAC As String = "' as datetime)"

    Private m_cn_Coneccion As New SqlClient.SqlConnection
    Private m_strConectionString As String
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    Private ListaCodigoTransaccion As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaMontoMoneda As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaCantidadLocal As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)
    Private ListaNombreTransaccionLocal As Generic.IList(Of String) = New Generic.List(Of String)

    Private ListaCodigoTransaccionSistema As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaMontoMonedaSistema As Generic.IList(Of String) = New Generic.List(Of String)
    Private ListaCantidadSistema As Generic.IList(Of Decimal) = New Generic.List(Of Decimal)
    Private ListaNombreTransaccionSistema As Generic.IList(Of String) = New Generic.List(Of String)

    Private CIFLocal As Decimal
    Private CIFSistema As Decimal

    'costos de la unidad por entrada de mercancia
    Dim dcCosto As Decimal = 0
    Dim dcCostoS As Decimal = 0
    'MANEJO de informacion para formato de decimales 
    Dim n As New Globalization.NumberFormatInfo

    Private oDataTableDimensionesContablesDMS As SAPbouiCOM.DataTable
    Private oDataTableDimensiones As System.Data.DataTable
    Private oDataTableConfiguracionDocumentosDimensiones As SAPbouiCOM.DataTable
    Private ListaConfiguracion As Hashtable


#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

    ''************************************************************
    'Se cargan la configuracion general del Addon DMS, para generar el asiento una vez que se crea la
    'entrada del Vehiculo en la facturacion del Contrato de Venta
    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company, ByVal p_configuracionGenerales As SCGDataAccess.ConfiguracionesGeneralesAddon)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        objConfiguracionGeneral = p_configuracionGenerales

    End Sub

    ''************************************************************

#End Region

#Region "Metodos"

    Protected Friend Sub CargaFormularioGoodReceive(ByVal p_strUnidad As String,
                                                    ByVal p_strVIN As String,
                                                    ByVal p_strMarca As String,
                                                    ByVal p_strEstilo As String,
                                                    ByVal p_strModelo As String,
                                                    ByVal p_strIDVehiculo As String,
                                                    ByVal p_strIDEntrada As String,
                                                    ByVal p_strDocRecepcion As String,
                                                    ByVal p_strDocPedido As String)

        Try

            Dim oMatriz As SAPbouiCOM.Matrix

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.FormType = "SCGD_GOODENT"

            strXMLACargar = My.Resources.Resource.GOODENTForm
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            oMatriz = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZDetalles).Specific, SAPbouiCOM.Matrix)

            m_oFormGenCotizacion.PaneLevel = 1

            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            If p_strUnidad <> "" Then

                m_oFormGenCotizacion.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                p_strUnidad = p_strUnidad.Trim()
                m_strUnidad = p_strUnidad
                p_strIDVehiculo = p_strIDVehiculo.Trim()
                p_strVIN = p_strVIN.Trim
                m_strVIN = p_strVIN
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Unidad", 0, p_strUnidad)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Marca", 0, p_strMarca)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Estilo", 0, p_strEstilo)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Modelo", 0, p_strModelo)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VIN", 0, p_strVIN)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ID_Vehiculo", 0, p_strIDVehiculo)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_DocRecep", 0, p_strDocRecepcion)
                m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_DocPedido", 0, p_strDocPedido)

                Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, False)
                m_oFormGenCotizacion.Items.Item("15").Enabled = True
                m_oFormGenCotizacion.Items.Item("15").Specific.String = "_"
                m_oFormGenCotizacion.Items.Item("15").Click()
                m_blnPrimeraCarga = True
                m_oFormGenCotizacion.Items.Item(mc_strUIDCargar).Click()

            ElseIf Not String.IsNullOrEmpty(p_strIDEntrada) Then

                Call CargarEntrada(p_strIDEntrada)
                Utilitarios.FormularioSoloLectura(m_oFormGenCotizacion, False)

            Else

                m_oFormGenCotizacion.Items.Item("btnCargar").Visible = False

            End If

            oMatriz.Columns.Item("col_0").Editable = False
            oMatriz.Columns.Item("col_1").Editable = False
            oMatriz.Columns.Item("col_2").Editable = False
            oMatriz.Columns.Item("col_3").Editable = False
            oMatriz.Columns.Item("col_4").Editable = False
            oMatriz.Columns.Item("col_5").Editable = False
            oMatriz.Columns.Item("col_6").Editable = False
            oMatriz.Columns.Item("col_7").Editable = False
            m_oFormGenCotizacion.Items.Item(mc_strFolder1).AffectsFormMode = False
            m_oFormGenCotizacion.Items.Item(mc_strFolder2).AffectsFormMode = False

            Call m_oFormGenCotizacion.EnableMenu("1282", False)
            Call m_oFormGenCotizacion.EnableMenu("1284", False)
            Call m_oFormGenCotizacion.EnableMenu("1286", False)

            Call CargarTipoCambio()

            m_oFormGenCotizacion.Items.Item("lblMon_L").Specific.Caption = m_oFormGenCotizacion.Items.Item("lblMon_L").Specific.Caption & " " & m_strMonedaLocal
            m_oFormGenCotizacion.Items.Item("lblTot_L").Specific.Caption = m_oFormGenCotizacion.Items.Item("lblTot_L").Specific.Caption & " " & m_strMonedaLocal
            m_oFormGenCotizacion.Items.Item("lblMon_S").Specific.Caption = m_oFormGenCotizacion.Items.Item("lblMon_S").Specific.Caption & " " & m_strMonedaSistema
            m_oFormGenCotizacion.Items.Item("lblTot_S").Specific.Caption = m_oFormGenCotizacion.Items.Item("lblTot_S").Specific.Caption & " " & m_strMonedaSistema

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub CargarEntrada(ByVal p_strItem As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim oitem As SAPbouiCOM.Item
        Dim oedit As SAPbouiCOM.EditText

        Dim strIdVehiculo As String
        If m_oFormGenCotizacion IsNot Nothing Then
            oitem = m_oFormGenCotizacion.Items.Item("5")
            oedit = CType(oitem.Specific, SAPbouiCOM.EditText)

            strIdVehiculo = p_strItem

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.Alias = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strItem

            oedit = oitem.Specific
            oedit.String = strIdVehiculo
            m_oFormGenCotizacion.Items.Item(mc_strUIDCargar).Visible = False
            Call m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Query(oConditions)
            Call m_oFormGenCotizacion.DataSources.DBDataSources.Item("@SCGD_GRLINES").Query(oConditions)
            m_oFormGenCotizacion.Items.Item(mc_strMTZDetalles).Specific.LoadFromDataSource()
            m_oFormGenCotizacion.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

        End If

    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosTab(ByRef oTmpForm As SAPbouiCOM.Form, _
                                ByRef pval As SAPbouiCOM.ItemEvent)

        If pval.ItemUID = mc_strFolder1 Then

            oTmpForm.PaneLevel = 1
        ElseIf pval.ItemUID = mc_strFolder2 Then

            oTmpForm.PaneLevel = 2

        End If

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try
            Dim decSaldoInicialLocal As Decimal
            Dim decSaldoInicialSistema As Decimal

            Dim decTotalesMonedaLocal As Decimal
            Dim decTotalesMonedaSistema As Decimal

            Dim dtsNotasCreditoProveedor As New RecosteoDataSet
            Dim dtaNotasCreditoProveedor As New RecosteoDataSetTableAdapters.NotaCreditoProveedorDataAdapter

            Dim dtsDocumentosFormularios As New RecosteoDataSet
            Dim dtaDocumentosFormularios As New RecosteoDataSetTableAdapters.FormulariosTableAdapter

            Dim dtsDocumentosFacturaClientes As New RecosteoDataSet
            Dim dtaDocumentosFacturaClientes As New RecosteoDataSetTableAdapters.FacturaClientesTableAdapter

            Dim dtsSaldosIniciales As New RecosteoDataSet
            Dim dtaSaldosIniciales As New RecosteoDataSetTableAdapters.SaldosInicialesTableAdapter

            Dim dtsAsientos As New RecosteoDataSet
            Dim dtaAsientos As New RecosteoDataSetTableAdapters.AsientosTableAdapter

            Dim dtsAsientosSalidasInventario As New RecosteoDataSet
            Dim dtaAsientosSalidasInventario As New RecosteoDataSetTableAdapters.AsientoSalidaInventarioTableAdapter

            Dim drwNotaCreditoProveedor As RecosteoDataSet.NotaCreditoProveedorRow

            Dim drwAsientosSalidas As RecosteoDataSet.AsientoSalidaInventarioRow

            Dim drwDocumentosF As RecosteoDataSet.FormulariosRow

            Dim drwAsientos As RecosteoDataSet.AsientosRow

            Dim drwSaldoInicial As RecosteoDataSet.SaldosInicialesRow

            Dim drwDocumentosFacturaClientes As RecosteoDataSet.FacturaClientesRow

            Dim strConectionString As String = String.Empty
            Dim cnConeccionBD As SqlClient.SqlConnection

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, _
                                                 m_oCompany.CompanyDB, _
                                                 strConectionString)


            cnConeccionBD = New SqlClient.SqlConnection
            cnConeccionBD.ConnectionString = strConectionString

            dtaNotasCreditoProveedor.Connection = cnConeccionBD

            dtaDocumentosFormularios.Connection = cnConeccionBD
            dtaAsientos.Connection = cnConeccionBD
            dtaSaldosIniciales.Connection = cnConeccionBD
            dtaAsientosSalidasInventario.Connection = cnConeccionBD
            dtaDocumentosFacturaClientes.Connection = cnConeccionBD


            cnConeccionBD.Open()

            '***************************************************************
            blnUtilizaCosteoAccesorios = Utilitarios.EjecutarConsulta("Select U_UsaAxC from dbo.[@SCGD_ADMIN]", m_oCompany.CompanyDB, m_oCompany.Server)

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing Then
                If (pVal.ItemUID = "1") AndAlso pVal.BeforeAction AndAlso pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                    Dim strCuentaTransito As String = String.Empty
                    Dim strCuentaInventario As String = String.Empty
                    Dim strTipoVehiculo As String = String.Empty
                    Dim strInvFacturado As String = String.Empty

                    m_strUnidad = m_oFormGenCotizacion.DataSources.DBDataSources.Item("@SCGD_GOODRECEIVE").GetValue("U_Unidad", 0).ToString().Trim()
                    strInvFacturado = objConfiguracionGeneral.InventarioVehiculoVendido

                    strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server)

                    If strTipoVehiculo = strInvFacturado Then
                        strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo_Ven FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server)
                    End If

                    strCuentaTransito = objConfiguracionGeneral.CuentaInventarioTransito(strTipoVehiculo)
                    strCuentaInventario = objConfiguracionGeneral.CuentaStock(strTipoVehiculo)

                    'asigno valor al n
                    n = DIHelper.GetNumberFormatInfo(m_oCompany)

                    If Not String.IsNullOrEmpty(m_strUnidad) Then

                        'obtenermos los costos de la unidad
                        dcCosto = Decimal.Parse(m_oFormGenCotizacion.DataSources.DBDataSources.Item("@SCGD_GOODRECEIVE").GetValue("U_GASTRA", 0).ToString(), n)
                        dcCostoS = Decimal.Parse(m_oFormGenCotizacion.DataSources.DBDataSources.Item("@SCGD_GOODRECEIVE").GetValue("U_GASTRA_S", 0).ToString(), n)

                    End If

                    If String.IsNullOrEmpty(strCuentaTransito) Or String.IsNullOrEmpty(strCuentaInventario) Then

                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCuentasTransitoInventario, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False

                    End If

                End If

                'manejo de action succes para boton crear entrada de mercancia 
                If pVal.ItemUID = "1" _
                    AndAlso pVal.ActionSuccess _
                    AndAlso pVal.FormMode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                    If Not String.IsNullOrEmpty(m_strUnidad) _
                        And Not String.IsNullOrEmpty(dcCosto) _
                        And Not String.IsNullOrEmpty(dcCostoS) Then

                        Dim objGoodI As New GoodIssueCls(SBO_Application, m_oCompany)

                        Dim strCode = Utilitarios.EjecutarConsulta(String.Format("SELECT Code FROM [@SCGD_VEHICULO] WHERE U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server)

                        If Not String.IsNullOrEmpty(strCode) Then

                            objGoodI.ActualizaCostoVehiculo(strCode, dcCostoS, dcCosto, True)

                        End If

                    End If


                End If

            End If

            If Not oForm Is Nothing _
                AndAlso pVal.ActionSuccess _
                AndAlso pVal.ItemUID = mc_strUIDCargar Then

                If Not m_blnPrimeraCarga Then
                    m_blnRecargarLineas = True
                Else
                    m_blnLineasAgregadas = False

                End If

                m_decTotalLocal = 0
                m_decTotalSistema = 0

                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCargandoTipoCambio, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                Call CargarTipoCambio()
                If m_decTipoCambio = -1 Then
                    Dim obItem As Item = DirectCast(oForm.Items.Item("1"), Item)
                    obItem.Enabled = False
                    Dim obItemGenerar As Item = DirectCast(oForm.Items.Item("btn_Genera"), Item)
                    obItemGenerar.Enabled = False
                    Exit Sub
                Else
                    Dim obItem As Item = DirectCast(oForm.Items.Item("1"), Item)
                    obItem.Enabled = True
                    Dim obItemGenerar As Item = DirectCast(oForm.Items.Item("btn_Genera"), Item)
                    obItemGenerar.Enabled = True
                End If
                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajecargandoCostos, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                If Not m_blnPrimeraCarga Then
                    Call LimpiarLineasCostos(oForm)
                Else
                    m_blnPrimeraCarga = False
                End If

                LimpiarListas()

                CIFLocal = 0
                CIFSistema = 0

                Dim strSeparadorDecimalesSAP As String = String.Empty
                Dim strSeparadorMilesSAP As String = String.Empty

                Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

                Dim strSysCurrency As String = Utilitarios.EjecutarConsulta("Select SysCurrncy from OADM with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
                Dim strMainCurrency As String = Utilitarios.EjecutarConsulta("Select MainCurncy from OADM with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)

                'Cambio Costeo Local
                Dim blnCosteoLocal As String = String.Empty
                blnCosteoLocal = Utilitarios.EjecutarConsulta("Select U_CosteoLocal from dbo.[@SCGD_ADMIN] with(nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
                If String.IsNullOrEmpty(blnCosteoLocal) Then
                    blnCosteoLocal = "N"
                End If

                'lleno el dataset con los formularios y sus transacciones

                If blnUtilizaCosteoAccesorios = "Y" Then
                    dtaDocumentosFormularios.SetTimeOut(240)
                    dtaDocumentosFormularios.FillFormularios(dtsDocumentosFormularios.Formularios, m_strUnidad, strTipoDocumentoServicio, strTipoDocumentoArticulo, m_dtFecha)

                    'Agregado Erick Sanabria 28.09.2012 Llenar DataSet Notas de Crédito Proveedores
                    dtaNotasCreditoProveedor.SetTimeOut(240)
                    dtaNotasCreditoProveedor.FillNotasCreditoProveedor(dtsNotasCreditoProveedor.NotaCreditoProveedor, m_strUnidad, strTipoDocumentoServicio, strTipoDocumentoArticulo, m_dtFecha)
                    'Agregado Erick Sanabria 28.09.2012 Llenar DataSet Notas de Crédito Proveedores

                Else
                    dtaDocumentosFormularios.SetTimeOut(240)
                    dtaDocumentosFormularios.FillFormularios(dtsDocumentosFormularios.Formularios, m_strUnidad, strTipoDocumentoServicio, Nothing, m_dtFecha)

                    'Llenar DataSet Notas de Crédito Proveedores
                    dtaNotasCreditoProveedor.SetTimeOut(240)
                    dtaNotasCreditoProveedor.FillNotasCreditoProveedor(dtsNotasCreditoProveedor.NotaCreditoProveedor, m_strUnidad, strTipoDocumentoServicio, Nothing, m_dtFecha)

                End If

                'se llena el DataSet con las facturas de clientes
                dtaDocumentosFacturaClientes.SetTimeOut(240)
                dtaDocumentosFacturaClientes.FillFacturaClientes(dtsDocumentosFacturaClientes.FacturaClientes, m_strUnidad, m_dtFecha)

                'Saldos iniciales
                dtaSaldosIniciales.SetTimeOut(240)
                dtaSaldosIniciales.FillSaldosIniciales(dtsSaldosIniciales.SaldosIniciales, m_strMonedaSistema, m_strUnidad, m_dtFecha, m_strMonedaLocal)

                dtaAsientos.SetTimeOut(240)
                dtaAsientos.FillAsientos(dtsAsientos.Asientos, strMainCurrency, strSysCurrency, m_strUnidad, m_dtFecha, m_strMonedaLocal)

                'lleno el dataset con los Asientos generados en Salidas de inventario
                dtaAsientosSalidasInventario.SetTimeOut(240)
                dtaAsientosSalidasInventario.FillAsientoSalidaInventario(dtsAsientosSalidasInventario.AsientoSalidaInventario, m_strUnidad, m_dtFecha)


                For Each drw As RecosteoDataSet.SaldosInicialesRow In dtsSaldosIniciales.SaldosIniciales
                    If drw.MonedaRegistro = m_strMonedaLocal Then
                        decSaldoInicialLocal = drw.Local
                    Else
                        decSaldoInicialSistema = drw.Systema
                    End If
                Next

                If dtsDocumentosFormularios.Formularios.Rows.Count > 0 Then

                    For Each drwDocumentosF In dtsDocumentosFormularios.Formularios.Rows

                        VerificarItem(drwDocumentosF, dtsDocumentosFormularios)

                    Next

                End If

                'factura de clientes
                If dtsDocumentosFacturaClientes.FacturaClientes.Rows.Count > 0 Then

                    For Each drwDocumentosFacturaClientes In dtsDocumentosFacturaClientes.FacturaClientes.Rows

                        VerificarItem(drwDocumentosFacturaClientes, dtsDocumentosFacturaClientes)

                    Next

                End If

                'Agregado Erick Sanabria 28.09.2012 
                If dtsNotasCreditoProveedor.NotaCreditoProveedor.Rows.Count > 0 Then
                    For Each drwNotaCreditoProveedor In dtsNotasCreditoProveedor.NotaCreditoProveedor.Rows
                        VerificarItem(drwNotaCreditoProveedor, dtsNotasCreditoProveedor)
                    Next
                End If
                'Agregado Erick Sanabria 28.09.2012 

                If dtsAsientos.Asientos.Rows.Count > 0 Then

                    For Each drwAsientos In dtsAsientos.Asientos.Rows
                        VerificarItem(drwAsientos, dtsAsientos)
                    Next

                End If

                If dtsAsientosSalidasInventario.AsientoSalidaInventario.Rows.Count > 0 Then

                    For Each drwAsientosSalidas In dtsAsientosSalidasInventario.AsientoSalidaInventario.Rows
                        VerificarItem(drwAsientosSalidas, dtsAsientosSalidasInventario, True, "TALLER")
                    Next

                End If

                decTotalesMonedaLocal = CalcularMontosTotales(ListaCantidadLocal)
                decTotalesMonedaLocal += decSaldoInicialLocal

                decTotalesMonedaSistema = CalcularMontosTotales(ListaCantidadSistema)
                decTotalesMonedaSistema += decSaldoInicialSistema

                oForm.Freeze(True)

                Limpiar_CamposResumen(oForm)

                Agregar_a_Campos2(ListaNombreTransaccionLocal, oForm, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decTotalesMonedaLocal, decTotalesMonedaSistema, ListaMontoMoneda)
                Agregar_a_Campos2(ListaNombreTransaccionSistema, oForm, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decTotalesMonedaLocal, decTotalesMonedaSistema)

                AgregarTotalesFormulario(oForm, CIFLocal, CIFSistema, strSeparadorDecimalesSAP, strSeparadorMilesSAP, decSaldoInicialLocal, decSaldoInicialSistema, decTotalesMonedaLocal, decTotalesMonedaSistema, blnCosteoLocal)

                For Each drwSaldoInicial In dtsSaldosIniciales.SaldosIniciales.Rows

                    Call AgregarLineaCosto(drwSaldoInicial.TransID, drwSaldoInicial.Memo, drwSaldoInicial.Rate, drwSaldoInicial.Local, drwSaldoInicial.Systema, "", "", drwSaldoInicial.MonedaRegistro, oForm, "")

                Next

                For Each drwDocumentosF In dtsDocumentosFormularios.Formularios.Rows

                    If drwDocumentosF.IsFCNull Then
                        Call AgregarLineaCosto(drwDocumentosF.TransId, drwDocumentosF.Memo, drwDocumentosF.Rate, drwDocumentosF.Local, drwDocumentosF.Systema, drwDocumentosF.FP, "", drwDocumentosF.MonedaRegistro, oForm, drwDocumentosF.AcctCode)
                    Else
                        Call AgregarLineaCosto(drwDocumentosF.TransId, drwDocumentosF.Memo, drwDocumentosF.Rate, drwDocumentosF.Local, drwDocumentosF.Systema, drwDocumentosF.FP, drwDocumentosF.FC, drwDocumentosF.MonedaRegistro, oForm, drwDocumentosF.AcctCode)
                    End If

                Next

                For Each drwDocumentosFacturaClientes In dtsDocumentosFacturaClientes.FacturaClientes.Rows

                    If drwDocumentosFacturaClientes.IsFPNull Then
                        Call AgregarLineaCosto(drwDocumentosFacturaClientes.TransID, drwDocumentosFacturaClientes.Memo, drwDocumentosFacturaClientes.Rate, drwDocumentosFacturaClientes.Local, drwDocumentosFacturaClientes.Systema, "", drwDocumentosFacturaClientes.FC, drwDocumentosFacturaClientes.MonedaRegistro, oForm, drwDocumentosFacturaClientes.AcctCode)
                    Else
                        Call AgregarLineaCosto(drwDocumentosFacturaClientes.TransID, drwDocumentosFacturaClientes.Memo, drwDocumentosFacturaClientes.Rate, drwDocumentosFacturaClientes.Local, drwDocumentosFacturaClientes.Systema, drwDocumentosFacturaClientes.FP, drwDocumentosFacturaClientes.FC, drwDocumentosFacturaClientes.MonedaRegistro, oForm, drwDocumentosFacturaClientes.AcctCode)
                    End If

                Next


                'Agregar Lineas de Costo Notas de Crédito
                For Each drwNotaCreditoProveedor In dtsNotasCreditoProveedor.NotaCreditoProveedor.Rows
                    If drwNotaCreditoProveedor.IsFCNull Then
                        Call AgregarLineaCosto(drwNotaCreditoProveedor.TransId, drwNotaCreditoProveedor.Memo, drwNotaCreditoProveedor.Rate, drwNotaCreditoProveedor.Local, drwNotaCreditoProveedor.Systema, "", "", drwNotaCreditoProveedor.MonedaRegistro, oForm, drwNotaCreditoProveedor.AcctCode)
                    Else
                        Call AgregarLineaCosto(drwNotaCreditoProveedor.TransId, drwNotaCreditoProveedor.Memo, drwNotaCreditoProveedor.Rate, drwNotaCreditoProveedor.Local, drwNotaCreditoProveedor.Systema, drwNotaCreditoProveedor.FP, drwNotaCreditoProveedor.FC, drwNotaCreditoProveedor.MonedaRegistro, oForm, drwNotaCreditoProveedor.AcctCode)
                    End If
                Next

                For Each drwAsientos In dtsAsientos.Asientos.Rows

                    Call AgregarLineaCosto(drwAsientos.TransId, drwAsientos.Memo, drwAsientos.Rate, drwAsientos.Local, drwAsientos.Systema, "", "", drwAsientos.MonedaRegistro, oForm, drwAsientos.AcctCode)

                Next

                For Each drwAsientosSalidas In dtsAsientosSalidasInventario.AsientoSalidaInventario.Rows

                    Call AgregarLineaCosto(drwAsientosSalidas.TransId, drwAsientosSalidas.Memo, drwAsientosSalidas.Rate, drwAsientosSalidas.Local, drwAsientosSalidas.Systema, "", "", drwAsientosSalidas.MonedaRegistro, oForm, drwAsientosSalidas.AcctCode)

                Next

                'Asignar Totales Costeo Local
                If blnCosteoLocal = "Y" Then
                    Dim decTotalLoc As Decimal = 0
                    Dim decTotalSys As Decimal = 0
                    For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("@SCGD_GRLINES").Size - 1
                        decTotalLoc += Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("@SCGD_GRLINES").GetValue("U_Mon_Loc", i), n)
                        decTotalSys += Utilitarios.ConvierteDecimal(oForm.DataSources.DBDataSources.Item("@SCGD_GRLINES").GetValue("U_Mon_Sis", i), n)
                    Next
                    oForm.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA", oForm.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, decTotalLoc.ToString(n))
                    oForm.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", oForm.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, decTotalSys.ToString(n))
                End If

                LimpiarListas()

                oForm.Freeze(False)

                cnConeccionBD.Close()

                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Private Sub Limpiar_CamposResumen(ByRef p_oform As SAPbouiCOM.Form)

        ''Genericos
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FOB", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLETE", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGFAC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMFOR", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMNEG", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCINT", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCEXT", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMAPE", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGLOC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TRASLA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_REDEST", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_BODALM", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_DESALM", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_IMPVTA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_AGENCIA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLELOC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_RESERVA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_OTROS", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TALLER", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)

        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCINT_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCEXT_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMAPE_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGLOC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TRASLA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_REDEST_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_BODALM_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_DESALM_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_IMPVTA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_AGENCI_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLELOC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_RESERV_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FOB_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLETE_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGFAC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMFOR_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMNEG_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_OTROS_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TALLER_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)

        ''Especiales
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Cambio", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_L", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VALHAC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VALHAC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Tot_Loc", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Tot_Sis", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, mc_strMontoCero)

    End Sub

    Private Sub AgregarTotalesFormulario(ByRef p_oform As SAPbouiCOM.Form, ByVal p_CIFLocal As Decimal, ByVal p_CIFSistema As Decimal, _
                                         ByVal p_strSeparadorDecimalesSAP As String, ByVal p_strSeparadorMilesSAP As String, _
                                         ByVal p_decSaldoInicialLocal As Decimal, ByVal p_decSaldoInicialSistema As Decimal, _
                                         ByVal p_decTotalLocal As Decimal, ByVal p_decTotalSistema As Decimal, ByVal p_blnCosteoLocal As String)


        If p_blnCosteoLocal <> "Y" Then
            Dim SumTotalL As Decimal = p_decTotalLocal + (p_decTotalSistema * m_decTipoCambio)
            Dim strSumTotalL As String = CStr(SumTotalL).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
            'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strSumTotalL)
            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, SumTotalL.ToString(n))

            If m_decTipoCambio <> 0 Then

                Dim SumTotalS As Decimal = p_decTotalSistema + (p_decTotalLocal / m_decTipoCambio)
                Dim strTotalS As String = CStr(SumTotalS).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
                'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strTotalS)
                p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, SumTotalS.ToString(n))
            Else

                Dim strTotalS As String = CStr(p_decTotalSistema).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
                'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strTotalS)
                p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decTotalSistema.ToString(n))
            End If
        End If



        'Dim strCIFLocal As String = CStr(p_CIFLocal).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
        'Dim strCIFSistema As String = CStr(p_CIFSistema).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
        'Dim strTipoCambio As String = CStr(m_decTipoCambio).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)

        'Dim strSaldoInicialLocal As String = CStr(p_decSaldoInicialLocal).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
        'Dim strSaldoInicialSistema As String = CStr(p_decSaldoInicialSistema).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
        'Dim strTotalLocal As String = CStr(p_decTotalLocal).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)
        'Dim strTotalSistema As String = CStr(p_decTotalSistema).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, p_strSeparadorDecimalesSAP)

        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Cambio", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strTipoCambio)
        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_L", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strCIFLocal)
        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strCIFSistema)
        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VALHAC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strSaldoInicialLocal)
        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VALHAC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strSaldoInicialSistema)
        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Tot_Loc", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strTotalLocal)
        'p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Tot_Sis", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, strTotalSistema)

        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Cambio", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, m_decTipoCambio.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_L", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_CIFLocal.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_CIFSistema.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VALHAC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decSaldoInicialLocal.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_VALHAC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decSaldoInicialSistema.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Tot_Loc", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decTotalLocal.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Tot_Sis", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decTotalSistema.ToString(n))
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SCGD_DocSalida", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, String.Empty)



    End Sub
    Private Sub LimpiarListas()
        ListaCodigoTransaccion.Clear()
        ListaMontoMoneda.Clear()
        ListaCantidadLocal.Clear()
        ListaNombreTransaccionLocal.Clear()

        ListaCodigoTransaccionSistema.Clear()
        ListaMontoMonedaSistema.Clear()
        ListaCantidadSistema.Clear()
        ListaNombreTransaccionSistema.Clear()
    End Sub

    Private Sub Agregar_a_Campos2(Of U As {Generic.IList(Of String)})(ByVal p As U, _
                                                          ByRef p_oform As SAPbouiCOM.Form, _
                                                          ByVal p_strSeparadorDecimalesSAP As String, ByVal p_strSeparadorMilesSAP As String, _
                                                          ByVal p_decSaldoInicialLocal As Decimal, ByVal p_decSaldoInicialSistema As Decimal, _
                                                          ByVal p_decTotalLocal As Decimal, ByVal p_decTotalSistema As Decimal, _
                                                         Optional ByVal lista As U = Nothing)

        Dim p_strCampoNombreTrasaccion As String = String.Empty
        Dim p_decMontoLocal As Decimal
        Dim p_decMontoSistema As Decimal
        Dim Moneda As String = String.Empty


        If Not p.Count = 0 Then

            For i As Integer = 0 To p.Count - 1

                If Not lista Is Nothing Then
                    Moneda = lista.Item(i)
                End If

                If Moneda = m_strMonedaLocal Then

                    Dim s As String = ListaNombreTransaccionLocal.Item(i)

                    p_strCampoNombreTrasaccion = s

                    p_decMontoLocal = ListaCantidadLocal.Item(i)

                    Select Case p_strCampoNombreTrasaccion

                        Case "FOB"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FOB", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "FLETE"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLETE", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "SEGFAC"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGFAC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "COMFOR"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMFOR", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "COMNEG"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMNEG", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                            CIFLocal = CIFLocal + p_decMontoLocal
                        Case "ACCINT"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCINT", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "ACCEXT"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCEXT", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "COMAPE" 'Comisión Apertura
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMAPE", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "SEGLOC" 'Seguros locales
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGLOC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "TRASLA" 'Traslado
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TRASLA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "REDEST" 'Redestino
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_REDEST", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "BODALM" 'Bodega almacen fiscal
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_BODALM", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "DESALM" 'Desalmacenaje
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_DESALM", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "IMPVTA" 'Impuesto
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_IMPVTA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "AGENCIA" 'Agencia
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_AGENCIA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "FLELOC" 'Flete Local
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLELOC", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "RESERVA"   'Reserva
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_RESERVA", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "OTROS_FP"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_OTROS", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "TALLER"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TALLER", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                        Case "CIF"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_L", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoLocal.ToString(n))
                            CIFLocal = CIFLocal + p_decMontoLocal
                    End Select

                Else

                    Dim StrNombreTransaccion As String = ListaNombreTransaccionSistema.Item(i)

                    p_strCampoNombreTrasaccion = StrNombreTransaccion
                    p_decMontoSistema = ListaCantidadSistema.Item(i)

                    Select Case p_strCampoNombreTrasaccion

                        Case "ACCINT"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCINT_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "ACCEXT"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_ACCEXT_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "COMAPE" 'Comisión Apertura
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMAPE_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "SEGLOC" 'Seguros locales
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGLOC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "TRASLA" 'Traslado
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TRASLA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "REDEST" 'Redestino
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_REDEST_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "BODALM" 'Bodega almacen fiscal
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_BODALM_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "DESALM" 'Desalmacenaje
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_DESALM_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "IMPVTA" 'Impuesto
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_IMPVTA_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "AGENCIA" 'Agencia
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_AGENCI_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "FLELOC" 'Flete Local
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLELOC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "RESERVA"   'Reserva
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_RESERV_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "FOB"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FOB_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "FLETE"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_FLETE_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "SEGFAC"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_SEGFAC_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "COMFOR"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMFOR_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "COMNEG"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_COMNEG_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                            CIFSistema = CIFSistema + p_decMontoSistema
                        Case "OTROS_FP"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_OTROS_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "TALLER"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TALLER_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                        Case "CIF"
                            p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_S", p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).Offset, p_decMontoSistema.ToString(n))
                            CIFSistema = CIFSistema + p_decMontoSistema
                    End Select
                End If
            Next

        End If
    End Sub



    Private Sub VerificarItem(Of U As {System.Data.DataRow})(ByRef p_drw As U, ByVal dtsF As RecosteoDataSet, Optional ByVal blnAsientoSalidaInventario As Boolean = False, Optional ByVal p_NombreTransaccion As String = "")

        Dim strCodigoTransaccion As String
        Dim strNombreTransaccion As String

        Dim strMoneda As String = p_drw.Item("MonedaRegistro")
        Dim decLocal As Decimal = p_drw.Item("Local")
        Dim decSistema As Decimal = p_drw.Item("Systema")


        If Not p_drw.Item("U_SCGD_Cod_Tran") Is DBNull.Value Then
            strCodigoTransaccion = p_drw.Item("U_SCGD_Cod_Tran")
        End If

        If Not p_drw.Item("NombreTransaccion") Is DBNull.Value Then
            strNombreTransaccion = p_drw.Item("NombreTransaccion")
        Else
            If blnAsientoSalidaInventario Then
                strNombreTransaccion = p_NombreTransaccion
            End If
        End If


        If strMoneda = m_strMonedaLocal Then

            If ListaNombreTransaccionLocal.Contains(strNombreTransaccion) Then

                Dim posit As Integer = ListaNombreTransaccionLocal.IndexOf(strNombreTransaccion)

                If ListaNombreTransaccionLocal.Item(posit) = strNombreTransaccion And ListaMontoMoneda.Item(posit) = strMoneda Then
                    ListaCantidadLocal.Item(posit) = ListaCantidadLocal.Item(posit) + decLocal
                End If

            Else

                ListaCodigoTransaccion.Add(strCodigoTransaccion)
                ListaMontoMoneda.Add(strMoneda)
                ListaCantidadLocal.Add(decLocal)
                ListaNombreTransaccionLocal.Add(strNombreTransaccion)

            End If

        ElseIf strMoneda = m_strMonedaSistema Then

            If ListaNombreTransaccionSistema.Contains(strNombreTransaccion) Then

                Dim posit As Integer = ListaNombreTransaccionSistema.IndexOf(strNombreTransaccion)

                If ListaNombreTransaccionSistema.Item(posit) = strNombreTransaccion And ListaMontoMonedaSistema.Item(posit) = strMoneda Then
                    ListaCantidadSistema.Item(posit) = ListaCantidadSistema.Item(posit) + decSistema
                End If
            Else
                ListaCodigoTransaccionSistema.Add(strCodigoTransaccion)
                ListaMontoMonedaSistema.Add(strMoneda)
                ListaCantidadSistema.Add(decSistema)
                ListaNombreTransaccionSistema.Add(strNombreTransaccion)

            End If
        Else
            If ListaNombreTransaccionSistema.Contains(strNombreTransaccion) Then

                Dim posit As Integer = ListaNombreTransaccionSistema.IndexOf(strNombreTransaccion)

                If ListaNombreTransaccionSistema.Item(posit) = strNombreTransaccion Then
                    ListaCantidadSistema.Item(posit) = ListaCantidadSistema.Item(posit) + decSistema
                End If
            Else
                ListaCodigoTransaccionSistema.Add(strCodigoTransaccion)
                ListaMontoMonedaSistema.Add(strMoneda)
                ListaCantidadSistema.Add(decSistema)
                ListaNombreTransaccionSistema.Add(strNombreTransaccion)

            End If

        End If

    End Sub

    Private Function CalcularMontosTotales(Of U As {Generic.IList(Of Decimal)})(ByVal p As U) As Decimal

        Dim decTotales As Decimal
        Dim decMonto As Decimal

        For i As Integer = 0 To p.Count - 1

            decMonto = p.Item(i)

            decTotales = decTotales + decMonto

        Next

        Return decTotales

    End Function

    Private Sub CalcularSaldosIniciales()

        Dim strConsulta As String

        Dim oform As SAPbouiCOM.Form

        Dim decMontoLocal As Decimal
        Dim decMontoSistema As Decimal

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        oform = SBO_Application.Forms.Item("SCGD_GOODENT")

        strConsulta = "Select -1 TransID, '" & My.Resources.Resource.DescripcionSaldoInicialSistema & "' Memo, (Select rate from ORTT where RateDate = '" & m_strFecha & "' and Currency = '" & m_strMonedaSistema & "') Rate, " & _
                      "ISNULL(U_SALINID,0)*(Select rate from ORTT where RateDate = '" & m_strFecha & "' and Currency = '" & m_strMonedaSistema & "') Local, " & _
                        "ISNULL(U_SALINID,0) Systema, null as FP, NULL as FC, '" & m_strMonedaSistema & "' as 'Moneda Registro', null AcctCode " & _
                        "from [@SCGD_VEHICULO] with(nolock) WHERE U_Cod_Unid = '" & m_strUnidad & "' and ( U_SALINID <> 0 and U_SALINID is not null) union " & _
                        "Select -1 TransID, '" & My.Resources.Resource.DescripcionSaldoInicialLocal & "' Memo, " & _
                        "(Select rate from ORTT where RateDate = '" & m_strFecha & "' and Currency = '" & m_strMonedaSistema & "') Rate, " & _
                        "ISNULL(U_SALINIC,0)  Local, ISNULL(U_SALINIC,0)/(Select rate from ORTT where RateDate = '" & m_strFecha & "' and Currency = '" & m_strMonedaSistema & "')Systema, null as FP, NULL as FC, '" & m_strMonedaLocal & "' as 'Moneda Registro', null AcctCode  " & _
                        "from [@SCGD_VEHICULO] with(nolock) WHERE U_Cod_Unid = '" & m_strUnidad & "' and ( U_SALINIC <> 0 and U_SALINIC is not null) "

        Call PartesCostos(strConsulta, "U_VALHAC", "U_VALHAC_S", decMontoLocal, decMontoSistema, oform, True)


    End Sub

    Private Sub CargarTipoCambio()

        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item("SCGD_GOODENT")

        m_strFecha = oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).GetValue("U_Fec_Cont", 0)
        m_strFechaFinDia = m_strFecha & " 23:59:59"
        If Not String.IsNullOrEmpty(m_strFecha) Then
            m_dtFecha = New Date(m_strFecha.Substring(0, 4), m_strFecha.Substring(4, 2), m_strFecha.Substring(6, 2), 0, 0, 0)
        End If
        m_objBLSBO.Set_Compania(m_oCompany)
        m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema()
        m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
        If m_strMonedaLocal <> m_strMonedaSistema Then
            If Not String.IsNullOrEmpty(m_strFecha) Then
                m_decTipoCambio = m_objBLSBO.RetornarTipoCambioMoneda(m_strMonedaSistema, m_dtFecha, m_strConectionString, False)
                If m_decTipoCambio = -1 Then
                    SBO_Application.MessageBox(My.Resources.Resource.TipoCambioNoActualizado)
                End If
            End If
        Else
            m_decTipoCambio = 1
        End If
    End Sub

    Public Sub ValidarTipoCambio(ByRef BubbleEvent As Boolean)

        Try

            Dim oform As SAPbouiCOM.Form
            Dim decTipoCambio As Decimal

            oform = SBO_Application.Forms.Item("SCGD_GOODENT")

            m_strFecha = oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).GetValue("U_Fec_Cont", 0)
            m_strFechaFinDia = m_strFecha & " 23:59:59"
            If Not String.IsNullOrEmpty(m_strFecha) Then
                m_dtFecha = New Date(m_strFecha.Substring(0, 4), m_strFecha.Substring(4, 2), m_strFecha.Substring(6, 2), 0, 0, 0)
            End If
            m_objBLSBO.Set_Compania(m_oCompany)
            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_strMonedaLocal <> m_strMonedaSistema Then
                If Not String.IsNullOrEmpty(m_strFecha) Then
                    decTipoCambio = m_objBLSBO.RetornarTipoCambioMoneda(m_strMonedaSistema, m_dtFecha, m_strConectionString, False)

                    If decTipoCambio = -1 Then
                        Throw New Exception(My.Resources.Resource.TipoCambioNoActualizado)
                    ElseIf decTipoCambio <> m_decTipoCambio Then
                        BubbleEvent = False
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeTipoCambioNoCoincide, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End If

                End If
            Else
                m_decTipoCambio = 1
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            BubbleEvent = False
            Throw ex

        End Try

    End Sub

    Private Sub CargarCostosAccesoriosInternos()

        'Comisión Apertura
        Call CostosFP("ACCINT", "U_ACCINT", "U_ACCINT_S")

    End Sub

    Private Sub CargarCostosAccesoriosExternos()

        'Comisión Apertura
        Call CostosFP("ACCEXT", "U_ACCEXT", "U_ACCEXT_S")

    End Sub

    Private Sub CargarCostosFP()

        'Comisión Apertura
        Call CostosFP("COMAPE", "U_COMAPE", "U_COMAPE_S")

        'Seguros locales
        Call CostosFP("SEGLOC", "U_SEGLOC", "U_SEGLOC_S")

        'Traslado
        Call CostosFP("TRASLA", "U_TRASLA", "U_TRASLA_S")

        'Redestino
        Call CostosFP("REDEST", "U_REDEST", "U_REDEST_S")

        'Bodega almacen fiscal
        Call CostosFP("BODALM", "U_BODALM", "U_BODALM_S")

        'Desalmacenaje
        Call CostosFP("DESALM", "U_DESALM", "U_DESALM_S")

        'Impuesto
        Call CostosFP("IMPVTA", "U_IMPVTA", "U_IMPVTA_S")

        'Agencia
        Call CostosFP("AGENCIA", "U_AGENCIA", "U_AGENCI_S")

        'Flete Local
        Call CostosFP("FLELOC", "U_FLELOC", "U_FLELOC_S")

        'Reserva
        Call CostosFP("RESERVA", "U_RESERVA", "U_RESERV_S")

    End Sub

    Private Sub CalcularCIF()

        Dim decCIFLocalAcumulado As Decimal
        Dim decCIFSistemaAcumulado As Decimal

        Dim strConsulta As String

        Dim strValorAsignar As String

        Dim strCodigoFOB As String
        Dim strCodigoFlete As String
        Dim strCodigoComisionFormalizacion As String
        Dim strCodigoComisionNegociacion As String
        Dim strCodigoSeguroFactura As String
        Dim strCodigoCIF As String

        Dim blnCIFRegistrado As Boolean = False

        Dim oform As SAPbouiCOM.Form

        Dim decMontoLocal As Decimal
        Dim decMontoSistema As Decimal

        Dim strSeparadorDecimalesSAP As String = ""
        Dim strSeparadorMilesSAP As String = ""

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        oform = SBO_Application.Forms.Item("SCGD_GOODENT")

        strCodigoFOB = Utilitarios.DevuelveTransaccionesAVisualizar("FOB")
        strCodigoFlete = Utilitarios.DevuelveTransaccionesAVisualizar("FLETE")
        strCodigoSeguroFactura = Utilitarios.DevuelveTransaccionesAVisualizar("SEGFAC")
        strCodigoComisionFormalizacion = Utilitarios.DevuelveTransaccionesAVisualizar("COMFOR")
        strCodigoComisionNegociacion = Utilitarios.DevuelveTransaccionesAVisualizar("COMNEG")
        strCodigoCIF = Utilitarios.DevuelveTransaccionesAVisualizar("CIF")

        strCodigoFOB = strCodigoFOB.Replace(",", "','")
        strCodigoFlete = strCodigoFlete.Replace(",", "','")
        strCodigoSeguroFactura = strCodigoSeguroFactura.Replace(",", "','")
        strCodigoComisionFormalizacion = strCodigoComisionFormalizacion.Replace(",", "','")
        strCodigoCIF = strCodigoCIF.Replace(",", "','")

        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoCIF & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP

        Call PartesCostos(strConsulta, "U_CIF_L", "U_CIF_S", decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoCIF & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                      mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoCIF & mc_strConsultaSextaParteLAC & m_strUnidad & _
                      mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_CIF_L", "U_CIF_S", decMontoLocal, decMontoSistema, oform, False)

        'Montos por nota de crédito
        strConsulta = mc_strConsultaPrimeraParteNC & m_strUnidad & mc_strConsultaSegundaParteNC & m_strUnidad & mc_strConsultaTerceraParteNC & m_strFechaFinDia & mc_strConsultaCuartaParteNC
        Call PartesCostos(strConsulta, "U_CIF_L", "U_CIF_S", decMontoLocal, decMontoSistema, oform, False)

        'Montos por nota de débito
        strConsulta = mc_strConsultaPrimeraParteFC & m_strUnidad & mc_strConsultaSegundaParteFC & m_strUnidad & mc_strConsultaTerceraParteFC & m_strFechaFinDia & mc_strConsultaCuartaParteFC
        Call PartesCostos(strConsulta, "U_CIF_L", "U_CIF_S", decMontoLocal, decMontoSistema, oform, True)


        blnCIFRegistrado = True
        decCIFSistemaAcumulado += decMontoSistema
        decCIFLocalAcumulado += decMontoLocal


        decMontoLocal = 0
        decMontoSistema = 0
        'FOB
        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoFOB & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        Call PartesCostos(strConsulta, "U_FOB", "U_FOB_S", decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoFOB & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoFOB & mc_strConsultaSextaParteLAC & m_strUnidad & _
                              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_FOB", "U_FOB_S", decMontoLocal, decMontoSistema, oform, True)


        blnCIFRegistrado = True
        decCIFSistemaAcumulado += decMontoSistema
        decCIFLocalAcumulado += decMontoLocal


        'Flete
        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoFlete & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        decMontoSistema = 0
        decMontoLocal = 0
        Call PartesCostos(strConsulta, "U_FLETE", "U_FLETE_S", decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoFlete & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoFlete & mc_strConsultaSextaParteLAC & m_strUnidad & _
                              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_FLETE", "U_FLETE_S", decMontoLocal, decMontoSistema, oform, True)


        blnCIFRegistrado = True
        decCIFSistemaAcumulado += decMontoSistema
        decCIFLocalAcumulado += decMontoLocal


        'Seguro Factura
        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoSeguroFactura & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        decMontoSistema = 0
        decMontoLocal = 0
        Call PartesCostos(strConsulta, "U_SEGFAC", "U_SEGFAC_S", decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoSeguroFactura & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoSeguroFactura & mc_strConsultaSextaParteLAC & m_strUnidad & _
                              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_SEGFAC", "U_SEGFAC_S", decMontoLocal, decMontoSistema, oform, True)


        blnCIFRegistrado = True
        decCIFSistemaAcumulado += decMontoSistema
        decCIFLocalAcumulado += decMontoLocal

        'Comisión Formalización
        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoComisionFormalizacion & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        decMontoSistema = 0
        decMontoLocal = 0
        Call PartesCostos(strConsulta, "U_COMFOR", "U_COMFOR_S", decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoComisionFormalizacion & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoComisionFormalizacion & mc_strConsultaSextaParteLAC & m_strUnidad & _
                              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_COMFOR", "U_COMFOR_S", decMontoLocal, decMontoSistema, oform, True)

        blnCIFRegistrado = True
        decCIFSistemaAcumulado += decMontoSistema
        decCIFLocalAcumulado += decMontoLocal

        'Comisión Negoción
        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoComisionNegociacion & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        decMontoSistema = 0
        decMontoLocal = 0
        Call PartesCostos(strConsulta, "U_COMNEG", "U_COMNEG_S", decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoComisionNegociacion & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoComisionNegociacion & mc_strConsultaSextaParteLAC & m_strUnidad & _
                              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_COMNEG", "U_COMNEG_S", decMontoLocal, decMontoSistema, oform, True)

        blnCIFRegistrado = True
        decCIFSistemaAcumulado += decMontoSistema
        decCIFLocalAcumulado += decMontoLocal

        strValorAsignar = CStr(decCIFLocalAcumulado).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_L", 0, strValorAsignar)
        strValorAsignar = CStr(decCIFSistemaAcumulado).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_CIF_S", 0, strValorAsignar)

    End Sub

    Private Sub PartesCostos(ByVal p_strConsulta As String, _
                          ByVal p_strCampoAActualizarLocal As String, _
                          ByVal p_strCampoAActualizarSistema As String, _
                          ByRef p_decAcumuladoLocal As Decimal, _
                          ByRef p_decAcumuladoSistema As Decimal, _
                          ByRef p_oform As SAPbouiCOM.Form, _
                          ByVal p_blnSumarATotal As Boolean)

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Dim strMoneda As String = ""
        Dim strConcepto As String = ""
        Dim decRate As Decimal
        Dim strTransId As String = ""
        Dim strfacturaProveedor As String = ""
        Dim strFacturaCliente As String = ""
        Dim decValorCIFLocal As Decimal
        Dim decValorCIFSistema As Decimal

        Dim strCuenta As String = ""

        Dim strValorAPornerEnCampo As String
        Dim strSeparadorDecimalesSAP As String = ""
        Dim strSeparadorMilesSAP As String = ""

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
        cn_Coneccion.ConnectionString = strConectionString
        cn_Coneccion.Open()

        cmdEjecutarConsulta.Connection = cn_Coneccion

        cmdEjecutarConsulta.CommandType = CommandType.Text
        cmdEjecutarConsulta.CommandText = p_strConsulta
        drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

        Do While drdResultadoConsulta.Read
            If Not drdResultadoConsulta.IsDBNull(3) Then
                decValorCIFLocal = drdResultadoConsulta.GetDecimal(3)
            End If
            If Not drdResultadoConsulta.IsDBNull(4) Then
                decValorCIFSistema = drdResultadoConsulta.GetDecimal(4)
            End If
            If Not drdResultadoConsulta.IsDBNull(7) Then
                strMoneda = drdResultadoConsulta.GetString(7)
            End If
            strConcepto = drdResultadoConsulta.GetString(1)
            decRate = drdResultadoConsulta.GetDecimal(2)
            strTransId = CStr(drdResultadoConsulta.GetInt32(0))
            If Not drdResultadoConsulta.IsDBNull(5) Then
                strfacturaProveedor = CStr(drdResultadoConsulta.GetInt32(5))
            End If
            If Not drdResultadoConsulta.IsDBNull(6) Then
                strFacturaCliente = CStr(drdResultadoConsulta.GetInt32(6))
            End If
            If Not drdResultadoConsulta.IsDBNull(8) Then
                strCuenta = drdResultadoConsulta.GetString(8)
            End If


            Call AgregarLineaCosto(strTransId, strConcepto, decRate, decValorCIFLocal, decValorCIFSistema, strfacturaProveedor, strFacturaCliente, strMoneda, p_oform, strCuenta)
            If m_strMonedaLocal <> strMoneda Then
                ' p_decAcumuladoLocal += decValorCIFSistema * m_decTipoCambio
                p_decAcumuladoSistema += decValorCIFSistema
            Else
                p_decAcumuladoLocal += decValorCIFLocal
                'p_decAcumuladoSistema += decValorCIFLocal / m_decTipoCambio
            End If

        Loop

        strValorAPornerEnCampo = CStr(p_decAcumuladoSistema).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue(p_strCampoAActualizarSistema, 0, strValorAPornerEnCampo)
        strValorAPornerEnCampo = CStr(p_decAcumuladoLocal).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue(p_strCampoAActualizarLocal, 0, strValorAPornerEnCampo)

        If p_blnSumarATotal Then
            m_decTotalLocal += p_decAcumuladoLocal
            m_decTotalSistema += p_decAcumuladoSistema
        End If
        strValorAPornerEnCampo = CStr(m_decTotalLocal).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TOT_LOC", 0, strValorAPornerEnCampo)
        strValorAPornerEnCampo = CStr(m_decTotalSistema).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_TOT_SIS", 0, strValorAPornerEnCampo)

        strValorAPornerEnCampo = CStr(m_decTotalLocal + (m_decTotalSistema * m_decTipoCambio)).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA", 0, strValorAPornerEnCampo)
        strValorAPornerEnCampo = CStr(m_decTotalSistema + (m_decTotalLocal / m_decTipoCambio)).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_GASTRA_S", 0, strValorAPornerEnCampo)

        strValorAPornerEnCampo = CStr(m_decTipoCambio).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
        p_oform.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).SetValue("U_Cambio", 0, strValorAPornerEnCampo)

        drdResultadoConsulta.Close()
        cmdEjecutarConsulta.Connection.Close()

    End Sub

    Private Sub CostosTaller()

        Dim strConsulta As String
        Dim strCodigoTaller As String
        Dim oform As SAPbouiCOM.Form

        Dim decMontoLocal As Decimal
        Dim decMontoSistema As Decimal

        oform = SBO_Application.Forms.Item("SCGD_GOODENT")

        'Salidas de Mercancia
        strConsulta = mc_strConsultaPrimeraParteSM & m_strUnidad & mc_strConsultaSegundaParteSM & m_strFechaFinDia & mc_strConsultaTerceraParteSM & m_strUnidad & mc_strConsultaCuartaParteSM
        Call PartesCostos(strConsulta, "U_TALLER", "U_TALLER_S", decMontoLocal, decMontoSistema, oform, False)

        'Costos en Taller Registrados por Facturas Proveedor

        strCodigoTaller = Utilitarios.DevuelveTransaccionesAVisualizar("TALLER")
        strCodigoTaller = strCodigoTaller.Replace(",", "','")

        strConsulta = mc_strConsultaPrimeraParteFP & strCodigoTaller & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        Call PartesCostos(strConsulta, "U_TALLER", "U_TALLER_S", decMontoLocal, decMontoSistema, oform, False)

        'Costos cargados por líneas de asientos contables
        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigoTaller & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigoTaller & mc_strConsultaSextaParteLAC & m_strUnidad & _
                              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC
        Call PartesCostos(strConsulta, "U_TALLER", "U_TALLER_S", decMontoLocal, decMontoSistema, oform, True)

    End Sub

    Private Sub CostosOtros()

        Dim strConsulta As String

        Dim strCodigo As String

        Dim oform As SAPbouiCOM.Form

        Dim decMontoLocal As Decimal
        Dim decMontoSistema As Decimal

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        oform = SBO_Application.Forms.Item("SCGD_GOODENT")

        'Otros costos por factura proveedor
        strCodigo = Utilitarios.DevuelveTransaccionesAVisualizar("OTROS_FP")

        strCodigo = strCodigo.Replace(",", "','")

        strConsulta = mc_strConsultaPrimeraParteFP & strCodigo & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP
        Call PartesCostos(strConsulta, "U_OTROS", "U_OTROS_S", decMontoLocal, decMontoSistema, oform, False)

        'otros costos por líneas de asientos contable
        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigo & mc_strConsultaTerceraParteLAC & m_strUnidad & _
              mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigo & mc_strConsultaSextaParteLAC & m_strUnidad & _
              mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, "U_OTROS", "U_OTROS_S", decMontoLocal, decMontoSistema, oform, True)

    End Sub

    Private Sub CostosFP(ByVal p_strCodigoTransaccion As String, _
                         ByVal p_strNombreCampoLocal As String, _
                         ByVal p_strNombreCampoSistema As String)

        Dim strConsulta As String

        Dim strCodigo As String

        Dim oform As SAPbouiCOM.Form

        Dim decMontoLocal As Decimal
        Dim decMontoSistema As Decimal

        Dim strSeparadorDecimalesSAP As String = ""
        Dim strSeparadorMilesSAP As String = ""

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        oform = SBO_Application.Forms.Item("SCGD_GOODENT")

        strCodigo = Utilitarios.DevuelveTransaccionesAVisualizar(p_strCodigoTransaccion)

        strCodigo = strCodigo.Replace(",", "','")

        strConsulta = mc_strConsultaPrimeraParteFP & strCodigo & mc_strConsultaPrimeraParteFP2 & m_strUnidad & mc_strConsultaSegundaParteFP & m_strUnidad & mc_strConsultaTerceraParteFP & m_strFechaFinDia & mc_strConsultaCuartaParteFP

        Call PartesCostos(strConsulta, p_strNombreCampoLocal, p_strNombreCampoSistema, decMontoLocal, decMontoSistema, oform, False)

        strConsulta = mc_strConsultaPrimeraParteLAC & m_strFecha & mc_strConsultaSegundaParteLAC & m_strMonedaLocal & mc_strConsultaSegundaParteLAC2 & strCodigo & mc_strConsultaTerceraParteLAC & m_strUnidad & _
                      mc_strConsultaCuartaParteLAC & m_strUnidad & mc_strConsultaCuartaParteLAC2 & m_strFechaFinDia & mc_strConsultaQuintaParteLAC1 & m_strFecha & mc_strConsultaQuintaParteLAC & strCodigo & mc_strConsultaSextaParteLAC & m_strUnidad & _
                      mc_strConsultaSetimaParteLAC & m_strUnidad & mc_strConsultaOctavaParteLAC & m_strFechaFinDia & mc_strConsultaNovenaParteLAC

        Call PartesCostos(strConsulta, p_strNombreCampoLocal, p_strNombreCampoSistema, decMontoLocal, decMontoSistema, oform, True)

    End Sub

    Private Sub AgregarLineaCosto(ByVal p_strTransID As String, _
                                  ByVal p_strMemo As String, _
                                  ByVal p_decRate As Decimal, _
                                  ByVal p_decLocal As Decimal, _
                                  ByVal p_decSistema As Decimal, _
                                  ByVal p_strFacturaProveedor As String, _
                                  ByVal p_strFacturaCliente As String, _
                                  ByVal p_strMoneda As String, _
                                  ByRef p_oform As SAPbouiCOM.Form, _
                                  ByVal p_strCuenta As String)

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intNuevoRegisto As Integer

        Dim strValorAColocar As String
        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        oMatriz = DirectCast(p_oform.Items.Item(mc_strMTZDetalles).Specific, SAPbouiCOM.Matrix)

        intNuevoRegisto = p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").Size
        If Not m_blnRecargarLineas Then
            If m_blnLineasAgregadas Then
                p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").InsertRecord(intNuevoRegisto)
            Else
                intNuevoRegisto = 0
                m_blnLineasAgregadas = True
            End If

        Else
            If m_blnRecargarLineas Then
                If intNuevoRegisto >= 1 Then
                    Call LimpiarLineasCostos(p_oform)
                End If
                intNuevoRegisto = 0
                m_blnLineasAgregadas = True
                oMatriz.LoadFromDataSource()
                p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").InsertRecord(intNuevoRegisto)
                m_blnRecargarLineas = False
                m_blnLineasAgregadas = True
            End If
        End If

        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_Concepto", intNuevoRegisto, p_strMemo)
        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_Mon_Loc", intNuevoRegisto, p_decLocal.ToString(n))
        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_Mon_Sis", intNuevoRegisto, p_decSistema.ToString(n))
        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_Mon_Reg", intNuevoRegisto, p_strMoneda)
        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_Tip_Cam", intNuevoRegisto, p_decRate.ToString(n))

        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_NoAsient", intNuevoRegisto, p_strTransID)

        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_NoFP", intNuevoRegisto, p_strFacturaProveedor)

        p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_No_FC", intNuevoRegisto, p_strFacturaCliente)

        If Not String.IsNullOrEmpty(p_strCuenta) Then
            p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").SetValue("U_Cuenta", intNuevoRegisto, p_strCuenta)
        End If

        oMatriz.LoadFromDataSource()
        If p_oform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
            p_oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Private Sub LimpiarLineasCostos(ByRef p_oform As SAPbouiCOM.Form)

        Dim intCantidadLineas As Integer
        Dim intLineaABorrar As Integer

        intCantidadLineas = p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").Size
        For intLineaABorrar = 1 To intCantidadLineas
            p_oform.DataSources.DBDataSources.Item("@SCGD_GRLINES").RemoveRecord(0)
        Next
        p_oform.Items.Item(mc_strMTZDetalles).Specific.LoadFromdataSource()
    End Sub

    Public Sub CrearAsientos(ByRef pVal As SAPbouiCOM.ItemEvent, _
                                ByRef BubbleEvent As Boolean)


        If pVal.BeforeAction Then

        End If

        If pVal.ActionSuccess Then

            Dim strConectionString As String = String.Empty
            Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim strConsulta As String
            Dim cmdGoodEntries As New SqlClient.SqlCommand
            Dim drdGoodEntries As SqlClient.SqlDataReader

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString
            cn_Coneccion.Open()

            cmdGoodEntries.Connection = cn_Coneccion
            strConsulta = String.Format(" SELECT DocEntry FROM [@SCGD_GOODRECEIVE] with(nolock) WHERE U_As_Entr is null AND U_Unidad = '{0}' ",
                                    m_strUnidad)
            cmdGoodEntries.CommandType = CommandType.Text
            cmdGoodEntries.CommandText = strConsulta
            drdGoodEntries = cmdGoodEntries.ExecuteReader()

            Do While drdGoodEntries.Read

                If DMS_Connector.Configuracion.ParamGenAddon.U_CosteoLocal.Trim.Equals("Y") Then
                    CrearAsientoCosteoLocal(drdGoodEntries.GetInt32(0), m_strUnidad.Trim(), False, False, , , , , False)
                Else
                    CrearAsiento(drdGoodEntries.GetInt32(0))
                End If

            Loop

            drdGoodEntries.Close()

        End If

    End Sub

    ''' se crea el asiento para un numero de entrada especifico
    ''' se obtiene cuando se crea el ingreso contable del vehiculo usado

    Public Sub CrearAsientoParaNumeroEntradaEspecifico(ByVal p_intNumeroEntrada As Integer, Optional ByVal tipoVeh As String = Nothing, Optional ByVal p_fechaDocumento As Date = Nothing, Optional ByVal p_strNoUnidadUsada As String = Nothing, _
                                                       Optional ByVal p_blnCosteoLocal As String = "", Optional ByVal p_blnEntradaMultipleUsa As Boolean = False, Optional ByVal p_blnAsientoAjusteCosto As Boolean = False, Optional ByVal p_blnDimension As Boolean = False, _
                                                       Optional ByVal p_ListaConfiguracion As Hashtable = Nothing, Optional ByVal p_strDocCurrency As String = "", Optional ByVal p_blnUsaValCompAsEntrada As Boolean = False, Optional ByVal strCodeVehiculo As String = Nothing, _
                                                       Optional ByRef sqlConnection As SqlClient.SqlConnection = Nothing, Optional ByVal usaCompTransaction As Boolean = True, Optional ByRef sqlTransaction As SqlClient.SqlTransaction = Nothing, Optional ByVal blnContrato As Boolean = False)


        If Not String.IsNullOrEmpty(p_blnCosteoLocal) Then
            If p_blnCosteoLocal = "Y" Then
                CrearAsientoCosteoLocal(p_intNumeroEntrada, p_strNoUnidadUsada, p_blnEntradaMultipleUsa, p_blnAsientoAjusteCosto, strCodeVehiculo, usaCompTransaction, sqlConnection, sqlTransaction, blnContrato, tipoVeh)
            Else
                CrearAsiento(p_intNumeroEntrada, tipoVeh, p_fechaDocumento, p_blnDimension, p_ListaConfiguracion, p_strDocCurrency, p_blnUsaValCompAsEntrada, usaCompTransaction, strCodeVehiculo, sqlConnection, sqlTransaction, p_strNoUnidadUsada)
            End If

        Else
            CrearAsiento(p_intNumeroEntrada, tipoVeh, p_fechaDocumento, p_blnDimension, p_ListaConfiguracion, p_strDocCurrency, p_blnUsaValCompAsEntrada, usaCompTransaction, strCodeVehiculo, sqlConnection, sqlTransaction, p_strNoUnidadUsada)
        End If

    End Sub


    Public Function CrearAsiento(ByVal p_intDocEntry As Integer, Optional ByVal tipoVehi As String = Nothing, Optional ByVal p_fechaDocumento As Date = Nothing, _
                                 Optional ByVal p_blnDimension As Boolean = False, Optional ByVal p_ListaConfiguracion As Hashtable = Nothing, _
                                 Optional ByVal p_strDocCurrency As String = "", Optional ByVal p_blnUsaValCompAsEntrada As Boolean = False, Optional ByVal usaTransaction As Boolean = True, Optional ByVal p_strCodeVehiculo As String = Nothing, _
                                 Optional ByRef sqlConection As SqlClient.SqlConnection = Nothing, Optional ByRef sqlTransaccion As SqlClient.SqlTransaction = Nothing, Optional ByRef p_strNoUnidadUsada As String = Nothing) As Integer


        Dim oJournalEntry As SAPbobsCOM.JournalEntries

        Dim intError As Integer
        Dim strMensajeError As String = String.Empty
        Dim strMonedaLocal As String = String.Empty

        Dim strNoAsiento As String = String.Empty

        Dim decTotal As Decimal
        Dim strCuenta As String = String.Empty
        Dim strContraCuenta As String = String.Empty
        Dim strTipoVehiculo As String = String.Empty

        Dim strConectionString As String = String.Empty
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = String.Empty
        Dim cmdContraCuentas As New SqlClient.SqlCommand
        Dim drdContraCuentas As SqlClient.SqlDataReader
        Dim blnPrimeraCuenta As Boolean = True
        Dim blnEntradaInvalida As Boolean = False
        Dim strInvFacturado As String = String.Empty
        Dim strCodeVehiculo As String = String.Empty

        'manejo para validacion de importes negativos 
        Dim strImpNeg As String = String.Empty
        Dim CreaAsientoNormal As Boolean = False

        Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls
        Dim blnAgregarDimension As Boolean = False
        Dim strValorDimension As String
        Dim blnVieneDeContrato As Boolean = False

        Dim oCompanyServiceGR As SAPbobsCOM.CompanyService
        Dim oGeneralServiceGR As SAPbobsCOM.GeneralService
        Dim oGeneralDataGR As SAPbobsCOM.GeneralData
        Dim oGeneralParamsGR As SAPbobsCOM.GeneralDataParams

        Dim oCompanyServiceVH As SAPbobsCOM.CompanyService
        Dim oGeneralServiceVH As SAPbobsCOM.GeneralService
        Dim oGeneralDataVH As SAPbobsCOM.GeneralData
        Dim oGeneralParamsVH As SAPbobsCOM.GeneralDataParams

        Try

            strNoAsiento = 0

            If usaTransaction Then
                m_oCompany.StartTransaction()
            End If

            oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

            m_dtFecha = New Date(Utilitarios.EjecutarConsulta("Select Datepart(YEAR,Isnull(U_Fec_Cont,Getdate())) from [@SCGD_GOODRECEIVE] WITH (nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server), _
                    Utilitarios.EjecutarConsulta("Select Datepart(MONTH,Isnull(U_Fec_Cont,Getdate())) from [@SCGD_GOODRECEIVE] WITH (nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server), _
                    Utilitarios.EjecutarConsulta("Select Datepart(DAY,Isnull(U_Fec_Cont,Getdate())) from [@SCGD_GOODRECEIVE] WITH (nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server), _
                    0, 0, 0)

            strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
            m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema()

            If p_blnDimension Then

                Dim strNotaCreditoUsado As String = ConfiguracionesGeneralesAddon.scgTipoDocumentosCV.NotasCreditoUsados
                strValorDimension = p_ListaConfiguracion.Item(strNotaCreditoUsado)

                ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)
                blnVieneDeContrato = True

            Else
                strValorDimension = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] WITH (nolock)", m_oCompany.CompanyDB, m_oCompany.Server).Trim

                If strValorDimension = "Y" Then
                    p_blnDimension = True
                    ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)
                    blnVieneDeContrato = False
                End If

            End If

            'Verifico si el codigo de unidad viene vacio sino hago la consulta
            If String.IsNullOrEmpty(p_strNoUnidadUsada) Then
                m_strUnidad = Utilitarios.EjecutarConsulta("Select U_Unidad from [@SCGD_GOODRECEIVE] WITH (nolock) where DocEntry = " & p_intDocEntry, m_oCompany.CompanyDB, m_oCompany.Server).Trim
            Else
                m_strUnidad = p_strNoUnidadUsada
            End If


            If Not String.IsNullOrEmpty(m_strUnidad) Then

                strInvFacturado = objConfiguracionGeneral.InventarioVehiculoVendido

                If Not String.IsNullOrEmpty(tipoVehi) Then
                    strTipoVehiculo = tipoVehi
                Else
                    strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                End If

                'Comparo el inventario de la Unidad con el Inventario "Post Venta"
                If strTipoVehiculo = strInvFacturado Then
                    strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo_Ven FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                End If

                'Verifico que el code no venga vacio, sino hago la consulta
                If Not String.IsNullOrEmpty(p_strCodeVehiculo) Then
                    strCodeVehiculo = p_strCodeVehiculo
                Else
                    strCodeVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT Code FROM [@SCGD_VEHICULO] with(nolock) where U_Cod_Unid = '{0}'", m_strUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                End If

                oCompanyServiceVH = m_oCompany.GetCompanyService()
                oGeneralServiceVH = oCompanyServiceVH.GetGeneralService("SCGD_VEH")
                oGeneralParamsVH = oGeneralServiceVH.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsVH.SetProperty("Code", strCodeVehiculo)
                oGeneralDataVH = oGeneralServiceVH.GetByParams(oGeneralParamsVH)
                oGeneralDataVH.SetProperty("U_TIPINV", "C")
                oGeneralDataVH.SetProperty("U_SALINIC", "0")
                oGeneralDataVH.SetProperty("U_SALINID", "0")
                oGeneralServiceVH.Update(oGeneralDataVH)

                'Obtengo las cuentas configuradas por el Inventario del Vehiculo
                strCuenta = objConfiguracionGeneral.CuentaInventarioTransito(strTipoVehiculo)
                strContraCuenta = objConfiguracionGeneral.CuentaStock(strTipoVehiculo)

                oJournalEntry.Reference = m_strUnidad

                If p_fechaDocumento <> Nothing Then
                    oJournalEntry.ReferenceDate = p_fechaDocumento
                Else
                    oJournalEntry.ReferenceDate = m_dtFecha

                End If

                oJournalEntry.Memo = My.Resources.Resource.RegistroDiarioMemoEntrada & " " & m_strUnidad

                oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"


                strConsulta = "Select docentry Documento, isnull(case U_Cuenta when '' then NULL else U_Cuenta end, '" & strCuenta & "') Cuenta, SUM (U_Mon_Sis) MontoSistema, SUM (U_Mon_Loc) MontoLocal, Isnull(U_Mon_Reg,'') Moneda from [@SCGD_GRLINES] WITH (nolock) " & _
                                "where docentry = " & p_intDocEntry & " and (U_Mon_Sis <> 0 or U_Mon_Loc <> 0) " & _
                                "group by U_Cuenta, docentry, U_Mon_Reg "


                If sqlConection Is Nothing Then
                    Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
                    cn_Coneccion.ConnectionString = strConectionString
                    cn_Coneccion.Open()
                Else
                    cn_Coneccion = sqlConection
                End If

                cmdContraCuentas.Connection = cn_Coneccion

                If Not sqlTransaccion Is Nothing Then
                    cmdContraCuentas.Transaction = sqlTransaccion
                End If
                cmdContraCuentas.CommandType = CommandType.Text
                cmdContraCuentas.CommandText = strConsulta
                drdContraCuentas = cmdContraCuentas.ExecuteReader()

                decTotal = 0

                '******************************************************************************************
                'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                If p_blnDimension Then
                    If Not String.IsNullOrEmpty(strValorDimension) Then
                        If strValorDimension = "Y" Then
                            If blnVieneDeContrato Then
                                Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marca_Us from dbo.[@SCGD_USADOXCONT] WITH (nolock) where U_Cod_Unid = '" & m_strUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))
                            Else
                                Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '" & m_strUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                                oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))
                            End If

                        End If
                    End If

                    If oDataTableDimensiones.Rows.Count <> 0 Then

                        blnAgregarDimension = True

                    End If
                End If
                '******************************************************************************************

                Do While drdContraCuentas.Read
                    If Not blnPrimeraCuenta Then
                        oJournalEntry.Lines.Add()
                    Else
                        blnPrimeraCuenta = False
                    End If

                    If Not drdContraCuentas.IsDBNull(1) Then
                        oJournalEntry.Lines.AccountCode = drdContraCuentas.GetString(1)
                    End If
                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                    decTotal = drdContraCuentas.GetDecimal(3)

                    CreaAsientoNormal = False

                    'Cambio para nota de credito de proveedores
                    If decTotal < 0 Then

                        If String.IsNullOrEmpty(strImpNeg) Then
                            'obtengo configuracion para importes negativos 
                            strImpNeg = Utilitarios.EjecutarConsulta("SELECT NegAmount FROM OADM WITH (nolock)", m_oCompany.CompanyDB, m_oCompany.Server)
                        End If

                        If strImpNeg = "N" Then
                            'cuando se reciben valores negativos se invierten las cuentas
                            If Not drdContraCuentas.IsDBNull(4) AndAlso (drdContraCuentas.Item("Moneda") = strMonedaLocal Or drdContraCuentas.Item("Moneda") = "") Then
                                oJournalEntry.Lines.Debit = drdContraCuentas.GetDecimal(3) * -1
                                oJournalEntry.Lines.FCDebit = 0
                            Else
                                oJournalEntry.Lines.FCDebit = drdContraCuentas.GetDecimal(2) * -1
                                oJournalEntry.Lines.FCCurrency = m_strMonedaSistema
                            End If

                            oJournalEntry.Lines.ContraAccount = strContraCuenta
                            'linea de importe negativo 
                            oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "Y"

                            If blnAgregarDimension Then
                                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                            End If

                            'Cuenta
                            oJournalEntry.Lines.Add()
                            oJournalEntry.Lines.AccountCode = strContraCuenta
                            'linea de importe negativo 
                            oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "Y"
                            If Not drdContraCuentas.IsDBNull(4) AndAlso (drdContraCuentas.Item("Moneda") = strMonedaLocal Or drdContraCuentas.Item("Moneda") = "") Then
                                oJournalEntry.Lines.Credit = drdContraCuentas.GetDecimal(3) * -1
                                oJournalEntry.Lines.FCCredit = 0
                            Else
                                oJournalEntry.Lines.FCCredit = drdContraCuentas.GetDecimal(2) * -1
                                oJournalEntry.Lines.FCCurrency = m_strMonedaSistema
                            End If
                        ElseIf strImpNeg = "Y" Then
                            CreaAsientoNormal = True
                        End If

                    Else
                        ' cuando los valores son positivos
                        CreaAsientoNormal = True
                    End If

                    If CreaAsientoNormal Then
                        If Not drdContraCuentas.IsDBNull(4) AndAlso (drdContraCuentas.Item("Moneda") = strMonedaLocal Or drdContraCuentas.Item("Moneda") = "") Then
                            oJournalEntry.Lines.Credit = drdContraCuentas.GetDecimal(3)
                            oJournalEntry.Lines.FCCredit = 0
                        Else

                            'para valores no compensados en ME
                            If p_blnUsaValCompAsEntrada Then
                                oJournalEntry.Lines.Credit = drdContraCuentas.GetDecimal(3)
                            End If

                            oJournalEntry.Lines.FCCredit = drdContraCuentas.GetDecimal(2)
                            oJournalEntry.Lines.FCCurrency = m_strMonedaSistema
                        End If
                        oJournalEntry.Lines.ContraAccount = strContraCuenta
                        'Linea de importe positivo
                        oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"
                        If blnAgregarDimension Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                        End If

                        'Cuenta
                        oJournalEntry.Lines.Add()
                        oJournalEntry.Lines.AccountCode = strContraCuenta
                        'Linea de importe positivo
                        oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"
                        If Not drdContraCuentas.IsDBNull(4) AndAlso (drdContraCuentas.Item("Moneda") = strMonedaLocal Or drdContraCuentas.Item("Moneda") = "") Then
                            oJournalEntry.Lines.Debit = drdContraCuentas.GetDecimal(3)
                            oJournalEntry.Lines.FCDebit = 0
                        Else

                            'para valores no compensados en ME
                            If p_blnUsaValCompAsEntrada Then
                                oJournalEntry.Lines.Debit = drdContraCuentas.GetDecimal(3)
                            End If

                            oJournalEntry.Lines.FCDebit = drdContraCuentas.GetDecimal(2)
                            oJournalEntry.Lines.FCCurrency = m_strMonedaSistema

                        End If
                        If blnAgregarDimension Then
                            ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                        End If

                    End If


                    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

                Loop
                drdContraCuentas.Close()

                If oJournalEntry.Add <> 0 Then
                    If decTotal = 0 Then
                        blnEntradaInvalida = True
                    Else
                        strNoAsiento = "0"
                        m_oCompany.GetLastError(intError, strMensajeError)
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                        Throw New ExceptionsSBO(intError, strMensajeError)
                    End If
                Else
                    m_oCompany.GetNewObjectCode(strNoAsiento)

                    oCompanyServiceGR = m_oCompany.GetCompanyService()
                    oGeneralServiceGR = oCompanyServiceGR.GetGeneralService("SCGD_GOODENT")
                    oGeneralParamsGR = oGeneralServiceGR.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParamsGR.SetProperty("DocEntry", p_intDocEntry)
                    oGeneralDataGR = oGeneralServiceGR.GetByParams(oGeneralParamsGR)
                    oGeneralDataGR.SetProperty("U_As_Entr", strNoAsiento)
                    oGeneralDataGR.SetProperty("U_Tipo", strTipoVehiculo)
                    oGeneralServiceGR.Update(oGeneralDataGR)

                    If Not String.IsNullOrEmpty(strCodeVehiculo) Then
                        oCompanyServiceVH = m_oCompany.GetCompanyService()
                        oGeneralServiceVH = oCompanyServiceVH.GetGeneralService("SCGD_VEH")
                        oGeneralParamsVH = oGeneralServiceVH.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                        oGeneralParamsVH.SetProperty("Code", strCodeVehiculo)
                        oGeneralDataVH = oGeneralServiceVH.GetByParams(oGeneralParamsVH)
                        oGeneralDataVH.SetProperty("U_TIPINV", "C")
                        oGeneralServiceVH.Update(oGeneralDataVH)
                    End If


                    If usaTransaction Then
                        If m_oCompany.InTransaction Then
                            m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeAsientoGeneradoSuccesfull, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                        End If
                    End If

                End If

            Else
                blnEntradaInvalida = True
            End If
            If blnEntradaInvalida Then
                If sqlConection Is Nothing Then
                    If cn_Coneccion.State <> ConnectionState.Open Then
                        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
                        cn_Coneccion.ConnectionString = strConectionString
                        cn_Coneccion.Open()
                    End If
                Else
                    cn_Coneccion = sqlConection
                End If

                cmdContraCuentas.Connection = cn_Coneccion
                cmdContraCuentas.CommandType = CommandType.Text
                strNoAsiento = "-1"

                oCompanyServiceGR = m_oCompany.GetCompanyService()
                oGeneralServiceGR = oCompanyServiceGR.GetGeneralService("SCGD_GOODENT")
                oGeneralParamsGR = oGeneralServiceGR.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsGR.SetProperty("DocEntry", p_intDocEntry)
                oGeneralDataGR = oGeneralServiceGR.GetByParams(oGeneralParamsGR)
                oGeneralDataGR.SetProperty("U_As_Entr", strNoAsiento)
                oGeneralDataGR.SetProperty("U_Tipo", strTipoVehiculo)
                oGeneralServiceGR.Update(oGeneralDataGR)

                If Not String.IsNullOrEmpty(strCodeVehiculo) Then
                    oCompanyServiceVH = m_oCompany.GetCompanyService()
                    oGeneralServiceVH = oCompanyServiceVH.GetGeneralService("SCGD_VEH")
                    oGeneralParamsVH = oGeneralServiceVH.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParamsVH.SetProperty("Code", strCodeVehiculo)
                    oGeneralDataVH = oGeneralServiceVH.GetByParams(oGeneralParamsVH)
                    oGeneralDataVH.SetProperty("U_TIPINV", "C")
                    oGeneralServiceVH.Update(oGeneralDataVH)
                End If

                If usaTransaction Then
                    If m_oCompany.InTransaction Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If
                End If

            End If

            Return CInt(strNoAsiento)


        Catch ex As Exception

            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

            If cn_Coneccion.State = ConnectionState.Open Then
                If Not sqlTransaccion Is Nothing Then
                    sqlTransaccion.Rollback()
                End If
                cn_Coneccion.Close()
            End If

            If ex.Message = "Cuenta de Tránsito No Definida" Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCuentaTransito, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
            Else
                Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            End If
        End Try

    End Function


    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteCostoVehiculo(ByVal FormUID As String, _
                                ByRef pVal As SAPbouiCOM.ItemEvent, _
                                ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)
        If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE AndAlso oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
            strParametros = oForm.DataSources.DBDataSources.Item(mc_strSCG_GOODRECEIVE).GetValue("DocEntry", 0)
            strParametros = strParametros.Replace(" ", "°")

            'strDireccionReporte = Utilitarios.LeerValoresConfiguracion(m_oCompany.CompanyDB, "RPContratoVenta", m_strDireccionConfiguracion) & "\" & My.Resources.Resource.rptCostoVehiculo & ".rpt"
            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptCostoVehiculo & ".rpt"

            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= My.Resources.Resource.TituloCostoVehiculo.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)
        Else
            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCrearDocumentoAntesImprimir, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        End If
    End Sub

#End Region

#Region "Metodos Nuevo Costeo"
    Public Sub CrearAsientoCosteoLocal(ByVal p_DocEntry As Integer, ByVal p_NoUnidad As String, ByVal p_blnEntradaMultipleUsa As Boolean, _
                                       ByVal p_blnAsientoAjusteCosto As Boolean, Optional ByVal p_strCodeVehiculo As String = Nothing, _
                                       Optional ByVal usaTransaction As Boolean = True, _
                                       Optional ByRef sqlConection As SqlClient.SqlConnection = Nothing, _
                                       Optional ByRef sqlTransaccion As SqlClient.SqlTransaction = Nothing, Optional ByVal blnContrato As Boolean = False, Optional ByVal p_strTipInvCont As String = "")
        Dim dtGRLines As System.Data.DataTable
        Dim intAsientoGenerado As Integer = 0
        Dim strNoAsiento As String = String.Empty
        Dim strTipoVehiculo As String = String.Empty

        Dim oCompanyServiceGR As SAPbobsCOM.CompanyService
        Dim oGeneralServiceGR As SAPbobsCOM.GeneralService
        Dim oGeneralDataGR As SAPbobsCOM.GeneralData
        Dim oGeneralParamsGR As SAPbobsCOM.GeneralDataParams

        Dim oCompanyServiceVH As SAPbobsCOM.CompanyService
        Dim oGeneralServiceVH As SAPbobsCOM.GeneralService
        Dim oGeneralDataVH As SAPbobsCOM.GeneralData
        Dim oGeneralParamsVH As SAPbobsCOM.GeneralDataParams

        Dim cmdContraCuentas As New SqlClient.SqlCommand
        Dim strConectionString As String = String.Empty
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Try

            dtGRLines = Utilitarios.EjecutarConsultaDataTable(String.Format("select DocEntry, U_NoAsient, U_Concepto, U_Mon_Loc from dbo.[@SCGD_GRLINES] WITH (nolock) where DocEntry= '{0}'",
                                                             p_DocEntry),
                                                          m_oCompany.CompanyDB,
                                                          m_oCompany.Server)

            If usaTransaction Then
                m_oCompany.StartTransaction()
            End If

            intAsientoGenerado = CrearAsientoEntradaCosteoLocal(dtGRLines, p_DocEntry, p_NoUnidad, p_blnEntradaMultipleUsa, p_blnAsientoAjusteCosto, strTipoVehiculo, , , blnContrato, p_strTipInvCont)

            If intAsientoGenerado <> 0 Then
                strNoAsiento = intAsientoGenerado.ToString.Trim()
            Else
                strNoAsiento = "-1"
            End If

            oCompanyServiceGR = m_oCompany.GetCompanyService()
            oGeneralServiceGR = oCompanyServiceGR.GetGeneralService("SCGD_GOODENT")
            oGeneralParamsGR = oGeneralServiceGR.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParamsGR.SetProperty("DocEntry", p_DocEntry)
            oGeneralDataGR = oGeneralServiceGR.GetByParams(oGeneralParamsGR)
            oGeneralDataGR.SetProperty("U_As_Entr", strNoAsiento)
            oGeneralDataGR.SetProperty("U_Tipo", strTipoVehiculo)

            If Not String.IsNullOrEmpty(p_strCodeVehiculo) Then
                oCompanyServiceVH = m_oCompany.GetCompanyService()
                oGeneralServiceVH = oCompanyServiceVH.GetGeneralService("SCGD_VEH")
                oGeneralParamsVH = oGeneralServiceVH.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParamsVH.SetProperty("Code", p_strCodeVehiculo)
                oGeneralDataVH = oGeneralServiceVH.GetByParams(oGeneralParamsVH)
                oGeneralDataVH.SetProperty("U_TIPINV", "C")
                oGeneralDataVH.SetProperty("U_SALINIC", "0")
                oGeneralDataVH.SetProperty("U_SALINID", "0")
                oGeneralServiceVH.Update(oGeneralDataVH)
            Else
                If sqlConection Is Nothing Then
                    If cn_Coneccion.State <> ConnectionState.Open Then
                        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
                        cn_Coneccion.ConnectionString = strConectionString
                        cn_Coneccion.Open()
                    End If
                Else
                    cn_Coneccion = sqlConection
                End If
                cmdContraCuentas.Connection = cn_Coneccion
                cmdContraCuentas.CommandType = CommandType.Text
                cmdContraCuentas.CommandText = "Update [@SCGD_VEHICULO] set U_TIPINV = 'C', U_SALINIC = 0, U_SALINID = 0 where U_Cod_Unid = '" & m_strUnidad & "'"
                cmdContraCuentas.ExecuteNonQuery()
                cmdContraCuentas.Connection.Close()
            End If

            oGeneralServiceGR.Update(oGeneralDataGR)

            If usaTransaction Then
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
            End If

        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            If cn_Coneccion.State = ConnectionState.Open Then
                If Not sqlTransaccion Is Nothing Then
                    sqlTransaccion.Rollback()
                End If
                cn_Coneccion.Close()
            End If
            If ex.Message = "Cuenta de Tránsito No Definida" Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorCuentaTransito, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)
            Else
                Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            End If
        End Try
    End Sub

    Public Function CrearAsientoEntradaCosteoLocal(ByVal p_dtGRLines As System.Data.DataTable, ByVal p_strDocEntry As String, ByVal p_strNoUnidad As String, ByVal p_blnEntradaMultipleUsa As Boolean, ByVal p_blnAsientoAjusteCosto As Boolean, ByRef p_strTipoVehiculo As String,
                                      Optional ByVal p_blnUsaDimension As Boolean = False, Optional ByVal p_listaConfiguracion As Hashtable = Nothing, Optional ByVal blnContrato As Boolean = False, Optional ByVal p_strTipInvCont As String = "") As Integer
        Try

            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim oListaLineasAsiento As New List(Of ListaLineaAsientoEntrada)()
            Dim oListaAsiento As New List(Of ListaLineaAsientoEntrada)()
            Dim rowGRLines As System.Data.DataRow
            Dim strAsientoGenerado As String = "0"
            Dim strMonedaLocal As String = String.Empty
            Dim strCuentaTransito As String = String.Empty
            Dim strCuentaInventario As String = String.Empty
            Dim strTipoVehiculo As String = String.Empty
            Dim strFechaFormateada As String
            Dim strFechaConta As String
            Dim intError As Integer
            Dim strMensajeError As String
            Dim formato As String
            Dim dateFechaRegistro As Date = Nothing
            Dim decCostosLocales As Decimal = 0
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls
            Dim blnAgregarDimension As Boolean = False
            Dim strValorDimension As String
            Dim blnVieneDeContrato As Boolean = False
            Dim strInvFacturado As String

            'manejo para validacion de importes negativos 
            Dim strImpNeg As String = Utilitarios.EjecutarConsulta("SELECT NegAmount FROM OADM  WITH (nolock)", m_oCompany.CompanyDB, m_oCompany.Server).Trim
            
            'Si proviene del Contrato use el del vehiculo usado
            If blnContrato Then
                strTipoVehiculo = p_strTipInvCont
            End If

            If String.IsNullOrEmpty(strTipoVehiculo) Then

                strInvFacturado = objConfiguracionGeneral.InventarioVehiculoVendido

                strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo FROM [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'", p_strNoUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim

                ' Comparo el inventario de la Unidad con el Inventario "Post Venta"
                If strInvFacturado = strTipoVehiculo Then
                    strTipoVehiculo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Tipo_Ven FROM [@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '{0}'", p_strNoUnidad), m_oCompany.CompanyDB, m_oCompany.Server).Trim
                End If

            End If

            p_strTipoVehiculo = strTipoVehiculo

            'Inicio: Dar Formato a Fecha de Contabilización
            strFechaConta = Utilitarios.EjecutarConsulta(
                            String.Format("Select convert(date,U_Fec_Cont) from [@SCGD_GOODRECEIVE] WITH (nolock) where DocEntry = '{0}' ", p_strDocEntry),
                                                                    m_oCompany.CompanyDB,
                                                                    m_oCompany.Server)
            strFechaFormateada = Utilitarios.EjecutarConsulta(
                            String.Format(" Select LEFT(CONVERT(VARCHAR, U_Fec_Cont, 103),10) from [@SCGD_GOODRECEIVE] WITH (nolock) where DocEntry ='{0}' ", p_strDocEntry),
                                                                    m_oCompany.CompanyDB,
                                                                    m_oCompany.Server)
            If strFechaConta.Contains("-") Then
                strFechaFormateada = strFechaFormateada.Replace("/", "-")
            End If
            formato = Utilitarios.ObtieneFormatoFecha(SBO_Application, m_oCompany)

            If IsDate(strFechaConta) Then dateFechaRegistro = Date.ParseExact(strFechaFormateada, formato, Nothing)

            'Fin: Formato Fecha de Contalibización


            If p_blnUsaDimension Then
                Dim strNotaCreditoUsado As String = ConfiguracionesGeneralesAddon.scgTipoDocumentosCV.NotasCreditoUsados
                strValorDimension = p_listaConfiguracion.Item(strNotaCreditoUsado)
                ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)
                blnVieneDeContrato = True
            Else
                If DMS_Connector.Configuracion.ParamGenAddon.U_UsaDimC.Trim.Equals("Y") Then
                    p_blnUsaDimension = True
                    ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(m_oCompany, SBO_Application)
                    blnVieneDeContrato = False
                End If
            End If


            '******************************************************************************************
            'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
            If p_blnUsaDimension Then
                If Not String.IsNullOrEmpty(strValorDimension) Then
                    If strValorDimension = "Y" Then
                        If blnVieneDeContrato Then
                            Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marca_Us from dbo.[@SCGD_USADOXCONT] WITH (nolock) where U_Cod_Unid = '" & p_strNoUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                            oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))
                        Else
                            Dim strCodigoMarca As String = Utilitarios.EjecutarConsulta("Select U_Cod_Marc from dbo.[@SCGD_VEHICULO] WITH (nolock) where U_Cod_Unid = '" & p_strNoUnidad.Trim & "'", m_oCompany.CompanyDB, m_oCompany.Server)
                            oDataTableDimensiones = (ClsLineasDocumentosDimension.DatatableDimensionesContablesDMS(strTipoVehiculo, strCodigoMarca))
                        End If

                        If oDataTableDimensiones.Rows.Count <> 0 Then
                            blnAgregarDimension = True
                        End If
                    End If
                End If
            End If

            If dateFechaRegistro <> Nothing Then
                'Carga de cuentas contables
                strCuentaTransito = objConfiguracionGeneral.CuentaInventarioTransito(strTipoVehiculo)
                strCuentaInventario = objConfiguracionGeneral.CuentaStock(strTipoVehiculo)
                strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()

                If Not String.IsNullOrEmpty(strCuentaTransito) Then
                    For Each rowGRLines In p_dtGRLines.Rows
                        decCostosLocales = Decimal.Parse(rowGRLines.Item("U_Mon_Loc"))
                        oListaLineasAsiento.Add(New ListaLineaAsientoEntrada() With {.Debit = decCostosLocales, .Credit = 0})
                    Next
                End If

                Dim decMontoTemp As Decimal = 0
                Dim blnAgregar As Boolean = False
                Dim strMoneda As String = String.Empty

                For Each C1 As ListaLineaAsientoEntrada In oListaLineasAsiento

                    decMontoTemp = 0
                    blnAgregar = False
                    strMoneda = String.Empty
                    If Not String.IsNullOrEmpty(C1.FCCurrency) Then
                        strMoneda = C1.FCCurrency
                    Else
                        strMoneda = strMonedaLocal
                    End If

                    For Each C2 As ListaLineaAsientoEntrada In oListaLineasAsiento

                        If Not String.IsNullOrEmpty(C1.FCCurrency) And Not String.IsNullOrEmpty(C2.FCCurrency) And C1.FCCurrency = C2.FCCurrency And C2.Aplicado = False Then
                            If C2.FCDebit <> 0 Then
                                decMontoTemp += C2.FCDebit
                                C2.Aplicado = True
                                blnAgregar = True
                            ElseIf C2.FCCredit <> 0 Then
                                If C2.ImpNeg <> "N" Then
                                    decMontoTemp += (C2.FCCredit * -1)
                                    C2.Aplicado = True
                                    blnAgregar = True
                                Else
                                    C2.Aplicado = True
                                End If
                            End If
                        ElseIf String.IsNullOrEmpty(C1.FCCurrency) And String.IsNullOrEmpty(C2.FCCurrency) And C1.FCCurrency = C2.FCCurrency And C2.Aplicado = False Then
                            If C2.Debit <> 0 Then
                                decMontoTemp += C2.Debit
                                C2.Aplicado = True
                                blnAgregar = True
                            ElseIf C2.Credit <> 0 Then
                                If C2.ImpNeg <> "N" Then
                                    decMontoTemp += (C2.Credit * -1)
                                    C2.Aplicado = True
                                    blnAgregar = True
                                Else
                                    C2.Aplicado = True
                                End If
                            End If
                        End If
                    Next
                    If blnAgregar = True And decMontoTemp > 0 Then
                        If strMonedaLocal = strMoneda Then
                            oListaAsiento.Add(New ListaLineaAsientoEntrada() With {.FCCurrency = strMonedaLocal, .Debit = decMontoTemp, .Credit = decMontoTemp, .Aplicado = True, .AumentaStock = True})
                        Else
                            oListaAsiento.Add(New ListaLineaAsientoEntrada() With {.FCCurrency = strMoneda, .FCDebit = decMontoTemp, .FCCredit = decMontoTemp, .Aplicado = True, .AumentaStock = True})
                        End If
                    ElseIf blnAgregar = True And decMontoTemp < 0 Then
                        If strMonedaLocal = strMoneda Then
                            oListaAsiento.Add(New ListaLineaAsientoEntrada() With {.FCCurrency = strMonedaLocal, .Debit = decMontoTemp, .Credit = decMontoTemp, .Aplicado = True, .ImpNeg = strImpNeg, .AumentaStock = False})
                        Else
                            oListaAsiento.Add(New ListaLineaAsientoEntrada() With {.FCCurrency = strMoneda, .FCDebit = decMontoTemp, .FCCredit = decMontoTemp, .Aplicado = True, .ImpNeg = strImpNeg, .AumentaStock = False})
                        End If
                    End If
                Next

                If oListaAsiento.Count() > 0 Then

                    strAsientoGenerado = "0"

                    oJournalEntry = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJournalEntry.Memo = My.Resources.Resource.RegistroDiarioMemoEntrada & " " & p_strNoUnidad
                    oJournalEntry.Reference = p_strNoUnidad
                    oJournalEntry.UserFields.Fields.Item("U_SCGD_AplVal").Value = "0"

                    If dateFechaRegistro <> Nothing Then
                        oJournalEntry.ReferenceDate = dateFechaRegistro
                    End If

                    For Each row As ListaLineaAsientoEntrada In oListaAsiento

                        If row.AumentaStock = True Then
                            '*********************
                            ' Contra cuenta
                            'Cuenta Credito
                            '*********************
                            oJournalEntry.Lines.AccountCode = strCuentaTransito

                            If strMonedaLocal = row.FCCurrency Then
                                oJournalEntry.Lines.Credit = row.Credit
                                oJournalEntry.Lines.FCCredit = 0
                            Else
                                oJournalEntry.Lines.FCCredit = row.FCCredit
                                oJournalEntry.Lines.FCCurrency = row.FCCurrency
                            End If
                            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                            oJournalEntry.Lines.Reference1 = p_strNoUnidad
                            oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

                            If blnAgregarDimension Then
                                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                            End If

                            oJournalEntry.Lines.Add()

                            '*****************
                            'Cuenta Debito
                            '*****************
                            oJournalEntry.Lines.AccountCode = strCuentaInventario
                            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                            oJournalEntry.Lines.Reference1 = p_strNoUnidad

                            If strMonedaLocal = row.FCCurrency Then
                                oJournalEntry.Lines.Debit = row.Debit
                                oJournalEntry.Lines.FCDebit = 0
                            Else
                                oJournalEntry.Lines.FCDebit = row.FCDebit
                                oJournalEntry.Lines.FCCurrency = row.FCCurrency
                            End If

                            oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

                            If blnAgregarDimension Then
                                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                            End If

                            oJournalEntry.Lines.Add()

                        ElseIf row.AumentaStock = False Then

                            If strImpNeg = "N" Then
                                'Se deden de invertir las cuentas ya que no utiliza importen negativos

                                '*********************
                                ' Contra cuenta
                                'Cuenta Credito
                                '*********************
                                oJournalEntry.Lines.AccountCode = strCuentaInventario

                                If strMonedaLocal = row.FCCurrency Then
                                    oJournalEntry.Lines.Credit = row.Credit * -1
                                    oJournalEntry.Lines.FCCredit = 0
                                Else
                                    oJournalEntry.Lines.FCCredit = row.FCCredit * -1
                                    oJournalEntry.Lines.FCCurrency = row.FCCurrency
                                End If
                                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                                oJournalEntry.Lines.Reference1 = p_strNoUnidad
                                oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "Y"

                                If blnAgregarDimension Then
                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                                End If
                                oJournalEntry.Lines.Add()
                                '*****************
                                'Cuenta Debito
                                '*****************
                                oJournalEntry.Lines.AccountCode = strCuentaTransito
                                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                                oJournalEntry.Lines.Reference1 = p_strNoUnidad

                                If strMonedaLocal = row.FCCurrency Then
                                    oJournalEntry.Lines.Debit = row.Debit * -1
                                    oJournalEntry.Lines.FCDebit = 0
                                Else
                                    oJournalEntry.Lines.FCDebit = row.FCDebit * -1
                                    oJournalEntry.Lines.FCCurrency = row.FCCurrency
                                End If

                                oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "Y"

                                If blnAgregarDimension Then
                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                                End If

                                oJournalEntry.Lines.Add()

                            ElseIf strImpNeg = "Y" Then

                                '*********************
                                ' Contra cuenta
                                'Cuenta Credito
                                '*********************
                                oJournalEntry.Lines.AccountCode = strCuentaTransito

                                If strMonedaLocal = row.FCCurrency Then
                                    oJournalEntry.Lines.Credit = row.Credit
                                    oJournalEntry.Lines.FCCredit = 0
                                Else
                                    oJournalEntry.Lines.FCCredit = row.FCCredit
                                    oJournalEntry.Lines.FCCurrency = row.FCCurrency
                                End If
                                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                                oJournalEntry.Lines.Reference1 = p_strNoUnidad
                                oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

                                If blnAgregarDimension Then
                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                                End If
                                oJournalEntry.Lines.Add()

                                '*****************
                                'Cuenta Debito
                                '*****************
                                oJournalEntry.Lines.AccountCode = strCuentaInventario
                                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                                oJournalEntry.Lines.Reference1 = p_strNoUnidad

                                If strMonedaLocal = row.FCCurrency Then
                                    oJournalEntry.Lines.Debit = row.Debit
                                    oJournalEntry.Lines.FCDebit = 0
                                Else
                                    oJournalEntry.Lines.FCDebit = row.FCDebit
                                    oJournalEntry.Lines.FCCurrency = row.FCCurrency

                                End If
                                oJournalEntry.Lines.UserFields.Fields.Item("U_SCGD_ImpNeg").Value = "N"

                                If blnAgregarDimension Then
                                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, oDataTableDimensiones, Nothing)
                                End If

                                oJournalEntry.Lines.Add()
                            End If

                        End If

                    Next


                    If oJournalEntry.Add <> 0 Then
                        strAsientoGenerado = "0"
                        m_oCompany.GetLastError(intError, strMensajeError)
                        Throw New ExceptionsSBO(intError, strMensajeError)
                    Else
                        m_oCompany.GetNewObjectCode(strAsientoGenerado)
                    End If
                End If
            End If
            Return CInt(strAsientoGenerado)
        Catch ex As Exception

        End Try
    End Function
#End Region

End Class
' Clase para la definición de la lista
Public Class ListaLineaAsientoEntrada

    Public Property FCCurrency() As String
        Get
            Return strFCCurrency
        End Get
        Set(ByVal value As String)
            strFCCurrency = value
        End Set
    End Property
    Private strFCCurrency As String

    Public Property Debit() As Decimal
        Get
            Return decDebit
        End Get
        Set(ByVal value As Decimal)
            decDebit = value
        End Set
    End Property
    Private decDebit As Decimal

    Public Property Credit() As Decimal
        Get
            Return decCredit
        End Get
        Set(ByVal value As Decimal)
            decCredit = value
        End Set
    End Property
    Private decCredit As Decimal

    Public Property FCDebit() As Decimal
        Get
            Return decFCDebit
        End Get
        Set(ByVal value As Decimal)
            decFCDebit = value
        End Set
    End Property
    Private decFCDebit As Decimal


    Public Property FCCredit() As Decimal
        Get
            Return decFCCredit
        End Get
        Set(ByVal value As Decimal)
            decFCCredit = value
        End Set
    End Property
    Private decFCCredit As Decimal

    Public Property Account() As String
        Get
            Return strAccount
        End Get
        Set(ByVal value As String)
            strAccount = value
        End Set
    End Property
    Private strAccount As String

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean

    Public Property ImpNeg() As String
        Get
            Return strImpNeg
        End Get
        Set(ByVal value As String)
            strImpNeg = value
        End Set
    End Property
    Private strImpNeg As String

    Public Property AumentaStock() As Boolean
        Get
            Return blnAumentaStock
        End Get
        Set(ByVal value As Boolean)
            blnAumentaStock = value
        End Set
    End Property
    Private blnAumentaStock As Boolean

End Class
