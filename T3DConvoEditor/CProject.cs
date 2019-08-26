using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using log4net;

namespace ConvoEditor
{
    public class CProject
    {
        private static ILog m_log = LogManager.GetLogger(typeof(CProject));

        private XmlDocument m_projectData;
        private String m_projectName;
        private String m_baseFolder;
        private String m_convFolder;
        private String m_scriptFolder;
        private String m_convExt;
        private String m_scriptExt;
        private Dictionary<String, String> m_conversations;
        private Dictionary<String, String> m_scripts;
        private bool m_dirty;

        public FileTreeView TreeView { get; set; }

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

        public String SaveFolder
        {
            get { return m_convFolder; }
            set { m_convFolder = value; }
        }

        public String ScriptFolder
        {
            get { return m_scriptFolder; }
            set { m_scriptFolder = value; }
        }

        public String SaveExt
        {
            get { return m_convExt; }
            set { m_convExt = value; }
        }

        public String ScriptExt
        {
            get { return m_scriptExt; }
            set { m_scriptExt = value; }
        }

        public bool IsDirty
        {
            get { return m_dirty; }
            private set { m_dirty = value; }
        }

        public Dictionary<String, String> Conversations
        {
            get { return m_conversations; }
            private set { m_conversations = value; }
        }

        public CProject(String path)
        {
            loadProject(path);
        }

        public CProject()
        {
            m_baseFolder = "";
            m_convFolder = "";
            m_scriptFolder = "";
            m_projectName = "";
            m_convExt = ".json";
            m_scriptExt = "";
            m_projectData = new XmlDocument();
            m_conversations = new Dictionary<string, string>();
            m_scripts = new Dictionary<string, string>();
        }

        public bool Save(String path)
        {
            try
            {
                XmlNode docNode, projectNode, projectnameNode, basepathNode, convpathNode, scriptpathNode, dataNode;
                docNode = m_projectData.CreateXmlDeclaration("1.0", "UTF-8", null);
                m_projectData.AppendChild(docNode);

                projectNode = m_projectData.CreateElement("Project");
                m_projectData.AppendChild(projectNode);

                projectnameNode = m_projectData.CreateElement("Name");
                projectnameNode.AppendChild(m_projectData.CreateTextNode(m_projectName));
                projectNode.AppendChild(projectnameNode);

                basepathNode = m_projectData.CreateElement("BasePath");
                basepathNode.AppendChild(m_projectData.CreateTextNode(m_baseFolder));
                projectNode.AppendChild(basepathNode);

                convpathNode = m_projectData.CreateElement("ConversationPath");
                convpathNode.AppendChild(m_projectData.CreateTextNode(m_convFolder));
                projectNode.AppendChild(convpathNode);

                scriptpathNode = m_projectData.CreateElement("ScriptPath");
                scriptpathNode.AppendChild(m_projectData.CreateTextNode(m_scriptFolder));
                projectNode.AppendChild(scriptpathNode);

                dataNode = m_projectData.CreateElement("Conversations");
                foreach (String key in m_conversations.Keys)
                {
                    XmlNode node = m_projectData.CreateElement(key);
                    node.AppendChild(m_projectData.CreateTextNode(m_conversations[key] + m_convExt));
                    dataNode.AppendChild(node);
                }
                projectNode.AppendChild(dataNode);

                dataNode = m_projectData.CreateElement("Scripts");
                foreach (String key in m_scripts.Keys)
                {
                    XmlNode node = m_projectData.CreateElement(key);
                    node.AppendChild(m_projectData.CreateTextNode(m_scripts[key] + m_scriptExt));
                    dataNode.AppendChild(node);
                }
                projectNode.AppendChild(dataNode);

                String folder = Path.GetDirectoryName(path);
                if (!Directory.Exists(folder))
                {
                    try
                    {
                        Directory.CreateDirectory(folder);
                    }
                    catch (Exception ex)
                    {
                        m_log.Error("Error saving project data : ", ex);
                        return false;
                    }
                }
                m_projectData.Save(path);

                m_dirty = false;

                this.TreeView.Nodes.Clear();
                this.TreeView.SetTopNodeName(m_projectName);
                TreeNode rootNode = new TreeNode(m_projectName);
                this.TreeView.Nodes.Add(rootNode);

                return true;
            }
            catch (Exception ex)
            {
                m_log.Error("Error saving project data : ", ex);
                return false;
            }
        }

