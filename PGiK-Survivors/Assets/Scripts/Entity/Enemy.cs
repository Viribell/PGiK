using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] public EnemySO enemyData;

    [SerializeField] private Transform moveTarget;
    [SerializeField] private float speed;
    [SerializeField] private List<DropBehaviour> dropBehaviours = new List<DropBehaviour>();

    private Dictionary<StatType, Stat> enemyStats;
    private Rigidbody2D rb;

    private void Awake() {
        enemyStats = enemyData?.GetStats();

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if ( PauseControl.IsGamePaused ) {
            rb.velocity = Vector2.zero;
            return;
        }

        MoveToTarget();
    }

    private void MoveToTarget() {
        Vector2 dir = ( moveTarget.position - transform.position ).normalized;

        rb.velocity = dir * speed;
    }

    public void TestDie() {
        Drop();

        //Destroy( gameObject );
    }

    private void Drop() {
        if( dropBehaviours == null || dropBehaviours.Count == 0 ) { Debug.Log("No drop behaviour on enemy!"); return; }

        foreach(DropBehaviour dropBehaviour in dropBehaviours ) {
            dropBehaviour?.Drop( transform.position );
        }
    }
}
