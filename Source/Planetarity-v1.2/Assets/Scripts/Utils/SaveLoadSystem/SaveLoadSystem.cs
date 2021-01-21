using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    public static string DataFileName = "/planetarity.data";

    public static bool CheckForDataFile()
    {
        string path = Application.persistentDataPath + DataFileName;
        return File.Exists(path);
    }

    public static void SaveData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + DataFileName;

        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData();

        try
        {
            formatter.Serialize(stream, data);
            Debug.Log("Saved successfuly!");
        }
        finally 
        {
            stream.Close();
        }
    }

    public static GameData LoadData()
    {
        string path = Application.persistentDataPath + DataFileName;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = null;

            try
            {
                data = formatter.Deserialize(stream) as GameData;
                Debug.Log("Loaded successfuly!");
            }
            finally
            {
                stream.Close();
            }

            return data;
        }
        else
        {
            Debug.LogError("Save file is not exist in path: " + path);
            return null;
        }

    }
}
