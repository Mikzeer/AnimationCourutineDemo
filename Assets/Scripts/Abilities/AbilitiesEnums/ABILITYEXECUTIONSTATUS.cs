namespace PositionerDemo
{
    public enum ABILITYEXECUTIONSTATUS
    {
        WAIT,
        EXECUTED, //=> para saber si se ejecuto
        STARTED,
        CANCELED //=> para saber si se cancelo y no se debe ejecutar => Click incorrecto... Algun Modifier que la cancele
    };

}
