// Copyright (c) 2015 Richard Ranft

// Permission is hereby granted, free of charge, to any person obtaining a copy of this 
// software and associated documentation files (the "Software"), to deal in the Software 
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following 
// conditions:

// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using BasicLogging;
using BasicSettings;
using PluginContracts;

namespace T3DConvoEditor
{
    public partial class Form1 : Form
    {
        private CLog m_log;
        private CSettings m_settings;
        private bool m_dirty;
        private FNodeEdit m_nodeEdit;
        private FConvPartEdit m_partEdit;
        private FPreferences m_preferences;
        private FPluginPage m_plugins;
        private FNewProject m_newProjectDlg;
        private FProjectSettings m_editProjectDlg;
        private String m_saveDefaultPath;
        private String m_personalPath;
        private Dictionary<string, IPlugin> m_availPlugins;
        private CSettings m_currentPluginSettings;
        private IPlugin m_currentPlugin;
        private CProject m_project;

        public Form1()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            String homeFolder = @Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            m_saveDefaultPath = homeFolder + @"\Roostertail Games\T3DConvoEditor\";
            m_dirty = false;
            m_log = new CLog();
            m_log.Filename = m_saveDefaultPath + "T3DConvoEditor.log";
            m_personalPath = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Personal)) + @"\Roostertail Games\T3DConvoEditor\"; ;
            m_log.WriteLine("Checking for settings file in " + m_personalPath);
            if (File.Exists(m_personalPath + "T3DConvoEditor.ini"))
            {
                m_log.WriteLine("Opening settings from " + m_personalPath);
                m_settings = new CSettings(m_personalPath + "T3DConvoEditor.ini");
            }
            else
            {
                m_log.WriteLine("Opening settings file from application path");
                m_settings = new CSettings("T3DConvoEditor.ini");
            }
            m_settings.LoadSettings();

            m_newProjectDlg = new FNewProject();
            m_editProjectDlg = new FProjectSettings();

            m_preferences = new FPreferences(m_settings);
            m_preferences.Settings = m_settings;

            m_nodeEdit = new FNodeEdit();
            m_nodeEdit.Text = "Edit Conversation Selection List";
            m_nodeEdit.MainForm = this;
            m_partEdit = new FConvPartEdit();
            m_plugins = new FPluginPage(m_log);
            m_log.WriteLine("T3D Conversation Editor started");

            if (!Directory.Exists(m_saveDefaultPath))
                Directory.CreateDirectory(m_saveDefaultPath);
            sfdSaveGraphFile.InitialDirectory = m_saveDefaultPath;
            ofdOpenFile.InitialDirectory = m_saveDefaultPath;

            String defFileName = m_settings.Attributes["[Default]"]["DEFAULTFILENAME"];
            tbxConvoName.Text = defFileName.Remove(defFileName.LastIndexOf('.'));

            graphCtrl.NodeAdded += new EventHandler<AcceptNodeEventArgs>(onNodeAdded);
            int width = pnlWork.Width - (2 * pnlWork.Margin.All);
            int height = pnlWork.Height - (2 * pnlWork.Margin.All);
            Rectangle graphBounds = new Rectangle(new Point(pnlWork.Margin.All, pnlWork.Margin.All), new Size(width, height));
            pnlGraph.Bounds = graphBounds;
            pnlGraph.Controls.Add(graphCtrl);

            m_availPlugins = m_plugins.Plugins;
            if(m_availPlugins.Keys.Count > 0)
            {
                String firstKey = "";
                foreach (String key in m_availPlugins.Keys)
                {
                    firstKey = key;
                    break;
                }
                IPlugin plugin = m_availPlugins[firstKey];
                plugin.Initialize(graphCtrl, m_log);
                m_currentPlugin = plugin;
                m_currentPluginSettings = plugin.Settings;
                m_plugins.SetActive(m_currentPlugin.Name);
            }
            createNodeButtons();
            //lbxConvList.Nodes.Clear();
            //TreeNode rootNode = new TreeNode();
            //lbxConvList.Nodes.Add(rootNode);
            m_project = new CProject(m_log);
            //m_project.TreeView = lbxConvList;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while(m_dirty)
            {
                System.Windows.Forms.DialogResult save = MessageBox.Show("Save before exiting?", "Save", MessageBoxButtons.YesNo);
                if (save == System.Windows.Forms.DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else
                    m_dirty = false;
            }
            Application.Exit();
        }

        private void pnlWork_Resize(object sender, EventArgs e)
        {
            int width = pnlWork.Width - (2 * pnlWork.Margin.All);
            int height = pnlWork.Height - (2 * pnlWork.Margin.All);
            Rectangle graphBounds = new Rectangle(new Point(pnlWork.Margin.All, pnlWork.Margin.All), new Size(width, height));
            pnlGraph.Bounds = graphBounds;
        }

        private void createNodeButtons()
        {
            gbxNodes.Controls.Clear();

            List<string> nodeTypes = m_currentPlugin.GetNodeTypenames();
            int top = 16;
            foreach (String type in nodeTypes)
            {
                Label lblBtn = new System.Windows.Forms.Label();
                lblBtn.AutoSize = true;
                lblBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
                lblBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                lblBtn.Location = new System.Drawing.Point(9, top);
                lblBtn.Size = new System.Drawing.Size(60, 15);
                lblBtn.TabIndex = 1;
                lblBtn.Text = type + " Node";
                MouseEventHandler hndl = m_currentPlugin.GetBtnHandler(this, type);
                lblBtn.MouseDown += hndl;
                gbxNodes.Controls.Add(lblBtn);
                top += lblBtn.Height + 4;
            }
        }

        private void lblStartNode_MouseDown(object sender, MouseEventArgs e)
        {
            var node = m_currentPlugin.GetNodeByTypename("start", ""); //new Node("Conversation Start");
            this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private void lblConvoNode_MouseDown(object sender, MouseEventArgs e)
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            String nodeName = m_settings.Attributes["[Default]"]["DEFAULTNODENAME"] + "_" + getConvNodeCount().ToString().PadLeft(4, '0');
            var node = m_currentPlugin.GetNodeByTypename("conversation", nodeName); //new Node("Conversation Node");
			this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private void lblEndNode_MouseDown(object sender, MouseEventArgs e)
        {
            String name = "Conversation_End_" + getEndNodeCount().ToString().PadLeft(3, '0');
            var node = m_currentPlugin.GetNodeByTypename("end", name); //new Node("Conversation End");
            this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private int getConvNodeCount()
        {
            int count = 0;

            foreach(Node node in graphCtrl.Nodes)
            {
                if (node.Title == "Conversation Node")
                    count++;
            }

            return count;
        }

        private int getEndNodeCount()
        {
            int count = 0;

            foreach (Node node in graphCtrl.Nodes)
            {
                if (node.Title == "Conversation End")
                    count++;
            }

            return count;
        }

        public EventHandler<NodeItemEventArgs> GetEditMouseHandler()
        {
            return new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
        }

        public EventHandler<NodeItemEventArgs> GetConvMouseHandler()
        {
            return new EventHandler<NodeItemEventArgs>(editConvNode_MouseDown);
        }

        private void editOutputListNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if(e.Item != null)
            {
                m_nodeEdit.EditingNode = e.Item.Node;
                m_nodeEdit.Settings = m_currentPluginSettings;
                m_nodeEdit.ShowDialog(this);
            }
        }

        private void editConvNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if (e.Item != null)
            {
                m_partEdit.Node = e.Item as NodeCompositeItem;
                m_partEdit.ShowDialog(this);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && m_currentPlugin.Validate(graphCtrl))
            {
                // see if current conversation is in current project.  If not, add it.

                // save conversation graph.
                m_currentPlugin.SaveGraph(graphCtrl, sfdSaveGraphFile.FileName);
                m_dirty = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdOpenFile.DefaultExt = "(JSON)|*.json";
            ofdOpenFile.FileName = "*.json";
            if (ofdOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                graphCtrl = m_currentPlugin.LoadGraph(ofdOpenFile.FileName);
                m_dirty = false;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_currentPlugin.Validate(graphCtrl))
            {
                if (m_availPlugins.ContainsKey("TSWriterPlugin"))
                {
                    IPlugin plugin = m_availPlugins["TSWriterPlugin"];
                    plugin.Initialize(graphCtrl, m_log);
                    sfdExportScript.DefaultExt = plugin.GetDefaultExtension();
                    sfdExportScript.InitialDirectory = m_settings.Attributes["[Default]"]["OUTPUTFOLDER"];
                    sfdExportScript.FileName = tbxConvoName.Text + ".cs";
                    sfdExportScript.Filter = "TorqueScript file|*.cs";
                    if(sfdExportScript.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        exportGraph(plugin, sfdExportScript.FileName);
                }
            }
        }

        private void exportGraph(IPlugin plugin, String filename)
        {
            plugin.Export(filename);
        }

        private void onNodeAdded(object sender, AcceptNodeEventArgs e)
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            if (nodes.Count > 0)
                m_dirty = true;
            graphCtrl.Focus();
        }

        void graphCtrl_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                GraphControl ctrl = (GraphControl)sender;
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        ctrl.RemoveNodes();
                        break;
                }
            }
            catch(Exception ex)
            {
                m_log.WriteLine("Unable to delete nodes from GraphControl : " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_log.WriteLine("Exiting T3DConvoEditor");
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_preferences.ShowDialog();
            String defFileName = m_settings.Attributes["[Default]"]["DEFAULTFILENAME"];
            tbxConvoName.Text = defFileName.Remove(defFileName.LastIndexOf('.'));
            m_settings = m_preferences.Settings;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_dirty)
            {
                if(MessageBox.Show("Save before starting a new conversation?", "Save", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    sfdSaveGraphFile.InitialDirectory = m_project.SaveFolder;
                    if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && m_currentPlugin.Validate(graphCtrl))
                    {
                        // handle project membership here
                        if(sfdSaveGraphFile.FileName.Contains(m_project.SaveFolder))
                        {
                            // is this save file already in the project?
                            if(!m_project.Contains(sfdSaveGraphFile.FileName))
                            {
                                m_project.AddConversation(sfdSaveGraphFile.FileName.Replace(m_project.SaveFolder, ""), sfdSaveGraphFile.FileName);
                                m_project.Save(m_project.BaseFolder + "\\" + m_project.Name + ".cnvproj");
                            }
                        }

                        CGraphManager graphman = new CGraphManager(this, m_log);
                        graphman.SaveGraph(graphCtrl, sfdSaveGraphFile.FileName);
                    }
                }
            }
            List<Node> nodeList = new List<Node>();
            foreach (Node n in graphCtrl.Nodes)
                nodeList.Add(n);
            graphCtrl.RemoveNodes(nodeList);
            graphCtrl.Refresh();

            // add new conversation to project here.
            String nodename = sfdSaveGraphFile.FileName.Replace(m_project.SaveFolder, "");
            nodename = nodename.Replace(m_project.SaveExt, "");
            //lbxConvList.AddPath(nodename);

            m_dirty = false;
        }

        private void pluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_plugins.ShowDialog();
            if(m_plugins.Active != m_currentPlugin)
            {
                if(MessageBox.Show("Save before switching plugins?", "Save", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    sfdSaveGraphFile.InitialDirectory = m_project.SaveFolder;
                    if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && m_currentPlugin.Validate(graphCtrl))
                    {
                        // handle project membership here
                        if(sfdSaveGraphFile.FileName.Contains(m_project.SaveFolder))
                        {
                            // is this save file already in the project?
                            if(!m_project.Contains(sfdSaveGraphFile.FileName))
                            {
                                m_project.AddConversation(sfdSaveGraphFile.FileName.Replace(m_project.SaveFolder, ""), sfdSaveGraphFile.FileName);
                                m_project.Save(m_project.BaseFolder + "\\" + m_project.Name + ".cnvproj");
                            }
                        }

                        CGraphManager graphman = new CGraphManager(this, m_log);
                        graphman.SaveGraph(graphCtrl, sfdSaveGraphFile.FileName);
                    }
                }
                List<Node> nodeList = new List<Node>();
                foreach (Node n in graphCtrl.Nodes)
                    nodeList.Add(n);
                graphCtrl.RemoveNodes(nodeList);
                graphCtrl.Refresh();
                m_currentPlugin = m_plugins.Active;
                m_currentPlugin.Initialize(graphCtrl, m_log);
                createNodeButtons();
            }
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            gbxConvName.Width = splitPanel.Panel1.Width - 10;
            tbxConvoName.Width = gbxConvName.Width - 16;
            gbxNodes.Width = splitPanel.Panel1.Width - 10;
            //gbxProject.Width = splitPanel.Panel1.Width - 10;
            //gbxProject.Height = splitPanel.Panel1.Height - gbxProject.Top - 6;
            //lbxConvList.Width = gbxProject.Width - 19;
            //lbxConvList.Height = gbxProject.Height - 30;
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_newProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (m_project != null && m_project.IsDirty)
                {
                    DialogResult res = MessageBox.Show("Do you want to save your current project?", "Save " + m_project.Name + "?", MessageBoxButtons.YesNoCancel);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                        m_project.Save(m_project.BaseFolder + "\\" + m_project.Name + ".cnvproj");
                    if (res == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }
                try
                {
                    if (!Directory.Exists(m_newProjectDlg.BasePath))
                        Directory.CreateDirectory(m_newProjectDlg.BasePath);
                }
                catch (Exception ex)
                {
                    String message = "Unable to create " + m_newProjectDlg.BasePath + " : " + Environment.NewLine;
                    message += ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += Environment.NewLine + ex.InnerException.Message;
                    }
                    m_log.WriteLine(message);
                    MessageBox.Show("Could not create " + m_newProjectDlg.BasePath + " : " + Environment.NewLine + ex.Message, "Error Creating Folder");
                    return;
                }
                m_project = new CProject(m_log);
                m_project.Name = m_newProjectDlg.ProjectName;
                m_project.BaseFolder = m_newProjectDlg.BasePath;
                m_project.SaveFolder = m_newProjectDlg.SavePath;
                m_project.ScriptFolder = m_newProjectDlg.ScriptPath;
                m_project.ScriptExt = m_currentPlugin.GetDefaultExtension();
                m_project.Save(m_project.BaseFolder + "\\" + m_project.Name + ".cnvproj");
                this.Text = "T3D Conversation Editor - " + m_project.Name;
                m_dirty = false;
            }
            else
                return;
            if(!m_newProjectDlg.IsValid)
                MessageBox.Show("Invalid or empty project name or path.  Please try again.", "Error");
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofdOpenFile.DefaultExt = "(Conversation Project)|*.cnvproj";
            ofdOpenFile.FileName = "*.cnvproj";
            if (ofdOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_project = new CProject(m_log, ofdOpenFile.FileName);
                this.Text = "T3D Conversation Editor - " + m_project.Name;
                //foreach (String item in m_project.Conversations.Keys)
                //    lbxConvList.AddPath(m_project.Conversations[item]);
                //lbxConvList.SetTopNodeName(m_project.Name);
                m_dirty = false;
            }
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save the project....
            m_project.Save(m_project.BaseFolder + "\\" + m_project.Name + ".cnvproj");
        }

        private void lbxConvList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // handle node double-click to open conversation save file.  Will prompt to save current
            // conversation tree before loading.
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_editProjectDlg.Project = m_project;
            if(m_editProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_project = m_editProjectDlg.Project;
            }
        }
    }
}
