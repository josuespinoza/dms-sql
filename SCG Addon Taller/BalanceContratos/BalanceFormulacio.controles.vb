
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'Manejo de controles de la pantalla de Balance de Contratos de ventas
'------ Inicializacion de controles en pantalla
'
'
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''Herencia de las librerias necesarias para el formulario
Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

Partial Public Class BalanceFormulario : Implements IFormularioSBO

#Region "Declaraciones"

    'maneja informacion de la aplicacion
    Private _applicationSbo As Application
    'maneja informacion de la compania 
    Private _companySbo As ICompany
    Private _formType As String
    Private _formularioSbo As IForm
    Private _inicializado As Boolean
    Private _nombreXml As String
    Private _titulo As String
    Private _strConexion As String

    'Campos EditText-Matrices-Botones de la pantalla
    'Encabezado
    Public EditTextClien As EditTextSBO
    Public EditTextTipoCl As EditTextSBO
    Public EditTextNumCt As EditTextSBO
    Public EditTextFechaCt As EditTextSBO
    Public EditTextCodeCliente As EditTextSBO
    Public EditTextVendedor As EditTextSBO
    Public EditTextMoneda As EditTextSBO
    Public EditTextEstado As EditTextSBO
    Public EditTextMonAse As EditTextSBO
    Public EditTextMonFin As EditTextSBO
    Public EditTextOtrCos As EditTextSBO
    Public CheckProyectado As CheckBoxSBO

    Public EditTextOtCPV As EditTextSBO
    Public EditTextMNeAs As EditTextSBO
    Public EditTextMNeFi As EditTextSBO
    Public EditTextDesc As EditTextSBO
    Public EditTextOtCU As EditTextSBO
    Public EditTextMAsU As EditTextSBO
    Public EditTextMFiU As EditTextSBO

    'Vehiculos
    Public EditTextValVeh As EditTextSBO
    Public EditTextCostVeh As EditTextSBO
    Public EditTextUtilVeh As EditTextSBO
    Public EditTextPUtilVeh As EditTextSBO
    Public EditTextBonoVeh As EditTextSBO
    Public EditTextPreLis As EditTextSBO
    'Accesorios
    Public EditTextValAcc As EditTextSBO
    Public EditTextCostAcc As EditTextSBO
    Public EditTextUtilAcc As EditTextSBO
    Public EditTextPUtilAcc As EditTextSBO
    'Tramites
    Public EditTextValTra As EditTextSBO
    Public EditTextCostTra As EditTextSBO
    Public EditTextUtilTra As EditTextSBO
    Public EditTextPUtilTra As EditTextSBO
    'Generales
    Public EditTextValG As EditTextSBO
    Public EditTextCostG As EditTextSBO
    Public EditTextUtilG As EditTextSBO
    Public EditTextPUtilG As EditTextSBO
    Public EditTextBonG As EditTextSBO
    'BotonOK
    Public Shared ButtonbtnOk As ButtonSBO
    'Matriz vehiculos
    Public MatrizVehiculos As MatrizVehiculos
    'Matriz accesorios
    Public MatrizAccesorios As MatrizAccesorios
    'Matriz tramites
    Public MatrizTramites As MatrizTramites
    'VARIABLES GENERALES
    Private _tGeneral As Decimal
    Private _cGeneral As Decimal
    Private _uGeneral As Decimal
    Private _bGeneral As Decimal
    'Los datatable 
    Private _dtValoresNuevos As DataTable
    Private _dtValoresAntiguos As DataTable
    Private _dtAccesoriosNuevos As DataTable
    Private _dtTramitesNuevos As DataTable
    'Valores originales
    Private Shared _precioVentaOriginalVeh() As Decimal
    Private Shared _costoOriginalVeh() As Decimal
    Private Shared _precioVentaOriginalAcc() As Decimal
    Private Shared _costoOriginalAcc() As Decimal
    Private Shared _precioVentaOriginalTra() As Decimal
    Private Shared _costoOriginalTra() As Decimal
    Private Shared _bonoOriginalVeh() As Decimal
    Private Shared _PreLisOriginalVeh() As Decimal
    Private Shared _DescOriginalVeh() As Decimal
    Private Shared _descuentoOriginalAcc() As Decimal
    Private Shared _precioListOriginalAcc() As Decimal

    Public Shared g_MontoFinanciera As Decimal
    Public Shared g_MontoAseguradora As Decimal
    Public Shared g_OtrosCostos As Decimal
    Public Shared g_Descuento As Decimal

#End Region

#Region "NEW"
    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany)
        'inicializa el objeto company y aplication
        _companySbo = companySbo
        _applicationSbo = application
    End Sub
#End Region

