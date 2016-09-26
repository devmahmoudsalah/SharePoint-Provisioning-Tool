namespace Karabina.SharePoint.Provisioning
{
    partial class EditWin
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
            this.tvTemplate = new System.Windows.Forms.TreeView();
            this.tbTemplate = new System.Windows.Forms.TextBox();
            this.lTemplate = new System.Windows.Forms.Label();
            this.bBrowse = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tvTemplate
            // 
            this.tvTemplate.FullRowSelect = true;
            this.tvTemplate.Location = new System.Drawing.Point(14, 61);
            this.tvTemplate.Name = "tvTemplate";
            this.tvTemplate.Size = new System.Drawing.Size(418, 394);
            this.tvTemplate.TabIndex = 3;
            // 
            // tbTemplate
            // 
            this.tbTemplate.Location = new System.Drawing.Point(14, 32);
            this.tbTemplate.Name = "tbTemplate";
            this.tbTemplate.ReadOnly = true;
            this.tbTemplate.Size = new System.Drawing.Size(418, 23);
            this.tbTemplate.TabIndex = 1;
            this.tbTemplate.Tag = "Source5";
            // 
            // lTemplate
            // 
            this.lTemplate.AutoSize = true;
            this.lTemplate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lTemplate.Location = new System.Drawing.Point(14, 12);
            this.lTemplate.Name = "lTemplate";
            this.lTemplate.Size = new System.Drawing.Size(57, 15);
            this.lTemplate.TabIndex = 0;
            this.lTemplate.Text = "Template";
            // 
            // bBrowse
            // 
            this.bBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bBrowse.Location = new System.Drawing.Point(438, 32);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 2;
            this.bBrowse.Tag = "Source6";
            this.bBrowse.Text = "Browse...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // bClose
            // 
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bClose.Location = new System.Drawing.Point(809, 445);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 18;
            this.bClose.Tag = "Source9";
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // EditWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 480);
            this.ControlBox = false;
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.tbTemplate);
            this.Controls.Add(this.lTemplate);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.tvTemplate);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditWin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Edit Provisioning Template";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvTemplate;
        private System.Windows.Forms.TextBox tbTemplate;
        private System.Windows.Forms.Label lTemplate;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Button bClose;
    }
}