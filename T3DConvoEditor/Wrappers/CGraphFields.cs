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

        protected CGraphFields()
        {
            id = "";
            name = "";
            Nodes = new List<CNodeFields>();
        }

        public CGraphFields(GraphControl control, ObjectIDGenerator idGen)
        {
            bool first = false;
            id = idGen.GetId(control, out first).ToString();
            name = control.Name;
            Nodes = new List<CNodeFields>();
            List<Node> nodes = (List<Node>)control.Nodes;
            foreach (Node node in nodes)
            {
                Nodes.Add(new CNodeFields(node, idGen));
            }
        }
    }
}
