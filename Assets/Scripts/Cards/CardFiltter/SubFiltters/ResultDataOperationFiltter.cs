namespace PositionerDemo
{
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
}
