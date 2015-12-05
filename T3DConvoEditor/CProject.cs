using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using BasicLogging;

namespace T3DConvoEditor
{
    public class CProject
    {
        private XmlDocument m_projectData;
        private String m_projectName;
        private String m_baseFolder;
        private Dictionary<String, String> m_conversations;
        private CLog m_log;

        public String Name
        {
            get { return m_projectName; }
            set { m_projectName = value; }
        }

        public String BaseFolder
        {
            get { return m_baseFolder; }
            set { m_baseFolder = value; }
        }

        public CProject(CLog log, String path)
        {
            m_log = log;
            loadProject(path);
        }

        public CProject(CLog log)
        {
            m_baseFolder = "";
            m_projectName = "";
            m_projectData = new XmlDocument();
            m_conversations = new Dictionary<string, string>();
            m_log = log;
        }

        public bool Save(String path)
        {
            try
            {
                XmlNode docNode, projectNode, projectnameNode, basepathNode, dataNode;
                docNode = m_projectData.CreateXmlDeclaration("1.0", "UTF-8", null);
                m_projectData.AppendChild(docNode);

                projectNode = m_projectData.CreateElement("Project");
                m_projectData.AppendChild(projectNode);

                projectnameNode = m_projectData.CreateElement("Name");
                projectnameNode.AppendChild(m_projectData.CreateTextNode(m_projectName));
                projectNode.AppendChild(projectnameNode);

                basepathNode = m_projectData.CreateElement("BasePath");
                projectNode.AppendChild(m_projectData.CreateTextNode(m_baseFolder));
                projectNode.AppendChild(basepathNode);

                dataNode = m_projectData.CreateElement("Data");
                foreach(String key in m_conversations.Keys)
                {
                    dataNode.AppendChild(m_projectData.CreateTextNode(key + "," + m_conversations[key]));
                }
                projectNode.AppendChild(dataNode);

                m_projectData.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                if (ex.InnerException != null)
                    message += Environment.NewLine + ex.InnerException.Message;
                m_log.WriteLine("Error saving project data : " + message);
                return false;
            }
        }

        public bool AddConversation(String name, String path)
        {
            // Add a conversation item to the project.  Saves a dictionary of name/path entries
            // Prevents duplicate conversation names, and therefore duplicate script object names.
            return false;
        }

        public bool RenameConversation(String oldName, String newName)
        {
            return false;
        }

        public bool RemoveConversation(String name)
        {

            return false;
        }

        private void loadProject(String path)
        {
            XmlDocument m_projectData = new XmlDocument();
            m_projectData.Load(path);
            XmlNode nameNode;
            
        }
    }
}
