using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    [System.Serializable]
    public class CardFiltter
    {
        public int ID { get; protected set; }

        public CardFiltter(int ID)
        {
            this.ID = ID;
        }

        public virtual ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            return cardTarget;
        }
    }

}

namespace MikzeerGame
{
    using PositionerDemo;
    using System.Collections.Generic;

    [System.Serializable]
    public class CardFiltter
    {
        public int ID { get; protected set; }
        public List<CARDTARGETTYPE> targetRequired { get; protected set; }

        public CardFiltter(int ID, List<CARDTARGETTYPE> targetRequired)
        {
            this.ID = ID;
            this.targetRequired = targetRequired;
        }

        public virtual ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            if (targetRequired == null) return cardTarget;
            // CHECKEAMOS SI ES UN TARGET DE LA LISTA DE TARGETS POSIBLES
            bool isTarget = false;
            for (int i = 0; i < targetRequired.Count; i++)
            {
                if (cardTarget.CardTargetType == targetRequired[i])
                {
                    isTarget = true;
                    break;
                }
            }

            if (isTarget == false) return null;

            return cardTarget;
        }
    }

    public enum FILTTERCOMPARATION
    {
        LESS,
        LESSOREQUAL,
        EQUEAL,
        GREATER,
        GREATEROREQUAL,
        DIFFERENT
    }

    public enum ESPECIFICFILTTERCOMPARATION
    {
        SIMPLE, // EL VALOR TAL CUAL ES
        ADITION, // EL VALOR SUMADO OTRO NUMERO
        SUBSTRACTION, // EL VALOR RESTADO OTRO NUMERO
        DIVISION, // EL VALOR DIVIDIDO POR UN NUMERO
        MULTIPLICATION, // EL VALOR MULTIPLICADO POR UN NUMERO
        PERCENT // UN PORCENTAJE DEL VALOR
    }

    public class CheckStatFiltter : CardFiltter
    {
        int IDstatToCheck;

        public CheckStatFiltter(int ID, int IDstatToCheck, List<CARDTARGETTYPE> targetRequired) : base(ID, targetRequired)
        {
            this.IDstatToCheck = IDstatToCheck;
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            ICardTarget targetAux = base.CheckTarget(cardTarget);
            if (targetAux == null) return null;

            IOcuppy ocuppyAux = targetAux.GetOcuppy();
            if (ocuppyAux == null) return null;
            if (ocuppyAux.Stats.ContainsKey(IDstatToCheck) == false) return null;

            return cardTarget;
        }
    }

    public class CheckStatComparisonFiltter : CheckStatFiltter
    {
        FiltterComparation filtterComparation;

        public CheckStatComparisonFiltter(int ID, int IDstatToCheck, FILTTERCOMPARATION filtter, int amountToCompare, bool compareMaxStatValue, List<CARDTARGETTYPE> targetRequired) : base(ID, IDstatToCheck, targetRequired)
        {
            filtterComparation = new FiltterComparation(filtter, amountToCompare, IDstatToCheck, compareMaxStatValue);
        }

        public override ICardTarget CheckTarget(ICardTarget cardTarget)
        {
            ICardTarget targetAux = base.CheckTarget(cardTarget);
            if (targetAux == null) return null;
            targetAux = filtterComparation.CompareCardTargetStatAmount(targetAux.GetOcuppy());
            if (targetAux == null) return null;

            return cardTarget;
        }
    }

    public class FiltterComparation
    {
        FILTTERCOMPARATION filtter;
        int amountToCompare;
        int IDstatToCheck;
        bool compareMaxStatValue;
        public FiltterComparation(FILTTERCOMPARATION filtter, int amountToCompare, int IDstatToCheck, bool compareMaxStatValue)
        {
            this.filtter = filtter;
            this.amountToCompare = amountToCompare;
            this.IDstatToCheck = IDstatToCheck;
        }

        public ICardTarget CompareCardTargetStatAmount(IOcuppy ocuppy)
        {
            if (ocuppy == null) return null;

            int statAmountToCheck = ocuppy.Stats[IDstatToCheck].ActualStatValue;
            if (compareMaxStatValue == true) statAmountToCheck = ocuppy.Stats[IDstatToCheck].MaxStatValue;

            switch (filtter)
            {
                case FILTTERCOMPARATION.LESS:
                    if (statAmountToCheck < amountToCompare) return ocuppy;
                    return null;
                case FILTTERCOMPARATION.LESSOREQUAL:
                    if (statAmountToCheck <= amountToCompare) return ocuppy;
                    return null;
                case FILTTERCOMPARATION.EQUEAL:
                    if (statAmountToCheck == amountToCompare) return ocuppy;
                    return null;
                case FILTTERCOMPARATION.GREATER:
                    if (statAmountToCheck > amountToCompare) return ocuppy;
                    return null;
                case FILTTERCOMPARATION.GREATEROREQUAL:
                    if (statAmountToCheck >= amountToCompare) return ocuppy;
                    return null;
                case FILTTERCOMPARATION.DIFFERENT:
                    if (statAmountToCheck != amountToCompare) return ocuppy;
                    return null;
                default:
                    return null;
            }
        }

    }

    /*
     * 
     * TARGET FILTTER SELECTOR
     * SE ENCARGA DE GENERAR LOS TARGETS PARA BUSCAR LO QUE NECESITAMOS
     * LISTA DE UNIDADES ALIADAS
     * RECORREMOS ESA LISTA 
     * 
     * 
     * 
     * 
     * SIMPLE COMPARATION DATA = PARA CUANDO SOLO COMPARAMOS UN INT
     * TARGET COMPARATION DATA = CUANDO COMPARAMOS ALGO DE ALGUN TARGET
     * TARGETS COMPARATION DATA = CUANDO COMPARAMOS ALGO DE VARIOS TARGETS
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * CHECK IF HAVE SPECIFIC STAT MODIFIER ID TYPE EJ: BUFF/NERF
     * CHECK STAT AMOUNT
     * 
     * 
     * CHECK AVAILABLE SPACE BEHIND UNIT // IN POSITION
     * CHECK OBJECT IN BATTLEFIELD
     * CHECK UNIT IN BATTLEFIELD
     * CHECK UNIT ENEMY
     * CHECK UNIT ON BOARD
     * CHECK IF HAVE ABILITY ID
     * CHECK UNIT IN SPAWN
     * CHECK UNIT TYPE
     * CHECK IF CAN PERFORM ABILITY ID
     * CHECK IF HAVE ABILITY MODIFIER ID
     * CHECK IF HAVE SPECIFIC ABILITY MODIFIER ID TYPE EJ: BUFF/NERF
     * CHECK PLAYER DECK CARD AMOUNT
     * CHECK PLAYER CEMENTERY CARD TYPE
     * CHECK CARD TYPE
     * 
     * 
     */
}

