using System;
using UnityEngine;

namespace PositionerDemo
{
    public class Board2DManager
    {
        private Tile[,] _gridArray; // Aca van a estar guardadas todas las tiles
        public Tile[,] GridArray { get => _gridArray; private set => _gridArray = value; }
        private int _columns; // widht
        public int columnsWidht { get => _columns; private set => _columns = value; }
        private int _rows; // height
        public int rowsHeight { get => _rows; private set => _rows = value; }

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
            _columns = columnsWidht + 4;
            _rows = rowsHeight;
            tileSize = 4;
            originPosition = board2DManagerUI.LoadSizeAndGetOriginPosition(_rows, _columns);
            GridArray = new Tile[_columns, _rows];

            board2DCreator = new Board2DCreator(board2DManagerUI, this, _rows, _columns);
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
            if (x >= 0 && y >= 0 && x < _columns && y < _rows)
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