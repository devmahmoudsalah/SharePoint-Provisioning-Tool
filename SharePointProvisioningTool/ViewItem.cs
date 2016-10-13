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
    public partial class ViewItem : Form
    {
        public ViewItem()
        {
            InitializeComponent();
        }

        public void SetTitle(string title)
        {
            Text = title;
        }

        public void SetKeyValue(KeyValue keyValue)
        {
            tbKey.Text = keyValue.Key;
            tbValue.Text = keyValue.Value;

        } //SetKeyValue

        public KeyValue GetKeyValue()
        {
            return new KeyValue(tbKey.Text, tbValue.Text);
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
