Imports SCG.DMSOne.Framework.MenuManager

Public Class BuscadorContratoVentaCls

#Region "Enums"

    Public Enum scgEstadoFormulario
        enumTramite = 1
        enumPendiente = 2
        enumVentas = 3
        enumGrenteGeneral = 4
        enumFacturacion = 5
        enumcanceladas = 0
    End Enum

#End Region


#Region "Declaraciones"

    Private m_intEstadoFormulario As scgEstadoFormulario
    Private m_blnUsaEmpleado As Boolean

    Private Const mc_strIdMainMenu As String = "43520"


    Private Const mc_strUIDContratoVenta As String = "SCGD_CTT"
    Private Const mc_strUIDCV_Tramite As String = "UIDOCVTra"
    Private Const mc_strUIDCV_Pendiente As String = "UIDOCV_PA"
    Private Const mc_strUIDCV_Ventas As String = "UIDOCV_GV"
    Private Const mc_strUIDCV_General As String = "UIDOCV_GG"
    Private Const mc_strUIDCV_Facturables As String = "UIDOCV_F"

    Private Const mc_strSCG_CVENTA As String = "@SCGD_CVENTA"
    Private Const mc_strSlpCode As String = "U_SlpCode"
    Private Const mc_strEstadoCV As String = "U_Estado"

    'Matriz
    Private Const mc_strMTZCotizacion As String = "mtContrat"

    'Nombres de columnas de matrix
    Private Const mc_strUIDIDContrato As String = "colIDCont"
    Private Const mc_strUIDUnid As String = "colUnid"
    Private Const mc_strUIDMarca As String = "colMarca"
    Private Const mc_strUIDCliente As String = "colCliente"

    'Nombres de los campos de texto
    Private Const mc_strUIDSlpCode As String = "txtSlpCode"
    Private Const mc_strUIDSlpName As String = "txtSlpName"
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
    Private m_oCompany As SAPbobsCOM.Company

    Private blnMenuDeshabilitadoContrato As Boolean = False

#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, _
                    ByVal ocompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = ocompany

    End Sub

#End Region

