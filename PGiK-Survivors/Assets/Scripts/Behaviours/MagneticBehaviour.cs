using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticBehaviour : MonoBehaviour {
    [field: SerializeField] public float GravitySpeed { get; private set; }

    [field: SerializeField] public bool isMagnetOn = false;
    [field: SerializeField] public Transform moveTarget;

    private void Start() {
        if ( GravitySpeed == default ) GravitySpeed = 10.0f;
    }

    private void FixedUpdate() {
        if ( PauseControl.IsGamePaused ) { return; }

        if ( isMagnetOn ) {
            Gravitate( moveTarget );
        }
    }

    public void TurnOn( Transform moveTarget, float gravitySpeed = 10.0f ) {
        this.moveTarget = moveTarget;
        isMagnetOn = true;
        GravitySpeed = 10.0f;
    }

    public void TurnOff() {
        isMagnetOn = false;
    }

    public void Gravitate( Transform moveTarget ) {
        Vector2 dir = ( moveTarget.position - transform.position ).normalized;

        transform.Translate( dir * GravitySpeed * Time.fixedDeltaTime );
    }

    public void Gravitate( Transform moveTarget, float gravitySpeed ) {
        Vector2 dir = ( moveTarget.position - transform.position ).normalized;

        transform.Translate( dir * gravitySpeed * Time.fixedDeltaTime );
    }
}
