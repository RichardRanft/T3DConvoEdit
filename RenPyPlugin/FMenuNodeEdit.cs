// Copyright (c) 2015 Richard Ranft

// Permission is hereby granted, free of charge, to any person obtaining a copy of this 
// software and associated documentation files (the "Software"), to deal in the Software 
// without restriction, including without limitation the rights to use, copy, modify, 
// merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to the following 
// conditions:

// The above copyright notice and this permission notice shall be included in all copies
// or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
using BasicSettings;

namespace RenPyPlugin
{
    public partial class FMenuNodeEdit : Form
    {
        private CSettings m_settings;

        public CRenPyPlugin PluginMain = null;
        public int MaxOutputs = 6;
        public Node EditingNode;

        public int ConvoNodeStart = 4;

        public FMenuNodeEdit()
        {
            InitializeComponent();
        }

        public CSettings Settings
        {
            get { return m_settings; }
            set { m_settings = value; }
        }

        public String Text
        {
            get { return tbxText.Text; }
            set { tbxText.Text = value; }
        }

        private void FNodeEdit_Activated(object sender, EventArgs e)
        {
            if(m_settings != null)
            {
                MaxOutputs = int.Parse(m_settings.Attributes["[Default]"]["MAXOUTPUTS"]);
                ConvoNodeStart = int.Parse(m_settings.Attributes["[Default]"]["CONVOOUTNODESTART"]);
            }
            lbxChoiceNodes.Items.Clear();
            List<NodeItem> items = (List<NodeItem>)EditingNode.Items;
            if (items.Count > ConvoNodeStart)
            {
                for (int i = ConvoNodeStart; i < items.Count; i++)
                {
                    NodeCompositeItem item = (NodeCompositeItem)items[i];
                    String text = "";
                    foreach(ItemTextBoxPart part in item.Parts)
                    {
                        if (part.Name == "ConvText")
                            text = part.Text;
                    }
                    lbxChoiceNodes.Items.Add(text);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            List<NodeItem> items = (List<NodeItem>)EditingNode.Items;
            if (items.Count < (MaxOutputs + ConvoNodeStart))
            {
                NodeCompositeItem newNode = new NodeCompositeItem(NodeIOMode.Output) { Tag = PluginContracts.TagType.TEXTBOX };
                newNode.Name = "button_" + findUnusedButtonIndex().ToString().PadLeft(2, '0');
                ItemTextBoxPart btnText = new ItemTextBoxPart("Enter player text");
                btnText.Name = "ConvText";
                ItemTextBoxPart btnMethod = new ItemTextBoxPart("Enter script method");
                btnMethod.Name = "ConvMethod";
                newNode.AddPart(btnText);
                newNode.AddPart(btnMethod);
                newNode.Clicked += PluginMain.GetConvMouseHandler();
                NodeTextBoxItem nodeName = (NodeTextBoxItem)items[0];
                EditingNode.AddItem(newNode);
                lbxChoiceNodes.Items.Clear();
                if (items.Count > ConvoNodeStart)
                {
                    for (int i = ConvoNodeStart; i < items.Count; i++)
                    {
                        NodeCompositeItem item = (NodeCompositeItem)items[i];
                        String text = "";
                        foreach(ItemTextBoxPart part in item.Parts)
                        {
                            if (part.Name == "ConvText")
                                text = part.Text;
                        }
                        lbxChoiceNodes.Items.Add(text);
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
                        NodeCompositeItem item = (NodeCompositeItem)items[i];
                        String text = "";
                        foreach (ItemTextBoxPart part in item.Parts)
                        {
                            if (part.Name == "ConvText")
                                text = part.Text;
                        }
                        lbxChoiceNodes.Items.Add(text);
                    }
                }
            }
        }
    }
}
