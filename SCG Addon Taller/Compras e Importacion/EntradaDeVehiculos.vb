Imports SAPbouiCOM
Imports SCG.DMSOne.Framework
Imports System.Globalization
Imports DMS_Addon.ControlesSBO
Imports DMSOneFramework.SCGCommon
Imports System.Collections.Generic
Imports SAPbobsCOM
Imports SCG.SBOFramework.DI

Public Class EntradaDeVehiculos : Implements IUsaPermisos


    Private WithEvents l_oSeleccionMarcaEstilo As VehiculoSeleccionMarcaEstilo

#Region "Declaraciones"

    Private m_TablePedidos As SAPbouiCOM.DataTable
    Private m_TableVehiculos As SAPbouiCOM.DataTable

    Private m_dtsVehiculo As New VehiculosAddonDataset
    Private m_tadVehiculo As New VehiculosAddonDatasetTableAdapters.SCG_VEHICULOTableAdapter
    Private m_tadConsultasVehiculos As New VehiculosAddonDatasetTableAdapters.SCG_VEHICULOTableAdapter

    Dim drwVehiculo As VehiculosAddonDataset.SCG_VEHICULORow = Nothing

    Private m_cnConeccionTransaccion As New SqlClient.SqlConnection
    Private m_tnTransaccion As SqlClient.SqlTransaction

    Dim udoVeh As UDOVehiculos = Nothing

    Dim n As NumberFormatInfo
    Dim precision As Decimal

    Private m_strUDFCodUnidad As String = "U_Cod_Uni"
    Private m_strUDFIDUnidad As String = "U_ID_Veh"
    Private m_strUDFCodMarca As String = "U_Cod_Mar"
    Private m_strUDFCodEstilo As String = "U_Cod_Est"
    Private m_strUDFCodModelo As String = "U_Cod_Mod"
    Private m_strUDFCodUbica As String = "U_Cod_Ubi"
    Private m_strUDFEstado As String = "U_Estado"
    Private m_strUDFCodTipo As String = "U_Cod_Tip"
    Private m_strUDFNumVin As String = "U_Num_Vin"
    Private m_strUDFNumMot As String = "U_Num_Mot"
    Private m_strUDFAñoVeh As String = "U_Ano_Veh"
    Private m_strUDFCodColor As String = "U_Cod_Col"
    Private m_StrUDFNumPedido As String = "U_Num_Ped"
    Private m_StrUDFMontoAsiento As String = "U_Monto_Gr"
    Private m_strUDFTipoTransac As String = "U_Tipo_Trans"
    Private m_StrUDFNumEntrada As String = "U_Num_Entrada"
    Private m_strUDFNUmAsiento As String = "U_Num_Asiento"

    Public Const strFLETE As String = "FLETE"
    Public Const strFOB As String = "FOB"
    Public Const strSEGFAC As String = "SEGFAC"
    Public Const strCOMFOR As String = "COMFOR"
    Public Const strCOMNEG As String = "COMNEG"
    Public Const strCIF As String = "CIF"

    Public Const strACCINT As String = "ACCINT"
    Public Const strACCEXT As String = "ACCEXT"
    Public Const strCOMAPE As String = "COMAPE"
    Public Const strSEGLOC As String = "SEGLOC"
    Public Const strTRASLA As String = "TRASLA"
    Public Const strREDEST As String = "REDEST"
    Public Const strBODALM As String = "BODALM"
    Public Const strDESALM As String = "DESALM"
    Public Const strIMPVTA As String = "IMPVTA"
    Public Const strAGENCIA As String = "AGENCIA"
    Public Const strFLELOC As String = "FLELOC"
    Public Const strRESERVA As String = "RESERVA"
    Public Const strOTROS_FP As String = "OTROS_FP"
    Public Const strTALLER As String = "TALLER"

    ' Dim service As CompanyService = _companySbo.GetCompanyService()
    'Dim info As AdminInfo = service.GetAdminInfo()

    Private m_strMonedaOrigen As String
    Private m_strMonedaDestino As String


    Private m_strMonLocal As String
    Private m_strMonSistema As String

    Private m_decTCOrigen As Decimal
    Private m_decTCDestino As Decimal
    Public g_strDocEntry As Decimal

    Private m_strValCodeUnid As String = String.Empty

    Private udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo

    Public m_objCosteo As CosteoCls
    Dim m_blnValidarCrearDoc As Boolean = True


    Enum EnableBtn
        Mostrar = 1
        Ocultar = 2
        Evaluar = 3
    End Enum

#End Region


    Public Sub AgregarPrimerLineaPedidos()
        Try
            MatrixEntradaPed.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Art", 0, String.Empty)
            MatrixEntradaPed.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Public Sub AgregarPrimerLineaUnidades()

        Try

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Cod_Art", 0, String.Empty)
            MatrixEntradaVeh.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Public Sub AgregarLineaSiguentePedidos()

        Dim intSize As Integer

        FormularioSBO.Freeze(True)

        MatrixEntradaPed.Matrix.FlushToDataSource()

        intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size

        Dim l_strTempCod As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", intSize - 1)
        If String.IsNullOrEmpty(l_strTempCod) AndAlso
            intSize - 1 = 0 Then

            AgregarPrimerLineaPedidos()

        ElseIf Not l_strTempCod.Equals(String.Empty) Then
            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).InsertRecord(intSize)
            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Art", intSize, String.Empty)
        End If

        MatrixEntradaPed.Matrix.LoadFromDataSource()

        FormularioSBO.Freeze(False)
    End Sub

    Public Sub AgregarLineaSiguenteUnidades()

        Dim intSize As Integer

        FormularioSBO.Freeze(True)

        MatrixEntradaVeh.Matrix.FlushToDataSource()

        intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size

        Dim l_strTempCod As String = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", intSize - 1)

        If l_strTempCod.Equals(String.Empty) AndAlso
            intSize - 1 = 0 Then

            AgregarPrimerLineaUnidades()

        ElseIf Not l_strTempCod.Equals(String.Empty) Then

            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).InsertRecord(intSize)
            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Cod_Art", intSize, String.Empty)
        End If

        MatrixEntradaVeh.Matrix.LoadFromDataSource()

        FormularioSBO.Freeze(False)
    End Sub

    'Public Sub AgregarLineaPedido()

    '    Dim intSize As Integer

    '    MatrixEntradaPed.Matrix.FlushToDataSource()

    '    intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1

    '    If intSize = 0 AndAlso
    '        String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", 0)) Then
    '        FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).InsertRecord(intSize)
    '    Else
    '        intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size

    '        If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", 0)) Then
    '            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).InsertRecord(intSize)
    '        End If

    '    End If
    '    ' FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).InsertRecord(intSize)

    '    MatrixEntradaPed.Matrix.LoadFromDataSource()

    'End Sub

    'Public Sub AgregarLineaVehiculo()
    '    Dim intSize As Integer

    '    MatrixEntradaVeh.Matrix.FlushToDataSource()

    '    intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size
    '    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).InsertRecord(intSize)

    '    MatrixEntradaVeh.Matrix.LoadFromDataSource()

    'End Sub

