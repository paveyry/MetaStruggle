using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameClient.Particules //test de moteur de particules maison
{
    internal class Particle
    {
        private readonly ParticleSettings _settings;
        private readonly ParticleMgr _mgr;

        public bool Alive;

        public Vector3 Pos { get; set; }
        public double LifeTime { get; set; }
        public Vector3 Velocity { get; set; }
    }

    public struct ParticleSettings
    {
        public Color ColorStart { get; set; }

        public int Max { get; set; }
        public double LifeTime { get; set; }

        public ParticleSettings(double lifeTime, Color colorStart, int max = 200):this()
        {
            ColorStart = colorStart;
            Max = max;
            LifeTime = lifeTime;
        }
    }


}
