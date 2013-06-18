using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Characters;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;

namespace GameClient.Renderable.Scene
{
    public class Camera3D
    {
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
        public float XRotation { get; set; }
        public float YRotation { get; set; }
        public float ZRotation { get; set; }

        public Matrix ViewMatrix
        {
            get
            {
                return Matrix.CreateLookAt(Position, Target, Vector3.Up) *
                       Matrix.CreateRotationX(XRotation) *
                       Matrix.CreateRotationY(YRotation) *
                       Matrix.CreateRotationZ(ZRotation);
            }
        }

        public Camera3D(Vector3 position, Vector3 target)
        {
            Position = position;
            Target = target;
        }

        public void SetTarget(I3DElement targetElement)
        {
            Target = new Vector3(targetElement.Position.X, targetElement.Position.Y + 4, targetElement.Position.Z);
        }

        public void FollowsCharacters(Camera3D camera, IEnumerable<I3DElement> characters)
        {
            if (!characters.Any())
                return;

            float minX = float.MaxValue, maxX = float.MinValue;
            foreach (Character character in characters)
            {
                if (character.Position.X < minX)
                    minX = character.Position.X;
                if (character.Position.X > maxX)
                    maxX = character.Position.X;
            }
            Target = new Vector3((maxX + minX) / 2, camera.Target.Y, characters.First().Position.Z);

            Position = new Vector3(Target.X, Position.Y, (characters.Count() == 1) ? Position.Z :
                                   -(float)((maxX - Target.X) / Math.Tan(MathHelper.PiOver4 / 2)) + Target.Z);
        }
    }
}
