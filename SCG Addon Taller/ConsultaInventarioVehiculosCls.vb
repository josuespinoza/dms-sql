Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports System.Globalization
Imports SCG.SBOFramework
Imports SCG.DMSOne.Framework.MenuManager
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI

Partial Public Class ConsultaInventarioVehiculosCls : Implements IFormularioSBO

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company
    Public n As NumberFormatInfo
    Private oDataTableConfiguracionesDMS As SAPbouiCOM.DataTable

    Private Const mc_strUIDVehiculos As String = "SCGD_MNO"
    Private Const mc_strUIDInventario As String = "SCGD_IVH"

    Private Const mc_strSCG_VEHICULO As String = "DT_CARS"

    'Matriz
    Private Const mc_strMatriz As String = "mtx_0"

    'Nombres de los campos de texto
    Public Const mc_strMarcaMotor As String = "cboMarcM"
    Public Const mc_strCategoria As String = "cboCat"
    Public Const mc_strDisponibilidad As String = "cboDisp"
    Public Const mc_strUbicacion As String = "cboUbi"
    Public Const mc_strStatus As String = "cboSta"
    Public Const mc_strTraccion As String = "cboTrac"
    Public Const mc_strCombustible As String = "cboComb"
    Public Const mc_strColor As String = "cboCol"
    Public Const mc_strColorTapiceria As String = "cboTap"
    Public Const mc_strTipo As String = "cboTipo"
    Public Const mc_strCabina As String = "cboCab"
    Public Const mc_strTecho As String = "cboTech"
    Public Const mc_strCarroceria As String = "cboCarr"
    Public Const mc_strTransmision As String = "cboTrans"
    Public Const mc_strMarca As String = "cboMarca"
    Public Const mc_strModelo As String = "cboModelo"
    Public Const mc_strEstilo As String = "cboEst"
    Public Const mc_strAno As String = "txtAño"
    Public Const mc_strCantidadRegistros As String = "2"
    Public Const mc_strNoUnidad As String = "txtNoUnid"
    Public Const mc_strComentarios As String = "txtComent"
    Public Const mc_strCboEstadoNuevo As String = "cboClase"
    Public Const mc_StrCboClasificacion As String = "cboClasif"
    Public Const mc_strCantidadDias As String = "txtDiasInv"
    Public Const mc_strPlaca As String = "txtPlaca"
    Public Const mc_strMatrixPed As String = "mtxPedidos"

    Public Const mc_strU_Cod_Marc As String = "U_Cod_Marc"
    Public Const mc_strU_Cod_MarcPed As String = "ESP.U_Cod_Marca"
    Public Const mc_strU_Cod_Mode As String = "U_Cod_Mode"
    Public Const mc_strU_Cod_ModePed As String = "U_Cod_Modelo"
    Public Const mc_strU_Cod_Esti As String = "U_Cod_Esti"
    Public Const mc_strU_Cod_EstiPed As String = "ESP.U_Cod_Estilo"
    Public Const mc_strU_Ano_Vehi As String = "U_Ano_Vehi"
    Public Const mc_strU_Cod_Col As String = "U_Cod_Col"
    Public Const mc_strU_ColorTap As String = "U_ColorTap"
    Public Const mc_strU_MarcaMot As String = "U_MarcaMot"
    Public Const mc_strU_MarcaMotPed As String = "U_Marca_Mot"
    Public Const mc_strU_Tipo As String = "U_Tipo"
    Public Const mc_strU_Tipo_Tra As String = "U_Tipo_Tra"
    Public Const mc_strU_Tipo_TraPed As String = "U_Tipo_Trac"
    Public Const mc_strU_TipTecho As String = "U_TipTecho"
    Public Const mc_strU_TipTechoPed As String = "U_Tipo_Techo"
    Public Const mc_strU_Carrocer As String = "U_Carrocer"
    Public Const mc_strU_CarrocerPed As String = "U_Carroceria"

    Public Const mc_strU_Categori As String = "U_Categori"
    Public Const mc_strU_CategoriPed As String = "U_Categoria"
    Public Const mc_strU_Combusti As String = "U_Combusti"
    Public Const mc_strU_Tip_Cabi As String = "U_Tip_Cabi"
    Public Const mc_strU_Tip_CabiPed As String = "U_Tipo_Cabina"
    Public Const mc_strU_Transmis As String = "U_Transmis"
    Public Const mc_strU_Dispo As String = "U_Dispo"
    Public Const mc_strU_Estatus As String = "ESTA.Code"
    Public Const mc_strU_Cod_Ubic As String = "U_Cod_Ubic"
    Public Const mc_strU_Cod_Unid As String = "U_Cod_Unid"
    Public Const mc_strU_Comentarios As String = "U_OBSRES"
    Public Const mc_strU_EstadoNuevo As String = "U_Estado_Nuevo"
    Public Const mc_strU_CodClasificacion As String = "U_Clasificacion"
    Public Const mc_strU_NumPlaca As String = "U_Num_Plac"
    Public Const mc_strU_Fha_Ing_Inv As String = "U_Fha_Ing_Inv"
    Public Const mc_strCodYearPed As String = "U_ano_veh"

    Public Const mc_strDTPedidos As String = "dtPedidos"

    Private m_strU_Cod_Marc As String
    Private m_strU_Cod_Mode As String
    Private m_strU_Cod_Esti As String
    Private m_strU_Ano_Vehi As Integer
    Private m_strU_Cod_Col As String
    Private m_strU_ColorTap As String
    Private m_strU_MarcaMot As String
    Private m_strU_Tipo As String
    Private m_strU_Tipo_Tra As String
    Private m_strU_TipTecho As String
    Private m_strU_Carrocer As String
    Private m_strU_Categori As String
    Private m_strU_Combusti As String
    Private m_strU_Tip_Cabi As String
    Private m_strU_Transmis As String
    Private m_strU_Dispo As String
    Private m_strU_Estatus As String
    Private m_strU_Cod_Ubic As String
    Private m_strU_Cod_Unid As String
    Private m_strU_Comentarios As String

    Private m_strEstadoNuevo As String
    Private m_strCodClasificacion As String
    Private m_strU_NumPlaca As String
    Private m_strU_Fha_Ing_Inv As String
    Private m_strCantidadDias As String

    Private m_dbInventario As SAPbouiCOM.DataTable
    Private m_dtPedidos As SAPbouiCOM.DataTable
    Private m_oForm As SAPbouiCOM.Form
    Private SBO_Application As SAPbouiCOM.Application

    Private Const mc_strFolderInventario As String = "fldInv"
    Private Const mc_strFolderPedidos As String = "fldPed"


    Private Const mc_strConsultavehiculos As String = "VEH.Code, VEH.U_Cod_Unid, VEH.U_Num_VIN, VEH.U_Num_Mot, VEH.U_Des_Marc, VEH.U_Des_Esti, VEH.U_Des_Mode, VEH.U_CardName, DATEDIFF(DD, VEH.U_Fha_Ing_Inv, GETDATE()), MAMO.Name as U_MarcaMot," &
                            "TR.Name as U_Transmis,TRA.Name as U_Tipo_Tra,COMB.Name as U_Combusti,TECH.Name as U_TipTecho,UBI.Name as U_Cod_Ubic," &
                            "TIPVEH.Name as U_Tipo, DISPO.Name as U_Dispo,VEH.U_Des_Col,CO.Name as U_ColorTap,CARR.Name as U_Carrocer,CABI.Name as U_Tip_Cabi," &
                            "CATEVEH.Name as U_Categori,VEH.U_Ano_Vehi,Esta.Name as U_Estado, Convert(Char(10), VEH.U_FCHRES, 101) as U_FecArri, Convert(Char(10), VEH.U_FchRsva, 101) as U_FecRes, " &
                            "Convert(Char(10), VEH.U_FchVcRva, 101) as U_FecVenc, VEH.U_VENRES, VEH.U_Moneda, VEH.U_Precio, VEH.U_ValorNet, VEH.U_Bono,  VEH.U_Num_Plac ," &
                            "Case When VEH.U_Dispo <> 1 then 1 else 0 end as Reservado " &
                            "FROM [@SCGD_VEHICULO] as VEH with(nolock) " &
                            "LEFT OUTER JOIN [@SCGD_MARCA_MOTOR] MAMO with(nolock) on VEH.U_MarcaMot = MAMO.Code " &
                            "LEFT OUTER JOIN [@SCGD_TRANSMISION] TR with(nolock) on VEH.U_Transmis = TR.Code " &
                            "LEFT OUTER JOIN [@SCGD_TRACCION] TRA with(nolock) on VEH.U_Tipo_Tra = TRA.Code " &
                            "LEFT OUTER JOIN [@SCGD_COMBUSTIBLE] COMB with(nolock) on VEH.U_Combusti = COMB.Code " &
                            "LEFT OUTER JOIN [@SCGD_TECHO] TECH with(nolock) on VEH.U_TipTecho = TECH.Code " &
                            "LEFT OUTER JOIN [@SCGD_UBICACIONES] UBI with(nolock) on VEH.U_Cod_Ubic = UBI.Code " &
                            "LEFT OUTER JOIN [@SCGD_TIPOVEHICULO] TIPVEH with(nolock) on VEH.U_Tipo = TIPVEH.Code " &
                            "LEFT OUTER JOIN [@SCGD_DISPONIBILIDAD] DISPO with(nolock) on VEH.U_Dispo = DISPO.Code " &
                            "LEFT OUTER JOIN [@SCGD_COLOR] CO with(nolock) on VEH.U_ColorTap = CO.Code " &
                            "LEFT OUTER JOIN [@SCGD_CARROCERIA] CARR with(nolock) on VEH.U_Carrocer = CARR.Code " &
                            "LEFT OUTER JOIN [@SCGD_CABINA] CABI with(nolock) on VEH.U_Tip_Cabi = CABI.Code " &
                            "LEFT OUTER JOIN [@SCGD_CATEGORIA_VEHI] CATEVEH with(nolock) on VEH.U_Categori = CATEVEH.Code " &
                            "LEFT OUTER JOIN [@SCGD_ESTADO] ESTA with(nolock) on VEH.U_Estatus = Esta.Code " &
                            " where 1=1 "

    Private Const mc_strConsultaPedidos As String = "PL.U_Cod_Art col_code, PL.U_Desc_Art col_desc, PL.U_ano_veh col_year,  col.Name col_color, " &
                            "Case when PL.U_Cant <  pl.U_Cant_Rec then 0 else (PL.U_Cant - isnull(pl.U_Cant_Rec, 0)) end col_qtyP, isnull(PL.U_Cant_Rec, 0) col_qtyR," & _
                            "trac.Name col_trac, trans.Name col_tran, convert(varchar, PE.U_Fha_Est_Arribo, {0}) col_feca,  com.Name col_comb, " & _
                            "esp.U_Cod_Marca U_Cod_Marc, esp.U_Cod_Modelo  U_Cod_Mode, esp.U_Cod_Estilo U_Cod_Esti, ESP.U_Marca_Mot U_MarcaMot," & _
                            "ESP.U_Tipo_Trac  U_Tipo_Tra, ESP.U_Tipo_Techo U_TipTecho, ESP.U_Carroceria U_Carrocer, ESP.U_Categoria U_Categori, " & _
                            "ESP.U_Combusti, ESP.U_Tipo_Cabina U_Tip_cabi, ESP.U_Transmis, PE.DocEntry as Pedido " & _
                            "From [@SCGD_PEDIDOS] PE with(nolock) " & _
                            "INNER JOIN [@SCGD_PEDIDOS_LINEAS] as PL with(nolock) on PE.DocEntry = PL.DocEntry " & _
                            "LEFT OUTER JOIN [@SCGD_CONF_ART_VENTA] as CAV with(nolock) on CAV.U_ArtVent = PL.U_Cod_Art " & _
                            "LEFT OUTER JOIN [@SCGD_ESPEXMODE] as ESP with(nolock) on  ESP.U_Cod_MarComer = CAV.Name " & _
                            "LEFT OUTER JOIN [@SCGD_MARCA] as marc with(nolock) on  ESP.U_Cod_Marca = marc.Code " & _
                            "LEFT OUTER JOIN [@SCGD_ESTILO] as EST with(nolock) on  ESP.U_Cod_Estilo = EST.Code and ESP.U_Cod_Marca = EST.U_Cod_Marc " & _
                            "LEFT OUTER JOIN [@SCGD_MODELO] as mode with(nolock) on  ESP.U_Cod_Modelo = mode.Code and ESP.U_Cod_Estilo = mode.U_Cod_Esti " & _
                            "LEFT OUTER JOIN [@SCGD_COMBUSTIBLE] com with(nolock) on ESP.U_Combusti = com.Code " & _
                            "LEFT OUTER JOIN [@SCGD_TRACCION] trac with(nolock) on ESP.U_Tipo_Trac = trac.Code " & _
                            "LEFT OUTER JOIN [@SCGD_TRANSMISION] trans with(nolock) on ESP.U_Transmis = trans.Code " & _
                            "LEFT OUTER JOIN [@SCGD_COLOR] col with(nolock) on PL.U_Cod_Col = col.Code " & _
                            " WHERE PL.U_Cod_Art is not null and PL.U_Cod_Art <> '' "

    Private m_objConfiguracionesGenerales As SCGDataAccess.ConfiguracionesGeneralesAddon
    Private m_lstReservar As Generic.List(Of Utilitarios.ListadoValidValues)
    Dim objComentarioR As ComentariosInventarioV
