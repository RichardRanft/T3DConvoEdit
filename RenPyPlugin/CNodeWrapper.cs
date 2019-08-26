using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using BasicSettings;
using log4net;

namespace RenPyPlugin
{
    class CNodeWrapper
    {
        private static ILog m_log = LogManager.GetLogger(typeof(CNodeWrapper));

        public CSettings Settings;
        public List<CNodeWrapper> Next;

        private Node m_node;

        public CNodeWrapper(Node node)
        {
            Next = new List<CNodeWrapper>();
            m_node = node;
        }

        public void Write(ref String script)
        {
            script += getScript();
            if (Next.Count > 1)
            {
                Queue<CNodeWrapper> branchQueue = new Queue<CNodeWrapper>();
                foreach (CNodeWrapper n in Next)
                    branchQueue.Enqueue(n);
                while (branchQueue.Count > 0)
                {
                    CNodeWrapper next = branchQueue.Dequeue();
                    Write(ref script);
                }
            }
            else if (Next.Count == 1)
                Write(ref script);
        }

        private String getScript()
        {
            String scriptText = "";
            List<NodeItem> items = (List<NodeItem>)m_node.Items;
            String title = items[0].Node.Title;
            switch (title)
            {
                case "Conversation Start":
                    scriptText = getStartNodeText(m_node);
                    break;
                case "Conversation Node":
                    scriptText = getConvNodeText(m_node);
                    break;
                case "Menu Node":
                    scriptText = getMenuNodeText(m_node);
                    break;
                case "Conditional Node":
                    scriptText = getCondNodeText(m_node);
                    break;
                case "Conversation End":
                    scriptText = getEndNodeText(m_node);
                    break;
            }
            return scriptText;
        }

        private string getEndNodeText(Node m_node)
        {
            String script = "";
            String nodename = "";
            List<NodeItem> items = (List<NodeItem>)m_node.Items;
            foreach (NodeItem item in items)
            {
                if (item.Name == "NodeName")
                {
                    NodeLabelItem nameitem = item as NodeLabelItem;
                    nodename = nameitem.Text;
                }
            }
            NodeTextBoxItem tb = (NodeTextBoxItem)items[2];
            if (!String.IsNullOrEmpty(tb.Text) && tb.Text != "Conversation Exit Script")
            {
                String[] parts = conditionText(tb.Text).Split('\n');
                foreach(String str in parts)
                    script += "\t\"" + str.Trim() + "\"" + Environment.NewLine;
            }
            script += "\treturn" + Environment.NewLine;
            return script;
        }

        private string getCondNodeText(Node m_node)
        {
            throw new NotImplementedException();
        }

        private string getMenuNodeText(Node m_node)
        {
            throw new NotImplementedException();
        }

