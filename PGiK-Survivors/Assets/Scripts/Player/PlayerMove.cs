using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5.0f; //chwilowo tutaj, powinno byæ pobierane z jakiejœ klasy Klas?

    private Rigidbody2D rb;
    private Vector2 moveVector;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        moveVector = new Vector2();
    }

    private void Start() {
        UpdateMoveSpeed();
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

        rb.velocity = moveVector * moveSpeed;
    }

    public void UpdateMoveSpeed() {
        moveSpeed = PlayerControl.Instance.Player.GetStatTotal( StatType.Speed );
    }
}
