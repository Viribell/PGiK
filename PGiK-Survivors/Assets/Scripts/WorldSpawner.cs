using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpawner : MonoBehaviour {
    [field: SerializeField] private DestructibleSO[] destructsData;

    [field: SerializeField] private GameObject spawnedObject;

    public void UpdateSelf() {
        bool isAlive = spawnedObject != null;

        if ( !isAlive ) Spawn();
    }

    public void Spawn() {
        if ( destructsData == null || destructsData.Length <= 0 ) return;

        int rand = Random.Range( 0, destructsData.Length );

        spawnedObject = SpawnControl.Instance.SpawnReadyBasicDestruct( destructsData[rand], transform.position );
        spawnedObject.transform.SetParent( transform );
    }

    public void LoadData( DestructibleSO[] data ) {
        destructsData = data;
    }
}
