using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class SaveToFile {
    private string saveDir = "";
    private string saveName = "";

    public bool isJsonFormatted = true;

    public SaveToFile() {
        saveDir = Application.persistentDataPath;
        saveName = "saveData.json";
    }

    public SaveToFile( string saveName ) {
        saveDir = Application.persistentDataPath;
        
        this.saveName = saveName;
    }

    public SaveToFile(string saveDir, string saveName) {
        this.saveDir = saveDir;
        this.saveName = saveName;
    }

    public SaveData Load() {
        string fullPath = Path.Combine( saveDir, saveName );
        SaveData loadedData = null;

        if( File.Exists(fullPath) ) {
            try {
                string jsonData = File.ReadAllText( fullPath );

                loadedData = JsonUtility.FromJson<SaveData>( jsonData );

            } catch ( Exception e ) {
                Debug.LogError( "Game loading error for path: " + fullPath + "\n" + e );
            }
        }

        return loadedData;
    }

    public void Save( SaveData data ) {
        string fullPath = Path.Combine( saveDir, saveName );

        try {
            Directory.CreateDirectory( Path.GetDirectoryName(fullPath) ); //create if doesnt exist

            string jsonData = JsonUtility.ToJson( data, isJsonFormatted );

            File.WriteAllText( fullPath, jsonData );

        } catch ( Exception e ) {
            Debug.LogError( "Game saving error for path: " + fullPath + "\n" + e );
        }
    }
}
