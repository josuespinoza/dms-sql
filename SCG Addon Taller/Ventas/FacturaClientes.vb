Imports System.Collections.Generic
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports SCG.SBOFramework
Imports DMSOneFramework
Imports System.Linq


Public Class FacturaClientes

#Region "Definiciones"

    Private SBO_Application As SAPbouiCOM.Application
    Private SBO_Company As SAPbobsCOM.Company

    Private dtSE As SAPbouiCOM.DataTable
    Private dtImpuestos As SAPbouiCOM.DataTable
    Private dtInfoImpuestos As SAPbouiCOM.DataTable

    Public n As NumberFormatInfo

    Private strNoAsiento As String = ""
    Private intNoAsientoMO As Integer = 0
    Private intNoAsientoGastos As Integer = 0
    Private intNoAsientoSE As Integer = 0

    Private _CreaAsiento As Boolean
    Private _FormFactPro As SAPbouiCOM.Form
    Private _Burbuja As Boolean
    Private DocEntryFacturaCliente As String = String.Empty

    Private strSCGD_TipoArticulo As String = "U_SCGD_TipoArticulo"
    Private mc_strSCGD_NoOT As String = "U_SCGD_NoOT"

    Private Const mc_strCboTipoPago As String = "cboTipPago"
    Private Const mc_strCboDptoSrv As String = "cboDptoSrv"

    Private oDataTableDimensionesContablesDMS As SAPbouiCOM.DataTable

    'Private ListaConfiguracionOT As Hashtable
    Private ListaConfiguracionOT As List(Of LineasConfiguracionOT)

    Public Const mc_strDataTableDimensionesOT As String = "DimensionesContablesDMSOT"

    Public ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls

    Private blnUsaDimensiones As Boolean = False

    Private listBaseEntry As List(Of String())
    Private blnBaseEntry As Boolean
    Private oPosicionControles As Dictionary(Of String, Coordenadas)

    Private Enum TipoPosicionControles
        Estandar = 1
        FacturaElectronica = 2
    End Enum

#End Region

