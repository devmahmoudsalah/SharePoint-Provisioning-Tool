namespace Karabina.SharePoint.Provisioning
{
    partial class SourceWin
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
            this.bCreate = new System.Windows.Forms.Button();
            this.lSharePointUrlError = new System.Windows.Forms.Label();
            this.lUserNameError = new System.Windows.Forms.Label();
            this.lPasswordError = new System.Windows.Forms.Label();
            this.lTemplateError = new System.Windows.Forms.Label();
            this.bClose = new System.Windows.Forms.Button();
            this.bOptions = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbNoUNP
            // 
            this.cbNoUNP.AutoSize = true;
            this.cbNoUNP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbNoUNP.Location = new System.Drawing.Point(21, 86);
            this.cbNoUNP.Name = "cbNoUNP";
            this.cbNoUNP.Size = new System.Drawing.Size(182, 20);
            this.cbNoUNP.TabIndex = 4;
            this.cbNoUNP.Text = "Authentication not required ";
            this.cbNoUNP.UseVisualStyleBackColor = true;
            this.cbNoUNP.CheckedChanged += new System.EventHandler(this.cbNoUNP_CheckedChanged);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(21, 197);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(243, 23);
            this.tbPassword.TabIndex = 9;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // lPassword
            // 
            this.lPassword.AutoSize = true;
            this.lPassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lPassword.Location = new System.Drawing.Point(21, 179);
            this.lPassword.Name = "lPassword";
            this.lPassword.Size = new System.Drawing.Size(57, 15);
            this.lPassword.TabIndex = 8;
            this.lPassword.Text = "Password";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(21, 134);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(243, 23);
            this.tbUserName.TabIndex = 6;
            // 
            // lUserName
            // 
            this.lUserName.AutoSize = true;
            this.lUserName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lUserName.Location = new System.Drawing.Point(21, 115);
            this.lUserName.Name = "lUserName";
            this.lUserName.Size = new System.Drawing.Size(65, 15);
            this.lUserName.TabIndex = 5;
            this.lUserName.Text = "User Name";
            // 
            // tbSharePointUrl
            // 
            this.tbSharePointUrl.Location = new System.Drawing.Point(21, 40);
            this.tbSharePointUrl.Name = "tbSharePointUrl";
            this.tbSharePointUrl.Size = new System.Drawing.Size(499, 23);
            this.tbSharePointUrl.TabIndex = 2;
            // 
            // lSharePointUrl
            // 
            this.lSharePointUrl.AutoSize = true;
            this.lSharePointUrl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lSharePointUrl.Location = new System.Drawing.Point(21, 22);
            this.lSharePointUrl.Name = "lSharePointUrl";
            this.lSharePointUrl.Size = new System.Drawing.Size(110, 15);
            this.lSharePointUrl.TabIndex = 1;
            this.lSharePointUrl.Text = "SharePoint Site URL";
            // 
            // bBrowse
            // 
            this.bBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bBrowse.Location = new System.Drawing.Point(445, 267);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 13;
            this.bBrowse.Text = "Browse...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // lTemplate
            // 
            this.lTemplate.AutoSize = true;
            this.lTemplate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lTemplate.Location = new System.Drawing.Point(21, 247);
            this.lTemplate.Name = "lTemplate";
            this.lTemplate.Size = new System.Drawing.Size(57, 15);
            this.lTemplate.TabIndex = 11;
            this.lTemplate.Text = "Template";
            // 
            // tbTemplate
            // 
            this.tbTemplate.Location = new System.Drawing.Point(21, 267);
            this.tbTemplate.Name = "tbTemplate";
            this.tbTemplate.ReadOnly = true;
            this.tbTemplate.Size = new System.Drawing.Size(418, 23);
            this.tbTemplate.TabIndex = 12;
            // 
            // bCreate
            // 
            this.bCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bCreate.Location = new System.Drawing.Point(300, 317);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(75, 23);
            this.bCreate.TabIndex = 15;
            this.bCreate.Text = "Create";
            this.bCreate.UseVisualStyleBackColor = true;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // lSharePointUrlError
            // 
            this.lSharePointUrlError.AutoSize = true;
            this.lSharePointUrlError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lSharePointUrlError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lSharePointUrlError.ForeColor = System.Drawing.Color.Red;
            this.lSharePointUrlError.Location = new System.Drawing.Point(24, 66);
            this.lSharePointUrlError.Name = "lSharePointUrlError";
            this.lSharePointUrlError.Size = new System.Drawing.Size(237, 13);
            this.lSharePointUrlError.TabIndex = 3;
            this.lSharePointUrlError.Text = "Please specify the URL of the SharePoint site.";
            this.lSharePointUrlError.Visible = false;
            // 
            // lUserNameError
            // 
            this.lUserNameError.AutoSize = true;
            this.lUserNameError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lUserNameError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lUserNameError.ForeColor = System.Drawing.Color.Red;
            this.lUserNameError.Location = new System.Drawing.Point(24, 159);
            this.lUserNameError.Name = "lUserNameError";
            this.lUserNameError.Size = new System.Drawing.Size(193, 13);
            this.lUserNameError.TabIndex = 7;
            this.lUserNameError.Text = "Please specify a value for the # field.";
            this.lUserNameError.Visible = false;
            // 
            // lPasswordError
            // 
            this.lPasswordError.AutoSize = true;
            this.lPasswordError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lPasswordError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lPasswordError.ForeColor = System.Drawing.Color.Red;
            this.lPasswordError.Location = new System.Drawing.Point(24, 222);
            this.lPasswordError.Name = "lPasswordError";
            this.lPasswordError.Size = new System.Drawing.Size(236, 13);
            this.lPasswordError.TabIndex = 10;
            this.lPasswordError.Text = "Please specify a value for the password field.";
            this.lPasswordError.Visible = false;
            // 
            // lTemplateError
            // 
            this.lTemplateError.AutoSize = true;
            this.lTemplateError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lTemplateError.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lTemplateError.ForeColor = System.Drawing.Color.Red;
            this.lTemplateError.Location = new System.Drawing.Point(24, 293);
            this.lTemplateError.Name = "lTemplateError";
            this.lTemplateError.Size = new System.Drawing.Size(286, 13);
            this.lTemplateError.TabIndex = 14;
            this.lTemplateError.Text = "Please specify the path and name for the template file.";
            this.lTemplateError.Visible = false;
            // 
            // bClose
            // 
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bClose.Location = new System.Drawing.Point(445, 317);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 16;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bOptions
            // 
            this.bOptions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOptions.Location = new System.Drawing.Point(155, 317);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(75, 23);
            this.bOptions.TabIndex = 17;
            this.bOptions.Text = "Options";
            this.bOptions.UseVisualStyleBackColor = true;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // SourceWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 345);
            this.ControlBox = false;
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.lTemplateError);
            this.Controls.Add(this.lPasswordError);
            this.Controls.Add(this.lUserNameError);
            this.Controls.Add(this.lSharePointUrlError);
            this.Controls.Add(this.bCreate);
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
            this.Name = "SourceWin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Create Template";
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
        private System.Windows.Forms.Button bCreate;
        private System.Windows.Forms.Label lSharePointUrlError;
        private System.Windows.Forms.Label lUserNameError;
        private System.Windows.Forms.Label lPasswordError;
        private System.Windows.Forms.Label lTemplateError;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bOptions;
    }
}

