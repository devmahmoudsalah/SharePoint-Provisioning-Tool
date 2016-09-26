namespace Karabina.SharePoint.Provisioning
{
    partial class TargetWin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbNoUNP = new System.Windows.Forms.CheckBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lPassword = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.lUserName = new System.Windows.Forms.Label();
            this.tbSharePointUrl = new System.Windows.Forms.TextBox();
            this.lSharePointUrl = new System.Windows.Forms.Label();
            this.bBrowse = new System.Windows.Forms.Button();
            this.lTemplate = new System.Windows.Forms.Label();
            this.tbTemplate = new System.Windows.Forms.TextBox();
            this.bApply = new System.Windows.Forms.Button();
            this.lTemplateError = new System.Windows.Forms.Label();
            this.lPasswordError = new System.Windows.Forms.Label();
            this.lUserNameError = new System.Windows.Forms.Label();
            this.lSharePointUrlError = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.bOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbNoUNP
            // 
            this.cbNoUNP.AutoSize = true;
            this.cbNoUNP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbNoUNP.Location = new System.Drawing.Point(21, 146);
            this.cbNoUNP.Name = "cbNoUNP";
            this.cbNoUNP.Size = new System.Drawing.Size(182, 20);
            this.cbNoUNP.TabIndex = 8;
            this.cbNoUNP.Tag = "Target4";
            this.cbNoUNP.Text = "Authentication not required ";
            this.cbNoUNP.UseVisualStyleBackColor = true;
            this.cbNoUNP.CheckedChanged += new System.EventHandler(this.cbNoUNP_CheckedChanged);
            this.cbNoUNP.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.cbNoUNP.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(21, 262);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(243, 23);
            this.tbPassword.TabIndex = 13;
            this.tbPassword.Tag = "Target6";
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tbPassword.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // lPassword
            // 
            this.lPassword.AutoSize = true;
            this.lPassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lPassword.Location = new System.Drawing.Point(21, 244);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(57, 15);
            this.lPassword.TabIndex = 12;
            this.lPassword.Text = "Password";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(21, 199);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(243, 23);
            this.tbUserName.TabIndex = 10;
            this.tbUserName.Tag = "Target5";
            this.tbUserName.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tbUserName.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // lUserName
            // 
            this.lUserName.AutoSize = true;
            this.lUserName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lUserName.Location = new System.Drawing.Point(21, 180);
            this.lUserName.Name = "lUserName";
            this.lUserName.Size = new System.Drawing.Size(65, 15);
            this.lUserName.TabIndex = 9;
            this.lUserName.Text = "User Name";
            // 
            // tbSharePointUrl
            // 
            this.tbSharePointUrl.Location = new System.Drawing.Point(21, 105);
            this.tbSharePointUrl.Name = "tbSharePointUrl";
            this.tbSharePointUrl.Size = new System.Drawing.Size(499, 23);
            this.tbSharePointUrl.TabIndex = 6;
            this.tbSharePointUrl.Tag = "Target3";
            this.tbSharePointUrl.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tbSharePointUrl.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // lSharePointUrl
            // 
            this.lSharePointUrl.AutoSize = true;
            this.lSharePointUrl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lSharePointUrl.Location = new System.Drawing.Point(21, 87);
            this.lSharePointUrl.Name = "lSharePointUrl";
            this.lSharePointUrl.Size = new System.Drawing.Size(110, 15);
            this.lSharePointUrl.TabIndex = 5;
            this.lSharePointUrl.Text = "SharePoint Site URL";
            // 
            // bBrowse
            // 
            this.bBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bBrowse.Location = new System.Drawing.Point(445, 40);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 3;
            this.bBrowse.Tag = "Target2";
            this.bBrowse.Text = "Browse...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            this.bBrowse.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.bBrowse.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // lTemplate
            // 
            this.lTemplate.AutoSize = true;
            this.lTemplate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lTemplate.Location = new System.Drawing.Point(21, 20);
            this.lTemplate.Name = "lTemplate";
            this.lTemplate.Size = new System.Drawing.Size(57, 15);
            this.lTemplate.TabIndex = 1;
            this.lTemplate.Text = "Template";
            // 
            // tbTemplate
            // 
            this.tbTemplate.Location = new System.Drawing.Point(21, 40);
            this.tbTemplate.Name = "tbTemplate";
            this.tbTemplate.ReadOnly = true;
            this.tbTemplate.Size = new System.Drawing.Size(418, 23);
            this.tbTemplate.TabIndex = 2;
            this.tbTemplate.Tag = "Target1";
            this.tbTemplate.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tbTemplate.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // bApply
            // 
            this.bApply.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bApply.Location = new System.Drawing.Point(345, 323);
            this.bApply.Name = "bApply";
            this.bApply.Size = new System.Drawing.Size(75, 23);
            this.bApply.TabIndex = 16;
            this.bApply.Tag = "Target8";
            this.bApply.Text = "Apply";
            this.bApply.UseVisualStyleBackColor = true;
            this.bApply.Click += new System.EventHandler(this.bApply_Click);
            this.bApply.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.bApply.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // lTemplateError
            // 
            this.lTemplateError.AutoSize = true;
            this.lTemplateError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lTemplateError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTemplateError.ForeColor = System.Drawing.Color.Red;
            this.lTemplateError.Location = new System.Drawing.Point(24, 65);
            this.lTemplateError.Name = "lTemplateError";
            this.lTemplateError.Size = new System.Drawing.Size(286, 13);
            this.lTemplateError.TabIndex = 4;
            this.lTemplateError.Text = "Please specify the path and name for the template file.";
            this.lTemplateError.Visible = false;
            // 
            // lPasswordError
            // 
            this.lPasswordError.AutoSize = true;
            this.lPasswordError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lPasswordError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPasswordError.ForeColor = System.Drawing.Color.Red;
            this.lPasswordError.Location = new System.Drawing.Point(24, 287);
            this.lPasswordError.Name = "lPasswordError";
            this.lPasswordError.Size = new System.Drawing.Size(236, 13);
            this.lPasswordError.TabIndex = 14;
            this.lPasswordError.Text = "Please specify a value for the password field.";
            this.lPasswordError.Visible = false;
            // 
            // lUserNameError
            // 
            this.lUserNameError.AutoSize = true;
            this.lUserNameError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lUserNameError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lUserNameError.ForeColor = System.Drawing.Color.Red;
            this.lUserNameError.Location = new System.Drawing.Point(24, 224);
            this.lUserNameError.Name = "lUserNameError";
            this.lUserNameError.Size = new System.Drawing.Size(193, 13);
            this.lUserNameError.TabIndex = 11;
            this.lUserNameError.Text = "Please specify a value for the # field.";
            this.lUserNameError.Visible = false;
            // 
            // lSharePointUrlError
            // 
            this.lSharePointUrlError.AutoSize = true;
            this.lSharePointUrlError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lSharePointUrlError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSharePointUrlError.ForeColor = System.Drawing.Color.Red;
            this.lSharePointUrlError.Location = new System.Drawing.Point(24, 130);
            this.lSharePointUrlError.Name = "lSharePointUrlError";
            this.lSharePointUrlError.Size = new System.Drawing.Size(237, 13);
            this.lSharePointUrlError.TabIndex = 7;
            this.lSharePointUrlError.Text = "Please specify the URL of the SharePoint site.";
            this.lSharePointUrlError.Visible = false;
            // 
            // bClose
            // 
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bClose.Location = new System.Drawing.Point(445, 323);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 17;
            this.bClose.Tag = "Target9";
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            this.bClose.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.bClose.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // bOptions
            // 
            this.bOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOptions.Location = new System.Drawing.Point(245, 323);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 23);
            this.bOptions.TabIndex = 15;
            this.bOptions.Tag = "Target7";
            this.bOptions.Text = "Options";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.bOptions.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // TargetWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 353);
            this.ControlBox = false;
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.lTemplateError);
            this.Controls.Add(this.lPasswordError);
            this.Controls.Add(this.lUserNameError);
            this.Controls.Add(this.lSharePointUrlError);
            this.Controls.Add(this.bApply);
            this.Controls.Add(this.tbTemplate);
            this.Controls.Add(this.lTemplate);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.cbNoUNP);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.lPassword);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.lUserName);
            this.Controls.Add(this.tbSharePointUrl);
            this.Controls.Add(this.lSharePointUrl);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TargetWin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Tag = "Target0";
            this.Text = "Apply Template";
            this.Activated += new System.EventHandler(this.SetStatusText);
            this.Shown += new System.EventHandler(this.FormShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbNoUNP;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label lUserName;
        private System.Windows.Forms.TextBox tbSharePointUrl;
        private System.Windows.Forms.Label lSharePointUrl;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Label lTemplate;
        private System.Windows.Forms.TextBox tbTemplate;
        private System.Windows.Forms.Button bApply;
        private System.Windows.Forms.Label lTemplateError;
        private System.Windows.Forms.Label lPasswordError;
        private System.Windows.Forms.Label lUserNameError;
        private System.Windows.Forms.Label lSharePointUrlError;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bOptions;
    }
}

