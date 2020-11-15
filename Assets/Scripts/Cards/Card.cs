using System;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{

    public class Card
    {
        #region VARIABLES

        public Player ownerPlayer { get; protected set; }
        public int playerID { get; protected set; }
        private int _id;
        public int ID { get { return _id; } protected set { _id = value; } }

        private bool isChainable;
        public bool IsChainable { get { return isChainable; } protected set { isChainable = value; } }
        private CARDTYPE cardType;
        public CARDTYPE CardType { get { return cardType; } protected set { cardType = value; } }
        private ACTIVATIONTYPE activationType;
        public ACTIVATIONTYPE ActivationType { get { return activationType; } protected set { activationType = value; } }
        private List<CARDTARGETTYPE> posibleTargets;
        public List<CARDTARGETTYPE> PosibleTargets { get { return posibleTargets; } protected set { posibleTargets = value; } }
        private CardScriptableObject _cardSO;
        public CardScriptableObject CardSO { get { return _cardSO; } protected set { _cardSO = value; } }

        // hay card que van a tener un efecto que segun la cantidad de units que existan en ese momento se va a poder
        // utilizar en una o en otra ya que el efecto es solo para 1 una unidad pero hay varias que cumplen los requisitos
        // otras cards tal vez se usan en varias unidades a la vez, entonces no hace falta seleccionar nada en la lista de targets creados
        // hay otras que tienen como target a un jugador, entonces queremos usarlas ni bien las dropeamos sin seleccionar el target
        private bool requireSelectTarget;
        public bool RequireSelectTarget { get { return requireSelectTarget; } protected set { requireSelectTarget = value; } }

        private List<CardFiltterScriptableObject> cardFiltters;
        public List<CardFiltterScriptableObject> CardFiltters { get { return cardFiltters; } protected set { cardFiltters = value; } }

        // CARD EFFECTS

        #endregion

        /* CARDS */
        /* Las Cards se componen de 6 datos:
         * CardID => Un ID para cada Card, para buscarla facilmente en la base de datos
         * CardGraphcis => Los graficos de la Card, deberia estar en algun lugar donde se pueda actualizar Online
         * CardText => Todos los string de la Card, deberia estar en algun lugar donde se pueda actualizar Online
         * CardProperties => Las propiedades de cada Card, deberia estar en algun lugar donde se pueda actualizar Online
         * 
         * CardFiltter => Los Filtters que tiene esa Card para poder usarse, deberia estar en algun lugar donde se pueda actualizar Online
         * CardEffect => Los Effects que genera esa Card al usarse, deberia estar en algun lugar donde se pueda actualizar Online
         * 
         * Las Cards crean 3 Objetos
         * Card => La Card con sus Filtter y Effect
         * CardDisplay => La parte Visual de la Card en la UI
         * CardUIDragAndDrop => El codigo para interactuar con la UI de la Card
         *   
         * Los Datos de las Cartas tienen que estar en algun lado, en un CARDDATABASE puede ser:
         * ScriptableObject => Facil de usar y cargar con Datos
         * Json => Un poco mas complejos de usar, dificil para cargar Sprites como datos a guardar
         * Addressable => Tengo que aprender a usarlos
         * Server => Deberia ver como hacer para conectarme al server Online, tambien es complejo
         * 
         * CARDDATABE => List<CardScriptableObject> cardsSO
         *               Dictionary<int, Card> cards // int => cardsSO.CardID
         *               int createCardNewIndex
         * 
         *
         * CARDDATABASE : Contiene un Diccionario de ID y CARD, estas son todas las cartas que existen en el juego con su ID unica para cada una
         * CARD: Esta es la carta en si, tiene un ID unico para referenciarse en el momento de la creacion, pero tambien deberia tener un ID de el juego en si
         *       La diferencia es que si queremos tener una lista general de cartas estaria bueno que cada una tenga un ID diferente, ya que el jugador uno puede
         *       tener la misma carta que el jugador dos, y en el momento del drop tenemos que enviar la ID de alguna manera y si esa ID no es unica para cada carta
         *       entonces podemos llamar a las dos cartas a la vez, o el jugador uno podria tener dos cartas iguales, entonces en el drop vamos a buscar
         * 
         * Entonces... Cuando hago el drop desde el CardUI invoko un evento que es el OnDropCard(int ID)... 
         * y ese evento lo escucha el CardManager para filtrar si lo puede hacer? 
         * que deberia filtrar... que Player la dropeo.. y que ID es la carta que se dropeo, 
         * el CardUI deberia tener una referencia al player tambien? por que al CardUI le interesaria saber que player es???? , no, pero si le interesa tener un ID
         * se podria llegar a hackear ese ID y que el jugador 1 ponga el ID a una CardUI de una carta del Jugador 2, entonces cuando la dropea el ID va a ser de la Card del player 2
         * y va a intentar acceder a los filtros del player dos  if (ownerPlayer != AnimotionHandler.Instance.GetPlayer()) return;
         * entonces voy a ver si el dueno de la Card es el mismo que esta en su turno... asi que si lo quiero hacer en mi turno deberia dar bien ya que el ID va a ser correcto
         * entonces esa card va a decir, soy del Juagador 2, estoy en mi turno al ser dropeada, voy a ponerme para usar
         * podria chequear en el OnCardBeinDrag, pero pasaria lo mismo, ya que si heckee el juego y digo que soy el jugador 2 me va a dejar seleccionar los posibles target y todo... 
         * Como hago para chequear que sea el jugador correcto, por que si hackean eso ya esta
         * 
         * 
         * 
         * Los Pasos al levantar una Card
         * Chequeamos si la Card es Automatic o no
         * Si no es Automatica la agregamos a la Hand del Jugador con la Animacion
         * Si es Automatica entonces primero verificamos si tiene Target disponibles
         *      Si no tiene target hacemos una animacion de No Target y la mandamos al Cementerio
         *      Si tiene target entonces la dejamos en el espacio de Espera de Seleccion de Target
         *      
         *      
         *  
         * FXANIMATION => ANIMACION QUE SE PRODUCE CUANDO SE USA / MIENTRAS ESTA EN USO / CUANDO SE VA EL EFECTO
         *       
         */

        public Card(int ID, Player ownerPlayer, CardScriptableObject CardSO)
        {
            this.ID = ID;
            this.ownerPlayer = ownerPlayer;
            this.CardSO = CardSO;

            isChainable = CardSO.IsChainable;
            cardType = CardSO.CardType;
            activationType = CardSO.ActivationType;
            posibleTargets = CardSO.PosibleTargets;

            //cardFiltters = new List<CardFiltter>();
            cardFiltters = CardSO.Filtters;

        }

        public Card(int ID, CardScriptableObject CardSO)
        {
            this.ID = ID;
            this.CardSO = CardSO;
            isChainable = CardSO.IsChainable;
            cardType = CardSO.CardType;
            activationType = CardSO.ActivationType;
            posibleTargets = CardSO.PosibleTargets;

            //cardFiltters = new List<CardFiltter>();
            cardFiltters = CardSO.Filtters;
        }

        public void SetPlayer(Player ownerPlayer)
        {
            this.ownerPlayer = ownerPlayer;
        }

        // ACTIVATION CONDITION
        // OnCardBeginDrag() 
        // CHEQUEAR DE QUIEN ES EL TURNO
        // SI NO ES NUESTRO TURNO ENTONCES SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
        // SI ES NUESTRO TURNO ENTONCES PASA LO SIGUIENTE
        // ACA DEBEMOS BUSCAR LOS TARGETS Y MARCARLOS DE ALGUNA MANERA
        // TODO LOS LOS IOcuppy VAN A SER TAMBIEN ICardTarget
        // UNIT = UNIT // OBJECT = BOARDOBJECT // PLAYER = BASENEXUS // NONE = ?
        // TODAS LAS Tiles VAN A SER TAMBIEN ICardTarget
        // SPAWN = SPAWN // BATTLEFIELD = BATTLEFIELD
        // CREAMOS UNA LISTA DE TIPO ICARDTARGET TARGETSENCONTRADOS
        // RECORREMOS LA LISTA DE POSIBLES TARGET DE LA CARD
        // RECORREMOS LA LISTA DE TILES
        // CHEQUEAR SI ESTAN OCUPADAS O NO
        // SI NO ESTAN OCUPADAS VAMOS A VER QUE TIPO DE CARDTARGETTYPE SON LA TILE
        // SI SON DEL TIPO DE TARGET QUE ESTA EN LA LISTA DE POSIBLES TARGET DE LA CARD Y NO ESTA EN LA LISTA DE TARGETSENCONTRADOS LO AGREGAMOS
        // SI ESTA OCUPADA ENTONCES VEMOS QUE TIPO DE CARDTARGETTYPE ES EL OCCUPIER
        // SI ES DEL TIPO DE TARGET QUE ESTA EN LA LISTA DE POSIBLES TARGET DE LA CARD Y NO ESTA EN LA LISTA DE TARGETSENCONTRADOS LO AGREGAMOS
        // CHEQUEAR LA CANTIDAD DE ITEMS DE LA LISTA DE TIPO ICARDTARGET TARGETENCONTRADOS 
        // SI LA LISTA ES IGUAL A 0 DESPUES DE RECORRER TODO, ENTONCES NO TENEMOS TARGET POSIBLES, DE SER ASI SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
        // SI LA LISTA ES MAYOR A 0 ENTONCES HAY TARGET DISPOBIBLES

        // ACTIVATION CONDITION FILTTER
        // ACA ES DONDE DEBEMOS VER SI LOS TARGETS DISPONIBLES CUMPLEN LA/S "CONDICION DE ACTIVACION DE LA CARD"
        // ESTAS CONDICIONES SON PARA QUE NO SE USEN LAS CARDS AL PEDO, EJEMPLO, RESTA 1 AL ATAQUE DE UNA UNIDAD, SI YA ESTA EN 0 EL ATK DE LA UNIDAD
        // ENTONCES NO VAMOS A PONERLO EN -1, LA CONDICION DE ACTIVACION ES QUE EL TARGET TENGA MAS DE 0 DE ATK POW POR EJEMPLO EN ESTE CASO
        // ENTONCES... 
        // ACTIVATION CONDITION FILTTER
        // RECIBE UNA LISTA DE TARGET Y FILTRA SEGUN DIFERENTES CONDICIONES
        // LAS CONDICIONES DE ACTIVACION PUEDEN SER:
        // CHEQUEAR TILE LIBRE NO CUENTA, YA QUE AL BUSCAR EL TARGETTYPE VAMOS A BUSCAR EL SPAWN O EL BATTLEFIELD QUE SERIA LO MISMO
        // 
        //
        // EN ALGUN LUGAR TENGO QUE CONVERTIR DE ICARDTARGET A UNIT/PLAYER..ETC???? YA QUE VOY A TENER QUE VER STATS,ACTIONS
        // 
        //
        //
        //
        // CHEQUEAR EL ESTADO DE UN STAT. EJ: ATK > 0 // ACTUALHP < MAXHP // ATKRANGE < 3 // MOVERANGE < 3
        // CHEQUEAR QUE EL RIVAL/JUGADOR TENGA CARTAS TARGETTYPE BASENEXUS 
        // CHEQUEAR SI EL TARGET ES ENEMIGO O AMIGO
        // CHEQUEAR QUE TENGAN QUE TENGAN BUFFS EN SUS STATS Y REVERTIRLOS ?????
        // CHEQUEAR SI EL ID DE UN ACTIONMODIFIER ESTA EN UNA UNIDAD. EJ: SI LA UNIDAD ESTA EN MODO DE DEFENSA
        // CHEQUEAR QUE HAYA UN ESPACIO LIBRE ATRAS DE EL TARGET
        // CHEQUEAR QUE CIERTAS UNIDADES ESTEN EN LA BASE 


        // CLASE LOGO Y ARROW
        // DEBERIAMOS TENER UNA CLASE PARA CREAR EL MODO DE SELECCION A UTILIZAR
        // LA CLASE SE ENCARGA DE CREAR LOS LOGOS DE SELECCION, LA LINEA, DE PRENDER Y APAGAR LA LINEA Y ACTUALIZAR SU POSICION, Y DE INSTANCIAR Y DESTRUIR LOS LOGOS
        // ES UN MANAGER EN GENERAL ENTONCES LO QUE TIENE QUE TENER UN TRACKEO ES DE LA POSICION DEL RECTRANSFORM DE LA CARD SI FUERA UN SOLO TARGET PARA LA LINEA
        // Y DE LA POSICION DE LOS TARGET PARA PONERLE LOS LOGOS ARRIBA
        // LA UI DE SELECCION PUEDE CONSTAR DE UNA O DOS PARTES
        // UNA ES EL LOGO INDICADOR DEL TARGET
        // LA OTRA ES LA LINA QUE UNA A LA CARD CON EL TARGET DE SOLO EXISTIR UNO SOLO
        // ENTONCES PRIMERO CHEQUEAMOS CANTIDAD DE TARGETS
        // SI ES UN TARGET ENTONCES VAMOS A CREAR UNA LINEA ENTRE LA CARD Y EL TARGET // CARDRECTTRANSFORMPOSITION // TARGET.TOSCREENPOINTPOSITION
        // LUEGO PONEMOS EL LOGO INDICADOR SEGUN EL TIPO DE LA CARD BUFF/NERF/NEUTRAL Y SEGUN EL TIPO DE CARDTARGETTYPE DEL ICARDTARGET UNIT/BOARDOBJECT/BASENEXO...
        // SI SON VARIOS ENTONCES SOLO LOS VAMOS A MARCAR CON LOGO INDICADOR SEGUN EL TIPO DE LA CARD BUFF/NERF/NEUTRAL SIN CREAR LA LINEA Y SEGUN EL TIPO DE CARDTARGETTYPE DEL ICARDTARGET UNIT/BOARDOBJECT/BASENEXO...
        // SI ES UN ICardTarget ENTONCES DEBERIAN TENER UN MODO/FUNCION/METODO OnCardTargetSelection()
        // LOGOS
        // PLAYER BUFF/NERF/NEUTRAL
        // UNIT BUFF/NERF/NEUTRAL
        // TILE NEUTRAL
        // BOARDOBJECT BUFF/NERF/NEUTRAL

        // OnCardDrag()
        // CHEQUEAR LA CANTIDAD DE ITEMS DE LA LISTA DE TIPO ICARDTARGET TARGETENCONTRADOS 
        // SI LA LISTA ES MAYOR A 0 ENTONCES HAY TARGET DISPOBIBLES
        // SI HAY UN SOLO TARGET ENTONCES NO HACEMOS NADA
        // SI HAY MAS DE UN TARGET DEBEMOS ACTUALIZAR LA POSICION ENTRE LA LINEA Y EL TARGET PARA QUE LA CARD AL MOVERSE MANTENGA LA UNION CON LA LINEA

        // OnCardDrop()
        // CHEQUEAR LA CANTIDAD DE ITEMS DE LA LISTA DE TIPO ICARDTARGET TARGETENCONTRADOS 
        // SI LA LISTA ES IGUAL A 0 ENTONCES LA CARTA VUELVE A LA MANO DEL JUGADOR
        // SI LA LISTA ES MAYOR A 0 ENTONCES LA CARTA VA A EL LUGAR DE ESPERA DE SELECCION DE TARGET
        // SI EL TARGET ES UNO SOLO LA CARD DEBERIA USARSE AUTOMATICAMENTE CUANDO LA DROPEAMOS EN EL AREA DE JUEGO SIN TANTO PREAMBULO OnCardUse()
        // TAMBIEN DEBEMOS CHEQUEAR EL BOOL RequireSelectTarget, YA QUE PUEDE SER QUE NO REQUIERA QUE SELECCIONEMOS AL TARGET PARA APLICAR LA CARD 
        // Y QUE SE APLIQUE AUTOMATICAMENTE EN EL/LOS TARGET/S QUE ESTEN EN LA LISTA. DE SER ASI SOLO PASARA A REALIZAR EL EFECTO DE OnCardUse()
        // SI HAY MAS DE UN TARGET Y REQUIERE SELECCIONAR TARGET PARA USARSE
        // EN LA ESPERA DE SELECCION DE TARGET DE UNA CARD NO PODEMOS HACER DRAG AND DROP NI REACOMODAR A NINGUNA OTRA CARD
        // Entrariamos en el State OnCardTargetSelection DONDE AGUARDAMOS EL INPUT DE SELECCION PARA VER SI LO QUE SELECCIONAMOS ES UN TARGET VALIDO
        // SI EL TARGET SELECCIONADO NO ESTA DENTRO DE LA LISTA ENTONCES VOLVEMOS AL TURN STATE Y LA CARTA VUELVE A LA MANO DEL JUGADOR
        // SI EL TARGET SELECCIONADO ESTA DENTRO DE LA LISTA ENTRONCES OnCardUse()


        // PERMANENCIA DE EFECTO.. esto lo define el action modifier con los expire... podemos estar suscriptos al OnChangeTurn Event del GameManager
        // o podemos estar suscripto a un evento OnUnitDie y hacer algo cada vez que una unidad se muera
        // Y QUE PASA SI QUEREMOS HACER UN STAT MODIFIER DE + 2 DE ATAQUE QUE DURE 2 TURNOS.........
        // PODRIA LLEGAR A AGREGARLE AL PLAYER UN "ACTION MODIFIER" QUE EN EL ENTER LE APLICO LOS +2 DE ATAQUE A LA UNIDAD
        // ME SUSCRIBO AL EVENTO DE OnChangeTurn Y CUANDO HAGO EL EXPIRE LE APLICO UN NERF DE -2 DE ATAQUE A LA UNIDAD 
        // ME SUSCRIBO TAMBIEN AL EVENTO OnUnitDie POR LAS DUDAS DE QUE SE MUERA ANTES DE QUE PUEDA APLICAR EL EXPIRE Y SE LO TERMINE APLICANDO A LA NADA

        public virtual void OnDropCard(int ID)
        {
            if (this.ID == ID)
            {
                Debug.Log("I have been droped " + CardSO.Description);
                CheckPosibleTargets();
            }
        }

        public void CheckPosibleTargets()
        {
            // CHEQUEAR DE QUIEN ES EL TURNO
            // SI NO ES NUESTRO TURNO ENTONCES SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (ownerPlayer != GameCreator.Instance.turnManager.GetActualPlayerTurn()) return;

            //bool onTargetSelection = true;

            List<ICardTarget> foundTargets = new List<ICardTarget>();

            // RECORREMOS LA LISTA DE POSIBLES TARGET DE LA CARD
            for (int i = 0; i < posibleTargets.Count; i++)
            {
                // RECORREMOS LA LISTA DE TILES
                for (int x = 0; x < GameCreator.Instance.board2D.GridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < GameCreator.Instance.board2D.GridArray.GetLength(1); y++)
                    {
                        Tile actualTile = GameCreator.Instance.board2D.GetGridObject(x, y);
                        if (actualTile.IsOccupied())
                        {
                            if (actualTile.GetOccupier().CardTargetType == posibleTargets[i])
                            {
                                if (foundTargets.Contains(actualTile.GetOccupier()) == false)
                                {
                                    foundTargets.Add(actualTile.GetOccupier());
                                }
                            }
                        }
                        else
                        {
                            if (actualTile.CardTargetType == posibleTargets[i])
                            {
                                if (foundTargets.Contains(actualTile) == false)
                                {
                                    foundTargets.Add(actualTile);
                                }
                            }
                        }
                    }
                }
            }

            // SI LA LISTA ES IGUAL A 0 SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (foundTargets.Count == 0) return;

            foundTargets = FilterTargets(foundTargets);

            Debug.Log("foundTargets.Count " + foundTargets.Count);

            if (foundTargets.Count == 0) return;

            //bool posibleTargetFound = true;

        }

        private List<ICardTarget> FilterTargets(List<ICardTarget> cardTargets)
        {
            List<ICardTarget> filtterTargets = new List<ICardTarget>();

            for (int i = 0; i < cardTargets.Count; i++)
            {
                bool allPass = true;

                for (int x = 0; x < cardFiltters.Count; x++)
                {
                    ICardTarget cardTarget = cardFiltters[x].CheckTarget(cardTargets[i]);

                    if (cardTarget == null)
                    {
                        allPass = false;
                    }
                }

                if (allPass)
                {
                    if (filtterTargets.Contains(cardTargets[i]) == false)
                    {
                        filtterTargets.Add(cardTargets[i]);
                    }
                }
            }

            return filtterTargets;
        }

        public void OnCardEffectApplyTarget(List<ICardTarget> cardTargets)
        {
            // Recorro la lista de Targets y aplico el efecto
            // Hay que saber que el/los Filtter y el/los Effect estan como enlazados de alguna manera
            // No voy a poder aplicar robar dos Cards si antes no chequee en el Filter tener dos Cards
            // Entonces en el momento de aplicar el Effect deberiamos chequear otra vez si los target cumplen los filtros
            // Por las dudas que algo en el medio los haya alterado
            // NO DEBERIAMOS PASAR POR CheckPosibleTargets SOLO POR FilterTargets
            cardTargets = FilterTargets(cardTargets);

            // Bueno... por un lado tengo los Modifiers/Effects de la Card y por el otro lado los Targets a aplicarles el efecto
            // en el momento del Apply primero tengo que cargar el card effect con el Occupier correspondiente
            // Esto lo puedo hacer desde el SetOccupier(IOcuppy occupier) de el AbilityModifier/Effect
            // Y el Modifier lo puedo tener en una lista static en el CardDataBase
            List<AbilityModifier> mods = new List<AbilityModifier>();

            mods.Add(CardPropertiesDatabase.GetCardAbilityModifierFromID(0));

        }
    }
}

