using CommandPatternActions;
using PositionerDemo;
using UnityEngine;

namespace StateMachinePattern
{
    public abstract class InitialAdministrationStateBase : AdministrationState
    {
        protected BaseState nextState;
        public InitialAdministrationStateBase(int duration, GameMachine game, int managmentPoints) : base(duration, game, managmentPoints)
        {
        }

        public override void OnExit()
        {
            if (logOn) Debug.Log("EXIT ADMIN STATE");
            // 1 - DESUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected -= ExecuteAction;
            // DETENEMOS EL TIEMPO
            gameTimer.Stop();
            // POR LAS DUDAS SACAMOS EL MENU DE SELECCION
            gmMachine.abilityButtonCreationUI.SetUnit(null);
            // RESTAMOS LAS ACCIONES DEL PLAYER PARA QUE NO PUEDE HACER NADA MAS
            game.actionsManager.RestPlayerActions(game.turnController.CurrentPlayerTurn);
            // CAMBIAMOS EL TURNO AL OTRO JUGADOR
            game.turnController.ChangeCurrentRound();
            // NOS DESUSCRIBIMOS AL EVENTO DE CAMBIAR EL TIEMPO
            gameTimer.OnTimePass += gmMachine.uiGeneralManagerInGame.UpdateTime;
        }

        public override void OnUpdate()
        {
            if (logOn) Debug.Log("IN STATE " + stateName);
            if (HaveReachCondition())
            {
                PositionerDemo.Motion bannerMotion = gmMachine.informationUIManager.SetAndShowBanner(nextState.stateName, 0.5f);
                InvokerMotion.AddNewMotion(bannerMotion);
                InvokerMotion.StartExecution(gmMachine);
                IState changePhaseState = new ChangePhaseState(gmMachine, nextState);
                OnNextState(changePhaseState);
            }
            gameTimer.RestTime();
        }
    }
}
