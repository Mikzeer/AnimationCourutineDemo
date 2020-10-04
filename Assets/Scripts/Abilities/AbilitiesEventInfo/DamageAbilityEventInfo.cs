namespace PositionerDemo
{
    public class DamageAbilityEventInfo : AbilityEventInfo
    {
        public DAMAGETYPE damageType { get; set; } // QUE TIPO DE DANO, DIRECTO A LOS PUNTOS DE VIDA O INDERECTO QUE PASA POR LOS MODIFICADORES DE ACTION
        public IOcuppy damageTaker; // QUIEN O QUE ES EL QUE RECIBE DANO // UNIT / PLAYER / BOARD OBJECT
        public IOcuppy attacker; // QUIEN O QUE ES EL ATACANTE // UNIT / PLAYER / BOARD OBJECT
        public int damageAmountRecived; // CANTIDAD DE DANO A REALIZAR

        public DamageAbilityEventInfo(IOcuppy attacker, IOcuppy damageTaker, DAMAGETYPE damageType, int damageAmountRecived)
        {
            this.attacker = attacker;
            this.damageTaker = damageTaker;
            this.damageType = damageType;
            this.damageAmountRecived = damageAmountRecived;
        }
    }
}
