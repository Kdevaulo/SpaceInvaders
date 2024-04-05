using System.Linq;

using UnityEngine;

namespace Kdevaulo.SpaceInvaders
{
    public static class Utilities
    {
        /// <summary>
        /// Checks if an object extends beyond the provided boundaries
        /// </summary>
        /// <returns>True if at least one corner of the object extends beyond the Rect</returns>
        public static bool IsOutOfRect(Vector2 position, float halfVerticalSize, float halfHorizontalSize, Rect bounds)
        {
            return position.x - halfHorizontalSize < bounds.xMin
                   || position.x + halfHorizontalSize > bounds.xMax
                   || position.y - halfVerticalSize < bounds.yMin
                   || position.y + halfVerticalSize > bounds.yMax;
        }

        /// <summary>
        /// Checks for matches between all tags in the list and the component tag
        /// </summary>
        /// <returns>True if at least one of the tags matches the behaviour tag</returns>
        public static bool IfAnyTag(this Component component, string[] tags)
        {
            return tags.Any(component.CompareTag);
        }

        public static bool IsOutOfBoundsHorizontal(float positionX, float halfSize, Rect rect)
        {
            return IsOutOfBounds(positionX, halfSize, rect.xMin, rect.xMax);
        }

        public static bool IsOutOfBoundsVertical(float positionY, float halfSize, Rect rect)
        {
            return IsOutOfBounds(positionY, halfSize, rect.yMin, rect.yMax);
        }

        private static bool IsOutOfBounds(float value, float halfSize, float min, float max)
        {
            return value - halfSize < min || value + halfSize > max;
        }
    }
}