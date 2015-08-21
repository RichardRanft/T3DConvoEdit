using System;
using System.IO;
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
using BasicLogging;
using BasicSettings;
using PluginContracts;

namespace T3DConvoEditor
{
    public partial class FPluginPage : Form
    {
        private Dictionary<string, IPlugin> _Plugins;
        private CLog m_log;

        public Dictionary<string, IPlugin> Plugins
        {
            get { return _Plugins; }
        }

        public FPluginPage(CLog log)
        {
            InitializeComponent();
            m_log = log;
            loadPlugins();
        }

        private void loadPlugins()
        {
            _Plugins = new Dictionary<string, IPlugin>();
            try
            {
                ICollection<IPlugin> plugins = GenericPluginLoader<IPlugin>.LoadPlugins("Plugins");
                foreach (var item in plugins)
                {
                    _Plugins.Add(item.Name, item);
                    m_log.WriteLine("Loaded " + item.Name);
                }
            }
            catch(Exception ex)
            {
                m_log.WriteLine("Failed to load plugins: " + ex.Message + "\n" + ex.StackTrace);
                MessageBox.Show("Failed to load plugins - see log for error.", "Plugin Load Failure");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void FPluginPage_Activated(object sender, EventArgs e)
        {
            lbxLoadedPlugins.Items.Clear();
            foreach(String key in _Plugins.Keys)
            {
                lbxLoadedPlugins.Items.Add(key);
            }
        }
    }
}
