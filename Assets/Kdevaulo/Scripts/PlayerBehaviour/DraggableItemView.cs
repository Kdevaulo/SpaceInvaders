using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    public sealed class DraggableItemView : MovingItemView, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public UnityEvent<PointerEventData> OnBeginDrag = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnDrag = new UnityEvent<PointerEventData>();
        public UnityEvent<PointerEventData> OnEndDrag = new UnityEvent<PointerEventData>();

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