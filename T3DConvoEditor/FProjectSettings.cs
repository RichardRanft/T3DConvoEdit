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
    public partial class FProjectSettings : Form
    {
        private String m_path;
        private String m_savePath;
        private String m_scriptPath;
        private String m_name;
        private String m_scriptExt;
        private CProject m_project;
        private bool m_isValid;

        public CProject Project { set { m_project = value; } get { return m_project; } }
        public String BasePath { set { m_path = value; tbxProjName.Text = m_path; } get { return m_path; } }
        public String SavePath { set { m_savePath = value; tbxConvPath.Text = m_savePath; } get { return m_savePath; } }
        public String ScriptPath { set { m_scriptPath = value; tbxScriptPath.Text = m_scriptPath; } get { return m_scriptPath; } }
        public String ScriptExt { set { m_scriptExt = value; tbxScriptPath.Text = m_scriptExt; } get { return m_scriptExt; } }
        public String ProjectName { set { m_name = value; tbxProjName.Text = m_name; } get { return m_name; } }
        public bool IsValid { get { return m_isValid; } }

        public FProjectSettings()
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
            if (m_isValid)
                this.DialogResult = DialogResult.OK;

            m_project.Name = m_name;
            m_project.BaseFolder = m_path;
            m_project.SaveFolder = m_savePath;
            m_project.ScriptFolder = m_scriptPath;
            m_project.ScriptExt = m_scriptExt;
            m_project.SaveExt = ".cnvproj";

            tbxProjName.Text = "";
            tbxBasePath.Text = "";
            tbxConvPath.Text = "";
            tbxScriptPath.Text = "";
            this.Close();
        }

        private void FProjectSettings_Activated(object sender, EventArgs e)
        {

        }
    }
}
