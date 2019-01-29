using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class PlayerDataManager : PersistentSingleton<PlayerDataManager>
{
    public delegate void OnDataMounted();

    private const string SAVE_PATH = "PlayerData.sav";

    public static string GetFilePath()
    {
        return string.Format("{0}/{1}", Application.persistentDataPath, SAVE_PATH);
    }

    private static readonly byte[] SALT = new byte[]
    {
        0x48, 0x61, 0x6D, 0x69, 0x6C, 0x74, 0x6F, 0x6E,0x48, 0x61, 0x6D, 0x69, 0x6C, 0x74, 0x6F, 0x6E,
        0x48, 0x61, 0x6D, 0x69, 0x6C, 0x74, 0x6F, 0x6E,0x48, 0x61, 0x6D, 0x69, 0x6C, 0x74, 0x6F, 0x6E
    };

    private static byte[] GetUniqueDeviceBytes()
    {
        byte[] deviceIdentifier = Encoding.ASCII.GetBytes(SystemInfo.deviceUniqueIdentifier);
        return deviceIdentifier;
    }

    [NonSerialized] private PersistentData m_Data;
    public OnDataMounted onDataMounted;

    private int m_CurrentSaveFileIndex = -1; 

    public int CurrentSaveFileIndex
    {
        get { return m_CurrentSaveFileIndex; }
    }

    public PersistentData PlayerData
    {
        get { return m_Data; }
    }

    public bool dataLoaded
    {
        get { return m_Data != null; }
    }

    private IEnumerator Start()
    {
        m_Data = new PersistentData();

        if (File.Exists(GetFilePath()))
        {
            LoadData();
        }
        SaveData();
        yield return null;
    }

    public void SaveData()
    {
        Debug.Log("Saving data to: " + GetFilePath());

        string dataString = JsonUtility.ToJson(m_Data, true);
        using (StreamWriter writer = GetWriteStream())
        {
            writer.Write(dataString);
        }
    }

    public void LoadData()
    {
        Debug.Log("Save File Found. Attempting to parse...");
        using (StreamReader reader = GetReadStream())
        {
            JsonUtility.FromJsonOverwrite(reader.ReadToEnd(), m_Data);
        }
        if (onDataMounted != null)
            onDataMounted();
        Debug.Log("Parse Successful. File has been loaded into game.");
    }

    public void DeleteData()
    {
        if (File.Exists(GetFilePath()))
        {
            File.Delete(GetFilePath());
            Debug.Log("Save file found and deleted");
        }
        else Debug.Log("Save file does not exist");
    }

    public static StreamWriter GetWriteStream()
    {
        FileStream underlyingStream;
        CryptoStream encryptedStream;

        underlyingStream = new FileStream(GetFilePath(), FileMode.Create);

        Rfc2898DeriveBytes byteGenerator = new Rfc2898DeriveBytes(GetUniqueDeviceBytes(), SALT, 1000);
        RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        byte[] key = byteGenerator.GetBytes(32);
        byte[] iv = new byte[16];
        random.GetBytes(iv);

        Rijndael ri = Rijndael.Create();
        ri.Key = key;
        ri.IV = iv;

        underlyingStream.Write(iv, 0, 16);
        encryptedStream = new CryptoStream(underlyingStream,ri.CreateEncryptor(),CryptoStreamMode.Write);

        return new StreamWriter(encryptedStream);
    }

    public static StreamReader GetReadStream()
    {
        FileStream underlyingStream;
        CryptoStream encryptedStream;

        underlyingStream = new FileStream(GetFilePath(), FileMode.Open);

        Rfc2898DeriveBytes byteGenerator = new Rfc2898DeriveBytes(GetUniqueDeviceBytes(), SALT, 1000);
        RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        byte[] key = byteGenerator.GetBytes(32);
        byte[] iv = new byte[16];
        random.GetBytes(iv);

        underlyingStream.Read(iv,0,16);

        Rijndael ri = Rijndael.Create();
        ri.Key = key;
        ri.IV = iv;

        encryptedStream = new CryptoStream(underlyingStream, ri.CreateDecryptor(), CryptoStreamMode.Read);

        return new StreamReader(encryptedStream);
    }

    public void SetCurrentSaveFileIndex(int index)
    {
        m_CurrentSaveFileIndex = index;
    }
}