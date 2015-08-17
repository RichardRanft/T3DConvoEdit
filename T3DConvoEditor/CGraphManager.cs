using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph;
using Graph.Compatibility;
using Graph.Items;
using Newtonsoft.Json;
using BasicLogging;
using T3DConvoEditor.Wrappers;
using System.Runtime.Serialization;

namespace T3DConvoEditor
{
    public class CGraphManager
    {
        private CLog m_log;
        private JsonSerializer m_serializer;
        private ObjectIDGenerator m_idGen;

        public CGraphManager(CLog log)
        {
            m_log = log;
            m_idGen = new ObjectIDGenerator();
            m_serializer = new JsonSerializer();
        }

        public void SaveGraph(GraphControl graph, String filename)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    writeGraph(graph, sw);
                }
            }
            catch (Exception ex)
            {
                m_log.WriteLine("Could not save file " + filename + " : " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void writeGraph(GraphControl graph, StreamWriter stream)
        {
            CGraphFields graphFields = new CGraphFields(graph, m_idGen);
            m_serializer.Serialize(stream, graphFields);
        }

        public void LoadGraph(String filename)
        {

        }
    }
}
