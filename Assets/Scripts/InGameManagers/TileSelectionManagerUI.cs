using System;
using UnityEngine;

namespace PositionerDemo
{
    public class TileSelectionManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject tileSelectionPrefab = default;
        MouseController mouseController;
        KeyBoardController keyBoardController;
        public Tile SelectedTilePlayerOne { get; private set; }
        public Tile SelectedTilePlayerTwo { get; private set; }
        GameObject tileSelectionFramePlayerOne;
        GameObject tileSelectionFramePlayerTwo;
        private Vector2 normalSize;
        Board2DManager board2D;
        public PLAYERWCONTROLLERTYPE controlerType = PLAYERWCONTROLLERTYPE.MOUSE;
        public Action<Tile> onTileSelected;

        private void Awake()
        {
            tileSelectionFramePlayerOne = Instantiate(tileSelectionPrefab);
            tileSelectionFramePlayerTwo = Instantiate(tileSelectionPrefab);
            SpriteRenderer spOne = tileSelectionFramePlayerOne.GetComponent<SpriteRenderer>();
            spOne.sortingOrder = 5;
            SpriteRenderer sp = tileSelectionFramePlayerTwo.GetComponent<SpriteRenderer>();
            sp.sortingOrder = 5;
            sp.color = Color.red;
            tileSelectionFramePlayerOne.SetActive(false);
            tileSelectionFramePlayerTwo.SetActive(false);

            normalSize = tileSelectionFramePlayerOne.transform.localScale;
        }

        public void SetController(Board2DManager board2D, MouseController mouseController, KeyBoardController keyBoardController)
        {
            this.board2D = board2D;
            this.mouseController = mouseController;
            this.keyBoardController = keyBoardController;
        }

        private void Update()
        {
            switch (controlerType)
            {
                case PLAYERWCONTROLLERTYPE.JOYSTICK:
                    if (keyBoardController == null) return;
                    Tile tileP1 = keyBoardController.GetTile();
                    OnTileSelection(tileP1, true);
                    if (keyBoardController.Select() == true)
                    {
                        if (tileP1 != null)
                        {
                            onTileSelected?.Invoke(tileP1);
                            //Debug.Log("SELECT TILE " + tileP1.PosY + "/" + tileP1.PosX);
                        }
                    }
                    break;
                case PLAYERWCONTROLLERTYPE.TOUCH:
                    break;
                case PLAYERWCONTROLLERTYPE.MOUSE:
                    if (mouseController == null) return;
                    tileP1 = mouseController.GetTile();
                    OnTileSelection(tileP1, true);
                    if (mouseController.Select() == true)
                    {
                        if (tileP1 != null)
                        {
                            onTileSelected?.Invoke(tileP1);
                            //Debug.Log("SELECT TILE " + tileP1.PosY + "/" + tileP1.PosX);
                        }
                    }
                    //mouseController.SpecialSelection();
                    break;
                default:
                    break;
            }
        }

        public void OnTileSelection(Tile TileObject, bool isPlayerOne)
        {
            if (TileObject != null)
            {
                if (isPlayerOne)
                {
                    if (SelectedTilePlayerOne != null)
                    {
                        if (SelectedTilePlayerOne == TileObject)
                        {
                            return;
                        }
                        else
                        {
                            SelectedTilePlayerOne = TileObject;
                            tileSelectionFramePlayerOne.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        }
                    }
                    else
                    {
                        tileSelectionFramePlayerOne.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        tileSelectionFramePlayerOne.SetActive(true);
                        SelectedTilePlayerOne = TileObject;
                    }
                }
                else
                {
                    if (SelectedTilePlayerTwo != null)
                    {
                        if (SelectedTilePlayerTwo == TileObject)
                        {
                            return;
                        }
                        else
                        {
                            SelectedTilePlayerTwo = TileObject;
                            tileSelectionFramePlayerTwo.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        }
                    }
                    else
                    {
                        tileSelectionFramePlayerTwo.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        tileSelectionFramePlayerTwo.SetActive(true);
                        SelectedTilePlayerTwo = TileObject;
                    }
                }
            }
            else
            {

                if (isPlayerOne)
                {
                    if (SelectedTilePlayerOne != null)
                    {
                        tileSelectionFramePlayerOne.SetActive(false);
                        SelectedTilePlayerOne = null;
                    }
                }
                else
                {
                    //Debug.Log("LLEGUE");
                    if (SelectedTilePlayerTwo != null)
                    {
                        tileSelectionFramePlayerTwo.SetActive(false);
                        SelectedTilePlayerTwo = null;
                    }
                }
            }
        }

        private Vector3 GetTilePositionAndScale(Tile TileObject, bool isPlayerOne)
        {
            GameObject goAux;
            if (isPlayerOne)
            {
                goAux = tileSelectionFramePlayerOne;
            }
            else
            {
                goAux = tileSelectionFramePlayerTwo;
            }

            switch (TileObject.tileType)
            {
                case TILETYPE.BASENEXO:
                    // HARDCODE
                    goAux.transform.localScale = new Vector2(2.1f, 5.8f);
                    //tileSelectionFramePlayerOne.transform.localScale = nexuzSize;
                    if (TileObject.GetRealWorldLocation().x < 0)
                    {
                        return board2D.GetPlayerNexusWorldPosition(true);
                    }
                    return board2D.GetPlayerNexusWorldPosition(false);
                case TILETYPE.SPAWN:
                case TILETYPE.BATTLEFILED:
                    goAux.transform.localScale = normalSize;
                    return TileObject.GetRealWorldLocation();
                default:
                    return Vector3.zero;
            }
        } 
    }
}