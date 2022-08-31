Imports System.Globalization
Imports System.Threading
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports System.Timers
Imports ICompany = SAPbobsCOM.ICompany
Imports SCG_User_Interface.SCG_User_Interface


Partial Public Class CargarPanelCitas : Implements IFormularioSBO, IUsaMenu

#Region "Declaraciones"

    Private _cargaFormulario As CargaFormularioAsociaxEspDelegate
    Private _formType As String
    Private _nombreXml As String
    Private _titulo As String
    Private _menuPadre As String
    Private _nombreMenu As String
    Private _idMenu As String
    Private _posicion As Integer
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _applicationSbo As Application
    Private _companySbo As ICompany
    Private _strConexion As String
    Private _strDireccionReportes As String
    Private _strUsuarioBD As String
    Private _strContraseñaBD As String

    Private ButtonCargar As ButtonSBO
    Private EditComboSucursal As ComboBoxSBO

    Private UserDataSourceCargar As SAPbouiCOM.UserDataSources
    Private WithEvents _frmPanelCitaDotNet As frmListaCitas

    Dim m_oGestorFormularios As GestorFormularios
    Private m_oFormularioCitas As CitasReservacion
    Dim otmpForm As SAPbouiCOM.Form

    Private m_blnFlagTimer As Boolean = False
    Shared m_oTimer As System.Timers.Timer

    Private m_strUsaGruposTrabajo As String
    Private md_Local As SAPbouiCOM.DataTable

    Private versionSap As Integer
    Private m_blnVersion9 As Boolean = True

    Enum TipoDeAgenda
        Agenda = 1
        Equipos = 2
    End Enum

#End Region

