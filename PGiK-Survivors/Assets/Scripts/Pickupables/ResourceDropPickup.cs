using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDropPickup : Pickupable {

    [SerializeField] private Resource bossDrop;
    [SerializeField] public int amount;

    private void Awake() {
        if( bossDrop.Sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = bossDrop.Sprite;
        }  
    }

    protected override void Interact() {
        GameResources.Instance.AddToResource( bossDrop, amount );
        Destroy( gameObject );
    }

    protected override bool CanInteract() {
        return true;
    }

}
