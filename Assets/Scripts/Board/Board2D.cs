using UnityEngine;

namespace PositionerDemo
{
    public class Board2D
    {
        #region VARIABLES
        private Tile[,] _gridArray; // Aca van a estar guardadas todas las tiles
        public Tile[,] GridArray { get => _gridArray; private set => _gridArray = value; }

        private int _columns; // widht
        private int _rows; // height

        private Vector3 originPosition;
        private float tileSize;

        private GameObject tilePrefab;
        private GameObject baseTilePrefab;
        private GameObject playerTilePrefab;

        float playerTileWidth;

        float tileWidth;
        float tileHeight;
        float totalWidth;
        float totalHeight;

        float boardHolderPosX;
        float boardHolderPosY;
        #endregion

        public Board2D(int rowsHeight, int columnsWidht, Player[] players, Vector3 originPosition)
        {
            // a la cantidad de columnas que tenga nuestro grid le vamos a agregar dos de cada lado
            // entonces si es de 5 filas por 7 columnas va a quedar de 5 filas por 11 columnas
            // le sumamos 4 para el espacio de la base del jugado 1 y del jugador 2
            _rows = rowsHeight;
            _columns = columnsWidht + 4;

            LoadTilePrefab();

            tileSize = 4;

            //this.originPosition = new Vector3(-22,-10,0);
            //this.originPosition = originPosition;
            boardHolderPosX = 0 - totalWidth / 2 - tileWidth / 2 + tileWidth;
            boardHolderPosY = 0 - totalHeight / 2 - tileHeight / 2 + tileHeight;

            float originPosX = 0 - totalWidth / 2;
            float originPosY = 0 - totalHeight / 2;

            //this.originPosition = new Vector3(boardHolderPosX, boardHolderPosY, 0);
            this.originPosition = new Vector3(originPosX, originPosY, 0);

            GridArray = new Tile[_columns, _rows];

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
                        CreateSpawnTileHorizontal(x, y, players[0]);
                    }
                    // todas las posiciones de la columna 8 van a estar ocupadas por la spawn tile del player 2
                    else if (x == _columns - 3)
                    {
                        // spawn =     posx, posy, grid playerID
                        CreateSpawnTileHorizontal(x, y, players[1]);
                    }
                    else
                    {
                        // battlefield posx, posy, grid
                        CreateBattlefiledTileHorizontal(x, y);
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

        private void LoadTilePrefab()
        {
            tilePrefab = Resources.Load("TilePrefab/Tile", typeof(GameObject)) as GameObject;
            baseTilePrefab = Resources.Load("TilePrefab/BaseTile", typeof(GameObject)) as GameObject;
            playerTilePrefab = Resources.Load("TilePrefab/PlayerTile", typeof(GameObject)) as GameObject;
            //elevatorSprite = Resources.LoadAll<Sprite>("Elevator");
            SpriteRenderer spRendererTile = tilePrefab.GetComponent<SpriteRenderer>();
            tileWidth = spRendererTile.bounds.size.x;
            tileHeight = spRendererTile.bounds.size.y;
            tileSize = tileWidth * tileHeight;

            SpriteRenderer spRendererNexo = playerTilePrefab.GetComponent<SpriteRenderer>();
            playerTileWidth = spRendererNexo.bounds.size.x;


            totalWidth = _columns * tileWidth;
            totalHeight = _rows * tileHeight;
        }

        private void CreateSpawnTileHorizontal(int posX, int posY, Player player)
        {
            Vector3 realWorldPosition = GetGridObjectRealWorldPositionByArrayPosition(posX, posY);
            SpawnTile spawnTile = new SpawnTile(realWorldPosition, posX, posY, player.PlayerID);
            Vector2 position = new Vector2(boardHolderPosX + posX * tileHeight, boardHolderPosY + posY * tileWidth);
            Vector2 position2 = GetWorldPosition(posX, posY);
            GameObject goAuxTile = MonoBehaviour.Instantiate(baseTilePrefab, position, Quaternion.identity);

            spawnTile.SetGoAnimContainer(new GameObjectAnimatorContainer(goAuxTile, goAuxTile.GetComponent<Animator>()));
            GridArray[posX, posY] = spawnTile;
        }

        private void CreateBattlefiledTileHorizontal(int posX, int posY)
        {
            Vector3 realWorldPosition = GetGridObjectRealWorldPositionByArrayPosition(posX, posY);
            BattlefieldTile battlefieldTile = new BattlefieldTile(realWorldPosition, posX, posY);


            Vector2 position = new Vector2(boardHolderPosX + posX * tileHeight, boardHolderPosY + posY * tileWidth);
            Vector2 position2 = GetWorldPosition(posX, posY);
            GameObject goAuxTile = MonoBehaviour.Instantiate(tilePrefab, position, Quaternion.identity);
            battlefieldTile.SetGoAnimContainer(new GameObjectAnimatorContainer(goAuxTile, goAuxTile.GetComponent<Animator>()));
            GridArray[posX, posY] = battlefieldTile;
        }

        private BaseNexoTile CreateBaseNexoTile(Player player)
        {
            BaseNexoTile baseNexoTile = null;

            float newWidth = (_columns - 4) * tileWidth;

            float halfBoardSize = newWidth / 2;
            float halfNexusWidth = playerTileWidth / 2;
            float posInitXJug1 = -(halfBoardSize + halfNexusWidth);
            float posInitXJug2 = -posInitXJug1;

            Vector2 position = Vector2.zero;

            switch (player.PlayerID)
            {
                case 0:
                    position = GetPlayerNexusWorldPosition(player);
                    baseNexoTile = new BaseNexoTile(position, 0, 0, player);
                    break;
                case 1:
                    position = GetPlayerNexusWorldPosition(player);
                    baseNexoTile = new BaseNexoTile(position, 9, 0, player);
                    break;
                default:
                    break;
            }

            GameObject goAuxTile = MonoBehaviour.Instantiate(playerTilePrefab, position, Quaternion.identity);
            baseNexoTile.SetGoAnimContainer(new GameObjectAnimatorContainer(goAuxTile, goAuxTile.GetComponent<Animator>()));

            return baseNexoTile;
        }

        public Vector3 GetPlayerNexusWorldPosition(Player player)
        {
            float newWidth = (_columns - 4) * tileWidth;

            float halfBoardSize = newWidth / 2;
            float halfNexusWidth = playerTileWidth / 2;
            float posInitXJug1 = -(halfBoardSize + halfNexusWidth);
            float posInitXJug2 = -posInitXJug1;

            Vector2 position = Vector2.zero;

            switch (player.PlayerID)
            {
                case 0:
                    position = new Vector2(posInitXJug1, 0);
                    return position;
                case 1:
                    position = new Vector2(posInitXJug2, 0);
                    return position;
                default:
                    return Vector3.zero;
            }
        }

        private void SetNeighborsTiles(Tile tile)
        {
            //if (tile.PosX >= 0 && tile.PosY >= 0 && tile.PosX < _columns && tile.PosY < _rows)
            //{

            //}
            //MATRIX//
            //RIGHT
            if (tile.position.posX + 1 <= _columns - 1)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.EAST.x, ArrayCardinalPosition.EAST.y] = GridArray[tile.position.posX + 1, tile.position.posY];
            // LEFT
            if (tile.position.posX - 1 >= 0)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.WEST.x, ArrayCardinalPosition.WEST.y] = GridArray[tile.position.posX - 1, tile.position.posY];            
            // UP
            if (tile.position.posY + 1 <= _rows - 1)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.NORTH.x, ArrayCardinalPosition.NORTH.y] = GridArray[tile.position.posX, tile.position.posY + 1];
            // DOWN
            if (tile.position.posY - 1 >= 0)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.SOUTH.x, ArrayCardinalPosition.SOUTH.y] = GridArray[tile.position.posX, tile.position.posY - 1];
            // UP RIGHT
            if (tile.position.posX + 1 <= _columns - 1 && tile.position.posY + 1 <= _rows - 1)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.NORTHEAST.x, ArrayCardinalPosition.NORTHEAST.y] = GridArray[tile.position.posX + 1, tile.position.posY + 1];            
            // DOWN RIGHT
            if (tile.position.posX + 1 <= _columns - 1 && tile.position.posY - 1 >= 0)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.SOUTHEAST.x, ArrayCardinalPosition.SOUTHEAST.y] = GridArray[tile.position.posX + 1, tile.position.posY - 1];            
            // UP LEFT
            if (tile.position.posX - 1 >= 0 && tile.position.posY + 1 <= _rows - 1)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.NORTHWEST.x, ArrayCardinalPosition.NORTHWEST.y] = GridArray[tile.position.posX - 1, tile.position.posY + 1];
            // DOWN LEFT
            if (tile.position.posX - 1 >= 0 && tile.position.posY - 1 >= 0)
                tile.NeighborsTilesMatrix[ArrayCardinalPosition.SOUTHWEST.x, ArrayCardinalPosition.SOUTHWEST.y] = GridArray[tile.position.posX - 1, tile.position.posY - 1];

        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * tileSize + originPosition;
        }

        private void GetXY(Vector3 worldposition, out int x, out int y)
        {
            Vector3 diference = worldposition - originPosition;

            x = Mathf.FloorToInt((worldposition - originPosition).x / tileSize);
            y = Mathf.FloorToInt((worldposition - originPosition).y / tileSize);
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

        public Vector3 GetGridObjectRealWorldPosition(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            Vector3 offsetPosition = GetWorldPosition(x, y);
            offsetPosition = offsetPosition + new Vector3(tileSize / 2, tileSize / 2, 0);
            return offsetPosition;
        }

        public Vector3 GetGridObjectRealWorldPositionByArrayPosition(int x, int y)
        {
            Vector3 offsetPosition = GetWorldPosition(x, y);
            offsetPosition = offsetPosition + new Vector3(tileSize / 2, tileSize / 2, 0);
            return offsetPosition;
        }

        public float GetTileSize()
        {
            return tileSize;
        }

        public Vector3 GetNexusSpriteSize()
        {
            SpriteRenderer spRendererNexo = playerTilePrefab.GetComponent<SpriteRenderer>();            
            return spRendererNexo.bounds.size;
        }

    }
}
