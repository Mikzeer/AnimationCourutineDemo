using System.Collections.Generic;

namespace PositionerDemo
{
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
}
