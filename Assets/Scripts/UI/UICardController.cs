using UnityEngine;
using UnityEngine.UI;

public class UICardController : MonoBehaviour
{
    public Image miniatureImage;

    public void SetCardUI(Sprite spriteMninature)
    {
        miniatureImage.sprite = spriteMninature;
    }
}
