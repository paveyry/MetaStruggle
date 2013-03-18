using Microsoft.Xna.Framework.Audio;

namespace GameClient.SoundEngine
{
    abstract class Sound
    {
        public SoundEffectInstance SoundEffect { get; set; }

        protected Sound(SoundEffect soundEffect)
        {
            SoundEffect = soundEffect.CreateInstance();
        }
    }
}
