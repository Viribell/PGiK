using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour {
    public static void SpawnResource( ResourceSO resource, Vector2 pos) {
        if ( resource.Prefab == null ) { Debug.Log("Empty resource prefab!"); return; }
        Instantiate( resource.Prefab, pos, resource.Prefab.rotation );
    }

    public static void TestRescue( NPCRescueSO rescue ) {
        if ( rescue.prefab == null ) { Debug.Log( "No rescue prefab given!" ); return; }
        Instantiate( rescue.prefab, new Vector2( 7.86f , 0.0f ), rescue.prefab.rotation );
    }

    public static void SpawnNPCRescue( NPCRescueSO rescue, Vector2 pos ) {
        if ( rescue.prefab == null ) { Debug.Log( "No rescue prefab given!" ); return; }
        Instantiate( rescue.prefab, pos, rescue.prefab.rotation );
    }

    public static void SpawnExpPoint( ExpPointSO expPoint, Vector2 pos ) {
        if ( expPoint.prefab == null ) { Debug.Log( "No expPoint prefab given!" ); return; }
        Instantiate( expPoint.prefab, pos, expPoint.prefab.transform.rotation );
    }
}
