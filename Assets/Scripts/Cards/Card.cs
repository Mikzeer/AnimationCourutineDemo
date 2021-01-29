using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Card
    {
        public int IDInGame { get; protected set; } // LA ID DE LA CARTA CREADA EN EL JUEGO
        public Player ownerPlayer { get; protected set; }
        public CardData CardData { get; protected set; } // TIENE UNA ID UNICA ESPECIFICA DE CADA CARD
        public bool RequireSelectTarget { get; protected set; }

        // LA ID QUE VAMOS A USAR PARA REFERENCIARLAS VIA REFLECTION....
        public int ID { get; protected set; }

        public Card(int IDInGame, Player ownerPlayer, CardData CardData)
        {
            this.IDInGame = IDInGame;
            this.ownerPlayer = ownerPlayer;
            this.CardData = CardData;
        }

        public Card(int ID)
        {
            this.ID = ID;
        }

        public virtual void InitializeCard(int IDInGame, Player ownerPlayer, CardData CardData)
        {
            this.IDInGame = IDInGame;
            this.ownerPlayer = ownerPlayer;
            this.CardData = CardData;
        }
        
    }

    public class BuffUnitAttackLevelOne : Card
    {
        private const int AID = 1;
        public BuffUnitAttackLevelOne() : base(AID)
        {

        }

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

            CardData.cardEffects = new List<CardEffect>();

            CardData.cardEffects.Add(statModificationEffect);
        }

        public override void InitializeCard(int IDInGame, Player ownerPlayer, CardData CardData)
        {
            base.InitializeCard(IDInGame, ownerPlayer, CardData);
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

            CardData.cardEffects = new List<CardEffect>();

            CardData.cardEffects.Add(statModificationEffect);
        }
    }

    public class NerfUnitAttackLevelOne : Card
    {
        private const int AID = 8;
        public NerfUnitAttackLevelOne() : base(AID)
        {

        }

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

        public override void InitializeCard(int IDInGame, Player ownerPlayer, CardData CardData)
        {
            base.InitializeCard(IDInGame, ownerPlayer, CardData);

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

            CardData.cardEffects = new List<CardEffect>();

            CardData.cardEffects.Add(statModificationEffect);
        }
    }

    public class HealUnitLevelOne : Card
    {
        private const int AID = 4;
        public HealUnitLevelOne() : base(AID)
        {

        }

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

        public override void InitializeCard(int IDInGame, Player ownerPlayer, CardData CardData)
        {
            base.InitializeCard(IDInGame, ownerPlayer, CardData);

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

            CardData.cardEffects = new List<CardEffect>();

            CardData.cardEffects.Add(statModificationEffect);
        }
    }
}

namespace PositionerDemo
{
    //public class HandGrandeLevelOne : Card
    //{
    //    public HandGrandeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE ESTE EN EL BATTLEFIELD
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();



    //        // EFFECTS


    //    }
    //}
    //public class BuffMoveRangeLevelOne : Card
    //{
    //    public BuffMoveRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetMoveRangeStatIDFiltter();

    //        // VERIFICAMOS QUE EL MOVIMIENTO ACTUAL/MAXIMO NO SEA MAYOR A:
    //        // MAXIMO 3 PTS * UNIDAD
    //        int amountGreaterOrEqual = 3;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


    //    }
    //}
    //public class BlockerLevelOne : Card
    //{
    //    public BlockerLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTER QUE SEA DE TIPO TILE EN BATTLEFILED Y QUE ESTE VACIA
    //        CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();
    //        CardFiltter tileOcuppyStateFiltter = new TargetEmptyOcuppyStateFiltter();

    //    }
    //}
    //public class ShieldLevelOne : Card
    //{
    //    public ShieldLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();


    //    }
    //}
    //public class BuffAttackRangeLevelOne : Card
    //{
    //    public BuffAttackRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetAttackRangeIDFiltter();

