using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameClient.IO
{
    public static class ConfigSerialization
    {
        public static Global.Config LoadFile(string configFileName)
        {
            if (!File.Exists(configFileName))
                return Global.Config.GetDefaultConfig();

            var xs = new XmlSerializer(typeof(Global.Config));

            using (var sr = new StreamReader(configFileName))
                return xs.Deserialize(sr) as Global.Config;
        }

        public static void SaveFile(string configFileName, Global.Config config)
        {
            var xs = new XmlSerializer(typeof(Global.Config));

            using (var sw = new StreamWriter(configFileName))
                xs.Serialize(sw, config);
        }
    }
}