namespace CardsEffects
{
    /* 
     * CARD EFFECTS
     * ACTION MODIFIERS
     * STATS MODIFIERS
     * 
     * LAS CARDS VAN A CONTENER STATS MODIFIER / ACTION MODIFIER
     * STAT MODIFIER => MODIFICA LOS STATS DE LAS UNIDADES
     * ACTION MODIFIER => MODIFICA LAS ACCIONES O REALIZA MODIFICACIONES A LAS ACTIONS O LOS STATS CUANDO SE REALIZAN LAS ACCIONES, PUEDE REALIZARSE ANTES O DESPUES DE EJECUTARSE UNA ACCION
     * SI ES UN MODIFIER QUE SE EJECUTA AL INICIO ENTONCES ES UN EARLYMODIFICATION 
     * SI ES UN MODIFIER QUE SE EJECUTA AL FINAL ENTONCES ES UN ENDMODIFICATION
     * 
     * enum ACTIONMODIFIEREJECUTIONTYPE{EARLY, END} // CUANDO DEBE REALIZARSE EL CHEQUEO DE LA MODIFICACION DE LA ACTION
     * 
     * 
     * CUANDO SE ACTIVA El Modifier/Effect se aplica automaticamente en el target si este es valido, ahora bien, su efecto puede surtir inmediatamente
     *                  o el efecto puede surtir solo cuando esta en su turno, o el efecto puede activarse solo cuando surte determinado evento, o cuando
     *                  la habilidad a la cual afecta (si es que esta enlazado a una habilidad) comienza a ejecutarse.                  
     *                  => INMEDIATA 
     *                  => CUANDO SE EJECUTA LA HABILIDAD
     *                  => CUANDO EL JUGADOR ESTA EN SU TURNO
     *                  => SI PASAS DETERMINADO EVENTO
     * 
     * Enter()//OnUse() -> ni bien arrancamos la MODIFICACION la prepara y llena todas sus dependencias y suscripciones
     * Expire() -> cuando la accion deja de surtir efecto y se va del actor
     * ExpireCondition => Puede llegar a ser una clase que tenga las condiciones para que un modifier deje de actuar, y se puede modificar en determinados eventos EJ: OnTurnChangeEvent()
     * ActivationCondition => Puede tambien ser una clase que tenga las condiciones para que se active un modifier, puede activarse con eventos EJ: OnUnitTakeDamage()
     * 
     * CUANTO TIEMPO LO AFECTA => PERMANENTE
     *                         => POR TURNOS (DURA "X" CANTIDAD DE TURNOS 1 TURNO = 2)
     *                         => HASTA QUE PASE DETERMINADO EVENTO (X DANO, LEVANTE MAS DE 2 CARDS, ETC)
     * 
     * 
     *  Todos los Action Modifier deberian tener un ORDER, una forma de ordenarlo y decir como van a afectar en la lista
     *  Ej: Tengo en las lista de Attack Modifier dos Action Modifier
     *      El Primero era +1hp cada vez que hacemos un Attack Action
     *      El Segundo es cancela el Attack Action
     *      Si no hay orden, entonces vamos a recibir +1hp sin haber atacado
     *      Entonces estaria bueno que tengan un orden 
     *      Cancel Attack Order 1
     *      +1hp          Order 2
     *      Cada vez que recorremos la Lista de Action Modifier, vamos a ir chequeando uno por uno
     *      El primero va a ser Cancel Attack, En su Execute va a poner a la Attack Action como cancel y como usada
     *      Cuando termina de hacer el execute va a volver a la Lista
     *      Antes de revisar el proximo Action Modifier, vamos a chequear si la Attack Action no esta Cancel o Usada
     *      Tal vez deberiamos verificar si tenemos los suficientes puntos como para efectuarla
     *      O tal vez deberiamos chequear si podemos efectuarla... imaginate que el efecto sea empujar a los enemigos
     *      antes de atacar o danar por 1hp a todos los enemigos alrededor antes de atacar, y justo matamos al enemigo
     *      que ibamos a atacar, se deberia efectuar la accion igual contra un Null Target... ?????
     *  
     *  
     *  
     *  Todos los action Modifier van a tener un ID, para poder encontrarlos facilmente en una lista
     *  
     *  
     *  // Todas estas habilidades van a tener un OnEnter/Exit para poder avisarle a otras habilidades o modificadores lo que esta pasando
     *  WHEN PLAYER TAKE CARD FROM THE DECK
     *  WHEN PLAYER USE A CARD AND CARD GOES TO GRAVEYARD
     *  WHEN PLAYER SPAWN UNIT
     *  WHEN UNIT MOVES 
     *  WHEN UNIT/PLAYER ATTACK 
     *  WHEN UNIT/PLAYER/BOARDOBJECT TAKE DAMAGE
     *  WHEN UNIT DIE
     *  WHEN UNIT ENTER DEFENSE MODE 
     *  WHEN UNIT COMBINE/BEEINGCOMBINE Entra como lo mismo, ya que cuando esta efectuando la descombinacion podemos buscar a todas sus unidades y aplicarles un efecto
     *  WHEN UNIT 
     *  WHEN UNIT DECOMBINE/BEEING DECOMBINE Tanto la principal como todos sus hijos pueden recibir modificaciones
     *  WHEN UNIT FUSION
     *  WHEN UNIT EVOLVE
     *  
     *  
     *  
     *  
     *  
     *  REMUEVE UN ACTION MODIFIER DE UNA LISTA, Busca Action por ID, busca en la Lista de Action Modifier un ID de Modifier
     *  SE SUMA A UNA LISTA DE ABILITIESMODIFIER DENTRO DE UNA ABILITYACTION
     *  SE APLICA UN STAT MODIFIER EN UN STAT 
     *  
     *  
     *  
     *  
     *  GENERA UNA ACTION DETERMINADA Ej: Levantar Una Card / Revivir una Card del cementerio / Spawn a Unit
     *  PERFORM ACTION REVIVIMOS UNA CARTA DEL CEMENTERIO
     *  Esta no se si entra, ya que revivir una carta del cementerio es como un Modifier/Effect especial de una Card especifica, entonces no entraria
     *  como AbilityAction ya que el jugador no va a tener esa ability action... como la ejecutariamos..
     *  Primero buscamos que el jugador tenga cartas en el cementerio
     *  Pueden cumplir cierto criterio esas Cards para ser seleccionadas
     *  Una vez que tenemos las listas hacemos una animacion de esas Cards del cementerio al centro de la pantalla en fila???
     *  Esto es complicado ya que puede ser que 1 Card cumpla el requisito, como 20 Cards cumplan el requisito....
     *  Hay que mostrarlas de una manera ordenada ya sea 1 o 20 Cards
     *  Luego hay que entrar en el State de WaitTargetSelection
     *  Seleccionamos una Card o varias Cards segun lo que el Efecto Diga
     *  Puede ser que algunas vayan a la Mano, y otras se usen Automaticamente
     *  Puede ser que se usen automaticamente
     *  Aca va a entrar tener una lista de Cards para usarse como en una Cadena
     *  WHEN PLAYER REVIVE A CARD FROM GRAVEYARD
     *  
     *  
     *  Todas estas entran en lo mismo, ya que dijimos que podemos crear una lista especial List<AbilityModifier> en cada Ocuppier
     *  Asi podemos tener todas estos efectos especiales que requieren algun evento y que no estan enlazados a una habilidad en especifica
     *  WHEN GAME CHANGE TURN 
     *  WHEN GAME ADD ACTION POINTS TO PLAYER
     *  WHEN ACTION MODIFIER IS APPLY / END
     *  WHEN CHAIN ACEPT
     *  
     *  
     *  SE APLICA UN STAT MODIFIER EN UN STAT POR UN TIEMPO/SUCESO DETERMINADO
     *  POR CADA TURNO RECIBE +1 ATK
     *  Desde donde levantamos esa informacion... quien va a tener el Enter o Execute para suscribirse al evento?
     *  ... O Tenemos una lista de Effects lo que se encargue de hacer el Change Turn para todos esos efectos especiales?
     *  El +1 ATK LO VA A RECIBIR LA UNIDAD QUE TENGA APLICADA EL MODIFICADOR DE STAT... 
     *  WHEN STAT MODIFIER IS APPLY / END 
     *  Este tambien es medio complicado, ya que para aplicarse no hay problema... pero... tecnicamente Stat Modifier "Termina"
     *  Salvo que dure una cierta cantidad de turnos, en donde Suscribimos el Expire, ya que el Effect al momento de aplicarse va a
     *  Decir, Unidad +2hp // Al momento de irse va a decir Unidad -2hp
     *  Cada ocuppier podria tener una lista de SpecialEffects/Modifiers para estos casos "Especiales".
     *  Entonces, si es un AbilityModifier o un EventAbilityModifier que me agrega dos de vida, y despues de dos turnos me saca los dos de vida que me agrego
     *  En el momento del de setear el Occupier hacemos el Enter()
     *  Enter()=> Chequeamos que la unidad tenga el Stat Health, y le aplicamos un buff de +2hp
     *         => Nos suscribimos al OnChangeTurn event la funcion Expire()
     *  Expire() => Aca vamos descontando de a 4 ya que 2 = 1 turno 4 = 2 turnos
     *           => Cuando llega a 4 vamos a seguir vivos por que esta en la unidad el evento
     *           => Chequeamos que la unidad tenga el Stat Health, y le aplicamos un nerf de -2hp
     *           => Nos desuscribimos al OnChangeTurn event la funcion Expire()
     * 
     * 
     *  SE APLICA UN STAT MODIFIER EN UN STAT CUANDO SURGE UN EVENTO
     *  CHANGE STAT ON EVENT +2 ATK POWER FOR EVERY ENEMY KILLED BY THIS UNIT
     *  Enter()=> Chequeamos que la unidad tenga el AttakPow Stat
     *         => Nos suscribimos al OnUnitDie event la funcion Execute()
     *  Execute() => Aca vemos si el AbilityEvntInfo es de tipo DieAbilityInfo
     *            => Si es asi entonces vemos si el Killer de la unit somos nosotros
     *            => Chequeamos que la unidad tenga el Stat AttakPow, y le aplicamos un buff de +2atkPow
     *           => Si la unit que esta muriendo dieOccuppy somos nosotros entonces
     *           => Cuando la Unit muere nos desuscribimos al OnUnitDie event la funcion Execute()
     *           
     *           
     */
}

