using System;
using UnityEngine;

namespace PositionerDemo
{
    public class Board2DManager
    {
        public Tile[,] GridArray { get; private set; }
        public int columnsWidht { get; private set; }
        public int rowsHeight { get; private set; }
        private Vector3 originPosition;
        private float tileSize;
        private Board2DManagerUI board2DManagerUI;
        private Board2DCreator board2DCreator;
        public Board2DManager(Board2DManagerUI board2DManagerUI, int rowsHeight, int columnsWidht)
        {
            this.board2DManagerUI = board2DManagerUI;
            // a la cantidad de columnas que tenga nuestro grid le vamos a agregar dos de cada lado
            // entonces si es de 5 filas por 7 columnas va a quedar de 5 filas por 11 columnas
            // le sumamos 4 para el espacio de la base del jugado 1 y del jugador 2
            this.columnsWidht = columnsWidht + 4;
            this.rowsHeight = rowsHeight;
            tileSize = 4;
            originPosition = board2DManagerUI.LoadSizeAndGetOriginPosition(this.rowsHeight, this.columnsWidht);
            GridArray = new Tile[this.columnsWidht, this.rowsHeight];

            board2DCreator = new Board2DCreator(board2DManagerUI, this, this.rowsHeight, this.columnsWidht);
        }

        public Motion CreateBoard(Player[] players, Action OnBoardLoadComplete)
        {
            board2DCreator.CreateBoard(players);
            return board2DManagerUI.CreateNewBoardMotion(this, OnBoardLoadComplete);
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
            switch (player.PlayerID)
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

    }
}