using WMPLib;

namespace GameClient.SoundEngine
{
    internal abstract class Sound
    {
        public enum Status
        {
            Play,
            Pause,
            Stop,
            Undefined
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
            Player = new WindowsMediaPlayer {URL = path};
            Player.settings.enableErrorDialogs = true;
            Position = 0;
            Loop = loop;
            Player.controls.stop();
            Player.PlayStateChange += PlayStateChange;
        }

        public void PlayStateChange(int state)
        {
            if ((WMPPlayState) state == WMPPlayState.wmppsStopped && RealLoop)
                Play();
            switch ((WMPPlayState) state)
            {
                case WMPPlayState.wmppsPaused:
                    PlayerStatus = Status.Pause;
                    break;
                case WMPPlayState.wmppsPlaying:
                    PlayerStatus = Status.Play;
                    break;
                case WMPPlayState.wmppsStopped:
                    PlayerStatus = Status.Stop;
                    break;
                default:
                    PlayerStatus = Status.Undefined;
                    break;
            }
        }

        #region BasicPlayer

        public void Play()
        {
            int Volume = Player.settings.volume;
            RealLoop = Loop;
            Player.controls.currentPosition = (Player.playState == WMPPlayState.wmppsPlaying) ? 0 : Position;

            Player.controls.play();
        }

        public void Pause()
        {
            Position = Player.controls.currentPosition;
            Player.controls.pause();
        }

        public void Stop()
        {
            RealLoop = false;
            Position = 0;
            Player.controls.stop();
        }

        #endregion
    }
}
