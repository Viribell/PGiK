using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, IPersistentData {
    public static GameState Instance { get; private set; }

    [field: SerializeField] public SerializableDictionary<NPCSO, bool> availableNPC { get; private set; }

    [field: SerializeField] public int chosenLevelStadium;
    [field: SerializeField] public ClassSO chosenPlayerClass;

    private void Awake() {
        if ( Instance == null ) { Instance = this; } else {
            Debug.Log( "There is more than one instance of GameState. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        DontDestroyOnLoad( gameObject );
    }

    public bool IsAvailable( NPCSO npcData ) {
        if ( availableNPC.ContainsKey( npcData ) ) return availableNPC[npcData];

        return false;
    }

    public void RescueNPC( NPCInteractable npc ) {
        NPCSO npcData = npc.npcData;

        if( availableNPC.ContainsKey(npcData) && !availableNPC[npcData] ) {
            availableNPC[npcData] = true;
            npc.RefreshActive();
        } else {
            Debug.Log( "NPC of name: " + npcData.npcName + " is not missing!" );
        }
    }

    public void RescueNPC( NPCSO npcData ) {
        if ( availableNPC.ContainsKey( npcData ) && !availableNPC[npcData] ) {
            availableNPC[npcData] = true;
        } else {
            Debug.Log( "NPC of name: " + npcData.npcName + " is not missing!" );
        }
    }

    public void LoadData( SaveData data ) {
        List<NPCSaveData> npcDataList = data.availableNPC;

        if ( npcDataList == null || npcDataList.Count == 0 ) return;

        List<NPCSO> tempKeys = new List<NPCSO>();

        foreach ( NPCSO key in availableNPC.Keys ) {
            tempKeys.Add( key );
        }

        foreach ( NPCSO key in tempKeys ) {
            NPCSaveData npcSaveData = npcDataList.Find( item => string.Equals( item.npc, key.npcName ) );

            if ( npcSaveData != null ) availableNPC[key] = npcSaveData.isAvailable;
        }

        tempKeys.Clear();
    }

    public void SaveData( SaveData data ) {
        if ( this != GameState.Instance ) {
            Debug.Log( "Wrong instance saving!" );
            return;
        }

        data.availableNPC.Clear();

        foreach ( KeyValuePair<NPCSO, bool> entry in availableNPC ) {
            NPCSaveData npcSaveData = new NPCSaveData { npc = entry.Key.npcName, isAvailable = entry.Value };
            data.availableNPC.Add( npcSaveData );
        }
    }
}
