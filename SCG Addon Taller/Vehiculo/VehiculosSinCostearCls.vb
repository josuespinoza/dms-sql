Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework.UI

Public Class VehiculosSinCostearCls

#Region "Declaraciones"

    Private Const mc_strUIDVehículos As String = "SCGD_MNO"
    Private Const mc_strUIDrVehiculosCostear As String = "SCGD_COS"
    Private Const mc_strUIDGood_Receive As String = "SCGD_ENT"
    Private Const mc_strUIDListadoGR As String = "SCGD_ESP"
    Private Const mc_strUIDGood_Issue As String = "SCGD_SAL"
    Private Const mc_strSCG_VEHICULO As String = "@SCGD_VEHICULO"
    Private Const mc_strEstadoInventario As String = "U_TIPINV"
    Private Const mc_strUnidad As String = "U_Cod_Unid"
    Private Const mc_strUDFRecVeh As String = "U_DocRecepcion"
    Private Const mc_strUDFPedidoVeh As String = "U_DocPedido"
    
    'Matriz
    Private Const mc_strMTZCotizacion As String = "mtVehiculo"

    'Nombres de columnas de matrix
    Private Const mc_strUIDIDContrato As String = "cl_Cont"
    Private Const mc_strUIDUnid As String = "cl_Unid"
    Private Const mc_strUIDMarca As String = "cl_Marc"
    Private Const mc_strUIDEstilo As String = "cl_Esti"
    Private Const mc_strUIDVIN As String = "cl_Vin"

    'Nombres de los botones
    Private Const mc_strUIDActualizar As String = "btnActuali"
    Private Const mc_strUIDCerrar As String = "btnCerrar"
    
    'Nombre de los CheckBox


    'Nombre de los Text Fields
    Private Const mc_strUIDTextUnidad As String = "txtUnidad"
    Private Const mc_strUIDFhaDesde As String = "txtFecIni"
    Private Const mc_strUIDFhaHasta As String = "txtFecFin"

    Private Const mc_strUIDTextRecVeh As String = "txtRecVeh"
    Private Const mc_strUIDTextCodPedido As String = "txtCodPed"

    Private Const mc_strUIDCboTipo As String = "cboTipo"
    Private Const mc_strUIDCbxFactura As String = "chkFac"
    'Nombres de campos del datasource
    Private Const mc_strIDContrato As String = "U_CTOVTA"
    Private Const mc_strMarca As String = "U_Des_Marc"
    Private Const mc_strEstilo As String = "U_Des_Esti"
    Private Const mc_strModelo As String = "U_Des_Mode"
    Private Const mc_strVIN As String = "U_Num_VIN"
    Private Const mc_strSinCostear As String = "S"
    Private m_dbContratos As DBDataSource
    Private m_oFormGenCotizacion As Form

    Private WithEvents SBO_Application As Application
    Dim m_cn_Coneccion As New SqlClient.SqlConnection
    Dim m_strConectionString As String
    Dim objConfiguracionGeneral As ConfiguracionesGeneralesAddon

    'Private m_UDSLocal As SAPbouiCOM.UserDataSource
    Private editChxFact As CheckBoxSBO
    Private TxtFhaDesde As EditTextSBO
    Private TxtFhaHasta As EditTextSBO
    Private TxtRecepcion As EditTextSBO
    Private TxtPedido As EditTextSBO
    Private cboTipoVeh As ComboBoxSBO
    Dim dtLocal As DataTable
    
    Private m_blnCheckFactura As Boolean = False
    Private m_blnFiltraTipoVeh As Boolean = False

    Public mo_Form As Form

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As Application)

        SBO_Application = p_SBO_Aplication

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String

        'Menu Tramite
        If Utilitarios.MostrarMenu(mc_strUIDrVehiculosCostear, SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu(mc_strUIDrVehiculosCostear, SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDrVehiculosCostear, BoMenuType.mt_STRING, strEtiquetaMenu, 25, False, True, mc_strUIDVehículos))
        End If

        If Utilitarios.MostrarMenu("SCGD_REC", SBO_Application.Company.UserName) Then
            'Menu Gerente Ventas
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_REC", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_REC", BoMenuType.mt_STRING, strEtiquetaMenu, 30, False, True, mc_strUIDVehículos))

        End If

        If Utilitarios.MostrarMenu("SCGD_SAL", SBO_Application.Company.UserName) Then
            'Menu Gerente Ventas
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_SAL", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDGood_Issue, BoMenuType.mt_STRING, strEtiquetaMenu, 35, False, True, mc_strUIDVehículos))
        End If

        If Utilitarios.MostrarMenu("SCGD_ENT", SBO_Application.Company.UserName) Then
            'Menu Gerente General
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_ENT", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDGood_Receive, BoMenuType.mt_STRING, strEtiquetaMenu, 40, False, True, mc_strUIDVehículos))
        End If

        If Utilitarios.MostrarMenu("SCGD_ESP", SBO_Application.Company.UserName) Then
            'Menu Gerente General
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_ESP", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDListadoGR, BoMenuType.mt_STRING, strEtiquetaMenu, 45, False, True, mc_strUIDVehículos))
        End If

    End Sub

    Protected Friend Sub CargaFormularioVehiculosSinCostear()
        '*******************************************************************    
        'Propósito: Se encarga de establecer los filtros para los eventos de la
        '            aplicacion que se van a manejar y posteriormente se los
        '            agrega al objeto aplicacion donde se esta almacenando la
        '            aplicacion SBO que esta corriendo
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_frmVeh_Cos"

            strXMLACargar = My.Resources.Resource.VehiculosACostear
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Dim strConexionDBSucursal As String = ""

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strSCG_VEHICULO)

            objConfiguracionGeneral = Nothing
            Configuracion.CrearCadenaDeconexion(SBO_Application.Company.ServerName, SBO_Application.Company.DatabaseName, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString
            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO)

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto

            If EnlazaColumnasMatrixaDatasource(oMatrix) Then

                Call CargarMatrix(oMatrix, _
                                  m_oFormGenCotizacion, m_dbContratos)

                m_oFormGenCotizacion.Visible = True

            End If

         

            Dim userDS As UserDataSources = m_oFormGenCotizacion.DataSources.UserDataSources
            userDS.Add("fac", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("des", BoDataType.dt_DATE, 100)
            userDS.Add("has", BoDataType.dt_DATE, 100)
            userDS.Add("tip", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("rec", BoDataType.dt_SHORT_TEXT)
            userDS.Add("ped", BoDataType.dt_SHORT_TEXT)

            TxtFhaDesde = New EditTextSBO(mc_strUIDFhaDesde, True, "", "des", m_oFormGenCotizacion)
            TxtFhaHasta = New EditTextSBO(mc_strUIDFhaHasta, True, "", "has", m_oFormGenCotizacion)
            TxtRecepcion = New EditTextSBO(mc_strUIDTextRecVeh, True, "", "rec", m_oFormGenCotizacion)
            TxtPedido = New EditTextSBO(mc_strUIDTextCodPedido, True, "", "ped", m_oFormGenCotizacion)

            editChxFact = New CheckBoxSBO(mc_strUIDCbxFactura, True, "", "fac", m_oFormGenCotizacion)
            cboTipoVeh = New ComboBoxSBO(mc_strUIDCboTipo, m_oFormGenCotizacion, True, "", "tip")

            editChxFact.AsignaBinding()
            TxtFhaDesde.AsignaBinding()
            TxtFhaHasta.AsignaBinding()
            cboTipoVeh.AsignaBinding()
            TxtRecepcion.AsignaBinding()
            TxtPedido.AsignaBinding()

            AddChooseFromList(m_oFormGenCotizacion, "SCGD_EDV", "CFL_Rec")
            AgregaCFLRecepcion(TxtRecepcion.UniqueId, "CFL_Rec", "DocEntry")


            'oItem = m_oFormGenCotizacion.Items.Item(TxtRecepcion.UniqueId)
            'oEdit = oItem.Specific

            'oEdit.ChooseFromListUID = "CFL_Rec"
            'oEdit.ChooseFromListAlias = "DocEntry"

            AddChooseFromList(m_oFormGenCotizacion, "SCGD_PDV", "CFL_Ped")
            AgregaCFLRecepcion(TxtPedido.UniqueId, "CFL_Ped", "DocEntry")

            dtLocal = m_oFormGenCotizacion.DataSources.DataTables.Add("dtLocal")

            CargarCombos()
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        '*******************************************************************    
        'Propósito:  Se encarga de cargar las formas desde el archivo XML,
        '             tomando como parámetro el nombre del archivo.
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    Private Function CargarCombos()
        Try
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            dtLocal = m_oFormGenCotizacion.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            oItem = m_oFormGenCotizacion.Items.Item(mc_strUIDCboTipo)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            dtLocal.ExecuteQuery(" SELECT Code, Name FROM [@SCGD_TIPOVEHICULO] with(nolock) Order by Name ")

            For i As Integer = 0 To dtLocal.Rows.Count - 1
                oCombo.ValidValues.Add(dtLocal.GetValue("Code", i), dtLocal.GetValue("Name", i))
            Next
            oCombo.ValidValues.Add("-", My.Resources.Resource.Todos)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oItem As SAPbouiCOM.Item
            Dim oChk As SAPbouiCOM.CheckBox
            Dim l_Strval As Boolean


            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing AndAlso pVal.ActionSuccess Then

                Select Case pVal.ItemUID
                    Case mc_strUIDActualizar
                        oMatrix = DirectCast(oForm.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)

                        If Not oMatrix Is Nothing Then

                            Call CargarMatrix(DirectCast(oForm.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix), _
                                              oForm, _
                                              m_dbContratos)

                        End If

                    Case mc_strUIDCbxFactura
                        If editChxFact.ObtieneValorUserDataSource = "Y" Then
                            oForm.Items.Item(mc_strUIDFhaDesde).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                            oForm.Items.Item(mc_strUIDFhaHasta).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                            m_blnCheckFactura = True
                        Else
                            oForm.Items.Item(mc_strUIDFhaDesde).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                            oForm.Items.Item(mc_strUIDFhaHasta).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                            TxtFhaDesde.AsignaValorUserDataSource("")
                            TxtFhaHasta.AsignaValorUserDataSource("")
                            m_blnCheckFactura = False
                        End If


                    Case mc_strUIDCerrar
                        Call oForm.Close()

                End Select
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ManejadroEventoCombo(ByVal FormuUID As String,
                                    ByRef pVal As SAPbouiCOM.ItemEvent,
                                    ByRef BubbleEvent As Boolean)
        Try

            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case cboTipoVeh.UniqueId
                        If String.IsNullOrEmpty(cboTipoVeh.ObtieneValorUserDataSource) OrElse
                            cboTipoVeh.ObtieneValorUserDataSource = "-" Then
                            m_blnFiltraTipoVeh = False
                        Else
                            m_blnFiltraTipoVeh = True
                        End If
                End Select


            ElseIf pVal.BeforeAction Then
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


    Public Sub ManejadorEventosChooseFromList(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim strCFL_Id As String

            Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)

            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            strCFL_Id = oCFLEvent.ChooseFromListUID
            oCFL = m_oFormGenCotizacion.ChooseFromLists.Item(strCFL_Id)
            Dim l_decTipoCambio As Decimal

            If oCFLEvent.ActionSuccess Then

                Dim oDataTable As SAPbouiCOM.DataTable
                oDataTable = oCFLEvent.SelectedObjects

                If Not oCFLEvent.SelectedObjects Is Nothing Then
                    If Not oDataTable Is Nothing And m_oFormGenCotizacion.Mode <> BoFormMode.fm_FIND_MODE Then
                        Select Case pVal.ItemUID
                            Case TxtRecepcion.UniqueId
                                TxtRecepcion.AsignaValorUserDataSource(oDataTable.GetValue("DocEntry", 0).ToString)
                            Case TxtPedido.UniqueId
                                TxtPedido.AsignaValorUserDataSource(oDataTable.GetValue("DocEntry", 0).ToString)
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Function CargarMatrix(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                 ByVal oform As SAPbouiCOM.Form, _
                                 ByVal dbCotizacion As SAPbouiCOM.DBDataSource) As Boolean

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim strTipoParaTaller As String
        Dim strNoDisponible As String
        Dim strUnidad As String
        Dim strCodRecepcion As String
        Dim strCodTipo As String
        Dim strCodPedido As String
        Dim strConsultaExacta As String = String.Empty

        Try

            strConsultaExacta = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Busq_exac FROM dbo.[@SCGD_ADMIN] with(nolock)"), SBO_Application.Company.DatabaseName, SBO_Application.Company.ServerName).Trim

            If String.IsNullOrEmpty(strConsultaExacta) Then
                strConsultaExacta = "N"
            End If

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            strUnidad = oform.Items.Item(mc_strUIDTextUnidad).Specific.String
            strCodRecepcion = oform.Items.Item(mc_strUIDTextRecVeh).Specific.String
            strCodPedido = oform.Items.Item(mc_strUIDTextCodPedido).Specific.String

            strNoDisponible = objConfiguracionGeneral.DisponibilidadVehiculoVendido
            strTipoParaTaller = objConfiguracionGeneral.InventarioVehiculoVendido

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strUnidad
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
            oCondition.BracketCloseNum = 1

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strUnidad
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
            oCondition.CondVal = ""
            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
            oCondition.BracketCloseNum = 1

            oCondition = oConditions.Add

            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strEstadoInventario
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = mc_strSinCostear
            If Not String.IsNullOrEmpty(strUnidad) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
            End If
            oCondition.BracketCloseNum = 1

            If Not String.IsNullOrEmpty(strUnidad) Then
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strUnidad

                If strConsultaExacta = "Y" Then
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                Else
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_CONTAIN
                End If

                oCondition.CondVal = strUnidad
                oCondition.BracketCloseNum = 1
            End If

            If Not String.IsNullOrEmpty(strCodRecepcion) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strUDFRecVeh
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strCodRecepcion
                oCondition.BracketCloseNum = 1
            End If
            If Not String.IsNullOrEmpty(strCodPedido) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strUDFPedidoVeh
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strCodPedido
                oCondition.BracketCloseNum = 1
            End If

            Dim strFechaInicioFormateada As String = String.Empty
            Dim strFechaFinFormateada As String = String.Empty

            Dim strFechaInicio As String ' = EditTextFechaInicio.ObtieneValorUserDataSource()
            Dim strFechaFin As String '= EditTextFechaFin.ObtieneValorUserDataSource()

            Dim l_fhaUnicio As Date
            Dim l_fhaFin As Date

            If m_blnCheckFactura Then

                strFechaInicioFormateada = String.Empty
                strFechaFinFormateada = String.Empty

                strFechaInicio = TxtFhaDesde.ObtieneValorUserDataSource ' oform.Items.Item(mc_strUIDFhaDesde).Specific.string ' EditTextFechaInicio.ObtieneValorUserDataSource()
                strFechaFin = TxtFhaHasta.ObtieneValorUserDataSource ' oform.Items.Item(mc_strUIDFhaHasta).Specific.string 'EditTextFechaFin.ObtieneValorUserDataSource()

                If Not String.IsNullOrEmpty(strFechaFin) And Not String.IsNullOrEmpty(strFechaInicio) Then
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_FechaVen"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_GRATER_EQUAL
                    oCondition.CondVal = strFechaInicio ' "20130625" ' l_fhaUnicio
                    oCondition.BracketCloseNum = 1
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_FechaVen"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_LESS_EQUAL
                    oCondition.CondVal = strFechaFin
                    oCondition.BracketCloseNum = 1

                End If

                If m_blnFiltraTipoVeh Then
                    strCodTipo = cboTipoVeh.ObtieneValorUserDataSource()

                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_Tipo"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = strCodTipo
                    oCondition.BracketCloseNum = 1
                End If

            End If

            oMatrix.Clear()

            dbCotizacion.Clear()
            dbCotizacion.Query(oConditions)
            oMatrix.LoadFromDataSource()

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean

        Dim oColumna As SAPbouiCOM.Column

        Try

            oColumna = oMatrix.Columns.Item(mc_strUIDIDContrato)
            oColumna.DataBind.SetBound(True, mc_strSCG_VEHICULO, mc_strIDContrato)

            oColumna = oMatrix.Columns.Item(mc_strUIDUnid)
            oColumna.DataBind.SetBound(True, mc_strSCG_VEHICULO, mc_strUnidad)

            oColumna = oMatrix.Columns.Item(mc_strUIDMarca)
            oColumna.DataBind.SetBound(True, mc_strSCG_VEHICULO, mc_strMarca)

            oColumna = oMatrix.Columns.Item(mc_strUIDEstilo)
            oColumna.DataBind.SetBound(True, mc_strSCG_VEHICULO, mc_strEstilo)

            oColumna = oMatrix.Columns.Item(mc_strUIDVIN)
            oColumna.DataBind.SetBound(True, mc_strSCG_VEHICULO, mc_strVIN)

            Return True
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False

        End Try
    End Function

    Public Function DevolverIDContrato(ByVal p_intRow As Integer, _
                                        ByVal p_strIDForm As String, _
                                        ByVal p_strColumna As String) As String

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strIDContrato As String

        oMatriz = DirectCast(SBO_Application.Forms.Item(p_strIDForm).Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
        strIDContrato = oMatriz.Columns.Item(p_strColumna).Cells.Item(p_intRow).Specific.String()

        Return strIDContrato

    End Function

    Public Sub DevolverDatosVehiculo(ByRef p_strUnidad As String,
                                      ByRef p_strVIN As String,
                                      ByRef p_strMarca As String,
                                      ByRef p_strEstilo As String,
                                      ByRef p_strModelo As String,
                                      ByVal p_strFormID As String,
                                      ByRef p_strIDVehiculo As String,
                                      ByRef p_strDocRecepcion As String,
                                      ByRef p_strDocPedido As String)

        Dim oForm As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intFila As Integer

        oForm = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oForm.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)

        intFila = oMatriz.GetNextSelectedRow()
        If intFila > -1 Then
            p_strUnidad = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue("U_Cod_Unid", intFila - 1).Trim
            p_strMarca = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue(mc_strMarca, intFila - 1).Trim
            p_strEstilo = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue(mc_strEstilo, intFila - 1).Trim
            p_strModelo = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue(mc_strModelo, intFila - 1).Trim
            p_strVIN = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue(mc_strVIN, intFila - 1).Trim
            p_strIDVehiculo = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue("Code", intFila - 1).Trim
            p_strDocRecepcion = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue("U_DocRecepcion", intFila - 1).Trim
            p_strDocPedido = oForm.DataSources.DBDataSources.Item(mc_strSCG_VEHICULO).GetValue("U_DocPedido", intFila - 1).Trim
        End If
    End Sub

    Private Sub AgregaCFLRecepcion(ByVal p_strIdCampo As String, ByVal p_strCFLId As String, ByVal p_strAlias As String)

        Dim oItem As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText

        oItem = m_oFormGenCotizacion.Items.Item(p_strIdCampo)
        oEdit = oItem.Specific

        oEdit.ChooseFromListUID = p_strCFLId
        oEdit.ChooseFromListAlias = p_strAlias

    End Sub

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form, ByVal p_strTipoObjeto As String, ByVal p_strUniqueID As String)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = p_strTipoObjeto ' "SCGD_EDV"
            oCFLCreationParams.UniqueID = p_strUniqueID '"CFL_Rec"
            oCFL = oCFLs.Add(oCFLCreationParams)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub


#End Region


End Class
