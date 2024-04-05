using UnityEngine.Assertions;

using Zenject;

namespace Kdevaulo.SpaceInvaders.LevelngSystem
{
    public sealed class LevelingService
    {
        [Inject] private LevelSettingsData[] _levelSettings;

        [Inject] private LevelingModel _levelingModel;

        private int _currentLevel;

        public void Restart()
        {
            ClearLevel();
            InitializeLevel(_currentLevel);
        }

        public void StartFromTheBeginning()
        {
            ClearLevel();
            Initialize();
        }

        public void Initialize()
        {
            _currentLevel = 0;
            InitializeLevel(_currentLevel);
        }

        public void StartNewStage()
        {
            ClearLevel();
            InitializeLevel(++_currentLevel);
        }

        public void ClearLevel()
        {
            _levelingModel.ClearLevel.Execute();
        }

        private void InitializeLevel(int levelIndex)
        {
            Assert.IsTrue(_levelSettings.Length > 0, "Level settings count == 0");

            int settingsIndex = levelIndex % _levelSettings.Length;
            var currentSettings = _levelSettings[settingsIndex];

            // note: I force a notification, as setting the same value does not notify subscribers
            _levelingModel.LevelSettings.SetValueAndForceNotify(currentSettings);
        }
    }
}