namespace Karabina.SharePoint.Provisioning
{
    partial class SharePointProvisioningTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharePointProvisioningTool));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.cascadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.editToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.windowsMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.MdiWindowListItem = this.windowsMenu;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1055, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "MenuStrip";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileMenu.ImageTransparentColor = System.Drawing.Color.White;
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Tag = "String09";
            this.fileMenu.Text = "&File";
            this.fileMenu.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.fileMenu.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.TemplateCreateRed;
            this.newToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(218, 38);
            this.newToolStripMenuItem.Tag = "String01";
            this.newToolStripMenuItem.Text = "Create &Template";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.ShowCreateForm);
            this.newToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.newToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.TemplateEditRed;
            this.editToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.editToolStripMenuItem.Size = new System.Drawing.Size(218, 38);
            this.editToolStripMenuItem.Tag = "String02";
            this.editToolStripMenuItem.Text = "&Edit Template";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.ShowEditForm);
            this.editToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.editToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.TemplateApplyRed;
            this.saveToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(218, 38);
            this.saveToolStripMenuItem.Tag = "String03";
            this.saveToolStripMenuItem.Text = "A&pply Template";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.ShowApplyForm);
            this.saveToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.saveToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(215, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.ExitApplicationRed;
            this.exitToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.exitToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShowShortcutKeys = false;
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(218, 38);
            this.exitToolStripMenuItem.Tag = "String04";
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolsStripMenuItem_Click);
            this.exitToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.exitToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // windowsMenu
            // 
            this.windowsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cascadeToolStripMenuItem,
            this.tileVerticalToolStripMenuItem,
            this.tileHorizontalToolStripMenuItem});
            this.windowsMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.windowsMenu.ImageTransparentColor = System.Drawing.Color.White;
            this.windowsMenu.Name = "windowsMenu";
            this.windowsMenu.Size = new System.Drawing.Size(68, 20);
            this.windowsMenu.Tag = "String08";
            this.windowsMenu.Text = "&Windows";
            this.windowsMenu.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.windowsMenu.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // cascadeToolStripMenuItem
            // 
            this.cascadeToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.ViewCascadeNew;
            this.cascadeToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cascadeToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.cascadeToolStripMenuItem.Name = "cascadeToolStripMenuItem";
            this.cascadeToolStripMenuItem.Size = new System.Drawing.Size(167, 38);
            this.cascadeToolStripMenuItem.Tag = "String05";
            this.cascadeToolStripMenuItem.Text = "&Cascade";
            this.cascadeToolStripMenuItem.Click += new System.EventHandler(this.CascadeToolStripMenuItem_Click);
            this.cascadeToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.cascadeToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // tileVerticalToolStripMenuItem
            // 
            this.tileVerticalToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.ViewVerticalNew;
            this.tileVerticalToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tileVerticalToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.tileVerticalToolStripMenuItem.Name = "tileVerticalToolStripMenuItem";
            this.tileVerticalToolStripMenuItem.Size = new System.Drawing.Size(167, 38);
            this.tileVerticalToolStripMenuItem.Tag = "String06";
            this.tileVerticalToolStripMenuItem.Text = "Tile &Vertical";
            this.tileVerticalToolStripMenuItem.Click += new System.EventHandler(this.TileVerticalToolStripMenuItem_Click);
            this.tileVerticalToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tileVerticalToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // tileHorizontalToolStripMenuItem
            // 
            this.tileHorizontalToolStripMenuItem.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.ViewHorizontalNew;
            this.tileHorizontalToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tileHorizontalToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.tileHorizontalToolStripMenuItem.Name = "tileHorizontalToolStripMenuItem";
            this.tileHorizontalToolStripMenuItem.Size = new System.Drawing.Size(167, 38);
            this.tileHorizontalToolStripMenuItem.Tag = "String07";
            this.tileHorizontalToolStripMenuItem.Text = "Tile &Horizontal";
            this.tileHorizontalToolStripMenuItem.Click += new System.EventHandler(this.TileHorizontalToolStripMenuItem_Click);
            this.tileHorizontalToolStripMenuItem.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.tileHorizontalToolStripMenuItem.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.editToolStripButton,
            this.saveToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1055, 39);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "ToolStrip";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.TemplateCreateRed;
            this.newToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.newToolStripButton.Tag = "String01";
            this.newToolStripButton.Text = "Create Template";
            this.newToolStripButton.Click += new System.EventHandler(this.ShowCreateForm);
            this.newToolStripButton.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.newToolStripButton.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // editToolStripButton
            // 
            this.editToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editToolStripButton.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.TemplateEditRed;
            this.editToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
            this.editToolStripButton.Name = "editToolStripButton";
            this.editToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.editToolStripButton.Tag = "String02";
            this.editToolStripButton.Text = "Edit Template";
            this.editToolStripButton.Click += new System.EventHandler(this.ShowEditForm);
            this.editToolStripButton.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.editToolStripButton.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = global::Karabina.SharePoint.Provisioning.Properties.Resources.TemplateApplyRed;
            this.saveToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.White;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(36, 36);
            this.saveToolStripButton.Tag = "String03";
            this.saveToolStripButton.Text = "Apply Template";
            this.saveToolStripButton.Click += new System.EventHandler(this.ShowApplyForm);
            this.saveToolStripButton.MouseEnter += new System.EventHandler(this.SetStatusText);
            this.saveToolStripButton.MouseLeave += new System.EventHandler(this.SetStatusDefault);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 432);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip.Size = new System.Drawing.Size(1055, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "StatusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel.Text = "#";
            // 
            // SharePointProvisioningTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1055, 454);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SharePointProvisioningTool";
            this.Text = "SharePoint Provisioning Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem tileHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowsMenu;
        private System.Windows.Forms.ToolStripMenuItem cascadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripButton editToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    }
}