namespace MikzeerGameNotas
{
    public enum COMPARATIONTYPE
    {
        LESS,
        LESSOREQUAL,
        EQUEAL,
        GREATER,
        GREATEROREQUAL,
        DIFFERENT
    }
    public enum STATAMOUNTTYPE
    {
        ACTUAL,
        MAX
    }
    public enum SPECIALOPERATION
    {
        SIMPLE, // EL VALOR TAL CUAL ES
        ADITION, // EL VALOR SUMADO OTRO NUMERO
        SUBSTRACTION, // EL VALOR RESTADO OTRO NUMERO
        DIVISION, // EL VALOR DIVIDIDO POR UN NUMERO
        MULTIPLICATION, // EL VALOR MULTIPLICADO POR UN NUMERO
        PERCENT // UN PORCENTAJE DEL VALOR
    }
    public class StatComparisonIDFiltter
    {
        int statID;
        public StatComparisonIDFiltter(int statID)
        {
            this.statID = statID;
        }
        public bool IsValidStat(IOcuppy ocuppy)
        {
            return ocuppy.Stats.ContainsKey(statID);
        }
    }
    public class Comparator
    {
        public bool IsValidComparation(COMPARATIONTYPE filtter, ResultData data, ResultData dataToChekAgainst)
        {
            bool isValid = false;
            switch (filtter)
            {
                case COMPARATIONTYPE.LESS:
                    if (data.amount < dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.LESSOREQUAL:
                    if (data.amount <= dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.EQUEAL:
                    if (data.amount == dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.GREATER:
                    if (data.amount > dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.GREATEROREQUAL:
                    if (data.amount >= dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.DIFFERENT:
                    if (data.amount != dataToChekAgainst.amount) isValid = true;
                    break;
                default:
                    break;
            }
            return isValid;
        }
        public bool IsValidComparation(COMPARATIONTYPE filtter, ResultData data, List<ResultData> dataToChekAgainst)
        {
            bool isValid = true;
            for (int i = 0; i < dataToChekAgainst.Count; i++)
            {
                bool hasPass = false;
                switch (filtter)
                {
                    case COMPARATIONTYPE.LESS:
                        if (data.amount < dataToChekAgainst[i].amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.LESSOREQUAL:
                        if (data.amount <= dataToChekAgainst[i].amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.EQUEAL:
                        if (data.amount == dataToChekAgainst[i].amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.GREATER:
                        if (data.amount > dataToChekAgainst[i].amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.GREATEROREQUAL:
                        if (data.amount >= dataToChekAgainst[i].amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.DIFFERENT:
                        if (data.amount != dataToChekAgainst[i].amount)
                        {
                            hasPass = true;
                        }
                        break;
                    default:
                        break;
                }

                if (hasPass == false)
                {
                    isValid = hasPass;
                    break;
                }
            }
            return isValid;
        }
        public bool IsValidComparation(COMPARATIONTYPE filtter, List<ResultData> data, ResultData dataToChekAgainst)
        {
            bool isValid = true;
            for (int i = 0; i < data.Count; i++)
            {
                bool hasPass = false;
                switch (filtter)
                {
                    case COMPARATIONTYPE.LESS:
                        if (data[i].amount < dataToChekAgainst.amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.LESSOREQUAL:
                        if (data[i].amount <= dataToChekAgainst.amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.EQUEAL:
                        if (data[i].amount == dataToChekAgainst.amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.GREATER:
                        if (data[i].amount > dataToChekAgainst.amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.GREATEROREQUAL:
                        if (data[i].amount >= dataToChekAgainst.amount)
                        {
                            hasPass = true;
                        }
                        break;
                    case COMPARATIONTYPE.DIFFERENT:
                        if (data[i].amount != dataToChekAgainst.amount)
                        {
                            hasPass = true;
                        }
                        break;
                    default:
                        break;
                }

                if (hasPass == false)
                {
                    isValid = hasPass;
                    break;
                }
            }
            return isValid;
        }
        public bool IsValidComparation(COMPARATIONTYPE filtter, List<ResultData> data, List<ResultData> dataToChekAgainst)
        {
            bool isValid = true;
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < dataToChekAgainst.Count; j++)
                {
                    bool hasPass = false;
                    switch (filtter)
                    {
                        case COMPARATIONTYPE.LESS:
                            if (data[i].amount < dataToChekAgainst[j].amount)
                            {
                                hasPass = true;
                            }
                            break;
                        case COMPARATIONTYPE.LESSOREQUAL:
                            if (data[i].amount <= dataToChekAgainst[j].amount)
                            {
                                hasPass = true;
                            }
                            break;
                        case COMPARATIONTYPE.EQUEAL:
                            if (data[i].amount == dataToChekAgainst[j].amount)
                            {
                                hasPass = true;
                            }
                            break;
                        case COMPARATIONTYPE.GREATER:
                            if (data[i].amount > dataToChekAgainst[j].amount)
                            {
                                hasPass = true;
                            }
                            break;
                        case COMPARATIONTYPE.GREATEROREQUAL:
                            if (data[i].amount >= dataToChekAgainst[j].amount)
                            {
                                hasPass = true;
                            }
                            break;
                        case COMPARATIONTYPE.DIFFERENT:
                            if (data[i].amount != dataToChekAgainst[j].amount)
                            {
                                hasPass = true;
                            }
                            break;
                        default:
                            break;
                    }

                    if (hasPass == false)
                    {
                        isValid = hasPass;
                        return isValid;
                    }
                }
            }
            return isValid;
        }
    }
    public class ResultData
    {
        public int amount { get; set; }

        public ResultData(int amount)
        {
            this.amount = amount;
        }
    }
    public class SpecialOperation
    {
        SPECIALOPERATION operationType;
        int soAmount;
        public SpecialOperation(SPECIALOPERATION operationType, int soAmount)
        {
            this.operationType = operationType;
            this.soAmount = soAmount;
        }

        public ResultData GetResultDataFiltter(ResultData resultData)
        {
            int finalAmount = 0;
            switch (operationType)
            {
                case SPECIALOPERATION.SIMPLE:
                    finalAmount = resultData.amount;
                    break;
                case SPECIALOPERATION.ADITION:
                    finalAmount = resultData.amount + soAmount;
                    break;
                case SPECIALOPERATION.SUBSTRACTION:
                    finalAmount = resultData.amount - soAmount;
                    break;
                case SPECIALOPERATION.DIVISION:
                    if (resultData.amount == 0 || soAmount == 0)
                    {
                        finalAmount = 0;
                        break;
                    }
                    finalAmount = resultData.amount / soAmount;
                    break;
                case SPECIALOPERATION.MULTIPLICATION:
                    finalAmount = resultData.amount * soAmount;
                    break;
                case SPECIALOPERATION.PERCENT:
                    finalAmount = soAmount * resultData.amount / 100;
                    break;
                default:
                    break;
            }
            resultData.amount = finalAmount;
            return resultData;
        }
    }

    public interface ICompareData
    {
        ResultData GetResultData();
    }
    public class ComparisonData : ICompareData
    {
        protected ResultData resultData;

        public ComparisonData(int amount)
        {
            resultData = new ResultData(amount);
        }

        public ResultData GetResultData()
        {
            return resultData;
        }
    }
    public class StatComparisonData : ICompareData
    {
        int statID;
        STATAMOUNTTYPE statAmountType;
        IOcuppy ocuppy;
        public StatComparisonData(int statID, STATAMOUNTTYPE statAmountType, IOcuppy ocuppy)
        {
            this.statID = statID;
            this.statAmountType = statAmountType;
            this.ocuppy = ocuppy;
        }

        public StatComparisonData(int statID, STATAMOUNTTYPE statAmountType)
        {
            this.statID = statID;
            this.statAmountType = statAmountType;
        }

        public void SetOcuppier(IOcuppy ocuppy)
        {
            this.ocuppy = ocuppy;
        }

        public ResultData GetResultData()
        {
            if (ocuppy == null) return null;

            ResultData resultData = new ResultData(0);
            switch (statAmountType)
            {
                case STATAMOUNTTYPE.ACTUAL:
                    resultData.amount = ocuppy.Stats[statID].ActualStatValue;
                    break;
                case STATAMOUNTTYPE.MAX:
                    resultData.amount = ocuppy.Stats[statID].MaxStatValue;
                    break;
            }
            return resultData;
        }
    }
    public class SpecialStatComparisonData : ICompareData
    {
        SpecialOperation specialOperation; //  QUE OPERACION QUIERO EFECTUAR
        StatComparisonData statComparisonData; // EN QUE TIPO DE STAT
        List<IOcuppy> occupiers;

        public SpecialStatComparisonData(List<IOcuppy> occupiers, SpecialOperation specialOperation, StatComparisonData statComparisonData)
        {
            this.occupiers = occupiers;
            this.specialOperation = specialOperation;
            this.statComparisonData = statComparisonData;
        }
        public ResultData GetResultData()
        {
            ResultData data = new ResultData(0);
            // RECORREMOS TODA LA LISTA DE OCCUPIER A EFECTUAR LA OPERACION ESPECIAL
            for (int i = 0; i < occupiers.Count; i++)
            {
                // SEGUN EL IDSTAT/STATAMOUNTTYPE VAMOS A BUSCAR EN ESE DETERMINADO OCUPPIER LA RESULT DATA QUE NECESITEMOS
                statComparisonData.SetOcuppier(occupiers[i]);
                ResultData occupierResultData = statComparisonData.GetResultData();
                // SE LO SUMAMOS A LA CANTIDAD ACTUAL QUE TENGAMOS
                // LO SUMAMOS YA QUE SIEMPRE VAMOS A EFECUTAR UNA OPERACION ESPECIAL LUEGO DE SUMAR TODA UNA CANTIDAD
                // NO VAMOS A RESTAR TODA UNA CANTIDAD O DIVIDIRLA O MULTIPLICAR UNA POR UNA
                // SINO QUE A UNA CANTIDAD TOTAL LE VAMOS A APLICAR EL O LOS FILTROS DETERMINADOS
                data.amount += occupierResultData.amount;

            }
            // APLICAMOS LA OPERACION ESPECIAL Y DEVOLVEMOS EL RESULTADO
            data = specialOperation.GetResultDataFiltter(data);
            return data;
        }


    }

    public class SpecialStatConditionalComparisonData
    {
        Comparator comparator;
        StatComparisonData statComparisonDt;

        public SpecialStatConditionalComparisonData()
        {
            comparator = new Comparator();
        }
    }

    public class Test
    {
        public Test()
        {

        }
        public void TestOne()
        {
            // CHECK UNIDAD CON STAT HP ACTUAL MAYOR O IGUAL A 5


            // 1 - CREAMOS Y SETEAMOS LOS TARGETS DESDE ALGUN LADO
            // 1A- A QUIEN/ES VAMOS A COMPRAR
            KimbokoXFactory ki = new KimbokoXFactory();
            Kimboko kimb = ki.CreateKimboko(0, new Player(0, PLAYERTYPE.PLAYER));

            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(0);
            if (statIDFiltter.IsValidStat(kimb) == false)
            {
                Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
                return;
            }
            // 3 - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            ICompareData unit = new StatComparisonData(0, STATAMOUNTTYPE.ACTUAL, kimb);
            ResultData unitResultData = unit.GetResultData();

            // 4 - OBTENEMOS LA RESULT DATA DEL COMPARE DATA SENCILLO
            // ACA NO TENEMSO QUE CHEQUEAR NINGUN ID YA QUE ES DE TIPO SENCILLO
            ICompareData data = new ComparisonData(1);
            ResultData simpleResultData = data.GetResultData();

            // 4- COMPARAMOS LOS RESULTADOS FINALES
            Comparator comparator = new Comparator();
            bool isValid = comparator.IsValidComparation(COMPARATIONTYPE.GREATEROREQUAL, unitResultData, simpleResultData);

            if (isValid)
            {
                Debug.Log("is VALID COMPARE DATA");
            }
            else
            {
                Debug.Log("INVALID COMPARE DATA");
            }
        }
        public void TestTwo()
        {
            // CHECK HP ACTUAL DEL OCUPPIER ES MAYOR O IGUAL AL HP MAXIMO DE OTRO OCCUPIER

            // 1 - CREAMOS Y SETEAMOS LOS TARGETS DESDE ALGUN LADO
            // 1A- A QUIEN/ES VAMOS A COMPRAR
            KimbokoXFactory ki = new KimbokoXFactory();
            Kimboko kimb = ki.CreateKimboko(0, new Player(0, PLAYERTYPE.PLAYER));

            Player player = new Player(0, PLAYERTYPE.PLAYER);

            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(0);
            if (statIDFiltter.IsValidStat(kimb) == false)
            {
                Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
                return;
            }
            // 3 - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            ICompareData unit = new StatComparisonData(0, STATAMOUNTTYPE.ACTUAL, kimb);
            ResultData unitResultData = unit.GetResultData();


            // 3b - CHEQUEAMOS SI EL PLAYER TIENE LA STAT REQUERIDA PARA BUSCAR 
            StatComparisonIDFiltter playerStatIDFiltter = new StatComparisonIDFiltter(0);
            if (playerStatIDFiltter.IsValidStat(player) == false)
            {
                Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
                return;
            }
            ICompareData data = new StatComparisonData(0, STATAMOUNTTYPE.MAX, player);
            ResultData playerResultData = data.GetResultData();

            // 4- COMPARAMOS LOS RESULTADOS FINALES
            Comparator comparator = new Comparator();
            bool isValid = comparator.IsValidComparation(COMPARATIONTYPE.GREATEROREQUAL, unitResultData, playerResultData);

            if (isValid)
            {
                Debug.Log("is VALID COMPARE DATA");
            }
            else
            {
                Debug.Log("INVALID COMPARE DATA");
            }
        }
        public void TestThree()
        {
            // CHECK HP ACTUAL DEL OCUPPIER ES MAYOR O IGUAL AL 90% DE LA SUMA DE TODO EL HP MAXIMO DE LAS UNIDADES ENEMIGAS

            // 1 - CREAMOS O SETEAMOS LOS TARGETS/OCUPPIER PARA GENERAR LAS REVISIONES
            KimbokoXFactory ki = new KimbokoXFactory();

            // 1A - A QUIEN/ES VAMOS A COMPRAR 
            Kimboko kimb = ki.CreateKimboko(0, new Player(0, PLAYERTYPE.PLAYER));
            // 2B - CONTRA QUIEN/ES VAMOS A COMPRAR
            Player player = new Player(0, PLAYERTYPE.PLAYER);
            Kimboko kimbEnem = ki.CreateKimboko(2, new Player(0, PLAYERTYPE.PLAYER));
            Kimboko kimbEnem2 = ki.CreateKimboko(3, new Player(0, PLAYERTYPE.PLAYER));

            List<IOcuppy> ocuppiers = new List<IOcuppy>();
            ocuppiers.Add(player);
            ocuppiers.Add(kimbEnem);
            ocuppiers.Add(kimbEnem2);

            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(0);
            if (statIDFiltter.IsValidStat(kimb) == false) return;

            // 2b - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            ICompareData unit = new StatComparisonData(0, STATAMOUNTTYPE.ACTUAL, kimb); // hp actual de la unidad
            ResultData unitResultData = unit.GetResultData();


            // 3 - CHEQUEAMOS SI LOS ENEMIGOS TIENEN LA STAT REQUERIDA PARA BUSCAR 
            StatComparisonIDFiltter enemiesStatIDFiltter = new StatComparisonIDFiltter(0);
            List<IOcuppy> ocpyAux = new List<IOcuppy>();
            for (int i = 0; i < ocuppiers.Count; i++)
            {
                if (enemiesStatIDFiltter.IsValidStat(ocuppiers[i]) == true)
                {
                    ocpyAux.Add(ocuppiers[i]);
                }
            }

            if (ocpyAux.Count == 0) return;


            // 3b - OBTENEMOS EL RESULT DATA DE EL 90% DE LA SUMA DE LA VIDA MAXIMA DE LAS UNIDADES ENEMIGAS
            SpecialOperation specialOperation = new SpecialOperation(SPECIALOPERATION.PERCENT, 90);
            StatComparisonData statComparisonData = new StatComparisonData(0, STATAMOUNTTYPE.MAX);
            ICompareData specialStatComparisonData = new SpecialStatComparisonData(ocuppiers, specialOperation, statComparisonData);
            ResultData enemiesResultData = specialStatComparisonData.GetResultData();

            // 4- COMPARAMOS LOS RESULTADOS FINALES
            Comparator comparator = new Comparator();
            bool isValid = comparator.IsValidComparation(COMPARATIONTYPE.GREATEROREQUAL, unitResultData, enemiesResultData);

            if (isValid)
            {
                Debug.Log("is VALID COMPARE DATA");
            }
            else
            {
                Debug.Log("INVALID COMPARE DATA");
            }
        }
        public void TestFour()
        {
            // CHECK SI
            // 10% DE LA SUMA DE TODO EL ATK POW ACTUAL STAT DE LAS UNIDADES ALIADAS
            // + MAS
            // 2 * LA SUMA DE TODO EL HP ACTUAL DE TODAS LAS UNIDADES ALIADAS
            // <= ES MENOS O IGUAL
            // 90% DE LA SUMA DE TODO EL HP ACTUAL STAT DE LAS UNIDADES ENEMIGAS
            //     CON ATK POW ACTUAL STAT
            //     >= MAYOR O IGUAL
            //     5


            // 1 - CREAMOS O SETEAMOS LOS TARGETS/OCUPPIER PARA GENERAR LAS REVISIONES
            KimbokoXFactory ki = new KimbokoXFactory();

            // 2B - CONTRA QUIEN/ES VAMOS A COMPRAR
            Player player = new Player(0, PLAYERTYPE.PLAYER);
            Kimboko kimbEnem = ki.CreateKimboko(2, new Player(0, PLAYERTYPE.PLAYER));
            Kimboko kimbEnem2 = ki.CreateKimboko(3, new Player(0, PLAYERTYPE.PLAYER));

            List<IOcuppy> ocuppiers = new List<IOcuppy>();
            ocuppiers.Add(player);
            ocuppiers.Add(kimbEnem);
            ocuppiers.Add(kimbEnem2);

            // 3 - CHEQUEAMOS SI LOS ALIADOS TIENE LA ATK POW STAT
            int targetStatID = 1;
            int soAmount = 10;
            StatComparisonIDFiltter alliesAtkPowStatIDFiltter = new StatComparisonIDFiltter(targetStatID);
            List<IOcuppy> ocpyAux = new List<IOcuppy>();
            for (int i = 0; i < ocuppiers.Count; i++)
            {
                if (alliesAtkPowStatIDFiltter.IsValidStat(ocuppiers[i]) == true)
                {
                    ocpyAux.Add(ocuppiers[i]);
                }
            }

            if (ocpyAux.Count == 0)
            {
                return;
            }

            // 3b - OBTENEMOS EL RESULT DATA DE 
            SpecialOperation specialOperation = new SpecialOperation(SPECIALOPERATION.PERCENT, soAmount);
            StatComparisonData statComparisonData = new StatComparisonData(targetStatID, STATAMOUNTTYPE.ACTUAL);
            ICompareData specialStatComparisonData = new SpecialStatComparisonData(ocpyAux, specialOperation, statComparisonData);
            ResultData alliesAtkPowResultData = specialStatComparisonData.GetResultData();


            // 3 - CHEQUEAMOS SI LOS ALIADOS TIENE LA HP STAT
            int HPtargetStatID = 0;
            int soAmounthp = 2;
            StatComparisonIDFiltter allieshPStatIDFiltter = new StatComparisonIDFiltter(HPtargetStatID);
            List<IOcuppy> ocpyhPAux = new List<IOcuppy>();
            for (int i = 0; i < ocuppiers.Count; i++)
            {
                if (allieshPStatIDFiltter.IsValidStat(ocuppiers[i]) == true)
                {
                    ocpyhPAux.Add(ocuppiers[i]);
                }
            }

            if (ocpyhPAux.Count == 0)
            {
                return;
            }

            // 3b - OBTENEMOS EL RESULT DATA DE 
            SpecialOperation specialOperationhp = new SpecialOperation(SPECIALOPERATION.MULTIPLICATION, soAmounthp);
            StatComparisonData statComparisonDataHP = new StatComparisonData(HPtargetStatID, STATAMOUNTTYPE.ACTUAL);
            ICompareData specialStatComparisonDataHP = new SpecialStatComparisonData(ocpyhPAux, specialOperationhp, statComparisonDataHP);
            ResultData alliesHpResultData = specialStatComparisonDataHP.GetResultData();


            // APLICAMOS LA OPERACION DE SUMAR 
            // 10% DE LA SUMA DE TODO EL ATK POW ACTUAL STAT DE LAS UNIDADES ALIADAS
            // + MAS
            // 2 * LA SUMA DE TODO EL HP ACTUAL DE TODAS LAS UNIDADES ALIADAS
            SpecialOperation additionSpecOp = new SpecialOperation(SPECIALOPERATION.ADITION, alliesAtkPowResultData.amount);
            ResultData allAliesResultData = additionSpecOp.GetResultDataFiltter(alliesHpResultData);

        }
    }
}
