using System.Runtime.InteropServices;
using SAPbobsCOM;

namespace SCG.SBOFramework.DI
{
    public static class DIExtensionMethods
    {
        public static void ReleaseComObject(this IUserFieldsMD userFieldsMD)
        {
            Marshal.ReleaseComObject(userFieldsMD);
        }

        public static void ReleaseComObject(this IUserTablesMD userTablesMD)
        {
            Marshal.ReleaseComObject(userTablesMD);
        }

        public static void ReleaseComObject(this IUserObjectsMD userObjectsMD)
        {
            Marshal.ReleaseComObject(userObjectsMD);
        }

    }
}