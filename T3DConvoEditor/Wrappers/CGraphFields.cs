﻿using System;
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
                    switch(item.GetType().ToString())
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
                Nodes.Add(nFields);
            }
        }
    }
}
