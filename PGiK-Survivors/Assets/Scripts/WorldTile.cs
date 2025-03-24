using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTile : MonoBehaviour {
    [SerializeField] private Vector2Int tilePos;

    private void Start() {
        GetComponentInParent<WorldScroll>().Add(gameObject, tilePos);
    }

}
