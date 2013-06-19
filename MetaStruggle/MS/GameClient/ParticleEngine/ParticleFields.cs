using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace GameClient.ParticleEngine
{
    public class ParticleFields
    {
        public string TextureDir { get; set; }
        #region ParticleEvents
        public bool BoolUpdateParticlePositionUsingVelocity { get; set; }
        public bool BoolUpdateParticleRotationUsingRotationalVelocity { get; set; }
        public bool BoolUpdateParticleWidthAndHeightUsingLerp { get; set; }
        public bool BoolUpdateParticleColorUsingLerp { get; set; }

        public bool BoolUpdateParticleTransparencyToFadeOutUsingLerp { get; set; }
        public int IntUpdateParticleTransparencyToFadeOutUsingLerp { get; set; }

        public bool BoolUpdateParticleToFaceTheCamera { get; set; }
        public int IntUpdateParticleToFaceTheCamera { get; set; }
        #endregion
    }
}
