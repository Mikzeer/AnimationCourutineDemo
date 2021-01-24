using UnityEngine;
using UnityEngine.UI;

namespace MikzeerGame
{
    namespace UI
    {
        public class OcuppierStatsInformationUI : MonoBehaviour
        {
            [SerializeField] private Text txtStatName = default;
            [SerializeField] private Text txtActualStatValue = default;
            [SerializeField] private Text txtMaxStatValue = default;

            public void SetOcuppierStatInformation(OcuppierStatInfo ocuppierStatInfo)
            {
                txtStatName.text = ocuppierStatInfo.statName;
                txtActualStatValue.text = ocuppierStatInfo.actualStat;
                txtMaxStatValue.text = ocuppierStatInfo.maxStat;
            }

            public void ChangeOcuppierStatValues(OcuppierStatInfo ocuppierStatInfo)
            {
                txtActualStatValue.text = ocuppierStatInfo.actualStat;
                txtMaxStatValue.text = ocuppierStatInfo.maxStat;
            }
        }
    }
}