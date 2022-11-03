using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
    private static readonly string fileName = "/player.kj";

    public static void Save(GameData data)
    {
        //Set file path
        string path = Application.persistentDataPath + fileName;

        //Save off the data to that path
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = File.Create(path))
        {
            formatter.Serialize(stream, data);
        }
    }

    public static GameData Load(bool displayLoadErrorLog = true)
    {
        GameData result = new GameData();

        //Set file path
        string path = Application.persistentDataPath + fileName;

        //Return if the file to load does not exist
        if (!File.Exists(path))
        {
            if (displayLoadErrorLog)
                Debug.LogError("Save file not found in " + path);

            return result;
        }

        //Load the data from that path
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            result = (GameData)formatter.Deserialize(stream);
            return result;
        }
    }
}
