namespace PositionerDemo
{
    public class DefendAbilityEventInfo : AbilityEventInfo
    {
        public IOcuppy defender; // QUIEN SE MURIO ID
        public IOcuppy attacker; // QUIEN LO MATO ID
        public int damageAmountDefended; // CANTIDAD DE DANO A defendida
        public DefendAbilityEventInfo(IOcuppy killer, IOcuppy dieOccupy, int damageAmountDefended)
        {
            this.attacker = killer;
            this.defender = dieOccupy;
            this.damageAmountDefended = damageAmountDefended;
        }
    }
}
