using System;
using System.Windows.Forms;
using System.Security;

namespace Karabina.SharePoint.Provisioning
{
    public partial class SharePointProvisioningTool : Form
    {
        private SharePoint2013OnPrem _sp2013OnPrem = null;
        private SharePoint2016OnPrem _sp2016OnPrem = null;
        private SharePoint2016Online _sp2016Online = null;

        private string lastFolder = string.Empty;

        public SharePointProvisioningTool()
        {
            InitializeComponent();
        }

        private string OpenFile()
        {
            string fileName = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(lastFolder))
            {
                openFileDialog.InitialDirectory = lastFolder;
            }
            else
            {
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            openFileDialog.Filter = "SharePoint Provisioning Template Files (*.pnp)|*.pnp|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                lastFolder = openFileDialog.FileName.Substring(0, openFileDialog.FileName.LastIndexOf('\\'));
                fileName = openFileDialog.FileName;
            }
            return fileName;
        }

        private string SaveFile()
        {
            string fileName = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (!string.IsNullOrWhiteSpace(lastFolder))
            {
                saveFileDialog.InitialDirectory = lastFolder;
            }
            else
            {
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            saveFileDialog.Filter = "SharePoint Provisioning Template Files (*.pnp)|*.pnp|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                lastFolder = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.LastIndexOf('\\'));
                fileName = saveFileDialog.FileName;
            }
            return fileName;
        }

        private void DestroyForm(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form closedForm = sender as Form;
                closedForm.Dispose();
                closedForm = null;
            }
            catch { }
        }

        private ProgressWin StartProgressWin(bool isCreating, string sharePointVersion)
        {
            ProgressWin progressWin = new ProgressWin();
            progressWin.FormClosed += new FormClosedEventHandler(DestroyForm);
            progressWin.MdiParent = this;
            progressWin.TopMost = true;
            if (isCreating)
            {
                progressWin.Text = "Creating provisioning template from " + sharePointVersion;
            }
            else
            {
                progressWin.Text = "Applying provisioning template to " + sharePointVersion;
            }
            progressWin.SetButtonState(false);
            progressWin.Show();
            Application.DoEvents();
            return progressWin;
        }

        private void FinishProgressWin(ProgressWin progressWin)
        {
            progressWin.SetButtonState(true);
            progressWin.BringToFront();
            progressWin.SetButtonFocus();
        }

        private bool CreateSP2013OPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            callee.Visible = false;
            ProgressWin progressWin = StartProgressWin(true, "SharePoint 2013 On Prem");
            if (_sp2013OnPrem == null)
            {
                _sp2013OnPrem = new SharePoint2013OnPrem();
            }
            result = _sp2013OnPrem.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);
            callee.Visible = true;
            FinishProgressWin(progressWin);
            return result;
        }

        private bool CreateSP2016OPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            callee.Visible = false;
            ProgressWin progressWin = StartProgressWin(true, "SharePoint 2016 On Prem");
            if (_sp2016OnPrem == null)
            {
                _sp2016OnPrem = new SharePoint2016OnPrem();
            }
            result = _sp2016OnPrem.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);
            callee.Visible = true;
            FinishProgressWin(progressWin);
            return result;
        }

        private bool CreateSP2016OLTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            callee.Visible = false;
            ProgressWin progressWin = StartProgressWin(true, "SharePoint 2016 Online");
            if (_sp2016Online == null)
            {
                _sp2016Online = new SharePoint2016Online();
            }
            result = _sp2016Online.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);
            callee.Visible = true;
            FinishProgressWin(progressWin);
            return result;
        }

        private bool ApplySP2013OPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            callee.Visible = false;
            ProgressWin progressWin = StartProgressWin(false, "SharePoint 2013 On Prem");
            if (_sp2013OnPrem == null)
            {
                _sp2013OnPrem = new SharePoint2013OnPrem();
            }
            result = _sp2013OnPrem.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);
            callee.Visible = true;
            FinishProgressWin(progressWin);
            return result;
        }

        private bool ApplySP2016OPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            callee.Visible = false;
            ProgressWin progressWin = StartProgressWin(false, "SharePoint 2016 On Prem");
            if (_sp2016OnPrem == null)
            {
                _sp2016OnPrem = new SharePoint2016OnPrem();
            }
            result = _sp2016OnPrem.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);
            callee.Visible = true;
            FinishProgressWin(progressWin);
            return result;
        }

        private bool ApplySP2016OLTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            bool result = false;
            callee.Visible = false;
            ProgressWin progressWin = StartProgressWin(false, "SharePoint 2016 Online");
            if (_sp2016Online == null)
            {
                _sp2016Online = new SharePoint2016Online();
            }
            result = _sp2016Online.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);
            callee.Visible = true;
            FinishProgressWin(progressWin);
            return result;
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_sp2016Online != null)
            {
                _sp2016Online = null;
            }
            if (_sp2016OnPrem != null)
            {
                _sp2016OnPrem = null;
            }
            if (_sp2013OnPrem != null)
            {
                _sp2013OnPrem = null;
            }
            Close();
        }


        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void ShowApplyForm(object sender, EventArgs e)
        {
            SelectSharePoint dialog = new SelectSharePoint();

            dialog.FormClosed += new FormClosedEventHandler(DestroyForm);

            dialog.setTopic("Select the SharePoint version to apply the template to");

            DialogResult result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                int version = dialog.VersionSelected;

                TargetWin applyForm = new TargetWin();

                applyForm.FormClosed += new FormClosedEventHandler(DestroyForm);

                switch (version)
                {
                    case Constants.SP2013ONPREM:
                        applyForm.Text = "Apply Template To SharePoint 2013 On Prem";
                        applyForm.ApplyTemplate = ApplySP2013OPTemplate;
                        break;
                    case Constants.SP2016ONPREM:
                        applyForm.Text = "Apply Template To SharePoint 2016 On Prem";
                        applyForm.ApplyTemplate = ApplySP2016OPTemplate;
                        break;
                    case Constants.SP2016ONLINE:
                        applyForm.Text = "Apply Template To SharePoint 2016 Online";
                        applyForm.ApplyTemplate = ApplySP2016OLTemplate;
                        break;
                    default:
                        break;
                }

                applyForm.SelectedVersion = version;

                applyForm.OpenTemplate = OpenFile;

                applyForm.MdiParent = this;

                applyForm.Show();
            }
        }

        private void ShowCreateForm(object sender, EventArgs e)
        {
            SelectSharePoint dialog = new SelectSharePoint();

            dialog.FormClosed += new FormClosedEventHandler(DestroyForm);

            dialog.setTopic("Select the SharePoint version to create the template from");

            DialogResult result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                int version = dialog.VersionSelected;

                SourceWin createForm = new SourceWin();

                createForm.FormClosed += new FormClosedEventHandler(DestroyForm);

                switch (version)
                {
                    case Constants.SP2013ONPREM:
                        createForm.Text = "Create Template From SharePoint 2013 On Prem";
                        createForm.CreateTemplate = CreateSP2013OPTemplate;
                        break;
                    case Constants.SP2016ONPREM:
                        createForm.Text = "Create Template From SharePoint 2016 On Prem";
                        createForm.CreateTemplate = CreateSP2016OPTemplate;
                        break;
                    case Constants.SP2016ONLINE:
                        createForm.Text = "Create Template From SharePoint 2016 Online";
                        createForm.CreateTemplate = CreateSP2016OLTemplate;
                        break;
                    default:
                        break;
                }

                createForm.SelectedVersion = version;

                createForm.SaveTemplate = SaveFile;

                createForm.MdiParent = this;

                createForm.Show();
            }

        }

    }

}
