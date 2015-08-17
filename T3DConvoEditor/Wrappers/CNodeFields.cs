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
    public class CNodeFields
    {
        public String id { get; set; }
        public String name { get; set; }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        public String Title { get; set; }
        public PointF Location { get; set; }
        public List<CNodeItemFields> Items { get; set; }

        public CNodeFields(Node node, ObjectIDGenerator idGen)
        {
            bool first = false;
            id = idGen.GetId(node, out first).ToString();
            Title = node.Title;
            name = node.Title;
            Location = node.Location;
            Items = new List<CNodeItemFields>();
            foreach (NodeItem item in node.Items)
            {
                String type = item.GetType().ToString();
                switch (type)
                {
                    case "Graph.Items.NodeCompositeItem":
                        {
                            NodeCompositeItem tempItem = item as NodeCompositeItem;
                            CNodeCompositeItemFields itemFields = new CNodeCompositeItemFields(tempItem, idGen);
                            Items.Add(itemFields);
                            if (item.Input != null && item.Input.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Input.Connectors)
                                    itemFields.Input.Add(new CConnectionFields(conn, idGen));
                            }
                            if (item.Output != null && item.Output.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Output.Connectors)
                                    itemFields.Output.Add(new CConnectionFields(conn, idGen));
                            }
                        }
                        break;
                    case "Graph.Items.NodeTextBoxItem":
                        {
                            NodeTextBoxItem tempItem = item as NodeTextBoxItem;
                            CNodeTextBoxItemFields itemFields = new CNodeTextBoxItemFields(tempItem, idGen);
                            Items.Add(itemFields);
                            if (item.Input != null && item.Input.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Input.Connectors)
                                    itemFields.Input.Add(new CConnectionFields(conn, idGen));
                            }
                            if (item.Output != null && item.Output.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Output.Connectors)
                                    itemFields.Output.Add(new CConnectionFields(conn, idGen));
                            }
                        }
                        break;
                    case "Graph.Items.NodeLabelItem":
                        {
                            NodeLabelItem tempItem = item as NodeLabelItem;
                            CNodeLabelItemFields itemFields = new CNodeLabelItemFields(tempItem, idGen);
                            Items.Add(itemFields);
                            if (item.Input != null && item.Input.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Input.Connectors)
                                    itemFields.Input.Add(new CConnectionFields(conn, idGen));
                            }
                            if (item.Output != null && item.Output.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Output.Connectors)
                                    itemFields.Output.Add(new CConnectionFields(conn, idGen));
                            }
                        }
                        break;
                    default:
                        {
                            CNodeItemFields itemFields = new CNodeItemFields(item, idGen);
                            Items.Add(itemFields);
                            if (item.Input != null && item.Input.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Input.Connectors)
                                    itemFields.Input.Add(new CConnectionFields(conn, idGen));
                            }
                            if (item.Output != null && item.Output.Connectors != null)
                            {
                                foreach (NodeConnection conn in item.Output.Connectors)
                                    itemFields.Output.Add(new CConnectionFields(conn, idGen));
                            }
                        }
                        break;
                }
            }
        }
    }
}
