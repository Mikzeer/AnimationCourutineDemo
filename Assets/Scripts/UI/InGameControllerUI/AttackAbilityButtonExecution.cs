using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class AttackAbilityButtonExecution : SpecificAbilityExecution
        {
            IOcuppy actualOccupier;
            GameMachine gameCreator;
            AbilityButtonCreationUI abilityButtonCreationUI;
            public string name { get; private set; } = "ATTACK";

            public AttackAbilityButtonExecution(IOcuppy actualOccupier, GameMachine gameCreator, AbilityButtonCreationUI abilityButtonCreationUI)
            {
                this.actualOccupier = actualOccupier;
                this.gameCreator = gameCreator;
                this.abilityButtonCreationUI = abilityButtonCreationUI;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;
                //Debug.Log("Unit " + actualUnit._ID + " ATTACK");

                // CUANDO TENGAMOS UN PLAYER Y EN NUESTRO TURNO LE HAGAMOS CLICK, NOS VA A APARECER AUTOMATICAMENTE PARA ATACAR A UN JUGADOR EN BASE
                // CUANDO TENGAMOS UNA BARRICADA QUE ATAQUE, Y LA SELECCIONEMOS, AUTOMATICAMENTE NOS VA A APARECER PARA ATACAR A LOS JUGADORES
                // CON LA UNIT NO, NOS APARECE UN BOTON
                //actualUnit.attackAbility.Set(gameCreator);
                //if (actualUnit.attackAbility.OnTryExecute())
                //{
                //    gameCreator.ChangeState(new GameStateMachine.AttackState(actualUnit, gameCreator, gameCreator.currentState));
                //}

                abilityButtonCreationUI.ClearAbilityButtons();
            }
        }

    }
}