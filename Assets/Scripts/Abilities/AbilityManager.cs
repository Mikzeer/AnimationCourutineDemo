using CommandPatternActions;
using System.Collections.Generic;

namespace PositionerDemo
{
    public abstract class AbilityManager
    {
        // PARA EARLY/LATE MODIFIERS, ANTES DE EJECUTAR Y DESPUES DE EJECUTADA        
        private void ActionModifierCheck(IAbility ability, ABILITYMODIFIEREXECUTIONTIME executionTime) 
        {
            if (ability.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            List<AbilityModifier> abilityModifier = ability.GetAbilityModifierOrderByExecution();
            for (int i = 0; i < abilityModifier.Count; i++)
            {
                if (abilityModifier[i].executionTime == executionTime)
                {
                    Invoker.AddNewCommand(abilityModifier[i].ExecuteCmd(ability));
                    Invoker.ExecuteCommands();
                    if (ability.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
                }
            }
        }
        // EVENTO 1 - OnActionStartExecute
        private void OnStartExecute(IAbility ability)
        {
            ability.OnStartExecute();
        }
        // EVENTO 2 - OnActionEndExecute
        private void OnEndExecute(IAbility ability)
        {
            ability.OnEndExecute();
        }
        // LE RESTA AL PERFORMER LOS PUNTOS POR HABER USADO LA HABILIDAD
        private void RestActionPoints(IOcuppy performer, int actionPoints)
        {
            Invoker.AddNewCommand(new IRestActionPointsCommand(performer, actionPoints));
            Invoker.ExecuteCommands();
        }

        public void AddAbilityModifier(AbilityModifier modifier, IAbility ability)
        {
            Invoker.AddNewCommand(new IAddAbilityActionModifierCommand(ability, modifier));
            Invoker.ExecuteCommands();
        }

        public void RemoveAbilityModifier(AbilityModifier modifier, IAbility ability)
        {
            Invoker.AddNewCommand(new IRemoveAbilityActionModifierCommand(ability, modifier));
            Invoker.ExecuteCommands();
        }

        public void StartPerform(IAbility ability)
        {
            if (ability.CanIExecute() == false) return;
            ActionModifierCheck(ability, ABILITYMODIFIEREXECUTIONTIME.EARLY);
            if (ability.CanIExecute() == false) return;
            ability.OnStartExecute();
        }

        public void EndPerform(IAbility ability)
        {
            ActionModifierCheck(ability, ABILITYMODIFIEREXECUTIONTIME.LATE);
            RestActionPoints(ability.performerIOcuppy, ability.GetActionPointsRequiredToUseAbility());
            ability.OnEndExecute();
        }

        public void Perform(IAbility ability)
        {
            //if (ability.CanIExecute())
            //{
            //    //if (ability.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            //    //ActionModifierCheck(ability, ABILITYMODIFIEREXECUTIONTIME.EARLY);
            //    //if (ability.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            //    //ability.OnStartExecute();
            //    //if (ability.actionStatus == ABILITYEXECUTIONSTATUS.CANCELED) return;
            //    ability.Execute();

            //    // ACA DEBERIA GENERAR EL COMANDO DONDE SE EJECUTA LA HABILIDAD

            //    // Y LUEGO APLICAR TODOS LOS END MODIFIERS PARA QUE TENGA SENTIDO

            //    // Y LUEGO RESTAR LOS PUNTOS

            //    // Y LUEGO EJECUTAR EL EVENTO DONDE INFORMO A TODOS LOS QUE NECESITEN QUE TERMINE DE EJECUTARME

            //    //ActionModifierCheck(ability, ABILITYMODIFIEREXECUTIONTIME.LATE);
            //    //RestActionPoints(ability.performerIOcuppy, ability.GetActionPointsRequiredToUseAbility());
            //    //ability.OnEndExecute();
            //}
        }
    }
}