#End Region

#Region "Constructor"

    Public Sub New(ByRef p_SBO_Aplication As Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania
        n = DIHelper.GetNumberFormatInfo(m_oCompany)
    End Sub

#End Region

#Region "... Inicializacion de Controles ..."

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then
            CargaFormulario()
        End If

    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        'Manejo de formulario
        FormularioSBO.Freeze(True)

        'cboTipoOtInterna = New ComboBoxSBO("cboTipOtIn", FormularioSBO, True, "", "")
        'CargarTiposOtEspeciales()

        'Manejo de formulario
        FormularioSBO.Freeze(False)
    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String

        If Utilitarios.MostrarMenu(mc_strUIDInventario, SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu(mc_strUIDInventario, SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDInventario, BoMenuType.mt_STRING, strEtiquetaMenu, 15, False, True, mc_strUIDVehiculos))
        End If

    End Sub

    Protected Friend Sub CargarFormulario()
        '*******************************************************************    
        'Propósito: Se encarga de establecer los filtros para los eventos de la
        '            aplicacion que se van a manejar y posteriormente se los
        '            agrega al objeto aplicacion donde se esta almacenando la
        '            aplicacion SBO que esta corriendo
        '
        'Acepta:    Ninguno
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 19 Abril 2006
        '********************************************************************
        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oMatrixPed As SAPbouiCOM.Matrix
            Dim strXMLACargar As String
            Dim cn_Coneccion As New SqlClient.SqlConnection
            Dim strConectionString As String = String.Empty

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_INV_VEHI"
            fcp.UniqueID = "SCGD_INV_VEHI"

            strXMLACargar = My.Resources.Resource.FormInventarioVehiculos
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oForm = SBO_Application.Forms.AddEx(fcp)

            m_oForm.Freeze(True)
            Call CargarCombos()

            oDataTableConfiguracionesDMS = m_oForm.DataSources.DataTables.Add("ConfigsDMS")

            oMatrix = DirectCast(m_oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)
            oMatrixPed = DirectCast(m_oForm.Items.Item(mc_strMatrixPed).Specific, SAPbouiCOM.Matrix)

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            If cn_Coneccion.State = ConnectionState.Open Then
                cn_Coneccion.Close()
            End If
            cn_Coneccion.ConnectionString = strConectionString
            m_objConfiguracionesGenerales = New SCGDataAccess.ConfiguracionesGeneralesAddon(cn_Coneccion)

            FormularioSBO = m_oForm
            Call InicializaFormulario()
            MostrarControlesInv(m_oForm.UniqueID, True)
            m_oForm.Freeze(False)
            'End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub CargarCombos()
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_MARCA] with(nolock) Order by Name", mc_strMarca)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_TRACCION] with(nolock) Order by Name", mc_strTraccion)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_COMBUSTIBLE] with(nolock) Order by Name", mc_strCombustible)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_MARCA_MOTOR] with(nolock)  Order by Name", mc_strMarcaMotor)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_TECHO] with(nolock) Order by Name", mc_strTecho)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_COLOR] with(nolock) Order by Name", mc_strColor)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_COLOR] with(nolock) Order by Name", mc_strColorTapiceria)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_CABINA] with(nolock) Order by Name", mc_strCabina)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_TIPOVEHICULO] with(nolock) Order by Name", mc_strTipo)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_CARROCERIA] with(nolock) Order by Name", mc_strCarroceria)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_TRANSMISION] with(nolock) Order by Name", mc_strTransmision)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_CATEGORIA_VEHI] with(nolock) Order by Name", mc_strCategoria)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_DISPONIBILIDAD] with(nolock) Order by Name", mc_strDisponibilidad)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_UBICACIONES] with(nolock) Order by Name", mc_strUbicacion)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,Name From [@SCGD_ESTADO] with(nolock) Order by Name", mc_strStatus)
        Call CargarValidValuesEnCombos(m_oForm, "Select Code,U_Desc From [@SCGD_CLASIFICACION] with(nolock) order by U_Desc", mc_StrCboClasificacion)

        oItem = m_oForm.Items.Item("cboClase")
        cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

        If cboCombo.ValidValues.Count = 0 Then
            cboCombo.ValidValues.Add("N", My.Resources.Resource.Valor_Vehiculo_Nuevo)
            cboCombo.ValidValues.Add("U", My.Resources.Resource.Valor_Vehiculo_Usado)
            cboCombo.ValidValues.Add("--", My.Resources.Resource.Valor_Vehiculo_Ninguno)
            cboCombo.Select("--")
        End If

        oItem = m_oForm.Items.Item(mc_strMarca)
        cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)
        cboCombo.Select("--")

        oMatrix = DirectCast(m_oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)
        m_lstReservar = CrearListadoValidValuesReservar()

        Call Utilitarios.CargarValidValuesEnCombos(oMatrix.Columns.Item("col_Res").ValidValues, m_lstReservar)
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

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If

        Return oXMLDoc.InnerXml

    End Function

    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                      ByVal strQuery As String, _
                                                      ByRef strIDItem As String)
        '*******************************************************************    
        'Propósito: Se encarga de cargar los values y descriptions de los 
        '           combos que utilizan catalogos en UserTables.
        'Acepta:    oForm As SAPbouiCOM.Form,
        '           ByVal strQuery As String,
        '           ByRef strIDItem As String
        '
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 21 Nov 2006
        '********************************************************************

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

            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            'Agrega los ValidValues
            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    Dim strDscpModelo As String = drdResultadoConsulta.GetString(1).Trim
                    If strDscpModelo.Length > 60 Then
                        Dim strDescripcion As String = strDscpModelo.Substring(0, 60)
                        cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, strDescripcion)
                    Else
                        cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, strDscpModelo)
                    End If
                    'cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, drdResultadoConsulta.GetString(1).Trim)
                End If
            Loop

            If intRecIndex = 0 Then
                Select Case oItem.UniqueID
                    Case mc_strEstilo
                        Call LimpiarValidValusCombo(oForm, mc_strModelo)
                    Case mc_strMarca
                        Call LimpiarValidValusCombo(oForm, mc_strEstilo)
                        Call LimpiarValidValusCombo(oForm, mc_strModelo)
                End Select
            End If

            cboCombo.ValidValues.Add("--", My.Resources.Resource.Ninguno)
            cboCombo.Select("--")

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        Finally
            cn_Coneccion.Close()
        End Try

    End Sub

    Private Sub LimpiarValidValusCombo(ByRef oForm As SAPbouiCOM.Form, _
                                   ByVal p_strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        oItem = oForm.Items.Item(p_strIDItem)
        cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

        If cboCombo.ValidValues.Count > 0 Then
            For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
            Next
        End If

    End Sub

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oMatrix As SAPbouiCOM.Matrix
            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing _
                    AndAlso pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "btnRefresh"
                        m_oForm = SBO_Application.Forms.Item("SCGD_INV_VEHI")
                        m_dbInventario = m_oForm.DataSources.DataTables.Item(mc_strSCG_VEHICULO)
                        If m_oForm.PaneLevel = 1 Then
                            oMatrix = DirectCast(m_oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)
                            Call CargarMatrixInv(oMatrix, m_oForm, m_dbInventario)
                        ElseIf m_oForm.PaneLevel = 2 Then
                            oMatrix = DirectCast(m_oForm.Items.Item(mc_strMatrixPed).Specific, SAPbouiCOM.Matrix)
                            Call CargarMatrixPed(oMatrix, m_oForm, m_dtPedidos)
                        End If
                    Case "btnClose"
                        m_oForm = SBO_Application.Forms.Item("SCGD_INV_VEHI")
                        m_oForm.Close()
                    Case "btnClean"
                        m_oForm.Freeze(True)
                        'm_oForm.Close()
                        Call SeleccionarValorDefecto(oForm, True)
                        'Call CargarFormulario()
                        m_oForm.Freeze(False)
                    Case "btnPrint"
                        Call ImprimirReporteInventario("SCGD_INV_VEHI", pVal, BubbleEvent)
                    Case mc_strMatriz
                        ' ReservaVehiculo(m_oForm, pVal, BubbleEvent)
                End Select

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ReservaVehiculo(ByVal oForm As SAPbouiCOM.Form, ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        oForm.Freeze(True)
        Try
            Dim oEditComentarios As SAPbouiCOM.EditText
            Dim oEditClienteIV As SAPbouiCOM.EditText
            Dim oEditV As SAPbouiCOM.EditText
            Dim oComboReserva As SAPbouiCOM.ComboBox
            Dim oDtLocal As DataTable
            If pVal.Row > 0 AndAlso pVal.ColUID = "col_Res" Then
                Dim oMatrix As SAPbouiCOM.Matrix
                oMatrix = DirectCast(m_oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)
                m_dbInventario.Rows.Clear()
                oMatrix.FlushToDataSource()
                m_dbInventario = FormularioSBO.DataSources.DataTables.Item(mc_strSCG_VEHICULO)

                oComboReserva = DirectCast(oMatrix.Columns.Item("col_Res").Cells.Item(pVal.Row).Specific, SAPbouiCOM.ComboBox)
                If oComboReserva.Selected.Value = "1" Then

                    oEditComentarios = DirectCast(oForm.Items.Item("txtH_RC").Specific, SAPbouiCOM.EditText)
                    oEditClienteIV = DirectCast(oForm.Items.Item("txtH_Cli").Specific, SAPbouiCOM.EditText)
                    oEditV = DirectCast(oForm.Items.Item("strVal").Specific, SAPbouiCOM.EditText)

                    objComentarioR = New ComentariosInventarioV(m_oCompany, SBO_Application)
                    objComentarioR.FormIV = oForm

                    oEditComentarios.Value = String.Empty

                    Dim strMensaje As String = objComentarioR.ShowInput("Cliente:")
                    If Not String.IsNullOrEmpty(oEditV.Value) Then
                        If oEditV.Value = "True" Then
                            Dim vehiCode = m_dbInventario.GetValue("Code", pVal.Row - 1).ToString
                            If Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_Disp_Res.Trim()) Then
                                If ReservacionVehiculo(vehiCode, oEditComentarios.Value.Trim(), oEditClienteIV.Value.Trim(), DMS_Connector.Configuracion.ParamGenAddon.U_Disp_Res.Trim()) Then
                                    CargarMatrixInv(oMatrix, m_oForm, m_dbInventario)
                                End If
                            End If
                        Else
                            m_dbInventario.SetValue("Reservado", pVal.Row - 1, "0")
                            BubbleEvent = False
                        End If
                    Else
                        m_dbInventario.SetValue("Reservado", pVal.Row - 1, "0")
                        BubbleEvent = False
                    End If
                Else
                    m_dbInventario.SetValue("Reservado", pVal.Row - 1, "1")
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.MSG_NoDesReserva, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    BubbleEvent = False
                End If
                oMatrix.LoadFromDataSource()
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            '
        End Try
        oForm.Freeze(False)
    End Sub

    Public Sub ValidaCambiosMatrix(ByVal oForm As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim setting As SAPbouiCOM.CommonSetting

        oMatrix = DirectCast(oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)
        setting = oMatrix.CommonSetting
        m_dbInventario.Rows.Clear()

        m_dbInventario = FormularioSBO.DataSources.DataTables.Item(mc_strSCG_VEHICULO)
        oMatrix.FlushToDataSource()

        For i As Integer = 0 To m_dbInventario.Rows.Count - 1
            If m_dbInventario.GetValue("Reservado", i).ToString.Trim() = "1" Then
                setting.SetRowEditable(i + 1, False)
            End If
        Next
    End Sub

    Public Function ReservacionVehiculo(ByVal srt_VehiCode As String, ByVal strComen As String, ByVal strClientID As String, ByVal strStatusReserva As String) As Boolean
        Dim result As Boolean = False
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try
            oCompanyService = m_oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", srt_VehiCode)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            'Asignación de los valores
            oGeneralData.SetProperty("U_FchRsva", DateTime.Now)
            oGeneralData.SetProperty("U_Dispo", strStatusReserva)
            If Not String.IsNullOrEmpty(strClientID) Then
                oGeneralData.SetProperty("U_VENRES", strClientID)
            End If
            If Not String.IsNullOrEmpty(strComen) Then
                Dim comentarios As String
                If strComen.Length >= 254 Then
                    comentarios = strComen.Substring(0, 254)
                Else
                    comentarios = strComen
                End If
                oGeneralData.SetProperty("U_OBSRES", comentarios)
            End If

            oGeneralService.Update(oGeneralData)
            result = True

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
        Return result
    End Function

    Public Sub ImprimirReporteInventario(ByVal FormUID As String, _
                                    ByRef pVal As SAPbouiCOM.ItemEvent, _
                                    ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = ""
        Dim strDBDMSOne As String = ""
        Dim strPathExe As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim CardCode As String = String.Empty

        strDBDMSOne = SBO_Application.Company.DatabaseName
        oForm = SBO_Application.Forms.Item(FormUID)

        Call PasarFiltrosAVariables()

        strParametros = String.Format("{0} U_Dispo!='{1}' AND ", strParametros, DMS_Connector.Configuracion.ParamGenAddon.U_Disp_V.Replace(" ", "°").Trim)
        strParametros = String.Format("{0} U_Tipo!='{1}'", strParametros, DMS_Connector.Configuracion.ParamGenAddon.U_Inven_V.Replace(" ", "°").Trim)

        If m_strU_Cod_Unid <> String.Empty Then
            strParametros = String.Format("{0} AND U_Cod_Unid='{1}'", strParametros, m_strU_Cod_Unid.Replace(" ", "°").Trim)
        End If

        If m_strU_Cod_Marc <> "--" Then
            strParametros = String.Format("{0} AND U_Cod_Marc='{1}'", strParametros, m_strU_Cod_Marc.Replace(" ", "°").Trim)
        End If

        If m_strU_Cod_Esti <> "--" Then
            strParametros = String.Format("{0} AND U_Cod_Esti='{1}'", strParametros, m_strU_Cod_Esti.Replace(" ", "°").Trim)
        End If

        If m_strU_Cod_Mode <> "--" Then
            strParametros = String.Format("{0} AND U_Cod_Mode='{1}'", strParametros, m_strU_Cod_Mode.Replace(" ", "°").Trim)
        End If

        If m_strU_MarcaMot <> "--" Then
            strParametros = String.Format("{0} AND U_MarcaMot='{1}'", strParametros, m_strU_MarcaMot.Replace(" ", "°").Trim)
        End If

        If m_strU_Transmis <> "--" Then
            strParametros = String.Format("{0} AND U_Transmis='{1}'", strParametros, m_strU_Transmis.Replace(" ", "°").Trim)
        End If

        If m_strU_Tipo_Tra <> "--" Then
            strParametros = String.Format("{0} AND U_Tipo_Tra='{1}'", strParametros, m_strU_Tipo_Tra.Replace(" ", "°").Trim)
        End If

        If m_strU_Combusti <> "--" Then
            strParametros = String.Format("{0} AND U_Combusti='{1}'", strParametros, m_strU_Combusti.Replace(" ", "°").Trim)
        End If

        If m_strU_TipTecho <> "--" Then
            strParametros = String.Format("{0} AND U_TipTecho='{1}'", strParametros, m_strU_TipTecho.Replace(" ", "°").Trim)
        End If

        If m_strU_Tipo <> "--" Then
            strParametros = String.Format("{0} AND U_Tipo='{1}'", strParametros, m_strU_Tipo.Replace(" ", "°").Trim)
        End If

        If Not String.IsNullOrEmpty(m_strU_Dispo) AndAlso IsNumeric(m_strU_Dispo) Then
            strParametros = String.Format("{0} AND U_Dispo={1}", strParametros, m_strU_Dispo.Replace(" ", "°").Trim)
        End If

        If m_strU_Cod_Col <> "--" Then
            strParametros = String.Format("{0} AND U_Cod_Col='{1}'", strParametros, m_strU_Cod_Col.Replace(" ", "°").Trim)
        End If

        If m_strU_ColorTap <> "--" Then
            strParametros = String.Format("{0} AND U_ColorTap='{1}'", strParametros, m_strU_ColorTap.Replace(" ", "°").Trim)
        End If

        If m_strU_Carrocer <> "--" Then
            strParametros = String.Format("{0} AND U_Carrocer = '{1}'", strParametros, m_strU_Carrocer.Replace(" ", "°").Trim)
        End If

        If m_strU_Tip_Cabi <> "--" Then
            strParametros = String.Format("{0} AND U_Tip_Cabi='{1}'", strParametros, m_strU_Tip_Cabi.Replace(" ", "°").Trim)
        End If

        If m_strU_Categori <> "--" Then
            strParametros = String.Format("{0} AND U_Categori='{1}'", strParametros, m_strU_Categori.Replace(" ", "°").Trim)
        End If

        If m_strU_Ano_Vehi > 0 Then
            strParametros = String.Format("{0} AND U_Ano_Vehi={1}", strParametros, m_strU_Ano_Vehi.ToString())
        End If

        If m_strU_Cod_Ubic <> "--" Then
            strParametros = String.Format("{0} AND U_Cod_Ubic='{1}'", strParametros, m_strU_Cod_Ubic.Replace(" ", "°").Trim)
        End If

        If m_strU_Estatus <> "--" Then
            strParametros = String.Format("{0} AND U_Estatus='{1}'", strParametros, m_strU_Estatus.Replace(" ", "°").Trim)
        End If

        CardCode = oForm.DataSources.UserDataSources.Item("CardCode").ValueEx
        If Not String.IsNullOrEmpty(CardCode) Then
            strParametros = String.Format("{0} AND U_CardCode = '{1}' ", strParametros, CardCode.Replace(" ", "°"))
        End If

        'If String.IsNullOrEmpty(strParametros) Then
        '    strParametros = "1=1"
        'Else
        '    strParametros = strParametros.Trim.TrimEnd(" ").Trim.TrimEnd("D").Trim.TrimEnd("N").Trim.TrimEnd("A") & " "
        'End If
        strParametros = strParametros.Replace(" ", "°").Replace("°°", "°")


        strDireccionReporte = m_objConfiguracionesGenerales.DireccionReportes & My.Resources.Resource.rptInventarioVehiculos & ".rpt"

        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= My.Resources.Resource.TituloInventarioVehiculos.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & m_oCompany.CompanyDB & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub

    Public Function CargarMatrixInv(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                       ByVal oform As SAPbouiCOM.Form, _
                                       ByVal dbInventarioVehiculo As SAPbouiCOM.DataTable) As Boolean

        Dim strCondiciones As String
        Dim CardCode As String = String.Empty

        Try
            Call PasarFiltrosAVariables()
            strCondiciones = ""

            strCondiciones &= " and " & "U_Activo" & " = " & "'Y'"

            If Not String.IsNullOrEmpty(m_strU_Cod_Unid) AndAlso m_strU_Cod_Unid <> "--" Then
                strCondiciones &= " and " & mc_strU_Cod_Unid & " = '" & m_strU_Cod_Unid & "'"

            End If

            If Not String.IsNullOrEmpty(m_strU_Cod_Marc) AndAlso m_strU_Cod_Marc <> "--" Then
                strCondiciones &= " and " & mc_strU_Cod_Marc & " = '" & m_strU_Cod_Marc & "'"

            End If

            If Not String.IsNullOrEmpty(m_strU_Cod_Mode) AndAlso m_strU_Cod_Mode <> "--" Then
                strCondiciones &= " and " & mc_strU_Cod_Mode & " = '" & m_strU_Cod_Mode & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Cod_Esti) AndAlso m_strU_Cod_Esti <> "--" Then
                strCondiciones &= " and " & mc_strU_Cod_Esti & " = '" & m_strU_Cod_Esti & "'"
            End If

            If m_strU_Ano_Vehi > -1 Then
                strCondiciones &= " and " & mc_strU_Ano_Vehi & " = " & m_strU_Ano_Vehi
            End If

            If Not String.IsNullOrEmpty(m_strU_Cod_Col) AndAlso m_strU_Cod_Col <> "--" Then
                strCondiciones &= " and " & mc_strU_Cod_Col & " = '" & m_strU_Cod_Col & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_ColorTap) AndAlso m_strU_ColorTap <> "--" Then
                strCondiciones &= " and " & mc_strU_ColorTap & " = '" & m_strU_ColorTap & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_MarcaMot) AndAlso m_strU_MarcaMot <> "--" Then
                strCondiciones &= " and " & mc_strU_MarcaMot & " = '" & m_strU_MarcaMot & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Tipo) AndAlso m_strU_Tipo <> "--" Then
                strCondiciones &= " and " & mc_strU_Tipo & " = '" & m_strU_Tipo & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Tipo_Tra) AndAlso m_strU_Tipo_Tra <> "--" Then
                strCondiciones &= " and " & mc_strU_Tipo_Tra & " = '" & m_strU_Tipo_Tra & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_TipTecho) AndAlso m_strU_TipTecho <> "--" Then
                strCondiciones &= " and " & mc_strU_TipTecho & " = '" & m_strU_TipTecho & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Carrocer) AndAlso m_strU_Carrocer <> "--" Then
                strCondiciones &= " and " & mc_strU_Carrocer & " = '" & m_strU_Carrocer & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Categori) AndAlso m_strU_Categori <> "--" Then
                strCondiciones &= " and " & mc_strU_Categori & " = '" & m_strU_Categori & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Combusti) AndAlso m_strU_Combusti <> "--" Then
                strCondiciones &= " and " & mc_strU_Combusti & " = '" & m_strU_Combusti & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Tip_Cabi) AndAlso m_strU_Tip_Cabi <> "--" Then
                strCondiciones &= " and " & mc_strU_Tip_Cabi & " = '" & m_strU_Tip_Cabi & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Transmis) AndAlso m_strU_Transmis <> "--" Then
                strCondiciones &= " and " & mc_strU_Transmis & " = '" & m_strU_Transmis & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Dispo) AndAlso m_strU_Dispo <> "--" Then
                strCondiciones &= " and " & mc_strU_Dispo & " = '" & m_strU_Dispo & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Cod_Ubic) AndAlso m_strU_Cod_Ubic <> "--" Then
                strCondiciones &= " and " & mc_strU_Cod_Ubic & " = '" & m_strU_Cod_Ubic & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Estatus) AndAlso m_strU_Estatus <> "--" Then
                strCondiciones &= " and " & mc_strU_Estatus & " = '" & m_strU_Estatus & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_Comentarios) Then

                strCondiciones &= " and " & mc_strU_Comentarios & " = '" & m_strU_Comentarios & "'"

            End If

            If Not String.IsNullOrEmpty(m_strEstadoNuevo) AndAlso m_strEstadoNuevo <> "--" Then

                strCondiciones &= " and " & mc_strU_EstadoNuevo & " = '" & m_strEstadoNuevo & "'"

            End If

            If Not String.IsNullOrEmpty(m_strCodClasificacion) AndAlso m_strCodClasificacion <> "--" Then
                strCondiciones &= " and " & mc_strU_CodClasificacion & " = '" & m_strCodClasificacion & "'"
            End If

            If Not String.IsNullOrEmpty(m_strU_NumPlaca) AndAlso m_strU_NumPlaca <> "--" Then
                strCondiciones &= " and " & mc_strU_NumPlaca & " like '%" & m_strU_NumPlaca & "%'"
            End If

            CardCode = oform.DataSources.UserDataSources.Item("CardCode").ValueEx
            If Not String.IsNullOrEmpty(CardCode) Then
                strCondiciones += String.Format(" AND U_CardCode = '{0}' ", CardCode)
            End If

            If Not String.IsNullOrEmpty(m_strCantidadDias) AndAlso m_strCantidadDias <> "--" Then

                Dim l_fhaActual As Date
                Dim l_strFhaActual As String

                l_fhaActual = ObternerFechaServer()
                l_strFhaActual = Utilitarios.RetornaFechaFormatoDB(l_fhaActual, m_oCompany.Server)

                strCondiciones &= " and (DATEDIFF(DD, " & mc_strU_Fha_Ing_Inv & ", '" & l_strFhaActual & "') >= " & m_strCantidadDias & ")"
            End If

            'Condicion para que muestre unicamente los vehiculos que se encuentra diferentes a "Vendidos o PostVenta"
            strCondiciones &= " and VEH.U_Dispo <> '" & DMS_Connector.Configuracion.ParamGenAddon.U_Disp_V & "'"
            strCondiciones &= " and VEH.U_Tipo <> '" & DMS_Connector.Configuracion.ParamGenAddon.U_Inven_V & "'"

            'Valida si es Carga Inicial de Pantalla o no para aplicarle el Select TOP
            strCondiciones = "SELECT " & mc_strConsultavehiculos & strCondiciones & " order by VEH.Code, VEH.U_Cod_Unid, VEH.U_Cod_Marc"

            dbInventarioVehiculo.Rows.Clear()
            dbInventarioVehiculo.ExecuteQuery(strCondiciones)
            oMatrix.Columns.Item(oMatrix.Columns.Count - 1).Editable = True
            oMatrix.LoadFromDataSource()
            oform.Items.Item(mc_strCantidadRegistros).Specific.String = CStr(oMatrix.RowCount)
            Dim versionSap As Integer
            versionSap = m_oCompany.Version
            If versionSap >= 900000 Then
                ValidaCambiosMatrix(oform)
            End If

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Public Function ObternerFechaServer() As DateTime
        Try
            Dim l_fhaActual As DateTime

            l_fhaActual = Utilitarios.EjecutarConsulta("select GETDATE()", m_oCompany.CompanyDB, m_oCompany.Server)

            Return l_fhaActual
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Function

    Private Sub PasarFiltrosAVariables()

        Dim oCombo As SAPbouiCOM.ComboBox
        Dim strAno As String
        Dim strNoUnidad As String
        Dim strNumPlaca As String
        Dim strComentarios As String
        Dim strCantDias As String

        strNoUnidad = m_oForm.Items.Item(mc_strNoUnidad).Specific.String
        strNoUnidad = strNoUnidad.Trim()

        If Not String.IsNullOrEmpty(strNoUnidad) Then
            m_strU_Cod_Unid = (CStr(strNoUnidad))
        Else
            m_strU_Cod_Unid = String.Empty
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strMarcaMotor).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_MarcaMot = oCombo.Selected.Value
        Else
            m_strU_MarcaMot = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strCategoria).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Categori = oCombo.Selected.Value
        Else
            m_strU_Categori = ""
        End If
        oCombo = DirectCast(m_oForm.Items.Item(mc_strDisponibilidad).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Dispo = oCombo.Selected.Value
        Else
            m_strU_Dispo = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strTraccion).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Tipo_Tra = oCombo.Selected.Value
        Else
            m_strU_Tipo_Tra = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strCombustible).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Combusti = oCombo.Selected.Value
        Else
            m_strU_Combusti = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strColor).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Cod_Col = oCombo.Selected.Value
        Else
            m_strU_Cod_Col = ""
        End If
        oCombo = DirectCast(m_oForm.Items.Item(mc_strColorTapiceria).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_ColorTap = oCombo.Selected.Value
        Else
            m_strU_ColorTap = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strStatus).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Estatus = oCombo.Selected.Value
        Else
            m_strU_Estatus = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strUbicacion).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Cod_Ubic = oCombo.Selected.Value
        Else
            m_strU_Cod_Ubic = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strTipo).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Tipo = oCombo.Selected.Value
        Else
            m_strU_Tipo = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strCabina).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Tip_Cabi = oCombo.Selected.Value
        Else
            m_strU_Tip_Cabi = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strTecho).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_TipTecho = oCombo.Selected.Value
        Else
            m_strU_TipTecho = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strCarroceria).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Carrocer = oCombo.Selected.Value
        Else
            m_strU_Carrocer = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strTransmision).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Transmis = oCombo.Selected.Value
        Else
            m_strU_Transmis = ""
        End If
        oCombo = DirectCast(m_oForm.Items.Item(mc_strMarca).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Cod_Marc = oCombo.Selected.Value
        Else
            m_strU_Cod_Marc = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strModelo).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Cod_Mode = oCombo.Selected.Value
        Else
            m_strU_Cod_Mode = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strEstilo).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strU_Cod_Esti = oCombo.Selected.Value
        Else
            m_strU_Cod_Esti = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_strCboEstadoNuevo).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strEstadoNuevo = oCombo.Selected.Value
        Else
            m_strEstadoNuevo = ""
        End If

        oCombo = DirectCast(m_oForm.Items.Item(mc_StrCboClasificacion).Specific, SAPbouiCOM.ComboBox)
        If oCombo.Selected IsNot Nothing Then
            m_strCodClasificacion = oCombo.Selected.Value
        Else
            m_strCodClasificacion = ""
        End If

        strAno = m_oForm.Items.Item(mc_strAno).Specific.String

        If Not String.IsNullOrEmpty(strAno) AndAlso IsNumeric(strAno) Then
            m_strU_Ano_Vehi = Math.Round(CDbl(strAno))
        Else
            m_strU_Ano_Vehi = -1
        End If

        strComentarios = m_oForm.Items.Item(mc_strComentarios).Specific.String
        strComentarios = strComentarios.Trim()

        If Not String.IsNullOrEmpty(strComentarios) Then
            m_strU_Comentarios = strComentarios
        Else
            m_strU_Comentarios = String.Empty
        End If

        strNumPlaca = m_oForm.Items.Item(mc_strPlaca).Specific.String
        strNumPlaca = strNumPlaca.Trim()

        If Not String.IsNullOrEmpty(strNumPlaca) Then
            m_strU_NumPlaca = strNumPlaca
        Else
            m_strU_NumPlaca = String.Empty
        End If

        strCantDias = m_oForm.Items.Item(mc_strCantidadDias).Specific.String
        strCantDias = strCantDias.Trim

        If Not String.IsNullOrEmpty(strCantDias) Then
            m_strCantidadDias = strCantDias
        Else
            m_strCantidadDias = String.Empty
        End If
    End Sub

    Public Sub ManejoEventosCombo(ByRef oTmpForm As SAPbouiCOM.Form, _
                                      ByVal pval As SAPbouiCOM.ItemEvent, _
                                      ByVal FormUID As String, _
                                      ByRef BubbleEvent As Boolean)
        '*******************************************************************    
        'Nombre: ManejoEventosCombo()
        'Propósito: Se encarga de manejar el evento que genera el ChooseFromList
        'Acepta:    ByRef oTmpForm As SAPbouiCOM.Form, 
        '           ByVal pval As SAPbouiCOM.ItemEvent, 
        '           ByVal FormUID As String, 
        '           ByRef BubbleEvent As Boolean
        'Retorna:   Ninguno
        'Desarrollador: Yeiner
        'Fecha: 29 Nov 2006
        '********************************************************************
        Try

            Dim strValorSeleccionado As String = String.Empty
            Dim cboCombo As SAPbouiCOM.ComboBox
            Dim oItem As SAPbouiCOM.Item
            Dim intRecIndex As Integer = 0
            Static Dim blnActive As Boolean = True

            If pval.ItemUID = mc_strMarca Then
                oItem = oTmpForm.Items.Item(mc_strMarca)
                cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)


                strValorSeleccionado = CStr(cboCombo.Selected.Value)
                Call CargarValidValuesEnCombos(oTmpForm, _
                                               "Select Code,Name From [@SCGD_ESTILO] with(nolock) Where U_Cod_Marc ='" & strValorSeleccionado & "' ORDER BY NAME", _
                                               mc_strEstilo)
            ElseIf pval.ItemUID = mc_strEstilo Then

                oItem = oTmpForm.Items.Item(mc_strEstilo)
                cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)


                strValorSeleccionado = CStr(cboCombo.Selected.Value)
                Call CargarValidValuesEnCombos(oTmpForm, _
                                               "Select Code,U_Descripcion From [@SCGD_MODELO] with(nolock) Where U_Cod_Esti ='" & strValorSeleccionado & "' ORDER BY U_Descripcion", _
                                               mc_strModelo)
            ElseIf pval.ItemUID = mc_strMatriz Then
                ReservaVehiculo(oTmpForm, pval, BubbleEvent)

            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                           ByRef pVal As SAPbouiCOM.ItemEvent, _
                                           ByRef BubbleEvent As Boolean)

        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
        Dim sCFL_VEH As String
        sCFL_VEH = oCFLEvento.ChooseFromListUID
        Dim oForm As SAPbouiCOM.Form
        oForm = SBO_Application.Forms.Item(FormUID)
        Dim oCFL As SAPbouiCOM.ChooseFromList
        oCFL = oForm.ChooseFromLists.Item(sCFL_VEH)
        Dim oDataTable As SAPbouiCOM.DataTable

        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions

        Dim strUnidad As String
        Dim strEstadoVendido As String
        Dim strTipoVendido As String

        If oCFLEvento.BeforeAction = False Then

            oDataTable = oCFLEvento.SelectedObjects

            If Not oCFLEvento.SelectedObjects Is Nothing Then

                If pVal.ItemUID = "btnCargarV" Then

                    strUnidad = oDataTable.Columns.Item("U_Cod_Unid").Cells.Item(0).Value
                    strUnidad = strUnidad.Trim()

                    oForm.Items.Item("txtNoUnid").Specific.value = strUnidad

                End If

                AsignarDatosCliente(oForm, oDataTable, pVal.ItemUID)

            End If

        Else

            If pVal.ItemUID = "btnCargarV" Then

                oDataTableConfiguracionesDMS = oForm.DataSources.DataTables.Item("ConfigsDMS")
                oDataTableConfiguracionesDMS.ExecuteQuery(String.Format("Select U_Disp_V, U_Inven_V from [@SCGD_ADMIN] with(nolock)"))

                strEstadoVendido = oDataTableConfiguracionesDMS.GetValue("U_Disp_V", 0)
                strTipoVendido = oDataTableConfiguracionesDMS.GetValue("U_Inven_V", 0)

                oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_Cod_Unid"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                oCondition.BracketCloseNum = 1
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 2
                oCondition.Alias = "U_Dispo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCondition.CondVal = strEstadoVendido
                oCondition.BracketCloseNum = 2
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 3
                oCondition.Alias = "U_Tipo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCondition.CondVal = strTipoVendido
                oCondition.BracketCloseNum = 3
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 4
                oCondition.Alias = "U_Dispo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                oCondition.BracketCloseNum = 4
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 5
                oCondition.Alias = "U_Tipo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                oCondition.BracketCloseNum = 5

                oCFL.SetConditions(oConditions)

            End If

        End If

    End Sub

    Private Sub AsignarDatosCliente(ByRef Formulario As SAPbouiCOM.Form, ByRef DataTable As SAPbouiCOM.DataTable, ByVal ItemUID As String)
        Try
            If ItemUID = "CardCode" Or ItemUID = "CardName" Then
                If Not DataTable Is Nothing AndAlso DataTable.Rows.Count > 0 Then
                    Formulario.DataSources.UserDataSources.Item("CardCode").ValueEx = DataTable.Columns.Item("CardCode").Cells.Item(0).Value
                    Formulario.DataSources.UserDataSources.Item("CardName").ValueEx = DataTable.Columns.Item("CardName").Cells.Item(0).Value
                Else
                    Formulario.DataSources.UserDataSources.Item("CardCode").ValueEx = String.Empty
                    Formulario.DataSources.UserDataSources.Item("CardName").ValueEx = String.Empty
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function DevolverCodeVehiculo(ByVal p_intFila As Integer, ByVal p_strFormID As String) As String

        Dim oForm As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim strIDVehiculo As String

        oForm = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix)

        oMatriz.FlushToDataSource()

        m_dbInventario = m_oForm.DataSources.DataTables.Item(mc_strSCG_VEHICULO)
        strIDVehiculo = m_dbInventario.GetValue("Code", p_intFila - 1).ToString

        Return strIDVehiculo

    End Function

    <System.CLSCompliant(False)> _
    Public Sub ManejoEventosTab(ByRef oTmpForm As SAPbouiCOM.Form, _
                                ByRef pval As SAPbouiCOM.ItemEvent)

        If pval.ItemUID = mc_strFolderInventario Then
            oTmpForm.Freeze(True)
            'oTmpForm.PaneLevel = 1
            MostrarControlesInv(oTmpForm.UniqueID, True)
            oTmpForm.Freeze(False)
        ElseIf pval.ItemUID = mc_strFolderPedidos Then
            oTmpForm.Freeze(True)
            'oTmpForm.PaneLevel = 2
            MostrarControlesPed(oTmpForm.UniqueID, True)
            oTmpForm.Freeze(False)
        End If

    End Sub

    Public Sub MostrarControlesInv(ByVal strFormUid As String, ByVal boolShow As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim blnLineasFactura As Boolean

        oform = SBO_Application.Forms.Item(strFormUid)
        'oform.Freeze(True)
        oform.PaneLevel = 1
        Call SeleccionarValorDefecto(oform, False)
        'muestra y oculta matrices y controles
        oform.Items.Item("mtx_0").Visible = boolShow
        oform.Items.Item("mtxPedidos").Visible = Not boolShow
        oform.Items.Item("3").Visible = boolShow
        oform.Items.Item("2").Visible = boolShow

        'Deshabilitar Controles
        DirectCast(oform.Items.Item("cboMarca").Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        oform.Items.Item("txtPlaca").Enabled = boolShow
        oform.Items.Item("cboTipo").Enabled = boolShow
        oform.Items.Item("cboDisp").Enabled = boolShow
        oform.Items.Item("cboSta").Enabled = boolShow
        oform.Items.Item("txtComent").Enabled = boolShow
        oform.Items.Item("txtDiasInv").Enabled = boolShow
        oform.Items.Item("btnCargarV").Enabled = boolShow

        oform.Items.Item("cboUbi").Enabled = boolShow
        oform.Items.Item("cboClasif").Enabled = boolShow
        oform.Items.Item("cboClase").Enabled = boolShow
        oform.Items.Item("cboTap").Enabled = boolShow
        'oform.Freeze(False)

    End Sub

    Public Sub MostrarControlesPed(ByVal strFormUid As String, ByVal boolShow As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim blnLineasFactura As Boolean
        oform = SBO_Application.Forms.Item(strFormUid)
        Call SeleccionarValorDefecto(oform, False)
        'oform.Freeze(True)
        oform.PaneLevel = 2
        'muestra y oculta matrices
        oform.Items.Item("mtx_0").Visible = Not boolShow
        oform.Items.Item("mtxPedidos").Visible = boolShow
        oform.Items.Item("3").Visible = Not boolShow
        oform.Items.Item("2").Visible = Not boolShow

        'Deshabilitar Controles
        DirectCast(oform.Items.Item("cboMarca").Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        oform.Items.Item("txtPlaca").Enabled = Not boolShow
        oform.Items.Item("cboTipo").Enabled = Not boolShow
        oform.Items.Item("cboDisp").Enabled = Not boolShow
        oform.Items.Item("cboSta").Enabled = Not boolShow
        oform.Items.Item("txtComent").Enabled = Not boolShow
        oform.Items.Item("txtDiasInv").Enabled = Not boolShow
        oform.Items.Item("btnCargarV").Enabled = Not boolShow
        oform.Items.Item("cboUbi").Enabled = Not boolShow
        oform.Items.Item("cboClasif").Enabled = Not boolShow
        oform.Items.Item("cboClase").Enabled = Not boolShow
        oform.Items.Item("cboTap").Enabled = Not boolShow

        'oform.Freeze(False)

    End Sub

    Public Function CargarMatrixPed(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                       ByVal oform As SAPbouiCOM.Form, _
                                       ByVal dbPedidoVehiculo As SAPbouiCOM.DataTable) As Boolean

        Dim strCondiciones As String

        Try


            Dim query As String = String.Format(mc_strConsultaPedidos, My.Resources.Resource.strCodeConverFechaSql)
            
                Call PasarFiltrosAVariables()
                strCondiciones = ""

                If Not String.IsNullOrEmpty(m_strU_Cod_Marc) AndAlso m_strU_Cod_Marc <> "--" Then
                    strCondiciones &= " and " & mc_strU_Cod_MarcPed & " = '" & m_strU_Cod_Marc & "'"

                End If

                If Not String.IsNullOrEmpty(m_strU_Cod_Mode) AndAlso m_strU_Cod_Mode <> "--" Then
                    strCondiciones &= " and " & mc_strU_Cod_ModePed & " = '" & m_strU_Cod_Mode & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Cod_Esti) AndAlso m_strU_Cod_Esti <> "--" Then
                    strCondiciones &= " and " & mc_strU_Cod_EstiPed & " = '" & m_strU_Cod_Esti & "'"
                End If

                If m_strU_Ano_Vehi > -1 Then
                    strCondiciones &= " and " & mc_strCodYearPed & " = '" & m_strU_Ano_Vehi & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Cod_Col) AndAlso m_strU_Cod_Col <> "--" Then
                    strCondiciones &= " and " & mc_strU_Cod_Col & " = '" & m_strU_Cod_Col & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_MarcaMot) AndAlso m_strU_MarcaMot <> "--" Then
                    strCondiciones &= " and " & mc_strU_MarcaMotPed & " = '" & m_strU_MarcaMot & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Tipo_Tra) AndAlso m_strU_Tipo_Tra <> "--" Then
                    strCondiciones &= " and " & mc_strU_Tipo_TraPed & " = '" & m_strU_Tipo_Tra & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_TipTecho) AndAlso m_strU_TipTecho <> "--" Then
                    strCondiciones &= " and " & mc_strU_TipTechoPed & " = '" & m_strU_TipTecho & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Carrocer) AndAlso m_strU_Carrocer <> "--" Then
                    strCondiciones &= " and " & mc_strU_CarrocerPed & " = '" & m_strU_Carrocer & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Categori) AndAlso m_strU_Categori <> "--" Then
                    strCondiciones &= " and " & mc_strU_CategoriPed & " = '" & m_strU_Categori & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Combusti) AndAlso m_strU_Combusti <> "--" Then
                    strCondiciones &= " and " & mc_strU_Combusti & " = '" & m_strU_Combusti & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Tip_Cabi) AndAlso m_strU_Tip_Cabi <> "--" Then
                    strCondiciones &= " and " & mc_strU_Tip_CabiPed & " = '" & m_strU_Tip_Cabi & "'"
                End If

                If Not String.IsNullOrEmpty(m_strU_Transmis) AndAlso m_strU_Transmis <> "--" Then
                    strCondiciones &= " and " & mc_strU_Transmis & " = '" & m_strU_Transmis & "'"
                End If

            strCondiciones = "SELECT " & query & strCondiciones
            dbPedidoVehiculo.Rows.Clear()
            dbPedidoVehiculo.ExecuteQuery(strCondiciones)
            oMatrix.LoadFromDataSource()

            Return True
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Return False
        End Try

    End Function

    Private Function CrearListadoValidValuesReservar() As Generic.List(Of Utilitarios.ListadoValidValues)

        Dim oListadoValidValues As New Generic.List(Of Utilitarios.ListadoValidValues)
        Dim oValidValue As Utilitarios.ListadoValidValues

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = 0
        oValidValue.strName = My.Resources.Resource.No
        oListadoValidValues.Add(oValidValue)

        oValidValue = New Utilitarios.ListadoValidValues
        oValidValue.strCode = 1
        oValidValue.strName = My.Resources.Resource.Si
        oListadoValidValues.Add(oValidValue)

        Return oListadoValidValues

    End Function

    Private Sub SeleccionarValorDefecto(ByRef oForm As SAPbouiCOM.Form, blnBLimpiar As Boolean)

        If blnBLimpiar Then
            oForm.PaneLevel = 1
            DirectCast(oForm.Items.Item("fldInv").Specific, SAPbouiCOM.Folder).Select()
            m_dbInventario.Rows.Clear()
            DirectCast(m_oForm.Items.Item(mc_strMatriz).Specific, SAPbouiCOM.Matrix).LoadFromDataSource()
            m_dtPedidos.Rows.Clear()
            DirectCast(m_oForm.Items.Item(mc_strMatrixPed).Specific, SAPbouiCOM.Matrix).LoadFromDataSource()
            End If

        DirectCast(oForm.Items.Item(mc_strMarcaMotor).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strCategoria).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strDisponibilidad).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strUbicacion).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strStatus).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strTraccion).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strCombustible).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strColor).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strColorTapiceria).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strTipo).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strCabina).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strTecho).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strCarroceria).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strTransmision).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strMarca).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strModelo).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strEstilo).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_strCboEstadoNuevo).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item(mc_StrCboClasificacion).Specific, SAPbouiCOM.ComboBox).Select("--", BoSearchKey.psk_ByValue)
        DirectCast(oForm.Items.Item("txtAño").Specific, SAPbouiCOM.EditText).Value = String.Empty
        DirectCast(oForm.Items.Item("txtNoUnid").Specific, SAPbouiCOM.EditText).Value = String.Empty
        DirectCast(oForm.Items.Item("txtComent").Specific, SAPbouiCOM.EditText).Value = String.Empty
        DirectCast(oForm.Items.Item("txtDiasInv").Specific, SAPbouiCOM.EditText).Value = String.Empty
        DirectCast(oForm.Items.Item("txtPlaca").Specific, SAPbouiCOM.EditText).Value = String.Empty




    End Sub

#End Region


End Class
