using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security;

namespace Karabina.SharePoint.Provisioning
{
    public partial class SharePointProvisioningTool : Form
    {
        private Color KarabinaRed = Color.FromArgb(Constants.Karabina_Red, Constants.Karabina_Green, Constants.Karabina_Blue);

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

            openFileDialog.Filter = Constants.File_Dialog_Filter;

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

            saveFileDialog.Filter = Constants.File_Dialog_Filter;

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

            progressWin.SetStatusBarText = SetStatusBarText;
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

        private bool CreateSPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            SourceWin callForm = callee as SourceWin;
            bool result = false;
            callForm.Visible = false;
            string spVerionTitle = string.Empty;
            switch (callForm.SelectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    spVerionTitle = Constants.SharePoint_2013_On_Premise;
                    if (_sp2013OnPrem == null)
                    {
                        _sp2013OnPrem = new SharePoint2013OnPrem();

                    }

                    break;

                case SharePointVersion.SharePoint_2016_On_Premise:
                    spVerionTitle = Constants.SharePoint_2016_On_Premise;
                    if (_sp2016OnPrem == null)
                    {
                        _sp2016OnPrem = new SharePoint2016OnPrem();

                    }

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    spVerionTitle = Constants.SharePoint_2016_Online;
                    if (_sp2016Online == null)
                    {
                        _sp2016Online = new SharePoint2016Online();

                    }

                    break;

                default:
                    break;

            }

            ProgressWin progressWin = StartProgressWin(true, spVerionTitle);

            switch (callForm.SelectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    result = _sp2013OnPrem.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_On_Premise:
                    result = _sp2016OnPrem.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    result = _sp2016Online.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                default:
                    break;

            }

            callForm.Visible = true;

            FinishProgressWin(progressWin);
            return result;

        }

        private bool ApplySPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            TargetWin callForm = callee as TargetWin;
            bool result = false;
            callForm.Visible = false;

            string spVerionTitle = string.Empty;
            switch (callForm.SelectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    spVerionTitle = Constants.SharePoint_2013_On_Premise;
                    if (_sp2013OnPrem == null)
                    {
                        _sp2013OnPrem = new SharePoint2013OnPrem();

                    }

                    break;

                case SharePointVersion.SharePoint_2016_On_Premise:
                    spVerionTitle = Constants.SharePoint_2016_On_Premise;
                    if (_sp2016OnPrem == null)
                    {
                        _sp2016OnPrem = new SharePoint2016OnPrem();

                    }

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    spVerionTitle = Constants.SharePoint_2016_Online;
                    if (_sp2016Online == null)
                    {
                        _sp2016Online = new SharePoint2016Online();

                    }

                    break;

            }

            ProgressWin progressWin = StartProgressWin(false, spVerionTitle);

            switch (callForm.SelectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premise:
                    result = _sp2013OnPrem.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_On_Premise:
                    result = _sp2016OnPrem.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    result = _sp2016Online.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

            }

            callForm.Visible = true;
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
                SharePointVersion version = dialog.VersionSelected;

                TargetWin applyForm = new TargetWin();

                applyForm.FormClosed += new FormClosedEventHandler(DestroyForm);
                applyForm.Text = "Apply Template To ";

                switch (version)
                {
                    case SharePointVersion.SharePoint_2013_On_Premise:
                        applyForm.Text += Constants.SharePoint_2013_On_Premise;

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premise:
                        applyForm.Text += Constants.SharePoint_2016_On_Premise;

                        break;

                    case SharePointVersion.SharePoint_2016_OnLine:
                        applyForm.Text += Constants.SharePoint_2016_Online;

                        break;

                    default:
                        break;

                }

                applyForm.ApplyTemplate = ApplySPTemplate;

                applyForm.SelectedVersion = version;

                applyForm.OpenTemplate = OpenFile;

                applyForm.SetStatusBarText = SetStatusBarText;

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
                SharePointVersion version = dialog.VersionSelected;

                SourceWin createForm = new SourceWin();

                createForm.FormClosed += new FormClosedEventHandler(DestroyForm);
                createForm.Text = "Create Template From ";

                switch (version)
                {
                    case SharePointVersion.SharePoint_2013_On_Premise:
                        createForm.Text += Constants.SharePoint_2013_On_Premise;

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premise:
                        createForm.Text += Constants.SharePoint_2016_On_Premise;

                        break;

                    case SharePointVersion.SharePoint_2016_OnLine:
                        createForm.Text += Constants.SharePoint_2016_Online;

                        break;

                    default:
                        break;

                }

                createForm.CreateTemplate = CreateSPTemplate;

                createForm.SelectedVersion = version;

                createForm.SaveTemplate = SaveFile;

                createForm.SetStatusBarText = SetStatusBarText;

                createForm.MdiParent = this;

                createForm.Show();

            }

        }

        private void ShowEditForm(object sender, EventArgs e)
        {
            SelectSharePoint dialog = new SelectSharePoint();

            dialog.FormClosed += new FormClosedEventHandler(DestroyForm);

            dialog.setTopic("Select the SharePoint version of the template to edit");

            DialogResult result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                SharePointVersion version = dialog.VersionSelected;

                EditWin editForm = new EditWin();

                editForm.FormClosed += new FormClosedEventHandler(DestroyForm);
                editForm.Text = "Edit Template - ";
                editForm.SelectedVersion = version;

                switch (version)
                {
                    case SharePointVersion.SharePoint_2013_On_Premise:
                        editForm.Text += Constants.SharePoint_2013_On_Premise;
                        if (_sp2013OnPrem == null)
                        {
                            _sp2013OnPrem = new SharePoint2013OnPrem();
                        }

                        editForm.SP2013OP = _sp2013OnPrem;

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premise:
                        editForm.Text += Constants.SharePoint_2016_On_Premise;
                        if (_sp2016OnPrem == null)
                        {
                            _sp2016OnPrem = new SharePoint2016OnPrem();

                        }

                        editForm.SP2016OP = _sp2016OnPrem;

                        break;

                    case SharePointVersion.SharePoint_2016_OnLine:
                        editForm.Text += Constants.SharePoint_2016_Online;
                        if (_sp2016Online == null)
                        {
                            _sp2016Online = new SharePoint2016Online();

                        }

                        editForm.SP2016OL = _sp2016Online;

                        break;

                    default:
                        break;
                }

                editForm.OpenTemplate = OpenFile;

                editForm.SetStatusBarText = SetStatusBarText;

                editForm.MdiParent = this;

                editForm.Show();

            }

        }

        private void SetStatusBarText(string message)
        {
            toolStripStatusLabel.Text = message;

        }

        private void SetStatusText(object sender, EventArgs e)
        {
            string tag = Constants.String0;
            tag = (sender as ToolStripItem).Tag.ToString();
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(tag));

        }

        private void SetStatusDefault(object sender, EventArgs e)
        {
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(Constants.String0));

        }

    }

}
