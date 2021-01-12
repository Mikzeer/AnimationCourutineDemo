namespace PositionerDemo
{
    public enum ABILITYEXECUTIONSTATUS
    {
        WAIT,
        EXECUTED, //SUCCESS=> para saber si se ejecuto
        STARTED,
        CANCELED,//=> para saber si se cancelo y no se debe ejecutar => Click incorrecto... Algun Modifier que la cancele
        NONEXECUTABLE // PARA CUANDO ESTAMOS ESPERANDO AL TURNO DEL OTRO JUGADOR
    };

}
