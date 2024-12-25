using UnityEngine;

namespace Gameplay
{
    public class GameChecker
    {
        public void PassLevel()
        {
            // if levelCreator.HasLevels => levelCreator.NextLevel
            // else => win
            
            Debug.Log("Pass Level");
        }

        public void FailLevel()
        {
            Debug.Log("Fail Level");
        }
    }
}