Imports System.Collections.Generic
Imports SCG.SBOFramework.DI
Imports DMSOneFramework
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SCG.DMSOne.Framework

Public Class MensajeriaAprobacion

#Region "Declaraciones"

    'declaracion de objetos generales 
    Private m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Private m_SBO_Application As SAPbouiCOM.Application
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    'objeto form 
    Private oForm As SAPbouiCOM.Form
    Private m_strDireccionConfiguracion As String
    Public n As NumberFormatInfo
    
    'Unidades por Nivel
    Private Const mc_strFormUnidadesPorNivel As String = "SCGD_UXN"
    Private Const strMatrizMSJ As String = "mtx_MSJ"

    'Unidades por Nivel 
    Private m_oUnidadesXNivel As UnidadesPorNivelCls

    'dt
    Private Shared dtSucursales As SAPbouiCOM.DataTable
    Private Shared dtSucursalesEnMSJS As SAPbouiCOM.DataTable
    Private Shared dtUsuariosBD As SAPbouiCOM.DataTable

    'objeto matriz
    Private Const strdtSucursales As String = "dtSucursales"
    Private Const strdtMSJS As String = "dtMSJS"
    Private Const strdtUsuariosBD As String = "dtUsuariosBD"

    'str tablas bd
    Private Const strMSJS As String = "@SCGD_MSJS"
    Private Const strMSJS1 As String = "@SCGD_MSJS1"

    'matriz usuariosDS
    Private MatrizMensajeriaDS As MatrizMensajeria

    'MSJ
    Dim oMatrizMSJ As SAPbouiCOM.Matrix
    Private Shared oComboSucursal As ComboBoxSBO
    Private Shared oTxtNivelesNV As EditTextSBO
    Private Shared oTxtSucursalNV As EditTextSBO
    Private Shared oTxtCode As EditTextSBO
    Private Shared oComboNiveles As ComboBoxSBO

    'variables
    Private _Agrega As Boolean = True
    Public Shared _int_IndicesAEliminar As New List(Of Integer)
    Private ExistenCambios As Boolean = False
    Private intPregunta As Integer = 1

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As SAPbouiCOM.Application)

        'declaracion de objetos acplication , company y decimaels 
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        m_strDireccionConfiguracion = CatchingEvents.DireccionConfiguracion
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

        'unidades por niveles
        m_oUnidadesXNivel = New UnidadesPorNivelCls(m_oCompany, SBOAplication)

    End Sub

    Public Property Agrega As Boolean
        Get
            Return _Agrega
        End Get
        Set(ByVal value As Boolean)
            _Agrega = value
        End Set
    End Property

#End Region

