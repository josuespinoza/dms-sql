using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public delegate void ManipuladorEditarElementoCita(IElementoCita elementoCita, Form formularioPadre, FlowLayoutPanel flowLayoutPanel, Control control);

    public partial class AgendaPropuestaCitas : UserControl
    {
        private AdministradorPropuestasCitas _administradorPropuestasCitas;
        private TipoAgenda _modoAgenda;
        private string _diasDeLaSemana = "Lunes|Martes|Miércoles|Jueves|Viernes|Sábado|Domingo";
        private readonly AdministradorAgendaSemanal _agendaSemanal;
        private readonly AdministradorAgendaMensual _agendaMensual;

        private bool _marcando;

        public AgendaPropuestaCitas(AdministradorPropuestasCitas administradorPropuestasCitas)
        {
            InitializeComponent();
            tableLayoutCalendario.AutoSize = false;
            tableLayoutCalendario.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            _administradorPropuestasCitas = administradorPropuestasCitas;
            TituloIntervaloSemana = "Semana del {0:dd MMM} al {1:dd MMM}";
            _agendaSemanal = new AdministradorAgendaSemanal();
            _agendaMensual = new AdministradorAgendaMensual();
            _agendaMensual.EditaElementoCita = ActivarEventoEditarElementoCita;
            _agendaSemanal.TableLayoutPanel = tableLayoutCalendario;
            _agendaMensual.TableLayoutPanel = tableLayoutCalendario;
            _agendaSemanal.EditaElementoCita = ActivarEventoEditarElementoCita;
        }

        public AgendaPropuestaCitas() : this(null)
        {
        }

        [Browsable(false)]
        public IAgenda AgendaActual { get; set; }

        [Browsable(false)]
        public IEnumerable<IElementoCita> ElementosCitas { get; private set; }

        [Browsable(false)]
        public AdministradorPropuestasCitas AdministradorPropuestasCitas
        {
            get { return _administradorPropuestasCitas; }
            set { _administradorPropuestasCitas = value; }
        }

        private void AgendaPropuestaCitas_Load(object sender, EventArgs e)
        {
            dateTimePickerFechaInicio.Value = DateTime.Today;
            
            CargaProgramacion();
        }

        /// <summary>
        /// Carga los filtros y las agendas.
        /// </summary>
        public void CargaProgramacion()
        {
            if (AdministradorPropuestasCitas != null)
            {
                AgregaFiltros();
                AgregaAgendas();
                Cargar();
            }
        }

        /// <summary>
        /// Carga las agendas configuradas
        /// </summary>
        protected virtual void AgregaAgendas()
        {
            bindingSourceAgendas.DataSource = AdministradorPropuestasCitas.Agendas;
            comboBoxAgenda.DisplayMember = "Agenda";
            comboBoxAgenda.ValueMember = "IdAgenda";
        }

        /// <summary>
        /// Carga la configuración de filtros.
        /// </summary>
        protected virtual void AgregaFiltros()
        {
            treeViewFiltros.Nodes.Clear();
            foreach (CategoriaFiltro categoriaFiltro in _administradorPropuestasCitas.CategoriasFiltros)
            {
                TreeNode node = treeViewFiltros.Nodes.Add(categoriaFiltro.CodigoCategoria.ToString(),
                                                          categoriaFiltro.Categoria);
                foreach (IFiltro filtro in _administradorPropuestasCitas.Filtros)
                {
                    if (filtro.CodigoCategoriaFiltro == categoriaFiltro.CodigoCategoria &&
                        filtro.ConfiguracionesPorAgenda.ContainsKey(AgendaActual.IdAgenda))
                    {
                        TreeNode node1 = node.Nodes.Add(filtro.Filtro, filtro.Descripcion);
                        node1.Checked = filtro.ConfiguracionesPorAgenda[AgendaActual.IdAgenda].IniciaActivo;
                        node1.Tag = filtro;
                    }
                    else
                        filtro.Activo = false;
                }
                if (node.Nodes.Count == 0) treeViewFiltros.Nodes.Remove(node);
            }
            treeViewFiltros.ExpandAll();
        }

        private void DibujaAgendaSemanal()
        {
//            tableLayoutCalendario.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            _agendaSemanal.HoraInicioJornada = _administradorPropuestasCitas.HoraInicioJornada;
            _agendaSemanal.HoraFinJornada = _administradorPropuestasCitas.HoraFinJornada;

            _agendaSemanal.DibujaDiasHorasAgenda(AgendaActual);
            labelIntervaloSemana.Text = string.Format(TituloIntervaloSemana,
                                                      dateTimePickerFechaInicio.Value.AddDays(-_agendaSemanal.DiaDeLaSemanaActual),
                                                      dateTimePickerFechaInicio.Value.AddDays(6 - _agendaSemanal.DiaDeLaSemanaActual));

            toolStripButtonDia.Checked = false;
            toolStripButtonSemana.Checked = true;
            toolStripButtonMes.Checked = false;
            _modoAgenda = TipoAgenda.Semanal;

            ActualizaFiltrosMarcados();
            if (ElementosCitas == null)
            {
                ActualizaDatos(true);
            }
            else
            {
                ActualizaDatos(false);
            }
//            tableLayoutCalendario.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
        }

        private void DibujaAgendaMensual()
        {
//            tableLayoutCalendario.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            _agendaMensual.HoraInicioJornada = _administradorPropuestasCitas.HoraInicioJornada;
            _agendaMensual.HoraFinJornada = _administradorPropuestasCitas.HoraFinJornada;
            _agendaMensual.DibujaDiasAgenda(AgendaActual);
            labelIntervaloSemana.Text = string.Format(string.Format("{0:MMMM}",_agendaMensual.Fecha));

            toolStripButtonDia.Checked = false;
            toolStripButtonSemana.Checked = false;
            toolStripButtonMes.Checked = true;
            _modoAgenda = TipoAgenda.Mensual;


            ActualizaFiltrosMarcados();
            if (ElementosCitas == null)
            {
                ActualizaDatos(true);
            }
            else
            {
                ActualizaDatos(false);
            }
//            tableLayoutCalendario.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
        }

        /// <summary>
        /// Aplica los filtros a los elementos cargados, calcula la fecha de próximo servicio
        /// y actualiza la ubicación de los elementos
        /// del calendario actual.
        /// </summary>
        public void ActualizaDatos(bool actualizaFechaProxServ)
        {
            if (actualizaFechaProxServ)
            {
                ElementosCitas = _administradorPropuestasCitas.AdministradorFiltros.ElementosCitas(_administradorPropuestasCitas.Filtros);
                CalculaFechaProximoServicio();
            }
            switch (_modoAgenda)
            {
                case TipoAgenda.Mensual:
                    _agendaMensual.ElementosCitas = ElementosCitas;
                    _agendaMensual.CargaEnAgenda(AgendaActual);
                    break;
                case TipoAgenda.Anual:
                    break;
                case TipoAgenda.Semanal:
                    _agendaSemanal.ElementosCitas = ElementosCitas;
                    _agendaSemanal.CargaEnAgenda(AgendaActual);
                    break;
            }
        }

        /// <summary>
        /// Calcula la fecha de proximo servicio a cada uno de los elementos especificados
        /// </summary>
        protected virtual void CalculaFechaProximoServicio()
        {
            if (ElementosCitas != null)
                foreach (IElementoCita elementoCita in ElementosCitas)
                {
                    DateTime fecha = dateTimePickerFechaInicio.Value;

                    if (elementoCita.FechaUltimoServicio.HasValue && elementoCita.FrecuenciaDias.HasValue)
                    {
                        DateTime fechaProximoServicio = elementoCita.FechaUltimoServicio.Value.AddDays(elementoCita.FrecuenciaDias.Value);

                        if (fechaProximoServicio.Month < fecha.Month && fechaProximoServicio.Year <= fecha.Year) //si ya se le pasó la fecha de proximo servicio lo pongo para el 1er día de este mes
                        {
                            fechaProximoServicio = new DateTime(fecha.Year, fecha.Month, 1, elementoCita.FechaUltimoServicio.Value.Hour, elementoCita.FechaUltimoServicio.Value.Minute, elementoCita.FechaUltimoServicio.Value.Second);
                        }

                        if ((elementoCita.FechaProximoServicio <= elementoCita.FechaUltimoServicio) || (fechaProximoServicio > elementoCita.FechaProximoServicio )) 
                        {
                            elementoCita.FechaProximoServicio = fechaProximoServicio < DateTime.Now
                                                                    ? DateTime.Now.AddDays(1)
                                                                    : fechaProximoServicio;
                            elementoCita.GenerarCita = true;
                        }
                        else elementoCita.GenerarCita = false;
                    }
                    else //si no tiene fecha último servicio o frecuencia lo pongo al primer día de este mes también
                    {
                        elementoCita.FechaProximoServicio = new DateTime(fecha.Year, fecha.Month, 1, _administradorPropuestasCitas.HoraInicioJornada.Hours, _administradorPropuestasCitas.HoraInicioJornada.Minutes, _administradorPropuestasCitas.HoraInicioJornada.Seconds);
                        elementoCita.GenerarCita = true;
                    }
                }
        }

        /// <summary>
        /// Actualiza los filtros que están activos en el TreeView
        /// </summary>
        private void ActualizaFiltrosMarcados()
        {
            foreach (TreeNode nodoPadre in treeViewFiltros.Nodes)
            {
                foreach (TreeNode nodoHijo in nodoPadre.Nodes)
                {
                    var filtro = (IFiltro) nodoHijo.Tag;
                    filtro.Activo = nodoHijo.Checked;
                }
            }
        }

        /// <summary>
        /// Actualiza los checks de los nodos al marcar o desmarcar un nodo
        /// </summary>
        /// <param name="nodo">
        /// Nodo al que se le hizo click
        /// </param>
        private void ActualizaChecksDeNodos(TreeNode nodo)
        {
            if (_marcando) return;
            _marcando = true;
            if (nodo.Parent == null)
            {
                foreach (TreeNode nodoHijo in nodo.Nodes)
                {
                    nodoHijo.Checked = nodo.Checked;
                }
            }
            else
            {
                bool marcar = false;
                foreach (TreeNode node in nodo.Parent.Nodes)
                {
                    marcar = marcar || node.Checked;
                }
                nodo.Parent.Checked = marcar;
            }
            _marcando = false;
        }

        protected virtual void ActivarEventoEditarElementoCita(IElementoCita elementoCita, Form formularioPadre, FlowLayoutPanel flowLayoutPanel, Control control)
        {
            EditarElementoCita(elementoCita, formularioPadre, flowLayoutPanel, control);
            tableLayoutCalendario.Visible = false;
            switch (_modoAgenda)
            {
                case TipoAgenda.Mensual:
                    if (elementoCita.ModificadoPorUsuario)
                        _agendaMensual.CargaEnAgenda(elementoCita, AgendaActual);
                    break;
                case TipoAgenda.Anual:
                    break;
                case TipoAgenda.Semanal:
                    if (elementoCita.ModificadoPorUsuario)
                        _agendaSemanal.CargaEnAgenda(elementoCita, AgendaActual);
                    break;
            }
            tableLayoutCalendario.Visible = true;
        }

        #region Propiedades Públicas del Componente

        public void Cargar()
        {
            ElementosCitas = null;
            switch (_modoAgenda)
            {
                case TipoAgenda.Mensual:
                    DibujaAgendaMensual();
                    break;
                case TipoAgenda.Anual:
                    break;
                case TipoAgenda.Semanal:
                    DibujaAgendaSemanal();
                    break;
            }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string DiasDeLaSemana
        {
            get { return _diasDeLaSemana; }
            set
            {
                if (value.Split('|').Length != 7)
                    throw new InvalidExpressionException("");
                _diasDeLaSemana = value;
                _agendaSemanal.DiasDeLaSemana = _diasDeLaSemana;
                _agendaMensual.DiasDeLaSemana = _diasDeLaSemana;
            }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public TipoAgenda ModoAgenda
        {
            get { return _modoAgenda; }
            set { _modoAgenda = value; }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string TituloIntervaloSemana { get; set; }


        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string BotonFiltros
        {
            get { return toolStripButtonFiltros.Text; }
            set { toolStripButtonFiltros.Text = value; }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string BotonActualizar
        {
            get { return toolStripButtonActualizar.Text; }
            set { toolStripButtonActualizar.Text = value; }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string TextoGeneracion
        {
            get { return panelAgendas.Text; }
            set { panelAgendas.Text = value; }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string BotonMes
        {
            get { return toolStripButtonMes.Text; }
            set { toolStripButtonMes.Text = value; }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string BotonSemana
        {
            get { return toolStripButtonSemana.Text; }
            set { toolStripButtonSemana.Text = value; }
        }

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public string TextFecha
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }


        #endregion

        [Browsable(true)]
        [Localizable(true)]
        [Category("Agenda")]
        public event ManipuladorEditarElementoCita EditarElementoCita;

        #region Eventos UI

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            Cargar();
        }

        private void dateTimePickerFechaInicio_ValueChanged(object sender, EventArgs e)
        {
            _agendaSemanal.Fecha = dateTimePickerFechaInicio.Value;
            _agendaMensual.Fecha = dateTimePickerFechaInicio.Value;
        }

        private void toolStripButtonFiltros_Click(object sender, EventArgs e)
        {
            panelFiltros.Visible = !panelFiltros.Visible;
            if (panelFiltros.Visible)
                panelAgendas.Width = flowLayoutPanel.Width - panelFiltros.Width;
            else
                panelAgendas.Width = flowLayoutPanel.Width - panelAgendas.Left;
        }

        private void comboBoxAgenda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAgenda.SelectedItem != null)
            {
                var agd = (IAgenda) comboBoxAgenda.SelectedItem;
                foreach (IAgenda agenda in AdministradorPropuestasCitas.Agendas)
                {
                    if (agenda.IdAgenda == agd.IdAgenda)
                    {
                        AgendaActual = agenda;
                        AgregaFiltros();
                        return;
                    }
                }
            }
        }

        private void treeViewFiltros_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ActualizaChecksDeNodos(e.Node);
        }

        private void toolStripButtonSemana_Click(object sender, EventArgs e)
        {
            DibujaAgendaSemanal();
        }

        private void toolStripButtonDia_Click(object sender, EventArgs e)
        {
            tableLayoutCalendario.Visible = false;
            tableLayoutCalendario.Controls.Clear();
            tableLayoutCalendario.Visible = true;

            toolStripButtonDia.Checked = true;
            toolStripButtonSemana.Checked = false;
            toolStripButtonMes.Checked = false;
        }

        private void toolStripButtonMes_Click(object sender, EventArgs e)
        {
            DibujaAgendaMensual();
        }

        #endregion

        private void treeViewFiltros_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                if (e.Node.Tag != null && AgendaActual != null)
                {
                    IFiltro filtro = (IFiltro) e.Node.Tag;
                    SolidBrush checkedBrush = new SolidBrush(filtro.ConfiguracionesPorAgenda[AgendaActual.IdAgenda].Color);
//                    HatchBrush uncheckedBrush = new HatchBrush(HatchStyle.DashedHorizontal, filtro.ConfiguracionesPorAgenda[AgendaActual.IdAgenda].Color);
                    SolidBrush brush2 =  new SolidBrush(treeViewFiltros.ForeColor );
                    Graphics g = e.Graphics;
                    int x, y;
                    x = e.Bounds.Left;
                    y = e.Bounds.Top;

                    g.FillRectangle(checkedBrush, x, y, 15, 15);
                    x = x + 15 + 2;
                    g.DrawString(e.Node.Text, treeViewFiltros.Font , brush2, new Rectangle(x, e.Bounds.Top, e.Bounds.Width + x, e.Bounds.Height));
                }
            }
            else
            {
                e.DrawDefault = true;
            }
        }
    }
}