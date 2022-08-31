Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SAPbobsCOM
Imports SCG.Requisiciones.UI
Imports DMSOneFramework.SCGBL.Requisiciones
Imports System.Collections.Generic
Imports System.Linq

Public Class OrdenVenta

#Region "... Declaraciones ..."
    'Objetos
    Private m_oCompany As SAPbobsCOM.Company
    Private WithEvents SBO_Application As SAPbouiCOM.Application
    Private g_dtConsulta As SAPbouiCOM.DataTable
    Private m_dataTableContratos As SAPbouiCOM.DataTable
    Private m_oTipoOtInterna As TipoOtInterna
    Private oGestorFormularios As GestorFormularios
    Private oFormTipoOTInterna As TipoOtInterna
    Private oPosicionControles As Dictionary(Of String, Coordenadas)

    'Constantes
    Private Const g_strFormTipoOTInterna As String = "SCGD_TOTI"
    Private Const g_strFormOrdenVenta As String = "139"
    Private Const g_strMTZItemsCotizacion As String = "mc_strMTZItemsCotizacion"
    Private Const g_strDtConsul As String = "dtConsul"
    Private Const mc_strBtnGen As String = "btnGen"
    Private Const mc_strstFI As String = "SCGD_stFI"
    Private Const mc_stretFI As String = "SCGD_etFI"
    Private Const mc_strLKBFI As String = "SCGD_LKFI"
    Private Const mc_strUIFacturasInt As String = "SCGD_FAC_INT"
    Private Const mc_strIDMatriz As String = "38"
    Private Const mc_strIDBotonEjecucion As String = "1"

    Private Const mc_stTipoPago As String = "stTipoPago"
    Private Const mc_stDptoSrv As String = "stDptoSrv"
    Private Const mc_strCboTipoPago As String = "cboTipPago"
    Private Const mc_strCboDptoSrv As String = "cboDptoSrv"

    Private Const mc_strUDFTipoPago As String = "U_SCGD_TipoPago"
    Private Const mc_strUDFServDpto As String = "U_SCGD_ServDpto"

    Private Const mc_strORDR As String = "ORDR"

    'decimales
    Dim n As Globalization.NumberFormatInfo

    Private oOrdenVentaAbierta As Documents
    Private boolCierreParcial As Boolean = False
    Private strGenerarRequisicionDevolucion As String = String.Empty
    Private Enum TipoPosicionControles
        Estandar = 1
        FacturaElectronica = 2
    End Enum

    Private _DocEntry As String = String.Empty

    Public Property DocEntry As String
        Get
            Return _DocEntry
        End Get
        Set(value As String)
            _DocEntry = value
        End Set
    End Property


#End Region

#Region "... Constructor ..."
    Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As SAPbobsCOM.Company)
        Try
            SBO_Application = p_SBO_Aplication
            m_oCompany = p_oCompania
            oFormTipoOTInterna = New TipoOtInterna(m_oCompany, SBO_Application)
            n = DIHelper.GetNumberFormatInfo(m_oCompany)
            InicializarPosicionControles()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

