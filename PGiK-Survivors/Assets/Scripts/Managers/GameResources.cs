using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour {
    public static GameResources Instance { get; private set; }

    [field: SerializeField] public SerializableDictionary<Resource, int> Resources { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; }

    }

    private void OnDisable() { Instance = null; }

    public int GetCount( Resource resource ) {
        if ( Resources.TryGetValue( resource, out int count ) ) return count;
        else return 0;
    }

    public void AddToResource(Resource resource, int count) {         
        Resources[resource] += count; 
    }
    
    public void SubtractFromResource(Resource resource, int count) {
        if ( Resources[resource] - count < 0 ) return;

        Resources[resource] -= count;
    }
}