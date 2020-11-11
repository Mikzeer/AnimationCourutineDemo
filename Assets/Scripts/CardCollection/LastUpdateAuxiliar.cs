using System;

[Serializable]
public class LastUpdateAuxiliar
{
    public long uctCreatedUnix;

    public LastUpdateAuxiliar()
    {

    }

    public LastUpdateAuxiliar(long uctCreatedUnix)
    {
        this.uctCreatedUnix = uctCreatedUnix;
    }
}