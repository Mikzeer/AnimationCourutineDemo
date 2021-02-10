using CommandPatternActions;
using UnityEngine;

namespace PositionerDemo
{
    public class TestAbilityModifier : TimeEventAbilityModifier
    {
        private const int ABILITYMODIFIFERID = 15;
        private const int MODIFEREXECUTIIONORDER = 0;
        Player player;
        SpawnAbilityEventInfo info;
        public TestAbilityModifier(Player performerIOcuppy) : base(ABILITYMODIFIFERID, MODIFEREXECUTIIONORDER, performerIOcuppy)
        {
            executionTime = ABILITYMODIFIEREXECUTIONTIME.LATE;
            this.player = performerIOcuppy;
        }

        public override void Enter()
        {
            SpawnAbility.OnActionStartExecute += OnEnemySpawnExecute;
        }

        public override void RestDuration()
        {
            SpawnAbility.OnActionStartExecute -= OnEnemySpawnExecute;
        }

        public override void Execute(IAbility abilityAction)
        {

            if (info.spawnerPlayer.kimbokoUnits.Count > 0)
            {
                Debug.Log("Es mayor a zero");
            }
            else
            {
                Debug.Log("Es zero");
                //return;
            }
            

            // POR UN LADO LE CAMBIO EL TIPO 
            Debug.Log("El Enemigo Spawneo un bichito " + info.spawnUnitType);
            info.spawnUnitType = UNITTYPE.Z;

            // Y POR OTRO LADO LA CANCELO
            if (abilityAction.AbilityType == ABILITYTYPE.SPAWN)
            {
                SpawnAbility ab = (SpawnAbility)abilityAction;
                if (ab != null)
                {
                    Debug.Log("CANCEL SPAWN RIVAL");
                    abilityAction.actionStatus = ABILITYEXECUTIONSTATUS.CANCELED;
                    RestDuration();
                }
            }

            // EXPIRO EL MODIFIER
            Invoker.AddNewCommand(ExpireCmd());

        }

        private void OnEnemySpawnExecute(SpawnAbilityEventInfo info)
        {
            // GUARDO LA INFO PARA EL MOMENTO DE LA EJECUCION
            this.info = info;
            if (info.spawnerPlayer.OwnerPlayerID != player.OwnerPlayerID)
            {
                Invoker.AddNewCommand(ExecuteCmd(info.spawnerPlayer.Abilities[ABILITYTYPE.SPAWN]));
                Invoker.ExecuteCommands();
            }           
        }
    }
}
