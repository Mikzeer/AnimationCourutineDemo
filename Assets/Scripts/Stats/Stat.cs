using System.Collections.Generic;
using System.Linq;

namespace PositionerDemo
{
    public abstract class Stat
    {
        /*
         * Los Stats hacen referencia a un cualidad de alguna UNIT o PLAYER, como puede ser el PODER DE ATAQUE, SU CANTIDAD DE VIDA, etc
         * Cada stat tiene una forma de medirse y es por medio de su ACTUAL VALUE. Algunos stats pueden tener un MAX VALUE que es la cantidad
         * maxima a la que un stat puede llegar cuando es modificado.
         * Cada stat tiene un ID unico para cada uno... esto sirve para poder referenciar a que stat queremos afectar de una manera muy chota pero
         * rapida y sin tener que usar reflexion ni algun enum para el mismo caso        
        */

        private int actualStatValue; // Valor actual del stat
        public int ActualStatValue { get { return actualStatValue; } set { actualStatValue = value; } }
        private int maxStatValue; // Valor maximo del stat
        public int MaxStatValue { get { return maxStatValue; } protected set { maxStatValue = value; } }
        private int ID; // Una id para poder referenciar al stat mas facil sin uso de enums o reflexion 
        public int id { get { return ID; } protected set { ID = value; } }
        private STATTYPE statType;
        public STATTYPE StatType { get { return statType; } protected set { statType = value; } }

        private List<StatModification> unitsModifiersList;

        public Stat(int actualStatValue, int maxStatValue, int ID, STATTYPE statType)
        {
            this.ActualStatValue = actualStatValue;
            this.MaxStatValue = maxStatValue;
            this.id = ID;
            this.StatType = statType;
            unitsModifiersList = new List<StatModification>();
        }

        public void AddStatModifier(StatModification modifierToAdd)
        {
            unitsModifiersList.Add(modifierToAdd);
        }

        public void ApplyModifications()
        {
            foreach (StatModification mod in unitsModifiersList)
            {
                if (mod.status == STATMODIFICATIONSTATUS.Queued || mod.status == STATMODIFICATIONSTATUS.ExecuteFailed)
                {
                    mod.Execute();
                }
                if (mod.status == STATMODIFICATIONSTATUS.RevertFailed)
                {
                    mod.Revert();
                }
            }
        }

        public void RevertModifications(StatModification modifierToRevert)
        {
            foreach (StatModification mod in unitsModifiersList)
            {
                if (mod == modifierToRevert)
                {
                    mod.Revert();
                }
                if (mod.status == STATMODIFICATIONSTATUS.RevertSucceeded)
                {
                    unitsModifiersList.Remove(mod);
                }
            }
        }

        public void RevertModificationByID(int modificationID)
        {
            RevertModifications(unitsModifiersList.FirstOrDefault(x => x.actualModificationID == modificationID));
        }

    }
}
