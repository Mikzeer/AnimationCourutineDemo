using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class DefenseAbilityButtonExecution : SpecificAbilityExecution
        {
            public string name { get; private set; } = "DEFENSE";
            IOcuppy actualOccupier;
            GameMachine gameCreator;
            AbilityButtonCreationUI abilityButtonCreationUI;

            public DefenseAbilityButtonExecution(IOcuppy actualOccupier, GameMachine gameCreator, AbilityButtonCreationUI abilityButtonCreationUI)
            {
                this.actualOccupier = actualOccupier;
                this.gameCreator = gameCreator;
                this.abilityButtonCreationUI = abilityButtonCreationUI;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;
                //if (actualUnit.defendAbility.OnTryExecute())
                //{
                //    //gameCreator.ChangeState(new GameStateMachine.AttackState(actualUnit, gameCreator, gameCreator.currentState));
                //    actualUnit.defendAbility.Perform();
                //}

                abilityButtonCreationUI.ClearAbilityButtons();
            }
        }

    }
}