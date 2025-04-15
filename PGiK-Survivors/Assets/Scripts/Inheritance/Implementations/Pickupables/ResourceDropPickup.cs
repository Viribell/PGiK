using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDropPickup : Pickupable {

    [SerializeField] public ResourceSO drop;
    [SerializeField] public int amount;

    private void Awake() {
        if( drop != null ) Init();
    }

    protected override void Interact( PlayerController player ) {
        GameResources.Instance.AddToResource( drop, amount ); //prolly to be changed for manager of resources gained in the Level, to get them after level completion
    }

    protected override bool CanInteract() {
        return true;
    }

    private void Init() {
        if ( drop.sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = drop.sprite;
        }
    }

    public void Init(ResourceSO resource) {
        drop = resource;

        if ( drop != null ) Init();
    }

    public void Init( ResourceSO resource, int amount ) {
        drop = resource;
        this.amount = amount;

        if ( drop != null ) Init();
    }
}
