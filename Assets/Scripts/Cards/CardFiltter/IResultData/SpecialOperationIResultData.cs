namespace PositionerDemo
{
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
}
