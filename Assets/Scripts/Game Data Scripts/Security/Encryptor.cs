using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Security.Cryptography;

public static class Encryptor
{
    private static byte[] key;
    private static byte[] iv;

    public static string Encrypt(this string text)
    {
        return Convert.ToBase64String(EncryptStringToBytes_Aes(text, key, iv));
    }

    public static string Decrypt(this string text)
    {
        var bytes = Convert.FromBase64String(text);
        return DecryptStringFromBytes_Aes(bytes, key, iv);
    }

    private static byte[] EncryptStringToBytes_Aes(string text, byte[] key, byte[] iv)
    {
        if (text == null || text.Length <= 0)
        {
            throw new ArgumentNullException("text");
        }
        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("iv");
        }

        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }

                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        return encrypted;
    }

    private static string DecryptStringFromBytes_Aes(byte[] text, byte[] key, byte[] iv)
    {
        if (text == null || text.Length <= 0)
        {
            throw new ArgumentNullException("text");
        }
        if (key == null || key.Length <= 0)
        {
            throw new ArgumentNullException("key");
        }
        if (iv == null || iv.Length <= 0)
        {
            throw new ArgumentNullException("iv");
        }

        string result = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(text))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        result = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return result;
    }

    [RuntimeInitializeOnLoadMethod]
    private static void InitKeyAndIV()
    {
        string keyStr = PlayerPrefs.GetString("ENC_Key");
        string ivStr = PlayerPrefs.GetString("ENC_IV");

        if (string.IsNullOrEmpty(keyStr))
        {
            keyStr = GenerateKeyString();
            PlayerPrefs.SetString("ENC_Key", keyStr);
        }

        if(string.IsNullOrEmpty(ivStr))
        {
            ivStr = GenerateIVString();
            PlayerPrefs.SetString("ENC_IV", ivStr);
        }

        key = Convert.FromBase64String(keyStr);
        iv = Convert.FromBase64String(ivStr);
    }

    private static string GenerateKeyString()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            return Convert.ToBase64String(aesAlg.Key);
        }
    }

    private static string GenerateIVString()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateIV();
            return Convert.ToBase64String(aesAlg.IV);
        }
    }
}

