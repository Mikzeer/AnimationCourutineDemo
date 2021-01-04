namespace PositionerDemo
{
    public class BuffStatModificationEffect : StatModificationEffect
    {
        protected const STATMODIFIERTYPE STAT_MODIFIER_TYPE = STATMODIFIERTYPE.BUFF;
        private const int EFFECT_ID = 0;
        public BuffStatModificationEffect(int statID, int amountToModify) : base(statID, STAT_MODIFIER_TYPE, amountToModify, EFFECT_ID)
        {
        }
    }
}
