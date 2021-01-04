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

    public class BuffUnitAttackLevelOne : Card
    {
        public BuffUnitAttackLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();

            CardData.cardTargetFiltters.Add(targetOccupierType);
            CardData.cardTargetFiltters.Add(statIDFiltter);

            // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
            int statID = 2;
            int amount = 1;
            CardEffect statModificationEffect = new BuffStatModificationEffect(statID, amount);

            CardData.cardEffects.Add(statModificationEffect);
        }
    }

    public class NerfUnitAttackLevelOne : Card
    {
        public NerfUnitAttackLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();

            // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
            int amountLessThan = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

            CardData.cardTargetFiltters.Add(targetOccupierType);
            CardData.cardTargetFiltters.Add(statIDFiltter);
            CardData.cardTargetFiltters.Add(targetAttackPowStatAgainstSimple);


            // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT - 1
            int statID = 2;
            int amount = 1;
            CardEffect statModificationEffect = new NerfStatModificationEffect(statID, amount);

            CardData.cardEffects.Add(statModificationEffect);
        }
    }

    public class HealUnitLevelOne : Card
    {
        public HealUnitLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

            // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
            int statID = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
            StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

            CardData.cardTargetFiltters.Add(targetOccupierType);
            CardData.cardTargetFiltters.Add(statIDFiltter);
            CardData.cardTargetFiltters.Add(targetHealtStatAmountFiltter);


            // EFFECT HP + 1
            int hpStatID = 0;
            int amount = 1;
            CardEffect statModificationEffect = new BuffStatModificationEffect(hpStatID, amount);

            CardData.cardEffects.Add(statModificationEffect);
        }
    }

    public class CardTargetManager
    {
        List<Tile> boardTiles;
        public CardTargetManager(Board2D board2D)
        {
            // RECORREMOS LA LISTA DE TILES
            for (int x = 0; x < board2D.GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < board2D.GridArray.GetLength(1); y++)
                {
                    Tile actualTile = board2D.GetGridObject(x, y);
                    boardTiles.Add(actualTile);
                }
            }
        }

        public List<ICardTarget> GetPosibleTargets(Card card)
        {
            List<ICardTarget> foundTargets = new List<ICardTarget>();

            // RECORREMOS LA LISTA DE POSIBLES TARGET DE LA CARD
            for (int i = 0; i < card.CardData.cardTargetTypes.Count; i++)
            {
                // RECORREMOS LA LISTA DE TILES
                for (int z = 0; z < boardTiles.Count; z++)
                {
                    if (boardTiles[z].IsOccupied())
                    {
                        if (boardTiles[z].GetOccupier().CardTargetType == card.CardData.cardTargetTypes[i])
                        {
                            if (foundTargets.Contains(boardTiles[z].GetOccupier()) == false)
                            {
                                foundTargets.Add(boardTiles[z].GetOccupier());
                            }
                        }
                    }
                    else
                    {
                        if (boardTiles[z].CardTargetType == card.CardData.cardTargetTypes[i])
                        {
                            if (foundTargets.Contains(boardTiles[z]) == false)
                            {
                                foundTargets.Add(boardTiles[z]);
                            }
                        }
                    }
                }
            }
            return foundTargets;
        }
    }

    public class CardFiltterManager
    {
        public List<ICardTarget> FilterTargets(List<ICardTarget> cardTargets, Card card)
        {
            List<ICardTarget> filtterTargets = new List<ICardTarget>();

            // RECORREMOS LOS TARGETS POSIBLES
            for (int i = 0; i < cardTargets.Count; i++)
            {
                bool allPass = true;

                // APLICAMOS CADA FILTRO QUE TENGA LA CARTA
                for (int x = 0; x < card.CardData.cardTargetFiltters.Count; x++)
                {
                    ICardTarget cardTarget = card.CardData.cardTargetFiltters[x].CheckTarget(cardTargets[i]);

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
    }

    public class CardTargetFiltterManager
    {
        TurnManager turnManager;
        CardTargetManager cardTargetManager;
        CardFiltterManager cardFiltterManager;

        public CardTargetFiltterManager(TurnManager turnManager, Board2D board2D)
        {
            this.turnManager = turnManager;
            cardTargetManager = new CardTargetManager(board2D);
            cardFiltterManager = new CardFiltterManager();
        }

        public List<ICardTarget> OnTryGetFiltterTargets(Card card)
        {
            // CHEQUEAR DE QUIEN ES EL TURNO
            // SI NO ES NUESTRO TURNO ENTONCES SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (card.ownerPlayer != turnManager.GetActualPlayerTurn()) return null;

            List<ICardTarget> foundTargets = new List<ICardTarget>();

            foundTargets = cardTargetManager.GetPosibleTargets(card);

            // SI LA LISTA ES IGUAL A 0 SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (foundTargets.Count == 0) return null;

            foundTargets = cardFiltterManager.FilterTargets(foundTargets, card);

            // SI LA LISTA ES IGUAL A 0 SOLO MOVEMOS LA CARTA PARA ACOMODARLA ASI QUE ACA NO PASA NADA
            if (foundTargets.Count == 0) return null;

            return foundTargets;
        }
    }

    public class CardEffectManager
    {
        public void ApplyCardEfect(List<ICardTarget> cardTargets, Card card)
        {
            for (int i = 0; i < cardTargets.Count; i++)
            {
                for (int j = 0; j < card.CardData.cardEffects.Count; j++)
                {
                    card.CardData.cardEffects[j].OnCardEffectApply(cardTargets[i]);
                }
            }
        }
    }

    public enum CARDSTATES
    {
        DECK,
        HAND,
        CEMENTERY,
        WAITFORUSE,
        WAITFORUSEWITHTARGET
    }
}

namespace PositionerDemo
{
    public class HandGrandeLevelOne : Card
    {
        public HandGrandeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE ESTE EN EL BATTLEFIELD
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();



            // EFFECTS


        }
    }
    public class BuffMoveRangeLevelOne : Card
    {
        public BuffMoveRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetMoveRangeStatIDFiltter();

            // VERIFICAMOS QUE EL MOVIMIENTO ACTUAL/MAXIMO NO SEA MAYOR A:
            // MAXIMO 3 PTS * UNIDAD
            int amountGreaterOrEqual = 3;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


        }
    }
    public class BlockerLevelOne : Card
    {
        public BlockerLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTER QUE SEA DE TIPO TILE EN BATTLEFILED Y QUE ESTE VACIA
            CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();
            CardFiltter tileOcuppyStateFiltter = new TargetEmptyOcuppyStateFiltter();

        }
    }
    public class ShieldLevelOne : Card
    {
        public ShieldLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();


        }
    }
    public class BuffAttackRangeLevelOne : Card
    {
        public BuffAttackRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackRangeIDFiltter();

            // VERIFICAMOS QUE EL ATTACK RANGE ACTUAL/MAXIMO NO SEA MAYOR A:
            // MAXIMO 3 PTS * UNIDAD
            int amountGreaterOrEqual = 3;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackRangeStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


        }
    }
    public class BuffUnitAttackLevelTwo : Card
    {
        public BuffUnitAttackLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();
            // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
        }
    }
    public class NerfUnitAttackLevelTwo : Card
    {
        public NerfUnitAttackLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();

            // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
            int amountLessThan = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

            // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
        }
    }
    public class HealUnitLevelTwo : Card
    {
        public HealUnitLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

            // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
            int statID = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
            StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

        }
    }
    public class HandGrandeLevelTwo : Card
    {
        public HandGrandeLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE ESTE EN EL BATTLEFIELD
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();



            // EFFECTS


        }
    }
    public class BuffMoveRangeLevelTwo : Card
    {
        public BuffMoveRangeLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetMoveRangeStatIDFiltter();

            // VERIFICAMOS QUE EL MOVIMIENTO ACTUAL/MAXIMO NO SEA MAYOR A:
            // MAXIMO 3 PTS * UNIDAD
            int amountGreaterOrEqual = 3;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


        }
    }
    public class BlockerLevelTwo : Card
    {
        public BlockerLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTER QUE SEA DE TIPO TILE EN BATTLEFILED Y QUE ESTE VACIA
            CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();
            CardFiltter tileOcuppyStateFiltter = new TargetEmptyOcuppyStateFiltter();

        }
    }
    public class BuffAttackRangeLevelTwo : Card
    {
        public BuffAttackRangeLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackRangeIDFiltter();

            // VERIFICAMOS QUE EL ATTACK RANGE ACTUAL/MAXIMO NO SEA MAYOR A:
            // MAXIMO 3 PTS * UNIDAD
            int amountGreaterOrEqual = 3;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackRangeStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


        }
    }
    public class NerfUnitAttackRangeLevelOne : Card
    {
        public NerfUnitAttackRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetAttackRangeIDFiltter();

            // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
            int amountLessThan = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackRangeStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

            // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
        }
    }
    public class NerfUnitMoveRangeLevelOne : Card
    {
        public NerfUnitMoveRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetMoveRangeStatIDFiltter();

            // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
            int amountLessThan = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
            CardFiltter targetAttackPowStatAgainstSimple = new TargetMoveRangeStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

            // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
        }
    }
    public class NoCardLevelOne : Card
    {
        public NoCardLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS TARGET SEA PLAYER, Y TENGA LA TAKE CARD ABILITY
            CardFiltter targetTypeFiltter = new TargetPlayerOccupierTypeFiltter();
            CardFiltter abilityIDFiltter = new TargetTakeCardAbilityIDFiltter();

            //  QUE TENGA UN DECK Y TENGA AL MENOS 1 CARTA EN EL
            CardFiltter cardStateFiltter = new TargetAmountCardStateFiltterAgianstSimple(CARDSTATES.DECK, 1, COMPARATIONTYPE.GREATEROREQUAL);
        }
    }
    public class NoSpawnLevelOne : Card
    {
        public NoSpawnLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS TARGET SEA PLAYER, Y TENGA LA TAKE CARD ABILITY
            CardFiltter targetTypeFiltter = new TargetPlayerOccupierTypeFiltter();
            CardFiltter abilityIDFiltter = new TargetSpawnAbilityIDFiltter();


        }
    }
    public class DefenseModeOffLevelOne : Card
    {
        public DefenseModeOffLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // QUE SEA UNIT Y QUE TENGA LA HABILIDAD DE TAKE DAMAGE
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter abilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
            // QUE TENGA LA ABILITY MODIFIER CON EL ID QUE NECESITO DEFENSA ID = 0;
            int abTakeDamageID = 6;
            int abModDefenseID = 0;
            CardFiltter abilityModifierIDFiltter = new TargetAbilityModifierIDFiltter(abTakeDamageID, abModDefenseID);

        }
    }
    public class TotalHealUnitLevelOne : Card
    {
        public TotalHealUnitLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TARGET UNIT
            // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

            // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
            int statID = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
            StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

        }
    }
    public class RangeGrandeLevelOne : Card
    {
        public RangeGrandeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // ACA DIVIDIMOS EN DOS, PRIMERO ES EL TARGET DE SELECCION, QUE LO UNICO QUE NECESITAMOS ES
            // FILTTER QUE SEA UNA TILE Y QUE ESTE EN EL BATTLEFIELD
            CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();




            // EFFECTS


            // EFFECTS FILTTERS 
            // QUE SEA DE TIPO UNIT O GAMEOBJECT
            // ACA NO HACE FALTA VER SI ES EL CAMPO DE BATALLA YA QUE ES EL EFECTO EL QUE VAMOS A APLICAR QUE ES EL DANO EN ESAS UNIDADES
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetGameObjectType = new TargetGameObjectOccupierTypeFiltter();
            // FILTTER PARA EL TIPO DE HABILIDAD TAKEDAMAGE
            CardFiltter abilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
        }
    }
    public class AddCardLevelOne : Card
    {
        public AddCardLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS TARGET SEA PLAYER, Y TENGA LA TAKE CARD ABILITY
            CardFiltter targetTypeFiltter = new TargetPlayerOccupierTypeFiltter();
            CardFiltter abilityIDFiltter = new TargetTakeCardAbilityIDFiltter();

            //  QUE TENGA UN DECK Y TENGA AL MENOS 1 CARTA EN EL
            CardFiltter cardStateFiltter = new TargetAmountCardStateFiltterAgianstSimple(CARDSTATES.DECK, 1, COMPARATIONTYPE.GREATEROREQUAL);
        }
    }
    public class TotalShield : Card
    {
        public TotalShield(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
        }
    }
    public class TotlaHealLevelTwo : Card
    {
        public TotlaHealLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // PODRIA CHEQUEAR QUE SI VIDA A FULL Y NINGUN STAT MODIFIER DE ALGUN TIPO ENTONCES NADA MEDIO AL PEDO APLICAR ALGO EN ESA UNIDAD
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

            // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
            int statID = 0;
            STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
            STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
            StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
            COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
            CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

            // VERIFICAMOS SI TIENE ALGUN NERF EN SUS STATS APLICADOS
            CardFiltter targetAbilityModifierTypeFiltter = new TargetStatModifierTypeFiltter(STATMODIFIERTYPE.NERF);
        }
    }
    public class TotalDebufferLevelOne : Card
    {
        public TotalDebufferLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // CHEQUEAMOS SI SON UNIDADES
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            // VEMOS SI SON ENEMIGOS
            CardFiltter targetTeamFiltter = new TargetEnemyTeamFiltter();
            // VERIFICAMOS SI TIENE ALGUN BUFF EN SUS STATS APLICADOS
            CardFiltter targetAbilityModifierTypeFiltter = new TargetStatModifierTypeFiltter(STATMODIFIERTYPE.BUFF);

        }
    }
    public class BackOffLevelOne : Card
    {
        Tile[,] GridArray;
        public BackOffLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTER QUE SEA DE TIPO UNIDAD
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();

            // TENEMOS QUE VERIFICAR SI HAY UN ESPACIO LIBRE DETAS DE LA UNIDAD.....
            // CON EL TARGET NO VA A HABER PROBLEMA... PERO... DE DONDE DECIMOS... ESTE ES EL Board o la lista de Tiles para que chequeesPosicion
            // PODRIAMOS TENER UN CARD MANAGER QUE VA A TENER QUE TENER UNA REFERENCIA A LA  Tile[,] GridArray del Board y pasarselo en el constructor
            CardFiltter targetFresSpaceFiltter = new TargetFreeSpaceBehindFiltter(GridArray);

        }

        public void SetBoard(Tile[,] GridArray)
        {
            this.GridArray = GridArray;
        }
    }
    public class HalfDamage : Card
    {
        public HalfDamage(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
        }
    }
    public class NoAttack : Card
    {
        public NoAttack(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetAttackAbilityIDFiltter();

            // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
            int abilityID = 5;
            ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
            ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
            List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
            exeAction.Add(actionStatusOne);
            exeAction.Add(actionStatusTwo);
            CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
        }
    }
    public class NoMove : Card
    {
        public NoMove(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetMoveAbilityIDFiltter();

            // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
            int abilityID = 4;
            ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
            ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
            List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
            exeAction.Add(actionStatusOne);
            exeAction.Add(actionStatusTwo);
            CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
        }
    }
    public class NoDefense : Card
    {
        public NoDefense(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetDefendAbilityIDFiltter();

            // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
            int abilityID = 8;
            ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
            ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
            List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
            exeAction.Add(actionStatusOne);
            exeAction.Add(actionStatusTwo);
            CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
        }
    }
    public class NoAttackOnBase : Card
    {
        public NoAttackOnBase(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetAttackAbilityIDFiltter();

            // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
            int abilityID = 5;
            ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
            ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
            List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
            exeAction.Add(actionStatusOne);
            exeAction.Add(actionStatusTwo);
            CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
        }
    }
    public class NoCombine : Card
    {
        public NoCombine(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetCombineAbilityIDFiltter();

            // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
            int abilityID = 9;
            ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
            ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
            List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
            exeAction.Add(actionStatusOne);
            exeAction.Add(actionStatusTwo);
            CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
        }
    }
    public class NoEvolve : Card
    {
        public NoEvolve(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetEvolveAbilityIDFiltter();

            // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
            int abilityID = 11;
            ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
            ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
            List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
            exeAction.Add(actionStatusOne);
            exeAction.Add(actionStatusTwo);
            CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
        }
    }
    public class OnlyBuff : Card
    {
        public OnlyBuff(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // EN SI NO VOY A CHEQUEAR NADA, EL JUGADOR PODRIA O NO TENER UN BUFF O NERF, ASI QUE ESO LO VOY A APLICAR
            // UNA VEZ QUE SE APLIQUE EL EFECTO EN SI, ES UN ABILITY MODIFIER DEL USE CARD,
        }
    }
    public class OnlyNerf : Card
    {
        public OnlyNerf(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // EN SI NO VOY A CHEQUEAR NADA, EL JUGADOR PODRIA O NO TENER UN BUFF O NERF, ASI QUE ESO LO VOY A APLICAR
            // UNA VEZ QUE SE USE O NO UNA CARTA
        }
    }
    public class OnlyNeutral : Card
    {
        public OnlyNeutral(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // EN SI NO VOY A CHEQUEAR NADA, EL JUGADOR PODRIA O NO TENER UN BUFF O NERF, ASI QUE ESO LO VOY A APLICAR
            // UNA VEZ QUE SE USE O NO UNA CARTA
        }
    }
    public class ChangePosition : Card
    {
        public ChangePosition(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // TENEMOS QUE VERIFICAR QUE ESTE COMBINADA
            CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
            CardFiltter unityTypeFiltter = new TargetUnitTypeFiltter(UNITTYPE.COMBINE);
            // QUE TENGA EL TAKE DAMAGE ABILITY Y EL DECOMBINE ABILITY
            CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
            CardFiltter targetAbilityIDFiltterTwo = new TargetDecombineAbilityIDFiltter();
        }
    }
    public class PlusAttackPerDamage : Card
    {
        public PlusAttackPerDamage(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
        {
            // VERIFICAR QUE TENGA EL TAKE DAMAGE ABILITY Y EL ATACK POWER STAT
            CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
            CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
            CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();
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