namespace PositionerDemo
{
    public interface IResultData
    {
        RESULTDATATYPE resultDataType { get; }
        ResultData GetResultData();
    }
}
