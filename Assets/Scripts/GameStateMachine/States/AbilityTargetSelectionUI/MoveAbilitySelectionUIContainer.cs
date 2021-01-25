using PositionerDemo;
using System.Collections.Generic;

namespace AbilitySelectionUI
{
    public class MoveAbilitySelectionUIContainer : AbilitySelectinUIContainer
    {
        private const ABILITYSELECTIONTYPE ABILITY_SELECTION_TYPE = ABILITYSELECTIONTYPE.SIMPLE;
        public MoveAbilitySelectionUIContainer(Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary) : base(tileHighlightTypesDictionary, ABILITY_SELECTION_TYPE)
        {
        }
    }
}
