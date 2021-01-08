using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class ToggleController : MonoBehaviour
    {
        [SerializeField] private Toggle tggMove;
        [SerializeField] private Toggle tggSpawn;
        [SerializeField] private Toggle tggAttack;
        STATETYPE _stateType;
        public STATETYPE StateType { get { return _stateType; } private set { _stateType = value; } }

        Dictionary<STATETYPE, Toggle> toggles = new Dictionary<STATETYPE, Toggle>();
        public enum STATETYPE
        {
            SPAWN,
            MOVE,
            ATTACK
        }

        private void Start()
        {
            toggles.Add(STATETYPE.SPAWN, tggSpawn);
            toggles.Add(STATETYPE.MOVE, tggMove);
            toggles.Add(STATETYPE.ATTACK, tggAttack);
        }

        private void OnEnable()
        {
            tggMove.onValueChanged.AddListener(OnMoveToggleChange);
            tggSpawn.onValueChanged.AddListener(OnSpawnToggleChange);
            tggAttack.onValueChanged.AddListener(OnAttackToggleChange);
        }

        private void OnDisable()
        {
            tggMove.onValueChanged.RemoveAllListeners();
            tggSpawn.onValueChanged.RemoveAllListeners();
            tggAttack.onValueChanged.RemoveAllListeners();
        }

        public void OnChangeStateType(STATETYPE _stateType)
        {
            foreach (KeyValuePair<STATETYPE, Toggle> stTgg in toggles)
            {
                if (stTgg.Key == _stateType) continue;
                stTgg.Value.isOn = false;
            }
            this._stateType = _stateType;
        }

        #region TOGGLES EVENTS

        public void OnMoveToggleChange(bool isOn)
        {
            if (isOn == true) OnChangeStateType(STATETYPE.MOVE);
        }

        public void OnSpawnToggleChange(bool isOn)
        {
            if (isOn == true) OnChangeStateType(STATETYPE.SPAWN);
        }

        public void OnAttackToggleChange(bool isOn)
        {
            if (isOn == true) OnChangeStateType(STATETYPE.ATTACK);
        }

        #endregion

    }
}