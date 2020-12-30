using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Card
    {
        #region VARIABLES
        private int _idInGame;
        public int IDInGame { get { return _idInGame; } protected set { _idInGame = value; } }
        public Player ownerPlayer { get; protected set; }
        private CardData _cardData;
        public CardData CardData { get { return _cardData; } protected set { _cardData = value; } }
        private bool requireSelectTarget;
        public bool RequireSelectTarget { get { return requireSelectTarget; } protected set { requireSelectTarget = value; } }                
        #endregion

        public Card(int ID, Player ownerPlayer, CardData CardData)
        {
            this.IDInGame = ID;
            this.ownerPlayer = ownerPlayer;
            this.CardData = CardData;
        }
      
        public virtual void OnDropCard(int ID)
        {
            if (this.IDInGame == ID)
            {
                Debug.Log("I have been droped " + CardData.Description);
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
            for (int i = 0; i < CardData.cardTargetTypes.Count; i++)
            {
                // RECORREMOS LA LISTA DE TILES
                for (int x = 0; x < GameCreator.Instance.board2D.GridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < GameCreator.Instance.board2D.GridArray.GetLength(1); y++)
                    {
                        Tile actualTile = GameCreator.Instance.board2D.GetGridObject(x, y);
                        if (actualTile.IsOccupied())
                        {
                            if (actualTile.GetOccupier().CardTargetType == CardData.cardTargetTypes[i])
                            {
                                if (foundTargets.Contains(actualTile.GetOccupier()) == false)
                                {
                                    foundTargets.Add(actualTile.GetOccupier());
                                }
                            }
                        }
                        else
                        {
                            if (actualTile.CardTargetType == CardData.cardTargetTypes[i])
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

                for (int x = 0; x < CardData.cardTargetFiltters.Count; x++)
                {
                    ICardTarget cardTarget = CardData.cardTargetFiltters[x].CheckTarget(cardTargets[i]);

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

namespace MikzeerGameNotas
{
    //public class UseCardAbility : AbilityAction
    //{
    //    private const int specificID = 8;
    //    private int amount = 0;
    //    private Player player;
    //    Card card;
    //    IUseCardCommnad useCardCommand;

    //    ICardTarget cardTarget;

    //    public List<Tile2D> posibleTargetTiles { get; protected set; }
    //    public Tile2D selectedTargetTile;


    //    public UseCardAbility(Player player) : base(specificID)
    //    {
    //        this.player = player;
    //        SetActionPointsRequired(amount);
    //        SetPerformerAPActor(player);
    //    }

    //    public void Set(Card card)
    //    {
    //        this.card = card;
    //    }

    //    public void SetTarget(ICardTarget cardTarget)
    //    {
    //        this.cardTarget = cardTarget;
    //    }

    //    public override void OnResetActionExecution()
    //    {

    //    }

    //    public override bool OnTryExecute()
    //    {
    //        // 1- QUE LA CARTA TENGA UN TARGET
    //        if (!card.DoIHaveTarget())
    //        {
    //            return false;
    //        }

    //        return true;
    //    }

    //    public override void StartActionModifierCheck()
    //    {

    //    }

    //    public override void OnStartExecute()
    //    {

    //    }

    //    public override void Execute()
    //    {
    //        if (actionStatus == ACTIONEXECUTIONSTATUS.CANCELED)
    //        {
    //            Debug.Log("UseCardAbility CANCEL");
    //            return;
    //        }

    //        useCardCommand = new IUseCardCommnad(player, card);

    //        Invoker.AddNewCommand(useCardCommand);
    //        Invoker.ExecuteCommands(null);

    //        //actionStatus = ACTIONEXECUTIONSTATUS.STARTED;
    //        //dummy.StartCoroutine(CheckExecution());
    //    }

    //    public override void EndActionModifierCheck()
    //    {

    //    }

    //    public override void OnEndExecute()
    //    {

    //    }

    //    public IEnumerator CheckExecution()
    //    {
    //        bool hasEnd = false;
    //        do
    //        {
    //            if (useCardCommand.hasFinish)
    //            {
    //                //Debug.Log("Ya termine de Spawnear segunda capa");
    //                actionStatus = ACTIONEXECUTIONSTATUS.EXECUTED;
    //                hasEnd = true;
    //                yield break;
    //            }

    //            //Debug.Log("Todavia no termine");
    //            yield return new WaitForSeconds(0.15f);
    //        } while (!hasEnd);

    //        //Debug.Log("Termine de Spawnear");

    //    }

    //}
}