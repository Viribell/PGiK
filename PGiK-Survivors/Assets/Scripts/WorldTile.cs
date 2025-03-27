using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour {

    private void Start() {
        GetComponentInParent<WorldScroll>().Add( gameObject );
    }

}
