using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform followAfter;

    private void LateUpdate() {
        if( followAfter != null ) Follow();
    }

    private void Follow() {
        Vector2 pos = new Vector2( followAfter.position.x, followAfter.position.y );
        transform.position = pos;
    }
}
