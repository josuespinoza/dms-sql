'
'Funcionalidad de la pantalla de Balance de Contratos de ventas
'------ Generacion de Costos por vehiculos y accesorios
'------ Obtención de utilidades para vehiculos y accesorios
'
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.DMSOne
Imports SCG.SBOFramework
Imports DMSOneFramework
Imports DMSOneFramework.SCGDataAccess
Imports System.Data.SqlClient
Imports System.Net.Configuration


Partial Public Class BalanceFormulario

#Region "Declaraciones"

    'Campos de la matriz vehiculo
    Public c_Unidad() As String
    Public c_Marca() As String
    Public c_Modelo() As String
    Public c_Estilo() As String
    Public c_ValVeh() As Decimal
    Public c_CosVeh() As Decimal
    Public c_UtilVeh() As Decimal
    Public c_PUtilVeh() As Decimal
    Public c_BonoVeh() As Decimal
    Public c_PreList() As Decimal
    Public c_Desc() As Decimal

    'campos de la matriz accesorios
    Public c_Codigo() As String
    Public c_Descripcion() As String
    Public c_ValAcc() As Decimal
    Public c_CosAcc() As Decimal
    Public c_UtilAcc() As Decimal
    Public c_PUtilAcc() As Decimal
    Public c_PreListAcc() As Decimal
    Public c_DescAcc() As Decimal

    'campos de la matriz tramites
    Public c_CodTra() As String
    Public c_DesTra() As String
    Public c_ValTra() As Decimal
    Public c_CosTra() As Decimal
    Public c_UtilTra() As Decimal
    Public c_PUtilTra() As Decimal

    'contrato
    Public contrato As String

    Private Const mc_strBtnPrint As String = "btnPrint"

#End Region

#Region "Eventos"

    <System.CLSCompliant(False)> _
    Public Function ManejadorEventoItemPress(ByRef pval As SAPbouiCOM.ItemEvent, ByVal FormUID As String, ByRef BubbleEvent As Boolean, ByVal m_SBO_Application As SAPbouiCOM.Application, ByVal company As SAPbobsCOM.ICompany) As Boolean
        Try
            'variables globales de la pantalla balance
            Dim oForm As SAPbouiCOM.Form
            Dim n As NumberFormatInfo

            'obtenemos el form de balance
            oForm = m_SBO_Application.Forms.Item(FormUID)

            'verifica el form
            If oForm IsNot Nothing Then
                oForm.Freeze(True)
                'decimales 
                n = DIHelper.GetNumberFormatInfo(company)
                If pval.Action_Success Then
                    Select Case pval.ItemUID
                        Case "btnCalc"
                            'totales generales a 0
                            CGeneral = 0
                            TGeneral = 0
                            UGeneral = 0
                            BGeneral = 0
                            ''llama al calcular los nuevos datos de la matriz
                            Call CalcularValores(oForm, n, "tVehiculos", "mtxVehic", "unidad", True, False, False, True)

                            Call CalcularValores(oForm, n, "tAccesorios", "mtxAcc", "codigo", False, True, False, False)

                            Call CalcularValores(oForm, n, "tTramites", "mtxTra", "codigo", False, False, True, False)

                        Case "btnActual"
                            'totales generales a 0
                            CGeneral = 0
                            TGeneral = 0
                            UGeneral = 0
                            BGeneral = 0
                            ''llama al calcular los nuevos datos de la matriz
                            Call Actualizar(oForm, n, "tVehiculos", "mtxVehic", "unidad", True, False, False, precioVentaOriginalVeh, costoOriginalVeh, bonoOriginalVeh, True, PreLisOriginalVeh, DescOriginalVeh)

                            Call Actualizar(oForm, n, "tAccesorios", "mtxAcc", "codigo", False, True, False, precioVentaOriginalAcc, costoOriginalAcc, bonoOriginalVeh, False, precioListOriginalAcc, DescuentoOriginalAcc)

                            Call Actualizar(oForm, n, "tTramites", "mtxTra", "codigo", False, False, True, precioVentaOriginalTra, costoOriginalTra, bonoOriginalVeh, False, Nothing, Nothing)

                        Case "btnPrint"
                            ImprimirReporteFacturaInterna(FormUID)
                    End Select
                End If
                oForm.Freeze(False)
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    Public Function ManejadorEventoLostFocus(ByRef pval As SAPbouiCOM.ItemEvent, _
                                                ByVal FormUID As String, _
                                                ByRef BubbleEvent As Boolean, _
                                                ByVal m_SBO_Application As SAPbouiCOM.Application, _
                                                ByVal company As SAPbobsCOM.ICompany) As Boolean

        'If pval.ItemUID = "mtx" And pval.ColUID = "clm" And pval.BeforeAction = True Then

        '    If "CellValue" = "" Then
        '        BubbleEvent = False
        '    End If
        'End If



        ''Manejo del evento lost focus
        'If pval.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
        '    If pval.FormMode = 1 _
        '        Or pval.FormMode = 2 _
        '        Or pval.FormMode = 3 Then

        '        If pval.ColUID = "U_SeqNo" Then

        '        ElseIf pval.ItemUID = "M1" Then

        '            If pval.ColUID = "U_PackQty" Then

        '                'oForm = SBO_Application.Forms.Item(pVal.FormUID);
        '                'SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("M1").Specific;
        '                'string tmpRecQty = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("").Cells.Item(pVal.Row).Specific).Value;





        '            End If
        '        End If
        '    End If
        'End If

    End Function

#End Region

#Region "Metodos"