namespace MikzeerGameNotas
{
    /*
         * EJ: 
     * A) INCREMENTA EL ATAQUE UN 50% MIENTRAS AL MOMENTO DE ATACAR TENGAS MAS UNIDADES QUE EL RIVAL Y SOLO CONTRA ENEMIGOS QUE TENGAS MAS DEL 90% DE VIDA
     * Esto seria un Action Modifier que modifica un Stat de la Unit en el Attack State de la misma
     * Primero vemos si el Target tiene mas del 90% de su vida
     * Luego recorremos el tablero y hacemos un conteo de unidades amigas/enemigas
     * Si tiene mas del 90% y hay mas amigos entonces incrementa un 50% ek ataque
     *
     * B) COUNTER ATTACK SOLO A UN ATAQUE DE UNA UNIT(CUANDO SOMO ATACADOS / QUE TIPO DE ATACANTE? / 
     *    INSERTAR COUNTER ATK EN UNA LISTA DE ACCIONA A REALIZAE DESPUES DEL ATAQUE SI SOBREVIVE
     * Esto seria un Action Modifier que.........devuelve otra accion al finalizar la otra, la accion es un TakeDamage y es un END ACTION MODIFIER
     * Primero vemos que clase de Attacker es... deberiamos saber si es una Unit o un Player o un BoardObject
     * Primero vemos que tipo de ataque estamos reciviendo MAGICO/FISICO
     * Hacemos una copia de ese Damage
     * Insertamos una Command de Attack, solo que ahora el Attacker es el Damager, deberiamos Convertirlo de alguna manera
     * Ya que puede haber muchos attacker, asi que deberia ver eso
     * Tambien la accion deberia verificar que la Unit todavia este viva para hacer el counter ya que no tendria sentido que me mate y aplique el counter igual
     *
     *
     * C) ESCUDO SOLO PARA DANO FISICO POR UN ATAQUE
     * Esto seria un Action Modifier en el TakeDamage y es un EARLY
    * Primero vemos que tipo de dano es MAGICO/FISICO
   * Luego ponemos en 0 esa cantidad de dano a recibir
     * luego eliminamos esta Action Modifier
     */