    //        // VERIFICAMOS QUE EL ATTACK RANGE ACTUAL/MAXIMO NO SEA MAYOR A:
    //        // MAXIMO 3 PTS * UNIDAD
    //        int amountGreaterOrEqual = 3;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackRangeStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


    //    }
    //}
    //public class BuffUnitAttackLevelTwo : Card
    //{
    //    public BuffUnitAttackLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();
    //        // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
    //    }
    //}
    //public class NerfUnitAttackLevelTwo : Card
    //{
    //    public NerfUnitAttackLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();

    //        // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
    //        int amountLessThan = 0;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

    //        // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
    //    }
    //}
    //public class HealUnitLevelTwo : Card
    //{
    //    public HealUnitLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

    //        // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
    //        int statID = 0;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
    //        StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

    //    }
    //}
    //public class HandGrandeLevelTwo : Card
    //{
    //    public HandGrandeLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE ESTE EN EL BATTLEFIELD
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();



    //        // EFFECTS


    //    }
    //}
    //public class BuffMoveRangeLevelTwo : Card
    //{
    //    public BuffMoveRangeLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetMoveRangeStatIDFiltter();

    //        // VERIFICAMOS QUE EL MOVIMIENTO ACTUAL/MAXIMO NO SEA MAYOR A:
    //        // MAXIMO 3 PTS * UNIDAD
    //        int amountGreaterOrEqual = 3;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackPowerStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


    //    }
    //}
    //public class BlockerLevelTwo : Card
    //{
    //    public BlockerLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTER QUE SEA DE TIPO TILE EN BATTLEFILED Y QUE ESTE VACIA
    //        CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();
    //        CardFiltter tileOcuppyStateFiltter = new TargetEmptyOcuppyStateFiltter();

    //    }
    //}
    //public class BuffAttackRangeLevelTwo : Card
    //{
    //    public BuffAttackRangeLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetAttackRangeIDFiltter();

    //        // VERIFICAMOS QUE EL ATTACK RANGE ACTUAL/MAXIMO NO SEA MAYOR A:
    //        // MAXIMO 3 PTS * UNIDAD
    //        int amountGreaterOrEqual = 3;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackRangeStatAmountAgainstSimplFiltter(amountType, amountGreaterOrEqual, comparationType);


    //    }
    //}
    //public class NerfUnitAttackRangeLevelOne : Card
    //{
    //    public NerfUnitAttackRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetAttackRangeIDFiltter();

    //        // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
    //        int amountLessThan = 0;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetAttackRangeStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

    //        // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
    //    }
    //}
    //public class NerfUnitMoveRangeLevelOne : Card
    //{
    //    public NerfUnitMoveRangeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetMoveRangeStatIDFiltter();

    //        // VERIFICAMOS QUE EL ATAQUE ACTUAL NO SEA MENOR A 0
    //        int amountLessThan = 0;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.GREATER;
    //        CardFiltter targetAttackPowStatAgainstSimple = new TargetMoveRangeStatAmountAgainstSimplFiltter(amountType, amountLessThan, comparationType);

    //        // EFFECTS BUFF ATTACK ACTUALSTAT/MAXSTAT + 1
    //    }
    //}
    //public class NoCardLevelOne : Card
    //{
    //    public NoCardLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS TARGET SEA PLAYER, Y TENGA LA TAKE CARD ABILITY
    //        CardFiltter targetTypeFiltter = new TargetPlayerOccupierTypeFiltter();
    //        CardFiltter abilityIDFiltter = new TargetTakeCardAbilityIDFiltter();

