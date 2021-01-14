using CommandPatternActions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class AbilityAction
    {
        // Las ACTION son diferentes habilidades con las cuales puede contar todos los ACTORS del juego
        public ABILITYTYPE AbilityType { get; private set; }
        private int actionPointsRequired; // Cantidad de Action Points que requiera la accion para ejecutarse
        public IOcuppy performerIOcuppy { get; private set; }

        public ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        public List<AbilityModifier> abilityModifier { get; private set; }

        public AbilityAction(IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE abilityType)
        {
            this.performerIOcuppy = performerIOcuppy;
            this.actionPointsRequired = actionPointsRequired;
            abilityModifier = new List<AbilityModifier>();
            this.AbilityType = abilityType;
        }

        public abstract bool CanIExecute();// Cuando queremos ver si podemos ejecutar esta accion CHEQUEA SI SE TIENE LOS AP NECESARIOS Y CUALQUIER COSA RELATIVA A LA ACTION EN SI        
        private void StartActionModifierCheck() // Cuando empezamos a ejecuta esta accion y chequeamos los modificadores de accion que se ejecutan al inicio de la ejecucion de la accion StartActionModifier
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            abilityModifier = abilityModifier.OrderBy(c => c.ModifierExecutionOrder).ToList();
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == ABILITYMODIFIEREXECUTIONTIME.EARLY)
                {
                    Invoker.AddNewCommand(abilityModifier[i].ExecuteCmd(this));
                    Invoker.ExecuteCommands();
                    if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                }
            }
        }
        public abstract void OnStartExecute(); // Cuando empezamos a ejecutar esta accion // EVENTO 1 - OnActionStartExecute
        public abstract void Execute();// Cuando ejecutamos esta accion // Esto es porpio de cada accion // ACA SE CREA EL CMD         
        private void EndActionModifierCheck()// Cuando terminamos de ejecutar esta accion y chequeamos los modificadores de accion que se ejecutan al final de la ejecucion de la accion EndActionModifier
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            abilityModifier = abilityModifier.OrderBy(c => c.ModifierExecutionOrder).ToList();
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == ABILITYMODIFIEREXECUTIONTIME.LATE)
                {
                    Invoker.AddNewCommand(abilityModifier[i].ExecuteCmd(this));
                    Invoker.ExecuteCommands();
                }
            }
        }
        public abstract void OnEndExecute();// Cuando terminamos de ejecutar esta accion // EVENTO 2 - OnActionEndExecute

        public void Perform()
        {
            if (CanIExecute())
            {
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                StartActionModifierCheck();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                OnStartExecute();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                Execute();

                // ACA DEBERIA GENERAR EL COMANDO DONDE SE EJECUTA LA HABILIDAD

                // Y LUEGO APLICAR TODOS LOS END MODIFIERS PARA QUE TENGA SENTIDO

                // Y LUEGO RESTAR LOS PUNTOS

                // Y LUEGO EJECUTAR EL EVENTO DONDE INFORMO A TODOS LOS QUE NECESITEN QUE TERMINE DE EJECUTARME

                EndActionModifierCheck();
                RestActionPoints();
                OnEndExecute();
            }
        }

        //resta la cantidad de action points requeridos por la accion
        private void RestActionPoints()
        {
            Invoker.AddNewCommand(new IRestActionPointsCommand(performerIOcuppy, actionPointsRequired));
            Invoker.ExecuteCommands();
        }

        public void AddAbilityModifier(AbilityModifier modifier)
        {
            Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(this, modifier));
            Invoker.ExecuteCommands();
        }

        public void RemoveAbilityModifier(AbilityModifier modifier)
        {
            Invoker.AddNewCommand(new IRemoveAbilityActionModifierCommand(this, modifier));
            Invoker.ExecuteCommands();
        }

        public int GetActionPointsRequiredToUseAbility() => actionPointsRequired;

        public bool IsModifierApply(int modifierID)
        {
            if (abilityModifier.Count == 0) return false;
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].ID == modifierID)
                {
                    return true;
                }
            }
            return false;
        }

    }

    public abstract class Abilityy
    {
        public ABILITYTYPE AbilityType { get; private set; }
        public IOcuppy performerIOcuppy { get; private set; }
        private int actionPointsRequired; 

        public ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        public List<AbilityModifier> abilityModifier { get; private set; }

        public Abilityy(IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE abilityType)
        {
            this.performerIOcuppy = performerIOcuppy;
            this.actionPointsRequired = actionPointsRequired;
            abilityModifier = new List<AbilityModifier>();
            this.AbilityType = abilityType;
        }

        // Cuando queremos ver si podemos ejecutar esta accion CHEQUEA SI SE TIENE LOS AP NECESARIOS Y CUALQUIER COSA RELATIVA A LA ACTION EN SI
        public abstract bool CanIExecute();

    }

    public class AbilityManager
    {
        GameTestController gameTestController;
        public AbilityManager(GameTestController gameTestController)
        {
            this.gameTestController = gameTestController;            
        }
    }
}