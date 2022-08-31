Option Explicit On

Imports System.Globalization
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework

Public Class EntregaVehiculosOT : Implements IFormularioSBO

#Region "Declaraciones"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As SAPbobsCOM.ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String
    Private objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon

    'objeto form 
    Private oForm As SAPbouiCOM.Form
    Private Const strMatrizVehiculos As String = "mtxVehi"
    Private Const strDT As String = "DT_VEH"
    Private Const strFORM_ID As String = "SCGD_ESTOT"
    Private Const txtUnidad As String = "txtNoUnid"
    Private Const txtPlaca As String = "txtNoPla"
    Private Const txtOT As String = "txtNoOT"


    'Matriz vehiculos
    Public MatrizVehiculos As MatrizVehi

    Private m_strDireccionConfiguracion As String
    Public n As NumberFormatInfo

    'Manejo de la matriz de vehiculos
    Private m_dbVehiculos As SAPbouiCOM.DataTable
    
    'Campos en bd 
    Private Const str_C_NoUnidad As String = "C.U_SCGD_Cod_Unidad"
    Private Const str_C_NoPlaca As String = "C.U_SCGD_Num_Placa"
    Private Const str_C_NoOT As String = "C.U_SCGD_Numero_OT"
    Private _strNoUnidad As String
    Private _strNoPlaca As String
    Private _strNoOT As String
    Private _strEstadoOT As String
    Private _strDesde As DateTime
    Private _strHasta As DateTime

    'campo con id de cotizacion
    Private _idCotizacion As String

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)

        'declaracion de objetos acplication , company y decimaels 
        _companySbo = ocompany
        _applicationSbo = SBOAplication
    End Sub

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

        End Set
    End Property

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

    Public Property strNoUnidad As String
        Get
            Return _strNoUnidad
        End Get
        Set(ByVal value As String)
            _strNoUnidad = value
        End Set
    End Property

    Public Property strNoPlaca As String
        Get
            Return _strNoPlaca
        End Get
        Set(ByVal value As String)
            _strNoPlaca = value
        End Set
    End Property

    Public Property strNoOt As String
        Get
            Return _strNoOT
        End Get
        Set(ByVal value As String)
            _strNoOT = value
        End Set
    End Property

    Public Property IdCotizacion As String
        Get
            Return _idCotizacion
        End Get
        Set(ByVal value As String)
            _idCotizacion = value
        End Set
    End Property

    Public Property StrEstadoOt As String
        Get
            Return _strEstadoOT
        End Get
        Set(ByVal value As String)
            _strEstadoOT = value
        End Set
    End Property

    Public Property StrDesde As DateTime
        Get
            Return _strDesde
        End Get
        Set(ByVal value As DateTime)
            _strDesde = value
        End Set
    End Property

    Public Property StrHasta As DateTime
        Get
            Return _strHasta
        End Get
        Set(ByVal value As DateTime)
            _strHasta = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        If FormularioSBO IsNot Nothing Then

        End If
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

    End Sub

    'Metodo para cargar la pantalla de reportes de contratos de Venta
    Public Sub CargarFormulario()
        'variables a utilizar
        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            'parametros para el form que se abrirá
            fcp = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed
            fcp.FormType = strFORM_ID

            'se designa el XML que se cargara
            strXMLACargar = My.Resources.Resource.XMLEstadosOT
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = ApplicationSBO.Forms.AddEx(fcp)

            'Manejo de formulario
            oForm.Freeze(True)

            'enlazar campos de texto
            LinkearCamposTexto()

            'enlazar matriz
            LinkearMatriz()

            oMatrix = DirectCast(oForm.Items.Item(strMatrizVehiculos).Specific, SAPbouiCOM.Matrix)
            m_dbVehiculos = oForm.DataSources.DataTables.Item(strDT)

            'If EnlazaColumnasMatrixaDatasource(oMatrix) Then
            Call CargarMatriz(oMatrix, oForm, m_dbVehiculos)
            'End If

            oForm.DataSources.DataTables.Add("dtConsulta")

            'Manejo de formulario
            oForm.Freeze(False)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    'CARGA EL XML DE LA PANTALLA 
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

    'Metodo para agregar items al menu
    Protected Friend Sub AddMenuItems()
        Dim strEtiquetaMenu As String = ""
        'Orcion de menu de Estados de OT
        If Utilitarios.MostrarMenu("SCGD_EOT", ApplicationSBO.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_EOT", ApplicationSBO.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_EOT", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 16, False, True, "SCGD_GOV"))

        End If

    End Sub

    'Carga la matriz con las OT que cumplan los filtros seleccionados
    Public Function CargarMatriz(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                       ByVal oform As SAPbouiCOM.Form, _
                                       ByVal dbVehiculo As SAPbouiCOM.DataTable) As Boolean

        Dim strCondiciones As String
        Dim strConsulta As String = ""

        Try
            Call ObtenerFiltros()
            strCondiciones = ""

            If Not String.IsNullOrEmpty(strNoOt) Then
                'strCondiciones &= " and " & str_C_NoOT & " = '" & strNoOt & "'"
                strCondiciones &= String.Format(" and {0} = '{1}' ", str_C_NoOT, strNoOt)
            End If

            If Not String.IsNullOrEmpty(strNoUnidad) Then
                'strCondiciones &= " and " & str_C_NoUnidad & " = '" & strNoUnidad & "'"
                strCondiciones &= String.Format(" and {0} = '{1}' ", str_C_NoUnidad, strNoUnidad)
            End If

            If Not String.IsNullOrEmpty(strNoPlaca) Then
                'strCondiciones &= " and " & str_C_NoPlaca & " = '" & strNoPlaca & "'"
                strCondiciones &= String.Format(" and {0} = '{1}' ", str_C_NoPlaca, strNoPlaca)
            End If

            If Not String.IsNullOrEmpty(StrEstadoOt) _
                And Not String.IsNullOrEmpty(StrDesde.Date.ToString) _
                And Not String.IsNullOrEmpty(StrHasta.Date.ToString) Then
                'se d formato a las fechas ingresadas
                Dim fdesde_local As String = Utilitarios.RetornaFechaFormatoDB(StrDesde, ApplicationSBO.Company.ServerName, False)
                Dim fhasta_local As String = Utilitarios.RetornaFechaFormatoDB(StrHasta, ApplicationSBO.Company.ServerName, False)
                strConsulta =
                    String.Format(" SELECT C.DocEntry, C.U_SCGD_Numero_OT, C.U_SCGD_Cod_Unidad, C.U_SCGD_Num_Placa ,C.U_SCGD_Num_VIN ,C.U_SCGD_Des_Marc, " +
                                  " C.U_SCGD_Des_Esti, C.U_SCGD_Des_Mode, C.U_SCGD_Ano_Vehi, C.U_SCGD_Num_Vehiculo " +
                                  " FROM OQUT AS C WHERE ( C.U_SCGD_Estado_Cot = '{0}' ) AND C.DocDate >= '{1}' AND C.DocDate <= '{2}' ", StrEstadoOt, fdesde_local, fhasta_local)
                'strConsulta = mc_strConsultavehiculos_p1 & StrEstadoOt & mc_strConsultavehiculos_p2 & mc_strWhere1 & fdesde_local & mc_strWhere2 & fhasta_local & mc_strWhere3
            End If

            If Not String.IsNullOrEmpty(strCondiciones) Then
                'se concatenan las condicioens a la consulta
                strConsulta = strConsulta & strCondiciones
            Else
                strConsulta = strConsulta
            End If

            oMatrix.Clear()

            dbVehiculo.Clear()
            If Not String.IsNullOrEmpty(strConsulta) Then
                dbVehiculo.ExecuteQuery(strConsulta)
            End If
            oMatrix.LoadFromDataSource()
            'oform.Items.Item(mc_strCantidadRegistros).Specific.String = CStr(oMatrix.RowCount)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try

    End Function

    'metodo para cargar los datos que se ingresaron en los filtros,
    ' de este modo sus respectivas propiedades se usaran en la creacion 
    'de la consulta
    Public Sub ObtenerFiltros()
        Try
            Dim strNoUnidad_Local As String
            Dim strNoPlaca_Local As String
            Dim strNoOT_Local As String
            Dim strEstadoOT_Local As String
            Dim strDesde_Local As String
            Dim strHasta_Local As String

            strNoUnidad_Local = oForm.DataSources.DataTables.Item("ESTADOSOT").GetValue("NoUnidad", 0).ToString
            strNoUnidad_Local = strNoUnidad_Local.Trim()

            If Not String.IsNullOrEmpty(strNoUnidad_Local) Then
                strNoUnidad = (CStr(strNoUnidad_Local))
            Else
                strNoUnidad = String.Empty
            End If

            strNoPlaca_Local = oForm.DataSources.DataTables.Item("ESTADOSOT").GetValue("NoPlaca", 0).ToString
            strNoPlaca_Local = strNoPlaca_Local.Trim()

            If Not String.IsNullOrEmpty(strNoPlaca_Local) Then
                strNoPlaca = (CStr(strNoPlaca_Local))
            Else
                strNoPlaca = String.Empty
            End If

            strNoOT_Local = oForm.DataSources.DataTables.Item("ESTADOSOT").GetValue("NoOT", 0).ToString
            strNoOT_Local = strNoOT_Local.Trim()

            If Not String.IsNullOrEmpty(strNoOT_Local) Then
                strNoOt = (strNoOT_Local)
            Else
                strNoOt = String.Empty
            End If

            strEstadoOT_Local = oForm.DataSources.DataTables.Item("ESTADOSOT").GetValue("EstadoOT", 0).ToString
            strEstadoOT_Local = strEstadoOT_Local.Trim()

            If Not String.IsNullOrEmpty(strEstadoOT_Local) Then
                Select Case strEstadoOT_Local
                    Case "1"
                        StrEstadoOt = "Facturada"
                    Case "2"
                        StrEstadoOt = "Cerrada"
                End Select
            Else
                StrEstadoOt = String.Empty
            End If

            strDesde_Local = oForm.DataSources.DataTables.Item("ESTADOSOT").GetValue("fDesde", 0)
            strDesde_Local = strDesde_Local.Trim()

            If Not String.IsNullOrEmpty(strDesde_Local) Then
                StrDesde = Date.Parse(strDesde_Local)
            Else
                StrDesde = String.Empty
            End If

            strHasta_Local = oForm.DataSources.DataTables.Item("ESTADOSOT").GetValue("fHasta", 0)
            strHasta_Local = strHasta_Local.Trim()

            If Not String.IsNullOrEmpty(strHasta_Local) Then
                StrHasta = Date.Parse(strHasta_Local)
            Else
                StrHasta = String.Empty
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    'devuelve el codigo del vehiculo para levantar el mantenimiento de esta unidad
    Public Function DevolverCodeVehiculo(ByVal p_intFila As Integer, ByVal p_strFormID As String) As String

        Dim oForm As SAPbouiCOM.Form
        'Dim oMatriz As SAPbouiCOM.Matrix
        Dim strIDVehiculo As String

        oForm = ApplicationSBO.Forms.Item(p_strFormID)
        'oMatriz = DirectCast(oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)


        'intFila = oMatriz.GetNextSelectedRow()
        strIDVehiculo = oForm.DataSources.DataTables.Item("DT_VEH").GetValue(9, p_intFila - 1)

        Return strIDVehiculo

    End Function

    'devuelve el codigo de la cotizacion para levantar el mantenimiento
    Public Sub ObtenerIdCotizacion(ByVal fila As Integer, ByVal formID As String, ByRef p_oMatrix As SAPbouiCOM.Matrix)

        Dim oForm As SAPbouiCOM.Form

        oForm = ApplicationSBO.Forms.Item(formID)

        p_oMatrix.LoadFromDataSource()

        If fila > 0 And p_oMatrix.RowCount > 0 Then
            IdCotizacion = oForm.DataSources.DataTables.Item("DT_VEH").GetValue(0, fila - 1)
            strNoOt = oForm.DataSources.DataTables.Item("DT_VEH").GetValue(1, fila - 1)
        Else
            IdCotizacion = 0
            strNoOt = 0
        End If


    End Sub

    'se asocian los campos de texto con un campo en una tabla, para hacer uso de la misma
    'al referenciar el campo
    Private Sub LinkearCamposTexto()
        'Para linkear edittext de interfaz se utilizan datatables
        Dim datatable As SAPbouiCOM.DataTable = oForm.DataSources.DataTables.Add("ESTADOSOT")
        datatable.Columns.Add(UID:="NoUnidad", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
        datatable.Columns.Add(UID:="NoPlaca", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
        datatable.Columns.Add(UID:="NoOT", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
        datatable.Columns.Add(UID:="EstadoOT", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_AlphaNumeric)
        datatable.Columns.Add(UID:="fDesde", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_Date)
        datatable.Columns.Add(UID:="fHasta", ColFieldType:=SAPbouiCOM.BoFieldsType.ft_Date)
        datatable.Rows.Add(1)
        datatable.SetValue(Column:="NoUnidad", rowIndex:=0, Value:="")
        datatable.SetValue(Column:="NoPlaca", rowIndex:=0, Value:="")
        datatable.SetValue(Column:="NoOT", rowIndex:=0, Value:="")
        datatable.SetValue(Column:="EstadoOT", rowIndex:=0, Value:="")
        datatable.SetValue(Column:="fDesde", rowIndex:=0, Value:=Now.AddYears(-1).ToString("yyyyMMdd"))
        datatable.SetValue(Column:="fHasta", rowIndex:=0, Value:=Date.Now.ToString("yyyMMdd"))

        Dim item As Item
        Dim txt As EditText
        Dim cbo As ComboBox

        item = oForm.Items.Item("txtNoUnid")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="ESTADOSOT", columnUid:="NoUnidad")

        item = oForm.Items.Item("txtNoPla")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="ESTADOSOT", columnUid:="NoPlaca")

        item = oForm.Items.Item("txtNoOT")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="ESTADOSOT", columnUid:="NoOT")

        item = oForm.Items.Item("cboEstado")
        cbo = DirectCast(item.Specific, ComboBox)
        cbo.DataBind.Bind(UID:="ESTADOSOT", columnUid:="EstadoOT")
        cbo.ValidValues.Add(1, My.Resources.Resource.Facturada)
        cbo.ValidValues.Add(2, My.Resources.Resource.Cerrada)
        cbo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index)

        item = oForm.Items.Item("dtFDesde")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="ESTADOSOT", columnUid:="fDesde")

        item = oForm.Items.Item("dtFHasta")
        txt = DirectCast(item.Specific, EditText)
        txt.DataBind.Bind(UID:="ESTADOSOT", columnUid:="fHasta")

    End Sub

    Private Sub LinkearMatriz()
        'datatable que es la matriz de vehiculos
        Dim dtVehiculos As DataTable = oForm.DataSources.DataTables.Add(strDT)
        dtVehiculos.Columns.Add(UID:="cotizacion", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="ot", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="unidad", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="placa", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="vin", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="marca", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="estilo", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="modelo", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="ano", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)
        dtVehiculos.Columns.Add(UID:="numV", ColFieldType:=BoFieldsType.ft_AlphaNumeric, DataSize:=100)

        'Instancia de la matriz de vehiculos, con la tabla tVehiculos
        MatrizVehiculos = New MatrizVehi("mtxVehi", oForm, strDT)
        MatrizVehiculos.CreaColumnas()
        MatrizVehiculos.LigaColumnas()

    End Sub

    Private Function EnlazaColumnasMatrixaDatasource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean

        Dim oColumna As SAPbouiCOM.Column

        Try
            oColumna = oMatrix.Columns.Item("Col_NoOT")
            oColumna.DataBind.Bind(UID:=strDT, columnUid:="U_SCGD_Numero_OT")

            oColumna = oMatrix.Columns.Item("Col_Unid")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Cod_Unidad")

            oColumna = oMatrix.Columns.Item("Col_Placa")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Num_Placa")

            oColumna = oMatrix.Columns.Item("Col_VIN")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Num_VIN")

            oColumna = oMatrix.Columns.Item("Col_Marca")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Des_Marc")

            oColumna = oMatrix.Columns.Item("Col_Estilo")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Des_Esti")

            oColumna = oMatrix.Columns.Item("Col_Modelo")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Des_Mode")

            oColumna = oMatrix.Columns.Item("Col_Ano")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Ano_Vehi")

            oColumna = oMatrix.Columns.Item("Col_NumV")
            oColumna.DataBind.SetBound(True, strDT, "U_SCGD_Num_Vehiculo")

            Return True
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False

        End Try

    End Function

