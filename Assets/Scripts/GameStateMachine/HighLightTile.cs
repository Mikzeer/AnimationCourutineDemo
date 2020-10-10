using PositionerDemo;
using UnityEngine;

public class HighLightTile
{
    private Tile selectedTilePlayerOne;
    private Tile selectedTilePlayerTwo;
    public GameObject tileSelectionFramePlayerOne;
    public GameObject tileSelectionFramePlayerTwo;

    private Vector2 normalSize;
    //private Vector2 nexuzSize;

    public HighLightTile(GameObject tileSelectionPrefab)
    {
        tileSelectionFramePlayerOne = GameObject.Instantiate(tileSelectionPrefab);
        tileSelectionFramePlayerTwo = GameObject.Instantiate(tileSelectionPrefab);

        SpriteRenderer sp = tileSelectionFramePlayerTwo.GetComponent<SpriteRenderer>();

        sp.color = Color.red;
        tileSelectionFramePlayerOne.SetActive(false);
        tileSelectionFramePlayerTwo.SetActive(false);

        normalSize = tileSelectionFramePlayerOne.transform.localScale;
        //nexuzSize = GameCreator.Instance.board2D.GetNexusSpriteSize() / 4;
    }

    public void OnTileSelection(Tile TileObject, Player highLPlayer)
    {
        //if (Helper.IsMouseOverUIWithIgnores())
        //{
        //    if (highLPlayer.PlayerID == 0)
        //    {
        //        if (selectedTilePlayerOne != null)
        //        {
        //            tileSelectionFramePlayerOne.SetActive(false);
        //            selectedTilePlayerOne = null;
        //        }

        //    }
        //    else
        //    {
        //        if (selectedTilePlayerTwo != null)
        //        {
        //            tileSelectionFramePlayerTwo.SetActive(false);
        //            selectedTilePlayerTwo = null;
        //        }

        //    }
        //    return;
        //}

        if (TileObject != null)
        {
            if (highLPlayer.PlayerID == 0)
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
                        tileSelectionFramePlayerOne.transform.position = GetTilePositionAndScale(TileObject, highLPlayer);
                    }
                }
                else
                {
                    tileSelectionFramePlayerOne.transform.position = GetTilePositionAndScale(TileObject, highLPlayer);
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
                        tileSelectionFramePlayerTwo.transform.position = GetTilePositionAndScale(TileObject, highLPlayer);
                    }
                }
                else
                {
                    tileSelectionFramePlayerTwo.transform.position = GetTilePositionAndScale(TileObject, highLPlayer);
                    tileSelectionFramePlayerTwo.SetActive(true);
                    selectedTilePlayerTwo = TileObject;
                }
            }
        }
        else
        {

            if (highLPlayer.PlayerID == 0)
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

    private Vector3 GetTilePositionAndScale(Tile TileObject, Player highLPlayer)
    {
        if (highLPlayer.PlayerID == 0)
        {
            switch (TileObject.tileType)
            {
                case TILETYPE.BASENEXO:
                    // HARDCODE
                    tileSelectionFramePlayerOne.transform.localScale = new Vector2(2.1f, 5.8f);
                    //tileSelectionFramePlayerOne.transform.localScale = nexuzSize;

                    if (TileObject.GetRealWorldLocation().x < 0)
                    {
                        return GameCreator.Instance.board2D.GetPlayerNexusWorldPosition(GameCreator.Instance.players[0]);
                    }
                    return GameCreator.Instance.board2D.GetPlayerNexusWorldPosition(GameCreator.Instance.players[1]);
                case TILETYPE.SPAWN:
                case TILETYPE.BATTLEFILED:
                    tileSelectionFramePlayerOne.transform.localScale = normalSize;
                    return TileObject.GetRealWorldLocation();
                default:
                    return Vector3.zero;
            }
        }
        else
        {
            switch (TileObject.tileType)
            {
                case TILETYPE.BASENEXO:
                    // HARDCODE
                    tileSelectionFramePlayerTwo.transform.localScale = new Vector2(2.1f, 5.8f);
                    //tileSelectionFramePlayerOne.transform.localScale = nexuzSize;

                    if (TileObject.GetRealWorldLocation().x < 0)
                    {
                        return GameCreator.Instance.board2D.GetPlayerNexusWorldPosition(GameCreator.Instance.players[0]);
                    }
                    return GameCreator.Instance.board2D.GetPlayerNexusWorldPosition(GameCreator.Instance.players[1]);
                case TILETYPE.SPAWN:
                case TILETYPE.BATTLEFILED:
                    tileSelectionFramePlayerTwo.transform.localScale = normalSize;
                    return TileObject.GetRealWorldLocation();
                default:
                    return Vector3.zero;
            }
        }

    }

}