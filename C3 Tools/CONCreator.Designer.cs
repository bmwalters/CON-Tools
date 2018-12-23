﻿namespace C3Tools
{
    partial class CONCreator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CONCreator));
            this.groupPanel1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControlPanel1 = new System.Windows.Forms.TabPage();
            this.picPin = new System.Windows.Forms.PictureBox();
            this.picWorking = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioLIVE = new System.Windows.Forms.RadioButton();
            this.radioCON = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboGameID = new System.Windows.Forms.ComboBox();
            this.picPackage = new System.Windows.Forms.PictureBox();
            this.picContent = new System.Windows.Forms.PictureBox();
            this.folderTree = new System.Windows.Forms.TreeView();
            this.node1 = new System.Windows.Forms.TreeNode();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabControlPanel6 = new System.Windows.Forms.TabPage();
            this.fileList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabControlPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderTree)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl2)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabControlPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.Controls.Add(this.tabControl1);
            this.groupPanel1.Location = new System.Drawing.Point(3, 314);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(506, 194);
            // 
            // 
            // 
            this.groupPanel1.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabControl1.Controls.Add(this.tabControlPanel1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(500, 188);
            this.tabControl1.TabIndex = 11;
            this.tabControl1.Text = "Media";
            // 
            // tabControlPanel1
            // 
            this.tabControlPanel1.AllowDrop = true;
            this.tabControlPanel1.Controls.Add(this.picPin);
            this.tabControlPanel1.Controls.Add(this.picWorking);
            this.tabControlPanel1.Controls.Add(this.groupBox1);
            this.tabControlPanel1.Controls.Add(this.label4);
            this.tabControlPanel1.Controls.Add(this.label2);
            this.tabControlPanel1.Controls.Add(this.txtDescription);
            this.tabControlPanel1.Controls.Add(this.txtDisplay);
            this.tabControlPanel1.Controls.Add(this.btnCreate);
            this.tabControlPanel1.Controls.Add(this.label1);
            this.tabControlPanel1.Controls.Add(this.cboGameID);
            this.tabControlPanel1.Controls.Add(this.picPackage);
            this.tabControlPanel1.Controls.Add(this.picContent);
            this.tabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel1.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel1.Name = "tabControlPanel1";
            this.tabControlPanel1.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel1.Size = new System.Drawing.Size(500, 162);
            this.tabControlPanel1.TabIndex = 1;
            // 
            // picPin
            // 
            this.picPin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picPin.BackColor = System.Drawing.Color.Transparent;
            this.picPin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPin.Image = global::C3Tools.Properties.Resources.unpinned;
            this.picPin.Location = new System.Drawing.Point(6, 138);
            this.picPin.Name = "picPin";
            this.picPin.Size = new System.Drawing.Size(20, 20);
            this.picPin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPin.TabIndex = 64;
            this.picPin.TabStop = false;
            this.picPin.Tag = "unpinned";
            this.toolTip1.SetToolTip(this.picPin, "Click to pin on top");
            this.picPin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picPin_MouseClick);
            // 
            // picWorking
            // 
            this.picWorking.BackColor = System.Drawing.Color.Transparent;
            this.picWorking.Image = global::C3Tools.Properties.Resources.working;
            this.picWorking.Location = new System.Drawing.Point(394, 88);
            this.picWorking.Name = "picWorking";
            this.picWorking.Size = new System.Drawing.Size(94, 15);
            this.picWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWorking.TabIndex = 57;
            this.picWorking.TabStop = false;
            this.picWorking.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.radioLIVE);
            this.groupBox1.Controls.Add(this.radioCON);
            this.groupBox1.Location = new System.Drawing.Point(394, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(93, 70);
            this.groupBox1.TabIndex = 50;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Format";
            // 
            // radioLIVE
            // 
            this.radioLIVE.AutoSize = true;
            this.radioLIVE.BackColor = System.Drawing.Color.Transparent;
            this.radioLIVE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioLIVE.Location = new System.Drawing.Point(6, 43);
            this.radioLIVE.Name = "radioLIVE";
            this.radioLIVE.Size = new System.Drawing.Size(84, 17);
            this.radioLIVE.TabIndex = 16;
            this.radioLIVE.Text = "LIVE (JTAG)";
            this.radioLIVE.UseVisualStyleBackColor = false;
            // 
            // radioCON
            // 
            this.radioCON.AutoSize = true;
            this.radioCON.BackColor = System.Drawing.Color.Transparent;
            this.radioCON.Checked = true;
            this.radioCON.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioCON.Location = new System.Drawing.Point(6, 19);
            this.radioCON.Name = "radioCON";
            this.radioCON.Size = new System.Drawing.Size(84, 17);
            this.radioCON.TabIndex = 15;
            this.radioCON.TabStop = true;
            this.radioCON.Text = "CON (Retail)";
            this.radioCON.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(22, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 48;
            this.label2.Text = "Package Title:";
            // 
            // txtDescription
            // 
            this.txtDescription.ForeColor = System.Drawing.Color.LightGray;
            this.txtDescription.Location = new System.Drawing.Point(91, 96);
            this.txtDescription.MaxLength = 80;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(222, 54);
            this.txtDescription.TabIndex = 47;
            this.txtDescription.Text = "Created with C3 CON Tools";
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            this.txtDescription.DoubleClick += new System.EventHandler(this.txtDescription_DoubleClick);
            this.txtDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDescription_KeyDown);
            this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
            this.txtDescription.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtDescription_MouseDown);
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(91, 36);
            this.txtDisplay.MaxLength = 80;
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.Size = new System.Drawing.Size(222, 54);
            this.txtDisplay.TabIndex = 46;
            this.txtDisplay.DoubleClick += new System.EventHandler(this.txtDisplay_DoubleClick);
            this.txtDisplay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDisplay_KeyDown);
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(169)))), ((int)(((byte)(31)))));
            this.btnCreate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreate.Enabled = false;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.ForeColor = System.Drawing.Color.White;
            this.btnCreate.Location = new System.Drawing.Point(394, 111);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(94, 39);
            this.btnCreate.TabIndex = 9;
            this.btnCreate.Text = "&Create Package";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(30, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Game ID:";
            // 
            // cboGameID
            // 
            this.cboGameID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboGameID.FormattingEnabled = true;
            this.cboGameID.Items.AddRange(new object[] {
            "45410829 - Rock Band",
            "45410869 - Rock Band 2",
            "45410914 - Rock Band 3"});
            this.cboGameID.Location = new System.Drawing.Point(92, 9);
            this.cboGameID.Name = "cboGameID";
            this.cboGameID.Size = new System.Drawing.Size(221, 21);
            this.cboGameID.TabIndex = 44;
            this.cboGameID.Text = "45410914 - Rock Band 3";
            this.toolTip1.SetToolTip(this.cboGameID, "Changing the game ID will most likely break the custom! Be careful!");
            this.cboGameID.SelectedIndexChanged += new System.EventHandler(this.cboGameID_SelectedIndexChanged);
            // 
            // picPackage
            // 
            this.picPackage.BackColor = System.Drawing.Color.Transparent;
            this.picPackage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPackage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picPackage.Image = global::C3Tools.Properties.Resources.RB3;
            this.picPackage.Location = new System.Drawing.Point(321, 9);
            this.picPackage.Name = "picPackage";
            this.picPackage.Size = new System.Drawing.Size(64, 64);
            this.picPackage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPackage.TabIndex = 6;
            this.picPackage.TabStop = false;
            this.picPackage.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox2_DragDrop);
            this.picPackage.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.picPackage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseClick);
            // 
            // picContent
            // 
            this.picContent.BackColor = System.Drawing.Color.Transparent;
            this.picContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picContent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picContent.Image = global::C3Tools.Properties.Resources.RB3;
            this.picContent.Location = new System.Drawing.Point(321, 86);
            this.picContent.Name = "picContent";
            this.picContent.Size = new System.Drawing.Size(64, 64);
            this.picContent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picContent.TabIndex = 5;
            this.picContent.TabStop = false;
            this.picContent.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox1_DragDrop);
            this.picContent.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            this.picContent.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);

            // 
            // folderTree
            // 
            this.folderTree.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.folderTree.AllowDrop = true;
            this.folderTree.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.folderTree.Location = new System.Drawing.Point(10, 4);
            this.folderTree.Name = "folderTree";
            this.folderTree.Nodes.Add(this.node1);
            this.folderTree.PathSeparator = ";";
            this.folderTree.Size = new System.Drawing.Size(216, 268);
            this.folderTree.TabIndex = 6;
            this.folderTree.Text = "advTree1";
            this.folderTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.advTree1_DragDrop);
            this.folderTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // node1
            // 
            this.node1.ContextMenuStrip = this.contextMenuStrip2;
            this.node1.Name = "node1";
            this.node1.Text = "Root";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(108, 26);
            // 
            // addFolderToolStripMenuItem
            // 
            this.addFolderToolStripMenuItem.Name = "addFolderToolStripMenuItem";
            this.addFolderToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addFolderToolStripMenuItem.Text = "Add Folder";
            this.addFolderToolStripMenuItem.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(98, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.addFileToolStripMenuItem.Text = "Add Files";
            this.addFileToolStripMenuItem.Click += new System.EventHandler(this.addFileToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFolderToolStripMenuItem1,
            this.deleteFolderToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.ShowImageMargin = false;
            this.contextMenuStrip3.Size = new System.Drawing.Size(119, 48);
            // 
            // addFolderToolStripMenuItem1
            // 
            this.addFolderToolStripMenuItem1.Name = "addFolderToolStripMenuItem1";
            this.addFolderToolStripMenuItem1.Size = new System.Drawing.Size(118, 22);
            this.addFolderToolStripMenuItem1.Text = "Add Folder";
            this.addFolderToolStripMenuItem1.Click += new System.EventHandler(this.addFolderToolStripMenuItem_Click);
            // 
            // deleteFolderToolStripMenuItem
            // 
            this.deleteFolderToolStripMenuItem.Name = "deleteFolderToolStripMenuItem";
            this.deleteFolderToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.deleteFolderToolStripMenuItem.Text = "Delete Folder";
            this.deleteFolderToolStripMenuItem.Click += new System.EventHandler(this.deleteFolderToolStripMenuItem_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.tabControl2.Controls.Add(this.tabControlPanel6);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.Size = new System.Drawing.Size(511, 302);
            this.tabControl2.TabIndex = 8;
            this.tabControl2.Text = "tabControl2";
            // 
            // tabControlPanel6
            // 
            this.tabControlPanel6.Controls.Add(this.folderTree);
            this.tabControlPanel6.Controls.Add(this.fileList);
            this.tabControlPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlPanel6.Location = new System.Drawing.Point(0, 26);
            this.tabControlPanel6.Name = "tabControlPanel6";
            this.tabControlPanel6.Padding = new System.Windows.Forms.Padding(1);
            this.tabControlPanel6.Size = new System.Drawing.Size(511, 276);
            this.tabControlPanel6.TabIndex = 7;
            // 
            // fileList
            // 
            this.fileList.AllowDrop = true;
            this.fileList.BackgroundImage = global::C3Tools.Properties.Resources.dragdropfiles1;
            this.fileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.fileList.ContextMenuStrip = this.contextMenuStrip1;
            this.fileList.Location = new System.Drawing.Point(232, 5);
            this.fileList.Name = "fileList";
            this.fileList.Size = new System.Drawing.Size(273, 267);
            this.fileList.TabIndex = 7;
            this.fileList.UseCompatibleStateImageBehavior = false;
            this.fileList.View = System.Windows.Forms.View.Details;
            this.fileList.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.fileList.DragEnter += new System.Windows.Forms.DragEventHandler(this.HandleDragEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File";
            this.columnHeader1.Width = 262;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // CONCreator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(511, 510);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CONCreator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CON Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CONCreator_FormClosing);
            this.groupPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabControlPanel1.ResumeLayout(false);
            this.tabControlPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWorking)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.folderTree)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabControl2)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabControlPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupPanel1;
        private System.Windows.Forms.TreeView folderTree;
        private System.Windows.Forms.TreeNode node1;
        private System.Windows.Forms.ListView fileList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabControlPanel1;
        private System.Windows.Forms.PictureBox picPackage;
        private System.Windows.Forms.PictureBox picContent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem addFolderToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem deleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabControlPanel6;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton radioLIVE;
        private System.Windows.Forms.RadioButton radioCON;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ComboBox cboGameID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtDisplay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picWorking;
        private System.Windows.Forms.PictureBox picPin;
    }
}