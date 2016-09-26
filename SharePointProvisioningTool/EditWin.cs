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
    public partial class EditWin : Form
    {
        private SharePointVersion _selectedVersion = SharePointVersion.SharePoint_Invalid;

        public SharePointVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set { _selectedVersion = value; }
        }

        public delegate void editTemplateDelegate(TreeView treeView, string templatePath, string templateName, SharePointVersion version);

        public editTemplateDelegate EditTemplate;

        public delegate string OpenTemplateDelegate();

        public OpenTemplateDelegate OpenTemplate;

        public delegate void SetStatusTextDelegate(string message);

        public SetStatusTextDelegate SetStatusBarText;

        public EditWin()
        {
            InitializeComponent();
        }

        private void bBrowse_Click(object sender, EventArgs e)
        {
            string fileName = OpenTemplate();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                tbTemplate.Text = fileName;

                try
                {
                    Cursor = Cursors.WaitCursor;

                    int slashPosition = tbTemplate.Text.LastIndexOf('\\');
                    string path = tbTemplate.Text.Substring(0, slashPosition);
                    slashPosition++;
                    string name = tbTemplate.Text.Substring(slashPosition).Replace(".pnp", "");

                    EditTemplate(tvTemplate, path, name, _selectedVersion);

                }
                catch
                {

                }
                finally
                {
                    Cursor = Cursors.Default;
                }

            }

        }

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

}
