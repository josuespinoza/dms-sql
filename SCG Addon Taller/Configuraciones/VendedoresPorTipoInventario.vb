Imports System.Collections.Generic
Imports SCG.SBOFramework.DI
Imports DMSOneFramework
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SCG.DMSOne.Framework

Partial Public Class VendedoresPorTipoInventario

#Region "Declaraciones"

    'declaracion de objetos generales 
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application

    'objeto form 
    Private oForm As Form

    Public n As NumberFormatInfo
    
    'componentes de interfaz
    Private UDS_dtBusquedas As UserDataSources
    Private Shared oTxtCodUsu As EditTextSBO
    Private Shared oTxtUsuario As EditTextSBO
    Private Shared oChkSucursal As CheckBoxSBO
    Private Shared oComboSucursal As ComboBoxSBO
    Private Shared oChkTodosV As CheckBoxSBO
    Private Shared oChkTodosTI As CheckBoxSBO

    Private dtVendedores As DataTable
    Private Const strDtVendedores As String = "Vendedores"
    Private dtTipoInventario As DataTable
    Private Const strDtTipoInventario As String = "TipoInventario"

    Private MatrizVendedores As MatrizVendedores
    Private MatrizTI As MatrizTipoInventario

    Private blExisteSeleccionado As Boolean = False
    Private _dtExistentes As Data.DataTable
    Private _blMultiple As Boolean = False
    Private _noHaySeleccionados As Boolean = False


#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)

        'declaracion de objetos acplication , company y decimaels 
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

    Public Property dtExistentes As Data.DataTable
        Get
            Return _dtExistentes
        End Get
        Set(ByVal value As Data.DataTable)
            _dtExistentes = value
        End Set
    End Property

    Public Property blMultiple As Boolean
        Get
            Return _blMultiple
        End Get
        Set(ByVal value As Boolean)
            _blMultiple = value
        End Set
    End Property

    Public Property noHaySeleccionados As Boolean
        Get
            Return _noHaySeleccionados
        End Get
        Set(ByVal value As Boolean)
            _noHaySeleccionados = value
        End Set
    End Property

#End Region

