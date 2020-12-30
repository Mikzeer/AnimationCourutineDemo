using UnityEngine;
using UnityEngine.UI;
using PositionerDemo;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class GameCreator : GameStateMachine
{

    #region SINGLETON

    [SerializeField]
    protected bool dontDestroy;

    private static GameCreator instance;
    public static GameCreator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameCreator>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<GameCreator>();
                }
            }
            return instance;
        }
    }

    #endregion

    #region VARIABLES

    public TurnManager turnManager { get; set; }
    public Player[] players;
    public Camera cam;

    public SpawnController spawnCotroller = new SpawnController();
    public GameObject Crane;
    public Transform CraneEnd;
    public GameObject kimbokoPrefab;

    public AudioSource audioSource;
    public List<AudioClip> audioClips;

    public CardManager cardManager;
    public GameObject cardUIPrefab;
    public RectTransform cardHolderP1;
    public RectTransform cardHolderP2;

    public Board2D board2D;
    public GameObject tileSelectionPrefab;
    public HighLightTile highLightTile;
    public RectTransform canvasRootTransform;
    public InfoPanel infoPanel;

    public static event Action OnTimeStart;
    public static event Action<bool> OnTakeCardAvailable;
    public static event Action<bool> OnEndTurnAvailable;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        cam = Camera.main;

        UIController.OnTakeCardActionClicked += AddCard;
        UIController.OnEndTurnActionClicked += EndTurn;

        if (instance == null)
        {
            instance = this as GameCreator;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        highLightTile = new HighLightTile(tileSelectionPrefab);
    }

    #endregion

    #region METHODS

    private void AddCard()
    {
        TryTakeCard(turnManager.GetActualPlayerTurn());
    }

    public void TryTakeCard(Player player)
    {
        cardManager.OnTryTakeCard(player);
    }

    public void EndTurn()
    {
        cardManager.OnTryTakeCard(turnManager.GetActualPlayerTurn());
    }

    public void SetTimer()
    {
        OnTimeStart?.Invoke();
    }

    public void TakeCardAvailable(bool available)
    {
        OnTakeCardAvailable?.Invoke(available);
    }

    public void EndTurnAvailable(bool available)
    {
        OnEndTurnAvailable?.Invoke(available);
    }

    #endregion

}