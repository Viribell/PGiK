using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour, IPersistentData {
    public static GameResources Instance { get; private set; }

    [field: SerializeField] public SerializableDictionary<ResourceSO, int> Resources { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; }

    }

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

        List<ResourceSO> tempKeys = new List<ResourceSO>();

        foreach(ResourceSO key in Resources.Keys) {
            tempKeys.Add( key );
        }

        foreach(ResourceSO key in tempKeys) {
            ResourceSaveData resourceData = resourcesData.Find( item => string.Equals( item.resource, key.itemName ) );

            if ( resourceData != null ) Resources[key] = resourceData.amount;
        }

        tempKeys.Clear();
    }

    public void SaveData( SaveData data ) {
        data.resources.Clear();

        foreach ( KeyValuePair<ResourceSO, int> entry in Resources) {
            ResourceSaveData resourceData = new ResourceSaveData { resource = entry.Key.itemName, amount = entry.Value };
            data.resources.Add( resourceData );
        }
    }
}