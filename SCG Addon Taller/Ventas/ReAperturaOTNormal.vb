Imports System.Xml
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Globalization
Imports System.Collections.Generic
Imports SCG.SBOFramework
Imports SAPbobsCOM
Imports SAPbouiCOM


Module ReAperturaOTNormal
#Region "Declaraciones"
    Private n As NumberFormatInfo
    Private m_dbOrdenVenta As DBDataSource
    Private oApplication As SAPbouiCOM.Application
    Private oCompany As SAPbobsCOM.Company
    Private udsForm As UserDataSources
#End Region

#Region "Constructor"
    Sub New()
        Try
            oApplication = DMS_Connector.Company.ApplicationSBO
            oCompany = DMS_Connector.Company.CompanySBO
            n = DIHelper.GetNumberFormatInfo(oCompany)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
#End Region

#Region "Eventos"
    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oForm As SAPbouiCOM.Form
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            If pVal.Before_Action Then
            Else
                Select Case pVal.EventType
                    Case BoEventTypes.et_ITEM_PRESSED
                        oForm = oApplication.Forms.Item(FormUID)
                        Select Case pVal.ItemUID
                            Case "btnGenerar"
                                'Recorro la linea Seleccionada
                                oMatrix = DirectCast(oForm.Items.Item("mtxCoti").Specific, Matrix)
                                RecorreLineaSeleccionada(oMatrix, oForm)

                                'Actualiza listado de documentos a re abrir
                                oMatrix = DirectCast(oForm.Items.Item("mtxCoti").Specific, Matrix)
                                oMatrix.SelectionMode = BoMatrixSelect.ms_Auto

                                If EnlazaColumnasMatrixaDataSource(oMatrix) Then
                                    CargaOrdenesAbiertas(oMatrix, oForm, m_dbOrdenVenta)
                                End If
                            Case "btnAct"
                                'Actualiza listado de documentos a re abrir
                                oMatrix = DirectCast(oForm.Items.Item("mtxCoti").Specific, Matrix)
                                oMatrix.SelectionMode = BoMatrixSelect.ms_Auto

                                If EnlazaColumnasMatrixaDataSource(oMatrix) Then
                                    CargaOrdenesAbiertas(oMatrix, oForm, m_dbOrdenVenta)
                                End If
                            Case "btnCancel"
                                'Cierra formulario("mtxCoti")
                                oForm = oApplication.Forms.Item(FormUID)
                                oForm.Close()
                        End Select
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
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
            oFormCreationParams.FormType = "SCGD_REAOT"

            Path = My.Resources.Resource.XMLReAperturaOT
            oFormCreationParams.XmlData = CargarDesdeXML(Path)

            oForm = DMS_Connector.Company.ApplicationSBO.Forms.AddEx(oFormCreationParams)
            udsForm = oForm.DataSources.UserDataSources
            oForm.DataSources.DBDataSources.Add("ORDR")

            m_dbOrdenVenta = oForm.DataSources.DBDataSources.Item("ORDR")

            oMatrix = DirectCast(oForm.Items.Item("mtxCoti").Specific, Matrix)
            oMatrix.SelectionMode = BoMatrixSelect.ms_Auto

            CargaOfertasAbiertas(oForm, "T0.""DocEntry""=-1")

            If EnlazaColumnasMatrixaDataSource(oMatrix) Then
                CargaOrdenesAbiertas(oMatrix, oForm, m_dbOrdenVenta)
            End If

            oForm.DataSources.DataTables.Add("dtConsulta")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para agregar el menú de Re Apertura de OT's a SAP
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AgregarMenu()
        Dim strTitulo As String = My.Resources.Resource.TituloReAperturaOT

        Try
            If PermisosValidos() Then
                GestorMenu.MenusManager.AddMenuEntry(New MenuEntry("SCGD_REAOT", SAPbouiCOM.BoMenuType.mt_STRING, strTitulo, 18, False, True, "SCGD_GOV"))
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try

    End Sub

    ''' <summary>
    ''' Valida si el usuario tiene permisos para abrir el formulario
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidarPermisos() As Boolean
        Dim oRecordSet As SAPbobsCOM.Recordset

        Try
            oRecordSet = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select * From ""@SCGD_NIVELES_PV"" where ""Code"" = 'SCGD_REAOT' ")

            If oRecordSet.RecordCount > 0 Then
                If PermisosValidos() Then
                    Return True
                End If
            End If
            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Metodo para validar el permiso SCGD_REAOT
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PermisosValidos() As Boolean
        Dim blnPermisoValido As Boolean = False

        Try
            If Utilitarios.MostrarMenu("SCGD_REAOT", DMS_Connector.Company.ApplicationSBO.Company.UserName) Then
                blnPermisoValido = True
            End If
            Return blnPermisoValido
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Método para cargar las formas desde el archivo XML
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarDesdeXML(ByRef strFileName As String) As String
        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml
    End Function

    ''' <summary>
    ''' Metodo para Cargar el grid de Ofertas Abiertas
    ''' </summary>
    ''' <param name="oForm"></param>
    ''' <param name="strOrdenes"></param>
    ''' <remarks></remarks>
    Private Sub CargaOfertasAbiertas(ByRef oForm As SAPbouiCOM.Form, ByVal strOfertas As String)
        Dim strOfertasDeVenta As String = String.Empty
        Dim oGrid As Grid
        Dim oEditTC As EditTextColumn

        Try
            strOfertasDeVenta = String.Format(DMS_Connector.Queries.GetStrSpecificQuery("strGenOFV"), My.Resources.Resource.CapNoCotizacion,
                                    My.Resources.Resource.CapNoOrdenTrabajo, My.Resources.Resource.CapIDCliente,
                                    My.Resources.Resource.CapCliente, My.Resources.Resource.CapPlaca,
                                    My.Resources.Resource.CapMarca, My.Resources.Resource.CapModelo)

            oGrid = oForm.Items.Item("grdOFV").Specific

            If oForm.DataSources.DataTables.Count < 1 Then
                oForm.DataSources.DataTables.Add("OVentas")
            End If

            strOfertasDeVenta &= " WHERE " & strOfertas
            oForm.DataSources.DataTables.Item("OVentas").ExecuteQuery(strOfertasDeVenta)
            oGrid.DataTable = oForm.DataSources.DataTables.Item("OVentas")
            oGrid.Columns.Item(0).Width = 80
            oGrid.Columns.Item(1).Width = 80
            oGrid.Columns.Item(2).Width = 80
            oGrid.Columns.Item(3).Width = 120
            oGrid.Columns.Item(4).Width = 80
            oGrid.Columns.Item(5).Width = 80
            oGrid.Columns.Item(6).Width = 80
            oEditTC = oGrid.Columns.Item(0)
            oEditTC.LinkedObjectType = BoLinkedObject.lf_Quotation
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para enlazar las columnas al DataSource
    ''' </summary>
    ''' <param name="oMatrix"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EnlazaColumnasMatrixaDataSource(ByRef oMatrix As SAPbouiCOM.Matrix) As Boolean
        Dim oColumna As SAPbouiCOM.Column

        Try
            oColumna = oMatrix.Columns.Item("col_NoCot")
            oColumna.DataBind.SetBound(True, "ORDR", "DocEntry")

            oColumna = oMatrix.Columns.Item("col_OT")
            oColumna.DataBind.SetBound(True, "ORDR", "U_SCGD_Numero_OT")

            oColumna = oMatrix.Columns.Item("col_empid")
            oColumna.DataBind.SetBound(True, "ORDR", "CardCode")

            oColumna = oMatrix.Columns.Item("col_emp")
            oColumna.DataBind.SetBound(True, "ORDR", "CardName")

            oColumna = oMatrix.Columns.Item("col_Placa")
            oColumna.DataBind.SetBound(True, "ORDR", "U_SCGD_Num_Placa")

            oColumna = oMatrix.Columns.Item("col_Marca")
            oColumna.DataBind.SetBound(True, "ORDR", "U_SCGD_Des_Marc")

            oColumna = oMatrix.Columns.Item("col_Mod")
            oColumna.DataBind.SetBound(True, "ORDR", "U_SCGD_Des_Mode")

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Metodo para CargarOrdenesAbiertas
    ''' </summary>
    ''' <param name="oMatrix"></param>
    ''' <param name="oForm"></param>
    ''' <param name="dbOrdenVenta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargaOrdenesAbiertas(ByRef oMatrix As Matrix, ByVal oForm As Form, ByVal dbOrdenVenta As DBDataSource) As Boolean
        Dim oCondition As Condition
        Dim oConditions As Conditions

        Try
            oConditions = oApplication.CreateObject(BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.BracketOpenNum = 1
            oCondition.Alias = "U_SCGD_Estado_CotID"
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = "6"
            oCondition.BracketCloseNum = 1
            oCondition.Relationship = BoConditionRelationship.cr_AND

            oCondition = oConditions.Add
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "DocStatus"
            oCondition.Operation = BoConditionOperation.co_EQUAL
            oCondition.CondVal = "O"
            oCondition.BracketCloseNum = 1

            oMatrix.Clear()

            dbOrdenVenta.Clear()
            dbOrdenVenta.Query(oConditions)

            oMatrix.LoadFromDataSource()
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Metodo para Validar la linea seleccionada
    ''' </summary>
    ''' <param name="oMatrix"></param>
    ''' <param name="m_oFormGenCotizacion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RecorreLineaSeleccionada(ByVal oMatrix As Matrix, ByRef m_oFormGenCotizacion As Form) As Boolean
        Dim intFilaMatrix As Integer
        Dim blnOrdenCancelada As Boolean
        Dim strDocEntry As String = ""
        Dim strCondicionOFV As String = ""
        Dim chEliminaOR() As Char = {"O", "R", " "}
        Dim idOrdenVenta As String
        Dim idOT As String

        Try
            If oMatrix.GetNextSelectedRow <> -1 Then
                For intFilaMatrix = 1 To oMatrix.RowCount
                    If oMatrix.IsRowSelected(intFilaMatrix) Then
                        idOrdenVenta = oMatrix.Columns.Item(1).Cells.Item(intFilaMatrix).Specific.value()
                        idOT = oMatrix.Columns.Item(2).Cells.Item(intFilaMatrix).Specific.value()
                        blnOrdenCancelada = ReAbriCotizacion(oMatrix.Columns.Item(1).Cells.Item(intFilaMatrix).Specific.value, DMS_Connector.Company.CompanySBO, strDocEntry)

                        If Not String.IsNullOrEmpty(strDocEntry) AndAlso strDocEntry <> "-2" Then
                            strCondicionOFV &= "T0.DocEntry=" & strDocEntry & " OR "
                        End If
                    End If
                Next
                strCondicionOFV = strCondicionOFV.TrimEnd(chEliminaOR)
                If Not String.IsNullOrEmpty(strCondicionOFV) Then
                    CargaOfertasAbiertas(m_oFormGenCotizacion, strCondicionOFV)
                    oApplication.StatusBar.SetText(My.Resources.Resource.ProcesoFinalizadoConExito, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Success)
                End If
            End If
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Metodo para Cancelar la Orden de Venta
    ''' </summary>
    ''' <param name="NoOrdenVenta"></param>
    ''' <param name="oCompany"></param>
    ''' <param name="strDocEntry"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReAbriCotizacion(ByVal NoOrdenVenta As Integer, ByRef oCompany As SAPbobsCOM.Company, ByRef strDocEntry As String) As Boolean
        Dim oOrdenDeVentas As SAPbobsCOM.Documents
        Dim intDocEntry As Integer
        Dim strNoOT As String
        Dim oCotizacion, oCotizacionNueva As SAPbobsCOM.Documents
        Dim oCotizacionEncabezadoList As CotizacionEncabezado_List
        Dim oCotizacionList As Cotizacion_List
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceOT As SAPbobsCOM.GeneralService
        Dim oGeneralDataOT As SAPbobsCOM.GeneralData

        Try
            oApplication.StatusBar.SetText(My.Resources.Resource.InicioReAperturaOT, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            oOrdenDeVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            'Asigno la información de la Orden de Venta seleccionada
            If oOrdenDeVentas.GetByKey(NoOrdenVenta) Then

                'Documento de Origen
                intDocEntry = oOrdenDeVentas.Lines.BaseEntry
                strNoOT = oOrdenDeVentas.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                oCotizacion = oCompany.GetBusinessObject(BoObjectTypes.oQuotations)

                'Asigno la información de la Cotización
                If oCotizacion.GetByKey(intDocEntry) Then
                    oCotizacionEncabezadoList = New CotizacionEncabezado_List()
                    oCotizacionList = New Cotizacion_List()

                    CargarCotizacionDataContract(oCotizacion, intDocEntry, oCotizacionEncabezadoList, oCotizacionList)

                    'Cargo la Cotización original al objeto
                    If CargarCotizacionObjeto(oCotizacionNueva, oCotizacionEncabezadoList, oCotizacionList) Then
                        LimpiarDocumento(oOrdenDeVentas)
                        LimpiarDocumento(oCotizacion)
                    Else
                        oApplication.StatusBar.SetText(My.Resources.Resource.ErrorReAperturaOtCargaObjetos, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If

                    oCompanyService = oCompany.GetCompanyService()
                    oGeneralServiceOT = oCompanyService.GetGeneralService("SCGD_OT")

                    'Limpia Datos, Crea Cotización y Actualiza OT
                    If GuardarDatosDB(oCotizacion, oCotizacionNueva, oOrdenDeVentas, oGeneralServiceOT, oGeneralDataOT, strNoOT, strDocEntry) Then
                        oApplication.StatusBar.SetText(My.Resources.Resource.FinReAperturaOT, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                    Else
                        oApplication.StatusBar.SetText(My.Resources.Resource.ErrorReAperturaOT, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    End If
                Else
                    oApplication.StatusBar.SetText(My.Resources.Resource.ErrorReAperturaOTSinOFV, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Metodo para DuplicarCotizacion
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CargarCotizacionObjeto(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, ByRef p_oCotizacionList As Cotizacion_List) As Boolean

        Try
            p_oCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            'Encabezado de la Cotizacion
            With p_oCotizacionEncabezadoList.Item(0)
                If Not String.IsNullOrEmpty(.NoOrden) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .NoOrden
                End If
                If Not String.IsNullOrEmpty(.Sucursal) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = .Sucursal
                End If
                If Not String.IsNullOrEmpty(.GeneraOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value = .GeneraOT
                End If
                If Not String.IsNullOrEmpty(.EstadoCotizacionID) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = "2"
                End If
                If .FechaCreacionOT <> Nothing Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value = .FechaCreacionOT
                End If
                If .HoraCreacionOT <> Nothing Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value = .HoraCreacionOT
                End If
                If Not String.IsNullOrEmpty(.GeneraRecepcion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value = .GeneraRecepcion
                End If
                If Not String.IsNullOrEmpty(.OTPadre) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value = .OTPadre
                End If
                If Not String.IsNullOrEmpty(.NoOTReferencia) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value = .NoOTReferencia
                End If
                If Not String.IsNullOrEmpty(.NumeroVIN) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = .NumeroVIN
                End If
                If Not String.IsNullOrEmpty(.CodigoUnidad) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = .CodigoUnidad
                End If
                If Not String.IsNullOrEmpty(.CodigoAsesor) Then
                    p_oCotizacion.DocumentsOwner = .CodigoAsesor
                End If
                If Not String.IsNullOrEmpty(.TipoOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = .TipoOT
                End If
                If Not String.IsNullOrEmpty(.CodigoProyecto) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value = .CodigoProyecto
                End If

                p_oCotizacion.CardCode = .CardCode
                p_oCotizacion.CardName = .CardName
                p_oCotizacion.DocCurrency = .DocCurrency
                p_oCotizacion.Series = .Serie
                p_oCotizacion.Comments = .Comments
                p_oCotizacion.SalesPersonCode = .SlpCode
                p_oCotizacion.DiscountPercent = .DiscountPercent

                If Not String.IsNullOrEmpty(.NoVisita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value = .NoVisita
                End If
                If Not String.IsNullOrEmpty(.NoSerieCita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = .NoSerieCita
                End If
                If Not String.IsNullOrEmpty(.NoCita) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = .NoCita
                End If
                If Not String.IsNullOrEmpty(.Cono) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value = .Cono
                End If
                If Not String.IsNullOrEmpty(.Year) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = .Year
                End If
                If Not String.IsNullOrEmpty(.DescripcionMarca) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = .DescripcionMarca
                End If
                If Not String.IsNullOrEmpty(.DescripcionModelo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = .DescripcionModelo
                End If
                If Not String.IsNullOrEmpty(.DescripcionEstilo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = .DescripcionEstilo
                End If
                If Not String.IsNullOrEmpty(.CodigoMarca) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = .CodigoMarca
                End If
                If Not String.IsNullOrEmpty(.CodigoEstilo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = .CodigoEstilo
                End If
                If Not String.IsNullOrEmpty(.CodigoModelo) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = .CodigoModelo
                End If
                If Not String.IsNullOrEmpty(.Kilometraje) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = .Kilometraje
                End If
                If Not String.IsNullOrEmpty(.Placa) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = .Placa
                End If
                If Not String.IsNullOrEmpty(.NombreClienteOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value = .NombreClienteOT
                End If
                If Not String.IsNullOrEmpty(.CodigoClienteOT) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value = .CodigoClienteOT
                End If
                If Not String.IsNullOrEmpty(.FechaRecepcion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value = .FechaRecepcion
                End If
                If Not String.IsNullOrEmpty(.HoraRecepcion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value = .HoraRecepcion
                End If
                If Not String.IsNullOrEmpty(.NivelGasolina) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value = .NivelGasolina
                End If
                If Not String.IsNullOrEmpty(.Observaciones) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value = .Observaciones
                End If
                If Not String.IsNullOrEmpty(.EstadoCotizacion) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = My.Resources.Resource.EstadoOrdenEnproceso
                End If
            End With

            p_oCotizacionEncabezadoList.Remove(p_oCotizacionEncabezadoList.Item(0))

            For rowCotizacion As Integer = 0 To p_oCotizacionList.Count - 1

                With p_oCotizacionList.Item(rowCotizacion)

                    p_oCotizacion.Lines.ItemCode = .ItemCode
                    p_oCotizacion.Lines.ItemDescription = .Description
                    p_oCotizacion.Lines.Quantity = .Quantity
                    p_oCotizacion.Lines.UnitPrice = .Price
                    p_oCotizacion.Lines.TaxCode = .TaxCode
                    p_oCotizacion.Lines.VatGroup = .VatGroup
                    p_oCotizacion.Lines.FreeText = .FreeText
                    p_oCotizacion.Lines.Currency = .Currency
                    p_oCotizacion.Lines.DiscountPercent = .LineDscPrcnt

                    If Not String.IsNullOrEmpty(.IdRepxOrd) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value = .IdRepxOrd
                    End If
                    If Not String.IsNullOrEmpty(.ID) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value = .ID
                    End If
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value = .Aprobado
                    p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value = .Trasladado
                    If Not String.IsNullOrEmpty(.OTHija) Then
                        If .OTHija <> 0 Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = .OTHija
                        Else
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value = 2
                        End If
                    End If
                    If Not String.IsNullOrEmpty(.DuracionEstandar) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = .DuracionEstandar
                    End If
                    If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                    End If
                    If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                    End If
                    If Not String.IsNullOrEmpty(.EstadoActividad) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                    End If
                    If Not String.IsNullOrEmpty(.CantidadRecibida) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value = .CantidadRecibida
                    End If
                    If Not String.IsNullOrEmpty(.CantidadSolicitada) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value = .CantidadSolicitada
                    End If
                    If Not String.IsNullOrEmpty(.CantidadPendiente) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value = .CantidadPendiente
                    End If
                    If Not String.IsNullOrEmpty(.CantidadPendienteBodega) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value = .CantidadPendienteBodega
                    End If
                    If Not String.IsNullOrEmpty(.CantidadPendienteTraslado) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value = .CantidadPendienteTraslado
                    End If
                    If Not String.IsNullOrEmpty(.CantidadPendienteDevolucion) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value = .CantidadPendienteDevolucion
                    End If
                    If Not String.IsNullOrEmpty(.Costo) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .Costo
                    End If
                    If Not String.IsNullOrEmpty(.NoOrden) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = .NoOrden
                    End If
                    If Not String.IsNullOrEmpty(.Entregado) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value = .Entregado
                    End If
                    If Not String.IsNullOrEmpty(.TipoArticulo) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = CStr(.TipoArticulo)
                    End If
                    If Not String.IsNullOrEmpty(.Comprar) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value = .Comprar
                    End If
                    If Not String.IsNullOrEmpty(.Sucursal) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value = .Sucursal
                    End If
                    If Not String.IsNullOrEmpty(.CentroCosto) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value = .CentroCosto
                    End If
                    If Not String.IsNullOrEmpty(.TipoOT) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value = .TipoOT
                    End If
                    If Not String.IsNullOrEmpty(.Procesar) Then
                        If .Procesar <> 0 Then
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = .ProcesarInteger
                        Else
                            p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value = 1
                        End If
                    End If
                    If Not String.IsNullOrEmpty(.EstadoActividad) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = .EstadoActividad
                    End If
                    If Not String.IsNullOrEmpty(.EmpleadoAsignado) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = .EmpleadoAsignado
                    End If
                    If Not String.IsNullOrEmpty(.NombreEmpleado) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = .NombreEmpleado
                    End If
                    If Not String.IsNullOrEmpty(.CostoEstandar) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .CostoEstandar
                    End If
                End With
                p_oCotizacion.Lines.Add()
            Next
            p_oCotizacionList.Clear()
            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Metodo para Desligar la información de OT a la Orden de Venta y Cotización según el momento
    ''' </summary>
    ''' <param name="p_oDocument"></param>
    ''' <remarks></remarks>
    Private Sub LimpiarDocumento(ByRef p_oDocument As SAPbobsCOM.Documents)

        Try
            With p_oDocument
                .UserFields.Fields.Item("U_SCGD_NoOtRef").Value = .UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                .UserFields.Fields.Item("U_SCGD_Numero_OT").Value = ""
                .UserFields.Fields.Item("U_SCGD_OT_Padre").Value = ""
                .UserFields.Fields.Item("U_SCGD_No_Visita").Value = ""
                .UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = ""
                .UserFields.Fields.Item("U_SCGD_NoCita").Value = ""
                .UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = ""
                .UserFields.Fields.Item("U_SCGD_Num_VIN").Value = ""
                .UserFields.Fields.Item("U_SCGD_Num_Placa").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = ""
                .UserFields.Fields.Item("U_SCGD_Fech_Recep").Value = ""
                .UserFields.Fields.Item("U_SCGD_idSucursal").Value = ""
                .UserFields.Fields.Item("U_SCGD_Des_Marc").Value = ""
                .UserFields.Fields.Item("U_SCGD_Des_Mode").Value = ""
                .UserFields.Fields.Item("U_SCGD_Des_Esti").Value = ""
                .UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = ""
                .UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = ""
                .UserFields.Fields.Item("U_SCGD_CCliOT").Value = ""
                .UserFields.Fields.Item("U_SCGD_NCliOT").Value = ""
                .UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = ""
                For index As Integer = 0 To .Lines.Count - 1
                    .Lines.SetCurrentLine(index)
                    .Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = ""
                Next
            End With

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para cargar la información a DataContract
    ''' </summary>
    ''' <param name="p_oCotizacion"></param>
    ''' <param name="p_intDocEntry"></param>
    ''' <param name="p_oCotizacionEncabezadoList"></param>
    ''' <param name="p_oCotizacionList"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Private Sub CargarCotizacionDataContract(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByVal p_intDocEntry As Integer, ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, ByRef p_oCotizacionList As Cotizacion_List)
        Dim oCotizacionEncabezado As CotizacionEncabezado
        Dim oCotizacion As Cotizacion

        Try
            If p_intDocEntry > 0 Then
                p_oCotizacion = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                If p_oCotizacion.GetByKey(p_intDocEntry) Then
                    'Carga Encabezado de la Cotizacion
                    oCotizacionEncabezado = New CotizacionEncabezado()
                    With oCotizacionEncabezado
                        .DocEntry = p_oCotizacion.DocEntry
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                            .NoOrden = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value) Then
                            .Sucursal = p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value.ToString()) Then
                            .GeneraOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Genera_OT").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value) Then
                            .EstadoCotizacionID = "4"
                        End If
                        If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value <> Nothing Then
                            .FechaCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_CreaOT").Value
                        End If
                        If p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value <> Nothing Then
                            .HoraCreacionOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_CreaOT").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value) Then
                            .GeneraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_GeneraOR").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value) Then
                            .OTPadre = p_oCotizacion.UserFields.Fields.Item("U_SCGD_OT_Padre").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value) Then
                            .NoOTReferencia = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoOtRef").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value) Then
                            .NumeroVIN = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                            .CodigoUnidad = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.DocumentsOwner.ToString()) Then
                            .CodigoAsesor = p_oCotizacion.DocumentsOwner
                        Else
                            .CodigoAsesor = 0
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value.ToString()) Then
                            .TipoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value
                        Else
                            .TipoOT = 0
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value) Then
                            .CodigoProyecto = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value
                        End If

                        .CardCode = p_oCotizacion.CardCode
                        .CardName = p_oCotizacion.CardName
                        .DocCurrency = p_oCotizacion.DocCurrency
                        .Serie = p_oCotizacion.Series
                        .Comments = p_oCotizacion.Comments
                        .SlpCode = p_oCotizacion.SalesPersonCode
                        .DiscountPercent = p_oCotizacion.DiscountPercent

                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value) Then
                            .NoVisita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_No_Visita").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value) Then
                            .NoSerieCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value) Then
                            .NoCita = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value) Then
                            .Cono = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gorro_Veh").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()) Then
                            .Year = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()) Then
                            .DescripcionMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()) Then
                            .DescripcionModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()) Then
                            .DescripcionEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()) Then
                            .CodigoMarca = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()) Then
                            .CodigoEstilo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()) Then
                            .CodigoModelo = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value.ToString.Trim()) Then
                            .Kilometraje = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString.Trim()) Then
                            .Placa = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value.ToString().Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString.Trim()) Then
                            .NombreClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value.ToString().Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString.Trim()) Then
                            .CodigoClienteOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value.ToString().Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value.ToString.Trim()) Then
                            .FechaRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value.ToString.Trim()) Then
                            .HoraRecepcion = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Hora_Recep").Value.ToString()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value.ToString.Trim()) Then
                            .NivelGasolina = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Gasolina").Value
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value) Then
                            .Observaciones = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Observ").Value.ToString.Trim()
                        End If
                        If Not String.IsNullOrEmpty(p_oCotizacion.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value) Then
                            .EstadoCotizacion = My.Resources.Resource.EstadoOTFinalizada
                        End If
                    End With

                    p_oCotizacionEncabezadoList.Add(oCotizacionEncabezado)

                    For rowCotizacion As Integer = 0 To p_oCotizacion.Lines.Count - 1
                        p_oCotizacion.Lines.SetCurrentLine(rowCotizacion)

                        If p_oCotizacion.Lines.TreeType = BoItemTreeTypes.iIngredient OrElse p_oCotizacion.Lines.TreeType = BoItemTreeTypes.iNotATree Then

                            oCotizacion = New Cotizacion()
                            With oCotizacion

                                .ItemCode = p_oCotizacion.Lines.ItemCode
                                .Description = p_oCotizacion.Lines.ItemDescription
                                .Quantity = p_oCotizacion.Lines.Quantity
                                .TreeType = p_oCotizacion.Lines.TreeType
                                .Price = p_oCotizacion.Lines.Price
                                .TaxCode = p_oCotizacion.Lines.TaxCode
                                .VatGroup = p_oCotizacion.Lines.VatGroup
                                .FreeText = p_oCotizacion.Lines.FreeText
                                .Currency = p_oCotizacion.Lines.Currency

                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                                    .IdRepxOrd = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                    .ID = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                End If
                                .Aprobado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                .Trasladado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value) Then
                                    .OTHija = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Then
                                    .DuracionEstandar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                Else
                                    .DuracionEstandar = 0
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()) Then
                                    .NombreEmpleado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                    .EstadoActividad = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value.ToString.Trim()) Then
                                    .CantidadRecibida = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString.Trim()) Then
                                    .CantidadSolicitada = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString.Trim()) Then
                                    .CantidadPendiente = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value.ToString.Trim()) Then
                                    .CantidadPendienteBodega = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value.ToString.Trim()) Then
                                    .CantidadPendienteTraslado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value.ToString.Trim()) Then
                                    .CantidadPendienteDevolucion = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()) Then
                                    .Costo = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()) Then
                                    .NoOrden = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value.ToString.Trim()) Then
                                    .Entregado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()) Then
                                    .TipoArticulo = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value.ToString.Trim()) Then
                                    .Comprar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim()) Then
                                    .Sucursal = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()) Then
                                    .CentroCosto = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()) Then
                                    .TipoOT = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value.ToString.Trim()) Then
                                    .ProcesarInteger = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                    .EstadoActividad = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                    .EmpleadoAsignado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                End If
                                'If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()) Then
                                '    .NombreEmpleado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()
                                'End If
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()) Then
                                    .CostoEstandar = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()
                                End If
                            End With
                            p_oCotizacionList.Add(oCotizacion)
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para Guardar los datos de Cotizacion, Orden de Ventas y OT
    ''' </summary>
    ''' <param name="p_oCotizacion"></param>
    ''' <param name="p_oCotizacionNueva"></param>
    ''' <param name="p_oPedido"></param>
    ''' <param name="p_oGeneralServiceOT"></param>
    ''' <param name="p_oGeneralDataOT"></param>
    ''' <param name="pVal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GuardarDatosDB(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oCotizacionNueva As SAPbobsCOM.Documents, ByRef p_oPedido As SAPbobsCOM.Documents, ByRef p_oGeneralServiceOT As SAPbobsCOM.GeneralService, ByRef p_oGeneralDataOT As SAPbobsCOM.GeneralData, ByRef strNoOT As String, ByRef strDocEntry As String) As Boolean
        Dim listDocEntry As List(Of Integer)
        Dim strError As String
        Dim intError As Integer
        Try
            listDocEntry = New List(Of Integer)()
            'Inicio de Transaction
            If Not oCompany.InTransaction() Then
                oCompany.StartTransaction()
                'Actualiza Cotización
                If p_oCotizacion.Update() = 0 Then
                    'Crea Nueva Cotización
                    If p_oCotizacionNueva.Add() = 0 Then
                        oCompany.GetNewObjectCode(strDocEntry)
                        'Actualiza Orden de Venta
                        ActualizarOT(p_oGeneralServiceOT, p_oGeneralDataOT, strNoOT, oCompany.GetNewObjectKey)
                        If Not p_oPedido Is Nothing Then
                            If p_oPedido.Update() = 0 Then
                                If p_oPedido.Cancel() = 0 Then
                                    'Actualiza la OT
                                    If Not p_oGeneralServiceOT Is Nothing Then
                                        p_oGeneralServiceOT.Update(p_oGeneralDataOT)
                                        If oCompany.InTransaction() Then
                                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                                            Return True
                                        End If
                                    Else
                                        oCompany.GetLastError(intError, strError)
                                        If oCompany.InTransaction() Then
                                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                        End If
                                        oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarOT + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                        Return False
                                    End If
                                Else
                                    oCompany.GetLastError(intError, strError)
                                    If oCompany.InTransaction() Then
                                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                    End If
                                    oApplication.StatusBar.SetText(My.Resources.Resource.ErrorCancelarOrdenVenta + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                    Return False
                                End If
                            Else
                                oCompany.GetLastError(intError, strError)
                                If oCompany.InTransaction() Then
                                    oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                End If
                                oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarOrdenVenta + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                                Return False
                            End If
                        Else
                            If oCompany.InTransaction() Then
                                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                                oCompany.GetLastError(intError, strError)
                            End If
                            Return False
                        End If
                    Else
                        oCompany.GetLastError(intError, strError)
                        If oCompany.InTransaction() Then
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                        End If
                        oApplication.StatusBar.SetText(My.Resources.Resource.ErrorReAperturaCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                Else
                    oCompany.GetLastError(intError, strError)
                    If oCompany.InTransaction() Then
                        oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                    oApplication.StatusBar.SetText(My.Resources.Resource.ErrorActualizarCotizacion + " " + strError.ToString(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If
        Catch ex As Exception
            If oCompany.InTransaction() Then
                oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Metodo para establecer en proceso la OT en trato
    ''' </summary>
    ''' <param name="p_oGeneralServiceOT"></param>
    ''' <param name="p_oGeneralDataOT"></param>
    ''' <param name="p_oForm"></param>
    ''' <param name="p_strDocEntry"></param>
    ''' <param name="p_strSucursal"></param>
    ''' <remarks></remarks>
    Private Sub ActualizarOT(ByRef p_oGeneralServiceOT As GeneralService, ByRef p_oGeneralDataOT As GeneralData, ByRef strNoOT As String, ByVal p_strDocEntry As String)
        Dim oGeneralParams As GeneralDataParams

        Try
            If Not String.IsNullOrEmpty(strNoOT) Then
                oGeneralParams = p_oGeneralServiceOT.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", strNoOT)
                p_oGeneralDataOT = p_oGeneralServiceOT.GetByParams(oGeneralParams)
                p_oGeneralDataOT.SetProperty("U_EstO", "2")
                p_oGeneralDataOT.SetProperty("U_DEstO", My.Resources.Resource.EstadoOrdenEnproceso)
                p_oGeneralDataOT.SetProperty("U_DocEntry", p_strDocEntry)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            Utilitarios.DestruirObjeto(oGeneralParams)
        End Try
    End Sub

#End Region
End Module
