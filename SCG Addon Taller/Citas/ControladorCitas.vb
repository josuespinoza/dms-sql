Imports SAPbouiCOM
Imports System.Globalization
Imports SCG.SBOFramework
Imports System.Linq
Imports System.Timers
Imports System.Runtime.InteropServices
Imports System.Collections.Generic

''' <summary>
''' Módulo encargado de toda la lógica de negocios del formulario de citas
''' </summary>
''' <remarks></remarks>
Public Module ControladorCitas

    Private n As NumberFormatInfo
    Private UsaConfiguracionEstiloModelo As String = String.Empty
    Private FiltroPorEstiloModelo As String = String.Empty
    Private MonedaLocal As String = String.Empty
    Private MonedaSistema As String = String.Empty
    Private WithEvents Calendario As frmCalendario
    Private WithEvents CalendarioColor As frmCalendarioColor
    Private WithEvents CalendarioPorEquipos As frmListaCitas
    Private VersionSAP As Integer
    Private UsaVersionSAP9 As Boolean = False
    Private oTimer As Timer
    Private OpenFormUID As String = String.Empty
    Private CodigoArticuloPrevio As String = String.Empty
    Private CodigoBarrasPrevio As String = String.Empty
    Private EstadoCitaPrevio As String = String.Empty
    Private CotizacionPrevia As String = String.Empty
    Private CotizacionNueva As String = String.Empty


    Enum EstadoCita
        Pendiente = 1
        Confirmada = 2
        Cancelada = 3
    End Enum

    Enum TipoAgenda
        Mecanico = 0
        Agenda = 1
        Grupos = 2
    End Enum


    Sub New()
        Try
            n = DIHelper.GetNumberFormatInfo(DMS_Connector.Company.CompanySBO)
            UsaConfiguracionEstiloModelo = DMS_Connector.Configuracion.ParamGenAddon.U_UsaAXEV.Trim()
            FiltroPorEstiloModelo = DMS_Connector.Configuracion.ParamGenAddon.U_EspVehic.Trim()
            'Al abrir el formulario se selecciona la moneda local de manera predeterminada
            DMS_Connector.Helpers.GetCurrencies(MonedaLocal, MonedaSistema)
            VersionSAP = DMS_Connector.Company.CompanySBO.Version
            If VersionSAP >= 900000 Then
                UsaVersionSAP9 = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores predeterminados cuando se abre el formulario de citas en el modo crear desde la ventana Ocupación de agenda
    ''' </summary>
    ''' <param name="oFormulario">Formulario de citas recien abierto (Sin datos y en modo crear)</param>
    ''' <param name="Sucursal">Código de la sucursal seleccionada desde el formulario de Ocupación de Agenda</param>
    ''' <param name="CodigoAgenda">Código de la agenda seleccionada desde el formulario de Ocupación de Agenda</param>
    ''' <param name="Fecha">Fecha seleccionada para la cita</param>
    ''' <remarks></remarks>
    Public Sub AsignarValoresPorParametro(ByRef oFormulario As SAPbouiCOM.Form, ByVal Sucursal As String, ByVal CodigoAgenda As String, ByVal Fecha As Date)
        Dim oEditText As SAPbouiCOM.EditText
        Dim Query As String = "SELECT DocEntry, U_Cod_Sucursal, U_CodAsesor, U_CodTecnico, U_RazonCita, U_NameAsesor, U_NameTecnico  FROM [@SCGD_AGENDA] with(nolock) where DocEntry = '{0}'"
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim CodigoAsesor As String = String.Empty
        Dim CodigoTecnico As String = String.Empty
        Dim oComboBox As SAPbouiCOM.ComboBox
        Try
            oFormulario.Freeze(True)
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Query = String.Format(Query, CodigoAgenda)
            oRecordset.DoQuery(Query)

            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            oComboBox.Select(Sucursal, BoSearchKey.psk_ByValue)
            oComboBox = oFormulario.Items.Item("cboAgenda").Specific
            oComboBox.Select(CodigoAgenda, BoSearchKey.psk_ByValue)

            If oRecordset.RecordCount > 0 Then
                oComboBox = oFormulario.Items.Item("cboAsesor").Specific
                oComboBox.Select(oRecordset.Fields.Item("U_CodAsesor").Value.ToString(), BoSearchKey.psk_ByValue)
                'oComboBox = oFormulario.Items.Item("cboTecnico").Specific
                'oComboBox.Select(oRecordset.Fields.Item("U_CodTecnico").Value.ToString(), BoSearchKey.psk_ByValue)
                oComboBox = oFormulario.Items.Item("cboRazon").Specific
                oComboBox.Select(oRecordset.Fields.Item("U_RazonCita").Value.ToString(), BoSearchKey.psk_ByValue)
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Asesor", 0, oRecordset.Fields.Item("U_NameAsesor").Value.ToString())
                'oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Tecnico", 0, oRecordset.Fields.Item("U_NameTecnico").Value.ToString())
            End If

            CargarListaAgendas(oFormulario, Sucursal, False)
            CargarListaRazones(oFormulario, False)
            CargarListaAsesores(oFormulario, Sucursal, False)
            CargarListaTecnicos(oFormulario, Sucursal, False)

            oEditText = oFormulario.Items.Item("txtFhaCita").Specific
            oEditText.Value = Fecha.ToString("yyyyMMdd")
            oEditText = oFormulario.Items.Item("txtHora").Specific
            oEditText.Value = String.Format("{0}{1}", Fecha.ToString("HH"), Fecha.ToString("mm"))
            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores predeterminados cuando se abre el formulario de citas en el modo crear desde la ventana Ocupación de agenda
    ''' </summary>
    ''' <param name="oFormulario">Formulario de citas recien abierto (Sin datos y en modo crear)</param>
    ''' <param name="Sucursal">Código de la sucursal seleccionada desde el formulario de Ocupación de Agenda</param>
    ''' <param name="CodigoAgenda">Código de la agenda seleccionada desde el formulario de Ocupación de Agenda</param>
    ''' <param name="Fecha">Fecha seleccionada para la cita</param>
    ''' <remarks></remarks>
    Public Sub AsignarValoresPorParametro(ByRef oFormulario As SAPbouiCOM.Form, ByVal Sucursal As String, ByVal CodigoAgenda As String, ByVal CodigoAsesor As String, ByVal FechaCita As Date, ByVal CodigoTecnico As String, ByVal FechaServicio As Date)
        Dim oEditText As SAPbouiCOM.EditText
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim RazonCita As String = String.Empty
        Dim Query As String = "SELECT T0.U_RazonCita, (SELECT S1.firstName + ' ' +  S1.lastName FROM OHEM S1 WHERE empID = '{1}') AS U_NameAsesor,  (SELECT S1.firstName + ' ' +  S1.lastName FROM OHEM S1 WHERE empID = '{2}') AS U_NameTecnico  FROM [@SCGD_AGENDA] T0 with(nolock) where T0.DocEntry = '{0}'"
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oFormulario.Freeze(True)
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Query = String.Format(Query, CodigoAgenda, CodigoAsesor, CodigoTecnico)
            oRecordset.DoQuery(Query)

            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            oComboBox.Select(Sucursal, BoSearchKey.psk_ByValue)
            oComboBox = oFormulario.Items.Item("cboAgenda").Specific
            oComboBox.Select(CodigoAgenda, BoSearchKey.psk_ByValue)

            If Not String.IsNullOrEmpty(CodigoAsesor) Then
                oComboBox = oFormulario.Items.Item("cboAsesor").Specific
                oComboBox.Select(CodigoAsesor, BoSearchKey.psk_ByValue)
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Asesor", 0, oRecordset.Fields.Item("U_NameAsesor").Value.ToString())
                If Not FechaCita = Date.MinValue Then
                    oEditText = oFormulario.Items.Item("txtFhaCita").Specific
                    oEditText.Value = FechaCita.ToString("yyyyMMdd")
                    oEditText = oFormulario.Items.Item("txtHora").Specific
                    oEditText.Value = String.Format("{0}{1}", FechaCita.ToString("HH"), FechaCita.ToString("mm"))
                End If
            End If

            If Not String.IsNullOrEmpty(CodigoTecnico) Then
                oComboBox = oFormulario.Items.Item("cboTecnico").Specific
                oComboBox.Select(CodigoTecnico, BoSearchKey.psk_ByValue)
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Tecnico", 0, oRecordset.Fields.Item("U_NameTecnico").Value.ToString())
                If Not FechaServicio = Date.MinValue Then
                    oEditText = oFormulario.Items.Item("txtFhaServ").Specific
                    oEditText.Value = FechaServicio.ToString("yyyyMMdd")
                    oEditText = oFormulario.Items.Item("txtHoraSer").Specific
                    oEditText.Value = String.Format("{0}{1}", FechaServicio.ToString("HH"), FechaServicio.ToString("mm"))
                End If
            End If

            RazonCita = oRecordset.Fields.Item("U_RazonCita").Value.ToString()
            If Not String.IsNullOrEmpty(RazonCita) Then
                oComboBox = oFormulario.Items.Item("cboRazon").Specific
                oComboBox.Select(RazonCita, BoSearchKey.psk_ByValue)
            End If

            CargarListaAgendas(oFormulario, Sucursal, False)
            CargarListaRazones(oFormulario, False)
            CargarListaAsesores(oFormulario, Sucursal, False)
            CargarListaTecnicos(oFormulario, Sucursal, False)
            oFormulario.Freeze(False)
        Catch ex As Exception
            oFormulario.Freeze(False)
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores predeterminados cuando se abre el formulario de citas en el modo crear
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario de citas en blanco y en modo crear</param>
    ''' <remarks></remarks>
    Public Sub CargarValoresPredeterminados(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            oFormulario.Items.Item("tabCtrl").Click()
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("CreateDate", 0, DateTime.Now.ToString("yyyyMMdd"))
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CreadoPor", 0, DMS_Connector.Company.CompanySBO.UserName)
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            If (oFormulario.Mode = BoFormMode.fm_ADD_MODE) Then
                oFormulario.Items.Item("txt_NumCot").Enabled = False
            End If
            oDataTable.Rows.Clear()
            CargarSucursales(oFormulario)
            CargarListaMonedas(oFormulario)
            CargarListaEstados(oFormulario)
            CargarMotivosCancelacion(oFormulario)
            CargarFormasContacto(oFormulario)
            CargarMetodosMovilidad(oFormulario)
            SeleccionarMonedaLocal(oFormulario)
            LigarChooseFromListImpuesto(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vincula el objeto de "impuesto" al ChooseFromList de la matriz de artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub LigarChooseFromListImpuesto(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            'Selecciona el objeto de acuerdo a la configuración de DMS
            If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                oMatrix.Columns.Item("Col_Imp").ChooseFromListUID = "VATG"
            Else
                oMatrix.Columns.Item("Col_Imp").ChooseFromListUID = "VAT"
            End If
            oMatrix.Columns.Item("Col_Imp").ChooseFromListAlias = "Code"
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Llena los valores válidos del ComboBox Formas de Contacto, pestaña detalle del formulario
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub CargarFormasContacto(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = " SELECT T0.""Code"", T0.""Name"" FROM ""@SCGD_FCONTACTO"" T0 WITH(nolock) "
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboCntc").Specific
            'Las formas de contacto solamente deben agregarse una vez al abrir el formulario
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count = 0 Then
                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores válidos para el ComboBox movilidad de la pestaña detalle
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub CargarMetodosMovilidad(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = " SELECT T0.""Code"", T0.""Name"" FROM ""@SCGD_MOVILIDAD"" T0 WITH(nolock) "
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboMovi").Specific
            'Los métodos de movilidad solamente deben cargarse una vez al abrir el formulario
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count = 0 Then
                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores válidos para el ComboBox Motivos de Cancelación de la pestaña general
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub CargarMotivosCancelacion(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = " SELECT T0.""Code"", T0.""Name"" FROM ""@SCGD_MOTIVOCANC"" T0 WITH(nolock) "
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboMCanc").Specific
            'Los motivos de cancelación solamente deben agregarse al abrir el formulario por primera vez
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count = 0 Then
                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Selecciona la moneda local
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <remarks></remarks>
    Public Sub SeleccionarMonedaLocal(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Try
            oComboBox = oFormulario.Items.Item("cboMoneda").Specific
            If String.IsNullOrEmpty(MonedaLocal) Then
                DMS_Connector.Helpers.GetCurrencies(MonedaLocal, MonedaSistema)
            End If
            oComboBox.Select(MonedaLocal, BoSearchKey.psk_ByValue)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los valores válidos del ComboBox estados
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="CargarEstadoPredeterminado">True = Carga el estado predeterminado para una cita nueva. False = No selecciona ningún estado y deja el que esta seleccionado (Cuando se abre una cita ya existente)</param>
    ''' <remarks></remarks>
    Public Sub CargarListaEstados(ByRef oFormulario As SAPbouiCOM.Form, Optional ByVal CargarEstadoPredeterminado As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = "SELECT T0.""Code"", T0.""U_Descripcion"" FROM ""@SCGD_CITA_ESTADOS"" T0 WITH(nolock) ORDER BY T0.""Code"" ASC"
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboEstado").Specific

            'Los estados solamente se deben agregar una vez al abrir el formulario
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count = 0 Then
                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If

            If CargarEstadoPredeterminado Then
                'Selecciona el estado predeterminado de la cita
                If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count > 0 Then
                    For Each oValidValue As SAPbouiCOM.ValidValue In oComboBox.ValidValues
                        If oValidValue.Value = EstadoCita.Pendiente Then
                            oComboBox.Select(oValidValue.Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
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
    ''' Carga los valores válidos del ComboBox monedas
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub CargarListaMonedas(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = "SELECT T0.""CurrCode"", T0.""CurrName"" FROM ""OCRN"" T0 WITH(nolock)"
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboMoneda").Specific

            'Las monedas solamente deben agregarse al abrir el formulario
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count = 0 Then
                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el listado de sucursales en el ComboBox sucursal
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub CargarSucursales(ByRef oFormulario As SAPbouiCOM.Form, Optional ByVal CargarSucursalUsuario As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Sucursal As String = String.Empty
        Dim Query As String = "SELECT T0.""Code"", T0.""Name"" FROM ""@SCGD_SUCURSALES"" T0 WITH (nolock)  ORDER BY T0.""Name"""
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            Sucursal = ObtenerSucursalUsuario()
            'Las sucursales solamente deben agregarse una vez al abrir el formulario
            'en caso de agregar nuevas sucursales es necesario cerrar y volver a abrir el formulario
            If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count = 0 Then
                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If

            If CargarSucursalUsuario Then
                'Selecciona la sucursal del usuario conectado
                If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count > 0 Then
                    If Not String.IsNullOrEmpty(Sucursal) Then
                        For Each oValidValue As SAPbouiCOM.ValidValue In oComboBox.ValidValues
                            If oValidValue.Value = Sucursal Then
                                oComboBox.Select(Sucursal, SAPbouiCOM.BoSearchKey.psk_ByValue)
                                Exit For
                            End If
                        Next
                    End If
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
    ''' Manejador de eventos tipo Menú
    ''' </summary>
    ''' <param name="FormTypeEx">Tipo de formulario</param>
    ''' <param name="FormUID">Unique ID de la instancia del formulario</param>
    ''' <param name="pVal">Variable con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para definir si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Public Sub MenuEvent(ByVal FormTypeEx As String, ByVal FormUID As String, ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If FormTypeEx = "SCGD_CCIT" Then
                'Obtiene la instancia del formulario desde la cual se generó el evento
                oFormulario = ObtenerFormulario(FormUID)
                If oFormulario IsNot Nothing Then
                    Select Case pVal.MenuUID
                        Case "1281" 'Botón Buscar
                            CambiarModoBusqueda(oFormulario, pVal, BubbleEvent)
                        Case "1282" 'Botón Crear
                            CambiarModoCrear(oFormulario, pVal, BubbleEvent)
                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se encarga de limpiar el formulario cuando se cambia al modo búsqueda
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Variable con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para definir si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub CambiarModoBusqueda(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            CargarValoresPredeterminados(oFormulario)
            oFormulario.DataSources.UserDataSources.Item("marca").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("estilo").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("modelo").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("ano").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("combust").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("motor").ValueEx = String.Empty
            'Habilitar los controles número de serie y cita para búsquedas
            oFormulario.Items.Item("txtSerie").Enabled = True
            oFormulario.Items.Item("txtNoCita").Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se encarga de limpiar el formulario y cambiar el estado de los controles al pasar a modo crear
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para definir si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub CambiarModoCrear(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            CargarValoresPredeterminados(oFormulario)
            oFormulario.DataSources.UserDataSources.Item("marca").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("estilo").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("modelo").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("ano").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("combust").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("motor").ValueEx = String.Empty
            'Habilitar los controles número de serie y cita para búsquedas
            oFormulario.Items.Item("txtSerie").Enabled = False
            oFormulario.Items.Item("txtNoCita").Enabled = False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encarga de los eventos tipo FormData
    ''' </summary>
    ''' <param name="BusinessObjectInfo">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para definir si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Public Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If BusinessObjectInfo.FormTypeEx = "SCGD_CCIT" Then
                oFormulario = ObtenerFormulario(BusinessObjectInfo.FormUID)
                If oFormulario IsNot Nothing Then
                    Select Case BusinessObjectInfo.EventType
                        Case BoEventTypes.et_FORM_DATA_LOAD
                            FormDataLoad(oFormulario, BusinessObjectInfo, BubbleEvent)
                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de cargar los valid values y otros datos cuando se abre o busca un documento de tipo cita (Abrir o bien navegar con las flechas anterior, siguiente)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub CargarDatosDesdeAgenda(ByRef oFormulario As SAPbouiCOM.Form)
        Dim Sucursal As String = String.Empty
        Dim DocEntryUnidad As String = String.Empty
        Dim CodigoCitaCancelada As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Try
            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim
            CargarListaMonedas(oFormulario)
            CargarListaEstados(oFormulario, False)
            CargarMotivosCancelacion(oFormulario)
            CargarFormasContacto(oFormulario)
            CargarMetodosMovilidad(oFormulario)
            CargarSucursales(oFormulario, False)
            CargarListaAgendas(oFormulario, Sucursal, False)
            CargarListaRazones(oFormulario, False)
            CargarListaAsesores(oFormulario, Sucursal, False)
            CargarListaTecnicos(oFormulario, Sucursal, False)
            CargarLineasMatriz(oFormulario)
            DocEntryUnidad = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CodVehi", 0).Trim
            AsignarValoresUnidad(oFormulario, DocEntryUnidad, False)
            RecalcularTotales(oFormulario)
            CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(Sucursal)).U_CodCitaCancel.Trim
            EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim()
            If EstadoCita = CodigoCitaCancelada Then
                oFormulario.Mode = BoFormMode.fm_VIEW_MODE
            Else
                oFormulario.Mode = BoFormMode.fm_OK_MODE
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de cargar todos los valores válidos e información necesaria
    ''' cuando se abre un documento ya existente ya sea a través del buscador o mediante las flechas del navegador
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="BusinessObjectInfo">Objeto con información del evento</param>
    ''' <param name="BubbleEvent">Variable para definir si se continua con el evento o no</param>
    ''' <remarks>El método carga valores válidos, recalcula totales, cambia el modo del formulario entre otras cosas</remarks>
    Private Sub FormDataLoad(ByRef oFormulario As SAPbouiCOM.Form, ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Dim Sucursal As String = String.Empty
        Dim DocEntryUnidad As String = String.Empty
        Dim CodigoCitaCancelada As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim MonedaSeleccionada As String = String.Empty
        Try
            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim
            If BusinessObjectInfo.BeforeAction Then
                'Implementar manejo del BeforeAction aquí
            Else
                If BusinessObjectInfo.ActionSuccess Then
                    CargarListaAgendas(oFormulario, Sucursal, False)
                    CargarListaRazones(oFormulario, False)
                    CargarListaAsesores(oFormulario, Sucursal, False)
                    CargarListaTecnicos(oFormulario, Sucursal, False)
                    CargarLineasMatriz(oFormulario)
                    DocEntryUnidad = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CodVehi", 0).Trim
                    AsignarValoresUnidad(oFormulario, DocEntryUnidad, False)
                    RecalcularTotales(oFormulario)
                    CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(Sucursal)).U_CodCitaCancel.Trim
                    EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim()
                    If EstadoCita = CodigoCitaCancelada Then
                        oFormulario.Mode = BoFormMode.fm_VIEW_MODE
                    Else
                        oFormulario.Mode = BoFormMode.fm_OK_MODE
                    End If
                    oMatrix = oFormulario.Items.Item("mtxArtic").Specific
                    oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
                    ActualizarFormatoTabla(oMatrix, oDataTable)
                    MonedaSeleccionada = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Moneda", 0).Trim()
                    If MonedaSeleccionada = MonedaLocal Then
                        oFormulario.Items.Item("txtTipoC").Visible = False
                    Else
                        oFormulario.Items.Item("txtTipoC").Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Este método se encarga de cambiar el estado (Habilitado, Visible) de los distintos controles
    ''' de acuerdo al estado del formulario (Modo Crear, Modo Búsqueda)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks>Cualquier otro manejo de controles relacionado al cambio de estado del formulario,
    ''' debe implementarse en este método</remarks>
    Private Sub ManejadorControlesPorEstado(ByRef oFormulario As SAPbouiCOM.Form)
        Dim NumeroOT As String = String.Empty
        Try
            Select Case oFormulario.Mode
                Case BoFormMode.fm_ADD_MODE
                    oFormulario.Items.Item("txtCliente").Enabled = True
                    oFormulario.Items.Item("txtNombre").Enabled = True
                    oFormulario.Items.Item("txt_cliOT").Enabled = True
                    oFormulario.Items.Item("txt_NoClOT").Enabled = True
                    oFormulario.Items.Item("txtCodUnid").Enabled = True
                    oFormulario.Items.Item("txtPlaca").Enabled = True
                    oFormulario.Items.Item("cboSucur").Enabled = True
                    oFormulario.Items.Item("cboAgenda").Enabled = True
                    oFormulario.Items.Item("cboRazon").Enabled = True
                Case BoFormMode.fm_UPDATE_MODE
                    oFormulario.Items.Item("txtCliente").Enabled = False
                    oFormulario.Items.Item("txtNombre").Enabled = False
                    oFormulario.Items.Item("txt_cliOT").Enabled = False
                    oFormulario.Items.Item("txt_NoClOT").Enabled = False
                    oFormulario.Items.Item("txtCodUnid").Enabled = False
                    oFormulario.Items.Item("txtPlaca").Enabled = False
                    oFormulario.Items.Item("cboSucur").Enabled = False
                    oFormulario.Items.Item("cboAgenda").Enabled = False
                    oFormulario.Items.Item("cboRazon").Enabled = False
                Case BoFormMode.fm_OK_MODE
                    oFormulario.Items.Item("txtCliente").Enabled = False
                    oFormulario.Items.Item("txtNombre").Enabled = False
                    oFormulario.Items.Item("txt_cliOT").Enabled = False
                    oFormulario.Items.Item("txt_NoClOT").Enabled = False
                    oFormulario.Items.Item("txtCodUnid").Enabled = False
                    oFormulario.Items.Item("txtPlaca").Enabled = False
                    oFormulario.Items.Item("cboSucur").Enabled = False
                    oFormulario.Items.Item("cboAgenda").Enabled = False
                    oFormulario.Items.Item("cboRazon").Enabled = False
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de cargar las líneas de la matriz
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <remarks>Las líneas de la matriz no son una tabla hija del UDO, son las líneas de la cotización y se deben cargar
    ''' en una tabla en memoria temporal (DataTable) cada vez que se abre un documento o se navega (Flechas del menú superior)</remarks>
    Private Sub CargarLineasMatriz(ByRef oFormulario As SAPbouiCOM.Form)
        Dim DocEntryCotizacion As String = String.Empty
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim TipoCambio As Double
        Dim TotalLinea As Double
        Dim CodigoArticulo As String = String.Empty
        Dim Descripcion As String = String.Empty
        Dim Moneda As String = String.Empty
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim MonedaSeleccionada As String = String.Empty
        Dim FechaCita As String = String.Empty
        Dim Fecha As Date
        Dim NumeroLinea As Integer
        Dim Precio As Double
        Dim Cantidad As Double
        Dim CodigoBarras As String = String.Empty
        Dim Impuesto As String = String.Empty
        Try
            'Abre la cotización y carga manualmente todas las líneas en el DataTable temporal "listServicios" del UDO
            DocEntryCotizacion = oFormulario.DataSources.DBDataSources().Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim()

            oComboBox = oFormulario.Items.Item("cboMoneda").Specific

            If oComboBox.Selected IsNot Nothing Then
                MonedaSeleccionada = oComboBox.Selected.Value
            End If

            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            oDataTable.Rows.Clear()
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oMatrix.LoadFromDataSource()
            If Not String.IsNullOrEmpty(DocEntryCotizacion) Then
                oCotizacion = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                If oCotizacion.GetByKey(DocEntryCotizacion) Then

                    'Buscar el tipo de cambio del acuerdo a la fecha de la cita
                    FechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0)
                    Fecha = Date.ParseExact(FechaCita, "yyyyMMdd", Nothing)
                    TipoCambio = ObtenerTipoCambio(MonedaSeleccionada, Fecha)

                    For i As Integer = 0 To oCotizacion.Lines.Count - 1
                        oCotizacion.Lines.SetCurrentLine(i)
                        oDataTable.Rows.Add()
                        CodigoArticulo = oCotizacion.Lines.ItemCode
                        Descripcion = oCotizacion.Lines.ItemDescription
                        Moneda = oCotizacion.Lines.Currency
                        NumeroLinea = oCotizacion.Lines.LineNum
                        Precio = oCotizacion.Lines.Price
                        Cantidad = oCotizacion.Lines.Quantity
                        CodigoBarras = oCotizacion.Lines.BarCode
                        oDataTable.SetValue("codigo", i, CodigoArticulo)
                        oDataTable.SetValue("descripcion", i, Descripcion)
                        oDataTable.SetValue("moneda", i, Moneda)
                        oDataTable.SetValue("linea", i, NumeroLinea)
                        oDataTable.SetValue("precio", i, Precio)
                        oDataTable.SetValue("cantidad", i, Cantidad)
                        oDataTable.SetValue("barras", i, CodigoBarras)

                        'Solamente las líneas de tipo ingrediente se marcan como líneas hijas
                        If oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                            oDataTable.SetValue("hijo", i, "Y")
                        End If

                        'Solamente las listas de materiales de tipo S (Sales) se marcan como lista de materiales
                        'ya que los modelos una vez creados se comportan como artículos individuales
                        If oCotizacion.Lines.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                            oDataTable.SetValue("paquete", i, "S")
                        End If

                        If String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value) Then
                            oDataTable.SetValue("tipo", i, String.Empty)
                        Else
                            oDataTable.SetValue("tipo", i, oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value)
                        End If

                        If String.IsNullOrEmpty(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value) OrElse oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value.Equals("5") Then
                            oDataTable.SetValue("duracion", i, 0)
                        Else
                            oDataTable.SetValue("duracion", i, oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value)
                        End If

                        If oCotizacion.DocCurrency.Equals(MonedaLocal) Then
                            TotalLinea = oCotizacion.Lines.LineTotal
                            oDataTable.SetValue("total", i, oCotizacion.Lines.LineTotal)
                        Else
                            TotalLinea = oCotizacion.Lines.LineTotal / TipoCambio
                            oDataTable.SetValue("total", i, TotalLinea)
                        End If
                        Impuesto = oCotizacion.Lines.TaxCode
                        oDataTable.SetValue("impuesto", i, Impuesto)
                    Next
                    oDataTable.Rows.Add()
                    oMatrix.LoadFromDataSource()
                End If
            End If
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
            If pVal.FormTypeEx = "SCGD_CCIT" Then
                'Obtiene la instancia del formulario desde la cual se generó el evento
                oFormulario = ObtenerFormulario(FormUID)
                If oFormulario IsNot Nothing Then
                    Select Case pVal.EventType
                        Case BoEventTypes.et_ITEM_PRESSED
                            ItemPressed(oFormulario, pVal, BubbleEvent)
                        Case BoEventTypes.et_COMBO_SELECT
                            ComboSelect(oFormulario, pVal, BubbleEvent)
                        Case BoEventTypes.et_CHOOSE_FROM_LIST
                            ChooseFromList(oFormulario, pVal, BubbleEvent)
                        Case BoEventTypes.et_VALIDATE
                            Validate(oFormulario, pVal, BubbleEvent)
                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de los eventos de tipo Validate
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para determinar si se debe continuar o no con el evento</param>
    ''' <remarks></remarks>
    Private Sub Validate(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                'Implementar manejo del BeforeAction aquí
            Else
                Select Case pVal.ItemUID
                    Case "mtxArtic"
                        ValidarColumnasMatriz(oFormulario, pVal, BubbleEvent)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de redondear las horas de la cita a horas que se adapten a los intervalos de las agendas
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks>Ejemplo: La agenda esta configurada con intervalos de 15 minutos e ingresamos una cita a las 3:34
    ''' al no adaptarse al intervalo, se debe hacer un redondeo al intervalo más cercano en este caso 3:30</remarks>
    Private Sub ValidarYAjustarFormatoHoras(ByRef oFormulario As SAPbouiCOM.Form)
        Dim TextoHoraCita As String = String.Empty
        Dim TextoHoraServicio As String = String.Empty
        Dim Agenda As String = String.Empty
        Dim IntervaloCita As Integer = 15
        Dim CodigoSucursal As String = String.Empty
        Dim CodigoAgenda As String = String.Empty
        Dim Query As String = " SELECT U_IntervaloCitas FROM [@SCGD_AGENDA] with (nolock) WHERE DocEntry = '{0}' AND U_Cod_Sucursal = '{1}' "
        Dim HoraCita As DateTime
        Dim HoraServicio As DateTime
        Dim ListaMinutosValidos As List(Of Integer)
        Dim Contador As Integer = 60
        Dim Diferencia As Integer
        Dim MinutosAjustados As Integer
        Try
            ListaMinutosValidos = New List(Of Integer)
            TextoHoraCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita", 0).Trim()
            TextoHoraServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraServ", 0).Trim()

            CodigoSucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            CodigoAgenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()

            If Not String.IsNullOrEmpty(CodigoSucursal) AndAlso Not String.IsNullOrEmpty(CodigoAgenda) Then
                Query = String.Format(Query, CodigoAgenda, CodigoSucursal)
                Integer.TryParse(DMS_Connector.Helpers.EjecutarConsulta(Query), IntervaloCita)

                While Contador > 0
                    Contador -= IntervaloCita
                    ListaMinutosValidos.Add(Contador)
                End While

                If Not String.IsNullOrEmpty(TextoHoraCita) Then
                    If TextoHoraCita.Length = 3 Then
                        TextoHoraCita = String.Format("0{0}", TextoHoraCita)
                    End If
                    HoraCita = DateTime.ParseExact(TextoHoraCita, "HHmm", Nothing)
                    Diferencia = HoraCita.Minute - 0
                    For Each valor As Integer In ListaMinutosValidos
                        If Math.Abs(valor - HoraCita.Minute) < Math.Abs(Diferencia) Or Math.Abs(valor - HoraCita.Minute) = Math.Abs(Diferencia) Then
                            Diferencia = valor - HoraCita.Minute
                        End If
                    Next
                    HoraCita = HoraCita.AddMinutes(Diferencia)
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraCita", 0, HoraCita.ToString("HHmm"))
                End If

                If Not String.IsNullOrEmpty(TextoHoraServicio) Then
                    If TextoHoraServicio.Length = 3 Then
                        TextoHoraServicio = String.Format("0{0}", TextoHoraServicio)
                    End If
                    HoraServicio = DateTime.ParseExact(TextoHoraServicio, "HHmm", Nothing)
                    Diferencia = HoraServicio.Minute - 0
                    For Each valor As Integer In ListaMinutosValidos
                        If Math.Abs(valor - HoraServicio.Minute) < Math.Abs(Diferencia) Or Math.Abs(valor - HoraServicio.Minute) = Math.Abs(Diferencia) Then
                            Diferencia = valor - HoraServicio.Minute
                        End If
                    Next
                    HoraServicio = HoraServicio.AddMinutes(Diferencia)
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraServ", 0, HoraServicio.ToString("HHmm"))
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida los datos de las distintas columnas de la matriz mtxArtic
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para determinar si se continua o no con el evento</param>
    ''' <remarks></remarks>
    Private Sub ValidarColumnasMatriz(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.ColUID
                Case "Col_Prec"
                    ValidarColumnaPrecio(oFormulario, pVal, BubbleEvent)
                Case "Col_Imp"
                    ValidarColumnaImpuesto(oFormulario, pVal, BubbleEvent)
                Case "Col_Cant"
                    ValidarColumnaCantidad(oFormulario, pVal, BubbleEvent)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida los datos de la columna cantidad
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ValidarColumnaCantidad(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            RecalcularTotales(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida los datos de la columna impuesto
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se continua o no con el evento</param>
    ''' <remarks></remarks>
    Private Sub ValidarColumnaImpuesto(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            RecalcularTotales(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida la columna precio
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ValidarColumnaPrecio(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            RecalcularTotales(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Recalcula los totales por línea, subtotal, impuesto, total general, tiempo estimado y cantidad de servicios
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Public Sub RecalcularTotales(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim TotalLinea As Double = 0
        Dim SubTotal As Double = 0
        Dim ImpuestoLinea As Double = 0
        Dim ImpuestoTotal As Double = 0
        Dim Total As Double = 0
        Dim Cantidad As Double = 0
        Dim Precio As Double = 0
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim CantidadServicios As Integer = 0
        Dim PorcentajeImpuesto As Double = 0
        Dim IndicadorImpuesto As String = String.Empty
        Dim DuracionTotal As Double = 0
        Dim FormatoHoras As String = String.Empty
        Dim CodigoTecnico As String = String.Empty
        Dim QueryTiempoServicioRapido As String = " SELECT U_SCGD_TiempServ FROM OHEM with (nolock) WHERE empID = '{0}' "
        Dim TiempoServicioRapido As Decimal = 0
        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oMatrix.FlushToDataSource()
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            CodigoTecnico = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Tecnico", 0).Trim()

            'Recorre todas las líneas del datatable y realiza las sumatorias
            For i As Integer = 0 To oDataTable.Rows.Count - 1
                If oDataTable.GetValue("impuesto", i) <> IndicadorImpuesto Then
                    IndicadorImpuesto = oDataTable.GetValue("impuesto", i)
                    PorcentajeImpuesto = DMS_Connector.Helpers.GetTaxRate(IndicadorImpuesto, DateTime.Now, DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup.Trim().Equals("Y"))
                End If

                Cantidad = oDataTable.GetValue("cantidad", i)
                Precio = oDataTable.GetValue("precio", i)
                TotalLinea = Cantidad * Precio
                oDataTable.SetValue("total", i, TotalLinea)
                SubTotal += TotalLinea
                ImpuestoLinea = TotalLinea * (PorcentajeImpuesto / 100)
                ImpuestoTotal += ImpuestoLinea
                DuracionTotal += (oDataTable.GetValue("duracion", i) * Cantidad)

                'Cuenta la cantidad de artículos de tipo servicio
                If oDataTable.GetValue("tipo", i) = "2" Then
                    CantidadServicios += 1
                End If
            Next
            Total = SubTotal + ImpuestoTotal
            oFormulario.DataSources.UserDataSources.Item("serv").ValueEx = CantidadServicios
            FormatoHoras = oFormulario.DataSources.UserDataSources.Item("tiemp").ValueEx
            If FormatoHoras = "Y" Then
                DuracionTotal = DuracionTotal / 60
            End If

            'Consulta el tiempo de servicio rápido
            If Not String.IsNullOrEmpty(CodigoTecnico) Then
                QueryTiempoServicioRapido = String.Format(QueryTiempoServicioRapido, CodigoTecnico)
                Decimal.TryParse(DMS_Connector.Helpers.EjecutarConsulta(QueryTiempoServicioRapido), TiempoServicioRapido)
            End If

            If TiempoServicioRapido > 0 Then
                oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx = TiempoServicioRapido.ToString(n)
            Else
                oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx = DuracionTotal.ToString(n)
            End If

            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Total_Lin", 0, SubTotal.ToString(n))
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Total_Imp", 0, ImpuestoTotal.ToString(n))
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Total_Doc", 0, Total.ToString(n))
            oMatrix.LoadFromDataSource()
            CalcularFechaFinalizacion(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de los eventos de tipo ChooseFromList
    ''' </summary>
    ''' <param name="oFormulario">Formulario desde el cual se ejecutó el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Boolean que indica si se debe continuar con el proceso no</param>
    ''' <remarks></remarks>
    Private Sub ChooseFromList(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.ItemUID
                Case "txtCodUnid"
                    ManejadorChooseFromListCodigoUnidadPlaca(oFormulario, pVal, BubbleEvent)
                Case "txtPlaca"
                    ManejadorChooseFromListCodigoUnidadPlaca(oFormulario, pVal, BubbleEvent)
                Case "txtCliente"
                    ManejadorChooseFromListCodigoCliente(oFormulario, pVal, BubbleEvent)
                Case "txtNombre"
                    ManejadorChooseFromListNombreCliente(oFormulario, pVal, BubbleEvent)
                Case "txt_cliOT"
                    ManejadorChooseFromListCodigoClienteOT(oFormulario, pVal, BubbleEvent)
                Case "txt_NoClOT"
                    ManejadorChooseFromListNombreClienteOT(oFormulario, pVal, BubbleEvent)
                Case "mtxArtic"
                    ManejadorChooseFromListMatriz(oFormulario, pVal, BubbleEvent)
                Case "txt_NumCot"
                    If (oFormulario.Mode = BoFormMode.fm_ADD_MODE) Then
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.CitaPorCrear, BoMessageTime.bmt_Medium, BoStatusBarMessageType.smt_Warning)
                    Else
                        ManejadorChooseFromListCotizacion(oFormulario, pVal, BubbleEvent)
                    End If

            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del ChooseFromList Cotizaciones
    ''' </summary>
    ''' <param name="oFormulario"></param>
    ''' <param name="pVal"></param>
    ''' <param name="BubbleEvent"></param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListCotizacion(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim CodigoCliente As String = String.Empty
        Dim NoUnidad As String = String.Empty
        Dim Sucursal As String = String.Empty

        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim
                NoUnidad = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Unid", 0).Trim
                CotizacionPrevia = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim
                Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim

                oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "CardCode"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = CodigoCliente.ToString()
                oCondition.BracketCloseNum = 1

                oCondition.Relationship = BoConditionRelationship.cr_AND
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "DocStatus"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "O"
                oCondition.BracketCloseNum = 1

                oCondition.Relationship = BoConditionRelationship.cr_AND
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_Cod_Unidad"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = NoUnidad.ToString()
                oCondition.BracketCloseNum = 1

                oCondition.Relationship = BoConditionRelationship.cr_AND
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_SCGD_idSucursal"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = Sucursal.ToString()
                oCondition.BracketCloseNum = 1

                oCFL.SetConditions(oConditions)
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Num_Cot", 0, oDataTable.GetValue("DocEntry", 0))
                    CotizacionNueva = oDataTable.GetValue("DocEntry", 0)
                    oFormulario.Mode = BoFormMode.fm_UPDATE_MODE
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del ChooseFromList Codigo Cliente
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListCodigoCliente(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oDataTable As SAPbouiCOM.DataTable

        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then

                oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "CardType"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "C"
                oCondition.BracketCloseNum = 1
                oCondition.Relationship = BoConditionRelationship.cr_AND
                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "frozenFor"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCondition.CondVal = "Y"
                oCondition.BracketCloseNum = 1

                oCFL.SetConditions(oConditions)
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CardCode", 0, oDataTable.GetValue("CardCode", 0))
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CardName", 0, oDataTable.GetValue("CardName", 0))
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del ChooseFromList Nombre Cliente
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListNombreCliente(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim NombreCliente As String = String.Empty
        Dim oEditText As SAPbouiCOM.EditText
        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                oEditText = oFormulario.Items.Item("txtNombre").Specific
                NombreCliente = oEditText.Value.Trim
                'Se debe limpiar el EditText antes de llamar al ChooseFromList de lo contrario 
                'no funciona el BoConditionOperation.co_CONTAIN correctamente
                oEditText.Value = String.Empty
                oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                oCondition = oConditions.Add()
                oCondition.Alias = "CardName"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_CONTAIN
                oCondition.CondVal = NombreCliente
                oCFL.SetConditions(oConditions)
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CardCode", 0, oDataTable.GetValue("CardCode", 0))
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CardName", 0, oDataTable.GetValue("CardName", 0))
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de ChooseFromListo CodigoClienteOT
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListCodigoClienteOT(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                'Implementar manejo del BeforeAction aquí
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CCliOT", 0, oDataTable.GetValue("CardCode", 0))
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_NCliOT", 0, oDataTable.GetValue("CardName", 0))
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de ChooseFromListo NombreClienteOT
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListNombreClienteOT(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                'Implementar manejo del BeforeAction aquí
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CCliOT", 0, oDataTable.GetValue("CardCode", 0))
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_NCliOT", 0, oDataTable.GetValue("CardName", 0))
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de ChooseFromListo Codigo Unidad y Placa
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListCodigoUnidadPlaca(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim CodigoCliente As String = String.Empty
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim DocEntryVehiculo As String = String.Empty

        Try
            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim

                If Not String.IsNullOrEmpty(CodigoCliente) Then
                    oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add()
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_CardCode"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = CodigoCliente
                    oCondition.BracketCloseNum = 1
                Else
                    oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                    oCondition = oConditions.Add()

                    If pVal.ItemUID = "txtPlaca" Then
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_Num_Plac"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                        oCondition.BracketCloseNum = 1
                    ElseIf pVal.ItemUID = "txtCodUnid" Then
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_Cod_Unid"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                        oCondition.BracketCloseNum = 1
                    End If
                End If
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                oCondition = oConditions.Add()
                oCondition.BracketOpenNum = 2
                oCondition.Alias = "U_Activo"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "Y"
                oCondition.BracketCloseNum = 2

                oCFL.SetConditions(oConditions)
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing AndAlso oFormulario.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                    DocEntryVehiculo = oDataTable.Columns.Item("DocEntry").Cells.Item(0).Value()
                    AsignarValoresUnidad(oFormulario, DocEntryVehiculo)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Asigna los valores de la unidad seleccionada en el ChooseFromList Unidad o Placa
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="DocEntryVehiculo">Código interno del vehículo (DocEntry) en formato texto</param>
    ''' <param name="VerificarCampana">Indica si se debe mostrar el MessageBox indicando que el vehículo tiene campaña</param>
    ''' <remarks></remarks>
    Private Sub AsignarValoresUnidad(ByRef oFormulario As SAPbouiCOM.Form, ByVal DocEntryVehiculo As String, Optional ByVal VerificarCampana As Boolean = True)
        Dim Combustible As String = String.Empty
        Dim NumeroMotor As String = String.Empty
        Dim CodigoCliente As String = String.Empty
        Dim CodigoCampana As String = String.Empty
        Dim NombreCampana As String = String.Empty
        Dim MultiplesCampanasAsignadas As Boolean = False
        Dim Observaciones As String = String.Empty
        Dim CodigoUnidad As String = String.Empty
        Dim Query As String = " SELECT T0.""DocEntry"", T0.""U_CardCode"", T0.""U_CardName"", T0.""U_Cod_Unid"", T0.""U_Num_Plac"", T0.""U_Des_Marc"", T0.""U_Des_Esti"", T0.""U_Des_Mode"", T0.""U_Ano_Vehi"", ISNULL(T1.""Name"",'') AS 'DscCombusti', ISNULL(T2.""Name"",'') AS 'DscMarcaMot' FROM ""@SCGD_VEHICULO"" T0 WITH (nolock) LEFT JOIN ""@SCGD_COMBUSTIBLE"" T1 WITH (nolock) ON T0.""U_Combusti"" = T1.""Code"" LEFT JOIN ""@SCGD_MARCA_MOTOR"" T2 WITH (nolock) ON T0.""U_MarcaMot"" = T2.""Code"" WHERE T0.""DocEntry"" = '{0}' "
        Dim oRecordSet As SAPbobsCOM.Recordset

        Try
            'Limpiamos los campos relacionados al vehículo
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CodVehi", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Num_Placa", 0, String.Empty)
            oFormulario.DataSources.UserDataSources.Item("marca").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("estilo").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("modelo").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("ano").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("combust").ValueEx = String.Empty
            oFormulario.DataSources.UserDataSources.Item("motor").ValueEx = String.Empty
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CpnNo", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CpnName", 0, String.Empty)

            If Not String.IsNullOrEmpty(DocEntryVehiculo) Then
                Query = String.Format(Query, DocEntryVehiculo)
                oRecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordSet.DoQuery(Query)

                If oRecordSet.RecordCount > 0 Then
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CodVehi", 0, oRecordSet.Fields.Item(0).Value)
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CCliOT", 0, oRecordSet.Fields.Item(1).Value)
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_NCliOT", 0, oRecordSet.Fields.Item(2).Value)
                    CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim
                    If String.IsNullOrEmpty(CodigoCliente) Then
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CardCode", 0, oRecordSet.Fields.Item(1).Value)
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CardName", 0, oRecordSet.Fields.Item(2).Value)
                    End If

                    CodigoUnidad = oRecordSet.Fields.Item(3).Value
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Cod_Unid", 0, CodigoUnidad)
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Num_Placa", 0, oRecordSet.Fields.Item(4).Value)
                    oFormulario.DataSources.UserDataSources.Item("marca").ValueEx = oRecordSet.Fields.Item(5).Value
                    oFormulario.DataSources.UserDataSources.Item("estilo").ValueEx = oRecordSet.Fields.Item(6).Value
                    oFormulario.DataSources.UserDataSources.Item("modelo").ValueEx = oRecordSet.Fields.Item(7).Value
                    oFormulario.DataSources.UserDataSources.Item("ano").ValueEx = oRecordSet.Fields.Item(8).Value
                    oFormulario.DataSources.UserDataSources.Item("combust").ValueEx = oRecordSet.Fields.Item(9).Value
                    oFormulario.DataSources.UserDataSources.Item("motor").ValueEx = oRecordSet.Fields.Item(10).Value

                    'Verifica si las campañas están habilitadas y muestra un mensaje al usuario
                    If DMS_Connector.Configuracion.ParamGenAddon.U_CnpDMS.Trim().Equals("Y") AndAlso VerificarCampana Then
                        Observaciones = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Observ", 0).Trim
                        Observaciones += " " + Utilitarios.VerificaCampanaPorUnidad(CodigoUnidad, String.Empty, DMS_Connector.Company.ApplicationSBO, MultiplesCampanasAsignadas, CodigoCampana, NombreCampana)
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Observ", 0, Observaciones)

                        If MultiplesCampanasAsignadas Then
                            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CpnNo", 0, My.Resources.Resource.Multiples)
                            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CpnName", 0, My.Resources.Resource.MultiplesCampanas)
                        Else
                            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CpnNo", 0, CodigoCampana)
                            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_CpnName", 0, NombreCampana)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Sub



    ''' <summary>
    ''' Manejador ChooseFromList para todas las columnas de la matriz
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se continua o no con el evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorChooseFromListMatriz(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim sCFL_ID As String
        Dim oCondition As SAPbouiCOM.Condition
        Dim oConditions As SAPbouiCOM.Conditions
        Dim CodigoCliente As String = String.Empty
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim CodigoArticulo As String = String.Empty
        Dim DocEntryCotizacion As String = String.Empty
        Try

            oCFLEvento = CType(pVal, SAPbouiCOM.IChooseFromListEvent)
            sCFL_ID = oCFLEvento.ChooseFromListUID
            oCFL = oFormulario.ChooseFromLists.Item(sCFL_ID)

            If pVal.BeforeAction Then
                CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
                CodigoArticuloPrevio = oFormulario.DataSources.DataTables.Item("listServicios").GetValue("codigo", pVal.Row - 1)
                CodigoBarrasPrevio = oFormulario.DataSources.DataTables.Item("listServicios").GetValue("barras", pVal.Row - 1)
                DocEntryCotizacion = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim()
                Select Case pVal.ColUID
                    Case "Col_Code"
                        If String.IsNullOrEmpty(CodigoCliente) Then
                            DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.DebeSeleccionarSN, BoMessageTime.bmt_Short, True)
                            BubbleEvent = False
                        End If
                        If ExisteOrdenTrabajo(DocEntryCotizacion) Then
                            DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.BloqueoAgregarEliminarLineas, BoMessageTime.bmt_Short, True)
                            BubbleEvent = False
                        End If
                    Case "Col_Barra"
                        If String.IsNullOrEmpty(CodigoCliente) Then
                            DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.DebeSeleccionarSN, BoMessageTime.bmt_Short, True)
                            BubbleEvent = False
                        End If
                        If ExisteOrdenTrabajo(DocEntryCotizacion) Then
                            DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.BloqueoAgregarEliminarLineas, BoMessageTime.bmt_Short, True)
                            BubbleEvent = False
                        End If
                    Case "Col_Imp"
                        oConditions = DMS_Connector.Company.ApplicationSBO.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_Conditions)
                        oCondition = oConditions.Add()
                        If (DMS_Connector.Configuracion.ParamGenAddon.U_UsaVATGroup = "Y") Then
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "Category"
                            oCondition.CondVal = "O"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Locked"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        Else
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "ValidForAR"
                            oCondition.CondVal = "Y"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.BracketCloseNum = 1
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 2
                            oCondition.Alias = "Lock"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = "N"
                            oCondition.BracketCloseNum = 2
                        End If
                        oCFL.SetConditions(oConditions)
                End Select
            Else
                oDataTable = oCFLEvento.SelectedObjects
                If oDataTable IsNot Nothing Then
                    Select Case pVal.ColUID
                        Case "Col_Code"
                            'Asignar valores del artículo
                            CodigoArticulo = oDataTable.GetValue("ItemCode", 0)
                            AsignarValoresArticulo(oFormulario, CodigoArticulo, pVal)
                        Case "Col_Barra"
                            'Asignar valores del código de barras
                            CodigoArticulo = oDataTable.GetValue("ItemCode", 0)
                            AsignarValoresArticulo(oFormulario, CodigoArticulo, pVal)
                        Case "Col_Imp"
                            'Asignar valores de impuesto
                            AsignarValoresImpuesto(oFormulario, pVal, oDataTable)
                    End Select
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agrega el artículo indicado a la matriz de artículos (Cuando se digita directamente en la matriz el código o se selecciona desde el ChooseFromList)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="CodigoArticulo">Código del artículo (ItemCode) en formato texto</param>
    ''' <remarks></remarks>
    Private Sub AsignarValoresArticulo(ByRef oFormulario As SAPbouiCOM.Form, ByVal CodigoArticulo As String, ByRef pVal As ItemEvent)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oDatosArticulo As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim UltimaLinea As Integer
        Dim Sucursal As String = String.Empty
        Dim TipoArticulo As String = String.Empty
        Dim Cantidad As Double = 0
        Dim Precio As Double = 0
        Dim TipoPaquete As String = String.Empty
        Dim UsaPrecioArticuloPadre As String
        Dim Moneda As String = String.Empty
        Dim Query As String = "SELECT it.ItemCode As 'Code', it.ItemName As 'Dsc', IT.CodeBars As 'BarCode', i1.Currency As Curr, i1.Price As Price, it.U_SCGD_Duracion As 'Dura', it.U_SCGD_TipoArticulo As 'Type', it.""TreeType"" FROM OITM it INNER JOIN ITM1 i1 on it.ItemCode = i1.ItemCode WHERE  it.ItemCode = '{0}'  AND i1.PriceList = '{1}' "
        Dim ListaPrecios As String = String.Empty
        Dim CodigoCliente As String = String.Empty
        Dim strImpuesto As String = String.Empty
        Dim strItemCode As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(CodigoArticulo) Then
                oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
                oDatosArticulo = oFormulario.DataSources.DataTables.Item("ItemData")
                UsaPrecioArticuloPadre = DMS_Connector.Helpers.EjecutarConsulta("Select TreePricOn from OADM")
                Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
                CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
                ListaPrecios = ObtenerListaPrecios(Sucursal, CodigoCliente)

                oMatrix = oFormulario.Items.Item("mtxArtic").Specific
                oMatrix.FlushToDataSource()

                Query = String.Format(Query, CodigoArticulo, ListaPrecios)
                oDatosArticulo.ExecuteQuery(Query)

                If oDatosArticulo.Rows.Count > 0 Then
                    If (oDataTable.Rows.Count = 1 AndAlso String.IsNullOrEmpty(oDataTable.GetValue("codigo", 0))) Or (oDataTable.IsEmpty() AndAlso oDataTable.Rows.Count = 1) Then
                        UltimaLinea = 0
                        oDataTable.Rows.Add()
                    Else
                        oDataTable.SetValue("codigo", pVal.Row - 1, CodigoArticuloPrevio)
                        oDataTable.SetValue("barras", pVal.Row - 1, CodigoBarrasPrevio)
                        UltimaLinea = oDataTable.Rows.Count - 1
                        oDataTable.Rows.Add()
                        If Not String.IsNullOrEmpty(oDataTable.GetValue("codigo", UltimaLinea)) Then
                            UltimaLinea = oDataTable.Rows.Count - 1
                        End If
                    End If

                    oDataTable.SetValue("codigo", UltimaLinea, oDatosArticulo.GetValue("Code", 0))
                    strItemCode = oDatosArticulo.GetValue("Code", 0)
                    oDataTable.SetValue("descripcion", UltimaLinea, oDatosArticulo.GetValue("Dsc", 0))
                    oDataTable.SetValue("cantidad", UltimaLinea, 1)
                    Moneda = oDatosArticulo.GetValue("Curr", 0)
                    If String.IsNullOrEmpty(Moneda) Then
                        If String.IsNullOrEmpty(MonedaLocal) Then
                            Moneda = ObtenerMonedaLocal()
                        Else
                            Moneda = MonedaLocal
                        End If
                    End If
                    oDataTable.SetValue("moneda", UltimaLinea, Moneda)
                    TipoArticulo = oDatosArticulo.GetValue("Type", 0)
                    oDataTable.SetValue("tipo", UltimaLinea, TipoArticulo)
                    oDataTable.SetValue("duracion", UltimaLinea, oDatosArticulo.GetValue("Dura", 0))
                    strImpuesto = String.Empty
                    If DMS_Connector.Configuracion.ParamGenAddon.U_LocCR = "Y" Then
                        If Not String.IsNullOrEmpty(CodigoCliente) And Not String.IsNullOrEmpty(strItemCode) Then
                            strImpuesto = DMS_Connector.Business_Logic.ImpuestoBL.ObtenerImpuesto(oFormulario, CodigoCliente, strItemCode)
                            If Not String.IsNullOrEmpty(strImpuesto) Then
                                oDataTable.SetValue("impuesto", UltimaLinea, strImpuesto)
                            End If
                        End If
                        If String.IsNullOrEmpty(strImpuesto) Then
                            oDataTable.SetValue("impuesto", UltimaLinea, ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo))
                        End If
                    Else
                        oDataTable.SetValue("impuesto", UltimaLinea, ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo))
                    End If
                    oDataTable.SetValue("hijo", UltimaLinea, "N")
                    oDataTable.SetValue("padre", UltimaLinea, String.Empty)
                    TipoPaquete = oDatosArticulo.GetValue("TreeType", 0)
                    oDataTable.SetValue("paquete", UltimaLinea, TipoPaquete)
                    If TipoArticulo = TiposArticulo.Paquete AndAlso (TipoPaquete = "S" Or TipoPaquete = "T") Then
                        'Solamente se muestra el precio del artículo padre si esta habilitada la configuración
                        'o si la lista de materiales es de tipo modelo
                        If UsaPrecioArticuloPadre = "Y" Or TipoPaquete = "T" Then
                            oDataTable.SetValue("precio", UltimaLinea, oDatosArticulo.GetValue("Price", 0))
                        Else
                            oDataTable.SetValue("precio", UltimaLinea, 0)
                        End If
                        AgregarLineasHijas(oFormulario, oDataTable, CodigoArticulo, TipoPaquete, Sucursal, UsaPrecioArticuloPadre, ListaPrecios)
                    Else
                        oDataTable.SetValue("precio", UltimaLinea, oDatosArticulo.GetValue("Price", 0))
                    End If

                    oDataTable.SetValue("barras", UltimaLinea, oDatosArticulo.GetValue("BarCode", 0))

                    oMatrix.LoadFromDataSource()
                    ControladorCitas.ConvertirMontosDesdeBusqueda(oFormulario)
                    If oFormulario.Mode = BoFormMode.fm_OK_MODE Then
                        oFormulario.Mode = BoFormMode.fm_UPDATE_MODE
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            ControladorCitas.ActualizarFormatoTabla(oMatrix, oDataTable)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el código de la lista de precios que se debe utilizar de acuerdo a la configuración de la sucursal y el cliente seleccionado
    ''' </summary>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="CodigoCliente">Código del cliente</param>
    ''' <returns>Código de la lista de precios</returns>
    ''' <remarks></remarks>
    Public Function ObtenerListaPrecios(ByVal CodigoSucursal As String, ByVal CodigoCliente As String) As String
        Dim UsaListaPreciosCliente As String = String.Empty
        Dim CodigoListaPreciosSucursal As String = String.Empty
        Dim ListaPrecio As String = String.Empty
        Dim Query As String = String.Empty

        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)) IsNot Nothing Then
                UsaListaPreciosCliente = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_UseLisPreCli.Trim()
                CodigoListaPreciosSucursal = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_CodLisPre.Trim()
            End If

            If UsaListaPreciosCliente = "Y" Then
                Query = "SELECT T0.""ListNum"" FROM ""OCRD"" T0 WITH (nolock) WHERE T0.""CardCode"" = '{0}'"
                Query = String.Format(Query, CodigoCliente)
                ListaPrecio = DMS_Connector.Helpers.EjecutarConsulta(Query)
            Else
                ListaPrecio = CodigoListaPreciosSucursal
            End If

            Return ListaPrecio
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Recorre las líneas hijas de una lista de materiales y las agrega a la matriz de artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="oDataTable">DataTable con la lista de artículos</param>
    ''' <param name="CodigoArticuloPadre">Código del artículo padre o superior en la lista de materiales</param>
    ''' <param name="TipoPaquete">Tipo de paquete (Ventas, Modelo, ...)</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="UsaPrecioArticuloPadre">Configuración de SAP que indica si se muestra el precio del artículo padre o solamente el de los hijos</param>
    ''' <param name="ListaPrecios">Código de la lista de precios que se debe utilizar</param>
    ''' <remarks></remarks>
    Private Sub AgregarLineasHijas(ByRef oFormulario As SAPbouiCOM.Form, ByRef oDataTable As SAPbouiCOM.DataTable, ByVal CodigoArticuloPadre As String, ByVal TipoPaquete As String, ByVal Sucursal As String, ByVal UsaPrecioArticuloPadre As String, ByVal ListaPrecios As String)
        Dim UltimaLinea As Integer
        Dim TipoArticulo As String
        Dim ListaMateriales As SAPbobsCOM.ProductTrees
        Dim MaestroArticulo As SAPbobsCOM.Items
        Dim oItemPriceParams As SAPbobsCOM.ItemPriceParams
        Dim oItemPriceReturnParams As SAPbobsCOM.ItemPriceReturnParams
        Dim CodigoCliente As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(CodigoArticuloPadre) Then
                ListaMateriales = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees)
                MaestroArticulo = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
                oItemPriceParams = DMS_Connector.Company.CompanySBO.GetCompanyService.GetDataInterface(SAPbobsCOM.CompanyServiceDataInterfaces.csdiItemPriceParams)


                If ListaMateriales.GetByKey(CodigoArticuloPadre) Then
                    'Recorre toda la lista de materiales y agrega los artículos al DataTable
                    For i As Integer = 0 To ListaMateriales.Items.Count - 1

                        UltimaLinea = oDataTable.Rows.Count - 1
                        oDataTable.Rows.Add()
                        If Not String.IsNullOrEmpty(oDataTable.GetValue("codigo", UltimaLinea)) Then
                            UltimaLinea = oDataTable.Rows.Count - 1
                        End If

                        ListaMateriales.Items.SetCurrentLine(i)

                        If MaestroArticulo.GetByKey(ListaMateriales.Items.ItemCode) Then
                            oDataTable.SetValue("codigo", UltimaLinea, ListaMateriales.Items.ItemCode)
                            oDataTable.SetValue("descripcion", UltimaLinea, MaestroArticulo.ItemName)
                            oDataTable.SetValue("cantidad", UltimaLinea, ListaMateriales.Items.Quantity)
                            If UsaPrecioArticuloPadre = "Y" AndAlso TipoPaquete <> "T" Then
                                oDataTable.SetValue("moneda", UltimaLinea, ListaMateriales.Items.Currency)
                                oDataTable.SetValue("precio", UltimaLinea, 0)
                            Else
                                CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
                                oItemPriceParams.ItemCode = MaestroArticulo.ItemCode
                                oItemPriceParams.PriceList = ListaPrecios
                                oItemPriceReturnParams = DMS_Connector.Company.CompanySBO.GetCompanyService.GetDataInterface(SAPbobsCOM.CompanyServiceDataInterfaces.csdiItemPriceReturnParams)
                                oItemPriceReturnParams = DMS_Connector.Company.CompanySBO.GetCompanyService().GetItemPrice(oItemPriceParams)
                                oDataTable.SetValue("moneda", UltimaLinea, oItemPriceReturnParams.Currency)
                                oDataTable.SetValue("precio", UltimaLinea, oItemPriceReturnParams.Price)
                            End If
                            TipoArticulo = MaestroArticulo.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value
                            oDataTable.SetValue("tipo", UltimaLinea, TipoArticulo) 'U_SCGD_TipoArticulo
                            oDataTable.SetValue("duracion", UltimaLinea, MaestroArticulo.UserFields.Fields.Item("U_SCGD_Duracion").Value) 'U_SCGD_Duracion
                            oDataTable.SetValue("impuesto", UltimaLinea, ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo))
                            oDataTable.SetValue("hijo", UltimaLinea, "Y")
                            oDataTable.SetValue("padre", UltimaLinea, CodigoArticuloPadre)

                            If MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Then
                                oDataTable.SetValue("paquete", UltimaLinea, "S")
                            End If
                            If MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree Then
                                oDataTable.SetValue("paquete", UltimaLinea, "T")
                            End If

                            oDataTable.SetValue("barras", UltimaLinea, MaestroArticulo.BarCode)

                            If TipoArticulo = TiposArticulo.Paquete AndAlso (MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iSalesTree Or MaestroArticulo.TreeType = SAPbobsCOM.BoItemTreeTypes.iTemplateTree) Then
                                AgregarLineasHijas(oFormulario, oDataTable, ListaMateriales.Items.ItemCode, oDataTable.GetValue("paquete", UltimaLinea), Sucursal, UsaPrecioArticuloPadre, ListaPrecios)
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve la moneda local
    ''' </summary>
    ''' <returns>Moneda local en formato texto</returns>
    ''' <remarks></remarks>
    Private Function ObtenerMonedaLocal() As String
        Try
            DMS_Connector.Helpers.GetCurrencies(MonedaLocal, MonedaSistema)
            Return MonedaLocal
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Asigna los valores del impuesto seleccionado en el ChooseFromList impuesto
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="oDataTableImpuestos">DataTable obtenido en el ChooseFromList con los resultados</param>
    ''' <remarks></remarks>
    Private Sub AsignarValoresImpuesto(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef oDataTableImpuestos As SAPbouiCOM.DataTable)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oMatrix As SAPbouiCOM.Matrix
        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            oMatrix.FlushToDataSource()
            oDataTable.SetValue("impuesto", pVal.Row - 1, oDataTableImpuestos.GetValue("Code", 0))
            oMatrix.LoadFromDataSource()
            RecalcularTotales(oFormulario)
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
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "cboMoneda"
                        'Implementar funcionalidad aquí
                    Case "cboEstado"
                        'Implementar funcionalidad aquí
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "cboSucur"
                        ManejadorComboSucursal(oFormulario)
                    Case "cboAgenda"
                        ManejadorComboAgenda(oFormulario)
                    Case "cboMoneda"
                        If ValidarTipoCambioMoneda(oFormulario, pVal, BubbleEvent) Then
                            ManejadorComboMoneda(oFormulario, pVal, BubbleEvent)
                        End If
                    Case "cboTecnico"
                        ManejadorComboTecnico(oFormulario)
                    Case "cboEstado"
                        ManejadorComboEstadoCita(oFormulario)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function ValidarEstadoRequisiciones(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim Sucursal As String = String.Empty
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Dim Query As String = " SELECT COUNT(*) FROM ""@SCGD_REQUISICIONES"" T0 WHERE T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' AND T0.""U_SCGD_CodEst"" = '1' "
        Dim Cuenta As Integer = 0
        Dim UsaRequisicionReserva As String = String.Empty
        Dim EstadoDisparaCancelacion As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim OpcionSeleccionada As String = String.Empty
        Dim DocEntryCotizacion As String = String.Empty
        Try
            SerieCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Serie", 0).Trim
            NumeroCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_NumCita", 0).Trim

            If Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
                EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim

                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                    UsaRequisicionReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_UsePrepicking.Trim
                    EstadoDisparaCancelacion = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_PrepickingCS.Trim
                End If

                If UsaRequisicionReserva = "Y" AndAlso EstadoCita = EstadoDisparaCancelacion Then

                    DocEntryCotizacion = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim
                    Query = String.Format(Query, SerieCita, NumeroCita)
                    Cuenta = DMS_Connector.Helpers.EjecutarConsulta(Query)

                    If Cuenta > 0 Then
                        If Not ExistenRequisicionesParciales(SerieCita, NumeroCita) Then
                            OpcionSeleccionada = DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.CancelarRequisiciones, 2, My.Resources.Resource.Si, My.Resources.Resource.No)
                            If OpcionSeleccionada = "2" Then
                                Return False
                            Else
                                Return CancelarRequisicionesPendientes(DocEntryCotizacion, SerieCita, NumeroCita)
                            End If
                        Else
                            DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.RequisicionesParciales, 1, "OK")
                            Return False
                        End If
                    End If
                End If
            End If

            Return True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    Private Function ExistenRequisicionesParciales(ByVal SerieCita As String, ByVal NumeroCita As String) As Boolean
        Dim Query = " SELECT COUNT(*) AS 'Cuenta' FROM ""@SCGD_REQUISICIONES"" T0 INNER JOIN ""@SCGD_LINEAS_REQ"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" WHERE T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' AND T0.""U_SCGD_CodEst"" = '1' AND T1.""U_SCGD_CantRec"" > 0 "
        Dim Cuenta As Integer = 0
        Try
            Query = String.Format(Query, SerieCita, NumeroCita)
            Cuenta = DMS_Connector.Helpers.EjecutarConsulta(Query)

            If Cuenta > 0 Then
                Return True
            End If

            Return False
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return True
        End Try
    End Function

    ''' <summary>
    ''' Valida que la información de la moneda sea correcta
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se continua o no con el evento</param>
    ''' <returns>True = Moneda y datos válidos, False = Moneda o datos inválidos</returns>
    ''' <remarks></remarks>
    Private Function ValidarTipoCambioMoneda(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim MonedaSeleccionada As String = String.Empty
        Dim Fecha As Date
        Dim FechaCita As String = String.Empty
        Dim TipoCambio As Double = 0
        Dim Resultado As Boolean = True
        Try
            oComboBox = oFormulario.Items.Item("cboMoneda").Specific
            If oComboBox.Selected IsNot Nothing Then
                MonedaSeleccionada = oComboBox.Selected.Value
            End If

            If String.IsNullOrEmpty(MonedaSeleccionada) Then
                Resultado = False
                'Mostrar mensaje de error no se ha seleccionado una moneda válida
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorMonedaInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            Else
                'Buscar el tipo de cambio del acuerdo a la fecha de la cita
                FechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0)
                Fecha = Date.ParseExact(FechaCita, "yyyyMMdd", Nothing)
                TipoCambio = ObtenerTipoCambio(MonedaSeleccionada, Fecha)

                If TipoCambio <= 0 Then
                    Resultado = False
                    'Mostrar mensaje de error no se ha definido el tipo de cambio para la moneda seleccionada
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorTipoCambioMoneda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                End If
            End If
            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Manejador del ComboBox moneda
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorComboMoneda(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim oEditText As SAPbouiCOM.EditText
        Dim MonedaSeleccionada As String = String.Empty
        Dim Fecha As Date
        Dim FechaCita As String = String.Empty
        Dim TipoCambio As Double = 0
        Try
            oComboBox = oFormulario.Items.Item("cboMoneda").Specific
            oEditText = oFormulario.Items.Item("txtTipoC").Specific

            If oComboBox.Selected IsNot Nothing Then
                MonedaSeleccionada = oComboBox.Selected.Value
            End If

            'Buscar el tipo de cambio del acuerdo a la fecha de la cita
            FechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0)
            Fecha = Date.ParseExact(FechaCita, "yyyyMMdd", Nothing)
            TipoCambio = ObtenerTipoCambio(MonedaSeleccionada, Fecha)

            If MonedaSeleccionada = MonedaLocal Then
                'Deshabilitar el control tipo de cambio
                oFormulario.Items.Item("txtTipoC").Visible = False
                oEditText.Value = 1
            Else
                'Habilitar el control tipo de cambio
                oFormulario.Items.Item("txtTipoC").Visible = True
                oEditText.Value = TipoCambio.ToString(n)
            End If

            'Convertir los valores en el documento (Precios y Totales) a la nueva moneda
            'esto realizando la conversión de la moneda anterior hacia la nueva moneda
            ConvertirMontos(oFormulario, Fecha, MonedaSeleccionada, TipoCambio)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Convierte los montos de las líneas a la moneda seleccionada. Este método se utiliza desde otros formularios.
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks>El método fue creado para ser utilizado desde otros formularios que llaman por referencia al formulario de citas</remarks>
    Public Sub ConvertirMontosDesdeBusqueda(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim MonedaSeleccionada As String = String.Empty
        Dim Fecha As Date
        Dim FechaCita As String = String.Empty
        Dim TipoCambio As Double = 0
        Try
            oComboBox = oFormulario.Items.Item("cboMoneda").Specific

            If oComboBox.Selected IsNot Nothing Then
                MonedaSeleccionada = oComboBox.Selected.Value
            End If

            'Buscar el tipo de cambio del acuerdo a la fecha de la cita
            FechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0)
            Fecha = Date.ParseExact(FechaCita, "yyyyMMdd", Nothing)
            TipoCambio = ObtenerTipoCambio(MonedaSeleccionada, Fecha)

            'Convertir los valores en el documento (Precios y Totales) a la nueva moneda
            'esto realizando la conversión de la moneda anterior hacia la nueva moneda
            ConvertirMontos(oFormulario, Fecha, MonedaSeleccionada, TipoCambio)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Convierte todos los montos de los artículos de la matriz de artículos a la moneda seleccionada
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="Fecha"></param>
    ''' <param name="MonedaSeleccionada"></param>
    ''' <param name="TipoCambio"></param>
    ''' <remarks></remarks>
    Private Sub ConvertirMontos(ByRef oFormulario As SAPbouiCOM.Form, ByVal Fecha As Date, ByVal MonedaSeleccionada As String, ByVal TipoCambio As Double)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim MonedaLinea As String = String.Empty
        Dim Precio As Double = 0
        Dim PrecioMonedaLocal As Double = 0
        Dim oTiposCambio As Dictionary(Of String, Double)
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific

            oTiposCambio = New Dictionary(Of String, Double)
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")

            For i As Integer = 0 To oDataTable.Rows.Count - 1
                MonedaLinea = oDataTable.GetValue("moneda", i)
                Precio = oDataTable.GetValue("precio", i)

                If Not String.IsNullOrEmpty(MonedaLinea) Then
                    If Not oTiposCambio.ContainsKey(MonedaLinea) Then
                        oTiposCambio.Add(MonedaLinea, ObtenerTipoCambio(MonedaLinea, Fecha))
                    End If

                    If oTiposCambio.Item(MonedaLinea) > 0 Then
                        PrecioMonedaLocal = Precio * oTiposCambio.Item(MonedaLinea)
                        Precio = PrecioMonedaLocal / TipoCambio
                        'Asignamos la nueva moneda y precio a la línea
                        oDataTable.SetValue("moneda", i, MonedaSeleccionada)
                        oDataTable.SetValue("precio", i, Precio)
                    End If
                Else
                    oDataTable.SetValue("moneda", i, MonedaSeleccionada)
                End If
            Next

            oMatrix.LoadFromDataSource()
            RecalcularTotales(oFormulario)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del ComboBox estado cita
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejadorComboEstadoCita(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim CodigoEstado As String = String.Empty
        Try
            oComboBox = oFormulario.Items.Item("cboEstado").Specific
            CodigoEstado = oComboBox.Selected.Value

            If CodigoEstado = "3" Then
                If Not oFormulario.Mode = BoFormMode.fm_VIEW_MODE Then
                    oFormulario.Items.Item("cboMCanc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    oFormulario.Items.Item("txtCCan").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                End If
            Else
                oFormulario.Items.Item("cboMCanc").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oFormulario.Items.Item("txtCCan").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub



    Private Sub ProcesarRequisicionesReserva(ByRef oFormulario As SAPbouiCOM.Form, ByRef Cotizacion As SAPbobsCOM.Documents, ByVal NumeroSerieCita As String, ByVal ConsecutivoCita As String, ByRef ErrorProcesando As Boolean)
        Dim UsaRequisicionReserva As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim EstadoDisparaReserva As String = String.Empty
        Dim EstadoDisparaCancelacion As String = String.Empty
        Dim NumeroOT As String = String.Empty
        Try
            NumeroOT = Cotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
            If String.IsNullOrEmpty(NumeroOT) Then
                Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim
                EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim
                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                    UsaRequisicionReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_UsePrepicking.Trim
                    EstadoDisparaReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_PrepickingSS.Trim
                    EstadoDisparaCancelacion = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_PrepickingCS.Trim
                End If
                If UsaRequisicionReserva = "Y" Then
                    If Not String.IsNullOrEmpty(EstadoCita) AndAlso EstadoCita = EstadoDisparaReserva Then
                        ControladorRequisicionesReserva.ProcesarRequisicionReserva(oFormulario, Cotizacion, NumeroSerieCita, ConsecutivoCita, False, ErrorProcesando)
                    End If

                    If Not String.IsNullOrEmpty(EstadoCita) AndAlso EstadoCita = EstadoDisparaCancelacion Then
                        ControladorRequisicionesReserva.ProcesarRequisicionReserva(oFormulario, Cotizacion, NumeroSerieCita, ConsecutivoCita, True, ErrorProcesando)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub


    ''' <summary>
    ''' Manejador el ComboBox Tecnico
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejadorComboTecnico(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim CodigoTecnico As String = String.Empty
        Try
            oComboBox = oFormulario.Items.Item("cboTecnico").Specific

            If oComboBox.Selected IsNot Nothing Then
                CodigoTecnico = oComboBox.Selected.Value
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Tecnico", 0, oComboBox.Selected.Description)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve el tipo de cambio para la moneda y fecha indicada
    ''' </summary>
    ''' <param name="Moneda">Moneda en formato texto</param>
    ''' <param name="Fecha">Fecha del tipo de cambio</param>
    ''' <returns>Tipo de cambio en formato double</returns>
    ''' <remarks></remarks>
    Private Function ObtenerTipoCambio(ByVal Moneda As String, ByVal Fecha As Date) As Double
        Dim oSBObob As SAPbobsCOM.SBObob
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim TipoCambio As String = String.Empty
        Try
            If Moneda = MonedaLocal Then
                Return 1
            Else
                oSBObob = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge)
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset = oSBObob.GetCurrencyRate(Moneda, Fecha)
                TipoCambio = oRecordset.Fields.Item(0).Value.ToString()
                Return Double.Parse(TipoCambio)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Manejador del ComboBox sucursal
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejadorComboSucursal(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Sucursal As String = String.Empty
        Dim UsaGruposTrabajo As String = String.Empty
        Try
            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            'Limpiar los campos dependientes de la sucursal
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FechaCita", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraCita", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Asesor", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraServ", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FhaServ", 0, String.Empty)

            'Limpiar los ComboBoxes dependientes de la sucursal
            RemoverValidValuesComboBox(oFormulario, "cboRazon", "U_Cod_Razon")

            'Carga el ComboBox Agenda con los datos relacionados a la sucursal seleccionada
            Sucursal = oComboBox.Selected.Value
            CargarListaAgendas(oFormulario, Sucursal)

            'Verifica si se usan grupos de trabajo y Habilita/Deshabilita los controles según la configuración
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_GrpTrabajo.Trim
            End If

            If UsaGruposTrabajo = "Y" Then
                oFormulario.Items.Item("cboAsesor").Enabled = False
                oFormulario.Items.Item("txtFhaServ").Enabled = True
                oFormulario.Items.Item("txtHoraSer").Enabled = True
                oFormulario.Items.Item("cboTecnico").Enabled = True
            Else
                oFormulario.Items.Item("cboAsesor").Enabled = True
                oFormulario.Items.Item("txtFhaServ").Enabled = False
                oFormulario.Items.Item("txtHoraSer").Enabled = False
                oFormulario.Items.Item("cboTecnico").Enabled = False
            End If

            CargarListaAsesores(oFormulario, Sucursal)
            CargarListaTecnicos(oFormulario, Sucursal)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el listado de agendas activas de acuerdo a la sucursal
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="LimpiarValorSeleccionado">True = Borra el valor seleccionado en el ComboBox. False = Carga los nuevos valores válidos y no borra el valor que estaba seleccionado
    ''' sin importar si existe o no en los valores válidos</param>
    ''' <remarks></remarks>
    Private Sub CargarListaAgendas(ByRef oFormulario As SAPbouiCOM.Form, ByVal Sucursal As String, Optional ByVal LimpiarValorSeleccionado As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = "SELECT T0.""DocNum"", T0.""U_Agenda"" FROM ""@SCGD_AGENDA"" T0 WITH (nolock) WHERE T0.""U_Cod_Sucursal"" = '{0}' AND T0.""U_EstadoLogico"" = 'Y'"
        Dim oRecordset As SAPbobsCOM.Recordset

        Try
            RemoverValidValuesComboBox(oFormulario, "cboAgenda", "U_Cod_Agenda", LimpiarValorSeleccionado)

            oComboBox = oFormulario.Items.Item("cboAgenda").Specific
            'Agrega los valores válidos al ComboBox
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Query = String.Format(Query, Sucursal)
            oRecordset.DoQuery(Query)

            While Not oRecordset.EoF
                oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                oRecordset.MoveNext()
            End While
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del ComboBox Agenda
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejadorComboAgenda(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Sucursal As String = String.Empty

        Try
            oComboBox = oFormulario.Items.Item("cboAgenda").Specific

            'Limpiar los campos dependientes de la sucursal
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Asesor", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraServ", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FhaServ", 0, String.Empty)

            'Limpiar los campos dependientes de la agenda
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FechaCita", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraCita", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Asesor", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraServ", 0, String.Empty)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FhaServ", 0, String.Empty)

            'Una vez seleccionada la agenda, se carga la lista de razones
            CargarListaRazones(oFormulario)

            oComboBox = oFormulario.Items.Item("cboSucur").Specific

            If oComboBox.Selected IsNot Nothing Then
                Sucursal = oComboBox.Selected.Value
                CargarListaAsesores(oFormulario, Sucursal)
                CargarListaTecnicos(oFormulario, Sucursal)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el listado de razones en el ComboBox
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="LimpiarValorSeleccionado">True = Limpia el valor seleccionado. False = No borra el valor seleccionado sin importar
    ''' si existe en los valores válidos o no</param>
    ''' <remarks></remarks>
    Private Sub CargarListaRazones(ByRef oFormulario As SAPbouiCOM.Form, Optional ByVal LimpiarValorSeleccionado As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = " SELECT T0.""Code"", T0.""Name"" FROM ""@SCGD_RAZONCITA"" T0 WITH(nolock) ORDER BY T0.""Name"" ASC "
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            RemoverValidValuesComboBox(oFormulario, "cboRazon", "U_Cod_Razon", LimpiarValorSeleccionado)
            oComboBox = oFormulario.Items.Item("cboRazon").Specific
            'Agrega los valores válidos al ComboBox
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(Query)

            While Not oRecordset.EoF
                oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                oRecordset.MoveNext()
            End While
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina los valores válidos del ComboBox indicado
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="ItemUID">ID ünica del ComboBox (ItemUID)</param>
    ''' <param name="UDF">Nombre del UDF vinculado al ComboBox</param>
    ''' <param name="LimpiarCampo">True = Se limpia el valor actual del UDF. False = No se limpia el valor actual</param>
    ''' <remarks></remarks>
    Private Sub RemoverValidValuesComboBox(ByRef oFormulario As SAPbouiCOM.Form, ByVal ItemUID As String, ByVal UDF As String, Optional ByVal LimpiarCampo As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Try
            'Limpia el valor seleccionado para evitar que quede "huérfano"
            If LimpiarCampo Then
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue(UDF, 0, String.Empty)
            End If

            If Not String.IsNullOrEmpty(ItemUID) Then
                oComboBox = oFormulario.Items.Item(ItemUID).Specific
                If oComboBox IsNot Nothing AndAlso oComboBox.ValidValues.Count > 0 Then
                    For i As Integer = 0 To oComboBox.ValidValues.Count - 1
                        oComboBox.ValidValues.Remove(0, BoSearchKey.psk_Index)
                    Next
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el formulario desde el cual se ejecutó el evento
    ''' </summary>
    ''' <param name="FormUID">ID única de la instancia del formulario</param>
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
    ''' Manejador de los eventos ItemPressed
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario desde el cual se ejecuto el evento</param>
    ''' <param name="pVal">Objeto pVal con la información del evento</param>
    ''' <param name="BubbleEvent">Booleano que indica si se debe continuar procesando el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ItemPressed(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case "1"
                        If oFormulario.Mode = BoFormMode.fm_ADD_MODE Or oFormulario.Mode = BoFormMode.fm_UPDATE_MODE Then
                            If ValidarDatosCita(oFormulario, pVal, BubbleEvent) Then
                                ProcesarDocumento(oFormulario, pVal, BubbleEvent)
                            End If
                        End If
                    Case "btnAgenda"
                        ValidarDatosAgenda(oFormulario, BubbleEvent)
                    Case "btnAdd"
                        ValidarDatosAgregarAdicionales(oFormulario, BubbleEvent)
                    Case "cbx_Artic"
                        ManejadorCheckSinArticulos(oFormulario, pVal, BubbleEvent)
                    Case "btnLess"
                        ValidarDatosEliminarAdicionales(oFormulario, pVal, BubbleEvent)
                End Select
            Else
                Select Case pVal.ItemUID
                    Case "1"
                        FormDataAddAfter(oFormulario, pVal, BubbleEvent)
                    Case "btnAgenda"
                        AbrirAgenda(oFormulario)
                    Case "btnLess"
                        EliminarFilaSeleccionada(oFormulario)
                    Case "btnAdd"
                        AbrirVentanaSeleccionAdicionales(oFormulario)
                    Case "cbx_Artic"
                        ManejadorCheckSinArticulos(oFormulario, pVal, BubbleEvent)
                    Case "btLkUnid"
                        AbrirMaestroVehiculo(oFormulario)
                    Case "chkTiempo"
                        ManejadorCheckFormatoTiempo(oFormulario)
                    Case "btnOcupa"
                        ConstructorDisponibilidadEmpleados.CrearInstanciaFormulario()
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ValidarDatosEliminarAdicionales(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim DocEntryCotizacion As String = String.Empty
        Try
            DocEntryCotizacion = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim()
            ValidarRequisicionesReserva(oFormulario, pVal, BubbleEvent)
            If ExisteOrdenTrabajo(DocEntryCotizacion) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.BloqueoAgregarEliminarLineas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ValidarRequisicionesReserva(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim Query As String = " SELECT COUNT(*) AS 'Cuenta' FROM ""@SCGD_REQUISICIONES"" T0 WHERE T0.""U_SCGD_CodTipoReq"" = '3' AND T0.""U_SerieCita"" = '{0}' AND T0.""U_NumeroCita"" = '{1}' "
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Dim Cuenta As Integer = 0
        Dim UsaRequisicionReserva As String = String.Empty
        Dim Sucursal As String = String.Empty
        Try
            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaRequisicionReserva = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_UsePrepicking.Trim
            End If

            If UsaRequisicionReserva = "Y" Then
                SerieCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Serie", 0).Trim()
                NumeroCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_NumCita", 0).Trim()

                If Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                    Query = String.Format(Query, SerieCita, NumeroCita)
                    Cuenta = DMS_Connector.Helpers.EjecutarConsulta(Query)
                    If Cuenta > 0 Then
                        If EsLineaProcesada(oFormulario, pVal) Then
                            'No se pueden eliminar líneas, existen requisiciones de reserva ligadas a la cita
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorRequisicionesReserva, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function EsLineaProcesada(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent) As Boolean
        Dim LineaSeleccionada As Integer = -1
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oEditText As SAPbouiCOM.EditText
        Dim NumeroLineaCotizacion As String = String.Empty
        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oMatrix.FlushToDataSource()
            LineaSeleccionada = oMatrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)
            If LineaSeleccionada <> -1 Then
                oEditText = oMatrix.Columns.Item("Col_Linea").Cells.Item(LineaSeleccionada).Specific
                NumeroLineaCotizacion = oEditText.Value
                If Not String.IsNullOrEmpty(NumeroLineaCotizacion) Then
                    EsLineaProcesada = True
                End If
            Else
                EsLineaProcesada = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    Private Sub FormDataAddAfter(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.ActionSuccess = True AndAlso oFormulario.Mode = BoFormMode.fm_ADD_MODE Then
                CargarValoresPredeterminados(oFormulario)
                oFormulario.DataSources.UserDataSources.Item("marca").ValueEx = String.Empty
                oFormulario.DataSources.UserDataSources.Item("estilo").ValueEx = String.Empty
                oFormulario.DataSources.UserDataSources.Item("modelo").ValueEx = String.Empty
                oFormulario.DataSources.UserDataSources.Item("ano").ValueEx = String.Empty
                oFormulario.DataSources.UserDataSources.Item("combust").ValueEx = String.Empty
                oFormulario.DataSources.UserDataSources.Item("motor").ValueEx = String.Empty
                'Habilitar los controles número de serie y cita para búsquedas
                oFormulario.Items.Item("txtSerie").Enabled = False
                oFormulario.Items.Item("txtNoCita").Enabled = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Calcula las fechas de finalización de la cita y de los servicios de acuerdo a las configuraciones del sistema
    ''' y el tiempo estimado
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub CalcularFechaFinalizacion(ByRef oFormulario As SAPbouiCOM.Form)
        Dim CodigoSucursal As String = String.Empty
        Dim CodigoAgenda As String = String.Empty
        Dim TextoFechaCita As String = String.Empty
        Dim HoraCita As String = String.Empty
        Dim TextoFechaServicio As String = String.Empty
        Dim HoraServicio As String = String.Empty
        Dim QueryConfiguracionAgenda As String = " Select DocEntry, U_IntervaloCitas, U_TmpServ from [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}' "
        Dim UsaTiempoServicio As String = String.Empty
        Dim IntervaloCita As Integer = 15
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim UsaGruposTrabajo As String = String.Empty
        Dim FechaCita As DateTime
        Dim FechaServicio As DateTime
        Dim FechaCitaValida As Boolean = False
        Dim FechaServicioValida As Boolean = False
        Dim UsaFormatoHoras As String = String.Empty
        Dim TiempoEstimado As Double = 15

        Try
            CodigoSucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            CodigoAgenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()

            If Not String.IsNullOrEmpty(CodigoSucursal) AndAlso Not String.IsNullOrEmpty(CodigoAgenda) Then
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                QueryConfiguracionAgenda = String.Format(QueryConfiguracionAgenda, CodigoAgenda)
                oRecordset.DoQuery(QueryConfiguracionAgenda)

                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)) IsNot Nothing Then
                    UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_GrpTrabajo.Trim
                End If

                TextoFechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FechaCita", 0).Trim()
                HoraCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita", 0).Trim()
                TextoFechaServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FhaServ", 0).Trim()
                HoraServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraServ", 0).Trim()

                If Not String.IsNullOrEmpty(HoraCita) AndAlso HoraCita.Length = 3 Then
                    HoraCita = String.Format("0{0}", HoraCita)
                End If

                If Not String.IsNullOrEmpty(HoraServicio) AndAlso HoraServicio.Length = 3 Then
                    HoraServicio = String.Format("0{0}", HoraServicio)
                End If

                If Not String.IsNullOrEmpty(TextoFechaCita) AndAlso Not String.IsNullOrEmpty(HoraCita) Then
                    FechaCita = DateTime.ParseExact(TextoFechaCita + HoraCita, "yyyyMMddHHmm", Nothing)
                    FechaCitaValida = True
                End If

                If Not String.IsNullOrEmpty(TextoFechaServicio) AndAlso Not String.IsNullOrEmpty(HoraServicio) Then
                    FechaServicio = DateTime.ParseExact(TextoFechaServicio + HoraServicio, "yyyyMMddHHmm", Nothing)
                    FechaServicioValida = True
                End If

                TiempoEstimado = Double.Parse(oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx, n)
                If TiempoEstimado = 0 Then
                    TiempoEstimado = 15
                Else
                    UsaFormatoHoras = oFormulario.DataSources.UserDataSources.Item("tiemp").ValueEx
                    If UsaFormatoHoras = "Y" Then
                        TiempoEstimado = TiempoEstimado * 60
                    End If
                End If

                If FechaCitaValida Then
                    UsaTiempoServicio = oRecordset.Fields.Item("U_TmpServ").Value.ToString()
                    If UsaTiempoServicio = "Y" Then
                        SumarHorasNoLaborales(FechaCita, CodigoSucursal, TiempoEstimado)
                    Else
                        Integer.TryParse(oRecordset.Fields.Item("U_IntervaloCitas").Value.ToString(), IntervaloCita)
                        SumarHorasNoLaborales(FechaCita, CodigoSucursal, IntervaloCita)
                    End If
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FhaCita_Fin", 0, FechaCita.ToString("yyyyMMdd"))
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraCita_Fin", 0, FechaCita.ToString("HHmm"))
                End If

                If FechaServicioValida Then
                    If UsaGruposTrabajo = "Y" Then
                        SumarHorasNoLaborales(FechaServicio, CodigoSucursal, TiempoEstimado)
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FhaServ_Fin", 0, FechaServicio.ToString("yyyyMMdd"))
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraServ_Fin", 0, FechaServicio.ToString("HHmm"))
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Suma las horas no laborales a una fecha que abarca varios días o que esta fuera del horario de la sucursal
    ''' </summary>
    ''' <param name="Fecha">Fecha en la que se realiza la cita o servicio</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="TiempoEstimado">Tiempo estimado de la cita o de la actividad</param>
    ''' <remarks></remarks>
    Private Sub SumarHorasNoLaborales(ByRef Fecha As DateTime, ByVal CodigoSucursal As String, ByVal TiempoEstimado As Double)
        Dim HoraApertura As DateTime
        Dim HoraAperturaValida As Boolean = False
        Dim HoraCierre As DateTime
        Dim HoraCierreValida As Boolean = False
        Dim DuracionTallerAbierto As Integer = 0
        Dim TiempoPendiente As Integer = 0
        Try
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)) IsNot Nothing Then
                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraInicio IsNot Nothing Then
                    HoraApertura = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraInicio
                    HoraApertura = New DateTime(Fecha.Year, Fecha.Month, Fecha.Day, HoraApertura.Hour, HoraApertura.Minute, 0)
                    HoraAperturaValida = True
                End If
                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFin IsNot Nothing Then
                    HoraCierre = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_HoraFin
                    HoraCierre = New DateTime(Fecha.Year, Fecha.Month, Fecha.Day, HoraCierre.Hour, HoraCierre.Minute, 0)
                    HoraCierreValida = True
                End If
            End If

            If HoraAperturaValida AndAlso HoraCierreValida Then
                DuracionTallerAbierto = DateDiff(DateInterval.Minute, HoraApertura, HoraCierre)

                If Fecha.AddMinutes(TiempoEstimado) > HoraCierre Then
                    TiempoPendiente = (Fecha.AddMinutes(TiempoEstimado) - HoraCierre).TotalMinutes
                Else
                    Fecha = Fecha.AddMinutes(TiempoEstimado)
                End If

                If DuracionTallerAbierto > 0 Then
                    While TiempoPendiente > 0
                        If TiempoPendiente / DuracionTallerAbierto > 1 Then
                            If Fecha.AddDays(1).DayOfWeek = DayOfWeek.Saturday Then
                                Fecha = Fecha.AddDays(3)
                            Else
                                Fecha = Fecha.AddDays(1)
                            End If
                            TiempoPendiente = TiempoPendiente - DuracionTallerAbierto
                        Else
                            If Fecha.AddDays(1).DayOfWeek = DayOfWeek.Saturday Then
                                Fecha = Fecha.AddDays(3)
                            Else
                                Fecha = Fecha.AddDays(1)
                            End If
                            Fecha = New DateTime(Fecha.Year, Fecha.Month, Fecha.Day, HoraApertura.Hour, HoraApertura.Minute, 0)
                            Fecha = Fecha.AddMinutes(TiempoPendiente)
                            TiempoPendiente = 0
                        End If
                    End While
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Crea la oferta de ventas con la información de la cita
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar o no con el evento</param>
    ''' <remarks></remarks>
    Private Sub CrearDocumento(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim ErrorProcesando As Boolean = False
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim Sucursal As String = String.Empty
        Dim Asesor As String = String.Empty
        Dim SalesPerson As String = String.Empty
        Dim QuerySalesPerson As String = "SELECT TOP 1 ""salesPrson"" FROM OHEM WITH(nolock) WHERE ""empID"" = '{0}'"
        Dim Code As String = String.Empty
        Dim Year As String = String.Empty
        Dim NumeroPlaca As String = String.Empty
        Dim CodigoMarca As String = String.Empty
        Dim CodigoModelo As String = String.Empty
        Dim CodigoEstilo As String = String.Empty
        Dim DescripcionMarca As String = String.Empty
        Dim DescripcionModelo As String = String.Empty
        Dim DescripcionEstilo As String = String.Empty
        Dim NumeroVIN As String = String.Empty
        Dim NumeroUnidad As String = String.Empty
        Dim CodigoClienteOT As String = String.Empty
        Dim NombreClienteOT As String = String.Empty
        Dim NumeroSerie As String = String.Empty
        Dim Consecutivo As String = String.Empty
        Dim PoseeCampana As String = String.Empty
        Dim GarantiaVigente As String = String.Empty
        Dim IngresoPorGrua As String = String.Empty
        Dim FechaCita As String = String.Empty
        Dim HoraCita As String = String.Empty
        Dim oFecha As Date
        Dim GeneraAvaluo As String = String.Empty
        Dim CodigoAgenda As String = String.Empty
        Dim AutoKeyAvaluo As String = String.Empty
        Dim EsCitaSinArticulos As String = String.Empty
        Dim CodigoError As Integer
        Dim DescripcionError As String = String.Empty
        Dim NewObjectKey As String = String.Empty
        Dim MensajeCitaCreada As String = String.Empty
        Dim Kilometraje As String = String.Empty

        Try
            '------------------------------------------------
            'Parte 1 Completar la información del encabezado
            '------------------------------------------------
            oCotizacion = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            oCotizacion.CardCode = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
            oCotizacion.CardName = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardName", 0).Trim()
            oCotizacion.Comments = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Observ", 0).Trim()
            oCotizacion.DocCurrency = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Moneda", 0).Trim()

            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            CodigoAgenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()

            If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                If Not String.IsNullOrEmpty(Sucursal) Then
                    oCotizacion.BPL_IDAssignedToInvoice = Integer.Parse(Sucursal)
                End If
            End If

            Asesor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Asesor", 0).Trim()

            If Not String.IsNullOrEmpty(Asesor) Then
                oCotizacion.DocumentsOwner = Asesor
                QuerySalesPerson = String.Format(QuerySalesPerson, Asesor)
                SalesPerson = DMS_Connector.Helpers.EjecutarConsulta(QuerySalesPerson)
                If Not String.IsNullOrEmpty(SalesPerson) Then
                    oCotizacion.SalesPersonCode = SalesPerson
                End If
            End If

            NumeroUnidad = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Unid", 0).Trim()
            ObtenerDatosVehiculo(NumeroUnidad, Code, Year, NumeroPlaca, CodigoMarca, CodigoModelo, CodigoEstilo, DescripcionMarca, DescripcionModelo, DescripcionEstilo, NumeroVIN, Kilometraje)

            oCotizacion.Series = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_SerOfV.Trim
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Unidad").Value = NumeroUnidad
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Vehiculo").Value = Code
            oCotizacion.UserFields.Fields.Item("U_SCGD_Ano_Vehi").Value = Year
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_Placa").Value = NumeroPlaca
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Marca").Value = CodigoMarca
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Modelo").Value = CodigoModelo
            oCotizacion.UserFields.Fields.Item("U_SCGD_Cod_Estilo").Value = CodigoEstilo
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Marc").Value = DescripcionMarca
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Mode").Value = DescripcionModelo
            oCotizacion.UserFields.Fields.Item("U_SCGD_Des_Esti").Value = DescripcionEstilo
            oCotizacion.UserFields.Fields.Item("U_SCGD_Num_VIN").Value = NumeroVIN
            oCotizacion.UserFields.Fields.Item("U_SCGD_Kilometraje").Value = Kilometraje

            CodigoClienteOT = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CCliOT", 0).Trim()
            NombreClienteOT = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_NCliOT", 0).Trim()

            oCotizacion.UserFields.Fields.Item("U_SCGD_CCliOT").Value = CodigoClienteOT
            oCotizacion.UserFields.Fields.Item("U_SCGD_NCliOT").Value = NombreClienteOT
            oCotizacion.UserFields.Fields.Item("U_SCGD_idSucursal").Value = Sucursal

            GenerarNumeroCita(oFormulario, NumeroSerie, Consecutivo, ErrorProcesando)

            oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = NumeroSerie
            oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = Consecutivo

            PoseeCampana = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Campana", 0).Trim
            If Not String.IsNullOrEmpty(PoseeCampana) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Campana").Value = PoseeCampana
            End If

            GarantiaVigente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Garantia", 0).Trim
            If Not String.IsNullOrEmpty(GarantiaVigente) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Garantia").Value = GarantiaVigente
            End If

            IngresoPorGrua = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Towing", 0).Trim
            If Not String.IsNullOrEmpty(IngresoPorGrua) Then
                oCotizacion.UserFields.Fields.Item("U_SCGD_Towing").Value = IngresoPorGrua
            End If

            FechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FechaCita", 0).Trim
            HoraCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita", 0).Trim
            If HoraCita.Length = 3 Then HoraCita = "0" & HoraCita
            oFecha = New Date(CInt(FechaCita.Substring(0, 4)), CInt(FechaCita.Substring(4, 2)), CInt(FechaCita.Substring(6, 2)), CInt(HoraCita.Substring(0, 2)), CInt(HoraCita.Substring(2, 2)), 0)
            oCotizacion.UserFields.Fields.Item("U_SCGD_FechCita").Value = oFecha
            oCotizacion.UserFields.Fields.Item("U_SCGD_HoraCita").Value = oFecha

            GeneraAvaluo = Utilitarios.EjecutarConsulta(String.Format(" Select ISNULL(U_GenAva, 'N') as U_GenAva from [@SCGD_AGENDA] where DocEntry = '{0}' ", CodigoAgenda)).Trim()
            If GeneraAvaluo = "Y" Then
                AutoKeyAvaluo = DMS_Connector.Helpers.EjecutarConsulta("Select AutoKey from ONNM WITH (NOLOCK) where ObjectCode = 'SCGD_AVA'")
                If Not String.IsNullOrEmpty(AutoKeyAvaluo) Then
                    oCotizacion.UserFields.Fields.Item("U_SCGD_NoAvaU").Value = AutoKeyAvaluo
                End If
            End If

            '------------------------------------------------
            'Parte 2 Completar la información de las líneas
            '------------------------------------------------
            EsCitaSinArticulos = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_UsaArt", 0).Trim

            If EsCitaSinArticulos = "Y" Then
                'Parte 2A Cita sin artículos, se usa un artículo especial para las líneas de la cotización
                AsignarArticuloTipoCita(oFormulario, Sucursal, ErrorProcesando)
            End If

            'Parte 2B Cita con artículos, se recorre la matriz y se agregan las líneas al documento
            CrearLineasCotizacion(oFormulario, oCotizacion, Sucursal, ErrorProcesando)

            If ErrorProcesando = False Then
                'Iniciar la transacción
                DMS_Connector.Company.CompanySBO.StartTransaction()
                CodigoError = oCotizacion.Add()
                If CodigoError <> 0 Then
                    DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0}{1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    ErrorProcesando = True
                Else
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Num_Serie", 0, NumeroSerie)
                    oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_NumCita", 0, Consecutivo)
                    DMS_Connector.Company.CompanySBO.GetNewObjectCode(NewObjectKey)

                    If Not String.IsNullOrEmpty(NewObjectKey) Then
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Num_Cot", 0, NewObjectKey)

                        If oCotizacion.GetByKey(NewObjectKey) Then
                            ProcesarRequisicionesReserva(oFormulario, oCotizacion, NumeroSerie, Consecutivo, ErrorProcesando)
                            ActualizarInformacionLineasCreadas(oFormulario, oCotizacion, Sucursal, ErrorProcesando)
                            CrearAvaluo(oFormulario, oCotizacion.DocEntry, oCotizacion.DocNum, ErrorProcesando)
                        End If
                    End If
                End If

                If ErrorProcesando Then
                    BubbleEvent = False
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                Else
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        MensajeCitaCreada = String.Format("{0} {1}-{2}", My.Resources.Resource.MensajeCitaCreada, NumeroSerie, Consecutivo)
                        DMS_Connector.Company.ApplicationSBO.MessageBox(MensajeCitaCreada, 1, "OK")
                    End If
                End If
            Else
                BubbleEvent = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            BubbleEvent = False
            If DMS_Connector.Company.CompanySBO.InTransaction Then
                DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Procesa el documento, ya sea al crear, actualizar o en cualquier otro estado
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ProcesarDocumento(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case oFormulario.Mode
                Case BoFormMode.fm_ADD_MODE
                    CrearDocumento(oFormulario, pVal, BubbleEvent)
                Case BoFormMode.fm_UPDATE_MODE
                    If CotizacionPrevia <> CotizacionNueva Then
                        ActualizarCotizacion(oFormulario, pVal, BubbleEvent)
                        'ActualizarDocumento(oFormulario, pVal, BubbleEvent)
                        'oFormulario.Mode = BoFormMode.fm_VIEW_MODE
                        'CargarValoresPredeterminados(oFormulario)
                        CargarLineasMatriz(oFormulario)
                    Else
                        ActualizarDocumento(oFormulario, pVal, BubbleEvent)
                    End If

                Case BoFormMode.fm_OK_MODE
                    ManejadorEstadosFormulario(oFormulario)
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Maneja los distintos estados del formulario
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejadorEstadosFormulario(ByRef oFormulario As SAPbouiCOM.Form)
        Dim CodigoCitaCancelada As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim Sucursal As String = String.Empty
        Try
            CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(Sucursal)).U_CodCitaCancel.Trim
            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim()
            If EstadoCita = CodigoCitaCancelada Then
                oFormulario.Mode = BoFormMode.fm_VIEW_MODE
            Else
                oFormulario.Mode = BoFormMode.fm_OK_MODE
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ActualizarCotizacion(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim NumeroSerie As String = String.Empty
        Dim Consecutivo As String = String.Empty
        Dim CodigoError As Integer
        Dim DescripcionError As String = String.Empty
        Dim ErrorProcesando As Boolean = False
        Dim FechaCita As String = String.Empty
        Dim HoraCita As String = String.Empty
        Dim oFecha As DateTime = New DateTime

        Try
            oCotizacion = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            If oCotizacion.GetByKey(CotizacionNueva) Then
                '------------------------------------------------
                'Asigna los valores de la cita a la nueva Cotización
                '------------------------------------------------
                NumeroSerie = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Serie", 0).Trim
                Consecutivo = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_NumCita", 0).Trim
                FechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FechaCita", 0).Trim

                HoraCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita", 0).Trim
                If HoraCita.Length = 3 Then HoraCita = "0" & HoraCita
                oFecha = New Date(CInt(FechaCita.Substring(0, 4)), CInt(FechaCita.Substring(4, 2)), CInt(FechaCita.Substring(6, 2)), CInt(HoraCita.Substring(0, 2)), CInt(HoraCita.Substring(2, 2)), 0)
                oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = NumeroSerie
                oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = Consecutivo
                oCotizacion.UserFields.Fields.Item("U_SCGD_FechCita").Value = oFecha 'DateTime.ParseExact(FechaCita, "yyyyMMdd", Nothing)
                oCotizacion.UserFields.Fields.Item("U_SCGD_HoraCita").Value = oFecha 'HoraCita

                'Start Transaction
                DMS_Connector.Company.CompanySBO.StartTransaction()

                CodigoError = oCotizacion.Update()

                If CodigoError <> 0 Then
                    DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                    ErrorProcesando = True
                End If

                If ErrorProcesando Then
                    BubbleEvent = False
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                Else
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.CitaCambiaCotizacionNueva, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                End If

            End If

            If oCotizacion.GetByKey(CotizacionPrevia) And ErrorProcesando = False Then
                '------------------------------------------------
                'Desliga los valores de la cita de la cotización
                '------------------------------------------------
                oCotizacion.UserFields.Fields.Item("U_SCGD_NoSerieCita").Value = String.Empty
                oCotizacion.UserFields.Fields.Item("U_SCGD_NoCita").Value = String.Empty
                oCotizacion.UserFields.Fields.Item("U_SCGD_FechCita").Value = String.Empty
                oCotizacion.UserFields.Fields.Item("U_SCGD_HoraCita").Value = String.Empty

                DMS_Connector.Company.CompanySBO.StartTransaction()

                CodigoError = oCotizacion.Update()

                If CodigoError <> 0 Then
                    DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0}{1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    ErrorProcesando = True
                End If

                If ErrorProcesando Then
                    BubbleEvent = False
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                Else
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.CitaCambiaCotizacionPrevia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success)
                End If
            ElseIf ErrorProcesando Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0}{1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            End If



        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza la cotización ligada a la cita
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ActualizarDocumento(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim ErrorProcesando As Boolean = False
        Dim oCotizacion As SAPbobsCOM.Documents
        Dim DocEntryCotizacion As String = String.Empty
        Dim Comentarios As String = String.Empty
        Dim PoseeCampana As String = String.Empty
        Dim GarantiaVigente As String = String.Empty
        Dim IngresoPorGrua As String = String.Empty
        Dim Asesor As String = String.Empty
        Dim CodigoCitaCancelada As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim CodigoError As Integer
        Dim DescripcionError As String = String.Empty
        Dim NumeroSerie As String = String.Empty
        Dim Consecutivo As String = String.Empty
        Try
            oCotizacion = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

            DocEntryCotizacion = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim()

            If oCotizacion.GetByKey(DocEntryCotizacion) Then
                '------------------------------------------------
                'Parte 1 Actualizar la información del encabezado
                '------------------------------------------------
                Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
                EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim()
                Comentarios = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Observ", 0).Trim()
                If Comentarios.Length > 254 Then
                    Comentarios = Comentarios.Substring(0, 254)
                End If

                oCotizacion.Comments = Comentarios

                PoseeCampana = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Campana", 0).Trim
                If Not String.IsNullOrEmpty(PoseeCampana) Then
                    oCotizacion.UserFields.Fields.Item("U_SCGD_Campana").Value = PoseeCampana
                End If

                GarantiaVigente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Garantia", 0).Trim
                If Not String.IsNullOrEmpty(GarantiaVigente) Then
                    oCotizacion.UserFields.Fields.Item("U_SCGD_Garantia").Value = GarantiaVigente
                End If

                IngresoPorGrua = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Towing", 0).Trim
                If Not String.IsNullOrEmpty(IngresoPorGrua) Then
                    oCotizacion.UserFields.Fields.Item("U_SCGD_Towing").Value = IngresoPorGrua
                End If

                Asesor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Asesor", 0).Trim
                If Not String.IsNullOrEmpty(Asesor) Then
                    oCotizacion.DocumentsOwner = Asesor
                End If

                NumeroSerie = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Serie", 0).Trim
                Consecutivo = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_NumCita", 0).Trim

                '------------------------------------------------
                'Parte 2 Actualizar la información de las líneas
                '------------------------------------------------

                CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(Sucursal)).U_CodCitaCancel.Trim
                If Not String.IsNullOrEmpty(EstadoCita) AndAlso (EstadoCita <> CodigoCitaCancelada) Then
                    ActualizarLineasCotizacion(oFormulario, oCotizacion, ErrorProcesando)
                End If


            End If

            If ErrorProcesando = False Then
                'Iniciar la transacción
                DMS_Connector.Company.CompanySBO.StartTransaction()

                If Not String.IsNullOrEmpty(EstadoCita) AndAlso (EstadoCita = CodigoCitaCancelada) Then
                    ProcesarRequisicionesReserva(oFormulario, oCotizacion, NumeroSerie, Consecutivo, ErrorProcesando)
                    If Not ErrorProcesando Then
                        'ControladorRequisicionesReserva.RestablecerEstadosLineas(oCotizacion)
                        CodigoError = oCotizacion.Update()
                        If CodigoError = 0 Then
                            CodigoError = oCotizacion.Cancel()
                        End If
                    End If
                End If

                If EstadoCita <> CodigoCitaCancelada Then
                    ProcesarRequisicionesReserva(oFormulario, oCotizacion, NumeroSerie, Consecutivo, ErrorProcesando)
                    CodigoError = oCotizacion.Update()
                End If

                If CodigoError <> 0 Then
                    DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0}{1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    ErrorProcesando = True
                End If

                If ErrorProcesando Then
                    BubbleEvent = False
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If
                Else
                    If DMS_Connector.Company.CompanySBO.InTransaction Then
                        DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                        CargarLineasMatriz(oFormulario)
                        'If EstadoCita = CodigoCitaCancelada Then
                        '    oFormulario.Mode = BoFormMode.fm_VIEW_MODE
                        'End If
                    End If
                End If
            Else
                BubbleEvent = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            BubbleEvent = False
            If DMS_Connector.Company.CompanySBO.InTransaction Then
                DMS_Connector.Company.CompanySBO.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If
        End Try
    End Sub



    ''' <summary>
    ''' Actualiza las líneas de la cotización
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="oCotizacion">Objeto cotización con la información de la oferta de ventas</param>
    ''' <param name="ErrorProcesando">Variable donde se va a guardar si ocurrió algún error</param>
    ''' <remarks></remarks>
    Public Sub ActualizarLineasCotizacion(ByRef oFormulario As SAPbouiCOM.Form, ByRef oCotizacion As SAPbobsCOM.Documents, ByRef ErrorProcesando As Boolean)
        Dim oLines As SAPbobsCOM.Document_Lines
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim NumeroOT As String = String.Empty
        Dim NumeroLineaTexto As String = String.Empty
        Dim NumeroLinea As Integer
        Dim LineaCotizacion As Integer
        Dim EsLineaNueva As Boolean
        Dim ListaLineasExistentes As List(Of Integer)
        Dim ListaLineasNuevas As List(Of Integer)
        Dim ListaLineasCotizacion As List(Of Integer)
        Dim CodigoArticulo As String
        Dim Descripcion As String
        Dim Cantidad As Double
        Dim Moneda As String
        Dim Precio As Double
        Dim CodigoTecnico As String = String.Empty
        Dim NombreTecnico As String = String.Empty
        Dim EsHija As String = String.Empty
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim TipoArticulo As String = String.Empty
        Dim CodigoImpuesto As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim ExisteLinea As Boolean = False
        Try
            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oMatrix.FlushToDataSource()
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            NumeroOT = oCotizacion.UserFields.Fields.Item("U_SCGD_Numero_OT").Value
            oComboBox = oFormulario.Items.Item("cboTecnico").Specific

            If oComboBox.Selected IsNot Nothing Then
                CodigoTecnico = oComboBox.Selected.Value
                NombreTecnico = oComboBox.Selected.Description
            End If

            ListaLineasExistentes = New List(Of Integer)
            ListaLineasNuevas = New List(Of Integer)
            ListaLineasCotizacion = New List(Of Integer)

            oLines = oCotizacion.Lines

            For j As Integer = 0 To oDataTable.Rows.Count - 1
                ExisteLinea = False
                NumeroLineaTexto = oDataTable.GetValue("linea", j)
                CodigoArticulo = oDataTable.GetValue("codigo", j)

                For i As Integer = 0 To oLines.Count - 1
                    oLines.SetCurrentLine(i)
                    If Not String.IsNullOrEmpty(NumeroLineaTexto) AndAlso Integer.TryParse(NumeroLineaTexto, NumeroLinea) AndAlso NumeroLinea = oLines.LineNum Then
                        If Not String.IsNullOrEmpty(CodigoArticulo) AndAlso CodigoArticulo = oLines.ItemCode Then
                            ExisteLinea = True
                            ListaLineasExistentes.Add(oLines.LineNum)
                            Descripcion = oDataTable.GetValue("descripcion", j)
                            If String.IsNullOrEmpty(NumeroOT) Then
                                EsHija = oDataTable.GetValue("hijo", j)
                                Cantidad = oDataTable.GetValue("cantidad", j)
                                Moneda = oDataTable.GetValue("moneda", j)
                                Precio = oDataTable.GetValue("precio", j)


                                If Not EsHija = "Y" Then
                                    oLines.Quantity = Cantidad
                                    oLines.Currency = Moneda
                                    oLines.UnitPrice = Precio
                                End If

                                oLines.ItemDescription = Descripcion

                                If oLines.UserFields.Fields.Item("U_SCGD_TipArt").Value = "2" Then
                                    oLines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = CodigoTecnico
                                    oLines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = NombreTecnico
                                End If
                            Else
                                oLines.ItemDescription = Descripcion
                            End If
                        End If
                    End If
                Next

                If Not ExisteLinea And Not String.IsNullOrEmpty(CodigoArticulo) AndAlso Not ListaLineasNuevas.Contains(j) Then
                    ListaLineasNuevas.Add(j)
                End If
            Next

            'Paso 2 Eliminar las líneas eliminadas desde la cita
            If String.IsNullOrEmpty(NumeroOT) Then
                Dim Contador As Integer = oLines.Count - 1

                While Contador >= 0
                    oLines.SetCurrentLine(Contador)
                    If Not ListaLineasExistentes.Contains(oLines.LineNum) AndAlso oLines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient Then
                        oLines.Delete()
                    End If
                    Contador = Contador - 1
                End While

            End If

            'Paso 3 Agregar las líneas nuevas
            For Each Value As Integer In ListaLineasNuevas
                oLines.Add()
                EsHija = oDataTable.GetValue("hijo", Value)
                CodigoArticulo = oDataTable.GetValue("codigo", Value)
                Cantidad = oDataTable.GetValue("cantidad", Value)
                Moneda = oDataTable.GetValue("moneda", Value)
                Precio = oDataTable.GetValue("precio", Value)
                Descripcion = oDataTable.GetValue("descripcion", Value)
                TipoArticulo = oDataTable.GetValue("tipo", Value)
                CodigoImpuesto = oDataTable.GetValue("impuesto", Value)

                If Not EsHija = "Y" Then
                    oLines.ItemCode = CodigoArticulo
                    oLines.ItemDescription = Descripcion
                    oLines.Quantity = Cantidad
                    oLines.Currency = Moneda
                    oLines.UnitPrice = Precio
                    If Not String.IsNullOrEmpty(TipoArticulo) Then
                        oLines.UserFields.Fields.Item("U_SCGD_TipArt").Value = TipoArticulo
                    End If
                    If Not String.IsNullOrEmpty(Sucursal) Then
                        oLines.UserFields.Fields.Item("U_SCGD_Sucur").Value = Sucursal
                    End If
                    If TipoArticulo = "2" Then
                        oLines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = CodigoTecnico
                        oLines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = NombreTecnico
                        oLines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1"
                    End If

                    If Not String.IsNullOrEmpty(CodigoImpuesto) Then
                        oLines.TaxCode = CodigoImpuesto
                        oLines.VatGroup = CodigoImpuesto
                    Else
                        oLines.TaxCode = ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo)
                        oLines.VatGroup = ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo)
                    End If
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    ''' <summary>
    ''' Crea el documento de avaluo
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="DocEntryCotizacion">DocEntry de la cotización ligada a la cita</param>
    ''' <param name="DocNumCotizacion">DocNum de la cotización ligada a la cita</param>
    ''' <param name="ErrorProcesando">Variable donde se va a indicar si ocurrió un error o no</param>
    ''' <remarks></remarks>
    Private Sub CrearAvaluo(ByRef oFormulario As SAPbouiCOM.Form, ByVal DocEntryCotizacion As String, ByVal DocNumCotizacion As String, ByRef ErrorProcesando As Boolean)
        Dim oCompanyService As SAPbobsCOM.CompanyService
        Dim oGeneralService As SAPbobsCOM.GeneralService
        Dim oGeneralData As SAPbobsCOM.GeneralData
        Dim Query As String = "Select ISNULL(U_GenAva, 'N') as U_GenAva from [@SCGD_AGENDA] where DocEntry = '{0}'"
        Dim CodigoVehiculo As String = String.Empty
        Dim CodigoAgenda As String = String.Empty
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim GenerarAvaluo As String = String.Empty
        Dim CodigoUnidad As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim CodigoCliente As String = String.Empty
        Dim NombreCliente As String = String.Empty
        Dim CodigoTecnico As String = String.Empty
        Dim Moneda As String = String.Empty
        Try
            CodigoAgenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()
            Query = String.Format(Query, CodigoAgenda)
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

            GenerarAvaluo = DMS_Connector.Helpers.EjecutarConsulta(Query)

            If Not String.IsNullOrEmpty(GenerarAvaluo) AndAlso GenerarAvaluo = "Y" Then
                CodigoVehiculo = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CodVehi", 0).Trim()
                Query = "Select U_Cod_Unid, isnull (U_Cod_Marc, '') U_Cod_Marc, isnull (U_Cod_Esti, '') U_Cod_Esti, isnull (U_Cod_Mode, '') U_Cod_Mode, isnull (U_Num_Plac, '') U_Num_Plac, isnull (U_Ano_Vehi, '') U_Ano_Vehi, isnull (U_Num_VIN, '') U_Num_VIN, isnull (U_Combusti, '') U_Combusti, isnull (U_Cod_Col, '') U_Cod_Col, isnull (U_Transmis, '') U_Transmis, isnull (U_Km_Unid, '') U_Km_Unid from [@SCGD_VEHICULO] with(nolock) where Code='{0}' "
                Query = String.Format(Query, CodigoVehiculo)
                oRecordset.ExecuteQuery(Query)

                If oRecordset.RecordCount > 0 Then
                    CodigoUnidad = oRecordset.Fields.Item("U_Cod_Unid").Value.ToString()
                    If Not String.IsNullOrEmpty(CodigoUnidad) Then
                        oCompanyService = DMS_Connector.Company.CompanySBO.GetCompanyService()
                        oGeneralService = oCompanyService.GetGeneralService("SCGD_AVA")
                        oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)

                        Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
                        CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
                        NombreCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardName", 0).Trim()

                        oGeneralData.SetProperty("U_IdSucu", Sucursal)
                        oGeneralData.SetProperty("U_PropCed", CodigoCliente)
                        oGeneralData.SetProperty("U_PropNom", NombreCliente)
                        oGeneralData.SetProperty("U_VehCod", CodigoVehiculo)
                        oGeneralData.SetProperty("U_CodUnid", CodigoUnidad)
                        oGeneralData.SetProperty("U_CodMarc", oRecordset.Fields.Item("U_Cod_Marc").Value.ToString())
                        oGeneralData.SetProperty("U_CodMode", oRecordset.Fields.Item("U_Cod_Mode").Value.ToString())
                        oGeneralData.SetProperty("U_CodEsti", oRecordset.Fields.Item("U_Cod_Esti").Value.ToString())
                        oGeneralData.SetProperty("U_Placa", oRecordset.Fields.Item("U_Num_Plac").Value.ToString())
                        oGeneralData.SetProperty("U_VIN", oRecordset.Fields.Item("U_Num_VIN").Value.ToString())
                        oGeneralData.SetProperty("U_Ano", oRecordset.Fields.Item("U_Ano_Vehi").Value.ToString())
                        oGeneralData.SetProperty("U_Combusti", oRecordset.Fields.Item("U_Combusti").Value.ToString())

                        CodigoTecnico = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Tecnico", 0).Trim()

                        If Not String.IsNullOrEmpty(CodigoTecnico) AndAlso CodigoTecnico <> 0 Then
                            oGeneralData.SetProperty("U_TecCode", CodigoTecnico)
                        End If

                        oGeneralData.SetProperty("U_CodCol", oRecordset.Fields.Item("U_Cod_Col").Value.ToString())
                        oGeneralData.SetProperty("U_Km_Ing", oRecordset.Fields.Item("U_Km_Unid").Value.ToString())
                        oGeneralData.SetProperty("U_Transmis", oRecordset.Fields.Item("U_Transmis").Value.ToString())
                        oGeneralData.SetProperty("U_Estado", "1")
                        Moneda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Moneda", 0).Trim()
                        oGeneralData.SetProperty("U_Moneda", Moneda)
                        oGeneralData.SetProperty("U_CotID", DocEntryCotizacion)
                        oGeneralData.SetProperty("U_CotDocN", DocNumCotizacion)

                        oGeneralService.Add(oGeneralData)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    ''' <summary>
    ''' Actualiza información de las líneas de la cotización recien creada, ya que por limitaciones de SAP hay algunos procesos
    ''' que no se pueden realizar hasta después de creado.
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="oCotizacion">Objeto cotización recien creado</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="ErrorProcesando"></param>
    ''' <remarks>Algunos procesos como actualizar la descripción de líneas hijas solamente se pueden realizar
    ''' hasta que la cotización ha sido creada. Otras operaciones similares Post-Creación deben realizarse en este método</remarks>
    Private Sub ActualizarInformacionLineasCreadas(ByRef oFormulario As SAPbouiCOM.Form, ByRef oCotizacion As SAPbobsCOM.Documents, ByVal Sucursal As String, ByRef ErrorProcesando As Boolean)
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim CodigoTecnico As String = String.Empty
        Dim oItem As SAPbobsCOM.Items
        Dim EsHijo As String = String.Empty
        Dim TipoArticulo As String = String.Empty
        Dim CodigoImpuesto As String = String.Empty
        Dim CodigoError As String = String.Empty
        Dim DescripcionError As String = String.Empty
        Try
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            CodigoTecnico = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Tecnico", 0).Trim
            oItem = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)

            'Recorremos todas las líneas de la oferta de ventas recien creada y actualizamos datos faltantes
            'como los datos en las líneas hijas de listas de materiales entre otros
            For i As Integer = 0 To oCotizacion.Lines.Count - 1
                oCotizacion.Lines.SetCurrentLine(i)
                EsHijo = oDataTable.GetValue("hijo", i)

                If Not String.IsNullOrEmpty(EsHijo) AndAlso EsHijo = "Y" Then
                    If oCotizacion.Lines.ItemCode = oDataTable.GetValue("codigo", i) Then
                        oCotizacion.Lines.ItemDescription = oDataTable.GetValue("descripcion", i)
                    End If
                End If

                If oItem.GetByKey(oCotizacion.Lines.ItemCode) Then
                    If String.IsNullOrEmpty(CStr(oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value)) AndAlso CStr(oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value).Trim().Equals("2") Then
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = CodigoTecnico
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oItem.UserFields.Fields.Item("U_SCGD_Duracion").Value
                    End If
                    TipoArticulo = oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value
                    oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = TipoArticulo
                    CodigoImpuesto = String.Empty
                    If DMS_Connector.Configuracion.ParamGenAddon.U_LocCR = "Y" Then
                        If Not String.IsNullOrEmpty(oCotizacion.CardCode) And Not String.IsNullOrEmpty(oCotizacion.Lines.ItemCode) Then
                            CodigoImpuesto = DMS_Connector.Business_Logic.ImpuestoBL.ObtenerImpuesto(oFormulario, oCotizacion.CardCode, oCotizacion.Lines.ItemCode)
                        End If
                    End If
                    If String.IsNullOrEmpty(CodigoImpuesto) Then
                        If oCotizacion.Lines.TreeType <> SAPbobsCOM.BoItemTreeTypes.iIngredient OrElse DMS_Connector.Company.AdminInfo.DisplayPriceforPriceOnly = SAPbobsCOM.BoYesNoEnum.tNO Then
                            If DMS_Connector.Configuracion.ConfiguracionSucursales.Any(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) Then
                                With DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal))
                                    Select Case CStr(oItem.UserFields.Fields.Item("U_SCGD_TipoArticulo").Value).Trim()
                                        Case "1"
                                            CodigoImpuesto = .U_Imp_Repuestos.Trim()
                                        Case "2"
                                            CodigoImpuesto = .U_Imp_Serv.Trim()
                                        Case "3"
                                            CodigoImpuesto = .U_Imp_Suminis.Trim()
                                        Case "4"
                                            CodigoImpuesto = .U_Imp_ServExt.Trim()
                                        Case "11", "12"
                                            CodigoImpuesto = .U_Imp_Gastos.Trim()
                                    End Select
                                End With
                            End If
                        End If
                    End If

                    If Not String.IsNullOrEmpty(CodigoImpuesto) Then
                        oCotizacion.Lines.TaxCode = CodigoImpuesto
                        oCotizacion.Lines.VatGroup = CodigoImpuesto
                    End If
                End If
            Next

            CodigoError = oCotizacion.Update()

            If CodigoError <> 0 Then
                DescripcionError = DMS_Connector.Company.CompanySBO.GetLastErrorDescription()
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(String.Format("Error: {0} {1}", CodigoError, DescripcionError), BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                ErrorProcesando = True
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    ''' <summary>
    ''' Asigna el artículo tipo cita al documento
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="ErrorProcesando">Variable que indica si ocurrió un error o no</param>
    ''' <remarks></remarks>
    Private Sub AsignarArticuloTipoCita(ByRef oFormulario As SAPbouiCOM.Form, ByVal Sucursal As String, ByRef ErrorProcesando As Boolean)
        Dim Query As String = " SELECT SU.U_ArtCita, IT.ItemName, IT.U_SCGD_TipoArticulo FROM [@SCGD_CONF_SUCURSAL] SU with (nolock) INNER JOIN OITM IT ON IT.ItemCode = SU.U_ArtCita WHERE U_Sucurs = '{0}' "
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            Query = String.Format(Query, Sucursal)
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(Query)

            If oRecordset.RecordCount > 0 Then
                oMatrix = oFormulario.Items.Item("mtxArtic").Specific
                oMatrix.FlushToDataSource()
                oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
                oDataTable.Rows.Clear()

                If oDataTable.Rows.Count = 0 Then
                    oDataTable.Rows.Add()
                End If

                oDataTable.SetValue("codigo", 0, oRecordset.Fields.Item("U_ArtCita").Value.ToString())
                oDataTable.SetValue("descripcion", 0, oRecordset.Fields.Item("ItemName").Value.ToString())
                oDataTable.SetValue("cantidad", 0, 1)
                oDataTable.SetValue("tipo", 0, oRecordset.Fields.Item("U_SCGD_TipoArticulo").Value.ToString())
                oDataTable.SetValue("moneda", 0, String.Empty)
                oDataTable.SetValue("precio", 0, 0)
                oDataTable.SetValue("duracion", 0, 0)
                oDataTable.SetValue("impuesto", 0, String.Empty)
                oDataTable.SetValue("total", 0, 0)
                oDataTable.SetValue("hijo", 0, "N")

                oMatrix.LoadFromDataSource()
            Else
                ErrorProcesando = True
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorConfigurarArticuloCita, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    ''' <summary>
    ''' Agrega las líneas de la matriz artículos al objeto cotización que se está creando
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="oCotizacion">Objeto cotización</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="ErrorProcesando">Variable que indica si ocurrió un error o no</param>
    ''' <remarks></remarks>
    Private Sub CrearLineasCotizacion(ByRef oFormulario As SAPbouiCOM.Form, ByRef oCotizacion As SAPbobsCOM.Documents, ByVal Sucursal As String, ByRef ErrorProcesando As Boolean)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim CodigoTecnico As String = String.Empty
        Dim NombreTecnico As String = String.Empty
        Dim EsHijo As String = String.Empty
        Dim ArticuloPadre As String = String.Empty
        Dim CodigoArticulo As String = String.Empty
        Dim TipoArticulo As String = String.Empty
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim CodigoImpuesto As String = String.Empty
        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oMatrix.FlushToDataSource()
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            oComboBox = oFormulario.Items.Item("cboTecnico").Specific
            If oComboBox.Selected IsNot Nothing Then
                CodigoTecnico = oComboBox.Selected.Value
                NombreTecnico = oComboBox.Selected.Description
            End If

            For i As Integer = 0 To oDataTable.Rows.Count - 1
                EsHijo = oDataTable.GetValue("hijo", i).ToString.Trim
                ArticuloPadre = oDataTable.GetValue("padre", i).ToString.Trim
                CodigoArticulo = oDataTable.GetValue("codigo", i).ToString.Trim
                'Solamente se agrega el artículo padre, SAP se encarga del manejo de los paquetes
                If Not String.IsNullOrEmpty(CodigoArticulo) AndAlso Not EsHijo = "Y" Then
                    oCotizacion.Lines.ItemCode = oDataTable.GetValue("codigo", i).ToString.Trim
                    oCotizacion.Lines.ItemDescription = oDataTable.GetValue("descripcion", i).ToString.Trim
                    oCotizacion.Lines.Quantity = oDataTable.GetValue("cantidad", i)
                    oCotizacion.Lines.Currency = oDataTable.GetValue("moneda", i).ToString.Trim
                    oCotizacion.Lines.UnitPrice = oDataTable.GetValue("precio", i)
                    TipoArticulo = oDataTable.GetValue("tipo", i).ToString.Trim

                    If Not String.IsNullOrEmpty(TipoArticulo) Then
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_TipArt").Value = TipoArticulo
                    End If

                    If TipoArticulo = "2" Then
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EmpAsig").Value = CodigoTecnico
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_NombEmpleado").Value = NombreTecnico
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_DurSt").Value = oDataTable.GetValue("duracion", i)
                        oCotizacion.Lines.UserFields.Fields.Item("U_SCGD_EstAct").Value = "1"
                    End If

                    If Not String.IsNullOrEmpty(oDataTable.GetValue("impuesto", i)) Then
                        oCotizacion.Lines.TaxCode = oDataTable.GetValue("impuesto", i)
                        oCotizacion.Lines.VatGroup = oDataTable.GetValue("impuesto", i)
                    Else
                        oCotizacion.Lines.TaxCode = ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo)
                        oCotizacion.Lines.VatGroup = ObtenerImpuestoPorTipoArticulo(Sucursal, TipoArticulo)
                    End If

                    oCotizacion.Lines.Add()
                End If
            Next
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el impuesto que se debe utilizar de acuerdo a la configuración de impuestos de la sucursal
    ''' por tipo de artículo
    ''' </summary>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="TipoArticulo">Tipo de artículo (Repuesto, suministro, paquete, ...)</param>
    ''' <returns>Código del impuesto en formato texto</returns>
    ''' <remarks></remarks>
    Public Function ObtenerImpuestoPorTipoArticulo(ByVal Sucursal As String, ByVal TipoArticulo As String) As String
        Dim CodigoImpuesto As String = String.Empty
        Dim oTipoArticulo As TiposArticulo

        Try
            If Not String.IsNullOrEmpty(TipoArticulo) AndAlso DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                oTipoArticulo = TipoArticulo
                Select Case oTipoArticulo
                    Case TiposArticulo.Repuesto
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_Repuestos.Trim()
                    Case TiposArticulo.Paquete
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_Repuestos.Trim()
                    Case TiposArticulo.Servicio
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_Serv.Trim()
                    Case TiposArticulo.Suministro
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_Suminis.Trim()
                    Case TiposArticulo.ServicioExterno
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_ServExt.Trim()
                    Case TiposArticulo.OtrosCostos
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_Gastos.Trim()
                    Case TiposArticulo.OtrosIngresos
                        CodigoImpuesto = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_Imp_Gastos.Trim()
                End Select
            End If
            Return CodigoImpuesto
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Genera el número de cita con base en la abreviatura de la agenda y el número de consecutivo
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="NumeroSerie">Variable donde se va a guardar el número de serie (Abreviatura de la agenda)</param>
    ''' <param name="Consecutivo">Variable donde se va a guardar el número de consecutivo (Número)</param>
    ''' <param name="ErrorProcesando">Variable que indica si ocurrió un error o no</param>
    ''' <remarks></remarks>
    Private Sub GenerarNumeroCita(ByRef oFormulario As SAPbouiCOM.Form, ByRef NumeroSerie As String, ByRef Consecutivo As String, ByRef ErrorProcesando As Boolean)
        Dim QuerySerie As String = "SELECT U_Abreviatura FROM [@SCGD_AGENDA] with (nolock) WHERE DocEntry = {0}"
        Dim QueryConsecutivo As String = "SELECT TOP 1 U_NumCita FROM [@SCGD_CITA] with (nolock)  WHERE  U_Num_Serie = '{0}' order by DocNum DESC"
        Dim CodigoAgenda As String = String.Empty
        Dim Abreviatura As String = String.Empty
        Dim Year As String = String.Empty
        Dim Month As String = String.Empty
        Dim Fecha As Date
        Dim TextoFecha As String = String.Empty

        Try
            'Generar Numero de serie
            CodigoAgenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()

            If String.IsNullOrEmpty(CodigoAgenda) Then
                CodigoAgenda = "0"
            End If

            QuerySerie = String.Format(QuerySerie, CodigoAgenda)
            Abreviatura = DMS_Connector.Helpers.EjecutarConsulta(QuerySerie)

            TextoFecha = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0).Trim()
            Fecha = Date.ParseExact(TextoFecha, "yyyyMMdd", Nothing)
            Year = String.Format("{0:yy}", Fecha)
            Month = String.Format("{0:MM}", Fecha)
            NumeroSerie = String.Format("{0}{1}{2}", Abreviatura, Year, Month)

            If Not String.IsNullOrEmpty(NumeroSerie) Then
                'Generar Numero de Consecutivo
                QueryConsecutivo = String.Format(QueryConsecutivo, NumeroSerie)
                Consecutivo = DMS_Connector.Helpers.EjecutarConsulta(QueryConsecutivo)
                If Not String.IsNullOrEmpty(Consecutivo) Then
                    Consecutivo += 1
                Else
                    Consecutivo = 1
                End If

                Select Case Consecutivo.Length
                    Case 1
                        Consecutivo = "000" & Consecutivo
                    Case 2
                        Consecutivo = "00" & Consecutivo
                    Case 3
                        Consecutivo = "0" & Consecutivo
                End Select
            End If

            If String.IsNullOrEmpty(NumeroSerie) Or String.IsNullOrEmpty(Consecutivo) Then
                ErrorProcesando = True
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorGenerandoNumeroCita, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            ErrorProcesando = True
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la información del vehículo de acuerdo al código de la unidad (Campo "U_Cod_Unid" de la tabla de vehículos)
    ''' </summary>
    ''' <param name="NumeroUnidad">Número de unidad a buscar</param>
    ''' <param name="Code">Código interno</param>
    ''' <param name="Year">Año</param>
    ''' <param name="NumeroPlaca">Número de placa</param>
    ''' <param name="CodigoMarca">Código de la marca</param>
    ''' <param name="CodigoModelo">Código del modelo</param>
    ''' <param name="CodigoEstilo">Código del estilo</param>
    ''' <param name="DescripcionMarca">Descripción de la marca</param>
    ''' <param name="DescripcionModelo">Descripción del modelo</param>
    ''' <param name="DescripcionEstilo">Descripción del estilo</param>
    ''' <param name="NumeroVIN">Número de VIN</param>
    ''' <remarks></remarks>
    Private Sub ObtenerDatosVehiculo(ByVal NumeroUnidad As String, ByRef Code As String, ByRef Year As String, ByRef NumeroPlaca As String, ByRef CodigoMarca As String, ByRef CodigoModelo As String, ByRef CodigoEstilo As String, ByRef DescripcionMarca As String, ByRef DescripcionModelo As String, ByRef DescripcionEstilo As String, ByRef NumeroVIN As String, ByRef Kilometraje As String)
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim Query As String = "select Code, U_Cod_Marc, U_Des_Marc, U_Cod_Mode, U_Des_Mode, U_Cod_Esti, U_Des_Esti, U_Num_Plac, U_Num_VIN, U_Ano_Vehi, U_Km_Unid from [@SCGD_VEHICULO] with (nolock) where U_Cod_Unid = '{0}'"
        Try
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Query = String.Format(Query, NumeroUnidad)
            oRecordset.DoQuery(Query)

            If oRecordset.RecordCount > 0 Then
                Code = oRecordset.Fields.Item("Code").Value.ToString()
                Year = oRecordset.Fields.Item("U_Ano_Vehi").Value.ToString()
                NumeroPlaca = oRecordset.Fields.Item("U_Num_Plac").Value.ToString()
                CodigoMarca = oRecordset.Fields.Item("U_Cod_Marc").Value.ToString()
                CodigoModelo = oRecordset.Fields.Item("U_Cod_Mode").Value.ToString()
                CodigoEstilo = oRecordset.Fields.Item("U_Cod_Esti").Value.ToString()
                DescripcionMarca = oRecordset.Fields.Item("U_Des_Marc").Value.ToString()
                DescripcionModelo = oRecordset.Fields.Item("U_Des_Mode").Value.ToString()
                DescripcionEstilo = oRecordset.Fields.Item("U_Des_Esti").Value.ToString()
                NumeroVIN = oRecordset.Fields.Item("U_Num_VIN").Value.ToString()
                Kilometraje = oRecordset.Fields.Item("U_Km_Unid").Value.ToString()
                If String.IsNullOrEmpty(Kilometraje) Then
                    Kilometraje = "0"
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida que la información de la cita sea correcta antes de crear u actualizar el documento
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar o detener el evento</param>
    ''' <returns>True = Cita Válida. False = Uno o más datos son inválidos</returns>
    ''' <remarks></remarks>
    Private Function ValidarDatosCita(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) As Boolean
        Dim CitaValida As Boolean = True
        Try
            'Prioridad 0
            'Validar cita sin artículos
            If Not ValidarCitaSinArticulos(oFormulario) Then
                BubbleEvent = False
                Return False
            End If

            'Prioridad 1
            'Validar Periodo Contable
            'If Not PeriodoContableValido(oFormulario) Then
            '    BubbleEvent = False
            '    Return False
            'End If

            'Prioridad 2
            'Validar Datos Generales
            If Not ValidarDatosGenerales(oFormulario) Then
                BubbleEvent = False
                Return False
            End If

            'Prioridad 3
            'Validar Datos Servicio
            'Nota: Antes de realizar este proceso es necesario volver a calcular las fechas de finalización ya que son utilizadas
            'por la validación
            CalcularFechaFinalizacion(oFormulario)
            If Not ValidarDatosServicio(oFormulario) Then
                BubbleEvent = False
                Return False
            End If

            'Prioridad 4
            'Validar datos interfaz Ford

            'Prioridad 5
            'Validar requisiciones pendientes al cancelar cita
            If Not ValidarEstadoRequisiciones(oFormulario, pVal, BubbleEvent) Then
                BubbleEvent = False
                Return False
            End If

            Return CitaValida
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Valida si se debe crear la cita sin artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <returns>True = Crear cita sin artículos. False = No crear la cita y detener el evento</returns>
    ''' <remarks></remarks>
    Private Function ValidarCitaSinArticulos(ByRef oFormulario As SAPbouiCOM.Form) As Boolean
        Dim Valido As Boolean = True
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
            If oDataTable.IsEmpty() Or oDataTable.Rows.Count = 0 Or (oDataTable.Rows.Count = 1 AndAlso String.IsNullOrEmpty(oDataTable.GetValue("codigo", 0))) Then
                If DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.MsjCrearCitaSinArticulos, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    oCheckBox = oFormulario.Items.Item("cbx_Artic").Specific
                    oCheckBox.Checked = True
                Else
                    Return False
                End If
            End If
            Return Valido
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Valida los datos relacionados con el servicio (Fechas, horarios, choques, ...)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <returns>True = Datos válidos. False = Datos inválidos</returns>
    ''' <remarks></remarks>
    Private Function ValidarDatosServicio(ByRef oFormulario As SAPbouiCOM.Form) As Boolean
        Dim UsaGruposTrabajo As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim Agenda As String = String.Empty
        Dim CodigoAsesor As String = String.Empty
        Dim CodigoTecnico As String = String.Empty
        Dim FechaCita As DateTime
        Dim FechaFinCita As DateTime
        Dim FechaServicio As DateTime
        Dim FechaFinServicio As DateTime
        Dim FechaValida As Boolean = True
        Dim TextoFechaCita As String = String.Empty
        Dim HoraCita As String = String.Empty
        Dim TextoFechaFinCita As String = String.Empty
        Dim HoraFinCita As String = String.Empty
        Dim TextoFechaServicio As String = String.Empty
        Dim HoraServicio As String = String.Empty
        Dim TextoFechaFinServicio As String = String.Empty
        Dim HoraFinServicio As String = String.Empty
        Dim CitasPorDia As Dictionary(Of DayOfWeek, Integer)
        Dim IntervaloCitas As Integer = 15
        Dim HoraApertura As DateTime
        Dim HoraCierre As DateTime
        Dim TextoHoraApertura As String = String.Empty
        Dim TextoHoraCierre As String = String.Empty
        Dim UsaTiempoServicio As String = String.Empty
        Dim CodigoCitaCancelada As String = String.Empty
        Try
            CitasPorDia = New Dictionary(Of DayOfWeek, Integer)
            'Ajusta la hora a los intervalos de las citas
            ValidarYAjustarFormatoHoras(oFormulario)
            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            Agenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()
            CodigoTecnico = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Tecnico", 0).Trim()
            'Validaciones para los grupos de trabajo
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_GrpTrabajo.Trim
                TextoHoraApertura = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_HoraInicio.Value.ToString("HHmm")
                TextoHoraCierre = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_HoraFin.Value.ToString("HHmm")
                CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(Sucursal)).U_CodCitaCancel.Trim
            End If
            'Fechas de inicio
            TextoFechaCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FechaCita", 0).Trim()
            HoraCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita", 0).Trim()
            CompletarFormato24Hrs(HoraCita)
            TextoFechaServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FhaServ", 0).Trim()
            HoraServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraServ", 0).Trim()
            CompletarFormato24Hrs(HoraServicio)
            'Fechas de finalizacion
            TextoFechaFinCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FhaCita_Fin", 0).Trim()
            HoraFinCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita_Fin", 0).Trim()
            CompletarFormato24Hrs(HoraFinCita)
            TextoFechaFinServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FhaServ_Fin", 0).Trim()
            HoraFinServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraServ_Fin", 0).Trim()
            CompletarFormato24Hrs(HoraFinServicio)

            FechaCita = DateTime.ParseExact(TextoFechaCita + HoraCita, "yyyyMMddHHmm", Nothing)
            FechaFinCita = DateTime.ParseExact(TextoFechaFinCita + HoraFinCita, "yyyyMMddHHmm", Nothing)

            'Validar que la fecha de la cita no sea menor a la fecha actual
            If FechaCita < DateTime.Now AndAlso oFormulario.Mode = BoFormMode.fm_ADD_MODE Then
                'Mostrar mensaje de error, la hora de la cita debe ser mayor a la hora actual
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorHoraCita, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If UsaGruposTrabajo = "Y" AndAlso Not String.IsNullOrEmpty(TextoFechaServicio) AndAlso Not String.IsNullOrEmpty(HoraServicio) Then
                FechaServicio = DateTime.ParseExact(TextoFechaServicio + HoraServicio, "yyyyMMddHHmm", Nothing)
                FechaFinServicio = DateTime.ParseExact(TextoFechaFinServicio + HoraFinServicio, "yyyyMMddHHmm", Nothing)
                If (FechaServicio < DateTime.Now AndAlso oFormulario.Mode = BoFormMode.fm_ADD_MODE) Or (FechaServicio < FechaCita) Then
                    'Mensaje de error la fecha del servicio no puede ser menor a la fecha de la cita ni menor a la fecha actual
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorHoraServicio, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            'Validar que la fecha no este fuera del horario de la sucursal
            If Not String.IsNullOrEmpty(TextoHoraApertura) AndAlso Not String.IsNullOrEmpty(TextoHoraCierre) Then
                HoraApertura = DateTime.ParseExact(TextoHoraApertura, "HHmm", Nothing)
                HoraCierre = DateTime.ParseExact(TextoHoraCierre, "HHmm", Nothing)

                If Not FechaCita.TimeOfDay >= HoraApertura.TimeOfDay AndAlso Not FechaCita.TimeOfDay < HoraCierre.TimeOfDay Then
                    'Mensaje de error, la fecha de la cita esta fuera del horario de la sucursal
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorHoraCitaFueraHorario, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If

                If Not String.IsNullOrEmpty(TextoFechaServicio) AndAlso Not FechaServicio.TimeOfDay >= HoraApertura.TimeOfDay AndAlso Not FechaServicio.TimeOfDay < HoraCierre.TimeOfDay Then
                    'Mensaje de error, la fecha del servicio está fuera del horario de la sucursal
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorHoraServicioFueraHorario, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            'Obtiene las configuraciones de la agenda necesarias para las siguientes validaciones
            ObtenerDatosAgenda(Sucursal, Agenda, CitasPorDia, IntervaloCitas, UsaTiempoServicio)

            'Valida que la cantidad de citas asignadas no sea mayor a la cantidad máxima de citas para el día
            If oFormulario.Mode = BoFormMode.fm_ADD_MODE AndAlso Not ValidarCantidadCitasDisponibles(CitasPorDia, FechaCita, Sucursal, Agenda, CodigoCitaCancelada) Then
                'Mensaje de error no hay citas disponibles
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCantidadMaximaCitas, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If Not ValidarHoraInicioFinCita(oFormulario, FechaCita, FechaFinCita, Sucursal, Agenda, CodigoCitaCancelada) Then
                'Mensaje de error, hora de la cita no es valida ya que hay conflictos con las horas de otras citas
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorHoraInicioFinCita, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If UsaGruposTrabajo = "Y" AndAlso Not String.IsNullOrEmpty(TextoFechaServicio) AndAlso Not String.IsNullOrEmpty(HoraServicio) Then
                If Not ValidarHoraInicioFinServicio(oFormulario, FechaServicio, FechaFinServicio, Sucursal, Agenda, CodigoTecnico, CodigoCitaCancelada) Then
                    'Mensaje de error, hora de la cita no es valida ya que hay conflictos con las horas de otras citas
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorHoraInicioFinServicio, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            Return FechaValida
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Verifica que la hora cumpla con el formato HHmm y lo completa en caso de ser posible
    ''' </summary>
    ''' <param name="Hora">Hora en formato texto</param>
    ''' <remarks></remarks>
    Private Sub CompletarFormato24Hrs(ByRef Hora As String)
        Try
            If Not String.IsNullOrEmpty(Hora) AndAlso Hora.Length = 3 Then
                Hora = String.Format("0{0}", Hora)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida que la hora de inicio y la hora de fin de la cita no entren en conflicto con otras citas
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="FechaCita">Fecha de la cita</param>
    ''' <param name="FechaFinCita">Fecha de finalización de la cita</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="Agenda">Código de la agenda</param>
    ''' <param name="CodigoCitaCancelada">Código de cita cancelada</param>
    ''' <returns>True = Horas válidas. False = Existe un choque o conflicto.</returns>
    ''' <remarks></remarks>
    Private Function ValidarHoraInicioFinCita(ByRef oFormulario As SAPbouiCOM.Form, ByRef FechaCita As DateTime, ByRef FechaFinCita As DateTime, ByVal Sucursal As String, ByVal Agenda As String, ByVal CodigoCitaCancelada As String) As Boolean
        Dim HoraValida As Boolean = True
        Dim Query As String = " SELECT T0.""U_FechaCita"" AS 'FechaInicio', T0.""U_HoraCita"" AS 'HoraInicio', T0.""U_FhaCita_Fin"" AS 'FechaFin', T0.""U_HoraCita_Fin"" AS 'HoraFin' FROM ""@SCGD_CITA"" T0 WITH (nolock) WHERE T0.""U_Cod_Sucursal"" = '{0}' AND T0.""U_Cod_Agenda"" = '{1}' AND T0.""U_Estado"" <> '{2}' AND T0.""U_FechaCita"" IS NOT NULL AND T0.""U_FhaCita_Fin"" IS NOT NULL AND T0.""U_HoraCita"" IS NOT NULL AND T0.""U_HoraCita_Fin"" IS NOT NULL AND (('{3}' BETWEEN T0.""U_FechaCita"" AND T0.""U_FhaCita_Fin"") OR ('{4}' BETWEEN T0.""U_FechaCita"" AND T0.""U_FhaCita_Fin"")) "
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim FechaInicioConflicto As DateTime
        Dim FechaFinConflicto As DateTime
        Dim TextoHora As String = String.Empty
        Dim DocEntry As String = String.Empty
        Try
            DocEntry = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("DocEntry", 0).Trim
            Query = String.Format(Query, Sucursal, Agenda, CodigoCitaCancelada, FechaCita.ToString("yyyyMMdd"), FechaFinCita.ToString("yyyyMMdd"))
            If Not String.IsNullOrEmpty(DocEntry) Then
                Query += String.Format(" AND T0.DocEntry <> '{0}' ", DocEntry)
            End If

            oDataTable = oFormulario.DataSources.DataTables.Item("Fechas")
            oDataTable.ExecuteQuery(Query)

            If Not oDataTable.IsEmpty() Then
                For i As Integer = 0 To oDataTable.Rows.Count - 1
                    'Obtenemos los datos de la cita ya existente que queremos comparar
                    FechaInicioConflicto = oDataTable.GetValue("FechaInicio", i).ToString()
                    TextoHora = oDataTable.GetValue("HoraInicio", i)
                    If Not String.IsNullOrEmpty(TextoHora) AndAlso TextoHora.Length = 3 Then
                        TextoHora = String.Format("0{0}", TextoHora)
                    End If
                    FechaInicioConflicto = DateTime.ParseExact(FechaInicioConflicto.ToString("yyyyMMdd") + TextoHora, "yyyyMMddHHmm", Nothing)
                    FechaFinConflicto = oDataTable.GetValue("FechaFin", i).ToString()
                    TextoHora = oDataTable.GetValue("HoraFin", i)
                    If Not String.IsNullOrEmpty(TextoHora) AndAlso TextoHora.Length = 3 Then
                        TextoHora = String.Format("0{0}", TextoHora)
                    End If
                    FechaFinConflicto = DateTime.ParseExact(FechaFinConflicto.ToString("yyyyMMdd") + TextoHora, "yyyyMMddHHmm", Nothing)

                    If FechaCita >= FechaInicioConflicto AndAlso FechaCita <= FechaFinConflicto.AddMinutes(-1) Then
                        'Mensaje de error fecha de inicio en conflicto
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ConflictoHoraInicioCita, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If

                    If FechaFinCita.AddMinutes(-1) >= FechaInicioConflicto AndAlso FechaFinCita <= FechaFinConflicto Then
                        'Mensaje de error fecha de finalización en conflicto
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ConflictoHoraFinCita, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                Next
            End If

            Return HoraValida
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Valida que la hora de inicio y la hora de fin del servicio no entren en conflicto con otros servicios
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="FechaServicio">Fecha de inicio del servicio</param>
    ''' <param name="FechaFinServicio">Fecha de finalización del servicio</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="Agenda">Código de la agenda</param>
    ''' <param name="CodigoTecnico">Código del técnico</param>
    ''' <param name="CodigoCitaCancelada">Código de cita cancelada</param>
    ''' <returns>True = Hora Válida. False = Hora Inválida.</returns>
    ''' <remarks></remarks>
    Private Function ValidarHoraInicioFinServicio(ByRef oFormulario As SAPbouiCOM.Form, ByRef FechaServicio As DateTime, ByRef FechaFinServicio As DateTime, ByVal Sucursal As String, ByVal Agenda As String, ByVal CodigoTecnico As String, ByVal CodigoCitaCancelada As String) As Boolean
        Dim HoraValida As Boolean = True
        Dim Query As String = " SELECT T0.""U_FhaServ"" AS 'FechaInicio', T0.""U_HoraServ"" AS 'HoraInicio', T0.""U_FhaServ_Fin"" AS 'FechaFin', T0.""U_HoraServ_Fin"" AS 'HoraFin' FROM ""@SCGD_CITA"" T0 WITH (nolock) WHERE T0.""U_Cod_Sucursal"" = '{0}' AND T0.""U_Cod_Agenda"" = '{1}' AND T0.""U_Estado"" <> '{2}' AND T0.""U_FhaServ"" IS NOT NULL AND T0.""U_FhaServ_Fin"" IS NOT NULL AND T0.""U_HoraServ"" IS NOT NULL AND T0.""U_HoraServ_Fin"" IS NOT NULL AND (('{3}' BETWEEN T0.""U_FhaServ"" AND T0.""U_FhaServ_Fin"") OR ('{4}' BETWEEN T0.""U_FhaServ"" AND T0.""U_FhaServ_Fin"")) AND T0.""U_Cod_Tecnico"" = '{5}' "
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim FechaInicioConflicto As DateTime
        Dim FechaFinConflicto As DateTime
        Dim TextoHora As String = String.Empty
        Dim DocEntry As String = String.Empty
        Try
            DocEntry = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("DocEntry", 0).Trim
            Query = String.Format(Query, Sucursal, Agenda, CodigoCitaCancelada, FechaServicio.ToString("yyyyMMdd"), FechaFinServicio.ToString("yyyyMMdd"), CodigoTecnico)
            If Not String.IsNullOrEmpty(DocEntry) Then
                Query += String.Format(" AND T0.DocEntry <> '{0}' ", DocEntry)
            End If

            oDataTable = oFormulario.DataSources.DataTables.Item("Fechas")
            oDataTable.ExecuteQuery(Query)

            If Not oDataTable.IsEmpty() Then
                For i As Integer = 0 To oDataTable.Rows.Count - 1
                    'Obtenemos los datos de la cita ya existente que queremos comparar
                    FechaInicioConflicto = oDataTable.GetValue("FechaInicio", i).ToString()
                    TextoHora = oDataTable.GetValue("HoraInicio", i)
                    If Not String.IsNullOrEmpty(TextoHora) AndAlso TextoHora.Length = 3 Then
                        TextoHora = String.Format("0{0}", TextoHora)
                    End If
                    FechaInicioConflicto = DateTime.ParseExact(FechaInicioConflicto.ToString("yyyyMMdd") + TextoHora, "yyyyMMddHHmm", Nothing)
                    FechaFinConflicto = oDataTable.GetValue("FechaFin", i).ToString()
                    TextoHora = oDataTable.GetValue("HoraFin", i)
                    If Not String.IsNullOrEmpty(TextoHora) AndAlso TextoHora.Length = 3 Then
                        TextoHora = String.Format("0{0}", TextoHora)
                    End If
                    FechaFinConflicto = DateTime.ParseExact(FechaFinConflicto.ToString("yyyyMMdd") + TextoHora, "yyyyMMddHHmm", Nothing)

                    If FechaServicio >= FechaInicioConflicto AndAlso FechaServicio <= FechaFinConflicto.AddMinutes(-1) Then
                        'Mensaje de error fecha de inicio en conflicto
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ConflictoHoraInicioServicio, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If

                    If FechaFinServicio.AddMinutes(-1) >= FechaInicioConflicto AndAlso FechaFinServicio <= FechaFinConflicto Then
                        'Mensaje de error fecha de finalización en conflicto
                        DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ConflictoHoraFinServicio, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                Next
            End If

            Return HoraValida
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Valida que la cantidad de citas para el día no haya superado la cantidad máxima configurada en la agenda
    ''' </summary>
    ''' <param name="CitasPorDia">Objeto con la información de la cantidad de citas permitidas por día</param>
    ''' <param name="FechaCita">Fecha de la cita</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="Agenda">Código de la agenda</param>
    ''' <param name="CodigoCitaCancelada">Código de cita cancelada</param>
    ''' <returns>True = Citas Disponibles. False = Se ha superado la cantidad máxima permitida o no esta configurada correctamente</returns>
    ''' <remarks></remarks>
    Private Function ValidarCantidadCitasDisponibles(ByRef CitasPorDia As Dictionary(Of DayOfWeek, Integer), ByRef FechaCita As DateTime, ByVal Sucursal As String, ByVal Agenda As String, ByVal CodigoCitaCancelada As String) As Boolean
        Dim CitasDisponibles As Boolean = True
        Dim Query As String = " SELECT COUNT(*) FROM ""@SCGD_CITA"" T0 WHERE T0.""U_Cod_Agenda"" = '{0}' AND T0.""U_Cod_Sucursal"" = '{1}' AND T0.""U_FechaCita"" = '{2}' AND T0.""U_Estado"" <> '{3}' "
        Dim CantidadCitas As Integer = 0
        Try
            Query = String.Format(Query, Agenda, Sucursal, FechaCita.ToString("yyyyMMdd"), CodigoCitaCancelada)
            Integer.TryParse(DMS_Connector.Helpers.EjecutarConsulta(Query), CantidadCitas)

            If CantidadCitas >= CitasPorDia.Item(FechaCita.DayOfWeek) Then
                CitasDisponibles = False
            End If

            Return CitasDisponibles
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Obtiene los datos de la agenda
    ''' </summary>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="Agenda">Código de la agenda</param>
    ''' <param name="CitasPorDia">Objeto donde se va a guardar la cantidad de citas por día</param>
    ''' <param name="IntervaloCita">Variable donde se va a guardar el intervalo de tiempo configurado para cada cita</param>
    ''' <param name="UsaTiempoServicio">Variable donde se va a guardar si la configuración de tiempo de servicio</param>
    ''' <remarks></remarks>
    Private Sub ObtenerDatosAgenda(ByVal Sucursal As String, ByVal Agenda As String, ByRef CitasPorDia As Dictionary(Of DayOfWeek, Integer), ByRef IntervaloCita As Integer, ByRef UsaTiempoServicio As String)
        Dim Query As String = " SELECT U_Agenda, U_EstadoLogico, U_IntervaloCitas, U_Abreviatura, U_CodAsesor, U_CodTecnico, U_RazonCita, U_ArticuloCita, U_VisibleWeb, U_CantCLunes, U_CantCMartes, U_CantCMiercoles, U_CantCJueves, U_CantCViernes, U_CantCSabado, U_CantCDomingo, U_Num_Art, U_Num_Razon, U_Cod_Sucursal, U_NameAsesor, U_NameTecnico, U_TmpServ FROM [@SCGD_AGENDA] with (nolock) WHERE DocEntry = '{0}' AND U_Cod_Sucursal = '{1}' "
        Dim oRecordset As SAPbobsCOM.Recordset
        Try
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Query = String.Format(Query, Agenda, Sucursal)
            oRecordset.DoQuery(Query)

            If oRecordset.RecordCount > 0 Then
                Integer.TryParse(oRecordset.Fields.Item("U_IntervaloCitas").Value.ToString(), IntervaloCita)

                CitasPorDia.Add(DayOfWeek.Monday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCLunes").Value.ToString(), CitasPorDia.Item(DayOfWeek.Monday))
                CitasPorDia.Add(DayOfWeek.Tuesday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCMartes").Value.ToString(), CitasPorDia.Item(DayOfWeek.Tuesday))
                CitasPorDia.Add(DayOfWeek.Wednesday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCMiercoles").Value.ToString(), CitasPorDia.Item(DayOfWeek.Wednesday))
                CitasPorDia.Add(DayOfWeek.Thursday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCJueves").Value.ToString(), CitasPorDia.Item(DayOfWeek.Thursday))
                CitasPorDia.Add(DayOfWeek.Friday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCViernes").Value.ToString(), CitasPorDia.Item(DayOfWeek.Friday))
                CitasPorDia.Add(DayOfWeek.Saturday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCSabado").Value.ToString(), CitasPorDia.Item(DayOfWeek.Saturday))
                CitasPorDia.Add(DayOfWeek.Sunday, 0)
                Integer.TryParse(oRecordset.Fields.Item("U_CantCDomingo").Value.ToString(), CitasPorDia.Item(DayOfWeek.Sunday))

                UsaTiempoServicio = oRecordset.Fields.Item("U_TmpServ").Value.ToString()
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida los datos generales de la cita
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <returns>True = Datos válidos. False = Datos inválidos.</returns>
    ''' <remarks></remarks>
    Private Function ValidarDatosGenerales(ByRef oFormulario As SAPbouiCOM.Form) As Boolean
        Dim Resultado = True
        Dim Valor As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim CitaSinArticulo As String = String.Empty
        Dim CitaSinAsesor As String = String.Empty
        Dim QueryConfiguracionSucursal As String = " SELECT SU.U_ArtCita, IT.ItemName, IT.U_SCGD_TipoArticulo, U_HoraInicio, U_HoraFin, isnull(U_CitaSinAsesor,'N') U_CitaSinAsesor FROM [@SCGD_CONF_SUCURSAL] SU with (nolock) INNER JOIN OITM IT with (nolock) ON IT.ItemCode = SU.U_ArtCita WHERE U_Sucurs = '{0}' "
        Dim QueryOrdenTrabajo As String = " Select TOP 1 U_SCGD_Numero_OT from OQUT with (nolock) where DocEntry = '{0}' "
        Dim oRecordSet As SAPbobsCOM.Recordset
        Dim UsaGruposTrabajo As String = String.Empty
        Dim TecnicoObligatorio As String = String.Empty
        Dim HoraAperturaSucursal As String = String.Empty
        Dim HoraCierreSucursal As String = String.Empty
        Dim EstadoCita As String = String.Empty
        Dim CodigoCitaCancelada As String = String.Empty
        Dim NumeroOT As String = String.Empty
        Dim DocEntryCotizacion As String = String.Empty
        Dim FechaDocumento As DateTime
        Dim TextoFechaDocumento As String = String.Empty
        Dim FechaServicio As DateTime
        Dim TextoFechaServicio As String = String.Empty
        Dim TextoHoraServicio As String = String.Empty

        Try
            'Validaciones obligatorias
            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinCliente, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Unid", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinUnidad, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            If String.IsNullOrEmpty(Sucursal) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinSucursal, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAgenda, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Razon", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinRazon, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FechaCita", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraCita", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHora, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            'Validaciones dependientes de la configuración de la sucursal
            oRecordSet = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            QueryConfiguracionSucursal = String.Format(QueryConfiguracionSucursal, Sucursal)
            oRecordSet.DoQuery(QueryConfiguracionSucursal)

            'Verifica si el asesor esta vacio, lo cual es dependiente de la configuración de la sucursal
            Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Asesor", 0).Trim()
            If String.IsNullOrEmpty(Valor) Then
                CitaSinArticulo = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_UsaArt", 0).Trim()
                If CitaSinArticulo = "Y" Then
                    CitaSinAsesor = oRecordSet.Fields.Item("U_CitaSinAsesor").Value.ToString()
                Else
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAsesor, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If

                If CitaSinAsesor <> "Y" Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinAsesor, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            'Validaciones para los grupos de trabajo
            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_GrpTrabajo.Trim
                TecnicoObligatorio = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_MTechnician.Trim
            End If

            If UsaGruposTrabajo = "Y" AndAlso Not TecnicoObligatorio = "N" Then
                Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FhaServ", 0).Trim()
                If String.IsNullOrEmpty(Valor) Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinFecha, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If

                Valor = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_HoraServ", 0).Trim()
                If String.IsNullOrEmpty(Valor) Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHora, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            HoraAperturaSucursal = oRecordSet.Fields.Item("U_HoraInicio").Value.ToString()
            HoraCierreSucursal = oRecordSet.Fields.Item("U_HoraFin").Value.ToString()

            If String.IsNullOrEmpty(HoraAperturaSucursal) Or String.IsNullOrEmpty(HoraCierreSucursal) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaSinHoraInicioCierre, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            EstadoCita = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Estado", 0).Trim()
            CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(Sucursal)).U_CodCitaCancel.Trim

            If EstadoCita = CodigoCitaCancelada Then
                DocEntryCotizacion = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim()
                QueryOrdenTrabajo = String.Format(QueryOrdenTrabajo, DocEntryCotizacion)
                NumeroOT = DMS_Connector.Helpers.EjecutarConsulta(QueryOrdenTrabajo)
                If Not String.IsNullOrEmpty(NumeroOT) Then
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MensajeCitaLigadaConOrderTrabajo, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            TextoFechaDocumento = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0).Trim()

            If String.IsNullOrEmpty(TextoFechaDocumento) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaFechaInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            FechaDocumento = DateTime.ParseExact(TextoFechaDocumento, "yyyyMMdd", Nothing)

            TextoFechaServicio = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_FechaCita", 0).Trim()

            If String.IsNullOrEmpty(TextoFechaServicio) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaFechaInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            FechaServicio = DateTime.ParseExact(TextoFechaServicio, "yyyyMMdd", Nothing)

            If FechaDocumento > FechaServicio Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaFechaInvalida, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Valida que el periodo contable sea válido (Abierto)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <returns>True = Periodo Contable válido. False = El periodo contable esta bloquedao, cerrado o no configurado.</returns>
    ''' <remarks></remarks>
    Private Function PeriodoContableValido(ByRef oFormulario As SAPbouiCOM.Form) As Boolean
        Dim Resultado = True
        Dim Query As String = " SELECT T0.""PeriodStat"" FROM ""OFPR"" T0 WITH (nolock) WHERE '{0}' BETWEEN  T0.""F_RefDate"" AND T0.""T_RefDate"" "
        Dim TextoFecha As String = String.Empty
        Dim EstadoPeriodo As String = String.Empty

        Try
            TextoFecha = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("CreateDate", 0).Trim()
            Query = String.Format(Query, TextoFecha)
            EstadoPeriodo = DMS_Connector.Helpers.EjecutarConsulta(Query)

            Select Case EstadoPeriodo
                Case "C"
                    Resultado = False
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaPeriodoCerrado, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Case "Y"
                    Resultado = False
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaPeriodoBloqueado, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                Case String.Empty
                    Resultado = False
                    DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorCitaPeriodoNoConfig, SAPbouiCOM.BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
            End Select

            Return Resultado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Valida que se tengan los datos mínimos antes de abrir la ventana búsqueda de artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="BubbleEvent">Variable que indica si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ValidarDatosAgregarAdicionales(ByRef oFormulario As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Dim CodigoCliente As String = String.Empty
        Dim CodigoSucursal As String = String.Empty
        Dim NumeroUnidad As String = String.Empty
        Dim DocEntryCotizacion As String = String.Empty
        Try
            CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
            CodigoSucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            NumeroUnidad = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Unid", 0).Trim()
            DocEntryCotizacion = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Num_Cot", 0).Trim()
            If String.IsNullOrEmpty(CodigoCliente) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MsjIngreseCliente, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            End If

            If String.IsNullOrEmpty(CodigoSucursal) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.MsjSeleccioneUnaSucursal, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            End If

            If ExisteOrdenTrabajo(DocEntryCotizacion) Then
                DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.BloqueoAgregarEliminarLineas, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                BubbleEvent = False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Function ExisteOrdenTrabajo(ByVal DocEntry As String)
        Dim Query As String = "SELECT U_SCGD_Numero_OT FROM OQUT WITH (nolock) WHERE DocEntry = '{0}'"
        Dim NumeroOT As String = String.Empty
        Try
            ExisteOrdenTrabajo = False

            If String.IsNullOrEmpty(DocEntry) Then
                ExisteOrdenTrabajo = False
            Else
                Query = String.Format(Query, DocEntry)
                NumeroOT = DMS_Connector.Helpers.EjecutarConsulta(Query)
                If String.IsNullOrEmpty(NumeroOT) Then
                    ExisteOrdenTrabajo = False
                Else
                    ExisteOrdenTrabajo = True
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Manejador del check para el formato de tiempo (Horas o minutos)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejadorCheckFormatoTiempo(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oLabel As SAPbouiCOM.StaticText
        Dim UsaFormatoHoras As String = String.Empty
        Dim TiempoEstimado As Decimal = 0
        Dim TiempoEstimadoConvertido As Decimal = 0
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Try
            'oCheckBox = oFormulario.Items.Item("chkTiempo").Specific
            oLabel = oFormulario.Items.Item("89").Specific
            UsaFormatoHoras = oFormulario.DataSources.UserDataSources.Item("tiemp").ValueEx
            TiempoEstimado = If(String.IsNullOrEmpty(oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx), 0, Decimal.Parse(oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx, n))

            If UsaFormatoHoras = "Y" Then
                oLabel.Caption = My.Resources.Resource.Horas
                TiempoEstimadoConvertido = TiempoEstimado / 60
            Else
                oLabel.Caption = My.Resources.Resource.Minutos
                TiempoEstimadoConvertido = TiempoEstimado * 60
            End If
            TiempoEstimadoConvertido = Decimal.Round(TiempoEstimadoConvertido, 2)
            oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx = TiempoEstimadoConvertido.ToString(n)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Abre el formulario Maestro de vehículos con la unidad seleccionada en la cita
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub AbrirMaestroVehiculo(ByRef oFormulario As SAPbouiCOM.Form)
        Dim CodigoCliente As String = String.Empty
        Dim NumeroUnidad As String = String.Empty
        Dim CodigoInternoVehiculo As String = String.Empty
        Dim Query As String = " SELECT T0.""Code"" FROM ""@SCGD_VEHICULO"" T0 WITH (nolock) WHERE T0.""U_Cod_Unid"" = '{0}' "
        Dim oVehiculo As VehiculosCls

        Try
            CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim
            NumeroUnidad = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Unid", 0).Trim
            Query = String.Format(Query, NumeroUnidad)
            CodigoInternoVehiculo = DMS_Connector.Helpers.EjecutarConsulta(Query)

            If Not FormularioAbierto("SCGD_DET_1") Then
                oVehiculo = New VehiculosCls(DMS_Connector.Company.CompanySBO, DMS_Connector.Company.ApplicationSBO)
                VehiculosCls.blnDesdeCita = True
                VehiculosCls.blnDesdeCotizacion = False
                Call oVehiculo.DibujarFormularioDetalleInformacionVehiculo(CodigoCliente, CodigoInternoVehiculo, True, String.Empty, 0, False, False, VehiculosCls.ModoFormulario.scgTaller)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Verifica si el formulario está abierto
    ''' </summary>
    ''' <param name="FormUID">Unique ID de la instancia del formulario</param>
    ''' <returns>True = El formulario se encuentra abierto. False = El formulario no se encuentra abierto.</returns>
    ''' <remarks></remarks>
    Private Function FormularioAbierto(ByVal FormUID As String) As Boolean
        Try
            If ObtenerFormulario(FormUID) IsNot Nothing Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Function

    ''' <summary>
    ''' Manejador del Check cita sin artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="pVal">Objeto con la información del evento</param>
    ''' <param name="BubbleEvent">Variable para indicar si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ManejadorCheckSinArticulos(ByRef oFormulario As SAPbouiCOM.Form, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean)
        Dim CitaSinArticulos As String = String.Empty
        Dim oDataTable As SAPbouiCOM.DataTable
        Try
            CitaSinArticulos = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_UsaArt", 0)
            If pVal.BeforeAction Then
                oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")
                If Not CitaSinArticulos = "Y" AndAlso oDataTable.IsEmpty() Then
                    If DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.MsjCitaSinArticulos, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                        oDataTable.Rows.Clear()
                    Else
                        BubbleEvent = False
                    End If
                End If
            Else
                If CitaSinArticulos = "Y" Then
                    oFormulario.Items.Item("btnAdd").Enabled = False
                    oFormulario.Items.Item("btnLess").Enabled = False
                Else
                    oFormulario.Items.Item("btnAdd").Enabled = True
                    oFormulario.Items.Item("btnLess").Enabled = True
                End If
                RecalcularTotales(oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Abre una nueva instancia del buscador de adicionales (Formulario usado para buscar artículos y agregarlos a la cita)
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub AbrirVentanaSeleccionAdicionales(ByRef oFormulario As SAPbouiCOM.Form)
        Dim CodigoCliente As String = String.Empty
        Dim CodigoSucursal As String = String.Empty
        Dim CodigoInternoVehiculo As String = String.Empty
        Try
            CodigoCliente = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CardCode", 0).Trim()
            CodigoSucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim()
            CodigoInternoVehiculo = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_CodVehi", 0).Trim()
            ConstructorBusquedaArticulosCitas.CrearInstanciaFormulario(oFormulario.UniqueID, CodigoSucursal, CodigoCliente, CodigoInternoVehiculo)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Valida que se haya seleccionado una sucursal y agenda antes de abrir el formulario de búsqueda de artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="BubbleEvent">Variable para indicar si se debe continuar con el evento o no</param>
    ''' <remarks></remarks>
    Private Sub ValidarDatosAgenda(ByRef oFormulario As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim CodigoSucursal As String = String.Empty
        Dim CodigoAgenda As String = String.Empty
        Dim UsaGruposTrabajo As String = String.Empty
        Try
            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            If oComboBox.Selected IsNot Nothing Then
                CodigoSucursal = oComboBox.Selected.Value
            End If

            oComboBox = oFormulario.Items.Item("cboAgenda").Specific
            If oComboBox.Selected IsNot Nothing Then
                CodigoAgenda = oComboBox.Selected.Value
            End If

            If String.IsNullOrEmpty(CodigoSucursal) Then
                'Debe seleccionar una sucursal
                DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorSeleccionarSucursal, BoMessageTime.bmt_Short, False)
                BubbleEvent = False
            Else
                If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)) IsNot Nothing Then
                    UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(CodigoSucursal)).U_GrpTrabajo.Trim
                End If
            End If

            If Not UsaGruposTrabajo = "Y" Then
                If String.IsNullOrEmpty(CodigoAgenda) Then
                    'Debe seleccionar una Agenda
                    DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.ErrorSeleccionarAgenda, BoMessageTime.bmt_Short, False)
                    BubbleEvent = False
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Function GetForegroundWindow() As IntPtr
    End Function

    ''' <summary>
    ''' Abre la agenda o calendario en forma modal desde las citas, esto para permitir escoger la hora
    ''' de la cita en forma visual
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub AbrirAgenda(ByRef oFormulario As SAPbouiCOM.Form)
        Dim Fecha As Date = Date.Today
        Dim ptr As IntPtr
        Dim Wrapper As WindowWrapper
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim CodigoAgenda As String = String.Empty
        Dim DescripcionAgenda As String = String.Empty
        Dim CodigoSucursal As String = String.Empty
        Dim IntervaloCitas As Integer
        Dim CantidadEspaciosAsesor As Integer
        Dim CantidadEspaciosTecnico As Integer
        Dim TiempoEstimado As Integer
        Dim UsaColorAgenda As String = "N"
        Dim UsaGruposTrabajo As String = "N"
        Dim NumeroEquipo As String = String.Empty
        Dim CodigoCitaCancelada As String = String.Empty

        Try
            OpenFormUID = oFormulario.UniqueID
            ptr = GetForegroundWindow()
            Wrapper = New WindowWrapper(ptr)

            oComboBox = oFormulario.Items.Item("cboAgenda").Specific
            If oComboBox.Selected IsNot Nothing Then
                CodigoAgenda = oComboBox.Selected.Value
                DescripcionAgenda = oComboBox.Selected.Description
            End If

            oComboBox = oFormulario.Items.Item("cboSucur").Specific
            If oComboBox.Selected IsNot Nothing Then
                CodigoSucursal = oComboBox.Selected.Value
            End If

            'Consulta los intervalos entre cada cita
            IntervaloCitas = ObtenerIntervaloCitas(CodigoAgenda)
            CantidadEspaciosAsesor = ObtenerCantidadEspaciosAgenda(IntervaloCitas)

            TiempoEstimado = ObtenerTiempoEstimado(oFormulario)
            CantidadEspaciosTecnico = ObtenerCantidadEspaciosAgenda(TiempoEstimado)

            UsaColorAgenda = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(CodigoSucursal)).U_AgendaColor
            UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(CodigoSucursal)).U_GrpTrabajo
            CodigoCitaCancelada = DMS_Connector.Configuracion.ConfiguracionSucursales.First(Function(x) x.U_Sucurs.Equals(CodigoSucursal)).U_CodCitaCancel

            If UsaGruposTrabajo = "Y" Then
                NumeroEquipo = ObtenerNumeroDeEquipo(CodigoAgenda)
                'Abre una instancia del calendario horizontal por equipos, donde se puede seleccionar el asesor y técnico
                CalendarioPorEquipos = New frmListaCitas(Fecha, CodigoSucursal, CodigoAgenda, NumeroEquipo, True, TipoAgenda.Grupos, UsaVersionSAP9, CantidadEspaciosAsesor, CantidadEspaciosTecnico, UsaColorAgenda, DMS_Connector.Company.CompanySBO, DMS_Connector.Company.ApplicationSBO, String.Empty, True)
                CalendarioPorEquipos.ShowInTaskbar = False
                If UsaVersionSAP9 Then
                    IniciarTemporizador()
                    CalendarioPorEquipos.ShowDialog(Wrapper)
                    FinalizarTemporizador()
                Else
                    CalendarioPorEquipos.ShowDialog(Wrapper)
                End If
            Else
                If Not UsaColorAgenda = "Y" Then
                    'Abre una instancia del calendario vertical sin colores (El que solo muestra las fechas y horas)
                    Calendario = New frmCalendario(True, Fecha, DescripcionAgenda, CodigoAgenda, CodigoSucursal, CodigoCitaCancelada, UsaVersionSAP9, True, DMS_Connector.Company.CompanySBO, DMS_Connector.Company.ApplicationSBO)
                    Calendario.ShowInTaskbar = False

                    If UsaVersionSAP9 Then
                        IniciarTemporizador()
                        Calendario.ShowDialog(Wrapper)
                        FinalizarTemporizador()
                    Else
                        Calendario.ShowDialog(Wrapper)
                    End If
                Else
                    'Abre una instancia del calendario vertical con colores (El que solo muestra las fechas y horas)
                    CalendarioColor = New frmCalendarioColor(True, Fecha, DescripcionAgenda, CodigoAgenda, CodigoSucursal, CodigoCitaCancelada, UsaVersionSAP9, True, DMS_Connector.Company.CompanySBO, DMS_Connector.Company.ApplicationSBO)
                    CalendarioColor.ShowInTaskbar = False

                    If UsaVersionSAP9 Then
                        IniciarTemporizador()
                        CalendarioColor.ShowDialog(Wrapper)
                        FinalizarTemporizador()
                    Else
                        CalendarioColor.ShowDialog(Wrapper)
                    End If
                End If
            End If

            If oFormulario.Mode = BoFormMode.fm_OK_MODE Then
                oFormulario.Mode = BoFormMode.fm_UPDATE_MODE
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el tiempo estimado de los servicios para la cita
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <returns>Tiempo estimado en formato entero y en minutos</returns>
    ''' <remarks></remarks>
    Private Function ObtenerTiempoEstimado(ByRef oFormulario As SAPbouiCOM.Form) As Integer
        Dim TiempoEstimado As Integer
        Dim Valor As String = String.Empty
        Try
            Valor = oFormulario.DataSources.UserDataSources.Item("tiempo").ValueEx

            If String.IsNullOrEmpty(Valor) Then
                Valor = "15"
            End If

            TiempoEstimado = Double.Parse(Valor, n)

            If TiempoEstimado = 0 Then
                TiempoEstimado = 15
            End If

            Return TiempoEstimado
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Manejador del evento fecha seleccionada desde el calendario
    ''' </summary>
    ''' <param name="Fecha">Fecha seleccionada en el calendario</param>
    ''' <param name="NombreAgenda">Nombre de la agenda</param>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <remarks></remarks>
    Private Sub _Calendario_FechaSeleccionada(ByVal Fecha As Date, ByVal NombreAgenda As String, ByVal CodigoAgenda As Integer) Handles Calendario.eFechaYHoraSeleccionada
        Dim oFormulario As SAPbouiCOM.Form
        Dim oEditText As SAPbouiCOM.EditText

        Try
            oFormulario = ObtenerFormulario(OpenFormUID)
            Calendario.Close()
            Calendario = Nothing

            oEditText = oFormulario.Items.Item("txtFhaCita").Specific
            oEditText.Value = Fecha.ToString("yyyyMMdd")

            oEditText = oFormulario.Items.Item("txtHora").Specific
            oEditText.Value = String.Format("{0}{1}", Fecha.ToString("HH"), Fecha.ToString("mm"))
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador del evento fecha seleccionada desde el calendario color
    ''' </summary>
    ''' <param name="Fecha">Fecha seleccionada</param>
    ''' <param name="NombreAgenda">Nombre de la agenda</param>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <remarks></remarks>
    Private Sub _CalendarioColor_FechaSeleccionada(ByVal Fecha As Date, ByVal NombreAgenda As String, ByVal CodigoAgenda As Integer) Handles CalendarioColor.eFechaYHoraSeleccionadaColor
        Dim oFormulario As SAPbouiCOM.Form
        Dim oEditText As SAPbouiCOM.EditText

        Try
            oFormulario = ObtenerFormulario(OpenFormUID)
            CalendarioColor.Close()
            CalendarioColor = Nothing

            oEditText = oFormulario.Items.Item("txtFhaCita").Specific
            oEditText.Value = Fecha.ToString("yyyyMMdd")

            oEditText = oFormulario.Items.Item("txtHora").Specific
            oEditText.Value = String.Format("{0}{1}", Fecha.ToString("HH"), Fecha.ToString("mm"))
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Manejador de eventos Cita Nueva desd el calendario por equipos
    ''' </summary>
    ''' <param name="FechaAsesor">Fecha seleccionada para el asesor</param>
    ''' <param name="FechaTecnico">Fecha seleccionada para el técnico</param>
    ''' <param name="CodigoAsesor">Código del asesor</param>
    ''' <param name="CodigoTecnico">Código del técnico</param>
    ''' <param name="CodigoSucursal">Código de la sucursal</param>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <remarks></remarks>
    Private Sub _CalendarioPorEquipos_CitaNueva(ByVal FechaAsesor As Date, ByVal FechaTecnico As Date, ByVal CodigoAsesor As String, ByVal CodigoTecnico As String, ByVal CodigoSucursal As String, ByVal CodigoAgenda As String) Handles CalendarioPorEquipos.eCargaCitaNueva_PorEquipos
        Dim oFormulario As SAPbouiCOM.Form
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim RazonCita As String = String.Empty
        Dim Sucursal As String = String.Empty
        Dim Agenda As String = String.Empty

        Try
            'Obtiene el último formulario desde el cual se abrio la agenda en forma Modal
            oFormulario = ObtenerFormulario(OpenFormUID)

            CodigoAsesor = IIf(CodigoAsesor.Equals("-1"), String.Empty, CodigoAsesor)
            CodigoTecnico = IIf(CodigoTecnico.Equals("-1"), String.Empty, CodigoTecnico)

            Sucursal = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Sucursal", 0).Trim
            Agenda = oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").GetValue("U_Cod_Agenda", 0).Trim

            If CodigoSucursal <> Sucursal Then
                oComboBox = oFormulario.Items.Item("cboSucur").Specific
                oComboBox.Select(CodigoSucursal, BoSearchKey.psk_ByValue)
            End If

            If Agenda <> CodigoAgenda Then
                oComboBox = oFormulario.Items.Item("cboAgenda").Specific
                oComboBox.Select(CodigoAgenda, BoSearchKey.psk_ByValue)
            End If

            'Fecha y hora de la cita (Asesor)
            If Not FechaAsesor = Date.MinValue Then
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FechaCita", 0, FechaAsesor.ToString("yyyyMMdd"))
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraCita", 0, String.Format("{0}{1}", FechaAsesor.ToString("HH"), FechaAsesor.ToString("mm")))
            End If

            'Fecha y hora del servicio (Técnico)
            If Not FechaTecnico = Date.MinValue Then
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_FhaServ", 0, FechaTecnico.ToString("yyyyMMdd"))
                oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_HoraServ", 0, String.Format("{0}{1}", FechaTecnico.ToString("HH"), FechaTecnico.ToString("mm")))
            End If

            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Cod_Sucursal", 0, CodigoSucursal)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Cod_Agenda", 0, CodigoAgenda)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Cod_Asesor", 0, CodigoAsesor)
            oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Cod_Tecnico", 0, CodigoTecnico)

            CalendarioPorEquipos.Close()
            CalendarioPorEquipos = Nothing
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    'Private Sub _CalendarioPorEquipos__AsesorTecnicoSinCita(ByVal FechaAsesor As Date, ByVal FechaTecnico As Date, ByVal CodigoAsesor As String, ByVal CodigoTecnico As String, ByVal CodigoSucursal As String, ByVal CodigoAgenda As String) Handles CalendarioPorEquipos.eCargaCitaNueva_PorEquipos
    '    Try
    '        ConstructorCitas.CrearInstanciaFormulario(CodigoSucursal, CodigoAgenda, CodigoAsesor, FechaAsesor, CodigoTecnico, FechaTecnico)
    '    Catch ex As Exception
    '        DMS_Connector.Helpers.ManejoErrores(ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Carga la lista de los asesores
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="LimpiarValorSeleccionado">True = Limpia el valor actual del UDF. False = No limpia el valor actual.</param>
    ''' <remarks></remarks>
    Private Sub CargarListaAsesores(ByRef oFormulario As SAPbouiCOM.Form, ByVal Sucursal As String, Optional LimpiarValorSeleccionado As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim oComboBoxAgenda As SAPbouiCOM.ComboBox
        Dim oEditText As SAPbouiCOM.EditText
        Dim Query As String = String.Empty
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim UsaGruposTrabajo As String = String.Empty
        Dim CodigoAgenda As String = String.Empty

        Try
            oComboBox = oFormulario.Items.Item("cboAsesor").Specific
            oComboBoxAgenda = oFormulario.Items.Item("cboAgenda").Specific
            RemoverValidValuesComboBox(oFormulario, "cboAsesor", "U_Cod_Asesor", LimpiarValorSeleccionado)

            If oComboBoxAgenda.Selected IsNot Nothing Then
                CodigoAgenda = oComboBoxAgenda.Selected.Value
            End If

            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_GrpTrabajo.Trim
            End If

            If UsaGruposTrabajo = "Y" Then
                If (DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES) Then
                    Query = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where   (branch = '{0}' or  BPLId = '{0}') and U_SCGD_TipoEmp = 'A' order by HE.lastName"
                Else
                    Query = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where   branch = '{0}' and U_SCGD_TipoEmp = 'A' order by HE.lastName"
                End If
                Query = String.Format(Query, Sucursal)
                oComboBox.Item.Enabled = True
            Else
                Query = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE  with (nolock) where HE.empId = (SELECT Top 1 U_CodAsesor FROM [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}' AND U_EstadoLogico = 'Y')"
                Query = String.Format(Query, CodigoAgenda)
                oComboBox.Item.Enabled = False
            End If

            'Agrega los valores válidos al ComboBox
            oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordset.DoQuery(Query)

            While Not oRecordset.EoF
                oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                oRecordset.MoveNext()
            End While

            If LimpiarValorSeleccionado Then
                If oComboBox.ValidValues.Count > 0 Then
                    If Not String.IsNullOrEmpty(CodigoAgenda) AndAlso UsaGruposTrabajo = "Y" Then
                        Query = "SELECT U_CodAsesor, U_NameAsesor FROM [@SCGD_AGENDA] with (nolock) where DocEntry = '{0}' AND U_EstadoLogico = 'Y'"
                        Query = String.Format(Query, CodigoAgenda)
                        oRecordset.DoQuery(Query)
                        If oRecordset.RecordCount > 0 Then
                            oComboBox.Select(oRecordset.Fields.Item(0).Value.ToString(), BoSearchKey.psk_ByValue)
                        End If
                    ElseIf UsaGruposTrabajo <> "Y" Then
                        oComboBox.Select(0, BoSearchKey.psk_Index)
                    End If

                    If oComboBox.Selected IsNot Nothing Then
                        oFormulario.DataSources.DBDataSources.Item("@SCGD_CITA").SetValue("U_Name_Asesor", 0, oComboBox.Selected.Description)
                    End If
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la lista de los técnicos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <param name="Sucursal">Código de la sucursal</param>
    ''' <param name="LimpiarValorSeleccionado">True = Limpia el valor seleccionado. False = No limpia el valor seleccionado actualmente.</param>
    ''' <remarks></remarks>
    Private Sub CargarListaTecnicos(ByRef oFormulario As SAPbouiCOM.Form, ByVal Sucursal As String, Optional ByVal LimpiarValorSeleccionado As Boolean = True)
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim Query As String = String.Empty
        Dim oRecordset As SAPbobsCOM.Recordset
        Dim AgendaSeleccionada As String = String.Empty
        Dim UsaGruposTrabajo As String = String.Empty

        Try
            oComboBox = oFormulario.Items.Item("cboAgenda").Specific
            If oComboBox.Selected IsNot Nothing Then
                AgendaSeleccionada = oComboBox.Selected.Value
            End If

            oComboBox = oFormulario.Items.Item("cboTecnico").Specific

            RemoverValidValuesComboBox(oFormulario, "cboTecnico", "U_Cod_Tecnico", LimpiarValorSeleccionado)

            If DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)) IsNot Nothing Then
                UsaGruposTrabajo = DMS_Connector.Configuracion.ConfiguracionSucursales.FirstOrDefault(Function(suc) suc.U_Sucurs.Trim().Equals(Sucursal)).U_GrpTrabajo.Trim
            End If

            If UsaGruposTrabajo = "Y" Then
                If (DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES) Then
                    Query = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_T_Fase is not null and  (branch = '{0}' or  BPLId = '{0}') and U_SCGD_TipoEmp = 'T'"
                Else
                    Query = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_T_Fase is not null and branch = '{0}' and U_SCGD_TipoEmp = 'T'"
                End If

                Query = String.Format(Query, Sucursal)

                If Not String.IsNullOrEmpty(AgendaSeleccionada) AndAlso UsaGruposTrabajo = "Y" Then
                    Query = "Select HE.empId,  HE.lastName + ' ' + HE.firstName  from OHEM HE with (nolock) where U_SCGD_Equipo = '{0}' and HE.U_SCGD_TipoEmp = 'T'"
                    Query = String.Format(Query, ObtenerNumeroDeEquipo(AgendaSeleccionada))
                End If

                'Agrega los valores válidos al ComboBox
                oRecordset = DMS_Connector.Company.CompanySBO.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Query)

                While Not oRecordset.EoF
                    oComboBox.ValidValues.Add(oRecordset.Fields.Item(0).Value.ToString(), oRecordset.Fields.Item(1).Value.ToString())
                    oRecordset.MoveNext()
                End While
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicia una nueva instancia del temporizador limpiar la cola de mensajes de SAP cada cierto tiempo
    ''' esto previene que SAP genere un error debido a la espera, usado en el calendario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub IniciarTemporizador()
        Try
            oTimer = New System.Timers.Timer()
            AddHandler oTimer.Elapsed, New ElapsedEventHandler(AddressOf ManejadorTemporizador)
            oTimer.Interval = 30000
            oTimer.Enabled = True
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Finaliza el temporizador que se encarga de limpiar las colas de mensajes de SAP
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FinalizarTemporizador()
        Try
            oTimer.Enabled = False
            oTimer.Stop()
            oTimer.Dispose()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpia la cola de mensajes de SAP para evitar que se acumulen y provoquen un timeout así como otros errores en SAP
    ''' </summary>
    ''' <param name="sender">Objeto con la información del emisor</param>
    ''' <param name="e">Objeto con la información del evento</param>
    ''' <remarks></remarks>
    Private Sub ManejadorTemporizador(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs)
        Try
            DMS_Connector.Company.ApplicationSBO.RemoveWindowsMessage(SAPbouiCOM.BoWindowsMessageType.bo_WM_TIMER, True)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el número de equipo de la agenda seleccionada
    ''' </summary>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <returns>Número de equipo en formato texto</returns>
    ''' <remarks></remarks>
    Private Function ObtenerNumeroDeEquipo(ByVal CodigoAgenda As String) As String
        Dim Query As String = "SELECT HE.U_SCGD_Equipo from [@SCGD_AGENDA] AG with (nolock) inner Join  OHEM HE with (nolock) on AG.U_CodAsesor = HE.empID where DocEntry = '{0}'"
        Dim NumeroEquipo As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(CodigoAgenda) Then
                Query = String.Format(Query, CodigoAgenda)
                NumeroEquipo = DMS_Connector.Helpers.EjecutarConsulta(Query)
            End If

            If String.IsNullOrEmpty(NumeroEquipo) Or String.IsNullOrEmpty(CodigoAgenda) Then
                NumeroEquipo = "-1"
            End If

            Return NumeroEquipo

        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return "-1"
        End Try
    End Function

    ''' <summary>
    ''' Obtiene la cantidad de espacios que se deben marcar en la agenda de acuerdo al intervalo de la agenda
    ''' en las casillas del asesor
    ''' </summary>
    ''' <param name="Intervalo">Intervalo de la agenda en formato entero</param>
    ''' <returns>Cantidad de espacios en formato entero</returns>
    ''' <remarks></remarks>
    Private Function ObtenerCantidadEspaciosAgenda(ByVal Intervalo As Integer) As Integer
        Dim CantidadEspacios As Integer
        Try
            If (Intervalo Mod 15) <> 0 Then
                CantidadEspacios = (Math.Truncate(Intervalo / 15)) + 1
            Else
                CantidadEspacios = (Math.Truncate(Intervalo / 15))
            End If

            Return CantidadEspacios
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' Obtiene el intervalo de citas desde la agenda
    ''' </summary>
    ''' <param name="CodigoAgenda">Código de la agenda</param>
    ''' <returns>Intervalo en formato entero</returns>
    ''' <remarks></remarks>
    Private Function ObtenerIntervaloCitas(ByVal CodigoAgenda As String) As Integer
        Dim Query As String = "SELECT T0.""U_IntervaloCitas"" FROM ""@SCGD_AGENDA"" T0 WITH (nolock) WHERE T0.""DocEntry"" = '{0}'"
        Dim IntervaloCitas As Integer = 0
        Try
            If Not String.IsNullOrEmpty(CodigoAgenda) Then
                Query = String.Format(Query, CodigoAgenda)
                IntervaloCitas = DMS_Connector.Helpers.EjecutarConsulta(Query)
            End If

            If IntervaloCitas < 15 Then
                IntervaloCitas = 15
            End If

            Return IntervaloCitas
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
            Return 15
        End Try
    End Function

    ''' <summary>
    ''' Elimina la fila seleccionada de la matriz de artículos
    ''' </summary>
    ''' <param name="oFormulario">Instancia del formulario</param>
    ''' <remarks></remarks>
    Private Sub EliminarFilaSeleccionada(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim LineaSeleccionada As Integer = -1
        Dim LineaDataTable As Integer = -1
        Dim TipoPaquete As String = String.Empty
        Dim CodigoArticulo As String = String.Empty
        Dim CuentaLineasEliminadas As Integer = 0
        Dim oEditText As SAPbouiCOM.EditText
        Dim EsHijo As String = String.Empty
        Dim CodigoArticuloPadre As String = String.Empty
        Try
            oMatrix = oFormulario.Items.Item("mtxArtic").Specific
            oMatrix.FlushToDataSource()
            oDataTable = oFormulario.DataSources.DataTables.Item("listServicios")

            LineaSeleccionada = oMatrix.GetNextSelectedRow(0, BoOrderType.ot_RowOrder)

            oEditText = oMatrix.Columns.Item("Col_Paque").Cells.Item(LineaSeleccionada).Specific
            TipoPaquete = oEditText.Value
            If TipoPaquete = "S" Or TipoPaquete = "T" Then
                If DMS_Connector.Company.ApplicationSBO.MessageBox(My.Resources.Resource.MsjEliminarListaMateriales, 1, My.Resources.Resource.Si, My.Resources.Resource.No) = 1 Then
                    oEditText = oMatrix.Columns.Item("Col_Code").Cells.Item(LineaSeleccionada).Specific
                    CodigoArticulo = oEditText.Value
                    oMatrix.DeleteRow(LineaSeleccionada)

                    For i As Integer = LineaSeleccionada To oMatrix.RowCount
                        oEditText = oMatrix.Columns.Item("Col_Padre").Cells.Item(LineaSeleccionada).Specific
                        CodigoArticuloPadre = oEditText.Value
                        oEditText = oMatrix.Columns.Item("Col_Hijo").Cells.Item(LineaSeleccionada).Specific
                        EsHijo = oEditText.Value
                        If EsHijo = "Y" AndAlso CodigoArticuloPadre = CodigoArticulo Then
                            oMatrix.DeleteRow(LineaSeleccionada)
                        Else
                            Exit For
                        End If
                    Next
                End If
            Else
                oEditText = oMatrix.Columns.Item("Col_Hijo").Cells.Item(LineaSeleccionada).Specific
                EsHijo = oEditText.Value
                If EsHijo = "Y" Then
                    DMS_Connector.Company.ApplicationSBO.SetStatusBarMessage(My.Resources.Resource.MsjEliminarComponente, BoMessageTime.bmt_Short, True)
                Else
                    oMatrix.DeleteRow(LineaSeleccionada)
                End If
            End If

            oMatrix.FlushToDataSource()
            RecalcularTotales(oFormulario)
            If oFormulario.Mode = BoFormMode.fm_OK_MODE Then
                oFormulario.Mode = BoFormMode.fm_UPDATE_MODE
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        Finally
            ActualizarFormatoTabla(oMatrix, oDataTable)
        End Try
    End Sub

    ''' <summary>
    ''' Convierte el formato visual de la tabla
    ''' </summary>
    ''' <param name="oMatrix">Objeto matriz donde se van a mostrar los datos</param>
    ''' <param name="oDataTable">Tabla con la información de los artículos</param>
    ''' <remarks></remarks>
    Public Sub ActualizarFormatoTabla(ByRef oMatrix As SAPbouiCOM.Matrix, ByRef oDataTable As SAPbouiCOM.DataTable)
        Dim EsHijo As String = String.Empty
        Dim TipoArticulo As String = String.Empty
        Dim TipoPaquete As String = String.Empty
        Dim CodigoArticuloPadre As String = String.Empty

        Try
            If oMatrix IsNot Nothing AndAlso oDataTable IsNot Nothing Then
                For i As Integer = 0 To oDataTable.Rows.Count - 1
                    EsHijo = oDataTable.GetValue("hijo", i)
                    TipoArticulo = oDataTable.GetValue("tipo", i)
                    TipoPaquete = oDataTable.GetValue("paquete", i)
                    CodigoArticuloPadre = oDataTable.GetValue("padre", i)
                    If Not String.IsNullOrEmpty(EsHijo) AndAlso EsHijo = "Y" Then
                        oMatrix.CommonSetting.SetRowFontColor(i + 1, 8421504) 'Gris Oscuro
                    Else
                        If Not String.IsNullOrEmpty(TipoArticulo) AndAlso TipoArticulo = TiposArticulo.Paquete Then
                            oMatrix.CommonSetting.SetRowFontColor(i + 1, 128) 'Rojo Oscuro
                        Else
                            oMatrix.CommonSetting.SetRowFontColor(i + 1, 0) 'Color predeterminado
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

End Module
