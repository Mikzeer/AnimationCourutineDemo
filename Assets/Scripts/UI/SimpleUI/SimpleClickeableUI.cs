using UnityEngine.EventSystems;

namespace PositionerDemo
{
    public abstract class SimpleClickeableUI : SimpleUI, IPointerDownHandler, IPointerUpHandler
    {
        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {           
        }
    }
}