    /* PUEDE ATACAR DOS VECES EN ESE TURNO SI NO REALIZO NINGUNA OTRA ACCION  */
    /*                             
     * CUANDO LA UNIDAD TERMINA EL ATAQUE Y LA ACCION YA ESTA EN HASEXECUTE = TRUE, LO QUE HACEMOS ES DECIRLE HASEXECUTE = FALSE, LO QUE HACE ESTO ES QUE PUEDA VOLVER A EJECUTAR
     * LA ACCION SIEMPRE Y CUANDO TENGA SUFICIENTES ACTION POINTS DISPOIBLES, SINO DESPERDICIA LA CARTA.
     *
     * 
    */

    /* REMOVER UN ACTION MODIFIER ESPECIFICO*/
    /*                      
     * TODAS LAS ACTION MODIFIER VAN A TENER UN ID? PARA PODER USARLAS FACILMENTE
     * ENTONCES CON ESTE TENIENDO ES ID PODEMOS OBTENER UNA REFERENCIA A UN ACTION MODIFIER ESPECIFICO???
     * EJ:
     * BUSCAMOS LA ACCION EN SI: DEFENEDACTION
     * BUSCAMOS EN SU LISTA DE ACTION MODIFIER: LIST<ACTIONMODIFIER>
     * SI LA ID QUE BUSCAMOS ES LA QUE MISMA ID DE ESE ACTION MODIFIER, ENTONCES LO REVERTIMOS(SI ES QUE SE NECESITARA PERO NO CREO), PARA QUITARLO Y LO REMOVEMOS DE LA LISTA 
     *                                
    */