#Region "... Eventos ..."

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                                            ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        Dim oComboTipoPago As SAPbouiCOM.ComboBox
        Dim oComboDptoServ As SAPbouiCOM.ComboBox
        Dim message As String = String.Empty

        Try
            oForm = SBO_Application.Forms.Item(FormUID)
            Select Case oForm.TypeEx
                Case g_strFormOrdenVenta
                    If pVal.BeforeAction Then
                        Select Case pVal.ItemUID
                            Case mc_strBtnGen
                                If ValidaCotizacionLines(oForm, pVal, BubbleEvent, message) Then
                                    CargarFormularioTiposOTInterna(pVal)
                                Else
                                    SBO_Application.StatusBar.SetText(message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                End If
                            Case mc_strLKBFI
                                CargarFormularioFacturaInterna(pVal, oForm)
                            Case mc_strIDBotonEjecucion

                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                                    Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                                    If usaInterFazFord Then
                                        Dim socioNegTip = Utilitarios.ValidaIFTipoSN(m_oCompany, oForm.DataSources.DBDataSources.Item("ORDR").GetValue("CardCode", 0))

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

                                

                                strGenerarRequisicionDevolucion = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_GenReqDev", 0).ToString()

                        End Select
                    ElseIf pVal.ActionSuccess Then
                        Select Case pVal.ItemUID
                            Case mc_strBtnGen
                                'Dim numOT As String
                                'Dim DocEntry As String
                                'numOT = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_Numero_OT", 0).Trim()
                                'DocEntry = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocEntry", 0).Trim()

                                'Dim result As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocStatus", 0).Trim()
                                ''Dim result As String = Utilitarios.EjecutarConsulta(query, SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName)
                                'If result = "O" Then
                                '    Dim blnUsaTallerOTSAP As Boolean = False
                                '    If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                                '        blnUsaTallerOTSAP = True
                                '    End If
                                '    oFormTipoOTInterna.CargaOT(numOT, DocEntry)
                                '    oFormTipoOTInterna.LoadMatrixLines(blnUsaTallerOTSAP)
                                'End If
                        End Select
                    End If

                    If pVal.BeforeAction = False Then
                        Select Case pVal.ItemUID
                            Case mc_strIDBotonEjecucion
                                'Si se cerraron líneas se generan las devoluciones correspondientes.
                                If boolCierreParcial Then
                                    If ObtenerCampoConfiguracionGeneral("U_GenReqDev").ToUpper().Equals("Y") Or strGenerarRequisicionDevolucion.ToUpper().Equals("Y") Then
                                        'Se procede a generar las requisiciones de devolucion
                                        GenerarDevoluciones(DocEntry, BubbleEvent, oOrdenVentaAbierta)
                                    End If
                                    boolCierreParcial = False
                                End If
                                ValidaLineaAdicional(oForm)
                        End Select
                    End If

            End Select

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub




    Public Sub ManejadorEventoLoad(ByVal FormUID As String, _
                           ByRef pVal As SAPbouiCOM.ItemEvent, _
                           ByRef BubbleEvent As Boolean)

        Try
            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If pVal.BeforeAction Then
                Dim userDS As UserDataSources = oForm.DataSources.UserDataSources
                If oForm IsNot Nothing Then
                    g_dtConsulta = oForm.DataSources.DataTables.Add(g_strDtConsul)
                    userDS.Add("btnGenFI", BoDataType.dt_LONG_TEXT, 100)
                End If

                If AgregaBtnGenFacInt(oForm, SBO_Application) Then
                    userDS.Item("btnGenFI").Value = "Y"
                    Dim oItem As SAPbouiCOM.Item
                    oItem = oForm.Items.Item(mc_strBtnGen)
                    Dim noOt As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_Numero_OT", 0).ToString()
                    If String.IsNullOrEmpty(noOt) Then
                        oItem.Visible = False
                    Else
                        oItem.Visible = True
                        If oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocStatus", 0).Trim() = "C" Then
                            oItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                        Else
                            oItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)
                        End If
                    End If
                End If
                AddInternalInvoiceReference(oForm, SBO_Application)

                Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_oCompany)
                If usaInterFazFord Then
                    'AgregaCamoposFI(oForm, SBO_Application)
                End If

            ElseIf pVal.ActionSuccess Then

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoFormDataLoad(ByVal oForm As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            If oForm.DataSources.UserDataSources.Item("btnGenFI").Value = "Y" Then

                Dim oItem As SAPbouiCOM.Item
                oItem = oForm.Items.Item(mc_strBtnGen)
                Dim etFI As SAPbouiCOM.EditText
                etFI = DirectCast(oForm.Items.Item(mc_stretFI).Specific, SAPbouiCOM.EditText)
                etFI.Value = String.Empty

                Dim noOt As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_Numero_OT", 0).ToString()
                If String.IsNullOrEmpty(noOt) Then
                    oItem.Visible = False
                Else
                    oItem.Visible = True
                    If oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocStatus", 0).Trim() = "C" Then
                        oItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)

                        Dim strFI As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_NoFI", 0).Trim()
                        If Not String.IsNullOrEmpty(strFI) Then
                            etFI.Value = strFI
                        End If
                    Else
                        oItem.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)
                    End If
                End If


            End If


        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

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

    Public Sub FormResizeEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            oFormulario = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            If pVal.BeforeAction Then
                'Implementar manejo del BeforeAction = false aquí
            Else
                AjustarPosicionControles(oFormulario)
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

            'StaticText Factura Interna
            oFormulario.Items.Item("SCGD_stFI").Top = intTop + 30
            oFormulario.Items.Item("SCGD_stFI").Left = intLeft

            'LinkButton Factura Interna
            oFormulario.Items.Item("SCGD_LKFI").Top = intTop + 30
            oFormulario.Items.Item("SCGD_LKFI").Left = intLeft + 106

            'EditText Factura Interna
            oFormulario.Items.Item("SCGD_etFI").Top = intTop + 30
            oFormulario.Items.Item("SCGD_etFI").Left = intLeft + 120

            'Verifica si utiliza la interfaz de Ford
            blnUsaInterfazFord = Utilitarios.UsaInterfazFord(m_oCompany)

            If blnUsaInterfazFord Then
                AjustarControlesInterfazFord(oFormulario)
            End If

            If Utilitarios.MostrarMenu("SCGD_OVF", SBO_Application.Company.UserName) Then
                'Cambia el Top y Left para basarse en la posición del botón "Copiar a"
                intTop = oFormulario.Items.Item("10000329").Top
                intLeft = oFormulario.Items.Item("10000329").Left

                oFormulario.Items.Item("btnGen").Top = intTop - 22
                oFormulario.Items.Item("btnGen").Left = intLeft
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

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Funcion que valida las linas de la orden contra las lineas de la cotizacion
    ''' </summary>
    ''' <param name="oForm">Formulario actual</param>
    ''' <param name="p_errMessage">Mensaje que genera si ocurre un error</param>
    ''' <returns>indica si son iguales o no</returns>
    Private Function ValidaCotizacionLines(ByVal oForm As SAPbouiCOM.Form, _
                                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                                            ByRef BubbleEvent As Boolean, _
                                            ByRef p_errMessage As String) As Boolean
        Dim resultFunc As Boolean = True
        Dim query As String
        Dim resultCount As String
        Dim strNoOt As String
        Dim contMatriz As Integer
        Dim contDt As Integer
        Dim decQuantityOV As Decimal
        Dim decQuatityOFV As Decimal
        Dim blnPermiteAgregarLineas As Boolean = False
        Dim strIDSucursal As String = String.Empty

        Try
            strNoOt = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_Numero_OT", 0).Trim()
            strIDSucursal = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_idSucursal", 0).Trim()
            If Not String.IsNullOrEmpty(strNoOt) Then

                query = String.Empty
                query = String.Format("select QUT1.DocEntry, QUT1.ItemCode, QUT1.Quantity, QUT1.LineNum from QUT1 with (nolock) inner join OQUT q with (nolock) on QUT1.DocEntry=q.DocEntry where q.U_SCGD_Numero_OT = '{0}' and QUT1.U_SCGD_Aprobado  = '1'", strNoOt)
                g_dtConsulta.ExecuteQuery(query)

                If g_dtConsulta.Rows.Count = (oForm.DataSources.DBDataSources.Item("RDR1").Size - 1) Then

                    For contMatriz = 0 To (oForm.DataSources.DBDataSources.Item("RDR1").Size - 1)
                        For contDt = 0 To (g_dtConsulta.Rows.Count - 1)

                            If (oForm.DataSources.DBDataSources.Item("RDR1").GetValue("ItemCode", contMatriz).Trim() = _
                                g_dtConsulta.GetValue("ItemCode", contDt).ToString().Trim()) And _
                                ((oForm.DataSources.DBDataSources.Item("RDR1").GetValue("BaseEntry", contMatriz).Trim() = _
                                g_dtConsulta.GetValue("DocEntry", contDt).ToString().Trim()) And _
                                ((oForm.DataSources.DBDataSources.Item("RDR1").GetValue("BaseLine", contMatriz).Trim() = _
                                g_dtConsulta.GetValue("LineNum", contDt).ToString().Trim()))) Then

                                decQuantityOV = Decimal.Parse(oForm.DataSources.DBDataSources.Item("RDR1").GetValue("Quantity", contMatriz), n)
                                decQuatityOFV = g_dtConsulta.GetValue("Quantity", contDt).ToString()
                                If decQuantityOV <> decQuatityOFV Then

                                    p_errMessage = String.Format(My.Resources.Resource.ErrDifferentItemQuantity, oForm.DataSources.DBDataSources.Item("RDR1").GetValue("Dscription", contMatriz).Trim())
                                    contMatriz = oForm.DataSources.DBDataSources.Item("RDR1").Size
                                    contDt = g_dtConsulta.Rows.Count

                                    resultFunc = False
                                    BubbleEvent = False
                                End If
                            End If
                        Next
                    Next
                Else
                    'Obtiene la configuración por sucursal para determinar si se pueden agregar líneas adicionales a una orden de venta ligada a orden de trabajo
                    'posterior al cierre de la orden de trabajo.
                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(x) x.U_Sucurs = strIDSucursal) Then
                        If DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs = strIDSucursal).U_UsaLAOV = "Y" Then
                            blnPermiteAgregarLineas = True

                            If oForm.Mode = BoFormMode.fm_UPDATE_MODE Then
                                BubbleEvent = False
                                resultFunc = False
                                p_errMessage = My.Resources.Resource.ErrorModoActualizar
                            End If
                        End If
                    End If

                    If Not blnPermiteAgregarLineas Then
                        BubbleEvent = False
                        resultFunc = False
                        p_errMessage = My.Resources.Resource.ErrDifferentItems
                    End If
                End If
            End If
            Return resultFunc
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Agrega Boton de generar factura interna en formulario de orden de ventas
    ''' </summary>
    ''' <param name="oform">Objeto de Formulario</param>
    ''' <remarks></remarks>
    Public Shared Function AgregaBtnGenFacInt(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application) As Boolean

        Dim result As Boolean = False

        Dim oItem As SAPbouiCOM.Item

        Dim oButton As SAPbouiCOM.Button
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try

            If Utilitarios.MostrarMenu("SCGD_OVF", p_SBO_Application.Company.UserName) Then

                intTop = oform.Items.Item("10000330").Top
                intLeft = oform.Items.Item("10000330").Left
                intWidth = oform.Items.Item("10000330").Width
                intHeight = oform.Items.Item("10000330").Height

                oItem = oform.Items.Add(mc_strBtnGen, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                oItem.Top = intTop
                oItem.Left = intLeft - 104
                oItem.Width = intWidth
                oItem.Height = intHeight
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                oItem.Enabled = False

                oButton = oItem.Specific
                oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
                oButton.Caption = My.Resources.Resource.btn_GenFactInt
                result = True
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
        Return result

    End Function

    Public Sub AddInternalInvoiceReference(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application)

        Dim oItem As SAPbouiCOM.Item

        Dim oLabel As SAPbouiCOM.StaticText
        Dim oEdit As SAPbouiCOM.EditText
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try
            'agrega EditText
            intTop = oform.Items.Item("46").Top
            intLeft = oform.Items.Item("46").Left
            intWidth = oform.Items.Item("46").Width
            intHeight = oform.Items.Item("46").Height

            oItem = oform.Items.Add(mc_stretFI, SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oItem.Top = intTop + 30
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.FromPane = 0
            oItem.ToPane = 0
            oItem.LinkTo = mc_strstFI

            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oItem.Enabled = False
            oEdit = oItem.Specific

            'agrega EditText
            intTop = oform.Items.Item("86").Top
            intLeft = oform.Items.Item("86").Left
            intWidth = oform.Items.Item("86").Width
            intHeight = oform.Items.Item("86").Height

            oItem = oform.Items.Add(mc_strstFI, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oItem.Top = intTop + 30
            oItem.Left = intLeft
            oItem.Width = intWidth
            oItem.Height = intHeight
            oItem.FromPane = 0
            oItem.ToPane = 0
            oItem.LinkTo = mc_stretFI
            oLabel = oItem.Specific
            oLabel.Caption = My.Resources.Resource.Txt_FacInt

            'Agrega LinkBtn'
            oItem = oform.Items.Item(mc_stretFI)
            oItem = AgregaLinkedButton(oform, mc_strLKBFI, oItem.Left - 14, oItem.Top + 3, 10, 13, mc_stretFI, SAPbouiCOM.BoLinkedObject.lf_None, p_SBO_Application)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Shared Function AgregaLinkedButton(ByRef oform As SAPbouiCOM.Form, _
                                         ByVal strNombrectrl As String, _
                                         ByVal intLeft As Integer, _
                                         ByVal intTop As Integer, _
                                         ByVal intHeight As Integer, _
                                         ByVal intWidth As Integer, _
                                         ByVal strLinkTo As String, _
                                         ByVal LinkedObject As SAPbouiCOM.BoLinkedObject, _
                                         ByVal objSBO_Application As SAPbouiCOM.Application) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oLinkedButton As SAPbouiCOM.LinkedButton
        Try


            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.Height = intHeight
            oitem.Width = intWidth
            oitem.LinkTo = strLinkTo
            oLinkedButton = oitem.Specific
            oLinkedButton.LinkedObjectType = LinkedObject

            Return oitem
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Carga el formulario de tipo de ot para generar la factura
    ''' </summary>
    Private Sub CargarFormularioTiposOTInterna(ByRef pVal As SAPbouiCOM.ItemEvent)
        Dim strPath As String
        Dim oForm As SAPbouiCOM.Form

        Try
            oForm = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            Dim numOT As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_Numero_OT", 0).Trim()
            Dim DocEntry As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocEntry", 0).Trim()
            Dim result As String = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocStatus", 0).Trim()
            If result = "O" Then
                oGestorFormularios = New GestorFormularios(SBO_Application)
                oFormTipoOTInterna = New TipoOtInterna(m_oCompany, SBO_Application)

                oFormTipoOTInterna.FormType = g_strFormTipoOTInterna '"SCGD_TOTI"
                oFormTipoOTInterna.Titulo = My.Resources.Resource.TituloTipoOtInterna
                'oFormTipoOTInterna.NumeroOT = numOT
                strPath = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLTipoOtInterna
                oFormTipoOTInterna.NombreXml = strPath
                oFormTipoOTInterna.NoOT = numOT
                oFormTipoOTInterna.DocEntryOV = DocEntry
                oFormTipoOTInterna.FormularioSBO = oGestorFormularios.CargaFormulario(oFormTipoOTInterna)
            Else
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ERR_SalesOrderClosed, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el formulario de Facturas inetrnas
    ''' </summary>
    Private Sub CargarFormularioFacturaInterna(ByVal pVal As SAPbouiCOM.ItemEvent, ByVal oForm As Form)
        Dim m_oFacturaInterna As FacturaInterna
        Dim etFI As SAPbouiCOM.EditText

        Try
            etFI = DirectCast(oForm.Items.Item(mc_stretFI).Specific, SAPbouiCOM.EditText)

            m_oFacturaInterna = New FacturaInterna(SBO_Application, m_oCompany)
            Dim strNumFI As String = etFI.Value
            If Not String.IsNullOrEmpty(strNumFI) AndAlso Not Utilitarios.ValidarSiFormularioAbierto(mc_strUIFacturasInt, True, SBO_Application) Then
                Call m_oFacturaInterna.CargaFormulario()
                Call m_oFacturaInterna.CargarFactura(strNumFI)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    Public Function FilaTieneNumeroOT(ByVal p_form As SAPbouiCOM.Form, ByVal row As Integer, ByVal itemUID As String, ByRef m_NumOT As String) As Boolean
        Dim result As Boolean = False

        Try
            If itemUID = mc_strIDMatriz Then

                Dim strNumOT As String
                Dim oMatrix As SAPbouiCOM.Matrix

                p_form.Freeze(True)
                oMatrix = DirectCast(p_form.Items.Item(mc_strIDMatriz).Specific, SAPbouiCOM.Matrix)
                If oMatrix.RowCount - 1 >= row And row <> 0 Then
                    'idRepXOrd = oform.DataSources.DBDataSources.Item("QUT1").GetValue("U_SCGD_IdRepxOrd", (pVal.Row - 1)).Trim()
                    strNumOT = oMatrix.Columns.Item("U_SCGD_NoOT").Cells.Item(row).Specific.Value.ToString().Trim()
                    If Not String.IsNullOrEmpty(strNumOT) Then
                        result = True
                        m_NumOT = strNumOT
                        'SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorEliminaLineaConOT, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End If
                End If
                p_form.Freeze(False)
            End If
            Return result
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Public Sub PermitirCancelar(ByVal p_StrIDForm As String, ByRef BubbleEvent As Boolean)
        Dim strNoOT As String
        Dim blnPermitirCancelar As Boolean = True
        Dim oForm As SAPbouiCOM.Form

        Try
            oForm = SBO_Application.Forms.Item(p_StrIDForm)

            'strNoOT = m_oFormGenCotizacion.Items.Item("SCGD_etOT").Specific.String()
            strNoOT = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_Numero_OT", 0).ToString().Trim()
            strNoOT = strNoOT.Trim

            If Not String.IsNullOrEmpty(strNoOT) Then
                'blnPermitirCancelar = False
                BubbleEvent = False
                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCancelarOV, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos tipo Menu
    ''' </summary>
    ''' <param name="pval">pval con el detalle del evento</param>
    ''' <param name="BubbleEvent">BubbleEvent proveniente de SAP</param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Private Sub ManejadorEventoMenu(ByRef pval As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles SBO_Application.MenuEvent
        Dim oForm As SAPbouiCOM.Form

        Try
            If SBO_Application.Forms.ActiveForm.TypeEx = "139" Then

                If pval.MenuUID = "1284" Or pval.MenuUID = "1286" Or pval.MenuUID = "1299" Then
                    oForm = SBO_Application.Forms.ActiveForm

                    Select Case pval.MenuUID
                        'Menú Cerrar documento y cerrar línea
                        Case "1286"

                            If oForm.Mode = BoFormMode.fm_OK_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then
                                DocEntry = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocEntry", 0).ToString()
                            End If

                            'Generar las devoluciones para todos los repuestos y suministros que no se vayan a facturar
                            If Not String.IsNullOrEmpty(DocEntry) Then
                                If pval.BeforeAction Then
                                    'Durante el BeforeAction, se guarda la orden de venta en memoria antes de ser cerrada.
                                    oOrdenVentaAbierta = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)
                                    oOrdenVentaAbierta.GetByKey(DocEntry)
                                    strGenerarRequisicionDevolucion = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_GenReqDev", 0).ToString()
                                Else
                                    If ObtenerCampoConfiguracionGeneral("U_GenReqDev").ToUpper().Equals("Y") Or strGenerarRequisicionDevolucion.ToUpper().Equals("Y") Then
                                        'Se procede a generar las requisiciones de devolucion
                                        GenerarDevoluciones(DocEntry, BubbleEvent, oOrdenVentaAbierta)
                                    End If
                                End If
                            End If
                        Case "1299"

                            If oForm.Mode = BoFormMode.fm_OK_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then
                                DocEntry = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("DocEntry", 0).ToString()
                            End If

                            If pval.BeforeAction Then
                                strGenerarRequisicionDevolucion = oForm.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_GenReqDev", 0).ToString()
                            End If

                            'Generar las devoluciones para todos los repuestos y suministros que no se vayan a facturar
                            If Not String.IsNullOrEmpty(DocEntry) Then
                                'Durante el BeforeAction, se guarda el estado de las lineas de la orden de venta en memoria
                                'ya que no obligatoriamente estan actualizadas, el usuario debe hacer clic en el botón actualizar para confirmar el cierre.
                                oOrdenVentaAbierta = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)
                                oOrdenVentaAbierta.GetByKey(DocEntry)
                                boolCierreParcial = True
                            End If
                    End Select
                End If

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Manejador de eventos tipo FormData
    ' ''' </summary>
    ' ''' <param name="BusinessObjectInfo"></param>
    ' ''' <param name="BubbleEvent"></param>
    ' ''' <remarks></remarks>
    'Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles SBO_Application.FormDataEvent
    '    Try
    '        Dim strKey As String = ""
    '        Dim xmlDocKey As New Xml.XmlDocument

    '        Select Case BusinessObjectInfo.FormTypeEx
    '            'Orden de venta clientes
    '            Case "139"
    '                Select Case BusinessObjectInfo.EventType
    '                    Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD, SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE, SAPbouiCOM.BoEventTypes.et_FORM_ACTIVATE
    '                        DocEntry = String.Empty
    '                        If BusinessObjectInfo.ActionSuccess Then
    '                            xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
    '                            Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
    '                            If Not String.IsNullOrEmpty(strKey) Then
    '                                DocEntry = strKey
    '                            End If
    '                        End If
    '                End Select
    '        End Select

    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, SBO_Application)
    '    End Try
    'End Sub

    'Private Sub GuardarDocEntryMemoria()

    '    Try
    '        Dim strKey As String = ""
    '        Dim xmlDocKey As New Xml.XmlDocument

    '        xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
    '        Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
    '        If Not String.IsNullOrEmpty(strKey) Then
    '            DocEntry = strKey
    '        End If
    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, SBO_Application)
    '    End Try


    'End Sub

    Private oDataTableConfiguracionesSucursal As System.Data.DataTable
    Private oDataRowConfiguracionSucursal As System.Data.DataRow

    ''' <summary>
    ''' Crea una requisición de devolución si se realiza una factura para un documento ligado a una OT
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerarDevoluciones(ByVal p_strDocEntryOrdenVenta As String, ByRef BubbleEvent As Boolean, ByRef p_oOrdenVenta As SAPbobsCOM.Documents)
        Dim boolUsaTallerInterno = False
        Dim strIDSucursal As String = String.Empty
        Dim strSerieTransferencias As String = String.Empty
        Dim boolUsaRequisiciones As Boolean = False
        Dim strNumeroOT As String = String.Empty

        'Objetos
        Dim oOrdenVenta As Documents

        Try
            oOrdenVenta = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)
            oOrdenVenta.GetByKey(p_strDocEntryOrdenVenta)

            strNumeroOT = oOrdenVenta.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim()
            strIDSucursal = oOrdenVenta.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()
            'strNumeroOT = FormNotCredito.DataSources.DBDataSources.Item("ORIN").GetValue("U_SCGD_Numero_OT", 0).Trim()
            'strIDSucursal = FormNotCredito.DataSources.DBDataSources.Item("ORIN").GetValue("U_SCGD_idSucursal", 0).Trim()

            'Verifica que la nota de crédito este ligada a una orden de trabajo, de lo contrario no es necesario realizar devoluciones
            If Not String.IsNullOrEmpty(strNumeroOT) Then

                boolUsaTallerInterno = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
                oDataTableConfiguracionesSucursal = Utilitarios.ObtenerConsultaConfiguracionPorSucursal(strIDSucursal, m_oCompany)
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
                        If oOrdenVenta.DocType = BoDocumentTypes.dDocument_Items Then
                            GenerarRequisicionDevolucion(BubbleEvent, strNumeroOT, strSerieTransferencias, p_strDocEntryOrdenVenta, False, -1, p_oOrdenVenta)
                        End If
                    End If

                End If

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
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

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_ADMIN")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", strCode)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            strValor = oGeneralData.GetProperty("U_GenReqDev").ToString()

            Return strValor

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Function

    Private Enum enumTipoArticulo
        Repuesto = 1
        Suministro = 3
    End Enum

    Private Enum enumTipoRequisicion
        Transferencia = 1
        Devolucion = 2
    End Enum

    ''' <summary>
    ''' Genera requisiciones de devolución
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GenerarRequisicionDevolucion(ByRef BubbleEvent As Boolean, ByVal p_strNumeroOT As String, ByVal p_strSerieTransferencias As String, ByVal p_strDocEntryOrdenVenta As String, ByVal p_boolCerrarLinea As Boolean, ByVal p_intNumeroLinea As Integer, ByRef p_oOrdenVentaAbierta As SAPbobsCOM.Documents)

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
        Dim oOrdenVenta As Documents

        'Enumeraciones
        Dim TipoArticulo As enumTipoArticulo

        Try

            oOrdenVenta = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)
            m_intDocEntry = CInt(p_strDocEntryOrdenVenta)

            'Procesa el documento
            If oOrdenVenta.GetByKey(m_intDocEntry) And m_intDocEntry > 0 Then

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

                    oCompanyService = m_oCompany.GetCompanyService()
                    oGeneralService = oCompanyService.GetGeneralService("SCGD_REQ")
                    oRequisicion = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralData)
                    oChildrenLineasReq = oRequisicion.Child("SCGD_LINEAS_REQ")

                    strIDSucursal = oOrdenVenta.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim()
                    'Consulta las bodegas a utilizar para la devolución
                    If Not String.IsNullOrEmpty(strIDSucursal) Then

                        oItem = m_oCompany.GetBusinessObject(BoObjectTypes.oItems)

                        'Obtiene la información del asesor desde el maestro de empleados
                        oEmployeesInfo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oEmployeesInfo)

                        If Not oEmployeesInfo Is Nothing Then
                            oEmployeesInfo.GetByKey(oOrdenVenta.DocumentsOwner)
                            strNombreAsesor = String.Format("{0} {1}", oEmployeesInfo.FirstName, oEmployeesInfo.LastName)
                        End If

                        strComentarios = String.Format("{0} {1} {2} {3}", My.Resources.Resource.OT_Referencia, p_strNumeroOT, My.Resources.Resource.Asesor, strNombreAsesor)

                        'Encabezado de la requisición
                        oRequisicion.SetProperty("U_SCGD_NoOrden", p_strNumeroOT)
                        oRequisicion.SetProperty("U_SCGD_CodCliente", oOrdenVenta.CardCode)
                        oRequisicion.SetProperty("U_SCGD_NombCliente", oOrdenVenta.CardName)
                        oRequisicion.SetProperty("U_SCGD_TipoReq", My.Resources.Resource.Devolucion)
                        oRequisicion.SetProperty("U_SCGD_CodTipoReq", CInt(enumTipoRequisicion.Devolucion))
                        oRequisicion.SetProperty("U_SCGD_TipoDoc", "Transf. Inv")
                        oRequisicion.SetProperty("U_SCGD_Usuario", m_oCompany.UserName)
                        oRequisicion.SetProperty("U_SCGD_Comm", strComentarios)
                        oRequisicion.SetProperty("U_SCGD_TipArt", CInt(eTipoArticulo).ToString())
                        oRequisicion.SetProperty("U_SCGD_CodEst", EstadosLineas.Pendiente)
                        oRequisicion.SetProperty("U_SCGD_Est", My.Resources.Resource.Pendiente)
                        oRequisicion.SetProperty("U_ActualizaDoc", "N")

                        'Metadata del encabezado
                        Dim m_objData As EncabezadoTrasladoDMSData = New EncabezadoTrasladoDMSData()
                        m_objData.TipoTransferencia = enumTipoRequisicion.Devolucion
                        m_objData.Serie = p_strSerieTransferencias
                        m_objData.NumCotizacionOrigen = oOrdenVenta.DocEntry

                        oRequisicion.SetProperty("U_SCGD_Data", m_objData.Serialize())
                        oRequisicion.SetProperty("U_SCGD_IDSuc", oOrdenVenta.UserFields.Fields.Item("U_SCGD_idSucursal").Value.ToString().Trim())

                        'Información del vehículo
                        oRequisicion.SetProperty("U_SCGD_Placa", oOrdenVenta.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim())
                        oRequisicion.SetProperty("U_SCGD_Marca", oOrdenVenta.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString().Trim())
                        oRequisicion.SetProperty("U_SCGD_Estilo", oOrdenVenta.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString().Trim())
                        oRequisicion.SetProperty("U_SCGD_VIN", oOrdenVenta.UserFields.Fields.Item("U_SCGD_Num_VIN").Value.ToString().Trim())

                        'Recorre las líneas de la orden de venta y genera las devoluciones para los artículos inventariables
                        'como repuestos o suministros
                        For i As Integer = 0 To oOrdenVenta.Lines.Count - 1

                            oOrdenVenta.Lines.SetCurrentLine(i)
                            p_oOrdenVentaAbierta.Lines.SetCurrentLine(i)

                            oItem.GetByKey(oOrdenVenta.Lines.ItemCode)

                            'Verifica si la nota de crédito produce movimiento de inventario
                            'en caso de no producir no se deben generar devoluciones para esta línea.
                            Dim boolSinContabilizacionStock = False

                            If oOrdenVenta.Lines.WithoutInventoryMovement = BoYesNoEnum.tYES Then
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

                            'Si la bodega destino es la misma que la bodega origen se omite la línea de la requisición
                            If strBodegaStock = oOrdenVenta.Lines.WarehouseCode Then
                                m_boolProcesarLinea = False
                            End If

                            Dim strItemCode1 As String = String.Empty
                            Dim strItemCode2 As String = String.Empty
                            Dim strEstado1 As String = String.Empty
                            Dim strEstado2 As String = String.Empty

                            strItemCode1 = oOrdenVenta.Lines.ItemCode
                            strItemCode2 = p_oOrdenVentaAbierta.Lines.ItemCode
                            strEstado1 = oOrdenVenta.Lines.LineStatus.ToString()
                            strEstado2 = p_oOrdenVentaAbierta.Lines.LineStatus.ToString()

                            'Solamente se generan devoluciones para las lineas que estaban abiertas y pasan a estado cerrado
                            If Not ((oOrdenVenta.Lines.LineStatus = BoStatus.bost_Close And p_oOrdenVentaAbierta.Lines.LineStatus = BoStatus.bost_Open) And m_boolProcesarLinea = True) Then
                                m_boolProcesarLinea = False
                            End If

                            'Agrega la linea a la requisicion
                            If m_boolProcesarLinea = True Then

                                oReqLinea = oChildrenLineasReq.Add()

                                'Completa la información de las columnas de la tabla hija "@SCGD_LINEAS_REQ" con los datos de la requisición
                                oReqLinea.SetProperty("U_SCGD_CodArticulo", oOrdenVenta.Lines.ItemCode)
                                oReqLinea.SetProperty("U_SCGD_DescArticulo", oItem.ItemName)
                                oReqLinea.SetProperty("U_SCGD_ID", oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_ID").Value)
                                oReqLinea.SetProperty("U_SCGD_CodBodOrigen", oOrdenVenta.Lines.WarehouseCode)
                                oReqLinea.SetProperty("U_SCGD_CodBodDest", strBodegaStock)
                                oReqLinea.SetProperty("U_SCGD_CantRec", 0)
                                oReqLinea.SetProperty("U_SCGD_CodEst", EstadosLineas.Pendiente)
                                oReqLinea.SetProperty("U_SCGD_CCosto", strCentroCosto)
                                oReqLinea.SetProperty("U_SCGD_LNumOr", oOrdenVenta.Lines.LineNum)
                                oReqLinea.SetProperty("U_SCGD_COrig", p_oOrdenVentaAbierta.Lines.RemainingOpenQuantity)
                                oReqLinea.SetProperty("U_SCGD_CantSol", p_oOrdenVentaAbierta.Lines.RemainingOpenQuantity)
                                oReqLinea.SetProperty("U_SCGD_Lidsuc", oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value)
                                oReqLinea.SetProperty("U_SCGD_CodTipoArt", strCodTipoArticulo)
                                oReqLinea.SetProperty("U_SCGD_TipoArticulo", strTipoArticulo)
                                oReqLinea.SetProperty("U_SCGD_Estado", My.Resources.Resource.Pendiente)
                                oReqLinea.SetProperty("U_SCGD_DocOr", oOrdenVenta.DocEntry)

                                If (m_oCompany.Version >= 900000) Then
                                    Dim strUbicacion As String = String.Empty
                                    strUbicacion = CargaUbicacion(oItem.ItemCode, strBodegaStock, oItem.ItemsGroupCode)
                                    oReqLinea.SetProperty("U_DeUbic", String.Empty)
                                    oReqLinea.SetProperty("U_AUbic", strUbicacion)
                                End If

                                Dim cantidadDisponible As Double = 0
                                cantidadDisponible = ObtenerCantidadDisponible(oItem.ItemCode, oOrdenVenta.Lines.WarehouseCode)
                                oReqLinea.SetProperty("U_SCGD_CantDispo", cantidadDisponible)

                                'Si se agrega al menos 1 línea, se puede crear el documento
                                m_boolCrearDocumento = True
                            End If
                        Next

                    End If

                    If m_boolCrearDocumento Then
                        'Inicia la transacción
                        If Not m_oCompany.InTransaction Then
                            m_oCompany.StartTransaction()
                        End If

                        'Genera la requisición de devolución
                        oGeneralService.Add(oRequisicion)

                        m_oCompany.GetLastError(intError, strErrorMsj)

                        If intError <> 0 Then
                            m_boolGenerarRollback = True
                            BubbleEvent = False
                        End If

                        'Finaliza la transacción
                        If m_oCompany.InTransaction Then
                            If m_boolGenerarRollback Then
                                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                            Else
                                m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                            End If
                        End If
                    End If
                Next
            Else
                'Mensaje de error número de documento inválido o incorrecto
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorNumeroDocumento, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            End If

            Utilitarios.DestruirObjeto(p_oOrdenVentaAbierta)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            BubbleEvent = False
            'En caso de errores finaliza la transacción y genera un rollback.
            If m_oCompany.InTransaction Then
                If m_boolGenerarRollback Then
                    m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
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
            oArticulo = m_oCompany.GetBusinessObject(BoObjectTypes.oItems)
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
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

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
            DMS_Connector.Helpers.ManejoErrores(ex)
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
            oArticulo = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

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
        Dim oBodega As SAPbobsCOM.Warehouses

        Try

            oIItemGroups = m_oCompany.GetBusinessObject(BoObjectTypes.oItemGroups)
            oBodega = m_oCompany.GetBusinessObject(BoObjectTypes.oWarehouses)

            If oBodega.GetByKey(p_strBodegaUbicacion) AndAlso oBodega.EnableBinLocations = BoYesNoEnum.tYES Then
                If oIItemGroups.GetByKey(p_intItemGroupCode) Then
                    For i As Integer = 0 To oIItemGroups.WarehouseInfo.Count - 1
                        oIItemGroups.WarehouseInfo.SetCurrentLine(i)

                        If oIItemGroups.WarehouseInfo.WarehouseCode = p_strBodegaUbicacion Then
                            If oIItemGroups.WarehouseInfo.DefaultBin > 0 Then
                                strUbicacion = oIItemGroups.WarehouseInfo.DefaultBin.ToString().Trim()
                                Exit For
                            End If
                        End If
                    Next
                End If
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

            oIWarehouses = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses)

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

