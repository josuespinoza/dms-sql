Imports System.Collections.Generic
Imports SCG.SBOFramework
Imports DMSOneFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon

Public Class TipoOtInterna : Implements IFormularioSBO

#Region "... Declaraciones ..."

    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSBO As IApplication

    Private sboItem As Item
    Private sboCombo As ComboBox

    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application
    Public n As NumberFormatInfo
    Public cboTipoOtInterna As ComboBoxSBO
    Private m_oCotizacion As Documents
    Private m_oOrdenVenta As Documents
    Private g_dtConsulta As DataTable
    Private dtLineas As DataTable
    Private blnUsaTallerOTSAP As Boolean
    'constantes
    Private Const strDataTableLineas As String = "tTodosLineas"
    Public Const mc_strMatizOVLines As String = "mtxOTLines"
    Private Const g_strDtConsul As String = "dtConsul"
    Public Const mc_strTipoOtInterna As String = "cboTipOtIn"
    Private Const g_strTipoOT As String = "U_SCGD_Tipo_OT"
    Private Const g_strTipoOTAnt As String = "U_SCGD_AntTipoOT"
    Private Const g_stridSucursal As String = "U_SCGD_idSucursal"
    Private Const mc_strNumFI As String = "U_SCGD_NoFI"

    Private g_oEditNoOT As EditText
    Private g_oChkLines As CheckBox
    Private g_oEditDocEntry As EditText
    Public num_OT As String
    Private g_oMtxOtLines As Matrix
    Private MatrixOVLines As MatrizSolicitaOTEspecial
    Private chkSelTo As CheckBoxSBO
    Private g_dtLocal As DataTable
    Public Const mc_strDataTableDimensionesOT As String = "DimensionesContablesDMSOT"
    Public blnUsaDimensiones As Boolean = False
    Private blnUsaConfiguracionInternaTaller As Boolean = False
    Private strAntTipoOT As String
#End Region

