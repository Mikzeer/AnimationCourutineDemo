using UnityEngine;


namespace PositionerDemo
{
    [CreateAssetMenu(fileName = "New Kimboko", menuName = "Kimboko/New Kimboko Properties")]
    public class KimbokoPropertiesScriptableObject : ScriptableObject
    {
        [SerializeField] private int actualAttackPower;
        [SerializeField] private int maxAttackPower;
        [SerializeField] private int actualAttackRange;
        [SerializeField] private int maxAttackRange;
        [SerializeField] private int actualMoveRange;
        [SerializeField] private int maxMoveRange;
        [SerializeField] private int actualHealth;
        [SerializeField] private int maxHealth;
        [SerializeField] private int actualActionPoints;
        [SerializeField] private int maxActionPoints;
        [SerializeField] private UNITTYPE unitType;
        [SerializeField] private MOVEDIRECTIONTYPE moveDirectionerType;

        public int ActualAttackPower { get { return actualAttackPower; } protected set { actualAttackPower = value; } }
        public int MaxAttackPower { get { return maxAttackPower; } protected set { maxAttackPower = value; } }
        public int ActualAttackRange { get { return actualAttackRange; } protected set { actualAttackRange = value; } }
        public int MaxAttackRange { get { return maxAttackRange; } protected set { maxAttackRange = value; } }
        public int ActualMoveRange { get { return actualMoveRange; } protected set { actualMoveRange = value; } }
        public int MaxMoveRange { get { return maxMoveRange; } protected set { maxMoveRange = value; } }
        public int ActualHealth { get { return actualHealth; } protected set { actualHealth = value; } }
        public int MaxHealth { get { return maxHealth; } protected set { maxHealth = value; } }
        public int ActualActionPoints { get { return actualActionPoints; } protected set { actualActionPoints = value; } }
        public int MaxActionPoints { get { return maxActionPoints; } protected set { maxActionPoints = value; } }

        public UNITTYPE UnitType { get { return unitType; } protected set { unitType = value; } }
        public MOVEDIRECTIONTYPE MoveDirectionerType { get { return moveDirectionerType; } protected set { moveDirectionerType = value; } }
    }

}