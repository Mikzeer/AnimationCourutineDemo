using PositionerDemo;
using StateMachinePattern;
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
                    game.spawnManager.OnEnterSpawnState(player);
                }
            }
        }

    }
}