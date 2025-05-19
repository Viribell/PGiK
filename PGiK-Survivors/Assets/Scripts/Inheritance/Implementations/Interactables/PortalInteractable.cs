using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalInteractable : Interactable {
    protected override void Interact() {
        if ( !CanInteract() ) return;

        LevelControl.Instance.EndScreen( EndScreenType.WinScreen );
    }

    protected override bool CanInteract() {
        return true;
    }
}
