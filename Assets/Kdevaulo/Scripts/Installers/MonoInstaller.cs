using Kdevaulo.SpaceInvaders.DropBehaviour;
using Kdevaulo.SpaceInvaders.EnemiesBehaviour;
using Kdevaulo.SpaceInvaders.LevelngSystem;
using Kdevaulo.SpaceInvaders.PauseSystem;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;
using Kdevaulo.SpaceInvaders.ProjectileBehaviour;
using Kdevaulo.SpaceInvaders.ResourceSystem;
using Kdevaulo.SpaceInvaders.ScoreSystem;
using Kdevaulo.SpaceInvaders.ScreenSystem;

using UnityEngine;
using UnityEngine.UI;

namespace Kdevaulo.SpaceInvaders.Installers
{
    [AddComponentMenu(nameof(MonoInstaller) + " in " + nameof(Installers))]
    public sealed class MonoInstaller : Zenject.MonoInstaller
    {
        [Header("References")]
        [SerializeField] private Camera _camera;
        [SerializeField] private RectTransform _safeZone;
        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private MovingItemView _bulletView;

        [SerializeField] private PositionsProvider _positionsProvider;

        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private PauseMenuView _pauseMenuView;

        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(_camera);
            Container.Bind<RectTransform>().FromInstance(_safeZone);
            Container.Bind<CanvasScaler>().FromInstance(_canvasScaler);

            Container.Bind<ScoreView>().FromInstance(_scoreView);
            Container.Bind<PauseView>().FromInstance(_pauseView);
            Container.Bind<MovingItemView>().FromInstance(_bulletView);
            Container.Bind<PauseMenuView>().FromInstance(_pauseMenuView);

            Container.Bind<LevelingModel>().AsSingle();
            Container.Bind<EnemiesFactory>().AsSingle();
            Container.Bind<PositionsProvider>().FromInstance(_positionsProvider);
            Container.BindInterfacesAndSelfTo<ProjectilePool>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceManger>().AsSingle();

            Container.Bind<PauseService>().AsSingle();
            Container.Bind<ScoreService>().AsSingle();
            Container.Bind<LevelingService>().AsSingle();
            Container.BindInterfacesAndSelfTo<DropService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScreenService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ProjectileService>().AsSingle();

            Container.BindInterfacesAndSelfTo<PauseController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnemiesController>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelingController>().AsSingle();
        }
    }
}