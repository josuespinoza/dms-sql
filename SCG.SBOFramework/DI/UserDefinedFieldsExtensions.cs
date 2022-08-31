using System;
using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    public static class UserDefinedFieldsExtensions
    {
        public static BoFieldTypes ConvertToBoFieldTypes(this UserDefinedFieldType definedFieldType)
        {
            switch (definedFieldType)
            {
                    case UserDefinedFieldType.Alpha: return BoFieldTypes.db_Alpha;
                    case UserDefinedFieldType.Date: return BoFieldTypes.db_Date;
                    case UserDefinedFieldType.Float: return BoFieldTypes.db_Float;
                    case UserDefinedFieldType.Memo: return BoFieldTypes.db_Memo;
                    case UserDefinedFieldType.Numeric: return BoFieldTypes.db_Numeric;
            }
            throw new InvalidOperationException("Enum not valid");
        }

        public static BoYesNoEnum ConvertToBoYesNoEnum(this bool b)
        {
            return b ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
        }

    }
}