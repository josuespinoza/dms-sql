using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS_Connector.Business_Logic.DataContract.Requisiciones
{
    [Serializable()]
    public class RequisicionData
    {
        public List<LineaRequisicion> LineasRequisicion { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int TipoArticulo { get; set; }
        public int CodigoEstadoLinea { get; set; }
        public int Check { get; set; }
        public int LineNumOrigen { get; set; }
        public int DocumentoOrigen { get; set; }
        public int CodigoEstadoRequisicion { get; set; }
        public int CodigoTipoRequisicion { get; set; }
        public int Serie { get; set; }

        public bool Procesar { get; set; }
        public bool UsaUbicaciones { get; set; }
        public bool RequisicionDevolucion { get; set; }
        public bool Aplicado { get; set; }

        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string DescripcionTipoArticulo { get; set; }
        public string NoOrden { get; set; }
        public string Entregado { get; set; }
        public string ID { get; set; }
        public string BodegaOrigen { get; set; }
        public string BodegaDestino { get; set; }
        public string UbicacionDBP { get; set; }
        public string EstadoLinea { get; set; }
        public string CentroCosto { get; set; }
        public string UbicacionOrigen { get; set; }
        public string UbicacionDestino { get; set; }
        public string BodegaUbicacion { get; set; }
        public string LineaSucursalID { get; set; }
        public string EstadoRequisicion { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public string TipoRequisicion { get; set; }
        public string TipoDocumento { get; set; }
        public string Usuario { get; set; }
        public string Comentario { get; set; }
        public string Data { get; set; }
        public string SucursalID { get; set; }
        public string IdRepxOrd { get; set; }
        public string ComentariosUser { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Estilo { get; set; }
        public string VIN { get; set; }

        public double CantidadDisponible { get; set; }
        public double CantidadTransferir { get; set; }
        public double CantidadOriginal { get; set; }
        public double CantidadAjuste { get; set; }
        public double CantidadRecibida { get; set; }
        public double CantidadSolicitada { get; set; }
        public double CantidadPendiente { get; set; }
        public double CantidadPendienteBodega { get; set; }
        public double CantidadPendienteDevolucion { get; set; }

        public DateTime CreateDate { get; set; }
    }
}