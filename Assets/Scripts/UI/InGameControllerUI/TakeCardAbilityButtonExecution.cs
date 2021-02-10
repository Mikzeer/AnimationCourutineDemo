using PositionerDemo;

namespace MikzeerGame
{
    namespace UI
    {
        public class TakeCardAbilityButtonExecution : SpecificAbilityExecution
        {
            IOcuppy actualOccupier;
            GameMachine game;
            AbilityButtonCreationUI abilityButtonCreationUI;
            public string name { get; private set; } = "TAKE CARD";
            public TakeCardAbilityButtonExecution(IOcuppy actualOccupier, GameMachine gameCreator, AbilityButtonCreationUI abilityButtonCreationUI)
            {
                this.game = gameCreator;
                this.actualOccupier = actualOccupier;
                this.abilityButtonCreationUI = abilityButtonCreationUI;
            }

            public void Execute()
            {
                if (actualOccupier == null) return;
                Player player = (Player)actualOccupier;
                if (player == null) return;
                if (game.cardManager.CanITakeACard(player))
                {
                    game.cardManager.OnTakeCard(player);
                }
                abilityButtonCreationUI.ClearAbilityButtons();
            }
        }
    }
}