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
        [SerializeField] private DropSettings _dropSettings;
        [SerializeField] private BulletSettings _bulletSettings;
        [SerializeField] private PlayerSettings _playerSettings;

        [SerializeField] private LevelSettings[] _levelSettings;

        public override void InstallBindings()
        {
            Container.Bind<DropSettings>().FromInstance(_dropSettings);
            Container.Bind<BulletSettings>().FromInstance(_bulletSettings);
            Container.Bind<PlayerSettings>().FromInstance(_playerSettings);
            Container.Bind<LevelSettings[]>().FromInstance(_levelSettings);
        }
    }
}