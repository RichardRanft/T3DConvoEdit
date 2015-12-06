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
        private String m_savePath;
        private String m_scriptPath;
        private String m_name;
        private bool m_isValid;

        public String BasePath { get { return m_path; } }
        public String SavePath { get { return m_savePath; } }
        public String ScriptPath { get { return m_scriptPath; } }
        public String ProjectName { get { return m_name; } }
        public bool IsValid { get { return m_isValid; } }

        public FNewProject()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (fbdProjFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (btn.Name == "btnBrowse")
                {
                    tbxBasePath.Text = fbdProjFolder.SelectedPath;
                    // defaults
                    tbxConvPath.Text = fbdProjFolder.SelectedPath + "\\Save";
                    tbxScriptPath.Text = fbdProjFolder.SelectedPath + "\\Scripts";
                }
                if (btn.Name == "btnCBrowse")
                    tbxConvPath.Text = fbdProjFolder.SelectedPath;
                if (btn.Name == "btnSBrowse")
                    tbxScriptPath.Text = fbdProjFolder.SelectedPath;
            }
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
            m_savePath = tbxConvPath.Text;
            m_scriptPath = tbxScriptPath.Text;
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
            tbxConvPath.Text = "";
            tbxScriptPath.Text = "";
            this.Close();
        }
    }
}
