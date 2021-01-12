using PositionerDemo;
using UnityEngine;

public class AdministrationState : TimeConditionState
{
    private const string name = "ADMINISTRATION";
    private int managmentPoints;

    public AdministrationState(int duration, GameCreator gameCreator, int managmentPoints) : base(duration, gameCreator, name)
    {
        this.managmentPoints = managmentPoints;
    }

    public override void Enter()
    {
        base.Enter();
        gameCreator.TakeCardAvailable(true);
        // ACA TENGO QUE PRENDER EL BOTON DE CARD
        //Debug.Log("Enter Administration State Player " + GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID);
    }

    public override void Exit()
    {
        // ACA TENGO QUE APAGAR EL BOTON DE CARD
        gameCreator.TakeCardAvailable(false);
        base.Exit();
    }

    public override bool MeetCondition()
    {
        if (gameCreator.turnManager.GetActualPlayerTurn().GetCurrentActionPoints() <= 0)
        {
            return true;
        }
        else if (gameTimer.running == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void GetBack()
    {
        base.GetBack();
    }

    public override State Update()
    {
        base.Update();
        Tile TileObject = gameCreator.board2D.GetGridObject(Helper.GetMouseWorldPosition(gameCreator.cam));
        if (Helper.IsMouseOverUIWithIgnores() == false)
        {            
            if (Input.GetMouseButtonDown(0))
            {
                if (TileObject != null)
                {
                    gameCreator.spawnCotroller.OnTrySpawn(TileObject, gameCreator.turnManager.GetActualPlayerTurn());
                }
            }
        }

        gameCreator.highLightTile.OnTileSelection(TileObject, gameCreator.turnManager.GetActualPlayerTurn());

        if (MeetCondition())
        {
            State nextState = new AdministrationState(15, gameCreator, 2);
            gameCreator.highLightTile.OnTileSelection(null, gameCreator.turnManager.GetActualPlayerTurn());
            return gameCreator.turnManager.ChangeTurnState(managmentPoints, nextState);
            //return nextState;
        }
        else
        {
            return null;
        }
    }
}