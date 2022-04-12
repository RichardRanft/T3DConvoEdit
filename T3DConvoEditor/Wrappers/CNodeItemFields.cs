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
    public class CNodeItemFields
    {
        public List<CConnectionFields> Input { get; set; }
        public List<CConnectionFields> Output { get; set; }
        public String id { get; set; }
        public String name { get; set; }
        public String ItemType { get; set; }
        public String ParentNode { get; set; }
        public Graph.NodeIOMode IOMode { get; set; }
        public String Text { get; set; }
        public bool Multiline { get; set; }
        public List<CItemPartFields> ItemParts { get; set; }
        public String Tag { get; set; }

        public CNodeItemFields()
        {
            List<CConnectionFields> Input = new List<CConnectionFields>();
            List<CConnectionFields> Output = new List<CConnectionFields>();
            List<CItemPartFields> ItemParts = new List<CItemPartFields>();
        }
    }
}
