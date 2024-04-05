using System.Collections.Generic;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders.EnemiesBehaviour
{
    public sealed class EnemiesFactory
    {
        public List<EnemyModel> Create(EnemySettings[] settings)
        {
            var models = new List<EnemyModel>();

            foreach (var data in settings)
            {
                int itemsCount = data.Count;

                for (int i = 0; i < itemsCount; i++)
                {
                    var view = Object.Instantiate(data.View);
                    var model = new EnemyModel(view, data);

                    models.Add(model);
                }
            }

            return models;
        }
    }
}