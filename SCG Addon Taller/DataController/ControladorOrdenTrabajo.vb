Imports SAPbouiCOM
Imports DMS_Connector.Business_Logic.DataContract.SAPDocumento

Public Class ControladorOrdenTrabajo

#Region "Variables Globales"
    Private _company As SAPbobsCOM.Company
    Private _application As Application
#End Region

#Region "Constantes"
    Public Const strUDOOrden As String = "SCGD_OT"
#End Region

#Region "Propiedades"
    Public ReadOnly Property oCompany() As SAPbobsCOM.Company
        Get
            Return _company
        End Get
    End Property

    Public ReadOnly Property oApplication() As Application
        Get
            Return _application
        End Get
    End Property
#End Region
#Region "Variables"

#End Region

#Region "Enumeradores"

#End Region

#Region "Constructor"
    Public Sub New(ByVal company As SAPbobsCOM.Company, ByVal application As Application)
        _company = company
        _application = application
    End Sub
#End Region

#Region "Metodos"
    Public Function CrearOrdenTrabajo(ByRef p_oCotizacionActual As oDocumento, _
                                      ByRef p_oControlColaboradorList As ControlColaborador_List) As Boolean
        Try
            '***************Objetos SAP ***********
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oControlColaborador As SAPbobsCOM.GeneralData
            Dim oControlColaboradorLineas As SAPbobsCOM.GeneralDataCollection
            '***************Variables *********
            Dim strHoraInicio As String = String.Empty
            Dim strFechaProduccion As String = String.Empty
            Dim strDocEntryCotizacion As String = String.Empty

            oCompanyService = oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService(strUDOOrden)
            oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)

            With p_oCotizacionActual
                If Not String.IsNullOrEmpty(.NoOrden) Then oGeneralData.SetProperty("Code", .NoOrden)
                strDocEntryCotizacion = Convert.ToString(.DocEntry)
                oGeneralData.SetProperty("U_DocEntry", strDocEntryCotizacion)
                If Not String.IsNullOrEmpty(.NoOrden) Then oGeneralData.SetProperty("U_NoOT", .NoOrden)
                If Not String.IsNullOrEmpty(.CodigoUnidad) Then oGeneralData.SetProperty("U_NoUni", .CodigoUnidad)
                If Not String.IsNullOrEmpty(.Cono) Then oGeneralData.SetProperty("U_NoCon", .Cono)
                If Not String.IsNullOrEmpty(.Year) Then oGeneralData.SetProperty("U_Ano", .Year)
                If Not String.IsNullOrEmpty(.Placa) Then oGeneralData.SetProperty("U_Plac", .Placa)
                If Not String.IsNullOrEmpty(.DescripcionMarca) Then oGeneralData.SetProperty("U_Marc", .DescripcionMarca)
                If Not String.IsNullOrEmpty(.DescripcionEstilo) Then oGeneralData.SetProperty("U_Esti", .DescripcionEstilo)
                If Not String.IsNullOrEmpty(.DescripcionModelo) Then oGeneralData.SetProperty("U_Mode", .DescripcionModelo)
                If Not String.IsNullOrEmpty(.CodigoMarca) Then oGeneralData.SetProperty("U_CMar", .CodigoMarca)
                If Not String.IsNullOrEmpty(.CodigoEstilo) Then oGeneralData.SetProperty("U_CEst", .CodigoEstilo)
                If Not String.IsNullOrEmpty(.CodigoModelo) Then oGeneralData.SetProperty("U_CMod", .CodigoModelo)
                If Not String.IsNullOrEmpty(.NoVisita) Then oGeneralData.SetProperty("U_NoVis", .NoVisita)
                If Not String.IsNullOrEmpty(.NumeroVIN) Then oGeneralData.SetProperty("U_VIN", .NumeroVIN)
                If Not String.IsNullOrEmpty(.Kilometraje) Then oGeneralData.SetProperty("U_km", .Kilometraje.ToString())
                If Not String.IsNullOrEmpty(.TipoOT.ToString()) Then oGeneralData.SetProperty("U_TipOT", .TipoOT.ToString())
                If Not String.IsNullOrEmpty(.Sucursal) Then oGeneralData.SetProperty("U_Sucu", .Sucursal)
                If Not String.IsNullOrEmpty(.CardCode) Then oGeneralData.SetProperty("U_CodCli", .CardCode)
                If Not String.IsNullOrEmpty(.CardName) Then oGeneralData.SetProperty("U_NCli", .CardName)
                If Not String.IsNullOrEmpty(.CodigoClienteOT) Then oGeneralData.SetProperty("U_CodCOT", .CodigoClienteOT)
                If Not String.IsNullOrEmpty(.NombreClienteOT) Then oGeneralData.SetProperty("U_NCliOT", .NombreClienteOT)
                If .FechaCreacionOT IsNot Nothing Then oGeneralData.SetProperty("U_FApe", .FechaCreacionOT)
                ' If .HoraCreacionOT IsNot Nothing Then oGeneralData.SetProperty("U_HApe", .HoraCreacionOT)
                oGeneralData.SetProperty("U_HApe", Date.Now)
                If .FechaRecepcion IsNot Nothing Then oGeneralData.SetProperty("U_FRec", .FechaRecepcion)
                If .HoraRecepcion IsNot Nothing Then oGeneralData.SetProperty("U_HRec", .HoraRecepcion)
                If .FechaCompromiso IsNot Nothing Then oGeneralData.SetProperty("U_FCom", .FechaCompromiso)
                If .HoraCompromiso IsNot Nothing Then oGeneralData.SetProperty("U_HCom", .HoraCompromiso)
                If Not String.IsNullOrEmpty(.NoOTReferencia) Then oGeneralData.SetProperty("U_OTRef", .NoOTReferencia)
                If Not String.IsNullOrEmpty(.NivelGasolina) Then oGeneralData.SetProperty("U_NGas", .NivelGasolina.ToString())
                If .HorasServicio IsNot Nothing Then oGeneralData.SetProperty("U_HMot", Convert.ToInt32(.HorasServicio))
                oGeneralData.SetProperty("U_EstO", "1")
                oGeneralData.SetProperty("U_DEstO", My.Resources.Resource.EstadoOrdenNoIniciada)
                If Not String.IsNullOrEmpty(.Observaciones) Then oGeneralData.SetProperty("U_Obse", .Observaciones)
                If .CodigoAsesor > 0 Then
                    oGeneralData.SetProperty("U_Ase", .CodigoAsesor.ToString())
                End If
                If Not String.IsNullOrEmpty(.NoSerieCita) Then
                    oGeneralData.SetProperty("U_NoCita", String.Format("{0}-{1}", .NoSerieCita, .NoCita))
                End If
            End With
            oControlColaboradorLineas = oGeneralData.Child("SCGD_CTRLCOL")
            For Each rowControlColaborador As ControlColaborador In p_oControlColaboradorList
                oControlColaborador = oControlColaboradorLineas.Add()
                If Not String.IsNullOrEmpty(rowControlColaborador.IdActividad) Then oControlColaborador.SetProperty("U_IdAct", rowControlColaborador.IdActividad)
                If Not String.IsNullOrEmpty(rowControlColaborador.Colaborador) Then oControlColaborador.SetProperty("U_Colab", rowControlColaborador.Colaborador)
                oControlColaborador.SetProperty("U_Estad", "1")
                oControlColaborador.SetProperty("U_CodFas", "1")
                If Not String.IsNullOrEmpty(rowControlColaborador.FaseProduccion) Then oControlColaborador.SetProperty("U_NoFas", rowControlColaborador.FaseProduccion)
                oControlColaborador.SetProperty("U_CosEst", 0)

                ObtenerDatosCita(strDocEntryCotizacion, strHoraInicio, strFechaProduccion)
                If Not String.IsNullOrEmpty(strFechaProduccion) AndAlso Not String.IsNullOrEmpty(strHoraInicio) Then
                    oControlColaborador.SetProperty("U_FechPro", strFechaProduccion)
                    oControlColaborador.SetProperty("U_HoraIni", strHoraInicio)
                End If

            Next
            oGeneralService.Add(oGeneralData)
            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Consulta los datos de la cita perteneciente a la oferta de ventas indicada
    ''' </summary>
    ''' <param name="p_strDocEntryCotizacion">DocEntry de la oferta de ventas</param>
    ''' <param name="p_strHoraInicio">Hora de inicio de la actividad asignada a un mecánico</param>
    ''' <param name="p_strFechaProduccion">Fecha de la actividad asignada al mecánico</param>
    ''' <remarks></remarks>
    Public Sub ObtenerDatosCita(ByVal p_strDocEntryCotizacion As String, ByRef p_strHoraInicio As String, ByRef p_strFechaProduccion As String)

        Dim strConsulta As String = String.Empty
        Dim dtCita As System.Data.DataTable
        Dim dtmFechaProduccion As DateTime
        Dim tsHoraInicio As TimeSpan
        Dim strHoras As String = String.Empty
        Dim strMinutos As String = String.Empty
        Dim blnDatosInvalidos As String = False

        Try
            'Limpiamos las variables antes de utilizarlas
            p_strFechaProduccion = String.Empty
            p_strHoraInicio = String.Empty

            'Consulta la fecha del servicio y la hora desde la tabla de la cita
            strConsulta = DMS_Connector.Queries.GetStrSpecificQuery("strConsultaDatosCita")
            strConsulta = String.Format(strConsulta, p_strDocEntryCotizacion)

            dtCita = DMS_Connector.Helpers.EjecutarConsultaDataTable(strConsulta)

            If Not IsNothing(dtCita) Then
                If dtCita.Rows.Count > 0 Then
                    p_strHoraInicio = dtCita.Rows(0)("U_HoraServ")
                    p_strFechaProduccion = dtCita.Rows(0)("U_FhaServ")
                End If
            End If


            If p_strHoraInicio.Equals("0") Or String.IsNullOrEmpty(p_strHoraInicio) Then
                'Cero significa que es nulo, por lo que la hora no es válida
                p_strHoraInicio = String.Empty
            End If

            If p_strHoraInicio.Length >= 3 AndAlso Not p_strHoraInicio.Contains(":") Then
                p_strHoraInicio = p_strHoraInicio.Insert(p_strHoraInicio.Length - 2, ":")
            End If

            If DateTime.TryParse(p_strFechaProduccion, dtmFechaProduccion) AndAlso TimeSpan.TryParse(p_strHoraInicio, tsHoraInicio) Then

                If Not tsHoraInicio.Hours = 0 Then
                    p_strFechaProduccion = dtmFechaProduccion.ToString()

                    strHoras = tsHoraInicio.Hours.ToString()
                    strMinutos = tsHoraInicio.Minutes.ToString()

                    If strHoras.Length = 1 Then
                        strHoras = String.Format("0{0}", strHoras)
                    End If

                    If strMinutos.Equals("0") Then
                        strMinutos = "00"
                    End If

                    p_strHoraInicio = String.Format("{0}:{1}", strHoras, strMinutos)
                Else
                    blnDatosInvalidos = True
                End If
            Else
                blnDatosInvalidos = True
            End If

            'Si alguno de los datos es inválido, se devuelve en blanco
            If blnDatosInvalidos = True Then
                p_strFechaProduccion = String.Empty
                p_strHoraInicio = String.Empty
            End If

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function CrearControlColaborador(ByRef p_oControlColaboradorList As ControlColaborador_List, _
                                            ByRef p_oCotizacionActual As oDocumento) As Boolean
        Try
            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oChildOT As SAPbobsCOM.GeneralData
            Dim oChildrenOT As SAPbobsCOM.GeneralDataCollection
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim strHora As String
            Dim strMinutos As String

            oCompanyService = oCompany.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_OT")
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("Code", p_oCotizacionActual.NoOrden)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oChildrenOT = oGeneralData.Child("SCGD_CTRLCOL")

            For Each rowControlColaborador As ControlColaborador In p_oControlColaboradorList
                oChildOT = oChildrenOT.Add()
                If Not String.IsNullOrEmpty(rowControlColaborador.IdActividad) Then oChildOT.SetProperty("U_IdAct", rowControlColaborador.IdActividad)
                If Not String.IsNullOrEmpty(rowControlColaborador.Colaborador) Then oChildOT.SetProperty("U_Colab", rowControlColaborador.Colaborador)
                oChildOT.SetProperty("U_Estad", "1")
                oChildOT.SetProperty("U_CodFas", "1")
                If Not String.IsNullOrEmpty(rowControlColaborador.FaseProduccion) Then oChildOT.SetProperty("U_NoFas", rowControlColaborador.FaseProduccion)
                oChildOT.SetProperty("U_CosEst", 0)
            Next
            oGeneralService.Update(oGeneralData)
            Return True
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return False
        End Try
    End Function

    Private Function ObtenerCodeSiguienteOT() As String
        Try
            Dim strValor As String
            strValor = Utilitarios.EjecutarConsulta(" select MAX(DocEntry + 1) from [@SCGD_OT] with(nolock) ",
                                                        oCompany.CompanyDB, oCompany.Server)
            If String.IsNullOrEmpty(strValor) Then
                Return String.Empty
            Else
                Return strValor
            End If
        Catch ex As Exception
            oApplication.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return String.Empty
        End Try
    End Function
#End Region
End Class