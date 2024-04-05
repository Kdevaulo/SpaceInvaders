using System;
using System.Collections.Generic;

using Kdevaulo.SpaceInvaders.EnemiesBehaviour;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

using Object = UnityEngine.Object;

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

            var enemyArray = GetValue(enemies, currentSettings.ColumnsCount);

            PlaceEnemies(enemies);

            var playerModel = CreatePlayer(currentSettings.PlayerSettings);

            //todo: fix behaviour, remove peer-to-peer reference
            _enemiesController.Initialize(enemies, enemyArray, currentSettings.EnemiesMoveDelayBounds,
                currentSettings.EnemyMovementPattern, currentSettings.VerticalStep, currentSettings.EnemyShootDelay,
                currentSettings.EnemiesBulletDirection);

            //todo: fix behaviour, remove peer-to-peer reference
            _playerController.Initialize(playerModel);
        }

        private EnemyModel[,] GetValue(List<EnemyModel> enemies, int columns)
        {
            Assert.IsTrue(columns > 0, "Enemy columns == 0");

            int enemiesCount = enemies.Count;
            int rows = (int) MathF.Ceiling(enemiesCount / (float) columns);
            int index = 0;

            var models = new EnemyModel[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var enemyModel = enemies[index++];
                    enemyModel.Index = new Vector2Int(row, column);
                    models[row, column] = enemyModel;

                    if (index >= enemiesCount)
                    {
                        return models;
                    }
                }
            }

            return models;
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