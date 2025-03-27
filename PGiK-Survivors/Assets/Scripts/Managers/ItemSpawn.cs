using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour{

    public static void TestDrop( Resource resource ) {
        Instantiate( resource.Prefab, new Vector2(10, 0), resource.Prefab.rotation );
    }

    public static void SpawnResource(Resource resource, Vector2 pos) {
        if ( resource.Prefab == null ) { Debug.Log("Empty resource prefab!"); return; }
        Instantiate( resource.Prefab, pos, resource.Prefab.rotation );
    }
}
