Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SCG.DMSOne.Framework.MenuManager

Public Class ConfiguracionPropiedadesVehiculosCls

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strIdMainMenu As String = "43520"

    Private Const mc_strUIDContratoVenta As String = "SCGD_UIDListCont"
    Private Const mc_strUIDCV_Listado As String = "UIDOCVTra"

    Private Const mc_strSCG_CONFPROPIEDADES As String = "@SCGD_CONFPROPIEDADE"
    Private Const mc_strSCG_PROP_VALORES As String = "@SCGD_PROP_VALORES"

    Private Const mc_strCode As String = "Code"
    Private Const mc_strName As String = "Name"
    Private Const mc_strAcctCod As String = "U_Acc_Cod"
    Private Const mc_strAcc_Nam As String = "U_Acc_Nam"
    Private Const mc_strLineId As String = "LineId"
    Private Const mc_strValor As String = "U_Valor"

    'Nombres de los campos de texto
    Private Const mc_strUIDCodigo As String = "3"
    Private Const mc_strUIDNombre As String = "5"
    Private Const mc_strUIDmtx_0 As String = "mtx_0"
    Private Const mc_strUIDAgregar As String = "add"
    Private Const mc_strUIDBorrar As String = "del"

    Private m_strLineasEliminadas As String
    Private m_strCodigoPropiedad As String

    Private m_oFormGenCotizacion As SAPbouiCOM.Form

    Private Const mc_intErrorOperationNoSupported As Integer = -5006

    Private SBO_Application As SAPbouiCOM.Application

#End Region

#Region "Constructor"

    <System.CLSCompliant(False)> _
    Public Sub New(ByRef p_SBO_Aplication As SAPbouiCOM.Application, ByRef p_oCompania As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Aplication
        m_oCompany = p_oCompania

    End Sub

#End Region

#Region "Metodos"

    Protected Friend Sub AddMenuItems()

        Dim strEtiquetaMenu As String
        
        If Utilitarios.MostrarMenu("SCGD_PRC", SBO_Application.Company.UserName) Then

            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_PRC", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_PRC", SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 30, False, True, "SCGD_CFG"))
        End If

    End Sub

    Protected Friend Sub CargaFormulario()
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
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable
            fcp.FormType = "SCGD_PROP"

            strXMLACargar = My.Resources.Resource.PROPForm
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, True)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
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

        strPath = Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If

        Return oXMLDoc.InnerXml

    End Function

    Public Sub HabilitarCampos(ByVal p_strFormID As String, ByVal blnModoNuevo As Boolean)

        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.Item(p_strFormID)

        If blnModoNuevo Then
            oform.Items.Item(mc_strUIDNombre).Enabled = True
            oform.Items.Item(mc_strUIDCodigo).Enabled = True
        Else

            ' oform = SBO_Application.Forms.Item(p_strFormID)

            oform.Items.Item(mc_strUIDNombre).Enabled = True
            oform.Items.Item(mc_strUIDCodigo).Enabled = False

        End If


    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressedBCV(ByVal FormUID As String, _
                                               ByRef pVal As SAPbouiCOM.ItemEvent, _
                                               ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If Not oForm Is Nothing Then
                If pVal.ActionSuccess Then
                    Select Case pVal.ItemUID
                        Case mc_strUIDBorrar
                            Call EliminarUsuarios(FormUID)
                        Case mc_strUIDAgregar
                            Call AsignarUsuarios(FormUID)
                    End Select
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorEventoItemPressedGenOV" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub AsignarUsuarios(ByVal p_strFormID As String)
        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intNuevoRegisto As Integer
        Dim blnLineasAgregadas As Boolean = False
        Dim strUsuario As String

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)

        oMatriz.FlushToDataSource()

        intNuevoRegisto = oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).Size
        strUsuario = oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).GetValue(mc_strValor, intNuevoRegisto - 1)
        If Not String.IsNullOrEmpty(strUsuario) Then

            oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).InsertRecord(intNuevoRegisto)
            intNuevoRegisto += 1
        Else
            intNuevoRegisto = 1
        End If

        blnLineasAgregadas = True

        If blnLineasAgregadas Then
            oMatriz.LoadFromDataSource()

        End If
        If oMatriz.RowCount = 0 Then
            oMatriz.AddRow()
            oMatriz.FlushToDataSource()
        End If
    End Sub

    Private Sub EliminarUsuarios(ByVal p_strFormID As String)

        Dim oform As SAPbouiCOM.Form
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim intRegistoEliminar As Integer
        Dim blnLineasEliminadas As Boolean = False

        oform = SBO_Application.Forms.Item(p_strFormID)
        oMatriz = DirectCast(oform.Items.Item("mtx_0").Specific, SAPbouiCOM.Matrix)
        intRegistoEliminar = oMatriz.GetNextSelectedRow()
        Do While intRegistoEliminar > -1

            If String.IsNullOrEmpty(m_strLineasEliminadas) Then
                m_strLineasEliminadas = oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).GetValue(mc_strLineId, intRegistoEliminar - 1)
            Else
                m_strLineasEliminadas &= oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).GetValue(mc_strLineId, intRegistoEliminar - 1)
            End If
            m_strCodigoPropiedad = oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).GetValue(mc_strCode, intRegistoEliminar - 1)
            oform.DataSources.DBDataSources.Item(mc_strSCG_PROP_VALORES).RemoveRecord(intRegistoEliminar - 1)

            blnLineasEliminadas = True
            intRegistoEliminar = oMatriz.GetNextSelectedRow(intRegistoEliminar)

        Loop
        If blnLineasEliminadas Then
            oMatriz.LoadFromDataSource()
            oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
        End If

    End Sub

    Public Sub EliminarUsuariosBD()

        Dim a_strLineasEliminadas() As String
        Dim strlineaEliminada As String
        Dim strConsulta As String
        Dim blnPrimeraLinea As Boolean = True

        If Not String.IsNullOrEmpty(m_strLineasEliminadas) Then
            a_strLineasEliminadas = m_strLineasEliminadas.Split(",")
            strConsulta = "Delete from [" & mc_strSCG_PROP_VALORES & "] where Code = '" & m_strCodigoPropiedad.Trim() & "' and (LineId = "
            For Each strlineaEliminada In a_strLineasEliminadas
                If blnPrimeraLinea Then
                    strConsulta &= strlineaEliminada
                Else
                    strConsulta &= " or LineId = " & strlineaEliminada
                    blnPrimeraLinea = False
                End If
            Next
            strConsulta &= ")"
            Utilitarios.EjecutarConsulta(strConsulta, m_oCompany.CompanyDB, m_oCompany.Server)
        End If
        m_strLineasEliminadas = ""
        m_strCodigoPropiedad = ""

    End Sub

    Public Sub LimpiarLineasAEliminar()
        m_strLineasEliminadas = ""
        m_strCodigoPropiedad = ""
    End Sub

#End Region


End Class

