using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOContratoVenta : IEncabezadoUDO
    {
        [UDOBind("Code")]
        public string Code { get; set; }

        [UDOBind("DocEntry", SoloLectura = true)]
        public string DocEntry { get; set; }

        [UDOBind("U_CardCode")]
        public string CodigoCliente { get; set; }

        [UDOBind("U_CardName")]
        public string NombreCliente { get; set; }

        [UDOBind("U_Tipo")]
        public int TipoContrato { get; set; }

        [UDOBind("U_Estado")]
        public int EstadoContrato { get; set; }

        [UDOBind("U_Opcion")]
        public string OpcionesContrato { get; set; }

        [UDOBind("U_Pre_Vta")]
        public int PrecioVenta { get; set; }

        [UDOBind("U_Ext_Adi")]
        public int Extras { get; set; }

        [UDOBind("U_Gas_Ins")]
        public int GastosInscripcion { get; set; }

        [UDOBind("U_Gas_Seg")]
        public int GastosSeguro { get; set; }

        [UDOBind("U_Gas_Pre")]
        public int GastosPrenda { get; set; }

        [UDOBind("U_DocTotal")]
        public int Total { get; set; }

        [UDOBind("U_Saldo")]
        public int Saldo { get; set; }

        [UDOBind("U_Observ")]
        public string Observaciones { get; set; }

        [UDOBind("U_Dat_Pre")]
        public string DatosPrenda { get; set; }

        [UDOBind("U_SlpCode")]
        public int CodigoVendedor { get; set; }

        [UDOBind("U_SlpName")]
        public string NombreVendedor { get; set; }

        [UDOBind("U_DocDate")]
        public DateTime FechaDocumento { get; set; }

        [UDOBind("U_Fec_Ent")]
        public DateTime FechaEntrega { get; set; }

        [UDOBind("U_Cod_Unid")]
        public string CodigoUnidad { get; set; }

        [UDOBind("U_Cod_Marc")]
        public string CodigoMarca { get; set; }

        [UDOBind("U_Des_Marc")]
        public string DescripcionMarca { get; set; }

        [UDOBind("U_Cod_Mode")]
        public string CodigoModelo { get; set; }

        [UDOBind("U_Des_Mode")]
        public string DescripcionModelo { get; set; }

        [UDOBind("U_Cod_Esti")]
        public string CodigoEstilo { get; set; }

        [UDOBind("U_Des_Esti")]
        public string DescripcionEstilo { get; set; }

        [UDOBind("U_Ano_Vehi")]
        public int AñoVehiculo { get; set; }

        [UDOBind("U_Num_Plac")]
        public string NumeroPlaca { get; set; }

        [UDOBind("U_Cod_Col")]
        public string CodigoColor { get; set; }

        [UDOBind("U_Des_Col")]
        public string DescripcionColor { get; set; }

        [UDOBind("U_Num_VIN")]
        public string NumeroVIN { get; set; }

        [UDOBind("U_Num_Mot")]
        public string NumeroMotor { get; set; }

        [UDOBind("U_Mar_Brt")]
        public int MargenBruto { get; set; }

        [UDOBind("U_Mon_Fin")]
        public int MontoFinanciamiento { get; set; }

        [UDOBind("U_Ent_Fin")]
        public string EnteFinanciero { get; set; }

        [UDOBind("U_Mon_pre")]
        public int MontoPrenda { get; set; }

        [UDOBind("U_Plazo")]
        public int Plazo { get; set; }

        [UDOBind("U_Tas_Anu")]
        public int TasaAnual { get; set; }

        [UDOBind("U_Abo_Men")]
        public int AbonoMensual { get; set; }

        [UDOBind("U_Cuo_Tot")]
        public int CuotaTotal { get; set; }

        [UDOBind("U_Seg_Pre")]
        public int Seguro { get; set; }

        [UDOBind("U_Obs_Pre")]
        public string ObservacionesPrenda { get; set; }

        [UDOBind("U_Unid_Us")]
        public string UnidadVehiculoUsado { get; set; }

        [UDOBind("U_Marc_Us")]
        public string MarcaVehiculoUsado { get; set; }

        [UDOBind("U_Esti_us")]
        public string EstiloVehiculoUsado { get; set; }

        [UDOBind("U_Mot_Us")]
        public string MotorVehiculoUsado { get; set; }

        [UDOBind("U_VIN_Us")]
        public string ChasisVehiculoUsado { get; set; }

        [UDOBind("U_Anio_Us")]
        public int AñoVehiculoUsado { get; set; }

        [UDOBind("U_Plac_Us")]
        public string PlacaVehiculoUsado { get; set; }

        [UDOBind("U_Col_Us")]
        public string ColorVehiculoUsado { get; set; }

        [UDOBind("U_Obs_Us")]
        public string ObservacionesVehiculoUsado { get; set; }

        [UDOBind("U_IDVehi")]
        public string IDVehiculoNuevo { get; set; }

        [UDOBind("U_Deposito")]
        public int Deposito { get; set; }

        [UDOBind("U_Mon_Usa")]
        public int MontoUsado { get; set; }

        [UDOBind("U_Deu_Usa")]
        public int DeudasUsado { get; set; }

        [UDOBind("U_Nota_Cre")]
        public int NotaCredito { get; set; }

        [UDOBind("U_Pag_ent")]
        public int PagosPorEntrada { get; set; }

        [UDOBind("U_Aval_us")]
        public DateTime Avaluo { get; set; }

        [UDOBind("U_RTV_MM")]
        public int RTVMes { get; set; }

        [UDOBind("U_RTV_AAAA")]
        public int RTVAno { get; set; }

        [UDOBind("U_Gravamen")]
        public int Gravamen { get; set; }

        [UDOBind("U_Val_Rec")]
        public int ValorRecibe { get; set; }

        [UDOBind("U_Val_Inv")]
        public int ValorInventario { get; set; }

        [UDOBind("U_Usu_Tra")]
        public string UsuarioTramite { get; set; }

        [UDOBind("U_Usu_Ven")]
        public string UsuarioVentas { get; set; }

        [UDOBind("U_Usu_Gen")]
        public string UsuarioGerencia { get; set; }

        [UDOBind("U_Usu_Fac")]
        public string UsuarioFacturacion { get; set; }

        [UDOBind("U_Usu_Can")]
        public string UsuarioCancela { get; set; }

        [UDOBind("U_Fec_Tra")]
        public DateTime FechaTramita { get; set; }

        [UDOBind("U_Fec_Ven")]
        public DateTime FechaVentas { get; set; }

        [UDOBind("U_Fec_Gen")]
        public DateTime FechaGerencia { get; set; }

        [UDOBind("U_Fec_Fan")]
        public DateTime FechaFacturacion { get; set; }

        [UDOBind("U_Fec_Can")]
        public DateTime FechaCancelada { get; set; }

        [UDOBind("U_Det_Ext")]
        public string DetalleExtras { get; set; }

        [UDOBind("U_Gra_Fec")]
        public DateTime FechaGravamen { get; set; }

        [UDOBind("U_Financia")]
        public int Financiamiento { get; set; }

        [UDOBind("U_Gas_Loc")]
        public int GastosLocales { get; set; }

        [UDOBind("U_Cos_Acc")]
        public int Accesorios { get; set; }

        [UDOBind("U_No_Fac")]
        public string NoFactura { get; set; }

        [UDOBind("U_Obs_GV")]
        public string ObservacionesGerenteVentas { get; set; }

        [UDOBind("U_Obs_GG")]
        public string ObservacionesGerenteGeneral { get; set; }

        [UDOBind("U_Moneda")]
        public string Moneda { get; set; }

        [UDOBind("U_Ent_FiP")]
        public string EnteFinancieroPrenda { get; set; }

        [UDOBind("U_Tip_Tasa")]
        public string TipoTasa { get; set; }

        [UDOBind("U_Der_Cir")]
        public DateTime DerechoCirculacion { get; set; }

        [UDOBind("U_Cat_Us")]
        public string CategoriaVehiculoUsado { get; set; }

        [UDOBind("U_Tip_Us")]
        public string TipoVehiculoUsado { get; set; }

        [UDOBind("U_Trac_Us")]
        public string TraccionVehiculoUsado { get; set; }

        [UDOBind("U_Cab_Us")]
        public string CabinaVehiculoUsado { get; set; }

        [UDOBind("U_Prop_Us")]
        public string PropietarioRegistral { get; set; }

        [UDOBind("U_CEnt_Fi")]
        public string CodigoEnteFinanciero { get; set; }

        [UDOBind("U_CEnt_FP")]
        public string CodigoEnteFinancieroPrenda { get; set; }

        [UDOBind("U_CCI_Veh")]
        public string CodigoClienteVehiculo { get; set; }

        [UDOBind("U_NCI_Veh")]
        public string NombreClienteVehiculo { get; set; }

        [UDOBind("U_Fec_1Ab")]
        public DateTime FechaPrimerAbono { get; set; }

        [UDOBind("U_GroupNum")]
        public int Periodo { get; set; }

        [UDOBind("U_Cod_FP")]
        public string FacturaProveedor { get; set; }

        [UDOBind("U_Nota_Deb")]
        public int NotaDebito { get; set; }

        [UDOBind("U_Cod_NotD")]
        public string CodigoNotaDebito { get; set; }

        [UDOBind("U_Nam_Acre")]
        public string NombreAcreedor { get; set; }

        [UDOBind("U_Cod_Nota")]
        public string CodigoNotaCredito { get; set; }

        [UDOBind("U_Cod_N_Us")]
        public string NotaUsado { get; set; }

        [UDOBind("U_ID_VehUs")]
        public string IdVehiculoUsado { get; set; }

        [UDOBind("U_Reaproba")]
        public string Reaprobacion { get; set; }

        [UDOBind("U_Cod_Acre")]
        public string CodigoAcreedor { get; set; }

        [UDOBind("U_OwrCode")]
        public string CodigoTitular { get; set; }

        [UDOBind("U_OwrName")]
        public string NombreTitular { get; set; }

        [UDOBind("U_Transmis")]
        public string Transmision { get; set; }

        [UDOBind("U_Cod_A_DU")]
        public string AjusteDeudaUsado { get; set; }

        [UDOBind("U_Cod_A_Co")]
        public string AsientoAjusteCosto { get; set; }

        [UDOBind("U_Cod_OV")]
        public string CodigoOportunidadVenta { get; set; }

        [UDOBind("U_Name_OV")]
        public string DescripcionOportunidadVenta { get; set; }

        [UDOBind("U_Otros_C")]
        public int OtrosDesgloseCosto { get; set; }

        [UDOBind("U_Otros_L")]
        public int OtrosLineasFactura { get; set; }

        [UDOBind("U_Pagos")]
        public int Pagos { get; set; }

        [UDOBind("U_PrecioVeh")]
        public int PrecioVehiculo { get; set; }

        [UDOBind("U_Foo")]
        public string Foo { get; set; }

        [UDOBind("U_SCGD_CodCotiz")]
        public string CodigoCotizacion { get; set; }

        [UDOBind("U_SCGD_NameCotiz")]
        public string NombreCotizacion { get; set; }

        [UDOBind("U_SCGD_TipoCambio")]
        public int TipoCambio { get; set; }

        [UDOBind("U_SCGD_NoSalida")]
        public string NumeroSalida { get; set; }

        [UDOBind("U_SCGD_DocPreliminar")]
        public string DocumentoPreliminarSalida { get; set; }

        [UDOBind("U_Reversa")]
        public string ContratoReversado { get; set; }

        [UDOBind("U_FooVend")]
        public string CFLVendedor { get; set; }

        [UDOBind("U_FacTram")]
        public string FacturaTramite { get; set; }

        [UDOBind("U_AsTraFc")]
        public string AsientoTramitesFact { get; set; }

        [UDOBind("U_NoFPVU")]
        public string FacturaProveedorVU { get; set; }

        [UDOBind("U_AsFPVU")]
        public string AsientoFacturaProveedorVU { get; set; }

        [UDOBind("U_ImpUsado")]
        public string ImpuestoUsado { get; set; }

        [UDOBind("U_TotalCImpuest")]
        public string TotalCImpuest { get; set; }

        #region IEncabezadoUDO Members

        public string TablaLigada
        {
            get { return "SCGD_CVENTA"; }
        }

        #endregion
    }
}