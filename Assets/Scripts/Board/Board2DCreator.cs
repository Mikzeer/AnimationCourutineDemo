using UnityEngine;

namespace PositionerDemo
{
    public class Board2DCreator
    {
        Board2DManagerUI board2DManagerUI;
        Board2DManager board2DManager;
        private int _columns; // widht
        private int _rows; // height
        Tile[,] GridArray;
        public Board2DCreator(Board2DManagerUI board2DManagerUI, Board2DManager board2DManager, int rowsHeight, int columnsWidht)
        {
            this.board2DManagerUI = board2DManagerUI;
            this.board2DManager = board2DManager;
            _columns = columnsWidht;
            _rows = rowsHeight;
            GridArray = board2DManager.GridArray;
        }

        public void CreateBoard(Player[] players)
        {
            BaseNexoTile[] nexoTiles = new BaseNexoTile[2];
            for (int i = 0; i < players.Length; i++)
            {
                // nexo =      posx, posy, grid Player
                nexoTiles[i] = CreateBaseNexoTile(players[i]);
            }

            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < GridArray.GetLength(1); y++)
                {
                    // todas las posiciones de las columnas 0 / 1 van a estar ocupadas por la misma tile del player 1
                    if (x == 0 || x == 1)
                    {
                        GridArray[x, y] = nexoTiles[0];
                        continue;
                    }
                    // todas las posiciones de las columnas 9 / 10 van a estar ocupadas por la misma tile del player 2
                    if (x == 9 || x == 10)
                    {
                        GridArray[x, y] = nexoTiles[1];
                        continue;
                    }
                    // todas las posiciones de la columna 2 van a estar ocupadas por la spawn tile del player 1
                    if (x == 2)
                    {
                        // spawn =     posx, posy, grid playerID
                        CreateSpawnTile(x, y, players[0]);
                    }
                    // todas las posiciones de la columna 8 van a estar ocupadas por la spawn tile del player 2
                    else if (x == _columns - 3)
                    {
                        // spawn =     posx, posy, grid playerID
                        CreateSpawnTile(x, y, players[1]);
                    }
                    else
                    {
                        // battlefield posx, posy, grid
                        CreateBattlefiledTile(x, y);
                    }

                }
            }

            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    if (x == 0 || x == 1)
                    {
                        continue;
                    }

