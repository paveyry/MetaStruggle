using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void SetTarget(_3D.I3DElement targetElement)
        {
            Target = new Vector3(targetElement.Position.X, targetElement.Position.Y + 4, targetElement.Position.Z);
        }
    }
}
