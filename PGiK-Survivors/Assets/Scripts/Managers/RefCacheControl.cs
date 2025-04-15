using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEditor.U2D.Aseprite;
using UnityEditor.UIElements;
using UnityEngine;

public class RefCacheControl : MonoBehaviour {
    public static RefCacheControl Instance { get; private set; }

    [field: SerializeField] public GameObject Player { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; } 
        else {
            Debug.Log( "There is more than one instance of RefCacheControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        Player = GameObject.FindWithTag("Player");
    }

}
