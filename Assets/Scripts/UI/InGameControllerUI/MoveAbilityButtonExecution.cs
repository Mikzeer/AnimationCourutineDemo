using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class MoveAbilityButtonExecution : SpecificAbilityExecution
        {
            IOcuppy actualOccupier;
            GameMachine gameCreator;
            AbilityButtonCreationUI abilityButtonCreationUI;
            public string name { get; private set; } = "MOVE";
            public MoveAbilityButtonExecution(IOcuppy actualOccupier, GameMachine gameCreator, AbilityButtonCreationUI abilityButtonCreationUI)
            {
                this.gameCreator = gameCreator;
                this.actualOccupier = actualOccupier;
                this.abilityButtonCreationUI = abilityButtonCreationUI;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;
                //actualUnit.moveAbility.Set(gameCreator);
                //if (actualUnit.moveAbility.OnTryExecute())
                //{
                //    gameCreator.ChangeState(new GameStateMachine.MoveState(actualUnit, gameCreator, gameCreator.currentState));
                //}

                abilityButtonCreationUI.ClearAbilityButtons();
            }
        }

    }
}