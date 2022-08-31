Option Explicit On

Imports System.Globalization
Imports System.IO
Imports System.Collections.Generic
Imports SCG.DMSOne.Framework.MenuManager
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SAPbouiCOM
Imports DMSOneFramework

Partial Public Class UsuariosPorNAprob

#Region "Declaraciones"
    'objeto form 
    Private oForm As SAPbouiCOM.Form
    Private _dtConfigLineas As SAPbouiCOM.DataTable


    Private m_oCompany As SAPbobsCOM.Company
    Private m_strBDConfiguracion As String
    Private m_strBDTalller As String
    Dim oGestorFormularios As GestorFormularios

    Private m_SBO_Application As SAPbouiCOM.Application
    Public n As NumberFormatInfo

    'objeto datatable 
    Private _dtEditText As DataTable

    'Conexion a los componentes que NO se encuentran en la matriz - Los EditText
    Dim userDS As UserDataSources

    'objeto matriz
    Private MatrizUsuarios As MatrizUsuarios
    Private Const str_tbUsuarios As String = "OUSR"
    Private Const strtb_LocalEditText As String = "dtET"
    Private Const strtb_LocalUserConsulta As String = "dtUserConsulta"
    Private Const strtb_LocalUser As String = "dtUser"
    Private Const strMatrizUsuarios As String = "mtx_User"

    Private m_strConectionString As String
    Dim m_cn_Coneccion As New SqlClient.SqlConnection

    Dim oMatrix As SAPbouiCOM.Matrix

    Private Shared dtUsuariosMatriz As SAPbouiCOM.DataTable
    Private Shared dtUsuariosConsulta As SAPbouiCOM.DataTable
    Private Shared dtElimina As SAPbouiCOM.DataTable
    Private Shared dtUsuariosEnBD As SAPbouiCOM.DataTable
    Private Shared _formMsj As SAPbouiCOM.Form

    'agrega
    Private Shared _agrega As Boolean

    'str
    Private Shared _strNivelAprobacion As String
    Private Shared _strSucursal As String

#End Region

#Region "Constructor"


#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, _
                   ByVal SBOAplication As Application)

        'declaracion de objetos acplication , company y decimales 
        m_oCompany = ocompany
        m_SBO_Application = SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region


#End Region

