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
    public class CConnectionFields
    {
        public CNodeItemFields From { get; set; }
        public CNodeItemFields To { get; set; }
        public String id { get; set; }
        public String name { get; set; }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        public CConnectionFields(NodeConnection conn, ObjectIDGenerator idGen)
        {
            bool first = false;
            id = idGen.GetId(conn, out first).ToString();
            name = conn.Name;
            From = new CNodeItemFields(conn.From.Item, idGen);
            To = new CNodeItemFields(conn.To.Item, idGen);
        }
    }
}
