using UnityEngine.EventSystems;

namespace PositionerDemo
{
    public abstract class SimpleClickeableUI : SimpleUI, IPointerDownHandler
    {
        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }
    }

}
