using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDesctructible : Destructible {
    [SerializeField] private DestructibleSO destructData;
    [SerializeField] private DropBehaviour dropBehaviour;

    private void Awake() {
        if ( destructData != null ) Init();
    }

    protected override void DestructAction() {
        Drop();
    }

    private void Drop() {
        if ( dropBehaviour == null ) { Debug.Log( "No drop behaviour on basicDestructible!" ); return; }

        dropBehaviour?.Drop( transform.position );
    }

    private void Init() {
        if ( destructData.sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = destructData.sprite;
        }
    }

    public void Init( DestructibleSO data ) {
        destructData = data;

        if ( destructData != null ) Init();
    }
}
