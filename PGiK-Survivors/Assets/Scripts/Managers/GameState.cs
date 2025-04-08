using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class GameState : MonoBehaviour, IPersistentData {
    public static GameState Instance { get; private set; }

    [field: SerializeField] public SerializableDictionary<NPCSO, bool> availableNPC { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; }
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
        data.availableNPC.Clear();

        foreach ( KeyValuePair<NPCSO, bool> entry in availableNPC ) {
            NPCSaveData npcSaveData = new NPCSaveData { npc = entry.Key.npcName, isAvailable = entry.Value };
            data.availableNPC.Add( npcSaveData );
        }
    }
}
