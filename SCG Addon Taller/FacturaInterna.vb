Imports System.Collections.Generic
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports DMSOneFramework.SCGDataAccess.DAConexion
Imports DMSOneFramework.SCGBusinessLogic
Imports DMSOneFramework.SCGCommon
Imports SAPbobsCOM
Imports SCG.DMSOne.Framework.MenuManager
Imports System.Linq
Imports SAPbouiCOM
Imports SCG.SBOFramework

Public Class FacturaInterna

#Region "Declaraciones"

    Private m_oCompany As SAPbobsCOM.Company

    Private Const mc_strFacturas As String = "@SCGD_FACTURAINTERNA"
    Private m_oFormGenCotizacion As SAPbouiCOM.Form
    Private WithEvents SBO_Application As SAPbouiCOM.Application
    Private Const mc_strUIDSubFacturas As String = "SCGD_FIN"
    Private Const mc_strUIDGeneraOV As String = "SCGD_GOV"
    Private blnPermisoReversar As Boolean

    Private Shared oTimer As System.Timers.Timer

    Private strIDSucursal As String = String.Empty
    Private strCodMarca As String = String.Empty
    Private strTipoOT As String = String.Empty
    Dim n As New Globalization.NumberFormatInfo
    Enum CentroCosto
        CostingCode
        CostingCode2
        CostingCode3
        CostingCode4
        CostingCode5
    End Enum

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

        Dim sPath As String

        sPath = System.Windows.Forms.Application.StartupPath

        If Utilitarios.MostrarMenu("SCGD_FIN", SBO_Application.Company.UserName) Then
            strEtiquetaMenu = Utilitarios.PermisosMenu("SCGD_FIN", SBO_Application.Language)

            GestorMenu.MenusManager.AddMenuEntry(New MenuEntry(mc_strUIDSubFacturas, SAPbouiCOM.BoMenuType.mt_STRING, strEtiquetaMenu, 15, False, True, mc_strUIDGeneraOV))

        End If

    End Sub

    Protected Friend Sub CargaFormulario()

        Try

            Dim fcp As SAPbouiCOM.FormCreationParams
            Dim strXMLACargar As String

            fcp = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)
            fcp.FormType = "SCGD_FAC_INT"

            strXMLACargar = My.Resources.Resource.FacturaInterna
            fcp.XmlData = CargarDesdeXML(strXMLACargar)

            m_oFormGenCotizacion = SBO_Application.Forms.AddEx(fcp)

            m_oFormGenCotizacion.PaneLevel = 1

            blnPermisoReversar = Utilitarios.MostrarMenu("SCGD_BFI", _SBO_Application.Company.UserName)
            m_oFormGenCotizacion.DataSources.DataTables.Add("dtLocal")
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select Code, Name from [@SCGD_MARCA]", "cboMarca")
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select Code, Name from [@SCGD_TIPO_ORDEN] Order by cast(code as int)", "cboTipo")
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select CurrCode, CurrName from OCRN", "cboMoneda")

            m_oFormGenCotizacion.EnableMenu("1282", False)
            Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, True)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    <System.CLSCompliant(False)> _
    Protected Friend Sub CargarValidValuesEnCombos(ByRef oForm As SAPbouiCOM.Form, _
                                                                ByVal strQuery As String, _
                                                                ByRef strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection
        Try
            oItem = oForm.Items.Item(strIDItem)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQuery
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If

            'Agrega los ValidValues
            'Do While drdResultadoConsulta.Read
            '    If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then

            '        cboCombo.ValidValues.Add(drdResultadoConsulta.Item(0).ToString.Trim, drdResultadoConsulta.Item(1).ToString.Trim)
            '    End If
            'Loop

            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    Dim strDscpModelo As String = drdResultadoConsulta.GetString(1).Trim
                    If strDscpModelo.Length > 60 Then
                        Dim strDescripcion As String = strDscpModelo.Substring(0, 60)
                        cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, strDescripcion)
                    Else
                        cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, strDscpModelo)
                    End If
                End If
            Loop


            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

    Private Sub LimpiarValidValuesCombo(ByRef oForm As SAPbouiCOM.Form, _
                                       ByVal p_strIDItem As String)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item

        oItem = oForm.Items.Item(p_strIDItem)
        cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

        If cboCombo.ValidValues.Count > 0 Then
            For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
            Next
        End If

    End Sub

    Public Sub CargarCombosEstiloyModelo(ByVal p_strFormID As String, ByVal p_strItemID As String)

        Dim strIDMarca As String = ""
        Dim strIDEstilo As String = ""
        Dim objCombo As SAPbouiCOM.ComboBox
        Dim blnCargarEstilos As Boolean
        Dim blnCargarModelos As Boolean

        m_oFormGenCotizacion = SBO_Application.Forms.Item(p_strFormID)

        Select Case p_strItemID
            Case "cboMarca"
                blnCargarEstilos = True
                blnCargarModelos = False

            Case "cboEstilo"
                blnCargarEstilos = False
                blnCargarModelos = True
            Case ""
                blnCargarEstilos = True
                blnCargarModelos = True
            Case Else
                blnCargarEstilos = False
                blnCargarModelos = False
        End Select

        If blnCargarEstilos Then
            LimpiarValidValuesCombo(m_oFormGenCotizacion, "cboModelo")
            LimpiarValidValuesCombo(m_oFormGenCotizacion, "cboEstilo")

            m_oFormGenCotizacion.Refresh()
            objCombo = DirectCast(m_oFormGenCotizacion.Items.Item("cboMarca").Specific, SAPbouiCOM.ComboBox)

            If objCombo.Selected IsNot Nothing Then
                strIDMarca = objCombo.Selected.Value
            End If
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select Code, Name from [@SCGD_ESTILO] where U_Cod_Marc = '" & strIDMarca & "'", "cboEstilo")
        End If
        If blnCargarModelos Then
            LimpiarValidValuesCombo(m_oFormGenCotizacion, "cboModelo")

            m_oFormGenCotizacion.Refresh()
            objCombo = DirectCast(m_oFormGenCotizacion.Items.Item("cboEstilo").Specific, SAPbouiCOM.ComboBox)

            If objCombo.Selected IsNot Nothing Then
                strIDEstilo = objCombo.Selected.Value
            End If
            Call CargarValidValuesEnCombos(m_oFormGenCotizacion, "Select Code, U_Descripcion from [@SCGD_MODELO] where U_Cod_Esti = '" & strIDEstilo & "'", "cboModelo")

        End If
    End Sub

    Private Function CargarDesdeXML(ByRef strFileName As String) As String

        Dim oXMLDoc As Xml.XmlDataDocument
        Dim strPath As String

        strPath = System.Windows.Forms.Application.StartupPath & "\" & strFileName
        oXMLDoc = New Xml.XmlDataDocument

        If Not oXMLDoc Is Nothing Then
            oXMLDoc.Load(strPath)
        End If
        Return oXMLDoc.InnerXml

    End Function

    Public Sub CargarFactura(ByVal p_strFacturaID As String)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim strIdVehiculo As String
        If m_oFormGenCotizacion IsNot Nothing Then


            strIdVehiculo = p_strFacturaID

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add

            oCondition.Alias = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strFacturaID

            Call m_oFormGenCotizacion.DataSources.DBDataSources.Item(mc_strFacturas).Query(oConditions)
            m_oFormGenCotizacion.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
            Utilitarios.FormularioDeshabilitado(m_oFormGenCotizacion, False)
            Utilitarios.FormularioSoloLectura(m_oFormGenCotizacion, False)

            ValidateStatus(m_oFormGenCotizacion)
            m_oFormGenCotizacion.Items.Item("txtObser").AffectsFormMode = True
            'LoadSalesOrder(m_oFormGenCotizacion)
        End If

    End Sub

    Public Function DevolverIDVehiculo(ByVal p_strIDForm As String) As String

        Dim oform As SAPbouiCOM.Form
        Dim strIDVehiculo As String

        oform = SBO_Application.Forms.Item(p_strIDForm)
        strIDVehiculo = oform.DataSources.DBDataSources.Item(mc_strFacturas).GetValue("U_ID_Vehi", 0)
        strIDVehiculo = strIDVehiculo.Trim
        Return strIDVehiculo

    End Function

    <System.CLSCompliant(False)> _
    Public Sub ImprimirReporteFacturaInterna(ByVal FormUID As String, _
                            ByRef pVal As SAPbouiCOM.ItemEvent, _
                            ByRef BubbleEvent As Boolean)

        Dim strDireccionReporte As String = String.Empty
        Dim strPathExe As String
        Dim strParametrosDocEntry As String
        Dim strParametrosNoOT As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim m_cn_Coneccion As New SqlClient.SqlConnection
        Dim m_strConectionString As String = String.Empty
        Dim objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
        Configuracion.CrearCadenaDeconexion(m_oCompany.Server, m_oCompany.CompanyDB, m_strConectionString)
        If m_cn_Coneccion.State = ConnectionState.Open Then
            m_cn_Coneccion.Close()
        End If
        m_cn_Coneccion.ConnectionString = m_strConectionString
        objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

        oForm = SBO_Application.Forms.Item(FormUID)

        strParametrosDocEntry = oForm.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("DocEntry", 0)

        strParametros = String.Format("{0}", strParametrosDocEntry)
        strParametros = strParametros.Replace(" ", "°")

        strDireccionReporte = objConfiguracionGeneral.DireccionReportes & My.Resources.Resource.rptFacturaInterna & ".rpt"

        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= My.Resources.Resource.TituloFacturaInterna.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & m_oCompany.Server & "," & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub


