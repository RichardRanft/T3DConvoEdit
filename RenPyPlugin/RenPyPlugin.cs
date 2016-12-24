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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using PluginContracts;
using BasicLogging;
using BasicSettings;

namespace RenPyPlugin
{
    public class CRenPyPlugin : IPlugin
    {
        private GraphControl m_graphCtrl;
        private CLog m_log;
        private CSettings m_settings;
        private String m_iniPath;
        private FMenuNodeEdit m_nodeEdit;
        private FConvPartEdit m_partEdit;
        private FSettings m_settingsForm;
        private List<String> m_nodeTypeNames;

        public String Name
        {
            get
            {
                return "RenPyPlugin";
            }
        }

        public CSettings Settings
        {
            get
            {
                return m_settings;
            }
        }

        public void ShowSettings()
        {
            m_settingsForm.ShowDialog();
        }

        public List<string> GetNodeTypenames()
        {
            return m_nodeTypeNames;
        }

        public Node GetNodeByTypename(string typename, string nodename = "")
        {
            switch (typename.ToLower())
            {
                case "start":
                    {
                        return getStartNode(nodename);
                    }
                case "end":
                    {
                        return getEndNode(nodename);
                    }
                case "conversation":
                    {
                        return getConversationNode(nodename);
                    }
                case "menu":
                    {
                        return getMenuNode(nodename);
                    }
                case "conditional":
                    {
                        return getConditionalNode(nodename);
                    }
            }
            return null;
        }

        private Node getStartNode(String name = "")
        {
            var node = new Node("Conversation Start");
            var startLabel = new NodeLabelItem("label start:", NodeIOMode.Output) { Tag = TagType.LABEL };
            startLabel.Name = "NodeName";
            node.AddItem(startLabel);
            return node;
        }

        private Node getEndNode(String name = "")
        {
            var node = new Node("Conversation End");
            node.AddItem(new NodeTextBoxItem("Enter text"));
            var endLabel = new NodeLabelItem(name, NodeIOMode.Input) { Tag = TagType.TEXTBOX };
            endLabel.Name = "NodeName";
            node.AddItem(endLabel);
            node.AddItem(new NodeTextBoxItem(m_settings.Attributes["[Default]"]["DEFAULTEXITMETHOD"]));
            return node;
        }

        private Node getConversationNode(String name = "")
        {
            var node = new Node("Conversation Node");

            var inputLabel = new NodeLabelItem("Conversation input", NodeIOMode.Input) { Tag = TagType.LABEL };
            inputLabel.Name = name + "_in";
            node.AddItem(inputLabel);

            var nodeNameItem = new NodeTextBoxItem(name);
            nodeNameItem.Name = "NodeName";
            node.AddItem(nodeNameItem);

            var bkgName = new NodeTextBoxItem("Enter Background Image Name", NodeIOMode.None);
            bkgName.Name = "Background";
            node.AddItem(bkgName);

            var sfxName = new NodeTextBoxItem("Enter Sound Effect Name", NodeIOMode.None);
            sfxName.Name = "SFX";
            node.AddItem(sfxName);

            var scriptBox = new NodeTextBoxItem("Enter Script Here", NodeIOMode.None, true);
            scriptBox.Name = "Script";
            node.AddItem(scriptBox);

            NodeTextBoxItem actor = new NodeTextBoxItem("Enter Character Name", NodeIOMode.None);
            actor.Name = "Character";
            node.AddItem(actor);

            NodeTextBoxItem dialog = new NodeTextBoxItem("Enter Character Speech", NodeIOMode.Output) { Tag = TagType.TEXTBOX };
            dialog.Name = "DisplayText";
            node.AddItem(dialog);

            return node;
        }

        private Node getMenuNode(String name = "")
        {
            var node = new Node("Menu Node");
            var nodeNameItem = new NodeTextBoxItem(name);
            nodeNameItem.Name = "NodeName";
            node.AddItem(nodeNameItem);

            var inputLabel = new NodeLabelItem("Conversation input", NodeIOMode.Input) { Tag = TagType.LABEL };
            inputLabel.Name = name + "_in";
            node.AddItem(inputLabel);

            var editNode = new NodeLabelItem("Click Here To Edit Output List");
            editNode.Name = "EditNodeItem";
            editNode.Clicked += new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
            node.AddItem(editNode);

            return node;
        }

