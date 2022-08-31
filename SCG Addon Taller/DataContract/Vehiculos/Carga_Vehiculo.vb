Imports System.Collections.Generic

Public Class Carga_Vehiculo

    Public Shared Function Carga_Vehiculo(p_Company As SAPbobsCOM.Company, p_strCode As String) As Vehiculo

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try
            oCompanyService = p_Company.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")
            oGeneralParams = DirectCast(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams), SAPbobsCOM.GeneralDataParams)
            oGeneralParams.SetProperty("Code", p_strCode)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            Return Carga_VehiculoDT(oGeneralData)
        Catch ex As Exception
            Return Nothing

        Finally
            Utilitarios.DestruirObjeto(oCompanyService)
            Utilitarios.DestruirObjeto(oGeneralService)
            Utilitarios.DestruirObjeto(oGeneralData)
            Utilitarios.DestruirObjeto(oGeneralParams)
        End Try

    End Function

    Private Shared Function Carga_VehiculoDT(ByRef p_oGeneralData As SAPbobsCOM.GeneralData) As Vehiculo
        Dim vehiculo As Vehiculo

        Try
            vehiculo = New Vehiculo
            With vehiculo
                .Canceled = p_oGeneralData.GetProperty("Canceled")
                .Transfered = p_oGeneralData.GetProperty("Transfered")
                .DataSource = p_oGeneralData.GetProperty("DataSource")
                .U_TIPINV = p_oGeneralData.GetProperty("U_TIPINV")
                .U_FCHRES = p_oGeneralData.GetProperty("U_FCHRES")
                .U_FECFINR = p_oGeneralData.GetProperty("U_FECFINR")
                .UpdateDate = p_oGeneralData.GetProperty("UpdateDate")
                .U_FechaVen = p_oGeneralData.GetProperty("U_FechaVen")
                .CreateDate = p_oGeneralData.GetProperty("CreateDate")
                .U_FCHINV = p_oGeneralData.GetProperty("U_FCHINV")
                .U_FchUSv = p_oGeneralData.GetProperty("U_FchUSv")
                .U_FchPrSv = p_oGeneralData.GetProperty("U_FchPrSv")
                .U_FchRsva = p_oGeneralData.GetProperty("U_FchRsva")
                .U_FchVcRva = p_oGeneralData.GetProperty("U_FchVcRva")
                .U_Fha_Ing_Inv = p_oGeneralData.GetProperty("U_Fha_Ing_Inv")
                .U_GaranIni = p_oGeneralData.GetProperty("U_GaranIni")
                .U_GaranFin = p_oGeneralData.GetProperty("U_GaranFin")
                .U_HorSer = p_oGeneralData.GetProperty("U_HorSer")
                .U_NUMFAC = p_oGeneralData.GetProperty("U_NUMFAC")
                .DocEntry = p_oGeneralData.GetProperty("DocEntry")
                .U_FrecSvc = p_oGeneralData.GetProperty("U_FrecSvc")
                .LogInst = p_oGeneralData.GetProperty("LogInst")
                .UserSign = p_oGeneralData.GetProperty("UserSign")
                .U_Num_Cili = p_oGeneralData.GetProperty("U_Num_Cili")
                .U_CTOVTA = p_oGeneralData.GetProperty("U_CTOVTA")
                .U_GarantKM = p_oGeneralData.GetProperty("U_GarantKM")
                .U_Potencia = p_oGeneralData.GetProperty("U_Potencia")
                .U_Peso = p_oGeneralData.GetProperty("U_Peso")
                .U_Cilindra = p_oGeneralData.GetProperty("U_Cilindra")
                .U_Dispo = p_oGeneralData.GetProperty("U_Dispo")
                .U_SALINID = p_oGeneralData.GetProperty("U_SALINID")
                .U_SALINIC = p_oGeneralData.GetProperty("U_SALINIC")
                .U_FLELOC = p_oGeneralData.GetProperty("U_FLELOC")
                .U_TIPCAM = p_oGeneralData.GetProperty("U_TIPCAM")
                .U_COSINV = p_oGeneralData.GetProperty("U_COSINV")
                .U_VALHAC = p_oGeneralData.GetProperty("U_VALHAC")
                .U_GASTRA = p_oGeneralData.GetProperty("U_GASTRA")
                .U_VTADOL = p_oGeneralData.GetProperty("U_VTADOL")
                .U_VTACOL = p_oGeneralData.GetProperty("U_VTACOL")
                .U_ValorNet = p_oGeneralData.GetProperty("U_ValorNet")
                .U_Precio = p_oGeneralData.GetProperty("U_Precio")
                .U_CosPro = p_oGeneralData.GetProperty("U_CosPro")
                .U_TCRSalIni = p_oGeneralData.GetProperty("U_TCRSalIni")
                .U_Km_Unid = p_oGeneralData.GetProperty("U_Km_Unid")
                .U_Bono = p_oGeneralData.GetProperty("U_Bono")
                .U_Cod_Prov = p_oGeneralData.GetProperty("U_Cod_Prov")
                .U_Nom_Prov = p_oGeneralData.GetProperty("U_Nom_Prov")
                .U_ContratoV = p_oGeneralData.GetProperty("U_ContratoV")
                .U_DocPedido = p_oGeneralData.GetProperty("U_DocPedido")
                .U_Cod_Tec = p_oGeneralData.GetProperty("U_Cod_Tec")
                .U_Consig = p_oGeneralData.GetProperty("U_Consig")
                .U_Moneda = p_oGeneralData.GetProperty("U_Moneda")
                .U_DocRecepcion = p_oGeneralData.GetProperty("U_DocRecepcion")
                .U_Comentarios = p_oGeneralData.GetProperty("U_Comentarios")
                .U_CCar = p_oGeneralData.GetProperty("U_CCar")
                .U_Pote = p_oGeneralData.GetProperty("U_Pote")
                .U_DiEje = p_oGeneralData.GetProperty("U_DiEje")
                .U_Ramv = p_oGeneralData.GetProperty("U_Ramv")
                .U_Cant_Eje = p_oGeneralData.GetProperty("U_Cant_Eje")
                .U_NoPedFb = p_oGeneralData.GetProperty("U_NoPedFb")
                .U_ArtVentDesc = p_oGeneralData.GetProperty("U_ArtVentDesc")
                .U_Des_Col_Tap = p_oGeneralData.GetProperty("U_Des_Col_Tap")
                .U_Clasificacion = p_oGeneralData.GetProperty("U_Clasificacion")
                .U_Estado_Nuevo = p_oGeneralData.GetProperty("U_Estado_Nuevo")
                .U_fechaSync = p_oGeneralData.GetProperty("U_fechaSync")
                .U_ArtVent = p_oGeneralData.GetProperty("U_ArtVent")
                .U_Cli_Ven = p_oGeneralData.GetProperty("U_Cli_Ven")
                .U_Tipo_Reing = p_oGeneralData.GetProperty("U_Tipo_Reing")
                .U_ClNo_Ven = p_oGeneralData.GetProperty("U_ClNo_Ven")
                .U_CardCode = p_oGeneralData.GetProperty("U_CardCode")
                .U_CardName = p_oGeneralData.GetProperty("U_CardName")
                .U_Categori = p_oGeneralData.GetProperty("U_Categori")
                .U_Combusti = p_oGeneralData.GetProperty("U_Combusti")
                .U_Tip_Cabi = p_oGeneralData.GetProperty("U_Tip_Cabi")
                .U_Transmis = p_oGeneralData.GetProperty("U_Transmis")
                .U_Accesori = p_oGeneralData.GetProperty("U_Accesori")
                .U_VENRES = p_oGeneralData.GetProperty("U_VENRES")
                .U_Cod_Fab = p_oGeneralData.GetProperty("U_Cod_Fab")
                .U_Tipo_Ven = p_oGeneralData.GetProperty("U_Tipo_Ven")
                .U_OBSRES = p_oGeneralData.GetProperty("U_OBSRES")
                .U_ARREST = p_oGeneralData.GetProperty("U_ARREST")
                .U_TipTecho = p_oGeneralData.GetProperty("U_TipTecho")
                .U_Carrocer = p_oGeneralData.GetProperty("U_Carrocer")
                .U_Cod_Ubic = p_oGeneralData.GetProperty("U_Cod_Ubic")
                .U_Tipo = p_oGeneralData.GetProperty("U_Tipo")
                .U_Estatus = p_oGeneralData.GetProperty("U_Estatus")
                .U_Tipo_Tra = p_oGeneralData.GetProperty("U_Tipo_Tra")
                .U_Num_Plac = p_oGeneralData.GetProperty("U_Num_Plac")
                .U_Cod_Col = p_oGeneralData.GetProperty("U_Cod_Col")
                .U_Des_Col = p_oGeneralData.GetProperty("U_Des_Col")
                .U_ColorTap = p_oGeneralData.GetProperty("U_ColorTap")
                .U_Num_VIN = p_oGeneralData.GetProperty("U_Num_VIN")
                .U_Num_Mot = p_oGeneralData.GetProperty("U_Num_Mot")
                .U_MarcaMot = p_oGeneralData.GetProperty("U_MarcaMot")
                .Code = p_oGeneralData.GetProperty("Code")
                .Name = p_oGeneralData.GetProperty("Name")
                .U_Cod_Unid = p_oGeneralData.GetProperty("U_Cod_Unid")
                .U_Cod_Marc = p_oGeneralData.GetProperty("U_Cod_Marc")
                .U_Des_Marc = p_oGeneralData.GetProperty("U_Des_Marc")
                .U_Cod_Mode = p_oGeneralData.GetProperty("U_Cod_Mode")
                .U_Des_Mode = p_oGeneralData.GetProperty("U_Des_Mode")
                .U_Cod_Esti = p_oGeneralData.GetProperty("U_Cod_Esti")
                .U_Des_Esti = p_oGeneralData.GetProperty("U_Des_Esti")
                .U_Ano_Vehi = p_oGeneralData.GetProperty("U_Ano_Vehi")
                .CreateTime = p_oGeneralData.GetProperty("CreateTime")
                .U_Cant_Pas = p_oGeneralData.GetProperty("U_Cant_Pas")
                .U_CantPuer = p_oGeneralData.GetProperty("U_CantPuer")
                .UpdateTime = p_oGeneralData.GetProperty("UpdateTime")
                .U_GarantTM = p_oGeneralData.GetProperty("U_GarantTM")
                .AccesoriosxVehiculo = Carga_AccesoriosXVehiculo(p_oGeneralData.Child("SCGD_ACCXVEH"))
                .BonosXVehiculo = Carga_BonosXVehiculo(p_oGeneralData.Child("SCGD_BONOXVEH"))
                .TrazabilizadXVehiculo = Carga_TrazabilizadXVehiculo(p_oGeneralData.Child("SCGD_VEHITRAZA"))
            End With
            Return vehiculo
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function Carga_AccesoriosXVehiculo(ByRef p_oChildrenAccXVeh As SAPbobsCOM.GeneralDataCollection) As List(Of AccesoriosxVehiculo)
        Dim accesoriosxVehiculoList As List(Of AccesoriosxVehiculo)
        Dim oChildCc As SAPbobsCOM.GeneralData

        Try
            accesoriosxVehiculoList = New List(Of AccesoriosxVehiculo)()
            For index As Integer = 0 To p_oChildrenAccXVeh.Count - 1
                oChildCc = p_oChildrenAccXVeh.Item(index)
                With accesoriosxVehiculoList
                    .Add(New AccesoriosxVehiculo())
                    With .Item(index)
                        .LogInst = oChildCc.GetProperty("LogInst")
                        .LineId = oChildCc.GetProperty("LineId")
                        .Code = oChildCc.GetProperty("Code")
                        .U_Acc = oChildCc.GetProperty("U_Acc")
                        .U_N_Acc = oChildCc.GetProperty("U_N_Acc")
                        .U_Tipo = oChildCc.GetProperty("U_Tipo")
                    End With
                End With
            Next
            Return accesoriosxVehiculoList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function Carga_BonosXVehiculo(ByRef p_oChildrenBonosXVeh As SAPbobsCOM.GeneralDataCollection) As List(Of BonosXVehiculo)
        Dim bonosXVehiculoList As List(Of BonosXVehiculo)
        Dim oChildCc As SAPbobsCOM.GeneralData

        Try
            bonosXVehiculoList = New List(Of BonosXVehiculo)()
            For index As Integer = 0 To p_oChildrenBonosXVeh.Count - 1
                oChildCc = p_oChildrenBonosXVeh.Item(index)
                With bonosXVehiculoList
                    .Add(New BonosXVehiculo())
                    With .Item(index)
                        .LineId = oChildCc.GetProperty("LineId")
                        .LogInst = oChildCc.GetProperty("LogInst")
                        .U_Monto = oChildCc.GetProperty("U_Monto")
                        .Code = oChildCc.GetProperty("Code")
                        .U_Bono = oChildCc.GetProperty("U_Bono")
                    End With
                End With
            Next
            Return bonosXVehiculoList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function Carga_TrazabilizadXVehiculo(ByRef p_oChildrenTraXVeh As SAPbobsCOM.GeneralDataCollection) As List(Of TrazabilizadXVehiculo)
        Dim trazabilizadXVehiculoList As List(Of TrazabilizadXVehiculo)
        Dim oChildCc As SAPbobsCOM.GeneralData

        Try
            trazabilizadXVehiculoList = New List(Of TrazabilizadXVehiculo)()
            For index As Integer = 0 To p_oChildrenTraXVeh.Count - 1
                oChildCc = p_oChildrenTraXVeh.Item(index)
                With trazabilizadXVehiculoList
                    .Add(New TrazabilizadXVehiculo())
                    With .Item(index)
                        .U_FhaDoc_I = oChildCc.GetProperty("U_FhaDoc_I")
                        .U_FhaCV_I = oChildCc.GetProperty("U_FhaCV_I")
                        .U_FhaCV_V = oChildCc.GetProperty("U_FhaCV_V")
                        .U_FhaFac_V = oChildCc.GetProperty("U_FhaFac_V")
                        .U_FFCom = oChildCc.GetProperty("U_FFCom")
                        .U_FGuia = oChildCc.GetProperty("U_FGuia")
                        .U_FecEntCV = oChildCc.GetProperty("U_FecEntCV")
                        .LineId = oChildCc.GetProperty("LineId")
                        .LogInst = oChildCc.GetProperty("LogInst")
                        .U_TotDoc_I = oChildCc.GetProperty("U_TotDoc_I")
                        .U_TotCV_V = oChildCc.GetProperty("U_TotCV_V")
                        .U_ValVeh = oChildCc.GetProperty("U_ValVeh")
                        .U_Km_Ingreso = oChildCc.GetProperty("U_Km_Ingreso")
                        .U_Km_Venta = oChildCc.GetProperty("U_Km_Venta")
                        .Code = oChildCc.GetProperty("Code")
                        .U_NoGuia = oChildCc.GetProperty("U_NoGuia")
                        .U_NumCo = oChildCc.GetProperty("U_NumCo")
                        .U_Obs_I = oChildCc.GetProperty("U_Obs_I")
                        .U_NumCV_V = oChildCc.GetProperty("U_NumCV_V")
                        .U_CodCli_V = oChildCc.GetProperty("U_CodCli_V")
                        .U_CodVen_V = oChildCc.GetProperty("U_CodVen_V")
                        .U_NumFac_V = oChildCc.GetProperty("U_NumFac_V")
                        .U_Obs_V = oChildCc.GetProperty("U_Obs_V")
                        .U_Cod_Unid = oChildCc.GetProperty("U_Cod_Unid")
                        .U_NumDoc_I = oChildCc.GetProperty("U_NumDoc_I")
                        .U_CodVen_I = oChildCc.GetProperty("U_CodVen_I")
                        .U_NumCV_I = oChildCc.GetProperty("U_NumCV_I")
                    End With
                End With
            Next
            Return trazabilizadXVehiculoList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
