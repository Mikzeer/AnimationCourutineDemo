using MikzeerGame.Animotion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Board2DManager
    {
        public Tile[,] GridArray { get; private set; }
        public int columnsWidht { get; private set; }
        public int rowsHeight { get; private set; }
        public Dictionary<IOcuppy, Position> occupierPositions { get; set; }
        public IOcuppy[,] ocuppiersByTile { get; set; }

        private Vector3 originPosition;
        private float tileSize;
        private Board2DManagerUI board2DManagerUI;
        private Board2DCreator board2DCreator;
        public Board2DManager(Board2DManagerUI board2DManagerUI, int rowsHeight, int columnsWidht)
        {
            this.board2DManagerUI = board2DManagerUI;
            occupierPositions = new Dictionary<IOcuppy, Position>();
            // a la cantidad de columnas que tenga nuestro grid le vamos a agregar dos de cada lado
            // entonces si es de 5 filas por 7 columnas va a quedar de 5 filas por 11 columnas
            // le sumamos 4 para el espacio de la base del jugado 1 y del jugador 2
            this.columnsWidht = columnsWidht + 4;
            this.rowsHeight = rowsHeight;

            tileSize = 4;
            originPosition = board2DManagerUI.LoadSizeAndGetOriginPosition(this.rowsHeight, this.columnsWidht);
            GridArray = new Tile[this.columnsWidht, this.rowsHeight];
            ocuppiersByTile = new IOcuppy[this.columnsWidht, this.rowsHeight];
            board2DCreator = new Board2DCreator(board2DManagerUI, this, this.rowsHeight, this.columnsWidht);
        }

        public Motion CreateBoard(Player[] players, Action OnBoardLoadComplete)
        {
            board2DCreator.CreateBoard(players);
            return board2DManagerUI.CreateNewBoardMotion(this, OnBoardLoadComplete);
        }

        public Animotion CreateBoardAnimotion(Player[] players, Action OnBoardLoadComplete)
        {
            board2DCreator.CreateBoard(players);
            return board2DManagerUI.CreateNewBoardAnimotion(this, OnBoardLoadComplete);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * tileSize + originPosition;
        }

        public Vector3 GetGridObjectRealWorldPositionByArrayPosition(int x, int y)
        {
            Vector3 offsetPosition = GetWorldPosition(x, y);
            offsetPosition = offsetPosition + new Vector3(tileSize / 2, tileSize / 2, 0);
            return offsetPosition;
        }

        public Vector3 GetPlayerNexusWorldPosition(Player player)
        {
            switch (player.OwnerPlayerID)
            {
                case 0:
                    return board2DManagerUI.GetPlayerNexusWorldPosition(true);
                case 1:
                    return board2DManagerUI.GetPlayerNexusWorldPosition(false);
                default:
                    return Vector3.zero;
            }
        }

        public Vector3 GetPlayerNexusWorldPosition(bool isPlayerOne)
        {
            return board2DManagerUI.GetPlayerNexusWorldPosition(isPlayerOne);
        }

        private void GetXY(Vector3 worldposition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldposition - originPosition).x / tileSize);
            y = Mathf.FloorToInt((worldposition - originPosition).y / tileSize);
        }

        public Vector3 GetGridObjectRealWorldPosition(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            Vector3 offsetPosition = GetWorldPosition(x, y);
            offsetPosition = offsetPosition + new Vector3(tileSize / 2, tileSize / 2, 0);
            return offsetPosition;
        }

        public Tile GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < columnsWidht && y < rowsHeight)
            {
                return GridArray[x, y];
            }
            else
            {
                return default(Tile);
            }
        }

        public Tile GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        public float GetTileSize()
        {
            return tileSize;
        }

        public void AddModifyOccupierPosition(IOcuppy ocuppier, Position position)
        {
            if (GetGridObject(position.posX, position.posY) == null)
            {
                return;
            }
            else
            {
                ocuppiersByTile[position.posX, position.posY] = ocuppier;
            }


            if (occupierPositions.ContainsKey(ocuppier))
            {
                occupierPositions[ocuppier] = position;
            }
            else
            {
                occupierPositions.Add(ocuppier, position);
            }
        }

        public void RemoveOccupierPosition(IOcuppy ocuppier)
        {
            if (ocuppiersByTile[ocuppier.actualPosition.posX, ocuppier.actualPosition.posY] == ocuppier)
            {
                ocuppiersByTile[ocuppier.actualPosition.posX, ocuppier.actualPosition.posY] = null;
            }

            if (occupierPositions.ContainsKey(ocuppier))
            {
                occupierPositions.Remove(ocuppier);
            }
        }

        public void MoveOccupier(IOcuppy ocuppier, Position position)
        {
            RemoveOccupierPosition(ocuppier);
            AddModifyOccupierPosition(ocuppier, position);
        }

        public Tile GetUnitPosition(IOcuppy ocuppier)
        {
            if (occupierPositions.ContainsKey(ocuppier)) return GetGridObject(occupierPositions[ocuppier].posX, occupierPositions[ocuppier].posY);
            return null;
        }

        public bool IsThereAPosibleSpawneableTile(int playerID)
        {
            int columnIndex = 2;
            if (playerID == 1) columnIndex = 8;
            for (int i = 0; i < rowsHeight; i++)
            {
                if (GridArray[columnIndex, i].IsOccupied() == false) return true;
                if (GridArray[columnIndex, i].GetOcuppy() == null) return false;
                if (GridArray[columnIndex, i].GetOcuppy().OccupierType != OCUPPIERTYPE.UNIT) return false;
                Kimboko auxKimb = (Kimboko)GridArray[columnIndex, i].GetOcuppy();
                if (auxKimb.OwnerPlayerID == playerID)
                {
                    if (CombineKimbokoRules.CanICombineAndEvolveWithUnitType(auxKimb, UNITTYPE.X)) return true;
                }
            }
            return false;
        }

        public bool IsThereAPosibleAttackableEnemyInBase(int playerID)
        {
            int columnIndex = 3;
            if (playerID == 1) columnIndex = 9;
            for (int i = 0; i < rowsHeight; i++)
            {
                if (GridArray[columnIndex, i].IsOccupied())
                {
                    if (GridArray[columnIndex, i].GetOcuppy().OwnerPlayerID != playerID) return true;
                }
            }
            return false;
        }

        public List<SpawnTile> GetPlayerSpawnTiles(int playerID)
        {
            List<SpawnTile> spawnTile = new List<SpawnTile>();
            int columnIndex = 2;
            if (playerID == 1) columnIndex = 8;
            for (int i = 0; i < rowsHeight; i++)
            {
                if (GridArray[columnIndex, i].IsOccupied() == false)
                {
                    SpawnTile aux = (SpawnTile)GridArray[columnIndex, i];
                    spawnTile.Add(aux);
                    continue;
                }
                if (GridArray[columnIndex, i].GetOcuppy() == null) continue;

                if (GridArray[columnIndex, i].GetOcuppy().OccupierType != OCUPPIERTYPE.UNIT) continue;

                Kimboko auxKimb = (Kimboko)GridArray[columnIndex, i].GetOcuppy();
                if (auxKimb.OwnerPlayerID == playerID)
                {
                    if (CombineKimbokoRules.CanICombineWithUnitType(auxKimb, UNITTYPE.X))
                    {
                        SpawnTile aux = (SpawnTile)GridArray[columnIndex, i];
                        spawnTile.Add(aux);
                        continue;
                    }

                    if (CombineKimbokoRules.CanICombineAndEvolveWithUnitType(auxKimb, UNITTYPE.X))
                    {
                        SpawnTile aux = (SpawnTile)GridArray[columnIndex, i];
                        spawnTile.Add(aux);
                        continue;
                    }
                }
            }
            return spawnTile;
        }
    }
}