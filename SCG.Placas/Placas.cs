using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbobsCOM;

namespace SCG.Placas
{
    public abstract class Placas
    {
        public ICompany Company { get; private set; }

        protected Placas(ICompany company)
        {
            Company = company;
        }
    }
}