        public bool AddConversation(String name, String path)
        {
            // Add a conversation item to the project.  Saves a dictionary of name/path entries
            // Prevents duplicate conversation names, and therefore duplicate script object names.
            if(!m_conversations.Keys.Contains(name))
            {
                m_conversations.Add(name, m_convFolder + "\\" + path);
                m_scripts.Add(name, m_scriptFolder + "\\" + path);
                this.TreeView.AddPath(path);
                m_dirty = true;

                return true;
            }
            m_log.Info("Conversation " + name + " already exists.");
            return false;
        }

        public bool RenameConversation(String oldName, String newName)
        {
            if(m_conversations.Keys.Contains(oldName))
            {
                String path = m_conversations[oldName];
                String newpath = path.Replace(oldName, newName);
                try
                {
                    File.Move(path, newpath);
                }
                catch (Exception ex)
                {
                    m_log.Error(String.Format("Unable to rename {0} to {1}:", path, newpath), ex);
                    return false;
                }
                m_conversations.Remove(oldName);
                m_conversations.Add(newName, newpath);

                path = m_scripts[oldName];
                newpath = path.Replace(oldName, newName);
                try
                {
                    File.Move(path, newpath);
                }
                catch (Exception ex)
                {
                    m_log.Error(String.Format("Unable to rename {0} to {1}:", path, newpath), ex);
                }
                m_scripts.Remove(oldName);
                m_scripts.Add(newName, newpath);
                this.TreeView.AddPath(newpath);
                // remove old file path - start at leaf, work back toward trunk until containing folder is not empty 
                // after the removal of nodes.
                m_dirty = true;

                return true;
            }
            m_log.Warn("Could not find a conversation named " + oldName + ".");
            return false;
        }

        public bool RemoveConversation(String name)
        {
            if(m_conversations.Keys.Contains(name))
            {
                String path = m_conversations[name];
                try
                {
                    File.Delete(path);
                }
                catch(Exception ex)
                {
                    String message = "Unable to delete conversation save file for " + name + " : " + Environment.NewLine;
                    message += ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += Environment.NewLine + ex.InnerException.Message;
                    }
                    return false;
                }
                path = m_scripts[name];
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                    String message = "Unable to delete script file for " + name + " : " + Environment.NewLine;
                    message += ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += Environment.NewLine + ex.InnerException.Message;
                    }
                    return false;
                }
                m_conversations.Remove(name);
                m_scripts.Remove(name);

                m_dirty = true;

                return true;
            }
            return false;
        }

        public bool Contains(String conv)
        {
            foreach(String key in m_conversations.Keys)
            {
                if (m_conversations[key].Equals(conv))
                    return true;
            }
            return false;
        }

        private void loadProject(String path)
        {
            if (!File.Exists(path))
                return;

            XmlDocument m_projectData = new XmlDocument();
            m_conversations = new Dictionary<string, string>();
            m_scripts = new Dictionary<string, string>();
            m_projectData.Load(path);
            m_baseFolder = m_projectData["Project"]["BasePath"].InnerText;
            m_convFolder = m_projectData["Project"]["ConversationPath"].InnerText;
            m_scriptFolder = m_projectData["Project"]["ScriptPath"].InnerText;
            m_projectName = m_projectData["Project"]["Name"].InnerText;
            int convCount = m_projectData["Project"]["Conversations"].ChildNodes.Count;
            for (int i = 0; i < convCount; ++i)
            {
                XmlNode node = m_projectData["Project"]["Conversations"].ChildNodes[i];
                m_conversations.Add(node.Name, node.InnerText);
            }
            for (int i = 0; i < convCount; ++i)
            {
                XmlNode node = m_projectData["Project"]["Scripts"].ChildNodes[i];
                m_scripts.Add(node.Name, node.InnerText);
            }

            m_dirty = false;
        }
    }
}
