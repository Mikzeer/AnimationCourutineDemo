using System.Collections.Generic;

namespace PositionerDemo
{
    public static class OccupierAbilityDatabase
    {
        public static Dictionary<ABILITYTYPE, IAbility> CreatePlayerAbilities(Player player)
        {
            Dictionary<ABILITYTYPE, IAbility> Abilities = new Dictionary<ABILITYTYPE, IAbility>();
            SpawnAbility spawnAbility = new SpawnAbility(player);
            TakeCardAbility takeCardAbility = new TakeCardAbility(player);
            Abilities.Add(spawnAbility.AbilityType, spawnAbility);
            Abilities.Add(takeCardAbility.AbilityType, takeCardAbility);
            return Abilities;
        }
    }
}