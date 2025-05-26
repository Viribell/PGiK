using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTest : Interactable {
    [SerializeField] private UnityEvent OnInteract;
    [field: SerializeField] private StatusEffectSO effect;
    [field: SerializeField] private ResourceSO resource;
    
    public void TestApply(GameObject target) {
        effect.Apply( target );
    }

    public void TestAddResource() {
        GameResources.Instance.AddToResource( resource, 100 );
    }

    protected override void Interact() {
        if ( !CanInteract() ) return;

        OnInteract?.Invoke();
    }

    protected override bool CanInteract() {
        return true;
    }
}
