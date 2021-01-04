namespace PositionerDemo
{
    public class StatModificationEffect : CardEffect
    {
        protected int statID;
        protected STATMODIFIERTYPE statModifierType;
        protected int amountToModify;
        public StatModificationEffect(int statID, STATMODIFIERTYPE statModifierType, int amountToModify, int ID) : base(ID)
        {
            this.statID = statID;
            this.amountToModify = amountToModify;
            this.statModifierType = statModifierType;
        }

        public override void OnCardEffectApply(ICardTarget cardTarget)
        {
            IOcuppy ocuppier = cardTarget.GetOcuppy();
            if (ocuppier == null) return;

            switch (statModifierType)
            {
                case STATMODIFIERTYPE.BUFF:
                    // SI ES VIDA NUNCA SE PUEDE CURAR MAS QUE EL MAXIMO
                    if (statID == 0)
                    {
                        // max 4 actual 3 to add 2 
                        // 4 - 3 = 1   2 > 1   2 = 1
                        int maxDiference = ocuppier.Stats[statID].MaxStatValue - ocuppier.Stats[statID].ActualStatValue;
                        if (amountToModify > maxDiference)
                        {
                            amountToModify = maxDiference;
                        }
                    }
                    else
                    {
                        // S ES OTRO STAT VAMOS A AUMENTAR EL MAXIMO
                        int diferenceToAdd = ocuppier.Stats[statID].MaxStatValue - ocuppier.Stats[statID].ActualStatValue;
                        if (amountToModify > diferenceToAdd)
                        {
                            // max 2 actual 2 diference to add = 0

                            // PERO SI ES MOVE RANGE ATTACK RANGE NO PUEDE SER MAS QUE 3
                            if (statID == 1 || statID == 2)
                            {
                                if (ocuppier.Stats[statID].MaxStatValue > 2)
                                {
                                    amountToModify = 1;
                                    return;
                                }
                            }
                            int newAmount = ocuppier.Stats[statID].MaxStatValue + amountToModify - diferenceToAdd;

                            ocuppier.Stats[statID].ChangeMaxStatValue(newAmount);

                        }
                    }

                    break;
                case STATMODIFIERTYPE.NERF:
                    int difference = ocuppier.Stats[statID].ActualStatValue - amountToModify;
                    if (difference < 0)
                    {
                        // attack 1, le va a sacar 2 == -1  2 +-1 = 1
                        amountToModify += difference; 
                    }
                    break;
                default:
                    break;
            }

            StatModification statModification = new StatModification(ocuppier, ocuppier.Stats[statID], amountToModify, statModifierType);
            ocuppier.Stats[statID].AddStatModifier(statModification);
            ocuppier.Stats[statID].ApplyModifications();
        }
    }
}
