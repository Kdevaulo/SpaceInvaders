using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kdevaulo.SpaceInvaders
{
    public sealed class ScreenUtilities
    {
        [Inject]
        private Camera _camera;

        [Inject]
        private RectTransform _canvasRectTransform;

        [Inject]
        private CanvasScaler _canvasScaler;

        public Rect GetScreenRectInUnits()
        {
            float ppu = _canvasScaler.referencePixelsPerUnit;
            var rect = _canvasRectTransform.rect;
            var position = _canvasRectTransform.anchoredPosition / ppu;

            float maxY = rect.yMax / ppu;
            float minY = rect.yMin / ppu;
            float sizeY = maxY - minY;
            float verticalCenter = minY + sizeY / 2;

            float maxX = rect.xMax / ppu;
            float minX = rect.xMin / ppu;
            float sizeX = maxX - minX;
            float horizontalCenter = minX + sizeX / 2;

            float positionX = position.x + horizontalCenter - sizeX / 2;
            float positionY = position.y + verticalCenter - sizeY / 2;

            return new Rect(positionX, positionY, sizeX, sizeY);
        }

        public Vector2 ScreenToWorldPoint(Vector2 eventDataPosition)
        {
            return _camera.ScreenToWorldPoint(eventDataPosition);
        }

        public bool IsOutOfRect(Vector2 position, float halfVerticalSize, float halfHorizontalSize, Rect bounds)
        {
            return position.x - halfHorizontalSize < bounds.xMin
                   || position.x + halfHorizontalSize > bounds.xMax
                   || position.y - halfVerticalSize < bounds.yMin
                   || position.y + halfVerticalSize > bounds.yMax;
        }
    }
}