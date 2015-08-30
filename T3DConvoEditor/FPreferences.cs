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
using BasicSettings;

namespace T3DConvoEditor
{
    public partial class FPreferences : Form
    {
        private CSettings m_settings;

        public CSettings Settings
        {
            get
            {
                return m_settings;
            }
            set
            {
                m_settings = value;
            }
        }

        public FPreferences(CSettings settings)
        {
            m_settings = settings;
            InitializeComponent();

            tbxDefaultNodeName.Text = m_settings.Attributes["[Default]"]["DEFAULTNODENAME"];
            tbxDefaultFilename.Text = m_settings.Attributes["[Default]"]["DEFAULTFILENAME"];
            tbxOutputFolder.Text = m_settings.Attributes["[Default]"]["OUTPUTFOLDER"];
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            m_settings.Set("[Default]", "DEFAULTNODENAME", tbxDefaultNodeName.Text);
            m_settings.Set("[Default]", "DEFAULTFILENAME", tbxDefaultFilename.Text);
            m_settings.Set("[Default]", "OUTPUTFOLDER", tbxOutputFolder.Text);
            m_settings.SaveSettings();
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void FPreferences_Activated(object sender, EventArgs e)
        {
            tbxDefaultNodeName.Text = m_settings.Attributes["[Default]"]["DEFAULTNODENAME"];
            tbxDefaultFilename.Text = m_settings.Attributes["[Default]"]["DEFAULTFILENAME"];
            tbxOutputFolder.Text = m_settings.Attributes["[Default]"]["OUTPUTFOLDER"];
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            if(fbdFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tbxOutputFolder.Text = fbdFolder.SelectedPath;
            }
        }
    }
}
