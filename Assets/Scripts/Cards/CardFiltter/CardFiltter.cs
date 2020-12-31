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

    public enum COMPARATIONTYPE
    {
        LESS,
        LESSOREQUAL,
        EQUEAL,
        GREATER,
        GREATEROREQUAL,
        DIFFERENT
    }
    public class ResultData
    {
        public int amount { get; set; }

        public ResultData(int amount)
        {
            this.amount = amount;
        }
    }
    public interface IResultData
    {
        ResultData GetResultData();
    }
    public class ComparatorFiltter
    {
        COMPARATIONTYPE filtter;
        public ComparatorFiltter(COMPARATIONTYPE filtter)
        {
            this.filtter = filtter;
        }
        public bool IsValidComparation(IResultData Idata, IResultData IdataToChekAgainst)
        {
            bool isValid = false;
            ResultData data = Idata.GetResultData();
            ResultData dataToChekAgainst = IdataToChekAgainst.GetResultData();
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
    }
    public class ResultDataValidator
    {
        IResultData rdToCheck;
        IResultData rdToCheckAgainst;
        ComparatorFiltter comparatorFiltter;

        public ResultDataValidator(IResultData rdToCheck, IResultData rdToCheckAgainst, COMPARATIONTYPE filtter)
        {
            this.rdToCheck = rdToCheck;
            this.rdToCheckAgainst = rdToCheckAgainst;
            comparatorFiltter = new ComparatorFiltter(filtter);
        }

        public bool IsValid()
        {
            return comparatorFiltter.IsValidComparation(rdToCheck, rdToCheckAgainst);
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
    public class Comparator
    {
        COMPARATIONTYPE filtter;
        public Comparator()
        {
        }
        public Comparator(COMPARATIONTYPE filtter)
        {
            this.filtter = filtter;
        }
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
        public bool IsValidComparation(ResultData data, ResultData dataToChekAgainst)
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
    }
    public class SpecialStatComparisonData : IResultData
    {
        public RESULTDATATYPE resultDataType { get; set; }
        ResultDataOperationFiltter specialOperation; //  QUE OPERACION QUIERO EFECTUAR
        StatIResultData statComparisonData; // EN QUE TIPO DE STAT
        List<IOcuppy> occupiers;

        public SpecialStatComparisonData(List<IOcuppy> occupiers, ResultDataOperationFiltter specialOperation, StatIResultData statComparisonData)
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
    public class ComparisonDataManager : IResultData
    {
        public RESULTDATATYPE resultDataType { get; set; }
        public ResultData GetResultData()
        {
            // EFFECTO ID 8 
            /*
             * SIMPLECOMPARISONDATA 
             *                      => int Amount
             *
             * SIMPLEOCUPPIERDATA // PODEMOS COMPARAR UN STAT, CANTIDAD DE USOS DE UNA HABILIDAD, CUANTOS TURNOS PASARON
             *                    // 
             *                    
             *                   => STATRESULTDATA => ResultData devuelve la cantidad de ese StatID
             *                   => SPECIALRESULTDATA => ResultData devuleve
             *                                           CANTIDAD DE TURNOS
             *                                           DANO RECIBIDO TOTAL UNIDADES ALIADAS/ENEMIGAS // DANO RECIBIDO TOTAL UNA UNIDAD
             *                                           ATAQUE REALIZAD TOTAL A UNIDADES ALIADAS/ENEMIGAS // ATAQUE REALIZAD TOTAL DE UNA UNIDAD
             *                                           CANTIDAD UNIDADES ALIADAS/ENEMIGAS QUE COMBINARON // CUANTAS VECES COMBINO UNA UNIDAD
             *                                           CANTIDAD UNIDADES QUE SE DESCOMBINARON // CUANTAS VECES SE DESCOMBINO UNA UNIDAD
             *                                           CANTIDAD UNIDADES QUE SE FUSIONARON // 
             *                                           CANTIDAD UNIDADES QUE EVOLUCIONARON // 
             *                                           CANTIDAD UNIDADES QUE SPAWNEO UN JUGADOR
             *                                           CANTIDAD UNIDADES QUE SE MURIERON TOTAL // CANTIDAD DE UNIDADES QUE SE MURIERON A UN PLAYER
             *                                           CANTIDAD DE PASOS DADOS TOTALES POR TODAS LAS UNIDADES / ALIADOS / ENEMIGOS / UNA UNIDAD
             *                                           CANTIDAD DE CARTAS LEVANTADAS / USADAS / DESCARTADAS
             *                                               
             *                   => ABILITYRESULTDATA => ResultData devuelve
             *                                           CANTIDAD DE MODIFIERS
             *                                           USO CON EXITO
             *                                           USO FALLIDO
             *                                               
             *                   
             * 
             * 
             * 
             */


            // 1 - CREAMOS O SETEAMOS LOS TARGETS/OCUPPIER PARA GENERAR LAS REVISIONES

            // SIMPLE COMPARISON DATA =>  SOLO UN NUMERO
            // SIMPLE OCUPPIER DATA => SOLO UN OCCUPIER
            // MULTIPLE OCCUPIER DATA => VARIOS OCCUPPIER
            // TILE DATA => UNA TILE EN EL CAMPO DE BATALLA O EL SPAWN





            // PODEMOS TENER DATA PRECARGADA, COMO UN RESULT DATA PRECARGADO DE UNA COMPARACION. EJ: SI ES >= 5(CINCO) 

            throw new System.NotImplementedException();
            // TODAS LAS CLASES VAN A TERMINAR CON UN COMPARATOR SIEMPRE
            // Y EL COMPARATIONTYPE VA A SER ESPECIFICA Y UNICA EL FILTRO QUE ESTEMOS APLICANDO
            // EJ >= = ETC

            // SI TENEMOS UN COMPARATOR ENTONCES VAMOS A NECESITAR SI O SI DOS RESULT DATA
            // RESULTA DATA A COMPARAR
            // RESULTA DATA CONTRA QUE COMPARAR

            // CHECK UNIDAD CON STAT HP ACTUAL MAYOR O IGUAL A 5

            // 1A- A QUIEN/ES VAMOS A COMPRAR
            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            // 3 - OBTENEMOS LA RESULT DATA DE LA UNIDAD

            // 4 - OBTENEMOS LA RESULT DATA DEL COMPARE DATA SENCILLO
            // ACA NO TENEMSO QUE CHEQUEAR NINGUN ID YA QUE ES DE TIPO SENCILLO

            // 5- COMPARAMOS LOS RESULTADOS FINALES


            // CHECK HP ACTUAL DEL OCUPPIER ES MAYOR O IGUAL AL HP MAXIMO DE OTRO OCCUPIER

            // 1A- A QUIEN/ES VAMOS A COMPRAR
            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            // 3 - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            // 3b - CHEQUEAMOS SI EL PLAYER TIENE LA STAT REQUERIDA PARA BUSCAR 
            // 3C- OBTENEMOS LA RESULT DATA DEL PLAYER
            // 4- COMPARAMOS LOS RESULTADOS FINALES


            // CHECK HP ACTUAL DEL OCUPPIER ES MAYOR O IGUAL AL 90% DE LA SUMA DE TODO EL HP MAXIMO DE LAS UNIDADES ENEMIGAS


            // 1A - A QUIEN/ES VAMOS A COMPRAR 
            // 2B - CONTRA QUIEN/ES VAMOS A COMPRAR
            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            // 2b - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            // 3 - CHEQUEAMOS SI LOS ENEMIGOS TIENEN LA STAT REQUERIDA PARA BUSCAR 
            // 3b - OBTENEMOS EL RESULT DATA DE EL 90% DE LA SUMA DE LA VIDA MAXIMA DE LAS UNIDADES ENEMIGAS
            // 4- COMPARAMOS LOS RESULTADOS FINALES

            // CHECK SI
            // 10% DE LA SUMA DE TODO EL ATK POW ACTUAL STAT DE LAS UNIDADES ALIADAS
            // + MAS
            // 2 * LA SUMA DE TODO EL HP ACTUAL DE TODAS LAS UNIDADES ALIADAS
            // <= ES MENOS O IGUAL
            // 90% DE LA SUMA DE TODO EL HP ACTUAL STAT DE LAS UNIDADES ENEMIGAS
            //     CON ATK POW ACTUAL STAT
            //     >= MAYOR O IGUAL
            //     5


            // 2B - CONTRA QUIEN/ES VAMOS A COMPRAR
            // 3 - CHEQUEAMOS SI LOS ALIADOS TIENE LA ATK POW STAT
            // 3b - OBTENEMOS EL RESULT DATA DE EL RESULTADO DE LA SUMA DEL 10 DEL ATK POW DE LOS ALIADOS
            // 3 - CHEQUEAMOS SI LOS ALIADOS TIENE LA HP STAT
            // 3b - OBTENEMOS EL RESULT DATA DE LA MULTIPLICACION POR 2 DE LA VIDA ACUTUAL DE LOS ALIADOS
            // 4  - APLICAMOS LA OPERACION DE SUMAR 
            // 10% DE LA SUMA DE TODO EL ATK POW ACTUAL STAT DE LAS UNIDADES ALIADAS
            // + MAS
            // 2 * LA SUMA DE TODO EL HP ACTUAL DE TODAS LAS UNIDADES ALIADAS
        }
    }
    public class Test
    {
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
            IResultData unit = new StatIResultData(0, STATAMOUNTTYPE.ACTUAL, kimb);
            ResultData unitResultData = unit.GetResultData();

            // 4 - OBTENEMOS LA RESULT DATA DEL COMPARE DATA SENCILLO
            // ACA NO TENEMSO QUE CHEQUEAR NINGUN ID YA QUE ES DE TIPO SENCILLO
            IResultData data = new SimpleIResultData(1);
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
            IResultData unit = new StatIResultData(0, STATAMOUNTTYPE.ACTUAL, kimb);
            ResultData unitResultData = unit.GetResultData();


            // 3b - CHEQUEAMOS SI EL PLAYER TIENE LA STAT REQUERIDA PARA BUSCAR 
            StatComparisonIDFiltter playerStatIDFiltter = new StatComparisonIDFiltter(0);
            if (playerStatIDFiltter.IsValidStat(player) == false)
            {
                Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
                return;
            }
            IResultData data = new StatIResultData(0, STATAMOUNTTYPE.MAX, player);
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
            IResultData unit = new StatIResultData(0, STATAMOUNTTYPE.ACTUAL, kimb); // hp actual de la unidad
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
            ResultDataOperationFiltter specialOperation = new ResultDataOperationFiltter(OPERATIONTYPE.PERCENT, 90);
            StatIResultData statComparisonData = new StatIResultData(0, STATAMOUNTTYPE.MAX);
            IResultData specialStatComparisonData = new SpecialStatComparisonData(ocuppiers, specialOperation, statComparisonData);
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

            // 3b - OBTENEMOS EL RESULT DATA DE EL RESULTADO DE LA SUMA DEL 10 DEL ATK POW DE LOS ALIADOS
            ResultDataOperationFiltter specialOperation = new ResultDataOperationFiltter(OPERATIONTYPE.PERCENT, soAmount);
            StatIResultData statComparisonData = new StatIResultData(targetStatID, STATAMOUNTTYPE.ACTUAL);
            IResultData specialStatComparisonData = new SpecialStatComparisonData(ocpyAux, specialOperation, statComparisonData);
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

            // 3b - OBTENEMOS EL RESULT DATA DE LA MULTIPLICACION POR 2 DE LA VIDA ACUTUAL DE LOS ALIADOS
            ResultDataOperationFiltter specialOperationhp = new ResultDataOperationFiltter(OPERATIONTYPE.MULTIPLICATION, soAmounthp);
            StatIResultData statComparisonDataHP = new StatIResultData(HPtargetStatID, STATAMOUNTTYPE.ACTUAL);
            IResultData specialStatComparisonDataHP = new SpecialStatComparisonData(ocpyhPAux, specialOperationhp, statComparisonDataHP);
            ResultData alliesHpResultData = specialStatComparisonDataHP.GetResultData();


            // APLICAMOS LA OPERACION DE SUMAR 
            // 10% DE LA SUMA DE TODO EL ATK POW ACTUAL STAT DE LAS UNIDADES ALIADAS
            // + MAS
            // 2 * LA SUMA DE TODO EL HP ACTUAL DE TODAS LAS UNIDADES ALIADAS
            ResultDataOperationFiltter additionSpecOp = new ResultDataOperationFiltter(OPERATIONTYPE.ADITION, alliesAtkPowResultData.amount);
            ResultData allAliesResultData = additionSpecOp.GetResultDataFiltter(alliesHpResultData);

        }
    }


    public enum STATAMOUNTTYPE
    {
        ACTUAL,
        MAX
    }
    public enum OPERATIONTYPE
    {
        SIMPLE, // EL VALOR TAL CUAL ES
        ADITION, // EL VALOR SUMADO OTRO NUMERO
        SUBSTRACTION, // EL VALOR RESTADO OTRO NUMERO
        DIVISION, // EL VALOR DIVIDIDO POR UN NUMERO
        MULTIPLICATION, // EL VALOR MULTIPLICADO POR UN NUMERO
        PERCENT // UN PORCENTAJE DEL VALOR
    }
    public enum COMPARATIONTYPE
    {
        LESS,
        LESSOREQUAL,
        EQUEAL,
        GREATER,
        GREATEROREQUAL,
        DIFFERENT
    }
    public class ResultData
    {
        public int amount { get; set; }

        public ResultData(int amount)
        {
            this.amount = amount;
        }
    }
    public class ComparatorFiltter
    {
        COMPARATIONTYPE filtter;
        public ComparatorFiltter(COMPARATIONTYPE filtter)
        {
            this.filtter = filtter;
        }
        public bool IsValidComparation(IResultData Idata, IResultData IdataToChekAgainst)
        {
            bool isValid = false;
            ResultData data = Idata.GetResultData();
            ResultData dataToChekAgainst = IdataToChekAgainst.GetResultData();
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
    }
    public enum RESULTDATATYPE
    {
        SIMPLE,
        STAT,
        SPECIALOPERATION,
        MULTIPLEOCCUPIER,
        VALIDATOR
    }
    public interface IResultData
    {
        RESULTDATATYPE resultDataType { get; }
        ResultData GetResultData();
    }
    public interface IResultDataOperationFillter : IResultData
    {
        ResultDataOperationFiltter resultDataOperationFiltter { get; }
        void SetOperationFiltter(ResultDataOperationFiltter rdoFiltter);
    }
    public interface IResultDataOccupier : IResultDataOperationFillter
    {
        void SetOcuppier(IOcuppy ocuppier);
    }
    public class SimpleIResultData : IResultData
    {
        protected ResultData resultData;
        public RESULTDATATYPE resultDataType { get; set; }
        public SimpleIResultData(int amount)
        {
            resultData = new ResultData(amount);
            resultDataType = RESULTDATATYPE.SIMPLE;
        }

        public ResultData GetResultData()
        {
            return resultData;
        }
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
    public class StatIResultData : IResultDataOccupier
    {
        public RESULTDATATYPE resultDataType { get; set; }
        public ResultDataOperationFiltter resultDataOperationFiltter { get; set; }

        int statID;
        STATAMOUNTTYPE statAmountType;
        IOcuppy ocuppy;
        public StatIResultData(int statID, STATAMOUNTTYPE statAmountType, IOcuppy ocuppy)
        {
            this.statID = statID;
            this.statAmountType = statAmountType;
            this.ocuppy = ocuppy;
            resultDataType = RESULTDATATYPE.STAT;
        }
        public StatIResultData(int statID, STATAMOUNTTYPE statAmountType)
        {
            this.statID = statID;
            this.statAmountType = statAmountType;
            resultDataType = RESULTDATATYPE.STAT;
        }
        public void SetOcuppier(IOcuppy ocuppy)
        {
            this.ocuppy = ocuppy;
        }
        public void SetOperationFiltter(ResultDataOperationFiltter rdoFiltter)
        {
            resultDataOperationFiltter = rdoFiltter;
        }
        public ResultData GetResultData()
        {
            // 1 - CHEQUEAMOS QUE TENGAMSO UN OCCUPIER
            if (ocuppy == null) return null;

            // 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            // ESTO DEBE ESTAR ACA YA QUE ACA VAMOS A USAR EN SI AL STAT
            StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(statID);
            if (statIDFiltter.IsValidStat(ocuppy) == false)
            {
                Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
                return null;
            }

            // GENERAMOS Y OBTENEMOS LA RESULT DATA
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

            if (resultDataOperationFiltter != null)
            {
                resultData = resultDataOperationFiltter.GetResultDataFiltter(resultData);
            }
            return resultData;
        }


    }
    public class SpecialOperationIResultData : IResultDataOperationFillter
    {
        public RESULTDATATYPE resultDataType { get; set; }
        public ResultDataOperationFiltter resultDataOperationFiltter { get; set; }
        IResultData firsResultData;
        IResultData secondResultData;
        OPERATIONTYPE operationType;

        public SpecialOperationIResultData(OPERATIONTYPE operationType, IResultData firsResultData, IResultData secondResultData)
        {
            this.operationType = operationType;
            this.firsResultData = firsResultData;
            this.secondResultData = secondResultData;
            resultDataType = RESULTDATATYPE.SPECIALOPERATION;
        }
        public void SetOperationFiltter(ResultDataOperationFiltter rdoFiltter)
        {
            resultDataOperationFiltter = rdoFiltter;
        }
        public ResultData GetResultData()
        {
            int firstAmount = firsResultData.GetResultData().amount;
            int secondAmount = secondResultData.GetResultData().amount;
            int finalAmount = 0;
            ResultData data;
            switch (operationType)
            {
                case OPERATIONTYPE.ADITION:
                    finalAmount = firstAmount + secondAmount;
                    break;
                case OPERATIONTYPE.SUBSTRACTION:
                    finalAmount = firstAmount - secondAmount;
                    break;
                case OPERATIONTYPE.DIVISION:

                    finalAmount = firstAmount / secondAmount;
                    break;
                case OPERATIONTYPE.MULTIPLICATION:
                    finalAmount = firstAmount * secondAmount;
                    break;
                default:
                    return null;
            }
            data = new ResultData(finalAmount);

            if (resultDataOperationFiltter != null)
            {
                data = resultDataOperationFiltter.GetResultDataFiltter(data);
            }

            return data;
        }
    }
    public class MultipleOcuppierIResultData : IResultDataOperationFillter
    {
        public RESULTDATATYPE resultDataType { get; set; }
        public ResultDataOperationFiltter resultDataOperationFiltter { get; set; }
        IResultDataOccupier resultDataOccupier;
        List<IOcuppy> occupiers;

        public MultipleOcuppierIResultData(IResultDataOccupier resultDataOccupier)
        {
            this.resultDataOccupier = resultDataOccupier;
            resultDataType = RESULTDATATYPE.MULTIPLEOCCUPIER; 
        }

        public void SetOperationFiltter(ResultDataOperationFiltter rdoFiltter)
        {
            resultDataOperationFiltter = rdoFiltter;
        }

        public void SetOcuppiers(List<IOcuppy> occupiers)
        {
            this.occupiers = occupiers;
        }

        public ResultData GetResultData()
        {
            ResultData data = new ResultData(0);
            // RECORREMOS TODA LA LISTA DE OCCUPIER A EFECTUAR LA OPERACION ESPECIAL
            for (int i = 0; i < occupiers.Count; i++)
            {
                resultDataOccupier.SetOcuppier(occupiers[i]);
                ResultData occupierResultData = resultDataOccupier.GetResultData();
                data.amount += occupierResultData.amount;
            }
            if (resultDataOperationFiltter != null)
            {
                data = resultDataOperationFiltter.GetResultDataFiltter(data);
            }
            return data;
        }

    }
    public class ResultDataOperationFiltter
    {
        OPERATIONTYPE operationType;
        int soAmount;
        public ResultDataOperationFiltter(OPERATIONTYPE operationType, int soAmount)
        {
            this.operationType = operationType;
            this.soAmount = soAmount;
        }

        public ResultData GetResultDataFiltter(ResultData resultData)
        {
            int finalAmount = 0;
            switch (operationType)
            {
                case OPERATIONTYPE.SIMPLE:
                    finalAmount = resultData.amount;
                    break;
                case OPERATIONTYPE.ADITION:
                    finalAmount = resultData.amount + soAmount;
                    break;
                case OPERATIONTYPE.SUBSTRACTION:
                    finalAmount = resultData.amount - soAmount;
                    break;
                case OPERATIONTYPE.DIVISION:
                    if (resultData.amount == 0 || soAmount == 0)
                    {
                        finalAmount = 0;
                        break;
                    }
                    finalAmount = resultData.amount / soAmount;
                    break;
                case OPERATIONTYPE.MULTIPLICATION:
                    finalAmount = resultData.amount * soAmount;
                    break;
                case OPERATIONTYPE.PERCENT:
                    finalAmount = soAmount * resultData.amount / 100;
                    break;
                default:
                    break;
            }
            resultData.amount = finalAmount;
            return resultData;
        }
    }
    public class ResultDataValidator
    {
        ComparatorFiltter comparatorFiltter;
        IResultData rdToCheck;
        IResultData rdToCheckAgainst;

        public ResultDataValidator(COMPARATIONTYPE filtter, IResultData rdToCheck, IResultData rdToCheckAgainst)
        {
            this.rdToCheck = rdToCheck;
            this.rdToCheckAgainst = rdToCheckAgainst;
            comparatorFiltter = new ComparatorFiltter(filtter);
        }

        public bool IsValid()
        {
            return comparatorFiltter.IsValidComparation(rdToCheck, rdToCheckAgainst);
        }
    }
}
