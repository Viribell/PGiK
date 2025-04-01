using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDropPickup : Pickupable {

    [SerializeField] private ResourceSO drop;
    [SerializeField] public int amount;

    private void Awake() {
        if( drop.Sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = drop.Sprite;
        }  
    }

    protected override void Interact() {
        GameResources.Instance.AddToResource( drop, amount );
        Destroy( gameObject );
    }

    protected override bool CanInteract() {
        return true;
    }
}
