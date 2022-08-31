Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports ICompany = SAPbobsCOM.ICompany


Partial Public Class DevolucionDeVehiculos : Implements IFormularioSBO

#Region "Declaracion Variables"
    Private _applicationSbo As Application
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSBO As SAPbouiCOM.IForm
    Private _inicializado As Boolean

    'propiedades
    Private _nombreXml As String
    Private _titulo As String
    Private _idMenu As String
    Private _menuPadre As String
    Private _nombre As String
    Private _posicion As Integer

    Dim txtFechaDocumento As EditTextSBO
    Dim txtFechaCont As EditTextSBO
    Dim txtComentario As EditTextSBO
    Dim txtDocEntry As EditTextSBO

    Private cboEstado As ComboBoxSBO

    Dim btnAceptar As ButtonSBO
    Dim btnCancelar As ButtonSBO
    Dim btnCopiar As ButtonSBO
    Dim btnProcesar As ButtonSBO
    Dim btnMenos As ButtonSBO

    Public oGestorFormularios As GestorFormularios
    Public MatrixDevolucionDeVehiculos As MatrizDevolucionDeVehiculos

    Public dtCuentas As SAPbouiCOM.DataTable
    Public dtLocal As SAPbouiCOM.DataTable

    Public m_strCodVehDevuelto As String

    Enum EnableBtn
        Mostrar = 1
        Ocultar = 2
        Evaluar = 3
    End Enum

#End Region

#Region "Propiedades"



    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
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

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSBO
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSBO = value
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

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _idMenu
        End Get
        Set(ByVal value As String)
            _idMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _menuPadre
        End Get
        Set(ByVal value As String)
            _menuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _posicion
        End Get
        Set(ByVal value As Integer)
            _posicion = value
        End Set
    End Property





#End Region

#Region "Contructor"
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strDevolucionDeVehiculos As String)

        _companySbo = companySbo
        _applicationSbo = application
        oGestorFormularios = New GestorFormularios(_applicationSbo)
        n = DIHelper.GetNumberFormatInfo(_companySbo)
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLDevolucionDeVehiculos
        MenuPadre = "SCGD_CEIM"
        Nombre = My.Resources.Resource.SubMenuDevolucionVehiculos
        IdMenu = p_strDevolucionDeVehiculos
        Titulo = My.Resources.Resource.SubMenuDevolucionVehiculos
        Posicion = 4
        FormType = p_strDevolucionDeVehiculos

    End Sub

#End Region

