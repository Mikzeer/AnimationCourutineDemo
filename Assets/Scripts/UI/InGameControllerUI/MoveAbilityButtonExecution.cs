using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class MoveAbilityButtonExecution : SpecificAbilityExecution
        {
            IOcuppy actualOccupier;
            IGame gameCreator;
            public string name { get; private set; } = "MOVE";
            public MoveAbilityButtonExecution(IOcuppy actualOccupier, IGame gameCreator)
            {
                this.gameCreator = gameCreator;
                this.actualOccupier = actualOccupier;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;                
                //actualUnit.moveAbility.Set(gameCreator);
                //if (actualUnit.moveAbility.OnTryExecute())
                //{
                //    gameCreator.ChangeState(new GameStateMachine.MoveState(actualUnit, gameCreator, gameCreator.currentState));
                //}
            }
        }

    }
}