using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SCG.UX.Windows.CitasAutomaticas
{
    public partial class VerticalLabel : Label
    {
        public VerticalLabel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            float vlblControlWidth;
            float vlblControlHeight;
            float vlblTransformX;
            float vlblTransformY;
            Color controlBackColor = BackColor;
            Pen labelBorderPen = new Pen(controlBackColor, 0);
            SolidBrush labelBackColorBrush = new SolidBrush(controlBackColor);
            SolidBrush labelForeColorBrush = new SolidBrush(base.ForeColor);
            base.OnPaint(e);
            vlblControlWidth = this.Size.Width;
            vlblControlHeight = this.Size.Height;
            e.Graphics.DrawRectangle(labelBorderPen, 0, 0, vlblControlWidth, vlblControlHeight);
            e.Graphics.FillRectangle(labelBackColorBrush, 0, 0, vlblControlWidth, vlblControlHeight);

            if (this.TextDrawMode == 0)
            {
                vlblTransformX = 0;
                vlblTransformY = vlblControlHeight;
                e.Graphics.TranslateTransform(vlblTransformX, vlblTransformY);
                e.Graphics.RotateTransform(270);
                e.Graphics.DrawString(Text, Font, labelForeColorBrush, 0, 0);
            }
            else
            {
                vlblTransformX = vlblControlWidth;
                vlblTransformY = vlblControlHeight;
                e.Graphics.TranslateTransform(vlblControlWidth, 0);
                e.Graphics.RotateTransform(90);
                e.Graphics.DrawString(Text, Font, labelForeColorBrush, 0, 0, StringFormat.GenericTypographic);
            }
        }

        protected int TextDrawMode { get; set; }
    }
}
