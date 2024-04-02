using Kdevaulo.SpaceInvaders.BulletBehaviour;
using Kdevaulo.SpaceInvaders.EnemiesBehaviour;
using Kdevaulo.SpaceInvaders.LevelSystem;
using Kdevaulo.SpaceInvaders.MenuBehaviour;
using Kdevaulo.SpaceInvaders.PauseBehaviour;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;
using Kdevaulo.SpaceInvaders.ScoreBehaviour;

using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kdevaulo.SpaceInvaders
{
    [AddComponentMenu(nameof(MainInstaller) + " in " + nameof(SpaceInvaders))]
    public sealed class MainInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private RectTransform _canvas;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private Camera _camera;

        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private PauseMenuView _pauseMenuView;
        [SerializeField] private MovingItemView _bulletView;

        [SerializeField] private PositionsProvider _positionsProvider;

        [SerializeField] private LevelSettings _levelSettings;

        public override void InstallBindings()
        {
            Container.Bind<Rect>().FromInstance(_canvas.rect);
            Container.Bind<CanvasScaler>().FromInstance(_canvasScaler);
            Container.Bind<Camera>().FromInstance(_camera);

            Container.Bind<ScoreView>().FromInstance(_scoreView);
            Container.Bind<PauseView>().FromInstance(_pauseView);
            Container.Bind<PauseMenuView>().FromInstance(_pauseMenuView);
            Container.Bind<MovingItemView>().FromInstance(_bulletView);

            Container.Bind<LevelSettings>().FromInstance(_levelSettings);

            Container.Bind<PositionsProvider>().FromInstance(_positionsProvider);
            Container.Bind<EnemiesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletPool>().AsSingle();

            Container.Bind<PauseService>().AsSingle();
            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<LevelingService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PauseController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelController>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesController>().AsSingle();
        }
    }
}