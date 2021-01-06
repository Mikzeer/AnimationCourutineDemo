using UnityEngine;

namespace PositionerDemo
{
    public class TileSelectionManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject tileSelectionPrefab;
        MouseController mouseController;
        private Tile selectedTilePlayerOne;
        private Tile selectedTilePlayerTwo;
        GameObject tileSelectionFramePlayerOne;
        GameObject tileSelectionFramePlayerTwo;
        private Vector2 normalSize;
        Board2DManager board2D;

        private void Awake()
        {
            tileSelectionFramePlayerOne = Instantiate(tileSelectionPrefab);
            tileSelectionFramePlayerTwo = Instantiate(tileSelectionPrefab);

            SpriteRenderer sp = tileSelectionFramePlayerTwo.GetComponent<SpriteRenderer>();

            sp.color = Color.red;
            tileSelectionFramePlayerOne.SetActive(false);
            tileSelectionFramePlayerTwo.SetActive(false);

            normalSize = tileSelectionFramePlayerOne.transform.localScale;
        }

        public void SetController(Board2DManager board2D)
        {
            this.board2D = board2D;
            mouseController = new MouseController(0, board2D, Camera.main);
        }

        private void Update()
        {
            if (mouseController == null) return;

            Tile tileP1 = mouseController.GetTile();
            OnTileSelection(tileP1, true);

            if (mouseController.Select() == true)
            {
                if (tileP1 != null)
                {
                    Debug.Log("SELECT TILE " + tileP1.PosY + "/" + tileP1.PosX);
                }
            }

            mouseController.SpecialSelection();
        }

        public void OnTileSelection(Tile TileObject, bool isPlayerOne)
        {
            if (TileObject != null)
            {
                if (isPlayerOne)
                {
                    if (selectedTilePlayerOne != null)
                    {
                        if (selectedTilePlayerOne == TileObject)
                        {
                            return;
                        }
                        else
                        {
                            selectedTilePlayerOne = TileObject;
                            tileSelectionFramePlayerOne.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        }
                    }
                    else
                    {
                        tileSelectionFramePlayerOne.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        tileSelectionFramePlayerOne.SetActive(true);
                        selectedTilePlayerOne = TileObject;
                    }

                }
                else
                {
                    if (selectedTilePlayerTwo != null)
                    {
                        if (selectedTilePlayerTwo == TileObject)
                        {
                            return;
                        }
                        else
                        {
                            selectedTilePlayerTwo = TileObject;
                            tileSelectionFramePlayerTwo.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        }
                    }
                    else
                    {
                        tileSelectionFramePlayerTwo.transform.position = GetTilePositionAndScale(TileObject, isPlayerOne);
                        tileSelectionFramePlayerTwo.SetActive(true);
                        selectedTilePlayerTwo = TileObject;
                    }
                }
            }
            else
            {

                if (isPlayerOne)
                {
                    if (selectedTilePlayerOne != null)
                    {

                        tileSelectionFramePlayerOne.SetActive(false);
                        selectedTilePlayerOne = null;
                    }

                }
                else
                {
                    //Debug.Log("LLEGUE");
                    if (selectedTilePlayerTwo != null)
                    {

                        tileSelectionFramePlayerTwo.SetActive(false);
                        selectedTilePlayerTwo = null;
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