#Region "Propiedades"

    Public ReadOnly Property ApplicationSBO As SAPbouiCOM.IApplication Implements SCG.SBOFramework.UI.IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO As SAPbobsCOM.ICompany Implements SCG.SBOFramework.UI.IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    Public Property FormType As String Implements SCG.SBOFramework.UI.IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    'Propiedad Formulario
    Public Property FormularioSBO As SAPbouiCOM.IForm Implements SCG.SBOFramework.UI.IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As SAPbouiCOM.IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado As Boolean Implements SCG.SBOFramework.UI.IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    Public Property NombreXml As String Implements SCG.SBOFramework.UI.IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo As String Implements SCG.SBOFramework.UI.IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property StrConexion As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property TGeneral As Decimal
        Get
            Return _tGeneral
        End Get
        Set(ByVal value As Decimal)
            _tGeneral = value
        End Set
    End Property

    Public Property CGeneral As Decimal
        Get
            Return _cGeneral
        End Get
        Set(ByVal value As Decimal)
            _cGeneral = value
        End Set
    End Property

    Public Property UGeneral As Decimal
        Get
            Return _uGeneral
        End Get
        Set(ByVal value As Decimal)
            _uGeneral = value
        End Set
    End Property

    Public Property BGeneral As Decimal
        Get
            Return _bGeneral
        End Get
        Set(ByVal value As Decimal)
            _bGeneral = value
        End Set
    End Property

    Public Property dtValoresNuevos As DataTable
        Get
            Return _dtValoresNuevos
        End Get
        Set(ByVal value As DataTable)
            _dtValoresNuevos = value
        End Set
    End Property

    Public Property dtValoresAntiguos As DataTable
        Get
            Return _dtValoresAntiguos
        End Get
        Set(ByVal value As DataTable)
            _dtValoresAntiguos = value
        End Set
    End Property

    Public Property dtAccesoriosNuevos As DataTable
        Get
            Return _dtAccesoriosNuevos
        End Get
        Set(ByVal value As DataTable)
            _dtAccesoriosNuevos = value
        End Set
    End Property

    Public Property dtTramitesNuevos As DataTable
        Get
            Return _dtTramitesNuevos
        End Get
        Set(ByVal value As DataTable)
            _dtTramitesNuevos = value
        End Set
    End Property

    'manejo de valores para la funciond e actualizar
    Public Shared Property precioVentaOriginalVeh As Decimal()
        Get
            Return _precioVentaOriginalVeh
        End Get
        Set(ByVal value As Decimal())
            _precioVentaOriginalVeh = value
        End Set
    End Property

    Public Shared Property costoOriginalVeh As Decimal()
        Get
            Return _costoOriginalVeh
        End Get
        Set(ByVal value As Decimal())
            _costoOriginalVeh = value
        End Set
    End Property

    Public Shared Property precioVentaOriginalAcc As Decimal()
        Get
            Return _precioVentaOriginalAcc
        End Get
        Set(ByVal value As Decimal())
            _precioVentaOriginalAcc = value
        End Set
    End Property

    Public Shared Property DescuentoOriginalAcc As Decimal()
        Get
            Return _descuentoOriginalAcc
        End Get
        Set(ByVal value As Decimal())
            _descuentoOriginalAcc = value
        End Set
    End Property

    Public Shared Property precioListOriginalAcc As Decimal()
        Get
            Return _precioListOriginalAcc
        End Get
        Set(ByVal value As Decimal())
            _precioListOriginalAcc = value
        End Set
    End Property

    Public Shared Property precioVentaOriginalTra As Decimal()
        Get
            Return _precioVentaOriginalTra
        End Get
        Set(ByVal value As Decimal())
            _precioVentaOriginalTra = value
        End Set
    End Property

    Public Shared Property costoOriginalAcc As Decimal()
        Get
            Return _costoOriginalAcc
        End Get
        Set(ByVal value As Decimal())
            _costoOriginalAcc = value
        End Set
    End Property

    Public Shared Property costoOriginalTra As Decimal()
        Get
            Return _costoOriginalTra
        End Get
        Set(ByVal value As Decimal())
            _costoOriginalTra = value
        End Set
    End Property

    Public Shared Property bonoOriginalVeh As Decimal()
        Get
            Return _bonoOriginalVeh
        End Get
        Set(ByVal value As Decimal())
            _bonoOriginalVeh = value
        End Set
    End Property

    Public Shared Property PreLisOriginalVeh As Decimal()
        Get
            Return _PreLisOriginalVeh
        End Get
        Set(ByVal value As Decimal())
            _PreLisOriginalVeh = value
        End Set
    End Property

    Public Shared Property DescOriginalVeh As Decimal()
        Get
            Return _DescOriginalVeh
        End Get
        Set(ByVal value As Decimal())
            _DescOriginalVeh = value
        End Set
    End Property

#End Region

#Region "Metodos"

