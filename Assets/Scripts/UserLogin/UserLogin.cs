using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System;
using Proyecto26;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Threading.Tasks;

public class UserLogin : MonoBehaviour
{
    [SerializeField] private Text txtUserNameRegister;
    [SerializeField] private Text txtPassRegister;
    [SerializeField] private Text txtRePassRegister;

    [SerializeField] private Text txtUserNameLogin;
    [SerializeField] private Text txtPassLogin;

    private const string projectId = "kimbokotooltest"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";

    private FirebaseDatabase fbDB;
    DatabaseReference reference;

    private void Awake()
    {
        //Debug.Log("MacAddress: " + GetMacAddress());
        //Debug.Log("LocalIPAddress: " + GetLocalIPAddress());

        string plaintext = "Mikzeer";

        string passwordHashMD5 = ComputeHash(plaintext, "MD5", null);
        string passwordHashSha1 = ComputeHash(plaintext, "SHA1", null);
        string passwordHashSha256 = ComputeHash(plaintext, "SHA256", null);
        string passwordHashSha384 = ComputeHash(plaintext, "SHA384", null);
        string passwordHashSha512 = ComputeHash(plaintext, "SHA512", null);

        //Debug.Log("Original String   : " + plaintext);
        //Debug.Log("Hash values :\r\n");
        //Debug.Log("MD5   : "+ passwordHashMD5);
        //Debug.Log("SHA1  : "+ passwordHashSha1);
        //Debug.Log("SHA256: "+ passwordHashSha256);
        //Debug.Log("SHA384: "+ passwordHashSha384);
        //Debug.Log("SHA512: "+ passwordHashSha512);
    }

    private void Start()
    {
        //await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    if (task.Exception != null)
        //    {
        //        Debug.Log("Error INIRIALIZAIN DATABASE");
        //    }

        //    fbDB = FirebaseDatabase.DefaultInstance;
        //});

        //bool x = await SaveExist();

        //Debug.Log("Save " + x);

        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;




    }

    public async void TestCreateUserButton()
    {
        await CreateNewUser();
    }

    public async Task CreateNewUser()
    {
        bool isDataCorrect = await CheckIfDataIsCorrect();
        if (isDataCorrect)
        {
            string userName = txtUserNameRegister.text;
            string userPass = txtPassRegister.text;
            string macAddres = GetMacAddress();
            string localIP = GetLocalIPAddress();

            HashWithSaltResult hashSalt = HashWithSalt(userPass, 32, new SHA256Managed());
            string pSalt = hashSalt.Salt;
            string hasPass = hashSalt.Digest;

            UserDB userDB = new UserDB(userName, macAddres, pSalt, hasPass);

            //Every time you call Push(), Firebase generates a unique key that can also be used as a unique identifier,
            // such as user-scores/<user-id>/<unique-score-id>.
            string key = reference.Child("Users").Push().Key;
            userDB.ID = key;
            string json = JsonUtility.ToJson(userDB);
            
            //PostUserWithRestApi(userDB);
            PostUserWithFireBaseRT(key, json);
        }
    }

    public async void TestButton()
    {
        await GetValueTest();
    }

    public async void TestBtnLogin()
    {
        UserDB logedUser = await NewLoginUserData();

        if (logedUser == null)
        {
            Debug.Log("No User");
        }
        else
        {
            Debug.Log("Login SUccesfull");
        }
    }

    public async Task<UserDB> NewLoginUserData()
    {
        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        DataSnapshot userNameExist = await PlayerDataSnapshotNameExist(txtUserNameLogin.text);
        UserDB logedUser = null;
        if (userNameExist != null)
        {
            if (userNameExist.Exists)
            {
                UserDB user = JsonUtility.FromJson<UserDB>(userNameExist.GetRawJsonValue());
                if (isCorrectPassword(user.Salt, user.Password, txtPassLogin.text, new SHA256Managed()))
                {
                    Debug.Log("PASSWORD CORRECT");
                    logedUser = user;
                }
                else
                {
                    Debug.Log("PASSWORD INCORRECT");
                }
            }
            else
            {
                Debug.Log("NAME INCORRECT");
            }
        }

        return logedUser;
    }