#Region "Metodos Nuevos"

    ''' <summary>
    ''' Verifica si se agregó una línea adicional a la orden de venta posterior al cierre de la orden de trabajo
    ''' De acuerdo a la configuración de la sucursal, permite agregarla asignándo un ID o bien rechazando la línea.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ValidaLineaAdicional(ByRef p_oFormulario As SAPbouiCOM.Form)
        Dim strIDSucursal As String = String.Empty
        Dim strUsaLineasAdicionalesOV As String = String.Empty

        Try
            'Obtiene el ID de la sucursal
            strIDSucursal = p_oFormulario.DataSources.DBDataSources.Item("ORDR").GetValue("U_SCGD_idSucursal", 0).Trim()

            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal))
                    strUsaLineasAdicionalesOV = .U_UsaLAOV
                End With
            End If

            If Not String.IsNullOrEmpty(strUsaLineasAdicionalesOV) AndAlso strUsaLineasAdicionalesOV.ToUpper.Equals("Y") Then
                'Si el parámetro para manejo de línea adicionales está activo, se procede a completar la información de la línea
                CompletarInformacionLineaAdicional(p_oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CompletarInformacionLineaAdicional(ByRef p_oFormulario As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strID As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strItemCode As String = String.Empty
        Dim strDocEntry As String = String.Empty
        Dim oDocument As SAPbobsCOM.Documents
        Dim blnActualizarDocumento As Boolean = False
        Dim strNumeroOT As String = String.Empty

        Try
            'Abre el documento que está en pantalla por medio del DI API y actualiza los valores correspondientes
            strDocEntry = p_oFormulario.DataSources.DBDataSources.Item("ORDR").GetValue("DocEntry", 0).ToString()
            oDocument = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)
            oDocument.GetByKey(strDocEntry)
            strNumeroOT = oDocument.UserFields.Fields.Item("U_SCGD_Numero_OT").Value.ToString().Trim()

            If Not String.IsNullOrEmpty(strNumeroOT) Then
                For i As Integer = 0 To oDocument.Lines.Count - 1
                    oDocument.Lines.SetCurrentLine(i)
                    strItemCode = oDocument.Lines.ItemCode
                    strTipoArticulo = oDocument.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString().Trim()
                    strID = oDocument.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim()
                    If String.IsNullOrEmpty(strTipoArticulo) AndAlso String.IsNullOrEmpty(strID) Then
                        oDocument.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = GeneraID(oDocument.Lines)
                        oDocument.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = ObtenerTipoArticulo(oDocument.Lines)
                        blnActualizarDocumento = True
                    End If
                Next
            End If

            If blnActualizarDocumento Then
                oDocument.Update()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el tipo de artículo de la línea seleccionada."
    ''' </summary>
    ''' <param name="p_oDocumentLine">Objeto Document_Lines con la línea seleccionada</param>
    ''' <returns>Campo "U_SCGD_TipoArticulo" de la tabla OITM en formato texto</returns>
    ''' <remarks></remarks>
    Private Function ObtenerTipoArticulo(ByRef p_oDocumentLine As SAPbobsCOM.Document_Lines) As String
        Dim strTipoArticulo As String = String.Empty
        Dim strItemCode As String = String.Empty
        Dim strQuery As String = "SELECT T0.""U_SCGD_TipoArticulo"" FROM OITM T0 WHERE T0.""ItemCode"" = '{0}'"

        Try
            strItemCode = p_oDocumentLine.ItemCode
            If Not String.IsNullOrEmpty(strItemCode) Then
                strQuery = String.Format(strQuery, strItemCode)
                strTipoArticulo = DMS_Connector.Helpers.EjecutarConsulta(strQuery)
            End If

            Return strTipoArticulo
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Genera un ID único para las líneas adicionales en las ordenes de venta. Este ID no aplica para el resto de líneas procesadas en forma tradicional.
    ''' </summary>
    ''' <param name="strIDSucursal">ID de la Sucursal en formato texto. Ejemplos: -2,3,5,7</param>
    ''' <param name="intLineNum">Siguiente número de línea del documento base. Recordar que el número de ID se obtiene con base a la línea proviene de la oferta de ventas.</param>
    ''' <param name="strNoOT">Número de orden de trabajo. Ejemplos: 450-01, 896-01.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GeneraID(ByRef p_oDocumentLine As SAPbobsCOM.Document_Lines) As String
        Dim strIDGenerado As String = String.Empty
        Dim strObjectType As String = String.Empty
        Dim strLineNum As Integer
        Dim strDocEntry As String = String.Empty

        Try
            strObjectType = "17" 'Object Type de las ordenes de venta
            strDocEntry = p_oDocumentLine.DocEntry
            strLineNum = p_oDocumentLine.LineNum

            If Not String.IsNullOrEmpty(strObjectType) AndAlso Not String.IsNullOrEmpty(strDocEntry) AndAlso Not String.IsNullOrEmpty(strLineNum) Then
                strIDGenerado = String.Format("{0}-{1}-{2}", strObjectType, strDocEntry, strLineNum)
            End If

            Return strIDGenerado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el siguiente número de línea de la oferta de ventas base
    ''' </summary>
    ''' <param name="strBaseDocEntry"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function NextLineNum(ByVal strBaseDocEntry As String) As Integer
        Dim intLineNum As Integer = -1
        Dim strLineNum As String = String.Empty
        Dim strQuery As String = "SELECT MAX(T0.""LineNum"") FROM ""QUT1"" T0 WHERE T0.""DocEntry"" = '{0}'"

        Try
            If Not String.IsNullOrEmpty(strBaseDocEntry) Then
                'Consulta el último número de línea de la oferta de ventas base y le suma 1
                strQuery = String.Format(strQuery, strBaseDocEntry)
                strLineNum = DMS_Connector.Helpers.EjecutarConsulta(strQuery)
                If Integer.TryParse(strLineNum, intLineNum) Then
                    'Le suma 1 ya que se desea obtener el siguiente ID
                    intLineNum += 1
                End If

            End If
            Return intLineNum
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Function ExisteID(ByVal strID As String) As Boolean
        Dim strCount As String = String.Empty
        Dim strQuery As String = "SELECT Count(*) FROM ""QUT1"" T0 WHERE T0.""U_SCGD_ID"" = '{0}'"

        Try
            strQuery = String.Format(strQuery, strID)
            strCount = DMS_Connector.Helpers.EjecutarConsulta(strQuery)

            If strCount.Equals("0") Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

#End Region

End Class
