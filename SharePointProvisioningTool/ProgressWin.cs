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
                Brush foreBrush = SystemBrushes.WindowText;
                Brush backBrush = SystemBrushes.Window;

                bool itemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

                if (itemSelected)
                {
                    foreBrush = SystemBrushes.HighlightText;
                    backBrush = SystemBrushes.Highlight;
                }

                string itemString = lbResult.Items[e.Index].ToString();

                if (itemString.StartsWith("Info: "))
                {
                    if (itemSelected)
                    {
                        backBrush = Brushes.DarkGreen;
                        foreBrush = Brushes.White;
                    }
                    else
                    {
                        foreBrush = Brushes.DarkGreen;
                    }
                }
                else if (itemString.StartsWith("Cleanup: "))
                {
                    if (itemSelected)
                    {
                        backBrush = Brushes.RoyalBlue;
                        foreBrush = Brushes.White;
                    }
                    else
                    {
                        foreBrush = Brushes.RoyalBlue;
                    }
                }
                else if (itemString.StartsWith("Error: "))
                {
                    if (itemSelected)
                    {
                        backBrush = Brushes.OrangeRed;
                        foreBrush = Brushes.White;
                    }
                    else
                    {
                        foreBrush = Brushes.OrangeRed;
                    }
                }
                else if (itemString.StartsWith("Warning: "))
                {
                    if (itemSelected)
                    {
                        backBrush = Brushes.DarkOrange;
                        foreBrush = Brushes.White;
                    }
                    else
                    {
                        foreBrush = Brushes.DarkOrange;
                    }
                }
                else if (itemString.StartsWith("   at "))
                {
                    if (itemSelected)
                    {
                        backBrush = Brushes.OrangeRed;
                        foreBrush = Brushes.White;
                    }
                    else
                    {
                        foreBrush = Brushes.OrangeRed;
                    }
                }

                if (itemSelected)
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);
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

        private void CopyLines(object sender, EventArgs e)
        {
            if (lbResult.SelectedItems?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object item in lbResult.SelectedItems)
                {
                    sb.AppendLine(item.ToString());
                }
                Clipboard.SetText(sb.ToString());
            }
        }

        private void CopyAllLines(object sender, EventArgs e)
        {
            if (lbResult.Items?.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (object item in lbResult.Items)
                {
                    sb.AppendLine(item.ToString());
                }
                Clipboard.SetText(sb.ToString());
            }
        }

    }

}
