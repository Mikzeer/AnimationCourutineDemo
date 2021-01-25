using AbilitySelectionUI;
using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachinePattern
{
    public class MoveState : SubSelectionState<Tile>
    {
        GameMachine gmMachine;
        IOcuppy mover; 
        MoveAbilitySelectionUIContainer moveUIContainer;
        public MoveState(GameMachine game, IState previousState, MoveAbilitySelectionUIContainer moveUIContainer, IOcuppy mover) : base(game, previousState)
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
            Debug.Log("ENTER MOVE STATE");
            gmMachine.tileSelectionManagerUI.onTileSelected += SetSelection;
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in moveUIContainer.tileHighlightTypesDictionary)
            {
                gmMachine.abilitySelectionManagerUI.HighlightTile(item.Key, item.Value);
            }
        }

        public override void OnExit()
        {
            Debug.Log("EXIT MOVE STATE");
            gmMachine.tileSelectionManagerUI.onTileSelected -= SetSelection;
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in moveUIContainer.tileHighlightTypesDictionary)
            {
                gmMachine.abilitySelectionManagerUI.HighlightTile(item.Key, HIGHLIGHTUITYPE.NONE);
            }
        }

        public override void OnUpdate()
        {
            Debug.Log("IN MOVE STATE");
            base.OnUpdate();
        }

    }
}