#Region "Metodos - Funciones"


    Private Function ValidarDatos(ByRef pval As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean) As Boolean
        Try
            Dim l_blnRes As Boolean = True

            MatrixEntradaPed.Matrix.FlushToDataSource()

            If String.IsNullOrEmpty(txtCodProv.ObtieneValorDataSource) Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosSinProveedor, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_blnRes = False
                bubbleEvent = False
            ElseIf FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1 <= 0 AndAlso
                                String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Num_Ped", 0).Trim) Then

                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosSinLinea, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_blnRes = False
                bubbleEvent = False
            End If

            Return l_blnRes
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarCrearUnidades(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Try

            Dim l_blnRes As Boolean = True
            Dim l_StrDocEntry As String
            Dim l_strDocNum As String
            Dim l_strSQL As String
            Dim l_StrUnidCreadas As String

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            l_StrDocEntry = txtDocEntry.ObtieneValorDataSource()
            l_strDocNum = txtDocNum.ObtieneValorDataSource()
            l_strSQL = "SELECT U_UnidGen FROM [@SCGD_ENTRADA_VEH] where DocEntry = '{0}'"
            l_StrUnidCreadas = Utilitarios.EjecutarConsulta(String.Format(l_strSQL, l_StrDocEntry), _companySbo.CompanyDB, _companySbo.Server)


            If m_blnUsaCostoAuto Then
                If String.IsNullOrEmpty(txtFhaCont.ObtieneValorDataSource) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosSinFechaCont, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    l_blnRes = False
                    BubbleEvent = False
                    Return l_blnRes
                ElseIf ValidarTipoDeTransaccion() Then

                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosFaltaConfTransaccion, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    l_blnRes = False
                    BubbleEvent = False
                    Return l_blnRes
                End If
            End If

            If l_StrUnidCreadas.Equals("Y") AndAlso pVal.ItemUID = btnCrea.UniqueId Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosYaCreados, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                BubbleEvent = False

            ElseIf FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1 <= 0 AndAlso
                String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Num_Ped", 0).Trim) Then

                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosNoHayVeh, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_blnRes = False
                BubbleEvent = False

            ElseIf ValidarCodigoUnidad() Then

                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosYaExisteUnidad & m_strValCodeUnid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                m_strValCodeUnid = String.Empty
                l_blnRes = False
                BubbleEvent = False

            ElseIf ValidarNumeroVIN() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosVinExiste & m_strValCodeUnid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                m_strValCodeUnid = String.Empty
                l_blnRes = False
                BubbleEvent = False

            ElseIf ValidarNumeroMotor() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculoExisteMotor & m_strValCodeUnid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                m_strValCodeUnid = String.Empty
                l_blnRes = False
                BubbleEvent = False
            ElseIf ValidarTipoInventario() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculoSeleccioneInv & m_strValCodeUnid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                m_strValCodeUnid = String.Empty
                l_blnRes = False
                BubbleEvent = False

            ElseIf ValidarMarcaYEstilo() Then

                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculoSinMarcaEstilo & m_strValCodeUnid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                m_strValCodeUnid = String.Empty
                l_blnRes = False
                BubbleEvent = False
            ElseIf ValidarEstadoVenta() Then
                ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculoSinDispobilidad & m_strValCodeUnid, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                m_strValCodeUnid = String.Empty
                l_blnRes = False
                BubbleEvent = False

            End If


            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarGenerarUnidades(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            Dim l_blnResult As Boolean = False

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            If _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1 <= 0 AndAlso
                String.IsNullOrEmpty(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", 0).Trim) Then

                l_blnResult = True

            End If

            Return l_blnResult


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Sub ValidarUnidadExiste(ByRef p_strCodUnid As String)
        Try
            Dim l_strSQL As String

            l_strSQL = "Select Code, U_Cod_Unid from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(String.Format(l_strSQL, p_strCodUnid))
            If Not String.IsNullOrEmpty(dtLocal.GetValue("Code", 0)) Then
                p_strCodUnid = p_strCodUnid & "(1)"
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function ValidarCodigoUnidad() As Boolean
        Try
            Dim l_strUnidCod As String
            Dim l_strCode As String
            Dim l_blnRes As Boolean = True
            Dim l_strSQL As String

            l_strSQL = "Select U_Cod_Unid from [@SCGD_VEHICULO] where U_Cod_Unid = '{0}' AND Code <> '{1}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                l_strUnidCod = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
                l_strCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", i).Trim()

                If Not String.IsNullOrEmpty(l_strUnidCod) Then

                    dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strUnidCod, l_strCode))

                    If String.IsNullOrEmpty(dtLocal.GetValue("U_Cod_Unid", 0)) OrElse
                         dtLocal.GetValue("U_Cod_Unid", 0) = "" Then
                        l_blnRes = False
                    Else
                        l_blnRes = True
                        m_strValCodeUnid = l_strUnidCod
                        Exit For
                    End If
                End If
            Next

            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarNumeroVIN() As Boolean
        Try
            Dim l_strUnidCod As String
            Dim l_strVin As String
            Dim l_strCode As String
            Dim l_blnRes As Boolean = False
            Dim l_strSQL As String

            l_strSQL = "Select U_Cod_Unid from [@SCGD_VEHICULO] where U_Num_Vin = '{0}' AND Code <> '{1}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

                l_strUnidCod = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
                l_strVin = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Num_Vin", i).Trim()
                l_strCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", i).Trim()

                If Not String.IsNullOrEmpty(l_strUnidCod) AndAlso
                    Not String.IsNullOrEmpty(l_strVin) Then

                    dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strVin, l_strCode))

                    If String.IsNullOrEmpty(dtLocal.GetValue("U_Cod_Unid", 0)) OrElse
                         dtLocal.GetValue("U_Cod_Unid", 0) = "" Then
                        l_blnRes = False
                    Else
                        l_blnRes = True
                        m_strValCodeUnid = l_strUnidCod
                        Exit For
                    End If
                End If
            Next

            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarNumeroMotor() As Boolean
        Try
            Dim l_strUnidCod As String
            Dim l_strNumMot As String
            Dim l_strCode As String
            Dim l_blnRes As Boolean = False
            Dim l_strSQL As String

            l_strSQL = "Select U_Cod_Unid from [@SCGD_VEHICULO] where U_Num_Mot = '{0}' AND Code <> '{1}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

                l_strUnidCod = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
                l_strNumMot = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Num_Mot", i).Trim()
                l_strCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", i).Trim()

                If Not String.IsNullOrEmpty(l_strUnidCod) And
                    Not String.IsNullOrEmpty(l_strNumMot) Then

                    dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strNumMot, l_strCode))

                    If String.IsNullOrEmpty(dtLocal.GetValue("U_Cod_Unid", 0)) OrElse
                         dtLocal.GetValue("U_Cod_Unid", 0) = "" Then
                        l_blnRes = False
                    Else
                        l_blnRes = True
                        m_strValCodeUnid = l_strUnidCod
                        Exit For
                    End If
                End If
            Next

            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarTipoInventario() As Boolean
        Try
            Dim l_blnResult As Boolean = False
            Dim l_strUnidCod As String = String.Empty
            Dim l_StrTipoVeh As String = String.Empty

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

                l_strUnidCod = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
                l_StrTipoVeh = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Tip", i).Trim()

                If Not String.IsNullOrEmpty(l_strUnidCod) Then

                    If String.IsNullOrEmpty(l_StrTipoVeh) Then
                        l_blnResult = True
                        m_strValCodeUnid = l_strUnidCod
                        Exit For
                    Else
                        l_blnResult = False

                    End If
                End If
            Next

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Private Function ValidarTipoDeTransaccion() As Boolean
        Try
            Dim l_blnResult As Boolean = False
            Dim l_strSQL As String

            l_strSQL = " Select Code,  U_TipoTransCostAuto from [@SCGD_ADMIN] where Code = 'DMS'"

            dtLocal = _formularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(l_strSQL)

            If String.IsNullOrEmpty(dtLocal.GetValue("U_TipoTransCostAuto", 0)) Then
                l_blnResult = True
            Else
                l_blnResult = False
            End If

            'MatrixEntradaVeh.Matrix.FlushToDataSource()
            'For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

            '    l_strUnidCod = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
            '    l_strTipoTrans = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Tipo_Trans", i).Trim()

            '    If Not String.IsNullOrEmpty(l_strUnidCod) Then

            '        If String.IsNullOrEmpty(l_strTipoTrans) Then
            '            l_blnResult = True
            '            m_strValCodeUnid = l_strUnidCod
            '            Exit For
            '        Else
            '            l_blnResult = False

            '        End If
            '    End If
            'Next

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarMarcaYEstilo() As Boolean
        Try
            Dim l_blnResult As Boolean = True
            Dim l_strUnidCod As String = String.Empty
            Dim l_strCodMarca As String = String.Empty
            Dim l_strCodEstilo As String = String.Empty


            MatrixEntradaVeh.Matrix.FlushToDataSource()
            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

                l_strUnidCod = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
                l_strCodMarca = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Mar", i).Trim()
                l_strCodEstilo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Est", i).Trim()

                If Not String.IsNullOrEmpty(l_strUnidCod) Then

                    If String.IsNullOrEmpty(l_strCodMarca) OrElse
                       String.IsNullOrEmpty(l_strCodEstilo) Then

                        m_strValCodeUnid = l_strUnidCod
                        l_blnResult = True
                        Exit For

                    Else
                        l_blnResult = False

                    End If
                End If
            Next

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Private Function ValidarEstadoVenta() As Boolean
        Try
            Dim l_blnResult As Boolean = False
            Dim l_strUnidCode As String = String.Empty
            Dim l_strEstado As String = String.Empty

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

                l_strUnidCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim()
                l_strEstado = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Estado", i).Trim()


                If Not String.IsNullOrEmpty(l_strUnidCode) Then
                    If String.IsNullOrEmpty(l_strEstado) Then
                        l_blnResult = True
                        m_strValCodeUnid = l_strUnidCode
                        Exit For
                    Else
                        l_blnResult = False
                    End If
                End If
            Next

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Private Function ManejaTipoCambio(ByRef bubbleEvent As Boolean) As Boolean
        Try
            Dim l_blnResult As Boolean = True
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String
            Dim l_FhaConta As Date

            Dim decTC As Decimal
            Dim strTC As String

            Dim l_strMonLocal As String
            Dim l_StrMonSist As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_StrMonSist = dtLocal.GetValue("SysCurrncy", 0)
            End If


            'If m_strMonedaOrigen <> cboMoneda.ObtieneValorDataSource Then

            If cboMoneda.ObtieneValorDataSource() = l_strMonLocal Then
                txtTipoC.AsignaValorDataSource(1)
                FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = False
            ElseIf m_strMonedaOrigen = m_strMonedaDestino Then

            Else

                If Not String.IsNullOrEmpty(txtFhaDoc.ObtieneValorDataSource) Then
                    l_FhaConta = DateTime.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing)
                Else
                    l_FhaConta = Date.Now
                End If

                l_strSQLTipoC = String.Format(l_strSQLTipoC, Utilitarios.RetornaFechaFormatoDB(l_FhaConta, _companySbo.Server), cboMoneda.ObtieneValorDataSource)

                dtLocal.Clear()
                dtLocal.ExecuteQuery(l_strSQLTipoC)

                If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) OrElse dtLocal.GetValue("Rate", 0) = 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioDoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    cboMoneda.AsignaValorDataSource(m_strMonedaOrigen)
                    bubbleEvent = False
                    l_blnResult = False
                Else
                    strTC = dtLocal.GetValue("Rate", 0)
                    decTC = Decimal.Parse(strTC)

                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_TipoCambio", 0, decTC.ToString(n))

                End If
                FormularioSBO.Items.Item(txtTipoC.UniqueId).Visible = True
            End If
            ' End If

            Return l_blnResult
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ObtenerTipoCambio(ByVal p_strMoneda As String, ByVal p_fhaFecha As Date) As Decimal
        Try
            Dim l_decResult As Decimal
            Dim l_strSQL As String
            Dim l_strFhaConta As String

            l_strSQL = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_strFhaConta = Utilitarios.RetornaFechaFormatoDB(p_fhaFecha, CompanySBO.Server)

            dtLocal2 = FormularioSBO.DataSources.DataTables.Item("dtLocal2")
            dtLocal.Clear()

            dtLocal2.ExecuteQuery(String.Format(l_strSQL, l_strFhaConta, p_strMoneda))
            If Not String.IsNullOrEmpty(dtLocal2.GetValue("Currency", 0)) Then
                l_decResult = Decimal.Parse(dtLocal2.GetValue("Rate", 0))
            End If

            If l_decResult = 0 Then
                l_decResult = 1
            End If

            Return l_decResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Sub ValidaTipoCambio(ByRef BubbleEvent As Boolean)
        Try
            Const strConsulta As String = "Select * From ORTT Where RateDate = '{0}'"
            Dim strSQLSys As String

            Dim strFecha As String
            Dim dtFecha As DateTime
            Dim strTipoCambio As String
            Dim dtSistema As System.Data.DataTable
            Dim strTipoCamb As String
            Dim strMon As String
            Dim oForm As SAPbouiCOM.Form

            Dim strSQLMonedaSis As String = "select MainCurncy, SysCurrncy  from OADM"
            Dim strSQLTipoC As String = "select  AD.SysCurrncy, TT.Rate from OADM AD inner JOIN ORTT TT ON TT.Currency = AD.SysCurrncy"
            strSQLTipoC &= " where TT.RateDate = '{0}'"

            dtFecha = Today.Date

            dtSistema = Utilitarios.EjecutarConsultaDataTable(strSQLMonedaSis, _companySbo.CompanyDB, _companySbo.Server)

            strFecha = Utilitarios.RetornaFechaFormatoDB(dtFecha, _companySbo.Server)
            strSQLTipoC = String.Format(strSQLTipoC, strFecha)

            strTipoCamb = Utilitarios.EjecutarConsulta(strSQLTipoC, _companySbo.CompanyDB, _companySbo.Server)

            Dim strLocal As String
            Dim strSistema As String

            strLocal = dtSistema.Rows(0).Item("MainCurncy").ToString
            strSistema = dtSistema.Rows(0).Item("SysCurrncy").ToString

            If Not strLocal.Equals(strSistema) Then

                If String.IsNullOrEmpty(strTipoCamb) Then
                    _applicationSbo.MessageBox(My.Resources.Resource.TipoCambioNoActualizado, BoMessageTime.bmt_Short, My.Resources.Resource.btnOk)
                    BubbleEvent = False
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ActualizaCostosValores()
        Try
            Dim l_decTotal As Decimal
            Dim l_decCosto As Decimal
            Dim l_intCantidad As Decimal
            Dim l_intSuma As Decimal

            Dim l_intSumVeh As Integer
            Dim l_decSumTotal As Decimal

            _formularioSBO.Freeze(True)

            MatrixEntradaPed.Matrix.FlushToDataSource()
            For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1

                If Not String.IsNullOrEmpty(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cant_Ent", i).Trim) Then
                    l_intCantidad = Decimal.Parse(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cant_Ent", i), n)
                Else
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cant_Ent", i, 0)
                    l_intCantidad = 0
                End If

                If Not String.IsNullOrEmpty(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cost_Veh", i).Trim) Then
                    l_decCosto = Decimal.Parse(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cost_Veh", i), n)
                Else
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cost_Veh", i, 0)
                    l_decCosto = 0
                End If

                If l_intCantidad <> 0 Then
                    l_decTotal = l_decCosto * l_intCantidad
                Else
                    l_decTotal = 0
                End If

                _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Total_L", i, l_decTotal.ToString(n))

            Next

            MatrixEntradaPed.Matrix.LoadFromDataSource()

            For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1

                If Not String.IsNullOrEmpty(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", i)) Then
                    l_intSuma = l_intSuma + Decimal.Parse(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cant_Ent", i).Trim, n)
                    l_decSumTotal = l_decSumTotal + Decimal.Parse(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Total_L", i).Trim, n)
                End If

            Next

            _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_Total_Doc", 0, l_decSumTotal.ToString(n))

            _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_Cant_Veh", 0, l_intSuma)
            _formularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaValoresMatArticulos(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            MatrixEntradaPed.Matrix.FlushToDataSource()

            oForm.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Art", pVal.Row - 1, (oDataTable.GetValue("ItemCode", 0)))
            oForm.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Desc_Art", pVal.Row - 1, (oDataTable.GetValue("ItemName", 0)))

            MatrixEntradaPed.Matrix.LoadFromDataSource()

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresProveedor(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)

        Try
            Dim l_strSQL As String
            Dim oitems As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox

            txtCodProv.AsignaValorDataSource(oDataTable.GetValue("CardCode", 0))
            txtNamProv.AsignaValorDataSource(oDataTable.GetValue("CardName", 0))

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQL = "Select CntctCode, Name from OCPR	where CardCode = '{0}'"

            dtLocal.ExecuteQuery(String.Format(l_strSQL, oDataTable.GetValue("CardCode", 0)))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("CntctCode", 0)) Then

                oitems = oForm.Items.Item(cboContact.UniqueId)
                oCombo = CType(oitems.Specific, SAPbouiCOM.ComboBox)

                If oCombo.ValidValues.Count <> 0 Then
                    For i As Integer = 0 To oCombo.ValidValues.Count - 1
                        oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                    Next
                End If

                For i As Integer = 0 To dtLocal.Rows.Count - 1
                    oCombo.ValidValues.Add(dtLocal.GetValue("CntctCode", i), dtLocal.GetValue("Name", i))
                Next

                cboContact.AsignaValorDataSource(dtLocal.GetValue("CntctCode", 0))
            End If

            'cboSource.AsignaValorDataSource("SN")
            'Call CargarMonedaSocio(oDataTable.GetValue("CardCode", 0))
            'Call ActualizaTipoCambio()

            If pVal.ActionSuccess = True AndAlso FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresEncabezado(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Try

            Dim l_intCodPedido As Integer
            Dim l_strSQLEnvcabezado As String
            Dim l_intSiguiente As Integer = 0
            Dim oitem As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim l_strSQL As String
            Dim strTipoC As String
            Dim decTipoC As Decimal

            l_strSQLEnvcabezado = " SELECT DocEntry,DocNum,U_Cod_Prov,U_Name_Prov,U_Enc_Compras,U_Cod_Titular,U_Name_Titular,U_DocCurr,U_DocRate,U_Fha_Pedido," +
                                    " U_Fha_Est_Fabrica, U_Total_Doc, U_Observ, U_Cant_Veh, U_Num_Ref, U_CodContac, U_Fha_Est_Arribo" +
                                    " FROM [@SCGD_PEDIDOS] where DocEntry = '{0}' "

            l_intCodPedido = oDataTable.GetValue("DocEntry", 0)

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(String.Format(l_strSQLEnvcabezado, l_intCodPedido))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("DocEntry", 0)) Then

                FormularioSBO.Freeze(True)

                txtCodProv.AsignaValorDataSource(dtLocal.GetValue("U_Cod_Prov", 0))
                txtNamProv.AsignaValorDataSource(dtLocal.GetValue("U_Name_Prov", 0))
                cboContact.AsignaValorDataSource(dtLocal.GetValue("U_CodContac", 0))
                cboMoneda.AsignaValorDataSource(dtLocal.GetValue("U_DocCurr", 0))

                FormularioSBO.Freeze(False)

            End If

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQL = "Select CntctCode, Name from OCPR	where CardCode = '{0}'"

            dtLocal.ExecuteQuery(String.Format(l_strSQL, txtCodProv.ObtieneValorDataSource))

            If Not String.IsNullOrEmpty(dtLocal.GetValue("CntctCode", 0)) Then

                oitem = oForm.Items.Item(cboContact.UniqueId)
                oCombo = CType(oitem.Specific, SAPbouiCOM.ComboBox)

                If oCombo.ValidValues.Count <> 0 Then
                    For i As Integer = 0 To oCombo.ValidValues.Count - 1
                        oCombo.ValidValues.Remove(oCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                    Next
                End If

                For i As Integer = 0 To dtLocal.Rows.Count - 1
                    oCombo.ValidValues.Add(dtLocal.GetValue("CntctCode", i), dtLocal.GetValue("Name", i))
                Next

                cboContact.AsignaValorDataSource(dtLocal.GetValue("CntctCode", 0))
            End If

            ' Call ManejaCampoMoneda()


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Public Sub AsignaValoresPedidos(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef oDataTable As SAPbouiCOM.DataTable)
        Try
            Dim l_intCodPedido As Integer
            Dim l_strSQLPedidos As String
            Dim l_intSiguiente As Integer = 0
            Dim intCantPendiente As Integer
            Dim numLinea As Integer
            Dim l_decCostoArticulo As Decimal
            Dim l_decTotalLinea As Decimal

            MatrixEntradaPed.Matrix.FlushToDataSource()

            l_strSQLPedidos = " Select PD.DocEntry, PL.U_Cod_Art, PL.U_Desc_Art, PL.U_Ano_Veh, PL.U_Cod_Col, PL.U_Des_Col, PL.U_Cant, PL.U_Cost_Art, PL.U_Cost_Tot, PL.LineId, PL.U_Cant_Rec " +
            " from [@SCGD_PEDIDOS] PD  with (nolock)" +
            " inner join [@SCGD_PEDIDOS_LINEAS] PL  with (nolock) on PD.DocEntry = PL.DocEntry " +
            " WHERE PD.DocEntry = '{0}' "

            l_intCodPedido = oDataTable.GetValue("DocEntry", 0)

            m_TablePedidos.Clear()
            m_TablePedidos.ExecuteQuery(String.Format(l_strSQLPedidos, l_intCodPedido))

            '  numLinea = MatrixEntradaPed.Matrix.RowCount
            ' l_intSiguiente = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1

            If FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1 = 0 AndAlso
                String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Num_Ped", 0).Trim()) Then
                numLinea = 0
            ElseIf FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art",
                                                                                               FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1).Trim().Equals(String.Empty) Then
                numLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
            Else
                numLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size
            End If


            If Not String.IsNullOrEmpty(m_TablePedidos.GetValue("DocEntry", 0)) Then
                For i As Integer = 0 To m_TablePedidos.Rows.Count - 1

                    If Not String.IsNullOrEmpty(m_TablePedidos.GetValue("U_Cod_Art", i)) Then
                        intCantPendiente = Decimal.Parse(m_TablePedidos.GetValue("U_Cant", i)) - Decimal.Parse(m_TablePedidos.GetValue("U_Cant_Rec", i))
                        If intCantPendiente > 0 Then

                            With FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed)

                                l_decCostoArticulo = m_TablePedidos.GetValue("U_Cost_Art", i)
                                l_decTotalLinea = m_TablePedidos.GetValue("U_Cost_Tot", i)

                                If numLinea = 0 Then
                                    .SetValue("U_Num_Ped", 0, m_TablePedidos.GetValue("DocEntry", i))
                                    .SetValue("U_Cod_Art", 0, m_TablePedidos.GetValue("U_Cod_Art", i))
                                    .SetValue("U_Desc_Art", 0, m_TablePedidos.GetValue("U_Desc_Art", i))
                                    .SetValue("U_Ano_Veh", 0, m_TablePedidos.GetValue("U_Ano_Veh", i))
                                    .SetValue("U_Cod_Col", 0, m_TablePedidos.GetValue("U_Cod_Col", i))
                                    .SetValue("U_Cant_Ent", 0, intCantPendiente)
                                    .SetValue("U_Cant_Veh", 0, intCantPendiente)
                                    .SetValue("U_Line_Ref", 0, m_TablePedidos.GetValue("LineId", i))
                                    .SetValue("U_Cost_Veh", 0, l_decCostoArticulo.ToString(n))
                                    .SetValue("U_Total_L", 0, l_decTotalLinea.ToString(n))
                                    numLinea += 1

                                Else

                                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).InsertRecord(numLinea)
                                    '  numLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size

                                    .SetValue("U_Num_Ped", numLinea, m_TablePedidos.GetValue("DocEntry", i))
                                    .SetValue("U_Cod_Art", numLinea, m_TablePedidos.GetValue("U_Cod_Art", i))
                                    .SetValue("U_Desc_Art", numLinea, m_TablePedidos.GetValue("U_Desc_Art", i))
                                    .SetValue("U_Ano_Veh", numLinea, m_TablePedidos.GetValue("U_Ano_Veh", i))
                                    .SetValue("U_Cod_Col", numLinea, m_TablePedidos.GetValue("U_Cod_Col", i))
                                    .SetValue("U_Cant_Ent", numLinea, intCantPendiente)
                                    .SetValue("U_Cant_Veh", numLinea, intCantPendiente)
                                    .SetValue("U_Line_Ref", numLinea, m_TablePedidos.GetValue("LineId", i))
                                    .SetValue("U_Cost_Veh", numLinea, l_decCostoArticulo.ToString(n))
                                    .SetValue("U_Total_L", numLinea, l_decTotalLinea.ToString(n))
                                    numLinea += 1


                                End If
                            End With


                        End If

                    End If
                Next
            End If

            MatrixEntradaPed.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub GenerarUnidades()
        Try

            Dim l_strPrefijo As String
            Dim l_intConsecutivo As Integer
            Dim l_intConscutivoUnid As Integer = 0
            Dim l_intVehPorLinea As Integer
            Dim l_pos As Integer
            Dim numLinea As Integer
            Dim l_Disponibilidad As String
            Dim l_Ubicacion As String
            Dim l_TipoVeh As String
            Dim l_Color As String
            Dim l_Año As String
            Dim l_Articulo As String
            Dim l_Num_Ped As String
            Dim l_LineRef As String
            Dim l_strCodMarc As String = String.Empty
            Dim l_strDesMarc As String = String.Empty
            Dim l_StrCodEsti As String = String.Empty
            Dim l_StrDesEsti As String = String.Empty
            Dim l_StrCodMode As String = String.Empty
            Dim l_StrDesMode As String = String.Empty
            Dim l_strCodEntrada As String = String.Empty
            Dim l_strTipoTrans As String = String.Empty
            Dim l_decMontoLinea As String = 0
            Dim l_strMarcaCom As String
            Dim l_strSQL As String
            Dim tmpUnid As String

            l_strPrefijo = txtPrefijo.ObtieneValorUserDataSource
            ' l_intConsecutivo = txtConsecutivo.ObtieneValorUserDataSource

            l_strSQL = "SELECT EXM.Code, EXM.Name,U_Cod_Marca, MAR.Name DesMar,U_Cod_Estilo,EST.Name DesEst, U_Cod_Modelo,mo.U_Descripcion DesMod, U_Num_Cili,U_Cant_Puerta,U_Cant_Pasaj,U_Cant_Ejes,U_Peso,U_Cilindrada,U_Potencia,U_Categoria,U_Marca_Mot,U_Transmis,U_Carroceria,U_Tipo_Trac,U_Tipo_Cabina,U_Combusti,U_GarantKM,U_GarantTM,U_Tipo_Techo,U_Cod_MarComer " +
                        " FROM [@SCGD_ESPEXMODE] EXM with (nolock) " +
                        " left outer join [@SCGD_MARCA] MAR with (nolock) on EXM.U_Cod_Marca = Mar.Code " +
                        " left outer join [@SCGD_ESTILO] EST with (nolock) on EXM.U_Cod_Estilo = EST.Code" +
                        " left outer join [@SCGD_MODELO] MO with (nolock) on EXm.U_Cod_Modelo = MO.Code " +
                        " where U_Cod_MarComer = '{0}'"

            MatrixEntradaPed.Matrix.FlushToDataSource()
            MatrixEntradaVeh.Matrix.FlushToDataSource()

            numLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
            l_pos = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size

            If String.IsNullOrEmpty(txtConsecutivo.ObtieneValorUserDataSource) Then
                l_intConscutivoUnid = 1
            Else
                l_intConscutivoUnid = txtConsecutivo.ObtieneValorUserDataSource()
            End If

            For i As Integer = 0 To MatrixEntradaPed.Matrix.RowCount - 1

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", i)) Then

                    l_intVehPorLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cant_Ent", i)
                    l_decMontoLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cost_Veh", i)

                    If l_intVehPorLinea > 0 Then

                        l_Disponibilidad = cboDisponibilidad.ObtieneValorDataSource()
                        l_Ubicacion = cboUbica.ObtieneValorDataSource()
                        l_Color = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Col", i).Trim
                        l_TipoVeh = cboTipoInv.ObtieneValorDataSource()
                        l_Año = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Ano_Veh", i).Trim
                        l_Articulo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", i).Trim
                        l_Num_Ped = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Num_Ped", i).Trim
                        l_LineRef = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Line_Ref", i).Trim
                        l_strMarcaCom = Utilitarios.EjecutarConsulta(String.Format("Select Name from [@SCGD_CONF_ART_VENTA] with (nolock) where U_ArtVent = '{0}'", l_Articulo), _companySbo.CompanyDB, _companySbo.Server)

                        dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
                        dtLocal.Clear()

                        dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strMarcaCom))

                        If Not String.IsNullOrEmpty(dtLocal.GetValue("Code", 0)) Then
                            l_strCodMarc = dtLocal.GetValue("U_Cod_Marca", 0)
                            l_strDesMarc = dtLocal.GetValue("DesMar", 0)
                            l_StrCodEsti = dtLocal.GetValue("U_Cod_Estilo", 0)
                            l_StrDesEsti = dtLocal.GetValue("DesEst", 0)
                            l_StrCodMode = dtLocal.GetValue("U_Cod_Modelo", 0)
                            l_StrDesMode = dtLocal.GetValue("DesMod", 0)
                        End If

                        For j As Integer = 0 To l_intVehPorLinea - 1
                            tmpUnid = l_strPrefijo & "" & l_intConscutivoUnid
                            Call ValidarUnidadExiste(tmpUnid)

                            With (FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi))
                                If numLinea = 0 Then
                                    .SetValue("U_Cod_Uni", 0, tmpUnid)
                                    .SetValue("U_Cod_Col", 0, l_Color)
                                    .SetValue("U_Cod_Ubi", 0, l_Ubicacion)
                                    .SetValue("U_Estado", 0, l_Disponibilidad)
                                    .SetValue("U_Ano_Veh", 0, l_Año)
                                    .SetValue("U_Cod_Art", 0, l_Articulo)
                                    .SetValue("U_Num_Ped", 0, l_Num_Ped)
                                    .SetValue("U_Line_Ref", 0, l_LineRef)
                                    .SetValue("U_Cod_Mar", 0, l_strCodMarc)
                                    .SetValue("U_Cod_Est", 0, l_StrCodEsti)
                                    .SetValue("U_Cod_Mod", 0, l_StrCodMode)
                                    .SetValue("U_Des_Mar", 0, l_strDesMarc)
                                    .SetValue("U_Des_Est", 0, l_StrDesEsti)
                                    .SetValue("U_Des_Mod", 0, l_StrDesMode)
                                    .SetValue("U_Cod_Tip", 0, l_TipoVeh)
                                    .SetValue("U_Tipo_Trans", 0, l_strTipoTrans)
                                    .SetValue("U_Monto_Gr", 0, l_decMontoLinea.ToString(n))
                                    numLinea += 1
                                Else
                                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).InsertRecord(l_pos)
                                    l_pos += 1
                                    .SetValue("U_Cod_Uni", l_pos - 1, tmpUnid)
                                    .SetValue("U_Cod_Col", l_pos - 1, l_Color)
                                    .SetValue("U_Cod_Ubi", l_pos - 1, l_Ubicacion)
                                    .SetValue("U_Estado", l_pos - 1, l_Disponibilidad)
                                    .SetValue("U_Ano_Veh", l_pos - 1, l_Año)
                                    .SetValue("U_Cod_Art", l_pos - 1, l_Articulo)
                                    .SetValue("U_Num_Ped", l_pos - 1, l_Num_Ped)
                                    .SetValue("U_Line_Ref", l_pos - 1, l_LineRef)
                                    .SetValue("U_Cod_Mar", l_pos - 1, l_strCodMarc)
                                    .SetValue("U_Cod_Est", l_pos - 1, l_StrCodEsti)
                                    .SetValue("U_Cod_Mod", l_pos - 1, l_StrCodMode)
                                    .SetValue("U_Des_Mar", l_pos - 1, l_strDesMarc)
                                    .SetValue("U_Des_Est", l_pos - 1, l_StrDesEsti)
                                    .SetValue("U_Des_Mod", l_pos - 1, l_StrDesMode)
                                    .SetValue("U_Cod_Tip", l_pos - 1, l_TipoVeh)
                                    .SetValue("U_Tipo_Trans", l_pos - 1, l_strTipoTrans)
                                    .SetValue("U_Monto_Gr", l_pos - 1, l_decMontoLinea.ToString(n))
                                End If
                            End With
                            l_intConscutivoUnid += 1
                        Next
                        l_strCodMarc = String.Empty
                        l_StrCodEsti = String.Empty
                        l_StrCodMode = String.Empty
                        l_strDesMarc = String.Empty
                        l_StrDesEsti = String.Empty
                        l_StrDesMode = String.Empty
                        l_strMarcaCom = String.Empty
                        dtLocal.Clear()
                    End If
                End If
            Next
            MatrixEntradaVeh.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function CrearDatosMaestrosVehiculos() As Boolean
        Dim l_result As Boolean = False

        Try
            Dim l_strIDVeh As String
            Dim l_strCodUnid As String
            Dim l_strCodMarca As String
            Dim l_strCodEstilo As String
            Dim l_strCodModelo As String
            Dim l_strCodColor As String
            Dim l_strCodUbicacion As String
            Dim l_strCodTipo As String
            Dim l_strVIN As String
            Dim l_strMotor As String
            Dim l_strColorDes As String = String.Empty
            Dim l_strSQL As String
            Dim strIDVehiculo As String

            Dim int_Cilindros As Integer = 0
            Dim int_Puertas As Integer = 0
            Dim int_Pasajeros As Integer = 0
            Dim int_Ejes As Integer = 0
            Dim int_Peso As Integer = 0
            Dim int_GarantKM As Integer = 0
            Dim int_GarantTM As Integer = 0
            Dim int_Año As Integer = 0
            Dim int_CodDispo As Integer = 0
            Dim int_Cilindrada As Integer = 0
            Dim int_Potencia As Integer = 0

            Dim str_Categoria As String = String.Empty
            Dim str_Marca_Mot As String = String.Empty
            Dim str_Transmis As String = String.Empty
            Dim str_Carroceria As String = String.Empty
            Dim str_Tipo_Trac As String = String.Empty
            Dim str_Tipo_Cabina As String = String.Empty
            Dim str_Combusti As String = String.Empty
            Dim str_Tipo_Techo As String = String.Empty
            Dim str_Des_MarComer As String = String.Empty
            Dim str_Cod_MarComer As String = String.Empty
            Dim str_IDVehiculo As String = String.Empty

            Dim str_DescMarca As String = String.Empty
            Dim str_DescEstilo As String = String.Empty
            Dim str_DescModelo As String = String.Empty

            Dim l_StrSQLVeh As String
            Dim l_strSQLMarcaC As String
            Dim str_año As String
            Dim str_Dispo As String
            Dim l_strDocEntry As String = String.Empty
            Dim l_strDocNum As String = String.Empty
            Dim l_strMarcaCom As String = String.Empty
            Dim l_strCodPedido As String = String.Empty

            Dim oListaAsientos As New List(Of DatosVehiculoCosteo)()
            Dim oLineaAsiento As New DatosVehiculoCosteo()

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            udoVeh = Nothing

            l_strSQL = "SELECT Code, Name, U_Cod_Marca,U_Cod_Modelo,U_Cod_Estilo,U_Num_Cili,U_Cant_Puerta,U_Cant_Pasaj,U_Cant_Ejes,U_Peso,U_Cilindrada," +
                "U_Potencia,U_Categoria,U_Marca_Mot,U_Transmis,U_Carroceria,U_Tipo_Trac,U_Tipo_Cabina,U_Combusti,U_GarantKM,U_GarantTM,U_Tipo_Techo,U_Cod_MarComer " +
                " FROM [@SCGD_ESPEXMODE] " +
                " where U_Cod_MarComer = '{0}'"

            l_strSQLMarcaC = "SELECT Code, Name, U_ArtVent from [@SCGD_CONF_ART_VENTA] where U_ArtVent = '{0}'"

            l_StrSQLVeh = "Select Code FROM [@SCGD_VEHICULO] where U_Cod_Unid = '{0}'"

            For i As Integer = 0 To MatrixEntradaVeh.Matrix.RowCount - 1
                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i)) Then

                    l_strMarcaCom = Utilitarios.EjecutarConsulta(String.Format("Select Name from [@SCGD_CONF_ART_VENTA] with (nolock) where U_ArtVent = '{0}'",
                                                                               FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Art", i).Trim()),
                                                                     _companySbo.CompanyDB,
                                                                     _companySbo.Server)

                    dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
                    dtLocal.Clear()

                    dtLocal2 = FormularioSBO.DataSources.DataTables.Item("dtLocal2")
                    dtLocal2.Clear()

                    dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strMarcaCom))

                    dtLocal2.ExecuteQuery(String.Format(l_strSQLMarcaC,
                                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Art", i).Trim()))

                    'para las especificaciones por modelo \ Estilo
                    If Not String.IsNullOrEmpty(dtLocal.GetValue("Code", 0)) Then

                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Num_Cili", 0)) Then
                            int_Cilindros = 0
                        Else
                            int_Cilindros = dtLocal.GetValue("U_Num_Cili", 0)
                        End If

                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Cant_Puerta", 0)) Then
                            int_Puertas = 0
                        Else
                            int_Puertas = dtLocal.GetValue("U_Cant_Puerta", 0)
                        End If

                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Cant_Pasaj", 0)) Then
                            int_Pasajeros = 0
                        Else
                            int_Pasajeros = dtLocal.GetValue("U_Cant_Pasaj", 0)
                        End If

                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Cant_Ejes", 0)) Then
                            int_Ejes = 0
                        Else
                            int_Ejes = dtLocal.GetValue("U_Cant_Ejes", 0)
                        End If

                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Peso", 0)) Then
                            int_Peso = 0
                        Else
                            int_Peso = dtLocal.GetValue("U_Peso", 0)
                        End If
                        If String.IsNullOrEmpty(dtLocal.GetValue("U_GarantKM", 0)) Then
                            int_GarantKM = 0
                        Else
                            int_GarantKM = dtLocal.GetValue("U_GarantKM", 0)
                        End If

                        If String.IsNullOrEmpty(dtLocal.GetValue("U_GarantTM", 0)) Then
                            int_GarantTM = 0
                        Else
                            int_GarantTM = dtLocal.GetValue("U_GarantTM", 0)
                        End If
                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Cilindrada", 0)) Then
                            int_Cilindrada = 0
                        Else
                            int_Cilindrada = dtLocal.GetValue("U_Cilindrada", 0)
                        End If
                        If String.IsNullOrEmpty(dtLocal.GetValue("U_Potencia", 0)) Then
                            int_Potencia = 0
                        Else
                            int_Potencia = dtLocal.GetValue("U_Potencia", 0)
                        End If
                        str_Categoria = dtLocal.GetValue("U_Categoria", 0)
                        str_Marca_Mot = dtLocal.GetValue("U_Marca_Mot", 0)
                        str_Transmis = dtLocal.GetValue("U_Transmis", 0)
                        str_Carroceria = dtLocal.GetValue("U_Carroceria", 0)
                        str_Tipo_Trac = dtLocal.GetValue("U_Tipo_Trac", 0)
                        str_Tipo_Cabina = dtLocal.GetValue("U_Tipo_Cabina", 0)
                        str_Combusti = dtLocal.GetValue("U_Combusti", 0)
                        str_Tipo_Techo = dtLocal.GetValue("U_Tipo_Techo", 0)

                    Else
                        int_Cilindros = Nothing
                        int_Puertas = Nothing
                        int_Pasajeros = Nothing
                        int_Ejes = Nothing
                        int_Peso = Nothing
                        int_Cilindrada = Nothing
                        int_Potencia = Nothing
                        str_Categoria = String.Empty
                        str_Marca_Mot = String.Empty
                        str_Transmis = String.Empty
                        str_Carroceria = String.Empty
                        str_Tipo_Trac = String.Empty
                        str_Tipo_Cabina = String.Empty
                        str_Combusti = String.Empty
                        int_GarantKM = 0
                        int_GarantTM = 0
                        str_Tipo_Techo = String.Empty
                        str_Des_MarComer = String.Empty
                        str_Cod_MarComer = String.Empty

                    End If
                    ' Para la marca comercial
                    If Not String.IsNullOrEmpty(dtLocal2.GetValue("Code", 0)) Then
                        str_Des_MarComer = dtLocal2.GetValue("Name", 0)
                        str_Cod_MarComer = dtLocal2.GetValue("Code", 0)
                    Else
                        str_Des_MarComer = String.Empty
                        str_Cod_MarComer = Nothing
                    End If


                    l_strCodUnid = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i).Trim()

                    If Not String.IsNullOrEmpty(l_strCodUnid) Then
                        l_strDocEntry = txtDocEntry.ObtieneValorDataSource()
                        l_strDocNum = txtDocNum.ObtieneValorDataSource()
                        l_strCodColor = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodColor, i).Trim()
                        l_strColorDes = DevuelveDescripcionColor(l_strCodColor).Trim()

                        l_strCodMarca = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodMarca, i).Trim()
                        l_strCodEstilo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodEstilo, i).Trim()
                        l_strCodModelo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodModelo, i).Trim()
                        l_strCodPedido = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_StrUDFNumPedido, i).Trim()

                        str_DescMarca = ObtenerDescMarca(l_strCodMarca)
                        str_DescEstilo = ObtenerDescEstilo(l_strCodEstilo)
                        str_DescModelo = ObtenerDescModelo(l_strCodModelo)

                        l_strCodUbicacion = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUbica, i).Trim()
                        str_Dispo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFEstado, i).Trim()
                        If Not String.IsNullOrEmpty(str_Dispo) Then
                            int_CodDispo = Integer.Parse(str_Dispo)
                        End If
                        'l_intCodDispo = Integer.Parse(IIf(.GetValue(m_strUDFEstado, i).Trim() = "", 0, .GetValue(m_strUDFEstado, i).Trim()))
                        l_strCodTipo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodTipo, i).Trim()
                        l_strVIN = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNumVin, i).Trim()
                        l_strMotor = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNumMot, i).Trim()
                        str_año = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFAñoVeh, i).Trim()
                        If Not String.IsNullOrEmpty(str_año) Then
                            int_Año = Integer.Parse(str_año)
                        End If

                        strIDVehiculo = CStr(DevuelveCodigoVehiculo())

                        InsertarVehiculoUDO(strIDVehiculo, l_strCodTipo, int_Año, l_strCodColor, l_strCodEstilo,
                                            l_strCodMarca, l_strCodModelo, l_strCodUnid, l_strColorDes, int_CodDispo,
                                            l_strMotor, l_strVIN, l_strCodUbicacion, int_Cilindros, int_Puertas, int_Pasajeros,
                                            int_Ejes, int_Peso, int_Cilindrada, int_Potencia, str_Categoria, str_Marca_Mot, str_Transmis,
                                            str_Carroceria, str_Tipo_Trac, str_Tipo_Cabina, str_Combusti, int_GarantKM, int_GarantTM, str_Tipo_Techo,
                                            str_DescMarca, str_DescEstilo, str_DescModelo, str_Des_MarComer, str_Cod_MarComer, l_strDocNum, l_strCodPedido)

                        ' ActualizaLineasUnidadesEnRecepcion(l_strCodUnid, txtDocNum.ObtieneValorDataSource, strIDVehiculo, "U_ID_Veh")
                        FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_ID_Veh", i, strIDVehiculo)

                    End If
                End If

                l_strCodColor = String.Empty
                l_strColorDes = String.Empty

                l_strCodMarca = String.Empty
                l_strCodEstilo = String.Empty
                l_strCodModelo = String.Empty

                str_DescMarca = String.Empty
                str_DescEstilo = String.Empty
                str_DescModelo = String.Empty

                l_strCodUbicacion = String.Empty
                int_CodDispo = 0
                int_Año = 0
                str_Dispo = String.Empty
                l_strCodTipo = String.Empty
                l_strVIN = String.Empty
                l_strMotor = String.Empty

                str_año = String.Empty
                l_strIDVeh = String.Empty
                dtLocal.Clear()

            Next
            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaUnidadesCreadas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
            l_result = True

            Return l_result
        Catch ex As Exception
            l_result = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function CrearAsiento(ByVal p_oListaAsientos As DatosVehiculoCosteo) As String
        Try

            Dim l_strAsGenerado As String
            Dim l_intError As Integer
            Dim l_strErrorMsj As String
            Dim l_fhaFechaCont As Date
            Dim l_strFechaCont As String
            Dim p_blnDimensiones As Boolean = False

            Dim oJournalEntry As SAPbobsCOM.JournalEntries

            l_strFechaCont = txtFhaDoc.ObtieneValorDataSource()
            l_fhaFechaCont = DateTime.ParseExact(l_strFechaCont, "yyyyMMdd", CultureInfo.CurrentCulture)

            oJournalEntry = _companySbo.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
            oJournalEntry.Reference = p_oListaAsientos.CodigoUnid
            oJournalEntry.ReferenceDate = l_fhaFechaCont
            oJournalEntry.Memo = My.Resources.Resource.RegistroDiarioMemoEntrada & " " & p_oListaAsientos.CodigoUnid

            oJournalEntry.Lines.AccountCode = p_oListaAsientos.CuentaCredito
            oJournalEntry.Lines.Credit = p_oListaAsientos.MontoAsientoLocal
            oJournalEntry.Lines.Reference1 = p_oListaAsientos.CodigoUnid
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO
            oJournalEntry.Lines.Add()

            oJournalEntry.Lines.AccountCode = p_oListaAsientos.CuentaDebito
            oJournalEntry.Lines.Debit = p_oListaAsientos.MontoAsientoLocal
            oJournalEntry.Lines.Reference1 = p_oListaAsientos.CodigoUnid
            oJournalEntry.Lines.VatLine = SAPbobsCOM.BoYesNoEnum.tNO

            If oJournalEntry.Add <> 0 Then
                l_strAsGenerado = "0"
                _companySbo.GetLastError(l_intError, l_strErrorMsj)
                Throw New ExceptionsSBO(l_intError, l_strErrorMsj)
            Else
                _companySbo.GetNewObjectCode(l_strAsGenerado)
            End If

            Return l_strAsGenerado

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Sub CrearEntradasCosteo(ByRef p_oListaAsientos As List(Of DatosVehiculoCosteo))

        Dim p_strCampoNombreTrasaccion As String = String.Empty
        Dim intAsientoEntrada As Integer = 0
        Dim blnCreacionEntrada As Boolean = False
        Dim decMontoLocal As Decimal = 0
        Dim decMontoSistema As Decimal = 0
        Dim l_strFechaCont As String
        Dim l_fhaFechaCont As Date
        Dim l_strTipoTransaccion As String
        Dim CIFLocal As Decimal
        Dim CIFSistema As Decimal
        Dim m_oLineaCosteo As ListaValoresCosteo

        Try

            l_strFechaCont = txtFhaDoc.ObtieneValorDataSource()
            l_fhaFechaCont = DateTime.ParseExact(l_strFechaCont, "yyyyMMdd", CultureInfo.CurrentCulture)
            udoEntrada = New SCG.DMSOne.Framework.UDOEntradaVehiculo(_companySbo)

            For Each row As DatosVehiculoCosteo In p_oListaAsientos

                m_oLineaCosteo = New ListaValoresCosteo()

                l_strTipoTransaccion = row.TipoTransaccion

                DatosEncabezadoEntrada(row.CodigoUnid, row.CodigoMarca, row.CodigoEstilo, row.CodigoModelo, row.NumVIN, row.IdUnid, row.TipoInventario, "", row.NumeroRecepcion, row.NumeroPedido, udoEntrada, l_fhaFechaCont)

                decMontoLocal = row.MontoAsientoLocal
                decMontoSistema = row.MontoAsientoSistema

                If row.MonedaRegistro.Equals(m_strMonLocal) Then
                    udoEntrada.Encabezado.Tot_Loc = decMontoLocal

                    Select Case l_strTipoTransaccion
                        Case strFOB
                            udoEntrada.Encabezado.FOB = udoEntrada.Encabezado.FOB + decMontoLocal
                            CIFLocal = CIFLocal + decMontoLocal
                        Case strFLETE
                            udoEntrada.Encabezado.FLETE = udoEntrada.Encabezado.FLETE + decMontoLocal
                            CIFLocal = CIFLocal + decMontoLocal
                        Case strSEGFAC
                            udoEntrada.Encabezado.SEGFAC = udoEntrada.Encabezado.SEGFAC + decMontoLocal
                            CIFLocal = CIFLocal + decMontoLocal
                        Case strCOMFOR
                            udoEntrada.Encabezado.COMFOR = udoEntrada.Encabezado.COMFOR + decMontoLocal
                            CIFLocal = CIFLocal + decMontoLocal
                        Case strCOMNEG
                            udoEntrada.Encabezado.COMNEG = udoEntrada.Encabezado.COMNEG + decMontoLocal
                            CIFLocal = CIFLocal + decMontoLocal
                        Case strCIF
                            CIFLocal = CIFLocal + decMontoLocal
                        Case strACCINT
                            udoEntrada.Encabezado.ACCINT = udoEntrada.Encabezado.ACCINT + decMontoLocal
                        Case strACCEXT
                            udoEntrada.Encabezado.ACCEXT = udoEntrada.Encabezado.ACCEXT + decMontoLocal
                        Case strCOMAPE 'Comisión Apertura
                            udoEntrada.Encabezado.COMAPE = udoEntrada.Encabezado.COMAPE + decMontoLocal
                        Case strSEGLOC 'Seguros locales
                            udoEntrada.Encabezado.SEGLOC = udoEntrada.Encabezado.SEGLOC + decMontoLocal
                        Case strTRASLA 'Traslado
                            udoEntrada.Encabezado.TRASLA = udoEntrada.Encabezado.TRASLA + decMontoLocal
                        Case strREDEST 'Redestino
                            udoEntrada.Encabezado.REDEST = udoEntrada.Encabezado.REDEST + decMontoLocal
                        Case strBODALM 'Bodega almacen fiscal
                            udoEntrada.Encabezado.BODALM = udoEntrada.Encabezado.BODALM + decMontoLocal
                        Case strDESALM 'Desalmacenaje
                            udoEntrada.Encabezado.DESALM = udoEntrada.Encabezado.DESALM + decMontoLocal
                        Case strIMPVTA 'Impuesto
                            udoEntrada.Encabezado.IMPVTA = udoEntrada.Encabezado.IMPVTA + decMontoLocal
                        Case strAGENCIA 'Agencia
                            udoEntrada.Encabezado.AGENCIA = udoEntrada.Encabezado.AGENCIA + decMontoLocal
                        Case strFLELOC 'Flete Local
                            udoEntrada.Encabezado.FLELOC = udoEntrada.Encabezado.FLELOC + decMontoLocal
                        Case strRESERVA   'Reserva
                            udoEntrada.Encabezado.RESERVA = udoEntrada.Encabezado.RESERVA + decMontoLocal
                        Case strOTROS_FP
                            udoEntrada.Encabezado.OTROS = udoEntrada.Encabezado.OTROS + decMontoLocal
                        Case strTALLER
                            udoEntrada.Encabezado.TALLER = udoEntrada.Encabezado.TALLER + decMontoLocal
                        Case "SaldoInicial"
                            udoEntrada.Encabezado.VALHAC = decMontoLocal
                    End Select

                ElseIf row.MonedaRegistro.Equals(m_strMonSistema) Then
                    udoEntrada.Encabezado.Tot_Sis = decMontoSistema

                    Select Case l_strTipoTransaccion

                        Case strFOB
                            udoEntrada.Encabezado.FOB_S = udoEntrada.Encabezado.FOB_S + decMontoSistema
                            CIFSistema = CIFSistema + decMontoSistema
                        Case strFLETE
                            udoEntrada.Encabezado.FLETE_S = udoEntrada.Encabezado.FLETE_S + decMontoSistema
                            CIFSistema = CIFSistema + decMontoSistema
                        Case strSEGFAC
                            udoEntrada.Encabezado.SEGFAC_S = udoEntrada.Encabezado.SEGFAC_S + decMontoSistema
                            CIFSistema = CIFSistema + decMontoSistema
                        Case strCOMFOR
                            udoEntrada.Encabezado.COMFOR_S = udoEntrada.Encabezado.COMFOR_S + decMontoSistema
                            CIFSistema = CIFSistema + decMontoSistema
                        Case strCOMNEG
                            udoEntrada.Encabezado.COMNEG_S = udoEntrada.Encabezado.COMNEG_S + decMontoSistema
                            CIFSistema = CIFSistema + decMontoSistema
                        Case strCIF
                            CIFSistema = CIFSistema + decMontoSistema
                        Case strACCINT
                            udoEntrada.Encabezado.ACCINT_S = udoEntrada.Encabezado.ACCINT_S + decMontoSistema
                        Case strACCEXT
                            udoEntrada.Encabezado.ACCEXT_S = udoEntrada.Encabezado.ACCEXT_S + decMontoSistema
                        Case strCOMAPE 'Comisión Apertura
                            udoEntrada.Encabezado.COMAPE_S = udoEntrada.Encabezado.COMAPE_S + decMontoSistema
                        Case strSEGLOC 'Seguros locales
                            udoEntrada.Encabezado.SEGLOC_S = udoEntrada.Encabezado.SEGLOC_S + decMontoSistema
                        Case strTRASLA 'Traslado
                            udoEntrada.Encabezado.TRASLA_S = udoEntrada.Encabezado.TRASLA_S + decMontoSistema
                        Case strREDEST 'Redestino
                            udoEntrada.Encabezado.REDEST_S = udoEntrada.Encabezado.REDEST_S + decMontoSistema
                        Case strBODALM 'Bodega almacen fiscal
                            udoEntrada.Encabezado.BODALM_S = udoEntrada.Encabezado.BODALM_S + decMontoSistema
                        Case strDESALM 'Desalmacenaje
                            udoEntrada.Encabezado.DESALM_S = udoEntrada.Encabezado.DESALM_S + decMontoSistema
                        Case strIMPVTA 'Impuesto
                            udoEntrada.Encabezado.IMPVTA_S = udoEntrada.Encabezado.IMPVTA_S + decMontoSistema
                        Case strAGENCIA 'Agencia
                            udoEntrada.Encabezado.AGENCI_S = udoEntrada.Encabezado.AGENCI_S + decMontoSistema
                        Case strFLELOC 'Flete Local
                            udoEntrada.Encabezado.FLELOC_S = udoEntrada.Encabezado.FLELOC_S + decMontoSistema
                        Case strRESERVA   'Reserva
                            udoEntrada.Encabezado.RESERVA_S = udoEntrada.Encabezado.RESERVA_S + decMontoSistema
                        Case strOTROS_FP
                            udoEntrada.Encabezado.OTROS_S = udoEntrada.Encabezado.OTROS_S + decMontoSistema
                        Case strTALLER
                            udoEntrada.Encabezado.TALLER_S = udoEntrada.Encabezado.TALLER_S + decMontoSistema
                        Case "SaldoInicial"
                            udoEntrada.Encabezado.VALHAC_S = decMontoSistema

                    End Select
                End If

                m_oLineaCosteo.MonedaRegistro = row.MonedaRegistro
                m_oLineaCosteo.Sistema = row.MontoAsientoSistema
                m_oLineaCosteo.Local = row.MontoAsientoLocal
                m_oLineaCosteo.CodigoTransaccion = row.TipoTransaccion
                m_oLineaCosteo.Memo = "Recepción Unidad : " & row.CodigoUnid & " [" & l_strTipoTransaccion & "]"
                m_oLineaCosteo.Rate = row.TipoCambio

                AgregarLineaCosto(udoEntrada, m_oLineaCosteo)

                udoEntrada.Encabezado.CIF_L = CIFLocal
                udoEntrada.Encabezado.CIF_S = CIFSistema

                udoEntrada.Encabezado.GASTRA = decMontoLocal
                udoEntrada.Encabezado.GASTRA_S = decMontoSistema

                udoEntrada.Encabezado.EsTraslado = "N"

                If CompanySBO.InTransaction = False Then
                    CompanySBO.StartTransaction()
                End If

                intAsientoEntrada = CrearAsiento(row)
                udoEntrada.Encabezado.AsientoEntrada = intAsientoEntrada
                blnCreacionEntrada = udoEntrada.Insert()


                If ActualizarDatosVehiculo(row.IdUnid, Date.ParseExact(txtFhaCont.ObtieneValorDataSource, "yyyymmdd", Nothing), "U_Fha_Ing_Inv") AndAlso
                    ActualizarDatosVehiculo(row.IdUnid, "C", "U_TIPINV") Then

                    If blnCreacionEntrada Then

                        row.NumeroAsiento = intAsientoEntrada
                        row.NumeroEntrada = udoEntrada.Encabezado.DocEntry

                        CompanySBO.EndTransaction(BoWfTransOpt.wf_Commit)
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVEhiculosEntradaCreada & row.CodigoUnid, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else
                        If CompanySBO.InTransaction Then
                            CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)

                        End If
                    End If
                Else
                    If CompanySBO.InTransaction Then
                        CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)

                    End If
                End If


                CIFLocal = 0
                CIFSistema = 0
                decMontoLocal = 0
                decMontoSistema = 0

            Next

        Catch ex As Exception
            If CompanySBO.InTransaction Then
                CompanySBO.EndTransaction(BoWfTransOpt.wf_RollBack)
            End If

            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Sub DatosEncabezadoEntrada(ByVal p_strUnidad As String,
                      ByVal p_strMarca As String,
                      ByVal p_strEstilo As String,
                      ByVal p_strModelo As String,
                      ByVal p_strVIN As String,
                      ByVal p_strIDVehiculo As String,
                      ByVal p_strTipo As String,
                      ByVal strContrato As String,
                      ByVal p_strDocRecepcion As String,
                      ByVal p_strCodigoPedido As String,
                      ByVal udoEntradaVehiculo As SCG.DMSOne.Framework.UDOEntradaVehiculo,
                      Optional ByVal p_fechaDocumento As Date = Nothing,
                      Optional ByVal p_intAsiento As Integer = 0,
                      Optional ByVal p_intContNumEntrada As Integer = 0,
                      Optional ByRef p_intSerie As Integer = 0)

        udoEntradaVehiculo.Encabezado = New SCG.DMSOne.Framework.EncabezadoUDOEntradaVehiculo

        udoEntradaVehiculo.Encabezado.Series = p_intSerie
        udoEntradaVehiculo.Encabezado.NoUnidad = p_strUnidad
        udoEntradaVehiculo.Encabezado.Marca = p_strMarca
        udoEntradaVehiculo.Encabezado.Estilo = p_strEstilo
        udoEntradaVehiculo.Encabezado.Modelo = p_strModelo
        udoEntradaVehiculo.Encabezado.Vin = p_strVIN
        udoEntradaVehiculo.Encabezado.ID_Vehiculo = p_strIDVehiculo
        udoEntradaVehiculo.Encabezado.Tipo = p_strTipo
        udoEntradaVehiculo.Encabezado.DocRecepcion = p_strDocRecepcion
        udoEntradaVehiculo.Encabezado.SCGD_DocSalida = Nothing
        udoEntradaVehiculo.Encabezado.ContratoVenta = strContrato
        udoEntradaVehiculo.Encabezado.DocPedido = p_strCodigoPedido

        If p_fechaDocumento <> Nothing Then
            udoEntradaVehiculo.Encabezado.Fec_Cont = p_fechaDocumento
            udoEntradaVehiculo.Encabezado.CreateDate = p_fechaDocumento
        Else
            udoEntradaVehiculo.Encabezado.Fec_Cont = Date.Now
            udoEntradaVehiculo.Encabezado.CreateDate = Date.Now
        End If

        '  udoEntradaVehiculo.Encabezado.Cambio = m_decTipoCambio

    End Sub

    Private Sub AgregarLineaCosto(ByRef udoEntrada As SCG.DMSOne.Framework.UDOEntradaVehiculo, ByVal linea As ListaValoresCosteo)
        Try

            udoEntrada.ListaLineas = New SCG.DMSOne.Framework.ListaUDOEntradaVehiculo()
            udoEntrada.ListaLineas.LineasUDO = New List(Of ILineaUDO)(1)

            Dim lineaEntrada As SCG.DMSOne.Framework.LineaUDOEntradaVehiculo = New SCG.DMSOne.Framework.LineaUDOEntradaVehiculo()

            lineaEntrada.Concepto = linea.Memo
            lineaEntrada.Cuenta = linea.AcctCode
            lineaEntrada.Mon_Loc = linea.Local
            lineaEntrada.Mon_Sis = linea.Sistema
            lineaEntrada.Mon_Reg = linea.MonedaRegistro
            lineaEntrada.NoAsient = linea.TransId
            lineaEntrada.Tip_Cam = linea.Rate
            lineaEntrada.No_FC = linea.DocEntryFC
            If Not linea.NotaCredito Then
                lineaEntrada.NoFP = linea.DocEntryFP
            End If

            udoEntrada.ListaLineas.LineasUDO.Add(lineaEntrada)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CosteoAutomaticoVehiculos()
        Try
            Dim oListaVehiculosCosteo As New List(Of DatosVehiculoCosteo)()
            Dim oLineaVehiculoCosteo As New DatosVehiculoCosteo()

            Dim l_strCodTipo As String
            Dim l_strCodUnid As String
            Dim l_strIDVehiculo As String
            Dim l_strCodMarca As String
            Dim l_strCodEstilo As String
            Dim l_strCodModelo As String
            Dim l_strNumVIN As String
            Dim l_strNumPedido As String
            Dim l_strNumRecepcion As String
            Dim l_decTCMonLocal As Decimal
            Dim l_decTCMonSistema As Decimal
            Dim l_strTipoTransac As String

            Dim strCuentaTransito As String
            Dim strCuentaInventario As String

            Dim strFechaCont As String
            Dim fhaContab As Date

            Dim l_strMoneda As String
            Dim l_decTipoCambio As Decimal
            Dim l_decMontoAs As Decimal

            l_strMoneda = cboMoneda.ObtieneValorDataSource
            strFechaCont = txtFhaCont.ObtieneValorDataSource
            fhaContab = DateTime.ParseExact(strFechaCont, "yyyyMMdd", Nothing)

            l_decTipoCambio = ObtenerTipoCambio(l_strMoneda, fhaContab)
            l_decTCMonLocal = ObtenerTipoCambio(m_strMonLocal, fhaContab)
            l_decTCMonSistema = ObtenerTipoCambio(m_strMonSistema, fhaContab)
            l_strTipoTransac = Utilitarios.EjecutarConsulta("Select U_TipoTransCostAuto from [@SCGD_ADMIN] where Code = 'DMS'", _companySbo.CompanyDB, _companySbo.Server)

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To MatrixEntradaVeh.Matrix.RowCount - 1

                dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
                dtLocal.Clear()

                dtLocal2 = FormularioSBO.DataSources.DataTables.Item("dtLocal2")
                dtLocal2.Clear()



                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i)) Then

                    l_strCodUnid = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i).Trim()
                    l_strIDVehiculo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFIDUnidad, i).Trim()
                    l_strCodMarca = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodMarca, i).Trim()
                    l_strCodTipo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodTipo, i).Trim()

                    l_strCodEstilo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodEstilo, i).Trim()
                    l_strCodModelo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodModelo, i).Trim()
                    l_strNumVIN = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNumVin, i).Trim()
                    l_strNumPedido = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_StrUDFNumPedido, i).Trim()
                    l_strNumRecepcion = txtDocNum.ObtieneValorDataSource()
                    l_strTipoTransac = l_strTipoTransac
                    l_decMontoAs = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_StrUDFMontoAsiento, i).Trim(), n)

                    dtLocal2.ExecuteQuery(String.Format("Select U_Tipo, U_Transito, U_Stock from [@SCGD_ADMIN4] where U_Tipo = '{0}'", l_strCodTipo))

                    If Not String.IsNullOrEmpty(dtLocal2.GetValue("U_Tipo", 0)) Then
                        strCuentaTransito = dtLocal2.GetValue("U_Transito", 0)
                        strCuentaInventario = dtLocal2.GetValue("U_Stock", 0)
                    End If

                    oLineaVehiculoCosteo = New DatosVehiculoCosteo()

                    If String.IsNullOrEmpty(cboMoneda.ObtieneValorDataSource) OrElse
                        cboMoneda.ObtieneValorDataSource.Equals(m_strMonLocal) Then
                        oLineaVehiculoCosteo.MonedaRegistro = m_strMonLocal
                    Else
                        oLineaVehiculoCosteo.MonedaRegistro = m_strMonSistema
                    End If

                    oLineaVehiculoCosteo.CodigoUnid = l_strCodUnid
                    oLineaVehiculoCosteo.IdUnid = l_strIDVehiculo
                    oLineaVehiculoCosteo.TipoInventario = l_strCodTipo
                    oLineaVehiculoCosteo.CodigoMarca = l_strCodMarca
                    oLineaVehiculoCosteo.CodigoEstilo = l_strCodEstilo
                    oLineaVehiculoCosteo.CodigoModelo = l_strCodModelo
                    oLineaVehiculoCosteo.NumVIN = l_strNumVIN
                    oLineaVehiculoCosteo.CuentaCredito = strCuentaTransito
                    oLineaVehiculoCosteo.CuentaDebito = strCuentaInventario
                    oLineaVehiculoCosteo.MontoAsientoLocal = ObtenerMontoEnMonedaLocal(l_decMontoAs, l_decTipoCambio, l_strMoneda, l_decTCMonLocal, m_strMonLocal)
                    oLineaVehiculoCosteo.MontoAsientoSistema = ObtenerMontoEnMonedaLocal(l_decMontoAs, l_decTipoCambio, l_strMoneda, l_decTCMonSistema, m_strMonSistema)
                    oLineaVehiculoCosteo.NumeroPedido = l_strNumPedido
                    oLineaVehiculoCosteo.NumeroRecepcion = l_strNumRecepcion
                    oLineaVehiculoCosteo.TipoTransaccion = l_strTipoTransac
                    oLineaVehiculoCosteo.AplicaDimensiones = True
                    oLineaVehiculoCosteo.TipoCambio = l_decTCMonSistema

                    If m_blnUsaDimensiones Then
                        ObtenerDimenciones(l_strCodMarca, l_strCodTipo, oLineaVehiculoCosteo)
                    End If

                    oListaVehiculosCosteo.Add(oLineaVehiculoCosteo)

                End If
            Next

            If oListaVehiculosCosteo.Count - 1 > 0 Then

                CrearEntradasCosteo(oListaVehiculosCosteo)
                ActualizarDatosRecepcion2(oListaVehiculosCosteo)

            End If

            ' MatrixEntradaVeh.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function CostoManualDeVehiculos() As Boolean
        Try
            Dim l_blnResult As Boolean = False
            Dim l_strCodTipo As String
            Dim l_strCodUnid As String
            Dim l_strIDVehiculo As String
            Dim l_strCodMarca As String
            Dim l_strCodEstilo As String
            Dim l_strCodModelo As String
            Dim l_strNumVIN As String
            Dim l_strNumPedido As String
            Dim l_strNumRecepcion As String
            Dim l_decTCMonLocal As Decimal
            Dim l_decTCMonSistema As Decimal
            Dim l_strTipoTransac As String
            Dim l_decMontoAs As Decimal
            Dim strCuentaTransito As String
            Dim strCuentaInventario As String

            Dim oListaVehiculosCosteo As New List(Of DatosVehiculoCosteo)()
            Dim oLineaVehiculoCosteo As New DatosVehiculoCosteo()
            Dim l_strMoneda As String
            Dim l_decTipoCambio As Decimal

            Dim strFechaCont As String
            Dim fhaContab As Date

            strFechaCont = txtFhaCont.ObtieneValorDataSource
            fhaContab = Date.ParseExact(strFechaCont, "yyyymmdd", Nothing)

            l_decTipoCambio = ObtenerTipoCambio(l_strMoneda, fhaContab)
            l_decTCMonLocal = ObtenerTipoCambio(m_strMonLocal, fhaContab)
            l_decTCMonSistema = ObtenerTipoCambio(m_strMonSistema, fhaContab)

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To MatrixEntradaVeh.Matrix.RowCount - 1

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i)) AndAlso
                     String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNUmAsiento, i)) AndAlso
                     String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_StrUDFNumEntrada, i)) Then

                    l_strCodUnid = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i).Trim()
                    l_strIDVehiculo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFIDUnidad, i).Trim()
                    l_strCodMarca = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodMarca, i).Trim()
                    l_strCodTipo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodTipo, i).Trim()

                    l_strCodEstilo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodEstilo, i).Trim()
                    l_strCodModelo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodModelo, i).Trim()
                    l_strNumVIN = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNumVin, i).Trim()
                    l_strNumPedido = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_StrUDFNumPedido, i).Trim()
                    l_strNumRecepcion = txtDocNum.ObtieneValorDataSource()
                    l_strTipoTransac = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFTipoTransac, i).Trim()
                    l_decMontoAs = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_StrUDFMontoAsiento, i).Trim(), n)

                    dtLocal2.ExecuteQuery(String.Format("Select U_Tipo, U_Transito, U_Stock from [@SCGD_ADMIN4] where U_Tipo = '{0}'", l_strCodTipo))

                    If Not String.IsNullOrEmpty(dtLocal2.GetValue("U_Tipo", 0)) Then
                        strCuentaTransito = dtLocal2.GetValue("U_Transito", 0)
                        strCuentaInventario = dtLocal2.GetValue("U_Stock", 0)
                    End If

                    oLineaVehiculoCosteo = New DatosVehiculoCosteo()

                    If String.IsNullOrEmpty(cboMoneda.ObtieneValorDataSource) OrElse
                        cboMoneda.ObtieneValorDataSource.Equals(m_strMonLocal) Then
                        oLineaVehiculoCosteo.MonedaRegistro = m_strMonLocal
                    Else
                        oLineaVehiculoCosteo.MonedaRegistro = m_strMonSistema
                    End If

                    oLineaVehiculoCosteo.CodigoUnid = l_strCodUnid
                    oLineaVehiculoCosteo.IdUnid = l_strIDVehiculo
                    oLineaVehiculoCosteo.TipoInventario = l_strCodTipo
                    oLineaVehiculoCosteo.CodigoMarca = l_strCodMarca
                    oLineaVehiculoCosteo.CodigoEstilo = l_strCodEstilo
                    oLineaVehiculoCosteo.CodigoModelo = l_strCodModelo
                    oLineaVehiculoCosteo.NumVIN = l_strNumVIN
                    oLineaVehiculoCosteo.CuentaCredito = strCuentaTransito
                    oLineaVehiculoCosteo.CuentaDebito = strCuentaInventario
                    oLineaVehiculoCosteo.MontoAsientoLocal = ObtenerMontoEnMonedaLocal(l_decMontoAs, l_decTipoCambio, l_strMoneda, l_decTCMonLocal, m_strMonLocal)
                    oLineaVehiculoCosteo.MontoAsientoSistema = ObtenerMontoEnMonedaLocal(l_decMontoAs, l_decTipoCambio, l_strMoneda, l_decTCMonSistema, m_strMonSistema)
                    oLineaVehiculoCosteo.NumeroPedido = l_strNumPedido
                    oLineaVehiculoCosteo.NumeroRecepcion = l_strNumRecepcion
                    oLineaVehiculoCosteo.TipoTransaccion = l_strTipoTransac
                    oLineaVehiculoCosteo.AplicaDimensiones = True
                    oLineaVehiculoCosteo.TipoCambio = l_decTCMonSistema

                    If m_blnUsaDimensiones Then
                        ObtenerDimenciones(l_strCodMarca, l_strCodTipo, oLineaVehiculoCosteo)
                    End If

                    oListaVehiculosCosteo.Add(oLineaVehiculoCosteo)


                End If

            Next

            If oListaVehiculosCosteo.Count - 1 > 0 Then

                CrearEntradasCosteo(oListaVehiculosCosteo)
                ActualizarDatosRecepcion2(oListaVehiculosCosteo)
                l_blnResult = True
            End If

            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            Return l_blnResult
        Catch ex As Exception
            Return False
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Function

    Private Sub ActualizarDatosRecepcion2(ByRef p_oListaVehiculos As List(Of DatosVehiculoCosteo))
        Try

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For Each row As DatosVehiculoCosteo In p_oListaVehiculos

                Dim strCodUnid As String = row.CodigoUnid
                Dim strNumAsiento As String = row.NumeroAsiento
                Dim strNumEntrada As String = row.NumeroEntrada
                Dim strNumPedido As String = row.NumeroPedido
                Dim strRecepcion As String = row.NumeroRecepcion

                If Not String.IsNullOrEmpty(strNumAsiento) AndAlso
                    Not String.IsNullOrEmpty(strNumEntrada) Then

                    For i As Integer = 0 To MatrixEntradaVeh.Matrix.RowCount - 1

                        If _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", i).Trim.Equals(strCodUnid) Then

                            _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Num_Asiento", i, row.NumeroAsiento)
                            _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Num_Entrada", i, row.NumeroEntrada)

                        End If

                    Next

                End If
            Next
            MatrixEntradaVeh.Matrix.LoadFromDataSource()
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Sub ActualizarDatosRecepcion(ByRef p_oListaVehiculos As List(Of DatosVehiculoCosteo))

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildRecepcion As SAPbobsCOM.GeneralData
        Dim oChildrenRecepcion As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try

            MatrixEntradaPed.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_EDV")

            For Each row As DatosVehiculoCosteo In p_oListaVehiculos

                Dim strCodUnid As String = row.CodigoUnid
                Dim strNumAsiento As String = row.NumeroAsiento
                Dim strNumEntrada As String = row.NumeroEntrada
                Dim strNumPedido As String = row.NumeroPedido
                Dim strRecepcion As String = row.NumeroRecepcion

                If Not String.IsNullOrEmpty(strNumAsiento) AndAlso
                    Not String.IsNullOrEmpty(strNumEntrada) Then

                    oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                    oGeneralParams.SetProperty("DocEntry", strRecepcion)
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                    oChildrenRecepcion = oGeneralData.Child("SCGD_ENTRADA_UNID")

                    For j As Integer = 0 To oChildrenRecepcion.Count - 1
                        oChildRecepcion = oChildrenRecepcion.Item(j)

                        If oChildRecepcion.GetProperty("U_Cod_Uni").Equals(strCodUnid) Then
                            oChildRecepcion.SetProperty("U_Num_Asiento", row.NumeroAsiento)
                            oChildRecepcion.SetProperty("U_Num_Entrada", row.NumeroEntrada)

                            oGeneralService.Update(oGeneralData)
                            Exit For
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Sub ActualizaLineasUnidadesEnRecepcion(ByVal p_StrCodUnid As String,
                                                   ByVal p_StrNumRecepcion As String,
                                                   ByVal p_StrValor As String,
                                                   ByVal p_StrColumna As String)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildRecepcion As SAPbobsCOM.GeneralData
        Dim oChildrenRecepcion As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try

            MatrixEntradaPed.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_EDV")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_StrNumRecepcion)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oChildrenRecepcion = oGeneralData.Child("SCGD_ENTRADA_UNID")

            For j As Integer = 0 To oChildrenRecepcion.Count - 1
                oChildRecepcion = oChildrenRecepcion.Item(j)

                If oChildRecepcion.GetProperty("U_Cod_Uni").Equals(p_StrCodUnid) Then
                    oChildRecepcion.SetProperty(p_StrColumna, p_StrValor)

                    oGeneralService.Update(oGeneralData)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Sub ActualizarDatosRecepcion(ByVal p_StrNumRecepcion As String,
                                                 ByVal p_StrValor As String,
                                                 ByVal p_StrCampo As String)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildRecepcion As SAPbobsCOM.GeneralData
        Dim oChildrenRecepcion As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Try

            MatrixEntradaPed.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_EDV")

            oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
            oGeneralParams.SetProperty("DocEntry", p_StrNumRecepcion)
            oGeneralData = oGeneralService.GetByParams(oGeneralParams)

            oGeneralData.SetProperty(p_StrCampo, p_StrValor)
            oGeneralService.Update(oGeneralData)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Function ActualizarDatosVehiculo(ByVal p_strCodeVehiculo As String,
                                                ByVal p_StrValor As String,
                                                ByVal p_StrCampo As String) As Boolean
        Dim l_blnResult As Boolean = True

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
            End If

            Return l_blnResult = True
        Catch ex As Exception
            Return l_blnResult = False
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Private Function ObtenerMontoEnMonedaLocal(ByVal p_DecMontoBase As Decimal, ByVal p_decTCOrigen As Decimal, ByVal p_strMonedaOrigen As String,
                                                                                ByVal p_decTCDestino As Decimal, ByVal p_strMonedaDestino As String) As Decimal
        Try

            Dim l_strMonLocal As String = m_strMonLocal
            Dim l_strMonSistema As String = m_strMonSistema

            Dim l_strMonedaOrigen As String
            Dim l_strMonedaDestino As String

            Dim l_decTipoCambioOrigen As Decimal
            Dim l_decTipoCambioDestino As Decimal

            Dim l_decMontoDestino As Decimal

            l_strMonedaOrigen = p_strMonedaOrigen
            l_strMonedaDestino = p_strMonedaDestino

            l_decTipoCambioOrigen = p_decTCOrigen
            l_decTipoCambioDestino = p_decTCDestino


            If l_strMonedaDestino = l_strMonedaOrigen Then
                l_decMontoDestino = p_DecMontoBase

            ElseIf l_strMonedaOrigen <> l_strMonedaDestino Then
                If l_decTipoCambioDestino = 0 Then
                    l_decTipoCambioDestino = 1
                End If
                If l_decTipoCambioOrigen = 0 Then
                    l_decTipoCambioOrigen = 1
                End If

                If l_strMonedaOrigen = l_strMonLocal Then
                    l_decMontoDestino = p_DecMontoBase / l_decTipoCambioDestino
                ElseIf l_strMonedaDestino = l_strMonLocal Then
                    l_decMontoDestino = p_DecMontoBase * l_decTipoCambioOrigen
                Else
                    l_decMontoDestino = (l_decMontoDestino * l_decTipoCambioOrigen) / l_decTipoCambioDestino
                End If

            End If

            Return l_decMontoDestino

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Private Sub ObtenerDimenciones(ByVal p_strCodMarca As String, ByVal p_strCodTipo As String, ByRef p_oLineaAsiento As DatosVehiculoCosteo)
        Dim l_strSQL As String
        Try
            dtDimensiones = FormularioSBO.DataSources.DataTables.Item("dtDimensiones")
            dtDimensiones.Clear()

            l_strSQL = "Select LD.DocEntry,LD.U_Dim1 , LD.U_Dim2, LD.U_Dim3 , LD.U_Dim4 , LD.U_Dim5  from dbo.[@SCGD_DIMEN] D inner join dbo.[@SCGD_LINEAS_DIMEN] LD on " & _
                "d.DocEntry = ld.DocEntry  where D.U_Tip_Inv = '{0}' And LD.U_CodMar = '{1}'"

            l_strSQL = String.Format(l_strSQL, p_strCodTipo, p_strCodMarca)
            dtDimensiones.ExecuteQuery(l_strSQL)

            If Not String.IsNullOrEmpty(dtDimensiones.GetValue("DocEntry", 0)) Then

                If Not String.IsNullOrEmpty(dtDimensiones.GetValue("U_Dim1", 0)) Then
                    p_oLineaAsiento.CostingCode1 = dtDimensiones.GetValue("U_Dim1", 0)
                End If
                If Not String.IsNullOrEmpty(dtDimensiones.GetValue("U_Dim2", 0)) Then
                    p_oLineaAsiento.CostingCode2 = dtDimensiones.GetValue("U_Dim2", 0)
                End If
                If Not String.IsNullOrEmpty(dtDimensiones.GetValue("U_Dim3", 0)) Then
                    p_oLineaAsiento.CostingCode3 = dtDimensiones.GetValue("U_Dim3", 0)
                End If
                If Not String.IsNullOrEmpty(dtDimensiones.GetValue("U_Dim4", 0)) Then
                    p_oLineaAsiento.CostingCode4 = dtDimensiones.GetValue("U_Dim4", 0)
                End If
                If Not String.IsNullOrEmpty(dtDimensiones.GetValue("U_Dim5", 0)) Then
                    p_oLineaAsiento.CostingCode5 = dtDimensiones.GetValue("U_Dim5", 0)
                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Function ActualizaUnidades() As Boolean
        Try

            Dim l_strUnidCode As String
            Dim l_strCode As String

            Dim l_StrUbicacionGen As String
            Dim l_intDispoGen As Integer

            Dim str_IDVeh As String = String.Empty
            Dim str_Tipo As String = String.Empty
            Dim int_Ano As Integer = 0
            Dim str_Color As String = String.Empty
            Dim str_DesColor As String = String.Empty
            Dim str_CodMarca As String = String.Empty
            Dim str_CodEstilo As String = String.Empty
            Dim str_CodModelo As String = String.Empty
            Dim str_CodUnid As String = String.Empty
            Dim str_DesMarca As String = String.Empty
            Dim str_DesEstilo As String = String.Empty
            Dim str_DesModelo As String = String.Empty
            Dim str_NumMot As String = String.Empty
            Dim str_NumVin As String = String.Empty
            Dim str_Ubicacion As String = String.Empty
            Dim int_Diponibilidad As Integer = 0
            Dim str_Dispo As String = String.Empty
            Dim str_Año As String = String.Empty

            l_StrUbicacionGen = cboUbica.ObtieneValorDataSource
            l_intDispoGen = cboDisponibilidad.ObtieneValorDataSource

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                l_strCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Id_Veh", i).Trim

                If Not String.IsNullOrEmpty(l_strCode) Then

                    'l_strUnidCode = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Cod_Unid FROM [@SCGD_VEHICULO] where code = '{0}'", l_strCode),
                    '                                            _companySbo.CompanyDB, CompanySBO.Server)

                    l_strUnidCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUnidad, i).Trim()

                    If Not String.IsNullOrEmpty(l_strUnidCode) Then

                        str_Color = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodColor, i).Trim()
                        str_DesColor = DevuelveDescripcionColor(str_Color).Trim()

                        str_CodMarca = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodMarca, i).Trim()
                        str_CodEstilo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodEstilo, i).Trim()
                        str_CodModelo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodModelo, i).Trim()
                        str_Ubicacion = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFCodUbica, i).Trim()
                        str_Dispo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFEstado, i).Trim()
                        str_NumVin = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNumVin, i).Trim()
                        str_NumMot = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue(m_strUDFNumMot, i).Trim()

                        str_DesMarca = ObtenerDescMarca(str_CodMarca)
                        str_DesEstilo = ObtenerDescEstilo(str_CodEstilo)
                        str_DesModelo = ObtenerDescModelo(str_CodModelo)

                        If Not String.IsNullOrEmpty(str_Dispo) Then
                            int_Diponibilidad = Integer.Parse(str_Dispo)
                        End If
                        If Not String.IsNullOrEmpty(str_Año) Then
                            int_Ano = Integer.Parse(str_Año)
                        End If

                    End If

                    ModificarVehiculoUDO(l_strCode, "", int_Ano, str_Color, str_CodEstilo, str_CodMarca, l_strUnidCode, str_DesColor, str_DesEstilo, str_DesMarca, str_NumMot, "", str_NumVin, int_Diponibilidad, str_Ubicacion)

                End If

                l_strCode = String.Empty
                str_Color = String.Empty
                str_DesColor = String.Empty

                str_CodMarca = String.Empty
                str_CodEstilo = String.Empty
                str_CodModelo = String.Empty

                str_DesMarca = String.Empty
                str_DesEstilo = String.Empty
                str_DesModelo = String.Empty

                str_Ubicacion = String.Empty
                str_NumVin = String.Empty
                str_NumMot = String.Empty
                str_Año = String.Empty
                str_Dispo = String.Empty
                int_Diponibilidad = 0
                int_Ano = 0

            Next

            Return True
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Sub ModificarVehiculoUDO(ByRef p_strIDVehiculo As String, _
                                 ByVal p_strTipo As String,
                                 ByVal p_intAnoVehiculo As Integer, _
                                 ByVal p_strCodColor As String, _
                                 ByVal p_strCodEstilo As String, _
                                 ByVal p_strCodMarca As String,
                                 ByVal p_strCodUnidad As String,
                                 ByVal p_strDescColor As String, _
                                 ByVal p_strDescEstilo As String, _
                                 ByVal p_strDescMarca As String,
                                 ByVal p_strNumMotor As String, _
                                 ByVal p_strPlaca As String, _
                                 ByVal p_strVIN As String,
                                 ByVal p_intDisponibilidad As Integer, _
                                 ByVal p_strUbicacion As String)
        Try

            Dim UDOVehiculo As UDOVehiculos

            UDOVehiculo = New UDOVehiculos(_companySbo)

            UDOVehiculo.Encabezado = New EncabezadoUDOVehiculos()
            UDOVehiculo.Encabezado.Code = p_strIDVehiculo
            UDOVehiculo.Company = _companySbo

            UDOVehiculo.Load()

            UDOVehiculo.Encabezado.Ano = p_intAnoVehiculo
            UDOVehiculo.Encabezado.CodigoMarca = p_strCodMarca
            'UDOVehiculo.Encabezado.NoUnidad = p_strCodUnidad
            UDOVehiculo.Encabezado.Color = p_strDescColor
            UDOVehiculo.Encabezado.Estilo = p_strDescEstilo
            UDOVehiculo.Encabezado.Marca = p_strDescMarca
            UDOVehiculo.Encabezado.NumeroMotor = p_strNumMotor
            UDOVehiculo.Encabezado.Placa = p_strPlaca
            UDOVehiculo.Encabezado.Vin = p_strVIN
            UDOVehiculo.Encabezado.Disponibilidad = p_intDisponibilidad
            UDOVehiculo.Encabezado.CodigoUbicacion = p_strUbicacion
            UDOVehiculo.Encabezado.CodigoEstilo = p_strCodEstilo
            UDOVehiculo.Encabezado.CodigoColor = p_strCodColor


            'If Not String.IsNullOrEmpty(p_FhaIngresoInv.ToString()) Then
            '    UDOVehiculo.Encabezado.FechaIngInventario = p_FhaIngresoInv
            'End If

            'If String.IsNullOrEmpty(strMonedaVehiculo) Then
            '    UDOVehiculo.Encabezado.Moneda = p_strMonedaCont
            '    strMonedaVehiculo = p_strMonedaCont
            'End If

            UDOVehiculo.Update()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Sub ModificarDatoVehiculoUDO(ByRef p_strIDVehiculo As String, _
                                         ByVal p_strValor As String,
                                         ByVal p_strCampo As String)
        Try

            Dim UDOVehiculo As UDOVehiculos

            UDOVehiculo = New UDOVehiculos(_companySbo)

            UDOVehiculo.Encabezado = New EncabezadoUDOVehiculos()
            UDOVehiculo.Encabezado.Code = p_strIDVehiculo
            UDOVehiculo.Company = _companySbo

            UDOVehiculo.Load()


            Select Case p_strValor
                Case "U_Fha_Ing_Inv"
                    UDOVehiculo.Encabezado.FechaIngInventario = p_strValor
            End Select

            UDOVehiculo.Update()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try


    End Sub

    Private Function DevuelveCodigoVehiculo() As System.Nullable(Of Integer)

        Dim intCodigo As System.Nullable(Of Integer)

        intCodigo = Utilitarios.EjecutarConsulta("SELECT AutoKey FROM ONNM WHERE (ObjectCode = 'SCGD_VEH')", _companySbo.CompanyDB, _companySbo.Server)

        If Not intCodigo.HasValue Then
            intCodigo = 1
        End If

        Return intCodigo

    End Function

    Private Function DevuelveDescripcionColor(ByVal p_CodColor As String) As String
        Try
            Dim l_strResult As String = String.Empty
            Dim l_strSQL As String

            l_strSQL = "Select Name from [@SCGD_COLOR] where code = '{0}'"
            l_strResult = Utilitarios.EjecutarConsulta(String.Format(l_strSQL, p_CodColor.Trim), _companySbo.CompanyDB, _companySbo.Server)

            Return l_strResult

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Sub InsertarVehiculoUDO(ByRef p_strIDVehiculo As String,
                                         ByVal p_strTipo As String,
                                         ByVal p_intAnoVehiculo As Integer,
                                         ByVal p_strCodColor As String,
                                         ByVal p_strCodEstilo As String,
                                         ByVal p_strCodMarca As String,
                                         ByVal p_strCodModelo As String,
                                         ByVal p_strCodUnidad As String,
                                         ByVal p_strDescColor As String,
                                         ByVal p_intDispo As Integer,
                                         ByVal p_strNumMotor As String,
                                         ByVal p_strVIN As String,
                                         ByVal p_strUbicacion As String,
                                         ByVal p_intCilindros As Integer,
                                         ByVal p_intPuertas As Integer,
                                         ByVal p_intPasajeros As Integer,
                                         ByVal p_intEjes As Integer,
                                         ByVal p_intPeso As Integer,
                                         ByVal p_intCilindrada As Integer,
                                         ByVal p_intPontencia As Integer,
                                         ByVal p_strCategoria As String,
                                         ByVal p_strMarcaMot As String,
                                         ByVal p_strTransmis As String,
                                         ByVal p_strCarroceria As String,
                                         ByVal p_strTipoTrac As String,
                                         ByVal p_strCabina As String,
                                         ByVal p_strCombustible As String,
                                         ByVal p_intGarantiaKM As Integer,
                                         ByVal p_intGarantiaTM As Integer,
                                         ByVal p_strTipoTecho As String,
                                         ByVal p_StrDescMarca As String,
                                         ByVal p_strDescEstilo As String,
                                         ByVal p_strDescModelo As String,
                                         ByVal p_strDesMarcaComercial As String,
                                         ByVal p_strCodMarcaComercial As String,
                                         ByVal p_strDocEntryRec As String,
                                         ByVal p_strDocPedido As String)

        Dim UDOVehiculo As UDOVehiculos
        Dim UDOEncabezado As EncabezadoUDOVehiculos
        '  Dim UDTTrazabilidad As TrazabilidadUDOVehiculo

        UDOVehiculo = New UDOVehiculos(_companySbo)
        UDOEncabezado = New EncabezadoUDOVehiculos()

        'UDOVehiculos.ListaTrazabilidad = New ListaTrazabilidadUDOVehiculo()
        ' UDOVehiculos.ListaTrazabilidad.LineasUDO = New List(Of ILineaUDO)()

        UDOEncabezado.Code = p_strIDVehiculo

        UDOEncabezado.Ano = p_intAnoVehiculo
        UDOEncabezado.CodigoColor = p_strCodColor
        UDOEncabezado.CodigoEstilo = p_strCodEstilo

        UDOEncabezado.Estilo = p_strDescEstilo
        UDOEncabezado.CodigoMarca = p_strCodMarca

        UDOEncabezado.Marca = p_StrDescMarca
        UDOEncabezado.CodigoModelo = p_strCodModelo

        UDOEncabezado.Modelo = p_strDescModelo
        UDOEncabezado.NoUnidad = p_strCodUnidad
        UDOEncabezado.Color = p_strDescColor
        UDOEncabezado.NumeroMotor = p_strNumMotor
        UDOEncabezado.Vin = p_strVIN
        '  UDOEncabezado.Disponibilidad = p_strDisponibilidad
        UDOEncabezado.Tipo = p_strTipo
        UDOEncabezado.EstadoNuevo = "N"
        UDOEncabezado.CodigoUbicacion = p_strUbicacion
        'UDOEncabezado.ArriboEstimado = p_fhaIngresoInv

        UDOEncabezado.Cilindros = p_intCilindros
        UDOEncabezado.Puertas = p_intPuertas
        UDOEncabezado.CantidadPasajeros = p_intPasajeros
        UDOEncabezado.Ejes = p_intEjes
        UDOEncabezado.Peso = p_intPeso
        UDOEncabezado.Cilindrada = p_intCilindrada
        UDOEncabezado.Potencia = p_intPontencia
        UDOEncabezado.Categoria = p_strCategoria
        UDOEncabezado.MarcaMotor = p_strMarcaMot
        UDOEncabezado.Transmision = p_strTransmis
        UDOEncabezado.Carroceria = p_strCarroceria
        UDOEncabezado.TipoTraccion = p_strTipoTrac
        UDOEncabezado.Cabina = p_strCabina
        UDOEncabezado.Combustible = p_strCombustible
        UDOEncabezado.GarantiaKm = p_intGarantiaKM
        UDOEncabezado.GarantiaTiempo = p_intGarantiaTM
        UDOEncabezado.TipoTecho = p_strTipoTecho
        UDOEncabezado.Disponibilidad = p_intDispo
        UDOEncabezado.ArticuloVenta = p_strCodMarcaComercial
        UDOEncabezado.DescArticuloVenta = p_strDesMarcaComercial
        UDOEncabezado.U_DocRecepcion = p_strDocEntryRec
        UDOEncabezado.U_DocPedido = p_strDocPedido

        UDOVehiculo.Encabezado = UDOEncabezado

        UDOVehiculo.Company = _companySbo
        UDOVehiculo.Insert()

    End Sub

    Private Function ObtenerDescMarca(ByVal p_strMarca As String) As String
        Try
            Dim l_Result As String = String.Empty
            Dim l_SQL As String

            l_SQL = "SELECT Code ,Name FROM [@SCGD_MARCA] where Code = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(String.Format(l_SQL, p_strMarca))
            If Not String.IsNullOrEmpty(dtLocal.GetValue("Name", 0)) Then
                l_Result = dtLocal.GetValue("Name", 0)
            End If

            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Return ""
        End Try
    End Function

    Private Function ObtenerDescEstilo(ByVal p_strMarca As String) As String
        Try
            Dim l_Result As String = String.Empty
            Dim l_SQL As String

            l_SQL = "SELECT Code ,Name FROM [@SCGD_ESTILO] where Code = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(String.Format(l_SQL, p_strMarca))
            If Not String.IsNullOrEmpty(dtLocal.GetValue("Name", 0)) Then
                l_Result = dtLocal.GetValue("Name", 0)
            End If

            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Return ""
        End Try
    End Function

    Private Function ObtenerDescModelo(ByVal p_strMarca As String) As String
        Try
            Dim l_Result As String = String.Empty
            Dim l_SQL As String

            l_SQL = "SELECT Code ,U_Descripcion FROM [@SCGD_MODELO] where Code = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            dtLocal.ExecuteQuery(String.Format(l_SQL, p_strMarca))
            If Not String.IsNullOrEmpty(dtLocal.GetValue("U_Descripcion", 0)) Then
                l_Result = dtLocal.GetValue("U_Descripcion", 0)
            End If

            Return l_Result
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
            Return ""
        End Try
    End Function

    Public Function ObternerFechaServer() As DateTime
        Try
            Dim l_fhaActual As DateTime

            l_fhaActual = Utilitarios.EjecutarConsulta("select GETDATE()", _companySbo.CompanyDB, _companySbo.Server)

            Return l_fhaActual
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Sub EliminarLineaUnidad(ByRef pVal As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean)
        Try
            Dim intSelect As Integer
            Dim oMat As SAPbouiCOM.Matrix

            If pVal.BeforeAction Then
                If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeEntradaVehiculosBorrarLinVeh, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                    bubbleEvent = False
                End If
            ElseIf pVal.ActionSuccess Then

            End If


            MatrixEntradaVeh.Matrix.FlushToDataSource()

            oMat = DirectCast(FormularioSBO.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)
            intSelect = oMat.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Do While intSelect > -1
                MatrixEntradaVeh.Matrix.FlushToDataSource()

                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).RemoveRecord(intSelect - 1)
                intSelect = oMat.GetNextSelectedRow(intSelect, SAPbouiCOM.BoOrderType.ot_RowOrder)

            Loop

            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ActualizarDatosPedidos(Optional ByVal p_blnCancelar As Boolean = False)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oChildPedido As SAPbobsCOM.GeneralData
        Dim oChildrenPedido As SAPbobsCOM.GeneralDataCollection
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
        Dim blnAbrirPedido As Boolean = False

        Dim strPedido As String
        Dim strLinea As String
        Dim intCantRec As Integer
        Dim intCantSol As Integer
        Dim strArticulo As String

        Dim intRecibidoGen As Integer
        Dim intSolicitadoGen As Integer
        Dim intPendienteGen As Integer

        Dim intRecLinea As Integer
        Dim intPenLinea As Integer
        Dim intSolLinea As Integer
        Dim blnCerrar As Boolean = True
        Try

            MatrixEntradaPed.Matrix.FlushToDataSource()

            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_PDV")

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1

                If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", i)) Then
                    strPedido = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Num_Ped", i).Trim
                    strLinea = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Line_Ref", i).Trim
                    intCantRec = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cant_Ent", i).Trim
                    strArticulo = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", i).Trim

                    If Not String.IsNullOrEmpty(strPedido) Then
                        oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                        oGeneralParams.SetProperty("DocEntry", strPedido)
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                        oChildrenPedido = oGeneralData.Child("SCGD_PEDIDOS_LINEAS")

                        intSolicitadoGen = oGeneralData.GetProperty("U_Cant_Veh").ToString.Trim()
                        intPendienteGen = oGeneralData.GetProperty("U_Pend_Veh").ToString.Trim()
                        intRecibidoGen = oGeneralData.GetProperty("U_Recib_Veh").ToString.Trim()

                        For j As Integer = 0 To oChildrenPedido.Count - 1
                            oChildPedido = oChildrenPedido.Item(j)

                            If strLinea = oChildPedido.GetProperty("LineId") AndAlso
                                strPedido = oChildPedido.GetProperty("DocEntry") AndAlso
                                strArticulo = oChildPedido.GetProperty("U_Cod_Art") Then

                                intSolLinea = oChildPedido.GetProperty("U_Cant")
                                intRecLinea = oChildPedido.GetProperty("U_Cant_Rec")
                                intPenLinea = oChildPedido.GetProperty("U_Pen_Rec")


                                If p_blnCancelar Then
                                    intCantRec = oChildPedido.GetProperty("U_Cant_Rec") - intCantRec
                                    oChildPedido.SetProperty("U_Cant_Rec", intCantRec)

                                    If intCantRec <= 0 Then
                                        blnAbrirPedido = True
                                    End If
                                Else
                                    If intPenLinea - intCantRec <= 0 Then
                                        oChildPedido.SetProperty("U_Pen_Rec", 0)
                                        oChildPedido.SetProperty("U_Cerrada", "Y")
                                    Else
                                        oChildPedido.SetProperty("U_Pen_Rec", intPenLinea - intCantRec)
                                        oChildPedido.SetProperty("U_Cerrada", "N")
                                    End If

                                    oChildPedido.SetProperty("U_Cant_Rec", intRecLinea + intCantRec)

                                    oGeneralData.SetProperty("U_Pend_Veh", intPendienteGen - intCantRec)
                                    oGeneralData.SetProperty("U_Recib_Veh", intRecibidoGen + intCantRec)

                                    'If intCantRec < intSolLinea Then
                                    '    blnCerrarPedido = False
                                    'End If
                                End If

                            End If
                        Next
                        oGeneralService.Update(oGeneralData)

                        intCantRec = 0
                        intCantSol = 0
                        For y As Integer = 0 To oChildrenPedido.Count - 1
                            oChildPedido = oChildrenPedido.Item(y)
                            If oChildPedido.GetProperty("U_Cerrada").Equals("N") Then
                                blnCerrar = False
                                Exit For
                            End If
                        Next
                        If blnCerrar Then
                            oGeneralService.Close(oGeneralParams)
                        End If
                        'ActualizaEstadoPedido(strPedido, 1)

                    End If
                End If
            Next
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ReAbrirPedido()
        Try
            Dim l_StrSQL As String
            Dim l_strDocEnty As String


            l_strDocEnty = txtDocEntry.ObtieneValorDataSource()

            l_StrSQL = "UPDATE [@SCGD_PEDIDOS] Set Canceled = 'N' WHERE DocEntry = '{0}'"
            Utilitarios.EjecutarConsulta(String.Format(l_StrSQL, l_strDocEnty), _companySbo.CompanyDB, _companySbo.Server)

            l_StrSQL = "Update [@SCGD_PEDIDOS] set Status = 'O' where DocEntry = '{0}'"
            Utilitarios.EjecutarConsulta(String.Format(l_StrSQL, l_strDocEnty), _companySbo.CompanyDB, _companySbo.Server)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaDisponibilidadAutomatico()
        Try
            Dim l_strEstado As String
            Dim l_intSize As Integer

            l_strEstado = cboDisponibilidad.ObtieneValorDataSource()

            FormularioSBO.Freeze(True)

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            l_intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size

            For i As Integer = 0 To l_intSize - 1
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Estado", i, l_strEstado)
            Next
            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaTipoTrasaccion()
        Try

            Dim l_strTipoTran As String
            ' l_strTipoTran = cboTipoTransac.ObtieneValorDataSource

            FormularioSBO.Freeze(True)
            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Tipo_Trans", i, l_strTipoTran)
            Next
            MatrixEntradaVeh.Matrix.LoadFromDataSource()
            FormularioSBO.Freeze(False)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub AsignaUbicacionUnidadAutomatico()
        Try
            Dim l_strUbicacion As String
            Dim l_intSize As Integer


            l_strUbicacion = cboUbica.ObtieneValorDataSource()
            FormularioSBO.Freeze(True)

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            l_intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size


            For i As Integer = 0 To l_intSize - 1
                'If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Ubi", i)) Then
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Cod_Ubi", i, l_strUbicacion)
                'End If
            Next
            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub AsignaTipoInvUnidadAutomatico()
        Try
            Dim l_strTipoInv As String
            Dim l_intSize As Integer

            l_strTipoInv = cboTipoInv.ObtieneValorDataSource()

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            l_intSize = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size
            FormularioSBO.Freeze(True)
            For i As Integer = 0 To l_intSize - 1
                ' If Not String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Tip", i)) Then
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Cod_Tip", i, l_strTipoInv)
                ' End If
            Next

            MatrixEntradaVeh.Matrix.LoadFromDataSource()
            FormularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub CargarDatosEncabezadoPedido(ByVal p_NumPed As String)

        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams

        Dim strPedido As String

        Try
            oCompanyService = _companySbo.GetCompanyService()
            oGeneralService = oCompanyService.GetGeneralService("SCGD_PDV")

            If Not String.IsNullOrEmpty(p_NumPed) Then
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", p_NumPed)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)

                If String.IsNullOrEmpty(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).GetValue("U_Cod_Prov", 0)) Then
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_Moneda", 0, oGeneralData.GetProperty("U_DocCurr").ToString().Trim())
                End If

                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_Cod_Prov", 0, oGeneralData.GetProperty("U_Cod_Prov").ToString().Trim())
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_Name_Prov", 0, oGeneralData.GetProperty("U_Name_Prov").ToString().Trim())
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_Contact", 0, oGeneralData.GetProperty("U_CodContac").ToString().Trim())

                




            End If
        Catch ex As Exception

        End Try
    End Sub


