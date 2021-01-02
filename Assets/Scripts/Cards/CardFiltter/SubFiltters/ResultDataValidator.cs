namespace PositionerDemo
{
    public class ResultDataValidator
    {
        ComparatorFiltter comparatorFiltter;
        IResultData rdToCheck;
        IResultData rdToCheckAgainst;

        public ResultDataValidator(COMPARATIONTYPE filtter, IResultData rdToCheck, IResultData rdToCheckAgainst)
        {
            this.rdToCheck = rdToCheck;
            this.rdToCheckAgainst = rdToCheckAgainst;
            comparatorFiltter = new ComparatorFiltter(filtter);
        }

        public bool IsValid()
        {
            return comparatorFiltter.IsValidComparation(rdToCheck, rdToCheckAgainst);
        }
    }

}
