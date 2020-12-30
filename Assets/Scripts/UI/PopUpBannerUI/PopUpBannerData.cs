using System;

[Serializable]
public class PopUpBannerData
{
    public string title { get; set; }
    public string description { get; set; }

    public PopUpBannerData(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}
