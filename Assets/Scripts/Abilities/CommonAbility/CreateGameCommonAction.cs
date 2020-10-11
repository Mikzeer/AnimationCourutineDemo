using System;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class CreateGameCommonAction : CommonAbility
    {
        MotionController motionControllerCreateBoard = new MotionController();
        Vector3 gridStartPosition = new Vector3(-10, -10, 0);
        float cellSize = 4f;
        int withd = 7;
        int height = 5;

        event Action eventAction;

        public CreateGameCommonAction(Action eventAction)
        {
            this.eventAction = eventAction;
        }

        public override void Execute()
        {
            CreatePlayers();
            CreateDeck();
            CreateNewBoard();

            actionStatus = ABILITYEXECUTIONSTATUS.STARTED;
        }

        private void CreateDeck()
        {
            GameCreator.Instance.cardManager.CreateDeck(GameCreator.Instance.players[0], GameCreator.Instance.playerOneCards);
            GameCreator.Instance.cardManager.CreateDeck(GameCreator.Instance.players[1], GameCreator.Instance.playerTwoCards);
        }

        private void CreatePlayers()
        {
            GameCreator.Instance.players = new Player[2];

            Stack<Card> deckPlayerOne = new Stack<Card>();
            Stack<Card> deckPlayerTwo = new Stack<Card>();

            GameCreator.Instance.players[0] = new Player(0, PLAYERTYPE.PLAYER, deckPlayerOne);
            GameCreator.Instance.players[1] = new Player(1, PLAYERTYPE.PLAYER, deckPlayerTwo);

            GameCreator.Instance.turnManager = new TurnManager(GameCreator.Instance.players);

            //GameCreator.Instance.SetPlayerTurn(GameCreator.Instance.players[0]);
        }

        private void CreateNewBoard()
        {
            GameCreator.Instance.board2D = new Board2D(height, withd, GameCreator.Instance.players, gridStartPosition);
            GameObject[,] tiles = new GameObject[withd + 4, height];
            withd += 4;
            int index = 1;
            GameObject tileParent = new GameObject("TileParent");

            List<Motion> motionsCreateBoard = new List<Motion>();

            for (int x = 0; x < withd; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == 1)
                    {
                        tiles[x, y] = GameCreator.Instance.board2D.GridArray[x, y].GetTransform().gameObject;
                        tiles[x, y].transform.SetParent(tileParent.transform);
                        continue;
                    }
                    if (x == 9 || x == 10)
                    {
                        tiles[x, y] = GameCreator.Instance.board2D.GridArray[x, y].GetTransform().gameObject;
                        tiles[x, y].transform.SetParent(tileParent.transform);
                        continue;
                    }

                    Vector3 thisTileFinalPosition = GameCreator.Instance.board2D.GetGridObject(x, y).GetRealWorldLocation();

                    tiles[x, y] = GameCreator.Instance.board2D.GridArray[x, y].GetTransform().gameObject;
                    tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                    tiles[x, y].transform.SetParent(tileParent.transform);

                    // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                    Motion motionTweenMove = new MoveTweenMotion(GameCreator.Instance, tiles[x, y].transform, index, thisTileFinalPosition, 1);
                    motionsCreateBoard.Add(motionTweenMove);
                }
                index++;
            }

            // para las spawn tiles
            Vector2 yOffset = new Vector2(0, 10);

            Vector3 pOneNexusFinalPosition = GameCreator.Instance.board2D.GetPlayerNexusWorldPosition(GameCreator.Instance.players[0]);
            tiles[0, 0].transform.position = new Vector3(pOneNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
            Motion motionTweenNexusP1Move = new MoveTweenMotion(GameCreator.Instance, tiles[0, 0].transform, index, pOneNexusFinalPosition, 1);
            motionsCreateBoard.Add(motionTweenNexusP1Move);

            Vector3 pTwoNexusFinalPosition = GameCreator.Instance.board2D.GetPlayerNexusWorldPosition(GameCreator.Instance.players[1]);
            tiles[9, 0].transform.position = new Vector3(pTwoNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
            Motion motionTweenNexusP2Move = new MoveTweenMotion(GameCreator.Instance, tiles[9, 0].transform, index, pTwoNexusFinalPosition, 1);
            motionsCreateBoard.Add(motionTweenNexusP2Move);

            index++;

            List<Configurable> configurables = new List<Configurable>();
            EventInvokerGenericContainer evenToInvoke = new EventInvokerGenericContainer(eventAction);
            InvokeEventConfigureAnimotion<EventInvokerGenericContainer, Transform> stopSoundConfigureAnimotion = new InvokeEventConfigureAnimotion<EventInvokerGenericContainer, Transform>(evenToInvoke, index);
            configurables.Add(stopSoundConfigureAnimotion);               

            CombineMotion combinMoveMotion = new CombineMotion(GameCreator.Instance, 1, motionsCreateBoard, configurables);

            motionControllerCreateBoard.SetUpMotion(combinMoveMotion);
            motionControllerCreateBoard.TryReproduceMotion();
        }

        public override void OnEndExecute()
        {

        }

        public override void OnStartExecute()
        {

        }

        public override bool OnTryExecute()
        {
            return true;
        }

    }

}
