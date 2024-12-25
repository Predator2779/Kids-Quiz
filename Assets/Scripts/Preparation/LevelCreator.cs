namespace Preparation
{
    public class LevelCreator
    {
        private readonly GridGenerator _gridGenerator;
        private readonly LevelParameters[] _levels;
        private int _currentLevel;

        public LevelCreator(GameSettings gameSettings, GridGenerator gridGenerator)
        {
            _gridGenerator = gridGenerator;
            _levels = gameSettings.Levels;
        }

        public void CreateStartLevel()
        {
            Create(_levels[0]);
        }

        public void NextLevel()
        {
            Clear();
            Create(_levels[_currentLevel++]);
        }

        public bool HasLevels()
        {
            return false;
        }
        
        private void Create(LevelParameters level)
        {
            _gridGenerator.Generate(level);
        }

        private void Clear()
        {
            // очищаем уровень
        }
    }
}