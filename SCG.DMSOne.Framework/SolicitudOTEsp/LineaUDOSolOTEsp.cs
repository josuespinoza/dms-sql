using System.Collections.Generic;
using SCG.SBOFramework.DI;
using System;

namespace SCG.DMSOne.Framework
{
    public class LineaUDOSolOTEsp : ILineaUDO
    {

        #region ... Propiedades ...

        [UDOBind("DocEntry", SoloLectura = true, Key = true)]
        public int DocEntry { get; set; }

        [UDOBind("U_ItemCode")]
        public String ItemCode { get; set; }

        [UDOBind("U_Descrip")]
        public String Description { get; set; }

        [UDOBind("U_PorcDs")]
        public Double PorcentajeDescuento { get; set; }

        [UDOBind("U_Moned")]
        public String Moneda { get; set; }

        [UDOBind("U_Precio")]
        public Double Precio { get; set; }

        [UDOBind("U_Coment")]
        public String Comentarios { get; set; }

        [UDOBind("U_IdRxO")]
        public int IdRepuestosXOrden { get; set; }

        [UDOBind("U_Costo")]
        public Double Costo { get; set; }

        [UDOBind("U_Cant")]
        public Double Cantidad { get; set; }

        [UDOBind("U_Tax")]
        public String Impuestos { get; set; }

        [UDOBind("U_Selec", ValorPredeterminado = "N")]
        public String Seleccionar { get; set; }

        [UDOBind("U_CPen")]
        public Double CantPendiente { get; set; }

        [UDOBind("U_CSol")]
        public Double CantSolicitada { get; set; }

        [UDOBind("U_CRec")]
        public Double CantRecibida { get; set; }

        [UDOBind("U_CPDe")]
        public Double CantPendDevolucion { get; set; }

        [UDOBind("U_CPTr")]
        public Double CantPendTraslado { get; set; }

        [UDOBind("U_CPBo")]
        public Double CantPendBodega { get; set; }

        [UDOBind("U_Compra")]
        public String Compra { get; set; }

        [UDOBind("U_ID_Linea")]
        public String IDLinea { get; set; }
        
        [UDOBind("U_TipArtSO")]
        public String TipoArticulo { get; set; }

        #endregion

    }
}
