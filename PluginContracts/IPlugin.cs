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
		void Export(System.String filename);
        System.String GetDefaultExtension();
        void Initialize(GraphControl ctrl, CLog log);
	}
}
