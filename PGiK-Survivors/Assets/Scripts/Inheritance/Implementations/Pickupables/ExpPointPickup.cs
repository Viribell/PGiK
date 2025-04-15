using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPointPickup : Pickupable {
    [SerializeField] private ExpPointSO expData;

    private void Awake() {
        if ( expData != null ) Init();
    }

    protected override void Interact( PlayerController player ) {
        player.PlayerLevel.GainXP( expData.amount );
    }

    protected override bool CanInteract() {
        return true;
    }

    private void Init() {
        if ( expData.sprite != null ) {
            SpriteRenderer spriteRenderer = gameObject.transform.Find( "Visual" ).GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = expData.sprite;
        }
    }

    public void Init( ExpPointSO exp ) {
        expData = exp;

        if ( expData != null ) Init();
    }
}
