using PositionerDemo;
using System.Collections.Generic;

namespace AbilitySelectionUI
{
    public class SpawnAbilitySelectionUIContainer : AbilitySelectinUIContainer
    {
        private const ABILITYSELECTIONTYPE ABILITY_SELECTION_TYPE = ABILITYSELECTIONTYPE.SIMPLE;
        public SpawnAbilitySelectionUIContainer(Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary) : base(tileHighlightTypesDictionary, ABILITY_SELECTION_TYPE)
        {
        }
    }
}
