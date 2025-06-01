using UnityEngine;

public class RefCacheControl : MonoBehaviour {
    public static RefCacheControl Instance { get; private set; }

    [field: SerializeField] public PlayerController Player { get; private set; }
    [field: SerializeField] public GameObject CompassUI { get; private set; }

    private void Awake() {
        if ( Instance == null ) { Instance = this; } 
        else {
            Debug.Log( "There is more than one instance of RefCacheControl. Destroying the new one." );
            Destroy( gameObject );
            return;
        }

        Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        CompassUI = GameObject.Find( "CompassUI" );
    }
}
