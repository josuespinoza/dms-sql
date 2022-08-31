Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager

''' <summary>
''' Módulo encargado de crear instancias del formulario Restablecer Cantidades Pendientes
''' </summary>
''' <remarks>No se debe implementar lógica de negocios en este módulo</remarks>
Module ConstructorRestablecerCantidadesPendientes

    Public Sub CrearInstanciaFormulario()
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.FormType = "SCGD_SRCP"
            strRutaXML = My.Resources.Resource.XMLRestablecerCantidadPendientes
            oFormCreationParams.XmlData = CargarXML(strRutaXML)
            oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            ControladorRestablecerCantidadesPendientes.RedimensionarColumnas(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub AgregarMenu()
        Dim strMenuPadre As String = "SCGD_CFG"
        Dim strTitulo As String = My.Resources.Resource.TituloRestablecerCantidadesPendientes
        Dim strIDMenu As String = "SCGD_SRCP"
        Dim intPosicion As Integer = 20

        Try
            'Verifica que el usuario tenga permisos para accesar al formulario
            'Disponibilidad agenda por empleados
            If DMS_Connector.Helpers.PermisosMenu("SCGD_SRCP") Then
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(strIDMenu, SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, intPosicion, False, True, strMenuPadre))
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el formulario desde un XML
    ''' </summary>
    ''' <param name="p_strFileName">Nombre del archivo. Ejemplo: \Formulario\archivo.xml</param>
    ''' <returns>Texto que contiene el XML del formulario</returns>
    ''' <remarks></remarks>
    Private Function CargarXML(ByVal p_strFileName As String) As String
        Dim oXMLDataDocument As Xml.XmlDataDocument
        Dim strPath As String = String.Empty
        Dim strInnerXML As String = String.Empty

        Try
            'Concatena la ruta de la aplicación con la ruta relativa del formulario y su nombre de archivo
            strPath = System.Windows.Forms.Application.StartupPath & "\" & p_strFileName
            oXMLDataDocument = New Xml.XmlDataDocument
            oXMLDataDocument.Load(strPath)
            strInnerXML = oXMLDataDocument.InnerXml
            Return strInnerXML
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strInnerXML
        End Try
    End Function
End Module
