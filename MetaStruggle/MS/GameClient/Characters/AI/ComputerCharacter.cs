using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;

namespace GameClient.Characters.AI
{
    public class ComputerCharacter
    {
        SceneManager SceneManager { get; set; }
        byte Handicap { get; set; }
        byte Level { get; set; }

        public ComputerCharacter(SceneManager sceneManager, byte handicap, byte level)
        {
            SceneManager = sceneManager;
            Handicap = handicap;
            Level = level;
        }

        public bool GetMovement(Movement movement) //
        {
            return (movement == Movement.Left);
        }

        public void Update(GameTime gameTime) //
        {
            
        }
    }
}
