namespace PositionerDemo
{
    public class AttackAbilityEventInfo : AbilityEventInfo
    {
        public DAMAGETYPE attackType { get; set; } // QUE TIPO DE DANO, DIRECTO A LOS PUNTOS DE VIDA O INDERECTO QUE PASA POR LOS MODIFICADORES DE ACTION
        public IOcuppy damageTaker; // QUIEN O QUE ES EL QUE RECIBE DANO // UNIT / PLAYER / BOARD OBJECT
        public IOcuppy attacker; // QUIEN O QUE ES EL ATACANTE // UNIT / PLAYER / BOARD OBJECT
        public int damageAmountDeal; // CANTIDAD DE DANO A REALIZAR

        public AttackAbilityEventInfo(IOcuppy attacker, IOcuppy damageTaker, DAMAGETYPE attackType, int damageAmountDeal)
        {
            this.attacker = attacker;
            this.damageTaker = damageTaker;
            this.attackType = attackType;
            this.damageAmountDeal = damageAmountDeal;
        }
    }
}