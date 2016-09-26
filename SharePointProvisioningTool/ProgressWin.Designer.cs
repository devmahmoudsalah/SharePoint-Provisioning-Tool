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
            this.components = new System.ComponentModel.Container();
            this.lResult = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.ListBox();
            this.cmsPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyLines = new System.Windows.Forms.ToolStripMenuItem();
            this.bClose = new System.Windows.Forms.Button();
            this.cmsPopupMenu.SuspendLayout();
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
            this.lbResult.ContextMenuStrip = this.cmsPopupMenu;
            this.lbResult.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbResult.ItemHeight = 16;
            this.lbResult.Location = new System.Drawing.Point(18, 39);
            this.lbResult.Name = "lbResult";
            this.lbResult.ScrollAlwaysVisible = true;
            this.lbResult.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbResult.Size = new System.Drawing.Size(622, 259);
            this.lbResult.TabIndex = 1;
            this.lbResult.Tag = "Progress1";
            this.lbResult.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.lbResult.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // cmsPopupMenu
            // 
            this.cmsPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyAll,
            this.tsmiCopyLines});
            this.cmsPopupMenu.Name = "cmsPopupMenu";
            this.cmsPopupMenu.Size = new System.Drawing.Size(175, 48);
            // 
            // tsmiCopyAll
            // 
            this.tsmiCopyAll.Name = "tsmiCopyAll";
            this.tsmiCopyAll.Size = new System.Drawing.Size(174, 22);
            this.tsmiCopyAll.Tag = "Progress3";
            this.tsmiCopyAll.Text = "Copy All Text";
            this.tsmiCopyAll.Click += new System.EventHandler(this.CopyAllLines);
            this.tsmiCopyAll.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tsmiCopyAll.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // tsmiCopyLines
            // 
            this.tsmiCopyLines.Name = "tsmiCopyLines";
            this.tsmiCopyLines.Size = new System.Drawing.Size(174, 22);
            this.tsmiCopyLines.Tag = "Progress4";
            this.tsmiCopyLines.Text = "Copy Selected Text";
            this.tsmiCopyLines.Click += new System.EventHandler(this.CopyLines);
            this.tsmiCopyLines.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tsmiCopyLines.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // bClose
            // 
            this.bClose.Enabled = false;
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bClose.Location = new System.Drawing.Point(300, 323);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(75, 23);
            this.bClose.TabIndex = 2;
            this.bClose.Tag = "Progress2";
            this.bClose.Text = "Close";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            this.bClose.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.bClose.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // ProgressWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 361);
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
            this.Tag = "Progress0";
            this.Text = "ProgressWin";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.SetStatusText);
            this.cmsPopupMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lResult;
        private System.Windows.Forms.ListBox lbResult;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.ContextMenuStrip cmsPopupMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyLines;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopyAll;
    }
}