using System.Collections.Generic;

namespace PositionerDemo
{
    public interface IResultData
    {
        RESULTDATATYPE resultDataType { get; }
        ResultData GetResultData();
    }

    public abstract class ComposeResultData
    {
        ResultDataOperationFiltter rdoFiltter;
        List<ResultDataValidator> dataValidators;

        public ComposeResultData(List<ResultDataValidator> dataValidators = null, ResultDataOperationFiltter rdoFiltter = null)
        {

        }
    }
}
