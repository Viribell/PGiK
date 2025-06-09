using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookInteractable : Interactable {
    protected override void Interact() {
        if ( !CanInteract() ) return;

        SkillBookUI.Instance.Activate( RefCacheControl.Instance.Player );

        Destroy( gameObject );
    }

    protected override bool CanInteract() {
        return true;
    }
}
