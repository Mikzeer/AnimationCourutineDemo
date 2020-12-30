using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigurationData
{
    public bool autoLogin;
    public bool cardVisibles;
    public UserDB user;
    public string email;
    public string password;
    public Deck selectedDeck;
}
