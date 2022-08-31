Imports SCG.SBOFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM

Partial Public Class AsignacionMultiple : Implements IFormularioSBO


#Region "... Declaraciones ..."

    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application
    Public n As NumberFormatInfo

    Private g_dtLocal As DataTable

    Public Const ConsultaAsignacionesOTInterna As String = "select distinct cc.Code ID, q1.U_SCGD_Sucur+'-'+Cast(q1.LineNum as varchar)+'-'+cc.Code as IDRepXOrd, itm.U_SCGD_T_Fase NoFase, q1.ItemCode, q1.U_SCGD_DurSt DuracionAprobada, cc.Code NoOrden, q1.Dscription ItemName, " & _
                                                                "cc.U_Estad Estado, fp.Name Descripcion, emp.empID IDEmp, (emp.firstName + ' ' + emp.lastName) as NombreEmp, " & _
                                                                "q1.docEntry as NumCot, q1.LineNum, 0 U_SCGD_DurSt, 0	U_SCGD_TiempoReal, 0 as Price " & _
                                                            "from [@SCGD_CTRLCOL] cc with (nolock) " & _
                                                                "inner Join QUT1 q1 with (nolock) on cc.code = q1.U_SCGD_NoOT and cc.U_IdAct = q1.U_SCGD_ID " & _
                                                                "inner join OITM itm with (nolock) on q1.ItemCode = itm.ItemCode " & _
                                                                "left join [@SCGD_FASEPRODUCCION] fp with (nolock) on itm.U_SCGD_T_Fase = fp.Code " & _
                                                                "inner join OHEM emp with (nolock) on cc. U_Colab = emp.empID " & _
                                                            "WHERE cc.Code = '{0}'	"

    Public Const ConsultaAsignacionesOTExterna As String = "SELECT distinct axo.ID, axo.NoFase, itm.ItemCode, axo.DuracionAprobada, axo.NoOrden, itm.ItemName, axo.Estado, fp.Descripcion, " & _
                                                                "ccol.EmpID as IDEmp, (oh.firstName + ' ' + oh.lastName) as NombreEmp, q1.docEntry as NumCot, q1.LineNum, q1.U_SCGD_DurSt, q1.U_SCGD_TiempoReal, q1.Price " & _
                                                            "FROM [{1}].[dbo].[SCGTA_TB_ActividadesxOrden] axo with (nolock) " & _
                                                                "Inner Join [{1}].[dbo].[SCGTA_VW_OITM] itm with (nolock) " & _
                                                                    "On axo.NoActividad = itm.ItemCode " & _
                                                                "Left Join [{1}].[dbo].[SCGTA_TB_FasesProduccion] fp with (nolock) " & _
                                                                "on axo.NoFase = fp.NoFase " & _
                                                                "Left Join [{1}].[dbo].[SCGTA_VW_OQUT_QUT1] q1 with (nolock) " & _
                                                                    "on axo.NoActividad = q1.ItemCode and axo.NoOrden = q1.U_SCGD_Numero_Ot and axo.ID = q1.U_SCGD_IdRepxOrd  " & _
                                                                "Left Join [{1}].[dbo].[SCGTA_TB_ControlColaborador] ccol with (nolock) " & _
                                                                    "on axo.NoOrden = ccol.NoOrden and axo.ID = ccol.IDActividad " & _
                                                                "left join [{1}].[dbo].[SCGTA_VW_OHEM] oh " & _
                                                                "on ccol.EmpID = oh.empID " & _
                                                            "WHERE axo.NoOrden = '{0}'"


#End Region

#Region "... Constructor ..."

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, ByVal p_SBOAplication As Application)

        m_oCompany = ocompany
        m_SBO_Application = p_SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "... Propiedades ..."

#End Region