    public async Task GetValueTest()
    {
        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task => 
        {
          if (task.IsFaulted)
          {
                Debug.Log("NoChild");
                  // Handle the error...
          }
          else if (task.IsCompleted)
          {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...

                foreach (var child in snapshot.Children)
                {
                    Debug.Log("The Key" + child.Key);
                    if (child.HasChild("Name"))
                    {
                        Debug.Log("NAME  FAUND");
                        if (child.Child("Name").Value.ToString() == "Lakitu")
                        {
                            Debug.Log("LAKITU FAUND");
                            UserDB user = JsonUtility.FromJson<UserDB>(child.GetRawJsonValue());
                            string json = JsonUtility.ToJson(user);

                            Debug.Log(json);
                        }
                    }                    
                }
            }
        });

    }

    private async Task<bool> NameExist(string name)
    {
        bool exist = false;
        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {                
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    if (child.HasChild("Name"))
                    {
                        if (child.Child("Name").Value.ToString().ToLower() == name.ToLower())
                        {
                            Debug.Log("NAME FAUND");
                            exist = true;
                        }
                    }
                }
            }
        });

        return exist;
    }

    private async Task<DataSnapshot> PlayerDataSnapshotNameExist(string name)
    {
        bool exist = false;
        DataSnapshot dtSnapshot = null;
        await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("NoChild");
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    if (child.HasChild("Name"))
                    {
                        if (child.Child("Name").Value.ToString().ToLower() == name.ToLower())
                        {
                            Debug.Log("NAME FAUND");
                            exist = true;
                            dtSnapshot = child;
                        }
                    }
                }
            }
        });

        return dtSnapshot;
    }

    private async Task<bool> SaveExist()
    {
        var dataSnapshot = await fbDB.GetReference("Users").GetValueAsync();

        return dataSnapshot.Exists;
    }

    private async Task<bool> CheckIfDataIsCorrect()
    {
        if (txtUserNameRegister.text == "" || txtUserNameRegister.text == string.Empty)
        {
            Debug.Log("No user Name");
            return false;
        }

        // CHEQUEAR SI HAY ALGUN CARACTER QUE NO SE PERMITE
        // CHEQUEAR QUE NO SEA UN NOMBRE OFENSIVO DE USUARIO



        // CHEQUEAR SI HAY UN USUARIO CON EL MISMO NOMBRE
        bool userNameExist = await NameExist(txtUserNameRegister.text);
        if (userNameExist)
        {
            Debug.Log("User Name Not Valid");
            return false;
        }

        if (txtPassRegister.text == "" || txtPassRegister.text == string.Empty)
        {
            Debug.Log("No Password");
            return false;
        }

        if (txtRePassRegister.text == "" || txtRePassRegister.text == string.Empty)
        {
            Debug.Log("No RePassword");
            return false;
        }

        if (txtRePassRegister.text != txtPassRegister.text)
        {
            Debug.Log("Your Password and Repassword must be the same");
            return false;
        }

        return true;
    }

    private void PostUserWithRestApi(UserDB user)
    {
        string key = reference.Child("Users").Push().Key;
        RestClient.Put<UserDB>($"{databaseURL}Users/{key}.json", user);
        //RestClient.Put<UserDB>($"{databaseURL}Users/.json", user);
    }

    private void PostUserWithFireBaseRT(string key, string json)
    {
        reference.Child("Users").Child(key).SetRawJsonValueAsync(json);
    }

    private string GetMacAddress()
    {
        string physicalAddress = "";

        NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface adaper in nice)
        {

            //Debug.Log("Adapter Data: " + adaper.Description);

            if (adaper.Description == "en0")
            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();
                break;
            }
            else
            {
                physicalAddress = adaper.GetPhysicalAddress().ToString();

                if (physicalAddress != "")
                {
                    break;
                };
            }
        }

        return physicalAddress;
    }

    private string GetLocalIPAddress()
    {
        if (NetworkInterface.GetIsNetworkAvailable() == false)
        {
            return string.Empty;
        }
        
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    private string GenerateRandomCryptographicKeySalt(int keyLengthSize)
    {
        // EL KEY LENGTH ES LA CANTIDAD DE BYTES QUE VAMOS A UTILIZAR PARA NUESTRA SALT
        // LO IDEAL ES QUE LA MISMA CANTIDAD DE BYTES DEL TEXTO DE LA CLAVE SEA PARA LA SALT
        // Return a Base64 string representation of the random number.
        return Convert.ToBase64String(GenerateRandomCryptographicBytesSalt(keyLengthSize));
    }

    private byte[] GenerateRandomCryptographicBytesSalt(int keyLengthSize)
    {
        //A cryptographically secure pseudorandom number generator(CSPRNG) is an algorithm that produces a pseudorandom sequence of bytes.
        // RNGCryptoServiceProvider() whose sole job is to generate random numbers.
        RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        byte[] randomBytes = new byte[keyLengthSize];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        // Fill the salt with cryptographically strong byte values.
        //rngCryptoServiceProvider.GetNonZeroBytes(randomBytes);
        return randomBytes;
    }

    private HashWithSaltResult HashWithSalt(string password, int saltLength, HashAlgorithm hashAlgo)
    {
        // genero una Salt nueva segun la saltLength que queremos
        byte[] saltBytes = GenerateRandomCryptographicBytesSalt(saltLength);
        // trnasformo el password en bytes
        byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
        // creo una lista y la agrego el array de PASS + arra de SALT
        List<byte> passwordWithSaltBytes = new List<byte>();
        passwordWithSaltBytes.AddRange(passwordAsBytes);
        passwordWithSaltBytes.AddRange(saltBytes);
        // segun el algoritmo deseado creo el array de bytes con el PASS + SALT
        byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());

        // Create list which will hold hash and original salt bytes.
        List<byte> hashWithSaltBytes = new List<byte>();
        // Copy hash bytes into resulting list.
        hashWithSaltBytes.AddRange(digestBytes);
        // Append salt bytes to the result.
        hashWithSaltBytes.AddRange(saltBytes);
        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes.ToArray());

        // devuelvo una clase que tiene el string SALT y el string del HASHPASS + SALT
        return new HashWithSaltResult(Convert.ToBase64String(saltBytes), hashValue);
    }

    private bool isCorrectPassword(string saltDeLaBDOnline, string passHashDeLaBDOnline, string passQueElUserIntrodujoAhora, HashAlgorithm hashAlgo)
    {
        // trnasformo el password y la salt en bytes
        byte[] passwordAsBytes = Encoding.UTF8.GetBytes(passQueElUserIntrodujoAhora);
        //byte[] saltBytes = Encoding.UTF8.GetBytes(saltDeLaBDOnline);
        byte[] saltBytes = Convert.FromBase64String(saltDeLaBDOnline);
        // creo una lista y la agrego el array de PASS + arra de SALT
        List<byte> passwordWithSaltBytes = new List<byte>();
        passwordWithSaltBytes.AddRange(passwordAsBytes);
        passwordWithSaltBytes.AddRange(saltBytes);

        // segun el algoritmo deseado creo el array de bytes con el PASS + SALT
        byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());

        // convierto ese hash en string
        //string hashSaltQueIntrodujoElUsuario = Convert.ToBase64String(digestBytes);

        // Create list which will hold hash and original salt bytes.
        List<byte> hashWithSaltBytes = new List<byte>();
        // Copy hash bytes into resulting list.
        passwordWithSaltBytes.AddRange(digestBytes);
        // Append salt bytes to the result.
        passwordWithSaltBytes.AddRange(saltBytes);
        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes.ToArray());

        // comparo el string de la bd con el que creamos segun la Salt guardad en la BD y el pass transformado en bytes que puso el user
        if (string.Equals(passHashDeLaBDOnline, hashValue))
        {
            return true;
        }

        return false;
    }

    private string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
    {
        // If salt is not specified, generate it on the fly.
        if (saltBytes == null)
        {
            // Define min and max salt sizes.
            int minSaltSize = 4;
            int maxSaltSize = 8;

            // Generate a random number for the size of the salt.
            System.Random random = new System.Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);

            // Allocate a byte array, which will hold the salt.
            saltBytes = new byte[saltSize];

            // Initialize a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            // Fill the salt with cryptographically strong byte values.
            rng.GetNonZeroBytes(saltBytes);
        }
        // Convert plain text into a byte array.
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        // Allocate array, which will hold plain text and salt.
        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
        // Copy plain text bytes into resulting array.
        for (int i = 0; i < plainTextBytes.Length; i++)
            plainTextWithSaltBytes[i] = plainTextBytes[i];
        // Append salt bytes to the resulting array.
        for (int i = 0; i < saltBytes.Length; i++)
            plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

        HashAlgorithm hash;
        // Make sure hashing algorithm name is specified.
        if (hashAlgorithm == null)
            hashAlgorithm = "";
        // Initialize appropriate hashing algorithm class.
        switch (hashAlgorithm.ToUpper())
        {
            case "SHA1":
                hash = new SHA1Managed();
                break;
            case "SHA256":
                hash = new SHA256Managed();
                break;
            case "SHA384":
                hash = new SHA384Managed();
                break;
            case "SHA512":
                hash = new SHA512Managed();
                break;
            default:
                hash = new MD5CryptoServiceProvider();
                break;
        }

        // Compute hash value of our plain text with appended salt.
        byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

        // Create array which will hold hash and original salt bytes.
        byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                            saltBytes.Length];

        // Copy hash bytes into resulting array.
        for (int i = 0; i < hashBytes.Length; i++)
            hashWithSaltBytes[i] = hashBytes[i];

        // Append salt bytes to the result.
        for (int i = 0; i < saltBytes.Length; i++)
            hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes);

        // Return the result.
        return hashValue;
    }

}

public class HashWithSaltResult
{
    public string Salt { get; }
    public string Digest { get; set; }

    public HashWithSaltResult(string salt, string digest)
    {
        Salt = salt;
        Digest = digest;
    }
}

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