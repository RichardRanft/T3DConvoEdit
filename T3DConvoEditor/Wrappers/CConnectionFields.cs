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
        public String From { get; set; }
        public String To { get; set; }
        public String id { get; set; }
        public String name { get; set; }

        internal string Identifier
        {
            get { return String.IsNullOrWhiteSpace(id) ? name : id; }
        }

        public CConnectionFields() { }
    }
}
