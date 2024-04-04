using System.Collections.Generic;

using Kdevaulo.SpaceInvaders.EnemiesBehaviour;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Kdevaulo.SpaceInvaders.LevelSystem
{
    public sealed class LevelingService
    {
        [Inject]
        private IResourceHandler[] _resourceHandlers;

        [Inject]
        private LevelSettings[] _levelSettings;

        [Inject]
        private EnemiesFactory _enemiesFactory;

        [Inject]
        private EnemiesController _enemiesController;

        [Inject]
        private PlayerController _playerController;

        [Inject]
        private PositionsProvider _positionsProvider;

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
            ++_currentLevel;
            InitializeLevel(_currentLevel);
        }

        public void ClearLevel()
        {
            foreach (var handler in _resourceHandlers)
            {
                handler.Release();
            }
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

            var playerModel = CreatePlayer(currentSettings.PlayerSettings);

            //todo: fix behaviour, remove peer-to-peer reference
            _enemiesController.Initialize(enemies, currentSettings.EnemiesMoveDelayBounds,
                currentSettings.EnemyMovementPattern,
                currentSettings.VerticalStep);

            //todo: fix behaviour, remove peer-to-peer reference
            _playerController.Initialize(playerModel);
        }

        private PlayerModel CreatePlayer(PlayerSettings settings)
        {
            var view = Object.Instantiate(settings.View);
            var model = new PlayerModel(view, settings);
            return model;
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