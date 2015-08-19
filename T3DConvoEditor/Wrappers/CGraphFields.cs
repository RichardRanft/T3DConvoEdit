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
                    String fName = nFields.name + ":" + (item.Name == null ? iFields.id : item.Name);
                    idFieldMap.Add(fName, iFields);
                    idItemMap.Add(item, fName);
                    iFields.Input = new List<CConnectionFields>();
                    iFields.Output = new List<CConnectionFields>();
                    iFields.ItemParts = new List<CItemPartFields>();
                    iFields.name = fName;
                    iFields.ParentNode = nFields.id;
                    if(item.Tag != null)
                        iFields.Tag = item.Tag.GetType().ToString();
                    iFields.IOMode = getIOMode(item);
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
                            Node nFrom = conn.From.Node;
                            String fromName = "";
                            foreach(NodeItem nItem in nFrom.Items)
                            {
                                if(nItem.Name == "NodeName")
                                {
                                    if(nItem.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                                    {
                                        NodeTextBoxItem i = nItem as NodeTextBoxItem;
                                        fromName = i.Text;
                                    }
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeLabelItem")
                                    {
                                        NodeLabelItem i = nItem as NodeLabelItem;
                                        fromName = i.Text;
                                    }
                                }
                            }
                            Node nTo = conn.To.Node;
                            String toName = "";
                            foreach (NodeItem nItem in nTo.Items)
                            {
                                if (nItem.Name == "NodeName")
                                {
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                                    {
                                        NodeTextBoxItem i = nItem as NodeTextBoxItem;
                                        toName = i.Text;
                                    }
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeLabelItem")
                                    {
                                        NodeLabelItem i = nItem as NodeLabelItem;
                                        toName = i.Text;
                                    }
                                }
                            }
                            if (conn.Tag != null)
                                cFields.Tag = conn.Tag.GetType().ToString();
                            else if (item.Tag != null)
                                cFields.Tag = item.Tag.GetType().ToString();
                            cFields.name = (conn.Name == null ? cFields.id : conn.Name);
                            cFields.From = fromName + ":" + conn.From.Item.Name;
                            cFields.To = toName + ":" + conn.To.Item.Name;
                            idFieldMap[iid].Input.Add(cFields);
                        }
                    }
                    if (item.Output.Enabled && item.Output.HasConnection)
                    {
                        foreach (NodeConnection conn in item.Output.Connectors)
                        {
                            CConnectionFields cFields = new CConnectionFields();
                            bool cfirst = false;
                            cFields.id = idGen.GetId(cFields, out cfirst).ToString();
                            Node nFrom = conn.From.Node;
                            String fromName = "";
                            foreach (NodeItem nItem in nFrom.Items)
                            {
                                if (nItem.Name == "NodeName")
                                {
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                                    {
                                        NodeTextBoxItem i = nItem as NodeTextBoxItem;
                                        fromName = i.Text;
                                    }
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeLabelItem")
                                    {
                                        NodeLabelItem i = nItem as NodeLabelItem;
                                        fromName = i.Text;
                                    }
                                }
                            }
                            Node nTo = conn.To.Node;
                            String toName = "";
                            foreach (NodeItem nItem in nTo.Items)
                            {
                                if (nItem.Name == "NodeName")
                                {
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                                    {
                                        NodeTextBoxItem i = nItem as NodeTextBoxItem;
                                        toName = i.Text;
                                    }
                                    if (nItem.GetType().ToString() == "Graph.Items.NodeLabelItem")
                                    {
                                        NodeLabelItem i = nItem as NodeLabelItem;
                                        toName = i.Text;
                                    }
                                }
                            }
                            if (conn.Tag != null)
                                cFields.Tag = conn.Tag.GetType().ToString();
                            else if (item.Tag != null)
                                cFields.Tag = item.Tag.GetType().ToString();
                            cFields.name = (conn.Name == null ? cFields.id : conn.Name);
                            cFields.From = fromName + ":" + conn.From.Item.Name;
                            cFields.To = toName + ":" + conn.To.Item.Name;
                            idFieldMap[iid].Input.Add(cFields);
                        }
                    }
                }
                Nodes.Add(nFields);
            }
        }

        private NodeIOMode getIOMode(NodeItem item)
        {
            bool input = item.Input.Enabled;
            bool output = item.Output.Enabled;
            if (input && output)
                return NodeIOMode.InOut;
            else if (input)
                return NodeIOMode.Input;
            else if (output)
                return NodeIOMode.Output;
            return NodeIOMode.None;
        }
    }
}
