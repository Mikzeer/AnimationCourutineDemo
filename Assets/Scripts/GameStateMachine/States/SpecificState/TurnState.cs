using CommandPatternActions;
using PositionerDemo;
using UIButtonPattern;

namespace StateMachinePattern
{
    public class TurnState : ActionState<Tile>
    {
        public bool abruptEnd = false;
        ButtonAndEventContainer endTurnButtonPartners;
        protected GameMachine gmMachine;
        public TurnState(int duration, GameMachine game) : base(game, duration)
        {
            this.gmMachine = game;
            stateName = "TURN STATE";
        }

        public override void OnEnter()
        {
            // 1 - SUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected += ExecuteAction;
            // 2 - TENGO QUE SETEAR LOS ACTIONS POINTS PARA ESTE JUGADOR
            game.actionsManager.IncrementPlayerUnitsActions(game.turnController.CurrentPlayerTurn, 2);
            // SOLO EN TURN STATE PODEMOS TERMINAR EL TURNO Y PASAR AL SIGUIENTE JUGADOR
            CreateAndSuscribeEndTurnButtonSelectionControl();
            // COMENZAMOS EL CONTADOR DE TIEMPO
            base.OnEnter();
            // NOS SUSCRIBIMOS AL EVENTO DE CAMBIAR EL TIEMPO
            gameTimer.OnTimePass += gmMachine.uiGeneralManagerInGame.UpdateTime;
        }

        public override void OnExit()
        {
            // 1 - DESUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected -= ExecuteAction;
            // NOS DESUSCRIBIMOS AL BUTTON DEL END TURNO
            UnsuscribeEndTurnButtonSelectionControl();
            // RESTAMOS LAS ACCIONES DE LAS UNIDADES DEL PLAYER PARA QUE NO PUEDE HACER NADA MAS
            game.actionsManager.RestPlayerUnitsActions(game.turnController.CurrentPlayerTurn);
            // CAMBIAMOS EL TURNO AL OTRO JUGADOR
            game.turnController.ChangeCurrentRound();
            // DETENEMOS EL TIEMPO
            base.OnExit();
            // NOS DESUSCRIBIMOS AL EVENTO DE CAMBIAR EL TIEMPO
            gameTimer.OnTimePass += gmMachine.uiGeneralManagerInGame.UpdateTime;
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

            // CHEQUEO ESPECIAL, SI NO TENGO CARDS  EN LA MANO, SI TENGO UNIDADES EN EL CAMPO
            bool havaCardsInHand = game.turnController.CurrentPlayerTurn.PlayersHands.Count > 0;
            bool haveUnits = game.turnController.CurrentPlayerTurn.kimbokoUnits.Count > 0;
            if (!havaCardsInHand && !haveUnits)
            {
                return true;
            }

            return false;
        }

        public override void OnUpdate()
        {
            if (HaveReachCondition())
            {
                AdministrationState adminState = new AdministrationState(20, gmMachine, 1);
                Motion bannerMotion = gmMachine.informationUIManager.SetAndShowBanner(adminState.stateName, 0.5f);
                InvokerMotion.AddNewMotion(bannerMotion);
                InvokerMotion.StartExecution(gmMachine);
                IState changePhaseState = new ChangePhaseState(gmMachine, adminState);
                OnNextState(changePhaseState);
                //OnNextState(adminState);
            }
            base.OnUpdate();
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
