Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports SAPbobsCOM
Imports SAPbouiCOM

Partial Public Class ConfiguracionMensajeriaDMS
    Implements IUsaPermisos

#Region "Declaraciones"

    'variables
    Public Shared _int_IndicesAEliminar As New List(Of Integer)
    Public Shared _int_IndicesAEliminarTB As New List(Of Integer)
    Private m_oUsuariosXRol As ListaEmpleadosSeleccion
    Private _g_blnCambios As Boolean

#End Region

#Region "Propiedades"

    Public Property g_blnCambios As Boolean
        Get
            Return _g_blnCambios
        End Get
        Set(ByVal value As Boolean)
            _g_blnCambios = value
        End Set
    End Property

#End Region

#Region "Metodos"

    'Carga las sucursales en el combo por medio de GeneralServices
    Public Sub CargaSucursales()
        Try
            Call CargarValidValuesEnCombos(FormularioSBO, "select Code, Name from [@SCGD_SUCURSALES] with (nolock)", "cboSucu")
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    'Carga las sucursales en el combo por medio de GeneralServices
    Public Sub CargaRoles()
        Try
            Call CargarValidValuesEnCombos(FormularioSBO, "select Code, Name from [@SCGD_ROL_MSJ] with (nolock)", "cboRID")
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    Public Function EliminaUsuarioMensajeria(ByVal strSucu As String, ByVal strIdRol As String, ByVal FormUID As String) As Boolean

        'datos leidos de matriz
        Dim strUsuario_lc As String = ""
        Dim strName_lc As String = ""
        Dim strCSucu_lc As String = ""
        Dim strEmpID_lc As String = ""
        Dim strUserName_lc As String = ""
        Dim strRolID_lc As String = ""
        Dim YaExiste As Boolean = False

        'objetos general services
        Dim oform As SAPbouiCOM.Form
        Dim txtDocEntry As SAPbouiCOM.EditText
        Dim cboRol As SAPbouiCOM.ComboBox
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChild As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim oChildGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim iChildActualizar As Integer
        Dim contador = 0
        Try
            oform = m_oApplication.Forms.Item(FormUID)
            txtDocEntry = DirectCast(oform.Items.Item("txtDE").Specific, SAPbouiCOM.EditText)
            cboRol = DirectCast(oform.Items.Item("cboRID").Specific, SAPbouiCOM.ComboBox)
            g_oMtxMSJ = DirectCast(oform.Items.Item(strMatrizMSJ).Specific, Matrix)

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CMSJ")

            If Not String.IsNullOrEmpty(txtDocEntry.Value.Trim()) Then
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", txtDocEntry.Value.Trim())
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                dtUsuariosBD.ExecuteQuery(String.Format(" SELECT U_IDRol, U_IDUSR, U_Usr_Name, U_EmpCode, U_Usr_UsrName FROM [@SCGD_CONF_MSJLN] where DocEntry = '{0}' ", txtDocEntry.Value.Trim()))
                g_oMtxMSJ.FlushToDataSource()
                Dim rowsDTConf As List(Of Integer) = New List(Of Integer)

                For i As Integer = dtConfigUsrRol.Rows.Count To 1 Step -1
                    If g_oMtxMSJ.IsRowSelected(i) Then
                        rowsDTConf.Add(i)
                    Else
                        contador = contador + 1
                    End If

                Next
                If contador >= g_oMtxMSJ.RowCount Then
                    m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrSelectUserDelete, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                    Return False
                Else
                    For Each i As Integer In rowsDTConf
                        dtConfigUsrRol.Rows.Remove(i - 1)
                    Next
                    g_oMtxMSJ.LoadFromDataSource()
                    Return True
                End If
            End If

        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            _int_IndicesAEliminar.Clear()
            _int_IndicesAEliminarTB.Clear()
            Utilitarios.ManejadorErrores(ex, m_oApplication)
            Return False
        End Try

    End Function

    Public Sub ActualizaTablaMSJS(ByVal strSucu As String, ByVal strIdRol As String, ByVal FormUID As String)

        'datos leidos de matriz
        Dim strUsuario_lc As String = ""
        Dim strName_lc As String = ""
        Dim strCSucu_lc As String = ""
        Dim strEmpID_lc As String = ""
        Dim strUserName_lc As String = ""
        Dim strRolID_lc As String = ""

        Dim iContador As Integer = 0
        Dim YaExiste As Boolean = False

        'objetos general services
        Dim oform As SAPbouiCOM.Form
        Dim txtDocEntry As SAPbouiCOM.EditText
        Dim cboRol As SAPbouiCOM.ComboBox
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChild As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim iChildActualizar As Integer
        Dim existeReg As Boolean
        Dim deleteChilds As List(Of Integer) = New List(Of Integer)
        Try
            oform = m_oApplication.Forms.Item(FormUID)
            txtDocEntry = DirectCast(oform.Items.Item("txtDE").Specific, SAPbouiCOM.EditText)
            cboRol = DirectCast(oform.Items.Item("cboRID").Specific, SAPbouiCOM.ComboBox)

            g_oMtxMSJ = DirectCast(oform.Items.Item(strMatrizMSJ).Specific, Matrix)

            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_CMSJ")

            If Not String.IsNullOrEmpty(txtDocEntry.Value.Trim()) Then
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", txtDocEntry.Value.Trim())
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                dtUsuariosBD.ExecuteQuery(String.Format(" SELECT U_IDRol, U_IDUSR, U_Usr_Name, U_EmpCode, U_Usr_UsrName FROM [@SCGD_CONF_MSJLN] where DocEntry = '{0}' ", txtDocEntry.Value.Trim()))
                g_oMtxMSJ.FlushToDataSource()

                For iUbicacion As Integer = 0 To dtUsuariosBD.Rows.Count - 1
                    existeReg = False
                    For i As Integer = 1 To dtConfigUsrRol.Rows.Count
                        strUsuario_lc = dtConfigUsrRol.GetValue("UsrID", i - 1)
                        strName_lc = dtConfigUsrRol.GetValue("Name", i - 1)
                        strEmpID_lc = dtConfigUsrRol.GetValue("EmpId", i - 1)

                        If strUsuario_lc.Trim = dtUsuariosBD.GetValue("U_IDUSR", iUbicacion) And
                            strName_lc.Trim = dtUsuariosBD.GetValue("U_Usr_Name", iUbicacion) And
                            strEmpID_lc.Trim = dtUsuariosBD.GetValue("U_EmpCode", iUbicacion) Then
                            existeReg = True
                            Exit For
                        End If
                    Next
                    If Not existeReg Then
                        deleteChilds.Add(iUbicacion)
                    End If
                Next

                For i As Integer = 1 To dtConfigUsrRol.Rows.Count
                    strUsuario_lc = dtConfigUsrRol.GetValue("UsrID", i - 1)
                    strName_lc = dtConfigUsrRol.GetValue("Name", i - 1)
                    strEmpID_lc = dtConfigUsrRol.GetValue("EmpId", i - 1)
                    strRolID_lc = cboRol.Selected.Value ' dtConfigUsrRol.GetValue("RolId", i - 1)
                    strUserName_lc = dtConfigUsrRol.GetValue("UserName", i - 1)

                    YaExiste = False
                    For iUbicacion As Integer = 0 To dtUsuariosBD.Rows.Count - 1
                        If strUsuario_lc.Trim = dtUsuariosBD.GetValue("U_IDUSR", iUbicacion) And
                            strName_lc.Trim = dtUsuariosBD.GetValue("U_Usr_Name", iUbicacion) And
                            strEmpID_lc.Trim = dtUsuariosBD.GetValue("U_EmpCode", iUbicacion) Then
                            YaExiste = True
                            iChildActualizar = iUbicacion
                            Exit For
                        End If
                    Next

                    oChildren = oGeneralData.Child("SCGD_CONF_MSJLN")
                    If Not YaExiste Then
                        If Not String.IsNullOrEmpty(strUsuario_lc) _
                            AndAlso Not String.IsNullOrEmpty(strEmpID_lc) AndAlso Not String.IsNullOrEmpty(strRolID_lc) Then

                            oChild = oChildren.Add

                            oChild.SetProperty("U_IDRol", strRolID_lc)
                            oChild.SetProperty("U_IDUSR", strUsuario_lc)
                            oChild.SetProperty("U_Usr_Name", strName_lc)
                            oChild.SetProperty("U_EmpCode", strEmpID_lc)
                            oChild.SetProperty("U_Usr_UsrName", strUserName_lc)
                        End If
                    ElseIf YaExiste Then
                        If Not String.IsNullOrEmpty(strUsuario_lc) _
                          AndAlso Not String.IsNullOrEmpty(strEmpID_lc) AndAlso Not String.IsNullOrEmpty(strRolID_lc) Then

                            oChild = oChildren.Item(iChildActualizar)

                            oChild.SetProperty("U_IDRol", strRolID_lc)
                            oChild.SetProperty("U_IDUSR", strUsuario_lc)
                            oChild.SetProperty("U_Usr_Name", strName_lc)
                            oChild.SetProperty("U_EmpCode", strEmpID_lc)
                            oChild.SetProperty("U_Usr_UsrName", strUserName_lc)
                        End If
                    End If
                Next
                Dim conta = oChildren.Count

                For i As Integer = deleteChilds.Count To 1 Step -1
                    oChildren.Remove(deleteChilds(i - 1))
                Next

                oGeneralService.Update(oGeneralData)
                Call CargaLineasXSucursal(strSucu, strIdRol, FormUID)
            Else
                oGeneralData = Nothing
                oGeneralData = DirectCast(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData), GeneralData)
                oGeneralData.SetProperty("U_IdSuc", oComboSucursal.Especifico.Value)
                oGeneralData.SetProperty("U_IdRol", oComboRoles.Especifico.Value)

                oChildren = oGeneralData.Child("SCGD_CONF_MSJLN")

                For i As Integer = 1 To dtConfigUsrRol.Rows.Count
                    g_oMtxMSJ.FlushToDataSource()

                    strUsuario_lc = dtConfigUsrRol.GetValue("UsrID", i - 1)
                    strName_lc = dtConfigUsrRol.GetValue("Name", i - 1)
                    strEmpID_lc = dtConfigUsrRol.GetValue("EmpId", i - 1)
                    strRolID_lc = oComboRoles.Especifico.Value 'dtConfigUsrRol.GetValue("RolId", i - 1)
                    strUserName_lc = dtConfigUsrRol.GetValue("UserName", i - 1)

                    If Not String.IsNullOrEmpty(strUsuario_lc) _
                          AndAlso Not String.IsNullOrEmpty(strEmpID_lc) AndAlso Not String.IsNullOrEmpty(strRolID_lc) Then
                        oChild = oChildren.Add
                        oChild.SetProperty("U_IDRol", strRolID_lc)
                        oChild.SetProperty("U_IDUSR", strUsuario_lc)
                        oChild.SetProperty("U_Usr_Name", strName_lc)
                        oChild.SetProperty("U_EmpCode", strEmpID_lc)
                        oChild.SetProperty("U_Usr_UsrName", strUserName_lc)
                    End If
                Next

                m_oCompany.StartTransaction()
                oGeneralService.Add(oGeneralData)
                m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                CargaLineasXSucursal(strSucu, strIdRol, FormUID)


            End If


        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            _int_IndicesAEliminar.Clear()
            _int_IndicesAEliminarTB.Clear()
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    Public Sub BuscarSucursal(ByVal FormUID As String)

        Dim strConsulta As String = "select DocEntry, U_IdSuc, U_IdRol from [@SCGD_CONF_MSJ] where U_IdSuc = '{0}'"

        Dim strNumeroOT As String = String.Empty
        Dim oForm As Form
        Dim strCodigo As String = String.Empty
        Dim strUsuario As String = String.Empty
        Dim strSucursalUsuario As String = String.Empty

        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)

            dtBusqueda = oForm.DataSources.DataTables.Item(g_strdtBusqueda)

            If Not String.IsNullOrEmpty(strNumeroOT) Then
                strConsulta = String.Format(strConsulta, strSucursalUsuario)

                dtBusqueda.Rows.Clear()
                dtBusqueda.ExecuteQuery(strConsulta)

                If Not String.IsNullOrEmpty(dtBusqueda.GetValue("U_CSucu", 0).ToString()) Then

                    udsConfiguracionMSJ.Item("Sucu").Value = dtBusqueda.GetValue("U_IdSuc", 0).ToString()
                    udsConfiguracionMSJ.Item("IdRol").Value = dtBusqueda.GetValue("U_IdRol", 0).ToString()

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Sub CargaLineasXSucursal(ByVal str_CodSucursal As String, ByVal str_IdRol As String, ByVal FormUID As String)
        Dim txtDocEntry As SAPbouiCOM.EditText

        Dim m_strConsulta As String =
            "  select ln.DocEntry, ln.LineId, ln.U_IDRol, ln.U_IDUSR, ln.U_Usr_Name, ln.U_EmpCode, ln.U_Usr_UsrName " & _
               "from [@SCGD_CONF_MSJLN] ln with (nolock)  " & _
               "inner join [@SCGD_CONF_MSJ] p with (nolock) on ln.Docentry=p.DocEntry " & _
               "where p.U_IdSuc = '{0}' and p.U_IdRol = '{1}' "

        Dim queryDocEntry = "select DocEntry from [@SCGD_CONF_MSJ] where U_IdRol='{1}' and U_IdSuc='{0}'"

        Try

            If Not String.IsNullOrEmpty(str_CodSucursal) AndAlso Not String.IsNullOrEmpty(str_IdRol) Then

                m_strConsulta = String.Format(m_strConsulta, str_CodSucursal, str_IdRol)
                queryDocEntry = String.Format(queryDocEntry, str_CodSucursal, str_IdRol)

                dtBusqueda.Rows.Clear()
                dtBusqueda.ExecuteQuery(m_strConsulta)
                dtConfigUsrRol.Rows.Clear()

                g_oMtxMSJ = DirectCast(FormularioSBO.Items.Item(strMatrizMSJ).Specific, Matrix)
                txtDocEntry = DirectCast(FormularioSBO.Items.Item("txtDE").Specific, SAPbouiCOM.EditText)

                For i As Integer = 0 To dtBusqueda.Rows.Count - 1
                    dtConfigUsrRol.Rows.Add(1)
                    dtConfigUsrRol.SetValue("UsrID", i, dtBusqueda.GetValue("U_IDUSR", i))
                    dtConfigUsrRol.SetValue("Name", i, dtBusqueda.GetValue("U_Usr_Name", i))
                    dtConfigUsrRol.SetValue("EmpId", i, dtBusqueda.GetValue("U_EmpCode", i))
                    dtConfigUsrRol.SetValue("DocEntry", i, dtBusqueda.GetValue("DocEntry", i))
                    dtConfigUsrRol.SetValue("LineId", i, dtBusqueda.GetValue("LineId", i))
                    dtConfigUsrRol.SetValue("RolId", i, dtBusqueda.GetValue("U_IDRol", i))
                    dtConfigUsrRol.SetValue("UserName", i, dtBusqueda.GetValue("U_Usr_UsrName", i))
                Next
                If dtBusqueda.Rows.Count = 1 AndAlso Not String.IsNullOrEmpty(dtBusqueda.GetValue("U_Usr_UsrName", 0)) Then
                    dtConfigUsrRol.Rows.Add(1)
                End If

                dtBusqueda.Rows.Clear()
                dtBusqueda.ExecuteQuery(queryDocEntry)

                g_oMtxMSJ.LoadFromDataSource()
                If dtBusqueda.GetValue("DocEntry", 0) <> "0" Then
                    txtDocEntry.Value = dtBusqueda.GetValue("DocEntry", 0)
                Else
                    txtDocEntry.Value = String.Empty
                End If
            Else
                g_oMtxMSJ.Clear()

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el formulario de Seleccion de empleados
    ''' </summary>
    Private Sub CargarFormularioSelEmpMsj(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim strPath As String
        Dim oForm As SAPbouiCOM.Form

        Try
            oForm = m_oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            oGestorFormularios = New GestorFormularios(m_oApplication)
            oFormListaEmpSel = New ListaEmpleadosSeleccion(m_oApplication, m_oCompany)
            oFormListaEmpSel.FormType = mc_strFormUnidadesPorNivel
            oFormListaEmpSel.Titulo = My.Resources.Resource.TituloAsigancionMultiple

            strPath = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.XMLFormSeleccionEmpMsj
            oFormListaEmpSel.NombreXml = strPath
            oFormListaEmpSel.FormularioSBO = oGestorFormularios.CargaFormulario(oFormListaEmpSel)
            oFormListaEmpSel.CargarMatriz()
            Dim oMtxConf
            If dtConfigUsrRol.Rows.Count > 0 Then
                oFormListaEmpSel.CargaSucRol(pVal, oComboRoles.Especifico.Value, oComboSucursal.Especifico.Value, dtConfigUsrRol.GetValue("DocEntry", 0).ToString())
            Else
                oFormListaEmpSel.CargaSucRol(pVal, oComboRoles.Especifico.Value, oComboSucursal.Especifico.Value, String.Empty)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

#End Region

#Region "Eventos"

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                    ByVal FormUID As String,
                                    ByRef BubbleEvent As Boolean,
                                    ByVal comp As SAPbobsCOM.Company)

        'obtenemos el form de mensajeria
        FormularioSBO = m_oApplication.Forms.Item(FormUID)
        'verifica el form
        If Not FormularioSBO Is Nothing _
                       AndAlso pval.ActionSuccess Then

            Select Case pval.ItemUID
                Case "btn_MSJAdd"
                    'verifica que se escoja sucursal y Niv Ap
                    If Not String.IsNullOrEmpty(oComboSucursal.Especifico.Value) And
                        Not String.IsNullOrEmpty(oComboRoles.Especifico.Value) Then
                        If Not Utilitarios.ValidarSiFormularioAbierto(mc_strFormUnidadesPorNivel, False, m_oApplication) Then
                            CargarFormularioSelEmpMsj(pval, BubbleEvent)
                        End If
                    Else
                        m_oApplication.StatusBar.SetText(My.Resources.Resource.ErrorUnidadesXSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    End If
                Case "btn_MSJEli"

                    'verifica que se escoja sucursal y Niv Ap
                    If Not String.IsNullOrEmpty(oComboSucursal.Especifico.Value) And
                         Not String.IsNullOrEmpty(oComboRoles.Especifico.Value) Then
                        If EliminaUsuarioMensajeria(oComboSucursal.Especifico.Value, oComboRoles.Especifico.Value, FormUID) Then
                            g_blnCambios = False
                            m_oApplication.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        End If
                    End If


                Case "1281"
                    FormularioSBO.Items.Item("cboSucu").Enabled = True
                Case "btnBus"
                    BuscarSucursal(FormUID)
                Case "btnAct"
                    Dim strSucu As String = ""
                    Dim strRolId As String = ""

                    strSucu = oComboSucursal.Especifico.Value
                    strSucu = strSucu.Trim
                    strRolId = oComboRoles.Especifico.Value.Trim()
                    If Not String.IsNullOrEmpty(strSucu) And
                        Not String.IsNullOrEmpty(strRolId) Then
                        'actualiza el datasource
                        Call ActualizaTablaMSJS(strSucu, strRolId, FormUID)
                        g_blnCambios = False
                        m_oApplication.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        'desactiva cod niv aprob
                        FormularioSBO.Items.Item("cboRID").Enabled = True
                    End If
                Case "mtx_MSJ"
                    g_blnCambios = True
            End Select
        End If
        'verifica el form
        'BEFORE ACTION
        'If Not FormularioSBO Is Nothing _
        '               AndAlso pval.BeforeAction Then
        '    Select Case pval.ItemUID
        '        Case "1"
        '            If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
        '                'actualizo el caption del boton
        '                'Dim oBtn As Button
        '                'oBtn = DirectCast(FormularioSBO.Items.Item("1").Specific, Button)
        '                'oBtn.Caption = My.Resources.Resource.Buscar
        '            End If
        '    End Select

        'End If
    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosCombo(ByRef oTmpForm As SAPbouiCOM.Form, _
                                  ByVal pval As SAPbouiCOM.ItemEvent, _
                                  ByVal FormUID As String, _
                                  ByRef BubbleEvent As Boolean)

        Dim str_CodSucursal As String = ""
        Dim str_CodRol As String = ""
        Dim strConsulta As String = ""

        Try
            FormularioSBO = m_oApplication.Forms.Item(FormUID)
            If pval.ActionSuccess Then
                Select Case pval.ItemUID

                    Case "cboRID"
                        str_CodSucursal = ""
                        str_CodRol = ""

                        str_CodSucursal = oComboSucursal.Especifico.Value
                        str_CodRol = oComboRoles.Especifico.Value
                        If Not String.IsNullOrEmpty(str_CodRol) And
                           Not String.IsNullOrEmpty(str_CodSucursal) Then

                            ManipulaComponentes(True, False, True, True, True, True)
                            Call CargaLineasXSucursal(str_CodSucursal, str_CodRol, FormUID)

                        End If
                    Case "cboSucu"
                        str_CodSucursal = oComboSucursal.Especifico.Value
                        If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE Then
                            If Not String.IsNullOrEmpty(str_CodSucursal) Then
                                g_oMtxMSJ = DirectCast(FormularioSBO.Items.Item(strMatrizMSJ).Specific, Matrix)

                                oComboRoles.AsignaValorUserDataSource(String.Empty)
                                dtConfigUsrRol.Rows.Clear()
                                g_oMtxMSJ.LoadFromDataSource()
                                _int_IndicesAEliminar.Clear()
                                _int_IndicesAEliminarTB.Clear()
                                ManipulaComponentes(True, False, True, False, False, False)

                            End If
                        End If
                End Select
            ElseIf pval.BeforeAction Then
                Select Case pval.ItemUID
                    Case "cboSucu", "cboRID"
                        If g_blnCambios Then
                            If m_oApplication.MessageBox(My.Resources.Resource.PreguntaUsuariosMensajeria,
                                                            1,
                                                            My.Resources.Resource.Si,
                                                            My.Resources.Resource.No) = 2 Then
                                BubbleEvent = False
                            Else
                                g_blnCambios = False
                            End If
                        End If
                End Select
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_oApplication)
        End Try
    End Sub

#End Region

End Class
