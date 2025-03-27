using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInteractable : Interactable {
    [SerializeField] private Resource material;
    [SerializeField] public int amount;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject depletedVisual;

    private bool isHarvested = false;

    protected override void Interact() {
        if ( !CanInteract() ) return;
        GameResources.Instance.AddToResource( material, amount );

        isHarvested = true;

        SwitchVisual();
    }

    protected override bool CanInteract() {
        return !isHarvested;
    }

    private void SwitchVisual() {
        visual.SetActive( false );
        depletedVisual.SetActive( true );
    }
}
