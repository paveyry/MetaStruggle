using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.GUI
{
    public class MenuButton
    {
        public string Id { get; set; }
        public string ParticularString { get; set; }
        public string DisplayedName { get { return Lang.Language.GetString(Id) + ParticularString; } }
        public Texture2D Image { get { return Lang.Language.GetImage(Id, true); } }
        public Texture2D ImageOnClick { get { return Lang.Language.GetImage(Id, false); } }
        public float Scale { get; set; }
        public ClickEventHandler OnClick { get; set; }

        public MenuButtonDisplayType DisplayType
        {
            get { return Image == null ? MenuButtonDisplayType.Text : MenuButtonDisplayType.Image; }
        }

        public delegate void ClickEventHandler();

        public MenuButton(string nameId, string particularString , float scale, ClickEventHandler onClick)
        {
            ParticularString = particularString;
            Id = nameId;
            OnClick = onClick;
            Scale = scale;
        }
        
        public MenuButton(string nameId, ClickEventHandler onClick) : this(nameId, "",  0.1f, onClick) {}

        public MenuButton(string nameId,float scale , ClickEventHandler onClick) : this(nameId, "",  scale, onClick) {}

        public MenuButton(string nameId, string particularString, ClickEventHandler onClick) : this(nameId, particularString, 0.1f, onClick) {}
    }
}
