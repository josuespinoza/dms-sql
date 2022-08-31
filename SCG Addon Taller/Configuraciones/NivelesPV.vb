Imports System.Collections.Generic
Imports DMS_Addon.ControlesSBO
Imports SAPbouiCOM
Imports SCG.SBOFramework.UI
Imports System.Linq
Imports SAPbobsCOM


Public Class NivelesPV : Implements IUsaPermisos, IFormularioSBO

#Region "Declaraciones"
    Private _FormType As String
    Private _NombreXml As String
    Private _Titulo As String
    Private _FormularioSBO As IForm
    Private _Inicializado As Boolean
    Private _ApplicationSBO As IApplication
    Private _CompanySBO As SAPbobsCOM.ICompany
    Private _IdMenu As String
    Private _MenuPadre As String
    Private _Posicion As Integer
    Private _Nombre As String
    Private oPermisosPV_list As PermisosPV_List
    Private oPermisosPV As PermisosPV

    ''Variables Globales
    Private oDbSCGD_NIVELES_PV As SAPbouiCOM.DBDataSource
    Private oDbSCGD_PERMISOS_PV As SAPbouiCOM.DBDataSource
    Private oDbOUSR As SAPbouiCOM.DBDataSource
    Private oDTPermisosPV As SAPbouiCOM.DataTable
    Private oDTPermisosPVTemp As SAPbouiCOM.DataTable
    Private oMatrix As SAPbouiCOM.Matrix
    Private oGeneralService As SAPbobsCOM.GeneralService
    Private oGeneralParams As SAPbobsCOM.GeneralDataParams
    Public oFormularioPermisosDeAcceso As PermisosDeAcceso
    Public oGestorFormularios As GestorFormularios

