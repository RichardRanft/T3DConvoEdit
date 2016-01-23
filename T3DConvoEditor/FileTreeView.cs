using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

namespace T3DConvoEditor
{
    class FileTreeView : TreeView
    {
        private string pathText;

        #region Path update methods

        /// <summary>
        /// Set the path text - this will generate a set of TreeNode objects to match.
        /// </summary>
        [Browsable(false)]
        public virtual void AddPath(string path)
        {
            if (this.Nodes == null || this.Nodes.Count < 1)
                this.Nodes.Add(new TreeNode("Start"));
            pathText = path;
            string[] parts = pathText.Split('\\');
            TreeNode start = this.SelectedNode == null ? this.Nodes[0] : this.SelectedNode;
            foreach (String part in parts)
            {
                if (start.Nodes.Find(part, true).Length > 0)
                {
                    start = start.Nodes.Find(part, true)[0];
                    continue;
                }
                else
                {
                    TreeNode node = new TreeNode(part);
                    node.Name = part;
                    start.Nodes.Add(node);
                    start = node;
                }
            }
        }

        /// <summary>
        /// Set the top node name and text to the assigned string.
        /// </summary>
        [Browsable(false)]
        public virtual void SetTopNodeName(string name)
        {
            if (this.Nodes == null || this.Nodes.Count < 1)
            {
                this.Nodes.Add(new TreeNode(name));
                this.Nodes[0].Name = name;
            }
            else
            {
                this.Nodes[0].Text = name;
                this.Nodes[0].Name = name;
            }
            this.Update();
        }

        #endregion

        #region Tooltip

		ToolTip tooltip = new ToolTip();

		#endregion
    }
}
