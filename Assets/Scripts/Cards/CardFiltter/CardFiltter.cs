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

    public class Test
    {
        public void TestOne()
        {
            // CHECK UNIDAD CON STAT HP ACTUAL MAYOR O IGUAL A 5


            // 1 - CREAMOS Y SETEAMOS LOS TARGETS DESDE ALGUN LADO
            // 1A- A QUIEN/ES VAMOS A COMPRAR
            //KimbokoXFactory ki = new KimbokoXFactory();
            //Kimboko kimb = ki.CreateKimboko(0, new Player(0, PLAYERTYPE.PLAYER));

            //// 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            //StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(0);
            //if (statIDFiltter.IsValidStat(kimb) == false)
            //{
            //    Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
            //    return;
            //}
            //// 3 - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            //IResultData unit = new StatIResultData(0, STATAMOUNTTYPE.ACTUAL, kimb);
            //ResultData unitResultData = unit.GetResultData();

            //// 4 - OBTENEMOS LA RESULT DATA DEL COMPARE DATA SENCILLO
            //// ACA NO TENEMSO QUE CHEQUEAR NINGUN ID YA QUE ES DE TIPO SENCILLO
            //IResultData data = new SimpleIResultData(1);
            //ResultData simpleResultData = data.GetResultData();

            //// 4- COMPARAMOS LOS RESULTADOS FINALES
            //Comparator comparator = new Comparator();
            //bool isValid = comparator.IsValidComparation(COMPARATIONTYPE.GREATEROREQUAL, unitResultData, simpleResultData);

            //if (isValid)
            //{
            //    Debug.Log("is VALID COMPARE DATA");
            //}
            //else
            //{
            //    Debug.Log("INVALID COMPARE DATA");
            //}
        }
        public void TestTwo()
        {
            // CHECK HP ACTUAL DEL OCUPPIER ES MAYOR O IGUAL AL HP MAXIMO DE OTRO OCCUPIER

            // 1 - CREAMOS Y SETEAMOS LOS TARGETS DESDE ALGUN LADO
            // 1A- A QUIEN/ES VAMOS A COMPRAR
            //KimbokoXFactory ki = new KimbokoXFactory();
            //Kimboko kimb = ki.CreateKimboko(0, new Player(0, PLAYERTYPE.PLAYER));

            //Player player = new Player(0, PLAYERTYPE.PLAYER);

            //// 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            //StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(0);
            //if (statIDFiltter.IsValidStat(kimb) == false)
            //{
            //    Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
            //    return;
            //}
            //// 3 - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            //IResultData unit = new StatIResultData(0, STATAMOUNTTYPE.ACTUAL, kimb);
            //ResultData unitResultData = unit.GetResultData();


            //// 3b - CHEQUEAMOS SI EL PLAYER TIENE LA STAT REQUERIDA PARA BUSCAR 
            //StatComparisonIDFiltter playerStatIDFiltter = new StatComparisonIDFiltter(0);
            //if (playerStatIDFiltter.IsValidStat(player) == false)
            //{
            //    Debug.Log("NO SE ENCUENTRA ESTA ID/KEY DE STAT EN EL OCCUPIER");
            //    return;
            //}
            //IResultData data = new StatIResultData(0, STATAMOUNTTYPE.MAX, player);
            //ResultData playerResultData = data.GetResultData();

            //// 4- COMPARAMOS LOS RESULTADOS FINALES
            //Comparator comparator = new Comparator();
            //bool isValid = comparator.IsValidComparation(COMPARATIONTYPE.GREATEROREQUAL, unitResultData, playerResultData);

            //if (isValid)
            //{
            //    Debug.Log("is VALID COMPARE DATA");
            //}
            //else
            //{
            //    Debug.Log("INVALID COMPARE DATA");
            //}
        }
        public void TestThree()
        {
            // CHECK HP ACTUAL DEL OCUPPIER ES MAYOR O IGUAL AL 90% DE LA SUMA DE TODO EL HP MAXIMO DE LAS UNIDADES ENEMIGAS

            // 1 - CREAMOS O SETEAMOS LOS TARGETS/OCUPPIER PARA GENERAR LAS REVISIONES
            //KimbokoXFactory ki = new KimbokoXFactory();

            //// 1A - A QUIEN/ES VAMOS A COMPRAR 
            //Kimboko kimb = ki.CreateKimboko(0, new Player(0, PLAYERTYPE.PLAYER));
            //// 2B - CONTRA QUIEN/ES VAMOS A COMPRAR
            //Player player = new Player(0, PLAYERTYPE.PLAYER);
            //Kimboko kimbEnem = ki.CreateKimboko(2, new Player(0, PLAYERTYPE.PLAYER));
            //Kimboko kimbEnem2 = ki.CreateKimboko(3, new Player(0, PLAYERTYPE.PLAYER));

            //List<IOcuppy> ocuppiers = new List<IOcuppy>();
            //ocuppiers.Add(player);
            //ocuppiers.Add(kimbEnem);
            //ocuppiers.Add(kimbEnem2);

            //// 2 - CHEQUEAMOS SI TIENE LA STAT REQUERIDA PARA BUSCAR 
            //StatComparisonIDFiltter statIDFiltter = new StatComparisonIDFiltter(0);
            //if (statIDFiltter.IsValidStat(kimb) == false) return;

            //// 2b - OBTENEMOS LA RESULT DATA DE LA UNIDAD
            //IResultData unit = new StatIResultData(0, STATAMOUNTTYPE.ACTUAL, kimb); // hp actual de la unidad
            //ResultData unitResultData = unit.GetResultData();


            //// 3 - CHEQUEAMOS SI LOS ENEMIGOS TIENEN LA STAT REQUERIDA PARA BUSCAR 
            //StatComparisonIDFiltter enemiesStatIDFiltter = new StatComparisonIDFiltter(0);
            //List<IOcuppy> ocpyAux = new List<IOcuppy>();
            //for (int i = 0; i < ocuppiers.Count; i++)
            //{
            //    if (enemiesStatIDFiltter.IsValidStat(ocuppiers[i]) == true)
            //    {
            //        ocpyAux.Add(ocuppiers[i]);
            //    }
            //}

            //if (ocpyAux.Count == 0) return;


            //// 3b - OBTENEMOS EL RESULT DATA DE EL 90% DE LA SUMA DE LA VIDA MAXIMA DE LAS UNIDADES ENEMIGAS
            //ResultDataOperationFiltter specialOperation = new ResultDataOperationFiltter(OPERATIONTYPE.PERCENT, 90);
            //StatIResultData statComparisonData = new StatIResultData(0, STATAMOUNTTYPE.MAX);
            //IResultData specialStatComparisonData = new SpecialStatComparisonData(ocuppiers, specialOperation, statComparisonData);
            //ResultData enemiesResultData = specialStatComparisonData.GetResultData();

            //// 4- COMPARAMOS LOS RESULTADOS FINALES
            //Comparator comparator = new Comparator();
            //bool isValid = comparator.IsValidComparation(COMPARATIONTYPE.GREATEROREQUAL, unitResultData, enemiesResultData);

            //if (isValid)
            //{
            //    Debug.Log("is VALID COMPARE DATA");
            //}
            //else
            //{
            //    Debug.Log("INVALID COMPARE DATA");
            //}
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
            //KimbokoXFactory ki = new KimbokoXFactory();

            //// 2B - CONTRA QUIEN/ES VAMOS A COMPRAR
            //Player player = new Player(0, PLAYERTYPE.PLAYER);
            //Kimboko kimbEnem = ki.CreateKimboko(2, new Player(0, PLAYERTYPE.PLAYER));
            //Kimboko kimbEnem2 = ki.CreateKimboko(3, new Player(0, PLAYERTYPE.PLAYER));

            //List<IOcuppy> ocuppiers = new List<IOcuppy>();
            //ocuppiers.Add(player);
            //ocuppiers.Add(kimbEnem);
            //ocuppiers.Add(kimbEnem2);

            //// 3 - CHEQUEAMOS SI LOS ALIADOS TIENE LA ATK POW STAT
            //int targetStatID = 1;
            //int soAmount = 10;
            //StatComparisonIDFiltter alliesAtkPowStatIDFiltter = new StatComparisonIDFiltter(targetStatID);
            //List<IOcuppy> ocpyAux = new List<IOcuppy>();
            //for (int i = 0; i < ocuppiers.Count; i++)
            //{
            //    if (alliesAtkPowStatIDFiltter.IsValidStat(ocuppiers[i]) == true)
            //    {
            //        ocpyAux.Add(ocuppiers[i]);
            //    }
            //}

            //if (ocpyAux.Count == 0)
            //{
            //    return;
            //}

            //// 3b - OBTENEMOS EL RESULT DATA DE EL RESULTADO DE LA SUMA DEL 10 DEL ATK POW DE LOS ALIADOS
            //ResultDataOperationFiltter specialOperation = new ResultDataOperationFiltter(OPERATIONTYPE.PERCENT, soAmount);
            //StatIResultData statComparisonData = new StatIResultData(targetStatID, STATAMOUNTTYPE.ACTUAL);
            //IResultData specialStatComparisonData = new SpecialStatComparisonData(ocpyAux, specialOperation, statComparisonData);
            //ResultData alliesAtkPowResultData = specialStatComparisonData.GetResultData();


            //// 3 - CHEQUEAMOS SI LOS ALIADOS TIENE LA HP STAT
            //int HPtargetStatID = 0;
            //int soAmounthp = 2;
            //StatComparisonIDFiltter allieshPStatIDFiltter = new StatComparisonIDFiltter(HPtargetStatID);
            //List<IOcuppy> ocpyhPAux = new List<IOcuppy>();
            //for (int i = 0; i < ocuppiers.Count; i++)
            //{
            //    if (allieshPStatIDFiltter.IsValidStat(ocuppiers[i]) == true)
            //    {
            //        ocpyhPAux.Add(ocuppiers[i]);
            //    }
            //}

            //if (ocpyhPAux.Count == 0)
            //{
            //    return;
            //}

            //// 3b - OBTENEMOS EL RESULT DATA DE LA MULTIPLICACION POR 2 DE LA VIDA ACUTUAL DE LOS ALIADOS
            //ResultDataOperationFiltter specialOperationhp = new ResultDataOperationFiltter(OPERATIONTYPE.MULTIPLICATION, soAmounthp);
            //StatIResultData statComparisonDataHP = new StatIResultData(HPtargetStatID, STATAMOUNTTYPE.ACTUAL);
            //IResultData specialStatComparisonDataHP = new SpecialStatComparisonData(ocpyhPAux, specialOperationhp, statComparisonDataHP);
            //ResultData alliesHpResultData = specialStatComparisonDataHP.GetResultData();


            //// APLICAMOS LA OPERACION DE SUMAR 
            //// 10% DE LA SUMA DE TODO EL ATK POW ACTUAL STAT DE LAS UNIDADES ALIADAS
            //// + MAS
            //// 2 * LA SUMA DE TODO EL HP ACTUAL DE TODAS LAS UNIDADES ALIADAS
            //ResultDataOperationFiltter additionSpecOp = new ResultDataOperationFiltter(OPERATIONTYPE.ADITION, alliesAtkPowResultData.amount);
            //ResultData allAliesResultData = additionSpecOp.GetResultDataFiltter(alliesHpResultData);

        }
    }
}

