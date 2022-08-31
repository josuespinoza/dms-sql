namespace SCG.UX.Windows.CitasAutomaticas
{
    partial class CitasEnIntervaloMensual
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
            this.flowLayoutPanelCitas = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDia = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flowLayoutPanelCitas
            // 
            this.flowLayoutPanelCitas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelCitas.AutoScroll = true;
            this.flowLayoutPanelCitas.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelCitas.Location = new System.Drawing.Point(2, 21);
            this.flowLayoutPanelCitas.Name = "flowLayoutPanelCitas";
            this.flowLayoutPanelCitas.Size = new System.Drawing.Size(129, 170);
            this.flowLayoutPanelCitas.TabIndex = 1;
            this.flowLayoutPanelCitas.WrapContents = false;
            // 
            // labelDia
            // 
            this.labelDia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDia.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelDia.Location = new System.Drawing.Point(3, 2);
            this.labelDia.Name = "labelDia";
            this.labelDia.Size = new System.Drawing.Size(128, 15);
            this.labelDia.TabIndex = 2;
            this.labelDia.Text = "label1";
            // 
            // CitasEnIntervaloMensual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelDia);
            this.Controls.Add(this.flowLayoutPanelCitas);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(136, 194);
            this.Name = "CitasEnIntervaloMensual";
            this.Size = new System.Drawing.Size(134, 192);
            this.Load += new System.EventHandler(this.CitasEnIntervaloMensual_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCitas;
        private System.Windows.Forms.Label labelDia;
    }
}
