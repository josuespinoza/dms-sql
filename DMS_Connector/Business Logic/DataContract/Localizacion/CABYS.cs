using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS_Connector.Business_Logic.DataContract.Localizacion
{
    public class CABYS
    {
        public String CABYS_AE { get; set; }
        public String CABYS_TI { get; set; }
        public String CABYS_CH { get; set; }
        public String CodigoUnidad { get; set; }
        public String CodigoArticulo { get; set; }
        public String TipoInventario { get; set; }
        public String CodigoHaciendaUDO { get; set; }
        public Double TasaIVA { get; set; }
        public String IndicadorIVA { get; set; }
        public String CardCode { get; set; }
        public String OrigenTributario { get; set; }
        public String TipoExoneracion { get; set; }
        public List<CodigosHacienda> CodigosHacienda { get; set; }
    }
}
