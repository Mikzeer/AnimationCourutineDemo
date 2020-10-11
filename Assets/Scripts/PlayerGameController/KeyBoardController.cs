using PositionerDemo;
using UnityEngine;

public class KeyBoardController : PlayerRealWorldController
{
    int width;
    int height;

    int actualPosX;
    int actualPosY;

    public KeyBoardController(int playerID) : base(playerID)
    {
        controllerType = PLAYERWCONTROLLERTYPE.JOYSTICK;
        controllerScheme = PLAYERWCONTROLLERSCHEME.INGAME;

        actualPosX = GameCreator.Instance.board2D.GridArray.GetLength(0);
        actualPosY = GameCreator.Instance.board2D.GridArray.GetLength(1);
        width = GameCreator.Instance.board2D.GridArray.GetLength(0);
        height = GameCreator.Instance.board2D.GridArray.GetLength(1);

        //actualPosX = Mathf.FloorToInt(actualPosX / 2);
        actualPosX = width - 2;
        actualPosY = Mathf.FloorToInt(actualPosY / 2);
    }

    public override Tile GetTile()
    {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (actualPosY + 1 < height)
            {
                actualPosY++;
            }
            else
            {
                actualPosY = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {

            if (actualPosY - 1 >= 0)
            {
                actualPosY--;
            }
            else
            {
                actualPosY = height - 1;
            }

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (actualPosX - 1 >= 1)
            {
                actualPosX--;
            }
            else
            {
                actualPosX = width - 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (actualPosX + 1 < width - 1)
            {
                actualPosX++;
            }
            else
            {
                actualPosX = 1;
            }


        }

        Tile TileObject = GameCreator.Instance.board2D.GetGridObject(actualPosX, actualPosY);        

        if (TileObject != null)
        {
            return TileObject;
        }

        return null;
    }

    public override bool Select()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            return true;
        }

        return false;
    }

    public override void SpecialSelection()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameCreator.Instance.TryTakeCard(GameCreator.Instance.players[playerID]);
        }
    }

}




