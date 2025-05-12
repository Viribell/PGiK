using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveControl : MonoBehaviour {
    public static SaveControl Instance { get; private set; }

    [Header( "Debug Config" )]
    [SerializeField] private bool createDataIfNull = false;

    [Header( "Save Config" )]
    [SerializeField] private string saveName;
    [SerializeField] private bool formatJson;

    [Header( "Data Config" )]
    [SerializeField] private bool findDisabledObjects = true;

    [Header( "Auto Saving Config" )]
    [SerializeField] private float secondsBetweenAutoSave = 60.0f;
    [SerializeField] private bool enableAutoSave = true;

    private SaveData saveData;
    private List<IPersistentData> persistentDataObjects;
    private SaveToFile dataSaveHandler;

    private Coroutine autoSaveCoroutine;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } 
        else { 
            Debug.LogWarning( "There is more than one instance of SaveControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );

        dataSaveHandler = new SaveToFile( saveName );
        dataSaveHandler.isJsonFormatted = formatJson;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded( Scene scene, LoadSceneMode mode ) {
        persistentDataObjects = FindAllPeristentData();

        LoadGame();

        StartAutoSave();
    }

    private void StartAutoSave() {
        if ( !enableAutoSave ) return;

        if ( autoSaveCoroutine != null ) StopCoroutine( autoSaveCoroutine );

        autoSaveCoroutine = StartCoroutine( AutoSave() );
    }

    private void StopAutoSave() {
        if ( enableAutoSave ) return;

        if ( autoSaveCoroutine != null ) {
            StopCoroutine( autoSaveCoroutine );
            autoSaveCoroutine = null;
        }
    }

    public void NewGame() {
        saveData = new SaveData();
    }

    public bool LoadGame() {
        saveData = dataSaveHandler.Load();

        if( saveData == null && createDataIfNull ) {
            NewGame();
        }
        
        if( !HasSaveData() ) {
            Debug.Log("No previous data. Cannot load. Create a new game!");
            return false;
        }

        foreach( IPersistentData persistentData in persistentDataObjects ) {
            persistentData.LoadData( saveData );
        }

        return true;
    }

    public bool SaveGame() {
        if( !HasSaveData() ) {
            Debug.LogWarning( "No data. Cannot Save. Create a new game!" );
            return false;
        }


        foreach ( IPersistentData persistentData in persistentDataObjects ) {
            persistentData.SaveData( saveData );
        }

        dataSaveHandler.Save( saveData );

        return true;
    }

    private List<IPersistentData> FindAllPeristentData() {
        IEnumerable<IPersistentData> peristentObjects = FindObjectsOfType<MonoBehaviour>( findDisabledObjects ).OfType<IPersistentData>();

        return new List<IPersistentData>( peristentObjects );
    }

    private IEnumerator AutoSave() {
        while( true ) {
            yield return new WaitForSeconds( secondsBetweenAutoSave );

            SaveGame();

            Debug.Log("Auto Saved.");

        }
    }

    private void OnApplicationQuit() {
        SaveGame();
    }

    public SaveData GetSaveData() { return saveData; }
    public bool HasSaveData() { return saveData != null; }
    public void SetEnableAutoSave(bool value) { enableAutoSave = value; StopAutoSave(); StartAutoSave(); }
    
    public void SetTimeBetweenAutoSaves(float seconds) {
        if ( seconds <= 0.0f ) seconds = 60.0f;

        secondsBetweenAutoSave = seconds;
    }
}
