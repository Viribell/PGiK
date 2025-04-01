using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPointPickup : Pickupable {
    [SerializeField] private ExpPointSO expData;

    private void Awake() {
        if ( expData.sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = expData.sprite;
        }
    }

    protected override void Interact() {
        PlayerControl.Instance.PlayerLevel.GainXP( expData.amount );
    }

    protected override bool CanInteract() {
        return true;
    }
}
