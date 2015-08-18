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

        public CNodeFields()
        {
            List<CNodeItemFields> Items = new List<CNodeItemFields>();
        }
    }
}
