namespace PositionerDemo
{
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
}
