using System;
using System.Windows.Forms;
using System.Security;
using System.Runtime.InteropServices;

namespace Karabina.SharePoint.Provisioning
{
    public partial class SourceWin : Form
    {
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        private SharePointVersion _selectedVersion = SharePointVersion.SharePoint_Invalid;

        public SharePointVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set { _selectedVersion = value; }

        } //SelectedVersion

        private ProvisioningOptions _options = null;

        public delegate string SaveTemplateDelegate();

        public SaveTemplateDelegate SaveTemplate;

        public delegate bool CreateTemplateDelegate(Form callee, ProvisioningOptions provisioningOptions);

        public CreateTemplateDelegate CreateTemplate;

        public delegate void SetStatusTextDelegate(string message);

        public SetStatusTextDelegate SetStatusBarText;

        public SourceWin()
        {
            InitializeComponent();

        } //SourceWin


        private void bBrowse_Click(object sender, EventArgs e)
        {
            string fileName = SaveTemplate();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                tbTemplate.Text = fileName;

            }

        } //bBrowse_Click

        private void cbNoUNP_CheckedChanged(object sender, EventArgs e)
        {
            if (cbNoUNP.Checked)
            {
                lUserName.Enabled = false;
                tbUserName.Enabled = false;
                lPassword.Enabled = false;
                tbPassword.Enabled = false;

            }
            else
            {
                lUserName.Enabled = true;
                tbUserName.Enabled = true;
                lPassword.Enabled = true;
                tbPassword.Enabled = true;

            }

        } //cbNoUNP_CheckedChanged

        private void bCreate_Click(object sender, EventArgs e)
        {
            lSharePointUrlError.Visible = false;
            lUserNameError.Visible = false;
            lPasswordError.Visible = false;
            lTemplateError.Visible = false;
            SecureString pwdSecure = new SecureString();

            if (string.IsNullOrWhiteSpace(tbSharePointUrl.Text))
            {
                lSharePointUrlError.Visible = true;
                tbSharePointUrl.Focus();
                return;

            }

            if (!cbNoUNP.Checked)
            {
                if (string.IsNullOrWhiteSpace(tbUserName.Text))
                {
                    lUserNameError.Visible = true;
                    tbUserName.Focus();
                    return;

                }

                if (string.IsNullOrWhiteSpace(tbPassword.Text))
                {
                    lPasswordError.Visible = true;
                    tbPassword.Focus();
                    return;

                }

                foreach (char c in tbPassword.Text.ToCharArray()) pwdSecure.AppendChar(c);

            }

            if (string.IsNullOrWhiteSpace(tbTemplate.Text))
            {
                lTemplateError.Visible = true;
                bBrowse.Focus();
                return;

            }

            int slashPosition = tbTemplate.Text.LastIndexOf('\\');
            string path = tbTemplate.Text.Substring(0, slashPosition);
            slashPosition++;
            string name = tbTemplate.Text.Substring(slashPosition);

            if (_options == null)
            {
                _options = new ProvisioningOptions();

            }

            _options.TemplatePath = path;
            _options.TemplateName = name.Replace(".pnp", "");
            _options.WebAddress = tbSharePointUrl.Text;
            _options.UserNameOrEmail = tbUserName.Text;
            _options.UserPassword = pwdSecure;
            _options.AuthenticationRequired = !cbNoUNP.Checked;

            Enabled = false;
            CreateTemplate(this, _options);
            Enabled = true;

        } //bCreate_Click

        private void bClose_Click(object sender, EventArgs e)
        {
            Close();

        } //bClose_Click

        private void FormShown(object sender, EventArgs e)
        {
            if (_selectedVersion == SharePointVersion.SharePoint_2016_OnLine)
            {
                SendMessage(tbSharePointUrl.Handle, EM_SETCUEBANNER, 0, "https://company.sharepoint.com/sites/site");
                cbNoUNP.Enabled = false;
                lUserName.Text = "Email";
                SendMessage(tbUserName.Handle, EM_SETCUEBANNER, 0, "john.doe@company.com");
                lUserNameError.Text = lUserNameError.Text.Replace("#", "email");

            }
            else
            {
                SendMessage(tbSharePointUrl.Handle, EM_SETCUEBANNER, 0, "https://sharepoint.company.com/sites/site");
                lUserName.Text = "User Name";
                SendMessage(tbUserName.Handle, EM_SETCUEBANNER, 0, "Domain\\UserName");
                lUserNameError.Text = lUserNameError.Text.Replace("#", "user name");

            }

            SendMessage(tbPassword.Handle, EM_SETCUEBANNER, 0, "●●●●●●●●"); //password

        } //FormShown

        private void bOptions_Click(object sender, EventArgs e)
        {
            if (_options == null)
            {
                _options = new ProvisioningOptions();

            }

            TemplateOptions templateOptions = new TemplateOptions("Template Create Options", true);
            templateOptions.ProvisioningOptions = _options;
            DialogResult result = templateOptions.ShowDialog(this);
            if(result== DialogResult.OK)
            {
                _options = templateOptions.ProvisioningOptions;

            }

        } //bOptions_Click

        private void SetStatusText(object sender, EventArgs e)
        {
            string tag = (sender as Control).Tag.ToString();
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(tag));

        } //SetStatusText

        private void SetStatusDefault(object sender, EventArgs e)
        {
            SetStatusBarText(Properties.Resources.ResourceManager.GetString(Constants.Source00));

        } //SetStatusDefault

    } //SourceWin

}
