Imports System.Linq
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager

Public Class ListadoContratosSeguroPostVenta


#Region "Declaraciones"

    Private m_intEstadoFormulario As Integer

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strUIDListaR As String = "SCGD_LSP"
    Private Const mc_strSCG_CVENTA As String = "@SCGD_CVENTA"
    Private Const mc_strEstadoCV As String = "U_Estado"

    'Nombres de columnas de matrix
    Private Const mc_strUIDIDContrato As String = "colIDCont"
    Private Const mc_strUIDUnid As String = "colUnid"
    Private Const mc_strUIDMarca As String = "colMarca"
    Private Const mc_strUIDCliente As String = "colCliente"
    Private Const mc_strColReversa As String = "colRever"

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
    Private Const mc_strReversa As String = "U_Reversa"

    Private m_oFormGenCotizacion As SAPbouiCOM.Form
    Private EditTextFechaIni As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextFechaFin As SCG.SBOFramework.UI.EditTextSBO
    Private EditTextNumCot As SCG.SBOFramework.UI.EditTextSBO

    'matriz AsignaMultiple
    Private MatrizListaContratosSPV As MatrizListaContratosSegurosPV
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

        If Utilitarios.MostrarMenu("SCGD_LSP", SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_LSP", SBO_Application.Language)
            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDListaR, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 30, False, True, "SCGD_CTT"))
        End If

    End Sub

    Protected Friend Sub CargaFormularioListadoCV()
        Try
            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim dtContratos As SAPbouiCOM.DataTable
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            strXMLACargar = My.Resources.Resource.ListaContratosSeguroPostVenta
            fcp.XmlData = CargarDesdeXML(strXMLACargar)
            fcp.FormType = "SCGD_CSPV"
            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Add(mc_strSCG_CVENTA)

            'matriz para todos los repuestos
            dtContratos = m_oFormGenCotizacion.DataSources.DataTables.Add("dtContratos")
            dtContratos.Columns.Add("colIDCont", BoFieldsType.ft_AlphaNumeric, 100)
            dtContratos.Columns.Add("colRever", BoFieldsType.ft_AlphaNumeric, 100)
            dtContratos.Columns.Add("colUnid", BoFieldsType.ft_AlphaNumeric, 100)
            dtContratos.Columns.Add("colMarca", BoFieldsType.ft_AlphaNumeric, 100)
            dtContratos.Columns.Add("colCliente", BoFieldsType.ft_AlphaNumeric, 254)

            'crea matriz
            MatrizListaContratosSPV = New MatrizListaContratosSegurosPV("mtListadoR", m_oFormGenCotizacion, "dtContratos")
            MatrizListaContratosSPV.CreaColumnas()
            MatrizListaContratosSPV.LigaColumnas()

            oMatrix = DirectCast(m_oFormGenCotizacion.Items.Item("mtListadoR").Specific, SAPbouiCOM.Matrix)
            oMatrix.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto

            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select SlpCode, SlpName from OSLP where SlpCode > -1 order by SlpName", "cboVendedo", True)
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "SELECT top 1 U_Prio, U_Estado FROM [@SCGD_ADMIN9] ORDER BY  U_Prio Desc", "cboEstado", False)

            m_oFormGenCotizacion.Visible = True
            m_oFormGenCotizacion.DataSources.UserDataSources.Add("FechaIni", BoDataType.dt_DATE)
            m_oFormGenCotizacion.DataSources.UserDataSources.Add("FechaFin", BoDataType.dt_DATE)
            m_oFormGenCotizacion.DataSources.UserDataSources.Add("NumCot", BoDataType.dt_LONG_NUMBER)
            EditTextFechaIni = New SCG.SBOFramework.UI.EditTextSBO("txtFecha", True, "", "FechaIni", m_oFormGenCotizacion)
            EditTextFechaIni.AsignaBinding()
            EditTextFechaFin = New SCG.SBOFramework.UI.EditTextSBO("txtFechaH", True, "", "FechaFin", m_oFormGenCotizacion)
            EditTextFechaFin.AsignaBinding()
            EditTextNumCot = New SCG.SBOFramework.UI.EditTextSBO("txtNumCot", True, "", "NumCot", m_oFormGenCotizacion)
            EditTextNumCot.AsignaBinding()
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

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

    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oForm As SAPbouiCOM.Form

            Dim fechaIniSeleccionada As String
            Dim fechaFinSeleccionada As String
            Dim numCot As String

            Dim fechaIni As DateTime
            Dim fechaFin As DateTime
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing AndAlso pVal.ActionSuccess AndAlso pVal.ItemUID = mc_strUIDActualizar Then
                oMatrix = DirectCast(oForm.Items.Item("mtListadoR").Specific, SAPbouiCOM.Matrix)
                If Not oMatrix Is Nothing Then
                    fechaIniSeleccionada = EditTextFechaIni.ObtieneValorUserDataSource()
                    fechaFinSeleccionada = EditTextFechaFin.ObtieneValorUserDataSource()
                    numCot = EditTextNumCot.ObtieneValorUserDataSource()
                    If Not String.IsNullOrEmpty(fechaIniSeleccionada) Then
                        fechaIni = Date.ParseExact(fechaIniSeleccionada, "yyyyMMdd", Nothing)
                    End If

                    If Not String.IsNullOrEmpty(fechaFinSeleccionada) Then
                        fechaFin = Date.ParseExact(fechaFinSeleccionada, "yyyyMMdd", Nothing)
                    End If

                    Call CargarMatrix(DirectCast(oForm.Items.Item("mtListadoR").Specific, SAPbouiCOM.Matrix), DirectCast(oForm.Items.Item(mc_strUIDSlpCode).Specific, SAPbouiCOM.ComboBox).Selected.Value, _
                                                          DirectCast(m_oFormGenCotizacion.Items.Item(mc_strUIDEstado).Specific, SAPbouiCOM.ComboBox).Selected.Value, oForm, fechaIni, fechaFin, numCot)
                End If
            ElseIf Not oForm Is Nothing AndAlso pVal.BeforeAction AndAlso pVal.ItemUID = mc_strUIDActualizar Then
                fechaIniSeleccionada = EditTextFechaIni.ObtieneValorUserDataSource()
                fechaFinSeleccionada = EditTextFechaFin.ObtieneValorUserDataSource()
                numCot = EditTextNumCot.ObtieneValorUserDataSource()
                If (String.IsNullOrEmpty(numCot)) AndAlso Not String.IsNullOrEmpty(fechaIniSeleccionada) AndAlso Not String.IsNullOrEmpty(fechaFinSeleccionada) Then
                    fechaIni = Date.ParseExact(fechaIniSeleccionada, "yyyyMMdd", Nothing)
                    fechaFin = Date.ParseExact(fechaFinSeleccionada, "yyyyMMdd", Nothing)
                    If fechaIni > fechaFin Then
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrMsgFechIniMay, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                ElseIf (String.IsNullOrEmpty(numCot)) AndAlso String.IsNullOrEmpty(fechaIniSeleccionada) AndAlso String.IsNullOrEmpty(fechaFinSeleccionada) Then
                    BubbleEvent = False
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrMsgSeleccionaFecha, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                End If

            ElseIf Not oForm Is Nothing AndAlso pVal.ActionSuccess AndAlso (pVal.ItemUID = mc_strUIDCerrar) Then
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

    Public Function CargarMatrix(ByRef oMatrix As SAPbouiCOM.Matrix, ByVal slpCode As String, ByVal CodEstado As String, ByVal oform As SAPbouiCOM.Form, ByVal fechaIni As DateTime, ByVal fechaFin As DateTime, ByVal numCot As String) As Boolean
        Dim dtContratosFacturados As SAPbouiCOM.DataTable
        'Dim query As String
        Dim m_strConsulta As String = "select DocNum colIDCont, U_Reversa colRever, U_Cod_Unid colUnid, U_Des_Marc colMarca, U_CardName colCliente from [@SCGD_CVENTA] "

        Try
            m_strConsulta = String.Format("{0} where {1} = '{2}' and (U_SegPV_Upd is null or U_SegPV_Upd = 'N') ", m_strConsulta, mc_strEstadoCV, CodEstado)
            dtContratosFacturados = oform.DataSources.DataTables.Item("dtContratos")

            ''********************* se agrega filtro por fecha*****************
            If Not fechaIni = New DateTime() Then
                m_strConsulta = String.Format("{0} and U_Fec_Fac >= '{1}' ", m_strConsulta, fechaIni.ToString("yyyy-MM-dd"))
            End If

            If Not fechaFin = New DateTime() Then
                m_strConsulta = String.Format("{0} and U_Fec_Fac <= '{1}' ", m_strConsulta, fechaFin.ToString("yyyy-MM-dd"))
            End If

            ''************************************************************************************************* 

            If Not String.IsNullOrEmpty(numCot) Then
                m_strConsulta = String.Format("{0} and DocNum = '{1}' ", m_strConsulta, numCot.Trim())
            End If

            If Not String.IsNullOrEmpty(slpCode) Then
                m_strConsulta = String.Format("{0} and U_SlpCode = '{1}' ", m_strConsulta, slpCode)
            End If
            
            Dim estadoFac as Integer=DMS_Connector.Configuracion.ParamGenAddon.Admin9.Max(Function(x) x.U_Prio)
            m_strConsulta = String.Format("{0} and ((U_AsCom is null or U_AsCom = '') or (U_AsCom is not null and U_MonAs = 0 and U_MoNFi <> 0) and U_Estado = '{1}' ) ", m_strConsulta, estadoFac.ToString())

            oMatrix.Clear()
            dtContratosFacturados.ExecuteQuery(m_strConsulta)
            oMatrix.LoadFromDataSource()
            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Public Function DevolverIDContrato(ByVal p_intRow As Integer, ByVal p_strIDForm As String) As String

        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strIDContrato As String

        oMatriz = DirectCast(SBO_Application.Forms.Item("SCGD_CSPV").Items.Item("mtListadoR").Specific, SAPbouiCOM.Matrix)
        strIDContrato = oMatriz.Columns.Item("colIDCont").Cells.Item(p_intRow).Specific.String()

        Return strIDContrato

    End Function

    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, ByVal strQuery As String, ByRef strIDItem As String, ByVal CampoEnBlaco As Boolean)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim strValorASeleccionar As String = String.Empty
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

            If CampoEnBlaco Then
                cboCombo.ValidValues.Add("", "")
            End If

            ''Agrega los ValidValues
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    If String.IsNullOrEmpty(strValorASeleccionar) AndAlso Not CampoEnBlaco Then
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


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