#Region "Constructor"

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        _companySbo = companySbo
        _applicationSbo = application
        DMS_Connector.Helpers.SetCulture(Thread.CurrentThread.CurrentUICulture, My.Resources.Resource.Culture)

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
            _formType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
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
            Return _nombreMenu
        End Get
        Set(ByVal value As String)
            _nombreMenu = value
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

    Public Property StrConexion As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property StrDireccionReportes As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If Not FormularioSBO Is Nothing Then

            FormularioSBO.Freeze(True)

            For Each Item As SAPbouiCOM.Item In FormularioSBO.Items
                Item.AffectsFormMode = False
            Next
            FormularioSBO.Freeze(False)
            CargarFormulario()

        End If

    End Sub

    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles

        If Not FormularioSBO Is Nothing Then

            FormularioSBO.Freeze(True)

            UserDataSourceCargar = FormularioSBO.DataSources.UserDataSources
            UserDataSourceCargar.Add("colSuc", BoDataType.dt_LONG_TEXT, 200)


            EditComboSucursal = New ComboBoxSBO("cboSucur", _formularioSbo, True, "", "colSuc")
            EditComboSucursal.AsignaBinding()

            ButtonCargar = New ButtonSBO("btnCargar", _formularioSbo)
            'ButtonGrupo = New ButtonSBO("btnGrupo", FormularioSBO)

            FormularioSBO.Freeze(False)
        End If
    End Sub

    Public Sub CargarFormulario()
        Try
            md_Local = FormularioSBO.DataSources.DataTables.Add("dtLocal")

            CargarCombo()

            versionSap = _companySbo.Version
            If versionSap <= 900000 Then
                m_blnVersion9 = False
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el ComboBox Sucursal con los valores válidos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarCombo()
        Try
            Call CargarValoresValidosCombos("SELECT Code, Name FROM [@SCGD_SUCURSALES]  ORDER BY name", EditComboSucursal.UniqueId, True)
            SeleccionarSucursalUsuario(EditComboSucursal.Especifico)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Busca el usuario en el ComboBox sucursal y si la sucursal existe, la selecciona
    ''' </summary>
    ''' <param name="oComboBox">Objeto ComboBox de SAP</param>
    ''' <remarks></remarks>
    Private Sub SeleccionarSucursalUsuario(ByRef oComboBox As SAPbouiCOM.ComboBox)
        Dim strSucursal As String = String.Empty
        Try
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count > 0 Then
                strSucursal = ObtenerSucursalUsuario()
                If Not String.IsNullOrEmpty(strSucursal) Then
                    For Each oValidValue As SAPbouiCOM.ValidValue In oComboBox.ValidValues
                        If oValidValue.Value = strSucursal Then
                            oComboBox.Select(strSucursal, SAPbouiCOM.BoSearchKey.psk_ByValue)
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve la sucursal del usuario conectado a SAP
    ''' </summary>
    ''' <returns>Código de la sucursal del usuario conectado</returns>
    ''' <remarks></remarks>
    Private Function ObtenerSucursalUsuario() As String
        Dim oUser As SAPbobsCOM.Users
        Dim strSucursal As String = String.Empty
        Dim strInternalKey As String = String.Empty
        Try
            strInternalKey = DMS_Connector.Company.CompanySBO.UserSignature
            oUser = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUsers)
            oUser.GetByKey(strInternalKey)
            strSucursal = oUser.Branch
            Return strSucursal
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strSucursal
        End Try
    End Function

    Private Sub CargarValoresValidosCombos(ByVal p_strSQL As String, ByRef p_strIDItem As String, Optional ByVal p_blnUsarVacio As Boolean = False)
        Try

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            Dim oItem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim intRecIndex As Integer
            _formularioSbo.Freeze(True)

            oItem = FormularioSBO.Items.Item(p_strIDItem)
            oCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            md_Local.Clear()
            md_Local.ExecuteQuery(p_strSQL)

            If oCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To oCombo.ValidValues.Count - 1
                    oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            If p_blnUsarVacio Then
                oCombo.ValidValues.Add("", "")
            End If

            For i As Integer = 0 To md_Local.Rows.Count - 1
                oCombo.ValidValues.Add(md_Local.GetValue(0, i), md_Local.GetValue(1, i))
            Next

            If md_Local.Rows.Count = 1 Then
                oCombo.Select(md_Local.GetValue(0, 0), SAPbouiCOM.BoSearchKey.psk_ByValue)
            End If

            _formularioSbo.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub


    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String,
                                         ByRef pVal As SAPbouiCOM.ItemEvent,
                                         ByRef BubbleEvent As Boolean,
                                         ByRef oCitasReservacion As CitasReservacion)

        If Not pVal.FormTypeEx = FormType Then Return
        m_oFormularioCitas = oCitasReservacion

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            ManejadorEventosItemPressed(FormUID, pVal, BubbleEvent)
        ElseIf pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
            ManejadorEventosComboSelect(FormUID, pVal, BubbleEvent)

        End If

    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim strCodSucur As String

        FormularioSBO.Freeze(True)

        If pVal.ActionSuccess Then
            Select Case pVal.ItemUID
                Case ButtonCargar.UniqueId
                    ButtonCargarItemPressed(FormUID, pVal, BubbleEvent)
            End Select

        ElseIf pVal.BeforeAction Then
            strCodSucur = EditComboSucursal.ObtieneValorUserDataSource

            If String.IsNullOrEmpty(strCodSucur) Then
                _applicationSbo.SetStatusBarMessage("Debe seleccionar una sucursal para mostrar la agenda", BoMessageTime.bmt_Short, True)
                BubbleEvent = False
            End If

        End If


        FormularioSBO.Freeze(False)

    End Sub

    Private Sub ManejadorEventosComboSelect(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim l_strSucur As String

        FormularioSBO.Freeze(True)
        If pVal.Action_Success Then

            Select Case pVal.ItemUID

                Case EditComboSucursal.UniqueId
                    l_strSucur = EditComboSucursal.ObtieneValorUserDataSource

                    m_strUsaGruposTrabajo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_GrpTrabajo FROM [@SCGD_CONF_SUCURSAL] WHERE U_Sucurs = '{0}'", l_strSucur), _companySbo.CompanyDB, _companySbo.Server)

            End Select

        End If


        FormularioSBO.Freeze(False)

    End Sub

    Private Sub HandlerTimer(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs)

        If m_blnFlagTimer Then
            _applicationSbo.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        End If
    End Sub

    Public Sub IniciaTimer()

        m_blnFlagTimer = True

        m_oTimer = New System.Timers.Timer()
        AddHandler m_oTimer.Elapsed, New ElapsedEventHandler(AddressOf HandlerTimer)
        m_oTimer.Interval = 30000
        m_oTimer.Enabled = True

    End Sub

    Private Sub FinalizaTimer()
        m_blnFlagTimer = False

        m_oTimer.Enabled = False
        m_oTimer.Stop()
        m_oTimer.Dispose()

    End Sub

#End Region

End Class
