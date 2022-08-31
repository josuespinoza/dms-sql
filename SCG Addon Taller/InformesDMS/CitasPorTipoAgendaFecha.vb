Option Explicit On

Imports System.Globalization
Imports System.IO
Imports DMSOneFramework.CitasTableAdapters
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework

Partial Public Class CitasPorTipoAgendaFecha

#Region "Declariones"
    'declaracion de objetos generales 
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As Application
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
    
    Public n As NumberFormatInfo

    'objeto datatable 
    Private _dt As DataTable
    Private _strParametros As String

    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    Public EditTextCdV As EditTextSBO

    Dim md_Agenda As DataTable
    Dim md_Sucursal As DataTable

    
#End Region

#Region "Properties"

    <System.CLSCompliant(False)> _
    Public Property SAPCompany() As SAPbobsCOM.Company
        Get
            Return m_oCompany
        End Get
        Set(ByVal value As SAPbobsCOM.Company)
            m_oCompany = value
        End Set
    End Property

    Public Property dt As DataTable
        Get
            Return _dt
        End Get
        Set(ByVal value As DataTable)
            _dt = value
        End Set
    End Property

    Public Property StrParametros As String
        Get
            Return _strParametros
        End Get
        Set(ByVal value As String)
            _strParametros = value
        End Set
    End Property

#End Region

#Region "Métodos"


    Public Sub CargarCombos()
        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox

            sboItem = FormularioSBO.Items.Item(EditCboSucursal.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, "SELECT Code, Name FROM [@SCGD_SUCURSALES]  ORDER BY name")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub CargarFormulario()

        md_Agenda = FormularioSBO.DataSources.DataTables.Add("tablaAgenda")
        md_Sucursal = FormularioSBO.DataSources.DataTables.Add("tablaSucur")

        EditCbxAgenda.AsignaValorUserDataSource("N")
        EditCbxTecnico.AsignaValorUserDataSource("N")

    End Sub

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

            If cboCombo.ValidValues.Count = 0 Then
                oForm.Items.Item("cboTAgen").Enabled = False
                'oForm.Items.Item("cboTAgen").
            Else
                oForm.Items.Item("cboTAgen").Enabled = True
                oForm.Items.Item("cboTAgen").DisplayDesc = True
            End If

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Throw ex
        End Try

    End Sub

    'Imprimir reportes
    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                               ByVal strBarraTitulo As String, _
                               ByVal strParametros As String)
        Try

            Dim strPathExe As String
            Dim strParametrosEjecutar As String

            objConfiguracionGeneral = Nothing

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
            If m_cn_Coneccion.State = ConnectionState.Open Then
                m_cn_Coneccion.Close()
            End If
            m_cn_Coneccion.ConnectionString = m_strConectionString

            objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

            strDireccionReporte = objConfiguracionGeneral.DireccionReportes & strDireccionReporte
            strDireccionReporte = strDireccionReporte.Replace(" ", "°")
            strParametros = strParametros.Replace(" ", "°")
            strBarraTitulo = strBarraTitulo.Replace(" ", "°")

            strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

            strPathExe &= strBarraTitulo & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
            Shell(strPathExe, AppWinStyle.MaximizedFocus)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region

