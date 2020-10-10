using UnityEngine;

namespace PositionerDemo
{
    public abstract class CardFiltterScriptableObject : ScriptableObject
    {
        public virtual ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            return cardTarget;
        }
    }

}
