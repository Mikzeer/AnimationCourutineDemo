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
        GameCreator.Instance.TakeCardAvailable(true);
        // ACA TENGO QUE PRENDER EL BOTON DE CARD
        //Debug.Log("Enter Administration State Player " + GameCreator.Instance.turnManager.GetActualPlayerTurn().PlayerID);
    }

    public override void Exit()
    {
        // ACA TENGO QUE APAGAR EL BOTON DE CARD
        GameCreator.Instance.TakeCardAvailable(false);
        base.Exit();
    }

    public override bool CheckCondition()
    {
        if (GameCreator.Instance.turnManager.GetActualPlayerTurn().GetCurrentActionPoints() <= 0)
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

        if (Helper.IsMouseOverUIWithIgnores() == false)
        {            
            if (Input.GetMouseButtonDown(0))
            {
                Tile TileObject = GameCreator.Instance.board2D.GetGridObject(Helper.GetMouseWorldPosition(GameCreator.Instance.cam));

                if (TileObject != null)
                {
                    if (GameCreator.Instance.turnManager.GetActualPlayerTurn().Abilities[0].OnTryEnter() == true)
                    {
                        GameCreator.Instance.spawnCotroller.OnTrySpawn(TileObject, GameCreator.Instance.turnManager.GetActualPlayerTurn());
                    }
                    else
                    {
                        Debug.Log("CANT ENTER");
                    }
                }
            }
        }

        if (CheckCondition())
        {
            State nextState = new AdministrationState(15, GameCreator.Instance, 2);
           
            return GameCreator.Instance.turnManager.ChangeTurnState(managmentPoints, nextState);
            //return nextState;
        }
        else
        {
            return null;
        }
    }
}