#Region "Propiedades"

    Public WriteOnly Property EstadoFormulario() As Integer
        Set(ByVal value As Integer)
            m_intEstadoFormulario = value
        End Set
    End Property

    Public WriteOnly Property UsaEmpleado() As Boolean
        Set(ByVal value As Boolean)
            m_blnUsaEmpleado = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems(ByVal p_udoMenusPlanVentas As Generic.Dictionary(Of String, Utilitarios.MenusPlanVentas))

        Dim strEtiquetaMenu As String

        Dim udoMenu As Utilitarios.MenusPlanVentas
        
        For Each udoMenu In p_udoMenusPlanVentas.Values

            If udoMenu.blnUsaMenu AndAlso Utilitarios.MostrarMenu(udoMenu.strCodigo, SBO_Application.Company.UserName) Then
                strEtiquetaMenu = udoMenu.strMenu
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(udoMenu.strCodigo, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 15, False, True, mc_strUIDContratoVenta))
            End If

        Next

    End Sub

    Protected Friend Sub CargaFormularioBusquedaCV()

        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oEdit As SAPbouiCOM.EditText
            Dim strXMLACargar As String
            Dim strCodigoVendedor As String
            Dim strCodSucursal As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_frmBuscador_CV"

            strXMLACargar = My.Resources.Resource.BuscadorContratoVentas
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Dim strConexionDBSucursal As String = ""

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strSCG_CVENTA)

            m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_CVENTA)

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto

            If m_blnUsaEmpleado Then


                m_oFormGenCotizacion.Items.Item(mc_strUIDSlpCode).Visible = True
                m_oFormGenCotizacion.Items.Item(mc_strUIDSlpName).Visible = True
                m_oFormGenCotizacion.Items.Item(mc_strUIDVendor).Visible = True
                m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Top = 30
                m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Height = 100

                oEdit = m_oFormGenCotizacion.Items.Item(mc_strUIDSlpCode).Specific
                strCodigoVendedor = Utilitarios.ObtieneSlpCode(SBO_Application)
                If String.IsNullOrEmpty(strCodigoVendedor) Then
                    strCodigoVendedor = "-1"
                End If
                oEdit.String = strCodigoVendedor

                oEdit = m_oFormGenCotizacion.Items.Item(mc_strUIDSlpName).Specific
                oEdit.String = Utilitarios.ObtieneSlpName(m_oFormGenCotizacion.Items.Item(mc_strUIDSlpCode).Specific.String, SBO_Application)
                'End If
            Else
                m_oFormGenCotizacion.Items.Item(mc_strUIDSlpCode).Visible = False
                m_oFormGenCotizacion.Items.Item(mc_strUIDSlpName).Visible = False
                m_oFormGenCotizacion.Items.Item(mc_strUIDVendor).Visible = False
                m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Top = 12
                m_oFormGenCotizacion.Items.Item(mc_strMTZCotizacion).Height = 118
            End If
            If EnlazaColumnasMatrixaDatasource(oMatrix) Then
                m_dbContratos = m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strSCG_CVENTA)
                Call CargarMatrix(oMatrix, _
                                  DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDSlpCode).Specific, SAPbouiCOM.EditText).String, _
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
                    m_dbContratos = oForm.DataSources.DBDataSources.Item(mc_strSCG_CVENTA)
                    Call CargarMatrix(DirectCast(oForm.Items.Item(mc_strMTZCotizacion).Specific, SAPbouiCOM.Matrix), _
                                      DirectCast(oForm.Items.Item(mc_strUIDSlpCode).Specific, SAPbouiCOM.EditText).String, _
                                      oForm, _
                                      m_dbContratos
                                      )

                End If

            ElseIf Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess _
                    AndAlso (pVal.ItemUID = mc_strUIDCerrar) Then

                Call oForm.Close()

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Function CargarMatrix(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                 ByVal slpCode As String, _
                                 ByVal oform As SAPbouiCOM.Form, _
                                 ByVal dbCotizacion As SAPbouiCOM.DBDataSource) As Boolean


        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim intBracket As Integer = 0
        Dim strUsuario As String = ""

        Try
            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            If m_blnUsaEmpleado AndAlso slpCode <> "" Then
                'oCondition = oConditions.Add

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strEstadoCV
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = m_intEstadoFormulario
                oCondition.BracketCloseNum = 1
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 2
                oCondition.Alias = mc_strSlpCode
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = slpCode
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR


                oCondition = oConditions.Add
                oCondition.Alias = "U_OwrCode"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = slpCode
                oCondition.BracketCloseNum = 2

                intBracket = 3
            Else

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = mc_strEstadoCV
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = m_intEstadoFormulario
                oCondition.BracketCloseNum = 1

                intBracket = 2
            End If

            strUsuario = SBO_Application.Company.UserName
            AgregaCondicionSucursal(oConditions, oCondition, m_intEstadoFormulario, strUsuario, intBracket)


            oMatrix.Clear()

            dbCotizacion.Clear()
            dbCotizacion.Query(oConditions)

            oMatrix.LoadFromDataSource()

            'oMatrix.Columns.Item(mc_strUIDNoCotizacion).DataBind.UnBin


            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try

    End Function


    Private Sub AgregaCondicionSucursal(ByRef oConditions As SAPbouiCOM.Conditions, _
                                        ByRef oCondition As SAPbouiCOM.Condition, _
                                        ByVal strNA As String, _
                                        ByVal strUsuario As String, _
                                        ByVal intBracket As Integer)


        Dim dtSucursales As System.Data.DataTable
        Dim contador As Integer = 0
        Dim strConsulta As String = "Select U_CSucu FROM [@SCGD_MSJS1] WHERE U_Usua = '{0}' and U_CNAp = (SELECT [U_Codigo] FROM [@SCGD_ADMIN9] WHERE u_PRIO = {1} ) and U_MCV = 'Y'"

        dtSucursales = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsulta, strUsuario, strNA), _
                                                             m_oCompany.CompanyDB, _
                                                             m_oCompany.Server)

        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

        If dtSucursales.Rows.Count > 0 Then
            
            For Each row As DataRow In dtSucursales.Rows

                If contador >= 1 Then oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR

                oCondition = oConditions.Add

                If contador = 0 Then oCondition.BracketOpenNum = intBracket

                oCondition.Alias = "U_CSucu"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = row("U_CSucu").ToString()

                contador = contador + 1
                
            Next

            oCondition.BracketCloseNum = intBracket
        Else
            oCondition = oConditions.Add
            oCondition.BracketOpenNum = intBracket
            oCondition.Alias = "U_CSucu"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = "-1"
            oCondition.BracketCloseNum = intBracket
        End If

    End Sub

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

#End Region
    
End Class
