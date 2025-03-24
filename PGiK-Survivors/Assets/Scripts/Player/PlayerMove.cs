using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5.0f; //chwilowo tutaj, powinno byæ pobierane z jakiejœ klasy Klas?

    private Rigidbody2D rb;
    Vector2 moveVector;


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        moveVector = new Vector2();
    }

    private void Update() {
        Move();
    }



    private void Move() {
        moveVector = GameInput.Instance.GetMovementVectorNormalized();

        rb.velocity = moveVector * moveSpeed;
    }
}
