namespace PositionerDemo
{
    public class NerfStatModificationEffect : StatModificationEffect
    {
        protected const STATMODIFIERTYPE STAT_MODIFIER_TYPE = STATMODIFIERTYPE.NERF;
        private const int EFFECT_ID = 1;
        public NerfStatModificationEffect(int statID, int amountToModify) : base(statID, STAT_MODIFIER_TYPE, amountToModify, EFFECT_ID)
        {
        }
    }
}
