namespace PositionerDemo
{
    public class KimbokoYFactory : KimbokoFactory
    {
        public override Kimboko CreateKimboko(int ID, Player player)
        {
            return new KimbokoY(ID, player);
        }
    }

}