#Region "... Constructor ..."

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, ByVal p_SBOAplication As Application)

        m_oCompany = ocompany
        m_SBO_Application = p_SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "... Inicializacion de Controles ..."

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then

            FormularioSBO.Freeze(True)

            g_dtConsulta = FormularioSBO.DataSources.DataTables.Add(g_strDtConsul)
            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources

            userDS.Add("noOT", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("DeOV", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("selTo", BoDataType.dt_LONG_TEXT, 100)

            chkSelTo = New CheckBoxSBO("chkAll", True, "", "selTo", FormularioSBO)
            chkSelTo.AsignaBinding()

            g_oEditNoOT = DirectCast(FormularioSBO.Items.Item("txtNoOT").Specific, SAPbouiCOM.EditText)
            g_oEditDocEntry = DirectCast(FormularioSBO.Items.Item("txtDeOV").Specific, SAPbouiCOM.EditText)
            g_oEditNoOT.DataBind.SetBound(True, "", "noOT")
            g_oEditDocEntry.DataBind.SetBound(True, "", "DeOV")

            'matriz para todos los repuestos
            dtLineas = FormularioSBO.DataSources.DataTables.Add(strDataTableLineas)
            dtLineas.Columns.Add("col_Sel", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Code", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Name", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Quant", BoFieldsType.ft_Quantity, 100)
            dtLineas.Columns.Add("col_Curr", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Price", BoFieldsType.ft_Price, 100)
            dtLineas.Columns.Add("col_Obs", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_DEnt", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_LNum", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_PrcDes", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_IdRXOr", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Costo", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_IndImp", BoFieldsType.ft_AlphaNumeric, 100)
            dtLineas.Columns.Add("col_Compra", BoFieldsType.ft_AlphaNumeric, 10)
            dtLineas.Columns.Add("col_CPend", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_CSol", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_CRec", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_PenDev", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_PenTra", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_PenBod", BoFieldsType.ft_Quantity)
            dtLineas.Columns.Add("col_IDLine", BoFieldsType.ft_AlphaNumeric)
            dtLineas.Columns.Add("col_TipAr", BoFieldsType.ft_AlphaNumeric)
            dtLineas.Columns.Add("col_IDPaqP", BoFieldsType.ft_AlphaNumeric)
            dtLineas.Columns.Add("col_TreeT", BoFieldsType.ft_AlphaNumeric)

            'crea matriz
            MatrixOVLines = New MatrizSolicitaOTEspecial(mc_strMatizOVLines, FormularioSBO, strDataTableLineas)
            MatrixOVLines.CreaColumnas()
            MatrixOVLines.LigaColumnas()

            MatrixOVLines.Matrix.Columns.Item("col_Sel").Editable = True
            g_dtLocal = FormularioSBO.DataSources.DataTables.Add("local")

            ValidarDataTable(FormularioSBO)


            If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                blnUsaTallerOTSAP = True
            Else
                blnUsaTallerOTSAP = False
            End If

            CargaOT(NoOT, DocEntryOV)
            LoadMatrixLines(blnUsaTallerOTSAP)
            FormatoMatrix(g_oMtxOtLines)
            Dim oItem As SAPbouiCOM.Item
            If FormularioSBO IsNot Nothing Then
                For Each oItem In FormularioSBO.Items
                    If oItem.UniqueID = "chkAll" Then
                        oItem.AffectsFormMode = False
                    End If
                Next
            End If

            FormularioSBO.Freeze(False)
        End If
    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        CargarTiposOtInternas()

        FormularioSBO.Freeze(False)
    End Sub

    ''' <summary>
    ''' Convierte el formato visual de la tabla
    ''' </summary>
    ''' <param name="oMatrix">Objeto matriz donde se van a mostrar los datos</param>
    ''' <remarks></remarks>
    Public Sub FormatoMatrix(ByRef p_Matrix As Matrix)
        Dim tipoItem As EditText
        Dim strTipoItem As String
        Try
            If p_Matrix IsNot Nothing Then
                For index As Integer = 0 To p_Matrix.RowCount - 1
                    tipoItem = DirectCast(p_Matrix.Columns.Item("col_TreeT").Cells.Item(index + 1).Specific, SAPbouiCOM.EditText)
                    If Not String.IsNullOrEmpty(tipoItem.Value.Trim) Then
                        strTipoItem = tipoItem.Value.Trim
                        Select Case strTipoItem
                            Case "I"
                                p_Matrix.CommonSetting.SetRowFontColor(index + 1, 8421504) 'Gris Oscuro
                            Case "S"
                                p_Matrix.CommonSetting.SetRowFontColor(index + 1, 128) 'Rojo Oscuro
                            Case Else
                                p_Matrix.CommonSetting.SetRowFontColor(index + 1, 0) 'Color predeterminado
                        End Select
                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

#Region "... Propiedades ..."

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    'Propiedad Formulario
    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property StrConexion As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Private _strNoOT As String
    Public Property NoOT() As String
        Get
            Return _strNoOT
        End Get
        Set(ByVal value As String)
            _strNoOT = value
        End Set
    End Property

    Private _docEntryOV As String
    Public Property DocEntryOV() As String
        Get
            Return _docEntryOV
        End Get
        Set(ByVal value As String)
            _docEntryOV = value
        End Set
    End Property


#End Region

#Region "... Eventos ..."

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        oForm = m_SBO_Application.Forms.Item(FormUID)


        If pVal.BeforeAction Then

        ElseIf pVal.ActionSuccess Then
            Select Case pVal.ItemUID
                Case "btnGenFI"
                    sboItem = oForm.Items.Item("cboTipOtIn")
                    sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
                    If Not String.IsNullOrEmpty(sboCombo.Value) Then

                        If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                            blnUsaConfiguracionInternaTaller = True
                        Else
                            blnUsaConfiguracionInternaTaller = False
                        End If
                        Dim strUsaDimension As String = Utilitarios.EjecutarConsulta("Select U_UsaDimC from dbo.[@SCGD_ADMIN] ", m_oCompany.CompanyDB, m_oCompany.Server)
                        If strUsaDimension = "Y" Then
                            blnUsaDimensiones = True
                        End If
                        Actualiza_CotizacionOtGeneraFI(oForm, blnUsaDimensiones)
                    Else
                        m_SBO_Application.StatusBar.SetText("Debe seleccionar un tipo de OT Interna", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    End If
                Case "chkAll"
                    oForm.Freeze(True)
                    SeleccionarTodasLineas(oForm)
                    oForm.Freeze(False)
                Case "mtxOTLines"
                    oForm.Freeze(True)
                    SeleccionarTodasLineasDelPaquete(oForm)
                    oForm.Freeze(False)
            End Select
        End If
    End Sub

    Public Sub SeleccionarTodasLineas(ByRef oForm As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim estado As String

        g_oChkLines = DirectCast(oForm.Items.Item("chkAll").Specific, SAPbouiCOM.CheckBox)
        oMatrix = DirectCast(oForm.Items.Item(mc_strMatizOVLines).Specific, SAPbouiCOM.Matrix)
        oMatrix.FlushToDataSource()
        dtLineas = oForm.DataSources.DataTables.Item(strDataTableLineas)
        If g_oChkLines.Checked Then
            estado = "Y"
        Else
            estado = "N"
        End If

        For index As Integer = 0 To dtLineas.Rows.Count - 1
            dtLineas.SetValue("col_Sel", index, estado)
        Next
        oMatrix.LoadFromDataSource()
    End Sub

    Public Sub SeleccionarTodasLineasDelPaquete(ByRef oForm As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strTreeType As String = String.Empty
        Dim strID As String = String.Empty
        Dim strCheck As String = "N"
        Try
            g_oChkLines = DirectCast(oForm.Items.Item("chkAll").Specific, SAPbouiCOM.CheckBox)
            oMatrix = DirectCast(oForm.Items.Item(mc_strMatizOVLines).Specific, SAPbouiCOM.Matrix)
            oMatrix.FlushToDataSource()
            dtLineas = oForm.DataSources.DataTables.Item(strDataTableLineas)

            For index As Integer = 0 To dtLineas.Rows.Count - 1
                strTreeType = dtLineas.GetValue("col_TreeT", index)
                If strTreeType = "S" Then
                    strID = dtLineas.GetValue("col_IDLine", index)
                    strCheck = dtLineas.GetValue("col_Sel", index)
                    For x As Integer = 0 To dtLineas.Rows.Count - 1
                        If strID = dtLineas.GetValue("col_IDPaqP", x) Then
                            dtLineas.SetValue("col_Sel", x, strCheck)
                        End If
                    Next
                End If
            Next
            oMatrix.LoadFromDataSource()
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "... Metodos ..."


    ''' <summary>
    ''' Carga combobox con los tipos de ot internas
    ''' </summary>
    Public Sub CargarTiposOtInternas()
        Try
            sboItem = FormularioSBO.Items.Item("cboTipOtIn")
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "SELECT Code, Name FROM [@SCGD_TIPO_ORDEN] with (nolock) Where U_Interna='Y' ORDER BY Code")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub LoadMatrixLines(ByRef p_blnUsaOTSAP As Boolean)
        Try
            Dim query2 As String = String.Empty
            Dim query As String = String.Empty
            If (dtLineas.Rows.Count = 0) Then
                g_dtLocal = FormularioSBO.DataSources.DataTables.Item("local")
                If Not p_blnUsaOTSAP Then
                    query2 = String.Format("SELECT RDR1.U_SCGD_IdRepxOrd FROM RDR1 with (nolock) INNER JOIN ORDR with (nolock) on RDR1.DocEntry = ORDR.DocEntry " & _
                                           " WHERE ORDR.U_SCGD_Numero_OT is null and ORDR.U_SCGD_No_Visita in " & _
                                           " (SELECT U_SCGD_No_Visita FROM ORDR with (nolock) WHERE ORDR.U_SCGD_Numero_OT = '{0}')", g_oEditNoOT.Value.Trim())

                    query = String.Format("SELECT RDR1.ItemCode, RDR1.Dscription, RDR1.Quantity, RDR1.Currency, RDR1.Price, RDR1.FreeTxt, RDR1.DocEntry, RDR1.LineNum, " & _
                                            " RDR1.DiscPrcnt, RDR1.U_SCGD_IdRepxOrd, RDR1.U_SCGD_Costo, RDR1.TaxCode, RDR1.U_SCGD_CPen, RDR1.U_SCGD_CSol, " & _
                                            " RDR1.U_SCGD_CRec, RDR1.U_SCGD_CPDe, RDR1.U_SCGD_CPTr, RDR1.U_SCGD_CPBo, RDR1.U_SCGD_Compra,  RDR1.U_SCGD_TipArt " & _
                                            " FROM RDR1 with (nolock) WHERE RDR1.DocEntry = '{0}' and RDR1.U_SCGD_Aprobado = 1 and RDR1.LineStatus = 'O' " & _
                                            " and RDR1.U_SCGD_IdRepxOrd not in ({1})", g_oEditDocEntry.Value.Trim(), query2)
                Else
                    query2 = String.Format("SELECT RDR1.U_SCGD_ID FROM RDR1 with (nolock) INNER JOIN ORDR with (nolock) on RDR1.DocEntry = ORDR.DocEntry " & _
                                           " WHERE ORDR.U_SCGD_Numero_OT is null and ORDR.U_SCGD_No_Visita in " & _
                                           " (SELECT U_SCGD_No_Visita FROM ORDR with (nolock) WHERE ORDR.U_SCGD_Numero_OT = '{0}')", g_oEditNoOT.Value.Trim())

                    query = String.Format("SELECT RDR1.ItemCode, RDR1.Dscription, RDR1.Quantity, RDR1.Currency, RDR1.Price, RDR1.FreeTxt, RDR1.DocEntry, RDR1.LineNum, " & _
                                            " RDR1.DiscPrcnt, RDR1.U_SCGD_ID, RDR1.U_SCGD_Costo, RDR1.TaxCode, RDR1.U_SCGD_CPen, RDR1.U_SCGD_CSol, RDR1.U_SCGD_CRec, " & _
                                            " RDR1.U_SCGD_CPDe, RDR1.U_SCGD_CPTr, RDR1.U_SCGD_CPBo, RDR1.U_SCGD_Compra, RDR1.U_SCGD_TipArt ,RDR1.U_SCGD_PaqPadre,RDR1.TreeType" & _
                                            " FROM RDR1 with (nolock)  WHERE RDR1.DocEntry = '{0}' and RDR1.U_SCGD_Aprobado = 1 and RDR1.LineStatus = 'O' " & _
                                            " and RDR1.U_SCGD_ID IS NOT NULL  and RDR1.U_SCGD_ID not in ({1})", g_oEditDocEntry.Value.Trim(), query2)
                End If
                g_dtLocal.ExecuteQuery(query)
                g_oMtxOtLines = DirectCast(FormularioSBO.Items.Item(mc_strMatizOVLines).Specific, SAPbouiCOM.Matrix)

                For i As Integer = 0 To g_dtLocal.Rows.Count - 1
                    If Not String.IsNullOrEmpty(g_dtLocal.GetValue("ItemCode", i).ToString().Trim()) Then
                        dtLineas.Rows.Add(1)

                        dtLineas.SetValue("col_Code", i, g_dtLocal.GetValue("ItemCode", i))
                        dtLineas.SetValue("col_Name", i, g_dtLocal.GetValue("Dscription", i))
                        dtLineas.SetValue("col_Quant", i, g_dtLocal.GetValue("Quantity", i))
                        dtLineas.SetValue("col_Curr", i, g_dtLocal.GetValue("Currency", i))
                        dtLineas.SetValue("col_Price", i, g_dtLocal.GetValue("Price", i))
                        dtLineas.SetValue("col_Obs", i, g_dtLocal.GetValue("FreeTxt", i))
                        dtLineas.SetValue("col_DEnt", i, g_dtLocal.GetValue("DocEntry", i))
                        dtLineas.SetValue("col_LNum", i, g_dtLocal.GetValue("LineNum", i))
                        dtLineas.SetValue("col_PrcDes", i, g_dtLocal.GetValue("DiscPrcnt", i))
                        If Not p_blnUsaOTSAP Then
                            dtLineas.SetValue("col_IdRXOr", i, g_dtLocal.GetValue("U_SCGD_IdRepxOrd", i))
                        Else
                            dtLineas.SetValue("col_IDLine", i, g_dtLocal.GetValue("U_SCGD_ID", i))
                        End If
                        dtLineas.SetValue("col_Costo", i, g_dtLocal.GetValue("U_SCGD_Costo", i))
                        dtLineas.SetValue("col_IndImp", i, g_dtLocal.GetValue("TaxCode", i))
                        dtLineas.SetValue("col_CPend", i, g_dtLocal.GetValue("U_SCGD_CPen", i))
                        dtLineas.SetValue("col_CSol", i, g_dtLocal.GetValue("U_SCGD_CSol", i))
                        dtLineas.SetValue("col_CRec", i, g_dtLocal.GetValue("U_SCGD_CRec", i))
                        dtLineas.SetValue("col_PenDev", i, g_dtLocal.GetValue("U_SCGD_CPDe", i))
                        dtLineas.SetValue("col_PenTra", i, g_dtLocal.GetValue("U_SCGD_CPTr", i))
                        dtLineas.SetValue("col_PenBod", i, g_dtLocal.GetValue("U_SCGD_CPBo", i))
                        dtLineas.SetValue("col_Compra", i, g_dtLocal.GetValue("U_SCGD_Compra", i))
                        dtLineas.SetValue("col_TipAr", i, g_dtLocal.GetValue("U_SCGD_TipArt", i))
                        dtLineas.SetValue("col_IDPaqP", i, g_dtLocal.GetValue("U_SCGD_PaqPadre", i))
                        dtLineas.SetValue("col_TreeT", i, g_dtLocal.GetValue("TreeType", i))
                    End If
                Next
                If dtLineas.Rows.Count > 0 Then
                    g_oMtxOtLines.LoadFromDataSource()
                Else
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTNoLinesAvailable, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    FormularioSBO.Close()
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza la Cotizacion y la OT y si el proceso fue exitoso genera la Factura Interna
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Actualiza_CotizacionOtGeneraFI(ByVal oForm As SAPbouiCOM.Form, Optional p_blnUsaDimension As Boolean = False)

        Try
            Dim query As String
            Dim docEntryCotizacion As String
            Dim docEntryOrdenVenta As String
            Dim strIdSucursal As String
            Dim cotizacionCLS As New CotizacionCLS(m_SBO_Application, m_oCompany)
            Dim m_strDocEntry As String = String.Empty
            Dim message As String
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim strDatabaseTaller As String
            Dim strNumeroOT As String = String.Empty
            Dim intIDSucursal As Integer
            Dim query2 As String = String.Empty
            Dim idLinea As SAPbouiCOM.EditText
            Dim strMensaje As String
            Dim intError As Integer

            g_dtConsulta = oForm.DataSources.DataTables.Item(g_strDtConsul)
            query = String.Format("SELECT DocEntry, DocNum, U_SCGD_idSucursal FROM [OQUT] with(nolock) WHERE U_SCGD_Numero_OT='{0}'",
                                  oForm.DataSources.UserDataSources.Item("noOT").ValueEx)

            g_dtConsulta.ExecuteQuery(query)

            If g_dtConsulta.Rows.Count > 0 Then
                If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                    blnUsaTallerOTSAP = True
                Else
                    blnUsaTallerOTSAP = False
                End If
                docEntryCotizacion = g_dtConsulta.GetValue("DocEntry", 0)
                strIdSucursal = g_dtConsulta.GetValue(g_stridSucursal, 0)

                sboItem = oForm.Items.Item("cboTipOtIn")
                sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

                docEntryOrdenVenta = oForm.DataSources.UserDataSources.Item("DeOV").ValueEx

                m_oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                m_oOrdenVenta = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders), SAPbobsCOM.Documents)

                If m_oCotizacion.GetByKey(docEntryCotizacion) And m_oOrdenVenta.GetByKey(docEntryOrdenVenta) Then

                    oMatrix = DirectCast(oForm.Items.Item(mc_strMatizOVLines).Specific, SAPbouiCOM.Matrix)
                    Dim cantLin As Integer = m_oOrdenVenta.Lines.Count
                    Dim canLinSel = 0
                    Dim chk As SAPbouiCOM.CheckBox

                    For index As Integer = 1 To oMatrix.RowCount
                        chk = DirectCast(oMatrix.Columns.Item("col_Sel").Cells.Item(index).Specific, SAPbouiCOM.CheckBox)
                        If chk.Checked Then
                            canLinSel += 1
                        End If
                    Next

                    If cantLin = canLinSel Then
                        cotizacionCLS.CrearFacturasInternas(m_oOrdenVenta, m_strDocEntry, docEntryCotizacion, m_oCotizacion.DocNum, strIdSucursal, p_blnUsaDimension, oForm, Nothing, sboCombo.Value)
                    Else
                        cotizacionCLS.CrearFacturasInternas(m_oOrdenVenta, m_strDocEntry, docEntryCotizacion, m_oCotizacion.DocNum, strIdSucursal, p_blnUsaDimension, oForm, oMatrix, sboCombo.Value)
                    End If
                End If
                Utilitarios.DestruirObjeto(m_oCotizacion)
                Utilitarios.DestruirObjeto(m_oOrdenVenta)
                oForm.Close()
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub


    ''' <summary>
    ''' Funcion que actualiza la Oferta de Venta que le asigna el nuevo tipo de OT y agrega el anterior
    ''' </summary>
    ''' <param name="p_oCotizacion">Cotizacion a actualizar</param>
    ''' <param name="nuevoTipoOT">Nuevo tipo de OT que se le va a asignar a la Oferta</param>
    ''' <returns> idicador si la transaccion fue o no exitosa</returns>
    Public Function ActualizaCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal nuevoTipoOT As String) As Boolean
        Try
            If Not String.IsNullOrEmpty(nuevoTipoOT) Then
                strAntTipoOT = p_oCotizacion.UserFields.Fields.Item(g_strTipoOT).Value()
                p_oCotizacion.UserFields.Fields.Item(g_strTipoOTAnt).Value() = strAntTipoOT
                p_oCotizacion.UserFields.Fields.Item(g_strTipoOT).Value() = nuevoTipoOT
            Else
                strAntTipoOT = p_oCotizacion.UserFields.Fields.Item(g_strTipoOTAnt).Value()
                If Not String.IsNullOrEmpty(strAntTipoOT) Then
                    p_oCotizacion.UserFields.Fields.Item(g_strTipoOT).Value() = strAntTipoOT
                    p_oCotizacion.UserFields.Fields.Item(g_strTipoOTAnt).Value() = String.Empty
                End If
            End If
            Return True
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Funcion que actualiza la Orden de Venta de SAP que le asigna el nuevo tipo de OT y agrega el anterior
    ''' </summary>
    ''' <param name="p_oOrdenVenta">Orden de Venta a actualizar</param>
    ''' <param name="nuevoTipoOT">Nuevo tipo de OT que se le va a asignar a la Oferta</param>
    ''' <returns> idicador si la transaccion fue o no exitosa</returns>
    Public Function ActualizaOrdenVenta(ByVal p_oOrdenVenta As SAPbobsCOM.Documents, ByVal nuevoTipoOT As String, ByVal oForm As SAPbouiCOM.Form) As Boolean
        Try
            If Not String.IsNullOrEmpty(nuevoTipoOT) Then
                strAntTipoOT = p_oOrdenVenta.UserFields.Fields.Item(g_strTipoOT).Value()
                p_oOrdenVenta.UserFields.Fields.Item(g_strTipoOTAnt).Value() = strAntTipoOT
                p_oOrdenVenta.UserFields.Fields.Item(g_strTipoOT).Value() = nuevoTipoOT
            Else
                strAntTipoOT = m_oOrdenVenta.UserFields.Fields.Item(g_strTipoOTAnt).Value()
                If Not String.IsNullOrEmpty(strAntTipoOT) Then
                    p_oOrdenVenta.UserFields.Fields.Item(g_strTipoOT).Value() = strAntTipoOT
                    p_oOrdenVenta.UserFields.Fields.Item(g_strTipoOTAnt).Value() = String.Empty
                End If
            End If
            'If p_oOrdenVenta.Update() = 0 Then
            '    If m_oCompany.InTransaction Then m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
            'Else
            '    If m_oCompany.InTransaction Then m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)

            'End If
            Return True
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Return False
        End Try
    End Function

    Public Function ActualizaOrdenVenta(ByVal p_oOrdenVenta As SAPbobsCOM.Documents, ByVal strNumeroFI As String) As Boolean
        Dim result As Boolean
        Dim inte As Integer
        Dim stre As String

        result = False
        Try
            m_oCompany.StartTransaction()

            If Not String.IsNullOrEmpty(strNumeroFI) Then
                p_oOrdenVenta.UserFields.Fields.Item(mc_strNumFI).Value() = strNumeroFI

                inte = p_oOrdenVenta.Update()

                If inte = 0 Then
                    result = True
                    If m_oCompany.InTransaction Then m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                Else
                    m_oCompany.GetLastError(inte, stre)
                    If m_oCompany.InTransaction Then m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                End If
            End If
        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
        Return result
    End Function


    ''' <summary>
    ''' Actualiza LA OT en la base da datos del taller
    ''' </summary>
    ''' <param name="numeroOT">nuemro de OT a aculizar</param>
    ''' <param name="nuevoTipoOT">nuevo tipo de OT</param>
    ''' <param name="idSucursal">Id de la sucursal donde esta esta ot</param>
    ''' <returns> indica si el proceso fue exitoso o no</returns>
    ''' <remarks></remarks>
    Public Function ActualizaTBOrden(ByVal numeroOT As String, ByVal nuevoTipoOT As String, ByVal idSucursal As String, ByRef p_oListaOrdenGeneralData As List(Of SAPbobsCOM.GeneralData)) As Boolean
        Dim result As Boolean
        Dim query As String
        Dim strDatabaseTaller As String

        result = False

        Try
            query = String.Format("select U_BDSucursal from [@SCGD_SUCURSALES] suc with(nolock) where suc.Code = '{0}'", idSucursal)
            strDatabaseTaller = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName)
            If Not String.IsNullOrEmpty(strDatabaseTaller) Then

                If Not blnUsaConfiguracionInternaTaller Then

                    query = String.Empty
                    query = String.Format("Update SCGTA_TB_Orden set CodTipoOrden = '{0}' where NoOrden= '{1}'", nuevoTipoOT, numeroOT)

                    Utilitarios.EjecutarConsulta(query, strDatabaseTaller, m_SBO_Application.Company.ServerName)
                    result = True
                Else
                    Dim oCompanyServiceOT As SAPbobsCOM.CompanyService
                    Dim oGeneralServiceOT As SAPbobsCOM.GeneralService
                    Dim oGeneralDataEntrada As SAPbobsCOM.GeneralData
                    Dim oGeneralParamsOT As SAPbobsCOM.GeneralDataParams

                    query = String.Empty
                    query = String.Format("Select Code From [@SCGD_OT] with(nolock) where U_NoOT = '{0}' and U_Sucu = '{1}'", numeroOT, idSucursal)
                    Dim strCode As String = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName)
                    oCompanyServiceOT = m_oCompany.GetCompanyService()
                    oGeneralServiceOT = oCompanyServiceOT.GetGeneralService("SCGD_OT")
                    oGeneralParamsOT = oGeneralServiceOT.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParamsOT.SetProperty("Code", strCode)
                    oGeneralDataEntrada = oGeneralServiceOT.GetByParams(oGeneralParamsOT)
                    oGeneralDataEntrada.SetProperty("U_TipOT", nuevoTipoOT)
                    'oGeneralServiceOT.Update(oGeneralDataEntrada)
                    p_oListaOrdenGeneralData.Add(oGeneralDataEntrada)
                    result = True
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Carga el numero de OT en el Formulario
    ''' </summary>
    Public Sub CargaOT(ByVal numOT As String, ByVal DocEntry As String)

        g_oEditNoOT.Value = numOT
        g_oEditDocEntry.Value = DocEntry
    End Sub

    Public Sub ValidarDataTable(ByRef p_form As Form)

        If Not Utilitarios.ValidaExisteDataTable(p_form, mc_strDataTableDimensionesOT) Then
            p_form.DataSources.DataTables.Add(mc_strDataTableDimensionesOT)
        End If

    End Sub

#End Region

#Region "Metodos Nuevos"
    Public Sub ManejaFacturaInterna()
        Try

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region
End Class
