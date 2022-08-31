Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager

Public Module ConstructorReporteBodegaReservas

    ''' <summary>
    ''' Agrega el menú para el formulario al menú de citas
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AgregarMenu()
        Dim strMenuPadre As String = "SCGD_IND"
        Dim strTitulo As String = My.Resources.Resource.TituloAuditoriaReserva
        Dim strIDMenu As String = "SCGD_RABR"
        Dim intPosicion As Integer = 11
        Try
            'Verifica que el usuario tenga permisos para accesar al formulario
            'Disponibilidad agenda por empleados
            If DMS_Connector.Helpers.PermisosMenu("SCGD_RABR") Then
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(strIDMenu, SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, intPosicion, False, True, strMenuPadre))
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Crea una nueva instancia del formulario Reporte Auditoría Bodega Reservas
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CrearInstanciaFormulario()
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.FormType = "SCGD_RABR"
            strRutaXML = My.Resources.Resource.XMLAuditoriaBodegaReserva
            oFormCreationParams.XmlData = CargarXML(strRutaXML)
            oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            ControladorReporteBodegaReservas.CargarValoresPredeterminados(oFormulario)
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