#End Region

#Region "Eventos"

    Public Sub ManejadorEventosItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)

        Try
            Dim oMatrix As SAPbouiCOM.Matrix
            'obtengo el form del que sucedio el evento
            oForm = ApplicationSBO.Forms.Item(FormUID)
            'nombre de base de datos DMS
            Dim strNombreTaller As String = ""
            
            If Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess Then
                'Manejo de formulario
                oForm.Freeze(True)
                Select Case pVal.ItemUID
                    Case "btnAct"
                        'actualizo la matriz
                        m_dbVehiculos = oForm.DataSources.DataTables.Item(strDT)
                        oMatrix = DirectCast(oForm.Items.Item(strMatrizVehiculos).Specific, Matrix)
                        Call CargarMatriz(oMatrix, oForm, m_dbVehiculos)
                    Case "btnEnt"
                        
                        'Creo el objeto sap de tipo cotizacion
                        Dim objSAP As SAPbobsCOM.Documents

                        Dim m_strCodeOT As String

                        objSAP = CType(CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
                        If Not String.IsNullOrEmpty(IdCotizacion) _
                            And Not String.IsNullOrEmpty(strNoOt) Then
                            'obtengo el objeto deseado
                            objSAP.GetByKey(IdCotizacion)
                            If objSAP IsNot Nothing Then
                                Utilitarios.DevuelveNombreBDTaller(ApplicationSBO, objSAP.UserFields.Fields.Item("U_SCGD_idSucursal").Value, strNombreTaller)
                                'actualizo la propiedad deseada
                                objSAP.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = "Entregada"
                                objSAP.UserFields.Fields.Item("U_SCGD_FEnt").Value = DateTime.Now()
                                objSAP.Update()

                                If Not Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO) Then
                                    'cambia ot del externo en Entregada
                                    Call Utilitarios.EjecutarConsulta("UPDATE [SCGTA_TB_Orden] SET Estado = 8 WHERE NoOrden = '" & strNoOt & "'", strNombreTaller, CompanySBO.Server)
                                Else

                                    Dim dtConsulta As SAPbouiCOM.DataTable
                                    Dim m_strEstado As String = String.Empty

                                    dtConsulta = oForm.DataSources.DataTables.Item("dtConsulta")

                                    dtConsulta.ExecuteQuery(" select Name from [@SCGD_ESTADOS_OT] where code = '8' ")
                                    m_strEstado = dtConsulta.GetValue(0, 0).ToString().Trim()

                                    dtConsulta.ExecuteQuery(String.Format(" select Code from [@SCGD_OT] where U_NoOT = '{0}' ", strNoOt))

                                    m_strCodeOT = dtConsulta.GetValue(0, 0).ToString().Trim()

                                    If Not String.IsNullOrEmpty(m_strCodeOT) Then
                                        Dim oCompanyService As SAPbobsCOM.CompanyService
                                        Dim oGeneralService As SAPbobsCOM.GeneralService
                                        Dim oGeneralData As SAPbobsCOM.GeneralData
                                        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

                                        oCompanyService = CompanySBO.GetCompanyService()
                                        oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
                                        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                                        oGeneralParams.SetProperty("Code", m_strCodeOT)
                                        oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                                        Dim fhaActual As DateTime
                                        fhaActual = Utilitarios.RetornaFechaActual(CompanySBO.CompanyDB, CompanySBO.Server)

                                        oGeneralData.SetProperty("U_FEntr", fhaActual)
                                        oGeneralData.SetProperty("U_DEstO", m_strEstado)
                                        oGeneralData.SetProperty("U_EstO", "8")

                                        oGeneralService.Update(oGeneralData)
                                    End If
                                End If

                                'actualizo la matriz
                                m_dbVehiculos = oForm.DataSources.DataTables.Item(strDT)
                                oMatrix = DirectCast(oForm.Items.Item(strMatrizVehiculos).Specific, SAPbouiCOM.Matrix)
                                Call CargarMatriz(oMatrix, oForm, m_dbVehiculos)
                                IdCotizacion = ""
                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ActualizacionEstadosOT, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                            Else
                                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorEstadosOT, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            End If
                        Else
                            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorSeleccionEstadosOT, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    Case "mtxVehi"
                        'obtengo el id cotizacion
                        oMatrix = DirectCast(oForm.Items.Item(strMatrizVehiculos).Specific, SAPbouiCOM.Matrix)
                        ObtenerIdCotizacion(pVal.Row, pVal.FormUID, oMatrix)
                End Select

                'Manejo de formulario
                oForm.Freeze(False)
            End If

        Catch ex As Exception
            'manejo de errores
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

#End Region

End Class
