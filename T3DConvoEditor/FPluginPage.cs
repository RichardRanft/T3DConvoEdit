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
