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
    public partial class Form1 : Form
    {
        private CLog m_log;
        private CSettings m_settings;
        private bool m_dirty;
        private FNodeEdit m_nodeEdit;
        private FPreferences m_preferences;
        private String m_saveDefaultPath;
        private Dictionary<string, IPlugin> _Plugins;

        public Form1()
        {
            m_dirty = false;
            m_log = new CLog();
            m_log.Filename = "T3DConvoEditor.log";
            m_settings = new CSettings("T3DConvoEditor.ini");
            m_settings.LoadSettings();

            InitializeComponent();
            init();

            m_preferences = new FPreferences(m_settings);
            m_preferences.Settings = m_settings;

            m_nodeEdit = new FNodeEdit();
            m_nodeEdit.MaxOutputs = int.Parse(m_settings.Attributes["[Default]"]["MAXOUTPUTS"]);
            m_nodeEdit.Text = "Edit Conversation Selection List";
            m_log.WriteLine("T3D Conversation Editor started");

            String homeFolder = Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            m_saveDefaultPath = homeFolder + @"\Roostertail Games\T3DConvoEditor\";
            if (!Directory.Exists(m_saveDefaultPath))
                Directory.CreateDirectory(m_saveDefaultPath);
            sfdSaveGraphFile.InitialDirectory = m_saveDefaultPath;
            ofdOpenFile.InitialDirectory = m_saveDefaultPath;

            String defFileName = m_settings.Attributes["[Default]"]["DEFAULTFILENAME"];
            tbxConvoName.Text = defFileName.Remove(defFileName.LastIndexOf('.'));
        }

        private void init()
        {
            graphCtrl.NodeAdded += new EventHandler<AcceptNodeEventArgs>(onNodeAdded);
            int width = pnlWork.Width - (2 * pnlWork.Margin.All);
            int height = pnlWork.Height - (2 * pnlWork.Margin.All);
            Rectangle graphBounds = new Rectangle(new Point(pnlWork.Margin.All, pnlWork.Margin.All), new Size(width, height));
            pnlGraph.Bounds = graphBounds;
            pnlGraph.Controls.Add(graphCtrl);

			_Plugins = new Dictionary<string, IPlugin>();
			ICollection<IPlugin> plugins = GenericPluginLoader<IPlugin>.LoadPlugins("Plugins");
            foreach (var item in plugins)
            {
                _Plugins.Add(item.Name, item);
                m_log.WriteLine("Loaded " + item.Name);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            while(m_dirty)
            {
                System.Windows.Forms.DialogResult save = MessageBox.Show("Save before exiting?", "Save", MessageBoxButtons.YesNo);
                if (save == System.Windows.Forms.DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else
                    m_dirty = false;
            }
            Application.Exit();
        }

        private void pnlWork_Resize(object sender, EventArgs e)
        {
            int width = pnlWork.Width - (2 * pnlWork.Margin.All);
            int height = pnlWork.Height - (2 * pnlWork.Margin.All);
            Rectangle graphBounds = new Rectangle(new Point(pnlWork.Margin.All, pnlWork.Margin.All), new Size(width, height));
            pnlGraph.Bounds = graphBounds;
        }

        private void lblStartNode_MouseDown(object sender, MouseEventArgs e)
        {
            var node = new Node("Conversation Start");
            var startLabel = new NodeLabelItem("Conversation_Start", NodeIOMode.Output) { Tag = TagType.LABEL };
            startLabel.Name = "NodeName";
            node.AddItem(startLabel);
            this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private void lblConvoNode_MouseDown(object sender, MouseEventArgs e)
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            String nodeName = m_settings.Attributes["[Default]"]["DEFAULTNODENAME"] + "_" + getConvNodeCount().ToString().PadLeft(4, '0');
			var node = new Node("Conversation Node");
            var nodeNameItem = new NodeTextBoxItem(nodeName);
            nodeNameItem.Name = "NodeName";
            node.AddItem(nodeNameItem);
            node.AddItem(new NodeTextBoxItem("Enter NPC text", NodeIOMode.None));
            var inputLabel = new NodeLabelItem("Conversation input", NodeIOMode.Input) { Tag = TagType.LABEL };
            inputLabel.Name = nodeName + "_in";
            node.AddItem(inputLabel);
            var editNode = new NodeLabelItem("Click Here To Edit Output List");
            editNode.Name = "EditNodeItem";
            editNode.Clicked += new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
            node.AddItem(editNode);
            NodeTextBoxItem firstButton = new NodeTextBoxItem("Enter player text", NodeIOMode.Output) { Tag = TagType.TEXTBOX };
            firstButton.Name = "button_1";
            node.AddItem(firstButton);
			this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private void lblEndNode_MouseDown(object sender, MouseEventArgs e)
        {
            var node = new Node("Conversation End");
            node.AddItem(new NodeTextBoxItem("Enter text"));
            String name = "Conversation_End_" + getEndNodeCount().ToString().PadLeft(3, '0');
            var endLabel = new NodeLabelItem(name, NodeIOMode.Input) { Tag = TagType.TEXTBOX };
            endLabel.Name = "NodeName";
            node.AddItem(endLabel);
            node.AddItem(new NodeTextBoxItem("Conversation Exit Script"));
            this.DoDragDrop(node, DragDropEffects.Copy);
        }

        private int getConvNodeCount()
        {
            int count = 0;

            foreach(Node node in graphCtrl.Nodes)
            {
                if (node.Title == "Conversation Node")
                    count++;
            }

            return count;
        }

        private int getEndNodeCount()
        {
            int count = 0;

            foreach (Node node in graphCtrl.Nodes)
            {
                if (node.Title == "Conversation End")
                    count++;
            }

            return count;
        }

        public EventHandler<NodeItemEventArgs> GetEditMouseHandler()
        {
            return new EventHandler<NodeItemEventArgs>(editOutputListNode_MouseDown);
        }

        private void editOutputListNode_MouseDown(object sender, NodeItemEventArgs e)
        {
            if(e.Item != null)
            {
                m_nodeEdit.EditingNode = e.Item.Node;
                m_nodeEdit.ShowDialog(this);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && validateGraph())
            {
                CGraphManager graphman = new CGraphManager(this, m_log);
                graphman.SaveGraph(graphCtrl, sfdSaveGraphFile.FileName);
                m_dirty = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ofdOpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<Node> nodeList = new List<Node>();
                foreach (Node n in graphCtrl.Nodes)
                    nodeList.Add(n);
                graphCtrl.RemoveNodes(nodeList);
                graphCtrl.Refresh();
                CGraphManager graphman = new CGraphManager(this, m_log);
                graphman.LoadGraph(graphCtrl, ofdOpenFile.FileName);
                m_dirty = false;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (validateGraph())
            {
                if (_Plugins.ContainsKey("TSWriterPlugin"))
                {
                    IPlugin plugin = _Plugins["TSWriterPlugin"];
                    plugin.Initialize(graphCtrl, m_log);
                    sfdExportScript.DefaultExt = plugin.GetDefaultExtension();
                    sfdExportScript.InitialDirectory = m_settings.Attributes["[Default]"]["OUTPUTFOLDER"];
                    sfdExportScript.FileName = tbxConvoName.Text + ".cs";
                    sfdExportScript.Filter = "TorqueScript file|*.cs";
                    if(sfdExportScript.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        exportGraph(plugin, sfdExportScript.FileName);
                }
            }
        }

        private void exportGraph(IPlugin plugin, String filename)
        {
            plugin.Export(filename);
        }

        private bool validateGraph()
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            if (nodes.Count < 1)
            {
                MessageBox.Show("There is nothing to save.", "Graph Empty");
                return false;
            }
            if (!checkContents(nodes))
            {
                MessageBox.Show("There are no conversation nodes in this graph.", "Graph Incomplete");
                return false;
            }
            if (!checkConnections(nodes))
            {
                MessageBox.Show("You have unconnected inputs or outputs in your conversation graph.  Please review your graph and ensure all node inputs and outputs are connected.", "Check Connections");
                return false;
            }
            return true;
        }

        private bool checkContents(List<Node> nodes)
        {
            bool convoNodeFound = false;

            foreach(Node node in nodes)
            {
                if(node.Title.Equals("Conversation Node"))
                {
                    convoNodeFound = true;
                    break;
                }
            }

            return convoNodeFound;
        }

        private bool checkConnections(List<Node> nodelist)
        {
            foreach(Node n in nodelist)
            {
                if (n.HasNoItems)
                    continue;
                if (n.AnyConnectorsDisconnected)
                    return false;
            }
            return true;
        }

        private void onNodeAdded(object sender, AcceptNodeEventArgs e)
        {
            List<Node> nodes = (List<Node>)graphCtrl.Nodes;
            if (nodes.Count > 0)
                m_dirty = true;
            graphCtrl.Focus();
        }

        void graphCtrl_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                GraphControl ctrl = (GraphControl)sender;
                switch(e.KeyCode)
                {
                    case Keys.Delete:
                        ctrl.RemoveNodes();
                        break;
                }
            }
            catch(Exception ex)
            {
                m_log.WriteLine("Unable to delete nodes from GraphControl : " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_log.WriteLine("Exiting T3DConvoEditor");
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_preferences.ShowDialog();
            String defFileName = m_settings.Attributes["[Default]"]["DEFAULTFILENAME"];
            tbxConvoName.Text = defFileName.Remove(defFileName.LastIndexOf('.'));
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_dirty)
            {
                if(MessageBox.Show("Save before starting a new conversation?", "Save", MessageBoxButtons.YesNoCancel) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (sfdSaveGraphFile.ShowDialog() == System.Windows.Forms.DialogResult.OK && validateGraph())
                    {
                        CGraphManager graphman = new CGraphManager(this, m_log);
                        graphman.SaveGraph(graphCtrl, sfdSaveGraphFile.FileName);
                    }
                }
            }
            List<Node> nodeList = new List<Node>();
            foreach (Node n in graphCtrl.Nodes)
                nodeList.Add(n);
            graphCtrl.RemoveNodes(nodeList);
            graphCtrl.Refresh();
            m_dirty = false;
        }
    }

    // translation tool for deserialization-time recovery of object types.
    public static class TagFactory
    {
        public static object GetTagObject(String typeName)
        {
            switch(typeName)
            {
                case "T3DConvoEditor.CheckboxClass":
                    return TagType.CHECKBOX;
                case "T3DConvoEditor.ColorClass":
                    return TagType.COLOR;
                case "T3DConvoEditor.DropDownClass":
                    return TagType.DROPDOWN;
                case "T3DConvoEditor.ImageClass":
                    return TagType.IMAGE;
                case "T3DConvoEditor.LabelClass":
                    return TagType.LABEL;
                case "T3DConvoEditor.NumericSliderClass":
                    return TagType.NUMERICSLIDER;
                case "T3DConvoEditor.SliderClass":
                    return TagType.SLIDER;
                case "T3DConvoEditor.TextBoxClass":
                    return TagType.TEXTBOX;
                case "T3DConvoEditor.NodeTitleClass":
                    return TagType.NODETITLE;
                case "T3DConvoEditor.CompositeClass":
                    return TagType.COMPOSITE;
                default:
                    return null;
            }
        }
    }

    // Friendly tag def
    [Serializable]
    public static class TagType
    {
        public static CheckboxClass CHECKBOX = new CheckboxClass();
        public static ColorClass COLOR = new ColorClass();
        public static DropDownClass DROPDOWN = new DropDownClass();
        public static ImageClass IMAGE = new ImageClass();
        public static LabelClass LABEL = new LabelClass();
        public static NumericSliderClass NUMERICSLIDER = new NumericSliderClass();
        public static SliderClass SLIDER = new SliderClass();
        public static TextBoxClass TEXTBOX = new TextBoxClass();
        public static NodeTitleClass NODETITLE = new NodeTitleClass();
        public static CompositeClass COMPOSITE = new CompositeClass();
    }

    // Dummy classes to use as types for item tags
    [Serializable]
    public class CheckboxClass : object { }
    [Serializable]
    public class ColorClass : object { }
    [Serializable]
    public class DropDownClass : object { }
    [Serializable]
    public class ImageClass : object { }
    [Serializable]
    public class LabelClass : object { }
    [Serializable]
    public class NumericSliderClass : object { }
    [Serializable]
    public class SliderClass : object { }
    [Serializable]
    public class TextBoxClass : object { }
    [Serializable]
    public class NodeTitleClass : object { }
    [Serializable]
    public class CompositeClass : object { }
}
