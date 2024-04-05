using UnityEngine;

namespace Kdevaulo.SpaceInvaders.LevelngSystem
{
    [AddComponentMenu(nameof(PositionsProvider) + " in " + nameof(LevelngSystem))]
    public sealed class PositionsProvider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform[] _positionPoints;

        public Vector2[] GetPositions()
        {
            int positionsCount = _positionPoints.Length;

            var positions = new Vector2[positionsCount];

            for (int i = 0; i < positionsCount; i++)
            {
                positions[i] = _positionPoints[i].position;
            }

            return positions;
        }
    }
}