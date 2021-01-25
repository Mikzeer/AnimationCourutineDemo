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
                // NO DEBERIA RETURN Y PONER EN CLEAR LAS ABILITY BUTTON ???
                if (actualOccupier == null) return;
                if (gameCreator.movementManager.CanIEnterMoveState(actualOccupier))
                {
                    gameCreator.movementManager.OnEnterMoveState(actualOccupier);
                }

                abilityButtonCreationUI.ClearAbilityButtons();
            }
        }

    }
}