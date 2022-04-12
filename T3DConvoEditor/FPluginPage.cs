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
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using BasicLogging;
using PluginContracts;

namespace T3DConvoEditor
{
    public partial class FPluginPage : Form
    {
        private Dictionary<string, IPlugin> _Plugins;
        private CLog m_log;
        private String m_pluginFolder;

        public IPlugin Active;

        public Dictionary<string, IPlugin> Plugins
        {
            get { return _Plugins; }
        }

        public void SetActive(String pluginName)
        {
            lblLoadedPlugin.Text = pluginName;
            Active = _Plugins[pluginName];
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
            m_pluginFolder = Path.GetFullPath(".\\" + "Plugins");
            m_log.WriteLine("Loading plugins from " + m_pluginFolder);
            try
            {
                ICollection<IPlugin> plugins = GenericPluginLoader<IPlugin>.LoadPlugins(m_pluginFolder);
                foreach (var item in plugins)
                {
                    _Plugins.Add(item.Name, item);
                    m_log.WriteLine("Loaded " + item.Name);
                }
                lbxAvailPlugins.Items.Clear();
                foreach(KeyValuePair<string, IPlugin> p in _Plugins)
                {
                    lbxAvailPlugins.Items.Add(p.Key);
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
            lbxAvailPlugins.Items.Clear();
            foreach(String key in _Plugins.Keys)
            {
                lbxAvailPlugins.Items.Add(key);
            }
        }

        private void lbxAvailPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbxAvailPlugins.SelectedIndex > -1)
            {
                Active = _Plugins[lbxAvailPlugins.SelectedItem.ToString()];
                lblLoadedPlugin.Text = Active.Name;
            }
        }
    }
}
