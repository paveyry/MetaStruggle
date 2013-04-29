using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Lang
{
    public class LanguageLoader
    {
        const string DirLang = "Languages";
        const string XmlMainElement = "Language";
        readonly Dictionary<string, Dictionary<string, Texture2D>> _languageImage = new Dictionary<string, Dictionary<string, Texture2D>>();
        readonly Dictionary<string, Dictionary<string, string>> _languageText = new Dictionary<string, Dictionary<string, string>>();
        public string[] LanguageAvailable { get { return _languageText.Keys.ToArray(); } }

        public LanguageLoader(GraphicsDevice graphics)
        {
            LoadingFiles(graphics);
        }

        void LoadingFiles(GraphicsDevice graphics)
        {
            if (!Directory.Exists(DirLang))
                Directory.CreateDirectory(DirLang);

            foreach (var dir in Directory.GetDirectories(DirLang))
            {
                try //Load XML
                {
                    var langfile = new XmlDocument();
                    langfile.Load(dir + "\\" + Path.GetFileNameWithoutExtension(dir) + ".xml");
                    foreach (XmlNode xmlNode in langfile.GetElementsByTagName(XmlMainElement)) _languageText.Add(Path.GetFileName(dir), xmlNode.ChildNodes.Cast<XmlNode>()
                                                .ToDictionary(childNode => childNode.LocalName,
                                                              childNode => childNode.InnerText));
                }
                catch { }
                try //Load Images
                {
                    _languageImage.Add(Path.GetFileName(dir), Directory.GetFiles(dir)
                                      .Where(file => Path.GetExtension(file) == ".png"
                                          && _languageText[Path.GetFileName(dir)].ContainsKey(GetNameFile(Path.GetFileNameWithoutExtension(file))))
                                               .ToDictionary(Path.GetFileNameWithoutExtension, file => Texture2D.FromStream(graphics, new FileStream(file, FileMode.Open))));

                }
                catch { }
            }
        }

        private string GetNameFile(string dir)
        {
            var str = dir.Split('.');
            if ((str.Length == 2 && str[1] == "click") || str.Length == 1)
                return str[0];
            return "";
        }

        #region GetLanguage
        public Texture2D GetImage(string language, string id, bool isNormal)
        {
            if (_languageImage.ContainsKey(language) && _languageImage[language].ContainsKey(id))
            {
                if (isNormal || !_languageImage[language].ContainsKey(id + ".click"))
                    return _languageImage[language][id];
                return _languageImage[language][id + ".click"];
            }

            return null;
        }

        public string GetString(string language, string id)
        {
            if (_languageText.ContainsKey(language) && _languageText[language].ContainsKey(id))
                return _languageText[language][id];
            return null;
        }
        #endregion
    }
}