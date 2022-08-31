using System;
using System.Runtime.InteropServices;
using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    public class UserDefinedFieldsManager
    {
        protected UserFieldsMD SBOUserFieldsMd { get; set; }
        protected ICompany Company { get; set; }
        
        public UserDefinedFieldsManager(ICompany company, UserFieldsMD sboUserFieldsMd)
        {
            Company = company;
            SBOUserFieldsMd = sboUserFieldsMd;
        }

        /// <summary>
        /// Sets or returns the default value of the field.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Sets or returns the field name. 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Sets or returns the description of the field. 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Sets or returns the name of the parent table that this field refers to.
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Sets or returns the data type, which describes the nature of the data, of the specified field.
        /// </summary>
        public UserDefinedFieldType DefinedFieldType { get; set; }

        /// <summary>
        /// Sets or returns a boolean value that determines wether or not this User Field is mandatory in SAP Business One.
        /// </summary>
        public bool IsMandatory { get; set; }
        /// <summary>
        /// Removes a specified field from the table.
        /// </summary>
        public void Remove(int fieldId)
        {
            SBOUserFieldsMd.GetByKey(TableName, fieldId);
            var code = SBOUserFieldsMd.Remove();
            SBOUserFieldsMd.ReleaseComObject();
            if (code != 0)
                throw new SboUncessfullOperationException(code, Company.GetLastErrorDescription(), "UserFieldsMD.Remove");
        }

        /// <summary>
        /// Add a user defined table to the table.
        /// </summary>
        public void Add()
        {
            SBOUserFieldsMd.TableName = TableName;
            SBOUserFieldsMd.Name = Name;
            SBOUserFieldsMd.Description = Description;
            SBOUserFieldsMd.DefaultValue = DefaultValue;
            SBOUserFieldsMd.Type = DefinedFieldType.ConvertToBoFieldTypes();
            SBOUserFieldsMd.Mandatory = IsMandatory.ConvertToBoYesNoEnum();
            var code = SBOUserFieldsMd.Add();
            SBOUserFieldsMd.ReleaseComObject();
            if (code != 0)
                throw new SboUncessfullOperationException(code, Company.GetLastErrorDescription(), "UserFieldsMD.Add");
        }
    }
}