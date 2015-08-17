using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Graph;
using Graph.Compatibility;
using Graph.Items;

namespace T3DConvoEditor
{
    public partial class FNodeEdit : Form
    {
        public int MaxOutputs = 6;
        public Node EditingNode;

        public int ConvoNodeStart = 4;

        public FNodeEdit()
        {
            InitializeComponent();
        }

        private void FNodeEdit_Activated(object sender, EventArgs e)
        {
            lbxChoiceNodes.Items.Clear();
            List<NodeItem> items = (List<NodeItem>)EditingNode.Items;
            if (items.Count > ConvoNodeStart)
            {
                for (int i = ConvoNodeStart; i < items.Count; i++)
                {
                    NodeTextBoxItem item = (NodeTextBoxItem)items[i];
                    lbxChoiceNodes.Items.Add(item.Text);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<NodeItem> items = (List<NodeItem>)EditingNode.Items;
            if (items.Count < (MaxOutputs + ConvoNodeStart))
            {
                NodeTextBoxItem newNode = new NodeTextBoxItem("Enter player text", NodeIOMode.Output) { Tag = TagType.TEXTBOX };
                NodeTextBoxItem nodeName = (NodeTextBoxItem)items[0];
                newNode.Name = nodeName.Text + "_btn" + findUnusedButtonIndex().ToString().PadLeft(2, '0');
                EditingNode.AddItem(newNode);
                lbxChoiceNodes.Items.Clear();
                if (items.Count > ConvoNodeStart)
                {
                    for (int i = ConvoNodeStart; i < items.Count; i++)
                    {
                        NodeTextBoxItem item = (NodeTextBoxItem)items[i];
                        lbxChoiceNodes.Items.Add(item.Text);
                    }
                }
            }
            else
            {
                MessageBox.Show("A maximum of " + MaxOutputs + " is allowed.");
            }
        }

        private int findUnusedButtonIndex()
        {
            List<NodeItem> items = (List<NodeItem>)EditingNode.Items;
            int index = items.Count - ConvoNodeStart;
            List<String> foundNames = new List<string>();
            for (int i = ConvoNodeStart; i < items.Count; i++ )
            {
                foundNames.Add(items[i].Name);
            }
            foreach(String name in foundNames)
            {
                if (name.Contains(index.ToString().PadLeft(2, '0')))
                    index++;
            }

            return index;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(lbxChoiceNodes.SelectedIndex >= 0)
            {
                List<NodeItem> items = (List<NodeItem>)EditingNode.Items;
                if (items.Count > ConvoNodeStart)
                {
                    NodeItem item = items[lbxChoiceNodes.SelectedIndex + ConvoNodeStart];
                    EditingNode.RemoveItem(item);
                }
                lbxChoiceNodes.Items.Clear();
                items = (List<NodeItem>)EditingNode.Items;
                if (items.Count > ConvoNodeStart)
                {
                    for (int i = ConvoNodeStart; i < items.Count; i++)
                    {
                        NodeTextBoxItem item = (NodeTextBoxItem)items[i];
                        lbxChoiceNodes.Items.Add(item.Text);
                    }
                }
            }
        }
    }
}
