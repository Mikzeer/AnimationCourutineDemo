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

    //[NonSerialized]
    //public DateTime utcDownloadCollection; // LAS CARD COLLECTION UPDATE
    //[NonSerialized]
    //public DateTime utcDownloadOwnedCards; // LAS CARD COLLECTION UPDATE

    public UserDB()
    {
        this.IsFirstTime = true;
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

    //public UserDB(Dictionary<string, object> fromFirebaseResult)
    //{
    //    ID = fromFirebaseResult.ContainsKey("ID") ? fromFirebaseResult.First(x => x.Key == "ID").Value.ToString() : string.Empty;
    //    Name = fromFirebaseResult.ContainsKey("Name") ? fromFirebaseResult.First(x => x.Key == "Name").Value.ToString() : string.Empty;
    //    Macaddress = fromFirebaseResult.ContainsKey("Macaddress") ? fromFirebaseResult.First(x => x.Key == "Macaddress").Value.ToString() : string.Empty;
    //    Salt = fromFirebaseResult.ContainsKey("Salt") ? fromFirebaseResult.First(x => x.Key == "Salt").Value.ToString() : string.Empty;
    //    Password = fromFirebaseResult.ContainsKey("Password") ? fromFirebaseResult.First(x => x.Key == "Password").Value.ToString() : string.Empty;

    //    if (fromFirebaseResult.ContainsKey("utcLastDownloadGameCollectionUnix"))
    //    {
    //        long milliseconds;
    //        if (long.TryParse(fromFirebaseResult.First(x => x.Key == "utcLastDownloadGameCollectionUnix").Value.ToString(), out milliseconds))
    //        {
    //            utcLastDownloadGameCollectionUnix = milliseconds;

    //            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    //            utcDownloadCollection = epoch.AddMilliseconds(milliseconds);
    //        }
    //    }

    //    if (fromFirebaseResult.ContainsKey("utcLastDownloadUserCollectionUnix"))
    //    {
    //        long milliseconds;
    //        if (long.TryParse(fromFirebaseResult.First(x => x.Key == "utcLastDownloadUserCollectionUnix").Value.ToString(), out milliseconds))
    //        {
    //            utcLastDownloadUserCollectionUnix = milliseconds;

    //            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    //            utcDownloadOwnedCards = epoch.AddMilliseconds(milliseconds);
    //        }
    //    }
    //}
}
