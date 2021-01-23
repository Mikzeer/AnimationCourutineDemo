using PositionerDemo;
using System.Collections.Generic;
using UIButtonPattern;
using UnityEngine;

namespace StateMachinePattern
{
    public class AdministrationState : ActionState<Tile>
    {
        private const string name = "ADMINISTRATION";
        private int managmentPoints;
        protected GameMachine gmMachine;
        public AdministrationState(int duration, GameMachine game, int managmentPoints) : base(game, duration)
        {
            this.managmentPoints = managmentPoints;
            this.gmMachine = game;
        }

        public override void OnEnter()
        {
            Debug.Log("ENTER ADMIN STATE");
            // 1 - SUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected += ExecuteAction;
            // 2 - TENGO QUE SETEAR LOS ACTIONS POINTS PARA ESTE JUGADOR
            game.actionsManager.IncrementPlayerActions(game.turnController.CurrentPlayerTurn, managmentPoints);
            // COMENZAMOS EL CONTADOR DE TIEMPO
            base.OnEnter();
            gmMachine.abilityButtonCreationUI.SetUnit(game.turnController.CurrentPlayerTurn);
        }

        public override void OnExit()
        {
            Debug.Log("EXIT ADMIN STATE");
            // 1 - DESUSCRIBIRSE AL EVENTO DE SELECCION
            gmMachine.tileSelectionManagerUI.onTileSelected -= ExecuteAction;
            // DETENEMOS EL TIEMPO
            base.OnExit();

            // POR LAS DUDAS SACAMOS EL MENU DE SELECCION
            gmMachine.abilityButtonCreationUI.SetUnit(null);

            // RESTAMOS LAS ACCIONES DEL PLAYER PARA QUE NO PUEDE HACER NADA MAS
            game.actionsManager.RestPlayerActions(game.turnController.CurrentPlayerTurn);
            //// CAMBIAMOS EL TURNO AL OTRO JUGADOR
            //game.turnController.ChangeCurrentRound();
        }

        public override bool HaveReachCondition()
        {
            // SIN ACTION POINTS A UTILIZAR
            if (game.turnController.CurrentPlayerTurn.GetCurrentActionPoints() <= 0)
            {
                return true;
            }
            // SIN ACCIONES A EJECUTAR
            if (game.actionsManager.DoesThePlayerHaveActionToExecute(game.turnController.CurrentPlayerTurn) == false)
            {
                return true;
            }
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
            base.OnUpdate();
            if (HaveReachCondition())
            {
                TurnState turnState = new TurnState(60, gmMachine);
                OnNextState(turnState);
            }
        }

        public override void ExecuteAction(Tile action)
        {
            if (action == null)
            {
                gmMachine.abilityButtonCreationUI.SetUnit(game.turnController.CurrentPlayerTurn);
            }
        }

        public override void OnBack()
        {
            gmMachine.abilityButtonCreationUI.SetUnit(game.turnController.CurrentPlayerTurn);
        }
    }
}
