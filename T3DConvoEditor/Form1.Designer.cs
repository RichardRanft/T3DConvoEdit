namespace ConvoEditor
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
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlPalette = new System.Windows.Forms.Panel();
            this.gbxNodes = new System.Windows.Forms.GroupBox();
            this.lblStartNode = new System.Windows.Forms.Label();
            this.lblConvoNode = new System.Windows.Forms.Label();
            this.lblEndNode = new System.Windows.Forms.Label();
            this.gbxConvName = new System.Windows.Forms.GroupBox();
            this.tbxConvoName = new System.Windows.Forms.TextBox();
            this.pnlWork = new System.Windows.Forms.Panel();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.graphCtrl = new Graph.GraphControl();
            this.sfdSaveGraphFile = new System.Windows.Forms.SaveFileDialog();
            this.ofdOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.sfdExportScript = new System.Windows.Forms.SaveFileDialog();
            this.splitPanel = new System.Windows.Forms.SplitContainer();
            this.menuStrip1.SuspendLayout();
            this.pnlPalette.SuspendLayout();
            this.gbxNodes.SuspendLayout();
            this.gbxConvName.SuspendLayout();
            this.pnlWork.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).BeginInit();
            this.splitPanel.Panel1.SuspendLayout();
            this.splitPanel.Panel2.SuspendLayout();
            this.splitPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.projectToolStripMenuItem,
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
            this.newProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.toolStripMenuItem2,
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
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(140, 6);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exportToolStripMenuItem.Text = "Ex&port";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(140, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
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
            this.pluginsToolStripMenuItem.Click += new System.EventHandler(this.pluginsToolStripMenuItem_Click);
            // 
            // pnlPalette
            // 
            this.pnlPalette.Controls.Add(this.gbxNodes);
            this.pnlPalette.Controls.Add(this.gbxConvName);
            this.pnlPalette.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPalette.Location = new System.Drawing.Point(0, 0);
            this.pnlPalette.Name = "pnlPalette";
            this.pnlPalette.Size = new System.Drawing.Size(156, 534);
            this.pnlPalette.TabIndex = 1;
            // 
            // gbxNodes
            // 
            this.gbxNodes.Controls.Add(this.lblStartNode);
            this.gbxNodes.Controls.Add(this.lblConvoNode);
            this.gbxNodes.Controls.Add(this.lblEndNode);
            this.gbxNodes.Location = new System.Drawing.Point(4, 59);
            this.gbxNodes.Name = "gbxNodes";
            this.gbxNodes.Size = new System.Drawing.Size(150, 207);
            this.gbxNodes.TabIndex = 4;
            this.gbxNodes.TabStop = false;
            this.gbxNodes.Text = "Node Types";
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
            // gbxConvName
            // 
            this.gbxConvName.Controls.Add(this.tbxConvoName);
            this.gbxConvName.Location = new System.Drawing.Point(4, 4);
            this.gbxConvName.Name = "gbxConvName";
            this.gbxConvName.Size = new System.Drawing.Size(150, 49);
            this.gbxConvName.TabIndex = 3;
            this.gbxConvName.TabStop = false;
            this.gbxConvName.Text = "Conversation Name";
            // 
            // tbxConvoName
            // 
            this.tbxConvoName.Location = new System.Drawing.Point(6, 19);
            this.tbxConvoName.Name = "tbxConvoName";
            this.tbxConvoName.Size = new System.Drawing.Size(135, 20);
            this.tbxConvoName.TabIndex = 0;
            // 
            // pnlWork
            // 
            this.pnlWork.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlWork.Controls.Add(this.pnlGraph);
            this.pnlWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWork.Location = new System.Drawing.Point(0, 0);
            this.pnlWork.Margin = new System.Windows.Forms.Padding(10);
            this.pnlWork.Name = "pnlWork";
            this.pnlWork.Size = new System.Drawing.Size(916, 534);
            this.pnlWork.TabIndex = 2;
            this.pnlWork.Resize += new System.EventHandler(this.pnlWork_Resize);
            // 
            // pnlGraph
            // 
            this.pnlGraph.Location = new System.Drawing.Point(237, 41);
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
            this.graphCtrl.Size = new System.Drawing.Size(1084, 562);
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
            // splitPanel
            // 
            this.splitPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPanel.Location = new System.Drawing.Point(0, 24);
            this.splitPanel.Name = "splitPanel";
            // 
            // splitPanel.Panel1
            // 
            this.splitPanel.Panel1.Controls.Add(this.pnlPalette);
            this.splitPanel.Panel1.Resize += new System.EventHandler(this.splitContainer1_Panel1_Resize);
            this.splitPanel.Panel1MinSize = 160;
            // 
            // splitPanel.Panel2
            // 
            this.splitPanel.Panel2.Controls.Add(this.pnlWork);
            this.splitPanel.Size = new System.Drawing.Size(1084, 538);
            this.splitPanel.SplitterDistance = 160;
            this.splitPanel.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1084, 562);
            this.Controls.Add(this.splitPanel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.graphCtrl);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1100, 600);
            this.Name = "Form1";
            this.Text = "Conversation Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlPalette.ResumeLayout(false);
            this.gbxNodes.ResumeLayout(false);
            this.gbxNodes.PerformLayout();
            this.gbxConvName.ResumeLayout(false);
            this.gbxConvName.PerformLayout();
            this.pnlWork.ResumeLayout(false);
            this.splitPanel.Panel1.ResumeLayout(false);
            this.splitPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).EndInit();
            this.splitPanel.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox gbxNodes;
        private System.Windows.Forms.GroupBox gbxConvName;
        private System.Windows.Forms.TextBox tbxConvoName;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdSaveGraphFile;
        private System.Windows.Forms.OpenFileDialog ofdOpenFile;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdExportScript;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitPanel;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    }
}

