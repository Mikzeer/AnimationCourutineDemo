namespace PositionerDemo
{
    public class ChangeStatModificationEffect : StatModificationEffect
    {
        protected const STATMODIFIERTYPE STAT_MODIFIER_TYPE = STATMODIFIERTYPE.CHANGE;
        private const int EFFECT_ID = 2;
        public ChangeStatModificationEffect(int statID, int amountToModify) : base(statID, STAT_MODIFIER_TYPE, amountToModify, EFFECT_ID)
        {
        }
    }
}
