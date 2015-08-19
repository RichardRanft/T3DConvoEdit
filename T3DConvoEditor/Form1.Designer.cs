namespace T3DConvoEditor
{
    partial class Form1
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
            Graph.Compatibility.AlwaysCompatible alwaysCompatible1 = new Graph.Compatibility.AlwaysCompatible();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlPalette = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStartNode = new System.Windows.Forms.Label();
            this.lblConvoNode = new System.Windows.Forms.Label();
            this.lblEndNode = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxConvoName = new System.Windows.Forms.TextBox();
            this.pnlWork = new System.Windows.Forms.Panel();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.graphCtrl = new Graph.GraphControl();
            this.sfdSaveGraphFile = new System.Windows.Forms.SaveFileDialog();
            this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.sfdExportScript = new System.Windows.Forms.SaveFileDialog();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.pnlPalette.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlWork.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1084, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exportToolStripMenuItem.Text = "Ex&port";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.pluginsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.pluginsToolStripMenuItem.Text = "Plugins";
            // 
            // pnlPalette
            // 
            this.pnlPalette.Controls.Add(this.groupBox2);
            this.pnlPalette.Controls.Add(this.groupBox1);
            this.pnlPalette.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPalette.Location = new System.Drawing.Point(0, 24);
            this.pnlPalette.Name = "pnlPalette";
            this.pnlPalette.Size = new System.Drawing.Size(167, 537);
            this.pnlPalette.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStartNode);
            this.groupBox2.Controls.Add(this.lblConvoNode);
            this.groupBox2.Controls.Add(this.lblEndNode);
            this.groupBox2.Location = new System.Drawing.Point(4, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 91);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Node Types";
            // 
            // lblStartNode
            // 
            this.lblStartNode.AutoSize = true;
            this.lblStartNode.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblStartNode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStartNode.Location = new System.Drawing.Point(9, 16);
            this.lblStartNode.Name = "lblStartNode";
            this.lblStartNode.Size = new System.Drawing.Size(60, 15);
            this.lblStartNode.TabIndex = 1;
            this.lblStartNode.Text = "Start Node";
            this.lblStartNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblStartNode_MouseDown);
            // 
            // lblConvoNode
            // 
            this.lblConvoNode.AutoSize = true;
            this.lblConvoNode.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblConvoNode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblConvoNode.Location = new System.Drawing.Point(9, 41);
            this.lblConvoNode.Name = "lblConvoNode";
            this.lblConvoNode.Size = new System.Drawing.Size(100, 15);
            this.lblConvoNode.TabIndex = 0;
            this.lblConvoNode.Text = "Conversation Node";
            this.lblConvoNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblConvoNode_MouseDown);
            // 
            // lblEndNode
            // 
            this.lblEndNode.AutoSize = true;
            this.lblEndNode.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.lblEndNode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEndNode.Location = new System.Drawing.Point(9, 66);
            this.lblEndNode.Name = "lblEndNode";
            this.lblEndNode.Size = new System.Drawing.Size(57, 15);
            this.lblEndNode.TabIndex = 2;
            this.lblEndNode.Text = "End Node";
            this.lblEndNode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblEndNode_MouseDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxConvoName);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 49);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conversation Name";
            // 
            // tbxConvoName
            // 
            this.tbxConvoName.Location = new System.Drawing.Point(9, 20);
            this.tbxConvoName.Name = "tbxConvoName";
            this.tbxConvoName.Size = new System.Drawing.Size(135, 20);
            this.tbxConvoName.TabIndex = 0;
            // 
            // pnlWork
            // 
            this.pnlWork.Controls.Add(this.pnlGraph);
            this.pnlWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWork.Location = new System.Drawing.Point(167, 24);
            this.pnlWork.Margin = new System.Windows.Forms.Padding(10);
            this.pnlWork.Name = "pnlWork";
            this.pnlWork.Size = new System.Drawing.Size(917, 537);
            this.pnlWork.TabIndex = 2;
            this.pnlWork.Resize += new System.EventHandler(this.pnlWork_Resize);
            // 
            // pnlGraph
            // 
            this.pnlGraph.Location = new System.Drawing.Point(7, 4);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(200, 100);
            this.pnlGraph.TabIndex = 0;
            // 
            // graphCtrl
            // 
            this.graphCtrl.AllowDrop = true;
            this.graphCtrl.BackColor = System.Drawing.Color.Gray;
            this.graphCtrl.CompatibilityStrategy = alwaysCompatible1;
            this.graphCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphCtrl.FocusElement = null;
            this.graphCtrl.HighlightCompatible = true;
            this.graphCtrl.LargeGridStep = 128F;
            this.graphCtrl.LargeStepGridColor = System.Drawing.Color.LightGray;
            this.graphCtrl.Location = new System.Drawing.Point(0, 0);
            this.graphCtrl.Name = "graphCtrl";
            this.graphCtrl.ShowLabels = false;
            this.graphCtrl.Size = new System.Drawing.Size(1084, 561);
            this.graphCtrl.SmallGridStep = 16F;
            this.graphCtrl.SmallStepGridColor = System.Drawing.Color.DarkGray;
            this.graphCtrl.TabIndex = 0;
            this.graphCtrl.Text = "graphCtrl";
            this.graphCtrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.graphCtrl_KeyUp);
            // 
            // sfdSaveGraphFile
            // 
            this.sfdSaveGraphFile.DefaultExt = "json";
            this.sfdSaveGraphFile.Filter = "JSON Files|*.json";
            // 
            // ofdOpenFile
            // 
            this.ofdOpenFile.DefaultExt = "(JSON)|*.json";
            this.ofdOpenFile.FileName = "*.json";
            this.ofdOpenFile.InitialDirectory = ".\\Conversations\\";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1084, 561);
            this.Controls.Add(this.pnlWork);
            this.Controls.Add(this.pnlPalette);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.graphCtrl);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "Form1";
            this.Text = "T3D Conversation Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlPalette.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlWork.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Graph.GraphControl graphCtrl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Panel pnlPalette;
        private System.Windows.Forms.Panel pnlWork;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.Label lblConvoNode;
        private System.Windows.Forms.Label lblEndNode;
        private System.Windows.Forms.Label lblStartNode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxConvoName;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdSaveGraphFile;
        private System.Windows.Forms.OpenFileDialog ofdOpenFile;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdExportScript;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    }
}

