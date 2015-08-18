using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using System.Runtime.Serialization;

namespace T3DConvoEditor.Wrappers
{
    [Serializable]
    public class CGraphFields
    {
        public String id { get; set; }
        public String name { get; set; }
        public List<CNodeFields> Nodes { get; set; }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        public CGraphFields() { }

        public CGraphFields(GraphControl control, ObjectIDGenerator idGen)
        {
            Dictionary<String, CNodeItemFields> idFieldMap = new Dictionary<String, CNodeItemFields>();
            Dictionary<NodeItem, String> idItemMap = new Dictionary<NodeItem, String>();
            bool first = false;
            id = idGen.GetId(control, out first).ToString();
            name = control.Name;
            Nodes = new List<CNodeFields>();
            List<Node> nodes = (List<Node>)control.Nodes;
            foreach (Node node in nodes)
            {
                CNodeFields nFields = new CNodeFields();
                nFields.Title = node.Title;
                nFields.name = node.Title;
                nFields.Location = node.Location;
                nFields.Items = new List<CNodeItemFields>();
                bool nfirst = false;
                nFields.id = idGen.GetId(nFields, out nfirst).ToString();
                foreach(NodeItem item in node.Items)
                {
                    CNodeItemFields iFields = new CNodeItemFields();
                    bool ifirst = false;
                    iFields.id = idGen.GetId(iFields, out ifirst).ToString();
                    iFields.Input = new List<CConnectionFields>();
                    iFields.Output = new List<CConnectionFields>();
                    iFields.ItemParts = new List<CItemPartFields>();
                    iFields.name = item.Name;
                    iFields.ParentNode = nFields.id;
                    if(item.Tag != null)
                        iFields.Tag = item.Tag.GetType().ToString();
                    if(item.Input.Enabled && item.Output.Enabled)
                        iFields.IOMode = NodeIOMode.InOut;
                    if(item.Input.Enabled && !item.Output.Enabled)
                        iFields.IOMode = NodeIOMode.Input;
                    if(!item.Input.Enabled && item.Output.Enabled)
                        iFields.IOMode = NodeIOMode.Output;
                    if (!item.Input.Enabled && !item.Output.Enabled)
                        iFields.IOMode = NodeIOMode.None;
                    iFields.ItemType = item.GetType().ToString();
                    switch(iFields.ItemType)
                    {
                        case "Graph.Items.NodeTextBoxItem":
                            {
                                NodeTextBoxItem temp = item as NodeTextBoxItem;
                                iFields.Text = temp.Text;
                            }
                            break;
                        case "Graph.Items.NodeLabelItem":
                            {
                                NodeLabelItem temp = item as NodeLabelItem;
                                iFields.Text = temp.Text;
                            }
                            break;
                        case "Graph.Items.NodeCompositeItem":
                            {
                                NodeCompositeItem temp = item as NodeCompositeItem;
                                foreach(ItemPart part in temp.Parts)
                                {
                                    CItemPartFields pFields = new CItemPartFields();
                                    bool pfirst = false;
                                    pFields.id = idGen.GetId(pFields, out pfirst).ToString();
                                    pFields.name = part.Name;
                                    pFields.PartType = part.GetType().ToString();
                                    switch(pFields.PartType)
                                    {
                                        case "Graph.Items.ItemTextBoxPart":
                                            {
                                                ItemTextBoxPart tempPart = part as ItemTextBoxPart;
                                                pFields.Text = tempPart.Text;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                    iFields.ItemParts.Add(pFields);
                                }
                            }
                            break;
                    }
                    nFields.Items.Add(iFields);
                }
                foreach(NodeItem item in node.Items)
                {
                    // now get connection info
                    String iid = idItemMap[item];
                    if (item.Input.Enabled && item.Input.HasConnection)
                    {
                        foreach (NodeConnection conn in item.Input.Connectors)
                        {
                            CConnectionFields cFields = new CConnectionFields();
                            bool cfirst = false;
                            cFields.id = idGen.GetId(cFields, out cfirst).ToString();
                            cFields.From = conn.From.Item.Name;
                            cFields.To = conn.To.Item.Name;
                            idFieldMap[iid].Input.Add(new CConnectionFields());
                        }
                    }
                    if (item.Output.Enabled && item.Output.HasConnection)
                    {
                        foreach (NodeConnection conn in item.Output.Connectors)
                        {
                            CConnectionFields cFields = new CConnectionFields();
                            bool cfirst = false;
                            cFields.id = idGen.GetId(cFields, out cfirst).ToString();
                            cFields.From = conn.From.Item.Name;
                            cFields.To = conn.To.Item.Name;
                            idFieldMap[iid].Input.Add(new CConnectionFields());
                        }
                    }
                }
                Nodes.Add(nFields);
            }
        }
    }
}
