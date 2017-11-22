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
    class CItemTextBoxPartFields : CItemPartFields
    {
        public String Text;

        public CItemTextBoxPartFields(ItemTextBoxPart itemPart, ObjectIDGenerator idGen)
            : base(itemPart as ItemPart, idGen)
        {
            Text = itemPart.Text;
        }
    }
}