    //        //  QUE TENGA UN DECK Y TENGA AL MENOS 1 CARTA EN EL
    //        CardFiltter cardStateFiltter = new TargetAmountCardStateFiltterAgianstSimple(CARDSTATES.DECK, 1, COMPARATIONTYPE.GREATEROREQUAL);
    //    }
    //}
    //public class NoSpawnLevelOne : Card
    //{
    //    public NoSpawnLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS TARGET SEA PLAYER, Y TENGA LA TAKE CARD ABILITY
    //        CardFiltter targetTypeFiltter = new TargetPlayerOccupierTypeFiltter();
    //        CardFiltter abilityIDFiltter = new TargetSpawnAbilityIDFiltter();


    //    }
    //}
    //public class DefenseModeOffLevelOne : Card
    //{
    //    public DefenseModeOffLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // QUE SEA UNIT Y QUE TENGA LA HABILIDAD DE TAKE DAMAGE
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter abilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
    //        // QUE TENGA LA ABILITY MODIFIER CON EL ID QUE NECESITO DEFENSA ID = 0;
    //        int abTakeDamageID = 6;
    //        int abModDefenseID = 0;
    //        CardFiltter abilityModifierIDFiltter = new TargetAbilityModifierIDFiltter(abTakeDamageID, abModDefenseID);

    //    }
    //}
    //public class TotalHealUnitLevelOne : Card
    //{
    //    public TotalHealUnitLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TARGET UNIT
    //        // FILTTERS QUE SEA DE TIPO UNIT Y QUE TENGA UN STAT DETERMINADO
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

    //        // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
    //        int statID = 0;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
    //        StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

    //    }
    //}
    //public class RangeGrandeLevelOne : Card
    //{
    //    public RangeGrandeLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // ACA DIVIDIMOS EN DOS, PRIMERO ES EL TARGET DE SELECCION, QUE LO UNICO QUE NECESITAMOS ES
    //        // FILTTER QUE SEA UNA TILE Y QUE ESTE EN EL BATTLEFIELD
    //        CardFiltter tileTypeFiltter = new TargetBattlefieldTileTypeFiltter();




    //        // EFFECTS


    //        // EFFECTS FILTTERS 
    //        // QUE SEA DE TIPO UNIT O GAMEOBJECT
    //        // ACA NO HACE FALTA VER SI ES EL CAMPO DE BATALLA YA QUE ES EL EFECTO EL QUE VAMOS A APLICAR QUE ES EL DANO EN ESAS UNIDADES
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetGameObjectType = new TargetGameObjectOccupierTypeFiltter();
    //        // FILTTER PARA EL TIPO DE HABILIDAD TAKEDAMAGE
    //        CardFiltter abilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
    //    }
    //}
    //public class AddCardLevelOne : Card
    //{
    //    public AddCardLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS TARGET SEA PLAYER, Y TENGA LA TAKE CARD ABILITY
    //        CardFiltter targetTypeFiltter = new TargetPlayerOccupierTypeFiltter();
    //        CardFiltter abilityIDFiltter = new TargetTakeCardAbilityIDFiltter();

    //        //  QUE TENGA UN DECK Y TENGA AL MENOS 1 CARTA EN EL
    //        CardFiltter cardStateFiltter = new TargetAmountCardStateFiltterAgianstSimple(CARDSTATES.DECK, 1, COMPARATIONTYPE.GREATEROREQUAL);
    //    }
    //}
    //public class TotalShield : Card
    //{
    //    public TotalShield(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
    //    }
    //}
    //public class TotlaHealLevelTwo : Card
    //{
    //    public TotlaHealLevelTwo(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // PODRIA CHEQUEAR QUE SI VIDA A FULL Y NINGUN STAT MODIFIER DE ALGUN TIPO ENTONCES NADA MEDIO AL PEDO APLICAR ALGO EN ESA UNIDAD
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter statIDFiltter = new TargetHealthStatIDFiltter();

