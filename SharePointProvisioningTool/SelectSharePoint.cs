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
    public partial class SelectSharePoint : Form
    {
        private int _versionSelected = 0;
        public int VersionSelected
        {
            get { return _versionSelected; }
            set { _versionSelected = value; }
        }

        public SelectSharePoint()
        {
            InitializeComponent();
        }

        public void setTopic(string topic)
        {
            lTopic.Text = topic;
        }

        private void bOkay_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetVersionSelected(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            _versionSelected = Convert.ToInt32(rb.Tag);
        }
    }
}
