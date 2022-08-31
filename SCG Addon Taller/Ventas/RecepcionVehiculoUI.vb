Option Strict Off
Option Explicit On
Imports System.Collections.Generic
Imports SAPbouiCOM
Imports SCG.UX.Windows
Imports DMSOneFramework.SCGCommon
Imports System.Threading

'// BEFORE STARTING:
'// 1. Add reference to the "SAP Business One UI API"
'// 2. Insert the development connection string to the "Command line argument"
'//-----------------------------------------------------------------
'// 1.
'//    a. Project->Add Reference...
'//    b. select the "SAP Business One UI API 2005" From the COM folder
'//
'// 2.
'//     a. Project->Properties...
'//     b. choose Configuration Properties folder (place the arrow on Debugging)
'//     c. place the following connection string in the 'Command line arguments' field
'// 0030002C0030002C00530041005000420044005F00440061007400650076002C0050004C006F006D0056004900490056
'//
'//**************************************************************************************************


Friend Class RecepcionVehiculo

    '//**********************************************************
    '// This parameter will use us to manipulate the
    '// SAP Business One Application
    '//**********************************************************


    Private SBO_Application As SAPbouiCOM.Application
    Public Shared LabelReferencia As String = "19"

#Region "Globales"
    '    Private listaFormularios As Dictionary(Of Integer, FrmArchivoDigital) = New Dictionary(Of Integer, FrmArchivoDigital)()
#End Region

#Region "Declaraciones"

    'Contantes de Captions de los statics

    Friend Structure VehiculoUDT
        Public NoPlaca As String
        Public DescMArca As String
        Public DescModelo As String
        Public DescEstilo As String
        Public DescVehiculo As String
        Public Vin As String
        Public Año As String
        Public NoUnidad As String
        Public NumVehiculo As String
        Public CodMarca As String
        Public CodModelo As String
        Public CodEstilo As String
        Public KmUnidad As String
        Public HoraServicio As String
        Public GarantiaInicio As Date
        Public GarantiaFin As Date
        Public strCodeCliente As String
        Public strNameCliente As String
    End Structure

    Private Enum TipoPosicionControles
        Estandar = 1
        FacturaElectronica = 2
    End Enum

    'Constantes de nombres de los controles TAP
    Private Const mc_strfdRecepcion As String = "SCGD_FdN"

    'Contantes de nombres de los controles statics
    Private Const mc_strstOT As String = "SCGD_stOT"
    Private Const mc_strstNoUnidad As String = "SCGD_stNoU"
    Private Const mc_strstPlaca As String = "SCGD_stPla"
    Private Const mc_strstVIN As String = "SCGD_stVIN"
    Private Const mc_strstMarca As String = "SCGD_stMar"
    Private Const mc_strstModelo As String = "SCGD_stMod"
    Private Const mc_strstEstilo As String = "SCGD_stEst"
    Private Const mc_strstAño As String = "SCGD_stAño"
    Private Const mc_strstKilometraje As String = "SCGD_stKil"
    Private Const mc_strstGasolina As String = "SCGD_stGas"
    Private Const mc_strstFechaRecepción As String = "SCGD_stFec"
    Private Const mc_strstHoraRecepcion As String = "SCGD_stHor"
    Private Const mc_strstNoCita As String = "SCGD_stNCi"
    Private Const mc_strstNoSerie As String = "SCGD_stNSe"
    Private Const mc_strstGeneraOrden As String = "SCGD_stGOT"
    Private Const mc_strstRetorTaller As String = "SCGD_stRTa"
    Private Const mc_strstTipoOrdendeTrabajo As String = "SCGD_stTOT"
    Private Const mc_strstEstadoCotizacion As String = "SCGD_stSta"
    Private Const mc_strstNoVisita As String = "SCGD_stNVi"
    Private Const mc_strstArchivos As String = "SCGD_stArc"
    Private Const mc_strstHoraCompra As String = "SCGD_stHoC"
    Private Const mc_strstCono As String = "SCGD_stCon"
    Private Const mc_strstOTReferencia As String = "SCGD_stRef"
    Private Const mc_strstGeneraRecepcion As String = "SCGD_stRec"
    Private Const mc_strstFechadeCompromiso As String = "SCGD_stFeC"
    Private Const mc_strstNumProyecto As String = "SCGD_stPro"
    Private Const mc_strstNomClienteOT As String = "SCGD_stNOT"
    Private Const mc_strstCodClienteOT As String = "SCGD_stCOT"
    Private Const mc_strstSucursal As String = "SCGD_stSuc"
    Private Const mc_strstFecCreaOT As String = "SCGD_stFOT"
    Private Const mc_strstHorCreaOT As String = "SCGD_stHOT"
    Private Const mc_strstFechaCita As String = "SCGD_stFC"
    Private Const mc_strstHoraCita As String = "SCGD_stHC"
    Private Const mc_strstGaranIni As String = "SCGD_stGI"
    Private Const mc_strstGaranFin As String = "SCGD_stGF"

    'Contantes de nombres de los controles edittext
    Private txtLlamadaServicio As String = "SCGD_TxLS" 'llamada servicio en cot.. flecha naranja
    Private udfLLamadaSvc As String = "U_SCGD_LlSv"
    Private lblLLamadaSvc As String = "SCGD_lbLlS"

    Private Const mc_stretOT As String = "SCGD_etOT"
    Private Const mc_stretNoUnidad As String = "SCGD_etNoU"
    Private Const mc_stretPlaca As String = "SCGD_etPla"
    Private Const mc_stretVIN As String = "SCGD_etVIN"
    Private Const mc_stretMarca As String = "SCGD_etMar"
    Private Const mc_stretModelo As String = "SCGD_etMod"
    Private Const mc_stretEstilo As String = "SCGD_etEst"
    Private Const mc_stretAño As String = "SCGD_etAño"
    Private Const mc_stretKilometraje As String = "SCGD_etKil"
    Private Const mc_stretGasolina As String = "SCGD_etGas"
    Private Const mc_stretFechaRecepción As String = "SCGD_etFec"
    Private Const mc_stretHoraRecepcion As String = "SCGD_etHor"
    Private Const mc_stretNoVisita As String = "SCGD_etNVi"
    Private Const mc_stretHoraCompra As String = "SCGD_etHoC"
    Private Const mc_stretCono As String = "SCGD_etCon"
    Private Const mc_stretOTReferencia As String = "SCGD_etRef"
    Private Const mc_strcbGeneraRecepcion As String = "SCGD_cbRec"
    Private Const mc_stretFechadeCompromiso As String = "SCGD_etFeC"
    Private Const mc_stretNoCita As String = "SCGD_etNCi"
    Private Const mc_stretNoSerie As String = "SCGD_etNSe"
    Private Const mc_strbtArchivos As String = "SCGD_btArc"
    Private Const mc_strbtEstadoServicios As String = "SCGD_btEsS"
    Private Const mc_strbtHistorialVehiculo As String = "SCGD_btHiV"
    Private Const mc_strbtRecepcionVehi As String = "SCGD_btRe"
    Private Const mc_strbtBalanceOT As String = "SCGD_btBal"
    Private Const mc_stretProyectoNumero As String = "SCGD_etPro"
    Private Const mc_stretProyectoNombre As String = "SCGD_etPrN"
    Private Const mc_strCClienteOT As String = "SCGD_etCOT"
    Private Const mc_strNClienteOT As String = "SCGD_etNOT"
    Private Const mc_stretFecCreOT As String = "SCGD_etFOT"
    Private Const mc_stretHoraCreOT As String = "SCGD_etHOT"
    Private Const mc_stretFechaCita As String = "SCGD_etFC"
    Private Const mc_stretHoraCita As String = "SCGD_etHC"
    Private Const mc_strEtGaranIni As String = "SCGD_etGI"
    Private Const mc_strEtGaranFin As String = "SCGD_etGF"

    'Escondidos para guarad id's
    Private Const mc_stretidMarca As String = "SCGD_etIMa"
    Private Const mc_stretidModelo As String = "SCGD_etIMo"
    Private Const mc_stretidEstilo As String = "SCGD_etIEs"
    Private Const mc_stretIdasesor As String = "SCGD_etIAs"
    Private Const mc_stretidNoVehiculo As String = "SCGD_etIVe"

    'Archivos digitales
    Private Const mc_strTablaArchivosDigitales As String = "SCGTA_Archivos"

    'Linked Button
    Private Const mc_strLKBCliente As String = "SCGD_LKCli"
    Private Const mc_strLKBOT As String = "SCGD_LKOT"

    'Campos que son combos
    Private Const mc_strcbGeneraOrden As String = "SCGD_cbGOT"
    Private Const mc_strcbTipoOT As String = "SCGD_cbTOT"
    Private Const mc_strcbEstado As String = "SCGD_cbEst"
    Private Const mc_strcbSucursal As String = "SCGD_cbSuc"
    Private Const mc_strcbRetornoTaller As String = "SCGD_cbRTa"

    'Constante boton pic vehiculos(placa)
    Private Const mc_strbtpicNoPlaca As String = "SCGD_btpPl"
    Private Const mc_strbtpicRecepcion As String = "SCGD_btRec"
    Private Const mc_strpicbtDetalleVehiculos As String = "SCGD_btDVe"

    'Constante Tabla de Usuario
    Private Const mc_strOQUT As String = "OQUT"

    'Constantes Campos de Tabla de usuario de vehiculos
    Private Const mc_strUDFPlaca As String = "U_Num_Plac"
    Private Const mc_strUDFMarcaDesc As String = "U_Des_Marc"
    Private Const mc_strUDFModeloDesc As String = "U_Des_Mode"
    Private Const mc_strUDFEstiloDesc As String = "U_Des_Esti"
    Private Const mc_strUDFAñoVehiclo As String = "U_Ano_Vehi"
    Private Const mc_strUDFNoUnidad As String = "U_Cod_Unid"
    Private Const mc_strUDFVin As String = "U_Num_VIN"
    Private Const mc_strUDFNumVehi As String = "Code"
    Private Const mc_strUDFCodMarca As String = "U_Cod_Marc"
    Private Const mc_strUDFCodModelo As String = "U_Cod_Mode"
    Private Const mc_strUDFCodEstilo As String = "U_Cod_Esti"

    'Contantes Campos de Table de usuario de la Cotización
    Private Const mc_strUDFKilometraje As String = "U_SCGD_Kilometraje"
    Private Const mc_strUDFGasolina As String = "U_SCGD_Gasolina"
    Private Const mc_strUDFFech_Recep As String = "U_SCGD_Fech_Recep"
    Private Const mc_strUDFHora_Recep As String = "U_SCGD_Hora_Recep"
    Private Const mc_strUDFGorro_Veh As String = "U_SCGD_Gorro_Veh"
    Private Const mc_strUDFHora_Comp As String = "U_SCGD_Hora_Comp"
    Private Const mc_strUDFGenera_OT As String = "U_SCGD_Genera_OT"
    Private Const mc_strUDFRetornoTaller As String = "U_SCGD_RTaller"
    Private Const mc_strUDFEstado_Cot As String = "U_SCGD_Estado_Cot"
    Private Const mc_strUDFNo_Visita As String = "U_SCGD_No_Visita"
    Private Const mc_strUDFTipo_OT As String = "U_SCGD_Tipo_OT"
    Private Const mc_strUDNoOT As String = "U_SCGD_Numero_OT"
    Private Const mc_strUDFOTReferencia As String = "U_SCGD_NoOtRef"
    Private Const mc_strUDFGeneraRecepcion As String = "U_SCGD_GeneraOR"
    Private Const mc_strUDFFech_Comp As String = "U_SCGD_Fech_Comp"
    Private Const mc_strUDFNoCita As String = "U_SCGD_NoCita"
    Private Const mc_strUDFNoSerie As String = "U_SCGD_NoSerieCita"
    Private Const mc_strUDFCotEstiloDesc As String = "U_SCGD_Des_Esti"
    Private Const mc_strUDFCotAñoVehiclo As String = "U_SCGD_Ano_Vehi"
    Private Const mc_strUDFCotMarcaDesc As String = "U_SCGD_Des_Marc"
    Private Const mc_strUDFCotModeloDesc As String = "U_SCGD_Des_Mode"
    Private Const mc_strUDFCotVin As String = "U_SCGD_Num_VIN"
    Private Const mc_strUDFProyecto As String = "U_SCGD_Proyec"
    Private Const mc_strUDFProyectoNombre As String = "U_SCGD_ProNom"
    Private Const mc_strUDFKM_Unid As String = "U_Km_Unid"
    Private Const mc_strUDFCClienteOT As String = "U_SCGD_CCliOT"
    Private Const mc_strUDFNClienteOT As String = "U_SCGD_NCliOT"
    Private Const mc_strUDFFecCreaOT As String = "U_SCGD_Fech_CreaOT"
    Private Const mc_strUDFHorCreaOT As String = "U_SCGD_Hora_CreaOT"
    Private Const mc_strUDFFecCita As String = "U_SCGD_FechCita"
    Private Const mc_strUDFHorCita As String = "U_SCGD_HoraCita"
    Private Const mc_strUDFGaranIni As String = "U_SCGD_GaraIni"
    Private Const mc_strUDFGaranFin As String = "U_SCGD_GaraFin"
    Private Const mc_strUDFVehiGaranIni As String = "U_GaranIni"
    Private Const mc_strUDFVehiGaranFin As String = "U_GaranFin"

    'Constante Campos definidos por usuario OQUT
    Private Const mc_strUDFNumPlaca As String = "U_SCGD_Num_Placa"
    Private Const mc_strCodUnidad As String = "U_SCGD_Cod_Unidad"

    'Constantes para choosefromlist de proyectos
    Private Const mc_strCodeProyecto As String = "PrjCode"
    Private Const mc_strNameProyecto As String = "PrjName"

    'Constantes Campos del formulario que contiene los UDF
    Private Const mc_strUDF2CodModelo As String = "U_SCGD_Cod_Modelo"
    Private Const mc_strUDF2Num_Vehiculo As String = "U_SCGD_Num_Vehiculo"
    Private Const mc_strUDF2CodEstilo As String = "U_SCGD_Cod_Estilo"
    Private Const mc_strUDF2CodMarca As String = "U_SCGD_Cod_Marca"

    'Costantes de los campos definidos del usuario Empleados
    Private Const mc_strUDF2idEmpleado As String = "U_SCGD_Emp_Recibe"

    Private Const mc_strUDFidSucursal As String = "U_SCGD_idSucursal"

    Private Const mc_intidFormaCotizacion As Integer = 149
    Private Const mc_strIDBotonEjecucion As String = "1"

    Public Shared m_existe_datatablevehiculo As Boolean = False
    Public Shared blnEsChooseFromListrecepcion As Boolean = False

    Private WithEvents m_oVehiculo As VehiculosCls

    Private m_udtVehiculo As VehiculoUDT

    Private Const mc_strUDFHorasServicio As String = "U_SCGD_HoSr"
    Private Const mc_strUDFHorasServicioVH As String = "U_HorSer"
    Private Const mc_stretHoraServicio As String = "SCGD_etHS"
    Private Const mc_strstHorasServicio As String = "SCGD_stHoS"

    Private Const mc_stTipoPago As String = "stTipoPago"
    Private Const mc_stDptoSrv As String = "stDptoSrv"
    Private Const mc_strCboTipoPago As String = "cboTipPago"
    Private Const mc_strCboDptoSrv As String = "cboDptoSrv"

    Private Const mc_strUDFTipoPago As String = "U_SCGD_TipoPago"
    Private Const mc_strUDFServDpto As String = "U_SCGD_ServDpto"

    Private strUsaAvaluo As String

    Private Structure tabAvaluo
        Const strLabelTab As String = "SCGD_TabA"
        Const strEtNoAva As String = "SCGD_etNoA"
        Const strLbNoAva As String = "SCGD_stNoA"
        Const strEtNoCaso As String = "SCGD_etNoC"
        Const strLbNoCaso As String = "SCGD_stNoC"
        Const strEtNoPoli As String = "SCGD_etNoP"
        Const strLbNoPoli As String = "SCGD_stNoP"
        Const strEtRef1 As String = "SCGD_etRe1"
        Const strLbRef1 As String = "SCGD_stRe1"
        Const strEtRef2 As String = "SCGD_etRe2"
        Const strLbRef2 As String = "SCGD_stRe2"
        Const strEtMontAsegurado As String = "SCGD_etMoA"
        Const strLbMontAsegurado As String = "SCGD_stMoA"
        Const strEtInfra As String = "SCGD_etInf"
        Const strLbInfra As String = "SCGD_stInf"
        Const strEtWAN As String = "SCGD_etWAN"
        Const strLbWAN As String = "SCGD_stWAN"
        Const strEtPoolAsig As String = "SCGD_etPAs"
        Const strLbPoolAsig As String = "SCGD_stPAs"
        Const strEtCompOri As String = "SCGD_etCOr"
        Const strLbCompOri As String = "SCGD_stCOr"
        Const strEtVigI As String = "SCGD_etVgI"
        Const strLbVigI As String = "SCGD_stVgI"
        Const strEtVigF As String = "SCGD_etVgF"
        Const strLbVigF As String = "SCGD_stVgF"
        Const strCbCober As String = "SCGD_cbCob"
        Const strLbCober As String = "SCGD_stCob"
        Const strCbDedu As String = "SCGD_cbDed"
        Const strLbDedu As String = "SCGD_stDed"
        Const strCbAgencia As String = "SCGD_cbAge"
        Const strLbAgencia As String = "SCGD_stAge"
        Const strCbAcrePren As String = "SCGD_cbAcP"
        Const strLbAcrePren As String = "SCGD_stAcP"
        Const strCbCompS As String = "SCGD_cbCoS"
        Const strLbCompS As String = "SCGD_stCos"
        Const strCbPerito As String = "SCGD_cbPer"
        Const strLbPerito As String = "SCGD_stPer"
        Const strCbSupervisor As String = "SCGD_cbSup"
        Const strLbSupervisor As String = "SCGD_stSup"
        Const strCbAccidente As String = "SCGD_cbAcc"
        Const strLbAccidente As String = "SCGD_stAcc"
        Const strCbAvaluo As String = "SCGD_cbAva"
        Const strLbAvaluo As String = "SCGD_stAva"
        Const strEtHAva As String = "SCGD_cbHAv"
        Const strLbHAva As String = "SCGD_stHAv"
        Const strEtMontoAvaluo As String = "SCGD_cbMA"
        Const strLbMontoAvaluo As String = "SCGD_stMA"
        Const strEtDetalle As String = "SCGD_cbDe"
        Const strLbDetalle As String = "SCGD_stDe"

        'UDF
        Const strUDFNoAva As String = "U_SCGD_NoAva"
        Const strUDFNoCas As String = "U_SCGD_NoCas"
        Const strUDFNoPoli As String = "U_SCGD_NoPol"
        Const strUDFRef1 As String = "U_SCGD_ARef1"
        Const strUDFRef2 As String = "U_SCGD_ARef2"
        Const strUDFMontAseguradoro As String = "U_SCGD_MontA"
        Const strUDFInfra As String = "U_SCGD_Infra"
        Const strUDFWAN As String = "U_SCGD_WAN"
        Const strUDFCompOri As String = "U_SCGD_CompOri"
        Const strUDFPoolAsig As String = "U_SCGD_PoolAsig"
        Const strUDFVigI As String = "U_SCGD_VigI"
        Const strUDFVigF As String = "U_SCGD_VigF"
        Const strUDFCober As String = "U_SCGD_Cobe"
        Const strUDFDedu As String = "U_SCGD_Dedu"
        Const strUDFAgencia As String = "U_SCGD_AAge"
        Const strUDFAcrePren As String = "U_SCGD_AcreP"
        Const strUDFCompSeguros As String = "U_SCGD_CompS"
        Const strUDFPerito As String = "U_SCGD_Peri"
        Const strUDFSupervisor As String = "U_SCGD_Super"
        Const strUDFAccidente As String = "U_SCGD_Acci"
        Const strUDFAvaluo As String = "U_SCGD_Ava"
        Const strUDFHorasAvaluo As String = "U_SCGD_AHora"
        Const strUDFMontoAvaluo As String = "U_SCGD_AMonto"
        Const strUDFAvaluoDetalle As String = "U_SCGD_ADet"

    End Structure

