

using System;
using SAPbobsCOM;

namespace SCG.Requisiciones.UI
{
    public class ManejadorArticulos
    {
        public string ItemCode { get; set; }
        public string WhsCode { get; set; }

        public ICompany CompanySBO { get; private set; }

        public ManejadorArticulos(ICompany companySBO)
        {
            CompanySBO = companySBO;
        }

        public float CantidadDisponible()
        {
            Items items = (Items) CompanySBO.GetBusinessObject(BoObjectTypes.oItems);
            if (items.GetByKey(ItemCode))
            {
                for (int i = 0; i < items.WhsInfo.Count && items.WhsInfo.WarehouseCode != WhsCode; i++)
                    items.WhsInfo.SetCurrentLine(i);
                return (float) (items.WhsInfo.InStock + items.WhsInfo.Ordered - items.WhsInfo.Committed);
            }
            throw new InvalidOperationException(string.Format("Item {0} does not exist",ItemCode));
        }

        public bool PermiteStockNegativo()
        {
            CompanyService companyService = CompanySBO.GetCompanyService();
            CompanyInfo companyInfo = companyService.GetCompanyInfo();
            return companyInfo.BlockStockNegativeQuantity == BoYesNoEnum.tNO;
        }

        public Boolean  CantidadDisponibleItemEspecifico(string strItemCode,  string strWhsCode) 
        {
            Items items = (Items)CompanySBO.GetBusinessObject(BoObjectTypes.oItems);
            float decDisponible = 0;
            if (items.GetByKey(strItemCode))
            {
                for (int i = 0; i < items.WhsInfo.Count; i++)
                
                {
                    items.WhsInfo.SetCurrentLine(i);

                    if (items.WhsInfo.WarehouseCode == strWhsCode)
                    {
                        decDisponible = (float)(items.WhsInfo.InStock + items.WhsInfo.Ordered - items.WhsInfo.Committed);
                    }

                }

               

                if (decDisponible == 0)
                {
                    return true;


                }
                else
                {
                    return false;
                }


            }
            throw new InvalidOperationException(string.Format("Item {0} does not exist", ItemCode));
        }

        //SE COMENTA PARA EL PROCESO DE UBICACIONES
        //public virtual int  UbicacionArticuloPorDefecto(string strItemCode)
        //{
        //    Items items = (Items)CompanySBO.GetBusinessObject(BoObjectTypes.oItems);
        //    int intUbicacionDefecto = 0;
        //    if (items.GetByKey(strItemCode))
        //    {
        //        for (int i = 0; i < items.WhsInfo.Count; i++)
        //        {
        //            items.WhsInfo.SetCurrentLine(i);
        //            intUbicacionDefecto = items.WhsInfo.DefaultBin; 
        //        }
                                 
        //        if (intUbicacionDefecto == 0)
        //        {
        //            return intUbicacionDefecto ;
        //        }
        //        else
        //        {
        //            return intUbicacionDefecto;
        //        }
        //    }
        //    throw new InvalidOperationException(string.Format("Item {0} does not exist", ItemCode));
        //}


    }
}