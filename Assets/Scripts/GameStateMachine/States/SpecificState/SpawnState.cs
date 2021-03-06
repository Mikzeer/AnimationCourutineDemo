﻿using AbilitySelectionUI;
using PositionerDemo;
using System.Collections.Generic;

namespace StateMachinePattern
{
    public class SpawnState : SubSelectionState<Tile>
    {
        #region blablabla
        // SELECT => SPAWN / ATTACK / COMBINE / DECOMBINE / MOVE / USE CARD
        // NORMAL STATE => TURN STATE / ADMIN STATE
        // SPECIAL STATE => WAIT FOR ANIMATION STATE // WAIT STATE // ABILITY RESOLUTION STATE
        // CHAIN STATE => SELECT => CHAIN START STATE 
        //             => SELECT => CHAIN CARD SELECTION STATE
        //             => SPECIAL => CHAIN DEFINITION STATE
        #endregion
        GameMachine gmMachine;
        SpawnAbilitySelectionUIContainer spawnUIContainer;
        public SpawnState(GameMachine game, IState previousState, SpawnAbilitySelectionUIContainer spawnUIContainer) : base(game, previousState)
        {
            gmMachine = game;
            this.spawnUIContainer = spawnUIContainer;
            // MOVE => Pinto Las Tiles
            // ATTACK => Selecciono las tiles a atacar y marco a los enemigos como van a quedar de vida ?
            // SPAWN => Pinto las tiles y marco a las unidades combinables
        }

        public override void SetSelection(Tile selection)
        {
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in spawnUIContainer.tileHighlightTypesDictionary)
            {
                if (item.Key == selection)
                {
                    // ESTO EN UN FUTURO SOLO DEBERIA SER EL ON TRY SPAWN ENVIANDOLO AL SERVER
                    if (game.spawnManager.CanISpawn(selection, game.turnController.CurrentPlayerTurn))
                    {
                        game.spawnManager.OnSpawn(selection, game.turnController.CurrentPlayerTurn);
                    }
                    break;
                }
            }
            // EL ON NEXT STATE TAMBIEN DEBERIA SER UN CMD O UN INTENTO DE CAMBIAR DE STATE QUE ENVIAMOS AL SERVER
            // O EL SERVER SOLO SE DEBERIA ENCARGAR SIEMPRE DE CAMBIAR LOS STATES
            OnNextState(previousState);
        }

        public override void OnEnter()
        {
            //Debug.Log("ENTER SPAWN STATE");
            gmMachine.tileSelectionManagerUI.onTileSelected += SetSelection;
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in spawnUIContainer.tileHighlightTypesDictionary)
            {
                gmMachine.abilitySelectionManagerUI.HighlightTile(item.Key, item.Value);
            }
        }

        public override void OnExit()
        {
            //Debug.Log("EXIT SPAWN STATE");
            gmMachine.tileSelectionManagerUI.onTileSelected -= SetSelection;
            foreach (KeyValuePair<Tile, HIGHLIGHTUITYPE> item in spawnUIContainer.tileHighlightTypesDictionary)
            {
                gmMachine.abilitySelectionManagerUI.HighlightTile(item.Key, HIGHLIGHTUITYPE.NONE);
            }
        }

        public override void OnUpdate()
        {
            //Debug.Log("IN SPAWN STATE");
            base.OnUpdate();
        }

    }
}
