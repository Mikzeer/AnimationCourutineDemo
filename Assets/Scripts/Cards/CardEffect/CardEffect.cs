namespace PositionerDemo
{
    [System.Serializable]
    public class CardEffect
    {
        public int ID { get; protected set; }

        public CardEffect(int ID)
        {
            this.ID = ID;
        }

        public virtual void OnCardEffectApply(ICardTarget cardTarget)
        {          
        }
    }
}
