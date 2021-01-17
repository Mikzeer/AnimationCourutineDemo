using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    #region VARIABLES

    [SerializeField] private TimePanelUI timePanelUI = default;

    [SerializeField] private Button btnTakeCard = default;
    public delegate void ClickTakeCardAction();
    public static event ClickTakeCardAction OnTakeCardActionClicked = default;

    [SerializeField] private Button btnEndTurn = default;
    public delegate void ClickEndTurnAction();
    public static event ClickEndTurnAction OnEndTurnActionClicked = default;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        btnTakeCard.onClick.AddListener(OnBtnTakeCardClick);
        btnEndTurn.onClick.AddListener(OnBtnEndTurnClick);
        btnEndTurn.gameObject.SetActive(false);
        btnTakeCard.gameObject.SetActive(false);
        GameCreator.OnTimeStart += SetTimer;
        GameCreator.OnTakeCardAvailable += SetCardButtonVisibility;
        GameCreator.OnEndTurnAvailable += SetEndTurnVisibility;
    }

    private void OnDestroy()
    {
        btnTakeCard.onClick.RemoveListener(OnBtnTakeCardClick);
        btnEndTurn.onClick.RemoveListener(OnBtnEndTurnClick);
    }

    #endregion

    public void SetTimer()
    {
        GameTimer.OnTimeChange += UpdateTime;
    }

    private void UpdateTime(string obj)
    {
        timePanelUI.SetTime(obj);
    }

    private void SetCardButtonVisibility(bool obj)
    {
        btnTakeCard.gameObject.SetActive(obj);
    }

    private void SetEndTurnVisibility(bool obj)
    {
        btnEndTurn.gameObject.SetActive(obj);
    }

    #region BUTTON EVENT REGION

    void OnBtnTakeCardClick()
    {
        OnTakeCardActionClicked?.Invoke();
    }

    void OnBtnEndTurnClick()
    {
        OnEndTurnActionClicked?.Invoke();
    }

    #endregion

}

namespace MikzeerGame
{
    namespace UI
    {
        //public class HighlightPanelUI : MonoBehaviour
        //{
        //    public Transform gridButtonContainer;
        //    public Button abilityBtn;
        //    List<AbilityButton> actualAbilityButtons;
        //    private GameCreator gameCreator;

        //    HighLightUnitMenu highLightUnitMenu;

        //    private void Awake()
        //    {
        //        gameCreator = FindObjectOfType<GameCreator>();
        //    }

        //    public void SetUnit(Unit.Unit2D unitToSet, HighLightUnitMenu highLightUnitMenu)
        //    {
        //        this.highLightUnitMenu = highLightUnitMenu;
        //        if (actualAbilityButtons != null && actualAbilityButtons.Count > 0)
        //        {
        //            for (int i = 0; i < actualAbilityButtons.Count; i++)
        //            {
        //                actualAbilityButtons[i].Unsuscribe();
        //                Destroy(actualAbilityButtons[i].btnAbility.gameObject);
        //            }
        //            actualAbilityButtons.Clear();
        //        }

        //        if (unitToSet == null)
        //        {
        //            //Debug.Log("NULL UNIT HIGHLIGHT ");
        //            gameObject.SetActive(false);
        //        }
        //        else
        //        {
        //            gameObject.SetActive(true);
        //            StartCreationOfButtons(unitToSet);
        //            //Debug.Log("Estas en tu turno Unidad!!! " + unitToSet._ID);
        //        }

        //    }

        //    public void StartCreationOfButtons(Unit.Unit2D unitToSet)
        //    {
        //        actualAbilityButtons = new List<AbilityButton>();
        //        //for (int i = 0; i < unitToSet.AbilityActions.Count; i++)
        //        //{

        //        //}
        //        CreateButton(unitToSet, ABILITYBUTTONTYPE.MOVE);
        //        CreateButton(unitToSet, ABILITYBUTTONTYPE.ATTACK);
        //        CreateButton(unitToSet, ABILITYBUTTONTYPE.DEFEND);

