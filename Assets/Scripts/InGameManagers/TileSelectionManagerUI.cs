using MikzeerGame.UI;
using System;
using UnityEngine;
namespace PositionerDemo
{
    public class TileSelectionManagerUI : MonoBehaviour
    {
        [SerializeField] private OcuppierInformationPanel ocuppierInformationPanel = default;
        [SerializeField] private GameObject tileSelectionPrefab = default;
        MouseController mouseController;
        KeyBoardController keyBoardController;
        public Tile SelectedTilePlayerOne { get; private set; }
        public Tile SelectedTilePlayerTwo { get; private set; }
        GameObject tileSelectionFramePlayerOne;
        GameObject tileSelectionFramePlayerTwo;
        private Vector2 normalSize;
        private Vector2 spawnZise = new Vector2(2.1f, 5.8f);
        Board2DManager board2D;
        public PLAYERWCONTROLLERTYPE controlerType = PLAYERWCONTROLLERTYPE.MOUSE;
        public Action<Tile> onTileSelected;

        private void Awake()
        {
            tileSelectionFramePlayerOne = Instantiate(tileSelectionPrefab);
            tileSelectionFramePlayerTwo = Instantiate(tileSelectionPrefab);
            SpriteRenderer spOne = tileSelectionFramePlayerOne.GetComponent<SpriteRenderer>();
            spOne.sortingOrder = 4;
            SpriteRenderer sp = tileSelectionFramePlayerTwo.GetComponent<SpriteRenderer>();
            sp.sortingOrder = 4;
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
                    if (tileP1 != null)
                    {
                        CreateOcuppierInformationPanel(tileP1.GetOcuppy());
                    }
                    else
                    {
                        CreateOcuppierInformationPanel(null);
                    }

                    if (keyBoardController.Select() == true)
                    {
                        onTileSelected?.Invoke(tileP1);
                    }
                    break;
                case PLAYERWCONTROLLERTYPE.TOUCH:
                    break;
                case PLAYERWCONTROLLERTYPE.MOUSE:
                    if (mouseController == null) return;
                    tileP1 = mouseController.GetTile();
                    OnTileSelection(tileP1, true);
                    if (tileP1 != null)
                    {
                        CreateOcuppierInformationPanel(tileP1.GetOcuppy());
                    }
                    else
                    {
                        CreateOcuppierInformationPanel(null);
                    }
                    if (mouseController.Select() == true)
                    {
                        onTileSelected?.Invoke(tileP1);
                    }
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
                    if (SelectedTilePlayerTwo != null)
                    {
                        tileSelectionFramePlayerTwo.SetActive(false);
                        SelectedTilePlayerTwo = null;
                    }
                }
            }
        }

        public void ChangeTileSelectionPosition(bool isPlayerOne, Vector3 newPosition)
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

            goAux.transform.position = newPosition;
        }

        public Vector3 GetTilePosition(Tile TileObject, bool isPlayerOne)
        {
            switch (TileObject.tileType)
            {
                case TILETYPE.BASENEXO:
                    // HARDCODE
                    if (TileObject.GetRealWorldLocation().x < 0)
                    {
                        return board2D.GetPlayerNexusWorldPosition(true);
                    }
                    return board2D.GetPlayerNexusWorldPosition(false);
                case TILETYPE.SPAWN:
                case TILETYPE.BATTLEFILED:
                    return TileObject.GetRealWorldLocation();
                default:
                    return Vector3.zero;
            }
        }

        public void ChangeTileSelectionLocalScale(Tile TileObject, bool isPlayerOne)
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
                    goAux.transform.localScale = spawnZise;
                    break;
                case TILETYPE.SPAWN:
                case TILETYPE.BATTLEFILED:
                    goAux.transform.localScale = normalSize;
                    break;
                default:
                    break;
            }
        }

        public void SetActiveTileSelectionPosition(bool isPlayerOne, bool isActive)
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
            goAux.SetActive(isActive);
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

        public void CreateOcuppierInformationPanel(IOcuppy ocuppier)
        {
            ocuppierInformationPanel.CreateOcuppierStatInformationPanel(ocuppier);
        }
    }
}