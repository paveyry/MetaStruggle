using System.Collections.Generic;
using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.Scene;

namespace GameClient.Renderable.Layout
{
    public class LayoutStack<T> : Stack<T>
    {
        new public void Pop()
        {
            var e = Peek();
            base.Pop();

            if (Count > 0)
            {
                if (!(e is Menu) && Peek() is Menu)
                    GameEngine.SoundCenter.Play("Main Title");
                else if (Peek() is SceneManager)
                {
                    GameEngine.SoundCenter.Stop("Main Title");
                    GameEngine.SoundCenter.ResumeAll();
                    GameEngine.ParticleEngine.SetDrawableParticles();
                }
            }
        }

        new public void Push(T e)
        {
            if (Count > 0)
            {
                if (!(e is Menu) && Peek() is Menu)
                    GameEngine.SoundCenter.Stop("Main Title");
                else if (e is Menu && Peek() is SceneManager)
                {
                    GameEngine.SoundCenter.PauseAll();
                    GameEngine.SoundCenter.Play("Main Title");
                }
                else if (!(e is SceneManager) && Peek() is SceneManager)
                    GameEngine.ParticleEngine.RemoveAndDestroyAll();
                else if (e is SceneManager )
                    GameEngine.ParticleEngine.SetDrawableParticles();
            }
            base.Push(e);
        }

        void SetParticleEngine(T e, T peek)
        {
            
        }

    }
}
