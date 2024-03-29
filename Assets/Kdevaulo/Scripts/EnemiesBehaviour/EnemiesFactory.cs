using UnityEngine;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesFactory
    {
        public EnemyModel Create(EnemySettings settings)
        {
            var view = Object.Instantiate(settings.View);
            var model = new EnemyModel(view, settings);
            return model;
        }
    }
}