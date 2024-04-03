using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class DraggableItemView : MovingItemView, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [field: SerializeField] public float VerticalSize { get; private set; }
        [field: SerializeField] public float HorizontalSize { get; private set; }

        public UnityEvent<PointerEventData> OnBeginDrag = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnDrag = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnEndDrag = new UnityEvent<PointerEventData>();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gameObject.transform.position, new Vector2(HorizontalSize, VerticalSize));
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDrag.Invoke(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag.Invoke(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag.Invoke(eventData);
        }
    }
}