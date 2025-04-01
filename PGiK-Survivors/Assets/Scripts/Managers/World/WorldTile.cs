using UnityEngine;

public class WorldTile : MonoBehaviour {

    private void Start() {
        GetComponentInParent<WorldScroll>().Add( gameObject );
    }

}
