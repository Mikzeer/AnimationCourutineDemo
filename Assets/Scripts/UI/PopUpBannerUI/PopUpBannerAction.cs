using UnityEngine.Events;

public class PopUpBannerAction
{
    public UnityAction OnCancelButtonPress;
    public UnityAction OnAcceptButtonPress;

    public PopUpBannerAction(UnityAction OnCancelButtonPress, UnityAction OnAcceptButtonPress)
    {
        this.OnCancelButtonPress += OnCancelButtonPress;
        this.OnAcceptButtonPress += OnAcceptButtonPress;
    }
}