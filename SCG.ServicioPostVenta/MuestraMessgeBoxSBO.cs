using System;
using SAPbouiCOM;

namespace SCG.ServicioPostVenta
{
    public class MuestraMessgeBoxSBO
    {
        private static SAPbouiCOM.Application _sboApplication;

        public MuestraMessgeBoxSBO(SAPbouiCOM.Application sboApplication)
        {
            _sboApplication = sboApplication;
            //Del g = MessageBxPreg;
        }

        public delegate void Del(string message);

        public bool MessageBxPreg(String mensaje)
        {
            return _sboApplication.MessageBox(Text: mensaje, DefaultBtn: 1, Btn1Caption: Resource.Si, Btn2Caption: "No", Btn3Caption: "") == 1;
        }

        public void MessageBxExc(string mensaje)
        {
            _sboApplication.MessageBox(Text: mensaje, DefaultBtn: 1, Btn1Caption: "Ok");
        }
    }
}
