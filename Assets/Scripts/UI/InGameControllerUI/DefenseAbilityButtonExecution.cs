using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class DefenseAbilityButtonExecution : SpecificAbilityExecution
        {
            public string name { get; private set; } = "DEFENSE";
            IOcuppy actualOccupier;
            IGame gameCreator;

            public DefenseAbilityButtonExecution(IOcuppy actualOccupier, IGame gameCreator)
            {
                this.actualOccupier = actualOccupier;
                this.gameCreator = gameCreator;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;
                //if (actualUnit.defendAbility.OnTryExecute())
                //{
                //    //gameCreator.ChangeState(new GameStateMachine.AttackState(actualUnit, gameCreator, gameCreator.currentState));
                //    actualUnit.defendAbility.Perform();
                //}
            }
        }

    }
}