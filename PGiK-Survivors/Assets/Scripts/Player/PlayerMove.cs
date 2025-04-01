using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private Rigidbody2D rb;
    private Vector2 moveVector;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        moveVector = new Vector2();
    }

    private void Update() {
        if( PauseControl.IsGamePaused ) {
            rb.velocity = Vector2.zero;
            return;
        }

        Move();
    }

    private void Move() {
        moveVector = GameInput.Instance.GetMovementVectorNormalized();

        rb.velocity = moveVector * GetTotalSpeed();
    }

    public float GetTotalSpeed() {
        return PlayerControl.Instance.Player.GetStatTotal( StatType.Speed );
    }
}
