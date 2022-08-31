Imports DMSOneFramework
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports SAPbobsCOM
Imports System.Collections.Generic
Imports System.Globalization

Public Class SolicitudOrdenEspecial

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private SBO_Application As Application

    Public n As NumberFormatInfo
    
    Public m_objBLSBO As New BLSBO.GlobalFunctionsSBO

    Private _dtEncabezado As DataTable

    Dim intError As Integer
    Dim strMensajeError As String


    'Constantes aplicables a la cotización
    Private Const mc_strNumUnidad As String = "U_SCGD_Cod_Unidad"
    Private Const mc_strNumVehiculo As String = "U_SCGD_Num_Vehiculo"
    Private Const mc_strTipoOT As String = "U_SCGD_Tipo_OT"
    Private Const mc_strNoVisita As String = "U_SCGD_No_Visita"
    Private Const mc_strEstadoCotizacion As String = "U_SCGD_Estado_Cot"
    Private Const mc_strEstadoCotizacionID As String = "U_SCGD_Estado_CotID"
    Private Const mc_strCardNameOrig As String = "U_SCGD_CardNameOrig"
    Private Const mc_strCardCodeOrig As String = "U_SCGD_CardCodeOrig"
    Private Const mc_strOTPadre As String = "U_SCGD_OT_Padre"
    Private Const mc_strAno_Vehi As String = "U_SCGD_Ano_Vehi"
    Private Const mc_strCod_Marca As String = "U_SCGD_Cod_Marca"
    Private Const mc_strCod_Modelo As String = "U_SCGD_Cod_Modelo"
    Private Const mc_strNum_VIN As String = "U_SCGD_Num_VIN"
    Private Const mc_strCosto As String = "U_SCGD_Costo"
    Private Const mc_strNum_Placa As String = "U_SCGD_Num_Placa"
    Private Const mc_strCod_Estilo As String = "U_SCGD_Cod_Estilo"
    Private Const mc_strDes_Marc As String = "U_SCGD_Des_Marc"
    Private Const mc_strDes_Mode As String = "U_SCGD_Des_Mode"
    Private Const mc_strDes_Esti As String = "U_SCGD_Des_Esti"
    Private Const mc_strNoOtRef As String = "U_SCGD_NoOtRef"
    Private Const mc_strFechaRecepcion As String = "U_SCGD_Fech_Recep"
    Private Const mc_strFechaCompromiso As String = "U_SCGD_Fech_Comp"
    Private Const mc_strIdSucursal As String = "U_SCGD_idSucursal"

    'Constantes aplicables a las líneas de la cotizacion
    Private Const mc_strItemAprobado As String = "U_SCGD_Aprobado"
    Private Const mc_strIdRepxOrd As String = "U_SCGD_IdRepxOrd"
    Private blnDocumentoCerrado As Boolean = False


    Public Property dtEncabezado As DataTable
        Get
            Return _dtEncabezado
        End Get
        Set(ByVal value As DataTable)
            _dtEncabezado = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As SAPbobsCOM.Company, ByVal p_strUISCGD_SolicituOTE As String)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLSolicitudOTEspecial
        MenuPadre = "SCGD_GOV"
        Nombre = My.Resources.Resource.TituloSolicitudOTEspecial
        IdMenu = p_strUISCGD_SolicituOTE
        Posicion = 6
        FormType = p_strUISCGD_SolicituOTE

    End Sub

