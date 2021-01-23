using System;
using UnityEngine;
using UnityEngine.UI;

namespace PositionerDemo
{
    public class UIGeneralManagerInGame : MonoBehaviour
    {
        #region VARIABLES
        //[SerializeField] private TimePanelUI timePanelUI = default;
        [SerializeField] private Button btnEndTurn = default;
        public delegate void ClickEndTurnAction();
        public event ClickEndTurnAction OnEndTurnActionClicked = default;
        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            btnEndTurn.onClick.AddListener(OnBtnEndTurnClick);
            btnEndTurn.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            btnEndTurn.onClick.RemoveListener(OnBtnEndTurnClick);
        }

        #endregion

        public void SetTimer()
        {
            //GameTimer.OnTimeChange += UpdateTime;
        }

        private void UpdateTime(string obj)
        {
            //timePanelUI.SetTime(obj);
        }

        public void SetActiveEndTurnButton(bool isActive)
        {
            btnEndTurn.gameObject.SetActive(isActive);
        }

        #region BUTTON EVENT REGION

        void OnBtnEndTurnClick()
        {
            OnEndTurnActionClicked?.Invoke();
        }

        #endregion
    }
}