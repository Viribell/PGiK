using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookInteractable : Interactable {
    protected override void Interact() {
        if ( !CanInteract() ) return;

        Debug.Log("Skill book");
        
        Destroy( gameObject );
    }

    protected override bool CanInteract() {
        return true;
    }
}
