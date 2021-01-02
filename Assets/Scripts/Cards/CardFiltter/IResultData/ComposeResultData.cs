using System.Collections.Generic;

namespace PositionerDemo
{
    public abstract class ComposeResultData
    {
        ResultDataOperationFiltter rdoFiltter;
        List<ResultDataValidator> dataValidators;

        public ComposeResultData(List<ResultDataValidator> dataValidators = null, ResultDataOperationFiltter rdoFiltter = null)
        {

        }
    }
}
