namespace PositionerDemo
{
    public abstract class TargetOcuppyStateFiltter : CardFiltter
    {
        bool isOcupied;
        public TargetOcuppyStateFiltter(bool isOcupied, int ID) : base(ID)
        {
            this.isOcupied = isOcupied;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            Tile tile = (Tile)cardTarget;
            if (tile == null) return null;
            if (tile.IsOccupied() != isOcupied) return null;
            return base.CheckTarget(cardTarget);
        }
    }
}