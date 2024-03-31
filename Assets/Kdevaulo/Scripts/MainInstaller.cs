using Kdevaulo.SpaceInvaders.EnemiesBehaviour;
using Kdevaulo.SpaceInvaders.LevelSystem;
using Kdevaulo.SpaceInvaders.MenuBehaviour;
using Kdevaulo.SpaceInvaders.PauseBehaviour;
using Kdevaulo.SpaceInvaders.ScoreBehaviour;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

using DisposableManager = Kdevaulo.SpaceInvaders.DisposableBehaviour.DisposableManager;

namespace Kdevaulo.SpaceInvaders
{
    [AddComponentMenu(nameof(MainInstaller) + " in " + nameof(SpaceInvaders))]
    public sealed class MainInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private PauseMenuView _pauseMenuView;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private EnemyView _enemyView;

        [SerializeField] private PositionsProvider _positionsProvider;

        [SerializeField] private LevelSettings _levelSettings;

        [SerializeField] private DisposableManager _disposableManager;

        private void OnDestroy()
        {
            _disposableManager.DisposeAll();
        }

        public override void InstallBindings()
        {
            Container.Bind<Rect>().FromInstance(_canvas.rect);
            Container.Bind<CanvasScaler>().FromInstance(_canvasScaler);

            Container.Bind<ScoreView>().FromInstance(_scoreView);
            Container.Bind<PauseView>().FromInstance(_pauseView);
            Container.Bind<EnemyView>().FromInstance(_enemyView);
            Container.Bind<PauseMenuView>().FromInstance(_pauseMenuView);

            Container.Bind<LevelSettings>().FromInstance(_levelSettings);

            Container.Bind<PositionsProvider>().FromInstance(_positionsProvider);
            Container.Bind<EnemiesFactory>().AsSingle();

            Container.Bind<PauseService>().AsSingle();
            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<LevelingService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PauseController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesController>().AsSingle();
        }
    }
}