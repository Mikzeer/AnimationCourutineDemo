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

        public static Dictionary<ABILITYTYPE, IAbility> CreateKimbokoAbilities(Kimboko kimboko)
        {
            Dictionary<ABILITYTYPE, IAbility> Abilities = new Dictionary<ABILITYTYPE, IAbility>();
            MoveAbility moveAbility = new MoveAbility(kimboko);
            Abilities.Add(moveAbility.AbilityType, moveAbility);
            CombineAbility combineAbility = new CombineAbility(kimboko);
            Abilities.Add(combineAbility.AbilityType, combineAbility);
            return Abilities;
        }
    }
}