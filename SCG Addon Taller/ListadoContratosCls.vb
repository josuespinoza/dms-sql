Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager

Public Class ListadoContratosCls

    '#Region "Enums"

    '    Public Enum scgEstadoFormulario
    '        enumTramite = 1
    '        enumPendiente = 2
    '        enumVentas = 3
    '        enumGrenteGeneral = 4
    '        enumFacturacion = 5
    '        enumcanceladas = 0
    '    End Enum

    '#End Region


#Region "Declaraciones"

    Private m_intEstadoFormulario As Integer

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strIdMainMenu As String = "43520"

    Private Const mc_strUIDContratoVenta As String = "SCGD_LST"
    Private Const mc_strUIDCV_Listado As String = "UIDOCVTra"

    Private Const mc_strSCG_CVENTA As String = "@SCGD_CVENTA"
    Private Const mc_strSlpCode As String = "U_SlpCode"
    Private Const mc_strEstadoCV As String = "U_Estado"

    'Matriz
    Private Const mc_strMTZCotizacion As String = "mtListado"

    'Nombres de columnas de matrix
    Private Const mc_strUIDIDContrato As String = "colIDCont"
    Private Const mc_strUIDUnid As String = "colUnid"
    Private Const mc_strUIDMarca As String = "colMarca"
    Private Const mc_strUIDCliente As String = "colCliente"

    'Nombres de los campos de texto
    Private Const mc_strUIDSlpCode As String = "cboVendedo"
    Private Const mc_strUIDEstado As String = "cboEstado"
    Private Const mc_strUIDVendor As String = "lblVendor"

    'Nombres de los botones
    Private Const mc_strUIDActualizar As String = "btnRefresh"
    Private Const mc_strUIDCerrar As String = "btnClose"

    'Nombres de campos del datasource
    Private Const mc_strCardName As String = "U_CardName"
    Private Const mc_strIDContrato As String = "DocNum"
    Private Const mc_strMarca As String = "U_Des_Marc"
    Private Const mc_strUnidad As String = "U_Cod_Unid"

    Private m_dbContratos As SAPbouiCOM.DBDataSource

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private m_intFilaMatrix As Integer = 1

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private WithEvents SBO_Application As SAPbouiCOM.Application

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Propiedades"

    Public WriteOnly Property EstadoFormulario() As Integer
        Set(ByVal value As Integer)
            m_intEstadoFormulario = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu("SCGD_LST", SBO_Application.Company.UserName) Then
            '          
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_LST", SBO_Application.Language)
            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDContratoVenta, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 20, False, True, "SCGD_CTT"))


        End If

    End Sub

    Protected Friend Sub CargaFormularioListadoCV()
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
'            Dim oButton As SAPbouiCOM.Button
'            Dim oEdit As SAPbouiCOM.EditText
'            Dim oGrid As SAPbouiCOM.Grid
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_frmListadoCV"

            strXMLACargar = My.Resources.Resource.ListadoContratos
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Dim strConexionDBSucursal As String = ""

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strSCG_CVENTA)

            m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_CVENTA)

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto

            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select SlpCode, SlpName from OSLP where SlpCode > -1 order by SlpName", "cboVendedo")

            'Carga el combo de estados de la tabla [@SCGD_ADMIN9]
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "SELECT U_Prio, U_Estado FROM [@SCGD_ADMIN9] ORDER BY U_Prio ", "cboEstado")

            If EnlazaColumnasMatrixaDatasource(oMatrix) Then

                Call CargarMatrix(oMatrix, _
                                  DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDSlpCode).Specific, SAPbouiCOM.ComboBox).Selected.Value, _
                                  DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDEstado).Specific, SAPbouiCOM.ComboBox).Selected.Value, _
                                  m_oFormGenCotizacion, m_dbContratos)

                m_oFormGenCotizacion.Visible = True

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                AndAlso pVal.ActionSuccess _
                AndAlso pVal.ItemUID = mc_strUIDActualizar Then


                oMatrix = DirectCast(oForm.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)

                If Not oMatrix Is Nothing Then

                    Call CargarMatrix(DirectCast(oForm.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix), _
                                      DirectCast(oForm.Items.Item(mc_strUIDSlpCode).Specific, SAPbouiCOM.ComboBox).Selected.Value, _
                                      DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDEstado).Specific, SAPbouiCOM.ComboBox).Selected.Value, _
                                      oForm, _
                                      m_dbContratos)

                End If

            ElseIf Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = mc_strUIDCerrar) Then

                Call oForm.Close()

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenOV" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub ActulizarLista()

        Dim oform As SAPbouiCOM.Form

        Try

            oform = SBO_Application.Forms.GetForm("SCGD_frmBuscador_CV", 0)
            If oform IsNot Nothing Then
                oform.Items.Item(mc_strUIDActualizar).Click()
            End If
        Catch ex As Runtime.InteropServices.COMException
            If ex.Message <> "Form - Not found  [66000-9]" Then
                Throw ex
            End If
            'No realiza ninguna acción pués es que en realidad en form no esta abierto
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

    Public Function CargarMatrix(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                 ByVal slpCode As String, _
                                 ByVal CodEstado As String, _
                                 ByVal oform As SAPbouiCOM.Form, _
                                 ByVal dbCotizacion As SAPbouiCOM.DBDataSource) As Boolean


        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions


        Try
            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strEstadoCV
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = CodEstado
            oCondition.BracketCloseNum = 1
            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

            '********************* se agrega para evitar que salgan los contratos revertidos*****************
            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 2
            oCondition.Alias = "U_Reversa"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = "N"
            oCondition.BracketCloseNum = 2
            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

            '************************************************************************************************* 

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 3
            oCondition.Alias = mc_strSlpCode
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = slpCode
            oCondition.BracketCloseNum = 3



            oMatrix.Clear()

            dbCotizacion.Clear()
            dbCotizacion.Query(oConditions)
            oMatrix.LoadFromDataSource()


            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try

    End Function

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean

        Dim oColumna As SAPbouiCOM.Column

        Try

            oColumna = oMatrix.Columns.Item(mc_strUIDIDContrato)
            oColumna.DataBind.SetBound(True, mc_strSCG_CVENTA, mc_strIDContrato)

            oColumna = oMatrix.Columns.Item(mc_strUIDUnid)
            oColumna.DataBind.SetBound(True, mc_strSCG_CVENTA, mc_strUnidad)

            oColumna = oMatrix.Columns.Item(mc_strUIDMarca)
            oColumna.DataBind.SetBound(True, mc_strSCG_CVENTA, mc_strMarca)

            oColumna = oMatrix.Columns.Item(mc_strUIDCliente)
            oColumna.DataBind.SetBound(True, mc_strSCG_CVENTA, mc_strCardName)

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function DevolverIDContrato(ByVal p_intRow As Integer, _
                                        ByVal p_strIDForm As String) As String

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strIDContrato As String

        oMatriz = DirectCast(SBO_Application.Forms.Item(p_strIDForm).Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
        strIDContrato = oMatriz.Columns.Item("colIDCont").Cells.Item(p_intRow).Specific.String()

        Return strIDContrato

    End Function

    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                            ByVal strQuery As String, _
                                                            ByRef strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim strValorASeleccionar As string =  String.Empty

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
                    If String.IsNullOrEmpty(strValorASeleccionar) Then
                        strValorASeleccionar = drdResultadoConsulta.Item(0).ToString.Trim()
                    End If
                    cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop
            If Not String.IsNullOrEmpty(strValorASeleccionar) Then
                cboCombo.Select(strValorASeleccionar)
            End If
            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

#End Region


End Class
