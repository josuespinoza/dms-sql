Imports SAPbouiCOM
Imports System.Globalization
Imports System.Collections.Generic
Imports SCG.SBOFramework

Public Module ControladorDisponibilidadEmpleados
    Private n As NumberFormatInfo

    ''' <summary>
    ''' Enumeración utilizada para definir cual tipo de vista se debe usar si semanal o por meses
    ''' </summary>
    ''' <remarks></remarks>
    Enum TipoVista
        Semanal = 0
        Mensual = 1
    End Enum

    ''' <summary>
    ''' Constructor del módulo
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        Try
            n = DIHelper.GetNumberFormatInfo(DMS_Connector.Company.CompanySBO)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de los eventos ItemEvent de SAP para el formulario de disponibilidad de empleados
    ''' </summary>
    ''' <param name="FormUID">ID del formulario</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Variable booleana de SAP para definir si se debe continuar con el proceso o no</param>
    ''' <remarks></remarks>
    Public Sub ItemEvent(ByVal FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If pVal.FormTypeEx = "SCGD_ODE" Then
                'Obtiene la instancia del formulario desde la cual se generó el evento
                oFormulario = ObtenerFormulario(FormUID)
                If oFormulario IsNot Nothing Then
                    If pVal.BeforeAction Then
                        'Sin implementar
                    Else
                        Select Case pVal.EventType
                            Case BoEventTypes.et_ITEM_PRESSED
                                ItemPressed(oFormulario, pVal, BubbleEvent)
                            Case BoEventTypes.et_COMBO_SELECT
                                ComboSelect(oFormulario, pVal, BubbleEvent)
                        End Select
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de los eventos de tipo ComboSelect
    ''' </summary>
    ''' <param name="oFormulario">Formulario desde el cual se ejecutó el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Boolean que indica si se debe continuar con el proceso no</param>
    ''' <remarks></remarks>
    Private Sub ComboSelect(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.ItemUID
                Case "cboSucu"
                    CargarDisponibilidad(oFormulario)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores iniciales al abrir el formulario
    ''' </summary>
    ''' <param name="oFormulario">Formulario recien abierto</param>
    ''' <remarks></remarks>
    Public Sub CargarValoresPredeterminados(ByRef oFormulario As SAPbouiCOM.Form)
        Try
            oFormulario.DataSources.UserDataSources.Item("Semana").ValueEx = "Y"
            oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx = DateTime.Now.ToString("yyyyMMdd")
            CargarSucursales(oFormulario)
            FijarColumnasMatrices(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Define cuales son las columnas fijas, es decir aquellas que no se deben mover al utilizar
    ''' la barra de desplazamiento de la matriz
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <remarks></remarks>
    Private Sub FijarColumnasMatrices(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oMatrix = oFormulario.Items.Item("mtxSeman").Specific
            oMatrix.CommonSetting.FixedColumnsCount = 2
            oMatrix = oFormulario.Items.Item("mtxMeses").Specific
            oMatrix.CommonSetting.FixedColumnsCount = 2
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el listado de sucursales en el ComboBox sucursal
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <remarks></remarks>
    Private Sub CargarSucursales(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim strSucursal As String = String.Empty
        Dim strQuery As String = "SELECT T0.""Code"", T0.""Name"" FROM ""@SCGD_SUCURSALES"" T0  ORDER BY T0.""Name"""
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboSucu").Specific

            'Agrega los valores válidos al ComboBox
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(strQuery)

            While Not oRecordset.EoF
                oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                oRecordset.MoveNext()
            End While

            'Selecciona la sucursal del usuario conectado
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count > 0 Then
                strSucursal = ObtenerSucursalUsuario()
                If Not String.IsNullOrEmpty(strSucursal) Then
                    For Each oValidValue As SAPbouiCOM.ValidValue In oComboBox.ValidValues
                        If oValidValue.Value = strSucursal Then
                            oComboBox.Select(strSucursal, SAPbouiCOM.BoSearchKey.psk_ByValue)
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve la sucursal del usuario conectado a SAP
    ''' </summary>
    ''' <returns>Código de la sucursal del usuario conectado</returns>
    ''' <remarks></remarks>
    Private Function ObtenerSucursalUsuario() As String
        Dim oUser As SAPbobsCOM.Users
        Dim strSucursal As String = String.Empty
        Dim strInternalKey As String = String.Empty
        Try
            strInternalKey = DMS_Connector.Company.CompanySBO.UserSignature
            oUser = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUsers)
            oUser.GetByKey(strInternalKey)
            strSucursal = oUser.Branch
            Return strSucursal
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strSucursal
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el formulario desde el cual se ejecutó el evento
    ''' </summary>
    ''' <param name="FormUID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObtenerFormulario(ByVal FormUID As String) As SAPbouiCOM.Form
        Try
            Return DMS_Connector.Company.ApplicationSBO.Forms.Item(FormUID)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Carga la disponibilidad de los empleados en la matriz
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde la cual se ejecutó el evento</param>
    ''' <remarks></remarks>
    Private Sub CargarDisponibilidad(ByRef oFormulario As SAPbouiCOM.Form)
        Dim strOcupacionMensual As String = String.Empty
        Try
            strOcupacionMensual = oFormulario.DataSources.UserDataSources.Item("Mes").ValueEx
            'Ejecuta un método de acuerdo al tipo de disponibilidad que se desea cargar ya sea mensual o por semanas
            If Not String.IsNullOrEmpty(strOcupacionMensual) AndAlso strOcupacionMensual = "Y" Then
                DisponibilidadMensual(oFormulario)
            Else
                DisponibilidadSemanal(oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la disponibilidad para la vista en formato mensual
    ''' </summary>
    ''' <param name="oFormulario">Instancia de formulario desde la cual se ejecutó el evento</param>
    ''' <remarks></remarks>
    Private Sub DisponibilidadMensual(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strQuery As String = String.Empty
        Dim strFecha As String = String.Empty
        Dim dtFecha As DateTime
        Dim strSucursal As String = String.Empty

        Try
            oFormulario.Freeze(True)
            'Paso 1 Consultar la ocupación
            strQuery = DMS_Connector.Queries.GetStrQueryFormat("strQueryOcupacionMensual")
            strFecha = oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx
            dtFecha = DateTime.ParseExact(strFecha, "yyyyMMdd", Nothing)
            dtFecha = New DateTime(dtFecha.Year, 1, 1)
            strSucursal = oFormulario.DataSources.UserDataSources.Item("Sucursal").ValueEx
            AgregarParametrosQueryMeses(strQuery, dtFecha, strSucursal)
            oDataTable = oFormulario.DataSources.DataTables.Item("DTMeses")
            oDataTable.Clear()
            oDataTable.ExecuteQuery(strQuery)
            'Paso 2 Restarle a la disponibilidad mensual la ocupación, el resultado se guarda en la misma tabla reemplazando las celdas
            CalcularDisponibilidad(oFormulario, oDataTable, TipoVista.Mensual)
            oMatrix = oFormulario.Items.Item("mtxMeses").Specific
            oMatrix.LoadFromDataSource()
            'Paso 3 Dar formato a las celdas (Formato de horas, color de la celda, otros)
            ActualizarFormatoTabla(oMatrix, oDataTable, TipoVista.Mensual)
            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de realizar la resta entre las horas ocupadas y las horas disponibles
    ''' el resultado se guarda en la misma tabla que fue utilizada para consultar la ocupación
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <param name="oDataTable"></param>
    ''' <param name="oTipoVista"></param>
    ''' <remarks></remarks>
    Private Sub CalcularDisponibilidad(ByRef oFormulario As SAPbouiCOM.Form, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal oTipoVista As TipoVista)
        Dim intMinutosDisponiblesPorDia As Integer = 0
        Dim strValor As String = String.Empty
        Dim intValor As Integer = 0
        Dim intTotalMinutos As Integer = 0
        Dim oListaDiasLaboralesPorMes As New List(Of Integer)

        Try
            'Consulta el horario de la sucursal y verifica la cantidad de minutos que se pueden trabajar por día
            'restando las horas de almuerzo
            intMinutosDisponiblesPorDia = ObtenerMinutosDisponiblesPorDia(oFormulario)
            ObtenerDiasLaboralesMensuales(oFormulario, oListaDiasLaboralesPorMes)

            'Recorre toda la tabla con la ocupación y reemplaza los valores con la resta (Disponibilidad final)
            For i As Integer = 0 To oDataTable.Rows.Count - 1
                For j As Integer = 2 To oDataTable.Columns.Count - 1
                    strValor = oDataTable.GetValue(j, i)
                    If Double.TryParse(strValor, System.Globalization.NumberStyles.Any, n, intValor) Then
                        intTotalMinutos = 0
                        If oTipoVista = TipoVista.Semanal Then
                            'Resta los minutos disponibles por día menos la ocupación y se guarda en la misma tabla
                            If intValor = 0 Then
                                oDataTable.SetValue(j, i, intMinutosDisponiblesPorDia.ToString())
                            Else
                                intTotalMinutos = intMinutosDisponiblesPorDia - intValor
                                oDataTable.SetValue(j, i, intTotalMinutos.ToString())
                            End If
                        Else
                            'Resta los minutos disponibles por día menos la ocupación y se guarda en la misma tabla
                            If intValor = 0 Then
                                oDataTable.SetValue(j, i, (oListaDiasLaboralesPorMes(j - 2) * intMinutosDisponiblesPorDia).ToString())
                            Else
                                intTotalMinutos = (oListaDiasLaboralesPorMes(j - 2) * intMinutosDisponiblesPorDia) - intValor
                                oDataTable.SetValue(j, i, intTotalMinutos.ToString())
                            End If
                        End If

                        'La cantidad de minutos no puede ser negativa, lo mínimo es cero
                        If intTotalMinutos < 0 Then
                            oDataTable.SetValue(j, i, "0")
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la cantidad de días laborales de cada mes (Restando sábados y domingos) y los guarda en un listado
    ''' no contempla los días feriados ni similares
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde la cual se ejecutó el evento</param>
    ''' <param name="oLista">Lista donde se va a guardar la cantidad de días laborales por cada mes</param>
    ''' <remarks></remarks>
    Private Sub ObtenerDiasLaboralesMensuales(ByRef oFormulario As SAPbouiCOM.Form, ByRef oLista As List(Of Integer))
        Dim strFecha As String = String.Empty
        Dim dtFecha As DateTime
        Dim dtMes As DateTime
        Dim dtDia As DateTime
        Dim Year As Integer
        Dim intDiasDelMes As Integer
        Dim intCantidadSabadosDomingos As Integer
        Try
            oLista.Clear()
            strFecha = oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx
            dtFecha = DateTime.ParseExact(strFecha, "yyyyMMdd", Nothing)
            Year = dtFecha.Year
            For i As Integer = 1 To 12
                intCantidadSabadosDomingos = 0
                intDiasDelMes = DateTime.DaysInMonth(Year, i)
                For j As Integer = 1 To intDiasDelMes
                    dtDia = New DateTime(Year, i, j)
                    If dtDia.DayOfWeek = DayOfWeek.Saturday Or dtDia.DayOfWeek = DayOfWeek.Sunday Then
                        intCantidadSabadosDomingos += 1
                    End If
                Next
                oLista.Add(intDiasDelMes - intCantidadSabadosDomingos)
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Convierte el formato de la tabla de minutos al formato solicitado por el cliente
    ''' </summary>
    ''' <param name="oMatrix">Objeto matriz donde se van a mostrar los datos</param>
    ''' <param name="oDataTable">Tabla con la información de la disponibilidad, los valores deben estar en minutos, no se permite espacios vacios ni nulos
    ''' en caso de tener valores nulos o vacíos debe asignarse un 0 a la celda</param>
    ''' <param name="oTipoVista">Tipo de vista ya sea semanal o mensual</param>
    ''' <remarks></remarks>
    Private Sub ActualizarFormatoTabla(ByRef oMatrix As SAPbouiCOM.Matrix, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal oTipoVista As TipoVista)
        Dim intValor As Integer
        Dim strValor As String = String.Empty

        Try
            'Recorre todas las filas y columnas de la tabla, les asigna el formato "2 hrs", redondea a dos dígitos y les cambia el color de acuerdo a la disponibilidad
            For i As Integer = 0 To oDataTable.Rows.Count - 1
                For j As Integer = 2 To oDataTable.Columns.Count - 1
                    strValor = oDataTable.GetValue(j, i)
                    If Integer.TryParse(strValor, intValor) Then
                        If intValor = 0 Then
                            oDataTable.SetValue(j, i, "0 hrs")
                            oMatrix.CommonSetting.SetCellBackColor(i + 1, j, 12632256)
                        Else
                            strValor = String.Format("{0} hrs", Math.Round(intValor / 60, 2))
                            oDataTable.SetValue(j, i, strValor)
                            If oTipoVista = TipoVista.Semanal Then
                                If j = 7 Or j = 8 Or j = 14 Or j = 15 Then
                                    oMatrix.CommonSetting.SetCellBackColor(i + 1, j, 16777152)
                                Else
                                    oMatrix.CommonSetting.SetCellBackColor(i + 1, j, 12648447)
                                End If
                            Else
                                oMatrix.CommonSetting.SetCellBackColor(i + 1, j, 12648447)
                            End If

                        End If
                    End If
                Next
            Next
            oMatrix.LoadFromDataSource()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Calcula en base al horario de la sucursal y la hora de almuerzo la cantidad de minutos laborales por cada día
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde la cual se ejecutó el evento</param>
    ''' <returns>Número entero con la cantidad de minutos laborales por día</returns>
    ''' <remarks></remarks>
    Private Function ObtenerMinutosDisponiblesPorDia(ByRef oFormulario As SAPbouiCOM.Form) As Integer
        Dim intMinutosDisponiblesDiarios As Integer = 0
        Dim dtHoraInicioLunesViernes As DateTime
        Dim dtHoraCierreLunesViernes As DateTime
        Dim intMinutosAlmuerzo As Integer = 0
        Dim strSucursal As String = String.Empty
        Dim oTimeSpan As TimeSpan
        Try
            strSucursal = oFormulario.DataSources.UserDataSources.Item("Sucursal").ValueEx
            If Not String.IsNullOrEmpty(strSucursal) Then
                ObtenerHorarioSucursal(strSucursal, dtHoraInicioLunesViernes, dtHoraCierreLunesViernes, intMinutosAlmuerzo)
                oTimeSpan = dtHoraCierreLunesViernes - dtHoraInicioLunesViernes
                intMinutosDisponiblesDiarios = oTimeSpan.TotalMinutes - intMinutosAlmuerzo
            End If
            Return intMinutosDisponiblesDiarios
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Consulta el horario de la sucursal y lo guarda en objetos de tipo DateTime para su uso por otros métodos
    ''' </summary>
    ''' <param name="p_strIDSucursal">ID de la sucursal</param>
    ''' <param name="dtInicioLunesViernes">Objeto DateTime donde se va a guardar la hora de inicio de la sucursal</param>
    ''' <param name="dtCierreLunesViernes">Objeto DateTime donde se va a guardar la hora de cierre de la sucursal</param>
    ''' <param name="intMinutosAlmuerzo">Variable donde se va a guardar la cantidad de minutos de almuerzo por día</param>
    ''' <remarks></remarks>
    Private Sub ObtenerHorarioSucursal(ByVal p_strIDSucursal As String, ByRef dtInicioLunesViernes As DateTime, ByRef dtCierreLunesViernes As DateTime, ByRef intMinutosAlmuerzo As Integer)
        Dim strQuery As String = "SELECT T0.""U_HoraInicio"", T0.""U_HoraFin"", T0.""U_HorAlI"", T0.""U_HoraAlF"" FROM ""@SCGD_CONF_SUCURSAL"" T0 WHERE T0.""U_Sucurs"" = '{0}'"
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim strHoraInicioLunesViernes As String = String.Empty
        Dim strHoraCierreLunesViernes As String = String.Empty
        Dim strHoraInicioAlmuerzo As String = String.Empty
        Dim strHoraFinAlmuerzo As String = String.Empty
        Dim dtHoraInicioAlmuerzo As DateTime
        Dim dtHoraFinAlmuerzo As DateTime
        Dim oTimeSpan As TimeSpan

        Try
            strQuery = String.Format(strQuery, p_strIDSucursal)
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(strQuery)

            While Not oRecordset.EoF
                strHoraInicioLunesViernes = oRecordset.Fields.Item(0).Value.ToString()
                strHoraCierreLunesViernes = oRecordset.Fields.Item(1).Value.ToString()
                strHoraInicioAlmuerzo = oRecordset.Fields.Item(2).Value.ToString()
                strHoraFinAlmuerzo = oRecordset.Fields.Item(3).Value.ToString()
                oRecordset.MoveNext()
            End While

            'Completa el formato de la hora para asegurarnos que esté en el formato correcto y evitar excepciones al momento de convertirlo
            'a un objeto DateTime
            CompletarFormatoHora(strHoraInicioLunesViernes)
            CompletarFormatoHora(strHoraCierreLunesViernes)
            CompletarFormatoHora(strHoraInicioAlmuerzo)
            CompletarFormatoHora(strHoraFinAlmuerzo)

            If String.IsNullOrEmpty(strHoraInicioLunesViernes) Or String.IsNullOrEmpty(strHoraCierreLunesViernes) Then
                'Agregar mensaje de error el horario de la sucursal no esta configurado correctamente
            Else
                'Lunes a Viernes
                dtInicioLunesViernes = DateTime.ParseExact(strHoraInicioLunesViernes, "HHmm", Nothing)
                dtCierreLunesViernes = DateTime.ParseExact(strHoraCierreLunesViernes, "HHmm", Nothing)
            End If

            If Not String.IsNullOrEmpty(strHoraInicioAlmuerzo) AndAlso Not String.IsNullOrEmpty(strHoraFinAlmuerzo) Then
                'Horario de almuerzo
                dtHoraInicioAlmuerzo = DateTime.ParseExact(strHoraInicioAlmuerzo, "HHmm", Nothing)
                dtHoraFinAlmuerzo = DateTime.ParseExact(strHoraFinAlmuerzo, "HHmm", Nothing)
                oTimeSpan = dtHoraFinAlmuerzo - dtHoraInicioAlmuerzo
                intMinutosAlmuerzo = oTimeSpan.TotalMinutes
            Else
                intMinutosAlmuerzo = 0
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Completa el formato de la hora de tal forma que contenga 4 números y el 0 adelante. Ejemplo: 1200, 0900, 0700, 1030
    ''' </summary>
    ''' <param name="p_strHora"></param>
    ''' <remarks></remarks>
    Private Sub CompletarFormatoHora(ByRef p_strHora As String)
        Try
            If Not String.IsNullOrEmpty(p_strHora) Then
                Select Case p_strHora.Length
                    Case 0
                        'Hora inválida
                        p_strHora = String.Empty
                    Case 1
                        'Hora inválida
                        p_strHora = String.Empty
                    Case 2
                        'Hora inválida
                        p_strHora = String.Empty
                    Case 3
                        p_strHora = String.Format("0{0}", p_strHora)
                    Case 4
                        'Hora en el formato correcto no se hace nada
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Completa los parámetros requeridos para el query de ocupación por meses
    ''' </summary>
    ''' <param name="strQuery">Query de ocupación por meses en formato texto</param>
    ''' <param name="dtFecha">Objeto DateTime con la fecha por consultar</param>
    ''' <param name="strSucursal">ID o código interno de la sucursal</param>
    ''' <remarks></remarks>
    Private Sub AgregarParametrosQueryMeses(ByRef strQuery As String, ByRef dtFecha As DateTime, ByVal strSucursal As String)
        Dim strEnero As String = String.Empty
        Dim strFebrero As String = String.Empty
        Dim strMarzo As String = String.Empty
        Dim strAbril As String = String.Empty
        Dim strMayo As String = String.Empty
        Dim strJunio As String = String.Empty
        Dim strJulio As String = String.Empty
        Dim strAgosto As String = String.Empty
        Dim strSeptiembre As String = String.Empty
        Dim strOctubre As String = String.Empty
        Dim strNoviembre As String = String.Empty
        Dim strDiciembre As String = String.Empty
        Dim strFinEnero As String = String.Empty
        Dim strFinFebrero As String = String.Empty
        Dim strFinMarzo As String = String.Empty
        Dim strFinAbril As String = String.Empty
        Dim strFinMayo As String = String.Empty
        Dim strFinJunio As String = String.Empty
        Dim strFinJulio As String = String.Empty
        Dim strFinAgosto As String = String.Empty
        Dim strFinSeptiembre As String = String.Empty
        Dim strFinOctubre As String = String.Empty
        Dim strFinNoviembre As String = String.Empty
        Dim strFinDiciembre As String = String.Empty
        Dim Year As Integer = 0

        Try
            Year = dtFecha.Year
            strEnero = String.Format("{0}{1}{2}", Year, "01", "01")
            strFinEnero = String.Format("{0}{1}{2}", Year, "01", DateTime.DaysInMonth(Year, 1))

            strFebrero = String.Format("{0}{1}{2}", Year, "02", "01")
            strFinFebrero = String.Format("{0}{1}{2}", Year, "02", DateTime.DaysInMonth(Year, 2))

            strMarzo = String.Format("{0}{1}{2}", Year, "03", "01")
            strFinMarzo = String.Format("{0}{1}{2}", Year, "03", DateTime.DaysInMonth(Year, 3))

            strAbril = String.Format("{0}{1}{2}", Year, "04", "01")
            strFinAbril = String.Format("{0}{1}{2}", Year, "04", DateTime.DaysInMonth(Year, 4))

            strMayo = String.Format("{0}{1}{2}", Year, "05", "01")
            strFinMayo = String.Format("{0}{1}{2}", Year, "05", DateTime.DaysInMonth(Year, 5))

            strJunio = String.Format("{0}{1}{2}", Year, "06", "01")
            strFinJunio = String.Format("{0}{1}{2}", Year, "06", DateTime.DaysInMonth(Year, 6))

            strJulio = String.Format("{0}{1}{2}", Year, "07", "01")
            strFinJulio = String.Format("{0}{1}{2}", Year, "07", DateTime.DaysInMonth(Year, 7))

            strAgosto = String.Format("{0}{1}{2}", Year, "08", "01")
            strFinAgosto = String.Format("{0}{1}{2}", Year, "08", DateTime.DaysInMonth(Year, 8))

            strSeptiembre = String.Format("{0}{1}{2}", Year, "09", "01")
            strFinSeptiembre = String.Format("{0}{1}{2}", Year, "09", DateTime.DaysInMonth(Year, 9))

            strOctubre = String.Format("{0}{1}{2}", Year, "10", "01")
            strFinOctubre = String.Format("{0}{1}{2}", Year, "10", DateTime.DaysInMonth(Year, 10))

            strNoviembre = String.Format("{0}{1}{2}", Year, "11", "01")
            strFinNoviembre = String.Format("{0}{1}{2}", Year, "11", DateTime.DaysInMonth(Year, 11))

            strDiciembre = String.Format("{0}{1}{2}", Year, "12", "01")
            strFinDiciembre = String.Format("{0}{1}{2}", Year, "12", DateTime.DaysInMonth(Year, 12))

            'Formateamos el query con las fechas de los meses
            strQuery = String.Format(strQuery, strEnero, strFinEnero, strFebrero, strFinFebrero, strMarzo, strFinMarzo, strAbril, strFinAbril, strMayo, strFinMayo, strJunio, strFinJunio, strJulio, strFinJulio, strAgosto, strFinAgosto, strSeptiembre, strFinSeptiembre, strOctubre, strFinOctubre, strNoviembre, strFinNoviembre, strDiciembre, strFinDiciembre, strSucursal)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la disponibilidad para la vista en formato semanal
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde la cual se ejecutó el evento</param>
    ''' <remarks></remarks>
    Private Sub DisponibilidadSemanal(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strQuery As String = String.Empty
        Dim strFecha As String = String.Empty
        Dim dtFecha As DateTime
        Dim strSucursal As String = String.Empty
        Dim strLunesS1 As String = String.Empty
        Dim strMartesS1 As String = String.Empty
        Dim strMiercolesS1 As String = String.Empty
        Dim strJuevesS1 As String = String.Empty
        Dim strViernesS1 As String = String.Empty
        Dim strSabadoS1 As String = String.Empty
        Dim strDomingoS1 As String = String.Empty
        Dim strLunesS2 As String = String.Empty
        Dim strMartesS2 As String = String.Empty
        Dim strMiercolesS2 As String = String.Empty
        Dim strJuevesS2 As String = String.Empty
        Dim strViernesS2 As String = String.Empty
        Dim strSabadoS2 As String = String.Empty
        Dim strDomingoS2 As String = String.Empty

        Try
            oFormulario.Freeze(True)
            'Paso 1 Consultar la ocupación
            strQuery = DMS_Connector.Queries.GetStrQueryFormat("strQueryOcupacionSemanal")
            oMatrix = oFormulario.Items.Item("mtxSeman").Specific
            strFecha = oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx
            dtFecha = DateTime.ParseExact(strFecha, "yyyyMMdd", Nothing)
            ObtenerFechaDiasSemana(oMatrix, dtFecha, strLunesS1, strMartesS1, strMiercolesS1, strJuevesS1, strViernesS1, strSabadoS1, strDomingoS1, strLunesS2, strMartesS2, strMiercolesS2, strJuevesS2, strViernesS2, strSabadoS2, strDomingoS2)
            strSucursal = oFormulario.DataSources.UserDataSources.Item("Sucursal").ValueEx
            strQuery = String.Format(strQuery, strLunesS1, strMartesS1, strMiercolesS1, strJuevesS1, strViernesS1, strSabadoS1, strDomingoS1, strSucursal, strLunesS2, strMartesS2, strMiercolesS2, strJuevesS2, strViernesS2, strSabadoS2, strDomingoS2)
            oDataTable = oFormulario.DataSources.DataTables.Item("DTSemana")
            oDataTable.Clear()
            oDataTable.ExecuteQuery(strQuery)
            'Paso 2 Restarle a la disponibilidad por día la ocupación, el resultado se guarda en la misma tabla reemplazando las celdas
            CalcularDisponibilidad(oFormulario, oDataTable, TipoVista.Semanal)
            oMatrix.LoadFromDataSource()
            'Paso 3 Dar formato a las celdas (Formato de horas, color de la celda, otros)
            ActualizarFormatoTabla(oMatrix, oDataTable, TipoVista.Semanal)
            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Con base  a la fecha seleccionada calcula las fechas de los siguientes 15 días y las devuelve para ser utilizadas en el query de ocupación semanal
    ''' además, modifica el título de la columna para que tenga la fecha del día que representa la columna
    ''' </summary>
    ''' <param name="oMatrix">Objeto matriz donde se muestran los datos</param>
    ''' <param name="dtFecha">Objeto DateTime con la fecha seleccionada</param>
    ''' <param name="strLunes">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strMartes">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strMiercoles">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strJueves">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strViernes">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strSabado">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strDomingo">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strLunesS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strMartesS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strMiercolesS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strJuevesS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strViernesS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strSabadoS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <param name="strDomingoS2">String donde se va a guardar la fecha en formato yyyyMMdd para su uso en queries</param>
    ''' <remarks></remarks>
    Private Sub ObtenerFechaDiasSemana(ByRef oMatrix As SAPbouiCOM.Matrix, ByRef dtFecha As DateTime, ByRef strLunes As String, ByRef strMartes As String, ByRef strMiercoles As String, ByRef strJueves As String, ByRef strViernes As String, ByRef strSabado As String, ByRef strDomingo As String, ByRef strLunesS2 As String, ByRef strMartesS2 As String, ByRef strMiercolesS2 As String, ByRef strJuevesS2 As String, ByRef strViernesS2 As String, ByRef strSabadoS2 As String, ByRef strDomingoS2 As String)
        Try
            If dtFecha.DayOfWeek = DayOfWeek.Sunday Then
                dtFecha = dtFecha.AddDays(-6)
            Else
                dtFecha = dtFecha.AddDays(DayOfWeek.Monday - dtFecha.DayOfWeek)
            End If

            'Primer semana
            strLunes = dtFecha.ToString("yyyyMMdd")
            oMatrix.Columns().Item("LunS1").TitleObject.Caption = GenerarTituloColumna(dtFecha)
            strMartes = dtFecha.AddDays(1).ToString("yyyyMMdd")
            oMatrix.Columns().Item("MarS1").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(1))
            strMiercoles = dtFecha.AddDays(2).ToString("yyyyMMdd")
            oMatrix.Columns().Item("MieS1").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(2))
            strJueves = dtFecha.AddDays(3).ToString("yyyyMMdd")
            oMatrix.Columns().Item("JueS1").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(3))
            strViernes = dtFecha.AddDays(4).ToString("yyyyMMdd")
            oMatrix.Columns().Item("VieS1").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(4))
            strSabado = dtFecha.AddDays(5).ToString("yyyyMMdd")
            oMatrix.Columns().Item("SabS1").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(5))
            strDomingo = dtFecha.AddDays(6).ToString("yyyyMMdd")
            oMatrix.Columns().Item("DomS1").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(6))
            'Segunda semana
            strLunesS2 = dtFecha.AddDays(7).ToString("yyyyMMdd")
            oMatrix.Columns().Item("LunS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(7))
            strMartesS2 = dtFecha.AddDays(8).ToString("yyyyMMdd")
            oMatrix.Columns().Item("MarS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(8))
            strMiercolesS2 = dtFecha.AddDays(9).ToString("yyyyMMdd")
            oMatrix.Columns().Item("MieS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(9))
            strJuevesS2 = dtFecha.AddDays(10).ToString("yyyyMMdd")
            oMatrix.Columns().Item("JueS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(10))
            strViernesS2 = dtFecha.AddDays(11).ToString("yyyyMMdd")
            oMatrix.Columns().Item("VieS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(11))
            strSabadoS2 = dtFecha.AddDays(12).ToString("yyyyMMdd")
            oMatrix.Columns().Item("SabS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(12))
            strDomingoS2 = dtFecha.AddDays(13).ToString("yyyyMMdd")
            oMatrix.Columns().Item("DomS2").TitleObject.Caption = GenerarTituloColumna(dtFecha.AddDays(13))
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Genera el título que se le va a asignar la columna en formato corto. Ejemplo: Lun 03 Dic.
    ''' </summary>
    ''' <param name="dtFecha">Objeto DateTime con la fecha que se desea convertir en el texto del título</param>
    ''' <returns>String con el formato corto de la fecha para ser usado en el título</returns>
    ''' <remarks></remarks>
    Private Function GenerarTituloColumna(ByVal dtFecha As DateTime) As String
        Dim strDescripcionDia As String = String.Empty
        Dim strDia As String = String.Empty
        Dim strDescripcionMes As String = String.Empty
        Dim strTituloColumna As String = String.Empty
        Dim oCultureInfo As CultureInfo

        Try
            'Obtiene la información cultural para generar el título en el idioma correcto
            oCultureInfo = GetCulture()
            strDescripcionDia = dtFecha.ToString("ddd", oCultureInfo)
            strDia = dtFecha.ToString("dd", oCultureInfo)
            strDescripcionMes = dtFecha.ToString("MMM", oCultureInfo)
            strTituloColumna = String.Format("{0} {1} {2}", strDescripcionDia, strDia, strDescripcionMes)
            Return strTituloColumna
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return strTituloColumna
        End Try
    End Function

    ''' <summary>
    ''' Obtiene la información cultural de SAP (Idioma)
    ''' </summary>
    ''' <returns>Objeto CultureInfo con la información del idioma actual de SAP Business One</returns>
    ''' <remarks></remarks>
    Public Function GetCulture() As CultureInfo
        Dim oCultureInfo As CultureInfo
        Try
            Select Case DMS_Connector.Company.ApplicationSBO.Language
                Case BoLanguages.ln_English, BoLanguages.ln_English_Cy, BoLanguages.ln_English_Gb, BoLanguages.ln_English_Sg
                    oCultureInfo = New CultureInfo("en-Us")
                Case BoLanguages.ln_Spanish, BoLanguages.ln_Spanish_Ar, BoLanguages.ln_Spanish_La, BoLanguages.ln_Spanish_Pa
                    oCultureInfo = New CultureInfo("es-Cr")
                Case Else
                    oCultureInfo = New CultureInfo("en-Us")
            End Select
            Return oCultureInfo
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return New CultureInfo("en-Us")
        End Try
    End Function

    ''' <summary>
    ''' Manejador de los eventos ItemPressed
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Booleano que indica si se debe continuar procesando el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ItemPressed(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.ItemUID
                Case "btnHoy"
                    ManejadorBotonHoy(oFormulario)
                    CargarDisponibilidad(oFormulario)
                Case "btnAnter"
                    ManejadorBotonAnterior(oFormulario)
                    CargarDisponibilidad(oFormulario)
                Case "btnSigui"
                    ManejadorBotonSiguiente(oFormulario)
                    CargarDisponibilidad(oFormulario)
                Case "chkMes", "chkSema", "chkSemaL"
                    CambiarTipoVista(oFormulario, pVal)
                    CargarDisponibilidad(oFormulario)
                Case "btnRefre"
                    CargarDisponibilidad(oFormulario)
                Case "txtFecha"
                    ManejadorEditTextFecha(oFormulario)
                    CargarDisponibilidad(oFormulario)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de los eventos sobre el Picker o selector de fecha del cuadro de texto fecha
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEditTextFecha(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oEditText As SAPbouiCOM.EditText
        Dim strFecha As String = String.Empty
        Try
            oEditText = oFormulario.Items.Item("txtFecha").Specific
            strFecha = oEditText.Value
            oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx = strFecha
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos del botón hoy
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorBotonHoy(ByRef oFormulario As SAPbouiCOM.Form)
        Try
            oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx = DateTime.Now.ToString("yyyyMMdd")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos del botón anterior
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorBotonAnterior(ByRef oFormulario As SAPbouiCOM.Form)
        Dim strFecha As String = String.Empty
        Dim dtFecha As DateTime
        Dim strOcupacionMensual As String = String.Empty
        Try
            strFecha = oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx
            dtFecha = DateTime.ParseExact(strFecha, "yyyyMMdd", Nothing)

            strOcupacionMensual = oFormulario.DataSources.UserDataSources.Item("Mes").ValueEx

            If Not String.IsNullOrEmpty(strOcupacionMensual) AndAlso strOcupacionMensual = "Y" Then
                dtFecha = dtFecha.AddYears(-1)
            Else
                dtFecha = dtFecha.AddDays(DayOfWeek.Monday - dtFecha.DayOfWeek)
                dtFecha = dtFecha.AddDays(-7)
            End If

            oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx = dtFecha.ToString("yyyyMMdd")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos del botón siguiente
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorBotonSiguiente(ByRef oFormulario As SAPbouiCOM.Form)
        Dim strFecha As String = String.Empty
        Dim dtFecha As DateTime
        Dim strOcupacionMensual As String = String.Empty
        Try
            strFecha = oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx
            dtFecha = DateTime.ParseExact(strFecha, "yyyyMMdd", Nothing)

            strOcupacionMensual = oFormulario.DataSources.UserDataSources.Item("Mes").ValueEx

            If Not String.IsNullOrEmpty(strOcupacionMensual) AndAlso strOcupacionMensual = "Y" Then
                dtFecha = dtFecha.AddYears(1)
            Else
                dtFecha = dtFecha.AddDays(DayOfWeek.Monday - dtFecha.DayOfWeek)
                dtFecha = dtFecha.AddDays(7)
            End If

            oFormulario.DataSources.UserDataSources.Item("Fecha").ValueEx = dtFecha.ToString("yyyyMMdd")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado del manejo de los checkbox para el tipo de vista
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <param name="pVal"></param>
    ''' <remarks></remarks>
    Private Sub CambiarTipoVista(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent)
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oFormulario.Freeze(True)
            Select Case pVal.ItemUID
                Case "chkMes"
                    oFormulario.DataSources.UserDataSources.Item("Semana").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("SemanaL").ValueEx = "N"
                    oMatrix = oFormulario.Items.Item("mtxSeman").Specific
                    oMatrix.Item.Visible = False
                    oMatrix = oFormulario.Items.Item("mtxMeses").Specific
                    oMatrix.Item.Visible = True
                Case "chkSema"
                    oFormulario.DataSources.UserDataSources.Item("Mes").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("SemanaL").ValueEx = "N"
                    oMatrix = oFormulario.Items.Item("mtxSeman").Specific
                    oMatrix.Columns.Item("SabS1").Visible = True
                    oMatrix.Columns.Item("DomS1").Visible = True
                    oMatrix.Columns.Item("SabS2").Visible = True
                    oMatrix.Columns.Item("DomS2").Visible = True
                    oMatrix.Item.Visible = True
                    oMatrix.AutoResizeColumns()
                    oMatrix = oFormulario.Items.Item("mtxMeses").Specific
                    oMatrix.Item.Visible = False
                Case "chkSemaL"
                    oFormulario.DataSources.UserDataSources.Item("Mes").ValueEx = "N"
                    oFormulario.DataSources.UserDataSources.Item("Semana").ValueEx = "N"
                    oMatrix = oFormulario.Items.Item("mtxSeman").Specific
                    oMatrix.Columns.Item("SabS1").Visible = False
                    oMatrix.Columns.Item("DomS1").Visible = False
                    oMatrix.Columns.Item("SabS2").Visible = False
                    oMatrix.Columns.Item("DomS2").Visible = False
                    oMatrix.Item.Visible = True
                    oMatrix.AutoResizeColumns()
                    oMatrix = oFormulario.Items.Item("mtxMeses").Specific
                    oMatrix.Item.Visible = False
            End Select
            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Module
