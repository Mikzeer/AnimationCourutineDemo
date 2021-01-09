using System.Collections.Generic;

namespace PositionerDemo
{
    public static class OccupierAbilityDatabase
    {
        public static Dictionary<ABILITYTYPE, AbilityAction> CreatePlayerAbilities(Player player)
        {
            Dictionary<ABILITYTYPE, AbilityAction> Abilities = new Dictionary<ABILITYTYPE, AbilityAction>();
            SpawnAbility spawnAbility = new SpawnAbility(player);
            TakeCardAbility takeCardAbility = new TakeCardAbility(player);
            Abilities.Add(spawnAbility.AbilityType, spawnAbility);
            Abilities.Add(takeCardAbility.AbilityType, takeCardAbility);
            return Abilities;
        }
    }
}