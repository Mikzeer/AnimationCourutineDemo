using UnityEngine;
using UnityEngine.UI;
namespace MikzeerGame
{
    namespace UI
    {
        public class InformationBanner : MonoBehaviour
        {
            [SerializeField] private RectTransform rectTransform = default;
            [SerializeField] private Text txtBannerInformation = default;

            public void SetBannerTextInformation(string txtBannerInfo)
            {
                txtBannerInformation.text = txtBannerInfo;
            }

            public RectTransform GetRectTransform() => rectTransform;
        }
    }
}