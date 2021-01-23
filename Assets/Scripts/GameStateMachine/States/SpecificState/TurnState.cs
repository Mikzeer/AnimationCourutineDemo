using PositionerDemo;
using System.Collections.Generic;
using UIButtonPattern;

namespace StateMachinePattern
{
    public class TurnState : ActionState<Tile>
    {
        private const string name = "TURN";
        private int managmentPoints;


        UIInputButtonPattern uiInputButtonPattern;
        protected GameMachine gmMachine;
        public TurnState(int duration, GameMachine game, int managmentPoints) : base(game, duration)
        {
            this.managmentPoints = managmentPoints;
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

            //selectInput.Suscribe();
            //uiInputActions.Suscribe();
            //for (int i = 0; i < gameCreator.actualPlayerTurn.units.Count; i++)
            //{
            //    gameCreator.actualPlayerTurn.units[i].SetActionPoints(2);
            //    gameCreator.actualPlayerTurn.units[i].attackAbility.actionStatus = MikzeerGameNotas.ACTIONEXECUTIONSTATUS.WAIT;
            //    gameCreator.actualPlayerTurn.units[i].moveAbility.actionStatus = MikzeerGameNotas.ACTIONEXECUTIONSTATUS.WAIT;
            //    gameCreator.actualPlayerTurn.units[i].defendAbility.actionStatus = MikzeerGameNotas.ACTIONEXECUTIONSTATUS.WAIT;
            //}
            ////Debug.Log("Enter Turn State Player " + gameCreator.actualPlayerTurn.ID);

            //game.tileSelectionManagerUI.onTileSelected += ExecuteAction;
            // ACA TENGO QUE PRENDER EL BOTON DE CARD
            //CreateButtonSelectionControl();
            //uiInputButtonPattern.Suscribe();
            //gameCreator.TakeCardAvailable(true);
            //Debug.Log("Enter Administration State Player " + GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID);
        }

        public override void OnExit()
        {
            // 1 - DESUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected -= ExecuteAction;


            // ACA TENGO QUE APAGAR EL BOTON DE CARD
            //gameCreator.TakeCardAvailable(false);
            // DETENEMOS EL TIEMPO
            base.OnExit();

            //uiInputButtonPattern.Unsuscribe();

            // RESTAMOS LAS ACCIONES DEL PLAYER PARA QUE NO PUEDE HACER NADA MAS
            //game.actionsManager.RestPlayerActions(game.turnController.CurrentPlayerTurn);

            // CAMBIAMOS EL TURNO AL OTRO JUGADOR
            game.turnController.ChangeCurrentRound();
        }

        public override bool HaveReachCondition()
        {
            //// SIN ACTION POINTS A UTILIZAR
            //if (game.turnController.CurrentPlayerTurn.GetCurrentActionPoints() <= 0)
            //{
            //    return true;
            //}

            //// SIN ACCIONES A EJECUTAR
            //if (game.actionsManager.DoesThePlayerHaveActionToExecute(game.turnController.CurrentPlayerTurn) == false)
            //{
            //    return true;
            //}

            // END TIME
            if (gameTimer.running == false)
            {
                return true;
            }

            // CHEQUEO ESPECIAL, SI NO TENGO CARDS, NI HAY UN ENEMIGO EN BASE, NI HAY UNA TILE POSIBLE PARA SPAWNEAR
            bool havaCardsInDeck = game.turnController.CurrentPlayerTurn.Deck.Count > 0;
            bool isAnEnemyInBase = game.board2DManager.IsThereAPosibleAttackableEnemyInBase(game.turnController.CurrentPlayerTurn.PlayerID);
            bool isThereAPosibleSpawnTile = game.board2DManager.IsThereAPosibleSpawneableTile(game.turnController.CurrentPlayerTurn.PlayerID); ;

            if (!havaCardsInDeck && !isAnEnemyInBase && !isThereAPosibleSpawnTile)
            {
                return true;
            }

            return false;
        }

        public override void OnUpdate()
        {
            // UPDATEO EL TIEMPO... PODRIA TENER ESTO EN UN MONO BEHAVIOUR PEEEEEEROOOOOO.... NO SE
            base.OnUpdate();

            if (HaveReachCondition())
            {
                AdministrationState adminState = new AdministrationState(20, gmMachine, 1);
                OnNextState(adminState);
            }
        }

        public void CreateButtonSelectionControl()
        {
            //CardButtonEventFire cardButtonEvent = new CardButtonEventFire();
            //SpecificCardButtonExecution cardButtonExecution = new SpecificCardButtonExecution(game.turnController.CurrentPlayerTurn, game.cardManager);
            //ButtonAndEventContainer cardButtonPartners = new ButtonAndEventContainer(cardButtonEvent, cardButtonExecution);
            EndTurnButtonEventFire EndTurnButtonEvent = new EndTurnButtonEventFire();
            SpecificEndTurnButtonExecution EndTurnButtonExecution = new SpecificEndTurnButtonExecution(game.turnController);
            ButtonAndEventContainer cardButtonPartners = new ButtonAndEventContainer(EndTurnButtonEvent, EndTurnButtonExecution);

            List<ButtonAndEventContainer> buttonPartners = new List<ButtonAndEventContainer>();
            buttonPartners.Add(cardButtonPartners);
            uiInputButtonPattern = new UIInputButtonPattern(buttonPartners);
        }

        public override void ExecuteAction(Tile action)
        {
            gmMachine.abilityButtonCreationUI.SetTile(action);
        }
    }
}
