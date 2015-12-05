﻿// Copyright (c) 2015 Richard Ranft

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
            }
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

        private void lblStartNode_MouseDown(object sender, MouseEventArgs e)
        {
            var node = new Node("Conversation Start");
            var startLabel = new NodeLabelItem("Conversation_Start", NodeIOMode.Output) { Tag = TagType.LABEL };
            startLabel.Name = "NodeName";
            node.AddItem(startLabel);
            this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private void lblConvoNode_MouseDown(object sender, MouseEventArgs e)
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            String nodeName = m_settings.Attributes["[Default]"]["DEFAULTNODENAME"] + "_" + getConvNodeCount().ToString().PadLeft(4, '0');
			var node = new Node("Conversation Node");
            var nodeNameItem = new NodeTextBoxItem(nodeName);
            nodeNameItem.Name = "NodeName";
            node.AddItem(nodeNameItem);
            NodeTextBoxItem displayText = new NodeTextBoxItem("Enter NPC text", NodeIOMode.None);
            displayText.Name = "DisplayText";
            node.AddItem(displayText);
            var inputLabel = new NodeLabelItem("Conversation input", NodeIOMode.Input) { Tag = TagType.LABEL };
            inputLabel.Name = nodeName + "_in";
            node.AddItem(inputLabel);
            var editNode = new NodeLabelItem("Click Here To Edit Output List");
            editNode.Name = "EditNodeItem";
            editNode.Clicked += new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
            node.AddItem(editNode);
            NodeCompositeItem firstButton = new NodeCompositeItem(NodeIOMode.Output) { Tag = TagType.TEXTBOX };
            firstButton.Name = "button_1";
            ItemTextBoxPart btnText = new ItemTextBoxPart("Enter player text");
            btnText.Name = "ConvText";
            ItemTextBoxPart btnMethod = new ItemTextBoxPart("Enter script method");
            btnMethod.Name = "ConvMethod";
            firstButton.AddPart(btnText);
            firstButton.AddPart(btnMethod);
            firstButton.Clicked += new EventHandler<NodeItemEventArgs>(editConvNode_MouseDown);
            node.AddItem(firstButton);
			this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private void lblEndNode_MouseDown(object sender, MouseEventArgs e)
        {
            var node = new Node("Conversation End");
            node.AddItem(new NodeTextBoxItem("Enter text"));
            String name = "Conversation_End_" + getEndNodeCount().ToString().PadLeft(3, '0');
            var endLabel = new NodeLabelItem(name, NodeIOMode.Input) { Tag = TagType.TEXTBOX };
            endLabel.Name = "NodeName";
            node.AddItem(endLabel);
            node.AddItem(new NodeTextBoxItem(m_currentPluginSettings.Attributes["[Default]"]["DEFAULTEXITMETHOD"]));
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
            if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && validateGraph())
            {
                m_currentPlugin.SaveGraph(graphCtrl, sfdSaveGraphFile.FileName);
                m_dirty = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ofdOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                graphCtrl = m_currentPlugin.LoadGraph(ofdOpenFile.FileName);
                m_dirty = false;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validateGraph())
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

        private bool validateGraph()
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            if (nodes.Count < 1)
            {
                MessageBox.Show("There is nothing to save.", "Graph Empty");
                return false;
            }
            if (!checkContents(nodes))
            {
                MessageBox.Show("There are no conversation nodes in this graph.", "Graph Incomplete");
                return false;
            }
            if (!checkConnections(nodes))
            {
                MessageBox.Show("You have unconnected inputs or outputs in your conversation graph.  Please review your graph and ensure all node inputs and outputs are connected.", "Check Connections");
                return false;
            }
            List<String> names = new List<string>();
            foreach(Node node in nodes)
            {
                foreach(NodeItem item in node.Items)
                {
                    if(item.Name == "NodeName")
                    {
                        String name = "";
                        if (item.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                        {
                            NodeTextBoxItem i = item as NodeTextBoxItem;
                            name = i.Text;
                        }
                        if (item.GetType().ToString() == "Graph.Items.NodeLabelItem")
                        {
                            NodeLabelItem i = item as NodeLabelItem;
                            name = i.Text;
                        }
                        if(names.Contains(name))
                        {
                            MessageBox.Show("Two or more nodes have the same name.  Node names must be unique.", "Duplicate Nodes Detected");
                            return false;
                        }
                        names.Add(name);
                    }
                }
            }

            return true;
        }

        private bool checkContents(List<Node> nodes)
        {
            bool convoNodeFound = false;

            foreach(Node node in nodes)
            {
                if(node.Title.Equals("Conversation Node"))
                {
                    convoNodeFound = true;
                    break;
                }
            }

            return convoNodeFound;
        }

        private bool checkConnections(List<Node> nodelist)
        {
            foreach(Node n in nodelist)
            {
                if (n.HasNoItems)
                    continue;
                if (n.AnyConnectorsDisconnected)
                    return false;
            }
            return true;
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
                    if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && validateGraph())
                    {
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
            m_dirty = false;
        }

        private void pluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_plugins.ShowDialog();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_newProjectDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_project = new CProject(m_log);
                m_project.Name = m_newProjectDlg.ProjectName;
                m_project.BaseFolder = m_newProjectDlg.BasePath;
                this.Text = "T3D Conversation Editor - " + m_project.Name;
            }
            if(!m_newProjectDlg.IsValid)
                MessageBox.Show("Invalid or empty project name or path.  Please try again.", "Error");
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            gbxConvName.Width = splitPanel.Panel1.Width - 10;
            tbxConvoName.Width = gbxConvName.Width - 10;
            gbxNodes.Width = splitPanel.Panel1.Width - 10;
            gbxProject.Width = splitPanel.Panel1.Width - 10;
            gbxProject.Height = splitPanel.Panel1.Height - gbxProject.Top - 6;
            lbxConvList.Width = gbxProject.Width - 12;
            lbxConvList.Height = gbxProject.Height - 30;
        }
    }
}
