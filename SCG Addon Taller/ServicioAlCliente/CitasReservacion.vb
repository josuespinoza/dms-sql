Imports DMS_Addon.ControlesSBO
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports SAPbobsCOM
Imports SAPbouiCOM

Partial Class CitasReservacion
    Implements IUsaPermisos

    Private m_strNumCotizacion As String

    Private m_strMonedaLocal As String
    Private m_strModelaSistema As String

    Private m_strMonedaOrigen As String
    Private m_strMonedaDestino As String
    Private m_strNumeroGrupo As String = String.Empty
    Public g_objAdcionalesArt As BuscadorArticulosCitas
    Public g_objGestorFormularios As GestorFormularios

#Region "Declaraciones"

    Public dtListaServicios As SAPbouiCOM.DataTable
    Public dtListaServiciosElimin As SAPbouiCOM.DataTable

    Public MatrizServicios As MatrizServicios
    Dim oCotizacion As SAPbobsCOM.Documents

    Public dtUnidad As System.Data.DataTable
    Public strConsultaUnidad As String = ""

#End Region

#Region "Metodos"


    Public Function ObternerFechaServer() As DateTime
        Try
            Dim l_fhaActual As DateTime

            l_fhaActual = Utilitarios.EjecutarConsulta("select GETDATE()", m_oCompany.CompanyDB, m_oCompany.Server)

            Return l_fhaActual
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Sub ActualizaCotizacion(ByRef bubbleEvent As Boolean)
        Try
            Dim l_strNumCotizacion As String
            Dim l_strDocEntry As String
            Dim oCotizacion As SAPbobsCOM.Documents
            Dim l_strComentario As String = String.Empty
            Dim strPoseeCampana As String = String.Empty
            Dim strGarantiaVigente As String = String.Empty
            Dim strIngresoPorGrua As String = String.Empty

            l_strNumCotizacion = EditTextCotizacion.ObtieneValorDataSource()

            oCotizacion = CType(m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations), SAPbobsCOM.Documents)
            l_strDocEntry = oCotizacion.GetByKey(l_strNumCotizacion)

            l_strComentario = EditTextObservaciones.ObtieneValorDataSource()

            If l_strComentario.Length > 254 Then
                l_strComentario = l_strComentario.Substring(0, 254)
            End If

            oCotizacion.Comments = l_strComentario

            strPoseeCampana = _formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Campana", 0).Trim

            If Not String.IsNullOrEmpty(strPoseeCampana) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Campana").Value = strPoseeCampana
            End If

            strGarantiaVigente = _formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Garantia", 0).Trim

            If Not String.IsNullOrEmpty(strGarantiaVigente) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Garantia").Value = strGarantiaVigente
            End If

            strIngresoPorGrua = _formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Towing", 0).Trim

            If Not String.IsNullOrEmpty(strIngresoPorGrua) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Towing").Value = strIngresoPorGrua
            End If

            If Not String.IsNullOrEmpty(EditCboAsesor.ObtieneValorDataSource()) Then
                oCotizacion.DocumentsOwner = EditCboAsesor.ObtieneValorDataSource()
            End If

            MatrizServicios.Matrix.FlushToDataSource()

            If EditCboEstado.ObtieneValorDataSource() <> m_strCodCitasCancel Then

                ActualizaLineasCotizacion(oCotizacion)

            ElseIf EditCboEstado.ObtieneValorDataSource() = m_strCodCitasCancel Then
                oCotizacion.Cancel()
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try
    End Sub

    Public Sub ActualizaLineasCotizacion(ByRef p_oCotizacion As SAPbobsCOM.Documents)
        Dim strCode As String
        Dim intLineNum As Integer
        Dim strEsHijo As String
        Dim strNomEmpleado As String
        Dim l_oLineasCot As SAPbobsCOM.Document_Lines
        Dim strNoOT As String = String.Empty

        l_oLineasCot = p_oCotizacion.Lines

        Try
            Dim l_blnExiteItem As Boolean = False
            Dim l_blnEliminaItem As Boolean = False
            Dim lisEliminar As New List(Of Integer)

            MatrizServicios.Matrix.FlushToDataSource()
            strNoOT = p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
            If EditCboEstado.ObtieneValorDataSource() <> m_strCodCitasCancel Then
                For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                    strNomEmpleado = ObtenerNombreEmpleado(EditCboTecnico.ObtieneValorDataSource)

                    If Not String.IsNullOrEmpty(dtListaServicios.GetValue("codigo", i)) Then

                        strCode = dtListaServicios.GetValue("codigo", i)
                        strEsHijo = dtListaServicios.GetValue("hijo", i)

                        If strEsHijo = String.Empty Then
                            strEsHijo = "N"
                        End If

                        l_blnExiteItem = False

                        If String.IsNullOrEmpty(dtListaServicios.GetValue("linea", i)) Then
                            intLineNum = 0
                        Else
                            intLineNum = dtListaServicios.GetValue("linea", i)
                        End If

                        For m As Integer = 0 To l_oLineasCot.Count - 1
                            l_oLineasCot.SetCurrentLine(m)

                            If strCode = l_oLineasCot.ItemCode AndAlso intLineNum = l_oLineasCot.LineNum AndAlso strEsHijo = "Y" Then
                                If dtListaServicios.GetValue("hijo", i) = "Y" Then
                                    'Actualiza la descripción de las líneas hijas de los paquetes
                                    l_oLineasCot.ItemDescription = dtListaServicios.GetValue("descripcion", i)
                                End If
                            End If

                            If strCode = l_oLineasCot.ItemCode AndAlso
                                intLineNum = l_oLineasCot.LineNum AndAlso
                                strEsHijo = "N" Then

                                l_oLineasCot.ItemDescription = dtListaServicios.GetValue("descripcion", i)

                                'Solamente se puede actualizar los precios y cantidades si no se ha creado la OT
                                If String.IsNullOrEmpty(strNoOT) Then
                                    l_oLineasCot.Quantity = dtListaServicios.GetValue("cantidad", i)
                                    l_oLineasCot.Currency = dtListaServicios.GetValue("moneda", i)
                                    l_oLineasCot.UnitPrice = dtListaServicios.GetValue("precio", i)
                                End If
                                l_blnExiteItem = True
                                Exit For
                            End If
                        Next

                        If l_blnExiteItem = False Then
                            If dtListaServicios.GetValue("hijo", i) = "N" Then
                                l_oLineasCot.Add()

                                l_oLineasCot.ItemCode = dtListaServicios.GetValue("codigo", i)
                                l_oLineasCot.ItemDescription = dtListaServicios.GetValue("descripcion", i)
                                l_oLineasCot.Quantity = dtListaServicios.GetValue("cantidad", i)
                                l_oLineasCot.Currency = dtListaServicios.GetValue("moneda", i)
                                l_oLineasCot.UnitPrice = dtListaServicios.GetValue("precio", i)

                            End If
                        End If

                        If dtListaServicios.GetValue("tipo", i).ToString() = "2" Then
                            l_oLineasCot.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = EditCboTecnico.ObtieneValorDataSource()
                            l_oLineasCot.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = strNomEmpleado
                        End If

                    End If

                    

                Next

                For i As Integer = 0 To l_oLineasCot.Count - 1
                    l_oLineasCot.SetCurrentLine(i)

                    Dim itmCot As String
                    Dim LinCot As String
                    Dim TreeCot As String
                    l_blnEliminaItem = True

                    itmCot = l_oLineasCot.ItemCode
                    LinCot = l_oLineasCot.LineNum
                    TreeCot = l_oLineasCot.TreeType

                    For j As Integer = 0 To dtListaServicios.Rows.Count - 1

                        If itmCot = dtListaServicios.GetValue("codigo", j) Then

                            l_blnEliminaItem = False
                            Exit For
                        End If
                    Next

                    If l_blnEliminaItem Then
                        If TreeCot <> "5" Then
                            lisEliminar.Add(i)
                        End If
                    End If
                Next

                For Each loNum As Integer In lisEliminar
                    l_oLineasCot.SetCurrentLine(loNum)
                    l_oLineasCot.Delete()
                Next

                p_oCotizacion.Update()

            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CambiarModoActualizar()
        Try
            If _formularioSbo IsNot Nothing AndAlso _formularioSbo.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSbo.Mode = BoFormMode.fm_UPDATE_MODE
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CrearAvaluo(ByRef BubbleEvent As Boolean, ByVal p_strNumCot As String, ByVal p_strDocNum As String)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceAva As SAPbobsCOM.GeneralService
        Dim oGeneralDataAva As SAPbobsCOM.GeneralData
        Dim query As String
        Dim strVehiCode As String
        Try
            query = "Select ISNULL(U_GenAva, 'N') as U_GenAva from [@SCGD_AGENDA] where DocEntry = '{0}'"
            query = String.Format(query, EditCboAgenda.ObtieneValorDataSource())
            If Utilitarios.ValidaExisteDataTable(_formularioSbo, "dtConsulta") Then
                dtConsulta = _formularioSbo.DataSources.DataTables.Item("dtConsulta")
            Else
                dtConsulta = _formularioSbo.DataSources.DataTables.Add("dtConsulta")
            End If

            dtConsulta.ExecuteQuery(query)
            If dtConsulta.Rows.Count > 0 Then
                If dtConsulta.GetValue(0, 0).ToString().Trim() = "Y" Then
                    strVehiCode = EditTextIdVehiculo.ObtieneValorDataSource()
                    query = "Select U_Cod_Unid, isnull (U_Cod_Marc, '') U_Cod_Marc, isnull (U_Cod_Esti, '') U_Cod_Esti, isnull (U_Cod_Mode, '') U_Cod_Mode, isnull (U_Num_Plac, '') U_Num_Plac, isnull (U_Ano_Vehi, '') U_Ano_Vehi, isnull (U_Num_VIN, '') U_Num_VIN, isnull (U_Combusti, '') U_Combusti, isnull (U_Cod_Col, '') U_Cod_Col, isnull (U_Transmis, '') U_Transmis, isnull (U_Km_Unid, '') U_Km_Unid from [@SCGD_VEHICULO] with(nolock) where Code='{0}' "
                    query = String.Format(query, strVehiCode)
                    dtConsulta.ExecuteQuery(query)
                    If dtConsulta.Rows.Count > 0 AndAlso Not dtConsulta.GetValue(0, 0) Is Nothing Then

                        oCompanyService = CompanySBO.GetCompanyService()
                        oGeneralServiceAva = oCompanyService.GetGeneralService("SCGD_AVA")
                        oGeneralDataAva = DirectCast(oGeneralServiceAva.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData), GeneralData)
                        oGeneralDataAva.SetProperty("U_IdSucu", EditCboSucursal.ObtieneValorDataSource())
                        oGeneralDataAva.SetProperty("U_PropCed", EditTextCardCode.ObtieneValorDataSource())
                        oGeneralDataAva.SetProperty("U_PropNom", EditTextCardName.ObtieneValorDataSource())
                        oGeneralDataAva.SetProperty("U_VehCod", strVehiCode)
                        oGeneralDataAva.SetProperty("U_CodUnid", dtConsulta.GetValue("U_Cod_Unid", 0).ToString().Trim())
                        oGeneralDataAva.SetProperty("U_CodMarc", dtConsulta.GetValue("U_Cod_Marc", 0))
                        oGeneralDataAva.SetProperty("U_CodMode", dtConsulta.GetValue("U_Cod_Mode", 0))
                        oGeneralDataAva.SetProperty("U_CodEsti", dtConsulta.GetValue("U_Cod_Esti", 0))
                        oGeneralDataAva.SetProperty("U_Placa", dtConsulta.GetValue("U_Num_Plac", 0))
                        oGeneralDataAva.SetProperty("U_VIN", dtConsulta.GetValue("U_Num_VIN", 0))
                        oGeneralDataAva.SetProperty("U_Ano", dtConsulta.GetValue("U_Ano_Vehi", 0))
                        oGeneralDataAva.SetProperty("U_Combusti", dtConsulta.GetValue("U_Combusti", 0))

                        If Not String.IsNullOrEmpty(EditCboTecnico.ObtieneValorDataSource()) AndAlso EditCboTecnico.ObtieneValorDataSource() <> 0 Then
                            oGeneralDataAva.SetProperty("U_TecCode", EditCboTecnico.ObtieneValorDataSource())
                        End If

                        oGeneralDataAva.SetProperty("U_CodCol", dtConsulta.GetValue("U_Cod_Col", 0))
                        oGeneralDataAva.SetProperty("U_Km_Ing", dtConsulta.GetValue("U_Km_Unid", 0))
                        oGeneralDataAva.SetProperty("U_Transmis", dtConsulta.GetValue("U_Transmis", 0))
                        oGeneralDataAva.SetProperty("U_Estado", "1")
                        oGeneralDataAva.SetProperty("U_Moneda", EditCboMoneda.ObtieneValorDataSource())
                        oGeneralDataAva.SetProperty("U_CotID", p_strNumCot)
                        oGeneralDataAva.SetProperty("U_CotDocN", p_strDocNum)

                        oGeneralServiceAva.Add(oGeneralDataAva)
                    End If

                End If
            End If
        Catch ex As Exception
            BubbleEvent = False
            Throw
        End Try
    End Sub

    Public Function CreaCotizacion(ByRef bubbleEvent As Boolean) As String
        Dim key As String = String.Empty
        Dim l_strSerie As String
        Dim l_strConsecutivo As String
        Dim codigoError As Integer
        Dim l_strCodSucursal As String
        Dim l_strSerieCotizacion As String
        Dim l_strComentario As String = String.Empty
        Dim l_strMoneda As String = String.Empty
        Dim strFechaCita As String
        Dim strHoraCita As String
        Dim strGenAva As String
        Dim strSalesPerson As String = String.Empty
        Dim strDocumentsOwner As String = String.Empty
        Dim strQuerySalesPerson As String = "SELECT TOP 1 ""salesPrson"" FROM OHEM WITH(nolock) WHERE ""empID"" = '{0}'"
        Dim strGarantiaVigente As String = String.Empty
        Dim strPoseeCampana As String = String.Empty
        Dim strIngresoPorGrua As String = String.Empty
        Dim Kilometraje As String = String.Empty

        Try

            l_strSerie = GeneraSerieCita()
            If Not String.IsNullOrEmpty(l_strSerie) Then
                l_strConsecutivo = GeneraConsecutivoCita(l_strSerie)
            End If

            If Utilitarios.ValidaExisteDataTable(_formularioSbo, "dtConsulta") Then
                dtConsulta = _formularioSbo.DataSources.DataTables.Item("dtConsulta")
            Else
                dtConsulta = _formularioSbo.DataSources.DataTables.Add("dtConsulta")
            End If

            l_strCodSucursal = EditCboSucursal.ObtieneValorDataSource()
            l_strSerieCotizacion = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(l_strCodSucursal)).U_SerOfV.Trim
            l_strComentario = EditTextObservaciones.ObtieneValorDataSource()
            l_strMoneda = EditCboMoneda.ObtieneValorDataSource()

            If l_strComentario.Length > 254 Then
                l_strComentario = l_strComentario.Substring(0, 254)
            End If

            oCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            oCotizacion.CardCode = EditTextCardCode.ObtieneValorDataSource()
            oCotizacion.CardName = EditTextCardName.ObtieneValorDataSource()
            oCotizacion.Comments = l_strComentario
            oCotizacion.DocCurrency = l_strMoneda

            If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                If Not String.IsNullOrEmpty(l_strCodSucursal) Then
                    oCotizacion.BPL_IDAssignedToInvoice = Integer.Parse(l_strCodSucursal)
                End If
            End If
            If Not String.IsNullOrEmpty(EditCboAsesor.ObtieneValorDataSource) Then
                oCotizacion.DocumentsOwner = EditCboAsesor.ObtieneValorDataSource
                strDocumentsOwner = EditCboAsesor.ObtieneValorDataSource

                If Not String.IsNullOrEmpty(strDocumentsOwner) Then
                    strQuerySalesPerson = String.Format(strQuerySalesPerson, strDocumentsOwner)
                    strSalesPerson = DMS_Connector.Helpers.EjecutarConsulta(strQuerySalesPerson)
                    If Not String.IsNullOrEmpty(strSalesPerson) Then
                        oCotizacion.SalesPersonCode = strSalesPerson
                    End If
                End If
            End If

            strConsultaUnidad = "select Code, U_Cod_Marc, U_Des_Marc, U_Cod_Mode, U_Des_Mode, U_Cod_Esti, U_Des_Esti, U_Num_Plac, U_Num_VIN, U_Ano_Vehi, U_Km_Unid from [@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '{0}'"

            dtUnidad = Utilitarios.EjecutarConsultaDataTable(String.Format(strConsultaUnidad,
                                                                           EditTextUnidad.ObtieneValorDataSource()),
                                                                           m_oCompany.CompanyDB,
                                                                           m_oCompany.Server)

            oCotizacion.Series = l_strSerieCotizacion
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = EditTextUnidad.ObtieneValorDataSource()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = dtUnidad.Rows(0)("Code").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = dtUnidad.Rows(0)("U_Ano_Vehi").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = dtUnidad.Rows(0)("U_Num_Plac").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = dtUnidad.Rows(0)("U_Cod_Marc").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = dtUnidad.Rows(0)("U_Cod_Mode").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = dtUnidad.Rows(0)("U_Cod_Esti").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = dtUnidad.Rows(0)("U_Des_Marc").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = dtUnidad.Rows(0)("U_Des_Mode").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = dtUnidad.Rows(0)("U_Des_Esti").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = dtUnidad.Rows(0)("U_Num_VIN").ToString()
            oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value = EditTextCardCodeCliOT.ObtieneValorDataSource()
            oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value = EditTextCardNameCliOT.ObtieneValorDataSource()
            oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = l_strCodSucursal
            oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = l_strSerie
            oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = l_strConsecutivo
            Kilometraje = dtUnidad.Rows(0)("U_Km_Unid").ToString()
            If String.IsNullOrEmpty(Kilometraje) Then
                Kilometraje = "0"
            End If
            oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = Kilometraje

            strPoseeCampana = _formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Campana", 0).Trim

            If Not String.IsNullOrEmpty(strPoseeCampana) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Campana").Value = strPoseeCampana
            End If

            strGarantiaVigente = _formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Garantia", 0).Trim

            If Not String.IsNullOrEmpty(strGarantiaVigente) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Garantia").Value = strGarantiaVigente
            End If

            strIngresoPorGrua = _formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Towing", 0).Trim

            If Not String.IsNullOrEmpty(strIngresoPorGrua) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Towing").Value = strIngresoPorGrua
            End If

            strFechaCita = EditTextFecha.ObtieneValorDataSource()
            strHoraCita = EditTextHora.ObtieneValorDataSource()
            If strHoraCita.Length = 3 Then strHoraCita = "0" & strHoraCita
            Dim dtFecha As Date = New Date(CInt(strFechaCita.Substring(0, 4)), CInt(strFechaCita.Substring(4, 2)), CInt(strFechaCita.Substring(6, 2)), CInt(strHoraCita.Substring(0, 2)), CInt(strHoraCita.Substring(2, 2)), 0)
            oCotizacion.UserFields.Fields.Item("U_SCGD_FechCita").Value = dtFecha
            oCotizacion.UserFields.Fields.Item("U_SCGD_HoraCita").Value = dtFecha
            MatrizServicios.Matrix.FlushToDataSource()

            strGenAva = Utilitarios.EjecutarConsulta(String.Format(" Select ISNULL(U_GenAva, 'N') as U_GenAva from [@SCGD_AGENDA] where DocEntry = '{0}' ", EditCboAgenda.ObtieneValorDataSource())).Trim()
            If strGenAva = "Y" Then
                dtConsulta.ExecuteQuery("Select AutoKey from ONNM WITH (NOLOCK) where ObjectCode = 'SCGD_AVA'")
                If dtConsulta.Rows.Count > 0 Then
                    oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value = dtConsulta.GetValue(0, 0).ToString().Trim()
                End If
            End If

            If EditCbxArticulos.ObtieneValorDataSource() = "Y" Then
                AsignaValoresMatrizSinArtic(bubbleEvent)
            End If

            CreaLineasCotizacion(oCotizacion)

            If DMS_Connector.Company.CompanySBO.InTransaction Then
                DMS_Connector.Company.CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            DMS_Connector.Company.CompanySBO.StartTransaction()
            codigoError = oCotizacion.Add()
            If codigoError <> 0 Then
                If codigoError = -5002 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & codigoError & ": " & m_oCompany.GetLastErrorDescription() & "for the Quotation", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Else
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCode & codigoError & ": " & m_oCompany.GetLastErrorDescription(), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                End If
                bubbleEvent = False
                Return String.Empty
            Else
                EditTextNumCita.AsignaValorDataSource(l_strConsecutivo)
                EditTextNumSerie.AsignaValorDataSource(l_strSerie)
                m_oCompany.GetNewObjectCode(key)
                If Not String.IsNullOrEmpty(key) Then
                    m_strNumCotizacion = key
                    ActualizarLineasCita(CInt(m_strNumCotizacion))
                    EditTextCotizacion.AsignaValorDataSource(m_strNumCotizacion)
                    oCotizacion.GetByKey(key)
                    CrearAvaluo(bubbleEvent, m_strNumCotizacion, oCotizacion.DocNum)
                    ActualizarDescripcionPaquetes(oCotizacion)
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                    End If
                End If

            End If

            If String.IsNullOrEmpty(key) Then
                Return String.Empty
            Else
                Return l_strSerie & "-" & l_strConsecutivo
            End If

        Catch ex As Exception
            bubbleEvent = False
            If DMS_Connector.Company.CompanySBO.InTransaction Then
                DMS_Connector.Company.CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Actualiza las descripciones de las líneas hijas de los paquetes para que coincidan con lo que se digitó en la cita
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ActualizarDescripcionPaquetes(ByRef p_oCotizacion As SAPbobsCOM.Documents)
        Dim strItemCode As String = String.Empty
        Dim intLineNum As Integer
        Dim strEsHijo As String = String.Empty
        Dim blnActualizar As Boolean = False

        Try
            For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                intLineNum = i
                strItemCode = dtListaServicios.GetValue("codigo", i)

                If String.IsNullOrEmpty(dtListaServicios.GetValue("hijo", i)) Then
                    strEsHijo = "N"
                Else
                    strEsHijo = dtListaServicios.GetValue("hijo", i)
                End If

                If strEsHijo = "Y" Then
                    For j As Integer = 0 To p_oCotizacion.Lines.Count - 1
                        p_oCotizacion.Lines.SetCurrentLine(j)
                        If strItemCode = p_oCotizacion.Lines.ItemCode AndAlso intLineNum = p_oCotizacion.Lines.LineNum Then
                            'Actualiza la descripción de las líneas hijas de los paquetes
                            p_oCotizacion.Lines.ItemDescription = dtListaServicios.GetValue("descripcion", i)
                            blnActualizar = True
                            Exit For
                        End If
                    Next
                End If
            Next

            If blnActualizar Then
                p_oCotizacion.Update()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub


    ''' <summary>
    ''' Función que asigna los valores necesarios a las líneas hijas para consulta de agendas
    ''' </summary>
    ''' <param name="p_intDocEntry">DocEntry de la cotización</param>
    ''' <remarks></remarks>
    Private Sub ActualizarLineasCita(ByVal p_intDocEntry As Integer)
        Dim oQuotations As Documents
        Dim oItem As SAPbobsCOM.Items
        Dim strIdEmpleado As String
        Dim strSucursal As String
        Dim strImpuesto As String
        Try
            strIdEmpleado = EditCboTecnico.ObtieneValorDataSource().Trim()
            strSucursal = EditCboSucursal.ObtieneValorDataSource().Trim()
            oQuotations = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations)
            oItem = DMS_Connector.Company.CompanySBO.GetBusinessObject(BoObjectTypes.oItems)
            If oQuotations.GetByKey(p_intDocEntry) Then
                With oQuotations.Lines
                    For index As Integer = 0 To .Count - 1
                        .SetCurrentLine(index)
                        If oItem.GetByKey(.ItemCode) Then
                            If String.IsNullOrEmpty(CStr(.UserFields.Fields.Item("U_SCGD_EmpAsig").Value)) AndAlso CStr(oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value).Trim().Equals("2") Then
                                .UserFields.Fields.Item("U_SCGD_EmpAsig").Value = strIdEmpleado
                                .UserFields.Fields.Item("U_SCGD_DurSt").Value = oItem.UserFields.Fields.Item("U_SCGD_Duracion").Value
                            End If
                            .UserFields.Fields.Item("U_SCGD_TipArt").Value = oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value
                            strImpuesto = String.Empty
                            If .TreeType <> BoItemTreeTypes.iIngredient OrElse DMS_Connector.Company.AdminInfo.DisplayPriceforPriceOnly = BoYesNoEnum.tNO Then
                                If DMS_Connector.Configuracion.ParamGenAddon.U_LocCR = "Y" Then
                                    If Not String.IsNullOrEmpty(oQuotations.CardCode) And Not String.IsNullOrEmpty(.ItemCode) Then
                                        strImpuesto = DMS_Connector.Business_Logic.ImpuestoBL.ObtenerImpuesto(_formularioSbo, oQuotations.CardCode, .ItemCode)
                                    End If
                                End If
                                If String.IsNullOrEmpty(strImpuesto) Then
                                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(strSucursal)) Then
                                        With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(strSucursal))
                                            Select Case CStr(oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value).Trim()
                                                Case "1"
                                                    strImpuesto = .U_Imp_Repuestos.Trim()
                                                Case "2"
                                                    strImpuesto = .U_Imp_Serv.Trim()
                                                Case "3"
                                                    strImpuesto = .U_Imp_Suminis.Trim()
                                                Case "4"
                                                    strImpuesto = .U_Imp_ServExt.Trim()
                                                Case "11", "12"
                                                    strImpuesto = .U_Imp_Gastos.Trim()
                                            End Select
                                        End With
                                    End If
                                End If
                            End If
                            If Not String.IsNullOrEmpty(strImpuesto) Then
                                .TaxCode = strImpuesto
                                .VatGroup = strImpuesto
                            End If
                        End If
                    Next
                End With
                If Not 0 = oQuotations.Update() Then
                    Throw New Exception(String.Format("{0}: {1}", DMS_Connector.Company.CompanySBO.GetLastErrorCode(), DMS_Connector.Company.CompanySBO.GetLastErrorDescription()))
                End If
            Else
                Throw New Exception(String.Format("{0}: {1}", DMS_Connector.Company.CompanySBO.GetLastErrorCode(), DMS_Connector.Company.CompanySBO.GetLastErrorDescription()))
            End If
        Catch ex As Exception
            Throw
        Finally
            Utilitarios.DestruirObjeto(oQuotations)
            Utilitarios.DestruirObjeto(oItem)
        End Try
    End Sub

    Private Function GeneraSerieCita() As String
        Try
            Dim l_fhaSolicitada As Date
            Dim l_strFecha As String
            Dim l_strMes As String
            Dim l_strAño As String
            Dim l_strTipoAgenda As String
            Dim l_strNumSerie As String
            Dim l_numTipoAgenda As String
            Dim l_strSQLAgenda As String

            'Tipo de agenda
            l_numTipoAgenda = EditCboAgenda.ObtieneValorDataSource()

            If String.IsNullOrEmpty(l_numTipoAgenda) Then
                l_numTipoAgenda = 0
            End If

            l_strSQLAgenda = "SELECT U_Abreviatura FROM [@SCGD_AGENDA] with (nolock) WHERE DocEntry = {0}"
            l_strSQLAgenda = String.Format(l_strSQLAgenda, l_numTipoAgenda)
            l_strTipoAgenda = Utilitarios.EjecutarConsulta(l_strSQLAgenda, m_oCompany.CompanyDB, m_oCompany.Server)

            'Numero de serie
            l_strFecha = EditTextFecha.ObtieneValorDataSource()
            l_fhaSolicitada = Date.ParseExact(l_strFecha, "yyyyMMdd", Nothing)
            l_strMes = String.Format("{0:MM}", l_fhaSolicitada)
            l_strAño = String.Format("{0:yy}", l_fhaSolicitada)

            l_strNumSerie = l_strTipoAgenda & l_strAño & l_strMes

            Return l_strNumSerie

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try
    End Function

    Public Function GeneraConsecutivoCita(ByVal p_strSerie As String) As String
        Try
            Dim l_strSQLConsecutivo As String
            Dim l_strConsecutivo As String
            Dim l_numTipoAgenda As Integer

            'Tipo de agenda
            l_numTipoAgenda = EditCboAgenda.ObtieneValorDataSource()

            If String.IsNullOrEmpty(l_numTipoAgenda) Then
                l_numTipoAgenda = 0
            End If

            'Numero de Cita
            l_strSQLConsecutivo = "SELECT TOP 1 U_NumCita FROM [@SCGD_CITA] with (nolock)  WHERE  U_Num_Serie = '{0}' order by DocNum DESC"
            l_strSQLConsecutivo = String.Format(l_strSQLConsecutivo, p_strSerie)

            l_strConsecutivo = Utilitarios.EjecutarConsulta(l_strSQLConsecutivo, m_oCompany.CompanyDB, m_oCompany.Server)
            If Not String.IsNullOrEmpty(l_strConsecutivo) Then
                l_strConsecutivo = l_strConsecutivo + 1
            Else
                l_strConsecutivo = 1
            End If

            Select Case l_strConsecutivo.Length
                Case 1
                    l_strConsecutivo = "000" & l_strConsecutivo
                Case 2
                    l_strConsecutivo = "00" & l_strConsecutivo
                Case 3
                    l_strConsecutivo = "0" & l_strConsecutivo
            End Select

            Return l_strConsecutivo
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try
    End Function

    Public Sub CreaLineasCotizacion(ByRef oCotizacion As Documents)
        Try
            Dim l_StrTipoItem As String
            Dim bolAgrega As Boolean = False
            Dim strNombreEmp As String

            strNombreEmp = ObtenerNombreEmpleado(EditCboTecnico.ObtieneValorDataSource)

            For i As Integer = 0 To dtListaServicios.Rows.Count - 1

                If (dtListaServicios.GetValue("hijo", i).ToString.Trim = "N" AndAlso
                    dtListaServicios.GetValue("codigo", i).Equals(dtListaServicios.GetValue("padre", i))) OrElse
                    (dtListaServicios.GetValue("hijo", i).Equals("N") AndAlso
                     String.IsNullOrEmpty(dtListaServicios.GetValue("padre", i)).ToString.Trim) Then

                    If bolAgrega Then
                        oCotizacion.Lines.Add()
                    Else
                        bolAgrega = True
                    End If
                    If Not String.IsNullOrEmpty(dtListaServicios.GetValue("codigo", i).ToString.Trim) Then

                        oCotizacion.Lines.ItemCode = dtListaServicios.GetValue("codigo", i).ToString.Trim
                        oCotizacion.Lines.ItemDescription = dtListaServicios.GetValue("descripcion", i).ToString.Trim
                        oCotizacion.Lines.Quantity = dtListaServicios.GetValue("cantidad", i).ToString.Trim
                        oCotizacion.Lines.Currency = dtListaServicios.GetValue("moneda", i).ToString.Trim
                        oCotizacion.Lines.UnitPrice = dtListaServicios.GetValue("precio", i).ToString.Trim

                        l_StrTipoItem = dtListaServicios.GetValue("tipo", i).ToString.Trim

                        If l_StrTipoItem = "2" Then
                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = EditCboTecnico.ObtieneValorDataSource()
                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = strNombreEmp.Trim
                            oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = dtListaServicios.GetValue("duracion", i).ToString.Trim
                        End If

                        If String.IsNullOrEmpty(dtListaServicios.GetValue("impuesto", i)) Then

                            Select Case l_StrTipoItem
                                Case 1, 5 'Repuestos - Paquetes
                                    If Not String.IsNullOrEmpty(m_strImpRepuesto) Then
                                        oCotizacion.Lines.TaxCode = m_strImpRepuesto
                                        oCotizacion.Lines.VatGroup = m_strImpRepuesto
                                    End If
                                Case 2, 10 'Servicio - Articulo Cita
                                    If Not String.IsNullOrEmpty(m_strImpServicio) Then
                                        oCotizacion.Lines.TaxCode = m_strImpServicio
                                        oCotizacion.Lines.VatGroup = m_strImpServicio
                                    End If
                                Case 3 ' Suministro
                                    If Not String.IsNullOrEmpty(m_strImpSuministro) Then
                                        oCotizacion.Lines.TaxCode = m_strImpSuministro
                                        oCotizacion.Lines.VatGroup = m_strImpSuministro
                                    End If
                                Case 4 'Servicio Externo    
                                    If Not String.IsNullOrEmpty(m_strImpServExt) Then
                                        oCotizacion.Lines.TaxCode = m_strImpServExt
                                        oCotizacion.Lines.VatGroup = m_strImpServExt
                                    End If
                                Case Else
                                    If Not String.IsNullOrEmpty(m_strImpRepuesto) Then
                                        oCotizacion.Lines.TaxCode = m_strImpRepuesto
                                        oCotizacion.Lines.VatGroup = m_strImpRepuesto
                                    End If
                            End Select
                        Else
                            oCotizacion.Lines.TaxCode = dtListaServicios.GetValue("impuesto", i)
                            oCotizacion.Lines.VatGroup = dtListaServicios.GetValue("impuesto", i)
                        End If

                    End If
                End If
            Next

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Sub

    Private Function ObtenerBaseDatosTaller(ByVal p_strCodSucur As String)
        Try

            Dim l_strBD As String

            l_strBD = Utilitarios.EjecutarConsulta(String.Format("SELECT U_BDSucursal FROM [@SCGD_SUCURSALES] with(nolock) WHERE Code = '{0}'", p_strCodSucur), _companySbo.CompanyDB, CompanySBO.Server)

            Return l_strBD

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function


    Private Function DevuelveValorItemAgenda(ByVal strUDfName As String, ByVal p_strAgenda As String) As String
        Try

            Dim strSQL As String
            Dim strResult As String
            strSQL = "SELECT {0} FROM [@SCGD_AGENDA] with (nolock) WHERE DocEntry = '{1}'"
            strSQL = String.Format(strSQL, strUDfName, p_strAgenda)

            strResult = Utilitarios.EjecutarConsulta(strSQL, m_oCompany.CompanyDB, m_oCompany.Server)

            If String.IsNullOrEmpty(strResult) Then
                strResult = -1
            End If

            Return strResult
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex
        End Try

    End Function

    Private Function ObtenerNumeroDeEquipo_PorAgenda(ByVal p_strIDAgenda As String)
        Try

            Dim l_strGrupo As String
            Dim l_strSQL As String = "Select HE.U_SCGD_Equipo from [@SCGD_AGENDA] AG with (nolock) " &
                                        " inner Join  OHEM HE with (nolock) on AG.U_CodAsesor = HE.empID " &
                                        " where DocEntry = '{0}'"

            If String.IsNullOrEmpty(p_strIDAgenda) Then
                l_strGrupo = "-1"
            Else
                l_strSQL = String.Format(l_strSQL, p_strIDAgenda)
                l_strGrupo = Utilitarios.EjecutarConsulta(l_strSQL, _companySbo.CompanyDB, _companySbo.Server)
            End If


            If String.IsNullOrEmpty(l_strGrupo) Then
                l_strGrupo = "-1"
            End If

            Return l_strGrupo

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function
    Private Function ObtenerNumeroDeEquipo_PorEmpleado(ByVal p_strCodEmpleado As String)
        Try

            Dim l_strGrupo As String
            Dim l_strSQL As String = "Select HE.U_SCGD_Equipo from OHEM HE with (nolock) where empID = '{0}'"

            If String.IsNullOrEmpty(p_strCodEmpleado) Then
                l_strGrupo = "-1"
            Else
                l_strSQL = String.Format(l_strSQL, p_strCodEmpleado)
                l_strGrupo = Utilitarios.EjecutarConsulta(l_strSQL, _companySbo.CompanyDB, _companySbo.Server)
            End If

            If String.IsNullOrEmpty(l_strGrupo) Then
                l_strGrupo = "-1"
            End If

            Return l_strGrupo

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ObtenerNombreEmpleado(ByVal p_strCodEmpleado As String)
        Try

            Dim l_strGrupo As String
            Dim l_strSQL As String = "Select HE.LastName + ' ' + FirstName from OHEM HE with (nolock) where empID = '{0}'"

            If String.IsNullOrEmpty(p_strCodEmpleado) Then
                l_strGrupo = ""
            Else
                l_strSQL = String.Format(l_strSQL, p_strCodEmpleado)
                l_strGrupo = Utilitarios.EjecutarConsulta(l_strSQL, _companySbo.CompanyDB, _companySbo.Server)
            End If

            If String.IsNullOrEmpty(l_strGrupo) Then
                l_strGrupo = ""
            End If

            Return l_strGrupo

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function


    Private Sub LimpiarCampos()
        Try
            EditTextAno.AsignaValorUserDataSource("")
            EditTextCombustible.AsignaValorUserDataSource("")
            EditTextEstilo.AsignaValorUserDataSource("")
            EditTextModelo.AsignaValorUserDataSource("")
            EditTextMotor.AsignaValorUserDataSource("")
            EditTextMarca.AsignaValorUserDataSource("")
            EditTextTiempo.AsignaValorUserDataSource("")
            EditTextServicios.AsignaValorUserDataSource("")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, FormularioSBO)
        End Try

    End Sub


    Public Sub LimpiarDatosSucursal()
        Try

            EditTextFecha.AsignaValorDataSource("")
            EditTextHora.AsignaValorDataSource("")
            EditCboAgenda.AsignaValorDataSource(Nothing)
            EditCboRazon.AsignaValorDataSource(Nothing)
            EditTextNomAsesor.AsignaValorDataSource("")
            EditCboTecnico.AsignaValorDataSource("")
            EditCboAsesor.AsignaValorDataSource("")

            EditTextHoraServicio.AsignaValorDataSource("")
            EditTextFhaServicio.AsignaValorDataSource("")

            m_strNumeroGrupo = "-1"
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, FormularioSBO)
        End Try
    End Sub

    Private Sub LimpiarDatosAgenda()
        Try
            _formularioSbo.Freeze(True)

            EditCboRazon.AsignaValorDataSource(Nothing)
            EditTextNomAsesor.AsignaValorDataSource("")
            EditCboTecnico.AsignaValorDataSource("")
            EditCboAsesor.AsignaValorDataSource("")

            EditTextHoraServicio.AsignaValorDataSource("")
            EditTextFhaServicio.AsignaValorDataSource("")

            _formularioSbo.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    Public Sub CargarMonedaLocal(Optional ByVal p_blnNuevo As Boolean = True)
        Try

            DMS_Connector.Helpers.GetCurrencies(m_strMonedaLocal, m_strModelaSistema)

            FormularioSBO.Items.Item(EditCboMoneda.UniqueId).Visible = True
            FormularioSBO.Items.Item(EditTextTipoCambio.UniqueId).Visible = False

            If p_blnNuevo Then
                EditCboMoneda.AsignaValorDataSource(m_strMonedaLocal)
                EditTextTipoCambio.AsignaValorDataSource(1)
            Else
                If EditCboMoneda.ObtieneValorDataSource <> m_strMonedaLocal Then
                    FormularioSBO.Items.Item(EditTextTipoCambio.UniqueId).Visible = True
                Else
                    FormularioSBO.Items.Item(EditTextTipoCambio.UniqueId).Visible = False
                    EditTextTipoCambio.AsignaValorDataSource(1)
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function ManejaTipoCambio(ByRef bubbleEvent As Boolean) As Boolean
        Try

            Dim l_strSQLTipoC As String
            Dim l_FhaConta As Date

            Dim l_decTC As Decimal
            Dim l_strTC As Decimal

            Dim l_blnResult As Boolean = True

            l_strSQLTipoC = "Select RateDate, Currency, Rate  from ORTT with (nolock) where RateDate = '{0}' and Currency = '{1}'"

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()

            If m_strMonedaOrigen <> m_strMonedaDestino Then
                If m_strMonedaDestino = m_strMonedaLocal Then

                    EditTextTipoCambio.AsignaValorDataSource(1)
                ElseIf m_strMonedaDestino <> m_strMonedaLocal Then

                    If Not String.IsNullOrEmpty(EditTextFechaDoc.ObtieneValorDataSource) Then
                        l_FhaConta = DateTime.ParseExact(EditTextFechaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                    Else
                        l_FhaConta = Date.Now
                    End If

                    l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), EditCboMoneda.ObtieneValorDataSource)

                    md_Local.Clear()
                    md_Local.ExecuteQuery(l_strSQLTipoC)

                    If String.IsNullOrEmpty(md_Local.GetValue("Rate", 0)) OrElse md_Local.GetValue("Rate", 0) = 0 Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        EditCboMoneda.AsignaValorDataSource(m_strMonedaOrigen)
                        bubbleEvent = False
                        l_blnResult = False

                    Else
                        l_strTC = md_Local.GetValue("Rate", 0)
                        l_decTC = Decimal.Parse(l_strTC)
                        FormularioSBO.DataSources.DBDataSources.Item(m_strCita).SetValue("U_TipoC", 0, l_decTC.ToString(n))
                    End If
                End If
            End If
            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub ManejaEstadoTextTipoCambio()
        Try
            FormularioSBO.Freeze(True)

            If EditCboMoneda.ObtieneValorDataSource.Equals(m_strMonedaLocal) Then
                FormularioSBO.Items.Item(EditTextTipoCambio.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            Else
                FormularioSBO.Items.Item(EditTextTipoCambio.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            End If
            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    ''' <summary>
    ''' Habilita los campos motivo de cancelación cuando la cita se encuentra en ese estado
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ManejaEstadoMotivoCancelacion()
        Dim strEstado As String = String.Empty

        Try
            FormularioSBO.Freeze(True)

            strEstado = EditCboEstado.ObtieneValorDataSource
            If strEstado = "3" Then
                If Not FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE Then
                    FormularioSBO.Items.Item("cboMCanc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    FormularioSBO.Items.Item("txtCCan").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                End If
            Else
                FormularioSBO.Items.Item("cboMCanc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                FormularioSBO.Items.Item("txtCCan").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            End If

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub CalculaTotales()
        Try
            Dim l_strMonLocal As String
            Dim l_strMonOrigen As String
            Dim l_strMonDestido As String

            Dim l_decTCOrigen As Decimal
            Dim l_decTCDestino As Decimal

            Dim l_decPreciosBase As Decimal
            Dim l_decTotDestino As Decimal

            Dim l_decCantidad As Decimal
            Dim l_decTotal As Decimal
            Dim l_decTotalDoc As Decimal
            Dim l_decTotalImp As Decimal
            Dim l_decPorImp As Decimal
            Dim l_strImpuesto As String

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()

            FormularioSBO.Freeze(True)

            MatrizServicios.Matrix.FlushToDataSource()

            l_strMonOrigen = m_strMonedaOrigen
            l_strMonDestido = m_strMonedaDestino

            l_decTCOrigen = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(EditTextFechaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
            l_decTCDestino = Decimal.Parse(EditTextTipoCambio.ObtieneValorDataSource, n)

            For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                l_strImpuesto = dtListaServicios.GetValue("impuesto", i)
                l_decPorImp = Utilitarios.RetornaImpuestoVenta(l_strImpuesto, DateTime.Now)
                
                l_strMonDestido = EditCboMoneda.ObtieneValorDataSource
                l_strMonOrigen = dtListaServicios.GetValue("moneda", i)

                l_decTCOrigen = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(EditTextFechaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
                l_decTCDestino = Decimal.Parse(EditTextTipoCambio.ObtieneValorDataSource, n)

                l_decPreciosBase = dtListaServicios.GetValue("precio", i)

                l_decCantidad = Decimal.Parse(IIf(String.IsNullOrEmpty(dtListaServicios.GetValue("cantidad", i)), 0, dtListaServicios.GetValue("cantidad", i)), n)

                If l_strMonDestido = l_strMonOrigen Then

                    l_decTotDestino = l_decCantidad * l_decPreciosBase

                ElseIf l_strMonDestido <> l_strMonOrigen Then
                    If l_decTCDestino = 0 Then
                        l_decTCDestino = 1
                    End If
                    If l_decTCOrigen = 0 Then
                        l_decTCOrigen = 1
                    End If

                    If l_strMonOrigen = l_strMonLocal Then

                        l_decTotDestino = (l_decPreciosBase * l_decCantidad) / l_decTCDestino

                    ElseIf l_strMonDestido = l_strMonLocal Then

                        l_decTotDestino = (l_decPreciosBase * l_decCantidad) / l_decTCOrigen

                    Else
                        l_decTotDestino = ((l_decPreciosBase * l_decCantidad) * l_decTCOrigen) / l_decTCDestino

                    End If

                End If

                dtListaServicios.SetValue("total", i, l_decTotDestino.ToString(n))

                l_decTotal = l_decTotal + l_decTotDestino
                l_decTotalImp = l_decTotalImp + (l_decTotDestino * (l_decPorImp / 100))

                l_decTotDestino = 0
            Next

            l_decTotalDoc = l_decTotal + l_decTotalImp
            EditTextTotalLineas.AsignaValorDataSource(l_decTotal.ToString(n))
            EditTextTotalImpuesto.AsignaValorDataSource(l_decTotalImp.ToString(n))
            EditTextTotalDocumento.AsignaValorDataSource(l_decTotalDoc.ToString(n))

            MatrizServicios.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    Public Sub ManejoCambioDeMoneda()
        Try
            Dim l_strMonLocal As String
            Dim l_strMonOrigen As String
            Dim l_strMonDestido As String

            Dim l_decTCOrigen As Decimal
            Dim l_decTCDestino As Decimal

            Dim l_decLineasBase As Decimal
            Dim l_decTotImpBase As Decimal
            Dim l_decTotBase As Decimal


            Dim l_decLineasDestino As Decimal
            Dim l_decTotImpDestino As Decimal
            Dim l_decTotDestino As Decimal

            FormularioSBO.Freeze(True)

            MatrizServicios.Matrix.FlushToDataSource()

            l_strMonOrigen = m_strMonedaOrigen
            l_strMonDestido = m_strMonedaDestino

            l_decTCOrigen = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(EditTextFechaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
            l_decTCDestino = Decimal.Parse(EditTextTipoCambio.ObtieneValorDataSource, n)

            Dim l_decPrecioLineas(dtListaServicios.Rows.Count - 1) As Decimal
            Dim l_decTotalLineas(dtListaServicios.Rows.Count - 1) As Decimal

            Dim l_decTotalDestino(dtListaServicios.Rows.Count - 1) As Decimal

            For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                l_decPrecioLineas(i) = dtListaServicios.GetValue("precio", i) 'Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cost_Art", i).Trim, n)
                l_decTotalLineas(i) = dtListaServicios.GetValue("total", i) 'Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTablePedidosLineas).GetValue("U_Cost_Tot", i).Trim, n)
            Next

            l_decLineasBase = Decimal.Parse(EditTextTotalLineas.ObtieneValorDataSource, n)
            l_decTotImpBase = Decimal.Parse(EditTextTotalImpuesto.ObtieneValorDataSource, n)
            l_decTotBase = Decimal.Parse(EditTextTotalDocumento.ObtieneValorDataSource, n)


            If l_strMonDestido = l_strMonOrigen Then

                For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                    l_decTotalDestino(i) = l_decTotalLineas(i)
                Next

                l_decLineasDestino = l_decLineasBase
                l_decTotImpDestino = l_decTotImpBase
                l_decTotDestino = l_decTotBase

            ElseIf l_strMonDestido <> l_strMonOrigen Then
                If l_decTCDestino = 0 Then
                    l_decTCDestino = 1
                End If
                If l_decTCOrigen = 0 Then
                    l_decTCOrigen = 1
                End If

                If l_strMonOrigen = l_strMonLocal Then

                    For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                        l_decTotalDestino(i) = l_decTotalLineas(i) / l_decTCDestino
                    Next

                    l_decLineasDestino = l_decLineasBase / l_decTCDestino
                    l_decTotImpDestino = l_decTotImpBase / l_decTCDestino
                    l_decTotDestino = l_decTotBase / l_decTCDestino

                ElseIf l_strMonDestido = l_strMonLocal Then

                    For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                        l_decTotalDestino(i) = l_decTotalLineas(i) * l_decTCOrigen
                    Next

                    l_decLineasDestino = l_decLineasBase * l_decTCOrigen
                    l_decTotImpDestino = l_decTotImpBase * l_decTCOrigen
                    l_decTotDestino = l_decTotBase * l_decTCOrigen

                Else

                    For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                        l_decTotalDestino(i) = (l_decTotalLineas(i) * l_decTCOrigen) / l_decTCDestino
                    Next

                    l_decLineasDestino = (l_decLineasBase * l_decTCOrigen) / l_decTCDestino
                    l_decTotImpDestino = (l_decTotImpBase * l_decTCOrigen) / l_decTCDestino
                    l_decTotDestino = (l_decTotBase * l_decTCOrigen) / l_decTCDestino

                End If
            End If

            For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                dtListaServicios.SetValue("total", i, l_decTotalDestino(i).ToString(n))
            Next

            EditTextTotalImpuesto.AsignaValorDataSource(l_decTotImpDestino.ToString(n))
            EditTextTotalLineas.AsignaValorDataSource(l_decLineasDestino.ToString(n))
            EditTextTotalDocumento.AsignaValorDataSource(l_decTotDestino.ToString(n))

            MatrizServicios.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub EventoValidateColumnaPrecio(ByVal pVal As SAPbouiCOM.ItemEvent)
        Try
            Dim l_strMonedaDoc As String
            Dim l_strMonedaLin As String

            FormularioSBO.Freeze(True)

            MatrizServicios.Matrix.FlushToDataSource()

            If pVal.ActionSuccess Then
                l_strMonedaDoc = EditCboMoneda.ObtieneValorDataSource
                l_strMonedaLin = dtListaServicios.GetValue("moneda", pVal.Row - 1)

                If Not l_strMonedaDoc.Equals(l_strMonedaLin) Then
                    dtListaServicios.SetValue("moneda", pVal.Row - 1, l_strMonedaDoc)
                End If
            End If

            MatrizServicios.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

            CalculaTotales()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Function ObtieneTipoCambio(ByVal p_StrMoneda As String, ByVal p_strFecha As Date) As Decimal
        Try

            Dim l_decTipoC As Double
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT with (nolock) where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM with (nolock) "

            l_strSQLTipoC = String.Format(l_strSQLTipoC,
                                          Utilitarios.RetornaFechaFormatoDB(p_strFecha, _companySbo.Server),
                                          p_StrMoneda)
            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()
            md_Local.ExecuteQuery(l_strSQLTipoC)

            If String.IsNullOrEmpty(md_Local.GetValue("Rate", 0)) Then
                l_decTipoC = -1
            Else
                l_decTipoC = md_Local.GetValue("Rate", 0)
            End If

            Return l_decTipoC

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub CargarCitaDesdePanel_Existe(ByVal strDocEntry As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim l_strCodSucursal As String
        Dim l_strDocEntry As String
        Dim l_strCardCode As String
        Dim strCodUnid As String
        Dim strDocEntryQout As String
        Dim strConsultClie As String

        Try

            If FormularioSBO IsNot Nothing Then

                FormularioSBO.Freeze(True)

                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add

                oCondition.Alias = "DocEntry"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = strDocEntry

                FormularioSBO.DataSources.DBDataSources.Item("@SCGD_CITA").Query(oConditions)

                l_strDocEntry = EditTextDocEntry.ObtieneValorDataSource()
                l_strCodSucursal = EditCboSucursal.ObtieneValorDataSource()
                l_strCardCode = EditTextCardCode.ObtieneValorDataSource()

                If Not String.IsNullOrEmpty(l_strDocEntry) Then
                    strCodUnid = EditTextUnidad.ObtieneValorDataSource()
                    strDocEntryQout = EditTextCotizacion.ObtieneValorDataSource()

                    If Not String.IsNullOrEmpty(strCodUnid) Then
                        ObtenerDatosVehiculo(strCodUnid)
                    End If
                    If Not String.IsNullOrEmpty(strDocEntryQout) Then
                        ObtenerLineasCotizacion(strDocEntryQout)
                        MarcarItemsTipoPaquete()
                    End If

                    m_strCardCode = EditTextCardCode.ObtieneValorDataSource()
                    m_strCodUnid = EditTextUnidad.ObtieneValorDataSource()
                    m_strHoraCita = EditTextHora.ObtieneValorDataSource()
                    m_strFechaCita = EditTextFecha.ObtieneValorDataSource()

                End If

                With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                    If .U_UseLisPreCli.Trim().Equals("Y") Then
                        strConsultClie = "SELECT ListNum FROM OCRD with(nolock) WHERE CardCode = '{0}' "
                        m_strListaPreciosCli = Utilitarios.EjecutarConsulta(String.Format(strConsultClie, EditTextCardCode.ObtieneValorDataSource()),m_oCompany.CompanyDB,m_oCompany.Server)
                    Else
                        m_strListaPreciosCli = .U_CodLisPre.Trim()
                    End If
                End With

                m_strCodCitasCancel = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(EditCboSucursal.ObtieneValorDataSource())).U_CodCitaCancel.Trim

                ObtenerInformacionDeTecnico()
                CalculaTiempoDeServicio()
                CalculaTotales()
                ActualizaValoresCombos()

                If EditCboEstado.ObtieneValorDataSource() = m_strCodCitasCancel Then
                    FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
                Else
                    FormularioSBO.Mode = BoFormMode.fm_OK_MODE
                End If

                FormularioSBO.Freeze(False)

            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex

        End Try

    End Sub

    Public Sub CargarCitaDesdePanel_Nueva(ByVal p_strSucursal As String,
                                    ByVal p_strAgenda As String,
                                    ByVal p_fhaCitaNueva As Date)

        Dim l_StrCitaNuevaEstado As String
        Dim l_strFecha As String
        Dim l_strHora As String



        Dim l_strSQLAgenda As String = " SELECT DocEntry, U_Cod_Sucursal, U_CodAsesor, U_CodTecnico, U_RazonCita, U_NameAsesor, U_NameTecnico  " +
            " FROM [@SCGD_AGENDA] with(nolock) where DocEntry = '{0}'"

        m_strUsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(p_strSucursal)).U_GrpTrabajo.Trim
        l_StrCitaNuevaEstado = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(p_strSucursal)).U_CodCitaNueva.Trim

        md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
        md_Local.Clear()

        Try

            If FormularioSBO IsNot Nothing Then

                FormularioSBO.Freeze(True)

                EditCboSucursal.AsignaValorDataSource(p_strSucursal)
                EditCboAgenda.AsignaValorDataSource(p_strAgenda)
                EditCboEstado.AsignaValorDataSource(l_StrCitaNuevaEstado)

                md_Local.ExecuteQuery(String.Format(l_strSQLAgenda, p_strAgenda))
                If Not String.IsNullOrEmpty(md_Local.GetValue("DocEntry", 0)) Then

                    EditCboAsesor.AsignaValorDataSource(md_Local.GetValue("U_CodAsesor", 0))
                    EditTextNomAsesor.AsignaValorDataSource(md_Local.GetValue("U_NameAsesor", 0))
                    EditCboTecnico.AsignaValorDataSource(md_Local.GetValue("U_CodTecnico", 0))
                    EditCboRazon.AsignaValorDataSource(md_Local.GetValue("U_RazonCita", 0))

                End If

                If p_fhaCitaNueva = Date.MinValue Then
                    l_strFecha = ""
                    l_strHora = ""
                Else
                    l_strFecha = p_fhaCitaNueva.ToString("yyyyMMdd")
                    l_strHora = p_fhaCitaNueva.ToString("HH") & p_fhaCitaNueva.ToString("mm")
                End If

                EditTextFechaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))
                EditTextFecha.AsignaValorDataSource(l_strFecha)
                EditTextHora.AsignaValorDataSource(l_strHora)

                ActualizaValoresCombos()
                CalculaFechaFinCita()

                If m_strUsaGruposTrabajo.Equals("Y") Then
                    _formularioSbo.Items.Item(EditCboAsesor.UniqueId).Enabled = False
                    _formularioSbo.Items.Item(EditTextFhaServicio.UniqueId).Enabled = True
                    _formularioSbo.Items.Item(EditTextHoraServicio.UniqueId).Enabled = True
                Else
                    _formularioSbo.Items.Item(EditCboAsesor.UniqueId).Enabled = True
                    _formularioSbo.Items.Item(EditTextFhaServicio.UniqueId).Enabled = False
                    _formularioSbo.Items.Item(EditTextHoraServicio.UniqueId).Enabled = False
                End If

            End If

            FormularioSBO.Freeze(False)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex

        End Try

    End Sub


    Public Sub CargarDesdePanelAsesorTecnico(ByVal p_fhaAsesor As Date,
                                             ByVal p_fhaTecnico As Date,
                                             ByVal p_strCodAsesor As String,
                                             ByVal p_strCodTecnico As String,
                                             ByVal p_strCodSucursal As String,
                                             ByVal p_strCodAgenda As String)


        Dim l_StrCitaNuevaEstado As String
        Dim l_strFhaAsesor As String
        Dim l_strFhaTecnico As String
        Dim l_strHraASesor As String
        Dim l_strHraTecnico As String

        Dim l_strSQLAgenda As String = " SELECT DocEntry, U_Cod_Sucursal, U_CodAsesor, U_CodTecnico, U_RazonCita, U_NameAsesor, U_NameTecnico  FROM [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}'"
        Dim l_strSQLConfig As String = " SELECT DocEntry, U_GrpTrabajo, U_CodCitaNueva FROM [@SCGD_CONF_SUCURSAL] with (nolock) WHERE U_Sucurs = '{0}'"

        Try

            If FormularioSBO IsNot Nothing Then

                FormularioSBO.Freeze(True)

                md_Local = _formularioSbo.DataSources.DataTables.Item("dtLocal")
                md_Local.Clear()
                md_Local.ExecuteQuery(String.Format(l_strSQLConfig, p_strCodSucursal))

                If Not String.IsNullOrEmpty(md_Local.GetValue("DocEntry", 0)) Then
                    m_strUsaGruposTrabajo = md_Local.GetValue("U_GrpTrabajo", 0)
                    l_StrCitaNuevaEstado = md_Local.GetValue("U_CodCitaNueva", 0)
                End If


                If p_fhaAsesor = Date.MinValue Then
                    l_strFhaAsesor = String.Empty
                    l_strHraASesor = String.Empty
                Else
                    l_strFhaAsesor = p_fhaAsesor.ToString("yyyyMMdd")
                    l_strHraASesor = p_fhaAsesor.ToString("HH") & p_fhaAsesor.ToString("mm")
                End If

                If p_fhaTecnico = Date.MinValue Then
                    l_strFhaTecnico = String.Empty
                    l_strHraTecnico = String.Empty
                Else
                    l_strFhaTecnico = p_fhaTecnico.ToString("yyyyMMdd")
                    l_strHraTecnico = p_fhaTecnico.ToString("HH") & p_fhaTecnico.ToString("mm")
                End If

                p_strCodAsesor = IIf(p_strCodAsesor.Equals("-1"), String.Empty, p_strCodAsesor)
                p_strCodTecnico = IIf(p_strCodTecnico.Equals("-1"), String.Empty, p_strCodTecnico)

                EditTextFecha.AsignaValorDataSource(l_strFhaAsesor)
                EditTextHora.AsignaValorDataSource(l_strHraASesor)
                EditTextFhaServicio.AsignaValorDataSource(l_strFhaTecnico)
                EditTextHoraServicio.AsignaValorDataSource(l_strHraTecnico)

                EditCboAsesor.AsignaValorDataSource(p_strCodAsesor)
                EditCboTecnico.AsignaValorDataSource(p_strCodTecnico)
                EditCboAgenda.AsignaValorDataSource(p_strCodAgenda)
                EditCboSucursal.AsignaValorDataSource(p_strCodSucursal)
                EditCboEstado.AsignaValorDataSource(l_StrCitaNuevaEstado)

                md_Local.ExecuteQuery(String.Format(l_strSQLAgenda, p_strCodAgenda))

                If Not String.IsNullOrEmpty(md_Local.GetValue("DocEntry", 0)) Then
                    EditCboRazon.AsignaValorDataSource(md_Local.GetValue("U_RazonCita", 0))
                End If

                m_strNombreBDTaller = ObtenerBaseDatosTaller(p_strCodSucursal)
                ActualizaValoresCombos()
                ObtenerInformacionDeTecnico()
                CalculaFechaFinCita()

            End If

            FormularioSBO.Freeze(False)

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            Throw ex

        End Try

    End Sub

    Public Sub AbreVentanaBuscadorCita(ByRef p_oFormularioAdicionalesCitasArt)

        g_objGestorFormularios = New GestorFormularios(ApplicationSBO)
        p_oFormularioAdicionalesCitasArt.NombreXml = System.Environment.CurrentDirectory + My.Resources.Resource.XMLFrmBuscarArtCitas
        p_oFormularioAdicionalesCitasArt.FormType = "SCGD_BCI"

        If (g_objGestorFormularios.FormularioAbierto(p_oFormularioAdicionalesCitasArt, True) = False) Then

            If (String.IsNullOrEmpty(EditTextCardCode.ObtieneValorDataSource) = False) Then
                p_oFormularioAdicionalesCitasArt.strCodCliente = EditTextCardCode.ObtieneValorDataSource

                If (String.IsNullOrEmpty(EditCboSucursal.ObtieneValorDataSource) = False) Then
                    p_oFormularioAdicionalesCitasArt.idSucursal = EditCboSucursal.ObtieneValorDataSource
                    p_oFormularioAdicionalesCitasArt.g_strUsaConfEstiMode = g_strUsaConfEstiMode
                    p_oFormularioAdicionalesCitasArt.g_strFiltroEstiMod = g_strFiltroEstiMod
                    If Not String.IsNullOrEmpty(EditTextIdVehiculo.ObtieneValorDataSource) Then
                        p_oFormularioAdicionalesCitasArt.g_CodVehi = EditTextIdVehiculo.ObtieneValorDataSource
                    Else
                        p_oFormularioAdicionalesCitasArt.g_CodVehi = 0
                    End If

                    p_oFormularioAdicionalesCitasArt.FormularioSBO = g_objGestorFormularios.CargaFormulario(p_oFormularioAdicionalesCitasArt)
                    p_oFormularioAdicionalesCitasArt.ManejadorEventoFormDataLoad()
                Else
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MsjSeleccioneUnaSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                End If
            Else
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MsjIngreseCliente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            End If

        End If


    End Sub

    Public Sub AgregaLineaVacia()
        Try
            MatrizServicios.Matrix.FlushToDataSource()
            Dim contLine As Integer = 0

            If dtListaServicios.Rows.Count = 1 AndAlso
                dtListaServicios.GetValue("codigo", 0) = String.Empty Then
                dtListaServicios.SetValue("codigo", 0, String.Empty)
            Else
                contLine = dtListaServicios.Rows.Count
                dtListaServicios.Rows.Add()

            End If
            MatrizServicios.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresMatriz(ByVal formUid As String, ByVal itemEvent As ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable, Optional ByRef p_VenBuscador As Boolean = False)
        Try
            Dim Code As String = ""
            Dim l_strSQLArticulo As String
            Dim l_strSQLCliente As String
            Dim l_decPrecio As Decimal
            Dim l_strMoneda As String
            Dim strImpuesto As String

            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                    If .U_UseLisPreCli.Trim().Equals("Y") Then
                        l_strSQLCliente = "SELECT ListNum FROM OCRD WHERE CardCode = '{0}' "
                        m_strListaPreciosCli = Utilitarios.EjecutarConsulta(String.Format(l_strSQLCliente, EditTextCardCode.ObtieneValorDataSource()),
                                                                                m_oCompany.CompanyDB,
                                                                                m_oCompany.Server)
                    Else
                        m_strListaPreciosCli = .U_CodLisPre.Trim()
                    End If
                End With
            End If

            If dtListaServicios.Rows.Count <> 0 Then
                MatrizServicios.Matrix.FlushToDataSource()
            End If

            For i As Integer = 0 To oDataTable.Rows.Count - 1
                If (p_VenBuscador) Then
                    Code = oDataTable.GetValue("Cod", i)
                Else
                    Code = oDataTable.GetValue("ItemCode", i)
                End If

                l_strSQLArticulo = "SELECT it.ItemCode As ItemCode, IT.CodeBars, it.ItemName As ItemName,  i1.Currency As Currency, i1.Price As Price, it.U_SCGD_TipoArticulo , it.U_SCGD_Duracion " & _
           " FROM OITM it" & _
           " INNER JOIN ITM1 i1 on it.ItemCode = i1.ItemCode " & _
           " WHERE  it.ItemCode = '{0}'  AND i1.PriceList = '{1}' "

                l_strSQLArticulo = String.Format(l_strSQLArticulo, Code, m_strListaPreciosCli)

                md_ArtPadre = FormularioSBO.DataSources.DataTables.Item("dtPadre")
                md_ArtPadre.Rows.Clear()
                md_ArtPadre.ExecuteQuery(l_strSQLArticulo)

                If md_ArtPadre.Rows.Count <> 0 Then
                    If Not String.IsNullOrEmpty(md_ArtPadre.GetValue("ItemCode", 0)) Then

                        If md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0) <> "5" Then

                            If String.IsNullOrEmpty(md_ArtPadre.GetValue("U_SCGD_Duracion", 0)) Then
                                l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                            Else
                                l_strMoneda = md_ArtPadre.GetValue("Currency", 0)
                            End If
                            If String.IsNullOrEmpty(md_ArtPadre.GetValue("Price", 0)) Then
                                l_decPrecio = 0
                            Else
                                l_decPrecio = md_ArtPadre.GetValue("Price", 0)

                            End If
                            strImpuesto = String.Empty
                            If DMS_Connector.Configuracion.ParamGenAddon.U_LocCR = "Y" Then
                                If Not String.IsNullOrEmpty(EditTextCardCode.ObtieneValorDataSource()) And Not String.IsNullOrEmpty(Code) Then
                                    strImpuesto = DMS_Connector.Business_Logic.ImpuestoBL.ObtenerImpuesto(_formularioSbo, EditTextCardCode.ObtieneValorDataSource(), Code)
                                End If
                            End If

                            If String.IsNullOrEmpty(strImpuesto) Then
                                If Not String.IsNullOrEmpty(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0)) Then
                                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                                        With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                                            Select Case md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0).ToString.Trim()
                                                Case "1", "5"
                                                    strImpuesto = .U_Imp_Repuestos.Trim()
                                                Case "2"
                                                    strImpuesto = .U_Imp_Serv.Trim()
                                                Case "3"
                                                    strImpuesto = .U_Imp_Suminis.Trim()
                                                Case "4"
                                                    strImpuesto = .U_Imp_ServExt.Trim()
                                                Case "11", "12"
                                                    strImpuesto = .U_Imp_Gastos.Trim()
                                            End Select
                                        End With
                                    End If
                                End If
                            End If

                            CargarArticuloEnMatriz(md_ArtPadre.GetValue("ItemCode", 0), md_ArtPadre.GetValue("ItemName", 0), 1, md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0), md_ArtPadre.GetValue("U_SCGD_Duracion", 0), "N", l_strMoneda, l_decPrecio, String.Empty, String.Empty, md_ArtPadre.GetValue("CodeBars", 0), strImpuesto)
                        Else
                            AsignaValoresTipoPaquete2(Code, m_strListaPreciosCli)
                        End If
                    End If
                End If

            Next

            MatrizServicios.Matrix.LoadFromDataSource()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaValoresTipoPaquete2(ByVal p_strItemCode As String, ByVal p_strListaPrecios As String, Optional p_strTipoPadre As String = "")
        Try
            Dim l_strHideComp As String
            Dim l_strUsaPrecioPadre As String
            Dim l_strSQLPaquete As String
            Dim l_strArticuloPadre As String

            Dim l_decCant As Decimal
            Dim l_strMoneda As String
            Dim l_decPrecio As Decimal
            Dim strImpuesto As String
            Dim strHijoPaquete As String

            'Precio de articulos subordinados
            l_strHideComp = Utilitarios.EjecutarConsulta(String.Format("Select HideComp from oitt where Code = '{0}'", p_strItemCode), CompanySBO.CompanyDB, CompanySBO.Server)

            'T: Modelo S:Servicio
            l_strUsaPrecioPadre = Utilitarios.EjecutarConsulta("Select TreePricOn from OADM", CompanySBO.CompanyDB, CompanySBO.Server)

            l_strArticuloPadre = "SELECT it.ItemCode As ItemCode,it.CodeBars, it.ItemName As ItemName,  i1.Currency As Currency, i1.Price As Price, it.U_SCGD_TipoArticulo , it.U_SCGD_Duracion , it.TreeType" & _
                                    " FROM OITM it with (nolock)" & _
                                    " INNER JOIN ITM1 i1 with (nolock) on it.ItemCode = i1.ItemCode " & _
                                    " WHERE  it.ItemCode = '{0}'  AND i1.PriceList = '{1}'"

            l_strSQLPaquete = " Select  TT.Code, oi.ItemName, oi.CodeBars, TT.Quantity, OI.U_SCGD_Duracion, T1.Currency, T1.Price, OI.U_SCGD_TipoArticulo ,  IT.Code as CodigoPadre, IT.TreeType " +
                      " from OITT IT with (nolock) " +
                      " INNER JOIN ITT1 TT with (nolock) ON IT.Code =  TT.Father " +
                      " INNER JOIN OITM OI with (nolock) ON  OI.ItemCode = TT.Code " +
                      " Inner Join ITM1 T1 with (nolock) ON T1.ItemCode =  OI.ItemCode " +
                      " where IT.Code = '{0}' and T1.PriceList = '{1}'"


            l_strArticuloPadre = String.Format(l_strArticuloPadre, p_strItemCode, p_strListaPrecios)

            md_ArtPadre = FormularioSBO.DataSources.DataTables.Item("dtPadre")
            md_ArtPadre.Clear()
            md_ArtPadre.ExecuteQuery(l_strArticuloPadre)

            If md_ArtPadre.Rows.Count <> 0 Then

                If md_ArtPadre.GetValue("TreeType", 0).ToString.Equals("T") Then '....... TIPO MODELO ....... 

                    If Not String.IsNullOrEmpty(md_ArtPadre.GetValue("ItemCode", 0)) Then

                        If String.IsNullOrEmpty(md_ArtPadre.GetValue("U_SCGD_Duracion", 0)) Then
                            l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                        Else
                            l_strMoneda = md_ArtPadre.GetValue("Currency", 0)
                        End If
                        If String.IsNullOrEmpty(md_ArtPadre.GetValue("Price", 0)) Then
                            l_decPrecio = 0
                        Else
                            l_decPrecio = md_ArtPadre.GetValue("Price", 0)
                        End If

                        strImpuesto = String.Empty
                        If DMS_Connector.Configuracion.ParamGenAddon.U_LocCR = "Y" Then
                            If Not String.IsNullOrEmpty(EditTextCardCode.ObtieneValorDataSource()) And Not String.IsNullOrEmpty(md_ArtPadre.GetValue("ItemCode", 0)) Then
                                strImpuesto = DMS_Connector.Business_Logic.ImpuestoBL.ObtenerImpuesto(_formularioSbo, EditTextCardCode.ObtieneValorDataSource(), md_ArtPadre.GetValue("ItemCode", 0))
                            End If
                        End If
                        If String.IsNullOrEmpty(strImpuesto) Then
                            If Not String.IsNullOrEmpty(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0)) Then
                                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                                    With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                                        Select Case md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0).ToString.Trim()
                                            Case "1"
                                                strImpuesto = .U_Imp_Repuestos.Trim()
                                            Case "2"
                                                strImpuesto = .U_Imp_Serv.Trim()
                                            Case "3"
                                                strImpuesto = .U_Imp_Suminis.Trim()
                                            Case "4"
                                                strImpuesto = .U_Imp_ServExt.Trim()
                                            Case "11", "12"
                                                strImpuesto = .U_Imp_Gastos.Trim()
                                        End Select
                                    End With
                                End If
                            End If
                        End If
                        
                        If String.IsNullOrEmpty(p_strTipoPadre) Then
                            strHijoPaquete = "N"
                        Else
                            strHijoPaquete = "Y"
                        End If
                        CargarArticuloEnMatriz(md_ArtPadre.GetValue("ItemCode", 0), md_ArtPadre.GetValue("ItemName", 0), 1, md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0), 0, strHijoPaquete, l_strMoneda, l_decPrecio, md_ArtPadre.GetValue("ItemCode", 0), md_ArtPadre.GetValue("TreeType", 0), md_ArtPadre.GetValue("CodeBars", 0), strImpuesto)

                    End If


                    l_strSQLPaquete = String.Format(l_strSQLPaquete, p_strItemCode, p_strListaPrecios)

                    For Each md_ArtHijos As DataRow In Utilitarios.EjecutarConsultaDataTable(l_strSQLPaquete).Rows

                        If Not String.IsNullOrEmpty(md_ArtHijos.Item("Code")) Then

                            If md_ArtHijos.Item("U_SCGD_TipoArticulo").ToString <> "5" Then

                                If String.IsNullOrEmpty(md_ArtHijos.Item("U_SCGD_Duracion")) Then
                                    l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                                Else
                                    l_strMoneda = md_ArtHijos.Item("Currency")
                                End If
                                If String.IsNullOrEmpty(md_ArtHijos.Item("Price")) Then
                                    l_decPrecio = 0
                                Else
                                    l_decPrecio = md_ArtHijos.Item("Price")
                                End If
                                If String.IsNullOrEmpty(md_ArtHijos.Item("Quantity")) Then
                                    l_decCant = 1
                                Else
                                    l_decCant = CDec(md_ArtHijos.Item("Quantity"))
                                End If
                                If Not String.IsNullOrEmpty(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0)) Then
                                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                                        With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                                            Select Case md_ArtHijos.Item("U_SCGD_TipoArticulo").ToString.Trim()
                                                Case "1", "5"
                                                    strImpuesto = .U_Imp_Repuestos.Trim()
                                                Case "2"
                                                    strImpuesto = .U_Imp_Serv.Trim()
                                                Case "3"
                                                    strImpuesto = .U_Imp_Suminis.Trim()
                                                Case "4"
                                                    strImpuesto = .U_Imp_ServExt.Trim()
                                                Case "11", "12"
                                                    strImpuesto = .U_Imp_Gastos.Trim()
                                            End Select
                                        End With
                                    End If
                                End If
                                CargarArticuloEnMatriz(md_ArtHijos.Item("Code"), md_ArtHijos.Item("ItemName"), l_decCant.ToString(n), md_ArtHijos.Item("U_SCGD_TipoArticulo"), md_ArtHijos.Item("U_SCGD_Duracion"), "Y", l_strMoneda, l_decPrecio, md_ArtHijos.Item("CodigoPadre"), md_ArtHijos.Item("TreeType"), md_ArtHijos.Item("CodeBars"), strImpuesto)
                            Else
                                AsignaValoresTipoPaquete2(md_ArtHijos.Item("Code"), p_strListaPrecios, md_ArtPadre.GetValue("TreeType", 0).ToString())
                            End If
                        End If
                    Next


                ElseIf md_ArtPadre.GetValue("TreeType", 0).ToString.Equals("S") Then ' ....... TIPO VENTAS ....... 

                    If l_strUsaPrecioPadre = "Y" OrElse
                    (l_strUsaPrecioPadre = "N" AndAlso l_strHideComp = "Y") Then

                        If Not String.IsNullOrEmpty(md_ArtPadre.GetValue("ItemCode", 0)) Then

                            If String.IsNullOrEmpty(md_ArtPadre.GetValue("U_SCGD_Duracion", 0)) Then
                                l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                            Else
                                l_strMoneda = md_ArtPadre.GetValue("Currency", 0)
                            End If
                            If String.IsNullOrEmpty(md_ArtPadre.GetValue("Price", 0)) Then
                                l_decPrecio = 0
                            Else
                                l_decPrecio = md_ArtPadre.GetValue("Price", 0)
                            End If
                            If Not String.IsNullOrEmpty(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0)) Then
                                If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                                    With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                                        Select Case md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0).ToString.Trim()
                                            Case "1", "5"
                                                strImpuesto = .U_Imp_Repuestos.Trim()
                                            Case "2"
                                                strImpuesto = .U_Imp_Serv.Trim()
                                            Case "3"
                                                strImpuesto = .U_Imp_Suminis.Trim()
                                            Case "4"
                                                strImpuesto = .U_Imp_ServExt.Trim()
                                            Case "11", "12"
                                                strImpuesto = .U_Imp_Gastos.Trim()
                                        End Select
                                    End With
                                End If
                            End If
                            If String.IsNullOrEmpty(p_strTipoPadre) Then
                                strHijoPaquete = "N"
                            Else
                                strHijoPaquete = "Y"
                            End If
                            If Not p_strTipoPadre.Trim().Equals("S") Then
                                CargarArticuloEnMatriz(md_ArtPadre.GetValue("ItemCode", 0), md_ArtPadre.GetValue("ItemName", 0), 1, md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0), 0, strHijoPaquete, l_strMoneda, l_decPrecio, md_ArtPadre.GetValue("ItemCode", 0), "S", md_ArtPadre.GetValue("CodeBars", 0), strImpuesto)
                            End If

                        End If

                        l_strSQLPaquete = String.Format(l_strSQLPaquete, p_strItemCode, p_strListaPrecios)
                        
                        For Each md_ArtHijos As DataRow In Utilitarios.EjecutarConsultaDataTable(l_strSQLPaquete).Rows

                            If Not String.IsNullOrEmpty(md_ArtHijos.Item("Code")) Then

                                If md_ArtHijos.Item("U_SCGD_TipoArticulo").ToString <> "5" Then

                                    If String.IsNullOrEmpty(md_ArtHijos.Item("U_SCGD_Duracion")) Then
                                        l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                                    Else
                                        l_strMoneda = md_ArtHijos.Item("Currency")
                                    End If
                                    If String.IsNullOrEmpty(md_ArtHijos.Item("Price")) Then
                                        l_decPrecio = 0
                                    Else
                                        l_decPrecio = 0
                                    End If
                                    If String.IsNullOrEmpty(md_ArtHijos.Item("Quantity")) Then
                                        l_decCant = 1
                                    Else
                                        l_decCant = CDec(md_ArtHijos.Item("Quantity"))
                                    End If
                                    CargarArticuloEnMatriz(md_ArtHijos.Item("Code"), md_ArtHijos.Item("ItemName"), l_decCant.ToString(n), md_ArtHijos.Item("U_SCGD_TipoArticulo"), md_ArtHijos.Item("U_SCGD_Duracion"), "Y", l_strMoneda, l_decPrecio, md_ArtHijos.Item("CodigoPadre"), "S", md_ArtHijos.Item("CodeBars"), strImpuesto)
                                Else
                                    AsignaValoresTipoPaquete2(md_ArtHijos.Item("Code"), p_strListaPrecios, md_ArtPadre.GetValue("TreeType", 0).ToString())
                                End If
                            End If
                        Next

                        'End If
                        '.............................................................................................
                    ElseIf l_strUsaPrecioPadre = "N" AndAlso l_strHideComp = "N" Then

                        If md_ArtPadre.Rows.Count <> 0 Then

                            If Not String.IsNullOrEmpty(md_ArtPadre.GetValue("ItemCode", 0)) Then


                                If String.IsNullOrEmpty(md_ArtPadre.GetValue("U_SCGD_Duracion", 0)) Then
                                    l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                                Else
                                    l_strMoneda = md_ArtPadre.GetValue("Currency", 0)
                                End If

                                l_decPrecio = 0
                                If Not String.IsNullOrEmpty(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0)) Then
                                    If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                                        With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                                            Select Case md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0).ToString.Trim()
                                                Case "1", "5"
                                                    strImpuesto = .U_Imp_Repuestos.Trim()
                                                Case "2"
                                                    strImpuesto = .U_Imp_Serv.Trim()
                                                Case "3"
                                                    strImpuesto = .U_Imp_Suminis.Trim()
                                                Case "4"
                                                    strImpuesto = .U_Imp_ServExt.Trim()
                                                Case "11", "12"
                                                    strImpuesto = .U_Imp_Gastos.Trim()
                                            End Select
                                        End With
                                    End If
                                End If
                                If String.IsNullOrEmpty(p_strTipoPadre) Then
                                    strHijoPaquete = "N"
                                Else
                                    strHijoPaquete = "Y"
                                End If
                                CargarArticuloEnMatriz(md_ArtPadre.GetValue("ItemCode", 0), md_ArtPadre.GetValue("ItemName", 0), 1, md_ArtPadre.GetValue("U_SCGD_TipoArticulo", 0), 0, strHijoPaquete, l_strMoneda, l_decPrecio, md_ArtPadre.GetValue("ItemCode", 0), md_ArtPadre.GetValue("TreeType", 0), md_ArtPadre.GetValue("CodeBars", 0), strImpuesto)

                            End If
                        End If

                        l_strSQLPaquete = String.Format(l_strSQLPaquete, p_strItemCode, p_strListaPrecios)

                        For Each md_ArtHijos As DataRow In Utilitarios.EjecutarConsultaDataTable(l_strSQLPaquete).Rows

                            If Not String.IsNullOrEmpty(md_ArtHijos.Item("Code")) Then

                                If md_ArtHijos.Item("U_SCGD_TipoArticulo").ToString <> "5" Then

                                    If String.IsNullOrEmpty(md_ArtHijos.Item("U_SCGD_Duracion")) Then
                                        l_strMoneda = EditCboMoneda.ObtieneValorDataSource
                                    Else
                                        l_strMoneda = md_ArtHijos.Item("Currency")
                                    End If
                                    If String.IsNullOrEmpty(md_ArtHijos.Item("Price")) Then
                                        l_decPrecio = 0
                                    Else
                                        l_decPrecio = md_ArtHijos.Item("Price")
                                    End If
                                    If String.IsNullOrEmpty(md_ArtHijos.Item("Quantity")) Then
                                        l_decCant = 1
                                    Else
                                        l_decCant = CDec(md_ArtHijos.Item("Quantity"))
                                    End If
                                    If Not String.IsNullOrEmpty(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0)) Then
                                        If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim)) Then
                                            With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(_formularioSbo.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim))
                                                Select Case md_ArtHijos.Item("U_SCGD_TipoArticulo").ToString.Trim()
                                                    Case "1", "5"
                                                        strImpuesto = .U_Imp_Repuestos.Trim()
                                                    Case "2"
                                                        strImpuesto = .U_Imp_Serv.Trim()
                                                    Case "3"
                                                        strImpuesto = .U_Imp_Suminis.Trim()
                                                    Case "4"
                                                        strImpuesto = .U_Imp_ServExt.Trim()
                                                    Case "11", "12"
                                                        strImpuesto = .U_Imp_Gastos.Trim()
                                                End Select
                                            End With
                                        End If
                                    End If
                                    CargarArticuloEnMatriz(md_ArtHijos.Item("Code"), md_ArtHijos.Item("ItemName"), l_decCant.ToString(n), md_ArtHijos.Item("U_SCGD_TipoArticulo"), md_ArtHijos.Item("U_SCGD_Duracion"), "Y", l_strMoneda, l_decPrecio, md_ArtHijos.Item("CodigoPadre"), md_ArtHijos.Item("TreeType"), md_ArtHijos.Item("CodeBars"), strImpuesto)
                                Else
                                    AsignaValoresTipoPaquete2(md_ArtHijos.Item("Code"), p_strListaPrecios, md_ArtPadre.GetValue("TreeType", 0).ToString())
                                End If
                            End If
                        Next
                    End If

                End If


            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Sub CargarArticuloEnMatriz(ByVal p_ItemCode As String,
                                    ByVal p_ItemName As String,
                                    ByVal p_IntCant As String,
                                    ByVal p_strTipoArt As String,
                                    ByVal p_strDuracion As String,
                                    ByVal p_strPadre As String,
                                    ByVal p_strMoneda As String,
                                    ByVal p_decPrecio As Decimal,
                                    ByVal p_strCodPadre As String,
                                    ByVal p_strTipoPaq As String,
                                    ByVal p_strCodigoBarra As String,
                                    ByVal p_strImpuesto As String)
        Try
            Dim l_pos As Integer = 0

            FormularioSBO.Freeze(True)

            If dtListaServicios.Rows.Count = 0 OrElse
                dtListaServicios.GetValue("codigo", 0) = String.Empty Then
                l_pos = 0
            Else
                l_pos = dtListaServicios.Rows.Count - 1

            End If

            dtListaServicios.SetValue("codigo", l_pos, p_ItemCode)
            dtListaServicios.SetValue("descripcion", l_pos, p_ItemName)
            dtListaServicios.SetValue("cantidad", l_pos, p_IntCant)
            dtListaServicios.SetValue("tipo", l_pos, p_strTipoArt)
            dtListaServicios.SetValue("duracion", l_pos, p_strDuracion)
            dtListaServicios.SetValue("hijo", l_pos, p_strPadre)
            dtListaServicios.SetValue("moneda", l_pos, p_strMoneda)
            dtListaServicios.SetValue("precio", l_pos, p_decPrecio.ToString(n))
            dtListaServicios.SetValue("padre", l_pos, p_strCodPadre)
            dtListaServicios.SetValue("paquete", l_pos, p_strTipoPaq)
            dtListaServicios.SetValue("barras", l_pos, p_strCodigoBarra)
            If Not String.IsNullOrEmpty(p_strImpuesto) Then dtListaServicios.SetValue("impuesto", l_pos, p_strImpuesto)

            dtListaServicios.Rows.Add()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub DesasignarValorMatriz(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Try
            Dim l_strSQL As String
            Dim l_strCodArt As String
            Dim l_strCodArtPadre As String
            Dim l_strTipoPadre As String

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()
            Dim l_pos As Integer

            Dim list As List(Of String)
            list = New List(Of String)

            l_pos = intFila - 1


            l_strCodArt = dtListaServicios.GetValue("codigo", l_pos)
            l_strCodArtPadre = ObtenerCodigoPadre(dtListaServicios.GetValue("padre", l_pos))
            l_strTipoPadre = ObtenerTipoDelPadre(dtListaServicios.GetValue("padre", l_pos))

            l_strSQL = " Select  TT.Code, TT.Quantity, OI.U_SCGD_Duracion, OI.U_SCGD_TipoArticulo " +
                        " from OITT IT with (nolock) " +
                        " INNER JOIN ITT1 TT with (nolock) ON IT.Code =  TT.Father " +
                        " INNER JOIN OITM OI with (nolock) ON  OI.ItemCode = TT.Code " +
                        " where IT.Code = '{0}' "

            If Not String.IsNullOrEmpty(l_strCodArt) Then

                If dtListaServicios.GetValue("paquete", l_pos).Equals("T") OrElse
                    String.IsNullOrEmpty(dtListaServicios.GetValue("paquete", l_pos)) Then

                    dtListaServicios.Rows.Remove(l_pos)

                ElseIf dtListaServicios.GetValue("paquete", l_pos).Equals("S") AndAlso
                    dtListaServicios.GetValue("hijo", l_pos).Equals("N") AndAlso
                    l_strTipoPadre.Equals("T") Then

                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaBorrarPaquetedeCita, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                        Dim codEliminar As String
                        codEliminar = l_strCodArtPadre & "##" & dtListaServicios.GetValue("codigo", l_pos)

                        For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                            If dtListaServicios.GetValue("padre", i).Equals(codEliminar) Then
                                list.Add(dtListaServicios.GetValue("codigo", i))
                            End If
                        Next
                        dtListaServicios.Rows.Remove(l_pos)
                    Else
                        BubbleEvent = False
                        Exit Sub
                    End If

                ElseIf dtListaServicios.GetValue("paquete", l_pos).Equals("S") AndAlso
                dtListaServicios.GetValue("hijo", l_pos).Equals("N") AndAlso
                dtListaServicios.GetValue("codigo", l_pos).Equals(dtListaServicios.GetValue("padre", l_pos)) Then

                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaBorrarPaquetedeCita, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then

                        Dim codEliminar As String
                        codEliminar = l_strCodArtPadre

                        For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                            If dtListaServicios.GetValue("padre", i).Contains(codEliminar) Then
                                list.Add(dtListaServicios.GetValue("codigo", i))
                            End If
                        Next
                        dtListaServicios.Rows.Remove(l_pos)
                    Else
                        BubbleEvent = False
                        Exit Sub
                    End If

                ElseIf dtListaServicios.GetValue("hijo", l_pos).Equals("N") AndAlso
                        dtListaServicios.GetValue("paquete", l_pos).Equals("S") AndAlso
                        dtListaServicios.GetValue("codigo", l_pos).Equals(dtListaServicios.GetValue("padre", l_pos)) Then

                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaBorrarPaquetedeCita, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                        Dim l_codPadre As String = dtListaServicios.GetValue("codigo", l_pos)

                        For i As Integer = 0 To dtListaServicios.Rows.Count - 1
                            If dtListaServicios.GetValue("padre", i).Contains(l_codPadre) AndAlso
                                Not dtListaServicios.GetValue("padre", i).Equals(dtListaServicios.GetValue("codigo", i)) Then

                                list.Add(dtListaServicios.GetValue("codigo", i))

                            End If
                        Next

                        dtListaServicios.Rows.Remove(l_pos)

                    End If

                ElseIf dtListaServicios.GetValue("paquete", l_pos).Equals("S") AndAlso
                dtListaServicios.GetValue("hijo", l_pos).Equals("Y") Then

                    _applicationSbo.SetStatusBarMessage(My.Resources.Resource.MensajeCitaBorrarTipoVenta, BoMessageTime.bmt_Short, True)

                End If
            End If

            If list.Count <> 0 Then
                For i As Integer = 0 To list.Count - 1
                    For j As Integer = 0 To dtListaServicios.Rows.Count - 1

                        If list(i) = dtListaServicios.GetValue("codigo", j) Then
                            dtListaServicios.Rows.Remove(j)
                            Exit For
                        End If
                    Next
                Next
            End If

            MatrizServicios.Matrix.LoadFromDataSource()
            intFila = -1

            If dtListaServicios.Rows.Count <= 0 Then
                dtListaServicios.Rows.Clear()
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, FormularioSBO)
        End Try
    End Sub

    Public Function ObtenerCodigoPadre(ByVal p_strColPadre As String) As String
        Try
            Dim l_strRes As String = String.Empty
            Dim l_intPos As String = -1

            l_intPos = p_strColPadre.IndexOf("##")
            If String.IsNullOrEmpty(p_strColPadre) Then
                l_strRes = String.Empty
            ElseIf Not p_strColPadre.Contains("##") Then
                l_strRes = p_strColPadre

            ElseIf p_strColPadre.Contains("##") Then

                l_strRes = p_strColPadre.Substring(l_intPos)
            End If
            Return l_strRes
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ObtenerTipoDelPadre(ByVal p_strColPadre As String) As String
        Try
            Dim l_strTipo As String = String.Empty
            Dim l_strSQL As String = " Select  TT.Code, TT.TreeType " +
                        " from OITT TT with (nolock) " +
                        " where TT.Code = '{0}' "

            md_Datos = _formularioSbo.DataSources.DataTables.Item("dtDatos")
            md_Datos.Clear()

            l_strSQL = String.Format(l_strSQL, p_strColPadre)

            md_Local2.ExecuteQuery(l_strSQL)

            If Not String.IsNullOrEmpty(md_Local2.GetValue("Code", 0)) Then
                l_strTipo = md_Local2.GetValue("TreeType", 0)
            Else
                l_strTipo = String.Empty
            End If

            Return l_strTipo
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Sub CalculaFechaFinCita()
        Try
            Dim l_strFechaCita As String
            Dim l_strFechaServ As String
            Dim l_strHoraCita As String
            Dim l_strHoraServ As String

            Dim horaCita As String
            Dim strFechaFin As String

            Dim strServFin As String
            Dim strHoraServFin As String

            Dim l_fhaCita As Date
            Dim l_FhaCitaFin As Date

            Dim l_fhaServ As Date
            Dim l_FhaServFin As Date

            Dim l_strSQLSucursal As String
            Dim l_strSQLAgenda As String
            Dim l_fhaHoraInicioTaller As Date
            Dim l_FhaHoraFinTaller As Date

            Dim l_intDiffMin As Long

            Dim strIntervAgn As String
            Dim strUsaTiempoAgn As String
            Dim blnUsaTiempoAgn As Boolean
            Dim intIntervAgn As Integer

            Dim intDuracionCita As Integer
            Dim intDuracionServ As Integer
            Dim l_strCodAgenda As String
            Dim intTiempoUsado As Integer = 0
            Dim blnDiaUno As Boolean = True

            l_strSQLSucursal = " SELECT U_HoraInicio, U_HoraFin FROM [@SCGD_CONF_SUCURSAL]  SU with (nolock) WHERE U_Sucurs = '{0}'"
            l_strSQLAgenda = "Select DocEntry, U_IntervaloCitas, U_TmpServ from [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}' "

            l_strFechaCita = EditTextFecha.ObtieneValorDataSource
            l_strHoraCita = EditTextHora.ObtieneValorDataSource

            l_strFechaServ = EditTextFhaServicio.ObtieneValorDataSource
            l_strHoraServ = EditTextHoraServicio.ObtieneValorDataSource
            l_strCodAgenda = EditCboAgenda.ObtieneValorDataSource

            md_Agenda = _formularioSbo.DataSources.DataTables.Item("DatosAgenda")
            md_Agenda.Clear()

            If Not String.IsNullOrEmpty(l_strCodAgenda) Then

                l_strSQLAgenda = String.Format(l_strSQLAgenda, l_strCodAgenda)
                md_Agenda.ExecuteQuery(l_strSQLAgenda)

                If Not String.IsNullOrEmpty(md_Agenda.GetValue("DocEntry", 0)) Then
                    strIntervAgn = md_Agenda.GetValue("U_IntervaloCitas", 0)
                    strUsaTiempoAgn = md_Agenda.GetValue("U_TmpServ", 0)
                End If

                blnUsaTiempoAgn = IIf(strUsaTiempoAgn.Equals("Y"), True, False)
                intIntervAgn = CInt(strIntervAgn)
            Else
                blnUsaTiempoAgn = True
                intIntervAgn = 15
            End If


            If Not String.IsNullOrEmpty(l_strFechaCita) AndAlso
               Not String.IsNullOrEmpty(l_strHoraCita) Then

                '---------------------------------  HORA INICIO - HORA FIN TALLER  --------------------------------------------------------

                md_Configuracion.Clear()
                md_Configuracion.ExecuteQuery(String.Format(l_strSQLSucursal, EditCboSucursal.ObtieneValorDataSource()))

                If String.IsNullOrEmpty(md_Configuracion.GetValue("U_HoraInicio", 0)) OrElse
                    String.IsNullOrEmpty(md_Configuracion.GetValue("U_HoraFin", 0)) Then
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHoraInicioCierre, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Sub
                Else
                    l_fhaHoraInicioTaller = DateTime.ParseExact(l_strFechaCita & Utilitarios.FormatoHora2(md_Configuracion.GetValue("U_HoraInicio", 0)), "yyyyMMddHHmm", Nothing)
                    l_FhaHoraFinTaller = DateTime.ParseExact(l_strFechaCita & Utilitarios.FormatoHora2(md_Configuracion.GetValue("U_HoraFin", 0)), "yyyyMMddHHmm", Nothing)
                End If
                '--------------------------------- ------------------------ --------------------------------------------------------

                If strUsaTiempoAgn.Equals("Y") Then
                    intDuracionCita = ObtenerDuracionCita(blnUsaTiempoAgn, intIntervAgn)
                Else
                    intDuracionCita = intIntervAgn
                End If

                l_fhaCita = DateTime.ParseExact(l_strFechaCita & Utilitarios.FormatoHora2(l_strHoraCita), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
                l_FhaCitaFin = l_fhaCita.AddMinutes(intDuracionCita)

                If l_FhaCitaFin > l_FhaHoraFinTaller Then

                    l_intDiffMin = DateDiff(DateInterval.Minute, l_FhaHoraFinTaller, l_FhaCitaFin)

                    If l_intDiffMin = 0 Then
                        l_intDiffMin = 15
                    End If

                    l_FhaCitaFin = l_fhaHoraInicioTaller.AddDays(1)
                    l_FhaCitaFin = l_FhaCitaFin.AddMinutes(l_intDiffMin)

                End If

                strFechaFin = l_FhaCitaFin.ToString("yyyyMMdd")
                horaCita = l_FhaCitaFin.ToString("HH") & l_FhaCitaFin.ToString("mm")

                EditTextFechaCitaFin.AsignaValorDataSource(strFechaFin)
                EditTextHoraCitaFin.AsignaValorDataSource(horaCita)

            End If
            '------------------------------------------------------------------------------------------------------------

            If m_strUsaGruposTrabajo.Equals("Y") Then
                Dim intDuracionTallerAbierto As Integer = 0

                If Not String.IsNullOrEmpty(l_strFechaServ) AndAlso
                    Not String.IsNullOrEmpty(l_strHoraServ) Then

                    '---------------------------------  HORA INICIO - HORA FIN TALLER  --------------------------------------------------------

                    md_Configuracion.Clear()
                    md_Configuracion.ExecuteQuery(String.Format(l_strSQLSucursal, EditCboSucursal.ObtieneValorDataSource()))

                    If String.IsNullOrEmpty(md_Configuracion.GetValue("U_HoraInicio", 0)) OrElse
                        String.IsNullOrEmpty(md_Configuracion.GetValue("U_HoraFin", 0)) Then
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHoraInicioCierre, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Exit Sub
                    Else
                        l_fhaHoraInicioTaller = DateTime.ParseExact(l_strFechaServ & Utilitarios.FormatoHora2(md_Configuracion.GetValue("U_HoraInicio", 0)), "yyyyMMddHHmm", Nothing)
                        l_FhaHoraFinTaller = DateTime.ParseExact(l_strFechaServ & Utilitarios.FormatoHora2(md_Configuracion.GetValue("U_HoraFin", 0)), "yyyyMMddHHmm", Nothing)
                        intDuracionTallerAbierto = DateDiff(DateInterval.Minute, l_fhaHoraInicioTaller, l_FhaHoraFinTaller)

                    End If
                    '--------------------------------- ---------------------------- --------------------------------------------------------

                    intDuracionServ = ObtenerDuracionCita(True, intIntervAgn)

                    If intDuracionServ < 15 Then
                        intDuracionServ = 15
                    End If

                    l_fhaServ = DateTime.ParseExact(l_strFechaServ & Utilitarios.FormatoHora2(l_strHoraServ), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
                    l_FhaServFin = l_fhaServ.AddMinutes(intDuracionServ)

                    While intDuracionServ > intTiempoUsado
                        If l_FhaServFin > l_FhaHoraFinTaller Then
                            l_intDiffMin = DateDiff(DateInterval.Minute, l_FhaHoraFinTaller, l_FhaServFin)
                            If l_intDiffMin <= intDuracionTallerAbierto Then

                                If l_intDiffMin = 0 Then
                                    l_intDiffMin = 15
                                End If

                                l_FhaServFin = l_fhaHoraInicioTaller.AddDays(1)
                                l_FhaServFin = l_FhaServFin.AddMinutes(l_intDiffMin)

                                intTiempoUsado = intDuracionServ

                            ElseIf l_intDiffMin > intDuracionTallerAbierto Then

                                If blnDiaUno Then
                                    intTiempoUsado += DateDiff(DateInterval.Minute, l_fhaServ, l_FhaHoraFinTaller)
                                    blnDiaUno = False
                                End If

                                If l_intDiffMin <= intDuracionTallerAbierto Then
                                    ' intTiempoUsado += l_intDiffMin
                                Else
                                    intTiempoUsado += intDuracionTallerAbierto

                                    l_FhaHoraFinTaller = l_FhaHoraFinTaller.AddDays(1)
                                    l_fhaHoraInicioTaller = l_fhaHoraInicioTaller.AddDays(1)

                                    l_FhaServFin = l_fhaHoraInicioTaller
                                    l_FhaServFin = l_FhaServFin.AddMinutes(intDuracionTallerAbierto)
                                    l_FhaServFin = l_FhaServFin.AddMinutes(intDuracionServ - intTiempoUsado)

                                End If

                            End If
                        Else
                            intDuracionServ = intTiempoUsado
                        End If

                    End While

                    strServFin = l_FhaServFin.ToString("yyyyMMdd")
                    strHoraServFin = l_FhaServFin.ToString("HH") & l_FhaServFin.ToString("mm")

                    EditTextFechaServFin.AsignaValorDataSource(strServFin)
                    EditTextHoraServFin.AsignaValorDataSource(strHoraServFin)

                End If

            End If

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub CalculaTiempoDeServicio(Optional ByVal p_NuenoServ As Boolean = False)

        Try
            Dim l_decTiempo As Decimal
            Dim l_decTSerRap As Decimal
            Dim l_decTiempoEstandar As Decimal
            Dim l_intServicios As Integer
            Dim l_strCodTecnico As String
            Dim l_strSQLConsultaTiempoOtorgado As String = "  Select U_SCGD_TiOtor as TiempoOtorgado from QUT1 as Q with(nolock) " &
                                                                          " inner join OQUT as OQ with(nolock) on q.DocEntry = oq.DocEntry  " &
                                                                          " where OQ.U_SCGD_NoSerieCita = '{0}' and OQ.U_SCGD_NoCita = '{1}' and Q.U_SCGD_TiOtor != 0  "
            Dim l_strSerieCita As String = EditTextNumSerie.ObtieneValorDataSource
            Dim l_strValorCita As String = EditTextNumCita.ObtieneValorDataSource
            Dim l_strValorTiempootorgado As String


            Dim l_strDuracion As String
            Dim l_decDuracion As Decimal
            Dim l_StrCantidad As String
            Dim l_decCantidad As Decimal
            Dim l_DecTiempoOtorgado As Decimal

            l_strCodTecnico = EditCboTecnico.ObtieneValorDataSource
            LimpiarTiempoServicio()

            FormularioSBO.Freeze(True)

            MatrizServicios.Matrix.FlushToDataSource()

            l_strValorTiempootorgado = Utilitarios.EjecutarConsulta(String.Format(l_strSQLConsultaTiempoOtorgado, l_strSerieCita, l_strValorCita), _companySbo.CompanyDB, _companySbo.Server)


            If Not String.IsNullOrEmpty(l_strValorTiempootorgado) Or (l_strValorTiempootorgado = "0" And l_strValorTiempootorgado = "") Then
                l_DecTiempoOtorgado = Decimal.Parse(l_strValorTiempootorgado)
            End If

            For i As Integer = 0 To dtListaServicios.Rows.Count - 1

                If dtListaServicios.GetValue("tipo", i) = "2" Then

                    l_strDuracion = dtListaServicios.GetValue("duracion", i)
                    l_StrCantidad = dtListaServicios.GetValue("cantidad", i)

                    If String.IsNullOrEmpty(l_strDuracion) Then
                        l_decDuracion = 0
                    Else
                        l_decDuracion = Decimal.Parse(l_strDuracion, n)
                    End If
                    If String.IsNullOrEmpty(l_StrCantidad) Then
                        l_decCantidad = 0
                    Else
                        l_decCantidad = Decimal.Parse(l_StrCantidad, n)
                    End If

                    l_decTiempoEstandar = l_decTiempoEstandar + (l_decDuracion * l_decCantidad)
                    l_intServicios = l_intServicios + l_decCantidad

                End If

            Next

            If Not String.IsNullOrEmpty(m_strTiempoServEmpleado) Then
                l_decTSerRap = Decimal.Parse(m_strTiempoServEmpleado)
                If l_DecTiempoOtorgado <> 0D Then
                    l_decTiempo = l_decTSerRap + l_DecTiempoOtorgado
                Else
                    l_decTiempo = l_decTSerRap
                End If
            Else
                If l_DecTiempoOtorgado <> 0D Then
                    l_decTiempo = l_decTiempoEstandar + l_DecTiempoOtorgado
                Else
                    l_decTiempo = l_decTiempoEstandar
                End If
            End If

            EditTextTiempo.AsignaValorUserDataSource(l_decTiempo.ToString(n))
            EditTextServicios.AsignaValorUserDataSource(l_intServicios)

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub LimpiarTiempoServicio()
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oLabel As SAPbouiCOM.StaticText

            oitem = _formularioSbo.Items.Item("89")
            oLabel = CType(oitem.Specific, SAPbouiCOM.StaticText)

            'oLabel.Caption = "Minutos"

            EditCbxTiempo.AsignaValorUserDataSource("N")

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Function ValidarDatos(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim blnResult As Boolean = False

            Dim strCliente As String
            Dim strUnidad As String
            Dim strAgenda As String
            Dim strRazon As String
            Dim strUsaArt As String
            Dim strFecha As String
            Dim strHora As String
            Dim strFechaServicio As String
            Dim strHoraServicio As String
            Dim strSucursal As String
            Dim strHoraServidor As String
            Dim strNumCot As String
            Dim strNumSuspension As String
            Dim strAsesor As String
            Dim strNumOT As String
            Dim strTecnico As String

            Dim l_blnUsaArticulo As Boolean

            Dim l_strSQLCitas As String
            Dim l_strSQLAgenda As String
            Dim l_strSQLSucursal As String
            Dim l_strSQLNumOT As String

            Dim l_strComments As String = String.Empty

            Dim fhaServidor As Date
            Dim fhaCitaComparar As Date
            Dim fhaServComparar As Date

            Dim hraCita As Date

            Dim l_numDiaCita As Integer
            Dim l_intCitasDia As Integer
            Dim l_intDuracionCita As Integer
            Dim l_intIntervalo As Integer
            Dim l_blnAgnTimpoServ As Boolean

            Dim l_HoraInicio As Date = "1900-01-01 08:00"
            Dim l_HoraFin As Date = "1900-01-01 18:00"

            strCliente = EditTextCardCode.ObtieneValorDataSource()
            strUnidad = EditTextUnidad.ObtieneValorDataSource()
            strAgenda = EditCboAgenda.ObtieneValorDataSource()
            strRazon = EditCboRazon.ObtieneValorDataSource()
            strUsaArt = EditCbxArticulos.ObtieneValorDataSource()
            strFecha = EditTextFecha.ObtieneValorDataSource()
            strHora = EditTextHora.ObtieneValorDataSource()
            strFechaServicio = EditTextFhaServicio.ObtieneValorDataSource
            strHoraServicio = EditTextHoraServicio.ObtieneValorDataSource
            strSucursal = EditCboSucursal.ObtieneValorDataSource()
            strNumCot = EditTextCotizacion.ObtieneValorDataSource()
            strAsesor = EditCboAsesor.ObtieneValorDataSource
            strTecnico = EditCboTecnico.ObtieneValorDataSource()
            l_strComments = EditTextObservaciones.ObtieneValorDataSource()


            l_strSQLCitas = " SELECT CI.DocEntry, CI.U_Num_Serie,CI.U_NumCita,CI.U_FechaCita, CI.U_HoraCita,CI.U_Cod_Agenda,CI.U_Cod_Sucursal, CI.U_Num_Cot, SUM (IT.U_SCGD_Duracion) as U_SCGD_Duracion" & _
                           " FROM [@SCGD_CITA] CI with (nolock)" & _
                           " LEFT OUTER JOIN  OQUT QU with (nolock) ON	QU.DocEntry = CI.U_Num_Cot	AND QU.U_SCGD_NoSerieCita is not null AND QU.U_SCGD_NoCita is not null" & _
                           " LEFT OUTER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry	AND Q1.U_SCGD_Aprobado in (1, 4)" & _
                           " INNER JOIN OITM IT with (nolock) ON IT.ItemCode = Q1.ItemCode" & _
                           " WHERE CI.U_Cod_Agenda	= '{0}' AND CI.U_Cod_Sucursal = '{3}' AND U_FechaCita BETWEEN '{1}' AND '{2}' AND CI.U_Estado <> '{4}'" & _
                           " GROUP BY CI.DocEntry, CI.U_NumCita, CI.U_Num_Serie,CI.U_FechaCita, CI.U_HoraCita,CI.U_Cod_Agenda,CI.U_Cod_Sucursal, CI.U_Num_Cot"

            l_strSQLAgenda = " SELECT " & _
                            " U_Agenda, U_EstadoLogico, U_IntervaloCitas, U_Abreviatura, U_CodAsesor, U_CodTecnico, U_RazonCita, U_ArticuloCita, U_VisibleWeb, U_CantCLunes, " & _
                            " U_CantCMartes, U_CantCMiercoles, U_CantCJueves, U_CantCViernes, U_CantCSabado, U_CantCDomingo, U_Num_Art, U_Num_Razon, U_Cod_Sucursal, U_NameAsesor, U_NameTecnico, U_TmpServ " & _
                            " FROM [@SCGD_AGENDA] with (nolock) " & _
                            " WHERE DocEntry = '{0}' AND U_Cod_Sucursal = '{1}'"

            l_strSQLSucursal = " SELECT SU.U_ArtCita, IT.ItemName, IT.U_SCGD_TipoArticulo, U_HoraInicio, U_HoraFin, isnull(U_CitaSinAsesor,'N') U_CitaSinAsesor FROM [@SCGD_CONF_SUCURSAL] SU with (nolock) " +
                                "   INNER JOIN OITM IT with (nolock) ON IT.ItemCode = SU.U_ArtCita " & _
                                "   WHERE U_Sucurs = '{0}'"

            l_strSQLNumOT = "Select U_SCGD_Numero_OT from OQUT with (nolock) where DocEntry = '{0}' "


            If EditCbxArticulos.ObtieneValorDataSource() = "N" Then
                l_blnUsaArticulo = False
            ElseIf EditCbxArticulos.ObtieneValorDataSource() = "Y" Then
                l_blnUsaArticulo = True
            End If

            md_Configuracion.Clear()
            md_Configuracion.ExecuteQuery(String.Format(l_strSQLSucursal, EditCboSucursal.ObtieneValorDataSource()))

            If String.IsNullOrEmpty(strCliente) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinCliente, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf String.IsNullOrEmpty(strUnidad) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinUnidad, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf String.IsNullOrEmpty(strAgenda) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAgenda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf String.IsNullOrEmpty(strRazon) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinRazon, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf String.IsNullOrEmpty(strFecha) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf String.IsNullOrEmpty(strHora) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHora, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            ElseIf md_Configuracion.GetValue("U_CitaSinAsesor", 0).ToString.Trim.Equals("N") AndAlso String.IsNullOrEmpty(strAsesor) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAsesor, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            Else
                blnResult = True
            End If

            If m_strUsaGruposTrabajo.Equals("Y") Then

                If String.IsNullOrEmpty(strFechaServicio) Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                ElseIf String.IsNullOrEmpty(strHoraServicio) Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHora, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                End If

            End If

            If String.IsNullOrEmpty(md_Configuracion.GetValue("U_HoraInicio", 0)) OrElse
                String.IsNullOrEmpty(md_Configuracion.GetValue("U_HoraFin", 0)) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHoraInicioCierre, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
            Else

                l_HoraInicio = DateTime.ParseExact("19000101" & Utilitarios.FormatoHora2(md_Configuracion.GetValue("U_HoraInicio", 0)), "yyyyMMddHHmm", Nothing)
                l_HoraFin = DateTime.ParseExact("19000101" & Utilitarios.FormatoHora2(md_Configuracion.GetValue("U_HoraFin", 0)), "yyyyMMddHHmm", Nothing)

            End If

            If l_strComments.Length > 254 Then
                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaTamanoObs, 1, My.Resources.Resource.btnOk) = 1 Then
                End If
            End If

            '''''''''''''''''''''''''' Informacion de la Agenda ''''''''''''''''''''''''''''''''''''

            l_strSQLAgenda = String.Format(l_strSQLAgenda, strAgenda, strSucursal)

            md_Agenda = FormularioSBO.DataSources.DataTables.Item("DatosAgenda")
            md_Agenda.Clear()
            md_Agenda.ExecuteQuery(l_strSQLAgenda)

            l_numDiaCita = fhaCitaComparar.DayOfWeek
            l_intCitasDia = ObtieneCitasDias(md_Agenda, l_numDiaCita)

            If Not String.IsNullOrEmpty(md_Agenda.GetValue("U_Agenda", 0)) Then

                l_blnAgnTimpoServ = md_Agenda.GetValue("U_TmpServ", 0) = "Y"

                l_intIntervalo = md_Agenda.GetValue("U_IntervaloCitas", 0)

                If l_intIntervalo < 15 Then
                    l_intIntervalo = 15
                End If

            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            fhaServidor = ObternerFechaServer()
            Dim hraServidor As Date
            hraServidor = CDate(String.Format("{0:HH:mm}", fhaServidor))
            strHoraServidor = String.Format("{0:HH:mm}", fhaServidor)

            fhaServidor = FormatDateTime(fhaServidor, DateFormat.ShortDate)
            fhaCitaComparar = DateTime.ParseExact(strFecha & Utilitarios.FormatoHora2(strHora), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
            fhaServComparar = DateTime.Parse(fhaServidor & " " & strHoraServidor)

            hraCita = DateTime.ParseExact("19000101" & Utilitarios.FormatoHora2(strHora), "yyyyMMddHHmm", CultureInfo.CurrentCulture)

            l_intDuracionCita = ObtenerDuracionCita(l_blnAgnTimpoServ, l_intIntervalo)

            If l_blnAgnTimpoServ = False Then
                l_intDuracionCita = l_intIntervalo
            End If

            If VerificaHora(FormUID, pVal, BubbleEvent, l_HoraInicio, l_HoraFin, l_intIntervalo) Then
                Exit Function
            End If

            If pVal.FormMode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                If EditTextCardCode.ObtieneValorDataSource() <> m_strCardCode Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCitaNoModifCliente, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                ElseIf EditTextUnidad.ObtieneValorDataSource() <> m_strCodUnid Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCitaNoModifVehiculo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                End If

                If EditCboEstado.ObtieneValorDataSource() = m_strCodCitasCancel Then
                    strNumOT = Utilitarios.EjecutarConsulta(String.Format(l_strSQLNumOT, strNumCot), CompanySBO.CompanyDB, CompanySBO.Server)
                    If Not String.IsNullOrEmpty(strNumOT) Then
                        BubbleEvent = False
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCitaLigadaConOrderTrabajo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Exit Function
                    End If
                End If
            End If

            If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then

                If fhaServComparar > fhaCitaComparar Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaFechaInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function

                ElseIf hraCita < l_HoraInicio OrElse hraCita >= l_HoraFin Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaHoraInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                End If

            ElseIf FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                If m_strHoraCita <> EditTextHora.ObtieneValorDataSource() OrElse
                    m_strFechaCita <> EditTextFecha.ObtieneValorDataSource() Then

                    If fhaServComparar > fhaCitaComparar Then
                        BubbleEvent = False
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaFechaInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Exit Function

                    ElseIf hraCita < l_HoraInicio OrElse hraCita > l_HoraFin Then
                        BubbleEvent = False
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaHoraInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Exit Function
                    End If


                End If
            End If

            If FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then

                'Valida si existen citas por dia, segun configuracion de la agenda

                l_strSQLCitas = String.Format(l_strSQLCitas, strAgenda, Utilitarios.RetornaFechaFormatoDB(fhaCitaComparar, m_oCompany.Server), _
                                                                        Utilitarios.RetornaFechaFormatoDB(fhaCitaComparar, m_oCompany.Server), _
                                                                        strSucursal, m_strCodCitasCancel)

                md_Cita = FormularioSBO.DataSources.DataTables.Item("DatosCita")
                md_Cita.Rows.Clear()
                md_Cita.ExecuteQuery(l_strSQLCitas)

                Dim lnCitas As Integer = 0

                If md_Cita.GetValue("DocEntry", 0) = "0" OrElse _
                    String.IsNullOrEmpty(md_Cita.GetValue("DocEntry", 0)) Then
                    lnCitas = 0
                Else
                    lnCitas = md_Cita.Rows.Count
                End If

                If lnCitas = l_intCitasDia Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinDisponible, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                End If

            End If

            m_strNumCotizacion = ValidarFecha(fhaCitaComparar, strNumCot, strSucursal, strAgenda, BubbleEvent, l_blnAgnTimpoServ, l_intIntervalo, strTecnico, EditTextCotizacion.ObtieneValorDataSource())

            If m_strNumCotizacion <> EditTextCotizacion.ObtieneValorDataSource() Then
                If m_strNumCotizacion > 0 Then
                    BubbleEvent = False
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitasAgendaOcupada, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                End If
            End If


            'CALCULA LA DURACION DE LA CITA EN CASO DE TENER ARTICULOS DE TIPO SERVICIO.

            strNumSuspension = ValidarSuspesionAgenda(fhaCitaComparar, strSucursal, strAgenda, l_intDuracionCita)

            If strNumSuspension <> String.Empty Then
                If Convert.ToInt32(strNumSuspension) > 0 Then
                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaChoqueSusp1 & strNumSuspension & My.Resources.Resource.MensajeCitaChoqueSusp2, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                        BubbleEvent = False
                        Exit Function
                    End If
                End If

            End If

            If l_blnUsaArticulo = False Then
                If String.IsNullOrEmpty(dtListaServicios.GetValue("codigo", 0)) Then

                    If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaContinuarSinArticulo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                        BubbleEvent = False
                        blnResult = False
                        Exit Function
                    Else
                        EditCbxArticulos.AsignaValorDataSource("Y")
                        If md_Configuracion.Rows.Count < 1 OrElse
                             (md_Configuracion.GetValue("U_ArtCita", 0) = "0") OrElse
                            String.IsNullOrEmpty(md_Configuracion.GetValue("U_ArtCita", 0)) Then

                            BubbleEvent = False
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitasArticSinConfig, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            Exit Function
                        End If
                    End If
                End If
            End If


            If ValidarChoqueCita(fhaCitaComparar, strSucursal, strAgenda, l_intDuracionCita, strTecnico) Then
                ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaReasignaCita & (md_Local.GetValue("U_Num_Serie", 0)) & "-" & (md_Local.GetValue("U_NumCita", 0)), 1, "OK")
            End If

            'Valida si el tiempo de los servicios supera la hora de cierre del taller.

            If hraCita.AddMinutes(l_intDuracionCita) > l_HoraFin Then
                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeCitaChoqueFindeHorario, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                    BubbleEvent = False
                    blnResult = False
                    Exit Function
                End If
            End If

            Return blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            BubbleEvent = False
        End Try
    End Function

    Private Function ValidarDatosServicio(ByVal formUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean) As Boolean
        Dim l_blnResult As Boolean = False
        Dim strFechaCita As String
        Dim strHoraCita As String
        Dim strFechaServ As String
        Dim strHoraServ As String
        Dim intDuracionCita As Integer
        Dim intIntevaloAgenda As Integer
        Dim strCodAgenda As String
        Dim strSucursal As String
        Dim strCodTecnico As String
        Dim strCodAsesor As String
        Dim strEquipoAsesor As String
        Dim strEquipoTecnico As String
        Dim fhaCita As Date
        Dim fhaCitaServ As Date

        Try

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()
            strFechaCita = EditTextFecha.ObtieneValorDataSource
            strHoraCita = EditTextHora.ObtieneValorDataSource
            strFechaServ = EditTextFhaServicio.ObtieneValorDataSource
            strHoraServ = EditTextHoraServicio.ObtieneValorDataSource

            strCodAgenda = EditCboAgenda.ObtieneValorDataSource()
            strSucursal = EditCboSucursal.ObtieneValorDataSource
            strCodTecnico = EditCboTecnico.ObtieneValorDataSource
            strCodAsesor = EditCboAsesor.ObtieneValorDataSource

            intIntevaloAgenda = DevuelveValorItemAgenda("U_IntervaloCitas", strCodAgenda)
            intDuracionCita = ObtenerDuracionCita(True, intIntevaloAgenda)

            fhaCita = DateTime.ParseExact(strFechaCita & Utilitarios.FormatoHora2(strHoraCita), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
            fhaCitaServ = DateTime.ParseExact(strFechaServ & Utilitarios.FormatoHora2(strHoraServ), "yyyyMMddHHmm", CultureInfo.CurrentCulture)

            strEquipoAsesor = ObtenerNumeroDeEquipo_PorEmpleado(strCodAsesor)
            strEquipoTecnico = ObtenerNumeroDeEquipo_PorEmpleado(strCodTecnico)


            If String.IsNullOrEmpty(strFechaServ) AndAlso Not String.IsNullOrEmpty(strCodTecnico) Then
                bubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaServicioSinFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return True
            ElseIf String.IsNullOrEmpty(strHoraServ) AndAlso Not String.IsNullOrEmpty(strCodTecnico) Then
                bubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaServicioSinHora, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return True
            ElseIf fhaCitaServ <= fhaCita Then
                bubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaServicioFechaPosterior, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return True
            End If

            If ValidarFechaServicio(fhaCitaServ, strSucursal, strCodTecnico, bubbleEvent, True, intIntevaloAgenda) Then
                bubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaServicioFechaOcupada, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return True
            End If
            If ValidarChoqueServicio(fhaCitaServ, strSucursal, strCodTecnico, intDuracionCita) Then

                If ApplicationSBO.MessageBox(My.Resources.Resource.ErrorCitaServicioConflictoTiempo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                    bubbleEvent = False
                    Return True
                End If
            ElseIf ValidarChoqueServicioConOrden(fhaCitaServ, strSucursal, strCodTecnico, intDuracionCita) Then
                If ApplicationSBO.MessageBox(My.Resources.Resource.ErrorCitaServicioConflictoTiempo, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                    bubbleEvent = False
                    Return True
                End If
            End If
            If strEquipoAsesor <> strEquipoTecnico Then
                bubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaServicioTecnicoAsesor, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return True
            End If

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)

        End Try
    End Function

    Public Function ValidarChoqueServicioConOrden(ByVal fhaCitaServ As Date, ByVal strSucursal As String, ByVal strCodTecnico As String, ByVal intDuracionCita As Integer) As Boolean
        Try
            Dim l_blnResutl As Boolean = False
            Dim l_strSQL As String
            Dim l_fhaOrderInicio As Date
            Dim l_fhaOrderFin As Date
            Dim l_fhaServInicio As Date
            Dim l_fhaServFin As Date
            Dim l_strFechaCitaServ As String
            Dim l_strSQL_Int As String

            l_fhaServInicio = fhaCitaServ
            l_fhaServFin = fhaCitaServ.AddMinutes(intDuracionCita)

            l_strSQL = " Select  Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT, replace(cc.horainicio,':', '') as horainicio" +
                                      " from OQUT QU  with (nolock) " +
                                      " INNER join QUT1 Q1 with (nolock) on QU.DocEntry = Q1.DocEntry  " +
                                      " left outer join {2}.dbo.SCGTA_TB_ControlColaborador  CC with (nolock) on  cc.IDActividad  = Q1.U_SCGD_IdRepxOrd   " +
                                      " left outer join {2}.dbo.SCGTA_TB_Orden ORD with (nolock) on cc.NoOrden = ORD.NoOrden  " +
                                      " where QU.U_SCGD_Estado_CotID in('1','2') " +
                                      " and (cc.FechaProgramacion = '{0}' )  " +
                                      " and Q1.U_SCGD_EmpAsig = '{1}'  " +
                                      " OR " +
                                        " (ORD.Reprogramacion = 1 AND QU.U_SCGD_Estado_CotID = '3') " +
                                        " AND (CC.FechaProgramacion = '{0}' AND CC.FechaProgramacion <> '1900-01-01' AND CC.FechaProgramacion IS Not NUll) " +
                                        " AND (CC.HoraInicio IS Not NULL AND CC.HoraInicio <> '') " +
                                        " AND q1.U_SCGD_EmpAsig = '{1}'" +
                                      " GROUP BY qU.DocEntry,Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT,cc.horainicio"

            l_strSQL_Int = " Select  Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT,  " +
                            " replace(cc.U_HoraIni,':', '')+'00' as horainicio, " +
                            " cc.U_FechPro , Q1.U_SCGD_EmpAsig " +
                            " from OQUT QU with (nolock)  " +
                            " INNER join QUT1 Q1 with (nolock) on QU.DocEntry = Q1.DocEntry   " +
                            " LEFT outer join [@SCGD_CTRLCOL] CC with (nolock) on  cc.U_IdAct  = Q1.U_SCGD_ID    " +
                            " LEFT outer join [@SCGD_OT] OT with (nolock) on OT.Code =  CC.Code " +
                            " where QU.U_SCGD_Estado_CotID in('1','2') " +
                            " and (cc.U_FechPro = '{0}' and CC.U_FechPro is not null )   " +
                            " and (cc.U_HoraIni IS NOT NULL AND cc.U_HoraIni <> '') " +
                            " and Q1.U_SCGD_EmpAsig = '{1}'   " +
                            " OR " +
                            " ( OT.U_Repro = 1 AND QU.U_SCGD_Estado_CotID = '3' " +
                            " AND  (cc.U_FechPro = '{0}' and CC.U_FechPro is not null AND cc.U_FechPro <> '1900-01-01' ) " +
                            " AND (cc.U_HoraIni IS NOT NULL AND cc.U_HoraIni <> '') " +
                            " AND  Q1.U_SCGD_EmpAsig = '{1}' " +
                            " )   " +
                            " GROUP BY qU.DocEntry,Qu.DocEntry, qu.U_SCGD_NoSerieCita, QU.U_SCGD_NoCita,QU.U_SCGD_Numero_OT,cc.U_HoraIni,cc.U_FechPro ,Q1.U_SCGD_EmpAsig "

            If Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO) Then
                l_strSQL = l_strSQL_Int
            End If

            l_strFechaCitaServ = Utilitarios.RetornaFechaFormatoDB(fhaCitaServ, _companySbo.Server)
            l_strSQL = String.Format(l_strSQL, l_strFechaCitaServ, strCodTecnico, m_strNombreBDTaller)

            md_Local = _formularioSbo.DataSources.DataTables.Item("dtLocal")
            md_Local.Clear()
            md_Local.ExecuteQuery(l_strSQL)


            If md_Local.GetValue("DocEntry", 0) <> 0 Then
                For i As Integer = 0 To md_Local.Rows.Count - 1


                    Dim l_intDuracionOrden As Integer = ObtenerDuracionOrden(md_Local.GetValue("U_SCGD_Numero_OT", i), strCodTecnico, strSucursal)
                    Dim strFechaOrden As String = EditTextFhaServicio.ObtieneValorDataSource()
                    Dim strHoraOrden As String = md_Local.GetValue("horainicio", i)


                    l_fhaOrderInicio = DateTime.ParseExact(strFechaOrden & Utilitarios.FormatoHora2(strHoraOrden), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
                    l_fhaOrderFin = l_fhaOrderInicio.AddMinutes(l_intDuracionOrden)


                    If (l_fhaServInicio < l_fhaOrderInicio AndAlso l_fhaServFin < l_fhaOrderInicio) OrElse
                        (l_fhaServInicio > l_fhaOrderFin AndAlso l_fhaServFin > l_fhaOrderFin) Then

                    Else

                        l_blnResutl = True
                        Exit For
                    End If
                Next

            End If
            Return l_blnResutl
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Public Function ValidarChoqueCita(ByVal p_fhaCita As Date, ByVal p_strSucursal As String, ByVal p_strAgenda As String, ByVal p_intDuracion As Integer, ByVal p_strTecnico As String) As Boolean
        Try
            Dim l_strSQL As String
            Dim l_blnResult As Boolean = False
            Dim l_intDuracion As Integer

            l_strSQL = " SELECT DocEntry, U_Num_Serie,U_NumCita, U_Cod_Sucursal, U_Cod_Agenda, U_FechaCita, U_HoraCita,  U_Estado, U_Num_Cot " & _
                        " FROM [@SCGD_CITA] with (nolock) " & _
                        " WHERE " & _
                        " (CONVERT(DATETIME, CONVERT (char(10), U_FechaCita, 112) + " & _
                        " 				   (SELECT CASE LEN(U_HoraCita)	WHEN 3 THEN  STUFF (U_HoraCita, 2, 0, ':') " & _
                        " 												WHEN 4 THEN  STUFF (U_HoraCita, 3, 0, ':')END) + ':00', 25)) " & _
                        " BETWEEN '{0}' and '{1}' AND U_Cod_Sucursal = '{2}' AND U_Cod_Agenda = '{3}' AND U_Estado <> '{4}' and U_Cod_Tecnico = '{5}' " & _
                        " ORDER BY U_FechaCita ASC , U_HoraCita ASC "

            l_intDuracion = p_intDuracion

            l_strSQL = String.Format(l_strSQL, Utilitarios.RetornaFechaFormatoDB(p_fhaCita, m_oCompany.Server, True), _
                                               Utilitarios.RetornaFechaFormatoDB(p_fhaCita.AddMinutes(l_intDuracion - 1), m_oCompany.Server, True), p_strSucursal, p_strAgenda, m_strCodCitasCancel, p_strTecnico)

            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Rows.Clear()
            md_Local.ExecuteQuery(l_strSQL)

            If md_Local.GetValue("U_Num_Cot", 0) <> EditTextCotizacion.ObtieneValorDataSource() Then
                If md_Local.GetValue("U_Num_Cot", 0) > "0" OrElse
                    Not String.IsNullOrEmpty(md_Local.GetValue("U_Num_Cot", 0)) Then
                    l_blnResult = True
                End If
            End If

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Function ValidarChoqueServicio(ByVal p_fhaServicio As Date, ByVal p_strSucursal As String, ByVal p_strCodTecnico As String, ByVal p_intDuracion As String) As Boolean
        Try
            Dim l_strSQL As String
            Dim l_blnResult As Boolean = False

            l_strSQL = " SELECT CI.DocEntry, CI.U_Num_Serie, CI.U_NumCita, CI.U_Cod_Sucursal, CI.U_Cod_Agenda, CI.U_FechaCita, CI.U_HoraCita, CI.U_Estado, CI.U_Num_Cot " & _
                        " FROM [@SCGD_CITA] CI with (nolock) " & _
                        " LEFT OUTER JOIN  OQUT QU with (nolock)  ON	QU.DocEntry = CI.U_Num_Cot AND QU.U_SCGD_NoSerieCita is not null " & _
                        " AND QU.U_SCGD_NoCita is not null INNER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry " & _
                        " AND Q1.U_SCGD_Aprobado in (1, 4) AND ISNULL(U_SCGD_EstAct,0) <> 3  " & _
                        " WHERE " & _
                        " (CONVERT(DATETIME, CONVERT (char(10), CI.U_FhaServ, 112) + " & _
                        " 				   (SELECT CASE LEN(CI.U_HoraServ)	WHEN 3 THEN  STUFF (CI.U_HoraServ, 2, 0, ':') " & _
                        " 												WHEN 4 THEN  STUFF (CI.U_HoraServ, 3, 0, ':')END) + ':00', 25)) " & _
                        " BETWEEN '{0}' and '{1}' AND CI.U_Cod_Sucursal = '{2}' AND CI.U_Cod_Tecnico = '{3}' AND CI.U_Estado <> '{4}' AND CI.U_Num_Serie is not null" & _
                        " ORDER BY CI.U_FechaCita ASC , CI.U_HoraCita ASC "

            l_strSQL = String.Format(l_strSQL, Utilitarios.RetornaFechaFormatoDB(p_fhaServicio, m_oCompany.Server, True), _
                                               Utilitarios.RetornaFechaFormatoDB(p_fhaServicio.AddMinutes(p_intDuracion - 1), m_oCompany.Server, True), p_strSucursal, p_strCodTecnico, m_strCodCitasCancel)
            md_Local = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            md_Local.Rows.Clear()
            md_Local.ExecuteQuery(l_strSQL)

            If md_Local.GetValue("U_Num_Cot", 0) <> EditTextCotizacion.ObtieneValorDataSource() Then
                If md_Local.GetValue("U_Num_Cot", 0) > "0" OrElse
                    Not String.IsNullOrEmpty(md_Local.GetValue("U_Num_Cot", 0)) Then
                    l_blnResult = True
                End If
            End If

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Function ObtenerDuracionCita(ByVal p_blnTiempoServ As Boolean, ByVal p_intIntervalo As Integer) As Integer
        Try
            Dim l_numMinutosCita As Integer = 0
            Dim l_Duracion As Decimal
            Dim l_Cantidad As Decimal = 0
            Dim l_strSQLConsultaTiempoOtorgado As String = "  Select U_SCGD_TiOtor as TiempoOtorgado from QUT1 as Q with(nolock) " &
                                                                         " inner join OQUT as OQ with(nolock) on q.DocEntry = oq.DocEntry  " &
                                                                         " where OQ.U_SCGD_NoSerieCita = '{0}' and OQ.U_SCGD_NoCita = '{1}' and OQ.U_SCGD_idSucursal  = '{2}' and Q.U_SCGD_TiOtor != 0  "
            Dim l_strSerieCita As String = EditTextNumSerie.ObtieneValorDataSource
            Dim l_strValorCita As String = EditTextNumCita.ObtieneValorDataSource
            Dim l_strValorSucursal As Integer = EditCboSucursal.ObtieneValorDataSource
            Dim l_strValorTiempootorgado As String
            Dim l_DecTiempoOtorgado As Integer


            l_strValorTiempootorgado = Utilitarios.EjecutarConsulta(String.Format(l_strSQLConsultaTiempoOtorgado, l_strSerieCita, l_strValorCita, l_strValorSucursal), _companySbo.CompanyDB, _companySbo.Server)


            If Not String.IsNullOrEmpty(l_strValorTiempootorgado) Or (l_strValorTiempootorgado = "0" And l_strValorTiempootorgado = "") Then

                l_DecTiempoOtorgado = Decimal.Parse(l_strValorTiempootorgado)
            End If

            If p_blnTiempoServ Then

                If Not String.IsNullOrEmpty(m_strTiempoServEmpleado) Then

                    l_numMinutosCita = Integer.Parse(m_strTiempoServEmpleado)

                    If l_DecTiempoOtorgado <> 0D Then

                        l_numMinutosCita = l_numMinutosCita + l_DecTiempoOtorgado
                    Else

                        l_numMinutosCita = l_numMinutosCita
                    End If

                Else

                    If dtListaServicios.Rows.Count <> 0 Then

                        For i As Integer = 0 To dtListaServicios.Rows.Count - 1

                            If String.IsNullOrEmpty(dtListaServicios.GetValue("duracion", i)) Then
                                l_Duracion = 0
                            Else
                                l_Duracion = Decimal.Parse(dtListaServicios.GetValue("duracion", i))
                            End If

                            If String.IsNullOrEmpty(dtListaServicios.GetValue("cantidad", i)) Then
                                l_Cantidad = 0
                            Else
                                l_Cantidad = Decimal.Parse(dtListaServicios.GetValue("cantidad", i))
                            End If

                            l_numMinutosCita += (l_Duracion * l_Cantidad)

                            l_Duracion = 0
                            l_Cantidad = 0

                        Next

                        If l_DecTiempoOtorgado <> 0D Then

                            l_numMinutosCita = l_numMinutosCita + l_DecTiempoOtorgado
                        Else

                            l_numMinutosCita = l_numMinutosCita
                        End If
                    End If
                End If
            Else
                l_numMinutosCita = p_intIntervalo
            End If

            Return l_numMinutosCita
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerDuracionOrden(ByVal p_strNoOrden As String, ByVal p_strCodTecnico As String, ByVal p_strSucursal As String) As Integer
        Try
            Dim intDuracion As Integer = 0
            Dim intDuracionEstadar As Integer = 0
            Dim intDuracionTiempoOtorgado As Integer = 0
            Dim strDuracion As String
            Dim strTiempoOtorgado As String


            Dim strSQL As String = " Select  SUM(I.U_SCGD_Duracion) " +
                                    " from OQUT QU with (nolock)" +
                                    " INNER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry " +
                                    " inner join OITM as I with (nolock) on Q1.ItemCode = I.ItemCode  " +
                                    " WHERE" +
                                    " Q1.U_SCGD_Aprobado in (1,4)   " +
                                    " AND I.U_SCGD_TipoArticulo = 2 " +
                                    " AND q1.U_SCGD_NoOT = '{0}' " +
                                    " AND Q1.U_SCGD_EmpAsig = '{1}' "

            strSQL = String.Format(strSQL, p_strNoOrden, p_strCodTecnico)

            Dim strSQLTOtor As String = "  Select U_SCGD_TiOtor as TiempoOtorgado from QUT1 as Q with(nolock) " &
                                  " inner join OQUT as OQ with(nolock) on q.DocEntry = oq.DocEntry  " &
                                  " where Q.U_SCGD_NoOT = '{0}' and OQ.U_SCGD_idSucursal = '{1}' and Q.U_SCGD_TiOtor != 0  "

            If String.IsNullOrEmpty(m_strTiempoServEmpleado) Then
                strDuracion = Utilitarios.EjecutarConsulta(strSQL, m_oCompany.CompanyDB, m_oCompany.Server)
            Else
                strDuracion = m_strTiempoServEmpleado
            End If

            strTiempoOtorgado = Utilitarios.EjecutarConsulta(String.Format(strSQLTOtor, p_strNoOrden, p_strSucursal), m_oCompany.CompanyDB, m_oCompany.Server)

            If Not String.IsNullOrEmpty(strTiempoOtorgado) Or (strTiempoOtorgado = "0" And strTiempoOtorgado = "") Then
                intDuracionTiempoOtorgado = Integer.Parse(strTiempoOtorgado)
            End If


            If Not String.IsNullOrEmpty(strDuracion) Then
                intDuracionEstadar = Integer.Parse(strDuracion)

                If intDuracionTiempoOtorgado <> 0 Then
                    intDuracion = intDuracionEstadar + intDuracionTiempoOtorgado
                Else
                    intDuracion = intDuracionEstadar
                End If
            End If

            Return intDuracion

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ObtenerCantidadEspaciosAgenda(ByVal p_strDuracion As String) As Integer
        Try
            Dim l_intResult As Integer
            Dim l_intDuracion As Integer

            If String.IsNullOrEmpty(p_strDuracion) OrElse
               p_strDuracion.Equals("-1") OrElse
               p_strDuracion.Equals("0") Then
                Return 1
            Else
                l_intDuracion = CInt(p_strDuracion)

                If (l_intDuracion Mod 15) <> 0 Then
                    l_intResult = (Math.Truncate(l_intDuracion / 15)) + 1
                Else
                    l_intResult = (Math.Truncate(l_intDuracion / 15))
                End If
            End If

            Return l_intResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function



    Public Function ValidarFecha(ByVal p_fhaCita As Date, ByVal p_strNumCot As String, ByVal p_strSucursal As String, ByVal p_strAgenda As String, ByRef BubbleEvent As Boolean, ByVal p_blnAgnTiempoServ As Boolean, ByVal p_intIntervalo As Integer, ByVal p_strTecnico As String, ByVal p_strDocEntry As String) As Integer
        Try
            Dim l_strSQLCitas As String
            Dim strFechaForm As String
            Dim l_fhaCitaFinal As DateTime
            Dim l_fhaCitaInicio As DateTime
            Dim l_intResult As Integer = 0
            Dim strFechaCita As String
            Dim strFechaCitaForm As String
            Dim dtFechaCita As Date
            Dim strHoraServ As String

            l_strSQLCitas = "SELECT CI.DocEntry, CI.U_FechaCita, CI.U_HoraCita, CI.U_Num_Cot" & _
                            " FROM [dbo].[@SCGD_CITA] CI with (nolock) " & _
                            " LEFT OUTER JOIN  OQUT QU with (nolock) ON	QU.DocEntry = CI.U_Num_Cot	AND QU.U_SCGD_NoSerieCita is not null AND QU.U_SCGD_NoCita is not null" & _
                            " LEFT OUTER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry	AND Q1.U_SCGD_Aprobado in (1, 4)" & _
                            " INNER JOIN OITM IT with (nolock) ON IT.ItemCode = Q1.ItemCode AND IT.U_SCGD_TipoArticulo='2' " & _
                            " WHERE  CI.U_FechaCita BETWEEN '{0}' AND '{0}' AND CI.U_Cod_Sucursal = '{1}' and CI.U_Cod_Agenda = '{2}' AND CI.U_Estado <> '{3}' " & _
                            " group by CI.DocEntry,  CI.U_FechaCita, CI.U_HoraCita, CI.U_Num_Cot"

            strFechaCita = EditTextFecha.ObtieneValorDataSource

            If Not String.IsNullOrEmpty(strFechaCita) Then

                strFechaForm = RegresaFecha(strFechaCita)
                dtFechaCita = Date.ParseExact(strFechaForm, "dd/MM/yyyy", CultureInfo.InvariantCulture)

                strFechaCitaForm = Utilitarios.RetornaFechaFormatoDB(dtFechaCita, CompanySBO.Server)
            End If

            l_strSQLCitas = String.Format(l_strSQLCitas, _
                                        strFechaCitaForm, _
                                          p_strSucursal, p_strAgenda, m_strCodCitasCancel)

            md_Cita = FormularioSBO.DataSources.DataTables.Item("DatosCita")
            md_Cita.Rows.Clear()
            md_Cita.ExecuteQuery(l_strSQLCitas)


            If md_Cita.GetValue("DocEntry", 0) = 0 Then
                l_intResult = 0
            Else
                p_fhaCita = DateTime.Parse(p_fhaCita)
                For i As Integer = 0 To md_Cita.Rows.Count - 1

                    l_fhaCitaInicio = DateTime.Parse(md_Cita.GetValue("U_FechaCita", i) & " " & Utilitarios.FormatoHora(md_Cita.GetValue("U_HoraCita", i)))

                    l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(p_intIntervalo - 1)

                    If p_fhaCita >= l_fhaCitaInicio AndAlso
                      p_fhaCita <= l_fhaCitaFinal Then

                        l_intResult = md_Cita.GetValue("U_Num_Cot", i)
                        Exit For
                    End If
                Next
            End If
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(p_strSucursal)) Then
                If DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(p_strSucursal)).U_GrpTrabajo.Trim().Equals("Y") Then
                    If p_strDocEntry.Trim().Equals(CStr(l_intResult)) OrElse String.IsNullOrEmpty(p_strDocEntry) Then
                        l_strSQLCitas = "SELECT CI.DocEntry, CI.U_FechaCita, CI.U_HoraServ AS U_HoraCita,ISNULL( SUM (IT.U_SCGD_Duracion * Q1.Quantity), 0) as U_SCGD_Duracion, CI.U_Num_Cot" & _
                                    " FROM [dbo].[@SCGD_CITA] CI with (nolock) " & _
                                    " LEFT OUTER JOIN  OQUT QU with (nolock) ON	QU.DocEntry = CI.U_Num_Cot	AND QU.U_SCGD_NoSerieCita is not null AND QU.U_SCGD_NoCita is not null" & _
                                    " LEFT OUTER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry	AND Q1.U_SCGD_Aprobado in (1, 4)" & _
                                    " INNER JOIN OITM IT with (nolock) ON IT.ItemCode = Q1.ItemCode AND IT.U_SCGD_TipoArticulo='2' " & _
                                    " WHERE  CI.U_FechaCita BETWEEN '{0}' AND '{0}' AND CI.U_Cod_Sucursal = '{1}' and CI.U_Cod_Agenda = '{2}' AND CI.U_Estado <> '{3}' and CI.U_Cod_Tecnico = '{4}'" & _
                                    " group by CI.DocEntry,  CI.U_FechaCita, CI.U_HoraServ, CI.U_Num_Cot"
                        strFechaCita = EditTextFhaServicio.ObtieneValorDataSource

                        If Not String.IsNullOrEmpty(strFechaCita) Then

                            strFechaForm = RegresaFecha(strFechaCita)
                            dtFechaCita = Date.ParseExact(strFechaForm, "dd/MM/yyyy", CultureInfo.InvariantCulture)

                            strFechaCitaForm = Utilitarios.RetornaFechaFormatoDB(dtFechaCita, CompanySBO.Server)
                        End If

                        l_strSQLCitas = String.Format(l_strSQLCitas, _
                                                    strFechaCitaForm, _
                                                      p_strSucursal, p_strAgenda, m_strCodCitasCancel, p_strTecnico)
                        md_Cita.Rows.Clear()
                        md_Cita.ExecuteQuery(l_strSQLCitas)

                        If md_Cita.GetValue("DocEntry", 0) = 0 Then
                            l_intResult = 0
                        Else
                            strHoraServ = EditTextHoraServicio.ObtieneValorDataSource()
                            If 3 = strHoraServ.Length Then strHoraServ = String.Format("0{0}", strHoraServ)
                            p_fhaCita = New DateTime(dtFechaCita.Year, dtFechaCita.Month, dtFechaCita.Day, strHoraServ.Substring(0, 2), strHoraServ.Substring(2, 2), 0)
                            For i As Integer = 0 To md_Cita.Rows.Count - 1

                                l_fhaCitaInicio = DateTime.Parse(md_Cita.GetValue("U_FechaCita", i) & " " & Utilitarios.FormatoHora(md_Cita.GetValue("U_HoraCita", i)))

                                If p_blnAgnTiempoServ Then
                                    l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(md_Cita.GetValue("U_SCGD_Duracion", i) - 1)
                                Else
                                    l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(p_intIntervalo - 1)
                                End If

                                If p_fhaCita >= l_fhaCitaInicio AndAlso
                                  p_fhaCita <= l_fhaCitaFinal Then

                                    l_intResult = md_Cita.GetValue("U_Num_Cot", i)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            End If
            Return l_intResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            BubbleEvent = False
        End Try
    End Function


    Public Function RegresaFecha(ByVal p_strFecha As String) As String
        Dim strMes As String
        Dim strYear As String
        Dim strDia As String
        Dim strFecha As String

        Try

            strYear = p_strFecha.Substring(0, 4)
            strMes = p_strFecha.Substring(4, 2)
            strDia = p_strFecha.Substring(6)

            strFecha = strDia & "/" & strMes & "/" & strYear

            Return strFecha

        Catch ex As Exception

            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function


    Public Function ValidarFechaServicio(ByVal p_fhaCita As Date,
                             ByVal p_strSucursal As String,
                             ByVal p_strAgenda As String,
                             ByRef BubbleEvent As Boolean,
                             ByVal p_blnAgnTiempoServ As Boolean,
                             ByVal p_intIntervalo As Integer) As Boolean
        Try
            Dim l_strSQLCitas As String
            Dim l_strSQLTiemOtor As String
            Dim l_strTiemOtor As String
            Dim l_fhaCitaFinal As DateTime
            Dim l_fhaCitaInicio As DateTime
            Dim l_intResult As Integer = 0
            Dim strFechaServ As String
            Dim strFechaServFormat As String
            Dim dtFechaServ As Date
            Dim l_blnResult As Boolean = False
            Dim l_decSerrap As Decimal
            Dim l_decTiempoEst As Decimal
            Dim l_dctiempoOtor As Decimal
            Dim l_dcTiempo As Decimal
            Dim strDiaFormateado As String
            Dim strMesFormateado As String

            l_strSQLCitas = "SELECT CI.DocEntry, CI.U_NumCita, CI.U_FechaCita, CI.U_HoraCita, CI.U_Cod_Sucursal, CI.U_Cod_Agenda, CI.U_Num_Serie, U_FhaServ, U_HoraServ, " +
                            " ISNULL( SUM (IT.U_SCGD_Duracion * Q1.Quantity), 0) as U_SCGD_Duracion,CI.U_Num_Cot" +
                            " FROM [dbo].[@SCGD_CITA] CI with (nolock)" +
                            " LEFT OUTER JOIN  OQUT QU with (nolock) ON	QU.DocEntry = CI.U_Num_Cot AND QU.U_SCGD_NoSerieCita is not null AND QU.U_SCGD_NoCita is not null" +
                            " LEFT OUTER JOIN QUT1 Q1 with (nolock) ON Q1.DocEntry = QU.DocEntry AND Q1.U_SCGD_Aprobado in (1, 4) AND ISNULL(U_SCGD_EstAct,0) <> 3 " +
                            " INNER JOIN OITM IT with (nolock) ON IT.ItemCode = Q1.ItemCode AND IT.U_SCGD_TipoArticulo = '2' " +
                            " WHERE  CI.U_FhaServ = '{0}' " +
                            " AND CI.U_Cod_Sucursal = '{1}' " +
                            " AND CI.U_Cod_Tecnico = '{2}' " +
                            " AND CI.U_Estado <> '{3}'" +
                            " group by CI.DocEntry,   CI.U_NumCita, CI.U_FechaCita, CI.U_HoraCita, CI.U_Cod_Sucursal, CI.U_Cod_Agenda, CI.U_Num_Serie, CI.U_Num_Cot, U_FhaServ, U_HoraServ"

            strFechaServ = EditTextFhaServicio.ObtieneValorDataSource


            If Not String.IsNullOrEmpty(strFechaServ) Then
                dtFechaServ = Date.ParseExact(strFechaServ, "yyyyMMdd", Nothing)
                strFechaServFormat = Utilitarios.RetornaFechaFormatoDB(dtFechaServ, CompanySBO.Server)
            End If

            If dtFechaServ.Month.ToString.Length = 1 Then
                strMesFormateado = "0" & dtFechaServ.Month
            Else
                strMesFormateado = dtFechaServ.Month.ToString()
            End If

            If dtFechaServ.Day.ToString.Length = 1 Then
                strDiaFormateado = "0" & dtFechaServ.Day
            Else
                strDiaFormateado = dtFechaServ.Day.ToString()
            End If

            l_strSQLCitas = String.Format(l_strSQLCitas, _
                                           dtFechaServ.Year & strMesFormateado & strDiaFormateado, _
                                            p_strSucursal, p_strAgenda, m_strCodCitasCancel)
            l_strSQLTiemOtor = " Select distinct(qu.U_SCGD_TiOtor) from QUT1 as qu with(nolock) " &
                               " inner join OQUT as oq with(nolock) on qu.DocEntry = oq.DocEntry " &
                               " where oq.U_SCGD_NoCita = '{0}' and oq.U_SCGD_NoSerieCita = '{1}' and qu.U_SCGD_TiOtor != 0"

            md_Cita = FormularioSBO.DataSources.DataTables.Item("DatosCita")
            md_Cita.Rows.Clear()
            md_Cita.ExecuteQuery(l_strSQLCitas)

            If md_Cita.Rows.Count - 1 = 0 Then
                l_intResult = 0
                l_blnResult = False
            Else
                p_fhaCita = DateTime.Parse(p_fhaCita)
                For i As Integer = 0 To md_Cita.Rows.Count - 1

                    l_fhaCitaInicio = DateTime.Parse(md_Cita.GetValue("U_FhaServ", i) & " " & Utilitarios.FormatoHora(md_Cita.GetValue("U_HoraServ", i)))
                    l_strTiemOtor = Utilitarios.EjecutarConsulta(String.Format(l_strSQLTiemOtor, md_Cita.GetValue("U_NumCita", i), md_Cita.GetValue("U_Num_Serie", i)), m_oCompany.CompanyDB, m_oCompany.Server)
                    If l_strTiemOtor = "" Then
                        l_strTiemOtor = "0"
                    End If
                    If Not String.IsNullOrEmpty(m_strTiempoServEmpleado) Then
                        If p_blnAgnTiempoServ Then

                            l_decSerrap = Decimal.Parse(m_strTiempoServEmpleado)
                            If l_strTiemOtor <> "0" Then
                                l_dctiempoOtor = Decimal.Parse(l_strTiemOtor)
                                l_dcTiempo = l_decSerrap + l_dctiempoOtor
                                l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(l_dcTiempo - 1)
                            Else
                                l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(l_decSerrap - 1)
                            End If

                        Else
                            l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(14)
                        End If
                    Else
                        If p_blnAgnTiempoServ Then

                            l_decTiempoEst = md_Cita.GetValue("U_SCGD_Duracion", i)
                            If l_strTiemOtor <> "0" Then
                                l_dctiempoOtor = Decimal.Parse(l_strTiemOtor)
                                l_dcTiempo = l_decTiempoEst + l_dctiempoOtor
                                l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(l_dcTiempo - 1)
                            Else
                                l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(md_Cita.GetValue("U_SCGD_Duracion", i) - 1)
                            End If

                        Else
                            l_fhaCitaFinal = l_fhaCitaInicio.AddMinutes(14)
                        End If
                    End If

                    If p_fhaCita >= l_fhaCitaInicio AndAlso
                      p_fhaCita <= l_fhaCitaFinal Then

                        l_intResult = md_Cita.GetValue("U_Num_Cot", i)
                        l_blnResult = True
                        Exit For
                    End If
                Next
            End If

            Return l_blnResult

        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)

        End Try
    End Function

    Public Function ValidarSuspesionAgenda(ByVal p_fhaCita As Date, ByVal p_strSucursal As String, ByVal p_strAgenda As String, ByVal p_numDuracionCita As Integer) As String
        Try
            Dim l_strSQLSuspension As String
            Dim l_strNumSuspension As String = String.Empty
            Dim l_fhaSuspDesde As Date
            Dim l_fhaSuspHasta As Date
            Dim l_fhaCitaInicio As Date
            Dim l_fhaCitaFin As Date


            l_strSQLSuspension = "SELECT AGS.DocEntry, AGS.U_Cod_Sucur, AGS.U_Cod_Agenda, AGS.U_Fha_Desde, AGS.U_Hora_Desde, AGS.U_Fha_Hasta, AGS.U_Hora_Hasta, AGS.U_Estado,U_Observ  " & _
                                " FROM [@SCGD_AGENDA_SUSP] AGS with (nolock) " & _
                                " WHERE AGS.U_Fha_Desde = '{0}' AND AGS.U_Cod_Sucur = '{1}' AND AGS.U_Cod_Agenda = '{2}' AND AGS.U_Estado = 'Y' "

            l_strSQLSuspension = String.Format(l_strSQLSuspension, _
                                               Utilitarios.RetornaFechaFormatoDB(p_fhaCita, m_oCompany.Server), _
                                               p_strSucursal, p_strAgenda)

            md_Suspension.Clear()
            md_Suspension.ExecuteQuery(l_strSQLSuspension)

            If md_Suspension.Rows.Count <> 0 Then
                If md_Suspension.GetValue("DocEntry", 0) <> 0 Then

                    l_fhaCitaInicio = p_fhaCita
                    l_fhaCitaFin = l_fhaCitaInicio.AddMinutes(p_numDuracionCita)

                    For i As Integer = 0 To md_Suspension.Rows.Count - 1

                        l_fhaSuspDesde = DateTime.Parse(md_Suspension.GetValue("U_Fha_Desde", i) & " " & Utilitarios.FormatoHora(md_Suspension.GetValue("U_Hora_Desde", i)))
                        l_fhaSuspHasta = DateTime.Parse(md_Suspension.GetValue("U_Fha_Hasta", i) & " " & Utilitarios.FormatoHora(md_Suspension.GetValue("U_Hora_Hasta", i)))

                        If (l_fhaSuspDesde <= l_fhaCitaInicio AndAlso l_fhaCitaInicio < l_fhaSuspHasta) OrElse
                            (l_fhaSuspDesde < l_fhaCitaFin AndAlso l_fhaCitaFin <= l_fhaSuspHasta) OrElse
                            (l_fhaCitaInicio <= l_fhaSuspDesde AndAlso l_fhaCitaFin >= l_fhaSuspHasta) Then

                            l_strNumSuspension = md_Suspension.GetValue("DocEntry", i)

                            Exit For
                        End If

                    Next

                End If
            End If
            Return l_strNumSuspension


        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ValidarPeriodoContable(ByRef BubbleEvent As Boolean) As Boolean
        Try

            Dim l_blnResult As Boolean = True
            Dim l_strSQL As String
            Dim l_fhaActual As Date
            Dim l_strFecha As String
            Dim l_strEstadoPeriodo As String = String.Empty

            l_strSQL = "Select PeriodStat FROM OFPR with (nolock) where '{0}' between  F_RefDate AND T_RefDate"

            l_fhaActual = ObternerFechaServer()
            l_strFecha = Utilitarios.RetornaFechaFormatoDB(l_fhaActual, m_oCompany.Server, False)

            l_strSQL = String.Format(l_strSQL, l_strFecha)
            l_strEstadoPeriodo = Utilitarios.EjecutarConsulta(l_strSQL, m_oCompany.CompanyDB, m_oCompany.Server)

            If l_strEstadoPeriodo = "C" Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaPeriodoCerrado, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                l_blnResult = False
            ElseIf l_strEstadoPeriodo = "Y" Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaPeriodoBloqueado, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                l_blnResult = False
            ElseIf String.IsNullOrEmpty(l_strEstadoPeriodo) Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaPeriodoNoConfig, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
                l_blnResult = False
            End If

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, FormularioSBO)
        End Try

    End Function

    Public Function ValidarCancelarCita(ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim l_blnResutl As Boolean = True
            Dim strNumOT As String
            Dim l_strSQLNumOT As String = "Select U_SCGD_Numero_OT  from OQUT with (nolock) where DocEntry = '{0}' "
            Dim strNumCot As String

            strNumOT = Utilitarios.EjecutarConsulta(String.Format(l_strSQLNumOT, strNumCot), CompanySBO.CompanyDB, CompanySBO.Server)
            If Not String.IsNullOrEmpty(strNumOT) Then
                BubbleEvent = False
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCitaLigadaConOrderTrabajo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Exit Function
                l_blnResutl = False
            End If

            Return l_blnResutl

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Function ObtieneCitasDias(ByRef p_dtAgenda As SAPbouiCOM.DataTable, ByVal p_strDia As String) As Integer
        Try
            Dim strUDF As String
            Dim l_intNumCitas As Integer = 0

            Select Case UCase(p_strDia)
                Case "1"
                    strUDF = "U_CantCLunes"
                Case "2"
                    strUDF = "U_CantCMartes"
                Case "3"
                    strUDF = "U_CantCMiercoles"
                Case "4"
                    strUDF = "U_CantCJueves"
                Case "5"
                    strUDF = "U_CantCViernes"
                Case "6"
                    strUDF = "U_CantCSabado"
                Case "0"
                    strUDF = "U_CantCDomingo"
            End Select

            If (IsDBNull(md_Agenda.GetValue(strUDF, 0)) OrElse _
                String.IsNullOrEmpty(md_Agenda.GetValue(strUDF, 0)) OrElse _
                md_Agenda.GetValue(strUDF, 0) < 0) Then
                l_intNumCitas = 0
            Else
                l_intNumCitas = md_Agenda.GetValue(strUDF, 0)
            End If

            Return l_intNumCitas

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    ''' <summary>
    ''' Verifica que la hora se ajuste a las horas definidas para citas, ya sea con duracion estandar o con la duracion de los intervalos
    ''' especificados para la agenda.
    ''' 
    ''' Actualiza en la pantalla de la cita, con la hora sugerida 
    ''' </summary>
    ''' 

    Private Function VerificaHora(ByVal FormUID As String,
                                  ByVal pVal As SAPbouiCOM.ItemEvent,
                                  ByRef BubbleEvent As Boolean,
                                ByVal p_fhaInicio As Date,
                                ByVal p_fhaCierre As Date,
                                ByVal p_intIntervalo As Integer) As Boolean
        Try
            Dim strAgenda As String
            Dim strFecha As String
            Dim strHora As String
            Dim strSucursal As String
            Dim l_strSQLCitas As String
            Dim l_HoraInicio As Date
            Dim l_HoraFin As Date
            Dim l_fhaCita As DateTime
            Dim l_horaCont As Date
            Dim l_horaCita As Date
            Dim l_horaSiguiente As Date
            Dim l_horaAnterior As Date
            Dim l_horaSugerida As Date
            Dim l_cont As Integer = 0

            Dim listHoras As New List(Of Date)
            Dim lisEliminar As New List(Of Date)
            Dim l_dtCitas As System.Data.DataTable

            Dim l_blnResult As Boolean = False

            Dim l_strSQLCitas2 As String
            Dim intIntervaloAgenda As Integer
            Dim strIntervaloAgenda As String

            strAgenda = EditCboAgenda.ObtieneValorDataSource()
            strFecha = EditTextFecha.ObtieneValorDataSource()
            strHora = EditTextHora.ObtieneValorDataSource()
            strSucursal = EditCboSucursal.ObtieneValorDataSource()

            l_HoraInicio = p_fhaInicio
            l_HoraFin = p_fhaCierre


            Dim strFechaP = DateTime.ParseExact(strFecha & Utilitarios.FormatoHora2(strHora), "yyyyMMddHHmm", CultureInfo.CurrentCulture)
            Dim strFechaHora As String = Utilitarios.RetornaFechaFormatoDB(strFechaP, m_oCompany.Server, True)

            l_fhaCita = DateTime.ParseExact(strFechaHora, Utilitarios.RetornaFormatoFechaDB(m_oCompany.Server) & " HH:mm:ss", Nothing)
            l_horaCont = l_HoraInicio
            l_horaCita = Utilitarios.RetornaFechaFormatoDB(DateTime.ParseExact("19000101" & String.Format("{0:HHmm}", l_fhaCita), "yyyyMMddHHmm", Nothing),
                                              m_oCompany.Server, True)

            l_strSQLCitas = " SELECT DocEntry, U_NumCita, U_FechaCita, U_HoraCita, U_Cod_Sucursal, U_Cod_Agenda, U_Num_Serie" & _
                " FROM [@SCGD_CITA] with (nolock) " & _
                " WHERE U_Cod_Agenda = '{0}' AND U_FechaCita BETWEEN '{1}' AND '{2}' AND U_Cod_Sucursal = '{3}' AND U_Estado <> '{4}'" & _
                " ORDER BY U_FechaCita, U_HoraCita DESC "

            l_strSQLCitas = String.Format(l_strSQLCitas, strAgenda, Utilitarios.RetornaFechaFormatoDB(l_fhaCita, m_oCompany.Server) + " 00:00:00", _
                                                                    Utilitarios.RetornaFechaFormatoDB(l_fhaCita, m_oCompany.Server) + " 23:59:59", strSucursal, m_strCodCitasCancel)


            l_strSQLCitas2 = String.Format("SELECT U_IntervaloCitas FROM [dbo].[@SCGD_AGENDA] with(nolock) WHERE U_Cod_Sucursal = {0} and DocEntry = {1}", strSucursal, strAgenda)
            md_Agenda = FormularioSBO.DataSources.DataTables.Item("DatosAgenda")
            md_Agenda.Rows.Clear()
            md_Agenda.ExecuteQuery(l_strSQLCitas2)

            strIntervaloAgenda = md_Agenda.GetValue("U_IntervaloCitas", 0).ToString.Trim

            If Not String.IsNullOrEmpty(strIntervaloAgenda) Then
                intIntervaloAgenda = Convert.ToInt32(strIntervaloAgenda)
            Else
                intIntervaloAgenda = 15
            End If

            While l_horaCont <= l_HoraFin
                listHoras.Add(l_horaCont)
                l_horaCont = l_horaCont.AddMinutes(intIntervaloAgenda)
            End While

            If Not listHoras.Contains(l_horaCita) Then

                l_dtCitas = Utilitarios.EjecutarConsultaDataTable(l_strSQLCitas, m_oCompany.CompanyDB, m_oCompany.Server)

                For i As Integer = 0 To listHoras.Count - 1
                    loRow = l_dtCitas.Select("U_HoraCita = " & String.Format("{0:HHmm}", listHoras(i)))
                    If loRow.Length <> 0 Then
                        lisEliminar.Add(listHoras(i))
                    End If
                Next

                For j As Integer = 0 To lisEliminar.Count - 1
                    listHoras.Remove(lisEliminar(j))
                Next

                If l_horaCita > l_HoraInicio AndAlso l_horaCita < l_HoraFin Then

                    For i As Integer = 0 To listHoras.Count - 1
                        If listHoras(i) < l_horaCita Then
                            l_cont += 1
                        Else
                            Exit For
                        End If
                    Next

                    If l_horaCita < listHoras(0) Then
                        l_horaSiguiente = listHoras(l_cont - 1)
                        l_horaSugerida = l_horaSiguiente

                    ElseIf l_cont = listHoras.Count Then
                        l_horaAnterior = listHoras(l_cont - 1)
                        l_horaSugerida = l_horaAnterior
                    Else
                        l_horaAnterior = listHoras(l_cont - 1)
                        l_horaSiguiente = listHoras(l_cont)

                        If DateDiff(DateInterval.Minute, l_horaAnterior, l_horaCita) <
                        DateDiff(DateInterval.Minute, l_horaCita, l_horaSiguiente) Then
                            l_horaSugerida = l_horaAnterior
                        Else
                            l_horaSugerida = l_horaSiguiente
                        End If

                    End If

                    BubbleEvent = False
                    l_blnResult = True
                    EditTextHora.AsignaValorDataSource(String.Format("{0:HHmm}", l_horaSugerida))
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeCitaFechaAjustada, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning)
                    Exit Function
                Else
                    BubbleEvent = False
                    l_blnResult = True
                    _applicationSbo.StatusBar.SetText(My.Resources.Resource.ErrorCitaHoraInvalidaAgenda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Exit Function
                End If
            End If

            Return l_blnResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
            BubbleEvent = False
        End Try
    End Function


#End Region


End Class
