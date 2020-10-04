namespace PositionerDemo
{
    public class KimbokoXFactory : KimbokoFactory
    {
        public override Kimboko CreateKimboko(int ID, Player player)
        {
            return new KimbokoX(ID, player);
        }
    }

}
