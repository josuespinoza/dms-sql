Option Explicit On

Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework

Public Class FacturacionVehiculosPorVendedor

#Region "Declaraciones"

    'General
    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As SAPbouiCOM.Application
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
    Public n As NumberFormatInfo
    Private m_strDireccionConfiguracion As String

    'ObjDataTable 
    Private _dt As DataTable
    Private _strParametros As String

    'Conection
    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    Public EditTextCdV As EditTextSBO

#End Region

#Region "Propiedades"

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

#Region "Metodos"

    ''' <summary>
    ''' Cargar Combos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaCombos()
        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombosuscursal As SAPbouiCOM.ComboBox
            Dim sboComboMarca As SAPbouiCOM.ComboBox
            Dim sboComboTipoVehi As SAPbouiCOM.ComboBox
            Dim strCodUsadoValue As String = "U"
            Dim strNameUsadoValue As String = My.Resources.Resource.NameValueUsedFactvehi
            Dim strCodNuevoValue As String = "N"
            Dim strNameNuevoValue As String = My.Resources.Resource.NameValueNewFactVehi

            ''Sucursal
            sboItem = FormularioSBO.Items.Item(_cbo_Sucursal.UniqueId)
            sboCombosuscursal = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboCombosuscursal.ValidValues, "SELECT Code, Name FROM [@SCGD_SUCURSALES]  ORDER BY name")
            CargaSucursal()

            ''Marca
            sboItem = FormularioSBO.Items.Item(_cbo_Marca.UniqueId)
            sboComboMarca = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            Call Utilitarios.CargarValidValuesEnCombos(sboComboMarca.ValidValues, "SELECT Code, Name FROM [@SCGD_Marca]  ORDER BY name")
            sboComboMarca.ValidValues.Add("", "")
            sboComboMarca.Select(0, BoSearchKey.psk_Index)


            ''TipoVehi
            sboItem = FormularioSBO.Items.Item(_cbo_TipoVehe.UniqueId)
            sboComboTipoVehi = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            sboComboTipoVehi.ValidValues.Add(strCodNuevoValue, strNameNuevoValue)
            sboComboTipoVehi.ValidValues.Add(strCodUsadoValue, strNameUsadoValue)
            sboComboTipoVehi.ValidValues.Add("", "")
            sboComboTipoVehi.Select(0, BoSearchKey.psk_Index)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' Carga Automatica de Sucursal
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargaSucursal()
        Try
            Dim sboItem As SAPbouiCOM.Item
            Dim sboCombo As SAPbouiCOM.ComboBox
            Dim strUsuario As String = String.Empty
            Dim strConsulta As String = String.Empty
            Dim strSucursalTaller As String = String.Empty


            strUsuario = m_oCompany.UserName.ToString.Trim()

            strConsulta = "Select SUC.Code " &
                            "From dbo.OUSR USR Inner Join [dbo].[@SCGD_SUCURSALES] SUC " &
                            "On USR.Branch=SUC.Code " &
                            "Where USR.USER_CODE='" & strUsuario & "'"

            strSucursalTaller = Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)


            sboItem = FormularioSBO.Items.Item(_cbo_Sucursal.UniqueId)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            sboCombo.Select(strSucursalTaller, SAPbouiCOM.BoSearchKey.psk_ByValue)
            'sboCombo.ValidValues.Add("", "")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Public Sub CargarFormulario()

        Dim strValorRb As String = "N"
        Dim strValorDefecto As String = "Y"
        _rb_Vendedor.AsignaValorUserDataSource(strValorRb)
        _rb_Marca.AsignaValorUserDataSource(strValorDefecto)

        _cbxTipVe.AsignaValorUserDataSource("N")
        _cbxMarc.AsignaValorUserDataSource("N")
        _cbxVend.AsignaValorUserDataSource("N")

    End Sub

    ''' <summary>
    ''' Validacion Combos
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="strQuery"></param>
    ''' <param name="strIDItem"></param>
    ''' <remarks></remarks>
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
        Dim m_cboSucursal As String = "cmb_Sucur"
        Dim m_cboTipoVehi As String = "cmb_TipoSu"
        Dim m_cboMarca As String = "cmb_Marc"
        Dim m_txtVendedores As String = "txtVend"

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

            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
                End If
            Loop

            If cboCombo.ValidValues.Count = 0 Then

                oForm.Items.Item(m_cboSucursal).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                oForm.Items.Item(m_cboTipoVehi).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                oForm.Items.Item(m_cboMarca).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)
                oForm.Items.Item(m_txtVendedores).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False)

            Else

                oForm.Items.Item(m_cboSucursal).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                oForm.Items.Item(m_cboTipoVehi).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                oForm.Items.Item(m_cboMarca).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)
                oForm.Items.Item(m_txtVendedores).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

            End If

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    ''' <summary>
    ''' Maneja Evnto del Combo
    ''' </summary>
    ''' <param name="formUID"></param>
    ''' <param name="pval"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejoEventosCombo(ByVal formUID As String, _
                                     ByVal pval As SAPbouiCOM.ItemEvent, _
                                     ByRef BubbleEvent As Boolean)


    End Sub

    ''' <summary>
    ''' Manejador de evento ItemPresed
    ''' </summary>
    ''' <param name="formUID"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoItemPress(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim strDet As String = ""
            Dim strRes As String = ""
            Dim strValorNegaRb As String = "N"
            Dim strValorPosRb As String = "Y"

            Dim sboItem As Item
            Dim sboCombo As ComboBox
            Dim m_txtFechaD As SAPbouiCOM.EditText
            Dim m_txtVendedor As SAPbouiCOM.EditText
            Dim l_TodoOT As String = String.Empty

            Select Case pVal.ItemUID

                Case _rb_Marca.UniqueId

                    strDet = _rb_Vendedor.ObtieneValorUserDataSource()

                    If strDet = strValorNegaRb Then
                        _rb_Marca.AsignaValorUserDataSource(strValorPosRb)
                        _rb_Vendedor.AsignaValorUserDataSource(strValorNegaRb)
                        FormularioSBO.Freeze(False)

                    ElseIf strDet = strValorPosRb Then
                        _rb_Marca.AsignaValorUserDataSource(strValorPosRb)
                        _rb_Vendedor.AsignaValorUserDataSource(strValorNegaRb)
                        FormularioSBO.Freeze(False)
                    End If

                Case _rb_Vendedor.UniqueId

                    strRes = _rb_Marca.ObtieneValorUserDataSource()

                    If strRes = strValorNegaRb Then
                        _rb_Vendedor.AsignaValorUserDataSource(strValorPosRb)
                        _rb_Marca.AsignaValorUserDataSource(strValorNegaRb)
                        FormularioSBO.Freeze(False)
                    ElseIf strRes = strValorPosRb Then
                        _rb_Marca.AsignaValorUserDataSource(strValorNegaRb)
                        _rb_Vendedor.AsignaValorUserDataSource(strValorPosRb)
                        FormularioSBO.Freeze(False)
                    End If

                Case BtnPrintSbo.UniqueId

                    If pVal.BeforeAction Then
                        ValidarDatos(BubbleEvent)
                    ElseIf pVal.ActionSuccess Then
                        CargarReporte()
                    End If

                    'Check de Tipo Vehiculo
                Case _cbxTipVe.UniqueId
                    If pVal.ActionSuccess Then
                        _FormularioSBO.Freeze(True)

                        l_TodoOT = _cbxTipVe.ObtieneValorUserDataSource()

                        sboItem = FormularioSBO.Items.Item(_cbo_TipoVehe.UniqueId)
                        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

                        If l_TodoOT = "N" Then
                            _FormularioSBO.Items.Item(_cbo_TipoVehe.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                            sboCombo.Select(0, BoSearchKey.psk_Index)
                        ElseIf l_TodoOT = "Y" Then
                            _FormularioSBO.Items.Item(_cbo_TipoVehe.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                            sboCombo.Select(sboCombo.ValidValues.Count - 1, BoSearchKey.psk_Index)
                        End If
                        _FormularioSBO.Freeze(False)
                    End If

                    'Check Marca Vehiculo
                Case _cbxMarc.UniqueId
                    If pVal.ActionSuccess Then
                        _FormularioSBO.Freeze(True)

                        l_TodoOT = _cbxMarc.ObtieneValorUserDataSource()

                        sboItem = FormularioSBO.Items.Item(_cbo_Marca.UniqueId)
                        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

                        If l_TodoOT = "N" Then
                            _FormularioSBO.Items.Item(_cbo_Marca.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)
                            sboCombo.Select(0, BoSearchKey.psk_Index)
                        ElseIf l_TodoOT = "Y" Then
                            _FormularioSBO.Items.Item(_cbo_Marca.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)
                            sboCombo.Select(sboCombo.ValidValues.Count - 1, BoSearchKey.psk_Index)
                        End If
                        _FormularioSBO.Freeze(False)
                    End If

                    'Check Vendedor
                Case _cbxVend.UniqueId
                    If pVal.ActionSuccess Then
                        _FormularioSBO.Freeze(True)

                        l_TodoOT = _cbxVend.ObtieneValorUserDataSource()

                        If l_TodoOT = "N" Then
                            'Asigno el focus al txt de la fecha para desbloquear el texto de vendedor
                            m_txtFechaD = FormularioSBO.Items.Item(_txtFechaDesde.UniqueId).Specific
                            m_txtFechaD.Active = True

                            'm_txtVendedor = FormularioSBO.Items.Item(_txtVendedores.UniqueId).Specific
                            'm_txtVendedor.Value = ""

                            'Desbloqueo el txt Vendedor
                            _FormularioSBO.Items.Item(_txtVendedores.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_True)

                        ElseIf l_TodoOT = "Y" Then
                            'Asigno el focus al txt de la fecha para bloquear el texto de vendedor
                            m_txtFechaD = FormularioSBO.Items.Item(_txtFechaDesde.UniqueId).Specific
                            m_txtFechaD.Active = True

                            m_txtVendedor = FormularioSBO.Items.Item(_txtVendedores.UniqueId).Specific
                            m_txtVendedor.Value = ""

                            'Bloqueo el txt Vendedor
                            _FormularioSBO.Items.Item(_txtVendedores.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, 11, BoModeVisualBehavior.mvb_False)

                            'Reseteo el Codigo del Vendedor
                            strCodVendedor = String.Empty
                        End If
                        _FormularioSBO.Freeze(False)
                    End If

            End Select
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub
    ''' <summary>
    ''' Valida datos
    ''' </summary>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ValidarDatos(ByRef BubbleEvent As Boolean)
        Try

            Dim strtxtfechD As String
            Dim strtxtfechH As String
            Dim DFechD As Date
            Dim DFechH As Date
            Dim strFechaDesFormateada As String = String.Empty
            Dim strFechHastFormateada As String = ""
            Dim cboSucu As String
            Dim cboTipV As String
            Dim cboMar As String
            Dim strtxtVen As String
            Dim rbDetallado As String
            Dim rbResumido As String


            cboSucu = _cbo_Sucursal.ObtieneValorUserDataSource()
            cboTipV = _cbo_TipoVehe.ObtieneValorUserDataSource()
            cboMar = _cbo_Marca.ObtieneValorUserDataSource()
            strtxtfechD = _txtFechaDesde.ObtieneValorUserDataSource()
            strtxtfechH = _txtFechaHasta.ObtieneValorUserDataSource()
            strtxtVen = _txtVendedores.ObtieneValorUserDataSource()
            rbDetallado = _rb_Vendedor.ObtieneValorUserDataSource()
            rbResumido = _rb_Marca.ObtieneValorUserDataSource()


            If (IsDBNull(cboSucu) OrElse String.IsNullOrEmpty(cboSucu) Or Nothing) Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehiSeleSu, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf (IsDBNull(cboTipV) OrElse String.IsNullOrEmpty(cboTipV) Or Nothing) AndAlso _cbxTipVe.ObtieneValorUserDataSource() = "N" Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehiTipVeh, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf (IsDBNull(cboMar) OrElse String.IsNullOrEmpty(cboMar) Or Nothing) AndAlso _cbxMarc.ObtieneValorUserDataSource() = "N" Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehiMarca, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf (String.IsNullOrEmpty(strtxtVen) Or Nothing) AndAlso _cbxVend.ObtieneValorUserDataSource() = "N" Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFactVehiVende, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(strtxtfechD) Or Nothing Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehiFechas, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf String.IsNullOrEmpty(strtxtfechH) Or Nothing Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehiFechas, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub
            ElseIf rbDetallado = rbResumido Then
                BubbleEvent = False
                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehiRadioButton, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Sub

            End If

            If strtxtfechD <> Nothing And strtxtfechH <> Nothing Then
                DFechH = Date.ParseExact(strtxtfechH, "yyyyMMdd", Nothing)
                DFechD = Date.ParseExact(strtxtfechD, "yyyyMMdd", Nothing)
                strFechaDesFormateada = Utilitarios.RetornaFechaFormatoDB(DFechD, m_oCompany.Server, False)
                strFechHastFormateada = Utilitarios.RetornaFechaFormatoDB(DFechH, m_oCompany.Server, False)
                If DFechD > DFechH Then
                    BubbleEvent = False
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorRptFacVehifech, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                End If
            End If
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Imprimir Reporte
    ''' </summary>
    ''' <param name="strDireccionReporte"></param>
    ''' <param name="strBarraTitulo"></param>
    ''' <param name="strParametros"></param>
    ''' <remarks></remarks>
    Public Sub ImprimirReporte(ByVal strDireccionReporte As String, _
                              ByVal strBarraTitulo As String, _
                              ByVal strParametros As String)
        Try
            Dim strPathExe As String = String.Empty

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

    ''' <summary>
    ''' Carga Reportes Parametros
    ''' </summary>

    ''' <remarks></remarks>
    Public Sub CargarReporte()
        Try

            Dim strTipoRpt As String = String.Empty
            StrParametros = ""
            'Dim strtxtVendedores As String = String.Empty
            Dim DtxtFechaD As Date
            Dim DtxtFechaH As Date
            Dim strFechHastFormateada As String = String.Empty
            Dim strFechDesFormateada As String = String.Empty
            Dim strSucursal As String = String.Empty
            Dim strMarca As String = String.Empty
            Dim strtxtTipoV As String = String.Empty
            Dim strrbVendedor As String = String.Empty
            Dim strrbMarca As String = String.Empty
            Dim strValorNegaRb As String = "N"
            Dim strValorPosRb As String = "Y"
            Dim strTodo As String = String.Empty

            Dim str_pMarca As String = "N"
            Dim str_pVehiculo As String = "N"
            Dim str_pVendedor As String = "N"

            strSucursal = _cbo_Sucursal.ObtieneValorUserDataSource()
            DtxtFechaD = Date.ParseExact(_txtFechaDesde.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
            DtxtFechaH = Date.ParseExact(_txtFechaHasta.ObtieneValorUserDataSource(), "yyyyMMdd", Nothing)
            strtxtTipoV = _cbo_TipoVehe.ObtieneValorUserDataSource()
            strMarca = _cbo_Marca.ObtieneValorUserDataSource()
            strrbVendedor = _rb_Vendedor.ObtieneValorUserDataSource()
            strrbMarca = _rb_Marca.ObtieneValorUserDataSource()
            strFechHastFormateada = Utilitarios.RetornaFechaFormatoDB(DtxtFechaH, m_oCompany.Server, False)
            strFechDesFormateada = Utilitarios.RetornaFechaFormatoDB(DtxtFechaD, m_oCompany.Server, False)

            'If strMarca = String.Empty Then str_pMarca = "Y"
            'If strtxtTipoV = String.Empty Then str_pVehiculo = "Y"
            'If strCodVendedor = String.Empty Then str_pVendedor = "Y"

            'If strMarca = String.Empty And
            '    strtxtTipoV = String.Empty And
            '    strCodVendedor = String.Empty Then
            '    str_pMarca = "N"
            '    str_pVehiculo = "N"
            '    str_pVendedor = "N"
            'End If

            If strrbVendedor = strValorPosRb And strrbMarca = strValorNegaRb Then strTipoRpt = "V"
            If strrbVendedor = strValorNegaRb And strrbMarca = strValorPosRb Then strTipoRpt = "M"

            StrParametros = String.Format("{0},{1},{2},{3},{4},{5}",
                                          strCodVendedor, strSucursal, strFechDesFormateada,
                                          strFechHastFormateada, strtxtTipoV, strMarca)
            'str_pMarca, str_pVehiculo, str_pVendedor)

            If Not String.IsNullOrEmpty(StrParametros) Then
                If strTipoRpt = "V" Then
                    Call ImprimirReporte(My.Resources.Resource.rptFacturacionVehiculoPorVendedor, My.Resources.Resource.TituloFacturacionVehiculosPorVendedor, StrParametros)
                End If
                If strTipoRpt = "M" Then
                    Call ImprimirReporte(My.Resources.Resource.rptFacturacionVehiculoPorMarca, My.Resources.Resource.TituloFacturacionVehiculosPorVendedor, StrParametros)
                End If
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub
#End Region

End Class
