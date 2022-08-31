using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public partial class CitasEnIntervaloSemanal : UserControl
    {
        public List<IElementoCita> ElementosCitas { get; set; }
        public ManipuladorEditarElementoCita EditaElementoCita { get; set; }
        public IAgenda Agenda { get; set; }

        public CitasEnIntervaloSemanal()
        {
            InitializeComponent();
            ElementosCitas = new List<IElementoCita>();
        }

        private void CitasEnIntervalo_Load(object sender, EventArgs e)
        {
            CargaElementosCitas();
        }

        public void CargaElementosCitas()
        {
            if (ElementosCitas != null)
            {
                flowLayoutPanelCitas.Controls.Clear();
                foreach (IElementoCita elementoCita in ElementosCitas)
                {
                    Label label = new Label();
                    label.AutoSize = true;
                    label.BorderStyle =  BorderStyle.Fixed3D;
                    label.BackColor = Color.FromArgb(194, 252, 233);
                    label.Margin = new Padding(0);
                    if (Agenda != null && elementoCita.Filtro != null && elementoCita.Filtro.ConfiguracionesPorAgenda.ContainsKey(Agenda.IdAgenda))
                    {
                        label.BackColor = elementoCita.Filtro.ConfiguracionesPorAgenda[Agenda.IdAgenda].Color;
                    }
                    label.Text = elementoCita.Descripcion;
                    label.Tag = elementoCita;
                    label.DoubleClick += label_DoubleClick;
                    flowLayoutPanelCitas.Controls.Add(label);
                }
            }
        }

        void label_DoubleClick(object sender, EventArgs e)
        {
            if (EditaElementoCita != null)
            {
                Label label = (Label)sender;
                var elementoCita = (IElementoCita)label.Tag;
                EditaElementoCita(elementoCita, ParentForm, flowLayoutPanelCitas, label);
                if (elementoCita.ModificadoPorUsuario)
                {
                    ElementosCitas.Remove(elementoCita);
                    flowLayoutPanelCitas.Controls.Remove(label);
                    elementoCita.ModificadoPorUsuario = false;
                }
            }
        }
    }
}