        private Node getConditionalNode(String name = "")
        {
            var node = new Node("Conditional Node");

            var inputLabel = new NodeLabelItem("Conversation input", NodeIOMode.Input) { Tag = TagType.LABEL };
            inputLabel.Name = name + "_in";
            node.AddItem(inputLabel);

            var editNode = new NodeLabelItem("Click Here To Edit Output List");
            editNode.Name = "EditNodeItem";
            editNode.Clicked += new EventHandler<NodeItemEventArgs>(editConditionListNode_MouseDown);
            node.AddItem(editNode);

            return node;
        }

        private int getConvNodeCount()
        {
            int count = 0;

            foreach (Node node in m_graphCtrl.Nodes)
            {
                if (node.Title == "Conversation Node")
                    count++;
            }

            return count;
        }

        private int getEndNodeCount()
        {
            int count = 0;

            foreach (Node node in m_graphCtrl.Nodes)
            {
                if (node.Title == "Conversation End")
                    count++;
            }

            return count;
        }

        public EventHandler<NodeItemEventArgs> GetEditMouseHandler(string type = "")
        {
            String nodeType = type.ToLower();
            switch(nodeType)
            { 
                case "conditional":
                    return new EventHandler<NodeItemEventArgs>(editConditionListNode_MouseDown);
                case "menu":
                    return new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
                default:
                    return new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
            }
        }

        public EventHandler<NodeItemEventArgs> GetConvMouseHandler(string type = "")
        {
            String nodeType = type.ToLower();
            switch (nodeType)
            {
                case "conditional":
                    return new EventHandler<NodeItemEventArgs>(editConvNode_MouseDown);
                case "menu":
                    return new EventHandler<NodeItemEventArgs>(editConvNode_MouseDown);
                default:
                    return new EventHandler<NodeItemEventArgs>(editConvNode_MouseDown);
            }
        }

        private void editOutputListNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if (e.Item != null)
            {
                m_nodeEdit.PluginMain = this;
                m_nodeEdit.EditingNode = e.Item.Node;
                m_nodeEdit.Settings = m_settings;
                m_nodeEdit.ShowDialog();
            }
        }

