using System;

namespace SCG.Requisiciones
{
    public class EncabezadoRequisicion
    {
        public int DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime CreateDate { get; set; }
        public string NoOrden { get; set; }
        public string CodigoCliente { get; set; }
        public string NombreCliente { get; set; }
        public int CodigoTipoRequisicion { get; set; }
        public string TipoRequisicion { get; set; }
        public string TipoDocumento { get; set; }
        public string Usuario { get; set; }
        public string Comentarios { get; set; }
        public string Data { get; set; }
        public bool Cancelada { get; set; }
        public string IDSucursal { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Estilo { get; set; }
        public string VIN { get; set; }
        public string TipoArticulo { get; set; }
        public String ComentariosUser { get; set; }
        public string NoSerieCita { get; set; }

        //        public object Clone()
        //        {
        //            var encabezadoRequisicion = new EncabezadoRequisicion
        //                                            {
        //                                                CodigoCliente = CodigoCliente,
        //                                                CodigoTipoRequisicion = CodigoTipoRequisicion,
        //                                                CreateDate = CreateDate,
        //                                                DocEntry = DocEntry,
        //                                                DocNum = DocNum,
        //                                                NombreCliente = NombreCliente,
        //                                                NoOrden = NoOrden,
        //                                                TipoDocumento = TipoDocumento,
        //                                                TipoRequisicion = TipoRequisicion,
        //                                                Usuario = Usuario
        //                                            };
        //            return encabezadoRequisicion;
        //        }
    }
}
