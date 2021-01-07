using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class Player : IOcuppy
    {
        #region VARIABLES

        private Animator playerAnimator;
        private Transform playerTransform;

        private int _playerID;
        public int PlayerID { get => _playerID; private set => _playerID = value; }

        bool isAlly;
        public bool IsAlly { get => isAlly; protected set => isAlly = value; }

        Dictionary<int, Stat> stats;
        public Dictionary<int, Stat> Stats { get => stats; protected set => stats = value; }


        Dictionary<STATTYPE, Stat> sttats;
        public Dictionary<STATTYPE, Stat> Sttats { get => sttats; protected set => sttats = value; }

        Dictionary<int, AbilityAction> abilities;
        public Dictionary<int, AbilityAction> Abilities { get => abilities; protected set => abilities = value; }

        Dictionary<ABILITYTYPE, AbilityAction> ability;
        public Dictionary<ABILITYTYPE, AbilityAction> Abilityes { get => ability; protected set => ability = value; }

        private OCUPPIERTYPE _occupierType;
        public OCUPPIERTYPE OccupierType { get => _occupierType; protected set => _occupierType = value; }

        private CARDTARGETTYPE _cardTargetType;
        public CARDTARGETTYPE CardTargetType { get => _cardTargetType; protected set => _cardTargetType = value; }

        public PLAYERTYPE playerType { get; set; }

        private List<Kimboko> kimbokoUnits = new List<Kimboko>();

        private Dictionary<IOcuppy, GameObject> _unitsPrefabs;
        public Dictionary<IOcuppy, GameObject> UnitsPrefabs { get => _unitsPrefabs; protected set => _unitsPrefabs = value; }

        private Stack<Card> _deck;
        public Stack<Card> Deck { get { return _deck; } set { _deck = value; } }
        private List<Card> _playersHands;
        public List<Card> PlayersHands { get { return _playersHands; } private set { _playersHands = value; } }
        private List<Card> _graveyard;
        public List<Card> Graveyard { get { return _graveyard; } private set { _graveyard = value; } }

        #endregion

        public Player(int PlayerID, PLAYERTYPE playerType, Stack<Card> Deck)
        {
            this.Deck = Deck;
            this.PlayerID = PlayerID;
            this.playerType = playerType;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
            PlayersHands = new List<Card>();
            Graveyard = new List<Card>();

            Abilities = new Dictionary<int, AbilityAction>();
            SpawnAbility spawnAbility = new SpawnAbility(this);
            TakeCardAbility takeCardAbility = new TakeCardAbility(this);
            Abilities.Add(spawnAbility.ID, spawnAbility);
            Abilities.Add(takeCardAbility.ID, takeCardAbility);

            AttackPowerStat attackPow = new AttackPowerStat(2, 2);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(2, 2);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats = new Dictionary<int, Stat>();
            Stats.Add(attackPow.id, attackPow);
            Stats.Add(attackRan.id, attackRan);
            Stats.Add(healthStat.id, healthStat);
            Stats.Add(actionPStat.id, actionPStat);

            TurnManager.OnPlayerResetActionPoints += ResetActionPoints;
        }

        public Player(int PlayerID, PLAYERTYPE playerType)
        {
            this.PlayerID = PlayerID;
            this.playerType = playerType;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
            PlayersHands = new List<Card>();
            Graveyard = new List<Card>();

            Abilities = new Dictionary<int, AbilityAction>();
            SpawnAbility spawnAbility = new SpawnAbility(this);
            TakeCardAbility takeCardAbility = new TakeCardAbility(this);
            Abilities.Add(spawnAbility.ID, spawnAbility);
            Abilities.Add(takeCardAbility.ID, takeCardAbility);

            AttackPowerStat attackPow = new AttackPowerStat(2, 2);
            AttackRangeStat attackRan = new AttackRangeStat(1, 3);
            HealthStat healthStat = new HealthStat(2, 2);
            ActionPointStat actionPStat = new ActionPointStat(2, 2);

            Stats = new Dictionary<int, Stat>();
            Stats.Add(attackPow.id, attackPow);
            Stats.Add(attackRan.id, attackRan);
            Stats.Add(healthStat.id, healthStat);
            Stats.Add(actionPStat.id, actionPStat);

            TurnManager.OnPlayerResetActionPoints += ResetActionPoints;
        }

        public Player(int PlayerID)
        {
            Abilityes = new Dictionary<ABILITYTYPE, AbilityAction>();
            Sttats = new Dictionary<STATTYPE, Stat>();
            this.PlayerID = PlayerID;
            this.playerType = PLAYERTYPE.PLAYER;
            OccupierType = OCUPPIERTYPE.PLAYER;
            CardTargetType = CARDTARGETTYPE.BASENEXO;
            PlayersHands = new List<Card>();
            Graveyard = new List<Card>();
        }

        public void SetStatsAndAbilities(Dictionary<ABILITYTYPE, AbilityAction> Ability, Dictionary<STATTYPE, Stat> Sttats)
        {
            this.Abilityes = Ability;
            this.Sttats = Sttats;
        }

        public void SetDeck(Stack<Card> Deck)
        {
            this.Deck = Deck;
        }

        public void OnSelect(bool isSelected, int playerID)
        {
            Debug.Log("PLAYER SELECTED");
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

        public int GetCurrentActionPoints()
        {
            int actionPoints = 0;

            if (stats != null)
            {
                if (stats.ContainsKey(4))
                {
                    actionPoints = stats[4].ActualStatValue;
                }
            }
            else
            {
                if (sttats.ContainsKey(STATTYPE.ACTIONPOINTS))
                {
                    actionPoints = sttats[STATTYPE.ACTIONPOINTS].ActualStatValue;
                }
            }

            return actionPoints;
        }

        public void ResetActionPoints(int playerID, int amount)
        {
            if (PlayerID != playerID) return;

            if (Stats.ContainsKey(4))
            {
                StatModification statModification = new StatModification(this, Stats[4], amount, STATMODIFIERTYPE.CHANGE);
                Stats[4].AddStatModifier(statModification);
                Stats[4].ApplyModifications();
            }
        }

        public IOcuppy GetOcuppy()
        {
            return this;
        }

        public Ability GetAbility(ABILITYTYPE abilityType)
        {
            if (Abilityes.ContainsKey(abilityType)) return Abilityes[abilityType];
            return null;
        }
    }        
}
