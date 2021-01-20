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
    }

    public class GameMachine : MonoBehaviour, IGame
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
        [Header("TOGGLE CONTROLLER")]
        [SerializeField] protected ToggleController toggleController = default;
        protected MouseController mouseController;
        protected KeyBoardController keyBoardController;
        [Header("TILE SELECTION MANAGER UI")]
        [SerializeField] protected TileSelectionManagerUI tileSelectionManagerUI = default;
        [Header("CARD MANAGER UI")]
        [SerializeField] protected CardManagerUI cardManagerUI = default;

        public CombineManager combineManager { get; protected set; }
        public TurnController turnController { get; protected set; }
        public PlayerManager playerManager { get; protected set; }
        public CardController cardManager { get; protected set; }

        protected bool isCardCollectionLoaded = false;
        protected bool isBoardLoaded = false;
        #endregion


        public State currentState;

        public void Initialize(State states)
        {
            currentState = states;
            currentState.Enter();
        }

        public void Update()
        {
            if (currentState != null)
            {
                State nextState = currentState.Update();
                ChangeState(nextState);
            }
            else
            {
                //Debug.Log("No hay State para Ejecutar");
            }

        }

        public void ChangeState(State nextState)
        {
            if (nextState != null)
            {
                currentState.Exit();
                nextState.Enter();
                currentState = nextState;
            }
        }

        public void GetBack(State previousState)
        {
            if (previousState != null)
            {
                currentState.Exit();
                previousState.GetBack();
                currentState = previousState;
            }
        }
    }
}