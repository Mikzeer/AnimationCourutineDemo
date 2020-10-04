namespace PositionerDemo
{
    public enum STATMODIFICATIONSTATUS
    {
        ExecuteFailed, // Para cuando le damos Execute y fallo
        ExecuteSucceeded, // Para cuando le damos Execute y se Ejecuto correctamente
        Queued, // Que todavia no se Ejecuto/Desejecuto
        RevertFailed, // Cuando intentamos revertirlo y no pudimos
        RevertSucceeded // Cuando intentamos revertirlo y se Revirtio correctamente
    }

}
