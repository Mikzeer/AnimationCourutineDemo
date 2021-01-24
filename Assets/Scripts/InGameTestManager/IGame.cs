using MikzeerGame.UI;
using StateMachinePattern;
using UnityEngine;

namespace PositionerDemo
{
    public interface IGame
    {
        SpawnManager spawnManager { get; }
        Board2DManager board2DManager { get; }
        CombineManager combineManager { get; }
        MovementManager movementManager { get; }
        TurnController turnController { get; }
        PlayerManager playerManager { get; }
        CardController cardManager { get; }
        BaseStateMachine baseStateMachine { get; }
        ActionsManager actionsManager { get; }
    }

    public class GameMachine : MonoBehaviour, IGame, IStateMachineHandler
    {
        #region VARIABLES
        [Header("SPAWN MANAGER UI")]
        [SerializeField] protected SpawnManagerUI spawnManagerUI = default; // DEFAULT => EVITAMOS WARNING EN EL INSPECTOR
        public SpawnManager spawnManager { get; protected set; }
        [Header("MOVEMENT MANAGER UI")]
        [SerializeField] protected MoveManagerUI moveManagerUI = default;
        public MovementManager movementManager { get; protected set; }
        [Header("BOARD MANAGER UI")]
        [SerializeField] protected Board2DManagerUI board2DManagerUI = default;
        public Board2DManager board2DManager { get; protected set; }
        protected MouseController mouseController;
        protected KeyBoardController keyBoardController;
        [Header("TILE SELECTION MANAGER UI")]
        public TileSelectionManagerUI tileSelectionManagerUI = default;
        [Header("CARD MANAGER UI")]
        [SerializeField] protected CardManagerUI cardManagerUI = default;
        [Header("ABILITY SELECTION MANAGER UI")]
        public AbilitySelectionManagerUI abilitySelectionManagerUI = default;
        [Header("ABILITY BUTTON CREATION UI")]
        public AbilityButtonCreationUI abilityButtonCreationUI = default;
        [Header("GENERAL MANAGER UI")]
        public UIGeneralManagerInGame uiGeneralManagerInGame = default;
        [Header("INFORMATION MANAGER UI")]
        public InformationUIManager informationUIManager = default;
        public CombineManager combineManager { get; protected set; }
        public TurnController turnController { get; protected set; }
        public PlayerManager playerManager { get; protected set; }
        public CardController cardManager { get; protected set; }
        public BaseStateMachine baseStateMachine { get; protected set; }
        public ActionsManager actionsManager { get; protected set; }
        protected bool isCardCollectionLoaded = false;
        protected bool isBoardLoaded = false;

        #endregion

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Update()
        {
            if (baseStateMachine == null || baseStateMachine.IsInitialized == false) return;
            baseStateMachine.Update();
        }
    }
}