using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;
using XNAnimation.Controllers;

namespace GameClient.Renderable._3D
{
    public class AnimatedModel3D : I3DElement
    {
        #region model fields
        public SkinnedModel Model { get; set; }
        public float XRotation { get; set; }
        public float YRotation { get; set; }
        public float ZRotation { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public string Name { get; set; }
        public SceneManager Scene { get; set; }
        private int _animClip;
        public AnimationController AnimationController { get; set; }
        private float _speed ;

        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                AnimationController.Speed = value;
            }
        }

        public float Gravity { get; set; }
        #endregion

        public Matrix World
        {
            get
            {
                return Matrix.Identity*
                       Matrix.CreateRotationX(Pitch)*
                       Matrix.CreateRotationY(Yaw)*
                       Matrix.CreateRotationZ(Roll)*
                       Matrix.CreateScale(Scale)*
                       Matrix.CreateTranslation(Position)*
                       Matrix.CreateRotationX(XRotation)*
                       Matrix.CreateRotationY(YRotation)*
                       Matrix.CreateRotationZ(ZRotation);
            }
        }

        public Animation CurrentAnimation
        {
            get { return (Animation) _animClip; }
        }

        public AnimatedModel3D(SceneManager scene, SkinnedModel model, Vector3 position, Vector3 scale)
        {
            Model = model;
            Scene = scene;
            Position = position;
            Scale = scale;
            _animClip = 0;

            AnimationController = new AnimationController(Model.SkeletonBones)
                {
                    Speed = Speed,
                    TranslationInterpolation = InterpolationMode.Linear,
                    OrientationInterpolation = InterpolationMode.Linear,
                    ScaleInterpolation = InterpolationMode.Linear
                };

            Speed = 1;

            SetAnimation(Animation.Default);
        }

        public void SetAnimation(Animation animation)
        {
            if (_animClip == (int) animation)
                return;

            switch (animation)
            {
                case Animation.Run:
                    _animClip = 2;
                    break;
                case Animation.Jump:
                    _animClip = 3;
                    break;
                case Animation.Attack:
                    _animClip = 4;
                    break;
                case Animation.Default:
                    _animClip = 1;
                    break;
            }

            if (_animClip > Model.AnimationClips.Count)
                _animClip = 0;
            AnimationController.StartClip(Model.AnimationClips.Values.Count == 1
                                              ? Model.AnimationClips.Values[0]
                                              : Model.AnimationClips.Values[_animClip]);
        }

        public virtual void Update(GameTime gameTime)
        {
            AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (ModelMesh mesh in Model.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(AnimationController.SkinnedBoneTransforms);

                    effect.EnableDefaultLighting();
                    /*effect.DiffuseColor = new Vector3(0.6f, 0.6f, 0.6f);
                    effect.AmbientLightColor = Vector3.One;
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;*/

                    effect.World = World;

                    effect.View = Scene.Camera.ViewMatrix;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), spriteBatch.GraphicsDevice.DisplayMode.AspectRatio, 1f, 100f); 
                }

                mesh.Draw();
            }
        }
    }

    public enum Animation
    {
        Default = 1, 
        Run, 
        Jump, 
        Attack
    }
}
