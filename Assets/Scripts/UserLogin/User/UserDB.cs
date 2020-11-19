using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class UserDB
{
    public string ID;
    public string Name;
    public string Macaddress;
    public string Salt;
    public string Password;
    public string LocalIP;
    public bool IsFirstTime;

    public long utcLastDownloadGameCollectionUnix;
    public long utcLastDownloadUserCollectionUnix;
    public long utcLastModificationUserCollectionUnix;
    public long utcLastDownloadCardLimitDataUnix;

    public UserDB()
    {
        this.IsFirstTime = true;
    }

    public UserDB(string Name)
    {
        this.Name = Name;
    }

    public UserDB(string Name, string Macaddress, string Salt, string Password)
    {
        this.Name = Name;
        this.Macaddress = Macaddress;
        this.Salt = Salt;
        this.Password = Password;
        this.IsFirstTime = true;
    }

    public UserDB(string Name, string Macaddress, string Salt, string Password, string ID)
    {
        this.Name = Name;
        this.Macaddress = Macaddress;
        this.Salt = Salt;
        this.Password = Password;
        this.ID = ID;
        this.IsFirstTime = true;
    }

    public UserDB(string Name, string Macaddress, string Salt, string Password, string ID, bool isFirstTime)
    {
        this.Name = Name;
        this.Macaddress = Macaddress;
        this.Salt = Salt;
        this.Password = Password;
        this.ID = ID;
        this.IsFirstTime = isFirstTime;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["ID"] = ID;
        result["Name"] = Name;
        result["Macaddress"] = Macaddress;
        result["Salt"] = Salt;
        result["Password"] = Password;
        result["IsFirstTime"] = IsFirstTime;

        return result;
    }
}
