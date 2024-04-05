using UniRx;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Kdevaulo.SpaceInvaders.PlayerBehaviour
{
    [AddComponentMenu(nameof(DraggableItemView) + " in " + nameof(PlayerBehaviour))]
    public sealed class DraggableItemView : MovingItemView, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [field: SerializeField] public float VerticalSize { get; private set; }
        [field: SerializeField] public float HorizontalSize { get; private set; }

        public ReactiveCommand<PointerEventData> OnDrag = new ReactiveCommand<PointerEventData>();
        public ReactiveCommand<PointerEventData> OnEndDrag = new ReactiveCommand<PointerEventData>();
        public ReactiveCommand<PointerEventData> OnBeginDrag = new ReactiveCommand<PointerEventData>();

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(gameObject.transform.position, new Vector2(HorizontalSize, VerticalSize));
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDrag.Execute(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag.Execute(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag.Execute(eventData);
        }
    }
}