#Region "Metodos - Funciones "

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles
        Try

            txtComentario = New EditTextSBO("txtComent", True, m_strTablaDevolucion, "U_Comments", _formularioSBO)
            txtFechaDocumento = New EditTextSBO("txtFhaDoc", True, m_strTablaDevolucion, "U_FhaDocumento", _formularioSBO)
            txtFechaCont = New EditTextSBO("txtFhaFac", True, m_strTablaDevolucion, "U_FhaFactura", _formularioSBO)
            txtDocEntry = New EditTextSBO("txtDocEntr", True, m_strTablaDevolucion, "DocEntry", _formularioSBO)

            cboEstado = New ComboBoxSBO("cboEstado", _formularioSBO, True, m_strTablaDevolucion, "Status")

            btnAceptar = New ButtonSBO("1", _formularioSBO)
            btnCancelar = New ButtonSBO("2", _formularioSBO)
            btnProcesar = New ButtonSBO("btnDevol", _formularioSBO)
            btnCopiar = New ButtonSBO("btnCopy", _formularioSBO)
            btnMenos = New ButtonSBO("btnMenos", _formularioSBO)

            txtComentario.AsignaBinding()
            txtFechaDocumento.AsignaBinding()
            txtDocEntry.AsignaBinding()
            txtFechaCont.AsignaBinding()

            cboEstado.AsignaBinding()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
        

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario
        Try

            CargarFormulario()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Public Sub CargarFormulario()
        Try


            m_strCodVehDevuelto = Utilitarios.EjecutarConsulta("Select U_Devol_Veh from [@SCGD_ADMIN] ", _companySbo.CompanyDB, _companySbo.Server)
            Dim l_strSQLCuentas As String = "Select U_Tipo, U_Transito,U_Stock,U_Costo,U_Ingreso,U_Devolucion from [@SCGD_Admin4]  "

            dtLocal = FormularioSBO.DataSources.DataTables.Add("dtlocal")
            dtCuentas = FormularioSBO.DataSources.DataTables.Add("dtCuentas")
            dtCuentas.Clear()
            dtCuentas.ExecuteQuery(l_strSQLCuentas)

            MatrixDevolucionDeVehiculos = New MatrizDevolucionDeVehiculos("mtxVeh", _formularioSBO, "@SCGD_DEVOLUCION_LIN")
            MatrixDevolucionDeVehiculos.CreaColumnas()
            LigarColumnas(MatrixDevolucionDeVehiculos)

            ManejoBtnProcesar(EnableBtn.Ocultar)

            txtFechaDocumento.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function LigarColumnas(ByRef oMatrix As MatrizDevolucionDeVehiculos) As Boolean

        Dim oColumna As ColumnaMatrixSBO(Of String)
        Dim oColumna2 As ColumnaMatrixSBO(Of Decimal)

        Try

            oColumna = oMatrix.ColumnaPed
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_Pedido")

            oColumna = oMatrix.ColumnaRec
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_Recepcion")

            oColumna = oMatrix.ColumnaUni
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Cod_Unid")

            oColumna = oMatrix.ColumnaMar
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Desc_Marca")

            oColumna = oMatrix.ColumnaEst
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Desc_Estilo")

            oColumna = oMatrix.ColumnaMod
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Desc_Modelo")

            oColumna = oMatrix.ColumnaVin
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_VIN")

            oColumna = oMatrix.ColumnaMot
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_Motor")

            oColumna = oMatrix.ColumnaTip
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Cod_Tipo_Inv")

            oColumna2 = oMatrix.ColumnaMon
            oColumna2.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Monto_As")

            oColumna = oMatrix.ColumnaCur
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Moneda")

            oColumna2 = oMatrix.ColumnaTC
            oColumna2.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Doc_Rate")

            oColumna = oMatrix.ColumnaAsi
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_Asiento")

            oColumna = oMatrix.ColumnaAsD
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_As_Dev")

            oColumna = oMatrix.ColumnaIdV
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Id_Veh")

            oColumna = oMatrix.ColumnaGR
            oColumna.Columna.DataBind.SetBound(True, "@SCGD_DEVOLUCION_LIN", "U_Num_GR")

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub CargarFormularioSeleccionVehiculo()
        Try

            m_oGestorFormularios = New GestorFormularios(_applicationSbo)

            m_oFormularioSeleccionVehiculos = New SeleccionUnidadDevolucion(_applicationSbo, CompanySBO)
            m_oFormularioSeleccionVehiculos.NombreXml = System.Environment.CurrentDirectory + My.Resources.Resource.XMLSeleccionUnidadDevolucion
            m_oFormularioSeleccionVehiculos.FormType = "SCGD_SUD"

            If m_oGestorFormularios.FormularioAbierto(m_oFormularioSeleccionVehiculos, True) = False Then

                m_oGestorFormularios.CargaFormulario(m_oFormularioSeleccionVehiculos)

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


#End Region

