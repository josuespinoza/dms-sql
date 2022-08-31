using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public partial class CitasEnIntervaloMensual : UserControl
    {
        public List<IElementoCita> ElementosCitas { get; set; }
        public DateTime Fecha { get; set; }
        public ManipuladorEditarElementoCita EditaElementoCita { get; set; }
        public IAgenda Agenda { get; set; }

        public CitasEnIntervaloMensual()
        {
            InitializeComponent();
            ElementosCitas = new List<IElementoCita>();
       }

        public void CargaElementosCitas()
        {
            labelDia.Text = string.Format("{0:dd MMM}", Fecha);
            labelDia.BorderStyle = BorderStyle.FixedSingle;
            if (ElementosCitas != null)
            {
                flowLayoutPanelCitas.Controls.Clear();
                foreach (IElementoCita elementoCita in ElementosCitas)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.BorderStyle =  BorderStyle.Fixed3D;
                    if (Agenda != null && elementoCita.Filtro != null && elementoCita.Filtro.ConfiguracionesPorAgenda.ContainsKey(Agenda.IdAgenda))
                    {
                        label.BackColor = elementoCita.Filtro.ConfiguracionesPorAgenda[Agenda.IdAgenda].Color;
                    }
                    label.Margin = new Padding(0);
                    label.Text = elementoCita.Descripcion;
                    label.DoubleClick += label_DoubleClick;
                    label.Tag = elementoCita;
                    flowLayoutPanelCitas.Controls.Add(label);
                }
            }
        }

        private void label_DoubleClick(object sender, EventArgs e)
        {
            if (EditaElementoCita != null)
            {
                Label label = (Label) sender;
                var elementoCita = (IElementoCita) label.Tag;
                EditaElementoCita(elementoCita, ParentForm, flowLayoutPanelCitas, label);
                if (elementoCita.ModificadoPorUsuario)
                {
                    ElementosCitas.Remove(elementoCita);
                    flowLayoutPanelCitas.Controls.Remove(label);
                    elementoCita.ModificadoPorUsuario = false;
                }
            }
        }

        private void CitasEnIntervaloMensual_Load(object sender, EventArgs e)
        {
            CargaElementosCitas();
        }

    }
}