namespace MikzeerGameNotas
{
    // SIMPLE COMPARISON DATA =>  SOLO UN NUMERO
    // SIMPLE OCUPPIER DATA => SOLO UN OCCUPIER
    // MULTIPLE OCCUPIER DATA => VARIOS OCCUPPIER
    // TILE DATA => UNA TILE EN EL CAMPO DE BATALLA O EL SPAWN
    // PODEMOS TENER DATA PRECARGADA, COMO UN RESULT DATA PRECARGADO DE UNA COMPARACION. EJ: SI ES >= 5(CINCO) 

    // TODAS LAS CLASES VAN A TERMINAR CON UN COMPARATOR SIEMPRE
    // Y EL COMPARATIONTYPE VA A SER ESPECIFICA Y UNICA EL FILTRO QUE ESTEMOS APLICANDO
    // EJ >= = ETC

    // SI TENEMOS UN COMPARATOR ENTONCES VAMOS A NECESITAR SI O SI DOS RESULT DATA
    // RESULTA DATA A COMPARAR
    // RESULTA DATA CONTRA QUE COMPARAR


    // 1 - CREAMOS O SETEAMOS LOS TARGETS/OCUPPIER PARA GENERAR LAS REVISIONES
    // 2 - CONTRA QUIEN/ES VAMOS A COMPRAR
    // 3a - Aplicamos Los Filtros + ComparatorFiltter
    // 3b - Aplicamos los ResultDataOperationFiltter
    // 3c - OBTENEMOS LA RESULT DATA 
    // 4- COMPARAMOS LOS RESULTADOS FINALES


}
