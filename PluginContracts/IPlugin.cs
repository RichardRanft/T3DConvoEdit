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
using BasicLogging;
using BasicSettings;

namespace PluginContracts
{
	public interface IPlugin
	{
		string Name { get; }
        CSettings Settings { get; }
        System.String GetDefaultExtension();

        EventHandler<NodeItemEventArgs> GetEditMouseHandler();
        EventHandler<NodeItemEventArgs> GetConvMouseHandler();
        List<string> GetNodeTypenames();
        Node GetNodeByTypename(string name);

        void ShowSettings();

        bool SaveGraph(GraphControl graph, String filename);
        GraphControl LoadGraph(String filename);

        void Initialize(GraphControl ctrl, CLog log);
        void Export(System.String filename);
    }

    // translation tool for deserialization-time recovery of object types.
    public static class TagFactory
    {
        public static object GetTagObject(String typeName)
        {
            switch (typeName)
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
