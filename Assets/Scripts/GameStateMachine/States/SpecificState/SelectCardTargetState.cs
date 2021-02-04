using AbilitySelectionUI;
using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachinePattern
{
    public class SelectCardTargetState : SubSelectionState<Tile>
    {
        // NO TIENE NADA HECHO, HAY QUE CAMBIAR TODO

        GameMachine gmMachine;
        IOcuppy mover;
        MoveAbilitySelectionUIContainer moveUIContainer;
        public SelectCardTargetState(GameMachine game, IState previousState, MoveAbilitySelectionUIContainer moveUIContainer, IOcuppy mover) : base(game, previousState)
        {
            gmMachine = game;
            this.moveUIContainer = moveUIContainer;
            this.mover = mover;
        }

        public override void SetSelection(Tile selection)
        {
            // LA SELECCION ES A DONDE ME VOY A MOVER
            // PERO TAMBIEN NECESITO QUIEN SE VA A MOVER....
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in moveUIContainer.tileHighlightTypesDictionary)
            {
                if (item.Key == selection)
                {
                    game.movementManager.OnTryMove(mover, selection);
                    break;
                }
            }
            OnNextState(previousState);
        }

        public override void OnEnter()
        {
            Debug.Log("ENTER CARD TARGET SELECTION STATE");
            gmMachine.tileSelectionManagerUI.onTileSelected += SetSelection;
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in moveUIContainer.tileHighlightTypesDictionary)
            {
                gmMachine.abilitySelectionManagerUI.HighlightTile(item.Key, item.Value);
            }
        }

        public override void OnExit()
        {
            Debug.Log("EXIT CARD TARGET SELECTION STATE");
            gmMachine.tileSelectionManagerUI.onTileSelected -= SetSelection;
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in moveUIContainer.tileHighlightTypesDictionary)
            {
                gmMachine.abilitySelectionManagerUI.HighlightTile(item.Key, HIGHLIGHTUITYPE.NONE);
            }
        }

        public override void OnUpdate()
        {
            Debug.Log("IN CARD TARGET SELECTION STATE");
            base.OnUpdate();
        }

    }
}
