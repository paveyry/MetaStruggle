using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Renderable._3D;

namespace GameClient.CollisionEngine
{
    class CollisionEngine
    {
        private Dictionary<string, BoundingObjectModel> _boundingObjectModels;

        public CollisionEngine()
        {
            _boundingObjectModels = new Dictionary<string, BoundingObjectModel>();
        }

        public void Add(string name, AnimatedModel3D model)
        {
            _boundingObjectModels.Add(name,new BoundingObjectModel(model));
        }

        public bool Intersects(string modelName)
        {
            foreach (var b in _boundingObjectModels)
                if (_boundingObjectModels[modelName].Intersects(b.Value))
                    return true;
            return false;
        }
    }
}
