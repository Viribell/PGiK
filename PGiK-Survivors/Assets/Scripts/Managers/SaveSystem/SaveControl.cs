using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveControl : MonoBehaviour {
    public static SaveControl Instance { get; private set; }

    [Header( "Save Config" )]
    [SerializeField] private string saveName;
    [SerializeField] private bool isJsonFormatted;

    private SaveData saveData;
    private List<IPersistentData> persistentDataObjects;
    private SaveToFile dataSaveHandler;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } 
        else Debug.LogError("There is more than one instance of SaveControl!");
    }

    private void Start() {
        dataSaveHandler = new SaveToFile( saveName );
        dataSaveHandler.isJsonFormatted = isJsonFormatted;

        persistentDataObjects = FindAllPeristentData();
        
        LoadGame();
    }

    public void NewGame() {
        this.saveData = new SaveData();
    }

    public void LoadGame() {
        saveData = dataSaveHandler.Load();
        
        if( saveData == null ) {
            Debug.Log("No previous data. Creating new save.");
            NewGame();
        }

        foreach( IPersistentData persistentData in persistentDataObjects ) {
            persistentData.LoadData( saveData );
        }
    }

    public void SaveGame() {
        foreach ( IPersistentData persistentData in persistentDataObjects ) {
            persistentData.SaveData( ref saveData );
        }

        dataSaveHandler.Save( saveData );
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    private List<IPersistentData> FindAllPeristentData() {
        IEnumerable<IPersistentData> peristentObjects = FindObjectsOfType<MonoBehaviour>().OfType<IPersistentData>();

        return new List<IPersistentData>( peristentObjects );
    }
}
