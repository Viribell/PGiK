using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour {
    
    private void OnTriggerEnter2D( Collider2D collision ) {
        if ( !CanInteract() ) return;

        if ( collision.TryGetComponent( out Player player ) ) {
            Interact();
            Destroy( gameObject );
        }
    }

    protected virtual void Interact() { }
    protected virtual bool CanInteract() { return true; }
}
