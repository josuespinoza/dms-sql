Imports System.Globalization
Imports SAPbouiCOM
Imports SCG.SBOFramework
Imports SCG.SBOFramework.UI
Imports SCG.SBOFramework.UI.Extensions
Imports ICompany = SAPbobsCOM.ICompany

'Clase para manejar los controles del formulario de plan de pagos del modulo de financiamiento

Partial Public Class PlanPagosFormulario : Implements IFormularioSBO

    Private _formType As String

    Private _nombreXml As String

    Private _titulo As String

    Private _formularioSbo As IForm

    Private _inicializado As Boolean

    Private _applicationSbo As Application

    Private _companySbo As ICompany

    Public dataTablePlanes As DataTable

    Private _strConexion As String

    Private _strDireccionReportes As String

    Private _strUsuarioBD As String

    Private _strContraseñaBD As String

    Public Shared g_strTipoPlan As String
    Public Shared g_strPrestamo As String
    Public Shared g_blnImprimeCreado As Boolean

    Public EditTextCliente As EditTextSBO
    Public EditTextEnteFinanciero As EditTextSBO
    Public EditTextMontoFin As EditTextSBO
    Public EditTextPlazo As EditTextSBO
    Public EditTextFechaInicio As EditTextSBO
    Public EditTextMoneda As EditTextSBO
    Public EditTextIntNormal As EditTextSBO
    Public EditTextIntMora As EditTextSBO
    Public EditTextTipoCuota As EditTextSBO
    Public EditTextPrecioVenta As EditTextSBO
    Public EditTextPrima As EditTextSBO
    Public MatrixPlanPagos As MatrixSBOPlanPagos

    Public Shared ButtonImprimirPlan As ButtonSBO

    Public Sub New(ByVal application As Application, ByVal companySbo As ICompany, dbUser As String, dbPassword As String, pstrXMl As String)
        _companySbo = companySbo
        _applicationSbo = application
        NombreXml = pstrXMl
        Titulo = My.Resources.Resource.TituloPlanPagos
        FormType = "SCGD_PlanTeorico"
        StrDireccionReportes = String.Format("{0}{1}", DMS_Connector.Configuracion.ParamGenAddon.U_Reportes.Trim(), "\")
        StrUsuarioBD = dbUser
        StrContraseñaBD = dbPassword
    End Sub

    Public Property FormType() As String Implements IFormularioSBO.FormType
        Get
            Return _formType
        End Get
        Set(ByVal value As String)
            _formType = value
        End Set
    End Property

    Public Property StrConexion() As String
        Get
            Return _strConexion
        End Get
        Set(ByVal value As String)
            _strConexion = value
        End Set
    End Property

    Public Property StrDireccionReportes() As String
        Get
            Return _strDireccionReportes
        End Get
        Set(ByVal value As String)
            _strDireccionReportes = value
        End Set
    End Property

    Public Property StrUsuarioBD() As String
        Get
            Return _strUsuarioBD
        End Get
        Set(ByVal value As String)
            _strUsuarioBD = value
        End Set
    End Property

    Public Property StrContraseñaBD() As String
        Get
            Return _strContraseñaBD
        End Get
        Set(ByVal value As String)
            _strContraseñaBD = value
        End Set
    End Property

    Public Property NombreXml() As String Implements IFormularioSBO.NombreXml
        Get
            Return _nombreXml
        End Get
        Set(ByVal value As String)
            _nombreXml = value
        End Set
    End Property

    Public Property Titulo() As String Implements IFormularioSBO.Titulo
        Get
            Return _titulo
        End Get
        Set(ByVal value As String)
            _titulo = value
        End Set
    End Property

    Public Property FormularioSBO() As IForm Implements IFormularioSBO.FormularioSBO
        Get
            Return _formularioSbo
        End Get
        Set(ByVal value As IForm)
            _formularioSbo = value
        End Set
    End Property

    Public Property Inicializado() As Boolean Implements IFormularioSBO.Inicializado
        Get
            Return _inicializado
        End Get
        Set(ByVal value As Boolean)
            _inicializado = value
        End Set
    End Property

    'Incializa controles de pantalla de plan de pagos, UserDataSource para campos y DataTable para columnas de la matriz de plan de pagos

    Public Sub InicializarControles() Implements IFormularioSBO.InicializarControles

        FormularioSBO.Freeze(True)

        dataTablePlanes = FormularioSBO.DataSources.DataTables.Add("Planes")

        Dim userDataSources As UserDataSources = FormularioSBO.DataSources.UserDataSources
        userDataSources.Add("cliente", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("ente", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("monto", BoDataType.dt_PRICE, 100)
        userDataSources.Add("plazo", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("fecha", BoDataType.dt_DATE, 100)
        userDataSources.Add("moneda", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("interes", BoDataType.dt_PRICE, 100)
        userDataSources.Add("moratorio", BoDataType.dt_PRICE, 100)
        userDataSources.Add("tipoCuota", BoDataType.dt_LONG_TEXT, 100)
        userDataSources.Add("precioVta", BoDataType.dt_PRICE, 100)
        userDataSources.Add("prima", BoDataType.dt_PRICE, 100)

        Dim dataTable As DataTable = FormularioSBO.DataSources.DataTables.Add("LinPlanP")
        dataTable.Columns.Add("numero", BoFieldsType.ft_Integer, 100)
        dataTable.Columns.Add("fecha", BoFieldsType.ft_Date, 100)
        dataTable.Columns.Add("saldoInicial", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("cuota", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("capital", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("interes", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("intMora", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("saldoFinal", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("pagado", BoFieldsType.ft_AlphaNumeric, 100)
        dataTable.Columns.Add("notaCred", BoFieldsType.ft_AlphaNumeric, 100)
        dataTable.Columns.Add("docInt", BoFieldsType.ft_AlphaNumeric, 100)
        dataTable.Columns.Add("docFac", BoFieldsType.ft_AlphaNumeric, 100)
        dataTable.Columns.Add("borrador", BoFieldsType.ft_AlphaNumeric, 100)
        dataTable.Columns.Add("capPend", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("intPend", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("moraPend", BoFieldsType.ft_Float, 100)
        dataTable.Columns.Add("diasInt", BoFieldsType.ft_Integer, 100)
        dataTable.Columns.Add("diasMora", BoFieldsType.ft_Integer, 100)

        EditTextCliente = New EditTextSBO("txtCliente", True, "", "cliente", FormularioSBO)
        EditTextEnteFinanciero = New EditTextSBO("txtEnteFin", True, "", "ente", FormularioSBO)
        EditTextMontoFin = New EditTextSBO("txtMontoFi", True, "", "monto", FormularioSBO)
        EditTextPlazo = New EditTextSBO("txtPlazo", True, "", "plazo", FormularioSBO)
        EditTextFechaInicio = New EditTextSBO("txtFechaIn", True, "", "fecha", FormularioSBO)
        EditTextMoneda = New EditTextSBO("txtMoneda", True, "", "moneda", FormularioSBO)
        EditTextIntNormal = New EditTextSBO("txtIntNorm", True, "", "interes", FormularioSBO)
        EditTextIntMora = New EditTextSBO("txtIntMora", True, "", "moratorio", FormularioSBO)
        EditTextTipoCuota = New EditTextSBO("txtTipoCuo", True, "", "tipoCuota", FormularioSBO)
        EditTextPrecioVenta = New EditTextSBO("txtPreVta", True, "", "precioVta", FormularioSBO)
        EditTextPrima = New EditTextSBO("txtPrima", True, "", "prima", FormularioSBO)

        EditTextCliente.AsignaBinding()
        EditTextEnteFinanciero.AsignaBinding()
        EditTextMontoFin.AsignaBinding()
        EditTextPlazo.AsignaBinding()
        EditTextFechaInicio.AsignaBinding()
        EditTextMoneda.AsignaBinding()
        EditTextIntNormal.AsignaBinding()
        EditTextIntMora.AsignaBinding()
        EditTextTipoCuota.AsignaBinding()
        EditTextPrecioVenta.AsignaBinding()
        EditTextPrima.AsignaBinding()

        MatrixPlanPagos = New MatrixSBOPlanPagos("mtxPlanPag", FormularioSBO, "LinPlanP")
        MatrixPlanPagos.CreaColumnas()
        MatrixPlanPagos.LigaColumnas()

        ButtonImprimirPlan = New ButtonSBO("btnImpPlan", FormularioSBO)

        ButtonImprimirPlan.ItemSBO.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_True)

        FormularioSBO.Freeze(False)

    End Sub

    Public Sub InicializaFormulario() Implements IFormularioSBO.InicializaFormulario

        If FormularioSBO IsNot Nothing Then

        End If

    End Sub

    Public ReadOnly Property ApplicationSBO() As IApplication Implements IFormularioSBO.ApplicationSBO
        Get
            Return _applicationSbo
        End Get
    End Property

    Public ReadOnly Property CompanySBO() As ICompany Implements IFormularioSBO.CompanySBO
        Get
            Return _companySbo
        End Get
    End Property

    'Carga los datos generales, de encabezado, a pantalla de plan de pagos mediante los UserDataSource

    Public Sub CargarPlanPagos(ByVal strCliente As String, ByVal strEnte As String, ByVal decMonto As Decimal, ByVal intPlazo As Integer, ByVal dtFecha As Date, ByVal strMoneda As String, _
                                    ByVal decIntNormal As Decimal, ByVal decIntMora As Decimal, ByVal strTipoCuota As String, ByVal decPrecioVenta As Decimal, ByVal decPrima As Decimal, ByVal blnImprimeCreado As Boolean, ByVal strPrestamo As String, _
                                    Optional ByVal strTipoPlan As String = "")

        Dim n As NumberFormatInfo

        Try

            If FormularioSBO IsNot Nothing Then

                n = DIHelper.GetNumberFormatInfo(CompanySBO)

                g_blnImprimeCreado = blnImprimeCreado

                If blnImprimeCreado = True Then

                    If strTipoPlan = "T" Then

                        g_strTipoPlan = "T"

                    ElseIf strTipoPlan = "R" Then

                        g_strTipoPlan = "R"

                    End If

                End If

                g_strPrestamo = strPrestamo

                EditTextCliente.AsignaValorUserDataSource(strCliente)
                EditTextEnteFinanciero.AsignaValorUserDataSource(strEnte)
                EditTextMontoFin.AsignaValorUserDataSource(decMonto.ToString(n))
                EditTextPlazo.AsignaValorUserDataSource(CStr(intPlazo) & My.Resources.Resource.Meses)
                EditTextFechaInicio.AsignaValorUserDataSource(dtFecha.ToString("yyyyMMdd"))
                EditTextMoneda.AsignaValorUserDataSource(strMoneda)
                EditTextIntNormal.AsignaValorUserDataSource(decIntNormal.ToString(n))
                EditTextIntMora.AsignaValorUserDataSource(decIntMora.ToString(n))
                EditTextTipoCuota.AsignaValorUserDataSource(strTipoCuota)
                EditTextPrecioVenta.AsignaValorUserDataSource(decPrecioVenta.ToString(n))
                EditTextPrima.AsignaValorUserDataSource(decPrima.ToString(n))

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Carga columnas de matriz de plan de pagos mediante el DataTable ligado a la matriz, según arreglos calculados para el plan de pagos

    Public Sub CargarColumnasPlanPagos(ByVal intPlazo As Integer)

        Dim dataTablePlan As DataTable
        Dim numberFormatInfo As Globalization.NumberFormatInfo

        Try

            numberFormatInfo = DIHelper.GetNumberFormatInfo(CompanySBO)

            dataTablePlan = FormularioSBO.DataSources.DataTables.Item("LinPlanP")

            For i As Integer = 0 To intPlazo - 1

                dataTablePlan.Rows.Add()
                dataTablePlan.SetValue("numero", i, g_intNumero(i))
                dataTablePlan.SetValue("fecha", i, g_dtFechaPago(i))
                dataTablePlan.SetValue("saldoInicial", i, g_decSaldoInicial(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("cuota", i, g_decCuota(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("capital", i, g_decCapital(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("interes", i, g_decInteres(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("saldoFinal", i, g_decSaldoFinal(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("intMora", i, g_decMoratorios(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("pagado", i, g_strPagado(i))
                dataTablePlan.SetValue("notaCred", i, g_strNotaCred(i))
                dataTablePlan.SetValue("docInt", i, g_strDocInt(i))
                dataTablePlan.SetValue("docFac", i, g_strDocFac(i))
                dataTablePlan.SetValue("borrador", i, g_strBorrador(i))
                dataTablePlan.SetValue("capPend", i, g_decCapPend(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("intPend", i, g_decIntPend(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("moraPend", i, g_decMoraPend(i).ToString(numberFormatInfo))
                dataTablePlan.SetValue("diasInt", i, g_intDiasInt(i))
                dataTablePlan.SetValue("diasMora", i, g_intDiasMora(i))

            Next

            MatrixPlanPagos.Matrix.LoadFromDataSource()

        Catch ex As Exception

            Throw ex

        End Try

    End Sub

    'Manejo de eventos de pantalla de plan de pagos

    Public Sub ApplicationSBOOnItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)

        If Not pVal.FormTypeEx = FormType Then Return

        If pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then

            If pVal.ItemUID = ButtonImprimirPlan.UniqueId Then

                ButtonSBOImprimirPlanItemPresed(FormUID, pVal, BubbleEvent)

            End If

        ElseIf pVal.EventType = BoEventTypes.et_FORM_CLOSE Then

            PlanPagosFormClose(FormUID, pVal)

        End If

    End Sub

End Class
