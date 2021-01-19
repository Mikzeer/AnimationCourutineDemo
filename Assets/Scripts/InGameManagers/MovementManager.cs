using CommandPatternActions;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class MovementManager : AbilityManager
    {
        IGame game;
        bool debugOn = false;
        public List<Tile> posibleMoveableTiles { get; private set; }
        public Tile selectedTileToMove;
        MoveManagerUI moveManagerUI;


        public MovementManager(IGame game, MoveManagerUI moveManagerUI)
        {
            this.game = game;
            this.moveManagerUI = moveManagerUI;
        }

        public void OnTryMove(IOcuppy occupier, Tile endPosition)
        {
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
            EndPerform(moveAb);
        }

        private bool IsLegalMovement(IOcuppy occupier, Tile endPosition)
        {
            return true;
        }

        private void Move(MoveAbilityEventInfo movInfo)
        {
            KimbokoCombine combien = (KimbokoCombine)movInfo.moveOccupy;
            if (combien != null)
            {
                CombineMove(combien, movInfo);
            }
            else
            {
                NormalMove(movInfo);
            }
        }

        private void NormalMove(MoveAbilityEventInfo movInfo)
        {
            IMoveCommand moveCommand = new IMoveCommand(movInfo, game);
            Invoker.AddNewCommand(moveCommand);
            Invoker.ExecuteCommands();

            Vector3 endPosition = movInfo.endPosition.GetRealWorldLocation();
            Motion normalMoveMotion = moveManagerUI.MoveMotion(movInfo.moveOccupy.goAnimContainer.GetGameObject(), endPosition);
            InvokerMotion.AddNewMotion(normalMoveMotion);
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
        }

        public void SetSelection(Tile selectedMoveableTile)
        {
            if (selectedMoveableTile == null)
            {
                return;
            }

            Position pos = new Position(selectedMoveableTile.position.posX, selectedMoveableTile.position.posY);

            for (int i = 0; i < posibleMoveableTiles.Count; i++)
            {
                if (pos.posX == posibleMoveableTiles[i].position.posX && pos.posY == posibleMoveableTiles[i].position.posY)
                {
                    selectedTileToMove = selectedMoveableTile;
                    return;
                }
            }
        }
    }
}