#Region "Eventos del Formulario"

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.FormTypeEx <> FormType Then Exit Sub

        Select Case pVal.EventType
            Case BoEventTypes.et_ITEM_PRESSED
                ManejadorEventoItemPressed(FormUID, pVal, BubbleEvent)

            Case BoEventTypes.et_CHOOSE_FROM_LIST
                ManejadorEventoChooseFromList(pVal, FormUID, BubbleEvent)

            Case BoEventTypes.et_COMBO_SELECT
                ManejadorEventoCombo(FormUID, pVal, BubbleEvent)
        End Select

    End Sub


    Private oForm As SAPbouiCOM.Form
    Public m_oFormularioSeleccionVehiculos As SeleccionUnidadDevolucion
    Public m_oGestorFormularios As GestorFormularios


    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case btnCopiar.UniqueId

                        CargarFormularioSeleccionVehiculo()

                    Case btnProcesar.UniqueId
                        ValidarDatos(BubbleEvent)

                    Case btnMenos.UniqueId

                        ValidarEliminarLineas(BubbleEvent)

                End Select
            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case btnProcesar.UniqueId

                        ProcesarDevolucion()
                        _formularioSBO.Mode = BoFormMode.fm_ADD_MODE

                    Case btnMenos.UniqueId

                        EliminarLineasVehiculos()
                        If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                            _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
                        End If

                End Select
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejadorEventoChooseFromList(ByVal pVal As SAPbouiCOM.ItemEvent, ByVal FormUID As String, ByRef BubbleEvent As Boolean)
        Try
            Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim strCFL_Id As String

            Dim oConditions As SAPbouiCOM.Conditions
            Dim oCondition As SAPbouiCOM.Condition

            Dim oDataTable As SAPbouiCOM.DataTable

            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            strCFL_Id = oCFLEvent.ChooseFromListUID
            oCFL = _formularioSBO.ChooseFromLists.Item(strCFL_Id)

            If pVal.BeforeAction Then

            ElseIf pVal.ActionSuccess Then
             
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejadorEventoCombo(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then

            ElseIf pVal.ActionSuccess Then

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item


            Select Case pval.MenuUID
                Case "1282"                 'BOTON NUEVO
                    FormularioSBO.Freeze(True)

                    txtFechaDocumento.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))
                    FormularioSBO.EnableMenu("1282", False)

                    If Not FormularioSBO Is Nothing Then
                        FormularioSBO = ApplicationSBO.Forms.Item("SCGD_DDV")
                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next
                    End If

                    FormularioSBO.Items.Item(txtFechaDocumento.UniqueId).Click()
                    ManejoBtnProcesar(EnableBtn.Ocultar)

                    FormularioSBO.Freeze(False)


                Case "1281"                 'BOTON BUSCAR

                    If Not FormularioSBO Is Nothing Then
                        FormularioSBO = ApplicationSBO.Forms.Item("SCGD_DDV")

                        FormularioSBO.Freeze(True)
                        For Each oItem In FormularioSBO.Items
                            If oItem.UniqueID <> "mtxVeh" Then
                                oItem.Enabled = True
                            End If
                        Next

                        FormularioSBO.EnableMenu("1282", False)
                        FormularioSBO.Freeze(False)
                    End If
                Case "1290", "1288", "1291", "1289"

                    FormularioSBO.EnableMenu("1282", True)

            End Select



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef oTmpForm As SAPbouiCOM.Form)
        Try
            Dim oItem As SAPbouiCOM.Item

            If cboEstado.ObtieneValorDataSource() = "C" Then
                FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
                FormularioSBO.EnableMenu(1282, True)

            Else
                If Not FormularioSBO Is Nothing Then
                    FormularioSBO = ApplicationSBO.Forms.Item("SCGD_DDV")

                    FormularioSBO.Freeze(True)
                    For Each oItem In FormularioSBO.Items
                        If oItem.UniqueID <> "mtxVeh" Then
                            oItem.Enabled = True
                        End If
                    Next
                    FormularioSBO.Freeze(False)
                End If

                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                ManejoBtnProcesar(EnableBtn.Evaluar)
            End If

            FormularioSBO.EnableMenu(1282, True)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub


 

    Private Sub ManejoBtnProcesar(ByVal p_valor As Integer)
        Try
            Dim l_strSQL As String
            Dim l_strGeneradas As String
            Dim l_strDocEntry As String

            l_strDocEntry = txtDocEntry.ObtieneValorDataSource()

            Select Case p_valor
                Case EnableBtn.Mostrar
                    FormularioSBO.Items.Item(btnProcesar.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                Case EnableBtn.Ocultar
                    FormularioSBO.Items.Item(btnProcesar.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                Case EnableBtn.Evaluar

                    If String.IsNullOrEmpty(l_strDocEntry) Then
                        FormularioSBO.Items.Item(btnProcesar.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    ElseIf Not String.IsNullOrEmpty(l_strDocEntry) Then
                        FormularioSBO.Items.Item(btnProcesar.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    End If

                    If ValidarUnidadesDevueltas() Then
                        FormularioSBO.Items.Item(btnProcesar.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    Else
                        FormularioSBO.Items.Item(btnProcesar.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    End If

            End Select
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    'Public Sub AsignaValoresCliente(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
    '    Try
    '        FormularioSBO.Freeze(True)

    '        txtCodigoCliente.AsignaValorDataSource(oDataTable.GetValue("CardCode", 0))
    '        txtNombreCliente.AsignaValorDataSource(oDataTable.GetValue("CardName", 0))

    '        CargarComboContacto()

    '        If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
    '            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
    '        End If


    '        FormularioSBO.Freeze(False)
    '    Catch ex As Exception
    '        Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
    '    End Try
    'End Sub

#End Region

End Class
