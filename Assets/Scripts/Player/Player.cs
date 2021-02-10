using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Player : Occupier
    {
        #region VARIABLES
        public int PlayerID { get; private set; }
        public PLAYERTYPE playerType { get; set; }
        public List<Kimboko> kimbokoUnits { get; private set; }
        public Stack<Card> Deck { get; set; }
        public List<Card> PlayersHands { get; private set; }
        public List<Card> Graveyard { get; private set; }
        // ACA ESTAN TODOS LOS MODIFIERS QUE SE ACTIVAN CON ALGUN EVENTO ESPECIFICO
        public List<AbilityModifier> GeneralModifiers { get; private set;}
        #endregion

        public Player(int PlayerID)
        {
            MoveDirectionerType = MOVEDIRECTIONTYPE.NONE;
            kimbokoUnits = new List<Kimboko>();
            GeneralModifiers = new List<AbilityModifier>();
            Abilities = new Dictionary<ABILITYTYPE, IAbility>();
            Stats = new Dictionary<STATTYPE, Stat>();
            this.PlayerID = PlayerID;
            OwnerPlayerID = PlayerID;
            this.playerType = PLAYERTYPE.PLAYER;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
            PlayersHands = new List<Card>();
            Graveyard = new List<Card>();
        }
       
        public void SetStatsAndAbilities(Dictionary<ABILITYTYPE, IAbility> Ability, Dictionary<STATTYPE, Stat> Sttats)
        {
            this.Abilities = Ability;
            this.Stats = Sttats;
        }

        public void AddUnit(Kimboko kimbokoUnit)
        {
            kimbokoUnits.Add(kimbokoUnit);
        }

        public void RemoveUnit(Kimboko kimbokoUnit)
        {
            kimbokoUnits.Remove(kimbokoUnit);
        }
    }
    
    public enum PLAYERPLACE
    {
        LEFT,
        RIGHT,
        NONE
    }
}