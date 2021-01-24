using PositionerDemo;
using System.Collections.Generic;
using UnityEngine;

namespace MikzeerGame
{
    namespace UI
    {
        public class OcuppierInformationPanel : MonoBehaviour
        {
            [SerializeField] private GameObject ocuppierStatInfoPrefab = default;
            [SerializeField] private RectTransform playerOneInfoPanelParent = default;
            [SerializeField] private RectTransform playerTwoInfoPanelParent = default;
            private List<GameObject> statInfoUICreated = new List<GameObject>();
            private IOcuppy actualSelectedOcuppier;

            public void CreateOcuppierStatInformationPanel(IOcuppy ocuppier)
            {
                if (ocuppier == null)
                {
                    actualSelectedOcuppier = null;
                    ClearStatInforPanel();
                    return;
                }

                if (actualSelectedOcuppier != null)
                {
                    if (actualSelectedOcuppier == ocuppier)
                    {
                        return;
                    }
                }
                ClearStatInforPanel();
                actualSelectedOcuppier = ocuppier;

                foreach (var pair in ocuppier.Stats)
                {
                    OcuppierStatsInformationUI statInfoUI = GetInformationStatInfo(ocuppier.OwnerPlayerID);
                    OcuppierStatInfo statInfo = new OcuppierStatInfo(pair.Value);
                    statInfoUI.SetOcuppierStatInformation(statInfo);
                    statInfoUICreated.Add(statInfoUI.gameObject);
                }
                if (ocuppier.OwnerPlayerID == 0)
                {
                    playerOneInfoPanelParent.gameObject.SetActive(true);
                }
                else
                {
                    playerTwoInfoPanelParent.gameObject.SetActive(true);
                }

            }

            private void ClearStatInforPanel()
            {
                playerOneInfoPanelParent.gameObject.SetActive(false);
                playerTwoInfoPanelParent.gameObject.SetActive(false);
                for (int i = statInfoUICreated.Count - 1; i >= 0; i--)
                {
                    Destroy(statInfoUICreated[i]);
                    statInfoUICreated.Remove(statInfoUICreated[i]);
                }
                statInfoUICreated.Clear();
            }

            private OcuppierStatsInformationUI GetInformationStatInfo(int playerID)
            {
                GameObject ocuppierStatInfoAux = Instantiate(ocuppierStatInfoPrefab);
                if (playerID == 0)
                {
                    ocuppierStatInfoAux.transform.SetParent(playerOneInfoPanelParent, false);
                }
                else
                {
                    ocuppierStatInfoAux.transform.SetParent(playerTwoInfoPanelParent, false);
                }
                OcuppierStatsInformationUI aux = ocuppierStatInfoAux.GetComponent<OcuppierStatsInformationUI>();
                return aux;
            }
        }
    }
}