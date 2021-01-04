namespace PositionerDemo
{
    // STAT MODIFIER
    public class StatModification : IStatModification
    {
        // Estos son los MODIFICADORES de ACTUAL VALUE / MAX VALUE  de los stats
        IOcuppy OcuppyToModify; // ESTA ES LA UNIDAD A LA CUAL LE VAMOS A MODIFICAR EL STAT
        Stat statToModify; // ESTE ES EL STAT EN SI A MODIFICAR 
        public STATMODIFICATIONSTATUS status { get; set; } = STATMODIFICATIONSTATUS.Queued;
        public static int ModificationID;
        public int actualModificationID; // ESTA ES LA ID DE ESTA UNIT STAT MODIFICATION, SIRVE PARA ENCONTRARLA MAS FACILMENTE POR ID

        public STATMODIFIERTYPE statModifierType { get; private set; }
        int amountToModify;

        public StatModification(IOcuppy unitToModify, Stat statToModify, int amountToModify, STATMODIFIERTYPE statModifierType)
        {
            this.OcuppyToModify = unitToModify;
            this.statToModify = statToModify;
            this.statModifierType = statModifierType;
            if (statModifierType == STATMODIFIERTYPE.CHANGE)
            {
                int valorDeseado = amountToModify; //
                int valorDeStatActual = this.statToModify.ActualStatValue;
                int valorDelStatAAplicar = valorDeseado - valorDeStatActual;
                amountToModify = valorDelStatAAplicar;
            }

            this.amountToModify = amountToModify;

            actualModificationID = ModificationID;
            ModificationID++;
        }

        public void Execute()
        {
            if (OcuppyToModify == null) return; status = STATMODIFICATIONSTATUS.ExecuteFailed;
            if (statToModify == null) return; status = STATMODIFICATIONSTATUS.ExecuteFailed;

            statToModify.ActualStatValue += amountToModify;
            //  5 ++ 1 = 6 buff / 5 +- 1 = 4 nerf / 11 +- 8 = 3 change
            status = STATMODIFICATIONSTATUS.ExecuteSucceeded;
        }

        public void Revert()
        {
            if (OcuppyToModify == null) return; status = STATMODIFICATIONSTATUS.RevertFailed;
            if (statToModify == null) return; status = STATMODIFICATIONSTATUS.RevertFailed;

            statToModify.ActualStatValue -= amountToModify;
            // 6 -+ 1 = 5 buff / 4 -- 1 = 5 nerf / 3 -- 8 = 11 change

            status = STATMODIFICATIONSTATUS.RevertSucceeded;
        }
    }

}
