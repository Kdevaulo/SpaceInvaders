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
        private Rect _canvasRect;

        [Inject]
        private CanvasScaler _canvasScaler;

        public Rect GetScreenRectInUnits()
        {
            float ppu = _canvasScaler.referencePixelsPerUnit;

            float maxY = _canvasRect.yMax / ppu;
            float minY = _canvasRect.yMin / ppu;
            float sizeY = maxY - minY;
            float verticalCenter = minY + sizeY / 2;

            float maxX = _canvasRect.xMax / ppu;
            float minX = _canvasRect.xMin / ppu;
            float sizeX = maxX - minX;
            float horizontalCenter = minX + sizeX / 2;

            return new Rect(horizontalCenter - sizeX / 2, verticalCenter - sizeY / 2, sizeX, sizeY);
        }

        public Vector2 ScreenToWorldPoint(Vector2 eventDataPosition)
        {
            return _camera.ScreenToWorldPoint(eventDataPosition);
        }
    }
}