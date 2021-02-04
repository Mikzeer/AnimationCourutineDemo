using System;

[Serializable]
public class ConfigurationData
{
    public bool autoLogin;
    public UserDB user;
    public string email;
    public string password;
    public Deck selectedDeck;
}
