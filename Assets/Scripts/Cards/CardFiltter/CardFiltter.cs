
namespace PositionerDemo
{
    [System.Serializable]
    public class CardFiltter
    {
        public int ID { get; protected set; }

        public CardFiltter(int ID)
        {
            this.ID = ID;
        }

        public virtual ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            return cardTarget;
        }
    }
}