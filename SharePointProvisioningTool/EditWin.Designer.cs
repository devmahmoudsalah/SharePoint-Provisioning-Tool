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
            this.pRegionalSettings = new System.Windows.Forms.Panel();
            this.cbAdjustHijriDate = new System.Windows.Forms.ComboBox();
            this.lAdjustHijriDate = new System.Windows.Forms.Label();
            this.cbTimeFormat = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbWorkDayEndTime = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbFirstWeekOfYear = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbWorkDayStartTime = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbFirstDayOfWeek = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.clbWorkDays = new System.Windows.Forms.CheckedListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbAlternateCalendar = new System.Windows.Forms.ComboBox();
            this.cbShowWeekNumbers = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCalendar = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSortOrder = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLocale = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbTimeZone = new System.Windows.Forms.ComboBox();
            this.bSave = new System.Windows.Forms.Button();
            this.pComposedLook = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.tbComposedLookName = new System.Windows.Forms.TextBox();
            this.tbBackgroundFile = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbColorFile = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbFontFile = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbComposedLookVersion = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.pRegionalSettings.SuspendLayout();
            this.pComposedLook.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvTemplate
            // 
            this.tvTemplate.FullRowSelect = true;
            this.tvTemplate.Location = new System.Drawing.Point(14, 61);
            this.tvTemplate.Name = "tvTemplate";
            this.tvTemplate.Size = new System.Drawing.Size(418, 488);
            this.tvTemplate.TabIndex = 3;
            this.tvTemplate.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.NodeSelected);
            // 
            // tbTemplate
            // 
            this.tbTemplate.Location = new System.Drawing.Point(14, 32);
            this.tbTemplate.Name = "tbTemplate";
            this.tbTemplate.ReadOnly = true;
            this.tbTemplate.Size = new System.Drawing.Size(418, 23);
            this.tbTemplate.TabIndex = 1;
            this.tbTemplate.Tag = "";
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
            this.bBrowse.Tag = "";
            this.bBrowse.Text = "Browse...";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // pRegionalSettings
            // 
            this.pRegionalSettings.Controls.Add(this.cbAdjustHijriDate);
            this.pRegionalSettings.Controls.Add(this.lAdjustHijriDate);
            this.pRegionalSettings.Controls.Add(this.cbTimeFormat);
            this.pRegionalSettings.Controls.Add(this.label11);
            this.pRegionalSettings.Controls.Add(this.cbWorkDayEndTime);
            this.pRegionalSettings.Controls.Add(this.label9);
            this.pRegionalSettings.Controls.Add(this.cbFirstWeekOfYear);
            this.pRegionalSettings.Controls.Add(this.label10);
            this.pRegionalSettings.Controls.Add(this.cbWorkDayStartTime);
            this.pRegionalSettings.Controls.Add(this.label8);
            this.pRegionalSettings.Controls.Add(this.cbFirstDayOfWeek);
            this.pRegionalSettings.Controls.Add(this.label7);
            this.pRegionalSettings.Controls.Add(this.clbWorkDays);
            this.pRegionalSettings.Controls.Add(this.label6);
            this.pRegionalSettings.Controls.Add(this.label5);
            this.pRegionalSettings.Controls.Add(this.cbAlternateCalendar);
            this.pRegionalSettings.Controls.Add(this.cbShowWeekNumbers);
            this.pRegionalSettings.Controls.Add(this.label4);
            this.pRegionalSettings.Controls.Add(this.cbCalendar);
            this.pRegionalSettings.Controls.Add(this.label3);
            this.pRegionalSettings.Controls.Add(this.cbSortOrder);
            this.pRegionalSettings.Controls.Add(this.label2);
            this.pRegionalSettings.Controls.Add(this.cbLocale);
            this.pRegionalSettings.Controls.Add(this.label1);
            this.pRegionalSettings.Controls.Add(this.cbTimeZone);
            this.pRegionalSettings.Location = new System.Drawing.Point(1038, 61);
            this.pRegionalSettings.Name = "pRegionalSettings";
            this.pRegionalSettings.Size = new System.Drawing.Size(514, 488);
            this.pRegionalSettings.TabIndex = 4;
            this.pRegionalSettings.Visible = false;
            // 
            // cbAdjustHijriDate
            // 
            this.cbAdjustHijriDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdjustHijriDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbAdjustHijriDate.FormattingEnabled = true;
            this.cbAdjustHijriDate.Items.AddRange(new object[] {
            "-2",
            "-1",
            "0",
            "+1",
            "+2"});
            this.cbAdjustHijriDate.Location = new System.Drawing.Point(217, 237);
            this.cbAdjustHijriDate.Name = "cbAdjustHijriDate";
            this.cbAdjustHijriDate.Size = new System.Drawing.Size(52, 23);
            this.cbAdjustHijriDate.TabIndex = 24;
            this.cbAdjustHijriDate.Visible = false;
            // 
            // lAdjustHijriDate
            // 
            this.lAdjustHijriDate.AutoSize = true;
            this.lAdjustHijriDate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lAdjustHijriDate.Location = new System.Drawing.Point(17, 240);
            this.lAdjustHijriDate.Name = "lAdjustHijriDate";
            this.lAdjustHijriDate.Size = new System.Drawing.Size(197, 15);
            this.lAdjustHijriDate.TabIndex = 23;
            this.lAdjustHijriDate.Text = "Adjust Hijri date by number of days:";
            this.lAdjustHijriDate.Visible = false;
            // 
            // cbTimeFormat
            // 
            this.cbTimeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimeFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbTimeFormat.FormattingEnabled = true;
            this.cbTimeFormat.Location = new System.Drawing.Point(104, 441);
            this.cbTimeFormat.Name = "cbTimeFormat";
            this.cbTimeFormat.Size = new System.Drawing.Size(91, 23);
            this.cbTimeFormat.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label11.Location = new System.Drawing.Point(20, 444);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 15);
            this.label11.TabIndex = 21;
            this.label11.Text = "Time format:";
            // 
            // cbWorkDayEndTime
            // 
            this.cbWorkDayEndTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkDayEndTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbWorkDayEndTime.FormattingEnabled = true;
            this.cbWorkDayEndTime.Location = new System.Drawing.Point(404, 402);
            this.cbWorkDayEndTime.Name = "cbWorkDayEndTime";
            this.cbWorkDayEndTime.Size = new System.Drawing.Size(81, 23);
            this.cbWorkDayEndTime.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label9.Location = new System.Drawing.Point(337, 404);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 15);
            this.label9.TabIndex = 19;
            this.label9.Text = "End time:";
            // 
            // cbFirstWeekOfYear
            // 
            this.cbFirstWeekOfYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirstWeekOfYear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbFirstWeekOfYear.FormattingEnabled = true;
            this.cbFirstWeekOfYear.Location = new System.Drawing.Point(124, 402);
            this.cbFirstWeekOfYear.Name = "cbFirstWeekOfYear";
            this.cbFirstWeekOfYear.Size = new System.Drawing.Size(158, 23);
            this.cbFirstWeekOfYear.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label10.Location = new System.Drawing.Point(20, 404);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 15);
            this.label10.TabIndex = 17;
            this.label10.Text = "First week of year:";
            // 
            // cbWorkDayStartTime
            // 
            this.cbWorkDayStartTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWorkDayStartTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbWorkDayStartTime.FormattingEnabled = true;
            this.cbWorkDayStartTime.Location = new System.Drawing.Point(404, 372);
            this.cbWorkDayStartTime.Name = "cbWorkDayStartTime";
            this.cbWorkDayStartTime.Size = new System.Drawing.Size(81, 23);
            this.cbWorkDayStartTime.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label8.Location = new System.Drawing.Point(337, 375);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "Start time:";
            // 
            // cbFirstDayOfWeek
            // 
            this.cbFirstDayOfWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFirstDayOfWeek.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbFirstDayOfWeek.FormattingEnabled = true;
            this.cbFirstDayOfWeek.Location = new System.Drawing.Point(124, 372);
            this.cbFirstDayOfWeek.Name = "cbFirstDayOfWeek";
            this.cbFirstDayOfWeek.Size = new System.Drawing.Size(121, 23);
            this.cbFirstDayOfWeek.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label7.Location = new System.Drawing.Point(20, 375);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "First day of week:";
            // 
            // clbWorkDays
            // 
            this.clbWorkDays.BackColor = System.Drawing.SystemColors.Control;
            this.clbWorkDays.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbWorkDays.ColumnWidth = 60;
            this.clbWorkDays.FormattingEnabled = true;
            this.clbWorkDays.Location = new System.Drawing.Point(16, 342);
            this.clbWorkDays.MultiColumn = true;
            this.clbWorkDays.Name = "clbWorkDays";
            this.clbWorkDays.Size = new System.Drawing.Size(469, 18);
            this.clbWorkDays.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.Location = new System.Drawing.Point(13, 324);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Work week:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label5.Location = new System.Drawing.Point(13, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Alternate Calendar:";
            // 
            // cbAlternateCalendar
            // 
            this.cbAlternateCalendar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlternateCalendar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbAlternateCalendar.FormattingEnabled = true;
            this.cbAlternateCalendar.Location = new System.Drawing.Point(13, 289);
            this.cbAlternateCalendar.Name = "cbAlternateCalendar";
            this.cbAlternateCalendar.Size = new System.Drawing.Size(472, 23);
            this.cbAlternateCalendar.TabIndex = 9;
            // 
            // cbShowWeekNumbers
            // 
            this.cbShowWeekNumbers.AutoSize = true;
            this.cbShowWeekNumbers.Location = new System.Drawing.Point(13, 213);
            this.cbShowWeekNumbers.Name = "cbShowWeekNumbers";
            this.cbShowWeekNumbers.Size = new System.Drawing.Size(256, 19);
            this.cbShowWeekNumbers.TabIndex = 8;
            this.cbShowWeekNumbers.Text = " Show week numbers in the Date Navigator ";
            this.cbShowWeekNumbers.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(13, 165);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Calendar:";
            // 
            // cbCalendar
            // 
            this.cbCalendar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCalendar.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbCalendar.FormattingEnabled = true;
            this.cbCalendar.Location = new System.Drawing.Point(13, 184);
            this.cbCalendar.Name = "cbCalendar";
            this.cbCalendar.Size = new System.Drawing.Size(472, 23);
            this.cbCalendar.TabIndex = 6;
            this.cbCalendar.SelectedIndexChanged += new System.EventHandler(this.CalendarItemSelected);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(13, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Sort order:";
            // 
            // cbSortOrder
            // 
            this.cbSortOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSortOrder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbSortOrder.FormattingEnabled = true;
            this.cbSortOrder.Location = new System.Drawing.Point(13, 131);
            this.cbSortOrder.Name = "cbSortOrder";
            this.cbSortOrder.Size = new System.Drawing.Size(472, 23);
            this.cbSortOrder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(13, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Locale:";
            // 
            // cbLocale
            // 
            this.cbLocale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocale.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbLocale.FormattingEnabled = true;
            this.cbLocale.Location = new System.Drawing.Point(13, 77);
            this.cbLocale.Name = "cbLocale";
            this.cbLocale.Size = new System.Drawing.Size(472, 23);
            this.cbLocale.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(13, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Time zone: ";
            // 
            // cbTimeZone
            // 
            this.cbTimeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTimeZone.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbTimeZone.FormattingEnabled = true;
            this.cbTimeZone.Location = new System.Drawing.Point(13, 27);
            this.cbTimeZone.Name = "cbTimeZone";
            this.cbTimeZone.Size = new System.Drawing.Size(472, 23);
            this.cbTimeZone.TabIndex = 0;
            // 
            // bSave
            // 
            this.bSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bSave.Location = new System.Drawing.Point(877, 31);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 23);
            this.bSave.TabIndex = 5;
            this.bSave.Tag = "";
            this.bSave.Text = "Save";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Visible = false;
            // 
            // pComposedLook
            // 
            this.pComposedLook.Controls.Add(this.tbComposedLookVersion);
            this.pComposedLook.Controls.Add(this.label16);
            this.pComposedLook.Controls.Add(this.tbFontFile);
            this.pComposedLook.Controls.Add(this.label15);
            this.pComposedLook.Controls.Add(this.tbColorFile);
            this.pComposedLook.Controls.Add(this.label14);
            this.pComposedLook.Controls.Add(this.tbBackgroundFile);
            this.pComposedLook.Controls.Add(this.label13);
            this.pComposedLook.Controls.Add(this.tbComposedLookName);
            this.pComposedLook.Controls.Add(this.label12);
            this.pComposedLook.Location = new System.Drawing.Point(438, 61);
            this.pComposedLook.Name = "pComposedLook";
            this.pComposedLook.Size = new System.Drawing.Size(514, 488);
            this.pComposedLook.TabIndex = 6;
            this.pComposedLook.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label12.Location = new System.Drawing.Point(13, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(42, 15);
            this.label12.TabIndex = 0;
            this.label12.Text = "Name:";
            // 
            // tbComposedLookName
            // 
            this.tbComposedLookName.Location = new System.Drawing.Point(13, 27);
            this.tbComposedLookName.Name = "tbComposedLookName";
            this.tbComposedLookName.Size = new System.Drawing.Size(463, 23);
            this.tbComposedLookName.TabIndex = 1;
            // 
            // tbBackgroundFile
            // 
            this.tbBackgroundFile.Location = new System.Drawing.Point(13, 89);
            this.tbBackgroundFile.Name = "tbBackgroundFile";
            this.tbBackgroundFile.Size = new System.Drawing.Size(463, 23);
            this.tbBackgroundFile.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label13.Location = new System.Drawing.Point(13, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 15);
            this.label13.TabIndex = 2;
            this.label13.Text = "Background file:";
            // 
            // tbColorFile
            // 
            this.tbColorFile.Location = new System.Drawing.Point(13, 157);
            this.tbColorFile.Name = "tbColorFile";
            this.tbColorFile.Size = new System.Drawing.Size(463, 23);
            this.tbColorFile.TabIndex = 5;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label14.Location = new System.Drawing.Point(13, 138);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 15);
            this.label14.TabIndex = 4;
            this.label14.Text = "Color file:";
            // 
            // tbFontFile
            // 
            this.tbFontFile.Location = new System.Drawing.Point(13, 232);
            this.tbFontFile.Name = "tbFontFile";
            this.tbFontFile.Size = new System.Drawing.Size(463, 23);
            this.tbFontFile.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label15.Location = new System.Drawing.Point(13, 213);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 15);
            this.label15.TabIndex = 6;
            this.label15.Text = "Font file:";
            // 
            // tbComposedLookVersion
            // 
            this.tbComposedLookVersion.Location = new System.Drawing.Point(13, 302);
            this.tbComposedLookVersion.Name = "tbComposedLookVersion";
            this.tbComposedLookVersion.Size = new System.Drawing.Size(463, 23);
            this.tbComposedLookVersion.TabIndex = 9;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label16.Location = new System.Drawing.Point(13, 283);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 15);
            this.label16.TabIndex = 8;
            this.label16.Text = "Version:";
            // 
            // EditWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 561);
            this.Controls.Add(this.pComposedLook);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.pRegionalSettings);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Provisioning Template";
            this.pRegionalSettings.ResumeLayout(false);
            this.pRegionalSettings.PerformLayout();
            this.pComposedLook.ResumeLayout(false);
            this.pComposedLook.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tvTemplate;
        private System.Windows.Forms.TextBox tbTemplate;
        private System.Windows.Forms.Label lTemplate;
        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Panel pRegionalSettings;
        private System.Windows.Forms.ComboBox cbTimeZone;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbLocale;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSortOrder;
        private System.Windows.Forms.CheckBox cbShowWeekNumbers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbCalendar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbAlternateCalendar;
        private System.Windows.Forms.ComboBox cbTimeFormat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbWorkDayEndTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbFirstWeekOfYear;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbWorkDayStartTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbFirstDayOfWeek;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckedListBox clbWorkDays;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbAdjustHijriDate;
        private System.Windows.Forms.Label lAdjustHijriDate;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Panel pComposedLook;
        private System.Windows.Forms.TextBox tbBackgroundFile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbComposedLookName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbFontFile;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbColorFile;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbComposedLookVersion;
        private System.Windows.Forms.Label label16;
    }
}