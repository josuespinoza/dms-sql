Imports SAPbouiCOM
Imports SAPbobsCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Globalization
Imports SCG.SBOFramework
Imports System.Xml
Imports System.IO
Imports System.Collections.Generic
Imports SCG.Cifrado
Imports SCG.Integration.InterfaceDPM
Imports SCG.Integration.InterfaceDPM.Entities
Imports System.Reflection
Module InterfaceJohnDeereConfiguration
    Private WithEvents oApplication As SAPbouiCOM.Application
    Private oCompany As SAPbobsCOM.Company
    Private oFormulario As SAPbouiCOM.Form
    Private n As NumberFormatInfo

    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Try
            oApplication = DMS_Connector.Company.ApplicationSBO
            oCompany = DMS_Connector.Company.CompanySBO
            n = DIHelper.GetNumberFormatInfo(oCompany)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

#Region "Eventos"
    'Public Sub SBO_Application_ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Handles oApplication.ItemEvent
    '    Dim oForm As SAPbouiCOM.Form

    '    Try
    '        If pVal.Before_Action Then
    '        Else
    '            Select Case pVal.EventType
    '                Case BoEventTypes.et_ITEM_PRESSED
    '                    oForm = oApplication.Forms.Item(FormUID)
    '                    Select Case pVal.ItemUID
    '                        Case "btnCargar"

    '                    End Select
    '            End Select
    '        End If
    '    Catch ex As Exception
    '        DMS_Connector.Helpers.ManejoErrores(ex)
    '    End Try
    'End Sub
#End Region
#Region "Metodos"
    ''' <summary>
    ''' Metodo para Abrir el Formulario de Re Apertura de OT's
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub AbrirFormulario()
        Dim oFormCreationParams As FormCreationParams
        Dim Path As String = String.Empty
        Dim oForm As SAPbouiCOM.Form
        Dim oMatrix As Matrix

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.BorderStyle = BoFormBorderStyle.fbs_Sizable
            oFormCreationParams.FormType = "SCGD_CJD"

            Path = My.Resources.Resource.XMLConfigurationJohnDeere
            oFormCreationParams.XmlData = CargarDesdeXML(Path)

            oForm = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    ''' <summary>
    ''' Método para cargar las formas desde el archivo XML
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        Dim oXMLDoc As XmlDocument
        Dim strPath As String

        strPath = Windows.Forms.Application.StartupPath & strFileName
        oXMLDoc = New XmlDocument()

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml
    End Function
    ''' <summary>
    ''' Metodo para agregar el menú de Tareas de Implementación a SAP
    ''' </summary>
    ''' <param name="pIndependiente"> True = Menu dentro del estándar de SAP - False = Menu dentro de las Configuraciones de DMS</param>
    ''' <remarks></remarks>
    Public Sub AgregarMenu()
        Dim strTitulo As String = "Interface John Deere (DPM) Configuration"
        Dim strIDMenu As String = "SCGD_CJD"
        Dim intPosicion As Integer = 19
        Dim oMenuItem As SAPbouiCOM.MenuItem
        Dim oMenus As SAPbouiCOM.Menus
        Dim oCreationPackage As SAPbouiCOM.MenuCreationParams

        Try

            If PermisosValidos() Then
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(strIDMenu, SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, intPosicion, False, True, "SCGD_IND"))
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para Validar el Permiso SCGD_OTDI
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PermisosValidos() As Boolean
        Dim blnPermisoValido As Boolean = False
        Try
            If Utilitarios.MostrarMenu("SCGD_CJD", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                blnPermisoValido = True
            End If
            Return blnPermisoValido
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    
#End Region
End Module
