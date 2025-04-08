using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablePickup : Pickupable {
    [SerializeField] private ConsumableSO consumableData;

    private void Awake() {
        if ( consumableData != null ) Init();
    }

    protected override void Interact() {
        PlayerControl.Instance.PlayerHealth.Heal( consumableData.useAmount );
    }

    protected override bool CanInteract() { return true; }

    private void Init() {
        if ( consumableData.sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = consumableData.sprite;
        }
    }

    public void Init( ConsumableSO data ) {
        consumableData = data;

        if ( consumableData != null ) Init();
    }
}