    /*SHIELD CARD UNIT TO AFFECT WILL TAKE 0 DAMAGE ON THE NEXT INDIRECT ATTACK IT RECIVES*/
    /*
     *  UNIT => BOOL CANRECIVEDAMAGE(DAMAGE damage) ESTA ES UNA FUNCION QUE NOS VA A DECIR SI PUEDE SER ATACADA O NO (ESTA DENTRO DEL RANGO, PUEDE RECIBIR ESE DANO)
     *          
     *          LIST<DAMAGEUNIT> TAKEDAMAGE(DAMAGE damage) ESTA FUNCION HACE QUE NUESTRA UNIDAD RECIBA DANO Y DEVULEVE UNA LISTA DE LA/S UNIDAD Y CUANTOS DANO SE LE HIZO A CADA UNA Y SI LA MATO 
     *          ONDIE()
     *          IF(CANRECIVEDAMAGE()) UNIT.TAKEDAMAGE()
     *          UNIT.ONDIE()
     * 
     * 
     * 
     *  NEED => UNIT TO AFFECT // DAMAGA RECIVE // TAKEDAMAGE()
     *  AFFECT => IF DAMAGE TYPE = DMGTYPE.INDIRECT = DMG.POWER = 0 // EXPIRE()
    */

    /*POR CADA VEZ QUE RECIBE DANO AUMENTA +1 ATK*/
    /*
        EN EL TAKEDAMAGE()
        ACTION MODIFIER => Potencia el ataque + 1 por cada dano que reciba
        STAT MODIFIER => +1 atk
     */

