using Kdevaulo.SpaceInvaders.BulletBehaviour;
using Kdevaulo.SpaceInvaders.DropBehaviour;
using Kdevaulo.SpaceInvaders.LevelSystem;
using Kdevaulo.SpaceInvaders.PlayerBehaviour;

using UnityEngine;

using Zenject;

namespace Kdevaulo.SpaceInvaders.Installers
{
    [CreateAssetMenu(fileName = nameof(ScriptableInstaller),
        menuName = nameof(Installers) + "/" + nameof(ScriptableInstaller))]
    public sealed class ScriptableInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private DropSettingsData _dropSettings;
        [SerializeField] private BulletSettingsData _bulletSettingsData;
        [SerializeField] private PlayerSettingsData _playerSettings;

        [SerializeField] private LevelSettingsData[] _levelSettings;

        public override void InstallBindings()
        {
            Container.Bind<DropSettingsData>().FromInstance(_dropSettings);
            Container.Bind<BulletSettingsData>().FromInstance(_bulletSettingsData);
            Container.Bind<PlayerSettingsData>().FromInstance(_playerSettings);
            Container.Bind<LevelSettingsData[]>().FromInstance(_levelSettings);
        }
    }
}