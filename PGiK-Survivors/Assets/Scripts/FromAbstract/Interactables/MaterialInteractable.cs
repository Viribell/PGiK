using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInteractable : Interactable {
    [SerializeField] private ResourceSO material;
    [SerializeField] public int amount;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject depletedVisual;

    private bool isHarvested = false;

    private void Awake() {
        if ( material != null ) Init();
    }

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

    private void Init() {
        if ( material.defaultVisual != null ) { visual.GetComponent<SpriteRenderer>().sprite = material.defaultVisual; }
        if ( material.depletedVisual != null ) { depletedVisual.GetComponent<SpriteRenderer>().sprite = material.depletedVisual; }
    }

    public void Init( ResourceSO resource ) {
        material = resource;

        if ( material != null ) Init();
    }

    public void Init( ResourceSO resource, int amount ) {
        material = resource;
        this.amount = amount;

        if ( material != null ) Init();
    }
}
