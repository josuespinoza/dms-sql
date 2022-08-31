Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SAPbouiCOM

Public Class RecosteosCls

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private m_intDocEntry As Integer

    'Matriz
    Private Const mc_strMTZDetalles As String = "mtxList"
    Private Const mc_strGoodReceipts As String = "@SCGD_GOODRECEIVE"

    Private m_dbContratos As SAPbouiCOM.DBDataSource

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private m_intFilaMatrix As Integer = 1

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private Const mc_strEntradas As String = "Good_R"

    Private WithEvents SBO_Application As SAPbouiCOM.Application

    Private Const mc_strUIDCargar As String = "btnRefresh"
    Private Const mc_strUIDCerrar As String = "btnCerrar"

    Private Const mc_strStatus As String = "Status"
    Private Const mc_strMarca As String = "U_Des_Marc"
    Private Const mc_strEstilo As String = "U_Des_Esti"
    Private Const mc_strModelo As String = "U_Des_Mode"
    Private Const mc_strVIN As String = "U_Num_VIN"
    Private Const mc_strAsientoEntrada As String = "U_As_Entr"
    Private Const mc_strUnidad As String = "U_Unidad"
    Private Const mc_strRecepcion As String = "U_DocRecep"
    Private Const mc_strPedido As String = "U_DocPedido"
    Private Const mc_strTipo As String = "U_Tipo"

    Private EditTextRecepcion As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextPedido As SCG.SBOFramework.UI.EditTextSBO

    Dim userDataSources As UserDataSources

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub CargaFormularioListadoGR()

        Try

            Dim oMatriz As SAPbouiCOM.Matrix
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim strXMLACargar As String
            Dim strTipoParaTaller As String



            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.FormType = "SCGD_Recosteo"

            strXMLACargar = My.Resources.Resource.Recosteos
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)
            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strGoodReceipts)

            m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strGoodReceipts)
            oMatriz = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZDetalles).Specific, SAPbouiCOM.Matrix)

            strTipoParaTaller = Utilitarios.EjecutarConsulta("Select U_Inven_V from [@SCGD_ADMIN] where code = 'DMS' ", m_oCompany.CompanyDB, m_oCompany.Server)

            oCombo = DirectCast(m_oFormGenCotizacion.Items.Item("cboTipo").Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, "Select Code,Name From [@SCGD_TIPOVEHICULO] where Code <> '" & strTipoParaTaller.Trim & "' Order by Name")

            If EnlazaColumnasMatrixaDatasource(oMatriz) Then
                Call CargarMatrix(oMatriz, m_oFormGenCotizacion, m_dbContratos)
            End If

            m_oFormGenCotizacion.PaneLevel = 1

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean

        Dim oColumna As SAPbouiCOM.Column

        Try

            oColumna = oMatrix.Columns.Item("V_1")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "DocEntry")

            oColumna = oMatrix.Columns.Item("V_2")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, mc_strUnidad)

            oColumna = oMatrix.Columns.Item("V_4")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_Marca")

            oColumna = oMatrix.Columns.Item("V_3")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_Estilo")

            oColumna = oMatrix.Columns.Item("V_0")
            oColumna.DataBind.SetBound(True, mc_strGoodReceipts, "U_VIN")

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False

        End Try
    End Function

    <System.CLSCompliant(False)> _
    Public Function CargarMatrix(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                     ByVal oform As SAPbouiCOM.Form, _
                                     ByVal dbCotizacion As SAPbouiCOM.DBDataSource) As Boolean


        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim strUnidad As String
        Dim strRecepcion As String
        Dim strNumPedido As String
        Dim strTipo As String
        Dim strConsultaExacta As String = String.Empty

        Dim oItem As SAPbouiCOM.Item
        Dim oCombo As SAPbouiCOM.ComboBox

        Try

            strConsultaExacta = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Busq_exac FROM dbo.[@SCGD_ADMIN] with(nolock)"), m_oCompany.CompanyDB, m_oCompany.Server).Trim

            If String.IsNullOrEmpty(strConsultaExacta) Then
                strConsultaExacta = "N"
            End If

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
            strUnidad = oform.Items.Item("txtUnidad").Specific.String
            strRecepcion = oform.Items.Item("txtDocRec").Specific.String
            strNumPedido = oform.Items.Item("txtDocPed").Specific.String

            oItem = oform.Items.Item("cboTipo")
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            strTipo = oCombo.Value()

            oCondition = oConditions.Add

            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strStatus
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = "O"
            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
            oCondition.BracketCloseNum = 1

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = mc_strAsientoEntrada
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCondition.BracketCloseNum = 1


            If Not String.IsNullOrEmpty(strUnidad) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
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


            If Not String.IsNullOrEmpty(strRecepcion) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strRecepcion
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strRecepcion
                oCondition.BracketCloseNum = 1
            End If

            If Not String.IsNullOrEmpty(strNumPedido) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strPedido
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strNumPedido
                oCondition.BracketCloseNum = 1

            End If

            If Not String.IsNullOrEmpty(strTipo) Then
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strTipo
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strTipo
                oCondition.BracketCloseNum = 1
            End If

            dbCotizacion.Clear()
            dbCotizacion.Query(oConditions)
            oMatrix.Clear()
            oMatrix.LoadFromDataSource()

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

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
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            Dim oMatriz As SAPbouiCOM.Matrix

            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                AndAlso pVal.ActionSuccess _
                AndAlso pVal.ItemUID = mc_strUIDCargar Then

                m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strGoodReceipts)
                oMatriz = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZDetalles).Specific, SAPbouiCOM.Matrix)
                Call CargarMatrix(oMatriz, m_oFormGenCotizacion, m_dbContratos)

            ElseIf Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso pVal.ItemUID = mc_strUIDCerrar Then

                oForm.Close()

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Public Function DevolverDatoGoodReceipt(ByVal p_strFormID As String, Optional ByVal p_intNoFila As Integer = -1) As String

        Dim oForm As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intFila As Integer
        Dim strIDEntrada As String = ""

        oForm = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oForm.Items.Item(mc_strMTZDetalles).Specific, SAPbouiCOM.Matrix)
        If p_intNoFila = -1 Then
            intFila = oMatriz.GetNextSelectedRow()
            If intFila > -1 Then
                strIDEntrada = oMatriz.Columns.Item("V_1").Cells.Item(intFila).Specific.String()
                oMatriz.ClearSelections()
            End If
        Else
            strIDEntrada = oMatriz.Columns.Item("V_1").Cells.Item(p_intNoFila).Specific.String()
        End If
        Return strIDEntrada

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
        Dim strEntrada As String
        oForm = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oForm.Items.Item(mc_strMTZDetalles).Specific, SAPbouiCOM.Matrix)

        intFila = oMatriz.GetNextSelectedRow()
        If intFila > -1 Then
            strEntrada = oMatriz.Columns.Item("V_1").Cells.Item(intFila).Specific.String()
            oMatriz.ClearSelections()

            p_strUnidad = Utilitarios.EjecutarConsulta("Select U_Unidad from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strMarca = Utilitarios.EjecutarConsulta("Select U_Marca from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strEstilo = Utilitarios.EjecutarConsulta("Select U_Estilo from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strModelo = Utilitarios.EjecutarConsulta("Select U_Modelo from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strVIN = Utilitarios.EjecutarConsulta("Select U_VIN from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strIDVehiculo = Utilitarios.EjecutarConsulta("Select U_ID_Vehiculo from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strDocRecepcion = Utilitarios.EjecutarConsulta("Select U_DocRecep from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)
            p_strDocPedido = Utilitarios.EjecutarConsulta("Select U_DocPedido from dbo.[@SCGD_GOODRECEIVE] with (nolock) where DocEntry = " & strEntrada, m_oCompany.CompanyDB, m_oCompany.Server)

        End If

    End Sub

#End Region


End Class
