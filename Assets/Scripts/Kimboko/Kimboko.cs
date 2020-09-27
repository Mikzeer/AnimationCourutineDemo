using UnityEngine;


namespace PositionerDemo
{
    namespace Kimboko
    {
        public abstract class Kimboko : IOcuppy
        {
            protected Animator kimbokoAnimator;
            protected Transform kimbokoTransform;

            private int _id;
            public int ID { get => _id; protected set => _id = value; }

            private UNITTYPE _unitType;
            public UNITTYPE UnitType { get => _unitType; protected set => _unitType = value; }
            private OCUPPIERTYPE _occupierType;
            public OCUPPIERTYPE OccupierType { get => _occupierType; protected set => _occupierType = value; }

            private MOVEDIRECTIONTYPE _moveDirectionerType;
            public MOVEDIRECTIONTYPE MoveDirectionerType { get => _moveDirectionerType; protected set => _moveDirectionerType = value; }
            private CARDTARGETTYPE _cardTargetType;
            public CARDTARGETTYPE CardTargetType { get => _cardTargetType; protected set => _cardTargetType = value; }

            public int PosX;
            public int PosY;

            public Player ownerPlayer { get; set; }
            public int ActionPoints { get; set; }

            public Kimboko(int ID, Player ownerPlayer)
            {
                this.ID = ID;
                this.ownerPlayer = ownerPlayer;
                CardTargetType = CARDTARGETTYPE.UNIT;
                OccupierType = OCUPPIERTYPE.UNIT;

                this.UnitType = UnitType;
                this.MoveDirectionerType = MoveDirectionerType;
            }

            public void OnSelect(bool isSelected, int playerID)
            {
                Debug.Log("KIMBOKO SELECTED");
            }

            public void SetGameObject(GameObject kimbokoPrefab)
            {
                kimbokoAnimator = kimbokoPrefab.GetComponent<Animator>();
                kimbokoTransform = kimbokoPrefab.transform;
            }

            public Transform GetTransform()
            {
                return kimbokoTransform;
            }

            public Animator GetAnimator()
            {
                return kimbokoAnimator;
            }

            public void SetActionPoints(int apPoints)
            {
                ActionPoints = apPoints;
            }

            public void RestActionPoints(int apPoints)
            {
                ActionPoints -= apPoints;
            }

            public void DestroyPrefab()
            {
                ownerPlayer.RemoveUnit(this);
                MonoBehaviour.Destroy(kimbokoTransform.gameObject);
            }
        }
    }

}