using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI
{
    public class MenuButton1
    {
        public string Id { get; set; }
        public string ParticularString { get; set; }
        public string DisplayedName { get { return GameEngine.LangCenter.GetString(Id) + ParticularString; } }
        public Texture2D Image { get { return GameEngine.LangCenter.GetImage(Id, true); } }
        public Texture2D ImageOnClick { get { return GameEngine.LangCenter.GetImage(Id, false); } }
        public float Scale { get; set; }
        public ClickEventHandler OnClick { get; set; }

        public MenuButtonDisplayType DisplayType
        {
            get { return Image == null ? MenuButtonDisplayType.Text : MenuButtonDisplayType.Image; }
        }

        public delegate void ClickEventHandler();

        public MenuButton1(string nameId, string particularString , float scale, ClickEventHandler onClick)
        {
            ParticularString = particularString;
            Id = nameId;
            OnClick = onClick;
            Scale = scale;
        }
        
        public MenuButton1(string nameId, ClickEventHandler onClick) : this(nameId, "",  0.5f, onClick) {}

        public MenuButton1(string nameId,float scale , ClickEventHandler onClick) : this(nameId, "",  scale, onClick) {}

        public MenuButton1(string nameId, string particularString, ClickEventHandler onClick) : this(nameId, particularString, 0.5f, onClick) {}
    }
}
