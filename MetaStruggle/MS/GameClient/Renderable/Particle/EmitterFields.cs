using System;
using DPSF;

namespace GameClient.Renderable.Particle
{
    [Serializable]
    public class EmitterFields
    {
        public bool EmitParticlesAutomatically { get; set; }
        public bool LerpEmittersPositionAndOrientation { get; set; }
        public bool LerpEmittersPositionAndOrientationOnNextUpdate { get; set; }
        public float ParticlesPerSecond { get; set; }
        public int BurstParticles { get; set; }
        public float BurstTime { get; set; }

        public static void CopyEmitterFieldsToParticleEmitter(EmitterFields copyEmitter, ParticleEmitter emitter)
        {
            emitter.EmitParticlesAutomatically = copyEmitter.EmitParticlesAutomatically;
            emitter.LerpEmittersPositionAndOrientation = copyEmitter.LerpEmittersPositionAndOrientation;
            emitter.LerpEmittersPositionAndOrientationOnNextUpdate =
                copyEmitter.LerpEmittersPositionAndOrientationOnNextUpdate;
            emitter.ParticlesPerSecond = copyEmitter.ParticlesPerSecond;
            emitter.BurstParticles = copyEmitter.BurstParticles;
            emitter.BurstTime = copyEmitter.BurstTime;
        }
    }
}