#Region "Metodos"

    ''' <summary>
    ''' 'CARGA EL XML DE LA PANTALLA 
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    ''' <summary>
    ''' 'Metodo para agregar items al menu [VENDEDORES POR TIPO DE INVENTARIO]
    ''' </summary>
    ''' <remarks></remarks>
    Protected Friend Sub AddMenuItems()
        Dim strEtiquetaMenu As String = ""
        'Opciones de menus para VENDEDORES POR TIPO DE INVENTARIO 
        If Utilitarios.MostrarMenu("SCGD_VTI", m_SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_VTI", m_SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_VTI", BoMenuType.mt_STRING, strEtiquetaMenu, 75, False, True, "SCGD_CFG"))

        End If
    End Sub

    ''' <summary>
    ''' carga los combos
    ''' </summary>
    ''' <param name="oForm">Formulario</param>
    ''' <param name="strQuery">Query a ejecutar para cargar el combo</param>
    ''' <param name="strIDItem">Combo que desea cargar</param>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' Metodo para cargar la pantalla de vendedores por tipos de inventario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarFormularioVendedoresTipoInventario()
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
            fcp.FormType = "SCGD_VENDXTI"
            'se designa el XML que se cargara
            strXMLACargar = My.Resources.Resource.VendedoresPorTipoInventario
            fcp.XmlData = CargarDesdeXML(strXMLACargar)
            oForm = m_SBO_Application.Forms.AddEx(fcp)

            oForm.Freeze(True)

            'deshabilita los menus superioes
            DesHabilitarMenus()

            'carga sucursales
            Call CargarValidValuesEnCombos(oForm, "SELECT Code, Name FROM OUBR", "cboSucu")

            'link entre edittext y tambien el combo
            LinkComponentes()
            
            CreaDataTablesSBO()

            dtVendedores.ExecuteQuery("SELECT '' AS seleccionar, USER_CODE AS codigo, U_NAME AS vendedor FROM OUSR")
            MatrizVendedores.Matrix.LoadFromDataSource()

            dtTipoInventario.ExecuteQuery("SELECT '' AS seleccionar, Code AS codigo, Name AS ti FROM [@SCGD_TIPOVEHICULO]")
            MatrizTI.Matrix.LoadFromDataSource()

            oForm.Freeze(False)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Deshabilita los menus superiores del formulario
    ''' asi como las opciones de copiar, cortar y pegar 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DesHabilitarMenus()
        If oForm IsNot Nothing Then

            'deshabilito el boton crear del menu
            oForm.EnableMenu("1282", False)
            oForm.EnableMenu("1288", False)
            oForm.EnableMenu("1289", False)
            oForm.EnableMenu("1290", False)
            oForm.EnableMenu("1291", False)
            oForm.EnableMenu("1281", False)
            oForm.EnableMenu("771", False)
            oForm.EnableMenu("772", False)
            oForm.EnableMenu("773", False)

        End If
    End Sub

    ''' <summary>
    ''' asocia los edit text con la tabla en base de datos
    ''' asocia tambien el combo
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LinkComponentes()

        UDS_dtBusquedas = oForm.DataSources.UserDataSources
        UDS_dtBusquedas.Add("CodVend", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtBusquedas.Add("NomVend", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtBusquedas.Add("Sucu", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtBusquedas.Add("Sucursal", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtBusquedas.Add("TodV", BoDataType.dt_LONG_TEXT, 100)
        UDS_dtBusquedas.Add("TodTI", BoDataType.dt_LONG_TEXT, 100)
        
        oTxtCodUsu = New EditTextSBO("txtCodUsu", True, "", "CodVend", oForm)
        oTxtCodUsu.AsignaBinding()

        oTxtUsuario = New EditTextSBO("txtNomUsu", True, "", "NomVend", oForm)
        oTxtUsuario.AsignaBinding()
        
        oChkSucursal = New CheckBoxSBO("chkSucu", True, "", "Sucu", oForm)
        oChkSucursal.AsignaBinding()
        
        oComboSucursal = New ComboBoxSBO("cboSucu", oForm, True, "", "Sucursal")
        oComboSucursal.AsignaBinding()

        oChkTodosV = New CheckBoxSBO("chkTodV", True, "", "TodV", oForm)
        oChkTodosV.AsignaBinding()

        oChkTodosTI = New CheckBoxSBO("chkTodTI", True, "", "TodTI", oForm)
        oChkTodosTI.AsignaBinding()
        
    End Sub

    'crea datatables para manejod e sucursales y Niveles de aprobacion
    Private Sub CreaDataTablesSBO()

        'datatable para amnejo de vendedores
        dtVendedores = oForm.DataSources.DataTables.Add(strDtVendedores)
        dtVendedores.Columns.Add("seleccionar", BoFieldsType.ft_AlphaNumeric, 100)
        dtVendedores.Columns.Add("codigo", BoFieldsType.ft_AlphaNumeric, 100)
        dtVendedores.Columns.Add("vendedor", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizVendedores = New MatrizVendedores("mtx_Vend", oForm, strDtVendedores)
        MatrizVendedores.CreaColumnas()
        MatrizVendedores.LigaColumnas()
        
        'datatable que es de tipos de inventario
        dtTipoInventario = oForm.DataSources.DataTables.Add(strDtTipoInventario)
        dtTipoInventario.Columns.Add("seleccionar", BoFieldsType.ft_AlphaNumeric, 100)
        dtTipoInventario.Columns.Add("codigo", BoFieldsType.ft_AlphaNumeric, 100)
        dtTipoInventario.Columns.Add("ti", BoFieldsType.ft_AlphaNumeric, 100)

        MatrizTI = New MatrizTipoInventario("mtx_TipoI", oForm, strDtTipoInventario)
        MatrizTI.CreaColumnas()
        MatrizTI.LigaColumnas()
        
    End Sub

    ''' <summary>
    ''' Ejecuta la busqueda con los filtros seleccionados
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EjecutarBusqueda()
        Dim strCodVend As String = ""
        Dim strVend As String = ""
        Dim strSucu As String = ""
        Dim Bandera As Boolean = False
        Dim strConsultaSelect As String = "SELECT '' as seleccionar, USER_CODE as codigo , U_NAME as vendedor FROM OUSR "
        Dim strConsultaWhere As String = " WHERE "

        Try

            strCodVend = oTxtCodUsu.ObtieneValorUserDataSource
            strVend = oTxtUsuario.ObtieneValorUserDataSource
            strSucu = oComboSucursal.ObtieneValorUserDataSource

            'valida si se ingreso un valor en el codigo de vendedor
            If Not String.IsNullOrEmpty(strCodVend) Then
                strConsultaWhere = strConsultaWhere + String.Format(" USER_CODE LIKE '{0}%' ", strCodVend)
                Bandera = True
            Else

                strConsultaWhere = strConsultaWhere + String.Format(" USER_CODE IS NOT NULL ", strCodVend)
                Bandera = True
            End If

            If Not String.IsNullOrEmpty(strVend) Then

                If Bandera Then
                    'relacion entre condiciones
                    strConsultaWhere = strConsultaWhere + " AND "
                    Bandera = False
                End If

                strConsultaWhere = strConsultaWhere + String.Format(" U_NAME LIKE '{0}%' ", strVend)

                Bandera = True
            End If

            If Not String.IsNullOrEmpty(strSucu) _
                And oChkSucursal.ObtieneValorUserDataSource = "Y" _
                And Not oChkSucursal.ObtieneValorUserDataSource = Nothing Then

                If Bandera Then
                    'relacion entre condiciones
                    strConsultaWhere = strConsultaWhere + " AND "
                    Bandera = False
                End If

                strConsultaWhere = strConsultaWhere + String.Format(" Branch = '{0}' ", strSucu)

                Bandera = True
            End If

            'carga los vendedores 
            dtVendedores.ExecuteQuery(strConsultaSelect + strConsultaWhere)

            'carga la matriz
            MatrizVendedores.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub GuardarPermisos()

        Dim strCodigoVendedor As String = ""
        Dim strCodigoTipoInventario As String = ""
        Dim lsListaEliminadas As Generic.IList(Of String) = New Generic.List(Of String)
        Dim ExisteEliminadas As Boolean = False

        Try

            MatrizVendedores.Matrix.FlushToDataSource()
            MatrizTI.Matrix.FlushToDataSource()


            For i As Integer = 0 To dtVendedores.Rows.Count - 1
                strCodigoVendedor = ""

                If dtVendedores.GetValue("seleccionar", i) = "Y" Then

                    strCodigoVendedor = dtVendedores.GetValue("codigo", i)

                    If Not blMultiple Then
                        ExisteEliminadas = ProcesaEliminadas(lsListaEliminadas)
                    End If

                    If lsListaEliminadas.Count > 0 Then
                        EliminaPermisos(strCodigoVendedor, lsListaEliminadas)
                    End If

                    'Utilitarios.EjecutarConsulta(String.Format("DELETE FROM [@SCGD_CONFEMPXVEH] WHERE U_Usuario = '{0}'", strCodigoVendedor),
                    '                             m_oCompany.CompanyDB,
                    '                             m_oCompany.Server)

                    For x As Integer = 0 To dtTipoInventario.Rows.Count - 1
                        strCodigoTipoInventario = ""

                        If dtTipoInventario.GetValue("seleccionar", x) = "Y" Then

                            strCodigoTipoInventario = dtTipoInventario.GetValue("codigo", x)

                            InsertaPermiso(strCodigoVendedor, strCodigoTipoInventario)

                        End If

                    Next

                End If

            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Sub InsertaPermiso(ByVal strCodV As String, ByVal strCodTI As String)

        If Not String.IsNullOrEmpty(strCodV) _
            And Not String.IsNullOrEmpty(strCodTI) Then
            
            Dim strQuerySeleccion As String = ""
            Dim strQueryInsersion As String = ""
            Dim strExiste As String = ""
            Dim strTotal As String = ""
            Dim intTotal As Integer = 0

            strQuerySeleccion = String.Format("SELECT Code FROM [@SCGD_CONFEMPXVEH] WHERE U_Tipo_Inv = '{0}' AND U_Usuario = '{1}'", strCodTI, strCodV)

            strExiste = Utilitarios.EjecutarConsulta(strQuerySeleccion,
                                                     m_oCompany.CompanyDB,
                                                     m_oCompany.Server)

            If String.IsNullOrEmpty(strExiste) Then

                strTotal = Utilitarios.EjecutarConsulta(" SELECT ISNULL(MAX(CAST(CODE AS INT)),0) FROM [@SCGD_CONFEMPXVEH] ",
                                                        m_oCompany.CompanyDB,
                                                        m_oCompany.Server)

                intTotal = Integer.Parse(strTotal)
                
                strQueryInsersion = String.Format("INSERT INTO [@SCGD_CONFEMPXVEH] (Code ,Name ,U_Tipo_Inv ,U_Usuario )VALUES('{0}','{1}','{2}','{3}')",
                                                  intTotal + 1, intTotal + 1, strCodTI, strCodV)

                Utilitarios.EjecutarConsulta(strQueryInsersion,
                                                        m_oCompany.CompanyDB,
                                                        m_oCompany.Server)
            End If


        End If

    End Sub

    Private Sub SeleccionaTiposPorVendedor(ByVal pval As SAPbouiCOM.ItemEvent)
        Dim strCodigoVendedor As String = ""
        Dim strTipoInv As String = ""
        Dim dtTIxVend As System.Data.DataTable
        Dim blExistenMarcadas As Boolean = False

        Try

            If dtVendedores.Rows.Count > 0 Then

                MatrizVendedores.Matrix.FlushToDataSource()
                MatrizTI.Matrix.FlushToDataSource()

                If dtVendedores.GetValue("seleccionar", pval.Row - 1) = "Y" Then

                    If Not blExisteSeleccionado Then

                        blExisteSeleccionado = True
                        blMultiple = False

                        strCodigoVendedor = dtVendedores.GetValue("codigo", pval.Row - 1)

                        dtTIxVend = Utilitarios.EjecutarConsultaDataTable(String.Format("SELECT U_Tipo_Inv as codigo FROM [@SCGD_CONFEMPXVEH] WHERE U_Usuario = '{0}'", strCodigoVendedor),
                                                                           m_oCompany.CompanyDB,
                                                                           m_oCompany.Server)

                        dtExistentes = dtTIxVend

                        For t As Integer = 0 To dtTIxVend.Rows.Count - 1

                            strTipoInv = dtTIxVend.Rows(t)("codigo").ToString.Trim()

                            For i As Integer = 0 To dtTipoInventario.Rows.Count - 1

                                If strTipoInv = dtTipoInventario.GetValue("codigo", i) Then
                                    dtTipoInventario.SetValue("seleccionar", i, "Y")
                                End If

                            Next
                        Next
                    Else
                        blMultiple = True
                        blExisteSeleccionado = True

                        For i As Integer = 0 To dtTipoInventario.Rows.Count - 1
                            dtTipoInventario.SetValue("seleccionar", i, "N")
                        Next

                    End If
                ElseIf dtVendedores.GetValue("seleccionar", pval.Row - 1) = "N" Then
                    blExistenMarcadas = False

                    For i As Integer = 0 To dtVendedores.Rows.Count - 1
                        If dtVendedores.GetValue("seleccionar", i) = "Y" Then
                            blExistenMarcadas = True
                            Exit For
                        End If
                    Next
                    If Not blExistenMarcadas Then
                        For i As Integer = 0 To dtTipoInventario.Rows.Count - 1
                            dtTipoInventario.SetValue("seleccionar", i, "N")
                        Next
                        blExisteSeleccionado = False
                        blMultiple = False
                    End If
                End If

                MatrizVendedores.Matrix.LoadFromDataSource()
                MatrizTI.Matrix.LoadFromDataSource()

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Private Function ProcesaEliminadas(ByRef ls As Generic.IList(Of String)) As Boolean
        Dim blEliminada As Boolean = True
        Dim blExistenEliminadas As Boolean = False
        Dim strCodeExistente As String = ""
        Dim strCodeTI As String = ""

        Try

            For i As Integer = 0 To dtExistentes.Rows.Count - 1

                strCodeExistente = dtExistentes.Rows(i)("codigo").ToString.Trim()
                blEliminada = True

                For x As Integer = 0 To dtTipoInventario.Rows.Count - 1

                    If dtTipoInventario.GetValue("seleccionar", x) = "Y" Then

                        strCodeTI = dtTipoInventario.GetValue("codigo", x)

                        If strCodeExistente = strCodeTI Then
                            blEliminada = False
                            Exit For
                        End If
                    End If

                Next

                If blEliminada Then

                    ls.Add(strCodeExistente)
                    blExistenEliminadas = True

                End If

            Next

            Return blExistenEliminadas

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Function
    
    Private Sub EliminaPermisos(ByVal strCodigoVendedor As String, ByVal lsListaEliminadas As IList(Of String))
        If Not String.IsNullOrEmpty(strCodigoVendedor)  Then

            Dim strQueryBorrar As String = ""
            Dim contador As String = ""
            Dim strExiste As String = ""

            If lsListaEliminadas.Count > 0 Then

                For Each Str As String In lsListaEliminadas

                    strQueryBorrar = String.Format("DELETE FROM [@SCGD_CONFEMPXVEH] WHERE U_Usuario = '{0}' AND U_Tipo_Inv = '{1}'", strCodigoVendedor, Str)

                    Utilitarios.EjecutarConsulta(strQueryBorrar,
                                                   m_oCompany.CompanyDB,
                                                   m_oCompany.Server)
                Next

            End If


        End If


    End Sub

#End Region

#Region "Eventos"

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                    ByVal FormUID As String,
                                    ByRef BubbleEvent As Boolean,
                                    ByVal comp As SAPbobsCOM.Company)

        'obtenemos el form de mensajeria
        oForm = m_SBO_Application.Forms.Item(FormUID)

        'verifica el form
        If Not oForm Is Nothing _
                       AndAlso pval.ActionSuccess Then

            Select Case pval.ItemUID
                Case "chkSucu"
                    
                    If oChkSucursal.ObtieneValorUserDataSource() = "N" Then
                        oComboSucursal.AsignaValorUserDataSource("")
                        oForm.Items.Item("cboSucu").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                    ElseIf oChkSucursal.ObtieneValorUserDataSource() = "Y" Then
                        oForm.Items.Item("cboSucu").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                    End If

                Case "btnBuscar"
                    'ejecuta la busqueda de acuerdo a los filtros
                    oForm.Freeze(True)

                    oChkTodosV.AsignaValorUserDataSource("N")
                    
                    EjecutarBusqueda()

                    MatrizTI.Matrix.FlushToDataSource()
                    
                    If dtTipoInventario.Rows.Count > 0 Then

                        For i As Integer = 0 To dtTipoInventario.Rows.Count - 1

                            If oChkTodosV.ObtieneValorUserDataSource() = "Y" Then
                                dtTipoInventario.SetValue("seleccionar", i, "N")
                            End If

                        Next

                    End If
                    MatrizTI.Matrix.LoadFromDataSource()
                    
                    oForm.Freeze(False)
                Case "chkTodV"
                    oForm.Freeze(True)

                    'ejecuta la seleccion de todos los datos de la matriz de vendedores 
                    MatrizVendedores.Matrix.FlushToDataSource()
                    MatrizTI.Matrix.FlushToDataSource()

                    If oChkTodosV.ObtieneValorUserDataSource = "N" Then
                        noHaySeleccionados = True
                        blExisteSeleccionado = False
                        blMultiple = False
                    ElseIf oChkTodosV.ObtieneValorUserDataSource = "Y" Then
                        noHaySeleccionados = False
                        blExisteSeleccionado = True
                        blMultiple = True
                    End If

                    If dtVendedores.Rows.Count > 0 Then

                        For i As Integer = 0 To dtVendedores.Rows.Count - 1

                            If oChkTodosV.ObtieneValorUserDataSource() = "Y" Then
                                dtVendedores.SetValue("seleccionar", i, "Y")
                            ElseIf oChkTodosV.ObtieneValorUserDataSource() = "N" Then
                                dtVendedores.SetValue("seleccionar", i, "N")
                            End If

                        Next

                    End If

                    If dtTipoInventario.Rows.Count > 0 Then

                        For i As Integer = 0 To dtTipoInventario.Rows.Count - 1

                            If oChkTodosV.ObtieneValorUserDataSource() = "Y" Then
                                dtTipoInventario.SetValue("seleccionar", i, "N")
                            End If

                        Next

                    End If

                    MatrizTI.Matrix.LoadFromDataSource()
                    MatrizVendedores.Matrix.LoadFromDataSource()

                    oForm.Freeze(False)

                Case "chkTodTI"
                    oForm.Freeze(True)
                    MatrizTI.Matrix.FlushToDataSource()

                    'ejecuta la selecciona de todos los registros de los tipos de inventario
                    If dtTipoInventario.Rows.Count > 0 Then
                        For i As Integer = 0 To dtTipoInventario.Rows.Count - 1

                            If oChkTodosTI.ObtieneValorUserDataSource() = "Y" Then
                                dtTipoInventario.SetValue("seleccionar", i, "Y")
                            ElseIf oChkTodosTI.ObtieneValorUserDataSource() = "N" Then
                                dtTipoInventario.SetValue("seleccionar", i, "N")
                            End If
                        Next

                        MatrizTI.Matrix.LoadFromDataSource()
                    End If
                    oForm.Freeze(False)
                Case "1"
                    GuardarPermisos()
                Case "mtx_Vend"
                    If pval.ColUID = "Col_SelV" Then

                        oForm.Freeze(True)

                        SeleccionaTiposPorVendedor(pval)

                        oForm.Freeze(False)
                    End If
            End Select
        End If
        'verifica el form
        'BEFORE ACTION
        If Not oForm Is Nothing _
                       AndAlso pval.BeforeAction Then
            Select Case pval.ItemUID
                Case "1"
                    If pval.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                        Dim intPregunta As Integer = 0

                        intPregunta = m_SBO_Application.MessageBox(My.Resources.Resource.PreguntaPermisosPorTipoInventario, 1, My.Resources.Resource.Si, My.Resources.Resource.No, My.Resources.Resource.btnCancelar)

                        If intPregunta <> 1 Then
                            BubbleEvent = False
                        End If

                    End If
                Case "btnBuscar"

                    Dim intPregunta As Integer = 0
                    Dim Preguntar As Boolean = False

                    MatrizVendedores.Matrix.FlushToDataSource()

                    If dtVendedores.Rows.Count > 0 Then

                        For i As Integer = 0 To dtVendedores.Rows.Count - 1

                            If dtVendedores.GetValue("seleccionar", i) = "Y" Then
                                Preguntar = True
                                Exit For
                            End If

                        Next
                    End If

                    If Preguntar Then
                        intPregunta = m_SBO_Application.MessageBox(My.Resources.Resource.PreguntaPermisosPorTipoInventario2, 1, My.Resources.Resource.Si, My.Resources.Resource.No, My.Resources.Resource.btnCancelar)

                        If intPregunta <> 1 Then
                            BubbleEvent = False
                        End If

                    End If
                    
            End Select

        End If
    End Sub

    '<System.CLSCompliant(False)> _
    'Public Sub ManejoEventosCombo(ByRef oTmpForm As SAPbouiCOM.Form, _
    '                              ByVal pval As SAPbouiCOM.ItemEvent, _
    '                              ByVal FormUID As String, _
    '                              ByRef BubbleEvent As Boolean)

    '    Dim str_CodSucursal As String = ""
    '    Dim str_CodNivAprob As String = ""
    '    Dim strConsulta As String = ""

    '    Try
    '        If pval.BeforeAction Then
    '            'seleccion de item
    '            Select Case pval.ItemUID
    '                'combo de niveles de aprobacion
    '                Case "cboNAp"
    '                    'pregunta ante cambios 
    '                    If ExistenCambios Then
    '                        intPregunta = 0
    '                        intPregunta = m_SBO_Application.MessageBox(My.Resources.Resource.PreguntaUsuariosMensajeria, 1, My.Resources.Resource.Si, My.Resources.Resource.No)
    '                        'no continuar, cancelar ejecucion 
    '                        If intPregunta = 2 Then
    '                            BubbleEvent = False
    '                        Else
    '                            ExistenCambios = False
    '                        End If
    '                    End If
    '            End Select
    '        End If
    '        If pval.ActionSuccess Then
    '            'seleccion de item
    '            Select Case pval.ItemUID
    '                'combo de niveles de aprobacion
    '                Case "cboNAp"
    '                    'si se aceptan los cambios
    '                    str_CodSucursal = ""
    '                    str_CodNivAprob = ""
    '                    'se obtienen valores de codigos
    '                    str_CodSucursal = oComboSucursal.Especifico.Value
    '                    str_CodNivAprob = oComboNiveles.Especifico.Value
    '                    If Not String.IsNullOrEmpty(str_CodNivAprob) And
    '                       Not String.IsNullOrEmpty(str_CodSucursal) Then
    '                        'se aplican filtros sobre la matriz de mensajeria
    '                        Call CargaLineas(str_CodSucursal, str_CodNivAprob, FormUID)
    '                        'actualizo el caption del boton
    '                        Dim oBtn As System.Windows.Forms.Button
    '                        oBtn = DirectCast(oForm.Items.Item("1").Specific, System.Windows.Forms.Button)
    '                        oBtn.Caption = "Buscar"
    '                    End If
    '                    'combo de sucursales
    '                Case "cboSucu"
    '                    str_CodSucursal = ""
    '                    str_CodNivAprob = ""
    '                    'codigo de sucursal al edittext de code
    '                    'se obtienen valores de codigos
    '                    str_CodSucursal = oComboSucursal.Especifico.Value
    '                    str_CodNivAprob = oComboNiveles.Especifico.Value
    '                    If Not oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
    '                        If Not String.IsNullOrEmpty(str_CodSucursal) Then
    '                            'carga datasource de msj
    '                            Call CargaDataSourceMSJ(str_CodSucursal)
    '                            'se aplican filtros sobre la matriz de mensajeria
    '                            Call CargaLineas(str_CodSucursal, str_CodNivAprob, FormUID)
    '                        End If
    '                    End If
    '            End Select
    '        End If
    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
    '    End Try
    'End Sub

    'Public Sub ManejadorEventoMenuBuscar(ByVal pval As SAPbouiCOM.MenuEvent, ByVal oForm As SAPbouiCOM.Form)
    '    Try
    '        'limpia el combo de niveles 
    '        oComboSucursal.AsignaValorDataSource("")
    '        'habilita sucursales
    '        oForm.Items.Item("cboSucu").Enabled = True
    '        'habilita btn 1
    '        oForm.Items.Item("1").Enabled = True
    '        'limpia el combo de niveles 
    '        oComboNiveles.AsignaValorUserDataSource("")
    '    Catch ex As Exception
    '        Utilitarios.ManejadorErrores(ex, m_SBO_Application)
    '    End Try
    'End Sub

    'Public Sub ManejoEventoGotFocus(ByVal oForm As SAPbouiCOM.Form, ByVal pval As SAPbouiCOM.ItemEvent)

    '    Dim strSucu As String = ""

    '    Try
    '        Select Case pval.ItemUID
    '            'Case ""
    '            '    If oTxtSucursal IsNot Nothing Then

    '            '        strSucu = oTxtSucursal.Especifico.Value
    '            '        If Not String.IsNullOrEmpty(strSucu) Then
    '            '            oTxtSucursalNV.AsignaValorUserDataSource(Utilitarios.EjecutarConsulta(String.Format("SELECT Name FROM OUBR WHERE Code = '{0}'", strSucu), _
    '            '                                                                                                     m_oCompany.CompanyDB, m_oCompany.Server))
    '            '        End If
    '            '    End If

    '        End Select
    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
    '    End Try
    'End Sub

#End Region

End Class
