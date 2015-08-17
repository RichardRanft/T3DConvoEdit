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
    public class CItemPartFields
    {
        protected String m_partType;
        public String id { get; set; }
        public String name { get; set; }
        public String PartType { get { return m_partType; } }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        public CItemPartFields(ItemPart itemPart, ObjectIDGenerator idGen)
        {
            bool first = false;
            id = idGen.GetId(itemPart, out first).ToString();
            name = itemPart.Name;
            m_partType = itemPart.GetType().ToString();
        }
    }
}
