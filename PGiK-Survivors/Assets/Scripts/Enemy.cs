using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private Transform moveTarget;
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        MoveToTarget();
    }

    private void MoveToTarget() {
        Vector2 dir = ( moveTarget.position - transform.position ).normalized;

        rb.velocity = dir * speed;
    }
}