#End Region

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String = String.Empty

        If Utilitarios.MostrarMenu("SCGD_SOT", SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_SOT", ApplicationSBO.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_SOT", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 25, False, True, "SCGD_CFG"))
        End If

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoLoad(ByVal p_Form As SAPbouiCOM.Form, _
                                   ByRef BubbleEvent As Boolean)

        Dim p_matriz As SAPbouiCOM.Matrix
        p_matriz = p_Form.Items.Item("mtxLinCz").Specific

        Try

            If Not p_Form Is Nothing Then

                If p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CotCread", 0).Trim = "Y" Or _
                    p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("Status", 0).Trim = "C" Then
                    p_Form.Items.Item("1").Enabled = False
                    p_Form.Items.Item("chkImp").Enabled = False
                    p_Form.Items.Item("cboStatus").Enabled = False
                    p_Form.Items.Item("chkAll").Enabled = False
                    p_Form.Items.Item("btnAddQ").Enabled = False
                    p_Form.Items.Item("54").Enabled = False
                    p_matriz.Columns.Item("col_Sel").Editable = False

                Else
                    p_Form.Items.Item("1").Enabled = True
                    p_Form.Items.Item("chkImp").Enabled = True
                    p_Form.Items.Item("cboStatus").Enabled = True
                    p_Form.Items.Item("chkAll").Enabled = True
                    p_Form.Items.Item("btnAddQ").Enabled = True
                    p_Form.Items.Item("54").Enabled = True
                    p_matriz.Columns.Item("col_Sel").Editable = True

                End If

            End If

        Catch ex As Exception
            m_oCompany.GetLastError(intError, strMensajeError)
            Throw New Exception(strMensajeError)
            BubbleEvent = False
        End Try

    End Sub

    Public Function ValidarFilasSeleccionadas(ByVal p_form As SAPbouiCOM.Form) As Boolean

        Dim xmlDocMatrix As Xml.XmlDocument
        Dim XmlNode As Xml.XmlNode
        Dim matrixXml As String
        Dim p_matriz As SAPbouiCOM.Matrix

        p_matriz = p_form.Items.Item("mtxLinCz").Specific
        matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

        xmlDocMatrix = New Xml.XmlDocument
        xmlDocMatrix.LoadXml(matrixXml)

        For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")

            Dim elementoSeleccion As Xml.XmlNode

            elementoSeleccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Sel']")

            If elementoSeleccion.InnerText = "Y" Then

                Return True

            End If

        Next

    End Function

    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                                  ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                  ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.Item(FormUID)


        Try

            Dim iReturnValue As Integer
            'If pVal.ActionSuccess = True AndAlso pVal.BeforeAction = False Then

            ' If pVal.BeforeAction Then

            If Not oForm Is Nothing Then

                If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

                    If pVal.ItemUID = "btnAddQ" Then

                        If oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CotCread", 0).Trim = "N" Then

                            If FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeActualiceSolicitudOT, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                            ElseIf FormularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then

                                If Not ValidarFilasSeleccionadas(oForm) Then
                                    SBO_Application.StatusBar.SetText("Seleccione algunas de las Filas para crear la Orden de Trabajo. ", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                                    Exit Sub
                                End If

                                CrearCotizacionParaOT(FormularioSBO)

                            ElseIf oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("Status", 0).Trim = "C" Then

                                '    iReturnValue = SBO_Application.MessageBox("Esta seguro que desea Cerrar la Solicitud de Orden Especial?", Nothing, "Si", "No")

                                '    Select Case iReturnValue
                                '        Case 1
                                '            FormularioSBO.Items.Item("1").Click()
                                '        Case 2
                                '            Exit Sub
                                '    End Select

                                'End If

                            Else
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeActualiceSolicitudOT, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                            End If
                        End If

                    ElseIf pVal.ItemUID = CheckBoxSel.UniqueId Then


                        If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE Then
                            If pVal.ActionSuccess = True And pVal.BeforeAction = False Then
                                Dim p_matriz As SAPbouiCOM.Matrix

                                p_matriz = FormularioSBO.Items.Item("mtxLinCz").Specific

                                Dim strSeleccionTodas As String = String.Empty

                                strSeleccionTodas = CheckBoxSel.ObtieneValorUserDataSource()

                                SeleccionarTodasFilas(p_matriz, strSeleccionTodas)
                            End If

                        End If


                    ElseIf pVal.ItemUID = "1" Then

                        If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE OrElse FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then

                            If Not pVal.ActionSuccess = True AndAlso pVal.BeforeAction = True Then
                                Dim p_matriz As SAPbouiCOM.Matrix

                                p_matriz = FormularioSBO.Items.Item("mtxLinCz").Specific

                                If oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("Status", 0).Trim = "C" Then

                                    iReturnValue = SBO_Application.MessageBox("Esta seguro que desea Cerrar la Solicitud de Orden Especial?", Nothing, "Si", "No")

                                    Select Case iReturnValue
                                        Case 1
                                            ''deshabilito algunos items para que no se pueda generar la cotizacion
                                            ''en caso de ser cerrada
                                            'FormularioSBO.Items.Item("54").Click()
                                            ' ''FormularioSBO.Items.Item("1").Enabled = False
                                            'FormularioSBO.Items.Item("chkImp").Enabled = False
                                            'FormularioSBO.Items.Item("cboStatus").Enabled = False
                                            'FormularioSBO.Items.Item("chkAll").Enabled = False
                                            'FormularioSBO.Items.Item("btnAddQ").Enabled = False
                                            'p_matriz.Columns.Item("col_Sel").Editable = False
                                            blnDocumentoCerrado = True
                                            ''FormularioSBO.Mode = BoFormMode.fm_OK_MODE
                                            'BubbleEvent = True
                                            ''FormularioSBO.Close()

                                        Case 2
                                            BubbleEvent = False
                                            Exit Sub
                                            blnDocumentoCerrado = False
                                    End Select

                                End If
                            End If
                        End If
                    End If

                ElseIf pVal.EventType = BoEventTypes.et_MENU_CLICK Then

                    If oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CotCread", 0).Trim = "Y" Then

                        oForm.Items.Item("1").Enabled = False

                    End If


                End If
            End If
            'End If
            ' End If

        Catch ex As SCGCommon.ExceptionsSBO
            m_oCompany.GetLastError(intError, strMensajeError)
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw New SCGCommon.ExceptionsSBO(strMensajeError, ex)
            BubbleEvent = False

        Catch ex As Exception
            m_oCompany.GetLastError(intError, strMensajeError)
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw New SCGCommon.ExceptionsSBO(strMensajeError, ex)
            BubbleEvent = False

        Finally
            If pVal.ActionSuccess = True And pVal.BeforeAction = False Then
                If blnDocumentoCerrado Then
                    If oForm.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("Status", 0).Trim = "C" Then
                        FormularioSBO.Close()
                    End If
                    blnDocumentoCerrado = False

                End If

            End If
        End Try
    End Sub

    Public Sub CrearCotizacionParaOT(ByVal p_Form As Form)

        Dim objCotizacionCreada As CotizacionCLS
        Dim xmlDocMatrix As Xml.XmlDocument
        Dim matrixXml As String
        Dim p_matriz As Matrix
        Dim blnCreaOT As Boolean = False

        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty
        Dim strSucursal As String
        Dim blnUsaTallerInterno As Boolean
        Dim blnSolOTEs_ContieneSE As Boolean = False
        Dim estadosActividades As List(Of Integer) = New List(Of Integer)
        Dim estado As Integer

        Try
            blnUsaTallerInterno = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

            p_matriz = p_Form.Items.Item("mtxLinCz").Specific
            matrixXml = p_matriz.SerializeAsXML(BoMatrixXmlSelect.mxs_All)

            xmlDocMatrix = New Xml.XmlDocument
            xmlDocMatrix.LoadXml(matrixXml)

            Dim blnAgregarFila As Boolean = False

            Dim CotizacionPadre As String = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CotRef", 0).Trim()

            If Not ValidarEstadoCotizacion(p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CotRef", 0).Trim()) Then
                SBO_Application.SetStatusBarMessage(String.Format(My.Resources.Resource.CotizacionCerradaCancelada, CotizacionPadre), BoMessageTime.bmt_Short, True)
                Exit Sub
            End If

            objCotizacionCreada = New CotizacionCLS(SBO_Application, m_oCompany)

            Dim objCotizacion As Documents
            objCotizacion = m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations)

            Dim objCotizacionPadre As Documents
            Dim oLineasCotizacionPadre As Document_Lines

            objCotizacionPadre = m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations)
            objCotizacionPadre.GetByKey(Convert.ToInt32(CotizacionPadre))
            oLineasCotizacionPadre = objCotizacionPadre.Lines


            Dim intResultado As Integer

            objCotizacion.CardCode = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Clie", 0).Trim()

            If Not p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Nom_Clie", 0).Trim = String.Empty Then

                objCotizacion.CardName = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Nom_Clie", 0).Trim()
            End If

            objCotizacion.Comments = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Comment", 0).Trim()

            objCotizacion.Series = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Series", 0).Trim()

            objCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CardCodeOrig", 0).Trim()
            objCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CardNameOrig", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Uni", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Id_Vehi", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strAno_Vehi).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Anno", 0).Trim
            objCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Mar", 0).Trim
            objCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Mod", 0).Trim
            objCotizacion.UserFields.Fields.Item(mc_strNum_VIN).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_VIN", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Placa", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Est", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strDes_Marc).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Des_Mar", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strDes_Mode).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Des_Mod", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strDes_Esti).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Des_Est", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strFechaRecepcion).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Fec_Ape", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strFechaCompromiso).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Fec_Com", 0).Trim()
            objCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_klm", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strCardCodeOrig).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CardCodeOrig", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strCardNameOrig).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_CardNameOrig", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strNoVisita).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_No_Vis", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_TipoOrd", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_OTRefer", 0).Trim()
            objCotizacion.UserFields.Fields.Item(mc_strNoOtRef).Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_OTRefer", 0).Trim()
            objCotizacion.UserFields.Fields.Item("U_SCGD_HoSr").Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_HoraServicio", 0).Trim()
            strSucursal = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_SCGD_idSucursal FROM OQUT with (nolock) WHERE U_SCGD_Numero_OT = '{0}' ",
                                                                    p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_OTRefer", 0).Trim()),
                                                                    m_oCompany.CompanyDB, m_oCompany.Server)
            objCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value = strSucursal

            If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                If Not String.IsNullOrEmpty(strSucursal) Then
                    objCotizacion.BPL_IDAssignedToInvoice = Integer.Parse(strSucursal)
                End If
            End If


            objCotizacion.DocumentsOwner = If(Not String.IsNullOrEmpty(p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Ases", 0).Trim()), p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_Cod_Ases", 0).ToString().Trim(), 0)

            If p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_ImpRecp", 0).Trim() = "Y" Then
                objCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "1"
            ElseIf p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_ImpRecp", 0).Trim() = "N" Then
                objCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "2"
            End If

            For Each node As Xml.XmlNode In xmlDocMatrix.SelectNodes("/Matrix/Rows/Row")
                Dim elementoItemCode As Xml.XmlNode
                Dim elementoDescripcion As Xml.XmlNode
                Dim elementoCantidad As Xml.XmlNode
                Dim elementoPorcentajeDesc As Xml.XmlNode
                Dim elementoMoneda As Xml.XmlNode
                Dim elementoPrecio As Xml.XmlNode
                Dim elementoComentarios As Xml.XmlNode
                Dim elementoImpuesto As Xml.XmlNode
                Dim elementoIdRXO As Xml.XmlNode
                Dim elementoCosto As Xml.XmlNode
                Dim elementoSeleccion As Xml.XmlNode
                Dim elementoCPend As Xml.XmlNode
                Dim elementoCSol As Xml.XmlNode
                Dim elementoCRec As Xml.XmlNode
                Dim elementoCPDev As Xml.XmlNode
                Dim elementoCPTr As Xml.XmlNode
                Dim elementoCPBo As Xml.XmlNode
                Dim elementoCompra As Xml.XmlNode
                Dim elementoTipoArticulo As Xml.XmlNode
                Dim elementoID As Xml.XmlNode

                elementoItemCode = node.SelectSingleNode("Columns/Column/Value[../ID = 'colItemCo']")
                elementoDescripcion = node.SelectSingleNode("Columns/Column/Value[../ID = 'colDesc']")
                elementoCantidad = node.SelectSingleNode("Columns/Column/Value[../ID = 'colQtn']")
                elementoPorcentajeDesc = node.SelectSingleNode("Columns/Column/Value[../ID = 'colPorDs']")
                elementoMoneda = node.SelectSingleNode("Columns/Column/Value[../ID = 'colMoned']")
                elementoPrecio = node.SelectSingleNode("Columns/Column/Value[../ID = 'colPrec']")
                elementoComentarios = node.SelectSingleNode("Columns/Column/Value[../ID = 'colComen']")
                elementoImpuesto = node.SelectSingleNode("Columns/Column/Value[../ID = 'colTaxCd']")
                elementoIdRXO = node.SelectSingleNode("Columns/Column/Value[../ID = 'colIdRXO']")
                elementoCosto = node.SelectSingleNode("Columns/Column/Value[../ID = 'colCosto']")
                elementoSeleccion = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Sel']")
                elementoCPend = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_CPend']")
                elementoCSol = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_CSol']")
                elementoCRec = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_CRec']")
                elementoCPDev = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_PenDev']")
                elementoCPTr = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_PenTra']")
                elementoCPBo = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_PenBod']")
                elementoCompra = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Compra']")
                elementoTipoArticulo = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_TipAr']")
                elementoID = node.SelectSingleNode("Columns/Column/Value[../ID = 'col_ID']")

                If elementoSeleccion.InnerText = "Y" Then

                    If blnAgregarFila Then
                        objCotizacion.Lines.Add()
                    Else
                        blnAgregarFila = True
                    End If

                    Dim Precio As String = CStr(elementoPrecio.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                    Dim Costo As String = CStr(elementoCosto.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                    Dim Cantidad As String = CStr(elementoCantidad.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                    Dim PorcDescuento As String = CStr(elementoPorcentajeDesc.InnerText).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)

                    Dim decPrecio As Decimal = Decimal.Parse(Precio)
                    Dim decCosto As Decimal = Decimal.Parse(Costo)
                    Dim decCantidad As Decimal = Decimal.Parse(Cantidad)
                    Dim decPorcDescuento As Decimal = Decimal.Parse(PorcDescuento)

                    decPrecio = CDec(Utilitarios.CambiarValoresACultureActual(elementoPrecio.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCosto = CDec(Utilitarios.CambiarValoresACultureActual(elementoCosto.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCantidad = CDec(Utilitarios.CambiarValoresACultureActual(elementoCantidad.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decPorcDescuento = CDec(Utilitarios.CambiarValoresACultureActual(elementoPorcentajeDesc.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))

                    Dim decCPen As Decimal
                    Dim decCSol As Decimal
                    Dim decCRec As Decimal
                    Dim decCPDe As Decimal
                    Dim decCPTr As Decimal
                    Dim decCPBo As Decimal
                    Dim i As Integer

                    decCPen = CDec(Utilitarios.CambiarValoresACultureActual(elementoCPend.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCSol = CDec(Utilitarios.CambiarValoresACultureActual(elementoCSol.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCRec = CDec(Utilitarios.CambiarValoresACultureActual(elementoCRec.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCPDe = CDec(Utilitarios.CambiarValoresACultureActual(elementoCPDev.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCPTr = CDec(Utilitarios.CambiarValoresACultureActual(elementoCPTr.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                    decCPBo = CDec(Utilitarios.CambiarValoresACultureActual(elementoCPBo.InnerText, strSeparadorMilesSAP, strSeparadorDecimalesSAP))

                    objCotizacion.Lines.ItemCode = elementoItemCode.InnerText
                    objCotizacion.Lines.Currency = elementoMoneda.InnerText
                    objCotizacion.Lines.FreeText = elementoComentarios.InnerText
                    objCotizacion.Lines.TaxCode = elementoImpuesto.InnerText
                    objCotizacion.Lines.VatGroup = elementoImpuesto.InnerText

                    If blnUsaTallerInterno Then
                        objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = elementoID.InnerText
                    Else
                        objCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = elementoIdRXO.InnerText
                    End If

                    For i = 0 To oLineasCotizacionPadre.Count - 1
                        oLineasCotizacionPadre.SetCurrentLine(i)

                        If blnUsaTallerInterno Then
                            If oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value = elementoID.InnerText Then
                                'objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CtrCos").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_FasePro").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_TipoOrd", 0).Trim()
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Sucur").Value

                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = elementoTipoArticulo.InnerText

                                oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2
                                oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1

                                If elementoTipoArticulo.InnerText = "2" Then
                                    If oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value IsNot Nothing Then

                                        If Not String.IsNullOrEmpty(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim) Then
                                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value
                                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value
                                        End If
                                    End If
                                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value
                                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_DurSt").Value

                                    estadosActividades.Add(Convert.ToInt32(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString()))
                                End If
                            End If
                        Else
                            If oLineasCotizacionPadre.UserFields.Fields.Item(mc_strIdRepxOrd).Value = elementoIdRXO.InnerText Then
                                'objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = 2
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CtrCos").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_FasePro").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_TipoOrd", 0).Trim()
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Sucur").Value

                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = elementoTipoArticulo.InnerText

                                'oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                'oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Aprobado").Value = 2
                                'oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1

                                If elementoTipoArticulo.InnerText = "2" Then
                                    If oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value IsNot Nothing Then

                                        If Not String.IsNullOrEmpty(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim) Then
                                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value
                                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value
                                        End If

                                    End If
                                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value
                                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                    estadosActividades.Add(Convert.ToInt32(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString()))
                                End If

                            End If
                        End If
                    Next

                    'se modifica propiedades para corregir convesion de valores
                    objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = CDbl(decCosto)
                    objCotizacion.Lines.DiscountPercent = CDbl(decPorcDescuento)
                    objCotizacion.Lines.Quantity = CDbl(decCantidad)
                    objCotizacion.Lines.UnitPrice = CDbl(decPrecio)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = CDbl(decCPen)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = CDbl(decCSol)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = CDbl(decCRec)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = CDbl(decCPDe)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = CDbl(decCPTr)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = CDbl(decCPBo)
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = elementoCompra.InnerText
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = elementoTipoArticulo.InnerText
                    objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = elementoID.InnerText

                    If blnUsaTallerInterno Then
                        If elementoTipoArticulo.InnerText.Trim() = "4" Then
                            objCotizacionCreada.ListaItemsCodeOTEspeciales.Add(elementoItemCode.InnerText)
                            objCotizacionCreada.ListaIdRepxOrdenOTEspeciales.Add(elementoID.InnerText)
                            objCotizacionCreada.ListaNoOrdenOTEspeciales.Add(p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("U_OTRefer", 0).Trim())
                            ' ListaNoOrdenOTEspeciales.Add(m_drwRepuestos.NoOrden)

                            blnSolOTEs_ContieneSE = True
                        End If
                    End If
                    blnAgregarFila = True
                End If
            Next
            estado = 1
            Dim todasFin As Boolean = True
            Dim todasSusp As Boolean = True
            For Each itm As Integer In estadosActividades
                If (itm = 2) Then
                    estado = 2
                    Exit For
                ElseIf (itm = 3) Then
                    estado = 3
                    todasFin = False
                ElseIf itm = 4 Then
                    estado = 4
                    todasSusp = False
                End If
            Next
            If estado = 4 AndAlso Not todasFin Then
                estado = 3
            ElseIf estado = 4 AndAlso todasFin Then
                estado = 2
            End If

            Dim dscEstado As String
            ObtieneDescripcionEstado(estado, dscEstado, p_Form)
            objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = dscEstado
            objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = estado.ToString()

            blnCreaOT = True
            If blnCreaOT Then
                objCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = 1
            End If
            objCotizacionPadre.Update()

            intResultado = objCotizacion.Add()

            If intResultado <> 0 Then
                m_oCompany.GetLastError(intResultado, strMensajeError)
                Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
            Else

                intResultado = CInt(m_oCompany.GetNewObjectKey())

                objCotizacionCreada.CargarCotizacionAnterior("", blnCreaOT, intResultado)

                objCotizacionCreada.strIdSucursal = strSucursal

                objCotizacionCreada.ManejarCotizacion(blnUsaTallerInterno, intResultado, True, blnCreaOT, 0, Nothing, p_Form, blnSolOTEs_ContieneSE)

                ActualizarIdLineasHijasPaquetes(intResultado)

                ActualizarSolicituOT(p_Form.DataSources.DBDataSources.Item("@SCGD_SOT_ESP").GetValue("DocEntry", 0).Trim, intResultado)

                SBO_Application.MessageBox(String.Format(My.Resources.Resource.MensajeSolicitudCotizacionCreada, intResultado), 1)
                'SBO_Application.Menus.Item("1286").Activate()
                p_Form.Close()

            End If
        Catch ex As SCGCommon.ExceptionsSBO
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try


    End Sub

    Private Sub ObtieneDescripcionEstado(ByVal p_idEstado As String, ByRef m_strEstadoIniciadoDes As String, Optional ByVal oFormOt As SAPbouiCOM.Form = Nothing)
        Dim m_dtEstadosOT As SAPbouiCOM.DataTable

        If (oFormOt IsNot Nothing) Then
            If Utilitarios.ValidaExisteDataTable(oFormOt, "tEstadosOT") Then
                m_dtEstadosOT = oFormOt.DataSources.DataTables.Item("tEstadosOT")
            Else
                m_dtEstadosOT = oFormOt.DataSources.DataTables.Add("tEstadosOT")
                m_dtEstadosOT.ExecuteQuery(" select Code, Name from [@SCGD_ESTADOS_OT] with(nolock) order by Code ")
            End If
        Else
            If Utilitarios.ValidaExisteDataTable(FormularioSBO, "tEstadosOT") Then
                m_dtEstadosOT = FormularioSBO.DataSources.DataTables.Item("tEstadosOT")
            Else
                m_dtEstadosOT = FormularioSBO.DataSources.DataTables.Add("tEstadosOT")
                m_dtEstadosOT.ExecuteQuery(" select Code, Name from [@SCGD_ESTADOS_OT] with(nolock) order by Code ")
            End If
        End If
        For i As Integer = 0 To m_dtEstadosOT.Rows.Count - 1
            If (m_dtEstadosOT.GetValue("Code", i).ToString().Trim() = p_idEstado) Then
                m_strEstadoIniciadoDes = m_dtEstadosOT.GetValue("Name", i).ToString().Trim()
                Exit For
            End If
        Next
    End Sub

    Public Function CrearCotizacionParaOT_Aprobadas(ByVal p_numeroSOTE As Integer, Optional p_form As Form = Nothing) As Boolean
        Dim resultado As Boolean = True
        Dim objCotizacionCreada As CotizacionCLS
        Dim blnCreaOT As Boolean = False
        Dim strSeparadorDecimalesSAP As String = String.Empty
        Dim strSeparadorMilesSAP As String = String.Empty
        Dim strSucursal As String
        Dim estadosActividades As List(Of Integer) = New List(Of Integer)
        Dim estado As Integer
        Dim blnUsaTallerInterno As Boolean
        Dim blnSolOTEsp_ContieneSE As Boolean = False
        Dim CotizacionPadre As String

        blnUsaTallerInterno = Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO)
        Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)

        Dim oCompanyService As CompanyService
        Dim oGeneralService As GeneralService
        Dim oGeneralParams As GeneralDataParams
        Dim oGeneralData As GeneralData
        Dim oGeneralDataCollection As GeneralDataCollection
        Dim y As Integer

        objCotizacionCreada = New CotizacionCLS(SBO_Application, m_oCompany)
        oCompanyService = m_oCompany.GetCompanyService()
        oGeneralService = oCompanyService.GetGeneralService("SCGD_SOTESP")
        oGeneralParams = oGeneralService.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
        oGeneralParams.SetProperty("DocEntry", p_numeroSOTE)
        oGeneralData = oGeneralService.GetByParams(oGeneralParams)
        oGeneralDataCollection = oGeneralData.Child("SCGD_LINEAS_SOT_ESP")

        Dim blnAgregarFila As Boolean = False

        If Not ValidarEstadoCotizacion(oGeneralData.GetProperty("U_CotRef")) Then
            SBO_Application.SetStatusBarMessage(String.Format(My.Resources.Resource.CotizacionCerradaCancelada, CotizacionPadre), BoMessageTime.bmt_Short, True)
            Exit Function
        End If
        CotizacionPadre = oGeneralData.GetProperty("U_CotRef").ToString().Trim()
        Dim objCotizacion As Documents
        Dim objCotizacionPadre As Documents
        objCotizacion = m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations)
        objCotizacionPadre = m_oCompany.GetBusinessObject(BoObjectTypes.oQuotations)
        objCotizacionPadre.GetByKey(Convert.ToInt32(CotizacionPadre))

        Dim oLineasCotizacionPadre As Document_Lines
        oLineasCotizacionPadre = objCotizacionPadre.Lines

        Dim intResultado As Integer

        objCotizacion.CardCode = oGeneralData.GetProperty("U_Cod_Clie").ToString().Trim()

        If Not oGeneralData.GetProperty("U_Nom_Clie").Trim = String.Empty Then
            objCotizacion.CardName = oGeneralData.GetProperty("U_Nom_Clie").ToString().Trim()
        End If

        objCotizacion.Comments = oGeneralData.GetProperty("U_Comment").ToString().Trim()
        Integer.TryParse(oGeneralData.GetProperty("U_Series").ToString().Trim(), objCotizacion.Series)

        objCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value = oGeneralData.GetProperty("U_Cod_Uni").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value = oGeneralData.GetProperty("U_Id_Vehi").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strAno_Vehi).Value = oGeneralData.GetProperty("U_Anno").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value = oGeneralData.GetProperty("U_Cod_Mar").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value = oGeneralData.GetProperty("U_Cod_Mod").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strNum_VIN).Value = oGeneralData.GetProperty("U_VIN").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value = oGeneralData.GetProperty("U_Placa").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value = oGeneralData.GetProperty("U_Cod_Est").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strDes_Marc).Value = oGeneralData.GetProperty("U_Des_Mar").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strDes_Mode).Value = oGeneralData.GetProperty("U_Des_Mod").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strDes_Esti).Value = oGeneralData.GetProperty("U_Des_Est").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strFechaRecepcion).Value = oGeneralData.GetProperty("U_Fec_Ape").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strFechaCompromiso).Value = oGeneralData.GetProperty("U_Fec_Com").ToString().Trim()
        objCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = oGeneralData.GetProperty("U_klm").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strCardCodeOrig).Value = oGeneralData.GetProperty("U_CardCodeOrig").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strCardNameOrig).Value = oGeneralData.GetProperty("U_CardNameOrig").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strNoVisita).Value = oGeneralData.GetProperty("U_No_Vis").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value = oGeneralData.GetProperty("U_TipoOrd").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value = oGeneralData.GetProperty("U_OTRefer").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strNoOtRef).Value = oGeneralData.GetProperty("U_OTRefer").ToString().Trim()
        objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = "No iniciada"
        objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = "1"
        strSucursal = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_SCGD_idSucursal FROM OQUT with (nolock) WHERE U_SCGD_Numero_OT = '{0}' ", oGeneralData.GetProperty("U_OTRefer").ToString().Trim()), m_oCompany.CompanyDB, m_oCompany.Server)
        objCotizacion.UserFields.Fields.Item(mc_strIdSucursal).Value = strSucursal
        If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
            If Not String.IsNullOrEmpty(strSucursal) Then
                objCotizacion.BPL_IDAssignedToInvoice = Integer.Parse(strSucursal)
            End If
        End If

        Integer.TryParse(oGeneralData.GetProperty("U_Cod_Ases").ToString().Trim(), objCotizacion.DocumentsOwner)

        If oGeneralData.GetProperty("U_ImpRecp").ToString().Trim() = "Y" Then
            objCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "1"
        ElseIf oGeneralData.GetProperty("U_ImpRecp").ToString().Trim() = "N" Then
            objCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "2"
        End If

        For Each oChildGD As GeneralData In oGeneralDataCollection
            Dim elementoItemCode As String
            Dim elementoDescripcion As String
            Dim elementoCantidad As String
            Dim elementoPorcentajeDesc As String
            Dim elementoMoneda As String
            Dim elementoPrecio As String
            Dim elementoComentarios As String
            Dim elementoImpuesto As String
            Dim elementoIdRXO As String
            Dim elementoCosto As String
            Dim elementoSeleccion As String
            Dim elementoIDLinea As String
            Dim elementoTipArt As String

            elementoItemCode = oChildGD.GetProperty("U_ItemCode").ToString().Trim() ' node.SelectSingleNode("Columns/Column/Value[../ID = 'colItemCo']")
            elementoDescripcion = oChildGD.GetProperty("U_Descrip").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colDesc']")
            elementoCantidad = oChildGD.GetProperty("U_Cant").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colQtn']")
            elementoPorcentajeDesc = oChildGD.GetProperty("U_PorcDs").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colPorDs']")
            elementoMoneda = oChildGD.GetProperty("U_Moned").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colMoned']")
            elementoPrecio = oChildGD.GetProperty("U_Precio").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colPrec']")
            elementoComentarios = oChildGD.GetProperty("U_Coment").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colComen']")
            elementoImpuesto = oChildGD.GetProperty("U_Tax").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colTaxCd']")
            elementoIdRXO = oChildGD.GetProperty("U_IdRxO").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colIdRXO']")
            elementoCosto = oChildGD.GetProperty("U_Costo").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'colCosto']")
            elementoSeleccion = oChildGD.GetProperty("U_Selec").ToString().Trim() 'node.SelectSingleNode("Columns/Column/Value[../ID = 'col_Sel']")
            elementoIDLinea = oChildGD.GetProperty("U_ID_Linea").ToString().Trim()
            elementoTipArt = oChildGD.GetProperty("U_TipArtSO").ToString().Trim()

            If elementoSeleccion = "Y" Then

                If blnAgregarFila Then
                    objCotizacion.Lines.Add()
                Else
                    blnAgregarFila = True
                End If

                Dim Precio As String = CStr(elementoPrecio).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Dim Costo As String = CStr(elementoCosto).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Dim Cantidad As String = CStr(elementoCantidad).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                Dim PorcDescuento As String = CStr(elementoPorcentajeDesc).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)

                Dim decPrecio As Decimal = Decimal.Parse(Precio)
                Dim decCosto As Decimal = Decimal.Parse(Costo)
                Dim decCantidad As Decimal = Decimal.Parse(Cantidad)
                Dim decPorcDescuento As Decimal = Decimal.Parse(PorcDescuento)

                decPrecio = CDec(Utilitarios.CambiarValoresACultureActual(elementoPrecio, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                decCosto = CDec(Utilitarios.CambiarValoresACultureActual(elementoCosto, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                decCantidad = CDec(Utilitarios.CambiarValoresACultureActual(elementoCantidad, strSeparadorMilesSAP, strSeparadorDecimalesSAP))
                decPorcDescuento = CDec(Utilitarios.CambiarValoresACultureActual(elementoPorcentajeDesc, strSeparadorMilesSAP, strSeparadorDecimalesSAP))

                objCotizacion.Lines.ItemCode = elementoItemCode
                'objCotizacion.Lines.Quantity = dblCantidad
                'If drwItem.IsDescuentoNull Then
                'objCotizacion.Lines.DiscountPercent = decPorcDescuento
                'Else
                objCotizacion.Lines.Currency = elementoMoneda
                'objCotizacion.Lines.UnitPrice = Precio

                objCotizacion.Lines.FreeText = elementoComentarios

                objCotizacion.Lines.TaxCode = elementoImpuesto
                objCotizacion.Lines.VatGroup = elementoImpuesto

                objCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = elementoIdRXO

                'objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = elementoCosto

                objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = CDbl(decCosto)
                objCotizacion.Lines.DiscountPercent = CDbl(decPorcDescuento)
                objCotizacion.Lines.Quantity = CDbl(decCantidad)
                objCotizacion.Lines.UnitPrice = CDbl(decPrecio)

                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = oChildGD.GetProperty("U_CPen").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = oChildGD.GetProperty("U_CSol").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = oChildGD.GetProperty("U_CRec").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = oChildGD.GetProperty("U_CPDe").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = oChildGD.GetProperty("U_CPTr").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = oChildGD.GetProperty("U_CPBo").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Compra").Value = oChildGD.GetProperty("U_Compra").ToString().Trim()
                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = elementoTipArt

                For y = 0 To oLineasCotizacionPadre.Count - 1
                    oLineasCotizacionPadre.SetCurrentLine(y)

                    If blnUsaTallerInterno Then
                        If oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value.ToString().Trim() = elementoIDLinea Then
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = elementoIDLinea
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CtrCos").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_FasePro").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = oGeneralData.GetProperty("U_TipoOrd").ToString().Trim()
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Sucur").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Aprobado").Value

                            If elementoTipArt = "2" Then
                                If oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value IsNot Nothing Then

                                    If Not String.IsNullOrEmpty(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim) Then
                                        objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value
                                        objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value
                                    End If
                                End If
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                estadosActividades.Add(Convert.ToInt32(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString()))
                            End If
                            Exit For
                        End If
                    Else
                        If oLineasCotizacionPadre.UserFields.Fields.Item(mc_strIdRepxOrd).Value = elementoIdRXO Then
                            objCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = elementoIdRXO
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CtrCos").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_FasePro").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_FasePro").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = oGeneralData.GetProperty("U_TipoOrd").ToString().Trim()
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CodMarcaVeh").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Sucur").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value
                            objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                            If elementoTipArt = "2" Then
                                If oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value IsNot Nothing Then

                                    If Not String.IsNullOrEmpty(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim) Then
                                        objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EmpAsig").Value
                                        objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value
                                    End If
                                End If
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value
                                objCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                estadosActividades.Add(Convert.ToInt32(oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_EstAct").Value))
                            End If
                            Exit For
                        End If
                    End If

                Next

                'If elementoTipArt = "4" Then

                '    objCotizacionCreada.ListaItemsCodeOTEspeciales.Add(elementoItemCode)
                '    objCotizacionCreada.ListaIdRepxOrdenOTEspeciales.Add(elementoIDLinea)
                '    objCotizacionCreada.ListaNoOrdenOTEspeciales.Add(oGeneralData.GetProperty("U_OTRefer").ToString().Trim())
                '    ' ListaNoOrdenOTEspeciales.Add(m_drwRepuestos.NoOrden)
                '    blnSolOTEsp_ContieneSE = True
                'End If

                blnAgregarFila = True
            End If
        Next
        estado = 1
        Dim todasFin As Boolean = True
        Dim todasSusp As Boolean = True
        For Each itm As Integer In estadosActividades
            If (itm = 2) Then
                estado = 2
                Exit For
            ElseIf (itm = 3) Then
                estado = 3
                todasFin = False
            ElseIf itm = 4 Then
                estado = 4
                todasSusp = False
            End If
        Next
        If estado = 4 AndAlso Not todasFin Then
            estado = 3
        ElseIf estado = 4 AndAlso todasFin Then
            estado = 2
        End If

        Dim dscEstado As String
        ObtieneDescripcionEstado(estado, dscEstado, p_form)
        objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = dscEstado
        objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacionID).Value = estado.ToString()


        blnCreaOT = True
        If blnCreaOT Then
            objCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = 1
        End If

        intResultado = objCotizacion.Add()

        If intResultado <> 0 Then
            m_oCompany.GetLastError(intResultado, strMensajeError)
            Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
            resultado = False
        Else
            intResultado = CInt(m_oCompany.GetNewObjectKey())

            objCotizacionCreada.CargarCotizacionAnterior("", blnCreaOT, intResultado)
            objCotizacionCreada.strIdSucursal = strSucursal
            ActualizarIdLineasHijasPaquetes(intResultado, blnUsaTallerInterno)
            objCotizacionCreada.ManejarCotizacion(blnUsaTallerInterno, intResultado, True, blnCreaOT, oGeneralData.GetProperty("U_CotRef"), Nothing, p_form, blnSolOTEsp_ContieneSE)

            objCotizacionPadre.GetByKey(oGeneralData.GetProperty("U_CotRef"))
            Dim i As Integer
            Dim contador As Integer = 0
            For i = 0 To objCotizacionPadre.Lines.Count - 1
                objCotizacionPadre.Lines.SetCurrentLine(i)
                If objCotizacionPadre.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = "2" Then
                    contador = contador + 1
                End If
            Next
            If contador = objCotizacionPadre.Lines.Count Then
                objCotizacionPadre.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenCancelada
                objCotizacionPadre.Update()
                objCotizacionPadre.Cancel()
            End If

            If Not objCotizacionPadre Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objCotizacionPadre)
                objCotizacionPadre = Nothing
            End If

            ActualizarSolicituOT(oGeneralData.GetProperty("DocEntry"), intResultado)
            SBO_Application.MessageBox(String.Format(My.Resources.Resource.MensajeSolicitudCotizacionCreada, intResultado), 1)
        End If

    End Function


    'Public Sub CrearCotizacion_OT_EspecialesAprobadas(ByVal p_numeroSolicitud As Integer)

    '    Dim objCotizacionCreada As CotizacionCLS
    '    Dim DataTableEncabezado As System.Data.DataTable
    '    Dim DataTableDetalle As System.Data.DataTable
    '    Dim datarowSolicitud As System.Data.DataRow
    '    Dim blnCreaOT As Boolean = False

    '    Dim objCotizacion As SAPbobsCOM.Documents
    '    objCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

    '    Dim strSeparadorDecimalesSAP As String = String.Empty
    '    Dim strSeparadorMilesSAP As String = String.Empty

    '    Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)


    '    DataTableDetalle = Utilitarios.EjecutarConsultaDataTable(" SELECT     [@SCGD_SOT_ESP].U_Cod_Clie, [@SCGD_SOT_ESP].U_Nom_Clie, [@SCGD_SOT_ESP].U_Cod_Ases, [@SCGD_SOT_ESP].U_Num_Coti, " & _
    '                                                            "[@SCGD_SOT_ESP].U_TipoOrd, [@SCGD_SOT_ESP].U_OTRefer, [@SCGD_SOT_ESP].U_Cod_Uni, [@SCGD_SOT_ESP].U_Id_Vehi, [@SCGD_SOT_ESP].U_VIN, " & _
    '                                                            "[@SCGD_SOT_ESP].U_Placa, [@SCGD_SOT_ESP].U_Anno, [@SCGD_SOT_ESP].U_klm, [@SCGD_SOT_ESP].U_Cod_Mar, [@SCGD_SOT_ESP].U_Cod_Mod, " & _
    '                                                            "[@SCGD_SOT_ESP].U_Cod_Est, [@SCGD_SOT_ESP].U_Des_Mar, [@SCGD_SOT_ESP].U_Des_Mod, [@SCGD_SOT_ESP].U_Des_Est, [@SCGD_SOT_ESP].U_Fec_Ape, " & _
    '                                                            "[@SCGD_SOT_ESP].U_Fec_Com, [@SCGD_SOT_ESP].U_No_Vis, [@SCGD_SOT_ESP].U_CardCodeOrig, [@SCGD_SOT_ESP].U_CardNameOrig, " & _
    '                                                            "[@SCGD_SOT_ESP].U_OTPadre, [@SCGD_SOT_ESP].U_Estad_OT, [@SCGD_SOT_ESP].U_Series, [@SCGD_SOT_ESP].U_Comment, [@SCGD_SOT_ESP].U_CotCread, " & _
    '                                                            "[@SCGD_SOT_ESP].U_Status, [@SCGD_SOT_ESP].U_CotRef, [@SCGD_SOT_ESP].U_NomTipOT, [@SCGD_SOT_ESP].U_NomAse, [@SCGD_SOT_ESP].U_ImpRecp, " & _
    '                                                            "[@SCGD_LINEAS_SOT_ESP].U_ItemCode, [@SCGD_LINEAS_SOT_ESP].U_Descrip, [@SCGD_LINEAS_SOT_ESP].U_PorcDs, [@SCGD_LINEAS_SOT_ESP].U_Moned, " & _
    '                                                            "[@SCGD_LINEAS_SOT_ESP].U_Precio, [@SCGD_LINEAS_SOT_ESP].U_Coment, [@SCGD_LINEAS_SOT_ESP].U_IdRxO, [@SCGD_LINEAS_SOT_ESP].U_Costo, " & _
    '                                                            "[@SCGD_LINEAS_SOT_ESP].U_Cant, [@SCGD_LINEAS_SOT_ESP].U_Tax " & _
    '                                                            "FROM         [@SCGD_SOT_ESP] INNER JOIN " & _
    '                                                            "[@SCGD_LINEAS_SOT_ESP] ON [@SCGD_SOT_ESP].DocEntry = [@SCGD_LINEAS_SOT_ESP].DocEntry " & _
    '                                                            "WHERE     ([@SCGD_SOT_ESP].DocEntry = = " & p_numeroSolicitud & "'", m_oCompany.CompanyDB, m_oCompany.Server)


    '    datarowSolicitud = DataTableDetalle.Rows(0)

    '    m_IdSucursal = Utilitarios.ObtieneIdSucursal(SBO_Application).ToString

    '    Dim blnAgregarFila As Boolean = False
    '    Dim intResultado As Integer

    '    'If Not ValidarEstadoCotizacion(datarowSolicitud.Item("U_CotRef").Trim) Then

    '    '    Dim CotizacionPadre As String = datarowSolicitud.Item("U_CotRef").Trim

    '    '    SBO_Application.SetStatusBarMessage(String.Format(My.Resources.Resource.CotizacionCerradaCancelada, CotizacionPadre), BoMessageTime.bmt_Short, True)
    '    '    Exit Sub
    '    'End If

    '    objCotizacion.CardCode = datarowSolicitud.Item("U_Cod_Clie").Trim

    '    If Not datarowSolicitud.Item("U_Nom_Clie").Trim = String.Empty Then

    '        objCotizacion.CardName = datarowSolicitud.Item("U_Nom_Clie").Trim
    '    End If


    '    'If strComentarios.Length <= 254 Then
    '    objCotizacion.Comments = datarowSolicitud.Item("U_OTRefer").Trim & " " & _
    '        datarowSolicitud.Item("U_Comment").Trim
    '    'End If

    '    objCotizacion.Series = datarowSolicitud.Item("U_Series").Trim

    '    objCotizacion.UserFields.Fields.Item(mc_strNumUnidad).Value = datarowSolicitud.Item("U_Cod_Uni").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strNumVehiculo).Value = datarowSolicitud.Item("U_Id_Vehi").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strAno_Vehi).Value = datarowSolicitud.Item("U_Anno").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strCod_Marca).Value = datarowSolicitud.Item("U_Cod_Mar").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strCod_Modelo).Value = datarowSolicitud.Item("U_Cod_Mod").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strNum_VIN).Value = datarowSolicitud.Item("U_VIN").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strNum_Placa).Value = datarowSolicitud.Item("U_Placa").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strCod_Estilo).Value = datarowSolicitud.Item("U_Cod_Est").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strDes_Marc).Value = datarowSolicitud.Item("U_Des_Mar").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strDes_Mode).Value = datarowSolicitud.Item("U_Des_Mod").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strDes_Esti).Value = datarowSolicitud.Item("U_Des_Est").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strFechaRecepcion).Value = datarowSolicitud.Item("U_Fec_Ape").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strFechaCompromiso).Value = datarowSolicitud.Item("U_Fec_Com").Trim
    '    objCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = datarowSolicitud.Item("U_klm").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strCardCodeOrig).Value = datarowSolicitud.Item("U_CardCodeOrig").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strCardNameOrig).Value = datarowSolicitud.Item("U_CardNameOrig").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strNoVisita).Value = datarowSolicitud.Item("U_No_Vis").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strTipoOT).Value = datarowSolicitud.Item("U_TipoOrd").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strOTPadre).Value = datarowSolicitud.Item("U_OTRefer").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strNoOtRef).Value = datarowSolicitud.Item("U_OTRefer").Trim
    '    objCotizacion.UserFields.Fields.Item(mc_strEstadoCotizacion).Value = "No iniciada"
    '    objCotizacion.DocumentsOwner = datarowSolicitud.Item("U_Cod_Ases").Trim
    '    objCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = "2"


    '    For Each drw As System.Data.DataRow In DataTableDetalle.Rows

    '        If blnAgregarFila Then

    '            objCotizacion.Lines.Add()

    '        Else

    '            blnAgregarFila = True

    '        End If

    '        Dim Precio As String = CStr(drw.Item("U_Precio")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
    '        Dim Costo As String = CStr(drw.Item("U_Costo")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
    '        Dim Cantidad As String = CStr(drw.Item("U_Cant")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
    '        Dim PorcDescuento As String = CStr(drw.Item("U_PorcDs")).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)

    '        Dim decPrecio As Decimal = Decimal.Parse(Precio)
    '        Dim decCosto As Decimal = Decimal.Parse(Costo)
    '        Dim dblCantidad As Decimal = Decimal.Parse(Cantidad)
    '        Dim decPorcDescuento As Decimal = Decimal.Parse(PorcDescuento)

    '        decPrecio = CDec(Utilitarios.CambiarValoresACultureActual(drw.Item("U_Precio"), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
    '        decCosto = CDec(Utilitarios.CambiarValoresACultureActual(drw.Item("U_Costo"), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
    '        dblCantidad = CDec(Utilitarios.CambiarValoresACultureActual(drw.Item("U_Cant"), strSeparadorMilesSAP, strSeparadorDecimalesSAP))
    '        decPorcDescuento = CDec(Utilitarios.CambiarValoresACultureActual(drw.Item("U_PorcDs"), strSeparadorMilesSAP, strSeparadorDecimalesSAP))

    '        objCotizacion.Lines.ItemCode = drw.Item("U_ItemCode")
    '        objCotizacion.Lines.Quantity = dblCantidad
    '        'If drwItem.IsDescuentoNull Then
    '        objCotizacion.Lines.DiscountPercent = decPorcDescuento
    '        'Else
    '        objCotizacion.Lines.Currency = drw.Item("U_Moned")
    '        objCotizacion.Lines.UnitPrice = Precio

    '        objCotizacion.Lines.FreeText = drw.Item("U_Coment")

    '        objCotizacion.Lines.TaxCode = drw.Item("U_Tax")

    '        objCotizacion.Lines.UserFields.Fields.Item(mc_strIdRepxOrd).Value = drw.Item("U_IdRxO")

    '        objCotizacion.Lines.UserFields.Fields.Item(mc_strCosto).Value = Costo

    '        blnAgregarFila = True

    '    Next

    '    blnCreaOT = True
    '    If blnCreaOT Then
    '        objCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = 1
    '    End If

    '    intResultado = objCotizacion.Add()

    '    If intResultado <> 0 Then
    '        m_oCompany.GetLastError(intResultado, strMensajeError)
    '        Throw New SCGCommon.ExceptionsSBO(intResultado, strMensajeError)
    '    Else

    '        intResultado = CInt(m_oCompany.GetNewObjectKey())

    '        objCotizacionCreada = New CotizacionCLS(SBO_Application, m_oCompany)

    '        objCotizacionCreada.CargarCotizacionAnterior("", blnCreaOT, intResultado)

    '        objCotizacionCreada.ManejarCotizacion(intResultado, True, blnCreaOT)

    '        ActualizarIdLineasHijasPaquetes(intResultado)

    '        ActualizarSolicituOT(p_numeroSolicitud, intResultado)
    '        SBO_Application.MessageBox(String.Format(My.Resources.Resource.MensajeSolicitudCotizacionCreada, intResultado), 1)

    '    End If

    'End Sub

    Public Sub ActualizarSolicituOT(ByVal p_NumSolicitud As Integer, ByVal p_NumCotizacion As Integer)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oGeneralData As SAPbobsCOM.GeneralData

        Try

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_SOTESP")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_NumSolicitud)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oGeneralData.SetProperty("U_Num_Coti", p_NumCotizacion)
            oGeneralData.SetProperty("U_CotCread", "Y")

            oGeneralService.Update(oGeneralData)
        Catch ex As SCGCommon.ExceptionsSBO
            SBO_Application.SetStatusBarMessage(ex.Message)
        Catch ex As Exception
            SBO_Application.SetStatusBarMessage(ex.Message)
        End Try


    End Sub

    Public Function ValidarEstadoCotizacion(ByVal p_intNumeroCotizacion As Integer) As Boolean

        Dim m_oCotizacionPadre As SAPbobsCOM.Documents
        m_oCotizacionPadre = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
        If m_oCotizacionPadre.GetByKey(p_intNumeroCotizacion) Then
            If m_oCotizacionPadre.DocumentStatus = BoStatus.bost_Close Then
                Return False
            ElseIf m_oCotizacionPadre.Cancelled Then
                Return False
            ElseIf m_oCotizacionPadre.DocumentStatus = BoStatus.bost_Open Then
                Return True
            End If
        End If
    End Function

    'Public Sub ActualizarIdLineasHijasPaquetes(ByVal p_intNumeroCotizacion As Integer, Optional p_blnUsaTallerInterno As Boolean = False)

    '    Dim m_oCotizacionEspecial As SAPbobsCOM.Documents
    '    Dim m_oLineasCotizacionEspecial As SAPbobsCOM.Document_Lines

    '    Dim m_oCotizacionPadre As SAPbobsCOM.Documents
    '    Dim m_oLineasCotizacionPadre As SAPbobsCOM.Document_Lines
    '    Dim KitPertenece As String = String.Empty
    '    Dim intSeguirBusquedalinea As Integer = 0

    '    Dim ListaId As Generic.IList(Of String) = New Generic.List(Of String)

    '    m_oCotizacionEspecial = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
    '    m_oCotizacionPadre = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

    '    If m_oCotizacionEspecial.GetByKey(p_intNumeroCotizacion) Then

    '        Dim OT_Padre As String = m_oCotizacionEspecial.UserFields.Fields.Item("U_SCGD_OT_Padre").Value

    '        Dim DocEntryPadre As String = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT with (nolock) where U_SCGD_Numero_OT = '" & OT_Padre & "'", m_oCompany.CompanyDB, m_oCompany.Server)

    '        m_oCotizacionPadre.GetByKey(DocEntryPadre)

    '        m_oLineasCotizacionPadre = m_oCotizacionPadre.Lines

    '        m_oLineasCotizacionEspecial = m_oCotizacionEspecial.Lines

    '        For i As Integer = 0 To m_oLineasCotizacionEspecial.Count - 1

    '            m_oLineasCotizacionEspecial.SetCurrentLine(i)

    '            Dim itemcodeOE As String = m_oLineasCotizacionEspecial.ItemCode

    '            Dim s As String = m_oLineasCotizacionEspecial.TreeType

    '            For j As Integer = intSeguirBusquedalinea To m_oLineasCotizacionPadre.Count - 1

    '                m_oLineasCotizacionPadre.SetCurrentLine(j)

    '                Dim itemcodePadre As String = m_oLineasCotizacionPadre.ItemCode

    '                Dim s_Padre As String = m_oLineasCotizacionEspecial.TreeType

    '                If itemcodeOE = itemcodePadre Then

    '                    If m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
    '                        Dim idlinearepuestoPadre As String = String.Empty
    '                        Dim idlinearepuestoHijo As String = String.Empty

    '                        If p_blnUsaTallerInterno Then
    '                            idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
    '                            idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value
    '                        Else
    '                            idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                            idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value

    '                        End If

    '                        If Not idlinearepuestoPadre = String.Empty Then
    '                            If Not ListaId.Contains(idlinearepuestoPadre) Then

    '                                If p_blnUsaTallerInterno Then
    '                                    If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = String.Empty Then

    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.Quantity

    '                                        ListaId.Add(idlinearepuestoPadre)
    '                                        intSeguirBusquedalinea = j
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
    '                                        Exit For

    '                                    End If
    '                                    'actualizo la linea padre con Aprobado = No
    '                                    m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
    '                                    m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
    '                                    m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0

    '                                Else
    '                                    If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then

    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                                        'm_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CRec").Value

    '                                        ListaId.Add(idlinearepuestoPadre)
    '                                        intSeguirBusquedalinea = j

    '                                        Exit For

    '                                    End If

    '                                End If


    '                            End If
    '                        Else
    '                            intSeguirBusquedalinea = j

    '                            Exit For
    '                        End If

    '                    ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then

    '                        Dim idlinearepuestoPadre As String = String.Empty
    '                        Dim idlinearepuestoHijo As String = String.Empty

    '                        If p_blnUsaTallerInterno Then
    '                            idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
    '                            idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value
    '                        Else
    '                            idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                            idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                        End If


    '                        If Not idlinearepuestoPadre = String.Empty Then
    '                            If Not ListaId.Contains(idlinearepuestoPadre) Then

    '                                If p_blnUsaTallerInterno Then

    '                                    If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = String.Empty Then

    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.Quantity
    '                                        ListaId.Add(idlinearepuestoPadre)
    '                                        intSeguirBusquedalinea = j

    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
    '                                        Exit For

    '                                    End If

    '                                Else
    '                                    If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then

    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                                        'm_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CRec").Value

    '                                        ListaId.Add(idlinearepuestoPadre)
    '                                        intSeguirBusquedalinea = j
    '                                        Exit For

    '                                    End If
    '                                End If

    '                            End If
    '                        Else
    '                            intSeguirBusquedalinea = j

    '                            Exit For
    '                        End If
    '                        'End If

    '                    ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree Then

    '                        Dim idlinearepuestoPadre As String = String.Empty
    '                        Dim idlinearepuestoHijo As String = String.Empty

    '                        If p_blnUsaTallerInterno Then

    '                            idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
    '                            idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value
    '                        Else
    '                            idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                            idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                        End If

    '                        If Not idlinearepuestoPadre = String.Empty Then
    '                            If Not ListaId.Contains(idlinearepuestoPadre) Then

    '                                If p_blnUsaTallerInterno Then
    '                                    If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = String.Empty Then

    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.Quantity

    '                                        ListaId.Add(idlinearepuestoPadre)
    '                                        intSeguirBusquedalinea = j
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
    '                                        m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
    '                                        Exit For

    '                                    End If

    '                                Else
    '                                    If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then

    '                                        m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
    '                                        'm_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CRec").Value

    '                                        ListaId.Add(idlinearepuestoPadre)
    '                                        intSeguirBusquedalinea = j

    '                                        Exit For

    '                                    End If

    '                                End If

    '                            End If
    '                        Else
    '                            intSeguirBusquedalinea = j
    '                            Exit For
    '                        End If
    '                    End If

    '                End If
    '            Next
    '            'End If
    '        Next

    '    End If
    '    'actualizo la cotizacion que se creo como especial
    '    ListaId.Clear()
    '    m_oCotizacionPadre.Update()
    '    m_oCotizacionEspecial.Update()

    '    If Not m_oCotizacionPadre Is Nothing Then
    '        'Destruyo el Objeto - Error HRESULT  
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oCotizacionPadre)
    '        m_oCotizacionPadre = Nothing
    '    End If

    '    If Not m_oCotizacionEspecial Is Nothing Then
    '        'Destruyo el Objeto - Error HRESULT  
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oCotizacionEspecial)
    '        m_oCotizacionEspecial = Nothing
    '    End If

    'End Sub

    Public Sub ActualizarIdLineasHijasPaquetes(ByVal p_intNumeroCotizacion As Integer, Optional p_blnUsaTallerInterno As Boolean = False)

        Dim m_oCotizacionEspecial As SAPbobsCOM.Documents
        Dim m_oLineasCotizacionEspecial As SAPbobsCOM.Document_Lines

        Dim m_oCotizacionPadre As SAPbobsCOM.Documents
        Dim m_oLineasCotizacionPadre As SAPbobsCOM.Document_Lines
        Dim KitPertenece As String = String.Empty
        Dim intSeguirBusquedalinea As Integer = 0

        Dim ListaId As Generic.IList(Of String) = New Generic.List(Of String)

        Try
            m_oCotizacionEspecial = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            m_oCotizacionPadre = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If m_oCotizacionEspecial.GetByKey(p_intNumeroCotizacion) Then

                Dim OT_Padre As String = m_oCotizacionEspecial.UserFields.Fields.Item("U_SCGD_OT_Padre").Value

                Dim DocEntryPadre As String = Utilitarios.EjecutarConsulta("Select DocEntry from OQUT with (nolock) where U_SCGD_Numero_OT = '" & OT_Padre & "'", m_oCompany.CompanyDB, m_oCompany.Server)

                m_oCotizacionPadre.GetByKey(DocEntryPadre)

                m_oLineasCotizacionPadre = m_oCotizacionPadre.Lines

                m_oLineasCotizacionEspecial = m_oCotizacionEspecial.Lines

                For i As Integer = 0 To m_oLineasCotizacionEspecial.Count - 1
                    m_oLineasCotizacionEspecial.SetCurrentLine(i)
                    Dim itemcodeOE As String = m_oLineasCotizacionEspecial.ItemCode
                    Dim s As String = m_oLineasCotizacionEspecial.TreeType
                    For j As Integer = intSeguirBusquedalinea To m_oLineasCotizacionPadre.Count - 1
                        m_oLineasCotizacionPadre.SetCurrentLine(j)
                        Dim cantlin = m_oLineasCotizacionEspecial.Count

                        Dim itemcodePadre As String = m_oLineasCotizacionPadre.ItemCode
                        Dim s_Padre As String = m_oLineasCotizacionEspecial.TreeType
                        If itemcodeOE = itemcodePadre Then
                            If m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                Dim idlinearepuestoPadre As String = String.Empty
                                Dim idlinearepuestoHijo As String = String.Empty
                                If p_blnUsaTallerInterno Then
                                    idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
                                    idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value
                                Else
                                    idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                    idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not idlinearepuestoPadre = String.Empty Then
                                    If Not ListaId.Contains(idlinearepuestoPadre) Then
                                        If p_blnUsaTallerInterno Then
                                            If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = String.Empty Then
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.Quantity
                                                ListaId.Add(idlinearepuestoPadre)
                                                intSeguirBusquedalinea = j
                                                m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                                m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
                                                m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                                Exit For
                                            End If
                                            'actualizo la linea padre con Aprobado = No
                                            m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                            m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
                                            m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                        Else
                                            If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                                'm_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_CRec").Value
                                                ListaId.Add(idlinearepuestoPadre)
                                                intSeguirBusquedalinea = j
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Else
                                    intSeguirBusquedalinea = j
                                    Exit For
                                End If
                            ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                                Dim idlinearepuestoPadre As String = String.Empty
                                Dim idlinearepuestoHijo As String = String.Empty
                                If p_blnUsaTallerInterno Then
                                    idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
                                    idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value
                                Else
                                    idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                    idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not idlinearepuestoPadre = String.Empty Then
                                    If Not ListaId.Contains(idlinearepuestoPadre) Then
                                        If p_blnUsaTallerInterno Then
                                            'If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = String.Empty Then
                                            '    m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
                                            '    m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.Quantity
                                            '    ListaId.Add(idlinearepuestoPadre)
                                            '    intSeguirBusquedalinea = j
                                            '    m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                            '    m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
                                            '    m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                            '    Exit For
                                            'End If
                                        Else
                                            If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                                ListaId.Add(idlinearepuestoPadre)
                                                intSeguirBusquedalinea = j
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Else
                                    intSeguirBusquedalinea = j
                                    Exit For
                                End If
                            ElseIf m_oLineasCotizacionPadre.TreeType = SAPbobsCOM.BoItemTreeTypes.iNotATree Then
                                Dim idlinearepuestoPadre As String = String.Empty
                                Dim idlinearepuestoHijo As String = String.Empty
                                If p_blnUsaTallerInterno Then
                                    idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
                                    idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value
                                Else
                                    idlinearepuestoPadre = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                    idlinearepuestoHijo = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not idlinearepuestoPadre = String.Empty Then
                                    If Not ListaId.Contains(idlinearepuestoPadre) Then
                                        If p_blnUsaTallerInterno Then
                                            Dim localID = m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value.ToString
                                            Dim cotEsp = m_oLineasCotizacionEspecial.ItemCode.ToString
                                            Dim cotP = m_oLineasCotizacionPadre.ItemCode.ToString
                                            If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And Not String.IsNullOrEmpty(localID) Then
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_ID").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_ID").Value
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_CRec").Value = m_oLineasCotizacionPadre.Quantity
                                                ListaId.Add(idlinearepuestoPadre)
                                                intSeguirBusquedalinea = j
                                                m_oLineasCotizacionPadre.UserFields.Fields.Item(mc_strItemAprobado).Value = 2
                                                m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_OTHija").Value = 1
                                                m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_Traslad").Value = 0
                                                Exit For
                                            End If
                                        Else
                                            If m_oLineasCotizacionEspecial.ItemCode = m_oLineasCotizacionPadre.ItemCode And m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = 0 Then
                                                m_oLineasCotizacionEspecial.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = m_oLineasCotizacionPadre.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                                ListaId.Add(idlinearepuestoPadre)
                                                intSeguirBusquedalinea = j
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Else
                                    intSeguirBusquedalinea = j
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                Next
            End If
            'actualizo la cotizacion que se creo como especial
            ListaId.Clear()
            m_oCotizacionPadre.Update()
            m_oCotizacionEspecial.Update()

            If Not m_oCotizacionPadre Is Nothing Then
                'Destruyo el Objeto - Error HRESULT  
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oCotizacionPadre)
                m_oCotizacionPadre = Nothing
            End If

            If Not m_oCotizacionEspecial Is Nothing Then
                'Destruyo el Objeto - Error HRESULT  
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_oCotizacionEspecial)
                m_oCotizacionEspecial = Nothing
            End If
        Catch ex As SCGCommon.ExceptionsSBO
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try


    End Sub

    Public Sub SeleccionarTodasFilas(ByRef p_matriz As SAPbouiCOM.Matrix, ByVal p_sel As String)

        Dim oItem As SAPbouiCOM.Item

        FormularioSBO.Freeze(True)

        For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_SOT_ESP").Size - 1


            FormularioSBO.DataSources.DBDataSources.Item("@SCGD_LINEAS_SOT_ESP").SetValue("U_Selec", i, p_sel)

        Next
        p_matriz.LoadFromDataSource()

        FormularioSBO.Freeze(False)

    End Sub

End Class