    /*SI ESTA COMBINADA RECIBE EL DANO PRIMERO*/
    /*
        SI ESTA COMBINADA
        AGARRO LA LISTA DE UNIDADES QUE CONFORMAN ESA COMBINACION
        SELECCIONO A LA UNIDAD QUE ESTA EN EL ESPACION 0, YA QUE ES LA QUE TIENE EL BUFF
        LA PONGO EN EL PRIMER LUGAR PARA RECIBIR DANO
        UNA VEZ QUE TERMINA EL TAKEDAMAGE VUELVO A ACOMODAR LA LISTA OTRA VEZ EN EL ORDEN QUE ESTABA SI LA UNIDAD SIGUE VIVA
        PODRIA LLEGAR A TENER UN STACK Y NO UNA LISTA PARA HACER MAS FACIL EL PUSH/PEEK/POP
     */

    /*LA UNIDAD NO PUEDE MOVERSE*/
    /*

     */

    /*LA UNIDAD NO PUEDE ATACAR*/
    /*

     */

    /*LA UNIDAD NO PUEDE RECIBIR DANO*/
    /*

     */


    /*REVIVIMOS UNA CARTA DEL CEMENTERIO*/
    /*
        NECESITAMOS UNA CEMENTERIO DE LAS CARTAS EN EL CEMENTERIO
        NECESITAMOS TENER UNA CARTA EN EL CEMENTERIO
     */

