using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour, IPersistentData {
    public static GameResources Instance { get; private set; }

    [field: SerializeField] public SerializableDictionary<ResourceSO, int> Resources { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; }

    }

    private void OnDisable() { Instance = null; }

    public int GetCount( ResourceSO resource ) {
        if ( Resources.TryGetValue( resource, out int count ) ) return count;
        else return 0;
    }

    public void AddToResource( ResourceSO resource, int count) {         
        Resources[resource] += count; 
    }
    
    public void SubtractFromResource( ResourceSO resource, int count) {
        if ( Resources[resource] - count < 0 ) return;

        Resources[resource] -= count;
    }

    public void LoadData( SaveData data ) {
        List<ResourceSaveData> resourcesData = data.resources;

        if ( resourcesData == null || resourcesData.Count == 0 ) return;

        foreach( ResourceSaveData resourceData in resourcesData ) {
            if ( Resources.ContainsKey( resourceData.resource ) ) Resources[resourceData.resource] = resourceData.amount;
        }
    }

    public void SaveData( ref SaveData data ) {
        data.resources.Clear();

        foreach ( KeyValuePair<ResourceSO, int> entry in Resources) {
            ResourceSaveData resourceData = new ResourceSaveData { resource = entry.Key, amount = entry.Value };
            data.resources.Add( resourceData );
        }
    }
}