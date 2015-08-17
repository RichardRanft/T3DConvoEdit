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
    public class CItemFields
    {
        public String id { get; set; }
        public String name { get; set; }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        protected CItemFields(NodeItem item, ObjectIDGenerator idGen)
        {
            bool first = false;
            id = idGen.GetId(item, out first).ToString();
            name = item.Name;
        }

    }
}