#Region "Metodos"

    'Metodo para cargar la pantalla de mensajeria
    Public Sub CargarFormularioMensajeria()
        'variables a utilizar
        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        'items de sap
        Dim oItem As SAPbouiCOM.Item
        Dim oMatriz As SAPbouiCOM.Matrix
        Try
            'parametros para el form que se abrirá
            fcp = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_MSJS"
            'se designa el XML que se cargara
            strXMLACargar = My.Resources.Resource.MensajeriaAprobacion
            fcp.XmlData = CargarDesdeXML(strXMLACargar)
            oForm = m_SBO_Application.Forms.AddEx(fcp)
            'deshabilito el boton crear del menu
            oForm.EnableMenu("1282", False)
            'oForm.EnableMenu("1288", True)
            'oForm.EnableMenu("1289", True)
            'oForm.EnableMenu("1290", True)
            'oForm.EnableMenu("1291", True)
            'oculto columnas de la matriz msjs
            oItem = oForm.Items.Item("mtx_MSJ")
            oMatriz = CType(oItem.Specific, SAPbouiCOM.Matrix)
            oMatriz.Columns.Item("Col_CSucu").Visible = False
            oMatriz.Columns.Item("Col_LineId").Visible = False
            'link entre edittext y tambien el combo
            LinkComponentes()
            'crea los datatableSBO
            CreaDataTablesSBO()
            'cargo los niveles de aprobacion
            Call CargarValidValuesEnCombos(oForm, "SELECT U_Codigo, U_Name FROM [@SCGD_ADMIN9]", "cboNAp")
            'carga sucursales
            Call CargarValidValuesEnCombos(oForm, "SELECT U_CSucu, U_Sucu FROM [@SCGD_MSJS]", "cboSucu")
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'CARGA EL XML DE LA PANTALLA 
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

    'Metodo para agregar items al menu [MENSAJERIA NIVELES APROBACION]
    Protected Friend Sub AddMenuItems()
        Dim strEtiquetaMenu As String = ""
        'Opciones de menus para MENSAJERIA APROBACION 
        If Utilitarios.MostrarMenu("SCGD_MSJ", m_SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_MSJ", m_SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_MSJ", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 74, False, True, "SCGD_CFG"))

        End If
    End Sub

    'asocia los edit text con la tabla en base de datos
    'asocia tambien el combo
    Private Sub LinkComponentes()

        Dim userDataSources As UserDataSources = oForm.DataSources.UserDataSources
        userDataSources.Add("nap", BoDataType.dt_LONG_TEXT, 100)

        oComboNiveles = New ComboBoxSBO("cboNAp", oForm, True, "", "nap")
        oComboNiveles.AsignaBinding()

        oComboSucursal = New ComboBoxSBO("cboSucu", oForm, True, "@SCGD_MSJS", "U_CSucu")
        oComboSucursal.AsignaBinding()

        oTxtCode = New EditTextSBO("txtCode", True, "@SCGD_MSJS", "Code", oForm)
        oTxtCode.AsignaBinding()


    End Sub

    'crea datatables para manejod e sucursales y Niveles de aprobacion
    Private Sub CreaDataTablesSBO()
        'datatable que es de sucursales
        dtSucursales = oForm.DataSources.DataTables.Add(strdtSucursales)
        dtSucursales.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
        dtSucursales.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric, 100)
        'datatable para amnejo de mensajeria
        dtSucursalesEnMSJS = oForm.DataSources.DataTables.Add(strdtMSJS)
        dtSucursalesEnMSJS.Columns.Add("Code", BoFieldsType.ft_AlphaNumeric, 100)
        dtSucursalesEnMSJS.Columns.Add("Name", BoFieldsType.ft_AlphaNumeric, 100)

        'datatable que es la matriz de mensajeria
        dtUsuariosBD = oForm.DataSources.DataTables.Add(strdtUsuariosBD)
        'dtMSJS_DS.Columns.Add("usua", BoFieldsType.ft_AlphaNumeric, 100)
        'dtMSJS_DS.Columns.Add("name", BoFieldsType.ft_AlphaNumeric, 100)
        'dtMSJS_DS.Columns.Add("csucu", BoFieldsType.ft_AlphaNumeric, 100)
        'dtMSJS_DS.Columns.Add("cnap", BoFieldsType.ft_AlphaNumeric, 100)
        'dtMSJS_DS.Columns.Add("lineid", BoFieldsType.ft_AlphaNumeric, 100)

        ''Instancia de la matriz de mensajeria
        'MatrizMensajeriaDS = New MatrizMensajeria("mtxMSJSDS", oForm, strdtMSJS_DS)
        'MatrizMensajeriaDS.CreaColumnas()
        'MatrizMensajeriaDS.LigaColumnas()
    End Sub

    'carga los combos
    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                            ByVal strQuery As String, _
                                                            ByRef strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Try
            oItem = oForm.Items.Item(strIDItem)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then

                    cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try

    End Sub

    'Carga las sucursales en el combo por medio de GeneralServices
    Public Sub CargaSucursales()
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim Existe As Boolean = False
        Dim UltimoCode As Integer = 0
        Dim ContIngresos As Integer = 0

        Try

            'cargo sucursales del sistema
            dtSucursales.ExecuteQuery("SELECT Code, Name FROM OUBR")
            'cargo sucursales creadas en UDO msjs
            dtSucursalesEnMSJS.ExecuteQuery("SELECT U_CSucu, U_Sucu FROM [@SCGD_MSJS]")
            UltimoCode = dtSucursalesEnMSJS.Rows.Count

            'Get GeneralService (oCmpSrv is the CompanyService)
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_MSJ")

            'Create data for new row in main UDO
            For i As Integer = 1 To dtSucursales.Rows.Count
                For x As Integer = 1 To dtSucursalesEnMSJS.Rows.Count
                    'verifica los codigos de sucursal de la tabla sucursales con la de mensajeria UDO
                    If dtSucursales.GetValue("Code", i - 1).ToString = dtSucursalesEnMSJS.GetValue("U_CSucu", x - 1) Then
                        Existe = True
                        Exit For
                    End If
                Next
                'si no existe lo ingresa por GeneralService
                If Not Existe Then
                    ContIngresos = ContIngresos + 1

                    oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)

                    'oGeneralData.SetProperty("Code", (UltimoCode + ContIngresos).ToString)
                    oGeneralData.SetProperty("Code", dtSucursales.GetValue("Code", i - 1).ToString)
                    oGeneralData.SetProperty("U_CSucu", dtSucursales.GetValue("Code", i - 1).ToString)
                    oGeneralData.SetProperty("U_Sucu", dtSucursales.GetValue("Name", i - 1).ToString)

                    'Add the new row, including children, to database
                    oGeneralService.Add(oGeneralData)
                Else
                    Existe = False
                End If
            Next

            If ContIngresos > 0 Then
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.IngresoSucursales1 & ContIngresos & My.Resources.Resource.IngresoSucursales2, _
                                                    BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)

                Call CargarValidValuesEnCombos(oForm, "SELECT U_CSucu, U_Sucu FROM [@SCGD_MSJS]", "cboSucu")
            Else
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.IngresoSucursales1 & ContIngresos & My.Resources.Resource.IngresoSucursales2, _
                                                    BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'aplica los filtros sobre la matriz de mensajeria
    Sub AplicaFiltros(ByVal str_CodSucursal As String, ByVal str_CodNivAprob As String, ByVal FormUID As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Try
            'obtengo el form del que sucedio el evento
            oForm = m_SBO_Application.Forms.Item(FormUID)
            'se obtiene la matriz de mensajeria
            oMatrizMSJ = DirectCast(oForm.Items.Item(strMatrizMSJ).Specific, SAPbouiCOM.Matrix)
            'comprueba codigos de sucursal y nivel de aprobacion
            If Not String.IsNullOrEmpty(str_CodSucursal) And Not String.IsNullOrEmpty(str_CodNivAprob) Then
                'se crea la coleccion de conditions
                oConditions = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                'se agrega la condicion
                oCondition = oConditions.Add()
                oCondition.BracketOpenNum = 2
                oCondition.Alias = "U_CSucu"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = str_CodSucursal
                oCondition.BracketCloseNum = 1
                'relacion entre condiciones
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                'se agrega el condition
                oCondition = oConditions.Add()
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_CNAp"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = str_CodNivAprob
                oCondition.BracketCloseNum = 2
                'se aplican las condiciones sobre el DBDataSource
                Call oForm.DataSources.DBDataSources.Item(strMSJS1).Query(oConditions)
                'se carga la matriz
                oMatrizMSJ.LoadFromDataSource()
            Else
                'se limpia la matriz
                oMatrizMSJ.Clear()
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'carga datasoruce de mensajes
    Public Sub CargaDataSourceMSJ(ByVal strSucu As String)
        Try
            If Not String.IsNullOrEmpty(strSucu) Then
                'dtMSJS_DS.ExecuteQuery(String.Format("SELECT LineId, U_CSucu, U_CNAp, U_Usua, U_Name FROM [@SCGD_MSJS1] WHERE U_CSucu = '{0}'", strSucu))
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ActualizaTablaMSJS1(ByVal strSucu As String, ByVal strNAp As String, ByVal FormUID As String)
        'datos leidos de matriz
        Dim strUsuario_lc As String = ""
        Dim strName_lc As String = ""
        Dim strCSucu_lc As String = ""
        Dim strCNAp_lc As String = ""
        Dim strLineId_lc As String = ""

        Dim iContador As Integer = 0
        Dim YaExiste As Boolean = False

        'objetos general services
        Dim oEdit As SAPbouiCOM.EditText
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChild As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try
            'se obtiene la matriz de mensajeria
            oMatrizMSJ = DirectCast(oForm.Items.Item(strMatrizMSJ).Specific, SAPbouiCOM.Matrix)

            oForm.DataSources.DBDataSources.Item(strMSJS1).Clear()

            'Get GeneralService (oCmpSrv is the CompanyService)
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_MSJ")

            'Get UDO record
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            'm_SBO_Application.StatusBar.SetText("-" & strSucu & "-", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
            oGeneralParams.SetProperty("Code", strSucu)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            If Agrega Then
                'Carga dt con usuarios en base de datos
                dtUsuariosBD.ExecuteQuery(String.Format("SELECT U_Usua, U_Name FROM [@SCGD_MSJS1] " & _
                                                        " WHERE U_CSucu = '{0}' AND U_CNAp = '{1}'", strSucu, strNAp))
                'Create data for new row in main UDO
                For i As Integer = 1 To oMatrizMSJ.RowCount
                    'Se obtienen los valores de la matriz de mensajeria
                    oEdit = oMatrizMSJ.Columns.Item("Col_Usua").Cells.Item(i).Specific
                    strUsuario_lc = oEdit.Value
                    oEdit = oMatrizMSJ.Columns.Item("Col_Name").Cells.Item(i).Specific
                    strName_lc = oEdit.Value
                    oEdit = oMatrizMSJ.Columns.Item("Col_CNAp").Cells.Item(i).Specific
                    strCNAp_lc = oEdit.Value
                    oEdit = oMatrizMSJ.Columns.Item("Col_CSucu").Cells.Item(i).Specific
                    strCSucu_lc = oEdit.Value

                    'verifica si el usuarios ya existe en BD
                    For iUbicacion As Integer = 0 To dtUsuariosBD.Rows.Count - 1
                        If strUsuario_lc.Trim = dtUsuariosBD.GetValue("U_Usua", iUbicacion) Then
                            YaExiste = True
                            Exit For
                        End If
                    Next

                    If Not YaExiste Then
                        If Not String.IsNullOrEmpty(strUsuario_lc) _
                            And Not String.IsNullOrEmpty(strName_lc) _
                            And Not String.IsNullOrEmpty(strCNAp_lc) _
                            And Not String.IsNullOrEmpty(strCSucu_lc) Then

                            'Create data for a row in the child table
                            oChildren = oGeneralData.Child("SCGD_MSJS1")
                            'agrego la linea
                            oChild = oChildren.Add
                            'seteo los datos de la linea 
                            oChild.SetProperty("U_Usua", strUsuario_lc)
                            oChild.SetProperty("U_Name", strName_lc)
                            oChild.SetProperty("U_CNAp", strCNAp_lc)
                            oChild.SetProperty("U_CSucu", strCSucu_lc)
                        End If
                    ElseIf YaExiste Then
                        YaExiste = False
                    End If
                Next

            ElseIf Not Agrega Then
                'Actualiza el UDO y los registros hijos 
                oChildren = oGeneralData.Child("SCGD_MSJS1")
                iContador = _int_IndicesAEliminar.Count - 1
                For i As Integer = 0 To _int_IndicesAEliminar.Count - 1
                    'elimina registro de lineas de UDO
                    oChildren.Remove(_int_IndicesAEliminar(iContador))
                    iContador = iContador - 1
                Next
            End If
            'Actualiza el UDO y los registros hijos 
            oGeneralService.Update(oGeneralData)
            'se aplican filtros sobre la matriz de mensajeria
            Call AplicaFiltros(strSucu, strNAp, FormUID)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region

#Region "Eventos"

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                    ByVal FormUID As String,
                                    ByRef BubbleEvent As Boolean,
                                    ByVal comp As SAPbobsCOM.Company,
                                    ByVal strUserName As String,
                                    ByVal strPass As String)

        'obtenemos el form de mensajeria
        oForm = m_SBO_Application.Forms.Item(FormUID)

        'verifica el form
        If Not oForm Is Nothing _
                       AndAlso pval.ActionSuccess Then

            Select Case pval.ItemUID
                Case "btn_MSJAdd"
                    Agrega = True
                    'verifica que se escoja sucursal y Niv Ap
                    If Not String.IsNullOrEmpty(oComboSucursal.Especifico.Value) And
                        Not String.IsNullOrEmpty(oComboNiveles.Especifico.Value) Then
                        If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_UXN", False, m_SBO_Application) Then
                            Dim objUnidades As New UnidadesPorNivelCls(m_oCompany, m_SBO_Application)
                            'set a las propiedades
                            UnidadesPorNivelCls.StrSucursal = oComboSucursal.Especifico.Value
                            UnidadesPorNivelCls.StrNivelAprobacion = oComboNiveles.Especifico.Value
                            'true cambios 
                            ExistenCambios = True
                            'cargo formulario
                            Call m_oUnidadesXNivel.CargaFormUnidades(oForm, True)
                            'si no se esta creando uno se pone en actualizar
                            'actualizo el caption del boton
                            Dim oBtn As Button
                            oBtn = DirectCast(oForm.Items.Item("1").Specific, Button)
                            oBtn.Caption = My.Resources.Resource.Buscar
                        End If
                    Else
                        'dbe ingreasr sucursal y nva
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorUnidadesXSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    End If
                Case "btn_MSJEli"
                    Agrega = False
                    'si no se esta creando uno se pone en actualizar
                    If Not oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        'verifica que se escoja sucursal y Niv Ap
                        If Not String.IsNullOrEmpty(oComboSucursal.Especifico.Value) And
                            Not String.IsNullOrEmpty(oComboNiveles.Especifico.Value) Then
                            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_UXN", False, m_SBO_Application) Then
                                Dim objUnidades As New UnidadesPorNivelCls(m_oCompany, m_SBO_Application)
                                'carga propiedades
                                UnidadesPorNivelCls.StrSucursal = oComboSucursal.Especifico.Value
                                UnidadesPorNivelCls.StrNivelAprobacion = oComboNiveles.Especifico.Value
                                'true cambios
                                ExistenCambios = True
                                'carga form de unidades
                                Call m_oUnidadesXNivel.CargaFormUnidades(oForm, False)
                                'actualizo el caption del boton
                                Dim oBtn As Button
                                oBtn = DirectCast(oForm.Items.Item("1").Specific, Button)
                                oBtn.Caption = My.Resources.Resource.Buscar
                            End If
                        Else
                            'dbe ingreasr sucursal y nva
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorUnidadesXSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    End If
                Case "btnCgSu"
                    'carga sucursales por general service
                    Call CargaSucursales()
                Case "1281"
                    oForm.Items.Item("cboSucu").Enabled = True
                Case "1"
                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        'pongo el formulario en modo buscar
                        oForm.Items.Item("cboSucu").Enabled = False
                        oForm.Items.Item("cboNAp").Enabled = True
                        oForm.Items.Item("1").Enabled = False
                        'actualizo el caption del boton
                        Dim oBtn As Button
                        oBtn = DirectCast(oForm.Items.Item("1").Specific, Button)
                        oBtn.Caption = My.Resources.Resource.Buscar
                        'oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                    End If
                Case "btnAct"
                    'pongo el formulario en modo buscar
                    Dim strSucu As String = ""
                    Dim strCNAp As String = ""
                    'hya cambios en matriz 
                    If ExistenCambios Then
                        'se obtienen valores de codigos
                        strSucu = oComboSucursal.Especifico.Value
                        strSucu = strSucu.Trim
                        strCNAp = oComboNiveles.Especifico.Value
                        strCNAp = strCNAp.Trim
                        If Not String.IsNullOrEmpty(strSucu) And
                            Not String.IsNullOrEmpty(strCNAp) Then
                            'actualiza el datasource
                            Call ActualizaTablaMSJS1(strSucu, strCNAp, FormUID)
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                            'desactiva cod niv aprob
                            oForm.Items.Item("cboNAp").Enabled = True
                        End If
                        ExistenCambios = False
                    Else
                        'no existen cambios
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.NoHayCambios, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                    End If
            End Select
        End If
        'verifica el form
        'BEFORE ACTION
        If Not oForm Is Nothing _
                       AndAlso pval.BeforeAction Then
            Select Case pval.ItemUID
                Case "1"
                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        'actualizo el caption del boton
                        Dim oBtn As Button
                        oBtn = DirectCast(oForm.Items.Item("1").Specific, Button)
                        oBtn.Caption = My.Resources.Resource.Buscar
                    End If
            End Select

        End If
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosCombo(ByRef oTmpForm As SAPbouiCOM.Form, _
                                  ByVal pval As SAPbouiCOM.ItemEvent, _
                                  ByVal FormUID As String, _
                                  ByRef BubbleEvent As Boolean)

        Dim str_CodSucursal As String = ""
        Dim str_CodNivAprob As String = ""
        Dim strConsulta As String = ""

        Try
            If pval.BeforeAction Then
                'seleccion de item
                Select Case pval.ItemUID
                    'combo de niveles de aprobacion
                    Case "cboNAp"
                        'pregunta ante cambios 
                        If ExistenCambios Then
                            intPregunta = 0
                            intPregunta = m_SBO_Application.MessageBox(My.Resources.Resource.PreguntaUsuariosMensajeria, 1, My.Resources.Resource.Si, My.Resources.Resource.No)
                            'no continuar, cancelar ejecucion 
                            If intPregunta = 2 Then
                                BubbleEvent = False
                            Else
                                ExistenCambios = False
                            End If
                        End If
                End Select
            End If
            If pval.ActionSuccess Then
                'seleccion de item
                Select Case pval.ItemUID
                    'combo de niveles de aprobacion
                    Case "cboNAp"
                        'si se aceptan los cambios
                        str_CodSucursal = ""
                        str_CodNivAprob = ""
                        'se obtienen valores de codigos
                        str_CodSucursal = oComboSucursal.Especifico.Value
                        str_CodNivAprob = oComboNiveles.Especifico.Value
                        If Not String.IsNullOrEmpty(str_CodNivAprob) And
                           Not String.IsNullOrEmpty(str_CodSucursal) Then
                            'se aplican filtros sobre la matriz de mensajeria
                            Call AplicaFiltros(str_CodSucursal, str_CodNivAprob, FormUID)
                            'actualizo el caption del boton
                            Dim oBtn As Button
                            oBtn = DirectCast(oForm.Items.Item("1").Specific, Button)
                            oBtn.Caption = "Buscar"
                        End If
                        'combo de sucursales
                    Case "cboSucu"
                        str_CodSucursal = ""
                        str_CodNivAprob = ""
                        'codigo de sucursal al edittext de code
                        'se obtienen valores de codigos
                        str_CodSucursal = oComboSucursal.Especifico.Value
                        str_CodNivAprob = oComboNiveles.Especifico.Value
                        If Not oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            If Not String.IsNullOrEmpty(str_CodSucursal) Then
                                'carga datasource de msj
                                Call CargaDataSourceMSJ(str_CodSucursal)
                                'se aplican filtros sobre la matriz de mensajeria
                                Call AplicaFiltros(str_CodSucursal, str_CodNivAprob, FormUID)
                            End If
                        End If
                End Select
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoMenuBuscar(ByVal pval As SAPbouiCOM.MenuEvent, ByVal oForm As SAPbouiCOM.Form)
        Try
            'limpia el combo de niveles 
            oComboSucursal.AsignaValorDataSource("")
            'habilita sucursales
            oForm.Items.Item("cboSucu").Enabled = True
            'habilita btn 1
            oForm.Items.Item("1").Enabled = True
            'limpia el combo de niveles 
            oComboNiveles.AsignaValorUserDataSource("")
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ManejoEventoGotFocus(ByVal oForm As SAPbouiCOM.Form, ByVal pval As SAPbouiCOM.ItemEvent)

        Dim strSucu As String = ""

        Try
            Select Case pval.ItemUID
                'Case ""
                '    If oTxtSucursal IsNot Nothing Then

                '        strSucu = oTxtSucursal.Especifico.Value
                '        If Not String.IsNullOrEmpty(strSucu) Then
                '            oTxtSucursalNV.AsignaValorUserDataSource(Utilitarios.EjecutarConsulta(String.Format("SELECT Name FROM OUBR WHERE Code = '{0}'", strSucu), _
                '                                                                                                     m_oCompany.CompanyDB, m_oCompany.Server))
                '        End If
                '    End If

            End Select
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region

End Class