#Region "Metodos de interfaz"

    'NO IMPLEMENTADO
    Public Sub InicializaFormulario() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then

        End If

    End Sub

    'Inicializa los controles de la pantalla 
    Public Sub InicializarControles() Implements SCG.SBOFramework.UI.IFormularioSBO.InicializarControles
        'Manejo de formulario
        FormularioSBO.Freeze(True)

        'Conexion a los componentes que NO se encuentran en la matriz - Los EditText
        Dim userDS As UserDataSources = FormularioSBO.DataSources.UserDataSources
        'agrega columnas al ds
        userDS.Add("cliente", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("tipo", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("numero", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("fecha", BoDataType.dt_DATE, 100)
        userDS.Add("codigo", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("vendedor", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("moneda", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("estado", BoDataType.dt_LONG_TEXT, 100)
        userDS.Add("proyectado", BoDataType.dt_LONG_TEXT, 100)

        userDS.Add("asegurador", BoDataType.dt_PRICE, 100)
        userDS.Add("financiera", BoDataType.dt_PRICE, 100)
        userDS.Add("otroscosto", BoDataType.dt_PRICE, 100)

        userDS.Add("otrocospv", BoDataType.dt_PRICE, 100)
        userDS.Add("monneaseg", BoDataType.dt_PRICE, 100)
        userDS.Add("monnefina", BoDataType.dt_PRICE, 100)
        userDS.Add("otrocosu", BoDataType.dt_PRICE, 100)
        userDS.Add("monneasu", BoDataType.dt_PRICE, 100)
        userDS.Add("monnefiu", BoDataType.dt_PRICE, 100)
        userDS.Add("descuen", BoDataType.dt_PRICE, 100)

        'instancia los edittext
        EditTextClien = New EditTextSBO("txtClien", True, "", "cliente", FormularioSBO)
        EditTextTipoCl = New EditTextSBO("txtTipoCl", True, "", "tipo", FormularioSBO)
        EditTextNumCt = New EditTextSBO("txtNumCt", True, "", "numero", FormularioSBO)
        EditTextFechaCt = New EditTextSBO("txtFechaCt", True, "", "fecha", FormularioSBO)
        EditTextCodeCliente = New EditTextSBO("txtCodCl", True, "", "codigo", FormularioSBO)
        EditTextVendedor = New EditTextSBO("txtVen", True, "", "vendedor", FormularioSBO)
        EditTextMoneda = New EditTextSBO("txtMon", True, "", "moneda", FormularioSBO)
        EditTextEstado = New EditTextSBO("txtEstado", True, "", "estado", FormularioSBO)
        CheckProyectado = New CheckBoxSBO("chkCosPro", True, "", "proyectado", FormularioSBO)

        EditTextMonAse = New EditTextSBO("txtMNeAs", True, "", "asegurador", FormularioSBO)
        EditTextMonFin = New EditTextSBO("txtMNeFi", True, "", "financiera", FormularioSBO)
        EditTextOtrCos = New EditTextSBO("txtOtrCos", True, "", "otroscosto", FormularioSBO)

        EditTextOtCPV = New EditTextSBO("txtOtCPV", True, "", "otrocospv", FormularioSBO)
        EditTextMNeAs = New EditTextSBO("txtMAsU", True, "", "monneaseg", FormularioSBO)
        EditTextMNeFi = New EditTextSBO("txtMFiU", True, "", "monnefina", FormularioSBO)
        EditTextOtCU = New EditTextSBO("txtOtCU", True, "", "otrocosu", FormularioSBO)
        EditTextMAsU = New EditTextSBO("txtMAsU", True, "", "monneasu", FormularioSBO)
        EditTextMFiU = New EditTextSBO("txtMFiU", True, "", "monnefiu", FormularioSBO)
        EditTextDesc = New EditTextSBO("txtDesc", True, "", "descuen", FormularioSBO)

        'enlaza los edittext y las columnas
        EditTextClien.AsignaBinding()
        EditTextTipoCl.AsignaBinding()
        EditTextNumCt.AsignaBinding()
        EditTextFechaCt.AsignaBinding()
        EditTextCodeCliente.AsignaBinding()
        EditTextVendedor.AsignaBinding()
        EditTextMoneda.AsignaBinding()
        EditTextEstado.AsignaBinding()
        EditTextMonAse.AsignaBinding()
        EditTextMonFin.AsignaBinding()
        EditTextOtrCos.AsignaBinding()
        CheckProyectado.AsignaBinding()

        EditTextOtCPV.AsignaBinding()
        EditTextMNeAs.AsignaBinding()
        EditTextMNeFi.AsignaBinding()
        EditTextOtCU.AsignaBinding()
        EditTextMAsU.AsignaBinding()
        EditTextMFiU.AsignaBinding()
        EditTextDesc.AsignaBinding()

        'datatable que es la matriz de vehiculos
        Dim dtVehiculos As DataTable = FormularioSBO.DataSources.DataTables.Add("tVehiculos")
        dtVehiculos.Columns.Add("unidad", BoFieldsType.ft_AlphaNumeric, 100)
        dtVehiculos.Columns.Add("modelo", BoFieldsType.ft_AlphaNumeric, 100)
        dtVehiculos.Columns.Add("marca", BoFieldsType.ft_AlphaNumeric, 100)
        dtVehiculos.Columns.Add("estilo", BoFieldsType.ft_AlphaNumeric, 100)
        dtVehiculos.Columns.Add("valor", BoFieldsType.ft_Float, 100)
        dtVehiculos.Columns.Add("costo", BoFieldsType.ft_Float, 100)
        dtVehiculos.Columns.Add("utilidad", BoFieldsType.ft_Float, 100)
        dtVehiculos.Columns.Add("putilidad", BoFieldsType.ft_Percent, 100)
        dtVehiculos.Columns.Add("bono", BoFieldsType.ft_Float, 100)
        dtVehiculos.Columns.Add("prelis", BoFieldsType.ft_Float, 100)
        dtVehiculos.Columns.Add("desc", BoFieldsType.ft_Percent, 100)

        'datatable que es la matriz de accesorios
        Dim dtAccesorios As DataTable = FormularioSBO.DataSources.DataTables.Add("tAccesorios")
        dtAccesorios.Columns.Add("codigo", BoFieldsType.ft_AlphaNumeric, 100)
        dtAccesorios.Columns.Add("descripcion", BoFieldsType.ft_AlphaNumeric, 100)
        dtAccesorios.Columns.Add("valor", BoFieldsType.ft_Float, 100)
        dtAccesorios.Columns.Add("costo", BoFieldsType.ft_Float, 100)
        dtAccesorios.Columns.Add("utilidad", BoFieldsType.ft_Float, 100)
        dtAccesorios.Columns.Add("putilidad", BoFieldsType.ft_Percent, 100)
        dtAccesorios.Columns.Add("prelis", BoFieldsType.ft_Float, 100)
        dtAccesorios.Columns.Add("desc", BoFieldsType.ft_Percent, 100)

        'datatable que es la matriz de tramites
        Dim dtTramites As DataTable = FormularioSBO.DataSources.DataTables.Add("tTramites")
        dtTramites.Columns.Add("codigo", BoFieldsType.ft_AlphaNumeric, 100)
        dtTramites.Columns.Add("descripcion", BoFieldsType.ft_AlphaNumeric, 100)
        dtTramites.Columns.Add("valor", BoFieldsType.ft_Float, 100)
        dtTramites.Columns.Add("costo", BoFieldsType.ft_Float, 100)
        dtTramites.Columns.Add("utilidad", BoFieldsType.ft_Float, 100)
        dtTramites.Columns.Add("putilidad", BoFieldsType.ft_Percent, 100)

        'Instancia de la matriz de vehiculos, con la tabla tVehiculos
        MatrizVehiculos = New MatrizVehiculos("mtxVehic", FormularioSBO, "tVehiculos")
        MatrizVehiculos.CreaColumnas()
        MatrizVehiculos.LigaColumnas()

        'Instancia de la matriz de accesorios, con la tabla tAcccesorios
        MatrizAccesorios = New MatrizAccesorios("mtxAcc", FormularioSBO, "tAccesorios")
        MatrizAccesorios.CreaColumnas()
        MatrizAccesorios.LigaColumnas()

        'Instancia de la matriz de accesorios, con la tabla tAcccesorios
        MatrizTramites = New MatrizTramites("mtxTra", FormularioSBO, "tTramites")
        MatrizTramites.CreaColumnas()
        MatrizTramites.LigaColumnas()

        'inicializacion de Boton
        ButtonbtnOk = New ButtonSBO("2", FormularioSBO)
        'ButtonbtnOk.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

        'agrego al userDataSources
        Dim oUDSVehiculo As UserDataSources = FormularioSBO.DataSources.UserDataSources
        oUDSVehiculo.Add("valV", BoDataType.dt_PRICE, 100)
        oUDSVehiculo.Add("cosV", BoDataType.dt_PRICE, 100)
        oUDSVehiculo.Add("utiV", BoDataType.dt_PRICE, 100)
        oUDSVehiculo.Add("putiV", BoDataType.dt_PERCENT, 100)
        oUDSVehiculo.Add("bonV", BoDataType.dt_PRICE, 100)

        'agrego al userDataSources
        Dim oUDSAccesorio As UserDataSources = FormularioSBO.DataSources.UserDataSources
        oUDSAccesorio.Add("valA", BoDataType.dt_PRICE, 100)
        oUDSAccesorio.Add("cosA", BoDataType.dt_PRICE, 100)
        oUDSAccesorio.Add("utiA", BoDataType.dt_PRICE, 100)
        oUDSAccesorio.Add("putiA", BoDataType.dt_PERCENT, 100)

        'agrego al userDataSources
        Dim oUDSTramite As UserDataSources = FormularioSBO.DataSources.UserDataSources
        oUDSTramite.Add("valT", BoDataType.dt_PRICE, 100)
        oUDSTramite.Add("cosT", BoDataType.dt_PRICE, 100)
        oUDSTramite.Add("utiT", BoDataType.dt_PRICE, 100)
        oUDSTramite.Add("putiT", BoDataType.dt_PERCENT, 100)

        'agrego al userDataSources General
        Dim oUDSGeneral As UserDataSources = FormularioSBO.DataSources.UserDataSources
        oUDSGeneral.Add("valG", BoDataType.dt_PRICE, 100)
        oUDSGeneral.Add("cosG", BoDataType.dt_PRICE, 100)
        oUDSGeneral.Add("utiG", BoDataType.dt_PRICE, 100)
        oUDSGeneral.Add("putiG", BoDataType.dt_PERCENT, 100)
        oUDSGeneral.Add("bonG", BoDataType.dt_PRICE, 100)

        ValidaPermisoBTNPrint(FormularioSBO, ApplicationSBO)

        'Manejo de formulario
        FormularioSBO.Freeze(False)
    End Sub

#End Region '"Metodos de interfaz"

#Region "Carga Valores en pantalla"

    'Carga de los EditText
    Public Sub CargaBalance(ByVal strCliente As String, ByVal strTipoCliente As String,
                            ByVal strNumeroContrato As String, ByVal strFechaContrato As String,
                            ByVal strCodeCliente As String, ByVal strVendedor As String,
                            ByVal strMoneda As String, ByVal strEstado As String,
                            ByVal strCostoProyectado As String, ByVal decMontoNetoAseguradora As Decimal,
                            ByVal decMontoNetoFinanciera As Decimal,
                            ByVal decOtrosCostos As Decimal, ByVal decDescuento As Decimal)

        Try
            Dim n As Globalization.NumberFormatInfo
            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            'Manejo del formulario
            FormularioSBO.Freeze(True)

            'define el contrato
            contrato = strNumeroContrato

            'obtiene el tipo de cliente
            Dim strTipoClienteLocal As String = RetornaTipoCliente(strTipoCliente)

            'obtiene la descripcion de l moneda
            Dim strModenaLocal As String = RetornaModena(strMoneda)

            'obtiene el estado del cv
            Dim strEstadoLocal As String = RetornaEstado(strEstado)

            'convierte a fecha 
            Dim fecha As Date
            fecha = Date.ParseExact(strFechaContrato, "yyyyMMdd", Nothing)
            fecha = New Date(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0)

            'verifica que el formulario exista para asignar cada valor a cada campo
            If FormularioSBO IsNot Nothing Then
                EditTextClien.AsignaValorUserDataSource(strCliente)
                EditTextTipoCl.AsignaValorUserDataSource(strTipoClienteLocal)
                EditTextNumCt.AsignaValorUserDataSource(strNumeroContrato)
                EditTextFechaCt.AsignaValorUserDataSource(fecha.ToString("yyyyMMdd"))
                EditTextCodeCliente.AsignaValorUserDataSource(strCodeCliente)
                EditTextVendedor.AsignaValorUserDataSource(strVendedor)
                EditTextMoneda.AsignaValorUserDataSource(strModenaLocal)
                EditTextEstado.AsignaValorUserDataSource(strEstadoLocal)
                EditTextMonAse.AsignaValorUserDataSource(decMontoNetoAseguradora.ToString(n))
                EditTextMonFin.AsignaValorUserDataSource(decMontoNetoFinanciera.ToString(n))
                EditTextOtrCos.AsignaValorUserDataSource(decOtrosCostos.ToString(n))
                CheckProyectado.AsignaValorUserDataSource(strCostoProyectado)
                EditTextOtCPV.AsignaValorUserDataSource(decOtrosCostos.ToString(n))
                EditTextMAsU.AsignaValorUserDataSource(decMontoNetoAseguradora.ToString(n))
                EditTextMFiU.AsignaValorUserDataSource(decMontoNetoFinanciera.ToString(n))
                EditTextOtCU.AsignaValorUserDataSource(0)
                EditTextDesc.AsignaValorUserDataSource(decDescuento.ToString(n))

                g_OtrosCostos = decOtrosCostos
                g_MontoAseguradora = decMontoNetoAseguradora
                g_MontoFinanciera = decMontoNetoFinanciera
                g_Descuento = decDescuento

            End If

            'Manejo del formulario
            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, CompanySBO)
        End Try

    End Sub

    'carga la matriz vehiculos 
    'todos los campos menos utilidad, porcentajes, costo, precio venrta
    Public Sub CargaDetallesVehiculos(ByVal count As Integer, ByVal strCostoProyectado As String,
                            ByVal strMonedaSistema As String, ByVal strMonedaLocal As String, ByVal strMonedaCV As String,
                            ByVal strTipoCambioCV As String, ByVal strTipoCambioSistema As String, ByVal fecha As Date)

        Try
            'Manejo del formulario
            FormularioSBO.Freeze(True)

            'inicializo a 0 los valores generales 
            TGeneral = 0
            CGeneral = 0
            UGeneral = 0
            BGeneral = 0

            dtValoresNuevos = FormularioSBO.DataSources.DataTables.Item("tVehiculos")

            'cargar costos vehiculos 
            c_CosVeh = GeneraCostosXUnidadVehiculo(_companySbo.Server, _companySbo.CompanyDB, strCostoProyectado,
                                                   _companySbo,
                                                   strMonedaSistema, strMonedaLocal, strMonedaCV,
                                                   strTipoCambioCV, strTipoCambioSistema, fecha)
            'carga utilidades de vehiculos
            c_UtilVeh = GeneraUtilidadXVehiculos()

            'carga porcentajes de utilidades
            c_PUtilVeh = GeneraPorcentajesVeh()

            If c_Unidad IsNot Nothing And
                c_Modelo IsNot Nothing And
                c_Marca IsNot Nothing And
                c_Estilo IsNot Nothing And
                c_ValVeh IsNot Nothing And
                c_BonoVeh IsNot Nothing Then

                'llena el datatable asociado a la matriz
                For i As Integer = 0 To count - 1
                    dtValoresNuevos.Rows.Add()
                    dtValoresNuevos.SetValue("unidad", i, c_Unidad(i))
                    dtValoresNuevos.SetValue("marca", i, c_Marca(i))
                    dtValoresNuevos.SetValue("modelo", i, c_Modelo(i))
                    dtValoresNuevos.SetValue("estilo", i, c_Estilo(i))
                Next i

                'pinta vehiculos
                'Costo, Precio de venta, utilidad, porcentajes 
                Call Pintar(count, c_ValVeh, c_CosVeh, c_UtilVeh, c_PUtilVeh, dtValoresNuevos, True, False, False, c_BonoVeh, c_PreList, c_Desc)

                'ejecuta la actualizacion del datatable a la matriz en interfaz
                MatrizVehiculos.Matrix.LoadFromDataSource()

            End If

            'Manejo del formulario
            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    'carga la matriz accesorios
    'todos los campos menos utilidad, porcentajes, costo, precio venta
    Public Sub CargaDetallesAccesorios(ByVal count As Integer)
        'numero de decimales
        Dim decimales As Globalization.NumberFormatInfo

        Try
            'Manejo del formulario
            FormularioSBO.Freeze(True)

            decimales = DIHelper.GetNumberFormatInfo(CompanySBO)
            dtAccesoriosNuevos = FormularioSBO.DataSources.DataTables.Item("tAccesorios")

            'cargar utilidad accesorios
            c_UtilAcc = GeneraUtilidadXAccesorios()

            'carga % utilidades
            c_PUtilAcc = GeneraPorcentajesAcc()

            If c_Codigo IsNot Nothing And
                c_Descripcion IsNot Nothing And
                c_ValAcc IsNot Nothing Then

                'carga de datos
                For i As Integer = 0 To count - 1
                    dtAccesoriosNuevos.Rows.Add()
                    dtAccesoriosNuevos.SetValue("codigo", i, c_Codigo(i))
                    dtAccesoriosNuevos.SetValue("descripcion", i, c_Descripcion(i))
                    dtAccesoriosNuevos.SetValue("desc", i, CDbl(c_DescAcc(i)))
                    dtAccesoriosNuevos.SetValue("prelis", i, CDbl(c_PreListAcc(i)))
                Next i

                'pinta vehiculos
                Call Pintar(count, c_ValAcc, c_CosAcc, c_UtilAcc, c_PUtilAcc, dtAccesoriosNuevos, False, True, False, Nothing, Nothing, Nothing)

                'actualiza la matriz de accesorio
                MatrizAccesorios.Matrix.LoadFromDataSource()

                'carga totales accesorios
                'CargaTotalesAccesrorios(totalValor, totalCosto, totalUtilidad)
            End If

            'Manejo del formulario
            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    ''' <summary>
    ''' Carga codigos y descripciones para los tramites
    ''' Nom carga Utilidad, Precio ni Costo
    ''' </summary>
    ''' <param name="count">Tamaño de la matriz de tramites </param>
    ''' <remarks></remarks>
    Public Sub CargaDetallesTramites(ByVal count As Integer)
        'numero de decimales
        Dim decimales As Globalization.NumberFormatInfo

        Try
            'Manejo del formulario
            FormularioSBO.Freeze(True)

            decimales = DIHelper.GetNumberFormatInfo(CompanySBO)
            dtTramitesNuevos = FormularioSBO.DataSources.DataTables.Item("tTramites")

            'cargar utilidad accesorios
            c_UtilTra = GeneraUtilidadXTramites()

            'carga % utilidades
            c_PUtilTra = GeneraPorcentajesTra()

            If c_CodTra IsNot Nothing And
                c_DesTra IsNot Nothing And
                c_ValTra IsNot Nothing Then

                'carga de datos
                For i As Integer = 0 To count - 1
                    dtTramitesNuevos.Rows.Add()
                    dtTramitesNuevos.SetValue("codigo", i, c_CodTra(i))
                    dtTramitesNuevos.SetValue("descripcion", i, c_DesTra(i))
                Next i

                'pinta vehiculos
                Call Pintar(count, c_ValTra, c_CosTra, c_UtilTra, c_PUtilTra, dtTramitesNuevos, False, False, True, Nothing, Nothing, Nothing)

                'actualiza la matriz de tramites
                MatrizTramites.Matrix.LoadFromDataSource()

                'carga totales accesorios
                'CargaTotalesAccesrorios(totalValor, totalCosto, totalUtilidad)
            End If

            'Manejo del formulario
            FormularioSBO.Freeze(False)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, _applicationSbo)
        End Try

    End Sub

    'carga los edittext de totales para vehiculos
    Public Sub CargaTotalesVehiculos(ByVal ValorTotal As Decimal,
                                     ByVal CostoTotal As Decimal,
                                     ByVal UtilidadTotal As Decimal,
                                     ByVal BonosTotal As Decimal)
        Try

            Dim n As Globalization.NumberFormatInfo
            n = DIHelper.GetNumberFormatInfo(CompanySBO)

            Dim pUtilidad As Decimal = 0
            Dim strConfig As String

            strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

            If FormularioSBO IsNot Nothing Then
                'instancia de los edit generales de vehiculos
                EditTextCostVeh = New EditTextSBO("txtCostVeh", True, "", "cosV", FormularioSBO)
                EditTextValVeh = New EditTextSBO("txtValVeh", True, "", "valV", FormularioSBO)
                EditTextUtilVeh = New EditTextSBO("txtUtilVeh", True, "", "utiV", FormularioSBO)
                EditTextPUtilVeh = New EditTextSBO("txtPUtilV", True, "", "putiV", FormularioSBO)
                EditTextBonoVeh = New EditTextSBO("txtBonVeh", True, "", "bonV", FormularioSBO)

                EditTextCostVeh.AsignaBinding()
                EditTextValVeh.AsignaBinding()
                EditTextUtilVeh.AsignaBinding()
                EditTextPUtilVeh.AsignaBinding()
                EditTextBonoVeh.AsignaBinding()

                If Not String.IsNullOrEmpty(ValorTotal.ToString()) _
                    And Not String.IsNullOrEmpty(CostoTotal.ToString()) _
                    And Not String.IsNullOrEmpty(UtilidadTotal.ToString()) Then

                    If Not CostoTotal = 0 Then

                        If strConfig = "Y" Then
                            pUtilidad = (100 / CostoTotal) * UtilidadTotal
                        Else
                            pUtilidad = (100 / (ValorTotal + BonosTotal)) * UtilidadTotal
                        End If

                        If pUtilidad < 0 Then
                            pUtilidad = 0
                        End If
                    ElseIf ValorTotal > 0 _
                        And CostoTotal = 0 Then
                        pUtilidad = 100
                    Else
                        pUtilidad = 0
                    End If

                End If

                If FormularioSBO IsNot Nothing Then
                    EditTextValVeh.AsignaValorUserDataSource(ValorTotal.ToString(n))
                    EditTextCostVeh.AsignaValorUserDataSource(CostoTotal.ToString(n))
                    EditTextUtilVeh.AsignaValorUserDataSource(UtilidadTotal.ToString(n))
                    EditTextPUtilVeh.AsignaValorUserDataSource(pUtilidad.ToString(n))
                    EditTextBonoVeh.AsignaValorUserDataSource(BonosTotal.ToString(n))
                End If
            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Sub

    'carga los edittext de totales para accesorios
    Public Sub CargaTotalesAccesrorios(ByVal ValorTotal As Decimal,
                                       ByVal CostoTotal As Decimal,
                                       ByVal UtilidadTotal As Decimal)
        Try

            Dim n As Globalization.NumberFormatInfo
            n = DIHelper.GetNumberFormatInfo(CompanySBO)
            Dim pUtilidad As Decimal = 0

            If FormularioSBO IsNot Nothing Then
                'instancia de los edit generales de vehiculos
                EditTextCostAcc = New EditTextSBO("txtCostAcc", True, "", "cosA", FormularioSBO)
                EditTextValAcc = New EditTextSBO("txtValAcc", True, "", "valA", FormularioSBO)
                EditTextUtilAcc = New EditTextSBO("txtUtilAcc", True, "", "utiA", FormularioSBO)
                EditTextPUtilAcc = New EditTextSBO("txtPUtilA", True, "", "putiA", FormularioSBO)

                EditTextCostAcc.AsignaBinding()
                EditTextValAcc.AsignaBinding()
                EditTextUtilAcc.AsignaBinding()
                EditTextPUtilAcc.AsignaBinding()

                If Not String.IsNullOrEmpty(ValorTotal.ToString()) _
                    And Not String.IsNullOrEmpty(CostoTotal.ToString()) _
                    And Not String.IsNullOrEmpty(UtilidadTotal.ToString()) Then

                    If Not CostoTotal = 0 Then
                        'porcentaje = (100/costo) * utilidad 
                        pUtilidad = (100 / CostoTotal) * UtilidadTotal
                        If pUtilidad < 0 Then
                            pUtilidad = 0
                        End If
                    ElseIf ValorTotal > 0 _
                        And CostoTotal = 0 Then
                        pUtilidad = 100
                    Else
                        pUtilidad = 0
                    End If

                End If

                If FormularioSBO IsNot Nothing Then
                    EditTextValAcc.AsignaValorUserDataSource(ValorTotal.ToString(n))
                    EditTextCostAcc.AsignaValorUserDataSource(CostoTotal.ToString(n))
                    EditTextUtilAcc.AsignaValorUserDataSource(UtilidadTotal.ToString(n))
                    EditTextPUtilAcc.AsignaValorUserDataSource(pUtilidad.ToString(n))
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Sub

    ''' <summary>
    ''' Carga totales de tramites en los campos de texto
    ''' </summary>
    ''' <param name="ValorTotal"></param>
    ''' <param name="CostoTotal"></param>
    ''' <param name="UtilidadTotal"></param>
    ''' <remarks></remarks>
    Public Sub CargaTotalesTramites(ByVal ValorTotal As Decimal,
                                       ByVal CostoTotal As Decimal,
                                       ByVal UtilidadTotal As Decimal)
        Try

            Dim n As Globalization.NumberFormatInfo
            n = DIHelper.GetNumberFormatInfo(CompanySBO)
            Dim pUtilidad As Decimal = 0

            If FormularioSBO IsNot Nothing Then
                'instancia de los edit generales de vehiculos
                EditTextCostTra = New EditTextSBO("txtCosTra", True, "", "cosT", FormularioSBO)
                EditTextValTra = New EditTextSBO("txtValTra", True, "", "valT", FormularioSBO)
                EditTextUtilTra = New EditTextSBO("txtUtilTra", True, "", "utiT", FormularioSBO)
                EditTextPUtilTra = New EditTextSBO("txtPUtilT", True, "", "putiT", FormularioSBO)

                EditTextCostTra.AsignaBinding()
                EditTextValTra.AsignaBinding()
                EditTextUtilTra.AsignaBinding()
                EditTextPUtilTra.AsignaBinding()

                If Not String.IsNullOrEmpty(ValorTotal.ToString()) _
                    And Not String.IsNullOrEmpty(CostoTotal.ToString()) _
                    And Not String.IsNullOrEmpty(UtilidadTotal.ToString()) Then

                    If Not CostoTotal = 0 Then
                        'porcentaje = (100/costo) * utilidad 
                        pUtilidad = (100 / CostoTotal) * UtilidadTotal
                        If pUtilidad < 0 Then
                            pUtilidad = 0
                        End If
                    ElseIf ValorTotal > 0 _
                        And CostoTotal = 0 Then
                        pUtilidad = 100
                    Else
                        pUtilidad = 0
                    End If

                End If

                If FormularioSBO IsNot Nothing Then
                    EditTextValTra.AsignaValorUserDataSource(ValorTotal.ToString(n))
                    EditTextCostTra.AsignaValorUserDataSource(CostoTotal.ToString(n))
                    EditTextUtilTra.AsignaValorUserDataSource(UtilidadTotal.ToString(n))
                    EditTextPUtilTra.AsignaValorUserDataSource(pUtilidad.ToString(n))
                End If
            End If

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Sub

    'carga los edittext de totales generales
    Public Sub CargaTotalesGenerales(ByVal GeneralTotal As Decimal,
                                     ByVal GeneralCosto As Decimal,
                                     ByVal GeneralUtilidad As Decimal,
                                     ByVal esVehiculo As Boolean,
                                     ByVal GeneralBono As Decimal)
        Try

            Dim n As Globalization.NumberFormatInfo
            n = DIHelper.GetNumberFormatInfo(CompanySBO)
            Dim pUtilidad As Decimal = 0
            Dim strConfig As String

            strConfig = DMS_Connector.Configuracion.ParamGenAddon.U_UtCos

            If FormularioSBO IsNot Nothing Then
                'instancia de los edit generales de vehiculos
                EditTextCostG = New EditTextSBO("txtCosGen", True, "", "cosG", FormularioSBO)
                EditTextValG = New EditTextSBO("txtValGen", True, "", "valG", FormularioSBO)
                EditTextUtilG = New EditTextSBO("txtUtiGen", True, "", "utiG", FormularioSBO)
                EditTextPUtilG = New EditTextSBO("txtPUtilG", True, "", "putiG", FormularioSBO)
                EditTextBonG = New EditTextSBO("txtBonGen", True, "", "bonG", FormularioSBO)

                EditTextCostG.AsignaBinding()
                EditTextValG.AsignaBinding()
                EditTextUtilG.AsignaBinding()
                EditTextPUtilG.AsignaBinding()
                EditTextBonG.AsignaBinding()

                GeneralUtilidad += g_MontoAseguradora + g_MontoFinanciera - g_Descuento
                GeneralCosto += g_OtrosCostos
                GeneralTotal += g_OtrosCostos + g_MontoAseguradora + g_MontoFinanciera - g_Descuento

                If Not String.IsNullOrEmpty(GeneralTotal.ToString()) _
                    And Not String.IsNullOrEmpty(GeneralCosto.ToString()) _
                    And Not String.IsNullOrEmpty(GeneralUtilidad.ToString()) Then

                    If Not GeneralCosto = 0 Then
                        'porcentaje = (100/costo) * utilidad 
                        If strConfig = "Y" Then
                            pUtilidad = (100 / GeneralCosto) * GeneralUtilidad
                        Else
                            pUtilidad = (100 / (GeneralTotal + GeneralBono)) * GeneralUtilidad
                        End If
                        If pUtilidad < 0 Then
                            pUtilidad = 0
                        End If
                    ElseIf GeneralTotal > 0 _
                        And GeneralCosto = 0 Then
                        pUtilidad = 100
                    Else
                        pUtilidad = 0
                    End If

                End If

                If FormularioSBO IsNot Nothing Then
                    EditTextValG.AsignaValorUserDataSource(GeneralTotal.ToString(n))
                    EditTextCostG.AsignaValorUserDataSource(GeneralCosto.ToString(n))
                    EditTextUtilG.AsignaValorUserDataSource(GeneralUtilidad.ToString(n))
                    EditTextPUtilG.AsignaValorUserDataSource(pUtilidad.ToString(n))
                    EditTextBonG.AsignaValorUserDataSource(GeneralBono.ToString(n))
                End If

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Sub

#End Region '"Carga Valores en pantalla"

#Region "Pintar los datos (Valores, Costos, Utilidades, porcentajes de utilidades) en pantalla"

    'PINTA EN PANTALLA
    Public Sub Pintar(ByVal Count As Integer, _
                              ByVal vVal As Decimal(), _
                              ByVal vCost As Decimal(), _
                              ByVal vUtil As Decimal(), _
                              ByVal pUtil As Decimal(),
                              ByVal dt As DataTable, _
                              ByVal esVehiculo As Boolean, _
                              ByVal esAccesorio As Boolean, _
                              ByVal esTramite As Boolean, _
                              ByVal vBono As Decimal(), _
                              ByVal vPreLis As Decimal(), _
                              ByVal vDesc As Decimal())

        'totales para vehiculos
        Dim totalUtilidad As Decimal
        Dim totalCosto As Decimal
        Dim totalValor As Decimal
        Dim totalBonos As Decimal
        Dim totalPreLis As Decimal

        Try
            'decimales
            'numero de decimales 
            Dim decimales As Globalization.NumberFormatInfo
            decimales = DIHelper.GetNumberFormatInfo(CompanySBO)

            'pinta los valores en pantalla 
            If dt IsNot Nothing Then
                For i As Integer = 0 To Count - 1
                    If vVal IsNot Nothing Then
                        dt.SetValue("valor", i, vVal(i).ToString(decimales))
                        totalValor = totalValor + vVal(i)
                        TGeneral = TGeneral + vVal(i)
                    End If
                    If vCost IsNot Nothing Then
                        dt.SetValue("costo", i, vCost(i).ToString(decimales))
                        totalCosto = totalCosto + vCost(i)
                        CGeneral = CGeneral + vCost(i)
                    End If
                    If vUtil IsNot Nothing Then
                        dt.SetValue("utilidad", i, vUtil(i).ToString(decimales))
                        totalUtilidad = totalUtilidad + vUtil(i)
                        UGeneral = UGeneral + vUtil(i)
                    End If
                    If pUtil IsNot Nothing Then
                        dt.SetValue("putilidad", i, pUtil(i).ToString(decimales))
                    End If
                    If esVehiculo Then
                        If vBono IsNot Nothing Then
                            dt.SetValue("bono", i, vBono(i).ToString(decimales))
                            totalBonos = totalBonos + vBono(i)
                            BGeneral = BGeneral + vBono(i)
                        End If
                    End If

                    If Not esTramite Then
                        If vPreLis IsNot Nothing Then
                            dt.SetValue("prelis", i, vPreLis(i).ToString(decimales))
                            totalPreLis = totalPreLis + vPreLis(i)
                        End If
                        If vDesc IsNot Nothing Then
                            dt.SetValue("desc", i, vDesc(i).ToString(decimales))
                        End If
                    End If

                Next
            End If

            If esVehiculo Then
                'carga totales de vehiculos
                CargaTotalesVehiculos(totalValor, totalCosto, totalUtilidad, totalBonos)
            ElseIf esAccesorio Then
                'carga totales de accesorios
                CargaTotalesAccesrorios(totalValor, totalCosto, totalUtilidad)
            ElseIf esTramite Then
                'carga totales de tramites
                CargaTotalesTramites(totalValor, totalCosto, totalUtilidad)
            End If
            'carga los totales generales
            CargaTotalesGenerales(TGeneral, CGeneral, UGeneral, esVehiculo, BGeneral)
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, Me)
        End Try
    End Sub

#End Region '"Pintar los datos (Valores, Costos, Utilidades, porcentajes de utilidades) en pantalla"

#End Region

End Class
