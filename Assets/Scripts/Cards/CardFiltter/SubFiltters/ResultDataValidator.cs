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

        public ResultDataValidator(COMPARATIONTYPE filtter, IResultData rdToCheckAgainst)
        {
            this.rdToCheckAgainst = rdToCheckAgainst;
            comparatorFiltter = new ComparatorFiltter(filtter);
        }

        public ResultDataValidator(IResultData rdToCheck)
        {
            this.rdToCheck = rdToCheck;
        }

        public void SetCompouse(COMPARATIONTYPE filtter,IResultData rdToCheckAgainst)
        {
            this.rdToCheckAgainst = rdToCheckAgainst;
            comparatorFiltter = new ComparatorFiltter(filtter);
        }

        public void SetCompouse(IResultData rdToCheck)
        {
            this.rdToCheck = rdToCheck;
        }

        public bool IsValid()
        {
            return comparatorFiltter.IsValidComparation(rdToCheck, rdToCheckAgainst);
        }
    }

}