#Region "... Inicializacion de Controles ..."

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then

            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources

            userDS.Add("noOT", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("noCot", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("idSuc", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("chkSelAll", BoDataType.dt_SHORT_TEXT, 1)

            g_oEditNoOT = DirectCast(FormularioSBO.Items.Item("txtNoOT").Specific, SAPbouiCOM.EditText)
            g_oEditNoCot = DirectCast(FormularioSBO.Items.Item("txtNoCot").Specific, SAPbouiCOM.EditText)
            g_oEditIdSuc = DirectCast(FormularioSBO.Items.Item("txtIdSuc").Specific, SAPbouiCOM.EditText)

            g_oMtxJobs = DirectCast(FormularioSBO.Items.Item("mtxTareas").Specific, SAPbouiCOM.Matrix)
            g_oChkSelAll = DirectCast(FormularioSBO.Items.Item("chkSelAll").Specific, SAPbouiCOM.CheckBox)

            g_oEditNoOT.DataBind.SetBound(True, "", "noOT")
            g_oEditNoCot.DataBind.SetBound(True, "", "noCot")
            g_oEditIdSuc.DataBind.SetBound(True, "", "idSuc")
            g_oChkSelAll.DataBind.SetBound(True, "", "chkSelAll")

            CargaFormulario()

        End If

    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        'Manejo de formulario
        FormularioSBO.Freeze(True)

        CargarMecanicos()
        'Manejo de formulario
        FormularioSBO.Freeze(False)
    End Sub

    Public Sub CargaMecanicosAsignados(ByVal FormUID As String)

        Dim oForm As SAPbouiCOM.Form = m_SBO_Application.Forms.Item("SCGD_ASM")
        Dim usaTallerSap As Boolean = Utilitarios.ValidarOTInternaConfiguracion(m_oCompany)
        FormCotizacion = m_SBO_Application.Forms.Item(FormUID)

        Dim queryNF As String
        Dim strIdSucursal As String
        Dim boolExiste As Boolean
        Dim query As String
        Dim m_strIdSucursal, m_strNoOT As String

        Dim m_dtMecanicosAsignados As SAPbouiCOM.DataTable

        m_strIdSucursal = CotizacionCLS.IdSucursal
        m_strNoOT = CotizacionCLS.NoOT
        m_dtMecanicosAsignados = FormCotizacion.DataSources.DataTables.Item("MecanicosAsignados")

        If Not String.IsNullOrEmpty(m_strIdSucursal) Then
            If Not String.IsNullOrEmpty(m_strNoOT) Then
                If Not usaTallerSap Then
                    Utilitarios.DevuelveNombreBDTaller(m_SBO_Application, m_strIdSucursal, strIdSucursal)
                    queryNF = ConsultaAsignacionesOTExterna
                    query = String.Format(queryNF, m_strNoOT, strIdSucursal)
                Else
                    queryNF = ConsultaAsignacionesOTInterna
                    query = String.Format(queryNF, m_strNoOT)
                End If

                g_dtLocal = oForm.DataSources.DataTables.Item("local")
                g_dtLocal.ExecuteQuery(query)

                Dim cont As Integer = m_dtMecanicosAsignados.Rows.Count

                For i As Integer = 0 To g_dtLocal.Rows.Count - 1
                    boolExiste = False
                    For y As Integer = 0 To m_dtMecanicosAsignados.Rows.Count - 1
                        If m_dtMecanicosAsignados.GetValue("col_IdRepXOrd", y).ToString().Trim() = g_dtLocal.GetValue("IDRepXOrd", i).ToString().Trim() AndAlso _
                            m_dtMecanicosAsignados.GetValue("col_CodEmp", y).ToString().Trim() = g_dtLocal.GetValue("IDEmp", i).ToString().Trim() Then
                            boolExiste = True
                        End If
                    Next
                    If Not boolExiste AndAlso Not String.IsNullOrEmpty(g_dtLocal.GetValue("ItemCode", i)) Then
                        m_dtMecanicosAsignados.Rows.Add()
                        cont = m_dtMecanicosAsignados.Rows.Count - 1
                        m_dtMecanicosAsignados.SetValue("col_CodAct", cont, g_dtLocal.GetValue("ItemCode", i))
                        m_dtMecanicosAsignados.SetValue("col_CodEmp", cont, g_dtLocal.GetValue("IDEmp", i))
                        m_dtMecanicosAsignados.SetValue("col_LineNum", cont, g_dtLocal.GetValue("LineNum", i))
                        m_dtMecanicosAsignados.SetValue("col_NomEmp", cont, g_dtLocal.GetValue("NombreEmp", i))
                        m_dtMecanicosAsignados.SetValue("col_NoFase", cont, g_dtLocal.GetValue("NoFase", i))
                        m_dtMecanicosAsignados.SetValue("col_IdRepXOrd", cont, g_dtLocal.GetValue("IDRepXOrd", i))
                        m_dtMecanicosAsignados.SetValue("col_NoOrden", cont, g_oEditNoOT.Value.Trim())
                        m_dtMecanicosAsignados.SetValue("col_Estado", cont, g_dtLocal.GetValue("Estado", i))
                        m_dtMecanicosAsignados.SetValue("col_Added", cont, "Y")
                        m_dtMecanicosAsignados.SetValue("col_DurEst", cont, g_dtLocal.GetValue("U_SCGD_DurSt", i))
                        m_dtMecanicosAsignados.SetValue("col_DurRe", cont, g_dtLocal.GetValue("U_SCGD_TiempoReal", i))
                        m_dtMecanicosAsignados.SetValue("col_PrecioSt", cont, g_dtLocal.GetValue("Price", i))
                        m_dtMecanicosAsignados.SetValue("col_DesNoFase", cont, g_dtLocal.GetValue("Descripcion", i))
                    End If

                    'cont = cont + 1
                Next
            End If
        End If
    End Sub

    Public Sub CargaMecanicosAsignados(ByVal FormUID As String, ByVal p_strSucursal As String, ByVal p_strNumeroOT As String)

        Dim oForm As SAPbouiCOM.Form = m_SBO_Application.Forms.Item("SCGD_ASM")
        Dim usaTallerSap As Boolean = Utilitarios.ValidarOTInternaConfiguracion(m_oCompany)
        Dim queryNF As String
        Dim strIdSucursal As String
        Dim boolExiste As Boolean
        Dim query As String
        Dim m_strIdSucursal, m_strNoOT As String
        Dim m_dtMecanicosAsignados As SAPbouiCOM.DataTable

        Try
            FormCotizacion = m_SBO_Application.Forms.Item(FormUID)

            m_strIdSucursal = p_strSucursal
            m_strNoOT = p_strNumeroOT
            m_dtMecanicosAsignados = FormCotizacion.DataSources.DataTables.Item("MecanicosAsignados")

            If Not String.IsNullOrEmpty(m_strIdSucursal) Then
                If Not String.IsNullOrEmpty(m_strNoOT) Then
                    If Not usaTallerSap Then
                        Utilitarios.DevuelveNombreBDTaller(m_SBO_Application, m_strIdSucursal, strIdSucursal)
                        queryNF = ConsultaAsignacionesOTExterna
                        query = String.Format(queryNF, m_strNoOT, strIdSucursal)
                    Else
                        queryNF = ConsultaAsignacionesOTInterna
                        query = String.Format(queryNF, m_strNoOT)
                    End If

                    g_dtLocal = oForm.DataSources.DataTables.Item("local")
                    g_dtLocal.ExecuteQuery(query)

                    Dim cont As Integer = m_dtMecanicosAsignados.Rows.Count

                    For i As Integer = 0 To g_dtLocal.Rows.Count - 1
                        boolExiste = False
                        For y As Integer = 0 To m_dtMecanicosAsignados.Rows.Count - 1
                            If m_dtMecanicosAsignados.GetValue("col_IdRepXOrd", y).ToString().Trim() = g_dtLocal.GetValue("IDRepXOrd", i).ToString().Trim() AndAlso _
                                m_dtMecanicosAsignados.GetValue("col_CodEmp", y).ToString().Trim() = g_dtLocal.GetValue("IDEmp", i).ToString().Trim() Then
                                boolExiste = True
                            End If
                        Next
                        If Not boolExiste AndAlso Not String.IsNullOrEmpty(g_dtLocal.GetValue("ItemCode", i)) Then
                            m_dtMecanicosAsignados.Rows.Add()
                            cont = m_dtMecanicosAsignados.Rows.Count - 1
                            m_dtMecanicosAsignados.SetValue("col_CodAct", cont, g_dtLocal.GetValue("ItemCode", i))
                            m_dtMecanicosAsignados.SetValue("col_CodEmp", cont, g_dtLocal.GetValue("IDEmp", i))
                            m_dtMecanicosAsignados.SetValue("col_LineNum", cont, g_dtLocal.GetValue("LineNum", i))
                            m_dtMecanicosAsignados.SetValue("col_NomEmp", cont, g_dtLocal.GetValue("NombreEmp", i))
                            m_dtMecanicosAsignados.SetValue("col_NoFase", cont, g_dtLocal.GetValue("NoFase", i))
                            m_dtMecanicosAsignados.SetValue("col_IdRepXOrd", cont, g_dtLocal.GetValue("IDRepXOrd", i))
                            m_dtMecanicosAsignados.SetValue("col_NoOrden", cont, g_oEditNoOT.Value.Trim())
                            m_dtMecanicosAsignados.SetValue("col_Estado", cont, g_dtLocal.GetValue("Estado", i))
                            m_dtMecanicosAsignados.SetValue("col_Added", cont, "Y")
                            m_dtMecanicosAsignados.SetValue("col_DurEst", cont, g_dtLocal.GetValue("U_SCGD_DurSt", i))
                            m_dtMecanicosAsignados.SetValue("col_DurRe", cont, g_dtLocal.GetValue("U_SCGD_TiempoReal", i))
                            m_dtMecanicosAsignados.SetValue("col_PrecioSt", cont, g_dtLocal.GetValue("Price", i))
                            m_dtMecanicosAsignados.SetValue("col_DesNoFase", cont, g_dtLocal.GetValue("Descripcion", i))
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    ''' <summary>
    ''' Carga el numero de OT en el Formulario
    ''' </summary>
    Public Sub CargaCOT_OT(ByRef pval As SAPbouiCOM.ItemEvent, ByVal numOT As String, ByVal DocEntry As String, ByVal idSuc As String)
        If (g_oEditNoOT.Value = "") Then
            g_oEditNoOT.Value = numOT
        End If
        If (g_oEditNoCot.Value = "") Then
            g_oEditNoCot.Value = DocEntry
        End If
        If (g_oEditIdSuc.Value = "") Then
            g_oEditIdSuc.Value = idSuc
        End If
    End Sub

    ''' <summary>
    ''' Carga combobox con los tipos de ot internas
    ''' </summary>
    Public Sub CargarMecanicos()
        Dim query As String
        Try
            sboItem = FormularioSBO.Items.Item(mc_strCboColabor)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

            query = String.Format("SELECT empID, firstName + ' '+ lastName as CompleteName FROM OHEM with (nolock) WHERE U_SCGD_T_FAse is not null and Active = 'Y' and (branch = {0} OR U_SCGD_MultiBranch = 'Y') ", IDSucursal)

            If (DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES) Then
                query = String.Format("{0} or BPLId = '{1}' ", query, IDSucursal)
            End If

            query = String.Format("{0}  order by completename ", query)

            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, query)

        Catch ex As Exception
            'Throw
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub LoadMatrixLines(ByVal FormUID As String, ByVal p_strIDSucursal As String, ByVal p_strNumeroOT As String, Optional ByVal idEmpSelected As String = "")

        Dim oForm As SAPbouiCOM.Form
        Dim dtMecAsignados As DataTable
        Dim m_dtConsutla As DataTable
        Dim mtxCot As SAPbouiCOM.Matrix
        Dim strIdSucursal As String
        Dim strNoOT As String
        Dim queryServ As String = String.Empty
        Dim queryEst As String = String.Empty
        Dim resultServ As String
        Dim resultEsta As String
        Dim itemCode As SAPbouiCOM.EditText
        Dim itemName As SAPbouiCOM.EditText
        Dim empName As SAPbouiCOM.EditText
        Dim empCode As SAPbouiCOM.EditText
        Dim lineNum As SAPbouiCOM.EditText
        Dim DurSt As SAPbouiCOM.EditText
        Dim EstAct As SAPbouiCOM.EditText
        Dim newRowNumber As Integer
        Dim m_strConsultaConfig As String
        Dim m_strConsulta As String = "select q.U_SCGD_T_Fase NoFase, fp.Name Descripcion from OITM q with (nolock) left join [@SCGD_FASEPRODUCCION] fp with (nolock) on q.U_SCGD_T_Fase = fp.Code where q.U_SCGD_TipoArticulo =2 and q.itemCode = ('{0}')"
        Dim m_strConsultaEstaso As String = "select Name from [@SCGD_ESTADOS_ACTOT] Where Code = {0}"
        Dim m_UsaAsigUni As String

        Try
            oForm = m_SBO_Application.Forms.Item("SCGD_ASM")
            oForm.Freeze(True)

            strIdSucursal = p_strIDSucursal
            strNoOT = p_strNumeroOT
            dtMecAsignados = FormCotizacion.DataSources.DataTables.Item("MecanicosAsignados")
            g_dtLocal = oForm.DataSources.DataTables.Item("local")
            m_dtConsutla = oForm.DataSources.DataTables.Item("dtConsulta")
            g_oMtxJobs = DirectCast(oForm.Items.Item(mc_strMatrizJobsLines).Specific, Matrix)
            dtAsigMultiple = oForm.DataSources.DataTables.Item(strDataTableLineas)
            g_oMtxJobs.FlushToDataSource()
            mtxCot = DirectCast(FormCotizacion.Items.Item("38").Specific, SAPbouiCOM.Matrix)

            If Not String.IsNullOrEmpty(strIdSucursal) Then

                m_strConsultaConfig = String.Format("select U_AsigUniMec from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", strIdSucursal)
                g_dtLocal.ExecuteQuery(m_strConsultaConfig)
                m_UsaAsigUni = g_dtLocal.GetValue("U_AsigUniMec", 0).ToString().Trim()

                If mtxCot.RowCount > 0 Then
                    dtAsigMultiple.Rows.Clear()

                    For y As Integer = 1 To mtxCot.RowCount - 1
                        itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        itemName = DirectCast(mtxCot.Columns.Item("3").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        empName = DirectCast(mtxCot.Columns.Item("U_SCGD_NombEmpleado").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        empCode = DirectCast(mtxCot.Columns.Item("U_SCGD_EmpAsig").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        lineNum = DirectCast(mtxCot.Columns.Item("0").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        DurSt = DirectCast(mtxCot.Columns.Item("U_SCGD_DurSt").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        EstAct = DirectCast(mtxCot.Columns.Item("U_SCGD_EstAct").Cells.Item(y).Specific, SAPbouiCOM.EditText)

                        queryServ = String.Format(m_strConsulta, itemCode.Value.Trim())
                        If Not String.IsNullOrEmpty(idEmpSelected) Then
                            Dim existeLinea As Boolean = True
                            If dtMecAsignados.Rows.Count > 0 Then
                                For u As Integer = 0 To dtMecAsignados.Rows.Count - 1
                                    Dim strLn = dtMecAsignados.GetValue("col_LineNum", u).ToString().Trim()
                                    Dim lineN = 0
                                    Integer.TryParse(strLn, lineN)
                                    If (lineNum.Value.Trim() <> (lineN + 1).ToString() Or idEmpSelected.Trim() <> dtMecAsignados.GetValue("col_CodEmp", u)) Then
                                        If m_UsaAsigUni = "Y" AndAlso lineNum.Value.Trim() = (lineN + 1).ToString() Then
                                            existeLinea = True
                                            Exit For
                                        ElseIf idEmpSelected.Trim() <> dtMecAsignados.GetValue("col_CodEmp", u) Then
                                            existeLinea = False
                                        End If

                                    Else
                                        existeLinea = True
                                        Exit For
                                    End If
                                Next
                                If Not existeLinea Then
                                    m_dtConsutla.ExecuteQuery(queryServ)
                                    resultServ = m_dtConsutla.GetValue(0, 0)
                                    If Not String.IsNullOrEmpty(resultServ) Then
                                        newRowNumber = dtAsigMultiple.Rows.Count
                                        dtAsigMultiple.Rows.Add(1)
                                        dtAsigMultiple.SetValue("col_code", newRowNumber, itemCode.Value.Trim())
                                        dtAsigMultiple.SetValue("col_desc", newRowNumber, itemName.Value.Trim())
                                        dtAsigMultiple.SetValue("col_IDEmpA", newRowNumber, empCode.Value.Trim())
                                        dtAsigMultiple.SetValue("col_asig", newRowNumber, empName.Value.Trim())
                                        dtAsigMultiple.SetValue("col_LnNum", newRowNumber, lineNum.Value.Trim())
                                        dtAsigMultiple.SetValue("col_idfa", newRowNumber, m_dtConsutla.GetValue(0, 0))
                                        dtAsigMultiple.SetValue("col_desfa", newRowNumber, m_dtConsutla.GetValue(1, 0))
                                    End If
                                End If
                            Else
                                m_dtConsutla.ExecuteQuery(queryServ)
                                resultServ = m_dtConsutla.GetValue(0, 0)
                                If Not String.IsNullOrEmpty(resultServ) Then
                                    newRowNumber = dtAsigMultiple.Rows.Count
                                    dtAsigMultiple.Rows.Add(1)
                                    dtAsigMultiple.SetValue("col_code", newRowNumber, itemCode.Value.Trim())
                                    dtAsigMultiple.SetValue("col_desc", newRowNumber, itemName.Value.Trim())
                                    dtAsigMultiple.SetValue("col_IDEmpA", newRowNumber, empCode.Value.Trim())
                                    dtAsigMultiple.SetValue("col_asig", newRowNumber, empName.Value.Trim())
                                    dtAsigMultiple.SetValue("col_LnNum", newRowNumber, lineNum.Value.Trim())
                                    dtAsigMultiple.SetValue("col_idfa", newRowNumber, m_dtConsutla.GetValue(0, 0))
                                    dtAsigMultiple.SetValue("col_desfa", newRowNumber, m_dtConsutla.GetValue(1, 0))
                                End If
                            End If
                        Else
                            resultEsta = String.Empty
                            If Not String.IsNullOrEmpty(EstAct.Value) Then
                                queryEst = String.Format(m_strConsultaEstaso, EstAct.Value.Trim())
                                m_dtConsutla.ExecuteQuery(queryEst)
                                resultEsta = m_dtConsutla.GetValue(0, 0)
                            End If
                            m_dtConsutla.ExecuteQuery(queryServ)
                            resultServ = m_dtConsutla.GetValue(0, 0)

                            If Not String.IsNullOrEmpty(resultServ) Then
                                newRowNumber = dtAsigMultiple.Rows.Count
                                dtAsigMultiple.Rows.Add(1)
                                dtAsigMultiple.SetValue("col_code", newRowNumber, itemCode.Value.Trim())
                                dtAsigMultiple.SetValue("col_desc", newRowNumber, itemName.Value.Trim())
                                dtAsigMultiple.SetValue("col_IDEmpA", newRowNumber, empCode.Value.Trim())
                                dtAsigMultiple.SetValue("col_asig", newRowNumber, empName.Value.Trim())
                                dtAsigMultiple.SetValue("col_LnNum", newRowNumber, lineNum.Value.Trim())
                                dtAsigMultiple.SetValue("col_idfa", newRowNumber, m_dtConsutla.GetValue(0, 0))
                                dtAsigMultiple.SetValue("col_desfa", newRowNumber, m_dtConsutla.GetValue(1, 0))
                                dtAsigMultiple.SetValue("col_fase", newRowNumber, m_dtConsutla.GetValue(1, 0))
                                dtAsigMultiple.SetValue("col_esta", newRowNumber, resultEsta)
                                dtAsigMultiple.SetValue("col_idac", newRowNumber, EstAct.Value.Trim())

                            End If
                        End If
                    Next

                End If
            End If

            g_oMtxJobs.LoadFromDataSource()

            If dtAsigMultiple.Rows.Count = 0 Or String.IsNullOrEmpty(idEmpSelected) Then
                oForm.Items.Item("btnAsi").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            Else
                oForm.Items.Item("btnAsi").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            End If

            oForm.Items.Item("chkSelAll").Update()
            oForm.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub LoadMatrixLines(ByVal FormUID As String, Optional ByVal idEmpSelected As String = "")

        Dim oForm As SAPbouiCOM.Form
        Dim dtMecAsignados As DataTable
        Dim m_dtConsutla As DataTable
        Dim mtxCot As SAPbouiCOM.Matrix
        Dim strIdSucursal As String
        Dim strNoOT As String
        Dim queryServ As String = String.Empty
        Dim resultServ As String
        Dim itemCode As SAPbouiCOM.EditText
        Dim itemName As SAPbouiCOM.EditText
        Dim empName As SAPbouiCOM.EditText
        Dim empCode As SAPbouiCOM.EditText
        Dim lineNum As SAPbouiCOM.EditText
        Dim DurSt As SAPbouiCOM.EditText
        Dim newRowNumber As Integer
        Dim m_strConsultaConfig As String
        Dim m_strConsulta As String = "select q.U_SCGD_T_Fase NoFase, fp.Name Descripcion from OITM q with (nolock) left join [@SCGD_FASEPRODUCCION] fp with (nolock) on q.U_SCGD_T_Fase = fp.Code where q.U_SCGD_TipoArticulo =2 and q.itemCode = ('{0}')"
        Dim m_UsaAsigUni As String

        Try
            oForm = m_SBO_Application.Forms.Item("SCGD_ASM")
            oForm.Freeze(True)

            strIdSucursal = CotizacionCLS.IdSucursal
            strNoOT = CotizacionCLS.NoOT
            dtMecAsignados = FormCotizacion.DataSources.DataTables.Item("MecanicosAsignados")
            g_dtLocal = oForm.DataSources.DataTables.Item("local")
            m_dtConsutla = oForm.DataSources.DataTables.Item("dtConsulta")
            g_oMtxJobs = DirectCast(oForm.Items.Item(mc_strMatrizJobsLines).Specific, Matrix)
            dtAsigMultiple = oForm.DataSources.DataTables.Item(strDataTableLineas)
            g_oMtxJobs.FlushToDataSource()
            mtxCot = DirectCast(FormCotizacion.Items.Item("38").Specific, SAPbouiCOM.Matrix)

            If Not String.IsNullOrEmpty(strIdSucursal) Then

                m_strConsultaConfig = String.Format("select U_AsigUniMec from [@SCGD_CONF_SUCURSAL] where U_Sucurs = '{0}'", strIdSucursal)
                g_dtLocal.ExecuteQuery(m_strConsultaConfig)
                m_UsaAsigUni = g_dtLocal.GetValue("U_AsigUniMec", 0).ToString().Trim()

                If mtxCot.RowCount > 0 Then
                    dtAsigMultiple.Rows.Clear()

                    For y As Integer = 1 To mtxCot.RowCount - 1
                        itemCode = DirectCast(mtxCot.Columns.Item("1").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        itemName = DirectCast(mtxCot.Columns.Item("3").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        empName = DirectCast(mtxCot.Columns.Item("U_SCGD_NombEmpleado").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        empCode = DirectCast(mtxCot.Columns.Item("U_SCGD_EmpAsig").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        lineNum = DirectCast(mtxCot.Columns.Item("0").Cells.Item(y).Specific, SAPbouiCOM.EditText)
                        DurSt = DirectCast(mtxCot.Columns.Item("U_SCGD_DurSt").Cells.Item(y).Specific, SAPbouiCOM.EditText)

                        queryServ = String.Format(m_strConsulta, itemCode.Value.Trim())
                        If Not String.IsNullOrEmpty(idEmpSelected) Then
                            Dim existeLinea As Boolean = True
                            If dtMecAsignados.Rows.Count > 0 Then
                                For u As Integer = 0 To dtMecAsignados.Rows.Count - 1
                                    Dim strLn = dtMecAsignados.GetValue("col_LineNum", u).ToString().Trim()
                                    Dim lineN = 0
                                    Integer.TryParse(strLn, lineN)
                                    If (lineNum.Value.Trim() <> (lineN + 1).ToString() Or idEmpSelected.Trim() <> dtMecAsignados.GetValue("col_CodEmp", u)) Then
                                        If m_UsaAsigUni = "Y" AndAlso lineNum.Value.Trim() = (lineN + 1).ToString() Then
                                            existeLinea = True
                                            Exit For
                                        ElseIf idEmpSelected.Trim() <> dtMecAsignados.GetValue("col_CodEmp", u) Then
                                            existeLinea = False
                                        End If

                                    Else
                                        existeLinea = True
                                        Exit For
                                    End If
                                Next
                                If Not existeLinea Then
                                    m_dtConsutla.ExecuteQuery(queryServ)
                                    resultServ = m_dtConsutla.GetValue(0, 0)
                                    If Not String.IsNullOrEmpty(resultServ) Then
                                        newRowNumber = dtAsigMultiple.Rows.Count
                                        dtAsigMultiple.Rows.Add(1)
                                        dtAsigMultiple.SetValue("col_code", newRowNumber, itemCode.Value.Trim())
                                        dtAsigMultiple.SetValue("col_desc", newRowNumber, itemName.Value.Trim())
                                        dtAsigMultiple.SetValue("col_IDEmpA", newRowNumber, empCode.Value.Trim())
                                        dtAsigMultiple.SetValue("col_asig", newRowNumber, empName.Value.Trim())
                                        dtAsigMultiple.SetValue("col_LnNum", newRowNumber, lineNum.Value.Trim())
                                        dtAsigMultiple.SetValue("col_idfa", newRowNumber, m_dtConsutla.GetValue(0, 0))
                                        dtAsigMultiple.SetValue("col_desfa", newRowNumber, m_dtConsutla.GetValue(1, 0))
                                    End If
                                End If
                            Else
                                m_dtConsutla.ExecuteQuery(queryServ)
                                resultServ = m_dtConsutla.GetValue(0, 0)
                                If Not String.IsNullOrEmpty(resultServ) Then
                                    newRowNumber = dtAsigMultiple.Rows.Count
                                    dtAsigMultiple.Rows.Add(1)
                                    dtAsigMultiple.SetValue("col_code", newRowNumber, itemCode.Value.Trim())
                                    dtAsigMultiple.SetValue("col_desc", newRowNumber, itemName.Value.Trim())
                                    dtAsigMultiple.SetValue("col_IDEmpA", newRowNumber, empCode.Value.Trim())
                                    dtAsigMultiple.SetValue("col_asig", newRowNumber, empName.Value.Trim())
                                    dtAsigMultiple.SetValue("col_LnNum", newRowNumber, lineNum.Value.Trim())
                                    dtAsigMultiple.SetValue("col_idfa", newRowNumber, m_dtConsutla.GetValue(0, 0))
                                    dtAsigMultiple.SetValue("col_desfa", newRowNumber, m_dtConsutla.GetValue(1, 0))
                                End If
                            End If
                        Else
                            m_dtConsutla.ExecuteQuery(queryServ)
                            resultServ = m_dtConsutla.GetValue(0, 0)
                            If Not String.IsNullOrEmpty(resultServ) Then
                                newRowNumber = dtAsigMultiple.Rows.Count
                                dtAsigMultiple.Rows.Add(1)
                                dtAsigMultiple.SetValue("col_code", newRowNumber, itemCode.Value.Trim())
                                dtAsigMultiple.SetValue("col_desc", newRowNumber, itemName.Value.Trim())
                                dtAsigMultiple.SetValue("col_IDEmpA", newRowNumber, empCode.Value.Trim())
                                dtAsigMultiple.SetValue("col_asig", newRowNumber, empName.Value.Trim())
                                dtAsigMultiple.SetValue("col_LnNum", newRowNumber, lineNum.Value.Trim())
                                dtAsigMultiple.SetValue("col_idfa", newRowNumber, m_dtConsutla.GetValue(0, 0))
                                dtAsigMultiple.SetValue("col_desfa", newRowNumber, m_dtConsutla.GetValue(1, 0))
                            End If
                        End If
                    Next

                End If
            End If

            g_oMtxJobs.LoadFromDataSource()

            If dtAsigMultiple.Rows.Count = 0 Or String.IsNullOrEmpty(idEmpSelected) Then
                oForm.Items.Item("btnAsi").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
            Else
                oForm.Items.Item("btnAsi").SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
            End If

            oForm.Items.Item("chkSelAll").Update()
            oForm.Freeze(False)
        Catch ex As Exception
            Throw
        End Try
    End Sub


#End Region

#Region "... Eventos ..."

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        oForm = m_SBO_Application.Forms.Item(FormUID)

        If pVal.BeforeAction Then
            Select Case pVal.ItemUID

            End Select
        ElseIf pVal.ActionSuccess Then
            Select Case pVal.ItemUID

                Case "chkSelAll"
                    sboItem = oForm.Items.Item("chkSelAll")
                    Dim chkSelAll As SAPbouiCOM.CheckBox
                    chkSelAll = DirectCast(sboItem.Specific, SAPbouiCOM.CheckBox)
                    g_oMtxJobs = DirectCast(oForm.Items.Item(mc_strMatrizJobsLines).Specific, Matrix)
                    dtAsigMultiple = oForm.DataSources.DataTables.Item(strDataTableLineas)
                    g_oMtxJobs.FlushToDataSource()
                    Dim i As Integer
                    For i = 0 To dtAsigMultiple.Rows.Count - 1
                        If chkSelAll.Checked Then
                            dtAsigMultiple.SetValue("col_sele", i, "Y")
                        Else
                            dtAsigMultiple.SetValue("col_sele", i, "N")
                        End If
                    Next
                    g_oMtxJobs.LoadFromDataSource()
                    oForm.Items.Item("chkSelAll").Update()

                Case "btnAsi"
                    sboItem = oForm.Items.Item("cboColabor")
                    sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
                    sboItemFas = oForm.Items.Item("cboFas")
                    sboComboFas = DirectCast(sboItemFas.Specific, SAPbouiCOM.ComboBox)
                    If Not String.IsNullOrEmpty(sboCombo.Value) Then
                        'Actualiza_CotizacionOtGeneraFI(oForm)
                        Try
                            'm_oCompany.StartTransaction()
                            If Not AsignaTareas(sboComboFas.Value, sboComboFas.Selected.Description.Trim(), sboCombo.Value, sboCombo.Selected.Description.Trim(), FormUID) Then
                                'm_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrAsigMult, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            Else
                                'm_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                oForm.Close()
                                'm_SBO_Application.StatusBar.SetText(My.Resources.Resource.MsjAsigMultSuccess, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            End If
                        Catch ex As Exception
                            If m_oCompany.InTransaction Then
                                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                            End If
                        End Try

                    Else
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrChooseTechnician, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If

            End Select
        End If
    End Sub

    Public Function AsignaTareas(ByVal strFaseID As String, ByVal desFase As String, ByVal idMecanico As String, ByVal nombreMecanico As String, ByVal FormUID As String) As Boolean
        Dim result = False
        Dim oForm As SAPbouiCOM.Form
        Dim oFormCot As SAPbouiCOM.Form
        Dim itemCode As String
        Dim idActXOrd As String
        Dim strLineNum As String
        Dim lineNum As Integer

        Try
            oForm = m_SBO_Application.Forms.Item(FormUID)
            oFormCot = FormCotizacion

            g_oMtxJobs = DirectCast(oForm.Items.Item(mc_strMatrizJobsLines).Specific, Matrix)
            dtAsigMultiple = oForm.DataSources.DataTables.Item(strDataTableLineas)
            g_oMtxJobs.FlushToDataSource()
            'strIdSucursal = oForm.DataSources.DBDataSources.Item("OQUT")

            Dim i As Integer
            For i = 0 To dtAsigMultiple.Rows.Count - 1
                If dtAsigMultiple.GetValue("col_sele", i) = "Y" Then
                    Dim cont = oFormCot.DataSources.DataTables.Item("MecanicosAsignados").Rows.Count
                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").Rows.Add()

                    lineNum = 0
                    strLineNum = dtAsigMultiple.GetValue("col_LnNum", i).ToString().Trim()
                    Integer.TryParse(strLineNum, lineNum)

                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_CodAct", cont, dtAsigMultiple.GetValue("col_code", i).ToString().Trim())
                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_CodEmp", cont, idMecanico)
                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_NomEmp", cont, nombreMecanico)
                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_LineNum", cont, (lineNum - 1).ToString().Trim())
                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_Estado", cont, dtAsigMultiple.GetValue("col_esta", i).ToString().Trim())

                    If Not String.IsNullOrEmpty(strFaseID) Then
                        oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_NoFase", cont, strFaseID)
                        oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_DesNoFase", cont, desFase)
                    Else
                        oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_NoFase", cont, dtAsigMultiple.GetValue("col_idfa", i).ToString().Trim())
                        oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_DesNoFase", cont, dtAsigMultiple.GetValue("col_desfa", i).ToString().Trim())
                    End If

                    oFormCot.DataSources.DataTables.Item("MecanicosAsignados").SetValue("col_Added", cont, "N")
                    If oFormCot.Mode = BoFormMode.fm_OK_MODE Then
                        oFormCot.Mode = BoFormMode.fm_UPDATE_MODE
                    End If
                End If
            Next
            result = True
            Return result
        Catch ex As Exception
            m_SBO_Application.StatusBar.SetText(ex.Message.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            Return result
        End Try
    End Function

    Public Sub ManejadroEventoCombo(ByVal FormuUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim oForm As SAPbouiCOM.Form
            oForm = m_SBO_Application.Forms.Item(FormuUID)
            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "cboColabor"
                        sboItem = oForm.Items.Item("cboColabor")
                        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
                        If Not String.IsNullOrEmpty(sboCombo.Value) Then
                            LoadMatrixLines(FormuUID, sboCombo.Value.Trim())
                            sboItem = oForm.Items.Item("chkSelAll")
                            Dim chek As SAPbouiCOM.CheckBox = DirectCast(sboItem.Specific, SAPbouiCOM.CheckBox)
                            chek.Checked = False
                        End If
                End Select
            End If
        Catch ex As Exception
            Throw
            'Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region




End Class
