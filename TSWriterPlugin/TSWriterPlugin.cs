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
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Graph;
using Graph.Items;
using PluginContracts;
using BasicLogging;
using BasicSettings;

namespace TSWriterPlugin
{
    public class CTSWriterPlugin : IPlugin
    {
        private GraphControl m_graphCtrl;
        private CLog m_log;
        private CSettings m_settings;
        private String m_iniPath;
        private FNodeEdit m_nodeEdit;
        private FConvPartEdit m_partEdit;
        private FSettings m_settingsForm;
        private List<String> m_nodeTypeNames;

        public String Name
        {
            get
            {
                return "TSWriterPlugin";
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

        public MouseEventHandler GetBtnHandler(Form parent, string type)
        {
            return new MouseEventHandler((sender, ea) => BtnHandler(sender, ea, parent, type));
        }

        public void BtnHandler(object sender, MouseEventArgs e, Form parent, string type)
        {
            switch (type.ToLower())
            {
                case "start":
                    {
                        var node = GetNodeByTypename(type.ToLower(), ""); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
                case "conversation":
                    {
                        var node = GetNodeByTypename(type.ToLower(), ""); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
                default:
                    {
                        var node = GetNodeByTypename(type.ToLower(), ""); //new Node("Conversation Start");
                        parent.DoDragDrop(node, DragDropEffects.Copy);
                        break;
                    }
            }
        }

        public Node GetNodeByTypename(string typename, string nodename)
        {
            switch(typename.ToLower())
            {
                case "start":
                    {
                        var node = new Node("Conversation Start");
                        var startLabel = new NodeLabelItem("Conversation_Start", NodeIOMode.Output) { Tag = TagType.LABEL };
                        startLabel.Name = "NodeName";
                        node.AddItem(startLabel);
                        return node;
                    }
                case "end":
                    {
                        var node = new Node("Conversation End");
                        node.AddItem(new NodeTextBoxItem("Enter text"));
                        var endLabel = new NodeLabelItem(nodename, NodeIOMode.Input) { Tag = TagType.TEXTBOX };
                        endLabel.Name = "NodeName";
                        node.AddItem(endLabel);
                        node.AddItem(new NodeTextBoxItem(m_settings.Attributes["[Default]"]["DEFAULTEXITMETHOD"]));
                        return node;
                    }
                case "conversation":
                    {
                        List<Node> nodes = (List<Node>)m_graphCtrl.Nodes;
                        String nodeName = m_settings.Attributes["[Default]"]["DEFAULTNODENAME"] + "_" + getConvNodeCount().ToString().PadLeft(4, '0');
                        var node = new Node("Conversation Node");
                        var nodeNameItem = new NodeTextBoxItem(nodename);
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
                        return node;
                    }
            }
            return null;
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
            return new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
        }

        public EventHandler<NodeItemEventArgs> GetConvMouseHandler(string type = "")
        {
            return new EventHandler<NodeItemEventArgs>(editConvNode_MouseDown);
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

        private void editConvNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if (e.Item != null)
            {
                m_partEdit.Node = e.Item as NodeCompositeItem;
                m_partEdit.ShowDialog();
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
            m_graphCtrl = ctrl;
            m_log = log;
            String homeFolder = @Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            m_iniPath = homeFolder + @"\Roostertail Games\T3DConvoEditor\";
            String iniFile = m_iniPath + "TSWriterPlugin.ini";
            m_log.WriteLine("Attempting to load " + iniFile);
            if (File.Exists(iniFile))
                m_settings = new CSettings(m_iniPath + "TSWriterPlugin.ini");
            else
            {
                m_iniPath = Path.GetFullPath(".\\");
                iniFile = m_iniPath + @"Plugins\TSWriterPlugin.ini";
                m_log.WriteLine("Attempting to load " + iniFile);
                m_settings = new CSettings(iniFile);
            }
            if (!m_settings.LoadSettings())
                m_log.WriteLine("Failed to locate TSWriterPlugin.ini");
            else
                m_log.WriteLine("TSWriterPlugin settings loaded");
            m_nodeEdit = new FNodeEdit();
            m_partEdit = new FConvPartEdit();
            m_settingsForm = new FSettings();
        }

        public void Export(String filename)
        {
            if (m_graphCtrl == null)
                MessageBox.Show("TorqueScript Writer Plugin can't export graph - call SetGraphControl() to set the graph for export.");
            CTorquescriptWriter writer = new CTorquescriptWriter(m_log, m_settings);
            if (filename.Length < 1)
                filename = "defaultScript.cs";
            writer.WriteScript(filename, (List<Node>)m_graphCtrl.Nodes);
        }

        public System.String GetDefaultExtension()
        {
            return m_settings.Attributes["[Default]"]["DEFAULTEXT"];
        }

        public bool Validate(GraphControl graph)
        {
            List<Node> nodes = (List<Node>)graph.Nodes;
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
            foreach (Node node in nodes)
            {
                foreach (NodeItem item in node.Items)
                {
                    if (item.Name == "NodeName")
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
                        if (names.Contains(name))
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

            foreach (Node node in nodes)
            {
                if (node.Title.Equals("Conversation Node"))
                {
                    convoNodeFound = true;
                    break;
                }
            }

            return convoNodeFound;
        }

        private bool checkConnections(List<Node> nodelist)
        {
            foreach (Node n in nodelist)
            {
                if (n.HasNoItems)
                    continue;
                if (n.AnyConnectorsDisconnected)
                    return false;
            }
            return true;
        }

        class CTorquescriptWriter
        {
            private CLog m_log;
            private CSettings m_settings;

            public CTorquescriptWriter(CLog log, CSettings settings)
            {
                m_log = log;
                m_settings = settings;
            }

            public bool WriteScript(String filename, List<Node> nodelist)
            {
                try
                {
                    String conversation = Path.GetFileName(filename).Replace(".cs", "");
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
                m_log.WriteLine("Generating conversation script " + convoName + "...");

                String script = "";

                script += "// Conversation output generated using T3DConvoEditor" + Environment.NewLine;
                script += "// Copyright © 2015 Roostertail Games" + Environment.NewLine;
                script += "// Use of T3DConvoEditor and its output are governed by the MIT license." + Environment.NewLine + Environment.NewLine;

                script += "// Permission is hereby granted, free of charge, to any person obtaining a copy" + Environment.NewLine;
                script += "// of this software and associated documentation files (the \"Software\"), to deal" + Environment.NewLine;
                script += "// in the Software without restriction, including without limitation the rights" + Environment.NewLine;
                script += "// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell" + Environment.NewLine;
                script += "// copies of the Software, and to permit persons to whom the Software is" + Environment.NewLine;
                script += "// furnished to do so, subject to the following conditions:" + Environment.NewLine + Environment.NewLine;

                script += "// The above copyright notice and this permission notice shall be included in" + Environment.NewLine;
                script += "// all copies or substantial portions of the Software." + Environment.NewLine + Environment.NewLine;

                script += "// THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR" + Environment.NewLine;
                script += "// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY," + Environment.NewLine;
                script += "// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE" + Environment.NewLine;
                script += "// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER" + Environment.NewLine;
                script += "// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM," + Environment.NewLine;
                script += "// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN" + Environment.NewLine;
                script += "// THE SOFTWARE." + Environment.NewLine + Environment.NewLine;

                script += "//--- OBJECT WRITE BEGIN ---" + Environment.NewLine;
                script += "new SimSet(" + convoName + "){" + Environment.NewLine;
                script += "\tclass = \"Conversation\";" + Environment.NewLine;
                script += "\tcanSave = \"1\";" + Environment.NewLine;
                script += "\tcanSaveDynamicFields = \"1\";" + Environment.NewLine + Environment.NewLine;

                foreach (Node node in nodes)
                {
                    List<NodeItem> items = (List<NodeItem>)node.Items;
                    String title = items[0].Node.Title;
                    switch (title)
                    {
                        case "Conversation Start":
                            script += getStartNodeScript(convoName, node);
                            break;
                        case "Conversation Node":
                            script += getConvoNodeScript(convoName, node);
                            break;
                        case "Conversation End":
                            script += getEndNodeScript(convoName, node);
                            break;
                    }
                }
                script += "};" + Environment.NewLine;
                script += "//--- OBJECT WRITE END ---" + Environment.NewLine;

                m_log.WriteLine(convoName + " script generated.");

                return script;
            }

            private String getStartNodeScript(String convName, Node node)
            {
                String script = "";
                List<NodeItem> items = (List<NodeItem>)node.Items;
                NodeLabelItem linkItem = (NodeLabelItem)items[0];
                script += "\tnew ScriptObject(" + convName + "_" + linkItem.Text + ") {" + Environment.NewLine;
                script += "\t\tclass = \"ConversationStart\";" + Environment.NewLine;
                script += "\t\tcanSave = \"1\";" + Environment.NewLine;
                script += "\t\tcanSaveDynamicFields = \"1\";" + Environment.NewLine;
                script += "\t\t\tnumOutLinks = 1;" + Environment.NewLine;
                List<NodeConnection> conns = (List<NodeConnection>)linkItem.Node.Connections;
                NodeConnection outconn = conns[0];
                List<NodeItem> targetItemList = (List<NodeItem>)outconn.To.Node.Items;
                NodeTextBoxItem targetItem = (NodeTextBoxItem)targetItemList[0];
                script += "\t\t\toutLink0 = " + convName + "_" + targetItem.Text + ";" + Environment.NewLine;
                script += "\t};" + Environment.NewLine;
                m_log.WriteLine("Generated Conversation Start node");
                return script;
            }

            private String getConvoNodeScript(String convName, Node node)
            {
                String script = "";
                List<NodeItem> items = (List<NodeItem>)node.Items;
                NodeTextBoxItem nameItem = (NodeTextBoxItem)items[0];
                script += "\tnew ScriptObject(" + convName + "_" + nameItem.Text + ") {" + Environment.NewLine;
                script += "\t\tclass = \"ConversationNode\";" + Environment.NewLine;
                script += "\t\tcanSave = \"1\";" + Environment.NewLine;
                script += "\t\tcanSaveDynamicFields = \"1\";" + Environment.NewLine;
                int outNodeCount = items.Count - int.Parse(m_settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
                int start = int.Parse(m_settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
                NodeTextBoxItem nodeText = (NodeTextBoxItem)items[1];
                script += "\t\t\tdisplayText = \"" + conditionText(nodeText.Text) + "\";" + Environment.NewLine;
                script += "\t\t\tnumOutLinks = " + outNodeCount + ";" + Environment.NewLine;

                String target = "";
                List<String> foundNodes = new List<String>();
                for (int i = start; i < items.Count; i++)
                {
                    NodeCompositeItem textItem = (NodeCompositeItem)items[i];
                    String Text = "";
                    String Method = "";
                    foreach(ItemTextBoxPart part in textItem.Parts)
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
                    script += "\t\t\tbutton" + (i - start).ToString() + "next = " + convName + "_" + target + ";" + Environment.NewLine;
                    script += "\t\t\tbutton" + (i - start).ToString() + " = \"" + conditionText(Text) + "\";" + Environment.NewLine;
                    if (Method != "Enter script method")
                        script += "\t\t\tbutton" + (i - start).ToString() + "cmd = \"" + conditionText(Method) + ";\";" + Environment.NewLine;
                }
                script += "\t};" + Environment.NewLine;
                m_log.WriteLine("Generated Conversation Node " + nameItem.Text);
                return script;
            }

            private String getEndNodeScript(String convName, Node node)
            {
                String script = "";
                String nodename = "";
                List<NodeItem> items = (List<NodeItem>)node.Items;
                foreach(NodeItem item in items)
                {
                    if(item.Name == "NodeName")
                    {
                        NodeLabelItem nameitem = item as NodeLabelItem;
                        nodename = nameitem.Text;
                    }
                }
                script += "\tnew ScriptObject(" + convName + "_" + nodename + ") {" + Environment.NewLine;
                script += "\t\tclass = \"ConversationEnd\";" + Environment.NewLine;
                script += "\t\tcanSave = \"1\";" + Environment.NewLine;
                script += "\t\tcanSaveDynamicFields = \"1\";" + Environment.NewLine;
                NodeTextBoxItem tb = (NodeTextBoxItem)items[0];
                script += "\t\t\tdisplayText = \"" + conditionText(tb.Text) + "\";" + Environment.NewLine;
                tb = (NodeTextBoxItem)items[2];
                if (tb.Text != "Conversation Exit Script")
                    script += "\t\t\tscriptMethod = \"" + conditionText(tb.Text) + ";\";" + Environment.NewLine;
                script += "\t};" + Environment.NewLine;
                m_log.WriteLine("Generated Conversation End Node" + nodename);
                return script;
            }

            private String conditionText(String text)
            {
                String clean = text.Replace("\"", "\\\"");
                clean = clean.Replace("\'", "\\\'");

                return clean;
            }
        }
    }
}
