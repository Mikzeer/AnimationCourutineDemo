using PositionerDemo;
using System.Collections.Generic;

namespace AbilitySelectionUI
{
    public class AbilitySelectinUIContainer
    {
        ABILITYSELECTIONTYPE selectionType;
        public Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary { get; protected set; }
        int totalSelectionRequirement;

        public AbilitySelectinUIContainer(Dictionary<Tile, HIGHLIGHTUITYPE> tileHighlightTypesDictionary, ABILITYSELECTIONTYPE selectionType, int totalSelectionRequiremen = 1)
        {
            this.selectionType = selectionType;
            this.tileHighlightTypesDictionary = tileHighlightTypesDictionary;
            this.totalSelectionRequirement = totalSelectionRequiremen;
        }
    }
}
