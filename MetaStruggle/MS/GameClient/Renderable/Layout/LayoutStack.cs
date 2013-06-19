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

            if (!(e is Menu) && Peek() is Menu)
                GameEngine.SoundCenter.Play("Main Title");
            if (!(Peek() is SceneManager))
                GameEngine.ParticleSystemManager.DestroyAndRemoveAllParticleSystems();
        }

        new public void Push(T e)
        {
            if (!(e is Menu) && Count > 0 && base.Peek() is Menu)
                GameEngine.SoundCenter.Stop("Main Title");
            if (!(e is SceneManager))
                GameEngine.ParticleSystemManager.DestroyAndRemoveAllParticleSystems();
            base.Push(e);
        }

    }
}