    //        // VERIFICAMOS QUE LA VIDA ACTUAL NO SEA MAYOR A LA VIDA MAXIMA PARA PODER CURARLO
    //        int statID = 0;
    //        STATAMOUNTTYPE amountType = STATAMOUNTTYPE.ACTUAL;
    //        STATAMOUNTTYPE amountTypeToCheck = STATAMOUNTTYPE.MAX;
    //        StatIResultData statDataToCheckAgainst = new StatIResultData(statID, amountTypeToCheck);
    //        COMPARATIONTYPE comparationType = COMPARATIONTYPE.LESS;
    //        CardFiltter targetHealtStatAmountFiltter = new TargetHealtStatAmountFiltter(amountType, statDataToCheckAgainst, comparationType);

    //        // VERIFICAMOS SI TIENE ALGUN NERF EN SUS STATS APLICADOS
    //        CardFiltter targetAbilityModifierTypeFiltter = new TargetStatModifierTypeFiltter(STATMODIFIERTYPE.NERF);
    //    }
    //}
    //public class TotalDebufferLevelOne : Card
    //{
    //    public TotalDebufferLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // CHEQUEAMOS SI SON UNIDADES
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        // VEMOS SI SON ENEMIGOS
    //        CardFiltter targetTeamFiltter = new TargetEnemyTeamFiltter();
    //        // VERIFICAMOS SI TIENE ALGUN BUFF EN SUS STATS APLICADOS
    //        CardFiltter targetAbilityModifierTypeFiltter = new TargetStatModifierTypeFiltter(STATMODIFIERTYPE.BUFF);

    //    }
    //}
    //public class BackOffLevelOne : Card
    //{
    //    Tile[,] GridArray;
    //    public BackOffLevelOne(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTER QUE SEA DE TIPO UNIDAD
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();

    //        // TENEMOS QUE VERIFICAR SI HAY UN ESPACIO LIBRE DETAS DE LA UNIDAD.....
    //        // CON EL TARGET NO VA A HABER PROBLEMA... PERO... DE DONDE DECIMOS... ESTE ES EL Board o la lista de Tiles para que chequeesPosicion
    //        // PODRIAMOS TENER UN CARD MANAGER QUE VA A TENER QUE TENER UNA REFERENCIA A LA  Tile[,] GridArray del Board y pasarselo en el constructor
    //        CardFiltter targetFresSpaceFiltter = new TargetFreeSpaceBehindFiltter(GridArray);

    //    }

    //    public void SetBoard(Tile[,] GridArray)
    //    {
    //        this.GridArray = GridArray;
    //    }
    //}
    //public class HalfDamage : Card
    //{
    //    public HalfDamage(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
    //    }
    //}
    //public class NoAttack : Card
    //{
    //    public NoAttack(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetAttackAbilityIDFiltter();

    //        // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
    //        int abilityID = 5;
    //        ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
    //        ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
    //        List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
    //        exeAction.Add(actionStatusOne);
    //        exeAction.Add(actionStatusTwo);
    //        CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
    //    }
    //}
    //public class NoMove : Card
    //{
    //    public NoMove(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetMoveAbilityIDFiltter();

    //        // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
    //        int abilityID = 4;
    //        ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
    //        ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
    //        List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
    //        exeAction.Add(actionStatusOne);
    //        exeAction.Add(actionStatusTwo);
    //        CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
    //    }
    //}
    //public class NoDefense : Card
    //{
    //    public NoDefense(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetDefendAbilityIDFiltter();

    //        // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
    //        int abilityID = 8;
    //        ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
    //        ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
    //        List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
    //        exeAction.Add(actionStatusOne);
    //        exeAction.Add(actionStatusTwo);
    //        CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
    //    }
    //}
    //public class NoAttackOnBase : Card
    //{
    //    public NoAttackOnBase(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetAttackAbilityIDFiltter();

    //        // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
    //        int abilityID = 5;
    //        ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
    //        ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
    //        List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
    //        exeAction.Add(actionStatusOne);
    //        exeAction.Add(actionStatusTwo);
    //        CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
    //    }
    //}
    //public class NoCombine : Card
    //{
    //    public NoCombine(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetCombineAbilityIDFiltter();

