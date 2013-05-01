using GameClient.Characters;
using GameClient.CollisionEngine.RenderBoundingObjects;
using GameClient.Renderable._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.CollisionEngine
{
    public class BoundingObjectModel
    {
        Vector3 ModelPosition { get; set; }
        public BoundingBox BoxModel;
        private readonly float Length, WeidthOver2;

        public BoundingObjectModel(Character model)
        {
            Length = model.Length;
            WeidthOver2 = model.Width / 2;
            ModelPosition = new Vector3();
            BoxModel = new BoundingBox();
            UpdateBox(model);

            //BonesSpheres = new BoundingSphere[model.Model.SkeletonBones.Count];
            //UpdateSpheres(model);
        }

        public void UpdateBox(AnimatedModel3D upModel)
        {
            ModelPosition = upModel.Position;

            BoxModel.Min = ModelPosition - new Vector3(WeidthOver2, 0, WeidthOver2);
            BoxModel.Max = ModelPosition + new Vector3(WeidthOver2, Length, WeidthOver2);
        }

        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection, AnimatedModel3D upModel)
        {
            //foreach (var boundingSphere in BonesSpheres)
            //    BoundingSphereRenderer.Render(boundingSphere, graphics, view, projection, new Color(255, 255, 255));

            BoundingBoxRenderer.RenderBox(BoxModel, graphics, view, projection, new Color(255, 255, 255));
        }

        public bool Intersects(BoundingObjectModel b)
        {
            return BoxModel.Intersects(b.BoxModel);
        }

        ////BoundingSphereBone OLD :(
        //BoundingSphere[] BonesSpheres { get; set; }
        //public void UpdateSpheres(AnimatedModel3D upModel)
        //{
        //    ModelPosition = upModel.Position;
        //    Matrix[] absolute = new Matrix[upModel.Model.Model.Bones.Count];
        //    Matrix[] translation = new Matrix[upModel.Model.Model.Bones.Count];
        //    upModel.Model.Model.CopyAbsoluteBoneTransformsTo(absolute);
        //    upModel.Model.Model.CopyBoneTransformsTo(translation);

        //    for (int i = 1; i < upModel.Model.SkeletonBones.Count; i++)
        //    {

        //        //BonesSpheres[i].Radius = (absolute[upModel.Model.Model.Bones[i].Parent.Index].Translation - absolute[i].Translation).Length()/2;
        //        BonesSpheres[i].Radius = upModel.Scale.X * upModel.Model.SkeletonBones[i].BindPose.Translation.Length() / 2;
        //        BonesSpheres[i].Center = upModel.Scale
        //            * Vector3.Transform(upModel.Model.SkeletonBones[i].BindPose.Translation, Matrix.CreateRotationX(MathHelper.PiOver2)
        //            * Matrix.CreateRotationZ(-MathHelper.Pi) * upModel.World);
        //        BonesSpheres[i].Transform(translation[i]);

        //    }
        //}
    }
}
