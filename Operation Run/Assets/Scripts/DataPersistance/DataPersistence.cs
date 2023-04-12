using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistence : MonoBehaviour
{
    [Header ("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List <iDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistence instance {get; private set;}

    private void Awake()
    {
        if(instance!=null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the Scene.");
        }

        instance=this;
    }

    private void Start()
    {
        this.dataHandler=new FileDataHandler(Application.persistentDataPath,fileName);
        this.dataPersistenceObjects= FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData=new GameData();
    }

    public void LoadGame()
    {
        //Load any saved data from a file using the data handler
        this.gameData=dataHandler.Load();

        //if no data can be loaded, initialize to a new game
        if(this.gameData==null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (iDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
         foreach (iDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }

        Debug.Log("Saved Score Count = " +gameData.PlayerScore);
        
        //save that data to a file using the data handler
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
       
    }

    private List<iDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<iDataPersistence>dataPersistenceObjects=
        FindObjectsOfType <MonoBehaviour>().OfType<iDataPersistence>();

        return new List<iDataPersistence>(dataPersistenceObjects);
    }

    public void CheckIfLevelTrue()
    {

    }

}   


