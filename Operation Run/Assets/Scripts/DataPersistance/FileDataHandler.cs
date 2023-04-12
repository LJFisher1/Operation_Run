using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public  FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath))
        {
            try
            {
                //load the serialized data from the file
                string dataToLoad = "";
                using(FileStream stream = new FileStream (fullPath, FileMode.Open))
                {

                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                //deserialize the data from Json back into the C# object
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file: " + fullPath + "\n" + e);
            }
        }
        Debug.Log(fullPath);
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //crete the directory the file will be written to if it dosnt already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            Debug.Log(data);
            //serialize the c# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);
            Debug.Log(dataToStore);
            //write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    Debug.Log(dataToStore);
                    writer.Write(dataToStore);
                    //writer.Write("true");
                }
                stream.Close();
            }

        }

        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to sava data to file: " + fullPath + "\n" + e);
        }
    }
}