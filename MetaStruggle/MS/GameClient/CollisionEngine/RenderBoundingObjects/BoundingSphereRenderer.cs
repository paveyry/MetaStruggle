//* Code C/P Pour afficher les bounding Sphere *

using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameClient.CollisionEngine.RenderBoundingObjects
{
    public static class BoundingSphereRenderer
    {
        static VertexBuffer vertBuffer;
        static BasicEffect effect;
        static int sphereResolution;

        public static void InitializeGraphics(GraphicsDevice graphicsDevice, int sphereResolution)
        {
            BoundingSphereRenderer.sphereResolution = sphereResolution;

            effect = new BasicEffect(graphicsDevice);
            effect.LightingEnabled = false;
            effect.VertexColorEnabled = false;

            VertexPositionColor[] verts = new VertexPositionColor[(sphereResolution + 1) * 3];

            int index = 0;

            float step = MathHelper.TwoPi / (float)sphereResolution;
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
                verts[index++] = new VertexPositionColor(
                    new Vector3((float) Math.Cos(a), (float) Math.Sin(a), 0f),
                    Color.White);
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
                verts[index++] = new VertexPositionColor(
                    new Vector3((float) Math.Cos(a), 0f, (float) Math.Sin(a)),
                    Color.White);
            for (float a = 0f; a <= MathHelper.TwoPi; a += step)
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float) Math.Cos(a), (float) Math.Sin(a)),
                    Color.White);

            vertBuffer = new VertexBuffer(
                graphicsDevice,
                VertexPositionColor.VertexDeclaration,
                verts.Length,
                BufferUsage.None);
            vertBuffer.SetData(verts);
        }

        public static void Render(BoundingSphere sphere, GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Color xyColor, Color xzColor, Color yzColor)
        {
            if (vertBuffer == null)
                InitializeGraphics(graphicsDevice, 30);

            graphicsDevice.SetVertexBuffer(vertBuffer);

            effect.World =
                Matrix.CreateScale(sphere.Radius) *
                Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = xyColor.ToVector3();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      sphereResolution + 1,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      (sphereResolution + 1) * 2,
                      sphereResolution);
            }
        }

        public static void Render(BoundingSphere sphere, GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Color color)
        {
            if (vertBuffer == null)
                InitializeGraphics(graphicsDevice, 30);

            graphicsDevice.SetVertexBuffer(vertBuffer);
            effect.World =
                  Matrix.CreateScale(sphere.Radius) *
                  Matrix.CreateTranslation(sphere.Center);
            effect.View = view;
            effect.Projection = projection;
            effect.DiffuseColor = color.ToVector3();
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      0,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      sphereResolution + 1,
                      sphereResolution);
                graphicsDevice.DrawPrimitives(
                      PrimitiveType.LineStrip,
                      (sphereResolution + 1) * 2,
                      sphereResolution);
            }
        }
    }
}