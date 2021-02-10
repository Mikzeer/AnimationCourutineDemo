using CommandPatternActions;
using MikzeerGame.Animotion;
using StateMachinePattern;
using System.Collections;
using UnityEngine;

namespace PositionerDemo
{
    public class GameTestController : GameMachine
    {
        bool logOn = true;
        private void Start()
        {
            StartCoroutine(InitializeGame());
        }
   
        public IEnumerator InitializeGame()
        {
            // ACA DEBERIA RECIBIR LA CONFIGURATION DATA DE CADA UNO
            // UNA VEZ QUE MIRROR ME DIGA "OK HERMANO", LOS DOS JUGADORES MANDARON SU CONFIGURATION DATA
            // ENTONCES AHI PUEDO VENIR A INITIALIZE GAME Y COMPROBAR LA PARTE DE FIREBASE Y SUS CONEXIONES

            // 0d- CHEQUEAR QUE LA BASE DE DATOS ESTE INITIALIZED
            //yield return WaitForDatabaseToLoad(); // REACTIVAR CUANDO FUNCIONE BIEN LA DB
            // 0- CHEQUEAR QUE LOS DOS JUGADORES ESTEN EN LINEA Y LISTOS PARA JUGAR.
            // 0b- CHEQUEAR QUE LOS DOS JUGADORES EXISTAN

            // 4- CARGAR EL DECK SELECCIONADO DE CADA JUGADOR
            // 4b- CHEQUEAR QUE SEA UN DECK VALIDO
            // 4c- SI ES INVALIDO SACAMOS TODO A LA MIERDA, SI ES VALID CREAMOS LOS DECKS DE CADA PLAYER


            // CREATE PLAYERS STATE
            // 1 - CREAR PLAYERS Y USERS
            // 0c- SEGUN EL JUGADOR QUE HAYA CREADO LA PARTIDA ESE SERA EL PLAYER ONE, NO SIGNIFICA QUE VA A EMPEZAR PRIMERO
            playerManager = new PlayerManager();
            playerManager.CreatePlayers();
            playerManager.CreateNewUser(null);


            // CREATE BOARD STATE
            // 3- IR CREANDO EL BOARD
            board2DManager = new Board2DManager(board2DManagerUI, 5, 7);
            //Motion motion = board2DManager.CreateBoard(playerManager.GetPlayer(), OnBoardComplete);
            //InvokerMotion.AddNewMotion(motion);
            //InvokerMotion.StartExecution(this);

            Animotion motion = board2DManager.CreateBoardAnimotion(playerManager.GetPlayer(), OnBoardComplete);
            InvokerAnimotion.AddNewMotion(motion);
            InvokerAnimotion.StartExecution(this);



            // CREATE TURNS STATE => ESTO LO DEBERIA HACER EL SERVER PARA QUE NO LO HAGA CADA JUGADOR Y SE PUEDE HACKEAR
            // 2- ASIGNAR LOS PLAYER AL TURN MANAGER
            turnController = new TurnController(playerManager.GetPlayer());
            // 2b - DECIDIR QUE PLAYER COMIENZA PRIMERO
            turnController.DecideStarterPlayer();


            // CREATE MANAGER STATE
            // 3b - Inicializar los managers generales  ESTOS LOS VA A TENER EL GAME... ASI QUE SEGURO LO HAGAMOS DESDE AHI
            spawnManager = new SpawnManager(spawnManagerUI, this);
            combineManager = new CombineManager(this, combineManagerUI);
            movementManager = new MovementManager(this, moveManagerUI);
            actionsManager = new ActionsManager();
            yield return null;


            // LOADCOLLECTIONSTATE  => LAS COLLECTION LAS DEBERIA TENER EL SERVER Y PASARSELAS A CADA JUGADOR
            // 3- CARGAR LA GAME COLLECTION
            InGameCardCollectionManager inGameCardCollectionManager = new InGameCardCollectionManager(this, OnCardCollectionLoadComplete);
            //inGameCardCollectionManager.LoadAllCollection(users);// REACTIVAR CUANDO FUNCIONE BIEN LA DB
            inGameCardCollectionManager.LoadAllCollectionJson(playerManager.GetUsers().ToArray());
            while (isCardCollectionLoaded == false)
            {
                if (logOn) Debug.Log("WAITING FOR CARD COLLECTION TO LOAD");
                yield return null;
            }
            if (logOn) Debug.Log("GAME CARD COLLECTION LOADED");


            // CREATEDECKSTATE
            cardManager = new CardController(inGameCardCollectionManager, cardManagerUI, this);
            // VOLVER A ACTIVAR QUE ESTA ES LA FORMA DE CARGAR EL DECK SEGUN LA INFO DE LOS PLAYERS
            //cardManager.LoadDeckFromConfigurationData(playerManager.GetPlayer()[0], playerManager.playerConfigurationData);
            //cardManager.LoadDeckFromConfigurationData(playerManager.GetPlayer()[1], playerManager.playerConfigurationData);
            cardManager.LoadDeckTest(playerManager.GetPlayer()[0]);
            cardManager.LoadDeckTest(playerManager.GetPlayer()[1]);


            // WAIT FOR BOARD TO LOAD STATE
            while (isBoardLoaded == false)
            {
                if (logOn) Debug.Log("WAITING FOR BOARD TO LOAD");
                yield return null;
            }



            // INITIALIZECONTROLLERSTATE
            // 5 - INICIALIZAMOS LOS CONTROLES
            mouseController = new MouseController(0, board2DManager, Camera.main);
            keyBoardController = new KeyBoardController(0, board2DManager, Camera.main);
            tileSelectionManagerUI.SetController(board2DManager, mouseController, keyBoardController);


            // STARTGAMESTATE => ACA DEBERIA MANDAR CADA JUGADOR QUE ESTA READY, Y AHI EL SERVER EMITIRIA EL NUEVO STATE PARA CADA UNO
            // CREAMOS EL STATE INICIAL
            AdministrationState AdminState = new AdministrationState(10, this, 1);
            TurnState turnState = new TurnState(50, this);

            InitialAdministrationStateA initialAdminStateA = new InitialAdministrationStateA(40, this, 4);
            //Motion bannerMotion = informationUIManager.SetAndShowBanner(initialAdminStateA.stateName, 0.5f);
            //InvokerMotion.AddNewMotion(bannerMotion);
            //InvokerMotion.StartExecution(this);      
            //Debug.Log("CREATING BANNER");
            Animotion bannerMotion = informationUIManager.SetAndShowBannerAnimotion(initialAdminStateA.stateName, 0.5f);
            InvokerAnimotion.AddNewMotion(bannerMotion);
            InvokerAnimotion.StartExecution(this);

            IState changePhaseState = new ChangePhaseState(this, initialAdminStateA);
            baseStateMachine = new BaseStateMachine(this);
            baseStateMachine.PushState(changePhaseState, true);
            baseStateMachine.Initialize();



            // DEBERIA DIVIR PLAYER == BASE NEXUS Y GAME PLAYER == JUGADOR EN SI QUE VA A ENVIAR LOS COMANDOS... 
            // PODRIAMOS TENER UN GAME PLAYER QUE LO UNICA QUE VA A TENER ES UNA ID
            // ENTONCES CUANDO 
            // game.actionsManager.IncrementPlayerActions(game.turnController.CurrentPlayerTurn, managmentPoints);
            // ESTO DEBARA CAMBIARSE game.turnController.CurrentPlayerTurn
            // SI UN JUGADOR APRETA UNA UNIDAD... COMO SABEMOS SI MOSTRAR O NO LA UI DE LOS BOTONES
            // POR QUE SOLO PODEMOS MOSTRARLA EN TURN STATE, OK, Y SOLO CON UNIDADES NUESTRAS OK
            // Y SI ACTIVAMOS EL BOTON ENTONCES IGUAL SIEMPRE VAMOS A SER NOSOTROS TECNICAMENTE...
            // POR QUE EL BOTON SE CREA CON EL OCUPIER O CON EL PLAYER EN ESTE CASO...
            // 
            // 
            // YA QUE ESTOY ENTRANDO A UN NUEVO STATE game.actionsManager.IncrementPlayerActions LO DEBARIA REALIZAR EL SERVER
            // 

            // REALIZA SI O SI JUGADOR
            //// 1 - SUSCRIBIRSE AL EVENTO DE SELECCION ESTO SI LO TIENE QUE HACER EL JUGADOR
            //gmMachine.tileSelectionManagerUI.onTileSelected += ExecuteAction;
            //ExecuteAction(Tile action)
            //if (action == null)
            //{
            //    gmMachine.abilityButtonCreationUI.SetUnit(game.turnController.CurrentPlayerTurn);
            //}
            // ESTO LO HACE EL SERVER, VUELVE A REAHBILITAR LOS BOTONES DEL JUGADOR PARA SPAWN O TAKE CARD

            // SOLO JUGADOR YA QUE ES PARA LA UI
            //// NOS SUSCRIBIMOS AL EVENTO DE CAMBIAR EL TIEMPO
            //gameTimer.OnTimePass += gmMachine.uiGeneralManagerInGame.UpdateTime;



            // DEBE REALIZARLO SERVER
            //// 2 - TENGO QUE SETEAR LOS ACTIONS POINTS PARA ESTE JUGADOR
            //game.actionsManager.IncrementPlayerActions(game.turnController.CurrentPlayerTurn, managmentPoints);

            // DEBE REALIZARLO SERVER Y ENVIAR EL END STATE EN TODO CASO
            //// COMENZAMOS EL CONTADOR DE TIEMPO
            //base.OnEnter();

            // DEBE REALIZARLO SERVER ????
            //gmMachine.abilityButtonCreationUI.SetUnit(game.turnController.CurrentPlayerTurn);

        }

        private void OnBoardComplete()
        {
            if (logOn) Debug.Log("BOARD SE TERMINO DE CREAR");
            isBoardLoaded = true;
        }

        private void OnCardCollectionLoadComplete()
        {
            isCardCollectionLoaded = true;
        }

        private IEnumerator WaitForDatabaseToLoad()
        {
            while (DatosFirebaseRTHelper.Instance.isInit == false)
            {
                if (logOn) Debug.Log("WAITING FOR DATABASE TO LOAD");
                yield return null;
            }
            if (logOn) Debug.Log("DB LOADED");
        }
    }
}