#End Region

#Region "Metodos Reversa"
    Private Sub CargarCotizacionDataContract(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                               ByVal p_intDocEntry As Integer, _
                                               ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, _
                                               ByRef p_oCotizacionList As Cotizacion_List, ByRef BubbleEvent As Boolean)
        Try
            Dim oCotizacionEncabezado As CotizacionEncabezado
            Dim oCotizacion As Cotizacion
            If p_intDocEntry > 0 Then
                p_oCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                If p_oCotizacion.GetByKey(p_intDocEntry) Then
                    '**********************************
                    'Carga Encabezado de la Cotizacion
                    '**********************************
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
                            .EstadoCotizacionID = "2"
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
                            .EstadoCotizacion = My.Resources.Resource.EstadoOrdenEnproceso
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
                                .Price = p_oCotizacion.Lines.UnitPrice
                                .TaxCode = p_oCotizacion.Lines.TaxCode
                                .VatGroup = p_oCotizacion.Lines.VatGroup
                                .FreeText = p_oCotizacion.Lines.FreeText
                                .Currency = p_oCotizacion.Lines.Currency
                                .DiscPrcnt = p_oCotizacion.Lines.DiscountPercent

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
                                If Not String.IsNullOrEmpty(p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()) Then
                                    .NombreEmpleado = p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()
                                End If
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
            BubbleEvent = False
            Throw ex
        End Try
    End Sub

    Private Sub CargarOrdenVentaDataContract(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                             ByRef p_oOrdenVenta As SAPbobsCOM.Documents, _
                                             ByVal p_intDocEntryCotizacion As Integer, _
                                             ByVal p_intDocEntryOrdenVenta As Integer, _
                                             ByVal p_intDocEntryFacturaInterna As Integer, _
                                               ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, _
                                               ByRef p_oCotizacionList As Cotizacion_List, ByRef BubbleEvent As Boolean)
        Try
            Dim oCotizacionEncabezado As CotizacionEncabezado
            Dim oCotizacion As Cotizacion
            Dim intDocEntryFacturaInterna As String = 0

            If p_intDocEntryCotizacion > 0 And p_intDocEntryOrdenVenta > 0 Then
                p_oCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                p_oOrdenVenta = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                If p_oCotizacion.GetByKey(p_intDocEntryCotizacion) And p_oOrdenVenta.GetByKey(p_intDocEntryOrdenVenta) Then

                    '**********************************
                    'Carga Encabezado de la Cotizacion
                    '**********************************
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
                            .EstadoCotizacionID = "2"
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
                            .EstadoCotizacion = My.Resources.Resource.EstadoOrdenEnproceso
                        End If

                    End With
                    p_oCotizacionEncabezadoList.Add(oCotizacionEncabezado)

                    For rowOrdenVenta As Integer = 0 To p_oOrdenVenta.Lines.Count - 1
                        p_oOrdenVenta.Lines.SetCurrentLine(rowOrdenVenta)

                        If p_oOrdenVenta.Lines.TreeType = BoItemTreeTypes.iIngredient OrElse p_oOrdenVenta.Lines.TreeType = BoItemTreeTypes.iNotATree Then
                            If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_NoFin").Value.ToString()) Then
                                intDocEntryFacturaInterna = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_NoFin").Value
                            Else
                                intDocEntryFacturaInterna = 0
                            End If
                            If intDocEntryFacturaInterna = p_intDocEntryFacturaInterna Then
                                oCotizacion = New Cotizacion()
                                With oCotizacion

                                    .ItemCode = p_oOrdenVenta.Lines.ItemCode
                                    .Description = p_oOrdenVenta.Lines.ItemDescription
                                    .Quantity = p_oOrdenVenta.Lines.Quantity
                                    .TreeType = p_oOrdenVenta.Lines.TreeType
                                    .Price = p_oOrdenVenta.Lines.UnitPrice
                                    .TaxCode = p_oOrdenVenta.Lines.TaxCode
                                    .VatGroup = p_oOrdenVenta.Lines.VatGroup
                                    .FreeText = p_oOrdenVenta.Lines.FreeText
                                    .Currency = p_oOrdenVenta.Lines.Currency
                                    .DiscPrcnt = p_oOrdenVenta.Lines.DiscountPercent

                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value.ToString()) Then
                                        .IdRepxOrd = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_IdRepxOrd").Value
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_ID").Value.ToString()) Then
                                        .ID = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                                    End If
                                    .Aprobado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Aprobado").Value
                                    .Trasladado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Traslad").Value
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value) Then
                                        .OTHija = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_OTHija").Value
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) Then
                                        .DuracionEstandar = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value
                                    Else
                                        .DuracionEstandar = 0
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                        .EmpleadoAsignado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()) Then
                                        .NombreEmpleado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                        .EstadoActividad = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value.ToString.Trim()) Then
                                        .CantidadRecibida = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CRec").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString.Trim()) Then
                                        .CantidadSolicitada = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CSol").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString.Trim()) Then
                                        .CantidadPendiente = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPen").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value.ToString.Trim()) Then
                                        .CantidadPendienteBodega = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPBo").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value.ToString.Trim()) Then
                                        .CantidadPendienteTraslado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPTr").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value.ToString.Trim()) Then
                                        .CantidadPendienteDevolucion = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CPDe").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()) Then
                                        .Costo = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()) Then
                                        .NoOrden = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value.ToString.Trim()) Then
                                        .Entregado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Entregado").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()) Then
                                        .TipoArticulo = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value.ToString.Trim()) Then
                                        .Comprar = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Comprar").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim()) Then
                                        .Sucursal = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Sucur").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()) Then
                                        .CentroCosto = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_CtrCos").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()) Then
                                        .TipoOT = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_TipoOT").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value.ToString.Trim()) Then
                                        .ProcesarInteger = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Procesar").Value
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()) Then
                                        .EstadoActividad = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()) Then
                                        .EmpleadoAsignado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()) Then
                                        .NombreEmpleado = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value.ToString.Trim()
                                    End If
                                    If Not String.IsNullOrEmpty(p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()) Then
                                        .CostoEstandar = p_oOrdenVenta.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value.ToString.Trim()
                                    End If
                                End With
                                p_oCotizacionList.Add(oCotizacion)
                                p_oOrdenVenta.Lines.LineStatus = BoStatus.bost_Close
                            End If


                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        End Try
    End Sub

    Private Sub CargarCotizacionObjeto(ByRef p_oCotizacion As SAPbobsCOM.Documents, _
                                              ByRef p_oCotizacionEncabezadoList As CotizacionEncabezado_List, _
                                              ByRef p_oCotizacionList As Cotizacion_List, ByRef BubbleEvent As Boolean)
        Try

            p_oCotizacion = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            '**********************************
            'Carga Encabezado de la Cotizacion
            '**********************************

            With p_oCotizacionEncabezadoList.Item(0)

                If Not String.IsNullOrEmpty(.NoOrden) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .NoOrden
                End If
                If Not String.IsNullOrEmpty(.Sucursal) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = .Sucursal
                    strIDSucursal = .Sucursal
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
                    strTipoOT = .TipoOT
                End If
                If Not String.IsNullOrEmpty(.CodigoProyecto) Then
                    p_oCotizacion.UserFields.Fields.Item("U_SCGD_Proyec").Value = .CodigoProyecto
                End If

                p_oCotizacion.CardCode = .CardCode
                p_oCotizacion.CardName = .CardName
                p_oCotizacion.DocCurrency = .DocCurrency

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
                    p_oCotizacion.Lines.DiscountPercent = .DiscPrcnt

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
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Emp_Realiza").Value = .NombreEmpleado
                    End If
                    If Not String.IsNullOrEmpty(.CostoEstandar) Then
                        p_oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_Costo").Value = .CostoEstandar
                    End If
                End With
                p_oCotizacion.Lines.Add()
            Next
            p_oCotizacionList.Clear()

        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub CargarSalidaMercanciaDataContract(ByRef p_oSalida As SAPbobsCOM.Documents, ByVal p_intDocEntry As Integer,
                                                   ByRef BubbleEvent As Boolean)
        Try
            'Dim oSalidaMercanciaEncabezado As SalidaMercanciaEncabezado
            'Dim oSalidaMercanciaLineas As SalidaMercanciaLineas

            If p_oSalida.GetByKey(p_intDocEntry) Then

                '**********************************
                'Carga Encabezado de la Salida
                '**********************************
                'oSalidaMercanciaEncabezado = New SalidaMercanciaEncabezado()
                'With oSalidaMercanciaEncabezado

                '    If Not String.IsNullOrEmpty(p_oSalida.DocCurrency) Then
                '        .DocCurrency = p_oSalida.DocCurrency
                '    End If
                '    If Not String.IsNullOrEmpty(p_oSalida.Project) Then
                '        .Proyecto = p_oSalida.Project
                '    End If
                '    If Not String.IsNullOrEmpty(p_oSalida.Reference2) Then
                '        .Reference2 = p_oSalida.Reference2
                '    End If
                'If Not String.IsNullOrEmpty(p_oSalida.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                '.CodUnidad = p_oSalida.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                '''Borra el código de unidad para que el monto no pueda costearsele al vehículo
                '    p_oSalida.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = ""
                'End If
                'If Not String.IsNullOrEmpty(p_oSalida.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                '    .NumeroOT = p_oSalida.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                'End If
                'If Not String.IsNullOrEmpty(p_oSalida.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value) Then
                '    .NumVehiculo = p_oSalida.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value
                'End If
                'If Not String.IsNullOrEmpty(p_oSalida.UserFields.Fields.Item("U_SCGD_Procesad").Value) Then
                '    .Procesado = p_oSalida.UserFields.Fields.Item("U_SCGD_Procesad").Value
                'End If
                'End With

                'p_oSalidaMercanciaEncabezadoList.Add(oSalidaMercanciaEncabezado)

                'For rowCotizacion As Integer = 0 To p_oSalida.Lines.Count - 1
                '    p_oSalida.Lines.SetCurrentLine(rowCotizacion)
                '    oSalidaMercanciaLineas = New SalidaMercanciaLineas()

                '    With oSalidaMercanciaLineas
                '        .ItemCode = p_oSalida.Lines.ItemCode
                '        .WarehouseCode = p_oSalida.Lines.WarehouseCode
                '        .Quantity = p_oSalida.Lines.Quantity
                '        .AccountCode = p_oSalida.Lines.AccountCode
                '        .ProjectCode = p_oSalida.Lines.ProjectCode
                '        If Not String.IsNullOrEmpty(p_oSalida.Lines.CostingCode) Then
                '            .CostingCode = p_oSalida.Lines.CostingCode
                '        End If
                '        If Not String.IsNullOrEmpty(p_oSalida.Lines.CostingCode2) Then
                '            .CostingCode2 = p_oSalida.Lines.CostingCode2
                '        End If
                '        If Not String.IsNullOrEmpty(p_oSalida.Lines.CostingCode3) Then
                '            .CostingCode3 = p_oSalida.Lines.CostingCode3
                '        End If
                '        If Not String.IsNullOrEmpty(p_oSalida.Lines.CostingCode4) Then
                '            .CostingCode4 = p_oSalida.Lines.CostingCode4
                '        End If
                '        If Not String.IsNullOrEmpty(p_oSalida.Lines.CostingCode5) Then
                '            .CostingCode5 = p_oSalida.Lines.CostingCode5
                '        End If

                '    End With
                '    p_oSalidaMercanciaLineasList.Add(oSalidaMercanciaLineas)
                'Next
            End If
        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        End Try
    End Sub

    Private Sub CargarEntradaMercanciaObjeto(ByRef p_oEntrada As SAPbobsCOM.Documents, _
                                            ByRef p_oSalida As SAPbobsCOM.Documents, _
                                            ByRef BubbleEvent As Boolean, ByRef p_dtDate As Date)

        Dim strValorCentroCosto As String = String.Empty
        Dim price As Double = 0
        Try

            '**********************************
            'Carga Encabezado de la Entrada
            '**********************************
            With p_oSalida

                If Not String.IsNullOrEmpty(.DocCurrency) Then
                    p_oEntrada.DocCurrency = .DocCurrency
                End If
                If Not String.IsNullOrEmpty(.Project) Then
                    p_oEntrada.Project = .Project
                End If
                If Not String.IsNullOrEmpty(.Reference2) Then
                    p_oEntrada.Reference2 = .Reference2
                End If
                If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value) Then
                    p_oEntrada.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = .UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                    '''Borra el código de unidad para que el monto no pueda costearsele al vehículo
                    p_oSalida.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = ""
                End If
                If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Numero_OT").Value) Then
                    p_oEntrada.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = .UserFields.Fields.Item("U_SCGD_Numero_OT").Value
                End If
                If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value) Then
                    p_oEntrada.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = .UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value
                End If
                If Not String.IsNullOrEmpty(.UserFields.Fields.Item("U_SCGD_Procesad").Value) Then
                    p_oEntrada.UserFields.Fields.Item("U_SCGD_Procesad").Value = .UserFields.Fields.Item("U_SCGD_Procesad").Value
                End If
            End With
            p_oEntrada.DocDate = p_dtDate

            For rowCotizacion As Integer = 0 To p_oSalida.Lines.Count - 1
                p_oSalida.Lines.SetCurrentLine(rowCotizacion)

                With p_oSalida.Lines
                    p_oEntrada.Lines.ItemCode = .ItemCode
                    p_oEntrada.Lines.WarehouseCode = .WarehouseCode
                    p_oEntrada.Lines.Quantity = .Quantity
                    p_oEntrada.Lines.AccountCode = .AccountCode
                    p_oEntrada.Lines.ProjectCode = .ProjectCode

                    'p_oEntrada.Lines.Currency = .Currency

                    '.UserFields.Fields.Item("U_SCGD_Costo").Value

                    Dim dblCosto As Double

                    'Determina si se debe utilizar el mismo precio de la salida o dejar el precio en blanco
                    'para permitir que SAP asigne el precio de acuerdo a sus configuraciones
                    If Not DMS_Connector.Configuracion.ParamGenAddon.U_UsaPrecioSalida = "N" Then
                        price = ConsultaStockPrice(.DocEntry, .ItemCode)
                        If price > 0 Then
                            p_oEntrada.Lines.UnitPrice = price
                        End If
                    Else
                        dblCosto = ObtenerCostoArticulo(.ItemCode, price)
                        p_oEntrada.Lines.UnitPrice = dblCosto
                    End If

                    '*** Centros de costo ******
                    If Not String.IsNullOrEmpty(.CostingCode) Then
                        p_oEntrada.Lines.CostingCode = .CostingCode
                    Else
                        AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode)
                        If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                            p_oEntrada.Lines.CostingCode = strValorCentroCosto
                        End If
                    End If
                    If Not String.IsNullOrEmpty(.CostingCode2) Then
                        p_oEntrada.Lines.CostingCode2 = .CostingCode2
                    Else
                        AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode2)
                        If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                            p_oEntrada.Lines.CostingCode2 = strValorCentroCosto
                        End If
                    End If
                    If Not String.IsNullOrEmpty(.CostingCode3) Then
                        p_oEntrada.Lines.CostingCode3 = .CostingCode3
                    Else
                        AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode3)
                        If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                            p_oEntrada.Lines.CostingCode3 = strValorCentroCosto
                        End If
                    End If
                    If Not String.IsNullOrEmpty(.CostingCode4) Then
                        p_oEntrada.Lines.CostingCode4 = .CostingCode4
                    Else
                        AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode4)
                        If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                            p_oEntrada.Lines.CostingCode4 = strValorCentroCosto
                        End If
                    End If
                    If Not String.IsNullOrEmpty(.CostingCode5) Then
                        p_oEntrada.Lines.CostingCode5 = .CostingCode5
                    Else
                        AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode5)
                        If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                            p_oEntrada.Lines.CostingCode5 = strValorCentroCosto
                        End If
                    End If
                    p_oEntrada.Lines.Add()
                End With
            Next
        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el costo del artículo para la reversión de salida de mercancías
    ''' </summary>
    ''' <param name="strItemCode">ItemCode en formato texto</param>
    ''' <returns>Costo estándar</returns>
    ''' <remarks></remarks>
    Private Function ObtenerCostoArticulo(ByVal strItemCode As String, ByVal dblPrice As Double) As Double
        Dim oItem As SAPbobsCOM.Items
        Dim dblCosto As Double = 0

        Try
            oItem = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
            If (oItem.GetByKey(strItemCode)) Then
                Select Case oItem.CostAccountingMethod
                    Case BoInventorySystem.bis_FIFO
                        dblCosto = dblPrice
                    Case BoInventorySystem.bis_MovingAverage
                        dblCosto = oItem.MovingAveragePrice
                    Case BoInventorySystem.bis_SNB
                        dblCosto = dblPrice
                    Case BoInventorySystem.bis_Standard
                        dblCosto = oItem.AvgStdPrice
                End Select
            End If
            Return dblCosto
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Sub LimpiarDocumentoOrigen(ByRef p_oDocument As SAPbobsCOM.Documents, ByRef p_oCotizacionList As Cotizacion_List, ByRef BubbleEvent As Boolean)
        Try
            Dim strID As String = String.Empty
            p_oDocument.UserFields.Fields.Item("U_SCGD_NoOtRef").Value = p_oDocument.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
            p_oDocument.UserFields.Fields.Item("U_SCGD_Numero_OT").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_OT_Padre").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_No_Visita").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_NoCita").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Estado_Cot").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Fech_Recep").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_idSucursal").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Tipo_OT").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_CCliOT").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_NCliOT").Value = ""
            p_oDocument.UserFields.Fields.Item("U_SCGD_Estado_CotID").Value = ""
            With p_oDocument
                For rowCotizacion As Integer = 0 To p_oCotizacionList.Count - 1
                    With p_oCotizacionList.Item(rowCotizacion)
                        For index As Integer = 0 To p_oDocument.Lines.Count - 1
                            p_oDocument.Lines.SetCurrentLine(index)
                            strID = p_oDocument.Lines.UserFields.Fields.Item("U_SCGD_ID").Value
                            If .ID = strID Then
                                p_oDocument.Lines.UserFields.Fields.Item("U_SCGD_NoOT").Value = ""
                                Exit For
                            End If
                        Next
                    End With
                Next
            End With
        Catch ex As Exception
            BubbleEvent = False
            Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try
    End Sub

    Private Sub ValidateStatus(ByVal oForm As SAPbouiCOM.Form)
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim blnVisible As Boolean

        Try
            oForm.Freeze(True)
            oCombo = CType(oForm.Items.Item("cboReversa").Specific, SAPbouiCOM.ComboBox)
            If (String.IsNullOrEmpty(oCombo.Value.Trim) OrElse oCombo.Value.Trim = "N") AndAlso blnPermisoReversar Then
                blnVisible = SAPbouiCOM.BoModeVisualBehavior.mvb_False
            Else
                blnVisible = SAPbouiCOM.BoModeVisualBehavior.mvb_True
            End If
            oItem = oForm.Items.Item("btnReversa")
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, Not blnVisible)
            oItem = oForm.Items.Item("btnPrint")
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, Not blnVisible)
            oItem = oForm.Items.Item("txtFeRe")
            oItem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, Not blnVisible)

        Catch ex As Exception
            Throw ex
        Finally
            oForm.Freeze(False)
        End Try
    End Sub

    Private Sub ReversarAsiento(ByRef oJournalEntrySalida As SAPbobsCOM.JournalEntries, ByVal intDocEntry As Integer, ByRef BubbleEvent As Boolean, ByRef p_dtDate As Date)
        Dim oJournalEntryOrigen As SAPbobsCOM.JournalEntries
        Dim strValorCentroCosto As String = String.Empty
        Try
            oJournalEntryOrigen = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            If oJournalEntryOrigen.GetByKey(intDocEntry) Then
                oJournalEntrySalida.ReferenceDate = p_dtDate
                oJournalEntrySalida.Memo = String.Format("Rev: {0}", oJournalEntryOrigen.Memo)

                For i As Integer = 0 To oJournalEntryOrigen.Lines.Count - 1
                    oJournalEntryOrigen.Lines.SetCurrentLine(i)

                    With oJournalEntryOrigen.Lines
                        oJournalEntrySalida.Lines.ShortName = .ShortName
                        oJournalEntrySalida.Lines.AccountCode = .AccountCode
                        oJournalEntrySalida.Lines.Debit = .Credit
                        oJournalEntrySalida.Lines.FCDebit = .FCCredit
                        oJournalEntrySalida.Lines.Credit = .Debit
                        oJournalEntrySalida.Lines.FCCredit = .FCDebit

                        oJournalEntrySalida.Lines.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = .UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value
                        oJournalEntrySalida.Lines.UserFields.Fields.Item("U_SCGD_Cod_Tran").Value = .UserFields.Fields.Item("U_SCGD_Cod_Tran").Value

                        If Not String.IsNullOrEmpty(.FCCurrency) Then
                            oJournalEntrySalida.Lines.FCCurrency = .FCCurrency
                        End If
                        '*** Centros de costo ******
                        If Not String.IsNullOrEmpty(.CostingCode) Then
                            oJournalEntrySalida.Lines.CostingCode = .CostingCode
                        Else
                            AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode)
                            If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                                oJournalEntrySalida.Lines.CostingCode = strValorCentroCosto
                            End If
                        End If
                        If Not String.IsNullOrEmpty(.CostingCode2) Then
                            oJournalEntrySalida.Lines.CostingCode2 = .CostingCode2
                        Else
                            AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode2)
                            If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                                oJournalEntrySalida.Lines.CostingCode2 = strValorCentroCosto
                            End If
                        End If
                        If Not String.IsNullOrEmpty(.CostingCode3) Then
                            oJournalEntrySalida.Lines.CostingCode3 = .CostingCode3
                        Else
                            AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode3)
                            If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                                oJournalEntrySalida.Lines.CostingCode3 = strValorCentroCosto
                            End If
                        End If
                        If Not String.IsNullOrEmpty(.CostingCode4) Then
                            oJournalEntrySalida.Lines.CostingCode4 = .CostingCode4
                        Else
                            AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode4)
                            If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                                oJournalEntrySalida.Lines.CostingCode4 = strValorCentroCosto
                            End If
                        End If
                        If Not String.IsNullOrEmpty(.CostingCode5) Then
                            oJournalEntrySalida.Lines.CostingCode5 = .CostingCode5
                        Else
                            AsignaDimensionesContables(strValorCentroCosto, CentroCosto.CostingCode5)
                            If Not String.IsNullOrEmpty(strValorCentroCosto) Then
                                oJournalEntrySalida.Lines.CostingCode5 = strValorCentroCosto
                            End If
                        End If

                        oJournalEntrySalida.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
                        oJournalEntrySalida.Lines.Add()
                    End With
                Next
            Else
                BubbleEvent = False
            End If
        Catch ex As Exception
            BubbleEvent = False
            Throw ex
        Finally
            Utilitarios.DestruirObjeto(oJournalEntryOrigen)
        End Try
    End Sub

    Private Sub ActualizarFacturaInterna(ByRef oGeneralService As SAPbobsCOM.GeneralService, ByRef oGeneralData As SAPbobsCOM.GeneralData, ByVal intDocEntry As Integer, ByVal listDocs As List(Of Integer), ByRef p_dtDate As Date)
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try
            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", intDocEntry)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oGeneralData.SetProperty("U_Reversado", "Y")
            oGeneralData.SetProperty("U_EntradaMercancia", listDocs.Item(0))
            oGeneralData.SetProperty("U_AsientoR_OG", listDocs.Item(1))
            oGeneralData.SetProperty("U_AsientoR_MO", listDocs.Item(2))
            oGeneralData.SetProperty("U_AsientoR_SE", listDocs.Item(3))
            oGeneralData.SetProperty("U_Fecha_Reversion", p_dtDate)
        Catch ex As Exception
            Throw ex
        Finally
            Utilitarios.DestruirObjeto(oGeneralParams)
        End Try
    End Sub

    Private Function GuardarDatosDB(ByRef p_oCotizacion As SAPbobsCOM.Documents, ByRef p_oCotizacionNueva As SAPbobsCOM.Documents, ByRef p_oPedido As SAPbobsCOM.Documents, ByRef p_oEntradaMercancia As SAPbobsCOM.Documents, ByRef p_oSalidaMercancia As SAPbobsCOM.Documents, ByRef p_oGeneralServiceFI As SAPbobsCOM.GeneralService, ByRef p_oGeneralDataFI As SAPbobsCOM.GeneralData, ByRef p_oJournalEntryOG As SAPbobsCOM.JournalEntries, ByRef p_oJournalEntryMO As SAPbobsCOM.JournalEntries, ByRef p_oJournalEntrySE As SAPbobsCOM.JournalEntries, ByVal p_intDocEntryFI As Integer, ByRef p_oGeneralServiceOT As SAPbobsCOM.GeneralService, ByRef p_oGeneralDataOT As SAPbobsCOM.GeneralData, ByRef p_dtDate As Date, ByRef pVal As SAPbouiCOM.ItemEvent) As Boolean
        Dim listDocEntry As List(Of Integer)
        Dim strError As String
        Dim intError As Integer

        Try
            listDocEntry = New List(Of Integer)()

            If Not m_oCompany.InTransaction() Then
                m_oCompany.StartTransaction()
            Else
                Throw New System.Exception(String.Format("{0}", My.Resources.Resource.ErrorTransaccion))
            End If

            If p_oCotizacion.Update() = 0 Then
                If p_oCotizacionNueva.Add() = 0 Then
                    ActualizarOT(p_oGeneralServiceOT, p_oGeneralDataOT, SBO_Application.Forms.Item(pVal.FormUID), m_oCompany.GetNewObjectKey, p_oCotizacionNueva.UserFields.Fields.Item("U_SCGD_idSucursal").Value)
                    If Not p_oPedido Is Nothing Then
                        If Not p_oPedido.Update() = 0 Then
                            m_oCompany.GetLastError(intError, strError)
                            Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                        End If
                    End If
                    If Not p_oEntradaMercancia Is Nothing Then
                        If p_oEntradaMercancia.Add() = 0 Then
                            listDocEntry.Add(CInt(m_oCompany.GetNewObjectKey()))
                        Else
                            m_oCompany.GetLastError(intError, strError)
                            Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                        End If
                    Else
                        listDocEntry.Add(0)
                    End If

                    '''Actualiza la Salida de Mercancía borrandole el código de unidad para que el monto no aparesca como costo pendiente del vehiculo
                    If Not p_oSalidaMercancia Is Nothing Then
                        If p_oSalidaMercancia.Update() <> 0 Then
                            m_oCompany.GetLastError(intError, strError)
                            Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                        End If
                    End If

                    If Not p_oJournalEntryOG Is Nothing Then
                        If p_oJournalEntryOG.Add() = 0 Then
                            listDocEntry.Add(CInt(m_oCompany.GetNewObjectKey()))
                        Else
                            m_oCompany.GetLastError(intError, strError)
                            Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                        End If
                    Else
                        listDocEntry.Add(0)
                    End If
                    If Not p_oJournalEntryMO Is Nothing Then
                        If p_oJournalEntryMO.Add() = 0 Then
                            listDocEntry.Add(CInt(m_oCompany.GetNewObjectKey()))
                        Else
                            m_oCompany.GetLastError(intError, strError)
                            Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                        End If
                    Else
                        listDocEntry.Add(0)
                    End If
                    If Not p_oJournalEntrySE Is Nothing Then
                        If p_oJournalEntrySE.Add() = 0 Then
                            listDocEntry.Add(CInt(m_oCompany.GetNewObjectKey()))
                        Else
                            m_oCompany.GetLastError(intError, strError)
                            Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                        End If
                    Else
                        listDocEntry.Add(0)
                    End If

                    ActualizarFacturaInterna(p_oGeneralServiceFI, p_oGeneralDataFI, p_intDocEntryFI, listDocEntry, p_dtDate)
                    p_oGeneralServiceFI.Update(p_oGeneralDataFI)
                    If Not p_oGeneralServiceOT Is Nothing Then
                        p_oGeneralServiceOT.Update(p_oGeneralDataOT)
                    End If

                    If m_oCompany.InTransaction() Then
                        m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        Return True
                    Else
                        Throw New System.Exception(String.Format("{0}", My.Resources.Resource.ErrorTransaccion))
                    End If
                Else
                    m_oCompany.GetLastError(intError, strError)
                    Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
                End If
            Else
                m_oCompany.GetLastError(intError, strError)
                Throw New System.Exception(String.Format("{0}: {1}", intError, strError))
            End If
        Catch ex As Exception
            If m_oCompany.InTransaction() Then
                m_oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
            Throw ex
        End Try
    End Function

    Private Sub RecargarFactura(ByVal p_intDocEntryFI As Integer, ByVal p_oform As SAPbouiCOM.Form)
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
        oCondition = oConditions.Add

        oCondition.Alias = "DocEntry"
        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
        oCondition.CondVal = p_intDocEntryFI

        p_oform.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").Query(oConditions)
    End Sub

    Private Sub ReversarFI(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCotizacionEncabezadoList As CotizacionEncabezado_List
        Dim oCotizacionList As Cotizacion_List
        Dim oCotizacion, oCotizacionNueva, oPedido, oSalidaMercancia, oEntradaMercancia As SAPbobsCOM.Documents
        Dim oJournalEntryOG, oJournalEntryMO, oJournalEntrySE As SAPbobsCOM.JournalEntries
        Dim intDocEntryOT, intDocEntryAsOG, intDocEntryAsMO, intDocEntryAsSE, intDocEntryFI As Integer
        Dim strDocEntrySM, strDocEntryOV As String
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralServiceFI, oGeneralServiceOT As SAPbobsCOM.GeneralService
        Dim oGeneralDataFI, oGeneralDataOT As SAPbobsCOM.GeneralData
        Dim dtDate As Date
        Dim intDocEntryOrdenVenta As Integer = 0
        Dim oOrdenVenta As SAPbobsCOM.Documents
        Try
            InicializarTimer()

            If pVal.BeforeAction Then
                If String.IsNullOrEmpty(SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_Fecha_Reversion", 0)) Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.FaltaFechaReversionFI, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    BubbleEvent = False
                    Exit Sub
                ElseIf SBO_Application.Forms.Item(pVal.FormUID).Mode <> SAPbouiCOM.BoFormMode.fm_OK_MODE OrElse SBO_Application.MessageBox(My.Resources.Resource.ReversarFI, 2, My.Resources.Resource.Si, My.Resources.Resource.No) <> 1 Then
                    BubbleEvent = False
                    Exit Sub
                Else
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ValidaReversionFI, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                End If
            ElseIf BubbleEvent And pVal.ActionSuccess Then
                dtDate = Date.ParseExact(SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_Fecha_Reversion", 0), "yyyyMMdd", Nothing)
                intDocEntryOT = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_Cot", 0)
                If intDocEntryOT < 0 Then
                    BubbleEvent = False
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.NoExisteCotizacionFI, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Exit Sub
                End If
                intDocEntryFI = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("DocEntry", 0)
                intDocEntryOT = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_Cot", 0)
                strDocEntryOV = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OV", 0).Trim
                strDocEntrySM = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_Sal", 0).Trim
                If Not String.IsNullOrEmpty(SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_AsientoGastos", 0)) Then
                    intDocEntryAsOG = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_AsientoGastos", 0)
                Else
                    intDocEntryAsOG = 0
                End If
                intDocEntryAsMO = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_Asiento", 0)
                intDocEntryAsSE = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_Asien_SE", 0)
                If Not String.IsNullOrEmpty(SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OV", 0)) Then
                    intDocEntryOrdenVenta = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OV", 0)
                End If

                If Not String.IsNullOrEmpty(SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OV", 0)) Then

                    intDocEntryOrdenVenta = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OV", 0)
                End If

                If Not String.IsNullOrEmpty(SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_Cod_Marc", 0)) Then
                    strCodMarca = SBO_Application.Forms.Item(pVal.FormUID).DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_Cod_Marc", 0).ToString()
                End If

                oCotizacionEncabezadoList = New CotizacionEncabezado_List()
                oCotizacionList = New Cotizacion_List()

                If intDocEntryOrdenVenta > 0 Then
                    CargarOrdenVentaDataContract(oCotizacion, oOrdenVenta, intDocEntryOT, intDocEntryOrdenVenta, intDocEntryFI, oCotizacionEncabezadoList, oCotizacionList, BubbleEvent)
                Else
                    CargarCotizacionDataContract(oCotizacion, intDocEntryOT, oCotizacionEncabezadoList, oCotizacionList, BubbleEvent)
                End If
                If BubbleEvent Then CargarCotizacionObjeto(oCotizacionNueva, oCotizacionEncabezadoList, oCotizacionList, BubbleEvent)
                If BubbleEvent Then LimpiarDocumentoOrigen(oCotizacion, oCotizacionList, BubbleEvent)
                'If intDocEntryOrdenVenta > 0 Then
                '    If BubbleEvent Then LimpiarDocumentoOrigen(oOrdenVenta, oCotizacionList, BubbleEvent)
                'End If
                If intDocEntryOrdenVenta > 0 Then
                    If BubbleEvent AndAlso Not String.IsNullOrEmpty(strDocEntryOV) AndAlso strDocEntryOV <> "0" Then
                        If BubbleEvent Then LimpiarDocumentoOrigen(oOrdenVenta, oCotizacionList, BubbleEvent)
                    End If
                End If

                'If BubbleEvent AndAlso Not String.IsNullOrEmpty(strDocEntryOV) AndAlso strDocEntryOV <> "0" Then
                '    oPedido = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                '    If oPedido.GetByKey(CInt(strDocEntryOV)) Then
                '        LimpiarDocumentoOrigen(oPedido, BubbleEvent)
                '    Else
                '        BubbleEvent = False
                '        SBO_Application.StatusBar.SetText(My.Resources.Resource.NoExistePedidoFI, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                '    End If
                'Else
                '    oPedido = Nothing
                'End If
                If BubbleEvent AndAlso intDocEntryAsOG > 0 Then
                    oJournalEntryOG = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    ReversarAsiento(oJournalEntryOG, intDocEntryAsOG, BubbleEvent, dtDate)
                Else
                    oJournalEntryOG = Nothing
                End If

                If BubbleEvent AndAlso intDocEntryAsMO > 0 Then
                    oJournalEntryMO = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    ReversarAsiento(oJournalEntryMO, intDocEntryAsMO, BubbleEvent, dtDate)
                Else
                    oJournalEntryMO = Nothing
                End If

                If BubbleEvent AndAlso intDocEntryAsSE > 0 Then
                    oJournalEntrySE = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    ReversarAsiento(oJournalEntrySE, intDocEntryAsSE, BubbleEvent, dtDate)
                Else
                    oJournalEntrySE = Nothing
                End If

                If BubbleEvent AndAlso Not String.IsNullOrEmpty(strDocEntrySM) AndAlso strDocEntrySM <> "0" Then
                    oSalidaMercancia = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
                    oEntradaMercancia = m_oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)
                    CargarSalidaMercanciaDataContract(oSalidaMercancia, CInt(strDocEntrySM), BubbleEvent)
                    If BubbleEvent Then CargarEntradaMercanciaObjeto(oEntradaMercancia, oSalidaMercancia, BubbleEvent, dtDate)
                Else
                    oSalidaMercancia = Nothing
                    oEntradaMercancia = Nothing
                End If

                oCompanyService = m_oCompany.GetCompanyService()
                oGeneralServiceFI = oCompanyService.GetGeneralService("SCGD_FAC_INT")
                oGeneralServiceOT = oCompanyService.GetGeneralService("SCGD_OT")

                If BubbleEvent Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.InicioReversionFI, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    If GuardarDatosDB(oCotizacion, oCotizacionNueva, oOrdenVenta, oEntradaMercancia, oSalidaMercancia, oGeneralServiceFI, oGeneralDataFI, oJournalEntryOG, oJournalEntryMO, oJournalEntrySE, intDocEntryFI, oGeneralServiceOT, oGeneralDataOT, dtDate, pVal) Then
                        RecargarFactura(intDocEntryFI, SBO_Application.Forms.Item(pVal.FormUID))
                        ValidateStatus(SBO_Application.Forms.Item(pVal.FormUID))
                        SBO_Application.StatusBar.SetText(My.Resources.Resource.FinReversionFI, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                    End If
                End If

            End If
        Catch ex As Exception
            SBO_Application.StatusBar.SetText(String.Format("{0}", ex.Message()), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        Finally
            Utilitarios.DestruirObjeto(oCotizacion)
            Utilitarios.DestruirObjeto(oCotizacionNueva)
            Utilitarios.DestruirObjeto(oPedido)
            Utilitarios.DestruirObjeto(oSalidaMercancia)
            Utilitarios.DestruirObjeto(oEntradaMercancia)
            Utilitarios.DestruirObjeto(oJournalEntryOG)
            Utilitarios.DestruirObjeto(oJournalEntryMO)
            Utilitarios.DestruirObjeto(oJournalEntrySE)
            Utilitarios.DestruirObjeto(oCompanyService)
            Utilitarios.DestruirObjeto(oGeneralServiceFI)
            Utilitarios.DestruirObjeto(oGeneralDataFI)
            Utilitarios.DestruirObjeto(oGeneralServiceOT)
            Utilitarios.DestruirObjeto(oGeneralDataOT)
            DetenerTimer()
        End Try
    End Sub

    Private Sub ActualizarOT(ByRef p_oGeneralServiceOT As GeneralService, ByRef p_oGeneralDataOT As GeneralData, ByRef p_oForm As SAPbouiCOM.Form, ByVal p_strDocEntry As String, ByVal p_strSucursal As String)
        Dim oGeneralParams As GeneralDataParams
        Dim strBdTalller As String

        Try
            If Utilitarios.ValidarOTInternaConfiguracion(DMS_Connector.Company.CompanySBO) Then
                If Not String.IsNullOrEmpty(p_oForm.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OT", 0)) Then
                    oGeneralParams = p_oGeneralServiceOT.GetDataInterface(GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParams.SetProperty("Code", p_oForm.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OT", 0).Trim())
                    p_oGeneralDataOT = p_oGeneralServiceOT.GetByParams(oGeneralParams)
                    p_oGeneralDataOT.SetProperty("U_EstO", "2")
                    p_oGeneralDataOT.SetProperty("U_DEstO", My.Resources.Resource.EstadoOrdenEnproceso)
                    p_oGeneralDataOT.SetProperty("U_DocEntry", p_strDocEntry)
                Else
                    p_oGeneralServiceOT = Nothing
                    p_oGeneralDataOT = Nothing
                End If
            Else
                p_oGeneralServiceOT = Nothing
                p_oGeneralDataOT = Nothing
                Utilitarios.DevuelveNombreBDTaller(SBO_Application, CInt(p_strSucursal), strBdTalller)
                Utilitarios.EjecutarConsulta(String.Format(" UPDATE SCGTA_TB_Orden SET Estado = 4, NoCotizacion = {1} WHERE NoOrden = '{0}' ", p_oForm.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("U_No_OT", 0).Trim, p_strDocEntry), strBdTalller, m_oCompany.Server)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            Utilitarios.DestruirObjeto(oGeneralParams)
        End Try
    End Sub

    Public Sub AsignaDimensionesContables(ByRef p_strValor As String, ByRef p_strCentroCosto As String)
        Try
            p_strValor = String.Empty
            Dim strUsaDimensiones As String = String.Empty
            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal)) Then
                With DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(confSucu) confSucu.U_Sucurs.Trim().Equals(strIDSucursal))
                    If .Configuracion_Tipo_Orden.Any(Function(tipoOT) tipoOT.U_Code.Equals(CInt(strTipoOT))) Then
                        If Not String.IsNullOrEmpty(.Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(CInt(strTipoOT))).U_UsaDim) Then strUsaDimensiones = .Configuracion_Tipo_Orden.FirstOrDefault(Function(tipoOT) tipoOT.U_Code.Equals(CInt(strTipoOT))).U_UsaDim
                        If Not String.IsNullOrEmpty(strUsaDimensiones) Then
                            If strUsaDimensiones = "Y" Then
                                If DMS_Connector.Configuracion.DimensionesOT.Any(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(strIDSucursal)) Then
                                    If DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(strIDSucursal)).DimensionesOT_Lineas.Any(Function(lineas) lineas.U_CodMar.Trim.Equals(strCodMarca)) Then
                                        With DMS_Connector.Configuracion.DimensionesOT.FirstOrDefault(Function(dimensiones) dimensiones.U_CodSuc.Trim.Equals(strIDSucursal)).DimensionesOT_Lineas.FirstOrDefault(Function(lineas) lineas.U_CodMar.Trim.Equals(strCodMarca))
                                            Select Case p_strCentroCosto
                                                Case CentroCosto.CostingCode
                                                    If Not String.IsNullOrEmpty(.U_Dim1) Then p_strValor = .U_Dim1
                                                Case CentroCosto.CostingCode2
                                                    If Not String.IsNullOrEmpty(.U_Dim2) Then p_strValor = .U_Dim2
                                                Case CentroCosto.CostingCode3
                                                    If Not String.IsNullOrEmpty(.U_Dim3) Then p_strValor = .U_Dim3
                                                Case CentroCosto.CostingCode4
                                                    If Not String.IsNullOrEmpty(.U_Dim4) Then p_strValor = .U_Dim4
                                                Case CentroCosto.CostingCode5
                                                    If Not String.IsNullOrEmpty(.U_Dim5) Then p_strValor = .U_Dim5
                                            End Select
                                        End With
                                    End If
                                End If
                            End If
                        End If
                    End If
                End With
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Function ConsultaStockPrice(ByRef p_intDocEntry As Integer, ByRef p_strItemCode As String) As Double
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim dsInformacion As DBDataSource
        Dim index As Integer
        Dim dblResultado As Double = 0
        Try
            n = DIHelper.GetNumberFormatInfo(m_oCompany)
            m_oFormGenCotizacion.DataSources.DBDataSources.Add("IGE1")
            dsInformacion = m_oFormGenCotizacion.DataSources.DBDataSources.Item("IGE1")

            oConditions = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "DocEntry"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_intDocEntry
            oCondition.BracketCloseNum = 1

            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

            oCondition = oConditions.Add()
            oCondition.BracketOpenNum = 1
            oCondition.Alias = "ItemCode"
            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCondition.CondVal = p_strItemCode
            oCondition.BracketCloseNum = 1
            'ejecuta query
            dsInformacion.Query(oConditions)

            For index = 0 To dsInformacion.Size - 1
                If Not String.IsNullOrEmpty(dsInformacion.GetValue("ItemCode", index)) Then
                    dblResultado = Double.Parse(dsInformacion.GetValue("StockPrice", index), n)
                    Exit For
                End If
            Next
            Return dblResultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function
#End Region

#Region "Eventos"

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                                   ByRef pVal As SAPbouiCOM.ItemEvent, _
                                                   ByRef BubbleEvent As Boolean)
        Try

            Dim oForm As SAPbouiCOM.Form
            oForm = SBO_Application.Forms.Item(FormUID)

            If pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case "btnPrint"
                        If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE AndAlso oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                            Call ImprimirReporteFacturaInterna(FormUID, pVal, BubbleEvent)
                        Else
                            SBO_Application.StatusBar.SetText(My.Resources.Resource.MensajeCrearDocumentoAntesImprimir, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    Case "btnReversa"
                        ReversarFI(pVal, BubbleEvent)
                    Case "fldDFI"
                        oForm.PaneLevel = 1
                    Case "fldDR"
                        oForm.PaneLevel = 2
                End Select
            Else

                Select Case pVal.ItemUID
                    Case "btnReversa"
                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                            ReversarFI(pVal, BubbleEvent)
                        Else
                            BubbleEvent = False
                        End If
                End Select
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoFormDataLoad(ByVal oForm As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            ValidateStatus(oForm)
            oForm.Items.Item("txtObser").AffectsFormMode = True
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub
#End Region

    Private Sub InicializarTimer()
        Try
            'Inicializa un timer que se ejecuta cada 30 segundos
            'y llama al método LimpiarColaMensajes
            oTimer = New System.Timers.Timer(30000)
            RemoveHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            AddHandler oTimer.Elapsed, AddressOf LimpiarColaMensajes
            oTimer.AutoReset = True
            oTimer.Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub DetenerTimer()
        Try
            oTimer.Stop()
            oTimer.Dispose()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub LimpiarColaMensajes()
        Try
            'En las operaciones muy largas, la cola de mensajes se llena ocasionando que el add-on se desconecte y genere errores como
            'RPC Server call o similares. Para solucionarlo se debe ejecutar este método cada cierto tiempo (30 o 60 segundos) para limpiar
            'la cola de mensajes
            DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub
End Class

