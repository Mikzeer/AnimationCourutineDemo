using System.Collections.Generic;

namespace PositionerDemo
{
    public class TargetAbilityExecutionStateFiltter : CardFiltter
    {
        bool hasExecute;
        int abilityID;
        List<ABILITYEXECUTIONSTATUS> actionStatus;
        private const int FILTTER_ID = 44;
        ABILITYTYPE abilityType;
        public TargetAbilityExecutionStateFiltter(int abilityID, List<ABILITYEXECUTIONSTATUS> actionStatus) : base(FILTTER_ID)
        {
            abilityType = (ABILITYTYPE)abilityID;
            this.actionStatus = actionStatus;
            this.abilityID = abilityID;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            IOcuppy occupier = cardTarget.GetOcuppy();
            if (occupier == null) return null;
            for (int i = 0; i < actionStatus.Count; i++)
            {
                if (occupier.Abilities[abilityType].actionStatus != actionStatus[i])
                {
                    return null;
                }
            }
            return base.CheckTarget(cardTarget);
        }
    }
}