    /*REVIVE X CANTIDAD DE CARTAS DEL CEMENTERIO DEL TIPO Y*/
    /*
        NECESITAMOS UNA CEMENTERIO DE LAS CARTAS EN EL CEMENTERIO
        NECESITAMOS TENER UNA CARTA EN EL CEMENTERIO
        MOVER FROM{GRAVEYARD} TO {HAND}
        MoveCardEffect(from, to, validCardFilter)
     */


    /* +2ATK // AUTOMATIC // 1 TURN DURATION*/
    /*                      
     * 
     * EJ + 2 ATK POWER // 1 TURN
     *  NEED => UNIT TO AFFECT // STATS TYPE ATTACK POWER
     *  AFFECT => UNIT TO AFFECT ATTACK POWER + 2
     *            ONUNITDIE(UNIT THIS) += EXPIRE
     *            ONTURNCHANGE(float amount) += REST EFFECT LIFE TIME
     *  REST EFFECT LIFE TIME => IF TIMELEFT = TOTALDURATION = EXPIPRE()   0.5f half turn // 1 turn
     *  EXPIRE => UNIT TO AFFECT ATTACK POWER - 2
     *            ONUNITDIE(UNIT THIS) -= EXPIRE
     *            ONTURNCHANGE(float amount) -= REST EFFECT LIFE TIME
    */


    /*+2 ATK POWER FOR EVERY ENEMY KILLED BY THIS UNIT*/
    /*
     *  NEED => UNIT TO AFFECT // LIST OF AMOUNT OF ENEMIES KILLED BY THIS UNIT //  STATS TYPE ATTACK POWER
     *  AFFECT => 
    */

    /*POR CADA UNIDAD ALIADA QUE MUERE +1 ATK A LA UNIDAD SELECCIONADA*/
    /*
        EN DONDE PONEMOS ESTE STAT MODFIER??? 
        .. PODRIA LLEGAR A TENERLOS EL PLAYER... Y QUE ESTE SUSCRIPTO AL EVENTO OnUnitTakeDamage, cuando se ejecuta ese evento
        lo que hace es agregar un UnitStatModification a la Unit que tenia como referencia para accionar 
     */

    /*POR CADA TURNO RECIBE +1 ATK*/
    /*

     */

}