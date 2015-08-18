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
using T3DConvoEditor.Wrappers;
using System.Runtime.Serialization;

namespace T3DConvoEditor
{
    public class CGraphManager
    {
        private CLog m_log;
        private JsonSerializer m_serializer;
        private ObjectIDGenerator m_idGen;
        private CGraphFields m_graphFields;
        private Dictionary<String, NodeItem> m_itemMap;
        private Dictionary<String, String> m_idNameMap;

        public CGraphManager(CLog log)
        {
            m_log = log;
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
            graph.Controls.Clear();
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
                        name = item.name;
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
                                                temp.AddPart(p);
                                            }
                                            break;
                                        default:
                                            {
                                            }
                                            break;
                                    }
                                }
                                m_itemMap.Add(item.id, temp);
                                m_idNameMap.Add(item.id, name);
                                n.AddItem(temp);
                            }
                            break;
                        case "Graph.Items.NodeTextBoxItem":
                            {
                                NodeTextBoxItem temp = new NodeTextBoxItem(item.Text, item.IOMode);
                                temp.Name = name;
                                m_itemMap.Add(item.id, temp);
                                m_idNameMap.Add(item.id, name);
                                n.AddItem(temp);
                            }
                            break;
                        case "Graph.Items.NodeLabelItem":
                            {
                                NodeLabelItem temp = new NodeLabelItem(item.Text, item.IOMode);
                                temp.Name = name;
                                m_itemMap.Add(item.id, temp);
                                m_idNameMap.Add(item.id, name);
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
            rebuildConnections(inputs, outputs);
        }

        private void rebuildConnections(Dictionary<string, CConnectionFields> inputs, Dictionary<string, CConnectionFields> outputs)
        {
            //throw new NotImplementedException();
        }
    }
}
