using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

namespace Kdevaulo.SpaceInvaders
{
    public static class ArrayUtilities<T>
    {
        public static T[,] PackItems(List<T> enemies, int columns, Action<T, Vector2Int> indexCallback)
        {
            Assert.IsTrue(columns > 0, "Columns count == 0");

            int enemiesCount = enemies.Count;
            int rows = (int) MathF.Ceiling(enemiesCount / (float) columns);
            int index = 0;

            var models = new T[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var item = enemies[index++];

                    indexCallback.Invoke(item, new Vector2Int(row, column));

                    models[row, column] = item;

                    if (index >= enemiesCount)
                    {
                        return models;
                    }
                }
            }

            return models;
        }
    }
}