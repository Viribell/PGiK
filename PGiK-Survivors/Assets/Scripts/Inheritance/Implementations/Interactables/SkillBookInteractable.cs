using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBookInteractable : Interactable {
    protected override void Interact() {
        if ( !CanInteract() ) return;
        
        if( SkillControl.Instance.availableSkills.Count <= 0 ) {
            GameResources.Instance.AddGold( 15 * LevelControl.Instance.levelStadium );
        } else {
            SkillBookUI.Instance.Activate( RefCacheControl.Instance.Player );
        }

        Destroy( gameObject );
    }

    protected override bool CanInteract() {
        return true;
    }
}
