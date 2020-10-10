using PositionerDemo;
using UnityEngine;


public enum PLAYERWCONTROLLERTYPE
{
    JOYSTICK,
    TOUCH,
    MOUSE
}

public enum PLAYERWCONTROLLERSCHEME
{
    INGAME,
    INMENU,
    NONE
}

public class PlayerRealWorldController
{
    public int playerID { get; set; }
    protected PLAYERWCONTROLLERTYPE controllerType;
    protected PLAYERWCONTROLLERSCHEME controllerScheme;

    public PlayerRealWorldController(int playerID)
    {
        this.playerID = playerID;
    }

    public virtual Tile GetTile()
    {
        return null;
    }

    public virtual bool Select()
    {
        return true;
    }

    public virtual void SpecialSelection()
    {

    }
}

public class MouseController : PlayerRealWorldController
{
    public MouseController(int playerID) : base(playerID)
    {
        controllerType = PLAYERWCONTROLLERTYPE.MOUSE;
        controllerScheme = PLAYERWCONTROLLERSCHEME.INGAME;
    }

    public override Tile GetTile()
    {
        if (Helper.IsMouseOverUIWithIgnores() == false)
        {
            Tile TileObject = GameCreator.Instance.board2D.GetGridObject(Helper.GetMouseWorldPosition(GameCreator.Instance.cam));
            if (TileObject != null)
            {
                return TileObject;
            }
        }

        return null;
    }

    public override bool Select()
    {
        if (Helper.IsMouseOverUIWithIgnores() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
        }

        return false;
    }
}

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