        //        for (int i = 0; i < actualAbilityButtons.Count; i++)
        //        {
        //            actualAbilityButtons[i].Suscribe();
        //        }

        //    }

        //    public void CreateButton(Unit.Unit2D unitToSet, ABILITYBUTTONTYPE abilityButtonType)
        //    {
        //        Button btnAbi = Instantiate(abilityBtn);
        //        btnAbi.transform.SetParent(gridButtonContainer, false);
        //        switch (abilityButtonType)
        //        {
        //            case ABILITYBUTTONTYPE.MOVE:
        //                MoveAbilityButtonExecution moveAbilityBtnExe = new MoveAbilityButtonExecution(unitToSet, gameCreator, this);
        //                AbilityButton btnMove = new AbilityButton(btnAbi, moveAbilityBtnExe, null);
        //                actualAbilityButtons.Add(btnMove);
        //                break;
        //            case ABILITYBUTTONTYPE.ATTACK:
        //                AttackAbilityButtonExecution attackAbilityBtnExe = new AttackAbilityButtonExecution(unitToSet, gameCreator, this);
        //                AbilityButton btnAttack = new AbilityButton(btnAbi, attackAbilityBtnExe, null);
        //                actualAbilityButtons.Add(btnAttack);
        //                break;
        //            case ABILITYBUTTONTYPE.DEFEND:
        //                DefenseAbilityButtonExecution defenseAbilityBtnExe = new DefenseAbilityButtonExecution(unitToSet, gameCreator, this);
        //                AbilityButton btnDefense = new AbilityButton(btnAbi, defenseAbilityBtnExe, null);
        //                actualAbilityButtons.Add(btnDefense);
        //                break;

        //            case ABILITYBUTTONTYPE.COMBINE:
        //                break;
        //            case ABILITYBUTTONTYPE.DECOMBINE:
        //                break;
        //            case ABILITYBUTTONTYPE.EVOLVE:
        //                break;
        //            case ABILITYBUTTONTYPE.FUSION:
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    public void RestartPanel()
        //    {
        //        gameObject.SetActive(false);
        //        highLightUnitMenu.selectionUnit.Execute(null);
        //        highLightUnitMenu.selectionEmptyTile.Execute(null);
        //        highLightUnitMenu.Execute(null);
        //    }
        //}

        public enum ABILITYBUTTONTYPE
        {
            MOVE,
            ATTACK,
            DEFEND,
            COMBINE,
            DECOMBINE,
            EVOLVE,
            FUSION
        }

        public class AbilityButton
        {
            Sprite btnSprite;
            public Button btnAbility;
            SpecificAbilityExecution abilityExecution;

            public AbilityButton(Button btnAbility, SpecificAbilityExecution abilityExecution, Sprite btnSprite)
            {
                this.btnSprite = btnSprite;
                this.btnAbility = btnAbility;
                btnAbility.GetComponentInChildren<Text>().text = abilityExecution.name;
                this.abilityExecution = abilityExecution;
            }

            public void Suscribe()
            {
                btnAbility.onClick.AddListener(Execute);
            }

            public void Unsuscribe()
            {
                btnAbility.onClick.RemoveAllListeners();
            }

            public void Execute()
            {
                abilityExecution.Execute();
            }

        }

        public interface SpecificAbilityExecution
        {
            // LO QUE PASA AL EJECUTAR ESE EVENTO ESPECIFICO DE BOTON
            void Execute();
            string name { get; }
        }

        //public class MoveAbilityButtonExecution : SpecificAbilityExecution
        //{
        //    Unit.Unit2D actualUnit;
        //    private const string nm = "MOVE";
        //    public string name { get; private set; }
        //    GameCreator gameCreator;
        //    HighlightPanelUI highlightPanel;
        //    public MoveAbilityButtonExecution(Unit.Unit2D actualUnit, GameCreator gameCreator, HighlightPanelUI highlightPanel)
        //    {
        //        this.gameCreator = gameCreator;
        //        name = nm;
        //        this.actualUnit = actualUnit;
        //        this.highlightPanel = highlightPanel;

