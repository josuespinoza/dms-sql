Imports SAPbouiCOM
Imports SCG.DMSOne.Framework.MenuManager

Module ConstructorCitas

    ''' <summary>
    ''' Crea una nueva instancia del formulario en blanco y en modo crear con los valores predeterminados
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CrearInstanciaFormulario()
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.FormType = "SCGD_CCIT"
            strRutaXML = My.Resources.Resource.XMLCitas
            oFormCreationParams.XmlData = CargarXML(strRutaXML)
            oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            'Llamar al método que realiza inicializaciones generales del formulario
            ControladorCitas.CargarValoresPredeterminados(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Crea una nueva instancia del formulario en modo crear, pero con ciertos campos completos a partir de los datos del calendario
    ''' </summary>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <param name="FechaSeleccionada">Fecha seleccionada</param>
    ''' <remarks></remarks>
    Public Sub CrearInstanciaFormulario(ByVal Sucursal As String, ByVal CodigoAgenda As String, ByVal FechaSeleccionada As Date)
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.FormType = "SCGD_CCIT"
            strRutaXML = My.Resources.Resource.XMLCitas
            oFormCreationParams.XmlData = CargarXML(strRutaXML)
            oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            'Llamar al método que realiza inicializaciones generales del formulario
            ControladorCitas.CargarValoresPredeterminados(oFormulario)
            ControladorCitas.AsignarValoresPorParametro(oFormulario, Sucursal, CodigoAgenda, FechaSeleccionada)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Crea una nueva instancia del formulario en modo crear, pero con ciertos campos completos a partir de los datos del calendario
    ''' </summary>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <param name="FechaSeleccionada">Fecha seleccionada</param>
    ''' <remarks></remarks>
    Public Sub CrearInstanciaFormulario(ByVal Sucursal As String, ByVal CodigoAgenda As String, ByVal CodigoAsesor As String, ByVal FechaCita As Date, ByVal CodigoTecnico As String, ByVal FechaServicio As Date)
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form

        Try
            oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            oFormCreationParams.FormType = "SCGD_CCIT"
            strRutaXML = My.Resources.Resource.XMLCitas
            oFormCreationParams.XmlData = CargarXML(strRutaXML)
            oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            'Llamar al método que realiza inicializaciones generales del formulario
            ControladorCitas.CargarValoresPredeterminados(oFormulario)
            ControladorCitas.AsignarValoresPorParametro(oFormulario, Sucursal, CodigoAgenda, CodigoAsesor, FechaCita, CodigoTecnico, FechaServicio)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CrearInstanciaFormularioExistente(ByVal DocEntryCita As String)
        Dim NumeroSerie As String = String.Empty
        Dim Consecutivo As String = String.Empty
        Dim Query As String = "SELECT T0.""U_Num_Serie"", T0.""U_NumCita"" FROM ""@SCGD_CITA"" T0 WITH (nolock) WHERE T0.""DocEntry"" = '{0}' "
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            If Not String.IsNullOrEmpty(DocEntryCita) Then
                Query = String.Format(Query, DocEntryCita)
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                If oRecordset.RecordCount > 0 Then
                    NumeroSerie = oRecordset.Fields.Item("U_Num_Serie").Value.ToString()
                    Consecutivo = oRecordset.Fields.Item("U_NumCita").Value.ToString()
                End If

                If Not String.IsNullOrEmpty(NumeroSerie) AndAlso Not String.IsNullOrEmpty(Consecutivo) Then
                    CrearInstanciaFormularioExistente(NumeroSerie, Consecutivo)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga una instancia del formulario de citas y carga los datos de la cita indicada (Cita ya existente)
    ''' </summary>
    ''' <param name="NumeroSerie">Número de serie u abreviatura</param>
    ''' <param name="Consecutivo">Consecutivo (Número de cita)</param>
    ''' <remarks></remarks>
    Public Sub CrearInstanciaFormularioExistente(ByVal NumeroSerie As String, ByVal Consecutivo As String)
        Dim oFormCreationParams As FormCreationParams
        Dim strXMLFormulario As String = String.Empty
        Dim strRutaXML As String = String.Empty
        Dim oFormulario As SAPbouiCOM.Form
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Dim DocEntry As String = String.Empty
        Dim Query As String = "SELECT T0.""DocEntry"" FROM ""@SCGD_CITA"" T0 WITH (nolock) WHERE T0.""U_Num_Serie"" = '{0}' AND T0.""U_NumCita"" = '{1}'"
        Try
            If Not String.IsNullOrEmpty(NumeroSerie) AndAlso Not String.IsNullOrEmpty(Consecutivo) Then
                Query = String.Format(Query, NumeroSerie, Consecutivo)
                DocEntry = DMS_Connector.Helpers.EjecutarConsulta(Query)
            End If

            If Not String.IsNullOrEmpty(DocEntry) Then
                oFormCreationParams = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
                oFormCreationParams.FormType = "SCGD_CCIT"
                strRutaXML = My.Resources.Resource.XMLCitas
                oFormCreationParams.XmlData = CargarXML(strRutaXML)
                oFormulario = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
                oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.Alias = "DocEntry"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = DocEntry
                oFormulario.Items.Item("tabCtrl").Click()
                'Utiliza oConditions para cargar la información del formulario
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").Query(oConditions)
                'Llamar al método que realiza inicializaciones generales del formulario
                ControladorCitas.CargarDatosDesdeAgenda(oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el menú para el formulario al menú de citas
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AgregarMenu()
        Dim strMenuPadre As String = "SCGD_CITS"
        Dim strTitulo As String = My.Resources.Resource.TituloCitas + " " + "Ver 2.0"
        Dim strIDMenu As String = "SCGD_CCIT"
        Dim intPosicion As Integer = 10
        Dim VersionModuloCitas As String = String.Empty

        Try
            VersionModuloCitas = DMS_Connector.Configuracion.ParamGenAddon.U_ScheduleType
            If Not String.IsNullOrEmpty(VersionModuloCitas) AndAlso VersionModuloCitas.Equals("2") Then
                'Verifica que el usuario tenga permisos para accesar al formulario
                'Disponibilidad agenda por empleados
                If DMS_Connector.Helpers.PermisosMenu("SCGD_CIT") Then
                    GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(strIDMenu, SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, intPosicion, False, True, strMenuPadre))
                End If
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
