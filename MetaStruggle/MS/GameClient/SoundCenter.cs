using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Audio;

namespace GameClient.SoundEngine
{
    public class SoundCenter
    {
        private const string DirMusic = "Content\\Musics";
        private const string DirEffect = "Content\\Effects";

        private static SoundCenter _soundCenter;

        private readonly Dictionary<string, Sound> _soundsBank = new Dictionary<string, Sound>();
        private readonly Dictionary<string, Thread> _poolTask = new Dictionary<string, Thread>();

        public bool SoundRunning { get { return _poolTask.Count > 0; } }

        public static SoundCenter Instance
        {
            get { return _soundCenter ?? (_soundCenter = new SoundCenter()); }
        }   

        SoundCenter()
        {
            CheckDirectories();
            LoadMusics();
            LoadEffects();
        }

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
                    _soundsBank.Add(Path.GetFileNameWithoutExtension(file),
                                   new Music(SoundEffect.FromStream(File.Open(file, FileMode.Open))));
                }
                catch (Exception)
                {
                }
        }

        void LoadEffects()
        {
            foreach (var file in Directory.GetFiles(DirEffect))
                try
                {
                    _soundsBank.Add(Path.GetFileNameWithoutExtension(file),
                                   new EffectSound(SoundEffect.FromStream(File.Open(file, FileMode.Open))));
                }
                catch (Exception)
                {
                }

        }

        #endregion LoadingFiles

        #region PlayerManager

        void playPool(object name)
        {
            _soundsBank[(string)name].SoundEffect.Play();
        }

        public void Play(string name)
        {
            if(!_soundsBank.ContainsKey(name))
                return;

            if (_poolTask.ContainsKey(name))
                Stop(name);

            if ((_soundsBank[name] is Music) && !_soundsBank[name].SoundEffect.IsLooped)
                _soundsBank[name].SoundEffect.IsLooped = true;

            _poolTask.Add(name, new Thread(playPool));
            _poolTask[name].Start(name);
        }

        public void Pause(string name)
        {
            if (!_soundsBank.ContainsKey(name))
                return;

            if (_soundsBank[name].SoundEffect.State == SoundState.Playing)
                _soundsBank[name].SoundEffect.Pause();
        }

        public void Resume(string name)
        {
            if (!_soundsBank.ContainsKey(name))
                return;

            if (_soundsBank[name].SoundEffect.State == SoundState.Paused)
                _soundsBank[name].SoundEffect.Resume();
        }

        public void Stop(string name)
        {
            if (!_soundsBank.ContainsKey(name))
                return;

            _soundsBank[name].SoundEffect.Stop();
            _poolTask.Remove(name);
        }

        #endregion PlayerManager

        #region PlayerManagerForAllMusics

        public void PauseAll()
        {
            foreach (KeyValuePair<string, Thread> kvp in _poolTask)
                Pause(kvp.Key);
        }

        public void ResumeAll()
        {
            foreach (KeyValuePair<string, Thread> kvp in _poolTask)
                Resume(kvp.Key);
        }

        public void StopAll()
        {
            foreach (KeyValuePair<string, Thread> kvp in _poolTask)
                _soundsBank[kvp.Key].SoundEffect.Stop();

            _poolTask.Clear();
        }

        #endregion PlayerManagerForAllMusics
    }
}