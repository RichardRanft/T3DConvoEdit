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
