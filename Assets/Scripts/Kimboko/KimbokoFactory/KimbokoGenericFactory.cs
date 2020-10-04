namespace PositionerDemo
{
    public class KimbokoGenericFactory : KimbokoFactoryGeneric
    {
        public override Kimboko CreateKimboko(int ID, Player player, KimbokoPropertiesScriptableObject KimSO)
        {
            return new KimbokoGeneric(ID, player, KimSO);
        }
    }

}
