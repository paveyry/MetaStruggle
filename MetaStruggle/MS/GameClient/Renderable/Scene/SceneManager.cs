using System;
using System.Collections.Generic;
using GameClient.Global;
using GameClient.Renderable._3D.Characters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable.Scene
{
    public class SceneManager : Layout.IBasicLayout
    {
        public Camera3D Camera { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public List<I3DElement> Items { get; private set; }
        public Skybox Skybox { get; set; }

        public SceneManager(Camera3D camera, SpriteBatch spriteBatch)
        {
            Camera = camera;
            SpriteBatch = spriteBatch;
            Items = new List<I3DElement>();
        }

        public void AddElement(I3DElement element)
        {
            Items.Add(element);
        }

        public static SceneManager CreateScene(Vector3 cameraPosition, Vector3 cameraTarget, SpriteBatch spriteBatch)
        {
            return new SceneManager(new Camera3D(cameraPosition, cameraTarget), spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (GameEngine.KeyboardState.IsKeyDown(Keys.Escape))
            {
                GameEngine.SoundCenter.PlayWithStatus();
                GameEngine.DisplayStack.Pop();
                return;
            }

            if (Skybox != null)
                Skybox.Update();

            foreach (var element in Items)
                element.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(Skybox != null)
                Skybox.Draw(spriteBatch);

            foreach (var element in Items)
                element.Draw(gameTime, spriteBatch);

            DrawHud(spriteBatch);
        }

        void DrawHud(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Texture2D face = Global.RessourceProvider.CharacterFaces["Zeus"];
            spriteBatch.Draw(face, new Rectangle(10,10,face.Width/10,face.Height/10), Color.White);
            Zeus main = Items.Find(current => current.Name == "MainCharacter") as Zeus;
            spriteBatch.DrawString(Global.RessourceProvider.Fonts["HUD"],
                                   "0% " +
                                   (main.IsDead
                                   ? Lang.Language.GetString("respawn") + ((int)(5 - (DateTime.Now - main.DeathDate).TotalSeconds)).ToString() : ""),
                                   new Vector2(80, 10), Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
