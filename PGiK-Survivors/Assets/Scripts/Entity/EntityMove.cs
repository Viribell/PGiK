using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMove : MonoBehaviour, IEntityComponent {
    private Rigidbody2D rb;
    private Vector2 moveVector;

    private EntityController entityController;

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
        moveVector = entityController.GetMoveVector();

        rb.velocity = moveVector * entityController.EntityStats.GetStatTotal( StatType.Speed );
    }

    public void LoadEntityController( EntityController controller ) {
        entityController = controller;
    }
}
