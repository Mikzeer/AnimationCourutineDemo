using System;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Board2DManagerUI : MonoBehaviour
    {
        GameObject tilePrefab;
        GameObject baseTilePrefab;
        GameObject playerTilePrefab;
        SpriteRenderer spRendererTile;
        SpriteRenderer spRendererNexo;
        float boardHolderPosX;
        float boardHolderPosY;
        float tileWidth;
        float tileHeight;
        float playerTileWidth;
        float posInitXJug1;
        float posInitXJug2;

        private void Awake()
        {
            tilePrefab = Resources.Load("TilePrefab/Tile", typeof(GameObject)) as GameObject;
            baseTilePrefab = Resources.Load("TilePrefab/BaseTile", typeof(GameObject)) as GameObject;
            playerTilePrefab = Resources.Load("TilePrefab/PlayerTile", typeof(GameObject)) as GameObject;
            spRendererTile = tilePrefab.GetComponent<SpriteRenderer>();
            spRendererNexo = playerTilePrefab.GetComponent<SpriteRenderer>();
        }

        public Vector3 LoadSizeAndGetOriginPosition(int rowsHeight, int columnsWidht)
        {
            tileWidth = spRendererTile.bounds.size.x;
            tileHeight = spRendererTile.bounds.size.y;
            playerTileWidth = spRendererNexo.bounds.size.x;

            float totalWidth = columnsWidht * tileWidth;
            float totalHeight = rowsHeight * tileHeight;

            boardHolderPosX = 0 - totalWidth / 2 - tileWidth / 2 + tileWidth;
            boardHolderPosY = 0 - totalHeight / 2 - tileHeight / 2 + tileHeight;

            float newWidth = (columnsWidht - 4) * tileWidth;
            float halfBoardSize = newWidth / 2;
            float halfNexusWidth = playerTileWidth / 2;
            posInitXJug1 = -(halfBoardSize + halfNexusWidth);
            posInitXJug2 = -posInitXJug1;

            float originPosX = 0 - totalWidth / 2;
            float originPosY = 0 - totalHeight / 2;
            Vector3 originPosition = new Vector3(originPosX, originPosY, 0);
            return originPosition;
        }

        public Motion CreateNewBoardMotion(Board2DManager board2D, Action OnBoardLoadComplete)
        {
            GameObject[,] tiles = new GameObject[board2D.columnsWidht, board2D.rowsHeight];
            int index = 1;
            GameObject tileParent = new GameObject("TileParent");

            List<Motion> motionsCreateBoard = new List<Motion>();

            for (int x = 0; x < board2D.columnsWidht; x++)
            {
                for (int y = 0; y < board2D.rowsHeight; y++)
                {
                    if (x == 0 || x == 1)
                    {
                        tiles[x, y] = board2D.GridArray[x, y].GetTransform().gameObject;
                        tiles[x, y].transform.SetParent(tileParent.transform);
                        continue;
                    }
                    if (x == 9 || x == 10)
                    {
                        tiles[x, y] = board2D.GridArray[x, y].GetTransform().gameObject;
                        tiles[x, y].transform.SetParent(tileParent.transform);
                        continue;
                    }

                    Vector3 thisTileFinalPosition = board2D.GetGridObject(x, y).GetRealWorldLocation();

                    tiles[x, y] = board2D.GridArray[x, y].GetTransform().gameObject;
                    tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                    tiles[x, y].transform.SetParent(tileParent.transform);

                    // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                    Motion motionTweenMove = new MoveTweenMotion(this, tiles[x, y].transform, index, thisTileFinalPosition, 1);
                    motionsCreateBoard.Add(motionTweenMove);
                }
                index++;
            }

            // para las spawn tiles
            Vector2 yOffset = new Vector2(0, 10);

            Vector3 pOneNexusFinalPosition = board2D.GetPlayerNexusWorldPosition(true);
            tiles[0, 0].transform.position = new Vector3(pOneNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
            Motion motionTweenNexusP1Move = new MoveTweenMotion(this, tiles[0, 0].transform, index, pOneNexusFinalPosition, 1);
            motionsCreateBoard.Add(motionTweenNexusP1Move);

            Vector3 pTwoNexusFinalPosition = board2D.GetPlayerNexusWorldPosition(false);
            tiles[9, 0].transform.position = new Vector3(pTwoNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
            Motion motionTweenNexusP2Move = new MoveTweenMotion(this, tiles[9, 0].transform, index, pTwoNexusFinalPosition, 1);
            motionsCreateBoard.Add(motionTweenNexusP2Move);

            index++;

            List<Configurable> configurables = new List<Configurable>();
            EventInvokerGenericContainer evenToInvoke = new EventInvokerGenericContainer(OnBoardLoadComplete);
            InvokeEventConfigureAnimotion<EventInvokerGenericContainer, Transform> onBoardCompleteInvokeEvent = 
                new InvokeEventConfigureAnimotion<EventInvokerGenericContainer, Transform>(evenToInvoke, index);
            configurables.Add(onBoardCompleteInvokeEvent);

            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCreateBoard, configurables);
            return combinMoveMotion;
        }

        public void CreateSpawnTileHorizontal(int posX, int posY, SpawnTile spawnTile)
        {
            Vector2 position = new Vector2(boardHolderPosX + posX * tileHeight, boardHolderPosY + posY * tileWidth);
            GameObject goAuxTile = Instantiate(baseTilePrefab, position, Quaternion.identity);
            spawnTile.SetGameObject(goAuxTile);
        }

        public void CreateBattlefiledTileHorizontal(int posX, int posY, BattlefieldTile battlefieldTile)
        {
            Vector2 position = new Vector2(boardHolderPosX + posX * tileHeight, boardHolderPosY + posY * tileWidth);
            GameObject goAuxTile = Instantiate(tilePrefab, position, Quaternion.identity);
            battlefieldTile.SetGameObject(goAuxTile);
        }

        public void CreateBaseNexoTile(BaseNexoTile baseNexoTile, bool isPlayerOne)
        {
            Vector2 position = Vector2.zero;
            if (isPlayerOne)
            {
                position = new Vector2(posInitXJug1, 0);
            }
            else
            {
                position = new Vector2(posInitXJug2, 0);
            }
            GameObject goAuxTile = Instantiate(playerTilePrefab, position, Quaternion.identity);
            baseNexoTile.SetGameObject(goAuxTile);
        }

        public Vector3 GetPlayerNexusWorldPosition(bool isPlayerOne)
        {
            if (isPlayerOne) return new Vector2(posInitXJug1, 0);
            return new Vector2(posInitXJug2, 0);
        }

        public Vector3 GetNexusSpriteSize()
        {
            return spRendererNexo.bounds.size;
        }
    }
}