        //    }

        //    public void Execute()
        //    {
        //        if (actualUnit == null)
        //        {
        //            //Debug.Log("Unit NULL");
        //        }
        //        else
        //        {
        //            //Debug.Log("Unit " + actualUnit._ID + " MOVE");

        //            actualUnit.moveAbility.Set(gameCreator);

        //            if (actualUnit.moveAbility.OnTryExecute())
        //            {
        //                gameCreator.ChangeState(new GameStateMachine.MoveState(actualUnit, gameCreator, gameCreator.currentState));
        //            }
        //            else
        //            {
        //                highlightPanel.RestartPanel();
        //            }
        //        }
        //    }
        //}

        //public class AttackAbilityButtonExecution : SpecificAbilityExecution
        //{
        //    Unit.Unit2D actualUnit;
        //    private const string nm = "ATTACK";
        //    public string name { get; private set; }
        //    GameCreator gameCreator;
        //    HighlightPanelUI highlightPanel;

        //    public AttackAbilityButtonExecution(Unit.Unit2D actualUnit, GameCreator gameCreator, HighlightPanelUI highlightPanel)
        //    {
        //        name = nm;
        //        this.actualUnit = actualUnit;

        //        this.gameCreator = gameCreator;
        //        this.highlightPanel = highlightPanel;
        //    }

        //    public void Execute()
        //    {
        //        if (actualUnit == null)
        //        {
        //            //Debug.Log("Unit NULL");
        //        }
        //        else
        //        {
        //            //Debug.Log("Unit " + actualUnit._ID + " ATTACK");
        //            actualUnit.attackAbility.Set(gameCreator);

        //            // CUANDO TENGAMOS UN PLAYER Y EN NUESTRO TURNO LE HAGAMOS CLICK, NOS VA A APARECER AUTOMATICAMENTE PARA ATACAR A UN JUGADOR EN BASE
        //            // CUANDO TENGAMOS UNA BARRICADA QUE ATAQUE, Y LA SELECCIONEMOS, AUTOMATICAMENTE NOS VA A APARECER PARA ATACAR A LOS JUGADORES
        //            // CON LA UNIT NO, NOS APARECE UN BOTON
        //            if (actualUnit.attackAbility.OnTryExecute())
        //            {
        //                gameCreator.ChangeState(new GameStateMachine.AttackState(actualUnit, gameCreator, gameCreator.currentState));
        //            }
        //            else
        //            {
        //                highlightPanel.RestartPanel();
        //            }

        //        }
        //    }
        //}

        //public class DefenseAbilityButtonExecution : SpecificAbilityExecution
        //{
        //    Unit.Unit2D actualUnit;
        //    private const string nm = "DEFENSE";
        //    public string name { get; private set; }
        //    GameCreator gameCreator;
        //    HighlightPanelUI highlightPanel;

        //    public DefenseAbilityButtonExecution(Unit.Unit2D actualUnit, GameCreator gameCreator, HighlightPanelUI highlightPanel)
        //    {
        //        name = nm;
        //        this.actualUnit = actualUnit;

        //        this.gameCreator = gameCreator;
        //        this.highlightPanel = highlightPanel;
        //    }

        //    public void Execute()
        //    {
        //        if (actualUnit == null)
        //        {
        //            //Debug.Log("Unit NULL");
        //        }
        //        else
        //        {
        //            if (actualUnit.defendAbility.OnTryExecute())
        //            {
        //                //gameCreator.ChangeState(new GameStateMachine.AttackState(actualUnit, gameCreator, gameCreator.currentState));
        //                actualUnit.defendAbility.Perform();
        //            }
        //            //else
        //            //{
        //            //    highlightPanel.RestartPanel();
        //            //}

        //            highlightPanel.RestartPanel();
        //        }
        //    }
        //}

    }
}