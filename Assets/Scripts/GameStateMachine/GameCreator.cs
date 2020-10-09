﻿using UnityEngine;
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

    public SpawnController spawnCotroller = new SpawnController();
    public TurnManager turnManager { get; set; }
    public CardManager cardManager = new CardManager();

    public Player[] players;
    public List<CardScriptableObject> playerOneCards;
    public List<CardScriptableObject> playerTwoCards;
    public GameObject Crane;
    public Transform CraneEnd;
    public GameObject kimbokoPrefab;
    public Board2D board2D;
    int rows = 5;//5
    int columns = 7;//7
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    public Camera cam;

    public GameObject cardUIPrefab;
    public RectTransform canvasRootTransform;
    public InfoPanel infoPanel;
    public RectTransform cardHolderP1;
    public RectTransform cardHolderP2;

    public static event Action OnTimeStart;
    public static event Action<bool> OnTakeCardAvailable;

    #endregion

    #region UNITY METHODS

    private void Awake()
    {
        cam = Camera.main;

        UIController.OnTakeCardActionClicked += AddCard;

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
        CrearEstadoInicial();
    }

    #endregion

    #region METHODS

    public void CrearEstadoInicial()
    {
        CreationState creationState = new CreationState();
        Initialize(creationState);
    }

    public void AddCard()
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

    #endregion

}