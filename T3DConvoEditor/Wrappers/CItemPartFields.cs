using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using System.Runtime.Serialization;

namespace ConvoEditor.Wrappers
{
    [Serializable]
    public class CItemPartFields
    {
        public String id { get; set; }
        public String name { get; set; }
        public String PartType { get; set; }
        public String Text { get; set; }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        public CItemPartFields() { }
    }
}