#Region "Propiedades"

    Public Shared Property FormMSJ As Form
        Get
            Return _formMsj
        End Get
        Set(ByVal value As Form)
            _formMsj = value
        End Set
    End Property

    Public Shared Property Agrega As Boolean
        Get
            Return _agrega
        End Get
        Set(ByVal value As Boolean)
            _agrega = value
        End Set
    End Property

    Public Shared Property StrNivelAprobacion As String
        Get
            Return _strNivelAprobacion
        End Get
        Set(ByVal value As String)
            _strNivelAprobacion = value
        End Set
    End Property

    Public Shared Property StrSucursal As String
        Get
            Return _strSucursal
        End Get
        Set(ByVal value As String)
            _strSucursal = value
        End Set
    End Property

    Public Property dtConfigLineas As DataTable
        Get
            Return _dtConfigLineas
        End Get
        Set(ByVal value As DataTable)
            _dtConfigLineas = value
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub CargaFormUnidades(ByVal p_form As Form, ByVal AgregaUsuarios As Boolean, ByRef p_dtConfigLineas As SAPbouiCOM.DataTable)
        'variables a utilizar
        Dim fcp As SAPbouiCOM.FormCreationParams
        Dim strXMLACargar As String
        Dim oBtn As Button

        Try
            'parametros para el form que se abrirá
            fcp = m_SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_UXN"

            'se designa el XML que se cargara
            strXMLACargar = My.Resources.Resource.XMLUsuariosXNivel
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            oForm = m_SBO_Application.Forms.AddEx(fcp)
            oForm.Mode = BoFormMode.fm_UPDATE_MODE
            'obtengo los valores de parametros
            FormMSJ = p_form
            dtConfigLineas = p_dtConfigLineas
            Agrega = AgregaUsuarios

            Call LinkMatriz()

            'matriz de usuarios
            oMatrix = DirectCast(oForm.Items.Item(strMatrizUsuarios).Specific, SAPbouiCOM.Matrix)
            
            dtUsuariosConsulta = oForm.DataSources.DataTables.Item(strtb_LocalUserConsulta)

            dtUsuariosMatriz = oForm.DataSources.DataTables.Item(strtb_LocalUser)
            dtElimina = oForm.DataSources.DataTables.Add("EliUsua")
            dtUsuariosEnBD = oForm.DataSources.DataTables.Add("dtUsuariosEnBD")

            'boton
            oBtn = DirectCast(oForm.Items.Item("btnSel").Specific, Button)

            'Si el formulario se carga para agregar
            If Agrega Then
                oBtn.Caption = My.Resources.Resource.Agregar
                Call CargarMatriz(oMatrix, oForm, "SELECT USERID, USER_CODE, U_NAME FROM OUSR  order by USER_CODE ")
            ElseIf Not Agrega Then
                oForm.Items.Item("btnBuscar").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oForm.Items.Item("txtUserC").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)

                oBtn.Caption = My.Resources.Resource.Eliminar
                Call CargarMatriz(oMatrix, oForm, String.Format(" SELECT O.USERID , O.USER_CODE, O.U_NAME " & _
                                                            " FROM OUSR AS O INNER JOIN [@SCGD_MSJS1] AS M1 " & _
                                                            " ON O.USER_CODE = M1.U_Usua  WHERE M1.U_CSucu = '{0}' " & _
                                                            " AND M1.U_CNAp = '{1}'  order by USER_CODE ", StrSucursal, StrNivelAprobacion))
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Function CargarMatriz(ByRef oMatrix As SAPbouiCOM.Matrix, _
                                       ByVal oform As SAPbouiCOM.Form, _
                                       ByVal Consulta As String) As Boolean

        Dim strConsulta As String = ""
        strConsulta = Consulta
        Try
            oMatrix.Clear()
            dtUsuariosConsulta.Rows.Clear()
            dtUsuariosMatriz.Rows.Clear()
            If Not String.IsNullOrEmpty(strConsulta) Then
                dtUsuariosConsulta.ExecuteQuery(strConsulta)
            End If

            CargaUsuariosEnDt()

            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, m_SBO_Application)
            Return False
        End Try

    End Function

    Private Sub CargaUsuariosEnDt()
        For i As Integer = 0 To dtUsuariosConsulta.Rows.Count - 1
            dtUsuariosMatriz.Rows.Add()
            dtUsuariosMatriz.SetValue("id", i, dtUsuariosConsulta.GetValue("USERID", i))
            dtUsuariosMatriz.SetValue("cod", i, dtUsuariosConsulta.GetValue("USER_CODE", i))
            dtUsuariosMatriz.SetValue("name", i, dtUsuariosConsulta.GetValue("U_NAME", i))
        Next
    End Sub


    'inserta usuarios por nivel y sucursal
    Public Sub InsertaNivXUsuarios(ByRef pval As SAPbouiCOM.ItemEvent, ByVal str_Usuarios As List(Of String), ByVal str_Names As List(Of String),
                                   ByVal str_CodSucursal As String, ByVal str_CodNivAprob As String,
                                   ByVal oMatrizUser As SAPbouiCOM.Matrix, ByVal oMatrizMSJ As SAPbouiCOM.Matrix,
                                   ByVal p_dtConfigLineas As SAPbouiCOM.DataTable)

        Dim EditValue As SAPbouiCOM.EditText
        Dim UltimoLineID As Integer = 0
        Dim Posicion As Integer = 0

        'If oMatrizMSJ.RowCount > 0 Then
        '    EditValue = DirectCast(oMatrizMSJ.GetCellSpecific("Col_LineId", oMatrizMSJ.RowCount), SAPbouiCOM.EditText)
        '    UltimoLineID = Integer.Parse(EditValue.Value.ToString().Trim())
        'Else
        '    UltimoLineID = 0
        'End If

        Posicion = p_dtConfigLineas.Rows.Count

        For x As Integer = 0 To str_Usuarios.Count - 1
            p_dtConfigLineas.Rows.Add(1)
            p_dtConfigLineas.SetValue("name", Posicion, str_Names(x))
            p_dtConfigLineas.SetValue("usua", Posicion, str_Usuarios(x))
            p_dtConfigLineas.SetValue("cnap", Posicion, StrNivelAprobacion)
            p_dtConfigLineas.SetValue("csucu", Posicion, StrSucursal)
            p_dtConfigLineas.SetValue("rmsj", Posicion, "Y")
            p_dtConfigLineas.SetValue("mcv", Posicion, "Y")
            p_dtConfigLineas.SetValue("acv", Posicion, "Y")

            'UltimoLineID = UltimoLineID + 1
            Posicion = Posicion + 1
        Next

        oMatrizMSJ.LoadFromDataSource()
    End Sub

    'elimina usuarios por nivel y sucursal
    Public Sub EliminaNivXUsuarios(ByVal p_dtConfigLineas As SAPbouiCOM.DataTable)

        Dim oMatrizMSJ As SAPbouiCOM.Matrix
        Dim oMatrizUser As SAPbouiCOM.Matrix
        Dim int_IndicesAEliminar As New List(Of Integer)
        Dim str_Lista As String = ""
        Dim strConsulta As String = ""
        Dim strLineEliminar As String = ""

        'Matrices
        oMatrizMSJ = DirectCast(FormMSJ.Items.Item("mtx_MSJ").Specific, SAPbouiCOM.Matrix)
        oMatrizUser = DirectCast(oForm.Items.Item("mtx_User").Specific, SAPbouiCOM.Matrix)
        ConfiguracionNivAprobacion._int_IndicesAEliminar.Clear()
        ConfiguracionNivAprobacion._int_IndicesAEliminarTB.Clear()
        For i As Integer = 1 To oMatrizUser.RowCount
            If oMatrizUser.IsRowSelected(i) Then
                If String.IsNullOrEmpty(str_Lista) Then
                    str_Lista = String.Format("'{0}'", dtUsuariosMatriz.GetValue("cod", i - 1).ToString)
                Else
                    str_Lista = str_Lista & String.Format(",'{0}'", dtUsuariosMatriz.GetValue("cod", i - 1).ToString)
                End If
            End If
        Next

        'carga los lineid a eliminar del datasource
        strConsulta = String.Format("SELECT LineId, U_Usua, U_CNAp, U_CSucu FROM [@SCGD_MSJS1] AD " & _
                                    " WHERE AD.U_CNAp = '{0}' AND AD.U_CSucu = '{1}'AND AD.U_Usua IN ({2})",
                                    StrNivelAprobacion, StrSucursal, str_Lista)
        dtUsuariosEnBD.ExecuteQuery(String.Format(" SELECT LineId, U_Usua, U_Name, U_CSucu, U_CNAp, U_ManejaApro FROM [@SCGD_MSJS1] where U_CSucu = '{0}' ", StrSucursal))
        dtElimina.ExecuteQuery(strConsulta)

        'agrego en una lista los 
        For rowEliminar As Integer = 0 To dtElimina.Rows.Count - 1
            strLineEliminar = dtElimina.GetValue("LineId", rowEliminar)
            For rowEliminaConfigLinea As Integer = 0 To p_dtConfigLineas.Rows.Count - 1
                If strLineEliminar.Trim = p_dtConfigLineas.GetValue("lineid", rowEliminaConfigLinea).Trim Then
                    ConfiguracionNivAprobacion._int_IndicesAEliminar.Add(rowEliminaConfigLinea)
                    For index As Integer = 0 To dtUsuariosEnBD.Rows.Count - 1
                        If strLineEliminar = dtUsuariosEnBD.GetValue("LineId", index) Then
                            ConfiguracionNivAprobacion._int_IndicesAEliminarTB.Add(index)
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next
        Next
        For index As Integer = 0 To ConfiguracionNivAprobacion._int_IndicesAEliminar.Count - 1
            p_dtConfigLineas.Rows.Remove(ConfiguracionNivAprobacion._int_IndicesAEliminar(index) - index)
        Next

        'Dim PosiInd As Integer = int_IndicesAEliminar.Count

        'For i As Integer = 1 To int_IndicesAEliminar.Count

        '    'FormMSJ.DataSources.DBDataSources.Item("@SCGD_MSJS1").RemoveRecord(int_IndicesAEliminar(PosiInd - 1))
        '    PosiInd = PosiInd - 1
        'Next

        oMatrizMSJ.LoadFromDataSource()

        'dtUsuariosEnBD.ExecuteQuery(String.Format("SELECT U_CNAp, U_Usua, U_Name FROM [@SCGD_MSJS1] WHERE U_CSucu = '{0}'", StrSucursal))
        'dtUsuariosEnBD.ExecuteQuery(" SELECT U_CNAp, U_Usua, U_Name, U_CSucu FROM [@SCGD_MSJS1] ")

        'For i As Integer = 0 To dtElimina.Rows.Count - 1
        '    For x As Integer = 0 To dtUsuariosEnBD.Rows.Count - 1
        '        If dtUsuariosEnBD.GetValue("U_Usua", x) = dtElimina.GetValue("U_Usua", i) And
        '            dtUsuariosEnBD.GetValue("U_CNAp", x) = dtElimina.GetValue("U_CNAp", i) And
        '            dtUsuariosEnBD.GetValue("U_CSucu", x) = dtElimina.GetValue("U_CSucu", i) Then
        '            ConfiguracionNivAprobacion._int_IndicesAEliminar.Add(i)
        '        End If
        '    Next
        'Next

    End Sub

    Public Sub ValidaUsuarios(ByRef pval As SAPbouiCOM.ItemEvent,
                                   ByVal strUsuarios As List(Of String),
                                   ByVal strCodSucursal As String,
                                   ByVal strCodNivAprob As String,
                                   ByVal oMatriz As SAPbouiCOM.Matrix,
                                   ByRef BubbleEvent As Boolean)

        Try
            Dim oEditUsuario As SAPbouiCOM.EditText
            Dim oEditSucu As SAPbouiCOM.EditText
            Dim oEditNivApr As SAPbouiCOM.EditText
            Dim strUsuario_local As String = ""
            Dim strNAp_local As String = ""
            Dim strSucu_local As String = ""

            For i As Integer = 0 To strUsuarios.Count - 1
                For x As Integer = 1 To oMatriz.RowCount

                    oEditUsuario = oMatriz.Columns.Item("Col_Usua").Cells.Item(x).Specific
                    oEditSucu = oMatriz.Columns.Item("Col_CSucu").Cells.Item(x).Specific
                    oEditNivApr = oMatriz.Columns.Item("Col_CNAp").Cells.Item(x).Specific

                    'elimina espacios
                    strUsuario_local = oEditUsuario.Value.Trim()
                    strSucu_local = oEditSucu.Value.Trim()
                    strNAp_local = oEditNivApr.Value.Trim()

                    'elimino espacios
                    strCodSucursal = strCodSucursal.Trim
                    strCodNivAprob = strCodNivAprob.Trim

                    If strUsuarios(i) = strUsuario_local _
                        And strCodSucursal = strSucu_local _
                        And strCodNivAprob = strNAp_local Then
                        strUsuarios.RemoveAt(i)
                        BubbleEvent = False
                        'Ya existe el usuario "" asociado a la sucursal y nivel de aprobacion seleccionado
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.YaExisteUsuarioXSucursal & oEditUsuario.Value & _
                                                            My.Resources.Resource.YaExisteSucursalXUsuario,
                                                            BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Exit Sub
                    End If
                Next
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