                    if (x == 9 || x == 10)
                    {
                        continue;
                    }
                    SetNeighborsTiles(GridArray[x, y]);
                }
            }

        }

        private void CreateSpawnTile(int posX, int posY, Player player)
        {
            Vector3 realWorldPosition = board2DManager.GetGridObjectRealWorldPositionByArrayPosition(posX, posY);
            SpawnTile spawnTile = new SpawnTile(realWorldPosition, posX, posY, player.PlayerID);
            GridArray[posX, posY] = spawnTile;
            board2DManagerUI.CreateSpawnTileHorizontal(posX, posY, spawnTile);
        }

        private void CreateBattlefiledTile(int posX, int posY)
        {
            Vector3 realWorldPosition = board2DManager.GetGridObjectRealWorldPositionByArrayPosition(posX, posY);
            BattlefieldTile battlefieldTile = new BattlefieldTile(realWorldPosition, posX, posY);
            GridArray[posX, posY] = battlefieldTile;
            board2DManagerUI.CreateBattlefiledTileHorizontal(posX, posY, battlefieldTile);
        }

        private BaseNexoTile CreateBaseNexoTile(Player player)
        {
            BaseNexoTile baseNexoTile = null;
            Vector2 position = board2DManager.GetPlayerNexusWorldPosition(player);
            switch (player.PlayerID)
            {
                case 0:
                    baseNexoTile = new BaseNexoTile(position, 0, 0, player);
                    board2DManagerUI.CreateBaseNexoTile(baseNexoTile, true);
                    break;
                case 1:
                    baseNexoTile = new BaseNexoTile(position, 9, 0, player);
                    board2DManagerUI.CreateBaseNexoTile(baseNexoTile, false);
                    break;
                default:
                    break;
            }

            return baseNexoTile;
        }

        private void SetNeighborsTiles(Tile tile)
        {
            //if (tile.PosX >= 0 && tile.PosY >= 0 && tile.PosX < _columns && tile.PosY < _rows)
            //{

            //}
            //LIST//
            //RIGHT
            if (tile.PosX + 1 <= _columns - 1) tile.NeighborsTiles.Add(GridArray[tile.PosX + 1, tile.PosY]);
            // LEFT
            if (tile.PosX - 1 >= 0) tile.NeighborsTiles.Add(GridArray[tile.PosX - 1, tile.PosY]);
            // UP
            if (tile.PosY + 1 <= _rows - 1) tile.NeighborsTiles.Add(GridArray[tile.PosX, tile.PosY + 1]);
            // DOWN
            if (tile.PosY - 1 >= 0) tile.NeighborsTiles.Add(GridArray[tile.PosX, tile.PosY - 1]);
            // UP RIGHT
            if (tile.PosX + 1 <= _columns - 1 && tile.PosY + 1 <= _rows - 1) tile.NeighborsTiles.Add(GridArray[tile.PosX + 1, tile.PosY + 1]);
            // DOWN RIGHT
            if (tile.PosX + 1 <= _columns - 1 && tile.PosY - 1 >= 0) tile.NeighborsTiles.Add(GridArray[tile.PosX + 1, tile.PosY - 1]);
            // UP LEFT
            if (tile.PosX - 1 >= 0 && tile.PosY + 1 <= _rows - 1) tile.NeighborsTiles.Add(GridArray[tile.PosX - 1, tile.PosY + 1]);
            // DOWN LEFT
            if (tile.PosX - 1 >= 0 && tile.PosY - 1 >= 0) tile.NeighborsTiles.Add(GridArray[tile.PosX - 1, tile.PosY - 1]);

            //MATRIX//
            //RIGHT
            if (tile.PosX + 1 <= _columns - 1) tile.NeighborsTilesMatrix[ArrayCardinalPosition.EAST.x, ArrayCardinalPosition.EAST.y] = GridArray[tile.PosX + 1, tile.PosY];
            // LEFT
            if (tile.PosX - 1 >= 0) tile.NeighborsTilesMatrix[ArrayCardinalPosition.WEST.x, ArrayCardinalPosition.WEST.y] = GridArray[tile.PosX - 1, tile.PosY];
            // UP
            if (tile.PosY + 1 <= _rows - 1) tile.NeighborsTilesMatrix[ArrayCardinalPosition.NORTH.x, ArrayCardinalPosition.NORTH.y] = GridArray[tile.PosX, tile.PosY + 1];
            // DOWN
            if (tile.PosY - 1 >= 0) tile.NeighborsTilesMatrix[ArrayCardinalPosition.SOUTH.x, ArrayCardinalPosition.SOUTH.y] = GridArray[tile.PosX, tile.PosY - 1];
            // UP RIGHT
            if (tile.PosX + 1 <= _columns - 1 && tile.PosY + 1 <= _rows - 1) tile.NeighborsTilesMatrix[ArrayCardinalPosition.NORTHEAST.x, ArrayCardinalPosition.NORTHEAST.y] = GridArray[tile.PosX + 1, tile.PosY + 1];
            // DOWN RIGHT
            if (tile.PosX + 1 <= _columns - 1 && tile.PosY - 1 >= 0) tile.NeighborsTilesMatrix[ArrayCardinalPosition.SOUTHEAST.x, ArrayCardinalPosition.SOUTHEAST.y] = GridArray[tile.PosX + 1, tile.PosY - 1];
            // UP LEFT
            if (tile.PosX - 1 >= 0 && tile.PosY + 1 <= _rows - 1) tile.NeighborsTilesMatrix[ArrayCardinalPosition.NORTHWEST.x, ArrayCardinalPosition.NORTHWEST.y] = GridArray[tile.PosX - 1, tile.PosY + 1];
            // DOWN LEFT
            if (tile.PosX - 1 >= 0 && tile.PosY - 1 >= 0) tile.NeighborsTilesMatrix[ArrayCardinalPosition.SOUTHWEST.x, ArrayCardinalPosition.SOUTHWEST.y] = GridArray[tile.PosX - 1, tile.PosY - 1];

        }
    }
}
