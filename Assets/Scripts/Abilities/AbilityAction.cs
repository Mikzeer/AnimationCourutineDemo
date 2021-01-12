using CommandPatternActions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PositionerDemo
{
    public abstract class AbilityAction
    {
        // Las ACTION son diferentes habilidades con las cuales puede contar todos los ACTORS del juego
        public int ID { get; private set; }
        public ABILITYTYPE AbilityType { get; private set; }
        private int actionPointsRequired; // Cantidad de Action Points que requiera la accion para ejecutarse
        public IOcuppy performerIOcuppy { get; private set; }

        public ABILITYEXECUTIONSTATUS actionStatus { get; set; }
        public List<AbilityModifier> abilityModifier { get; private set; }

        public AbilityAction(int ID, IOcuppy performerIOcuppy, int actionPointsRequired, ABILITYTYPE abilityType)
        {
            this.performerIOcuppy = performerIOcuppy;
            this.ID = ID;
            this.actionPointsRequired = actionPointsRequired;
            abilityModifier = new List<AbilityModifier>();
            this.AbilityType = abilityType;
        }

        public abstract bool OnTryExecute();// Cuando queremos ver si podemos ejecutar esta accion CHEQUEA SI SE TIENE LOS AP NECESARIOS Y CUALQUIER COSA RELATIVA A LA ACTION EN SI        
        private void StartActionModifierCheck() // Cuando empezamos a ejecuta esta accion y chequeamos los modificadores de accion que se ejecutan al inicio de la ejecucion de la accion StartActionModifier
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            abilityModifier.OrderBy(c => c.ModifierExecutionOrder);
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == ABILITYMODIFIEREXECUTIONTIME.EARLY)
                {
                    Invoker.AddNewCommand(abilityModifier[i].ExecuteCmd(this));
                    // a reahabilitar
                    //abilityModifier[i].Execute(this);
                    if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                }
            }
        }
        public abstract void OnStartExecute(); // Cuando empezamos a ejecutar esta accion // EVENTO 1 - OnActionStartExecute
        public abstract void Execute();// Cuando ejecutamos esta accion // Esto es porpio de cada accion // ACA SE CREA EL CMD         
        private void EndActionModifierCheck()// Cuando terminamos de ejecutar esta accion y chequeamos los modificadores de accion que se ejecutan al final de la ejecucion de la accion EndActionModifier
        {
            if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;

            abilityModifier.OrderBy(c => c.ModifierExecutionOrder);

            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == ABILITYMODIFIEREXECUTIONTIME.LATE)
                {
                    abilityModifier[i].Execute(this);
                }
            }
        }
        public abstract void OnEndExecute();// Cuando terminamos de ejecutar esta accion // EVENTO 2 - OnActionEndExecute

        public void Perform()
        {
            //OnResetActionExecution();
            if (OnTryExecute())
            {
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                StartActionModifierCheck();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                OnStartExecute();
                if (actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                Execute();
                EndActionModifierCheck();
                RestActionPoints();
                OnEndExecute();
            }
        }

        //resta la cantidad de action points requeridos por la accion
        private void RestActionPoints() => Invoker.AddNewCommand(new IRestActionPointsCommand(performerIOcuppy, actionPointsRequired));
        public void AddAbilityModifier(AbilityModifier modifier) => Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(this, modifier));
        public void RemoveAbilityModifier(AbilityModifier modifier) => Invoker.AddNewCommand(new IRemoveAbilityActionModifierCommand(this, modifier));
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
}