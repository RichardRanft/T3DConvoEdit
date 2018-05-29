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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using Newtonsoft.Json;
using BasicLogging;
using ConvoEditor.Wrappers;
using System.Runtime.Serialization;

namespace ConvoEditor
{
    public class CGraphManager
    {
        private CLog m_log;
        private JsonSerializer m_serializer;
        private ObjectIDGenerator m_idGen;
        private CGraphFields m_graphFields;
        private Form1 m_parentForm;
        private Dictionary<String, NodeItem> m_itemMap;
        private Dictionary<String, String> m_idNameMap;

        public CGraphManager(Form1 parent, CLog log)
        {
            m_log = log;
            m_parentForm = parent;
            m_idGen = new ObjectIDGenerator();
            m_serializer = new JsonSerializer();
            m_itemMap = new Dictionary<String, NodeItem>();
            m_idNameMap = new Dictionary<String, String>();
        }

        public void SaveGraph(GraphControl graph, String filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    writeGraph(graph, sw);
                }
            }
            catch (Exception ex)
            {
                m_log.WriteLine("Could not save file " + filename + " : " + ex.Message + "\n" + ex.StackTrace);
            }
            m_log.WriteLine("Saved " + filename);
        }

        private void writeGraph(GraphControl graph, StreamWriter stream)
        {
            CGraphFields graphFields = new CGraphFields(graph, m_idGen);
            m_serializer.Serialize(stream, graphFields);
        }

        public void LoadGraph(GraphControl graph, String filename)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    var tempFields = (CGraphFields)m_serializer.Deserialize(sr, typeof(CGraphFields));
                    m_graphFields = tempFields;
                }
            }
            catch(Exception ex)
            {
                m_log.WriteLine("Could not load file " + filename + " : " + ex.Message + "\n" + ex.StackTrace);
                return;
            }
            rebuildGraph(graph);
            m_log.WriteLine("Loaded " + filename);
        }

        private void rebuildGraph(GraphControl graph)
        {
            Dictionary<String, CConnectionFields> inputs = new Dictionary<String, CConnectionFields>();
            Dictionary<String, CConnectionFields> outputs = new Dictionary<String, CConnectionFields>();
            foreach(CNodeFields node in m_graphFields.Nodes)
            {
                m_idNameMap.Add(node.id, node.name);
                Node n = new Node(node.Title);
                n.Location = node.Location;
                foreach(CNodeItemFields item in node.Items)
                {
                    String name = "";
                    if(item.name != null)
                        name = item.name.Split(':')[1];
                    if (item.Input != null)
                    {
                        foreach (CConnectionFields conn in item.Input)
                        {
                            if (!inputs.ContainsKey(conn.id))
                                inputs.Add(conn.id, conn);
                        }
                    }
                    if (item.Output != null)
                    {
                        foreach (CConnectionFields conn in item.Output)
                        {
                            if (!outputs.ContainsKey(conn.id))
                                outputs.Add(conn.id, conn);
                        }
                    }
                    switch (item.ItemType)
                    {
                        case "Graph.Items.NodeCompositeItem":
                            {
                                NodeCompositeItem temp = new NodeCompositeItem(item.IOMode);
                                temp.Name = name;
                                foreach(CItemPartFields part in item.ItemParts)
                                {
                                    switch(part.PartType)
                                    {
                                        case "Graph.Items.ItemTextBoxPart":
                                            {
                                                ItemTextBoxPart p = new ItemTextBoxPart(part.Text);
                                                p.Name = part.name;
                                                temp.AddPart(p);
                                            }
                                            break;
                                        default:
                                            {
                                            }
                                            break;
                                    }
                                }
                                if (item.Tag != null)
                                    temp.Tag = PluginContracts.TagFactory.GetTagObject(item.Tag);
                                if (temp.Name.Contains("button_"))
                                    temp.Clicked += m_parentForm.GetConvMouseHandler();
                                m_itemMap.Add(item.id, temp);
                                m_idNameMap.Add(item.id, name);
                                n.AddItem(temp);
                            }
                            break;
                        case "Graph.Items.NodeTextBoxItem":
                            {
                                NodeTextBoxItem temp = new NodeTextBoxItem(item.Text, item.IOMode);
                                temp.Name = name;
                                if (item.Tag != null)
                                    temp.Tag = PluginContracts.TagFactory.GetTagObject(item.Tag);
                                m_itemMap.Add(item.id, temp);
                                m_idNameMap.Add(item.id, name);
                                n.AddItem(temp);
                            }
                            break;
                        case "Graph.Items.NodeLabelItem":
                            {
                                NodeLabelItem temp = new NodeLabelItem(item.Text, item.IOMode);
                                temp.Name = name;
                                if (item.Tag != null)
                                    temp.Tag = PluginContracts.TagFactory.GetTagObject(item.Tag);
                                m_itemMap.Add(item.id, temp);
                                m_idNameMap.Add(item.id, name);
                                if (temp.Name == "EditNodeItem")
                                    temp.Clicked += m_parentForm.GetEditMouseHandler();
                                n.AddItem(temp);
                            }
                            break;
                        default:
                            {
                                //NodeItem temp = new NodeItem(item.IOMode);
                            }
                            break;
                    }
                }
                graph.AddNode(n);
            }
            rebuildConnections(graph, inputs, outputs);
        }

        private void rebuildConnections(GraphControl graph, Dictionary<string, CConnectionFields> inputs, Dictionary<string, CConnectionFields> outputs)
        {
            foreach(String key in inputs.Keys)
            {
                CConnectionFields inputFields = inputs[key];
                NodeConnection conn = new NodeConnection();
                conn.Tag = PluginContracts.TagFactory.GetTagObject(inputFields.Tag);
                conn.Name = inputFields.name;
                String[] fromNameParts = inputFields.From.Split(':');
                String fromNodeName = fromNameParts[0];
                String fromItemName = fromNameParts[1];
                String[] toNameParts = inputFields.To.Split(':');
                String toNodeName = toNameParts[0];
                String toItemName = toNameParts[1];
                Node fromNode = findNode(graph, fromNodeName);
                NodeItem fromItem = findItem(fromNode, fromItemName);
                Node toNode = findNode(graph, toNodeName);
                NodeItem toItem = findItem(toNode, toItemName);
                graph.Connect(fromItem.Output, toItem.Input);
                conn.FromItem = fromItemName;
                conn.ToItem = toItemName;
            }
            foreach (String key in outputs.Keys)
            {
                CConnectionFields outputFields = outputs[key];
                NodeConnection conn = new NodeConnection();
                conn.Tag = PluginContracts.TagFactory.GetTagObject(outputFields.Tag);
                conn.Name = outputFields.name;
                String[] fromNameParts = outputFields.From.Split(':');
                String fromNodeName = fromNameParts[0];
                String fromItemName = fromNameParts[1];
                String[] toNameParts = outputFields.To.Split(':');
                String toNodeName = toNameParts[0];
                String toItemName = toNameParts[1];
                Node fromNode = findNode(graph, fromNodeName);
                NodeItem fromItem = findItem(fromNode, fromItemName);
                Node toNode = findNode(graph, toNodeName);
                NodeItem toItem = findItem(toNode, toItemName);
                graph.Connect(fromItem.Output, toItem.Input);
                conn.FromItem = fromItemName;
                conn.ToItem = toItemName;
            }
            graph.Refresh();
        }

        private Node findNode(GraphControl ctrl, String name)
        {
            foreach(Node n in ctrl.Nodes)
            {
                foreach (NodeItem i in n.Items)
                {
                    if (i.Name == "NodeName")
                    {
                        if(i.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                        {
                            NodeTextBoxItem item = i as NodeTextBoxItem;
                            if (item.Text == name)
                                return n;
                        }
                        if(i.GetType().ToString() == "Graph.Items.NodeLabelItem")
                        {
                            NodeLabelItem item = i as NodeLabelItem;
                            if (item.Text == name)
                                return n;
                        }
                    }
                }
            }
            return null;
        }

        private NodeItem findItem(Node node, String name)
        {
            foreach(NodeItem i in node.Items)
            {
                if (i.Name == name)
                    return i;
            }
            return null;
        }
    }
}
