using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Renderable;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.Environments
{
    public static class Environment1
    {
        public static SceneManager GetScene(SpriteBatch spriteBatch)
        {
            var sm = SceneManager.CreateScene(
                    new Vector3(-5, 5, -30),    //Position initiale de la caméra
                    new Vector3(0, 0, 0),       //Point visé par la caméra
                    spriteBatch);               //SpriteBatch

            //sm.Skybox = new Skybox(Global.RessourceProvider.Videos["Intro"]);
            sm.AddElement(new Character("Spiderman", "Spiderman", 3,sm, new Vector3(-5, 10, -17), new Vector3(1), 1.6f));
            sm.AddElement(new Character("Zeus", "Zeus", 2,sm, new Vector3(-12, 10, -17), new Vector3(1)));
            sm.AddElement(new Character("Ironman", "Ironman", 1,sm, new Vector3(-12, 10, -17), new Vector3(1)));
            sm.AddElement(new Character("Alex", "Alex", 0,sm, new Vector3(-8, 10, -17), new Vector3(1), 1.6f));
            //sm.AddElement(new Character("Ares", "Ares", sm, new Vector3(-3, 10, -17), new Vector3(1)));
            sm.AddElement(new Model3D(sm, Global.RessourceProvider.StaticModels["MapDesert"], new Vector3(10, 0, 0),
                          new Vector3(1f, 1f, 0.8f)));
            //sm.AddElement(new AnimatedModel3D("Dwarf",sm, new Vector3(-9, 0, -10), new Vector3(0.04f)) { Speed = 0.5f });

            //sm.Camera.SetTarget(sm.Items.First(current => current.ModelName == "Spiderman"));
            sm.Camera.FollowsCharacters(sm.Camera, sm.Items.FindAll(e => e is Character));
            return sm;
        }
    }
}
