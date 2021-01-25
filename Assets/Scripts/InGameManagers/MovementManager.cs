using AbilitySelectionUI;
using CommandPatternActions;
using StateMachinePattern;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class MovementManager : AbilityManager
    {
        GameMachine game;
        bool debugOn = false;
        public List<Tile> posibleMoveableTiles { get; private set; }
        public Tile selectedTileToMove;
        MoveManagerUI moveManagerUI;

        public MovementManager(GameMachine game, MoveManagerUI moveManagerUI)
        {
            this.game = game;
            this.moveManagerUI = moveManagerUI;
        }

        public bool CanIEnterMoveState(IOcuppy occupier)
        {
            // 6- SI ESTOY ONLINE TENGO QUE PREGUNTARLE AL SERVER SI ES UN MOVIMIENTO VALIDO
            // ES EL TURNO DEL PLAYER, Y SU UNIDAD TIENE AC Y LA LA HABILIDAD 

            // SI EL PLAYER ESTA EN SU TURNO
            if (occupier.OwnerPlayerID != game.turnController.CurrentPlayerTurn.OwnerPlayerID)
            {
                if (debugOn) Debug.Log("NO ES EL TURNO DEL PLAYER");
                return false;
            }

            if (occupier.Abilities.ContainsKey(ABILITYTYPE.MOVE) == false)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD MOVE NO ENCONTRADA EN PLAYER");
                return false;
            }
            MoveAbility moveAbility = (MoveAbility)occupier.Abilities[ABILITYTYPE.MOVE];
            if (moveAbility == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD MOVE NO ENCONTRADA EN PLAYER");
                return false;
            }

            return true;
        }

        public void OnEnterMoveState(IOcuppy occupier)
        {
            // CREO LA LISTA/DICCTIONARY DE LAS POSIBLES TILES A MOVERSE CON SU HIGHLIGHT CORRESPONDIENTE
            Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary = CreateHighlightUIDictionary(occupier);
            MoveAbilitySelectionUIContainer moveUIContainer = new MoveAbilitySelectionUIContainer(tileHighlightTypesDictionary);
            MoveState moveState = new MoveState(game, game.baseStateMachine.currentState, moveUIContainer, occupier);
            game.baseStateMachine.PopState(true);
            game.baseStateMachine.PushState(moveState);
        }

        private Dictionary<Tile, HIGHLIGHTUITYPE> CreateHighlightUIDictionary(IOcuppy occupier)
        {
            Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary = new Dictionary<Tile, HIGHLIGHTUITYPE>();
            int moveAmount = occupier.Stats[STATTYPE.MOVERANGE].ActualStatValue;
            MOVEDIRECTIONTYPE movementType = occupier.MoveDirectionerType;
            Tile fromTile = game.board2DManager.GetGridObject(occupier.actualPosition.posX, occupier.actualPosition.posY);

            List<Tile> moveTiles = GetPosibleMoveableTiles(fromTile, movementType, moveAmount);

            if (moveTiles.Count <= 0) return tileHighlightTypesDictionary;

            for (int i = 0; i < moveTiles.Count; i++)
            {
                tileHighlightTypesDictionary.Add(moveTiles[i], HIGHLIGHTUITYPE.MOVE);
            }

            return tileHighlightTypesDictionary;
        }

        public List<Tile> GetPosibleMoveableTiles(Tile fromTile, MOVEDIRECTIONTYPE moveDirection, int movementAmount)
        {
            List<Tile> posibleMoveableTiles = new List<Tile>();
            // Filtramos esas tiles y las ponemos en la lista
            FilterMoveableWithBoardStrategyPattern filterMoveable = new FilterMoveableWithBoardStrategyPattern(fromTile, moveDirection, game.board2DManager.GridArray, movementAmount);
            posibleMoveableTiles = filterMoveable.moveableTiles;
            return posibleMoveableTiles;
        }

        public void OnTryMove(IOcuppy occupier, Tile endPosition)
        {
            if (CanIEnterMoveState(occupier) == false)
            {
                return;
            }

            if (occupier == null || endPosition == null)
            {
                if (debugOn) Debug.Log("No Tile Object or Ocuppier");
                return;
            }

            /////////////////////////////////////////////////////////////////

            // CREAMOS POSIBLE MOVEABLE TILES, SOLO LA LISTA SIN PINTARLAS NI NADA
            // TILE EN DONDE ESTAMOS PARADOS
            Tile fromTile = game.board2DManager.GetUnitPosition(occupier);
            if (fromTile == null)
            {
                if (debugOn) Debug.Log("You are walking on the sky my men");
                return;
            }

            // TIPO DE MOVIMIENTO DE NUESTRA UNIDAD
            MOVEDIRECTIONTYPE moveDirection = occupier.MoveDirectionerType;
            if (moveDirection == MOVEDIRECTIONTYPE.NONE)
            {
                return;
            }

            // CANTIDA DE CASILLA QUE PUEDO MOVERME
            int movementAmount = occupier.Stats[STATTYPE.MOVERANGE].ActualStatValue;

            if (movementAmount == 0)
            {
                return;
            }

            // 3 - QUE SEA UNA DISTANCIA VALIDA SEGUN MI RANGO DE MOVIMIENTO
            // CANTIDA DE CASILLA QUE PUEDO MOVERME DEBERIA RESTAR A LA POS INICIAL LA POS FINAL Y VER LA DIFERENCIA EN DISTANCIA
            // Filtramos esas tiles y las ponemos en la lista
            FilterMoveableWithBoardStrategyPattern filterMoveable = 
                new FilterMoveableWithBoardStrategyPattern(fromTile, moveDirection, game.board2DManager.GridArray, movementAmount);
            posibleMoveableTiles = filterMoveable.moveableTiles;

            // 2- SI NO HAY TILES LIBRES PARA MOVERSE
            if (posibleMoveableTiles.Count == 0)
            {
                return;
            }

            /////////////////////////////////////

            if (!IsLegalMovement(occupier, endPosition))
            {
                if (debugOn) Debug.Log("Ilegal MOVE");
                return;
            }
            if (occupier.Abilities.ContainsKey(ABILITYTYPE.MOVE) == false)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD MOVE NO ENCONTRADA EN OCUPPIER");
                return;
            }
            MoveAbility moveAb = (MoveAbility)occupier.Abilities[ABILITYTYPE.MOVE];
            if (moveAb == null)
            {
                if (debugOn) Debug.Log("ERROR HABILIDAD MOVE NO ENCONTRADA EN EL OCCUPIER");
                return;
            }
            MoveAbilityEventInfo movInfo = new MoveAbilityEventInfo(occupier, fromTile, endPosition);

            moveAb.SetRequireGameData(movInfo);
            StartPerform(moveAb);
            if (moveAb.CanIExecute() == false)
            {
                if (debugOn) Debug.Log("SPAWN ABILITY NO SE PUEDE EJECUTAR");
                return;
            }
            Move(movInfo);
            Perform(moveAb);
            EndPerform(moveAb);
        }

        private bool IsLegalMovement(IOcuppy occupier, Tile endPosition)
        {
            return true;
        }

        private void Move(MoveAbilityEventInfo movInfo)
        {
            Kimboko kimb = (Kimboko)movInfo.moveOccupy;
            if (kimb != null && kimb.UnitType == UNITTYPE.COMBINE)
            {
                KimbokoCombine combien = (KimbokoCombine)kimb;
                if (combien != null)
                {
                    CombineMove(combien, movInfo);
                    return;
                }
            }
            NormalMove(movInfo);
        }

        private void NormalMove(MoveAbilityEventInfo movInfo)
        {
            IMoveCommand moveCommand = new IMoveCommand(movInfo, game);
            Invoker.AddNewCommand(moveCommand);
            Invoker.ExecuteCommands();

            Vector3 endPosition = movInfo.endPosition.GetRealWorldLocation();
            Motion normalMoveMotion = moveManagerUI.MoveMotion(movInfo.moveOccupy.goAnimContainer.GetGameObject(), endPosition);
            InvokerMotion.AddNewMotion(normalMoveMotion);
            InvokerMotion.StartExecution(game);
        }

        private void CombineMove(KimbokoCombine combien, MoveAbilityEventInfo movInfo)
        {
            IMoveCommand moveCommand = new IMoveCommand(movInfo, game);
            Invoker.AddNewCommand(moveCommand);
            Invoker.ExecuteCommands();

            Vector3 startPosition = movInfo.fromTile.GetRealWorldLocation();
            Vector3 endPosition = movInfo.endPosition.GetRealWorldLocation();
            GameObject[] goToMove = new GameObject[combien.kimbokos.Count];

            for (int i = 0; i < goToMove.Length; i++)
            {
                goToMove[i] = combien.kimbokos[i].goAnimContainer.GetGameObject();
            }

            Motion combineMoveMotion = moveManagerUI.CombineMoveMotion(startPosition, endPosition, goToMove);
            InvokerMotion.AddNewMotion(combineMoveMotion);
            InvokerMotion.StartExecution(game);
        }

    }
}
