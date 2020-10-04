namespace PositionerDemo
{
    public class KimbokoZFactory : KimbokoFactory
    {
        public override Kimboko CreateKimboko(int ID, Player player)
        {
            return new KimbokoZ(ID, player);
        }
    }

}
