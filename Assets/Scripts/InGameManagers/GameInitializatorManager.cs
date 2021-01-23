using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class GameInitializatorManager : MonoBehaviour
    {
        InGameCardCollectionManager inGameCardCollectionManager;
        TurnManager turnManager;
        CardManager cardManager;
        bool isCardCollectionLoaded = false;
        bool isBoardLoaded = false;
        MotionController motionControllerCreateBoard = new MotionController();
        Vector3 gridStartPosition = new Vector3(-10, -10, 0);
        int withd = 7;
        int height = 5;


        //  ESTO LO DEBERIA PASAR A UN STATE
        //  DEBERIA SER EL INITIALIZATION STATE
        //  VOLVEMOS A LO DE ANTES XD XD XD XD

        private void Start()
        {
            StartCoroutine(InitializeGame());
        }

        private IEnumerator InitializeGame()
        {
            // DATOS FIEBASE RT HELPER
            // INGAMECARDCOLLECTION
            // CARD MANAGER / CARD MANAGER UI
            // BOARD MANAGER // BOARD MANAGER UI

            // ACA DEBERIA RECIBIR LA CONFIGURATION DATA DE CADA UNO
            // UNA VEZ QUE MIRROR ME DIGA "OK HERMANO", LOS DOS JUGADORES MANDARON SU CONFIGURATION DATA
            // ENTONCES AHI PUEDO VENIR A INITIALIZE GAME Y COMPROBAR LA PARTE DE FIREBASE Y SUS CONEXIONES
            // 0- CHEQUEAR QUE LOS DOS JUGADORES ESTEN EN LINEA Y LISTOS PARA JUGAR.
            // 0b- CHEQUEAR QUE LOS DOS JUGADORES EXISTAN
            // 0c- SEGUN EL JUGADOR QUE HAYA CREADO LA PARTIDA ESE SERA EL PLAYER ONE


            // 0d- CHEQUEAR QUE LA BASE DE DATOS ESTE INITIALIZED
            //yield return WaitForDatabaseToLoad(); // REACTIVAR CUANDO FUNCIONE BIEN LA DB
        


            // 1- CARGAR LA CONFIGURATION DATA DE CADA PLAYER
            HelperCardCollectionJsonKimboko helperCardCollectionJsonKimboko = new HelperCardCollectionJsonKimboko();
            ConfigurationData cnfDat = helperCardCollectionJsonKimboko.GetConfigurationDataFromJson();

            // 1b- CREAR A LOS DOS PLAYERS SEGUN LA CONFIGURATION DATA
            Player playerOne = new Player(0);
            playerOne.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerOne), OccupierStatDatabase.CreatePlayerStat());

            Player playerTwo = new Player(1);
            playerTwo.SetStatsAndAbilities(OccupierAbilityDatabase.CreatePlayerAbilities(playerTwo), OccupierStatDatabase.CreatePlayerStat());

            Player[] players = new Player[2];
            players[0] = playerOne;
            players[1] = playerTwo;

            UserDB userOne = cnfDat.user;
            UserDB userTestJson = new UserDB("ppp");
            userTestJson.ID = "ppp";
            UserDB userTwo = cnfDat.user;
            UserDB[] users = new UserDB[2];
            users[0] = userOne;
            //users[1] = userTwo;
            users[1] = userTestJson;
 
            
            
            // 2- ASIGNAR LOS PLAYER AL TURN MANAGER
            turnManager = new TurnManager(players);

            // 2b- IR CREANDO EL BOARD
            Board2D board = CreateNewBoard(players);







            // 3- CARGAR LA GAME COLLECTION
            inGameCardCollectionManager = new InGameCardCollectionManager(this, OnCardCollectionLoadComplete);
            //inGameCardCollectionManager.LoadAllCollection(users);// REACTIVAR CUANDO FUNCIONE BIEN LA DB
            inGameCardCollectionManager.LoadAllCollectionJson(users);
            while (isCardCollectionLoaded == false)
            {
                Debug.Log("WAITING FOR CARD COLLECTION TO LOAD");
                yield return null;
            }

            Debug.Log("GAME CARD COLLECTION LOADED");

            // 4- CARGAR EL DECK SELECCIONADO DE CADA JUGADOR
            // 4b- CHEQUEAR QUE SEA UN DECK VALIDO

            // 4c- SI ES INVALIDO SACAMOS TODO A LA MIERDA, SI ES VALID CREAMOS LOS DECKS DE CADA PLAYER
            cardManager = new CardManager(inGameCardCollectionManager);

            cardManager.LoadDeckFromConfigurationData(playerOne, cnfDat);
            cardManager.LoadDeckFromConfigurationData(playerTwo, cnfDat);



            //  ESTA ASIGNACION ES SOLAMENTE MOMENTANEA HASTA QUE VEA A DONDE VA A PARAR CADA INFORMACION O DE DONDE VIENE...
            //GameCreator.Instance.players = players;
            //GameCreator.Instance.turnManager = turnManager;
            ////GameCreator.Instance.cardManager = cardManager;
            //GameCreator.Instance.board2D = board;

            while (isBoardLoaded == false)
            {
                Debug.Log("WAITING FOR BOARD TO LOAD");
                yield return null;
            }

            Debug.Log("BOARD LOADED");

            //State nextState = new InitialAdministrationState(40, GameCreator.Instance, 4, 0);
            //GameCreator.Instance.Initialize(GameCreator.Instance.turnManager.ChangeTurnState(4, nextState));
        }

        private IEnumerator WaitForDatabaseToLoad()
        {
            while (DatosFirebaseRTHelper.Instance.isInit == false)
            {
                Debug.Log("WAITING FOR DATABASE TO LOAD");
                yield return null;
            }

            Debug.Log("DB LOADED");
        }

        private void OnCardCollectionLoadComplete()
        {
            isCardCollectionLoaded = true;
        }

        private void OnBoardLoadComplete()
        {
            isBoardLoaded = true;
        }

        private Board2D CreateNewBoard(Player[] players)
        {
            Board2D board2D = new Board2D(height, withd, players, gridStartPosition);
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
                        tiles[x, y] = board2D.GridArray[x, y].goAnimContainer.GetTransform().gameObject;
                        tiles[x, y].transform.SetParent(tileParent.transform);
                        continue;
                    }
                    if (x == 9 || x == 10)
                    {
                        tiles[x, y] = board2D.GridArray[x, y].goAnimContainer.GetTransform().gameObject;
                        tiles[x, y].transform.SetParent(tileParent.transform);
                        continue;
                    }

                    Vector3 thisTileFinalPosition = board2D.GetGridObject(x, y).GetRealWorldLocation();

                    tiles[x, y] = board2D.GridArray[x, y].goAnimContainer.GetTransform().gameObject;
                    tiles[x, y].transform.position = new Vector3(thisTileFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y, 0);
                    tiles[x, y].transform.SetParent(tileParent.transform);

                    // TWEEN DE LA CRANE A LA POSICION DE SPAWNEO
                    Motion motionTweenMove = new MoveTweenMotion(this, tiles[x, y].transform, index, thisTileFinalPosition, 1);
                    motionsCreateBoard.Add(motionTweenMove);
                }
                index++;
            }

            // para las spawn tiles
            Vector2 yOffset = new Vector2(0, 10);

            Vector3 pOneNexusFinalPosition = board2D.GetPlayerNexusWorldPosition(players[0]);
            tiles[0, 0].transform.position = new Vector3(pOneNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
            Motion motionTweenNexusP1Move = new MoveTweenMotion(this, tiles[0, 0].transform, index, pOneNexusFinalPosition, 1);
            motionsCreateBoard.Add(motionTweenNexusP1Move);

            Vector3 pTwoNexusFinalPosition = board2D.GetPlayerNexusWorldPosition(players[1]);
            tiles[9, 0].transform.position = new Vector3(pTwoNexusFinalPosition.x, Helper.GetCameraTopBorderYWorldPostion().y + yOffset.y, 0);
            Motion motionTweenNexusP2Move = new MoveTweenMotion(this, tiles[9, 0].transform, index, pTwoNexusFinalPosition, 1);
            motionsCreateBoard.Add(motionTweenNexusP2Move);

            index++;

            List<Configurable> configurables = new List<Configurable>();
            EventInvokerGenericContainer evenToInvoke = new EventInvokerGenericContainer(OnBoardLoadComplete);
            InvokeEventConfigureAnimotion<EventInvokerGenericContainer, Transform> stopSoundConfigureAnimotion = new InvokeEventConfigureAnimotion<EventInvokerGenericContainer, Transform>(evenToInvoke, index);
            configurables.Add(stopSoundConfigureAnimotion);

            CombineMotion combinMoveMotion = new CombineMotion(this, 1, motionsCreateBoard, configurables);

            motionControllerCreateBoard.SetUpMotion(combinMoveMotion);
            motionControllerCreateBoard.TryReproduceMotion();

            return board2D;
        }
    }
}
