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
                    GameEngine.ParticleEngine.ResumeAll();
                    GameEngine.ParticleEngine.Draw();
                }
            }
        }

        new public void Push(T e)
        {
            if (Count > 0)
            {
                if (!(e is Menu) && Peek() is Menu)
                {  GameEngine.SoundCenter.Stop("Main Title");
                    if (e is SceneManager)
                        GameEngine.SoundCenter.Play((e as SceneManager).MapName);
                }
                else if (e is Menu && Peek() is SceneManager)
                {
                    GameEngine.SoundCenter.PauseAll();
                    GameEngine.SoundCenter.Play("Main Title");
                }
                if (!(e is SceneManager) && Peek() is SceneManager)
                    GameEngine.ParticleEngine.PauseAll();
            }
            base.Push(e);
        }

    }
}
