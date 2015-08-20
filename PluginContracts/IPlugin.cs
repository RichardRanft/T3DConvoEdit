using System.Windows.Forms;
using Graph;
using BasicLogging;

namespace PluginContracts
{
	public interface IPlugin
	{
		string Name { get; }
		void Export(System.String filename);
        System.String GetDefaultExtension();
        void Initialize(GraphControl ctrl, CLog log);
	}
}
