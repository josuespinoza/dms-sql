using System;

namespace SCGDMSWebApp.UserControl
{
    public partial class UserControl_SCGWOStatusDisplay : System.Web.UI.UserControl
    {


        /// <summary>
        /// direccion url para redireccionar el item
        /// </summary>
        public string RedirectUrl
        { get; set; }

        /// <summary>
        /// tiempo en milisegundos  para refrescar
        /// </summary>
        public int MillisecondsLoop
        {
            get { return scgtimer.Interval; }
            set { scgtimer.Interval = value;}
        }

        public object Datasource
        {
            get { return Repeater2.DataSource; }
            set { Repeater2.DataSource = value;
                Repeater2.DataBind();} 

        }


        /// <summary>
        /// 
        /// </summary>
        public UserControl_SCGWOStatusDisplay()
        {
        
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Repeater2.DataSource = Datasource;
            // Repeater2.DataBind();

    
        }


   
        //public  string devolverMilisegundos()
        //{
        //    return MillisecondsLoop.ToString();
        //}


        public string DevolverColor(int porcentaje)
        {
            if (porcentaje == 0)
            { 
                return
                    "barraceleste.png";
            }
            else if (porcentaje < 75)
            {
                return

                    "barraverde.png";
            }
            else if
                (porcentaje >= 100)
            {
                return
                    "barraroja.png";
            }
            else
            {
                return
                    "barraamarilla.png";
            }
        }



        /// <summary>
        /// despliega el warning pegado a lso estados
        /// </summary>
        /// <param name="porcentaje"></param>
        /// <returns></returns>
        public string DisplayWarning(int porcentaje)
        {

            if (porcentaje == 102 || porcentaje == 76 || porcentaje == 20)
            {
                return "'warning.gif'";

            }

            else
            {
                return "'warningBlanco.png'";
            }

        }



        public string Redirect(string orderID, string Status)
        {




            return RedirectUrl +"?OT=" + orderID.Split('#')[1] + "&STAT=" + Status;


        }


        protected void scgtimer_Tick(object sender, EventArgs e)
        {
            Repeater2.DataSource = Datasource;
            Repeater2.DataBind();
        
        }
    }
}
