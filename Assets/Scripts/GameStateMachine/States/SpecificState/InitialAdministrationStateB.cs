using PositionerDemo;
using UnityEngine;

namespace StateMachinePattern
{
    public class InitialAdministrationStateB : AdministrationState
    {
        public InitialAdministrationStateB(int duration, GameMachine gmMachine, int mngPoints) : base(duration, gmMachine, mngPoints)
        {
        }

        public override void OnUpdate()
        {
            gameTimer.RestTime();
            if (HaveReachCondition())
            {
                InitialAdministrationStateC adminState = new InitialAdministrationStateC(20, gmMachine, 2);
                OnNextState(adminState);
            }
        }

        public override void OnExit()
        {
            Debug.Log("EXIT ADMIN STATE");
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
        }
    }
}
