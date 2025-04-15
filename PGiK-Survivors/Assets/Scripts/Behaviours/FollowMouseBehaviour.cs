using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseBehaviour : MonoBehaviour {
    [field: SerializeField] private bool useAxisDirectionChange = true;

    private void OnEnable() {
        GameInput.Instance.myInputActions.Player.MouseMove.performed += OnMouseMove;
    }

    private void OnDisable() {
        GameInput.Instance.myInputActions.Player.MouseMove.performed -= OnMouseMove;
    }

    private void OnMouseMove( UnityEngine.InputSystem.InputAction.CallbackContext obj ) {
        if ( PauseControl.IsGamePaused ) return;

        if ( useAxisDirectionChange ) ChangeAxisDirection();
        else ChangeRotation();
    }

    //simple tho jumpy
    private void ChangeAxisDirection() {
        transform.right = ( GameInput.Instance.GetPointerPosition() - (Vector2)transform.position ).normalized;
    }

    //more precise and less jumpy when mouse close to player
    private void ChangeRotation() {
        Vector2 dir = ( GameInput.Instance.GetPointerPosition() - ( Vector2 )transform.position ).normalized;

        float rotation_z = Mathf.Atan2( dir.y, dir.x ) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler( 0f, 0f, rotation_z );
    }

}
