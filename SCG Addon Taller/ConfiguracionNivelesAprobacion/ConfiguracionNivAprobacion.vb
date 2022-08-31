Imports System.Collections.Generic
Imports SCG.SBOFramework.DI
Imports DMSOneFramework
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SCG.DMSOne.Framework

Partial Class ConfiguracionNivAprobacion

#Region "Declaraciones"

    'variables
    Public Shared _int_IndicesAEliminar As New List(Of Integer)
    Public Shared _int_IndicesAEliminarTB As New List(Of Integer)
    Private m_oUnidadesXNivel As UsuariosPorNAprob
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

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim Existe As Boolean = False
        Dim UltimoCode As Integer = 0
        Dim ContIngresos As Integer = 0

        Try

            dtSucursales.ExecuteQuery("SELECT Code, Name FROM OUBR")
            dtSucursalesEnMSJS.ExecuteQuery("SELECT U_CSucu, U_Sucu FROM [@SCGD_MSJS]")

            UltimoCode = dtSucursalesEnMSJS.Rows.Count
            If UltimoCode = 1 And String.IsNullOrEmpty(dtSucursalesEnMSJS.GetValue("U_CSucu", 0)) Then
                UltimoCode = 0
            End If

            If dtSucursales.Rows.Count > UltimoCode Then

                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_MSJ")

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

                    Call CargarValidValuesEnCombos(FormularioSBO, "SELECT U_CSucu, U_Sucu FROM [@SCGD_MSJS]", "cboSucu")
                Else
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.IngresoSucursales1 & ContIngresos & My.Resources.Resource.IngresoSucursales2, _
                                                        BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                End If

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ActualizaTablaMSJS1(ByVal strSucu As String, ByVal strNAp As String, ByVal FormUID As String)

        'datos leidos de matriz
        Dim strUsuario_lc As String = ""
        Dim strName_lc As String = ""
        Dim strCSucu_lc As String = ""
        Dim strCNAp_lc As String = ""
        Dim strCMsj_lc As String = ""
        Dim strCMCV_lc As String = ""
        Dim strCACV_lc As String = ""
        Dim strLineId_lc As String = ""

        Dim iContador As Integer = 0
        Dim YaExiste As Boolean = False

        'objetos general services
        Dim oEdit As SAPbouiCOM.EditText
        Dim oCheck As SAPbouiCOM.CheckBox
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChild As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim iChildActualizar As Integer
        Try
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_MSJ")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", strSucu)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            
            'dtUsuariosBD.ExecuteQuery(String.Format("SELECT U_Usua, U_Name FROM [@SCGD_MSJS1] " & _
            '                                        " WHERE U_CSucu = '{0}' AND U_CNAp = '{1}'", strSucu, strNAp))

            dtUsuariosBD.ExecuteQuery(String.Format(" SELECT U_Usua, U_Name, U_CSucu, U_CNAp, U_ManejaApro FROM [@SCGD_MSJS1] where U_CSucu = '{0}' ", strSucu))

            For i As Integer = 1 To dtConfigLineas.Rows.Count

                MatrizConfigNAprob.Matrix.FlushToDataSource()

                strUsuario_lc = dtConfigLineas.GetValue("usua", i - 1)
                strName_lc = dtConfigLineas.GetValue("name", i - 1)
                strCNAp_lc = dtConfigLineas.GetValue("cnap", i - 1)
                strCSucu_lc = dtConfigLineas.GetValue("csucu", i - 1)
                strCMsj_lc = dtConfigLineas.GetValue("rmsj", i - 1)
                strCMCV_lc = dtConfigLineas.GetValue("mcv", i - 1)
                strCACV_lc = dtConfigLineas.GetValue("acv", i - 1)
                YaExiste = False

                For iUbicacion As Integer = 0 To dtUsuariosBD.Rows.Count - 1
                    If strUsuario_lc.Trim = dtUsuariosBD.GetValue("U_Usua", iUbicacion) And
                        strCSucu_lc.Trim = dtUsuariosBD.GetValue("U_CSucu", iUbicacion) And
                        strCNAp_lc.Trim = dtUsuariosBD.GetValue("U_CNAp", iUbicacion)  Then
                        YaExiste = True
                        iChildActualizar = iUbicacion
                        Exit For
                    End If
                Next

                If Not YaExiste Then
                    If Not String.IsNullOrEmpty(strUsuario_lc) And Not String.IsNullOrEmpty(strName_lc) _
                        And Not String.IsNullOrEmpty(strCNAp_lc) And Not String.IsNullOrEmpty(strCSucu_lc) _
                        And Not String.IsNullOrEmpty(strCMsj_lc) And Not String.IsNullOrEmpty(strCSucu_lc) Then

                        oChildren = oGeneralData.Child("SCGD_MSJS1")

                        oChild = oChildren.Add

                        oChild.SetProperty("U_Usua", strUsuario_lc)
                        oChild.SetProperty("U_Name", strName_lc)
                        oChild.SetProperty("U_CNAp", strCNAp_lc)
                        oChild.SetProperty("U_CSucu", strCSucu_lc)
                        oChild.SetProperty("U_RMsj", strCMsj_lc)
                        oChild.SetProperty("U_MCV", strCMCV_lc)
                        oChild.SetProperty("U_ManejaApro", strCACV_lc)
                    End If
                ElseIf YaExiste Then
                    If Not String.IsNullOrEmpty(strUsuario_lc) And Not String.IsNullOrEmpty(strName_lc) _
                        And Not String.IsNullOrEmpty(strCNAp_lc) And Not String.IsNullOrEmpty(strCSucu_lc) _
                        And Not String.IsNullOrEmpty(strCMsj_lc) And Not String.IsNullOrEmpty(strCSucu_lc) Then

                        oChildren = oGeneralData.Child("SCGD_MSJS1")
                        oChild = oChildren.Item(iChildActualizar)

                        oChild.SetProperty("U_Usua", strUsuario_lc)
                        oChild.SetProperty("U_Name", strName_lc)
                        oChild.SetProperty("U_CNAp", strCNAp_lc)
                        oChild.SetProperty("U_CSucu", strCSucu_lc)
                        oChild.SetProperty("U_RMsj", strCMsj_lc)
                        oChild.SetProperty("U_MCV", strCMCV_lc)
                        oChild.SetProperty("U_ManejaApro", strCACV_lc)
                    End If
                End If
            Next
            oChildren = oGeneralData.Child("SCGD_MSJS1")
            iContador = _int_IndicesAEliminarTB.Count - 1
            For i As Integer = 0 To _int_IndicesAEliminarTB.Count - 1

                oChildren.Remove(_int_IndicesAEliminarTB(iContador))
                iContador = iContador - 1
            Next
            _int_IndicesAEliminar.Clear()
            _int_IndicesAEliminarTB.Clear()
            oGeneralService.Update(oGeneralData)
            Call CargaLineasXSucursal(strSucu, strNAp, FormUID)

        Catch ex As Exception
            _int_IndicesAEliminar.Clear()
            _int_IndicesAEliminarTB.Clear()
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub BuscarSucursal(ByVal FormUID As String)

        Dim strConsulta As String = "select Code, U_CSucu, U_Sucu from [@SCGD_MSJS] where U_CSucu = '{0}'"

        Dim strNumeroOT As String = String.Empty
        Dim oForm As Form
        Dim strCodigo As String = String.Empty
        Dim strUsuario As String = String.Empty
        Dim strSucursalUsuario As String = String.Empty

        Try
            oForm = ApplicationSBO.Forms.Item(FormUID)

            dtBusqueda = oForm.DataSources.DataTables.Item(g_strdtBusqueda)

            If Not String.IsNullOrEmpty(strNumeroOT) Then
                strConsulta = String.Format(strConsulta, strNumeroOT.Trim(), strSucursalUsuario)

                dtBusqueda.Rows.Clear()
                dtBusqueda.ExecuteQuery(strConsulta)

                If Not String.IsNullOrEmpty(dtBusqueda.GetValue("U_CSucu", 0).ToString()) Then

                    udsConfiguracionNAprob.Item("csucu").Value = dtBusqueda.GetValue("U_CSucu", 0).ToString()
                    udsConfiguracionNAprob.Item("cnap").Value = dtBusqueda.GetValue("NAprob", 0).ToString()

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Sub CargaLineasXSucursal(ByVal str_CodSucursal As String, ByVal str_CodNivAprob As String, ByVal FormUID As String)

        Dim m_strConsulta As String =
            " select LineId, U_Usua, U_Name, U_CNAp, U_RMsj, U_MCV, U_CSucu , U_ManejaApro " & _
            " from [@SCGD_MSJS1] where U_CSucu = '{0}' and U_CNAp = '{1}'"

        Try

            If Not String.IsNullOrEmpty(str_CodSucursal) And Not String.IsNullOrEmpty(str_CodNivAprob) Then

                m_strConsulta = String.Format(m_strConsulta, str_CodSucursal, str_CodNivAprob)

                dtBusqueda.Rows.Clear()
                dtBusqueda.ExecuteQuery(m_strConsulta)
                dtConfigLineas.Rows.Clear()

                For i As Integer = 0 To dtBusqueda.Rows.Count - 1
                    dtConfigLineas.Rows.Add(1)
                    dtConfigLineas.SetValue("usua", i, dtBusqueda.GetValue("U_Usua", i))
                    dtConfigLineas.SetValue("name", i, dtBusqueda.GetValue("U_Name", i))
                    dtConfigLineas.SetValue("cnap", i, dtBusqueda.GetValue("U_CNAp", i))
                    dtConfigLineas.SetValue("rmsj", i, dtBusqueda.GetValue("U_RMsj", i))
                    dtConfigLineas.SetValue("mcv", i, dtBusqueda.GetValue("U_MCV", i))
                    dtConfigLineas.SetValue("acv", i, dtBusqueda.GetValue("U_ManejaApro", i))
                    dtConfigLineas.SetValue("csucu", i, dtBusqueda.GetValue("U_CSucu", i))
                    dtConfigLineas.SetValue("lineid", i, dtBusqueda.GetValue("LineId", i))
                Next

                MatrizConfigNAprob.Matrix.LoadFromDataSource()

            Else

                MatrizConfigNAprob.Matrix.Clear()

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
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
        FormularioSBO = m_SBO_Application.Forms.Item(FormUID)

        'verifica el form
        If Not FormularioSBO Is Nothing _
                       AndAlso pval.ActionSuccess Then

            Select Case pval.ItemUID
                Case "btn_MSJAdd"
                    'verifica que se escoja sucursal y Niv Ap
                    If Not String.IsNullOrEmpty(oComboSucursal.Especifico.Value) And
                        Not String.IsNullOrEmpty(oComboNiveles.Especifico.Value) Then
                        If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_UXN", False, m_SBO_Application) Then
                            Dim objUnidades As New UsuariosPorNAprob(m_oCompany, m_SBO_Application)
                            UsuariosPorNAprob.StrSucursal = oComboSucursal.Especifico.Value
                            UsuariosPorNAprob.StrNivelAprobacion = oComboNiveles.Especifico.Value
                            m_oUnidadesXNivel.CargaFormUnidades(FormularioSBO, True, dtConfigLineas)
                            g_blnCambios = True
                        End If
                    Else
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorUnidadesXSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    End If
                Case "btn_MSJEli"
                    'si no se esta creando uno se pone en actualizar
                    If Not FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                        'verifica que se escoja sucursal y Niv Ap
                        If Not String.IsNullOrEmpty(oComboSucursal.Especifico.Value) And
                            Not String.IsNullOrEmpty(oComboNiveles.Especifico.Value) Then
                            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_UXN", False, m_SBO_Application) Then
                                Dim objUnidades As New UsuariosPorNAprob(m_oCompany, m_SBO_Application)
                                'carga propiedades
                                UsuariosPorNAprob.StrSucursal = oComboSucursal.Especifico.Value
                                UsuariosPorNAprob.StrNivelAprobacion = oComboNiveles.Especifico.Value
                                m_oUnidadesXNivel.CargaFormUnidades(FormularioSBO, False, dtConfigLineas)
                                g_blnCambios = True
                            End If
                        Else
                            'dbe ingreasr sucursal y nva
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorUnidadesXSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    End If
                Case "1281"
                    FormularioSBO.Items.Item("cboSucu").Enabled = True
                Case "btnBus"
                    BuscarSucursal(FormUID)
                Case "btnAct"
                    Dim strSucu As String = ""
                    Dim strCNAp As String = ""

                    strSucu = oComboSucursal.Especifico.Value
                    strSucu = strSucu.Trim
                    strCNAp = oComboNiveles.Especifico.Value
                    strCNAp = strCNAp.Trim
                    If Not String.IsNullOrEmpty(strSucu) And
                        Not String.IsNullOrEmpty(strCNAp) Then
                        'actualiza el datasource
                        Call ActualizaTablaMSJS1(strSucu, strCNAp, FormUID)
                        g_blnCambios = False
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                        'desactiva cod niv aprob
                        FormularioSBO.Items.Item("cboNAp").Enabled = True
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
        Dim str_CodNivAprob As String = ""
        Dim strConsulta As String = ""

        Try
            'If pval.BeforeAction Then
            '    'seleccion de item
            '    Select Case pval.ItemUID
            '        'combo de niveles de aprobacion
            '        Case "cboNAp"
            '            'pregunta ante cambios 
            '            If ExistenCambios Then
            '                intPregunta = 0
            '                intPregunta = m_SBO_Application.MessageBox(My.Resources.Resource.PreguntaUsuariosMensajeria, 1, My.Resources.Resource.Si, My.Resources.Resource.No)
            '                'no continuar, cancelar ejecucion 
            '                If intPregunta = 2 Then
            '                    BubbleEvent = False
            '                Else
            '                    ExistenCambios = False
            '                End If
            '            End If
            '    End Select
            'End If
            If pval.ActionSuccess Then
                Select Case pval.ItemUID

                    Case "cboNAp"
                        str_CodSucursal = ""
                        str_CodNivAprob = ""

                        str_CodSucursal = oComboSucursal.Especifico.Value
                        str_CodNivAprob = oComboNiveles.Especifico.Value
                        If Not String.IsNullOrEmpty(str_CodNivAprob) And
                           Not String.IsNullOrEmpty(str_CodSucursal) Then

                            ManipulaComponentes(True, False, True, True, True, True)
                            Call CargaLineasXSucursal(str_CodSucursal, str_CodNivAprob, FormUID)

                        End If
                    Case "cboSucu"
                        str_CodSucursal = oComboSucursal.Especifico.Value
                        If Not FormularioSBO.Mode = BoFormMode.fm_FIND_MODE Then
                            If Not String.IsNullOrEmpty(str_CodSucursal) Then

                                oComboNiveles.AsignaValorUserDataSource(String.Empty)
                                dtConfigLineas.Rows.Clear()
                                MatrizConfigNAprob.Matrix.LoadFromDataSource()
                                _int_IndicesAEliminar.Clear()
                                _int_IndicesAEliminarTB.Clear()
                                ManipulaComponentes(True, False, True, False, False, False)

                            End If
                        End If
                End Select
            ElseIf pval.BeforeAction Then
                Select Case pval.ItemUID
                    Case "cboSucu", "cboNAp"
                        If g_blnCambios Then
                            If m_SBO_Application.MessageBox(My.Resources.Resource.PreguntaUsuariosMensajeria,
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
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoMenuBuscar(ByVal pval As SAPbouiCOM.MenuEvent, ByVal oForm As SAPbouiCOM.Form)
        Try
            ''limpia el combo de niveles 
            'oComboSucursal.AsignaValorDataSource("")
            ''habilita sucursales
            'oForm.Items.Item("cboSucu").Enabled = True
            ''habilita btn 1
            'oForm.Items.Item("1").Enabled = True
            ''limpia el combo de niveles 
            'oComboNiveles.AsignaValorUserDataSource("")
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ManejoEventoGotFocus(ByVal oForm As SAPbouiCOM.Form, ByVal pval As SAPbouiCOM.ItemEvent)

        Try
            Select Case pval.ItemUID

            End Select
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByVal p_oForm As Form, ByRef BubbleEvent As Boolean)
        Try
            ManipulaComponentes(False, False, True, True, True, True)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

#End Region

End Class
