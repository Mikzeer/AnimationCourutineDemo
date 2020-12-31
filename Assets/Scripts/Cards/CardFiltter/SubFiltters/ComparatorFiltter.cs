namespace PositionerDemo
{
    public class ComparatorFiltter
    {
        COMPARATIONTYPE filtter;
        public ComparatorFiltter(COMPARATIONTYPE filtter)
        {
            this.filtter = filtter;
        }
        public bool IsValidComparation(IResultData Idata, IResultData IdataToChekAgainst)
        {
            bool isValid = false;
            ResultData data = Idata.GetResultData();
            ResultData dataToChekAgainst = IdataToChekAgainst.GetResultData();
            switch (filtter)
            {
                case COMPARATIONTYPE.LESS:
                    if (data.amount < dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.LESSOREQUAL:
                    if (data.amount <= dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.EQUEAL:
                    if (data.amount == dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.GREATER:
                    if (data.amount > dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.GREATEROREQUAL:
                    if (data.amount >= dataToChekAgainst.amount) isValid = true;
                    break;
                case COMPARATIONTYPE.DIFFERENT:
                    if (data.amount != dataToChekAgainst.amount) isValid = true;
                    break;
                default:
                    break;
            }
            return isValid;
        }
    }

}
