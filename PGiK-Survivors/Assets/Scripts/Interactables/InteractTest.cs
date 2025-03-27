using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTest : Interactable {
    [SerializeField] private UnityEvent OnInteract;
    
    protected override void Interact() {
        if ( !CanInteract() ) return;

        Debug.Log("Interact");

        OnInteract?.Invoke();
    }

    protected override bool CanInteract() {
        return true;
    }
}