#Region "Eventos"


    Public Sub ValidarDatos(ByRef BubbleEvent As Boolean)
        Try

            Dim strCodAgenda As String
            Dim strCodSucur As String
            Dim strCodTecnico As String
            Dim strDesde As String
            Dim strHasta As String
            Dim fhaDesde As Date
            Dim fhaHasta As Date

            strCodAgenda = EditCboAgenda.ObtieneValorUserDataSource()
            strCodSucur = EditCboSucursal.ObtieneValorUserDataSource()
            strCodTecnico = EditTextEmpCode.ObtieneValorUserDataSource()
            strDesde = EditTextFhaDesde.ObtieneValorUserDataSource()
            strHasta = EditTextFhaHasta.ObtieneValorUserDataSource()

            If IsDBNull(strCodSucur) OrElse String.IsNullOrEmpty(strCodSucur) Then
                If EditCbxSucursal.ObtieneValorUserDataSource = "N" Then
                    BubbleEvent = False
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptCitaSinSucur, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                End If
            ElseIf (IsDBNull(strCodAgenda) OrElse String.IsNullOrEmpty(strCodAgenda)) AndAlso EditCbxAgenda.ObtieneValorUserDataSource() = "N" Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptCitaSinAgenda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(strDesde) Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptCitaSinFhaDesde, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(strHasta) Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptCitaSinFhaHasta, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
                'ElseIf String.IsNullOrEmpty(strCodTecnico) AndAlso EditCbxAsesor.ObtieneValorUserDataSource() = "N" Then
                '    BubbleEvent = False
                '    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptCitaSinTecnico, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                '    Exit Sub
            End If

        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        End Try
    End Sub

    Public Sub CargarReporte(ByRef BubbleEvent As Boolean)
        Try
            Me.StrParametros = ""

            Dim strCodAgenda As String
            Dim strCodSucur As String = String.Empty
            Dim strCodTecnico As String
            Dim fhaDesde As Date
            Dim fhaHasta As Date
            Dim strWhereTec As String
            Dim strWhereAge As String
            Dim strWhereSucursal As String = " AND CI.U_Cod_Sucursal = '{0}' "
            Dim strNomAgenda As String
            Dim strNomSucur As String = String.Empty
            Dim strNomComp As String
            Dim strOrden As String
            Dim strSQLOrden As String

            Dim strSQLAgenda As String
            Dim strSQLSucursal As String

            strSQLAgenda = "SELECT DocNum, U_Agenda FROM [@SCGD_AGENDA] where DocNum = '{0}' "
            strSQLSucursal = "SELECT Code, Name FROM [@SCGD_SUCURSALES] where Code = '{0}'"


            strCodAgenda = EditCboAgenda.ObtieneValorUserDataSource()
            strCodSucur = EditCboSucursal.ObtieneValorUserDataSource()
            strCodTecnico = EditTextEmpCode.ObtieneValorUserDataSource()

            Dim strDesde As String
            Dim strHasta As String

            strDesde = EditTextFhaDesde.ObtieneValorUserDataSource()
            strHasta = EditTextFhaHasta.ObtieneValorUserDataSource()

            fhaDesde = Date.ParseExact(EditTextFhaDesde.ObtieneValorUserDataSource(), "yyyyMMdd", n)
            fhaHasta = Date.ParseExact(EditTextFhaHasta.ObtieneValorUserDataSource(), "yyyyMMdd", n)

            strOrden = EditCboOrdenar.ObtieneValorUserDataSource()

            strWhereTec = " AND CI.U_Cod_Tecnico = '{0}' "
            strWhereAge = " AND CI.U_Cod_Agenda = '{0}' "

            If Not String.IsNullOrEmpty(EditTextFhaDesde.ObtieneValorUserDataSource) AndAlso
                Not String.IsNullOrEmpty(EditTextFhaHasta.ObtieneValorUserDataSource) Then

                strSQLAgenda = String.Format(strSQLAgenda, strCodAgenda)


                md_Agenda.Clear()
                md_Agenda.ExecuteQuery(strSQLAgenda)

                strSQLSucursal = String.Format(strSQLSucursal, strCodSucur)
                md_Sucursal.Clear()
                md_Sucursal.ExecuteQuery(strSQLSucursal)

                If md_Agenda.GetValue("DocNum", 0) <> 0 AndAlso md_Agenda.Rows.Count <> 0 Then
                    strNomAgenda = md_Agenda.GetValue("U_Agenda", 0)
                End If

                If Not String.IsNullOrEmpty(strCodSucur) Then
                    strWhereSucursal = String.Format(strWhereSucursal, strCodSucur)

                    If md_Sucursal.GetValue("Code", 0) <> 0 AndAlso md_Agenda.Rows.Count <> 0 Then
                        strNomSucur = md_Sucursal.GetValue("Name", 0)
                    End If

                End If

                strNomComp = m_oCompany.CompanyName

                If fhaDesde <= fhaHasta Then

                    If EditCbxSucursal.ObtieneValorUserDataSource() = "Y" Then
                        strWhereSucursal = String.Empty
                        strNomSucur = My.Resources.Resource.TODAS
                    End If

                    If EditCbxAgenda.ObtieneValorUserDataSource() = "Y" Then
                        strWhereAge = String.Empty
                        strNomAgenda = "Todas"
                    ElseIf EditCbxAgenda.ObtieneValorUserDataSource() = "N" Then
                        strWhereAge = String.Format(strWhereAge, strCodAgenda)
                    End If

                    If EditCbxTecnico.ObtieneValorUserDataSource() = "Y" OrElse String.IsNullOrEmpty(strCodTecnico) Then
                        strWhereTec = String.Empty
                    ElseIf EditCbxTecnico.ObtieneValorUserDataSource() = "N" Then
                        strWhereTec = String.Format(strWhereTec, strCodTecnico)
                    End If

                    If EditCboOrdenar.ObtieneValorUserDataSource = "2" Then
                        strSQLOrden = " order by CI.U_Num_Serie , CI.U_NumCita ASC"
                    Else
                        strSQLOrden = " Order By CI.U_FechaCita, CI.U_HoraCita ASC "
                    End If

                    StrParametros = fhaDesde & "," & fhaHasta & "," & strWhereSucursal & "," & strWhereAge & "," & strWhereTec & "," & strSQLOrden & "," & strNomAgenda & "," & strNomSucur & "," & strNomComp
                Else

                End If
            End If

            If Not String.IsNullOrEmpty(StrParametros) Then

                Call ImprimirReporte(My.Resources.Resource.rptCitasSucursalAgendaTec, My.Resources.Resource.TituloReporteCitasXTipo, StrParametros)

            Else
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorReporteCV, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

            End If


        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    'Public Sub ManejadorEventoClick(ByRef pval As SAPbouiCOM.ItemEvent,
    '                                ByVal FormUID As String,
    '                                ByRef BubbleEvent As Boolean,
    '                                ByVal comp As SAPbobsCOM.Company,
    '                                ByVal strUserName As String,
    '                                ByVal strPass As String)

    '    Try
    '        'obtengo el form del que sucedio el evento
    '        oForm = m_SBO_Application.Forms.Item(FormUID)
    '        m_oCompany = comp

    '        'ACTION SUCCESS
    '        If pval.ItemUID = "btnPrint" _
    '            And pval.ActionSuccess = True _
    '            And pval.BeforeAction = False Then

    '            Dim strFechaDesde As String = ""
    '            Dim strFechaHasta As String = ""
    '            Dim strTipoAgenda As String = ""
    '            Dim strAsesor As String = ""
    '            Me.StrParametros = ""

    '            'obtengo los parametros del datatable
    '            strFechaDesde = dt.GetValue("fechaDesde", 0)
    '            strFechaHasta = dt.GetValue("fechaHasta", 0)
    '            strTipoAgenda = dt.GetValue("tipoAgenda", 0)
    '            strAsesor = dt.GetValue("asesor", 0)

    '            'verifico que los parametros sean correctos
    '            If Not String.IsNullOrEmpty(strFechaDesde) _
    '                And Not String.IsNullOrEmpty(strFechaHasta) _
    '                And Not String.IsNullOrEmpty(strTipoAgenda) _
    '                And Not String.IsNullOrEmpty(strAsesor) _
    '                And oForm.Items.Item(EditCboAgenda.UniqueId).Enabled = True _
    '                And pval.BeforeAction = False _
    '                And pval.ActionSuccess = True Then

    '                'Obtengo las fechas ingresadas
    '                Dim strFechaDesdeFormateada As String = ""
    '                Dim strFechaHastaFormateada As String = ""

    '                strFechaDesdeFormateada = Utilitarios.RetornaFechaFormatoRegional(strFechaDesde)
    '                strFechaHastaFormateada = Utilitarios.RetornaFechaFormatoRegional(strFechaHasta)

    '                If Not String.IsNullOrEmpty(strFechaDesdeFormateada.ToString()) _
    '                    And Not String.IsNullOrEmpty(strFechaHastaFormateada.ToString()) Then

    '                    'Verifico las fechas, hasta mayor que la desde
    '                    If CDate(strFechaDesdeFormateada) <= CDate(strFechaHastaFormateada) Then

    '                        StrParametros = strFechaDesdeFormateada.ToString() & "," & strFechaHastaFormateada.ToString() & "," & strTipoAgenda ' & "," & strAsesor
    '                    End If
    '                End If

    '            End If

    '            'parametros
    '            If Not String.IsNullOrEmpty(StrParametros) Then
    '                Call ImprimirReporte(My.Resources.Resource.rptCitasXTipoFecha, My.Resources.Resource.TituloReporteCitasXTipo, StrParametros)
    '            Else
    '                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorReporteCV, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Error)

    '            End If
    '        End If

    '    Catch ex As Exception
    '        'manejo de errores
    '        Utilitarios.ManejadorErrores(ex, m_SBO_Application)
    '    End Try

    'End Sub

    Public Sub ManejoEventosCombo(ByVal formUID As String, _
                                      ByVal pval As SAPbouiCOM.ItemEvent, _
                                      ByRef BubbleEvent As Boolean)
        Try
            Dim cboCombo As ComboBox
            Dim oItem As Item

            Dim l_strSucursal As String
            Dim l_strAgenda As String
            Dim l_strSQLAgendas As String
            Dim l_strSQLTecnico As String


            l_strSQLAgendas = "SELECT DocNum, U_Agenda, U_CodTecnico, U_NameTecnico FROM [@SCGD_AGENDA] where U_Cod_Sucursal = '{0}' AND U_EstadoLogico = 'Y'"
            l_strSQLTecnico = "SELECT DocNum, U_Agenda, U_CodTecnico, U_NameTecnico, U_CodAsesor, U_NameAsesor , U_RazonCita FROM [@SCGD_AGENDA] where DocEntry = '{0}' AND U_EstadoLogico = 'Y'"

            If pval.ActionSuccess Then
                If pval.ItemUID = EditCboSucursal.UniqueId Then
                    oItem = FormularioSBO.Items.Item(EditCboSucursal.UniqueId)
                    cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                    l_strSucursal = cboCombo.Selected.Value

                    If cboCombo.Active Then
                        oItem = FormularioSBO.Items.Item(EditCboAgenda.UniqueId)
                        cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                        Call Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, _
                                                                    String.Format(l_strSQLAgendas, l_strSucursal))
                    End If

                ElseIf pval.ItemUID = EditCboAgenda.UniqueId Then
                    oItem = FormularioSBO.Items.Item(EditCboAgenda.UniqueId)
                    cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                    l_strAgenda = cboCombo.Selected.Value

                    l_strSQLTecnico = String.Format(l_strSQLTecnico, l_strAgenda)

                    'md_Agenda.Clear()
                    'md_Agenda.ExecuteQuery(l_strSQLTecnico)

                    'If md_Agenda.Rows.Count <> 0 Then
                    '    EditTextEmpName.AsignaValorUserDataSource(md_Agenda.GetValue("U_NameTecnico", 0))
                    '    EditTextEmpCode.AsignaValorUserDataSource(md_Agenda.GetValue("U_CodTecnico", 0))
                    'End If
                End If

                'FormularioSBO.Mode
            ElseIf pval.BeforeAction Then

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim l_TodoAgenda As String
            Dim l_TodosAsesor As String
            Dim strTodasSucursales As String = String.Empty

            Dim txtTexto As TextBox
            Dim oItem As Item

            oItem = FormularioSBO.Items.Item(EditTextEmpName.UniqueId)
            ' txtTexto = DirectCast(oItem.Specific, SAPbouiCOM.EditText)

            If pVal.ItemUID = BtnPrintSbo.UniqueId Then
                If pVal.BeforeAction Then
                    ValidarDatos(BubbleEvent)
                ElseIf pVal.ActionSuccess Then
                    CargarReporte(BubbleEvent)
                End If

            ElseIf pVal.ItemUID = EditCbxAgenda.UniqueId Then

                If pVal.ActionSuccess Then
                    _formularioSbo.Freeze(True)

                    l_TodoAgenda = EditCbxAgenda.ObtieneValorUserDataSource()

                    If l_TodoAgenda = "Y" Then

                        _formularioSbo.Items.Item(EditTextFhaDesde.UniqueId).Click()
                        _formularioSbo.Items.Item(EditCboAgenda.UniqueId).Enabled = False

                        EditTextEmpName.AsignaValorUserDataSource(String.Empty)
                        EditTextEmpCode.AsignaValorUserDataSource(String.Empty)
                    ElseIf l_TodoAgenda = "N" Then

                        _formularioSbo.Items.Item(EditCboAgenda.UniqueId).Enabled = True

                    End If
                    _formularioSbo.Freeze(False)
                End If

            ElseIf pVal.ItemUID = EditCbxTecnico.UniqueId Then

                If pVal.ActionSuccess Then
                    _formularioSbo.Freeze(True)

                    l_TodosAsesor = EditCbxTecnico.ObtieneValorUserDataSource()

                    If l_TodosAsesor = "Y" Then
                        _formularioSbo.Items.Item(EditTextFhaDesde.UniqueId).Click()

                        _formularioSbo.Items.Item(EditTextEmpCode.UniqueId).Enabled = False

                        EditTextEmpName.AsignaValorUserDataSource(String.Empty)
                        EditTextEmpCode.AsignaValorUserDataSource(String.Empty)
                    ElseIf l_TodosAsesor = "N" Then
                        _formularioSbo.Items.Item(EditTextEmpCode.UniqueId).Enabled = True
                    End If
                    _formularioSbo.Freeze(False)

                ElseIf pVal.BeforeAction Then



                End If
            ElseIf pVal.ItemUID = EditCbxSucursal.UniqueId Then
                If pVal.ActionSuccess Then
                    _formularioSbo.Freeze(True)

                    strTodasSucursales = EditCbxSucursal.ObtieneValorUserDataSource()

                    If strTodasSucursales.ToUpper().Trim() = "Y" Then
                        _formularioSbo.Items.Item(EditTextFhaDesde.UniqueId).Click()
                        _formularioSbo.Items.Item(EditTextEmpCode.UniqueId).Enabled = False

                        EditTextEmpCode.AsignaValorUserDataSource(String.Empty)
                        EditTextEmpName.AsignaValorUserDataSource(String.Empty)
                    ElseIf strTodasSucursales.ToUpper().Trim() = "N" Then
                        _formularioSbo.Items.Item(EditTextEmpCode.UniqueId).Enabled = True
                    End If

                    _formularioSbo.Freeze(False)
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

#End Region

End Class
