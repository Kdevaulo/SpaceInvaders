using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;

using Zenject;

namespace Kdevaulo.SpaceInvaders.DropBehaviour
{
    public sealed class DropService : IResourceHandler
    {
        [Inject] private DropSettingsData _dropSettings;

        private List<DropView> _instantiatedDrops = new List<DropView>();

        public void Add(Drop dropType, Vector2 modelPosition)
        {
            var drop = _dropSettings.Drops.FirstOrDefault(x => x.DropType == dropType);

            Assert.IsNotNull(drop, $"Couldn't find drop with type - {dropType}");

            var view = Object.Instantiate(drop.View, modelPosition, Quaternion.identity);

            _instantiatedDrops.Add(view);
        }

        void IResourceHandler.Release()
        {
            foreach (var dropView in _instantiatedDrops)
            {
                Object.Destroy(dropView.gameObject);
            }

            _instantiatedDrops.Clear();
        }
    }
}