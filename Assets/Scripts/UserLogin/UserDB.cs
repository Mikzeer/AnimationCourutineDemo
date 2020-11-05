using System.Collections.Generic;
using System;

[Serializable]
public class UserDB
{
    public string ID;
    public string Name;
    public string Macaddress;
    public string Salt;
    public string Password;
    public string LocalIP;

    public UserDB()
    {

    }

    public UserDB(string Name, string Password)
    {
        this.Name = Name;
        this.Password = Password;
    }

    public UserDB(string Name, string Macaddress, string Salt, string Password)
    {
        this.Name = Name;
        this.Macaddress = Macaddress;
        this.Salt = Salt;
        this.Password = Password;
    }

    public UserDB(string Name, string Macaddress, string Salt, string Password, string ID)
    {
        this.Name = Name;
        this.Macaddress = Macaddress;
        this.Salt = Salt;
        this.Password = Password;
        this.ID = ID;
    }

    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["ID"] = ID;
        result["Name"] = Name;
        result["Macaddress"] = Macaddress;
        result["Salt"] = Salt;
        result["Password"] = Password;

        return result;
    }
}
