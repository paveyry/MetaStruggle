using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable;
using GameClient.Renderable.Scene;
using GameClient.Renderable._3D;
using GameClient.Renderable._3D.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable.Environments
{
    public static class Environment1
    {
        public static SceneManager GetScene(SpriteBatch spriteBatch)
        {
            var sm = SceneManager.CreateScene(
                    new Vector3(-5, 5,-30),    //Position initiale de la caméra
                    new Vector3(0, 0, 0),       //Point visé par la caméra
                    spriteBatch);               //SpriteBatch

            //sm.Skybox = new Skybox(Global.RessourceProvider.Videos["Intro"]);
            sm.AddElement(new Spiderman(sm, new Vector3(-5, 0, -17), new Vector3(1.2f), 1.6f) {Name = "test"});
            sm.AddElement(new Zeus(sm, new Vector3(-8, 0, -17), new Vector3(1)) { Name = "MainCharacter" });
            //sm.AddElement(new Model3D(sm, Global.RessourceProvider.StaticModels["MapDesert"], new Vector3(10, 0, 0),
            //                          new Vector3(1f, 1f, 0.8f)));

            //sm.AddElement(new AnimatedModel3D(sm, Global.RessourceProvider.AnimatedModels["Dwarf"],
            //                                  new Vector3(-9, 0, -10), new Vector3(0.04f))
            //                                  {Speed = 0.5f});

            sm.Camera.SetTarget(sm.Items.First(current => current.Name == "MainCharacter"));

            return sm;
        }
    }
}
