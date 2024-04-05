using Kdevaulo.SpaceInvaders.DropBehaviour;
using Kdevaulo.SpaceInvaders.LevelngSystem;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;
using Kdevaulo.SpaceInvaders.ProjectileBehaviour;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.Installers
{
    [CreateAssetMenu(fileName = nameof(ScriptableInstaller),
        menuName = nameof(Installers) + "/" + nameof(ScriptableInstaller))]
    public sealed class ScriptableInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private DropSettingsData _dropSettings;
        [SerializeField] private PlayerSettingsData _playerSettings;
        [SerializeField] private ProjectileSettingsData _projectileSettingsData;

        [SerializeField] private LevelSettingsData[] _levelSettings;

        public override void InstallBindings()
        {
            Container.Bind<DropSettingsData>().FromInstance(_dropSettings);
            Container.Bind<PlayerSettingsData>().FromInstance(_playerSettings);
            Container.Bind<ProjectileSettingsData>().FromInstance(_projectileSettingsData);

            Container.Bind<LevelSettingsData[]>().FromInstance(_levelSettings);
        }
    }
}