#Region "Constructor"
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)
        
        SBO_Application = SBOAplication
        SBO_Company = ocompany

        n = DIHelper.GetNumberFormatInfo(SBO_Company)
        listBaseEntry = New List(Of String())
        InicializarPosicionControles()
    End Sub

    ''' <summary>
    ''' Guarda la posición de todos los controles utilizados por DMS en un objeto Diccionario
    ''' </summary>
    ''' <remarks>Ejemplo de como agregar las coordenadas de un control:
    ''' oPosicionControles.Add("IDControl", New Coordenadas(Left, Top))</remarks>
    Private Sub InicializarPosicionControles()
        Dim strPosicionCampos As String = String.Empty
        Try
            'Instancia un objeto diccionario
            'la llave corresponde al ID único del control y el valor es un objeto que contiene las coordenadas
            oPosicionControles = New Dictionary(Of String, Coordenadas)

            strPosicionCampos = DMS_Connector.Configuracion.ParamGenAddon.U_FieldsPosition
            If String.IsNullOrEmpty(strPosicionCampos) Then
                strPosicionCampos = TipoPosicionControles.Estandar
            End If

            Select Case strPosicionCampos
                Case TipoPosicionControles.Estandar
                    oPosicionControles.Add("SCGD_stCOT", New Coordenadas(6, 80)) 'StaticText Cliente OT
                    oPosicionControles.Add("SCGD_etCOT", New Coordenadas(127, 80)) 'EditText Cliente OT
                    oPosicionControles.Add("SCGD_LKCli", New Coordenadas(114, 82)) 'LinkButton Cliente OT
                    oPosicionControles.Add("SCGD_stNOT", New Coordenadas(6, 95)) 'StaticText Nombre Cliente
                    oPosicionControles.Add("SCGD_etNOT", New Coordenadas(127, 95)) 'EditText Nombre Cliente
                Case TipoPosicionControles.FacturaElectronica
                    oPosicionControles.Add("SCGD_stCOT", New Coordenadas(301, 5)) 'StaticText Cliente OT
                    oPosicionControles.Add("SCGD_etCOT", New Coordenadas(422, 5)) 'EditText Cliente OT
                    oPosicionControles.Add("SCGD_LKCli", New Coordenadas(409, 5)) 'LinkButton Cliente OT
                    oPosicionControles.Add("SCGD_stNOT", New Coordenadas(301, 20)) 'StaticText Nombre Cliente
                    oPosicionControles.Add("SCGD_etNOT", New Coordenadas(422, 20)) 'EditText Nombre Cliente
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
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
#End Region
#Region "Propiedades"

    Public Property CreaAsiento As Boolean
        Get
            Return _CreaAsiento
        End Get
        Set(ByVal value As Boolean)
            _CreaAsiento = value
        End Set
    End Property

    Public Property FormFacPro As Form
        Get
            Return _FormFactPro
        End Get
        Set(ByVal value As Form)
            _FormFactPro = value
        End Set
    End Property

    Public Property Burbuja As Boolean
        Get
            Return _Burbuja
        End Get
        Set(ByVal value As Boolean)
            _Burbuja = value
        End Set
    End Property


    Private m_strBDTalllerDMS As String
    Public Property BDTallerDMS() As String

        Get

            Return m_strBDTalllerDMS

        End Get

        Set(ByVal value As String)

            m_strBDTalllerDMS = value

        End Set

    End Property

#End Region

#Region "Manejo de eventos"

    Public Sub ManejadorEventoGOTFOCUSPress(ByRef pval As SAPbouiCOM.ItemEvent, ByVal FormUID As String, ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)
        Try
            If oForm IsNot Nothing Then
                If pval.ActionSuccess Then
                    If IsNothing(BooleanBaseEntry) Then
                        BooleanBaseEntry = False
                    End If
                    If (pval.ItemUID = "4" OrElse pval.ItemUID = "54") AndAlso Not BooleanBaseEntry Then
                        saveBaseEntry(oForm)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    Public Sub FormResizeEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            oFormulario = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            If pVal.FormTypeEx = "133" Then
                If pVal.BeforeAction Then
                    'Implementar manejo del BeforeAction = false aquí
                Else
                    AjustarPosicionControles(oFormulario)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AjustarPosicionControles(ByRef oFormulario As SAPbouiCOM.Form)
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim blnUsaInterfazFord As Boolean = False

        Try
            'Controles con posición fija en el formulario
            For Each oPosicion As KeyValuePair(Of String, Coordenadas) In oPosicionControles
                If Not String.IsNullOrEmpty(oPosicion.Key) Then
                    oFormulario.Items.Item(oPosicion.Key).Left = oPosicion.Value.Left
                    oFormulario.Items.Item(oPosicion.Key).Top = oPosicion.Value.Top
                End If
            Next

            'Controles con posición relativa
            'que requieren adaptarse respecto a otros controles

            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            'StaticText No OT
            oFormulario.Items.Item("SCGD_stOT").Top = intTop + 15
            oFormulario.Items.Item("SCGD_stOT").Left = intLeft

            ''LinkButton No OT
            'oFormulario.Items.Item("SCGD_LKOT").Top = intTop + 15
            'oFormulario.Items.Item("SCGD_LKOT").Left = intLeft + 104

            'EditText No OT
            oFormulario.Items.Item("SCGD_etOT").Top = intTop + 15
            oFormulario.Items.Item("SCGD_etOT").Left = intLeft + 120

            'Verifica si utiliza la interfaz de Ford
            blnUsaInterfazFord = Utilitarios.UsaInterfazFord(SBO_Company)

            If blnUsaInterfazFord Then
                AjustarControlesInterfazFord(oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AjustarControlesInterfazFord(ByRef oFormulario As SAPbouiCOM.Form)
        Dim strPosicionCampos As String = String.Empty
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Try
            strPosicionCampos = DMS_Connector.Configuracion.ParamGenAddon.U_FieldsPosition
            If String.IsNullOrEmpty(strPosicionCampos) Then
                strPosicionCampos = TipoPosicionControles.Estandar
            End If

            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            Select Case strPosicionCampos
                Case TipoPosicionControles.Estandar
                    'StaticText Tipo de Pago
                    oFormulario.Items.Item("stTipoPago").Top = 110
                    oFormulario.Items.Item("stTipoPago").Left = 6

                    'ComboBox Tipo de Pago
                    oFormulario.Items.Item("cboTipPago").Top = 110
                    oFormulario.Items.Item("cboTipPago").Left = 127
                Case TipoPosicionControles.FacturaElectronica
                    'StaticText Tipo de Pago
                    oFormulario.Items.Item("stTipoPago").Top = 35
                    oFormulario.Items.Item("stTipoPago").Left = 301

                    'ComboBox Tipo de Pago
                    oFormulario.Items.Item("cboTipPago").Top = 35
                    oFormulario.Items.Item("cboTipPago").Left = 422
            End Select

            'StaticText Departamento de Servicio
            oFormulario.Items.Item("stDptoSrv").Top = intTop + 45
            oFormulario.Items.Item("stDptoSrv").Left = intLeft

            'ComboBox Departamento de Servicio
            oFormulario.Items.Item("cboDptoSrv").Top = intTop + 45
            oFormulario.Items.Item("cboDptoSrv").Left = intLeft + 120
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub ManejadorEventoClose(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                 ByVal FormUID As String, _
                                                 ByRef BubbleEvent As Boolean)
        If pval.ActionSuccess Then
            BooleanBaseEntry = False
            ListaBaseEntry.Clear()
        End If

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                 ByVal FormUID As String, _
                                                 ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim oComboTipoPago As SAPbouiCOM.ComboBox
        Dim oComboDptoServ As SAPbouiCOM.ComboBox
        Dim ExisteDataSource As Boolean
        Dim ExisteDataSourceDimensiones As Boolean

        Try

            oForm = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)

            If oForm IsNot Nothing Then
                If pval.BeforeAction Then
                    
                    'Evento para el boton crear factura 
                    If pval.ItemUID = "1" AndAlso oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                        ExisteDataSource = False
                        ExisteDataSourceDimensiones = False
                        'verifica los datasources creados 
                        If oForm.DataSources.DataTables.Count > 0 Then
                            For i As Integer = 0 To oForm.DataSources.DataTables.Count - 1

                                If oForm.DataSources.DataTables.Item(i).UniqueID = mc_strDataTableDimensionesOT Then
                                    ExisteDataSourceDimensiones = True
                                End If

                                If oForm.DataSources.DataTables.Item(i).UniqueID = "SE" Then
                                    ExisteDataSource = True
                                    Exit For
                                End If
                            Next
                        End If

                        'de no existir el datasource se crea uno nuevo 
                        If Not ExisteDataSource Then
                            oForm.DataSources.DataTables.Add("dtConsulta")
                            dtSE = oForm.DataSources.DataTables.Add("SE")
                            dtSE.Columns.Add("LineId", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("ItemCode", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("WhsCode", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("ImpCode", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("LineVat", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("LineVatlF", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("CtaDebe", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("CtaHaber", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("LineTotal", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("TotalFrgn", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("U_SCGD_NoOT", BoFieldsType.ft_AlphaNumeric, 100)
                            dtSE.Columns.Add("U_SCGD_IdRepxOrd", BoFieldsType.ft_AlphaNumeric, 100)
                        End If

                        If Not ExisteDataSourceDimensiones Then
                            oDataTableDimensionesContablesDMS = oForm.DataSources.DataTables.Add(mc_strDataTableDimensionesOT)
                        End If

                        'se habilita para crear el asiento 
                        CreaAsiento = True

                        FormFacPro = oForm

                    End If
                    If pval.ItemUID = "1" AndAlso oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        Dim cardCode = oForm.DataSources.DBDataSources.Item("OINV").GetValue("CardCode", 0)
                        If Not String.IsNullOrEmpty(cardCode) Then
                            Dim usaInterFazFord = Utilitarios.UsaInterfazFord(SBO_Company)
                            If usaInterFazFord Then
                                Dim socioNegTip = Utilitarios.ValidaIFTipoSN(SBO_Company, oForm.DataSources.DBDataSources.Item("OINV").GetValue("CardCode", 0))
                                If Not socioNegTip Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoSN, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    BubbleEvent = False
                                    Exit Sub
                                End If

                                oComboTipoPago = oForm.Items.Item(mc_strCboTipoPago).Specific
                                oComboDptoServ = oForm.Items.Item(mc_strCboDptoSrv).Specific

                                If String.IsNullOrEmpty(oComboDptoServ.Value) Or String.IsNullOrEmpty(oComboTipoPago.Value) Then
                                    SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTValidaTipoPagoDptoServ, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                                    BubbleEvent = False
                                    Exit Sub
                                End If
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

#End Region
#Region "Metodos Nuevos"

    ''' <summary>
    ''' Actualiza los valores de la orden de venta al crear una factura cliente.
    ''' Adicionalmente, cierra las líneas aunque se haya cambiado de cliente la factura.
    ''' </summary>
    ''' <param name="p_oNoOrdenList"></param>
    ''' <param name="p_oLineaFacturaClienteList"></param>
    ''' <remarks></remarks>
    Public Sub ActualizaValoresOrdenVenta(ByRef p_oNoOrdenList As Generic.List(Of String), _
                                          ByRef p_oLineaFacturaClienteList As DocumentoMarketing_List)
        Dim oOrdenVenta As SAPbobsCOM.Documents
        Dim strIDSucursal As String = String.Empty
        Dim strCerrarLineasOV As String = String.Empty

        Try
            '*************Objetos SAP *******************
            Dim oListaOrdenVenta As List(Of SAPbobsCOM.Documents) = New List(Of SAPbobsCOM.Documents)
            '***********Listas Genericas **********
            Dim oDocEntryOrdenVentaList As List(Of String) = New List(Of String)
            '*************Variables *********************
            Dim intDocEntry As Integer = 0
            Dim intResultado As Integer = 1
            Dim blnActualiza As Boolean = False
            SBO_Application.StatusBar.SetText(My.Resources.Resource.ActualizaOrdenVenta, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            CargarDocEntryOrdenVenta(p_oNoOrdenList, oDocEntryOrdenVentaList)
            For Each rowDocEntry As String In oDocEntryOrdenVentaList
                If Not String.IsNullOrEmpty(rowDocEntry) Then
                    oOrdenVenta = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders),  _
                                                           SAPbobsCOM.Documents)
                    intDocEntry = Convert.ToInt32(rowDocEntry)
                    If oOrdenVenta.GetByKey(intDocEntry) Then
                        strIDSucursal = oOrdenVenta.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()

                        If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal)) Then
                            With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal))
                                strCerrarLineasOV = .U_CloseSOL
                            End With
                        End If

                        If Not strCerrarLineasOV.Equals("N") Then

                            blnActualiza = False
                            If Not String.IsNullOrEmpty(oOrdenVenta.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                                For Each rowFactura As DocumentoMarketing In p_oLineaFacturaClienteList
                                    If oOrdenVenta.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString.Trim() = rowFactura.NoOrden Then
                                        For contador As Integer = 0 To oOrdenVenta.Lines.Count - 1
                                            oOrdenVenta.Lines.SetCurrentLine(contador)
                                            If oOrdenVenta.Lines.LineStatus <> SAPbobsCOM.BoStatus.bost_Close Then
                                                If oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString.Trim() = rowFactura.ID Then
                                                    oOrdenVenta.Lines.LineStatus = SAPbobsCOM.BoStatus.bost_Close
                                                    blnActualiza = True
                                                    Exit For
                                                End If
                                            End If
                                        Next
                                    End If
                                Next
                                If blnActualiza Then oListaOrdenVenta.Add(oOrdenVenta)
                            End If

                        End If
                    End If
                End If
            Next
            '****************Manejo Transaccion SAP ********************
            If oListaOrdenVenta.Count > 0 Then
                ResetTransaction()
                StartTransaction()
                For Each rowOrdenVenta As SAPbobsCOM.Documents In oListaOrdenVenta
                    intResultado = rowOrdenVenta.Update()
                    If intResultado <> 0 Then
                        RollbackTransaction()
                        Exit Sub
                    End If
                Next
                CommitTransaction()
            End If
        Catch ex As Exception
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oOrdenVenta)
        End Try
    End Sub

    Public Sub CargarDocEntryOrdenVenta(ByVal p_oListaNoOrden As Generic.List(Of String), _
                                        ByRef p_oListaOrdenVenta As Generic.List(Of String))
        Try
            Dim strNoOrden As String = String.Empty
            Dim strQuery As String = String.Empty
            Dim dtOrdenVenta As System.Data.DataTable
            Dim intDocEntry As Integer = 0

            For Each rowOT As String In p_oListaNoOrden
                If Not strNoOrden.Contains(rowOT) Then
                    strNoOrden = strNoOrden & String.Format("'{0}', ", rowOT)
                End If
            Next
            If (strNoOrden.Length > 0) Then
                strNoOrden = strNoOrden.Substring(0, strNoOrden.Length - 2)
                strQuery = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strDocEntryOrdenVenta"), strNoOrden)
                dtOrdenVenta = Utilitarios.EjecutarConsultaDataTable(strQuery)
            End If
            For Each rowCotizacion As DataRow In dtOrdenVenta.Rows
                If Not String.IsNullOrEmpty(rowCotizacion.Item("DocEntry")) Then
                    If Not p_oListaOrdenVenta.Contains(rowCotizacion.Item("DocEntry")) Then
                        p_oListaOrdenVenta.Add(rowCotizacion.Item("DocEntry"))
                    End If
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Public Function CargaFactura(ByVal p_intDocEntry As Integer, _
                                 ByRef p_oLineaFacturaClienteList As DocumentoMarketing_List, _
                                 ByRef p_oSucursalList As Generic.List(Of String), _
                                 ByRef p_oNoOrdenList As Generic.List(Of String), _
                                 ByRef p_oCodigoMarcaList As Generic.List(Of String), _
                                 ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                 ByRef p_oDatosGeneralesList As DatoGenerico_List) As Boolean
        Dim oFacturaCliente As SAPbobsCOM.Documents
        Try
            '**************Declaracion de data contract**********
            Dim oLineaFacturaCliente As DocumentoMarketing
            Dim oTipoOT As ConfiguracionOrdenTrabajo
            Dim oDatosGenerales As DatoGenerico
            '************Variables********************************
            Dim intTipoArticulo As Integer = 0
            Dim strTipoArticulo As String = String.Empty
            Dim strSucursal As String = String.Empty
            Dim strNoOrden As String = String.Empty
            Dim strCodigoMarca As String = String.Empty
            Dim blnProcesoAsientoFactura As Boolean = False
            Dim strMonedaLocal As String = String.Empty

            '****Consulta moneda local*********
            strMonedaLocal = ConsultaMonedaLocal()
            '************Verifica si DocEntry posee valor válido********************************
            If p_intDocEntry > 0 Then
                oFacturaCliente = CType(SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices),  _
                                                     SAPbobsCOM.Documents)

                '************Carga Objeto Factura de clientes********************************
                If oFacturaCliente.GetByKey(p_intDocEntry) Then
                    oDatosGenerales = New DatoGenerico
                    With oDatosGenerales
                        .DocEntry = oFacturaCliente.DocEntry
                        .DocNum = oFacturaCliente.DocNum
                        .FechaContabilizacion = oFacturaCliente.DocDate
                        .MonedaLocal = strMonedaLocal
                    End With
                    p_oDatosGeneralesList.Add(oDatosGenerales)
                    '********Recorre lineas de la factura***********************
                    For rowFactura As Integer = 0 To oFacturaCliente.Lines.Count - 1
                        oFacturaCliente.Lines.SetCurrentLine(rowFactura)
                        intTipoArticulo = 0
                        strTipoArticulo = String.Empty
                        strSucursal = String.Empty
                        strNoOrden = String.Empty
                        '************Valido si la linea pertenece a una OT********************************
                        If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                            If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString()) Then
                                intTipoArticulo = CInt(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                            Else
                                strTipoArticulo = DevuelveValorArticulo(oFacturaCliente.Lines.ItemCode, "U_SCGD_TipoArticulo")
                                If Not String.IsNullOrEmpty(strTipoArticulo) Then
                                    intTipoArticulo = CInt(strTipoArticulo)
                                End If
                            End If
                            oLineaFacturaCliente = New DocumentoMarketing()
                            With oLineaFacturaCliente
                                .ItemCode = oFacturaCliente.Lines.ItemCode
                                .BodegaOrigen = oFacturaCliente.Lines.WarehouseCode
                                .TipoArticulo = intTipoArticulo
                                .CostingCode = oFacturaCliente.Lines.CostingCode
                                .CostingCode2 = oFacturaCliente.Lines.CostingCode2
                                .CostingCode3 = oFacturaCliente.Lines.CostingCode3
                                .CostingCode4 = oFacturaCliente.Lines.CostingCode4
                                .CostingCode5 = oFacturaCliente.Lines.CostingCode5
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                    .NoOrden = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_ID").Value) Then
                                    .ID = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                    .TipoOT = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                ElseIf Not String.IsNullOrEmpty(oFacturaCliente.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                    .TipoOT = oFacturaCliente.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value) Then
                                    .CodigoProyecto = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_CodProy").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString()) Then
                                    .Costo = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()) Then
                                    .Sucursal = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value
                                ElseIf Not String.IsNullOrEmpty(oFacturaCliente.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                    .Sucursal = oFacturaCliente.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                                End If
                                If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value.ToString()) Then
                                    .CodigoMarca = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                ElseIf Not String.IsNullOrEmpty(oFacturaCliente.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                    .CodigoMarca = oFacturaCliente.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                End If
                            End With
                            p_oLineaFacturaClienteList.Add(oLineaFacturaCliente)

                            '***************Agrega Sucursal al List*************************
                            If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value) Then
                                strSucursal = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString()
                                If Not p_oSucursalList.Contains(strSucursal) Then
                                    p_oSucursalList.Add(strSucursal)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oFacturaCliente.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                                strSucursal = oFacturaCliente.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString()
                                If Not p_oSucursalList.Contains(strSucursal) Then
                                    p_oSucursalList.Add(strSucursal)
                                End If
                            End If
                            '**************Agrega NoOrden al List******************
                            If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value) Then
                                strNoOrden = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value
                                If Not p_oNoOrdenList.Contains(strNoOrden) Then
                                    p_oNoOrdenList.Add(strNoOrden)
                                End If
                            End If
                            '**************Agrega Codigo Marca al List******************
                            If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value) Then
                                strCodigoMarca = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                    p_oCodigoMarcaList.Add(strCodigoMarca)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oFacturaCliente.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value) Then
                                strCodigoMarca = oFacturaCliente.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value
                                If Not p_oCodigoMarcaList.Contains(strCodigoMarca) Then
                                    p_oCodigoMarcaList.Add(strCodigoMarca)
                                End If
                            End If
                            '**************Agrega TipoOT al List******************
                            If Not String.IsNullOrEmpty(oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value) Then
                                oTipoOT = New ConfiguracionOrdenTrabajo
                                With oTipoOT
                                    .TipoOT = oFacturaCliente.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value
                                End With
                                If Not p_oTipoOTList.Contains(oTipoOT) Then
                                    p_oTipoOTList.Add(oTipoOT)
                                End If
                            ElseIf Not String.IsNullOrEmpty(oFacturaCliente.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value) Then
                                oTipoOT = New ConfiguracionOrdenTrabajo
                                With oTipoOT
                                    .TipoOT = oFacturaCliente.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                                End With
                                If Not p_oTipoOTList.Contains(oTipoOT) Then
                                    p_oTipoOTList.Add(oTipoOT)
                                End If
                            End If
                            blnProcesoAsientoFactura = True
                        End If
                    Next
                End If
            End If
            Return blnProcesoAsientoFactura
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        Finally
            If Not oFacturaCliente Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oFacturaCliente)
                oFacturaCliente = Nothing
            End If

        End Try
    End Function

    Public Function ValidaUsaDimensionOfertaVentas(ByVal p_strIDSucursal As String, ByVal p_strTipoOT As String) As Boolean
        Dim strUsaDimensiones As String = String.Empty
        Dim strUsaDimensionesOFV As String = String.Empty
        Dim intTipoOT As Integer = 0
        Try
            If Not String.IsNullOrEmpty(p_strIDSucursal) And Not String.IsNullOrEmpty(p_strTipoOT) Then
                intTipoOT = Convert.ToInt32(p_strTipoOT)
                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal)) Then
                    With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(p_strIDSucursal))
                        If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)) Then
                            If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDim) Then strUsaDimensiones = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDim
                            If Not String.IsNullOrEmpty(strUsaDimensiones) Then
                                If strUsaDimensiones = "Y" Then
                                    If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDOFV) Then strUsaDimensionesOFV = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(intTipoOT)).U_UsaDOFV
                                    If Not String.IsNullOrEmpty(strUsaDimensionesOFV) Then
                                        If strUsaDimensionesOFV = "Y" Then
                                            Return True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End With
                End If
            End If

            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Public Sub AsignaCentrosCostoDimensiones(ByRef p_rowLineaFactura As DocumentoMarketing, _
                                             ByRef p_oListaTipoArticulo As DocumentoMarketing, _
                                             ByRef p_oTipoOTList As ConfiguracionOrdenTrabajo_List, _
                                             ByRef p_oDimensionesContablesList As DimensionesContables_List)
        Try
            For Each rowTipoOT As ConfiguracionOrdenTrabajo In p_oTipoOTList
                If p_rowLineaFactura.TipoOT = rowTipoOT.TipoOT Then
                    If rowTipoOT.UsaDimensiones Then
                        If ValidaUsaDimensionOfertaVentas(p_rowLineaFactura.Sucursal, p_rowLineaFactura.TipoOT) Then
                            With p_oListaTipoArticulo
                                .CostingCode = p_rowLineaFactura.CostingCode
                                .CostingCode2 = p_rowLineaFactura.CostingCode2
                                .CostingCode3 = p_rowLineaFactura.CostingCode3
                                .CostingCode4 = p_rowLineaFactura.CostingCode4
                                .CostingCode5 = p_rowLineaFactura.CostingCode5
                                .UsaDimensiones = True
                            End With
                        Else
                            For Each rowDimensionesContables As DimensionesContables In p_oDimensionesContablesList
                                If p_rowLineaFactura.Sucursal = rowDimensionesContables.Sucursal And p_rowLineaFactura.CodigoMarca = rowDimensionesContables.CodigoMarca Then
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
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub CargaListasTipoArticulo(ByRef p_oLineaFacturaClienteList As DocumentoMarketing_List, _
                                       ByRef p_oServicioExternoList As DocumentoMarketing_List, _
                                       ByRef p_oServicioList As DocumentoMarketing_List, _
                                       ByRef p_oOtroGastoList As DocumentoMarketing_List, _
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

            For Each rowLineaFactura As DocumentoMarketing In p_oLineaFacturaClienteList
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
                        Case TipoArticulo.Servicio
                            If p_rowConfiguracionSucursal.UsaCosteoManoObra Then
                                oServicio = New DocumentoMarketing()
                                With oServicio
                                    .ItemCode = rowLineaFactura.ItemCode
                                    .BodegaOrigen = rowLineaFactura.BodegaOrigen
                                    .TipoArticulo = rowLineaFactura.TipoArticulo
                                    .NoOrden = rowLineaFactura.NoOrden
                                    .TipoOT = rowLineaFactura.TipoOT
                                    .CodigoProyecto = rowLineaFactura.CodigoProyecto
                                    .Costo = rowLineaFactura.Costo
                                    .Sucursal = rowLineaFactura.Sucursal
                                    .CodigoMarca = rowLineaFactura.CodigoMarca
                                    If Not String.IsNullOrEmpty(p_rowConfiguracionSucursal.MonedaManoObra) Then
                                        .MonedaManoObra = p_rowConfiguracionSucursal.MonedaManoObra
                                    End If
                                    If Not String.IsNullOrEmpty(p_rowConfiguracionSucursal.CuentaCreditoManoObra) Then
                                        .CuentaCreditoManoObra = p_rowConfiguracionSucursal.CuentaCreditoManoObra
                                    End If
                                    '*********************Valida que usa dimensiones y asigna centro de costo*********
                                    If p_rowConfiguracionSucursal.UsaDimensiones Then
                                        AsignaCentrosCostoDimensiones(rowLineaFactura, oServicio, p_oTipoOTList, p_oDimensionesContablesList)
                                    End If
                                End With
                                p_oServicioList.Add(oServicio)
                            End If
                        Case TipoArticulo.OtrosCostosGastos
                            If p_rowConfiguracionSucursal.UsaAsientosGastos Then
                                oOtroGasto = New DocumentoMarketing()
                                With oOtroGasto
                                    .ItemCode = rowLineaFactura.ItemCode
                                    .BodegaOrigen = rowLineaFactura.BodegaOrigen
                                    .TipoArticulo = rowLineaFactura.TipoArticulo
                                    .NoOrden = rowLineaFactura.NoOrden
                                    .TipoOT = rowLineaFactura.TipoOT
                                    .CodigoProyecto = rowLineaFactura.CodigoProyecto
                                    .Costo = rowLineaFactura.Costo
                                    .Sucursal = rowLineaFactura.Sucursal
                                    .CodigoMarca = rowLineaFactura.CodigoMarca
                                    If Not String.IsNullOrEmpty(p_rowConfiguracionSucursal.MonedaOtrosGastos) Then
                                        .MonedaOtrosGastos = p_rowConfiguracionSucursal.MonedaOtrosGastos
                                    End If
                                    If Not String.IsNullOrEmpty(p_rowConfiguracionSucursal.CuentaCreditoOtrosGastos) Then
                                        .CuentaCreditoOtrosGastos = p_rowConfiguracionSucursal.CuentaCreditoOtrosGastos
                                    End If
                                    '*********************Valida que usa dimensiones y asigna centro de costo*********
                                    If p_rowConfiguracionSucursal.UsaDimensiones Then
                                        AsignaCentrosCostoDimensiones(rowLineaFactura, oOtroGasto, p_oTipoOTList, p_oDimensionesContablesList)
                                    End If
                                End With
                                p_oOtroGastoList.Add(oOtroGasto)
                            End If
                    End Select
                End If
            Next
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub
    Public Sub ManejaAsientosFacturaCliente(ByVal p_strDocEntry As String)
        Try
            '**********DataContract****************
            Dim oConfiguracionSucursalList As ConfiguracionSucursal_List = New ConfiguracionSucursal_List
            Dim oLineaFacturaClienteList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oServicioExternoList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oServicioList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oOtroGastoList As DocumentoMarketing_List = New DocumentoMarketing_List
            Dim oDimensionesContablesList As DimensionesContables_List = New DimensionesContables_List
            Dim oDatosGeneralesList As DatoGenerico_List = New DatoGenerico_List
            Dim oAsientoServicioList As Asiento_List = New Asiento_List
            Dim oAsientoServicioExternoList As Asiento_List = New Asiento_List
            Dim oAsientoOtrosGastosList As Asiento_List = New Asiento_List
            '********Listas genericas*************
            Dim oSucursalList As List(Of String) = New Generic.List(Of String)
            Dim oNoOrdenList As List(Of String) = New Generic.List(Of String)
            Dim oCodigoMarcaList As List(Of String) = New Generic.List(Of String)
            Dim oTipoOTList As ConfiguracionOrdenTrabajo_List = New ConfiguracionOrdenTrabajo_List
            '*************Clases**************************
            Dim ClsLineasDocumentosDimension As AgregarDimensionLineasDocumentosCls = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            '**********Declaración Variables*****************
            Dim blnProcesaAsientosFacturaCliente As Boolean = False
            Dim strMonedaLocal As String = String.Empty
            Dim blnDimensionesYaCargadas As Boolean = False
            Dim blnAsientoServicioExitoso As Boolean = False
            Dim blnAsientoServicioExternoExitoso As Boolean = False
            Dim blnAsientoOtrosGastosExitoso As Boolean = False
            Dim blnMensajeServicioExitoso As Boolean = False
            Dim blnMensajeServicioExternoExitoso As Boolean = False
            Dim blnMensajeOtrosGastosExitoso As Boolean = False
            '********Carga información lineas de factura*************
            If Not String.IsNullOrEmpty(p_strDocEntry) Then
                blnProcesaAsientosFacturaCliente = CargaFactura(CInt(p_strDocEntry), oLineaFacturaClienteList, oSucursalList, oNoOrdenList, oCodigoMarcaList, oTipoOTList, oDatosGeneralesList)
            End If
            '********Valida si existen lineas en la factura de clientes que sean de tipo(Servicio Externo, Servicio o otros gastos) que esten ligadas a una OT y que necesite procesar para saber si genera asiento*************
            If blnProcesaAsientosFacturaCliente Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesaAsientoFacturaClientes, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Warning)
                '********Carga configuración sucursal*************
                If oSucursalList.Count > 0 Then
                    CargaConfiguracionSucursal(oSucursalList, oConfiguracionSucursalList)
                End If
                '********Si a nivel de compañia se usan dimensiones, valida si lo hace a nivel de Tipo OT*************
                For Each rowConfiguracionSucursal As ConfiguracionSucursal In oConfiguracionSucursalList
                    If rowConfiguracionSucursal.UsaAsientoServicioExterno Or rowConfiguracionSucursal.UsaAsientosGastos Or rowConfiguracionSucursal.UsaCosteoManoObra Then
                        If rowConfiguracionSucursal.UsaDimensiones Then
                            If oTipoOTList.Count > 0 Then
                                ValidaUsaDimensionesTipoOT(oTipoOTList)
                            End If
                            If Not blnDimensionesYaCargadas Then
                                ClsLineasDocumentosDimension.CargaCentrosCostoDimensionesOT(oSucursalList, oCodigoMarcaList, oDimensionesContablesList)
                                blnDimensionesYaCargadas = True
                            End If
                        End If
                        CargaListasTipoArticulo(oLineaFacturaClienteList, oServicioExternoList, oServicioList, oOtroGastoList, oTipoOTList, rowConfiguracionSucursal, oDimensionesContablesList)
                    End If
                Next
                ProcesaAsientoServicio(oServicioList, oAsientoServicioList)
                ProcesaAsientoServicioExterno(oServicioExternoList, oAsientoServicioExternoList)
                ProcesaAsientoOtrosCostosGastos(oOtroGastoList, oAsientoOtrosGastosList)
                'Cierra las líneas de la orden de venta
                ActualizaValoresOrdenVenta(oNoOrdenList, oLineaFacturaClienteList)

                If oAsientoServicioList.Count() > 0 Or oAsientoServicioExternoList.Count() > 0 Or oAsientoOtrosGastosList.Count() > 0 Then
                    '****************Maneja transacción**************
                    ResetTransaction()
                    StartTransaction()
                    '************Verifica si genera asiento para servicio****************
                    If oAsientoServicioList.Count > 0 Then
                        If CrearAsiento(oDatosGeneralesList, oAsientoServicioList, TipoArticulo.Servicio) > 0 Then
                            blnAsientoServicioExitoso = True
                            blnMensajeServicioExitoso = True
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Sub
                        End If
                    Else
                        blnAsientoServicioExitoso = True
                        blnMensajeServicioExitoso = False
                    End If
                    '************Verifica si genera asiento para servicio externo****************
                    If oAsientoServicioExternoList.Count > 0 Then
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
                    '************Verifica si genera asiento para otros gastos****************
                    If oAsientoOtrosGastosList.Count > 0 Then
                        If CrearAsiento(oDatosGeneralesList, oAsientoOtrosGastosList, TipoArticulo.OtrosCostosGastos) > 0 Then
                            blnAsientoOtrosGastosExitoso = True
                            blnMensajeOtrosGastosExitoso = True
                        Else
                            RollbackTransaction()
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoOtrosGastosError, SAPbouiCOM.BoMessageTime.bmt_Long, BoStatusBarMessageType.smt_Error)
                            Exit Sub
                        End If
                    Else
                        blnAsientoOtrosGastosExitoso = True
                        blnMensajeOtrosGastosExitoso = False
                    End If
                    If blnAsientoServicioExitoso And blnAsientoServicioExternoExitoso And blnAsientoOtrosGastosExitoso Then
                        '*****************Realiza commit ala transaccion**************
                        CommitTransaction()
                        '*****************Mensaje asiento generado correctamente*****************
                        If blnMensajeServicioExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        If blnMensajeServicioExternoExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoServicioExternoExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        If blnMensajeOtrosGastosExitoso Then SBO_Application.StatusBar.SetText(My.Resources.Resource.AsientoOtrosGastosExitoso, SAPbouiCOM.BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                    Else
                        RollbackTransaction()
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

            For Each rowGeneral As DatoGenerico In p_oDatosGeneralesList
                With rowGeneral
                    intDocEntry = .DocEntry
                    intDocNum = .DocNum
                    dateFechaContabilizacion = .FechaContabilizacion
                    strMonedaLocal = .MonedaLocal
                End With
                Exit For
            Next

            If p_oAsientoList.Count > 0 Then
                oJournalEntry = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                If Not dateFechaContabilizacion = Nothing Then
                    oJournalEntry.ReferenceDate = dateFechaContabilizacion
                End If

                Select Case p_intTipoArticulo
                    Case TipoArticulo.Servicio
                        oJournalEntry.Memo = My.Resources.Resource.AsientoManoObra + ": " + intDocNum.ToString()
                    Case TipoArticulo.ServicioExterno
                        oJournalEntry.Memo = My.Resources.Resource.AsientoServiciosExternos + intDocNum.ToString()
                    Case TipoArticulo.OtrosCostosGastos
                        oJournalEntry.Memo = My.Resources.Resource.AsientoOtrosGastos + ": " + intDocNum.ToString()
                End Select
                oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = intDocEntry.ToString()


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
                    strCuentaDebito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.SaleCostAc, rowServicioExterno.Sucursal, rowServicioExterno.BodegaOrigen)
                    strCuentaCredito = Utilitarios.ObtenerCuentaContable(Utilitarios.TiposArticulos.scgServicioExt, Utilitarios.Account.ExpensesAc, rowServicioExterno.Sucursal, rowServicioExterno.BodegaOrigen)
                    ''******Cuenta debito y cuenta credito************
                    'If Not String.IsNullOrEmpty(rowServicioExterno.ItemCode) And Not String.IsNullOrEmpty(rowServicioExterno.BodegaOrigen) Then
                    '    strCuentaDebito = ObtenerCuentaArticulo(rowServicioExterno.ItemCode, rowServicioExterno.BodegaOrigen, "SaleCostAc")
                    '    strCuentaCredito = ObtenerCuentaArticulo(rowServicioExterno.ItemCode, rowServicioExterno.BodegaOrigen, "ExpensesAc")
                    'End If
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
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.CuentaCredito = rowAsiento1.CuentaCredito And rowAsiento2.CostingCode = rowAsiento1.CostingCode And rowAsiento2.CostingCode2 = rowAsiento1.CostingCode2 And rowAsiento2.CostingCode3 = rowAsiento1.CostingCode3 And rowAsiento2.CostingCode4 = rowAsiento1.CostingCode4 And rowAsiento2.CostingCode5 = rowAsiento1.CostingCode5 And rowAsiento2.Aplicado = False Then
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
                    'If Not String.IsNullOrEmpty(rowServicio.ItemCode) And Not String.IsNullOrEmpty(rowServicio.BodegaOrigen) Then
                    'strCuentaDebito = ObtenerCuentaArticulo(rowServicio.ItemCode, rowServicio.BodegaOrigen, "SaleCostAc")
                    'End If
                    strCuentaDebito = Utilitarios.ObtenerCuentaContableArticulo(Utilitarios.TiposArticulos.scgActividad, rowServicio.ItemCode, Utilitarios.Account.SaleCostAc, rowServicio.Sucursal, rowServicio.BodegaOrigen)
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(rowServicio.Sucursal) Then oLineaAsientoTemporal.IDSucursal = rowServicio.Sucursal
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
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.CostingCode = rowAsiento1.CostingCode And rowAsiento2.CostingCode2 = rowAsiento1.CostingCode2 And rowAsiento2.CostingCode3 = rowAsiento1.CostingCode3 And rowAsiento2.CostingCode4 = rowAsiento1.CostingCode4 And rowAsiento2.CostingCode5 = rowAsiento1.CostingCode5 And rowAsiento2.Aplicado = False Then
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
                    'If Not String.IsNullOrEmpty(rowOtroGasto.ItemCode) And Not String.IsNullOrEmpty(rowOtroGasto.BodegaOrigen) Then
                    '    strCuentaDebito = ObtenerCuentaArticulo(rowOtroGasto.ItemCode, rowOtroGasto.BodegaOrigen, "SaleCostAc")
                    'End If
                    strCuentaDebito = Utilitarios.ObtenerCuentaContableArticulo(Utilitarios.TiposArticulos.scgOtrosGastos_Costos, rowOtroGasto.ItemCode, Utilitarios.Account.SaleCostAc, rowOtroGasto.Sucursal, rowOtroGasto.BodegaOrigen)
                    If Not String.IsNullOrEmpty(strCuentaDebito) Then
                        .CuentaDebito = strCuentaDebito
                    Else
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                    End If
                    If Not String.IsNullOrEmpty(rowOtroGasto.Sucursal) Then oLineaAsientoTemporal.IDSucursal = rowOtroGasto.Sucursal
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
                    If rowAsiento2.NoOrden = rowAsiento1.NoOrden And rowAsiento2.CuentaDebito = rowAsiento1.CuentaDebito And rowAsiento2.CostingCode = rowAsiento1.CostingCode And rowAsiento2.CostingCode2 = rowAsiento1.CostingCode2 And rowAsiento2.CostingCode3 = rowAsiento1.CostingCode3 And rowAsiento2.CostingCode4 = rowAsiento1.CostingCode4 And rowAsiento2.CostingCode5 = rowAsiento1.CostingCode5 And rowAsiento2.Aplicado = False Then
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
#End Region

    Public Function ValidaGeneraAsientoServicioExterno() As Boolean
        Try
            Dim strCreaAsiento As String = String.Empty
            Dim blnCreaAsiento As Boolean = False

            blnCreaAsiento = False
            strCreaAsiento = Utilitarios.EjecutarConsulta("Select U_GenAsSE from dbo.[@SCGD_ADMIN] with (nolock)", SBO_Company.CompanyDB, SBO_Company.Server)
            If String.IsNullOrEmpty(strCreaAsiento) Then
                blnCreaAsiento = False
            ElseIf strCreaAsiento = "Y" Then
                blnCreaAsiento = True
            End If
            Return blnCreaAsiento
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function FinalizaTransaccion(Optional ByVal p_DocEntry As String = "") As Boolean
        Try
            Dim dtConfSucursal As System.Data.DataTable
            Dim drwConfSucursal As System.Data.DataRow
            Dim idSucursal As String = String.Empty
            Dim usaTEstandar As String
            Dim blnCreaAsientoMO As String = False
            Dim decMonto As Decimal
            Dim strNoOrden As String = String.Empty
            Dim strMoneda As String = String.Empty
            Dim strCuentaAcredita As String = String.Empty
            Dim strUsaTiempoEstandar As String = String.Empty
            Dim blnCreaAsientoGastos As String = False
            Dim strCuentaDebitaGastos As String = String.Empty
            Dim strMonedaGastos As String = String.Empty
            Dim blnCreaAsientoServicioExterno As Boolean = False

            blnCreaAsientoServicioExterno = ValidaGeneraAsientoServicioExterno()

            idSucursal = FormFacPro.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0)

            If idSucursal <> String.Empty Then

                dtConfSucursal = Utilitarios.EjecutarConsultaDataTable(String.Format("Select U_Sucurs,U_CosteoMO_C, U_TiempoEst_C, U_TiempoReal_C, U_Moneda_C, U_CuentaSys_C, U_DescCuenta_C, U_GenASGastos,U_MonDocGastos,U_CtaDebGast From [@SCGD_CONF_SUCURSAL] with (nolock) Where U_Sucurs = '{0}'",
                                                        idSucursal.Trim()),
                                                        SBO_Company.CompanyDB,
                                                        SBO_Company.Server)
                If dtConfSucursal.Rows.Count > 0 Then
                    drwConfSucursal = dtConfSucursal.Rows(0)

                    'Costeo de Mano de Obra
                    If drwConfSucursal.Item("U_CosteoMO_C").ToString.Trim() = "Y" Then

                        strMoneda = drwConfSucursal.Item("U_Moneda_C").ToString.Trim()
                        strCuentaAcredita = drwConfSucursal.Item("U_CuentaSys_C").ToString.Trim()

                        If Not String.IsNullOrEmpty(strMoneda) And Not String.IsNullOrEmpty(strCuentaAcredita) Then
                            blnCreaAsientoMO = True
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfManoObraCliente, SAPbouiCOM.BoMessageTime.bmt_Short)
                            blnCreaAsientoMO = False
                        End If
                    Else
                        blnCreaAsientoMO = False
                    End If
                    'Costeo de Articulos Otros Gastos
                    If drwConfSucursal.Item("U_GenASGastos").ToString.Trim() = "Y" Then

                        strMonedaGastos = drwConfSucursal.Item("U_MonDocGastos").ToString.Trim()
                        strCuentaDebitaGastos = drwConfSucursal.Item("U_CtaDebGast").ToString.Trim()

                        If Not String.IsNullOrEmpty(strMonedaGastos) And Not String.IsNullOrEmpty(strCuentaDebitaGastos) Then
                            blnCreaAsientoGastos = True
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfOtrosGastos, SAPbouiCOM.BoMessageTime.bmt_Short)
                            blnCreaAsientoGastos = False
                        End If
                    Else
                        blnCreaAsientoGastos = False
                    End If
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ConfOtrosGastos, SAPbouiCOM.BoMessageTime.bmt_Short)
                    blnCreaAsientoMO = False
                    blnCreaAsientoGastos = False
                End If
            End If

            If Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OINV").GetValue("DocDate", 0)) AndAlso
                            Not String.IsNullOrEmpty(FormFacPro.DataSources.DBDataSources.Item("OINV").GetValue("CardCode", 0)) AndAlso
                            FormFacPro.DataSources.DBDataSources.Item("INV1").Size > 0 Then

                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    strNoAsiento = String.Empty
                End If

                DocEntryFacturaCliente = p_DocEntry

                'inicia transaccionmanager  
                SBO_Company.StartTransaction()
                'Crea Asiento Mano de Obra
                If blnCreaAsientoMO = True Then
                    intNoAsientoMO = CrearAsientoManoObra(SBO_Company, strMoneda, strCuentaAcredita, strUsaTiempoEstandar, FormFacPro)
                End If
                'Crea Asiento Otros Gastos
                If blnCreaAsientoGastos = True Then
                    intNoAsientoGastos = CrearAsientoOtrosGastos(SBO_Company, strMonedaGastos, strCuentaDebitaGastos, FormFacPro)
                End If
                'Crea Asiento Servicios Externos
                If blnCreaAsientoServicioExterno Then
                    intNoAsientoSE = CrearAsientoServicioExterno(SBO_Company, FormFacPro)
                End If
            Else
                'strNoAsiento = String.Empty
                intNoAsientoSE = 0
                intNoAsientoMO = 0
                blnCreaAsientoMO = False
                blnCreaAsientoGastos = False
                intNoAsientoGastos = 0
            End If
            If intNoAsientoMO <> 0 Or intNoAsientoGastos <> 0 Or intNoAsientoSE <> 0 Then
                'commit en la transaccion 
                SBO_Company.EndTransaction(BoWfTransOpt.wf_Commit)
                'strNoAsiento = String.Empty
                intNoAsientoSE = 0
                intNoAsientoMO = 0
                blnCreaAsientoMO = False
                blnCreaAsientoGastos = False
                intNoAsientoGastos = 0
            Else
                If SBO_Company.InTransaction Then
                    SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                    'strNoAsiento = String.Empty
                    intNoAsientoSE = 0
                    intNoAsientoMO = 0
                    blnCreaAsientoMO = False
                    blnCreaAsientoGastos = False
                    intNoAsientoGastos = 0
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                'strNoAsiento = String.Empty
                intNoAsientoSE = 0
                intNoAsientoMO = 0
                intNoAsientoGastos = 0
            End If
        Finally
            If SBO_Company.InTransaction Then
                SBO_Company.EndTransaction(BoWfTransOpt.wf_RollBack)
                'strNoAsiento = String.Empty
                intNoAsientoSE = 0
                intNoAsientoMO = 0
                intNoAsientoGastos = 0
            End If
        End Try
    End Function

    <System.CLSCompliant(False)> _
    Public Function CrearAsientoFacturaClientes(ByRef ocompany As SAPbobsCOM.Company,
                                                         ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO

        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strNoAsiento As String
        Dim decAjuste As Decimal
        Dim strContraCuenta As String
        Dim strTipo As String

        'Bodegas por servicios externos 
        Dim htBodegas_SE As New Hashtable
        'servicios externos
        Dim oListaSE As IList(Of String) = New Generic.List(Of String)

        'Lista para almacenar el numero de Orden de Trabajo
        Dim oListaNumeroOT As IList(Of String) = New Generic.List(Of String)
        Dim oListaBaseRef As IList(Of String) = New Generic.List(Of String)
        Dim oListaBodegasServiciosExternos As IList(Of String) = New Generic.List(Of String)
        Dim oListaIdRepxOrd As IList(Of String) = New Generic.List(Of String)



        'Entrega de recibidas no facturadas
        Dim htCuentas_Debe As New Hashtable
        'Servicios externos por asignar
        Dim htCuentas_Haber As New Hashtable
        Dim ExistenCostos As Boolean

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

        'manejo de impuestos 
        Dim strCodeImp As String = ""
        Dim strCtaImp As String = ""
        Dim strCantImpuestos As String = ""
        Dim dcICantImpuestos As Decimal = 0
        Dim dcICantImpuestosAcumulado As Decimal = 0
        Dim dcImp As Decimal = 0

        Dim dcValorRetorno As Decimal = 0
        Dim strMemo As String = ""
        Dim strNumeroOT As String = String.Empty
        Dim strBaseRef As String = String.Empty

        Dim oListaNumeroOTValidados As IList(Of String) = New Generic.List(Of String)

        Dim strTipoOT As String = String.Empty

        Dim blnAgregarDimension As Boolean = False

        Dim DataTableValoresCotizacion As System.Data.DataTable

        ValidarConfiguracionDimensiones(oForm)

        'carga servicios externos
        If Not oForm.DataSources.DBDataSources.Item("INV1") Is Nothing Then
            oListaBodegasServiciosExternos = CargaServiciosExternos(oForm, oListaSE, oListaNumeroOT, oListaBaseRef, oListaIdRepxOrd)
        End If

        If oForm.DataSources.DataTables.Item("SE").Rows.Count > 0 Then
            ObtieneCuentasYBodegas(oListaSE, htCuentas_Debe, htCuentas_Haber, oForm, htBodegas_SE, oListaBodegasServiciosExternos)
            ObtieneImpuestos(oForm)
        End If
        strNoAsiento = 0

        'If htBodegas_SE.Count > 0 Then

        oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)

        oJournalEntry.Reference = oForm.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_Numero_OT", 0).Trim()
        strFechaEntrada = oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocDate", 0).Trim()

        If Not String.IsNullOrEmpty(DocEntryFacturaCliente) Then
            oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = DocEntryFacturaCliente
        Else

            oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = String.Empty
        End If

        strMemo = My.Resources.Resource.AsientoFacturaClientes +
        oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocNum", 0).Trim()

        strMonedaEntrada = oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocCur", 0).Trim()
        strTipoCambioEntrada = oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocRate", 0).Trim()

        strMonedaLocal = RetornarMonedaLocal()

        oJournalEntry.Memo = strMemo
        Dim Contador As Integer = 0

        Dim row As System.Data.DataRow

        'SERVICIO EXTERNO ************************************************************** 

        For j As Integer = 0 To oListaNumeroOT.Count - 1

            Dim strNoOt As String = oListaNumeroOT.Item(j)

            If blnUsaDimensiones Then

                If Not oListaNumeroOTValidados.Contains(strNoOt) Then
                    oListaNumeroOTValidados.Add(strNoOt)
                    DataTableValoresCotizacion = Utilitarios.EjecutarConsultaDataTable("select Q.U_SCGD_Tipo_OT, Q.U_SCGD_idSucursal, Q.U_SCGD_Cod_Marca from OQUT Q  where Q.U_SCGD_Numero_OT = '" & strNoOt & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                    row = DataTableValoresCotizacion.Rows(0)
                End If

                strTipoOT = row.Item(0).ToString

                Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOT, False, False)
                '******************************************************************************************
                'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                If Not String.IsNullOrEmpty(strValorDimension) Then
                    If strValorDimension = "Y" Then
                        oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(oForm, row.Item(1), row.Item(2), oDataTableDimensionesContablesDMS))

                        If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                            blnAgregarDimension = True
                        End If

                    End If
                End If
                '******************************************************************************************
            End If

            ExistenCostos = False
            'For Each ServExt As String In oListaSE

            dcValorRetorno = 0
            'strPrecioML = RetornaCampo(oForm, Contador, "LineTotal", True, False)
            dcPrecioML = RetornaCampo(oForm, Contador, "LineTotal", True, False, strNoOt)
            'dcPrecioML = Decimal.Parse(strPrecioML)

            strPrecioME = RetornaCampo(oForm, Contador, "TotalFrgn", True, False, strNoOt)
            dcPrecioME = Decimal.Parse(strPrecioME)

            Contador = Contador + 1
            dcPrecioAcumuladoML = Decimal.Parse(dcPrecioAcumuladoML) + Decimal.Parse(dcPrecioML)
            If dcPrecioAcumuladoML > 0 Then
                ExistenCostos = True
            End If

            ''''dcPrecioAcumuladoME = Decimal.Parse(dcPrecioAcumuladoME) + Decimal.Parse(dcPrecioME)
            'Next

            'SERVICIO EXTERNO ************************************************************** 

            'GENERA ASIENTOS ****************************************************************
            oJournalEntry.Lines.Reference1 = strNoOt

            oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaHaber", 0).Trim()
            dcValorRetorno = 0

            oJournalEntry.Lines.Debit = Decimal.Parse(dcPrecioAcumuladoML)

            'oJournalEntry.Lines.FCDebit = Decimal.Parse(dcPrecioAcumuladoME)
            'oJournalEntry.Lines.FCCurrency = strMonedaEntrada

            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            If blnAgregarDimension Then
                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
            End If

            oJournalEntry.Lines.Add()

            'COSTOS ************************************************************************ 

            oJournalEntry.Lines.AccountCode = oForm.DataSources.DataTables.Item("SE").GetValue("CtaDebe", 0).Trim()
            oJournalEntry.Lines.Credit = Decimal.Parse(dcPrecioAcumuladoML)
            oJournalEntry.Lines.Reference1 = strNoOt
            'oJournalEntry.Lines.FCCredit = dcPrecioAcumulado
            'oJournalEntry.Lines.FCCurrency = strMonedaEntrada
            'oJournalEntry.Lines.Reference1 = strRef1
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            If blnAgregarDimension Then
                ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
            End If

            oJournalEntry.Lines.Add()

            dcPrecioML = 0
            dcPrecioAcumuladoML = 0
            dcICantImpuestos = 0
            dcICantImpuestosAcumulado = 0

            'COSTOS *************************************************************************
        Next

        If ExistenCostos Then
            'GENERA ASIENTOS ****************************************************************
            If oJournalEntry.Add <> 0 Then
                strNoAsiento = "0"
                ocompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else
                dcPrecioML = 0
                dcPrecioAcumuladoML = 0
                dcICantImpuestos = 0
                dcICantImpuestosAcumulado = 0
                oListaBaseRef.Clear()
                oListaBodegasServiciosExternos.Clear()
                oListaNumeroOT.Clear()
                oListaSE.Clear()
                oListaIdRepxOrd.Clear()
                ocompany.GetNewObjectCode(strNoAsiento)
            End If
        End If

        'End If


        Return CInt(strNoAsiento)

    End Function

    ''' <summary>
    ''' Carga datatable con Codigos y Cuentas de impuestos 
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub ObtieneImpuestos(ByVal oForm As Form)

        Dim Existe As Boolean

        Existe = False
        If oForm.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To oForm.DataSources.DataTables.Count - 1
                If oForm.DataSources.DataTables.Item(i).UniqueID = "IMP" Then
                    oForm.DataSources.DataTables.Item("IMP").Clear()
                    dtImpuestos.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
                    dtImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
                    Existe = True
                    Exit For
                End If
            Next
        End If

        If Not Existe Then
            dtImpuestos = oForm.DataSources.DataTables.Add("IMP")
            dtImpuestos.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
            dtImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
        End If

        dtImpuestos.ExecuteQuery("SELECT Code , SalesTax FROM OSTA")

    End Sub

    ''' <summary>
    ''' crea un datatable para manejo de la informaicon de impuestos 
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub CreaDTInfoImp(ByVal oForm As Form)
        Dim Existe As Boolean

        Existe = False
        If oForm.DataSources.DataTables.Count > 0 Then
            For i As Integer = 0 To oForm.DataSources.DataTables.Count - 1
                If oForm.DataSources.DataTables.Item(i).UniqueID = "INFOIMP" Then
                    oForm.DataSources.DataTables.Item("INFOIMP").Clear()
                    dtInfoImpuestos.Columns.Add("SE", BoFieldsType.ft_AlphaNumeric, 100)
                    dtInfoImpuestos.Columns.Add("ImpCode", BoFieldsType.ft_AlphaNumeric, 100)
                    dtInfoImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
                    dtInfoImpuestos.Columns.Add("LineVat", BoFieldsType.ft_AlphaNumeric, 100)
                    Existe = True
                    Exit For
                End If
            Next
        End If

        If Not Existe Then
            dtInfoImpuestos = oForm.DataSources.DataTables.Add("INFOIMP")
            dtInfoImpuestos.Columns.Add("SE", BoFieldsType.ft_AlphaNumeric, 100)
            dtInfoImpuestos.Columns.Add("ImpCode", BoFieldsType.ft_AlphaNumeric, 100)
            dtInfoImpuestos.Columns.Add("SalesTax", BoFieldsType.ft_AlphaNumeric, 100)
            dtInfoImpuestos.Columns.Add("LineVat", BoFieldsType.ft_AlphaNumeric, 100)
        End If
    End Sub

    ''' <summary>
    ''' Carga Un datatable con la informacion de los servicios externos
    ''' Asi como un HashTable con las bodegas de cada servicio externo
    ''' </summary>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <param name="oListaSE">Lista con los servicios externos</param>
    ''' <returns>HashTable con las bodegas de cada servicio externo</returns>
    ''' <remarks></remarks>
    Private Function CargaServiciosExternos(ByVal oForm As SAPbouiCOM.Form,
                                            ByRef oListaSE As IList(Of String), Optional ByRef oListaNumeroOT As IList(Of String) = Nothing, _
                                            Optional ByRef oListaBaseRef As IList(Of String) = Nothing, _
                                            Optional ByRef oListaIdRepxOrd As List(Of String) = Nothing) As Generic.List(Of String)

        'servicios externos
        Dim oBodegas_SE As New Hashtable

        Dim listaBodegas_SE As Generic.List(Of String) = New Generic.List(Of String)
        Dim listaIdRxO As Generic.List(Of String) = New Generic.List(Of String)

        Dim strTipoArticulo As String = ""
        Dim strInventariable As String = ""
        Dim strNoOT As String = ""
        Dim strNoArticulo As String = ""
        Dim strCostoML As String = ""
        Dim strCostoME As String = ""
        Dim dcCostoML As Decimal = 0
        Dim dcCostoME As Decimal = 0
        Dim strImpML As String = ""
        Dim strImpME As String = ""
        Dim dcImpML As Decimal = 0
        Dim dcImpME As Decimal = 0
        Dim Contador As Integer = 0
        Dim artContabilizado As Boolean = False
        Dim CostosSExFP As String = ""
        Dim strTabla As String = ""
        Dim strTablaHija As String = ""

        Dim strBodProceso As String = String.Empty
        Dim strNombreTaller As String = String.Empty

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0).Trim(), strNombreTaller)

        Dim strConsultaBodProcXCC As String =
            " select Proceso " & _
            " from [dbo].[SCGTA_TB_ConfBodegasXCentroCosto] as ccc " & _
            " inner join [dbo].[SCGTA_VW_OITM] as itm " & _
            " on ccc.IDCentroCosto = itm.[U_SCGD_CodCtroCosto] where itm.ItemCode = '{0}'"

        oForm.DataSources.DataTables.Item("SE").Rows.Clear()

        strNoOT = ""
        strNoOT = oForm.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_Numero_OT", 0).Trim()

        'relaiza los costos para se por FACTURA PROVEEDOR
        CostosSExFP = Utilitarios.EjecutarConsulta("SELECT U_CostSExFP FROM [@SCGD_ADMIN]",
                                                   SBO_Company.CompanyDB,
                                                   SBO_Company.Server)

        If CostosSExFP = "Y" Then
            strTabla = "OPCH"
            strTablaHija = "PCH1"
        ElseIf CostosSExFP = "N" Then
            strTabla = "OPDN"
            strTablaHija = "PDN1"
        End If

        For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("INV1").Size - 1

            strNoOT = oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim()

            strTipoArticulo = ""
            strTipoArticulo =
                Utilitarios.EjecutarConsulta(
                    String.Format("SELECT U_SCGD_TipoArticulo FROM OITM WHERE ItemCode = '{0}'",
                    oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()),
               SBO_Company.CompanyDB,
               SBO_Company.Server)
            strInventariable = ""
            strInventariable =
                Utilitarios.EjecutarConsulta(
                    String.Format("SELECT InvntItem FROM OITM WHERE ItemCode = '{0}'",
                    oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()),
                SBO_Company.CompanyDB,
                SBO_Company.Server)
            strNoArticulo = ""
            strNoArticulo = oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()
            strNoArticulo = strNoArticulo.Trim

            'costo moneda local
            strCostoML = ""
            strCostoML =
                Utilitarios.EjecutarConsulta(
                    String.Format("SELECT SUM(P1.LineTotal) FROM {0} AS P INNER JOIN {1} AS P1 ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{2}' AND P1.ItemCode = '{3}' and P1.U_SCGD_IdRepxOrd = '{4}' GROUP BY P1.ItemCode  ",
                    strTabla,
                    strTablaHija,
                    strNoOT,
                    strNoArticulo,
                    oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_IdRepxOrd", i).Trim()),
                SBO_Company.CompanyDB,
                SBO_Company.Server)
            dcCostoML = 0
            If Not String.IsNullOrEmpty(strCostoML) Then dcCostoML = Decimal.Parse(strCostoML)

            ''costo moneda extranjera
            'strCostoME = ""
            'strCostoME =
            '    Utilitarios.EjecutarConsulta(
            '        String.Format("SELECT SUM(P1.TotalFrgn) FROM OPCH AS P INNER JOIN PCH1 AS P1 ON P.DocEntry = P1.DocEntry  WHERE P.U_SCGD_Numero_OT = '{0}' AND P1.ItemCode = '{1}' GROUP BY P1.ItemCode  ",
            '        strNoOT,
            '        strNoArticulo),
            '    SBO_Company.CompanyDB,
            '    SBO_Company.Server)
            'dcCostoME = 0
            'If Not String.IsNullOrEmpty(strCostoME) Then dcCostoME = Decimal.Parse(strCostoME)

            'impuestos moneda local 
            strImpML = ""
            strImpML =
                Utilitarios.EjecutarConsulta(
                    String.Format("SELECT SUM(P1.LineVat) FROM OPCH AS P INNER JOIN PCH1 AS P1 ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' and P1.U_SCGD_IdRepxOrd = '{2}' GROUP BY P1.ItemCode  ",
                    strNoOT,
                    strNoArticulo,
                    oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_IdRepxOrd", i).Trim()),
                SBO_Company.CompanyDB,
                SBO_Company.Server)
            dcImpML = 0
            If Not String.IsNullOrEmpty(strImpML) Then dcImpML = Decimal.Parse(strImpML)

            'impuestos moneda extranjera 
            strImpME = ""
            strImpME =
                Utilitarios.EjecutarConsulta(
                    String.Format("SELECT SUM(P1.LineVatlF) FROM OPCH AS P INNER JOIN PCH1 AS P1 ON P.DocEntry = P1.DocEntry  WHERE P1.U_SCGD_NoOT = '{0}' AND P1.ItemCode = '{1}' and P1.U_SCGD_IdRepxOrd = '{2}' GROUP BY P1.ItemCode  ",
                    strNoOT,
                    strNoArticulo,
                    oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_IdRepxOrd", i).Trim()),
                SBO_Company.CompanyDB,
                SBO_Company.Server)
            dcImpME = 0
            If Not String.IsNullOrEmpty(strImpME) Then dcImpME = Decimal.Parse(strImpME)

            artContabilizado = False
            'If oListaSE.Count > 0 Then
            '    For x As Integer = 0 To oListaSE.Count - 1
            '        If oListaSE(x).Trim = strNoArticulo.Trim Then
            '            artContabilizado = True
            '            Exit For
            '        End If
            '    Next
            'End If

            If Not artContabilizado Then

                If strTipoArticulo = "4" _
                    And strInventariable = "N" Then

                    oForm.DataSources.DataTables.Item("SE").Rows.Add(1)

                    oForm.DataSources.DataTables.Item("SE").SetValue("LineId",
                                                                     Contador,
                                                                     Contador)
                    oForm.DataSources.DataTables.Item("SE").SetValue("ItemCode",
                                                                     Contador,
                                                                     strNoArticulo)
                    oForm.DataSources.DataTables.Item("SE").SetValue("WhsCode",
                                                                     Contador,
                                                                     oForm.DataSources.DBDataSources.Item("INV1").GetValue("WhsCode", i).Trim())
                    oForm.DataSources.DataTables.Item("SE").SetValue("ImpCode",
                                                                     Contador,
                                                                     oForm.DataSources.DBDataSources.Item("INV1").GetValue("TaxCode", i).Trim())
                    oForm.DataSources.DataTables.Item("SE").SetValue("LineVat",
                                                                     Contador,
                                                                     dcImpML.ToString())
                    oForm.DataSources.DataTables.Item("SE").SetValue("LineVatlF",
                                                                    Contador,
                                                                    dcImpME.ToString())
                    oForm.DataSources.DataTables.Item("SE").SetValue("CtaDebe",
                                                                     Contador,
                                                                     "")
                    oForm.DataSources.DataTables.Item("SE").SetValue("CtaHaber",
                                                                     Contador,
                                                                     "")
                    oForm.DataSources.DataTables.Item("SE").SetValue("LineTotal",
                                                                    Contador,
                                                                    dcCostoML.ToString())
                    oForm.DataSources.DataTables.Item("SE").SetValue("TotalFrgn",
                                                                     Contador,
                                                                     dcCostoME.ToString())

                    oForm.DataSources.DataTables.Item("SE").SetValue("U_SCGD_NoOT",
                                                             Contador,
                                                             oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim())

                    oForm.DataSources.DataTables.Item("SE").SetValue("U_SCGD_IdRepxOrd",
                                                                 Contador,
                                                                 oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_IdRepxOrd", i).Trim())

                    oListaSE.Add(oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim())
                    oListaIdRepxOrd.Add(oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_IdRepxOrd", i).Trim())

                    If Not oListaNumeroOT.Contains(oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim()) Then
                        oListaNumeroOT.Add(oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim())
                    End If

                    strBodProceso =
                        Utilitarios.EjecutarConsulta(String.Format(strConsultaBodProcXCC,
                                                                   oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()),
                                                     strNombreTaller,
                                                     SBO_Application.Company.ServerName)
                    listaBodegas_SE.Add(strBodProceso)
                    'oBodegas_SE.Add(Contador,
                    '                strBodProceso)

                    Contador = Contador + 1
                End If
            End If
        Next

        Return listaBodegas_SE

    End Function

    ''' <summary>
    ''' Ingresa en el DataTable de Servicios externos Las cuentas ERNF y SEPA
    ''' </summary>
    ''' <param name="oListaSE">Lista de servicios externos</param>
    ''' <param name="htCuentasErnf">HashTable con cuentas ERNF</param>
    ''' <param name="htCuentasSepa">HashTable con cuentas SEPA</param>
    ''' <param name="oForm">Objeto formulario</param>
    ''' <param name="htBodegas_SE">HashTable con servicios externos</param>
    ''' <remarks></remarks>
    Private Sub ObtieneCuentasYBodegas(ByVal oListaSE As IList(Of String),
                               ByRef htCuentasErnf As Hashtable,
                               ByRef htCuentasSepa As Hashtable,
                               ByVal oForm As SAPbouiCOM.Form,
                               ByVal htBodegas_SE As Hashtable, _
                                Optional ByVal p_listaBodegasSE As Generic.List(Of String) = Nothing)

        'Entrega recibidos no facturados
        'Servicios externos por asignar 
        Dim strCuentaDebe As String = ""
        Dim strCuentaHaber As String = ""

        'Dim oCtas_DebeXBod As New Hashtable
        'Dim oCtas_HaberXBod As New Hashtable

        Dim oCtas_DebeXBod As New Generic.List(Of String)
        Dim oCtas_HaberXBod As New Generic.List(Of String)

        Dim strServicioExterno As String
        Dim strCampoCuentaDebe As String = ""
        Dim strCampoCuentaHaber As String = ""

        strCampoCuentaDebe = "ExpensesAc"
        strCampoCuentaHaber = "SaleCostAc"

        For x As Integer = 0 To oForm.DataSources.DataTables.Item("SE").Rows.Count - 1
            strCuentaDebe = ""
            strCuentaHaber = ""
            strServicioExterno = ""

            Dim strNombreServicioExterno As String = oListaSE.Item(x)
            Dim strBodega As String = p_listaBodegasSE.Item(x)

            strServicioExterno = oForm.DataSources.DataTables.Item("SE").Columns.Item("ItemCode").Cells.Item(x).Value

            If oCtas_DebeXBod.Contains(strCuentaDebe) Then
                Dim position As Integer
                position = oCtas_DebeXBod.IndexOf(strCuentaDebe)
                strCuentaDebe = oCtas_DebeXBod.Item(position)
            End If

            If String.IsNullOrEmpty(strCuentaDebe) Then
                strCuentaDebe = Utilitarios.EjecutarConsulta(
                                String.Format("SELECT {0} FROM OWHS	WHERE WhsCode = '{1}'",
                                              strCampoCuentaDebe,
                                             strBodega),
                                          SBO_Company.CompanyDB,
                                          SBO_Company.Server)
                oCtas_DebeXBod.Add(strCuentaDebe)
            Else
                strCuentaDebe = strCuentaDebe
            End If

            oForm.DataSources.DataTables.Item("SE").SetValue("CtaDebe",
                                                             x,
                                                             strCuentaDebe)


            If oCtas_HaberXBod.Contains(strCuentaHaber) Then
                Dim position As Integer
                position = oCtas_HaberXBod.IndexOf(strCuentaHaber)
                strCuentaHaber = oCtas_HaberXBod.Item(position)
            End If

            If String.IsNullOrEmpty(strCuentaHaber) Then
                strCuentaHaber = Utilitarios.EjecutarConsulta(
                                String.Format("SELECT {0} FROM OWHS	WHERE WhsCode = '{1}'",
                                              strCampoCuentaHaber,
                                              strBodega),
                                          SBO_Company.CompanyDB,
                                          SBO_Company.Server)
                oCtas_HaberXBod.Add(strCuentaHaber)
            Else
                strCuentaHaber = strCuentaHaber
            End If

            oForm.DataSources.DataTables.Item("SE").SetValue("CtaHaber",
                                                             x,
                                                             strCuentaHaber)
        Next

    End Sub

    ''' <summary>
    ''' Retorna el campo deseado tanto del Datatable de Servicios externos como del de Impuestos 
    ''' </summary>
    ''' <param name="oForm">Objeto Formulario</param>
    ''' <param name="Condicion">Condicios para seleccionar el row</param>
    ''' <param name="Campo">Campo a retornar</param>
    ''' <param name="EsServExterno">Si va a obtener valores de la tabla de SE</param>
    ''' <param name="EsCtaImpuesto">Si va a obtener valores de la tabla de IMP</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RetornaCampo(ByVal oForm As Form,
                                          ByVal Condicion As String,
                                          ByVal Campo As String,
                                          ByVal EsServExterno As Boolean,
                                          ByVal EsCtaImpuesto As Boolean, Optional ByVal p_strNoOT As String = "") As Decimal
        Dim strImpuesto As String = ""
        Dim valorAcumulado As Decimal = 0
        Dim valor As String = String.Empty

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty

        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, SBO_Company.CompanyDB, SBO_Company.Server)

        If EsServExterno Then
            If oForm.DataSources.DataTables.Item("SE").Rows.Count > 0 Then
                For i As Integer = 0 To oForm.DataSources.DataTables.Item("SE").Rows.Count - 1
                    If oForm.DataSources.DataTables.Item("SE").GetValue("U_SCGD_NoOT", i) = p_strNoOT Then
                        Dim decPrecio As Decimal = CDec(oForm.DataSources.DataTables.Item("SE").GetValue(Campo, i))

                        valorAcumulado = valorAcumulado + decPrecio

                    End If
                Next

                If valorAcumulado <> 0 Then
                    Return valorAcumulado 'CStr(valorAcumulado).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Else
                    Return 0
                End If
            End If
            'ElseIf EsCtaImpuesto Then
            '    If oForm.DataSources.DataTables.Item("IMP").Rows.Count > 0 Then
            '        For i As Integer = 0 To oForm.DataSources.DataTables.Item("IMP").Rows.Count
            '            If oForm.DataSources.DataTables.Item("IMP").GetValue("Code", i) = Condicion Then
            '                Return oForm.DataSources.DataTables.Item("IMP").GetValue(Campo, i)
            '            End If
            '        Next
            '    End If
        End If


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

    ''' <summary>
    ''' Calcula costos por tipo de moneda del sistema, local y la entrada
    ''' </summary>
    ''' <param name="p_ocompany">Objeto compania</param>
    ''' <param name="p_montoDocumento">Monto de la entrada</param>
    ''' <param name="strMonedaEntrada">Moneda de la entrada</param>
    ''' <param name="strFechaEntrada">Fecha de la entrada</param>
    ''' <param name="strTipoCambioEntrada">Tipo de cambios de la entrada</param>
    ''' <returns>Costo de acierdo a la moneda de la entrada y del sistema</returns>
    ''' <remarks></remarks>
    Public Function CalcularCostosPorCambioMoneda(ByVal p_ocompany As SAPbobsCOM.Company, ByVal p_montoDocumento As Decimal, _
                                                  ByVal strMonedaEntrada As String, ByVal strFechaEntrada As String, _
                                                  ByVal strTipoCambioEntrada As String) As Decimal

        Dim m_objBLSBO As New BLSBO.GlobalFunctionsSBO
        Dim n As NumberFormatInfo
        Dim m_strMonedaLocal As String
        Dim m_strMonedaSistema As String
        Dim strTipoCambioSistema As String

        Dim decTipoCambioOrigen As Decimal
        Dim decValorDevuelto As Decimal
        Dim strMonedaBase As String
        Dim dtFecha As Date
        Dim valor As Decimal

        m_objBLSBO.Set_Compania(p_ocompany)

        m_strMonedaLocal = m_objBLSBO.RetornarMonedaLocal()
        m_strMonedaSistema = m_objBLSBO.RetornarMonedaSistema

        dtFecha = Date.ParseExact(strFechaEntrada, "yyyyMMdd", Nothing)

        strTipoCambioSistema = m_objBLSBO.RetornarTipoCambioMonedaRS(m_strMonedaSistema, dtFecha)

        n = DIHelper.GetNumberFormatInfo(p_ocompany)

        If Trim(strMonedaEntrada) <> Trim(m_strMonedaSistema) And
            Trim(strMonedaEntrada) = Trim(m_strMonedaLocal) Then

            Return Decimal.Parse(p_montoDocumento.ToString)

        ElseIf Trim(strMonedaEntrada) = Trim(m_strMonedaSistema) And
            Trim(strMonedaEntrada) <> Trim(m_strMonedaLocal) Then

            p_montoDocumento = p_montoDocumento * strTipoCambioSistema
            valor = Decimal.Parse(p_montoDocumento.ToString)
            Return Decimal.Parse(valor.ToString)

        ElseIf Trim(strMonedaEntrada) <> Trim(m_strMonedaSistema) And
        Trim(strMonedaEntrada) <> Trim(m_strMonedaLocal) Then

            p_montoDocumento = p_montoDocumento * strTipoCambioEntrada
            valor = Decimal.Parse(p_montoDocumento.ToString)
            Return Decimal.Parse(valor.ToString)

        End If

    End Function



    ''Creación de mano de obra
    'Public Function CrearAsientoManoObra(ByRef ocompany As SAPbobsCOM.Company,
    '                                    ByVal oForm As SAPbouiCOM.Form, _
    '                                    ByVal p_strCuentaAcredita As String, _
    '                                    ByVal p_strCuentaDebita As String, _
    '                                    ByVal p_strMoneda As String, _
    '                                    ByVal p_decMontoAsiento As Decimal, _
    '                                    ByVal p_strNoOT As String) As Integer

    '    Dim oJournalEntry As SAPbobsCOM.JournalEntries
    '    Dim strMonedaLocal As String
    '    Dim intError As Integer
    '    Dim strMensajeError As String = ""
    '    Dim strNoAsiento As String

    '    strNoAsiento = 0

    '    strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", ocompany.CompanyDB, ocompany.Server)

    '    oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)


    '    oJournalEntry.Memo &= "Mano de Obra"
    '    oJournalEntry.Reference = p_strNoOT


    '    '*****************
    '    'Cuenta Debito
    '    '*****************
    '    oJournalEntry.Lines.AccountCode = p_strCuentaDebita

    '    If strMonedaLocal = p_strMoneda Then
    '        oJournalEntry.Lines.Debit = p_decMontoAsiento
    '    Else
    '        oJournalEntry.Lines.FCDebit = p_decMontoAsiento
    '        oJournalEntry.Lines.FCCurrency = p_strMoneda

    '    End If

    '    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
    '    oJournalEntry.Lines.Add()

    '    '*********************
    '    ' Contra cuenta
    '    'Cuenta Credito
    '    '*********************
    '    oJournalEntry.Lines.AccountCode = p_strCuentaAcredita
    '    oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

    '    If strMonedaLocal = p_strMoneda Then
    '        oJournalEntry.Lines.Credit = p_decMontoAsiento
    '    Else
    '        oJournalEntry.Lines.FCCredit = p_decMontoAsiento
    '        oJournalEntry.Lines.FCCurrency = p_strMoneda
    '    End If


    '    If oJournalEntry.Add <> 0 Then
    '        strNoAsiento = "0"
    '        ocompany.GetLastError(intError, strMensajeError)
    '        Throw New ExceptionsSBO(intError, strMensajeError)
    '    Else
    '        ocompany.GetNewObjectCode(strNoAsiento)
    '    End If

    '    Return CInt(strNoAsiento)

    'End Function

    'Creación de mano de obra
    Public Function CrearAsientoOtrosGastos(ByRef ocompany As SAPbobsCOM.Company,
                                        ByVal p_strMoneda As String, _
                                        ByVal p_strCuentaDebitaGastos As String, _
                                        ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim strMonedaLocal As String
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strAsientoGenerado As String = "0"
        Dim strItemCode As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strIDRepXOrden As String = String.Empty
        Dim decCosto As Decimal = 0
        Dim strNoOrden As String = String.Empty
        Dim strAlmacen As String = String.Empty
        Dim strCuentaDebito As String = String.Empty

        Dim oListaGastos As New List(Of ListaCuentas)()

        Dim oListaAsientoGastos As New List(Of ListaCuentas)()

        Dim oListaNumeroOTValidados As IList(Of String) = New Generic.List(Of String)

        Dim strTipoOT As String = String.Empty

        Dim blnAgregarDimension As Boolean = False

        Dim DataTableValoresCotizacion As System.Data.DataTable

        Dim rowDim As System.Data.DataRow

        ValidarConfiguracionDimensiones(oForm)

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0).Trim(), BDTallerDMS)

        For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("INV1").Size - 1
            strItemCode = oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()
            If Not String.IsNullOrEmpty(strItemCode) Then
                strTipoArticulo = DevuelveValorItem(strItemCode, strSCGD_TipoArticulo).ToString.Trim()

                'El valor de TipoArticulo 11 es de Otros Gastos/Costos
                If Not String.IsNullOrEmpty(strTipoArticulo) And strTipoArticulo = "11" Then

                    strNoOrden = oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim()

                    If Not String.IsNullOrEmpty(strNoOrden) Then
                        decCosto = Decimal.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_Costo", i).Trim(), n)
                        strAlmacen = oForm.DataSources.DBDataSources.Item("INV1").GetValue("WhsCode", i).Trim()
                        strCuentaDebito = ObtenerCuentaItem(strItemCode, strAlmacen)
                        ' Add parts to the list.

                        If Not String.IsNullOrEmpty(strCuentaDebito) Then
                            If decCosto > 0 Then
                                oListaGastos.Add(New ListaCuentas() With {.NoOrden = strNoOrden, .CuentaDebito = strCuentaDebito, .Costo = decCosto, .Aplicado = False})
                            End If
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                            Return 0
                        End If
                    End If
                End If
            End If
        Next



        Dim NoOrdenTemp As String = String.Empty
        Dim CuentaTemp As String = String.Empty
        Dim decMontoTemp As Decimal = 0
        Dim strCuentaDebitoTemp As String = String.Empty
        Dim blnAgregar As Boolean = False

        For Each C1 As ListaCuentas In oListaGastos

            NoOrdenTemp = C1.NoOrden
            CuentaTemp = C1.CuentaDebito
            decMontoTemp = 0
            blnAgregar = False

            For Each C2 As ListaCuentas In oListaGastos

                If C2.NoOrden = NoOrdenTemp And C2.CuentaDebito = CuentaTemp And C2.Aplicado = False Then
                    strCuentaDebitoTemp = C2.CuentaDebito
                    C2.Aplicado = True
                    decMontoTemp += C2.Costo
                    blnAgregar = True
                End If

            Next
            If blnAgregar = True And Not String.IsNullOrEmpty(NoOrdenTemp) And Not String.IsNullOrEmpty(CuentaTemp) And decMontoTemp > 0 Then
                oListaAsientoGastos.Add(New ListaCuentas() With {.NoOrden = NoOrdenTemp, .CuentaDebito = CuentaTemp, .Costo = decMontoTemp, .Aplicado = True})
            End If
        Next


        If oListaAsientoGastos.Count() > 0 Then

            strAsientoGenerado = "0"

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", ocompany.CompanyDB, ocompany.Server)

            oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Memo = My.Resources.Resource.AsientoOtrosGastos
            oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocEntry", 0).Trim()

            For Each row As ListaCuentas In oListaAsientoGastos

                Dim strNoOt As String = row.NoOrden

                If blnUsaDimensiones Then

                    If Not oListaNumeroOTValidados.Contains(strNoOt) Then
                        oListaNumeroOTValidados.Add(strNoOt)
                        DataTableValoresCotizacion = Utilitarios.EjecutarConsultaDataTable("select Q.U_SCGD_Tipo_OT, Q.U_SCGD_idSucursal, Q.U_SCGD_Cod_Marca from OQUT Q  where Q.U_SCGD_Numero_OT = '" & strNoOt & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                        rowDim = DataTableValoresCotizacion.Rows(0)
                    End If

                    strTipoOT = rowDim.Item(0).ToString

                    Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOT, False, False)
                    '******************************************************************************************
                    'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                    If Not String.IsNullOrEmpty(strValorDimension) Then
                        If strValorDimension = "Y" Then
                            oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(oForm, rowDim.Item(1), rowDim.Item(2), oDataTableDimensionesContablesDMS))

                            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                blnAgregarDimension = True
                            End If

                        End If
                    End If
                    '******************************************************************************************
                End If
                '*****************
                ' Contra cuenta
                'Cuenta Credito
                '*****************
                oJournalEntry.Lines.AccountCode = p_strCuentaDebitaGastos

                If strMonedaLocal = p_strMoneda Then
                    oJournalEntry.Lines.Credit = row.Costo
                Else
                    oJournalEntry.Lines.FCCredit = row.Costo
                    oJournalEntry.Lines.FCCurrency = p_strMoneda
                End If

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()

                '*********************
                'Cuenta Debito
                '*********************
                oJournalEntry.Lines.AccountCode = row.CuentaDebito ' En este Caso aplica como cuenta de credito 
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                If strMonedaLocal = p_strMoneda Then
                    oJournalEntry.Lines.Debit = row.Costo
                Else
                    oJournalEntry.Lines.FCDebit = row.Costo
                    oJournalEntry.Lines.FCCurrency = p_strMoneda

                End If

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()
            Next


            If oJournalEntry.Add <> 0 Then
                strAsientoGenerado = "0"
                ocompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else

                ocompany.GetNewObjectCode(strAsientoGenerado)
            End If
        End If

        Return CInt(strAsientoGenerado)

    End Function

    'Creación de mano de obra
    Public Function CrearAsientoManoObra(ByRef ocompany As SAPbobsCOM.Company,
                                        ByVal p_strMoneda As String, _
                                        ByVal p_strCuentaAcredita As String, _
                                        ByVal p_strUsaTiempoEstandar As String, _
                                        ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim strMonedaLocal As String
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strAsientoGenerado As String = "0"
        Dim strItemCode As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strIDRepXOrden As String = String.Empty
        Dim decCosto As Decimal = 0
        Dim strNoOrden As String = String.Empty
        Dim strAlmacen As String = String.Empty
        Dim strCuentaDebito As String = String.Empty

        Dim oLista As New List(Of ListaCuentas)()

        Dim oListaAsiento As New List(Of ListaCuentas)()

        Dim oListaNumeroOTValidados As IList(Of String) = New Generic.List(Of String)

        Dim strTipoOT As String = String.Empty

        Dim blnAgregarDimension As Boolean = False

        Dim DataTableValoresCotizacion As System.Data.DataTable

        Dim rowDim As System.Data.DataRow

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0).Trim(), BDTallerDMS)

        ValidarConfiguracionDimensiones(oForm)

        For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("INV1").Size - 1
            strItemCode = oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()
            If Not String.IsNullOrEmpty(strItemCode) Then
                strTipoArticulo = DevuelveValorItem(strItemCode, strSCGD_TipoArticulo).ToString.Trim()

                'El valor de TipoArticulo 2 es de Servicios
                If Not String.IsNullOrEmpty(strTipoArticulo) And strTipoArticulo = "2" Then

                    strNoOrden = oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim()

                    If Not String.IsNullOrEmpty(strNoOrden) Then
                        decCosto = Decimal.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_Costo", i).Trim(), n)
                        strAlmacen = oForm.DataSources.DBDataSources.Item("INV1").GetValue("WhsCode", i).Trim()
                        strCuentaDebito = ObtenerCuentaItem(strItemCode, strAlmacen)
                        ' Add parts to the list.

                        If Not String.IsNullOrEmpty(strCuentaDebito) Then
                            If decCosto > 0 Then
                                oLista.Add(New ListaCuentas() With {.NoOrden = strNoOrden, .CuentaDebito = strCuentaDebito, .Costo = decCosto, .Aplicado = False})
                            End If
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                            Return 0
                        End If
                    End If
                End If
            End If
        Next



        Dim NoOrdenTemp As String = String.Empty
        Dim CuentaTemp As String = String.Empty
        Dim decMontoTemp As Decimal = 0
        Dim strCuentaDebitoTemp As String = String.Empty
        Dim blnAgregar As Boolean = False

        For Each C1 As ListaCuentas In oLista

            NoOrdenTemp = C1.NoOrden
            CuentaTemp = C1.CuentaDebito
            decMontoTemp = 0
            blnAgregar = False

            For Each C2 As ListaCuentas In oLista

                If C2.NoOrden = NoOrdenTemp And C2.CuentaDebito = CuentaTemp And C2.Aplicado = False Then
                    strCuentaDebitoTemp = C2.CuentaDebito
                    C2.Aplicado = True
                    decMontoTemp += C2.Costo
                    blnAgregar = True
                End If

            Next
            If blnAgregar = True And Not String.IsNullOrEmpty(NoOrdenTemp) And Not String.IsNullOrEmpty(CuentaTemp) And decMontoTemp > 0 Then
                oListaAsiento.Add(New ListaCuentas() With {.NoOrden = NoOrdenTemp, .CuentaDebito = CuentaTemp, .Costo = decMontoTemp, .Aplicado = True})
            End If
        Next

        If oListaAsiento.Count() > 0 Then

            strAsientoGenerado = "0"

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", ocompany.CompanyDB, ocompany.Server)

            oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Memo = My.Resources.Resource.AsientoManoObra
            oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocEntry", 0).Trim()

            For Each row As ListaCuentas In oListaAsiento

                Dim strNoOt As String = row.NoOrden

                If blnUsaDimensiones Then

                    If Not oListaNumeroOTValidados.Contains(strNoOt) Then
                        oListaNumeroOTValidados.Add(strNoOt)
                        DataTableValoresCotizacion = Utilitarios.EjecutarConsultaDataTable("select Q.U_SCGD_Tipo_OT, Q.U_SCGD_idSucursal, Q.U_SCGD_Cod_Marca from OQUT Q  where Q.U_SCGD_Numero_OT = '" & strNoOt & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                        rowDim = DataTableValoresCotizacion.Rows(0)
                    End If

                    strTipoOT = rowDim.Item(0).ToString
                    'Dim strValorDimension As String = ListaConfiguracionOT.Item(strTipoOT)
                    ' Dim strValorDimension As String = ListaConfiguracionOT.Item("Code").TipoOT
                    Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOT, False, False)
                    '******************************************************************************************
                    'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                    If Not String.IsNullOrEmpty(strValorDimension) Then
                        If strValorDimension = "Y" Then
                            oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(oForm, rowDim.Item(1), rowDim.Item(2), oDataTableDimensionesContablesDMS))

                            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                blnAgregarDimension = True
                            End If

                        End If
                    End If
                    '******************************************************************************************
                End If
                '*****************
                'Cuenta Debito
                '*****************

                oJournalEntry.Lines.AccountCode = row.CuentaDebito

                If strMonedaLocal = p_strMoneda Then
                    oJournalEntry.Lines.Debit = row.Costo
                Else
                    oJournalEntry.Lines.FCDebit = row.Costo
                    oJournalEntry.Lines.FCCurrency = p_strMoneda

                End If

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()

                '*********************
                ' Contra cuenta
                'Cuenta Credito
                '*********************
                oJournalEntry.Lines.AccountCode = p_strCuentaAcredita
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                If strMonedaLocal = p_strMoneda Then
                    oJournalEntry.Lines.Credit = row.Costo
                Else
                    oJournalEntry.Lines.FCCredit = row.Costo
                    oJournalEntry.Lines.FCCurrency = p_strMoneda
                End If

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()
            Next


            If oJournalEntry.Add <> 0 Then
                strAsientoGenerado = "0"
                ocompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else

                ocompany.GetNewObjectCode(strAsientoGenerado)
            End If
        End If

        Return CInt(strAsientoGenerado)

    End Function

    'Creación Servicio Externo
    Public Function CrearAsientoServicioExterno(ByRef ocompany As SAPbobsCOM.Company,
                                        ByVal oForm As SAPbouiCOM.Form) As Integer

        Dim oJournalEntry As SAPbobsCOM.JournalEntries
        Dim strMonedaLocal As String
        Dim intError As Integer
        Dim strMensajeError As String = ""
        Dim strAsientoGenerado As String = "0"
        Dim strItemCode As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strIDRepXOrden As String = String.Empty
        Dim decCosto As Decimal = 0
        Dim strNoOrden As String = String.Empty
        Dim strAlmacen As String = String.Empty
        Dim strCuentaDebito As String = String.Empty
        Dim strCuentaCredito As String = String.Empty

        Dim oLista As New List(Of ListaCuentas)()

        Dim oListaAsiento As New List(Of ListaCuentas)()

        Dim oListaNumeroOTValidados As IList(Of String) = New Generic.List(Of String)

        Dim strTipoOT As String = String.Empty

        Dim blnAgregarDimension As Boolean = False

        Dim DataTableValoresCotizacion As System.Data.DataTable

        Dim rowDim As System.Data.DataRow

        Utilitarios.DevuelveNombreBDTaller(SBO_Application, oForm.DataSources.DBDataSources.Item("OINV").GetValue("U_SCGD_idSucursal", 0).Trim(), BDTallerDMS)

        ValidarConfiguracionDimensiones(oForm)

        For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("INV1").Size - 1
            strItemCode = oForm.DataSources.DBDataSources.Item("INV1").GetValue("ItemCode", i).Trim()
            If Not String.IsNullOrEmpty(strItemCode) Then
                strTipoArticulo = DevuelveValorItem(strItemCode, strSCGD_TipoArticulo).ToString.Trim()

                'El valor de TipoArticulo 4 es de Servicios Externos
                If Not String.IsNullOrEmpty(strTipoArticulo) And strTipoArticulo = "4" Then

                    strNoOrden = oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_NoOT", i).Trim()

                    If Not String.IsNullOrEmpty(strNoOrden) Then
                        decCosto = Decimal.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("U_SCGD_Costo", i).Trim(), n)
                        strAlmacen = oForm.DataSources.DBDataSources.Item("INV1").GetValue("WhsCode", i).Trim()
                        strCuentaDebito = ObtenerCuentaDebitoSE(strAlmacen)
                        strCuentaCredito = ObtenerCuentaCreditoSE(strAlmacen)
                        ' Add parts to the list.

                        If Not String.IsNullOrEmpty(strCuentaDebito) And Not String.IsNullOrEmpty(strCuentaCredito) Then
                            If decCosto > 0 Then
                                oLista.Add(New ListaCuentas() With {.NoOrden = strNoOrden, .CuentaDebito = strCuentaDebito, .CuentaCredito = strCuentaCredito, .Costo = decCosto, .Aplicado = False})
                            End If
                        Else
                            If String.IsNullOrEmpty(strCuentaDebito) Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaDebitoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                                Return 0
                            End If
                            If String.IsNullOrEmpty(strCuentaCredito) Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.CuentaCreditoNoDefinida, SAPbouiCOM.BoMessageTime.bmt_Short)
                                Return 0
                            End If
                        End If
                    End If
                End If
            End If
        Next

        Dim NoOrdenTemp As String = String.Empty
        Dim CuentaDebitoTemp As String = String.Empty
        Dim CuentaCreditoTemp As String = String.Empty
        Dim decMontoTemp As Decimal = 0
        Dim strCuentaDebitoTemp As String = String.Empty
        Dim strCuentaCreditoTemp As String = String.Empty
        Dim blnAgregar As Boolean = False

        For Each C1 As ListaCuentas In oLista

            NoOrdenTemp = C1.NoOrden
            CuentaDebitoTemp = C1.CuentaDebito
            CuentaCreditoTemp = C1.CuentaCredito
            decMontoTemp = 0
            blnAgregar = False

            For Each C2 As ListaCuentas In oLista

                If C2.NoOrden = NoOrdenTemp And C2.CuentaDebito = CuentaDebitoTemp And C2.CuentaCredito = CuentaCreditoTemp And C2.Aplicado = False Then
                    strCuentaDebitoTemp = C2.CuentaDebito
                    strCuentaCreditoTemp = C2.CuentaCredito
                    C2.Aplicado = True
                    decMontoTemp += C2.Costo
                    blnAgregar = True
                End If

            Next
            If blnAgregar = True And Not String.IsNullOrEmpty(NoOrdenTemp) And Not String.IsNullOrEmpty(CuentaDebitoTemp) And Not String.IsNullOrEmpty(CuentaCreditoTemp) And decMontoTemp > 0 Then
                oListaAsiento.Add(New ListaCuentas() With {.NoOrden = NoOrdenTemp, .CuentaDebito = CuentaDebitoTemp, .CuentaCredito = CuentaCreditoTemp, .Costo = decMontoTemp, .Aplicado = True})
            End If
        Next

        If oListaAsiento.Count() > 0 Then

            strAsientoGenerado = "0"

            strMonedaLocal = Utilitarios.EjecutarConsulta("Select mainCurncy from OADM", ocompany.CompanyDB, ocompany.Server)

            oJournalEntry = ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Memo = My.Resources.Resource.AsientoServiciosExternos + oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocNum", 0).Trim()

            oJournalEntry.UserFields.Fields.Item("U_SCGD_FacC").Value = oForm.DataSources.DBDataSources.Item("OINV").GetValue("DocEntry", 0).Trim()

            For Each row As ListaCuentas In oListaAsiento

                Dim strNoOt As String = row.NoOrden

                If blnUsaDimensiones Then

                    If Not oListaNumeroOTValidados.Contains(strNoOt) Then
                        oListaNumeroOTValidados.Add(strNoOt)
                        DataTableValoresCotizacion = Utilitarios.EjecutarConsultaDataTable("select Q.U_SCGD_Tipo_OT, Q.U_SCGD_idSucursal, Q.U_SCGD_Cod_Marca from OQUT Q  where Q.U_SCGD_Numero_OT = '" & strNoOt & "'", SBO_Company.CompanyDB, SBO_Company.Server)
                        rowDim = DataTableValoresCotizacion.Rows(0)
                    End If

                    strTipoOT = rowDim.Item(0).ToString
                    Dim strValorDimension As String = ClsLineasDocumentosDimension.ValidacionAsientosDimensiones(ListaConfiguracionOT, strTipoOT, False, False)
                    '******************************************************************************************
                    'lleno el datatable de dimensiones para el tipo de inventario y la marca del vehiculo
                    If Not String.IsNullOrEmpty(strValorDimension) Then
                        If strValorDimension = "Y" Then
                            oDataTableDimensionesContablesDMS = (ClsLineasDocumentosDimension.DatatableDimensionesContablesOrdenTrabajo(oForm, rowDim.Item(1), rowDim.Item(2), oDataTableDimensionesContablesDMS))

                            If oDataTableDimensionesContablesDMS.Rows.Count <> 0 Then
                                blnAgregarDimension = True
                            End If

                        End If
                    End If
                    '******************************************************************************************
                End If
                '*****************
                'Cuenta Debito
                '*****************

                oJournalEntry.Lines.AccountCode = row.CuentaDebito
                oJournalEntry.Lines.Debit = row.Costo

                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()

                '*********************
                ' Contra cuenta
                'Cuenta Credito
                '*********************
                oJournalEntry.Lines.AccountCode = row.CuentaCredito
                oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                oJournalEntry.Lines.UserFields.Fields.Item(mc_strSCGD_NoOT).Value = row.NoOrden
                oJournalEntry.Lines.Reference1 = row.NoOrden

                oJournalEntry.Lines.Credit = row.Costo

                If blnAgregarDimension Then
                    ClsLineasDocumentosDimension.AgregarDimensionesLineasAsiento(oJournalEntry.Lines, Nothing, oDataTableDimensionesContablesDMS)
                End If

                oJournalEntry.Lines.Add()
            Next

            If oJournalEntry.Add <> 0 Then
                strAsientoGenerado = "0"
                ocompany.GetLastError(intError, strMensajeError)
                Throw New ExceptionsSBO(intError, strMensajeError)
            Else
                ocompany.GetNewObjectCode(strAsientoGenerado)
            End If
        End If
        Return CInt(strAsientoGenerado)
    End Function

    Public Function ObtenerCuentaItem(ByVal p_itemCode As String, ByVal strAlmacen As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim cuentaContable As String = String.Empty

        Try

            oItemArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            oItemArticulo.GetByKey(p_itemCode)

            'Almacen= SAPbobsCOM.BoGLMethods.glm_WH
            'Grupo de articulos= SAPbobsCOM.BoGLMethods.glm_ItemClass
            'Nivel de artículos= SAPbobsCOM.BoGLMethods.glm_ItemLevel

            Select Case oItemArticulo.GLMethod
                Case SAPbobsCOM.BoGLMethods.glm_WH
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc FROM OWHS Where WhsCode = '{0}'",
                                                        strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)

                Case SAPbobsCOM.BoGLMethods.glm_ItemClass
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc From OITB Where ItmsGrpCod = '{0}'",
                                                        oItemArticulo.ItemsGroupCode.ToString.Trim()),
                                                        SBO_Company.CompanyDB,
                                                        SBO_Company.Server)

                Case SAPbobsCOM.BoGLMethods.glm_ItemLevel

                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc From OITW Where ItemCode= '{0}' AND WhsCode = '{1}'",
                                                        p_itemCode, strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)

                Case Else
                    cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc FROM OWHS Where WhsCode = '{0}'",
                                                        strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)

            End Select

            Return cuentaContable
        Catch ex As Exception

        Finally
            If Not oItemArticulo Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oItemArticulo)
                oItemArticulo = Nothing
            End If
        End Try
    End Function

    Public Function ObtenerCuentaCreditoSE(ByVal strAlmacen As String) As String
        Try
            Dim cuentaContable As String = String.Empty

            cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select ExpensesAc FROM OWHS Where WhsCode = '{0}'",
                                                strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)
            Return cuentaContable

        Catch ex As Exception

        End Try
    End Function

    Public Function ObtenerCuentaDebitoSE(ByVal strAlmacen As String) As String
        Try
            Dim cuentaContable As String = String.Empty

            cuentaContable = Utilitarios.EjecutarConsulta(String.Format("Select SaleCostAc FROM OWHS Where WhsCode = '{0}'",
                                                strAlmacen), SBO_Company.CompanyDB, SBO_Company.Server)
            Return cuentaContable

        Catch ex As Exception

        End Try
    End Function


    Private Function DevuelveValorItem(ByVal strItemcode As String, _
                                      ByVal strUDfName As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim valorUDF As String

        oItemArticulo = SBO_Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)
        valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value

        Return valorUDF

    End Function

    Private Sub ValidarConfiguracionDimensiones(ByVal p_form As SAPbouiCOM.Form)

        'configuraciones para Dimensiones para OTs
        Dim strUsaDimension As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] ", SBO_Company.CompanyDB, SBO_Company.Server)

        If strUsaDimension = "Y" Then

            oDataTableDimensionesContablesDMS = p_form.DataSources.DataTables.Item(mc_strDataTableDimensionesOT)
            blnUsaDimensiones = True

            'hago el llamado para cargar la configuracion de los documentos
            'que usaran Dimensiones
            ClsLineasDocumentosDimension = New AgregarDimensionLineasDocumentosCls(SBO_Company, SBO_Application)
            'ListaConfiguracionOT = New Hashtable
            ListaConfiguracionOT = New List(Of LineasConfiguracionOT)()
            ListaConfiguracionOT = ClsLineasDocumentosDimension.DatatableConfiguracionDocumentosDimensionesOT(p_form)

        End If


    End Sub

    Private Sub saveBaseEntry(ByVal oFormFacturaCliente As SAPbouiCOM.Form)

        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oEditText As SAPbouiCOM.EditText
        Try
            oMatrix = DirectCast(oFormFacturaCliente.Items.Item("38").Specific, SAPbouiCOM.Matrix)
            If oMatrix.RowCount > 0 Then
                oEditText = CType(oMatrix.Columns.Item("1").Cells.Item(1).Specific, EditText)
                If Not String.IsNullOrEmpty(oEditText.Value) Then
                    For index As Integer = 1 To oMatrix.RowCount - 1
                        Dim intLinea(1) As String
                        oEditText = CType(oMatrix.Columns.Item("44").Cells.Item(index).Specific, EditText)
                        intLinea(0) = oEditText.Value
                        oEditText = CType(oMatrix.Columns.Item("U_SCGD_NoOT").Cells.Item(index).Specific, EditText)
                        intLinea(1) = oEditText.Value
                        ListaBaseEntry.Add(intLinea)
                    Next
                    BooleanBaseEntry = True
                End If
            Else
                BooleanBaseEntry = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
        

    End Sub

    Public Property ListaBaseEntry() As List(Of String())
        Get
            Return listBaseEntry
        End Get
        Set(ByVal value As List(Of String()))
            listBaseEntry = value
        End Set
    End Property
    Public Property BooleanBaseEntry() As Boolean
        Get
            Return blnBaseEntry
        End Get
        Set(ByVal value As Boolean)
            blnBaseEntry = value
        End Set
    End Property

End Class



' Clase para la definición de la lista
Public Class ListaCuentas

    Public Property NoOrden() As String
        Get
            Return strNoOrden
        End Get
        Set(ByVal value As String)
            strNoOrden = value
        End Set
    End Property
    Private strNoOrden As String

    Public Property CuentaDebito() As String
        Get
            Return strCuentaDebito
        End Get
        Set(ByVal value As String)
            strCuentaDebito = value
        End Set
    End Property
    Private strCuentaDebito As String

    Public Property CuentaCredito() As String
        Get
            Return strCuentaCredito
        End Get
        Set(ByVal value As String)
            strCuentaCredito = value
        End Set
    End Property
    Private strCuentaCredito As String

    Public Property Costo() As Decimal
        Get
            Return decCosto
        End Get
        Set(ByVal value As Decimal)
            decCosto = value
        End Set
    End Property
    Private decCosto As Decimal

    Public Property Aplicado() As Boolean
        Get
            Return blnAplicado
        End Get
        Set(ByVal value As Boolean)
            blnAplicado = value
        End Set
    End Property
    Private blnAplicado As Boolean


End Class
