using System.Collections.Generic;
using UnityEngine;
using PositionerDemo;
using System;

public class GameManager : MonoBehaviour
{
    #region EVENTS

    public static event Action OnChangeTurn;

    #endregion

    #region STATIC LAZY SINGLETON

    [SerializeField]
    protected bool dontDestroy;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as GameManager;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    #endregion

    #region VARIABLES

    MotionController motionControllerCreateBoard = new MotionController();
    public Vector3 gridStartPosition = new Vector3(-10, -10, 0);
    public float cellSize = 4f;
    public int withd = 5;
    public int height = 7;
    Board2D board;
    public GameObject tilePrefab;

    public AudioSource audioSource;
    Tile actualTileObject;
    Player[] players;
    Player actualPlayerTurn;

    #endregion

    #region BOARD

    private void CreateNewBoard()
    {
        board = new Board2D(height, withd, players, gridStartPosition);
        GameObject[,] tiles = new GameObject[withd + 4, height];
       
        withd += 4;
        int index = 1;
        GameObject tileParent = new GameObject("TileParent");

        List<PositionerDemo.Motion> motionsCreateBoard = new List<PositionerDemo.Motion>();

        for (int x = 0; x < withd; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == 1)
                {
                    tiles[x, y] = board.GridArray[x, y].GetTransform().gameObject;
                    tiles[x, y].transform.SetParent(tileParent.transform);
                    continue;
                }
                if (x == 9 || x == 10)
                {
                    tiles[x, y] = board.GridArray[x, y].GetTransform().gameObject;
                    tiles[x, y].transform.SetParent(tileParent.transform);
                    continue;
                }

                Vector3 thisTileFinalPosition = board.GetGridObject(x, y).GetRealWorldLocation();

                tiles[x, y] = board.GridArray[x, y].GetTransform().gameObject;
                tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                tiles[x, y].transform.SetParent(tileParent.transform);

                // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                PositionerDemo.Motion motionTweenMove = new MoveTweenMotion(this, tiles[x, y].transform, index, thisTileFinalPosition, 1);
                motionsCreateBoard.Add(motionTweenMove);
            }
            index++;
        }

        // para las spawn tiles
        Vector2 yOffset = new Vector2(0, 10);

        Vector3 pOneNexusFinalPosition = board.GetPlayerNexusWorldPosition(players[0]);
        tiles[0, 0].transform.position = new Vector3(pOneNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
        PositionerDemo.Motion motionTweenNexusP1Move = new MoveTweenMotion(this, tiles[0, 0].transform, index, pOneNexusFinalPosition, 1);
        motionsCreateBoard.Add(motionTweenNexusP1Move);

        Vector3 pTwoNexusFinalPosition = board.GetPlayerNexusWorldPosition(players[1]);
        tiles[9, 0].transform.position = new Vector3(pTwoNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
        PositionerDemo.Motion motionTweenNexusP2Move = new MoveTweenMotion(this, tiles[9, 0].transform, index, pTwoNexusFinalPosition, 1);
        motionsCreateBoard.Add(motionTweenNexusP2Move);

        CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCreateBoard);

        motionControllerCreateBoard.SetUpMotion(combinMoveMotion);
        motionControllerCreateBoard.TryReproduceMotion();
    }

    #endregion

    #region PLAYER

    public void CreatePlayers()
    {
        players = new Player[2];

        Stack<Card> deckPlayerOne = new Stack<Card>();
        Stack<Card> deckPlayerTwo = new Stack<Card>();

        players[0] = new Player(0, PLAYERTYPE.PLAYER, deckPlayerOne);
        players[1] = new Player(1, PLAYERTYPE.PLAYER, deckPlayerTwo);

        SetPlayerTurn(players[0]);
    }

    public void SetPlayerTurn(Player player)
    {
        actualPlayerTurn = player;
    }

    public Player GetPlayer()
    {
        return actualPlayerTurn;
    }

    public void ChangeTurn()
    {
        if (actualPlayerTurn == players[0])
        {
            actualPlayerTurn = players[1];
        }
        else
        {
            actualPlayerTurn = players[0];
        }

        OnChangeTurn?.Invoke();
    }

    #endregion

}

