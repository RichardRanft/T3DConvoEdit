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
    public class CNodeCompositeItemFields : CNodeItemFields
    {
        public List<CItemPartFields> ItemParts { get; set; }

        public CNodeCompositeItemFields(NodeItem nodeItem, ObjectIDGenerator idGen) : base(nodeItem, idGen)
        {
            ItemParts = new List<CItemPartFields>();
            NodeCompositeItem item = nodeItem as NodeCompositeItem;
            foreach(ItemPart part in item.Parts)
            {
                switch(part.GetType().ToString())
                {
                    case "Graph.Items.ItemTextBoxPart":
                        {
                            ItemTextBoxPart temp = part as ItemTextBoxPart;
                            CItemTextBoxPartFields tempFields = new CItemTextBoxPartFields(temp, idGen);
                            ItemParts.Add(tempFields);
                        }
                        break;
                    default:
                        {
                            CItemPartFields temp = new CItemPartFields(part, idGen);
                            ItemParts.Add(temp);
                        }
                        break;
                }
            }
        }
    }
}
