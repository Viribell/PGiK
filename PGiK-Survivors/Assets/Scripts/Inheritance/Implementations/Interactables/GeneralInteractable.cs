using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralInteractable : Interactable {
    [field: SerializeField] private UnityEvent OnInteract;

    protected override void Interact() {
        if ( !CanInteract() ) return;

        OnInteract?.Invoke();
    }

    protected override bool CanInteract() {
        return true;
    }
}
