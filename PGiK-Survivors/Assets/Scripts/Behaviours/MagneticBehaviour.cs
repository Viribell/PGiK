using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBehaviour : MonoBehaviour {
    [field: SerializeField] public float GravitySpeed { get; private set; }

    private void Start() {
        if ( GravitySpeed == default ) GravitySpeed = 10.0f;
    }

    public void Gravitate() {
        Vector2 dir = ( PlayerControl.Instance.Player.transform.position - transform.position ).normalized;

        transform.Translate( dir * GravitySpeed * Time.fixedDeltaTime );
    }

    public void Gravitate( float gravitySpeed ) {
        Vector2 dir = ( PlayerControl.Instance.Player.transform.position - transform.position ).normalized;

        transform.Translate( dir * gravitySpeed * Time.fixedDeltaTime );
    }
}
