using PositionerDemo;
using System.Collections.Generic;

namespace MikzeerGame
{
    namespace UI
    {
        public class SpawnAbilityButtonExecution : SpecificAbilityExecution
        {
            IOcuppy actualOccupier;
            IGame game;
            public string name { get; private set; } = "SPAWN";
            public SpawnAbilityButtonExecution(IOcuppy actualOccupier, IGame gameCreator)
            {
                this.game = gameCreator;
                this.actualOccupier = actualOccupier;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;

                Player player = (Player)actualOccupier;

                if (player == null) return;

                if (game.spawnManager.CanIEnterSpawnState(player))
                {
                    // CREO LA LISTA/DICCTIONARY DE LAS POSIBLES TILES A SPAWNEAR / SPAWN COMBINAR CON SU HIGHLIGHT CORRESPONDIENTE

                    // CREO EL SELECTION STATE

                    // CAMBIO AL NUEVO STATE
                    //    gameCreator.ChangeState(new GameStateMachine.MoveState(actualUnit, gameCreator, gameCreator.currentState));
                }
            }
        }

    }
}