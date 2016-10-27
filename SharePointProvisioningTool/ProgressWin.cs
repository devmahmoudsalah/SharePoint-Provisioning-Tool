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
        public delegate void SetStatusTextDelegate(string message);

        public SetStatusTextDelegate SetStatusBarText;

        public bool ShouldShowHorizontalScrollBar { get; set; }

        public ProgressWin()
        {
            InitializeComponent();
            ShouldShowHorizontalScrollBar = false;
            lbResult.IntegralHeight = true;
            lbResult.DrawItem += new DrawItemEventHandler(DrawItemInColor);
            lbResult.MeasureItem += new MeasureItemEventHandler(MeasureItemToDraw);

        }

        private void MeasureItemToDraw(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.ItemHeight = (int)(TextRenderer.MeasureText(e.Graphics, "A", Font).Height * 1.05);
            int horzWidth = TextRenderer.MeasureText(e.Graphics, lbResult.Items[e.Index].ToString(), Font).Width;
            if (lbResult.HorizontalExtent < horzWidth)
            {
                lbResult.HorizontalExtent = horzWidth + 5;
                if (lbResult.Width < horzWidth)
                {
                    ShouldShowHorizontalScrollBar = true;

                }

            }

        }

        private void DrawItemInColor(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                Brush backBrush = SystemBrushes.Window;
                Color foreColour = SystemColors.WindowText;
                Color backColour = SystemColors.Window;

                bool itemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

                if (itemSelected)
                {
                    foreColour = SystemColors.HighlightText;
                    backColour = SystemColors.Highlight;
                    backBrush = SystemBrushes.Highlight;
                }

                string itemString = lbResult.Items[e.Index].ToString();

                if (itemString.StartsWith("Info: "))
                {
                    if (itemSelected)
                    {
                        foreColour = Color.White;
                        backColour = Color.DarkGreen;
                        backBrush = Brushes.DarkGreen;
                    }
                    else
                    {
                        foreColour = Color.DarkGreen;
                    }
                }
                else if (itemString.StartsWith("Cleanup: "))
                {
                    if (itemSelected)
                    {
                        foreColour = Color.White;
                        backColour = Color.RoyalBlue;
                        backBrush = Brushes.RoyalBlue;
                    }
                    else
                    {
                        foreColour = Color.RoyalBlue;
                    }
                }
                else if (itemString.StartsWith("Error: "))
                {
                    if (itemSelected)
                    {
                        foreColour = Color.White;
                        backColour = Color.OrangeRed;
                        backBrush = Brushes.OrangeRed;
                    }
                    else
                    {
                        foreColour = Color.OrangeRed;
                    }
                }
                else if (itemString.StartsWith("Warning: "))
                {
                    if (itemSelected)
                    {
                        foreColour = Color.White;
                        backColour = Color.DarkOrange;
                        backBrush = Brushes.DarkOrange;
                    }
                    else
                    {
                        foreColour = Color.DarkOrange;
                    }
                }
                else if (itemString.StartsWith("   at "))
                {
                    if (itemSelected)
                    {
                        foreColour = Color.White;
                        backColour = Color.OrangeRed;
                        backBrush = Brushes.OrangeRed;
                    }
                    else
                    {
                        foreColour = Color.OrangeRed;
                    }
                }
                
                if (itemSelected)
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);
                }
                
                TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(e.Graphics, itemString, Font, e.Bounds, foreColour, backColour, flags);

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

        private void SetStatusText(object sender, EventArgs e)
        {
            Control control = (sender as Control);
            string tag = Constants.Progress0;
            if (control.Tag != null)
            {
                tag = control.Tag.ToString();
            }

            SetStatusBarText(Properties.Resources.ResourceManager.GetString(tag));

        }

        private void SetStatusDefault(object sender, EventArgs e)
        {
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(Constants.Progress0));

        }

    }

}
