using PositionerDemo;
using UnityEngine;

public class MouseController : PlayerRealWorldController
{
    Board2DManager board2DManager;
    Camera cam;
    public MouseController(int playerID) : base(playerID)
    {
        controllerType = PLAYERWCONTROLLERTYPE.MOUSE;
        controllerScheme = PLAYERWCONTROLLERSCHEME.INGAME;
    }

    public MouseController(int playerID, Board2DManager board2DManager, Camera cam) : base(playerID)
    {
        this.board2DManager = board2DManager;
        this.cam = cam;
    }

    public override Tile GetTile()
    {
        if (board2DManager != null && cam != null)
        {
            Tile TileObject = board2DManager.GetGridObject(Helper.GetMouseWorldPosition(cam));
            if (TileObject != null)
            {
                return TileObject;
            }
            else
            {
                return null;
            }
        }

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




