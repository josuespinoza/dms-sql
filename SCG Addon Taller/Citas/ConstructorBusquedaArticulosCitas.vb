Imports SAPbouiCOM

Module ConstructorBusquedaArticulosCitas

    ''' <summary>
    ''' Crea una nueva instancia del formulario búsqueda de artículos
    ''' </summary>
    ''' <param name="FormUIDPadre">Unique ID del formulario padre, obligatorio</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="CodigoCliente">Código del cliente</param>
    ''' <param name="CodigoInternoVehiculo">Código interno del vehículo</param>
    ''' <remarks>El FormUIDPadre es obligatorio ya que se utiliza para saber desde cual formulario fue abierto el buscador
    ''' una vez que se seleccionan los artículos es posible agregarlos al formulario padre mediante ese ID</remarks>
    Public Sub CrearInstanciaFormulario(ByVal FormUIDPadre As String, ByVal CodigoSucursal As String, ByVal CodigoCliente As String, ByVal CodigoInternoVehiculo As String)
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.FormType = "SCGD_ISSC"
            strRutaXML = My.Resources.Resource.XMLBuscadorArticulosCitas
            oFormCreationParams.XmlData = CargarXML(strRutaXML)
            oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            ControladorBusquedaArticulosCitas.CargarValoresPredeterminados(oFormulario, FormUIDPadre, CodigoSucursal, CodigoCliente, CodigoInternoVehiculo)
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
