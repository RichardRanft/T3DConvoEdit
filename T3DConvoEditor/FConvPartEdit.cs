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
