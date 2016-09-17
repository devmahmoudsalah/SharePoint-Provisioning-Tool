namespace Karabina.SharePoint.Provisioning
{
    partial class ProgressWin
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
            this.lResult = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.ListBox();
            this.bClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lResult
            // 
            this.lResult.AutoSize = true;
            this.lResult.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lResult.Location = new System.Drawing.Point(18, 21);
            this.lResult.Name = "lResult";
            this.lResult.Size = new System.Drawing.Size(80, 15);
            this.lResult.TabIndex = 0;
            this.lResult.Text = "Result Output";
            // 
            // lbResult
            // 
            this.lbResult.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbResult.ItemHeight = 15;
            this.lbResult.Location = new System.Drawing.Point(18, 39);
            this.lbResult.Name = "lbResult";
            this.lbResult.ScrollAlwaysVisible = true;
            this.lbResult.Size = new System.Drawing.Size(621, 259);
            this.lbResult.TabIndex = 1;
            // 
            // bClose
            // 
            this.bClose.Enabled = false;
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bClose.Location = new System.Drawing.Point(289, 313);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 2;
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // ProgressWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 349);
            this.ControlBox = false;
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.lResult);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressWin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressWin";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lResult;
        private System.Windows.Forms.ListBox lbResult;
        private System.Windows.Forms.Button bClose;
    }
}