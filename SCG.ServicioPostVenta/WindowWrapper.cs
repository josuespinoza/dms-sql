using System;

namespace SCG.ServicioPostVenta
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window 
    {
        private IntPtr _hwnd;

        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public System.IntPtr Handle 
        {
            get
            {
                return _hwnd;
            }
        }
    }
}
