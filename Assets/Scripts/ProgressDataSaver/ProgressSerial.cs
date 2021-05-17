using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

static class Constants
{
    public const String fileName = "/ProgressData.dat";
}

public class ProgressSerial
{
    private long rubyToSave;

    public long RubyToSave
    { get; set; }

    private long expToSave;

    public long ExpToSave
    { get; set; }

    public static ProgressSerial getInstance()
    {
        return SingletonHelper.INSTANCE;
    }

    private static class SingletonHelper
    {
        public static readonly ProgressSerial INSTANCE = new ProgressSerial();
    }


    public void saveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + Constants.fileName);
        ProgressData data = new ProgressData();
        data.savedRuby = rubyToSave;
        data.savedExp = expToSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public void loadData()
    {
        if (File.Exists(Application.persistentDataPath + Constants.fileName))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Constants.fileName, FileMode.Open);
            ProgressData data = (ProgressData)bf.Deserialize(file);
            file.Close();
            rubyToSave = data.savedRuby;
            expToSave = data.savedExp;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    public void restartData()
    {
        if (File.Exists(Application.persistentDataPath + Constants.fileName))
        {
            File.Delete(Application.persistentDataPath + Constants.fileName);
            rubyToSave = 0;
            expToSave = 0;
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }

    public bool checkExist()
    {
        return File.Exists(Application.persistentDataPath + Constants.fileName);
    }
}

[Serializable]
class ProgressData
{
    public long savedRuby;
    public long savedExp;
}