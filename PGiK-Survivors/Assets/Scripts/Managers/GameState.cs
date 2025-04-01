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

        foreach ( NPCSaveData npcData in npcDataList ) {
            if ( availableNPC.ContainsKey( npcData.npc ) ) availableNPC[npcData.npc] = npcData.isAvailable;
        }
    }

    public void SaveData( ref SaveData data ) {
        data.availableNPC.Clear();

        foreach ( KeyValuePair<NPCSO, bool> entry in availableNPC ) {
            NPCSaveData npcSaveData = new NPCSaveData { npc = entry.Key, isAvailable = entry.Value };
            data.availableNPC.Add( npcSaveData );
        }
    }
}
