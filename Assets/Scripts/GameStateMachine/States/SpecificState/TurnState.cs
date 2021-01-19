using UIButtonPattern;

public class TurnState : TimeConditionState
{
    private const string name = "TURN STATE";
    //SelectInputIClickeable selectInput;
    //SelectionEmptyTile selectionEmptyTile;
    //SelectionPlayer selectionPlayer;
    //SelectionUnit selectionUnit;
    //HighLightUnitMenu highLightUnitMenu;
    //UIInputActionPattern uiInputActions;

    public TurnState(int duration, GameCreator gameCreator) : base(duration, gameCreator, name)
    {
    }

    public override void Enter()
    {
        //base.Enter();
        //CreateSelectionControl();
        //CreateButtonSelectionControl();
        //selectInput.Suscribe();
        //uiInputActions.Suscribe();
        //for (int i = 0; i < gameCreator.actualPlayerTurn.units.Count; i++)
        //{
        //    gameCreator.actualPlayerTurn.units[i].SetActionPoints(2);
        //    gameCreator.actualPlayerTurn.units[i].attackAbility.actionStatus = MikzeerGameNotas.ACTIONEXECUTIONSTATUS.WAIT;
        //    gameCreator.actualPlayerTurn.units[i].moveAbility.actionStatus = MikzeerGameNotas.ACTIONEXECUTIONSTATUS.WAIT;
        //    gameCreator.actualPlayerTurn.units[i].defendAbility.actionStatus = MikzeerGameNotas.ACTIONEXECUTIONSTATUS.WAIT;
        //}
        ////Debug.Log("Enter Turn State Player " + gameCreator.actualPlayerTurn.ID);
    }

    public override void Exit()
    {
        //selectInput.Unsuscribe();
        //uiInputActions.Unsuscribe();
        //highLightUnitMenu.Unexecute();
    }

    public void CreateSelectionControl()
    {
        //// LEFT CLICK INPUT EN EL CASO DE ESTAR USANDO UNA COMPUTADORA
        //InputAction leftClickInputAction = new Controls().PlayerSelection.Select;
        //InputEventSelection leftClickSelectionCmd = new ISelectIClickeableByMousePositionCommand();
        //InputPartners leftClickPartners = new InputPartners(leftClickInputAction, leftClickSelectionCmd);
        //InputActionPattern leftClickInputPattern = new InputActionPattern(leftClickPartners);

        //// KEYBOARD INPUT.. AHORA ES SPACEBAR PARA CANCELAR TODAS LAS ACCIONES YA QUE MANDA UN TRANSFORM NULL, 
        //// DESPUES SE PODRIA AGREGAR WASD + FLECHITAS PARA MOVERSE POR LAS TILES VA A SER DIFICIL YA QUE EN LA BASE SI SELECCIONAMOS UNA TILA SPAWNEAMOS EN ADM STATE....
        //InputAction spacebarInputAction = new Controls().PlayerSelection.KeyBoardSelect;
        //InputEventSelection keyboardSelection = new ISelectIClickeableByKeyboardCommand();
        //InputPartners spaceBarPartners = new InputPartners(spacebarInputAction, keyboardSelection);
        //InputActionPattern spaceBarInputPattern = new InputActionPattern(spaceBarPartners);


        //InputAction leftClickHoldInputAction = new Controls().PlayerSelection.HoldSelect;
        //InputEventSelection LefClickHoldSelection = new ISelectIClickeableCardByMousePositionCommand(gameCreator.m_EventSystem, gameCreator.m_Raycaster);
        //InputPartners HoldLeftClikcPartners = new InputPartners(leftClickHoldInputAction, LefClickHoldSelection);
        //InputActionPattern HoldLeftClikcPattern = new InputActionPattern(HoldLeftClikcPartners);


        //// ESTA ES LA LISTA DE TODO LO QUE VA A DEVOLVER UN TRANSFORM
        //List<InputActionPattern> inputPattern = new List<InputActionPattern>();
        //inputPattern.Add(leftClickInputPattern);
        //inputPattern.Add(spaceBarInputPattern);
        //inputPattern.Add(HoldLeftClikcPattern);


        //// RECIBE EL TRANSFORM Y ENVIA EL ICLICKEABLE A TODOS LOS QUE ESTEN ESCUCHANDO EL EVENTO
        //selectInput = new SelectInputIClickeable(inputPattern, gameCreator);

        //// SON LAS DIFERENTES MANERAS EN QUE SE ADMINISTRA ESE ICLICKEABLE QUE NOS ENVIA EL SELECTINPUTICLICKEABLE
        //selectionEmptyTile = new SelectionEmptyTile(selectInput, gameCreator);
        //selectionPlayer = new SelectionPlayer(selectInput, gameCreator);
        //selectionUnit = new SelectionUnit(selectInput, gameCreator);
        //highLightUnitMenu = new HighLightUnitMenu(selectInput, gameCreator, selectionUnit, selectionEmptyTile);
    }

    public void CreateButtonSelectionControl()
    {
        //EndTurnButtonEventFire EndTurnButtonEvent = new EndTurnButtonEventFire();
        //SpecificEndTurnButtonExecution EndTurnButtonExecution = new SpecificEndTurnButtonExecution(this);

        //ButtonPartners cardButtonPartners = new ButtonPartners(EndTurnButtonEvent, EndTurnButtonExecution);

        //List<ButtonPartners> buttonPartners = new List<ButtonPartners>();
        //buttonPartners.Add(cardButtonPartners);

        //uiInputActions = new UIInputActionPattern(buttonPartners);
    }

    public override void GetBack()
    {
        //base.GetBack();
        ////Debug.Log("REENTER Turn State Player " + gameCreator.actualPlayerTurn.ID);
        //selectInput.Suscribe();
        //uiInputActions.Suscribe();
    }

    public override State Update()
    {
        //if (endState)
        //{
        //    Debug.Log("Turn State ENDS");
        //    gameCreator.ChangeTurn();
        //    return new AdministrationState(15, gameCreator, 1);
        //}
        //if (RestTime())
        //{
        //    //Debug.Log("Turn State InGame");
        //    return null;
        //}
        //else
        //{
        //    //Debug.Log("Turn State ENDS");
        //    gameCreator.ChangeTurn();
        //    return new AdministrationState(15, gameCreator, 1);
        //}
        return null;
    }
}