using UnityEngine;

namespace PositionerDemo
{
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
}
