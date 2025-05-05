using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntityComponent {
    [SerializeField] private List<DropBehaviour> dropBehaviours = new List<DropBehaviour>();

    private EnemyController enemyController;

    private void Start() {
        if ( enemyController.EntityData != null ) Init();
    }

    public void Die() {
        Drop();

        Destroy( gameObject );
    }

    private void Drop() {
        if( dropBehaviours == null || dropBehaviours.Count == 0 ) { Debug.Log("No drop behaviour on enemy!"); return; }

        foreach(DropBehaviour dropBehaviour in dropBehaviours ) {
            dropBehaviour?.Drop( transform.position );
        }
    }

    private void Init() {
        if ( enemyController.EntityData.sprite != null && enemyController.SpriteRenderer != null ) enemyController.SpriteRenderer.sprite = enemyController.EntityData.sprite;
    }

    public void LoadEntityController( EntityController controller ) {
        enemyController = ( EnemyController )controller;
    }
}
