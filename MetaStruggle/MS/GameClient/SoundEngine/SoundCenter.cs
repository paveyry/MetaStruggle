using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GameClient.Characters;
using GameClient.Global;
using GameClient.Renderable._3D;

namespace GameClient.SoundEngine
{
    public class SoundCenter
    {
        private const string DirMusic = "Musics";
        private const string DirEffect = "Effects";
        private readonly Dictionary<string, Sound> _soundBank = new Dictionary<string, Sound>();
        private readonly Dictionary<string, Thread> _poolTask = new Dictionary<string, Thread>();
        private Sound.Status MusicStatus { get; set; }

        public int VolumeMusic
        {
            get { return GameEngine.Config.VolumeMusic; }
            set
            {
                GameEngine.Config.VolumeMusic = (value >= 0 && value <= 100) ? value : 100;
                ChangeVolume(GameEngine.Config.VolumeMusic, true);
            }
        }

        public int VolumeEffect
        {
            get { return GameEngine.Config.VolumeEffect; }
            set
            {
                GameEngine.Config.VolumeEffect = (value >= 0 && value <= 100) ? value : 100;
                ChangeVolume(GameEngine.Config.VolumeEffect, false);
            }
        }

        private static SoundCenter _soundCenter;

        public static SoundCenter Instance
        {
            get { return _soundCenter ?? (_soundCenter = new SoundCenter()); }
        }

        public SoundCenter()
        {
            CheckDirectories();
            LoadMusics();
            LoadEffects();
            MusicStatus = Sound.Status.Stop;

            GameEngine.EventManager.Register("Character.Jump", (data) => PlaySoundEvent("Jump", (Character) data));
            GameEngine.EventManager.Register("Character.Die", (data) => PlaySoundEvent("Die", (Character) data));
            GameEngine.EventManager.Register("Character.Attack", (data) => PlaySoundEvent("Attack", (Character) data));
            GameEngine.EventManager.Register("Character.Run", (data) => PlaySoundEvent("Run", (Character) data));
        }

        void ChangeVolume(int volume, bool isMusic)
        {
            foreach (var sound in _soundBank)
                if ((sound.Value is Music) && isMusic)
                    sound.Value.Volume = volume;
                else if ((sound.Value is EffectSound) && !isMusic)
                    sound.Value.Volume = volume;
        }

        #region PlayerWithStatus

        public void PlayWithStatus()
        {
            if (MusicStatus == Sound.Status.Play)
            {
                PauseAll();
                MusicStatus = Sound.Status.Pause;
            }
            else if (MusicStatus == Sound.Status.Pause)
            {
                ResumeAll();
                MusicStatus = Sound.Status.Play;
            }
        }

        public void PlayWithStatus(string sound)
        {
            if (MusicStatus == Sound.Status.Stop)
            {
                Play(sound);
                MusicStatus = Sound.Status.Play;
            }

            else if (MusicStatus == Sound.Status.Pause)
            {
                ResumeAll();
                MusicStatus = Sound.Status.Play;
            }
        }

        public void StopAllWithStatus()
        {
            MusicStatus = Sound.Status.Stop;
            StopAll();
        }

        #endregion PlayerWithStatus

        #region LoadingFiles

        void CheckDirectories()
        {
            if (!Directory.Exists(DirMusic))
                Directory.CreateDirectory(DirMusic);
            if (!Directory.Exists(DirEffect))
                Directory.CreateDirectory(DirEffect);
        }

        void LoadMusics()
        {
            foreach (var file in Directory.GetFiles(DirMusic))
                try
                {
                    _soundBank.Add(Path.GetFileNameWithoutExtension(file), new Music(file));
                }
                catch { }
        }

        void LoadEffects()
        {
            foreach (var file in Directory.GetFiles(DirEffect))
                try
                {
                    _soundBank.Add(Path.GetFileNameWithoutExtension(file), new EffectSound(file));
                }
                catch { }
        }

        #endregion

        #region PlayerManager

        void PlayPool(object name)
        {
            _soundBank[(string)name].Play();
        }

        public void Play(string name)
        {
            if (!_soundBank.ContainsKey(name))
                return;

            if (_poolTask.ContainsKey(name))
                return;

            _poolTask.Add(name, new Thread(() => PlayPool(name)));
            _poolTask[name].Start();
        }

        public void Pause(string name)
        {
            if (!_soundBank.ContainsKey(name))
                return;

            if (_soundBank[name].PlayerStatus == Sound.Status.Play)
                _soundBank[name].Pause();
        }

        public void Resume(string name)
        {
            if (!_soundBank.ContainsKey(name))
                return;
            if (_soundBank[name].PlayerStatus == Sound.Status.Pause)
                _soundBank[name].Play();
        }

        public void Stop(string name)
        {
            if (!_soundBank.ContainsKey(name))
                return;

            _soundBank[name].Stop();
            _poolTask.Remove(name);
        }

        #endregion

        #region PlayerForAll

        public void PauseAll()
        {
            foreach (var thread in _poolTask)
                Pause(thread.Key);
        }

        public void ResumeAll()
        {
            foreach (var thread in _poolTask)
                Resume(thread.Key);
        }

        public void StopAll()
        {
            foreach (var thread in _poolTask)
            {
                thread.Value.Abort();
                _soundBank[thread.Key].Stop();
            }
            _poolTask.Clear();
        }

        #endregion

        void PlaySoundEvent(string type, I3DElement character)
        {
            Play(character.ModelName + type);
        }
    }
}