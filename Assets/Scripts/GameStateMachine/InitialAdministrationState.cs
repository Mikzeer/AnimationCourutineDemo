using PositionerDemo;
using UnityEngine;

public class InitialAdministrationState : AdministrationState
{
    private int managmentPoints;
    int index;
    MouseController mouseController;
    KeyBoardController keyBoardController;
    public InitialAdministrationState(int duration, GameCreator gameCreator, int mngPoints, int index) : base(duration, gameCreator, mngPoints)
    {
        managmentPoints = mngPoints;
        this.index = index;
        mouseController = new MouseController(0);
        keyBoardController = new KeyBoardController(1);
    }

    public override void Enter()
    {
        base.Enter();
        //GameCreator.Instance.turnManager.ChangeTurn(managmentPoints);
        //Debug.Log("Enter InitialAdministrationState State Player "
        //           + GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID 
        //           + " Index INITADMSTATE: "
        //           + index
        //           + " Mis Action Points Son: "
        //           + GameCreator.Instance.turnManager.GetActualPlayerTurn().Stats[4].ActualStatValue);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override bool CheckCondition()
    {
        return base.CheckCondition();
    }

    public override void GetBack()
    {
        base.GetBack();
    }

    public override State Update()
    {
        gameTimer.RestTime();

        //Tile TileObject = GameCreator.Instance.board2D.GetGridObject(Helper.GetMouseWorldPosition(GameCreator.Instance.cam));

        //if (Helper.IsMouseOverUIWithIgnores() == false)
        //{
           
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        if (TileObject != null)
        //        {
        //            if (GameCreator.Instance.turnManager.GetActualPlayerTurn().Abilities[0].OnTryEnter() == true)
        //            {
        //                GameCreator.Instance.spawnCotroller.OnTrySpawn(TileObject, GameCreator.Instance.turnManager.GetActualPlayerTurn());
        //            }
        //            else
        //            {
        //                Debug.Log("CANT ENTER");
        //            }
        //        }
        //    }
        //}

        Tile tileP1 = mouseController.GetTile();
        Tile tileP2 = keyBoardController.GetTile();

        GameCreator.Instance.highLightTile.OnTileSelection(tileP1, GameCreator.Instance.players[0]);
        GameCreator.Instance.highLightTile.OnTileSelection(tileP2, GameCreator.Instance.players[1]);

        if (mouseController.playerID == GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID)
        {
            if (mouseController.Select() == true)
            {
                if (tileP1 != null)
                {
                    if (GameCreator.Instance.turnManager.GetActualPlayerTurn().Abilities[0].OnTryEnter() == true)
                    {
                        GameCreator.Instance.spawnCotroller.OnTrySpawn(tileP1, GameCreator.Instance.turnManager.GetActualPlayerTurn());
                    }
                    else
                    {
                        Debug.Log("CANT ENTER");
                    }
                }
            }

            mouseController.SpecialSelection();
        }

        if (keyBoardController.playerID == GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID)
        {
            if (keyBoardController.Select() == true)
            {
                if (tileP2 != null)
                {
                    if (GameCreator.Instance.turnManager.GetActualPlayerTurn().Abilities[0].OnTryEnter() == true)
                    {
                        GameCreator.Instance.spawnCotroller.OnTrySpawn(tileP2, GameCreator.Instance.turnManager.GetActualPlayerTurn());
                    }
                    else
                    {
                        Debug.Log("CANT ENTER");
                    }
                }
            }

            keyBoardController.SpecialSelection();
        }


        if (CheckCondition())
        {
            State nextState = null;
            int mngPts = 0;
            switch (index)
            {
                case 0:
                    nextState = new InitialAdministrationState(20, GameCreator.Instance, 2, 1);
                    mngPts = 2;
                    break;
                case 1:
                    nextState = new InitialAdministrationState(20, GameCreator.Instance, 2, 2);
                    mngPts = 2;
                    break;
                case 2:
                    nextState = new InitialAdministrationState(40, GameCreator.Instance, 4, 3);
                    mngPts = 4;
                    break;
                case 3:
                    nextState = new AdministrationState(15, GameCreator.Instance, 1);
                    mngPts = 1;
                    break;
                default:
                    nextState = null;
                    break;
            }

            //return nextState;
            GameCreator.Instance.highLightTile.OnTileSelection(null, GameCreator.Instance.turnManager.GetActualPlayerTurn());
            return GameCreator.Instance.turnManager.ChangeTurnState(mngPts, nextState);
        }
        else
        {
            return null;
        }
    }

}