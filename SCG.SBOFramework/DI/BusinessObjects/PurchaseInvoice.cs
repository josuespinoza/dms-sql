///Autor: Werner F.R.
///Fecha: 09/03/2012

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;

namespace SCG.SBOFramework.DI.BusinessObjects
{
    public class PurchaseInvoice
    {
        private SAPbobsCOM.Company _company;
        public SAPbobsCOM.Documents Entity { get; set; }

        public PurchaseInvoice(SAPbobsCOM.Company company)
        {
            _company = company;
            New();
        }

        public void New()
        {
            Entity = (SAPbobsCOM.Documents)_company.GetBusinessObject(BoObjectTypes.oPurchaseInvoices);
        }

        public void Add()
        {
            int returnCode = Entity.Add();

            if (returnCode != 0)
            {
                throw new SboUncessfullOperationException(returnCode, _company.GetLastErrorDescription(), "PurchaseInvoice.Add");
            }
        }

        public bool GetByKey(int docEntry)
        {
            return Entity.GetByKey(docEntry);
        }
    }
}