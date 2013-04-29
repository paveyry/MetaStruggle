//* Code C/P Pour afficher les bounding box *

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameClient.CollisionEngine.RenderBoundingObjects
{
    public static class BoundingBoxRenderer
    {
        #region Fields

        private static VertexPositionColor[] verts = new VertexPositionColor[8];

        private static int[] indices = new int[]
                                           {
                                               0, 1,
                                               1, 2,
                                               2, 3,
                                               3, 0,
                                               0, 4,
                                               1, 5,
                                               2, 6,
                                               3, 7,
                                               4, 5,
                                               5, 6,
                                               6, 7,
                                               7, 4,
                                           };

        private static BasicEffect effect;

        #endregion

        public static void RenderBox(BoundingBox box, GraphicsDevice graphicsDevice, Matrix view, Matrix projection, Color color)
        {
            if (effect == null)
            {
                effect = new BasicEffect(graphicsDevice);
                effect.VertexColorEnabled = true;
                effect.LightingEnabled = false;
            }

            Vector3[] corners = box.GetCorners();
            for (int i = 0; i < 8; i++)
            {
                verts[i].Position = corners[i];
                verts[i].Color = color;
            }

            effect.View = view;
            effect.Projection = projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.LineList,
                    verts,
                    0,
                    8,
                    indices,
                    0,
                    indices.Length / 2);

            }
        }
    }
}