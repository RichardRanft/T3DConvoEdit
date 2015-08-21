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

namespace TSWriterPlugin
{
    public class CTSWriterPlugin : IPlugin
    {
        private GraphControl m_graphCtrl;
        private CLog m_log;
        private CSettings m_settings;

        public String Name
        {
            get
            {
                return "TSWriterPlugin";
            }
        }

        public void Initialize(GraphControl ctrl, CLog log)
        {
            m_graphCtrl = ctrl;
            m_log = log;
            m_settings = new CSettings("plugins\\TSWriterPlugin.ini");
            m_settings.LoadSettings();
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
                        script += "\t\t\tbutton" + (i - start).ToString() + "cmd = \"" + Method + "\";" + Environment.NewLine;
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
                script += "\t\t\tscriptMethod = \"" + tb.Text + "\";" + Environment.NewLine;
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
