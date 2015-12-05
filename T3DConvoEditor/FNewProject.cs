using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace T3DConvoEditor
{
    public partial class FNewProject : Form
    {
        private String m_path;
        private String m_name;
        private bool m_isValid;

        public String BasePath { get { return m_path; } }
        public String ProjectName { get { return m_name; } }
        public bool IsValid { get { return m_isValid; } }

        public FNewProject()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (fbdProjFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                tbxBasePath.Text = fbdProjFolder.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_isValid = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_path = tbxBasePath.Text;
            m_name = tbxProjName.Text;
            String pathinfo = Path.GetDirectoryName(m_path);
            if (pathinfo == "")
                m_isValid = false;
            else
                m_isValid = true;
            if(m_isValid)
                this.DialogResult = DialogResult.OK;

            tbxProjName.Text = "";
            tbxBasePath.Text = "";
            this.Close();
        }
    }
}
