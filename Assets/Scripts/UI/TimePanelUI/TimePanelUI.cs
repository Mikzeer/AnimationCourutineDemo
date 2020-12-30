using UnityEngine;
using UnityEngine.UI;

public class TimePanelUI : MonoBehaviour
{
    public Text txtTime;

    public void SetTime(string actulaTime)
    {
        txtTime.text = actulaTime;
    }
}
