using UnityEngine;

public class InteractTest : Interactable {
    protected override void Interact() {
        if ( !CanInteract() ) return;
        Debug.Log( "Interacted with " + gameObject.name );
    }

    protected override bool CanInteract() {
        return true;
    }
}
