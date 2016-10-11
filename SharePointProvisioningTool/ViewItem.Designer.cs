namespace Karabina.SharePoint.Provisioning
{
    partial class ViewItem
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
            this.lKey = new System.Windows.Forms.Label();
            this.tbKey = new System.Windows.Forms.TextBox();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.lValue = new System.Windows.Forms.Label();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lKey
            // 
            this.lKey.AutoSize = true;
            this.lKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lKey.Location = new System.Drawing.Point(14, 12);
            this.lKey.Name = "lKey";
            this.lKey.Size = new System.Drawing.Size(26, 15);
            this.lKey.TabIndex = 0;
            this.lKey.Text = "Key";
            // 
            // tbKey
            // 
            this.tbKey.Location = new System.Drawing.Point(14, 32);
            this.tbKey.Name = "tbKey";
            this.tbKey.ReadOnly = true;
            this.tbKey.Size = new System.Drawing.Size(395, 23);
            this.tbKey.TabIndex = 1;
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(14, 91);
            this.tbValue.Multiline = true;
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(395, 97);
            this.tbValue.TabIndex = 3;
            // 
            // lValue
            // 
            this.lValue.AutoSize = true;
            this.lValue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lValue.Location = new System.Drawing.Point(14, 73);
            this.lValue.Name = "lValue";
            this.lValue.Size = new System.Drawing.Size(36, 15);
            this.lValue.TabIndex = 2;
            this.lValue.Text = "Value";
            // 
            // bCancel
            // 
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bCancel.Location = new System.Drawing.Point(334, 207);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 5;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOK.Location = new System.Drawing.Point(234, 207);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 4;
            this.bOK.Text = "Ok";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // ViewItem
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(423, 243);
            this.ControlBox = false;
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.lValue);
            this.Controls.Add(this.tbKey);
            this.Controls.Add(this.lKey);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewItem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ViewItem";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lKey;
        private System.Windows.Forms.TextBox tbKey;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Label lValue;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
    }
}