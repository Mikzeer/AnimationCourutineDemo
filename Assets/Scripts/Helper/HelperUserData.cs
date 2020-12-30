using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System;

public static class HelperUserData
{
    public static string GetMacAddress()
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

    public static string GetLocalIPAddress()
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

    public static string GenerateRandomCryptographicKeySalt(int keyLengthSize)
    {
        // EL KEY LENGTH ES LA CANTIDAD DE BYTES QUE VAMOS A UTILIZAR PARA NUESTRA SALT
        // LO IDEAL ES QUE LA MISMA CANTIDAD DE BYTES DEL TEXTO DE LA CLAVE SEA PARA LA SALT
        // Return a Base64 string representation of the random number.
        return Convert.ToBase64String(GenerateRandomCryptographicBytesSalt(keyLengthSize));
    }

    public static byte[] GenerateRandomCryptographicBytesSalt(int keyLengthSize)
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

    public static HashWithSaltResult HashWithSalt(string password, int saltLength, HashAlgorithm hashAlgo)
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

    public static bool isCorrectPassword(string saltDeLaBDOnline, string passHashDeLaBDOnline, string passQueElUserIntrodujoAhora, HashAlgorithm hashAlgo)
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
        hashWithSaltBytes.AddRange(digestBytes);
        // Append salt bytes to the result.
        hashWithSaltBytes.AddRange(saltBytes);
        // Convert result into a base64-encoded string.
        string hashValue = Convert.ToBase64String(hashWithSaltBytes.ToArray());

        // comparo el string de la bd con el que creamos segun la Salt guardad en la BD y el pass transformado en bytes que puso el user
        if (string.Equals(passHashDeLaBDOnline, hashValue))
        {
            return true;
        }

        return false;
    }

    public static bool isCorrectPassword(string passHashDeLaBDOnline, string hashValue)
    {
        // comparo el string de la bd con el que creamos segun la Salt guardad en la BD y el pass transformado en bytes que puso el user
        if (string.Equals(passHashDeLaBDOnline, hashValue))
        {
            return true;
        }

        return false;
    }

    public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
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

    public static bool Equality(byte[] a1, byte[] b1)
    {
        int i;
        if (a1.Length == b1.Length)
        {
            i = 0;
            while (i < a1.Length && (a1[i] == b1[i])) //Earlier it was a1[i]!=b1[i]
            {
                i++;
            }
            if (i == a1.Length)
            {
                return true;
            }
        }

        return false;
    }
}