#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _ApplicationSBO
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _CompanySBO
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _FormType
        End Get
        Set(value As String)
            _FormType = value
        End Set
    End Property

    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _FormularioSBO
        End Get
        Set(value As SAPbouiCOM.IForm)
            _FormularioSBO = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _Inicializado
        End Get
        Set(value As Boolean)
            _Inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _NombreXml
        End Get
        Set(value As String)
            _NombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _Titulo
        End Get
        Set(value As String)
            _Titulo = value
        End Set
    End Property

    Public Property IdMenu As String Implements SCG.SBOFramework.UI.IUsaMenu.IdMenu
        Get
            Return _IdMenu
        End Get
        Set(value As String)
            _IdMenu = value
        End Set
    End Property

    Public Property MenuPadre As String Implements SCG.SBOFramework.UI.IUsaMenu.MenuPadre
        Get
            Return _MenuPadre
        End Get
        Set(value As String)
            _MenuPadre = value
        End Set
    End Property

    Public Property Nombre As String Implements SCG.SBOFramework.UI.IUsaMenu.Nombre
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
        End Set
    End Property

    Public Property Posicion As Integer Implements SCG.SBOFramework.UI.IUsaMenu.Posicion
        Get
            Return _Posicion
        End Get
        Set(value As Integer)
            _Posicion = value
        End Set
    End Property

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Contructor para la aplicacion
    ''' </summary>
    ''' <param name="p_Application"></param>
    ''' <param name="p_CompanySbo"></param>
    ''' <remarks></remarks>
    <System.CLSCompliant(False)> _
    Public Sub New(ByVal p_Application As Application, ByVal p_CompanySbo As SAPbobsCOM.ICompany, ByVal mc_strUISCGD_FormPermisos As String)
        _CompanySBO = p_CompanySbo
        _ApplicationSBO = p_Application
        NombreXml = System.Windows.Forms.Application.StartupPath & My.Resources.Resource.NIVELES_PVForm
        MenuPadre = "SCGD_CFG"
        Nombre = My.Resources.Resource.txtPermisosAcceso
        IdMenu = mc_strUISCGD_FormPermisos
        Titulo = My.Resources.Resource.txtPermisosAcceso
        Posicion = 5
        FormType = mc_strUISCGD_FormPermisos
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Inicializa Formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario
        Try
            FormularioSBO.Freeze(True)

            ''Carga Grid Permisos PV
            oDbSCGD_NIVELES_PV = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_NIVELES_PV")
            ''Inicializa DataTable
            oDTPermisosPV = FormularioSBO.DataSources.DataTables.Item("dtPermisosPV")
            oDTPermisosPVTemp = FormularioSBO.DataSources.DataTables.Item("dtPermisosPVTemp")

            oDbSCGD_NIVELES_PV.Query()
            If oDbSCGD_NIVELES_PV.Size > 0 Then
                For i As Integer = 0 To oDbSCGD_NIVELES_PV.Size - 1
                    oDTPermisosPV.Rows.Add()
                    oDTPermisosPV.SetValue("colSelect", i, "N")
                    oDTPermisosPV.SetValue("colCode", i, oDbSCGD_NIVELES_PV.GetValue("Code", i).Trim())
                    oDTPermisosPV.SetValue("colName", i, oDbSCGD_NIVELES_PV.GetValue("Name", i).Trim())
                Next
            End If

            ''Carga Permisos de Usuarios Existentes a la lista
            oDbSCGD_PERMISOS_PV = FormularioSBO.DataSources.DBDataSources.Item("@SCGD_PERMISOS_PV")
            If oDbSCGD_PERMISOS_PV.Size > 0 Then
                oPermisosPV_list = New PermisosPV_List()
                CargaListaPermisosPV()
            End If


            ''Carga Matriz Usuarios
            oDbOUSR = FormularioSBO.DataSources.DBDataSources.Item("OUSR")
            oMatrix = FormularioSBO.Items.Item("mtxOUSR").Specific
            oDbOUSR.Query()
            oMatrix.LoadFromDataSourceEx()

            FormularioSBO.DataSources.UserDataSources.Item("udUserId").ValueEx = -1
            FormularioSBO.DataSources.UserDataSources.Item("udAll").ValueEx = "N"

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa Controles
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        Try
            oGeneralService = CompanySBO.GetCompanyService.GetGeneralService("SCGD_NIVELES_PV")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oFormularioPermisosDeAcceso = New PermisosDeAcceso(ApplicationSBO, CompanySBO, "SCGD_NIVELES_PV")
            oGestorFormularios = New GestorFormularios(ApplicationSBO)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' EventoItemPressed
    ''' </summary>
    ''' <param name="p_FormUID"></param>
    ''' <param name="p_pVal"></param>
    ''' <param name="p_BubbleEvent"></param>
    ''' <remarks></remarks>
    Sub ManejadorEventoItemPressed(p_FormUID As String, p_pVal As SAPbouiCOM.ItemEvent, ByRef p_BubbleEvent As Boolean)
        Dim strUser As String
        Try
            If p_pVal.ActionSuccess Then
                FormularioSBO.Freeze(True)

                ''Carga Permisos Usuario Seleccionado
                If p_pVal.ColUID = "colOUSR" Then
                    If ObtieneUsuarioMatrix(strUser) Then
                        CargarPermisosUsuario(strUser)
                    End If
                End If

                Select Case p_pVal.ItemUID
                    Case "btnClear"
                        ''Limpia Matriz OUSR y Checks de permisos
                        BuscarUsuario(oDbOUSR)

                    Case "btnCrear"
                        ''Abre Formulario de Niveles Plan Ventas para crear permiso nuevo
                        If (oFormularioPermisosDeAcceso IsNot Nothing) Then
                            If Not oGestorFormularios.FormularioAbierto(oFormularioPermisosDeAcceso, activarSiEstaAbierto:=True) Then
                                oFormularioPermisosDeAcceso.FormularioSBO = oGestorFormularios.CargaFormulario(oFormularioPermisosDeAcceso)
                            End If
                        End If

                    Case "btnUpdate"
                        ''Actualiza los permisos del usuario seleccionado
                        If ObtieneUsuarioMatrix(strUser) Then
                            If oDTPermisosPV.Rows.Count > 0 Then
                                ActualizarPermisosUsuario(strUser)
                            End If
                        Else
                            ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.NivelesPVSeleccioneUsuario, BoMessageTime.bmt_Short)
                        End If

                    Case "chkAll"
                        ''Selecciona todos los permisos o los deselecciona
                        If ObtieneUsuarioMatrix(strUser) Then
                            If oDTPermisosPV.Rows.Count > 0 Then
                                ManejoCheckAll(FormularioSBO.DataSources.UserDataSources.Item("udAll").ValueEx)
                            End If
                        Else
                            FormularioSBO.DataSources.UserDataSources.Item("udAll").ValueEx = "N"
                            ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.NivelesPVSeleccioneUsuario, BoMessageTime.bmt_Short)
                        End If
                End Select

                FormularioSBO.Freeze(False)
            Else
                If p_pVal.ColUID = "colSelect" Then
                    FormularioSBO.Freeze(True)
                    '''Valida que hay un usuario seleccionado
                    If Not ObtieneUsuarioMatrix(strUser) Then
                        If p_pVal.Row > -1 And oDTPermisosPV.Rows.Count > 0 Then
                            oDTPermisosPV.SetValue("colSelect", p_pVal.Row, "N")
                        End If
                        ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.NivelesPVSeleccioneUsuario, BoMessageTime.bmt_Short)
                        p_BubbleEvent = False
                    End If

                    FormularioSBO.Freeze(False)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' EventoChooseFromList
    ''' </summary>
    ''' <param name="p_pval"></param>
    ''' <param name="p_FormID"></param>
    ''' <param name="p_BubbleEvent"></param>
    ''' <remarks></remarks>
    Public Sub ManejadorEventoChooseFromList(p_pval As ItemEvent, p_FormID As String, p_BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Try
            oCFLEvento = CType(p_pval, SAPbouiCOM.IChooseFromListEvent)
            If Equals(oCFLEvento.ChooseFromListUID, "cflOUSR") Then
                If oCFLEvento.SelectedObjects.Rows.Count > 0 Then
                    FormularioSBO.DataSources.UserDataSources.Item("udUser").ValueEx = oCFLEvento.SelectedObjects.GetValue("USER_CODE", 0)
                    FormularioSBO.DataSources.UserDataSources.Item("udUserId").ValueEx = oCFLEvento.SelectedObjects.GetValue("USERID", 0)
                    BuscarUsuario(oDbOUSR, FormularioSBO.DataSources.UserDataSources.Item("udUser").ValueEx.Trim(), "USER_CODE")
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Carga los permisos al grid del usuario seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarPermisosUsuario(p_strU_Usuario As String)
        Dim oUsuarioPermisosPV_list As List(Of PermisosPV)
        Try
            ''Filtra los permisos por el usuario seleccionado
            oUsuarioPermisosPV_list = oPermisosPV_list.Where(Function(x) x.U_Usuario = p_strU_Usuario).ToList()

            If oUsuarioPermisosPV_list.Count > 0 Then
                For k As Integer = 0 To oDTPermisosPV.Rows.Count - 1
                    If oUsuarioPermisosPV_list.Any(Function(x) x.Code = oDTPermisosPV.GetValue("colCode", k)) Then
                        oDTPermisosPV.SetValue("colSelect", k, "Y")
                    Else
                        oDTPermisosPV.SetValue("colSelect", k, "N")
                    End If
                Next
            Else
                For l As Integer = 0 To oDTPermisosPV.Rows.Count - 1
                    oDTPermisosPV.SetValue("colSelect", l, "N")
                Next
            End If

            ''Copia Permisos de Usuario para la actualización de permisos
            oDTPermisosPVTemp.CopyFrom(oDTPermisosPV)
            FormularioSBO.DataSources.UserDataSources.Item("udAll").ValueEx = "N"
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el permiso seleccionado del usuario
    ''' </summary>
    ''' <param name="p_strU_Usuario">Código de Usuario</param>
    ''' <param name="p_strCode">Código de Permiso</param>
    ''' <remarks></remarks>
    Private Function AgregarPermisosUsuario(p_strU_Usuario As String, p_strCode As String) As Boolean
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection
        Dim oChild As SAPbobsCOM.GeneralData

        Try
            oGeneralParams.SetProperty("Code", p_strCode)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oChildren = oGeneralData.Child("SCGD_PERMISOS_PV")
            oChild = oChildren.Add()
            oChild.SetProperty("U_Usuario", p_strU_Usuario)
            oGeneralService.Update(oGeneralData)

            ''Guarda el permisos en la lista
            oPermisosPV = New PermisosPV()
            oPermisosPV.Code = p_strCode
            oPermisosPV.U_Usuario = p_strU_Usuario
            oPermisosPV_list.Add(oPermisosPV)
            
            ApplicationSBO.SetStatusBarMessage(String.Format(My.Resources.Resource.NivelesPVUsuarioAgregado, p_strCode, p_strU_Usuario), BoMessageTime.bmt_Short, False)
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Elimina el permiso deseleccionado del usuario
    ''' </summary>
    ''' <param name="p_strU_Usuario">Código de Usuario</param>
    ''' <param name="p_strCode">Código de Permiso</param>
    ''' <remarks></remarks>
    Private Function EliminarPermisosUsuario(p_strU_Usuario As String, p_strCode As String) As Boolean
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildren As SAPbobsCOM.GeneralDataCollection
        Dim oChild As SAPbobsCOM.GeneralData
        Dim intChild As Integer
        Try
            oGeneralParams.SetProperty("Code", p_strCode)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)
            oChildren = oGeneralData.Child("SCGD_PERMISOS_PV")
            intChild = 0
            For Each oChild In oChildren
                If String.Equals(oChild.GetProperty("U_Usuario"), p_strU_Usuario) Then
                    oChildren.Remove(intChild)
                    Exit For
                End If
                intChild += 1
            Next
            oGeneralService.Update(oGeneralData)

            ''Elimina el registro de la lista de permisos
            oPermisosPV_list.RemoveAll(Function(x) x.Code = p_strCode And x.U_Usuario = p_strU_Usuario)

            ApplicationSBO.SetStatusBarMessage(String.Format(My.Resources.Resource.NivelesPVUsuarioEliminado, p_strCode, p_strU_Usuario), BoMessageTime.bmt_Short, False)
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el usuario seleccionado en la matrix mtxOUSR
    ''' </summary>
    ''' <param name="p_strUser">Devuelve el código del usuario seleccionado</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtieneUsuarioMatrix(ByRef p_strUser As String) As Boolean
        Try
            If oMatrix.RowCount > 0 Then
                If FormularioSBO.DataSources.DBDataSources.Item("OUSR").Size > 0 Then
                    For i As Integer = 1 To oMatrix.RowCount
                        If oMatrix.IsRowSelected(i) Then
                            p_strUser = FormularioSBO.DataSources.DBDataSources.Item("OUSR").GetValue("USER_CODE", i - 1).Trim()
                            Return True
                        End If
                    Next
                End If
                Return False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Busca Usuario Seleccionado en el Choose From List y también limpia la matrix de la búsqueda
    ''' </summary>
    ''' <param name="p_oDBDataSource"></param>
    ''' <param name="p_strValor"></param>
    ''' <param name="p_strCampo"></param>
    ''' <remarks></remarks>
    Private Sub BuscarUsuario(p_oDBDataSource As DBDataSource, Optional ByVal p_strValor As String = "", Optional ByVal p_strCampo As String = "")
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Try
            oMatrix.FlushToDataSource()
            If Not String.IsNullOrEmpty(p_strValor) Then
                oConditions = ApplicationSBO.CreateObject(BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add()
                oCondition.Alias = p_strCampo
                oCondition.Operation = BoConditionOperation.co_CONTAIN
                oCondition.CondVal = p_strValor
                p_oDBDataSource.Query(oConditions)
            Else
                ''Limpia la matriz y los valores de búsqueda de ChooseFrom List
                FormularioSBO.DataSources.UserDataSources.Item("udUser").ValueEx = String.Empty
                FormularioSBO.DataSources.UserDataSources.Item("udUserId").ValueEx = -1
                p_oDBDataSource.Query()
            End If

            For i As Integer = 0 To oDTPermisosPV.Rows.Count - 1
                oDTPermisosPV.SetValue("colSelect", i, "N")
            Next

            FormularioSBO.DataSources.UserDataSources.Item("udAll").ValueEx = "N"
            oMatrix.LoadFromDataSource()

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la lista de Permisos PV al actualizar permisos de usuarios y al iniciar el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaListaPermisosPV()
        Try
            oDbSCGD_PERMISOS_PV.Query()
            oPermisosPV_list.Clear()
            For j As Integer = 0 To oDbSCGD_PERMISOS_PV.Size - 1
                oPermisosPV = New PermisosPV()
                oPermisosPV.Code = oDbSCGD_PERMISOS_PV.GetValue("Code", j).Trim
                oPermisosPV.U_Usuario = oDbSCGD_PERMISOS_PV.GetValue("U_Usuario", j).Trim
                oPermisosPV_list.Add(oPermisosPV)
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza los permisos del usuario crea o elimina
    ''' </summary>
    ''' <param name="p_strUser"></param>
    ''' <remarks></remarks>
    Private Sub ActualizarPermisosUsuario(p_strUser As String)
        Dim blnCommit As Boolean
        Try
            blnCommit = True
            If Not CompanySBO.InTransaction Then

                CompanySBO.StartTransaction()

                ''Verifica los cambios en los permisos para el usuarios entre el dt ligado al grid y el que carga los permisos iniciales temporales del user
                For i As Integer = 0 To oDTPermisosPV.Rows.Count - 1

                    If Not Equals(oDTPermisosPV.GetValue("colSelect", i), oDTPermisosPVTemp.GetValue("colSelect", i)) Then

                        If oDTPermisosPV.GetValue("colSelect", i) = "Y" Then
                            If Not AgregarPermisosUsuario(p_strUser, oDTPermisosPV.GetValue("colCode", i)) Then
                                blnCommit = False
                                Exit For
                            End If
                        Else
                            If Not EliminarPermisosUsuario(p_strUser, oDTPermisosPV.GetValue("colCode", i)) Then
                                blnCommit = False
                                Exit For
                            End If
                        End If

                    End If
                Next

                If CompanySBO.InTransaction And blnCommit Then
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                    CargaListaPermisosPV()
                    CargarPermisosUsuario(p_strUser)
                    oDTPermisosPVTemp.CopyFrom(oDTPermisosPV)
                Else
                    ''En caso de haber error revierte los checks marcados o desmarcados
                    CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
                    CargaListaPermisosPV()
                    CargarPermisosUsuario(p_strUser)
                End If
                FormularioSBO.DataSources.UserDataSources.Item("udAll").ValueEx = "N"
            End If

        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' "des/marca todos"
    ''' </summary>
    ''' <param name="p_strCheck"></param>
    ''' <remarks></remarks>
    Private Sub ManejoCheckAll(p_strCheck As String)
        Try
            If Not String.IsNullOrEmpty(p_strCheck) Then
                For i As Integer = 0 To oDTPermisosPV.Rows.Count - 1
                    If Not Equals(oDTPermisosPV.GetValue("colSelect", i), p_strCheck) Then
                        oDTPermisosPV.SetValue("colSelect", i, p_strCheck)
                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#End Region

End Class
