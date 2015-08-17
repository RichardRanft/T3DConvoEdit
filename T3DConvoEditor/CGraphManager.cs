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

        public CGraphManager(CLog log)
        {
            m_log = log;
            m_idGen = new ObjectIDGenerator();
            m_serializer = new JsonSerializer();
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
                    m_graphFields = (CGraphFields)m_serializer.Deserialize(sr, typeof(CGraphFields));
                }
            }
            catch(Exception ex)
            {
                m_log.WriteLine("Could not load file " + filename + " : " + ex.Message + "\n" + ex.StackTrace);
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
                Node n = new Node(node.Title);
                foreach(CNodeItemFields item in node.Items)
                {
                    foreach (CConnectionFields conn in item.Input)
                        inputs.Add(conn.id, conn);
                    foreach (CConnectionFields conn in item.Output)
                        outputs.Add(conn.id, conn);
                    switch (item.ItemType)
                    {
                        case "Graph.Items.NodeCompositeItem":
                            {
                                CNodeCompositeItemFields tempFields = item as CNodeCompositeItemFields;
                                NodeCompositeItem temp = new NodeCompositeItem(tempFields.IOMode);
                                foreach(CItemPartFields part in tempFields.ItemParts)
                                {
                                    switch(part.PartType)
                                    {
                                        case "Graph.Items.ItemTextBoxPart":
                                            {
                                                CItemTextBoxPartFields tmpPrtFld = part as CItemTextBoxPartFields;
                                                ItemTextBoxPart p = new ItemTextBoxPart(tmpPrtFld.Text);
                                                temp.AddPart(p);
                                            }
                                            break;
                                        default:
                                            {

                                            }
                                            break;
                                    }
                                }
                                n.AddItem(temp);
                            }
                            break;
                        case "Graph.Items.NodeTextBoxItem":
                            {
                                CNodeTextBoxItemFields tempFields = item as CNodeTextBoxItemFields;
                                NodeTextBoxItem temp = new NodeTextBoxItem(tempFields.Text, tempFields.IOMode);
                                
                                n.AddItem(temp);
                            }
                            break;
                        case "Graph.Items.NodeLabelItem":
                            {
                                CNodeLabelItemFields tempFields = item as CNodeLabelItemFields;
                                NodeLabelItem temp = new NodeLabelItem(tempFields.Text, tempFields.IOMode);
                                
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
            throw new NotImplementedException();
        }
    }
}