#Region "Retorna: Comandos / Vectores llenos"

    'retorna las consulta para los costos de unidades
    Public Function RetornaComandoVehiculos(ByVal Bandera As String) As String

        Try
            'verifica que no utilice los costos proyectados o los reales
            'utiliza costro real 
            If Bandera = "N" Then
                Dim comando As String = "SELECT	SUM(G.U_GASTRA) AS COSTO, SUM(G.U_GASTRA_S) AS COSTO_S, G.U_Unidad AS UNIDAD FROM [@SCGD_GOODRECEIVE] AS G with(nolock) WHERE "
                Dim condicion As String = ""
                Dim agrupar As String = " GROUP BY G.U_Unidad "

                'verifica que el vector de unidades no sea nulo
                If c_Unidad IsNot Nothing And c_Unidad.Length > 0 Then
                    For i As Integer = 0 To c_Unidad.Length - 1
                        If i = 0 Then
                            condicion = condicion & "U_Unidad = '" & c_Unidad(i) & "'"
                        Else
                            condicion = condicion & "OR U_Unidad = '" & c_Unidad(i) & "'"
                        End If
                    Next

                    Return comando & condicion & agrupar
                Else
                    Return ""
                End If
            ElseIf Bandera = "Y" Then
                'utiliza costo proyectado 
                Dim comando As String = "SELECT U_CosPro  AS COSTO, U_Cod_Unid AS UNIDAD, U_Moneda AS MONEDA FROM [@SCGD_VEHICULO] with(nolock) WHERE "
                Dim condicion As String = ""

                'verifica que el vector de unidades no sea nulo
                If c_Unidad IsNot Nothing And c_Unidad.Length > 0 Then
                    For i As Integer = 0 To c_Unidad.Length - 1
                        If i = 0 Then
                            condicion = condicion & " U_Cod_Unid = '" & c_Unidad(i) & "'"
                        Else
                            condicion = condicion & " OR U_Cod_Unid = '" & c_Unidad(i) & "'"
                        End If
                    Next

                    Return comando & condicion
                Else
                    Return ""
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    'retorna la consulta para costos de accesorios
    Public Function RetornaComandoAccesoriso(ByVal top As String) As String

        Dim comando1 As String = "Select TOP("
        Dim comando2 As String = ") U_Cant_Acc AS CANTIDAD, U_Cost_Acc AS COSTO, U_Acc AS CODIGO FROM [@SCGD_ACCXCONT] WHERE DocEntry = '"
        Dim Order As String = "' ORDER BY LogInst DESC"

        Try
            'verifica que el vector de unidades no sea nulo
            If c_Codigo IsNot Nothing And c_Codigo.Length > 0 Then
                Return comando1 & top & comando2 & contrato & Order
            End If

            Return ""

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Function

    'Llena vectores de vehiuclos 
    Public Sub LlenaVectoresVehiculos(ByVal lsUnidad() As String,
                                      ByVal lsMarca() As String,
                                      ByVal lsModelo() As String,
                                      ByVal lsEstilo() As String,
                                      ByVal lsVal() As Decimal,
                                      ByVal lsBon() As Decimal,
                                      ByVal lsPreLis() As Decimal,
                                      ByVal lsDesc() As Decimal)
        With Me
            .c_Unidad = lsUnidad
            .c_Marca = lsMarca
            .c_Modelo = lsModelo
            .c_Estilo = lsEstilo
            .c_ValVeh = lsVal
            .c_BonoVeh = lsBon
            .c_PreList = lsPreLis
            .c_Desc = lsDesc

            'Copio datos originales para funcion actualizar
            Dim valorVehiculos(0 To lsVal.Length - 1) As Decimal
            Array.Copy(lsVal, valorVehiculos, lsVal.Length)

            precioVentaOriginalVeh = valorVehiculos

            'Copio datos originales para funcion actualizar
            Dim bonosVehiculos(0 To lsBon.Length - 1) As Decimal
            Array.Copy(lsBon, bonosVehiculos, lsBon.Length)

            bonoOriginalVeh = bonosVehiculos

            'Copio datos originales para funcion actualizar
            Dim PreLisVehiculos(0 To lsBon.Length - 1) As Decimal
            Array.Copy(lsPreLis, PreLisVehiculos, lsBon.Length)

            PreLisOriginalVeh = PreLisVehiculos

            'Copio datos originales para funcion actualizar
            Dim DescVehiculos(0 To lsBon.Length - 1) As Decimal
            Array.Copy(lsDesc, DescVehiculos, lsBon.Length)

            DescOriginalVeh = DescVehiculos

        End With
    End Sub

    'llena vectores accesorios
    Public Sub LlenaVectoresAccesorios(ByVal lsCod() As String,
                                       ByVal lsDes() As String,
                                       ByVal lsPre() As Decimal,
                                       ByVal lsCos() As Decimal,
                                       ByVal lsPreList() As Decimal,
                                       ByVal lsDescuento() As Decimal)
        With Me
            .c_Codigo = lsCod
            .c_Descripcion = lsDes
            .c_ValAcc = lsPre
            .c_CosAcc = lsCos
            .costoOriginalAcc = lsCos
            .c_PreListAcc = lsPreList
            .c_DescAcc = lsDescuento

            'Copio datos originales para funcion actualizar
            Dim valorAccesorios(0 To lsPre.Length - 1) As Decimal
            Array.Copy(lsPre, valorAccesorios, lsPre.Length)
            precioVentaOriginalAcc = valorAccesorios

            Dim descAccesorios(0 To lsDescuento.Length - 1) As Decimal
            Array.Copy(lsDescuento, descAccesorios, lsDescuento.Length)
            DescuentoOriginalAcc = descAccesorios

            Dim PreListAccesorios(0 To lsPreList.Length - 1) As Decimal
            Array.Copy(lsPreList, PreListAccesorios, lsPreList.Length)
            precioListOriginalAcc = PreListAccesorios

        End With
    End Sub

    ''' <summary>
    ''' Llena vectores de tramites
    ''' </summary>
    ''' <param name="lsCod">Códigos de trámites</param>
    ''' <param name="lsDes">Descripciones de Trámites</param>
    ''' <param name="lsPre">Precioes de Trámites</param>
    ''' <param name="lsCos">Costos de Trámites</param>
    ''' <remarks></remarks>
    Public Sub LlenaVectoresTramites(ByVal lsCod() As String, ByVal lsDes() As String, ByVal lsPre() As Decimal, ByVal lsCos() As Decimal)
        With Me
            .c_CodTra = lsCod
            .c_DesTra = lsDes
            .c_ValTra = lsPre
            .c_CosTra = lsCos
            .costoOriginalTra = lsCos

            'Copio datos originales para funcion actualizar
            Dim valorTramites(0 To lsPre.Length - 1) As Decimal
            Array.Copy(lsPre, valorTramites, lsPre.Length)

            precioVentaOriginalTra = valorTramites
        End With
    End Sub

#End Region '"Retorna: Comandos / Vectores llenos"

#Region "Acceso a datos"

    'retorna el tipo de cliente de acuerdo al id que se envia (1 o 2)
    Public Function RetornaTipoCliente(ByVal id As String) As String
        Try
            If Not String.IsNullOrEmpty(id) Then
                'consulta para retornar el tipo de clietne 
                Dim consulta As String = "Select Distinct UF.Descr AS Tipo " &
                            "from CUFD as CU left outer join UFD1 as UF on CU.FieldID = UF.FieldID " &
                            " where (UF.TableID = '@SCGD_CVENTA' and CU.AliasID = 'Tipo' and UF.FldValue = '" & id & "')"
                Return EjecutaConsulta(consulta)
            Else
                Return ""
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'retorna la descripcion de la moneda a partir del id
    Public Function RetornaModena(ByVal id As String) As String
        Try
            If Not String.IsNullOrEmpty(id) Then
                'consulta para seleccion de descripcion de moneda
                Dim consulta As String = "Select CurrName from [OCRN] with(nolock) where CurrCode = '" & id & "'"
                Return EjecutaConsulta(consulta)
            Else
                Return ""
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'retorna estado por contrato de ventas
    Public Function RetornaEstado(ByVal id As String) As String
        Try
            If Not String.IsNullOrEmpty(id) Then
                'consulta para seleccion de descripcion de moneda
                Dim consulta As String = "SELECT U_Estado FROM [@SCGD_NIVELES_PV] with(nolock) WHERE U_Nivel = '" & id & "'"
                Return EjecutaConsulta(consulta)
            Else
                Return ""
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'Retorna el valor de la primera fila y la primera columna 
    Public Function EjecutaConsulta(ByVal consulta As String) As String
        'manejo de la conexion
        Dim strConectionString As String = ""
        Dim conexion As New SqlConnection

        Try
            If _companySbo IsNot Nothing Then
                'obtiene la conexion y abre la misma
                Configuracion.CrearCadenaDeconexion(_companySbo.Server, _companySbo.CompanyDB, strConectionString)
                conexion.ConnectionString = strConectionString
                conexion.Open()

                'si no existe consulta que retorne ""
                If Not String.IsNullOrEmpty(consulta) Then
                    Dim comando As String
                    comando = consulta

                    Dim cmd As New SqlCommand(comando, conexion)

                    Dim dt As New System.Data.DataTable

                    'retorna el primer registro de la primera fila
                    Return cmd.ExecuteScalar()
                End If
                Return ""
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

#End Region '"Acceso a datos"

