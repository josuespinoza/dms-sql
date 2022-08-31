using System;
using SCG.SBOFramework.DI;

namespace SCG.DMSOne.Framework
{
    public class EncabezadoUDOVehiculos : IEncabezadoUDO
    {
        public EncabezadoUDOVehiculos()
        {
            TablaLigada = "SCGD_VEHICULO";
        }

        [UDOBind("Code", Key = true)]
        public string Code { get; set; }

        [UDOBind("DocEntry", SoloLectura = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_Cod_Unid")]
        public string NoUnidad { get; set; }

        [UDOBind("U_Cod_Marc")]
        public string CodigoMarca { get; set; }

        [UDOBind("U_Des_Marc")]
        public string Marca { get; set; }

        [UDOBind("U_Cod_Mode")]
        public string CodigoModelo { get; set; }

        [UDOBind("U_Des_Mode")]
        public string Modelo { get; set; }

        [UDOBind("U_Cod_Esti")]
        public string CodigoEstilo { get; set; }

        [UDOBind("U_Des_Esti")]
        public string Estilo { get; set; }

        [UDOBind("U_Ano_Vehi")]
        public int Ano { get; set; }

        [UDOBind("U_Num_Plac")]
        public string Placa { get; set; }

        [UDOBind("U_Cod_Col")]
        public string CodigoColor { get; set; }

        [UDOBind("U_Des_Col")]
        public string Color { get; set; }

        [UDOBind("U_ColorTap")]
        public string ColorTapiceria { get; set; }

        [UDOBind("U_Num_VIN")]
        public string NumeroChasis { get; set; }

        [UDOBind("U_Num_Mot")]
        public string NumeroMotor { get; set; }

        [UDOBind("U_MarcaMot")]
        public string MarcaMotor { get; set; }

        [UDOBind("U_Cant_Pas")]
        public int CantidadPasajeros { get; set; }

        [UDOBind("U_Cod_Ubic")]
        public string CodigoUbicacion { get; set; }

        [UDOBind("U_Tipo")]
        public string Tipo { get; set; }

        [UDOBind("U_Estatus")]
        public string Estado { get; set; }

        [UDOBind("U_Tipo_Tra")]
        public string TipoTraccion { get; set; }

        [UDOBind("U_Num_Cili")]
        public int Cilindros { get; set; }

        [UDOBind("U_TipTecho")]
        public string TipoTecho { get; set; }

        [UDOBind("U_Carrocer")]
        public string Carroceria { get; set; }

        [UDOBind("U_CantPuer")]
        public int Puertas { get; set; }

        [UDOBind("U_Peso")]
        public int Peso { get; set; }

        [UDOBind("U_Cilindra")]
        public int Cilindrada { get; set; }

        [UDOBind("U_Categori")]
        public string Categoria { get; set; }

        [UDOBind("U_Combusti")]
        public string Combustible { get; set; }

        [UDOBind("U_Cant_Eje")]
        public string Ejes { get; set; }

        [UDOBind("U_Tip_Cabi")]
        public string Cabina { get; set; }

        [UDOBind("U_Potencia")]
        public int Potencia { get; set; }

        [UDOBind("U_Transmis")]
        public string Transmision { get; set; }

        [UDOBind("U_Accesori")]
        public string Accesorios { get; set; }

        [UDOBind("U_GarantKM")]
        public int GarantiaKm { get; set; }

        [UDOBind("U_GarantTM")]
        public int GarantiaTiempo { get; set; }

        [UDOBind("U_CardCode")]
        public string CodigoCliente { get; set; }

        [UDOBind("U_CardName")]
        public string NombreCliente { get; set; }

        [UDOBind("U_FechaVen")]
        public DateTime FechaVenta { get; set; }

        [UDOBind("U_CTOVTA")]
        public int NumContrato { get; set; }

        [UDOBind("U_VTADOL")]
        public double VentaDolares { get; set; }

        [UDOBind("U_VTACOL")]
        public double VentaColones { get; set; }

        [UDOBind("U_FCHINV")]
        public DateTime FechaIngresoInventario { get; set; }

        [UDOBind("U_NUMFAC")]
        public int NumeroFacturaVenta { get; set; }

        [UDOBind("U_TIPINV", ValorPredeterminado = "S")]
        public string TipoInventario { get; set; }

        [UDOBind("U_FCHRES")]
        public DateTime Fecha_Reserva { get; set; }

        [UDOBind("U_OBSRES")]
        public string ObservacionesReservas { get; set; }

        [UDOBind("U_ARREST")]
        public string ArriboEstimado { get; set; }

        [UDOBind("U_FECFINR")]
        public DateTime FechaFinalReserva { get; set; }

        [UDOBind("U_SALINID")]
        public double SaldoInicialDolares { get; set; }

        [UDOBind("U_SALINIC")]
        public double SaldoInicialColones { get; set; }

        [UDOBind("U_FLELOC")]
        public double FleteLocalColones { get; set; }

        [UDOBind("U_TIPCAM")]
        public double TipoCambio { get; set; }

        [UDOBind("U_COSINV")]
        public double CostoInventarioColones { get; set; }

        [UDOBind("U_VALHAC")]
        public double ValorHacienda { get; set; }

        [UDOBind("U_GASTRA")]
        public double GastosTraspaso { get; set; }

        [UDOBind("U_Dispo")]
        public int Disponibilidad { get; set; }

        [UDOBind("U_VENRES")]
        public string Vendedor { get; set; }

        [UDOBind("U_Cod_Fab")]
        public string CodigoFabrica { get; set; }

        [UDOBind("U_Tipo_Ven")]
        public string TipoVenta { get; set; }

        [UDOBind("U_Precio")]
        public double Precio { get; set; }

        [UDOBind("U_FchUSv")]
        public DateTime FechaUltimoServicio { get; set; }

        [UDOBind("U_FchPrSv")]
        public DateTime FechaProxServicio { get; set; }

        [UDOBind("U_FchRsva")]
        public DateTime FechaReserva { get; set; }

        [UDOBind("U_FchVcRva")]
        public DateTime FechaVencidoReserva { get; set; }

        [UDOBind("U_NoPedFb")]
        public string NumeroPedidoFabrica { get; set; }

        [UDOBind("U_FrecSvc")]
        public int FrecuenciaServicio { get; set; }

        [UDOBind("U_fechaSync")]
        public string FechaUltimaActualizacion { get; set; }

        [UDOBind("U_ArtVent")]
        public string ArticuloVenta { get; set; }

        [UDOBind("U_Num_VIN")]
        public string Vin { get; set; }

        [UDOBind("U_Moneda")]
        public string Moneda { get; set; }

        [UDOBind("U_ArtVentDesc")]
        public string DescArticuloVenta { get; set; }

        [UDOBind("U_Des_Col_Tap")]
        public string DescColorTap { get; set; }

        [UDOBind("U_Km_Unid")]
        public int Kilometraje { get; set; }

        [UDOBind("U_Clasificacion")]
        public string Clasificacion { get; set; }

        [UDOBind("U_Estado_Nuevo")]
        public string EstadoNuevo { get; set; }

        [UDOBind("U_Fha_Ing_Inv")]
        public DateTime FechaIngInventario { get; set; }

        [UDOBind("U_Cli_Ven")]
        public string CodigoClienteVenta { get; set; }

        [UDOBind("U_ClNo_Ven")]
        public string NombreClienteVenta { get; set; }

        [UDOBind("U_Tipo_Reing")]
        public string TipoInventarioReing { get; set; }

        [UDOBind("U_CosPro")]
        public double CostoProyectado { get; set; }

        [UDOBind("U_DiEje")]
        public string U_DiEje { get; set; }

        [UDOBind("U_CCar")]
        public string U_CCar { get; set; }

        [UDOBind("U_Pote")]
        public string U_Pote { get; set; }

        [UDOBind("U_Ramv")]
        public string U_Ramv { get; set; }

        [UDOBind("U_ValorNet")]
        public Double U_ValorNet { get; set; }

        [UDOBind("U_Bono")]
        public Double U_Bono { get; set; }

        [UDOBind("U_HorSer")]
        public int U_HorSer { get; set; }

        [UDOBind("U_DocRecepcion")]
        public string U_DocRecepcion { get; set; }

        [UDOBind("U_Cod_Prov")]
        public string CodigoProveedor { get; set; }

        [UDOBind("U_Nom_Prov")]
        public string NombreProveedor { get; set; }

        [UDOBind("U_DocPedido")]
        public string U_DocPedido { get; set; }

        [UDOBind("U_TipoCo")]
        public string U_TipoCo { get; set; }



        #region IEncabezadoUDO Members

        public string TablaLigada { get; private set; }

        #endregion
    }
}