#End Region

#Region "Evento del Fomulario"
    Public Sub ManejadorEventoClick(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        If pVal.BeforeAction Then
            If pVal.Row > 0 Then
                Select Case pVal.ItemUID
                    Case MatrixEntradaVeh.UniqueId
                        Dim strValorSel As String
                        Select Case pVal.ColUID

                            Case "col_Esti"

                                CargarComboEstilos(FormularioSBO, pVal, pVal.EventType)

                                If FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).GetValue("U_UnidGen", 0).Trim = "Y" Then
                                    FormularioSBO.Mode = BoFormMode.fm_OK_MODE
                                End If

                            Case "col_Mode"

                                CargarComboModelo(FormularioSBO, pVal, pVal.EventType)

                        End Select
                End Select
            End If

        End If
    End Sub

    Public Sub ManejadorEventoCombo(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim strValorSel As String = String.Empty

            If pVal.ActionSuccess Then

                Select Case pVal.ItemUID
                    Case cboMoneda.UniqueId
                        m_strMonedaDestino = cboMoneda.ObtieneValorDataSource()


                        If ManejaTipoCambio(BubbleEvent) Then
                            m_decTCDestino = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)

                            ManejoCambioMoneda()
                            ActualizaCostosValores()

                        End If

                    Case MatrixEntradaVeh.UniqueId

                        If pVal.Row > 0 Then
                            MatrixEntradaVeh.Matrix.FlushToDataSource()

                            Select Case pVal.ColUID

                                Case "col_Marc"

                                    CargarComboEstilos(FormularioSBO, pVal, BoEventTypes.et_CLICK)
                                Case "col_Esti"

                                    CargarComboEstilos(FormularioSBO, pVal, pVal.EventType)
                                    CargarComboModelo(FormularioSBO, pVal, BoEventTypes.et_CLICK)

                                Case "col_Mode"

                                    CargarComboModelo(FormularioSBO, pVal, BoEventTypes.et_CLICK)

                            End Select

                            If pVal.ColUID = MatrixEntradaVeh.ColumnaColMar.UniqueId OrElse
                               pVal.ColUID = MatrixEntradaVeh.ColumnaColEst.UniqueId OrElse
                               pVal.ColUID = MatrixEntradaVeh.ColumnaColMod.UniqueId OrElse
                               pVal.ColUID = MatrixEntradaVeh.ColumnaColUbi.UniqueId OrElse
                               pVal.ColUID = MatrixEntradaVeh.ColumnaColSta.UniqueId Then

                                If FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).GetValue("U_UnidGen", 0).Trim = "Y" Then
                                    FormularioSBO.Freeze(True)
                                    FormularioSBO.Items.Item(txtPrefijo.UniqueId).Enabled = False
                                    FormularioSBO.Items.Item(txtConsecutivo.UniqueId).Enabled = False

                                    FormularioSBO.Freeze(False)
                                End If

                            End If
                        End If

                    Case cboUbica.UniqueId
                        AsignaUbicacionUnidadAutomatico()

                    Case cboTipoInv.UniqueId
                        AsignaTipoInvUnidadAutomatico()

                    Case cboDisponibilidad.UniqueId
                        AsignaDisponibilidadAutomatico()


                End Select

            ElseIf pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case cboMoneda.UniqueId
                        m_strMonedaOrigen = cboMoneda.ObtieneValorDataSource
                        m_decTCOrigen = Decimal.Parse(txtTipoC.ObtieneValorDataSource(), n)
                End Select
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

 

    <System.CLSCompliant(False)> _
    Public Sub ManejadorEventoChooseFromList(ByRef pval As SAPbouiCOM.ItemEvent, _
                                              ByVal FormUID As String, _
                                              ByRef BubbleEvent As Boolean)

        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition

        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim strCFL_Id As String

        Dim oCFLEvent As SAPbouiCOM.IChooseFromListEvent
        oCFLEvent = CType(pval, SAPbouiCOM.IChooseFromListEvent)

        oCFLEvent = CType(pval, SAPbouiCOM.IChooseFromListEvent)
        strCFL_Id = oCFLEvent.ChooseFromListUID
        oCFL = _formularioSBO.ChooseFromLists.Item(strCFL_Id)


        If oCFLEvent.ActionSuccess Then

            Dim oDataTable As SAPbouiCOM.DataTable
            oDataTable = oCFLEvent.SelectedObjects

            If Not oCFLEvent.SelectedObjects Is Nothing Then
                If Not oDataTable Is Nothing And _formularioSBO.Mode <> BoFormMode.fm_FIND_MODE Then

                    Select Case pval.ItemUID
                        Case MatrixEntradaPed.UniqueId
                            Select Case pval.ColUID
                                Case "col_Code"
                                    AsignaValoresMatArticulos(FormUID, pval, oDataTable)
                            End Select
                        Case MatrixEntradaVeh.UniqueId

                            ' VentanaMarcaEstiloModelo(pval, FormUID, BubbleEvent)


                        Case MatrixEntradaVeh.UniqueId

                        Case txtCodProv.UniqueId

                            m_strMonedaOrigen = cboMoneda.ObtieneValorDataSource
                            AsignaValoresProveedor(FormUID, pval, oDataTable)
                            CargarMonedaSocio(txtCodProv.ObtieneValorDataSource)
                            m_strMonedaDestino = cboMoneda.ObtieneValorDataSource

                            ManejaTipoCambio(BubbleEvent)
                            ManejoCambioMoneda()
                        Case "btnCopy"
                            AsignaValoresEncabezado(FormUID, pval, oDataTable)
                            AsignaValoresPedidos(FormUID, pval, oDataTable)
                            CargaTipoCambio()
                            'CargarMonedaLocal(False)
                            Call ActualizaCostosValores()

                    End Select

                End If
            End If

        ElseIf oCFLEvent.BeforeAction Then
            If pval.ItemUID = MatrixEntradaPed.UniqueId Then
                Select Case pval.ColUID
                    Case "col_Code"
                        oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_SCGD_TipoArticulo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = "8"
                        oCondition.BracketCloseNum = 1
                        oCFL.SetConditions(oConditions)
                End Select
            ElseIf pval.ItemUID = MatrixEntradaVeh.UniqueId Then

                Select Case pval.ColUID
                    Case "col_DMar"
                        VentanaMarcaEstiloModelo(pval, FormUID, BubbleEvent)
                    Case "col_DEst"
                        VentanaMarcaEstiloModelo(pval, FormUID, BubbleEvent)
                    Case "col_DMod"
                        VentanaMarcaEstiloModelo(pval, FormUID, BubbleEvent)
                End Select

            ElseIf pval.ItemUID = txtCodProv.UniqueId Then

                oConditions = _applicationSbo.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add

                oCondition.BracketOpenNum = 1
                oCondition.Alias = "CardType"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "S"
                oCondition.BracketCloseNum = 1

                oCFL.SetConditions(oConditions)
            End If

            Select Case pval.ItemUID
                Case "btnCopy"

                    oConditions = ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "Status"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = "O"
                    oCondition.BracketCloseNum = 1

                    If Not String.IsNullOrEmpty(txtCodProv.ObtieneValorDataSource()) Then

                        oCondition.Relationship = BoConditionRelationship.cr_AND
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 2
                        oCondition.Alias = "U_Cod_Prov"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = txtCodProv.ObtieneValorDataSource()
                        oCondition.BracketCloseNum = 2

                    End If

                    oCFL.SetConditions(oConditions)
            End Select
        End If
    End Sub

    Public Sub VentanaMarcaEstiloModelo(ByRef pval As SAPbouiCOM.ItemEvent, ByVal FormUID As String, ByRef BubbleEvent As Boolean)
        Try
            Dim l_strCodMarca As String
            Dim l_strCodEstilo As String
            Dim l_strCodModelo As String
            Dim l_intPos As Integer
            Dim l_strUnidCode As String

            MatrixEntradaVeh.Matrix.FlushToDataSource()


            oForm = _applicationSbo.Forms.Item(FormUID)

            If Not Utilitarios.ValidarSiFormularioAbierto("SCGD_SME", False, _applicationSbo) Then
                l_intPos = pval.Row - 1

                l_strCodMarca = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Mar", l_intPos).Trim
                l_strCodEstilo = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Est", l_intPos).Trim
                l_strCodModelo = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Mod", l_intPos).Trim
                l_strUnidCode = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", l_intPos).Trim

                l_oSeleccionMarcaEstilo.FormConfiguracion = oForm
                l_oSeleccionMarcaEstilo.MatrizVeh = MatrixEntradaVeh
                Call l_oSeleccionMarcaEstilo.CargaFormulario(l_intPos, l_strCodMarca, l_strCodEstilo, l_strCodModelo, l_strUnidCode)

            End If

            BubbleEvent = False
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejadorEventosItemPressed(ByVal FormUID As String, ByVal pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If pVal.ActionSuccess Then

            Select Case pVal.ItemUID
                Case FolderPedidos.UniqueId
                    FormularioSBO.PaneLevel = 1

                Case FolderUnidades.UniqueId
                    FormularioSBO.PaneLevel = 2
                    FormularioSBO.Items.Item("txtPref").Click()

                Case btnAddPed.UniqueId
                    AgregarLineaSiguentePedidos()

                Case btnAddUnid.UniqueId
                    AgregarLineaSiguenteUnidades()

                Case btnDelPed.UniqueId
                    EliminarLineasPedido()
                    ActualizaCostosValores()

                Case btnDelUnid.UniqueId

                    EliminarLineasUnidad()

                Case btnGenera.UniqueId

                    Call GenerarUnidades()

                Case btnCrea.UniqueId

                    If CrearDatosMaestrosVehiculos() Then

                        _formularioSBO.Freeze(True)

                        If m_blnUsaCostoAuto Then
                            CosteoAutomaticoVehiculos()
                        End If

                        _formularioSBO.Freeze(False)


                        _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue("U_UnidGen", 0, "Y")

                        'Cambia el formulario a modo fm_UPDATE_MODE ya que en modo fm_OK_MODE no guarda en base de datos los valores que están en pantalla
                        'al momento de crear las unidades
                        If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE

                        If _formularioSBO.Mode = BoFormMode.fm_ADD_MODE Or
                           _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then

                            ActualizarDatosPedidos()
                            m_blnValidarCrearDoc = False
                            _formularioSBO.Items.Item("1").Click()

                        End If

                    End If

                Case btnActualiza.UniqueId

                    If ActualizaUnidades() Then

                        If _formularioSBO.Mode = BoFormMode.fm_ADD_MODE Or
                            _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then

                            m_blnValidarCrearDoc = False
                            _formularioSBO.Items.Item("1").Click()

                        End If

                    End If

                Case btnCosteo.UniqueId

                    If CostoManualDeVehiculos() Then
                        CargarFormularioModoAdd()
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeEntredaVehiculosCosteoRealizado, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                    Else
                        _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeEntradaDeVehiculosProblemaCostear, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)

                    End If
                Case "btnCopy"
                    Dim l_strCodProv As String
                    m_strMonedaOrigen = cboMoneda.ObtieneValorDataSource()
                    l_strCodProv = txtCodProv.ObtieneValorDataSource()
                    CargarFormularioSeleccionPedidos(l_strCodProv)
                    If String.IsNullOrEmpty(l_strCodProv) Then
                        boolCambiarMoneda = True
                    End If


                Case "1"
                    Dim oitem As SAPbouiCOM.Item

                    m_blnValidarCrearDoc = True

                    txtFhaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))

                    If FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Or FormularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                        CargarMonedaLocal(False)
                    Else
                        CargarMonedaLocal()
                    End If


                    If FormularioSBO.Mode = BoFormMode.fm_ADD_MODE Then

                        CargarSerieDocumento()
                        AgregarPrimerLineaPedidos()
                        AgregarPrimerLineaUnidades()
                    End If

                    FormularioSBO.EnableMenu("1282", False)

                    If Not FormularioSBO Is Nothing Then
                        oForm = ApplicationSBO.Forms.Item("SCGD_EDV")
                        For Each oitem In FormularioSBO.Items
                            oitem.Enabled = True
                        Next
                    End If

                    FormularioSBO.Items.Item(txtDocNum.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    FormularioSBO.Items.Item(cboEstadoDoc.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    FormularioSBO.Items.Item(cbxCancelado.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    FormularioSBO.Items.Item(cboTipoInv.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)

                    FormularioSBO.Items.Item(txtPrefijo.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)
                    FormularioSBO.Items.Item(txtConsecutivo.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)

                    MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColCod.UniqueId).Editable = True
                    MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColTip.UniqueId).Editable = True
            End Select

        ElseIf pVal.BeforeAction Then
            Dim oMatPedidos As SAPbouiCOM.Matrix
            Dim oMatVEhiculo As SAPbouiCOM.Matrix
            Select Case pVal.ItemUID

                Case "1"
                    If m_blnValidarCrearDoc Then

                        If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            If ValidarDatos(pVal, BubbleEvent) Then
                                ActualizarDatosPedidos()
                            Else
                                BubbleEvent = False
                            End If
                        End If

                    End If


                Case FolderPedidos.UniqueId
                    oMatPedidos = DirectCast(oForm.Items.Item("mtx_Pedido").Specific, SAPbouiCOM.Matrix)
                    oMatVEhiculo = DirectCast(oForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)

                    oMatPedidos.FlushToDataSource()
                    oMatVEhiculo.FlushToDataSource()

                Case FolderUnidades.UniqueId
                    oMatPedidos = DirectCast(oForm.Items.Item("mtx_Pedido").Specific, SAPbouiCOM.Matrix)
                    oMatVEhiculo = DirectCast(oForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)

                    oMatPedidos.FlushToDataSource()
                    oMatVEhiculo.FlushToDataSource()

                Case btnDelPed.UniqueId
                    'EliminarLineaPedido(pVal, BubbleEvent)
                    ValidarEliminarLineaPedido(BubbleEvent)

                Case btnDelUnid.UniqueId

                    If ValidarEliminarLineaUnidad(BubbleEvent) Then
                        BubbleEvent = False
                    End If

                Case btnCrea.UniqueId

                    If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE OrElse
                                            FormularioSBO.Mode = BoFormMode.fm_OK_MODE OrElse
                                            FormularioSBO.Mode = BoFormMode.fm_UPDATE_MODE Then

                        If ValidarDatos(pVal, BubbleEvent) = False Then
                            BubbleEvent = False
                        ElseIf Not ValidarCrearUnidades(pVal, BubbleEvent) Then
                            BubbleEvent = False
                        End If


                    End If
                Case btnGenera.UniqueId
                    If ValidarGenerarUnidades(pVal, BubbleEvent) Then
                        BubbleEvent = False
                        _applicationSbo.SetStatusBarMessage(My.Resources.Resource.MensajeEntradaVehiculosSinLineasParaGenerar, BoMessageTime.bmt_Short, True)
                    End If
                Case btnActualiza.UniqueId

                    FormularioSBO.Items.Item("txtPref").Click()
            End Select
        End If

    End Sub


    'Private Sub SBO_Application_FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean) Handles _applicationSbo.FormDataEvent
    '    Try
    '        Dim strKey As String = ""
    '        Dim xmlDocKey As New Xml.XmlDocument
    '        Dim DocEntryActual As String
    '        xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
    '        Select Case BusinessObjectInfo.EventType
    '            Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD
    '                DocEntryActual = String.Empty
    '                If BusinessObjectInfo.ActionSuccess Then
    '                    Select Case BusinessObjectInfo.FormTypeEx
    '                        'Oferta de ventas
    '                        Case "SCGD_EDV"
    '                            xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
    '                            Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
    '                            If Not String.IsNullOrEmpty(strKey) Then
    '                                DocEntryActual = strKey
    '                            End If
    '                    End Select
    '                End If
    '            Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
    '                DocEntryActual = String.Empty
    '                Select Case BusinessObjectInfo.FormTypeEx
    '                    'Oferta de ventas
    '                    Case "SCGD_EDV"
    '                        '  xmlDocKey.LoadXml(BusinessObjectInfo.ObjectKey)
    '                        Utilitarios.SacarValorObjectKey("DocumentParams", "DocEntry", strKey, xmlDocKey)
    '                        If Not String.IsNullOrEmpty(strKey) Then
    '                            DocEntryActual = strKey
    '                        End If
    '                End Select
    '        End Select
    '    Catch ex As Exception
    '    End Try
    'End Sub



    Private Sub EliminarLineasPedido()
        Try
            Dim intSeleccion As Integer
            Dim l_list As New List(Of Integer)
            Dim l_strCant As String

            _formularioSBO.Freeze(True)

            MatrixEntradaPed.Matrix.FlushToDataSource()
            intSeleccion = MatrixEntradaPed.Matrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)

            Do While intSeleccion > -1
                l_list.Add(intSeleccion)
                intSeleccion = MatrixEntradaPed.Matrix.GetNextSelectedRow(intSeleccion, BoOrderType.ot_RowOrder)
            Loop


            l_list.Reverse()
            Dim num As Integer
            For Each num In l_list
                l_strCant = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cant_Ent", num - 1).Trim

                If Not String.IsNullOrEmpty(l_strCant) OrElse
                    l_strCant <> "0" Then

                    EliminarLineasUnidadesPorPedido(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Num_Ped", num - 1).Trim,
                                               _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Line_Ref", num - 1).Trim,
                                               _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Art", num - 1).Trim,
                                               _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cod_Col", num - 1).Trim)

                End If

                _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).RemoveRecord(num - 1)

            Next

            MatrixEntradaPed.Matrix.LoadFromDataSource()


            If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

            _formularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub EliminarLineasUnidad()
        Try
            Dim l_list As New List(Of Integer)
            Dim intSeleccion As Integer

            _formularioSBO.Freeze(True)
            MatrixEntradaVeh.Matrix.FlushToDataSource()

            intSeleccion = MatrixEntradaVeh.Matrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)

            Do While intSeleccion > -1
                l_list.Add(intSeleccion)
                intSeleccion = MatrixEntradaVeh.Matrix.GetNextSelectedRow(intSeleccion, BoOrderType.ot_RowOrder)
            Loop

            l_list.Reverse()

            Dim num As Integer
            For Each num In l_list
                _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).RemoveRecord(num - 1)
            Next

            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If


            _formularioSBO.Freeze(False)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub EliminarLineasUnidadesPorPedido(ByVal p_strNumPedido As String,
                                                ByVal p_strLineRef As String,
                                                ByVal p_strCodArt As String,
                                                ByVal p_strCodCol As String)
        Try

            Dim l_strNumPedido As String
            Dim l_strLineRef As String
            Dim l_strCodArt As String
            Dim l_strCodCol As String
            Dim l_list As New List(Of String)

            _formularioSBO.Freeze(True)
            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To MatrixEntradaVeh.Matrix.RowCount - 1

                l_strNumPedido = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Num_Ped", i).Trim
                l_strLineRef = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Line_Ref", i).Trim
                l_strCodArt = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Art", i).Trim
                l_strCodCol = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Col", i).Trim

                If l_strNumPedido.Equals(p_strNumPedido) AndAlso
                    l_strLineRef.Equals(p_strLineRef) AndAlso
                    l_strCodArt.Equals(p_strCodArt) AndAlso
                    l_strCodCol.Equals(p_strCodCol) Then

                    l_list.Add(i)

                End If


            Next

            l_list.Reverse()

            For Each num As Integer In l_list
                _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).RemoveRecord(num)
            Next

            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            _formularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    Private Function ValidarEliminarLineaPedido(ByRef BubbleEvent As Boolean) As Boolean
        Try
            Dim l_strUnidCreadas As String

            Dim l_blnResutl As Boolean = False
            l_strUnidCreadas = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).GetValue("U_UnidGen", 0).Trim

            If l_strUnidCreadas.Equals("Y") Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.MensajeEntradaDeVehiculosUnidadesCreadas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_blnResutl = True
                BubbleEvent = False
            ElseIf _applicationSbo.MessageBox(My.Resources.Resource.MensajeEntradaVehiculosBorrarLinEnt, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                l_blnResutl = True
                BubbleEvent = False
            End If


            Return l_blnResutl

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Function ValidarEliminarLineaUnidad(ByRef BubbleEvent As Boolean) As Boolean

        Dim l_blnResult As Boolean = False
        Dim l_strSQL As String = "Select Code from [@SCGD_Vehiculo] where U_Cod_Unid = '{0}'"
        Dim intSeleccion As Integer
        Dim l_strCodUnid As String
        Dim l_strUnidCreadas As String
        dtLocal = _formularioSBO.DataSources.DataTables.Item("dtLocal")
        dtLocal.Clear()

        Try

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            l_strUnidCreadas = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).GetValue("U_UnidGen", 0).Trim

            If l_strUnidCreadas.Equals("Y") Then
                _applicationSbo.StatusBar.SetText(My.Resources.Resource.Entrada1, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                l_blnResult = True
            Else
                If _applicationSbo.MessageBox(My.Resources.Resource.Entrada2, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    
                    intSeleccion = MatrixEntradaVeh.Matrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)

                    Do While intSeleccion > -1

                        l_strCodUnid = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Cod_Uni", intSeleccion - 1).Trim
                        l_strSQL = String.Format(l_strSQL, l_strCodUnid)

                        dtLocal.Clear()
                        dtLocal.ExecuteQuery(l_strSQL)

                        If Not String.IsNullOrEmpty(dtLocal.GetValue("Code", 0)) Then
                            _applicationSbo.StatusBar.SetText(My.Resources.Resource.Entrada3, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                            l_blnResult = True
                            Exit Do
                        ElseIf l_strUnidCreadas.Equals("Y") Then

                        End If
                        intSeleccion = MatrixEntradaVeh.Matrix.GetNextSelectedRow(intSeleccion, BoOrderType.ot_RowOrder)
                    Loop

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function





    Public Sub ManejadorEventoValidate(ByVal FormUID As String, ByRef pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try

            Dim oMatriz As SAPbouiCOM.Matrix
            oMatriz = DirectCast(FormularioSBO.Items.Item("mtx_Pedido").Specific, SAPbouiCOM.Matrix)
            oMatriz.FlushToDataSource()

            If pval.BeforeAction Then

            ElseIf pval.ActionSuccess Then
                If pval.ItemUID = "mtx_Pedido" Then
                    Select Case pval.ColUID
                        Case "col_Cant", "col_Cost", "col"
                            ActualizaCostosValores()
                    End Select
                End If

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Public Sub ManejadorEventosMenus(ByVal pval As SAPbouiCOM.MenuEvent, ByVal formUID As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Try
            Dim oItem As SAPbouiCOM.Item

            If pval.BeforeAction Then
                Select Case pval.MenuUID
                    Case 1284
                        CancelarEntrada(pval, BubbleEvent)
                    Case "SCGD_EDV"
                        ValidaTipoCambio(BubbleEvent)
                        '   ValidarCancelar(pval, BubbleEvent)
                End Select
            End If

            Select Case pval.MenuUID
                Case "1282"                 'BOTON NUEVO
                    FormularioSBO.Freeze(True)

                    txtFhaDoc.AsignaValorDataSource(Date.Now.ToString("yyyyMMdd"))
                    CargarMonedaLocal()
                    CargarSerieDocumento()
                    ManejoBtnUnidades(1)

                    AgregarPrimerLineaPedidos()
                    AgregarPrimerLineaUnidades()

                    FormularioSBO.EnableMenu("1282", False)

                    If Not FormularioSBO Is Nothing Then
                        oForm = ApplicationSBO.Forms.Item("SCGD_EDV")
                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next
                    End If

                    FormularioSBO.Items.Item(txtDocNum.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    FormularioSBO.Items.Item(cboEstadoDoc.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    FormularioSBO.Items.Item(cbxCancelado.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False)
                    FormularioSBO.Items.Item(cboTipoInv.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)

                    FormularioSBO.Items.Item(txtPrefijo.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)
                    FormularioSBO.Items.Item(txtConsecutivo.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_True)

                    MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColCod.UniqueId).Editable = True
                    MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColTip.UniqueId).Editable = True


                    FormularioSBO.Freeze(False)


                Case "1281"                 'BOTON BUSCAR

                    If Not FormularioSBO Is Nothing Then
                        oForm = ApplicationSBO.Forms.Item("SCGD_EDV")

                        FormularioSBO.Freeze(True)
                        For Each oItem In FormularioSBO.Items
                            oItem.Enabled = True
                        Next

                        FormularioSBO.EnableMenu("1282", True)

                        FormularioSBO.Freeze(False)
                    End If
                Case "1290", "1288", "1291", "1289"

                    FormularioSBO.EnableMenu("1282", True)

            End Select



        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Public Sub ManejadorEventoFormDataLoad(ByRef oTmpForm As SAPbouiCOM.Form)
        Try
            Dim l_strDocNum As String
            Dim l_strCardCode As String
            Dim oItem As SAPbouiCOM.Item
            Dim l_strUnidsGen As String
            Dim l_strDocEntry As String

            l_strDocNum = txtDocNum.ObtieneValorDataSource()
            l_strCardCode = txtCodProv.ObtieneValorDataSource()
            l_strUnidsGen = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).GetValue("U_UnidGen", 0).Trim

            'Call CargarMonedaSocio(l_strCardCode)

            If cboEstadoDoc.ObtieneValorDataSource() = "C" Then
                FormularioSBO.Mode = BoFormMode.fm_VIEW_MODE
                FormularioSBO.EnableMenu(1282, True)

                'LAS UNIDADES YA HAN SIDO GENERADAS
            ElseIf l_strUnidsGen = "Y" Then


                FormularioSBO.Items.Item("mtx_Pedido").Enabled = False
                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColCod.UniqueId).Editable = False
                MatrixEntradaVeh.Matrix.Columns.Item(MatrixEntradaVeh.ColumnaColTip.UniqueId).Editable = False

                FormularioSBO.Items.Item(cboTipoInv.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) ' Enabled = False
                FormularioSBO.Items.Item(txtPrefijo.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) 'Enabled = False
                FormularioSBO.Items.Item(txtConsecutivo.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) 'Enabled = False

                ManejoBtnUnidades(2)

            Else
                If Not FormularioSBO Is Nothing Then
                    oForm = ApplicationSBO.Forms.Item("SCGD_EDV")

                    FormularioSBO.Freeze(True)
                    For Each oItem In FormularioSBO.Items
                        oItem.Enabled = True
                    Next
                    FormularioSBO.Freeze(False)
                End If

                FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE
                FormularioSBO.Items.Item(txtDocNum.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) 'Enabled = False
                FormularioSBO.Items.Item(cboSerie.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) 'Enabled = False
                FormularioSBO.Items.Item(cboEstadoDoc.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) 'Enabled = False
                FormularioSBO.Items.Item(cbxCancelado.UniqueId).SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, -1, BoModeVisualBehavior.mvb_False) 'Enabled = False

                ManejoBtnUnidades(1)

            End If

            FormularioSBO.EnableMenu(1282, True)
            ActualizaCodigosDeUnidad()
            CargarValoresPorDefecto(oForm)
            MatrixEntradaVeh.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try

    End Sub

    Private Sub CargarValoresPorDefecto(ByRef oForm As SAPbouiCOM.Form)
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oMatriz As SAPbouiCOM.Matrix
        Dim str_DescDataRow As String
        Dim blnExiste As Boolean

        oMatriz = DirectCast(oForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)

        'If oMatriz.Columns.Item("col_Esti").ValidValues.Count > 0 Then
        '    For i As Integer = 0 To oMatriz.Columns.Item("col_Esti").ValidValues.Count - 1
        '        oMatriz.Columns.Item("col_Esti").ValidValues.Remove(oMatriz.Columns.Item("col_Esti").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
        '    Next
        'End If
        'If oMatriz.Columns.Item("col_Mode").ValidValues.Count > 0 Then
        '    For i As Integer = 0 To oMatriz.Columns.Item("col_Mode").ValidValues.Count - 1
        '        oMatriz.Columns.Item("col_Mode").ValidValues.Remove(oMatriz.Columns.Item("col_Mode").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
        '    Next
        'End If

        'For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").Size - 1
        '    blnExiste = False
        '    For l As Integer = 0 To oMatriz.Columns.Item("col_Esti").ValidValues.Count - 1
        '        If oMatriz.Columns.Item("col_Esti").ValidValues.Item(l).Value.Trim = oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim Then
        '            blnExiste = True
        '            Exit For
        '        End If
        '    Next
        '    If Not blnExiste Then
        '        str_DescDataRow = Utilitarios.EjecutarConsulta(String.Format("SELECT Name FROM [@SCGD_ESTILO] with (nolock) WHERE Code = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim), ApplicationSBO.Company.DatabaseName, ApplicationSBO.Company.ServerName)
        '        If str_DescDataRow.Length > 60 Then
        '            Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
        '            oMatriz.Columns.Item("col_Esti").ValidValues.Add(oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim, strDescripcion)
        '        Else
        '            oMatriz.Columns.Item("col_Esti").ValidValues.Add(oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim, str_DescDataRow)
        '        End If
        '    End If
        'Next
        'For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").Size - 1
        '    blnExiste = False
        '    For l As Integer = 0 To oMatriz.Columns.Item("col_Mode").ValidValues.Count - 1
        '        If oMatriz.Columns.Item("col_Mode").ValidValues.Item(l).Value.Trim = oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim Then
        '            blnExiste = True
        '            Exit For
        '        End If
        '    Next
        '    If Not blnExiste Then
        '        str_DescDataRow = Utilitarios.EjecutarConsulta(String.Format(" SELECT U_Descripcion AS Name FROM [@SCGD_MODELO] WHERE Code = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim), ApplicationSBO.Company.DatabaseName, ApplicationSBO.Company.ServerName)
        '        If str_DescDataRow.Length > 60 Then
        '            Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
        '            oMatriz.Columns.Item("col_Mode").ValidValues.Add(oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim, strDescripcion)
        '        Else
        '            oMatriz.Columns.Item("col_Mode").ValidValues.Add(oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim, str_DescDataRow)
        '        End If
        '    End If
        'Next
        oMatriz.LoadFromDataSource()
    End Sub

    Private Sub ActualizaCodigosDeUnidad()
        Try
            Dim l_strSQL As String

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQL = "Select U_Cod_Unid FROM [@SCGD_VEHICULO] where Code = '{0}'"

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                dtLocal.ExecuteQuery(String.Format(l_strSQL, FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", i).Trim))

                If Not String.IsNullOrEmpty(dtLocal.GetValue("U_Cod_Unid", 0)) Then
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Cod_Uni", i, dtLocal.GetValue("U_Cod_Unid", 0))
                End If

            Next

            MatrixEntradaVeh.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Protected Friend Sub CargarComboEstilos(ByRef oForm As SAPbouiCOM.Form, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef type As SAPbouiCOM.BoEventTypes)
        Try
            Dim l_strSQL As String
            Dim oItems As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim str_DescDataRow As String
            Dim blnExiste As Boolean = False
            Dim index As Integer = -1
            Dim descEstilo As String
            oForm.Freeze(True)

            oMatriz = DirectCast(oForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)
            Select Case type
                Case BoEventTypes.et_COMBO_SELECT

                    For i As Integer = 0 To dtEstiloLocal.Rows.Count - 1
                        If i <> pVal.Row - 1 Then
                            blnExiste = False
                            For l As Integer = 0 To oMatriz.Columns.Item("col_Esti").ValidValues.Count - 1
                                If oMatriz.Columns.Item("col_Esti").ValidValues.Item(l).Value.Trim = oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim Then
                                    blnExiste = True
                                    Exit For
                                End If
                            Next
                            If Not blnExiste Then
                                str_DescDataRow = dtEstiloLocal.GetValue("Name", i)

                                If str_DescDataRow.Length > 60 Then
                                    Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
                                    oMatriz.Columns.Item("col_Esti").ValidValues.Add(dtEstiloLocal.GetValue("Code", i), strDescripcion)
                                    Continue For
                                Else
                                    oMatriz.Columns.Item("col_Esti").ValidValues.Add(dtEstiloLocal.GetValue("Code", i), str_DescDataRow)
                                    Continue For
                                End If
                            End If
                        End If
                    Next

                    oMatriz.LoadFromDataSource()

                Case BoEventTypes.et_CLICK

                    dtEstiloLocal.Rows.Clear()
                    Dim oCombos As SAPbouiCOM.ComboBox
                    Dim oItem As SAPbouiCOM.Item

                    For i As Integer = 1 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size  ' oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").Size - 1

                        oItem = oMatriz.Columns.Item("col_Esti").Cells.Item(i).Specific
                        oCombos = CType(oItem.Specific, SAPbouiCOM.ComboBox)

                        Dim str As String = oCombos.Description

                    Next

                    For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").Size - 1

                        index = -1
                        descEstilo = String.Empty




                        For k As Integer = 0 To oMatriz.Columns.Item("col_Esti").ValidValues.Count - 1
                            index = k

                            If oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim = oMatriz.Columns.Item("col_Esti").ValidValues.Item(k).Value Then
                                dtEstiloLocal.Rows.Add(1)
                                dtEstiloLocal.Columns.Item("Code").Cells.Item(dtEstiloLocal.Rows.Count - 1).Value = oMatriz.Columns.Item("col_Esti").ValidValues.Item(k).Value
                                dtEstiloLocal.Columns.Item("Name").Cells.Item(dtEstiloLocal.Rows.Count - 1).Value = oMatriz.Columns.Item("col_Esti").ValidValues.Item(k).Description
                                index = -1

                                Exit For
                            End If
                        Next
                        If index <> -1 Then
                            dtEstiloLocal.Rows.Add(1)
                            dtEstiloLocal.Columns.Item("Code").Cells.Item(dtEstiloLocal.Rows.Count - 1).Value = oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim
                            descEstilo = Utilitarios.EjecutarConsulta(String.Format("SELECT Name FROM [@SCGD_ESTILO] with (nolock) WHERE Code = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", i).Trim), ApplicationSBO.Company.DatabaseName, ApplicationSBO.Company.ServerName)
                            dtEstiloLocal.Columns.Item("Name").Cells.Item(dtEstiloLocal.Rows.Count - 1).Value = descEstilo
                        End If

                    Next

                    If oMatriz.Columns.Item("col_Esti").ValidValues.Count > 0 Then
                        For i As Integer = 0 To oMatriz.Columns.Item("col_Esti").ValidValues.Count - 1
                            oMatriz.Columns.Item("col_Esti").ValidValues.Remove(oMatriz.Columns.Item("col_Esti").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                        Next
                    End If

                    dtEstilo.Rows.Clear()
                    dtEstilo.ExecuteQuery(String.Format("SELECT Code, Name, U_Cod_Marc FROM [@SCGD_ESTILO] with (nolock) WHERE U_Cod_Marc = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mar", pVal.Row - 1).Trim))
                    For i As Integer = 0 To dtEstilo.Rows.Count - 1
                        str_DescDataRow = dtEstilo.GetValue("Name", i)
                        If str_DescDataRow.Length > 60 Then
                            Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
                            oMatriz.Columns.Item("col_Esti").ValidValues.Add(dtEstilo.GetValue("Code", i), strDescripcion)
                        Else
                            oMatriz.Columns.Item("col_Esti").ValidValues.Add(dtEstilo.GetValue("Code", i), str_DescDataRow)
                        End If
                    Next

                    If pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
                        CargarComboEstilos(oForm, pVal, BoEventTypes.et_COMBO_SELECT)
                        oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Cod_Est", pVal.Row - 1, oMatriz.Columns.Item("col_Esti").ValidValues.Item(0).Value)
                        CargarComboModelo(oForm, pVal, BoEventTypes.et_CLICK)
                    End If

                    oMatriz.LoadFromDataSource()

            End Select

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Protected Friend Sub CargarComboModelo(ByRef oForm As SAPbouiCOM.Form, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef type As SAPbouiCOM.BoEventTypes)
        Try
            Dim l_strSQL As String
            Dim oItems As SAPbouiCOM.Item
            Dim oCombo As SAPbouiCOM.ComboBox
            Dim oMatriz As SAPbouiCOM.Matrix
            Dim str_DescDataRow As String
            Dim blnExiste As Boolean = False
            Dim index As Integer = -1
            Dim descModelo As String
            oForm.Freeze(True)

            oMatriz = DirectCast(oForm.Items.Item("mtx_Unidad").Specific, SAPbouiCOM.Matrix)
            Select Case type
                Case BoEventTypes.et_COMBO_SELECT

                    For i As Integer = 0 To dtModeloLocal.Rows.Count - 1
                        If i <> pVal.Row - 1 Then
                            blnExiste = False
                            For l As Integer = 0 To oMatriz.Columns.Item("col_Mode").ValidValues.Count - 1
                                If oMatriz.Columns.Item("col_Mode").ValidValues.Item(l).Value.Trim = oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim Then
                                    blnExiste = True
                                    Exit For
                                End If
                            Next
                            If Not blnExiste Then
                                str_DescDataRow = dtModeloLocal.GetValue("Name", i)

                                If str_DescDataRow.Length > 60 Then
                                    Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
                                    oMatriz.Columns.Item("col_Mode").ValidValues.Add(dtModeloLocal.GetValue("Code", i), strDescripcion)
                                    Continue For
                                Else
                                    oMatriz.Columns.Item("col_Mode").ValidValues.Add(dtModeloLocal.GetValue("Code", i), str_DescDataRow)
                                    Continue For
                                End If
                            End If
                        End If
                    Next

                    oMatriz.LoadFromDataSource()

                Case BoEventTypes.et_CLICK

                    dtModeloLocal.Rows.Clear()

                    For i As Integer = 0 To oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").Size - 1

                        index = -1
                        descModelo = String.Empty
                        For k As Integer = 0 To oMatriz.Columns.Item("col_Mode").ValidValues.Count - 1
                            index = k
                            If oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim = oMatriz.Columns.Item("col_Mode").ValidValues.Item(k).Value Then
                                dtModeloLocal.Rows.Add(1)
                                dtModeloLocal.Columns.Item("Code").Cells.Item(dtModeloLocal.Rows.Count - 1).Value = oMatriz.Columns.Item("col_Mode").ValidValues.Item(k).Value
                                dtModeloLocal.Columns.Item("Name").Cells.Item(dtModeloLocal.Rows.Count - 1).Value = oMatriz.Columns.Item("col_Mode").ValidValues.Item(k).Description
                                oMatriz.DeleteRow(k)
                                index = -1
                                Exit For
                            End If
                        Next
                        If index <> -1 Then
                            dtModeloLocal.Rows.Add(1)
                            dtModeloLocal.Columns.Item("Code").Cells.Item(dtModeloLocal.Rows.Count - 1).Value = oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim
                            descModelo = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Descripcion AS Name FROM [@SCGD_MODELO] WHERE Code = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Mod", i).Trim), ApplicationSBO.Company.DatabaseName, ApplicationSBO.Company.ServerName)
                            dtModeloLocal.Columns.Item("Name").Cells.Item(dtModeloLocal.Rows.Count - 1).Value = descModelo
                        End If

                    Next

                    If oMatriz.Columns.Item("col_Mode").ValidValues.Count > 0 Then
                        For i As Integer = 0 To oMatriz.Columns.Item("col_Mode").ValidValues.Count - 1
                            oMatriz.Columns.Item("col_Mode").ValidValues.Remove(oMatriz.Columns.Item("col_Mode").ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                        Next
                    End If
                    dtModelo.Rows.Clear()
                    dtModelo.ExecuteQuery(String.Format(" SELECT Code,U_Descripcion AS Name FROM [@SCGD_MODELO] Where U_Cod_Esti  = '{0}'", oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").GetValue("U_Cod_Est", pVal.Row - 1).Trim))
                    For i As Integer = 0 To dtModelo.Rows.Count - 1
                        str_DescDataRow = dtModelo.GetValue("Name", i)
                        If str_DescDataRow.Length > 60 Then
                            Dim strDescripcion As String = str_DescDataRow.Substring(0, 60)
                            oMatriz.Columns.Item("col_Mode").ValidValues.Add(dtModelo.GetValue("Code", i), strDescripcion)
                        Else
                            oMatriz.Columns.Item("col_Mode").ValidValues.Add(dtModelo.GetValue("Code", i), str_DescDataRow)
                        End If
                    Next

                    If pVal.EventType = BoEventTypes.et_COMBO_SELECT Then
                        CargarComboModelo(oForm, pVal, BoEventTypes.et_COMBO_SELECT)
                        oForm.DataSources.DBDataSources.Item("@SCGD_ENTRADA_UNID").SetValue("U_Cod_Mod", pVal.Row - 1, oMatriz.Columns.Item("col_Mode").ValidValues.Item(0).Value)
                    End If

                    oMatriz.LoadFromDataSource()

            End Select

            oForm.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub CancelarEntrada(ByRef pval As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Dim l_strSQLEntrada As String
            Dim l_strEntrada As String

            l_strEntrada = txtDocEntry.ObtieneValorDataSource

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQLEntrada = "Select CE.DocEntry, CA.U_Cod_Entrada, CA.U_Cod_Unid " +
                                " from [@SCGD_COSTEO_ENT] CE INNER JOIN [@SCGD_COST_ART] CA ON CE.DocEntry = CA.DocEntry " +
                                " where CE.Status = 'O' and CA.U_Cod_Entrada = '{0}'"


            If ApplicationSBO.MessageBox(My.Resources.Resource.MensajeEntradaDeVehiculosCancelarEntrada, 2, My.Resources.Resource.Si, My.Resources.Resource.No) = 2 Then
                BubbleEvent = False
            Else
                dtLocal.ExecuteQuery(String.Format(l_strSQLEntrada, l_strEntrada))
                If (dtLocal.GetValue("DocEntry", 0) <> 0) Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosNoPuedeEliminar, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    Exit Sub
                ElseIf ValidarPerteneceAContrato(BubbleEvent) Then
                    BubbleEvent = False
                    Exit Sub
                ElseIf ValidarTieneCosteos(BubbleEvent) Then
                    BubbleEvent = False
                    Exit Sub
                Else
                    BorrarDatoMaestroVehiculo()
                    ActualizarDatosPedidos(True)
                    If FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                        FormularioSBO.Items.Item("1").Click()
                    ElseIf FormularioSBO.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                        FormularioSBO.Items.Item("1").Click()
                    End If

                End If

            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function ValidarPerteneceAContrato(ByRef BubbleEvent As Boolean) As Boolean
        Dim l_blnRes As Boolean = False
        Dim l_strSQL As String
        Dim l_strSQLCV As String
        Dim l_strCodUnid As String
        Try
            l_strSQL = " Select CV.DocEntry, VE.U_Cod_Unid, CV.U_Reversa " +
                        " from [@SCGD_CVENTA] CV INNER JOIN [@SCGD_VEHIXCONT] VE ON CV.DocEntry = VE.DocEntry " +
                        " where CV.U_Reversa = 'N' AND  VE.U_Cod_Unid = '{0}'"

            l_strSQLCV = "Select U_CTOVTA FROM [@SCGD_VEHICULO] WHERE Code = '{0}'"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                l_strCodUnid = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Cod_Unid FROM [@SCGD_VEHICULO] where Code = '{0}'",
                                                                          FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", 0).Trim),
                                                                      _companySbo.CompanyDB, _companySbo.Server)
                If Not String.IsNullOrEmpty(l_strCodUnid) Then
                    dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strCodUnid))

                    If dtLocal.GetValue("DocEntry", 0) <> 0 Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosNoEliminaCV, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False
                        l_blnRes = True
                        Return l_blnRes
                    End If

                End If

                dtLocal.Clear()
                dtLocal.ExecuteQuery(String.Format(l_strSQLCV,
                                                   FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", i).Trim))

                If String.IsNullOrEmpty(dtLocal.GetValue("U_CTOVTA", 0)) OrElse
                    dtLocal.GetValue("U_CTOVTA", 0) <> 0 Then
                    ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosNoEliminaCV, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    l_blnRes = True
                    Return l_blnRes
                End If
            Next

            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    Private Function ValidarTieneCosteos(ByRef bubbleEvent As Boolean) As Boolean

        Try
            Dim l_blnRes As Boolean = False
            Dim l_strCodUnid As String
            Dim l_strSQL As String

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()

            l_strSQL = "Select U_TIPINV FROM [@SCGD_VEHICULO]  where U_Cod_Unid = '{0}'"

            MatrixEntradaVeh.Matrix.FlushToDataSource()

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1

                l_strCodUnid = Utilitarios.EjecutarConsulta(String.Format("SELECT U_Cod_Unid FROM [@SCGD_VEHICULO] where Code = '{0}'",
                                                                          FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_ID_Veh", 0).Trim),
                                                                      _companySbo.CompanyDB, _companySbo.Server)

                If Not String.IsNullOrEmpty(l_strCodUnid) Then

                    dtLocal.ExecuteQuery(String.Format(l_strSQL, l_strCodUnid))

                    If dtLocal.GetValue("U_TIPINV", 0) = "C" Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosNoEliminarPorCosteo, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        bubbleEvent = False
                        l_blnRes = True
                        Return l_blnRes
                    End If

                    If Utilitarios.ConsultaCosteos(l_strCodUnid, _companySbo.CompanyDB, _companySbo.Server, m_strMonSistema, m_strMonLocal, False) Then
                        ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeEntradaVehiculosNoEliminarCosteoAsoc, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        bubbleEvent = False
                        l_blnRes = True
                        Return l_blnRes
                    End If
                End If
            Next

            Return l_blnRes

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Sub BorrarDatoMaestroVehiculo()
        Try
            Dim l_strSQL As String
            Dim l_strCode As String

            l_strSQL = " DELETE FROM [@SCGD_VEHICULO] WHERE Code = '{0}'" +
                        " DELETE FROM [@SCGD_VEHITRAZA] WHERE Code = '{0}'" +
                        " DELETE FROM [@SCGD_ACCXVEH] WHERE Code = '{0}'" +
                        " DELETE FROM [@SCGD_BONOXVEH] WHERE Code = '{0}'"

            MatrixEntradaVeh.Matrix.FlushToDataSource()
            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                l_strCode = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Id_Veh", i).Trim

                If Not String.IsNullOrEmpty(l_strCode) Then
                    Utilitarios.EjecutarConsulta(String.Format(l_strSQL, l_strCode), _companySbo.CompanyDB, _companySbo.Server)
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Cod_Uni", i, String.Empty)
                    FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Id_Veh", i, String.Empty)
                    l_strCode = String.Empty
                End If
            Next

            MatrixEntradaVeh.Matrix.LoadFromDataSource()

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejoCambioMoneda()
        Try

            Dim l_decTotalEncabBase As Decimal
            Dim l_decTotalEncabDestino As Decimal

            Dim l_strMonDestino As String
            Dim l_strMonOrigen As String
            Dim l_strMonLocal As String = String.Empty
            Dim l_strMonSistema As String = String.Empty

            Dim l_decTCOrigen As Decimal
            Dim l_decTCDestino As Decimal
            Dim l_StrSQLSys As String
            Dim l_strTC As String

            _formularioSBO.Freeze(False)

            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_StrSQLSys)

            If Not String.IsNullOrEmpty(dtLocal.GetValue("MainCurncy", 0)) Then
                l_strMonLocal = dtLocal.GetValue("MainCurncy", 0)
                l_strMonSistema = dtLocal.GetValue("SysCurrncy", 0)
            End If

            l_strMonOrigen = m_strMonedaOrigen
            l_strMonDestino = m_strMonedaDestino


            '//////////////////////////////////////////
            l_strTC = ObtieneTipoCambio(l_strMonOrigen, Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
            l_decTCOrigen = Decimal.Parse(l_strTC)

            l_strTC = ObtieneTipoCambio(l_strMonDestino, Date.ParseExact(txtFhaDoc.ObtieneValorDataSource, "yyyyMMdd", Nothing))
            l_decTCDestino = Decimal.Parse(l_strTC)
            '//////////////////////////////////////////

            MatrixEntradaPed.Matrix.FlushToDataSource()
            MatrixEntradaVeh.Matrix.FlushToDataSource()

            Dim l_decPedidoCostoBase(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1) As Decimal
            Dim l_decPedidoTotalBase(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1) As Decimal

            Dim l_decPedidoCostoDestino(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1) As Decimal
            Dim l_decPedidoTotalDestino(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1) As Decimal

            Dim l_decUnidadesCostoBase(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1) As Decimal
            Dim l_decUnidadesCostoDestino(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1) As Decimal


            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
                l_decPedidoCostoBase(i) = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Cost_Veh", i), n)
                l_decPedidoTotalBase(i) = Decimal.Parse(FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Total_L", i), n)
            Next

            For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                l_decUnidadesCostoBase(i) = Decimal.Parse(_formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).GetValue("U_Monto_Gr", i), n)
            Next

            l_decTotalEncabBase = Decimal.Parse(txtTotal.ObtieneValorDataSource, n)
            l_decTotalEncabDestino = 0



            If l_strMonDestino = l_strMonOrigen Then

                For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
                    l_decPedidoCostoDestino(i) = l_decPedidoCostoBase(i)
                    l_decPedidoTotalDestino(i) = l_decPedidoTotalBase(i)
                Next

                For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                    l_decUnidadesCostoDestino(i) = l_decUnidadesCostoBase(i)
                Next

                l_decTotalEncabDestino = l_decTotalEncabBase

            ElseIf l_strMonOrigen <> l_strMonDestino Then
                If l_decTCDestino = 0 Then
                    l_decTCDestino = 1
                End If
                If l_decTCOrigen = 0 Then
                    l_decTCOrigen = 1
                End If

                If l_strMonOrigen = l_strMonLocal Then

                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
                        l_decPedidoCostoDestino(i) = l_decPedidoCostoBase(i) / l_decTCDestino
                        l_decPedidoTotalDestino(i) = l_decPedidoTotalBase(i) / l_decTCDestino
                    Next

                    For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                        l_decUnidadesCostoDestino(i) = l_decUnidadesCostoBase(i) / l_decTCDestino
                    Next

                    l_decTotalEncabDestino = l_decTotalEncabBase / l_decTCDestino

                ElseIf l_strMonDestino = l_strMonLocal Then
                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
                        l_decPedidoCostoDestino(i) = l_decPedidoCostoBase(i) * l_decTCOrigen
                        l_decPedidoTotalDestino(i) = l_decPedidoTotalBase(i) * l_decTCOrigen
                    Next

                    For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                        l_decUnidadesCostoDestino(i) = l_decUnidadesCostoBase(i) * l_decTCOrigen
                    Next

                    l_decTotalEncabDestino = l_decTotalEncabBase * l_decTCOrigen
                Else
                    For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
                        l_decPedidoCostoDestino(i) = (l_decPedidoCostoBase(i) * l_decTCOrigen) / l_decTCDestino
                        l_decPedidoTotalDestino(i) = (l_decPedidoTotalBase(i) * l_decTCOrigen) / l_decTCDestino
                    Next

                    For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                        l_decUnidadesCostoDestino(i) = (l_decUnidadesCostoBase(i) / l_decTCOrigen) / l_decTCDestino
                    Next

                    l_decTotalEncabDestino = (l_decTotalEncabBase * l_decTCOrigen) / l_decTCDestino
                End If

            End If

            For i As Integer = 0 To FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size - 1
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cost_Veh", i, l_decPedidoCostoDestino(i).ToString(n))
                FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Total_L", i, l_decPedidoTotalDestino(i).ToString(n))
            Next

            For i As Integer = 0 To _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).Size - 1
                _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaVehi).SetValue("U_Monto_Gr", i, l_decUnidadesCostoDestino(i).ToString(n))
            Next

            FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntrada).SetValue(txtTotal.ColumnaLigada, 0, l_decTotalEncabDestino.ToString(n))
            'txtTipoC.AsignaValorDataSource(l_decTotalEncabDestino.ToString(n))

            MatrixEntradaPed.Matrix.LoadFromDataSource()
            MatrixEntradaVeh.Matrix.LoadFromDataSource()

            _formularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Function ObtieneTipoCambio(ByVal p_StrMoneda As String, ByVal p_strFecha As Date) As String
        Try

            Dim l_strTipoC As String
            Dim l_strSQLTipoC As String
            Dim l_StrSQLSys As String

            l_strSQLTipoC = "select RateDate, Currency, Rate  from ORTT where RateDate = '{0}' and Currency = '{1}'"
            l_StrSQLSys = "select MainCurncy, SysCurrncy  from OADM"

            l_strSQLTipoC = String.Format(l_strSQLTipoC,
                                          Utilitarios.RetornaFechaFormatoDB(p_strFecha, _companySbo.Server),
                                          p_StrMoneda)
            dtLocal = FormularioSBO.DataSources.DataTables.Item("dtLocal")
            dtLocal.Clear()
            dtLocal.ExecuteQuery(l_strSQLTipoC)

            If String.IsNullOrEmpty(dtLocal.GetValue("Rate", 0)) Then
                l_strTipoC = -1
            Else
                l_strTipoC = dtLocal.GetValue("Rate", 0)
            End If


            Return l_strTipoC

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Function

    Private Sub ManejoBtnCrearUnidades(ByVal p_valor As Integer)
        Try
            Dim l_strSQL As String
            Dim l_strGeneradas As String
            Dim l_strDocEntry As String

            l_strDocEntry = txtDocEntry.ObtieneValorDataSource()

            l_strSQL = "SELECT U_UnidGen FROM [@SCGD_ENTRADA_VEH] where DocEntry = '{0}'"
            l_strGeneradas = Utilitarios.EjecutarConsulta(String.Format(l_strSQL, l_strDocEntry), _companySbo.CompanyDB, _companySbo.Server)

            Select Case p_valor
                Case EnableBtn.Mostrar
                    FormularioSBO.Items.Item(btnCrea.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                Case EnableBtn.Ocultar
                    FormularioSBO.Items.Item(btnCrea.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                Case EnableBtn.Evaluar
                    If l_strGeneradas.Equals("Y") Then
                        FormularioSBO.Items.Item(btnCrea.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                        FormularioSBO.Items.Item(btnActualiza.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    ElseIf l_strGeneradas.Equals("N") OrElse String.IsNullOrEmpty(l_strGeneradas) Then
                        FormularioSBO.Items.Item(btnCrea.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                        FormularioSBO.Items.Item(btnActualiza.UniqueId).SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    End If
            End Select
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub

    Private Sub ManejoBtnUnidades(ByVal p_intEstado As Integer)
        Try
            Dim oitem As SAPbouiCOM.Item
            Dim oBtn As SAPbouiCOM.Button


            Select Case p_intEstado
                Case 1  'Formulario Nuevo

                    _formularioSBO.Items.Item(btnGenera.UniqueId).Enabled = True
                    _formularioSBO.Items.Item(btnGenera.UniqueId).Visible = True

                    _formularioSBO.Items.Item(btnCrea.UniqueId).Visible = True

                    _formularioSBO.Items.Item(btnActualiza.UniqueId).Visible = False

                    oitem = _formularioSBO.Items.Item(btnActualiza.UniqueId)
                    oitem.FromPane = 2
                    oitem.ToPane = 2

                    oitem = _formularioSBO.Items.Item(btnCrea.UniqueId)
                    oitem.FromPane = 2
                    oitem.ToPane = 2

                    oitem = _formularioSBO.Items.Item(btnGenera.UniqueId)
                    oitem.FromPane = 2
                    oitem.ToPane = 2


                Case 2  'Unidades Creadas

                    _formularioSBO.Items.Item(btnGenera.UniqueId).Visible = False

                    _formularioSBO.Items.Item(btnGenera.UniqueId).Enabled = False
                    _formularioSBO.Items.Item(btnCrea.UniqueId).Visible = False
                    _formularioSBO.Items.Item(btnActualiza.UniqueId).Visible = True

                    oitem = _formularioSBO.Items.Item(btnActualiza.UniqueId)
                    oitem.FromPane = 2
                    oitem.ToPane = 2

                    oitem = _formularioSBO.Items.Item(btnGenera.UniqueId)
                    oitem.FromPane = 2
                    oitem.ToPane = 2


            End Select


        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try
    End Sub


    Public Sub AgregarLineasSeleccionadas(ByVal p_dtSeleccionados As SAPbouiCOM.DataTable)
        Try
            Dim l_intTamano As Integer
            Dim l_decMontoLinea As Decimal
            _formularioSBO = ApplicationSBO.Forms.Item("SCGD_EDV")



            MatrixEntradaPed = New MatrizEntradaPedido("mtx_Pedido", _formularioSBO, m_strTableEntradaPed)
            MatrixEntradaPed.Matrix.FlushToDataSource()

            l_intTamano = _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size

            For i As Integer = 0 To p_dtSeleccionados.Rows.Count - 1

                l_decMontoLinea = p_dtSeleccionados.GetValue("mont", i)

                If l_intTamano = 1 AndAlso
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).GetValue("U_Num_Ped", l_intTamano - 1).Equals(String.Empty) Then

                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Num_Ped", 0, p_dtSeleccionados.GetValue("pedi", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Art", 0, p_dtSeleccionados.GetValue("cart", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Desc_Art", 0, p_dtSeleccionados.GetValue("arti", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Ano_Veh", 0, p_dtSeleccionados.GetValue("ano", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Col", 0, p_dtSeleccionados.GetValue("colo", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cant_Ent", 0, p_dtSeleccionados.GetValue("pend", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cant_Veh", 0, p_dtSeleccionados.GetValue("pend", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Line_Ref", 0, p_dtSeleccionados.GetValue("line", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cost_Veh", 0, l_decMontoLinea.ToString(n))

                    ' l_intTamano = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size
                Else
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).InsertRecord(l_intTamano)
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Num_Ped", l_intTamano, p_dtSeleccionados.GetValue("pedi", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Art", l_intTamano, p_dtSeleccionados.GetValue("cart", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Desc_Art", l_intTamano, p_dtSeleccionados.GetValue("arti", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Ano_Veh", l_intTamano, p_dtSeleccionados.GetValue("ano", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cod_Col", l_intTamano, p_dtSeleccionados.GetValue("colo", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cant_Ent", l_intTamano, p_dtSeleccionados.GetValue("pend", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cant_Veh", l_intTamano, p_dtSeleccionados.GetValue("pend", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Line_Ref", l_intTamano, p_dtSeleccionados.GetValue("line", i))
                    _formularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).SetValue("U_Cost_Veh", l_intTamano, l_decMontoLinea.ToString(n))


                    ' l_intTamano = FormularioSBO.DataSources.DBDataSources.Item(m_strTableEntradaPed).Size


                    l_intTamano = l_intTamano + 1
                End If
            Next

            MatrixEntradaPed.Matrix.LoadFromDataSource()

            CargarDatosEncabezadoPedido(p_dtSeleccionados.GetValue("pedi", 0).ToString().Trim())

            Call ActualizaCostosValores()

            If _formularioSBO.Mode = BoFormMode.fm_OK_MODE Then
                _formularioSBO.Mode = BoFormMode.fm_UPDATE_MODE
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub


#End Region


End Class

#Region "Datos de vehiculo Costeo"

Public Class DatosVehiculoCosteo

    Private strCodUnid As String
    Private strIdUnid As String
    Private strTipoInv As String
    Private strCodMarca As String
    Private strCodEstilo As String
    Private strCodModelo As String
    Private strDebitAccount As String
    Private strCreditAccount As String
    Private strVIN As String
    Private strMonedaRegistro As String
    Private decMontoLocal As Decimal
    Private decMontoSistema As Decimal
    Private strCostingCode1 As String
    Private strCostingCode2 As String
    Private strCostingCode3 As String
    Private strCostingCode4 As String
    Private strCostingCode5 As String
    Private blnAplicaDim As Boolean
    Private strNumRecepcion As String
    Private strNumPedio As String
    Private strTipoTrans As String
    Private decTipoCambio As Decimal
    Private strNumEntrada As String
    Private strNumAsiento As String



    Public Property CodigoUnid() As String
        Get
            Return strCodUnid
        End Get
        Set(ByVal value As String)
            strCodUnid = value
        End Set
    End Property

    Public Property IdUnid() As String
        Get
            Return strIdUnid
        End Get
        Set(ByVal value As String)
            strIdUnid = value
        End Set
    End Property

    Public Property TipoInventario() As String
        Get
            Return strTipoInv
        End Get
        Set(ByVal value As String)
            strTipoInv = value
        End Set
    End Property

    Public Property CodigoMarca() As String
        Get
            Return strCodMarca
        End Get
        Set(ByVal value As String)
            strCodMarca = value
        End Set
    End Property

    Public Property CodigoEstilo() As String
        Get
            Return strCodEstilo
        End Get
        Set(ByVal value As String)
            strCodEstilo = value
        End Set
    End Property

    Public Property CodigoModelo() As String
        Get
            Return strCodModelo
        End Get
        Set(ByVal value As String)
            strCodModelo = value
        End Set
    End Property

    Public Property NumVIN() As String
        Get
            Return strVIN
        End Get
        Set(ByVal value As String)
            strVIN = value
        End Set
    End Property

    Public Property CuentaCredito() As String
        Get
            Return strCreditAccount
        End Get
        Set(ByVal value As String)
            strCreditAccount = value
        End Set
    End Property

    Public Property CuentaDebito() As String
        Get
            Return strDebitAccount
        End Get
        Set(ByVal value As String)
            strDebitAccount = value
        End Set
    End Property

    Public Property MonedaRegistro() As String
        Get
            Return strMonedaRegistro
        End Get
        Set(ByVal value As String)
            strMonedaRegistro = value
        End Set
    End Property


    Public Property MontoAsientoLocal() As Decimal
        Get
            Return decMontoLocal
        End Get
        Set(ByVal value As Decimal)
            decMontoLocal = value
        End Set
    End Property

    Public Property MontoAsientoSistema() As Decimal
        Get
            Return decMontoSistema
        End Get
        Set(ByVal value As Decimal)
            decMontoSistema = value
        End Set
    End Property

    Public Property CostingCode1() As String
        Get
            Return strCostingCode1
        End Get
        Set(ByVal value As String)
            strCostingCode1 = value
        End Set
    End Property

    Public Property CostingCode2() As String
        Get
            Return strCostingCode2
        End Get
        Set(ByVal value As String)
            strCostingCode2 = value
        End Set
    End Property

    Public Property CostingCode3() As String
        Get
            Return strCostingCode3
        End Get
        Set(ByVal value As String)
            strCostingCode3 = value
        End Set
    End Property

    Public Property CostingCode4() As String
        Get
            Return strCostingCode4
        End Get
        Set(ByVal value As String)
            strCostingCode4 = value
        End Set
    End Property

    Public Property CostingCode5() As String
        Get
            Return strCostingCode5
        End Get
        Set(ByVal value As String)
            strCostingCode5 = value
        End Set
    End Property

    Public Property NumeroPedido() As String
        Get
            Return strNumPedio
        End Get
        Set(ByVal value As String)
            strNumPedio = value
        End Set
    End Property

    Public Property NumeroRecepcion() As String
        Get
            Return strNumRecepcion
        End Get
        Set(ByVal value As String)
            strNumRecepcion = value
        End Set
    End Property

    Public Property TipoTransaccion() As String
        Get
            Return strTipoTrans
        End Get
        Set(ByVal value As String)
            strTipoTrans = value
        End Set
    End Property

    Public Property AplicaDimensiones() As Boolean
        Get
            Return blnAplicaDim
        End Get
        Set(ByVal value As Boolean)
            blnAplicaDim = value
        End Set
    End Property

    Public Property TipoCambio() As Decimal
        Get
            Return decTipoCambio
        End Get
        Set(ByVal value As Decimal)
            decTipoCambio = value
        End Set
    End Property

    Public Property NumeroEntrada() As String
        Get
            Return strNumEntrada
        End Get
        Set(ByVal value As String)
            strNumEntrada = value
        End Set
    End Property

    Public Property NumeroAsiento() As String
        Get
            Return strNumAsiento
        End Get
        Set(ByVal value As String)
            strNumAsiento = value
        End Set
    End Property

End Class

#End Region

