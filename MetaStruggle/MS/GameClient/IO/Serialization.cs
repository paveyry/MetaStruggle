using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameClient.IO
{
    public static class Serialization
    {
        public static Global.Config LoadConfigFile(string configFileName)
        {
            try
            {
                return LoadFile(configFileName, typeof(Global.Config)) as Global.Config;
            }
            catch (Exception)
            {
                return Global.Config.GetDefaultConfig();
            }
        }

        public static void SaveConfigFile(string configFileName, Global.Config config)
        {
            var xs = new XmlSerializer(typeof(Global.Config));

            using (var sw = new StreamWriter(configFileName))
                xs.Serialize(sw, config);
        }

        public static void GetFields(object copy, object objectOut)
        {
            var tempFields = copy;
            foreach (var field in objectOut.GetType().GetFields())
                field.SetValue(objectOut, field.GetValue(tempFields));
        }

        public static object LoadFile(string dir, Type t)
        {
            var xs = new XmlSerializer(t);

            using (var sr = new StreamReader(dir))
                return xs.Deserialize(sr);
        }
    }
}
