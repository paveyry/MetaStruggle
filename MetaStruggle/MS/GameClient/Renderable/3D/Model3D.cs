using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Renderable._3D
{
    public class Model3D : I3DElement
    {
        #region model fields
        public Model Model { get; set; }
        public float XRotation { get; set; }
        public float YRotation { get; set; }
        public float ZRotation { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public string ModelName { get; set; }
        public SceneManager Scene { get; set; }
        public float Speed { get; set; }
        public float Gravity { get; set; }
        #endregion

        public Model3D(SceneManager scene, Model model, Vector3 position, Vector3 scale)
        {
            Model = model;
            Scene = scene;
            Position = position;
            Scale = scale;
        }

        public Matrix World
        {
            get
            {
                return Matrix.Identity *
                       Matrix.CreateRotationX(Pitch) *
                       Matrix.CreateRotationY(Yaw) *
                       Matrix.CreateRotationZ(Roll) *
                       Matrix.CreateTranslation(Position) *
                       Matrix.CreateRotationX(XRotation) *
                       Matrix.CreateRotationY(YRotation) *
                       Matrix.CreateRotationZ(ZRotation) *
                       Matrix.CreateScale(Scale);
            }
        }

        public void Update(GameTime gameTime)
        {}

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = mesh.ParentBone.Transform*World;
                    effect.View = Scene.Camera.ViewMatrix;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), spriteBatch.GraphicsDevice.DisplayMode.AspectRatio, 1f, 100f);
                }

                mesh.Draw();
            }
        }
    }
}
