using PositionerDemo;
using UnityEngine;

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




