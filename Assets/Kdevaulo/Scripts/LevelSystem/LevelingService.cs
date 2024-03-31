using System.Collections.Generic;

using Kdevaulo.SpaceInvaders.EnemiesBehaviour;

using UnityEngine.Assertions;

using Zenject;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    public sealed class LevelingService
    {
        [Inject]
        private LevelSettings[] _levelSettings;

        [Inject]
        private EnemiesFactory _enemiesFactory;

        [Inject]
        private EnemiesController _enemiesController;

        [Inject]
        private PositionsProvider _positionsProvider;

        private int _currentLevel;

        public void Restart()
        {
            // todo: clear previous stage
            InitializeLevel(_currentLevel);
        }

        public void StartFromTheBeginning()
        {
            // todo: clear previous stage
            _currentLevel = 0;
            InitializeLevel(_currentLevel);
        }

        public void StartNewStage()
        {
            // todo: clear previous stage
            ++_currentLevel;
            InitializeLevel(_currentLevel);
        }

        private void InitializeLevel(int levelIndex)
        {
            Assert.IsTrue(_levelSettings.Length > 0, "Level settings count == 0");

            int settingsIndex = levelIndex % _levelSettings.Length;
            var currentSettings = _levelSettings[settingsIndex];
            var enemiesSettings = currentSettings.EnemiesSettings;

            var enemies = new List<EnemyModel>();

            foreach (var settings in enemiesSettings)
            {
                for (int i = 0; i < settings.Count; i++)
                {
                    var model = _enemiesFactory.Create(settings);
                    enemies.Add(model);
                }
            }

            PlaceEnemies(enemies);

            //todo: fix behaviour, remove peer-to-peer reference
            _enemiesController.Initialize(enemies, currentSettings.EnemiesStartSpeed, currentSettings.EnemiesSpeedStep,
                currentSettings.VerticalStep);
        }

        private void PlaceEnemies(List<EnemyModel> enemies)
        {
            var positions = _positionsProvider.GetPositions();

            int enemiesCount = enemies.Count;

            Assert.IsTrue(enemiesCount <= positions.Length, "Positions count is less than enemies count");

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Position = positions[i];
            }
        }
    }
}