    //        // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
    //        int abilityID = 9;
    //        ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
    //        ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
    //        List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
    //        exeAction.Add(actionStatusOne);
    //        exeAction.Add(actionStatusTwo);
    //        CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
    //    }
    //}
    //public class NoEvolve : Card
    //{
    //    public NoEvolve(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // FILTTERS QUE SEA UNIDAD Y QUE TENGA LA TAKE DAMAGE ABILITY PARA PODER PONERLE EL BUFF
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetEvolveAbilityIDFiltter();

    //        // SE PODRIA CHEQUEAR SI LA ACCION ESTE EN STARTED O WAIT, YA QUE SI SE EJECUTO O CANCELO ES MEDIO AL PEDO APLICAR ESTO
    //        int abilityID = 11;
    //        ABILITYEXECUTIONSTATUS actionStatusOne = ABILITYEXECUTIONSTATUS.STARTED;
    //        ABILITYEXECUTIONSTATUS actionStatusTwo = ABILITYEXECUTIONSTATUS.WAIT;
    //        List<ABILITYEXECUTIONSTATUS> exeAction = new List<ABILITYEXECUTIONSTATUS>();
    //        exeAction.Add(actionStatusOne);
    //        exeAction.Add(actionStatusTwo);
    //        CardFiltter targetAbilityExecutionFiltter = new TargetAbilityExecutionStateFiltter(abilityID, exeAction);
    //    }
    //}
    //public class OnlyBuff : Card
    //{
    //    public OnlyBuff(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // EN SI NO VOY A CHEQUEAR NADA, EL JUGADOR PODRIA O NO TENER UN BUFF O NERF, ASI QUE ESO LO VOY A APLICAR
    //        // UNA VEZ QUE SE APLIQUE EL EFECTO EN SI, ES UN ABILITY MODIFIER DEL USE CARD,
    //    }
    //}
    //public class OnlyNerf : Card
    //{
    //    public OnlyNerf(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // EN SI NO VOY A CHEQUEAR NADA, EL JUGADOR PODRIA O NO TENER UN BUFF O NERF, ASI QUE ESO LO VOY A APLICAR
    //        // UNA VEZ QUE SE USE O NO UNA CARTA
    //    }
    //}
    //public class OnlyNeutral : Card
    //{
    //    public OnlyNeutral(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // EN SI NO VOY A CHEQUEAR NADA, EL JUGADOR PODRIA O NO TENER UN BUFF O NERF, ASI QUE ESO LO VOY A APLICAR
    //        // UNA VEZ QUE SE USE O NO UNA CARTA
    //    }
    //}
    //public class ChangePosition : Card
    //{
    //    public ChangePosition(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // TENEMOS QUE VERIFICAR QUE ESTE COMBINADA
    //        CardFiltter targetOccupierTypeFiltter = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter unityTypeFiltter = new TargetUnitTypeFiltter(UNITTYPE.COMBINE);
    //        // QUE TENGA EL TAKE DAMAGE ABILITY Y EL DECOMBINE ABILITY
    //        CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
    //        CardFiltter targetAbilityIDFiltterTwo = new TargetDecombineAbilityIDFiltter();
    //    }
    //}
    //public class PlusAttackPerDamage : Card
    //{
    //    public PlusAttackPerDamage(int ID, Player ownerPlayer, CardData CardData) : base(ID, ownerPlayer, CardData)
    //    {
    //        // VERIFICAR QUE TENGA EL TAKE DAMAGE ABILITY Y EL ATACK POWER STAT
    //        CardFiltter targetOccupierType = new TargetUnitOccupierTypeFiltter();
    //        CardFiltter targetAbilityIDFiltter = new TargetTakeDamageAbilityIDFiltter();
    //        CardFiltter statIDFiltter = new TargetAttackPowerIDFiltter();
    //    }
    //}
}