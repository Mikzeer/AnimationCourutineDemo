using PositionerDemo;
using UIButtonPattern;

namespace StateMachinePattern
{
    public class TurnState : ActionState<Tile>
    {
        private const string name = "TURN";
        public bool abruptEnd = false;
        ButtonAndEventContainer endTurnButtonPartners;
        protected GameMachine gmMachine;
        public TurnState(int duration, GameMachine game) : base(game, duration)
        {
            this.gmMachine = game;
        }

        public override void OnEnter()
        {
            // 1 - SUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected += ExecuteAction;

            // 2 - TENGO QUE SETEAR LOS ACTIONS POINTS PARA ESTE JUGADOR
            game.actionsManager.IncrementPlayerUnitsActions(game.turnController.CurrentPlayerTurn, 2);

            // COMENZAMOS EL CONTADOR DE TIEMPO
            base.OnEnter();

            CreateAndSuscribeEndTurnButtonSelectionControl();
        }

        public override void OnExit()
        {
            // 1 - DESUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected -= ExecuteAction;

            // DETENEMOS EL TIEMPO
            base.OnExit();

            UnsuscribeEndTurnButtonSelectionControl();

            // RESTAMOS LAS ACCIONES DE LAS UNIDADES DEL PLAYER PARA QUE NO PUEDE HACER NADA MAS
            game.actionsManager.RestPlayerUnitsActions(game.turnController.CurrentPlayerTurn);

            // CAMBIAMOS EL TURNO AL OTRO JUGADOR
            game.turnController.ChangeCurrentRound();
        }

        public override bool HaveReachCondition()
        {
            // EL JUGADOR APRETO EL BOTON END TURN
            if (abruptEnd)
            {
                return true;
            }
            // END TIME
            if (gameTimer.running == false)
            {
                return true;
            }

            // CHEQUEO ESPECIAL, SI NO TENGO CARDS, NI HAY UN ENEMIGO EN BASE, NI HAY UNA TILE POSIBLE PARA SPAWNEAR
            //bool havaCardsInDeck = game.turnController.CurrentPlayerTurn.Deck.Count > 0;
            //bool isAnEnemyInBase = game.board2DManager.IsThereAPosibleAttackableEnemyInBase(game.turnController.CurrentPlayerTurn.PlayerID);
            //bool isThereAPosibleSpawnTile = game.board2DManager.IsThereAPosibleSpawneableTile(game.turnController.CurrentPlayerTurn.PlayerID); ;

            //if (!havaCardsInDeck && !isAnEnemyInBase && !isThereAPosibleSpawnTile)
            //{
            //    return true;
            //}

            return false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (HaveReachCondition())
            {
                AdministrationState adminState = new AdministrationState(20, gmMachine, 1);
                OnNextState(adminState);
            }
        }
        
        public void CreateAndSuscribeEndTurnButtonSelectionControl()
        {
            EndTurnButtonEventFire EndTurnButtonEvent = new EndTurnButtonEventFire();
            SpecificEndTurnButtonExecution EndTurnButtonExecution = new SpecificEndTurnButtonExecution(this);
            endTurnButtonPartners = new ButtonAndEventContainer(EndTurnButtonEvent, EndTurnButtonExecution);
            gmMachine.uiGeneralManagerInGame.OnEndTurnActionClicked += EndTurnButtonExecution.Execute;
            gmMachine.uiGeneralManagerInGame.SetActiveEndTurnButton(true);
        }

        public void UnsuscribeEndTurnButtonSelectionControl()
        {
            gmMachine.uiGeneralManagerInGame.OnEndTurnActionClicked -= endTurnButtonPartners.buttonEventFire.Execute;
            gmMachine.uiGeneralManagerInGame.SetActiveEndTurnButton(false);
        }

        public override void ExecuteAction(Tile action)
        {
            gmMachine.abilityButtonCreationUI.SetTile(action);
        }
    }
}