#End Region

#Region "Eventos"

    Public Sub ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent,
                                    ByVal FormUID As String,
                                    ByRef BubbleEvent As Boolean,
                                    ByVal comp As SAPbobsCOM.Company)
        Try
            oForm = m_SBO_Application.Forms.Item(FormUID)
            m_oCompany = comp

            Dim oMatrizMSJ As SAPbouiCOM.Matrix
            Dim oMatrizUser As SAPbouiCOM.Matrix
            Dim str_Usuarios As New List(Of String)
            Dim str_Names As New List(Of String)

            Dim dtConfigLineas As SAPbouiCOM.DataTable

            oMatrizMSJ = DirectCast(FormMSJ.Items.Item("mtx_MSJ").Specific, SAPbouiCOM.Matrix)
            oMatrizUser = DirectCast(oForm.Items.Item("mtx_User").Specific, SAPbouiCOM.Matrix)

            dtConfigLineas = FormMSJ.DataSources.DataTables.Item("dtConfigL")

            'se recorre la matriz de usuarios para obtener los seleccionados
            For i As Integer = 1 To oMatrizUser.RowCount
                If oMatrizUser.IsRowSelected(i) Then
                    str_Usuarios.Add(dtUsuariosMatriz.GetValue("cod", i - 1))
                    str_Names.Add(dtUsuariosMatriz.GetValue("name", i - 1))
                End If
            Next

            '***********          BEFORE SUCCESS          ***********
            If pval.BeforeAction = True _
                And pval.ActionSuccess = False Then

                Select Case pval.ItemUID
                    Case "btnSel"
                        If Agrega Then
                            'validar lista de usuarios
                            Call ValidaUsuarios(pval, str_Usuarios, StrSucursal, StrNivelAprobacion, oMatrizMSJ, BubbleEvent)
                        End If
                End Select

            End If
            '***********          ACTION SUCCESS          ***********
            If pval.ActionSuccess = True _
                And pval.BeforeAction = False Then

                Select Case pval.ItemUID
                    Case "btnSel"
                        If Agrega Then
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.AgregandoUsuariosXSuc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)

                            Call InsertaNivXUsuarios(pval, str_Usuarios, str_Names, StrSucursal, StrNivelAprobacion,
                                                     oMatrizUser, oMatrizMSJ, dtConfigLineas)

                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                        ElseIf Not Agrega Then
                            'Mensajes al borrar usuarios
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.EliminandoUsuariosXSuc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                            'Eliminaod usuarios
                            Call EliminaNivXUsuarios(dtConfigLineas)
                            'proceso finalizado
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                        End If
                        'Cierra el form 
                        oForm.Close()
                    Case "btnBuscar"
                        Dim strCodigoUsuario As String
                        Dim strConsulta As String
                        Dim strOrden As String = " order by USER_CODE "

                        strConsulta = " SELECT USERID , USER_CODE, U_NAME FROM OUSR "

                        strCodigoUsuario = DirectCast(oForm.Items.Item("txtUserC").Specific, SAPbouiCOM.EditText).Value
                        If Not String.IsNullOrEmpty(strCodigoUsuario) Then

                            strConsulta = strConsulta + " WHERE USER_CODE like '" + strCodigoUsuario + "%' "
                            
                        End If

                        strConsulta = strConsulta + strOrden

                        Call CargarMatriz(oMatrizUser, oForm, strConsulta)

                End Select

            End If
        Catch ex As Exception
            'manejo de errores
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try

    End Sub

#End Region

End Class
