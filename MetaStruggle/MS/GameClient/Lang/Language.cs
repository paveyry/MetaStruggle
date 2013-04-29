using GameClient.Global;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Lang
{
    public class Language
    {
        public static string GetString(string key)
        {
            string value = GameEngine.LangCenter.GetString(GameEngine.Config.Language, key);
            return string.IsNullOrEmpty(value) ? key : value;
        }

        public static Texture2D GetImage(string key, bool isNormal)
        {
            return GameEngine.LangCenter.GetImage(GameEngine.Config.Language, key, isNormal);
        }
    }
}
