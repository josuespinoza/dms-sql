Imports System.Linq
Imports DMSOneFramework
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess
Imports SCG.SBOFramework.DI

'*******************************************
'*Maneja el formulario para incluir repuestos en la OT
'*******************************************

Partial Public Class IncluirRepuestosOT

#Region "Declaraciones"

    Public _hsAprobado As New Hashtable
    Public _hsTrasladado As New Hashtable

    'datatable
    Dim dtAprobado As SAPbouiCOM.DataTable
    Dim dtTrasladado As SAPbouiCOM.DataTable
    Dim g_strdtBusqueda As String = "tBusqueda"
    Dim _blnCambio As Boolean

    'obj global
    Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO


    Private Const g_strTipoArticulo As String = "U_SCGD_TipoArticulo"
    Private Const g_strCodCentroCosto As String = "U_SCGD_CodCtroCosto"
    Private Const g_strNum_OT As String = "U_SCGD_Numero_OT"
    Private Const g_strItemAprobado As String = "U_SCGD_Aprobado"
    Private Const g_strTrasladado As String = "U_SCGD_Traslad"
    Private Const g_strGenerico As String = "U_SCGD_Generico"
    Private Const g_strItemAProcesar As String = "U_SCGD_Procesar"
    Private Const g_strResultado As String = "U_SCGD_Resultado"
    Private Const g_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"
    Private Const g_strImprimirOT As String = "U_SCGD_GeneraOR"

    Private Const g_strCodeEspecifico As String = "U_SCGD_CodEspecifico"
    Private Const g_strNameEspecifico As String = "U_SCGD_NombEspecific"

    Public Const mc_strNombEmpleado As String = "U_SCGD_NombEmpleado"
    Public Const mc_strEmpRealiza As String = "U_SCGD_Emp_Realiza"
    'Public g_strSucursal As String

    Private g_blnDraft As Boolean

    Private m_intRealizarTraslados As RealizarTraslados

    Private dtbRepuestosxOrden As New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable

    Private m_lstCantidadesAnteriores As New Generic.List(Of stTipoListaCantAnteriores)


    Private m_drwRepuestos As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow
    Private m_dstRepuestosxOrden As RepuestosxOrdenDataset
    Private m_adpRepuestosxOrden As RepuestosxOrdenDataAdapter

    Private dtbLineasActualizadas As New dtsMovimientoStock.LineaActualizadaDataTable
    Private objTransferenciaStock As TransferenciaItems

    Dim adpConf As ConfiguracionDataAdapter
    Dim dstConf As New ConfiguracionDataSet

    Private g_trnTransaccion As SqlClient.SqlTransaction
    Private g_cnnSCGTaller As SqlClient.SqlConnection

    Public g_blnModificaItemsAdicionales As Boolean = False

    Private g_intEstCotizacion As Integer

    'para validar los LineNumErroneos
    Private LineNumDelete As Nullable(Of Integer)
    Private objItemsLineasLineNumErroneos As New Generic.List(Of LineasLineNumErroneos)
    Private objItemLineNumErroneo As New LineasLineNumErroneos

    Private m_lstRepuestos As New Generic.List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstSuministros As New Generic.List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstServiociosEX As New Generic.List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstItemsEliminarRepuestos As New Generic.List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstItemsEliminarSuministros As New Generic.List(Of TransferenciaItems.LineasTransferenciaStock)
    Private m_lstItemACambiarEstado As New Generic.List(Of TransferenciaItems.LineasCambiarEstado)
    Private m_lstItemACambiarEstadoAdicional As New Generic.List(Of TransferenciaItems.LineasCambiarEstado)


    Private Const gcol_strCardCode As String = "CardCode"
    Private Const gcol_strDescMarca As String = "U_SCGD_Des_Marc"
    Private Const gcol_strDesc_Estilo As String = "U_SCGD_Des_Esti"
    Private Const gcol_strDescModelo As String = "U_SCGD_Des_Mode"
    Private Const gcol_strPlaca As String = "U_SCGD_Num_Placa"
    Private Const gcol_strVIN As String = "U_SCGD_Num_VIN"
    Private Const gcol_strNum_Visita As String = "U_SCGD_No_Visita"


    Private g_strPlaca As String
    Private g_strVIN As String
    Private g_strDescMarca As String
    Private g_strDescEstilo As String
    Private g_strDescModelo As String
    Private g_strEmpleadoRecibe As String
    Private g_strCodigoCliente As String
    Private g_strNoVisita As String
    Private g_strNoOrden As String

    Private g_strBodegaRepuestos As String = String.Empty
    Private g_strBodegaProcesoRepuestos As String = String.Empty
    Private g_strBodegaProcesoSuministros As String = String.Empty

    Private m_strNoBodegaRepu As String = ""
    Private m_strNoBodegaSumi As String = ""
    Private m_strNoBodegaSeEx As String = ""
    Private m_strNoBodegaProceso As String = ""
    Private m_strIDSerieDocTrasnf As String = ""

#End Region

#Region "Propiedades"

    Public Property blnCambio As Boolean
        Get
            Return _blnCambio
        End Get
        Set(ByVal value As Boolean)
            _blnCambio = value
        End Set
    End Property


    Public Property hsAprobado As Hashtable
        Get
            Return _hsAprobado
        End Get
        Set(ByVal value As Hashtable)
            _hsAprobado = value
        End Set
    End Property

    Public Property hsTrasladado As Hashtable
        Get
            Return _hsTrasladado
        End Get
        Set(ByVal value As Hashtable)
            _hsTrasladado = value
        End Set
    End Property


#End Region

#Region "Enumeraciones"

    Private Enum TiposArticulos
        scgRepuesto = 1
        scgActividad = 2
        scgSuministro = 3
        scgServicioExt = 4
        scgPaquete = 5
        scgNinguno = 0
    End Enum

    Private Enum EstadosTraslados
        NoProcesado = 0
        No = 1
        Si = 2
        PendienteTraslado = 3
        PendienteBodega = 4
    End Enum

    Private Enum EstadosAprobacion
        Aprobado = 1
        NoAprobado = 2
        FaltoAprobacion = 3
    End Enum

    Private Enum ResultadoValidacionPorItem
        scgSinCambio = 0
        scgNoAprobar = 1
        scgModQtyCoti = 2
        scgPendTransf = 3
        scgComprar = 4
        scgPendBodega = 5
    End Enum

    Private Enum RealizarTraslados
        Si = 1
        No = 0
    End Enum

    Private Enum CotizacionEstado
        creada = 1
        modificada = 2
        sinCambio = 3
    End Enum

    Private Enum LineaAProcesar
        scgSi = 1
        scgNo = 2
    End Enum

    Private Structure LineasLineNumErroneos
        Dim NoOrden As String
        Dim IdItem As String
        Dim Id As String
        Dim intLineNum As Integer
        Dim TipoRow As Integer
    End Structure

    Private Enum enumTipoRow
        scgRepuestoRow = 1
        scgSuministroRow = 2
        scgActividadRow = 3
    End Enum


    Private Enum ImprimirOT
        scgSi = 1
        scgNo = 2
    End Enum

#End Region

