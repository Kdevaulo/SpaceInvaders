using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Kdevaulo.SpaceInvaders.ScreenBehaviour
{
    public sealed class ScreenService : IInitializable
    {
        [Inject]
        private Camera _camera;

        [Inject]
        private RectTransform _safeZone;

        [Inject]
        private CanvasScaler _canvasScaler;

        private Rect _screenRect;

        private bool _rectCalculated;

        public Vector2 ScreenToWorldPoint(Vector2 eventDataPosition)
        {
            return _camera.ScreenToWorldPoint(eventDataPosition);
        }

        public void UpdateCameraSize()
        {
            var referenceResolution = _canvasScaler.referenceResolution;

            float referenceAR = CalculateAspectRatio(referenceResolution.x, referenceResolution.y);
            float currentAR = CalculateAspectRatio(Screen.width, Screen.height);

            _camera.orthographicSize = _camera.orthographicSize * referenceAR / currentAR;
        }

        public Rect GetScreenRectInUnits(bool recalculate = false)
        {
            if (_rectCalculated && !recalculate)
            {
                return _screenRect;
            }

            float ppu = _canvasScaler.referencePixelsPerUnit;
            var rect = _safeZone.rect;
            var position = _safeZone.anchoredPosition / ppu;

            float yMax = rect.yMax / ppu;
            float yMin = rect.yMin / ppu;
            float sizeY = yMax - yMin;
            float verticalCenter = yMin + sizeY / 2;

            float xMax = rect.xMax / ppu;
            float xMin = rect.xMin / ppu;
            float sizeX = xMax - xMin;
            float horizontalCenter = xMin + sizeX / 2;

            float positionX = position.x + horizontalCenter - sizeX / 2;
            float positionY = position.y + verticalCenter - sizeY / 2;

            _screenRect = new Rect(positionX, positionY, sizeX, sizeY);
            _rectCalculated = true;

            return _screenRect;
        }

        void IInitializable.Initialize()
        {
            UpdateCameraSize();
        }

        private float CalculateAspectRatio(float width, float height)
        {
            return width / height;
        }
    }
}