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
    public class CNodeTextBoxItemFields : CNodeItemFields
    {
        public String Text { get; set; }

        public CNodeTextBoxItemFields(NodeItem nodeItem, ObjectIDGenerator idGen)
            : base(nodeItem, idGen)
        {
            NodeTextBoxItem item = nodeItem as NodeTextBoxItem;
            Text = item.Text;
        }
    }
}
