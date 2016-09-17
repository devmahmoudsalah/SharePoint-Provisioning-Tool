using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karabina.SharePoint.Provisioning
{
    public partial class ProgressWin : Form
    {
        public ProgressWin()
        {
            InitializeComponent();
            lbResult.IntegralHeight = true;
            lbResult.DrawItem += new DrawItemEventHandler(DrawItemInColor);
            lbResult.MeasureItem += new MeasureItemEventHandler(MeasureItemToDraw);
        }

        private void MeasureItemToDraw(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;
            Graphics g = e.Graphics;
            e.ItemHeight = (int)(g.MeasureString("A", Font).Height * 1.05);
            int horzWidth = (int)g.MeasureString(lbResult.Items[e.Index].ToString(), Font).Width;
            if (lbResult.HorizontalExtent < horzWidth)
            {
                lbResult.HorizontalExtent = horzWidth + 5;
            }
        }

        private void DrawItemInColor(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                Brush foreBrush = Brushes.Black;
                Brush backBrush = SystemBrushes.Highlight;

                bool isError = false;
                bool itemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

                string itemString = lbResult.Items[e.Index].ToString();

                if (itemString.StartsWith("Error: "))
                {
                    foreBrush = Brushes.Red;
                    isError = true;
                }
                else if (itemString.StartsWith("Warning: "))
                {
                    foreBrush = Brushes.Red;
                    isError = true;
                }
                else if (itemString.StartsWith("   at "))
                {
                    foreBrush = Brushes.Red;
                    isError = true;
                }

                if ((itemSelected) && (isError))
                {
                    e.Graphics.FillRectangle(foreBrush, e.Bounds);
                    foreBrush = Brushes.White;
                }

                e.Graphics.DrawString(itemString, Font, foreBrush, e.Bounds);
            }

        }

        public ListBox ResultOutput
        {
            get { return lbResult; }
        }

        public void SetButtonFocus()
        {
            bClose.Focus();
        }

        public void SetButtonState(bool enable)
        {
            bClose.Enabled = enable;
        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }

}
