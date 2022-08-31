Imports System.Globalization
Imports System.Collections.Generic
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany


Partial Public Class EspecificacionPorModeloCls : Implements IFormularioSBO, IUsaMenu

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company
    Private m_oForm As SAPbouiCOM.Form

    Private _applicationSbo As Application
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String

    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String

    Private _Conexion As String
    Private _DireccionReportes As String
    Private _UsuarioBD As String
    Private _ContraseñaBD As String


    Public EditCboModelo As ComboBoxSBO
    Public EditCboEstilo As ComboBoxSBO
    Public EditCboMarca As ComboBoxSBO

    Public EditTxtNoPasajeros As EditTextSBO
    Public EditTxtNoEjes As EditTextSBO
    Public EditTxtNoPuertas As EditTextSBO
    Public EditTxtNoCilindros As EditTextSBO
    Public EditTxtPeso As EditTextSBO
    Public EditTxtCilindrada As EditTextSBO

    Public EditTxtPotencia As EditTextSBO
    Public EditCboCategoria As ComboBoxSBO
    Public EditCboMarcaMotor As ComboBoxSBO
    Public EditCboTransmision As ComboBoxSBO
    Public EditCboCarroceria As ComboBoxSBO
    Public EditCboTraccion As ComboBoxSBO

    Public EditCboCabina As ComboBoxSBO
    Public EditCboCombustible As ComboBoxSBO
    Public EditTxtGarantiaKm As EditTextSBO
    Public EditTxtGarantiaAnos As EditTextSBO
    Public EditCboTipoTecho As ComboBoxSBO

    Public EditTextCodItmInv As EditTextSBO
    Public EditTextDesItmInv As EditTextSBO
    Public EditTextMarcaCom As EditTextSBO
    Public EditTextMarcaComDes As EditTextSBO

    Public md_EspecificosXMod As DataTable
    Public md_matrixAcc As DataTable
    Public dtLocal As SAPbouiCOM.DataTable

    Private m_SBO_Application As SAPbouiCOM.Application

    Dim mc_strCboMarca = "cboMarca"
    Dim mc_strCboEstilo = "cboEstilo"
    Dim mc_strCboModelo = "cboModelo"

    Dim m_strUIDMatListAcc As String = "mtxListAcc"
    Dim m_strUIDMatAccVeh As String = "mtxAccVeh"

    Dim strFilasEliminar As IList(Of String) = New List(Of String)

    Dim m_blnUsaModelo As Boolean
    Dim blnMsj As Boolean = False

    Dim blnCambio As Boolean = False

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
            Return _formularioSbo
        End Get
        'Set(ByVal value As SAPbouiCOM.IForm)
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

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(ByVal value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(ByVal value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(ByVal value As Integer)
            _Posicion = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Conexion As String
        Get
            Return _Conexion
        End Get
        Set(ByVal value As String)
            _Conexion = value
        End Set
    End Property

    Public Property DireccionReportes As String
        Get
            Return _DireccionReportes
        End Get
        Set(ByVal value As String)
            _DireccionReportes = value
        End Set
    End Property

    Public Property UsuarioBd As String
        Get
            Return _UsuarioBD
        End Get
        Set(ByVal value As String)
            _UsuarioBD = value
        End Set
    End Property

    Public Property ContraseñaBd As String
        Get
            Return _ContraseñaBD
        End Get
        Set(ByVal value As String)
            _ContraseñaBD = value
        End Set
    End Property

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        CargarCombos()
        CargarFormulario()
    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Dim oMatriz As SAPbouiCOM.Matrix

        If FormularioSBO IsNot Nothing Then
            FormularioSBO.Freeze(True)

            md_EspecificosXMod = FormularioSBO.DataSources.DataTables.Add("EspeXMode")


            Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
            userDataSources.Add("Marca", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Estilo", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Modelo", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("NoPasaj", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("NoEjes", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("NoPuertas", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("NoCilin", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Peso", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Cilind", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Potencia", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Categoria", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("MarcaMot", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Transmis", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Carroceria", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Traccion", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Cabina", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Combust", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("GarantKM", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("GarantAn", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("TipoTecho", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("Item", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("ItemD", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("MarcCom", BoDataType.dt_LONG_TEXT, 100)
            userDataSources.Add("MarcComD", BoDataType.dt_LONG_TEXT, 100)

            EditCboMarca = New ComboBoxSBO("cboMarca", FormularioSBO, True, "", "Marca")
            EditCboEstilo = New ComboBoxSBO("cboEstilo", FormularioSBO, True, "", "Estilo")
            EditCboModelo = New ComboBoxSBO("cboModelo", FormularioSBO, True, "", "Modelo")

            EditTxtNoPasajeros = New EditTextSBO("txtNoPasaj", True, "", "NoPasaj", FormularioSBO)
            EditTxtNoEjes = New EditTextSBO("txtNoEjes", True, "", "NoEjes", FormularioSBO)
            EditTxtNoPuertas = New EditTextSBO("txtNoPuert", True, "", "NoPuertas", FormularioSBO)
            EditTxtNoCilindros = New EditTextSBO("txtNoCili", True, "", "NoCilin", FormularioSBO)
            EditTxtPeso = New EditTextSBO("txtPeso", True, "", "Peso", FormularioSBO)
            EditTxtCilindrada = New EditTextSBO("txtCilind", True, "", "Cilind", FormularioSBO)
            EditTxtPotencia = New EditTextSBO("txtPotenc", True, "", "Potencia", FormularioSBO)

            EditCboCategoria = New ComboBoxSBO("cboCatego", FormularioSBO, True, "", "Categoria")
            EditCboMarcaMotor = New ComboBoxSBO("cboMarcaMo", FormularioSBO, True, "", "MarcaMot")
            EditCboTransmision = New ComboBoxSBO("cboTransmi", FormularioSBO, True, "", "Transmis")
            EditCboCarroceria = New ComboBoxSBO("cboCarroce", FormularioSBO, True, "", "Carroceria")
            EditCboTraccion = New ComboBoxSBO("cboTraccio", FormularioSBO, True, "", "Traccion")
            EditCboCabina = New ComboBoxSBO("cboCabina", FormularioSBO, True, "", "Cabina")
            EditCboCombustible = New ComboBoxSBO("cboCombust", FormularioSBO, True, "", "Combust")

            EditTxtGarantiaKm = New EditTextSBO("txtGaranKm", True, "", "GarantKM", FormularioSBO)
            EditTxtGarantiaAnos = New EditTextSBO("txtGaranAn", True, "", "GarantAn", FormularioSBO)

            EditCboTipoTecho = New ComboBoxSBO("cboTecho", FormularioSBO, True, "", "TipoTecho")

            EditTextCodItmInv = New EditTextSBO("txtItem", True, "", "Item", FormularioSBO)
            EditTextDesItmInv = New EditTextSBO("txtItemD", True, "", "ItemD", FormularioSBO)
            EditTextMarcaCom = New EditTextSBO("txtMarCom", True, "", "MarcCom", FormularioSBO)
            EditTextMarcaComDes = New EditTextSBO("txtMarComD", True, "", "MarcComD", FormularioSBO)

            EditTxtNoPasajeros.AsignaBinding()
            EditTxtNoEjes.AsignaBinding()
            EditTxtNoPuertas.AsignaBinding()
            EditTxtNoCilindros.AsignaBinding()
            EditTxtPeso.AsignaBinding()
            EditTxtCilindrada.AsignaBinding()
            EditTxtPotencia.AsignaBinding()
            EditTxtGarantiaKm.AsignaBinding()
            EditTxtGarantiaAnos.AsignaBinding()

            EditCboMarca.AsignaBinding()
            EditCboModelo.AsignaBinding()
            EditCboEstilo.AsignaBinding()
            EditCboCategoria.AsignaBinding()
            EditCboMarcaMotor.AsignaBinding()
            EditCboTransmision.AsignaBinding()
            EditCboCarroceria.AsignaBinding()
            EditCboTraccion.AsignaBinding()
            EditCboCabina.AsignaBinding()
            EditCboTraccion.AsignaBinding()
            EditCboCombustible.AsignaBinding()
            EditCboTipoTecho.AsignaBinding()

            EditTextCodItmInv.AsignaBinding()
            EditTextDesItmInv.AsignaBinding()
            EditTextMarcaCom.AsignaBinding()
            EditTextMarcaComDes.AsignaBinding()

            FormularioSBO.Freeze(False)
        End If


    End Sub

#End Region

#Region "Metodos"

    Private Sub CargarCombos()
        Dim ocombo As SAPbouiCOM.ComboBox
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim fcp As SAPbouiCOM.FormCreationParams

        Dim sboItem As SAPbouiCOM.Item
        Dim sboCombo As SAPbouiCOM.ComboBox

        sboItem = FormularioSBO.Items.Item("cboMarca")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMarca"), ""))

        sboItem = FormularioSBO.Items.Item("cboTraccio")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTraccion"), "Order by ""Name"" "))

        sboItem = FormularioSBO.Items.Item("cboCombust")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCombustible"), "Order by ""Name"" "))

        sboItem = FormularioSBO.Items.Item("cboMarcaMo")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbMarcaMotor"), "Order by ""Name"" "))

        sboItem = FormularioSBO.Items.Item("cboTecho")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTecho"), "Order by ""Name"" "))
        '--------------------------------

        sboItem = FormularioSBO.Items.Item("cboCabina")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCabina"), "Order by ""Name"" "))

        sboItem = FormularioSBO.Items.Item("cboCarroce")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCarroceria"), "Order by ""Name"" "))

        sboItem = FormularioSBO.Items.Item("cboTransmi")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbTrasmision"), "Order by ""Name"" "))

        sboItem = FormularioSBO.Items.Item("cboCatego")
        sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
        Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbCategoriaVehiculo"), "Order by ""Name"" "))

    End Sub

    Public Sub ManejadorEventosCombos(ByRef oTmpForm As SAPbouiCOM.Form, _
                                  ByVal pval As SAPbouiCOM.ItemEvent, _
                                  ByVal FormUID As String, _
                                  ByRef BubbleEvent As Boolean)


        Try
            If pval.ActionSuccess Then
                Dim strValorSeleccionado As String = String.Empty
                Dim cboCombo As SAPbouiCOM.ComboBox
                Dim oItem As SAPbouiCOM.Item
                Dim id As String = ""

                If pval.ItemUID = mc_strCboMarca Then
                    oItem = FormularioSBO.Items.Item(mc_strCboMarca)
                    cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

                    If Utilitarios.blnAutoMarcaEstiloModelo Then
                        strValorSeleccionado = CStr(cboCombo.Selected.Value)
                        oItem = FormularioSBO.Items.Item(mc_strCboEstilo)
                        cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                        Call Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, _
                                                                    String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstilo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Marc", strValorSeleccionado)))
                        LimpiarCamposEspecificacionesTecnicas(FormUID)
                    ElseIf cboCombo.Active Then

                        strValorSeleccionado = CStr(cboCombo.Selected.Value)
                        oItem = FormularioSBO.Items.Item(mc_strCboEstilo)
                        cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                        Call Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, _
                                                                    String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbEstilo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Marc", strValorSeleccionado)))
                        LimpiarCamposEspecificacionesTecnicas(FormUID)
                        dtAccVehiculo.Clear()
                        MatrizAccVehi.Matrix.LoadFromDataSource()
                        FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                    End If
                End If

                If pval.ItemUID = mc_strCboEstilo Then
                    oItem = FormularioSBO.Items.Item(mc_strCboEstilo)
                    cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)

                    If Utilitarios.blnAutoMarcaEstiloModelo Then
                        strValorSeleccionado = CStr(cboCombo.Selected.Value)
                        oItem = FormularioSBO.Items.Item(mc_strCboModelo)
                        cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                        Call Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, _
                                                                    String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbModelo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Esti", strValorSeleccionado)))
                        LimpiarCamposEspecificacionesTecnicas(FormUID)
                        Utilitarios.blnAutoMarcaEstiloModelo = False
                    Else
                        If cboCombo.Active Then
                            strValorSeleccionado = CStr(cboCombo.Selected.Value)
                            oItem = FormularioSBO.Items.Item(mc_strCboModelo)
                            cboCombo = DirectCast(oItem.Specific, SAPbouiCOM.ComboBox)
                            Call Utilitarios.CargarValidValuesEnCombos(cboCombo.ValidValues, _
                                                                       String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strCbModelo"), String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strWhereCode"), "U_Cod_Esti", strValorSeleccionado)))
                            LimpiarCamposEspecificacionesTecnicas(FormUID)
                            dtAccVehiculo.Clear()
                            MatrizAccVehi.Matrix.LoadFromDataSource()


                        End If
                        Utilitarios.blnAutoMarcaEstiloModelo = False
                    End If

                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''
                'Lista de Accesorios por Modelo/Estilo

                If pval.ItemUID = mc_strCboEstilo OrElse pval.ItemUID = mc_strCboModelo Then
                    Dim l_strUsaEstilo As String = String.Empty
                    Dim accesorioXEstilo As Boolean = False
                    Dim cboComboLocal As SAPbouiCOM.ComboBox
                    Dim oItemLocal As SAPbouiCOM.Item

                    l_strUsaEstilo = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()
                    If l_strUsaEstilo = "E" AndAlso Not String.IsNullOrEmpty(l_strUsaEstilo) Then
                        oItemLocal = oTmpForm.Items.Item(mc_strCboEstilo)
                        cboComboLocal = CType(oItemLocal.Specific, SAPbouiCOM.ComboBox)
                        accesorioXEstilo = True
                        strFilasEliminar.Clear()
                    ElseIf l_strUsaEstilo = "M" AndAlso Not String.IsNullOrEmpty(l_strUsaEstilo) Then
                        oItemLocal = oTmpForm.Items.Item(mc_strCboModelo)
                        cboComboLocal = CType(oItemLocal.Specific, SAPbouiCOM.ComboBox)
                        accesorioXEstilo = False
                        strFilasEliminar.Clear()
                    End If

                    'si se selecciono un combo
                    If cboComboLocal.Selected IsNot Nothing Then
                        id = CStr(cboComboLocal.Selected.Value)
                    End If

                    'Si se obtiene un id para cargar componentes 
                    If Not String.IsNullOrEmpty(id) Then
                        CargarComponentesPorEstilo(FormUID, id, accesorioXEstilo)
                        CargarEspeficacionesPorEstilo(FormUID, id, accesorioXEstilo)

                        FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE

                    End If
                End If

                If pval.ItemUID = "1" Then
                    blnCambio = False
                End If

            ElseIf pval.BeforeAction Then

                Select Case pval.ItemUID
                    Case mc_strCboMarca
                        If blnCambio OrElse FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If m_blnUsaModelo Then
                                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosModelo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                    BubbleEvent = False
                                End If
                            Else
                                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosEstilo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                    BubbleEvent = False
                                End If
                            End If

                        End If

                    Case mc_strCboEstilo
                        If blnCambio OrElse FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If m_blnUsaModelo Then
                                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosModelo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                    BubbleEvent = False
                                End If
                            Else
                                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosEstilo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                    BubbleEvent = False
                                End If
                            End If

                        End If
                    Case mc_strCboModelo
                        If blnCambio OrElse FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            If m_blnUsaModelo Then
                                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosModelo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                    BubbleEvent = False
                                End If
                            Else
                                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajePerderaCambiosEstilo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                                    BubbleEvent = False
                                End If
                            End If

                        End If
                End Select

            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
        
    End Sub


    Public Sub AsignarAccesorioACategoria(ByVal pval As SAPbouiCOM.ItemEvent)
        Try

            Dim l_strCodigo As String = String.Empty
            Dim l_strNombre As String = String.Empty
            Dim l_numFila As String
            Dim ln_FilasAcc As Integer
            Dim oItem As SAPbouiCOM.Item
            Dim cboCombo As SAPbouiCOM.ComboBox
            Dim id As String

            If pval.ItemUID = m_strUIDMatListAcc Then

                If m_blnUsaModelo = False Then
                    oItem = FormularioSBO.Items.Item(mc_strCboEstilo)
                    cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                ElseIf m_blnUsaModelo Then
                    oItem = FormularioSBO.Items.Item(mc_strCboModelo)
                    cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                End If

                If cboCombo.Selected IsNot Nothing Then
                    id = CStr(cboCombo.Selected.Value)
                End If

                If Not String.IsNullOrEmpty(id) Then
                    If pval.ItemUID = m_strUIDMatListAcc Then
                        l_numFila = pval.Row
                    End If

                    If Not String.IsNullOrEmpty(l_numFila) AndAlso l_numFila > 0 Then
                        l_strCodigo = MatrizAcc.ObtieneValorColumnaEditText("Col_Code", l_numFila)
                        l_strNombre = MatrizAcc.ObtieneValorColumnaEditText("Col_Name", l_numFila)

                        If Not String.IsNullOrEmpty(l_strCodigo) Then
                            If dtAccVehiculo.Rows.Count = 0 Then
                                ln_FilasAcc = 0
                            ElseIf String.IsNullOrEmpty(dtAccVehiculo.GetValue("code", 0)) Then
                                ln_FilasAcc = 0
                            Else
                                ln_FilasAcc = dtAccVehiculo.Rows.Count
                            End If
                            If ValidarExisteAccesorio(l_strCodigo) And ln_FilasAcc >= 0 Then

                                dtAccVehiculo.Rows.Add()
                                
                                dtAccVehiculo.SetValue("code", ln_FilasAcc, l_strCodigo)
                                dtAccVehiculo.SetValue("name", ln_FilasAcc, l_strNombre)

                                For Each codeEliminar As String In strFilasEliminar
                                    If codeEliminar = l_strCodigo Then
                                        strFilasEliminar.Remove(l_strCodigo)
                                        Exit For
                                    End If
                                Next

                            Else
                                If m_blnUsaModelo Then
                                    ApplicationSBO.StatusBar.SetText((My.Resources.Resource.MensajeModeloAccYaExiste), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                                Else
                                    ApplicationSBO.StatusBar.SetText((My.Resources.Resource.MensajeEstiloAccYaExiste), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                                End If

                            End If
                        End If
                    End If


                Else
                    If m_blnUsaModelo Then
                        ApplicationSBO.StatusBar.SetText((My.Resources.Resource.MensajeModeloSinSeleccionar), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    Else
                        ApplicationSBO.StatusBar.SetText((My.Resources.Resource.MensajeEstiloSinSeleccionar), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    End If


                End If
                MatrizAccVehi.Matrix.LoadFromDataSource()
            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Private Sub CargarEspeficacionesPorEstilo(ByVal ps_FormID As String, ByVal ps_Codigo As String, ByVal pl_EspecifXEstilo As Boolean)

        Dim ls_Consulta As String 
        Dim ld_TablaDatos As Data.DataTable
        Dim l_strSQL As String
        
        If Not String.IsNullOrEmpty(ps_Codigo) Then

            If pl_EspecifXEstilo Then
                ls_Consulta = " WHERE ""U_Cod_Estilo"" = '" & ps_Codigo & "'"
            Else
                ls_Consulta = " WHERE ""U_Cod_Modelo"" = '" & ps_Codigo & "'"
            End If

            l_strSQL = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strExpeXModelo"), ls_Consulta)
            
            ld_TablaDatos = Utilitarios.EjecutarConsultaDataTable(String.Format(l_strSQL))

            If ld_TablaDatos.Rows.Count > 0 AndAlso Not IsNothing(ld_TablaDatos) Then
                With ld_TablaDatos.Rows(0)
                    EditTxtNoCilindros.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Num_Cili")), String.Empty, .Item("U_Num_Cili")))
                    EditTxtNoPuertas.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Cant_Puerta")), String.Empty, .Item("U_Cant_Puerta")))
                    EditTxtNoPasajeros.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Cant_Pasaj")), String.Empty, .Item("U_Cant_Pasaj")))
                    EditTxtNoEjes.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Cant_Ejes")), String.Empty, .Item("U_Cant_Ejes")))
                    EditTxtPeso.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Peso")), String.Empty, .Item("U_Peso")))
                    EditTxtPotencia.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Potencia")), String.Empty, .Item("U_Potencia")))
                    EditTxtCilindrada.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Cilindrada")), String.Empty, .Item("U_Cilindrada")))

                    EditCboCategoria.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Categoria")), String.Empty, .Item("U_Categoria")))
                    EditCboMarcaMotor.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Marca_Mot")), String.Empty, .Item("U_Marca_Mot")))
                    EditCboTraccion.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Tipo_Trac")), String.Empty, .Item("U_Tipo_Trac")))
                    EditCboTransmision.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Transmis")), String.Empty, .Item("U_Transmis")))
                    EditCboCarroceria.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Carroceria")), String.Empty, .Item("U_Carroceria")))
                    EditCboCabina.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Tipo_Cabina")), String.Empty, .Item("U_Tipo_Cabina")))
                    EditCboCombustible.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Combusti")), String.Empty, .Item("U_Combusti")))
                    EditCboTipoTecho.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Tipo_Techo")), String.Empty, .Item("U_Tipo_Techo")))

                    EditTxtGarantiaKm.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_GarantKM")), String.Empty, .Item("U_GarantKM")))
                    EditTxtGarantiaAnos.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_GarantTM")), String.Empty, .Item("U_GarantTM")))
                    EditTextMarcaComDes.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_Cod_MarComer")), String.Empty, .Item("U_Cod_MarComer")))
                    If Not String.IsNullOrEmpty(.Item("CodeAV").ToString()) Then
                        EditTextMarcaCom.AsignaValorUserDataSource(IIf(IsDBNull(.Item("CodeAV")), String.Empty, .Item("CodeAV")))
                        EditTextDesItmInv.AsignaValorUserDataSource(IIf(IsDBNull(.Item("U_ArtVent")), String.Empty, .Item("U_ArtVent")))
                    End If
                End With
            
        ElseIf ld_TablaDatos.Rows.Count = 0 OrElse IsNothing(ld_TablaDatos) Then
            LimpiarCamposEspecificacionesTecnicas(ps_FormID)
        End If

        End If


    End Sub

    Private Sub CargarComponentesPorEstilo(ByVal p_strFormID As String, ByVal p_strCodEstilo As String, ByVal accesorioXEstilo As Boolean)

        Dim oform As SAPbouiCOM.Form
        Dim l_strSQL As String
        Try

            l_strSQL = String.Format("Select ""U_Accesorio"" as ""code"" , ""U_ItemName"" as ""name"" from ""@SCGD_ACCXMODE"" inner join ""OITM"" on ""OITM"".""ItemCode"" = ""U_Accesorio"" where ""@SCGD_ACCXMODE"".""U_Modelo"" = '{0}'", p_strCodEstilo)
            dtAccVehiculo.ExecuteQuery(l_strSQL)

            If String.IsNullOrEmpty(dtAccVehiculo.GetValue("code", 0)) OrElse IsNothing(dtAccVehiculo.GetValue("code", 0)) Then
                dtAccVehiculo.Rows.Remove(0)
            End If

            MatrizAccVehi.Matrix.LoadFromDataSource()


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

    Private Sub LimpiarCamposEspecificacionesTecnicas(ByVal ps_FormID As String)

        FormularioSBO.Freeze(True)
        EditTxtNoCilindros.AsignaValorUserDataSource(Nothing)
        EditTxtNoPuertas.AsignaValorUserDataSource(Nothing)
        EditTxtNoPasajeros.AsignaValorUserDataSource(Nothing)
        EditTxtNoEjes.AsignaValorUserDataSource(Nothing)
        EditTxtPeso.AsignaValorUserDataSource(Nothing)
        EditTxtPotencia.AsignaValorUserDataSource(Nothing)
        EditTxtCilindrada.AsignaValorUserDataSource(Nothing)

        EditCboCategoria.AsignaValorUserDataSource(Nothing)
        EditCboMarcaMotor.AsignaValorUserDataSource(Nothing)
        EditCboTraccion.AsignaValorUserDataSource(Nothing)
        EditCboTransmision.AsignaValorUserDataSource(Nothing)
        EditCboCarroceria.AsignaValorUserDataSource(Nothing)
        EditCboCabina.AsignaValorUserDataSource(Nothing)
        EditCboCombustible.AsignaValorUserDataSource(Nothing)
        EditCboTipoTecho.AsignaValorUserDataSource(Nothing)

        EditTxtGarantiaKm.AsignaValorUserDataSource(Nothing)
        EditTxtGarantiaAnos.AsignaValorUserDataSource(Nothing)

        EditTextMarcaCom.AsignaValorUserDataSource(Nothing)
        EditTextMarcaComDes.AsignaValorUserDataSource(Nothing)
        EditTextDesItmInv.AsignaValorUserDataSource(Nothing)
        FormularioSBO.Freeze(False)
    End Sub

#End Region

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, ByVal p_strUISCGD_EspecificosModelo As String)
        _companySbo = companySbo
        _applicationSbo = application
        m_oCompany = companySbo
        DMS_Connector.Helpers.SetCulture(Threading.Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture )
        NombreXml = Windows.Forms.Application.StartupPath + My.Resources.Resource.XMLFormEspecificacionXModelo
        MenuPadre = "SCGD_CFG"
        Nombre = My.Resources.Resource.MenuEspecificacionesPorModelo
        IdMenu = p_strUISCGD_EspecificosModelo
        Titulo = My.Resources.Resource.MenuEspecificacionesPorModelo
        Posicion = 20
        FormType = p_strUISCGD_EspecificosModelo
        DireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        UsuarioBd = CatchingEvents.DBUser
        ContraseñaBd = CatchingEvents.DBPassword

    End Sub


    Sub ApplicationSboOnItemEvent(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
            ManejadorEventosCombos(FormularioSBO, pVal, FormUID, BubbleEvent)
        ElseIf pVal.EventType = BoEventTypes.et_CHOOSE_FROM_LIST Then
            ManejadorEventosChooseFromList(pVal, BubbleEvent)
        ElseIf pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
            ManejadorEventosItemPress(FormUID, pVal, BubbleEvent)
        End If



    End Sub

    Public Sub ManejadorEventosItemPress(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            If pVal.ActionSuccess Then

                Select Case pVal.ItemUID
                    Case "1"
                        ActualizaAccesoriosPorModelo()
                        ActualizarEspecificosPorModelo()

                        If blnMsj Then
                            blnMsj = False
                            If m_blnUsaModelo Then
                                ApplicationSBO.StatusBar.SetText((My.Resources.Resource.MensajeModeloActualizado), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                            Else
                                ApplicationSBO.StatusBar.SetText((My.Resources.Resource.MensajeEstiloActualizado), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                            End If
                        End If

                    Case "mtxListAcc"
                        If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse _
                            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE OrElse _
                            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                            AsignarAccesorioACategoria(pVal)
                            blnCambio = True
                            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                        End If

                    Case "mtxAccVeh"
                        If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse _
                            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE OrElse _
                            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then

                            blnCambio = True
                            EliminarAccesorioVehiculo(pVal)
                            FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                        End If
                    Case "btnArtVent"
                        ButtonSeleccionArticuloVenta(FormUID, pVal, BubbleEvent)
                End Select

            ElseIf pVal.BeforeAction Then
                Dim strValorEnCombo As String = ""

                If m_blnUsaModelo Then
                    strValorEnCombo = EditCboModelo.ObtieneValorUserDataSource
                    strValorEnCombo = strValorEnCombo.Trim
                ElseIf Not m_blnUsaModelo Then
                    strValorEnCombo = EditCboEstilo.ObtieneValorUserDataSource
                    strValorEnCombo = strValorEnCombo.Trim
                End If

                If String.IsNullOrEmpty(strValorEnCombo) AndAlso pVal.ItemUID <> "2" Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeModeloSinSeleccionar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    BubbleEvent = False
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ManejadorEventosChooseFromList(ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim strCFL_Id As String
            Dim oCondition As SAPbouiCOM.Condition
            Dim oConditions As SAPbouiCOM.Conditions

            Dim oDataTable As SAPbouiCOM.DataTable

            oCFLEvent = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            strCFL_Id = oCFLEvent.ChooseFromListUID
            oCFL = _formularioSbo.ChooseFromLists.Item(strCFL_Id)


            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case EditTextCodItmInv.UniqueId

                End Select
            ElseIf pVal.BeforeAction Then
                Select Case pVal.ItemUID

                    Case EditTextCodItmInv.UniqueId
                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "8"
                        oCondition.BracketCloseNum = 1

                        oCFL.SetConditions(oConditions)

                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ButtonSeleccionArticuloVenta(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.BeforeAction AndAlso pVal.ActionSuccess = False Then

        ElseIf pVal.BeforeAction = False AndAlso pVal.ActionSuccess Then

            m_oForm = ApplicationSBO.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_VAV", False, ApplicationSBO) Then
                Dim objArticuloVenta As New VehiculoArticuloVenta(m_oCompany, ApplicationSBO)
                objArticuloVenta.FormConfiguracion = m_oForm
                Call objArticuloVenta.CargaFormulario()
            End If

        End If
    End Sub

    '  Private Sub AsignaValoresItem(ByRef pVal As SAPbouiCOM.)
    Public Sub AsignaValoresItem(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim oMat As SAPbouiCOM.Matrix

            FormularioSBO.Freeze(True)

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub ActualizarEspecificosPorModelo()
        
        Dim strSQLValues As String
        Dim strSQL As String
        Dim strCboUsado As String
        Dim strCodeCbo As String
        Dim strCont As String

        If m_blnUsaModelo Then
            strCodeCbo = EditCboModelo.ObtieneValorUserDataSource()
        Else
            strCodeCbo = EditCboEstilo.ObtieneValorUserDataSource()
        End If

        If m_blnUsaModelo Then
            strCboUsado = " U_Cod_Modelo "
        Else
            strCboUsado = " U_Cod_Estilo "
        End If

        strCont = Utilitarios.EjecutarConsulta("select MAX(""Code"" + 1) from ""@SCGD_ESPEXMODE"" ")
        If String.IsNullOrEmpty(Utilitarios.EjecutarConsulta(String.Format("SELECT ""Code"" FROM ""@SCGD_ESPEXMODE"" WHERE ""{0}"" = '{1}'", strCboUsado.Trim, strCodeCbo.Trim()))) Then
            strSQL = "INSERT INTO  ""@SCGD_ESPEXMODE"" ("
            strSQLValues = " VALUES ("
            If Not String.IsNullOrEmpty(strCont) Then
                strSQL = String.Format("{0}{1}", strSQL, """Code"",""Name"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}','{0}',", strCont))
            End If
            If Not String.IsNullOrEmpty(EditCboMarca.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cod_Marca"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboMarca.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboModelo.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cod_Modelo"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboModelo.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboEstilo.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cod_Estilo"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboEstilo.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtNoCilindros.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Num_Cili"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtNoCilindros.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtNoPuertas.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cant_Puerta"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtNoPuertas.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtNoPasajeros.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cant_Pasaj"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtNoPasajeros.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtNoEjes.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cant_Ejes"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtNoEjes.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtPeso.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Peso"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtPeso.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtCilindrada.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cilindrada"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtCilindrada.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtPotencia.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Potencia"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtPotencia.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboCategoria.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Categoria"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboCategoria.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboMarcaMotor.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Marca_Mot"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboMarcaMotor.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboTransmision.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Transmis"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboTransmision.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboCarroceria.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Carroceria"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboCarroceria.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboTraccion.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Tipo_Trac"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboTraccion.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboCabina.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Tipo_Cabina"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboCabina.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboCombustible.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Combusti"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboCombustible.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtGarantiaKm.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_GarantKM"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtGarantiaKm.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTxtGarantiaAnos.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_GarantTM"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTxtGarantiaAnos.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditCboTipoTecho.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Tipo_Techo"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditCboTipoTecho.ObtieneValorUserDataSource().Trim()))
            End If
            If Not String.IsNullOrEmpty(EditTextMarcaComDes.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0}{1}", strSQL, """U_Cod_MarComer"",")
                strSQLValues = String.Format("{0}{1}", strSQLValues, String.Format("'{0}',", EditTextMarcaComDes.ObtieneValorUserDataSource().Trim()))
            End If
            strSQL = String.Format("{0}){1})", strSQL.TrimEnd(","), strSQLValues.TrimEnd(","))
            blnMsj = True


        Else
            strSQL = "UPDATE ""@SCGD_ESPEXMODE"" SET "
            If Not String.IsNullOrEmpty(strCont) Then
                strSQL = String.Format("{0} {1}='{2}',", strSQL, """Code""", strCont)
                strSQL = String.Format("{0} {1}='{2}',", strSQL, """Name""", strCont)
               End If
            If Not String.IsNullOrEmpty(EditCboMarca.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cod_Marca""", EditCboMarca.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboModelo.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cod_Modelo""", EditCboModelo.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboEstilo.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cod_Estilo""", EditCboEstilo.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtNoCilindros.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Num_Cili""", EditTxtNoCilindros.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtNoPuertas.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cant_Puerta""", EditTxtNoPuertas.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtNoPasajeros.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cant_Pasaj""", EditTxtNoPasajeros.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtNoEjes.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cant_Ejes""", EditTxtNoEjes.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtPeso.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Peso""", EditTxtPeso.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtCilindrada.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cilindrada""", EditTxtCilindrada.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtPotencia.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Potencia""", EditTxtPotencia.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboCategoria.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Categoria""", EditCboCategoria.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboMarcaMotor.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Marca_Mot""", EditCboMarcaMotor.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboTransmision.ObtieneValorUserDataSource()) Then
                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Transmis""", EditCboTransmision.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboCarroceria.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Carroceria""", EditCboCarroceria.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboTraccion.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Tipo_Trac""", EditCboTraccion.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboCabina.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Tipo_Cabina""", EditCboCabina.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboCombustible.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Combusti""", EditCboCombustible.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtGarantiaKm.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_GarantKM""", EditTxtGarantiaKm.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTxtGarantiaAnos.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_GarantTM""", EditTxtGarantiaAnos.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditCboTipoTecho.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Tipo_Techo""", EditCboTipoTecho.ObtieneValorUserDataSource())
            End If
            If Not String.IsNullOrEmpty(EditTextMarcaComDes.ObtieneValorUserDataSource()) Then

                strSQL = String.Format("{0} {1}='{2}',", strSQL, """U_Cod_MarComer""", EditTextMarcaComDes.ObtieneValorUserDataSource())
            End If
            strSQL = String.Format("{0} {1}", strSQL.TrimEnd(","), String.Format(" WHERE ""{0}"" = '{1}'", strCboUsado.Trim(), strCodeCbo.Trim()))
            
            blnMsj = True
        End If
        Utilitarios.EjecutarConsulta(strSQL)

    End Sub

    Public Sub ActualizaAccesoriosPorModelo()
        Dim strCode As String
        Dim strName As String
        Dim strTamano As String
        Dim intTamano As Integer = 0
        Dim strCodigo As String
        Dim strSQL As String


        strTamano = Utilitarios.EjecutarConsulta("select MAX(""Code"" + 1) from ""@SCGD_ACCXMODE""")

        If Not String.IsNullOrEmpty(strTamano) Then intTamano = Integer.Parse(strTamano)

        If m_blnUsaModelo Then
            strCodigo = EditCboModelo.ObtieneValorUserDataSource()
        Else
            strCodigo = EditCboEstilo.ObtieneValorUserDataSource()
        End If

        FormularioSBO.Freeze(True)
        For i As Integer = 0 To dtAccVehiculo.Rows.Count - 1
            strCode = dtAccVehiculo.GetValue("code", i).ToString().Trim()
            strName = dtAccVehiculo.GetValue("name", i).ToString().Trim()

            If String.IsNullOrEmpty(Utilitarios.EjecutarConsulta(
                                    String.Format("SELECT ""Code"" FROM  ""@SCGD_ACCXMODE"" WHERE ""U_Accesorio"" = '{0}' AND ""U_Modelo"" = {1}",
                                                  strCode,
                                                  strCodigo))) Then

                strSQL = String.Format("INSERT INTO ""@SCGD_ACCXMODE"" (""Code"", ""Name"", ""U_Accesorio"", ""U_ItemName"", ""U_Modelo"") VALUES ({0},{0},'{1}','{2}','{3}')",
                                       intTamano,
                                       strCode,
                                       strName,
                                       strCodigo)
                Utilitarios.EjecutarConsulta(strSQL)
                blnMsj = True
                intTamano = intTamano + 1

            End If
        Next

        For Each codeEliminar As String In strFilasEliminar
            If Not String.IsNullOrEmpty(Utilitarios.EjecutarConsulta(
                                    String.Format("SELECT ""Code"" FROM  ""@SCGD_ACCXMODE"" WHERE ""U_Accesorio"" = '{0}' AND ""U_Modelo"" = {1}",
                                                  codeEliminar,
                                                  strCodigo))) Then

                strSQL = String.Format("DELETE FROM ""@SCGD_ACCXMODE"" WHERE ""U_Accesorio"" = '{0}' AND ""U_Modelo"" = '{1}'",
                                       codeEliminar,
                                       strCodigo)

                Utilitarios.EjecutarConsulta(strSQL)
                blnMsj = True
            End If
        Next
        FormularioSBO.Freeze(False)


    End Sub

    Public Sub EliminarAccesorioVehiculo(ByVal pval As SAPbouiCOM.ItemEvent)
        Dim l_numFila As String

        FormularioSBO.Freeze(True)

        If dtAccVehiculo.Rows.Count <> 0 Then
            If pval.ItemUID = m_strUIDMatAccVeh Then
                l_numFila = pval.Row - 1
            End If
            If Not IsNothing(l_numFila) AndAlso l_numFila >= 0 Then
                strFilasEliminar.Add(dtAccVehiculo.GetValue("code", l_numFila).ToString.Trim())
                dtAccVehiculo.Rows.Remove(l_numFila)
            End If
        End If
        MatrizAccVehi.Matrix.LoadFromDataSource()
        FormularioSBO.Freeze(False)
    End Sub
End Class