#Region "Costos / utilidades / Porcentajes"

    'genera los costos de los vehiculos 
    Public Function GeneraCostosXUnidadVehiculo(ByVal ser As String, ByVal bd As String,
                                                ByVal strCostoProyectado As String, ByVal company As SAPbobsCOM.ICompany,
                                                ByVal strMonedaSistema As String, ByVal strMonedaLocal As String, ByVal strMonedaCV As String,
                                                ByVal strTipoCambioCV As String, ByVal strTipoCambioSistema As String, ByVal fecha As Date) As Decimal()

        'manejo de conexion
        Dim strConectionString As String = ""
        Dim conexion As New SqlConnection
        Dim n As New Globalization.NumberFormatInfo

        Dim objGlobal As DMSOneFramework.BLSBO.GlobalFunctionsSBO
        Dim strTipoCambioCV_Local As String

        Try
            'retorna la cadena de conexion
            Configuracion.CrearCadenaDeconexion(ser, bd, strConectionString)
            conexion.ConnectionString = strConectionString
            'abre conexion
            conexion.Open()

            'decimales
            n = DIHelper.GetNumberFormatInfo(company)

            Dim comando As String
            'obtengo comando para vehiculos
            If strCostoProyectado = "Y" Then
                'utiliza costo proyectado? 
                comando = RetornaComandoVehiculos("Y")
            Else
                comando = RetornaComandoVehiculos("N")
            End If

            'verifica el comando a ejecutar
            If Not String.IsNullOrEmpty(comando) Then
                Dim cmd As New SqlCommand(comando, conexion)
                Dim dr As SqlDataReader
                Dim dt As New System.Data.DataTable

                costoOriginalVeh = Nothing

                'retorna comando en datareader para cargar en un datatable
                dr = cmd.ExecuteReader()
                dt.Load(dr)

                If dt IsNot Nothing And dt.Rows.Count > 0 _
                    And c_Unidad.Length > 0 Then
                    'declaro el vector de retorno
                    Dim vCos(0 To c_Unidad.Length - 1) As Decimal

                    'contador para ubicacion en vectores
                    Dim cont As Integer = 0

                    'ingresa los costos obtenidos por unidad de vehiculo
                    'en un vector de costos de vehiculos de la partial class .controles
                    'asignando costo a cada unidad
                    For Each i As String In c_Unidad

                        Dim dcCosPro As Decimal = 0

                        For Each row As DataRow In dt.Rows
                            'verifica si el costo se debe asignar a determinada unidad
                            If i = row("UNIDAD") Then
                                Dim costo As String = row("COSTO").ToString()
                                Dim strVacia As String = ""
                                If Not String.IsNullOrEmpty(costo) Then

                                    strTipoCambioCV_Local = strTipoCambioCV



                                    'Asignar el costo 
                                    If strCostoProyectado = "Y" And Not String.IsNullOrEmpty(costo) Then

                                        Dim strMoneda_CostoP As String = row("MONEDA").ToString()

                                        If strMoneda_CostoP <> strMonedaLocal And
                                        strMoneda_CostoP <> strMonedaSistema Then

                                            objGlobal = New DMSOneFramework.BLSBO.GlobalFunctionsSBO

                                            strTipoCambioCV_Local = objGlobal.RetornarTipoCambioMonedaRS(strMoneda_CostoP, fecha)

                                        End If

                                        'COSTO PROYECTADO
                                        '***************************************************************************
                                        '* Para generar el costo proyectado primero se verifica qeu tipo de moneda *
                                        '* se le asignó en el Maestro de Vehículos, dependiendo de esta y de la    *
                                        '* moneda del contrato de ventas se hará la respectiva conversión.         *
                                        '***************************************************************************
                                        Select Case strMoneda_CostoP

                                            'case Costo Proyectado = Moneda local
                                            Case strMonedaLocal
                                                Select Case strMonedaCV
                                                    'CASO MC = ML
                                                    Case strMonedaLocal
                                                        dcCosPro = Decimal.Parse(costo)
                                                        vCos(cont) = dcCosPro

                                                        'CASO MC = MS
                                                    Case strMonedaSistema
                                                        'valida tipo cambio nulo o vacio
                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                                                        'costo proyectado Local * tipo cambio sistema
                                                        dcCosPro = Decimal.Parse(costo) / tc_MonedaSistema
                                                        vCos(cont) = dcCosPro

                                                        'CASO MC <> ML <> MS
                                                    Case Else
                                                        'valida tipo cambio nulo o vacio
                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)
                                                        'valido tipo cambio del contrtato de ventas
                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'costo proyectado tipo cambio contrato ventas
                                                        dcCosPro = Decimal.Parse(costo) / tc_MonedaCV

                                                        vCos(cont) = dcCosPro
                                                End Select

                                                'case Costo Proyectado = Moneda sistema
                                            Case strMonedaSistema
                                                Select Case strMonedaCV
                                                    'CASO MC = ML
                                                    Case strMonedaLocal
                                                        'valida tipo cambio nulo o vacio
                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                                                        'costo proyectado Local * tipo cambio sistema
                                                        dcCosPro = Decimal.Parse(costo) * tc_MonedaSistema
                                                        vCos(cont) = dcCosPro

                                                        'CASO MC = MS
                                                    Case strMonedaSistema
                                                        dcCosPro = Decimal.Parse(costo)
                                                        vCos(cont) = dcCosPro

                                                        'CASO MC <> ML <> MS
                                                    Case Else
                                                        'valida tipo cambio nulo o vacio
                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)
                                                        'valido tipo cambio del contrtato de ventas
                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'costo proyectado * tipo de cambio del sistema / 
                                                        'tipo cambio contrato ventas
                                                        dcCosPro = Decimal.Parse(costo) * tc_MonedaSistema
                                                        dcCosPro = dcCosPro / tc_MonedaCV

                                                        vCos(cont) = dcCosPro
                                                End Select

                                                'case Costo Proyectado No es ni m_local ni m_sistema
                                            Case Else

                                                Select Case strMonedaCV
                                                    'CASO MC = ML
                                                    Case strMonedaLocal
                                                        'valido tipo cambio del contrtato de ventas
                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'costo proyectado * tipo de cambio del sistema / 
                                                        'tipo cambio contrato ventas
                                                        dcCosPro = Decimal.Parse(costo) * tc_MonedaCV

                                                        vCos(cont) = dcCosPro
                                                        'CASO MC = MS
                                                    Case strMonedaSistema
                                                        'valida tipo cambio nulo o vacio
                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)
                                                        'valido tipo cambio del contrtato de ventas
                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'costo proyectado * tipo de cambio del sistema / 
                                                        'tipo cambio contrato ventas
                                                        dcCosPro = Decimal.Parse(costo) / tc_MonedaCV
                                                        dcCosPro = dcCosPro * tc_MonedaSistema

                                                        vCos(cont) = dcCosPro
                                                        'CASO MC <> ML <> MS
                                                    Case Else
                                                        dcCosPro = Decimal.Parse(costo)
                                                        vCos(cont) = dcCosPro
                                                End Select
                                        End Select

                                    ElseIf strCostoProyectado = "N" Then
                                        'COSTO REAL
                                        Dim dcCostoCosteo As Decimal
                                        Dim dcCosReal As Decimal

                                        'Obtengo tipos de cambio
                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                            strTipoCambioSistema = 1
                                        End If
                                        Dim tc_MonedaSistema1 As Decimal = Decimal.Parse(strTipoCambioSistema)
                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                            strTipoCambioCV_Local = 1
                                        End If
                                        Dim tc_MonedaCV1 As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                        If Not String.IsNullOrEmpty(costo) Then
                                            'Se maneja la moneda del CV, 
                                            'para obtener el costo del GOODRECEIVE
                                            dcCostoCosteo = Decimal.Parse(costo)
                                        Else
                                            dcCostoCosteo = 0
                                        End If

                                        Dim strMoneda_CostoP As String = strMonedaLocal

                                        If strMoneda_CostoP <> strMonedaLocal And
                                        strMoneda_CostoP <> strMonedaSistema Then

                                            objGlobal = New DMSOneFramework.BLSBO.GlobalFunctionsSBO

                                            strTipoCambioCV_Local = objGlobal.RetornarTipoCambioMonedaRS(strMoneda_CostoP, fecha)

                                        End If

                                        '*****************************************
                                        'CASOS:     M_CV = ML
                                        '               M_C = ML
                                        '               M_C = MS
                                        '               M_C = MO
                                        '           M_CV = MS
                                        '               M_C = ML
                                        '               M_C = MS
                                        '               M_C = MO
                                        '           M_CV = MO
                                        '               M_C = ML
                                        '               M_C = MS
                                        '               M_C = MO
                                        '*****************************************
                                        Select Case strMonedaCV
                                            Case strMonedaLocal

                                                Select Case strMoneda_CostoP

                                                    Case strMonedaLocal, strVacia

                                                        'local = local
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        vCos(cont) = vCos(cont) + dcCosReal

                                                    Case strMonedaSistema

                                                        Dim costoSistema As Decimal

                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                                                        'local = costo * tc_sistema
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        costoSistema = dcCosReal * tc_MonedaSistema

                                                        vCos(cont) = vCos(cont) + costoSistema

                                                    Case Else

                                                        Dim costoOtro As Decimal

                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'local = costo * tc_cv
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        costoOtro = dcCosReal * tc_MonedaCV

                                                        vCos(cont) = vCos(cont) + costoOtro

                                                End Select

                                            Case strMonedaSistema

                                                Select Case strMoneda_CostoP

                                                    Case strMonedaLocal, strVacia

                                                        Dim costoLocal As Decimal

                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                                                        'sistema = costo / tc_ms
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        costoLocal = dcCosReal / tc_MonedaSistema

                                                        vCos(cont) = vCos(cont) + costoLocal

                                                    Case strMonedaSistema

                                                        'sistema = sistema
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        vCos(cont) = vCos(cont) + dcCosReal

                                                    Case Else

                                                        Dim costoOtro As Decimal

                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'sistema = (costo * tc_mcv) / tc_ms
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)

                                                        costoOtro = dcCosReal * tc_MonedaCV
                                                        costoOtro = costoOtro / tc_MonedaSistema

                                                        vCos(cont) = vCos(cont) + costoOtro

                                                End Select

                                            Case Else

                                                Select Case strMoneda_CostoP

                                                    Case strMonedaLocal, strVacia
                                                        Dim costoLocal As Decimal

                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'sistema = costo / tc_mcv
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        costoLocal = dcCosReal / tc_MonedaCV

                                                        vCos(cont) = vCos(cont) + costoLocal

                                                    Case strMonedaSistema

                                                        Dim costoSistema As Decimal

                                                        If String.IsNullOrEmpty(strTipoCambioSistema) Then
                                                            strTipoCambioSistema = 1
                                                        End If
                                                        Dim tc_MonedaSistema As Decimal = Decimal.Parse(strTipoCambioSistema)

                                                        If String.IsNullOrEmpty(strTipoCambioCV_Local) Then
                                                            strTipoCambioCV_Local = 1
                                                        End If
                                                        Dim tc_MonedaCV As Decimal = Decimal.Parse(strTipoCambioCV_Local)

                                                        'sistema = (costo * tc_ms) / tc_mcv
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)

                                                        costoSistema = dcCosReal * tc_MonedaSistema
                                                        costoSistema = costoSistema / tc_MonedaCV

                                                        vCos(cont) = vCos(cont) + costoSistema

                                                    Case Else

                                                        'otro = otro
                                                        dcCosReal = Decimal.Parse(dcCostoCosteo)
                                                        vCos(cont) = vCos(cont) + dcCosReal

                                                End Select

                                        End Select
                                    End If
                                End If
                            End If
                        Next
                        'si no se asigna un costo en la posicion de esta unidad se 
                        'asigna un 0
                        If vCos(cont) = Nothing Then
                            vCos(cont) = 0
                        End If
                        cont = cont + 1
                    Next

                    'asigna los costos originales 
                    If costoOriginalVeh Is Nothing Then
                        costoOriginalVeh = vCos
                    End If
                    Return vCos
                ElseIf c_Unidad.Length > 0 Then

                    'declaro el vector de retorno
                    Dim vCos(0 To c_Unidad.Length - 1) As Decimal

                    For i As Integer = 0 To c_Unidad.Length - 1
                        vCos(i) = 0
                    Next

                    'asigna los costos originales 
                    If costoOriginalVeh Is Nothing Then
                        costoOriginalVeh = vCos
                    End If

                    Return vCos
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'genera utilidad por vehiculos
    Public Function GeneraUtilidadXVehiculos() As Decimal()
        Try
            Dim strConfig As String
            strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

            If c_ValVeh IsNot Nothing And c_CosVeh IsNot Nothing Then
                If c_ValVeh.Length > 0 And c_CosVeh.Length > 0 Then
                    'declaracion de vector a retornar 
                    Dim vUtil(0 To c_ValVeh.Length) As Decimal

                    'declaracion de vector %utilidad
                    Dim vPUtil(0 To c_ValVeh.Length) As Decimal

                    'recorre el vector de valores (Precios ventas)
                    For i As Integer = 0 To c_ValVeh.Length - 1
                        'Utilidad = Precio Venta - Costo real
                        vUtil(i) = c_ValVeh(i) + c_BonoVeh(i) - c_CosVeh(i)

                        'If strConfig = "Y" Then
                        '    'putilidad = ( 100 / valor ) * utilidad
                        '    vPUtil(i) = (100 / c_CosVeh(i)) * vUtil(i)
                        'Else
                        '    'putilidad = ( 100 / valor ) * utilidad
                        '    vPUtil(i) = (100 - c_ValVeh(i) + c_BonoVeh(i)) * vUtil(i)
                        'End If
                    Next i

                    Return vUtil
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'genera utilidad accesorios
    Public Function GeneraUtilidadXAccesorios() As Decimal()
        Try
            If c_ValAcc IsNot Nothing And c_CosAcc IsNot Nothing Then
                If c_ValAcc.Length > 0 And c_CosAcc.Length > 0 Then
                    'ventor de retorno 
                    Dim vUtil(0 To c_ValAcc.Length - 1) As Decimal

                    'utilidad en % de accesorio
                    Dim vPUtil(0 To c_ValAcc.Length - 1) As Decimal

                    'recorrer el vector de precios de ventas
                    For i As Integer = 0 To c_ValAcc.Length - 1
                        'Utilidad = precio Venta - Costo Real
                        vUtil(i) = c_ValAcc(i) - c_CosAcc(i)

                    Next i

                    Return vUtil
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    ''' <summary>
    ''' Genra utilidad por Tramites
    ''' </summary>
    ''' <returns>Vector con la utilidad de los tramites </returns>
    ''' <remarks></remarks>
    Public Function GeneraUtilidadXTramites() As Decimal()
        Try
            If c_ValTra IsNot Nothing And c_CosTra IsNot Nothing Then
                If c_ValTra.Length > 0 And c_CosTra.Length > 0 Then
                    'ventor de retorno 
                    Dim vUtil(0 To c_ValTra.Length - 1) As Decimal

                    'utilidad en % de accesorio
                    Dim vPUtil(0 To c_ValTra.Length - 1) As Decimal

                    'recorrer el vector de precios de ventas
                    For i As Integer = 0 To c_ValTra.Length - 1
                        'Utilidad = precio Venta - Costo Real
                        vUtil(i) = c_ValTra(i) - c_CosTra(i)

                    Next i

                    Return vUtil
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'retorna vector con procentajes vehiculos
    Public Function GeneraPorcentajesVeh() As Decimal()
        Dim strConfig As String

        Try

            strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

            If c_ValVeh IsNot Nothing And c_UtilVeh IsNot Nothing Then
                If c_ValVeh.Length > 0 And c_UtilVeh.Length > 0 Then
                    'declaracion de vector %utilidad
                    Dim vPUtil(0 To c_ValVeh.Length) As Decimal

                    'recorre el vector de valores (Precios ventas)
                    For i As Integer = 0 To c_ValVeh.Length - 1

                        If strConfig = "Y" Then
                            If c_CosVeh(i) <> 0 AndAlso Not c_CosVeh Is Nothing Then
                                vPUtil(i) = (100 / c_CosVeh(i) + c_BonoVeh(i)) * c_UtilVeh(i)
                            Else
                                vPUtil(i) = 0
                            End If
                        Else
                            If c_ValVeh(i) <> 0 AndAlso Not c_ValVeh Is Nothing Then
                                vPUtil(i) = (100 / (c_ValVeh(i) + c_BonoVeh(i))) * c_UtilVeh(i)
                            Else
                                vPUtil(i) = 0
                            End If
                        End If

                    Next i

                    Return vPUtil
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    'retorna vector con procentajes accesorios
    Public Function GeneraPorcentajesAcc() As Decimal()
        Dim strConfig As String

        Try
            strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

            If c_ValAcc IsNot Nothing And c_UtilAcc IsNot Nothing Then
                If c_ValAcc.Length > 0 And c_UtilAcc.Length > 0 Then
                    'declaracion de vector %utilidad
                    Dim vPUtil(0 To c_ValAcc.Length) As Decimal

                    'recorre el vector de valores (Precios ventas)
                    For i As Integer = 0 To c_ValAcc.Length - 1
                        If strConfig = "Y" Then
                            If c_CosAcc(i) <> 0 AndAlso Not c_CosAcc Is Nothing Then
                                vPUtil(i) = (100 / c_CosAcc(i)) * c_UtilAcc(i)
                            Else
                                vPUtil(i) = 0
                            End If
                        Else
                            If c_ValAcc(i) <> 0 AndAlso Not c_ValAcc Is Nothing Then
                                vPUtil(i) = (100 / c_ValAcc(i)) * c_UtilAcc(i)
                            Else
                                vPUtil(i) = 0
                            End If
                        End If

                        vPUtil(i) = Math.Round(vPUtil(i), DMS_Connector.Company.AdminInfo.PercentageAccuracy, MidpointRounding.AwayFromZero)

                    Next i

                    Return vPUtil
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

    ''' <summary>
    ''' Genra vector con porcentajes de utilidades para Tramites 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GeneraPorcentajesTra() As Decimal()
        Dim strConfig As String

        Try
            strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

            If c_ValTra IsNot Nothing And c_UtilTra IsNot Nothing Then
                If c_ValTra.Length > 0 And c_UtilTra.Length > 0 Then
                    'declaracion de vector %utilidad
                    Dim vPUtil(0 To c_ValTra.Length) As Decimal

                    'recorre el vector de valores (Precios ventas)
                    For i As Integer = 0 To c_ValTra.Length - 1
                        'putilidad = ( 100 / costo ) * utilidad
                        If strConfig = "Y" Then
                            If c_CosTra(i) <> 0 AndAlso Not c_CosTra Is Nothing Then
                                vPUtil(i) = (100 / c_CosTra(i)) * c_UtilTra(i)
                            Else
                                vPUtil(i) = 0
                            End If
                        Else
                            If c_ValTra(i) <> 0 AndAlso Not c_ValTra Is Nothing Then
                                vPUtil(i) = (100 / c_ValTra(i)) * c_UtilTra(i)
                            Else
                                vPUtil(i) = 0
                            End If
                        End If

                        vPUtil(i) = Math.Round(vPUtil(i), DMS_Connector.Company.AdminInfo.PercentageAccuracy, MidpointRounding.AwayFromZero)

                    Next i

                    Return vPUtil
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Function

#End Region '"Costos / utilidades / Porcentajes"

#Region "Manejo de cambios en pantalla"

    'Actualizar la pantalla a los valores originales 
    Public Sub Actualizar(ByVal oForm As SAPbouiCOM.Form, _
                                 ByVal n As NumberFormatInfo, _
                                 ByVal nombreTabla As String, _
                                 ByVal nombreMatriz As String, _
                                 ByVal nombreColumna As String, _
                                 ByVal esVehiculo As Boolean, _
                                 ByVal esAccesorio As Boolean, _
                                 ByVal esTramite As Boolean, _
                                 ByVal precio As Decimal(), _
                                 ByVal costo As Decimal(), _
                                 ByVal bono As Decimal(), _
                                 ByVal esBono As Boolean, _
                                 ByVal PreLis As Decimal(), _
                                 ByVal Desc As Decimal())
        Try
            If precio IsNot Nothing _
                And costo IsNot Nothing Then
                If precio.Length > 0 _
                And costo.Length > 0 _
                And bono.Length > 0 Then

                    Dim objM As SAPbouiCOM.IMatrix
                    objM = DirectCast(oForm.Items.Item(nombreMatriz).Specific, SAPbouiCOM.Matrix)
                    Dim dt As SAPbouiCOM.DataTable = oForm.DataSources.DataTables.Item(nombreTabla)

                    Dim strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

                    If dt IsNot Nothing Then
                        If dt.Rows.Count > 0 Then
                            If Not String.IsNullOrEmpty(dt.GetValue(nombreColumna, 0)) Then

                                Dim l_uti(0 To precio.Length - 1) As Decimal
                                Dim l_puti(0 To precio.Length - 1) As Decimal

                                'CAMBIOS EN PRECIOS DE VENTA
                                'si no se han realizado cambios en porcentajes de utilidades
                                'y cambios en utilidades
                                For i As Integer = 0 To precio.Length - 1
                                    'cambio precio de venta 
                                    'utilidad = precio venta + bono - costo
                                    If esBono Then
                                        l_uti(i) = precio(i) + bono(i) - costo(i)
                                    Else
                                        l_uti(i) = precio(i) - costo(i)
                                    End If

                                    If strConfig = "Y" Then
                                        If costo(i) <> 0 AndAlso Not costo Is Nothing Then
                                            l_puti(i) = (100 / costo(i)) * l_uti(i)
                                        Else
                                            l_puti(i) = 0
                                        End If
                                    Else
                                        If precio(i) <> 0 AndAlso Not precio Is Nothing Then
                                            l_puti(i) = (100 / (precio(i) + bono(i))) * l_uti(i)
                                        Else
                                            l_puti(i) = 0
                                        End If
                                    End If

                                Next i

                                Call Pintar(precio.Length, precio, costo, l_uti, l_puti, dt, esVehiculo, esAccesorio, esTramite, bono, PreLis, Desc)

                                objM.LoadFromDataSource()
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Sub

    '*****************Funcionalidad para pantalla con valores cambiantes*************
    'calcular totales para modificacioens en
    '       *  porcentajes de utilidades
    '       *  cantidades de totales
    '       *  precios de venta


    Public Sub CalcularValores(ByVal oForm As SAPbouiCOM.Form, _
                                 ByVal n As NumberFormatInfo, _
                                 ByVal nombreTabla As String, _
                                 ByVal nombreMatriz As String, _
                                 ByVal nombreColumna As String, _
                                 ByVal esVehiculo As Boolean, _
                                 ByVal esAccesorio As Boolean, _
                                 ByVal esTramite As Boolean, _
                                 ByVal Bonos As Boolean)
        Try

            'capturo los datos sin cambios en pantalla
            dtValoresAntiguos = oForm.DataSources.DataTables.Item(nombreTabla)

            If dtValoresAntiguos IsNot Nothing Then
                'string locales
                Dim strPrecioVenta As String
                Dim strCosto As String
                Dim strUtilidad As String
                Dim strPUtilidad As String
                Dim strBono As String
                Dim strPreLis As String
                Dim strDesc As String


                If dtValoresAntiguos.Rows.Count > 0 Then
                    'listas de los datos sin modificar 
                    'SIN MODIFICAR
                    Dim dcAntiguoPreVenta(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoCosto(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoUtilidad(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoPUtilidad(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoBono(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoPreLis(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoDesc(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    Dim dcAntiguoPUtilidadCalculo(0 To dtValoresAntiguos.Rows.Count - 1) As Decimal
                    'banderas
                    Dim CambioPorcUtilidad As Boolean = False
                    Dim CambioUtilidad As Boolean = False
                    Dim CambioPreVenta As Boolean = False

                    Dim strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

                    If Not String.IsNullOrEmpty(dtValoresAntiguos.GetValue(nombreColumna, 0)) Then
                        'obtengo datos sin modificar

                        'verifica que no sea una linea vacia 
                        If Not String.IsNullOrEmpty(dtValoresAntiguos.GetValue(nombreColumna, 0)) Then
                            For i As Integer = 0 To dtValoresAntiguos.Rows.Count - 1
                                'MANEJO DEL CALCULAR    
                                'Obtiene los valores de la matriz y los carga en las listas
                                'verifica si el precio total, costo, utilidad, porcentaje 
                                ' de vehiculo es nulo o 0

                                strPrecioVenta = dtValoresAntiguos.GetValue("valor", i).ToString.Trim
                                If Not String.IsNullOrEmpty(strPrecioVenta) Then
                                    'asigno antiguo precio de venta
                                    dcAntiguoPreVenta(i) = Decimal.Parse(strPrecioVenta)
                                Else
                                    dcAntiguoPreVenta(i) = Decimal.Parse(0, n)
                                End If

                                strCosto = dtValoresAntiguos.GetValue("costo", i).ToString.Trim
                                If Not String.IsNullOrEmpty(strCosto) Then
                                    dcAntiguoCosto(i) = Decimal.Parse(strCosto)
                                End If

                                strUtilidad = dtValoresAntiguos.GetValue("utilidad", i).ToString.Trim
                                If Not String.IsNullOrEmpty(strUtilidad) Then
                                    dcAntiguoUtilidad(i) = Decimal.Parse(strUtilidad)
                                End If

                                strPUtilidad = dtValoresAntiguos.GetValue("putilidad", i).ToString.Trim
                                If Not String.IsNullOrEmpty(strPUtilidad) Then
                                    dcAntiguoPUtilidadCalculo(i) = Decimal.Parse(strPUtilidad)
                                    dcAntiguoPUtilidad(i) = Decimal.Parse(strPUtilidad)
                                    dcAntiguoPUtilidad(i) = Math.Round(dcAntiguoPUtilidad(i), DMS_Connector.Company.AdminInfo.PercentageAccuracy, MidpointRounding.AwayFromZero)
                                Else
                                    dcAntiguoPUtilidad(i) = Decimal.Parse(0)
                                End If

                                If Bonos Then
                                    strBono = dtValoresAntiguos.GetValue("bono", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strBono) Then
                                        dcAntiguoBono(i) = Decimal.Parse(strBono)
                                    Else
                                        dcAntiguoBono(i) = Decimal.Parse(0)
                                    End If
                                End If

                                If Not esTramite Then

                                    strPreLis = dtValoresAntiguos.GetValue("prelis", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strPreLis) Then
                                        dcAntiguoPreLis(i) = Decimal.Parse(strPreLis)
                                    Else
                                        dcAntiguoPreLis(i) = Decimal.Parse(0)
                                    End If

                                    strDesc = dtValoresAntiguos.GetValue("desc", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strDesc) Then
                                        dcAntiguoDesc(i) = Decimal.Parse(strDesc)
                                        dcAntiguoDesc(i) = Math.Round(dcAntiguoDesc(i), DMS_Connector.Company.AdminInfo.PercentageAccuracy, MidpointRounding.AwayFromZero)
                                    Else
                                        dcAntiguoDesc(i) = Decimal.Parse(0)
                                    End If

                                End If

                            Next i
                        End If

                    End If ' si existen datos en dtvaloresantiguos

                    'datos de pantalla ya modificados
                    Dim objM As SAPbouiCOM.IMatrix
                    objM = DirectCast(oForm.Items.Item(nombreMatriz).Specific, SAPbouiCOM.Matrix)
                    objM.FlushToDataSource()

                    dtValoresNuevos = oForm.DataSources.DataTables.Item(nombreTabla)
                    'si obtuvo los datos de pantalla
                    If dtValoresNuevos IsNot Nothing Then
                        If dtValoresNuevos.Rows.Count > 0 Then
                            'listas para tener los datos del datatable 
                            'YA MODIFICADOS
                            Dim dcModificadoPreVenta(0 To dtValoresNuevos.Rows.Count - 1) As Decimal
                            Dim dcModificadoCosto(0 To dtValoresNuevos.Rows.Count - 1) As Decimal
                            Dim dcModificadoUtilidad(0 To dtValoresNuevos.Rows.Count - 1) As Decimal
                            Dim dcModificadoPUtilidad(0 To dtValoresNuevos.Rows.Count - 1) As Decimal

                            'nuevos valores
                            Dim dcNuevoPreVenta(0 To dtValoresNuevos.Rows.Count - 1) As Decimal
                            Dim dcNuevoUtilidad(0 To dtValoresNuevos.Rows.Count - 1) As Decimal
                            Dim dcNuevoPUtilidad(0 To dtValoresNuevos.Rows.Count - 1) As Decimal
                            Dim dcNuevoDescuento(0 To dtValoresNuevos.Rows.Count - 1) As Decimal

                            'recorrer matriz de vehiculos con datos modificados

                            If Not String.IsNullOrEmpty(dtValoresNuevos.GetValue(nombreColumna, 0)) Then
                                For i As Integer = 0 To dtValoresNuevos.Rows.Count - 1

                                    'Obtiene los valores de la matriz y los carga en las listas
                                    'verifica si el precio total, costo, utilidad, porcentaje 
                                    ' de vehiculo es nulo o 0
                                    strPrecioVenta = dtValoresNuevos.GetValue("valor", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strPrecioVenta) Then
                                        dcModificadoPreVenta(i) = Decimal.Parse(strPrecioVenta)
                                    Else
                                        dcModificadoPreVenta(i) = Decimal.Parse(0)
                                    End If

                                    strCosto = dtValoresNuevos.GetValue("costo", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strCosto) Then
                                        dcModificadoCosto(i) = Decimal.Parse(strCosto)
                                    End If

                                    strUtilidad = dtValoresNuevos.GetValue("utilidad", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strUtilidad) Then
                                        dcModificadoUtilidad(i) = Decimal.Parse(strUtilidad)
                                    End If

                                    strPUtilidad = dtValoresNuevos.GetValue("putilidad", i).ToString.Trim
                                    If Not String.IsNullOrEmpty(strPUtilidad) Then
                                        dcModificadoPUtilidad(i) = Decimal.Parse(strPUtilidad)
                                        dcModificadoPUtilidad(i) = Math.Round(dcModificadoPUtilidad(i), DMS_Connector.Company.AdminInfo.PercentageAccuracy, MidpointRounding.AwayFromZero)
                                    Else
                                        dcModificadoPUtilidad(i) = Decimal.Parse(0)
                                    End If

                                    If Not esTramite Then
                                        strDesc = dtValoresNuevos.GetValue("desc", i).ToString.Trim
                                        If Not String.IsNullOrEmpty(strDesc) Then
                                            dcNuevoDescuento(i) = Decimal.Parse(strDesc)
                                        Else
                                            dcNuevoDescuento(i) = Decimal.Parse(0)
                                        End If
                                    End If

                                Next i
                            End If

                            '***** CALCULO DE LAS NUEVAS CANTIDADES *****'
                            '
                            'PRIMERO: Se realizan cambios para las alteraciones en las cantidades de porcentajes'
                            'SEGUNDO: Se modifican las cantidades al alterar las cantidades de utilidades'
                            'TERCERO: Se cambian los totales al modificar los precios de venta
                            '
                            '*********************************************
                            If dcAntiguoPreVenta.Length > 0 _
                                And dcAntiguoCosto.Length > 0 _
                                And dcAntiguoPUtilidad.Length > 0 _
                                And dcAntiguoUtilidad.Length > 0 _
                                And dcAntiguoBono.Length > 0 _
                                And dcModificadoPreVenta.Length > 0 _
                                And dcModificadoCosto.Length > 0 _
                                And dcModificadoUtilidad.Length > 0 _
                                And dcModificadoPUtilidad.Length > 0 _
                                And dcNuevoPreVenta.Length > 0 _
                                And dcNuevoUtilidad.Length > 0 _
                                And dcNuevoPUtilidad.Length > 0 Then

                                'CAMBIOS EN PORCENTAJES DE UTILIDADES
                                'si no se han realizado cambios en utilidades
                                'y cambios en precios de ventas
                                For x1 As Integer = 0 To dtValoresNuevos.Rows.Count - 1
                                    If CambioPreVenta = False _
                                        And CambioUtilidad = False Then
                                        'cambio porcentajes de utilidades 
                                        If Not dcAntiguoPUtilidad(x1) = dcModificadoPUtilidad(x1) Then

                                            If Not dcModificadoPUtilidad(x1) = 0 Then
                                                If Not dcAntiguoCosto(x1) = 0 Then

                                                    If strConfig = "Y" Then
                                                        dcNuevoPreVenta(x1) = ((dcModificadoCosto(x1) * dcModificadoPUtilidad(x1)) / dcAntiguoPUtilidadCalculo(x1))
                                                    Else
                                                        dcNuevoPreVenta(x1) = ((dcModificadoPreVenta(x1) * dcModificadoPUtilidad(x1)) / dcAntiguoPUtilidadCalculo(x1))
                                                    End If

                                                    'dcNuevoPreVenta(x1) = ((dcAntiguoCosto(x1) / 100) * dcModificadoPUtilidad(x1)) + dcAntiguoCosto(x1)

                                                    'porcentaje de utilidad
                                                    dcNuevoPUtilidad(x1) = dcModificadoPUtilidad(x1)
                                                    'valida negativos en procentajes de utilidad
                                                    If dcNuevoPUtilidad(x1) < 0 Then dcNuevoPUtilidad(x1) = 0
                                                    'utilidad = (precio venta - costo)
                                                    dcNuevoUtilidad(x1) = (dcNuevoPreVenta(x1) + dcAntiguoBono(x1)) - dcAntiguoCosto(x1)
                                                Else
                                                    'precio de venta
                                                    dcNuevoPreVenta(x1) = dcAntiguoPreVenta(x1)
                                                    'porcentaje de utilidad
                                                    dcNuevoPUtilidad(x1) = dcAntiguoPUtilidad(x1)
                                                    'valida negativos en procentajes de utilidad
                                                    If dcNuevoPUtilidad(x1) < 0 Then dcNuevoPUtilidad(x1) = 0
                                                    'utilidad = (precio venta - costo)
                                                    dcNuevoUtilidad(x1) = (dcAntiguoPreVenta(x1) + dcAntiguoBono(x1)) - dcAntiguoCosto(x1)
                                                End If
                                            Else
                                                dcNuevoPreVenta(x1) = dcAntiguoCosto(x1)
                                                dcNuevoUtilidad(x1) = 0
                                                dcNuevoPUtilidad(x1) = dcModificadoPUtilidad(x1)
                                            End If
                                            'hubo un cambio en porcentajes
                                            CambioPorcUtilidad = True
                                        Else
                                            dcNuevoPUtilidad(x1) = dcAntiguoPUtilidad(x1)
                                            'valida negativos en procentajes de utilidad
                                            If dcNuevoPUtilidad(x1) < 0 Then dcNuevoPUtilidad(x1) = 0
                                            dcNuevoPreVenta(x1) = dcAntiguoPreVenta(x1)
                                            dcNuevoUtilidad(x1) = dcAntiguoUtilidad(x1)
                                        End If

                                        If Not esTramite Then
                                            dcNuevoDescuento(x1) = ((dcAntiguoPreLis(x1) - dcNuevoPreVenta(x1)) * 100) / dcAntiguoPreLis(x1)
                                        End If


                                    End If
                                Next x1

                                'CAMBIOS EN UTILIDADES
                                'si no se han realizado cambios en porcentajes de utilidades
                                'y cambios en precios de ventas
                                For x2 As Integer = 0 To dtValoresNuevos.Rows.Count - 1
                                    If CambioPorcUtilidad = False _
                                      And CambioPreVenta = False Then

                                        'cambio utilidades 
                                        If Not dcAntiguoUtilidad(x2) = dcModificadoUtilidad(x2) Then
                                            'si el porcentajes de utilidad y el precio de venta no son 0
                                            If Not dcModificadoUtilidad(x2) = 0 _
                                                And Not dcModificadoPreVenta(x2) = 0 Then
                                                'dcprecio venta = costo + utilidad 
                                                dcNuevoPreVenta(x2) = dcAntiguoCosto(x2) + dcModificadoUtilidad(x2)
                                                If Not dcAntiguoCosto(x2) = 0 Then
                                                    If strConfig = "Y" Then
                                                        dcNuevoPUtilidad(x2) = (100 / dcAntiguoCosto(x2)) * dcModificadoUtilidad(x2)
                                                    Else
                                                        dcNuevoPUtilidad(x2) = (100 / dcNuevoPreVenta(x2) + dcAntiguoBono(x2)) * dcModificadoUtilidad(x2)
                                                    End If

                                                    'valida negativos en procentajes de utilidad
                                                    If dcNuevoPUtilidad(x2) < 0 Then dcNuevoPUtilidad(x2) = 0
                                                ElseIf dcAntiguoCosto(x2) = 0 _
                                                    And Not dcNuevoUtilidad(x2) = 0 _
                                                    And Not dcNuevoPreVenta(x2) = 0 Then
                                                    dcNuevoPUtilidad(x2) = 100
                                                Else
                                                    dcNuevoPUtilidad(x2) = 0
                                                End If
                                                dcNuevoUtilidad(x2) = dcModificadoUtilidad(x2)
                                            Else
                                                dcNuevoUtilidad(x2) = 0
                                                dcNuevoPreVenta(x2) = dcAntiguoCosto(x2)
                                                dcNuevoPUtilidad(x2) = 0
                                            End If
                                            'hubo un cambio en utilidades 
                                            CambioUtilidad = True
                                        Else
                                            dcNuevoPUtilidad(x2) = dcAntiguoPUtilidad(x2)
                                            'valida negativos en procentajes de utilidad
                                            If dcNuevoPUtilidad(x2) < 0 Then dcNuevoPUtilidad(x2) = 0
                                            dcNuevoPreVenta(x2) = dcAntiguoPreVenta(x2)
                                            dcNuevoUtilidad(x2) = dcAntiguoUtilidad(x2)
                                        End If

                                        If Not esTramite Then
                                            dcNuevoDescuento(x2) = ((dcAntiguoPreLis(x2) - dcNuevoPreVenta(x2)) * 100) / dcAntiguoPreLis(x2)
                                        End If

                                    End If
                                Next x2

                                'CAMBIOS EN PRECIOS DE VENTA
                                'si no se han realizado cambios en porcentajes de utilidades
                                'y cambios en utilidades
                                For x3 As Integer = 0 To dtValoresNuevos.Rows.Count - 1
                                    If CambioPorcUtilidad = False _
                                    And CambioUtilidad = False Then
                                        'cambio precio de venta 
                                        If Not dcAntiguoPreVenta(x3) = dcModificadoPreVenta(x3) Then
                                            'utilidad = precio venta - costo
                                            dcNuevoUtilidad(x3) = (dcModificadoPreVenta(x3) + dcAntiguoBono(x3)) - dcModificadoCosto(x3)
                                            If Not dcModificadoCosto(x3) = 0 Then
                                                'porcentaje = (100/costo) * utilidad 
                                                If strConfig = "Y" Then
                                                    dcNuevoPUtilidad(x3) = (100 / dcModificadoCosto(x3)) * dcNuevoUtilidad(x3)
                                                Else
                                                    dcNuevoPUtilidad(x3) = (100 / dcModificadoPreVenta(x3)) * dcNuevoUtilidad(x3)
                                                End If

                                                'valida negativos en procentajes de utilidad
                                                If dcNuevoPUtilidad(x3) < 0 Then dcNuevoPUtilidad(x3) = 0
                                            ElseIf dcModificadoPreVenta(x3) > 0 _
                                                And dcModificadoCosto(x3) = 0 Then
                                                dcNuevoPUtilidad(x3) = 100
                                            Else
                                                dcNuevoPUtilidad(x3) = 0
                                            End If
                                            'precio de venta
                                            dcNuevoPreVenta(x3) = dcModificadoPreVenta(x3)
                                            CambioPreVenta = True
                                        Else
                                            dcNuevoPUtilidad(x3) = dcAntiguoPUtilidad(x3)
                                            'valida negativos en procentajes de utilidad
                                            If dcNuevoPUtilidad(x3) < 0 Then dcNuevoPUtilidad(x3) = 0
                                            dcNuevoPreVenta(x3) = dcAntiguoPreVenta(x3)
                                            dcNuevoUtilidad(x3) = dcAntiguoUtilidad(x3)
                                        End If

                                        If Not esTramite Then
                                            dcNuevoDescuento(x3) = ((dcAntiguoPreLis(x3) - dcNuevoPreVenta(x3)) * 100) / dcAntiguoPreLis(x3)
                                        End If

                                    End If
                                Next x3
                                'retorna el total de la suma de los items dentro del vector
                                'carga los totales a pantalla 
                                FormularioSBO = oForm

                            End If
                            'verifica que existan cambios 
                            If CambioPorcUtilidad _
                                Or CambioPreVenta _
                                Or CambioUtilidad _
                                And dcNuevoPreVenta IsNot Nothing _
                                And dcNuevoPUtilidad IsNot Nothing _
                                And dcNuevoUtilidad IsNot Nothing Then

                                If esVehiculo Then
                                    'se pintan en pantalla los vehiculos 
                                    Call Pintar(dtValoresNuevos.Rows.Count,
                                                dcNuevoPreVenta,
                                                dcModificadoCosto,
                                                dcNuevoUtilidad,
                                                dcNuevoPUtilidad,
                                                dtValoresNuevos,
                                                True,
                                                False,
                                                False,
                                                dcAntiguoBono, dcAntiguoPreLis, dcNuevoDescuento)
                                ElseIf esAccesorio Then
                                    'se pintan en pantalla los accesorios
                                    Call Pintar(dtValoresNuevos.Rows.Count,
                                                dcNuevoPreVenta,
                                                dcModificadoCosto,
                                                dcNuevoUtilidad,
                                                dcNuevoPUtilidad,
                                                dtValoresNuevos,
                                                False,
                                                True,
                                                False,
                                                dcAntiguoBono, dcAntiguoPreLis, dcNuevoDescuento)
                                ElseIf esTramite Then
                                    'se pintan en pantalla los tramites
                                    Call Pintar(dtValoresNuevos.Rows.Count,
                                                dcNuevoPreVenta,
                                                dcModificadoCosto,
                                                dcNuevoUtilidad,
                                                dcNuevoPUtilidad,
                                                dtValoresNuevos,
                                                False,
                                                False,
                                                True,
                                                dcAntiguoBono, dcAntiguoPreLis, dcNuevoDescuento)
                                End If
                                'actualizo la pantalla
                                objM.LoadFromDataSource()
                            Else
                                'si no existen cambios se pintan los valores anteiores
                                If esVehiculo Then
                                    'se pintan en pantalla los vehiculos 
                                    Call Pintar(dtValoresAntiguos.Rows.Count,
                                                dcAntiguoPreVenta,
                                                dcAntiguoCosto,
                                                dcAntiguoUtilidad,
                                                dcAntiguoPUtilidad,
                                                dtValoresAntiguos,
                                                True,
                                                False,
                                                False,
                                                dcAntiguoBono, dcAntiguoPreLis, dcNuevoDescuento)
                                ElseIf esAccesorio Then
                                    'se pintan en pantalla los accesorios
                                    Call Pintar(dtValoresAntiguos.Rows.Count,
                                                dcAntiguoPreVenta,
                                                dcAntiguoCosto,
                                                dcAntiguoUtilidad,
                                                dcAntiguoPUtilidad,
                                                dtValoresAntiguos,
                                                False,
                                                True,
                                                False,
                                                dcAntiguoBono, dcAntiguoPreLis, dcNuevoDescuento)
                                ElseIf esTramite Then
                                    'se pintan en pantalla los accesorios
                                    Call Pintar(dtValoresAntiguos.Rows.Count,
                                                dcAntiguoPreVenta,
                                                dcAntiguoCosto,
                                                dcAntiguoUtilidad,
                                                dcAntiguoPUtilidad,
                                                dtValoresAntiguos,
                                                False,
                                                False,
                                                True,
                                                dcAntiguoBono, dcAntiguoPreLis, dcNuevoDescuento)
                                End If
                                'actualizo la pantalla
                                objM.LoadFromDataSource()
                            End If 'pintar
                        End If 'valores nuevos rows >0
                    End If 'valores nuevos is not null
                End If 'antiguos rowcount > 0
            End If 'valores antiguos isnot null
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Sub

    'Recorre la lista sumando los items dentro de la misma, para retornarlos
    'al final ya sumarizado
    Public Function RetornaTotalPorLista(ByVal lista As Decimal()) As Decimal
        Dim Total As Decimal = 0
        For Each Item As Decimal In lista
            Total = Total + Item
        Next
        Return Total
    End Function

#End Region '"Manejo de cambios en pantalla"

    ''' <summary>
    ''' Agrega Boton de Imprimir Balance
    ''' </summary>
    ''' <param name="oform">Objeto de Formulario</param>
    ''' <remarks></remarks>
    Public Function ValidaPermisoBTNPrint(ByVal oform As SAPbouiCOM.Form, ByVal p_SBO_Application As SAPbouiCOM.Application) As Boolean

        Dim oItem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Dim intTop As Integer
        Dim intLeft As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Try
            If Utilitarios.MostrarMenu("SCGD_CVP", p_SBO_Application.Company.UserName) Then
                intTop = oform.Items.Item("btnCalc").Top
                intLeft = oform.Items.Item("btnCalc").Left
                intWidth = oform.Items.Item("btnCalc").Width
                intHeight = oform.Items.Item("btnCalc").Height

                oItem = oform.Items.Add(mc_strBtnPrint, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                oItem.Top = intTop
                oItem.Left = intLeft - 80
                oItem.Width = intWidth
                oItem.Height = intHeight

                oItem.Enabled = True
                oItem.Visible = True

                oButton = oItem.Specific
                oButton.Type = SAPbouiCOM.BoButtonTypes.bt_Caption
                oButton.Caption = My.Resources.Resource.TXTPrint
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, ApplicationSBO)
        End Try
    End Function

    Public Sub ImprimirReporteFacturaInterna(ByVal FormUID As String)

        Dim strDireccionReporte As String = String.Empty
        Dim strPathExe As String
        Dim strParametrosDocEntry As String
        Dim strParametros As String
        Dim oForm As SAPbouiCOM.Form
        Dim m_cn_Coneccion As New SqlClient.SqlConnection
        Dim m_strConectionString As String = String.Empty
        Dim objConfiguracionGeneral As SCGDataAccess.ConfiguracionesGeneralesAddon
        Dim editNumCont As SAPbouiCOM.EditText

        Configuracion.CrearCadenaDeconexion(CompanySBO.Server, CompanySBO.CompanyDB, m_strConectionString)

        If m_cn_Coneccion.State = ConnectionState.Open Then
            m_cn_Coneccion.Close()
        End If

        m_cn_Coneccion.ConnectionString = m_strConectionString
        objConfiguracionGeneral = New SCGDataAccess.ConfiguracionesGeneralesAddon(m_cn_Coneccion)

        oForm = ApplicationSBO.Forms.Item(FormUID)
        editNumCont = DirectCast(oForm.Items.Item("txtNumCt").Specific, EditText)
        strParametrosDocEntry = editNumCont.Value ' oForm.DataSources.DBDataSources.Item("@SCGD_FACTURAINTERNA").GetValue("DocEntry", 0)

        strParametros = String.Format("{0}", strParametrosDocEntry)
        strParametros = strParametros.Replace(" ", "°")

        strDireccionReporte = String.Format("{0}{1}.rpt", objConfiguracionGeneral.DireccionReportes, My.Resources.Resource.RptBalanceCV)

        strDireccionReporte = strDireccionReporte.Replace(" ", "°")
        strPathExe = My.Application.Info.DirectoryPath & "\SCG Visualizador de Reportes.exe "

        strPathExe &= My.Resources.Resource.TituloBalances.Replace(" ", "°") & " " & strDireccionReporte & " " & CatchingEvents.DBUser & "," & CatchingEvents.DBPassword & "," & CompanySBO.Server & "," & CompanySBO.CompanyDB & " " & strParametros
        Shell(strPathExe, AppWinStyle.MaximizedFocus)

    End Sub


#End Region 'Metodos

End Class
