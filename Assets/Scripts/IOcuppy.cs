namespace PositionerDemo
{
    public interface IOcuppy
    {
        OCUPPIERTYPE OccupierType { get; }
        void OnSelect(bool isSelected, int playerID); // eSTE METODO LO VAMOS A DISPARAR CUANDO SELECCIONAMOS AL OCUPPY
        // SI ES UNA UNIDAD EN EL OnSelect vamos a ver si es nuestra o no
        // SI ES UN OBJETO EN EL OnSelect vamos a ver si es nuestro o no
        // SI ES UN PLAYER EN EL OnSelect vamos a ver si es nuestro o no
        // SI ES UNA TILE VACIA EN EL OnSelect vamos a chequear que es null el Ocuppier y no vamos a hacer un choto
        // IOccupy ? ActionAbility.Count > 0 
        // if ActionAbility.Contains(int AbilityID);
        // IOccupy.ActionAbility(AbilityID).Execute();
    }

}