using CommandPatternActions;
using System.Collections;
using UnityEngine;

namespace PositionerDemo
{
    public class GameTestController : GameMachine, IStateMachineHandler
    {
        private void Start()
        {
            StartCoroutine(InitializeGame());
        }
   
        private void OnBoardComplete()
        {
            Debug.Log("BOARD SE TERMINO DE CREAR");
            isBoardLoaded = true;
        }

        public IEnumerator InitializeGame()
        {
            // ACA DEBERIA RECIBIR LA CONFIGURATION DATA DE CADA UNO
            // UNA VEZ QUE MIRROR ME DIGA "OK HERMANO", LOS DOS JUGADORES MANDARON SU CONFIGURATION DATA
            // ENTONCES AHI PUEDO VENIR A INITIALIZE GAME Y COMPROBAR LA PARTE DE FIREBASE Y SUS CONEXIONES
            // 0- CHEQUEAR QUE LOS DOS JUGADORES ESTEN EN LINEA Y LISTOS PARA JUGAR.
            // 0b- CHEQUEAR QUE LOS DOS JUGADORES EXISTAN
            // 0c- SEGUN EL JUGADOR QUE HAYA CREADO LA PARTIDA ESE SERA EL PLAYER ONE, NO SIGNIFICA QUE VA A EMPEZAR PRIMERO

            // 0d- CHEQUEAR QUE LA BASE DE DATOS ESTE INITIALIZED
            //yield return WaitForDatabaseToLoad(); // REACTIVAR CUANDO FUNCIONE BIEN LA DB


            // 1 - CREAR PLAYERS Y USERS
            playerManager = new PlayerManager();

            /////////
            playerManager.CreatePlayers();
            playerManager.CreateNewUser(null);


            // 3- IR CREANDO EL BOARD
            board2DManager = new Board2DManager(board2DManagerUI, 5, 7);

            /////////
            Motion motion = board2DManager.CreateBoard(playerManager.GetPlayer(), OnBoardComplete);
            InvokerMotion.AddNewMotion(motion);
            InvokerMotion.StartExecution(this);


            // 2- ASIGNAR LOS PLAYER AL TURN MANAGER
            turnController = new TurnController(playerManager.GetPlayer());

            /////////
            // 2b - DECIDIR QUE PLAYER COMIENZA PRIMERO
            turnController.DecideStarterPlayer();

            // 3b - Inicializar los managers generales  ESTOS LOS VA A TENER EL GAME... ASI QUE SEGURO LO HAGAMOS DESDE AHI
            spawnManager = new SpawnManager(spawnManagerUI, this);
            combineManager = new CombineManager(this);
            movementManager = new MovementManager(this, moveManagerUI);



            //tileSelectionManagerUI.onTileSelected += ExecuteActions;








            yield return null;
            // 3- CARGAR LA GAME COLLECTION
            InGameCardCollectionManager inGameCardCollectionManager = new InGameCardCollectionManager(this, OnCardCollectionLoadComplete);
            //inGameCardCollectionManager.LoadAllCollection(users);// REACTIVAR CUANDO FUNCIONE BIEN LA DB
            inGameCardCollectionManager.LoadAllCollectionJson(playerManager.GetUsers().ToArray());
            while (isCardCollectionLoaded == false)
            {
                Debug.Log("WAITING FOR CARD COLLECTION TO LOAD");
                yield return null;
            }

            Debug.Log("GAME CARD COLLECTION LOADED");

            // 4- CARGAR EL DECK SELECCIONADO DE CADA JUGADOR
            // 4b- CHEQUEAR QUE SEA UN DECK VALIDO

            // 4c- SI ES INVALIDO SACAMOS TODO A LA MIERDA, SI ES VALID CREAMOS LOS DECKS DE CADA PLAYER
            cardManager = new CardController(inGameCardCollectionManager, cardManagerUI);

            cardManager.LoadDeckFromConfigurationData(playerManager.GetPlayer()[0], playerManager.playerConfigurationData);
            cardManager.LoadDeckFromConfigurationData(playerManager.GetPlayer()[1], playerManager.playerConfigurationData);

            while (isBoardLoaded == false)
            {
                Debug.Log("WAITING FOR BOARD TO LOAD");
                yield return null;
            }


            // 5 - INICIALIZAMOS LOS CONTROLES
            mouseController = new MouseController(0, board2DManager, Camera.main);
            keyBoardController = new KeyBoardController(0, board2DManager, Camera.main);
            tileSelectionManagerUI.SetController(board2DManager, mouseController, keyBoardController);


            // CREAMOS LOS STATES INICIALES
            Statee initialAdminStateA = new AdministrationStatee(40, this, 4);
            Statee initialAdminStateB = new AdministrationStatee(20, this, 2);
            Statee initialAdminStateC = new AdministrationStatee(20, this, 2);
            Statee initialAdminStateD = new AdministrationStatee(40, this, 4);

            baseStateMachine = new BaseStateMachine(this);

            baseStateMachine.PushState(initialAdminStateA, true);
            baseStateMachine.PushState(initialAdminStateB, true);
            baseStateMachine.PushState(initialAdminStateC, true);
            baseStateMachine.PushState(initialAdminStateD, true);
        }

        private void OnCardCollectionLoadComplete()
        {
            isCardCollectionLoaded = true;
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

        private void ExecuteActions(Tile tile)
        {
            switch (toggleController.StateType)
            {
                case ToggleController.STATETYPE.SPAWN:
                    spawnManager.OnTrySpawn(tile, turnController.CurrentPlayerTurn);
                    break;
                case ToggleController.STATETYPE.MOVE:
                    break;
                case ToggleController.STATETYPE.ATTACK:
                    break;
                default:
                    break;
            }
            Invoker.ExecuteCommands();
            InvokerMotion.StartExecution(this);
        }
    }
}