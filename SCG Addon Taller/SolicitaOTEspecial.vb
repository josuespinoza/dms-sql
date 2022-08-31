Imports SCG.SBOFramework
Imports DMSOneFramework
Imports System.Globalization
Imports SCG.SBOFramework.UI
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports SCG.DMSOne.Framework

Partial Public Class SolicitaOTEspecial : Implements IFormularioSBO


#Region "... Declaraciones ..."

    Private m_oCompany As SAPbobsCOM.Company
    Private m_SBO_Application As SAPbouiCOM.Application
    Public n As NumberFormatInfo
    
    Private g_dtLocal As DataTable
    Public Const mc_strQUT1 As String = "QUT1"
    
    Private blnUsaConfiguracionInternaTaller As Boolean = False

#End Region

#Region "... Constructor ..."

    Public Sub New(ByVal ocompany As SAPbobsCOM.Company, ByVal p_SBOAplication As Application)
        
        m_oCompany = ocompany
        m_SBO_Application = p_SBOAplication
        n = DIHelper.GetNumberFormatInfo(m_oCompany)

    End Sub

#End Region

#Region "... Propiedades ..."

#End Region

#Region "... Inicializacion de Controles ..."

    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then
            CargaFormulario()
            'g_dtLocal = FormularioSBO.DataSources.DataTables.Add(g_strDtConsul)
            'dtLineas = FormularioSBO.DataSources.DataTables.Add(g_strDtConsul)
            Call FormularioSBO.DataSources.DBDataSources.Add(mc_strQUT1)
            Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources

            userDS.Add("noOT", BoDataType.dt_LONG_TEXT, 100)
            userDS.Add("noCot", BoDataType.dt_LONG_TEXT, 100)

            g_oEditNoOT = DirectCast(FormularioSBO.Items.Item("txtNoOT").Specific, SAPbouiCOM.EditText)
            g_oEditNoCot = DirectCast(FormularioSBO.Items.Item("txtNoCot").Specific, SAPbouiCOM.EditText)
            g_oMtxOtLines = DirectCast(FormularioSBO.Items.Item("mtxOTLines").Specific, SAPbouiCOM.Matrix)
            g_oEditNoOT.DataBind.SetBound(True, "", "noOT")
            g_oEditNoCot.DataBind.SetBound(True, "", "noCot")
        End If

    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        'Manejo de formulario
        FormularioSBO.Freeze(True)

        'cboTipoOtInterna = New ComboBoxSBO("cboTipOtIn", FormularioSBO, True, "", "")
        CargarTiposOtEspeciales()

        'Manejo de formulario
        FormularioSBO.Freeze(False)
    End Sub

#End Region

