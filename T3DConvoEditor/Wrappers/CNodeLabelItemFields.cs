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
    public class CNodeLabelItemFields : CNodeItemFields
    {
        public String Text { get; set; }

        public CNodeLabelItemFields(NodeItem nodeItem, ObjectIDGenerator idGen)
            : base(nodeItem, idGen)
        {
            NodeLabelItem item = nodeItem as NodeLabelItem;
            Text = item.Text;
        }
    }
}