        private string getConvNodeText(Node m_node)
        {
            String script = "";
            List<NodeItem> items = (List<NodeItem>)m_node.Items;
            NodeTextBoxItem nameItem = (NodeTextBoxItem)items[0];
            int outNodeCount = items.Count - int.Parse(Settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
            int start = int.Parse(Settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
            NodeTextBoxItem nodeText = (NodeTextBoxItem)items[1];
            script += "\t" + conditionText(nodeText.Text) + "\"" + Environment.NewLine;

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
            }
            return script;
        }

        private string getStartNodeText(Node m_node)
        {
            String script = "";
            script += "label start:" + Environment.NewLine;
            return script;
        }

        public void GenerateTree()
        {
            List<NodeItem> items = (List<NodeItem>)m_node.Items;
            String title = items[0].Node.Title;
            switch (title)
            {
                case "Conversation Start":
                    getNextNodes(m_node);
                    break;
                case "Conversation Node":
                    getNextNodes(m_node);
                    break;
                case "Menu Node":
                    getNextNodes(m_node);
                    break;
                case "Conditional Node":
                    getNextNodes(m_node);
                    break;
                case "Conversation End":
                    break;
            }
        }

        private void getNextNodes(Node node)
        {
            List<NodeItem> items = (List<NodeItem>)node.Items;
            NodeLabelItem linkItem = (NodeLabelItem)items[0];
            List<NodeConnection> conns = (List<NodeConnection>)linkItem.Node.Connections;
            NodeConnection outconn = conns[0];
            List<NodeItem> linkItems = getConnections(linkItem.Node, (NodeOutputConnector)linkItem.Output);
            m_log.Info("Generated Node Links for " + node.Title);
        }

        private void getStartNodeWrapper(Node node)
        {
            List<NodeItem> items = (List<NodeItem>)node.Items;
            NodeLabelItem linkItem = (NodeLabelItem)items[0];
            List<NodeConnection> conns = (List<NodeConnection>)linkItem.Node.Connections;
            NodeConnection outconn = conns[0];
            List<NodeItem> linkItems = getConnections(linkItem.Node, (NodeOutputConnector)linkItem.Output);
            m_log.Info("Generated Conversation Start Node");
        }

        private String getConvoNodeWrapper(String convName, Node node)
        {
            String script = "";
            List<NodeItem> items = (List<NodeItem>)node.Items;
            NodeTextBoxItem nameItem = (NodeTextBoxItem)items[0];
            int outNodeCount = items.Count - int.Parse(Settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
            int start = int.Parse(Settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
            NodeTextBoxItem nodeText = (NodeTextBoxItem)items[1];

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
                    script += "\t\t\tbutton" + (i - start).ToString() + "cmd = \"" + conditionText(Method) + ";\";" + Environment.NewLine;
            }
            m_log.Info("Generated Conversation Node " + nameItem.Text);
            return script;
        }

        private String getMenuNodeWrapper(String convName, Node node)
        {
            String script = "";
            List<NodeItem> items = (List<NodeItem>)node.Items;
            NodeLabelItem linkItem = (NodeLabelItem)items[0];
            List<NodeConnection> conns = (List<NodeConnection>)linkItem.Node.Connections;
            NodeConnection outconn = conns[0];
            List<NodeItem> targetItemList = (List<NodeItem>)outconn.To.Node.Items;
            NodeTextBoxItem targetItem = (NodeTextBoxItem)targetItemList[0];
            m_log.Info("Generated Menu Node");
            return script;
        }

        private String getConditionNodeWrapper(String convName, Node node)
        {
            String script = "";
            List<NodeItem> items = (List<NodeItem>)node.Items;
            NodeLabelItem linkItem = (NodeLabelItem)items[0];
            List<NodeConnection> conns = (List<NodeConnection>)linkItem.Node.Connections;
            NodeConnection outconn = conns[0];
            List<NodeItem> targetItemList = (List<NodeItem>)outconn.To.Node.Items;
            NodeTextBoxItem targetItem = (NodeTextBoxItem)targetItemList[0];
            m_log.Info("Generated Condition Node");
            return script;
        }

        private String getEndNodeWrapper(String convName, Node node)
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
            tb = (NodeTextBoxItem)items[2];
            if (tb.Text != "Conversation Exit Script")
                script += "\t\t\tscriptMethod = \"" + conditionText(tb.Text) + ";\";" + Environment.NewLine;
            m_log.Info("Generated Conversation End Node" + nodename);
            return script;
        }

        private List<NodeItem> getConnections(Node outnode, NodeOutputConnector output)
        {
            List<NodeItem> foundNodes = new List<NodeItem>();
            foreach (NodeConnection con in output.Connectors)
            {
                if (con.To.Node == outnode)
                    continue;
                foreach (NodeConnection targetCon in con.To.Node.Connections)
                {
                    if (targetCon.From.Node != outnode)
                        continue;
                    foreach (NodeItem item in con.To.Node.Items)
                    {
                        if (foundNodes.Contains(item))
                            continue;
                        foundNodes.Add(item);
                    }
                }
            }
            return foundNodes;
        }

        private String conditionText(String text)
        {
            String clean = text.Replace("\"", "\\\"");
            clean = clean.Replace("\'", "\\\'");

            return clean;
        }
    }
}
