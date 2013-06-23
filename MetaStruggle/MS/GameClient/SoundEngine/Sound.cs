using WMPLib;

namespace GameClient.SoundEngine
{
    internal abstract class Sound
    {
        public enum Status
        {
            Stop,
            Play,
            Pause
        }

        private WindowsMediaPlayer Player { get; set; }
        private double Position { get; set; }
        private bool Loop { get; set; }
        private bool RealLoop { get; set; }
        public Status PlayerStatus { get; set; }

        public int Volume
        {
            get { return Player.settings.volume; }
            set
            {
                Player.settings.volume = value;
            }
        }

        protected Sound(string path, bool loop)
        {
            Player = new WindowsMediaPlayer { URL = path };
            Player.settings.enableErrorDialogs = true;
            Position = 0;
            Loop = loop;
            Player.controls.stop();
            PlayerStatus = Status.Stop;
            Player.PlayStateChange += PlayStateChange;
        }

        public void PlayStateChange(int state)
        {
            if ((WMPPlayState)state == WMPPlayState.wmppsStopped)
            {
                if (RealLoop)
                    Play();
                else
                    PlayerStatus = Status.Stop;
            }
        }

        #region BasicPlayer

        public void Play()
        {
            RealLoop = Loop;
            if (PlayerStatus == Status.Pause)
            {
                Player.controls.currentPosition = Position;
                Position = 0;
            }

            PlayerStatus = Status.Play;
            Player.controls.play();
        }

        public void Pause()
        {
            Position = Player.controls.currentPosition;
            PlayerStatus = Status.Pause;
            Player.controls.pause();
        }

        public void Stop()
        {
            RealLoop = false;
            Position = 0;
            PlayerStatus = Status.Stop;
            Player.controls.stop();
        }

        #endregion
    }
}
