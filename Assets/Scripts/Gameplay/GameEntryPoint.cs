using Preparation;
using UnityEngine;

namespace Gameplay
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private Canvas _canvas;

        private GameChecker _gameChecker;
        
        private void Start()
        {
            _gameChecker = new GameChecker();
            var cardCreator = new CardCreator(_gameChecker);
            var gridGenerator = new GridGenerator(cardCreator, _canvas);
            new LevelCreator(_gameSettings, gridGenerator).CreateStartLevel();
        }
    }
}