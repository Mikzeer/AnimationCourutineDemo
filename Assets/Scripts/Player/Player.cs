using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Player : Occupier
    {
        #region VARIABLES
        private Animator playerAnimator;
        private Transform playerTransform;
        public int PlayerID { get; private set; }
        public PLAYERTYPE playerType { get; set; }
        private List<Kimboko> kimbokoUnits = new List<Kimboko>();
        public Dictionary<IOcuppy, GameObject> UnitsPrefabs { get; protected set; }
        public Stack<Card> Deck { get; set; }
        public List<Card> PlayersHands { get; private set; }
        public List<Card> Graveyard { get; private set; }
        #endregion

        public Player(int PlayerID)
        {
            Abilities = new Dictionary<ABILITYTYPE, AbilityAction>();
            Stats = new Dictionary<STATTYPE, Stat>();
            this.PlayerID = PlayerID;
            this.playerType = PLAYERTYPE.PLAYER;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
            PlayersHands = new List<Card>();
            Graveyard = new List<Card>();

            TurnManager.OnPlayerResetActionPoints += ResetActionPoints;
        }

        public override void OnSelect(bool isSelected, int playerID)
        {
            Debug.Log("PLAYER SELECTED");
        }
       
        public void SetStatsAndAbilities(Dictionary<ABILITYTYPE, AbilityAction> Ability, Dictionary<STATTYPE, Stat> Sttats)
        {
            this.Abilities = Ability;
            this.Stats = Sttats;
        }

        public void SetDeck(Stack<Card> Deck)
        {
            this.Deck = Deck;
        }

        public void SetGameObject(GameObject playerPrefab)
        {
            playerAnimator = playerPrefab.GetComponent<Animator>();
            playerTransform = playerPrefab.transform;
        }

        public Transform GetTransform()
        {
            return playerTransform;
        }

        public Animator GetAnimator()
        {
            return playerAnimator;
        }

        public void AddUnit(Kimboko kimbokoUnit)
        {
            kimbokoUnits.Add(kimbokoUnit);
        }

        public void RemoveUnit(Kimboko kimbokoUnit)
        {
            kimbokoUnits.Remove(kimbokoUnit);
        }

        public Ability GetAbility(ABILITYTYPE abilityType)
        {
            if (Abilities.ContainsKey(abilityType)) return Abilities[abilityType];
            return null;
        }
    }        
}