#Region "... Metodos ..."

    ''' <summary>
    ''' Carga el numero de OT en el Formulario
    ''' </summary>
    Public Sub CargaCOT_OT(ByRef pval As SAPbouiCOM.ItemEvent, ByVal numOT As String, ByVal DocEntry As String)
        If (g_oEditNoOT.Value = "") Then
            g_oEditNoOT.Value = numOT
        End If
        If (g_oEditNoCot.Value = "") Then
            g_oEditNoCot.Value = DocEntry
        End If
    End Sub

    ''' <summary>
    ''' Carga combobox con los tipos de ot internas
    ''' </summary>
    Public Sub CargarTiposOtEspeciales()
        Try
            sboItem = FormularioSBO.Items.Item(mc_strTipoOtEspeciales)
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

            Dim query As String = "select  ote.U_IDTipoOrden, tot.Name " & _
                                  "FROM [@SCGD_CONF_OT_ESP] ote with (nolock) " & _
                                  "Left join [@SCGD_TIPO_ORDEN] tot with (nolock) " & _
                                  "on ote.U_IDTipoOrden=tot.Code " & _
                                  "ORDER BY ote.U_IDTipoOrden"

            Call Utilitarios.CargarValidValuesEnCombos(sboCombo.ValidValues, query)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Sub LoadMatrixLines(ByRef p_blnUsaOTSAP As Boolean, ByRef g_strCreaHjaCanPend As String)
        Try

            'Dim dtLocal As DataTable
            'Dim oMatriz As Matrix
            Dim query2 As String = String.Empty
            Dim query As String = String.Empty

            If (dtLineas.Rows.Count = 0) Then

                g_dtLocal = FormularioSBO.DataSources.DataTables.Item("local")

                If Not p_blnUsaOTSAP Then

                    If g_strCreaHjaCanPend = "N" Then

                        query2 = String.Format("SELECT QUT1.U_SCGD_IdRepxOrd FROM QUT1 with (nolock) INNER JOIN OQUT with (nolock) on QUT1.DocEntry = OQUT.DocEntry WHERE OQUT.U_SCGD_Numero_OT is null and OQUT.U_SCGD_No_Visita in (SELECT U_SCGD_No_Visita FROM OQUT with (nolock) WHERE oqut.U_SCGD_Numero_OT = '{0}')", g_oEditNoOT.Value.Trim())

                        query = String.Format("SELECT QUT1.ItemCode, QUT1.Dscription, QUT1.Quantity, QUT1.Currency, QUT1.Price, QUT1.FreeTxt, QUT1.DocEntry, QUT1.LineNum, QUT1.DiscPrcnt, QUT1.U_SCGD_IdRepxOrd, QUT1.U_SCGD_Costo, QUT1.TaxCode, " & _
                                                           "QUT1.U_SCGD_CPen, QUT1.U_SCGD_CSol, QUT1.U_SCGD_CRec, QUT1.U_SCGD_CPDe, QUT1.U_SCGD_CPTr, QUT1.U_SCGD_CPBo, QUT1.U_SCGD_Compra,  QUT1.U_SCGD_TipArt " & _
                                                           "FROM QUT1 with (nolock) INNER JOIN OITM on QUT1.itemCode = OITM.itemCode " & _
                                                           "WHERE QUT1.DocEntry = '{0}' and  QUT1.TreeType <> 'I' and QUT1.TreeType <> 'T' and QUT1.U_SCGD_Aprobado = 1 " & _
                                                           "and OITM.U_SCGD_TipoArticulo in (1,2,3,4,5,6,11,12) and QUT1.U_SCGD_IdRepxOrd not in ({1})", g_oEditNoCot.Value.Trim(), query2)

                    Else 'Permite Crear HIjas con Cantidades Pendientes
                        query2 = String.Format("SELECT QUT1.U_SCGD_IdRepxOrd FROM QUT1 with (nolock) INNER JOIN OQUT with (nolock) on QUT1.DocEntry = OQUT.DocEntry WHERE OQUT.U_SCGD_Numero_OT is null and OQUT.U_SCGD_No_Visita in (SELECT U_SCGD_No_Visita FROM OQUT with (nolock) WHERE oqut.U_SCGD_Numero_OT = '{0}')", g_oEditNoOT.Value.Trim())

                        query = String.Format("SELECT QUT1.ItemCode, QUT1.Dscription, QUT1.Quantity, QUT1.Currency, QUT1.Price, QUT1.FreeTxt, QUT1.DocEntry, QUT1.LineNum, QUT1.DiscPrcnt, QUT1.U_SCGD_IdRepxOrd, QUT1.U_SCGD_Costo, QUT1.TaxCode, " & _
                                                           "QUT1.U_SCGD_CPen, QUT1.U_SCGD_CSol, QUT1.U_SCGD_CRec, QUT1.U_SCGD_CPDe, QUT1.U_SCGD_CPTr, QUT1.U_SCGD_CPBo, QUT1.U_SCGD_Compra,  QUT1.U_SCGD_TipArt " & _
                                                           "FROM QUT1 with (nolock) INNER JOIN OITM on QUT1.itemCode = OITM.itemCode " & _
                                                           "WHERE QUT1.DocEntry = '{0}' and  QUT1.TreeType <> 'I' and QUT1.TreeType <> 'T' and QUT1.U_SCGD_Aprobado = 1 " & _
                                                           "and OITM.U_SCGD_TipoArticulo in (1,2,3,4,5,6,11,12) and	QUT1.U_SCGD_Traslad <> 3 and QUT1.U_SCGD_Traslad <> 4 " & _
                                                           "and QUT1.U_SCGD_IdRepxOrd not in ({1})", g_oEditNoCot.Value.Trim(), query2)
                    End If

                Else

                    If g_strCreaHjaCanPend = "N" Then

                        query2 = String.Format("SELECT QUT1.U_SCGD_ID FROM QUT1 with (nolock) INNER JOIN OQUT with (nolock) on QUT1.DocEntry = OQUT.DocEntry WHERE OQUT.U_SCGD_Numero_OT is null and OQUT.U_SCGD_No_Visita in (SELECT U_SCGD_No_Visita FROM OQUT with (nolock) WHERE oqut.U_SCGD_Numero_OT = '{0}')", g_oEditNoOT.Value.Trim())

                        query = String.Format("SELECT QUT1.ItemCode, QUT1.Dscription, QUT1.Quantity, QUT1.Currency, QUT1.Price, QUT1.FreeTxt, QUT1.DocEntry, QUT1.LineNum, QUT1.DiscPrcnt, QUT1.U_SCGD_ID, QUT1.U_SCGD_Costo, QUT1.TaxCode, " & _
                                                            "QUT1.U_SCGD_CPen, QUT1.U_SCGD_CSol, QUT1.U_SCGD_CRec, QUT1.U_SCGD_CPDe, QUT1.U_SCGD_CPTr, QUT1.U_SCGD_CPBo, QUT1.U_SCGD_Compra, QUT1.U_SCGD_TipArt " & _
                                                            "FROM QUT1 with (nolock) INNER JOIN OITM on QUT1.itemCode = OITM.itemCode " & _
                                                            "WHERE QUT1.DocEntry = '{0}' and  QUT1.TreeType <> 'I' and QUT1.TreeType <> 'T' and QUT1.U_SCGD_Aprobado = 1 " & _
                                                            "and OITM.U_SCGD_TipoArticulo in (1,2,3,4,5,6,11,12) and QUT1.U_SCGD_ID not in ({1})", g_oEditNoCot.Value.Trim(), query2)


                    Else
                        query2 = String.Format("SELECT QUT1.U_SCGD_ID FROM QUT1 with (nolock) INNER JOIN OQUT with (nolock) on QUT1.DocEntry = OQUT.DocEntry WHERE OQUT.U_SCGD_Numero_OT is null and OQUT.U_SCGD_No_Visita in (SELECT U_SCGD_No_Visita FROM OQUT with (nolock) WHERE oqut.U_SCGD_Numero_OT = '{0}')", g_oEditNoOT.Value.Trim())

                        query = String.Format("SELECT QUT1.ItemCode, QUT1.Dscription, QUT1.Quantity, QUT1.Currency, QUT1.Price, QUT1.FreeTxt, QUT1.DocEntry, QUT1.LineNum, QUT1.DiscPrcnt, QUT1.U_SCGD_ID, QUT1.U_SCGD_Costo, QUT1.TaxCode, " & _
                                                            "QUT1.U_SCGD_CPen, QUT1.U_SCGD_CSol, QUT1.U_SCGD_CRec, QUT1.U_SCGD_CPDe, QUT1.U_SCGD_CPTr, QUT1.U_SCGD_CPBo, QUT1.U_SCGD_Compra, QUT1.U_SCGD_TipArt " & _
                                                            "FROM QUT1 with (nolock) INNER JOIN OITM on QUT1.itemCode = OITM.itemCode " & _
                                                            "WHERE QUT1.DocEntry = '{0}' and  QUT1.TreeType <> 'I' and QUT1.TreeType <> 'T' and QUT1.U_SCGD_Aprobado = 1 " & _
                                                            "and OITM.U_SCGD_TipoArticulo in (1,2,3,4,5,6,11,12) and QUT1.U_SCGD_Traslad <> 3 and QUT1.U_SCGD_Traslad <> 4 " & _
                                                            "and QUT1.U_SCGD_ID not in ({1})", g_oEditNoCot.Value.Trim(), query2)

                    End If

                End If

                g_dtLocal.ExecuteQuery(query)
                g_oMtxOtLines = DirectCast(FormularioSBO.Items.Item(mc_strMatizCotLines).Specific, SAPbouiCOM.Matrix)


                For i As Integer = 0 To g_dtLocal.Rows.Count - 1
                    If Not String.IsNullOrEmpty(g_dtLocal.GetValue("ItemCode", i).ToString().Trim()) Then
                        dtLineas.Rows.Add(1)

                        dtLineas.SetValue("col_Code", i, g_dtLocal.GetValue("ItemCode", i))
                        dtLineas.SetValue("col_Name", i, g_dtLocal.GetValue("Dscription", i))
                        dtLineas.SetValue("col_Quant", i, g_dtLocal.GetValue("Quantity", i))
                        dtLineas.SetValue("col_Curr", i, g_dtLocal.GetValue("Currency", i))
                        dtLineas.SetValue("col_Price", i, g_dtLocal.GetValue("Price", i))
                        dtLineas.SetValue("col_Obs", i, g_dtLocal.GetValue("FreeTxt", i))
                        dtLineas.SetValue("col_DEnt", i, g_dtLocal.GetValue("DocEntry", i))
                        dtLineas.SetValue("col_LNum", i, g_dtLocal.GetValue("LineNum", i))
                        dtLineas.SetValue("col_PrcDes", i, g_dtLocal.GetValue("DiscPrcnt", i))
                        If Not p_blnUsaOTSAP Then
                            dtLineas.SetValue("col_IdRXOr", i, g_dtLocal.GetValue("U_SCGD_IdRepxOrd", i))
                        Else
                            dtLineas.SetValue("col_IDLine", i, g_dtLocal.GetValue("U_SCGD_ID", i))
                        End If
                        dtLineas.SetValue("col_Costo", i, g_dtLocal.GetValue("U_SCGD_Costo", i))
                        dtLineas.SetValue("col_IndImp", i, g_dtLocal.GetValue("TaxCode", i))
                        dtLineas.SetValue("col_CPend", i, g_dtLocal.GetValue("U_SCGD_CPen", i))
                        dtLineas.SetValue("col_CSol", i, g_dtLocal.GetValue("U_SCGD_CSol", i))
                        dtLineas.SetValue("col_CRec", i, g_dtLocal.GetValue("U_SCGD_CRec", i))
                        dtLineas.SetValue("col_PenDev", i, g_dtLocal.GetValue("U_SCGD_CPDe", i))
                        dtLineas.SetValue("col_PenTra", i, g_dtLocal.GetValue("U_SCGD_CPTr", i))
                        dtLineas.SetValue("col_PenBod", i, g_dtLocal.GetValue("U_SCGD_CPBo", i))
                        dtLineas.SetValue("col_Compra", i, g_dtLocal.GetValue("U_SCGD_Compra", i))
                        dtLineas.SetValue("col_TipAr", i, g_dtLocal.GetValue("U_SCGD_TipArt", i))


                    End If
                Next
                If dtLineas.Rows.Count > 0 Then
                    g_oMtxOtLines.LoadFromDataSource()
                Else
                    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.TXTNoLinesAvailable, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    FormularioSBO.Close()
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
    End Sub

    Public Function CreaSolicitudOTEsp(ByVal oForm As SAPbouiCOM.Form) As Boolean
        Dim result As Boolean = False

        Try
            Dim udoSolOTEsp As UDOSolOTEsp
            Dim encabezadoUDO As New EncabezadoUDOSolOTEsp()
            Dim listaLineasUDO As New ListaLineasUDOSolOTEsp()
            Dim lineaUDO As New LineaUDOSolOTEsp()
            Dim query As String
            Dim strSeparadorDecimalesSAP As String = String.Empty
            Dim strSeparadorMilesSAP As String = String.Empty
            Dim dtConfOTEspeciales As System.Data.DataTable
            Dim drwConfOTEsp As System.Data.DataRow
            Dim QueryConfOTEspeciales As String = String.Empty
            Dim strAsesorOTEspecial As String = String.Empty
            Dim strCardCodeClienteOTEspecial As String = String.Empty
            Dim strNoOrdenOrigen As String = String.Empty

            Utilitarios.ObtenerSeparadoresNumerosSAP(strSeparadorMilesSAP, strSeparadorDecimalesSAP, m_oCompany.CompanyDB, m_oCompany.Server)


            g_oEditNoOT = DirectCast(oForm.Items.Item("txtNoOT").Specific, SAPbouiCOM.EditText)
            g_oEditNoCot = DirectCast(oForm.Items.Item("txtNoCot").Specific, SAPbouiCOM.EditText)

            udoSolOTEsp = New UDOSolOTEsp(m_oCompany)
            g_dtLocal = oForm.DataSources.DataTables.Item("local")

            query = String.Format("select U_SCGD_Ano_Vehi, U_SCGD_CardCodeOrig, U_SCGD_CardNameOrig, OwnerCode, CardCode, U_SCGD_Cod_Estilo, U_SCGD_Cod_Marca, U_SCGD_Cod_Modelo, U_SCGD_Cod_Unidad, Comments, DocEntry, U_SCGD_Des_Esti, U_SCGD_Des_Mode, U_SCGD_Des_Marc, U_SCGD_idSucursal, U_SCGD_Fech_Comp, U_SCGD_Num_Vehiculo, U_SCGD_GeneraOR, U_SCGD_Kilometraje, CardName, U_SCGD_Numero_OT, U_SCGD_No_Visita, U_SCGD_Num_Placa, U_SCGD_Num_VIN, U_SCGD_Tipo_OT from OQUT with (nolock) where DocEntry='{0}'", g_oEditNoCot.Value)

            g_dtLocal.ExecuteQuery(query)

            encabezadoUDO.Anno = Convert.ToInt16(g_dtLocal.GetValue("U_SCGD_Ano_Vehi", 0))
            encabezadoUDO.CardCodeOrigen = g_dtLocal.GetValue("U_SCGD_CardCodeOrig", 0).ToString().Trim()
            encabezadoUDO.CardNameOrigen = g_dtLocal.GetValue("U_SCGD_CardNameOrig", 0).ToString().Trim()
            encabezadoUDO.CodigoAsesor = g_dtLocal.GetValue("OwnerCode", 0).ToString().Trim()
            encabezadoUDO.CodigoCliente = g_dtLocal.GetValue("CardCode", 0).ToString().Trim()
            encabezadoUDO.CodigoEstilo = g_dtLocal.GetValue("U_SCGD_Cod_Estilo", 0).ToString().Trim()
            encabezadoUDO.CodigoMarca = g_dtLocal.GetValue("U_SCGD_Cod_Marca", 0).ToString().Trim()
            encabezadoUDO.CodigoModelo = g_dtLocal.GetValue("U_SCGD_Cod_Modelo", 0).ToString().Trim()
            encabezadoUDO.CodigoUnidad = g_dtLocal.GetValue("U_SCGD_Cod_Unidad", 0).ToString().Trim()

            strNoOrdenOrigen = g_dtLocal.GetValue("U_SCGD_Numero_OT", 0).ToString().Trim()
            Dim comentarios As String = String.Format(My.Resources.Resource.Sederivadelaorden & " {0} ", strNoOrdenOrigen) & g_dtLocal.GetValue("Comments", 0).ToString().Trim()
            If comentarios.Length > 250 Then
                comentarios = comentarios.Substring(0, 250)
            End If
            encabezadoUDO.Comentarios = comentarios

            encabezadoUDO.CotizacionReferencia = g_dtLocal.GetValue("DocEntry", 0)
            encabezadoUDO.DescripcionEstilo = g_dtLocal.GetValue("U_SCGD_Des_Esti", 0).ToString().Trim()
            encabezadoUDO.DescripcionModelo = g_dtLocal.GetValue("U_SCGD_Des_Mode", 0).ToString().Trim()
            encabezadoUDO.DescripcionMarca = g_dtLocal.GetValue("U_SCGD_Des_Marc", 0).ToString().Trim()

            Dim bdidSucursal = g_dtLocal.GetValue("U_SCGD_idSucursal", 0).ToString().Trim()
            Dim bdSucursalName As String
            Utilitarios.DevuelveCadenaConexionBDTaller(m_SBO_Application, bdidSucursal, bdSucursalName)
            If Not String.IsNullOrEmpty(bdSucursalName) Then

                Dim estado As String

                If blnUsaConfiguracionInternaTaller Then
                    query = String.Empty
                    query = String.Format("select U_EstO from [@SCGD_OT] ord with (nolock) where ord.U_NoOT = '{0}' and ord.U_Sucu = '{1}'", g_oEditNoOT.Value, bdidSucursal)
                    estado = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()
                    If Not String.IsNullOrEmpty(estado) Then
                        query = String.Empty
                        query = String.Format("select Name from [@SCGD_ESTADOS_OT] with (nolock) where Code = '{0}'", estado)
                        encabezadoUDO.EstadoOT = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()
                    End If

                Else
                    query = String.Empty
                    query = String.Format("select Estado from SCGTA_TB_Orden ord with (nolock) where ord.NoOrden = '{0}'", g_oEditNoOT.Value)
                    estado = Utilitarios.EjecutarConsulta(query, bdSucursalName, m_SBO_Application.Company.ServerName).ToString().Trim()
                    If Not String.IsNullOrEmpty(estado) Then
                        query = String.Empty
                        query = String.Format("select Name from [@SCGD_ESTADOS_OT] with (nolock) where Code = '{0}'", estado)
                        encabezadoUDO.EstadoOT = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()
                    End If
                End If

                query = String.Empty
                query = String.Format("select U_SerOfV from [@SCGD_CONF_SUCURSAL] with (nolock) where U_Sucurs = '{0}'", bdidSucursal)
                encabezadoUDO.Series = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()

            End If
            encabezadoUDO.FechaApertura = DateTime.Now
            encabezadoUDO.FechaCompromiso = g_dtLocal.GetValue("U_SCGD_Fech_Comp", 0)
            encabezadoUDO.IdVehiculo = g_dtLocal.GetValue("U_SCGD_Num_Vehiculo", 0)
            If Not String.IsNullOrEmpty(g_dtLocal.GetValue("U_SCGD_GeneraOR", 0).ToString().Trim()) Then
                If g_dtLocal.GetValue("U_SCGD_GeneraOR", 0).ToString().Trim() = "1" Then
                    encabezadoUDO.ImprimeRecepcion = "Y"
                End If
            End If

            encabezadoUDO.Kilometraje = g_dtLocal.GetValue("U_SCGD_Kilometraje", 0)

            query = String.Empty
            query = String.Format("select (firstName+ ' ' + lastName) as Name from OHEM with (nolock) where empID='{0}'", g_dtLocal.GetValue("OwnerCode", 0).ToString().Trim())
            encabezadoUDO.NombreAsesor = Utilitarios.EjecutarConsulta(query, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()
            encabezadoUDO.NombreCliente = g_dtLocal.GetValue("CardName", 0).ToString().Trim()

            sboItem = oForm.Items.Item("cboTipOtE")
            sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)
            encabezadoUDO.NombreTipoOT = sboCombo.Selected.Description
            encabezadoUDO.NumeroCotizacion = 0
            encabezadoUDO.NumeroOTPadre = g_dtLocal.GetValue("U_SCGD_Numero_OT", 0).ToString().Trim()
            encabezadoUDO.NumeroVisita = g_dtLocal.GetValue("U_SCGD_No_Visita", 0).ToString().Trim()
            encabezadoUDO.OTReferencia = encabezadoUDO.NumeroOTPadre
            encabezadoUDO.Placa = g_dtLocal.GetValue("U_SCGD_Num_Placa", 0).ToString().Trim()

            encabezadoUDO.CardCodeOrigen = g_dtLocal.GetValue("CardCode", 0).ToString().Trim()
            encabezadoUDO.CardNameOrigen = g_dtLocal.GetValue("CardName", 0).ToString().Trim()

            encabezadoUDO.TipoOrden = sboCombo.Value

            QueryConfOTEspeciales = String.Format("Select U_IDAsesor, U_CardCodCliente From dbo.[@SCGD_CONF_OT_ESP] where U_IDTipoOrden = '{0}'", encabezadoUDO.TipoOrden)

            dtConfOTEspeciales = Utilitarios.EjecutarConsultaDataTable(QueryConfOTEspeciales, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName)

            If dtConfOTEspeciales.Rows.Count <> 0 Then

                drwConfOTEsp = dtConfOTEspeciales.Rows(0)

                Dim strQuery = String.Empty

                If Not drwConfOTEsp.IsNull("U_IDAsesor") Then

                    strAsesorOTEspecial = drwConfOTEsp.Item("U_IDAsesor")
                    encabezadoUDO.CodigoAsesor = strAsesorOTEspecial
                    strQuery = String.Format("select (firstName+ ' ' + lastName) as Name from OHEM with (nolock) where empID='{0}'", strAsesorOTEspecial).ToString().Trim()
                    encabezadoUDO.NombreAsesor = Utilitarios.EjecutarConsulta(strQuery, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()
                End If

                If Not drwConfOTEsp.IsNull("U_CardCodCliente") Then

                    strCardCodeClienteOTEspecial = drwConfOTEsp.Item("U_CardCodCliente")
                    encabezadoUDO.CodigoCliente = strCardCodeClienteOTEspecial
                    'falta agregar nombre del cliente configurado para esta OT Especial
                    strQuery = String.Format("select CardName from dbo.[OCRD] where CardType = 'C' and cardcode ='{0}'", strCardCodeClienteOTEspecial).ToString().Trim()
                    encabezadoUDO.NombreCliente = Utilitarios.EjecutarConsulta(strQuery, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName).ToString().Trim()
                End If

            End If


            encabezadoUDO.VIN = g_dtLocal.GetValue("U_SCGD_Num_VIN", 0).ToString().Trim()

            udoSolOTEsp.Encabezado = encabezadoUDO

            dtLineas = oForm.DataSources.DataTables.Item(strDataTableLineas)
            g_oMtxOtLines = DirectCast(oForm.Items.Item(mc_strMatizCotLines).Specific, Matrix)
            g_oMtxOtLines.FlushToDataSource()

            Dim aprobacionesXSuc As Boolean = True
            '''''''''''''''''''''''''''''''''
            'aqui va la validacion de aprobaciones por tipo de orden

            Dim queryValApr = "SELECT U_EspAprob FROM [@SCGD_CONF_APROBAC] CAP with (nolock)" & _
                                    "LEFT JOIN [@SCGD_CONF_SUCURSAL] CS with (nolock) ON CAP.DocEntry = CS.DocEntry " & _
                                    "WHERE CS.U_Sucurs ='{0}' and cap.U_TipoOT = '{1}'"

            Dim equeryValApr = String.Format(queryValApr, bdidSucursal, sboCombo.Value)
            Dim espApr As String = Utilitarios.EjecutarConsulta(equeryValApr, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName)
            If Not String.IsNullOrEmpty(espApr) Then
                If espApr = "Y" Then
                    aprobacionesXSuc = True
                Else
                    aprobacionesXSuc = False
                End If
            Else
                aprobacionesXSuc = False
            End If

            udoSolOTEsp.ListaLineas = New ListaLineasUDOSolOTEsp()
            listaLineasUDO.LineasUDO = New System.Collections.Generic.List(Of DI.ILineaUDO)()
            Dim elementoPrecio As String
            Dim elementoCosto As String
            Dim elementoCantidad As String
            Dim elementoPorcentajeDesc As String

            For i As Integer = 0 To dtLineas.Rows.Count - 1
                If dtLineas.GetValue("col_Sel", i) = "Y" Then
                    elementoPrecio = dtLineas.GetValue("col_Price", i).ToString().Trim()
                    elementoCosto = dtLineas.GetValue("col_Costo", i).ToString().Trim()
                    elementoCantidad = dtLineas.GetValue("col_Quant", i).ToString().Trim()
                    elementoPorcentajeDesc = dtLineas.GetValue("col_PrcDes", i).ToString().Trim()

                    Dim Precio As String = CStr(elementoPrecio).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                    Dim Costo As String = CStr(elementoCosto).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                    Dim Cantidad As String = CStr(elementoCantidad).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)
                    Dim PorcDescuento As String = CStr(elementoPorcentajeDesc).Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, strSeparadorDecimalesSAP)

                    Dim decPrecio As Decimal = Decimal.Parse(Precio)
                    Dim decCosto As Decimal = Decimal.Parse(Costo)
                    Dim decCantidad As Decimal = Decimal.Parse(Cantidad)
                    Dim decPorcDescuento As Decimal = Decimal.Parse(PorcDescuento)

                    decPrecio = Decimal.Parse(elementoPrecio, n)
                    decCosto = Decimal.Parse(elementoCosto, n)
                    decCantidad = Decimal.Parse(elementoCantidad, n)
                    decPorcDescuento = Decimal.Parse(elementoPorcentajeDesc, n)

                    lineaUDO = New LineaUDOSolOTEsp()
                    'cantidad
                    lineaUDO.Cantidad = CDbl(decCantidad)
                    'Observaciones
                    If dtLineas.GetValue("col_Obs", i).ToString().Trim().Length >= 100 Then
                        lineaUDO.Comentarios = dtLineas.GetValue("col_Obs", i).ToString().Trim().Substring(0, 100)
                    Else
                        lineaUDO.Comentarios = dtLineas.GetValue("col_Obs", i).ToString().Trim()
                    End If
                    'Costo
                    lineaUDO.Costo = CDbl(decCosto)
                    'Nombre/Descripcion
                    lineaUDO.Description = dtLineas.GetValue("col_Name", i)
                    'IdRepuestos por Orden
                    Integer.TryParse(dtLineas.GetValue("col_IdRXOr", i), lineaUDO.IdRepuestosXOrden)
                    'Indicador de impuestos
                    lineaUDO.Impuestos = dtLineas.GetValue("col_IndImp", i)
                    'Codigo
                    lineaUDO.ItemCode = dtLineas.GetValue("col_Code", i)
                    'Moneda
                    lineaUDO.Moneda = dtLineas.GetValue("col_Curr", i)
                    'Porcentaje de descuento
                    lineaUDO.PorcentajeDescuento = CDbl(decPorcDescuento)
                    'Precio
                    lineaUDO.Precio = CDbl(decPrecio)
                    lineaUDO.CantPendiente = dtLineas.GetValue("col_CPend", i)
                    lineaUDO.CantSolicitada = dtLineas.GetValue("col_CSol", i)
                    lineaUDO.CantRecibida = dtLineas.GetValue("col_CRec", i)
                    lineaUDO.CantPendDevolucion = dtLineas.GetValue("col_PenDev", i)
                    lineaUDO.CantPendTraslado = dtLineas.GetValue("col_PenTra", i)
                    lineaUDO.CantPendBodega = dtLineas.GetValue("col_PenBod", i)

                    If Not String.IsNullOrEmpty(dtLineas.GetValue("col_Compra", i).ToString().Trim()) Then
                        lineaUDO.Compra = dtLineas.GetValue("col_Compra", i).ToString().Trim()
                    Else
                        lineaUDO.Compra = "N"
                    End If

                    If aprobacionesXSuc Then
                        lineaUDO.Seleccionar = "Y"
                    End If

                    lineaUDO.IDLinea = dtLineas.GetValue("col_IDLine", i).ToString().Trim()
                    lineaUDO.TipoArticulo = dtLineas.GetValue("col_TipAr", i).ToString().Trim()
                    listaLineasUDO.LineasUDO.Add(lineaUDO)
                End If
            Next
            udoSolOTEsp.ListaLineas = listaLineasUDO

            m_oCompany.StartTransaction()

            result = udoSolOTEsp.Insert()

            If result Then

                m_SBO_Application.StatusBar.SetText(My.Resources.Resource.MsgSolOtEspSuccess, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(BoWfTransOpt.wf_Commit)
                End If

                If aprobacionesXSuc Then
                    Dim solicitudOTEspecial As SolicitudOrdenEspecial
                    solicitudOTEspecial = New SolicitudOrdenEspecial(m_SBO_Application, m_oCompany, "SCGD_SOT")
                    solicitudOTEspecial.CrearCotizacionParaOT_Aprobadas(udoSolOTEsp.Encabezado.DocEntry, oForm)
                Else
                    Dim mensajeria As MensajeriaCls
                    mensajeria = New MensajeriaCls(m_SBO_Application, m_oCompany)
                    mensajeria.CreaMensajeSBO(My.Resources.Resource.MsgSolOtEspSuccess, udoSolOTEsp.Encabezado.DocEntry, m_oCompany, udoSolOTEsp.Encabezado.NumeroOTPadre, False, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSOE), bdidSucursal)
                    'mensajeria.CreaMensajeSBO(My.Resources.Resource.MsgSolOtEspSuccess, udoSolOTEsp.Encabezado.DocEntry, m_oCompany, udoSolOTEsp.Encabezado.NumeroOTPadre, False, Convert.ToInt32(Utilitarios.RolesMensajeria.EncargadoSOE), bdidSucursal, oForm, "local")
                End If
                oForm.Close()
            Else
                If m_oCompany.InTransaction Then
                    m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                End If
            End If


        Catch ex As Exception
            If m_oCompany.InTransaction Then
                m_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
        Return result

    End Function

    Public Function VerificarEstadoTrasladoFilasCotizacion(ByVal p_intNumeroCotizacion As Integer, ByVal p_strCreaHjaCanPend As String) As Boolean

        Dim resultFunction As Boolean = True

        Try

            If p_strCreaHjaCanPend = "N" Then

                Dim query As String = "select count(QUT1.ItemCode) from QUT1 with (nolock) " & _
                                                          "inner join OITM with (nolock) on QUT1.ItemCode = OITM.ItemCode " & _
                                                          "where (OITM.U_SCGD_TipoArticulo = 1 AND QUT1.U_SCGD_Traslad <> 2 and U_SCGD_Compra = 'N') " & _
                                                          "and QUT1.U_SCGD_Aprobado = 1 " & _
                                                          "and QUT1.DocEntry = '{0}'"

                Dim queryF = String.Format(query, p_intNumeroCotizacion)

                Dim resultQuery As String = Utilitarios.EjecutarConsulta(queryF, m_SBO_Application.Company.DatabaseName, m_SBO_Application.Company.ServerName)
                If Not String.IsNullOrEmpty(resultQuery) Then
                    Dim result As Integer = 0
                    Integer.TryParse(resultQuery, result)
                    If (result > 0) Then
                        resultFunction = False
                    End If
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, m_SBO_Application)
        End Try
        Return resultFunction

    End Function

#End Region

#Region "... Eventos ..."

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        Dim oForm As SAPbouiCOM.Form
        oForm = m_SBO_Application.Forms.Item(FormUID)

        If pVal.BeforeAction Then
            'If oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
            '    m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrFormQuotationUpdateMode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '    BubbleEvent = False

            'End If

        ElseIf pVal.ActionSuccess Then
            Select Case pVal.ItemUID
                Case "btnGeSOTE"
                    sboItem = oForm.Items.Item("cboTipOtE")
                    sboCombo = DirectCast(sboItem.Specific, SAPbouiCOM.ComboBox)

                    If Utilitarios.ValidarOTInternaConfiguracion(m_oCompany) Then
                        blnUsaConfiguracionInternaTaller = True
                    Else
                        blnUsaConfiguracionInternaTaller = False
                    End If

                    If Not String.IsNullOrEmpty(sboCombo.Value) Then
                        'Actualiza_CotizacionOtGeneraFI(oForm)3
                        'm_oCompany.StartTransaction()
                        If Not CreaSolicitudOTEsp(oForm) Then
                            'm_oCompany.EndTransaction(BoWfTransOpt.wf_RollBack)
                            m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrSolOTEsp, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    Else
                        m_SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrChooseOTEspType, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                Case mc_strMatizCotLines
                    oForm.Freeze(True)
                    If pVal.ColUID = "col_Sel" Then

                        dtLineas = oForm.DataSources.DataTables.Item(strDataTableLineas)
                        g_oMtxOtLines = DirectCast(oForm.Items.Item(mc_strMatizCotLines).Specific, Matrix)
                        g_oMtxOtLines.FlushToDataSource()

                        If dtLineas.GetValue("col_Sel", pVal.Row - 1) = "Y" Then
                            'AgregaLista(pVal.Row.ToString())
                        End If
                    End If
                    oForm.Freeze(False)
            End Select
        End If
    End Sub
#End Region


End Class
