using System;
using System.Drawing;
using System.Windows.Forms;
using System.Security;

namespace Karabina.SharePoint.Provisioning
{
    public partial class SharePointProvisioningTool : Form
    {
        private Color KarabinaRed = Color.FromArgb(Constants.Karabina_Red, Constants.Karabina_Green, Constants.Karabina_Blue);

        private SPLoader _sp2013OnPrem = null;
        private SPLoader _sp2016OnPrem = null;
        private SPLoader _sp2016Online = null;

        private string lastFolder = string.Empty;

        public SharePointProvisioningTool()
        {
            InitializeComponent();

            SetStatusBarText(Properties.Resources.ResourceManager.GetString(Constants.String00));

        } //SharePointProvisioningTool

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

        } //OpenFile

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

        } //SaveFile

        private void DestroyForm(object sender, FormClosedEventArgs e)
        {
            try
            {
                Form closedForm = sender as Form;
                closedForm.Dispose();
                closedForm = null;

            }
            catch { }

        } //DestroyForm

        private string EnsureVersionLoaded(SharePointVersion version)
        {
            string spVersionTitle = string.Empty;
            switch (version)
            {
                case SharePointVersion.SharePoint_2013_On_Premises:
                    spVersionTitle = Constants.SharePoint_2013_On_Premises;
                    if (_sp2013OnPrem == null)
                    {
                        _sp2013OnPrem = Program.LoadSPLoader(SharePointVersion.SharePoint_2013_On_Premises);

                    }

                    break;

                case SharePointVersion.SharePoint_2016_On_Premises:
                    spVersionTitle = Constants.SharePoint_2016_On_Premises;
                    if (_sp2016OnPrem == null)
                    {
                        _sp2016OnPrem = Program.LoadSPLoader(SharePointVersion.SharePoint_2016_On_Premises);

                    }

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    spVersionTitle = Constants.SharePoint_2016_Online;
                    if (_sp2016Online == null)
                    {
                        _sp2016Online = Program.LoadSPLoader(SharePointVersion.SharePoint_2016_OnLine);

                    }

                    break;

                default:
                    break;

            }

            return spVersionTitle;

        } //EnsureVersionLoaded

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

        } //StartProgressWin

        private void FinishProgressWin(ProgressWin progressWin)
        {
            progressWin.SetButtonState(true);
            if (progressWin.ShouldShowHorizontalScrollBar)
            {
                progressWin.ResultOutput.HorizontalScrollbar = true;

            }

            progressWin.BringToFront();
            progressWin.SetButtonFocus();

        } //FinishProgressWin

        private bool CreateSPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            SourceWin callForm = callee as SourceWin;
            bool result = false;
            callForm.Visible = false;
            string spVersionTitle = EnsureVersionLoaded(callForm.SelectedVersion);            

            ProgressWin progressWin = StartProgressWin(true, spVersionTitle);

            switch (callForm.SelectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premises:
                    result = _sp2013OnPrem.CreateProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_On_Premises:
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

        } //CreateSPTemplate

        private bool ApplySPTemplate(Form callee, ProvisioningOptions provisioningOptions)
        {
            TargetWin callForm = callee as TargetWin;
            bool result = false;
            callForm.Visible = false;

            string spVersionTitle = EnsureVersionLoaded(callForm.SelectedVersion);            

            ProgressWin progressWin = StartProgressWin(false, spVersionTitle);

            switch (callForm.SelectedVersion)
            {
                case SharePointVersion.SharePoint_2013_On_Premises:
                    result = _sp2013OnPrem.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_On_Premises:
                    result = _sp2016OnPrem.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

                case SharePointVersion.SharePoint_2016_OnLine:
                    result = _sp2016Online.ApplyProvisioningTemplate(progressWin.ResultOutput, provisioningOptions);

                    break;

            }

            callForm.Visible = true;
            FinishProgressWin(progressWin);
            return result;

        } //ApplySPTemplate

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();

        } //ExitToolsStripMenuItem_Click


        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);

        } //CascadeToolStripMenuItem_Click

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);

        } //TileVerticalToolStripMenuItem_Click

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);

        } //TileHorizontalToolStripMenuItem_Click

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);

        } //ArrangeIconsToolStripMenuItem_Click

        private SharePointVersion GetVersion(string topic)
        {
            SharePointVersion version = SharePointVersion.SharePoint_Invalid;

            SelectSharePoint dialog = new SelectSharePoint();

            dialog.FormClosed += new FormClosedEventHandler(DestroyForm);

            dialog.setTopic("Select the SharePoint version " + topic);

            dialog.SetStatusBarText = SetStatusBarText;

            DialogResult result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                version = dialog.VersionSelected;

            }

            return version;

        } //GetVersion


        private void ShowApplyForm(object sender, EventArgs e)
        {
            SharePointVersion version = GetVersion("to apply the template to");

            if (version != SharePointVersion.SharePoint_Invalid)
            {
                TargetWin applyForm = new TargetWin();

                applyForm.FormClosed += new FormClosedEventHandler(DestroyForm);
                applyForm.Text = "Apply Template To ";

                switch (version)
                {
                    case SharePointVersion.SharePoint_2013_On_Premises:
                        applyForm.Text += Constants.SharePoint_2013_On_Premises;

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premises:
                        applyForm.Text += Constants.SharePoint_2016_On_Premises;

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

        } //ShowApplyForm

        private void ShowCreateForm(object sender, EventArgs e)
        {
            SharePointVersion version = GetVersion("to create the template from");

            if (version != SharePointVersion.SharePoint_Invalid)
            {
                SourceWin createForm = new SourceWin();

                createForm.FormClosed += new FormClosedEventHandler(DestroyForm);
                createForm.Text = "Create Template From ";

                switch (version)
                {
                    case SharePointVersion.SharePoint_2013_On_Premises:
                        createForm.Text += Constants.SharePoint_2013_On_Premises;

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premises:
                        createForm.Text += Constants.SharePoint_2016_On_Premises;

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

        } //ShowCreateForm

        private void ShowEditForm(object sender, EventArgs e)
        {
            SharePointVersion version = GetVersion("of the template to edit");

            if (version != SharePointVersion.SharePoint_Invalid)
            {
                EditWin editForm = new EditWin();

                editForm.FormClosed += new FormClosedEventHandler(DestroyForm);
                editForm.Text = "Edit Template - ";
                editForm.SelectedVersion = version;

                switch (version)
                {
                    case SharePointVersion.SharePoint_2013_On_Premises:
                        editForm.Text += Constants.SharePoint_2013_On_Premises;
                        if (_sp2013OnPrem == null)
                        {
                            _sp2013OnPrem = Program.LoadSPLoader(version);

                        }

                        editForm.SharePointLoader = _sp2013OnPrem;

                        break;

                    case SharePointVersion.SharePoint_2016_On_Premises:
                        editForm.Text += Constants.SharePoint_2016_On_Premises;
                        if (_sp2016OnPrem == null)
                        {
                            _sp2016OnPrem = Program.LoadSPLoader(version);

                        }

                        editForm.SharePointLoader = _sp2016OnPrem;

                        break;

                    case SharePointVersion.SharePoint_2016_OnLine:
                        editForm.Text += Constants.SharePoint_2016_Online;
                        if (_sp2016Online == null)
                        {
                            _sp2016Online = Program.LoadSPLoader(version);

                        }

                        editForm.SharePointLoader = _sp2016Online;

                        break;

                    default:
                        break;
                }

                editForm.OpenTemplate = OpenFile;

                editForm.SetStatusBarText = SetStatusBarText;

                editForm.MdiParent = this;

                editForm.Show();

            }

        } //ShowEditForm

        private void SetStatusBarText(string message)
        {
            toolStripStatusLabel.Text = message;

        } //SetStatusBarText

        private void SetStatusText(object sender, EventArgs e)
        {
            string tag = (sender as ToolStripItem).Tag.ToString();
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(tag));

        } //SetStatusText

        private void SetStatusDefault(object sender, EventArgs e)
        {
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(Constants.String00));

        } //SetStatusDefault

    } //SharePointProvisioningTool

}