#End Region

    Private Shared m_ocompany As SAPbobsCOM.Company
    Private oPosicionControles As Dictionary(Of String, Coordenadas)


    Public Sub New(ByVal p_SBO_Application As SAPbouiCOM.Application, _
                    ByVal ocompany As SAPbobsCOM.Company)

        SBO_Application = p_SBO_Application
        m_ocompany = ocompany

        strUsaAvaluo = IIf(Not String.IsNullOrEmpty(DMS_Connector.Configuracion.ParamGenAddon.U_UsaTAva), DMS_Connector.Configuracion.ParamGenAddon.U_UsaTAva, "N")
        InicializarPosicionControles()
    End Sub

    ''' <summary>
    ''' Guarda la posición de todos los controles de la oferta de ventas en un objeto Diccionario
    ''' </summary>
    ''' <remarks>Ejemplo de como agregar las coordenadas de un control:
    ''' oPosicionControles.Add("IDControl", New Coordenadas(Left, Top))</remarks>
    Private Sub InicializarPosicionControles()
        Try
            'Instancia un objeto diccionario
            'la llave corresponde al ID único del control y el valor es un objeto que contiene las coordenadas
            oPosicionControles = New Dictionary(Of String, Coordenadas)
            'Controles del Encabezado
            InicializarPosicionesControlesEncabezado()
            InicializarPosicionesRecepcionPorGrupos()
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub InicializarPosicionesRecepcionPorGrupos()
        Try
            'Controles de la pestaña Recepción

            'Columna 1
            oPosicionControles.Add("SCGD_stPla", New Coordenadas(10, 165)) 'StaticText Placa
            oPosicionControles.Add("SCGD_btDVe", New Coordenadas(77, 161)) 'LinkButton Placa
            oPosicionControles.Add("SCGD_etPla", New Coordenadas(95, 165)) 'EditText Placa
            oPosicionControles.Add("SCGD_btpPl", New Coordenadas(178, 161)) 'Botón Placa
            oPosicionControles.Add("SCGD_stNoU", New Coordenadas(10, 180)) 'StaticText Número de Unidad
            oPosicionControles.Add("SCGD_etNoU", New Coordenadas(95, 180)) 'EditText Número de Unidad
            oPosicionControles.Add("SCGD_stVIN", New Coordenadas(10, 195)) 'StaticText Chasis
            oPosicionControles.Add("SCGD_etVIN", New Coordenadas(95, 195)) 'EditText Chasis
            oPosicionControles.Add("SCGD_stAño", New Coordenadas(10, 210)) 'StaticText Año
            oPosicionControles.Add("SCGD_etAño", New Coordenadas(95, 210)) 'EditText Año
            oPosicionControles.Add("SCGD_stNVi", New Coordenadas(10, 235)) 'StaticText Número de Visita
            oPosicionControles.Add("SCGD_etNVi", New Coordenadas(95, 235)) 'EditText Número de Visita
            oPosicionControles.Add("SCGD_btRec", New Coordenadas(178, 231)) 'Botón Número de Visita
            oPosicionControles.Add("SCGD_stNSe", New Coordenadas(10, 250)) 'StaticText Número de Cita
            oPosicionControles.Add("SCGD_etNSe", New Coordenadas(95, 250)) 'EditText Número de Cita Parte A
            oPosicionControles.Add("SCGD_etNCi", New Coordenadas(148, 250)) 'EditText Número de Cita Parte B
            oPosicionControles.Add("SCGD_stFC", New Coordenadas(10, 265)) 'StaticText Fecha de la Cita
            oPosicionControles.Add("SCGD_etFC", New Coordenadas(95, 265)) 'EditText Fecha de la Cita
            oPosicionControles.Add("SCGD_etHC", New Coordenadas(148, 265)) 'EditText Hora de la Cita
            oPosicionControles.Add("SCGD_stGOT", New Coordenadas(10, 293)) 'StaticText Genera Orden de Trabajo
            oPosicionControles.Add("SCGD_cbGOT", New Coordenadas(95, 293)) 'ComboBox Genera Orden de Trabajo
            oPosicionControles.Add("SCGD_stRec", New Coordenadas(10, 308)) 'StaticText Imprimir Orden de Trabajo
            oPosicionControles.Add("SCGD_cbRec", New Coordenadas(95, 308)) 'ComboBox Imprimir Orden de Trabajo
            oPosicionControles.Add("SCGD_stTOT", New Coordenadas(10, 323)) 'StaticText Tipo de Orden
            oPosicionControles.Add("SCGD_cbTOT", New Coordenadas(95, 323)) 'ComboBox Tipo de Orden
            oPosicionControles.Add("SCGD_stSta", New Coordenadas(10, 338)) 'StaticText Estado de la Orden de Trabajo
            oPosicionControles.Add("SCGD_cbEst", New Coordenadas(95, 338)) 'ComboBox Estado de la Orden de Trabajo

            'Columna 2
            oPosicionControles.Add("SCGD_stMar", New Coordenadas(200, 165)) 'StaticText Marca
            oPosicionControles.Add("SCGD_etMar", New Coordenadas(285, 165)) 'EditText Marca
            oPosicionControles.Add("SCGD_stEst", New Coordenadas(200, 180)) 'StaticText Estilo
            oPosicionControles.Add("SCGD_etEst", New Coordenadas(285, 180)) 'EditText Estilo
            oPosicionControles.Add("SCGD_stMod", New Coordenadas(200, 195)) 'StaticText Modelo
            oPosicionControles.Add("SCGD_etMod", New Coordenadas(285, 195)) 'EditText Modelo
            'oPosicionControles.Add("SCGD_stm2", New Coordenadas(200, 210)) 'StaticText Metros Cuadrados
            'oPosicionControles.Add("SCGD_etm2", New Coordenadas(285, 210)) 'EditText Metros Cuadrados
            oPosicionControles.Add("SCGD_stFec", New Coordenadas(200, 235)) 'StaticText Fecha Recibida
            oPosicionControles.Add("SCGD_etFec", New Coordenadas(285, 235)) 'EditText Fecha Recibida
            oPosicionControles.Add("SCGD_etHor", New Coordenadas(338, 235)) 'EditText Hora de Recepción
            oPosicionControles.Add("SCGD_stFeC", New Coordenadas(200, 250)) 'StaticText Fecha Compromiso
            oPosicionControles.Add("SCGD_etFeC", New Coordenadas(285, 250)) 'EditText Fecha Compromiso
            oPosicionControles.Add("SCGD_etHoC", New Coordenadas(338, 250)) 'EditText Hora de Compromiso
            oPosicionControles.Add("SCGD_stFOT", New Coordenadas(200, 265)) 'StaticText Fecha Orden de Trabajo
            oPosicionControles.Add("SCGD_etFOT", New Coordenadas(285, 265)) 'EditText Fecha Orden de Trabajo
            oPosicionControles.Add("SCGD_etHOT", New Coordenadas(338, 265)) 'EditText Hora de la Orden de Trabajo
            oPosicionControles.Add("SCGD_lbLlS", New Coordenadas(200, 293)) 'StaticText Llamada de Servicio
            oPosicionControles.Add("SCGD_TxLS", New Coordenadas(285, 293)) 'EditText Llamada de Servicio
            oPosicionControles.Add("SCGD_stPro", New Coordenadas(200, 308)) 'StaticText Proyecto
            oPosicionControles.Add("SCGD_etPrN", New Coordenadas(285, 308)) 'EditText Proyecto
            'oPosicionControles.Add("SCGD_stApA", New Coordenadas(200, 323)) 'StaticText Aprobación de Seguro
            'oPosicionControles.Add("SCGD_cbApA", New Coordenadas(285, 323)) 'ComboBox Aprobación de Seguro
            'oPosicionControles.Add("SCGD_stUbT", New Coordenadas(200, 338)) 'StaticText Ubicación
            'oPosicionControles.Add("SCGD_cbUbT", New Coordenadas(285, 338)) 'EditText Ubicación

            'Columna 3
            oPosicionControles.Add("SCGD_stKil", New Coordenadas(389, 165)) 'StaticText Kilometraje
            oPosicionControles.Add("SCGD_etKil", New Coordenadas(475, 165)) 'EditText Kilometraje

            oPosicionControles.Add("SCGD_stCon", New Coordenadas(389, 180)) 'StaticText Cono
            oPosicionControles.Add("SCGD_etCon", New Coordenadas(475, 180)) 'EditText Cono

            oPosicionControles.Add("SCGD_stGas", New Coordenadas(389, 195)) 'StaticText Combustible
            oPosicionControles.Add("SCGD_etGas", New Coordenadas(475, 195)) 'EditText Combustible

            'oPosicionControles.Add("SCGD_stCPa", New Coordenadas(389, 210)) 'StaticText Cantidad Paneles
            'oPosicionControles.Add("SCGD_etCPa", New Coordenadas(475, 210)) 'EditText Cantidad Paneles

            oPosicionControles.Add("SCGD_stHoS", New Coordenadas(389, 235)) 'StaticText Horas Servicio
            oPosicionControles.Add("SCGD_etHS", New Coordenadas(475, 235)) 'EditText Horas Servicio

            oPosicionControles.Add("SCGD_stGI", New Coordenadas(389, 250)) 'StaticText Garantía Desde
            oPosicionControles.Add("SCGD_etGI", New Coordenadas(475, 250)) 'EditText Garantía Desde

            oPosicionControles.Add("SCGD_stGF", New Coordenadas(389, 265)) 'StaticText Garantía Hasta
            oPosicionControles.Add("SCGD_etGF", New Coordenadas(475, 265)) 'EditText Garantía Hasta

            oPosicionControles.Add("SCGD_stRef", New Coordenadas(389, 293)) 'StaticText Referencia Orden de Trabajo
            oPosicionControles.Add("SCGD_etRef", New Coordenadas(475, 293)) 'EditText Referencia Orden de Trabajo

            oPosicionControles.Add("SCGD_stRTa", New Coordenadas(389, 308)) 'StaticText Retorna Taller
            oPosicionControles.Add("SCGD_cbRTa", New Coordenadas(475, 308)) 'EditText Retorna Taller

            'oPosicionControles.Add("SCGD_lbTO", New Coordenadas(389, 323)) 'StaticText Tiempo Otorgado
            'oPosicionControles.Add("SCGD_etTO", New Coordenadas(475, 323)) 'EditText Tiempo Otorgado
            'oPosicionControles.Add("SCGD_btTO", New Coordenadas(557, 323)) 'Botón Tiempo Otorgado

            'oPosicionControles.Add("SCGD_stCtr", New Coordenadas(389, 340)) 'StaticText Control de Procesos
            'oPosicionControles.Add("SCGD_btCP", New Coordenadas(475, 340)) 'Botón Control de Procesos

            oPosicionControles.Add("SCGD_stArc", New Coordenadas(389, 360)) 'StaticText Adjuntos
            oPosicionControles.Add("SCGD_btArc", New Coordenadas(475, 360)) 'Botón Adjuntos
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub InicializarPosicionesControlesEncabezado()
        Dim strPosicionCampos As String = String.Empty
        Try
            strPosicionCampos = DMS_Connector.Configuracion.ParamGenAddon.U_FieldsPosition
            If String.IsNullOrEmpty(strPosicionCampos) Then
                strPosicionCampos = TipoPosicionControles.Estandar
            End If
            Select Case strPosicionCampos
                Case TipoPosicionControles.Estandar
                    oPosicionControles.Add("SCGD_stCOT", New Coordenadas(6, 80)) 'StaticText Cliente OT
                    oPosicionControles.Add("SCGD_etCOT", New Coordenadas(127, 80)) 'EditText Cliente OT
                    oPosicionControles.Add("btnSN", New Coordenadas(278, 78)) 'Botón choose from list Cliente OT
                    oPosicionControles.Add("SCGD_LKCli", New Coordenadas(114, 82)) 'LinkButton Cliente OT
                    oPosicionControles.Add("SCGD_stNOT", New Coordenadas(6, 95)) 'StaticText Nombre Cliente
                    oPosicionControles.Add("SCGD_etNOT", New Coordenadas(127, 95)) 'EditText Nombre Cliente
                Case TipoPosicionControles.FacturaElectronica
                    oPosicionControles.Add("SCGD_stCOT", New Coordenadas(301, 5)) 'StaticText Cliente OT
                    oPosicionControles.Add("SCGD_etCOT", New Coordenadas(422, 5)) 'EditText Cliente OT
                    oPosicionControles.Add("btnSN", New Coordenadas(571, 5)) 'Botón choose from list Cliente OT
                    oPosicionControles.Add("SCGD_LKCli", New Coordenadas(409, 5)) 'LinkButton Cliente OT
                    oPosicionControles.Add("SCGD_stNOT", New Coordenadas(301, 20)) 'StaticText Nombre Cliente
                    oPosicionControles.Add("SCGD_etNOT", New Coordenadas(422, 20)) 'EditText Nombre Cliente
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Sub FormResizeEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            oFormulario = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
            If pVal.BeforeAction Then
                'Implementar manejo del BeforeAction = false aquí
            Else
                AjustarPosicionControles(oFormulario)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AjustarPosicionControles(ByRef oFormulario As SAPbouiCOM.Form)
        Try
            If Not oFormulario Is Nothing Then
                Select Case oFormulario.TypeEx
                    Case "149" 'Oferta de Ventas
                        ReposicionarControlesOfertaVentas(oFormulario)
                        'AjustarControlesOfertaVentas(oFormulario)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ReposicionarControlesOfertaVentas(ByRef oFormulario As SAPbouiCOM.Form)
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim blnUsaInterfazFord As Boolean = False
        Dim XML As String = String.Empty
        Dim DocumentoXML As Xml.XmlDataDocument
        Dim Nodo As Xml.XmlNode
        Dim InnerXML As String = String.Empty
        Try
            DocumentoXML = New Xml.XmlDataDocument
            XML = My.Resources.Resource.XMLReposicion
            DocumentoXML.LoadXml(XML)

            'Se debe actualizar el UID en el XML para que coincida con el formulario en el cual vamos agregar los controles
            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form")
            Nodo.Attributes.ItemOf("uid").Value = oFormulario.UniqueID
            Nodo = DocumentoXML.SelectSingleNode("/Application/forms/action/form/items/action")

            'Controles con posición fija en el formulario
            For Each oPosicion As KeyValuePair(Of String, Coordenadas) In oPosicionControles
                If Not String.IsNullOrEmpty(oPosicion.Key) Then
                    CrearNodoHijo(DocumentoXML, Nodo, oPosicion.Key, oPosicion.Value.Left, oPosicion.Value.Top)
                End If
            Next

            'Controles con posición relativa
            'que requieren adaptarse respecto a otros controles

            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            'StaticText No OT
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_stOT", intLeft, intTop + 15)

            'LinkButton No OT
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_LKOT", intLeft + 104, intTop + 15)

            'EditText No OT
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_etOT", intLeft + 120, intTop + 15)

            'StaticText Sucursal
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_stSuc", intLeft, intTop + 30)

            'ComboBox Sucursal
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_cbSuc", intLeft + 120, intTop + 30)

            'Verifica si utiliza la interfaz de Ford
            blnUsaInterfazFord = Utilitarios.UsaInterfazFord(m_ocompany)

            If blnUsaInterfazFord Then
                ReposicionarControlesInterfazFord(oFormulario, DocumentoXML, Nodo)
            End If

            'Obtiene la posición relativa del EditText Comentarios
            intTop = oFormulario.Items.Item("16").Top
            intLeft = oFormulario.Items.Item("16").Left

            'Controles del Área Inferior

            'Botón Asignación Múltiple
            CrearNodoHijo(DocumentoXML, Nodo, "btnAsM", intLeft + 150, intTop + 18)

            'Botón Recepción de Vehículo
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_btRe", intLeft + 217, intTop + 18)

            'Botón Historial Vehículo
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_btHiV", intLeft + 150, intTop + 39)

            'Botón Balance OT
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_btBal", intLeft + 217, intTop + 39)

            'Botón Estado OT
            CrearNodoHijo(DocumentoXML, Nodo, "SCGD_btEsS", intLeft + 284, intTop + 39)

            If Utilitarios.MostrarMenu("SCGD_SOE", SBO_Application.Company.UserName) Then
                'Botón Solicitud OT Especial
                CrearNodoHijo(DocumentoXML, Nodo, "btnSotE", intLeft + 284, intTop + 18)
            End If

            'Actualiza la oferta de ventas con base al XML
            InnerXML = DocumentoXML.InnerXml
            DMS_Connector.Company.ApplicationSBO.LoadBatchActions(InnerXML)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub ReposicionarControlesInterfazFord(ByRef oFormulario As SAPbouiCOM.Form, ByRef DocumentoXML As Xml.XmlDataDocument, ByRef NodoPadre As Xml.XmlNode)
        Dim strPosicionCampos As String = String.Empty
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Try
            strPosicionCampos = DMS_Connector.Configuracion.ParamGenAddon.U_FieldsPosition
            If String.IsNullOrEmpty(strPosicionCampos) Then
                strPosicionCampos = TipoPosicionControles.Estandar
            End If

            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            Select Case strPosicionCampos
                Case TipoPosicionControles.Estandar
                    'StaticText Tipo de Pago
                    CrearNodoHijo(DocumentoXML, NodoPadre, "stTipoPago", 6, 110)

                    'ComboBox Tipo de Pago
                    CrearNodoHijo(DocumentoXML, NodoPadre, "cboTipPago", 127, 110)
                Case TipoPosicionControles.FacturaElectronica
                    'StaticText Tipo de Pago
                    CrearNodoHijo(DocumentoXML, NodoPadre, "stTipoPago", 301, 35)

                    'ComboBox Tipo de Pago
                    CrearNodoHijo(DocumentoXML, NodoPadre, "cboTipPago", 422, 35)
            End Select

            'StaticText Departamento de Servicio
            CrearNodoHijo(DocumentoXML, NodoPadre, "stDptoSrv", intLeft, intTop + 45)

            'ComboBox Departamento de Servicio
            CrearNodoHijo(DocumentoXML, NodoPadre, "cboDptoSrv", intLeft + 120, intTop + 45)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CrearNodoHijo(ByRef DocumentoXML As Xml.XmlDataDocument, ByRef NodoPadre As Xml.XmlNode, ByVal UID As String, ByVal Left As Integer, ByVal Top As Integer)
        Dim NodoHijo As Xml.XmlNode
        Dim Atributo As Xml.XmlAttribute
        Try
            NodoHijo = DocumentoXML.CreateElement("item")

            'UID
            Atributo = DocumentoXML.CreateAttribute("uid")
            Atributo.Value = UID
            NodoHijo.Attributes.Append(Atributo)
            'Left
            Atributo = DocumentoXML.CreateAttribute("left")
            Atributo.Value = Left
            NodoHijo.Attributes.Append(Atributo)
            'Top
            Atributo = DocumentoXML.CreateAttribute("top")
            Atributo.Value = Top
            NodoHijo.Attributes.Append(Atributo)

            NodoPadre.AppendChild(NodoHijo)
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AjustarControlesOfertaVentas(ByRef oFormulario As SAPbouiCOM.Form)
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim blnUsaInterfazFord As Boolean = False

        Try
            'Controles con posición fija en el formulario
            For Each oPosicion As KeyValuePair(Of String, Coordenadas) In oPosicionControles
                If Not String.IsNullOrEmpty(oPosicion.Key) Then
                    oFormulario.Items.Item(oPosicion.Key).Left = oPosicion.Value.Left
                    oFormulario.Items.Item(oPosicion.Key).Top = oPosicion.Value.Top
                End If
            Next

            'Controles con posición relativa
            'que requieren adaptarse respecto a otros controles

            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            'StaticText No OT
            oFormulario.Items.Item("SCGD_stOT").Top = intTop + 15
            oFormulario.Items.Item("SCGD_stOT").Left = intLeft

            'LinkButton No OT
            oFormulario.Items.Item("SCGD_LKOT").Top = intTop + 15
            oFormulario.Items.Item("SCGD_LKOT").Left = intLeft + 104

            'EditText No OT
            oFormulario.Items.Item("SCGD_etOT").Top = intTop + 15
            oFormulario.Items.Item("SCGD_etOT").Left = intLeft + 120

            'StaticText Sucursal
            oFormulario.Items.Item("SCGD_stSuc").Top = intTop + 30
            oFormulario.Items.Item("SCGD_stSuc").Left = intLeft

            'ComboBox Sucursal
            oFormulario.Items.Item("SCGD_cbSuc").Top = intTop + 30
            oFormulario.Items.Item("SCGD_cbSuc").Left = intLeft + 120

            'Verifica si utiliza la interfaz de Ford
            blnUsaInterfazFord = Utilitarios.UsaInterfazFord(m_ocompany)

            If blnUsaInterfazFord Then
                AjustarControlesInterfazFord(oFormulario)
            End If

            'Obtiene la posición relativa del EditText Comentarios
            intTop = oFormulario.Items.Item("16").Top
            intLeft = oFormulario.Items.Item("16").Left

            'Controles del Área Inferior

            'Botón Asignación Múltiple
            oFormulario.Items.Item("btnAsM").Top = intTop + 18
            oFormulario.Items.Item("btnAsM").Left = intLeft + 150

            'Botón Recepción de Vehículo
            oFormulario.Items.Item("SCGD_btRe").Top = intTop + 18
            oFormulario.Items.Item("SCGD_btRe").Left = intLeft + 217

            If Utilitarios.MostrarMenu("SCGD_SOE", SBO_Application.Company.UserName) Then
                'Botón Solicitud OT Especial
                oFormulario.Items.Item("btnSotE").Top = intTop + 18
                oFormulario.Items.Item("btnSotE").Left = intLeft + 284
            End If

            'Botón Historial Vehículo
            oFormulario.Items.Item("SCGD_btHiV").Top = intTop + 39
            oFormulario.Items.Item("SCGD_btHiV").Left = intLeft + 150

            'Botón Balance OT
            oFormulario.Items.Item("SCGD_btBal").Top = intTop + 39
            oFormulario.Items.Item("SCGD_btBal").Left = intLeft + 217

            'Botón Estado OT
            oFormulario.Items.Item("SCGD_btEsS").Top = intTop + 39
            oFormulario.Items.Item("SCGD_btEsS").Left = intLeft + 284
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub AjustarControlesInterfazFord(ByRef oFormulario As SAPbouiCOM.Form)
        Dim strPosicionCampos As String = String.Empty
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Try
            strPosicionCampos = DMS_Connector.Configuracion.ParamGenAddon.U_FieldsPosition
            If String.IsNullOrEmpty(strPosicionCampos) Then
                strPosicionCampos = TipoPosicionControles.Estandar
            End If

            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            Select Case strPosicionCampos
                Case TipoPosicionControles.Estandar
                    'StaticText Tipo de Pago
                    oFormulario.Items.Item("stTipoPago").Top = 110
                    oFormulario.Items.Item("stTipoPago").Left = 6

                    'ComboBox Tipo de Pago
                    oFormulario.Items.Item("cboTipPago").Top = 110
                    oFormulario.Items.Item("cboTipPago").Left = 127
                Case TipoPosicionControles.FacturaElectronica
                    'StaticText Tipo de Pago
                    oFormulario.Items.Item("stTipoPago").Top = 35
                    oFormulario.Items.Item("stTipoPago").Left = 301

                    'ComboBox Tipo de Pago
                    oFormulario.Items.Item("cboTipPago").Top = 35
                    oFormulario.Items.Item("cboTipPago").Left = 422
            End Select

            'StaticText Departamento de Servicio
            oFormulario.Items.Item("stDptoSrv").Top = intTop + 45
            oFormulario.Items.Item("stDptoSrv").Left = intLeft

            'ComboBox Departamento de Servicio
            oFormulario.Items.Item("cboDptoSrv").Top = intTop + 45
            oFormulario.Items.Item("cboDptoSrv").Left = intLeft + 120
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Public Shared Sub AgregarControlesDocumentos(ByRef oFormulario As SAPbouiCOM.Form)
        Try
            If Not oFormulario Is Nothing Then
                Select Case oFormulario.TypeEx
                    Case "540000988", "142", "60092" 'Documentos de compras
                        AgregarControlesDocumentosCompras(oFormulario)
                    Case "133", "139", "140", "180", "65304", "60090", "60091" 'Documentos de Ventas
                        AgregarControlesDocumentosVentas(oFormulario)
                    Case "940" 'Documentos de ajuste
                        AgregarControlesDocumentosAjuste(oFormulario)
                End Select
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Shared Sub AgregarControlesDocumentosAjuste(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oLabel As SAPbouiCOM.StaticText
        Dim oEditText As SAPbouiCOM.EditText
        Dim oButton As SAPbouiCOM.Button
        Dim oCheck As SAPbouiCOM.CheckBox
        Dim oLinkButton As LinkedButton
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim strTablaEncabezado As String = String.Empty

        Try
            'Busca la tabla de acuerdo al formulario abierto
            ObtenerTablaPorDocumento(oFormulario, strTablaEncabezado)

            'Controles con posición relativa
            'que requieren adaptarse respecto a otros controles

            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("17").Top
            intLeft = oFormulario.Items.Item("17").Left

            'StaticText Número de OT
            oLabel = oFormulario.Items.Add("SCGD_stOT", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
            oLabel.Item.Top = intTop + 15
            oLabel.Item.Left = intLeft
            oLabel.Caption = My.Resources.Resource.CapNoOrdenTrabajo

            'EditText Número de OT
            oEditText = oFormulario.Items.Add("SCGD_etOT", SAPbouiCOM.BoFormItemTypes.it_EDIT).Specific
            oEditText.Item.Top = intTop + 15
            oEditText.Item.Left = intLeft + 115
            oEditText.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oEditText.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_Numero_OT")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Shared Sub AgregarControlesDocumentosCompras(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oLabel As SAPbouiCOM.StaticText
        Dim oEditText As SAPbouiCOM.EditText
        Dim oButton As SAPbouiCOM.Button
        Dim oCheck As SAPbouiCOM.CheckBox
        Dim oLinkButton As LinkedButton
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim strTablaEncabezado As String = String.Empty

        Try
            'Busca la tabla de acuerdo al formulario abierto
            ObtenerTablaPorDocumento(oFormulario, strTablaEncabezado)

            'Controles con posición relativa
            'que requieren adaptarse respecto a otros controles

            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            'StaticText Número de OT
            oLabel = oFormulario.Items.Add("SCGD_stOT", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
            oLabel.Item.Top = intTop + 30
            oLabel.Item.Left = intLeft
            oLabel.Caption = My.Resources.Resource.CapNoOrdenTrabajo

            'EditText Número de OT
            oEditText = oFormulario.Items.Add("SCGD_etOT", SAPbouiCOM.BoFormItemTypes.it_EDIT).Specific
            oEditText.Item.Top = intTop + 30
            oEditText.Item.Left = intLeft + 120
            oEditText.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oEditText.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_Numero_OT")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Shared Sub AgregarControlesDocumentosVentas(ByRef oFormulario As SAPbouiCOM.Form)
        Dim oLabel As SAPbouiCOM.StaticText
        Dim oEditText As SAPbouiCOM.EditText
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim oButton As SAPbouiCOM.Button
        Dim oCheck As SAPbouiCOM.CheckBox
        Dim oLinkButton As LinkedButton
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim strTablaEncabezado As String = String.Empty
        Dim blnUsaInterfazFord As Boolean = False

        Try
            'Busca la tabla de acuerdo al formulario abierto
            ObtenerTablaPorDocumento(oFormulario, strTablaEncabezado)

            'Controles con posición relativa
            'que requieren adaptarse respecto a otros controles

            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            'StaticText Número de OT
            oLabel = oFormulario.Items.Add("SCGD_stOT", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
            oLabel.Item.Top = intTop + 15
            oLabel.Item.Left = intLeft
            oLabel.Caption = My.Resources.Resource.CapNoOrdenTrabajo

            'EditText Número de OT
            oEditText = oFormulario.Items.Add("SCGD_etOT", SAPbouiCOM.BoFormItemTypes.it_EDIT).Specific
            oEditText.Item.Top = intTop + 15
            oEditText.Item.Left = intLeft + 120
            oEditText.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            oEditText.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_Numero_OT")

            'Si el documento es orden de venta o factura de ventas
            If oFormulario.TypeEx = "139" Or oFormulario.TypeEx = "133" Then
                '----------------------------------------------
                'Controles con posición fija
                '----------------------------------------------
                'StaticText Código del Cliente
                oLabel = oFormulario.Items.Add("SCGD_stCOT", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
                oLabel.Caption = My.Resources.Resource.CapCodClienOT
                oLabel.Item.Top = 5
                oLabel.Item.Left = 301
                oLabel.Item.Width = 80

                'EditText Código del Cliente
                oEditText = oFormulario.Items.Add("SCGD_etCOT", SAPbouiCOM.BoFormItemTypes.it_EDIT).Specific
                oEditText.Item.Top = 5
                oEditText.Item.Left = 422
                oEditText.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEditText.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_CCliOT")
                oEditText.Item.Width = 148

                'LinkButton Código del Cliente
                oLinkButton = oFormulario.Items.Add("SCGD_LKCli", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON).Specific
                oLinkButton.Item.Top = 5
                oLinkButton.Item.Left = 409
                oLinkButton.Item.LinkTo = "SCGD_etCOT"
                oLinkButton.LinkedObjectType = "2" 'Socios de Negocios
                oLinkButton.Item.Width = 12
                oLinkButton.Item.Height = 10

                'StaticText Nombre del Cliente
                oLabel = oFormulario.Items.Add("SCGD_stNOT", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
                oLabel.Caption = My.Resources.Resource.CapNomClienOT
                oLabel.Item.Top = 20
                oLabel.Item.Left = 301
                oLabel.Item.Width = 100

                'EditText Nombre del Cliente
                oEditText = oFormulario.Items.Add("SCGD_etNOT", SAPbouiCOM.BoFormItemTypes.it_EDIT).Specific
                oEditText.Item.Top = 20
                oEditText.Item.Left = 422
                oEditText.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEditText.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_NCliOT")
                oEditText.Item.Width = 148

                'Verifica si utiliza la interfaz de Ford
                blnUsaInterfazFord = Utilitarios.UsaInterfazFord(m_ocompany)

                If blnUsaInterfazFord Then
                    AgregarControlesInterfazFord(oFormulario, strTablaEncabezado)
                End If
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene el nombre de la tabla de encabezado de acuerdo al formulario abierto
    ''' </summary>
    ''' <param name="oFormulario">Objeto formulario de SAP</param>
    ''' <param name="strTablaEncabezado">Tabla del encabezado en formato string</param>
    ''' <remarks></remarks>
    Private Shared Sub ObtenerTablaPorDocumento(ByRef oFormulario As SAPbouiCOM.Form, ByRef strTablaEncabezado As String)
        Try
            Select Case oFormulario.TypeEx
                Case "540000988" 'Oferta de Compras
                    strTablaEncabezado = "OPQT"
                Case "133" 'Factura de Ventas
                    strTablaEncabezado = "OINV"
                Case "139" 'Orden de Venta
                    strTablaEncabezado = "ORDR"
                Case "140" 'Entrega de Mercancías Ventas
                    strTablaEncabezado = "ODLN"
                Case "142" 'Orden de Compra
                    strTablaEncabezado = "OPOR"
                Case "180" 'Devolución de Mercancías Ventas
                    strTablaEncabezado = "ORDN"
                Case "65304" 'Boleta de Ventas
                    strTablaEncabezado = "OINV"
                Case "60090" 'Factura de Ventas + Pago
                    strTablaEncabezado = "OINV"
                Case "60092" 'Factura de Reserva Compras
                    strTablaEncabezado = "OPCH"
                Case "60091" 'Factura de Reserva Ventas
                    strTablaEncabezado = "OINV"
                Case "940" 'Transferencia de Stock
                    strTablaEncabezado = "OWTR"
            End Select
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Shared Sub AgregarControlesInterfazFord(ByRef oFormulario As SAPbouiCOM.Form, ByVal strTablaEncabezado As String)
        Dim oLabel As SAPbouiCOM.StaticText
        Dim oEditText As SAPbouiCOM.EditText
        Dim oComboBox As SAPbouiCOM.ComboBox
        Dim intTop As Integer = 0
        Dim intLeft As Integer = 0
        Dim strPosicionCampos As String = String.Empty
        Try
            'Obtiene la posición relativa del StaticText fecha de documento para usarlo como referencia
            intTop = oFormulario.Items.Item("86").Top
            intLeft = oFormulario.Items.Item("86").Left

            strPosicionCampos = DMS_Connector.Configuracion.ParamGenAddon.U_FieldsPosition
            If String.IsNullOrEmpty(strPosicionCampos) Then
                strPosicionCampos = TipoPosicionControles.Estandar
            End If

            'StaticText Tipo de Pago
            oLabel = oFormulario.Items.Add("stTipoPago", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
            oLabel.Caption = My.Resources.Resource.TXTTipoPago
            oLabel.Item.Top = 110
            oLabel.Item.Left = 6
            oLabel.Item.Width = 100
            'ComboBox Tipo de Pago
            oComboBox = oFormulario.Items.Add("cboTipPago", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX).Specific
            oComboBox.Item.Top = 110
            oComboBox.Item.Left = 127
            oComboBox.Item.Width = 148
            oComboBox.Item.DisplayDesc = True
            oComboBox.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oComboBox.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_TipoPago")

            Select Case strPosicionCampos
                Case TipoPosicionControles.Estandar
                    oLabel.Item.Top = 110
                    oLabel.Item.Left = 6
                    oComboBox.Item.Top = 110
                    oComboBox.Item.Left = 127
                Case TipoPosicionControles.FacturaElectronica
                    oLabel.Item.Top = 35
                    oLabel.Item.Left = 301
                    oComboBox.Item.Top = 35
                    oComboBox.Item.Left = 422
            End Select

            'StaticText Departamento de Servicio
            oLabel = oFormulario.Items.Add("stDptoSrv", SAPbouiCOM.BoFormItemTypes.it_STATIC).Specific
            oLabel.Caption = My.Resources.Resource.TXTDptoServ
            oLabel.Item.Top = intTop + 45
            oLabel.Item.Left = intLeft
            oLabel.Item.Width = 110
            'ComboBox Departamento de Servicio
            oComboBox = oFormulario.Items.Add("cboDptoSrv", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX).Specific
            oComboBox.Item.Top = intTop + 45
            oComboBox.Item.Left = intLeft + 120
            oComboBox.Item.Width = 137
            oComboBox.Item.DisplayDesc = True
            oComboBox.Item.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
            oComboBox.DataBind.SetBound(True, strTablaEncabezado, "U_SCGD_ServDpto")
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Shared Function GetForegroundWindow() As IntPtr
    End Function

    Public Sub ActivarAvaluo(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Dim oFormulario As SAPbouiCOM.Form
        Try
            If pVal.BeforeAction AndAlso pVal.FormTypeEx = "149" Then
                oFormulario = ObtenerFormulario(FormUID)
                If strUsaAvaluo = "Y" Then
                    AgregaControlesTabAvaluo(oFormulario)
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

    Public Sub ManejadorEventoLoad(ByVal FormUID As String, _
                                    ByRef pVal As SAPbouiCOM.ItemEvent, _
                                    ByRef BubbleEvent As Boolean)

        Dim oitem As SAPbouiCOM.Item
        Dim onewitem As SAPbouiCOM.Item
        Dim ofolder As SAPbouiCOM.Folder
        Dim oform As SAPbouiCOM.Form
        Dim strEtiquetaTab As String


        If pVal.FormTypeEx = mc_intidFormaCotizacion Then


            oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If pVal.BeforeAction _
                AndAlso pVal.FormTypeEx = mc_intidFormaCotizacion Then

                oitem = oform.Items.Item("138")
                onewitem = oform.Items.Add(mc_strfdRecepcion, SAPbouiCOM.BoFormItemTypes.it_FOLDER)
                onewitem.Left = oitem.Left + oitem.Width
                onewitem.Width = oitem.Width
                onewitem.Top = oitem.Top
                onewitem.Height = oitem.Height
                onewitem.AffectsFormMode = False

                ofolder = onewitem.Specific

                strEtiquetaTab = My.Resources.Resource.Recepcion
                ofolder.Caption = strEtiquetaTab

                ofolder.GroupWith("138")

                AgrgegaControlesTabRecepción(oform)

                AgregaControlesTabContenido(oform)

                If strUsaAvaluo = "Y" Then
                    AgregaControlesTabAvaluo(oform)
                End If

            End If

        End If

    End Sub

    ''' <summary>
    ''' Agregar los controles para el tab de Avaluo
    ''' </summary>
    ''' <param name="oform">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub AgregaControlesTabAvaluo(ByVal oform As SAPbouiCOM.Form)

        Dim oitem As SAPbouiCOM.Item
        Dim onewitem As SAPbouiCOM.Item
        Dim ofolder As SAPbouiCOM.Folder
        Dim oEdit As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox

        Dim intTopActual As Integer
        Const c_intPanel As Integer = 16
        Dim m_intTabOrder As Integer = 1326

        Const c_intFirstColumna As Integer = 0
        Const c_intFirstColLeftEdit As Integer = c_intFirstColumna + 95
        Const c_intFirstColLeftStatic As Integer = c_intFirstColumna + 13

        Const c_intSecodColumna = c_intFirstColumna + 180
        Const c_intSecondColEdit As Integer = c_intSecodColumna + 100
        Const c_intSecondColStatic As Integer = c_intSecodColumna + 20

        Const c_intThirdColumna As Integer = c_intSecodColumna + 180
        Const c_intThirdColLeftEdit As Integer = c_intThirdColumna + 100
        Const c_intThirdColLeftStatic As Integer = c_intThirdColumna + 20

        oitem = oform.Items.Item(LabelReferencia)
        intTopActual = oitem.Top

        'Inicio del Folder
        oitem = oform.Items.Item("138")
        onewitem = oform.Items.Add(tabAvaluo.strLabelTab, SAPbouiCOM.BoFormItemTypes.it_FOLDER)
        onewitem.Left = oitem.Left + oitem.Width
        onewitem.Width = oitem.Width
        onewitem.Top = oitem.Top
        onewitem.Height = oitem.Height
        onewitem.AffectsFormMode = False

        ofolder = onewitem.Specific

        ofolder.Caption = My.Resources.Resource.TabAvaluo

        ofolder.GroupWith("138")
        'Fin del Folder


        oitem = AgregaEditText(oform, tabAvaluo.strEtNoAva, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbNoAva)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFNoAva)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbNoAva, My.Resources.Resource.CapNoAva, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtNoAva)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtNoCaso, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbNoCaso)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFNoCas)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbNoCaso, My.Resources.Resource.CapNoCas, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtNoCaso)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtNoPoli, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbNoPoli)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFNoPoli)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbNoPoli, My.Resources.Resource.CapNoPoli, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtNoPoli)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtRef1, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbRef1)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFRef1)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbRef1, My.Resources.Resource.CapARef1, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtRef1)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtRef2, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbRef2)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFRef2)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbRef2, My.Resources.Resource.CapARef2, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtRef2)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtMontAsegurado, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbMontAsegurado)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFMontAseguradoro)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbMontAsegurado, My.Resources.Resource.CapMontAsegurado, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtMontAsegurado)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtInfra, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbInfra)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFInfra)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbInfra, My.Resources.Resource.CapInfra, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtInfra)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtWAN, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbWAN)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFWAN)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbWAN, My.Resources.Resource.CapWAN, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtWAN)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtPoolAsig, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbPoolAsig)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFPoolAsig)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbPoolAsig, My.Resources.Resource.CapPAsig, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtPoolAsig)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtCompOri, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbCompOri)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFCompOri)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbCompOri, My.Resources.Resource.CapCompOr, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtCompOri)


        intTopActual += 18

        oitem = AgregaEditText(oform, tabAvaluo.strEtDetalle, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbDetalle)
        oitem.Width = 490
        oitem.Height = 50
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFAvaluoDetalle)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbDetalle, My.Resources.Resource.CapDetalleAvaluo, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtDetalle)

        oitem = oform.Items.Item(LabelReferencia)
        intTopActual = oitem.Top

        oitem = AgregaEditText(oform, tabAvaluo.strEtVigI, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbVigI)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFVigI)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbVigI, My.Resources.Resource.CapVigI, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtVigI)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtVigF, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbVigF)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFVigF)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbVigF, My.Resources.Resource.CapVigF, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtVigF)

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbCober, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbCober)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFCober)
        AgregaStatics(oform, tabAvaluo.strLbCober, My.Resources.Resource.CapCobertura, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbCober)
        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_ACOBERTURA] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbDedu, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbDedu)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFDedu)
        AgregaStatics(oform, tabAvaluo.strLbDedu, My.Resources.Resource.CapDeducible, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbDedu)
        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_ADEDUCIBLE] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbAgencia, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbAgencia)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFAgencia)
        AgregaStatics(oform, tabAvaluo.strLbAgencia, My.Resources.Resource.CapAgencia, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbAgencia)
        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_AAGENCIA] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbAcrePren, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbAcrePren)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFAcrePren)
        AgregaStatics(oform, tabAvaluo.strLbAcrePren, My.Resources.Resource.CapAcreedorPrendario, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbAcrePren)
        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_AACREEDOR] with (nolock) ")

        oitem = oform.Items.Item(LabelReferencia)
        intTopActual = oitem.Top

        oitem = AgregaCombobox(oform, tabAvaluo.strCbCompS, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbCompS)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFCompSeguros)

        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbCompS, My.Resources.Resource.CapCompSeguros, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbCompS)

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_ACOMPSEGUROS] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbPerito, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbPerito)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFPerito)

        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbPerito, My.Resources.Resource.CapPerito, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbPerito)

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_APERITO] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbSupervisor, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbSupervisor)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFSupervisor)

        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbSupervisor, My.Resources.Resource.CapSupervisor, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbSupervisor)

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_ASUPERVISOR] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbAccidente, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbAccidente)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFAccidente)

        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbAccidente, My.Resources.Resource.CapAccidente, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbAccidente)

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_AACCIDENTE] with (nolock) ")

        intTopActual += 16

        oitem = AgregaCombobox(oform, tabAvaluo.strCbAvaluo, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbAvaluo)
        oitem.DisplayDesc = True
        oCombo = oitem.Specific
        oCombo.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFAvaluo)

        oCombo.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbAvaluo, My.Resources.Resource.CapAvaluo, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strCbAvaluo)

        Call Utilitarios.CargarValidValuesEnCombos(oCombo.ValidValues, " SELECT Code, Name FROM [@SCGD_AVALUO] with (nolock) ")

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtHAva, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbHAva)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFHorasAvaluo)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbHAva, My.Resources.Resource.CapHorasAvaluo, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtHAva)

        intTopActual += 16

        oitem = AgregaEditText(oform, tabAvaluo.strEtMontoAvaluo, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strLbMontoAvaluo)
        oEdit = oitem.Specific
        oEdit.DataBind.SetBound(True, mc_strOQUT, tabAvaluo.strUDFMontoAvaluo)

        oEdit.TabOrder = m_intTabOrder
        m_intTabOrder += 1
        AgregaStatics(oform, tabAvaluo.strLbMontoAvaluo, My.Resources.Resource.CapMontoAvaluo, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, tabAvaluo.strEtMontoAvaluo)

        intTopActual += 16

    End Sub


    Public Sub ManejadorEventoKeyDown(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean)

        Dim oform As SAPbouiCOM.Form

        Try
            oform = SBO_Application.Forms.Item(FormUID)

            If pVal.ActionSuccess Then

                Select Case pVal.ItemUID
                    Case "SCGD_etPla"
                        If pVal.CharPressed = "9" Then
                            Enter_Placa(String.Empty, oform, oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Num_Placa", 0).Trim())
                        End If
                End Select

            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub ManejadorEventoItemPressed(ByVal FormUID As String, _
                                          ByRef pVal As SAPbouiCOM.ItemEvent, _
                                          ByRef BubbleEvent As Boolean, _
                                          ByRef oDetVehiculos As VehiculosCls, _
                                          ByRef TypeVehiculo As String, _
                                          ByRef TypeCountVehiculo As Integer)

        Dim oEdit As SAPbouiCOM.EditText
        Dim strCardCode As String
        Dim strNoVehiculo As String
        Dim oform As SAPbouiCOM.Form
        Dim strNumeroOT As String = ""
        Dim strDireccionReporte As String = ""
        Dim strDocEntryConsulta As String = ""
        Dim strDocEntry As String = ""
 

        Try

            oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If pVal.BeforeAction Then
                Select Case pVal.ItemUID
                    Case mc_strfdRecepcion
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Else
                            oform.PaneLevel = 5
                        End If
                    Case "btnAsM"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, "SCGD_btnAsM") Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    Case "SCGD_btRe"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    Case "SCGD_btHiV"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    Case "SCGD_btBal"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    Case "SCGD_btEsS"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    Case "SCGD_LKOT"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        End If
                    Case tabAvaluo.strLabelTab
                        oform.PaneLevel = 16

                    Case "SCGD_etPla"
                        'If (RecepcionVehiculo.blnEsChooseFromListrecepcion = True) Then
                        '    Enter_Placa(String.Empty, oform, oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Num_Placa", 0).Trim())
                        'End If
                    Case mc_strIDBotonEjecucion

                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                            AsignaValoresdeRecepcionControlesUIDefecto(oform)
                        End If
                    Case "lkCit"
                        If Not AdministradorLicencias.LicenciaUsuarioValida(DMS_Connector.Company.CompanySBO.UserSignature, pVal.ItemUID) Then
                            BubbleEvent = False
                            DMS_Connector.Company.ApplicationSBO.StatusBar.SetText(My.Resources.Resource.ErrorLicencia, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        Else

                        End If
                        AbrirCita(oform)
                End Select
            ElseIf pVal.ActionSuccess Then
                Select Case pVal.ItemUID
                    Case mc_strIDBotonEjecucion

                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                            AsignaValoresdeRecepcionControlesUIDefecto(oform)
                        End If

                    Case mc_strpicbtDetalleVehiculos
                        oEdit = oform.Items.Item(mc_stretNoUnidad).Specific
                        If String.IsNullOrEmpty(oEdit.String) Then
                            If Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then
                                oEdit = oform.Items.Item("4").Specific
                                strCardCode = oEdit.String
                                m_oVehiculo = oDetVehiculos
                                VehiculosCls.blnDesdeCita = False
                                VehiculosCls.blnDesdeCotizacion = True

                                Call m_oVehiculo.DibujarFormularioDetalleInformacionVehiculo(strCardCode, "", True, TypeVehiculo, TypeCountVehiculo, False, False, VehiculosCls.ModoFormulario.scgTaller, True)
                            End If
                        Else
                            oEdit = oform.Items.Item("4").Specific
                            strCardCode = oEdit.String

                            oEdit = oform.Items.Item(mc_stretidNoVehiculo).Specific
                            strNoVehiculo = oEdit.String

                            If Not ValidarSiFormularioAbierto("SCGD_DET_1", False) Then


                                m_oVehiculo = oDetVehiculos

                                VehiculosCls.blnDesdeCita = False
                                VehiculosCls.blnDesdeCotizacion = True

                                Call m_oVehiculo.DibujarFormularioDetalleInformacionVehiculo(strCardCode, strNoVehiculo, True, TypeVehiculo, TypeCountVehiculo, False, False, VehiculosCls.ModoFormulario.scgTaller, False)

                            End If
                        End If
                    Case mc_strbtArchivos
                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oform.Mode = BoFormMode.fm_OK_MODE Then
                            Dim tr As System.Threading.Thread = New System.Threading.Thread(New System.Threading.ThreadStart(AddressOf CargaDialogo))
                            tr.CurrentUICulture = My.Resources.Resource.Culture
                            tr.SetApartmentState(Threading.ApartmentState.STA)
                            tr.Start()
                        End If

                    Case mc_strbtEstadoServicios
                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oform.Mode = BoFormMode.fm_OK_MODE Then
                            oEdit = oform.Items.Item(mc_stretOT).Specific
                            strNumeroOT = oEdit.String
                            strNumeroOT = strNumeroOT.Trim()

                            If Not String.IsNullOrEmpty(strNumeroOT) Then

                                Dim strNombreBDTaller As String = ""

                                Utilitarios.DevuelveNombreBDTaller(SBO_Application, strNombreBDTaller)

                                strDireccionReporte = Utilitarios.EjecutarConsulta("Select U_Reportes from [@SCGD_ADMIN] where Code = 'DMS'", m_ocompany.CompanyDB, m_ocompany.Server)
                                strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptServiciosporOrden & ".rpt"

                                Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.TituloServiciosPorOrden, strNumeroOT, CatchingEvents.DBUser, CatchingEvents.DBPassword, strNombreBDTaller, m_ocompany.Server)

                            ElseIf String.IsNullOrEmpty(strNumeroOT) Then

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorFaltaOTReporteEstServ, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                            End If
                        End If

                    Case mc_strbtHistorialVehiculo
                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oform.Mode = BoFormMode.fm_OK_MODE Then

                            oEdit = oform.Items.Item(mc_stretNoUnidad).Specific
                            strNoVehiculo = oEdit.String
                            strNoVehiculo = strNoVehiculo.Trim()

                            If Not String.IsNullOrEmpty(strNoVehiculo) Then

                                strDireccionReporte = Utilitarios.EjecutarConsulta("Select U_Reportes from [@SCGD_ADMIN] where Code = 'DMS'", m_ocompany.CompanyDB, m_ocompany.Server)
                                strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptHistorialVehiculo & ".rpt"

                                Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.TituloHistorialVehiculo, strNoVehiculo, CatchingEvents.DBUser, CatchingEvents.DBPassword, m_ocompany.CompanyDB, m_ocompany.Server)

                            ElseIf String.IsNullOrEmpty(strNoVehiculo) Then

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorFaltaOTReporteHisVehiculo, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                            End If

                        End If
                    Case mc_strbtBalanceOT
                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oform.Mode = BoFormMode.fm_OK_MODE Then
                            oEdit = oform.Items.Item(mc_stretOT).Specific
                            strNumeroOT = oEdit.String
                            strNumeroOT = strNumeroOT.Trim()

                            If Not String.IsNullOrEmpty(strNumeroOT) Then

                                Dim strNombreBDTaller As String = ""

                                Utilitarios.DevuelveNombreBDTaller(SBO_Application, strNombreBDTaller)

                                strDireccionReporte = Utilitarios.EjecutarConsulta("Select U_Reportes from [@SCGD_ADMIN] where Code = 'DMS'", m_ocompany.CompanyDB, m_ocompany.Server)
                                strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptBalanceOT & ".rpt"

                                Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.TituloBalanceOT, strNumeroOT, CatchingEvents.DBUser, CatchingEvents.DBPassword, strNombreBDTaller, m_ocompany.Server)

                            ElseIf String.IsNullOrEmpty(strNumeroOT) Then

                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorFaltaOTReporteEstServ, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)

                            End If
                        End If

                    Case mc_strbtRecepcionVehi
                        If oform.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oform.Mode = BoFormMode.fm_OK_MODE Then
                            oEdit = oform.Items.Item(mc_stretOT).Specific
                            strNumeroOT = oEdit.String
                            strNumeroOT = strNumeroOT.Trim()

                            If Not String.IsNullOrEmpty(strNumeroOT) Then

                                If Utilitarios.ValidarOTInternaConfiguracion(m_ocompany) Then

                                    'Dim strNombreBDTaller As String = ""
                                    'Utilitarios.DevuelveNombreBD(SBO_Application, strNombreBDTaller)

                                    strDireccionReporte = Utilitarios.EjecutarConsulta("Select U_Reportes from [@SCGD_ADMIN] where Code = 'DMS'", m_ocompany.CompanyDB, m_ocompany.Server)
                                    strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptOrdenRecepcionInterna & ".rpt"

                                    strDocEntryConsulta = String.Format("Select DocEntry from OQUT with(nolock) where U_SCGD_Numero_OT = '{0}'", strNumeroOT)

                                    strDocEntry = Utilitarios.EjecutarConsulta(strDocEntryConsulta, m_ocompany.CompanyDB, m_ocompany.Server)

                                    Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.rptOrdenRecepcionInterna, strDocEntry, CatchingEvents.DBUser, CatchingEvents.DBPassword, m_ocompany.CompanyDB, m_ocompany.Server)

                                Else
                                    Dim strNombreBDTaller As String = ""
                                    Utilitarios.DevuelveNombreBDTaller(SBO_Application, strNombreBDTaller)

                                    strDireccionReporte = Utilitarios.EjecutarConsulta("Select U_Reportes from [@SCGD_ADMIN] where Code = 'DMS'", m_ocompany.CompanyDB, m_ocompany.Server)
                                    strDireccionReporte = strDireccionReporte & "\" & My.Resources.Resource.rptOrdenRecepcion & ".rpt"

                                    Call Utilitarios.ImprimirReporte(strDireccionReporte, My.Resources.Resource.OrdenRecepcion, strNumeroOT, CatchingEvents.DBUser, CatchingEvents.DBPassword, strNombreBDTaller, m_ocompany.Server)
                                End If


                            ElseIf String.IsNullOrEmpty(strNumeroOT) Then
                                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorFaltaOTReporteEstServ, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                            End If
                        End If

                        'Case mc_strbtVisualizaFotos

                        'If oform.Mode = SAPbouiCOM.BoFormMode.fm_VIEW_MODE OrElse oform.Mode = SAPbouiCOM.BoFormMode.fm_EDIT_MODE OrElse oform.Mode = BoFormMode.fm_OK_MODE Then
                        'oEdit = oform.Items.Item(mc_stretOT).Specific
                        'strNumeroOT = oEdit.String
                        'strNumeroOT = strNumeroOT.Trim()

                        'Dim ptr As IntPtr = GetForegroundWindow()
                        'Dim wrapper As New WindowWrapper(ptr)


                        'If String.IsNullOrEmpty(strNumeroOT.ToString()) = False Then
                        '_Fotos = New frmVisualFotos(strNumeroOT.ToString(), SBO_Application, m_ocompany)
                        '_Fotos.ShowInTaskbar = False


                        'ElseIf String.IsNullOrEmpty(strNumeroOT) Then
                        'SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorFaltaOTReporteEstServ, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        'End If

                        'End If

                    Case mc_strcbTipoOT

                        'Dim strIdSucursal As String = oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd()

                        'CargarValidValuesEnComboTipoOrden(oform, strIdSucursal, True)

                End Select
            End If


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("ManejadorItemPressed" & "" & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Private Sub AbrirCita(ByRef Formulario As SAPbouiCOM.Form)
        Dim SerieCita As String = String.Empty
        Dim NumeroCita As String = String.Empty
        Try
            SerieCita = Formulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoSerieCita", 0).Trim
            NumeroCita = Formulario.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_NoCita", 0).Trim
            If Not String.IsNullOrEmpty(SerieCita) AndAlso Not String.IsNullOrEmpty(NumeroCita) Then
                ConstructorCitas.CrearInstanciaFormularioExistente(SerieCita, NumeroCita)
            End If
        Catch ex As Exception
            DMS_Connector.Helpers.ManejoErrores(ex)
        End Try
    End Sub

    Private Sub CargaDialogo()
        Try
            Dim strConectionString As String = String.Empty
            Dim oform As SAPbouiCOM.Form
            Dim oItem As SAPbouiCOM.Item
            Dim edit As SAPbouiCOM.EditText
            Dim strNombreTaller As String = String.Empty

            Utilitarios.DevuelveNombreBDTaller(SBO_Application, strNombreTaller)
            Configuracion.CrearCadenaDeconexion(m_ocompany.Server, strNombreTaller, strConectionString)
            oform = SBO_Application.Forms.ActiveForm
            oItem = oform.Items.Item(mc_stretNoVisita)
            edit = oItem.Specific

            Dim value As Integer

            If edit.Value IsNot Nothing AndAlso Integer.TryParse(edit.Value, value) Then
                Dim tipoSkin As Integer = Utilitarios.CargarTipoSkin()

                Dim archivoDigital As FrmArchivoDigital = New FrmArchivoDigital(My.Resources.Resource.TituloDialogoArchivo, "SCGTA_TB_Visita", value, mc_strTablaArchivosDigitales, strConectionString, 10, tipoSkin)
                archivoDigital.Tag = value

                Dim MyProcs() As Process
                MyProcs = Process.GetProcessesByName("SAP Business One")
                Dim currentProcess As Process = Process.GetCurrentProcess()

                If MyProcs.Length <> 0 Then
                    For i As Integer = 0 To MyProcs.Length - 1
                        If MyProcs(i).SessionId = currentProcess.SessionId Then
                            Dim MyWindow As New WindowWrapper(MyProcs(i).MainWindowHandle)
                            archivoDigital.ShowInTaskbar = False
                            archivoDigital.ShowDialog(MyWindow)
                        End If
                    Next
                End If


            End If
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    '    Public Sub FormClosedEventHandler(ByVal sender As Object, ByVal e As FormClosedEventArgs)
    '        Dim frm As FrmArchivoDigital = TryCast(sender, FrmArchivoDigital)
    '        Dim value As Integer = CInt(frm.Tag)
    '        If (listaFormularios.ContainsKey(value)) Then
    '            listaFormularios.Remove(value)
    '        End If
    '    End Sub

    Public Sub ManejadorEventoClose(ByVal formID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef bubbleEvent As Boolean)
    End Sub

    Private Function ValidarSiFormularioAbierto(ByVal strFormUID As String, _
                                              ByVal blnselectIfOpen As Boolean) As Boolean

        Dim intI As Integer = 0
        Dim blnFound As Boolean = False
        Dim frmForma As SAPbouiCOM.Form

        While (Not blnFound AndAlso intI < SBO_Application.Forms.Count)

            frmForma = SBO_Application.Forms.Item(intI)

            If frmForma.UniqueID = strFormUID Then
                blnFound = True
                If (blnselectIfOpen) Then
                    If Not (frmForma.Selected) Then
                        SBO_Application.Forms.Item(strFormUID).Select()
                    End If
                End If
            Else

                intI += 1
            End If

        End While

        If (blnFound) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Sub ManejadorEventoChooseFromList(ByVal FormUID As String, _
                                              ByRef pVal As ItemEvent, _
                                              ByRef BubbleEvent As Boolean)
        Dim oform As SAPbouiCOM.Form

        Try

            blnEsChooseFromListrecepcion = True

            oform = SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            If Not oform Is Nothing _
                AndAlso oform.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                Dim oedit As SAPbouiCOM.EditText
                Dim oitem As SAPbouiCOM.Item
                Dim strMensaje As String = String.Empty
                Dim strProyecto As String = String.Empty
                Dim strProyectoNombre As String = String.Empty
                Dim oMSocioNegocios As SAPbobsCOM.BusinessPartners

                'oMSocioNegocios = m_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)

                Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
                oCFLEvento = pVal
                Dim sCFL_ID As String
                sCFL_ID = oCFLEvento.ChooseFromListUID
                Dim oCFL As SAPbouiCOM.ChooseFromList

                oCFL = oform.ChooseFromLists.Item(sCFL_ID)

                If oCFLEvento.ActionSuccess Then

                    Dim oDataTable As SAPbouiCOM.DataTable
                    oDataTable = oCFLEvento.SelectedObjects

                    If Not oDataTable Is Nothing Then
                        If (pVal.ItemUID = mc_strbtpicNoPlaca) Or pVal.ItemUID = mc_stretPlaca Then

                            m_existe_datatablevehiculo = True
                            Dim val As VehiculoUDT = Nothing
                            Dim m_blMultiples As Boolean = False

                            If DMS_Connector.Configuracion.ParamGenAddon.U_CnpDMS.Trim().Equals("Y") Then
                                strMensaje = Utilitarios.VerificaCampanaPorUnidad(oDataTable.GetValue(mc_strUDFNoUnidad, 0), oDataTable.GetValue(mc_strUDFVin, 0), SBO_Application, m_blMultiples)
                            End If

                            oedit = oform.Items.Item("16").Specific
                            oedit.String = strMensaje

                            Call AsignaValoresVehiculo(val, oDataTable)

                            AsignaValoresdeVehiculoaControlesUI(val, oform)
                            BubbleEvent = False
                        ElseIf pVal.ItemUID = mc_strbtpicRecepcion Or pVal.ItemUID = mc_stretNoVisita Then

                            m_existe_datatablevehiculo = False

                            Call AsignaValoresdeRecepcionControlesUI(oDataTable, oform)

                        ElseIf pVal.ItemUID = mc_stretProyectoNombre Then

                            If oform.Mode = BoFormMode.fm_OK_MODE Then oform.Mode = BoFormMode.fm_UPDATE_MODE

                            strProyecto = oDataTable.GetValue(mc_strCodeProyecto, 0)
                            strProyectoNombre = oDataTable.GetValue(mc_strNameProyecto, 0)

                            oform.Items.Item(mc_stretOTReferencia).Click(BoCellClickType.ct_Regular)

                            oitem = oform.Items.Item(mc_stretProyectoNombre)
                            oitem.Enabled = False
                            oedit = oitem.Specific
                            oedit.String = strProyectoNombre
                            oitem.Enabled = True

                            oitem = oform.Items.Item(mc_stretProyectoNumero)
                            oedit = oitem.Specific
                            oedit.String = strProyecto

                            oform.Items.Item(mc_stretProyectoNumero).Click(BoCellClickType.ct_Regular)

                        ElseIf pVal.ItemUID = "4" OrElse pVal.ItemUID = "54" Then
                            oform.Freeze(True)
                            'Asigno el Nombre y codigo a los campos de Codigo y Cliente OT
                            oitem = oform.Items.Item(mc_strNClienteOT)
                            oedit = oitem.Specific

                            If String.IsNullOrEmpty(oedit.String) Then
                                oitem.Enabled = False
                                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                                oedit.String = oDataTable.GetValue("CardName", 0)
                                oitem.Enabled = True
                                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                                oitem = oform.Items.Item(mc_strCClienteOT)
                                oedit = oitem.Specific
                                oitem.Enabled = False
                                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                                oedit.String = oDataTable.GetValue("CardCode", 0)
                                oitem.Enabled = True
                                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                            End If
                            oform.Freeze(False)
                        End If

                        'manejo de la matriz
                        Select Case pVal.ItemUID
                            Case "38"
                                If pVal.ColUID = "U_SCGD_EmpAsig" Then
                                    ManejaCFLColaboradores(pVal, oDataTable, oform)
                                End If

                            Case "btnSN"
                                oitem = oform.Items.Item(mc_strNClienteOT)
                                oedit = oitem.Specific
                                oedit.String = oDataTable.GetValue("CardName", 0)

                                oitem = oform.Items.Item(mc_strCClienteOT)
                                oedit = oitem.Specific
                                oedit.String = oDataTable.GetValue("CardCode", 0)
                        End Select
                    End If
                End If

                If pVal.Before_Action Then
                    Select Case pVal.ItemUID
                        Case mc_strbtpicNoPlaca
                            oedit = oform.Items.Item(mc_strCClienteOT).Specific
                            oedit.Item.Enabled = False
                            AplicaValorDeCondicion(0, oform, "CFL1", oedit.String)
                            oedit.Item.Enabled = True

                        Case mc_strbtpicRecepcion
                            oedit = oform.Items.Item(mc_stretNoUnidad).Specific
                            AplicaValorDeCondicion(0, oform, "CFL3", oedit.String)

                        Case mc_stretPlaca
                            oedit = oform.Items.Item("4").Specific
                            AplicaValorDeCondicion(0, oform, "CFL5", oedit.String)

                        Case "38"
                            ValidaCFLColaboradores(pVal, oform, BubbleEvent)
                    End Select
                End If
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub AsignaValoresVehiculo(ByRef udtVehiculo As VehiculoUDT, _
                                           ByVal oDataTable As SAPbouiCOM.DataTable)

        Try
            udtVehiculo = New VehiculoUDT

            With udtVehiculo

                If Not oDataTable.GetValue(mc_strUDFPlaca, 0) Is System.Convert.DBNull Then

                    .NoPlaca = oDataTable.GetValue(mc_strUDFPlaca, 0)

                End If

                If Not oDataTable.GetValue(mc_strUDFEstiloDesc, 0) Is System.Convert.DBNull Then
                    .DescEstilo = oDataTable.GetValue(mc_strUDFEstiloDesc, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFMarcaDesc, 0) Is System.Convert.DBNull Then
                    .DescMArca = oDataTable.GetValue(mc_strUDFMarcaDesc, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFModeloDesc, 0) Is System.Convert.DBNull Then
                    .DescModelo = oDataTable.GetValue(mc_strUDFModeloDesc, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFModeloDesc, 0) Is System.Convert.DBNull Then
                    .DescVehiculo = oDataTable.GetValue(mc_strUDFModeloDesc, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFAñoVehiclo, 0) Is System.Convert.DBNull Then
                    .Año = oDataTable.GetValue(mc_strUDFAñoVehiclo, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFVin, 0) Is System.Convert.DBNull Then
                    .Vin = oDataTable.GetValue(mc_strUDFVin, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFNoUnidad, 0) Is System.Convert.DBNull Then
                    .NoUnidad = oDataTable.GetValue(mc_strUDFNoUnidad, 0)
                End If


                If Not oDataTable.GetValue(mc_strUDFNumVehi, 0) Is System.Convert.DBNull Then
                    .NumVehiculo = oDataTable.GetValue(mc_strUDFNumVehi, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFCodEstilo, 0) Is System.Convert.DBNull Then
                    .CodEstilo = oDataTable.GetValue(mc_strUDFCodEstilo, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFCodMarca, 0) Is System.Convert.DBNull Then
                    .CodMarca = oDataTable.GetValue(mc_strUDFCodMarca, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFCodModelo, 0) Is System.Convert.DBNull Then
                    .CodModelo = oDataTable.GetValue(mc_strUDFCodModelo, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFKM_Unid, 0) Is System.Convert.DBNull Then
                    .KmUnidad = oDataTable.GetValue(mc_strUDFKM_Unid, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFHorasServicioVH, 0) Is System.Convert.DBNull Then
                    .HoraServicio = oDataTable.GetValue(mc_strUDFHorasServicioVH, 0)
                End If

                If Not (oDataTable.GetValue(mc_strUDFVehiGaranIni, 0) Is System.Convert.DBNull) AndAlso Not (oDataTable.GetValue(mc_strUDFVehiGaranIni, 0) Is Nothing) Then
                    .GarantiaInicio = oDataTable.GetValue(mc_strUDFVehiGaranIni, 0)
                End If

                If Not oDataTable.GetValue(mc_strUDFVehiGaranFin, 0) Is System.Convert.DBNull AndAlso Not (oDataTable.GetValue(mc_strUDFVehiGaranFin, 0) Is Nothing) Then
                    .GarantiaFin = oDataTable.GetValue(mc_strUDFVehiGaranFin, 0)
                End If

                If Not oDataTable.GetValue("U_CardName", 0) Is System.Convert.DBNull Then
                    .strNameCliente = oDataTable.GetValue("U_CardName", 0)
                End If

                If Not oDataTable.GetValue("U_CardCode", 0) Is System.Convert.DBNull Then
                    .strCodeCliente = oDataTable.GetValue("U_CardCode", 0)
                End If

            End With

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Private Sub AsignaValoresdeVehiculoaControlesUI(ByVal udtVehiculo As VehiculoUDT, _
                                                    ByVal oform As SAPbouiCOM.Form,
                                                    Optional ByVal esPlaca As Boolean = False)

        Dim oEdit As SAPbouiCOM.EditText
        Dim formato As String

        Try

            formato = Utilitarios.ObtieneFormatoFecha(SBO_Application, m_ocompany)
            oform.Freeze(True)

            If udtVehiculo.Año > 0 Then
                'oForm.Items.Item(mc_strtxtAsesor).Specific.Value = oDataTable.GetValue(0, 0).ToString()
                oEdit = oform.Items.Item(mc_stretAño).Specific
                oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Ano_Vehi")
                oEdit.Value = String.Empty
                oEdit.Value = udtVehiculo.Año
            Else
                oEdit = oform.Items.Item(mc_stretAño).Specific
                oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Ano_Vehi")
                oEdit.Value = String.Empty
            End If

            oEdit = oform.Items.Item(mc_stretMarca).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Des_Marc")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.DescMArca

            oEdit = oform.Items.Item(mc_stretEstilo).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Des_Esti")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.DescEstilo

            oEdit = oform.Items.Item(mc_stretVIN).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Num_VIN")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.Vin

            oEdit = oform.Items.Item(mc_stretModelo).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Des_Mode")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.DescModelo

            oEdit = oform.Items.Item(mc_stretNoUnidad).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Cod_Unidad")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.NoUnidad

            oEdit = oform.Items.Item(mc_stretidModelo).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Cod_Modelo")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.CodModelo

            oEdit = oform.Items.Item(mc_stretidEstilo).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Cod_Estilo")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.CodEstilo

            oEdit = oform.Items.Item(mc_stretidNoVehiculo).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Num_Vehiculo")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.NumVehiculo

            oEdit = oform.Items.Item(mc_stretKilometraje).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Kilometraje")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.KmUnidad

            oEdit = oform.Items.Item(mc_stretidMarca).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_Cod_Marca")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.CodMarca

            oEdit = oform.Items.Item(mc_stretHoraServicio).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_HoSr")
            oEdit.Value = String.Empty
            oEdit.Value = udtVehiculo.HoraServicio

            Dim newdate As Date

            oEdit = oform.Items.Item(mc_strEtGaranIni).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_GaraIni")
            oEdit.Value = String.Empty
            If newdate <> udtVehiculo.GarantiaInicio Then
                oEdit.String = udtVehiculo.GarantiaInicio.ToString(formato)
            End If

            oEdit = oform.Items.Item(mc_strEtGaranFin).Specific
            oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_GaraFin")
            oEdit.Value = String.Empty
            If newdate <> udtVehiculo.GarantiaFin Then
                oEdit.String = udtVehiculo.GarantiaFin.ToString(formato)
                If udtVehiculo.GarantiaFin >= Date.Today Then
                    SBO_Application.MessageBox(My.Resources.Resource.txtVehiculoGarantia)
                End If
            End If

            If Not String.IsNullOrEmpty(udtVehiculo.strCodeCliente) Then
                oEdit = oform.Items.Item(mc_strCClienteOT).Specific
                oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_CCliOT")
                oEdit.Value = udtVehiculo.strCodeCliente

                oEdit = oform.Items.Item(mc_strNClienteOT).Specific
                oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_NCliOT")
                oEdit.Value = udtVehiculo.strNameCliente
            Else
                oEdit = oform.Items.Item(mc_strCClienteOT).Specific
                oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_CCliOT")
                oEdit.Value = String.Empty

                oEdit = oform.Items.Item(mc_strNClienteOT).Specific
                oEdit.DataBind.SetBound("true", "OQUT", "U_SCGD_NCliOT")
                oEdit.Value = String.Empty
            End If

            oform.Items.Item(mc_stretHoraServicio).Click()

            If Not esPlaca Then

                Dim oed As SAPbouiCOM.EditText
                Dim strPlaca As String = String.Empty
                strPlaca = udtVehiculo.NoPlaca.ToString().Trim()

                oed = oform.Items.Item(mc_stretPlaca).Specific

                oform.Items.Item(mc_stretPlaca).Enabled = False
                oed.Value = strPlaca
                oform.Items.Item(mc_stretPlaca).Enabled = True

            End If

            oform.Freeze(False)
        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Private Sub AsignaValoresdeRecepcionControlesUI(ByVal oDataTable As SAPbouiCOM.DataTable, _
                                                    ByVal oform As SAPbouiCOM.Form)

        Dim oEdit As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim dtConsulta As SAPbouiCOM.DataTable
        Dim strConsulta As String

        Try
            If Utilitarios.ValidaExisteDataTable(oform, "dtConsulta") Then
                dtConsulta = oform.DataSources.DataTables.Item("dtConsulta")
            Else
                dtConsulta = oform.DataSources.DataTables.Add("dtConsulta")
            End If

            strConsulta = String.Format("SELECT U_SCGD_Gorro_Veh, U_SCGD_Kilometraje,U_SCGD_Gasolina FROM OQUT with(nolock) WHERE U_SCGD_Numero_OT = '{0}'", oDataTable.GetValue("U_NoOT", 0))
            dtConsulta.ExecuteQuery(strConsulta)

            'Datatable ChoosefromList
            oEdit = oform.Items.Item(mc_stretFechaRecepción).Specific
            oEdit.String = oDataTable.GetValue("U_FRec", 0)

            oEdit = oform.Items.Item(mc_stretHoraRecepcion).Specific
            oEdit.String = oDataTable.GetValue("U_HRec", 0)

            oEdit = oform.Items.Item(mc_stretFechadeCompromiso).Specific
            oEdit.String = oDataTable.GetValue("U_FCom", 0)

            oEdit = oform.Items.Item(mc_stretHoraCompra).Specific
            oEdit.String = oDataTable.GetValue("U_HCom", 0)

            oEdit = oform.Items.Item(mc_stretNoVisita).Specific
            oEdit.String = oDataTable.GetValue("U_NoVis", 0)

            'Datatable local - consulta
            oEdit = oform.Items.Item(mc_stretCono).Specific
            oEdit.String = dtConsulta.GetValue("U_SCGD_Gorro_Veh", 0)

            oEdit = oform.Items.Item(mc_stretKilometraje).Specific
            oEdit.String = dtConsulta.GetValue("U_SCGD_Kilometraje", 0)

            oCombo = oform.Items.Item(mc_stretGasolina).Specific
            Call oCombo.Select(CStr(dtConsulta.GetValue("U_SCGD_Gasolina", 0)), SAPbouiCOM.BoSearchKey.psk_ByValue)

            'Valores para crear la OT hija
            oEdit = oform.Items.Item(mc_stretOT).Specific
            oEdit.Value = String.Empty

            oEdit = oform.Items.Item(mc_stretOTReferencia).Specific
            oEdit.Value = oDataTable.GetValue("U_NoOT", 0)

            oEdit = oform.Items.Item("16").Specific
            oEdit.Value = String.Empty


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    Public Sub AsignaValoresdeRecepcionControlesUIDefecto(ByRef oform As SAPbouiCOM.Form)
        Try
            If String.IsNullOrEmpty(oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Fech_Recep", 0)) Then
                oform.Items.Item(mc_stretFechaRecepción).Specific.String = DateTime.Now.ToString("yyyyMMdd")
                oform.Items.Item(mc_stretHoraRecepcion).Specific.String = DateTime.Now.ToString("HHmm")
            End If

            If String.IsNullOrEmpty(oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_Fech_Comp", 0)) Then
                oform.Items.Item(mc_stretFechadeCompromiso).Specific.String = DateTime.Now.ToString("yyyyMMdd")
                oform.Items.Item(mc_stretHoraCompra).Specific.String = DateTime.Now.ToString("HHmm")
            End If

        Catch ex As Exception

            Call Utilitarios.ManejadorErrores(ex, SBO_Application)

        End Try

    End Sub

    Public Shared Sub AgregaContotrolNoOT(ByVal oform As SAPbouiCOM.Form, _
                                          ByVal NombreTablaSBO As String, _
                                          ByVal p_SBO_Application As SAPbouiCOM.Application, _
                                          Optional ByVal p_Top As Integer = 81, _
                                          Optional ByVal p_LeftEtiqueta As Integer = 307, _
                                          Optional ByVal p_LeftText As Integer = 427)

        Dim oitem As SAPbouiCOM.Item
        Dim oitem2 As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText
        Dim oStatic As SAPbouiCOM.StaticText
        Dim strEtiqueta As String

        Try

            Dim ItemRef As String = "18" 'item de referencia para tomar el alto y agregar el control de numero de Ot de la Transferencia de Stock
            Dim intTopReF As Integer = 0
            oitem = oform.Items.Item(ItemRef)
            intTopReF = oitem.Top
            Select Case oform.TypeEx
                Case "540000988"
                    ItemRef = "46"
                    oitem = oform.Items.Item(ItemRef)
                    intTopReF = oitem.Top

                Case "139", "65304", "142", "60092", "133", "60091"
                    ItemRef = "12"
                    oitem = oform.Items.Item(ItemRef)
                    intTopReF = oitem.Top

            End Select

            oitem = oform.Items.Add(mc_stretOT, SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oitem.Left = p_LeftText
            oitem.Top = intTopReF + 30
            oitem.FromPane = 0
            oitem.ToPane = 0
            oitem.LinkTo = mc_strstOT
            oEdit = oitem.Specific
            oEdit.TabOrder = 438
            oitem.Enabled = False
            Call oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
            Call oEdit.DataBind.SetBound(True, NombreTablaSBO, mc_strUDNoOT)

            strEtiqueta = My.Resources.Resource.CapNoOrdenTrabajo
            oitem = oform.Items.Add(mc_strstOT, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oitem.Left = p_LeftEtiqueta
            oitem.Top = intTopReF + 30
            oitem.FromPane = 0
            oitem.ToPane = 0
            oitem.LinkTo = mc_stretOT
            oStatic = oitem.Specific
            oStatic.Caption = strEtiqueta

            If (oform.TypeEx = "149" Or oform.TypeEx = "139" Or oform.TypeEx = "133") Then
                'Codigo Cliente OT
                oitem = oform.Items.Add(mc_strCClienteOT, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem2 = oform.Items.Item("4")
                oitem.Width = oitem2.Width
                oitem.Left = oitem2.Left
                oitem.Top = oitem2.Top + 76
                oitem.LinkTo = mc_strstCodClienteOT
                oEdit = oitem.Specific
                oitem.Enabled = False
                Call oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                Call oEdit.DataBind.SetBound(True, NombreTablaSBO, mc_strUDFCClienteOT)
                strEtiqueta = My.Resources.Resource.CapCodClienOT
                oitem = oform.Items.Item("51")
                AgregaLinkedButton(oform, mc_strLKBCliente, oitem.Left, oitem.Top + 76, oitem.Height, oitem.Width, mc_strCClienteOT, "2", p_SBO_Application)

                oitem = oform.Items.Add(mc_strstCodClienteOT, SAPbouiCOM.BoFormItemTypes.it_STATIC)
                oitem.Left = 6
                oitem.Top = 81
                oitem.LinkTo = mc_strCClienteOT
                oStatic = oitem.Specific
                oStatic.Caption = strEtiqueta


                'Texto Nombre Cliente OT
                oitem = oform.Items.Add(mc_strNClienteOT, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem.Width = oitem2.Width
                oitem.Left = oitem2.Left
                oitem.Top = oitem2.Top + 92
                oitem.LinkTo = mc_strstNomClienteOT
                oEdit = oitem.Specific
                oitem.Enabled = False
                Call oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                Call oEdit.DataBind.SetBound(True, NombreTablaSBO, mc_strUDFNClienteOT)
                strEtiqueta = My.Resources.Resource.CapNomClienOT

                oitem = oform.Items.Add(mc_strstNomClienteOT, SAPbouiCOM.BoFormItemTypes.it_STATIC)
                oitem.Width = 100
                oitem.Left = 6
                oitem.Top = 96
                oitem.LinkTo = mc_strNClienteOT
                oStatic = oitem.Specific
                oStatic.Caption = strEtiqueta

                Dim usaInterFazFord = Utilitarios.UsaInterfazFord(m_ocompany)
                If usaInterFazFord Then

                    Dim oCombo As SAPbouiCOM.ComboBox

                    ' Dim oStaticText As SAPbouiCOM.StaticText
                    'Agerga Combo Tipo Pago
                    oitem = oform.Items.Add(mc_strCboTipoPago, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
                    oitem2 = oform.Items.Item("4")
                    oitem.Top = oitem2.Top + 108
                    oitem.Left = oitem2.Left
                    oitem.Width = oitem2.Width
                    oitem.Height = oitem2.Height
                    oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    oitem.Enabled = True
                    oitem.Visible = True
                    oitem.DisplayDesc = True
                    oCombo = oitem.Specific
                    Call oCombo.DataBind.SetBound(True, NombreTablaSBO, mc_strUDFTipoPago)

                    ''agrega texto tipo pago
                    oitem = Nothing
                    oitem = oform.Items.Add(mc_stTipoPago, SAPbouiCOM.BoFormItemTypes.it_STATIC)
                    oitem2 = oform.Items.Item("5")
                    oitem.Top = oitem2.Top + 108
                    oitem.Left = oitem2.Left
                    oitem.Width = oitem2.Width
                    oitem.Height = oitem2.Height
                    oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    oitem.Enabled = True
                    oitem.Visible = True
                    oitem.LinkTo = mc_strCboTipoPago

                    oStatic = oitem.Specific
                    'oStatic.Item.LinkTo = mc_strCboTipoPago
                    oStatic.Caption = My.Resources.Resource.TXTTipoPago '"Tipo de Pago"

                    'Agerga Combo Departamento Servicio
                    oitem = oform.Items.Add(mc_strCboDptoSrv, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
                    oitem2 = oform.Items.Item("8")
                    oitem.Top = oitem2.Top + 108
                    oitem.Left = oitem2.Left
                    oitem.Width = oitem2.Width
                    oitem.Height = oitem2.Height
                    oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    oitem.Enabled = True
                    oitem.Visible = True
                    oitem.DisplayDesc = True
                    oCombo = oitem.Specific
                    Call oCombo.DataBind.SetBound(True, NombreTablaSBO, mc_strUDFServDpto)

                    ''agrega texto Departamento Servicio
                    oitem = Nothing
                    oitem = oform.Items.Add(mc_stDptoSrv, SAPbouiCOM.BoFormItemTypes.it_STATIC)
                    oitem2 = oform.Items.Item("9")
                    oitem.Top = oitem2.Top + 108
                    oitem.Left = oitem2.Left
                    oitem.Width = oform.Items.Item("11").Width
                    oitem.Height = oitem2.Height
                    oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                    oitem.Enabled = True
                    oitem.Visible = True
                    oitem.LinkTo = mc_strCboDptoSrv

                    oStatic = oitem.Specific
                    '  oStatic.Item.LinkTo = mc_strCboDptoSrv
                    oStatic.Caption = My.Resources.Resource.TXTDptoServ '"Tipo de Pago"

                End If

            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub


    ''' <summary>
    ''' Agregar los controles para el tab de contenio
    ''' </summary>
    ''' <param name="oform">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub AgregaControlesTabContenido(ByVal oform As SAPbouiCOM.Form)
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix

        Try
            If Not oform Is Nothing Then

                Call AddChooseFromListColaboradores(oform)

                oitem = oform.Items.Item("38")
                oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

                oMatrix.Columns.Item("U_SCGD_EmpAsig").ChooseFromListUID = "CFL_Col"
                oMatrix.Columns.Item("U_SCGD_EmpAsig").ChooseFromListAlias = "empID"

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Sub AgrgegaControlesTabRecepción(ByVal oform As SAPbouiCOM.Form)

        Dim oitem As SAPbouiCOM.Item
        Dim oitem2 As SAPbouiCOM.Item
        Dim oEdit As SAPbouiCOM.EditText
        Dim oCombo As SAPbouiCOM.ComboBox
        Dim oButton As SAPbouiCOM.Button
        Dim intTopActual As Integer '= 123
        Dim intleftpicPlaca As Integer
        Dim intLeftButton As Integer
        Dim intHeight As Integer
        Dim intWidth As Integer

        Const c_intFirstColumna As Integer = 0
        Const c_intFirstColLeftEdit As Integer = c_intFirstColumna + 95
        Const c_intFirstColLeftStatic As Integer = c_intFirstColumna + 13

        Const c_intSecodColumna = c_intFirstColumna + 180
        Const c_intSecondColEdit As Integer = c_intSecodColumna + 105
        Const c_intSecondColStatic As Integer = c_intSecodColumna + 20

        Const c_intThirdColumna As Integer = c_intSecodColumna + 180
        Const c_intThirdColLeftEdit As Integer = c_intThirdColumna + 115
        Const c_intThirdColLeftStatic As Integer = c_intThirdColumna + 20

        Const c_intPanel As Integer = 5
        Dim m_intTabOrder As Integer = 1326

        'Agregado 07072010 Diego Herrera
        oitem = oform.Items.Item(LabelReferencia)
        intTopActual = oitem.Top

        Try

            If Not oform Is Nothing Then
                Call AddChooseFromList(oform)
                'Codigo Cliente OT
                oitem2 = oform.Items.Item("4")
                oitem = AgregaEditText(oform, mc_strCClienteOT, oitem2.Left, oitem2.Top + 76, oitem2.FromPane, oitem2.ToPane, mc_strstCodClienteOT)
                oitem.Width = oitem2.Width
                oitem.Enabled = True
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                oEdit = oitem.Specific
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCClienteOT)
                Call AgregaStatics(oform, mc_strstCodClienteOT, My.Resources.Resource.CapCodClienOT, 6, 81, 0, 0, mc_strCClienteOT)
                oitem = oform.Items.Item("51")
                AgregaLinkedButton(oform, mc_strLKBCliente, oitem.Left, oitem.Top + 76, oitem.Height, oitem.Width, mc_strCClienteOT, "2", SBO_Application)
                Call AgregaButtonPic(oform, "btnSN", oitem2.Left + oitem.Left + 35, oitem2.Top + 76, 0, 0, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "CFL6")
                'Nombre Cliente OT
                oitem = AgregaEditText(oform, mc_strNClienteOT, 127, 96, 0, 0, mc_strstNomClienteOT)
                oitem.Width = 148
                oitem.Enabled = True
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                oEdit = oitem.Specific
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFNClienteOT)
                Call AgregaStatics(oform, mc_strstNomClienteOT, My.Resources.Resource.CapNomClienOT, 6, 96, 0, 0, mc_strNClienteOT, Nothing, Nothing, True)

                'Sucursal
                oitem = AgregaCombobox(oform, mc_strcbSucursal, 427, 96, 0, 0, Nothing)
                oitem.DisplayDesc = True
                oitem.Width = 138
                oCombo = oitem.Specific
                Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFidSucursal)
                Call AgregaStatics(oform, mc_strstSucursal, My.Resources.Resource.Sucursal, 307, 96, 0, 0, mc_strcbSucursal)
                If DMS_Connector.Company.AdminInfo.EnableBranches = SAPbobsCOM.BoYesNoEnum.tYES Then
                    oitem.Visible = False
                    oform.Items.Item(mc_strstSucursal).Visible = False
                End If
                '''''''

                'No OT
                oitem = AgregaEditText(oform, mc_stretOT, 427, 81, 0, 0, mc_strstOT)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.TabOrder = 438
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDNoOT)
                Call AgregaStatics(oform, mc_strstOT, My.Resources.Resource.CapNoOrdenTrabajo, 307, 81, 0, 0, mc_stretOT)

                Dim blnUSaOTSap As Boolean = Utilitarios.ValidarOTInternaConfiguracion(m_ocompany)
                If blnUSaOTSap Then
                    oitem2 = oform.Items.Item("51")
                    AgregaLinkedButton(oform, mc_strLKBOT, oitem.Left - 15, oitem2.Top + 76, oitem2.Height, oitem2.Width, mc_stretOT, SAPbouiCOM.BoLinkedObject.lf_None, SBO_Application)
                End If

                '                oitem = oform.Items.Add(mc_stretidNoVehiculo, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem = AgregaEditText(oform, mc_stretidNoVehiculo, 427, 81, 0, 0, Nothing)
                oitem.Visible = False
                oEdit = oitem.Specific
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDF2Num_Vehiculo)

                'Placa

                oitem = AgregaEditText(oform, mc_stretPlaca, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstPlaca)
                oEdit = oitem.Specific
                oEdit.Item.Width = 80
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFNumPlaca)
                'oEdit.ChooseFromListUID = "CFL5"
                'oEdit.ChooseFromListAlias = mc_strUDFPlaca
                oEdit.TabOrder = m_intTabOrder

                Call AgregaStatics(oform, mc_strstPlaca, My.Resources.Resource.CapPlaca, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretPlaca)

                intleftpicPlaca = oform.Items.Item(mc_stretPlaca).Left + oform.Items.Item(mc_stretPlaca).Width

                Call AgregaButtonPic(oform, mc_strpicbtDetalleVehiculos, c_intFirstColLeftEdit - 20, intTopActual - 7, c_intPanel, c_intPanel, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\Flecha.BMP", "")

                Call AgregaButtonPic(oform, mc_strbtpicNoPlaca, intleftpicPlaca + 1, intTopActual - 2, c_intPanel, c_intPanel, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "CFL1")

                'Estilo
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretEstilo, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstEstilo)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCotEstiloDesc)
                Call AgregaStatics(oform, mc_strstEstilo, My.Resources.Resource.CapEstilo, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretEstilo)

                'Año
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretAño, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstAño)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCotAñoVehiclo)
                Call AgregaStatics(oform, mc_strstAño, My.Resources.Resource.CapAño, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretAño)

                'Campos de la Recepcion del vehiculo primera Fila
                '**************************************************************************************************
                'Fecha Recepcion
                intTopActual += 32
                oitem = AgregaEditText(oform, mc_stretFechaRecepción, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstFechaRecepción)
                oEdit = oitem.Specific
                oEdit.Item.Width = 52
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFFech_Recep)
                Call AgregaStatics(oform, mc_strstFechaRecepción, My.Resources.Resource.CapFechaRecepción, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretFechaRecepción)
                oEdit.String = DMS_Connector.Helpers.FormatoFecha(System.DateTime.Today)
                oEdit.TabOrder = m_intTabOrder + 1

                'Fecha Compromiso
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretFechadeCompromiso, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstFechadeCompromiso)
                oEdit = oitem.Specific
                oEdit.Item.Width = 52
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFFech_Comp)
                Call AgregaStatics(oform, mc_strstFechadeCompromiso, My.Resources.Resource.CapFechaDeCompromiso, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretFechadeCompromiso)
                oEdit.String = DMS_Connector.Helpers.FormatoFecha(System.DateTime.Today)
                oEdit.TabOrder = m_intTabOrder + 2

                'Fecha Crea OT
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretFecCreOT, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstFecCreaOT)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 52
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFFecCreaOT)
                Call AgregaStatics(oform, mc_strstFecCreaOT, My.Resources.Resource.CapFechaDeCreaOT, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretFecCreOT)
                oEdit.TabOrder = m_intTabOrder + 3

                'GeneraOT
                intTopActual += 16
                oitem = AgregaCombobox(oform, mc_strcbGeneraOrden, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstGeneraOrden)
                oitem.DisplayDesc = True
                oCombo = oitem.Specific
                oCombo.Item.Width = 100
                Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFGenera_OT)
                Call AgregaStatics(oform, mc_strstGeneraOrden, My.Resources.Resource.CapGeneraOrden, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strcbGeneraOrden)
                oCombo.TabOrder = m_intTabOrder + 4

                'No Visita
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretNoVisita, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstNoVisita)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 80
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFNo_Visita)
                Call AgregaStatics(oform, mc_strstNoVisita, My.Resources.Resource.CapNoVisita, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretNoVisita)
                intleftpicPlaca = oform.Items.Item(mc_stretNoVisita).Left + oform.Items.Item(mc_stretNoVisita).Width
                Call AgregaButtonPic(oform, mc_strbtpicRecepcion, intleftpicPlaca + 1, intTopActual - 2, c_intPanel, c_intPanel, SAPbouiCOM.BoButtonTypes.bt_Image, System.Windows.Forms.Application.StartupPath.ToString & "\CFL.BMP", "CFL3")

                'Llamada Servicio
                intTopActual += 16
                oitem = AgregaEditText(oform, txtLlamadaServicio, c_intFirstColLeftEdit, intTopActual, c_intPanel, c_intPanel, String.Empty)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                oEdit.DataBind.SetBound(True, mc_strOQUT, udfLLamadaSvc)
                AgregaStatics(oform, lblLLamadaSvc, My.Resources.Resource.CapLLamadaServicio, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, txtLlamadaServicio)

                'Numero Proyecto
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretProyectoNombre, c_intFirstColLeftEdit - 25, intTopActual, c_intPanel, c_intPanel, mc_strstNumProyecto)
                oitem.Width = 100
                oEdit = oitem.Specific
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFProyectoNombre)
                oEdit.ChooseFromListUID = "CFL4"
                oEdit.ChooseFromListAlias = "PrjCode"
                Call AgregaStatics(oform, mc_strstNumProyecto, My.Resources.Resource.CapProyecto, c_intFirstColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretProyectoNombre, , True)
                oEdit.TabOrder = m_intTabOrder + 14

                'Botón Estado de Servicios

                oitem = oform.Items.Item("2")
                intHeight = oitem.Height
                intWidth = 65
                intTopActual = oitem.Top
                intLeftButton = oitem.Left

                intLeftButton = intLeftButton + 70
                oitem = AgregaButton(oform, mc_strbtEstadoServicios, intLeftButton, intTopActual, 0, 0, My.Resources.Resource.ButtonEstadoServicios, SAPbouiCOM.BoButtonTypes.bt_Caption)
                oitem.Width = 65
                oitem.Height = intHeight
                oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)

                'Botón Historial Vehiculo
                intLeftButton = intLeftButton + 65
                oitem = AgregaButton(oform, mc_strbtHistorialVehiculo, intLeftButton, intTopActual, 0, 0, My.Resources.Resource.ButtonHistorialVehiculo, SAPbouiCOM.BoButtonTypes.bt_Caption)
                oitem.Width = 65
                oitem.Height = intHeight
                oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)

                'Botón Balance OT
                intLeftButton = intLeftButton + 65
                oitem = AgregaButton(oform, mc_strbtBalanceOT, intLeftButton, intTopActual, 0, 0, My.Resources.Resource.ButtonBalanceOT, SAPbouiCOM.BoButtonTypes.bt_Caption)
                oitem.Width = 65
                oitem.Height = intHeight
                oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)


                intHeight = oform.Items.Item("10000330").Height
                intWidth = oform.Items.Item("10000330").Width
                intTopActual = oform.Items.Item("10000330").Top
                intLeftButton = oform.Items.Item("10000330").Left



                'Botón Recepcion Vehiculo
                intTopActual -= 23

                'intLeftButton = intLeftButton + 65
                oitem = AgregaButton(oform, mc_strbtRecepcionVehi, intLeftButton, intTopActual, 0, 0, My.Resources.Resource.ButtonRecepcionVehiculo, SAPbouiCOM.BoButtonTypes.bt_Caption)
                oitem.Width = 65
                oitem.Height = intHeight
                oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)

                If Not Utilitarios.MostrarMenu("SCGD_BBL", SBO_Application.Company.UserName) Then
                    oform.Items.Item("SCGD_btBal").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                Else
                    oform.Items.Item("SCGD_btBal").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
                End If

                'AgregaStatics(oform, mc_strstArchivos, My.Resources.Resource.LabelArchivosDigitales, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strbtArchivos)


                '                oitem = oform.Items.Add(lkLLamadaSvc, BoFormItemTypes.it_LINKED_BUTTON)
                '                oitem.Left = c_intFirstColLeftEdit - 15
                '                oitem.Top = intTopActual
                '                oitem.Width = 10
                '                oitem.Enabled = True
                '                oitem.FromPane = c_intPanel
                '                oitem.ToPane = c_intPanel
                '                '                oLink = DirectCast(oitem.Specific, LinkedButton)
                '                '                oLink.LinkedObject = BoLinkedObject.lf_ServiceCall
                '                oitem.LinkTo = txtLlamadaServicio

                '**************************************************************************************************

                'Segunda Columna
                oitem = oform.Items.Item(LabelReferencia)
                intTopActual = oitem.Top
                'Marca
                oitem = AgregaEditText(oform, mc_stretMarca, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstMarca)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCotMarcaDesc)
                Call AgregaStatics(oform, mc_strstMarca, My.Resources.Resource.CapMarca, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretMarca)


                'Modelo
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretModelo, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstModelo)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCotModeloDesc)
                Call AgregaStatics(oform, mc_strstModelo, My.Resources.Resource.CapModelo, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretModelo)

                'Vin
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretVIN, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstVIN)
                oitem.Enabled = False
                oitem.Width = 100
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFCotVin)
                'Call AgregaStatics(oform, mc_strstVIN, My.Resources.Resource.CapVIN, c_intSecondColStatic - 20, intTopActual, c_intPanel, c_intPanel, mc_stretVIN, True)
                Call AgregaStatics(oform, mc_strstVIN, My.Resources.Resource.CapVIN, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretVIN, True)

                'Campos de la recepcion  Segunda columna
                '**************************************************************************************************

                'Horas Servicio
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretHoraServicio, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstHorasServicio)
                'oitem.Width = 120
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFHorasServicio)
                Call AgregaStatics(oform, mc_strstHorasServicio, "Horas Serv.", c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretHoraServicio)
                oEdit.TabOrder = m_intTabOrder + 14

                'Hora Recepcion
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretHoraRecepcion, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstHoraRecepcion)
                oEdit = oitem.Specific
                oEdit.Item.Width = 47
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFHora_Recep)
                'Call AgregaStatics(oform, mc_strstHoraRecepcion, My.Resources.Resource.CapHoraRecepcion, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretHoraRecepcion)
                oEdit.String = CInt(Strings.StrDup(2 - CStr(System.DateTime.Now.Hour).Length, "0") & CStr(System.DateTime.Now.Hour) & Strings.StrDup(2 - CStr(System.DateTime.Now.Minute).Length, "0") & CStr(System.DateTime.Now.Minute))
                oEdit.TabOrder = m_intTabOrder + 5

                'Hora de compromiso
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretHoraCompra, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstHoraCompra)
                oEdit = oitem.Specific
                oEdit.Item.Width = 47
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFHora_Comp)
                'Call AgregaStatics(oform, mc_strstHoraCompra, My.Resources.Resource.CapHoraCompra, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretHoraCompra, False, False, False, True)
                oEdit.String = CInt(Strings.StrDup(2 - CStr(System.DateTime.Now.Hour).Length, "0") & CStr(System.DateTime.Now.Hour) & Strings.StrDup(2 - CStr(System.DateTime.Now.Minute).Length, "0") & CStr(System.DateTime.Now.Minute))
                oEdit.TabOrder = m_intTabOrder + 6

                'Hora Crea OT
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretHoraCreOT, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstHorCreaOT)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 47
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFHorCreaOT)
                'Call AgregaStatics(oform, mc_strstHorCreaOT, My.Resources.Resource.CapHoraDeCreaOT, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretHoraCreOT)
                oEdit.TabOrder = m_intTabOrder + 7

                'Estado
                intTopActual += 16
                oitem = AgregaCombobox(oform, mc_strcbEstado, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstEstadoCotizacion)
                oitem.DisplayDesc = True
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oCombo = oitem.Specific
                oCombo.Item.Width = 100
                Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFEstado_Cot)

                Call AgregaStatics(oform, mc_strstEstadoCotizacion, My.Resources.Resource.CapEstadoCotizacion, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_strcbEstado)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenNoIniciada, My.Resources.Resource.EstadoOrdenNoIniciada)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenEnproceso, My.Resources.Resource.EstadoOrdenEnproceso)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenFinalizada, My.Resources.Resource.EstadoOrdenFinalizada)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenSuspendida, My.Resources.Resource.EstadoOrdenSuspendida)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenCancelada, My.Resources.Resource.EstadoOrdenCancelada)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenFacturada, My.Resources.Resource.EstadoOrdenFacturada)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenEntregada, My.Resources.Resource.EstadoOrdenEntregada)
                oCombo.ValidValues.Add(My.Resources.Resource.EstadoOrdenCerrada, My.Resources.Resource.EstadoOrdenCerrada)

                'Genera OR
                intTopActual += 16
                oitem = AgregaCombobox(oform, mc_strcbGeneraRecepcion, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstGeneraRecepcion)
                oitem.DisplayDesc = True
                oCombo = oitem.Specific
                oCombo.Item.Width = 100
                Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFGeneraRecepcion)
                Call AgregaStatics(oform, mc_strstGeneraRecepcion, My.Resources.Resource.CapGeneraRecepcion, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_strcbGeneraRecepcion)
                oCombo.TabOrder = m_intTabOrder + 8

                'No Cita
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretNoSerie, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstNoSerie)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oitem.Width = 52
                oitem.AffectsFormMode = True
                oEdit = oitem.Specific
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFNoSerie)
                oEdit.TabOrder = m_intTabOrder + 9
                oitem = AgregaStatics(oform, mc_strstNoSerie, My.Resources.Resource.CapNoCita, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretNoSerie)

                'No Serie
                intleftpicPlaca = oform.Items.Item(mc_stretNoSerie).Left + oform.Items.Item(mc_stretNoSerie).Width

                oitem = AgregaEditText(oform, mc_stretNoCita, intleftpicPlaca, intTopActual, c_intPanel, c_intPanel, mc_strstNoCita)
                'oitem = AgregaEditText(oform, mc_stretNoCita, c_intSecondColStatic + 80, intTopActual, c_intPanel, c_intPanel, mc_strstNoCita)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oitem.Width = 47
                oitem.AffectsFormMode = True
                oEdit = oitem.Specific
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFNoCita)
                oEdit.TabOrder = m_intTabOrder + 10
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_True)

                'Fecha Cita
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretFechaCita, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstFechaCita)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 52
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFFecCita)
                Call AgregaStatics(oform, mc_strstFechaCita, My.Resources.Resource.CapFechaCita, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretFechaCita, False, False, False, True)
                oEdit.TabOrder = m_intTabOrder + 11

                'Fecha Cita
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretHoraCita, c_intSecondColEdit, intTopActual, c_intPanel, c_intPanel, mc_strstHoraCita)
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 47
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFHorCita)
                'Call AgregaStatics(oform, mc_strstHoraCita, My.Resources.Resource.CapHoraCita, c_intSecondColStatic, intTopActual, c_intPanel, c_intPanel, mc_stretHoraCita, False, False, False, True)
                oEdit.TabOrder = m_intTabOrder + 12


                '***************************************************************************************************
                'Archivos
                oitem = oform.Items.Item(LabelReferencia)
                intTopActual = oitem.Top - 3
                oitem = AgregaButton(oform, mc_strbtArchivos, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, My.Resources.Resource.ButtonArchivosDigitales, SAPbouiCOM.BoButtonTypes.bt_Caption)
                oitem.Width = 100
                '                oitem.Height = oitem.Height - 3
                oitem.Height = 19
                oButton = DirectCast(oitem.Specific, SAPbouiCOM.Button)
                AgregaStatics(oform, mc_strstArchivos, My.Resources.Resource.LabelArchivosDigitales, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strbtArchivos)

                'No Unidad
                intTopActual += 21
                oitem = AgregaEditText(oform, mc_stretNoUnidad, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstNoUnidad)
                oitem.Enabled = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, 11, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strCodUnidad)
                Call AgregaStatics(oform, mc_strstNoUnidad, My.Resources.Resource.CapNoUnidad, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretNoUnidad)

                'Gasolina
                intTopActual += 30
                oitem = AgregaCombobox(oform, mc_stretGasolina, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstGasolina)
                oitem.DisplayDesc = True
                oCombo = oitem.Specific
                oCombo.Item.Width = 100
                Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFGasolina)
                Call AgregaStatics(oform, mc_strstGasolina, My.Resources.Resource.CapGasolina, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretGasolina)
                oCombo.TabOrder = m_intTabOrder + 12

                'Kilometraje
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretKilometraje, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstKilometraje)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFKilometraje)
                Call AgregaStatics(oform, mc_strstKilometraje, My.Resources.Resource.CapKilometraje, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretKilometraje)
                oEdit.TabOrder = m_intTabOrder + 13

                'Tipo O.T
                intTopActual += 16
                oitem = AgregaCombobox(oform, mc_strcbTipoOT, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstTipoOrdendeTrabajo)
                oitem.DisplayDesc = True
                oCombo = oitem.Specific
                oCombo.Item.Width = 100
                oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFTipo_OT)
                Call AgregaStatics(oform, mc_strstTipoOrdendeTrabajo, My.Resources.Resource.CapTipoOrdendeTrabajo, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strcbTipoOT)
                oCombo.TabOrder = m_intTabOrder + 14

                'Cono
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretCono, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstCono)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFGorro_Veh)
                Call AgregaStatics(oform, mc_strstCono, My.Resources.Resource.CapCono, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretCono)
                oEdit.TabOrder = m_intTabOrder + 15

                'OT Referecia
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_stretOTReferencia, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstOTReferencia)
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFOTReferencia)
                Call AgregaStatics(oform, mc_strstOTReferencia, My.Resources.Resource.CapOTReferencia, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_stretOTReferencia)
                oEdit.TabOrder = m_intTabOrder + 16

                'Retorno Taller
                intTopActual += 16
                oitem = AgregaCombobox(oform, mc_strcbRetornoTaller, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, Nothing)
                oitem.DisplayDesc = True
                oCombo = oitem.Specific
                oCombo.Item.Width = 100
                Call oCombo.DataBind.SetBound(True, mc_strOQUT, mc_strUDFRetornoTaller)
                Call AgregaStatics(oform, mc_strstRetorTaller, My.Resources.Resource.CapRetornoTaller, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strcbRetornoTaller, False, False, False, True)

                'Garantia inicio
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_strEtGaranIni, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstGaranIni)
                oitem.DisplayDesc = True
                oitem.Enabled = False
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFGaranIni)
                Call AgregaStatics(oform, mc_strstGaranIni, My.Resources.Resource.TxtFGaranIni, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strEtGaranIni, False, False, False, True)

                'Garantia fin
                intTopActual += 16
                oitem = AgregaEditText(oform, mc_strEtGaranFin, c_intThirdColLeftEdit, intTopActual, c_intPanel, c_intPanel, mc_strstGaranFin)
                oitem.DisplayDesc = True
                oitem.Enabled = False
                oEdit = oitem.Specific
                oEdit.Item.Width = 100
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFGaranFin)
                Call AgregaStatics(oform, mc_strstGaranFin, My.Resources.Resource.TxtFGaranFin, c_intThirdColLeftStatic, intTopActual, c_intPanel, c_intPanel, mc_strEtGaranFin, False, False, False, True)

                'oCombo.TabOrder = m_intTabOrder + 14

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Los siguientes controles Edit no se muestran en pantalla
                'porque se utilizan para almacenar datos de ID unicamente

                '                oitem = oform.Items.Add(mc_stretidMarca, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem = AgregaEditText(oform, mc_stretidMarca, 1000, 20, c_intPanel, c_intPanel, Nothing)
                oitem.Visible = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDF2CodMarca)

                '                oitem = oform.Items.Add(mc_stretidModelo, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem = AgregaEditText(oform, mc_stretidModelo, 1000, 20, c_intPanel, c_intPanel, Nothing)
                oitem.Visible = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDF2CodModelo)

                '                oitem = oform.Items.Add(mc_stretidEstilo, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem = AgregaEditText(oform, mc_stretidEstilo, 1000, 20, c_intPanel, c_intPanel, Nothing)
                oitem.Visible = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDF2CodEstilo)

                '                oitem = oform.Items.Add(mc_stretIdasesor, SAPbouiCOM.BoFormItemTypes.it_EDIT)
                oitem = AgregaEditText(oform, mc_stretIdasesor, 1000, 20, c_intPanel, c_intPanel, Nothing)
                oitem.Visible = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDF2idEmpleado)
                oEdit.String = Utilitarios.ObtieneEmpid(SBO_Application)

                oitem = AgregaEditText(oform, mc_stretProyectoNumero, 1000, 20, c_intPanel, c_intPanel, Nothing)
                oitem.Visible = False
                oitem.SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Visible, BoAutoFormMode.afm_All, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                oEdit = oitem.Specific
                Call oEdit.DataBind.SetBound(True, mc_strOQUT, mc_strUDFProyecto)



                Call CargarValidValuesEnComboTipoOrden(oform)
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Private Function AgregaStatics(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal strCaption As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal strLinkTo As String, _
                                   Optional ByVal isVIN As Boolean = False, _
                                   Optional ByVal isProyecto As Boolean = False, _
                                   Optional ByVal isNomClieOT As Boolean = False, _
                                   Optional ByVal isExtraWidth As Boolean = False) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oStatic As SAPbouiCOM.StaticText
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_STATIC)
            oitem.Left = intLeft
            oitem.Top = intTop
            If isVIN Then
                oitem.Width = 40
            End If
            If isProyecto Then
                oitem.Width = 50
            End If
            If isNomClieOT Then
                oitem.Width = 100
            End If
            If isExtraWidth Then
                oitem.Width = 90
            End If
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oitem.LinkTo = strLinkTo
            oStatic = oitem.Specific
            oStatic.Caption = strCaption


            Return oitem

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("Error: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
        Return Nothing
    End Function

    Private Function AgregaEditText(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal strLinkTo As String) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.EditText
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_EDIT)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            If Not String.IsNullOrEmpty(strLinkTo) Then oitem.LinkTo = strLinkTo
            oEditText = oitem.Specific

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("Error: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try
    End Function

    Private Function AgregaCheckBox(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal strLinkTo As String) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oEditText As SAPbouiCOM.CheckBox
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            If Not String.IsNullOrEmpty(strLinkTo) Then oitem.LinkTo = strLinkTo
            oEditText = oitem.Specific

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("Error: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try
    End Function

    Private Function AgregaCombobox(ByRef oform As SAPbouiCOM.Form, _
                                   ByVal strNombrectrl As String, _
                                   ByVal intLeft As Integer, _
                                   ByVal intTop As Integer, _
                                   ByVal intFromPane As Integer, _
                                   ByVal intTopane As Integer, _
                                   ByVal strLinkTo As String) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        '        Dim oCombo As SAPbouiCOM.ComboBox
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oitem.LinkTo = strLinkTo
            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText("Error: " & ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try
    End Function

    Private Function AgregaButton(ByRef oform As SAPbouiCOM.Form, _
                                     ByVal strNombrectrl As String, _
                                     ByVal intLeft As Integer, _
                                     ByVal intTop As Integer, _
                                     ByVal intFromPane As Integer, _
                                     ByVal intTopane As Integer, _
                                     ByVal strCaption As String, _
                                     ByVal ButtonType As SAPbouiCOM.BoButtonTypes) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane

            oButton = oitem.Specific
            oButton.Type = ButtonType
            oButton.Caption = strCaption

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

    Private Function AgregaButtonPic(ByRef oform As SAPbouiCOM.Form, _
                                     ByVal strNombrectrl As String, _
                                     ByVal intLeft As Integer, _
                                     ByVal intTop As Integer, _
                                     ByVal intFromPane As Integer, _
                                     ByVal intTopane As Integer, _
                                     ByVal ButtonType As SAPbouiCOM.BoButtonTypes, _
                                     ByVal PathImagen As String, _
                                     ByVal UDO As String) As SAPbouiCOM.Item

        Dim oitem As SAPbouiCOM.Item
        Dim oButton As SAPbouiCOM.Button
        Try

            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oButton = oitem.Specific
            oButton.Type = ButtonType
            oitem.Width = 18
            oitem.Height = 18
            oitem.FromPane = intFromPane
            oitem.ToPane = intTopane
            oButton.Image = PathImagen

            If UDO <> "" Then
                oButton.ChooseFromListUID = UDO
            End If

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

    Private Shared Function AgregaLinkedButton(ByRef oform As SAPbouiCOM.Form, _
                                         ByVal strNombrectrl As String, _
                                         ByVal intLeft As Integer, _
                                         ByVal intTop As Integer, _
                                         ByVal intHeight As Integer, _
                                         ByVal intWidth As Integer, _
                                         ByVal strLinkTo As String, _
                                         ByVal LinkedObject As String, _
                                         ByVal objSBO_Application As SAPbouiCOM.Application) As SAPbouiCOM.Item
        Dim oitem As SAPbouiCOM.Item
        Dim oLinkedButton As SAPbouiCOM.LinkedButton
        Try


            oitem = oform.Items.Add(strNombrectrl, SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON)
            oitem.Left = intLeft
            oitem.Top = intTop
            oitem.Height = intHeight
            oitem.Width = intWidth
            oitem.LinkTo = strLinkTo
            oLinkedButton = oitem.Specific
            oLinkedButton.LinkedObjectType = LinkedObject

            Return oitem
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, objSBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            Return Nothing
        End Try

    End Function

    Private Sub AddChooseFromList(ByVal oform As SAPbouiCOM.Form)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition
            Dim editText As SAPbouiCOM.EditText
            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "SCGD_VEH"
            oCFLCreationParams.UniqueID = "CFL1"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL1
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_CardCode"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCFL.SetConditions(oCons)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "SCGD_VEH"
            oCFLCreationParams.UniqueID = "CFL5"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL5
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_CardCode"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCFL.SetConditions(oCons)


            oCFLCreationParams.UniqueID = "CFL2"
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = SAPbouiCOM.BoLinkedObject.lf_Employee
            oCFL = oCFLs.Add(oCFLCreationParams)


            oCFLCreationParams.UniqueID = "CFL3"
            oCFLCreationParams.MultiSelection = False
            'oCFLCreationParams.ObjectType = "23"
            oCFLCreationParams.ObjectType = "SCGD_OT"
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            'oCon.Alias = "U_SCGD_Num_Vehiculo"
            oCon.Alias = "U_NoUni"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCFL.SetConditions(oCons)

            oCFLCreationParams.UniqueID = "CFL4"
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = SAPbobsCOM.ServiceTypes.ProjectsService
            oCFL = oCFLs.Add(oCFLCreationParams)

            AplicaValorDeCondicion(0, oform, "CFL3", "")

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "2"
            oCFLCreationParams.UniqueID = "CFL6"
            oCFL = oCFLs.Add(oCFLCreationParams)


            'oCFLCreationParams.MultiSelection = False
            'oCFLCreationParams.ObjectType = "2"
            'oCFLCreationParams.UniqueID = "CFL7"
            'oCFL = oCFLs.Add(oCFLCreationParams)
            'editText = CType(oform.Items.Item(mc_strNClienteOT).Specific, EditText)
            'editText.ChooseFromListUID = oCFL.UniqueID


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub AplicaValorDeCondicion(ByVal intIndice As Integer, _
                                      ByVal oform As SAPbouiCOM.Form, _
                                      ByVal idCFL As String, _
                                      ByVal strValor As String)


        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList

        Try
            oCFL = oform.ChooseFromLists.Item(idCFL)
            oCons = oform.ChooseFromLists.Item(idCFL).GetConditions
            oCon = oCons.Item(intIndice)

            'oCon.BracketOpenNum = 1
            If strValor <> "" Then
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCon.CondVal = strValor
            Else
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                oCon.CondVal = "'" & CStr(-1) & ""
            End If

            If (idCFL = "CFL1" Or idCFL = "CFL5") And oCons.Count < 2 Then
                If oCons.Count = 1 And strValor = "" Then
                    oCon.BracketOpenNum = 1
                    oCon.Relationship = BoConditionRelationship.cr_OR
                    oCon = oCons.Add()
                    oCon.Alias = "U_CardCode"
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_IS_NULL
                    oCon.BracketCloseNum = 1
                End If

                oCon.Relationship = BoConditionRelationship.cr_AND
                oCon = oCons.Add
                oCon.Alias = "U_Activo"
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCon.CondVal = "Y"
            End If

            oCFL.SetConditions(oCons)
        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'Call SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub


    Public Sub SeleccionarFolderInicial()

        Dim oform As SAPbouiCOM.Form

        oform = SBO_Application.Forms.ActiveForm
        oform.Items.Item(mc_strfdRecepcion).Specific.Select()

    End Sub

    Private Sub CargaValoresDevehiculo(ByVal strNoVehiculo As String, _
                                       ByRef udtVehiculo As VehiculoUDT)
        Try

            Dim strVehiculoNuevo As String = "Select  [Code]" & _
                                              ",[U_Num_Plac]" & _
                                              ",[U_Cod_Esti]" & _
                                              ",[U_Des_Esti]" & _
                                              ",[U_Ano_Vehi]" & _
                                              ",[U_Cod_Marc]" & _
                                              ",[U_Des_Marc]" & _
                                              ",[U_Cod_Mode]" & _
                                              ",[U_Des_Mode]" & _
                                              ",[U_Num_VIN]" & _
                                              ",U_Cod_Unid" & _
                                              ",[U_Km_Unid] " & _
                                              ",[U_HorSer] " & _
                                              ",[U_GaranIni] " & _
                                              ",[U_GaranFin], U_CardCode, U_CardName " & _
                                              " FROM [@SCGD_VEHICULO]" & _
                                              " WHERE Code='" & strNoVehiculo & "'"

            Dim drdResultadoConsulta As SqlClient.SqlDataReader
            Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
            Dim strConectionString As String = ""
            Dim cn_Coneccion As New SqlClient.SqlConnection

            Configuracion.CrearCadenaDeconexion(m_ocompany.Server, m_ocompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strVehiculoNuevo
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            Do While drdResultadoConsulta.Read

                With udtVehiculo


                    If Not (drdResultadoConsulta.Item(mc_strUDFPlaca) Is System.Convert.DBNull) Then

                        .NoPlaca = drdResultadoConsulta.Item(mc_strUDFPlaca)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFEstiloDesc) Is System.Convert.DBNull) Then

                        .DescEstilo = drdResultadoConsulta.Item(mc_strUDFEstiloDesc)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFMarcaDesc) Is System.Convert.DBNull) Then

                        .DescMArca = drdResultadoConsulta.Item(mc_strUDFMarcaDesc)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFModeloDesc) Is System.Convert.DBNull) Then

                        .DescModelo = drdResultadoConsulta.Item(mc_strUDFModeloDesc)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFAñoVehiclo) Is System.Convert.DBNull) Then

                        .Año = drdResultadoConsulta.Item(mc_strUDFAñoVehiclo)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFVin) Is System.Convert.DBNull) Then

                        .Vin = drdResultadoConsulta.Item(mc_strUDFVin)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFNoUnidad) Is System.Convert.DBNull) Then

                        .NoUnidad = drdResultadoConsulta.Item(mc_strUDFNoUnidad)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFNumVehi) Is System.Convert.DBNull) Then

                        .NumVehiculo = drdResultadoConsulta.Item(mc_strUDFNumVehi)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFCodEstilo) Is System.Convert.DBNull) Then

                        .CodEstilo = drdResultadoConsulta.Item(mc_strUDFCodEstilo)

                    End If


                    If Not (drdResultadoConsulta.Item(mc_strUDFCodMarca) Is System.Convert.DBNull) Then

                        .CodMarca = drdResultadoConsulta.Item(mc_strUDFCodMarca)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFCodModelo) Is System.Convert.DBNull) Then

                        .CodModelo = drdResultadoConsulta.Item(mc_strUDFCodModelo)

                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFKM_Unid) Is System.Convert.DBNull) Then
                        .KmUnidad = drdResultadoConsulta.Item(mc_strUDFKM_Unid)
                    End If


                    If Not (drdResultadoConsulta.Item(mc_strUDFHorasServicioVH) Is System.Convert.DBNull) Then
                        .HoraServicio = drdResultadoConsulta.Item(mc_strUDFHorasServicioVH)
                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFVehiGaranIni) Is System.Convert.DBNull) Then
                        .GarantiaInicio = drdResultadoConsulta.Item(mc_strUDFVehiGaranIni)
                    End If

                    If Not (drdResultadoConsulta.Item(mc_strUDFVehiGaranFin) Is System.Convert.DBNull) Then
                        .GarantiaFin = drdResultadoConsulta.Item(mc_strUDFVehiGaranFin)
                    End If

                    If Not drdResultadoConsulta.Item("U_CardName") Is System.Convert.DBNull Then
                        .strNameCliente = drdResultadoConsulta.Item("U_CardName")
                    End If

                    If Not drdResultadoConsulta.Item("U_CardCode") Is System.Convert.DBNull Then
                        .strCodeCliente = drdResultadoConsulta.Item("U_CardCode")
                    End If

                End With

            Loop

            drdResultadoConsulta.Close()
            cn_Coneccion.Close()

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            'SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub

    Protected Friend Sub CargarValidValuesEnComboTipoOrden(ByRef oForm As SAPbouiCOM.Form, _
                                                           Optional strIdSucursal As String = "", _
                                                           Optional p_blnUsaConfiguracionInternaTaller As Boolean = False)

        Dim intRecIndex As Integer
        Dim cboCombo As SAPbouiCOM.ComboBox
        Dim oItem As SAPbouiCOM.Item
        Dim strQueryTipos As String
        Dim drdResultadoConsulta As SqlClient.SqlDataReader
        Dim cmdEjecutarConsulta As New SqlClient.SqlCommand
        Dim strConectionString As String = ""
        Dim cn_Coneccion As New SqlClient.SqlConnection

        Try
            oItem = oForm.Items.Item(mc_strcbTipoOT)
            cboCombo = CType(oItem.Specific, SAPbouiCOM.ComboBox)

            Configuracion.CrearCadenaDeconexion(m_ocompany.Server, m_ocompany.CompanyDB, strConectionString)
            cn_Coneccion.ConnectionString = strConectionString

            If Not p_blnUsaConfiguracionInternaTaller Then
                strQueryTipos = "Select Code, Name from [@SCGD_TIPO_ORDEN] Order By Code"

            Else
                strQueryTipos = "SELECT [@SCGD_CONF_TIP_ORDEN].U_Code, [@SCGD_CONF_TIP_ORDEN].U_Name " & _
                                "FROM   [@SCGD_CONF_SUCURSAL] INNER JOIN " & _
                                "[@SCGD_CONF_TIP_ORDEN] ON [@SCGD_CONF_SUCURSAL].DocEntry = [@SCGD_CONF_TIP_ORDEN].DocEntry " & _
                                "WHERE ([@SCGD_CONF_SUCURSAL].U_Sucurs = '" & strIdSucursal & "')"
            End If


            cn_Coneccion.Open()
            cmdEjecutarConsulta.Connection = cn_Coneccion
            cmdEjecutarConsulta.CommandType = CommandType.Text
            cmdEjecutarConsulta.CommandText = strQueryTipos
            drdResultadoConsulta = cmdEjecutarConsulta.ExecuteReader()

            'Borra los ValidValues de los combos. 
            If cboCombo.ValidValues.Count > 0 Then
                For intRecIndex = 0 To cboCombo.ValidValues.Count - 1
                    cboCombo.ValidValues.Remove(cboCombo.ValidValues.Item(0).Value, SAPbouiCOM.BoSearchKey.psk_ByValue)
                Next
            End If
            'Agregar Valid Values
            Do While drdResultadoConsulta.Read
                If Not drdResultadoConsulta.IsDBNull(0) AndAlso Not drdResultadoConsulta.IsDBNull(1) Then
                    cboCombo.ValidValues.Add(drdResultadoConsulta.GetString(0).Trim, drdResultadoConsulta.GetString(1).Trim)
                End If
            Loop


            drdResultadoConsulta.Close()
            cn_Coneccion.Close()


        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
            Throw ex
        End Try

    End Sub

    ''********************************************************************************************
    ''Agregado 27/02/2012: Limpiar UDFs al duplicar


    'Public Sub Duplicar(ByRef oForm As SAPbouiCOM.Form)

    '    'Limpiar UDF de OT
    '    oForm.Items.Item("SCGD_etOT").Specific.value = String.Empty


    '    'Limpiar UDFs de pestaña de Recepción

    '    oForm.Items.Item(mc_stretPlaca).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretEstilo).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretAño).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretNoVisita).Specific.value = String.Empty
    '    oForm.Items.Item(txtLlamadaServicio).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretMarca).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretModelo).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretVIN).Specific.value = String.Empty
    '    'oForm.Items.Item(mc_strcbEstado).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretNoSerie).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretNoCita).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretNoUnidad).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretKilometraje).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretCono).Specific.value = String.Empty
    '    oForm.Items.Item(mc_stretOTReferencia).Specific.value = String.Empty


    '    'Asignar fechas y horas por defecto
    '    AsignaValoresdeRecepcionControlesUIDefecto(oForm)

    '    oForm.Items.Item(mc_stretHoraRecepcion).Specific.String = CStr((System.DateTime.Now.Hour) & (System.DateTime.Now.Minute))

    '    oForm.Items.Item(mc_stretHoraCompra).Specific.String = CStr((System.DateTime.Now.Hour) & (System.DateTime.Now.Minute))

    '    'Asignar valores por defecto a combo box

    '    Dim oitem As SAPbouiCOM.Item
    '    Dim oCombo As SAPbouiCOM.ComboBox

    '    oitem = oForm.Items.Item(mc_strcbGeneraOrden)
    '    oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
    '    oCombo.Select("2", BoSearchKey.psk_ByValue)

    '    oitem = oForm.Items.Item(mc_strcbGeneraRecepcion)
    '    oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
    '    oCombo.Select("1", BoSearchKey.psk_ByValue)

    '    oitem = oForm.Items.Item(mc_stretGasolina)
    '    oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
    '    oCombo.Select("5", BoSearchKey.psk_ByValue)

    '    'oitem = oForm.Items.Item(mc_strcbEstado)
    '    'oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
    '    'oCombo.Select("0", BoSearchKey.psk_ByValue)

    '    oitem = oForm.Items.Item(mc_strcbEstado)
    '    oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
    '    oCombo.Select(String.Empty, BoSearchKey.psk_ByValue)

    '    oitem = oForm.Items.Item(mc_strcbTipoOT)
    '    oCombo = DirectCast(oitem.Specific, SAPbouiCOM.ComboBox)
    '    oCombo.Select(String.Empty, BoSearchKey.psk_ByValue)

    'End Sub


    ''********************************************************************************************

    ' ''********************************************************************************************

    'Agregado 06/03/2012: Cargar campos del vehículo en el formulario , con el evento "intro"
    'Autor: José Soto


    Public Sub Enter_Placa(ByVal NoVehiculo As String, ByRef Form As SAPbouiCOM.Form, Optional ByVal Placa As String = "")
        'Modificado 14/60/2013: Se agrega parametro codigo de unidad. Erick Sanabria

        Try

            Dim dtVehiculo As SAPbouiCOM.DataTable
            Dim m_blnVariosRegistros As Boolean = False
            Dim strVehiculoNuevo As String = String.Empty
            Dim blnEsPlaca As Boolean = False

            If String.IsNullOrEmpty(NoVehiculo) And String.IsNullOrEmpty(Placa) Then
                Placa = "----"
            End If

            dtVehiculo = Form.DataSources.DataTables.Item("dtVehiculo")

            If Not String.IsNullOrEmpty(NoVehiculo) Then
                strVehiculoNuevo = String.Format(" Select  U_Num_Plac, U_Des_Esti, U_Des_Marc, U_Des_Mode, U_Ano_Vehi, U_Num_VIN, U_Cod_Unid, Code, U_Cod_Esti, " & _
                                                 " U_Cod_Marc, U_Cod_Mode, U_Km_Unid, U_GaranIni, U_GaranFin, U_CardCode, U_CardName " & _
                                                 " FROM [@SCGD_VEHICULO] with(nolock) WHERE U_Cod_Unid = '{0}'", NoVehiculo)
                blnEsPlaca = False
            ElseIf Not String.IsNullOrEmpty(Placa) Then
                strVehiculoNuevo = String.Format(" Select  U_Num_Plac, U_Des_Esti, U_Des_Marc, U_Des_Mode, U_Ano_Vehi, U_Num_VIN, U_Cod_Unid, Code, U_Cod_Esti, " & _
                                                 " U_Cod_Marc, U_Cod_Mode, U_Km_Unid, U_GaranIni, U_GaranFin, U_CardCode, U_CardName " & _
                                                 " FROM [@SCGD_VEHICULO] with(nolock) WHERE U_Num_Plac = '{0}'", Placa)
                blnEsPlaca = True
            End If

            dtVehiculo.ExecuteQuery(strVehiculoNuevo)

            m_udtVehiculo = New VehiculoUDT

            If dtVehiculo.Rows.Count > 1 Then m_blnVariosRegistros = True

            With m_udtVehiculo

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFPlaca, 0))) Then
                    .NoPlaca = dtVehiculo.GetValue(mc_strUDFPlaca, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFEstiloDesc, 0))) Then
                    .DescEstilo = dtVehiculo.GetValue(mc_strUDFEstiloDesc, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFMarcaDesc, 0))) Then
                    .DescMArca = dtVehiculo.GetValue(mc_strUDFMarcaDesc, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFModeloDesc, 0))) Then
                    .DescModelo = dtVehiculo.GetValue(mc_strUDFModeloDesc, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFAñoVehiclo, 0))) Then
                    .Año = dtVehiculo.GetValue(mc_strUDFAñoVehiclo, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFVin, 0))) Then
                    .Vin = dtVehiculo.GetValue(mc_strUDFVin, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFNoUnidad, 0))) Then
                    .NoUnidad = dtVehiculo.GetValue(mc_strUDFNoUnidad, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFNumVehi, 0))) Then
                    .NumVehiculo = dtVehiculo.GetValue(mc_strUDFNumVehi, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFCodEstilo, 0))) Then
                    .CodEstilo = dtVehiculo.GetValue(mc_strUDFCodEstilo, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFCodMarca, 0))) Then
                    .CodMarca = dtVehiculo.GetValue(mc_strUDFCodMarca, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFCodModelo, 0))) Then
                    .CodModelo = dtVehiculo.GetValue(mc_strUDFCodModelo, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFKM_Unid, 0))) Then
                    .KmUnidad = dtVehiculo.GetValue(mc_strUDFKM_Unid, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFVehiGaranIni, 0))) Then
                    .GarantiaInicio = dtVehiculo.GetValue(mc_strUDFVehiGaranIni, 0)
                End If

                If Not (String.IsNullOrEmpty(dtVehiculo.GetValue(mc_strUDFVehiGaranFin, 0))) Then
                    .GarantiaFin = dtVehiculo.GetValue(mc_strUDFVehiGaranFin, 0)
                End If

                If Not dtVehiculo.GetValue("U_CardName", 0) Is System.Convert.DBNull Then
                    .strNameCliente = dtVehiculo.GetValue("U_CardName", 0)
                End If

                If Not dtVehiculo.GetValue("U_CardCode", 0) Is System.Convert.DBNull Then
                    .strCodeCliente = dtVehiculo.GetValue("U_CardCode", 0)
                End If

            End With

            If (m_blnVariosRegistros) Then
                SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorMultiplesPlacas, SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                m_udtVehiculo = Nothing
            End If

            AsignaValoresdeVehiculoaControlesUI(m_udtVehiculo, Form, blnEsPlaca)

        Catch ex As Exception
            Call Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try

    End Sub

    ' ''********************************************************************************************

    ''' <summary>
    ''' Agrega el choosefromlist al formulario
    ''' </summary>
    ''' <param name="oform">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub AddChooseFromListColaboradores(ByVal oform As Form)
        Try
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition

            oCFLs = oform.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = "171"
            oCFLCreationParams.UniqueID = "CFL_Col"
            oCFL = oCFLs.Add(oCFLCreationParams)

            ' Adding Conditions to CFL1
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "U_SCGD_T_Fase"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
            oCFL.SetConditions(oCons)

        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    ''' <summary>
    ''' Maneja el evento del choosefromlist para los colaboradores
    ''' </summary>
    ''' <param name="pVal">Objeto Evento</param>
    ''' <param name="oDataTable">Data Table con la info del ChooseFromList</param>
    ''' <param name="oform">objeto formulario</param>
    ''' <remarks></remarks>
    Private Sub ManejaCFLColaboradores(ByVal pVal As ItemEvent, ByVal oDataTable As SAPbouiCOM.DataTable, ByVal oform As SAPbouiCOM.Form)
        Dim oitem As SAPbouiCOM.Item
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim strEmpID As String = String.Empty
        Dim strEmpleado As String = String.Empty

        Try
            oform.Select()
            oform.Freeze(False)

            oitem = oform.Items.Item("38")
            oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)
            Select Case pVal.ColUID
                Case "U_SCGD_EmpAsig"

                    strEmpID = oDataTable.GetValue("empID", 0).ToString().Trim()
                    strEmpleado = oDataTable.GetValue("firstName", 0).ToString().Trim() + " " + oDataTable.GetValue("lastName", 0).ToString().Trim()

                    oMatrix.Columns.Item("U_SCGD_NombEmpleado").Cells.Item(pVal.Row).Specific.Value = strEmpleado
                    oMatrix.Columns.Item("U_SCGD_EmpAsig").Cells.Item(pVal.Row).Specific.Value = strEmpID

            End Select
        Catch ex As Exception
            If ex.Message <> "Form - Bad Value" Then
                Utilitarios.ManejadorErrores(ex, SBO_Application)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Valida el CFLColaboradores
    ''' </summary>
    ''' <param name="pVal">Objeto Evento</param>
    ''' <param name="oform">Objeto Formulario</param>
    ''' <param name="BubbleEvent">Evento Burbuja</param>
    ''' <remarks></remarks>
    Private Sub ValidaCFLColaboradores(ByVal pVal As ItemEvent, ByVal oform As SAPbouiCOM.Form, ByRef BubbleEvent As Boolean)

        Dim strCodigoItem As String = String.Empty
        Dim strTipoArticulo As String = String.Empty
        Dim strEstadoNoProcesado As String = String.Empty
        Dim strEstadoActividad As String = String.Empty
        Dim strIDActividadxOrden As String = String.Empty
        Dim m_strBDTalller As String = String.Empty
        Dim strCampoTipoArticulo As String = "U_SCGD_TipoArticulo"
        Dim oMatrix As SAPbouiCOM.Matrix
        Dim oitem As SAPbouiCOM.Item
        Dim blError As Boolean = False

        Try
            If pVal.ColUID = "U_SCGD_EmpAsig" Then

                Utilitarios.DevuelveNombreBDTaller(SBO_Application, oform.DataSources.DBDataSources.Item("OQUT").GetValue("U_SCGD_idSucursal", 0).TrimEnd, m_strBDTalller)

                oitem = oform.Items.Item("38")
                oMatrix = DirectCast(oitem.Specific, SAPbouiCOM.Matrix)

                strCodigoItem = oMatrix.Columns.Item("1").Cells.Item(pVal.Row).Specific.value

                strTipoArticulo = DevuelveValorItem(strCodigoItem, strCampoTipoArticulo)

                If strTipoArticulo.Trim() <> "2" Then
                    SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorNoEsServicio, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                    BubbleEvent = False
                    blError = True
                End If


                If Not blError Then

                    strIDActividadxOrden = oMatrix.Columns.Item("U_SCGD_IdRepxOrd").Cells.Item(pVal.Row).Specific.value
                    strEstadoActividad = Utilitarios.EjecutarConsulta(String.Format(" SELECT Estado FROM [SCGTA_TB_ActividadesxOrden] WHERE ID = '{0}'", strIDActividadxOrden),
                                                                      m_strBDTalller,
                                                                      m_ocompany.Server)

                    strEstadoNoProcesado = Utilitarios.EjecutarConsulta(" SELECT name FROM [@SCGD_ESTADOS_ACTOT] WHERE code = '1' ",
                                                                      m_ocompany.CompanyDB,
                                                                      m_ocompany.Server)

                    If strEstadoActividad.ToLower().Trim() <> strEstadoNoProcesado.ToLower().Trim() And _
                        Not String.IsNullOrEmpty(strEstadoActividad.Trim()) Then

                        SBO_Application.StatusBar.SetText(My.Resources.Resource.ErrorEstadoServicio, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Error)
                        BubbleEvent = False

                    End If
                End If

            End If
        Catch ex As Exception
            Utilitarios.ManejadorErrores(ex, SBO_Application)
        End Try
    End Sub

    Private Function DevuelveValorItem(ByVal strItemcode As String, _
                                      ByVal strUDfName As String) As String

        Dim oItemArticulo As SAPbobsCOM.IItems
        Dim valorUDF As String

        oItemArticulo = m_ocompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems)
        oItemArticulo.GetByKey(strItemcode)
        valorUDF = oItemArticulo.UserFields.Fields.Item(strUDfName).Value

        Return valorUDF

    End Function

    Public Sub ManejadorEventoLinkPress(ByRef pval As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByRef formOT As SCG.ServicioPostVenta.OrdenTrabajo)

        Dim oform As SAPbouiCOM.Form = SBO_Application.Forms.GetForm(pval.FormTypeEx, pval.FormTypeCount)
        Dim OtId As String = String.Empty
        Dim oMatriz As SAPbouiCOM.Matrix
        Try
            OtId = (oform.Items.Item("SCGD_etOT").Specific).Value.ToString().Trim()
            formOT.CargarOT(OtId)
            ''se setea en false la variable bubble event para que el action success no llame nuevamente el evento cargar ot
            BubbleEvent = False
        Catch ex As Exception
            Throw
        End Try


    End Sub

End Class
