using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour {
    private void OnCollisionEnter2D( Collision2D collision ) {
        //if Attack then do destruction

        if ( collision.gameObject.TryGetComponent( out Player player ) ) { //to be changed for checking Attack
            DestructAction();
            Destroy( gameObject );
        }
    }

    protected virtual void DestructAction() { }
}
