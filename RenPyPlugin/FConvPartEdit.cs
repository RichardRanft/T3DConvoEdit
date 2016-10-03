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

namespace RenPyPlugin
{
    public partial class FConvPartEdit : Form
    {
        public NodeCompositeItem Node = null;

        public FConvPartEdit()
        {
            InitializeComponent();
        }

        private void FConvPartEdit_Activated(object sender, EventArgs e)
        {
            if(Node != null)
            {
                foreach(ItemTextBoxPart part in Node.Parts)
                {
                    if (part.Name == "ConvText")
                        tbxPlayerText.Text = part.Text;
                    if (part.Name == "ConvMethod")
                        tbxBtnMethod.Text = part.Text;
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if(Node != null)
            {
                foreach(ItemTextBoxPart part in Node.Parts)
                {
                    if (part.Name == "ConvText")
                        part.Text = tbxPlayerText.Text;
                    if (part.Name == "ConvMethod")
                        part.Text = tbxBtnMethod.Text;
                }
            }
            this.Hide();
        }
    }
}
