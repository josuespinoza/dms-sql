Imports DMSOneFramework
Imports System.Globalization
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports DMSOneFramework.SCGCommon
Imports DMSOneFramework.SCGDataAccess
Imports DMS_Addon.ControlesSBO
Imports SCG.SBOFramework.UI
Imports System.Collections.Generic

Partial Public Class DevolucionDeVehiculos : Implements IUsaPermisos

    Dim m_strTablaLineasDevolucion As String = "@SCGD_DEVOLUCION_LIN"
    Dim m_strTablaDevolucion As String = "@SCGD_DEVOLUCION"

    Dim n As NumberFormatInfo

    Dim m_strUDFCuentaTransito As String = "U_Transito"
    Dim m_strUDFCuentaDevolucion As String = "U_Devolucion"
    Dim m_strUDFCuentaStock As String = "U_Stock"

    Public Sub AgregarVehiculos(ByVal p_dtSeleccionados As SAPbouiCOM.DataTable)
        Try
            Dim l_intTamano As Integer
            Dim l_decMontoAs As Decimal
            Dim l_decTipoC As Decimal

            FormularioSBO = ApplicationSBO.Forms.Item("SCGD_DDV")

            MatrixDevolucionDeVehiculos = New MatrizDevolucionDeVehiculos("mtxVeh", _formularioSBO, "@SCGD_DEVOLUCION_LIN")

            MatrixDevolucionDeVehiculos.Matrix.FlushToDataSource()

            l_intTamano = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).Size
            'l_intTamano = l_intTamano + 1

            For i As Integer = 0 To p_dtSeleccionados.Rows.Count - 1
                If String.IsNullOrEmpty(p_dtSeleccionados.GetValue("mont", i).ToString.Trim) Then
                    l_decMontoAs = 0
                Else
                    l_decMontoAs = p_dtSeleccionados.GetValue("mont", i)
                End If
                If String.IsNullOrEmpty(p_dtSeleccionados.GetValue("rate", i).ToString.Trim) Then
                    l_decTipoC = 0
                Else
                    l_decTipoC = p_dtSeleccionados.GetValue("rate", i)
                End If

                If l_intTamano = 1 AndAlso
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).GetValue("U_Cod_Unid", l_intTamano - 1) = String.Empty Then
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Recepcion", 0, p_dtSeleccionados.GetValue("rece", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Pedido", 0, p_dtSeleccionados.GetValue("pedi", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Cod_Unid", 0, p_dtSeleccionados.GetValue("unid", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Desc_Marca", 0, p_dtSeleccionados.GetValue("marc", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Desc_Estilo", 0, p_dtSeleccionados.GetValue("esti", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Desc_Modelo", 0, p_dtSeleccionados.GetValue("mode", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_VIN", 0, p_dtSeleccionados.GetValue("vin", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Motor", 0, p_dtSeleccionados.GetValue("moto", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Cod_Tipo_Inv", 0, p_dtSeleccionados.GetValue("tipo", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Monto_As", 0, l_decMontoAs.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Moneda", 0, p_dtSeleccionados.GetValue("mone", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Doc_Rate", 0, l_decTipoC.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Asiento", 0, p_dtSeleccionados.GetValue("asie", i))
                    ' FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Asiento_Dev", 0, String.Empty)
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Id_Veh", 0, p_dtSeleccionados.GetValue("code", i))

                    l_intTamano = FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).Size
                Else
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).InsertRecord(l_intTamano)

                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Recepcion", l_intTamano, p_dtSeleccionados.GetValue("rece", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Pedido", l_intTamano, p_dtSeleccionados.GetValue("pedi", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Cod_Unid", l_intTamano, p_dtSeleccionados.GetValue("unid", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Desc_Marca", l_intTamano, p_dtSeleccionados.GetValue("marc", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Desc_Estilo", l_intTamano, p_dtSeleccionados.GetValue("esti", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Desc_Modelo", l_intTamano, p_dtSeleccionados.GetValue("mode", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_VIN", l_intTamano, p_dtSeleccionados.GetValue("vin", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Motor", l_intTamano, p_dtSeleccionados.GetValue("moto", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Cod_Tipo_Inv", l_intTamano, p_dtSeleccionados.GetValue("tipo", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Monto_As", l_intTamano, l_decMontoAs.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Moneda", l_intTamano, p_dtSeleccionados.GetValue("mone", i))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Doc_Rate", l_intTamano, l_decTipoC.ToString(n))
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Asiento", l_intTamano, p_dtSeleccionados.GetValue("asie", i))
                    'FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Num_Asiento_Dev", l_intTamano, String.Empty)
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).SetValue("U_Id_Veh", l_intTamano, p_dtSeleccionados.GetValue("code", i))

                    l_intTamano = l_intTamano + 1
                End If
            Next

            MatrixDevolucionDeVehiculos.Matrix.LoadFromDataSource()

            If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    Public Sub ProcesarDevolucion()
        Try
            Dim l_intNumAsiento As Integer
            Dim l_strCod_Unid As String
            Dim l_strTipo_Unid As String
            Dim l_strAsientoDevolucion As String
            Dim l_strCodDevolucion As String
            Dim l_strIDUnidad As String
            Dim l_blnUpdateDoc As Boolean = False
            Dim l_blnUpdateVeh As Boolean = False

            MatrixDevolucionDeVehiculos.Matrix.FlushToDataSource()

            l_strCodDevolucion = txtDocEntry.ObtieneValorDataSource()

            For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).Size - 1

                With _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion)

                    If Not String.IsNullOrEmpty(.GetValue("U_Num_Asiento", i)) AndAlso
                        String.IsNullOrEmpty(.GetValue("U_Num_As_Dev", i)) Then

                        l_intNumAsiento = .GetValue(MatrixDevolucionDeVehiculos.ColumnaAsi.ColumnaLigada, i).ToString.Trim
                        l_strCod_Unid = .GetValue(MatrixDevolucionDeVehiculos.ColumnaUni.ColumnaLigada, i).ToString.Trim
                        l_strTipo_Unid = .GetValue(MatrixDevolucionDeVehiculos.ColumnaTip.ColumnaLigada, i).ToString.Trim
                        l_strIDUnidad = .GetValue(MatrixDevolucionDeVehiculos.ColumnaIdV.ColumnaLigada, i).ToString.Trim


                        If Not _companySbo.InTransaction Then
                            _companySbo.StartTransaction()
                        End If


                        l_strAsientoDevolucion = CrearAsiento(l_strCod_Unid, l_strTipo_Unid, l_intNumAsiento)
                        If Not String.IsNullOrEmpty(l_strAsientoDevolucion) Then
                            l_blnUpdateDoc = ActualizaLineasDevolucion(l_strCodDevolucion, l_strCod_Unid, l_strIDUnidad, l_intNumAsiento, "U_Num_As_Dev", l_strAsientoDevolucion)
                            l_blnUpdateVeh = ActualizarDatosVehiculo(l_strIDUnidad, m_strCodVehDevuelto, "U_Dispo")
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeDevolverVehiculoDevRealizada & l_strCod_Unid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                        End If

                        If String.IsNullOrEmpty(l_strAsientoDevolucion) OrElse
                            l_blnUpdateDoc = False OrElse
                            l_blnUpdateVeh = False Then
                            _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
                        Else
                            _companySbo.EndTransaction(BoWfTransOpt.wf_Commit)
                        End If

                    End If
                End With
                l_blnUpdateDoc = False
                l_blnUpdateVeh = False
            Next

            ' MatrixDevolucionDeVehiculos.Matrix.LoadFromDataSource()

        Catch ex As Exception

            If _companySbo.InTransaction Then
                _companySbo.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function CrearAsiento(ByVal p_strUnidad As String,
                                  ByVal p_strTipoVeh As String,
                                  ByVal p_intAsiento As Integer) As String
        Try

            Dim l_strAsGenerado As String
            Dim l_intError As Integer
            Dim l_strErrorMsj As String
            Dim l_fhaFechaCont As Date
            Dim l_strFechaCont As String
            Dim p_blnDimensiones As Boolean = False

            Dim oJournalEntry As SAPbobsCOM.JournalEntries
            Dim o_JE_Line As SAPbobsCOM.JournalEntries_Lines

            Dim oJournalEntry_Dev As SAPbobsCOM.JournalEntries

            Dim l_strCtaInvVehiculo = ObtenerNumeroCuenta(p_strTipoVeh, m_strUDFCuentaStock)
            Dim l_strCtaDevolucion = ObtenerNumeroCuenta(p_strTipoVeh, m_strUDFCuentaDevolucion)

            l_strFechaCont = txtFechaCont.ObtieneValorDataSource()
            l_fhaFechaCont = DateTime.ParseExact(l_strFechaCont, "yyyyMMdd", CultureInfo.CurrentCulture)

            oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.GetByKey(p_intAsiento)
            oJournalEntry.SetCurrentLine(0)

            oJournalEntry_Dev = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry_Dev.ReferenceDate = l_fhaFechaCont
            oJournalEntry_Dev.Memo = My.Resources.Resource.MensajeDevolverVehAsiento1 & p_strUnidad & My.Resources.Resource.MensajeDevolverVehAsiento & p_intAsiento
            oJournalEntry_Dev.Reference = p_strUnidad

            oJournalEntry_Dev.Lines.AccountCode = l_strCtaInvVehiculo
            oJournalEntry_Dev.Lines.Credit = oJournalEntry.Lines.Credit
            oJournalEntry_Dev.Lines.Reference1 = p_strUnidad

            oJournalEntry_Dev.Lines.Add()
            oJournalEntry_Dev.Lines.AccountCode = l_strCtaDevolucion
            oJournalEntry_Dev.Lines.Debit = oJournalEntry.Lines.Credit
            oJournalEntry_Dev.Lines.Reference1 = p_strUnidad

            If oJournalEntry_Dev.Add <> 0 Then
                ' l_strAsGenerado = "0"
                _companySbo.GetLastError(l_intError, l_strErrorMsj)
            Else
                _companySbo.GetNewObjectCode(l_strAsGenerado)
            End If

            Return l_strAsGenerado


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function EliminarLineasVehiculos()
        Try
            Dim intSelect As Integer
            Dim oMat As SAPbouiCOM.Matrix
            Dim oUnid As String
            Dim list As List(Of String)
            list = New List(Of String)

            Dim l_strCodUnid As String



            oMat = DirectCast(_formularioSBO.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)

            MatrixDevolucionDeVehiculos.Matrix.FlushToDataSource()

            intSelect = oMat.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Do While intSelect > -1
                l_strCodUnid = _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).GetValue("U_Cod_Unid", intSelect - 1).ToString.Trim

                list.Add(l_strCodUnid)

                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)
            Loop

            list.Reverse()

            For Each oUnid In list

                For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).Size - 1
                    Dim tmpUnid As String
                    tmpUnid = _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).GetValue("U_Cod_Unid", i).ToString.Trim

                    If tmpUnid.Equals(oUnid) Then

                        _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).RemoveRecord(i)

                        Exit For
                    End If

                Next
            Next

            MatrixDevolucionDeVehiculos.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ObtenerNumeroCuenta(ByVal p_strTipoInv As String, ByVal p_strCuenta As String) As String
        Try
            Dim l_strResult As String = String.Empty

            dtCuentas = _formularioSBO.DataSources.DataTables.Item("dtCuentas")

            If Not String.IsNullOrEmpty(p_strTipoInv) Then
                For i As Integer = 0 To dtCuentas.Rows.Count - 1
                    If dtCuentas.GetValue("U_Tipo", i).Equals(p_strTipoInv) Then
                        l_strResult = dtCuentas.GetValue(p_strCuenta, i)
                        Exit For
                    End If
                Next
            End If

            Return l_strResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ActualizaLineasDevolucion(ByVal p_strCodDevolucion As String,
                                          ByVal p_strCodUnid As String,
                                          ByVal p_strIDUnid As String,
                                          ByVal p_NumAsiento As String,
                                          ByVal p_strColumna As String,
                                          ByVal p_StrValor As String) As Boolean
        Try

            Dim l_blnResult As Boolean = False

            Dim oCompanyService As SAPbobsCOM.CompanyService
            Dim oGeneralService As SAPbobsCOM.GeneralService
            Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
            Dim oGeneralData As SAPbobsCOM.GeneralData
            Dim oChildrenDevolucion As SAPbobsCOM.GeneralDataCollection
            Dim oChildDevolucion As SAPbobsCOM.GeneralData

            MatrixDevolucionDeVehiculos.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_DDV")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_strCodDevolucion)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oChildrenDevolucion = oGeneralData.Child("SCGD_DEVOLUCION_LIN")

            For j As Integer = 0 To oChildrenDevolucion.Count - 1
                oChildDevolucion = oChildrenDevolucion.Item(j)

                If oChildDevolucion.GetProperty("U_Cod_Unid").Equals(p_strCodUnid) AndAlso
                    oChildDevolucion.GetProperty("U_Id_Veh").Equals(p_strIDUnid) AndAlso
                    oChildDevolucion.GetProperty("U_Num_Asiento").Equals(p_NumAsiento) Then

                    oChildDevolucion.SetProperty(p_strColumna, p_StrValor)

                    oGeneralService.Update(oGeneralData)
                    l_blnResult = True
                    Exit For
                End If
            Next

            Return l_blnResult
        Catch ex As Exception
            Return False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ActualizarDatosVehiculo(ByVal p_strCodeVehiculo As String,
                                              ByVal p_StrValor As String,
                                              ByVal p_StrCampo As String) As Boolean
        Dim l_blnResult As Boolean = False

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildRecepcion As SAPbobsCOM.GeneralData
        Dim oChildrenRecepcion As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try
            If Not String.IsNullOrEmpty(p_strCodeVehiculo) Then

                '  MatrixEntradaPed.Matrix.FlushToDataSource()

                oCompanyService = _companySbo.GetCompanyService()
                oGeneralService = oCompanyService.GetGeneralService("SCGD_VEH")

                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("Code", p_strCodeVehiculo)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                oGeneralData.SetProperty(p_StrCampo, p_StrValor)
                oGeneralService.Update(oGeneralData)
                l_blnResult = True
            End If

            Return l_blnResult
        Catch ex As Exception
            Return l_blnResult = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Public Function ValidarDatos(ByRef bubbleEvent As Boolean) As Boolean
        Try
            Dim l_strResutl As Boolean = False

            If String.IsNullOrEmpty(txtFechaDocumento.ObtieneValorDataSource) Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeDevolverVehSinFecha, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                l_strResutl = False
            ElseIf String.IsNullOrEmpty(txtFechaCont.ObtieneValorDataSource) Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeDevolverVehSinFechaFact, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                l_strResutl = False

            ElseIf ValidarUnidadesDevueltas() Then

                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeDevolverVehUnidadesCreadas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                l_strResutl = False

            ElseIf String.IsNullOrEmpty(txtDocEntry.ObtieneValorDataSource) Then

                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeDevolverVehCrearDocumento, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                l_strResutl = False

            ElseIf _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then

                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeDevolverVehFormularioActualizar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                bubbleEvent = False
                l_strResutl = False

            End If

            Return l_strResutl

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarUnidadesDevueltas() As Boolean
        Try
            Dim l_blnResutl As Boolean = True
            Dim l_strAsiento As String

            MatrixDevolucionDeVehiculos.Matrix.FlushToDataSource()

            For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).Size - 1
                l_strAsiento = _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).GetValue("U_Num_As_Dev", i).ToString.Trim()

                If String.IsNullOrEmpty(l_strAsiento) Then
                    l_blnResutl = False
                    Exit For
                End If
            Next

            Return l_blnResutl

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarEliminarLineas(ByRef bubbleEvent As Boolean) As Boolean
        Try

            Dim intSelect As Integer
            Dim oMat As SAPbouiCOM.Matrix
            Dim l_strAsiento As String
            Dim l_blnResutl As Boolean = True

            oMat = DirectCast(_formularioSBO.Items.Item("mtxVeh").Specific, SAPbouiCOM.Matrix)

            MatrixDevolucionDeVehiculos.Matrix.FlushToDataSource()

            intSelect = oMat.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Do While intSelect > -1

                l_strAsiento = _formularioSBO.DataSources.DBDataSources.Item(m_strTablaLineasDevolucion).GetValue("U_Num_As_Dev", intSelect - 1).ToString.Trim

                If Not String.IsNullOrEmpty(l_strAsiento) Then
                    _applicationSbo.StatusBar.SetText("No puede eliminar de documento Vehiculos que ya han sido devueltos", BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                    l_blnResutl = False
                    bubbleEvent = False
                    Exit Do
                End If

                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)
            Loop

            Return l_blnResutl

        Catch ex As Exception
            bubbleEvent = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function


End Class