        private void editConditionListNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if (e.Item != null)
            {
                m_nodeEdit.PluginMain = this;
                m_nodeEdit.EditingNode = e.Item.Node;
                m_nodeEdit.Settings = m_settings;
                m_nodeEdit.ShowDialog();
            }
        }

        private void editConvNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if (e.Item != null)
            {
                m_partEdit.Node = e.Item as NodeCompositeItem;
                m_partEdit.ShowDialog();
            }
        }

        public MouseEventHandler GetBtnHandler(Form parent, string type)
        {
            return new MouseEventHandler((sender, ea) => BtnHandler(sender, ea, parent, type));
        }

        public void BtnHandler(object sender, MouseEventArgs e, Form parent, string type)
        {
            String nodeType = type.ToLower();
            switch (nodeType)
            {
                case "start":
                    {
                        var node = GetNodeByTypename(nodeType); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
                case "menu":
                    {
                        var node = GetNodeByTypename(nodeType); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
                case "conditional":
                    {
                        var node = GetNodeByTypename(nodeType); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
                case "conversation":
                    {
                        var node = GetNodeByTypename(nodeType); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
                default:
                    {
                        var node = GetNodeByTypename(nodeType); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
            }
        }

        public bool SaveGraph(GraphControl graph, String filename)
        {
            CGraphManager graphman = new CGraphManager(m_log, this);
            graphman.SaveGraph(m_graphCtrl, filename);
            return false;
        }

        public GraphControl LoadGraph(String filename)
        {
            return null;
        }

        public void Initialize(GraphControl ctrl, CLog log)
        {
            m_nodeTypeNames = new List<string>();
            m_nodeTypeNames.Add("Start");
            m_nodeTypeNames.Add("End");
            m_nodeTypeNames.Add("Conversation");
            m_nodeTypeNames.Add("Menu");
            m_nodeTypeNames.Add("Conditional");
            m_graphCtrl = ctrl;
            m_log = log;
            String homeFolder = @Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            m_iniPath = homeFolder + @"\Roostertail Games\T3DConvoEditor\";
            String iniFile = m_iniPath + "RenPyPlugin.ini";
            m_log.WriteLine("Attempting to load " + iniFile);
            if (File.Exists(iniFile))
                m_settings = new CSettings(m_iniPath + "RenPyPlugin.ini");
            else
            {
                m_iniPath = Path.GetFullPath(".\\");
                iniFile = m_iniPath + @"Plugins\RenPyPlugin.ini";
                m_log.WriteLine("Attempting to load " + iniFile);
                m_settings = new CSettings(iniFile);
            }
            if (!m_settings.LoadSettings())
                m_log.WriteLine("Failed to locate RenPyPlugin.ini");
            else
                m_log.WriteLine("RenPyPlugin settings loaded");
            m_nodeEdit = new FMenuNodeEdit();
            m_partEdit = new FConvPartEdit();
            m_settingsForm = new FSettings();
        }

        public void Export(String filename)
        {
            if (m_graphCtrl == null)
                MessageBox.Show("Ren'Py Writer Plugin can't export graph - call SetGraphControl() to set the graph for export.");
            CRenPyWriter writer = new CRenPyWriter(m_log, m_settings);
            if (filename.Length < 1)
                filename = "defaultScript.cs";
            writer.WriteScript(filename, (List<Node>)m_graphCtrl.Nodes);
        }

        public System.String GetDefaultExtension()
        {
            return m_settings.Attributes["[Default]"]["DEFAULTEXT"];
        }

        class CRenPyWriter
        {
            private CLog m_log;
            private CSettings m_settings;

            public CRenPyWriter(CLog log, CSettings settings)
            {
                m_log = log;
                m_settings = settings;
            }

            public bool WriteScript(String filename, List<Node> nodelist)
            {
                try
                {
                    String conversation = Path.GetFileName(filename).Replace(".rpy", "");
                    String script = generateScript(conversation, nodelist);

                    using (StreamWriter sr = new StreamWriter(filename))
                    {
                        m_log.WriteLine("Writing script to " + filename);
                        sr.Write(script);
                    }
                    m_log.WriteLine(filename + " successfully written.");
                }
                catch (Exception ex)
                {
                    m_log.WriteLine("Failed to write conversation script " + filename + " : " + ex.Message + "\n" + ex.StackTrace);
                    return false;
                }
                return true;
            }

            private String generateScript(String convoName, List<Node> nodes)
            {
                m_log.WriteLine("Generating script.rpy...");

                String script = "";

                script += "## Use of T3DConvoEditor and its output are governed by the MIT license." + Environment.NewLine + Environment.NewLine;

                script += "## Permission is hereby granted, free of charge, to any person obtaining a copy" + Environment.NewLine;
                script += "## of this software and associated documentation files (the \"Software\"), to deal" + Environment.NewLine;
                script += "## in the Software without restriction, including without limitation the rights" + Environment.NewLine;
                script += "## to use, copy, modify, merge, publish, distribute, sublicense, and/or sell" + Environment.NewLine;
                script += "## copies of the Software, and to permit persons to whom the Software is" + Environment.NewLine;
                script += "## furnished to do so, subject to the following conditions:" + Environment.NewLine + Environment.NewLine;

                script += "## The above copyright notice and this permission notice shall be included in" + Environment.NewLine;
                script += "## all copies or substantial portions of the Software." + Environment.NewLine + Environment.NewLine;

                script += "## THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR" + Environment.NewLine;
                script += "## IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY," + Environment.NewLine;
                script += "## FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE" + Environment.NewLine;
                script += "## AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER" + Environment.NewLine;
                script += "## LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM," + Environment.NewLine;
                script += "## OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN" + Environment.NewLine;
                script += "## THE SOFTWARE." + Environment.NewLine + Environment.NewLine;

                script += "## The script of the game goes in this file." + Environment.NewLine + Environment.NewLine;

                script += "## Declare characters used by this game. The color argument colorizes the name" + Environment.NewLine;
                script += "## of the character." + Environment.NewLine + Environment.NewLine;

                // next write Character definitions


                // next write image definitions

                script += "## The game starts here." + Environment.NewLine;
                script += "label start:" + Environment.NewLine;

                CNodeWrapper node = new CNodeWrapper(nodes[0]);
                node.Log = m_log; // borrow the logger
                node.Settings = m_settings; // and the settings
                node.GenerateTree();
                node.Write(ref script);

                script += "    " + Environment.NewLine;
                script += "    ## This ends the game" + Environment.NewLine + Environment.NewLine;
                script += "    return" + Environment.NewLine;

                m_log.WriteLine(convoName + " script generated.");

                return script;
            }

            private String getStartNodeScript(String convName, Node node)
            {
                String script = "";
                List<NodeItem> items = (List<NodeItem>)node.Items;
                NodeLabelItem linkItem = (NodeLabelItem)items[0];
                // start writing script
                script += "label start:" + Environment.NewLine;

                List<NodeConnection> conns = (List<NodeConnection>)linkItem.Node.Connections;
                NodeConnection outconn = conns[0];
                List<NodeItem> targetItemList = (List<NodeItem>)outconn.To.Node.Items;
                NodeTextBoxItem targetItem = (NodeTextBoxItem)targetItemList[0];
                script += "    jump " + targetItem.Text + Environment.NewLine;
                m_log.WriteLine("Generated Conversation Start node");
                return script;
            }

            private String getConvoNodeScript(String convName, Node node)
            {
                String script = "";
                List<NodeItem> items = (List<NodeItem>)node.Items;
                NodeTextBoxItem nameItem = (NodeTextBoxItem)items[0];
                script += nameItem.Text + ":" + Environment.NewLine;
                int outNodeCount = items.Count - int.Parse(m_settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
                int start = int.Parse(m_settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
                NodeTextBoxItem nodeText = (NodeTextBoxItem)items[1];
                script += "    " + conditionText(nodeText.Text) + Environment.NewLine;

                String target = "";
                List<String> foundNodes = new List<String>();
                for (int i = start; i < items.Count; i++)
                {
                    NodeCompositeItem textItem = (NodeCompositeItem)items[i];
                    String Text = "";
                    String Method = "";
                    foreach (ItemTextBoxPart part in textItem.Parts)
                    {
                        if (part.Name == "ConvText")
                            Text = part.Text;
                        if (part.Name == "ConvMethod")
                            Method = part.Text;
                    }
                    NodeOutputConnector conn = (NodeOutputConnector)textItem.Output;
                    foreach (NodeConnection con in conn.Connectors)
                    {
                        bool found = false;
                        if (con.To.Node == textItem.Node)
                            continue;
                        foreach (NodeConnection targetCon in con.To.Node.Connections)
                        {
                            if (targetCon.From.Item != textItem)
                                continue;
                            foreach (NodeItem item in con.To.Node.Items)
                            {
                                if (item.Name == "NodeName" && item.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                                {
                                    NodeTextBoxItem targetItem = (NodeTextBoxItem)item;
                                    if (foundNodes.Contains(targetItem.Text))
                                        continue;
                                    target = targetItem.Text;
                                    foundNodes.Add(target);
                                    found = true;
                                }
                                if (item.Name == "NodeName" && item.GetType().ToString() == "Graph.Items.NodeLabelItem")
                                {
                                    NodeLabelItem targetItem = (NodeLabelItem)item;
                                    if (foundNodes.Contains(targetItem.Text))
                                        continue;
                                    target = targetItem.Text;
                                    foundNodes.Add(target);
                                    found = true;
                                }
                                if (found)
                                    continue;
                            }
                        }
                    }
                    if (Method != "Enter script method")
                        script += "     " + conditionText(Method) + Environment.NewLine;
                }
                m_log.WriteLine("Generated Conversation Node " + nameItem.Text);
                return script;
            }

            private String getEndNodeScript(String convName, Node node)
            {
                String script = "";
                String nodename = "";
                List<NodeItem> items = (List<NodeItem>)node.Items;
                foreach (NodeItem item in items)
                {
                    if (item.Name == "NodeName")
                    {
                        NodeLabelItem nameitem = item as NodeLabelItem;
                        nodename = nameitem.Text;
                    }
                }
                NodeTextBoxItem tb = (NodeTextBoxItem)items[0];
                script += "    " + conditionText(tb.Text) + Environment.NewLine;
                tb = (NodeTextBoxItem)items[2];
                if (tb.Text != "Conversation Exit Script")
                    script += "    " + conditionText(tb.Text) + ";\";" + Environment.NewLine;
                script += "    ## This ends the game" + Environment.NewLine + Environment.NewLine;
                script += "    return" + Environment.NewLine;
                m_log.WriteLine("Generated Conversation End Node" + nodename);
                return script;
            }

            // clean up single and double quotes in text.
            private String conditionText(String text)
            {
                String clean = text.Replace("\"", "\\\"");
                clean = clean.Replace("\'", "\\\'");

                return clean;
            }
        }
    }
}
