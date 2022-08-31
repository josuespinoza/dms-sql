namespace SCG.UX.Windows.CitasAutomaticas
{
    partial class AgendaPropuestaCitas
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgendaPropuestaCitas));
            this.treeViewFiltros = new System.Windows.Forms.TreeView();
            this.comboBoxAgenda = new System.Windows.Forms.ComboBox();
            this.bindingSourceAgendas = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutCalendario = new System.Windows.Forms.TableLayoutPanel();
            this.dateTimePickerFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFiltros = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonActualizar = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panelFiltros = new System.Windows.Forms.GroupBox();
            this.panelAgendas = new System.Windows.Forms.GroupBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonDia = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSemana = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMes = new System.Windows.Forms.ToolStripButton();
            this.labelIntervaloSemana = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAgendas)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.panelFiltros.SuspendLayout();
            this.panelAgendas.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewFiltros
            // 
            this.treeViewFiltros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewFiltros.CheckBoxes = true;
            this.treeViewFiltros.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeViewFiltros.FullRowSelect = true;
            resources.ApplyResources(this.treeViewFiltros, "treeViewFiltros");
            this.treeViewFiltros.Name = "treeViewFiltros";
            this.treeViewFiltros.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewFiltros_AfterCheck);
            this.treeViewFiltros.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeViewFiltros_DrawNode);
            // 
            // comboBoxAgenda
            // 
            this.comboBoxAgenda.DataSource = this.bindingSourceAgendas;
            this.comboBoxAgenda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAgenda.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxAgenda, "comboBoxAgenda");
            this.comboBoxAgenda.Name = "comboBoxAgenda";
            this.comboBoxAgenda.SelectedIndexChanged += new System.EventHandler(this.comboBoxAgenda_SelectedIndexChanged);
            // 
            // tableLayoutCalendario
            // 
            resources.ApplyResources(this.tableLayoutCalendario, "tableLayoutCalendario");
            this.tableLayoutCalendario.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutCalendario.Name = "tableLayoutCalendario";
            // 
            // dateTimePickerFechaInicio
            // 
            this.dateTimePickerFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dateTimePickerFechaInicio, "dateTimePickerFechaInicio");
            this.dateTimePickerFechaInicio.Name = "dateTimePickerFechaInicio";
            this.dateTimePickerFechaInicio.Value = new System.DateTime(2009, 10, 5, 16, 10, 23, 0);
            this.dateTimePickerFechaInicio.ValueChanged += new System.EventHandler(this.dateTimePickerFechaInicio_ValueChanged);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFiltros,
            this.toolStripSeparator1,
            this.toolStripButtonActualizar});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // toolStripButtonFiltros
            // 
            this.toolStripButtonFiltros.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButtonFiltros, "toolStripButtonFiltros");
            this.toolStripButtonFiltros.Name = "toolStripButtonFiltros";
            this.toolStripButtonFiltros.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            this.toolStripButtonFiltros.Click += new System.EventHandler(this.toolStripButtonFiltros_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripButtonActualizar
            // 
            this.toolStripButtonActualizar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButtonActualizar, "toolStripButtonActualizar");
            this.toolStripButtonActualizar.Name = "toolStripButtonActualizar";
            this.toolStripButtonActualizar.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90;
            this.toolStripButtonActualizar.Click += new System.EventHandler(this.buttonActualizar_Click);
            // 
            // flowLayoutPanel
            // 
            resources.ApplyResources(this.flowLayoutPanel, "flowLayoutPanel");
            this.flowLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(206)))));
            this.flowLayoutPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel.Controls.Add(this.panelFiltros);
            this.flowLayoutPanel.Controls.Add(this.panelAgendas);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            // 
            // panelFiltros
            // 
            this.panelFiltros.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(206)))));
            this.panelFiltros.Controls.Add(this.comboBoxAgenda);
            this.panelFiltros.Controls.Add(this.treeViewFiltros);
            resources.ApplyResources(this.panelFiltros, "panelFiltros");
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.TabStop = false;
            // 
            // panelAgendas
            // 
            this.panelAgendas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(206)))));
            this.panelAgendas.Controls.Add(this.toolStrip2);
            this.panelAgendas.Controls.Add(this.labelIntervaloSemana);
            this.panelAgendas.Controls.Add(this.label1);
            this.panelAgendas.Controls.Add(this.dateTimePickerFechaInicio);
            this.panelAgendas.Controls.Add(this.tableLayoutCalendario);
            resources.ApplyResources(this.panelAgendas, "panelAgendas");
            this.panelAgendas.Name = "panelAgendas";
            this.panelAgendas.TabStop = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(206)))));
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonDia,
            this.toolStripButtonSemana,
            this.toolStripButtonMes});
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // toolStripButtonDia
            // 
            this.toolStripButtonDia.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButtonDia, "toolStripButtonDia");
            this.toolStripButtonDia.Name = "toolStripButtonDia";
            this.toolStripButtonDia.Click += new System.EventHandler(this.toolStripButtonDia_Click);
            // 
            // toolStripButtonSemana
            // 
            this.toolStripButtonSemana.Checked = true;
            this.toolStripButtonSemana.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonSemana.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButtonSemana, "toolStripButtonSemana");
            this.toolStripButtonSemana.Name = "toolStripButtonSemana";
            this.toolStripButtonSemana.Click += new System.EventHandler(this.toolStripButtonSemana_Click);
            // 
            // toolStripButtonMes
            // 
            this.toolStripButtonMes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripButtonMes, "toolStripButtonMes");
            this.toolStripButtonMes.Name = "toolStripButtonMes";
            this.toolStripButtonMes.Click += new System.EventHandler(this.toolStripButtonMes_Click);
            // 
            // labelIntervaloSemana
            // 
            resources.ApplyResources(this.labelIntervaloSemana, "labelIntervaloSemana");
            this.labelIntervaloSemana.Name = "labelIntervaloSemana";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // AgendaPropuestaCitas
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(223)))), ((int)(((byte)(206)))));
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Name = "AgendaPropuestaCitas";
            this.Load += new System.EventHandler(this.AgendaPropuestaCitas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAgendas)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.panelFiltros.ResumeLayout(false);
            this.panelAgendas.ResumeLayout(false);
            this.panelAgendas.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewFiltros;
        private System.Windows.Forms.ComboBox comboBoxAgenda;
        private System.Windows.Forms.BindingSource bindingSourceAgendas;
        private System.Windows.Forms.TableLayoutPanel tableLayoutCalendario;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonFiltros;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.DateTimePicker dateTimePickerFechaInicio;
        private System.Windows.Forms.GroupBox panelFiltros;
        private System.Windows.Forms.GroupBox panelAgendas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButtonDia;
        private System.Windows.Forms.ToolStripButton toolStripButtonSemana;
        private System.Windows.Forms.ToolStripButton toolStripButtonMes;
        private System.Windows.Forms.Label labelIntervaloSemana;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonActualizar;
    }
}
