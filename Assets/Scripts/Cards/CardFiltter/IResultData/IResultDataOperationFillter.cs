namespace PositionerDemo
{
    public interface IResultDataOperationFillter : IResultData
    {
        ResultDataOperationFiltter resultDataOperationFiltter { get; }
        void SetOperationFiltter(ResultDataOperationFiltter rdoFiltter);
    }
}