#Region "Estructuras"

    Structure stTipoListaCantAnteriores
        Dim ItemCode As String
        Dim LineNum As Integer
        Dim Cantidad As Decimal
    End Structure

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Carga repuestos a la matriz dependiendo del numero de Órden de Trabajo
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaRepuestos(oCotizacion As Documents)
        Dim intRowDT As Integer
        Dim BloquearControles As Boolean = False
        Try
            
            dtRepuestos = FormularioSBO.DataSources.DataTables.Item(strDTRepuestos)

            If Not oCotizacion Is Nothing Then
                dtRepuestos.Rows.Clear()
                For i As Integer = 0 To oCotizacion.Lines.Count - 1
                    oCotizacion.Lines.SetCurrentLine(i)

                    If oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = "1" Then
                        dtRepuestos.Rows.Add(1)
                        intRowDT = dtRepuestos.Rows.Count - 1
                        dtRepuestos.SetValue("per", intRowDT, My.Resources.Resource.Si)
                        dtRepuestos.SetValue("cod", intRowDT, oCotizacion.Lines.ItemCode)
                        dtRepuestos.SetValue("des", intRowDT, oCotizacion.Lines.ItemDescription)
                        dtRepuestos.SetValue("can", intRowDT, oCotizacion.Lines.Quantity)
                        dtRepuestos.SetValue("mon", intRowDT, oCotizacion.Lines.Currency)
                        dtRepuestos.SetValue("pre", intRowDT, oCotizacion.Lines.Price)
                        dtRepuestos.SetValue("apr", intRowDT, DMS_Connector.Configuracion.Aprobado.FirstOrDefault(Function(x) x.Code = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value).Name)
                        dtRepuestos.SetValue("tra", intRowDT, DMS_Connector.Configuracion.Trasladado.FirstOrDefault(Function(x) x.Code = oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value).Name)
                        dtRepuestos.SetValue("ln", intRowDT, oCotizacion.Lines.LineNum)
                        dtRepuestos.SetValue("com", intRowDT, oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value)
                        dtRepuestos.SetValue("rec", intRowDT, oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value)
                        dtRepuestos.SetValue("sol", intRowDT, oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value)
                    End If

                Next

                MatrizRepuestosOT.Matrix.LoadFromDataSource()

                'If (oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "1" Or oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "2") Then
                '    FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                '    FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                'Else
                '    BloquearControles = True
                'End If

            Else
                dtRepuestos.Rows.Clear()
                MatrizRepuestosOT.Matrix.LoadFromDataSource()
                BloquearControles = True
            End If

            'If BloquearControles Then
            '    'modificacion para botones de agregar y eliminar repuestos
            '    FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            '    FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            'Else
            '    'modificacion para botones de agregar y eliminar repuestos
            '    FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            '    FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            'End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Crear las listas para los valores de los UDF's
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    Public Sub CargaValoresUDF(ByVal oForm As Form)

        Dim strConsultaAprob As String = " select FldValue, Descr from UFD1 with(nolock) where TableID = 'QUT1' and FieldID in (select FieldID from CUFD with(nolock) where AliasID = 'SCGD_Aprobado') "
        Dim strConsultaTrasl As String = " select FldValue, Descr from UFD1 with(nolock) where TableID = 'QUT1' and FieldID in (select FieldID from CUFD with(nolock) where AliasID = 'SCGD_Traslad')"

        Try
            hsAprobado.Clear()
            dtAprobado = oForm.DataSources.DataTables.Item("tAprobado")
            dtTrasladado = oForm.DataSources.DataTables.Item("tTrasladado")
            dtAprobado.ExecuteQuery(strConsultaAprob)
            dtTrasladado.ExecuteQuery(strConsultaTrasl)
            hsAprobado.Clear()
            hsTrasladado.Clear()
            For i As Integer = 0 To dtAprobado.Rows.Count - 1
                hsAprobado.Add(dtAprobado.GetValue("FldValue", i), dtAprobado.GetValue("Descr", i))
            Next
            For i As Integer = 0 To dtTrasladado.Rows.Count - 1
                hsTrasladado.Add(dtTrasladado.GetValue("FldValue", i), dtTrasladado.GetValue("Descr", i))
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    ''' <summary>
    ''' Obtiene de las listas el valor de acuerdo al que obtiene de la 
    ''' tabla 
    ''' </summary>
    ''' <param name="dtRepuestos"></param>
    ''' <param name="colAprobacion"></param>
    ''' <param name="colTraslado"></param>
    ''' <param name="oForm"></param>
    ''' <remarks></remarks>
    'Private Sub ActualizaDescripcionesPorUDF(ByRef dtRepuestos As SAPbouiCOM.DataTable,
    '                                  ByVal colAprobacion As String,
    '                                  ByVal colTraslado As String,
    '                                  ByVal oForm As Form)
    '    Dim keyAprob As String = ""
    '    Dim keyTras As String = ""

    '    Try
    '        CargaValoresUDF(oForm)
    '        For i As Integer = 0 To dtRepuestos.Rows.Count - 1

    '            keyAprob = hsAprobado(dtRepuestos.GetValue(colAprobacion, i).ToString())
    '            keyTras = hsTrasladado(dtRepuestos.GetValue(colTraslado, i).ToString())

    '            dtRepuestos.SetValue(colAprobacion, i, keyAprob)
    '            dtRepuestos.SetValue(colTraslado, i, keyTras)
    '        Next
    '    Catch ex As Exception
    '        Utilitarios.ManejadorErrores(ex, ApplicationSBO)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Agrega los repuestos a la matriz de la cotización
    ''' </summary>
    ''' <param name="dtSeleccionados"></param>
    ''' <remarks></remarks>
    Public Sub IncluirRepuestosSeleccionados(ByVal dtSeleccionados As SAPbouiCOM.DataTable, ByRef BubbleEvent As Boolean)

        Dim oMatrix As Matrix
        Dim oForm As Form
        Dim oEditText As EditText
        Dim Posicion As Integer = 0
        Dim dcPrecio As Decimal
        Dim strAprobacion As String
        Dim dcCantidad As Decimal
        Dim dcPrecioF As Decimal
        Dim strConsultaAprobaciones As String =
            " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap with(nolock) inner join [@SCGD_CONF_SUCURSAL] as csu on csu.DocEntry = cap.DocEntry " & _
            " where csu.U_Sucurs in ( select U_SCGD_idSucursal from [OQUT] with(nolock) where U_SCGD_Numero_OT = '{0}') " & _
            " and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] with(nolock) where U_SCGD_Numero_OT = '{0}')"
        Dim strNoOT As String = String.Empty

        Try
            'If Validacion Then ValidaPrecios(dtSeleccionados, BubbleEvent)

            'If Validacion Then Exit Try
            oForm = ApplicationSBO.Forms.Item("SCGD_AROT")
            dtRepuestos = oForm.DataSources.DataTables.Item(strDTRepuestos)
            oMatrix = DirectCast(oForm.Items.Item("mtxRep").Specific, Matrix)
            Posicion = dtRepuestos.Rows.Count

            oEditText = DirectCast(oForm.Items.Item("txtNoOrden").Specific, EditText)
            strNoOT = oEditText.Value.Trim()

            CargaValoresUDF(oForm)

            If Utilitarios.EjecutarConsulta(
                String.Format(strConsultaAprobaciones, strNoOT),
                CompanySBO.CompanyDB,
                CompanySBO.Server).Trim() = "Y" Then

                strAprobacion = EstadosAprobacion.Aprobado
            Else
                strAprobacion = EstadosAprobacion.FaltoAprobacion
            End If

            For i As Integer = 0 To dtSeleccionados.Rows.Count - 1

                dtRepuestos.Rows.Add(1)

                If Not String.IsNullOrEmpty(dtSeleccionados.GetValue("can", i)) Then
                    dcCantidad = Decimal.Parse(dtSeleccionados.GetValue("can", i))
                Else
                    dcCantidad = 0
                End If
                If Not String.IsNullOrEmpty(dtSeleccionados.GetValue("pre", i)) Then
                    dcPrecioF = Decimal.Parse(dtSeleccionados.GetValue("pre", i))
                Else
                    dcPrecioF = 0
                End If

                dtRepuestos.SetValue("per", Posicion, My.Resources.Resource.No)
                dtRepuestos.SetValue("cod", Posicion, dtSeleccionados.GetValue("cod", i))
                dtRepuestos.SetValue("des", Posicion, dtSeleccionados.GetValue("des", i))
                dtRepuestos.SetValue("can", Posicion, dcCantidad.ToString(n))
                dtRepuestos.SetValue("mon", Posicion, dtSeleccionados.GetValue("mon", i))
                dtRepuestos.SetValue("pre", Posicion, dcPrecioF.ToString(n))
                dtRepuestos.SetValue("apr", Posicion, hsAprobado(strAprobacion))
                dtRepuestos.SetValue("tra", Posicion, hsTrasladado("0"))
                ''dtRepuestos.SetValue("CodBar", Posicion, dtSeleccionados.GetValue("CodBar", i))

                Posicion += 1
            Next

            oForm.Items.Item("btnAct").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Function ValidaCambios() As Boolean
        Dim oMatrix As Matrix
        Dim oForm As Form
        Dim Posicion As Integer = 0

        Try
            oForm = ApplicationSBO.Forms.Item("SCGD_AROT")
            dtRepuestos = oForm.DataSources.DataTables.Item(strDTRepuestos)
            oMatrix = DirectCast(oForm.Items.Item("mtxRep").Specific, Matrix)

            For i As Integer = 0 To dtRepuestos.Rows.Count - 1

                If dtRepuestos.GetValue("per", i) = My.Resources.Resource.No Then
                    FormularioSBO.Items.Item("btnAct").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    Return True
                End If

            Next

            'FormularioSBO.Items.Item("btnAct").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            Return False
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Sub ActualizaCotizacion(ByVal FormUID As String, Optional ByVal p_boolDesaprobar As Boolean = False)

        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
        Dim m_intDocEntry As Integer = 0
        Dim oForm As Form
        Dim intError As Integer
        Dim strMensaje As String = String.Empty
        Dim oMatrix As IMatrix
        Dim strAprobacion As String = String.Empty
        Dim strImpuestosRepuestos As String
        Dim strShipToCode As String
        Dim Asesor As String = String.Empty

        Dim strConsultaAprobaciones As String =
        " select U_ItmAprob from [@SCGD_CONF_APROBAC] as cap with(nolock) inner join [@SCGD_CONF_SUCURSAL] as csu on csu.DocEntry = cap.DocEntry " & _
        " where csu.U_Sucurs in ( select U_SCGD_idSucursal from [OQUT] with(nolock) where U_SCGD_Numero_OT = '{0}') " & _
        " and cap.U_TipoOT in ( select U_SCGD_Tipo_OT from [OQUT] with(nolock) where U_SCGD_Numero_OT = '{0}')"

        Dim m_strNoOrden As String
        Dim strCadenaConexionBDTaller As String = String.Empty

        Dim m_strDocEntrysTransfRepuestos As String

        Dim m_dstRepuestosAdicionalesxOrden As RepuestosxOrdenDataset
        Dim m_adpRepuestosAdicionalesxOrden As RepuestosxOrdenDataAdapter

        Dim m_strIDSerieDocTrasnf As String = String.Empty
        Dim m_strDocEntrysTransfREP As String = String.Empty
        Dim m_strDocEntrysTransfSum As String = String.Empty
        Dim m_intLineNumAdded As Integer = 0
        Dim blnProcesoLineas As Boolean = False

        Dim m_blnConf_TallerEnSAP As Boolean
        Dim ErrorCode As Integer = 0
        Dim ErrorMessage As String = String.Empty

        Try
            m_blnConf_TallerEnSAP = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
            m_lstRepuestos.Clear()
            m_lstSuministros.Clear()
            m_lstServiociosEX.Clear()
            m_lstItemsEliminarRepuestos.Clear()
            m_lstItemsEliminarSuministros.Clear()
            m_lstItemACambiarEstado.Clear()
            m_lstItemACambiarEstadoAdicional.Clear()

            oForm = ApplicationSBO.Forms.Item(FormUID)
            dtLocal = oForm.DataSources.DataTables.Item("tLocal")
            oMatrix = DirectCast(oForm.Items.Item("mtxRep").Specific, IMatrix)
            oMatrix.FlushToDataSource()

            If Not String.IsNullOrEmpty(txtDocE.ObtieneValorUserDataSource) Then
                m_intDocEntry = Integer.Parse(txtDocE.ObtieneValorUserDataSource())

                m_strNoOrden = txtNoOrden.ObtieneValorUserDataSource().Trim()
                oCotizacion = CargaObjetoCotizacion(m_intDocEntry)

                If Not m_blnConf_TallerEnSAP Then

                    m_dstRepuestosxOrden = New RepuestosxOrdenDataset

                    Utilitarios.DevuelveCadenaConexionBDTaller(ApplicationSBO, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value, strCadenaConexionBDTaller)

                    m_adpRepuestosxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)
                    adpConf = New ConfiguracionDataAdapter(strCadenaConexionBDTaller)

                    adpConf.Fill(dstConf)
                    'Pasar para arriba
                    m_dstRepuestosAdicionalesxOrden = New RepuestosxOrdenDataset
                    m_adpRepuestosAdicionalesxOrden = New RepuestosxOrdenDataAdapter(strCadenaConexionBDTaller)

                    dtbRepuestosxOrden = Nothing
                    dtbRepuestosxOrden = New RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable


                    m_adpRepuestosAdicionalesxOrden.FillRepuestosxOrdenAdicionales(m_dstRepuestosAdicionalesxOrden, m_strNoOrden)

                End If
                Dim query As String = String.Format("select U_Imp_RepVenta, U_SerInv from [@SCGD_CONF_SUCURSAL] with(nolock) where U_Sucurs = '{0}'", oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value)

                dtLocal.ExecuteQuery(query)
                If (dtLocal.Rows.Count > 0) Then
                    strImpuestosRepuestos = dtLocal.GetValue("U_Imp_RepVenta", 0)
                    m_strIDSerieDocTrasnf = dtLocal.GetValue("U_SerInv", 0)
                End If

                strShipToCode = Utilitarios.EjecutarConsulta(String.Format("select ShipToDef from OCRD as crd with(nolock) inner join OQUT as qut with(nolock) on crd.CardCode = qut.CardCode where qut.DocEntry = '{0}'", oCotizacion.DocEntry),
                                                                        CompanySBO.CompanyDB,
                                                                        CompanySBO.Server).Trim()

                If Not oCotizacion Is Nothing Then
                    Dim cantidad As Integer
                    oLineasCotizacion = oCotizacion.Lines
                    If Not p_boolDesaprobar Then
                        If Utilitarios.EjecutarConsulta(String.Format(strConsultaAprobaciones, m_strNoOrden), CompanySBO.CompanyDB, CompanySBO.Server).Trim() = "Y" Then
                            strAprobacion = "1"
                        Else
                            strAprobacion = "3"
                        End If
                        For m As Integer = 0 To dtRepuestos.Rows.Count - 1
                            If dtRepuestos.GetValue("per", m) = My.Resources.Resource.No Then
                                If Not String.IsNullOrEmpty(oLineasCotizacion.ItemCode) Then
                                    oLineasCotizacion.Add()
                                End If
                                If m_intLineNumAdded = 0 Then
                                    m_intLineNumAdded = oLineasCotizacion.LineNum
                                End If
                                oLineasCotizacion.ItemCode = dtRepuestos.GetValue("cod", m)
                                oLineasCotizacion.ItemDescription = dtRepuestos.GetValue("des", m)
                                oLineasCotizacion.Quantity = Double.Parse(dtRepuestos.GetValue("can", m))
                                oLineasCotizacion.UnitPrice = Double.Parse(dtRepuestos.GetValue("pre", m).ToString())
                                oLineasCotizacion.Currency = dtRepuestos.GetValue("mon", m)
                                If (DMS_Connector.Configuracion.ParamGenAddon.U_LocCR <> "Y") Then
                                    oLineasCotizacion.TaxCode = strImpuestosRepuestos
                                    oLineasCotizacion.VatGroup = strImpuestosRepuestos
                                End If
                                oLineasCotizacion.DiscountPercent = 0
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = strAprobacion
                                oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                oLineasCotizacion.ShipToCode = strShipToCode
                            End If
                        Next

                        cantidad = oCotizacion.Lines.Count
                        blnProcesoLineas = False
                        If ProcesaLineasCotizacion(oCotizacion, strCadenaConexionBDTaller, m_blnConf_TallerEnSAP, m_intLineNumAdded) Then
                            blnProcesoLineas = True
                        End If

                        If Not m_blnConf_TallerEnSAP Then

                            ActualizarListasRep(m_lstCantidadesAnteriores, dtbRepuestosxOrden)
                            g_cnnSCGTaller = New SqlClient.SqlConnection(strCadenaConexionBDTaller)

                            If g_blnDraft Then
                                g_blnModificaItemsAdicionales = True
                                m_adpRepuestosxOrden.UpdateDraft(dtbRepuestosxOrden, g_cnnSCGTaller, g_trnTransaccion, True)
                                m_adpRepuestosxOrden.UpdateDraft(m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, g_cnnSCGTaller, g_trnTransaccion, False, False, True)
                            Else
                                g_blnModificaItemsAdicionales = False
                                m_adpRepuestosxOrden.Update(dtbRepuestosxOrden, g_cnnSCGTaller, g_trnTransaccion, True)
                                m_adpRepuestosxOrden.Update(m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden, g_cnnSCGTaller, g_trnTransaccion, False, False, True)
                            End If

                            If g_blnModificaItemsAdicionales Then
                                Call ActualizarEstadoRepuestosDesdeCotizacion(oCotizacion, m_dstRepuestosAdicionalesxOrden, g_trnTransaccion)
                            End If

                            Call AsignarIDsLineas(True, oCotizacion)
                        End If
                    Else
                        Dim intPosCot As Integer = 0
                        Dim strstr As String
                        Dim strCln As String
                        Dim strApr As String
                        Dim strTrs As String
                        Dim strCentroCosto As String
                        Dim strIdSucursal As String = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()

                        For m As Integer = 0 To dtRepuestos.Rows.Count - 1

                            If (dtRepuestos.GetValue("sel", m) = "Y" AndAlso dtRepuestos.GetValue("per", m) = My.Resources.Resource.Si) Then
                                For i As Integer = intPosCot To oLineasCotizacion.Count - 1

                                    oLineasCotizacion.SetCurrentLine(i)
                                    If dtRepuestos.GetValue("ln", m) = oLineasCotizacion.LineNum.ToString() Then
                                        strCentroCosto = DevuelveValorItem(oLineasCotizacion.ItemCode, g_strCodCentroCosto)
                                        m_strNoBodegaRepu = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaRepuestos, strIdSucursal, ApplicationSBO)
                                        m_strNoBodegaProceso = Utilitarios.GetBodegaXCentroCosto(strCentroCosto, TransferenciaItems.mc_strBodegaProceso, strIdSucursal, ApplicationSBO)
                                        strstr = oLineasCotizacion.LineNum.ToString()
                                        strCln = dtRepuestos.GetValue("ln", m)
                                        strTrs = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value.ToString.Trim()
                                        strApr = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value

                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = "2"
                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = "0"

                                        Dim udtLineaTransf As TransferenciaItems.LineasTransferenciaStock = Nothing

                                        udtLineaTransf.strItemCode = oLineasCotizacion.ItemCode
                                        udtLineaTransf.strItemDescription = oLineasCotizacion.ItemDescription
                                        udtLineaTransf.decCantidad = oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value
                                        udtLineaTransf.strNoBodegaDest = m_strNoBodegaRepu
                                        udtLineaTransf.strNoBodegaOrig = m_strNoBodegaProceso
                                        udtLineaTransf.intTipoArticulo = 1
                                        udtLineaTransf.intIDColaborador = IIf(IsNumeric(oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value), oLineasCotizacion.UserFields.Fields.Item(mc_strEmpRealiza).Value, 0)
                                        udtLineaTransf.strNombreMecanico = oLineasCotizacion.UserFields.Fields.Item(mc_strNombEmpleado).Value
                                        If (strTrs = "4") Then
                                            udtLineaTransf.intReqOriPen = 2
                                        Else
                                            udtLineaTransf.intReqOriPen = 1
                                        End If
                                        m_lstItemsEliminarRepuestos.Add(udtLineaTransf)

                                        If g_blnDraft Then
                                            oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CPDe").Value += oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value
                                        End If

                                        oLineasCotizacion.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                        intPosCot = i + 1
                                        g_intEstCotizacion = CotizacionEstado.modificada
                                        Exit For
                                    End If
                                Next
                            End If

                        Next
                    End If

                    ObtieneDatosCotizacion(oCotizacion)
                    If Not m_blnConf_TallerEnSAP Then
                        ConfiguracionDataAdapter.DevuelveValorDeParametosConfiguracion(dstConf.SCGTA_TB_Configuracion, TransferenciaItems.mc_strIDSerieDocumentosTraslado, m_strIDSerieDocTrasnf)
                    End If

                    objTransferenciaStock.intCodigoCotizacion = m_intDocEntry

                    CompanySBO.StartTransaction()

                    m_strDocEntrysTransfRepuestos =
                       objTransferenciaStock.CrearTrasladoAddOnNuevo(m_lstRepuestos, m_lstSuministros, m_lstServiociosEX, m_lstItemsEliminarRepuestos, m_lstItemsEliminarSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional,
                                                                     m_strNoOrden, g_strBodegaRepuestos, String.Empty, String.Empty, g_strBodegaProcesoRepuestos,
                                                                     m_strIDSerieDocTrasnf, g_cnnSCGTaller, g_trnTransaccion, True, m_strDocEntrysTransfREP,
                                                                     m_strDocEntrysTransfSum, String.Empty, gcol_strDescMarca, g_strDescEstilo,
                                                                     g_strDescModelo, g_strPlaca, g_strVIN, g_strEmpleadoRecibe, g_strCodigoCliente,
                                                                     False, False, oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value)


                    cantidad = oCotizacion.Lines.Count

                    If Not String.IsNullOrEmpty(m_strDocEntrysTransfREP) Or Not String.IsNullOrEmpty(m_strDocEntrysTransfSum) Then
                        If SCG.Requisiciones.TransferenciasDirectas.PermiteTransferenciasDirectas(oCotizacion) Then
                            CrearTransferenciasDirectas(m_strDocEntrysTransfREP, m_strDocEntrysTransfSum, ErrorCode, ErrorMessage)

                            If ErrorCode <> 0 Then
                                Throw New ExceptionsSBO(ErrorCode, ErrorMessage)
                            End If

                            SCG.Requisiciones.TransferenciasDirectas.AjustarPendientesRequisicion(oCotizacion, False, ErrorCode, ErrorMessage)

                            If ErrorCode <> 0 Then
                                Throw New ExceptionsSBO(ErrorCode, ErrorMessage)
                            End If
                        End If
                    End If

                    If oCotizacion.Update() <> 0 Then
                        CompanySBO.GetLastError(intError, strMensaje)
                        If intError <> 0 Then
                            Throw New ExceptionsSBO(intError, strMensaje)
                        End If
                    Else
                        blnProcesoLineas = True
                        dtRepuestos = FormularioSBO.DataSources.DataTables.Item(strDTRepuestos)
                        dtRepuestos.Rows.Clear()
                    End If

                End If

                If CompanySBO.InTransaction Then
                    CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If

                If g_trnTransaccion IsNot Nothing Then

                    If g_trnTransaccion.Connection IsNot Nothing Then
                        g_trnTransaccion.Commit()
                    End If
                    g_trnTransaccion = Nothing
                End If



                If blnProcesoLineas Then

                    If g_intEstCotizacion = CotizacionEstado.modificada Or g_intEstCotizacion = CotizacionEstado.creada Or m_strDocEntrysTransfREP <> "" Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.EnviandoAlertas, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        If (String.IsNullOrEmpty(m_strDocEntrysTransfREP)) Then
                            m_strDocEntrysTransfREP = m_strDocEntrysTransfRepuestos
                        End If
                        Dim strIdSuc As String = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()
                        Asesor = Utilitarios.EjecutarConsulta(" Select T1.firstName + ' ' + T1.lastName  From [OQUT] T0 ,[OHEM] T1 Where T0.[OwnerCode] = T1.[empID] and T0.U_SCGD_Numero_OT = '" + m_strNoOrden + "' ", CompanySBO.CompanyDB, CompanySBO.Server)
                        EnviarMensaje(m_strDocEntrysTransfREP, g_intEstCotizacion, g_strNoVisita, g_strNoOrden, m_intDocEntry, oForm, m_blnConf_TallerEnSAP, strIdSuc, m_strDocEntrysTransfSum, g_blnDraft, Asesor)

                    End If

                    If oCotizacion.UserFields.Fields.Item(g_strImprimirOT).Value <> ImprimirOT.scgSi Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                    End If

                End If
                CargaRepuestos(oCotizacion)
            End If
            ValidaCambios()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            If g_trnTransaccion IsNot Nothing Then
                g_trnTransaccion.Rollback()
            End If
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
        Finally
            If Not oCotizacion Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oCotizacion)
                oCotizacion = Nothing
            End If
            If g_cnnSCGTaller IsNot Nothing Then
                g_cnnSCGTaller.Close()
            End If
            g_cnnSCGTaller = Nothing
            g_trnTransaccion = Nothing
        End Try
    End Sub

    Private Sub CrearTransferenciasDirectas(ByVal DocEntryRequisicionRepuestos As String, ByVal DocEntryRequisicionSuministros As String, ByRef ErrorCode As Integer, ByRef ErrorMessage As String)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim RequisicionRepuestos As SAPbobsCOM.GeneralData
        Dim RequisicionSuministros As SAPbobsCOM.GeneralData
        Try

            oCompanyService = DMS_Connector.Company.CompanySBO.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)

            If Not String.IsNullOrEmpty(DocEntryRequisicionRepuestos) Then
                oGeneralParams.SetProperty("DocEntry", DocEntryRequisicionRepuestos)
                RequisicionRepuestos = oGeneralService.GetByParams(oGeneralParams)
                SCG.Requisiciones.TransferenciasDirectas.CrearTransferencia(RequisicionRepuestos, ErrorCode, ErrorMessage)
            End If

            If Not String.IsNullOrEmpty(DocEntryRequisicionSuministros) Then
                oGeneralParams.SetProperty("DocEntry", DocEntryRequisicionSuministros)
                RequisicionSuministros = oGeneralService.GetByParams(oGeneralParams)
                SCG.Requisiciones.TransferenciasDirectas.CrearTransferencia(RequisicionSuministros, ErrorCode, ErrorMessage)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorCode = 69784
            ErrorMessage = ex.Message
        End Try
    End Sub

    Private Sub EnviarMensaje(ByVal p_strDocEntryTrasfRep As String, _
                              ByVal p_intEstCotizacion As Integer, _
                              ByVal p_strNumeroVisita As String, _
                              ByVal p_strNoOrden As String,
                              ByVal p_intDocEntry As String,
                              ByVal p_oForm As SAPbouiCOM.Form, _
                              ByVal p_blnConf_TallerEnSAP As Boolean, _
                              ByVal strIdSucursal As String, ByVal p_strDocEntryTrasfSum As String, ByVal p_blnDraft As Boolean, Optional ByVal Asesor As String = "")

        Dim clsUtilitarios As New Utilitarios

        Utilitarios.CargarCulturaActual()
        g_blnDraft = p_blnDraft

        Try
            Dim clsMensajeria As New MensajeriaCls(ApplicationSBO, CompanySBO)
            'Envia mensaje al encargado de Taller para avisar de una creación o actualización de la cotización
            If Not p_blnConf_TallerEnSAP Then
                If ((Not String.IsNullOrEmpty(g_strNum_OT)) AndAlso (Not p_strNumeroVisita Is Nothing)) Then
                    If p_intEstCotizacion = CotizacionEstado.creada Then
                        clsMensajeria.CreaMensajeSBO_DMS(My.Resources.Resource.MensajeCotizacionCreada, p_strNoOrden, p_intDocEntry, -1, 0, p_strNumeroVisita)
                    ElseIf p_intEstCotizacion = CotizacionEstado.modificada Then
                        clsMensajeria.CreaMensajeSBO_DMS(My.Resources.Resource.MensajeCotizacionActualizada, p_strNoOrden, p_intDocEntry, -1, 0, p_strNumeroVisita)
                    End If
                End If
            Else
                If ((Not String.IsNullOrEmpty(g_strNum_OT)) AndAlso (Not p_strNumeroVisita Is Nothing)) Then

                    If p_intEstCotizacion = CotizacionEstado.creada Then
                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeCotizacionCreada, String.Empty, p_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, g_blnDraft, p_oForm, strDTLocal, strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion), True, p_blnConf_TallerEnSAP, Asesor)
                    ElseIf p_intEstCotizacion = CotizacionEstado.modificada Then
                        clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeCotizacionActualizada, String.Empty, p_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos, g_blnDraft, p_oForm, strDTLocal, strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoProduccion), False, p_blnConf_TallerEnSAP, Asesor)
                    End If
                End If
            End If

            If p_strDocEntryTrasfRep <> "" Then
                If Not p_blnConf_TallerEnSAP Then
                    clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfRep, p_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos,
                                                              g_blnDraft, p_oForm, strDTLocal, strIdSucursal, -1, False, p_blnConf_TallerEnSAP, Asesor)
                Else
                    clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfRep, p_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionRepuestos,
                                                              g_blnDraft, p_oForm, strDTLocal, strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoRepuestos), False, p_blnConf_TallerEnSAP, Asesor)
                End If
            End If

            If p_strDocEntryTrasfSum <> "" Then
                If Not p_blnConf_TallerEnSAP Then
                    clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfSum, p_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionSuministros,
                                                              g_blnDraft, p_oForm, strDTLocal, strIdSucursal, -1, False, p_blnConf_TallerEnSAP, Asesor)
                Else
                    'clsMensajeria.CreaMensajeSBO(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfRep, CompanySBO, p_strNoOrden, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoBodega).ToString, g_blnDraft, strIdSucursal,p_oForm, strDTLocal, False);

                    clsMensajeria.CreaMensajeSBO_SBOCotizacion(My.Resources.Resource.MensajeTraslado, p_strDocEntryTrasfSum, p_strNoOrden, MensajeriaSBOTallerDataAdapter.TipoMensaje.scgPeticionSuministros,
                                                              g_blnDraft, p_oForm, strDTLocal, strIdSucursal, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSuministros), False, p_blnConf_TallerEnSAP, Asesor)
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Sub

    Private Sub ObtieneDatosCotizacion(ByVal p_oCotizacion As Documents)
        g_strEmpleadoRecibe = IIf(IsNumeric(p_oCotizacion.DocumentsOwner), p_oCotizacion.DocumentsOwner, "")
        g_strCodigoCliente = p_oCotizacion.UserFields.Fields.Item(gcol_strCardCode).Value
        g_strPlaca = p_oCotizacion.UserFields.Fields.Item(gcol_strPlaca).Value
        g_strVIN = p_oCotizacion.UserFields.Fields.Item(gcol_strVIN).Value
        g_strDescMarca = p_oCotizacion.UserFields.Fields.Item(gcol_strDescMarca).Value
        g_strDescModelo = p_oCotizacion.UserFields.Fields.Item(gcol_strDescModelo).Value
        g_strDescEstilo = p_oCotizacion.UserFields.Fields.Item(gcol_strDesc_Estilo).Value
        g_strNoVisita = p_oCotizacion.UserFields.Fields.Item(gcol_strNum_Visita).Value
        g_strNoOrden = p_oCotizacion.UserFields.Fields.Item(g_strNum_OT).Value
    End Sub

    Private Sub AsignarIDsLineas(ByVal p_blnEsActualizacion As Boolean, ByRef p_oCotizacion As Documents)
        Dim visOrder As Integer
        Dim cadenaConexion As String = String.Empty
        Dim nombreTabla As String = "QUT1"

        Dim oItem As SAPbobsCOM.Items
        Dim intTipoArticulo As Integer
        Dim blnItemNuevo As Boolean = False

        Dim intIdrepXOrd As Integer
        Dim strIdrepXOrd As String = String.Empty

        'dr de Articulos
        Dim drAdentroArt As System.Data.DataRowCollection
        Dim drAfueraArt As System.Data.DataRowCollection

        Try
            drAdentroArt = dtbRepuestosxOrden.Copy.Rows
            drAfueraArt = m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Copy.Rows

            For index As Integer = 0 To p_oCotizacion.Lines.Count - 1
                p_oCotizacion.Lines.SetCurrentLine(index)
                intIdrepXOrd = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                strIdrepXOrd = Convert.ToString(intIdrepXOrd)

                If String.IsNullOrEmpty(strIdrepXOrd) OrElse strIdrepXOrd = 0 Then
                    For Each m_drwRepuestos In drAdentroArt
                        If p_oCotizacion.Lines.LineNum = m_drwRepuestos.LineNum Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_drwRepuestos.ID
                            drAdentroArt.Remove(m_drwRepuestos)
                            blnItemNuevo = True
                            Exit For
                        End If
                    Next
                End If

                If Not blnItemNuevo Then
                    For Each m_drwRepuestos In drAfueraArt
                        If p_oCotizacion.Lines.LineNum = m_drwRepuestos.LineNum Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_drwRepuestos.ID
                            drAfueraArt.Remove(m_drwRepuestos)
                            Exit For
                        End If
                    Next
                End If
            Next

            'Items que se van a borrar -- ARTICULOS
            If drAfueraArt.Count > 0 Then
                For Each m_drwRepuestos In drAfueraArt
                    objItemLineNumErroneo.NoOrden = m_drwRepuestos.NoOrden
                    objItemLineNumErroneo.IdItem = m_drwRepuestos.NoRepuesto
                    objItemLineNumErroneo.Id = m_drwRepuestos.ID
                    objItemLineNumErroneo.intLineNum = m_drwRepuestos.LineNum
                    objItemLineNumErroneo.TipoRow = enumTipoRow.scgRepuestoRow
                    objItemsLineasLineNumErroneos.Add(objItemLineNumErroneo)
                Next
            End If


            Dim strIdSucursal As String
            strIdSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString.Trim

            If objItemsLineasLineNumErroneos.Count <> 0 Then
                Call EliminarRegistroLineNumErroneo(objItemsLineasLineNumErroneos, strIdSucursal)
                objItemsLineasLineNumErroneos.Clear()
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


    Private Sub ActualizarEstadoRepuestosDesdeCotizacion(ByVal p_oCotizacion As Documents,
                                                         ByRef m_dstRepuestosAdicionalesxOrden As RepuestosxOrdenDataset, _
                                                         Optional ByRef tran As SqlClient.SqlTransaction = Nothing)

        Dim intDocEntryCotizacion As Integer
        Dim oLineasCotizacion As SAPbobsCOM.Document_Lines
        Dim ListaLineNumPB As New Generic.List(Of Integer)
        Dim strNumeroOT As String = String.Empty

        intDocEntryCotizacion = p_oCotizacion.DocEntry
        strNumeroOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value

        oLineasCotizacion = p_oCotizacion.Lines
        If m_dstRepuestosAdicionalesxOrden.SCGTA_TB_RepuestosxOrden.Rows.Count > 0 Then
            For i As Integer = 0 To oLineasCotizacion.Count - 1

                oLineasCotizacion.SetCurrentLine(i)
                If oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 3 And oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1 Then
                    For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosAdicionalesxOrden.SCGTA_TB_RepuestosxOrden.Rows

                        If oLineasCotizacion.LineNum = drwRep.LineNum Then
                            drwRep.CodEstadoRep = 5
                            Exit For
                        End If
                    Next
                ElseIf oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Traslad").Value = 4 And oLineasCotizacion.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 1 Then
                    For Each drwRep As DMSOneFramework.RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow In m_dstRepuestosAdicionalesxOrden.SCGTA_TB_RepuestosxOrden.Rows

                        If oLineasCotizacion.LineNum = drwRep.LineNum Then
                            drwRep.CodEstadoRep = 6
                            Exit For
                        End If
                    Next
                End If

            Next
            m_adpRepuestosxOrden.UpdateCodigoRepuesto(m_dstRepuestosAdicionalesxOrden, tran)

        End If

    End Sub

    Private Sub ActualizarListasRep(ByRef p_lstCantLineasAnt As Generic.List(Of stTipoListaCantAnteriores), ByRef p_dtbRepXOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenDataTable)
        Dim intContCant As Integer
        Dim drwRepXOrden As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

        For intContCant = 0 To p_lstCantLineasAnt.Count - 1

            For Each drwRepXOrden In p_dtbRepXOrden

                If drwRepXOrden.LineNum = p_lstCantLineasAnt(intContCant).LineNum AndAlso _
                    drwRepXOrden.NoRepuesto = p_lstCantLineasAnt(intContCant).ItemCode Then

                    If drwRepXOrden.IsCantidadLineasAnteNull Then
                        drwRepXOrden.CantidadLineasAnte = p_lstCantLineasAnt(intContCant).Cantidad
                    Else
                        drwRepXOrden.CantidadLineasAnte += p_lstCantLineasAnt(intContCant).Cantidad
                    End If

                    Exit For

                End If
            Next
        Next
        'p_lstCantLineasAnt.Clear()
    End Sub

    Private Function ProcesaLineasCotizacion(ByRef p_oCotizacion As Documents, ByVal p_strCadenaConexionBDTaller As String,
                                             ByVal p_blnConf_TallerEnSAP As Boolean, ByRef p_intLineNumAdded As Integer) As Boolean

        Dim m_strTipoArticulo As String = String.Empty
        Dim m_intTipoArticulo As Integer
        Dim m_blnConfigArticulo As Boolean = False
        Dim m_strNombreTaller As String = String.Empty
        Dim m_strCentroCosto As String = String.Empty
        Dim m_strNoOrden As String = String.Empty
        Dim m_intEstadoItem As Integer
        Dim m_strGenerico As String = String.Empty
        Dim m_intGenerico As Integer
        Dim m_decCantidadItem As Decimal
        Dim m_intEstadoTraslado As String = String.Empty
        Dim m_blmRechazarItem As Boolean
        Dim m_strEstadoTraslado As String = String.Empty
        Dim m_blnEsLineaNueva As Boolean
        Dim m_blnProcesarSi As Boolean = False
        Dim m_blnProcesarNo As Boolean = False
        Dim m_intTrasladoLineaCot As String = String.Empty

        Dim m_strDraft As String = "N"
        Dim m_strSucursal As String = String.Empty

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim strID As String

        Try

            m_strSucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

            Utilitarios.DevuelveNombreBDTaller(ApplicationSBO, m_strSucursal, m_strNombreTaller)
            m_strNoOrden = p_oCotizacion.UserFields.Fields.Item(g_strNum_OT).Value

            m_strDraft = Utilitarios.EjecutarConsulta(String.Format(" select U_Requis from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}' ", m_strSucursal), CompanySBO.CompanyDB, CompanySBO.Server)

            If Not String.IsNullOrEmpty(m_strDraft) Then
                If m_strDraft = "Y" Then
                    g_blnDraft = True
                Else
                    g_blnDraft = False
                End If
            End If

            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ActulizarLineasOT, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

            For i As Integer = p_oCotizacion.Lines.Count - 1 To p_intLineNumAdded Step -1

                p_oCotizacion.Lines.SetCurrentLine(i)

                oItemArticulo = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                oItemArticulo.GetByKey(p_oCotizacion.Lines.ItemCode)

                'Tipo Articulo
                m_strTipoArticulo = oItemArticulo.UserFields.Fields.Item(g_strTipoArticulo).Value
                m_intTipoArticulo = IIf(IsNumeric(m_strTipoArticulo), CInt(m_strTipoArticulo), -1)

                'Generico
                m_strGenerico = oItemArticulo.UserFields.Fields.Item(g_strGenerico).Value
                m_intGenerico = IIf(IsNumeric(m_strGenerico), CInt(m_strGenerico), 0)

                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ProcesandoItem & (p_oCotizacion.Lines.LineNum + 1) & My.Resources.Resource.Separador & p_oCotizacion.Lines.ItemDescription, SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                m_strEstadoTraslado = p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value
                m_intEstadoItem = p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAprobado).Value

                If (m_intTipoArticulo = TiposArticulos.scgRepuesto) AndAlso (m_strEstadoTraslado = EstadosTraslados.NoProcesado Or m_strEstadoTraslado = EstadosTraslados.PendienteTraslado) Then
                    Select Case m_intTipoArticulo
                        Case TiposArticulos.scgRepuesto
                            m_blnConfigArticulo = ValidaConfiguracionArticulo(p_oCotizacion.Lines.ItemCode, BoYesNoEnum.tYES, BoYesNoEnum.tYES, BoYesNoEnum.tYES, True, m_strCentroCosto, m_strSucursal, p_blnConf_TallerEnSAP)
                    End Select


                    If m_blnConfigArticulo Then

                        Select Case m_intTipoArticulo
                            Case CInt(TipoArticulo.Repuesto)
                                g_strBodegaRepuestos = Utilitarios.GetBodegaXCentroCosto(m_strCentroCosto, TransferenciaItems.mc_strBodegaRepuestos, m_strSucursal, ApplicationSBO)
                                g_strBodegaProcesoRepuestos = Utilitarios.GetBodegaXCentroCosto(m_strCentroCosto, TransferenciaItems.mc_strBodegaProceso, m_strSucursal, ApplicationSBO)
                            Case CInt(TipoArticulo.Suministro)
                                g_strBodegaRepuestos = Utilitarios.GetBodegaXCentroCosto(m_strCentroCosto, TransferenciaItems.mc_strBodegaSuministros, m_strSucursal, ApplicationSBO)
                                g_strBodegaProcesoRepuestos = Utilitarios.GetBodegaXCentroCosto(m_strCentroCosto, TransferenciaItems.mc_strBodegaProceso, m_strSucursal, ApplicationSBO)
                        End Select


                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = m_strNoOrden

                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = m_strTipoArticulo
                        If String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = String.Format("{0}-{1}-{2}", m_strSucursal, p_oCotizacion.Lines.LineNum, m_strNoOrden)
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = m_strSucursal
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = m_strCentroCosto

                            m_blmRechazarItem = False

                            If (m_intTipoArticulo = TiposArticulos.scgRepuesto) AndAlso (m_strEstadoTraslado = EstadosTraslados.NoProcesado) Or (m_strEstadoTraslado = EstadosTraslados.PendienteTraslado) Then

                                RevisaStockArticulo(p_oCotizacion.Lines, p_oCotizacion.DocEntry, g_strBodegaRepuestos, m_intTipoArticulo, m_intGenerico, m_decCantidadItem, m_intEstadoTraslado, m_blmRechazarItem, False, p_strCadenaConexionBDTaller, p_blnConf_TallerEnSAP)

                                If Not String.IsNullOrEmpty(m_intEstadoTraslado) Then

                                    If (m_intEstadoTraslado <> ResultadoValidacionPorItem.scgModQtyCoti AndAlso m_intEstadoTraslado <> ResultadoValidacionPorItem.scgComprar AndAlso p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value = EstadosTraslados.NoProcesado) AndAlso m_intTipoArticulo = TiposArticulos.scgRepuesto Then
                                        If g_blnDraft And m_intEstadoTraslado = ResultadoValidacionPorItem.scgPendBodega Then
                                            p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value = EstadosTraslados.PendienteBodega
                                        Else
                                            p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value = m_intEstadoTraslado
                                        End If

                                    ElseIf m_intEstadoTraslado = ResultadoValidacionPorItem.scgComprar Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y"
                                        p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value = EstadosTraslados.No
                                        p_oCotizacion.Lines.UserFields.Fields.Item(g_strResultado).Value = My.Resources.Resource.ParaComprar
                                    ElseIf m_intEstadoTraslado = EstadosTraslados.PendienteTraslado AndAlso m_intTipoArticulo = TiposArticulos.scgSuministro Then
                                        p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value = EstadosTraslados.PendienteTraslado
                                    End If
                                End If
                                If p_oCotizacion.Lines.Quantity <> m_decCantidadItem AndAlso m_decCantidadItem <> 0 Then
                                    p_oCotizacion.Lines.Quantity = m_decCantidadItem
                                End If

                                Select Case p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                    Case 1
                                        If p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = "Y" Then
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = m_decCantidadItem.ToString(n)
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                        Else
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = m_decCantidadItem.ToString(n)
                                        End If
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                    Case 3
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = m_decCantidadItem.ToString(n)
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = 0
                                    Case 4
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = 0
                                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = m_decCantidadItem.ToString(n)
                                End Select
                            End If
                        Else
                            m_blmRechazarItem = True
                        End If


                        If Not m_blmRechazarItem Then
                            m_blnEsLineaNueva = False
                            If (m_intEstadoItem = EstadosAprobacion.Aprobado) Then
                                m_blnEsLineaNueva = ActualizarOrdenTrabajoAgregar(p_oCotizacion.Lines, m_intTipoArticulo, m_strNoOrden, p_strCadenaConexionBDTaller, p_blnConf_TallerEnSAP)

                                m_intTrasladoLineaCot = p_oCotizacion.Lines.UserFields.Fields.Item(g_strTrasladado).Value

                                p_oCotizacion.Lines.WarehouseCode = g_strBodegaProcesoRepuestos

                                If (m_intTrasladoLineaCot = EstadosTraslados.NoProcesado.ToString() AndAlso m_intEstadoTraslado <> EstadosTraslados.No.ToString()) _
                                    Or m_intTrasladoLineaCot = EstadosTraslados.PendienteTraslado _
                                    Or m_intTrasladoLineaCot = EstadosTraslados.PendienteBodega Then
                                    Select Case m_intTipoArticulo

                                        Case TiposArticulos.scgRepuesto
                                            'Genera la Lista de los Repuestos que se van a trasladar
                                            objTransferenciaStock.GeneraLista(
                                                TransferenciaItems.scgTiposMovimientoXBodega.TransfRepuestos, m_lstRepuestos,
                                                p_oCotizacion.Lines, g_strBodegaRepuestos, Nothing, Nothing,
                                                g_strBodegaProcesoRepuestos, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, m_intTipoArticulo,
                                                Nothing, Nothing, m_intGenerico, False, g_blnDraft, FormularioSBO, 0, p_oCotizacion.DocEntry)
                                        Case TiposArticulos.scgSuministro
                                            objTransferenciaStock.GeneraLista(
                                                TransferenciaItems.scgTiposMovimientoXBodega.TransfSuministros, m_lstSuministros,
                                                p_oCotizacion.Lines, g_strBodegaRepuestos, Nothing, Nothing,
                                                g_strBodegaProcesoSuministros, m_lstItemACambiarEstado, m_lstItemACambiarEstadoAdicional, True, m_intTipoArticulo,
                                                Nothing, Nothing, m_intGenerico, False, g_blnDraft, FormularioSBO, 0, p_oCotizacion.DocEntry)
                                    End Select
                                End If
                            End If
                        Else
                            p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAprobado).Value = 2
                        End If

                        If p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAprobado).Value = EstadosAprobacion.Aprobado And p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                            m_blnProcesarSi = True
                        ElseIf p_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                            m_blnProcesarSi = False
                        End If

                        If p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAprobado).Value = EstadosAprobacion.NoAprobado And p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                            m_blnProcesarNo = True
                        ElseIf p_oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                            m_blnProcesarNo = False
                        End If

                        If m_blnProcesarSi = True Then
                            If p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAProcesar).Value = LineaAProcesar.scgSi
                            End If
                        ElseIf m_blnProcesarNo = True Then
                            If p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAprobado).Value = EstadosAprobacion.NoAprobado And p_oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                p_oCotizacion.Lines.UserFields.Fields.Item(g_strItemAProcesar).Value = LineaAProcesar.scgNo
                            End If
                        End If
                    Else
                        If m_intTipoArticulo = TiposArticulos.scgRepuesto Then
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.El_Item + p_oCotizacion.Lines.ItemDescription + My.Resources.Resource.MalConfigurado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            Return False
                        End If
                    End If
                End If
                Utilitarios.DestruirObjeto(oItemArticulo)
            Next
            Return True
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Private Function ActualizarOrdenTrabajoAgregar(ByRef p_oLineasCotizacion As SAPbobsCOM.Document_Lines,
                                                   ByVal p_intTipoArticulo As TiposArticulos,
                                                   ByVal p_strNoOrden As String,
                                                   ByVal p_strCadenaConexionBDTaller As String,
                                                   ByVal p_blnConf_TallerEnSAP As Boolean) As Boolean

        Dim decCantidad As Decimal
        Dim strItemCode As String
        Dim intLineNum As Integer
        Dim decDuracion As Decimal
        Dim intItemAprobado As Integer
        Dim intEstadoTransf As Integer
        Dim blnYaAgregada As Boolean = False
        Dim intIDEmpleado As Integer
        Dim drwLineaActualizada As dtsMovimientoStock.LineaActualizadaRow
        Dim objUtilitarios As New SCGDataAccess.Utilitarios(p_strCadenaConexionBDTaller)

        Dim blnLineaNueva As Boolean
        Dim strNameEspecifico As String
        Dim strCodeEspecifico As String

        Dim strIdRepXOrd As String

        Dim strDuracionActividad As String = String.Empty

        decCantidad = p_oLineasCotizacion.Quantity
        strItemCode = p_oLineasCotizacion.ItemCode
        intLineNum = p_oLineasCotizacion.LineNum

        intItemAprobado = p_oLineasCotizacion.UserFields.Fields.Item(g_strItemAprobado).Value
        intEstadoTransf = p_oLineasCotizacion.UserFields.Fields.Item(g_strTrasladado).Value
        strIdRepXOrd = p_oLineasCotizacion.UserFields.Fields.Item(g_strIdRepxOrd).Value

        strCodeEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(g_strCodeEspecifico).Value
        strNameEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(g_strNameEspecifico).Value

        If intItemAprobado = EstadosAprobacion.Aprobado Then
            If Not p_blnConf_TallerEnSAP Then
                If p_intTipoArticulo <> TiposArticulos.scgNinguno Then
                    Select Case p_intTipoArticulo

                        Case TiposArticulos.scgRepuesto

                            For Each m_drwRepuestos In m_dstRepuestosxOrden.SCGTA_TB_RepuestosxOrden.Rows
                                If m_drwRepuestos.RowState <> DataRowState.Deleted Then
                                    If m_drwRepuestos.NoRepuesto = strItemCode Then
                                        If p_oLineasCotizacion.LineNum = m_drwRepuestos.LineNum Then
                                            blnYaAgregada = True
                                            If p_oLineasCotizacion.Quantity <> m_drwRepuestos.Cantidad Then
                                                drwLineaActualizada = dtbLineasActualizadas.NewLineaActualizadaRow
                                                drwLineaActualizada.ItemCode = p_oLineasCotizacion.ItemCode
                                                drwLineaActualizada.ItemName = p_oLineasCotizacion.ItemDescription
                                                drwLineaActualizada.LineNum = p_oLineasCotizacion.LineNum
                                                drwLineaActualizada.Cantidad = p_oLineasCotizacion.Quantity - m_drwRepuestos.Cantidad
                                                dtbLineasActualizadas.AddLineaActualizadaRow(drwLineaActualizada)
                                                m_drwRepuestos.Cantidad = p_oLineasCotizacion.Quantity
                                                m_drwRepuestos.Trasladado = p_oLineasCotizacion.UserFields.Fields.Item(g_strTrasladado).Value
                                                If p_oLineasCotizacion.UserFields.Fields.Item(g_strCodeEspecifico).Value <> "" Then
                                                    m_drwRepuestos.ItemCodeEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(g_strCodeEspecifico).Value
                                                    If p_oLineasCotizacion.UserFields.Fields.Item(g_strNameEspecifico).Value <> "" Then
                                                        m_drwRepuestos.ItemNameEspecifico = p_oLineasCotizacion.UserFields.Fields.Item(g_strNameEspecifico).Value
                                                    End If
                                                End If
                                                g_intEstCotizacion = CotizacionEstado.modificada
                                            End If
                                            Exit For
                                        End If

                                    End If
                                End If
                            Next
                            If Not blnYaAgregada Then

                                Call AgregarRepuesto(strItemCode, decCantidad, intLineNum, intEstadoTransf, strCodeEspecifico, strNameEspecifico, TiposArticulos.scgRepuesto, p_strNoOrden, strIdRepXOrd)
                                g_intEstCotizacion = CotizacionEstado.modificada
                                blnLineaNueva = True

                            Else
                                blnLineaNueva = True
                                blnYaAgregada = False
                            End If
                    End Select
                Else
                    ApplicationSBO.MessageBox(My.Resources.Resource.El_Item + strItemCode + My.Resources.Resource.MalConfigurado)
                End If
            End If
        Else
            p_oLineasCotizacion.UserFields.Fields.Item(g_strItemAprobado).Value = EstadosAprobacion.NoAprobado
        End If

        Return blnLineaNueva

    End Function

    Private Sub AgregarRepuesto(ByVal NoRepuesto As String, _
                                ByVal decCantidad As Decimal, _
                                ByVal intLineNum As Integer, _
                                ByVal intTransf As Integer, _
                                ByVal p_strCodeEspecifico As String, _
                                ByVal p_strNameEspecifico As String, _
                                ByVal p_intTipo As Integer,
                                ByVal p_strNoOrden As String,
                                ByVal p_strIdRepXOrd As String)
        Try
            Dim drwRepuesto As RepuestosxOrdenDataset.SCGTA_TB_RepuestosxOrdenRow

            If p_strIdRepXOrd.Trim = 0 Then

                drwRepuesto = dtbRepuestosxOrden.NewSCGTA_TB_RepuestosxOrdenRow

                With drwRepuesto

                    .NoOrden = p_strNoOrden
                    .NoRepuesto = NoRepuesto
                    .Cantidad = decCantidad
                    .Adicional = 0
                    .TipoArticulo = p_intTipo
                    .LineNum = intLineNum
                    .EstadoTransf = intTransf
                    .LineNumFather = "-1"

                    Dim intLineNumOriginal As Integer = intLineNum

                    .LineNumOriginal = intLineNumOriginal

                    If p_strCodeEspecifico = String.Empty Then

                        .IsItemCodeEspecificoNull()
                    Else

                        .ItemCodeEspecifico = p_strCodeEspecifico
                    End If

                    .Itemname = p_strNameEspecifico
                End With

                Call dtbRepuestosxOrden.AddSCGTA_TB_RepuestosxOrdenRow(drwRepuesto)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex

        End Try

    End Sub

    Private Sub RevisaStockArticulo(ByRef p_oLineas As Document_Lines, ByVal p_DocEntry As Integer, ByVal p_strBodegaProcesoRepuestos As String,
                                    ByVal p_intTipoArticulo As Integer, ByVal p_intGenerico As Integer, ByRef p_decCantidadItem As Decimal,
                                    ByRef p_intEstadoTraslado As String, ByRef p_blnRechazarItem As Boolean, ByVal p_blnActualizarCantidad As Boolean,
                                    ByVal p_strCadenaConexionBDTaller As String, ByVal p_blnConf_TallerEnSAP As Boolean)

        Dim m_strValidacionResultado As ResultadoValidacionPorItem
        Dim decCantidad As Decimal
        Dim m_strEstadoAprobacion As String = String.Empty
        Dim m_strEstadoTraslado As String = String.Empty
        Dim m_strItemCode As String = String.Empty

        Try
            With p_oLineas
                m_strItemCode = .ItemCode
                m_strEstadoAprobacion = .UserFields.Fields.Item(g_strItemAprobado).Value
                m_strEstadoTraslado = .UserFields.Fields.Item(g_strTrasladado).Value

                If ((m_strEstadoAprobacion = EstadosAprobacion.Aprobado) _
                     AndAlso m_strEstadoTraslado = EstadosTraslados.NoProcesado) Or p_blnActualizarCantidad Then

                    If p_intTipoArticulo = TiposArticulos.scgRepuesto _
                        AndAlso p_intGenerico = 1 Then

                        p_decCantidadItem = .Quantity

                        m_strValidacionResultado = ValidarCantidadDisponibleRepuesto(.ItemCode, .ItemDescription, .LineNum, p_DocEntry, p_decCantidadItem, p_strBodegaProcesoRepuestos, p_blnActualizarCantidad, .UserFields.Fields.Item(g_strTrasladado).Value, p_strCadenaConexionBDTaller, p_blnConf_TallerEnSAP)

                        If m_strValidacionResultado = ResultadoValidacionPorItem.scgNoAprobar Then

                            p_blnRechazarItem = True
                            p_intEstadoTraslado = ResultadoValidacionPorItem.scgSinCambio
                            m_intRealizarTraslados = RealizarTraslados.No

                        ElseIf m_strValidacionResultado = ResultadoValidacionPorItem.scgModQtyCoti Then

                            p_decCantidadItem = decCantidad
                            p_intEstadoTraslado = ResultadoValidacionPorItem.scgModQtyCoti
                            m_intRealizarTraslados = RealizarTraslados.Si

                        ElseIf m_strValidacionResultado = ResultadoValidacionPorItem.scgPendTransf Then

                            p_intEstadoTraslado = ResultadoValidacionPorItem.scgPendTransf
                            m_intRealizarTraslados = RealizarTraslados.Si

                        ElseIf m_strValidacionResultado = ResultadoValidacionPorItem.scgComprar Then

                            p_decCantidadItem = .Quantity

                            p_intEstadoTraslado = ResultadoValidacionPorItem.scgComprar
                            m_intRealizarTraslados = RealizarTraslados.Si

                        Else

                            If g_blnDraft Then

                                If Not p_blnActualizarCantidad Then
                                    p_decCantidadItem = .Quantity
                                End If

                                'p_intEstadoTraslado = 5
                                p_intEstadoTraslado = ResultadoValidacionPorItem.scgPendBodega
                                m_intRealizarTraslados = RealizarTraslados.No

                            Else

                                p_decCantidadItem = .Quantity
                                'p_intEstadoTraslado = 2
                                p_intEstadoTraslado = ResultadoValidacionPorItem.scgModQtyCoti
                                m_intRealizarTraslados = RealizarTraslados.Si

                            End If
                        End If
                    End If

                End If
            End With
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Private Function ValidarCantidadDisponibleRepuesto(ByVal p_strItemCode As String,
                                                       ByVal p_strItemDescription As String,
                                                       ByVal p_intlineNum As Integer,
                                                       ByVal p_intDocEntry As Integer,
                                                       ByVal p_decCantidadItem As Decimal,
                                                       ByVal p_strBodegaProcesoRepuestos As String,
                                                       ByVal p_blnActualizarCantidad As Boolean,
                                                       ByVal p_intEstadoItem As Integer,
                                                       ByVal p_strCadenaConexionBDTaller As String,
                                                       ByVal p_blnConf_TallerEnSAP As Boolean) As ResultadoValidacionPorItem
        Try
            Dim decCantidad As Decimal
            Dim decCantXLineasAnteriores As Decimal = 0
            Dim intMsgResult As Integer
            Dim l_enumResult As ResultadoValidacionPorItem
            Dim ItemCantAnterior As stTipoListaCantAnteriores

            decCantidad = DevuelveStockDisponibleItem(p_strItemCode, p_strBodegaProcesoRepuestos)
            If Not p_blnConf_TallerEnSAP Then
                decCantXLineasAnteriores = DevuelveCantXLineasAnteriores(p_strItemCode, p_intlineNum, p_intDocEntry, p_strCadenaConexionBDTaller)

            End If
            If decCantXLineasAnteriores <> 0 Then

                With ItemCantAnterior
                    .Cantidad = decCantXLineasAnteriores
                    .ItemCode = p_strItemCode
                    .LineNum = p_intlineNum
                End With

                m_lstCantidadesAnteriores.Add(ItemCantAnterior)

            End If

            If (decCantidad - decCantXLineasAnteriores) <= 0 AndAlso p_intEstadoItem = 0 Then

                If Not p_blnActualizarCantidad Then
                    intMsgResult = ApplicationSBO.MessageBox(My.Resources.Resource.El_Item & p_strItemDescription & My.Resources.Resource.SinInventario, 1, My.Resources.Resource.Comprar, My.Resources.Resource.Rechazar, My.Resources.Resource.Trasladar)
                Else
                    intMsgResult = ApplicationSBO.MessageBox(My.Resources.Resource.El_Item & p_strItemDescription & My.Resources.Resource.SinInventario, 1, My.Resources.Resource.Comprar, My.Resources.Resource.Rechazar)
                End If

                If intMsgResult = 1 Then
                    l_enumResult = ResultadoValidacionPorItem.scgComprar
                ElseIf intMsgResult = 2 Then
                    l_enumResult = ResultadoValidacionPorItem.scgNoAprobar
                ElseIf intMsgResult = 3 Then
                    l_enumResult = ResultadoValidacionPorItem.scgPendTransf
                End If

            ElseIf (decCantidad - decCantXLineasAnteriores) < p_decCantidadItem AndAlso p_intEstadoItem = 0 Then

                intMsgResult = ApplicationSBO.MessageBox(My.Resources.Resource.ItemCantidadInventario & p_strItemDescription & My.Resources.Resource.InventarioInsuficiente, 1, My.Resources.Resource.PendienteTraslado, My.Resources.Resource.Trasladar, My.Resources.Resource.Rechazar)

                If intMsgResult = 1 Then
                    l_enumResult = ResultadoValidacionPorItem.scgPendTransf
                ElseIf intMsgResult = 2 Then
                    l_enumResult = ResultadoValidacionPorItem.scgModQtyCoti
                    p_decCantidadItem = decCantidad
                ElseIf intMsgResult = 3 Then
                    l_enumResult = ResultadoValidacionPorItem.scgNoAprobar
                End If
            Else
                If g_blnDraft Then
                    m_intRealizarTraslados = RealizarTraslados.No
                    l_enumResult = ResultadoValidacionPorItem.scgPendBodega
                Else
                    m_intRealizarTraslados = RealizarTraslados.Si
                    l_enumResult = ResultadoValidacionPorItem.scgSinCambio
                End If

            End If

            Return l_enumResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Private Function DevuelveStockDisponibleItem(ByVal strItemcode As String, ByVal strWhsCode As String) As Double

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim oItemWhsInfo As SAPbobsCOM.IItemWarehouseInfo
        Dim intCount As Integer
        Dim dblStock As Double

        oItemArticulo = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)
        oItemWhsInfo = oItemArticulo.WhsInfo

        For intCount = 0 To oItemWhsInfo.Count - 1
            With oItemWhsInfo
                .SetCurrentLine(intCount)
                If .WarehouseCode = strWhsCode Then
                    dblStock = .InStock - .Committed
                    Exit For
                End If
            End With
        Next
        Return dblStock
    End Function

    Private Function DevuelveCantXLineasAnteriores(ByVal p_strItemCode As String, ByVal p_intLineNum As Integer, _
                            ByVal p_intDocEntry As Integer, ByVal p_strCadenaConexionBDTaller As String) As Decimal


        Dim objUtilitarios As New SCGDataAccess.Utilitarios(p_strCadenaConexionBDTaller)
        Dim dstCotizacionLineas As Cotizacion_LineasDataset
        Dim drwCotizacionLinea As Cotizacion_LineasDataset.Cotizacion_LineasRow

        Dim intContLineas As Integer
        Dim decCantidadAnterior As Integer = 0

        dstCotizacionLineas = objUtilitarios.ObtenerItemsCotizaRepetidosByItemCode(p_intDocEntry, p_intLineNum, p_strItemCode)

        For intContLineas = 0 To dstCotizacionLineas.Cotizacion_Lineas.Rows.Count - 1

            drwCotizacionLinea = dstCotizacionLineas.Cotizacion_Lineas.Rows(intContLineas)

            If drwCotizacionLinea.LineNum < p_intLineNum Then

                If drwCotizacionLinea.U_SCGD_Aprobado = 1 AndAlso (drwCotizacionLinea.U_SCGD_Traslad = 0 Or _
                        drwCotizacionLinea.U_SCGD_Traslad = 3) Then

                    decCantidadAnterior += drwCotizacionLinea.Quantity

                End If

            Else

                Exit For

            End If

        Next

        Return decCantidadAnterior

    End Function

    Private Function ValidaConfiguracionArticulo(ByVal p_ItemCode As String, ByVal p_Inventariable As BoYesNoEnum, ByVal p_DeVenta As BoYesNoEnum,
                                                 ByVal p_DeCompra As BoYesNoEnum, ByVal p_blnTomaEnCuentaVenta As Boolean, ByRef p_strCentroCosto As String,
                                                 ByVal p_strIdSucursal As String, ByVal p_blnUsaConfiguracionInternaTaller As Boolean) As Boolean
        Try
            Dim m_oItemArticulo As SAPbobsCOM.IItems
            Dim strNombre As String = String.Empty


            m_oItemArticulo = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            m_oItemArticulo.GetByKey(p_ItemCode)

            strNombre = m_oItemArticulo.ItemName.ToString()

            If m_oItemArticulo.InventoryItem <> p_Inventariable Then Return False
            If (m_oItemArticulo.PurchaseItem <> p_DeCompra AndAlso p_blnTomaEnCuentaVenta) Then Return False
            If m_oItemArticulo.SalesItem <> p_DeVenta Then Return False

            Return ValidarCentroCosto(p_ItemCode, p_strCentroCosto, p_strIdSucursal, p_blnUsaConfiguracionInternaTaller)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Function ValidarCentroCosto(ByVal strItemcode As String, ByRef p_strCentroCosto As String, ByVal p_strIdSucursal As String,
                                       ByVal p_blnUsaConfiguracionInternaTaller As Boolean) As Boolean

        Try
            Dim strConsultaCentroCosto As String

            Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim strConectionString As String = String.Empty
            Dim cn_Coneccion As New SqlClient.SqlConnection

            Dim strBodegaProcesoCC As String = String.Empty
            Dim strBodegaProcesoItem As String = String.Empty
            Dim strConsultaBodXArt As String = String.Empty

            Dim m_strBDTalller As String = String.Empty

            Utilitarios.DevuelveNombreBDTaller(ApplicationSBO, p_strIdSucursal, m_strBDTalller)

            p_strCentroCosto = DevuelveValorItem(strItemcode, "U_SCGD_CodCtroCosto")

            If IsNumeric(p_strCentroCosto) Then

                If Integer.TryParse(p_strCentroCosto, Nothing) Then

                    p_strCentroCosto = CStr(p_strCentroCosto)

                    If Not p_blnUsaConfiguracionInternaTaller Then

                        strConsultaCentroCosto = "Select Proceso from [SCGTA_TB_ConfBodegasXCentroCosto] where IDCentroCosto ='" & p_strCentroCosto & "'"

                        Configuracion.CrearCadenaDeconexion(CompanySBO.Server, m_strBDTalller, strConectionString)
                        cn_Coneccion.ConnectionString = strConectionString

                        cn_Coneccion.Open()

                        cmdEjecutarConsulta.Connection = cn_Coneccion
                        cmdEjecutarConsulta.CommandType = CommandType.Text
                        cmdEjecutarConsulta.CommandText = strConsultaCentroCosto
                        strBodegaProcesoCC = cmdEjecutarConsulta.ExecuteScalar()

                        strBodegaProcesoCC = strBodegaProcesoCC.Trim()

                        'valida la existencia del centro de costo
                        If String.IsNullOrEmpty(strBodegaProcesoCC) Then Return False

                        strConsultaBodXArt = String.Format("SELECT WhsCode FROM OITW WHERE ItemCode = '{0}'AND WhsCode = '{1}'",
                                                           strItemcode,
                                                           strBodegaProcesoCC)

                        strBodegaProcesoItem = Utilitarios.EjecutarConsulta(strConsultaBodXArt, CompanySBO.CompanyDB, CompanySBO.Server)

                        strBodegaProcesoItem = strBodegaProcesoItem.Trim()

                        'valida la existencia de la bodega de proceso para ese item
                        If String.IsNullOrEmpty(strBodegaProcesoItem) Then
                            ApplicationSBO.StatusBar.SetText(
                                My.Resources.Resource.El_Item + strItemcode + My.Resources.Resource.NoEncontradoEnAlmacen + strBodegaProcesoCC,
                                BoMessageTime.bmt_Medium,
                                BoStatusBarMessageType.smt_Error)
                            Return False
                        End If

                        cn_Coneccion.Close()

                    Else

                        Dim strBodegaProcesoInterno As String
                        strBodegaProcesoInterno = Utilitarios.GetBodegaXCentroCosto(p_strCentroCosto, TransferenciaItems.mc_strBodegaProceso, p_strIdSucursal, ApplicationSBO)

                        strBodegaProcesoCC = strBodegaProcesoInterno.Trim()

                        strBodegaProcesoCC = Utilitarios.GetBodegaXCentroCosto(p_strCentroCosto, "BodegaProceso", p_strIdSucursal, ApplicationSBO)
                        strBodegaProcesoCC = strBodegaProcesoCC.Trim()
                        'valida la existencia del centro de costo
                        If String.IsNullOrEmpty(strBodegaProcesoCC) Then
                            Return False
                        End If

                        strConsultaBodXArt = String.Format("SELECT WhsCode FROM OITW WHERE ItemCode = '{0}'AND WhsCode = '{1}'",
                                                          strItemcode,
                                                          strBodegaProcesoCC)

                        strBodegaProcesoItem = Utilitarios.EjecutarConsulta(strConsultaBodXArt, CompanySBO.CompanyDB, CompanySBO.Server)

                        strBodegaProcesoItem = strBodegaProcesoItem.Trim()

                        'valida la existencia de la bodega de proceso para ese item
                        If String.IsNullOrEmpty(strBodegaProcesoItem) Then
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.El_Item + strItemcode + My.Resources.Resource.NoEncontradoEnAlmacen + strBodegaProcesoCC,
                                                              SAPbouiCOM.BoMessageTime.bmt_Medium,
                                                              SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            Return False
                        End If

                    End If

                Else
                    Return False
                End If
            Else
                Return False
            End If
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try

        Return True

    End Function



    Private Function CargaObjetoCotizacion(ByVal p_NumCotizacion As Integer) As SAPbobsCOM.Documents

        Dim oCotizacion As SAPbobsCOM.Documents

        Try
            oCotizacion = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If oCotizacion.GetByKey(p_NumCotizacion) Then

                Return oCotizacion

            End If

        Catch ex As Exception

            Throw ex

        End Try
        Return Nothing
    End Function

    Public Sub BuscarCotizacion(ByVal DocEntry As String)
        Dim oCotizacion As Documents
        Dim intEstadoControles As Integer

        Try
            oCotizacion = _companySbo.GetBusinessObject(BoObjectTypes.oQuotations)
            If oCotizacion.GetByKey(Convert.ToInt32(DocEntry)) Then

                UDS_IncluyeRepuestos.Item("NoOrden").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString()
                UDS_IncluyeRepuestos.Item("NoUni").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value.ToString()
                UDS_IncluyeRepuestos.Item("TiOr").Value = DMS_Connector.Configuracion.TipoOt.FirstOrDefault(Function(x) x.Code = oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()).Name
                UDS_IncluyeRepuestos.Item("EsOT").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value.ToString()
                UDS_IncluyeRepuestos.Item("Marca").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString()
                UDS_IncluyeRepuestos.Item("Estilo").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString()
                UDS_IncluyeRepuestos.Item("Modelo").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString()
                UDS_IncluyeRepuestos.Item("NoCono").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString()
                UDS_IncluyeRepuestos.Item("NoVin").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString()
                UDS_IncluyeRepuestos.Item("Kim").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString()
                UDS_IncluyeRepuestos.Item("Placa").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString()
                UDS_IncluyeRepuestos.Item("DocE").Value = oCotizacion.UserFields.Fields.Item("DocEntry").Value.ToString()
                UDS_IncluyeRepuestos.Item("CodCli").Value = oCotizacion.UserFields.Fields.Item("CardCode").Value.ToString()
                UDS_IncluyeRepuestos.Item("Moneda").Value = oCotizacion.UserFields.Fields.Item("DocCur").Value.ToString()
                UDS_IncluyeRepuestos.Item("TipoCam").Value = oCotizacion.UserFields.Fields.Item("DocRate").Value.ToString()
                UDS_IncluyeRepuestos.Item("FechaCot").Value = oCotizacion.UserFields.Fields.Item("DocDate").Value.ToString()
                UDS_IncluyeRepuestos.Item("IdEsT").Value = oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value.ToString()

                CodeCliente = txtCodCli.ObtieneValorUserDataSource()
                NoOT = txtNoOrden.ObtieneValorUserDataSource()
                strMoneda = txtMonCot.ObtieneValorUserDataSource()
                strFechaCot = txtFechaCot.ObtieneValorUserDataSource()
                Sucursal = oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()

                If Not String.IsNullOrEmpty(txtTCCot.ObtieneValorUserDataSource().ToString()) Then
                    dcTCCot = Decimal.Parse(txtTCCot.ObtieneValorUserDataSource().ToString())
                Else
                    dcTCCot = 110
                End If

                If (oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "1" Or oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "2") Then
                    intEstadoControles = 1
                End If


                CargaRepuestos(oCotizacion)

            Else

            txtNoUni.AsignaValorUserDataSource(String.Empty)
            txtTiOr.AsignaValorUserDataSource(String.Empty)
            txtEsOT.AsignaValorUserDataSource(String.Empty)
            txtMarca.AsignaValorUserDataSource(String.Empty)
            txtEstilo.AsignaValorUserDataSource(String.Empty)
            txtModelo.AsignaValorUserDataSource(String.Empty)
            txtNoCono.AsignaValorUserDataSource(String.Empty)
            txtNoVin.AsignaValorUserDataSource(String.Empty)
            txtKim.AsignaValorUserDataSource(String.Empty)
            txtPlaca.AsignaValorUserDataSource(String.Empty)
            txtDocE.AsignaValorUserDataSource(String.Empty)
            txtCodCli.AsignaValorUserDataSource(String.Empty)
            txtMonCot.AsignaValorUserDataSource(String.Empty)
            txtTCCot.AsignaValorUserDataSource(String.Empty)
            txtFechaCot.AsignaValorUserDataSource(String.Empty)
            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.NoOTNoExiste, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

            End If


            FormularioSBO.Items.Item("btnAdd").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstadoControles)
            FormularioSBO.Items.Item("btnDel").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstadoControles)
            FormularioSBO.Items.Item("btnAct").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstadoControles)
            FormularioSBO.Items.Item("btnDesA").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, intEstadoControles)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina los repuestos seleccionados en la matriz
    ''' </summary>
    ''' <param name="FormUID">Identificador del formulario</param>
    ''' <remarks></remarks>
    Private Sub EliminarRepuestosSeleccionados(ByVal FormUID As String)

        Dim oMatrix As IMatrix
        Dim oForm As Form

        Dim lsListaOrdenada As Generic.IList(Of Integer) = New Generic.List(Of Integer)

        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)
            dtRepuestos = FormularioSBO.DataSources.DataTables.Item(strDTRepuestos)

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxRep").Specific, Matrix)
            oMatrix.FlushToDataSource()

            SeleccionarRepuestosOT.OrdenaLista(lsListaEliminar, lsListaOrdenada)

            For Each Str As String In lsListaOrdenada
                Posicion = Integer.Parse(Str)

                If dtRepuestos.GetValue("sel", Posicion - 1).ToString = "Y" Then

                    Select Case dtRepuestos.GetValue("per", Posicion - 1).ToString
                        Case My.Resources.Resource.No
                            dtRepuestos.Rows.Remove(Posicion - 1)
                        Case My.Resources.Resource.Si
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.AdvertenciaSeleccionRepuestos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            dtRepuestos.SetValue("sel", Posicion - 1, "N")
                    End Select
                End If
            Next

            ValidaCambios()

            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    ''' <summary>
    ''' Realiza el manejo multi Moneda
    ''' </summary>
    ''' <param name="Precio">Precio a manipular</param>
    ''' <param name="MonedaLP">Moneda de la lista de precios</param>
    ''' <param name="MonedaCot">Moneda de cotizacion</param>
    ''' <param name="TCCot">Tipo cambio de la cotizacion</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ManejoMultiMoneda(ByVal Precio As Decimal,
                                       ByVal MonedaLP As String,
                                       ByVal MonedaCot As String,
                                       ByVal TCCot As Decimal) As String

        Dim strMonedaLocal As String = ""
        Dim strMonedaSistema As String = ""

        Dim fecha As Date

        Dim strTipoCambioMS As String = ""
        Dim dcTipoCambio_MS As Decimal
        Dim dcTipoCambio_Cotizacion As Decimal
        Dim strTipoCambioME As String = ""
        Dim dcTipoCambio_ME As Decimal
        Dim dcPrecioManipulado As Decimal = 0
        Dim strQuery As String = "SELECT Rate FROM ORTT WHERE Currency='{0}'" & _
                              " AND RateDate='{1}'"
        Dim strNuevaFecha As String = ""
        Dim dt As Date

        Try
            objGlobal = New DMSOneFramework.BLSBO.GlobalFunctionsSBO

            dt = Date.Parse(strFechaCot)

            strNuevaFecha = Utilitarios.RetornaFechaFormatoDB(dt, CompanySBO.Server)

            'strNuevaFecha = Utilitarios.RetornaFechaFormatoRegional(strFechaCot)

            strMonedaLocal = Utilitarios.EjecutarConsulta("select MainCurncy from oadm", CompanySBO.CompanyDB, CompanySBO.Server)

            strMonedaSistema = Utilitarios.EjecutarConsulta("select SysCurrncy from oadm", CompanySBO.CompanyDB, CompanySBO.Server)

            If String.IsNullOrEmpty(MonedaLP) Then MonedaLP = strMonedaLocal

            If MonedaCot <> strMonedaLocal And MonedaCot <> strMonedaSistema Then strTipoCambioME = Utilitarios.EjecutarConsulta(String.Format(strQuery, MonedaCot, strNuevaFecha), CompanySBO.CompanyDB, CompanySBO.Server)

            If MonedaLP <> strMonedaLocal And MonedaLP <> strMonedaSistema Then strTipoCambioME = Utilitarios.EjecutarConsulta(String.Format(strQuery, MonedaLP, strNuevaFecha), CompanySBO.CompanyDB, CompanySBO.Server)

            If Not String.IsNullOrEmpty(strTipoCambioME) Then
                dcTipoCambio_ME = Decimal.Parse(strTipoCambioME)
            Else
                dcTipoCambio_ME = 0
            End If

            strTipoCambioMS = Utilitarios.EjecutarConsulta(String.Format(strQuery, strMonedaSistema, strNuevaFecha), CompanySBO.CompanyDB, CompanySBO.Server)

            If Not String.IsNullOrEmpty(strTipoCambioMS) Then
                dcTipoCambio_MS = Decimal.Parse(strTipoCambioMS)
            Else
                dcTipoCambio_MS = 0
            End If

            dcTipoCambio_Cotizacion = TCCot

            Select Case MonedaLP

                Case strMonedaLocal

                    Select Case MonedaCot

                        Case strMonedaLocal

                            dcPrecioManipulado = Precio
                            Return dcPrecioManipulado

                        Case strMonedaSistema

                            If dcTipoCambio_Cotizacion = 0 Then Return -333
                            dcPrecioManipulado = Precio / dcTipoCambio_Cotizacion
                            Return dcPrecioManipulado

                        Case Else

                            If dcTipoCambio_Cotizacion = 0 Then Return -333
                            dcPrecioManipulado = Precio / dcTipoCambio_Cotizacion
                            Return dcPrecioManipulado

                    End Select

                Case strMonedaSistema

                    Select Case MonedaCot

                        Case strMonedaLocal

                            If dcTipoCambio_MS = 0 Then Return -111
                            dcPrecioManipulado = Precio * dcTipoCambio_MS
                            Return dcPrecioManipulado

                        Case strMonedaSistema

                            dcPrecioManipulado = Precio
                            Return dcPrecioManipulado

                        Case Else

                            If dcTipoCambio_Cotizacion = 0 Then Return -111
                            If dcTipoCambio_MS = 0 Then Return -333
                            dcPrecioManipulado = (Precio * dcTipoCambio_MS) / dcTipoCambio_Cotizacion
                            Return dcPrecioManipulado

                    End Select

                Case Else

                    Select Case MonedaCot

                        Case strMonedaLocal

                            If dcTipoCambio_ME = 0 Then Return -222
                            dcPrecioManipulado = Precio * dcTipoCambio_ME
                            Return dcPrecioManipulado

                        Case strMonedaSistema

                            If dcTipoCambio_ME = 0 Then Return -222
                            If dcTipoCambio_MS = 0 Then Return -111
                            dcPrecioManipulado = (Precio * dcTipoCambio_ME) / dcTipoCambio_MS
                            Return dcPrecioManipulado

                        Case Else

                            dcPrecioManipulado = Precio
                            Return dcPrecioManipulado

                    End Select

            End Select

            If dcPrecioManipulado = 0 Then dcPrecioManipulado = Precio

            Return dcPrecioManipulado

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Valida el precio de acuerdo a los posibles faltantes de tipos de cambio
    ''' </summary>
    ''' <param name="dtSeleccionados">Datatable con los valore seleccionados</param>
    ''' <param name="BubbleEvent">Valor por referencia del BubbleEvent</param>
    ''' <remarks></remarks>
    Public Sub ValidaPrecios(ByVal dtSeleccionados As DataTable, ByRef BubbleEvent As Boolean)

        Dim dcPrecio As Decimal
        Dim strMensaje As String

        Try
            For i As Integer = 0 To dtSeleccionados.Rows.Count - 1
                dcPrecio = ManejoMultiMoneda(Decimal.Parse(dtSeleccionados.GetValue("pre", i)),
                                                  dtSeleccionados.GetValue("mon", i),
                                                  strMoneda,
                                                  dcTCCot)

                If dcPrecio = -111 Or dcPrecio = -222 Or dcPrecio = -333 Then

                    Select Case dcPrecio
                        Case -111
                            strMensaje = My.Resources.Resource.MonedaSistema
                        Case -222
                            strMensaje = My.Resources.Resource.MonedaExtranjera
                        Case -333
                            strMensaje = My.Resources.Resource.LaCotizacion
                    End Select

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ElTipoCambioPara + strMensaje +
                                                     My.Resources.Resource.EnLaFecha + strFechaCot +
                                                     My.Resources.Resource.NoSeEncuentraDefinido,
                                                     BoMessageTime.bmt_Short,
                                                     BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False

                End If
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Function DevuelveValorItem(ByVal strItemcode As String, _
                                       ByVal strUDfName As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim valorUDF As String

        oItemArticulo = CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)
        valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value

        Return valorUDF

    End Function

    Private Function DesaprobarRepuestosOT(ByVal FormUID As String) As Boolean
        Dim oMatrix As IMatrix
        Dim oForm As Form
        Dim dtRepDesAprob As SAPbouiCOM.DataTable
        Dim lsListaOrdenada As Generic.IList(Of Integer) = New Generic.List(Of Integer)
        Dim strMensaje As String
        Dim result As Boolean = True
        Dim message As String = String.Empty
        Dim oSelect As SAPbouiCOM.CheckBox
        Dim noRowsSelected As Integer = 0
        Dim cRec As Double
        Dim cSol As Double
        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)
            dtRepuestos = FormularioSBO.DataSources.DataTables.Item(strDTRepuestos)
            dtRepDesAprob = dtRepuestos

            oMatrix = DirectCast(FormularioSBO.Items.Item("mtxRep").Specific, Matrix)
            oMatrix.FlushToDataSource()

            'SeleccionarRepuestosOT.OrdenaLista(lsListaEliminar, lsListaOrdenada)

            For i As Integer = 0 To dtRepuestos.Rows.Count - 1
                If (dtRepuestos.GetValue("per", i).ToString.Trim = My.Resources.Resource.No) Then
                    message = My.Resources.Resource.TXTLineasNoActualizadas
                    ApplicationSBO.StatusBar.SetText(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    result = False
                    Return result
                Else
                    oSelect = DirectCast(oMatrix.Columns.Item("Col_sel").Cells.Item(i + 1).Specific, SAPbouiCOM.CheckBox)
                    If oSelect.Checked Then
                        message = String.Format(My.Resources.Resource.ErrorDesaprobarNoAprobados, dtRepuestos.GetValue("des", i).ToString.Trim)
                        Select Case dtRepuestos.GetValue("per", i).ToString
                            Case My.Resources.Resource.No
                                ApplicationSBO.StatusBar.SetText(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                                oSelect.Checked = False
                                result = False
                                Return result
                            Case My.Resources.Resource.Si
                                If dtRepuestos.GetValue("apr", i).ToString().Trim() <> "Si" Then
                                    ApplicationSBO.StatusBar.SetText(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                                    oSelect.Checked = False
                                    result = False
                                    Return result
                                End If
                                If dtRepuestos.GetValue("com", i).ToString().Trim() = "Y" Then
                                    cRec = Convert.ToDouble(dtRepuestos.GetValue("rec", i).ToString())
                                    cSol = Convert.ToDouble(dtRepuestos.GetValue("sol", i).ToString())
                                    If cRec > 0 OrElse cSol > 0 Then
                                        message = String.Format(My.Resources.Resource.ErrorDesaprobarCompRecSol, dtRepuestos.GetValue("des", i).ToString.Trim)
                                        ApplicationSBO.StatusBar.SetText(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                                        oSelect.Checked = False
                                        result = False
                                        Return result
                                    End If
                                End If
                        End Select
                    Else
                        noRowsSelected += 1
                    End If
                End If

            Next
            If noRowsSelected = dtRepuestos.Rows.Count Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrNoRowsSelected, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                result = False
                Return result
            End If
            'oMatrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Return False
        End Try
        Return result
    End Function

    Private Sub EliminarRegistroLineNumErroneo(ByVal p_ListaLineNum As IList, ByVal p_strIdSucursal As String)

        Dim strConectionString As String = ""
        Dim strNombreTaller As String = String.Empty
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Dim strConsulta As String = ""
        Dim cmdAsiento As New SqlClient.SqlCommand

        Utilitarios.DevuelveNombreBDTaller(ApplicationSBO, p_strIdSucursal, strNombreTaller)

        Dim baseDatos As String
        baseDatos = ApplicationSBO.Company.DatabaseName
        Dim Server As String
        Server = ApplicationSBO.Company.ServerName

        For Each objlist As LineasLineNumErroneos In p_ListaLineNum

            Select Case objlist.TipoRow
                Case 1

                    strConsulta = "DELETE FROM SCGTA_TB_RepuestosxEstado WHERE IdRepuestosxOrden = '" & objlist.Id & "'"
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)

                    strConsulta = String.Empty

                    strConsulta = "DELETE FROM SCGTA_TB_RepuestosxOrden Where  NoOrden = '" & objlist.NoOrden & "' and NoRepuesto = '" & objlist.IdItem & "' and LineNum = " & objlist.intLineNum & ""
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)
                Case 2

                    strConsulta = "DELETE FROM SCGTA_TB_SuministroxOrden Where  NoOrden = '" & objlist.NoOrden & "' and NoSuministro = '" & objlist.IdItem & "' and LineNum = " & objlist.intLineNum & ""
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)
                Case 3
                    strConsulta = "DELETE FROM SCGTA_TB_ActividadesxOrden Where  NoOrden  = '" & objlist.NoOrden & "' and NoActividad = '" & objlist.IdItem & "' and LineNum = " & objlist.intLineNum & ""
                    Utilitarios.EjecutarConsulta(strConsulta, strNombreTaller, Server)
            End Select

        Next

    End Sub


#End Region

End Class
