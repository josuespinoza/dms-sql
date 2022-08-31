Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager

Public Class ListadoContratoReversadosCls


#Region "Declaraciones"

    Private m_intEstadoFormulario As Integer

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strIdMainMenu As String = "43521"

    Private Const mc_strUIDContratoVentaReversado As String = "SCGD_CTR"
    Private Const mc_strUIDCV_Listado As String = "UIDOCVTra"

    Private Const mc_strSCG_CVENTA As String = "@SCGD_CVENTA"
    Private Const mc_strSlpCode As String = "U_SlpCode"
    Private Const mc_strEstadoCV As String = "U_Estado"

    'Matriz
    Private Const mc_strMTZCotizacion As String = "mtListadoR"

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

    Private m_dbReversados As SAPbouiCOM.DBDataSource

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

        If Utilitarios.MostrarMenu("SCGD_CTR", SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_CTR", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDContratoVentaReversado, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 25, False, True, "SCGD_CTT"))

        End If

    End Sub

    Protected Friend Sub CargaFormularioListadoContRevertidos()

        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim ocolumn As SAPbouiCOM.Column
            Dim linkBtn As SAPbouiCOM.LinkedButton
            '            Dim oButton As SAPbouiCOM.Button
            '            Dim oEdit As SAPbouiCOM.EditText
            '            Dim oGrid As SAPbouiCOM.Grid
            Dim strXMLACargar As String

            Dim oItem As SAPbouiCOM.Item
            Dim oMatriz As SAPbouiCOM.Matrix

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_Revertir"

            strXMLACargar = My.Resources.Resource.ListadoContratosRevertidos

            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            'Agregado 09/11/2010: Conecta udfs de salida y entrada de mercancia con interfaz
            oItem = m_oFormGenCotizacion.Items.Item("mtx_01")
            oMatriz = DirectCast(oItem.Specific, SAPbouiCOM.Matrix)
            oMatriz.Columns.Item("colSalMerc").DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_SCGD_SalMerc")
            oMatriz.Columns.Item("colEntMerc").DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_SCGD_EntMerc")
            oMatriz.Columns.Item("colSaCoVeh").DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_SCGD_SaCoVeh")
            oMatriz.Columns.Item("colAsiAjus").DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_SCGD_AsAj")
            oMatriz.Columns.Item("colAsAjuRv").DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_SCGD_AsAjR")

            Dim strCreaNCparaVehiculoUsado As String = Utilitarios.EjecutarConsulta("Select U_NCSalNeg from [@SCGD_ADMIN] where Code = 'DMS'", m_oCompany.CompanyDB, m_oCompany.Server)

            If strCreaNCparaVehiculoUsado = "Y" Then

                ocolumn = DirectCast(oMatriz.Columns.Item("colNDUsRev"), SAPbouiCOM.Column)
                linkBtn = ocolumn.ExtendedObject

                linkBtn.LinkedObjectType = 14

                ocolumn = DirectCast(oMatriz.Columns.Item("col_RevPri"), SAPbouiCOM.Column)
                linkBtn = ocolumn.ExtendedObject

                linkBtn.LinkedObjectType = 14

            End If

            Dim strConexionDBSucursal As String = ""

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add("@SCGD_CV_REVERLINEA")

            m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item("@SCGD_CV_REVERLINEA")

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto

            If EnlazaColumnasMatrixaDatasource(oMatrix) Then

                Call CargarMatrix(DirectCast(m_oFormGenCotizacion.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix), _
                                            "", _
                                            "", _
                                 m_oFormGenCotizacion, _
                                 m_dbContratos)

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

        'strPath = "D:\Proyectos\Proyecto SCG DMS One\Fuentes\DMS One\SCG.DMSOne.AddonTaller\Formularios\" & strFileName
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
'            Dim oEditNC As SAPbouiCOM.EditText
'            Dim oEditCC As SAPbouiCOM.EditText
'            Dim oEditCN As SAPbouiCOM.EditText

            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                AndAlso pVal.ActionSuccess _
                AndAlso pVal.ItemUID = "btnRefresh" Then


                oMatrix = DirectCast(oForm.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
                'oEditNC = DirectCast(oForm.Items.Item("txtNumC").Specific, SAPbouiCOM.EditText)
                'oEditCC = DirectCast(oForm.Items.Item("txtCardCo").Specific, SAPbouiCOM.EditText)
                'oEditCN = DirectCast(oForm.Items.Item("txtCardNa").Specific, SAPbouiCOM.EditText)

                If Not oMatrix Is Nothing Then

                    'oForm.Items.Item("1").Click()

                    'oEditNC.Value = ""
                    'oEditCC.Value = ""
                    'oEditCN.Value = ""

                    Call CargarMatrix(DirectCast(oForm.Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix), _
                                      "", _
                                      "", _
                                      oForm, _
                                      m_dbContratos)

                    'oMatrix.LoadFromDataSource()






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


'        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions


        Try
            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            'oCondition = oConditions.Add
            'oCondition.BracketOpenNum = 1
            'oCondition.Alias = mc_strEstadoCV
            'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            'oCondition.CondVal = CodEstado
            'oCondition.BracketCloseNum = 1
            'oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

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

            oColumna = oMatrix.Columns.Item("colNumCont")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_NumC")

            oColumna = oMatrix.Columns.Item("colNFact")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_NoFacC")

            oColumna = oMatrix.Columns.Item("colNCFRev")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_NCFRev")

            oColumna = oMatrix.Columns.Item("colNCUs")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_NoCUsC")

            oColumna = oMatrix.Columns.Item("colNDUsRev")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_NDURev")


            oColumna = oMatrix.Columns.Item("colAsEnt")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_EntMeC")

            oColumna = oMatrix.Columns.Item("colAsEnRev")
            oColumna.DataBind.SetBound(True, "@SCGD_CV_REVERLINEA", "U_AsERev")



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

        'oMatriz = DirectCast(SBO_Application.Forms.Item("SCGPR_REQ_").Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
        oMatriz = DirectCast(SBO_Application.Forms.Item("SCGD_Revertir_").Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)
        strIDContrato = oMatriz.Columns.Item("colNumCont").Cells.Item(p_intRow).Specific.String()

        Return strIDContrato

    End Function

    Public Function ValidarEntradas(ByVal intFila As Integer, ByVal strColumna As String) As Boolean

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strValor As String

        Try

            oMatriz = DirectCast(SBO_Application.Forms.Item("SCGD_Revertir_").Items.Item("mtx_01").Specific, SAPbouiCOM.Matrix)

            strValor = oMatriz.Columns.Item(strColumna).Cells.Item(intFila).Specific.Value

            If IsNumeric(strValor) = True Then

                Return False

            Else

                Return True

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Function


    'Protected Friend Sub CargaFormularioReversados()
    '    '*******************************************************************    
    '    'Propósito: Se encarga de establecer los filtros para los eventos de la
    '    '            aplicacion que se van a manejar y posteriormente se los
    '    '            agrega al objeto aplicacion donde se esta almacenando la
    '    '            aplicacion SBO que esta corriendo
    '    '
    '    'Acepta:    Ninguno
    '    'Retorna:   Ninguno
    '    'Desarrollador: Yeiner
    '    'Fecha: 19 Abril 2006
    '    '********************************************************************
    '    Try

    '        Dim strXMLACargar As String
    '        Dim oForm As SAPbouiCOM.Form

    '        Dim fcp As SAPbouiCOM.FormCreationParams

    '        fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
    '        fcp.UniqueID = "SCGPR_REQ_"
    '        fcp.FormType = "SCGPR_REQ"
    '        'fcp.ObjectType = "SCG_REVERSION"

    '        strXMLACargar = "SCGPR_REQForm.xml"
    '        fcp.XmlData = CargarDesdeXML(strXMLACargar)

    '        oForm = SBO_Application.Forms.AddEx(fcp)


    '        Dim strConexionDBSucursal As String = ""

    '        'Call oForm.DataSources.DBDataSources.Add("@SCG_CV_REVERSADOS")
    '        'oForm.DataSources.DBDataSources.Item("@SCG_CV_REVERSADOS")


    '        'm_dbReversados = m_oFormGenCotizacion.DataSources.DBDataSources.Item("@SCG_CV_REVERSADOS")


    '    Catch ex As Exception
    '        SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
    '    End Try
    'End Sub




#End Region


End Class



