using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour {
    
    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( !CanInteract() ) return;

        if ( collision.TryGetComponent( out PlayerController player ) ) {
            Interact( player );
            Destroy( gameObject );
        }
    }

    protected virtual void Interact( PlayerController player ) { }
    protected virtual bool CanInteract() { return true; }
}
