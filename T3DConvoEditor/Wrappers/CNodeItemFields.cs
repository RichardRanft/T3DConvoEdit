using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using System.Runtime.Serialization;

namespace T3DConvoEditor.Wrappers
{
    [Serializable]
    public class CNodeItemFields : CItemFields
    {
        public List<CConnectionFields> Input { get; set; }
        public List<CConnectionFields> Output { get; set; }
        public String ItemType { get; set; }
        public String ParentNode { get; set; }

        public CNodeItemFields(NodeItem nodeItem, ObjectIDGenerator idGen) : base(nodeItem, idGen)
        {
            bool first = false;
            id = idGen.GetId(nodeItem, out first).ToString();
            name = nodeItem.Name;
            foreach(NodeItem item in nodeItem.Node.Items)
            {
                if (item.Name == "NodeName")
                {
                    if(item.GetType().ToString() == "Graph.Items.NodeLabelItem")
                    {
                        NodeLabelItem temp = item as NodeLabelItem;
                        ParentNode = temp.Text;
                    }
                    if (item.GetType().ToString() == "Graph.Items.NodeTextBoxItem")
                    {
                        NodeTextBoxItem temp = item as NodeTextBoxItem;
                        ParentNode = temp.Text;
                    }
                }
            }
            ItemType = nodeItem.GetType().ToString();
            Input = new List<CConnectionFields>();
            Output = new List<CConnectionFields>();
        }
    }
}
