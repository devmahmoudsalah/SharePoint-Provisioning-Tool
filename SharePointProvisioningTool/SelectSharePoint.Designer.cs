namespace Karabina.SharePoint.Provisioning
{
    partial class SelectSharePoint
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
            this.rbSP2013OP = new System.Windows.Forms.RadioButton();
            this.rbSP2016OP = new System.Windows.Forms.RadioButton();
            this.rbSP2016OL = new System.Windows.Forms.RadioButton();
            this.lTopic = new System.Windows.Forms.Label();
            this.bOkay = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbSP2013OP
            // 
            this.rbSP2013OP.AutoSize = true;
            this.rbSP2013OP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbSP2013OP.Location = new System.Drawing.Point(25, 46);
            this.rbSP2013OP.Name = "rbSP2013OP";
            this.rbSP2013OP.Size = new System.Drawing.Size(190, 20);
            this.rbSP2013OP.TabIndex = 2;
            this.rbSP2013OP.TabStop = true;
            this.rbSP2013OP.Tag = "1";
            this.rbSP2013OP.Text = " SharePoint 2013 On Premises ";
            this.rbSP2013OP.UseVisualStyleBackColor = true;
            this.rbSP2013OP.CheckedChanged += new System.EventHandler(this.SetVersionSelected);
            // 
            // rbSP2016OP
            // 
            this.rbSP2016OP.AutoSize = true;
            this.rbSP2016OP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbSP2016OP.Location = new System.Drawing.Point(25, 71);
            this.rbSP2016OP.Name = "rbSP2016OP";
            this.rbSP2016OP.Size = new System.Drawing.Size(190, 20);
            this.rbSP2016OP.TabIndex = 3;
            this.rbSP2016OP.TabStop = true;
            this.rbSP2016OP.Tag = "2";
            this.rbSP2016OP.Text = " SharePoint 2016 On Premises ";
            this.rbSP2016OP.UseVisualStyleBackColor = true;
            this.rbSP2016OP.CheckedChanged += new System.EventHandler(this.SetVersionSelected);
            // 
            // rbSP2016OL
            // 
            this.rbSP2016OL.AutoSize = true;
            this.rbSP2016OL.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.rbSP2016OL.Location = new System.Drawing.Point(25, 96);
            this.rbSP2016OL.Name = "rbSP2016OL";
            this.rbSP2016OL.Size = new System.Drawing.Size(159, 20);
            this.rbSP2016OL.TabIndex = 4;
            this.rbSP2016OL.TabStop = true;
            this.rbSP2016OL.Tag = "3";
            this.rbSP2016OL.Text = " SharePoint 2016 Online ";
            this.rbSP2016OL.UseVisualStyleBackColor = true;
            this.rbSP2016OL.CheckedChanged += new System.EventHandler(this.SetVersionSelected);
            // 
            // lTopic
            // 
            this.lTopic.AutoSize = true;
            this.lTopic.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lTopic.Location = new System.Drawing.Point(22, 19);
            this.lTopic.Name = "lTopic";
            this.lTopic.Size = new System.Drawing.Size(307, 15);
            this.lTopic.TabIndex = 1;
            this.lTopic.Text = "Select the SharePoint version to create the template from";
            // 
            // bOkay
            // 
            this.bOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bOkay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOkay.Location = new System.Drawing.Point(103, 153);
            this.bOkay.Name = "bOkay";
            this.bOkay.Size = new System.Drawing.Size(75, 23);
            this.bOkay.TabIndex = 5;
            this.bOkay.Text = "&OK";
            this.bOkay.UseVisualStyleBackColor = true;
            this.bOkay.Click += new System.EventHandler(this.bOkay_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bCancel.Location = new System.Drawing.Point(235, 153);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 6;
            this.bCancel.Text = "&Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // SelectSharePoint
            // 
            this.AcceptButton = this.bOkay;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(393, 204);
            this.ControlBox = false;
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOkay);
            this.Controls.Add(this.lTopic);
            this.Controls.Add(this.rbSP2013OP);
            this.Controls.Add(this.rbSP2016OP);
            this.Controls.Add(this.rbSP2016OL);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectSharePoint";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select SharePoint Version";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbSP2013OP;
        private System.Windows.Forms.RadioButton rbSP2016OP;
        private System.Windows.Forms.RadioButton rbSP2016OL;
        private System.Windows.Forms.Label lTopic;
        private System.Windows.Forms.Button bOkay;
        private System.Windows.Forms.Button bCancel;
    }
}